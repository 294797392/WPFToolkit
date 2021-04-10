using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM.Internals
{
    internal class TreeViewJSON
    {
        /// <summary>
        /// 节点列表
        /// </summary>
        [JsonProperty("nodes")]
        public List<TreeNodeJSON> NodeList { get; set; }

        public TreeViewJSON()
        {
            this.NodeList = new List<TreeNodeJSON>();
        }
    }
}
