using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 定义配置文件里的菜单模型
    /// </summary>
    public class MenuDefinition
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 菜单名字
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 界面类名
        /// </summary>
        [JsonProperty("className")]
        public string ClassName { get; set; }

        /// <summary>
        /// ViewModel类名
        /// </summary>
        [JsonProperty("vmClassName")]
        public string VMClassName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        [JsonProperty("child")]
        public List<MenuDefinition> Children { get; private set; }

        /// <summary>
        /// 输入参数
        /// </summary>
        [JsonProperty("parameters")]
        public IDictionary Parameters { get; set; }

        public MenuDefinition()
        {
            this.Children = new List<MenuDefinition>();
            this.Parameters = new Dictionary<string, object>();
        }
    }

    /// <summary>
    /// 表示内容控件生命周期钩子对象
    /// </summary>
    public interface IContentHook
    {
        /// <summary>
        /// 第一次初始化会调用
        /// </summary>
        void Initialize();

        /// <summary>
        /// 释放ContentHost的资源
        /// </summary>
        void Release();

        /// <summary>
        /// 每次显示在界面上之后都会触发
        /// </summary>
        void OnLoaded();

        /// <summary>
        /// 每次从界面上移除之前都会触发
        /// </summary>
        void OnUnload();
    }

    /// <summary>
    /// 菜单ViewModel
    /// </summary>
    public class MenuVM : ViewModelBase
    {
        #region 类变量

        private static log4net.ILog logger = log4net.LogManager.GetLogger("MenuVM");

        #endregion

        #region 实例变量

        /// <summary>
        /// 当前选中的页面
        /// </summary>
        private FrameworkElement currentContent;

        /// <summary>
        /// 上次选中的菜单
        /// </summary>
        private MenuItemVM previouseSelectedMenu;

        /// <summary>
        /// 当前的内容是否正在初始化
        /// </summary>
        private bool isContentLoading;

        private MenuItemVM selectedMenu;

        #endregion

        #region 属性

        /// <summary>
        /// 当前选中的菜单
        /// </summary>
        public MenuItemVM SelectedMenu
        {
            get { return this.selectedMenu; }
            set
            {
                this.selectedMenu = value;
                this.NotifyPropertyChanged("SelectedMenu");
            }
        }

        /// <summary>
        /// 所有的菜单列表
        /// </summary>
        public ObservableCollection<MenuItemVM> MenuItems { get; private set; }

        /// <summary>
        /// 当前的内容是否正在初始化
        /// 界面上可以通过这个值给用户显示一个“加载中”的界面
        /// </summary>
        public bool IsContentLoading
        {
            get { return this.isContentLoading; }
            set
            {
                this.isContentLoading = true;
                this.NotifyPropertyChanged("IsContentLoading");
            }
        }

        /// <summary>
        /// 当前显示的界面
        /// </summary>
        public FrameworkElement CurrentContent
        {
            get { return this.currentContent; }
            set
            {
                this.currentContent = value;
                this.NotifyPropertyChanged("CurrentContent");
            }
        }

        #endregion

        #region 构造方法

        public MenuVM()
        {
            this.MenuItems = new ObservableCollection<MenuItemVM>();
        }

        #endregion

        #region 公开接口

        public void Initialize(string filePath)
        {
            this.ParseMenuDefinition(filePath);
        }

        public void Initialize(IEnumerable<MenuDefinition> menuDefinitions)
        {
            this.LoadMenu(menuDefinitions);
        }

        public void InvokeWhenSelectionChanged(MenuItemVM selectedMenu)
        {
            this.SelectedMenu = selectedMenu;

            if (this.SelectedMenu == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.SelectedMenu.ClassName))
            {
                return;
            }

            if (this.SelectedMenu == this.previouseSelectedMenu)
            {
                return;
            }

            if (this.CurrentContent != null)
            {
                // 触发Unload事件
                this.ProcessContentUnload(this.CurrentContent);

                // 先移除之前显示的界面
                this.CurrentContent = null;
            }

            FrameworkElement content = this.SelectedMenu.Content;
            ViewModelBase contentVM = this.SelectedMenu.ContentVM;

            // 开始加载本次选中的菜单界面
            if (content == null)
            {
                try
                {
                    content = ConfigFactory<FrameworkElement>.CreateInstance(this.SelectedMenu.ClassName);

                    if (!string.IsNullOrEmpty(this.SelectedMenu.VMClassName))
                    {
                        // 如果存在ViewModel，那么实例化ViewModel并绑定
                        // 此时会覆盖掉调用者在构造函数里绑定的ViewModel
                        contentVM = ConfigFactory<ViewModelBase>.CreateInstance(this.SelectedMenu.VMClassName);
                    }
                    else
                    {
                        // 有可能在动态创建Content实例的时候，构造函数里绑定了ViewModel
                        contentVM = content.DataContext as ViewModelBase;
                    }

                    // 初始化MenuContentVM
                    if (contentVM is MenuContentVM)
                    {
                        MenuContentVM menuContentVM = contentVM as MenuContentVM;
                        menuContentVM.content = content;
                        menuContentVM.parameters = selectedMenu.Parameters;
                        menuContentVM.Initialize();
                    }

                    if (content.DataContext != contentVM)
                    {
                        content.DataContext = contentVM;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("创建菜单内容控件异常", ex);
                    return;
                }

                this.SelectedMenu.Content = content;
                this.SelectedMenu.ContentVM = contentVM;
            }

            this.CurrentContent = this.SelectedMenu.Content;

            // 优先处理DataContent是IContentHost的情况
            if (contentVM is MenuContentVM)
            {
                MenuContentVM menuContentVM = contentVM as MenuContentVM;

                this.ProcessContentLoaded(menuContentVM);
            }

            if (content is IContentHook)
            {
                // 再处理Content是IContentHost的情况
                IContentHook contentHost = content as IContentHook;

                this.ProcessContentLoaded(contentHost);
            }

            this.previouseSelectedMenu = this.SelectedMenu;
        }

        public void InvokeWhenSelectionChanged()
        {
            this.InvokeWhenSelectionChanged(this.SelectedMenu);
        }

        public TViewModel GetViewModel<TViewModel>() where TViewModel : ViewModelBase
        {
            return this.MenuItems.Select(v => v.ContentVM).OfType<TViewModel>().FirstOrDefault();
        }

        #endregion

        #region 实例方法

        private void LoadMenu(IEnumerable<MenuDefinition> menuDefinitions)
        {
            foreach (MenuDefinition menu in menuDefinitions)
            {
                MenuItemVM menuItem = new MenuItemVM(menu);

                this.MenuItems.Add(menuItem);

                // 递归加载子菜单
                this.LoadSubMenus(menuItem, menu.Children);
            }
        }

        private void ParseMenuDefinition(string filePath)
        {
            if (!File.Exists(filePath))
            {
                logger.WarnFormat("菜单配置文件不存在:{0}", filePath);
                return;
            }

            List<MenuDefinition> menus = null;

            try
            {
                menus = JSONHelper.ParseFile<List<MenuDefinition>>(filePath);
            }
            catch (Exception ex)
            {
                logger.Error("反序列化菜单配置文件异常", ex);
                return;
            }

            this.LoadMenu(menus);
        }

        private void ProcessContentLoaded(IContentHook contentHost)
        {
            this.IsContentLoading = true;

            try
            {
                // 如果界面还没初始化，那么初始化
                if (!this.SelectedMenu.IsInitialized)
                {
                    contentHost.Initialize();
                    this.SelectedMenu.IsInitialized = true;
                }

                // 初始化完后再触发OnLoaded事件
                contentHost.OnLoaded();
            }
            catch (Exception ex)
            {
                logger.Error("初始化Content异常", ex);
            }
            finally
            {
                this.IsContentLoading = false;
            }
        }

        private void ProcessContentUnload(FrameworkElement content)
        {
            if (content.DataContext is MenuContentVM)
            {
                IContentHook contentHost = content.DataContext as IContentHook;
                contentHost.OnUnload();
            }

            if (content is IContentHook)
            {
                IContentHook contentHost = this.CurrentContent as IContentHook;
                contentHost.OnUnload();
            }
        }

        /// <summary>
        /// 递归加载子菜单
        /// </summary>
        /// <param name="parentMenu"></param>
        /// <param name="childMenus"></param>
        private void LoadSubMenus(MenuItemVM parentMenu, List<MenuDefinition> childMenus)
        {
            foreach (MenuDefinition menu in childMenus)
            {
                MenuItemVM menuItem = new MenuItemVM(menu);
                parentMenu.MenuItems.Add(menuItem);
                this.LoadSubMenus(parentMenu, menu.Children);
            }
        }

        #endregion
    }
}
