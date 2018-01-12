using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
	/// <summary>
	/// 区服列表
	/// </summary>
	public partial class SectionServerList : AGmPage
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public SectionServerList()
			: base(PrivilegeType.Modify)
		{
		}

		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected override void OnGmPageLoad()
		{
			if (!this.IsPostBack)
			{
                this.returnGmModifyTipLabel.Text = TableManager.GetGMTText(187);
                this.titleTipLabel.InnerText = TableManager.GetGMTText(199);
                this.bianjifuwuqi.Text = TableManager.GetGMTText(200);
                this.daqu.Text = TableManager.GetGMTText(201);
                this.qudao.Text = TableManager.GetGMTText(202);
                this.addButton.Text = TableManager.GetGMTText(174);
                this.modifyButton.Text = TableManager.GetGMTText(175);
                this.deleteButton.Text = TableManager.GetGMTText(176);

				for (int i = 0; i < ServerListConfig.DataList.Count; ++i)
				{
					string name = ServerListConfig.DataList[i].Name;
					this.channelListBox.Items.Add(new ListItem(name, name));
				}

				if (this.channelListBox.Items.Count > 0)
				{
					this.channelListBox.SelectedIndex = 0;
					this.UpdateCurrentServerList();
				}
			}
		}

		/// <summary>
		/// 渠道列表选择改变响应
		/// </summary>
		//protected void channelListBox_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	this.UpdateCurrentServerList();
		//}

		/// <summary>
		/// 添加按钮点击响应
		/// </summary>
		protected void addButton_Click(object sender, EventArgs e)
		{
			ServerListConfigData data = new ServerListConfigData();
			this.UpdateData(data);
			ServerListConfig.Add(data);
			this.channelListBox.Items.Add(new ListItem(data.Name, data.Name));
            this.channelListBox.SelectedIndex = channelListBox.Items.Count - 1;
		}

		/// <summary>
		/// 修改按钮点击响应
		/// </summary>
		protected void modifyButton_Click(object sender, EventArgs e)
		{
			if (this.channelListBox.SelectedIndex < 0) { return; }

			ServerListConfigData data = ServerListConfig.GetData(this.channelListBox.SelectedIndex);
			this.UpdateData(data);
			ServerListConfig.Modify(this.channelListBox.SelectedIndex, data);
			this.channelListBox.Items[this.channelListBox.SelectedIndex].Text = data.Name;
		}

		/// <summary>
		/// 删除按钮点击响应
		/// </summary>
		protected void deleteButton_Click(object sender, EventArgs e)
		{
			if (this.channelListBox.SelectedIndex < 0) { return; }

            ServerListConfig.Delete(this.channelListBox.SelectedIndex);
            channelListBox.Items.RemoveAt(this.channelListBox.SelectedIndex);
		}
		
		/// <summary>
		/// 更新当前显示的服务器列表
		/// </summary>
		private void UpdateCurrentServerList()
		{
			if (this.channelListBox.SelectedIndex >= 0)
			{
				ServerListConfigData data = ServerListConfig.DataList[this.channelListBox.SelectedIndex];
				this.nameTextBox.Text = data.Name;
				this.channelTextBox.Text = "";
				foreach (var channel in data.ChannelList)
				{
					if (!string.IsNullOrEmpty(this.channelTextBox.Text))
					{
						this.channelTextBox.Text += "\r\n";
					}
					this.channelTextBox.Text += channel;
				}
			}
			else
			{
				this.nameTextBox.Text = "";
				this.channelTextBox.Text = "";
			}
		}

		/// <summary>
		/// 更新服务器列表配置数据
		/// </summary>
		/// <param name="data">服务器列表配置数据</param>
		private void UpdateData(ServerListConfigData data)
		{
			data.Name = this.nameTextBox.Text;
			data.ChannelList.Clear();
			string[] lineSet = this.channelTextBox.Text.Replace("\r\n", "\n").Split('\n');

			foreach (var line in lineSet)
			{
				if (!string.IsNullOrEmpty(line))
				{
					data.ChannelList.Add(line);
				}
			}
		}

        protected void channelListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
		    this.UpdateCurrentServerList();
        }
	}
}