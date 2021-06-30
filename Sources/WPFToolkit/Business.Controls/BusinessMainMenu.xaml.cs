using System;
using System.Collections.Generic;
using DotNEToolkit;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFToolkit.MVVM;
using System.ComponentModel;
using System.Windows.Media;

namespace WPFToolkit.Business.Controls
{
    /// <summary>
    /// 主菜单业务逻辑控件
    /// </summary>
    public partial class BusinessMainMenu : UserControl
    {
        internal class MenuItem
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            /// <summary>
            /// 菜单的详细描述信息
            /// </summary>
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("entry")]
            public string EntryClass { get; set; }

            [JsonProperty("Icon")]
            public string Icon { get; set; }
        }

        internal class BusinessMainMenuJson
        {
            [JsonProperty("menus")]
            public List<MenuItem> MenuList { get; set; }

            public BusinessMainMenuJson()
            {
                this.MenuList = new List<MenuItem>();
            }
        }

        #region 类变量

        private static log4net.ILog logger = log4net.LogManager.GetLogger("BusinessMainMenu");

        #endregion

        #region 实例变量

        /// <summary>
        /// 上一个选中的菜单
        /// </summary>
        private BusinessMainMenuItemVM previouSelected;

        private BusinessMainMenuJson menuConfig;

        private ItemsPanelTemplate verticalPanel;

        private ItemsPanelTemplate horizontalPanel;

        #endregion

        #region 依赖属性

        /// <summary>
        /// 指定显示菜单所对应的界面的区域
        /// </summary>
        public ContentControl ContentContainer
        {
            get { return (ContentControl)GetValue(ContentContainerProperty); }
            set { SetValue(ContentContainerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentContainer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentContainerProperty =
            DependencyProperty.Register("ContentContainer", typeof(ContentControl), typeof(BusinessMainMenu), new PropertyMetadata(null, ContentContainerPropertyChangedCallback));


        /// <summary>
        /// 指定菜单要使用的配置文件
        /// </summary>
        public string ConfigFile
        {
            get { return (string)GetValue(ConfigFileProperty); }
            set { SetValue(ConfigFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfigFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigFileProperty =
            DependencyProperty.Register("ConfigFile", typeof(string), typeof(BusinessMainMenu), new PropertyMetadata(string.Empty, ConfigFilePropertyChangedCallback));

        /// <summary>
        /// 指定菜单的方向
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for OrientationProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(BusinessMainMenu), new PropertyMetadata(Orientation.Vertical, OrientationPropertyChangedCallback));


        /// <summary>
        /// 指定当前选中项
        /// </summary>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(BusinessMainMenu), new PropertyMetadata(-1, SelectedIndexPropertyChangedCallback));


        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(BusinessMainMenu), new PropertyMetadata(null, ItemTemplatePropertyChangedCallback));

        #endregion

        #region 属性

        public BusinessMainMenuVM ViewModel { get; private set; }

        #endregion

        #region 构造方法

        public BusinessMainMenu()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            this.verticalPanel = this.FindResource("ItemsPanelTemplateVertical") as ItemsPanelTemplate;
            this.horizontalPanel = this.FindResource("ItemsPanelTemplateHorizontal") as ItemsPanelTemplate;

            this.ViewModel = new BusinessMainMenuVM();
            this.DataContext = this.ViewModel;
        }

        #endregion

        #region 实例方法

        private void SwitchContent(int index)
        {
            if (this.ViewModel.Count == 0)
            {
                return;
            }

            if (index > this.ViewModel.Count - 1 || index < 0)
            {
                return;
            }

            this.ViewModel.SelectedItem = this.ViewModel[index];
        }

        #endregion

        #region 依赖属性回调

        private void OnConfigFilePropertyChanged(object oldValue, object newValue)
        {
            if (newValue == null)
            {
                this.ViewModel.Clear();
                this.ViewModel.SelectedItem = null;
                this.ViewModel.SelectedItems.Clear();
            }
            else
            {
                string configFile = newValue.ToString();

                if (!JSONHelper.TryParseFile<BusinessMainMenuJson>(configFile, out this.menuConfig))
                {
                    return;
                }

                foreach (MenuItem menuItem in this.menuConfig.MenuList)
                {
                    BusinessMainMenuItemVM vm = new BusinessMainMenuItemVM()
                    {
                        ID = menuItem.ID,
                        Name = menuItem.Name,
                        Description = menuItem.Description,
                        EntryClass = menuItem.EntryClass,
                        IconURI = menuItem.Icon
                    };
                    this.ViewModel.Add(vm);
                }
            }
        }

        private static void ConfigFilePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusinessMainMenu me = d as BusinessMainMenu;
            me.OnConfigFilePropertyChanged(e.OldValue, e.NewValue);
        }


        private void OnOrientationPropertyChanged(object oldValue, object newValue)
        {
            Orientation orientation = (Orientation)newValue;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    ListBoxMenu.ItemsPanel = this.horizontalPanel;
                    break;

                case Orientation.Vertical:
                    ListBoxMenu.ItemsPanel = this.verticalPanel;
                    break;

                default:
                    return;
            }
        }

        private static void OrientationPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusinessMainMenu me = d as BusinessMainMenu;
            me.OnOrientationPropertyChanged(e.OldValue, e.NewValue);
        }


        private void OnSelectedIndexPropertyChanged(object oldValue, object newValue)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            if (newValue == null)
            {
                return;
            }

            int index = (int)newValue;

            this.SwitchContent(index);
        }

        private static void SelectedIndexPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusinessMainMenu me = d as BusinessMainMenu;
            me.OnSelectedIndexPropertyChanged(e.OldValue, e.NewValue);
        }


        private void OnContentContainerPropertyChanged(object oldValue, object newValue)
        {
            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            if (oldValue != null)
            {
                ContentControl container = oldValue as ContentControl;
                container.Content = null;
            }

            if (newValue != null)
            {
                this.ViewModel.SelectedItem = null;
                this.SwitchContent(this.SelectedIndex);
            }
        }

        private static void ContentContainerPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusinessMainMenu me = d as BusinessMainMenu;
            me.OnContentContainerPropertyChanged(e.OldValue, e.NewValue);
        }


        private void OnItemTemplatePropertyChanged(object oldValue, object newValue)
        {
            if (newValue != null)
            {
                ListBoxMenu.ItemTemplate = newValue as DataTemplate;
            }
        }

        private static void ItemTemplatePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            BusinessMainMenu me = d as BusinessMainMenu;
            me.OnItemTemplatePropertyChanged(e.OldValue, e.NewValue);
        }

        #endregion

        #region 事件处理器

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems == null || e.AddedItems.Count == 0 || this.ContentContainer == null)
            {
                return;
            }

            BusinessMainMenuItemVM selectedMenu = e.AddedItems[0] as BusinessMainMenuItemVM;
            if (selectedMenu == null)
            {
                return;
            }

            if (selectedMenu.Content == null)
            {
                try
                {
                    selectedMenu.Content = ConfigFactory<DependencyObject>.CreateInstance(selectedMenu.EntryClass);

                    if (selectedMenu is IBusinessMainMenuHook)
                    {
                        (selectedMenu as IBusinessMainMenuHook).Initialize();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("创建界面实例异常", ex);
                    return;
                }
            }

            this.ContentContainer.Content = null;
            if (this.previouSelected != null && this.previouSelected.Content is IBusinessMainMenuHook)
            {
                (this.previouSelected.Content as IBusinessMainMenuHook).OnUnload();
            }
            this.ContentContainer.Content = selectedMenu.Content;
            IBusinessMainMenuHook hook = selectedMenu.Content as IBusinessMainMenuHook;
            if (hook != null)
            {
                hook.OnLoaded();
            }

            this.previouSelected = selectedMenu;
        }

        #endregion
    }
}
