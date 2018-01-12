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

namespace gm
{
    /// <summary>
    /// GM操作界面
    /// </summary>
    public partial class Gm : AGmPage
    {
        /// <summary>
        /// 版本
        /// </summary>
        public const string Version = "1.1.0";

        /// <summary>
        /// 是否需要返回
        /// </summary>
        private bool needReturn = true;

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            this.errorLabel.Text = "";

            if (!this.IsPostBack)
            {
                this.versionLabel.Text = Gm.Version;

                for (int i = 0; i < gm.Server.Count; ++i)
                {
                    gm.Server theServer = gm.Server.GetServerAt(i);
                    this.serverListBox.Items.Add(new ListItem(theServer.Name));
                }

                this.rankTypeDropDownList.Items.Add("战斗力");
                this.rankTypeDropDownList.Items.Add("PVP");
                this.rankTypeDropDownList.Items.Add("等级");
                this.rankTypeDropDownList.Items.Add("宠物");
                this.rankTypeDropDownList.Items.Add("英雄");
            }
        }

        /// <summary>
        /// 初始化按钮点击响应
        /// </summary>
        protected void initializeButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "没有选择服务器";
                return;
            }

            for (int i = 0; i < this.selectListBox.Items.Count; ++i)
            {
                gm.Server server = gm.Server.GetServerAt(i);

                List<mw.ActivityConfig> Updateactivitytable = new List<mw.ActivityConfig>();
                foreach (var pair in OtherActivityManager.ActivityDictionary)
                {
                    Updateactivitytable.Add(pair.Value);
                }
                TableManager.Send(Updateactivitytable);

                List<mw.AchieveConfig> Updateachievetable = new List<mw.AchieveConfig>();
                foreach (var pair in OtherActivityManager.AchieveDictionary)
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
            }
        }

        /// <summary>
        /// 卡牌按钮点击响应
        /// </summary>
        protected void cardButton_Click(object sender, EventArgs e)
        {
            if (this.cardAllCheckBox.Checked)
            {
                if (this.selectListBox.Items.Count == 0)
                {
                    this.errorLabel.Text = "没有选择服务器";
                    return;
                }

                this.needReturn = false;
                foreach (var item in this.selectListBox.Items)
                {
                    gm.Server server = gm.Server.GetServer(item.ToString());

                    if (server != null)
                    {
                        DatabaseAssistant.Execute(reader =>
                        {
                            while (reader.Read())
                            {
                                uint id = reader.GetUInt32(0);
                                string cyuid = reader.GetString(1);

                                this.ExecuteGmCommand(id.ToString(), string.Format("NC({0})", this.cardIdTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);

                                Thread.Sleep(100);
                            }
                        },
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "SELECT `uid`,`cyuid` FROM `user`;");
                    }
                }
                this.needReturn = true;
            }
            else
            {
                uint playerId = this.GetPlayerId();

                if (playerId != 0)
                {
                    this.ExecuteGmCommand(playerId.ToString(), string.Format("NC({0})", this.cardIdTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
                }
            }
        }

        /// <summary>
        /// 经验按钮点击响应
        /// </summary>
        protected void expButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AE({0},{1})", this.teamExpTextBox.Text, this.heroExpTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 物品按钮点击响应
        /// </summary>
        protected void itemButton_Click(object sender, EventArgs e)
        {
            if (this.itemAllCheckBox.Checked)
            {
                if (this.selectListBox.Items.Count == 0)
                {
                    this.errorLabel.Text = "没有选择服务器";
                    return;
                }

                this.needReturn = false;
                foreach (var item in this.selectListBox.Items)
                {
                    gm.Server server = gm.Server.GetServer(item.ToString());

                    if (server != null)
                    {
                        DatabaseAssistant.Execute(reader =>
                        {
                            if (reader.Read())
                            {
                                uint id = reader.GetUInt32(0);
                                this.ExecuteGmCommand(id.ToString(), string.Format("NI({0},{1})", this.itemIdTextBox.Text, this.itemCountTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
                            }
                        },
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "SELECT `uid` FROM `user`;");
                    }
                }
                this.needReturn = true;
            }
            else
            {
                uint playerId = this.GetPlayerId();

                if (playerId != 0)
                {
                    this.ExecuteGmCommand(playerId.ToString(), string.Format("NI({0},{1})", this.itemIdTextBox.Text, this.itemCountTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
                }
            }
        }

        /// <summary>
        /// 碎片按钮点击响应
        /// </summary>
        protected void peaceButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("NCP({0},{1})", this.peaceIdTextBox.Text, this.peaceCountTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 金钱按钮点击响应
        /// </summary>
        protected void moneyButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(2,{0})", this.moneyTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 元宝按钮点击响应
        /// </summary>
        protected void tokenButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(3,{0})", this.tokenTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 技能点按钮点击响应
        /// </summary>
        protected void skillPointButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(5,{0})", this.skillPointTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 友情点按钮点击响应
        /// </summary>
        protected void friendPointButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(8,{0})", this.friendPointTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /*
        //点击大月卡
        protected void BMonCardButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.BMonCardTextBox.Text))
            {
                this.reportLabel.Text = "请输入大月卡的天数";
                return;
            }
            uint playerId = this.GetPlayerId();
            if(playerId!=0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(16,{0})",this.BMonCardTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text); 
            }
           
        }
        //点击小月卡
        protected void SMonCardButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.SMonCardTextBox.Text))
            {
                this.reportLabel.Text = "请输入小月卡的天数";
                return;
            }
            uint playerId = this.GetPlayerId();
            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), string.Format("AM(15,{0})",this.SMonCardTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
            }
           
        }
        */

        /// <summary>
        /// 排行榜奖励按钮点击响应
        /// </summary>
        protected void rankRewardButton_Click(object sender, EventArgs e)
        {
            int gift;
            if (!int.TryParse(this.rankGiftTextBox.Text, out gift) || gift == 0)
            {
                this.errorLabel.Text = "礼包ID错误";
                return;
            }

            this.ExecuteGmCommand("0", string.Format("GRANK({0},{1})", this.rankTypeDropDownList.SelectedIndex, gift), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 禁言按钮点击响应
        /// </summary>
        protected void shutupButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("SHUTUP({0},1)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 解禁按钮点击响应
        /// </summary>
        protected void notShutupButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("SHUTUP({0},0)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 清理角色按钮点击响应
        /// </summary>
        protected void clearButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand(playerId.ToString(), "DP()", "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 跳过新手按钮点击响应
        /// </summary>
        protected void skipNewbieButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("CO({0},1)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 踢人按钮点击响应
        /// </summary>
        protected void kickButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("KICK({0})", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 在线按钮点击响应
        /// </summary>
        protected void onlineButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand("0", string.Format("SP({0})", this.onlineTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 预创建按钮点击响应
        /// </summary>
        protected void preButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand("0", string.Format("SPC({0})", this.preTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 封号按钮点击响应
        /// </summary>
        protected void banButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("LOGINF({0},1)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 解封按钮点击响应
        /// </summary>
        protected void unbanButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("LOGINF({0},0)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// VIP按钮点击响应
        /// </summary>
        protected void vipButton_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 公告按钮点击响应
        /// </summary>
        protected void noticeButton_Click(object sender, EventArgs e)
        {
            if (this.noticeTextBox.Text == null || this.noticeTextBox.Text == "")
            {
                return;
            }

            this.ExecuteGmCommand("0", string.Format("SC(\"{0}\")", this.noticeTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 发礼包按钮点击响应
        /// </summary>
        protected void giftButton_Click(object sender, EventArgs e)
        {
            int giftId = 0;
            if (!int.TryParse(this.giftTextBox.Text, out giftId))
            {
                return;
            }

            uint playerId = 0;

            if (!this.allCheckBox.Checked)
            {
                playerId = this.GetPlayerId();

                if (playerId == 0)
                {
                    return;
                }
            }

            this.ExecuteGmCommand("0", string.Format("GIFT({0},{1})", playerId, giftId), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 阻止按钮点击响应
        /// </summary>
        protected void stopButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand("0", string.Format("SIPC(1)"), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 不阻止按钮点击响应
        /// </summary>
        protected void notStopButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand("0", string.Format("SIPC(0)"), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 白名单按钮点击响应
        /// </summary>
        protected void whiteListButton_Click(object sender, EventArgs e)
        {
            string[] partSet = this.whileListTextBox.Text.Split('.');

            if (partSet.Length != 4)
            {
                this.errorLabel.Text += "IP格式不正确\r\n";
                return;
            }

            foreach (string part in partSet)
            {
                int number = 0;
                if (!int.TryParse(part, out number) || number < 0 || number > 255)
                {
                    this.errorLabel.Text += "IP格式不正确\r\n";
                    return;
                }
            }

            this.ExecuteGmCommand("0", string.Format("SIP(\"{0}\")", this.whileListTextBox.Text), "", this.needReturn, text => this.errorLabel.Text += text);
        }

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
        /// 添加服务器钮点击响应
        /// </summary>
        protected void addServerButton_Click(object sender, EventArgs e)
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
        /// 移除服务器钮点击响应
        /// </summary>
        protected void removeServerButton_Click(object sender, EventArgs e)
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

        /// <summary>
        /// GM命令按钮点击响应
        /// </summary>
        protected void gmButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId(false);

            if (playerId != 0 || string.IsNullOrEmpty(this.playerNameTextBox.Text))
            {
                this.ExecuteGmCommand(playerId.ToString(), this.gmTextBox.Text, "", this.needReturn, text => this.errorLabel.Text += text);
            }
        }

        /// <summary>
        /// 关服按钮点击响应
        /// </summary>
        protected void shutdownButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand("0", string.Format("SHUTDOWN()"), "", this.needReturn, text => this.errorLabel.Text += text);
        }

        /// <summary>
        /// 查询ID按钮点击响应
        /// </summary>
        protected void findIdButton_Click(object sender, EventArgs e)
        {
            //情况显示信息
            this.VipLevelText.Text = "";
            this.OnlineTimeText.Text = "";
            this.yuanbao_all.Text = "";
            this.RolecreateTimeText.Text = "";
            this.DescPlayeruidText.Text = "";
            this.MoneyText.Text = "";
            this.YubaoText.Text = "";
            this.EnergeText.Text = "";
            this.SkillpointText.Text = "";
            this.ResorvepointText.Text = "";
            this.ActivepointText.Text = "";
            this.HonorText.Text = "";
            this.SkillpointTimeText.Text = "";
            this.ExpText.Text = "";
            this.LevelText.Text = "";
            this.TTTCurCengText.Text = "";
            this.TTTMaxCengText.Text = "";
            this.TTTResetTimeText.Text = "";
            this.FubenprogressText.Text = "";
            this.TalentlevelText.Text = "";
            this.PVPText.Text = "";
            this.EnergeTimeText.Text = "";
            this.CardSlotNumText.Text = "";
            this.BigMonthText.Text = "";
            this.SmallMonthText.Text = "";
            this.QingyuanText.Text = "";
            //this.RoleflagText.Text = "";
            //this.FreechoujiangText.Text = "";
            //this.LastDateText.Text = "";
            //this.IndexText.Text = "";
            this.VipChalengeText.Text = "";
            this.VipEnergeText.Text = "";
            this.VipMoneyText.Text = "";
            this.VipSkillpointText.Text = "";
            this.VipYuanbaoText.Text = "";
            this.Cardinfolabel.Text = "";
            this.Chipinfolabel.Text = "";
            this.Friendinfolabel.Text = "";
            this.Petinfolabel.Text = "";
            //this.gift_flaglabel.Text = "";
            //this.giftinfoslabel.Text = "";


            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "没有选择服务器";
                return;
            }

            if (string.IsNullOrEmpty(this.playerNameTextBox.Text) &&
                string.IsNullOrEmpty(this.uidTextBox.Text) &&
                string.IsNullOrEmpty(this.cyidTextBox.Text))
            {
                this.errorLabel.Text = "没有输入玩家信息";
                return;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.selectListBox.Items.Count; ++i)
            {
                gm.Server server = gm.Server.GetServer(this.selectListBox.Items[i].Text);

                builder.Append(server.Name).Append("<br>");

                string sql;

                if (!string.IsNullOrEmpty(this.playerNameTextBox.Text))
                {
                    sql = string.Format("SELECT `uname`,`uid`,`cyuid` FROM `user` WHERE `uname` LIKE '%{0}%';", DatabaseAssistant.DetectionSql(this.playerNameTextBox.Text));
                }
                else if (!string.IsNullOrEmpty(this.uidTextBox.Text))
                {
                    sql = string.Format("SELECT `uname`,`uid`,`cyuid` FROM `user` WHERE `uid`='{0}';", DatabaseAssistant.DetectionSql(this.uidTextBox.Text));
                }
                else
                {
                    sql = string.Format("SELECT `uname`,`uid`,`cyuid` FROM `user` WHERE `cyuid`='{0}';", DatabaseAssistant.DetectionSql(this.cyidTextBox.Text));
                }

                DatabaseAssistant.Execute(reader =>
                {
                    bool finded = false;
                    while (reader.Read())
                    {
                        finded = true;
                        builder.Append("玩家名称(").Append(reader.GetString(0));
                        builder.Append(") 游戏ID(").Append(reader.GetUInt32(1));
                        builder.Append(") 平台ID(").Append(reader.GetString(2)).Append(")<br>");
                    }

                    if (!finded) { builder.Append("找不到该用户<br>"); }
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                sql);
            }

            this.idLabel.Text = builder.ToString();
        }

        /// <summary>
        /// 数据库更新按钮点击响应
        /// </summary>
        protected void dbUpdateButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "没有选择服务器";
                return;
            }

            foreach (var item in this.selectListBox.Items)
            {
                gm.Server server = gm.Server.GetServer(item.ToString());

                if (server == null)
                {
                    //TODO
                    continue;
                }
                byte[] serverId = new byte[4];
                string[] serverIdText = server.Id.Split('-');

                for (int j = 0; j < serverId.Length; ++j)
                {
                    serverId[j] = byte.Parse(serverIdText[j]);
                }
                /*
                this.Execute(server, "guildinfo.sql");
                this.Execute(server, "guilduserinfo.sql");
                this.Execute(server, "xp_clear_user.sql");
                this.Execute(server, "xp_mix_data.sql");

                bool hasColumn = true;

                DatabaseAssistant.Execute
                (
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "CALL `xp_clear_user`;"
                );

                DatabaseAssistant.Execute(reader =>
                {
                    hasColumn = reader.Read();
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `COLUMN_NAME` FROM `information_schema`.`COLUMNS` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='user_info' AND `COLUMN_NAME`='devote';", server.GameDatabase);

                if (!hasColumn)
                {
                    DatabaseAssistant.Execute
                    (
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "ALTER TABLE `user_info` ADD COLUMN `devote` INT(11) NULL DEFAULT '0' AFTER `honor`;"
                    );
                }

                DatabaseAssistant.Execute(reader =>
                {
                    hasColumn = reader.Read();
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `COLUMN_NAME` FROM `information_schema`.`COLUMNS` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='user' AND `COLUMN_NAME`='server_id';", server.GameDatabase);

                if (!hasColumn)
                {
                    DatabaseAssistant.Execute
                    (
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "ALTER TABLE `user` ADD COLUMN `server_id` INT NULL DEFAULT '0'" +
                        "AFTER `channel_id`, DROP INDEX `cc_index`, ADD UNIQUE INDEX" +
                        "`cc_index`(`cyuid`, `channel_id`, `server_id`);UPDATE `user` SET `server_id`={0};", BitConverter.ToInt32(serverId, 0)
                    );
                }

                DatabaseAssistant.Execute(reader =>
                {
                    hasColumn = reader.Read();
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `COLUMN_NAME` FROM `information_schema`.`COLUMNS` WHERE `TABLE_SCHEMA`='{0}' AND `TABLE_NAME`='global' AND `COLUMN_NAME`='guild_war_data';", server.GameDatabase);

                if (!hasColumn)
                {
                    DatabaseAssistant.Execute
                    (
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "ALTER TABLE `global`" +
                        "ADD COLUMN `guild_war_data` LONGBLOB NULL AFTER `activity_data`," +
                        "ADD COLUMN `gmt_gift_data` LONGBLOB NULL AFTER `guild_war_data`," +
                        "ADD COLUMN `gmt_arand_data` LONGBLOB NULL AFTER `gmt_gift_data`," +
                        "ADD COLUMN `gmt_rrand_data` LONGBLOB NULL AFTER `gmt_arand_data`," +
                        "ADD COLUMN `gmt_achieve_data` LONGBLOB NULL AFTER `gmt_rrand_data`," +
                        "ADD COLUMN `gmt_reward_data` LONGBLOB NULL AFTER `gmt_achieve_data`;"
                    );
                }*/
                DatabaseAssistant.Execute
                (
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "UPDATE `user` SET `server_id`={0};", BitConverter.ToInt32(serverId, 0)
                );
            }

            this.errorLabel.Text = "数据库更新完成";
        }

        /// <summary>
        /// 显示信息按钮点击响应
        /// </summary>
        protected void informationButton_Click(object sender, EventArgs e)
        {   


            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                gm.Server server = gm.Server.GetServer(this.selectListBox.Items[0].ToString());

                StringBuilder builder = new StringBuilder();
                if (string.IsNullOrEmpty(server.GameDatabase))
                {
                    return;
                }
                DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        this.VipLevelText.Text = reader.GetInt32(0).ToString();
                        this.OnlineTimeText.Text = reader.GetInt32(9).ToString();
                        this.MoneyText.Text = reader.GetInt32(1).ToString();
                        this.YubaoText.Text = reader.GetInt32(2).ToString();
                        this.EnergeText.Text = reader.GetInt32(3).ToString();
                        this.SkillpointText.Text = reader.GetInt32(4).ToString();
                        this.ResorvepointText.Text = reader.GetInt32(5).ToString();
                        this.ActivepointText.Text = reader.GetInt32(6).ToString();
                        if (reader.GetInt32(7) != 0)
                            this.EnergeTimeText.Text = Getdaytime(reader.GetInt32(7)).ToString();
                        else
                            this.EnergeTimeText.Text = "已满";
                        if (reader.GetInt32(8) != 0)
                            this.SkillpointTimeText.Text = Getdaytime(reader.GetInt32(8)).ToString();
                        else
                            this.SkillpointTimeText.Text = "已满";
                        this.OnlineTimeText.Text = reader.GetInt32(9)/3600+"小时";
                        this.OnlineTimeText.Text += (reader.GetInt32(9)%3600)/60 + "分钟";
                        this.OnlineTimeText.Text += (reader.GetInt32(9)%60)+ "秒";
                        this.DescPlayeruidText.Text = reader.GetInt32(10).ToString();
                        this.yuanbao_all.Text = reader.GetInt32(11).ToString();
                        this.HonorText.Text = reader.GetInt32(12).ToString();
                        builder.Append(string.Format
                        (
                            "VIP等级({0})，金钱({1})，元宝({2})，精力({3})，技能点({4})，分解点({5})，活跃点({6})，精力时间({7})，技能点时间({8})，在线时间({9})，uid({10})，元宝充值数({11})，荣誉点({12})<br><br>",
                            reader.GetInt32(0),
                            reader.GetInt32(1),
                            reader.GetInt32(2),
                            reader.GetInt32(3),
                            reader.GetInt32(4),
                            reader.GetInt32(5),
                            reader.GetInt32(6),
                            reader.GetInt32(7),
                            reader.GetInt32(8),                         
                            reader.GetInt32(9),
                            reader.GetInt32(10),
                            reader.GetInt32(11),
                            reader.GetInt32(12)
                            
                        ));
                    }
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `vip_level`,`money`,`yuanbao`,`energy`,`skill`,`resolve`,`active`,`energy_time`,`skill_time`,`online_time` ,`uid` ,`yuanbao_all`,`honor` FROM `user_info` WHERE `uid`={0};",
                playerId);


                DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        byte[] buffer = new byte[reader.GetBytes(0, 0, null, 0, int.MaxValue)];

                        reader.GetBytes(0, 0, buffer, 0, buffer.Length);

                        using (MemoryStream stream = new MemoryStream(buffer))
                        {
                            mw.OtherInfo othern = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.OtherInfo)) as mw.OtherInfo;
                            this.TTTCurCengText.Text = othern.challengeInfo.level.ToString();
                            this.TTTMaxCengText.Text = othern.challengeInfo.high_level.ToString();
                            this.TTTResetTimeText.Text = othern.challengeInfo.clean_count.ToString();
                            builder.Append(string.Format
                            (
                                "通天塔：层数({0})，最高层数({1})，重置次数({2})<br>",
                                othern.challengeInfo.level,
                                othern.challengeInfo.high_level,
                                othern.challengeInfo.clean_count
                            ));
                            this.Chipinfolabel.Text = "";
                            builder.Append("chipInfos:<br>");
                            foreach (mw.ChipInfo chipInfos in othern.chipInfos)
                            {
                                builder.Append("type:");
                                builder.Append(chipInfos.type);
                                this.Chipinfolabel.Text += "type:" + chipInfos.type.ToString();
                                builder.Append(",count:");
                                builder.Append(chipInfos.count);
                                this.Chipinfolabel.Text += ",count:" + chipInfos.count.ToString() + ";" + "\n";
                                builder.Append("<br>");
                            }

                            /*builder.Append("cooltimeInfos:<br>");
                            foreach (mw.CoolTimeInfo cooltimeInfos in othern.cooltimeInfos)
                            {
                                builder.Append("id:");
                                builder.Append(cooltimeInfos.id);
                                builder.Append(",time:");
                                builder.Append(cooltimeInfos.time);
                                builder.Append("<br>");
                            }*/
                            this.Friendinfolabel.Text = "";
                            builder.Append("friendInfos:<br>");
                            foreach (mw.Role4Friend friendInfos in othern.friendInfos)
                            {
                                builder.Append("uid:");
                                builder.Append(friendInfos.uid);
                                this.Friendinfolabel.Text += "uid:" + friendInfos.uid.ToString();
                                builder.Append(",level:");
                                builder.Append(friendInfos.level);
                                this.Friendinfolabel.Text += ",level:" + friendInfos.level.ToString();
                                builder.Append(",name:");
                                builder.Append(friendInfos.name);
                                this.Friendinfolabel.Text += ",name:" + friendInfos.name.ToString() + ";" + "\n";
                                builder.Append("<br>");
                            }
                            //this.gift_flaglabel.Text = "";
                            builder.Append("gift_flag:");
                            foreach (int gift_flag in othern.giftInfo.gift_flag)
                            {
                                builder.Append(gift_flag);
                                //this.gift_flaglabel.Text += gift_flag.ToString() + ",";
                                builder.Append(",");
                            }
                            //this.giftinfoslabel.Text = "";
                            builder.Append("<br>");
                            builder.Append("giftInfos:");
                            foreach (int giftInfos in othern.giftInfo.giftInfos)
                            {
                                builder.Append(giftInfos);
                                //this.giftinfoslabel.Text += giftInfos.ToString() + ",";
                                builder.Append(",");
                            }
                            builder.Append("<br>");
                            //this.RoleflagText.Text = othern.roleexInfo.flag.ToString();
                            //this.FreechoujiangText.Text = othern.roleexInfo.free_roll_time.ToString();
                            //this.LastDateText.Text = othern.roleexInfo.last_date.ToString();
                            //this.IndexText.Text = othern.roleexInfo.pos_info.ToString();
                            builder.Append(string.Format
                            (
                                "角色额外数据：flag({0})，免费抽奖({1})，最后日期({2})，位置({3})<br>",
                                othern.roleexInfo.flag,
                                othern.roleexInfo.free_roll_time,
                                othern.roleexInfo.last_date,
                                othern.roleexInfo.pos_info
                            ));
                            this.VipChalengeText.Text = othern.vipcountInfo.challenge_count.ToString();
                            this.VipEnergeText.Text = othern.vipcountInfo.energy_count.ToString();
                            this.VipMoneyText.Text = othern.vipcountInfo.money_count.ToString();
                            this.VipSkillpointText.Text = othern.vipcountInfo.skillpt_count.ToString();
                            this.VipYuanbaoText.Text = othern.vipcountInfo.yuanbao_count.ToString();
                            builder.Append(string.Format
                            (
                                "VIP购买次数：挑战({0})，精力({1})，金钱({2})，技能点({3})，元宝({4})<br>",
                                othern.vipcountInfo.challenge_count,
                                othern.vipcountInfo.energy_count,
                                othern.vipcountInfo.money_count,
                                othern.vipcountInfo.skillpt_count,
                                othern.vipcountInfo.yuanbao_count
                            ));
                            //副本进度信息
                            this.FubenprogressText.Text="";
                            this.FubenprogressText.Text = othern.instanceInfo.curr_instance.ToString();
                            //天赋信息
                            this.TalentlevelText.Text="";
                            foreach (mw.TalentInfo pair in othern.talentInfos)
                            {
                                this.TalentlevelText.Text += pair.talentInfo + ",";
                            }

           

                            //竞技场历史最高排名
                            if (othern.roleexInfo.pvp_pos != 0)
                                this.PVPText.Text = othern.roleexInfo.pvp_pos.ToString();
                            else
                                this.PVPText.Text = "暂无排名";
                            //public  bool GetQinyuanFalg(int ID)
                            //{
                            //   List<int> rewardFlag = Globals.Instance.MainData.qingyuanRewardFlag;
                            //   int idx = ID / 32;
                            //   int pos = ID % 32;
                            //   if (idx > rewardFlag.Count - 1) return false;
                            //   return Globals.Instance.HasFlag(rewardFlag[idx], pos);
                            // }

                            this.QingyuanText.Text="无";
                            Dictionary<int,QingyuanConfig> qingyuanlist = QingyuanTable.Instance().GetQingyuanDictionary();
                            foreach (var pair in qingyuanlist)
                            {
                               List<int> flags = othern.roleexInfo.qingyuan_flag;
                               int idx = pair.Value.type / 32;
                               int pos = pair.Value.type % 32;
                               if (idx > flags.Count - 1) continue;

                               if (HasFlag(flags[idx], pos))
                               {
                                   this.QingyuanText.Text += pair.Value.type.ToString() + ";";
                               }
                            }


                            //foreach (int flag in othern.roleexInfo.qingyuan_flag)
                            //{     
                            //     //if(HasFlag(flag,0))
                            //    //this.QingyuanText.Text += flag.ToString() + ",";
                            //}          

                            ////出战英雄
                            //mw.RoleFightInfo rolefight = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.RoleFightInfo)) as mw.RoleFightInfo;
                            //foreach(mw.CardInfo pair in rolefight.cardInfos)
                            //{
                            //    this.FightheroText.Text += pair.index.ToString()+";";
                            //}

                            
                        }
                    }
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `other_data` FROM `other_info` WHERE `uid`={0};", playerId);

                builder.Append("<br>");

                

                //大月卡
                DatabaseAssistant.Execute(reader =>
                {
                    this.BigMonthText.Text = "无";
                    if (reader.Read())
                    {
                        if (reader.Read())
                        {
                            DateTime datetime = reader.GetDateTime(0);
                            this.BigMonthText.Text = datetime.ToString();
                        }
                    }
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `b_month_time` FROM `user_info` WHERE `uid`={0};", playerId);
                builder.Append("<br>");

                //小月卡
                DatabaseAssistant.Execute(reader =>
                {
                    this.SmallMonthText.Text = "无";
                    if (reader.Read())
                    {
                        if (reader.Read())
                        {
                            DateTime datetime = reader.GetDateTime(0);
                            this.SmallMonthText.Text = datetime.ToString();   

                        }
                    }
                },
               server.DatabaseAddress,
               server.DatabasePort,
               server.DatabaseCharSet,
               server.GameDatabase,
               server.DatabaseUserId,
               server.DatabasePassword,
               "SELECT `s_month_time` FROM `user_info` WHERE `uid`={0};", playerId);
                builder.Append("<br>");
                //角色创建时间
                DatabaseAssistant.Execute(reader =>
                {
                    this.RolecreateTimeText.Text = "未知";
                    if (reader.Read())
                    {                  
                         DateTime datetime = reader.GetDateTime(0);
                         this.RolecreateTimeText.Text = datetime.ToString();     
                    }
                },
               server.DatabaseAddress,
               server.DatabasePort,
               server.DatabaseCharSet,
               server.GameDatabase,
               server.DatabaseUserId,
               server.DatabasePassword,
               "SELECT `register_time` FROM `user` WHERE `uid`={0};", playerId);
                builder.Append("<br>");

                DatabaseAssistant.Execute(reader =>
                {
                    if (reader.Read())
                    {
                        byte[] buffer = new byte[reader.GetBytes(0, 0, null, 0, int.MaxValue)];

                        reader.GetBytes(0, 0, buffer, 0, buffer.Length);

                        using (MemoryStream stream = new MemoryStream(buffer))
                        {
                            mw.CardPackage cn = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.CardPackage)) as mw.CardPackage;
                            this.ExpText.Text = cn.exp.ToString();
                            this.LevelText.Text = cn.level.ToString();
                            builder.Append(string.Format
                            (
                                "经验({0})，等级({1})<br>",
                                cn.exp,
                                cn.level
                            ));
                            this.Cardinfolabel.Text = "";

                            builder.Append("cardInfos:<br>");
                            foreach (mw.CardInfo cardInfo in cn.cardInfos)
                            {
                                builder.Append("index:");
                                builder.Append(cardInfo.index);
                                this.Cardinfolabel.Text += "index:" + cardInfo.index.ToString();
                                builder.Append(",type:");
                                builder.Append(cardInfo.type);
                                this.Cardinfolabel.Text += ",type:" + cardInfo.type.ToString();
                                builder.Append(",exp:");
                                builder.Append(cardInfo.exp);
                                this.Cardinfolabel.Text += ",exp:" + cardInfo.exp.ToString();
                                builder.Append(",level:");
                                builder.Append(cardInfo.level);
                                this.Cardinfolabel.Text += ",level:" + cardInfo.level.ToString();
                                builder.Append(",levelex:");
                                builder.Append(cardInfo.levelex);
                                this.Cardinfolabel.Text += ",levelex:" + cardInfo.levelex.ToString() + ";" + "\n";
                                builder.Append("<br>");

                            }
                            //魂阵信息卡牌包里面的SlotInfo
                            //this.CardSlotNumText.Text = cn.slotInfos.Count().ToString();
                            this.CardSlotNumText.Text ="0";
                            int slotcount = 0;
                            foreach (mw.SlotInfo pair in cn.slotInfos)
                            {   
                                if(pair.flag==1)
                                {
                                   slotcount++;
                                }
                                this.CardSlotNumText.Text = slotcount.ToString();
                            }

                            this.Petinfolabel.Text = "";
                            //宠物数据
                            foreach (mw.PetInfo petInfo in cn.petInfos)
                            {
                                this.Petinfolabel.Text += "index:" + petInfo.index.ToString();
                                this.Petinfolabel.Text += ",type:" + petInfo.type.ToString();
                                this.Petinfolabel.Text += ",exp:" + petInfo.exp.ToString();
                                this.Petinfolabel.Text += ",level:" + petInfo.level.ToString();
                                this.Petinfolabel.Text += ",levelex:" + petInfo.levelex.ToString() + ";" + "\n";
                            }


                            /*builder.Append("teamInfos:<br>");
                            foreach (mw.TeamInfo teamInfos in cn.teamInfos)
                            {
                                builder.Append("leader_idx:");
                                builder.Append(teamInfos.leader_idx);
                                builder.Append(",card_0:");
                                builder.Append(teamInfos.card_0);
                                builder.Append(",card_1:");
                                builder.Append(teamInfos.card_1);
                                builder.Append(",card_2:");
                                builder.Append(teamInfos.card_2);
                                builder.Append("<br>");
                            }*/
                        }
                    }
                },
                server.DatabaseAddress,
                server.DatabasePort,
                server.DatabaseCharSet,
                server.GameDatabase,
                server.DatabaseUserId,
                server.DatabasePassword,
                "SELECT `card_data` FROM `card_package` WHERE `uid`={0};", playerId);
                
                //先屏蔽 
                //this.otherLabel.Text = builder.ToString();
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
        protected override bool ExecuteGmCommand(string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "没有选择服务器";
                return false;
            }

            bool success = true;
            string user = Session["user"] as string;

            foreach (var item in this.selectListBox.Items)
            {
                gm.Server server = gm.Server.GetServer(item.ToString());

                if (server != null)
                {
                    success &= AGmPage.ExecuteGmCommand(user, server, playerId, commandText, operateText, needReturn, reportProcess);
                }
            }

            return success;
        }

        /// <summary>
        /// 获取玩家编号
        /// </summary>
        /// <param name="needReport">是否需要报告</param>
        /// <returns>玩家编号</returns>
        private uint GetPlayerId(bool needReport = true)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                if (needReport) { this.errorLabel.Text = "没有选择服务器"; }
                return 0;
            }
            if (this.selectListBox.Items.Count != 1)
            {
                if (needReport) { this.errorLabel.Text = "只能查一个服务器啦"; }
                return 0;
            }
            gm.Server server = gm.Server.GetServer(this.selectListBox.Items[0].ToString());

            if (server == null)
            {
                if (needReport) { this.errorLabel.Text = "查询服务器失败"; }
                return 0;
            }

            uint playerId = 0;

            string sql;

            if (!string.IsNullOrEmpty(this.playerNameTextBox.Text))
            {
                sql = string.Format("SELECT `uid` FROM `user` WHERE `uname`='{0}';", DatabaseAssistant.DetectionSql(this.playerNameTextBox.Text));
            }
            else if (!string.IsNullOrEmpty(this.uidTextBox.Text))
            {
                sql = string.Format("SELECT `uid` FROM `user` WHERE `uid`='{0}';", DatabaseAssistant.DetectionSql(this.uidTextBox.Text));
            }
            else if (!string.IsNullOrEmpty(this.cyidTextBox.Text))
            {
                sql = string.Format("SELECT `uid` FROM `user` WHERE `cyuid`='{0}';", DatabaseAssistant.DetectionSql(this.cyidTextBox.Text));
            }
            else
            {
                this.errorLabel.Text = "没有输入玩家信息";
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
                this.errorLabel.Text += "找不到该玩家\r\n";
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
                //using (StreamReader reader = new StreamReader(respone.GetResponseStream()))
                //{
                //	string text = reader.ReadToEnd();

                //	if (text != null && text != "")
                //	{
                //		this.errorLabel.Text += text;
                //	}
                //	else
                //	{
                //		this.errorLabel.Text += "返回的是空";
                //	}
                //}
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                this.errorLabel.Text += exception.ToString();
            }
        }

        //重导但是不包括活动
        protected void InputNotActivityButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "请选择服务器";
                return;
            }

            string user = Session["user"] as string;
            this.errorLabel.Text = "";
            foreach (var item in this.selectListBox.Items)
            {
                gm.Server server = gm.Server.GetServer(item.ToString());
                if (server != null)
                {
                    AGmPage.ExecuteGmCommand
                    (
                        user,
                        server,
                        "0",
                        string.Format("RL(0)"),
                        "",
                        true,
                        text => { this.errorLabel.Text += text; }
                    );
                }
            }
        }


        //全部重导
        protected void InputAllButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = "请选择服务器";
                return;
            }

            string user = Session["user"] as string;
            this.errorLabel.Text = "";
            foreach (var item in this.selectListBox.Items)
            {
                gm.Server server = gm.Server.GetServer(item.ToString());
                if (server != null)
                {
                    AGmPage.ExecuteGmCommand
                    (
                        user,
                        server,
                        "0",
                        string.Format("RL(1)"),
                        "",
                        true,
                        text => { this.errorLabel.Text += text; }
                    );
                }
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


        //点击查询,清空所有下面的显示信息,防止误解

            //this.VipLevelText.Text="";
            //this.OnlineTimeText.Text = "";
            //this.yuanbao_all.Text = "";
            //this.RolecreateTimeText.Text = "";
            //this.DescPlayeruidText.Text = "";
            //this.MoneyText.Text = "";
            //this.YubaoText.Text = "";
            //this.EnergeText.Text = "";
            //this.SkillpointText.Text = "";
            //this.ResorvepointText.Text = "";
            //this.ActivepointText.Text = "";
            //this.HonorText.Text = "";
            //this.SkillpointTimeText.Text = "";
            //this.ExpText.Text = "";
            //this.LevelText.Text = "";
            //this.TTTCurCengText.Text = "";
            //this.TTTMaxCengText.Text = "";
            //this.TTTResetTimeText.Text = "";
            //this.FubenprogressText.Text = "";
            //this.TalentlevelText.Text = "";
            //this.PVPText.Text = "";
            //this.EnergeTimeText.Text = "";
            //this.CardSlotNumText.Text = "";
            //this.BigMonthText.Text = "";
            //this.SmallMonthText.Text = "";
            //this.QingyuanText.Text = "";
            //this.RoleflagText.Text = "";
            //this.FreechoujiangText.Text = "";
            //this.LastDateText.Text = "";
            //this.IndexText.Text = "";
            //this.VipChalengeText.Text = "";
            //this.VipEnergeText.Text = "";
            //this.VipMoneyText.Text = "";
            //this.VipSkillpointText.Text = "";
            //this.VipYuanbaoText.Text = "";
            //this.Cardinfolabel.Text = "";
            //this.Chipinfolabel.Text = "";
            //this.Friendinfolabel.Text = "";
            //this.Petinfolabel.Text = "";
            //this.gift_flaglabel.Text = "";
            //this.giftinfoslabel.Text = "";


 
    }
}