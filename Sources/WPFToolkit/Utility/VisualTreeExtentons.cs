using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 提供对VisualTree的扩展函数
    /// </summary>
    public static class VisualTreeExtention
    {
        public static TChild FindVisualChild<TChild>(this DependencyObject parent) where TChild : DependencyObject
        {
            throw new NotImplementedException();
        }
    }
}