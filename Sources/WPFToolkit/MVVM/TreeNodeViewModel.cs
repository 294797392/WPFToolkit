using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 通用属性列表每个列表项的ViewModel
    /// </summary>
    public class TreeNodeViewModel : ItemViewModel
    {
        #region 实例变量

        internal ObservableCollection<TreeNodeViewModel> children;
        private TreeViewModelContext context;

        #endregion

        #region 属性

        /// <summary>
        /// 获取第一个根节点
        /// </summary>
        public TreeNodeViewModel FirstNode
        {
            get
            {
                if (this.children.Count == 0)
                {
                    return null;
                }

                return this.children[0];
            }
        }

        /// <summary>
        /// 获取最后一个根节点
        /// </summary>
        public TreeNodeViewModel LastNode
        {
            get
            {
                if (this.children.Count == 0)
                {
                    return null;
                }

                return this.children[this.children.Count - 1];
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNodeViewModel Parent { get; private set; }

        /// <summary>
        /// 该节点下的子节点列表
        /// 不要使用Children.Add和Children.Remove去增加和删除子节点
        /// 请调用AddChildNode和RemoveChildNode方法去增加和删除子节点
        /// </summary>
        public IReadOnlyList<TreeNodeViewModel> Children { get { return this.children; } }

        /// <summary>
        /// 存储该节点的用户自定义数据模型
        /// </summary>
        public object Data { get; private set; }

        /// <summary>
        /// 树形列表上下文信息
        /// </summary>
        public TreeViewModelContext Context { get { return this.context; } }

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

                    // 如果有父节点那么自动展开父节点
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
                            this.Context.SelectedItems.Add(this);
                        }
                    }
                    else
                    {
                        // 如果控件可以多选，那么this.Context.SelectedItem可能不是this
                        this.Context.SelectedItems.Remove(this);

                        if (this.Context != null && this.Context.SelectedItem == this)
                        {
                            // 设置选中列表里的第一个项为选中项
                            this.Context.SelectedItem = this.Context.SelectedItems.FirstOrDefault();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 设置是否选中当前节点
        /// 如果是选中，那么该操作会对上级节点进行递归选中
        /// </summary>
        public override bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyPropertyChanged("IsChecked");

                    if (value)
                    {
                        this.Context.CheckedItems.Add(this);

                        if (this.Parent != null)
                        {
                            this.Parent.IsChecked = true;
                        }
                    }
                    else
                    {
                        this.Context.CheckedItems.Remove(this);
                    }
                }
            }
        }

        /// <summary>
        /// 控制该节点是否显示
        /// </summary>
        public override bool IsVisible
        {
            get 
            {
                return this.isVisible;
            }
            set 
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    this.NotifyPropertyChanged("IsVisible");
                }
            }
        }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context">树形列表上下文信息</param>
        /// <param name="data">该节点所包含的数据</param>
        public TreeNodeViewModel(TreeViewModelContext context, object data = null)
        {
            this.context = context;
            this.children = new ObservableCollection<TreeNodeViewModel>();
            this.Data = data;
            this.IsVisible = true;
            this.IsChecked = false;
            this.IsSelected = false;
            this.IsExpanded = false;
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 增加一个子节点
        /// </summary>
        /// <param name="node">要增加的子节点</param>
        public void Add(TreeNodeViewModel node)
        {
            node.Parent = this;
            this.children.Add(node);
            this.context.nodeMap[node.ID.ToString()] = node;
        }

        public void Add(IEnumerable<TreeNodeViewModel> nodes)
        {
            foreach (TreeNodeViewModel node in nodes)
            {
                this.Add(node);
            }
        }

        /// <summary>
        /// 插入一个子节点
        /// </summary>
        /// <param name="index">要插入的位置</param>
        /// <param name="node">要插入的节点</param>
        public void Insert(int index, TreeNodeViewModel node)
        {
            this.children.Insert(index, node);

            node.Parent = this;
        }

        /// <summary>
        /// 移除位于指定索引处的子节点
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (index >= this.children.Count)
            {
                return;
            }

            this.children[index].Remove();
        }

        /// <summary>
        /// 移除最后一个元素
        /// </summary>
        public void RemoveLast()
        {
            TreeNodeViewModel toRemove = this.LastNode;

            if (toRemove != null) 
            {
                toRemove.Remove();
            }
        }

        /// <summary>
        /// 移除第一个元素
        /// </summary>
        public void RemoveFirst()
        {
            TreeNodeViewModel toRemove = this.FirstNode;

            if (toRemove != null) 
            {
                toRemove.Remove();
            }
        }


        /// <summary>
        /// 从node处开始删除后面的所有元素（包含node）
        /// </summary>
        /// <param name="node"></param>
        public void Truncate(TreeNodeViewModel node)
        {
            int index = this.children.IndexOf(node);
            if (index == -1)
            {
                return;
            }

            for (int i = index; i < this.children.Count; i++)
            {
                this.children[i].Remove();
            }
        }

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public void Clear() 
        {
            foreach (TreeNodeViewModel child in this.Children)
            {
                this.context.nodeMap.Remove(child.ID.ToString());

                child.Clear();

                child.Parent = null;
            }

            this.children.Clear();
        }

        #endregion

        /// <summary>
        /// 从树形列表里移除自己
        /// </summary>
        internal void Remove()
        {
            this.Clear();

            if (this.Parent != null)
            {
                // 该节点是一个子节点
                this.Parent.children.Remove(this);
            }
            else
            {
                // 该节点是一个根节点
                this.context.roots.Remove(this);
            }

            this.context.nodeMap.Remove(this.ID.ToString());

            this.IsSelected = false;

            this.Parent = null;
        }
    }
}