using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkit.MVVM.Internals
{
    internal class TreeNodeJSON
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("child")]
        public List<TreeNodeJSON> Children { get; set; }

        public TreeNodeJSON()
        {
            this.Children = new List<TreeNodeJSON>();
        }
    }
}
