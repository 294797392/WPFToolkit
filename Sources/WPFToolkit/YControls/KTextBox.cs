using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFToolkit.Controls
{
    public class KTextBox : TextBox
    {
        static KTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(KTextBox), new System.Windows.FrameworkPropertyMetadata(typeof(KTextBox)));
        }

        public KTextBox()
        {
        }
    }
}
