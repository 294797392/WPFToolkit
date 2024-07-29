using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.Attributes
{
    /// <summary>
    /// 描述DataGrid的一个列的信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DataGridColumnAttribute : Attribute
    {
        /// <summary>
        /// 该列的索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 列标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 该列使用的数据模板Key
        /// 如果没指定，那么会根据DataType类型去自动生成
        /// 如果指定了，那么会在被绑定的DataGrid里去寻找对应的DataTemplate
        /// </summary>
        public string DataTemplateKey { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 列宽度类型
        /// </summary>
        public DataGridLengthUnitType WidthUnitType { get; set; }

        /// <summary>
        /// 该列里的水平对齐方式
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment { get; set; }

        /// <summary>
        /// 是否可以排序
        /// </summary>
        public bool CanSort { get; set; }

        public DataGridColumnAttribute(string title)
        {
            this.Title = title;
            this.Width = 1;
            this.WidthUnitType = DataGridLengthUnitType.Star;
            this.HorizontalContentAlignment = HorizontalAlignment.Center;
            this.CanSort = true;
        }
    }
}
