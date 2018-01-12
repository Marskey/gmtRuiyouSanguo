using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
	/// <summary>
	/// 幸运抽奖
	/// </summary>
	public partial class LuckDraw : AGmPage
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public LuckDraw()
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
				this.versionTextBox.Text = GmNormal.Version;

				this.randTypeDropDownList.Items.Add(new ListItem("幸运值", ((int)mw.Enums.RandType.RAND_TYPE_LUCK).ToString()));
				this.randTypeDropDownList.Items.Add(new ListItem("幸运", ((int)mw.Enums.RandType.RAND_TYPE_XINGYUN).ToString()));
				this.randTypeDropDownList.Items.Add(new ListItem("豪华", ((int)mw.Enums.RandType.RAND_TYPE_HAOHUA).ToString()));
				this.randTypeDropDownList.Items.Add(new ListItem("至尊", ((int)mw.Enums.RandType.RAND_TYPE_ZHIZUN).ToString()));

				this.RefreshRandList();
			}

			this.errorLabel.Text = "";
		}

		/// <summary>
		/// 商店类型下拉列表改变响应
		/// </summary>
		protected void randTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RefreshRandList();
		}

		/// <summary>
		/// 商品类型下拉列表改变响应
		/// </summary>
		protected void rewardTypeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateRewordId();
		}

		/// <summary>
		/// 配置列表选中改变响应
		/// </summary>
		protected void configListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.configListBox.SelectedIndex < 0) { return; }

			mw.RandConfig config = RandTable.GetConfig(this.configListBox.SelectedIndex, (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue));

			if (config == null) { return; }

			for (int i = 0; i < this.rewardTypeDropDownList.Items.Count; ++i)
			{
				if (this.rewardTypeDropDownList.Items[i].Value == ((int)config.reward_idx).ToString())
				{
					this.rewardTypeDropDownList.SelectedIndex = i;
					break;
				}
			}

			this.UpdateRewordId();

			for (int i = 0; i < this.idDropDownList.Items.Count; ++i)
			{
				if (this.idDropDownList.Items[i].Value == config.reward_type.ToString())
				{
					this.idDropDownList.SelectedIndex = i;
					break;
				}
			}

			if (config.reward_count > 0)
			{
				this.minCountTextBox.Text = config.reward_count.ToString();
				this.countTextBox.Text = config.reward_count.ToString();
			}
			else
			{
				this.minCountTextBox.Text = config.reward_min_count.ToString();
				this.countTextBox.Text = config.reward_max_count.ToString();
			}

			if (config.rand_type != mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				this.counterIndexTextBox.Text = config.check_idx.ToString();
				this.counterValueTextBox.Text = config.limit_count.ToString();
			}

			this.limitTextBox.Text = config.limit_count.ToString();
			this.minTextBox.Text = config.min_rand.ToString();
			this.maxTextBox.Text = config.max_rand.ToString();
		}

		/// <summary>
		/// 添加按钮响应
		/// </summary>
		protected void addButton_Click(object sender, EventArgs e)
		{
			mw.Enums.RandType type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);
			if (this.configListBox.Items.Count >= LuckDraw.GetRandTypeLimit(type)) { return; }

			mw.RandConfig config = new mw.RandConfig();
			config.rand_type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);
			if (!this.UpdateConfig(config)) { return; }
			config.index = this.CreateIndex();

			RandTable.RandList.Add(config);
			TableManager.Save(RandTable.RandList);

			string text = this.GetConfigText(config);
			this.configListBox.Items.Add(new ListItem(text, text + this.configListBox.Items.Count));

			this.addButton.Enabled = this.configListBox.Items.Count < LuckDraw.GetRandTypeLimit(type);
		}

		/// <summary>
		/// 修改按钮响应
		/// </summary>
		protected void modifyButton_Click(object sender, EventArgs e)
		{
			if (this.configListBox.SelectedIndex < 0) { return; }

			mw.RandConfig config = RandTable.GetConfig(this.configListBox.SelectedIndex, (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue));

			if (!this.UpdateConfig(config)) { return; }

			TableManager.Save(RandTable.RandList);
			this.configListBox.SelectedItem.Text = this.GetConfigText(config);
			this.configListBox.SelectedItem.Value = this.configListBox.SelectedItem.Text + this.configListBox.SelectedIndex;
		}

		/// <summary>
		/// 发送按钮响应
		/// </summary>
		protected void sendButton_Click(object sender, EventArgs e)
		{
			for (int i = 0; i < this.randTypeDropDownList.Items.Count; ++i)
			{
				ListItem			item	= this.randTypeDropDownList.Items[i];
				mw.Enums.RandType	type	= (mw.Enums.RandType)int.Parse(item.Value);
				if (!this.CheckRepeat(type))
				{
					this.errorLabel.Text = item.Text + "分页奖励重复";
					return;
				}
				if (!this.CheckCount(type))
				{
					this.errorLabel.Text = item.Text + "分页数量不是" + LuckDraw.RandTypeLimitDictionary[type];
					return;
				}
				if (!this.CheckRate(type))
				{
					this.errorLabel.Text = item.Text + "分页随机区间错误";
					return;
				}
				if (type != mw.Enums.RandType.RAND_TYPE_LUCK)
				{
					if (!this.CheckCertainly(type))
					{
						this.errorLabel.Text = item.Text + "分页必出计数器重复";
						return;
					}
				}
			}

			List<mw.RandConfig> list = new List<mw.RandConfig>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
				{
					list.Add(config);
				}
			}

			this.errorLabel.Text = TableManager.Send(list, (int)mw.EGMTSettintType.E_GMT_SETTINT_RAND);
		}

		/// <summary>
		/// 直接发送所有表格按钮点击响应
		/// </summary>
		protected void sendFileButton_Click(object sender, EventArgs e)
		{
			if (ServerListConfig.DataList.Count == 0)
			{
				this.errorLabel.Text = "没有配置任何大区，不能发送表格";
				return;
			}

			if (TableManager.SendTable(this.versionTextBox.Text))
			{
				this.errorLabel.Text = "发送完成";
			}
			else
			{
				this.errorLabel.Text = "发送失败";
			}
		}

		/// <summary>
		/// 发送ZIP包按钮点击响应
		/// </summary>
		protected void sendButton0_Click(object sender, EventArgs e)
		{
			if (ServerListConfig.DataList.Count == 0)
			{
				this.errorLabel.Text = "没有配置任何大区，不能发送表格";
				return;
			}

			FTP ftp = new FTP();
			ftp.Upload("dtslz_updateex_" + ServerListConfig.DataList[0].Name + ".zip", CdnZip.PackTable(this.versionTextBox.Text));
		}

		/// <summary>
		/// 上传按钮点击响应
		/// </summary>
		protected void uploadButton_Click(object sender, EventArgs e)
		{
			if (this.rewardFileUpload.HasFile)
			{
				RandTable.RandList = TableManager.Unserialize<mw.RandConfig>(this.rewardFileUpload.FileBytes);

				this.RefreshRandList();

				this.errorLabel.Text = "上传完成";

				TableManager.Save(RandTable.RandList);
			}
			else
			{
				this.errorLabel.Text = "请选择上传的文件";
			}
		}

		/// <summary>
		/// 刷新商店列表
		/// </summary>
		private void RefreshRandList()
		{
			mw.Enums.RandType type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);
			
			this.rewardTypeDropDownList.Items.Clear();

			if (type == mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				this.rewardTypeDropDownList.Items.Add(new ListItem("碎片", ((int)mw.Enums.RewardType.RWD_TYPE_CHIP).ToString()));
				this.minCountTextBox.Visible = true;
				this.certainlyLabel.Visible = false;
				this.counterIndexTextBox.Visible = false;
				this.counterValueTextBox.Visible = false;
			}
			else
			{
				//this.rewardTypeDropDownList.Items.Add(new ListItem("经济", ((int)mw.Enums.RewardType.RWD_TYPE_ECONOMIC).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("物品", ((int)mw.Enums.RewardType.RWD_TYPE_ITEM).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("卡牌", ((int)mw.Enums.RewardType.RWD_TYPE_CARD).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("碎片", ((int)mw.Enums.RewardType.RWD_TYPE_CHIP).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("宠物", ((int)mw.Enums.RewardType.RWD_TYPE_PET).ToString()));
				this.minCountTextBox.Visible = false;
				this.certainlyLabel.Visible = true;
				this.counterIndexTextBox.Visible = true;
				this.counterValueTextBox.Visible = true;
			}

			if (type == mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
			{
				this.limitTextBox.Visible = true;
				this.minTextBox.Visible = false;
				this.maxTextBox.Visible = false;
				this.numberLabel.Text = "限制购买次数";
			}
			else
			{
				this.limitTextBox.Visible = false;
				this.minTextBox.Visible = true;
				this.maxTextBox.Visible = true;
				this.numberLabel.Text = "随机范围";
			}

			this.configListBox.Items.Clear();

			for (int i = 0; i < RandTable.RandList.Count;)
			{
				mw.RandConfig config = RandTable.RandList[i];

				if (config.rand_type != type)
				{
					++i;
					continue;
				}

				if (this.configListBox.Items.Count < LuckDraw.GetRandTypeLimit(type))
				{
					string text = this.GetConfigText(config);
					this.configListBox.Items.Add(new ListItem(text, text + this.configListBox.Items.Count));
					++i;
				}
				else
				{
					RandTable.RandList.RemoveAt(i);
				}
			}

			this.addButton.Enabled = this.configListBox.Items.Count < LuckDraw.GetRandTypeLimit(type);

			this.UpdateRewordId();
		}

		/// <summary>
		/// 更新商品编号
		/// </summary>
		private void UpdateRewordId()
		{
			this.idDropDownList.Items.Clear();

			switch ((mw.Enums.RewardType)int.Parse(this.rewardTypeDropDownList.SelectedValue))
			{
				case mw.Enums.RewardType.RWD_TYPE_ECONOMIC:
					this.idDropDownList.Items.Add(new ListItem("金钱", "2"));
					this.idDropDownList.Items.Add(new ListItem("元宝", "3"));
					break;

				case mw.Enums.RewardType.RWD_TYPE_ITEM:
					foreach (var pair in TableManager.ItemTable)
					{
						if (pair.Value.gmt_use == "1")
						{
							this.idDropDownList.Items.Add(new ListItem(TextManager.GetText(pair.Value.name), pair.Key.ToString()));
						}
					}
					break;

				case mw.Enums.RewardType.RWD_TYPE_CARD:
					foreach (var pair in TableManager.CardTable)
					{
						if (pair.Value.gmt_use1 == "1")
						{
							this.idDropDownList.Items.Add(new ListItem(TextManager.GetText(pair.Value.name), pair.Key.ToString()));
						}
					}
					break;

				case mw.Enums.RewardType.RWD_TYPE_CHIP:
					foreach (var pair in TableManager.CardTable)
					{
						if (pair.Value.gmt_use2 == "1")
						{
							this.idDropDownList.Items.Add(new ListItem(TextManager.GetText(pair.Value.name), pair.Key.ToString()));
						}
					}
					break;

				case mw.Enums.RewardType.RWD_TYPE_PET:
					foreach (var pair in TableManager.PetTable)
					{
						this.idDropDownList.Items.Add(new ListItem(TextManager.GetText(pair.Value.name), pair.Key.ToString()));
					}
					break;
			}
		}

		/// <summary>
		/// 更新配置
		/// </summary>
		/// <param name="config">配置</param>
		/// <returns>是否成功</returns>
		private bool UpdateConfig(mw.RandConfig config)
		{
			int intValue;

			config.reward_idx = (mw.Enums.RewardType)int.Parse(this.rewardTypeDropDownList.SelectedValue);
			config.reward_type = int.Parse(this.idDropDownList.SelectedItem.Value);

			if (!int.TryParse(this.countTextBox.Text, out intValue) || intValue <= 0)
			{
				this.errorLabel.Text = "数量输入错误";
				return false;
			}

			switch (config.reward_idx)
			{
				case mw.Enums.RewardType.RWD_TYPE_ITEM:
				case mw.Enums.RewardType.RWD_TYPE_CHIP:
					if (intValue > 99)
					{
						this.errorLabel.Text = "数量不能大于99";
						return false;
					}
					break;

				case mw.Enums.RewardType.RWD_TYPE_CARD:
				case mw.Enums.RewardType.RWD_TYPE_PET:
					if (intValue > 1)
					{
						this.errorLabel.Text = "数量不能大于1";
						return false;
					}
					break;
			}

			if (config.rand_type == mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				config.reward_max_count = intValue;

				if (!int.TryParse(this.minCountTextBox.Text, out intValue) ||
					intValue <= 0 ||
					intValue > config.reward_max_count)
				{
					this.errorLabel.Text = "数量输入错误";
					return false;
				}

				if (config.reward_max_count == intValue)
				{
					config.reward_count = intValue;
					config.reward_min_count = 0;
					config.reward_max_count = 0;
				}
				else
				{
					config.reward_count = 0;
					config.reward_min_count = intValue;
				}
			}
			else
			{
				config.reward_count = intValue;
				config.reward_min_count = 0;
				config.reward_max_count = 0;
			}

			config.cost_type = 0;
			config.cost_val = 0;

			if (!int.TryParse(this.minTextBox.Text, out intValue) || intValue < 0)
			{
				this.errorLabel.Text = "最小随机输入错误";
				return false;
			}
			config.min_rand = intValue;
			if (!int.TryParse(this.maxTextBox.Text, out intValue) || intValue < 0)
			{
				this.errorLabel.Text = "最大随机输入错误";
				return false;
			}

			config.max_rand = intValue;
			config.limit_count = 0;

			if (config.max_rand <= config.min_rand)
			{
				this.errorLabel.Text = "随机输入错误";
				return false;
			}

			if (config.rand_type != mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				if (!int.TryParse(this.counterIndexTextBox.Text, out intValue) || intValue < 0 || intValue > 5)
				{
					this.errorLabel.Text = "必抽计数器必需为 0~6";
					return false;
				}
				config.check_idx = intValue;

				if (!int.TryParse(this.counterValueTextBox.Text, out intValue) || intValue < 0)
				{
					this.errorLabel.Text = "必抽数量输入错误";
					return false;
				}
				config.limit_count = intValue;
			}

			return true;
		}

		/// <summary>
		/// 创建索引
		/// </summary>
		/// <returns>索引</returns>
		private int CreateIndex()
		{
			mw.Enums.RandType type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);

			if (type == mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				return 0;
			}

			int index = 0;
			switch (type)
			{
				case mw.Enums.RandType.RAND_TYPE_XINGYUN:
					index = 68;
					break;

				case mw.Enums.RandType.RAND_TYPE_HAOHUA:
					index = 74;
					break;

				case mw.Enums.RandType.RAND_TYPE_ZHIZUN:
					index = 80;
					break;
			}

			for (int i = 0; i < LuckDraw.RandTypeLimitDictionary[type]; ++i)
			{
				bool isFind = false;

				foreach (var config in RandTable.RandList)
				{
					if (config.rand_type == type && config.index == index + i)
					{
						isFind = true;
						break;
					}
				}

				if (!isFind) { return index + i; }
			}

			return 0;
		}

		/// <summary>
		/// 检查重复
		/// </summary>
		/// <param name="type">抽奖类型</param>
		/// <returns>是否正确</returns>
		private bool CheckRepeat(mw.Enums.RandType type)
		{
			List<int[]> itemList = new List<int[]>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != type) { continue; }

				itemList.Add(new int[] { (int)config.reward_idx, config.reward_type });
			}

			for (int i = 0; i < itemList.Count; ++i)
			{
				for (int j = i + 1; j < itemList.Count; ++j)
				{
					if (itemList[i][0] == itemList[j][0] &&
						itemList[i][1] == itemList[j][1])
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// 检查数量
		/// </summary>
		/// <param name="type">抽奖类型</param>
		/// <returns>是否正确</returns>
		private bool CheckCount(mw.Enums.RandType type)
		{
			int count = 0;
			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != type) { continue; }

				++count;
			}

			return LuckDraw.RandTypeLimitDictionary[type] == count;
		}

		/// <summary>
		/// 检查几率
		/// </summary>
		/// <param name="type">抽奖类型</param>
		/// <returns>是否正确</returns>
		private bool CheckRate(mw.Enums.RandType type)
		{
			List<int[]> rateList = new List<int[]>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != type) { continue; }

				rateList.Add(new int[] { config.min_rand, config.max_rand });
			}

			if (rateList.Count == 0) { return false; }

			while (rateList.Count > 1)
			{
				int count = rateList.Count;
				for (int i = 1; i < rateList.Count; ++i)
				{
					if (rateList[0][0] == rateList[i][1] + 1)
					{
						rateList[0][0] = rateList[i][0];
						rateList.RemoveAt(i);
						break;
					}
					else if (rateList[0][1] + 1 == rateList[i][0])
					{
						rateList[0][1] = rateList[i][1];
						rateList.RemoveAt(i);
						break;
					}
				}

				if (count == rateList.Count) { return false; }
			}

			return rateList[0][0] == 1 && rateList[0][1] == 10000;
		}

		/// <summary>
		/// 检查必出
		/// </summary>
		/// <param name="type">抽奖类型</param>
		/// <returns>是否正确</returns>
		private bool CheckCertainly(mw.Enums.RandType type)
		{
			List<int> itemList = new List<int>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != type || config.limit_count == 0) { continue; }

				itemList.Add(config.check_idx);
			}

			for (int i = 0; i < itemList.Count; ++i)
			{
				for (int j = i + 1; j < itemList.Count; ++j)
				{
					if (itemList[i] == itemList[j])
					{
						return false;
					}
				}
			}

			return true;
		}

		/// <summary>
		/// 获取配置的文本
		/// </summary>
		/// <param name="config">配置</param>
		/// <returns>文本</returns>
		private string GetConfigText(mw.RandConfig config)
		{
			StringBuilder builder = new StringBuilder();

			if (config.rand_type != mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				builder.Append(config.index.ToString());
				builder.Append(" | ");
			}

			for (int i = 0; i < this.rewardTypeDropDownList.Items.Count; ++i)
			{
				ListItem item = this.rewardTypeDropDownList.Items[i];
				if (item.Value == ((int)config.reward_idx).ToString())
				{
					builder.Append(item.Text);
					break;
				}
			}
			builder.Append(" | ");

			switch (config.reward_idx)
			{
				case mw.Enums.RewardType.RWD_TYPE_ECONOMIC:
					if (config.reward_type == 2)
					{
						builder.Append("金钱");
					}
					else if (config.reward_type == 3)
					{
						builder.Append("元宝");
					}
					break;

				case mw.Enums.RewardType.RWD_TYPE_ITEM:
					builder.Append(TableManager.ItemTable[config.reward_type].name);
					break;

				case mw.Enums.RewardType.RWD_TYPE_CARD:
				case mw.Enums.RewardType.RWD_TYPE_CHIP:
					builder.Append(TableManager.CardTable[config.reward_type].name);
					break;

				case mw.Enums.RewardType.RWD_TYPE_PET:
					builder.Append(TableManager.PetTable[config.reward_type].name);
					break;
			}

			builder.Append(" ");

			if (config.reward_count > 0)
			{
				builder.Append(config.reward_count);
			}
			else
			{
				builder.Append(config.reward_min_count).Append("~").Append(config.reward_max_count);
			}

			builder.Append(" | ").Append(config.min_rand).Append("~").Append(config.max_rand);
			builder.Append(" | ").Append((config.max_rand - config.min_rand + 1) / 100f).Append("%");

			if (config.rand_type != mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				builder.Append(" | ").Append(config.check_idx).Append(" ").Append(config.limit_count);
			}

			return builder.ToString();
		}

		/// <summary>
		/// 获取商城类型数量限制
		/// </summary>
		/// <param name="type">商城类型</param>
		/// <returns>商城类型数量限制</returns>
		private static int GetRandTypeLimit(mw.Enums.RandType type)
		{
			int limit;
			if (LuckDraw.RandTypeLimitDictionary.TryGetValue(type, out limit))
			{
				return limit;
			}
			else
			{
				return int.MaxValue;
			}
		}

		/// <summary>
		/// 静态构造
		/// </summary>
		static LuckDraw()
		{
			LuckDraw.RandTypeLimitDictionary = new Dictionary<mw.Enums.RandType, int>()
			{
				{ mw.Enums.RandType.RAND_TYPE_LUCK,		8 },
				{ mw.Enums.RandType.RAND_TYPE_XINGYUN,	6 },
				{ mw.Enums.RandType.RAND_TYPE_HAOHUA,	6 },
				{ mw.Enums.RandType.RAND_TYPE_ZHIZUN,	6 },
			};
		}

		/// <summary>
		/// 商城类型数量限制字典
		/// </summary>
		private static Dictionary<mw.Enums.RandType, int> RandTypeLimitDictionary;
	}
}