using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 管理KControl的样式和皮肤
    /// </summary>
    public static class Templates
    {
        private const string ResourceURI = "pack://application:,,,/WPFToolkit;component/Controls/Templates/element_ui.xaml";
        private const string DefaultThemeURI = "pack://application:,,,/WPFToolkit;component/Themes/Blue.xaml";

        public static Style KComboBoxStyle { get; private set; }

        public static Style KButtonStyle { get; private set; }

        public static Style KListBoxStyle { get; private set; }

        public static Style KColorPickerStyle { get; private set; }

        public static Style KTextBoxStyle { get; private set; }

        static Templates()
        {
            ResourceDictionary resource = new ResourceDictionary()
            {
                Source = new Uri(ResourceURI)
            };

            // 把资源文件加到应用程序里
            Application.Current.Resources.MergedDictionaries.Add(resource);
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(DefaultThemeURI) });

            KComboBoxStyle = resource["KComboBoxStyle"] as Style;
            KButtonStyle = resource["KButtonStyle"] as Style;
            KListBoxStyle = resource["KListBoxStyle"] as Style;
            KColorPickerStyle = resource["KColorPickerStyle"] as Style;
            KTextBoxStyle = resource["KTextBoxStyle"] as Style;
        }
    }
}
