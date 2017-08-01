using Extensions;
using Extensions.Encryption;
using Extensions.Enums;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Users
{
    /// <summary>Interaction logic for NewUserPage.xaml</summary>
    public partial class NewUserPage
    {
        /// <summary>Checks whether each box has text to determine if Submit button should be enabled.</summary>
        private void CheckInput() => BtnSubmit.IsEnabled =
            TxtUsername.Text.Length > 0 && TxtFirstName.Text.Length > 0 &&
            TxtLastName.Text.Length > 0 &&
            PswdPassword.Password.Length > 0 && PswdConfirm.Password.Length > 0;

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            User checkUser = await AppState.LoadUser(TxtUsername.Text);
            if (checkUser != new User())
                AppState.DisplayNotification("This username has already been taken.", "Time Clock");
            else
            {
                if (TxtUsername.Text.Length >= 4 && TxtFirstName.Text.Length >= 2 && TxtLastName.Text.Length >= 2 && PswdPassword.Password.Length >= 4 && PswdConfirm.Password.Length >= 4)
                {
                    if (PswdPassword.Password == PswdConfirm.Password)
                    {
                        User newUser = new User(await AppState.GetNextUserIndex(), TxtUsername.Text.Trim(), TxtFirstName.Text.Trim(), TxtLastName.Text.Trim(), PBKDF2.HashPassword(PswdPassword.Password.Trim()), false, new List<Shift>());
                        if (await AppState.NewUser(newUser))
                            ClosePage();
                    }
                    else
                        AppState.DisplayNotification("Please ensure the passwords match.", "Time Clock");
                }
                else
                    AppState.DisplayNotification("Please ensure the user ID and password are at least 4 characters long, and first and last names are at least 2 characters long.", "Time Clock");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public NewUserPage()
        {
            InitializeComponent();
            TxtUsername.Focus();
        }

        private void NewUserPage_OnLoaded(object sender, RoutedEventArgs e) => AppState.CalculateScale(Grid);

        private void Txt_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void Pswd_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void TxtName_PreviewKeyDown(object sender, KeyEventArgs e) => Functions.PreviewKeyDown(e, KeyType.Letters);

        private void Txt_TextChanged(object sender, TextChangedEventArgs e) => CheckInput();

        private void Pswd_TextChanged(object sender, RoutedEventArgs e) => CheckInput();

        #endregion Page-Manipulation Methods
    }
}