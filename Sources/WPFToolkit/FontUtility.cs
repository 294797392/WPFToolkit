using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 对字体的帮助类
    /// </summary>
    public static class FontUtility
    {
        public static List<FontFamily> GetFontFamily()
        {
            ICollection<FontFamily> fonts = Fonts.SystemFontFamilies;
            return fonts.Select(v => new FontFamily(v.Source)).ToList();
        }
    }
}

