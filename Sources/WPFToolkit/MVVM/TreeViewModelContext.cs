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
        private Dictionary<string, TreeNodeViewModel> NodeMap { get; set; }

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

        public TreeViewModelContext()
        {
            this.SelectedItems = new ObservableCollection<TreeNodeViewModel>();
            this.CheckedItems = new ObservableCollection<TreeNodeViewModel>();
            this.NodeMap = new Dictionary<string, TreeNodeViewModel>();
        }

        /// <summary>
        /// 缓存TreeNode，方便以后查询
        /// </summary>
        /// <param name="node"></param>
        internal void Add(TreeNodeViewModel node)
        {
            this.NodeMap[node.ID.ToString()] = node;
        }

        internal void Remove(TreeNodeViewModel node)
        {
            this.NodeMap.Remove(node.ID.ToString());
        }

        /// <summary>
        /// 删除一个缓存的树形节点
        /// </summary>
        /// <param name="nodeID"></param>
        internal void Remove(string nodeID)
        {
            TreeNodeViewModel node;
            if (this.NodeMap.TryGetValue(nodeID, out node))
            {
                this.NodeMap.Remove(nodeID);
            }
        }

        /// <summary>
        /// 清空所有节点
        /// </summary>
        internal void Clear()
        {
            this.NodeMap.Clear();
        }

        public bool TryGetNode(string nodeID, out TreeNodeViewModel node)
        {
            return this.NodeMap.TryGetValue(nodeID, out node);
        }
    }
}


