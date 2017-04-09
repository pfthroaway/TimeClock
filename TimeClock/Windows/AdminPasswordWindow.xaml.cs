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

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(PswdAdmin.Password, AppState.AdminPassword))
            {
                _admin = true;
                CloseWindow();
            }
            else
            {
                AppState.DisplayNotification("Invalid login.", "Sulimn", NotificationButtons.OK, this);
                PswdAdmin.Focus();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
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
            PswdAdmin.Focus();
        }

        private void PswdAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void PswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BtnSubmit.IsEnabled = PswdAdmin.Password.Length > 0;
        }

        private void WindowAdminPassword_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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