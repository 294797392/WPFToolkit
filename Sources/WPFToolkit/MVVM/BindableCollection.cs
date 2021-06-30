using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// ObservableCollection的增强版
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindableCollection<T> : ObservableCollection<T>
    {
        #region 实例变量

        private T selectedItem;
        private ObservableCollection<T> selectedItems;

        #endregion

        #region 属性

        /// <summary>
        /// 当前选中的项
        /// </summary>
        public T SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                base.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));
            }
        }

        /// <summary>
        /// 当前选中的项集合
        /// </summary>
        public ObservableCollection<T> SelectedItems
        {
            get { return this.selectedItems; }
            set
            {
                this.selectedItems = value;
                base.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItems"));
            }
        }

        #endregion

        #region 构造方法

        public BindableCollection()
        {
            this.SelectedItems = new ObservableCollection<T>();
        }

        #endregion
    }
}
