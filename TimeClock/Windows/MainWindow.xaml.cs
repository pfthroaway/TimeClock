using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace TimeClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Click Methods

        private void mnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            AdminPasswordWindow adminPasswordWindow = new AdminPasswordWindow();
            adminPasswordWindow.RefToMainWindow = this;
            adminPasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void mnuFileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Where(user => user.ID == txtUserID.Text).Count() > 0)
            {
                User selectedUser = AppState.AllUsers.Find(user => user.ID == txtUserID.Text);
                if (PasswordHash.ValidatePassword(pswdPassword.Password, selectedUser.Password))
                {
                    AppState.CurrentUser = new User(selectedUser);
                    txtUserID.Clear();
                    pswdPassword.Clear();
                    txtUserID.Focus();
                    TimeClockWindow timeClockWindow = new TimeClockWindow();
                    timeClockWindow.RefToMainWindow = this;
                    timeClockWindow.Show();
                    this.Visibility = Visibility.Hidden;
                }
                else
                    MessageBox.Show("Invalid login.", "Time Clock", MessageBoxButton.OK);
            }
            else
                MessageBox.Show("Invalid login.", "Time Clock", MessageBoxButton.OK);
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        public MainWindow()
        {
            InitializeComponent();
            AppState.LoadAll();
            txtUserID.Focus();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdPassword.SelectAll();
        }

        private void txtUserID_GotFocus(object sender, RoutedEventArgs e)
        {
            txtUserID.SelectAll();
        }

        private void txtUserID_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key k = e.Key;

            List<bool> keys = AppState.GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.LeftShift, Key.RightShift, Key.Enter, Key.Tab, Key.LeftAlt, Key.RightAlt, Key.Left, Key.Right, Key.LeftCtrl, Key.RightCtrl, Key.Escape);

            if (keys.Any(key => key == true) || Key.A <= k && k <= Key.Z || (Key.D0 <= k && k <= Key.D9) || (Key.NumPad0 <= k && k <= Key.NumPad9))
                e.Handled = false;
            else
                e.Handled = true;
        }

        #endregion Window-Manipulation Methods
    }
}