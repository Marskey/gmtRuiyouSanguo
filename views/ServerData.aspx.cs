using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace gmt.views
{
    public class STServerTableData
    {
        public STServerTableData()
        {
            section = "";
            serverName = "";
            channelId = "";
            newUserCnt = 0;
            newDeviceCnt = 0;
            activeUserCnt = 0;
            newUserPayCnt = 0;
            totalPayCnt = 0;
            newUserPayVal = 0;
            totalPayVal = 0;
            newUserPayRate = 0;
            newPayUserCnt = 0;
            activePayRate = 0;
            arppu = 0;
            arpu = 0;
        }

        public string section;
        public string serverName;
        public string channelId;
        public int newUserCnt;
        public int newDeviceCnt;
        public int activeUserCnt;
        public int newUserPayCnt;
        public int totalPayCnt;
        public uint newUserPayVal;
        public uint totalPayVal;
        public float newUserPayRate;
        public int newPayUserCnt;
        public float activePayRate;
        public float arppu;
        public float arpu;
    };

    public class STSevenData
    {

        public STSevenData()
        {
            serverName = "";
            activeUserCnt = new int[7];
            totalPayVal = new int[7];
            totalPayCnt = new int[7];
        }
        public string serverName;
        public int[] activeUserCnt;
        public int[] totalPayVal;
        public int[] totalPayCnt;
    }

    public partial class ServerData : AGmPage
    {
        public ServerData()
            : base(PrivilegeType.ServerData)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
        }

        [WebMethod(EnableSession = true)]
        public static string QueryServerData(string channel_id, DateTime date_start, DateTime date_end)
        {
            List<STServerTableData> listTable = new List<STServerTableData>();

            for (int i = 0; i < gmt.Server.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServerAt(i);
                if (server != null)
                {
                    if (!GetServerDataFromDB(server, channel_id, date_start, date_end, ref listTable))
                        break;
                }
            }

            return JsonConvert.SerializeObject(listTable);
        }

        public static bool GetServerDataFromDB(gmt.Server server
                                                , string channel_id
                                                , DateTime date_start
                                                , DateTime date_end
                                                , ref List<STServerTableData> listTable)
        {
            STServerTableData stData = new STServerTableData();
            stData.section = server.Id;
            stData.serverName = server.Name;
            stData.channelId = channel_id;

            Dictionary<string, int> dicRawType = new Dictionary<string, int>(){
                {"lzsgkr_ios", 1},
                {"lzsgkr_google", 2},
                {"lzsgkr_onestore", 3}
            };

            string sqlCondi = "";
            string sqlAuthCondi = "";
            if (!string.IsNullOrEmpty(channel_id))
            {
                sqlCondi += string.Format("and channel_id = '{0}'", channel_id);
                sqlAuthCondi += string.Format("and raw_type={0}", dicRawType[channel_id]);
            }

            if (date_start != new DateTime(0))
            {
                sqlCondi += string.Format(" and date >= '{0}'", date_start.ToString("yyyy/MM/dd HH:mm:ss"));
                sqlAuthCondi += string.Format(" and last_update_time >= '{0}'", date_start.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            if (date_end != new DateTime(0))
            {
                sqlCondi += string.Format(" and date <= '{0}'", date_end.ToString("yyyy/MM/dd HH:mm:ss"));
                sqlAuthCondi += string.Format(" and last_update_time <= '{0}'", date_end.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            string sql = "";

            #region 今日活跃
            sql = string.Format("SELECT COUNT(DISTINCT cyuid) FROM login_ WHERE cyuid <> 'null' {0};", sqlCondi);
            bool suc = DatabaseAssistant.Execute(reader =>
               {
                   if (reader.Read())
                   {
                       stData.activeUserCnt += reader.GetInt32(0);
                   }
               },
               server.LogDatabaseAddress,
               server.LogDatabasePort,
               server.LogDatabaseCharSet,
               server.LogDatabase,
               server.LogDatabaseUserId,
               server.LogDatabasePassword,
               sql);
            #endregion
            if (!suc)
                return false;

            #region 今日新增
            sql = string.Format("SELECT COUNT(DISTINCT uid) FROM `rolebuild_` WHERE uid <> 'null' {0};", sqlCondi);
            DatabaseAssistant.Execute(reader =>
               {
                   if (reader.Read())
                   {
                       stData.newUserCnt += reader.GetInt32(0);
                   }
               },
               server.LogDatabaseAddress,
               server.LogDatabasePort,
               server.LogDatabaseCharSet,
               server.LogDatabase,
               server.LogDatabaseUserId,
               server.LogDatabasePassword,
               sql);
            #endregion

            #region 今日设备新增
            sql = string.Format("SELECT COUNT(DISTINCT deviceid) FROM `rolebuild_` WHERE uid <> 'null' {0};", sqlCondi);
            DatabaseAssistant.Execute(reader =>
               {
                   if (reader.Read())
                   {
                       stData.newDeviceCnt += reader.GetInt32(0);
                   }
               },
               server.LogDatabaseAddress,
               server.LogDatabasePort,
               server.LogDatabaseCharSet,
               server.LogDatabase,
               server.LogDatabaseUserId,
               server.LogDatabasePassword,
               sql);
            #endregion

            StringBuilder sbIds = new StringBuilder();
            #region 新用户付费数
            sql = string.Format("SELECT uid FROM rolebuild_ WHERE uid <> 'null' {0}", sqlCondi);
            DatabaseAssistant.Execute(reader =>
               {
                   while (reader.Read())
                   {
                       sbIds.Append(reader.GetString(0));
                       sbIds.Append(",");
                   }

                   if (sbIds.Length != 0)
                   {
                       sbIds.Remove(sbIds.Length - 1, 1);
                   }
               },
               server.LogDatabaseAddress,
               server.LogDatabasePort,
               server.LogDatabaseCharSet,
               server.LogDatabase,
               server.LogDatabaseUserId,
               server.LogDatabasePassword,
               sql
               );

            sql = string.Format("SELECT COUNT(DISTINCT userID) FROM receipt WHERE server_id = {0} and userID IN ({1})"
                , server.serverID
                , sbIds.ToString());
            DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        stData.newUserPayCnt += reader.GetInt32(0);
                    }
                },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);
            #endregion

            #region 总付费数
            sql = string.Format("SELECT COUNT(DISTINCT userID) FROM receipt WHERE server_id = {0} {1}"
                , server.serverID
                , sqlAuthCondi);
            DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        stData.totalPayCnt += reader.GetInt32(0);
                    }
                },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);
            #endregion

            #region 新用户充值额
            sql = string.Format("SELECT uid FROM rolebuild_ WHERE uid <> 'null' {0}", sqlCondi);
            sbIds.Remove(0, sbIds.Length);
            DatabaseAssistant.Execute(reader =>
               {
                   while (reader.Read())
                   {
                       sbIds.Append(reader.GetString(0));
                       sbIds.Append(",");
                   }

                   if (sbIds.Length != 0)
                   {
                       sbIds.Remove(sbIds.Length - 1, 1);
                   }
               },
               server.LogDatabaseAddress,
               server.LogDatabasePort,
               server.LogDatabaseCharSet,
               server.LogDatabase,
               server.LogDatabaseUserId,
               server.LogDatabasePassword,
               sql
               );

            sql = string.Format("SELECT orderID FROM receipt WHERE server_id = {0} and userID IN ({1})"
                , server.serverID
                , sbIds.ToString());
            DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        string orderInfo = reader.GetString(0);
                        string[] orderArray = orderInfo.Split(',');

                        mw.RmbShopConfig config = null;
                        string index = string.Format("{0}-{1}", orderArray[1], orderArray[4]);
                        TableManager.RmbShopTable.TryGetValue(index, out config);
                        int cost = (null == config) ? config.rmb_cost : 0;
                        stData.newUserPayVal += (uint)cost;
                    }
                },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);
            #endregion

            #region 总充值额
            sql = string.Format("SELECT orderID FROM receipt WHERE server_id = {0} {1}"
                , server.serverID
                , sqlAuthCondi);
            Log.AddLog(sql);
            DatabaseAssistant.Execute(reader =>
               {
                   while (reader.Read())
                   {
                        string orderInfo = reader.GetString(0);
                        string[] orderArray = orderInfo.Split(',');

                        mw.RmbShopConfig config = null;
                        string index = string.Format("{0}-{1}", orderArray[1], orderArray[4]);
                        TableManager.RmbShopTable.TryGetValue(index, out config);
                        int cost = (null == config) ? config.rmb_cost : 0;
                        stData.totalPayVal += (uint)cost;
                   }
               },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
               sql
               );
            #endregion

            #region 新用户付费率
            if (stData.newUserCnt != 0)
            {
                stData.newUserPayRate = stData.newUserPayCnt / (float)stData.newUserCnt * 100;
            }
            #endregion

            #region 新增付费用户
            sql = string.Format("SELECT last_update_time FROM receipt where server_id = {0} GROUP BY userID;"
                , server.serverID);
            DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        DateTime firstTime = reader.GetDateTime(0);
                        if (firstTime >= date_start
                            && firstTime <= date_end)
                        {
                            stData.newPayUserCnt += 1;
                        }
                    }
                },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);
            #endregion

            #region 活跃付费率
            if (stData.activeUserCnt != 0)
            {
                stData.activePayRate = stData.totalPayCnt / (float)stData.activeUserCnt * 100;
            }
            #endregion

            #region ARPPU
            if (stData.totalPayCnt != 0)
            {
                stData.arppu = stData.totalPayVal / (float)stData.totalPayCnt;
            }
            #endregion

            #region ARPU
            if (stData.activeUserCnt != 0)
            {
                stData.arpu = stData.totalPayVal / (float)stData.activeUserCnt;
            }
            #endregion

            listTable.Add(stData);
            return true;
        }

        [WebMethod(EnableSession = true)]
        public static string QuerySevenData(string channel_num, int week_num)
        {
            List<STSevenData> sdList = new List<STSevenData>();


            for (int i = 0; i < gmt.Server.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServerAt(i);
                if (server != null)
                {
                    ActivityOpenTime.STDateInfo stInfo;
                    ActivityOpenTime.GetActivityOpenTime(server, out stInfo);
                    //2017-09-15 23:59:59
                    DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(stInfo.openDate);
                    string start_time = dt.AddDays(7 * week_num - 7).ToString("yyyy-MM-dd 00:00:00");
                    string end_time = dt.AddDays(7 * week_num - 1).ToString("yyyy-MM-dd 23:59:59");
                    GetSevenDataFromDB(server, channel_num, start_time, end_time, ref sdList);
                }
            }

            return JsonConvert.SerializeObject(sdList);
        }

        public static void GetSevenDataFromDB(gmt.Server server, string channel_id, string start_time, string end_time, ref List<STSevenData> sdList)
        {
            string channel_condi = "= " + channel_id;
            if (channel_id == "0" || channel_id == "" || channel_id == TableManager.GetGMTText(699))
            {
                channel_condi = "<> " + channel_id;
            }

            DateTime st = Convert.ToDateTime(start_time);
            DateTime et = Convert.ToDateTime(end_time);

            STSevenData sd = new STSevenData();
            sd.serverName = server.Name;
            int idx = 0;
            for (DateTime dt = st; dt <= et; dt = dt.AddDays(1))
            {
                // 今日活跃
                DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        sd.activeUserCnt[idx] = reader.GetInt32(0);
                    }
                },
                   server.LogDatabaseAddress,
                   server.LogDatabasePort,
                   server.LogDatabaseCharSet,
                   server.LogDatabase,
                   server.LogDatabaseUserId,
                   server.LogDatabasePassword,
                   "SELECT COUNT(DISTINCT cyuid) FROM login_{0} WHERE (cyuid <> 'null' OR cyuid IS NOT NULL) and channelid {1};"
                   , dt.ToString("yyyy_MM_dd")
                   , channel_condi);

                // 总充值额
                DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        sd.totalPayVal[idx] = reader.GetInt32(0);
                    }
                },
                   server.LogDatabaseAddress,
                   server.LogDatabasePort,
                   server.LogDatabaseCharSet,
                   server.LogDatabase,
                   server.LogDatabaseUserId,
                   server.LogDatabasePassword,
                   @"SELECT SUM(rmb_1) + SUM(rmb_2) + SUM(rmb_3) FROM (SELECT uid, IF(typeid=21,168,0)'rmb_1', IF(typeid=15,25,0)'rmb_2', IF(typeid=13,quantity/10,0)'rmb_3' FROM economic_{0} WHERE causeid = 50 and channel_id {1})t"
                   , dt.ToString("yyyy_MM_dd")
                   , channel_condi);

                // 总付费数
                DatabaseAssistant.Execute(reader =>
                   {
                       if (reader.Read())
                       {
                           sd.totalPayCnt[idx] = reader.GetInt32(0);
                       }
                   },
                   server.LogDatabaseAddress,
                   server.LogDatabasePort,
                   server.LogDatabaseCharSet,
                   server.LogDatabase,
                   server.LogDatabaseUserId,
                   server.LogDatabasePassword,
                   @"SELECT COUNT(DISTINCT uid) FROM economic_{0} WHERE causeid = 50 and channel_id {1}"
                   , dt.ToString("yyyy_MM_dd")
                   , channel_condi
                   );

                idx++;
            }

            sdList.Add(sd);
        }

    }
}