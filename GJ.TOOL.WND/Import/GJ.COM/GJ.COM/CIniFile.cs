using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace GJ.COM
{
    public class CIniFile
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileSection")]
        private extern static Int32 GetPrivateProfileSection(string lpApplicationName, byte[] lpReturnedString,
                                                            int nSize, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "WritePrivateProfileString")]
        private extern static Int32 WritePrivateProfileString(string lpApplicationName, string lpKeyName,
                                                            string lpString, string lpFileName);
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileString")]
        private extern static Int32 GetPrivateProfileString(string lpApplicationName, string lpKeyName,
                                                           string lpDefault, System.Text.StringBuilder lpReturnedString,
                                                           Int32 nSize, string lpFileName);
        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="lpApplicationName">应用集</param>
        /// <param name="lpKeyName">Key名</param>
        /// <param name="lpString">Key值</param>
        /// <param name="lpFileName">文件路径</param>
        public static void WriteToIni(string lpApplicationName, string lpKeyName, string lpString, string lpFileName)
        {
            WritePrivateProfileString(lpApplicationName, lpKeyName, lpString, lpFileName);
        }
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="lpApplicationName">应用集</param>
        /// <param name="lpKeyName">Key</param>
        /// <param name="lpFileName">Key值</param>
        /// <returns></returns>
        public static string ReadFromIni(string lpApplicationName, string lpKeyName, string lpFileName, string lpDefault = "")
        {
            StringBuilder strBuilder = new StringBuilder(250);
            GetPrivateProfileString(lpApplicationName, lpKeyName, lpDefault, strBuilder, strBuilder.Capacity, lpFileName);
            return strBuilder.ToString();
        }
        /// <summary>
        /// 获取应用集KEY组中参数名和数值
        /// </summary>
        /// <param name="lpApplicationName">应用集</param>
        /// <param name="keyNameList">Key名列表</param>
        /// <param name="keyValList">Key值列表</param>
        /// <param name="lpFileName">文件路径</param>
        /// <returns></returns>
        public static bool GetIniKeySection(string lpApplicationName, out List<string> keyNameList, out List<string> keyValList, string lpFileName)
        {
            keyNameList = new List<string>();

            keyValList = new List<string>();

            try
            {
                byte[] f_pData = new byte[1024];

                int nLen = GetPrivateProfileSection(lpApplicationName, f_pData, f_pData.Length, lpFileName);

                if (nLen > 0)
                {
                    string keys = System.Text.Encoding.Default.GetString(f_pData, 0, nLen - 1);

                    //string[] keyList = keys.Split((char)0);

                    string[] keyList = keys.Split('\0');

                    for (int i = 0; i < keyList.Length; i++)
                    {
                        int index = keyList[i].LastIndexOf("=");

                        keyNameList.Add(keyList[i].Substring(0, index));

                        keyValList.Add(keyList[i].Substring(index + 1, keyList[i].Length - index - 1));
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取KEY组中参数名和数值
        /// </summary>
        /// <param name="lpApplicationName">应用集</param>
        /// <param name="lpKeyValues">Key字典</param>
        /// <param name="lpFileName">文件路径</param>
        /// <returns></returns>
        public static bool GetIniKeySection(string lpApplicationName, ref Dictionary<string, string> lpKeyValues, string lpFileName)
        {
            try
            {
                byte[] f_pData = new byte[1024];

                int nLen = GetPrivateProfileSection(lpApplicationName, f_pData, f_pData.Length, lpFileName);

                if (nLen > 0)
                {
                    string keys = System.Text.Encoding.Default.GetString(f_pData, 0, nLen - 1);

                    //string[] keyList = keys.Split((char)0);

                    string[] keyList = keys.Split('\0');

                    for (int i = 0; i < keyList.Length; i++)
                    {
                        int index = keyList[i].LastIndexOf("=");

                        string keyName = keyList[i].Substring(0, index);

                        string keyVal = keyList[i].Substring(index + 1, keyList[i].Length - index - 1);

                        if (!lpKeyValues.ContainsKey(keyName))
                            lpKeyValues.Add(keyName, keyVal);
                        else
                            lpKeyValues[keyName] = keyVal;
                    }

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
