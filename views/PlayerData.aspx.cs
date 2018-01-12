using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace gmt.views
{
    public partial class PlayerData : AGmPage
    {
        public PlayerData()
            : base(PrivilegeType.Modify)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                page = this;
            }
        }

        [WebMethod(EnableSession = true)]
        public static string QueryPlayerData(int server_id)
        {
            gmt.Server server = gmt.Server.GetServerAt(server_id);
            GetPlayerDataFromDB(server);
            System.Data.DataTable dt = DicToTable();
            if (ExportExcel.DoExport(dt, server.Name))
            {
                return "[{\"error\":0}]";
            }
            else
            {
                return "[{\"error\":1}]";
            }
        }

        /// <summary>
        /// 获取玩家数据
        /// </summary>
        private static void GetPlayerDataFromDB(gmt.Server server)
        {
            DatabaseAssistant.Execute(reader =>
                    {
                        while (reader.Read())
                        {
                            PlayerDataInfo pdi = new PlayerDataInfo();

                            PlayerDataBaseInfo bi = new PlayerDataBaseInfo();
                            bi.uid = reader.GetUInt32(0);

                            if (!reader.IsDBNull(1))
                            {
                                byte[] buffer = new byte[reader.GetBytes(1, 0, null, 0, int.MaxValue)];
                                reader.GetBytes(1, 0, buffer, 0, buffer.Length);
                                using (MemoryStream stream = new MemoryStream(buffer))
                                {
                                    mw.Role3rdData roledata = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.Role3rdData)) as mw.Role3rdData;
                                    bi.team_level = roledata.level;
                                    bi.soulex_level = roledata.soulex_level;

                                    for (int i = 0; i < 6; i++)
                                    {
                                        if (i == roledata.detailInfo.heroInfos.Count)
                                        {
                                            break;
                                        }

                                        #region 英雄战力信息
                                        PlayerDataHeroInfo hi = new PlayerDataHeroInfo();

                                        hi.hero_id = roledata.detailInfo.heroInfos[i].heroInfo.index;
                                        hi.hero_color = roledata.detailInfo.heroInfos[i].heroInfo.color;
                                        hi.hero_star = roledata.detailInfo.heroInfos[i].heroInfo.star;

                                        if (roledata.detailInfo.heroInfos[i].heroInfo.equipInfos.Count > 0)
                                        {
                                            hi.hero_equip_min_level = roledata.detailInfo.heroInfos[i].heroInfo.equipInfos[0].levelex;
                                            for (int j = 0; j < roledata.detailInfo.heroInfos[i].heroInfo.equipInfos.Count; j++)
                                            {
                                                if (roledata.detailInfo.heroInfos[i].heroInfo.equipInfos[j].levelex < hi.hero_equip_min_level)
                                                {
                                                    hi.hero_equip_min_level = roledata.detailInfo.heroInfos[i].heroInfo.equipInfos[j].levelex;
                                                }
                                            }
                                        }

                                        if (roledata.detailInfo.heroInfos[i].heroInfo.starArrayInfos.Count > 0)
                                        {
                                            hi.hero_star_min_level = roledata.detailInfo.heroInfos[i].heroInfo.starArrayInfos[0].star_count;
                                            for (int j = 0; j < roledata.detailInfo.heroInfos[i].heroInfo.starArrayInfos.Count; j++)
                                            {
                                                if (roledata.detailInfo.heroInfos[i].heroInfo.starArrayInfos[j].star_count < hi.hero_star_min_level)
                                                {
                                                    hi.hero_star_min_level = roledata.detailInfo.heroInfos[i].heroInfo.starArrayInfos[j].star_count;
                                                }
                                            }
                                        }

                                        hi.hero_pet_star = (roledata.lookInfo.heroInfos[i].pet_type - roledata.lookInfo.heroInfos[i].pet_type / 10000 * 10000) / 100;

                                        pdi.heroInfo.Add(hi);
                                        #endregion

                                        #region 助战位信息
                                        PlayerDataFightInfo fi = new PlayerDataFightInfo();
                                        if (roledata.detailInfo.heroInfos[i].fightStoneInfos.Count > 0)
                                        {
                                            fi.equip_min_level = roledata.detailInfo.heroInfos[i].fightStoneInfos[0].level;
                                            fi.equip_min_levelex = roledata.detailInfo.heroInfos[i].fightStoneInfos[0].levelex;
                                            for (int j = 0; j < roledata.detailInfo.heroInfos[i].fightStoneInfos.Count; j++)
                                            {
                                                if (roledata.detailInfo.heroInfos[i].fightStoneInfos[j].level < fi.equip_min_level)
                                                {
                                                    fi.equip_min_level = roledata.detailInfo.heroInfos[i].fightStoneInfos[j].level;
                                                }
                                                if (roledata.detailInfo.heroInfos[i].fightStoneInfos[j].levelex < fi.equip_min_levelex)
                                                {
                                                    fi.equip_min_levelex = roledata.detailInfo.heroInfos[i].fightStoneInfos[j].levelex;
                                                }

                                                switch ((FightStoneColor)roledata.detailInfo.heroInfos[i].fightStoneInfos[j].color)
                                                {
                                                    case FightStoneColor.Blue: bi.blue_equip_count++; break;
                                                    case FightStoneColor.Purple: bi.purple_equip_count++; break;
                                                    case FightStoneColor.Orange: bi.orange_equip_count++; break;
                                                    case FightStoneColor.Red: bi.red_equip_count++; break;
                                                }

                                            }
                                        }

                                        if (roledata.detailInfo.heroInfos[i].stoneInfos.Count > 0)
                                        {
                                            fi.stone_min_level = roledata.detailInfo.heroInfos[i].stoneInfos[0].level;
                                            fi.stone_min_levelex = roledata.detailInfo.heroInfos[i].stoneInfos[0].levelex;
                                            for (int j = 0; j < roledata.detailInfo.heroInfos[i].stoneInfos.Count; j++)
                                            {
                                                if (roledata.detailInfo.heroInfos[i].stoneInfos[j].level < fi.stone_min_level)
                                                {
                                                    fi.stone_min_level = roledata.detailInfo.heroInfos[i].stoneInfos[j].level;
                                                }
                                                if (roledata.detailInfo.heroInfos[i].stoneInfos[j].levelex < fi.stone_min_levelex)
                                                {
                                                    fi.stone_min_levelex = roledata.detailInfo.heroInfos[i].stoneInfos[j].levelex;
                                                }

                                                switch ((StoneColor)(roledata.detailInfo.heroInfos[i].stoneInfos[j].type % 10 + 1))
                                                {
                                                    case StoneColor.Blue: bi.blue_stone_count++; break;
                                                    case StoneColor.Purple: bi.purple_stone_count++; break;
                                                    case StoneColor.Orange: bi.orange_stone_count++; break;
                                                    case StoneColor.Red: Log.AddLog(bi.uid + " " + hi.hero_id + " 5"); break;
                                                }

                                            }
                                        }

                                        pdi.fightInfo.Add(fi);
                                        #endregion

                                    }

                                }
                            }
                            bi.money = reader.GetInt32(2);
                            bi.yuanbao = reader.GetInt32(3);

                            pdi.baseInfo = bi;

                            if (!playerDataInfoDic.ContainsKey(bi.uid))
                            {
                                playerDataInfoDic.Add(bi.uid, pdi);
                            }
                        }
                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT user.uid,3rd_data,money,yuanbao FROM USER,user_info WHERE user.uid=user_info.uid"
            );
        }

        public static System.Data.DataTable DicToTable()
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt.Columns.Add(TableManager.GetGMTText(8), typeof(uint));
            dt.Columns.Add(TableManager.GetGMTText(61), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(63), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(830), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(653), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(831), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(832), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(833), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(834), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(835), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(836), typeof(int));
            dt.Columns.Add(TableManager.GetGMTText(837), typeof(int));

            for (int i = 1; i <= 6; i++)
            {
                dt.Columns.Add(TableManager.GetGMTText(438) + i + "ID", typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(438) + i + TableManager.GetGMTText(838), typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(438) + i + TableManager.GetGMTText(839), typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(840) + i, typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(841) + i, typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(842) + i, typeof(int));
            }

            for (int i = 1; i <= 6; i++)
            {
                dt.Columns.Add(TableManager.GetGMTText(843) + i, typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(844) + i, typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(845) + i, typeof(int));
                dt.Columns.Add(TableManager.GetGMTText(846) + i, typeof(int));
            }

            foreach (var pdi in playerDataInfoDic)
            {
                DataRow dr = dt.NewRow();
                dr[0] = pdi.Key;

                PlayerDataBaseInfo bi = pdi.Value.baseInfo;
                List<PlayerDataHeroInfo> hiList = pdi.Value.heroInfo;
                List<PlayerDataFightInfo> fiList = pdi.Value.fightInfo;

                dr[1] = bi.team_level;
                dr[2] = bi.yuanbao;
                dr[3] = bi.money;
                dr[4] = bi.soulex_level;
                dr[5] = bi.blue_equip_count;
                dr[6] = bi.purple_equip_count;
                dr[7] = bi.orange_equip_count;
                dr[8] = bi.red_equip_count;
                dr[9] = bi.blue_stone_count;
                dr[10] = bi.purple_stone_count;
                dr[11] = bi.orange_stone_count;

                for (int i = 0; i < 6; i++)
                {
                    int k = 12 + (i * 6);
                    if (i < hiList.Count)
                    {
                        dr[k] = hiList[i].hero_id;
                        dr[k + 1] = hiList[i].hero_color;
                        dr[k + 2] = hiList[i].hero_star;
                        dr[k + 3] = hiList[i].hero_equip_min_level;
                        dr[k + 4] = hiList[i].hero_star_min_level;
                        dr[k + 5] = hiList[i].hero_pet_star;
                    }
                    else
                    {
                        dr[k] = -1;
                        dr[k + 1] = -1;
                        dr[k + 2] = -1;
                        dr[k + 3] = -1;
                        dr[k + 4] = -1;
                        dr[k + 5] = -1;
                    }

                    k = 48 + (4 * i);
                    if (i < fiList.Count)
                    {
                        dr[k] = fiList[i].equip_min_level;
                        dr[k + 1] = fiList[i].equip_min_levelex;
                        dr[k + 2] = fiList[i].stone_min_level;
                        dr[k + 3] = fiList[i].stone_min_levelex;
                    }
                    else
                    {
                        dr[k] = -1;
                        dr[k + 1] = -1;
                        dr[k + 2] = -1;
                        dr[k + 3] = -1;
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private static System.Web.UI.Page page;

        private static Dictionary<uint, PlayerDataInfo> playerDataInfoDic = new Dictionary<uint, PlayerDataInfo>();
    }

    public class PlayerDataInfo
    {
        /// <summary>
        /// 基础信息
        /// </summary>
        public PlayerDataBaseInfo baseInfo;
        /// <summary>
        /// 助战位信息
        /// </summary>
        public List<PlayerDataFightInfo> fightInfo;
        /// <summary>
        /// 英雄信息
        /// </summary>
        public List<PlayerDataHeroInfo> heroInfo;

        public PlayerDataInfo()
        {
            baseInfo = new PlayerDataBaseInfo();
            fightInfo = new List<PlayerDataFightInfo>();
            heroInfo = new List<PlayerDataHeroInfo>();
        }
    }

    public class PlayerDataBaseInfo
    {
        /// <summary>
        /// 玩家ID
        /// </summary>
        public uint uid;
        /// <summary>
        /// 团队等级
        /// </summary>
        public int team_level;
        /// <summary>
        /// 元宝
        /// </summary>
        public int yuanbao;
        /// <summary>
        /// 金币
        /// </summary>
        public int money;
        /// <summary>
        /// 官职等级
        /// </summary>
        public int hero_soul_ex;
        /// <summary>
        /// 炼魂等级，Role3rdData.soulex_level
        /// </summary>
        public int soulex_level;
        /// <summary>
        /// 蓝装备数，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.color
        /// </summary>
        public int blue_equip_count;
        /// <summary>
        /// 紫装备数，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.color
        /// </summary>
        public int purple_equip_count;
        /// <summary>
        /// 橙装备数，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.color
        /// </summary>
        public int orange_equip_count;
        /// <summary>
        /// 红装备数，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.color
        /// </summary>
        public int red_equip_count;
        /// <summary>
        /// 蓝饰品数，饰品ID：Role3rdData/RoleDetailInfo/HeroDetailInfo/StoneInfo.index，饰品颜色=ID%10，
        /// </summary>
        public int blue_stone_count;
        /// <summary>
        /// 紫饰品数，Role3rdData/RoleDetailInfo/HeroDetailInfo/StoneInfo
        /// </summary>
        public int purple_stone_count;
        /// <summary>
        /// 橙饰品数，Role3rdData/RoleDetailInfo/HeroDetailInfo/StoneInfo
        /// </summary>
        public int orange_stone_count;

        public PlayerDataBaseInfo()
        {
            uid = 0;
            team_level = 0;
            yuanbao = 0;
            money = 0;
            hero_soul_ex = 0;
            soulex_level = 0;
            blue_equip_count = 0;
            purple_equip_count = 0;
            orange_equip_count = 0;
            red_equip_count = 0;
            blue_stone_count = 0;
            purple_stone_count = 0;
            orange_stone_count = 0;
        }
    }

    public class PlayerDataHeroInfo
    {
        /// <summary>
        /// 英雄ID，Role3rdData/RoleDetailInfo/HeroDetailInfo/HeroInfo.index
        /// </summary>
        public int hero_id;
        /// <summary>
        /// 英雄品质，Role3rdData/RoleDetailInfo/HeroDetailInfo/HeroInfo.color
        /// </summary>
        public int hero_color;
        /// <summary>
        /// 英雄星级，Role3rdData/RoleDetailInfo/HeroDetailInfo/HeroInfo.star
        /// </summary>
        public int hero_star;
        /// <summary>
        /// 兵魂共鸣等级，Role3rdData/RoleDetailInfo/HeroDetailInfo/HeroInfo/EquipInfo.levelex
        /// </summary>
        public int hero_equip_min_level;
        /// <summary>
        /// 命格共鸣等级，Role3rdData/RoleDetailInfo/HeroDetailInfo/HeroInfo/StarArrayInfo.star_count
        /// </summary>
        public int hero_star_min_level;
        /// <summary>
        /// 灵宝星级，灵宝ID：Role3rdData/RoleLookInfo/HeroLookInfo.pet_type，星级=（ID-ID/10000*10000）/100
        /// </summary>
        public int hero_pet_star;
    }

    public class PlayerDataFightInfo
    {
        /// <summary>
        /// 装备强化共鸣，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.level
        /// </summary>
        public int equip_min_level;
        /// <summary>
        /// 装备突破共鸣，Role3rdData/RoleDetailInfo/HeroDetailInfo/FightStoneInfo.levelex
        /// </summary>
        public int equip_min_levelex;
        /// <summary>
        /// 饰品精炼共鸣，Role3rdData/RoleDetailInfo/HeroDetailInfo/StoneInfo.level
        /// </summary>
        public int stone_min_level;
        /// <summary>
        /// 饰品附魔共鸣，Role3rdData/RoleDetailInfo/HeroDetailInfo/StoneInfo.levelex
        /// </summary>
        public int stone_min_levelex;
    }

    /// <summary>
    /// 饰品颜色
    /// </summary>
    public enum StoneColor
    {
        Null = -1,
        Blue = 2,
        Purple = 3,
        Orange = 4,
        Red = 5,
        Max,
    }

    /// <summary>
    /// 装备颜色
    /// </summary>
    public enum FightStoneColor
    {
        Null = -1,
        Green = 0,
        Blue = 1,
        Purple = 2,
        Orange = 3,
        Red = 4,
    }

    /// <summary>
    /// 导出Excel文件
    /// </summary>
    public class ExportExcel
    {
        /// <summary>
        /// DataTable直接导出Excel,此方法会把DataTable的数据用Excel打开,再自己手动去保存到确切的位置
        /// </summary>
        /// <param name="dt">要导出Excel的DataTable</param>
        /// <returns></returns>
        public static bool DoExport(System.Data.DataTable dt, string serverName)
        {
            Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.ApplicationClass();
            if (app == null)
            {
                Log.AddLog(DateTime.Now.ToString() + ":Excel无法启动");
                return false;
            }
            app.Visible = false;
            app.DisplayAlerts = false;
            app.AlertBeforeOverwriting = false;

            Workbooks wbs = app.Workbooks;
            Workbook wb = wbs.Add(Missing.Value);
            Worksheet ws = (Worksheet)wb.Worksheets[1];

            int rowcnt = dt.Rows.Count;
            int colcnt = dt.Columns.Count;

            object[,] objData = new Object[rowcnt + 1, colcnt];

            //获取列标题
            for (int i = 0; i < colcnt; i++)
            {
                objData[0, i] = dt.Columns[i].ColumnName;
            }

            //获取具体数据
            for (int i = 0; i < rowcnt; i++)
            {
                DataRow dr = dt.Rows[i];
                for (int j = 0; j < colcnt; j++)
                {
                    objData[i + 1, j] = dr[j];
                }
            }

            //写入Excel
            Range r = ws.get_Range(app.Cells[1, 1], app.Cells[rowcnt + 1, colcnt]);
            r.NumberFormat = "@";
            r.Value2 = objData;
            r.EntireColumn.AutoFit();

            try
            {
                #region GetSaveAsFilename

                string fileName = "E:\\" + serverName + DateTime.Now.ToString("yyyyMMdd");
                string fileFilter = string.Format("{0}(*.xlsx),*.xlsx,{1}(*.xls),*.xls,{2}(*.*),*.*", TableManager.GetGMTText(849), TableManager.GetGMTText(849), TableManager.GetGMTText(851));

                string filePath = (string)app.GetSaveAsFilename(fileName, fileFilter, 1, TableManager.GetGMTText(848), Missing.Value);

                wb.SaveAs(filePath);

                #endregion

                app.Workbooks.Close();
                app.Quit();
                app = null;
                return true;
            }
            catch (Exception e)
            {
                Log.AddLog(DateTime.Now.ToString() + "：" + e.Message);
                return false;
            }
        }

    }

}