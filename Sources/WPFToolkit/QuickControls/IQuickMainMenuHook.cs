using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.QuickControls
{
    /// <summary>
    /// 菜单钩子程序
    /// </summary>
    public interface IQuickMainMenuHook
    {
        /// <summary>
        /// 初始化该界面
        /// 只有当第一次实例化该控件的时候才会调用这个函数
        /// </summary>
        void Initialize();

        /// <summary>
        /// 当界面已经加载了并显示出来之后出发
        /// </summary>
        void OnLoaded();

        /// <summary>
        /// 当控件从界面上卸载了之后触发
        /// </summary>
        void OnUnload();
    }
}
