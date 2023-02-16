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
using WPFToolkit.Attributes;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// DataGridColumnAttributeUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DataGridUtilsUserControl : UserControl
    {
        public class DataGridItem
        {
            [DataGridColumn("名字")]
            public string Name { get; set; }

            [DataGridColumn("编号")]
            public string ID { get; set; }

            public DataGridItem() 
            {
                this.Name = Guid.NewGuid().ToString();
                this.ID = Guid.NewGuid().ToString();
            }
        }

        public DataGridUtilsUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            List<DataGridItem> items = new List<DataGridItem>();

            for (int i = 0; i < 100; i++)
            {
                items.Add(new DataGridItem());
            }

            DataGrid1.ItemsSource = items;
        }

    }
}
