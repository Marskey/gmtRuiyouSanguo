using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    /// <summary>
    /// 礼包
    /// </summary>
    public partial class Gift : AGmPage
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public Gift()
            : base(PrivilegeType.Gift)
        {

        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            if (!this.IsPostBack)
            {
                this.VersionIDText.Text = GmModify.Version;

                // 0 经济类型
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20000), "0"));
                // 1 物品
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20001), "1"));
                // 2 武魂
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20003), "2"));
                // 3 饰品
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22017), "3"));
                // 4 晶石
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22018), "4"));
                // 5 坐骑碎片
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(22019), "5"));
                // 6 武将
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(336), "6"));
                // 8 宠物
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(725), "8"));
                // 9 坐骑
                this.typeDropDownList.Items.Add(new ListItem(TableManager.GetGMTText(20009), "9"));

                GmModify.itemGiveOptionDisplay(ref typeDropDownList, ref idDropDownList, ref countTextBox);


                if (GiftTable.GiftListEx != null)
                {
                    foreach (var config in GiftTable.GiftListEx)
                    {
                        this.giftListBox.Items.Add(new ListItem(this.GetGiftText(config), config.id.ToString()));

                    }
                }
            }

            this.outputLabel.Text = "";
        }

        /// <summary>
        /// 礼包列表选中改变响应
        /// </summary>
        protected void giftListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.itemListBox.Items.Clear();
            if (this.giftListBox.SelectedIndex >= 0)
            {
                var config = GiftTable.GiftListEx[this.giftListBox.SelectedIndex];
                this.titleTextBox.Text = TextManager.GetText(config.title);
                this.descriptionTextBox.Text = TextManager.GetText(config.desc);

                this.UpdateItem(0, config.reward_idx_0, config.reward_type_0, config.reward_count_0);
                this.UpdateItem(1, config.reward_idx_1, config.reward_type_1, config.reward_count_1);
                this.UpdateItem(2, config.reward_idx_2, config.reward_type_2, config.reward_count_2);
                this.UpdateItem(3, config.reward_idx_3, config.reward_type_3, config.reward_count_3);
                //this.UpdateItem(4, config.reward_idx_4, config.reward_type_4, config.reward_count_4);
            }
        }

        /// <summary>
        /// 添加按钮点击响应
        /// </summary>
        protected void addButton_Click(object sender, EventArgs e)
        {
            mw.GiftConfig config = new mw.GiftConfig();
            config.id = GiftTable.IdIncrease;
            config.title = TextManager.CreateText();
            config.desc = TextManager.CreateText();
            config.icon = "131409";
            config.from = 160044;
            TextManager.SetText(config.title, this.titleTextBox.Text);
            TextManager.SetText(config.desc, this.descriptionTextBox.Text);
            GiftTable.GiftList.Add(config);
            GiftTable.GiftListEx.Add(config);
            TableManager.Save(GiftTable.GiftListEx);
            this.giftListBox.Items.Add(new ListItem(this.GetGiftText(config), config.id.ToString()));
            ++GiftTable.IdIncrease;
        }

        /// <summary>
        /// 修改按钮点击响应
        /// </summary>
        protected void modifyButton_Click(object sender, EventArgs e)
        {
            if (this.giftListBox.SelectedIndex < 0) { return; }

            var config = GiftTable.GiftListEx[this.giftListBox.SelectedIndex];
            config.title = TextManager.CreateText();
            TextManager.SetText(config.title, this.titleTextBox.Text);
            config.desc = TextManager.CreateText();
            TextManager.SetText(config.desc, this.descriptionTextBox.Text);
            TableManager.Save(GiftTable.GiftListEx);
            this.giftListBox.SelectedItem.Text = this.GetGiftText(config);
        }

        /// <summary>
        /// 删除按钮点击响应
        /// </summary>
        protected void deleteButton_Click(object sender, EventArgs e)
        {
            if (this.giftListBox.SelectedIndex < 0) { return; }

            GiftTable.GiftListEx.RemoveAt(this.giftListBox.SelectedIndex);
            TableManager.Save(GiftTable.GiftListEx);
            this.giftListBox.Items.RemoveAt(this.giftListBox.SelectedIndex);
        }

        /// <summary>
        /// 发送按钮点击响应
        /// </summary>
        protected void sendButton_Click(object sender, EventArgs e)
        {
            if (GiftTable.GiftListEx == null)
            {
                this.outputLabel.Text = TableManager.GetGMTText(701);
                return;
            }

            this.outputLabel.Text = TableManager.Send(GiftTable.GiftListEx);
            this.outputLabel.Text += TableManager.Send(TextManager.GetConfigList());
        }

        /// <summary>
        /// 上传按钮点击响应
        /// </summary>
        //protected void uploadButton_Click(object sender, EventArgs e)
        //{
        //    if (this.giftFileUpload.HasFile)
        //    {
        //        GiftTable.GiftList = TableManager.Unserialize<mw.GiftConfig>(this.giftFileUpload.FileBytes);
        //        GiftTable.DoStart();

        //        this.giftListBox.Items.Clear();

        //        foreach (var config in GiftTable.GiftList)
        //        {
        //            this.giftListBox.Items.Add(new ListItem(this.GetGiftText(config), config.id.ToString()));
        //        }

        //        this.UpdateRewordId();

        //        this.outputLabel.Text = TableManager.GetGMTText(655);

        //        TableManager.Save(GiftTable.GiftList);
        //    }
        //    else
        //    {
        //        this.outputLabel.Text = TableManager.GetGMTText(656);
        //    }
        //}

        /// <summary>
        /// 奖励类型下拉列表选中改变响应
        /// </summary>
        protected void typeDropDownList_SelectedIndexChanged(object sender, EventArgs e)
        {
            GmModify.itemGiveOptionDisplay(ref typeDropDownList, ref idDropDownList, ref countTextBox);
        }

        /// <summary>
        /// 修改物品按钮点击响应
        /// </summary>
        protected void modifyItemButton_Click(object sender, EventArgs e)
        {
            //道具数量不能为空
            if (string.IsNullOrEmpty(this.countTextBox.Text))
            {
                this.outputLabel.Text = TableManager.GetGMTText(702) + "!";
                return;
            }

            if (int.Parse(this.countTextBox.Text) == 0)
            {
                this.outputLabel.Text = TableManager.GetGMTText(703) + "!";
                return;
            }

            if (this.itemListBox.SelectedIndex < 0) { return; }
            if (giftListBox.SelectedIndex < 0)
            {
                outputLabel.Text = TableManager.GetGMTText(704) + "!";
                return;
            }

            var config_in_all = GiftTable.GiftListEx[this.giftListBox.SelectedIndex];
            mw.GiftConfig config = null;
            foreach (var cf in GiftTable.GiftListEx)
            {
                if (cf.id == config_in_all.id)
                {
                    config = cf;
                }
            }

            if (null == config)
            {
                outputLabel.Text = TableManager.GetGMTText(705) + "!";
                return;
            }

            switch (this.itemListBox.SelectedIndex)
            {
                case 0:
                    config.reward_idx_0 = (mw.Enums.RewardType)this.typeDropDownList.SelectedIndex;
                    config.reward_type_0 = int.Parse(this.idDropDownList.SelectedValue);
                    config.reward_count_0 = int.Parse(this.countTextBox.Text);
                    this.UpdateItem(0, config.reward_idx_0, config.reward_type_0, config.reward_count_0);
                    break;

                case 1:
                    config.reward_idx_1 = (mw.Enums.RewardType)this.typeDropDownList.SelectedIndex;
                    config.reward_type_1 = int.Parse(this.idDropDownList.SelectedValue);
                    config.reward_count_1 = int.Parse(this.countTextBox.Text);
                    this.UpdateItem(1, config.reward_idx_1, config.reward_type_1, config.reward_count_1);
                    break;

                case 2:
                    config.reward_idx_2 = (mw.Enums.RewardType)this.typeDropDownList.SelectedIndex;
                    config.reward_type_2 = int.Parse(this.idDropDownList.SelectedValue);
                    config.reward_count_2 = int.Parse(this.countTextBox.Text);
                    this.UpdateItem(2, config.reward_idx_2, config.reward_type_2, config.reward_count_2);
                    break;

                case 3:
                    config.reward_idx_3 = (mw.Enums.RewardType)this.typeDropDownList.SelectedIndex;
                    config.reward_type_3 = int.Parse(this.idDropDownList.SelectedValue);
                    config.reward_count_3 = int.Parse(this.countTextBox.Text);
                    this.UpdateItem(3, config.reward_idx_3, config.reward_type_3, config.reward_count_3);
                    break;
            }

            TableManager.Save(GiftTable.GiftListEx);
        }

        /// <summary>
        /// 物品列表选中改变响应
        /// </summary>
        protected void itemListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.giftListBox.SelectedIndex == -1)
            {
                return;
            }

            if (this.itemListBox.SelectedIndex < 0) { return; }

            var config = GiftTable.GiftListEx[this.giftListBox.SelectedIndex];

            switch (this.itemListBox.SelectedIndex)
            {
                case 0:
                    this.SetItem(config.reward_idx_0, config.reward_type_0, config.reward_count_0);
                    break;

                case 1:
                    this.SetItem(config.reward_idx_1, config.reward_type_1, config.reward_count_1);
                    break;

                case 2:
                    this.SetItem(config.reward_idx_2, config.reward_type_2, config.reward_count_2);
                    break;

                case 3:
                    this.SetItem(config.reward_idx_3, config.reward_type_3, config.reward_count_3);
                    break;

                case 4:
                    this.SetItem(config.reward_idx_4, config.reward_type_4, config.reward_count_4);
                    break;
            }
        }

        /// <summary>
        /// 获取礼包文本
        /// </summary>
        /// <param name="config">礼包配置</param>
        /// <returns>礼包文本</returns>
        private string GetGiftText(mw.GiftConfig config)
        {
            //return config.id + " | " + config.title;
            return config.id + " | " + TextManager.GetText(config.title);
        }

        /// <summary>
        /// 更新物品
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="type">奖励类型</param>
        /// <param name="id">物品编号</param>
        /// <param name="count">数量</param>
        private void UpdateItem(int index, mw.Enums.RewardType type, int id, int count)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(this.typeDropDownList.Items[(int)type].Text);
            builder.Append(" | ");

            switch ((int)type)
            {
                // 0 经济类型
                case 0:
                    {
                        if (id == 0 || !playerHistroy.economicName.ContainsKey(id)) goto cannotFindID;
                        builder.Append(playerHistroy.economicName[id] + "(" + id + ")");
                    }
                    break;
                // 1 物品
                case 1:
                    {
                        if (!TableManager.ItemTable.ContainsKey(id)) goto cannotFindID;
                        mw.ItemConfig itemInfo = TableManager.ItemTable[id];
                        builder.Append(TextManager.GetText(itemInfo.name) + "(" + id + ")");
                    }
                    break;
                // 2 武魂
                case 2:
                    {
                        if (!TableManager.HeroTable.ContainsKey(id)) goto cannotFindID;
                        mw.HeroBaseConfig heroInfo = TableManager.HeroTable[id];
                        builder.Append(TextManager.GetText(heroInfo.name) + "(" + id + ")");
                    }
                    break;
                // 3 饰品
                case 3:
                    {
                        if (!TableManager.StoneTable.ContainsKey(id)) goto cannotFindID;
                        mw.StoneConfig stoneInfo = TableManager.StoneTable[id];
                        builder.Append(TextManager.GetText(stoneInfo.name) + "(" + id + ")");
                    }
                    break;
                // 4 晶石
                case 4:
                    {
                        if (!TableManager.PetStoneTable.ContainsKey(id)) goto cannotFindID;
                        mw.ItemConfig psInfo = TableManager.PetStoneTable[id];
                        builder.Append(TextManager.GetText(psInfo.name) + "(" + id + ")");
                    }
                    break;
                // 5 坐骑碎片
                case 5:
                    {
                        if (!TableManager.MountTable.ContainsKey(id)) goto cannotFindID;
                        mw.MountConfig mountInfo = TableManager.MountTable[id];
                        builder.Append(TableManager.MountTable[id].name + "(" + id + ")");
                    }
                    break;
                // 6 武将
                case 6:
                    {
                        if (!TableManager.HeroTable.ContainsKey(id)) goto cannotFindID;
                        builder.Append(TableManager.HeroTable[id].name + "(" + id + ")");
                    }
                    break;
                // 7 经验
                case 7:
                    {
                        if (id == 0)
                            builder.Append(TableManager.GetGMTText(338));
                        else
                            builder.Append(TableManager.GetGMTText(339));
                    }
                    break;
                // 8 宠物
                case 8:
                    {
                        if (!TableManager.PetTable.ContainsKey(id)) goto cannotFindID;
                        mw.PetConfig petInfo = TableManager.PetTable[id];
                        builder.Append(TextManager.GetText(petInfo.name) + "[" + petInfo.petstar + " STAR]" + "(" + id + ")");
                    }
                    break;
                // 9 坐骑
                case 9:
                    {
                        if (!TableManager.MountTable.ContainsKey(id)) goto cannotFindID;
                        mw.MountConfig mountInfo = TableManager.MountTable[id];
                        builder.Append(TextManager.GetText(mountInfo.name) + "(" + id + ")");
                    }
                    break;
            }

            goto count;

        cannotFindID:
            builder.Append("Unknown(" + id + ")");
        count:
            builder.Append(" | ").Append(count);

            if (index < this.itemListBox.Items.Count)
            {
                this.itemListBox.Items[index].Text = builder.ToString();
            }
            else
            {
                this.itemListBox.Items.Add(new ListItem(builder.ToString(), index.ToString()));
            }
        }

        /// <summary>
        /// 设置奖励物品
        /// </summary>
        /// <param name="type">奖励类型</param>
        /// <param name="id">物品编号</param>
        /// <param name="count">数量</param>
        private void SetItem(mw.Enums.RewardType type, int id, int count)
        {
            this.typeDropDownList.SelectedIndex = (int)type;

            GmModify.itemGiveOptionDisplay(ref typeDropDownList, ref idDropDownList, ref countTextBox);

            for (int i = 0; i < this.idDropDownList.Items.Count; ++i)
            {
                if (this.idDropDownList.Items[i].Value == id.ToString())
                {
                    this.idDropDownList.SelectedIndex = i;
                    break;
                }
            }

            this.countTextBox.Text = count.ToString();
        }

        protected void sendtableButton_Click(object sender, EventArgs e)
        {
            GiftManager.addgiftconfig = GiftTable.GiftListEx;

            if (TableManager.SendTable(VersionIDText.Text))
            {
                this.outputLabel.Text = TableManager.GetGMTText(658);
            }
            else
            {
                this.outputLabel.Text = TableManager.GetLastError();
            }
        }
    }

    /// <summary>
    /// 礼包表格
    /// </summary>
    class GiftTable
    {
        /// <summary>
        /// 开始
        /// </summary>
        public static void Start()
        {
            GiftTable.GiftListEx = TableManager.Load<mw.GiftConfig>();
            if (null == GiftTable.GiftListEx)
            {
                GiftTable.GiftListEx = new List<mw.GiftConfig>();
            }

            List<mw.GiftConfig> list = new List<mw.GiftConfig>();
            for (int i = 0; i < GiftTable.GiftListEx.Count; ++i)
            {
                if (GiftTable.GiftListEx[i].id >= 1000000)
                {
                    list.Add(GiftTable.GiftListEx[i]);
                }
            }

            TableManager.Save(list);

            GiftTable.GiftList = TableManager.Load<mw.GiftConfig>("protodatas/GiftConfig.protodata.bytes");
            if (null == GiftTable.GiftList)
            {
                GiftTable.GiftList = new List<mw.GiftConfig>();
            }

            if (GiftTable.GiftListEx != null)
            {
                for (int i = 0; i < GiftTable.GiftListEx.Count; i++)
                {
                    int j = 0;
                    for (; j < GiftTable.GiftList.Count; j++)
                    {
                        if (GiftTable.GiftListEx[i].id == GiftTable.GiftList[j].id)
                            break;
                    }
                    if (j == GiftTable.GiftList.Count)
                        GiftTable.GiftList.Add(GiftTable.GiftListEx[i]);
                }
            }

            foreach (var gift in GiftTable.GiftList)
            {
                gift.title_id = gift.title;
                gift.desc_id = gift.desc;
            }

            GiftTable.DoStart();
        }

        /// <summary>
        /// 开始
        /// </summary>
        public static void DoStart()
        {
            foreach (var config in GiftTable.GiftList)
            {
                GiftTable.IdIncrease = Math.Max(GiftTable.IdIncrease, config.id + 1);
            }
        }

        /// <summary>
        /// 礼包配置列表
        /// </summary>
        public static List<mw.GiftConfig> GiftList;
        public static List<mw.GiftConfig> GiftListEx;

        /// <summary>
        /// 增长编号
        /// </summary>
        public static int IdIncrease = 1000000;
    }


    class GiftManager
    {
        public static List<mw.GiftConfig> addgiftconfig = new List<mw.GiftConfig>();
    }
}