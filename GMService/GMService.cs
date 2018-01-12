using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Text;

namespace GMService
{
	/// <summary>
	/// GM服务
	/// </summary>
	public partial class GMService : ServiceBase
	{

#region 对外方法

		/// <summary>
		/// 构造方法
		/// </summary>
		public GMService()
		{
			InitializeComponent();
		}

		/// <summary>
		/// 启动
		/// </summary>
		public void Start()
		{
			System.Net.ServicePointManager.DefaultConnectionLimit = 100;

			GMService.timer = new System.Timers.Timer(1000);
			GMService.timer.Elapsed += GMService.timer_Elapsed;
			GMService.timer.Start();

			Network.RegisterMessageProcess(MessageType.Url, (type, reader) =>
			{
				GMService.lastSaveDay = DateTime.Today;
				GMService.url = reader.ReadString();
			});

			Activity.Start();
			PVPReward.Start();
			Broadcast.Start();
			Network.Start();
		}

		/// <summary>
		/// 停止
		/// </summary>
		public new void Stop()
		{
		}
		
#endregion

#region 事件处理
		
		/// <summary>
		/// 定时器计时
		/// </summary>
		private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (string.IsNullOrEmpty(GMService.url)) { return; }

			if (DateTime.Today > GMService.lastSaveDay)
			{
				GMService.lastSaveDay = DateTime.Today;
			}
			else
			{
				return;
			}

			HttpWebRequest	request = null;
			HttpWebResponse	respone = null;

			try
			{
				request = WebRequest.Create("http://" + GMService.url + "/MessageByVipSave.aspx") as HttpWebRequest;
				request.KeepAlive = false;
				respone = request.GetResponse() as HttpWebResponse;
			}

			catch (Exception)
			{
			}

			finally
			{
				if (request != null) { request.Abort(); }
				if (respone != null) { respone.Close(); }
			}
		}

#endregion
		
#region 内部方法

		/// <summary>
		/// 服务器启动响应
		/// </summary>
		/// <param name="args">参数</param>
		protected override void OnStart(string[] args)
		{
			this.Start();
		}

		/// <summary>
		/// 服务停止响应
		/// </summary>
		protected override void OnStop()
		{
			this.Stop();
		}

#endregion
		
#region 变量

		/// <summary>
		/// 网址
		/// </summary>
		private static string url;

		/// <summary>
		/// 最后保存天
		/// </summary>
		private static DateTime lastSaveDay;

		/// <summary>
		/// 定时器
		/// </summary>
		private static System.Timers.Timer timer;

#endregion
		
	}
}