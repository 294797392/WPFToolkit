using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFToolkit.DragDrop
{
    public interface IDropTarget
    {
        void DragOver(DropInfo dropInfo);
        void Drop(DropInfo dropInfo);
    }
}
