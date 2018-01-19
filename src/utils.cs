using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Text;

namespace gmt
{
    public static class CUtils
    {
        /// <summary>
        /// 获取Json的Value值
        /// </summary>
        /// <param name="jsondata"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetValueFromJson(string jsondata, string key)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsondata);
            if (obj.ContainsKey(key))
            {
                return obj[key];
            }
            else
            {
                return "";
            }
        }

        /// ===== INI File About ===== //
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        public static string ReadIniValue(string strSection, string strKey, string strDef, string strPath)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(strSection, strKey, strDef, temp, 255, strPath);
            return temp.ToString();
        }
        public static void WriteIniValue(string strSection, string strKey, string strValue, string strPath)
        {
            WritePrivateProfileString(strSection, strKey, strValue, strPath);
        }

        public static uint GetTimestamp(DateTime dt)
        {
            DateTime DateStart = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return Convert.ToUInt32((dt - DateStart).TotalSeconds);
        }

        public static bool HasFlag(int flag, int pos)
        {
            return (uint)(flag & (1 << pos)) > 0;
        }

        public static void InsertFlag(ref int flag, int pos)
        {
            flag = flag | (1 << pos);
        }

        public static void RemoveFlag(ref int flag, int pos)
        {
            flag = flag & ~(1 << pos);
        }

        public static ushort MakeWord(byte low, byte high)
        {
            return (ushort)(((ushort)high << 8) | low);
        }
        public static uint MakeLong(ushort low, ushort high)
        {
            return (((uint)high << 16) | low);
        }
        public static uint idatoi(string svr_id)
        {
            string[] str = svr_id.Split('-');
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; ++i)
            {
                bytes[i] = Convert.ToByte(str[i]);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }
    }
}