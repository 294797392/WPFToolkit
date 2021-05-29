using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.Themes
{
    internal class SharedDictionaryManager
    {
        internal static ResourceDictionary SharedDictionary
        {
            get
            {
                if (_sharedDictionary == null)
                {
                    _sharedDictionary = new ResourceDictionary() 
                    {
                        Source = new Uri("pack://application:,,,/WPFToolkit;component/Themes/Blue.xaml")
                    };
                }

                return _sharedDictionary;
            }
        }

        private static ResourceDictionary _sharedDictionary;
    }
}
