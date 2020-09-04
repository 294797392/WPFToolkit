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
    public class ListItemViewModel : ViewModelBase, ISelectableViewModel, IExpandableViewModel
    {
        #region 实例变量

        protected string iconURI;

        protected bool isSelected;

        protected bool isExpanded;

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

        #endregion
    }
}