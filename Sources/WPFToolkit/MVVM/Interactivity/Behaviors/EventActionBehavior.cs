using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WPFToolkit.MVVM.Interactivity.Behaviors
{
    /// <summary>
    /// 事件转Action
    /// 使用反射注册事件：https://www.cnblogs.com/walterlv/p/10236420.html
    /// </summary>
    public class EventActionBehavior : Behavior
    {
        public const string SelectionChangedEventName = "SelectionChanged";

        public static readonly DependencyProperty EventNameProperty = DependencyProperty.Register("EventName", typeof(string), typeof(EventActionBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty ActionProperty = DependencyProperty.Register("Action", typeof(Action<object>), typeof(EventActionBehavior), new PropertyMetadata(null));
        public static readonly DependencyProperty EventArgsConverterProperty = DependencyProperty.Register("EventArgsConverter", typeof(IValueConverter), typeof(EventActionBehavior), new PropertyMetadata(null));

        /// <summary>
        /// 要执行Action的Event
        /// </summary>
        public string EventName
        {
            get { return (string)GetValue(EventNameProperty); }
            set { SetValue(EventNameProperty, value); }
        }

        /// <summary>
        /// 要执行的Action
        /// </summary>
        public Action<object> Action
        {
            get { return (Action<object>)GetValue(ActionProperty); }
            set { SetValue(ActionProperty, value); }
        }

        /// <summary>
        /// 事件参数转换器
        /// </summary>
        public IValueConverter EventArgsConverter
        {
            get { return (IValueConverter)GetValue(EventArgsConverterProperty); }
            set { SetValue(EventArgsConverterProperty, value); }
        }

        internal override void Initialize()
        {
            if (string.IsNullOrEmpty(this.EventName))
            {
                // XAML里没有设置或绑定EventName
                return;
            }

            // 使用反射注册事件
            this.RegisterEvent(this.EventName, this.AttachedObject);
        }

        internal override void Release()
        {
        }

        /// <summary>
        /// 给一个对象注册事件处理器
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="registeredObject"></param>
        private void RegisterEvent(string eventName, object registeredObject)
        {
            Type t = registeredObject.GetType();

            if (registeredObject is ItemsControl && string.Compare(eventName, SelectionChangedEventName) == 0)
            {
                // TODO:SelectionChanged反射绑定会出问题，暂时没查到原因。这里对SelectionChanged事件做单独绑定操作
            }
            else
            {
                // 获取要注册的事件信息
                //EventInfo @event = t.GetEvent("SelectionChanged");
                EventInfo @event = t.GetEvent(eventName);

                /*
                 * 获取要注册的事件的参数
                 * 相当于：
                 * SelectionChangedEventHandler eventHandler;
                 * eventHandler.Invoke()
                 */
                ParameterInfo[] parameters = @event.EventHandlerType.GetMethod("Invoke").GetParameters();

                // 使用一个代理类保存不同的事件参数
                Type eventHandlerType = typeof(EventHandlerContainer<,>).MakeGenericType(new Type[] {
                    parameters[0].GetType(),
                    parameters[1].GetType()
                });

                // 创建可以保存不同参数类型的代理类的实例
                object handler = Activator.CreateInstance(eventHandlerType, new object[] { this.Action });

                // 创建要注册的事件委托
                Delegate @delegate = Delegate.CreateDelegate(@event.EventHandlerType, handler, handler.GetType().GetMethod("Handler"));

                @event.AddEventHandler(this, @delegate);
            }
        }

        private void RegisterEventEx(string eventName, object registeredObject)
        {
            Type t = registeredObject.GetType();

            if (registeredObject is ItemsControl && string.Compare(eventName, SelectionChangedEventName) == 0)
            {
                // TODO:SelectionChanged反射绑定会出问题，暂时没查到原因。这里对SelectionChanged事件做单独绑定操作
            }
            else
            {
                // 获取要注册的事件信息
                //EventInfo @event = t.GetEvent("SelectionChanged");
                EventInfo @event = t.GetEvent(eventName);

                /*
                 * 获取要注册的事件的参数
                 * 相当于：
                 * SelectionChangedEventHandler eventHandler;
                 * eventHandler.Invoke()
                 */
                ParameterInfo[] parameters = @event.EventHandlerType.GetMethod("Invoke").GetParameters();

                // 创建要注册的事件委托
                Delegate @delegate = Delegate.CreateDelegate(@event.EventHandlerType, this, this.GetType().GetMethod("AttachedObjectEventHandler", BindingFlags.NonPublic | BindingFlags.Instance));

                @event.AddEventHandler(this, @delegate);
            }
        }

        /// <summary>
        /// 被附加对象的事件处理器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void AttachedObjectEventHandler(object sender, object eventArgs)
        {
            object parameter = eventArgs;

            if (this.EventArgsConverter != null)
            {
                parameter = this.EventArgsConverter.Convert(sender, null, eventArgs, CultureInfo.CurrentCulture);
            }

            if (this.Action != null)
            {
                this.Action(parameter);
            }
        }

        public class EventHandlerContainer<T1, T2>
        {
            public EventHandlerContainer(Action<object, object> action)
            {

            }

            public void Handler(object o1, object o2)
            {
            }
        }
    }
}