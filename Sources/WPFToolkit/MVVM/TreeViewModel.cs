using DotNEToolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.Attributes;
using WPFToolkit.MVVM.Internals;

namespace WPFToolkit.MVVM
{
    /// <summary>
    /// 通用树形列表ViewModel
    /// 项目在开发的时候可以继承这个ViewModel，也可以直接使用该ViewModel
    /// </summary>
    public class TreeViewModel : ItemsViewModel<TreeNodeViewModel>
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

        #region 静态方法

        private static void LoadChildNodes(TreeNodeViewModel parentNode, List<TreeNodeJSON> childNodes)
        {
            foreach (TreeNodeJSON nodeJson in childNodes)
            {
                TreeNodeViewModel vm = new TreeNodeViewModel(parentNode.Context, parentNode)
                {
                    ID = nodeJson.ID,
                    Name = nodeJson.Name,
                    IconURI = nodeJson.Icon
                };

                parentNode.Children.Add(vm);

                LoadChildNodes(vm, nodeJson.Children);
            }
        }

        /// <summary>
        /// 从数据源创建TreeViewModel
        /// </summary>
        /// <param name="sourceType"></param>
        /// <param name="sourceURI"></param>
        /// <returns></returns>
        public static TreeViewModel Create(ItemsSourceType sourceType, string sourceURI)
        {
            TreeViewJSON tv = null;

            switch (sourceType)
            {
                case ItemsSourceType.JsonFile:
                    {
                        if (!File.Exists(sourceURI))
                        {
                            return null;
                        }

                        tv = JSONHelper.ParseFile<TreeViewJSON>(sourceURI);

                        break;
                    }

                default:
                    throw new NotImplementedException();
            }

            TreeViewModelContext context = new TreeViewModelContext();

            TreeViewModel treeVM = new TreeViewModel();

            foreach (TreeNodeJSON nodeJson in tv.NodeList)
            {
                TreeNodeViewModel vm = new TreeNodeViewModel(context)
                {
                    ID = nodeJson.ID,
                    Name = nodeJson.Name,
                    IconURI = nodeJson.Icon,
                };

                treeVM.Items.Add(vm);

                LoadChildNodes(vm, nodeJson.Children);
            }

            return treeVM;
        }

        #endregion
    }
}