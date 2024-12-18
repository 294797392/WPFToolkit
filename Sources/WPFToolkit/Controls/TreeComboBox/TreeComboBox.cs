﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using WPFToolkit.DragDrop;

namespace WPFToolkit.Controls
{
    [TemplatePart(Name = "PART_DropDownButton", Type = typeof(ToggleButton))]
    [TemplatePart(Name = "PART_Popup", Type = typeof(Popup))]
    public class TreeComboBox : TreeView
    {
        public DataTemplate SelectionBoxItemTemplate
        {
            get { return (DataTemplate)GetValue(SelectionBoxItemTemplateProperty); }
            set { SetValue(SelectionBoxItemTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionBoxItemTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionBoxItemTemplateProperty =
            DependencyProperty.Register("SelectionBoxItemTemplate", typeof(DataTemplate), typeof(TreeComboBox), new PropertyMetadata(null));

        public object SelectionBoxItem
        {
            get { return (object)GetValue(SelectionBoxItemProperty); }
            set { SetValue(SelectionBoxItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionBoxItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionBoxItemProperty =
            DependencyProperty.Register("SelectionBoxItem", typeof(object), typeof(TreeComboBox), new PropertyMetadata(null, SelectionBoxPropertyChangedCallback));

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsDropDownOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDropDownOpenProperty =
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(TreeComboBox), new PropertyMetadata(false, IsDropDownOpenPropertyChangedCallback));

        public double MaxDropDownHeight
        {
            get { return (double)GetValue(MaxDropDownHeightProperty); }
            set { SetValue(MaxDropDownHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxDropDownHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxDropDownHeightProperty =
            DependencyProperty.Register("MaxDropDownHeight", typeof(double), typeof(TreeComboBox), new PropertyMetadata(200D));

        public TreeComboBox()
        {
        }

        #region 实例方法

        #endregion

        #region 重写事件

        protected override void OnSelectedItemChanged(RoutedPropertyChangedEventArgs<object> e)
        {
            base.OnSelectedItemChanged(e);

            // 依赖项属性要调用SetValue设置值，如果直接赋值的话会导致绑定无效
            this.SetValue(SelectionBoxItemProperty, e.NewValue);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        /// <summary>
        /// When we have capture, all clicks off the popup will have the treeComboBox as the OriginalSource
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (Mouse.Captured == this && e.OriginalSource == this)
            {
                this.SetValue(IsDropDownOpenProperty, false);
            }
        }

        #endregion

        #region 依赖属性事件

        private static void IsDropDownOpenPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeComboBox treeComboBox = (TreeComboBox)d;
            treeComboBox.OnIsDropDownOpenPropertyChanged(e.OldValue, e.NewValue);
        }

        private void OnIsDropDownOpenPropertyChanged(object oldValue, object newValue)
        {
            bool isDropDownOpen = (bool)newValue;

            if (isDropDownOpen)
            {
                Mouse.Capture(this, CaptureMode.SubTree);
            }
            else
            {
                Mouse.Capture(null);
            }

            // 等TreeView的节点加载结束之后再执行选中某个TreeViewItem的操作
            base.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (this.SelectionBoxItem != null)
                {
                    DependencyObject dependencyObject = this.ItemContainerGenerator.ContainerFromItem(this.SelectionBoxItem);
                    if (dependencyObject != null)
                    {
                        dependencyObject.SetValue(TreeViewItem.IsSelectedProperty, true);
                    }
                }
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private static void SelectionBoxPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TreeComboBox treeComboBox = (TreeComboBox)d;
            treeComboBox.OnSelectionBoxPropertyChanged(e.OldValue, e.NewValue);
        }

        private void OnSelectionBoxPropertyChanged(object oldValue, object newValue)
        {
            if (newValue != null)
            {
                DependencyObject dependencyObject = this.ItemContainerGenerator.ContainerFromItem(this.SelectionBoxItem);
                if (dependencyObject != null)
                {
                    dependencyObject.SetValue(TreeViewItem.IsSelectedProperty, true);
                }

                base.SetValue(TreeComboBox.IsDropDownOpenProperty, false);
            }
        }

        #endregion
    }
}
