using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 公告编辑
    /// </summary>
    public partial class NoticeEdit : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public NoticeEdit()
            : base(PrivilegeType.Modify)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                this.titleTipLabel.Text = TableManager.GMTDescTable[186].desc;
                this.returnTipLabel.Text = TableManager.GMTDescTable[187].desc;
                this.noticeTitleTipLabel.Text = TableManager.GMTDescTable[188].desc;
                this.addButton.Text = TableManager.GMTDescTable[174].desc;
                this.modifyButton.Text = TableManager.GMTDescTable[175].desc;
                this.deleteButton.Text = TableManager.GMTDescTable[176].desc;
                this.sendButton.Text = TableManager.GMTDescTable[177].desc;
                this.jieriTipLabel.Text = TableManager.GMTDescTable[815].desc;
                this.cuxiaoTipLabel.Text = TableManager.GMTDescTable[816].desc;
                this.huodongTipLabel.Text = TableManager.GMTDescTable[817].desc;
                this.gonggaoTipLabel.Text = TableManager.GMTDescTable[818].desc;
                this.contentTipLabel.Text = TableManager.GMTDescTable[189].desc;
                this.bindIdTipLabel.Text = TableManager.GMTDescTable[190].desc;
                this.sendFileButton.Text = TableManager.GMTDescTable[191].desc;
                this.sendButton0.Text = TableManager.GMTDescTable[192].desc;
                this.downloadButton.Text = TableManager.GMTDescTable[193].desc;
                this.downloadNoticeButton.Text = TableManager.GMTDescTable[194].desc;
                this.upLoadTipLabel.Text = TableManager.GMTDescTable[195].desc;
                this.uploadButton.Text = TableManager.GMTDescTable[196].desc;

                this.versionTextBox.Text = GmNormal.Version;

                if (NoticeEditData.NoticeList != null)
                {
                    foreach (var config in NoticeEditData.NoticeList)
                    {
                        if (config.id < NoticeEditData.CommonIdStart)
                        {
                            this.noticeListBox.Items.Add(new ListItem(this.GetNoticeText(config), config.title_id.ToString()));
                        }
                    }
                }
            }

            this.errorLabel.Text = "";
        }

        /// <summary>
        /// 添加按钮点击响应
        /// </summary>
        protected void addButton_Click(object sender, EventArgs e)
        {
            mw.SysNtfConfig config = new mw.SysNtfConfig();

            config.type = mw.Enums.SysNtfType.SYS_NTF_TYPE_LOGIN;
            config.id = NoticeEditData.IdIncrease;
            config.title_id = NoticeEditData.IdIncrease;
            config.details_id = NoticeEditData.IdIncrease + 10000;

            //文字(2016.3.21)
            config.details_str = this.contentTextBox.Text;
            config.title_str = this.titleTextBox.Text;

            this.UpdateNotice(config);

            mw.SysNtfConfig commonConfig = new mw.SysNtfConfig();
            commonConfig.type = mw.Enums.SysNtfType.SYS_NTF_TYPE_COMMON;
            commonConfig.id = config.id + NoticeEditData.CommonIdAdded;
            commonConfig.title_id = config.title_id + NoticeEditData.CommonIdAdded;
            commonConfig.details_id = config.details_id + NoticeEditData.CommonIdAdded;
            this.UpdateNotice(commonConfig);

            ++NoticeEditData.IdIncrease;

            NoticeEditData.NoticeList.Insert(this.noticeListBox.Items.Count, config);
            NoticeEditData.NoticeList.Add(commonConfig);
            NoticeEditData.Save();

            this.noticeListBox.Items.Add(new ListItem(this.GetNoticeText(config), config.title_id.ToString()));
        }

        /// <summary>
        /// 修改按钮点击响应
        /// </summary>
        protected void modifyButton_Click(object sender, EventArgs e)
        {
            if (this.noticeListBox.SelectedIndex < 0) { return; }

            mw.SysNtfConfig config = NoticeEditData.NoticeList[this.noticeListBox.SelectedIndex];

            this.UpdateNotice(config);

            foreach (var commonConfig in NoticeEditData.NoticeList)
            {
                if (commonConfig.id == config.id + NoticeEditData.CommonIdAdded)
                {
                    this.UpdateNotice(commonConfig);
                    break;
                }
            }

            NoticeEditData.Save();

            this.noticeListBox.SelectedItem.Text = this.GetNoticeText(config);
        }

        /// <summary>
        /// 删除按钮点击响应
        /// </summary>
        protected void deleteButton_Click(object sender, EventArgs e)
        {
            if (this.noticeListBox.SelectedIndex < 0) { return; }

            mw.SysNtfConfig config = NoticeEditData.NoticeList[this.noticeListBox.SelectedIndex];
            NoticeEditData.NoticeList.RemoveAt(this.noticeListBox.SelectedIndex);

            for (int i = 0; i < NoticeEditData.NoticeList.Count; ++i)
            {
                mw.SysNtfConfig commonConfig = NoticeEditData.NoticeList[i];
                if (commonConfig.id == config.id + NoticeEditData.CommonIdAdded)
                {
                    NoticeEditData.NoticeList.RemoveAt(i);
                    break;
                }
            }

            NoticeEditData.Save();

            this.noticeListBox.Items.RemoveAt(this.noticeListBox.SelectedIndex);
        }

        /// <summary>
        /// 发送按钮点击响应
        /// </summary>
        protected void sendButton_Click(object sender, EventArgs e)
        {
            this.errorLabel.Text = TableManager.Send(NoticeEditData.NoticeList);
        }

        /// <summary>
        /// 公告列表选中改变响应
        /// </summary>
        protected void noticeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.noticeListBox.SelectedIndex < 0) { return; }

            mw.SysNtfConfig config = NoticeEditData.NoticeList[this.noticeListBox.SelectedIndex];

            this.titleTextBox.Text = TextManager.GetText(config.title_id);
            this.contentTextBox.Text = TextManager.GetText(config.details_id);
            this.activityTextBox.Text = config.activity.ToString();
            this.icon1TextBox.Text = config.icon1;
            this.icon2TextBox.Text = config.icon2;
        }

        /// <summary>
        /// 直接发送所有表格按钮点击响应
        /// </summary>
        protected void sendFileButton_Click(object sender, EventArgs e)
        {
            if (ServerListConfig.DataList.Count == 0)
            {
                this.errorLabel.Text = TableManager.GMTDescTable[657].desc;
                return;
            }

            if (TableManager.SendTable(this.versionTextBox.Text))
            {
                this.errorLabel.Text = TableManager.GMTDescTable[658].desc;
            }
            else
            {
                this.errorLabel.Text = TableManager.GetLastError();
            }
        }

        /// <summary>
        /// 发送ZIP包按钮点击响应
        /// </summary>
        protected void sendButton0_Click(object sender, EventArgs e)
        {
            if (ServerListConfig.DataList.Count == 0)
            {
                this.errorLabel.Text = TableManager.GMTDescTable[657].desc;
                return;
            }

            FTPManager.Upload("updateex_" + ServerListConfig.DataList[0].Name + ".zip", CdnZip.PackTable(this.versionTextBox.Text));
        }

        /// <summary>
        /// 上传按钮点击响应
        /// </summary>
        protected void uploadButton_Click(object sender, EventArgs e)
        {
            if (this.noticeFileUpload.HasFile)
            {
                NoticeEditData.NoticeList = TableManager.Unserialize<mw.SysNtfConfig>(this.noticeFileUpload.FileBytes);

                NoticeEditData.DoLoad();
                this.noticeListBox.Items.Clear();
                foreach (var config in NoticeEditData.NoticeList)
                {
                    if (config.id < NoticeEditData.CommonIdStart)
                    {
                        this.noticeListBox.Items.Add(new ListItem(this.GetNoticeText(config), config.title_id.ToString()));
                    }
                }
                this.errorLabel.Text = TableManager.GMTDescTable[655].desc;

                TableManager.Save(NoticeEditData.NoticeList);
            }
            else
            {
                this.errorLabel.Text = TableManager.GMTDescTable[656].desc;
            }
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="config">公告配置</param>
        private void UpdateNotice(mw.SysNtfConfig config)
        {
            int activity;

            config.title_id = TextManager.CreateText();
            TextManager.SetText(config.title_id, this.titleTextBox.Text);
            config.details_id = TextManager.CreateText();
            TextManager.SetText(config.details_id, this.contentTextBox.Text);

            if (int.TryParse(this.activityTextBox.Text, out activity))
            {
                config.activity = activity;
            }
            else
            {
                config.activity = 0;
            }

            config.icon1 = this.icon1TextBox.Text;
            config.icon2 = this.icon2TextBox.Text;
        }

        /// <summary>
        /// 获取公告文本
        /// </summary>
        /// <param name="config">公告配置</param>
        /// <returns>公告文本</returns>
        private string GetNoticeText(mw.SysNtfConfig config)
        {
            string text = config.title_str + " " + config.activity;

            return text;
        }
    }

    /// <summary>
    /// 公告数据
    /// </summary>
    class NoticeEditData
    {
        /// <summary>
        /// 通常编号开始
        /// </summary>
        public const int CommonIdStart = 56000;

        /// <summary>
        /// 通常编号增加
        /// </summary>
        public const int CommonIdAdded = 4000;

        /// <summary>
        /// 载入
        /// </summary>
        public static void Load()
        {
            NoticeEditData.NoticeList = TableManager.Load<mw.SysNtfConfig>();

            NoticeEditData.DoLoad();
        }

        /// <summary>
        /// 载入
        /// </summary>
        public static void DoLoad()
        {
            if (NoticeEditData.NoticeList != null)
            {
                foreach (var config in NoticeEditData.NoticeList)
                {
                    if (config.id < NoticeEditData.CommonIdStart)
                    {
                        NoticeEditData.IdIncrease = Math.Max(NoticeEditData.IdIncrease, config.id + 1);
                    }
                }
            }
            else
            {
                NoticeEditData.NoticeList = new List<mw.SysNtfConfig>();
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            TableManager.Save(NoticeEditData.NoticeList);
        }

        /// <summary>
        /// 公告列表
        /// </summary>
        public static List<mw.SysNtfConfig> NoticeList;

        /// <summary>
        /// 编号增长
        /// </summary>
        public static int IdIncrease = 52000;
    }
}