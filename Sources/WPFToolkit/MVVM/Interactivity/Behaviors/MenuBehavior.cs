using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.MVVM.Interactivity.Behaviors
{
    public class MenuBehavior : Behavior
    {
        internal class MenuItem
        {

        }

        public ContentControl ContentHost
        {
            get { return (ContentControl)GetValue(ContentHostProperty); }
            set { SetValue(ContentHostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ContentHost.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentHostProperty =
            DependencyProperty.Register("ContentHost", typeof(ContentControl), typeof(MenuBehavior), new PropertyMetadata(null));



        public string MenuFile
        {
            get { return (string)GetValue(MenuFileProperty); }
            set { SetValue(MenuFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MenuFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MenuFileProperty =
            DependencyProperty.Register("MenuFile", typeof(string), typeof(MenuBehavior), new PropertyMetadata(null));



        internal override void Initialize()
        {
            if (this.AttachedObject is ListBox)
            {
                ListBox listBox = this.AttachedObject as ListBox;
                listBox.SelectionChanged += this.ListBox_SelectionChanged;
            }
            else
            {
                // 不支持其他控件作为导航
            }
        }

        internal override void Release()
        {
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
