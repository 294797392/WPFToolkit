﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 树形结构的菜单节点
    /// </summary>
    public class MenuItemVM : ItemViewModel
    {
        /// <summary>
        /// 在ContentVM初始化之前调用
        /// </summary>
        public event Action<MenuItemVM, ViewModelBase, DependencyObject> ContentInitializing;

        #region 实例变量

        private bool isInitialized;
        private FrameworkElement content;
        internal MenuContext context;

        #endregion

        #region 属性

        /// <summary>
        /// 界面入口点
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// ViewModel的完整类名
        /// </summary>
        public string VMClassName { get; set; }

        /// <summary>
        /// 该菜单所要显示的内容
        /// </summary>
        public FrameworkElement Content
        {
            get { return this.content; }
            internal set
            {
                this.content = value;
                this.NotifyPropertyChanged("Content");
            }
        }

        /// <summary>
        /// 该菜单所显示的内容的ViewModel
        /// </summary>
        public ViewModelBase ContentVM { get; internal set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        public ObservableCollection<MenuItemVM> MenuItems { get; private set; }

        /// <summary>
        /// 输入参数
        /// 该参数会传递到MenuContentVM里
        /// </summary>
        public IDictionary Parameters { get; set; }

        /// <summary>
        /// 当前节点是否选中
        /// </summary>
        public override bool IsSelected
        {
            get
            {
                return this.isSelected;
            }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.NotifyPropertyChanged("IsSelected");

                    if (value)
                    {
                        if (this.context.SelectedItem != this)
                        {
                            this.context.SelectedItem = this;
                        }
                    }
                    else
                    {
                        if (this.context.SelectedItem == this)
                        {
                            this.context.SelectedItem = null;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 父节点
        /// </summary>
        public MenuItemVM Parent { get; internal set; }

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

        #endregion

        #region 构造方法

        public MenuItemVM(MenuDefinition menu)
        {
            this.MenuItems = new ObservableCollection<MenuItemVM>();
            this.ID = menu.ID;
            this.Name = menu.Name;
            this.ClassName = menu.ClassName;
            this.VMClassName = menu.VMClassName;
            this.IconURI = menu.Icon;
            this.Parameters = menu.Parameters;
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 新加一个子节点
        /// </summary>
        /// <param name="menuItem"></param>
        public void AddMenuItem(MenuItemVM menuItem)
        {
            menuItem.context = this.context;
            menuItem.Parent = this;
            this.MenuItems.Add(menuItem);
        }

        #endregion

        #region Internal

        internal void RaiseContentInitializing(ViewModelBase viewModel, DependencyObject view) 
        {
            if (this.ContentInitializing != null) 
            {
                this.ContentInitializing(this, viewModel, view);
            }
        }

        #endregion
    }
}
