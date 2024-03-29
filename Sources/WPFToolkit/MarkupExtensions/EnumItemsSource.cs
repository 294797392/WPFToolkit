using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;
using System.Windows.Markup;
using WPFToolkit.MVVM;
using WPFToolkit.Attributes;

namespace WPFToolkit.MarkupExtensions
{
    /// <summary>
    /// 把一个枚举类型转换成可以直接绑定到ItemsControl上的类型
    /// </summary>
    public class EnumItemsSource : MarkupExtension
    {
        /// <summary>
        /// 枚举的类型
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// 枚举类型的名字
        /// 优先使用EnumTypeName进行反射
        /// </summary>
        public string EnumTypeName { get; set; }

        /// <summary>
        /// 对值进行转换的转换器
        /// </summary>
        public IValueConverter EnumMemberConverter { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            Type enumType = this.GetEnumType();
            if (enumType == null)
            {
                return null;
            }

            // TODO：反射信息缓存

            ObservableCollection<EnumMemberVM> result = new ObservableCollection<EnumMemberVM>();

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                EnumMemberAttribute attribute = field.GetCustomAttribute<EnumMemberAttribute>(true);
                if (attribute == null)
                {
                    // 不存在特性
                    continue;
                }

                object value = Enum.Parse(enumType, field.Name);
                if (value == null)
                {
                    // 枚举类型和名字不匹配
                    continue;
                }

                EnumMemberVM member = attribute.ToEnumMember((int)value);
                if (this.EnumMemberConverter != null)
                {
                    // 如果指定了转换器，那么进行转换
                    object convertedVal = this.EnumMemberConverter.Convert(member.Name, null, null, CultureInfo.CurrentCulture);
                    if (convertedVal != null)
                    {
                        member.Name = convertedVal.ToString();
                    }
                }

                result.Add(member);
            }

            return result;
        }

        private Type GetEnumType()
        {
            if (!string.IsNullOrEmpty(this.EnumTypeName))
            {
                return Type.GetType(this.EnumTypeName);
            }
            else
            {
                if (this.EnumType == null || !this.EnumType.IsEnum)
                {
                    return null;
                }

                return this.EnumType;
            }
        }
    }

    public class EnumMemberVM : ViewModelBase
    {
        /// <summary>
        /// 枚举的值
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// 绑定的时候，ItemsControl会自动调用每个模型的ToString函数并显示到界面上
        /// 这里我们需要显示Name属性
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}