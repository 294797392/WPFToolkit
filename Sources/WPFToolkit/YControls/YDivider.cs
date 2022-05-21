using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFToolkit.YControls
{
    /// <summary>
    /// 分割线控件
    /// </summary>
    public class YDivider : Control
    {
        private const int DefaultMargin = 0;

        /// <summary>
        /// 分割线的颜色
        /// </summary>
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(YDivider), new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 分割线的方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(YDivider), new FrameworkPropertyMetadata(Orientation.Horizontal, FrameworkPropertyMetadataOptions.AffectsRender, OrientationPropertyChangedCallback));


        /// <summary>
        /// 分割线的粗细
        /// </summary>
        public int Thickness
        {
            get { return (int)GetValue(ThicknessProperty); }
            set { SetValue(ThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Thickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThicknessProperty =
            DependencyProperty.Register("Thickness", typeof(int), typeof(YDivider), new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsRender));




        public YDivider()
        {
            this.MinHeight = this.Thickness;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            Pen pen = new Pen(this.Color, this.Thickness);

            switch (Orientation)
            {
                case Orientation.Horizontal:
                    {
                        drawingContext.DrawLine(pen, new Point(DefaultMargin, this.ActualHeight / 2), new Point(this.ActualWidth - DefaultMargin, this.ActualHeight / 2));

                        break;
                    }

                case Orientation.Vertical:
                    {
                        drawingContext.DrawLine(pen, new Point(this.ActualWidth / 2, DefaultMargin), new Point(this.ActualWidth / 2, this.ActualHeight - DefaultMargin));

                        break;
                    }
            }
        }

        private void OnOrientationPropertyChanged(object oldValue, object newValue)
        {
            Orientation orientation = (Orientation)newValue;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    {
                        this.MinHeight = this.Thickness;
                        break;
                    }

                case Orientation.Vertical:
                    {
                        this.MinWidth = this.Thickness;
                        break;
                    }
            }
        }

        private static void OrientationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            YDivider me = d as YDivider;
            me.OnOrientationPropertyChanged(e.OldValue, e.NewValue);
        }
    }
}

