using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace gmt
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class Server
    {
        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < Server.serverList.Count; ++i)
            {
                Server server = Server.serverList[i];

                if (i != 0) { builder.Append("\n"); }
                builder.Append(server.Id).Append("\t").Append(server.Name).Append("\t").Append(server.GmAddress).Append("\t");
                builder.Append(server.DatabaseAddress).Append("\t").Append(server.DatabasePort).Append("\t");
                builder.Append(server.DatabaseCharSet).Append("\t").Append(server.DatabaseUserId).Append("\t");
                builder.Append(server.DatabasePassword).Append("\t").Append(server.GameDatabase).Append("\t");
                builder.Append(server.CodeDatabaseAddress).Append("\t").Append(server.CodeDatabasePort).Append("\t");
                builder.Append(server.CodeDatabaseCharSet).Append("\t").Append(server.CodeDatabaseUserId).Append("\t");
                builder.Append(server.CodeDatabasePassword).Append("\t").Append(server.CodeDatabase).Append("\t");

                builder.Append(server.LogDatabaseAddress).Append("\t").Append(server.LogDatabasePort).Append("\t");
                builder.Append(server.LogDatabaseCharSet).Append("\t").Append(server.LogDatabaseUserId).Append("\t");
                builder.Append(server.LogDatabasePassword).Append("\t").Append(server.LogDatabase).Append("\t");

                builder.Append(server.BillDatabaseAddress).Append("\t").Append(server.BillDatabasePort).Append("\t");
                builder.Append(server.BillDatabaseCharSet).Append("\t").Append(server.BillDatabaseUserId).Append("\t");
                builder.Append(server.BillDatabasePassword).Append("\t").Append(server.BillDatabase);
            }


            byte[] buffer = Encoding.UTF8.GetBytes(builder.ToString());

            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)(buffer[i] ^ 0x37);
            }

            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Config.bytes";

            using (FileStream fileStream = File.Create(configBinaryFile))
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        public static void Load()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Config.bytes";

            if (File.Exists(configBinaryFile))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(configBinaryFile)))
                {
                    byte[] buffer = reader.ReadBytes((int)reader.BaseStream.Length);

                    for (int i = 0; i < buffer.Length; ++i)
                    {
                        buffer[i] = (byte)(buffer[i] ^ 0x37);
                    }

                    string text = Encoding.UTF8.GetString(buffer);

                    string[] lineSet = text.Replace("\r\n", "\n").Split('\n');

                    foreach (string line in lineSet)
                    {
                        string[] temp = line.Split(new char[] { '\t' }, 27);
                        string[] parameterSet = new string[27];
                        for (int i = 0; i < temp.Length; i++)
                        {
                            parameterSet[i] = temp[i];
                        }

                        gmt.Server.AddServer(new Server
                        (
                            parameterSet[0],
                            parameterSet[1],
                            parameterSet[2],
                            parameterSet[3],
                            parameterSet[4],
                            parameterSet[5],
                            parameterSet[6],
                            parameterSet[7],
                            parameterSet[8],
                            parameterSet[9],
                            parameterSet[10],
                            parameterSet[11],
                            parameterSet[12],
                            parameterSet[13],
                            parameterSet[14],

                            parameterSet[15],
                            parameterSet[16],
                            parameterSet[17],
                            parameterSet[18],
                            parameterSet[19],
                            parameterSet[20],

                            parameterSet[21],
                            parameterSet[22],
                            parameterSet[23],
                            parameterSet[24],
                            parameterSet[25],
                            parameterSet[26]
                        ));
                    }
                }
            }
        }

        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="server">服务器</param>
        public static void AddServer(Server server)
        {
            Server.serverList.Add(server);
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>服务器</returns>
        public static Server GetServer(string name)
        {
            if (string.IsNullOrEmpty(name)) { return null; }

            Regex reg = new Regex(@"\(.*\)");
            name = reg.Replace(name, "");

            foreach (var server in Server.serverList)
            {
                if (server.Name == name) { return server; }
            }

            return null;
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>服务器</returns>
        public static Server GetServerAt(int index)
        {
            if (index >= 0 && index < Server.serverList.Count)
            {
                return Server.serverList[index];
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 服务器数量
        /// </summary>
        public static int Count
        {
            get
            {
                return Server.serverList.Count;
            }
        }

        private static uint GetServerIDByName(string name)
        {
            string[] str = name.Split('-');
            byte[] bytes = new byte[str.Length];
            for (int i = 0; i < str.Length; ++i)
            {
                bytes[i] = Convert.ToByte(str[i]);
            }

            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="name">名称</param>
        /// <param name="gmAddress">GM地址</param>
        /// <param name="databaseAddress">数据库地址</param>
        /// <param name="databasePort">数据库端口</param>
        /// <param name="databaseCharSet">数据库字符集</param>
        /// <param name="databaseUserId">数据库用户</param>
        /// <param name="databasePassword">数据库密码</param>
        /// <param name="gameDatabase">游戏数据库</param>
        /// <param name="codeDatabaseAddress">发码数据库地址</param>
        /// <param name="codeDatabasePort">发码数据库端口</param>
        /// <param name="codeDatabaseCharSet">发码数据库字符集</param>
        /// <param name="codeDatabaseUserId">发码数据库用户</param>
        /// <param name="codeDatabasePassword">发码数据库密码</param>
        /// <param name="codeDatabase">发码数据库</param>
        public Server
        (
            string id,
            string name,
            string gmAddress,
            string databaseAddress,
            string databasePort,
            string databaseCharSet,
            string databaseUserId,
            string databasePassword,
            string gameDatabase,
            string codeDatabaseAddress,
            string codeDatabasePort,
            string codeDatabaseCharSet,
            string codeDatabaseUserId,
            string codeDatabasePassword,
            string codeDatabase,
            string logDatabaseAddress,
            string logDatabasePort,
            string logDatabaseCharSet,
            string logDatabaseUserId,
            string logDatabasePassword,
            string logDatabase,
            string billDatabaseAddress,
            string billDatabasePort,
            string billDatabaseCharSet,
            string billDatabaseUserId,
            string billDatabasePassword,
            string billDatabase
        )
        {
            this.Id = id;
            this.serverID = GetServerIDByName(Id);
            this.Name = name;
            this.GmAddress = gmAddress;
            this.DatabaseAddress = databaseAddress;
            this.DatabasePort = databasePort;
            this.DatabaseCharSet = databaseCharSet;
            this.DatabaseUserId = databaseUserId;
            this.DatabasePassword = databasePassword;
            this.GameDatabase = gameDatabase;
            this.CodeDatabaseAddress = codeDatabaseAddress;
            this.CodeDatabasePort = codeDatabasePort;
            this.CodeDatabaseCharSet = codeDatabaseCharSet;
            this.CodeDatabaseUserId = codeDatabaseUserId;
            this.CodeDatabasePassword = codeDatabasePassword;
            this.CodeDatabase = codeDatabase;

            this.LogDatabaseAddress = logDatabaseAddress;
            this.LogDatabasePort = logDatabasePort;
            this.LogDatabaseCharSet = logDatabaseCharSet;
            this.LogDatabaseUserId = logDatabaseUserId;
            this.LogDatabasePassword = logDatabasePassword;
            this.LogDatabase = logDatabase;

            this.BillDatabaseAddress = billDatabaseAddress;
            this.BillDatabasePort = billDatabasePort;
            this.BillDatabaseCharSet = billDatabaseCharSet;
            this.BillDatabaseUserId = billDatabaseUserId;
            this.BillDatabasePassword = billDatabasePassword;
            this.BillDatabase = billDatabase;

        }

        /// <summary>
        /// 编号
        /// </summary>
        public string Id;

        public uint serverID;

        public string Name;

        public string GmAddress;

        public string DatabaseAddress;

        public string DatabasePort;

        public string DatabaseCharSet;

        public string DatabaseUserId;

        public string DatabasePassword;

        public string GameDatabase;

        public string CodeDatabaseAddress;

        public string CodeDatabasePort;

        public string CodeDatabaseCharSet;

        public string CodeDatabaseUserId;

        public string CodeDatabasePassword;

        public string CodeDatabase;

        public string LogDatabaseAddress;

        public string LogDatabasePort;

        public string LogDatabaseCharSet;

        public string LogDatabaseUserId;

        public string LogDatabasePassword;

        public string LogDatabase;

        public string BillDatabaseAddress;


        public string BillDatabasePort;

        public string BillDatabaseCharSet;

        public string BillDatabaseUserId;

        public string BillDatabasePassword;

        public string BillDatabase;
        private static List<Server> serverList = new List<Server>();




        public static STServerInfo GetServerInfo(uint id)
        {
            if (dicServers.ContainsKey(id))
            {
                return dicServers[id];
            }
            return null;
        }

        public static List<STServerInfo> GetServerList()
        {
            return dicServers.Values.ToList();
        }

        public static bool AddServerInfo(uint id, STServerInfo server)
        {
            if (dicServers.ContainsKey(id))
            {
                dicServers[id] = server;
            }
            else
            {
                dicServers.Add(id, server);
            }
            newSave();
            return true;
        }

        public static bool RemoveServerInfo(uint id)
        {
            if (dicServers.ContainsKey(id))
            {
                dicServers.Remove(id);
                return true;
            }
            return false;
        }

        public static void newSave()
        {
            string svrJsonData = JsonConvert.SerializeObject(dicServers);
            byte[] buffer = Encoding.UTF8.GetBytes(svrJsonData);

            for (int i = 0; i < buffer.Length; ++i)
            {
                buffer[i] = (byte)(buffer[i] ^ 0x37);
            }

            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Configdd.bytes";

            using (FileStream fileStream = File.Create(configBinaryFile))
            {
                fileStream.Write(buffer, 0, buffer.Length);
            }
        }

        public static void newLoad()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Configdd.bytes";

            if (File.Exists(configBinaryFile))
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(configBinaryFile)))
                {
                    byte[] buffer = reader.ReadBytes((int)reader.BaseStream.Length);

                    for (int i = 0; i < buffer.Length; ++i)
                    {
                        buffer[i] = (byte)(buffer[i] ^ 0x37);
                    }

                    string text = Encoding.UTF8.GetString(buffer);
                    dicServers = JsonConvert.DeserializeObject<Dictionary<uint, STServerInfo>>(text);
                }
            }
        }

        private static Dictionary<uint, STServerInfo> dicServers = new Dictionary<uint, STServerInfo>();
    }

    public class STDBInfo
    {
        public string name;
        public string host;
        public ushort port;
        public string user;
        public string pwd;
        public string cset;
    }

    public class STServerInfo
    {
        public uint id;
        public string svrID;
        public string name;
        public int section;
        public string sectionName;
        public string ip;
        public int status;
        public string authGMHttp;
        public bool recommend;
        public string param;
        public STDBInfo gamedb;
        public STDBInfo codedb;
        public STDBInfo logdb;
        public STDBInfo authdb;
    }
}