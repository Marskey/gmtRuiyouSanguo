using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GMService
{
	/// <summary>
	/// 广播
	/// </summary>
	static class Broadcast
	{
		
#region 对外方法

		/// <summary>
		/// 启动
		/// </summary>
		public static void Start()
		{
			Broadcast.timer = new System.Timers.Timer(1000);
			Broadcast.timer.Elapsed += Broadcast.timer_Elapsed;
			Broadcast.timer.Start();

			Network.RegisterMessageProcess(MessageType.BroadcastUpdate, Broadcast.ProcessUpdate);
		}

#endregion

#region 事件处理
		
		/// <summary>
		/// 定时器计时
		/// </summary>
		private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (Broadcast.dataList.Count == 0) { return; }

			lock (Broadcast.dataList)
			{
				foreach (var server in Broadcast.dataList)
				{
					foreach (var data in server.BroadcastDataList)
					{
						if (++data.Accumulation >= data.IntervalSecond)
						{
							data.Accumulation = 0;
							GMCommand.Execute(server.Address, server.ServerId, "0", string.Format("SC(\"{0}\")", data.Content), "");
						}
					}
				}
			}
		}

#endregion

#region 消息处理

		/// <summary>
		/// 消息处理：更新
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="reader">读取器</param>
		private static void ProcessUpdate(ushort type, BinaryReader reader)
		{
			lock (Broadcast.dataList)
			{
				Broadcast.dataList.Clear();

				ushort count = reader.ReadUInt16();

				for (int i = 0; i < count; ++i)
				{
					BroadcastServer server = new BroadcastServer();
					server.Address = reader.ReadString();
					server.ServerId = reader.ReadString();

					byte dataCount = reader.ReadByte();
					for (int j = 0; j < dataCount; ++j)
					{
						BroadcastData data = new BroadcastData();
						data.IntervalSecond = reader.ReadInt32();
						data.Content = reader.ReadString();
						server.BroadcastDataList.Add(data);
					}
					Broadcast.dataList.Add(server);
				}
			}
		}

#endregion

#region 变量

		/// <summary>
		/// 定时器
		/// </summary>
		private static System.Timers.Timer timer;

		/// <summary>
		/// 广播列表
		/// </summary>
		private static List<BroadcastServer> dataList = new List<BroadcastServer>();

#endregion

	}

	/// <summary>
	/// 广播服务器
	/// </summary>
	class BroadcastServer
	{
		/// <summary>
		/// 发送地址
		/// </summary>
		public string Address;

		/// <summary>
		/// 服务器编号
		/// </summary>
		public string ServerId;

		/// <summary>
		/// 广播数据列表
		/// </summary>
		public List<BroadcastData> BroadcastDataList = new List<BroadcastData>();
	}

	/// <summary>
	/// 广播数据
	/// </summary>
	class BroadcastData
	{
		/// <summary>
		/// 间隔秒
		/// </summary>
		public int IntervalSecond;

		/// <summary>
		/// 内容
		/// </summary>
		public string Content;

		/// <summary>
		/// 累计
		/// </summary>
		public int Accumulation;
	}
}