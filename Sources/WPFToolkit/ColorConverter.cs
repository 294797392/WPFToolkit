using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFToolkit
{
    /// <summary>
    /// 颜色转换工具
    /// 
    /// https://www.jianshu.com/p/1dde91956d9d?utm_campaign=maleskine&utm_content=note&utm_medium=seo_notes&utm_source=recommendation
    /// HSB颜色模式可以调节颜色的亮度和饱和度
    /// HSB 的 B（明度）控制纯色中混入黑色的量，越往上，值越大，黑色越少，颜色明度越高。
    /// HSB 的 S（饱和度）控制纯色中混入白色的量，越往右，值越大，白色越少，颜色纯度越高。
    /// </summary>
    public static class ColorConverter
    {
        /// <summary>
        /// RGB颜色模式转HSV颜色模式
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        public static void RGB2HSB(byte r, byte g, byte b, out double h, out double s, out double v)
        {
            //System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
            //h = color.GetHue();
            //s = color.GetSaturation();
            //ob = color.GetBrightness();

            //System.Drawing.IDeviceContext

            System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);

            int max = Math.Max(r, Math.Max(g, b));
            int min = Math.Min(r, Math.Min(g, b));

            h = color.GetHue();
            s = (max == 0) ? 0 : 1d - (1d * min / max);
            v = max / 255d;
        }

        /// <summary>
        /// RGB颜色模式转HSV颜色模式
        /// </summary>
        public static void RGB2HSB(Color color, out double h, out double s, out double b)
        {
            RGB2HSB(color.R, color.G, color.B, out h, out s, out b);
        }

        /// <summary>
        /// HSV颜色模式转RGB颜色模式
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v1"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public static void HSB2RGB(double h, double s, double v1, out byte r, out byte g, out byte b)
        {
            r = 0;
            g = 0;
            b = 0;

            int hi = Convert.ToInt32(Math.Floor(h / 60)) % 6;
            double f = h / 60 - Math.Floor(h / 60);

            v1 = v1 * 255;
            byte v = Convert.ToByte(v1);
            byte p = Convert.ToByte(v * (1 - s));
            byte q = Convert.ToByte(v * (1 - f * s));
            byte t = Convert.ToByte(v * (1 - (1 - f) * s));

            if (hi == 0)
            {
                r = v;
                g = t;
                b = p;
            }
            else if (hi == 1)
            {
                r = q;
                g = v;
                b = p;
            }
            else if (hi == 2)
            {
                r = p;
                g = v;
                b = t;
            }
            else if (hi == 3)
            {
                r = p;
                g = q;
                b = v;
            }
            else if (hi == 4)
            {
                r = t;
                g = p;
                b = v;
            }
            else
            {
                r = v;
                g = p;
                b = q;
            }
        }

        /// <summary>
        /// HSV颜色模式转RGB颜色模式
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="brightness"></param>
        /// <param name="c"></param>
        public static void HSB2RGB(double hue, double saturation, double brightness, out Color c)
        {
            byte r, g, b;
            HSB2RGB(hue, saturation, brightness, out r, out g, out b);
            c = Color.FromRgb(r, g, b);
        }
    }
}