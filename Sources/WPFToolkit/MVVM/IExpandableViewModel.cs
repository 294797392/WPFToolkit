using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 表示一个可展开的项
    /// </summary>
    public interface IExpandableViewModel
    {
        /// <summary>
        /// 是否展开了项
        /// </summary>
        bool IsExpanded { get; set; }
    }
}