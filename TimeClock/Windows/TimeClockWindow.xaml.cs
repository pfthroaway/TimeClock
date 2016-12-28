using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for TimeClockWindow.xaml</summary>
    public partial class TimeClockWindow
    {
        internal MainWindow RefToMainWindow { private get; set; }

        private void CheckButton()
        {
            btnInOut.Content = AppState.CurrentUser.LoggedIn ? "_OUT" : "_IN";
        }

        #region Button-Click Methods

        private async void btnInOut_Click(object sender, RoutedEventArgs e)
        {
            btnInOut.IsEnabled = false;

            if (AppState.CurrentUser.LoggedIn == false)
                await AppState.LogIn(AppState.CurrentUser);
            else
                await AppState.LogOut(AppState.CurrentUser);

            btnInOut.IsEnabled = true;
            CheckButton();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            UserChangePasswordWindow userChangePasswordWindow = new UserChangePasswordWindow
            {
                RefToTimeClockWindow = this
            };
            userChangePasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnLog_Click(object sender, RoutedEventArgs e)
        {
            UserLogWindow userLogWindow = new UserLogWindow { RefToTimeClockWindow = this };
            userLogWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
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

        public TimeClockWindow()
        {
            InitializeComponent();
            DataContext = AppState.CurrentUser;
            CheckButton();
        }

        private async void windowTimeClock_Loaded(object sender, RoutedEventArgs e)
        {
            await AppState.LoadUserTimes(AppState.CurrentUser);
        }

        private void windowTimeClock_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}