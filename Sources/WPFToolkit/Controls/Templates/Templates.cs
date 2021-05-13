using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.Controls
{
    public static class Templates
    {
        private const string ResourceURI = "pack://application:,,,/WPFToolkit;component/Controls/Templates/element_ui.xaml";

        public static Style KComboBoxStyle { get; private set; }

        public static Style KButtonStyle { get; private set; }

        public static Style KListBoxStyle { get; private set; }

        static Templates()
        {
            ResourceDictionary resource = new ResourceDictionary()
            {
                Source = new Uri(ResourceURI)
            };

            // 把资源文件加到应用程序里
            Application.Current.Resources.MergedDictionaries.Add(resource);

            KComboBoxStyle = resource["KComboBoxStyle"] as Style;
            KButtonStyle = resource["KButtonStyle"] as Style;
            KListBoxStyle = resource["KListBoxStyle"] as Style;
        }
    }
}
