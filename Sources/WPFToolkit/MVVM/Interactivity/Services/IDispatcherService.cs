using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WPFToolkit.MVVM.Interactivity.Services
{
    /// <summary>
    /// 提供可以访问UI线程的能力
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        /// UI工作线程调度队列
        /// </summary>
        Dispatcher Dispatcher { get; }
    }
}