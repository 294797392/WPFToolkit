using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace WPFToolkit.Controls
{
    public class GridViewRowPresenterWithGridLines : GridViewRowPresenter
    {
        private static readonly Style DefaultSeparatorStyle;
        private readonly List<FrameworkElement> verticalLines = new List<FrameworkElement>();

        static GridViewRowPresenterWithGridLines()
        {
            DefaultSeparatorStyle = new Style(typeof(Rectangle));
            DefaultSeparatorStyle.Setters.Add(new Setter(Shape.FillProperty, SystemColors.ControlLightBrush));
        }

        public bool ShowGridLines
        {
            get { return (bool)GetValue(ShowGridLinesProperty); }
            set { SetValue(ShowGridLinesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowGridLines.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowGridLinesProperty =
            DependencyProperty.Register("ShowGridLines", typeof(bool), typeof(GridViewRowPresenterWithGridLines), new PropertyMetadata(false));

        public static readonly DependencyProperty GridLineWidthProperty = DependencyProperty.Register(nameof(GridLineWidth), typeof(double),
                                                                        typeof(GridViewRowPresenterWithGridLines), new PropertyMetadata(1.0));
        public double GridLineWidth
        {
            get { return (double)GetValue(GridLineWidthProperty); }
            set { SetValue(GridLineWidthProperty, value); }
        }

        public static readonly DependencyProperty GridLineBrushProperty = DependencyProperty.Register(nameof(GridLineBrush), typeof(Brush),
                                                                        typeof(GridViewRowPresenterWithGridLines), new PropertyMetadata(Brushes.Silver));
        public Brush GridLineBrush
        {
            get { return (Brush)GetValue(GridLineBrushProperty); }
            set { SetValue(GridLineBrushProperty, value); }
        }

        private IEnumerable<FrameworkElement> Children
        {
            get { return LogicalTreeHelper.GetChildren(this).OfType<FrameworkElement>(); }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var size = base.ArrangeOverride(arrangeSize);
            var children = Children.ToList();

            if (!this.ShowGridLines)
            {
                return size;
            }

            double width = this.GridLineWidth;
            if (width == 0)
            {
                return size;
            }

            this.EnsureLines(children.Count);

            for (var i = 0; i < verticalLines.Count; i++)
            {
                var child = children[i];

                if (i == verticalLines.Count - 1)
                {
                    break;
                }

                double x = child.TransformToAncestor(this).Transform(new Point(child.ActualWidth, 0)).X + child.Margin.Right;
                Rect rect = new Rect(x, -Margin.Top, width, size.Height + Margin.Top + Margin.Bottom);
                Rectangle verticalLine = verticalLines[i] as Rectangle;
                verticalLine.Measure(rect.Size);
                verticalLine.Arrange(rect);
            }
            return size;
        }

        private void EnsureLines(int count)
        {
            count = count - verticalLines.Count;
            for (var i = 0; i < count; i++)
            {
                Rectangle line = new Rectangle() { Fill = this.GridLineBrush };
                AddVisualChild(line);
                verticalLines.Add(line);
            }
        }

        protected override int VisualChildrenCount
        {
            get { return base.VisualChildrenCount + verticalLines.Count; }
        }

        protected override Visual GetVisualChild(int index)
        {
            var count = base.VisualChildrenCount;
            return index < count ? base.GetVisualChild(index) : verticalLines[index - count];
        }
    }
}
