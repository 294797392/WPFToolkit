using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM
{
    public class MenuItemVM : ItemViewModel
    {
        private bool isInitialized;
        private FrameworkElement content;

        /// <summary>
        /// 界面入口点
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// ViewModel的完整类名
        /// </summary>
        public string VMClassName { get; set; }

        /// <summary>
        /// 该菜单是否初始化完了
        /// </summary>
        public bool IsInitialized
        {
            get { return this.isInitialized; }
            set
            {
                this.isInitialized = value;
                this.NotifyPropertyChanged("IsInitialized");
            }
        }

        /// <summary>
        /// 该菜单所要显示的内容
        /// </summary>
        public FrameworkElement Content
        {
            get { return this.content; }
            set
            {
                this.content = value;
                this.NotifyPropertyChanged("Content");
            }
        }

        /// <summary>
        /// 该菜单所显示的内容的ViewModel
        /// </summary>
        public ViewModelBase ContentVM { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ObservableCollection<MenuItemVM> MenuItems { get; private set; }

        public MenuItemVM(MenuDefinition menu) 
        {
            this.MenuItems = new ObservableCollection<MenuItemVM>();
            this.ID = menu.ID;
            this.Name = menu.Name;
            this.ClassName = menu.ClassName;
            this.VMClassName = menu.VMClassName;
            this.IconURI = menu.Icon;
        }
    }
}
