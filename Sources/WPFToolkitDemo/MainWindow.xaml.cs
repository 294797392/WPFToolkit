using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFToolkit;
using WPFToolkit.Attributes;
using WPFToolkit.Utility;
using WPFToolkit.Windows;

namespace WPFToolkitDemo
{
    public class A
    {
        [DataGridColumn("名字", DataTemplateURI = "DataTemplate1")]
        public string Name { get; set; }

        [DataGridColumn("编号", DataTemplateURI = "DataTemplate1")]
        public string ID { get; set; }
    }

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ThemeManager.ApplyDefaultTheme();

            List<A> list = new List<A>();
            for (int i = 0; i < 100; i++)
            {
                A a = new A();
                a.Name = Guid.NewGuid().ToString();
                a.ID = Guid.NewGuid().ToString();
                list.Add(a);
            }

            DataGrid1.ItemsSource = list;
        }
    }
}
