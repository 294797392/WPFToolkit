using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 颜色选择器
    /// </summary>
    [TemplatePart(Name = "PART_Colors", Type = typeof(ListBox))]
    public class ColorPicker : Control
    {
        #region 实例变量

        private ListBox colorList;

        #endregion

        #region 依赖属性

        public Brush SelectedColor
        {
            get { return (Brush)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Brush), typeof(ColorPicker), new PropertyMetadata(null));

        #endregion

        #region 构造方法

        public ColorPicker()
        {
            this.Style = WPFToolkit.Controls.Resources.ColorPickerStyle;
        }

        #endregion

        #region 实例方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.colorList = this.FindName("PART_Colors") as ListBox;
        }

        #endregion
    }
}
