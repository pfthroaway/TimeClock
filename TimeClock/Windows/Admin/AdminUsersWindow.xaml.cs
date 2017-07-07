using Extensions;
using Extensions.ListViewHelp;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Windows.Admin
{
    /// <summary>Interaction logic for AdminUsersWindow.xaml</summary>
    public partial class AdminUsersWindow
    {
        internal List<User> AllUsers = new List<User>();
        internal AdminWindow PreviousWindow { get; set; }
        private ListViewSort _sort = new ListViewSort();
        private User _selectedUser = new User();

        internal void RefreshItemsSource()
        {
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private async void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to delete this User?";
            if (_selectedUser.Shifts.Any())
                message += $" You will also be deleting their {_selectedUser.Shifts.Count()} shifts.";
            message += " This action cannot be undone.";
            if (AppState.YesNoNotification(message, "Time Clock", this))
                await AppState.DeleteUser(_selectedUser);
        }

        private void BtnModifyTimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnModifyUser_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LVUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            _sort = Functions.ListViewColumnHeaderClick(sender, _sort, LVUsers, "#BDC7C1");
        }

        private void LVUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = LVUsers.SelectedIndex >= 0 ? (User)LVUsers.SelectedItem : new User();
        }

        #endregion Click

        #region Window-Manipulation Methods

        /// <summary>Closes the Window.</summary>
        private void CloseWindow()
        {
            Close();
        }

        public AdminUsersWindow()
        {
            InitializeComponent();
        }

        private void WindowAdminUsers_Closing(object sender, CancelEventArgs e)
        {
            PreviousWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}