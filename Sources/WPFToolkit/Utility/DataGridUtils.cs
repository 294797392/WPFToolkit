using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DotNEToolkit.Utility;
using WPFToolkit.Attributes;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 为DataGrid附加功能
    /// </summary>
    public class DataGridUtils : DependencyObject
    {
        public static bool GetAutoGenerateColumn(DependencyObject obj)
        {
            return (bool)obj.GetValue(AutoGenerateColumnProperty);
        }

        public static void SetAutoGenerateColumn(DependencyObject obj, bool value)
        {
            obj.SetValue(AutoGenerateColumnProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoGenerateColumn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoGenerateColumnProperty =
            DependencyProperty.RegisterAttached("AutoGenerateColumn", typeof(bool), typeof(DataGridUtils), new PropertyMetadata(false, AutoGenerateColumnPropertyChangedCallback));

        private static void AutoGenerateColumnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            bool value = (bool)e.NewValue;
            if (!value)
            {
                return;
            }

            DataGrid dataGrid = d as DataGrid;

            dataGrid.Loaded += (sender, e1) => 
            {
                // 一项的数据类型
                Type itemType = GetAutoGenerateColumnDataType(dataGrid);

                // 反射获取所有带有DataGridColumn特性的属性
                List<PropertyAttribute<DataGridColumnAttribute>> properties = ReflectionUtils.GetPropertyAttribute<DataGridColumnAttribute>(itemType);
                if (properties == null || properties.Count == 0)
                {
                    return;
                }

                // 遍历并动态生成列
                foreach (PropertyAttribute<DataGridColumnAttribute> property in properties)
                {
                    DataGridColumnAttribute attribute = property.Attribute;

                    // 创建模板类型的数据
                    DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                    templateColumn.Header = attribute.Title;
                    templateColumn.Width = attribute.Width;
                    templateColumn.CellTemplate = string.IsNullOrEmpty(attribute.DataTemplateURI) ? null : dataGrid.FindResource(attribute.DataTemplateURI) as DataTemplate;

                    dataGrid.Columns.Add(templateColumn);
                }
            };
        }

        public static Type GetAutoGenerateColumnDataType(DependencyObject obj)
        {
            return (Type)obj.GetValue(AutoGenerateColumnDataTypeProperty);
        }

        public static void SetAutoGenerateColumnDataType(DependencyObject obj, Type value)
        {
            obj.SetValue(AutoGenerateColumnDataTypeProperty, value);
        }

        // Using a DependencyProperty as the backing store for AutoGenerateColumnDataType.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AutoGenerateColumnDataTypeProperty =
            DependencyProperty.RegisterAttached("AutoGenerateColumnDataType", typeof(Type), typeof(DataGridUtils), new PropertyMetadata(null));

    }
}
