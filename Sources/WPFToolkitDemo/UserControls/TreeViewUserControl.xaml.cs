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
    /// <summary>
    /// TreeViewUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TreeViewUserControl : UserControl
    {
        private TreeViewModel<TreeViewModelContext> treeViewModel;

        public TreeViewUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            this.treeViewModel = new TreeViewModel<TreeViewModelContext>();

            for (int i = 0; i < 3; i++)
            {
                TreeNodeViewModel node = new TreeNodeViewModel(this.treeViewModel.Context);
                node.ID = Guid.NewGuid().ToString();
                node.Name = i.ToString();

                for (int j = 0; j < 3; j++)
                {
                    TreeNodeViewModel node2 = new TreeNodeViewModel(this.treeViewModel.Context);
                    node2.ID = Guid.NewGuid().ToString();
                    node2.Name = string.Format("{0}-{1}", i, j);

                    for (int k = 0; k < 3; k++)
                    {
                        TreeNodeViewModel node3 = new TreeNodeViewModel(this.treeViewModel.Context);
                        node3.ID = Guid.NewGuid().ToString();
                        node3.Name = string.Format("{0}-{1}-{2}", i, j, k);
                        node2.Add(node3);
                    }

                    node.Add(node2);
                }

                this.treeViewModel.Add(node);
            }


            TreeView1.ItemsSource = this.treeViewModel.Roots;
        }

        private void PostOrderTraversal_Click(object sender, RoutedEventArgs e)
        {
            int i = 0;

            bool success = this.treeViewModel.PostOrderTraversal((x) =>
            {
                i++;

                if (i == 2)
                {
                    return false;
                }

                Console.WriteLine(x.Name);

                return true;

            }, false);

            Console.WriteLine("遍历结束, {0}", success);
        }
    }
}
