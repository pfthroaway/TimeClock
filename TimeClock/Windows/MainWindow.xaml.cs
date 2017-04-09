using Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TimeClock
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region Click Methods

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminPasswordWindow adminPasswordWindow = new AdminPasswordWindow { RefToMainWindow = this };
            adminPasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void MnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Count(user => user.ID == TxtUserID.Text) > 0)
            {
                User selectedUser = AppState.AllUsers.Find(user => user.ID == TxtUserID.Text);
                if (PasswordHash.ValidatePassword(PswdPassword.Password, selectedUser.Password))
                {
                    AppState.CurrentUser = new User(selectedUser);
                    TxtUserID.Clear();
                    PswdPassword.Clear();
                    TxtUserID.Focus();
                    TimeClockWindow timeClockWindow = new TimeClockWindow { RefToMainWindow = this };
                    timeClockWindow.Show();
                    this.Visibility = Visibility.Hidden;
                }
                else
                    AppState.DisplayNotification("Invalid login.", "Time Clock", NotificationButtons.OK, this);
            }
            else
                AppState.DisplayNotification("Invalid login.", "Time Clock", NotificationButtons.OK, this);
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

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

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            await AppState.LoadAll();
        }
    }
}