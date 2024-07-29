using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFToolkit.Panels
{
    /// <summary>
    /// 圆形布局
    /// </summary>
    public class CirclePanel : Panel
    {
        private static log4net.ILog logger = log4net.LogManager.GetLogger("CirclePanel");




        public Brush CircleBrush
        {
            get { return (Brush)GetValue(CircleBrushProperty); }
            set { SetValue(CircleBrushProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CircleBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CircleBrushProperty =
            DependencyProperty.Register("CircleBrush", typeof(Brush), typeof(CirclePanel), new PropertyMetadata(Brushes.Black));




        /// <summary>
        /// 边框距离每个元素的距离
        /// </summary>
        public double CircleMargin
        {
            get { return (double)GetValue(CircleMarginProperty); }
            set { SetValue(CircleMarginProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CircleMargin.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CircleMarginProperty =
            DependencyProperty.Register("CircleMargin", typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(0.0D, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));



        /// <summary>
        /// 边框宽度
        /// </summary>
        public double CircleWidth
        {
            get { return (double)GetValue(CircleWidthProperty); }
            set { SetValue(CircleWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CircleWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CircleWidthProperty =
            DependencyProperty.Register("CircleWidth", typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(1.0D, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));




        /// <summary>
        /// 指定圆形的直径
        /// </summary>
        public double Diameter
        {
            get { return (double)GetValue(DiameterProperty); }
            set { SetValue(DiameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Diameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register("Diameter", typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(100.0D, FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange));


        /// <summary>
        /// 指定每个元素的角度偏移
        /// </summary>
        public double AngleOffset
        {
            get { return (double)GetValue(AngleOffsetProperty); }
            set { SetValue(AngleOffsetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AngleOffset.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleOffsetProperty =
            DependencyProperty.Register("AngleOffset", typeof(double), typeof(CirclePanel), new FrameworkPropertyMetadata(0.0D, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.AffectsMeasure));


        protected override Size MeasureOverride(Size availableSize)
        {
            if (base.Children.Count == 0)
            {
                return new Size();
            }

            foreach (UIElement item in Children)
            {
                //给定子控件的可用空间
                item.Measure(availableSize);
            }

            // 上下左右边缘的四个圆形的中心点就在可视区域的边上，所以圆形会超出边缘半个圆，所以这里基于边缘的位置再扩大一个Item的大小
            double width = base.Children[0].DesiredSize.Width;
            double height = base.Children[0].DesiredSize.Height;

            return new Size(this.Diameter + width, this.Diameter + height);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (base.Children.Count == 0)
            {
                return new Size(0, 0);
            }

            double radius = this.Diameter / 2;

            double degree = 0;
            double degreeStep = (double)360 / this.Children.Count;

            for (int i = 0; i < base.Children.Count; i++)
            {
                UIElement item = base.Children[i];

                //角度转弧度
                double angle = Math.PI * (degree + this.AngleOffset) / 180.0;

                //转换为直角坐标系 r*cos
                double cos = Math.Cos(angle);
                double x = cos * radius;
                //转换为直角坐标系 r*sin
                double sin = Math.Sin(angle);
                double y = sin * radius;
                //决定子控件的位置和大小
                item.Arrange(new Rect(x + radius, y + radius, item.DesiredSize.Width, item.DesiredSize.Height));

                degree += degreeStep;
            }

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (this.CircleWidth > 0)
            {
                double radius = this.Diameter / 2;

                double centerX = this.ActualWidth / 2;
                double centerY = this.ActualHeight / 2;
                double margin = this.CircleWidth * 2;

                dc.DrawEllipse(null, new Pen(this.CircleBrush, this.CircleWidth), new Point(centerX, centerY), radius - margin, radius - margin);
            }
        }
    }
}
