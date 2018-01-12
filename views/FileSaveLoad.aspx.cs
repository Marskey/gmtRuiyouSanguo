using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
    public partial class FileSaveLoad : System.Web.UI.Page
    {
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
                    Session["Server"] = gm.Server.GetServerAt(0);
                }
            }
        }
      

       
        //移除
        protected void RemoveactivityButton_Click(object sender, EventArgs e)
        {
            //从字典中移除
            ActivityData removedata = new ActivityData(int.Parse(this.nextListBox.SelectedItem.Value), -1);
            mw.ActivityConfig removedataactivityConfig = ActivityManger.ActivityDataDictionary[removedata.Id];
            ActivityManger.ActivityDataDictionary.Remove(removedataactivityConfig.id);

            this.nextListBox.Items.RemoveAt(this.nextListBox.SelectedIndex);

            List<mw.ActivityConfig> list = new List<mw.ActivityConfig>();
            for (int i = 0; i < this.nextListBox.Items.Count; i++)
            {
                ActivityData data = new ActivityData(int.Parse(this.nextListBox.Items[i].Value), -1);
                mw.ActivityConfig activityConfig = ActivityManger.ActivityDataDictionary[data.Id];
                list.Add(activityConfig);
            }

               //重新保存文件
               RWManager.Save<mw.ActivityConfig>("ActivityConfig.protodata.bytes", list);

               this.ErrotLable.Text = "移除活动成功！";
        }


        //读取
        protected void LoadactivityButton_Click(object sender, EventArgs e)
        {   
            //读文件的时候需先把数据放入字典
            //ActivityManger.ActivityDataDictionary.Clear();
            //this.ErrotLable.Text = "字典:"+ActivityManger.ActivityDataDictionary.Count().ToString();
            //string path = HttpRuntime.AppDomainAppPath + "ActivityConfig2.protodata.bytes";
            //if (File.Exists(path))
            //{
            //    using (FileStream stream = File.OpenRead(path))
            //    {
            //        byte[] buffer = new byte[stream.Length];
            //        stream.Read(buffer, 0, buffer.Length);

            //        ProtoData<mw.ActivityConfig> activityConfigSet = new ProtoData<mw.ActivityConfig>(buffer);
            //        for (int i = 0; i < activityConfigSet.Count; ++i)
            //        {
            //            mw.ActivityConfig activityConfig = activityConfigSet[i];
            //            ActivityManger.ActivityDataDictionary.Add(activityConfig.id, activityConfig);
            //        }
            //    }
            //}

            this.nextListBox.Items.Clear();
            List<mw.ActivityConfig> list = new List<mw.ActivityConfig>();
            list = RWManager.Load<mw.ActivityConfig>("ActivityConfig.protodata.bytes");
            if (list == null)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                this.nextListBox.Items.Add(new ListItem(ActivityManger.GetConfigText(list[i], -1), list[i].id.ToString()));
            }

        }


        //修改取出的活动按钮
        protected void UpdateacticityButton_Click(object sender, EventArgs e)
        {
            ActivityData changedata = new ActivityData(int.Parse(this.nextListBox.SelectedItem.Value), -1);
            mw.ActivityConfig changeactivityConfig = ActivityManger.ActivityDataDictionary[changedata.Id];
            changeactivityConfig.name = this.activityname.Text;
            changeactivityConfig.param = int.Parse(this.paramTextBox.Text);
            changeactivityConfig.sign = int.Parse(this.signTextBox.Text);
            changeactivityConfig.last_days = int.Parse(this.lastTextBox.Text);
            List<mw.ActivityConfig> list = new List<mw.ActivityConfig>();
            for (int i = 0; i < this.nextListBox.Items.Count; i++)
            {
                ActivityData data = new ActivityData(int.Parse(this.nextListBox.Items[i].Value), -1);
                mw.ActivityConfig activityConfig = ActivityManger.ActivityDataDictionary[data.Id];
                list.Add(activityConfig);
            }

            //重新保存文件
            RWManager.Save<mw.ActivityConfig>("ActivityConfig.protodata.bytes", list);
            //刷新页面
            this.nextListBox.Items.Clear();
            List<mw.ActivityConfig> newlist = new List<mw.ActivityConfig>();
            newlist = RWManager.Load<mw.ActivityConfig>("ActivityConfig.protodata.bytes");
            if (newlist == null)
                return;
            for (int i = 0; i < newlist.Count; i++)
            {
                this.nextListBox.Items.Add(new ListItem(ActivityManger.GetConfigText(newlist[i], -1), newlist[i].id.ToString()));
            }

             this.ErrotLable.Text = "修改活动成功";
        }



        //添加新活动
        protected void addnewactivityButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.activityname.Text))
            {
                this.ErrotLable.Text = "请输入活动名称";
                return;
            }

            if (string.IsNullOrEmpty(this.idTextBox.Text))
            {
                this.ErrotLable.Text = "请输入活动ID";
            }
            else
            {

                mw.ActivityConfig addacticityConfig = new mw.ActivityConfig();
                addacticityConfig.id = int.Parse(this.idTextBox.Text);
                addacticityConfig.name = this.activityname.Text;
                addacticityConfig.sign = int.Parse(this.signTextBox.Text);
                addacticityConfig.param = int.Parse(this.paramTextBox.Text);
                addacticityConfig.last_days = int.Parse(this.lastTextBox.Text);
                ////如果活动ID已经存在，则不允许添加
                //for (int i = 0; i < ActivityManger.ActivityDataDictionary.Count; i++)
                //{
                //    //字典中已经存在了该ID
                //    if(addacticityConfig.id==ActivityManger.ActivityDataDictionary[i].id)
                //    {
                //        this.ErrotLable.Text = "该活动ID已存在，请重新填写活动ID";
                //        return;
                //    }

                    
                //}


                List<mw.ActivityConfig> list = new List<mw.ActivityConfig>();
                list = RWManager.Load<mw.ActivityConfig>("ActivityConfig.protodata.bytes");
                //添加新活动到链表中
                list.Add(addacticityConfig);
                //把新活动放入字典中
                ActivityManger.ActivityDataDictionary.Add(addacticityConfig.id, addacticityConfig);
                //ActivityManger.ActivityDataDictionary.Remove(3600);
                RWManager.Save<mw.ActivityConfig>("ActivityConfig.protodata.bytes", list);
                //刷新界面
                this.nextListBox.Items.Clear();
                List<mw.ActivityConfig> afterlist = new List<mw.ActivityConfig>();
                afterlist = RWManager.Load<mw.ActivityConfig>("ActivityConfig.protodata.bytes");
                if (afterlist == null)
                    return;
                for (int i = 0; i < afterlist.Count; i++)
                {
                    this.nextListBox.Items.Add(new ListItem(ActivityManger.GetConfigText(afterlist[i], -1), afterlist[i].id.ToString()));
                }

                this.ErrotLable.Text = "新活动添加成功";
            }

        }

 


        //取出活动按钮
        protected void GetacticityButton_Click(object sender, EventArgs e)
        {   
            ////更新活动字典
            //ActivityManger.ActivityDataDictionary.Clear();
            //string path = HttpRuntime.AppDomainAppPath + "ActivityConfig2.protodata.bytes";
            //if (File.Exists(path))
            //{
            //    using (FileStream stream = File.OpenRead(path))
            //    {
            //        byte[] buffer = new byte[stream.Length];
            //        stream.Read(buffer, 0, buffer.Length);

            //        ProtoData<mw.ActivityConfig> activityConfigSet = new ProtoData<mw.ActivityConfig>(buffer);
            //        for (int i = 0; i < activityConfigSet.Count; ++i)
            //        {
            //            mw.ActivityConfig activityConfig = activityConfigSet[i];
            //            ActivityManger.ActivityDataDictionary.Add(activityConfig.id, activityConfig);
            //        }
            //    }
            //}


            if (this.nextListBox.SelectedIndex == -1)
            {
                this.ErrotLable.Text = "请选择一个活动";
                return;
            }
            else
            {
                ActivityData data = new ActivityData(int.Parse(this.nextListBox.SelectedItem.Value), -1);
                mw.ActivityConfig activityConfig = ActivityManger.ActivityDataDictionary[data.Id];
            
                this.activityname.Text = activityConfig.name;
                this.idTextBox.Text = activityConfig.id.ToString();
                this.signTextBox.Text = activityConfig.sign.ToString();
                this.paramTextBox.Text = activityConfig.param.ToString();
                this.lastTextBox.Text = activityConfig.last_days.ToString();
                this.ErrotLable.Text = "取出活动成功";
            }
        }



      


      
    }
}