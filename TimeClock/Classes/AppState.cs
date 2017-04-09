using Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace TimeClock
{
    internal static class AppState
    {
        private static string _adminPassword;
        internal static User CurrentUser = new User();
        internal static readonly List<Shift> CurrentUserTimes = new List<Shift>();
        internal static readonly List<User> AllUsers = new List<User>();
        internal static readonly List<Shift> CurrentlyLoggedIn = new List<Shift>();
        private static readonly SQLiteDatabaseInteraction _databaseInteraction = new SQLiteDatabaseInteraction();

        /// <summary>Administrator Password</summary>
        public static string AdminPassword
        {
            get { return _adminPassword; }
            set { _adminPassword = value; }
        }

        #region Administrator Management

        /// <summary>Changes the Admin password in the database.</summary>
        /// <param name="hashedAdminPassword">New hashed admin password</param>
        internal static async Task<bool> ChangeAdminPassword(string hashedAdminPassword)
        {
            if (await _databaseInteraction.ChangeAdminPassword(hashedAdminPassword))
            {
                AdminPassword = hashedAdminPassword;
                return true;
            }
            return false;
        }

        #endregion Administrator Management

        #region Load

        /// <summary>Loads all the selected User's times from the database.</summary>
        /// <param name="user">User whose times are loaded from the database</param>
        /// <returns>True</returns>
        /// <summary>Loads all required items from the database on application load.</summary>
        internal static async Task LoadAll()
        {
            if (_databaseInteraction.VerifyDatabaseIntegrity())
            {
                AdminPassword = await _databaseInteraction.LoadAdminPassword();
                AllUsers.AddRange(await _databaseInteraction.LoadUsers());
                CurrentlyLoggedIn.AddRange(await _databaseInteraction.LoadLoggedInUsers());
            }
        }

        internal static async Task LoadUserTimes(User user)
        {
            CurrentUserTimes.Clear();
            CurrentUserTimes.AddRange(await _databaseInteraction.LoadShifts(user));
        }

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginUser">User logging in</param>
        internal static async Task LogIn(User loginUser)
        {
            if (await _databaseInteraction.LogIn(loginUser))
            {
                CurrentlyLoggedIn.Add(new Shift(loginUser.ID, DateTime.Now));
                AllUsers.Find(user => user.ID == loginUser.ID).LoggedIn = true;
            }
        }

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutUser">User logging out</param>
        internal static async Task LogOut(User logOutUser)
        {
            DateTime shiftStartTime = CurrentlyLoggedIn.Find(shift => shift.ID == logOutUser.ID).ShiftStart;
            Shift newShift = new Shift(logOutUser.ID, shiftStartTime, DateTime.Now);
            if (await _databaseInteraction.LogOut(logOutUser, newShift))
            {
                CurrentUserTimes.Add(newShift);
                CurrentlyLoggedIn.Remove(CurrentlyLoggedIn.Find(shift => shift.ID == logOutUser.ID));
                AllUsers.Find(user => user.ID == logOutUser.ID).LoggedIn = false;
            }
        }

        #endregion Log In/Out

        #region Notification Management

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification Window</param>
        /// <param name="buttons">Button type to be displayed on the Window</param>
        internal static void DisplayNotification(string message, string title, NotificationButtons buttons)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                new Notification(message, title, buttons).ShowDialog();
            });
        }

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification Window</param>
        /// <param name="buttons">Button type to be displayed on the Window</param>
        /// <param name="Window">Window being referenced</param>
        internal static void DisplayNotification(string message, string title, NotificationButtons buttons, Window Window)
        {
            Application.Current.Dispatcher.Invoke((Action)delegate
            {
                new Notification(message, title, buttons, Window).ShowDialog();
            });
        }

        #endregion Notification Management

        #region User Management

        /// <summary>Changes a User's password.</summary>
        /// <param name="user">User whose password needs to be changed</param>
        /// <param name="newHashedPassword">New hashed password</param>
        internal static async Task<bool> ChangeUserPassword(User user, string newHashedPassword)
        {
            return await _databaseInteraction.ChangeUserPassword(user, newHashedPassword);
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        internal static async Task<bool> NewUser(User newUser)
        {
            if (await _databaseInteraction.NewUser(newUser))
            {
                AllUsers.Add(newUser);
                return true;
            }
            return false;
        }

        /// <summary>Saves a User.</summary>
        /// <param name="saveUser">User to be saved</param>
        internal static void SaveUser(User saveUser)
        {
        }

        #endregion User Management
    }
}