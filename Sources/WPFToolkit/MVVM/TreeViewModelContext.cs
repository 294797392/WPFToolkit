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

        private List<TreeNodeViewModel> Nodes { get; set; }

        /// <summary>
        /// 根节点列表
        /// </summary>
        internal ObservableCollection<TreeNodeViewModel> Roots { get; set; }

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
        /// 获取所有的节点
        /// </summary>
        public IEnumerable<TreeNodeViewModel> NodeList
        {
            get { return this.Nodes; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public TreeViewModelContext()
        {
            this.SelectedItems = new ObservableCollection<TreeNodeViewModel>();
            this.CheckedItems = new ObservableCollection<TreeNodeViewModel>();
            this.NodeMap = new Dictionary<string, TreeNodeViewModel>();
            this.Nodes = new List<TreeNodeViewModel>();
        }

        /// <summary>
        /// 缓存TreeNode，方便以后查询
        /// </summary>
        /// <param name="node"></param>
        internal void Add(TreeNodeViewModel node)
        {
            this.NodeMap[node.ID.ToString()] = node;
            this.Nodes.Add(node);
        }

        internal void Remove(TreeNodeViewModel node)
        {
            this.Remove(node.ID.ToString());
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
                this.Nodes.Remove(node);
            }
        }

        /// <summary>
        /// 清空所有节点
        /// </summary>
        internal void Clear()
        {
            this.NodeMap.Clear();
            this.Nodes.Clear();
        }

        public bool TryGetNode(string nodeID, out TreeNodeViewModel node)
        {
            return this.NodeMap.TryGetValue(nodeID, out node);
        }
    }
}


