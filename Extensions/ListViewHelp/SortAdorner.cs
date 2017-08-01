using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Extensions.ListViewHelp
{
    /// <summary>Represents an adorner on a ListView.</summary>
    public class SortAdorner : Adorner
    {
        private static readonly Geometry AscGeometry =
            Geometry.Parse("M 0 8 L 7 0 L 14 8 Z");

        private static readonly Geometry DescGeometry =
            Geometry.Parse("M 0 0 L 7 8 L 14 0 Z");

        public ListSortDirection Direction { get; }

        private readonly Brush _adornerColor;

        public SortAdorner(UIElement element, ListSortDirection dir, Color color)
            : base(element)
        {
            Direction = dir;
            _adornerColor = new SolidColorBrush(color);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
            (
                AdornedElement.RenderSize.Width - 30,
                (AdornedElement.RenderSize.Height - 5) / 2
            );
            drawingContext.PushTransform(transform);

            Geometry geometry = Direction != ListSortDirection.Descending ? AscGeometry : DescGeometry;
            drawingContext.DrawGeometry(_adornerColor, null, geometry);
            drawingContext.Pop();
        }
    }
}