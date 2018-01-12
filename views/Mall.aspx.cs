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
	/// 商城
	/// </summary>
	public partial class Mall : AGmPage
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public Mall()
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
				this.randTypeDropDownList.Items.Add(new ListItem("元宝活动", ((int)mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY).ToString()));

				this.rewardTypeDropDownList.Items.Add(new ListItem("物品", ((int)mw.Enums.RewardType.RWD_TYPE_ITEM).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("卡牌", ((int)mw.Enums.RewardType.RWD_TYPE_CARD).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("碎片", ((int)mw.Enums.RewardType.RWD_TYPE_CHIP).ToString()));
				this.rewardTypeDropDownList.Items.Add(new ListItem("宠物", ((int)mw.Enums.RewardType.RWD_TYPE_PET).ToString()));

				this.costDropDownList.Items.Add(new ListItem("金钱", "2"));
				this.costDropDownList.Items.Add(new ListItem("元宝", "3"));
				this.costDropDownList.Items.Add(new ListItem("荣誉点", "9"));
				this.costDropDownList.Items.Add(new ListItem("帮贡", "17"));

				this.UpdateRewordId();
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

			this.indexTextBox.Text = config.index.ToString();

			for (int i = 0; i < this.idDropDownList.Items.Count; ++i)
			{
				if (this.idDropDownList.Items[i].Value == config.reward_type.ToString())
				{
					this.idDropDownList.SelectedIndex = i;
					break;
				}
			}

			this.countTextBox.Text = config.reward_count.ToString();
			this.costTextBox.Text = config.cost_val.ToString();

			for (int i = 0; i < this.costDropDownList.Items.Count; ++i)
			{
				if (this.costDropDownList.Items[i].Value == config.cost_type.ToString())
				{
					this.costDropDownList.SelectedIndex = i;
					break;
				}
			}

			this.limitTextBox.Text = config.limit_count.ToString();
			this.minTextBox.Text = config.min_rand.ToString();
			this.maxTextBox.Text = config.max_rand.ToString();

			this.groupTextBox.Text = config.check_idx.ToString();
		}

		/// <summary>
		/// 添加按钮响应
		/// </summary>
		protected void addButton_Click(object sender, EventArgs e)
		{
			mw.RandConfig config = new mw.RandConfig();
			config.rand_type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);
			if (!this.UpdateConfig(config)) { return; }

			RandTable.RandList.Add(config);
			TableManager.Save(RandTable.RandList);

			string text = this.GetConfigText(config);
			this.configListBox.Items.Add(new ListItem(text, text + this.configListBox.Items.Count));

			this.addButton.Enabled = this.configListBox.Items.Count < Mall.GetRandTypeLimit(config.rand_type);
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
		/// 删除按钮响应
		/// </summary>
		protected void deleteButton_Click(object sender, EventArgs e)
		{
			if (this.configListBox.SelectedIndex < 0) { return; }

			mw.Enums.RandType type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);
			RandTable.RandList.RemoveAt(RandTable.GetRealIndex(this.configListBox.SelectedIndex, type));
			this.configListBox.Items.RemoveAt(this.configListBox.SelectedIndex);

			TableManager.Save(RandTable.RandList);

			this.addButton.Enabled = this.configListBox.Items.Count < Mall.GetRandTypeLimit(type);
		}

		/// <summary>
		/// 发送按钮响应
		/// </summary>
		protected void sendButton_Click(object sender, EventArgs e)
		{
			if (!this.CheckIndexRepeat())
			{
				this.errorLabel.Text = "索引重复";
				return;
			}

			List<mw.RandConfig> list = new List<mw.RandConfig>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type == mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
				{
					list.Add(config);
				}
			}

			this.errorLabel.Text = TableManager.Send(list, (int)mw.EGMTSettintType.E_GMT_SETTINT_ACTIVITY_SHOP);
		}

		/// <summary>
		/// 刷新商店列表
		/// </summary>
		private void RefreshRandList()
		{
			mw.Enums.RandType type = (mw.Enums.RandType)int.Parse(this.randTypeDropDownList.SelectedValue);

			if (type == mw.Enums.RandType.RAND_TYPE_LUCK)
			{
				this.costLabel.Visible = false;
				this.costTextBox.Visible = false;
				this.costDropDownList.Visible = false;
			}
			else
			{
				this.costLabel.Visible = true;
				this.costTextBox.Visible = true;
				this.costDropDownList.Visible = true;
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

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != type) { continue; }

				string text = this.GetConfigText(config);
				this.configListBox.Items.Add(new ListItem(text, text + this.configListBox.Items.Count.ToString()));
			}

			this.addButton.Enabled = this.configListBox.Items.Count < Mall.GetRandTypeLimit(type);
		}

		/// <summary>
		/// 更新商品编号
		/// </summary>
		private void UpdateRewordId()
		{
			this.idDropDownList.Items.Clear();

			switch ((mw.Enums.RewardType)int.Parse(this.rewardTypeDropDownList.SelectedValue))
			{
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

			if (!int.TryParse(this.indexTextBox.Text, out intValue) || intValue < 0)
			{
				this.errorLabel.Text = "位置索引输入错误";
				return false;
			}
			config.index = intValue;

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

			config.reward_count = intValue;

			if (config.rand_type == mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
			{
				config.cost_type = int.Parse(this.costDropDownList.SelectedItem.Value);

				if (!int.TryParse(this.costTextBox.Text, out intValue) || intValue <= 0)
				{
					this.errorLabel.Text = "费用输入错误";
					return false;
				}
				if (intValue > 9999999)
				{
					this.errorLabel.Text = "费用不能大于9999999";
					return false;
				}
				config.cost_val = intValue;

				if (!int.TryParse(this.limitTextBox.Text, out intValue) || intValue < 0)
				{
					this.errorLabel.Text = "限购数量输入错误";
					return false;
				}
				//if (intValue != 0 && config.reward_count > intValue)
				//{
				//	this.errorLabel.Text = "数量不能大于限制购买次数";
				//	return false;
				//}

				config.limit_count = intValue;
				config.min_rand = 1;
				config.max_rand = 10000;
			}
			else
			{
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
			}

			if (!int.TryParse(this.groupTextBox.Text, out intValue) || ((intValue < 65 || intValue > 127) && intValue != 0))
			{
				this.errorLabel.Text = "限制分组只能在 65~127 之间, 或为 0";
				return false;
			}
			config.check_idx = intValue;

			return true;
		}

		/// <summary>
		/// 获取配置的文本
		/// </summary>
		/// <param name="config">配置</param>
		/// <returns>文本</returns>
		private string GetConfigText(mw.RandConfig config)
		{
			StringBuilder builder = new StringBuilder(config.index.ToString());

			builder.Append(" | ");

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
					for (int i = 0; i < this.costDropDownList.Items.Count; ++i)
					{
						ListItem item = this.costDropDownList.Items[i];
						if (config.reward_type.ToString() == item.Value)
						{
							builder.Append(item.Text);
							break;
						}
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

			builder.Append(" ").Append(config.reward_count);

			if (config.rand_type == mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY)
			{
				builder.Append(" | ").Append(config.cost_val).Append(" ");

				for (int i = 0; i < this.costDropDownList.Items.Count; ++i)
				{
					ListItem item = this.costDropDownList.Items[i];
					if (config.cost_type.ToString() == item.Value)
					{
						builder.Append(item.Text);
						break;
					}
				}

				builder.Append(" | 限购 ").Append(config.limit_count);
			}
			else
			{
				builder.Append(" | ").Append(config.min_rand).Append("~").Append(config.max_rand);
			}

			builder.Append(" | 限制分组 ").Append(config.check_idx);

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
			if (Mall.RandTypeLimitDictionary.TryGetValue(type, out limit))
			{
				return limit;
			}
			else
			{
				return int.MaxValue;
			}
		}

		/// <summary>
		/// 检查索引重复
		/// </summary>
		/// <returns>是否正确</returns>
		private bool CheckIndexRepeat()
		{
			HashSet<int> itemSet = new HashSet<int>();

			foreach (var config in RandTable.RandList)
			{
				if (config.rand_type != mw.Enums.RandType.RAND_TYPE_YUANBAO_ACTIVITY) { continue; }

				if (itemSet.Contains(config.index)) { return false; }

				itemSet.Add(config.index);
			}

			return true;
		}

		/// <summary>
		/// 静态构造
		/// </summary>
		static Mall()
		{
			Mall.RandTypeLimitDictionary = new Dictionary<mw.Enums.RandType, int>()
			{
				{ mw.Enums.RandType.RAND_TYPE_LUCK,		8 },
				{ mw.Enums.RandType.RAND_TYPE_XINGYUN,	6 },
				{ mw.Enums.RandType.RAND_TYPE_HAOHUA,	6 },
				{ mw.Enums.RandType.RAND_TYPE_ZHIZUN,	6 },
			};

			Mall.EconomicDefine = new List<string[]>()
			{
				new string[] { "金钱",		"2"		},
				new string[] { "元宝",		"3"		},
				new string[] { "精力",		"4"		},
				new string[] { "技能点",	"5"		},
				new string[] { "分解点",	"6"		},
				new string[] { "活跃点",	"7"		},
				new string[] { "友情点",	"8"		},
				new string[] { "荣誉点",	"9"		},
				new string[] { "小月卡",	"15"	},
				new string[] { "大月卡",	"16"	},
				new string[] { "帮贡",		"17"	},
			};
		}

		/// <summary>
		/// 商城类型数量限制字典
		/// </summary>
		public static readonly Dictionary<mw.Enums.RandType, int> RandTypeLimitDictionary;

		/// <summary>
		/// 经济定义
		/// </summary>
		public static readonly List<string[]> EconomicDefine;
	}

	/// <summary>
	/// 商城配置
	/// </summary>
	class RandTable
	{
		/// <summary>
		/// 开始
		/// </summary>
		public static void Start()
		{
			RandTable.RandList = TableManager.Load<mw.RandConfig>();
			if (RandTable.RandList == null)
			{
				RandTable.RandList = new List<mw.RandConfig>();
			}
		}

		/// <summary>
		/// 获取配置
		/// </summary>
		/// <param name="index">配置索引</param>
		/// <param name="type">商城类型</param>
		/// <returns>配置</returns>
		public static mw.RandConfig GetConfig(int index, mw.Enums.RandType type)
		{
			int realIndex = RandTable.GetRealIndex(index, type);

			if (realIndex >= 0)
			{
				return RandTable.RandList[realIndex];
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获取精确索引
		/// </summary>
		/// <param name="index">配置索引</param>
		/// <param name="type">商城类型</param>
		/// <returns>精确索引</returns>
		public static int GetRealIndex(int index, mw.Enums.RandType type)
		{
			int currentIndex = 0;

			for (int i = 0; i < RandTable.RandList.Count; ++i)
			{
				mw.RandConfig listConfig = RandTable.RandList[i];
				if (listConfig.rand_type != type) { continue; }

				if (currentIndex == index)
				{
					return i;
				}
				else
				{
					++currentIndex;
				}
			}

			return -1;
		}

		/// <summary>
		/// 商城配置列表
		/// </summary>
		public static List<mw.RandConfig> RandList;
	}
}