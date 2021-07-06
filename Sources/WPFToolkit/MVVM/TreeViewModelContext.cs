using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// 当前选中的节点集合
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> SelectedItems { get; internal set; }

        public TreeViewModelContext()
        {
            this.SelectedItems = new ObservableCollection<TreeNodeViewModel>();
        }
    }
}