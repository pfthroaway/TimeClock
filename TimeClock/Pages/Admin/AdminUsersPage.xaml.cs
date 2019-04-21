using Extensions;
using Extensions.ListViewHelp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TimeClock.Classes;
using TimeClock.Classes.Entities;

namespace TimeClock.Pages.Admin
{
    /// <summary>Interaction logic for AdminUsersPage.xaml</summary>
    public partial class AdminUsersPage
    {
        internal List<User> AllUsers = new List<User>();
        internal AdminPage PreviousPage { get; set; }
        private ListViewSort _sort = new ListViewSort();
        private User _selectedUser = new User();
        private List<User> _allUsers = new List<User>();

        /// <summary>Refreshes the LVUsers's ItemSource.</summary>
        internal async Task RefreshItemsSource()
        {
            _allUsers = await AppState.LoadUsers().ConfigureAwait(false);
            Dispatcher.Invoke(() =>
           {
               LVUsers.ItemsSource = _allUsers;
               LVUsers.Items.Refresh();
           });
        }

        /// <summary>Toggles the buttons.</summary>
        /// <param name="enabled">Should buttons be enabled?</param>
        private void ToggleButtons(bool enabled)
        {
            BtnModifyTimes.IsEnabled = enabled;
            BtnModifyUser.IsEnabled = enabled;
            BtnDeleteUser.IsEnabled = enabled;
        }

        #region Click

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private async void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to delete this User?";
            if (_selectedUser.Shifts.Any())
                message += $" You will also be deleting their {_selectedUser.Shifts.Count()} shifts.";
            message += " This action cannot be undone.";
            if (AppState.YesNoNotification(message, "Time Clock"))
            {
                await AppState.DeleteUser(_selectedUser).ConfigureAwait(false);
                await RefreshItemsSource().ConfigureAwait(false);
            }
        }

        private void BtnModifyTimes_Click(object sender, RoutedEventArgs e)
        {
        }

        private void BtnModifyUser_Click(object sender, RoutedEventArgs e)
        {
            AdminManageUserPage manageUserPage = new AdminManageUserPage { OriginalUser = _selectedUser, SelectedUser = new User(_selectedUser) };
            AppState.Navigate(manageUserPage);
            manageUserPage.Reset();
        }

        private void BtnNewUser_Click(object sender, RoutedEventArgs e) => AppState.Navigate(new AdminManageUserPage { OriginalUser = new User(), SelectedUser = new User() });

        private void LVUsersColumnHeader_Click(object sender, RoutedEventArgs e) => _sort = Functions.ListViewColumnHeaderClick(sender, _sort, LVUsers, "#CCCCCC");

        private void LVUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedUser = LVUsers.SelectedIndex >= 0 ? (User)LVUsers.SelectedItem : new User();
            ToggleButtons(LVUsers.SelectedIndex >= 0);
        }

        #endregion Click

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public AdminUsersPage() => InitializeComponent();

        private async void AdminUsersPage_OnLoaded(object sender, RoutedEventArgs e) => await RefreshItemsSource().ConfigureAwait(false);

        #endregion Page-Manipulation Methods
    }
}