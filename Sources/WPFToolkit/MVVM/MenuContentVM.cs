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

        /// <summary>
        /// 该菜单在配置文件里的parameters数据
        /// </summary>
        internal IDictionary parameters;

        #endregion

        #region 属性

        /// <summary>
        /// 获取该菜单对应的内容控件
        /// </summary>
        public DependencyObject Content
        {
            get { return this.content; }
        }

        /// <summary>
        /// 获取该菜单内容的所有参数
        /// </summary>
        public IDictionary Parameters { get { return this.parameters; } }

        #endregion

        /// <summary>
        /// 读取该模块的输入参数，如果参数不存在则报异常
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetParameter<T>(string key)
        {
            IDictionary parameters = this.parameters;

            if (!parameters.Contains(key))
            {
                logger.ErrorFormat("没有找到必需的参数:{0}", key);
                throw new KeyNotFoundException();
            }

            return this.GetParameter<T>(parameters, key);
        }

        /// <summary>
        /// 读取该模块的输入参数，如果参数不存在则返回defaultValue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T GetParameter<T>(string key, T defaultValue)
        {
            IDictionary parameters = this.parameters;

            if (!parameters.Contains(key))
            {
                return defaultValue;
            }

            return this.GetParameter<T>(parameters, key);
        }

        private T GetParameter<T>(IDictionary parameters, string key)
        {
            Type t = typeof(T);

            if (t == TypeString)
            {
                return parameters.GetValue<T>(key);
            }

            if (t.IsClass)
            {
                string json = parameters[key].ToString();
                return JsonConvert.DeserializeObject<T>(json);
            }

            if (t.IsValueType)
            {
                return parameters.GetValue<T>(key);
            }

            if (t.IsEnum)
            {
                return parameters.GetValue<T>(key);
            }

            throw new NotImplementedException();
        }

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
