using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace TimeClock
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        internal MainWindow RefToMainWindow { get; set; }

        #region Button-Click Methods

        private void btnNewUser_Click(object sender, RoutedEventArgs e)
        {
            NewUserWindow newUserWindow = new NewUserWindow();
            newUserWindow.RefToAdminWindow = this;
            newUserWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnEditTimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void btnLogOutAll_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentlyLoggedIn.Count > 0)
            {
                List<Shift> loggedIn = new List<Shift>(AppState.CurrentlyLoggedIn);
                foreach (Shift shft in loggedIn)
                    await AppState.LogOut(AppState.AllUsers.Find(user => user.ID == shft.ID));

                MessageBox.Show("All users now logged out.", "Time Clock", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("All users are currently logged out.", "Time Clock", MessageBoxButton.OK);
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            AdminChangePasswordWindow adminChangePasswordWindow = new AdminChangePasswordWindow();
            adminChangePasswordWindow.RefToAdminWindow = this;
            adminChangePasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>
        /// Closes the Window.
        /// </summary>
        private void CloseWindow()
        {
            this.Close();
        }

        public AdminWindow()
        {
            InitializeComponent();
        }

        private void windowAdmin_Closing(object sender, CancelEventArgs e)
        {
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}