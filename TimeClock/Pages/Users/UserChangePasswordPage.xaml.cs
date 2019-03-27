using Extensions;
using Extensions.Encryption;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Users
{
    /// <summary>Interaction logic for UserChangePasswordPage.xaml</summary>
    public partial class UserChangePasswordPage
    {
        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, AppState.CurrentUser.Password) && PswdNewPassword.Password == PswdConfirmPassword.Password && PswdCurrentPassword.Password != PswdNewPassword.Password)
            {
                User newUser = new User(AppState.CurrentUser) { Password = PBKDF2.HashPassword(PswdNewPassword.Password) };
                if (await AppState.ChangeUserDetails(AppState.CurrentUser, newUser))
                {
                    AppState.DisplayNotification("Successfully changed user password.", "Time Clock");
                    AppState.CurrentUser.Password = newUser.Password;
                    ClosePage();
                }
            }
            else if (PswdCurrentPassword.Password == PswdNewPassword.Password)
                AppState.DisplayNotification("The new password can't be the same as the current password.", "Time Clock");
            else if (PswdNewPassword.Password != PswdConfirmPassword.Password)
                AppState.DisplayNotification("Please ensure the new passwords match.", "Time Clock");
            else
                AppState.DisplayNotification("Invalid current user password.", "Time Clock");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page_Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public UserChangePasswordPage()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void PswdChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled =
            PswdCurrentPassword.Password.Length >= 4 && PswdNewPassword.Password.Length >= 4 &&
            PswdConfirmPassword.Password.Length >= 4;

        #endregion Page_Manipulation Methods
    }
}