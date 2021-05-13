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
        public KTextBox()
        {
            this.Style = Templates.KTextBoxStyle;
        }
    }
}
