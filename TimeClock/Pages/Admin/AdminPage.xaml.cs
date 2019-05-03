using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;
using TimeClock.Pages.SharedPages;

namespace TimeClock.Pages.Admin
{
    /// <summary>Interaction logic for AdminPage.xaml</summary>
    public partial class AdminPage
    {
        #region Button-Click Methods

        private void BtnManageRoles_Click(object sender, RoutedEventArgs e) => AppState.Navigate(new AdminRolesPage());

        private void BtnManageUsers_Click(object sender, RoutedEventArgs e) => AppState.Navigate(new AdminUsersPage());

        private async void BtnLogOutAll_Click(object sender, RoutedEventArgs e)
        {
            List<User> loggedInUsers = await AppState.LoadUsers(true).ConfigureAwait(false);
            if (loggedInUsers.Count > 0)
            {
                List<string> users = new List<string>(loggedInUsers.Count);
                foreach (User user in loggedInUsers)
                {
                    users.Add(user.Names);
                    await AppState.LogOut(new Shift(user.GetMostRecentShift()) { ShiftEnd = DateTime.Now }).ConfigureAwait(false);
                }
                AppState.DisplayNotification($"Users\n{string.Join("\n",users)}\nnow logged out.", "Time Clock");
            }
            else
                AppState.DisplayNotification("All users are currently logged out.", "Time Clock");
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e) => AppState.Navigate(new ChangePasswordPage { Admin = true });

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            AppState.MainWindow.MainFrame.RemoveBackEntry();
            AppState.GoBack();
            AppState.MainWindow.MnuAdmin.IsEnabled = true;
        }

        public AdminPage() => InitializeComponent();

        #endregion Page-Manipulation Methods
    }
}