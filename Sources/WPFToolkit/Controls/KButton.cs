using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPFToolkit.Controls
{
    public class KButton : Button
    {
        public bool? IsChecked
        {
            get { return (bool?)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool?), typeof(KButton), new PropertyMetadata(null));


        public bool CanChecked
        {
            get { return (bool)GetValue(CanCheckedProperty); }
            set { SetValue(CanCheckedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanChecked.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanCheckedProperty =
            DependencyProperty.Register("CanChecked", typeof(bool), typeof(KButton), new PropertyMetadata(false));


        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CornerRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(KButton), new PropertyMetadata(null));


        public KButton()
        {
            this.Style = Templates.KButtonStyle;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (this.CanChecked)
            {
                if (this.IsChecked == null)
                {
                    this.IsChecked = true;
                }
                else
                {
                    this.IsChecked = !this.IsChecked;
                }
            }
        }
    }
}
