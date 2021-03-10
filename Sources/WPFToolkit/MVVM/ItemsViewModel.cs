using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 适用于所有ItemsControl的ViewModel
    /// </summary>
    public class ItemsViewModel<T> : ViewModelBase where T : ItemViewModel
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
                this.NotifyPropertyChanged("SelectedItem");
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
                this.NotifyPropertyChanged("SelectedItems");
            }
        }

        /// <summary>
        /// 所有的项集合
        /// </summary>
        public ObservableCollection<T> Items { get; private set; }

        #endregion

        #region 构造方法

        public ItemsViewModel()
        {
            this.SelectedItems = new ObservableCollection<T>();
            this.Items = new ObservableCollection<T>();
        }

        #endregion
    }
}