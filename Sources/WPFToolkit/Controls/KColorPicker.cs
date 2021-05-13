using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFToolkit.Controls
{
    /// <summary>
    /// 颜色选择器
    /// 
    /// 色相是分段线性变化的，因此，我们可以利用LinearGradientBrush来绘制颜色条
    /// 
    /// H(Hue) 为色相, 取值范围：0-360°，表示基础颜色。
    /// S(Saturation) 为饱和度， 取值范围：0 - 1(0% - 100%), 表示色彩的纯度。
    /// B(Brightness)为明度, 取值范围：0 - 1(0% - 100%)，表示对光量的感知。
    /// 
    /// 
    /// HSB 的 B（明度）控制纯色中混入黑色的量，越往上，值越大，黑色越少，颜色明度越高。
    /// HSB 的 S（饱和度）控制纯色中混入白色的量，越往右，值越大，白色越少，颜色纯度越高。
    /// 
    /// https://www.cnblogs.com/nabian/p/9267646.html
    /// </summary>
    [TemplatePart(Name = "PART_ColorBar", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_ColorGrid", Type = typeof(Grid))]
    public class KColorPicker : Control
    {
        #region 实例变量

        private Canvas colorBar;
        private Grid colorGrid;

        private LinearGradientBrush colorBarBrush;
        private GradientStop gradientStop;

        #endregion

        #region 依赖属性

        //public Color SelectedColor
        //{
        //    get { return (Color)GetValue(SelectedColorProperty); }
        //    set { SetValue(SelectedColorProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty SelectedColorProperty =
        //    DependencyProperty.Register("SelectedColor", typeof(Color), typeof(KColorPicker), new PropertyMetadata(null));






        public string InternalSelectedColor
        {
            get { return (string)GetValue(InternalSelectedColorProperty); }
            set { SetValue(InternalSelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for InternalSelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty InternalSelectedColorProperty =
            DependencyProperty.Register("InternalSelectedColor", typeof(string), typeof(KColorPicker), new PropertyMetadata(string.Empty));



        public Color SelectedColor
        {
            get { return (Color)GetValue(SelectedColorProperty); }
            set { SetValue(SelectedColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedColor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register("SelectedColor", typeof(Color), typeof(KColorPicker), new PropertyMetadata(null));


        #endregion

        #region 构造方法

        public KColorPicker()
        {
            this.Style = Templates.KColorPickerStyle;
        }

        #endregion

        #region 实例方法

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.colorBar = this.Template.FindName("PART_ColorBar", this) as Canvas;
            this.colorBar.MouseMove += ColorBar_MouseMove;
            this.colorBarBrush = this.colorBar.Background as LinearGradientBrush;
            this.gradientStop = this.Template.FindName("GradientStop", this) as GradientStop;

            this.colorGrid = this.Template.FindName("PART_ColorGrid", this) as Grid;
            this.colorGrid.MouseMove += ColorGrid_MouseMove;
        }

        private void ColorGrid_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point relativePos = e.GetPosition(this.colorGrid);

            float h, s, b;
            ColorConverter.RGB2HSB(this.gradientStop.Color, out h, out s, out b);

            // 观察Photoshop的HSB值，从上到下，b是100 - 0。b决定了颜色的亮度。白色最量，黑色最暗
            // 从左到右，s是0 - 100，s决定了颜色的饱和度。饱和度越高色彩越鲜艳
            // h值不变，说明决定颜色的值是h

            b = 1 - (float)(relativePos.Y / this.colorGrid.ActualHeight);
            s = (float)(relativePos.X / this.colorGrid.ActualWidth);

            Color color;
            ColorConverter.HSB2RGB(h, s, b, out color);

            this.InternalSelectedColor = color.ToString();
            this.SelectedColor = color;

            this.Background = new SolidColorBrush(this.SelectedColor);
        }

        private void ColorBar_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Point relativePos = e.GetPosition(this.colorBar);

            double percent = relativePos.Y / this.colorBar.ActualHeight;

            this.gradientStop.Color = this.colorBarBrush.GradientStops.GetRelativeColor(percent);

            this.InternalSelectedColor = this.gradientStop.Color.ToString();
        }

        #endregion
    }

    /// <summary>
    /// https://stackoverflow.com/questions/9650049/get-color-in-specific-location-on-gradient
    /// </summary>
    public static class GradientStopCollectionExtensions
    {
        public static Color GetRelativeColor(this GradientStopCollection gsc, double offset)
        {
            var point = gsc.SingleOrDefault(f => f.Offset == offset);
            if (point != null) return point.Color;

            GradientStop before = gsc.Where(w => w.Offset == gsc.Min(m => m.Offset)).First();
            GradientStop after = gsc.Where(w => w.Offset == gsc.Max(m => m.Offset)).First();

            foreach (var gs in gsc)
            {
                if (gs.Offset < offset && gs.Offset > before.Offset)
                {
                    before = gs;
                }
                if (gs.Offset > offset && gs.Offset < after.Offset)
                {
                    after = gs;
                }
            }

            var color = new Color();

            color.ScA = (float)((offset - before.Offset) * (after.Color.ScA - before.Color.ScA) / (after.Offset - before.Offset) + before.Color.ScA);
            color.ScR = (float)((offset - before.Offset) * (after.Color.ScR - before.Color.ScR) / (after.Offset - before.Offset) + before.Color.ScR);
            color.ScG = (float)((offset - before.Offset) * (after.Color.ScG - before.Color.ScG) / (after.Offset - before.Offset) + before.Color.ScG);
            color.ScB = (float)((offset - before.Offset) * (after.Color.ScB - before.Color.ScB) / (after.Offset - before.Offset) + before.Color.ScB);

            return color;
        }
    }
}
