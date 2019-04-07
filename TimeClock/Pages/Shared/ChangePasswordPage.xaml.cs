using Extensions;
using Extensions.Encryption;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Shared
{
    /// <summary>Interaction logic for AdminChangePasswordPage.xaml</summary>
    public partial class ChangePasswordPage
    {
        internal bool Admin { get; set; }

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            bool success = false;
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, Admin ? AppState.AdminPassword : AppState.CurrentUser.Password) && PswdNewPassword.Password == PswdConfirmPassword.Password && PswdCurrentPassword.Password != PswdNewPassword.Password)
            {
                if (Admin && await AppState.ChangeAdminPassword(PBKDF2.HashPassword(PswdNewPassword.Password)))
                {
                    AppState.DisplayNotification("Successfully changed administrator password.", "Time Clock");
                    success = true;
                }
                else if (!Admin)
                {
                    User newUser = new User(AppState.CurrentUser) { Password = PBKDF2.HashPassword(PswdNewPassword.Password) };
                    if (await AppState.ChangeUserDetails(AppState.CurrentUser, newUser))
                    {
                        AppState.DisplayNotification("Successfully changed user password.", "Time Clock");
                        AppState.CurrentUser.Password = newUser.Password;
                        success = true;
                    }
                }
            }
            else if (PswdCurrentPassword.Password == PswdNewPassword.Password)
            {
                AppState.DisplayNotification("The new password can't be the same as the current password.", "Time Clock");
                PswdNewPassword.Focus();
            }
            else if (PswdNewPassword.Password != PswdConfirmPassword.Password)
            {
                AppState.DisplayNotification("Please ensure the new passwords match.", "Time Clock");
                PswdConfirmPassword.Focus();
            }
            else
            {
                AppState.DisplayNotification("Invalid current password.", "Time Clock");
                PswdCurrentPassword.Focus();
            }

            if (success)
                AppState.GoBack();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => AppState.GoBack();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public ChangePasswordPage()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void PswdChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled = PswdCurrentPassword.Password.Length >= 4 && PswdNewPassword.Password.Length >= 4 && PswdConfirmPassword.Password.Length >= 4;

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        #endregion Page-Manipulation Methods
    }
}