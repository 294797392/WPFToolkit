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
        /// <summary>
        /// 指定某个动作的选项
        /// </summary>
        public enum ActionOptions
        {
            /// <summary>
            /// 没有选项
            /// </summary>
            None,

            /// <summary>
            /// 递归对子节点执行同样的动作
            /// </summary>
            Recursion
        }

        #region 实例变量

        #endregion

        #region 属性

        /// <summary>
        /// 树形列表的根节点
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> Roots { get; private set; }

        /// <summary>
        /// 存储树形列表上下文信息
        /// </summary>
        public TContext Context { get; private set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public TreeViewModel()
        {
            this.Roots = new ObservableCollection<TreeNodeViewModel>();

            this.Context = Activator.CreateInstance<TContext>();
            this.Context.Roots = this.Roots;
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 增加一个根节点
        /// </summary>
        /// <param name="root">要增加的根节点</param>
        public void AddRootNode(TreeNodeViewModel root)
        {
            this.Roots.Add(root);
            this.Context.Add(root);
        }

        /// <summary>
        /// 删除所有节点
        /// 包括清空缓存的节点
        /// </summary>
        public void ClearNodes()
        {
            this.Roots.Clear();
            this.Context.Clear();
        }

        /// <summary>
        /// 获取一个节点VM
        /// </summary>
        /// <param name="nodeID">要获取的节点的ID</param>
        /// <param name="node">获取到的节点</param>
        /// <returns>是否存在节点</returns>
        public bool TryGetNode(string nodeID, out TreeNodeViewModel node)
        {
            return this.Context.TryGetNode(nodeID, out node);
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
        public void SelectNode(string nodeID)
        {
            TreeNodeViewModel vm;
            if (!this.TryGetNode(nodeID, out vm))
            {
                return;
            }

            vm.IsSelected = true;
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

        #endregion

        #region 实例方法

        private void ExpandAll(TreeNodeViewModel parentNode)
        {
            if (parentNode.Children.Count > 0)
            {
                parentNode.IsExpanded = true;

                foreach (TreeNodeViewModel treeNode in parentNode.Children)
                {
                    if (treeNode.Children.Count > 0)
                    {
                        treeNode.IsExpanded = true;

                        this.ExpandAll(treeNode);
                    }
                }
            }
        }

        #endregion

        #region 静态方法

        #endregion
    }
}