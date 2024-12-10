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
using WPFToolkit.MVVM;
using WPFToolkit.Utility;
using WPFToolkit.Windows;

namespace WPFToolkitDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private MenuVM menuVM;

        public MainWindow()
        {
            InitializeComponent();

            this.InitializeWindow();
        }

        private void InitializeWindow()
        {
            this.menuVM = new MenuVM();
            this.menuVM.Initialize(ToolkitApp.Context.Manifest.MenuList);
            ListBoxMenuItems.DataContext = this.menuVM;
            TreeView1.DataContext = this.menuVM;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MenuItemVM menuItem = ListBoxMenuItems.SelectedItem as MenuItemVM;
            if (menuItem == null) 
            {
                return;
            }

            DependencyObject dependencyObject = this.menuVM.SwitchContent(menuItem);
            if (dependencyObject == null) 
            {
                return;
            }

            ContentControlContent.Content = dependencyObject;
        }
    }
}
