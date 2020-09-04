
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WPFClient
{
    /// <summary>
    /// ItemsControlExtentionDemo.xaml 的交互逻辑
    /// </summary>
    public partial class ItemsControlExtentionDemo : Window
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ItemsControlExtentionDemo()
        {
            this.InitializeComponent();

            List<string> items = new List<string>();
            for (int i = 0; i < 50000; i++)
            {
                items.Add(Guid.NewGuid().ToString());
            }

            ListBox.ItemsSource = items;
        }

        private void StackPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
