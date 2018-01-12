using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace gmt
{
    /// <summary>
    /// FTP编辑
    /// </summary>
    public partial class FTPEdit : AGmPage
    {
        private int m_curIdx = 0;
        /// <summary>
        /// 页面载入响应
        /// </summary>
        public FTPEdit()
            : base(PrivilegeType.FTPEdit)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                List<int> ftpList = new List<int>();
                for (int i = 0; i < FTPManager.Count; ++i)
                {
                    FTP ftp = FTPManager.GetFTP(i);
                    if (null == ftp)
                        continue;

                    addFTP(i, ftp);
                    ftpList.Add(i);
                }
                ViewState["ftpList"] = ftpList;
                m_curIdx = ftpList.Count;
            }
            else if (null != ViewState["ftpList"])
            {
                List<int> ftpList = (List<int>)ViewState["ftpList"];
                for (int i = 0; i < ftpList.Count; ++i)
                {
                    addFTP(ftpList[i], FTPManager.GetFTP(ftpList[i]));
                }
                int nIncIdx = 0;
                if (ftpList.Count != 0)
                {
                    nIncIdx = ftpList.Max();
                }
                m_curIdx = nIncIdx + 1;
            }
        }

        /// <summary>
        /// 保存按钮点击响应
        /// </summary>
        protected void saveButton_Click(object sender, EventArgs e)
        {
            int i = 0;
            FTPManager.Clear();
            foreach (TableRow Row in ftpTable.Rows)
            {
                FTP ftp = new FTP("", "", "");
                foreach (Control ctrl in Row.Controls)
                {
                    foreach (Control subCtrl in ctrl.Controls)
                    {
                        if (subCtrl.GetType().ToString() == "System.Web.UI.WebControls.TextBox")
                        {
                            string name = ((TextBox)subCtrl).ID.Split('_')[0];
                            if (name == "tbFTP")
                            {
                                string textSite = ((TextBox)subCtrl).Text;
                                if (textSite.Length != 0
                                    && textSite[textSite.Length - 1] != '/')
                                {
                                    textSite += '/';
                                }

                                ftp.ftpSite = textSite;
                            }
                            else if (name == "tbUser")
                            {
                                ftp.ftpUser = ((TextBox)subCtrl).Text;
                            }
                            else if (name == "tbPWD")
                            {
                                ftp.ftpPassword = ((TextBox)subCtrl).Text;
                                ((TextBox)subCtrl).Attributes.Add("value", ftp.ftpPassword);
                            }
                        }
                    }
                }

                FTPManager.AddFTP(i, ftp);
                ++i;
            }

            FTPManager.Save();
        }

        protected void addButton_Click(object sender, EventArgs e)
        {
            addFTP(m_curIdx, null);
            List<int> ftpList = (List<int>)ViewState["ftpList"];
            ftpList.Add(m_curIdx);
            ViewState["ftpList"] = ftpList;
            m_curIdx++;
        }

        protected void removeButton_Click(object sender, EventArgs e)
        {
            if (null == ViewState["ftpList"])
                return;

            int ID = int.Parse(((Button)sender).ID.Split('_')[1]);

            List<int> ftpList = (List<int>)ViewState["ftpList"];
            int i = 0;
            for (; i < ftpList.Count; ++i)
            {
                if (ID == ftpList[i])
                {
                    RemoveFTP(i);
                    break;
                }
            }
            ftpList.RemoveAt(i);
        }

        private void addFTP(int idx, FTP ftp)
        {
            TableRow tRow = new TableRow();

            TableCell tCell1 = new TableCell();
            Label cellName1 = new Label();
            cellName1.Text = string.Format("&nbsp;FTP&nbsp;Site:&nbsp;");
            tCell1.Controls.Add(cellName1);
            TextBox textBoxSite = new TextBox();
            textBoxSite.Width = 350;
            textBoxSite.ID = "tbFTP_" + idx;

            if (null != ftp)
            {
                textBoxSite.Text = ftp.ftpSite;
            }

            tCell1.Controls.Add(textBoxSite);
            tRow.Cells.Add(tCell1);

            TableCell tCell2 = new TableCell();
            Label cellName2 = new Label();
            cellName2.Text = "&nbsp;" + TableManager.GetGMTText(230) +":&nbsp;";
            tCell2.Controls.Add(cellName2);

            TextBox textBoxUser = new TextBox();
            textBoxUser.ID = "tbUser_" + idx;

            if (null != ftp)
            {
                textBoxUser.Text = ftp.ftpUser;
            }

            tCell2.Controls.Add(textBoxUser);
            tRow.Cells.Add(tCell2);

            TableCell tCell3 = new TableCell();
            Label cellName3 = new Label();
            cellName3.Text = "&nbsp;" + TableManager.GetGMTText(231) +":&nbsp;";
            tCell3.Controls.Add(cellName3);
            TextBox textBoxPwd = new TextBox();
            textBoxPwd.TextMode = TextBoxMode.Password;
            textBoxPwd.ID = "tbPWD_" + idx;

            if (null != ftp)
            {
                textBoxPwd.Attributes.Add("value", ftp.ftpPassword);
            }

            tCell3.Controls.Add(textBoxPwd);
            tRow.Cells.Add(tCell3);

            TableCell tCell4 = new TableCell();
            Button button = new Button();
            button.Text = "-";
            button.Click += new EventHandler(removeButton_Click);
            button.ID = "tbRemove_" + idx;
            tCell4.Controls.Add(button);
            tRow.Cells.Add(tCell4);

            ftpTable.Rows.Add(tRow);
        }

        private void RemoveFTP(int idx)
        {
            ftpTable.Rows.RemoveAt(idx);
        }
    }
}