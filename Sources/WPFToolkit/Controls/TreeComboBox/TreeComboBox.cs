using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using WPFToolkit.DragDrop;

namespace WPFToolkit.Controls
{
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
            DependencyProperty.Register("IsDropDownOpen", typeof(bool), typeof(TreeComboBox), new PropertyMetadata(false));



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

        private void HandleSelectionBoxItemChanged()
        {
            if (this.SelectionBoxItem != null)
            {
                DependencyObject dependencyObject = this.ItemContainerGenerator.ContainerFromItem(this.SelectionBoxItem);
                if (dependencyObject != null)
                {
                    dependencyObject.SetValue(TreeViewItem.IsSelectedProperty, true);
                }
            }
            else
            {
            }
        }

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

            base.ItemContainerGenerator.StatusChanged += ItemContainerGenerator_StatusChanged;
        }

        #endregion

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            switch (this.ItemContainerGenerator.Status)
            {
                case GeneratorStatus.ContainersGenerated:
                    {
                        this.HandleSelectionBoxItemChanged();
                        break;
                    }

                default:
                    break;
            }
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
                this.HandleSelectionBoxItemChanged();

                base.SetValue(TreeComboBox.IsDropDownOpenProperty, false);
            }
        }
    }
}
