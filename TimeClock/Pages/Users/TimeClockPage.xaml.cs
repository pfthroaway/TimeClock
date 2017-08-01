using System;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Users
{
    /// <summary>Interaction logic for TimeClockPage.xaml</summary>
    public partial class TimeClockPage : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        private void CheckButton() => BtnInOut.Content = AppState.CurrentUser.LoggedIn ? "_OUT" : "_IN";

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

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e) => AppState.Navigate(
            new UserChangePasswordPage());

        private void BtnLog_Click(object sender, RoutedEventArgs e) => AppState.Navigate(new UserLogPage());

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        #endregion Button-Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public TimeClockPage()
        {
            InitializeComponent();
            DataContext = AppState.CurrentUser;
            CheckButton();
        }

        private void TimeClockPage_OnLoaded(object sender, RoutedEventArgs e) => AppState.CalculateScale(Grid);

        #endregion Page-Manipulation Methods
    }
}