using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    public class MenuItemVM : ItemViewModel
    {
        private bool isInitialized;

        /// <summary>
        /// 界面入口点
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// 该菜单是否初始化完了
        /// </summary>
        public bool IsInitialized 
        {
            get { return this.isInitialized; }
            set
            {
                this.isInitialized = value;
                this.NotifyPropertyChanged("IsInitialized");
            }
        }
    }
}
