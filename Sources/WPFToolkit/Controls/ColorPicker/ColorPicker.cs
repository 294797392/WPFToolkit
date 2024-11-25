using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace WPFToolkit.Controls
{
    public delegate void SelectionChangingEventHandler(object sender, SelectionChangingEventArgs e);

    public class SelectionChangingEventArgs : RoutedEventArgs
    {
        public Brush SelectedBrush { get; internal set; }

        public SelectionChangingEventArgs()
        {
            this.RoutedEvent = ColorPicker.SelectionChangingEvent;
        }
    }

    /// <summary>
    /// 颜色选择器
    /// 
    /// 色相是分段线性变化的，因此，我们可以利用LinearGradientBrush来绘制颜色条
    /// 
    /// H(Hue) 为色相, 取值范围：0-360°，就是颜色名称，例如“红色”、“蓝色”。
    /// S(Saturation) 为饱和度， 取值范围：0 - 1(0% - 100%), 即颜色的纯度。
    /// B(Brightness)为明度, 取值范围：0 - 1(0% - 100%)，颜色的明亮程度。
    /// 
    /// HSB 的 B（明度）控制纯色中混入黑色的量，越往上，值越大，黑色越少，颜色明度越高。
    /// HSB 的 S（饱和度）控制纯色中混入白色的量，越往右，值越大，白色越少，颜色纯度越高。
    /// 
    /// HSV和HSB是一样的
    /// 
    /// https://www.cnblogs.com/nabian/p/9267646.html
    /// </summary>
    [TemplatePart(Name = "PART_ColorBar", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ColorGrid", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ColorText", Type = typeof(TextBox))]
    [TemplatePart(Name = "PART_Ball", Type = typeof(Border))]
    [TemplatePart(Name = "PART_OKButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_ClearButton", Type = typeof(Button))]
    [ContentProperty("Content")]
    [DefaultProperty("Content")]
    public class ColorPicker : ContentControl
    {
        #region 类变量

        private static Color DefaultColor = Colors.Blue;

        #endregion

        #region 实例变量

        private Canvas colorBar;
        private Grid colorGrid;
        private Border ball;

        private Button okButton;
        private Button clearButton;

        private LinearGradientBrush colorBarBrush;
        private GradientStop gradientStop;

        private double h;
        private double s;
        private double b;

        #endregion

        #region 依赖属性

        public bool IsOpened
        {
            get { return (bool)GetValue(IsOpenedProperty); }
            set { SetValue(IsOpenedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOpened.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOpenedProperty =
            DependencyProperty.Register("IsOpened", typeof(bool), typeof(ColorPicker), new PropertyMetadata(false, IsOpenedPropertyChangedCallback));


        public Brush SelectedBrush
        {
            get { return (Brush)GetValue(SelectedBrushProperty); }
            set { SetValue(SelectedBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedBrushProperty =
            DependencyProperty.Register("SelectedBrush", typeof(Brush), typeof(ColorPicker), new PropertyMetadata(null));


        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker), new PropertyMetadata(DefaultColor, SelectedColorPropertyChangedCallback));

        #endregion

        #region 路由事件

        /// <summary>
        ///     An event fired when the selection changes.
        /// </summary>
        public static readonly RoutedEvent SelectionChangingEvent = EventManager.RegisterRoutedEvent(
            "SelectionChanging", RoutingStrategy.Bubble, typeof(SelectionChangingEventHandler), typeof(ColorPicker));

        /// <summary>
        ///     An event fired when the selection changes.
        /// </summary>
        public event SelectionChangingEventHandler SelectionChanging
        {
            add { AddHandler(SelectionChangingEvent, value); }
            remove { RemoveHandler(SelectionChangingEvent, value); }
        }

        #endregion

        #region 构造方法

        static ColorPicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorPicker), new FrameworkPropertyMetadata(typeof(ColorPicker)));
        }

        public ColorPicker()
        {
            this.SelectedBrush = new SolidColorBrush(DefaultColor);
        }

        #endregion

        #region 实例方法

        /// <summary>
        /// https://stackoverflow.com/questions/9650049/get-color-in-specific-location-on-gradient
        /// </summary>
        /// <param name="gsc"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private static Color GetRelativeColor(GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                {
                    before = gs;
                }
                if (gs.Offset > offset && gs.Offset < after.Offset)
                {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.colorBar = this.Template.FindName("PART_ColorBar", this) as Canvas;
            this.colorBar.MouseMove += ColorBar_MouseMove;
            this.colorBarBrush = this.colorBar.Background as LinearGradientBrush;
            this.gradientStop = this.Template.FindName("GradientStop", this) as GradientStop;

            this.colorGrid = this.Template.FindName("PART_ColorGrid", this) as Grid;
            this.colorGrid.MouseMove += ColorGrid_MouseMove;
            this.colorGrid.MouseLeftButtonDown += ColorGrid_MouseLeftButtonDown;

            this.okButton = this.Template.FindName("PART_OKButton", this) as Button;
            this.okButton.Click += OkButton_Click;
            this.clearButton = this.Template.FindName("PART_ClearButton", this) as Button;
            this.clearButton.Click += ClearButton_Click;

            this.ball = this.Template.FindName("PART_Ball", this) as Border;

            ColorConverter.RGB2HSB(this.SelectedColor, out this.h, out this.s, out this.b);
            this.gradientStop.Color = this.SelectedColor;

            this.ResetBallPosition(this.s, this.b);
        }

        private void HandleColorGridMouseEvent(MouseEventArgs e)
        {
            Point relativePos = e.GetPosition(this.colorGrid);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 观察Photoshop的HSB值，从上到下，b是100 - 0。b决定了颜色的亮度。白色最量，黑色最暗
                // 从左到右，s是0 - 100，s决定了颜色的饱和度。饱和度越高色彩越鲜艳
                // h值不变，说明决定颜色的值是h

                ColorConverter.RGB2HSB(this.SelectedColor, out this.h, out this.s, out this.b);

                this.b = 1 - (relativePos.Y / this.colorGrid.ActualHeight);
                this.s = (relativePos.X / this.colorGrid.ActualWidth);

                Color color;
                ColorConverter.HSB2RGB(this.h, this.s, this.b, out color);

                this.SelectedColor = color;

                this.ResetBallPosition(this.s, this.b);

                SelectionChangingEventArgs selectionChanging = new SelectionChangingEventArgs();
                selectionChanging.Source = this;
                selectionChanging.SelectedBrush = this.SelectedBrush;

                this.RaiseEvent(selectionChanging);
            }
        }

        private void ResetBallPosition(double s, double b)
        {
            double halfWidth = this.ball.Width / 2;
            double halfHeight = this.ball.Height / 2;
            Canvas.SetLeft(this.ball, s * this.colorGrid.ActualWidth - halfWidth);
            Canvas.SetTop(this.ball, (1 - b) * this.colorGrid.ActualHeight - halfHeight);
        }

        #endregion

        #region 事件处理器

        private void ColorGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.HandleColorGridMouseEvent(e);
        }

        private void ColorGrid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            this.HandleColorGridMouseEvent(e);
        }

        private void ColorBar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point relativePos = e.GetPosition(this.colorBar);

            double percent = relativePos.Y / this.colorBar.ActualHeight;

            this.gradientStop.Color = GetRelativeColor(this.colorBarBrush.GradientStops, percent);

            double s, b;
            ColorConverter.RGB2HSB(this.gradientStop.Color, out this.h, out s, out b);

            // 按照当前的饱和度的明亮度去取值
            Color color;
            ColorConverter.HSB2RGB(this.h, this.s, this.b, out color);

            this.SelectedColor = color;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ColorConverter.RGB2HSB(DefaultColor, out this.h, out this.s, out this.b);
            this.SelectedColor = DefaultColor;
            this.gradientStop.Color = DefaultColor;
            this.ResetBallPosition(this.s, this.b);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.IsOpened = false;
        }

        #endregion

        #region 依赖属性回调

        private void OnSelectedColorPropertyChanged(object oldValue, object newValue)
        {
            if (newValue == null && oldValue != null)
            {
                // 清空颜色
            }
            else
            {
                Color color = (Color)newValue;
                if (this.SelectedBrush == null)
                {
                    this.SelectedBrush = new SolidColorBrush(color);
                }
                else
                {
                    this.SelectedBrush.SetValue(SolidColorBrush.ColorProperty, color);
                }
            }
        }

        private static void SelectedColorPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker me = d as ColorPicker;
            me.OnSelectedColorPropertyChanged(e.OldValue, e.NewValue);
        }

        private void OnIsOpenedPropertyChanged(object oldValue, object newValue)
        {
            if (newValue == null)
            {
                return;
            }

            bool isOpened = (bool)newValue;

            if (isOpened)
            {
                Mouse.Capture(this, CaptureMode.SubTree);
            }
            else 
            {
                Mouse.Capture(null);
            }
        }

        private static void IsOpenedPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColorPicker me = d as ColorPicker;
            me.OnIsOpenedPropertyChanged(e.OldValue, e.NewValue);
        }

        #endregion

        /// <summary>
        /// When we have capture, all clicks off the popup will have the combobox as the OriginalSource
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (Mouse.Captured == this && e.OriginalSource == this)
            {
                this.IsOpened = false;
            }
        }
    }

    /// <summary>
    /// 颜色结构体转文本
    /// </summary>
    public class ColorTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            Color color = (Color)value;

            return color.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
