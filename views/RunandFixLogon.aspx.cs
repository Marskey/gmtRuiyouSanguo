using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
    public partial class RunandFixLogon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void logonButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.userTextBox.Text) || string.IsNullOrEmpty(this.passwordTextBox.Text))
            {
                this.outputLabel.Text = "用户名或密码不能为空";
            }
            else {

                this.outputLabel.Text = "用户名或密码错误";
               
            }
            if (this.userTextBox.Text == "admin" && this.passwordTextBox.Text == "datangwushuang")
            {
                this.outputLabel.Text = "验证成功";
                Response.Redirect("./RunandFixManage.aspx", false);
            }

           
        }
    }
}