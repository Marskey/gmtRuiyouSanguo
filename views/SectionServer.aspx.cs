using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 区服
    /// </summary>
    public partial class SectionServer : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public SectionServer()
            : base(PrivilegeType.SectionServer)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                this.versionTextBox.Text = GmNormal.Version;

                this.statusDropDownList.Items.Add(TableManager.GetGMTText(341));
                this.statusDropDownList.Items.Add(TableManager.GetGMTText(342));
                this.statusDropDownList.Items.Add(TableManager.GetGMTText(343));
                this.statusDropDownList.Items.Add(TableManager.GetGMTText(344));

                for (int i = 0; i < ServerListConfig.DataList.Count; ++i)
                {
                    string name = ServerListConfig.DataList[i].Name;
                    this.channelListBox.Items.Add(new ListItem(name, name));
                }

                if (this.channelListBox.Items.Count > 0)
                {
                    this.channelListBox.SelectedIndex = 0;
                }

                this.UpdateServerList();
            }

            this.outputLabel.Text = "";
        }

        /// <summary>
        /// 渠道列表选择改变响应
        /// </summary>
        protected void channelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.UpdateServerList();
        }

        /// <summary>
        /// 服务器列表选择改变响应
        /// </summary>
        protected void serverListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.serverListBox.SelectedIndex >= 0)
            {
                ServerConfig config = this.CurrentServerList.GetServerConfig(this.serverListBox.SelectedIndex);

                string[] valueSet = config.Text.Substring(0, config.Text.Length - 1).Split(',');

                this.indexTextBox.Text = valueSet[0];
                this.nameTextBox.Text = valueSet[1];
                this.ipTextBox.Text = valueSet[2];
                this.portTextBox.Text = valueSet[3];
                this.statusDropDownList.SelectedIndex = int.Parse(valueSet[4]);
                this.recommendCheckBox.Checked = valueSet[5] == "1";
                this.regionTextBox.Text = valueSet[6];
                this.idTextBox.Text = valueSet[7];
                if (valueSet.Length >= 9)
                {
                    this.paramTextBox.Text = valueSet[8];
                }

                this.visibleCheckBox.Checked = config.Visible;
            }
            else
            {
                this.indexTextBox.Text = "";
                this.nameTextBox.Text = "";
                this.ipTextBox.Text = "";
                this.portTextBox.Text = "";
                this.statusDropDownList.SelectedIndex = 0;
                this.recommendCheckBox.Checked = false;
                this.regionTextBox.Text = "";
                this.idTextBox.Text = "";
                this.paramTextBox.Text = "";
                this.visibleCheckBox.Checked = false;
            }
        }

        /// <summary>
        /// 添加按钮点击响应
        /// </summary>
        protected void addButton_Click(object sender, EventArgs e)
        {
            if (this.CurrentServerList != null)
            {
                string config = this.GetConfig();
                this.CurrentServerList.Add(config, this.visibleCheckBox.Checked);
                if (!this.visibleCheckBox.Checked) { config += ("," + TableManager.GetGMTText(400) + ""); }
                ServerListConfig.Save();
                this.serverListBox.Items.Add(new ListItem(config, this.idTextBox.Text));
            }
        }

        /// <summary>
        /// 修改按钮点击响应
        /// </summary>
        protected void modifyButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.SelectedIndex < 0) { return; }

            if (this.CurrentServerList != null)
            {
                string config = this.GetConfig();
                this.CurrentServerList.Modify(config, this.visibleCheckBox.Checked, this.serverListBox.SelectedIndex);
                if (!this.visibleCheckBox.Checked) { config += ("," + TableManager.GetGMTText(400) + ""); }
                this.serverListBox.Items[this.serverListBox.SelectedIndex].Text = config;
            }
        }

        /// <summary>
        /// 删除按钮点击响应
        /// </summary>
        protected void deleteButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.SelectedIndex < 0) { return; }

            if (this.CurrentServerList != null)
            {
                int index = this.serverListBox.SelectedIndex;
                this.CurrentServerList.Delete(index);
                this.serverListBox.Items.RemoveAt(index);
                if (index < this.serverListBox.Items.Count)
                {
                    this.serverListBox.SelectedIndex = index;
                }
                else
                {
                    this.serverListBox.SelectedIndex = this.serverListBox.Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// 上移按钮点击响应
        /// </summary>
        protected void upButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.SelectedIndex <= 0) { return; }

            if (this.CurrentServerList != null)
            {
                int index = this.serverListBox.SelectedIndex;
                this.CurrentServerList.MoveUp(index);
                ListItem item = this.serverListBox.Items[index];
                this.serverListBox.Items.RemoveAt(index);
                this.serverListBox.Items.Insert(index - 1, item);
                this.serverListBox.SelectedIndex = index - 1;
            }
        }

        /// <summary>
        /// 下移按钮点击响应
        /// </summary>
        protected void downButton_Click(object sender, EventArgs e)
        {
            if (this.serverListBox.SelectedIndex < 0) { return; }

            if (this.CurrentServerList != null &&
                this.serverListBox.SelectedIndex < this.CurrentServerList.Count - 1)
            {
                int index = this.serverListBox.SelectedIndex;
                this.CurrentServerList.MoveDown(index);
                ListItem item = this.serverListBox.Items[index];
                this.serverListBox.Items.RemoveAt(index);
                this.serverListBox.Items.Insert(index + 1, item);
                this.serverListBox.SelectedIndex = index + 1;
            }
        }

        /// <summary>
        /// 直接发送问按钮点击响应
        /// </summary>
        protected void sendFileButton_Click(object sender, EventArgs e)
        {
            string version = this.versionTextBox.Text;

            // 服务器列表
            if (!string.IsNullOrEmpty(version) && version[version.Length - 1] != '/')
            {
                version = version + '/';
                FTPManager.MakeDirectory(version);
            }

            foreach (var data in ServerListConfig.DataList)
            {
                foreach (var channel in data.ChannelList)
                {
                    string text = string.Format("#{0} #{1} #{2} #{3} #{4} #{5} #{6} #{7} #{8};\r\n"
                        , TableManager.GetGMTText(203)
                        , TableManager.GetGMTText(204)
                        , TableManager.GetGMTText(205)
                        , TableManager.GetGMTText(206)
                        , TableManager.GetGMTText(207)
                        , TableManager.GetGMTText(401)
                        , TableManager.GetGMTText(402)
                        , TableManager.GetGMTText(403)
                        , TableManager.GetGMTText(910))
                         + data.ServerList.GetText()
                         + data.Name;

                    if (!FTPManager.Upload(version + "ServerList." + channel + ".txt", Encoding.UTF8.GetBytes(text)))
                    {
                        this.outputLabel.Text = TableManager.GetGMTText(404);
                        return;
                    }
                }
            }

            this.outputLabel.Text = TableManager.GetGMTText(405);
        }

        /// <summary>
        /// 发送按钮点击响应
        /// </summary>
        protected void sendButton_Click(object sender, EventArgs e)
        {
            /*if (string.IsNullOrEmpty(FTP.Site))
            {
                this.outputLabel.Text = "FTP还没有配置";
                return;
            }

            FTP ftp = new FTP();

            string version;
            if (string.IsNullOrEmpty(this.versionTextBox.Text))
            {
                version = "";
            }
            else
            {
                version = this.versionTextBox.Text + "/";
                ftp.MakeDirectory(this.versionTextBox.Text);
            }

            StringBuilder builder = new StringBuilder();
            foreach (var data in ServerListConfig.DataList)
            {
                foreach (var channel in data.ChannelList)
                {
                    builder.Append("channel:");
                    string text = "#编号 #服务器名 #IP # 端口 #状态 #是否推荐 #大区ID #服务器名称ID;\r\n" + data.ServerList.GetText();
                    if (ftp.Upload(version + "ServerList." + channel + ".txt", Encoding.UTF8.GetBytes(text)))
                    {
                        builder.Append("成功<br>");
                    }
                    else
                    {
                        builder.Append("失败<br>");
                    }
                }
            }

            this.outputLabel.Text = builder.ToString();*/

            if (FTPManager.Upload("ServerList_" + ServerListConfig.DataList[0].Name + ".zip", CdnZip.PackServerList(this.versionTextBox.Text)))
            {
                this.outputLabel.Text = TableManager.GetGMTText(405);
            }
            else
            {
                this.outputLabel.Text = TableManager.GetGMTText(404);
            }
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <returns>服务器配置</returns>
        private string GetConfig()
        {
            return string.Format
            (
                "{0},{1},{2},{3},{4},{5},{6},{7},{8};",
                this.indexTextBox.Text,
                this.nameTextBox.Text,
                this.ipTextBox.Text,
                this.portTextBox.Text,
                this.statusDropDownList.SelectedIndex,
                this.recommendCheckBox.Checked ? 1 : 0,
                this.regionTextBox.Text,
                this.idTextBox.Text,
                this.paramTextBox.Text
            );
        }

        /// <summary>
        /// 更新服务器列表
        /// </summary>
        private void UpdateServerList()
        {
            ServerList list = this.CurrentServerList;

            this.serverListBox.Items.Clear();
            if (list != null)
            {
                for (int i = 0; i < list.Count; ++i)
                {
                    ServerConfig config = list.GetServerConfig(i);
                    string text = config.Text;
                    if (!config.Visible) { text += ("," + TableManager.GetGMTText(400) + ""); }
                    this.serverListBox.Items.Add(text);
                }
            }
        }

        /// <summary>
        /// 当前服务器列表
        /// </summary>
        private ServerList CurrentServerList
        {
            get
            {
                if (this.channelListBox.SelectedIndex >= 0 &&
                    this.channelListBox.SelectedIndex < ServerListConfig.DataList.Count)
                {
                    return ServerListConfig.DataList[this.channelListBox.SelectedIndex].ServerList;
                }
                else
                {
                    return null;
                }
            }
        }
    }

    /// <summary>
    /// 服务器列表
    /// </summary>
    class ServerList
    {
        /// <summary>
        /// 添加服务器
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="visible">是否可见</param>
        public void Add(string text, bool visible)
        {
            this.configList.Add(new ServerConfig(text, visible));
        }

        /// <summary>
        /// 修改服务器
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="visible">是否可见</param>
        /// <param name="index">索引</param>
        public void Modify(string text, bool visible, int index)
        {
            if (index >= 0 && index < this.configList.Count)
            {
                ServerConfig config = this.configList[index];
                config.Text = text;
                config.Visible = visible;
                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="index">索引</param>
        public void Delete(int index)
        {
            if (index >= 0 && index < this.configList.Count)
            {
                this.configList.RemoveAt(index);
                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="index">索引</param>
        public void MoveUp(int index)
        {
            if (index > 0 && index < this.configList.Count)
            {
                ServerConfig config = this.configList[index];

                this.configList.RemoveAt(index);
                this.configList.Insert(index - 1, config);

                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="index">索引</param>
        public void MoveDown(int index)
        {
            if (index >= 0 && index < this.configList.Count - 1)
            {
                ServerConfig config = this.configList[index];

                this.configList.RemoveAt(index);
                this.configList.Insert(index + 1, config);

                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 获取服务器
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>服务器配置</returns>
        public ServerConfig GetServerConfig(int index)
        {
            if (index >= 0 && index < this.configList.Count)
            {
                return this.configList[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置文本
        /// </summary>
        /// <param name="text">文本</param>
        public void SetText(string text)
        {
            //TODO
            if (!string.IsNullOrEmpty(text))
            {
                string[] lineSet = text.Replace("\r\n", "\n").Split('\n');
                foreach (var line in lineSet)
                {
                    this.configList.Add(new ServerConfig(line, false));
                }
            }
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns>文本</returns>
        public string GetText()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < this.configList.Count; ++i)
            {
                ServerConfig config = this.configList[i];

                if (!config.Visible) { continue; }

                if (i > 0) { builder.Append("\r\n"); }

                builder.Append(config.Text);
            }

            return builder.ToString();
        }

        /// <summary>
        /// 服务器配置数量
        /// </summary>
        public int Count { get { return this.configList.Count; } }

        /// <summary>
        /// 服务器配置列表
        /// </summary>
        private List<ServerConfig> configList = new List<ServerConfig>();
    }

    /// <summary>
    /// 服务器配置
    /// </summary>
    class ServerConfig
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="visible">是否可见</param>
        public ServerConfig(string text, bool visible)
        {
            this.Text = text;
            this.Visible = visible;
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text;

        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible;
    }

    /// <summary>
    /// 服务器列表配置
    /// </summary>
    static class ServerListConfig
    {
        /// <summary>
        /// 添加服务器列表配置数据
        /// </summary>
        /// <param name="data">服务器列表配置数据</param>
        public static void Add(ServerListConfigData data)
        {
            ServerListConfig.DataList.Add(data);
            ServerListConfig.Save();
        }

        /// <summary>
        /// 修改服务器列表配置数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="data">服务器列表配置数据</param>
        public static void Modify(int index, ServerListConfigData data)
        {
            if (index >= 0 && index < ServerListConfig.DataList.Count)
            {
                ServerListConfig.DataList[index] = data;
                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 删除服务器列表配置数据
        /// </summary>
        /// <param name="index">索引</param>
        public static void Delete(int index)
        {
            if (index >= 0 && index < ServerListConfig.DataList.Count)
            {
                ServerListConfig.DataList.RemoveAt(index);
                ServerListConfig.Save();
            }
        }

        /// <summary>
        /// 获取服务器列表配置数据
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>服务器列表配置数据</returns>
        public static ServerListConfigData GetData(int index)
        {
            if (index >= 0 && index < ServerListConfig.DataList.Count)
            {
                return ServerListConfig.DataList[index];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)ServerListConfig.DataList.Count);

                foreach (var data in ServerListConfig.DataList)
                {
                    writer.Write(data.Name);
                    writer.Write((byte)data.ChannelList.Count);

                    foreach (var channel in data.ChannelList)
                    {
                        writer.Write(channel);
                    }

                    writer.Write((byte)data.ServerList.Count);

                    for (int i = 0; i < data.ServerList.Count; ++i)
                    {
                        ServerConfig config = data.ServerList.GetServerConfig(i);
                        writer.Write(config.Text);
                        writer.Write(config.Visible);
                    }
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/ServerList.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }
        }

        /// <summary>
        /// 载入
        /// </summary>
        public static void Load()
        {
            string configBinaryFile = HttpRuntime.AppDomainAppPath + "configs/ServerList.bytes";

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
                        ServerListConfigData data = new ServerListConfigData();

                        data.Name = reader.ReadString();

                        byte channelCount = reader.ReadByte();
                        for (int j = 0; j < channelCount; ++j)
                        {
                            data.ChannelList.Add(reader.ReadString());
                        }

                        byte serverCount = reader.ReadByte();
                        for (int j = 0; j < serverCount; ++j)
                        {
                            data.ServerList.Add(reader.ReadString(), reader.ReadBoolean());
                        }

                        ServerListConfig.DataList.Add(data);
                    }
                }
            }
        }

        /// <summary>
        /// 服务器列表配置数据列表
        /// </summary>
        public static List<ServerListConfigData> DataList = new List<ServerListConfigData>();
    }

    /// <summary>
    /// 服务器列表配置数据
    /// </summary>
    class ServerListConfigData
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 渠道列表
        /// </summary>
        public List<string> ChannelList = new List<string>();

        /// <summary>
        /// 服务器列表
        /// </summary>
        public ServerList ServerList = new ServerList();
    }

}