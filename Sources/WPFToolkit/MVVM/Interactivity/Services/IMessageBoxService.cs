using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFToolkit.MVVM.Interactivity.Services
{
    /// <summary>
    /// 提供弹窗服务
    /// </summary>
    public interface IMessageBoxService
    {
    }

    /// <summary>
    /// Windows经典弹窗
    /// </summary>
    public class ClassicMessageBoxService : Service, IMessageBoxService
    {
        internal override void Initialize()
        {
            throw new NotImplementedException();
        }

        internal override void Release()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 模拟Win10弹窗
    /// </summary>
    public class Windows10MessageBoxService : Service, IMessageBoxService
    {
        internal override void Initialize()
        {
            throw new NotImplementedException();
        }

        internal override void Release()
        {
            throw new NotImplementedException();
        }
    }
}