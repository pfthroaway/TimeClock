using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TimeClock.Classes;

namespace TimeClock.Views.Admin
{
    /// <summary>Interaction logic for AdminRoles.xaml</summary>
    public partial class AdminRolesPage : INotifyPropertyChanged
    {
        private ObservableCollection<string> _allRoles = new ObservableCollection<string>();

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>Updates the data binding for this Page.</summary>
        private void UpdateBindings()
        {
            Dispatcher.Invoke(() =>
            {
                _allRoles = new ObservableCollection<string>(AppState.AllRoles);
                LstRoles.ItemsSource = _allRoles;
                LstRoles.Items.Refresh();
            });
        }

        #endregion Data-Binding

        /// <summary>Checks which Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            BtnDeleteRole.IsEnabled = LstRoles.SelectedIndex >= 0;
            BtnModifyRole.IsEnabled = LstRoles.SelectedIndex >= 0;
        }

        #region Click

        private async void BtnNewRole_Click(object sender, RoutedEventArgs e)
        {
            string newRole = AppState.InputDialog("What name would you like your new role to have?", "Time Clock");
            if (newRole.Length > 0)
            {
                if (!AppState.AllRoles.Contains(newRole))
                {
                    await AppState.AddNewRole(newRole).ConfigureAwait(false);
                    UpdateBindings();
                }
                else
                    AppState.DisplayNotification("That role already exists.", "Time Clock");
            }
        }

        private async void BtnDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.YesNoNotification("Are you sure you want to delete this role? This action cannot be undone.", "Time Clock"))
            {
                await AppState.DeleteRole(LstRoles.SelectedItem.ToString()).ConfigureAwait(false);
                UpdateBindings();
            }
        }

        private async void BtnModifyRole_Click(object sender, RoutedEventArgs e)
        {
            string originalRole = LstRoles.SelectedItem.ToString();
            string modifyRole = AppState.InputDialog("What role would you like to change this name to be?", "Time Clock", originalRole);
            if (modifyRole.Length > 0 && modifyRole != originalRole)
            {
                await AppState.ModifyRole(originalRole, modifyRole).ConfigureAwait(false);
                UpdateBindings();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e) => AppState.GoBack();

        private void TxtNewRole_TextChanged(object sender, TextChangedEventArgs e) => CheckButtons();

        private void LstRoles_SelectionChanged(object sender, SelectionChangedEventArgs e) => CheckButtons();

        #endregion Click

        #region Page Manipulation Methods

        public AdminRolesPage() => InitializeComponent();

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBindings();
        }

        #endregion Page Manipulation Methods
    }
}