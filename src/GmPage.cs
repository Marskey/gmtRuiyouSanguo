using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace gmt
{
    /// <summary>
    /// GM页面抽象
    /// </summary>
    public abstract class AGmPage : System.Web.UI.Page
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="needPrivilege">需要的权限</param>
        public AGmPage(PrivilegeType needPrivilege)
        {
            this.needPrivilege = needPrivilege;
        }

        public AGmPage()
        {

        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            string account = Session["user"] as string;
            UserInfo data;
            //Server server	= gmt.Server.GetServerAt(gmt.Server.Count - 1);
            if ((!string.IsNullOrEmpty(account) && UserManager.UserTable.TryGetValue(account, out data) && (data.privilege & this.needPrivilege) == this.needPrivilege)/* ||
				(server != null && server.GmAddress == "do_not_need_Login")*/)
            {
                data.language = "zh-CN";
                if (null != Request.Cookies["lan"])
                {
                    string lang = Request.Cookies["lan"].Value;
                    if (!string.IsNullOrEmpty(lang))
                    {
                        data.language = lang;
                        UserManager.ModifyUser(data.account, data, UserManager.UpdateType.Language);
                    }
                }

                this.OnGmPageLoad();
            }
            else
            {
                if (string.IsNullOrEmpty(account))
                {
                    Session["return"] = Request.RawUrl;
                    Response.Redirect("./Login.aspx", true);
                }
                else if (this.needPrivilege == PrivilegeType.Download)
                {
                    Response.Write("<script>alert(\"" + TableManager.GetGMTText(900) + "\")</script>");
                }
                else
                {
                    Response.Redirect("./Error.aspx", true);
                }
            }
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        /// <param name="playerId">玩家编号</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="operateText">操作文本</param>
        /// <param name="needReturn">是否需要返回</param>
        /// <param name="reportProcess">报告处理</param>
        /// <returns>是否成功</returns>
        protected virtual bool ExecuteGmCommand(string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        {
            Server server = Session["Server"] as Server;
            string account = Session["user"] as string;

            return AGmPage.ExecuteGmCommand(account, server, playerId, commandText, operateText, needReturn, reportProcess);
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="server">服务器</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="operateText">操作文本</param>
        /// <param name="needReturn">是否需要返回</param>
        /// <param name="reportProcess">报告处理</param>
        /// <returns>是否成功</returns>
        internal static bool ExecuteGmCommand(string user, Server server, string playerId, string commandText, string operateText, bool needReturn, Action<string> reportProcess)
        {
            if (AGmPage.ExecuteGmCommand(server, playerId, Encoding.UTF8.GetBytes(commandText), Encoding.UTF8.GetBytes(operateText), needReturn, reportProcess))
            {
                Log.AddRecord(user, string.Format("{0}\r\n{1}\r\n" + TableManager.GetGMTText(755), server.Name, commandText));
                return true;
            }
            else
            {
                Log.AddRecord(user, string.Format("{0}\r\n{1}\r\n" + TableManager.GetGMTText(756), server.Name, commandText));
                return false;
            }
        }

        /// <summary>
        /// 执行GM命令
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="commandText">命令文本</param>
        /// <param name="operateText">操作文本</param>
        /// <param name="needReturn">是否需要返回</param>
        /// <param name="reportProcess">报告处理</param>
        /// <returns>是否成功</returns>
        internal static bool ExecuteGmCommand(Server server, string playerId, byte[] command, byte[] operate, bool needReturn, Action<string> reportProcess)
        {
            if (server == null)
            {
                return false;
            }

            HttpWebRequest request = null;
            HttpWebResponse respone = null;

            try
            {
                request = WebRequest.Create(server.GmAddress) as HttpWebRequest;
                request.KeepAlive = false;
                request.Headers["svr"] = server.Id;
                request.Headers["uid"] = playerId;
                request.Headers["cmd"] = Convert.ToBase64String(command);
                request.Headers["opt"] = Convert.ToBase64String(operate);
                request.Timeout = 10000;

                respone = request.GetResponse() as HttpWebResponse;
                if (needReturn && reportProcess != null)
                {
                    using (StreamReader reader = new StreamReader(respone.GetResponseStream()))
                    {
                        reportProcess(reader.ReadToEnd());
                    }
                }
                return true;
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                if (reportProcess != null) { reportProcess(exception.ToString()); }
                return false;
            }

            finally
            {
                if (request != null) { request.Abort(); }
                if (respone != null) { respone.Close(); }
            }
        }

        /// <summary>
        /// 执行数据库
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="fileName">SQL文件名</param>
        /// <returns>是否成功</returns>
        protected bool Execute(gmt.Server server, string fileName)
        {
            string file = HttpRuntime.AppDomainAppPath + fileName;

            if (File.Exists(file))
            {
                using (var reader = File.OpenText(file))
                {
                    return DatabaseAssistant.Execute
                    (
                        server.DatabaseAddress,
                        server.DatabasePort,
                        server.DatabaseCharSet,
                        server.GameDatabase,
                        server.DatabaseUserId,
                        server.DatabasePassword,
                        reader.ReadToEnd()
                    );
                }
            }

            return false;
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected abstract void OnGmPageLoad();

        /// <summary>
        /// 需要的权限
        /// </summary>
        protected PrivilegeType needPrivilege;
    }
}