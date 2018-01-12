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
    /// 生成礼包码页面
    /// </summary>
    public partial class CreateGift : AGmPage
    {
        public CreateGift()
            : base(PrivilegeType.CreateGift)
        {
        }

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

                DateTime expiryDate = DateTime.Today;
                ViewState["expiryDate"] = expiryDate;
                this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
                this.countTextBox.Text = "1";
                for (int i = 0; i < 100; i++)
                {
                    this.Mutitimes.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
                this.Mutitimes.SelectedIndex = 0;

                if (GiftTable.GiftList != null)
                {
                    this.giftDropDownList.Items.Clear();
                    foreach (var config in GiftTable.GiftList)
                    {
                        this.giftDropDownList.Items.Add(new ListItem(config.id + " | " + TextManager.GetText(config.title), config.id.ToString()));
                    }
                    this.giftLabel.Text = "*" + TextManager.GetText(GiftTable.GiftList[this.giftDropDownList.SelectedIndex].desc);
                }
            }

            object channelSelected = ViewState["ChannelSelected"];
            Channel currentChannel = CreateKey.ChannelSet[this.channelList.SelectedIndex];

            this.expiryDateTextBox.Attributes["onclick"] = ClientScript.GetPostBackEventReference(this.hideButton, null);




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
        /// 隐藏按钮点击响应
        /// </summary>
        protected void hideButton_Click(object sender, EventArgs e)
        {
            this.expirySelectCalendar.Visible = true;
        }

        /// <summary>
        /// 生成按钮点击响应
        /// </summary>
        protected void createButton_Click(object sender, EventArgs e)
        {
            this.errorLabel.Text = "";

            if (this.channelList.SelectedIndex < 0)
            {
                return;
            }

            if (this.typeTextBox.Text == null || this.typeTextBox.Text == "")
            {
                return;
            }

            if (this.giftDropDownList.Items == null)
            {
                return;
            }

            int count = 0;
            if (!int.TryParse(this.countTextBox.Text, out count) || count <= 0)
            {
                return;
            }

            //如果使用次数不填,则默认为1
            if (string.IsNullOrEmpty(this.UseTimes.Text))
            {
                this.UseTimes.Text = "1";
            }


            Channel channel = CreateKey.ChannelSet[this.channelList.SelectedIndex];
            gmt.Server keyServer = gmt.Server.GetServerAt(this.serverList.SelectedIndex);

            int selectvalue = 0;
            if (!int.TryParse(this.Mutitimes.SelectedValue, out selectvalue))
                return;

            if (keyServer != null)
            {
                bool error = DatabaseAssistant.Execute
                (
                    keyServer.CodeDatabaseAddress,
                    keyServer.CodeDatabasePort,
                    keyServer.CodeDatabaseCharSet,
                    keyServer.CodeDatabase,
                    keyServer.CodeDatabaseUserId,
                    keyServer.CodeDatabasePassword,
                    "CALL xp_gift_gen({0}, {1}, {2}, '{3}', {4}, '{5}','{6}','{7}','{8}');",
                    count,
                    10,
                    channel.Id,
                    DatabaseAssistant.DetectionSql(this.typeTextBox.Text),
                    this.giftDropDownList.SelectedValue,
                    channel.GiftChar,
                    ((DateTime)ViewState["expiryDate"]).ToString("yyyy-MM-dd"), int.Parse(this.UseTimes.Text), selectvalue
                );

                if (!error)
                {
                    this.resultLabel.Text = TableManager.GetGMTText(691);
                    return;
                }

                StringBuilder builder = new StringBuilder(TableManager.GetGMTText(692) + ":");

                DatabaseAssistant.Execute
                (
                    reader => { while (reader.Read()) { builder.Append("<br>").Append(reader.GetString(0)); } },
                    keyServer.CodeDatabaseAddress,
                    keyServer.CodeDatabasePort,
                    keyServer.CodeDatabaseCharSet,
                    keyServer.CodeDatabase,
                    keyServer.CodeDatabaseUserId,
                    keyServer.CodeDatabasePassword,
                    "SELECT `gift_code` FROM `gift` ORDER BY `gen_time` DESC LIMIT {0};",
                    count
                );

                this.resultLabel.Text = builder.ToString();
            }
        }

        /// <summary>
        /// 服务器列表改变响应
        /// </summary>
        protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.serverList.SelectedIndex);
        }

        protected void UseTimes_TextChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 选择礼包下拉列表框选中值变化时响应
        /// </summary>
        protected void giftDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GiftTable.GiftList != null)
                this.giftLabel.Text = "*" + TextManager.GetText(GiftTable.GiftList[this.giftDropDownList.SelectedIndex].desc);
        }

        /// <summary>
        /// 礼包类型输入框值变化时响应
        /// </summary>
        protected void typeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (this.typeTextBox.Text.Length > 2)
            {
                this.errorLabel.Text = TableManager.GetGMTText(693);
                this.typeTextBox.Text = "";
                return;
            }

            this.typeTextBox.Text = this.typeTextBox.Text.ToLower();
            for (int i = 0; i < this.typeTextBox.Text.Length; ++i)
            {
                char theChar = this.typeTextBox.Text[i];

                if (!((theChar >= '0' && theChar <= '9') || (theChar >= 'a' && theChar <= 'z')))
                {
                    this.errorLabel.Text = TableManager.GetGMTText(694);
                    this.typeTextBox.Text = "";
                    return;
                }
            }

            if (this.typeTextBox.Text.Length == 2)
            {
                if (this.typeTextBox.Text[0] >= 'a' && this.typeTextBox.Text[0] <= 'z' || this.typeTextBox.Text[0] > '3' || (this.typeTextBox.Text[0] == '3' && this.typeTextBox.Text[1] >= 'j' && this.typeTextBox.Text[1] <= 'z'))
                {
                    this.Mutitimes.Enabled = true;
                }
                else
                {
                    this.Mutitimes.SelectedIndex = 0;
                    this.Mutitimes.Enabled = false;
                }
            }
        }

    }
}