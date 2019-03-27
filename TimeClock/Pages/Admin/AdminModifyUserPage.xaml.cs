using Extensions;
using Extensions.Encryption;
using Extensions.Enums;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Admin
{
    /// <summary>Interaction logic for AdminModifyUserPage.xaml</summary>
    public partial class AdminModifyUserPage
    {
        internal User SelectedUser { get; set; }

        /// <summary>Determines if buttons should be enabled.</summary>
        private void CheckInput()
        {
            bool enabled = TxtUsername.Text.Length > 0
                && TxtFirstName.Text.Length > 0
                && TxtLastName.Text.Length > 0
                && TxtUsername.Text != SelectedUser.Username
                || TxtFirstName.Text != SelectedUser.FirstName
                || TxtLastName.Text != SelectedUser.LastName
                || PswdPassword.Password.Length > 0
                && PswdConfirm.Password.Length > 0
                && TxtRoles.Text.Length > 0;
            BtnSubmit.IsEnabled = enabled;
            BtnReset.IsEnabled = enabled;
        }

        /// <summary>Resets all inputs to their default values.</summary>
        internal void Reset()
        {
            TxtUsername.Text = SelectedUser.Username;
            TxtFirstName.Text = SelectedUser.FirstName;
            TxtLastName.Text = SelectedUser.LastName;
            TxtRoles.Text = SelectedUser.RolesToString;
            PswdPassword.Password = "";
            PswdConfirm.Password = "";
        }

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (TxtFirstName.Text.Length >= 2 && TxtLastName.Text.Length >= 2 && (PswdPassword.Password.Length == 0 &&
                PswdConfirm.Password.Length == 0 || PswdPassword.Password.Length >= 4 &&
                PswdConfirm.Password.Length >= 4) && PswdPassword.Password == PswdConfirm.Password)
            {
                User checkUser = await AppState.LoadUser(TxtUsername.Text);
                if (checkUser == new User() || checkUser.Username == SelectedUser.Username)
                {
                    checkUser.Username = TxtUsername.Text.Trim();
                    checkUser.FirstName = TxtFirstName.Text.Trim();
                    checkUser.LastName = TxtLastName.Text.Trim();
                    checkUser.Password = PswdPassword.Password.Length >= 4 ? PBKDF2.HashPassword(PswdPassword.Password.Trim()) : SelectedUser.Password;

                    if (await AppState.ChangeUserDetails(SelectedUser, checkUser))
                        AppState.GoBack();
                }
                else
                    AppState.DisplayNotification("This username has already been taken.", "Time Clock");
            }
            else if (PswdPassword.Password.Length != 0 && PswdConfirm.Password.Length != 0 &&
                     PswdPassword.Password.Length < 4 && PswdConfirm.Password.Length < 4)
                AppState.DisplayNotification("Please ensure the new password is 4 or more characters in length.", "Time Clock");
            else if (PswdPassword.Password != PswdConfirm.Password)
                AppState.DisplayNotification("Please ensure the passwords match.", "Time Clock");
            else if (TxtFirstName.Text.Length < 2 && TxtLastName.Text.Length < 2)
                AppState.DisplayNotification("Please ensure the user ID and password are at least 4 characters long, and first and last names are at least 2 characters long.", "Time Clock");
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e) => Reset();

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => AppState.GoBack();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        public AdminModifyUserPage()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void AdminModifyUserPage_OnLoaded(object sender, RoutedEventArgs e) => Reset();

        private void Txt_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void TxtName_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Letters);

        private void Txt_TextChanged(object sender, TextChangedEventArgs e) => CheckInput();

        private void Pswd_TextChanged(object sender, RoutedEventArgs e) => CheckInput();

        #endregion Page-Manipulation Methods
    }
}