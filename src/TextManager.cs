using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gmt
{
    /// <summary>
    /// 文本管理器
    /// </summary>
    public static class TextManager
    {
        /// <summary>
        /// 开始编号
        /// </summary>
        public const int IdStart = 10000000;

        /// <summary>
        /// 创建新文本
        /// </summary>
        /// <returns>编号</returns>
        public static int CreateText()
        {
            lock (TextManager.textDictionary)
            {
                mw.UIDescConfig config = new mw.UIDescConfig();
                config.id = ++TextManager.idIncrease;

                TextManager.textDictionary.Add(config.id, config);

                TextManager.Save();

                return config.id;
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="id">文本编号</param>
        /// <param name="text">文本内容</param>
        public static void SetText(int id, string text)
        {
            lock (TextManager.textDictionary)
            {
                mw.UIDescConfig config;
                if (TextManager.textDictionary.TryGetValue(id, out config))
                {
                    config.desc = text;
                    TextManager.Save();
                }
            }
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="id">文本编号</param>
        /// <returns>文本内容</returns>
        public static string GetText(int id)
        {
            lock (TextManager.textDictionary)
            {
                mw.UIDescConfig config;

                TextManager.textDictionary.TryGetValue(id, out config);

                return config != null ? config.desc : string.Empty;
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        public static void Load()
        {

            TextManager.textDictionary.Clear();

            List<mw.UIDescConfig>[] listSet =
            {
                TableManager.Load<mw.UIDescConfig>("protodatas/UIDescConfig.protodata.bytes"),
                TableManager.Load<mw.UIDescConfig>("protodatas/UIDescExConfig.protodata.bytes"),
            };

            foreach (var list in listSet)
            {
                if (list != null)
                {
                    foreach (var config in list)
                    {
                        try
                        {
                            TextManager.textDictionary.Add(config.id, config);
                            TextManager.idIncrease = Math.Max(TextManager.idIncrease, config.id + 1);
                            //Log.AddLog("不报错ID：" + config.id);
                        }
                        catch (Exception exception)
                        {
                            Log.AddLog(exception.ToString());
                            Log.AddLog("报错ID：" + config.id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            TableManager.Save(TextManager.GetConfigList());
        }

        /// <summary>
        /// 获取配置列表
        /// </summary>
        /// <returns>配置列表</returns>
        public static List<mw.UIDescConfig> GetConfigList()
        {
            List<mw.UIDescConfig> list = new List<mw.UIDescConfig>();

            foreach (var config in TextManager.textDictionary.Values)
            {
                if (config.id >= TextManager.IdStart)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        /// <summary>
        /// 编号增长
        /// </summary>
        private static int idIncrease = TextManager.IdStart;

        /// <summary>
        /// 文本字典
        /// </summary>
        private static Dictionary<int, mw.UIDescConfig> textDictionary = new Dictionary<int, mw.UIDescConfig>();
    }
}