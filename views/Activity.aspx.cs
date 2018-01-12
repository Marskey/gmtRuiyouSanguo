using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 活动页面
    /// </summary>
    public partial class Activity : AGmPage
    {
        private DateTime nextSDate;

        /// <summary>
        /// 构造方法
        /// </summary>
        public Activity()
            : base(PrivilegeType.Activity)
        {
        }

        public static Dictionary<int, ActivityData> m_curActivityDictionary;
        public static Dictionary<int, ActivityData> m_nextActivityDictionary;

        /// <summary>
        /// 当前日期
        /// </summary>
        private static DateTime m_currentDate;

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
                    this.serverListBox.Items.Add(theServer.Name);

                    if (theServer == server)
                    {
                        this.serverList.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = gmt.Server.GetServerAt(0);

                    ActivityManger.m_curServer = gmt.Server.GetServerAt(0);
                }

                Activity.m_curActivityDictionary = new Dictionary<int, ActivityData>();
                Activity.m_nextActivityDictionary = new Dictionary<int, ActivityData>();

                DateTime expiryDate = DateTime.Today;
                ViewState["expiryDate"] = expiryDate;
                this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
                this.expiryDateTextBox.Attributes["onclick"] = ClientScript.GetPostBackEventReference(this.hideButton, null);

                this.UpdateList();
                this.applyButton.Enabled = false;
            }

        }

        /// <summary>
        /// 服务器列表改变响应
        /// </summary>
        protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.serverList.SelectedIndex);
            //记录
            ActivityManger.m_curServer = gmt.Server.GetServerAt(this.serverList.SelectedIndex);
            this.UpdateList();
        }

        private void AddCurActivity(int id, int duration, int delay, int forever, int param)
        {
            ActivityData data = new ActivityData(id, duration, delay, forever, param);
            if (m_curActivityDictionary.ContainsKey(data.Id))
            {
                m_curActivityDictionary[data.Id] = data;
            }
            else
            {
                m_curActivityDictionary.Add(data.Id, data);
            }

            mw.ActivityConfig activityConfig = null;
            if (!ActivityManger.ActivityDataDictionary.TryGetValue(data.Id, out activityConfig))
            {
                activityConfig = GMTActivityMananger.GetActivityConfigsById(data.Id);
            }

            this.currentListBox.Items.Add(new ListItem(ActivityManger.GetListText(
                    data.Id
                    , activityConfig
                    , duration
                    , delay
                    , forever
                    , param
                    , this.nextSelectCalendar.SelectedDate
                    )
                , id.ToString())
                );
        }

        private void AddNextActivity(int id, int duration, int delay, int forever, int param)
        {
            ActivityData data = new ActivityData(id, duration, delay, forever, param);
            if (m_nextActivityDictionary.ContainsKey(data.Id))
            {
                this.errorLabel.Text = TableManager.GetGMTText(406);
                return;
            }
            else
            {
                m_nextActivityDictionary.Add(data.Id, data);
            }

            mw.ActivityConfig activityConfig = null;
            if (!ActivityManger.ActivityDataDictionary.TryGetValue(data.Id, out activityConfig))
            {
                activityConfig = GMTActivityMananger.GetActivityConfigsById(data.Id);
            }

            this.nextListBox.Items.Add(new ListItem(ActivityManger.GetListText(
                    data.Id
                    , activityConfig
                    , duration
                    , delay
                    , forever
                    , param
                    , this.nextSelectCalendar.SelectedDate
                    )
                , id.ToString())
                );
        }

        /// <summary>
        /// 添加活动按钮点击响应
        /// </summary>
        protected void addActivityButton_Click(object sender, EventArgs e)
        {
            if (this.allListBox.SelectedIndex < 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(407);
                return;
            }

            int duration;
            if (!int.TryParse(this.durationTextBox.Text, out duration))
            {
                this.errorLabel.Text = TableManager.GetGMTText(408);
                return;
            }

            int delay;
            if (!int.TryParse(this.delayTextBox.Text, out delay) || delay < 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(409);
                return;
            }

            int forever = 0;
            if (this.forverCheckBox.Checked)
            {
                forever = 1;
                return;
            }

            int param = 0;
            if (!int.TryParse(this.paramTextBox.Text, out param) || param < 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(410);
                return;
            }

            int id = int.Parse(this.allListBox.SelectedItem.Value);

            Button button = (Button)sender;
            if (button.ID == "addCurActivityButton")
            {
                AddCurActivity(id, duration, delay, forever, param);
                if (9 == id)
                {
                    AddCurActivity(10, duration + 1, delay, 0, 0);
                }
            }
            else if (button.ID == "addNextActivityButton")
            {
                AddNextActivity(id, duration, delay, forever, param);
                if (9 == id)
                {
                    AddNextActivity(10, duration + 1, delay, 0, 0);
                }
            }

            this.applyButton.Enabled = true;
            //this.addListBox.Items.Add(this.allListBox.SelectedItem);
            //this.allListBox.Items.RemoveAt(this.allListBox.SelectedIndex);
        }

        /// <summary>
        /// 移除活动按钮点击响应
        /// </summary>
        protected void removeButton_Click(object sender, EventArgs e)
        {
            //if (this.addListBox.SelectedIndex < 0) { return; }

            //int index = this.addListBox.SelectedIndex;

            //int activityId = int.Parse(this.addListBox.SelectedItem.Value);

            //Activity.m_editActivityDictionary.Remove(activityId);

            //mw.ActivityConfig activityConfig;
            //if (!ActivityManger.ActivityDataDictionary.TryGetValue(activityId, out activityConfig))
            //{
            //    OtherActivityManager.ActivityDictionary.TryGetValue(activityId, out activityConfig);
            //}

            //this.allListBox.Items.Add(new ListItem(ActivityManger.GetListText(activityId, activityConfig, 0, 0, 0, 0, new DateTime()), this.addListBox.SelectedItem.Value));
            //this.addListBox.Items.RemoveAt(index);

            //this.addListBox.SelectedIndex = Math.Min(index, this.addListBox.Items.Count - 1);
        }

        /// <summary>
        /// 添加普通活动按钮点击响应
        /// </summary>
        protected void normalButton_Click(object sender, EventArgs e)
        {
            //this.AddEditedActivity(9, 3, 1, 0, -1);
            ////this.AddEditedActivity(10, 4, 1, 0, -1);
            //this.AddEditedActivity(14, 4, 3, 0, -1);

            //this.AddNextActivity(1001, 3, 0);
            //this.AddNextActivity(1002, 3, 0);
            //this.AddNextActivity(1003, 3, 0);
            //this.AddNextActivity(1004, 3, 0);
            //this.AddNextActivity(1005, 3, 0);
            //this.AddNextActivity(2001, 3, 0);
            //this.AddNextActivity(2002, 3, 0);
            //this.AddNextActivity(2003, 3, 0);
            //this.AddNextActivity(2004, 3, 0);
            //this.AddNextActivity(2005, 3, 0);
            //System.Threading.Thread.Sleep(1000);
            //UpdateList();
        }

        /// <summary>
        /// 添加QQ活动按钮点击响应
        /// </summary>
        protected void qqButton_Click(object sender, EventArgs e)
        {

            //this.AddNextActivity(1001, 7, 0);
            //this.AddNextActivity(1002, 7, 0);
            //this.AddNextActivity(1003, 7, 0);
            //this.AddNextActivity(1007, 7, 0);
            //this.AddNextActivity(1014, -1, 0);
            //this.AddNextActivity(1018, 7, 0);
            //this.AddNextActivity(1029, -1, 0);
            //this.AddNextActivity(2001, 7, 0);
            //this.AddNextActivity(2002, 7, 0);
            //this.AddNextActivity(2003, 7, 0);
            //this.AddNextActivity(2007, 7, 0);
            //this.AddNextActivity(2014, -1, 0);
            //this.AddNextActivity(2018, 7, 0);
            //this.AddNextActivity(2029, -1, 0);
            //this.AddNextActivity(3502, -1, 0);
            //this.AddNextActivity(3511, 7, 0);
        }

        /// <summary>
        /// 添加1060活动按钮点击响应
        /// </summary>
        protected void Button1060_Click(object sender, EventArgs e)
        {
            //this.AddNextActivity(1060, -1, 0);
        }

        /// <summary>
        /// 添加VIP打折活动按钮点击响应
        /// </summary>
        protected void vipDiscountButton_Click(object sender, EventArgs e)
        {
            //this.AddNextActivity(322, -1, 0);
        }

        ///// <summary>
        ///// 立即更新活动按钮点击响应
        ///// </summary>
        //protected void immediateButton_Click(object sender, EventArgs e)
        //{
        //    Server server = Session["Server"] as gmt.Server;
        //    List<Server> otherserver = new List<Server>();
        //    otherserver.Add(server);

        //    if (m_editActivityDictionary.Count != 0)
        //    {
        //        this.errorLabel.Text = ActivityManger.NtfTable(Activity.m_editActivityDictionary, otherserver, true);
        //        System.Threading.Thread.Sleep(1000);
        //        UpdateList();
        //    }
        //}

        /// <summary>
        /// 立即更新时间按钮点击响应
        /// </summary>
        protected void updateTimeButton_Click(object sender, EventArgs e)
        {
            if (nextSelectCalendar.Visible)
            {
                if (this.nextSelectCalendar.SelectedDate <= m_currentDate)
                {
                    this.errorLabel.Text = TableManager.GetGMTText(411);
                    return;
                }

                if (nextSelectCalendar.SelectedDate == nextSDate)
                {
                    return;
                }

                //this.ExecuteGmCommand
                //(
                //    "0",
                //    string.Format("SDATE({0},{1},{2})",
                //    this.nextSelectCalendar.SelectedDate.Year,
                //    this.nextSelectCalendar.SelectedDate.Month,
                //    this.nextSelectCalendar.SelectedDate.Day),
                //    "",
                //    true,
                //    text => { this.errorLabel.Text = text; }
                //);

                this.nextDateTextBox.Text = this.nextSelectCalendar.SelectedDate.ToShortDateString();
                nextSDate = this.nextSelectCalendar.SelectedDate;


                DateTime expiryDate = this.nextSelectCalendar.SelectedDate;
                ViewState["expiryDate"] = expiryDate;
                this.expiryDateTextBox.Text = expiryDate.ToShortDateString();

                this.nextSelectCalendar.Visible = false;
                this.updateTimeButton.Visible = false;
                this.closeTimeButton.Visible = false;
                this.applyButton.Enabled = true;
            }
        }

        /// <summary>
        /// 保存活动与时间按钮点击响应
        /// </summary>
        protected void saveButton_Click(object sender, EventArgs e)
        {
            //gmt.Server server = Session["Server"] as gmt.Server;
            //if (server == null) { return; }

            //List<Server> othersever = new List<Server>();
            //othersever.Add(server);

            //lock (ActivityManger.ActivityServerDictionary)
            //{
            //    ActivityServerConfig config;
            //    if (!ActivityManger.ActivityServerDictionary.TryGetValue(server, out config))
            //    {
            //        ActivityManger.ActivityServerDictionary.Add(server, config = new ActivityServerConfig());
            //    }
            //    else
            //    {
            //        config.NextActivityDictionary.Clear();
            //    }

            //    config.NextActivityDate = this.nextSelectCalendar.SelectedDate;
            //    foreach (var pair in Activity.editActivityDictionary)
            //    {
            //        config.NextActivityDictionary.Add(pair.Key, new ActivityData(pair.Value.Id, pair.Value.Duration, pair.Value.Delay, pair.Value.Forever));
            //    }

            //    ActivityManger.Save();
            //    if (ActivityManger.UpdateService(othersever))
            //    {
            //        this.errorLabel.Text = "保存完毕";
            //    }
            //    else
            //    {
            //        this.errorLabel.Text = "错误：未能连接GMT后台服务";
            //    }
            //}
        }

        /// <summary>
        /// 下次选择日历选择响应
        /// </summary>
        protected void nextSelectCalendar_SelectionChanged(object sender, EventArgs e)
        {
            this.errorLabel.Text = "";
            if (this.nextSelectCalendar.SelectedDate <= m_currentDate)
            {
                this.errorLabel.Text = TableManager.GetGMTText(412);
                this.updateTimeButton.Enabled = false;
                return;
            }

            if (nextSelectCalendar.SelectedDate == nextSDate)
            {
                return;
            }

            updateTimeButton.Enabled = true;

            //Dictionary<int, mw.ActivityConfig>[] activitySet = { ActivityManger.ActivityDataDictionary, OtherActivityManager.ActivityDictionary };

            //this.nextListBox.Items.Clear();

            //foreach (var dictionary in activitySet)
            //{
            //    foreach (var pair in dictionary)
            //    {
            //        ActivityData activityData;
            //        if (m_editActivityDictionary.TryGetValue(pair.Key, out activityData))
            //        {
            //            this.nextListBox.Items.Add(new ListItem(ActivityManger.GetListText(pair.Key, pair.Value, activityData.Duration, activityData.Delay, activityData.Forever, activityData.Param, this.nextSelectCalendar.SelectedDate), pair.Key.ToString()));
            //        }
            //    }
            //}
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

        /// <summary>
        /// 更新列表
        /// </summary>
        private void UpdateList()
        {
            this.currentListBox.Items.Clear();
            this.nextListBox.Items.Clear();
            this.errorLabel.Text = "";
            m_currentDate = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            this.curDate.Text = TableManager.GetGMTText(413);
            this.nextDateTextBox.Text = TableManager.GetGMTText(413);
            m_curActivityDictionary.Clear();
            m_nextActivityDictionary.Clear();

            gmt.Server server = Session["Server"] as gmt.Server;
            ActivityServerConfig config = null;

            Dictionary<int, mw.ActivityConfig>[] activitySet = { 
                ActivityManger.ActivityDataDictionary
                , GMTActivityMananger.GetTableActivity() 
            };

            if (server != null)
            {
                mw.GS_SVR_DATE_INFO svrDateInfo = null;
                mw.AUTH_GMT_SETTINT_Ntf gmtSetting = null;
                mw.AUTH_GMT_SETTINT_Ntf NextASetting = null;

                DatabaseAssistant.Execute(reader =>
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

                        nIdx = 1;
                        if (!reader.IsDBNull(nIdx))
                        {
                            long size = reader.GetBytes(nIdx, 0, null, 0, int.MaxValue);
                            if (0 != size)
                            {
                                byte[] dateBuffer = new byte[size];
                                reader.GetBytes(nIdx, 0, dateBuffer, 0, dateBuffer.Length);
                                using (MemoryStream stream = new MemoryStream(dateBuffer))
                                {
                                    gmtSetting = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.AUTH_GMT_SETTINT_Ntf)) as mw.AUTH_GMT_SETTINT_Ntf;
                                }
                            }
                        }

                        nIdx = 2;
                        if (!reader.IsDBNull(nIdx))
                        {
                            long size = reader.GetBytes(nIdx, 0, null, 0, int.MaxValue);
                            if (0 != size)
                            {
                                byte[] dateBuffer = new byte[size];
                                reader.GetBytes(nIdx, 0, dateBuffer, 0, dateBuffer.Length);
                                using (MemoryStream stream = new MemoryStream(dateBuffer))
                                {
                                    NextASetting = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.AUTH_GMT_SETTINT_Ntf)) as mw.AUTH_GMT_SETTINT_Ntf;
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
                "SELECT `svr_date_data`,`gmt_activity_data`,`gmt_next_activity_data` FROM `global`;");

                List<mw.ActivityConfig> activitylist = new List<mw.ActivityConfig>();

                if (gmtSetting != null)
                {
                    for (int i = 0; i < gmtSetting.info.Count; i++)
                    {
                        mw.ActivityConfig activity = null;
                        var s = new mw_serializer0();
                        activity = s.Deserialize(new MemoryStream(gmtSetting.info[i].data), null, typeof(mw.ActivityConfig)) as mw.ActivityConfig;
                        activitylist.Add(activity);
                    }
                }

                if (NextASetting != null)
                {
                    for (int i = 0; i < NextASetting.info.Count; i++)
                    {
                        mw.ActivityConfig activity = null;
                        var s = new mw_serializer0();
                        activity = s.Deserialize(new MemoryStream(NextASetting.info[i].data), null, typeof(mw.ActivityConfig)) as mw.ActivityConfig;
                        AddNextActivity(activity.id, activity.last_days, activity.start_days, activity.eternal, activity.param);
                    }
                }

                if (svrDateInfo != null)
                {
                    if (0 != svrDateInfo.signDate)
                    {
                        m_currentDate += TimeSpan.FromSeconds(svrDateInfo.signDate);
                        this.curDate.Text = m_currentDate.ToShortDateString();
                    }

                    if (0 != svrDateInfo.singReset)
                    {
                        this.nextSelectCalendar.SelectedDate = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)) + TimeSpan.FromSeconds(svrDateInfo.singReset);
                        this.nextDateTextBox.Text = nextSelectCalendar.SelectedDate.ToShortDateString();
                    }
                }

                if (gmtSetting != null)
                {
                    foreach (var data in activitylist)
                    {
                        if (data.last_days <= 0)
                        //|| !string.IsNullOrEmpty(data.gmt_no_use))
                        {
                            continue;
                        }

                        mw.ActivityConfig aconfig = null;
                        if (!ActivityManger.ActivityDataDictionary.TryGetValue(data.id, out aconfig))
                        {
                            aconfig = GMTActivityMananger.GetActivityConfigsById(data.id);
                        }

                        if (aconfig == null)
                        {
                            continue;
                        }

                        m_curActivityDictionary.Add(data.id, new ActivityData(data.id, data.last_days, data.start_days, data.eternal, data.param));
                        this.currentListBox.Items.Add(new ListItem(ActivityManger.GetListText(
                                data.id
                                , aconfig
                                , data.last_days
                                , data.start_days
                                , data.eternal
                                , data.param
                                , m_currentDate
                                )
                            , data.id.ToString()));
                    }
                }

                //ActivityManger.ActivityServerDictionary.TryGetValue(server, out config);
                //if (config != null)
                //{
                //    foreach (var dictionary in activitySet)
                //    {
                //        foreach (var pair in dictionary)
                //        {
                //            ActivityData activityData;
                //            if (config.NextActivityDictionary.TryGetValue(pair.Key, out activityData))
                //            {
                //                this.nextListBox.Items.Add(
                //                    new ListItem(
                //                        ActivityManger.GetListText(
                //                            pair.Key
                //                            , pair.Value
                //                            , activityData.Duration
                //                            , activityData.Delay
                //                            , activityData.Forever
                //                            , activityData.Param
                //                            , this.nextSelectCalendar.SelectedDate
                //                        )
                //                        , pair.Key.ToString()
                //                    )
                //                );
                //            }
                //        }
                //    }
                //}
            }


            foreach (var dictionary in activitySet)
            {
                foreach (var pair in dictionary)
                {
                    if (string.IsNullOrEmpty(pair.Value.gmt_no_use)
                        && (config == null || !config.NextActivityDictionary.ContainsKey(pair.Key))
                        )
                    {
                        this.allListBox.Items.Add(
                            new ListItem(
                                ActivityManger.GetListText(pair.Key
                                    , pair.Value
                                    , 0
                                    , 0
                                    , 0
                                    , 0
                                    , new DateTime()
                                    )
                                , pair.Key.ToString()
                                )
                            );
                    }
                }
            }

            updateTimeButton.Enabled = (nextListBox.Items.Count > 0);
        }

        protected void ResetThreeYuanbaoButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(393);
                return;
            }

            string account = Session["user"] as string;

            this.errorLabel.Text = "";
            foreach (var item in this.selectListBox.Items)
            {
                gmt.Server server = gmt.Server.GetServer(item.ToString());
                if (server != null)
                {
                    AGmPage.ExecuteGmCommand
                    (
                        account,
                        server,
                        "0",
                        string.Format("RDATE()"),
                        "",
                        true,
                        text => { this.errorLabel.Text += text; }
                    );
                }
            }
        }

        protected void ResetVipGiftBuyButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(393);
                return;
            }

            string account = Session["user"] as string;

            this.errorLabel.Text = "";
            foreach (var item in this.selectListBox.Items)
            {
                gmt.Server server = gmt.Server.GetServer(item.ToString());
                if (server != null)
                {
                    AGmPage.ExecuteGmCommand
                    (
                        account,
                        server,
                        "0",
                        string.Format("YDATE()"),
                        "",
                        true,
                        text => { this.errorLabel.Text += text; }
                    );
                }
            }
        }

        protected void Button_add_activity_Click(object sender, EventArgs e)
        {

            //if (this.addListBox.SelectedIndex < 0) { return; }

            //if (null != m_currentList && 0 != m_currentList.Count)
            //{
            //    foreach (var data in m_currentList)
            //    {
            //        this.AddNextActivity(data.Id, data.Duration, data.Delay, data.Forever, data.Param);
            //    }
            //}

            //Server server = Session["Server"] as gmt.Server;
            //List<Server> listServer = new List<Server>();
            //listServer.Add(server);

            //if (this.addListBox.Items.Count == 0)
            //{
            //    this.errorLabel.Text = "未添加新的活动.";
            //}
            //else
            //{
            //    this.errorLabel.Text = ActivityManger.NtfTable(Activity.m_editActivityDictionary, listServer);
            //    System.Threading.Thread.Sleep(1000);
            //    UpdateList();
            //}
        }

        protected void Button_refresh_list_Click(object sender, EventArgs e)
        {
            UpdateList();
        }

        protected void CleanNext_Click(object sender, EventArgs e)
        {
            //Server server = Session["Server"] as gmt.Server;
            //List<Server> otherserver = new List<Server>();
            //otherserver.Add(server);
            //m_editActivityDictionary.Clear();
            //this.errorLabel.Text = ActivityManger.NtfTable(Activity.m_editActivityDictionary, otherserver, true);
            //System.Threading.Thread.Sleep(1000);
            //UpdateList();
        }

        /// <summary>
        /// 隐藏按钮点击响应
        /// </summary>
        protected void hideButton_Click(object sender, EventArgs e)
        {
            this.nextSelectCalendar.Visible = true;
            this.updateTimeButton.Visible = true;
            this.updateTimeButton.Enabled = false;
            this.closeTimeButton.Visible = true;
        }

        protected void refresh_Click(object sender, EventArgs e)
        {
            UpdateList();
            this.applyButton.Enabled = false;
        }

        protected void closeTimeButton_Click(object sender, EventArgs e)
        {
            this.nextSelectCalendar.Visible = false;
            this.updateTimeButton.Visible = false;
            this.updateTimeButton.Enabled = false;
            this.closeTimeButton.Visible = false;
        }

        protected void applyButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(414);
                return;
            }

            if (this.selectListBox.Items.Count == 1
                && m_curActivityDictionary.Count != 0
                && this.curDate.Text == TableManager.GetGMTText(413))
            {
                this.errorLabel.Text = TableManager.GetGMTText(415);
                return;
            }

            if (m_nextActivityDictionary.Count != 0
                && this.nextDateTextBox.Text == TableManager.GetGMTText(413))
            {
                this.errorLabel.Text = TableManager.GetGMTText(416);
                return;
            }

            List<Server> otherserver = new List<Server>();
            foreach (var item in this.selectListBox.Items)
            {
                gmt.Server server = gmt.Server.GetServer(item.ToString());
                otherserver.Add(server);
            }

            if (this.selectListBox.Items.Count == 1)
            {
                this.errorLabel.Text += ActivityManger.NtfTable(Activity.m_curActivityDictionary, otherserver, false);
            }
            this.errorLabel.Text += ActivityManger.NtfTable(Activity.m_nextActivityDictionary, otherserver, true);

            string account = Session["user"] as string;
            foreach (var server in otherserver)
            {
                if (server != null)
                {
                    AGmPage.ExecuteGmCommand
                    (
                        account,
                        server,
                        "0",
                        "RL(1)",
                        "",
                        true,
                        text => { this.errorLabel.Text += text; }
                    );

                    // 其中包括限时神将
                    if (m_nextActivityDictionary.ContainsKey(9))
                    {
                        int nHeroIdx = m_nextActivityDictionary[9].Param;
                        AGmPage.ExecuteGmCommand
                        (
                            account,
                            server,
                            "0",
                            string.Format("HERO({0})", nHeroIdx),
                            "",
                            true,
                            text => { this.errorLabel.Text = text; }
                        );
                    }

                    //砸金蛋
                    if (m_nextActivityDictionary.ContainsKey(20))
                    {
                        int nEggIdx = m_nextActivityDictionary[20].Param;
                        AGmPage.ExecuteGmCommand
                        (
                            account,
                            server,
                            "0",
                            string.Format("EGG({0})", nEggIdx),
                            "",
                            true,
                            text => { this.errorLabel.Text = text; }
                        );
                    }

                    //充值排行榜
                    if (m_nextActivityDictionary.ContainsKey(18))
                    {
                        int nPayIdx = m_nextActivityDictionary[18].Param == 0 ? 15000 : m_nextActivityDictionary[18].Param;
                        AGmPage.ExecuteGmCommand
                        (
                            account,
                            server,
                            "0",
                            string.Format("CZR({0})", nPayIdx),
                            "",
                            true,
                            text => { this.errorLabel.Text = text; }
                        );
                    }

                    if (nextSelectCalendar.SelectedDate > m_currentDate)
                    {
                        AGmPage.ExecuteGmCommand
                        (
                            account,
                            server,
                            "0",
                            string.Format("SDATE({0},{1},{2})",
                                          this.nextSelectCalendar.SelectedDate.Year,
                                          this.nextSelectCalendar.SelectedDate.Month,
                                          this.nextSelectCalendar.SelectedDate.Day),
                            "",
                            true,
                            text => { this.errorLabel.Text = text; }
                        );
                    }
                }
            }

            this.errorLabel.Text += (" " + TableManager.GetGMTText(417) + "...");
            System.Threading.Thread.Sleep(1000);
            UpdateList();
            this.applyButton.Enabled = false;
        }

        protected void delCurActivityButton_Click(object sender, EventArgs e)
        {
            if (this.currentListBox.SelectedIndex < 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(407);
                return;
            }

            int nId = int.Parse(this.currentListBox.SelectedValue);
            if (m_curActivityDictionary.ContainsKey(nId))
            {
                m_curActivityDictionary.Remove(nId);
            }

            this.currentListBox.Items.RemoveAt(currentListBox.SelectedIndex);
            this.currentListBox.SelectedIndex = -1;
            this.applyButton.Enabled = true;
        }

        protected void cleanCurActivityButton_Click(object sender, EventArgs e)
        {
            currentListBox.Items.Clear();
            m_curActivityDictionary.Clear();
            this.currentListBox.SelectedIndex = -1;
            this.applyButton.Enabled = true;
        }

        protected void cleanPlayerDataButton_Click(object sender, EventArgs e)
        {
            this.ExecuteGmCommand
            (
                "0",
                string.Format("SDATE({0},{1},{2})",
                m_currentDate.Year,
                m_currentDate.Month,
                m_currentDate.Day),
                "",
                true,
                text => { this.errorLabel.Text = text; }
            );
        }

        protected void delNextActivityButton_Click(object sender, EventArgs e)
        {
            if (this.nextListBox.SelectedIndex < 0)
            {
                this.errorLabel.Text = TableManager.GetGMTText(407);
                return;
            }

            int nId = int.Parse(this.nextListBox.SelectedValue);
            if (m_nextActivityDictionary.ContainsKey(nId))
            {
                m_nextActivityDictionary.Remove(nId);
            }

            this.nextListBox.Items.RemoveAt(nextListBox.SelectedIndex);
            nextListBox.SelectedIndex = -1;
            this.applyButton.Enabled = true;
        }

        protected void cleanNextActivityButton_Click(object sender, EventArgs e)
        {
            nextListBox.Items.Clear();
            m_nextActivityDictionary.Clear();
            this.nextListBox.SelectedIndex = -1;
            this.applyButton.Enabled = true;
        }

        // 开服活动
        protected void addOpenActivityButton_Click(object sender, EventArgs e)
        {
            //限时神将
            AddNextActivity(9, 3, 0, 0, 4);
            //限时神将排行榜
            AddNextActivity(10, 4, 0, 0, 0);
            //占星台折扣
            AddNextActivity(16, 2, 3, 0, 60);
            //新服3-7天
            AddNextActivity(1808, 5, 0, 0, 0);
            //新服3-7天单笔充值活动
            AddNextActivity(1813, 5, 0, 0, 0);
            //新服3-7天累计消费活动
            AddNextActivity(1814, 5, 0, 0, 0);
            this.applyButton.Enabled = true;
        }

        /// <summary>
        /// 活动时间日历选择响应
        /// </summary>
        //protected void nextSelectCalendar_SelectionChanged1(object sender, EventArgs e)
        //{
        //    DateTime expiryDate = this.nextSelectCalendar.SelectedDate;
        //    ViewState["expiryDate"] = expiryDate;
        //    this.expiryDateTextBox.Text = expiryDate.ToShortDateString();
        //    this.nextSelectCalendar.Visible = false;
        //}
    }

    /// <summary>
    /// 活动管理器
    /// </summary>
    class ActivityManger
    {
        //记录当前选择的服务器
        public static gmt.Server m_curServer;

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            ActivityManger.Load();

            //Network.RegisterMessageProcess(MessageType.ActivityCheck, ActivityManger.ProcessCheck);
            //Network.RegisterMessageProcess(MessageType.ActivityUpdate, ActivityManger.ProcessUpdate);
        }

        /// <summary>
        /// 检查
        /// </summary>
        public static void Check()
        {
            BinaryWriter writer = new BinaryWriter(new MemoryStream());

            writer.Write((int)0);
            writer.Write((ushort)MessageType.ActivityCheck);

            Network.Send(writer);
        }

        /// <summary>
        /// 消息处理：检查
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="reader">读取器</param>
        private static void ProcessCheck(ushort type, BinaryReader reader)
        {
            if (!reader.ReadBoolean())
            {
                //20161217
                List<gmt.Server> server = new List<gmt.Server>();
                server.Add(ActivityManger.m_curServer);

                ActivityManger.UpdateService(server);
            }
        }

        /// <summary>
        /// 消息处理：更新
        /// </summary>
        /// <param name="type">消息类型</param>
        /// <param name="reader">读取器</param>
        private static void ProcessUpdate(ushort type, BinaryReader reader)
        {
            ushort count = reader.ReadUInt16();
            List<string[]> list = new List<string[]>();

            for (int i = 0; i < count; ++i)
            {
                list.Add(new string[] { reader.ReadString(), reader.ReadString() });
            }

            lock (ActivityManger.ActivityServerDictionary)
            {
                List<Server> removeServerList = new List<Server>();
                foreach (var pair in ActivityManger.ActivityServerDictionary)
                {
                    bool exist = false;

                    foreach (var server in list)
                    {
                        if (pair.Key.GmAddress == server[0] && pair.Key.Id == server[1])
                        {
                            exist = true;
                            break;
                        }
                    }

                    if (!exist) { removeServerList.Add(pair.Key); }
                }

                if (removeServerList.Count > 0)
                {
                    foreach (var server in removeServerList)
                    {
                        ActivityManger.ActivityServerDictionary.Remove(server);
                    }

                    ActivityManger.Save();
                }
            }
        }

        /// <summary>
        /// 更新服务
        /// </summary>
        /// <returns>是否成功</returns>
        public static bool UpdateService(List<gmt.Server> server)
        {
            //BinaryWriter writer = new BinaryWriter(new MemoryStream());

            //writer.Write((int)0);
            //writer.Write((ushort)MessageType.ActivityUpdate);

            //lock (ActivityManger.ActivityServerDictionary)
            //{
            //    writer.Write((ushort)ActivityManger.ActivityServerDictionary.Count);

            //    foreach (var pair in ActivityManger.ActivityServerDictionary)
            //    {
            //        writer.Write(pair.Key.GmAddress);
            //        writer.Write(pair.Key.Id);
            //        writer.Write((ushort)pair.Value.NextActivityDate.Year);
            //        writer.Write((byte)pair.Value.NextActivityDate.Month);
            //        writer.Write((byte)pair.Value.NextActivityDate.Day);

            //        MemoryStream stream = new MemoryStream();
            //        List<Server> newserver = new List<Server>();
            //        newserver.Add(pair.Key);
            //        ProtoSerializer.Instance.Serialize(stream, ActivityManger.GetNtf(pair.Value.NextActivityDictionary, newserver));

            //        writer.Write((ushort)stream.Length);
            //        writer.Write(stream.GetBuffer(), 0, (int)stream.Length);
            //    }
            //}

            //return Network.Send(writer);
            return true;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                lock (ActivityManger.ActivityServerDictionary)
                {
                    writer.Write((ushort)ActivityManger.ActivityServerDictionary.Count);

                    foreach (var pair in ActivityManger.ActivityServerDictionary)
                    {
                        writer.Write(pair.Key.Name);

                        writer.Write(pair.Value.CurrentActivityDate.Year * 10000 + pair.Value.CurrentActivityDate.Month * 100 + pair.Value.CurrentActivityDate.Day);
                        writer.Write((byte)pair.Value.CurrentActivityDictionary.Count);
                        foreach (var dataPair in pair.Value.CurrentActivityDictionary)
                        {
                            writer.Write(dataPair.Value.Id);
                            writer.Write(dataPair.Value.Duration);
                            writer.Write(dataPair.Value.Delay);
                            writer.Write(dataPair.Value.Forever);
                            writer.Write(dataPair.Value.Param);
                        }

                        writer.Write(pair.Value.NextActivityDate.Year * 10000 + pair.Value.NextActivityDate.Month * 100 + pair.Value.NextActivityDate.Day);
                        writer.Write((byte)pair.Value.NextActivityDictionary.Count);
                        foreach (var dataPair in pair.Value.NextActivityDictionary)
                        {
                            writer.Write(dataPair.Value.Id);
                            writer.Write(dataPair.Value.Duration);
                            writer.Write(dataPair.Value.Delay);
                            writer.Write(dataPair.Value.Forever);
                            writer.Write(dataPair.Value.Param);
                        }
                    }
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Activity.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        private static void Load()
        {
            //string path = HttpRuntime.AppDomainAppPath + "protodatas/ActivityConfig.protodata.bytes";
            //if (File.Exists(path))
            //{
            //    using (FileStream stream = File.OpenRead(path))
            //    {
            //        byte[] buffer = new byte[stream.Length];
            //        stream.Read(buffer, 0, buffer.Length);

            //        ProtoData<mw.ActivityConfig> activityConfigSet = new ProtoData<mw.ActivityConfig>(buffer);
            //        for (int i = 0; i < activityConfigSet.Count; ++i)
            //        {
            //            mw.ActivityConfig activityConfig = activityConfigSet[i];
            //            ActivityManger.ActivityDataDictionary.Add(activityConfig.id, activityConfig);
            //        }
            //    }
            //}

            List<mw.ActivityConfig> activityList = TableManager.Load<mw.ActivityConfig>("protodatas/ActivityConfig.protodata.bytes");
            if (null != activityList)
            {
                ActivityManger.ActivityDataDictionary = activityList.ToDictionary(key => key.id, value => value);
            }

            string path = HttpRuntime.AppDomainAppPath + "configs/Activity.bytes";
            if (File.Exists(path))
            {
                using (FileStream stream = File.OpenRead(path))
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

                        for (int i = 0; i < count - 1; ++i)
                        {
                            Server server = Server.GetServer(reader.ReadString());
                            ActivityServerConfig config = new ActivityServerConfig();

                            if (server != null) { ActivityManger.ActivityServerDictionary.Add(server, config); }

                            int currentDate = reader.ReadInt32();
                            byte currentCount = reader.ReadByte();

                            config.CurrentActivityDate = new DateTime(currentDate / 10000, currentDate % 10000 / 100, currentDate % 100);
                            for (int j = 0; j < currentCount; ++j)
                            {
                                ActivityData data = new ActivityData
                                (
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32()
                                );
                                config.CurrentActivityDictionary.Add(data.Id, data);
                            }

                            int nextDate = reader.ReadInt32();
                            byte nextCount = reader.ReadByte();

                            config.NextActivityDate = new DateTime(nextDate / 10000, nextDate % 10000 / 100, nextDate % 100);
                            for (int j = 0; j < nextCount; ++j)
                            {
                                ActivityData data = new ActivityData
                                (
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32(),
                                    reader.ReadInt32()
                                );
                                config.NextActivityDictionary.Add(data.Id, data);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取活动通知
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        //public static mw.AUTH_ACTIVITY_Ntf GetNtf(Dictionary<int, ActivityData> activityDictionary)
        //{
        //    mw.AUTH_ACTIVITY_Ntf ntf = new mw.AUTH_ACTIVITY_Ntf();

        //    Dictionary<int, mw.ActivityConfig>[] activitySet = { ActivityManger.ActivityDataDictionary, OtherActivityManager.ActivityDictionary };
        //    foreach (var dictionary in activitySet)
        //    {
        //        foreach (var pair in dictionary)
        //        {
        //            mw.AUTH_ActivityData	data = new mw.AUTH_ActivityData();
        //            ActivityData			activityData;
        //            if (activityDictionary.TryGetValue(pair.Key, out activityData))
        //            {
        //                data.lastDay = activityData.Duration;
        //                data.startDay = activityData.Delay;
        //            }
        //            else
        //            {
        //                data.lastDay = pair.Value.last_days;
        //                data.startDay = pair.Value.start_days;
        //            }
        //            data.activityID = pair.Value.id;
        //            data.signData = pair.Value.sign;
        //            data.eHour = pair.Value.end_hour;
        //            data.eMin = pair.Value.end_min;
        //            data.sHour = pair.Value.start_hour;
        //            data.sMin = pair.Value.start_min;
        //            data.param = pair.Value.param;

        //            if (pair.Value.every == 1)
        //            {
        //                data.dayFlag = 0xFF;
        //            }
        //            else
        //            {
        //                data.dayFlag = 0;
        //                if (pair.Value.sun == 1)
        //                {
        //                    data.dayFlag |= 1;
        //                }

        //                if (pair.Value.mon == 1)
        //                {
        //                    data.dayFlag |= 1 << 1;
        //                }

        //                if (pair.Value.tues == 1)
        //                {
        //                    data.dayFlag |= 1 << 2;
        //                }

        //                if (pair.Value.wed == 1)
        //                {
        //                    data.dayFlag |= 1 << 3;
        //                }

        //                if (pair.Value.thur == 1)
        //                {
        //                    data.dayFlag |= 1 << 4;
        //                }

        //                if (pair.Value.fri == 1)
        //                {
        //                    data.dayFlag |= 1 << 5;
        //                }

        //                if (pair.Value.sat == 1)
        //                {
        //                    data.dayFlag |= 1 << 6;
        //                }
        //            }

        //            ntf.activityInfos.Add(data);
        //        }
        //    }

        //    return ntf;
        //}

        public static string NtfTable(Dictionary<int, ActivityData> activityDictionary, List<gmt.Server> server, bool isNext = false)
        {
            List<mw.ActivityConfig> Updateactivitytable = new List<mw.ActivityConfig>();

            Dictionary<int, mw.ActivityConfig>[] activitySet = { ActivityManger.ActivityDataDictionary, GMTActivityMananger.GetTableActivity() };
            foreach (var dictionary in activitySet)
            {
                foreach (var pair in dictionary)
                {
                    mw.ActivityConfig data = new mw.ActivityConfig();
                    ActivityData activityData;
                    if (activityDictionary.TryGetValue(pair.Key, out activityData))
                    {
                        data = pair.Value;
                        data.last_days = activityData.Duration;
                        data.start_days = activityData.Delay;
                        data.eternal = activityData.Forever;
                        data.param = activityData.Param;
                        Updateactivitytable.Add(data);
                    }
                }
            }

            int eType = -1;
            if (isNext) eType = (int)mw.EGMTSettintType.E_GMT_SETTINT_NEXT_ACTIVITY;

            string output = "";
            //只更新活动
            if (server != null)
            {
                for (int i = 0; i < server.Count; i++)
                {
                    output += TableManager.Send<mw.ActivityConfig>(Updateactivitytable, eType, server[i]);
                }
            }
            else
            {
                output += TableManager.Send<mw.ActivityConfig>(Updateactivitytable, eType);
            }

            return output;
        }

        /// <summary>
        /// 获取配置文本
        /// </summary>
        /// <param name="id">活动编号</param>
        /// <param name="config">配置</param>
        /// <param name="duration">持续天数</param>
        /// <param name="delay">推迟天数</param>
        /// <returns>文本</returns>
        public static string GetListText(int id, mw.ActivityConfig config, int duration, int delay, int forever, int param, DateTime startTime)
        {
            if (config != null)
            {
                string text = "ID:" + config.id;

                text += " - ";
                text += TableManager.GetGMTText(418) + ":" + duration;

                text += " - ";
                text += TableManager.GetGMTText(419) + ":" + delay;

                if (0 != startTime.Ticks)
                {
                    text += " - ";
                    text += TableManager.GetGMTText(420) + ":" + (startTime + TimeSpan.FromDays(delay)).ToShortDateString();
                }

                text += " - ";
                text += (forever == 1) ? "" : TableManager.GetGMTText(421) + TableManager.GetGMTText(136);

                text += " - ";
                text += TableManager.GetGMTText(135) + ":" + param;
                text += " - ";

                int outname = -1;
                if (int.TryParse(config.name, out outname))
                {
                    text += TextManager.GetText(outname);
                }
                else
                {
                    text += config.name;
                }

                return text;
            }
            else
            {
                return TableManager.GetGMTText(422) + ":" + id;
            }
        }

        /// <summary>
        /// 活动服务器字典
        /// </summary>
        public static Dictionary<Server, ActivityServerConfig> ActivityServerDictionary = new Dictionary<Server, ActivityServerConfig>();

        /// <summary>
        /// 活动数据列表
        /// </summary>
        public static Dictionary<int, mw.ActivityConfig> ActivityDataDictionary = new Dictionary<int, mw.ActivityConfig>();
    }

    /// <summary>
    /// 活动服务器配置
    /// </summary>
    class ActivityServerConfig
    {
        /// <summary>
        /// 当前活动日期
        /// </summary>
        public DateTime CurrentActivityDate;

        /// <summary>
        /// 当前活动列表
        /// </summary>
        public Dictionary<int, ActivityData> CurrentActivityDictionary = new Dictionary<int, ActivityData>();

        /// <summary>
        /// 下次活动日期
        /// </summary>
        public DateTime NextActivityDate = DateTime.Today + new TimeSpan(7, 0, 0, 0);

        /// <summary>
        /// 下次活动列表
        /// </summary>
        public Dictionary<int, ActivityData> NextActivityDictionary = new Dictionary<int, ActivityData>();
    }

    /// <summary>
    /// 活动数据
    /// </summary>
    public class ActivityData
    {
        /// <summary>
        /// 编号
        /// </summary>
        public int Id;

        /// <summary>
        /// 持续天数
        /// </summary>
        public int Duration;

        /// <summary>
        /// 推迟天数
        /// </summary>
        public int Delay;

        /// <summary>
        /// 是否永久
        /// </summary>
        public int Forever;

        public int Param;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="duration">持续天数</param>
        /// <param name="delay">推迟天数</param>
        public ActivityData(int id, int duration, int delay, int forever, int param)
        {
            this.Id = id;
            this.Duration = duration;
            this.Delay = delay;
            this.Forever = forever;
            this.Param = param;
        }
    }
}