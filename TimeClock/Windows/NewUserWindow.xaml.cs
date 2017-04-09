using Extensions;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeClock
{
    /// <summary>Interaction logic for NewUserWindow.xaml</summary>
    public partial class NewUserWindow
    {
        internal AdminWindow RefToAdminWindow { private get; set; }

        /// <summary>Checks each box has text to determine if Submit button should be enabled.</summary>
        private void CheckInput()
        {
            BtnSubmit.IsEnabled = TxtID.Text.Length > 0 && TxtFirstName.Text.Length > 0 && TxtLastName.Text.Length > 0 &&
                                  PswdPassword.Password.Length > 0 && PswdConfirm.Password.Length > 0;
        }

        #region Button-Click Methods

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Any(user => user.ID == TxtID.Text))
                AppState.DisplayNotification("This user ID has already been taken.", "Time Clock", NotificationButtons.OK, this);
            else
            {
                if (TxtID.Text.Length >= 4 && TxtFirstName.Text.Length >= 2 && TxtLastName.Text.Length >= 2 && PswdPassword.Password.Length >= 4 && PswdConfirm.Password.Length >= 4)
                {
                    if (PswdPassword.Password == PswdConfirm.Password)
                    {
                        User newUser = new User(TxtID.Text.Trim(), TxtFirstName.Text.Trim(), TxtLastName.Text.Trim(), PasswordHash.HashPassword(PswdPassword.Password.Trim()), false);
                        if (await AppState.NewUser(newUser))
                            CloseWindow();
                    }
                    else
                        AppState.DisplayNotification("Please ensure the passwords match.", "Time Clock", NotificationButtons.OK, this);
                }
                else
                    AppState.DisplayNotification("Please ensure the user ID and password are at least 4 characters long, and first and last names are at least 2 characters long.", "Sulimn", NotificationButtons.OK, this);
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

        public NewUserWindow()
        {
            InitializeComponent();
            TxtID.Focus();
        }

        private void Txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void Pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void TxtName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Functions.PreviewKeyDown(e, KeyType.Letters);
        }

        private void Txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput();
        }

        private void Pswd_TextChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }

        private void WindowNewUser_Closing(object sender, CancelEventArgs e)
        {
            RefToAdminWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}