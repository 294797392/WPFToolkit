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
    /// 适用于所有ItemsControl的ViewModel
    /// </summary>
    public class ItemsViewModel<T> : ObservableCollection<T> where T : ItemViewModel
    {
        #region 实例变量

        private string id;
        private string name;
        private T selectedItem;
        private ObservableCollection<T> selectedItems;

        #endregion

        #region 属性

        public string ID
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

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

        public ItemsViewModel()
        {
            this.SelectedItems = new ObservableCollection<T>();
        }

        #endregion
    }
}