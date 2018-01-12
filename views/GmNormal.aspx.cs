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

namespace gmt
{
    /// <summary>
    /// GM操作界面
    /// </summary>
    public partial class GmNormal : AGmPage
    {
        /// <summary>
        /// 版本
        /// </summary>
        //public const string Version = "2.0.0";
        public const string Version = "0.0.1";

        /// <summary>
        /// 是否需要返回
        /// </summary>
        private bool needReturn = true;

        /// <summary>
        /// 构造方法
        /// </summary>
        public GmNormal()
            : base(PrivilegeType.GmNormal)
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
                this.versionLabel.Text = GmNormal.Version;

                if (Ban.banList.Count == 0 || Ban.banList == null)
                {
                    Ban.LoadBanList();
                }

                if (ShutUp.shutupList.Count == 0 || ShutUp.shutupList == null)
                {
                    ShutUp.LoadShutupList();
                }

                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server theServer = gmt.Server.GetServerAt(i);
                    this.serverListBox.Items.Add(new ListItem(theServer.Name));
                }
            }
        }

        protected void Page_LoadComplete()
        {
            Ban.SaveBanList();
            ShutUp.SaveShutupList();
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
                ShutUp.shutupList.Add(playerId);
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
                ShutUp.shutupList.Remove(playerId);
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
        /// 封号按钮点击响应
        /// </summary>
        protected void banButton_Click(object sender, EventArgs e)
        {
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                this.ExecuteGmCommand("0", string.Format("LOGINF({0},1)", playerId), "", this.needReturn, text => this.errorLabel.Text += text);
                Ban.banList.Add(playerId);
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
                Ban.banList.Remove(playerId);
            }
        }

        /// <summary>
        /// 查询ID按钮点击响应
        /// </summary>
        protected void findIdButton_Click(object sender, EventArgs e)
        {
            this.idLabel.Text = "";
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(740);
                return;
            }

            if (string.IsNullOrEmpty(this.playerNameTextBox.Text) &&
                string.IsNullOrEmpty(this.uidTextBox.Text) &&
                string.IsNullOrEmpty(this.cyidTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GetGMTText(721);
                return;
            }

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.selectListBox.Items.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[i].Text);

                builder.Append("<b style='color:orange'>").Append(server.Name).Append("</b><br>");

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
                        builder.Append(TableManager.GetGMTText(46) + ":").Append(reader.GetString(0));
                        builder.Append("&#09;" + TableManager.GetGMTText(722) + ":").Append(reader.GetUInt32(1));
                        builder.Append("&#09;" + TableManager.GetGMTText(48) + ":").Append(reader.GetString(2));
                        builder.Append("&#09;" + TableManager.GetGMTText(59) + ":").Append(GetPlayerStatus(reader.GetUInt32(1)));

                        builder.Append("<br>");
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
            }

            this.idLabel.Text = builder.ToString();
        }

        /// <summary>
        /// 显示信息按钮点击响应
        /// </summary>
        protected void informationButton_Click(object sender, EventArgs e)
        {
            this.idLabel.Text = "";
            uint playerId = this.GetPlayerId();

            if (playerId != 0)
            {
                gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].ToString());
                StringBuilder builder = new StringBuilder();
                if (string.IsNullOrEmpty(server.GameDatabase))
                {
                    return;
                }

                //记录各项信息的字符串
                string vipLevel = ""
                    , teamLevel = ""
                    , yuanbao_all = ""
                    , yuanbao = ""
                    , money = ""
                    , energy = ""
                    , lingyu = ""
                    , registerTime = ""
                    , heroCount = ""
                    , fuben = ""
                    , guildName = ""
                    , onlinetime = ""
                    , smallMonth = ""
                    , bigMonth = ""
                    , lastSaveTime = "";

                #region VIP等级、金钱、元宝、体力、在线时长、充值元宝总数、灵玉、最后登录时间
                DatabaseAssistant.Execute(
                    reader =>
                    {
                        if (reader.Read())
                        {

                            vipLevel = reader.GetInt32(0).ToString();
                            money = reader.GetInt32(1).ToString();
                            yuanbao = reader.GetInt32(2).ToString();
                            energy = reader.GetInt32(3).ToString();

                            onlinetime = reader.GetInt32(4) / 3600 + TableManager.GetGMTText(741);
                            onlinetime += (reader.GetInt32(4) % 3600) / 60 + TableManager.GetGMTText(742);
                            onlinetime += reader.GetInt32(4) % 60 + TableManager.GetGMTText(94);

                            yuanbao_all = reader.GetInt32(5).ToString();
                            lingyu = reader.GetInt32(6).ToString();

                            lastSaveTime = reader.GetDateTime(7).ToString();

                        }

                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT `vip_level`,`money`,`yuanbao`,`energy`,`online_time`,`yuanbao_all`,`hero_soul_ex`,`last_save_time` FROM `user_info` WHERE `uid`={0};",
                    playerId);
                #endregion

                #region 副本进度、帮派名称
                DatabaseAssistant.Execute(
                    reader =>
                    {
                        if (reader.Read())
                        {
                            byte[] buffer = new byte[reader.GetBytes(0, 0, null, 0, int.MaxValue)];
                            reader.GetBytes(0, 0, buffer, 0, buffer.Length);
                            using (MemoryStream stream = new MemoryStream(buffer))
                            {
                                mw.OtherInfo othern = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.OtherInfo)) as mw.OtherInfo;
                                fuben = othern.instanceInfo.comm_instance.ToString();
                                guildName = othern.guildInfo.name;
                                if (string.IsNullOrEmpty(guildName))
                                    guildName = TableManager.GetGMTText(743);
                            }
                        }
                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT `other_data` FROM `other_info` WHERE `uid`={0};",
                    playerId);
                #endregion

                #region 创建时间
                int dayidx = 0;         //记录创建时间是一年中的第几天
                DateTime registertime = DateTime.Now;
                //角色创建时间
                DatabaseAssistant.Execute(reader =>
                {
                    registerTime = TableManager.GetGMTText(719);
                    if (reader.Read())
                    {
                        DateTime datetime = reader.GetDateTime(0);
                        registerTime = datetime.ToString();
                        dayidx = reader.GetDateTime(0).DayOfYear;
                        registertime = reader.GetDateTime(0);
                    }
                },
               server.DatabaseAddress,
               server.DatabasePort,
               server.DatabaseCharSet,
               server.GameDatabase,
               server.DatabaseUserId,
               server.DatabasePassword,
               "SELECT `register_time` FROM `user` WHERE `uid`={0};",
               playerId);
                #endregion

                #region 团队等级、武将数量
                DatabaseAssistant.Execute(
                    reader =>
                    {
                        if (reader.Read())
                        {
                            byte[] buffer = new byte[reader.GetBytes(0, 0, null, 0, int.MaxValue)];
                            reader.GetBytes(0, 0, buffer, 0, buffer.Length);
                            using (MemoryStream stream = new MemoryStream(buffer))
                            {
                                mw.HeroPackage heroPackage = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.HeroPackage)) as mw.HeroPackage;
                                teamLevel = heroPackage.level.ToString();
                                heroCount = heroPackage.heroInfos.Count.ToString();
                            }
                        }
                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT `card_data` FROM `card_package` WHERE `uid`={0};",
                    playerId);
                #endregion

                #region 月卡
                //小月卡=超值月卡
                DatabaseAssistant.Execute(
                    reader =>
                    {
                        smallMonth = TableManager.GetGMTText(743);
                        if (reader.Read())
                        {
                            DateTime datetime = reader.GetDateTime(0);
                            smallMonth = datetime.ToString();
                        }
                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT `s_month_time` FROM `user_info` WHERE `uid`={0};",
                    playerId);

                //大月卡=至尊卡
                DatabaseAssistant.Execute(
                    reader =>
                    {
                        bigMonth = TableManager.GetGMTText(744);
                        if (reader.Read())
                        {
                            if (reader.GetInt32(0) == 1)
                            {
                                bigMonth = TableManager.GetGMTText(745);
                            }
                        }
                    },
                    server.DatabaseAddress,
                    server.DatabasePort,
                    server.DatabaseCharSet,
                    server.GameDatabase,
                    server.DatabaseUserId,
                    server.DatabasePassword,
                    "SELECT `ex_month_time` FROM `user_info` WHERE `uid`={0};",
                    playerId);
                #endregion

                #region 显示

                if (vipLevel == "")
                {
                    vipLevel = "0";
                }
                if (teamLevel == "")
                {
                    teamLevel = "0";
                }
                if (yuanbao_all == "")
                {
                    yuanbao_all = "0";
                }
                if (yuanbao == "")
                {
                    yuanbao = "0";
                }
                if (money == "")
                {
                    money = "0";
                }
                if (energy == "")
                {
                    energy = "0";
                }
                if (lingyu == "")
                {
                    lingyu = "0";
                }
                if (heroCount == "")
                {
                    heroCount = "0";
                }
                if (fuben == "")
                {
                    fuben = TableManager.GetGMTText(743);
                }
                if (guildName == "")
                {
                    guildName = TableManager.GetGMTText(743);
                }
                if (onlinetime == "")
                {
                    onlinetime = "0";
                }

                builder.Append("<table><tr><td colspan='8'>" + TableManager.GetGMTText(57) + "</td></tr>");
                builder.Append("<tr><td colspan='2'>" + TableManager.GetGMTText(58) + ":").Append(playerId);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(59) + ":").Append(GetPlayerStatus(playerId));
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(60) + ":").Append(vipLevel);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(61) + ":").Append(teamLevel);
                builder.Append("</td></tr><tr><td colspan='2'>" + TableManager.GetGMTText(62) + ":").Append(yuanbao_all);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(63) + ":").Append(yuanbao);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(64) + ":").Append(money);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(65) + ":").Append(energy);
                builder.Append("</td></tr><tr><td colspan='2'>" + TableManager.GetGMTText(66) + ":").Append(lingyu);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(67) + ":").Append(heroCount);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(68) + ":").Append(fuben);
                builder.Append("</td><td colspan='2'>" + TableManager.GetGMTText(69) + ":").Append(guildName);
                builder.Append("</td></tr><tr><td colspan='4'>" + TableManager.GetGMTText(70) + ":").Append(registerTime);
                builder.Append("</td><td colspan='4'>" + TableManager.GetGMTText(71) + ":").Append(lastSaveTime);
                builder.Append("</td></tr><tr><td colspan='2'>" + TableManager.GetGMTText(72) + ":").Append(onlinetime);
                builder.Append("</td><td colspan='4'>" + TableManager.GetGMTText(746) + ":").Append(smallMonth);
                builder.Append("</td></tr><tr><td colspan='2'>" + TableManager.GetGMTText(74) + ":").Append(bigMonth);
                builder.Append("</td></tr></table>");

                this.idLabel.Text = builder.ToString();

                #endregion

            }
        }

        /// <summary>
        /// 获取玩家状态
        /// </summary>
        private string GetPlayerStatus(uint playerId)
        {
            for (int i = 0; i < Ban.banList.Count; i++)
            {
                if (playerId == Ban.banList[i])
                    return "<b style='color:red'>" + TableManager.GetGMTText(53) + "</b>";
            }
            for (int i = 0; i < ShutUp.shutupList.Count; i++)
            {
                if (playerId == ShutUp.shutupList[i])
                    return "<b style='color:orange'>" + TableManager.GetGMTText(51) + "</b>";
            }
            return "<b style='color:green'>" + TableManager.GetGMTText(747) + "</b>";
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
                this.errorLabel.Text = TableManager.GetGMTText(740);
                return false;
            }

            bool success = true;
            string account = Session["user"] as string;

            foreach (var item in this.selectListBox.Items)
            {
                gmt.Server server = gmt.Server.GetServer(item.ToString());

                if (server != null)
                {
                    success &= AGmPage.ExecuteGmCommand(account, server, playerId, commandText, operateText, needReturn, reportProcess);
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
                if (needReport) { this.errorLabel.Text = TableManager.GetGMTText(740); }
                return 0;
            }
            if (this.selectListBox.Items.Count != 1)
            {
                if (needReport) { this.errorLabel.Text = TableManager.GetGMTText(748); }
                return 0;
            }
            gmt.Server server = gmt.Server.GetServer(this.selectListBox.Items[0].ToString());

            if (server == null)
            {
                if (needReport) { this.errorLabel.Text = TableManager.GetGMTText(749); }
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

                byte[] byteArray = Encoding.UTF8.GetBytes("pushInfo=order:测试orderPushInfo&receipt=eyJhcmVhTmFtZSI6IiAiLCJjaGFubmVsSWQiOiIyMDAxIiwiZ29vZHNOdW1iZXIiOiIxIiwidXNlcmlkIjoiNjkyMDAxMTA0NDQ4NDAxIiwiZ29vZHNQcmljZSI6MjQwMCwicm9sZU5hbWUiOiIgIiwiZ29vZHNSZWdpc3RlcklkIjoiNzU2NzgwMDciLCJhcHBEYXRlIjoiMjAxNTA3MjAxMTMzMzYiLCJvcmRlcklkIjoiMTAwMDAwMDE2NDE2NzMzNiIsImFyZWFJZCI6IjEtMTUtNi0xIiwicm9sZUlkIjoiMTAwMDIifQ%3D%3D&sign=38313c03b8d6e62dc7e879543e183252");
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

        //protected void informationButton_Click1(object sender, EventArgs e)
        //{

        //}
    }

    /// <summary>
    /// 封号
    /// </summary>
    public class Ban
    {
        public static List<uint> banList = new List<uint>();

        /// <summary>
        /// 读取封号列表
        /// </summary>
        public static void LoadBanList()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/BanList.bytes";

            if (!File.Exists(configBinaryFile)) { return; }

            using (FileStream stream = File.OpenRead(configBinaryFile))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < stream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    ushort count = reader.ReadUInt16();
                    for (int i = 0; i < count; i++)
                    {
                        Ban.banList.Add(reader.ReadUInt32());
                    }
                }
            }
        }

        /// <summary>
        /// 存储封号列表
        /// </summary>
        public static void SaveBanList()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)Ban.banList.Count);

                for (int i = 0; i < Ban.banList.Count; i++)
                {
                    writer.Write(Ban.banList[i]);
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/BanList.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }
    }

    /// <summary>
    /// 禁言
    /// </summary>
    public class ShutUp
    {
        public static List<uint> shutupList = new List<uint>();

        /// <summary>
        /// 读取禁言列表
        /// </summary>
        public static void LoadShutupList()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/ShutupList.bytes";

            if (!File.Exists(configBinaryFile)) { return; }

            using (FileStream stream = File.OpenRead(configBinaryFile))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < stream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    ushort count = reader.ReadUInt16();
                    for (int i = 0; i < count; i++)
                    {
                        ShutUp.shutupList.Add(reader.ReadUInt32());
                    }
                }
            }
        }

        /// <summary>
        /// 存储封号列表
        /// </summary>
        public static void SaveShutupList()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)ShutUp.shutupList.Count);

                for (int i = 0; i < ShutUp.shutupList.Count; i++)
                {
                    writer.Write(ShutUp.shutupList[i]);
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/ShutupList.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }
    }
}