using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 合并服务器
    /// </summary>
    public partial class MergeServer : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public MergeServer()
            : base(PrivilegeType.MergeServer)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server server = gmt.Server.GetServerAt(i);

                    this.fromDropDownList.Items.Add(new ListItem(server.Name, server.Name));
                    this.toDropDownList.Items.Add(new ListItem(server.Name, server.Name));
                }

            }

            this.outputLabel.Text = "";
        }

        /// <summary>
        /// 合服按钮点击响应
        /// </summary>
        protected void mergeButton_Click(object sender, EventArgs e)
        {
            if (this.fromDropDownList.SelectedIndex == this.toDropDownList.SelectedIndex)
            {
                this.outputLabel.Text = TableManager.GetGMTText(754);
                return;
            }

            gmt.Server fromServer = gmt.Server.GetServerAt(this.fromDropDownList.SelectedIndex);
            gmt.Server toServer = gmt.Server.GetServerAt(this.toDropDownList.SelectedIndex);

            if (DatabaseAssistant.Execute
            (
                fromServer.DatabaseAddress,
                fromServer.DatabasePort,
                fromServer.DatabaseCharSet,
                fromServer.GameDatabase,
                fromServer.DatabaseUserId,
                fromServer.DatabasePassword,
                "CALL `xp_mix_data`('{0}',{1});", toServer.GameDatabase, fromServer.Id.Split('-')[1]
            ))
            {
                this.outputLabel.Text = TableManager.GetGMTText(755);
            }
            else
            {
                this.outputLabel.Text = TableManager.GetGMTText(756);
            }
        }
    }
}