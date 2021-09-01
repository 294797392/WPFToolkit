using DotNEToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 表示一个内容控件
    /// </summary>
    public interface IContentHost
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

        private Dictionary<string, object> contentMap;

        private object currentContent;

        /// <summary>
        /// 上次选中的菜单
        /// </summary>
        private MenuItemVM previouseSelectedMenu;

        /// <summary>
        /// 当前的内容是否正在初始化
        /// </summary>
        private bool isContentInitializing;

        #endregion

        #region 属性

        /// <summary>
        /// 当前的内容是否正在初始化
        /// 界面上可以通过这个值给用户显示一个“加载中”的界面
        /// </summary>
        public bool IsContentInitializing
        {
            get { return this.isContentInitializing; }
            set 
            {
                this.isContentInitializing = true;
                this.OnPropertyChanged(new PropertyChangedEventArgs("IsContentInitializing"));
            }
        }


        /// <summary>
        /// 当前显示的界面
        /// </summary>
        public object CurrentContent
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
            this.contentMap = new Dictionary<string, object>();
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
                if (this.CurrentContent is IContentHost)
                {
                    IContentHost contentHost = this.CurrentContent as IContentHost;
                    contentHost.OnUnload();
                }

                // 先移除之前显示的界面
                this.CurrentContent = null;
            }

            // 开始加载本次选中的菜单界面
            object content;
            if (!this.contentMap.TryGetValue(this.SelectedItem.ClassName, out content))
            {
                try
                {
                    content = ConfigFactory<object>.CreateInstance(this.SelectedItem.ClassName);
                }
                catch (Exception ex)
                {
                    logger.Error("创建菜单内容控件异常", ex);
                    return;
                }

                this.contentMap[this.SelectedItem.ClassName] = content;
            }

            this.CurrentContent = content;

            if (content is IContentHost)
            {
                IContentHost contentHost = content as IContentHost;

                if (!this.SelectedItem.IsInitialized)
                {
                    this.IsContentInitializing = true;

                    try
                    {
                        contentHost.Initialize();
                        this.SelectedItem.IsInitialized = true;
                    }
                    catch (Exception ex)
                    {
                        logger.Error("初始化Content异常", ex);
                    }
                    finally
                    {
                        this.IsContentInitializing = false;
                    }
                }

                contentHost.OnLoaded();
            }

            this.previouseSelectedMenu = this.SelectedItem;
        }

        #endregion
    }
}
