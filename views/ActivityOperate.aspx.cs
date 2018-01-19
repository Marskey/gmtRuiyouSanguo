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
        e_task_info_type_yuanbao_in = 3,                                    ///3	元宝充值数
        e_task_info_type_yuanbao_out = 4,                                   ///4	元宝消费数
        e_task_info_type_group_instance_count = 20,                         ///20	组队副本完成次数
        e_task_info_type_login_count_no_clear = 21,                         ///21	累计登录	
        e_task_info_type_challenge_count = 23,                              ///23	八阵图完成次数
        e_task_info_type_common_instance_count = 24,                        ///24	主线副本完成次数
        e_task_info_type_hard_instance_count = 25,                          ///25	军武校场完成次数
        e_task_info_type_spe_instance_count = 26,                           ///26	日常挑战完成次数
        e_task_info_type_pvp = 30,                                          ///30	演武场完成次数
        e_task_info_type_give_active_count = 36,                            ///36   赠送好友体力次数
        e_task_info_type_buy_money_count = 38,                              ///38   购买铜钱次数
        e_task_info_type_buy_active_count = 39,                             ///39   购买体力次数
        e_task_info_type_wboss_count = 45,                                  ///45   无尽挑战次数
        e_task_info_type_jiebiao_count = 46,                                ///46   木牛流马劫粮次数
        e_task_info_type_treasury_rob_count = 51,                           ///51	诸侯割据掠夺次数
        e_task_info_type_treasuy_patrol_count = 52,                         ///52	诸侯割据巡城次数
        e_task_info_type_yb_roll_b_count_10 = 54,                           ///54	武将占星10连抽次数
        e_task_info_type_yb_roll_b_count = 58,                              ///58   武将占星次数
        e_task_info_type_vshundred_win_count = 60,                          ///60   过关斩将胜利次数【小关】
        e_task_info_type_spe_ins_type_money_count = 64,                     ///64	完成日常挑战-铜钱的次数
        e_task_info_type_spe_ins_type_exp_count = 65,                       ///65	完成日常挑战-经验丹的次数数
        e_task_info_type_green_escort_yabiao_count = 66,                    ///66	完成木牛流马含绿色以上的押运次数
        e_task_info_type_blue_escort_yabiao_count = 67,                     ///67	完成木牛流马含蓝色以上的押运次数
        e_task_info_type_purple_escort_yabiao_count = 68,                   ///68	完成木牛流马含紫色以上的押运次数
        e_task_info_type_orange_escort_yabiao_count = 69,                   ///69	完成木牛流马含橙色以上的押运次数
        e_task_info_type_jiebiao_win_count = 70,                            ///70   木牛流马劫粮成功次数
        e_task_info_type_yigu_instance_count = 72,                          ///72   进行千里单骑的次数
        e_task_info_type_pet_s_roll_count = 76,                             ///76	普通寻宝次数
        e_task_info_type_pet_b_roll_count = 77,                             ///77   高级寻宝次数
        e_task_info_type_pet_summon_count = 79,                             ///79   寻宝次数
        e_task_info_type_normal_box_open_count = 81,                        ///81	突破宝箱开启次数
        e_task_info_type_yb_roll_b_count_yuanbao_only = 110,                ///110  武将占星次数【元宝】
        e_task_info_type_task_king_attend_cnt = 116,                        ///116  参加王者之战次数
        e_task_info_type_task_guild_war_attend_cnt = 117,                   ///117  参加军团资源战次数
        e_task_info_type_task_group_ins_type_0_attend_cnt = 118,            ///118  组队副本群雄争霸次数
        e_task_info_type_task_group_ins_type_1_attend_cnt = 119,            ///119  组队副本晶石挑战次数

        e_task_info_type_db_end = 127,
        e_task_info_type_common = e_task_info_type_db_end,
        e_task_info_type_team_level = 128,                                  ///128 团队等级
        e_task_info_type_team_zdl = 153,                                    ///153 团队战力
        e_task_info_type_hero_id_b_star_level_a = 178,                      ///178 ID为b的英雄a星级

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
                descTextId = TextManager.CreateText();
                TextManager.SetText(descTextId, desc);
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
            act.desc = TextManager.GetText(descTextId);

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