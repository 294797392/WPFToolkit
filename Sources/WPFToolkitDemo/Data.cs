using DotNEToolkit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.MVVM;

namespace WPFToolkitDemo
{
    public class ItemData
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
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

        public static TreeViewModel<TreeViewModelContext> TreeVM { get; private set; }

        static DataSource()
        {
            Data = JSONHelper.ParseFile<Data>("data.json");

            TreeVM = new TreeViewModel<TreeViewModelContext>();
            for (int i = 1; i <= 10; i++)
            {
                TreeNodeViewModel root = new TreeNodeViewModel(TreeVM.Context)
                {
                    ID = string.Format("ID:{0}", i),
                    Name = string.Format("Name:{0}", i)
                };
                TreeVM.AddRootNode(root);

                for (int j = 1; j <= 10; j++)
                {
                    TreeNodeViewModel child = new TreeNodeViewModel(TreeVM.Context)
                    {
                        ID = string.Format("ID:{0}-{1}", i, j),
                        Name = string.Format("Name:{0}-{1}", i, j)
                    };

                    root.AddChildNode(child);
                }
            }
        }
    }
}
