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
using WPFToolkit.Utils;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// CircularPanelUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class CircularPanelUserControl : UserControl
    {
        public CircularPanelUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            List<double> items = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                items.Add(new Random(i).Next(50, 200));
            }

            ListBoxCircularPanel.ItemsSource = items;
        }

        private void ListBoxCircularPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Point position = ItemsControlUtils.GetItemPosition(ListBoxCircularPanel, ListBoxCircularPanel.SelectedItem, ItemsControlUtils.ItemPositions.Center);
            MessageBox.Show(string.Format("X = {0}, Y = {1}", position.X, position.Y));
        }
    }
}
