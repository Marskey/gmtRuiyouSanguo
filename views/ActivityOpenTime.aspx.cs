using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    public partial class ActivityOpenTime : AGmPage
    {
        public ActivityOpenTime()
            : base(PrivilegeType.ActivityOpenTime)
        {
        }

        //private bool needReturn = true;
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
                    this.OpentimeserverList.Items.Add(theServer.Name);

                    if (theServer == server)
                    {
                        this.OpentimeserverList.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = gmt.Server.GetServerAt(0);
                }


                DateTime expiryDate = DateTime.Today + new TimeSpan(1, 0, 0, 0);
                ViewState["expiryDate"] = expiryDate;

                this.expiryDateTextBox.Text = expiryDate.ToShortDateString();

                this.expiryDateTextBox.Attributes["onclick"] = ClientScript.GetPostBackEventReference(this.hideButton, null);
                UpdateActivityOpenTimeListBox();
            }

        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="commandText"></param>
        /// <param name="operateText"></param>
        /// <param name="needReturn"></param>
        /// <param name="reportProcess"></param>
        /// <returns></returns>
        protected override bool ExecuteGmCommand(string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        {
            bool success = true;
            string account = Session["user"] as string;

            gmt.Server keyServer = gmt.Server.GetServerAt(this.OpentimeserverList.SelectedIndex);
            if (keyServer != null)
            {
                success &= AGmPage.ExecuteGmCommand(account, keyServer, "0", commandText, operateText, needReturn, reportProcess);
            }

            return success;
        }

        /// <summary>
        /// 确认设置按钮点击响应
        /// </summary>
        protected void ActivityOpenButton_Click(object sender, EventArgs e)
        {
            gmt.Server keyServer = gmt.Server.GetServerAt(this.OpentimeserverList.SelectedIndex);

            string[] date = this.expiryDateTextBox.Text.Split('/');

            if (keyServer != null)
            {
                if (this.ExecuteGmCommand("0", string.Format("ODATE({0},{1},{2})", date[0], date[1], date[2]), "", true, text => { this.reportLabel.Text = text; }))
                    UpdateActivityOpenTimeListBox();
            }
            else
            {
                this.reportLabel.Text = TableManager.GetGMTText(423);
            }

        }

        /// <summary>
        /// 隐藏按钮点击响应
        /// </summary>
        protected void hideButton_Click(object sender, EventArgs e)
        {
            this.expirySelectCalendar.Visible = true;
        }

        /// <summary>
        /// 日历点击响应
        /// </summary>
        protected void expirySelectCalendar_SelectionChanged(object sender, EventArgs e)
        {
            DateTime expiryDate = this.expirySelectCalendar.SelectedDate;
            ViewState["expiryDate"] = expiryDate;
            this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
            this.expirySelectCalendar.Visible = false;
        }

        /// <summary>
        /// 更新显示所有服务器的开服活动时间
        /// </summary>
        private void UpdateActivityOpenTimeListBox()
        {
            this.activityOpenTimeListBox.Items.Clear();

            for (int i = 0; i < gmt.Server.Count; ++i)
            {
                gmt.Server server = gmt.Server.GetServerAt(i);
                if (server != null)
                {
                    STDateInfo stInfo;
                    GetActivityOpenTime(server, out stInfo);

                    string str = server.Name + "\t" + TableManager.GetGMTText(424) + "：[";
                    if (stInfo.openDate == 0)
                    {
                        str += TableManager.GetGMTText(425);
                    }
                    else
                    {
                        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(stInfo.openDate);
                        str += dt.Year + TableManager.GetGMTText(426) + dt.Month + TableManager.GetGMTText(427) + dt.Day + TableManager.GetGMTText(428);
                    }
                    str += "]";

                    str += "\t" + TableManager.GetGMTText(429) + ": [";
                    if (stInfo.curSDate == 0)
                    {
                        str += TableManager.GetGMTText(425);
                    }
                    else
                    {
                        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(stInfo.curSDate);
                        str += dt.Year + TableManager.GetGMTText(426) + dt.Month + TableManager.GetGMTText(427) + dt.Day + TableManager.GetGMTText(428);
                    }
                    str += "]";

                    str += "\t" + TableManager.GetGMTText(430) + ": [";
                    if (stInfo.nextSDate == 0)
                    {
                        str += TableManager.GetGMTText(425);
                    }
                    else
                    {
                        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(stInfo.nextSDate);
                        str += dt.Year + TableManager.GetGMTText(426) + dt.Month + TableManager.GetGMTText(427) + dt.Day + TableManager.GetGMTText(428);
                    }
                    str += "]";

                    str += "\t" + TableManager.GetGMTText(22037) + ": [";
                    if (stInfo.rmbDate == 0)
                    {
                        str += TableManager.GetGMTText(425);
                    }
                    else
                    {
                        DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(stInfo.rmbDate);
                        str += dt.Year + TableManager.GetGMTText(426) + dt.Month + TableManager.GetGMTText(427) + dt.Day + TableManager.GetGMTText(428);
                    }
                    str += "]";

                    this.activityOpenTimeListBox.Items.Add(new ListItem(str));
                }
            }

            #region 开服活动时间Label 已注释
            //this.acyivityOpenTimeLabel.Text = "";

            //StringBuilder builder = new StringBuilder();
            //builder.Append("<Table>");

            //for (int i = 0; i < gmt.Server.Count; ++i)
            //{
            //    builder.Append("<tr>");
            //    gmt.Server theServer = gmt.Server.GetServerAt(i);
            //    builder.Append("<td colspan='2'>" + theServer.Name + "</td>");
            //    builder.Append("<td></td>");
            //    builder.Append("<td>开服活动时间：</td>");
            //    if (theServer != null)
            //    {
            //        int openTime = this.GetActivityOpenTime(theServer);

            //        if (openTime == 0)
            //        {
            //            builder.Append("<td style='color:red'>未设置</td>");
            //        }
            //        else
            //        {
            //            DateTime date = new DateTime();
            //            date = new DateTime(1970, 1, 1, 8, 0, 0) + TimeSpan.FromSeconds(openTime);
            //            builder.Append("<td>" + date.Year + "年" + date.Month + "月" + date.Day + "日</td>");
            //        }
            //    }
            //    builder.Append("</tr>");
            //}

            //this.acyivityOpenTimeLabel.Text = builder.ToString();
            #endregion
        }

        public class STDateInfo
        {
            public STDateInfo()
            {
                openDate = 0;
                curSDate = 0;
                nextSDate = 0;
                rmbDate = 0;
            }
            public int openDate;
            public int curSDate;
            public int nextSDate;
            public int rmbDate;
        }

        /// <summary>
        /// 获取服务器开服活动时间
        /// </summary>
        public static bool GetActivityOpenTime(gmt.Server server, out STDateInfo stInfo)
        {
            mw.GS_SVR_DATE_INFO svrDateInfo = null;

            #region 从数据库读取信息
            DatabaseAssistant.Execute(
                        reader =>
                        {
                            if (reader.Read())
                            {
                                int nIdx = 0;
                                if (!reader.IsDBNull(nIdx))
                                {
                                    long size = reader.GetBytes(nIdx, 0, null, 0, int.MaxValue);
                                    if (0 != size)
                                    {
                                        byte[] dateBuffer = new byte[size];
                                        reader.GetBytes(nIdx, 0, dateBuffer, 0, dateBuffer.Length);
                                        using (MemoryStream stream = new MemoryStream(dateBuffer))
                                        {
                                            svrDateInfo = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.GS_SVR_DATE_INFO)) as mw.GS_SVR_DATE_INFO;
                                        }
                                    }
                                }
                            }
                        },
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        "SELECT `svr_date_data` FROM `global`;"
                        );
            #endregion

            stInfo = new STDateInfo();
            if (null != svrDateInfo)
            {
                stInfo.openDate = svrDateInfo.openDate;
                stInfo.curSDate = svrDateInfo.signDate;
                stInfo.nextSDate = svrDateInfo.singReset;
                stInfo.rmbDate = svrDateInfo.rmbDate;
                return true;
            }
            return false;
        }

    }
}
