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
        private static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();

        /// <summary>Administrator Password</summary>
        public static string AdminPassword
        {
            get => _adminPassword;
            set => _adminPassword = value;
        }

        #region Administrator Management

        /// <summary>Changes the Admin password in the database.</summary>
        /// <param name="hashedAdminPassword">New hashed admin password</param>
        internal static async Task<bool> ChangeAdminPassword(string hashedAdminPassword)
        {
            if (await DatabaseInteraction.ChangeAdminPassword(hashedAdminPassword))
            {
                AdminPassword = hashedAdminPassword;
                return true;
            }
            return false;
        }

        #endregion Administrator Management

        #region Load

        /// <summary>Loads all required items from the database on application load.</summary>
        internal static async Task LoadAll()
        {
            DatabaseInteraction.VerifyDatabaseIntegrity();
            AdminPassword = await DatabaseInteraction.LoadAdminPassword();
            AllUsers.AddRange(await DatabaseInteraction.LoadUsers());
            CurrentlyLoggedIn.AddRange(await DatabaseInteraction.LoadLoggedInUsers());
        }

        internal static async Task LoadUserTimes(User user)
        {
            CurrentUserTimes.Clear();
            CurrentUserTimes.AddRange(await DatabaseInteraction.LoadShifts(user));
        }

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginUser">User logging in</param>
        internal static async Task LogIn(User loginUser)
        {
            if (await DatabaseInteraction.LogIn(loginUser))
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
            if (await DatabaseInteraction.LogOut(logOutUser, newShift))
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
        internal static void DisplayNotification(string message, string title)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                new Notification(message, title, NotificationButtons.OK).ShowDialog();
            });
        }

        /// <summary>Displays a new Notification in a thread-safe way.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification Window</param>
        /// <param name="window">Window being referenced</param>
        internal static void DisplayNotification(string message, string title, Window window)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                new Notification(message, title, NotificationButtons.OK, window).ShowDialog();
            });
        }

        /// <summary>Displays a new Notification in a thread-safe way and retrieves a boolean result upon its closing.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification Window</param>
        /// <param name="window">Window being referenced</param>
        /// <returns>Returns value of clicked button on Notification.</returns>
        internal static bool YesNoNotification(string message, string title, Window window)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (new Notification(message, title, NotificationButtons.YesNo, window).ShowDialog() == true)
                    result = true;
            });
            return result;
        }

        #endregion Notification Management

        #region User Management

        /// <summary>Changes a User's password.</summary>
        /// <param name="user">User whose password needs to be changed</param>
        /// <param name="newHashedPassword">New hashed password</param>
        internal static async Task<bool> ChangeUserPassword(User user, string newHashedPassword)
        {
            return await DatabaseInteraction.ChangeUserPassword(user, newHashedPassword);
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        internal static async Task<bool> NewUser(User newUser)
        {
            if (await DatabaseInteraction.NewUser(newUser))
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