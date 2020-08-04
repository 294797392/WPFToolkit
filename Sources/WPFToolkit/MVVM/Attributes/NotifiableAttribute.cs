using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 表示一个可通知的属性
    /// 动态创建ViewModel的时候，如果检测到属性上有这个特性，那么自动为属性附加PropertyChanged通知
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotifiableAttribute : Attribute
    {
    }
}