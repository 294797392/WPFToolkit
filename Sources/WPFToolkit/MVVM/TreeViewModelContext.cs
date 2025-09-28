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
        /// 存储所有节点列表
        /// </summary>
        internal Dictionary<string, TreeNodeViewModel> nodeMap;
        internal ObservableCollection<TreeNodeViewModel> roots;


        /// <summary>
        /// 当前选中的节点
        /// </summary>
        public TreeNodeViewModel SelectedItem { get; internal set; }

        /// <summary>
        /// 当前选中的节点集合
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> SelectedItems { get; internal set; }

        /// <summary>
        /// 当前选中的节点集合
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> CheckedItems { get; internal set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeViewModelContext()
        {
            this.SelectedItems = new ObservableCollection<TreeNodeViewModel>();
            this.CheckedItems = new ObservableCollection<TreeNodeViewModel>();
            this.nodeMap = new Dictionary<string, TreeNodeViewModel>();
        }
    }
}


