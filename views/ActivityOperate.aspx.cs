using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Web.Script.Services;

namespace gmt
{
    public enum ERequestType
    {
        e_task_info_type_db_start,                                          ///0
        e_task_info_type_login_count,                                       ///1	连续登录
        e_task_info_type_sign_count,                                        ///2	签到次数
        e_task_info_type_yuanbao_in,                                        ///3	元宝充值数
        e_task_info_type_yuanbao_out,                                       ///4	元宝消费数
        e_task_info_type_yuanbao_get_pre_day_1,                             ///5	元宝连续每日充值档位1
        e_task_info_type_yuanbao_get_pre_day_2,                             ///6	元宝连续每日充值档位2
        e_task_info_type_yuanbao_get_pre_day_3,                             ///7	元宝连续每日充值档位3
        e_task_info_type_yuanbao_get_pre_day_4,                             ///8	元宝连续每日充值档位4
        e_task_info_type_yuanbao_get_pre_day_5,                             ///9	元宝连续每日充值档位5
        e_task_info_type_yuanbao_get_pre_day_6,                             ///10	元宝连续每日充值档位6
        e_task_info_type_task_count,                                        ///11	任务数量
        e_task_info_type_achieve_count,                                     ///12	成就数量
        e_task_info_type_yuanbao_count,                                     ///13	元宝数量
        e_task_info_type_spe_ins_type_quality_count,                        ///14	进阶丹副本次数
        e_task_info_type_spe_ins_type_skill_count,                          ///15	兵法副本次数
        e_task_info_type_spe_ins_type_stone_count,                          ///16	饰品精炼石副本次数
        e_task_info_type_active_count,                                      ///17	活跃点
        e_task_info_type_team_lvl_up,                                       ///18	团队升级次数
        e_task_info_type_hero_lvl_up,                                       ///19	英雄升级次数
        e_task_info_type_group_instance_count,                              ///20	团队本次数
        e_task_info_type_login_count_no_clear,                              ///21	累计登录	
        e_task_info_type_hero_skill,                                        ///22	英雄技能升级次数
        e_task_info_type_challenge_count,                                   ///23	挑战千机楼次数
        e_task_info_type_common_instance_count,                             ///24	完成普通副本
        e_task_info_type_hard_instance_count,                               ///25	完成精英副本
        e_task_info_type_spe_instance_count,                                ///26	完成每日副本次数
        e_task_info_type_soul_ex_lv_up_coutn,                               ///27	煉魂次數
        e_task_info_type_pet_stone_lv_up_count,                             ///28   晶石強化次數
        e_task_info_type_fight_stone_lv_up_count,                           ///29   裝備強化次數
        e_task_info_type_pvp,                                               ///30	完成pvp竞技场
        e_task_info_type_item_equip_count,                                  ///31   物品装备次数
        e_task_info_type_star_active_count,                                 ///32   星阵激活次数
        e_task_info_type_stone_equip_count,                                 ///33   灵石镶嵌次数
        e_task_info_type_stone_lvl_up_count,                                ///34	飾品精炼次数
        e_task_info_type_hero_equip_lvl_up,                                 ///35	专属装备强化次数
        e_task_info_type_give_active_count,                                 ///36   赠送好友体力次数
        e_task_info_type_get_active_count,                                  ///37   领取好友体力次数
        e_task_info_type_buy_money_count,                                   ///38   购买金币次数
        e_task_info_type_buy_active_count,                                  ///39   购买体力次数
        e_task_info_type_xunbao_count,                                      ///40   万宝楼寻宝次数
        e_task_info_type_linglong_buy_count,                                ///41   玲珑阁购买物品次数
        e_task_info_type_monopoly_roll_count,                               ///42   大富翁摇色子次数
        e_task_info_type_wboss_money_inspire_count,                         ///43	豪杰试炼金币鼓舞次数
        e_task_info_type_wboss_yuanbao_inspire_count,                       ///44	豪杰试炼元宝鼓舞次数
        e_task_info_type_wboss_count,                                       ///45   豪杰试炼次数
        e_task_info_type_jiebiao_count,                                     ///46   木牛流马劫镖次数
        e_task_info_type_activity_roll_count,                               ///47   抽奖活动抽奖次数
        e_task_info_type_vshundred_count,                                   ///48   百战千军次数
        e_task_info_type_guild_devote_count,                                ///49	帮派捐献次数
        e_task_info_type_guild_instance_count,                              ///50	帮派禁地战斗次数
        e_task_info_type_treasury_rob_count,                                ///51	诸侯割据掠夺次数
        e_task_info_type_treasuy_patrol_count,                              ///52	诸侯割据巡城次数
        e_task_info_type_treasuy_patrol_hours,                              ///53	诸侯割据巡城时间小时
        e_task_info_type_yb_roll_b_count_10,                                ///54	进行万宝楼元宝大额10连抽次数
        e_task_info_type_baoku_roll_count,                                  ///55	进行幸运抽奖次数
        e_task_info_type_treasury_rescue_other_count,                       ///56   诸侯割据救援他人次数
        e_task_info_type_yb_roll_s_count,                                   ///57	进行万宝楼元宝小额抽奖次数
        e_task_info_type_yb_roll_b_count,                                   ///58   进行万宝楼元宝大额抽奖次数
        e_task_info_type_get_free_active_count,                             ///59	领取免费体力的次数
        e_task_info_type_vshundred_win_count,                               ///60   百战千军胜利次数
        e_task_info_type_linglong_refresh_count,                            ///61   残兵阁刷新次数
        e_task_info_type_hero_equip_soul_lv_up_count,                       ///62   完成专属装备铸魂次数
        e_task_info_type_rmb_rank_receipt_val,                              ///63   充值排行榜活动累计值
        e_task_info_type_spe_ins_type_money_count,                          ///64	完成铜钱副本的次数
        e_task_info_type_spe_ins_type_exp_count,                            ///65	完成经验丹副本的次数
        e_task_info_type_green_escort_yabiao_count,                         ///66	完成含绿色以上的押票次数
        e_task_info_type_blue_escort_yabiao_count,                          ///67	完成含蓝色以上的押票次数
        e_task_info_type_purple_escort_yabiao_count,                        ///68	完成含紫色以上的押票次数
        e_task_info_type_orange_escort_yabiao_count,                        ///69	完成含橙色以上的押票次数
        e_task_info_type_jiebiao_win_count,                                 ///70   木牛流马劫镖胜利次数
        e_task_info_type_linglong_buy_hero_soul_count,                      ///71   玲珑阁购买英雄武魂的数量
        e_task_info_type_yigu_instance_count,                               ///72   进行一孤侠道的次数
        e_task_info_type_linglong_hero_soul_buy_count,                      ///73   残兵阁英雄武魂的购买次数
        e_task_info_type_escort_credits,                                    ///74	押劫鏢積分
        e_task_info_type_equip_instance_count,                              ///75   觉醒副本次数
        e_task_info_type_pet_s_roll_count,                                  ///76	宠物普通召唤次数   
        e_task_info_type_pet_b_roll_count,                                  ///77   宠物高级召唤次数
        e_task_info_type_pet_ex_roll_count,                                 ///78	宠物神宠召唤次数
        e_task_info_type_pet_summon_count,                                  ///79   宠物召唤次数
        e_task_info_type_challenge_box_open_count,                          ///80	八阵图开启隐藏宝箱次数
        e_task_info_type_normal_box_open_count,                             ///81	突破宝箱开启次数
        e_task_info_type_task_count_week_0,                                 ///82   开服活动1任务完成数
        e_task_info_type_task_count_week_1,                                 ///83   开服活动2任务完成数
        e_task_info_type_task_rmb_shop_index_min,                           ///84	充值成就项
        e_task_info_type_task_rmb_shop_index_max = e_task_info_type_task_rmb_shop_index_min + 15, ///99	充值成就项
        e_task_info_type_task_challenge_reset_count,                        ///100  八阵图累计重置次数
        e_task_info_type_spe_ins_type_pet_count,                            ///101	晶石经验石副本
        e_task_info_type_spe_ins_type_equip_count,                          ///102	神兵精炼石副本
        e_task_info_type_spe_ins_type_stone_quality_count,                  ///103	饰品附魔石副本
        e_task_info_type_shengpin_shop_refresh_count,                       ///104	锻造商店刷新次数
        e_task_info_type_shengpin_shop_buy_count,                           ///105	锻造商店购买次数
        e_task_info_type_energy,                                            ///106	体力值消耗
        e_task_info_type_yuanbao_open_count,                                ///107 聚宝盆聚宝次数
        e_task_info_type_yuanbao_in_of_mystic_shop,                         ///108 神秘商店的累计充值
        e_task_info_type_yb_roll_b_10_per_day,                              ///109 每日大抽奖档位1
        e_task_info_type_yb_roll_b_count_yuanbao_only,                      ///110 武将占星次数(只算元宝)
        e_task_info_type_activity_yuanbao_in,                               ///111	活动元宝充值数
        e_task_info_type_activity_yuanbao_out,                              ///112	活动元宝消费数
        e_task_info_type_task_8_14_open_task_treasury_patrol_count,         ///113  8至14日累计诸侯巡城次数
        e_task_info_type_task_8_14_open_task_treasury_rob_count,            // 114  8至14日累计诸侯掠夺次数
        e_task_info_type_task_king_all_attend_cnt,                          // 115  参加最强之战次数
        e_task_info_type_task_king_attend_cnt,                              // 116  参加王者之战次数
        e_task_info_type_task_guild_war_attend_cnt,                         // 117  参加军团资源战次数
        e_task_info_type_task_group_ins_type_0_attend_cnt,                  // 118  组队副本类型1次数
        e_task_info_type_task_group_ins_type_1_attend_cnt,                  // 119  组队副本类型2次数
        e_task_info_type_task_8_14_open_task_purple_escort_count,           // 120  NULL
        e_task_info_type_task_8_14_open_task_orange_escort_count,           // 121  NULL
        e_task_info_type_task_discount_shop_buy_energy_count,               // 122  折扣商店购买体力的次数
        e_task_info_type_task_join_guild_count,                             // 123  加入帮派的次数
        e_task_info_type_activity_yuanbao_3rd,                              ///124	3倍元宝充值数
        e_task_info_type_worship,                                           ///125  膜拜次数
        e_task_info_type_online_time,                                       ///126  在线时长

        e_task_info_type_db_end = 127,
        e_task_info_type_common = e_task_info_type_db_end,
        e_task_info_type_team_level,                                        ///128 团队等级
        e_task_info_type_hero_level,                                        ///129 阵容英雄最大等级
        e_task_info_type_friend_count,                                      ///130 好友数量
        e_task_info_type_vip_level,                                         ///131 vip等级
        e_task_info_type_hero_type,                                         ///132 拥有英雄
        e_task_info_type_hero_count,                                        ///133 拥有英雄数量
        e_task_info_type_challenge_take_to_chapter_b_level_a,               ///134 八阵图挑战到b章a关
        e_task_info_type_instance_comm,                                     ///135 副本通关
        e_task_info_type_instance_hard,                                     ///136 精英通关
        e_task_info_type_instance_comm_star_max,                            ///137 普通章节满星通关
        e_task_info_type_instance_hard_star_max,                            ///138 精英章节满星通关
        e_task_info_type_x_lv_hero_count,                                   ///139 等级b的英雄a个
        e_task_info_type_x_star_hero_count,                                 ///140 星级b的英雄a个
        e_task_info_type_x_quality_hero_count,                              ///141 品质b的英雄a个
        e_task_info_type_x_lv_hero_skill_count,                             ///142 等级b的英雄技能a个
        e_task_info_type_x_yang_stone_x_qua_hero_count,                     ///143 c品质的装备b个的英雄a个
        e_task_info_type_x_yin_stone_x_qua_hero_count,                      ///144 c品质的饰品b个的英雄a个
        e_task_info_type_x_star_rlv_hero_count,                             ///145 b等级星阵共鸣的英雄a个
        e_task_info_type_x_yang_stone_resonance_hero_count,                 ///146 阳灵石共鸣等级b的英雄a个
        e_task_info_type_x_yin_stone_resonance_hero_count,                  ///147 阴灵石共鸣等级b的英雄a个
        e_task_info_type_b_equip_rlvex_a,                                   ///148 b进化共鸣神兵a个
        e_task_info_type_x_qua_stone_x_lvex_count,                          ///149 突破至b级的饰品a颗
        e_task_info_type_challenge_rank_level_max,                          ///150 八阵图历史最高排名
        e_task_info_type_wboss_rank_level_max,                              ///151 豪杰试炼历史最高排名
        e_task_info_type_jjc_rank_level_max,                                ///152 竞技场历史最高排名
        e_task_info_type_team_zdl,                                          ///153 团队战力
        e_task_info_type_invest,                                            ///154 购买理财计划
        e_task_info_type_guild_active_count,                                ///155 帮派活跃值
        e_task_info_type_buy_invest_player_count,                           ///156 购买理财计划的玩家人数(全民福利)
        e_task_info_type_craft_soul_level,                                  ///157 官职等级
        e_task_info_type_instance_equip_star_max,                           ///158 神兵本章节满星通关
        e_task_info_type_ex_month_card,                                     ///159 大月卡(1表示需要)
        e_task_info_type_instance_yigu_star_max,                            ///160 一孤本章节满星通关
        e_task_info_type_instance_eqip,                                     ///161 神兵通关
        e_task_info_type_instance_yigu,                                     ///162 一孤通关
        e_task_info_type_x_qua_fight_stone_x_lvex_count,                    ///163 突破至b级的装备a颗
        e_task_info_type_b_roll_val,                                        ///164 占星大成就
        e_task_info_type_guild_instance_index_a,                            ///165 完成帮派副本a章节
        e_task_info_type_guild_instance_index_a_no_clear,                   ///166 首次完成帮派副本a章节
        e_task_info_type_new_invest_0,                                      ///167 新手理财0
        e_task_info_type_a_pos_equip_b_quality_pet,                         ///168 a个战位上装备了b品质的灵宝
        e_task_info_type_fight_pet_equip_a_pet_stone_with_b_quality,        ///169 战位上的宠物装备a个b品质的晶石
        e_task_info_type_a_hero_has_b_levelex_equip,                        ///170 a名武将有觉醒等级b的神兵
        e_task_info_type_week_yuanbao_in,                                   ///171 周元宝充值
        e_task_info_type_new_invest_1,                                      ///172 新手理财1
        e_task_info_type_new_invest_2,                                      ///173 新手理财2
        e_task_info_type_new_invest_3,                                      ///174 新手理财3
        e_task_info_type_week_roll_10,                                      ///175 周10连抽
        e_task_info_type_craft_soulex_level,                                ///176 炼魂等级
        e_task_info_type_b_qua_fight_stone_a_count,                         ///177 b品质的装备a个
        e_task_info_type_hero_id_b_star_level_a,                            ///178 ID为b的英雄a星级
        e_task_info_type_b_sub_equip_lvex_a,                                // 179 b进化等级的神兵兵魂a个
        e_task_info_type_s_month_flag,                                      ///180 小月卡
        e_task_info_type_b_month_flag,                                      ///181 大月卡
        e_task_info_type_ex_month_flag,                                     ///182 至尊卡
        e_task_info_type_sb_month_flag,                                     ///183 大小月卡
        e_task_info_type_sex_month_flag,                                    ///184 至尊小月卡
        e_task_info_type_bex_month_flag,                                    ///185 至尊大月卡
        e_task_info_type_all_month_flag,                                    ///186 所有月卡
        e_task_info_type_review_done,                                       ///187 完成一次评价
        e_task_info_type_zdl_rank_level_max,                                ///188 战力榜历史最高排名
        e_task_info_type_zdl_all_rank_level_max,                            ///189 全区战力榜历史最高排名
        e_task_info_type_share_done,                                        ///190 完成一次分享
        e_task_info_type_b_color_hero_a_count,                              ///191 b颜色的英雄a个
        e_task_info_type_b_level_mount_a_count,								///192 b等级的坐骑a个

        e_task_info_type_max,
    }

    public class STActivityInfo
    {
        public int id;
        public string title;
        public string desc;
        public bool firstmark;
        public int order;
        public List<STQuestInfo> quests;
    }

    public class STRewardInfo
    {
        public int type;
        public string typename;
        public int item;
        public string itemname;
        public int count;
    };

    public class STQuestInfo
    {
        public string desc;
        public int type;
        public string typename;
        public int request;
        public string requestname;
        public int count_a;
        public int count_b;
        public int count_c;
        public bool overlap;
        public List<STRewardInfo> rewards;
    };

    public struct STSelectOption
    {
        public string text;
        public string value;
    }

    public partial class ActivityOperate : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ActivityOperate()
            : base(PrivilegeType.ActivityOperate)
        {

        }

        protected override void OnGmPageLoad()
        {
            if (Request.Files.Count != 0)
            {
                string strMsg = "";
                EErrType errcode = EErrType.ERR_SUCCESS;
                for (int i = 0; i < Request.Files.Count; ++i)
                {
                    bool suc = true;
                    HttpPostedFile file = Request.Files[i];
                    string savePath = Global.ProtoDataPath + file.FileName;
                    try
                    {
                        file.SaveAs(savePath);
                    }
                    catch (Exception e)
                    {
                        Log.AddLog(e.ToString());
                        suc = false;
                    }
                    if (suc)
                    {
                        strMsg += file.FileName + "\\n";
                    }
                }
                if (strMsg == "")
                {
                    errcode = EErrType.ERR_TABLE_DATA_SAVE_FAILED;
                }

                if (errcode == EErrType.ERR_SUCCESS)
                {
                    TextManager.Load();
                    GMTActivityMananger.start();
                }

                Response.Write("{\"error\":" + (int)errcode + ",\"msg\":\"" + strMsg + "\"}");
                Response.End();
            }
        }

        [WebMethod(EnableSession = true)]
        public static string AddNewActivity(string title, string desc, bool firstmark, int order, List<STQuestInfo> quests)
        {
            mw.ActivityConfig configActivity = null;
            if (quests.Count == 0)
            {
                CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_ACHIEVE_ZERO);
                goto err;
            }

            configActivity = GMTActivityMananger.GetNewConfigActivity();
            if (null == configActivity)
            {
                goto err;
            }

            configActivity.NeedFlag = (firstmark)?1:0;
            configActivity.sortvalue = order;

            List<mw.AchieveConfig> listAchieve = new List<mw.AchieveConfig>();
            List<mw.RewardConfig> listReward = new List<mw.RewardConfig>();

            if (!DoAddQuest(configActivity.id, quests, ref listAchieve, ref listReward))
            {
                goto err;
            }

            int titleTextId = 0;
            if (!string.IsNullOrEmpty(title))
            {
                titleTextId = TextManager.CreateText();
                TextManager.SetText(titleTextId, title);
            }

            int descTextId = 0;
            if (!string.IsNullOrEmpty(desc))
            {
                TextManager.SetText(descTextId, desc);
                descTextId = TextManager.CreateText();
            }

            foreach (var ach in listAchieve)
            {
                ach.name = titleTextId;
                ach.txt = descTextId;
            }

            // 创建条件满足

            STActivityInfo stActivity = ConvertToSTActivity(configActivity);
            m_listActivity4Client.Add(stActivity);

            GMTActivityMananger.SaveTable();

            string res = JsonConvert.SerializeObject(stActivity);
            return "{\"error\":0, \"data\":" + res + "}";

        // 创建条件不满足, 上方textid 创建还没销毁, 后面补充
        err:
            // 销毁 (还没写)
            if (null != configActivity)
            {
                GMTActivityMananger.RemoveActivityNode(configActivity.id);
            }
            return CErrMgr.GetLastErrMsg();
        }

        [WebMethod(EnableSession = true)]
        public static string RemoveActivity(List<int> ids)
        {
            List<int> removedIds = new List<int>();
            foreach(var id in ids)
            {
                if (GMTActivityMananger.RemoveActivityNode(id))
                {
                    removedIds.Add(id);
                }
            }
            GMTActivityMananger.SaveTable();
            m_listActivity4Client.RemoveAll(o => removedIds.Contains(o.id));
            string res = JsonConvert.SerializeObject(removedIds);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetQuestTypes()
        {
            List<STSelectOption> list = new List<STSelectOption>();
            foreach (int e in Enum.GetValues(typeof(mw.Enums.TaskType)))
            {
                if (e == (int)mw.Enums.TaskType.TASK_TYPE_DAILY
                    || e == (int)mw.Enums.TaskType.TASK_TYPE_SIGN
                    )
                {
                    string text = GetQuestTypeName(e);
                    STSelectOption info = new STSelectOption();
                    info.text = text;
                    info.value = e.ToString();
                    list.Add(info);
                }
            }
            string res = JsonConvert.SerializeObject(list);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetRequestInfos()
        {
            List<STSelectOption> list = new List<STSelectOption>();
            foreach (int e in Enum.GetValues(typeof(ERequestType)))
            {
                string text = GetRequestName(e);
                STSelectOption info = new STSelectOption();
                info.text = text;
                info.value = e.ToString();
                list.Add(info);
            }
            string res = JsonConvert.SerializeObject(list);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetRewardTypes()
        {
            List<STSelectOption> list = new List<STSelectOption>();
            foreach (int e in Enum.GetValues(typeof(mw.Enums.RewardType)))
            {
                if (e == (int)mw.Enums.RewardType.RWD_TYPE_BUFF
                    || e == (int)mw.Enums.RewardType.RWD_TYPE_EQUIP)
                {
                    continue;
                }

                string text = GetRewardTypeName(e);
                STSelectOption info = new STSelectOption();
                info.text = text;
                info.value = e.ToString();
                list.Add(info);
            }
            string res = JsonConvert.SerializeObject(list);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetItemName(int rwdType)
        {
            List<STSelectOption> list = new List<STSelectOption>();
            switch (rwdType)
            {
                // 0 经济类型
                case 0:
                    foreach (var pair in playerHistroy.economicName)
                    {
                        STSelectOption stOption = new STSelectOption();
                        if (pair.Key == 0) { continue; }
                        string text = string.Format("{0}({1})", TableManager.GetGMTText(21000 + (pair.Key)), pair.Key);
                        stOption.value = pair.Key.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
                // 1 物品
                case 1:
                    foreach (var pair in TableManager.ItemTable)
                    {
                        STSelectOption stOption = new STSelectOption();
                        if (pair.Key == 0) { continue; }
                        string text = string.Format("{0}({1})", TextManager.GetText(pair.Value.name), pair.Value.id);
                        stOption.value = pair.Value.id.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
                // 2 武魂
                case 2:
                case 3:
                    foreach (var pair in TableManager.HeroTable)
                    {
                        STSelectOption stOption = new STSelectOption();
                        if (pair.Key == 0) { continue; }
                        string text = string.Format("{0}({1})", TextManager.GetText(pair.Value.name), pair.Value.id);
                        stOption.value = pair.Value.id.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
                // 3 饰品
                case 4:
                    foreach (var pair in TableManager.StoneTable)
                    {
                        if (pair.Key == 0) { continue; }
                        string[] color = { "0", "1", "blue", "purple", "orange", "red" };
                        STSelectOption stOption = new STSelectOption();
                        string text = TextManager.GetText(pair.Value.name) + "[" + color[pair.Value.color] + "]" + "(" + pair.Value.id + ")";
                        stOption.value = pair.Value.id.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
                case 5:
                    {
                        foreach (var pair in TableManager.PetTable)
                        {
                            STSelectOption stOption = new STSelectOption();
                            string text = TextManager.GetText(pair.Value.name) + "[" + pair.Value.petstar + " STAR]" + "(" + pair.Value.idx + ")";
                            stOption.value = pair.Value.idx.ToString();
                            stOption.text = text;
                            list.Add(stOption);
                        }
                    }
                    break;
                // 8 晶石
                case 8:
                    foreach (var pair in TableManager.PetStoneTable)
                    {
                        STSelectOption stOption = new STSelectOption();
                        if (pair.Key == 0) { continue; }
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        stOption.value = pair.Value.id.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
                // 9 10 坐骑碎片
                case 9:
                case 10:
                    Regex r = new Regex("\\[.*?\\]");
                    foreach (var pair in TableManager.MountTable)
                    {
                        STSelectOption stOption = new STSelectOption();
                        if (pair.Key == 0) { continue; }
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        text = r.Replace(text, "");
                        stOption.value = pair.Value.id.ToString();
                        stOption.text = text;
                        list.Add(stOption);
                    }
                    break;
            }

            string res = JsonConvert.SerializeObject(list);
            return res;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public static string GetActivities()
        {
            string res = JsonConvert.SerializeObject(m_listActivity4Client);
            return res;
        }

        private static string GetQuestTypeName(int id)
        {
            return string.Format("{0}({1})", TableManager.GetGMTText(30501 + id), id);
        }

        private static string GetRequestName(int id)
        {
            return string.Format("{0}({1})", TableManager.GetGMTText(30000 + id), id);
        }

        private static string GetRewardTypeName(int id)
        {
            return string.Format("{0}({1})", TableManager.GetGMTText(20000 + id), id);
        }

        private static string GetItemName(int rwdType, int id)
        {
            switch (rwdType)
            {
                // 0 经济类型
                case 0:
                    return string.Format("{0}({1})", TableManager.GetGMTText(21000 + (id)), id);
                    break;
                // 1 物品
                case 1:
                    {
                        mw.ItemConfig config = null;
                        TableManager.ItemTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            return string.Format("{0}({1})", TextManager.GetText(config.name), id);
                        }
                    }
                    break;
                // 2 武魂
                case 2:
                case 3:
                    {
                        mw.HeroBaseConfig config = null;
                        TableManager.HeroTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            return string.Format("{0}({1})", TextManager.GetText(config.name), id);
                        }
                    }
                    break;
                // 3 饰品
                case 4:
                    {
                        mw.StoneConfig config = null;
                        TableManager.StoneTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            string[] color = { "0", "1", "blue", "purple", "orange", "red" };
                            return string.Format("{0}[{1}]({2})", TextManager.GetText(config.name), color[config.color], id);
                        }
                    }
                    break;
                case 5:
                    {
                        mw.PetConfig config = null;
                        TableManager.PetTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            return string.Format("{0}[{1} STAR]({2})", TextManager.GetText(config.name), config.petstar, id);
                        }
                    }
                    break;
                // 8 晶石
                case 8:
                    {
                        mw.ItemConfig config = null;
                        TableManager.PetStoneTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            return string.Format("{0}({1})", TextManager.GetText(config.name), id);
                        }
                    }
                    break;
                // 9 10 坐骑碎片
                case 9:
                case 10:
                    {
                        Regex r = new Regex("\\[.*?\\]");
                        mw.MountConfig config = null;
                        TableManager.MountTable.TryGetValue(id, out config);
                        if (null != config)
                        {
                            return string.Format("{0}({1})", TextManager.GetText(config.name), id);
                        }
                    }
                    break;
            }
            return "Unknown Id: " + id;
        }

        private static bool DoAddQuest(int activiytId, List<STQuestInfo> quests, ref List<mw.AchieveConfig> listAchieve, ref List<mw.RewardConfig> listReward)
        {
            for (int i = 0; i < quests.Count; ++i)
            {
                mw.Enums.TaskType taskType = (mw.Enums.TaskType)quests[i].type;
                mw.AchieveConfig configAchieve = GMTActivityMananger.GetNewConfigAchieve(taskType);
                if (null == configAchieve)
                {
                    return false;
                }

                configAchieve.activity = activiytId;
                configAchieve.taskFlag = (quests[i].overlap) ? 1 : 0;
                configAchieve.opt_type_1 = quests[i].type;
                configAchieve.opt_val_1_a = quests[i].count_a;
                configAchieve.opt_val_1_b = quests[i].count_b;
                configAchieve.opt_val_1_c = quests[i].count_c;

                if (quests[i].count_a < 1 || quests[i].count_a > 9999
                    || quests[i].count_b < 0 || quests[i].count_b > 9999
                    || quests[i].count_c < 0 || quests[i].count_c > 9999
                    )
                {
                    CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_ACHIEVE_REQUEST_COUNT);
                    return false;
                }

                if (quests[i].rewards.Count == 0
                    || quests[i].rewards.Count > 4)
                {
                    CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_REWARD_COUNT);
                    return false;
                }

                mw.RewardConfig configReward = null;
                bool ret = MakeRewardConfigWithRewardInfos(quests[i].rewards, out configReward);
                if (null != configReward)
                {
                    listReward.Add(configReward);
                }

                if (!ret)
                {
                    return false;
                }

                configAchieve.reward = configReward.id;

                int taskDescTextId = 0;
                if (!string.IsNullOrEmpty(quests[i].desc))
                {
                    taskDescTextId = TextManager.CreateText();
                    TextManager.SetText(taskDescTextId, quests[i].desc);
                }

                // 这里贼乱, name 是活动标题， txt是活动内容， desc 是活动任务描述
                configAchieve.desc = taskDescTextId;

                listAchieve.Add(configAchieve);
            }

            return true;
        }

        private static bool MakeRewardConfigWithRewardInfos(List<STRewardInfo> listRewards, out mw.RewardConfig configReward)
        {
            configReward = GMTActivityMananger.GetNewConfigReward();
            if (null == configReward)
            {
                CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_REWARD_ID_MAX);
                return false;
            }

            // 最大4个 不然客户端排版会出问题
            for (int i = 0; i < listRewards.Count; i++)
            {
                if (listRewards[i].count > 9999)
                {
                    CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_REWARD_ITEM_COUNT);
                    return false;
                }

                if (i == 0)
                {
                    configReward.reward_idx_1 = (mw.Enums.RewardType)listRewards[0].type;
                    configReward.reward_type_1 = listRewards[0].item;
                    configReward.reward_count_1 = listRewards[0].count;
                    configReward.max_rand_1 = 10000;
                    configReward.min_rand_1 = 1;
                }
                else if (i == 1)
                {
                    configReward.reward_idx_2 = (mw.Enums.RewardType)listRewards[1].type;
                    configReward.reward_type_2 = listRewards[1].item;
                    configReward.reward_count_2 = listRewards[1].count;
                    configReward.max_rand_2 = 10000;
                    configReward.min_rand_2 = 1;
                }
                else if (i == 2)
                {
                    configReward.reward_idx_3 = (mw.Enums.RewardType)listRewards[2].type;
                    configReward.reward_type_3 = listRewards[2].item;
                    configReward.reward_count_3 = listRewards[2].count;
                    configReward.max_rand_3 = 10000;
                    configReward.min_rand_3 = 1;
                }
                else if (i == 3)
                {
                    configReward.reward_idx_4 = (mw.Enums.RewardType)listRewards[3].type;
                    configReward.reward_type_4 = listRewards[3].item;
                    configReward.reward_count_4 = listRewards[3].count;
                    configReward.max_rand_4 = 10000;
                    configReward.min_rand_4 = 1;
                }
            }

            return true;
        }

        public static bool LoadAcitvity4Client()
        {
            m_listActivity4Client.Clear();
            Dictionary<int, mw.ActivityConfig> dic = GMTActivityMananger.GetTableActivity();
            foreach (var config in dic.Values)
            {
                STActivityInfo act = ConvertToSTActivity(config);
                if (null != act)
                {
                    m_listActivity4Client.Add(act);
                }
            }

            return true;
        }

        private static STActivityInfo ConvertToSTActivity(mw.ActivityConfig config)
        {
            STActivityInfo act = new STActivityInfo();
            act.id = config.id;

            List<mw.AchieveConfig> listAchieve = GMTActivityMananger.GetAchieveConfigsByActivityId(act.id);

            if (listAchieve.Count == 0)
            {
                return null;
            }

            int titleTextId = listAchieve[0].name;
            act.title = TextManager.GetText(titleTextId);

            int descTextId = listAchieve[0].txt;
            act.desc = TextManager.GetText(titleTextId);

            act.firstmark = (config.NeedFlag==1);
            act.order = config.sortvalue;

            act.quests = new List<STQuestInfo>();

            foreach(var AchieveConfig in listAchieve)
            {
                STQuestInfo quest = ConvertToSTQuest(AchieveConfig);
                act.quests.Add(quest);
            }

            return act;
        }

        private static STQuestInfo ConvertToSTQuest(mw.AchieveConfig config)
        {
            STQuestInfo quest = new STQuestInfo();
            quest.type = (int)config.type;
            quest.typename = GetQuestTypeName(quest.type);
            quest.desc = TextManager.GetText(config.desc);
            quest.overlap = (config.taskFlag == 1);
            quest.request = config.opt_type_1;
            quest.requestname = GetRequestName(quest.request);
            quest.count_a = config.opt_val_1_a;
            quest.count_b = config.opt_val_1_b;
            quest.count_c = config.opt_val_1_c;

            mw.RewardConfig configReward = GMTActivityMananger.GetRewardConfigsById(config.reward);
            if (null != configReward)
            {
                ConvertToSTReward(configReward, out quest.rewards);
            }

            return quest;
        }

        private static bool ConvertToSTReward(mw.RewardConfig config, out List<STRewardInfo> rewards)
        {
            rewards = new List<STRewardInfo>();

            if (config.reward_type_1 != 0)
            {
                STRewardInfo rd = new STRewardInfo();
                rd.type = (int)config.reward_idx_1;
                rd.typename = GetRewardTypeName(rd.type);
                rd.item = config.reward_type_1;
                rd.itemname = GetItemName(rd.type, rd.item);
                rd.count = config.reward_count_1;
                rewards.Add(rd);
            }

            if (config.reward_type_2 != 0)
            {
                STRewardInfo rd = new STRewardInfo();
                rd.type = (int)config.reward_idx_2;
                rd.typename = GetRewardTypeName(rd.type);
                rd.item = config.reward_type_2;
                rd.itemname = GetItemName(rd.type, rd.item);
                rd.count = config.reward_count_2;
                rewards.Add(rd);
            }

            if (config.reward_type_3 != 0)
            {
                STRewardInfo rd = new STRewardInfo();
                rd.type = (int)config.reward_idx_3;
                rd.typename = GetRewardTypeName(rd.type);
                rd.item = config.reward_type_3;
                rd.itemname = GetItemName(rd.type, rd.item);
                rd.count = config.reward_count_3;
                rewards.Add(rd);
            }

            if (config.reward_type_4 != 0)
            {
                STRewardInfo rd = new STRewardInfo();
                rd.type = (int)config.reward_idx_4;
                rd.typename = GetRewardTypeName(rd.type);
                rd.item = config.reward_type_4;
                rd.itemname = GetItemName(rd.type, rd.item);
                rd.count = config.reward_count_4;
                rewards.Add(rd);
            }

            return true;
        }

        private static List<STActivityInfo> m_listActivity4Client = new List<STActivityInfo>();
    }

    public static class GMTActivityMananger
    {
        private const int MinActivityID = 900;
        private const int MaxActivityID = 1000;

        private const int MinAchieveID = 1800;
        private const int MaxAchieveID = 2000;

        private const int MinRewardID = 0;
        private const int MaxRewardID = 1000;

        private const int MinGrouping = 800;
        private const int MaxGrouping = 1000;

        public static void start()
        {
            LoadTable();
            ActivityOperate.LoadAcitvity4Client();
        }

        public static void InitSpareIds()
        {
            spareActivityIds.Clear();
            spareAchieveIds.Clear();
            spareRewardIds.Clear();

            for(int i = MinActivityID; i <= MaxActivityID; ++i)
            {
                spareActivityIds.Add(i);
            }

            for(int i = MinAchieveID; i <= MaxAchieveID; ++i)
            {
                foreach (int e in Enum.GetValues(typeof(mw.Enums.TaskType)))
                {
                    if (e == (int)mw.Enums.TaskType.TASK_TYPE_MAIN
                        || e == (int)mw.Enums.TaskType.TASK_TYPE_MYSTIC
                        || e == (int)mw.Enums.TaskType.TASK_TYPE_GUILD
                        || e == (int)mw.Enums.TaskType.TASK_TYPE_COMMON
                        )
                    {
                        continue;
                    }

                    uint id = CUtils.MakeLong((ushort)e, (ushort)i);
                    spareAchieveIds.Add(id);
                }
            }

            for(int i = MinRewardID; i <= MaxRewardID; ++i)
            {
                spareRewardIds.Add(i);
            }
        }

        public static void LoadTable()
        {
            InitSpareIds();
            tableActivity.Clear();
            tableAchieve.Clear();
            tableReward.Clear();
            List<mw.ActivityConfig> listActivityEx = TableManager.Load<mw.ActivityConfig>("protodatas/ActivityExConfig.protodata.bytes");
            if (listActivityEx != null)
            {
                foreach(var val in listActivityEx)
                {
                    if (val != null)
                    {
                        AddActivityNode(val);
                    }
                }
            }

            List<mw.AchieveConfig> listAchieve = TableManager.Load<mw.AchieveConfig>("protodatas/AchieveExConfig.protodata.bytes");
            if (listAchieve != null)
            {
                foreach(var val in listAchieve)
                {
                    if (val != null)
                    {
                        AddAchieveNode(val);
                    }
                }
            }

            List<mw.RewardConfig> listReward = TableManager.Load<mw.RewardConfig>("protodatas/RewardExConfig.protodata.bytes");
            if (listReward != null)
            {
                foreach(var val in listReward)
                {
                    if (val != null)
                    {
                        AddRewardNode(val);
                    }
                }
            }
        }

        public static void SaveTable()
        {
            if (!m_isDirty)
                return;

            List<mw.ActivityConfig> listActivityGMT = new List<mw.ActivityConfig>();
            foreach (var pair in tableActivity)
            {
                if (pair.Key >= MinActivityID && pair.Key <= MaxActivityID)
                {
                    listActivityGMT.Add(pair.Value);
                }
            }
            TableManager.Save<mw.ActivityConfig>(listActivityGMT);
            TableManager.Save<mw.AchieveConfig>(tableAchieve.Values.ToList());
            TableManager.Save<mw.RewardConfig>(tableReward.Values.ToList());
        }
        
        public static mw.ActivityConfig GetNewConfigActivity()
        {
            if (spareActivityIds.Count == 0)
            {
                CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_ID_MAX);
                return null;
            }

            mw.ActivityConfig config = new mw.ActivityConfig();
            config.id = spareActivityIds.First();
            config.sign = 1;
            config.gmt_no_use = "0";
            config.name = "";

            AddActivityNode(config);

            return config;
        }

        public static mw.AchieveConfig GetNewConfigAchieve(mw.Enums.TaskType type)
        {
            if (spareAchieveIds.Count == 0)
            {
                CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_ACHIEVE_ID_MAX);
                return null;
            }

            mw.AchieveConfig config = new mw.AchieveConfig();
            for (int i = MinAchieveID; i <= MaxAchieveID; ++i)
            {
                uint id = CUtils.MakeLong((ushort)type, (ushort)i);
                if (spareAchieveIds.Contains(id))
                {
                    config.id = i;
                    config.type = type;
                    break;
                }
            }

            AddAchieveNode(config);

            return config;
        }

        public static mw.RewardConfig GetNewConfigReward()
        {
            if (spareRewardIds.Count == 0)
            {
                CErrMgr.SetLastErrMsg(EErrType.ERR_ACTIVITY_REWARD_ID_MAX);
                return null;
            }

            mw.RewardConfig config = new mw.RewardConfig();
            config.id = spareRewardIds.First();

            AddRewardNode(config);

            return config;
        }

        public static void AddActivityNode(mw.ActivityConfig val)
        {
            if (tableActivity.ContainsKey(val.id))
            {
                tableActivity[val.id] = val;
            }
            else
            {
                tableActivity.Add(val.id, val);
            }

            if (val.id >= MinActivityID && val.id <= MaxActivityID)
            {
                spareActivityIds.Remove(val.id);
            }
            m_isDirty = true;
        }

        public static void AddAchieveNode(mw.AchieveConfig val)
        {
            uint id = CUtils.MakeLong((ushort)val.type, (ushort)val.id);
            if (tableAchieve.ContainsKey(id))
            {
                tableAchieve[id] = val;
            }
            else
            {
                tableAchieve.Add(id, val);
            }

            if (val.id >= MinAchieveID && val.id <= MaxAchieveID)
            {
                spareAchieveIds.Remove(id);
            }
            m_isDirty = true;
        }

        public static void AddRewardNode(mw.RewardConfig val)
        {
            if (tableReward.ContainsKey(val.id))
            {
                tableReward[val.id] = val;
            }
            else
            {
                tableReward.Add(val.id, val);
            }

            if (val.id >= MinRewardID && val.id <= MaxRewardID)
            {
                spareRewardIds.Remove(val.id);
            }
            m_isDirty = true;
        }

        public static bool RemoveActivityNode(int id)
        {
            if (!tableActivity.ContainsKey(id))
                return false;

            mw.ActivityConfig config = null;
            tableActivity.TryGetValue(id, out config);

            if (null == config)
                return false;

            List<mw.AchieveConfig> listAchieve = GetAchieveConfigsByActivityId(id);
            foreach (var achieveConfig in listAchieve)
            {
                uint longid = CUtils.MakeLong((ushort)achieveConfig.type, (ushort)achieveConfig.id);
                RemoveAchieveNode(longid);
            }

            tableActivity.Remove(id);
            spareActivityIds.Add(id);
            config = null;

            m_isDirty = true;
            return true;
        }

        public static bool RemoveAchieveNode(uint id)
        {
            mw.AchieveConfig config = null;
            tableAchieve.TryGetValue(id, out config);
            if (null == config)
                return false;

            // text do not recycle
            //config.txt;
            //config.desc;
            //config.name;

            RemoveRewardNode(config.reward);
            config.reward = 0;

            tableAchieve.Remove(id);
            spareAchieveIds.Add(id);
            config = null;
            m_isDirty = true;
            return true;
        }

        public static bool RemoveRewardNode(int id)
        {
            mw.RewardConfig config = null;
            tableReward.TryGetValue(id, out config);
            if (null == config) return false;

            tableReward.Remove(id);
            spareRewardIds.Add(id);
            config = null;
            m_isDirty = true;
            return true;
        }

        public static List<mw.AchieveConfig> GetAchieveConfigsByActivityId(int id)
        {
            List<mw.AchieveConfig> list = new List<mw.AchieveConfig>();
            foreach (var config in tableAchieve.Values)
            {
                if (config.activity == id)
                {
                    list.Add(config);
                }
            }

            return list;
        }

        public static mw.ActivityConfig GetActivityConfigsById(int id)
        {
            mw.ActivityConfig config = null;
            tableActivity.TryGetValue(id, out config);
            return config;
        }
        public static mw.RewardConfig GetRewardConfigsById(int id)
        {
            mw.RewardConfig config = null;
            tableReward.TryGetValue(id, out config);
            return config;
        }

        public static Dictionary<int, mw.ActivityConfig> GetTableActivity()
        {
            return tableActivity;
        }
        public static Dictionary<uint, mw.AchieveConfig> GetTableAchieve()
        {
            return tableAchieve;
        }
        public static Dictionary<int, mw.RewardConfig> GetTableReward()
        {
            return tableReward;
        }

        private static bool m_isDirty = false;

        private static HashSet<int> spareActivityIds = new HashSet<int>();
        private static HashSet<uint> spareAchieveIds = new HashSet<uint>();
        private static HashSet<int> spareRewardIds = new HashSet<int>();

        private static Dictionary<int, mw.ActivityConfig> tableActivity = new Dictionary<int, mw.ActivityConfig>();
        private static Dictionary<uint, mw.AchieveConfig> tableAchieve = new Dictionary<uint, mw.AchieveConfig>();
        private static Dictionary<int, mw.RewardConfig> tableReward = new Dictionary<int, mw.RewardConfig>();
    }
}