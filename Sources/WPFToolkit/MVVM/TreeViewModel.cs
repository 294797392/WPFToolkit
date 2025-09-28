using DotNEToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.Attributes;
using WPFToolkit.MVVM.Internals;
using WPFToolkit.Utility;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 通用树形列表ViewModel
    /// 项目在开发的时候可以继承这个ViewModel，也可以直接使用该ViewModel
    /// </summary>
    public class TreeViewModel<TContext> : ViewModelBase where TContext : TreeViewModelContext
    {
        #region 实例变量

        private ObservableCollection<TreeNodeViewModel> roots;
        private TContext context;

        #endregion

        #region 属性

        /// <summary>
        /// 树形列表的根节点
        /// </summary>
        public IReadOnlyList<TreeNodeViewModel> Roots { get { return this.roots; } }

        /// <summary>
        /// 存储树形列表上下文信息
        /// </summary>
        public TContext Context { get { return this.context; } }

        /// <summary>
        /// 读取或设置树形列表的选中项
        /// </summary>
        public TreeNodeViewModel SelectedItem
        {
            get { return this.Context.SelectedItem; }
            set
            {
                if (this.context.SelectedItem != value)
                {
                    this.context.SelectedItem = value;
                    if (value != null)
                    {
                        value.IsSelected = true;
                    }
                    this.NotifyPropertyChanged("SelectedItem");
                }
            }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public TreeViewModel()
        {
            this.roots = new ObservableCollection<TreeNodeViewModel>();
            this.context = Activator.CreateInstance<TContext>();
            this.context.roots = this.roots;
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 增加一个根节点
        /// </summary>
        /// <param name="node">要增加的根节点</param>
        public void Add(TreeNodeViewModel node)
        {
            this.roots.Add(node);
            this.context.nodeMap[node.ID.ToString()] = node;
        }

        /// <summary>
        /// 增加多个根节点
        /// </summary>
        /// <param name="nodes"></param>
        public void Add(IEnumerable<TreeNodeViewModel> nodes)
        {
            foreach (TreeNodeViewModel node in nodes)
            {
                this.Add(node);
            }
        }

        /// <summary>
        /// 插入根节点到指定位置
        /// </summary>
        /// <param name="index"></param>
        /// <param name="node"></param>
        public void Insert(int index, TreeNodeViewModel node)
        {
            this.roots.Insert(index, node);
        }

        /// <summary>
        /// 移除指定的子节点
        /// </summary>
        /// <param name="node">要移除的根节点</param>
        public void Remove(TreeNodeViewModel node)
        {
            node.Remove();
        }

        /// <summary>
        /// 移除位于指定索引处的子节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index >= this.roots.Count)
            {
                return;
            }

            this.roots[index].Remove();
        }

        /// <summary>
        /// 从node处开始删除后面的所有元素（包含node）
        /// </summary>
        /// <param name="node"></param>
        public void Truncate(TreeNodeViewModel node)
        {
            int index = this.roots.IndexOf(node);
            if (index == -1)
            {
                return;
            }

            for (int i = index; i < this.roots.Count; i++)
            {
                this.roots[i].Remove();
            }
        }

        /// <summary>
        /// 删除所有节点
        /// 包括清空缓存的节点
        /// </summary>
        public void Clear()
        {
            this.roots.Clear();
            this.context.nodeMap.Clear();
            this.context.SelectedItem = null;
            this.context.SelectedItems.Clear();
            this.context.CheckedItems.Clear();
        }

        /// <summary>
        /// 获取一个节点VM
        /// </summary>
        /// <param name="nodeID">要获取的节点的ID</param>
        /// <param name="node">获取到的节点</param>
        /// <returns>是否存在节点</returns>
        public bool TryGetNode(string nodeID, out TreeNodeViewModel node)
        {
            return this.context.nodeMap.TryGetValue(nodeID, out node);
        }

        /// <summary>
        /// 获取指定类型的所有节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAllNodes<T>() where T : TreeNodeViewModel
        {
            return this.context.nodeMap.Values.OfType<T>();
        }

        /// <summary>
        /// 展开某个节点
        /// </summary>
        /// <param name="nodeID">要展开的节点ID</param>
        public void ExpandNode(string nodeID)
        {
            TreeNodeViewModel vm;
            if (!this.TryGetNode(nodeID, out vm))
            {
                // 不存在该节点
                return;
            }

            // 设置IsExpanded会对父节点进行递归展开
            vm.IsExpanded = true;
        }

        /// <summary>
        /// 选中某个节点
        /// </summary>
        /// <param name="nodeID">要选中的节点ID</param>
        /// <returns>如果选中成功则返回被选中的节点，否则返回null</returns>
        public TreeNodeViewModel SelectNode(string nodeID)
        {
            TreeNodeViewModel vm;
            if (!this.TryGetNode(nodeID, out vm))
            {
                return null;
            }

            vm.IsSelected = true;
            return vm;
        }

        /// <summary>
        /// 展开所有子节点
        /// </summary>
        public void ExpandAll()
        {
            foreach (TreeNodeViewModel treeNode in this.Roots)
            {
                this.ExpandAll(treeNode);
            }
        }

        /// <summary>
        /// 后序遍历树形列表
        /// 先处理子节点，再处理父节点
        /// </summary>
        /// <param name="visit"></param>
        /// <param name="forceVisit">如果为true，那么即使有一个失败的也继续遍历。如果为false，那么有一个失败的则直接退出遍历</param>
        /// <returns>是否所有的节点遍历都成功</returns>
        public bool PostOrderTraversal(Func<TreeNodeViewModel, bool> visit, bool forceVisit)
        {
            List<bool> results = new List<bool>();

            foreach (TreeNodeViewModel node in this.Roots)
            {
                bool success = this.PostOrderTraversal(results, node, visit, forceVisit);

                if (!success && !forceVisit)
                {
                    return false;
                }

                results.Add(success);
            }

            return results.All(x => x);
        }

        #endregion

        #region 实例方法

        private void ExpandAll(TreeNodeViewModel parentNode)
        {
            if (parentNode.children.Count > 0)
            {
                parentNode.IsExpanded = true;

                foreach (TreeNodeViewModel treeNode in parentNode.Children)
                {
                    if (treeNode.children.Count > 0)
                    {
                        treeNode.IsExpanded = true;

                        this.ExpandAll(treeNode);
                    }
                }
            }
        }

        private bool PostOrderTraversal(List<bool> results, TreeNodeViewModel parentNode, Func<TreeNodeViewModel, bool> visit, bool forceVisit)
        {
            if (parentNode == null)
            {
                return true;
            }

            // 1. 先遍历所有子节点（递归）
            foreach (TreeNodeViewModel child in parentNode.Children)
            {
                bool success = this.PostOrderTraversal(results, child, visit, forceVisit);

                if (!success && !forceVisit)
                {
                    return false;
                }

                results.Add(success);
            }

            // 2. 再访问当前节点
            return visit(parentNode);
        }

        #endregion
    }
}