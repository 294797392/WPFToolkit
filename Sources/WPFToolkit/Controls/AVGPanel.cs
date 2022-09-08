using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 一个可以根据内容平分高度的面板
    /// </summary>
    public class AVGPanel : Panel
    {


        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(AVGPanel), new PropertyMetadata(Orientation.Vertical));



        protected override Size ArrangeOverride(Size finalSize)
        {
            switch (this.Orientation)
            {
                case Orientation.Vertical:
                    {
                        var itemHeight = finalSize.Height / Children.Count;
                        for (var i = 0; i < Children.Count; i++)
                        {
                            Children[i].Arrange(new Rect(0, i * itemHeight, finalSize.Width, itemHeight));
                        }
                        return finalSize;
                    }

                case Orientation.Horizontal:
                    {
                        var itemWidth = finalSize.Width / Children.Count;
                        for (var i = 0; i < Children.Count; i++)
                        {
                            Children[i].Arrange(new Rect(i * itemWidth, 0, itemWidth, finalSize.Height));
                        }
                        return finalSize;
                    }

                default:
                    return Size.Empty;
            }
        }
    }
}
