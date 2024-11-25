using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit
{
    /// <summary>
    /// 动态换肤工具
    /// </summary>
    public static class ThemeManager
    {
        public static void ApplyDefaultTheme()
        {
            Application.Current.Resources.MergedDictionaries.Add(SharedDictionaryManager.SharedDictionary);
        }
    }
}
