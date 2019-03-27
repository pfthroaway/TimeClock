using Extensions;
using Extensions.Encryption;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Pages.Admin
{
    /// <summary>Interaction logic for AdminChangePasswordPage.xaml</summary>
    public partial class AdminChangePasswordPage
    {
        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdCurrentPassword.Password, AppState.AdminPassword) && PswdNewPassword.Password == PswdConfirmPassword.Password && PswdCurrentPassword.Password != PswdNewPassword.Password && await AppState.ChangeAdminPassword(PBKDF2.HashPassword(PswdNewPassword.Password)))
            {
                AppState.DisplayNotification("Successfully changed administrator password.", "Time Clock");
                ClosePage();
            }
            else if (PswdCurrentPassword.Password == PswdNewPassword.Password)
                AppState.DisplayNotification("The new password can't be the same as the current password.", "Time Clock");
            else if (PswdNewPassword.Password != PswdConfirmPassword.Password)
                AppState.DisplayNotification("Please ensure the new passwords match.", "Time Clock");
            else
                AppState.DisplayNotification("Invalid current administrator password.", "Time Clock");
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public AdminChangePasswordPage()
        {
            InitializeComponent();
            PswdCurrentPassword.Focus();
        }

        private void PswdChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled =
            PswdCurrentPassword.Password.Length >= 4 && PswdNewPassword.Password.Length >= 4 &&
            PswdConfirmPassword.Password.Length >= 4;

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        #endregion Page-Manipulation Methods
    }
}