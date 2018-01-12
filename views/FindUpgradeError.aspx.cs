using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
	/// <summary>
	/// 查找新手引导错误界面
	/// </summary>
	public partial class FindUpgradeError : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				gm.Server server = Session["Server"] as gm.Server;

				for (int i = 0; i < gm.Server.Count; ++i)
				{
					gm.Server theServer = gm.Server.GetServerAt(i);
					this.serverList.Items.Add(theServer.Name);

					if (theServer == server)
					{
						this.serverList.SelectedIndex = i;
					}
				}

				if (server == null)
				{
					Session["Server"] = server = gm.Server.GetServerAt(0);
				}
			}
		}

		/// <summary>
		/// 服务器列表改变响应
		/// </summary>
		protected void serverList_SelectedIndexChanged(object sender, EventArgs e)
		{
			Session["Server"] = gm.Server.GetServerAt(this.serverList.SelectedIndex);
		}

		/// <summary>
		/// 查找按钮点击响应
		/// </summary>
		protected void findButton_Click(object sender, EventArgs e)
		{
			Server server = Session["Server"] as Server;

			if (server == null)
			{
				this.resultLabel.Text = "错误：无法获取服务器";
				return;
			}

			Dictionary<int, object[]> userDictionary = new Dictionary<int, object[]>();

			DatabaseAssistant.Execute
			(
				reader =>
				{
					while (reader.Read())
					{
						int		uid		= reader.GetInt32(0);
						int		level	= reader.GetInt32(1);

						if (level < 20) { continue; }

						byte[]	buffer	= new byte[reader.GetBytes(2, 0, null, 0, int.MaxValue)];

						reader.GetBytes(2, 0, buffer, 0, buffer.Length);

						using (MemoryStream stream = new MemoryStream(buffer))
						{
							try
							{
								mw.CardPackage cn = ProtoSerializer.Instance.Deserialize(stream, null, typeof(mw.CardPackage)) as mw.CardPackage;

								int type = cn.cardInfos[0].type;
								if (type >= 9000 && type <= 9004)
								{
									userDictionary.Add(uid, new object[] { reader.GetString(3), level });
								}
							}

							catch (Exception exception)
							{
								continue;
							}
						}
					}
				},
				server.DatabaseAddress,
				server.DatabasePort,
				server.DatabaseCharSet,
				server.GameDatabase,
				server.DatabaseUserId,
				server.DatabasePassword,
				"SELECT `user`.`uid`,`card_package`.`team_level`,`card_package`.`card_data`,`user`.`uname` FROM `user`,`card_package` WHERE `card_package`.`uid`=`user`.`uid`;"
			);

			if (userDictionary.Count == 0)
			{
				this.resultLabel.Text = "没有找到";
			}
			else
			{
				StringBuilder buider = new StringBuilder(string.Format("找到 {0} 个进阶卡错误玩家", userDictionary.Count));

				foreach (var pair in userDictionary)
				{
					buider.Append("<br>");
					buider.Append(pair.Key);
					buider.Append(" ");
					buider.Append(pair.Value[0]);
					buider.Append(" 等级：");
					buider.Append(pair.Value[1]);
				}

				this.resultLabel.Text = buider.ToString();
			}
		}
	}
}