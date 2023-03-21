using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using WPFToolkit;

namespace WPFToolkitDemo
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private const string DataTemplateXaml =
            @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                <TextBlock VerticalAlignment=""Center"" HorizontalAlignment=""Center"" Text=""{{Binding Path={0}, , StringFormat={{}}{1}}}""/>
            </DataTemplate>";

        static App()
        {
            string xaml = string.Format(DataTemplateXaml, "123", "{0}AAA");

            Console.WriteLine(xaml);
        }
    }
}
