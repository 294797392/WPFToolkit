using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM.Internals
{
    internal class InternalTreeNode
    {
        /// <summary>
        /// 节点ID
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 节点名字
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// TreeItem图标
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 子节点列表
        /// </summary>
        [JsonProperty("child")]
        public List<InternalTreeNode> Children { get; set; }

        /// <summary>
        /// 用户自定义数据
        /// </summary>
        [JsonProperty("data")]
        public object Data { get; set; }

        public InternalTreeNode()
        {
            this.Children = new List<InternalTreeNode>();
        }
    }
}
