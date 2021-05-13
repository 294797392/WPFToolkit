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
    public class KComboBox : ComboBox
    {
        public Brush SelectionsBackground
        {
            get { return (Brush)GetValue(SelectionsBackgroundProperty); }
            set { SetValue(SelectionsBackgroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionsBackground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionsBackgroundProperty =
            DependencyProperty.Register("SelectionsBackground", typeof(Brush), typeof(KComboBox), new PropertyMetadata(null));


        public Brush SelectionsBorder
        {
            get { return (Brush)GetValue(SelectionsBorderProperty); }
            set { SetValue(SelectionsBorderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionsBorder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionsBorderProperty =
            DependencyProperty.Register("SelectionsBorder", typeof(Brush), typeof(KComboBox), new PropertyMetadata(null));


        public Thickness SelectionsThickness
        {
            get { return (Thickness)GetValue(SelectionsThicknessProperty); }
            set { SetValue(SelectionsThicknessProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectionsThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionsThicknessProperty =
            DependencyProperty.Register("SelectionsThickness", typeof(Thickness), typeof(KComboBox), new PropertyMetadata(null));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(KComboBox), new PropertyMetadata(null));





        public KComboBox()
        {
            this.Style = Templates.KComboBoxStyle;
        }
    }
}
