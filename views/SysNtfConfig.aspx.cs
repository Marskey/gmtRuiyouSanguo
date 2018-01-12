using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{   
    public partial class SysNtfConfig : System.Web.UI.Page
    {   
        
        private static Dictionary<int, mw.SysNtfConfig> SysNtfDictionary = new Dictionary<int, mw.SysNtfConfig>();
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        //读SysNtfConfig.protodata.bytes文件
        protected void LoadBtn_Click(object sender, EventArgs e)
        {
            this.SysNtfListBox.Items.Clear();
            SysNtfDictionary.Clear();
            List<mw.SysNtfConfig> list = new List<mw.SysNtfConfig>();
            list = RWManager.Load<mw.SysNtfConfig>("SysNtfConfig.protodata.bytes");
            for (int i = 0; i < list.Count; i++)
            {    
                SysNtfDictionary.Add(list[i].id, list[i]); 
                this.SysNtfListBox.Items.Add(new ListItem(list[i].id.ToString()+""+list[i].title_str+""+list[i].details_str, list[i].id.ToString()));
            }
                this.ErrorLable.Text = list.Count.ToString();        
        }
        //移除
        protected void RemoveBtn_Click(object sender, EventArgs e)
        {
            this.SysNtfListBox.Items.RemoveAt(this.SysNtfListBox.SelectedIndex);

            List<mw.SysNtfConfig> list = new List<mw.SysNtfConfig>();
            for (int i = 0; i < this.SysNtfListBox.Items.Count; i++)
            {
               
            }

            //重新保存文件
           // RWManager.Save<mw.SysNtfConfig>("SysNtfConfig.protodata.bytes", list);
        }

        //取出
        protected void GetBtn_Click(object sender, EventArgs e)
        {
            
            if (this.SysNtfListBox.SelectedIndex == -1)
            {
                 this.ErrorLable.Text = "请选择一行数据";
                 return;
            }
            else
            {
   
                this.TipLabel.Text = SysNtfDictionary.Count.ToString();
                mw.SysNtfConfig config = SysNtfDictionary[int.Parse(this.SysNtfListBox.SelectedItem.Value)];
                this.SysNtfname.Text = config.title_str;
                this.SysNtfID.Text = config.id.ToString();
            }
        }

        //添加
        protected void AddBtn_Click(object sender, EventArgs e)
        {


        }

        //修改
        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            List<mw.SysNtfConfig> list = new List<mw.SysNtfConfig>();
            if (this.SysNtfListBox.SelectedIndex == -1)
            {
                this.TipLabel.Text = "请选择修改项";
                return;
            }

            //填写信息校验，ID不允许重复
            string id = this.SysNtfID.Text;
            if (this.SysNtfListBox.Items.FindByValue(id) != null&&id!=this.SysNtfListBox.SelectedValue)
            {
                this.TipLabel.Text = "ID不能重复";
                return;
            
            }

            mw.SysNtfConfig config = SysNtfDictionary[int.Parse(this.SysNtfListBox.SelectedItem.Value)];
            config.title_str = this.SysNtfname.Text;
            config.id = int.Parse(this.SysNtfID.Text);

            foreach (var pair in SysNtfDictionary.Values)
            {
                list.Add(pair);
            }
            RWManager.Save<mw.SysNtfConfig>("SysNtfConfig.protodata.bytes", list);

            //刷新数据
            this.SysNtfListBox.Items.Clear();
            SysNtfDictionary.Clear();
            List<mw.SysNtfConfig> list2 = new List<mw.SysNtfConfig>();
            list2 = RWManager.Load<mw.SysNtfConfig>("SysNtfConfig.protodata.bytes");
            for (int i = 0; i < list2.Count; i++)
            {   
                   SysNtfDictionary.Add(list2[i].id, list2[i]);
                   this.SysNtfListBox.Items.Add(new ListItem(list2[i].id.ToString() + "" + list2[i].title_str + "" + list2[i].details_str, list2[i].id.ToString()));
            }

            this.ErrorLable.Text = list2.Count.ToString();
          

        }

        //选择项变化响应 
        protected void SysNtfListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.TipLabel.Text = "选择项改变！";
            //if (this.SysNtfListBox.SelectedIndex == -1)
            //{
            //    this.ErrorLable.Text = "请选择一行数据";
            //    return;
            //}
            //else
            //{
            //    mw.SysNtfConfig config = SysNtfDictionary[int.Parse(this.SysNtfListBox.SelectedItem.Value)];
            //    this.SysNtfname.Text = config.title_str;
            //    this.SysNtfID.Text = config.id.ToString();
            //}

        }

       

    }


    /// <summary>
    /// SysNtf数据
    /// </summary>
    public class SysNtfData
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">编号</param>
        public SysNtfData(int id,string title_str)
        {
            this.Id = id;
            this.Title_str = title_str;
        }

        /// <summary>
        /// 编号
        /// </summary>
        public int Id;

        public string Title_str;
    }

    
}