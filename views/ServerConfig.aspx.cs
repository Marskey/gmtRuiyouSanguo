using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.Script.Services;
using System.Text;
using Newtonsoft.Json;

namespace gmt.views
{
    public partial class ServerConfig : AGmPage
    {
        public ServerConfig()
            : base(PrivilegeType.ServerConfig)
        {
        }

        protected override void OnGmPageLoad()
        {
            if (string.IsNullOrEmpty(Request.Form["svr_id"]))
                return;

            STServerInfo serverInfo = new STServerInfo();
            serverInfo.svrID = Request.Form["svr_id"];
            serverInfo.id = CUtils.idatoi(serverInfo.svrID);
            serverInfo.section = int.Parse(Request.Form["svr_section"]);
            serverInfo.sectionName = Request.Form["svr_section_name"];
            serverInfo.name = Request.Form["svr_name"];
            serverInfo.ip = Request.Form["svr_ip"];
            serverInfo.authGMHttp = Request.Form["auth_gm_http"];
            serverInfo.recommend = (Request.Form["svr_recommend"] == "on") ? true : false;
            serverInfo.status = int.Parse(Request.Form["svr_status"]);
            serverInfo.param = Request.Form["svr_param"];

            serverInfo.gamedb = JsonConvert.DeserializeObject<STDBInfo>(Request.Form["gamedb"]);
            serverInfo.codedb = JsonConvert.DeserializeObject<STDBInfo>(Request.Form["codedb"]);
            serverInfo.logdb = JsonConvert.DeserializeObject<STDBInfo>(Request.Form["logdb"]);
            serverInfo.authdb = JsonConvert.DeserializeObject<STDBInfo>(Request.Form["authdb"]);

            gmt.Server.AddServerInfo(serverInfo.id, serverInfo);
            Response.Write("{\"error\":0}");
            Response.End();
        }

        [WebMethod(EnableSession = true)]
        public static string GetServerInfoList(string order)
        {
            List<STServerInfo> list = gmt.Server.GetServerList();
            return JsonConvert.SerializeObject(list);
        }

        [WebMethod(EnableSession = true)]
        public static string RemoveServerInfo(List<uint> ids)
        {
            List<uint> removedIds = new List<uint>();
            foreach (var id in ids)
            {
                if (gmt.Server.RemoveServerInfo(id))
                {
                    removedIds.Add(id);
                }
            }
            gmt.Server.newSave();
            return JsonConvert.SerializeObject(removedIds);
        }

        [WebMethod(EnableSession = true)]
        public static string ModifyServerStatus(List<uint> ids, bool bIsRecommend, int nListStatus)
        {
            List<uint> modifyIds = new List<uint>();
            foreach (var id in ids)
            {
                STServerInfo st = gmt.Server.GetServerInfo(id);
                if (null == st)
                    continue;
                st.recommend = bIsRecommend;
                st.status = nListStatus;
            }
            return "{\"error\":0}";
        }

        [WebMethod(EnableSession = true)]
        public static string StopServer(List<uint> ids)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in ids)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SDN()"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string StopAllUserIn(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SIPC(1)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string AllowAllUserIn(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SIPC(0)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string StopAllUserRegister(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SRP(1)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string AllowAllUserRegister(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SRP(0)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string StopFightCheck(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SFC(1)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string StartFightCheck(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("SFC(0)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string ReloadAllTableConfig(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("RL(1)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string ReloadAllTableConfigButActivity(List<uint> svrIds)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes("RL(0)"), Encoding.UTF8.GetBytes(""), false, text =>
                {
                    msg += server.svrID + ": " + text;
                }))
                {
                    idsSuccess.Add(id);
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }

        [WebMethod(EnableSession = true)]
        public static string AddWhiteList(List<uint> svrIds, List<string> ips)
        {
            string msg = "";
            List<uint> idsSuccess = new List<uint>();
            foreach (var id in svrIds)
            {
                STServerInfo server = gmt.Server.GetServerInfo(id);
                foreach (var ip in ips)
                {
                    string cmd = string.Format("SIP(\"{0}\")", ip);
                    if (AGmPage.ExecuteGmCommand(server, "0", Encoding.UTF8.GetBytes(cmd), Encoding.UTF8.GetBytes(""), false, text =>
                    {
                        msg += server.svrID + ": " + text;
                    }))
                    {
                        idsSuccess.Add(id);
                    }
                }
            }

            return string.Format("{{\"ids\":{0}, \"msg\": \"{1}\"}}", JsonConvert.SerializeObject(idsSuccess), msg);
        }
    }
}