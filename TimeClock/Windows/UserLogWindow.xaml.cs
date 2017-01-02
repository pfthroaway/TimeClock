using Extensions;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace TimeClock
{
    /// <summary>Interaction logic for UserLogWindow.xaml</summary>
    public partial class UserLogWindow
    {
        private GridViewColumnHeader _listViewSortCol;
        private SortAdorner _listViewSortAdorner;
        internal TimeClockWindow RefToTimeClockWindow { private get; set; }

        #region Click Methods

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        private void lvShiftsColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            if (column != null)
            {
                string sortBy = column.Tag.ToString();
                if (_listViewSortCol != null)
                {
                    AdornerLayer.GetAdornerLayer(_listViewSortCol).Remove(_listViewSortAdorner);
                    lvShifts.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (Equals(_listViewSortCol, column) && _listViewSortAdorner.Direction == newDir)
                    newDir = ListSortDirection.Descending;

                _listViewSortCol = column;
                _listViewSortAdorner = new SortAdorner(_listViewSortCol, newDir);
                AdornerLayer.GetAdornerLayer(_listViewSortCol).Add(_listViewSortAdorner);
                lvShifts.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            }
        }

        #endregion Click Methods

        #region Window-Manipulation Methods

        private void CloseWindow()
        {
            this.Close();
        }

        public UserLogWindow()
        {
            InitializeComponent();
            lvShifts.ItemsSource = AppState.CurrentUserTimes;
        }

        private void windowUserLog_Closing(object sender, CancelEventArgs e)
        {
            RefToTimeClockWindow.Show();
        }

        #endregion Window-Manipulation Methods
    }
}