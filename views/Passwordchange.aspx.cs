using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace gmt
{
    public partial class Passwordchange : AGmPage
    {
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                this.userTextBox.Text = Session["user"] as string;
            }
        }


        //修改密码
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.userTextBox.Text) ||
                string.IsNullOrEmpty(this.oldpassword.Text) || string.IsNullOrEmpty(this.newpassword.Text))
            {
                this.outputLabel.Text = TableManager.GetGMTText(827);
                return;
            }

            string account = this.userTextBox.Text.Replace(@"\", @"\\").Replace(@"'", @"\'").Replace("\"", "\\\"");
            string oldpassword = this.oldpassword.Text.Replace(@"\", @"\\").Replace(@"'", @"\'").Replace("\"", "\\\"");
            string newpassword = this.newpassword.Text.Replace(@"\", @"\\").Replace(@"'", @"\'").Replace("\"", "\\\"");

            UserInfo user;
            if (UserManager.UserTable.TryGetValue(account, out user) && user.password == oldpassword)
            {
                user.password = newpassword;
                string result = UserManager.ModifyUser(user.account, user, UserManager.UpdateType.Password);
                Dictionary<string, string> resultDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (resultDic["error"] == "Update_User_Success")
                    this.outputLabel.Text = TableManager.GetGMTText(829);
                else
                    this.outputLabel.Text = resultDic["error"];
            }
            else
            {
                this.outputLabel.Text = TableManager.GetGMTText(828);
            }
        }
    }
}