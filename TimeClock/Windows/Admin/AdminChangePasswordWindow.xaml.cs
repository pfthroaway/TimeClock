using Extensions;
using Extensions.Encryption;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Windows.Admin
{
    /// <summary>Interaction logic for AdminChangePasswordWindow.xaml</summary>
    public partial class AdminChangePasswordWindow
    {
        internal AdminWindow PreviousWindow { private get; set; }

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, AppState.AdminPassword))
            {
                if (PswdNewPassword.Password == PswdConfirmPassword.Password)
                {
                    if (PswdCurrentPassword.Password != PswdNewPassword.Password)
                    {
                        if (await AppState.ChangeAdminPassword(PBKDF2.HashPassword(PswdNewPassword.Password)))
                        {
                            AppState.DisplayNotification("Successfully changed administrator password.", "Time Clock", this);
                            CloseWindow();
                        }
                    }
                    else
                        AppState.DisplayNotification("The new password can't be the same as the current password.", "Time Clock", this);
                }
                else
                    AppState.DisplayNotification("Please ensure the new passwords match.", "Time Clock", this);
            }
            else
                AppState.DisplayNotification("Invalid current administrator password.", "Time Clock", this);
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
            Close();
        }

        public AdminChangePasswordWindow()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void PswdChanged(object sender, RoutedEventArgs e)
        {
            BtnSubmit.IsEnabled = PswdCurrentPassword.Password.Length >= 4 && PswdNewPassword.Password.Length >= 4 &&
                                  PswdConfirmPassword.Password.Length >= 4;
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void WindowAdminChangePassword_Closing(object sender, CancelEventArgs e)
        {
            PreviousWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}