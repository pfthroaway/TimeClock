using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeClock
{
    /// <summary>
    /// Interaction logic for NewUserWindow.xaml
    /// </summary>
    public partial class NewUserWindow : Window
    {
        internal AdminWindow RefToAdminWindow { get; set; }

        /// <summary>
        /// Checks each box has text to determine if Submit button should be enabled.
        /// </summary>
        private void CheckInput()
        {
            if (txtID.Text.Length > 0 && txtFirstName.Text.Length > 0 && txtLastName.Text.Length > 0 && pswdPassword.Password.Length > 0 && pswdConfirm.Password.Length > 0)
                btnSubmit.IsEnabled = true;
            else
                btnSubmit.IsEnabled = false;
        }

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Where(user => user.ID == txtID.Text).Count() > 0)
                MessageBox.Show("This user ID has already been taken.", "Time Clock", MessageBoxButton.OK);
            else
            {
                if (txtID.Text.Length >= 4 && txtFirstName.Text.Length >= 2 && txtLastName.Text.Length >= 2 && pswdPassword.Password.Length >= 4 && pswdConfirm.Password.Length >= 4)
                {
                    if (pswdPassword.Password == pswdConfirm.Password)
                    {
                        User newUser = new User(txtID.Text.Trim(), txtFirstName.Text.Trim(), txtLastName.Text.Trim(), PasswordHash.HashPassword(pswdPassword.Password.Trim()), false);
                        AppState.NewUser(newUser);
                        CloseWindow();
                    }
                    else
                        MessageBox.Show("Please ensure the passwords match.", "Time Clock", MessageBoxButton.OK);
                }
                else
                    MessageBox.Show("Please ensure the user ID and password are at least 4 characters long, and first and last names are at least 2 characters long.", "Sulimn", MessageBoxButton.OK);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>
        /// Closes the Window.
        /// </summary>
        private void CloseWindow()
        {
            this.Close();
        }

        public NewUserWindow()
        {
            InitializeComponent();
        }

        private void txtID_GotFocus(object sender, RoutedEventArgs e)
        {
            txtID.Focus();
        }

        private void txtFirstName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtFirstName.Focus();
        }

        private void txtLastName_GotFocus(object sender, RoutedEventArgs e)
        {
            txtLastName.Focus();
        }

        private void pswdPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdPassword.Focus();
        }

        private void pswdConfirm_GotFocus(object sender, RoutedEventArgs e)
        {
            pswdConfirm.Focus();
        }

        private void txtName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Key k = e.Key;

            List<bool> keys = AppState.GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.LeftShift, Key.RightShift, Key.Enter, Key.Tab, Key.LeftAlt, Key.RightAlt, Key.Left, Key.Right, Key.LeftCtrl, Key.RightCtrl, Key.Escape);

            if (keys.Any(key => key == true) || Key.A <= k && k <= Key.Z)
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void txt_TextChanged(object sender, TextChangedEventArgs e)
        {
            CheckInput();
        }

        private void pswd_TextChanged(object sender, RoutedEventArgs e)
        {
            CheckInput();
        }

        private void windowNewUser_Closing(object sender, CancelEventArgs e)
        {
            RefToAdminWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}