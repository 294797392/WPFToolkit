using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DotNEToolkit.Utility;
using WPFToolkit.Attributes;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 为DataGrid附加功能
    /// </summary>
    public class DataGridUtils : DependencyObject
    {
        #region 类方法

        private static DataTemplate CreateDefaultDataTemplate(PropertyAttribute<DataGridColumnAttribute> attribute)
        {
            DataTemplate dataTemplate = new DataTemplate();

            FrameworkElementFactory grid = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory textBlock = new FrameworkElementFactory(typeof(TextBlock));
            textBlock.SetValue(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center);
            textBlock.SetValue(TextBlock.HorizontalAlignmentProperty, attribute.Attribute.HorizontalContentAlignment);
            textBlock.SetBinding(TextBlock.TextProperty, new Binding(attribute.Property.Name));
            grid.AppendChild(textBlock);

            dataTemplate.VisualTree = grid;

            return dataTemplate;
        }

        #endregion

        private static void AutoGenerateColumnDataTypePropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                return;
            }

            DataGrid dataGrid = d as DataGrid;

            // Loaded之后才能找到DataGrid里的资源
            // 设置了DataTemplateKey之后会去找资源
            dataGrid.Loaded += GenerateDataGridColumns;
        }

        private static void GenerateDataGridColumns(object sender, RoutedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            Type itemType = GetAutoGenerateColumnDataType(dataGrid);

            // 反射获取所有带有DataGridColumn特性的属性
            List<PropertyAttribute<DataGridColumnAttribute>> properties = ReflectionUtils.GetPropertyAttribute<DataGridColumnAttribute>(itemType);
            if (properties == null || properties.Count == 0)
            {
                return;
            }

            List<PropertyAttribute<DataGridColumnAttribute>> orderedProperties = properties.OrderBy(v => v.Attribute.Index).ToList();

            // 遍历并动态生成列
            foreach (PropertyAttribute<DataGridColumnAttribute> property in orderedProperties)
            {
                DataGridColumnAttribute attribute = property.Attribute;

                // 创建模板类型的数据
                DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
                templateColumn.CanUserSort = attribute.CanSort;
                templateColumn.Header = attribute.Title;
                templateColumn.Width = new DataGridLength(attribute.Width, attribute.WidthUnitType);

                // 设置DataTemplate
                DataTemplate dataTemplate = null;
                if (!string.IsNullOrEmpty(attribute.DataTemplateKey))
                {
                    dataTemplate = dataGrid.TryFindResource(attribute.DataTemplateKey) as DataTemplate;
                }
                if (dataTemplate == null)
                {
                    dataTemplate = CreateDefaultDataTemplate(property);
                }

                templateColumn.CellTemplate = dataTemplate;

                dataGrid.Columns.Add(templateColumn);
            }

            dataGrid.Loaded -= GenerateDataGridColumns;
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
            DependencyProperty.RegisterAttached("AutoGenerateColumnDataType", typeof(Type), typeof(DataGrid), new PropertyMetadata(null, AutoGenerateColumnDataTypePropertyChangedCallback));
    }
}
