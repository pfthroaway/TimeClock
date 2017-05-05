using Extensions;
using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for UserChangePasswordWindow.xaml</summary>
    public partial class UserChangePasswordWindow
    {
        internal TimeClockWindow RefToTimeClockWindow { private get; set; }

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, AppState.CurrentUser.Password))
            {
                if (PswdNewPassword.Password == PswdConfirmPassword.Password)
                {
                    if (PswdCurrentPassword.Password != PswdNewPassword.Password)
                    {
                        await AppState.ChangeUserPassword(AppState.CurrentUser, PBKDF2.HashPassword(PswdNewPassword.Password));
                        AppState.DisplayNotification("Successfully changed user password.", "Time Clock", this);
                        CloseWindow();
                    }
                    else
                        AppState.DisplayNotification("The new password can't be the same as the current password.", "Time Clock", this);
                }
                else
                    AppState.DisplayNotification("Please ensure the new passwords match.", "Time Clock", this);
            }
            else
                AppState.DisplayNotification("Invalid current user password.", "Time Clock", this);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window_Manipulation Methods

        /// <summary>Closes the Window.</summary>
        private void CloseWindow()
        {
            Close();
        }

        public UserChangePasswordWindow()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void PswdChanged(object sender, RoutedEventArgs e)
        {
            BtnSubmit.IsEnabled = PswdCurrentPassword.Password.Length >= 4 && PswdNewPassword.Password.Length >= 4 &&
                                  PswdConfirmPassword.Password.Length >= 4;
        }

        private void WindowChangeUserPassword_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefToTimeClockWindow.Show();
        }

        #endregion Window_Manipulation Methods
    }
}