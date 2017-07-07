using Extensions;
using Extensions.ListViewHelp;
using System.ComponentModel;
using System.Windows;
using TimeClock.Classes;

namespace TimeClock.Windows.Users
{
    /// <summary>Interaction logic for UserLogWindow.xaml</summary>
    public partial class UserLogWindow
    {
        private ListViewSort _sort = new ListViewSort();
        internal TimeClockWindow PreviousWindow { private get; set; }

        #region Click Methods

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void LVShiftsColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            _sort = Functions.ListViewColumnHeaderClick(sender, _sort, LVShifts, "#BDC7C1");
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        private void CloseWindow()
        {
            Close();
        }

        public UserLogWindow()
        {
            InitializeComponent();
            LVShifts.ItemsSource = AppState.CurrentUser.Shifts;
        }

        private void WindowUserLog_Closing(object sender, CancelEventArgs e)
        {
            PreviousWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}