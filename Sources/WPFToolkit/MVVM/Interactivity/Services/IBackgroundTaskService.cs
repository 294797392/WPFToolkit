using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFToolkit.MVVM.Interactivity.Services
{
    /// <summary>
    /// 为执行后台任务提供服务
    /// 该服务会在会把函数放到异步线程去运行，同时界面上会弹出一个ProrgessWindow以提示用户任务运行进度
    /// </summary>
    public interface IBackgroundTaskService
    {
        /// <summary>
        /// UI工作线程调度队列
        /// </summary>
        Dispatcher Dispatcher { get; }

    }
}