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
    /// 登录界面
    /// </summary>
    public partial class Login : System.Web.UI.Page
    {
        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {

                string account = Session["user"] as string;

                if (!string.IsNullOrEmpty(account))
                {
                    Response.Redirect("./GmNormal.aspx", false);
                    return;
                }
            }

            ////模拟生成一个验证码
            //if(this.checkLabel.Text==""||this.checkLabel.Text==null)
            //{
            //   Random random = new Random();
            //   int rand = random.Next(100000, 999999);
            //   this.checkLabel.Text = rand.ToString();
            //}
        }

        /// <summary>
        /// 登录按钮点击响应
        /// </summary>
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            //Server server = gmt.Server.GetServerAt(gmt.Server.Count - 1);
            //if (server == null) { return; }

            if (string.IsNullOrEmpty(this.userTextBox.Text) ||
                string.IsNullOrEmpty(this.passwordTextBox.Text))
            {
                this.outputLabel.Text = TableManager.GetGMTText(685);
                //Random random = new Random();
                //int rand = random.Next(100000, 999999);
                //this.checkLabel.Text = rand.ToString();
                return;
            }

            string account = this.userTextBox.Text.Replace(@"\", @"\\").Replace(@"'", @"\'").Replace("\"", "\\\"");
            string password = this.passwordTextBox.Text.Replace(@"\", @"\\").Replace(@"'", @"\'").Replace("\"", "\\\"");
            //string  checkword   = this.checkTextBox.Text;
            bool hasUser = false;

            UserInfo user = null;

            if (UserManager.UserTable.TryGetValue(account, out user) && user.password == password)
            {
                Session["user"] = account;
                if ((user.privilege & PrivilegeType.GmModify) == PrivilegeType.GmModify)
                {
                    Response.Redirect("./GmModify.aspx", false);
                }
                else
                {
                    foreach (PrivilegeType privilege in Enum.GetValues(typeof(PrivilegeType)))
                    {
                        if (privilege != PrivilegeType.Normal && privilege != PrivilegeType.Modify && privilege != PrivilegeType.Download)
                        {
                            if ((user.privilege & privilege) == privilege)
                            {
                                Response.Redirect(string.Format(@"./{0}.aspx", privilege.ToString()), false);
                            }
                        }
                    }
                }
            }
            else
            {
                this.outputLabel.Text = TableManager.GetGMTText(686);
            }

        }

    }
}