using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    public interface ITreeViewModelContext
    {
        IEnumerable<T> GetSelectedItems<T>() where T : TreeNodeViewModel;

        T GetSelectedItem<T>() where T : TreeNodeViewModel;
    }
}
