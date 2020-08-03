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
using WPFClient.ViewModels;
using WPFToolkit.MarkupExtensions;

namespace WPFClient
{
    /// <summary>
    /// EnumItemsSourceDemo.xaml 的交互逻辑
    /// </summary>
    public partial class EnumItemsSourceDemo : Window
    {
        #region 字段

        #endregion

        #region 属性

        #endregion

        #region 构造

        /// <summary>
        /// 构造方法
        /// </summary>
        public EnumItemsSourceDemo()
        {
            this.InitializeComponent();
        }

        #endregion

        #region 方法

        #endregion

        private void ButtonDisplaySelectedValue_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show((this.DataContext as EnumItemsSourceVM).SelectedValue.ToString());
        }
    }
}
