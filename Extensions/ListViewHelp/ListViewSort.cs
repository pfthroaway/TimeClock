using System.Windows.Controls;
using System.Windows.Media;

namespace Extensions.ListViewHelp
{
    /// <summary>Represents a helper for organizing ListViews.</summary>
    public class ListViewSort
    {
        /// <summary>Column being modified.</summary>
        public GridViewColumnHeader Column { get; set; }

        /// <summary>Adorner being created.</summary>
        public SortAdorner Adorner { get; set; }

        /// <summary>Color for the adorner.</summary>
        public Color AdornerColor { get; set; }

        /// <summary>Initializes a default instance of ListViewSort.</summary>
        public ListViewSort()
        {
        }

        /// <summary>Initializes an instance of ListViewSort that assigns Property values.</summary>
        /// <param name="column">Column being modified</param>
        /// <param name="adorner">Adorner being created</param>
        /// <param name="adornerColor">Color for the adorner</param>
        public ListViewSort(GridViewColumnHeader column, SortAdorner adorner, Color adornerColor)
        {
            Column = column;
            Adorner = adorner;
            AdornerColor = adornerColor;
        }
    }
}