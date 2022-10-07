using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 通用属性列表每个列表项的ViewModel
    /// </summary>
    public class TreeNodeViewModel : ItemViewModel
    {
        #region 属性

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNodeViewModel Parent { get; private set; }

        /// <summary>
        /// 该节点下的子节点列表
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> Children { get; private set; }

        /// <summary>
        /// 存储该节点的用户自定义数据模型
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 树形列表上下文信息
        /// </summary>
        public TreeViewModelContext Context { get; private set; }

        /// <summary>
        /// 设置是否展开该节点
        /// 如果是展开，那么该操作会对上级节点进行递归展开
        /// </summary>
        public override bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    this.NotifyPropertyChanged("IsExpanded");

                    if (value && this.Parent != null)
                    {
                        this.Parent.IsExpanded = value;
                    }
                }
            }
        }

        /// <summary>
        /// 设置是否选中当前节点
        /// 如果是选中，那么该操作会对上级节点进行递归展开
        /// </summary>
        public override bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.NotifyPropertyChanged("IsSelected");

                    if (value)
                    {
                        // 如果选中当前节点，那么自动展开父节点
                        if (this.Parent != null)
                        {
                            this.Parent.IsExpanded = true;
                        }

                        // 记录当前选中的节点
                        if (this.Context != null)
                        {
                            this.Context.SelectedItem = this;
                        }
                    }
                }
            }
        }

        #endregion

        public TreeNodeViewModel(TreeViewModelContext context, object data = null)
        {
            this.Context = context;
            this.Children = new ObservableCollection<TreeNodeViewModel>();
            this.Data = data;
        }

        public void AddChildNode(TreeNodeViewModel node)
        {
            node.Parent = this;
            this.Children.Add(node);
            this.Context.NodeMap[node.ID.ToString()] = node;
        }

        public void RemoveChildNode(TreeNodeViewModel node)
        {
            node.Parent = null;
            this.Children.Remove(node);
            this.Context.NodeMap.Remove(node.ID.ToString());
        }
    }
}