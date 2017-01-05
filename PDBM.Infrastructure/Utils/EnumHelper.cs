using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 枚举工具
    /// </summary>
    public static class EnumHelper
    {
        public static string GetEnumText(Type enumType, object value)
        {
            return Enum.GetName(enumType, value);
        }

        /// <summary>
        /// 将枚举类型转为列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>包含所有枚举值的列表</returns>
        public static List<Dictionary<string, string>> EnumToList(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (object value in enumValues)
            {
                Dictionary<string, string> dict = new Dictionary<string, string>(2);
                dict.Add("id", Enum.Format(enumType, value, "D"));
                dict.Add("text", Enum.GetName(enumType, value));
                list.Add(dict);
            }
            return list;
        }

        /// <summary>
        ///  将枚举类型中指定的枚举值转为列表
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="specifiedValueStr">指定的枚举值字符串，值与值之间以逗号分隔</param>
        /// <returns>包含指定枚举值的列表</returns>
        public static List<Dictionary<string, string>> EnumToList(Type enumType, string specifiedValuesStr)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }
            if (string.IsNullOrWhiteSpace(specifiedValuesStr))
            {
                throw new ArgumentNullException("specifiedValuesStr");
            }

            List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
            string[] specifiedValues = specifiedValuesStr.Split(',');
            Array enumValues = Enum.GetValues(enumType);
            foreach (object value in enumValues)
            {
                if (specifiedValues.Contains(Enum.Format(enumType, value, "D")))
                {
                    Dictionary<string, string> dict = new Dictionary<string, string>(2);
                    dict.Add("id", Enum.Format(enumType, value, "D"));
                    dict.Add("text", Enum.GetName(enumType, value));
                    list.Add(dict);
                }
            }
            return list;
        }

        /// <summary>
        /// 将枚举类型转为列表，用于树形显示
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns>包含所有枚举值的列表</returns>
        public static List<Dictionary<string, object>> EnumToListByTree(Type enumType)
        {
            if (enumType == null)
            {
                throw new ArgumentNullException("enumType");
            }

            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            Array enumValues = Enum.GetValues(enumType);
            foreach (object value in enumValues)
            {
                Dictionary<string, object> dict = new Dictionary<string, object>(5);
                dict.Add("id", Enum.Format(enumType, value, "D"));
                dict.Add("text", Enum.GetName(enumType, value));
                dict.Add("pid", "0");
                dict.Add("isLeaf", true);
                dict.Add("asyncLoad", false);
                list.Add(dict);
            }
            return list;
        }
    }
}
