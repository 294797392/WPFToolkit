using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFToolkit.MarkupExtensions;

namespace WPFToolkit.Attributes
{
    public class EnumMemberAttribute : Attribute
    {
        public string Name { get; set; }

        public EnumMemberAttribute(string name)
        {
            this.Name = name;
        }

        public EnumMemberVM ToEnumMember(int value)
        {
            return new EnumMemberVM()
            {
                Name = this.Name,
                Value = value
            };
        }
    }
}