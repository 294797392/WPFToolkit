using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFToolkit.Windows
{
    /// <summary>
    /// SplashScreenWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SplashScreenWindow : Window
    {
        private Storyboard fadeOut;

        public int FadeInMilliseconds
        {
            get { return (int)GetValue(FadeInMillisecondsProperty); }
            set { SetValue(FadeInMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeInMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeInMillisecondsProperty =
            DependencyProperty.Register("FadeInMilliseconds", typeof(int), typeof(SplashScreenWindow), new PropertyMetadata(100));

        public int FadeOutMilliseconds
        {
            get { return (int)GetValue(FadeOutMillisecondsProperty); }
            set { SetValue(FadeOutMillisecondsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FadeOutMilliseconds.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FadeOutMillisecondsProperty =
            DependencyProperty.Register("FadeOutMilliseconds", typeof(int), typeof(SplashScreenWindow), new PropertyMetadata(100));

        public UserControl ContentControl
        {
            get { return (UserControl)GetValue(ContentControlProperty); }
            set { SetValue(ContentControlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentControl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentControlProperty =
            DependencyProperty.Register("ContentControl", typeof(UserControl), typeof(SplashScreenWindow), new PropertyMetadata(null));

        public SplashScreenWindow()
        {
            InitializeComponent();

            this.fadeOut = this.FindResource("StoryboardFadeOut") as Storyboard;
            fadeOut.Completed += StoryboardFadeOut_Completed;
        }

        public new void Close()
        {
            this.fadeOut.Begin(this);
        }

        private void StoryboardFadeOut_Completed(object sender, EventArgs e)
        {
            this.fadeOut.Completed -= this.StoryboardFadeOut_Completed;
            base.Close();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
