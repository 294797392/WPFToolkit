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
    public class ItemsViewModel<T> : BindableCollection<T> where T : ItemViewModel
    {
        #region 实例变量

        #endregion

        #region 属性

        #endregion

        #region 构造方法

        public ItemsViewModel()
        {
        }

        #endregion
    }
}