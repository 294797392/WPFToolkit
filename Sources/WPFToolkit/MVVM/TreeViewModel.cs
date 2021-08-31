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
    public class TreeViewModel : ViewModelBase
    {
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
        public TreeViewModelContext Context { get; private set; }

        #endregion

        #region 构造方法

        public TreeViewModel()
        {
            this.Roots = new ObservableCollection<TreeNodeViewModel>();
            this.Context = new TreeViewModelContext();
        }

        #endregion

        #region 公开接口

        public void AddRootNode(TreeNodeViewModel root)
        {
            this.Roots.Add(root);
            this.Context.NodeMap[root.ID.ToString()] = root;
        }

        /// <summary>
        /// 展开某个节点
        /// </summary>
        /// <param name="nodeID">要展开的节点ID</param>
        public void ExpandNode(string nodeID)
        {
            TreeNodeViewModel vm;
            if (!this.Context.NodeMap.TryGetValue(nodeID, out vm))
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
            if (!this.Context.NodeMap.TryGetValue(nodeID, out vm))
            {
                return;
            }

            vm.IsSelected = true;
        }

        #endregion

        #region 静态方法

        /// <summary>
        /// 加载子节点
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="childNodes">要加载的子节点</param>
        private static void LoadChildNodes<TNode>(TNode parentNode, List<InternalTreeNode> childNodes) where TNode : TreeNodeViewModel
        {
            foreach (InternalTreeNode treeNode in childNodes)
            {
                TNode nodeVM = ConfigFactory<TNode>.CreateInstance(typeof(TNode), parentNode, treeNode.Data);
                nodeVM.ID = treeNode.ID;
                nodeVM.Name = treeNode.Name;
                nodeVM.IconURI = treeNode.Icon;

                parentNode.AddChildNode(nodeVM);

                LoadChildNodes<TNode>(nodeVM, treeNode.Children);
            }
        }

        /// <summary>
        /// 从数据源创建TreeViewModel
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="sourceURI"></param>
        /// <returns></returns>
        public static TTreeVM Create<TTreeVM, TNode>(ItemsSourceType sourceType, string sourceURI) where TNode : TreeNodeViewModel where TTreeVM : TreeViewModel
        {
            InternalTreeView tv = null;

            switch (sourceType)
            {
                case ItemsSourceType.JSONFile:
                    {
                        if (!File.Exists(sourceURI))
                        {
                            return null;
                        }

                        tv = JSONHelper.ParseFile<InternalTreeView>(sourceURI);

                        break;
                    }

                default:
                    throw new NotImplementedException();
            }

            TreeViewModelContext context = new TreeViewModelContext();

            TTreeVM treeVM = ConfigFactory<TTreeVM>.CreateInstance(typeof(TTreeVM));

            foreach (InternalTreeNode treeNode in tv.NodeList)
            {
                TNode nodeVM = ConfigFactory<TNode>.CreateInstance(typeof(TNode), context, treeNode.Data);
                nodeVM.ID = treeNode.ID;
                nodeVM.Name = treeNode.Name;
                nodeVM.IconURI = treeNode.Icon;

                treeVM.AddRootNode(nodeVM);

                LoadChildNodes<TNode>(nodeVM, treeNode.Children);
            }

            return treeVM;
        }

        #endregion
    }
}