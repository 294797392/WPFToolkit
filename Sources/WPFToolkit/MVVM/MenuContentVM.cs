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
        private static log4net.ILog logger = log4net.LogManager.GetLogger("MenuContentVM");
        private static readonly Type TypeString = typeof(string);

        internal DependencyObject content;

        /// <summary>
        /// 该菜单在配置文件里的parameters数据
        /// </summary>
        internal IDictionary parameters;

        /// <summary>
        /// 获取该菜单对应的内容控件
        /// </summary>
        public DependencyObject Content
        {
            get { return this.content; }
        }

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

        public virtual void Initialize()
        {
        }

        public virtual void OnLoaded()
        {
        }

        public virtual void OnUnload()
        {
        }

        public virtual void Release()
        {
        }
    }
}
