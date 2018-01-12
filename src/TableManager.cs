using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;

namespace gmt
{
    /// <summary>
    /// 表格管理器
    /// </summary>
    public static class TableManager
    {

        #region 常量定义

        /// <summary>
        /// 目录
        /// </summary>
        public const string Directory = "ClientProto/";

        /// <summary>
        /// 写入长度
        /// </summary>
        private const int WriteLength = 1024;

        private static string errorText = "";

        #endregion

        #region 对外方法

        /// <summary>
        /// 开始
        /// </summary>
        public static void Start()
        {
            List<mw.ActivityConfig> listActivity = TableManager.Load<mw.ActivityConfig>("protodatas/ActivityConfig.protodata.bytes");
            if (null != listActivity)
            {
                foreach (var config in listActivity)
                {
                    ActivityTable.Add(config.id, config);
                }
            }

            List<mw.ItemConfig> itemList = TableManager.Load<mw.ItemConfig>();
            if (itemList != null)
            {
                foreach (var item in itemList)
                {
                    if (item.type != mw.Enums.ItemType.ITEM_TYPE_PET_STONE)
                    {
                        if (!TableManager.ItemTable.ContainsKey(item.id))
                        {
                            TableManager.ItemTable.Add(item.id, item);
                        }
                    }
                    else
                    {
                        if (!TableManager.PetStoneTable.ContainsKey(item.id))
                        {
                            TableManager.PetStoneTable.Add(item.id, item);
                        }
                    }
                }
            }

            List<mw.HeroBaseConfig> heroList = TableManager.Load<mw.HeroBaseConfig>();
            if (heroList != null)
            {
                foreach (var card in heroList)
                {
                    TableManager.HeroTable.Add(card.id, card);
                }
            }

            List<mw.PetConfig> petList = TableManager.Load<mw.PetConfig>();
            if (petList != null)
            {
                foreach (var pet in petList)
                {
                    TableManager.PetTable.Add(pet.idx, pet);
                }
            }

            List<mw.GMTDescConfig> gmtDescList = TableManager.Load<mw.GMTDescConfig>();
            if (gmtDescList != null)
            {
                foreach (var gmtDesc in gmtDescList)
                {
                    TableManager.GMTDescTable.Add(gmtDesc.id, gmtDesc);
                }
            }

            List<mw.RmbShopConfig> rmbShopList = TableManager.Load<mw.RmbShopConfig>();
            if (null != rmbShopList)
            {
                for (int i = 0; i < rmbShopList.Count; i++)
                {
                    TableManager.RmbShopTable.Add(rmbShopList[i].goods_RegisterId, rmbShopList[i]);
                }
            }

            List<mw.MountConfig> mountList = TableManager.Load<mw.MountConfig>();
            if (null != mountList)
            {
                for (int i = 0; i < mountList.Count; ++i)
                {
                    if (!TableManager.MountTable.ContainsKey(mountList[i].id))
                    {
                        TableManager.MountTable.Add(mountList[i].id, mountList[i]);
                    }
                }
            }

            List<mw.StoneConfig> stoneList = TableManager.Load<mw.StoneConfig>();
            if (null != stoneList)
            {
                for (int i = 0; i < stoneList.Count; ++i)
                {
                    TableManager.StoneTable.Add(stoneList[i].id, stoneList[i]);
                }
            }
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="list">配置列表</param>
        /// <param name="sendType">发送类型</param>
        /// <param name="initializedServer">初始化服务器</param>
        /// <returns>发送信息</returns>
        public static string Send<T>(List<T> list, int sendType = -1, gmt.Server initializedServer = null) where T : class, global::ProtoBuf.IExtensible
        {
            Type type = typeof(T);

            string[] name;
            if (!TableManager.tableNameDictionary.TryGetValue(type, out name))
            {
                Log.AddLog(string.Format("错误表格的类型:", type.FullName));
                return TableManager.GetGMTText(901);
            }

            string output = "";
            if (sendType < 0)
            {
                if (!string.IsNullOrEmpty(name[2]))
                {
                    sendType = int.Parse(name[2]);
                }
            }

            if (sendType >= 0)
            {
                byte[] versions = { (byte)DateTime.Now.Month, (byte)DateTime.Now.Day, (byte)DateTime.Now.Hour, (byte)DateTime.Now.Minute };
                mw.AUTH_GMT_SETTINT_Ntf Ntf = new mw.AUTH_GMT_SETTINT_Ntf();
                Ntf.type = (mw.EGMTSettintType)sendType;
                Ntf.verson = BitConverter.ToInt32(versions, 0);

                mw_serializer0 serializer = new mw_serializer0();

                for (int i = 0; i < list.Count; ++i)
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        serializer.Serialize(stream, list[i]);
                        byte[] buffer = new byte[stream.Length];
                        stream.Position = 0;
                        stream.Read(buffer, 0, buffer.Length);

                        mw.AUTH_GMT_SETTINT_NODE node = new mw.AUTH_GMT_SETTINT_NODE();
                        node.len = buffer.Length;
                        node.data = buffer;

                        Ntf.info.Add(node);
                    }
                }

                MemoryStream memoryStream = new MemoryStream();
                ProtoSerializer.Instance.Serialize(memoryStream, Ntf);
                byte[] sendBuffer = new byte[memoryStream.Length];
                memoryStream.Position = 0;
                memoryStream.Read(sendBuffer, 0, sendBuffer.Length);

                if (initializedServer != null)
                {
                    output = TableManager.Send2Server(output, initializedServer, sendBuffer);
                }
                else
                {
                    for (int i = 0; i < gmt.Server.Count; ++i)
                    {
                        output = TableManager.Send2Server(output, gmt.Server.GetServerAt(i), sendBuffer);
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="list">配置列表</param>
        /// <returns>数据缓冲区</returns>
        public static byte[] Serialize<T>(List<T> list) where T : class, global::ProtoBuf.IExtensible
        {
            using (var writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write(typeof(T).FullName);
                writer.Write((uint)list.Count);

                mw_serializer0 serializer = new mw_serializer0();

                for (int i = 0; i < list.Count; ++i)
                {
                    MemoryStream stream = new MemoryStream();
                    serializer.Serialize(stream, list[i]);
                    writer.Write((int)stream.Length);
                    writer.Write(stream.GetBuffer(), 0, (int)stream.Length);
                }

                byte[] buffer = new byte[writer.BaseStream.Length];
                Array.Copy((writer.BaseStream as MemoryStream).GetBuffer(), 0, buffer, 0, buffer.Length);

                return buffer;
            }
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="buffer">缓冲区</param>
        /// <returns>配置列表</returns>
        public static List<T> Unserialize<T>(byte[] buffer) where T : class, global::ProtoBuf.IExtensible
        {
            ProtoData<T> data = new ProtoData<T>(buffer);
            List<T> list = new List<T>();

            for (int i = 0; i < data.Count; ++i)
            {
                list.Add(data[i]);
            }

            return list;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="list">配置列表</param>
        /// <returns>是否成功</returns>
        public static bool Save<T>(List<T> list) where T : class, global::ProtoBuf.IExtensible
        {
            if (list.Count == 0)
            {
                return false;
            }

            Type type = typeof(T);

            string[] name;
            if (!TableManager.tableNameDictionary.TryGetValue(type, out name))
            {
                Log.AddLog(string.Format("错误表格的类型:", type.FullName));
                return false;
            }

            byte[] buffer = TableManager.Serialize(list);
            if (buffer == null) { return false; }

            string configBinaryFile = HttpRuntime.AppDomainAppPath + "protodatas/" + name[0];
            using (FileStream fileStream = File.Create(configBinaryFile))
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }

            return true;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T">配置类型</typeparam>
        /// <param name="file">文件</param>
        /// <returns>配置列表</returns>
        public static List<T> Load<T>(string file = "") where T : class, global::ProtoBuf.IExtensible
        {
            Type type = typeof(T);

            string Path = "";

            if (string.IsNullOrEmpty(file))
            {
                string[] name;
                if (!TableManager.tableNameDictionary.TryGetValue(type, out name))
                {
                    Log.AddLog(string.Format("错误表格的类型:", type.FullName));
                    return null;
                }

                Path = HttpRuntime.AppDomainAppPath + "protodatas/" + name[0];
            }
            else
            {
                Path = HttpRuntime.AppDomainAppPath + file;
            }

            if (File.Exists(Path))
            {
                using (FileStream stream = File.OpenRead(Path))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    return TableManager.Unserialize<T>(buffer);
                }
            }
            else
            {
                Log.AddLog(string.Format("表格不存在:", type.FullName));
                return null;
            }
        }

        /// <summary>
        /// 清除MD5记录
        /// </summary>
        public static void ClearMD5Record()
        {
            TableManager.tableMD5Dictionary.Clear();
        }

        /// <summary>
        /// 更新MD5
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="name">名称</param>
        public static void UpdateMD5(byte[] buffer, string name)
        {
            TableMD5 md5;
            if (!tableMD5Dictionary.TryGetValue(name, out md5))
            {
                md5 = new TableMD5();
                md5.Name = name;
                tableMD5Dictionary.Add(name, md5);
            }

            md5.Size = buffer.Length;
            md5.MD5 = TableManager.GetMd5Hash(buffer);
        }

        /// <summary>
        /// 获取MD5文本
        /// </summary>
        /// <returns>MD5文本</returns>
        public static string GetMD5Text()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var pair in tableMD5Dictionary)
            {
                if (builder.Length > 0) { builder.AppendLine(); }

                builder.Append(pair.Key).Append('$').Append(pair.Value.MD5).Append('$').Append(pair.Value.Size);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 发送表格
        /// </summary>
        /// <param name="version">版本号</param>
        /// <returns>是否成功</returns>
        public static bool SendTable(string version)
        {
            byte[] buffer = null;
            errorText = "";

            foreach (var data in ServerListConfig.DataList)
            {
                string md5Path = version + "/updateex/" + data.Name + "/";
                string tablePath = md5Path + TableManager.Directory;
                FTPManager.MakeDirectory(version + "/");
                FTPManager.MakeDirectory(version + "/updateex/");
                FTPManager.MakeDirectory(version + "/updateex/" + data.Name);
                FTPManager.MakeDirectory(tablePath);

                TableManager.ClearMD5Record();

                // 活动
                List<mw.ActivityConfig> Updateactivitytable = GMTActivityMananger.GetTableActivity().Values.ToList();
                buffer = TableManager.Serialize(Updateactivitytable);
                TableManager.UpdateMD5(buffer, TableManager.Directory + tableNameDictionary[typeof(mw.ActivityConfig)][0]);
                if (!FTPManager.Upload(tablePath + tableNameDictionary[typeof(mw.ActivityConfig)][0], buffer))
                {
                    errorText += FTPManager.GetLastError();
                }

                List<mw.AchieveConfig> Updateachievetable = GMTActivityMananger.GetTableAchieve().Values.ToList();
                buffer = TableManager.Serialize(Updateachievetable);
                TableManager.UpdateMD5(buffer, TableManager.Directory + tableNameDictionary[typeof(mw.AchieveConfig)][0]);
                if (!FTPManager.Upload(tablePath + tableNameDictionary[typeof(mw.AchieveConfig)][0], buffer))
                {
                    errorText += FTPManager.GetLastError();
                }

                List<mw.RewardConfig> Updaterewardtable = GMTActivityMananger.GetTableReward().Values.ToList();
                buffer = TableManager.Serialize(Updaterewardtable);
                TableManager.UpdateMD5(buffer, TableManager.Directory + tableNameDictionary[typeof(mw.RewardConfig)][0]);
                if (!FTPManager.Upload(tablePath + tableNameDictionary[typeof(mw.RewardConfig)][0], buffer))
                {
                    errorText += FTPManager.GetLastError();
                }

                //编辑礼包
                List<mw.GiftConfig> UpdatereGiftTable = new List<mw.GiftConfig>();
                UpdatereGiftTable = GiftManager.addgiftconfig;
                buffer = TableManager.Serialize(UpdatereGiftTable);
                TableManager.UpdateMD5(buffer, TableManager.Directory + tableNameDictionary[typeof(mw.GiftConfig)][0]);
                if (!FTPManager.Upload(tablePath + tableNameDictionary[typeof(mw.GiftConfig)][0], buffer))
                {
                    errorText += FTPManager.GetLastError();
                }

                // 文本
                buffer = TableManager.Serialize(TextManager.GetConfigList());
                TableManager.UpdateMD5(buffer, TableManager.Directory + tableNameDictionary[typeof(mw.UIDescConfig)][0]);
                if (!FTPManager.Upload(tablePath + tableNameDictionary[typeof(mw.UIDescConfig)][0], buffer))
                {
                    errorText += FTPManager.GetLastError();
                }

                buffer = Encoding.UTF8.GetBytes(TableManager.GetMD5Text());
                if (!FTPManager.Upload(md5Path + "md5.txt", buffer))
                {
                    errorText += FTPManager.GetLastError();
                }
            }

            if (string.IsNullOrEmpty(errorText))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取gmt文本
        /// </summary>
        /// <param name="descIdx">文本id</param>
        /// <returns></returns>
        public static string GetGMTText(int descIdx)
        {
            //UserData userData =  
            string lang = "zh-CN";
            try
            {
                string account = HttpContext.Current.Session["user"] as string;
                UserInfo ud;
                if (UserManager.UserTable.TryGetValue(account, out ud))
                {
                    lang = ud.language;
                }
            }
            catch (Exception ex)
            {
                Log.AddLog(ex.ToString());
            }

            if (!GMTDescTable.ContainsKey(descIdx))
            {
                return "GetGMTText ERROR : " + descIdx + " NOT FOUND";
            }

            if (lang == "ko-KR")
            {
                return GMTDescTable[descIdx].desc_kr;
            }

            return GMTDescTable[descIdx].desc_cn;
        }

        #endregion

        #region 对外属性

        /// <summary>
        /// 活动表 
        /// </summary>
        public static Dictionary<int, mw.ActivityConfig> ActivityTable = new Dictionary<int, mw.ActivityConfig>();
        /// <summary>
        /// 物品表
        /// </summary>
        public static Dictionary<int, mw.ItemConfig> ItemTable = new Dictionary<int, mw.ItemConfig>();

        /// <summary>
        /// 英雄表
        /// </summary>
        public static Dictionary<int, mw.HeroBaseConfig> HeroTable = new Dictionary<int, mw.HeroBaseConfig>();

        ///// <summary>
        ///// 宠物表
        ///// </summary>
        public static Dictionary<int, mw.PetConfig> PetTable = new Dictionary<int, mw.PetConfig>();

        //CommonshopTable
        public static List<mw.CommonShopConfig> CommonShopTable = new List<mw.CommonShopConfig>();

        //翻译表
        public static Dictionary<int, mw.GMTDescConfig> GMTDescTable = new Dictionary<int, mw.GMTDescConfig>();

        /// <summary>
        /// 商品表
        /// </summary>
        public static Dictionary<string, mw.RmbShopConfig> RmbShopTable = new Dictionary<string, mw.RmbShopConfig>();

        /// <summary>
        /// 坐骑表
        /// </summary>
        public static Dictionary<int, mw.MountConfig> MountTable = new Dictionary<int, mw.MountConfig>();

        /// <summary>
        /// 饰品
        /// </summary>
        public static Dictionary<int, mw.StoneConfig> StoneTable = new Dictionary<int, mw.StoneConfig>();

        /// <summary>
        /// 晶石
        /// </summary>
        public static Dictionary<int, mw.ItemConfig> PetStoneTable = new Dictionary<int, mw.ItemConfig>();

        #endregion

        #region 内部方法

        /// <summary>
        /// 静态构造
        /// </summary>
        static TableManager()
        {
            TableManager.tableNameDictionary = new Dictionary<Type, string[]>()
			{
                { typeof(mw.ItemConfig),        new string[] { "ItemConfig.protodata.bytes",		"",		""	} },
				{ typeof(mw.HeroBaseConfig),    new string[] { "HeroBaseConfig.protodata.bytes",	"",		""	} },
                { typeof(mw.PetConfig),		    new string[] { "PetConfig.protodata.bytes",		"",		""	} },
                { typeof(mw.StoneConfig),       new string[] { "StoneConfig.protodata.bytes",	"",		""	} },
                { typeof(mw.VipConfig),         new string[] { "VipConfig.protodata.bytes",		"",		""	} },
                { typeof(mw.MountConfig),	    new string[] { "MountConfig.protodata.bytes",	"1",	"0"	} },
				{ typeof(mw.GMTDescConfig),		new string[] { "GMTDescConfig.protodata.bytes",	"1",	""	} },
				{ typeof(mw.RmbShopConfig),		new string[] { "RmbShopConfig.protodata.bytes",	"1",	""	} },
                { typeof(mw.ActivityConfig),	new string[] { "ActivityExConfig.protodata.bytes",	"1",	"0"	} },
                { typeof(mw.AchieveConfig),		new string[] { "AchieveExConfig.protodata.bytes",	"1",	"2"	} },
                { typeof(mw.RewardConfig),		new string[] { "RewardExConfig.protodata.bytes",	"1",	"3"	} },
				{ typeof(mw.GiftConfig),		new string[] { "GiftExConfig.protodata.bytes",		"",		"1"	} },
				{ typeof(mw.SysNtfConfig),		new string[] { "SysNtfExConfig.protodata.bytes",	"1",	""	} },
				{ typeof(mw.UIDescConfig),		new string[] { "UIDescExConfig.protodata.bytes",	"1",	""	} },
			};
        }

        /// <summary>
        /// 发送给服务器
        /// </summary>
        /// <param name="output">输出文本</param>
        /// <param name="server">服务器</param>
        /// <param name="sendBuffer">缓冲区</param>
        /// <returns>输出文本</returns>
        private static string Send2Server(string output, gmt.Server server, byte[] sendBuffer)
        {
            output += string.Format(TableManager.GetGMTText(235) + "{0}:", server.Name);
            if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("3"), sendBuffer, false, null))
            {
                output += TableManager.GetGMTText(755) + "<br />";
            }
            else
            {
                output += TableManager.GetGMTText(756) + "<br />";
            }

            return output;
        }
        /*
        /// <summary>
        /// 更新MD5
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <param name="name">名称</param>
        private static void UpdateMD5(byte[] buffer, string name)
        {
            FTP								ftp					= new FTP();
            byte[]							md5Buffer			= ftp.Download("md5.txt");
            Dictionary<string, TableMD5>	tableMD5Dictionary	= new Dictionary<string, TableMD5>();

            if (md5Buffer != null)
            {
                string[] lineSet = Encoding.UTF8.GetString(md5Buffer).Replace("\r\n", "\n").Split('\n');
                foreach (var line in lineSet)
                {
                    string[] partSet = line.Split('$');
                    if (partSet.Length < 3) { continue; }

                    TableMD5 md5 = new TableMD5();
                    md5.Name = partSet[0];
                    md5.MD5 = partSet[1];
                    int.TryParse(partSet[2], out md5.Size);
                    tableMD5Dictionary.Add(md5.Name, md5);
                }
            }

            TableMD5 curretnMd5;
            if (!tableMD5Dictionary.TryGetValue(name, out curretnMd5))
            {
                curretnMd5 = new TableMD5();
                curretnMd5.Name = name;
                tableMD5Dictionary.Add(name, curretnMd5);
            }

            curretnMd5.Size = buffer.Length;
            curretnMd5.MD5 = TableManager.GetMd5Hash(buffer);

            StringBuilder builder = new StringBuilder();
            foreach (var pair in tableMD5Dictionary)
            {
                if (builder.Length > 0) { builder.AppendLine(); }

                builder.Append(pair.Key).Append('$').Append(pair.Value.MD5).Append('$').Append(pair.Value.Size);
            }

            ftp.Upload("md5.txt", Encoding.UTF8.GetBytes(builder.ToString()));
        }
        */
        /// <summary>
        /// 获取MD5字符串
        /// </summary>
        /// <param name="buffer">缓冲区</param>
        /// <returns>MD5字符串</returns>
        private static string GetMd5Hash(byte[] buffer)
        {
            MD5 md5Hash = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(buffer);

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }


        public static string GetLastError()
        {
            return errorText;
        }

        #endregion

        #region 变量

        /// <summary>
        /// 表格名称字典
        /// </summary>
        private static Dictionary<Type, string[]> tableNameDictionary;

        /// <summary>
        /// 表格MD5字典
        /// </summary>
        private static Dictionary<string, TableMD5> tableMD5Dictionary = new Dictionary<string, TableMD5>();

        #endregion

    }

    /// <summary>
    /// 表格MD5
    /// </summary>
    class TableMD5
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// MD5
        /// </summary>
        public string MD5;

        /// <summary>
        /// 尺寸
        /// </summary>
        public int Size;
    }

    /// <summary>
    /// CDNZip包
    /// </summary>
    static class CdnZip
    {
        /// <summary>
        /// 打包服务器列表
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns>Zip包缓冲区</returns>
        public static byte[] PackServerList(string version)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (ZipOutputStream stream = new ZipOutputStream(memory))
                {
                    byte[] buffer = null;

                    // 服务器列表
                    if (!string.IsNullOrEmpty(version) && version[version.Length - 1] != '/')
                    {
                        version = version + '/';
                    }

                    foreach (var data in ServerListConfig.DataList)
                    {
                        foreach (var channel in data.ChannelList)
                        {
                            string text = string.Format("#{0} #{1} #{2} #{3} #{4} #{5} #{6} #{7} #{8};\r\n"
                        , TableManager.GetGMTText(203)
                        , TableManager.GetGMTText(204)
                        , TableManager.GetGMTText(205)
                        , TableManager.GetGMTText(206)
                        , TableManager.GetGMTText(207)
                        , TableManager.GetGMTText(401)
                        , TableManager.GetGMTText(402)
                        , TableManager.GetGMTText(403)
                        , TableManager.GetGMTText(910))
                         + data.ServerList.GetText()
                         + data.Name;

                            ZipEntry entry = new ZipEntry("android/ServerList/" + version + "ServerList." + channel + ".txt");
                            stream.PutNextEntry(entry);
                            buffer = Encoding.UTF8.GetBytes(text);
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    stream.CloseEntry();
                    stream.Finish();

                    byte[] zipBuffer = new byte[memory.Length];
                    Array.Copy(memory.GetBuffer(), zipBuffer, zipBuffer.Length);

                    return zipBuffer;
                }
            }
        }

        /// <summary>
        /// 打包表格
        /// </summary>
        /// <param name="version">版本</param>
        /// <returns>Zip包缓冲区</returns>
        public static byte[] PackTable(string version)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (ZipOutputStream stream = new ZipOutputStream(memory))
                {
                    byte[] buffer = null;

                    // 服务器列表
                    if (!string.IsNullOrEmpty(version) && version[version.Length - 1] != '/')
                    {
                        version = version + '/';
                    }

                    foreach (var data in ServerListConfig.DataList)
                    {
                        string md5Path = version + "updateex/" + data.Name + "/";
                        string tablePath = md5Path + TableManager.Directory;

                        TableManager.ClearMD5Record();

                        // 活动
                        List<mw.ActivityConfig> Updateactivitytable = GMTActivityMananger.GetTableActivity().Values.ToList();
                        if (null != Updateactivitytable)
                        {
                            buffer = TableManager.Serialize(Updateactivitytable);
                            TableManager.UpdateMD5(buffer, TableManager.Directory + "ActivityExConfig.protodata.bytes");
                            stream.PutNextEntry(new ZipEntry(tablePath + "ActivityExConfig.protodata.bytes"));
                            stream.Write(buffer, 0, buffer.Length);
                        }

                        List<mw.AchieveConfig> Updateachievetable = GMTActivityMananger.GetTableAchieve().Values.ToList();
                        if (null != Updateachievetable)
                        {
                            buffer = TableManager.Serialize(Updateachievetable);
                            TableManager.UpdateMD5(buffer, TableManager.Directory + "AchieveExConfig.protodata.bytes");
                            stream.PutNextEntry(new ZipEntry(tablePath + "AchieveExConfig.protodata.bytes"));
                            stream.Write(buffer, 0, buffer.Length);
                        }

                        List<mw.RewardConfig> Updaterewardtable = GMTActivityMananger.GetTableReward().Values.ToList();
                        if (null != Updaterewardtable)
                        {
                            buffer = TableManager.Serialize(Updaterewardtable);
                            TableManager.UpdateMD5(buffer, TableManager.Directory + "RewardExConfig.protodata.bytes");
                            stream.PutNextEntry(new ZipEntry(tablePath + "RewardExConfig.protodata.bytes"));
                            stream.Write(buffer, 0, buffer.Length);
                        }

                        // 文本
                        buffer = TableManager.Serialize(TextManager.GetConfigList());
                        TableManager.UpdateMD5(buffer, TableManager.Directory + "UIDescExConfig.protodata.bytes");
                        stream.PutNextEntry(new ZipEntry(tablePath + "UIDescExConfig.protodata.bytes"));
                        stream.Write(buffer, 0, buffer.Length);

                        buffer = Encoding.UTF8.GetBytes(TableManager.GetMD5Text());
                        stream.PutNextEntry(new ZipEntry(md5Path + "md5.txt"));
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    stream.CloseEntry();
                    stream.Finish();

                    byte[] zipBuffer = new byte[memory.Length];
                    Array.Copy(memory.GetBuffer(), zipBuffer, zipBuffer.Length);

                    return zipBuffer;
                }
            }
        }
    }
}