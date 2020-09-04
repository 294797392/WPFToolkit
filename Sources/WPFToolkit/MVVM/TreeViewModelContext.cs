using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.MVVM.DataProviders;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 存储树形列表ViewModel共享的数据
    /// </summary>
    public class TreeViewModelContext
    {
        /// <summary>
        /// 当前选中的节点
        /// </summary>
        public TreeNodeViewModel SelectedItem { get; internal set; }

        /// <summary>
        /// 数据提供器
        /// </summary>
        internal IDataProvider DataProvider { get; set; }
    }
}