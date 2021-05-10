using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFToolkit.Business.Controls
{
    public class BusinessColumnAttribute : Attribute
    {
        public enum BusinessColumnType
        {
            /// <summary>
            /// 纯文本列
            /// </summary>
            Text,
        }

        /// <summary>
        /// 列标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 绑定的字段
        /// </summary>
        public string BindingField { get; set; }

        /// <summary>
        /// 列的类型
        /// </summary>
        public BusinessColumnType Type { get; set; }
    }

    /// <summary>
    /// BusinessDataGrid.xaml 的交互逻辑
    /// </summary>
    public partial class BusinessDataGrid : UserControl
    {
        #region Inner Class

        internal class DataGridJson
        {
            [JsonProperty("columns")]
            public List<BusinessColumn> ColumnList { get; set; }

            public DataGridJson()
            {
                this.ColumnList = new List<BusinessColumn>();
            }
        }

        /// <summary>
        /// 表示DataGrid里的一列
        /// </summary>
        internal class BusinessColumn
        {

        }

        #endregion

        #region 依赖属性

        public BusinessDataGridVM ViewModel
        {
            get { return (BusinessDataGridVM)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(BusinessDataGridVM), typeof(BusinessDataGrid), new PropertyMetadata(null));

        #endregion

        #region 构造方法

        public BusinessDataGrid()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }

            this.ViewModel = new BusinessDataGridVM();
            this.DataContext = this.ViewModel;

            this.InitializeDataGrid();
        }

        #endregion

        #region 实例方法

        private void InitializeDataGrid()
        {

        }

        #endregion
    }
}
