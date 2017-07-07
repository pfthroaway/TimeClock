using Extensions;
using Extensions.Encryption;
using System.Windows;
using System.Windows.Input;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Windows
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region Click Methods

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            Admin.AdminPasswordWindow adminPasswordWindow = new Admin.AdminPasswordWindow { PreviousWindow = this };
            adminPasswordWindow.Show();
            Visibility = Visibility.Hidden;
        }

        private void MnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User checkUser = await AppState.LoadUser(TxtUserID.Text);
            if (checkUser != null && PBKDF2.ValidatePassword(PswdPassword.Password, checkUser.Password))
            {
                AppState.CurrentUser = new User(checkUser);
                TxtUserID.Clear();
                PswdPassword.Clear();
                TxtUserID.Focus();
                Users.TimeClockWindow timeClockWindow = new Users.TimeClockWindow { PreviousWindow = this };
                timeClockWindow.Show();
                Visibility = Visibility.Hidden;
            }
            else
                AppState.DisplayNotification("Invalid login.", "Time Clock", this);
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            await AppState.LoadAll();
        }

        public MainWindow()
        {
            InitializeComponent();
            TxtUserID.Focus();
        }

        private void PswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void TxtUserID_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void TxtUserID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        #endregion Window-Manipulation Methods
    }
}