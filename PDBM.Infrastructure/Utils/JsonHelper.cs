using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// Json工具
    /// </summary>
    public static class JsonHelper
    {
        private static IsoDateTimeConverter dt = new IsoDateTimeConverter() { DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss" };

        /// <summary>
        /// 将对象序列化为Json字符串
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>Json字符串</returns>
        public static string Encode(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            if (obj.GetType() == typeof(string))
            {
                return obj.ToString();
            }
            return JsonConvert.SerializeObject(obj, dt);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <returns>对象</returns>
        public static object Decode(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json");
            }

            object obj = JsonConvert.DeserializeObject(json);
            if (obj.GetType() == typeof(string))
            {
                obj = JsonConvert.DeserializeObject(obj.ToString());
            }
            return ToObject(obj);
        }

        /// <summary>
        /// 将Json字符串反序列化为指定类型的对象
        /// </summary>
        /// <param name="json">Json字符串</param>
        /// <param name="type">指定类型</param>
        /// <returns>指定类型的对象</returns>
        public static object Decode(string json, Type type)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ArgumentNullException("json");
            }
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return JsonConvert.DeserializeObject(json, type);
        }

        /// <summary>
        /// 对象转换
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object ToObject(object obj)
        {
            if (obj == null)
            {
                return null;
            }

            if (obj.GetType() == typeof(string))
            {
                string str = obj.ToString();
                if (RegexHelper.IsISO8601DateTime(str))
                {
                    obj = Convert.ToDateTime(obj);
                }
            }
            else if (obj is JObject)
            {
                JObject jObj = obj as JObject;
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (KeyValuePair<string, JToken> kvp in jObj)
                {
                    dic[kvp.Key] = ToObject(kvp.Value);
                }
                obj = dic;
            }
            else if (obj is IList)
            {
                ArrayList list = new ArrayList();
                list.AddRange((obj as IList));
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = ToObject(list[i]);
                }
                obj = list;
            }
            else if (obj.GetType() == typeof(JValue))
            {
                JValue jVal = (JValue)obj;
                obj = ToObject(jVal.Value);
            }

            return obj;
        }
    }
}
