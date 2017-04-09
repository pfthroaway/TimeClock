using System.Windows;

namespace TimeClock
{
    /// <summary>Interaction logic for TimeClockWindow.xaml</summary>
    public partial class TimeClockWindow
    {
        internal MainWindow RefToMainWindow { private get; set; }

        private void CheckButton()
        {
            BtnInOut.Content = AppState.CurrentUser.LoggedIn ? "_OUT" : "_IN";
        }

        #region Button-Click Methods

        private async void BtnInOut_Click(object sender, RoutedEventArgs e)
        {
            BtnInOut.IsEnabled = false;

            if (AppState.CurrentUser.LoggedIn == false)
                await AppState.LogIn(AppState.CurrentUser);
            else
                await AppState.LogOut(AppState.CurrentUser);

            BtnInOut.IsEnabled = true;
            CheckButton();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            UserChangePasswordWindow userChangePasswordWindow = new UserChangePasswordWindow
            {
                RefToTimeClockWindow = this
            };
            userChangePasswordWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            UserLogWindow userLogWindow = new UserLogWindow { RefToTimeClockWindow = this };
            userLogWindow.Show();
            this.Visibility = Visibility.Hidden;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
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

        private async void WindowTimeClock_Loaded(object sender, RoutedEventArgs e)
        {
            await AppState.LoadUserTimes(AppState.CurrentUser);
        }

        private void WindowTimeClock_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            RefToMainWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}