using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 菜单内容控件ViewModel的基类
    /// 继承该类后，可以获取到在配置文件里设置的参数
    /// </summary>
    public abstract class MenuContentVM : ViewModelBase, IContentHook
    {
        #region 类变量

        private static log4net.ILog logger = log4net.LogManager.GetLogger("MenuContentVM");
        private static readonly Type TypeString = typeof(string);

        #endregion

        #region 实例变量

        internal DependencyObject content;

        #endregion

        #region 属性

        /// <summary>
        /// 获取该菜单对应的内容控件
        /// </summary>
        public DependencyObject Content
        {
            get { return this.content; }
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 当菜单内容第一次加载的时候触发
        /// </summary>
        public abstract void OnInitialize();

        /// <summary>
        /// 如果界面绑定了CurrentContent，那么当内容显示到界面之后触发
        /// 如果手动调用SwitchContent加载界面，那么在内容显示之前触发
        /// </summary>
        public abstract void OnLoaded();

        public abstract void OnUnload();

        /// <summary>
        /// 释放的时候触发
        /// </summary>
        public abstract void OnRelease();

        #endregion
    }
}
