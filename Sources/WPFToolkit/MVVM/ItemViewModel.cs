using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 列表项通用ViewModel
    /// </summary>
    public class ItemViewModel : ViewModelBase, ISelectableViewModel, IExpandableViewModel
    {
        #region 实例变量

        protected string iconURI;
        protected bool isSelected;
        protected bool isExpanded;
        protected bool isChecked;
        protected bool isVisible = true;
        private int level;

        #endregion

        #region 属性

        /// <summary>
        /// 当前项要显示的图标URI
        /// </summary>
        public virtual string IconURI
        {
            get { return this.iconURI; }
            set
            {
                this.iconURI = value;
                this.NotifyPropertyChanged("IconURI");
            }
        }

        /// <summary>
        /// 当前项是否被选中
        /// </summary>
        public virtual bool IsSelected
        {
            get { return this.isSelected; }
            set
            {
                if (this.isSelected != value)
                {
                    this.isSelected = value;
                    this.NotifyPropertyChanged("IsSelected");
                }
            }
        }

        /// <summary>
        /// 当前节点是否被展开
        /// </summary>
        public virtual bool IsExpanded
        {
            get { return this.isExpanded; }
            set
            {
                if (this.isExpanded != value)
                {
                    this.isExpanded = value;
                    this.NotifyPropertyChanged("IsExpanded");
                }
            }
        }

        /// <summary>
        /// 节点是否被勾选（当节点带有CheckBox的时候）
        /// </summary>
        public virtual bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                if (this.isChecked != value)
                {
                    this.isChecked = value;
                    this.NotifyPropertyChanged("IsChecked");
                }
            }
        }

        /// <summary>
        /// 是否显示该节点
        /// </summary>
        public virtual bool IsVisible
        {
            get { return this.isVisible; }
            set
            {
                if (this.isVisible != value)
                {
                    this.isVisible = value;
                    this.NotifyPropertyChanged("IsVisible");
                }
            }
        }

        /// <summary>
        /// 该节点的等级
        /// 可以用来计算每个TreeItem的Margin
        /// </summary>
        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (level != value)
                {
                    level = value;
                    NotifyPropertyChanged("Level");
                }
            }
        }

        #endregion
    }
}