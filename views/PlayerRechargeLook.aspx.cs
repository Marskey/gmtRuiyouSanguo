using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;

namespace gmt
{
    public partial class PlayerRechargeLook : AGmPage
    {
        public static string[] m_rawType = { "None", "IOS", "Google", "OneStore" };

        private static STQueryInfo queryInfo = new STQueryInfo();
        //class STServerID
        //{
        //    public byte byRegion;
        //    public byte byGroup;
        //    public byte byType;
        //    public byte byIndex;
        //}

        public PlayerRechargeLook()
            : base(PrivilegeType.PlayerRechargeLook)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {

            if (!this.IsPostBack)
            {
            }

        }
        [WebMethod(EnableSession = true)]
        public static string BtnQuery(int playerId, DateTime start_time, DateTime end_time)
        {
            queryInfo.playerId = playerId;
            queryInfo.start_time = start_time;
            queryInfo.end_time = end_time;

            return "{\"error\":0}";
        }

        [WebMethod(EnableSession = true)]
        public static string UpdatePlayerRechargeTableData(int limit
            , int offset
            , string sort
            , string order
            , string filter
            )
        {
            #region 数据库查询语句拼接
            string sqlCondi = "";

            if (queryInfo.start_time != new DateTime(0))
            {
                sqlCondi = string.Format(" last_update_time >= '{0}'", queryInfo.start_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"));
            }

            if (queryInfo.end_time != new DateTime(0))
            {
                sqlCondi = string.Format(" and last_update_time <= '{0}'", queryInfo.end_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss"));
            }

            if (queryInfo.playerId != 0)
            {
                sqlCondi += " and userID = " + queryInfo.playerId;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                sqlCondi += GetFilterStringByJson(filter);
            }
            #endregion

            int totalRows = 0;
            List<PlayerRechargeInfo> rechargeList = new List<PlayerRechargeInfo>();

            #region 查询目标总数
            gmt.Server server = gmt.Server.GetServerAt(0);
            string sql = string.Format("SELECT COUNT(state) FROM receipt where 1 {0}", sqlCondi);
            DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        totalRows = reader.GetInt32(0);
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

            if (totalRows != 0)
            {
                string sql_order = "";
                #region 排序
                if (!string.IsNullOrEmpty(sort))
                {
                    sql_order = string.Format("ORDER BY {0} {1}", sort, order);
                }

                #endregion

                StringBuilder sbIds = new StringBuilder();
                #region 查询id集合
                sql = string.Format("select id from receipt where 1 {0} {1} limit {2}, {3}"
                    , sqlCondi
                    , sql_order
                    , offset
                    , limit
                    );

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
                    server.BillDatabaseAddress,
                    server.BillDatabasePort,
                    server.BillDatabaseCharSet,
                    server.BillDatabase,
                    server.BillDatabaseUserId,
                    server.BillDatabasePassword,
                    sql
                );

                #endregion

                #region 根据id集合查询结果

                sql = string.Format("SELECT orderID, state, plat_order_id, raw_type, last_update_time FROM receipt where id in ({0}) {1}"
                    , sbIds.ToString()
                    , sql_order);

                DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        PlayerRechargeInfo prInfo = new PlayerRechargeInfo();

                        string orderInfo = reader.GetString(0);
                        string[] orderArray = orderInfo.Split(',');

                        int.TryParse(orderArray[0], out prInfo.userID);
                        prInfo.cyUid = orderArray[2];
                        int.TryParse(orderArray[3], out prInfo.serverId);
                        prInfo.serverIdStr = IntToBytes(prInfo.serverId);

                        mw.RmbShopConfig config = null;
                        string index = string.Format("{0}-{1}", orderArray[1], orderArray[4]);
                        TableManager.RmbShopTable.TryGetValue(index, out config);
                        if (null == config)
                        {
                            prInfo.goodsName = config.goods_name;
                            prInfo.goodsCost = config.rmb_cost;
                        }
                        else
                        {
                            prInfo.goodsName = TableManager.GetGMTText(719) + "(" + orderArray[4] + ")";
                            prInfo.goodsCost = 0;
                        }

                        int.TryParse(orderArray[5], out prInfo.goodsNum);

                        int nState = reader.GetInt32(1);
                        if (ReceiptStateTable.ContainsKey((EReceiptState)nState))
                        {
                            prInfo.state = ReceiptStateTable[(EReceiptState)nState];
                        }
                        else
                        {
                            prInfo.state = TableManager.GetGMTText(719) + "(" + nState + ")";
                        }

                        prInfo.plat_order_id = reader.IsDBNull(2) ? "" : reader.GetString(2);
                        prInfo.raw_type = m_rawType[reader.GetInt16(3)];
                        prInfo.last_update_time = reader.GetDateTime(4).ToString("yyyy-MM-dd HH:mm:ss");

                        rechargeList.Add(prInfo);
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

            }

            string rechargeData = JsonConvert.SerializeObject(rechargeList);
            return string.Format(@"{{""error"":0, ""data"": {{""total"":{0}, ""rows"":{1} }} }}", totalRows, rechargeData);
        }

        public static string IntToBytes(int serverId)
        {
            string idStr = "";
            byte[] idBytes = BitConverter.GetBytes(serverId);
            for (int i = 0; i < idBytes.Length; i++)
            {
                idStr += (i == 0 ? "" : "-") + idBytes[i];
            }
            return idStr;
        }

        [WebMethod(EnableSession = true)]
        public static string QueryTotalCost(string filter)
        {
            string sqlCondi = "";

            if (queryInfo.start_time == null || queryInfo.end_time == null)
            {
                sqlCondi = string.Format(" last_update_time between '{0}' and '{1}'"
                                             , queryInfo.start_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss")
                                             , queryInfo.end_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss")
                                             );
            }

            if (queryInfo.playerId != 0)
            {
                sqlCondi += " and uid = " + queryInfo.playerId;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                sqlCondi += GetFilterStringByJson(filter);
            }

            string sql = string.Format("SELECT orderID FROM receipt where 1 {0}", sqlCondi);

            UInt64 totalCost = 0;
            gmt.Server server = gmt.Server.GetServerAt(0);
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
                        totalCost += (uint)cost;
                    }
                },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);

            return string.Format(@"{{ ""totalCost"": {0} }}", totalCost);
        }

        [WebMethod(EnableSession = true)]
        public static string QueryPlayerCnt(string filter)
        {
            string sqlCondi = "";

            if (queryInfo.start_time == null || queryInfo.end_time == null)
            {
                sqlCondi = string.Format(" last_update_time between '{0}' and '{1}'"
                                             , queryInfo.start_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss")
                                             , queryInfo.end_time.ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss")
                                             );
            }

            if (queryInfo.playerId != 0)
            {
                sqlCondi += " and uid = " + queryInfo.playerId;
            }

            if (!string.IsNullOrEmpty(filter))
            {
                sqlCondi += GetFilterStringByJson(filter);
            }

            string sql = string.Format("SELECT COUNT(DISTINCT(userID)) FROM receipt where 1 {0}", sqlCondi);

            int playerCnt = 0;
            gmt.Server server = gmt.Server.GetServerAt(0);
            DatabaseAssistant.Execute(reader =>
            {
                if (reader.Read())
                {
                    playerCnt = reader.GetInt32(0);
                }
            },
                server.BillDatabaseAddress,
                server.BillDatabasePort,
                server.BillDatabaseCharSet,
                server.BillDatabase,
                server.BillDatabaseUserId,
                server.BillDatabasePassword,
                sql);

            return string.Format(@"{{ ""playerCnt"": {0} }}", playerCnt);
        }

        public static Dictionary<EReceiptState, string> ReceiptStateTable = new Dictionary<EReceiptState, string>()
        {
            {EReceiptState.e_receipt_state_null,"NULL"},
            {EReceiptState.e_receipt_state_success,"성공"},
            {EReceiptState.e_receipt_state_new,"E_RECEIPT_STATE_NEW"},
            {EReceiptState.e_receipt_state_raw_new,"아이템 지급중"},
            {EReceiptState.e_receipt_state_raw_post_faile,"영수증 검증 요청 시간초과"},
            {EReceiptState.e_receipt_state_check_faile,"영수증검증 실패"},
            {EReceiptState.e_receipt_state_raw_order_duplicate,"E_RECEIPT_STATE_RAW_ORDER_DUPLICATE"},
            {EReceiptState.e_receipt_state_gs,"아이템 지급중"},
            {EReceiptState.e_receipt_state_gs_unknown,"아이템 지급 실패(서버미오픈)"},
            {EReceiptState.e_receipt_state_gs_success,"E_RECEIPT_STATE_GS_SUCCESS"},
            {EReceiptState.e_receipt_state_gs_faile,"E_RECEIPT_STATE_GS_FAILE"},
            {EReceiptState.e_receipt_state_upd_faile,"E_RECEIPT_STATE_UPD_FAILE"},
            {EReceiptState.e_receipt_state_max,"MAX"},
        };

        private static string GetFilterStringByJson(string json)
        {
            string filter = "";
            Dictionary<string, string> dicFilter = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (var pair in dicFilter)
            {
                string key = pair.Key;
                string val = pair.Value;

                if (pair.Key == "raw_type")
                {
                    val = GetRawTypeByName(pair.Value).ToString();
                }
                else if (pair.Key == "serverIdStr")
                {
                    key = "server_id";
                    val = GetServerIDByName(pair.Value).ToString();
                }
                else if (pair.Key == "state")
                {
                    val = GetStateByName(pair.Value).ToString();
                }

                filter += " and " + key + " = '" + val + "'";
            }
            return filter;
        }

        private static int GetRawTypeByName(string name)
        {
            for (int i = 0; i < m_rawType.Length; ++i)
            {
                if (m_rawType[i] == name)
                    return i;
            }
            return -1;
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

        private static int GetStateByName(string name)
        {
            foreach (var pair in ReceiptStateTable)
            {
                if (pair.Value == name)
                {
                    return (int)pair.Key;
                }
            }
            return -1;
        }
    }


    /// <summary>
    /// 订单数据信息
    /// </summary>
    public class PlayerRechargeInfo
    {
        public PlayerRechargeInfo()
        {
            userID = 0;
            raw_type = "";
            cyUid = "";
            serverId = 0;
            serverIdStr = "";
            goodsName = "";
            goodsNum = 0;
            goodsCost = 0;
            last_update_time = "";
            state = "";
            plat_order_id = "";
        }
        public int userID;
        public string raw_type;
        public string cyUid;
        public int serverId;
        public string serverIdStr;
        public string goodsName;
        public int goodsNum;
        public int goodsCost;
        public string last_update_time;
        public string state;
        public string plat_order_id;
    }

    /// <summary>
    /// 订单状态
    /// </summary>
    public enum EReceiptState
    {
        e_receipt_state_null = -1,
        e_receipt_state_success,
        e_receipt_state_new,
        e_receipt_state_raw_new,
        e_receipt_state_raw_post_faile,
        e_receipt_state_check_faile,
        e_receipt_state_raw_order_duplicate,
        e_receipt_state_gs,
        e_receipt_state_gs_unknown,
        e_receipt_state_gs_success,
        e_receipt_state_gs_faile,
        e_receipt_state_upd_faile,
        e_receipt_state_max,
    };

}