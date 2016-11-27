using System.Windows;

namespace TimeClock
{
    /// <summary>
    /// Interaction logic for AdminPasswordWindow.xaml
    /// </summary>
    public partial class AdminPasswordWindow : Window
    {
        internal MainWindow RefToMainWindow { get; set; }
        private bool admin = false;

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(pswdAdmin.Password, AppState.AdminPassword))
            {
                admin = true;
                CloseWindow();
            }
            else
                MessageBox.Show("Invalid login.", "Sulimn", MessageBoxButton.OK);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
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

        public AdminPasswordWindow()
        {
            InitializeComponent();
            pswdAdmin.Focus();
        }

        private void pswdAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdAdmin.SelectAll();
        }

        private void pswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (pswdAdmin.Password.Length > 0)
                btnSubmit.IsEnabled = true;
            else
                btnSubmit.IsEnabled = false;
        }

        private void windowAdminPassword_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!admin)
                RefToMainWindow.Show();
            else
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.RefToMainWindow = RefToMainWindow;
                adminWindow.Show();
            }
        }

        #endregion Window-Manipulation Methods
    }
}