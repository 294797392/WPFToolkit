using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace WPFToolkit.Controls
{
    public class PanAndZoomViewer : ContentControl
    {
        public double MaxZoomLevel = 8;
        public double MinZoomLevel = 0.25;
        public double CurrentZoomLevel = 1;

        public double DefaultZoomFactor { get; set; }
        private FrameworkElement source;
        private Point ScreenStartPoint = new Point(0, 0);
        private TranslateTransform translateTransform = new TranslateTransform();
        private ScaleTransform zoomTransform = new ScaleTransform();
        private TransformGroup transformGroup = new TransformGroup();
        private Point startOffset;

        public PanAndZoomViewer()
        {
            this.DefaultZoomFactor = 2.5;
        }

        public double ZoomFactor
        {
            get { return this.zoomTransform == null ? 1 : this.zoomTransform.ScaleX; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Setup(this);
        }

        void Setup(FrameworkElement control)
        {
            this.source = VisualTreeHelper.GetChild(this, 0) as FrameworkElement;
            //this.translateTransform = new TranslateTransform();
            //this.zoomTransform = new ScaleTransform();
            //this.transformGroup = new TransformGroup();
            this.transformGroup.Children.Add(this.zoomTransform);
            this.transformGroup.Children.Add(this.translateTransform);
            this.source.RenderTransform = this.transformGroup;
            this.Focusable = true;
            this.KeyDown += new KeyEventHandler(source_KeyDown);
            this.MouseMove += new MouseEventHandler(control_MouseMove);
            this.MouseDown += new MouseButtonEventHandler(source_MouseDown);
            this.MouseUp += new MouseButtonEventHandler(source_MouseUp);
            this.MouseWheel += new MouseWheelEventHandler(source_MouseWheel);
        }

        void source_KeyDown(object sender, KeyEventArgs e)
        {
            // hit escape to reset everything
            if (e.Key == Key.Escape) Reset();
        }

        void source_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // zoom into the content.  Calculate the zoom factor based on the direction of the mouse wheel.
            double zoomFactor = this.DefaultZoomFactor;
            if (e.Delta <= 0)
            {
                zoomFactor = 1.0 / this.DefaultZoomFactor;
            }

            // DoZoom requires both the logical and physical location of the mouse pointer
            var physicalPoint = e.GetPosition(this);

            //Point p = this.TranslatePoint(physicalPoint, this.source);
            Point p = this.transformGroup.Inverse.Transform(physicalPoint);

            //Console.WriteLine("鼠标相对于窗口的坐标 = {0}, 鼠标相对于图片的坐标(逆矩阵转换) = {1}", physicalPoint, p);

            DoZoom(zoomFactor, p, physicalPoint);

            //Point mouseP = e.GetPosition(this);
            //if (e.Delta < 0)
            //{
            //    if (this.CurrentZoomLevel - 0.25 > MinZoomLevel)
            //    {
            //        this.CurrentZoomLevel -= 0.25;
            //        this.DoZoomPercent(this.CurrentZoomLevel, mouseP, mouseP);
            //    }
            //}
            //else
            //{
            //    if (this.CurrentZoomLevel <= 8)
            //    {
            //        this.CurrentZoomLevel += 0.25;
            //        this.DoZoomPercent(this.CurrentZoomLevel, mouseP, mouseP);
            //    }
            //}
        }

        void source_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                // we're done.  reset the cursor and release the mouse pointer
                this.Cursor = Cursors.Arrow;
                this.ReleaseMouseCapture();
            }
        }

        void source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Save starting point, used later when determining how much to scroll.

            if (e.ClickCount == 2)
            {
                return;
            }

            this.ScreenStartPoint = e.GetPosition(this);
            this.startOffset = new Point(this.translateTransform.X, this.translateTransform.Y);
            this.CaptureMouse();
            this.Cursor = Cursors.ScrollAll;
        }


        void control_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.IsMouseCaptured)
            {
                // if the mouse is captured then move the content by changing the translate transform.  
                // use the Pan Animation to animate to the new location based on the delta between the 
                // starting point of the mouse and the current point.
                var physicalPoint = e.GetPosition(this);
                this.translateTransform.BeginAnimation(TranslateTransform.XProperty, CreatePanAnimation(physicalPoint.X - this.ScreenStartPoint.X + this.startOffset.X), HandoffBehavior.Compose);
                this.translateTransform.BeginAnimation(TranslateTransform.YProperty, CreatePanAnimation(physicalPoint.Y - this.ScreenStartPoint.Y + this.startOffset.Y), HandoffBehavior.Compose);
            }
        }


        /// <summary>Helper to create the panning animation for x,y coordinates.</summary>
        /// <param name="toValue">New value of the coordinate.</param>
        /// <returns>Double animation</returns>
        private DoubleAnimation CreatePanAnimation(double toValue)
        {
            var da = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(300)));
            da.AccelerationRatio = 0.1;
            da.DecelerationRatio = 0.9;
            da.FillBehavior = FillBehavior.HoldEnd;
            da.Freeze();
            return da;
        }


        /// <summary>Helper to create the zoom double animation for scaling.</summary>
        /// <param name="toValue">Value to animate to.</param>
        /// <returns>Double animation.</returns>
        private DoubleAnimation CreateZoomAnimation(double toValue)
        {
            var da = new DoubleAnimation(toValue, new Duration(TimeSpan.FromMilliseconds(500)));
            da.AccelerationRatio = 0.1;
            da.DecelerationRatio = 0.9;
            da.FillBehavior = FillBehavior.HoldEnd;
            da.Freeze();
            return da;
        }

        /// <summary>Zoom into or out of the content.</summary>
        /// <param name="deltaZoom">Factor to mutliply the zoom level by. </param>
        /// <param name="mousePosition">Logical mouse position relative to the original content.</param>
        /// <param name="physicalPosition">Actual mouse position on the screen (relative to the parent window)</param>
        public void DoZoom(double deltaZoom, Point mousePosition, Point physicalPosition)
        {
            double currentZoom = this.zoomTransform.ScaleX;
            currentZoom *= deltaZoom;
            this.translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateZoomAnimation(-1 * (mousePosition.X * currentZoom - physicalPosition.X)));
            this.translateTransform.BeginAnimation(TranslateTransform.YProperty, CreateZoomAnimation(-1 * (mousePosition.Y * currentZoom - physicalPosition.Y)));
            this.zoomTransform.BeginAnimation(ScaleTransform.ScaleXProperty, CreateZoomAnimation(currentZoom));
            this.zoomTransform.BeginAnimation(ScaleTransform.ScaleYProperty, CreateZoomAnimation(currentZoom));
        }

        /// <summary>Reset to default zoom level and centered content.</summary>
        public void Reset()
        {
            this.translateTransform.BeginAnimation(TranslateTransform.XProperty, CreateZoomAnimation(0));
            this.translateTransform.BeginAnimation(TranslateTransform.YProperty, CreateZoomAnimation(0));
            this.zoomTransform.BeginAnimation(ScaleTransform.ScaleXProperty, CreateZoomAnimation(1));
            this.zoomTransform.BeginAnimation(ScaleTransform.ScaleYProperty, CreateZoomAnimation(1));

            this.CurrentZoomLevel = 1;
        }
    }
}
