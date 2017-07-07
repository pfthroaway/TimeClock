using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Windows.Admin
{
    /// <summary>Interaction logic for AdminWindow.xaml</summary>
    public partial class AdminWindow
    {
        internal MainWindow PreviousWindow { private get; set; }

        #region Button-Click Methods

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            Users.NewUserWindow newUserWindow = new Users.NewUserWindow { PreviousWindow = this };
            newUserWindow.Show();
            Visibility = Visibility.Hidden;
        }

        private void BtnViewUsers_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void BtnLogOutAll_Click(object sender, RoutedEventArgs e)
        {
            List<User> loggedInUsers = await AppState.LoadUsers(true);
            if (loggedInUsers.Count > 0)
            {
                foreach (User user in loggedInUsers)
                    await AppState.LogOut(new Shift(user.GetMostRecentShift()) { ShiftEnd = DateTime.Now });
                AppState.DisplayNotification("All users now logged out.", "Time Clock", this);
            }
            else
                AppState.DisplayNotification("All users are currently logged out.", "Time Clock", this);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AdminChangePasswordWindow window = new AdminChangePasswordWindow
            {
                PreviousWindow = this
            };
            window.Show();
            Visibility = Visibility.Hidden;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>Closes the Window.</summary>
        private void CloseWindow()
        {
            Close();
        }

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void WindowAdmin_Closing(object sender, CancelEventArgs e)
        {
            PreviousWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}