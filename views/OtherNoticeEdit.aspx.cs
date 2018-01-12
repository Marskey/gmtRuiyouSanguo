using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    public partial class OtherNoticeEdit : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public OtherNoticeEdit()
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
                
                OnLoad();
            }
        }



        //保存公告
        protected void saveButton_Click(object sender, EventArgs e)
        {
            Save();
        }


        //上传公告
        protected void uploadButton_Click(object sender, EventArgs e)
        {
            //foreach (var data in ServerListConfig.DataList)
            //{
            //string versiotn = GmModify.Version;
            //string NoticePath = versiotn + "/updateex/" + data.Name + "/ClientProto/";
            //string NoticePath = versiotn + "/updateex/";
            string NoticePath = "/update/Notice/";
            string content = this.Noticetxt.Text;
            byte[] buffer = null;
            buffer = System.Text.UnicodeEncoding.UTF8.GetBytes(content);
            if (!FTPManager.Upload(NoticePath + "NoticeList.txt", buffer))
            {
                this.OutputLable.Text = TableManager.GetGMTText(821);
                return;
            }

            this.OutputLable.Text = TableManager.GetGMTText(822);

            //}

        }


        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            string noticestring = this.Noticetxt.Text;
            string filePath = HttpRuntime.AppDomainAppPath + "NoticeList.txt";
            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            fs.SetLength(0);
            sw.Write(noticestring);
            sw.Close();
            this.OutputLable.Text = TableManager.GetGMTText(823);
        }

        /// <summary>
        /// 载入
        /// </summary>
        public void OnLoad()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "NoticeList.txt";
            //存在，则显示之前的这个文件
            if (File.Exists(configBinaryFile))
            {
                string contents = System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath + "NoticeList.txt");
                this.Noticetxt.Text = contents;
            }

        }
    }
}