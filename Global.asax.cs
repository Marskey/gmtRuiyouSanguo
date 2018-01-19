using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace gmt
{
    /// <summary>
    /// 全局应用程序
    /// </summary>
    public class Global : System.Web.HttpApplication
    {
        private static string m_ConfigPath = HttpRuntime.AppDomainAppPath + "configs/";
        private static string m_ProtoDataPath = HttpRuntime.AppDomainAppPath + "protodatas/";
        private static string m_MiscLogPath = HttpRuntime.AppDomainAppPath + "MiscLog/";

        public static string ConfigPath { get { return m_ConfigPath; } }
        public static string ProtoDataPath { get { return m_ProtoDataPath; } }
        public static string MiscLogPath { get { return m_MiscLogPath; } }

        public static string GMT_DB_Address = "";
        public static string GMT_DB_Port = "";
        public static string GMT_DB_Charset = "";
        public static string GMT_DB_Name = "";
        public static string GMT_DB_User = "";
        public static string GMT_DB_Pwd = "";

        private static string m_appID = "";
        public static string GetGameAppID { get { return m_appID; } }

        /// <summary>
        /// 初始化
        /// </summary>
        protected static void Init()
        {
            GMT_DB_Address = CUtils.ReadIniValue("GMT_DB_Info", "address", "", Global.ConfigPath + "config.ini");
            GMT_DB_Port = CUtils.ReadIniValue("GMT_DB_Info", "port", "", Global.ConfigPath + "config.ini");
            GMT_DB_Charset = CUtils.ReadIniValue("GMT_DB_Info", "charset", "", Global.ConfigPath + "config.ini");
            GMT_DB_Name = CUtils.ReadIniValue("GMT_DB_Info", "dbname", "", Global.ConfigPath + "config.ini");
            GMT_DB_User = CUtils.ReadIniValue("GMT_DB_Info", "user", "", Global.ConfigPath + "config.ini");
            GMT_DB_Pwd = CUtils.ReadIniValue("GMT_DB_Info", "pwd", "", Global.ConfigPath + "config.ini");

            m_appID = CUtils.ReadIniValue("GAME_INFO", "app_id", "", Global.ConfigPath + "config.ini");
        }

        /// <summary>
        /// 应用程序启动
        /// </summary>
        protected void Application_Start(object sender, EventArgs e)
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 100;

            Init();

            UserManager.Init();

            gmt.Server.Load();
            gmt.Server.newLoad();

            Log.Start();
            Network.Start();
            FTPManager.Load();
            TableManager.Start();
            TextManager.Load();
            RevolvingManager.Start();
            NoticeManager.Load();

            ActivityManger.Start();
            ServerListConfig.Load();
            GMTActivityMananger.start();
            GiftTable.Start();
            TimedMailSender.Init();
            RevolvingManager.UpdateService();
            PayType.LoadPayType();

        }

        /// <summary>
        /// 新会话启动
        /// </summary>
        protected void Session_Start(object sender, EventArgs e)
        {
            BinaryWriter writer = new BinaryWriter(new MemoryStream());

            writer.Write((int)0);
            writer.Write((ushort)MessageType.Url);
            writer.Write(this.Request.Url.Authority);

            Network.Send(writer);
        }

        /// <summary>
        /// 出现未处理的错误
        /// </summary>
        protected void Application_Error(object sender, EventArgs e)
        {
            DatabaseAssistant.ReportException(Server.GetLastError());
            //Server.ClearError();
        }

        /// <summary>
        /// 会话结束
        /// </summary>
        protected void Session_End(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 应用程序关闭
        /// </summary>
        protected void Application_End(object sender, EventArgs e)
        {
            Log.End();
        }

        /// <summary>
        /// 查询启动
        /// </summary>
        protected void Request_Start(object sender, EventArgs e)
        {

        }
    }
}