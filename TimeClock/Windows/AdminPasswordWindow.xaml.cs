using Extensions;
using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for AdminPasswordWindow.xamlA</summary>
    public partial class AdminPasswordWindow
    {
        internal MainWindow RefToMainWindow { private get; set; }
        private bool _admin;

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(pswdAdmin.Password, AppState.AdminPassword))
            {
                _admin = true;
                CloseWindow();
            }
            else
            {
                new Notification("Invalid login.", "Sulimn", NotificationButtons.OK, this).ShowDialog();
                pswdAdmin.Focus();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>Closes the Window.</summary>
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
            Functions.PasswordBoxGotFocus(sender);
        }

        private void pswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            btnSubmit.IsEnabled = pswdAdmin.Password.Length > 0;
        }

        private void windowAdminPassword_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_admin)
                RefToMainWindow.Show();
            else
            {
                AdminWindow adminWindow = new AdminWindow { RefToMainWindow = RefToMainWindow };
                adminWindow.Show();
            }
        }

        #endregion Window-Manipulation Methods
    }
}