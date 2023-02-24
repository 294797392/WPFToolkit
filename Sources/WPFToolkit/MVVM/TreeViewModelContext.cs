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
        /// 获取树形列表里的所有的节点列表
        /// </summary>
        public List<TreeNodeViewModel> NodeList { get; private set; }

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
            this.NodeList = new List<TreeNodeViewModel>();
        }

        /// <summary>
        /// 缓存TreeNode，方便以后查询
        /// </summary>
        /// <param name="node"></param>
        public void AddNode(TreeNodeViewModel node)
        {
            this.NodeMap[node.ID.ToString()] = node;
            this.NodeList.Add(node);
        }

        /// <summary>
        /// 删除一个缓存的树形节点
        /// </summary>
        /// <param name="nodeID"></param>
        public void RemoveNode(string nodeID)
        {
            TreeNodeViewModel node;
            if (this.NodeMap.TryGetValue(nodeID, out node))
            {
                this.NodeMap.Remove(nodeID);
                this.NodeList.Remove(node);
            }
        }

        public bool TryGetNode(string nodeID, out TreeNodeViewModel node)
        {
            return this.NodeMap.TryGetValue(nodeID, out node);
        }

        /// <summary>
        /// 根据条件查询节点，如果没查询到则返回null
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T QueryNode<T>(Func<T, bool> predicate) where T : TreeNodeViewModel
        {
            return this.NodeList.Cast<T>().FirstOrDefault(predicate);
        }
    }
}


