using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace gmt
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string account = Session["user"] as string;

                if (!string.IsNullOrEmpty(account))
                {
                    Response.Redirect("./GmNormal.aspx");
                    Response.End();
                    return;
                }

                string directUrl = "";
                string user = Request.Form["user"];
                string pwd = Request.Form["pwd"];

                if (string.IsNullOrEmpty(user)
                    || string.IsNullOrEmpty(pwd))
                {
                    return;
                }

                int errCode = (int)userLogin(user, pwd, out directUrl);
                if (errCode == 0)
                {
                    Response.Redirect(directUrl);
                    return;
                }
                else
                {
                    Response.Write("<script> alert('Account or password not correct!'); </script>");
                }
            }
        }

        protected EErrType userLogin(string user, string pwd, out string url)
        {
            url = "";

            Regex rgAcc = new Regex("^[a-zA-Z0-9_]{3,15}$");
            if (!rgAcc.IsMatch(user))
            {
                return EErrType.ERR_LOGIN_FAILED;
            }

            Regex rgPwd = new Regex("^[a-zA-Z0-9_+`!@#$%^&*;./:<>?]{3,18}$");
            if (!rgPwd.IsMatch(pwd))
            {
                return EErrType.ERR_LOGIN_FAILED;
            }

            UserInfo userInfo = null;
            if (!UserManager.UserTable.TryGetValue(user, out userInfo) || userInfo.password != pwd)
            {
                return EErrType.ERR_LOGIN_ACC_PWD_NOT_FOUND;
            }
            else
            {
                Session["user"] = userInfo.account;
                if ((userInfo.privilege & PrivilegeType.GmModify) == PrivilegeType.GmModify)
                {
                    url = "./GmModify.aspx";
                }
                else
                {
                    foreach (PrivilegeType privilege in Enum.GetValues(typeof(PrivilegeType)))
                    {
                        if (privilege != PrivilegeType.Normal && privilege != PrivilegeType.Modify && privilege != PrivilegeType.Download)
                        {
                            if ((userInfo.privilege & privilege) == privilege)
                            {
                                url = string.Format(@"./{0}.aspx", privilege.ToString());
                            }
                        }
                    }
                }
            }

            return EErrType.ERR_SUCCESS;
        }
    }
}