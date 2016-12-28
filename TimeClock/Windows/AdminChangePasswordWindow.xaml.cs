using System.ComponentModel;
using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for AdminChangePasswordWindow.xaml</summary>
    public partial class AdminChangePasswordWindow
    {
        internal AdminWindow RefToAdminWindow { private get; set; }

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordHash.ValidatePassword(pswdCurrentPassword.Password, AppState.AdminPassword))
            {
                if (pswdNewPassword.Password == pswdConfirmPassword.Password)
                {
                    if (pswdCurrentPassword.Password != pswdNewPassword.Password)
                    {
                        AppState.AdminPassword = PasswordHash.HashPassword(pswdNewPassword.Password);
                        new Notification("Successfully changed administrator password.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
                        CloseWindow();
                    }
                    else
                        new Notification("The new password can't be the same as the current password.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
                }
                else
                    new Notification("Please ensure the new passwords match.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
            }
            else
                new Notification("Invalid current administrator password.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
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

        public AdminChangePasswordWindow()
        {
            InitializeComponent();
            pswdCurrentPassword.Focus();
        }

        private void pswdChanged(object sender, RoutedEventArgs e)
        {
            btnSubmit.IsEnabled = pswdCurrentPassword.Password.Length >= 4 && pswdNewPassword.Password.Length >= 4 &&
                                  pswdConfirmPassword.Password.Length >= 4;
        }

        private void pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void windowAdminChangePassword_Closing(object sender, CancelEventArgs e)
        {
            RefToAdminWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}