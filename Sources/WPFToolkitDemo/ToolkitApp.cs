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
    public class ToolkitManifest : AppManifest
    {
        /// <summary>
        /// 菜单列表
        /// </summary>
        [JsonProperty("menus")]
        public List<MenuDefinition> MenuList { get; private set; }

        public ToolkitManifest()
        {
            this.MenuList = new List<MenuDefinition>();
        }
    }

    public class ToolkitApp : ModularApp<ToolkitApp, ToolkitManifest>
    {
        protected override int OnInitialize()
        {
            return 0;
        }

        protected override void OnRelease()
        {
        }
    }
}
