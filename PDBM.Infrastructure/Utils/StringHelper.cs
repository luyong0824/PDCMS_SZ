using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDBM.Infrastructure.Utils
{
    /// <summary>
    /// 字符串工具
    /// </summary>
    public static class StringHelper
    {
        private static SymmetricAlgorithm objCrypto = new RijndaelManaged();
        private static string key = "9HuB045G$yuz(86Ftma%&hj7x(UF*jkIPh%(H87jH7Cy76T5&lhj!y6&*ilJ$fvH";

        private static byte[] GetLegalKey()
        {
            string text = key;
            objCrypto.GenerateKey();
            byte[] keys = objCrypto.Key;
            int num = keys.Length;
            if (text.Length > num)
            {
                text = text.Substring(0, num);
            }
            else if (text.Length < num)
            {
                text = text.PadRight(num, ' ');
            }
            return Encoding.ASCII.GetBytes(text);
        }

        private static byte[] GetLegalIV()
        {
            string text = "hg7!hUfb&4ghj*r5Y86b%h(G#e$7H(rNIBg4uiJGfg95hg6HGUEk7u%hW&!hjk$j";
            objCrypto.GenerateIV();
            byte[] ivs = objCrypto.IV;
            int num = ivs.Length;
            if (text.Length > num)
            {
                text = text.Substring(0, num);
            }
            else if (text.Length < num)
            {
                text = text.PadRight(num, ' ');
            }
            return Encoding.ASCII.GetBytes(text);
        }

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="source">原字符串</param>
        /// <returns>加密的字符串</returns>
        public static string Encrypto(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(source);
            MemoryStream memoryStream = new MemoryStream();
            objCrypto.Key = GetLegalKey();
            objCrypto.IV = GetLegalIV();
            ICryptoTransform transform = objCrypto.CreateEncryptor();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            memoryStream.Close();
            byte[] inArray = memoryStream.ToArray();
            return Convert.ToBase64String(inArray);
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="source">加密的字符串</param>
        /// <returns>原字符串</returns>
        public static string Decrypto(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException("source");
            }

            byte[] array = Convert.FromBase64String(source);
            MemoryStream stream = new MemoryStream(array, 0, array.Length);
            objCrypto.Key = GetLegalKey();
            objCrypto.IV = GetLegalIV();
            ICryptoTransform transform = objCrypto.CreateDecryptor();
            CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(stream2);
            return streamReader.ReadToEnd();
        }

        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string result = "";
            for (int i = 0; i < b.Length; i++)
            {
                result += b[i].ToString("x").PadLeft(2, '0');
            }
            return result;
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// HTML字符串编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string HtmlEncode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// HTML字符串解码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string HtmlDecode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            return HttpUtility.HtmlDecode(str);
        }

        /// <summary>
        /// URL字符串编码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>编码结果</returns>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            return HttpUtility.UrlEncode(str);
        }

        /// <summary>
        /// URL字符串解码
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>解码结果</returns>
        public static string UrlDecode(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                throw new ArgumentNullException("str");
            }

            return HttpUtility.UrlDecode(str);
        }

        /// <summary>
        /// 写入Cookie
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void WriteCookie(string name, string value, int expires)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie == null)
            {
                cookie = new HttpCookie(name);
            }
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            HttpContext.Current.Response.AppendCookie(cookie);
        }

        /// <summary>
        /// 读取Cookie
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>Cookie值</returns>
        public static string GetCookie(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (HttpContext.Current.Request.Cookies != null &&
                HttpContext.Current.Request.Cookies[name] != null)
            {
                return HttpContext.Current.Request.Cookies[name].Value.ToString();
            }
            return "";
        }
    }
}
