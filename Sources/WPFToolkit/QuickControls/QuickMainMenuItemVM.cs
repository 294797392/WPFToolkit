using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFToolkit.MVVM;

namespace WPFToolkit.QuickControls
{
    public class QuickMainMenuItemVM : ItemViewModel
    {
        private string entryClass;

        /// <summary>
        /// 菜单所对应的界面的入口类名
        /// </summary>
        public string EntryClass 
        {
            get { return this.entryClass; }
            set
            {
                this.entryClass = value;
                this.NotifyPropertyChanged("EntryClass");
            }
        }

        /// <summary>
        /// 实例
        /// </summary>
        public DependencyObject Content { get; set; }
    }
}
