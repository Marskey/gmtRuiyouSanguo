using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace gmt
{
    /// <summary>
    /// GM操作界面
    /// </summary>
    public partial class GmModify : AGmPage
    {
        /// <summary>
        /// 版本
        /// </summary>
        public const string Version = "0.0.7";

        /// <summary>
        /// 是否需要返回
        /// </summary>
        private bool needReturn = true;

        private List<string> serverStatus = new List<string>();

        /// <summary>
        /// 构造方法
        /// </summary>
        public GmModify()
            : base(PrivilegeType.GmModify)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            this.errorLabel.Text = "";

            if (!this.IsPostBack)
            {
                this.versionLabel.Text = GmModify.Version;

                // 0 经济类型
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20000), "0"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20000), "0"));
                // 1 物品
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20001), "1"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20001), "1"));
                // 2 武魂
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20003), "2"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20003), "2"));
                // 3 饰品
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22017), "3"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22017), "3"));
                // 4 晶石
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22018), "4"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22018), "4"));
                // 5 坐骑碎片
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22019), "5"));
                this.itemDelTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22019), "5"));
                // 6 武将
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(336), "6"));
                // 7 经验
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(337), "7"));
                // 8 宠物
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(725), "8"));
                // 9 坐骑
                this.itemGiveTypeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20009), "9"));

                itemGiveOptionDisplay(ref itemGiveTypeDropDownList, ref itemGiveOptionDropDownList, ref itemGiveCountTextBox);
                itemDelOptionDisplay(ref itemDelTypeDropDownList, ref itemDelOptionDropDownList, ref itemDelCountTextBox);

                //List<mw.GiftConfig> giftConfig = TableManager.Load<mw.GiftConfig>();
                //for (int i = 0; i < giftConfig.Count; i++)
                //{
                //    this.giftOptionDropDownList.Items.Add(giftConfig[i].id + " | " + giftConfig[i].title);
                //}
                if (GiftTable.GiftList != null)
                {
                    foreach (var config in GiftTable.GiftList)
                    {
                        this.giftOptionDropDownList.Items.Add(new ListItem(config.id + " | " + TextManager.GetText(config.title), config.id.ToString()));
                    }
                }

                serverStatus.Add(TableManager.GetGMTText(341));
                serverStatus.Add(TableManager.GetGMTText(342));
                serverStatus.Add(TableManager.GetGMTText(343));
                serverStatus.Add(TableManager.GetGMTText(344));
                showServerStatus();

                this.itemGiveOptionDropDownList.SelectedIndex = 0;
                this.itemGiveTypeDropDownList.SelectedIndex = 0;

                this.itemDelOptionDropDownList.SelectedIndex = 0;
                this.itemDelTypeDropDownList.SelectedIndex = 0;

                this.giftOptionDropDownList.SelectedIndex = 0;

                gmt.Server server = Session["Server"] as gmt.Server;

                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server theServer = gmt.Server.GetServerAt(i);
                    string svrName = string.Format("{0}(ID:{1})", theServer.Name, theServer.serverID);
                    this.serverListBox.Items.Add(new ListItem(svrName, i.ToString()));

                    if (theServer == server)
                    {
                        this.serverListBox.SelectedIndex = i;
                    }
                }
            }
        }

        /// <summary>
        /// 初始化按钮点击响应
        /// </summary>
        protected void initializeButton_Click(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < this.selectListBox.Items.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServerAt(i);

                List<mw.ActivityConfig> Updateactivitytable = new List<mw.ActivityConfig>();
                foreach (var pair in OtherActivityManager.ActivityDictionary)
                {
                    Updateactivitytable.Add(pair.Value);
                }
                TableManager.Send(Updateactivitytable);

                List<mw.AchieveConfig> Updateachievetable = new List<mw.AchieveConfig>();
                foreach (var pair in OtherActivityManager.AchieveDictionarydaily)
                {
                    Updateachievetable.Add(pair.Value);
                }
                foreach (var pair in OtherActivityManager.AchieveDictionarysign)
                {
                    Updateachievetable.Add(pair.Value);
                }
                TableManager.Send(Updateachievetable);

                List<mw.RewardConfig> Updaterewardtable = new List<mw.RewardConfig>();
                foreach (var pair in OtherActivityManager.RewardDictionary)
                {
                    Updaterewardtable.Add(pair.Value);
                }
                TableManager.Send(Updaterewardtable);

                TableManager.Send(GiftTable.GiftList, -1, server);

                List<mw.RandConfig> list = new List<mw.RandConfig>();
                foreach (var config in RandTable.RandList)
                {
                    if (config.rand_type != mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
                    {
                        list.Add(config);
                    }
                }
                TableManager.Send(list, (int)mw.EGMTSettintType.E_GMT_SETTINT_RAND, server);

                list = new List<mw.RandConfig>();
                foreach (var config in RandTable.RandList)
                {
                    if (config.rand_type == mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
                    {
                        list.Add(config);
                    }
                }
                TableManager.Send(list, (int)mw.EGMTSettintType.E_GMT_SETTINT_ACTIVITY_SHOP, server);
            }*/
        }

        /// <summary>
        /// 选择服务器下来列表框选中值变化时响应
        /// </summary>
        protected void serverDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            showServerStatus();
        }

        private void showServerStatus()
        {
            if (gmt.ServerListConfig.DataList != null
                && this.selectListBox.Items.Count == 1
                )
            {
                string serverText = gmt.ServerListConfig.DataList[int.Parse(selectListBox.Items[0].Value)].ServerList.GetText();
                string[] serverValue = serverText.Split(',');
                int index;
                if (serverValue != null && serverValue.Length >= 4)
                {
                    if (int.TryParse(serverValue[4], out index) && index >= 0 && index < serverStatus.Count)
                    {
                        this.serverStatusLabel.Text = serverStatus[index];
                        return;
                    }
                }
            }

            this.serverStatusLabel.Text = TableManager.GetGMTText(719);
        }

        /// <summary>
        /// 确认玩家按钮点击响应
        /// </summary>
        protected void ensureButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            StringBuilder builder = new StringBuilder();
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);
            builder.Append(server.Name).Append("<br>");

            string sql = "";

            if (!string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                sql = string.Format("SELECT `uname`,`uid`,`cyuid` FROM `user` WHERE `uid`='{0}';", DatabaseAssistant.DetectionSql(this.uidTextBox.Text));
            }

            DatabaseAssistant.Execute(reader =>
            {
                bool finded = false;
                while (reader.Read())
                {
                    finded = true;
                    builder.Append(TableManager.GetGMTText(9) + "(").Append(reader.GetString(0));
                    this.playerNameLabel.Text = reader.GetString(0);
                    builder.Append(") " + TableManager.GetGMTText(722) + "(").Append(reader.GetUInt32(1));
                    builder.Append(") " + TableManager.GetGMTText(48) + "(").Append(reader.GetString(2)).Append(")<br>");
                }

                if (!finded) { builder.Append(TableManager.GetGMTText(674) + "<br>"); }
            },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                sql);

            //显示玩家信息
            //this.playerNameLabel.Text = builder.ToString();
        }

        #region 发放道具
        /// <summary>
        /// 发放道具类型下拉列表框选中值变化时响应
        /// </summary>
        protected void itemGiveTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemGiveOptionDisplay(ref itemGiveTypeDropDownList, ref itemGiveOptionDropDownList, ref itemGiveCountTextBox);
        }

        protected void itemDelTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            itemDelOptionDisplay(ref itemDelTypeDropDownList, ref itemDelOptionDropDownList, ref itemDelCountTextBox);
        }

        /// <summary>
        /// 发放道具选择下拉列表框显示内容
        /// </summary>
        public static void itemGiveOptionDisplay(ref DropDownList typeDropList, ref DropDownList dropList, ref TextBox txtBox)
        {
            dropList.Items.Clear();
            txtBox.Enabled = true;
            int type = int.Parse(typeDropList.SelectedValue);
            switch (type)
            {
                // 0 经济类型
                case 0:
                // 1 物品
                case 1:
                // 2 武魂
                case 2:
                // 3 饰品
                case 3:
                // 4 晶石
                case 4:
                // 5 坐骑碎片
                case 5: itemDelOptionDisplay(ref typeDropList, ref dropList, ref txtBox); break;
                // 6 武将
                case 6:
                    {
                        itemDelOptionDisplay(ref typeDropList, ref dropList, ref txtBox);
                        txtBox.Text = "1";
                        txtBox.Enabled = false;
                    }
                    break;
                // 7 经验
                case 7:
                    {
                        dropList.Items.Add(TableManager.GetGMTText(338));
                        dropList.Items.Add(TableManager.GetGMTText(339));
                    }
                    break;
                // 8 宠物
                case 8:
                    {
                        foreach (var pair in TableManager.PetTable)
                        {
                            string text = TextManager.GetText(pair.Value.name) + "[" + pair.Value.petstar + " STAR]" + "(" + pair.Value.idx + ")";
                            dropList.Items.Add(new ListItem(text, pair.Value.idx.ToString()));
                        }
                    }
                    break;
                // 9 坐骑
                case 9:
                    {
                        itemDelOptionDisplay(ref typeDropList, ref dropList, ref txtBox);
                        txtBox.Text = "1";
                        txtBox.Enabled = false;
                    }
                    break;
            }
        }

        private static void itemDelOptionDisplay(ref DropDownList typeDropList, ref DropDownList dropList, ref TextBox txtBox)
        {
            dropList.Items.Clear();
            txtBox.Enabled = true;
            int type = int.Parse(typeDropList.SelectedValue);
            switch (type)
            {
                // 0 经济类型
                case 0:
                    foreach (var pair in playerHistroy.economicName)
                    {
                        if (pair.Key == 0) { continue; }
                        string text = TableManager.GetGMTText(21000 + (pair.Key)) + "(" + pair.Key + ")";
                        dropList.Items.Add(new ListItem(text, pair.Key.ToString()));
                    }
                    break;
                // 1 物品
                case 1:
                    foreach (var pair in TableManager.ItemTable)
                    {
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        dropList.Items.Add(new ListItem(text, pair.Value.id.ToString()));
                    }
                    break;
                // 2 武魂
                case 2:
                case 6:
                    foreach (var pair in TableManager.HeroTable)
                    {
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        dropList.Items.Add(new ListItem(text, pair.Value.id.ToString()));
                    }
                    break;
                // 3 饰品
                case 3:
                    foreach (var pair in TableManager.StoneTable)
                    {
                        string[] color = { "0", "1", "blue", "purple", "orange", "red" };
                        string text = TextManager.GetText(pair.Value.name) + "[" + color[pair.Value.color] + "]" + "(" + pair.Value.id + ")";
                        dropList.Items.Add(new ListItem(text, pair.Value.id.ToString()));
                    }
                    break;
                // 4 晶石
                case 4:
                    foreach (var pair in TableManager.PetStoneTable)
                    {
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        dropList.Items.Add(new ListItem(text, pair.Value.id.ToString()));
                    }
                    break;
                // 5 坐骑碎片
                case 5:
                case 9:
                    Regex r = new Regex("\\[.*?\\]");
                    foreach (var pair in TableManager.MountTable)
                    {
                        string text = TextManager.GetText(pair.Value.name) + "(" + pair.Value.id + ")";
                        text = r.Replace(text, "");
                        dropList.Items.Add(new ListItem(text, pair.Value.id.ToString()));
                    }
                    break;
            }
        }

        /// <summary>
        /// 确认发放按钮点击响应
        /// </summary>
        protected void givePropButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            uint playerId = this.GetPlayerId();
            int count = 0;

            string gmCommand = string.Empty;

            if (!int.TryParse(this.itemGiveCountTextBox.Text, out count) || count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(732);
                return;
            }

            if (playerId != 0)
            {
                this.needReturn = true;
                switch (this.itemGiveTypeDropDownList.SelectedIndex)
                {
                    // 0 经济类型
                    case 0:
                        gmCommand = string.Format("AM({0},{1})", this.itemGiveOptionDropDownList.SelectedItem.Value, count);
                        break;
                    // 1 物品
                    case 1:
                        gmCommand = string.Format("NI({0},{1})", this.itemGiveOptionDropDownList.SelectedItem.Value, count);
                        break;
                    // 2 武魂
                    case 2:
                        gmCommand = string.Format("NHS({0},{1})", this.itemGiveOptionDropDownList.SelectedItem.Value, count);
                        break;
                    // 3 饰品
                    case 3:
                        gmCommand = string.Format("NS({0})", this.itemGiveOptionDropDownList.SelectedItem.Value);
                        break;
                    // 4 晶石
                    case 4:
                        gmCommand = string.Format("NPS({0},{1})", this.itemGiveOptionDropDownList.SelectedItem.Value, count);
                        break;
                    // 5 坐骑碎片
                    case 5:
                        gmCommand = string.Format("NMC({0},{1})", this.itemGiveOptionDropDownList.SelectedItem.Value, count);
                        break;
                    // 6 武将
                    case 6:
                        gmCommand = string.Format("NH({0})", this.itemGiveOptionDropDownList.SelectedItem.Value);
                        break;
                    // 7 经验
                    case 7:
                        {
                            int nTeamExp = 0, nHeroExp = 0;
                            if (this.itemGiveOptionDropDownList.SelectedIndex == 0)
                            {
                                nTeamExp = count;
                            }
                            else
                            {
                                nHeroExp = count;
                            }

                            gmCommand = string.Format("ATE({0},{1})", nTeamExp, nHeroExp);
                        }
                        break;
                    // 8 宠物
                    case 8:
                        gmCommand = string.Format("NP({0})", this.itemGiveOptionDropDownList.SelectedItem.Value);
                        break;
                    // 9 坐骑
                    case 9:
                        gmCommand = string.Format("NM({0})", this.itemGiveOptionDropDownList.SelectedItem.Value);
                        break;
                }

                string account = Session["user"] as string;
                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), gmCommand, "", true, text => this.errorLabel.Text += text);
            }

        }

        protected void delItemButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            uint playerId = this.GetPlayerId();
            int count = 0;

            string gmCommand = string.Empty;

            if (!int.TryParse(this.itemDelCountTextBox.Text, out count) || count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(732);
                return;
            }

            if (playerId != 0)
            {
                this.needReturn = true;
                int type = GetRwdID4DelItemIdx(this.itemDelTypeDropDownList.SelectedIndex);
                if (type == -1)
                    return;
                gmCommand = string.Format("DI({0},{1},{2})", type, this.itemDelOptionDropDownList.SelectedItem.Value, count);
                string account = Session["user"] as string;
                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), gmCommand, "", true, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 返回道具ID
        /// </summary>
        private int GetPropID(string propName, int type)
        {
            switch (type)
            {
                //英雄
                case 0:
                    List<mw.HeroBaseConfig> hbc = TableManager.Load<mw.HeroBaseConfig>();
                    for (int i = 0; i < hbc.Count; i++)
                    {
                        if (propName.Equals(TextManager.GetText(hbc[i].name)))
                            return hbc[i].id;
                    }
                    break;
                //物品
                case 1:
                    List<mw.ItemConfig> ic = TableManager.Load<mw.ItemConfig>();
                    for (int i = 0; i < ic.Count; i++)
                    {
                        if (propName.Equals(TextManager.GetText(ic[i].name)))
                            return ic[i].id;
                    }
                    break;
            }
            return -1;
        }

        private int GetRwdID4DelItemIdx(int index)
        {
            switch (index)
            {
                // 0 经济类型
                case 0: return (int)mw.Enums.RewardType.RWD_TYPE_ECONOMIC;
                // 1 物品
                case 1: return (int)mw.Enums.RewardType.RWD_TYPE_ITEM;
                // 2 武魂
                case 2: return (int)mw.Enums.RewardType.RWD_TYPE_HERO_SOUL;
                // 3 饰品
                case 3: return (int)mw.Enums.RewardType.RWD_TYPE_STONE;
                // 4 晶石
                case 4: return (int)mw.Enums.RewardType.RWD_TYPE_PET_STONE;
                // 5 坐骑碎片
                case 5: return (int)mw.Enums.RewardType.RWD_TYPE_MOUNT_CL;
            }
            return -1;
        }

        #endregion

        /// <summary>
        /// 清理角色按钮点击响应
        /// </summary>
        protected void clearButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            string account = Session["user"] as string;
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), "DP()", "", true, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 跳过新手按钮点击响应
        /// </summary>
        protected void skipNewbieButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            string account = Session["user"] as string;
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), string.Format("CO({0},1)", playerId), "", true, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// VIP按钮点击响应
        /// </summary>
        protected void vipButton_Click(object sender, EventArgs e)
        {

        }

        #region 发放礼包

        /// <summary>
        /// 礼包下拉列表框选中值变化时响应
        /// </summary>
        protected void giftOptionDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GiftTable.GiftList != null)
                this.giftLabel.Text = "*" + TextManager.GetText(GiftTable.GiftList[this.giftOptionDropDownList.SelectedIndex].desc);
        }

        /// <summary>
        /// 发放单人礼包按钮点击响应
        /// </summary>
        protected void singleGiftButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            string account = Session["user"] as string;
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            int giftId = 0;
            if (!int.TryParse(this.giftOptionDropDownList.SelectedValue, out giftId))
            {
                return;
            }

            uint playerId = this.GetPlayerId();

            if (playerId == 0)
            {
                return;
            }

            string gmcmd = string.Format("GIFT({0},{1})", playerId, giftId);

            bool isGiftTimeOn = (Request.Form["gift_timer_checker"] == "on") ? true : false;
            string give_time = this.datetimepicker_1.Value;

            if (isGiftTimeOn)
            {
                STTimedMail stMailInfo = new STTimedMail();
                stMailInfo.uid = playerId;
                stMailInfo.mailId = giftId;
                stMailInfo.serverName = this.selectListBox.Items[0].Value;
                stMailInfo.sendTime = CUtils.GetTimestamp(Convert.ToDateTime(give_time));
                stMailInfo.cmd = gmcmd;
                TimedMailSender.AddTimedMail(stMailInfo);
            }
            else
            {
                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), gmcmd, "", true, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 发放全员礼包按钮点击响应
        /// </summary>
        protected void wholeGiftButton_Click(object sender, EventArgs e)
        {
            int giftId = 0;
            if (!int.TryParse(this.giftOptionDropDownList.SelectedValue, out giftId))
            {
                return;
            }

            uint playerId = 0;

            bool isGiftTimeOn = (Request.Form["gift_timer_checker"] == "on") ? true : false;
            string give_time = this.datetimepicker_1.Value;
            string gmcmd = string.Format("GIFT({0},{1})", playerId, giftId);

            string account = Session["user"] as string;
            for (int i = 0; i < selectListBox.Items.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServer(selectListBox.Items[i].Text);
                if (server == null)
                {
                    this.errorLabel.Text += TableManager.GetGMTText(733) + ":" + selectListBox.Items[i].Text;
                    continue;
                }

                if (isGiftTimeOn)
                {
                    STTimedMail stMailInfo = new STTimedMail();
                    stMailInfo.uid = playerId;
                    stMailInfo.mailId = giftId;
                    stMailInfo.serverName = selectListBox.Items[i].Text;
                    stMailInfo.sendTime = CUtils.GetTimestamp(Convert.ToDateTime(give_time));
                    stMailInfo.cmd = gmcmd;
                    TimedMailSender.AddTimedMail(stMailInfo);
                }
                else
                {
                    AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), gmcmd, "", true, text => this.errorLabel.Text += text);
                }
            }
        }

        #endregion

        /// <summary>
        /// GM命令按钮点击响应
        /// </summary>
        protected void gmButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            string account = Session["user"] as string;
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            uint playerId = this.GetPlayerId(false);

            if (playerId != 0)
            {
                //转大写
                int starIdx = this.gmTextBox.Text.IndexOfAny("(".ToCharArray());
                string textA = this.gmTextBox.Text.Substring(0, starIdx);
                string textB = this.gmTextBox.Text.Substring(starIdx);
                string commond = textA.ToUpper() + textB;

                AGmPage.ExecuteGmCommand(account, server, playerId.ToString(), commond, "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 关服按钮点击响应
        /// </summary>
        protected void shutdownButton_Click(object sender, EventArgs e)
        {
            if (selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
            }

            string account = Session["user"] as string;
            for (int i = 0; i < selectListBox.Items.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServer(selectListBox.Items[i].Text);
                if (server == null)
                {
                    this.errorLabel.Text += TableManager.GetGMTText(733) + ":" + selectListBox.Items[i].Text;
                    continue;
                }

                AGmPage.ExecuteGmCommand(account, server, "0", "SDN()", "", true, text => this.errorLabel.Text += text);
            }
        }

        public bool HasFlag(int value, int pos)
        {
            return (value & (1 << pos)) != 0;
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        /// <param name="playerId">玩家编号</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="operateText">操作文本</param>
        /// <param name="needReturn">是否需要返回</param>
        /// <param name="reportProcess">报告处理</param>
        /// <returns>是否成功</returns>
        //protected override bool ExecuteGmCommand(string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        //{
        //    if (this.serverDropDownList.Items.Count == 0)
        //    {
        //        this.errorLabel.Text = "没有选择服务器";
        //        return false;
        //    }

        //    bool success = true;
        //    string account = Session["user"] as string;

        //    var item = this.serverDropDownList.SelectedItem;
        //    gmt.Server server = gmt.Server.GetServer(item.ToString());

        //    if (server != null)
        //    {
        //        success &= AGmPage.ExecuteGmCommand(account, server, playerId, commandText, operateText, needReturn, reportProcess);
        //    }

        //    return success;
        //}

        /// <summary>
        /// 获取玩家编号
        /// </summary>
        /// <param name="needReport">是否需要报告</param>
        /// <returns>玩家编号</returns>
        private uint GetPlayerId(bool needReport = true)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return 0;
            }

            if (string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return 0;
            }

            StringBuilder builder = new StringBuilder();
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);

            if (server == null)
            {
                if (needReport) { this.errorLabel.Text = TableManager.GetGMTText(736); }
                return 0;
            }

            uint playerId = 0;

            string sql;

            if (!string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                sql = string.Format("SELECT `uid` FROM `user` WHERE `uid`='{0}';", DatabaseAssistant.DetectionSql(this.uidTextBox.Text));
            }
            else
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return 0;
            }

            DatabaseAssistant.Execute(reader =>
            {
                if (reader.Read()) { playerId = reader.GetUInt32(0); }
            },
            server.DatabaseAddress,
            server.DatabasePort,
            server.DatabaseCharSet,
            server.GameDatabase,
            server.DatabaseUserId,
            server.DatabasePassword,
            sql);

            //else if (this.playerChannelIdTextBox.Text != null && this.playerChannelIdTextBox.Text != "")
            //{
            //	DatabaseAssistant.Execute(reader =>
            //	{
            //		if (reader.Read()) { playerId = reader.GetUInt32(0); }
            //	},
            //	server.DatabaseAddress,
            //	server.DatabasePort,
            //	server.DatabaseCharSet,
            //	server.GmAddress,
            //	server.DatabaseUserId,
            //	server.DatabasePassword,
            //	"SELECT `uid` FROM `user` WHERE `cyuid`='{0}';", DatabaseAssistant.DetectionSql(this.playerChannelIdTextBox.Text));
            //}

            if (playerId == 0 && needReport)
            {
                this.errorLabel.Text += TableManager.GetGMTText(737) + "\r\n";
            }

            return playerId;
        }

        protected void rechargeButton_Click(object sender, EventArgs e)
        {
            try
            {
                HttpWebRequest request = WebRequest.Create("http://192.168.1.26:8081/bill") as HttpWebRequest;

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                byte[] byteArray = Encoding.UTF8.GetBytes("pushInfo=order:测试orderPushInfo&receipt=eyJhcmVhTmFtZSI6IiAiLCJjaGFubmVsSWQiOiIzMDUwIiwiZ29vZHNOdW1iZXIiOiI2MCIsInVzZXJpZCI6IjcxMzA1MDM5Mjg2OCIsImdvb2RzUHJpY2UiOjYsInJvbGVOYW1lIjoiICIsImdvb2RzUmVnaXN0ZXJJZCI6Ijg1Njc4MDAyIiwiYXBwRGF0ZSI6IjIwMTUwNDEzMDE1NzA2Iiwib3JkZXJJZCI6IjE1MDQxMzAxNTYzNzM0MjEyNyIsImFyZWFJZCI6IjUtMS02LTEiLCJyb2xlSWQiOiI0MzM2NiJ9&sign=31153d9089c07d739c6afe3900c93376");
                request.ContentLength = byteArray.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();

                HttpWebResponse respone = request.GetResponse() as HttpWebResponse;
                using (StreamReader reader = new StreamReader(respone.GetResponseStream()))
                {
                    string text = reader.ReadToEnd();

                    if (text != null && text != "")
                    {
                        this.errorLabel.Text += text;
                    }
                    else
                    {
                        this.errorLabel.Text += TableManager.GetGMTText(391);
                    }
                }
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                this.errorLabel.Text += exception.ToString();
            }
        }

        //计算时间 
        public DateTime Getdaytime(int time)
        {
            double a = (double)time;
            TimeSpan st = new TimeSpan();
            st = TimeSpan.FromSeconds(a);
            double b = 8 * 60 * 60;
            TimeSpan sd = new TimeSpan();
            sd = TimeSpan.FromSeconds(b);
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
            DateTime now = new DateTime();
            now = dt.Add(st);
            now = now.Add(sd);
            return now;
        }

        /// <summary>
        /// 查询ID按钮点击响应
        /// </summary>
        //        protected void findIdButton_Click(object sender, EventArgs e)
        //        {
        //            if (this.serverDropDownList.Items.Count == 0)
        //            {
        //                this.errorLabel.Text = "没有选择服务器";
        //                return;
        //            }

        //            if (string.IsNullOrEmpty(this.uidTextBox.Text))
        //            {
        //                this.errorLabel.Text = "没有输入玩家信息";
        //                return;
        //            }

        //            StringBuilder builder = new StringBuilder();
        //            for (int i = 0; i < this.serverDropDownList.Items.Count; ++i)
        //            {
        //                gmt.Server server = gmt.Server.GetServer(this.serverDropDownList.Items[i].Text);

        //                builder.Append(server.Name).Append("<br>");

        //                string sql = "";

        //                if (!string.IsNullOrEmpty(this.uidTextBox.Text))
        //                {
        //                    sql = string.Format("SELECT `uname`,`uid`,`cyuid` FROM `user` WHERE `uid`='{0}';",

        //DatabaseAssistant.DetectionSql(this.uidTextBox.Text));
        //                }

        //                DatabaseAssistant.Execute(reader =>
        //                {
        //                    bool finded = false;
        //                    while (reader.Read())
        //                    {
        //                        finded = true;
        //                        builder.Append("玩家名称(").Append(reader.GetString(0));
        //                        builder.Append(") 游戏ID(").Append(reader.GetUInt32(1));
        //                        builder.Append(") 平台ID(").Append(reader.GetString(2)).Append(")<br>");
        //                    }

        //                    if (!finded) { builder.Append("找不到该用户<br>"); }
        //                },
        //                server.DatabaseAddress,
        //                server.DatabasePort,
        //                server.DatabaseCharSet,
        //                server.GameDatabase,
        //                server.DatabaseUserId,
        //                server.DatabasePassword,
        //                sql);
        //            }

        //            //builder.ToString();
        //        }


        //protected void stoneButton_Click(object sender, EventArgs e)
        //{
        //    uint playerId = this.GetPlayerId();

        //    if (playerId != 0)
        //    {
        //        this.ExecuteGmCommand(playerId.ToString(), string.Format("NS({0})", this.stoneButtonText.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        //    }
        //}

        //protected void ZSEquip_Click(object sender, EventArgs e)
        //{
        //    uint playerId = this.GetPlayerId();

        //    if (playerId != 0)
        //    {
        //        this.ExecuteGmCommand(playerId.ToString(), string.Format("NE({0})", this.ZSEquipText.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        //    }
        //}

        //protected void petButton_Click(object sender, EventArgs e)
        //{

        //    uint playerId = this.GetPlayerId();

        //    if (playerId != 0)
        //    {
        //        this.ExecuteGmCommand(playerId.ToString(), string.Format("NP({0})", this.petIdTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        //    }
        //}

        //protected void petstoneButton_Click(object sender, EventArgs e)
        //{

        //    uint playerId = this.GetPlayerId();

        //    if (playerId != 0)
        //    {
        //        this.ExecuteGmCommand(playerId.ToString(), string.Format("NPS({0},1)", this.petstoneButtonText.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        //    }

        //}

        /// <summary>
        /// 添加全部钮点击响应
        /// </summary>
        protected void addAllButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.Items.Count == 0) { return; }

            foreach (var item in this.serverListBox.Items)
            {
                this.selectListBox.Items.Add(item.ToString());
            }

            this.serverListBox.Items.Clear();
        }

        /// <summary>
        /// 添加选择按钮点击响应
        /// </summary>
        protected void addSelectButton_Click(object sender, EventArgs e)
        {
            int[] selectSet = this.serverListBox.GetSelectedIndices();
            if (selectSet == null || selectSet.Length == 0) { return; }

            for (int i = selectSet.Length - 1; i >= 0; --i)
            {
                this.selectListBox.Items.Add(this.serverListBox.Items[selectSet[i]].Text);
                this.serverListBox.Items.RemoveAt(selectSet[i]);
            }
        }

        /// <summary>
        /// 移除选择按钮点击响应
        /// </summary>
        protected void removeSelectButton_Click(object sender, EventArgs e)
        {
            int[] selectSet = this.selectListBox.GetSelectedIndices();
            if (selectSet == null || selectSet.Length == 0) { return; }

            for (int i = selectSet.Length - 1; i >= 0; --i)
            {
                this.serverListBox.Items.Add(this.selectListBox.Items[selectSet[i]].Text);
                this.selectListBox.Items.RemoveAt(selectSet[i]);
            }
        }

        /// <summary>
        /// 移除全部钮点击响应
        /// </summary>
        protected void removeAllButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0) { return; }

            foreach (var item in this.selectListBox.Items)
            {
                this.serverListBox.Items.Add(item.ToString());
            }

            this.selectListBox.Items.Clear();
        }

        [WebMethod(EnableSession = true)]
        public static string GetTimedMailData()
        {
            return TimedMailSender.GetMailList();
        }

        [WebMethod(EnableSession = true)]
        public static string DelTimedMail(int Id)
        {
            if (TimedMailSender.RemoveTimedMail(Id))
            {
                return "{\"error\":0, \"msg\": \"success\"}";
            }
            return "{\"error\":1, \"msg\": \"fail\"}";
        }

        protected void jsonDataButton_Click(object sender, EventArgs e)
        {
            //if (this.selectListBox.Items.Count == 0)
            //{
            //    this.errorLabel.Text = TableManager.GetGMTText(734);
            //    return;
            //}

            //Dictionary<string, int>[] jsonData = JsonConvert.DeserializeObject<Dictionary<string, int>[]>(jsondataTextBox.Text);
            //if (null == jsonData)
            //{
            //    this.errorLabel.Text = "json data error";
            //    return;
            //}

            //foreach (var data in jsonData)
            //{
            //    foreach (var item in this.selectListBox.Items)
            //    {
            //        gmt.Server server = gmt.Server.GetServer(item.ToString());
            //        if (server != null)
            //        {
            //            string gmcmd = string.Format("AM(2, {0})", data["count"] * 300);
            //            AGmPage.ExecuteGmCommand("SYSTEM", server, data["uid"].ToString(), gmcmd, "", true, null);
            //        }
            //    }
            //}
        }

        protected void firstChargeResetButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(734);
                return;
            }

            foreach (var item in this.selectListBox.Items)
            {
                gmt.Server server = gmt.Server.GetServer(item.ToString());
                if (server != null)
                {
                    string gmcmd = "RDATE()";
                    AGmPage.ExecuteGmCommand("SYSTEM", server, "0", gmcmd, "", true, null);
                }
            }
        }

        protected void btn_swap_openid_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count != 1
                || string.IsNullOrEmpty(this.selectListBox.Items[0].Value))
            {
                this.errorLabel.Text = TableManager.GetGMTText(720);
                return;
            }

            uint uid_1 = 0;
            uint.TryParse(Request.Form["input_swap_uid_1"].ToString(), out uid_1);
            uint uid_2 = 0;
            uint.TryParse(Request.Form["input_swap_uid_2"].ToString(), out uid_2);

            if (0 == uid_1)
            {
                this.errorLabel.Text = "uid_1 Cannot be null or 0";
            }

            if (0 == uid_2)
            {
                this.errorLabel.Text = "uid_2 Cannot be null or 0";
            }

            string device_id_1 = "", device_id_2 = "";
            string open_id_1 = "", open_id_2 = "";
            string cyuid_1 = "", cyuid_2 = "";

            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].Value);
            string sql = string.Format("SELECT t.uid, device_id, open_id, t.cyuid FROM sglogindb_kr.`tb_user_info` a RIGHT JOIN (SELECT uid, cyuid FROM USER WHERE uid IN ({0}, {1}))t ON a.cyuid = t.cyuid;", uid_1, uid_2);
            DatabaseAssistant.Execute(reader =>
            {
                while (reader.Read())
                {
                    if (reader.GetInt32(0) == uid_1)
                    {
                        device_id_1 = reader.GetString(1);
                        open_id_1 = reader.GetString(2);
                        cyuid_1 = reader.GetString(3);
                    }

                    if (reader.GetInt32(0) == uid_2)
                    {
                        device_id_2 = reader.GetString(1);
                        open_id_2 = reader.GetString(2);
                        cyuid_2 = reader.GetString(3);
                    }
                }
            },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                sql);

            if (string.IsNullOrEmpty(device_id_1))
            {
                this.errorLabel.Text = "cannot find player! uid: " + uid_1;
                return;
            }

            if (string.IsNullOrEmpty(device_id_2))
            {
                this.errorLabel.Text = "cannot find player! uid: " + uid_2;
                return;
            }

            //if (device_id_1 != device_id_2)
            //{
            //    this.errorLabel.Text = "device id is not the same!";
            //    return;
            //}

            sql = string.Format("update sglogindb_kr.tb_user_info set open_id = '{0}' where cyuid = '{1}'", open_id_2, cyuid_1);
            DatabaseAssistant.Execute(reader =>
            {
            },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                sql);

            sql = string.Format("update sglogindb_kr.tb_user_info set open_id = '{0}' where cyuid = '{1}'", open_id_1, cyuid_2);
            DatabaseAssistant.Execute(reader =>
            {
            },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                sql);

            this.errorLabel.Text = "SWAP SUCCESS!";
        }
    }
}