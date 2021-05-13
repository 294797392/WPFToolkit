using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit
{
    public static class DefaultStyles
    {
        private const string ResourceURI = "pack://application:,,,/WPFToolkit;component/DefaultStyles.xaml";

        public static Style DefaultListBoxStyle { get; private set; }

        public static Style DefaultListBoxItemStyle { get; private set; }

        static DefaultStyles()
        {
            ResourceDictionary resource = new ResourceDictionary()
            {
                Source = new Uri(ResourceURI)
            };

            DefaultListBoxStyle = resource["StyleListBox"] as Style;
            DefaultListBoxItemStyle = resource["StyleListBoxItem"] as Style;
        }
    }
}
