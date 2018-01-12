using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace gmt
{
    class STQueryInfo
    {
        public int server_id;
        public int query_type;
        public int playerId;
        public DateTime start_time;
        public DateTime end_time;
        public int resTotal;
    }

    public partial class playerHistroy : AGmPage
    {
        #region 事件名称

        /// <summary>
        /// GMTDescConfig，10000~19999
        /// </summary>
        private static List<string> causeNameList = new List<string>();

        //private static string[] causeName = {
        //                        "GM",//0
        //                        "创建角色",//1 
        //                        "普通副本开始",//2 
        //                        "普通副本结束",//3 
        //                        "精英副本开始",//4 
        //                        "精英副本结束",//5 
        //                        "金币本, 经验本, 宠物本, 守卫本结束",//6 
        //                        "英雄升阶",//7 
        //                        "英雄技能相关",//8 
        //                        "英雄升级",//9 
        //                        "砸金蛋",//10 
        //                        "饰品重生",//11 
        //                        "物品合成(旧的专属升级道具和进阶道具合成)",//12 
        //                        "背包整理",//13 
        //                        "物品出售",//14 
        //                        "物品使用",//15 
        //                        "英雄合成",//16 
        //                        "副本扫荡",//17 
        //                        "副本重置(精英本和专属本)",//18 
        //                        "邮件相关",//19 
        //                        "商店重置",//20 
        //                        "商店购买",//21 
        //                        "无尽挑战鼓舞和重生",//22 
        //                        "八阵图相关(无重置，宝箱)",//23 
        //                        "八阵图章节奖励",//24 
        //                        "八阵图扫荡",//25 
        //                        "八阵图重置",//26 
        //                        "百战千军重置",//27 
        //                        "押镖刷新",//28 
        //                        "押镖相关",//29 
        //                        "劫镖相关",//30 
        //                        "百战千军相关(包含一键通关)",//31 
        //                        "帮派副本",//32 
        //                        "精力值获取(免费)",//33 
        //                        "精力值获取(元宝购买)",//34 
        //                        "金币获取(元宝购买)",//35 
        //                        "技能点获取(没了)",//36 
        //                        "守卫剑阁相关(没了)",//37 
        //                        "诸侯割据抽奖相关 (没了) ",//38 
        //                        "活动抽奖相关 (没有）",//39 
        //                        "幸运占星",//40 
        //                        "觉醒占星(无用)",//41 
        //                        "武将占星",//42 
        //                        "突破宝箱相关",//43 
        //                        "团队等级升级",//44 
        //                        "竞技场相关 (刷新)",//45 
        //                        "运营活动相关",//46 
        //                        "任务相关",//47 
        //                        "评星奖励",//48 
        //                        "竞技场名次奖励",//49 
        //                        "rmb商店",//50 
        //                        "小月卡相关",//51 
        //                        "大月卡相关",//52 
        //                        "首充相关",//53 
        //                        "理财计划相关",//54 
        //                        "运营活动 周任务 相关",//55 
        //                        "运营活动 vip 相关",//56 
        //                        "元宝充值",//57 
        //                        "宠物晶石重生",//58 
        //                        "帮派创建",//59 
        //                        "帮派创建失败",//60 
        //                        "帮派祈福",//61 
        //                        "帮派贡献(不包含帮派红包)",//62 
        //                        "排行榜buff相关",//63 
        //                        "帮派战争相关",//64 
        //                        "神兵兵魂洗练",//65 
        //                        "神兵兵魂铸魂相关(无用)",//66 
        //                        "英雄升阶材料装备",//67 
        //                        "神兵兵魂进化",//68 
        //                        "精力值获取(好友)",//69 
        //                        "精力值赠予(好友)",//70 
        //                        "诸侯割据 巡城",//71 
        //                        "诸侯割据 战斗相关",//72 
        //                        "诸侯割据 收成相关",//73 
        //                        "诸侯割据 抢夺相关",//74 
        //                        "诸侯割据 保护相关",//75 
        //                        "饰品精炼",//76 
        //                        "饰品附魔",//77 
        //                        "诸侯割据 救援",//78 
        //                        "英雄命格",//79 
        //                        "帮派技能",//80 
        //                        "炼魂相关",//81 
        //                        "英雄升星",//82 
        //                        "vip祭祀 (没了)",//83 
        //                        "神兵祭祀 (没了)",//84 
        //                        "星阵祈愿 (没了)",//85 
        //                        "英雄提品",//86 
        //                        "英雄传承",//87 
        //                        "神兵本开始 (没了）",//88 
        //                        "神兵本结束 (没了）",//89 
        //                        "千里单骑本开始",//90 
        //                        "千里单骑本结束",//91 
        //                        "神兵进化相关",//92 
        //                        "登出",//93 
        //                        "收集",//94 
        //                        "灵宠灵石升级",//95 
        //                        "灵宠灵石分解",//96 
        //                        "灵宠商店普通抽奖",//97 
        //                        "灵宠商店高级抽奖",//98 
        //                        "灵宠商店皇家抽奖(没了)",//99 
        //                        "金币本, 经验本, 守卫本, 宠物本扫荡开始",//100 
        //                        "称号激活",//101 
        //                        "炼魂升级",//102 
        //                        "离线奖励相关",//103 
        //                        "缘分激活相关",//104 
        //                        "八阵图神秘宝箱开启",//105 
        //                        "大富翁相关 (没了)",//106 
        //                        "大富翁副本跳过(没了)",//107 
        //                        "大富翁副本相关(没了)",//108 
        //                        "帮派任务刷新",//109 
        //                        "帮派任务相关(直接完成任务)",//110 
        //                        "帮派任务副本结束",//111 
        //                        "帮派祈福加成",//112 
        //                        "组队副本",//113 
        //                        "宠物晶石突破",//114 
        //                        "投掷鸡蛋",//115 
        //                        "赠送鲜花",//116 
        //                        "玩家补偿",//117 
        //                        "聚宝盆",//118 
        //                        "角色改名",//119 
        //                        "助战位升级",//120 
        //                        "技能重置",//121 
        //                        "复活",//122 
        //                        "神秘商店",//123 
        //                        "命格重置",//124 
        //                        "神兵降星",//125 
        //                        "竞技场重置",//126 
        //                        "道具转换",//127 
        //                        "武魂分解(出售)",//128 
        //                        "武将天命重置",//129	
        //                        "至尊卡相关",//130 
        //                        "红包",//131 
        //                        "7天充值返还",//132 
        //                        "坐骑碎片合成",//133 
        //                        "坐骑升级",//134 
        //                        "英雄品质重置",//124 

        //};
        #endregion

        #region 奖励类型名称

        /// <summary>
        /// GMTDescConfig，20000~20999
        /// </summary>
        private static Dictionary<int, string> typeNameDic = new Dictionary<int, string>()
        {
          {0,"经济类型"}, //0 
          {1,"物品"}, //1 
          {2,"英雄"}, //2 
          {3,"武魂"}, //3 
          {4,"装备"}, //4
          {5,"宠物"}, //5
          {6,"buff"}, //6
          {7,"专属装备"}, //7
          {8,"灵宝灵石"}, //8
          {9,"坐骑"}, //9
          {99,"邮件"},
        };
        #endregion

        #region 经济类型名称
        /// <summary>
        /// GMTDescConfig，21000~21999，下标为经济类型ID+21000
        /// </summary>
        public static Dictionary<int, string> economicName = new Dictionary<int, string>()
        {
            {0,"GM"},
            {1,"VIP"},
            {2,"铜钱"},
            {3,"元宝"},
            {4,"体力"},
            {5,"红包点"},
            {6,"晶石精粹"},
            {7,"活跃点"},
            {8,"兵魂精华"},
            {9,"荣誉点"},
            {13,"充值元宝"},
            {15,"超值月卡"},
            {17,"个人贡献"},
            {18,"勇气点"},
            {19,"声名"},
            {20,"王者荣耀"},
            {21,"至尊月卡"},
            {22,"水晶(限时折扣商店用)"},
            {23,"装备精华"},
            {24,"军功"},
            {25,"觉醒"},
            {26,"晶石碎片"},
            {27,"龙珠精华"},
            {28,"VIP经验"},
            {99,"运营活动 兑换道具数量(小)"},
            {100,"运营活动 兑换道具数量(大)"},
            {101,"诸侯割据特殊道具"},
            {102,"押劫镖特殊道具"},
        };
        #endregion

        #region buff名称

        /// <summary>
        /// GMTDescConfig，22000~22999
        /// </summary>
        private static List<string> buffNameList = new List<string>();

        //private static string[] buffName = { 
        //                                    "普通副本产出翻倍",//0
        //                                    "精英副本产出翻倍",//1
        //                                    "每日任务产出翻倍",//2
        //                                    "单次pvp奖励翻倍", //3
        //                                    "押镖奖励翻倍1",   //4
        //                                    "押镖奖励翻倍2",   //5
        //                                    "神秘商店",        //6
        //                                    "八阵图免费重置",  //7
        //                                    "演武场副本冷却",  //8
        //                                    "殊副本冷却",      //9
        //                                    "殊副本冷却",      //10
        //                                    "殊副本冷却",      //11
        //                                    "殊副本冷却",      //12
        //                                    "殊副本冷却",      //13
        //                                    "殊副本冷却",      //14
        //                                    "殊副本冷却",      //15
        //                                    "殊副本冷却",      //16
        //                                   };
        #endregion

        #region 英雄信息类型
        private static Dictionary<int, string> HeroInfoTypeDic = new Dictionary<int, string>()
        {
            {0, "武将升级"},
            {1, "武将提品"},
            {2, "武将进阶"},
            {3, "武将升星"},
            {4, "武将技能"},
            {5, "武将缘分"},
            {6, "武将命格"},
            {7, "武将神兵洗练"},
            {8, "武将神兵进化"},
            {9, "武将装备强化"},
            {10, "武将装备提品"},
            {11, "武将装备突破"},
            {12, "武将饰品 佩戴/卸下"},
            {13, "武将饰品精炼"},
            {14, "武将饰品附魔"},
            {15, "武将阵容更换"},
        };
        #endregion


        class PHServerData
        {
            public int id;
            public string name;
        }

        class ItemData
        {
            public string date;
            public uint uid;
            public string channel_id;
            public int team_level;
            public int item_type;
            public string item_type_name;
            public int item_id;
            public string item_name;
            public int causeid;
            public string cause_name;
            public int quantity;
            public int total;
            public int vip;
            public int action;
        }

        class HeroInfoData
        {
            public string date;
            public uint uid;
            public string channel_id;
            public int team_level;
            public int type;
            public string type_name;
            public int hero_id;
            public string hero_name;
            public int target_id;
            public string target_name;
            public int value_bf;
            public int value_af;
            public int zdl_bf;
            public int zdl_af;
            public int team_zdl_bf;
            public int team_zdl_af;
            public int vip;
        }

        private static Dictionary<int, string> itemNameDic = null;

        private static List<ItemData> itemList = new List<ItemData>();
        private static List<HeroInfoData> heroInfoList = new List<HeroInfoData>();

        private static STQueryInfo queryInfo = new STQueryInfo();

        public playerHistroy()
            : base(PrivilegeType.PlayerHistory)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                int idx = 0;

                #region 读取事件名称
                causeNameList.Clear();
                idx = 10000;
                while (idx < 20000)
                {
                    if (TableManager.GMTDescTable.ContainsKey(idx))
                    {
                        causeNameList.Add(TableManager.GetGMTText(idx));
                    }
                    else
                    {
                        break;
                    }
                    idx++;
                }
                #endregion

                #region 读取奖励类型名称
                {
                    int[] keyArr = typeNameDic.Keys.ToArray();
                    for (int i = 0; i < keyArr.Length; ++i)
                    {
                        if (TableManager.GMTDescTable.ContainsKey(20000 + keyArr[i]))
                        {
                            typeNameDic[keyArr[i]] = TableManager.GetGMTText(20000 + keyArr[i]);
                        }
                    }
                }
                #endregion

                #region 读取经济类型名称
                {
                    int[] keyArr = economicName.Keys.ToArray();
                    for (int i = 0; i < keyArr.Length; ++i)
                    {
                        if (TableManager.GMTDescTable.ContainsKey(21000 + keyArr[i]))
                        {
                            economicName[keyArr[i]] = TableManager.GetGMTText(21000 + keyArr[i]);
                        }
                    }
                }

                //economicName.Clear();
                //idx = 21000;
                //while (idx < 22000)
                //{
                //    if (TableManager.GMTDescTable.ContainsKey(idx))
                //    {
                //        economicName.Add(idx - 21000, TableManager.GetGMTText(idx));
                //    }
                //    idx++;
                //}
                #endregion

                #region 读取buff名称
                buffNameList.Clear();
                idx = 22000;
                while (idx < 23000)
                {
                    if (TableManager.GMTDescTable.ContainsKey(idx))
                    {
                        buffNameList.Add(TableManager.GetGMTText(idx));
                    }
                    else
                    {
                        break;
                    }
                    idx++;
                }
                #endregion

                #region 物品名称
                if (itemNameDic == null)
                {
                    itemNameDic = new Dictionary<int, string>();
                    // 英雄
                    try
                    {
                        List<mw.HeroBaseConfig> hbc = TableManager.Load<mw.HeroBaseConfig>();
                        for (int i = 0; i < hbc.Count; i++)
                        {
                            itemNameDic.Add(hbc[i].id, TextManager.GetText(hbc[i].name));
                        }

                        //物品
                        List<mw.ItemConfig> ic = TableManager.Load<mw.ItemConfig>();
                        for (int i = 0; i < ic.Count; i++)
                        {
                            if (itemNameDic.ContainsKey(ic[i].id))
                            {
                                itemNameDic[ic[i].id] = TextManager.GetText(ic[i].name);
                            }
                            else
                            {
                                itemNameDic.Add(ic[i].id, TextManager.GetText(ic[i].name));
                            }
                        }
                    }
                    catch (Exception ec)
                    {
                    }
                }
                #endregion
            }
        }

        [WebMethod(EnableSession = true)]
        public static string BtnQuery(int server_id
                                   , int query_type
                                   , int playerId
                                   , DateTime start_time
                                   , DateTime end_time
            )
        {
            queryInfo.server_id = server_id;
            queryInfo.query_type = query_type;
            queryInfo.playerId = playerId;
            queryInfo.start_time = start_time;
            queryInfo.end_time = end_time;
            queryInfo.resTotal = -1;

            return "{\"error\":0}";
        }

        [WebMethod(EnableSession = true)]
        public static string QueryPlayerHistoryData(int limit
                                                    , int offset
                                                    , string sort
                                                    , string order
                                                    , string search
            )
        {
            itemList.Clear();
            heroInfoList.Clear();
            int total = 0;

            string sqlCondi = "";

            if (queryInfo.start_time != new DateTime(0))
            {
                sqlCondi += string.Format("and date >= '{0}'", queryInfo.start_time.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            if (queryInfo.end_time != new DateTime(0))
            {
                sqlCondi += string.Format(" and date <= '{0}'", queryInfo.end_time.ToString("yyyy/MM/dd HH:mm:ss"));
            }

            if (queryInfo.playerId != 0)
            {
                sqlCondi += " and uid = " + queryInfo.playerId;
            }

            if (!string.IsNullOrEmpty(search))
            {
                sqlCondi += playerHistroy.GetFilterStringByJson(search);
            }

            string dataStr = "";
            switch (queryInfo.query_type)
            {
                case 0:
                    total = QueryEconomicData(queryInfo.server_id, limit, offset, sqlCondi, sort, order);
                    dataStr = JsonConvert.SerializeObject(itemList);
                    break;

                case 1:
                    total = QueryItemData(queryInfo.server_id, limit, offset, sqlCondi, sort, order);
                    dataStr = JsonConvert.SerializeObject(itemList);
                    break;
                case 2:
                    total = QueryHeroData(queryInfo.server_id, limit, offset, sqlCondi, sort, order);
                    dataStr = JsonConvert.SerializeObject(heroInfoList);
                    break;
            }

            return string.Format("{{\"error\":0,\"data\":{{\"total\":{0},\"rows\":{1} }} }}", total, dataStr);
        }

        [WebMethod(EnableSession = true)]
        public static string GetServerList()
        {
            List<PHServerData> sList = new List<PHServerData>();

            for (int i = 0; i < gmt.Server.Count; i++)
            {
                PHServerData ph = new PHServerData();
                ph.id = i;
                gmt.Server server = gmt.Server.GetServerAt(i);
                ph.name = server.Name;
                sList.Add(ph);
            }

            if (sList.Count > 0)
                return JsonConvert.SerializeObject(sList);
            return "[{\"error\":1}]";
        }

        /// <summary>
        /// 查询经济流水
        /// </summary>
        /// <param name="server_id"></param>
        /// <param name="player_id"></param>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="sort"></param>
        /// <param name="order"></param>
        /// <returns>返回总数</returns>
        private static int QueryEconomicData(int server_id
                                                , int limit
                                                , int offset
                                                , string search
                                                , string sort
                                                , string order
            )
        {
            gmt.Server server = gmt.Server.GetServerAt(server_id);

            #region 查询总数
            //if (-1 == queryInfo.resTotal)
            //{
            string sqlCnt = "select count(uid) from economic_ where 1 " + search;

            DatabaseAssistant.Execute(reader =>
            {
                if (reader.Read())
                {
                    queryInfo.resTotal = reader.GetInt32(0);
                }
            },
                server.LogDatabaseAddress,
                server.LogDatabasePort,
                server.LogDatabaseCharSet,
                server.LogDatabase,
                server.LogDatabaseUserId,
                server.LogDatabasePassword,
                sqlCnt
            );
            //}
            #endregion

            if (queryInfo.resTotal != 0)
            {
                string orderSql = "";
                #region 排序
                if (!string.IsNullOrEmpty(sort))
                {
                    if (sort == "item_type_name")
                    {
                        sort = "";
                    }
                    else if (sort == "item_name")
                    {
                        sort = "typeid";
                    }
                    else if (sort == "cause_name")
                    {
                        sort = "causeid";
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        orderSql = string.Format("order by {0} {1}", sort, order);
                    }
                }
                #endregion

                StringBuilder sbIds = new StringBuilder();
                #region 查询id集合
                string sql = string.Format("SELECT id FROM economic_ where 1 {0} {1} limit {2}, {3}"
                                            , search
                                            , orderSql
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
                    server.LogDatabaseAddress,
                    server.LogDatabasePort,
                    server.LogDatabaseCharSet,
                    server.LogDatabase,
                    server.LogDatabaseUserId,
                    server.LogDatabasePassword,
                    sql
                );
                #endregion

                #region 根据id集合查询结果
                sql = string.Format("select date, uid, cyuid, channel_id, team_level, causeid, quantity, total, typeid, vip, action from economic_ where id in ({0}) {1}", sbIds.ToString(), orderSql);
                DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        try
                        {
                            ItemData item = new ItemData();
                            item.date = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                            item.uid = reader.GetUInt32(1);
                            item.channel_id = reader.GetString(3);
                            item.team_level = reader.GetInt32(4);
                            item.causeid = reader.GetInt32(5);
                            // 除掉登出事件
                            if (item.causeid == 93)
                            {
                                continue;
                            }

                            if (item.causeid < causeNameList.Count)
                            {
                                item.cause_name = causeNameList[item.causeid];
                            }
                            else
                            {
                                item.cause_name = string.Format("Unknown(id: {0})", item.causeid);
                            }

                            item.quantity = reader.GetInt32(6);
                            item.item_type = 0;

                            if (typeNameDic.ContainsKey(item.item_type))
                            {
                                item.item_type_name = typeNameDic[item.item_type];
                            }
                            else
                            {
                                item.item_type_name = string.Format("Unknown(type: {0})", item.item_type);
                            }

                            item.total = reader.GetInt32(7);
                            item.item_id = reader.GetInt32(8);
                            if (economicName.ContainsKey(item.item_id))
                            {
                                item.item_name = economicName[item.item_id];
                            }
                            else
                            {
                                item.item_name = string.Format("Unknown(id:{0})", item.item_id);
                            }

                            if (item.item_id == 15
                                || item.item_id == 21)
                            {
                                item.quantity = 1;
                                item.total = 1;
                            }

                            item.vip = reader.GetInt32(9);
                            item.action = reader.GetInt32(10);
                            itemList.Add(item);
                        }
                        catch (Exception e)
                        {
                            Log.AddLog(e.ToString());
                        }
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
                #endregion
            }
            return queryInfo.resTotal;
        }

        private static int QueryItemData(int server_id
                                         , int limit
                                         , int offset
                                         , string search
                                         , string sort
                                         , string order
            )
        {
            gmt.Server server = gmt.Server.GetServerAt(server_id);

            #region 查询总数
            string sqlCnt = "select count(uid) from item_ where 1 " + search;

            DatabaseAssistant.Execute(reader =>
            {
                if (reader.Read())
                {
                    queryInfo.resTotal = reader.GetInt32(0);
                }
            },
                server.LogDatabaseAddress,
                server.LogDatabasePort,
                server.LogDatabaseCharSet,
                server.LogDatabase,
                server.LogDatabaseUserId,
                server.LogDatabasePassword,
                sqlCnt
            );
            #endregion

            if (queryInfo.resTotal != 0)
            {
                string orderSql = "";
                #region 排序
                if (!string.IsNullOrEmpty(sort))
                {
                    if (sort == "item_type_name")
                    {
                        sort = "item_type";
                    }
                    else if (sort == "item_name")
                    {
                        sort = "item_id";
                    }
                    else if (sort == "cause_name")
                    {
                        sort = "causeid";
                    }
                    else if (sort == "total")
                    {
                        sort = "";
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        orderSql = string.Format("order by {0} {1}", sort, order);
                    }
                }
                #endregion

                #region 查找id集合
                string sql = string.Format("SELECT id FROM item_ where 1 {0} {1} limit {2}, {3}"
                                            , search
                                            , orderSql
                                            , offset
                                            , limit
                                            );

                StringBuilder sbIds = new StringBuilder();
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
                #endregion

                #region 根据id集合查找数据
                sql = string.Format("select date, uid, cyuid, channel_id, team_level, item_type, item_id, causeid, quantity, vip, action from item_ where id in ({0}) {1}", sbIds.ToString(), orderSql);
                DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        try
                        {
                            ItemData item = new ItemData();
                            item.date = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                            item.uid = reader.GetUInt32(1);
                            item.channel_id = reader.GetString(3);
                            item.team_level = reader.GetInt32(4);
                            item.item_type = reader.GetInt32(5);

                            if (typeNameDic.ContainsKey(item.item_type))
                            {
                                item.item_type_name = typeNameDic[item.item_type];
                            }
                            else
                            {
                                item.item_type_name = string.Format("Unknown(type: {0})", item.item_type);
                            }

                            item.item_id = reader.GetInt32(6);

                            if (itemNameDic.ContainsKey(item.item_id))
                            {
                                item.item_name = itemNameDic[item.item_id];
                            }
                            else
                            {
                                item.item_name = string.Format("Unknown(id: {0})", item.item_id);
                            }

                            item.causeid = reader.GetInt32(7);

                            item.total = -1;

                            if (item.causeid < causeNameList.Count)
                            {
                                item.cause_name = causeNameList[item.causeid];
                            }
                            else
                            {
                                item.cause_name = string.Format("Unknown(id: {0})", item.causeid);
                            }

                            item.quantity = reader.GetInt32(8);
                            item.vip = reader.GetInt32(9);
                            item.action = reader.GetInt32(10);
                            itemList.Add(item);
                        }
                        catch (Exception e)
                        {
                            Log.AddLog(e.ToString());
                        }
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
            }
            return queryInfo.resTotal;
        }

        private static int QueryHeroData(int server_id
                                         , int limit
                                         , int offset
                                         , string search
                                         , string sort
                                         , string order
            )
        {
            gmt.Server server = gmt.Server.GetServerAt(server_id);

            #region 查询总数
            string sqlCnt = "select count(uid) from heroinfo_ where 1 " + search;

            DatabaseAssistant.Execute(reader =>
            {
                if (reader.Read())
                {
                    queryInfo.resTotal = reader.GetInt32(0);
                }
            },
                server.LogDatabaseAddress,
                server.LogDatabasePort,
                server.LogDatabaseCharSet,
                server.LogDatabase,
                server.LogDatabaseUserId,
                server.LogDatabasePassword,
                sqlCnt
            );
            #endregion

            if (queryInfo.resTotal != 0)
            {
                string orderSql = "";
                #region 排序
                if (!string.IsNullOrEmpty(sort))
                {
                    if (!string.IsNullOrEmpty(sort))
                    {
                        if (sort == "type_name")
                        {
                            sort = "type";
                        }
                        orderSql = string.Format("order by {0} {1}", sort, order);
                    }
                }
                #endregion

                #region 查找id集合
                string sql = string.Format("SELECT id FROM heroinfo_ where 1 {0} {1} limit {2}, {3}"
                                            , search
                                            , orderSql
                                            , offset
                                            , limit
                                            );

                StringBuilder sbIds = new StringBuilder();
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
                #endregion

                #region 根据id集合查找数据
                sql = string.Format("select date, uid, cyuid, channel_id, team_level, type, hero_id, target_id, value_bf, value_af, zdl_bf, zdl_af, team_zdl_bf, team_zdl_af, vip from heroinfo_ where id in ({0}) {1}", sbIds.ToString(), orderSql);
                DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        try
                        {
                            HeroInfoData hero = new HeroInfoData();
                            hero.date = reader.GetDateTime(0).ToString("yyyy-MM-dd HH:mm:ss");
                            hero.uid = reader.GetUInt32(1);
                            hero.channel_id = reader.GetString(3);
                            hero.team_level = reader.GetInt32(4);
                            hero.type = reader.GetInt32(5);

                            if (-1 != hero.type)
                            {
                                if (HeroInfoTypeDic.ContainsKey(hero.type))
                                    hero.type_name = HeroInfoTypeDic[hero.type];
                                else
                                    hero.type_name = string.Format("Unknown(type: {0})", hero.type);
                            }

                            hero.hero_id = reader.GetInt32(6);

                            if (-1 != hero.hero_id)
                            {
                                if (itemNameDic.ContainsKey(hero.hero_id))
                                {
                                    hero.hero_name = itemNameDic[hero.hero_id];
                                }
                                else
                                {
                                    hero.hero_name = string.Format("Unknown(id: {0})", hero.hero_id);
                                }
                            }

                            hero.target_id = reader.GetInt32(7);
                            //if (itemNameDic.ContainsKey(hero.target_id))
                            //{
                            //    hero.target_name = itemNameDic[hero.target_id];
                            //}
                            //else
                            //{
                            //    hero.target_name = string.Format("Unknown(id: {0})", hero.target_id);
                            //}

                            hero.value_bf = reader.GetInt32(8);
                            hero.value_af = reader.GetInt32(9);

                            hero.zdl_bf = reader.GetInt32(10);
                            hero.zdl_af = reader.GetInt32(11);

                            hero.team_zdl_bf = reader.GetInt32(12);
                            hero.team_zdl_af = reader.GetInt32(13);

                            hero.vip = reader.GetInt32(14);

                            heroInfoList.Add(hero);
                        }
                        catch (Exception e)
                        {
                            Log.AddLog(e.ToString());
                        }
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
            }

            return queryInfo.resTotal;
        }

        private static string GetFilterStringByJson(string json)
        {
            string filter = "";
            Dictionary<string, string> dicFilter = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            foreach (var pair in dicFilter)
            {
                string key = pair.Key;
                string val = pair.Value;

                if (pair.Key == "action")
                {
                    if (pair.Value == "소모")
                    {
                        val = "-1";
                    }
                    else if (pair.Value == "획득")
                    {
                        val = "1";
                    }
                }
                else if (pair.Key == "cause_name")
                {
                    key = "causeid";
                    val = GetCauseIDByName(pair.Value).ToString();
                }
                else if (pair.Key == "item_type_name")
                {
                    if (queryInfo.query_type != 0)
                    {
                        key = "item_type";
                        val = GetRwdTypeIDByName(pair.Value).ToString();
                    }
                }
                else if (pair.Key == "item_name")
                {
                    if (queryInfo.query_type == 0)
                    {
                        key = "typeid";
                        val = GetEconomicIDByName(pair.Value).ToString();
                    }
                    else
                    {
                        key = "item_id";
                        val = GetItemIDByName(pair.Value).ToString();
                    }
                }

                filter += " and " + key + " = " + val;
            }

            return filter;
        }

        private static int GetCauseIDByName(string name)
        {
            int i = 0;
            foreach (var value in causeNameList)
            {
                if (name == value)
                {
                    return i;
                }
                i++;
            }
            return -1;
        }

        private static int GetRwdTypeIDByName(string name)
        {
            foreach (var pair in typeNameDic)
            {
                if (pair.Value == name)
                    return pair.Key;
            }
            return -1;
        }

        private static int GetBuffIDByName(string name)
        {
            int i = 0;
            foreach (var value in buffNameList)
            {
                if (name == value)
                    return i;
                i++;
            }
            return -1;
        }

        private static int GetEconomicIDByName(string name)
        {
            foreach (var pair in economicName)
            {
                if (pair.Value == name)
                    return pair.Key;
            }
            return -1;
        }

        private static int GetItemIDByName(string name)
        {
            if (itemNameDic != null)
            {
                foreach (var pair in itemNameDic)
                {
                    if (pair.Value == name)
                        return pair.Key;
                }
            }
            return -1;
        }
    }
}