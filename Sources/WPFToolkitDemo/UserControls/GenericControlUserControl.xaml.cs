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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// GenericControlUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class GenericControlUserControl : UserControl
    {
        public GenericControlUserControl()
        {
            InitializeComponent();

            List<string> itemsSource = new List<string>();
            for (int i = 0; i < 50; i++)
            {
                itemsSource.Add(Guid.NewGuid().ToString());
            }

            ComboBoxList.ItemsSource = itemsSource;
        }
    }
}
