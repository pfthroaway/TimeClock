using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Globalization;
using System.Threading.Tasks;

namespace TimeClock
{
    internal static class AppState
    {
        // ReSharper disable once InconsistentNaming
        private const string _DBPROVIDERANDSOURCE = "Data Source = TimeClock.sqlite;Version=3";

        private static string _adminPassword;
        internal static User CurrentUser = new User();
        internal static readonly List<Shift> CurrentUserTimes = new List<Shift>();
        internal static readonly List<User> AllUsers = new List<User>();
        internal static readonly List<Shift> CurrentlyLoggedIn = new List<Shift>();

        public static string AdminPassword
        {
            get { return _adminPassword; }
            set { _adminPassword = value; ChangeAdminPassword(); }
        }

        /// <summary>Loads all the selected User's times from the database.</summary>
        /// <param name="user">User whose times are loaded from the database</param>
        /// <returns>True</returns>
        internal static async Task<bool> LoadUserTimes(User user)
        {
            CurrentUserTimes.Clear();
            SQLiteConnection con = new SQLiteConnection();
            SQLiteDataAdapter da;
            DataSet ds = new DataSet();
            con.ConnectionString = _DBPROVIDERANDSOURCE;

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    string sql = "SELECT * FROM Times WHERE [ID]='" + user.ID + "'";
                    string table = "Times";
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            CurrentUserTimes.Add(new Shift(user.ID, DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeIn"]), DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeOut"])));
                    }
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Loading Times", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
            return true;
        }

        /// <summary>Logs in a User.</summary>
        /// <param name="loginUser">User logging in</param>
        internal static async Task<bool> LogIn(User loginUser)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection { ConnectionString = _DBPROVIDERANDSOURCE };

            string sql = "INSERT INTO LoggedInUsers([ID],[TimeIn])VALUES(@id,@timeIn)";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@id", loginUser.ID);
            cmd.Parameters.AddWithValue("@timeIn", DateTime.Now.ToString(CultureInfo.InvariantCulture));
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    CurrentlyLoggedIn.Add(new Shift(loginUser.ID, DateTime.Now));
                    loginUser.LoggedIn = true;
                    AllUsers.Find(user => user.ID == loginUser.ID).LoggedIn = true;

                    sql = "UPDATE Users SET [LoggedIn] = @loggedIn WHERE [ID] = @id";
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@loggedIn", Int32Helper.Parse(loginUser.LoggedIn));
                    cmd.Parameters.AddWithValue("@id", loginUser.ID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Logging In", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });

            return true;
        }

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutUser">User logging out</param>
        internal static async Task<bool> LogOut(User logOutUser)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection { ConnectionString = _DBPROVIDERANDSOURCE };
            DateTime shiftStartTime = CurrentlyLoggedIn.Find(shift => shift.ID == logOutUser.ID).ShiftStart;
            DateTime logOutTime = DateTime.Now;
            string sql = "INSERT INTO Times([ID],[TimeIn],[TimeOut])VALUES(@id,@timeIn,@timeOut)";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@id", logOutUser.ID);
            cmd.Parameters.AddWithValue("@timeIn", shiftStartTime.ToString(CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@timeOut", logOutTime.ToString(CultureInfo.InvariantCulture));
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    cmd.CommandText = "DELETE FROM LoggedInUsers WHERE [ID] = @id";
                    cmd.Parameters.AddWithValue("@id", logOutUser.ID);
                    cmd.ExecuteNonQuery();

                    cmd.Parameters.Clear();
                    CurrentUserTimes.Add(new Shift(logOutUser.ID, shiftStartTime, logOutTime));
                    CurrentlyLoggedIn.Remove(CurrentlyLoggedIn.Find(shift => shift.ID == logOutUser.ID));
                    logOutUser.LoggedIn = false;
                    AllUsers.Find(user => user.ID == logOutUser.ID).LoggedIn = false;
                    cmd.CommandText = "UPDATE Users SET [LoggedIn] = @loggedIn WHERE [ID] = @id";
                    cmd.Parameters.AddWithValue("@loggedIn", Int32Helper.Parse(logOutUser.LoggedIn));
                    cmd.Parameters.AddWithValue("@id", logOutUser.ID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Logging Out", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
            return true;
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        internal static async void NewUser(User newUser)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection { ConnectionString = _DBPROVIDERANDSOURCE };

            string sql = "INSERT INTO Users([ID],[UserPassword],[FirstName],[LastName],[LoggedIn])VALUES(@id,@password,@firstName,@lastName,@loggedIn)";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@id", newUser.ID);
            cmd.Parameters.AddWithValue("@password", PasswordHash.HashPassword(newUser.Password));
            cmd.Parameters.AddWithValue("@firstName", newUser.FirstName);
            cmd.Parameters.AddWithValue("@lastName", newUser.LastName);
            cmd.Parameters.AddWithValue("@loggedIn", newUser.LoggedIn);
            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    AllUsers.Add(newUser);
                    new Notification("New user added successfully.", "Time Clock", NotificationButtons.OK).ShowDialog();
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Creating New User", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
        }

        /// <summary>Loads all required items from the database on application load.</summary>
        internal static async void LoadAll()
        {
            SQLiteConnection con = new SQLiteConnection();
            SQLiteDataAdapter da;
            DataSet ds = new DataSet();
            con.ConnectionString = _DBPROVIDERANDSOURCE;

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    string sql = "SELECT * FROM Admin";
                    string table = "Admin";
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                        AdminPassword = ds.Tables[0].Rows[0]["AdminPassword"].ToString();

                    ds = new DataSet();
                    sql = "SELECT * FROM Users";
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            User newUser = new User(ds.Tables[0].Rows[i]["ID"].ToString(), ds.Tables[0].Rows[i]["FirstName"].ToString(), ds.Tables[0].Rows[i]["lastName"].ToString(), ds.Tables[0].Rows[i]["UserPassword"].ToString(), BoolHelper.Parse(ds.Tables[0].Rows[i]["LoggedIn"]));

                            AllUsers.Add(newUser);
                        }
                    }
                    ds = new DataSet();
                    sql = "SELECT * FROM LoggedInUsers";
                    da = new SQLiteDataAdapter(sql, con);
                    da.Fill(ds, table);

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Shift newShift = new Shift(ds.Tables[0].Rows[i]["ID"].ToString(), DateTimeHelper.Parse(ds.Tables[0].Rows[i]["TimeIn"].ToString()));

                            CurrentlyLoggedIn.Add(newShift);
                        }
                    }
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Filling DataSet", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
        }

        /// <summary>Saves a User.</summary>
        /// <param name="saveUser">User to be saved</param>
        internal static void SaveUser(User saveUser)
        {
        }

        /// <summary>Changes the Admin password in the database.</summary>
        private static async void ChangeAdminPassword()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection { ConnectionString = _DBPROVIDERANDSOURCE };
            string sql = "UPDATE Admin SET [AdminPassword] = @adminPassword";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@adminPassword", AdminPassword);

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Changing Admin Password", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
        }

        /// <summary>Changes a User's password.</summary>
        /// <param name="user">User whose password needs to be changed</param>
        /// <param name="newHashedPassword">New hashed password</param>
        internal static async void ChangeUserPassword(User user, string newHashedPassword)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteConnection con = new SQLiteConnection { ConnectionString = _DBPROVIDERANDSOURCE };
            string sql = "UPDATE Users SET [UserPassword] = @userPassword WHERE [ID] = @id";

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;
            cmd.Parameters.AddWithValue("@userPassword", newHashedPassword);
            cmd.Parameters.AddWithValue("@id", user.ID);

            await Task.Factory.StartNew(() =>
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    new Notification(ex.Message, "Error Changing User Password", NotificationButtons.OK).ShowDialog();
                }
                finally { con.Close(); }
            });
        }
    }
}