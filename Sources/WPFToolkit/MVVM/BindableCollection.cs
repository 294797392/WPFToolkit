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
        /// <summary>
        /// 当选项改变的时候触发
        /// </summary>
        public event Action<T, T> SelectionChanged;

        #region 实例变量

        private T selectedItem;
        private ObservableCollection<T> selectedItems;
        private string id;
        private string name;

        #endregion

        #region 属性

        public string ID
        {
            get { return this.id; }
            set
            {
                this.id = value;
                base.OnPropertyChanged(new PropertyChangedEventArgs("ID"));
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                base.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
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
                if (this.selectedItem == null ||
                    !this.selectedItem.Equals(value))
                {
                    T oldValue = this.selectedItem;

                    this.selectedItem = value;
                    base.OnPropertyChanged(new PropertyChangedEventArgs("SelectedItem"));

                    if (this.SelectionChanged != null)
                    {
                        this.SelectionChanged(oldValue, value);
                    }
                }
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

        #region 公开接口

        /// <summary>
        /// 把一个元素往上移
        /// </summary>
        public void MoveUp(T item)
        {
            int index = this.IndexOf(item);
            if (index <= 0)
            {
                return;
            }

            this.Move(index, index - 1);
        }

        /// <summary>
        /// 把一个元素往下移
        /// </summary>
        /// <param name="item"></param>
        public void MoveDown(T item)
        {
            int index = this.IndexOf(item);
            if (index < 0 || index == this.Count - 1)
            {
                return;
            }

            this.Move(index, index + 1);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 选中下一个项目
        /// </summary>
        public void SelectNext() 
        {
            if (this.SelectedItem == null) 
            {
                this.SelectedItem = this[0];
                return;
            }

            int index = this.IndexOf(this.SelectedItem);
            if (index < 0) 
            {
                this.SelectedItem = this[0];
                return;
            }

            if (index == this.Count - 1)
            {
                // 最后一个元素
                this.SelectedItem = this[0];
                return;
            }

            this.SelectedItem = this[index + 1];
        }

        /// <summary>
        /// 选中上一个项目
        /// </summary>
        public void SelectPrevious()
        {
            if (this.SelectedItem == null) 
            {
                this.SelectedItem = this[this.Count - 1];
                return;
            }

            int index = this.IndexOf(this.SelectedItem);
            if (index < 0) 
            {
                this.SelectedItem = this[this.Count - 1];
                return;
            }

            if (index == 0) 
            {
                this.SelectedItem = this[this.Count - 1];
                return;
            }

            this.SelectedItem = this[index - 1];
        }

        #endregion
    }
}







