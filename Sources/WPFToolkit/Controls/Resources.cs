using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.Controls
{
    public static class Resources
    {
        public static Style ColorPickerStyle { get; private set; }

        static Resources()
        {
            ColorPickerStyle = (Application.LoadComponent(new Uri("pack://application:,,,/WPFToolkit;component/Controls/ColorPicker.xaml")) as ResourceDictionary)["StyleColorPicker"] as Style;
        }
    }
}
