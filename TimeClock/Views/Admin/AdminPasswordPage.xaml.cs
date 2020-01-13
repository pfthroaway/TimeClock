using Extensions;
using Extensions.Encryption;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Views.Admin
{
    /// <summary>Interaction logic for AdminPasswordPage.xaml</summary>
    public partial class AdminPasswordPage
    {
        private bool _admin;

        #region Button-Click Methods

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (PBKDF2.ValidatePassword(PswdAdmin.Password, AppState.AdminPassword))
            {
                _admin = true;
                ClosePage();
            }
            else
            {
                AppState.DisplayNotification("Invalid login.", "Time Clock");
                PswdAdmin.Focus();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage()
        {
            AppState.GoBack();
            if (_admin)
                AppState.Navigate(new AdminPage());
            else
                AppState.MainWindow.MnuAdmin.IsEnabled = true;
        }

        public AdminPasswordPage()
        {
            InitializeComponent();
            PswdAdmin.Focus();
        }

        private void PswdAdmin_GotFocus(object sender, RoutedEventArgs e) => Functions.PasswordBoxGotFocus(sender);

        private void PswdAdmin_PasswordChanged(object sender, RoutedEventArgs e) => BtnSubmit.IsEnabled = PswdAdmin.Password.Length > 0;

        #endregion Page-Manipulation Methods
    }
}