using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace gmt
{
    /// <summary>
    /// 批量给予物品页面
    /// </summary>
    public partial class BatchGive : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public BatchGive()
            : base(PrivilegeType.BatchGive)
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
                    this.sourceList.Items.Add(theServer.Name);

                    if (theServer == server)
                    {
                        this.serverList.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = gmt.Server.GetServerAt(0);
                }

                if (GiftTable.GiftList != null)
                {
                    foreach (var config in GiftTable.GiftList)
                    {
                        this.giftDropDownList.Items.Add(new ListItem(config.id + " | " + TextManager.GetText(config.title), config.id.ToString()));
                    }
                }

            }
        }

        /// <summary>
        /// 服务器列表改变响应
        /// </summary>
        protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.serverList.SelectedIndex);
        }

        /// <summary>
        /// 玩家添加按钮点击响应
        /// </summary>
        protected void playerAddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.playerTextBox.Text))
            {
                this.reportLabel.Text = TableManager.GetGMTText(660);
                return;
            }

            string uidListText = this.playerTextBox.Text; 
            Regex rg = new Regex("[\\r\\n,]");
            uidListText = rg.Replace(uidListText, "\n");
            string[] idSet = uidListText.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var id in idSet)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    this.playerListBox.Items.Add(id);
                }
            }

            this.playerTextBox.Text = "";
        }

        /// <summary>
        /// 玩家移除按钮点击响应
        /// </summary>
        protected void playerRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.playerListBox.SelectedIndex < 0)
            {
                this.reportLabel.Text = TableManager.GetGMTText(661);
                return;
            }

            this.playerListBox.Items.RemoveAt(this.playerListBox.SelectedIndex);
        }

        /// <summary>
        /// 玩家清除按钮点击响应
        /// </summary>
        protected void playerClearButton_Click(object sender, EventArgs e)
        {
            this.playerListBox.Items.Clear();
        }

        /// <summary>
        /// 礼包添加按钮点击响应
        /// </summary>
        protected void giftAddButton_Click(object sender, EventArgs e)
        {
            if (this.giftDropDownList.Items == null)
            {
                this.reportLabel.Text = TableManager.GetGMTText(662);
                return;
            }

            if (this.giftListBox.Items.Count >= 5)
            {
                this.reportLabel.Text = "Only support 5 mails";
                return;
            }
            this.giftListBox.Items.Add(this.giftDropDownList.SelectedItem);
        }

        /// <summary>
        /// 礼包移除按钮点击响应
        /// </summary>
        protected void giftRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.giftListBox.SelectedIndex < 0)
            {
                this.reportLabel.Text = TableManager.GetGMTText(663);
                return;
            }

            this.giftListBox.Items.RemoveAt(this.giftListBox.SelectedIndex);
        }

        /// <summary>
        /// 礼包清除按钮点击响应
        /// </summary>
        protected void giftClearButton_Click(object sender, EventArgs e)
        {
            this.giftListBox.Items.Clear();
        }

        /// <summary>
        /// 给予按钮点击响应
        /// </summary>
        protected void giveButton_Click(object sender, EventArgs e)
        {
            if (this.playerListBox.Items.Count == 0)
            {
                this.reportLabel.Text = TableManager.GetGMTText(664);
                return;
            }

            if (this.giftListBox.Items.Count == 0)
            {
                this.reportLabel.Text = TableManager.GetGMTText(665);
                return;
            }

            List<string> uidList = new List<string>(this.playerListBox.Items.Count);
            foreach (ListItem item in this.playerListBox.Items)
            {
                uidList.Add(item.Text);
            }

            List<string> giftList = new List<string>(this.giftListBox.Items.Count);
            foreach (ListItem item in this.giftListBox.Items)
            {
                giftList.Add(item.Value);
            }

            this.reportLabel.Text = "";
            this.Send(uidList, giftList);
        }

        /// <summary>
        /// 发送
        /// </summary>
        /// <param name="uidSet">ID集合</param>
        /// <param name="cyidSet">畅游ID集合</param>
        /// <param name="giftText">礼包文本</param>
        private void Send(List<string> uidList, List<string> giftList)
        {
            if (uidList != null)
            {
                foreach (var uid in uidList)
                {
                    foreach (var giftid in giftList)
                    {
                        string cmd = string.Format("NG({0})", giftid);
                        this.ExecuteGmCommand(uid, cmd, "", true, text => this.reportLabel.Text += text);
                    }
                }
            }
        }

        /// <summary>
        /// 选择礼包下拉列表框选中值变化时响应
        /// </summary>
        protected void giftDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GiftTable.GiftList != null)
                this.giftLabel.Text = "*" + TextManager.GetText(GiftTable.GiftList[this.giftDropDownList.SelectedIndex].desc);
        }

        protected void gmButton_Click(object sender, EventArgs e)
        {
            List<string> uidList = new List<string>(this.playerListBox.Items.Count);
            foreach (var item in this.playerListBox.Items)
            {
                uidList.Add(item.ToString());
            }

            if (uidList.Count == 0)
            {

                this.reportLabel.Text = TableManager.GetGMTText(667);
                return;
            }

            this.reportLabel.Text = "";

            //转大写
            int starIdx = this.gmTextBox.Text.IndexOfAny("(".ToCharArray());
            string textA = this.gmTextBox.Text.Substring(0, starIdx);
            string textB = this.gmTextBox.Text.Substring(starIdx);
            string commond = textA.ToUpper() + textB;

            string[] uidArr = uidList.ToArray();
            if (uidList.Count != 0)
            {
                StringBuilder uidBuilder = new StringBuilder();
                for (int i = 0; i < uidArr.Length; i += 100)
                {
                    int end = Math.Min(i + 100, uidArr.Length);

                    for (int j = i; j < end; ++j)
                    {
                        if (j != i) { uidBuilder.Append(","); }
                        uidBuilder.Append(uidArr[j]);
                    }
                }

                string[] uidSet = uidBuilder.ToString().Split(',');
                HashSet<string> uidCheck = new HashSet<string>();

                foreach (var uid in uidSet)
                {
                    if (!string.IsNullOrEmpty(uid)
                        && !uidCheck.Contains(uid))
                    {
                        this.ExecuteGmCommand(uid, commond, "", true, text => this.reportLabel.Text += text);
                        uidCheck.Add(uid);
                    }
                }
            }
        }
    }
}