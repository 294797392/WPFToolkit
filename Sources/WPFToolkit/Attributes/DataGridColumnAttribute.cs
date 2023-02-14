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
        /// 列标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 列数据类型
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// 该列使用的数据模板URI
        /// 如果没指定，那么会根据DataType类型去自动生成（这个功能暂时没实现）
        /// </summary>
        public string DataTemplateURI { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 列宽度类型
        /// </summary>
        public DataGridLengthUnitType WidthUnitType { get; set; }

        public DataGridColumnAttribute(string title)
        {
            this.Title = title;
            this.Width = 1;
            this.WidthUnitType = DataGridLengthUnitType.Star;
        }
    }
}
