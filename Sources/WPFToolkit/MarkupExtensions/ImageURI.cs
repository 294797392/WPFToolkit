using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using WPFToolkit.Utility;

namespace WPFToolkit.MarkupExtensions
{
    /// <summary>
    /// 提供把图片文件转换成ImageSource的功能
    /// </summary>
    public class ImageURI : MarkupExtension
    {
        /// <summary>
        /// 要显示的图像URI
        /// </summary>
        public string URI { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(this.URI))
            {
                return null;
            }

            return ImageUtility.FromURI(this.URI);
        }
    }
}