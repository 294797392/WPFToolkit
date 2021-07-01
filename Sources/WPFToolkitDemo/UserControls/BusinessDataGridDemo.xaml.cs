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
using WPFToolkit.MVVM;

namespace WPFToolkitDemo.UserControls
{
    public class BusinessDataItem : ItemViewModel
    {
        [WPFToolkit.Business.Controls.DataGridColumn("编号")]
        public override object ID { get => base.ID; set => base.ID = value; }

        [WPFToolkit.Business.Controls.DataGridColumn("名字")]
        public override string Name { get; set; }
    }

    /// <summary>
    /// BusinessDataGridDemo.xaml 的交互逻辑
    /// </summary>
    public partial class BusinessDataGridDemo : UserControl
    {
        public BusinessDataGridDemo()
        {
            InitializeComponent();

            this.InitializeDemo();
        }

        private void InitializeDemo()
        {
            ItemsViewModel<BusinessDataItem> items = new ItemsViewModel<BusinessDataItem>();

            for (int i = 0; i < 100; i++)
            {
                items.Add(new BusinessDataItem()
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = Guid.NewGuid().ToString()
                });
            }

            BusinessDataGrid.ItemsSource = items;
        }
    }
}
