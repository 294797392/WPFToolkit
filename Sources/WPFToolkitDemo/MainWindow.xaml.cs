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
            this.menuVM.Initialize("menu.json");
            ListBoxMenuItems.DataContext = this.menuVM;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                MenuItemVM selectedMenu = e.AddedItems[0] as MenuItemVM;
                if (selectedMenu.MenuItems.Count > 0)
                {
                    MenuItemVM firstSelectedMenu = selectedMenu.MenuItems.FirstOrDefault(v => v.IsSelected);
                    if (firstSelectedMenu != null)
                    {
                        this.menuVM.InvokeWhenSelectionChanged(firstSelectedMenu);
                    }
                    else
                    {
                        selectedMenu.MenuItems[0].IsSelected = true;
                    }
                }
                else
                {
                    this.menuVM.InvokeWhenSelectionChanged(selectedMenu);
                }
            }
        }
    }
}
