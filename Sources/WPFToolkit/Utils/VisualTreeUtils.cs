using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WPFToolkit.Utils
{
    /// <summary>
    /// 提供对VisualTree的扩展函数
    /// </summary>
    public static class VisualTreeUtils
    {
        public static T FindVisualChild<T>(this DependencyObject parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = FindVisualChild<T>(v);
                }

                if (child != null)
                {
                    break;
                }
            }

            return child;
        }

        /// <summary>
        /// 找到一个节点下的指定类型的所有子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static List<T> FindAllVisualChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            List<T> visualList = new List<T>();

            int visuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < visuals; i++)
            {
                DependencyObject visual = VisualTreeHelper.GetChild(parent, i);
                if (visual is T)
                {
                    visualList.Add(visual as T);
                }

                FindAllChildVisualChild<T>(visual, visualList);
            }

            return visualList.Cast<T>().ToList();
        }

        /// <summary>
        /// 找到一个节点下的指定类型的所有子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parent"></param>
        /// <returns></returns>
        private static void FindAllChildVisualChild<T>(this DependencyObject parent, List<T> visualList) where T : DependencyObject
        {
            int visuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < visuals; i++)
            {
                DependencyObject visual = VisualTreeHelper.GetChild(parent, i);
                if (visual is T)
                {
                    visualList.Add(visual as T);
                }

                FindAllChildVisualChild<T>(visual, visualList);
            }
        }


        /// <summary>
        /// 把一个控件里的所有子控件的值拷贝到另外一个相同的控件里
        /// </summary>
        /// <param name="srcElement"></param>
        /// <param name="targetElement"></param>
        /// <param name="copyWhat"></param>
        public static void CopyFrom(FrameworkElement srcElement, FrameworkElement targetElement, Dictionary<Type, DependencyProperty> copyWhat)
        {
            CopyFromVisualTree(srcElement, targetElement, copyWhat);
        }

        private static void CopyFromVisualTree(DependencyObject srcRoot, FrameworkElement targetRoot, Dictionary<Type, DependencyProperty> copyWhat)
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(srcRoot); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(srcRoot, i);

                Type childType = child.GetType();

                DependencyProperty targetProperty;
                if (copyWhat.TryGetValue(childType, out targetProperty))
                {
                    FrameworkElement srcElement = child as FrameworkElement;

                    FrameworkElement targetElement = targetRoot.FindName(srcElement.Name) as FrameworkElement;
                    if (targetElement != null)
                    {
                        object value = srcElement.GetValue(targetProperty);
                        targetElement.SetValue(targetProperty, value);
                    }
                }

                CopyFromVisualTree(child, targetRoot, copyWhat);
            }
        }
    }
}