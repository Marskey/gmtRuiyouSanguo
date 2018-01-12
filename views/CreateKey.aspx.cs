using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 生成激活码页面
    /// </summary>
    public partial class CreateKey : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public CreateKey()
            : base(PrivilegeType.CreateKey)
        {
        }

        /// <summary>
        /// 渠道商集合
        /// </summary>
        public static readonly Channel[] ChannelSet;

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                gmt.Server server = Session["Server"] as gmt.Server;

                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server theServer = gmt.Server.GetServerAt(i);
                    this.serverList.Items.Add(theServer.Name);

                    if (theServer == server)
                    {
                        this.serverList.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = gmt.Server.GetServerAt(0);
                }

                foreach (Channel channel in CreateKey.ChannelSet)
                {
                    this.channelList.Items.Add(channel.Name);
                }

                DateTime expiryDate = DateTime.Today + new TimeSpan(1, 0, 0, 0);
                ViewState["expiryDate"] = expiryDate;
                this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
                this.countTextBox.Text = "1";
            }

            this.expiryDateTextBox.Attributes["onclick"] = ClientScript.GetPostBackEventReference(this.hideButton, null);
        }

        /// <summary>
        /// 隐藏按钮点击响应
        /// </summary>
        protected void hideButton_Click(object sender, EventArgs e)
        {
            this.expirySelectCalendar.Visible = true;
        }

        /// <summary>
        /// 失效选择日历选择响应
        /// </summary>
        protected void expirySelectCalendar_SelectionChanged(object sender, EventArgs e)
        {
            DateTime expiryDate = this.expirySelectCalendar.SelectedDate;
            ViewState["expiryDate"] = expiryDate;
            this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
            this.expirySelectCalendar.Visible = false;
        }

        /// <summary>
        /// 生成按钮点击响应
        /// </summary>
        protected void createButton_Click(object sender, EventArgs e)
        {
            if (this.channelList.SelectedIndex < 0)
            {
                return;
            }

            DateTime expiryDate = (DateTime)ViewState["expiryDate"];
            if (expiryDate <= DateTime.Today)
            {
                return;
            }

            int count = 0;
            if (!int.TryParse(this.countTextBox.Text, out count) || count <= 0)
            {
                return;
            }

            Channel channel = CreateKey.ChannelSet[this.channelList.SelectedIndex];

            gmt.Server keyServer = gmt.Server.GetServerAt(this.serverList.SelectedIndex);

            if (keyServer != null)
            {
                DatabaseAssistant.Execute
                (
                    keyServer.CodeDatabaseAddress,
                    keyServer.CodeDatabasePort,
                    keyServer.CodeDatabaseCharSet,
                    keyServer.CodeDatabase,
                    keyServer.CodeDatabaseUserId,
                    keyServer.CodeDatabasePassword,
                    "CALL xp_active_gen({0}, '{1}', {2}, {3}, {4}, {5});",
                    count,
                    channel.Prefix,
                    channel.NumberCount > 0,
                    channel.CharCount,
                    channel.Id,
                    (int)(expiryDate.Date - DateTime.Today).TotalDays
                );

                StringBuilder builder = new StringBuilder(TableManager.GetGMTText(695) + ":");

                DatabaseAssistant.Execute
                (
                    reader => { while (reader.Read()) { builder.Append("<br>").Append(reader.GetString(0)); } },
                    keyServer.CodeDatabaseAddress,
                    keyServer.CodeDatabasePort,
                    keyServer.CodeDatabaseCharSet,
                    keyServer.CodeDatabase,
                    keyServer.CodeDatabaseUserId,
                    keyServer.CodeDatabasePassword,
                    "SELECT `key_code` FROM `active` ORDER BY `gen_time` DESC LIMIT {0};",
                    count
                );

                this.resultLabel.Text = builder.ToString();
            }
        }

        /// <summary>
        /// 静态构造
        /// </summary>
        static CreateKey()
        {
            CreateKey.ChannelSet = new Channel[]
			{
                new Channel(0, "ALL", "ALL", 1, 8, '0'),
			};
        }

        /// <summary>
        /// 服务器列表改变响应
        /// </summary>
        protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.serverList.SelectedIndex);
        }
    }
}