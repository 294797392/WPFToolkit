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
        /// 不要使用Children.Add和Children.Remove去增加和删除子节点
        /// 请调用AddChildNode和RemoveChildNode方法去增加和删除子节点
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
                        if(this.Context != null) 
                        {
                            this.Context.SelectedItem = null;
                            this.Context.SelectedItems.Remove(this);
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

        /// <summary>
        /// 节点的等级
        /// </summary>
        public int Level { get; set; }

        #endregion

        #region 构造方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="context">树形列表上下文信息</param>
        /// <param name="data">该节点所包含的数据</param>
        public TreeNodeViewModel(TreeViewModelContext context, object data = null)
        {
            this.Context = context;
            this.Children = new ObservableCollection<TreeNodeViewModel>();
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
            this.Children.Add(node);
            this.Context.Add(node);
        }

        /// <summary>
        /// 从树形列表里移除自己
        /// </summary>
        public void Remove() 
        {
            this.Clear();

            if (this.Parent != null)
            {
                // 该节点是一个子节点
                this.Parent.Children.Remove(this);
            }
            else
            {
                // 该节点是一个根节点
                this.Context.Roots.Remove(this);
            }

            this.Context.Remove(this);
        }

        ///// <summary>
        ///// 删除一个子节点
        ///// </summary>
        ///// <param name="node">要删除的子节点</param>
        //public void Remove(TreeNodeViewModel node)
        //{
        //    node.Parent.Remove(node);

        //    node.Parent = null;
        //    this.Children.Remove(node);
        //    this.Context.Remove(node.ID.ToString());
        //}

        /// <summary>
        /// 清除所有子节点
        /// </summary>
        public void Clear() 
        {
            foreach (TreeNodeViewModel child in this.Children)
            {
                this.Context.Remove(child);

                child.Clear();
            }

            this.Children.Clear();
        }

        #endregion
    }
}