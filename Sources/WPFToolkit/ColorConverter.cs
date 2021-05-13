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
        public static void RGB2HSB(byte r, byte g, byte b, out float h, out float s, out float ob)
        {
            System.Drawing.ColorConverter
            System.Drawing.Color color = System.Drawing.Color.FromArgb(r, g, b);
            h = color.GetHue();
            s = color.GetSaturation();
            ob = color.GetBrightness();
        }

        public static void RGB2HSB(Color color, out float h, out float s, out float b)
        {
            RGB2HSB(color.R, color.G, color.B, out h, out s, out b);
        }

        public static void HSB2RGB(double hue, double saturation, double brightness, out byte r, out byte g, out byte b)
        {
            r = 0;
            g = 0;
            b = 0;

            if (saturation == 0)
            {
                r = g = b = (byte)(brightness * 255.0f + 0.5f);
            }
            else
            {
                double h = (hue - (double)Math.Floor(hue)) * 6.0f;
                double f = h - (double)Math.Floor(h);
                double p = brightness * (1.0f - saturation);
                double q = brightness * (1.0f - saturation * f);
                double t = brightness * (1.0f - (saturation * (1.0f - f)));
                switch ((int)h)
                {
                    case 0:
                        r = (byte)(brightness * 255.0f + 0.5f);
                        g = (byte)(t * 255.0f + 0.5f);
                        b = (byte)(p * 255.0f + 0.5f);
                        break;
                    case 1:
                        r = (byte)(q * 255.0f + 0.5f);
                        g = (byte)(brightness * 255.0f + 0.5f);
                        b = (byte)(p * 255.0f + 0.5f);
                        break;
                    case 2:
                        r = (byte)(p * 255.0f + 0.5f);
                        g = (byte)(brightness * 255.0f + 0.5f);
                        b = (byte)(t * 255.0f + 0.5f);
                        break;
                    case 3:
                        r = (byte)(p * 255.0f + 0.5f);
                        g = (byte)(q * 255.0f + 0.5f);
                        b = (byte)(brightness * 255.0f + 0.5f);
                        break;
                    case 4:
                        r = (byte)(t * 255.0f + 0.5f);
                        g = (byte)(p * 255.0f + 0.5f);
                        b = (byte)(brightness * 255.0f + 0.5f);
                        break;
                    case 5:
                        r = (byte)(brightness * 255.0f + 0.5f);
                        g = (byte)(p * 255.0f + 0.5f);
                        b = (byte)(q * 255.0f + 0.5f);
                        break;
                }
            }
        }

        public static void HSB2RGB(double hue, double saturation, double brightness, out Color c)
        {
            byte r, g, b;
            HSB2RGB(hue, saturation, brightness, out r, out g, out b);
            c = Color.FromRgb(r, g, b);
        }
    }
}