using System;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFToolkitDemo.UserControls
{
    /// <summary>
    /// ColorListUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class ColorListUserControl : UserControl
    {
        public ColorListUserControl()
        {
            InitializeComponent();

            this.InitializeUserControl();
        }

        private void InitializeUserControl()
        {
            // 取得保存颜色命名的类类型
            Type colorType = typeof(Colors);

            // 利用反射，得到所有颜色
            var colorNames = from MemberInfo color in colorType.GetMembers()
                             where color.MemberType == MemberTypes.Property
                             select color.Name;

            // 通过类型转换和颜色名，得到对应颜色实例
            var colorList = from name in colorNames
                            let color = (Color)TypeDescriptor.
                                                GetConverter(typeof(Color)).
                                                ConvertFromInvariantString(name)
                            select new { Name = name, Color = color };


            ListBoxColorList.ItemsSource = colorList.Select(v => new SolidColorBrush(v.Color)).ToList();
        }
    }
}
