﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFToolkit.Panels
{
    public class CircularPanel : Panel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableSize">FrameworkElement所测量到的可供Panel使用的空间大小</param>
        /// <returns></returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size();

            foreach (UIElement element in base.InternalChildren)
            {
                element.Measure(availableSize);
                size.Width += element.DesiredSize.Width;
                size.Height += element.DesiredSize.Height;
            }

            Console.WriteLine("MeasureW = {0}, MeasureH = {1}", size.Width, size.Height);

            return size;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double offsetX = finalSize.Width / 2;
            double offsetY = finalSize.Height / 2;
            double diameter = Math.Min(finalSize.Width, finalSize.Height);
            double radius = diameter / 2;
            double anglePerStation = base.InternalChildren.Count == 0 ? 0 : 360 / base.InternalChildren.Count;
            int childIndex = 0;

            foreach (UIElement element in base.InternalChildren)
            {
                double x = radius * Math.Cos(anglePerStation * childIndex / 180 * Math.PI);
                double y = radius * Math.Sin(anglePerStation * childIndex / 180 * Math.PI);

                double halfWidth = element.DesiredSize.Width / 2;
                double halfHeight = element.DesiredSize.Height / 2;

                x -= halfWidth;
                y -= halfHeight;
                x += offsetX;
                y += offsetY;

                //Console.WriteLine("X = {0}, Y = {1}", x, y);

                Rect position = new Rect(x, y, element.DesiredSize.Width, element.DesiredSize.Height);
                element.Arrange(position);

                childIndex++;
            }

            Console.WriteLine("ArrangeW = {0}, ArrangeH = {1}", finalSize.Width, finalSize.Height);

            return finalSize;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            Console.WriteLine("RenderW = {0}, RenderH = {1}", this.RenderSize.Width, this.RenderSize.Height);
        }
    }
}
