using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 可以对ItemsControl等控件进行框选
    /// </summary>
    public static class SelectionUtility
    {
        public static bool GetEnableSelection(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnableSelectionProperty);
        }

        public static void SetEnableSelection(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableSelectionProperty, value);
        }

        public static readonly DependencyProperty EnableSelectionProperty =
            DependencyProperty.RegisterAttached("EnableSelection", typeof(bool), typeof(SelectionUtility), new PropertyMetadata(false));
    }
}