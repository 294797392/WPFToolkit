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
using WPFToolkit.DragDrop;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// DataGridDragDropUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class DragDropUserControl : UserControl, IDropHandler
    {
        public DragDropUserControl()
        {
            InitializeComponent();

            List<string> items = new List<string>();

            for (int i = 0; i < 10; i++)
            {
                items.Add(i.ToString());
            }

            DataGrid1.ItemsSource = items;
        }

        public void OnDragOver(DropInfo dropInfo)
        {
            dropInfo.Effects = DragDropEffects.Move;
        }

        public void OnDrop(DropInfo dropInfo)
        {
            if (dropInfo.Data is List<string>)
            {
                List<string> strings = dropInfo.Data as List<string>;

                foreach (string str in strings)
                {
                    Console.WriteLine(str);
                }

                Console.WriteLine("target:{0}", dropInfo.TargetItem as string);
            }
            else if (dropInfo.Data is string)
            { 
                string str = dropInfo.Data as string;

                Console.WriteLine(str);

                Console.WriteLine("target:{0}", dropInfo.TargetItem as string);
            }

            //Console.WriteLine("{0},{1}", Guid.NewGuid(), dropInfo.Data);
        }

        private void DataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(e.Source.GetType().ToString());
            MessageBox.Show(e.OriginalSource.GetType().ToString());
        }
    }
}
