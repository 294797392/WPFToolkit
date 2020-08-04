using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.MarkupExtensions;
using WPFToolkit.Attributes;

namespace WPFClient
{
    /// <summary>
    /// DemoEnumeration类
    /// </summary>
    public enum DemoEnumeration
    {
        [EnumMember("第一个")]
        One,

        [EnumMember("第二个")]
        Two,

        [EnumMember("第三个")]
        Three
    }
}