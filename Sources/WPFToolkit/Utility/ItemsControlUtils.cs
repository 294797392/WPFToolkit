using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFToolkit.Utils
{
    public class SelectionCompletedEventArgs : RoutedEventArgs
    {
        public List<object> SelectedItems { get; private set; }

        public List<DependencyObject> SelectedObjects { get; private set; }

        public SelectionCompletedEventArgs(RoutedEvent routedEvent, object source, List<object> selectedItems, List<DependencyObject> selectedElements) : base(routedEvent, source)
        {
            this.SelectedItems = selectedItems;
            this.SelectedObjects = selectedElements;
        }
    }

    /// <summary>
    /// 专门用来绘制选框的控件
    /// </summary>
    public class SelectionArea : Control
    {
        #region SelectionCompleted路由事件

        public static readonly RoutedEvent SelectionCompletedRoutedEvent = EventManager.RegisterRoutedEvent("SelectionCompleted", RoutingStrategy.Bubble, typeof(EventHandler<SelectionCompletedEventArgs>), typeof(SelectionArea));

        public event Action<object, SelectionCompletedEventArgs> SelectionCompleted
        {
            add { this.AddHandler(SelectionCompletedRoutedEvent, value); }
            remove { this.RemoveHandler(SelectionCompletedRoutedEvent, value); }
        }

        #endregion

        private static Brush BackgroundBrush = new SolidColorBrush(Color.FromRgb(90, 182, 226)) { Opacity = 0.5 };
        private static Pen BorderPen = new Pen(new SolidColorBrush(Color.FromRgb(38, 160, 218)), 1);

        private Rect selectionRect;

        public SelectionArea()
        {
            this.IsHitTestVisible = false;
        }

        internal void Update(Rect selectionRect)
        {
            this.selectionRect = selectionRect;
            this.InvalidateVisual();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            drawingContext.DrawRectangle(BackgroundBrush, BorderPen, this.selectionRect);
        }
    }

    /// <summary>
    /// 对ItemsControl进行扩展
    /// 在实现SelectionArea属性的时候，意外的解决了一个困扰我多年的问题。。。如何实现在框选的时候滚动条自动往下滚动？
    /// 原来这个功能根本不用我们自己去实现，WPF已经帮我们实现好了，只要在鼠标按下的时候，调用控件的CaptureMouse即可。。。。
    /// 
    /// 注意，如果ItemsControl开启了虚拟化支持，那么请设置：VirtualizingPanel.ScrollUnit="Pixel"，不然获取不到ScrollViewer所滚动的距离
    /// </summary>
    public class ItemsControlUtils : DependencyObject
    {
        public enum ItemPositions
        {
            LeftTop,
            Center
        }

        #region AutoBringIntoView

        public static bool GetAutoBringIntoView(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoBringIntoViewProperty);
        }

        public static void SetAutoBringIntoView(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoBringIntoViewProperty, value);
        }

        /// <summary>
        /// 当列表项被选中的时候，是否自动让滚动条滚动到列表项可见区域
        /// </summary>
        public static readonly DependencyProperty AutoBringIntoViewProperty = DependencyProperty.RegisterAttached("AutoBringIntoView", typeof(bool), typeof(ItemsControlUtils), new PropertyMetadata(false, AutoBringIntoViewChangedCallback));

        private static void AutoBringIntoViewChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Selector)
            {
                // DataGrid和ListBox都继承自Selector

                Selector selector = d as Selector;

                bool value = (bool)e.NewValue;
                if (value)
                {
                    selector.SelectionChanged += Selector_SelectionChanged;
                }
                else
                {
                    selector.SelectionChanged -= Selector_SelectionChanged;
                }
            }
            else
            {

            }
        }

        private static void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = sender as Selector;
            ScrollViewer scrollViewer = selector.FindVisualChild<ScrollViewer>();
            if (scrollViewer != null)
            {
                if (selector is ListBox)
                {
                    ListBox listBox = selector as ListBox;
                    listBox.ScrollIntoView(selector.SelectedItem);
                }
                else if (selector is DataGrid)
                {
                    DataGrid dataGrid = selector as DataGrid;
                    dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                }
            }
        }

        #endregion

        #region SelectionArea

        private static Point startPosition;
        private static ScrollViewer scrollViewer;
        private static bool isMouseDown = false;
        private static SelectionArea selectionArea;

        public static SelectionArea GetSelectionArea(DependencyObject obj)
        {
            return (SelectionArea)obj.GetValue(SelectionAreaProperty);
        }

        public static void SetSelectionArea(DependencyObject obj, Panel value)
        {
            obj.SetValue(SelectionAreaProperty, value);
        }

        public static readonly DependencyProperty SelectionAreaProperty = DependencyProperty.RegisterAttached("SelectionArea", typeof(SelectionArea), typeof(ItemsControlUtils), new PropertyMetadata(null, SelectionAreaPropertyChangedCallback));

        private static void SelectionAreaPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement itemsControl = d as UIElement;

            SelectionArea selectionArea = e.NewValue as SelectionArea;

            if (selectionArea == null)
            {
                itemsControl.MouseLeftButtonDown -= ItemsControl_MouseLeftButtonDown;
                itemsControl.MouseMove -= ItemsControl_MouseMove;
                itemsControl.MouseLeftButtonUp -= ItemsControl_MouseLeftButtonUp;
            }
            else
            {
                itemsControl.MouseLeftButtonDown += ItemsControl_MouseLeftButtonDown;
                itemsControl.MouseMove += ItemsControl_MouseMove;
                itemsControl.MouseLeftButtonUp += ItemsControl_MouseLeftButtonUp;
            }
        }

        private static void ItemsControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ItemsControl itemsControl = sender as ItemsControl;
            isMouseDown = false;
            SelectionArea selectionArea = GetSelectionArea(sender as DependencyObject);
            selectionArea.Update(Rect.Empty);
            selectionArea.Visibility = Visibility.Collapsed;
            itemsControl.ReleaseMouseCapture();

            List<object> selectedItems = new List<object>();
            List<DependencyObject> selectedElements = new List<DependencyObject>();

            int numItem = itemsControl.Items.Count;
            for (int i = 0; i < numItem; i++)
            {
                DependencyObject element = itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                if (Selector.GetIsSelected(element))
                {
                    selectedItems.Add(element.GetValue(FrameworkElement.DataContextProperty));
                    selectedElements.Add(element);
                }
            }

            selectionArea.RaiseEvent(new SelectionCompletedEventArgs(SelectionArea.SelectionCompletedRoutedEvent, selectionArea, selectedItems, selectedElements));

            if (scrollViewer != null)
            {
                scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
                scrollViewer = null;
            }
        }

        private static void ItemsControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!isMouseDown)
            {
                return;
            }

            ItemsControl itemsControl = sender as ItemsControl;

            if (scrollViewer == null)
            {
                // 如果用户重写了模板，有可能找不到ScrollViewer
                scrollViewer = itemsControl.FindVisualChild<ScrollViewer>();
                if (scrollViewer != null)
                {
                    scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;
                }
            }

            // 画SelectionArea
            Point currentPosition = e.GetPosition(sender as IInputElement);
            Rect selectionRect = new Rect(startPosition, currentPosition);
            selectionArea = GetSelectionArea(sender as DependencyObject);
            selectionArea.Update(selectionRect);

            // 实时选中Item
            int numItem = itemsControl.Items.Count;
            for (int i = 0; i < numItem; i++)
            {
                FrameworkElement visual = itemsControl.ItemContainerGenerator.ContainerFromIndex(i) as FrameworkElement;
                if (visual == null)
                {
                    // 如果使用了虚拟化的Panel（VirtualizingPanel），那么没渲染到界面上的Item是获取不到的
                    // 运行到此处说明该Item没有渲染到界面上，直接跳过就可以
                    continue;
                }

                // 获取Item的中点
                GeneralTransform transform = visual.TransformToAncestor(itemsControl);
                Point visualCenter = transform.Transform(new Point(visual.Width / 2, visual.Height / 2));

                // 判断当前的SelectionArea是否包含了Item的中点
                if (selectionRect.Contains(visualCenter))
                {
                    // 如果包含了那么把Item设置为选中状态
                    Selector.SetIsSelected(visual, true);
                }
                else
                {
                    // 如果没包含那么设置成非选中状态
                    Selector.SetIsSelected(visual, false);
                }

                //Console.WriteLine("x = {0}, y = {1}", itemPos.X, itemPos.Y);
            }
        }

        private static void ItemsControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            UIElement itemsControl = sender as UIElement;
            startPosition = e.GetPosition(itemsControl);
            isMouseDown = true;
            selectionArea = GetSelectionArea(itemsControl);
            selectionArea.Visibility = Visibility.Visible;
            itemsControl.CaptureMouse();
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (selectionArea != null)
            {
                startPosition.Y = startPosition.Y - e.VerticalChange;
                startPosition.X = startPosition.X - e.HorizontalChange;

                Point cursorPosition = Mouse.GetPosition(sender as IInputElement);
                Rect selectionRect = new Rect(startPosition, cursorPosition);
                selectionArea.Update(selectionRect);

                //Console.WriteLine("startPosition.Y = {0}, startPosition.X = {1}", startPosition.Y, startPosition.X);
            }
        }

        #endregion

        /// <summary>
        /// 获取ItemsControl里的某个Item的位置
        /// </summary>
        /// <param name="itemsControl">要获取的ItemControl</param>
        /// <param name="item">要获取的Item的DataContext</param>
        /// <param name="itemPosition">要获取Item的哪个位置</param>
        /// <returns>如果获取失败则会返回NAN</returns>
        public static Point GetItemPosition(ItemsControl itemsControl, object item, ItemPositions itemPosition)
        {
            DependencyObject dependencyObject = itemsControl.ItemContainerGenerator.ContainerFromItem(item);
            if (dependencyObject == null)
            {
                return new Point(double.NaN, double.NaN);
            }

            FrameworkElement frameworkElement = dependencyObject as FrameworkElement;

            switch (itemPosition)
            {
                case ItemPositions.LeftTop:
                    {
                        return frameworkElement.TranslatePoint(new Point(), itemsControl);
                    }

                case ItemPositions.Center:
                    {
                        // 获取中心点相对于ItemsControl的位置
                        return frameworkElement.TranslatePoint(new Point(frameworkElement.ActualWidth / 2, frameworkElement.ActualHeight / 2), itemsControl);
                    }

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
