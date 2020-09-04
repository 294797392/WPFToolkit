using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 通用树形列表ViewModel
    /// 项目在开发的时候可以继承这个ViewModel，也可以直接使用该ViewModel
    /// </summary>
    public class TreeViewModel : ListViewModel
    {
        #region 实例变量

        private Dictionary<object, TreeNodeViewModel> nodeMap;

        #endregion

        #region 属性

        /// <summary>
        /// 树形列表的根节点
        /// </summary>
        public ObservableCollection<TreeNodeViewModel> Roots { get; private set; }

        /// <summary>
        /// 存储树形列表上下文信息
        /// </summary>
        public TreeViewModelContext Context { get; private set; }

        #endregion

        #region 构造方法

        public TreeViewModel()
        {
            this.nodeMap = new Dictionary<object, TreeNodeViewModel>();
            this.Roots = new ObservableCollection<TreeNodeViewModel>();
            this.Context = new TreeViewModelContext();
        }

        #endregion

        #region 公开接口

        /// <summary>
        /// 展开某个节点
        /// </summary>
        public void ExpandNode(string nodeID)
        {
            TreeNodeViewModel vm;
            if (!this.nodeMap.TryGetValue(nodeID, out vm))
            {
                // 不存在该节点
                return;
            }

            // 设置IsExpanded会对父节点进行递归展开
            vm.IsExpanded = true;
        }

        #endregion
    }
}