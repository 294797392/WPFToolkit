using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace WPFToolkit.DragDrop
{
    public class DropInfo
    {
        public DropInfo()
        {
        }

        public DropInfo(object sender, DragEventArgs e, DragInfo dragInfo, string dataFormat)
        {
            Data = (e.Data.GetDataPresent(dataFormat)) ? e.Data.GetData(dataFormat) : e.Data;
            this.DragEventArgs = e;
            DragInfo = dragInfo;

            VisualTarget = sender as UIElement;

            if (sender is ItemsControl)
            {
                ItemsControl itemsControl = (ItemsControl)sender;
                UIElement item = itemsControl.GetItemContainerAt(e.GetPosition(itemsControl));

                VisualTargetOrientation = itemsControl.GetItemsPanelOrientation();

                if (item != null)
                {
                    ItemsControl itemParent = ItemsControl.ItemsControlFromItemContainer(item);

                    InsertIndex = itemParent.ItemContainerGenerator.IndexFromContainer(item);
                    TargetCollection = itemParent.ItemsSource ?? itemParent.Items;
                    TargetItem = itemParent.ItemContainerGenerator.ItemFromContainer(item);
                    VisualTargetItem = item;

                    if (VisualTargetOrientation == Orientation.Vertical)
                    {
                        if (e.GetPosition(item).Y > item.RenderSize.Height / 2) InsertIndex++;
                    }
                    else
                    {
                        if (e.GetPosition(item).X > item.RenderSize.Width / 2) InsertIndex++;
                    }
                }
                else
                {
                    TargetCollection = itemsControl.ItemsSource ?? itemsControl.Items;
                    InsertIndex = itemsControl.Items.Count;
                }
            }
        }


        public object Data { get; internal set; }
        public DragInfo DragInfo { get; internal set; }
        public Type DropTargetAdorner { get; set; }
        public DragDropEffects Effects { get; set; }
        public int InsertIndex { get; private set; }
        public IEnumerable TargetCollection { get; internal set; }
        public object TargetItem { get; internal set; }
        public UIElement VisualTarget { get; internal set; }
        public UIElement VisualTargetItem { get; internal set; }
        public Orientation VisualTargetOrientation { get; internal set; }
        public Point DropPosition { get; set; }

        public DragEventArgs DragEventArgs { get; internal set; }
    }
}
