using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.Attributes
{
    public enum ItemsSourceType
    {
        /// <summary>
        /// 文件数据源
        /// </summary>
        JSONFile,
    }

    public class ItemsSourceAttribute : Attribute
    {
        /// <summary>
        /// 数据源类型
        /// </summary>
        public ItemsSourceType SourceType { get; set; }

        /// <summary>
        /// 数据源的地址
        /// </summary>
        public string URI { get; set; }

        public ItemsSourceAttribute(ItemsSourceType sourceType, string uri)
        {
            this.SourceType = sourceType;
            this.URI = uri;
        }
    }
}
