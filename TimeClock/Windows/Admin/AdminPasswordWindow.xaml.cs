using Extensions;
using Extensions.Encryption;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Windows.Admin
{
    /// <summary>Interaction logic for AdminPasswordWindow.xamlA</summary>
    public partial class AdminPasswordWindow
    {
        internal MainWindow PreviousWindow { private get; set; }
        private bool _admin;

        #region Button-Click Methods

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdAdmin.Password, AppState.AdminPassword))
            {
                _admin = true;
                CloseWindow();
            }
            else
            {
                AppState.DisplayNotification("Invalid login.", "Time Clock", this);
                PswdAdmin.Focus();
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
            Close();
        }

        public AdminPasswordWindow()
        {
            InitializeComponent();
            PswdAdmin.Focus();
        }

        private void PswdAdmin_GotFocus(object sender, RoutedEventArgs e)
        {
            Functions.PasswordBoxGotFocus(sender);
        }

        private void PswdAdmin_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BtnSubmit.IsEnabled = PswdAdmin.Password.Length > 0;
        }

        private void WindowAdminPassword_Closing(object sender, CancelEventArgs e)
        {
            if (!_admin)
                PreviousWindow.Show();
            else
            {
                AdminWindow adminWindow = new AdminWindow { PreviousWindow = PreviousWindow };
                adminWindow.Show();
            }
        }

        #endregion Window-Manipulation Methods
    }
}