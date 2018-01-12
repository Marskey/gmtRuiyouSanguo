using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 公告界面
    /// </summary>
    public partial class Notice : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Notice()
            : base(PrivilegeType.Notice)
        {
        }

        /// <summary>
        /// 发送走马灯
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="server">服务器</param>
        /// <param name="content">走马灯内容</param>
        /// <param name="reportProcess">报告处理</param>
        /// <returns>是否成功</returns>
        public static bool SendCommand(string user, Server server, string content, Action<string> reportProcess)
        {
            HttpWebRequest request = null;
            HttpWebResponse respone = null;

            try
            {
                request = WebRequest.Create(server.GmAddress) as HttpWebRequest;
                request.Headers["svr"] = server.Id;
                request.Headers["uid"] = "0";
                request.Headers["cmd"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
                request.Timeout = 1000;

                respone = request.GetResponse() as HttpWebResponse;

                if (reportProcess != null)
                {
                    using (StreamReader reader = new StreamReader(respone.GetResponseStream()))
                    {
                        string text = reader.ReadToEnd();

                        if (text != null && text != "")
                        {
                            reportProcess(text);
                        }
                        else
                        {
                            reportProcess(TableManager.GetGMTText(391));
                        }
                    }
                }
                Log.AddRecord(user, string.Format("{0}\r\n{1}\r\n成功", server.Name, content));
                return true;
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                Log.AddRecord(user, string.Format("{0}\r\n{1}\r\n失败", server.Name, content));
                return false;
            }

            finally
            {
                if (request != null) { request.Abort(); }
                if (respone != null) { respone.Close(); }
            }
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                gmt.Server server = Session["Server"] as gmt.Server;

                for (int i = 0; i < gmt.Server.Count; ++i)
                {
                    gmt.Server theServer = gmt.Server.GetServerAt(i);
                    this.serverList.Items.Add(theServer.Name);
                    this.serverListBox.Items.Add(theServer.Name);

                    if (theServer == server)
                    {
                        this.serverList.SelectedIndex = i;
                    }
                }

                if (server == null)
                {
                    Session["Server"] = server = gmt.Server.GetServerAt(0);
                }

                this.RefreshRevolving();
                //this.RefreshNotice();
            }
        }

        /// <summary>
        /// 服务器列表改变响应
        /// </summary>
        protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["Server"] = gmt.Server.GetServerAt(this.serverList.SelectedIndex);

            this.RefreshRevolving();
            //this.RefreshNotice();
        }

        /// <summary>
        /// 添加走马灯按钮点击响应
        /// </summary>
        protected void addRevolvingButton_Click(object sender, EventArgs e)
        {
            if (this.revolvingTextBox.Text == "") { return; }

            int second;
            if (!int.TryParse(this.intervalTextBox.Text, out second) || second <= 0) { return; }

            RevolvingData data = RevolvingManager.AddRevolving(Session["Server"] as Server, this.revolvingTextBox.Text, second);
            RevolvingManager.Save();
            RevolvingManager.UpdateService();
            this.revolvingListBox.Items.Add(data.ToString());
        }

        /// <summary>
        /// 移除走马灯按钮点击响应
        /// </summary>
        protected void removeRevolvingButton_Click(object sender, EventArgs e)
        {
            if (this.revolvingListBox.SelectedIndex < 0) { return; }

            RevolvingManager.RemoveRevolving(Session["Server"] as Server, this.revolvingListBox.SelectedIndex);
            RevolvingManager.Save();
            RevolvingManager.UpdateService();
            this.revolvingListBox.Items.RemoveAt(this.revolvingListBox.SelectedIndex);
        }

        /// <summary>
        /// 添加公告按钮点击响应
        /// </summary>
        //protected void addNoticeButton_Click(object sender, EventArgs e)
        //{
        //    if (this.titleTextBox.Text == "" ||
        //        this.detailTextBox.Text == "" ||
        //        this.idTextBox.Text == "")
        //    {
        //        return;
        //    }

        //    int id;
        //    if (!int.TryParse(this.idTextBox.Text, out id))
        //    {
        //        this.outputLabel.Text = "编号格式不对";
        //        return;
        //    }

        //    if (id < 50000 || id >= 52000)
        //    {
        //        this.outputLabel.Text = "编号只能在[50000,52000)之间";
        //        return;
        //    }

        //    Server	server	= Session["Server"] as Server;
        //    string account = Session["user"] as string;
        //    bool	isAdded	= NoticeManager.AddNotice(server, id, this.titleTextBox.Text, this.detailTextBox.Text.Replace("\r\n", "\\n"));
        //    NoticeManager.Save();

        //    if (isAdded)
        //    {
        //        this.noticeListBox.Items.Add(new ListItem(id + " " + this.titleTextBox.Text, id.ToString()));
        //    }
        //    else
        //    {
        //        for (int i = 0; i < this.noticeListBox.Items.Count; ++i)
        //        {
        //            ListItem item = this.noticeListBox.Items[i];
        //            if (item.Value == id.ToString())
        //            {
        //                item.Text = id + " " + this.titleTextBox.Text;
        //                break;
        //            }
        //        }
        //    }

        //    Notice.SendCommand(account, server, string.Format("ACN({0},{0},\"{2}\",{1},\"{3}\")", id, id + 10000, this.titleTextBox.Text, this.detailTextBox.Text.Replace("\r\n", "\\n")), text => this.outputLabel.Text = text);
        //}

        /// <summary>
        /// 移除公告按钮点击响应
        /// </summary>
        //protected void removeNoticeButton_Click(object sender, EventArgs e)
        //{
        //    if (this.noticeListBox.SelectedIndex < 0) { return; }

        //    Server server = Session["Server"] as Server;
        //    string account = Session["user"] as string;

        //    int id = int.Parse(this.noticeListBox.SelectedItem.Value);
        //    if (Notice.SendCommand(account, server, string.Format("DCN({0})", id), text => this.outputLabel.Text = text))
        //    {
        //        NoticeManager.RemoveNotice(server, id);
        //        NoticeManager.Save();
        //        this.noticeListBox.Items.RemoveAt(this.noticeListBox.SelectedIndex);
        //    }
        //}

        /// <summary>
        /// 发送到服务器按钮点击响应
        /// </summary>
        protected void sendButton_Click(object sender, EventArgs e)
        {
            Server server = Session["Server"] as Server;
            string account = Session["user"] as string;

            foreach (var data in NoticeManager.GetNotice(server).List)
            {
                Notice.SendCommand(account, server, string.Format("ACN({0},{0},\"{2}\",{1},\"{3}\")", data.Id, data.Id + 10000, data.Title, data.Content), text => this.outputLabel.Text = text);
            }
        }

        /// <summary>
        /// 刷新走马灯
        /// </summary>
        private void RefreshRevolving()
        {
            this.revolvingListBox.Items.Clear();

            List<RevolvingData> list = RevolvingManager.GetRevolving(Session["Server"] as Server);

            if (list != null)
            {
                foreach (var data in list)
                {
                    this.revolvingListBox.Items.Add(data.ToString());
                }
            }
        }

        /// <summary>
        /// 刷新公告
        /// </summary>
        //private void RefreshNotice()
        //{
        //    this.noticeListBox.Items.Clear();

        //    NoticeList list = NoticeManager.GetNotice(Session["Server"] as Server);

        //    if (list != null)
        //    {
        //        foreach (var data in list.List)
        //        {
        //            this.noticeListBox.Items.Add(new ListItem(data.Id + " " + data.Title, data.Id.ToString()));
        //        }
        //    }
        //}

        /// <summary>
        /// 添加全部钮点击响应
        /// </summary>
        protected void addAllButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.Items.Count == 0) { return; }

            foreach (var item in this.serverListBox.Items)
            {
                this.selectListBox.Items.Add(item.ToString());
            }

            this.serverListBox.Items.Clear();
        }

        /// <summary>
        /// 添加选择按钮点击响应
        /// </summary>
        protected void addSelectButton_Click(object sender, EventArgs e)
        {
            int[] selectSet = this.serverListBox.GetSelectedIndices();
            if (selectSet == null || selectSet.Length == 0) { return; }

            for (int i = selectSet.Length - 1; i >= 0; --i)
            {
                this.selectListBox.Items.Add(this.serverListBox.Items[selectSet[i]].Text);
                this.serverListBox.Items.RemoveAt(selectSet[i]);
            }
        }

        /// <summary>
        /// 移除选择按钮点击响应
        /// </summary>
        protected void removeSelectButton_Click(object sender, EventArgs e)
        {
            int[] selectSet = this.selectListBox.GetSelectedIndices();
            if (selectSet == null || selectSet.Length == 0) { return; }

            for (int i = selectSet.Length - 1; i >= 0; --i)
            {
                this.serverListBox.Items.Add(this.selectListBox.Items[selectSet[i]].Text);
                this.selectListBox.Items.RemoveAt(selectSet[i]);
            }
        }

        /// <summary>
        /// 移除全部钮点击响应
        /// </summary>
        protected void removeAllButton_Click(object sender, EventArgs e)
        {
            if (this.selectListBox.Items.Count == 0) { return; }

            foreach (var item in this.selectListBox.Items)
            {
                this.serverListBox.Items.Add(item.ToString());
            }

            this.selectListBox.Items.Clear();
        }

        /// <summary>
        /// 复制走马灯按钮点击响应
        /// </summary>
        protected void copyRevolvingButton_Click(object sender, EventArgs e)
        {
            gmt.Server server = Session["Server"] as gmt.Server;
            if (server == null) { return; }

            foreach (var item in this.selectListBox.Items)
            {
                if (item.ToString() == this.serverList.Text)
                {
                    continue;
                }

                RevolvingManager.Copy(server, gmt.Server.GetServer(item.ToString()));
            }

            RevolvingManager.Save();
            RevolvingManager.UpdateService();
        }

        /// <summary>
        /// 复制公告钮点击响应
        /// </summary>
        protected void copyNoticeButton_Click(object sender, EventArgs e)
        {
            gmt.Server server = Session["Server"] as gmt.Server;
            string account = Session["user"] as string;
            if (server == null) { return; }

            foreach (var item in this.selectListBox.Items)
            {
                if (item.ToString() == this.serverList.Text)
                {
                    continue;
                }

                NoticeManager.Copy(account, server, gmt.Server.GetServer(item.ToString()));
            }

            NoticeManager.Save();
        }

        /// <summary>
        /// 删除走马灯钮点击响应
        /// </summary>
        protected void deleteRevolvingButton_Click(object sender, EventArgs e)
        {
            foreach (var item in this.selectListBox.Items)
            {
                RevolvingManager.ClearRevolving(gmt.Server.GetServer(item.ToString()));
            }

            RevolvingManager.Save();
            RevolvingManager.UpdateService();
        }
    }

    /// <summary>
    /// 走马灯管理器
    /// </summary>
    static class RevolvingManager
    {
        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            RevolvingManager.Load();
        }

        /// <summary>
        /// 更新服务
        /// </summary>
        /// <returns>是否成功</returns>
        public static bool UpdateService()
        {
            BinaryWriter writer = new BinaryWriter(new MemoryStream());

            writer.Write((int)0);
            writer.Write((ushort)MessageType.BroadcastUpdate);

            lock (RevolvingManager.revolvingDictionary)
            {
                writer.Write((ushort)RevolvingManager.revolvingDictionary.Count);

                foreach (var pair in RevolvingManager.revolvingDictionary)
                {
                    writer.Write(pair.Key.GmAddress);
                    writer.Write(pair.Key.Id);
                    writer.Write((byte)pair.Value.Count);

                    foreach (var data in pair.Value)
                    {
                        writer.Write(data.IntervalSecond);
                        writer.Write(data.Text);
                    }
                }
            }

            return Network.Send(writer);
        }

        /// <summary>
        /// 添加走马灯
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="content">走马灯内容</param>
        /// <param name="interval">间隔时间</param>
        /// <returns>走马灯数据</returns>
        public static RevolvingData AddRevolving(Server server, string content, int interval)
        {
            List<RevolvingData> list;
            lock (RevolvingManager.revolvingDictionary)
            {
                if (!RevolvingManager.revolvingDictionary.TryGetValue(server, out list))
                {
                    RevolvingManager.revolvingDictionary.Add(server, list = new List<RevolvingData>());
                }
            }
            RevolvingData data = new RevolvingData(content, interval);
            list.Add(data);
            return data;
        }

        /// <summary>
        /// 复制走马灯
        /// </summary>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        public static void Copy(Server source, Server target)
        {
            if (source == null || target == null) { return; }

            List<RevolvingData> list;
            List<RevolvingData> targetList;

            lock (RevolvingManager.revolvingDictionary)
            {
                if (!RevolvingManager.revolvingDictionary.TryGetValue(source, out list)) { return; }

                if (RevolvingManager.revolvingDictionary.TryGetValue(target, out targetList))
                {
                    targetList.Clear();
                }
                else
                {
                    RevolvingManager.revolvingDictionary.Add(target, targetList = new List<RevolvingData>());
                }
            }

            foreach (var data in list)
            {
                targetList.Add(new RevolvingData(data.Text, data.IntervalSecond));
            }
        }

        /// <summary>
        /// 移除走马灯
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="index">索引</param>
        public static void RemoveRevolving(Server server, int index)
        {
            if (index < 0) { return; }

            List<RevolvingData> list;
            lock (RevolvingManager.revolvingDictionary)
            {
                if (!RevolvingManager.revolvingDictionary.TryGetValue(server, out list)) { return; }
            }

            if (index >= list.Count) { return; }

            list.RemoveAt(index);
        }

        /// <summary>
        /// 删除走马灯
        /// </summary>
        /// <param name="server">服务器</param>
        public static void ClearRevolving(Server server)
        {
            List<RevolvingData> list;
            lock (RevolvingManager.revolvingDictionary)
            {
                if (!RevolvingManager.revolvingDictionary.TryGetValue(server, out list)) { return; }
            }

            list.Clear();
        }

        /// <summary>
        /// 获取走马灯
        /// </summary>
        /// <param name="server">服务器</param>
        /// <returns>走马灯列表</returns>
        public static List<RevolvingData> GetRevolving(Server server)
        {
            List<RevolvingData> list;

            lock (RevolvingManager.revolvingDictionary)
            {
                RevolvingManager.revolvingDictionary.TryGetValue(server, out list);
            }

            return list;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)RevolvingManager.revolvingDictionary.Count);

                foreach (var pair in RevolvingManager.revolvingDictionary)
                {
                    writer.Write(pair.Key.Name);
                    writer.Write((byte)pair.Value.Count);

                    foreach (var data in pair.Value)
                    {
                        writer.Write(data.Text);
                        writer.Write(data.IntervalSecond);
                    }
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Revolving.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        private static void Load()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Revolving.bytes";

            if (!File.Exists(configBinaryFile)) { return; }

            using (FileStream stream = File.OpenRead(configBinaryFile))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < stream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    ushort count = reader.ReadUInt16();

                    for (int i = 0; i < count; ++i)
                    {
                        Server server = Server.GetServer(reader.ReadString());
                        byte noticeCount = reader.ReadByte();

                        for (int j = 0; j < noticeCount; ++j)
                        {
                            string text = reader.ReadString();
                            int interval = reader.ReadInt32();

                            if (server != null) { RevolvingManager.AddRevolving(server, text, interval); }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 上一次事件时间
        /// </summary>
        private static DateTime lastEventTime = DateTime.Now;

        /// <summary>
        /// 走马灯字典
        /// </summary>
        private static Dictionary<Server, List<RevolvingData>> revolvingDictionary = new Dictionary<Server, List<RevolvingData>>();
    }

    /// <summary>
    /// 走马灯数据
    /// </summary>
    class RevolvingData
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="text">走马灯文本</param>
        /// <param name="intervalMillisecond">间隔毫秒</param>
        public RevolvingData(string text, int intervalMillisecond)
        {
            this.Text = text;
            this.IntervalSecond = intervalMillisecond;
        }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns>字符串</returns>
        public override string ToString()
        {
            return string.Format("{0}\t({1}" + TableManager.GetGMTText(94) + ")", this.Text, this.IntervalSecond);
        }

        /// <summary>
        /// 走马灯文本
        /// </summary>
        public string Text;

        /// <summary>
        /// 间隔秒
        /// </summary>
        public int IntervalSecond;
    }

    /// <summary>
    /// 公告管理器
    /// </summary>
    static class NoticeManager
    {
        /// <summary>
        /// 载入
        /// </summary>
        public static void Load()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Notice.bytes";

            if (!File.Exists(configBinaryFile)) { return; }

            using (FileStream stream = File.OpenRead(configBinaryFile))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                for (int i = 0; i < stream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    ushort count = reader.ReadUInt16();

                    for (int i = 0; i < count; ++i)
                    {
                        Server server = Server.GetServer(reader.ReadString());
                        byte noticeCount = reader.ReadByte();

                        List<NoticeData> list = new List<NoticeData>();

                        for (int j = 0; j < noticeCount; ++j)
                        {
                            int id = reader.ReadInt32();
                            string title = reader.ReadString();
                            string content = reader.ReadString();

                            if (server != null) { list.Add(new NoticeData(id, title, content)); }
                        }

                        if (server != null)
                        {
                            NoticeManager.noticeDictionary.Add(server, new NoticeList(list));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)NoticeManager.noticeDictionary.Count);

                foreach (var pair in NoticeManager.noticeDictionary)
                {
                    writer.Write(pair.Key.Name);
                    writer.Write((byte)pair.Value.List.Count);

                    foreach (var data in pair.Value.List)
                    {
                        writer.Write(data.Id);
                        writer.Write(data.Title);
                        writer.Write(data.Content);
                    }
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/Notice.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="id">编号</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>是否添加</returns>
        public static bool AddNotice(Server server, int id, string title, string content)
        {
            NoticeList list;
            if (!NoticeManager.noticeDictionary.TryGetValue(server, out list))
            {
                NoticeManager.noticeDictionary.Add(server, list = new NoticeList());
            }

            return list.AddNotice(id, title, content);
        }

        /// <summary>
        /// 复制公告
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="source">源</param>
        /// <param name="target">目标</param>
        public static void Copy(string user, Server source, Server target)
        {
            if (source == null || target == null) { return; }

            NoticeList list;
            if (!NoticeManager.noticeDictionary.TryGetValue(source, out list)) { return; }

            NoticeList targetList;
            if (NoticeManager.noticeDictionary.TryGetValue(target, out targetList))
            {
                targetList.List.Clear();
            }
            else
            {
                NoticeManager.noticeDictionary.Add(target, targetList = new NoticeList());
            }

            foreach (var data in list.List)
            {
                targetList.AddNotice(data.Id, data.Title, data.Content);
                Notice.SendCommand(user, target, string.Format("ACN({0},{0},\"{2}\",{1},\"{3}\")", data.Id, data.Id + 10000, data.Title, data.Content), null);
            }
        }

        /// <summary>
        /// 移除公告
        /// </summary>
        /// <param name="server">服务器</param>
        /// <param name="id">编号</param>
        public static void RemoveNotice(Server server, int id)
        {
            NoticeList list;
            if (!NoticeManager.noticeDictionary.TryGetValue(server, out list)) { return; }

            list.Remove(id);
        }

        /// <summary>
        /// 获取公告
        /// </summary>
        /// <param name="server">服务器</param>
        /// <returns>公告列表</returns>
        public static NoticeList GetNotice(Server server)
        {
            NoticeList list;

            NoticeManager.noticeDictionary.TryGetValue(server, out list);

            return list;
        }

        /// <summary>
        /// 公告字典
        /// </summary>
        private static Dictionary<Server, NoticeList> noticeDictionary = new Dictionary<Server, NoticeList>();
    }

    /// <summary>
    /// 公告列表
    /// </summary>
    class NoticeList
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public NoticeList()
            : this(new List<NoticeData>())
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="increaseId">增长编号</param>
        /// <param name="list">公告列表</param>
        public NoticeList(List<NoticeData> list)
        {
            this.List = list;
        }

        /// <summary>
        /// 添加公告
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <returns>是否添加</returns>
        public bool AddNotice(int id, string title, string content)
        {
            lock (this)
            {
                NoticeData data = new NoticeData(id, title, content);

                for (int i = 0; i < this.List.Count; ++i)
                {
                    if (this.List[i].Id == id)
                    {
                        this.List[i] = data;
                        return false;
                    }
                }

                this.List.Add(data);
                return true;
            }
        }

        /// <summary>
        /// 移除公告
        /// </summary>
        /// <param name="id">编号</param>
        public void Remove(int id)
        {
            for (int i = 0; i < this.List.Count; ++i)
            {
                if (this.List[i].Id == id)
                {
                    this.List.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 公告列表
        /// </summary>
        public List<NoticeData> List;
    }

    /// <summary>
    /// 公告数据
    /// </summary>
    class NoticeData
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        public NoticeData(int id, string title, string content)
        {
            this.Id = id;
            this.Title = title;
            this.Content = content;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id;

        /// <summary>
        /// 标题
        /// </summary>
        public string Title;

        /// <summary>
        /// 内容
        /// </summary>
        public string Content;
    }
}