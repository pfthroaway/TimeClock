using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeClock.Classes;

namespace TimeClock.Pages.Admin
{
    /// <summary>Interaction logic for AdminRoles.xaml</summary>
    public partial class AdminRolesPage : INotifyPropertyChanged
    {
        private List<string> _allRoles = new List<string>();

        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        /// <summary>Updates the data binding for this Page.</summary>
        private void UpdateBindings()
        {
            LstRoles.Items.Clear();
            _allRoles = AppState.AllRoles;
            LstRoles.ItemsSource = _allRoles;
            LstRoles.Items.Refresh();
        }

        #endregion Data-Binding

        /// <summary>Checks which Buttons should be enabled.</summary>
        private void CheckButtons()
        {
            BtnNewRole.IsEnabled = TxtNewRole.Text.Length > 0;
            BtnDeleteRole.IsEnabled = LstRoles.SelectedIndex >= 0;
            BtnModifyRole.IsEnabled = LstRoles.SelectedIndex >= 0;
        }

        #region Click

        private void BtnNewRole_Click(object sender, RoutedEventArgs e)
        {
            string newRole = AppState.DisplayInputNotification("What name would you like your new role to be?", "Time Clock");
            if (newRole.Length > 0)
            {
                AppState.AllRoles.Add(newRole);
                AppState.AllRoles.Sort();
                UpdateBindings();
            }
        }

        private void BtnDeleteRole_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.YesNoNotification("Are you sure you want to delete this role? This action cannot be undone.", "Time Clock"))
                AppState.AllRoles.RemoveAt(LstRoles.SelectedIndex);
        }

        private void BtnModifyRole_Click(object sender, RoutedEventArgs e)
        {
            string originalRole = LstRoles.SelectedItem.ToString();
            string modifyRole = AppState.DisplayInputNotification("What role would you like to change this name to be?", "Time Clock", originalRole);
            if (modifyRole.Length > 0 && modifyRole != originalRole)
            {
                AppState.AllRoles.Remove(originalRole);
                AppState.AllRoles.Add(modifyRole);
                AppState.AllRoles.Sort();
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
        }

        #endregion Page Manipulation Methods
    }
}