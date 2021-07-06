using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFToolkit.MVVM;

namespace WPFToolkit.Business.Controls
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataGridColumnAttribute : Attribute
    {
        /// <summary>
        /// 列标题
        /// </summary>
        public object Title { get; set; }

        /// <summary>
        /// 格式化信息
        /// </summary>
        public string Format { get; set; }

        public DataGridColumnAttribute(object title)
        {
            this.Title = title;
            this.Format = "{0}";
        }
    }

    /// <summary>
    /// 表示一个DataGrid的动作
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class DataGridActionAttribute : Attribute
    {
        public string Name { get; set; }

        public string ImageURI { get; set; }
    }

    /// <summary>
    /// BusinessDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class BusinessDataGrid : UserControl
    {
        private const string DataTemplateXaml =
            @"<DataTemplate xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
                <TextBlock VerticalAlignment=""Center"" HorizontalAlignment=""Center"" Text=""{{Binding Path={0}, StringFormat={{}}{1}}}""/>
            </DataTemplate>";

        #region 实例变量

        private Type itemType;

        #endregion

        #region 依赖属性

        /// <summary>
        /// 每一项的类型
        /// </summary>
        public Type ItemType
        {
            get { return this.itemType; }
            set
            {
                this.itemType = value;

                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    return;
                }
                this.GenerateColumns(value);
            }
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ItemsSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(BusinessDataGrid), new PropertyMetadata(null));

        #endregion

        #region 构造方法

        public BusinessDataGrid()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            this.InitializeDataGrid();
        }

        #endregion

        #region 实例方法

        private void InitializeDataGrid()
        {

        }

        private void GenerateColumns(Type itemType)
        {
            DataGrid.Columns.Clear();

            PropertyInfo[] properties = itemType.GetProperties();
            if (properties == null || properties.Length == 0)
            {
                return;
            }

            foreach (PropertyInfo property in properties)
            {
                DataGridColumnAttribute columnAttribute = property.GetCustomAttribute<DataGridColumnAttribute>();
                if (columnAttribute == null)
                {
                    continue;
                }

                if (property.PropertyType == typeof(int) || property.PropertyType == typeof(double) ||
                    property.PropertyType == typeof(float) || property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(DateTime))
                {
                    string xaml = string.Format(DataTemplateXaml, property.Name, columnAttribute.Format);

                    DataGridTemplateColumn templateColumn = new DataGridTemplateColumn()
                    {
                        Header = columnAttribute.Title,
                        CellTemplate = (DataTemplate)XamlReader.Parse(xaml),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star)      // 平分空间
                    };

                    DataGrid.Columns.Add(templateColumn);
                }
                else
                {
                    // TODO：处理其他类型的列
                    string xaml = string.Format(DataTemplateXaml, property.Name, columnAttribute.Format);

                    DataGridTemplateColumn templateColumn = new DataGridTemplateColumn()
                    {
                        Header = columnAttribute.Title,
                        CellTemplate = (DataTemplate)XamlReader.Parse(xaml),
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star)      // 平分空间
                    };

                    DataGrid.Columns.Add(templateColumn);
                }
            }
        }

        private void InitializeActions()
        {

        }

        #endregion
    }
}
