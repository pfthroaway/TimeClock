using Extensions;
using Extensions.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using TimeClock.Classes.Database;
using TimeClock.Classes.Entities;

namespace TimeClock.Classes
{
    /// <summary>Represents the current state of the application.</summary>
    internal static class AppState
    {
        internal static User CurrentUser = new User();
        private static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();

        /// <summary>Administrator Password</summary>
        public static string AdminPassword { get; set; }

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

        /// <summary>Gets the next User ID autoincrement value in the database for the Users table.</summary>
        /// <returns>Next User ID value</returns>
        public static async Task<int> GetNextUserIndex()
        {
            return await DatabaseInteraction.GetNextUserIndex();
        }

        /// <summary>Loads all required items from the database on application load.</summary>
        internal static async Task LoadAll()
        {
            DatabaseInteraction.VerifyDatabaseIntegrity();
            AdminPassword = await DatabaseInteraction.LoadAdminPassword();
        }

        /// <summary>Loads a User from the database.</summary>
        /// <returns>User</returns>
        public static async Task<User> LoadUser(string username)
        {
            return await DatabaseInteraction.LoadUser(username);
        }

        /// <summary>Loads all Users from the database.</summary>
        /// <returns>User</returns>
        public static async Task<List<User>> LoadUsers(bool loggedIn = false)
        {
            return await DatabaseInteraction.LoadUsers(loggedIn);
        }

        #endregion Load

        #region Log In/Out

        /// <summary>Logs in a User.</summary>
        /// <param name="loginShift">Shift started by User</param>
        internal static async Task<bool> LogIn(Shift loginShift) => await DatabaseInteraction.LogIn(loginShift);

        /// <summary>Logs out a User.</summary>
        /// <param name="logOutShift">Shift to be created on logout</param>
        internal static async Task<bool> LogOut(Shift logOutShift) => await DatabaseInteraction.LogOut(logOutShift);

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

        /// <summary>Changes a User's details in the database.</summary>
        /// <param name="oldUser">User whose details needs to be changed</param>
        /// <param name="newUser">User with new details</param>
        /// <returns>True if successfully updated in the database</returns>
        internal static async Task<bool> ChangeUserDetails(User oldUser, User newUser)
        {
            return await DatabaseInteraction.ChangeUserDetails(oldUser, newUser);
        }

        /// <summary>Deletes a User and all their Shifts from the database.</summary>
        /// <param name="user">User to be deleted</param>
        /// <returns>True if successful</returns>
        public static async Task<bool> DeleteUser(User user)
        {
            return await DatabaseInteraction.DeleteUser(user);
        }

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        internal static async Task<bool> NewUser(User newUser)
        {
            return await DatabaseInteraction.NewUser(newUser);
        }

        /// <summary>Updates a User.</summary>
        /// <param name="updateUser">User to be updated</param>
        internal static void UpdateUser(User updateUser)
        {
        }

        #endregion User Management
    }
}