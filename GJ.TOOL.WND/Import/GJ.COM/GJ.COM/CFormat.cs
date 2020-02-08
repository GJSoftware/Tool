using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace GJ.COM
{
    /// <summary>
    /// 字符格式化
    /// </summary>
    public class CFormat
    {        
        /// <summary>
        /// 检查IP地址是否合法?
        /// </summary>
        /// <param name="IP"></param>
        /// <returns></returns>
        public static bool IPCheck(string IP)
        {
            string num = "(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)";
            return Regex.IsMatch(IP, ("^" + num + "\\." + num + "\\." + num + "\\." + num + "$"));
        }
        /// <summary>
        /// 检查url地址是否合法?
        /// </summary>
        /// <param name="str_url"></param>
        /// <returns></returns>
        public static bool IsUrl(string str_url)
        {
            return Regex.IsMatch(str_url, @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
        }
        /// <summary>
        /// 电话号码是否合法?
        /// </summary>
        /// <param name="str_telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(string str_telephone)
        {
            return Regex.IsMatch(str_telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        /// 检查字符串是否由数字和26个英文字母?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidChar(string str)
        {
            return Regex.IsMatch(str, "^[0-9a-zA-Z]+$");
        }
    }
}
