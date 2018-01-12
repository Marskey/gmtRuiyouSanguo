using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    public partial class RunandFixManage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                gmt.Server server = Session["Server"] as gmt.Server;

                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server theServer = gmt.Server.GetServerAt(i);
                    this.Serverlist.Items.Add(theServer.Name);
                    this.ImitateloginServer.Items.Add(theServer.Name);
                    if (theServer == server)
                    {
                        this.Serverlist.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = gmt.Server.GetServerAt(0);
                }

                foreach (Channel channel in CreateKey.ChannelSet)
                {
                    this.ChannelList.Items.Add(channel.Name);
                }

                //服务器端启动的服务


            }
        }

        //最大同时在线人数
        protected void MaxOnlineTextBox_TextChanged(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(870) + ":" + this.MaxOnlineTextBox.Text;
        }

        //客户端资源文件下载地址
        protected void URLOfClientResourceTextBox_TextChanged(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(871) + ":" + this.URLOfClientResourceTextBox.Text;
        }

        //最低版本号
        protected void MinVersionTextBox_TextChanged(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(872) + ":" + this.MinVersionTextBox.Text;
        }

        //新包下载地址
        protected void URLOfNewPackageTextBox_TextChanged(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(873) + ":" + this.URLOfNewPackageTextBox.Text;
        }

        //是否强制单选按钮
        protected void ForceUpdateCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this.CheckBox1.Checked == true)
                this.reportLabel.Text = TableManager.GetGMTText(869);
            if (this.CheckBox1.Checked == false)
                this.reportLabel.Text = TableManager.GetGMTText(874);
            //this.ExecuteGmCommand("0", string.Format("NC({0})", 5040), "", false, text => this.reportLabel .Text += text);
        }

        protected bool ExecuteGmCommand(string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        {
            Server server = Session["Server"] as Server;
            string account = Session["user"] as string;

            return AGmPage.ExecuteGmCommand(account, server, playerId, commandText, operateText, needReturn, reportProcess);
        }


        //服务器某一个server的状态
        protected void Checkservers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        //查看服务器某一个server按钮
        protected void SeeServerState_Click(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(861);
        }

        //查看服务器某一个server的运行状态（开启或关闭）
        protected void ServerStateTextBox_TextChanged(object sender, EventArgs e)
        {
            this.reportLabel.Text = TableManager.GetGMTText(875);
        }



        //选择要查看的服务器
        protected void ServerList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.Serverlist.SelectedIndex);
        }

        //选择渠道
        protected void ChannelList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //添加白名单
        protected void WhiteNameAddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.WhiteNameTextBox.Text))
            {
                this.reportLabel.Text = TableManager.GetGMTText(876);
                return;
            }
            if (this.WhiteNameTextBox.Text == "" || this.WhiteNameTextBox.Text == String.Empty)
            {
                return;
            }

            string[] idSet = this.WhiteNameTextBox.Text.Replace("\r\n", "\n").Split('\n');
            //当前已经加入的白名单
            //string[] curSet=this.WhiteNameListBox.Text.Replace("\r\n", "\n").Split('\n');
            foreach (var id in idSet)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    this.WhiteNameListBox.Items.Add(id);
                    this.reportLabel.Text = TableManager.GetGMTText(877);
                }
            }

            this.WhiteNameTextBox.Text = "";
        }

        //移除白名单
        protected void WhiteNameRemoveButton_Click(object sender, EventArgs e)
        {
            if (this.WhiteNameListBox.SelectedIndex < 0)
            {
                this.reportLabel.Text = TableManager.GetGMTText(878);
                return;
            }
            this.WhiteNameListBox.Items.RemoveAt(this.WhiteNameListBox.SelectedIndex);
            this.reportLabel.Text = TableManager.GetGMTText(879);
        }

        //是否全服
        protected void AllServerCheckBox_CheckedChanged1(object sender, EventArgs e)
        {
            if (this.AllServerCheckBox.Checked == true)
                this.reportLabel.Text = TableManager.GetGMTText(880);
            if (this.AllServerCheckBox.Checked == false)
                this.reportLabel.Text = TableManager.GetGMTText(881);
        }

        protected void ImitateloginServer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void WhiteNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        protected void WhiteNameListBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void WhiteNameClearButton_Click(object sender, EventArgs e)
        {
            this.WhiteNameListBox.Items.Clear();
            this.reportLabel.Text = TableManager.GetGMTText(882);
        }

    }
}