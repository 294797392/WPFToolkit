using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFToolkit.MVVM;
using WPFToolkit.QuickControls;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 主菜单业务逻辑控件
    /// </summary>
    public class QuickMainMenu : ListBox
    {
        internal class MenuItem
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("entryClass")]
            public string EntryClass { get; set; }
        }

        internal class QuickMainMenuJson
        {
            [JsonProperty("menus")]
            public List<MenuItem> MenuList { get; set; }

            public QuickMainMenuJson()
            {
                this.MenuList = new List<MenuItem>();
            }
        }

        #region 类变量

        private static log4net.ILog logger = log4net.LogManager.GetLogger("QuickMainMenu");

        #endregion

        #region 实例变量

        /// <summary>
        /// 上一个选中的菜单
        /// </summary>
        private QuickMainMenuItemVM previouSelected;

        private QuickMainMenuJson menuConfig;

        #endregion

        #region 依赖属性

        public ContentControl Content
        {
            get { return (ContentControl)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(ContentControl), typeof(QuickMainMenu), new PropertyMetadata(null));

        public string ConfigFile
        {
            get { return (string)GetValue(ConfigFileProperty); }
            set { SetValue(ConfigFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConfigFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConfigFileProperty =
            DependencyProperty.Register("ConfigFile", typeof(string), typeof(QuickMainMenu), new PropertyMetadata(string.Empty));

        #endregion

        #region 属性

        public QuickMainMenuVM ViewModel { get; private set; }

        #endregion

        #region 构造方法

        public QuickMainMenu()
        {
            this.ViewModel = new QuickMainMenuVM();
        }

        #endregion

        #region 重写方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (string.IsNullOrEmpty(this.ConfigFile))
            {
                return;
            }

            if (!JSONHelper.TryParseFile<QuickMainMenuJson>(this.ConfigFile, out this.menuConfig))
            {
                return;
            }

            foreach (MenuItem menuItem in this.menuConfig.MenuList)
            {
                QuickMainMenuItemVM vm = new QuickMainMenuItemVM() 
                {
                    ID = menuItem.ID,
                    Name = menuItem.Name
                };
                this.ViewModel.Items.Add(vm);
            }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (this.Content == null)
            {
                return;
            }

            if (e.AddedItems == null || e.AddedItems.Count == 0)
            {
                return;
            }

            QuickMainMenuItemVM selectedMenu = e.AddedItems[0] as QuickMainMenuItemVM;
            if (selectedMenu == null)
            {
                return;
            }

            if (selectedMenu.Content == null)
            {
                try
                {
                    selectedMenu.Content = ConfigFactory<DependencyObject>.CreateInstance<DependencyObject>(selectedMenu.EntryClass);

                    if (selectedMenu is IQuickMainMenuHook)
                    {
                        (selectedMenu as IQuickMainMenuHook).Initialize();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("创建界面实例异常", ex);
                    return;
                }
            }

            this.Content.Content = null;
            if (this.previouSelected != null && this.previouSelected.Content is IQuickMainMenuHook)
            {
                (this.previouSelected.Content as IQuickMainMenuHook).OnUnload();
            }
            this.Content.Content = selectedMenu.Content;
            IQuickMainMenuHook hook = selectedMenu.Content as IQuickMainMenuHook;
            if (hook != null)
            {
                hook.OnLoaded();
            }

            this.previouSelected = selectedMenu;
        }

        #endregion

        #region 实例方法

        #endregion
    }
}
