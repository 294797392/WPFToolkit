using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM.Internals
{
    /// <summary>
    /// 存储数据模型
    /// </summary>
    internal class InternalTreeView
    {
        /// <summary>
        /// 节点列表
        /// </summary>
        [JsonProperty("nodes")]
        public List<InternalTreeNode> NodeList { get; set; }

        public InternalTreeView()
        {
            this.NodeList = new List<InternalTreeNode>();
        }
    }
}
