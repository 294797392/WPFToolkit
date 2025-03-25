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
        void OnInitialize();

        /// <summary>
        /// 释放ContentHost的资源
        /// </summary>
        void OnRelease();

        /// <summary>
        /// 每次显示在界面上之后都会触发
        /// </summary>
        void OnLoaded();

        /// <summary>
        /// 每次从界面上移除之前都会触发
        /// </summary>
        void OnUnload();
    }

    public class MenuContext
    {
        public MenuItemVM SelectedItem { get; set; }

        public List<MenuItemVM> AllItems { get; private set; }

        public MenuContext()
        {
            this.AllItems = new List<MenuItemVM>();
        }
    }

    /// <summary>
    /// 树形结构的菜单ViewModel
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
        /// 当前的内容是否正在初始化
        /// </summary>
        private bool isContentLoading;

        #endregion

        #region 属性

        /// <summary>
        /// 保存菜单的上下文信息
        /// </summary>
        public MenuContext Context { get; private set; }

        /// <summary>
        /// 当前选中的菜单
        /// 该属性是只读的，考虑到菜单有可能是树形结构，TreeView没有SelectedItem属性，无法直接绑定
        /// 当MenuItemVM.IsSelected改变的时候，修改这个值
        /// </summary>
        public MenuItemVM SelectedMenu { get { return this.Context.SelectedItem; } }

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
            this.Context = new MenuContext();
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

        public void Initialize(IEnumerable<MenuItemVM> menuItems)
        {
            foreach (MenuItemVM menuItem in menuItems)
            {
                this.AddMenuItem(menuItem);
            }
        }

        /// <summary>
        /// 加载指定的界面，会执行相关的生命周期函数
        /// </summary>
        /// <param name="menuItem">要切换的菜单</param>
        /// <returns>如果菜单没有对应的页面，那么返回null。否则返回对应的页面实例</returns>
        public DependencyObject LoadContent(MenuItemVM menuItem)
        {
            if (menuItem == null)
            {
                return null;
            }

            if (string.IsNullOrEmpty(menuItem.ClassName))
            {
                return null;
            }

            if (this.CurrentContent != null)
            {
                // 如果要加载的界面和当前显示的界面一致，那么直接返回
                if (this.CurrentContent == menuItem.Content)
                {
                    return this.CurrentContent;
                }

                // 触发Unload事件
                this.ProcessContentUnload(this.CurrentContent);

                // 先移除之前显示的界面
                this.CurrentContent = null;
            }

            FrameworkElement content = menuItem.Content;
            ViewModelBase contentVM = menuItem.ContentVM;

            // 开始加载本次选中的菜单界面
            if (content == null)
            {
                try
                {
                    content = ConfigFactory<FrameworkElement>.CreateInstance(menuItem.ClassName);

                    if (!string.IsNullOrEmpty(menuItem.VMClassName))
                    {
                        // 如果存在ViewModel，那么实例化ViewModel并绑定
                        // 此时会覆盖掉调用者在构造函数里绑定的ViewModel
                        contentVM = ConfigFactory<ViewModelBase>.CreateInstance(menuItem.VMClassName);
                        contentVM.Name = menuItem.Name;
                    }
                    else
                    {
                        // 有可能在动态创建Content实例的时候，构造函数里绑定了ViewModel
                        contentVM = content.DataContext as ViewModelBase;
                    }

                    // 先初始化界面，再初始化ViewModel
                    if (content is IContentHook)
                    {
                        IContentHook contentHook = content as IContentHook;
                        contentHook.OnInitialize();
                    }

                    // 初始化ViewModel
                    if (contentVM is MenuContentVM)
                    {
                        MenuContentVM menuContentVM = contentVM as MenuContentVM;
                        menuContentVM.content = content;
                        menuContentVM.parameters = menuItem.Parameters;
                        menuContentVM.OnInitialize();
                    }

                    if (content.DataContext != contentVM)
                    {
                        content.DataContext = contentVM;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("创建菜单内容控件异常", ex);
                    return null;
                }

                menuItem.Content = content;
                menuItem.ContentVM = contentVM;
            }

            this.CurrentContent = menuItem.Content;

            if (content is IContentHook)
            {
                // 再处理Content是IContentHost的情况
                IContentHook contentHost = content as IContentHook;

                this.ProcessContentLoaded(contentHost);
            }

            // 优先处理DataContent是IContentHost的情况
            if (contentVM is MenuContentVM)
            {
                MenuContentVM menuContentVM = contentVM as MenuContentVM;

                this.ProcessContentLoaded(menuContentVM);
            }

            menuItem.IsSelected = true;

            return menuItem.Content;
        }

        /// <summary>
        /// 根据Id获取指定的节点
        /// </summary>
        /// <param name="menuId">要获取的节点的Id</param>
        /// <param name="menuItem"></param>
        /// <returns></returns>
        public bool TryGetItem(string menuId, out MenuItemVM menuItem)
        {
            menuItem = null;

            MenuItemVM menuItemVM = this.Context.AllItems.FirstOrDefault(v => v.ID.ToString() == menuId);
            if (menuItemVM is MenuItemVM)
            {
                menuItem = menuItemVM as MenuItemVM;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 展开所有子节点
        /// </summary>
        public void ExpandAll()
        {
            foreach (MenuItemVM menuItem in this.MenuItems)
            {
                this.ExpandAll(menuItem);
            }
        }

        /// <summary>
        /// 新加一个子节点
        /// </summary>
        /// <param name="menuItem"></param>
        public void AddMenuItem(MenuItemVM menuItem)
        {
            this.Context.AllItems.Add(menuItem);
            menuItem.context = this.Context;
            this.MenuItems.Add(menuItem);
        }

        #endregion

        #region 实例方法

        private void LoadMenu(IEnumerable<MenuDefinition> menuDefinitions)
        {
            foreach (MenuDefinition menu in menuDefinitions)
            {
                MenuItemVM menuItem = new MenuItemVM(menu);
                menuItem.context = this.Context;
                menuItem.Level = 0;
                this.AddMenuItem(menuItem);

                // 递归加载子菜单
                this.LoadSubMenus(menuItem, menu.Children);

                this.Context.AllItems.Add(menuItem);
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
                menuItem.Level = parentMenu.Level + 1;
                menuItem.context = this.Context;
                menuItem.Parent = parentMenu;
                parentMenu.AddMenuItem(menuItem);
                this.LoadSubMenus(menuItem, menu.Children);

                this.Context.AllItems.Add(menuItem);
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

        private void ExpandAll(MenuItemVM parentNode)
        {
            if (parentNode.MenuItems.Count > 0)
            {
                parentNode.IsExpanded = true;

                foreach (MenuItemVM treeNode in parentNode.MenuItems)
                {
                    if (treeNode.MenuItems.Count > 0)
                    {
                        treeNode.IsExpanded = true;

                        this.ExpandAll(treeNode);
                    }
                }
            }
        }

        #endregion
    }
}
