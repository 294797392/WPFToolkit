using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFToolkit.Utility
{
    /// <summary>
    /// 提供对ImageSource的帮助函数
    /// 
    /// 位图和调色板：https://blog.csdn.net/kingbyang1/article/details/5564704
    /// </summary>
    public static class ImageUtility
    {
        /// <summary>
        /// 使用imageURI创建一个ImageSource
        /// </summary>
        /// <param name="imageURI"></param>
        /// <returns></returns>
        public static ImageSource FromURI(string imageURI)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imageURI);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            return bitmap;
        }

        /// <summary>
        /// 使用Winform的Bitmap创建一个ImageSource
        /// 注意，该函数不会释放Bitmap
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ImageSource FromBitmap(Bitmap source)
        {
            IntPtr imagePtr = source.GetHbitmap();//从GDI+ Bitmap创建GDI位图对象
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(imagePtr, IntPtr.Zero, Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
