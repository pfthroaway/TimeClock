using Extensions;
using Extensions.ListViewHelp;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Pages.Users
{
    /// <summary>Interaction logic for UserLogPage.xaml</summary>
    public partial class UserLogPage : INotifyPropertyChanged
    {
        #region Data-Binding

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion Data-Binding

        private ListViewSort _sort = new ListViewSort();

        #region Click Methods

        private void BtnBack_Click(object sender, RoutedEventArgs e) => ClosePage();

        private void LVShiftsColumnHeader_Click(object sender, RoutedEventArgs e) => _sort =
            Functions.ListViewColumnHeaderClick(sender, _sort, LVShifts, "#CCCCCC");

        #endregion Click Methods

        #region Page-Manipulation Methods

        /// <summary>Closes the Page.</summary>
        private void ClosePage() => AppState.GoBack();

        public UserLogPage()
        {
            InitializeComponent();
            LVShifts.ItemsSource = AppState.CurrentUser.Shifts;
        }

        #endregion Page-Manipulation Methods
    }
}