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
    /// 定义当按钮被按下的时候的效果
    /// </summary>
    public enum YPressedEffects
    {
        /// <summary>
        /// 当鼠标移动到按钮上的时候显示一个浅颜色的背景
        /// 当鼠标按下的时候显示一个深颜色的背景
        /// </summary>
        BackgroundEffect,
    }

    public class YImageButton : Button
    {
        static YImageButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(YImageButton), new FrameworkPropertyMetadata(typeof(YImageButton)));
        }




        public Stretch Stretch
        {
            get { return (Stretch)GetValue(StretchProperty); }
            set { SetValue(StretchProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Stretch.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register("Stretch", typeof(Stretch), typeof(YImageButton), new PropertyMetadata(Stretch.None));



        public ImageSource ImageURI
        {
            get { return (ImageSource)GetValue(ImageURIProperty); }
            set { SetValue(ImageURIProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageURI.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageURIProperty =
            DependencyProperty.Register("ImageURI", typeof(ImageSource), typeof(YImageButton), new PropertyMetadata(null));





        public ImageSource ImageURIHover
        {
            get { return (ImageSource)GetValue(ImageURIHoverProperty); }
            set { SetValue(ImageURIHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageURIHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageURIHoverProperty =
            DependencyProperty.Register("ImageURIHover", typeof(ImageSource), typeof(YImageButton), new PropertyMetadata(null));




        public ImageSource ImageURIPressed
        {
            get { return (ImageSource)GetValue(ImageURIPressedProperty); }
            set { SetValue(ImageURIPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageURIPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageURIPressedProperty =
            DependencyProperty.Register("ImageURIPressed", typeof(ImageSource), typeof(YImageButton), new PropertyMetadata(null));



        /// <summary>
        /// 指定当鼠标移动到按钮上显示的背景颜色
        /// </summary>
        public Brush BackgroundHover
        {
            get { return (Brush)GetValue(BackgroundHoverProperty); }
            set { SetValue(BackgroundHoverProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundHover.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundHoverProperty =
            DependencyProperty.Register("BackgroundHover", typeof(Brush), typeof(YImageButton), new PropertyMetadata(Brushes.Transparent));



        /// <summary>
        /// 指定当鼠标按下的时候显示的背景颜色
        /// </summary>
        public Brush BackgroundPressed
        {
            get { return (Brush)GetValue(BackgroundPressedProperty); }
            set { SetValue(BackgroundPressedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundPressed.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundPressedProperty =
            DependencyProperty.Register("BackgroundPressed", typeof(Brush), typeof(YImageButton), new PropertyMetadata(Brushes.Transparent));


    }
}
