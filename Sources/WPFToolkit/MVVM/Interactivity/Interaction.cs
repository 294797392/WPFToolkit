using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM.Interactivity
{
    /// <summary>
    /// Interaction类
    /// 
    /// BehaviorsProperty这个附加属性是连接XAML和ViewMode的桥梁，它也是完美实现ViewModel和Xaml分离的最重要的属性。它是直接和XAML打交道的，是要直接定义在XAML里的
    /// 如果我们直接在XAML里定义Behaviors，那么WPF在解析XAML的时候，发现Behaviors这个属性是空的，导致的结果就是会直接抛出一个（XamlObjectWriterException: “集合属性“System.Windows.Controls.Grid”.“Behaviors”为 null）的异常
    /// 后来经过查看其它MVVM框架的源码，发现在注册附加属性的时候，如果在附加属性的名字后面加一个Internal（参照BehaviorsProperty的注册方式），那么WPF就会先去调用Get访问器。。我们可以在Get访问器里初始化Behaviors这个集合，这样就不会抛异常了
    /// 
    /// WPF执行顺序：
    /// 1. 调用GetBehaviors函数（在这个函数里我们会初始化Behaviors属性）
    /// 2. 对XAML里定义的每个Behavior进行实例化
    /// 3. Behavior里如果定义了依赖属性并且设置了依赖属性回调，那么回调用Behavior -> 依赖项属性回调函数
    /// 4. 把Behavior加入到Behaviors里
    /// 5. Behaviors集合发生改变，发出CollectionChanged通知
    /// 
    /// 根据这个顺序，我们现在要做的事情就是要在Behaviors的CollectionChanged的时候初始化每个Behavior
    /// </summary>
    public static class Interaction
    {
        #region 附加属性

        public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("BehaviorsInternal", typeof(BehaviorCollection), typeof(Interaction), new PropertyMetadata(null));

        public static BehaviorCollection GetBehaviors(DependencyObject d)
        {
            BehaviorCollection behaviors = (BehaviorCollection)d.GetValue(BehaviorsProperty);
            if (behaviors == null)
            {
                behaviors = new BehaviorCollection();
                behaviors.AttachedObject = d;
                behaviors.CollectionChanged += Behaviors_CollectionChanged;
                d.SetValue(BehaviorsProperty, behaviors);
            }
            return behaviors;
        }

        public static void SetBehaviors(DependencyObject d, BehaviorCollection value)
        {
            d.SetValue(BehaviorsProperty, value);
        }

        #endregion

        /// <summary>
        /// XAML里的Behavior被加入到集合里了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Behaviors_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BehaviorCollection behaviors = sender as BehaviorCollection;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        if (e.NewItems == null || e.NewItems.Count == 0)
                        {
                            return;
                        }

                        Behavior behavior = e.NewItems[0] as Behavior;
                        behavior.AttachedObject = behaviors.AttachedObject;
                        behavior.Initialize();

                        break;
                    }
            }
        }
    }
}