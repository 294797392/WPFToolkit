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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFToolkit.MVVM;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// TreeComboBoxUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class TreeComboBoxUserControl : UserControl
    {
        private BindableCollection<TreeNodeViewModel> roots;

        public TreeComboBoxUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            TreeViewModelContext context = new TreeViewModelContext();

            roots = new BindableCollection<TreeNodeViewModel>();

            for (int i = 0; i < 10; i++)
            {
                TreeNodeViewModel treeNodeViewModel = new TreeNodeViewModel(context)
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = i.ToString(),
                };

                for (int j = 0; j < 5; j++)
                {
                    TreeNodeViewModel treeNodeViewModel1 = new TreeNodeViewModel(context)
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = j.ToString(),
                    };

                    treeNodeViewModel.Add(treeNodeViewModel1);
                }

                roots.Add(treeNodeViewModel);
            }

            ComboBox1.DataContext = roots;
            ComboBox1.ItemsSource = roots;

            roots.SelectedItem = roots.LastOrDefault();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this.roots.SelectedItem.Name);
        }
    }
}
