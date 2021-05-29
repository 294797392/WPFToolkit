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
    public class Images : MarkupExtension
    {
        private const string URIPattern = "pack://application:,,,/WPFToolkit;component/Images/{0}.png";

        /// <summary>
        /// 要显示的图像名字
        /// </summary>
        public string Name { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (string.IsNullOrEmpty(this.Name))
            {
                return null;
            }

            string uri = string.Format(URIPattern, this.Name);

            return ImageUtility.FromURI(uri);
        }
    }
}