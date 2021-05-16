using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFToolkitDemo
{
    public class ItemData
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Data : SingletonObject<Data>
    {
        [JsonProperty("itemsData")]
        public List<ItemData> ItemsData { get; set; }

        public Data()
        {
            this.ItemsData = new List<ItemData>();
        }
    }

    public static class DataSource
    {
        public static Data Data { get; set; }

        static DataSource()
        {
            Data = JSONHelper.ParseFile<Data>("data.json");
        }
    }
}
