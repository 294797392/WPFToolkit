using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.MVVM;

namespace WPFClient.ViewModels
{
    /// <summary>
    /// EnumItemsSourceVM类
    /// </summary>
    public class EnumItemsSourceVM : ViewModelBase
    {
        public int SelectedValue { get; set; }
    }
}