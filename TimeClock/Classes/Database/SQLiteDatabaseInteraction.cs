﻿using Extensions;
using Extensions.DatabaseHelp;
using Extensions.DataTypeHelpers;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TimeClock.Classes.Entities;

namespace TimeClock.Classes.Database
{
    /// <summary>Represents database interaction covered by SQLite.</summary>
    internal class SQLiteDatabaseInteraction : IDatabaseInteraction
    {
        private readonly string _con = $"Data Source = {_DATABASENAME}; foreign keys = TRUE; Version = 3";

        // ReSharper disable once InconsistentNaming
        private const string _DATABASENAME = "TimeClock.sqlite";

        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        /// <returns>Returns true once the database has been validated</returns>
        public void VerifyDatabaseIntegrity() => Functions.VerifyFileIntegrity(Assembly.GetExecutingAssembly()
            .GetManifestResourceStream($"TimeClock.{_DATABASENAME}"), _DATABASENAME);

        #region Administrator Management

        /// <summary>Changes the Admin password in the database.</summary>
        /// <param name="hashedAdminPassword">New hashed admin password</param>
        /// <returns>Whether the admin password was updated in the database</returns>
        public async Task<bool> ChangeAdminPassword(string hashedAdminPassword)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Admin SET [AdminPassword] = @adminPassword" };
            cmd.Parameters.AddWithValue("@adminPassword", hashedAdminPassword);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Loads the administrator password from the database.</summary>
        /// <returns>Administrator password</returns>
        public async Task<string> LoadAdminPassword()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM Admin");

            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["AdminPassword"].ToString() : "";
        }

        #endregion Administrator Management

        #region Audit

        /// <summary>Creates a database entry for actions taken in modifying data in the database.</summary>
        /// <param name="editor">Who modified the data?</param>
        /// <param name="action">Was the action an update, deletion, etc.?</param>
        /// <param name="originalItem">Original item</param>
        /// <param name="alteredItem">Altered item</param>
        /// <returns></returns>
        public async Task<bool> InsertAudit(string editor, string action, string originalItem, string alteredItem)
        {
            SQLiteCommand cmd = new SQLiteCommand
            {
                CommandText = "INSERT INTO Audit([Editor], [Action], [OriginalItem], [AlteredItem]) VALUES(@editor, @action, @originalItem, @alteredItem)"
            };
            cmd.Parameters.AddWithValue("@editor", editor);
            cmd.Parameters.AddWithValue("@action", action);
            cmd.Parameters.AddWithValue("@originalItem", originalItem);
            cmd.Parameters.AddWithValue("@alteredItem", alteredItem);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        #endregion Audit

        #region Load

        /// <summary>Gets the next User ID autoincrement value in the database for the Users table.</summary>
        /// <returns>Next User ID value</returns>
        public async Task<int> GetNextUserIndex()
        {
            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM SQLITE_SEQUENCE WHERE name = 'Users'");

            if (ds.Tables[0].Rows.Count > 0)
                return Int32Helper.Parse(ds.Tables[0].Rows[0]["seq"]) + 1;
            return 1;
        }

        /// <summary>Loads all Users currently logged in</summary>
        /// <returns>List of all Users currently logged in</returns>
        public async Task<List<Shift>> LoadLoggedInUsers()
        {
            List<Shift> currentlyLoggedIn = new List<Shift>();

            DataSet ds = await SQLite.FillDataSet(_con, "SELECT * FROM LoggedInUsers");
            if (ds.Tables[0].Rows.Count > 0)
            {
                currentlyLoggedIn.AddRange(from DataRow dr in ds.Tables[0].Rows select new Shift(Int32Helper.Parse(dr["ID"]), DateTimeHelper.Parse(dr["TimeIn"].ToString())));
            }
            return currentlyLoggedIn;
        }

        /// <summary>Loads all the selected User's Shifts from the database.</summary>
        /// <param name="userID"></param>
        /// <returns>Returns the list of Shifts</returns>
        public async Task<List<Shift>> LoadShifts(int userID)
        {
            List<Shift> userShifts = new List<Shift>();
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "SELECT * FROM Times WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@id", userID);

            DataSet ds = await SQLite.FillDataSet(_con, cmd);

            if (ds.Tables[0].Rows.Count > 0)
            {
                userShifts.AddRange(from DataRow dr in ds.Tables[0].Rows select new Shift(userID, DateTimeHelper.Parse(dr["TimeIn"]), DateTimeHelper.Parse(dr["TimeOut"])));
            }

            return userShifts.OrderByDescending(shift => shift.ShiftStart).ToList();
        }

        /// <summary>Loads a User from the database.</summary>
        /// <returns>User</returns>
        public async Task<User> LoadUser(string username)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "SELECT * FROM Users WHERE [Username] = @name" };
            cmd.Parameters.AddWithValue("@name", username);
            DataSet ds = await SQLite.FillDataSet(_con, cmd);
            User loadUser = new User();
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                loadUser = new User(Int32Helper.Parse(dr["ID"]), dr["Username"].ToString(), dr["FirstName"].ToString(),
                dr["LastName"].ToString(), dr["Password"].ToString(), BoolHelper.Parse(dr["LoggedIn"]),
                await LoadShifts(Int32Helper.Parse(dr["ID"])));
            }
            return loadUser;
        }

        /// <summary>Loads all Users from the database.</summary>
        /// <param name="loggedIn">Only logged in Users?</param>
        /// <returns>List of all Users</returns>
        public async Task<List<User>> LoadUsers(bool loggedIn = false)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "SELECT * FROM Users" };
            if (loggedIn)
                cmd.CommandText += " WHERE [LoggedIn] = 1";
            DataSet ds = await SQLite.FillDataSet(_con, cmd);

            List<User> allUsers = new List<User>();
            if (ds.Tables[0].Rows.Count > 0)
                foreach (DataRow dr in ds.Tables[0].Rows)
                    allUsers.Add(new User(Int32Helper.Parse(dr["ID"]), dr["Username"].ToString(), dr["FirstName"].ToString(), dr["LastName"].ToString(), dr["Password"].ToString(), BoolHelper.Parse(dr["LoggedIn"]), await LoadShifts(Int32Helper.Parse(dr["ID"]))));
            return allUsers;
        }

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginShift">Shift started by User</param>
        public async Task<bool> LogIn(Shift loginShift)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Times([ID],[TimeIn])VALUES(@id,@timeIn); UPDATE Users SET [LoggedIn] = @loggedIn WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@id", loginShift.ID);
            cmd.Parameters.AddWithValue("@timeIn", loginShift.ShiftStartToString);
            cmd.Parameters.AddWithValue("@loggedIn", 1);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutShift">Shift to be created on logout</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        public async Task<bool> LogOut(Shift logOutShift)
        {
            SQLiteCommand cmd = new SQLiteCommand
            {
                CommandText = "UPDATE Times SET [TimeOut] = @timeOut WHERE [TimeIn] = @timeIn; UPDATE Users SET [LoggedIn] = @loggedIn WHERE [ID] = @id"
            };

            cmd.Parameters.AddWithValue("@timeOut", logOutShift.ShiftEndToString);
            cmd.Parameters.AddWithValue("@timeIn", logOutShift.ShiftStartToString);
            cmd.Parameters.AddWithValue("@loggedIn", 0);
            cmd.Parameters.AddWithValue("@id", logOutShift.ID);
            return await SQLite.ExecuteCommand(_con, cmd);
        }

        #endregion Log In/Out

        #region User Management

        /// <summary>Changes a User's details in the database.</summary>
        /// <param name="oldUser">User whose details needs to be changed</param>
        /// <param name="newUser">User with new details</param>
        /// <returns>True if successfully updated in the database</returns>
        public async Task<bool> ChangeUserDetails(User oldUser, User newUser)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Users SET [Username] = @username, [Password] = @password, [FirstName] = @firstName, [LastName] = @lastName WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@username", newUser.Username);
            cmd.Parameters.AddWithValue("@firstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@lastName", newUser.LastName);
            cmd.Parameters.AddWithValue("@password", newUser.Password);
            cmd.Parameters.AddWithValue("@id", oldUser.ID);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Deletes a User and all their Shifts from the database.</summary>
        /// <param name="user">User to be deleted</param>
        /// <returns>True if successful</returns>
        public async Task<bool> DeleteUser(User user)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "DELETE FROM Users WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@id", user.ID);
            if (user.Shifts.Any())
            {
                foreach (Shift shift in user.Shifts)
                {
                    await InsertAudit("Admin", "Delete Shift",
                        $"Logged in: {shift.ShiftStartToString}, Logged out: {shift.ShiftEndToString}", "[Deleted]");
                }
                cmd.CommandText += ";DELETE FROM Times WHERE [ID] = @id";
            }
            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        public async Task<bool> NewUser(User newUser)
        {
            bool success = false;
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Users([Username], [Password], [FirstName], [LastName], [LoggedIn])VALUES(@id, @password, @firstName, @lastName, @loggedIn)" };
            cmd.Parameters.AddWithValue("@id", newUser.Username);
            cmd.Parameters.AddWithValue("@password", newUser.Password);
            cmd.Parameters.AddWithValue("@firstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@lastName", newUser.LastName);
            cmd.Parameters.AddWithValue("@loggedIn", newUser.LoggedIn);

            if (await SQLite.ExecuteCommand(_con, cmd))
            {
                AppState.DisplayNotification("New user added successfully.", "Time Clock");
                success = true;
            }

            return success;
        }

        #endregion User Management
    }
}