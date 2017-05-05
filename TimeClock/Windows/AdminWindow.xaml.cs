using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for AdminWindow.xaml</summary>
    public partial class AdminWindow
    {
        internal MainWindow RefToMainWindow { private get; set; }

        #region Button-Click Methods

        private void BtnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow { RefToAdminWindow = this };
            newUserWindow.Show();
            Visibility = Visibility.Hidden;
        }

        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnEditTimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void BtnLogOutAll_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentlyLoggedIn.Count > 0)
            {
                List<Shift> loggedIn = new List<Shift>(AppState.CurrentlyLoggedIn);
                foreach (Shift shft in loggedIn)
                    await AppState.LogOut(AppState.AllUsers.Find(user => user.ID == shft.ID));
                AppState.DisplayNotification("All users now logged out.", "Time Clock", this);
            }
            else
                AppState.DisplayNotification("All users are currently logged out.", "Time Clock", this);
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AdminChangePasswordWindow adminChangePasswordWindow = new AdminChangePasswordWindow
            {
                RefToAdminWindow = this
            };
            adminChangePasswordWindow.Show();
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
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}