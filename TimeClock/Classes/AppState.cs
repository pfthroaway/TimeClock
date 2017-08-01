using Extensions;
using Extensions.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimeClock.Classes.Database;
using TimeClock.Classes.Entities;
using TimeClock.Pages;

namespace TimeClock.Classes
{
    /// <summary>Represents the current state of the application.</summary>
    internal static class AppState
    {
        internal static User CurrentUser = new User();
        private static readonly SQLiteDatabaseInteraction DatabaseInteraction = new SQLiteDatabaseInteraction();

        #region Navigation

        /// <summary>Instance of MainWindow currently loaded</summary>
        internal static MainWindow MainWindow { get; set; }

        /// <summary>Width of the Page currently being displayed in the MainWindow</summary>
        internal static double CurrentPageWidth { get; set; }

        /// <summary>Height of the Page currently being displayed in the MainWindow</summary>
        internal static double CurrentPageHeight { get; set; }

        /// <summary>Calculates the scale needed for the MainWindow.</summary>
        /// <param name="grid">Grid of current Page</param>
        internal static void CalculateScale(Grid grid)
        {
            CurrentPageHeight = grid.ActualHeight;
            CurrentPageWidth = grid.ActualWidth;
            MainWindow.CalculateScale();

            Page newPage = MainWindow.MainFrame.Content as Page;
            if (newPage != null)
                newPage.Style = (Style)MainWindow.FindResource("PageStyle");
        }

        /// <summary>Navigates to selected Page.</summary>
        /// <param name="newPage">Page to navigate to.</param>
        internal static void Navigate(Page newPage) => MainWindow.MainFrame.Navigate(newPage);

        /// <summary>Navigates to the previous Page.</summary>
        internal static void GoBack()
        {
            if (MainWindow.MainFrame.CanGoBack)
                MainWindow.MainFrame.GoBack();
        }

        #endregion Navigation

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
        public static async Task<int> GetNextUserIndex() => await DatabaseInteraction.GetNextUserIndex();

        /// <summary>Loads all required items from the database on application load.</summary>
        internal static async Task LoadAll()
        {
            DatabaseInteraction.VerifyDatabaseIntegrity();
            AdminPassword = await DatabaseInteraction.LoadAdminPassword();
        }

        /// <summary>Loads a User from the database.</summary>
        /// <returns>User</returns>
        public static async Task<User> LoadUser(string username) => await DatabaseInteraction.LoadUser(username);

        /// <summary>Loads all Users from the database.</summary>
        /// <returns>All Users</returns>
        public static async Task<List<User>> LoadUsers(bool loggedIn = false) => await DatabaseInteraction.LoadUsers(loggedIn);

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
        /// <param name="title">Title of the Notification window</param>
        internal static void DisplayNotification(string message, string title) => Application.Current.Dispatcher.Invoke(
            () => { new Notification(message, title, NotificationButtons.OK, MainWindow).ShowDialog(); });

        /// <summary>Displays a new Notification in a thread-safe way and retrieves a boolean result upon its closing.</summary>
        /// <param name="message">Message to be displayed</param>
        /// <param name="title">Title of the Notification window</param>
        /// <returns>Returns value of clicked button on Notification.</returns>
        internal static bool YesNoNotification(string message, string title)
        {
            bool result = false;
            Application.Current.Dispatcher.Invoke(delegate
            {
                if (new Notification(message, title, NotificationButtons.YesNo, MainWindow).ShowDialog() == true)
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
        internal static async Task<bool> ChangeUserDetails(User oldUser, User newUser) => await DatabaseInteraction
            .ChangeUserDetails(oldUser, newUser);

        /// <summary>Deletes a User and all their Shifts from the database.</summary>
        /// <param name="user">User to be deleted</param>
        /// <returns>True if successful</returns>
        public static async Task<bool> DeleteUser(User user) => await DatabaseInteraction.DeleteUser(user);

        /// <summary>Adds a new User to the database.</summary>
        /// <param name="newUser">User to be added to the database.</param>
        internal static async Task<bool> NewUser(User newUser) => await DatabaseInteraction.NewUser(newUser);

        #endregion User Management
    }
}