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
    }
}
