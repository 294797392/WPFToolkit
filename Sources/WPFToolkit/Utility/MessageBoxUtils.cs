using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.Utility
{
    public static class MessageBoxUtils
    {
        public static void Info(string message, params object[] param)
        {
            string msg = string.Format(message, param);
            MessageBox.Show(msg, "信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Warn(string message, params object[] param)
        {
            string msg = string.Format(message, param);
            MessageBox.Show(msg, "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void Error(string message, params object[] param)
        {
            string msg = string.Format(message, param);
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static bool Confirm(string message, params object[] param)
        {
            string msg = string.Format(message, param);
            return MessageBox.Show(msg, "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK;
        }
    }
}
