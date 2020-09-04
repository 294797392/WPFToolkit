using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 表示一个可选中的项
    /// </summary>
    public interface ISelectableViewModel
    {
        /// <summary>
        /// 当前是否被选中
        /// </summary>
        bool IsSelected { get; set; }
    }
}