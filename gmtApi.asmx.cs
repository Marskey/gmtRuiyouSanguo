using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Newtonsoft.Json;

namespace gmt.api
{
    /// <summary>
    /// gmtApi 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class gmtApi : System.Web.Services.WebService
    {

        [WebMethod]
        public void HelloWorld()
        {
            this.Context.Response.Write("Hello World");
            this.Context.Response.End();
        }

        struct STSvrInfo
        {
            public uint serv_id;
            public string serv_name;
        };

        [WebMethod]
        public void GetServerList()
        {
            List<gmt.STServerInfo> listSvrInfo = gmt.Server.GetServerList();
            List<STSvrInfo> list = new List<STSvrInfo>();
            foreach (var si in listSvrInfo)
            {
                STSvrInfo svr = new STSvrInfo();
                svr.serv_id = si.id;
                svr.serv_name = si.name;
                list.Add(svr);
            }

            string ret = string.Format("{{\"err_code\":{0}, \"serv\":{1}}}", 0, JsonConvert.SerializeObject(list));
            this.Context.Response.Write(ret);
            this.Context.Response.End();
        }

        struct STRoleInfo
        {
            public int err_code;
            public uint user_id;
            public string role_name;
            public int role_rank;
            public int role_id;
            public int app_id;
        };

        [WebMethod]
        public void GetRoleInfo(uint serv_id, uint user_id)
        {
            STRoleInfo st = new STRoleInfo();
            st.err_code = -1;
            st.user_id = user_id;
            st.role_name = "";
            st.app_id = int.Parse(gmt.Global.GetGameAppID);

            if (serv_id != 0 && user_id != 0)
            {
                gmt.STServerInfo server = gmt.Server.GetServerInfo(serv_id);
                if (server != null)
                {
                    #region
                    string sql = string.Format("SELECT t1.uid , uname , team_level FROM USER AS t1 JOIN card_package AS t2 ON t1.uid = t2.uid WHERE t1.cyuid = {0} LIMIT 1", user_id);
                    DatabaseAssistant.Execute(reader =>
                    {
                        if (reader.Read())
                        {
                            st.err_code = 1;
                            st.role_id = reader.GetInt32(0);
                            st.role_name = reader.GetString(1);
                            st.role_rank = reader.GetInt32(2);
                        }
                    },
                        server.gamedb.host,
                        server.gamedb.port.ToString(),
                        server.gamedb.cset.ToString(),
                        server.gamedb.name,
                        server.gamedb.user,
                        server.gamedb.pwd,
                        sql
                    );
                    #endregion
                }
            }

            string ret = JsonConvert.SerializeObject(st);
            this.Context.Response.Write(ret);
            this.Context.Response.End();
        }
    }
}
