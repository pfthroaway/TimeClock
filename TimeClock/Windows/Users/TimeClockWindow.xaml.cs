using System;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Windows.Users
{
    /// <summary>Interaction logic for TimeClockWindow.xaml</summary>
    public partial class TimeClockWindow
    {
        internal MainWindow PreviousWindow { private get; set; }

        private void CheckButton()
        {
            BtnInOut.Content = AppState.CurrentUser.LoggedIn ? "_OUT" : "_IN";
        }

        #region Button-Click Methods

        private async void BtnInOut_Click(object sender, RoutedEventArgs e)
        {
            BtnInOut.IsEnabled = false;

            if (AppState.CurrentUser.LoggedIn == false)
            {
                Shift newShift = new Shift(AppState.CurrentUser.ID, DateTime.Now);
                if (await AppState.LogIn(newShift))
                {
                    AppState.CurrentUser.AddShift(newShift);
                    AppState.CurrentUser.LoggedIn = true;
                }
            }
            else
            {
                Shift currentShift = new Shift(AppState.CurrentUser.GetMostRecentShift()) { ShiftEnd = DateTime.Now };
                if (await AppState.LogOut(currentShift))
                {
                    AppState.CurrentUser.ModifyShift(AppState.CurrentUser.GetMostRecentShift(), currentShift);
                    AppState.CurrentUser.LoggedIn = false;
                }
            }

            BtnInOut.IsEnabled = true;
            CheckButton();
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            UserChangePasswordWindow userChangePasswordWindow = new UserChangePasswordWindow
            {
                PreviousWindow = this
            };
            userChangePasswordWindow.Show();
            Visibility = Visibility.Hidden;
        }

        private void BtnLog_Click(object sender, RoutedEventArgs e)
        {
            UserLogWindow userLogWindow = new UserLogWindow { PreviousWindow = this };
            userLogWindow.Show();
            Visibility = Visibility.Hidden;
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
            Close();
        }

        public TimeClockWindow()
        {
            InitializeComponent();
            DataContext = AppState.CurrentUser;
            CheckButton();
        }

        private void WindowTimeClock_Closing(object sender, CancelEventArgs e)
        {
            PreviousWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}