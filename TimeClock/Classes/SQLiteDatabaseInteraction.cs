using Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace TimeClock
{
    internal class SQLiteDatabaseInteraction : IDatabaseInteraction
    {
        private readonly string _con = $"Data Source = {_DATABASENAME};Version=3";

        // ReSharper disable once InconsistentNaming
        private const string _DATABASENAME = "TimeClock.sqlite";

        /// <summary>Verifies that the requested database exists and that its file size is greater than zero. If not, it extracts the embedded database file to the local output folder.</summary>
        /// <returns>Returns true once the database has been validated</returns>
        public void VerifyDatabaseIntegrity()
        {
            Functions.VerifyFileIntegrity(Assembly.GetExecutingAssembly()
                .GetManifestResourceStream($"TimeClock.{_DATABASENAME}"), _DATABASENAME);
        }

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
            DataSet ds = await SQLite.FillDataSet("SELECT * FROM Admin", _con);

            return ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["AdminPassword"].ToString() : "";
        }

        #endregion Administrator Management

        #region Load

        /// <summary>Loads all Users currently logged in</summary>
        /// <returns>List of all Users currently logged in</returns>
        public async Task<List<Shift>> LoadLoggedInUsers()
        {
            List<Shift> currentlyLoggedIn = new List<Shift>();

            DataSet ds = await SQLite.FillDataSet("SELECT * FROM LoggedInUsers", _con);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Shift newShift = new Shift(ds.Tables[0].Rows[i]["ID"].ToString(), DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeIn"].ToString()));

                    currentlyLoggedIn.Add(newShift);
                }
            }
            return currentlyLoggedIn;
        }

        /// <summary>Loads all the selected User's Shifts from the database.</summary>
        /// <param name="user">User whose Shifts are loaded from the database</param>
        /// <returns>Returns the list of Shifts</returns>
        public async Task<List<Shift>> LoadShifts(User user)
        {
            List<Shift> userShifts = new List<Shift>();
            DataSet ds = await SQLite.FillDataSet("SELECT * FROM Times WHERE [ID]='" + user.ID + "'", _con);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    userShifts.Add(new Shift(user.ID, DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeIn"]), DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeOut"])));
            }

            return userShifts;
        }

        /// <summary>Loads all Users from the database.</summary>
        /// <returns>List of all Users</returns>
        public async Task<List<User>> LoadUsers()
        {
            List<User> allUsers = new List<User>();
            DataSet ds = await SQLite.FillDataSet("SELECT * FROM Users", _con);

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    User newUser = new User(ds.Tables[0].Rows[i]["ID"].ToString(), ds.Tables[0].Rows[i]["FirstName"].ToString(), ds.Tables[0].Rows[i]["lastName"].ToString(), ds.Tables[0].Rows[i]["UserPassword"].ToString(), BoolHelper.Parse(ds.Tables[0].Rows[i]["LoggedIn"]));

                    allUsers.Add(newUser);
                }
            }
            return allUsers;
        }

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginUser">User logging in</param>
        public async Task<bool> LogIn(User loginUser)
        {
            bool success = false;
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO LoggedInUsers([ID],[TimeIn])VALUES(@id,@timeIn); UPDATE Users SET[LoggedIn] = @loggedIn WHERE[ID] = @id" };
            cmd.Parameters.AddWithValue("@id", loginUser.ID);
            cmd.Parameters.AddWithValue("@timeIn", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@loggedIn", Int32Helper.Parse(true));

            if (await SQLite.ExecuteCommand(_con, cmd))
            {
                loginUser.LoggedIn = true;
                success = true;
            }
            return success;
        }

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutUser">User logging out</param>
        /// <param name="logOutShift">Shift to be created on logout</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        public async Task<bool> LogOut(User logOutUser, Shift logOutShift)
        {
            bool success = false;
            SQLiteCommand cmd = new SQLiteCommand();

            string sql = "INSERT INTO Times([ID],[TimeIn],[TimeOut])VALUES(@id,@timeIn,@timeOut);DELETE FROM LoggedInUsers WHERE[ID] = @id;UPDATE Users SET [LoggedIn] = @loggedIn WHERE [ID] = @id";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@id", logOutUser.ID);
            cmd.Parameters.AddWithValue("@timeIn", logOutShift.ShiftStart.ToString(CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@timeOut", logOutShift.ShiftEnd.ToString(CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@loggedIn", Int32Helper.Parse(false));
            if (await SQLite.ExecuteCommand(_con, cmd))
            {
                logOutUser.LoggedIn = false;
                success = true;
            }

            return success;
        }

        #endregion Log In/Out

        #region User Management

        /// <summary>Changes a User's password.</summary>
        /// <param name="user">User whose password needs to be changed</param>
        /// <param name="newHashedPassword">New hashed password</param>
        /// <returns>Whether the user's password was updated in the database</returns>
        public async Task<bool> ChangeUserPassword(User user, string newHashedPassword)
        {
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "UPDATE Users SET [UserPassword] = @userPassword WHERE [ID] = @id" };
            cmd.Parameters.AddWithValue("@userPassword", newHashedPassword);
            cmd.Parameters.AddWithValue("@id", user.ID);

            return await SQLite.ExecuteCommand(_con, cmd);
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        /// <returns>Whether a shift was successfully added to the database</returns>
        public async Task<bool> NewUser(User newUser)
        {
            bool success = false;
            SQLiteCommand cmd = new SQLiteCommand { CommandText = "INSERT INTO Users([ID],[UserPassword],[FirstName],[LastName],[LoggedIn])VALUES(@id,@password,@firstName,@lastName,@loggedIn)" };
            cmd.Parameters.AddWithValue("@id", newUser.ID);
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