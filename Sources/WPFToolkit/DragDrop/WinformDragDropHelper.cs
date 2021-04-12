using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace WPFToolkit.DragDrop
{
    public class WinformDragDropHelper
    {
        public static void RegisterDropHandler(System.Windows.Forms.Control control)
        {
            if (control is IDropHandler)
            {
                control.AllowDrop = true;
                control.DragDrop += FormControl_DragDrop;
                control.DragEnter += FormControl_DragEnter;
                control.DragLeave += FormControl_DragLeave;
                control.DragOver += FormControl_DragOver;
            }
        }

        public static void UnRegisterDropHandler(System.Windows.Forms.Control control)
        {
            control.AllowDrop = false;
            control.DragDrop -= FormControl_DragDrop;
            control.DragEnter -= FormControl_DragEnter;
            control.DragLeave -= FormControl_DragLeave;
            control.DragOver -= FormControl_DragOver;
        }

        static void FormControl_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            IDropHandler dropTarget = sender as IDropHandler;
            if (dropTarget != null)
            {
                DropInfo dropInfo = new DropInfo();
                if (DragDrop.m_DragInfo != null)
                {
                    dropInfo.Data = DragDrop.m_DragInfo.Data;
                }

                dropTarget.OnDragOver(dropInfo);

                if (dropInfo.Effects != DragDropEffects.None)
                {
                    e.Effect = Convert(dropInfo.Effects);
                }
            }
        }

        static void FormControl_DragLeave(object sender, EventArgs e)
        {

        }

        static void FormControl_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            FormControl_DragOver(sender, e);
        }

        static void FormControl_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            IDropHandler dropTarget = sender as IDropHandler;
            if (dropTarget != null)
            {
                DropInfo dropInfo = new DropInfo();
                if (DragDrop.m_DragInfo != null)
                {
                    dropInfo.Data = DragDrop.m_DragInfo.Data;
                }

                dropTarget.OnDrop(dropInfo);
            }
        }

        static System.Windows.Forms.DragDropEffects Convert(DragDropEffects effect)
        {
            switch (effect)
            {
                case DragDropEffects.All:
                    return System.Windows.Forms.DragDropEffects.All;

                case DragDropEffects.Copy:
                    return System.Windows.Forms.DragDropEffects.Copy;

                case DragDropEffects.Link:
                    return System.Windows.Forms.DragDropEffects.Link;

                case DragDropEffects.Move:
                    return System.Windows.Forms.DragDropEffects.Move;

                case DragDropEffects.Scroll:
                    return System.Windows.Forms.DragDropEffects.Scroll;

                case DragDropEffects.None:
                default:
                    return System.Windows.Forms.DragDropEffects.None;
            }
        }

    }
}
