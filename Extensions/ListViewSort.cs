using System.Windows.Controls;

namespace Extensions
{
    public class ListViewSort
    {
        public GridViewColumnHeader Column { get; set; }

        public SortAdorner Adorner { get; set; }

        /// <summary>Initializes a default instance of ListViewSort.</summary>
        public ListViewSort()
        {
        }

        /// <summary>Initializes an instance of ListViewSort that assigns Property values.</summary>
        /// <param name="column"></param>
        /// <param name="adorner"></param>
        public ListViewSort(GridViewColumnHeader column, SortAdorner adorner)
        {
            Column = column;
            Adorner = adorner;
        }
    }
}