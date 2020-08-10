using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace WPFToolkit.MVVM.Interactivity
{
    public class BehaviorCollection : ObservableCollection<Behavior>
    {
        /// <summary>
        /// 记录被Behavior附加的UI对象
        /// </summary>
        internal DependencyObject AttachedObject { get; set; }
    }

    /// <summary>
    /// 可以附加到Xaml界面上的状态
    /// 继承DependencyObject的目的是为了让Behavior可以定义依赖项属性，从而可以在界面上进行绑定操作
    /// </summary>
    public abstract class Behavior : DependencyObject
    {
        /// <summary>
        /// 被Behavior附加的界面对象
        /// </summary>
        public DependencyObject AttachedObject { get; set; }

        /// <summary>
        /// 属性附加完成了（AttachedObject不为空），可以做初始化操作了
        /// </summary>
        internal abstract void Initialize();

        /// <summary>
        /// 释放Behavior占用的资源
        /// </summary>
        internal abstract void Release();
    }
}