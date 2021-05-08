using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 颜色选择器
    /// </summary>
    public class ColorPicker : Control
    {
        #region 实例变量

        private ListBox colorList;

        #endregion

        public ColorPicker()
        {
            this.Style = WPFToolkit.Controls.Resources.ColorPickerStyle;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
