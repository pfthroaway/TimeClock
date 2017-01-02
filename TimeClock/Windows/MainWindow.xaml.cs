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

        private void mnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminPasswordWindow adminPasswordWindow = new AdminPasswordWindow { RefToMainWindow = this };
            adminPasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Count(user => user.ID == txtUserID.Text) > 0)
            {
                User selectedUser = AppState.AllUsers.Find(user => user.ID == txtUserID.Text);
                if (PasswordHash.ValidatePassword(pswdPassword.Password, selectedUser.Password))
                {
                    AppState.CurrentUser = new User(selectedUser);
                    txtUserID.Clear();
                    pswdPassword.Clear();
                    txtUserID.Focus();
                    TimeClockWindow timeClockWindow = new TimeClockWindow { RefToMainWindow = this };
                    timeClockWindow.Show();
                    this.Visibility = Visibility.Hidden;
                }
                else
                    new Notification("Invalid login.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
            }
            else
                new Notification("Invalid login.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        public MainWindow()
        {
            InitializeComponent();
            txtUserID.Focus();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void txtUserID_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void txtUserID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Functions.PreviewKeyDown(e, KeyType.Letters);
        }

        #endregion Window-Manipulation Methods

        private async void windowMain_Loaded(object sender, RoutedEventArgs e)
        {
            await AppState.LoadAll();
        }
    }
}