using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// ViewModelBase类
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 冗余Name属性
        /// </summary>
        private string name;

        /// <summary>
        /// 冗余属性，字符串格式的ID
        /// </summary>
        private object id;

        /// <summary>
        /// 冗余Description属性
        /// </summary>
        private string description;

        /// <summary>
        /// 名字
        /// </summary>
        public virtual string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.NotifyPropertyChanged("Name");
            }
        }

        /// <summary>
        /// 描述信息
        /// </summary>
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            set
            {
                this.description = value;
                this.NotifyPropertyChanged("Description");
            }
        }

        /// <summary>
        /// 唯一标识符
        /// </summary>
        public virtual object ID
        {
            get { return this.id; }
            set
            {
                this.id = value;
                this.NotifyPropertyChanged("ID");
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}