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
    public class ListViewModel : ViewModelBase
    {
        #region 实例变量

        private ListItemViewModel selectedItem;
        private ObservableCollection<ListItemViewModel> selectedItems;

        #endregion

        #region 属性

        /// <summary>
        /// 当前选中的项
        /// </summary>
        public ListItemViewModel SelectedItem
        {
            get { return this.selectedItem; }
            set
            {
                this.selectedItem = value;
                this.NotifyPropertyChanged("SelectedItem");
            }
        }

        public ObservableCollection<ListItemViewModel> SelectedItems
        {
            get { return this.selectedItems; }
            set
            {
                this.selectedItems = value;
                this.NotifyPropertyChanged("SelectedItems");
            }
        }

        #endregion

        #region 构造方法

        public ListViewModel()
        {
            this.SelectedItems = new ObservableCollection<ListItemViewModel>();
        }

        #endregion
    }
}