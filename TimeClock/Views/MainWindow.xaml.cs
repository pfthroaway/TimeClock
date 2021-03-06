﻿using System.Windows;
using TimeClock.Classes;
using TimeClock.Views.Admin;

namespace TimeClock.Views
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region Click Methods

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminPasswordPage());
            MnuAdmin.IsEnabled = false;
        }

        private void MnuFileExit_Click(object sender, RoutedEventArgs e) => Close();

        #endregion Click Methods

        #region Page-Manipulation Methods

        public MainWindow()
        {
            InitializeComponent();
            AppState.MainWindow = this;
        }

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e) => await AppState.LoadAll().ConfigureAwait(false);

        #endregion Page-Manipulation Methods
    }
}