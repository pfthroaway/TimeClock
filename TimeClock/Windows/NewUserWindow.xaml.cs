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
            btnSubmit.IsEnabled = txtID.Text.Length > 0 && txtFirstName.Text.Length > 0 && txtLastName.Text.Length > 0 &&
                                  pswdPassword.Password.Length > 0 && pswdConfirm.Password.Length > 0;
        }

        #region Button-Click Methods

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.AllUsers.Count(user => user.ID == txtID.Text) > 0)
                new Notification("This user ID has already been taken.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
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
                        new Notification("Please ensure the passwords match.", "Time Clock", NotificationButtons.OK, this).ShowDialog();
                }
                else
                    new Notification("Please ensure the user ID and password are at least 4 characters long, and first and last names are at least 2 characters long.", "Sulimn", NotificationButtons.OK, this).ShowDialog();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
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
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.TextBoxGotFocus(sender);
        }

        private void pswd_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void txtName_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Functions.PreviewKeyDown(e, KeyType.Letters);
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