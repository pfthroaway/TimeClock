using Extensions;
using Extensions.Encryption;
using System.Windows;
using System.Windows.Input;
using TimeClock.Classes;
using TimeClock.Classes.Entities;
using TimeClock.Pages.Users;

namespace TimeClock.Pages
{
    /// <summary>Interaction logic for LoginPage.xaml</summary>
    public partial class LoginPage
    {
        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User checkUser = await AppState.LoadUser(TxtUserID.Text.Trim()).ConfigureAwait(false);
            if (checkUser != new User() && PBKDF2.ValidatePassword(PswdPassword.Password.Trim(), checkUser.Password))
            {
                Dispatcher.Invoke(() =>
                {
                    AppState.CurrentUser = checkUser;
                    TxtUserID.Clear();
                    PswdPassword.Clear();
                    TxtUserID.Focus();
                    AppState.Navigate(new TimeClockPage());
                });
            }
            else
                AppState.DisplayNotification("Invalid login.", "Time Clock");
        }

        public LoginPage()
        {
            InitializeComponent();
            TxtUserID.Focus();
        }

        private void PswdPassword_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void TxtUserID_GotFocus(object sender, RoutedEventArgs e) => Functions.TextBoxGotFocus(sender);

        private void TxtUserID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}