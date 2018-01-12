using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Web.Services;
using System.IO;
using Newtonsoft.Json;

namespace gmt
{
    public partial class User : AGmPage
    {
        public User() : base(PrivilegeType.User) { }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
        }

        [WebMethod(EnableSession = true)]
        public static string GetUserData()
        {
            List<UserInfo> userList = UserManager.UserTable.Values.ToList<UserInfo>();
            string account = HttpContext.Current.Session["user"] as string;
            for (int i = userList.Count - 1; i >= 0; i--)
            {
                if (userList[i].account == account || userList[i].account == "admin")
                {
                    userList.RemoveAt(i);
                }
            }
            if (userList.Count > 0)
            {
                return JsonConvert.SerializeObject(userList);
            }
            return @"{ ""error"" : 1}";
        }

        [WebMethod(EnableSession = true)]
        public static string UpdateUserInfo(string account, string pwd, string privileges, string opt)
        {
            UserInfo user = new UserInfo();
            user.account = account;
            user.password = pwd;
            Dictionary<string, bool> privilegeDic = JsonConvert.DeserializeObject<Dictionary<string, bool>>(privileges);
            if (privilegeDic.Count > 0)
            {
                user.privilege = PrivilegeType.Normal;
                foreach (PrivilegeType privilege in Enum.GetValues(typeof(PrivilegeType)))
                {
                    if (privilegeDic.ContainsKey(privilege.ToString()) && privilegeDic[privilege.ToString()])
                        user.privilege |= privilege;
                }
            }
            switch (opt)
            {
                case "Add": return UserManager.AddUser(user);
                case "Modify": return UserManager.ModifyUser(account, user, UserManager.UpdateType.Privilege);
                case "Delete": return UserManager.DeleteUser(account);
            }
            return @"{{ ""error"" : 1}}";
        }

    }
}