using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    internal class MenuDefinition
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
        [JsonProperty("vmclsName")]
        public string VMClassName { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }
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
    public class MenuVM : BindableCollection<MenuItemVM>
    {
        #region 类变量

        private static log4net.ILog logger = log4net.LogManager.GetLogger("MenuVM");

        #endregion

        #region 实例变量

        private Dictionary<string, FrameworkElement> contentMap;

        private FrameworkElement currentContent;

        /// <summary>
        /// 上次选中的菜单
        /// </summary>
        private MenuItemVM previouseSelectedMenu;

        /// <summary>
        /// 当前的内容是否正在初始化
        /// </summary>
        private bool isContentLoading;

        #endregion

        #region 属性

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
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsContentLoading"));
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
                this.OnPropertyChanged(new PropertyChangedEventArgs("CurrentContent"));
            }
        }

        #endregion

        #region 构造方法

        public MenuVM()
        {
            this.contentMap = new Dictionary<string, FrameworkElement>();
        }

        #endregion

        #region 公开接口

        public void InvokeWhenSelectionChanged()
        {
            if (this.SelectedItem == null)
            {
                return;
            }

            if (this.SelectedItem == this.previouseSelectedMenu)
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

            // 开始加载本次选中的菜单界面
            FrameworkElement content;
            if (!this.contentMap.TryGetValue(this.SelectedItem.ClassName, out content))
            {
                try
                {
                    content = ConfigFactory<FrameworkElement>.CreateInstance(this.SelectedItem.ClassName);

                    // 如果存在ViewModel，那么实例化ViewModel并绑定
                    if (!string.IsNullOrEmpty(this.SelectedItem.VMClassName))
                    {
                        content.DataContext = ConfigFactory<ViewModelBase>.CreateInstance(this.SelectedItem.VMClassName);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("创建菜单内容控件异常", ex);
                    return;
                }

                this.contentMap[this.SelectedItem.ClassName] = content;
            }

            this.CurrentContent = content;

            // 优先处理DataContent是IContentHost的情况
            if (content.DataContext is IContentHook)
            {
                IContentHook contentHost = content.DataContext as IContentHook;

                this.ProcessContentLoaded(contentHost);
            }
            else if (content is IContentHook)
            {
                // 再处理Content是IContentHost的情况
                IContentHook contentHost = content as IContentHook;

                this.ProcessContentLoaded(contentHost);
            }

            this.previouseSelectedMenu = this.SelectedItem;
        }

        public void ParseMenuDefinition(string filePath)
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

            foreach (MenuDefinition menu in menus)
            {
                this.Add(new MenuItemVM()
                {
                    ID = menu.ID,
                    Name = menu.Name,
                    ClassName = menu.ClassName,
                    VMClassName = menu.VMClassName,
                    IconURI = menu.Icon
                });
            }
        }

        #endregion

        #region 实例方法

        private void ProcessContentLoaded(IContentHook contentHost)
        {
            this.IsContentLoading = true;

            try
            {
                // 如果界面还没初始化，那么初始化
                if (!this.SelectedItem.IsInitialized)
                {
                    contentHost.Initialize();
                    this.SelectedItem.IsInitialized = true;
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
            if (content.DataContext is IContentHook)
            {
                IContentHook contentHost = content.DataContext as IContentHook;
                contentHost.OnUnload();
            }
            else if (content is IContentHook)
            {
                IContentHook contentHost = this.CurrentContent as IContentHook;
                contentHost.OnUnload();
            }
        }

        #endregion
    }
}
