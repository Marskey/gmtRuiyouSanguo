using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace gmt
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public static class UserManager
    {
        #region 对外方法

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            InitUserDictionary();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>添加结果</returns>
        public static string AddUser(UserInfo user)
        {
            if (UserDictionary.ContainsKey(user.account))
            {
                return @"{ ""error"" : ""Account_Already_Exist""}";
            }
            lock (UserDictionary)
            {
                user.state = StateType.Normal;
                if (InsertUserDataToDB(user))
                {
                    SetPrivilegeDictionary(user.privilege, out user.privilegeDic);
                    UserDictionary.Add(user.account, user);
                    return @"{ ""error"" : ""Add_User_Success"" }";
                }
                return @"{ ""error"" : ""Insert_DB_Failed"" }";
            }
        }

        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="user">用户信息</param>
        /// <returns></returns>
        public static string ModifyUser(string account, UserInfo user, UpdateType updateType)
        {
            if (UserDictionary.ContainsKey(account))
            {
                lock (UserDictionary)
                {
                    if (UpdateUserDataFromDB(account, user, updateType))
                    {
                        if ((updateType & UpdateType.Account) == UpdateType.Account)
                        {
                            UserDictionary[account].account = user.account;
                        }
                        if ((updateType & UpdateType.Password) == UpdateType.Password)
                        {
                            UserDictionary[account].password = user.password;
                        }
                        if ((updateType & UpdateType.Privilege) == UpdateType.Privilege)
                        {
                            UserDictionary[account].privilege = user.privilege;
                            SetPrivilegeDictionary(UserDictionary[account].privilege, out UserDictionary[account].privilegeDic);
                        }
                        if ((updateType & UpdateType.State) == UpdateType.State)
                        {
                            UserDictionary[account].state = user.state;
                        }
                        if ((updateType & UpdateType.Language) == UpdateType.Language)
                        {
                            UserDictionary[account].language = user.language;
                        }
                        return @"{ ""error"" : ""Update_User_Success""}";
                    }
                    return @"{ ""error"" : ""Update_DB_Failed""}";
                }
            }
            return @"{ ""error"" : ""Account_Not_Exist""}";
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="account">用户名</param>
        /// <returns></returns>
        public static string DeleteUser(string account)
        {
            if (UserDictionary.ContainsKey(account))
            {
                lock (UserDictionary)
                {
                    if (DeleteUserFromDB(account))
                    {
                        UserDictionary.Remove(account);
                        return @"{ ""error"" : ""Delete_User_Success""}";
                    }
                    return @"{ ""error"" : ""Delete_DB_Failed""}";
                }
            }
            return @"{ ""error"" : ""Account_Not_Exist""}";
        }

        #endregion

        #region 内部方法

        /// <summary>
        /// 设置用户权限字典
        /// </summary>
        /// <param name="userPrivilege"></param>
        /// <param name="privilegeDic"></param>
        private static void SetPrivilegeDictionary(PrivilegeType userPrivilege, out Dictionary<string, bool> privilegeDic)
        {
            privilegeDic = new Dictionary<string, bool>();
            foreach (PrivilegeType privilege in Enum.GetValues(typeof(PrivilegeType)))
            {
                privilegeDic.Add(privilege.ToString(), (userPrivilege & privilege) == privilege);
            }
        }

        /// <summary>
        /// 初始化用户字典
        /// </summary>
        private static void InitUserDictionary()
        {
            UserDictionary.Clear();
            string sql = @"SELECT account, password, privilege, state, language From tb_user";
            DatabaseAssistant.Execute(reader =>
                {
                    while (reader.Read())
                    {
                        UserInfo user = new UserInfo();
                        user.account = reader.GetString(0);
                        user.password = reader.GetString(1);
                        user.privilege = (PrivilegeType)reader.GetInt32(2);
                        SetPrivilegeDictionary(user.privilege, out user.privilegeDic);
                        user.state = (StateType)reader.GetInt32(3);
                        user.language = reader.GetString(4);
                        UserDictionary.Add(user.account, user);
                    }
                }
                , Global.GMT_DB_Address
                , Global.GMT_DB_Port
                , Global.GMT_DB_Charset
                , Global.GMT_DB_Name
                , Global.GMT_DB_User
                , Global.GMT_DB_Pwd
                , sql);

            if (!UserDictionary.ContainsKey("admin"))
            {
                UserInfo admin = new UserInfo();
                admin.account = "admin";
                admin.password = "123456";
                foreach (PrivilegeType privilege in Enum.GetValues(typeof(PrivilegeType)))
                {
                    admin.privilege |= privilege;
                }
                SetPrivilegeDictionary(admin.privilege, out admin.privilegeDic);
                admin.state = StateType.Normal;
                admin.language = "zh-CN";
                UserDictionary.Add(admin.account, admin);
                InsertUserDataToDB(admin);
            }
        }

        /// <summary>
        /// 保存用户数据到数据库
        /// </summary>
        /// <param name="user">用户数据</param>
        /// <returns></returns>
        private static bool InsertUserDataToDB(UserInfo user)
        {
            string sql = string.Format(@"INSERT INTO tb_user (account, password, privilege, state, language) VALUES ('{0}', '{1}', {2}, {3}, '{4}')", user.account, user.password, (int)user.privilege, (int)user.state, user.language);
            if (!DatabaseAssistant.Execute(Global.GMT_DB_Address, Global.GMT_DB_Port, Global.GMT_DB_Charset, Global.GMT_DB_Name, Global.GMT_DB_User, Global.GMT_DB_Pwd, sql))
            {
                Log.AddLog(string.Format(@"Error: mysql insert failed. sql: {0}", sql));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从数据库删除用户数据
        /// </summary>
        /// <param name="account">用户名</param>
        /// <returns></returns>
        private static bool DeleteUserFromDB(string account)
        {
            string sql = string.Format(@"DELETE FROM tb_user WHERE account='{0}'", account);
            if (!DatabaseAssistant.Execute(Global.GMT_DB_Address, Global.GMT_DB_Port, Global.GMT_DB_Charset, Global.GMT_DB_Name, Global.GMT_DB_User, Global.GMT_DB_Pwd, sql))
            {
                Log.AddLog(string.Format(@"Error: mysql delete failed. sql: {0}", sql));
                return false;
            }
            return true;
        }

        /// <summary>
        /// 更新用户数据
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="user">用户数据</param>
        /// <param name="updateType">更新类型</param>
        /// <returns></returns>
        private static bool UpdateUserDataFromDB(string account, UserInfo user, UpdateType updateType)
        {
            string sqlex = "";
            if ((updateType & UpdateType.Account) == UpdateType.Account)
            {
                sqlex = string.Format(@"{0}{1} account='{2}'", sqlex, string.IsNullOrEmpty(sqlex) ? "" : ",", user.account);
            }
            if ((updateType & UpdateType.Password) == UpdateType.Password)
            {
                sqlex = string.Format(@"{0}{1} password='{2}'", sqlex, string.IsNullOrEmpty(sqlex) ? "" : ",", user.password);
            }
            if ((updateType & UpdateType.Privilege) == UpdateType.Privilege)
            {
                sqlex = string.Format(@"{0}{1} privilege={2}", sqlex, string.IsNullOrEmpty(sqlex) ? "" : ",", (int)user.privilege);
            }
            if ((updateType & UpdateType.State) == UpdateType.State)
            {
                sqlex = string.Format(@"{0}{1} state={2}", sqlex, string.IsNullOrEmpty(sqlex) ? "" : ",", (int)user.state);
            }
            if ((updateType & UpdateType.Language) == UpdateType.Language)
            {
                sqlex = string.Format(@"{0}{1} language='{2}'", sqlex, string.IsNullOrEmpty(sqlex) ? "" : ",", user.language);
            }
            string sql = string.Format(@"UPDATE tb_user SET {0} WHERE account='{1}'", sqlex, account);

            if (!DatabaseAssistant.Execute(Global.GMT_DB_Address, Global.GMT_DB_Port, Global.GMT_DB_Charset, Global.GMT_DB_Name, Global.GMT_DB_User, Global.GMT_DB_Pwd, sql))
            {
                Log.AddLog(string.Format(@"Error: mysql update failed. sql: {0}", sql));
                return false;
            }
            return true;
        }

        #endregion


        /// <summary>
        /// 用户字典
        /// </summary>
        private static Dictionary<string, UserInfo> UserDictionary = new Dictionary<string, UserInfo>();

        /// <summary>
        /// 用户列表
        /// </summary>
        public static Dictionary<string, UserInfo> UserTable
        {
            get
            {
                return UserDictionary;
            }
        }

        /// <summary>
        /// 数据库更新类型
        /// </summary>
        public enum UpdateType
        {
            Null = 0,
            /// <summary>
            /// 更改账号
            /// </summary>
            Account = 1,
            /// <summary>
            /// 更改密码
            /// </summary>
            Password = 1 << 1,
            /// <summary>
            /// 更改权限
            /// </summary>
            Privilege = 1 << 2,
            /// <summary>
            /// 更改状态
            /// </summary>
            State = 1 << 3,
            /// <summary>
            /// 更改偏好语言
            /// </summary>
            Language = 1 << 4,
        }
    }

    /// <summary>
    /// 用户权限
    /// </summary>
    public enum PrivilegeType
    {
        /// <summary>
        /// 普通
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 修改
        /// </summary>
        Modify = 1,

        /// <summary>
        /// 下载
        /// </summary>
        Download = 1 << 1,

        #region 访问界面的权限

        /// <summary>
        /// 用户管理界面
        /// </summary>
        User = 1 << 2,

        /// <summary>
        /// 首页
        /// </summary>
        GmModify = 1 << 3,

        /// <summary>
        /// 滚屏公告
        /// </summary>
        Notice = 1 << 4,

        /// <summary>
        /// 运营活动设置
        /// </summary>
        Activity = 1 << 5,

        /// <summary>
        /// 活动编辑
        /// </summary>
        ActivityOperate = 1 << 6,

        /// <summary>
        /// 游戏激活码
        /// </summary>
        CreateKey = 1 << 7,

        /// <summary>
        /// 游戏礼包码
        /// </summary>
        CreateGift = 1 << 8,

        /// <summary>
        /// 礼包编辑
        /// </summary>
        Gift = 1 << 9,

        /// <summary>
        /// 批量给予物品
        /// </summary>
        BatchGive = 1 << 10,

        /// <summary>
        /// 开服活动设置
        /// </summary>
        ActivityOpenTime = 1 << 11,

        /// <summary>
        /// 玩家信息查询
        /// </summary>
        GmNormal = 1 << 12,

        /// <summary>
        /// 充值记录查询
        /// </summary>
        PlayerRechargeLook = 1 << 13,

        /// <summary>
        /// 玩家历史记录
        /// </summary>
        PlayerHistory = 1 << 14,

        /// <summary>
        /// 服务器数据
        /// </summary>
        ServerData = 1 << 15,

        /// <summary>
        /// FTP地址编辑
        /// </summary>
        FTPEdit = 1 << 16,

        /// <summary>
        /// 支付方式
        /// </summary>
        PayType = 1 << 17,

        /// <summary>
        /// 合并服务器
        /// </summary>
        MergeServer = 1 << 18,

        /// <summary>
        /// 服务器列表
        /// </summary>
        SectionServer = 1 << 19,

        #endregion

    }

    /// <summary>
    /// 用户状态
    /// </summary>
    public enum StateType
    {
        /// <summary>
        /// 无状态
        /// </summary>
        Null = 0,

        /// <summary>
        /// 等待审核
        /// </summary>
        WaitForCheck = 1,

        /// <summary>
        /// 正常状态
        /// </summary>
        Normal = 1 << 1,

        /// <summary>
        /// 离线状态
        /// </summary>
        Offline = 1 << 2,

    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {
        public UserInfo()
        {
            account = "";
            password = "";
            privilege = PrivilegeType.Normal;
            privilegeDic = new Dictionary<string, bool>();
            state = StateType.Null;
            language = "";
        }

        /// <summary>
        /// 账号
        /// </summary>
        public string account;
        /// <summary>
        /// 密码
        /// </summary>
        public string password;
        /// <summary>
        /// 权限
        /// </summary>
        public PrivilegeType privilege;
        /// <summary>
        /// 权限字典
        /// </summary>
        public Dictionary<string, bool> privilegeDic;
        /// <summary>
        /// 状态
        /// </summary>
        public StateType state;
        /// <summary>
        /// 偏好语言
        /// </summary>
        public string language;
    }

}