using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GMService
{
	/// <summary>
	/// 活动
	/// </summary>
	static class Activity
	{

#region 对外方法

		/// <summary>
		/// 启动
		/// </summary>
		public static void Start()
		{
			Activity.timer = new System.Timers.Timer(1000 * 60);
			Activity.timer.Elapsed += Activity.timer_Elapsed;
			Activity.timer.Start();

			Network.RegisterMessageProcess(MessageType.ActivityCheck, Activity.ProcessCheck);
			Network.RegisterMessageProcess(MessageType.ActivityUpdate, Activity.ProcessUpdate);
		}

#endregion

#region 事件处理
		
		/// <summary>
		/// 定时器计时
		/// </summary>
		private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (!Activity.hasInitialize) { return; }

			bool hasSend = false;

			lock (Activity.dataList)
			{
				for (int i = 0; i < Activity.dataList.Count; )
				{
					ActivityData data = Activity.dataList[i];

					if ((DateTime.Now - data.UpdateTime).TotalDays >= 7)
					{
						hasSend = true;
						Activity.dataList.RemoveAt(i);
						Log.AddLog(data.ServerId + " 活动更新时间太早 " + data.UpdateTime.ToShortDateString());
						continue;
					}

					if (DateTime.Now >= data.UpdateTime &&
						GMCommand.Execute(data.Address, data.ServerId, "0", Encoding.UTF8.GetBytes("2"), data.Buffer) &&
                        GMCommand.Execute(data.Address, data.ServerId, "0", string.Format("SDATE({0},{1},{2})", data.UpdateTime.Year,data.UpdateTime.Month, data.UpdateTime.Day), ""))
					{
                        Log.AddLog("定时更新:" + DateTime.Now);
                        Log.AddLog("年:" + data.UpdateTime.Year);
                        Log.AddLog("月:" + data.UpdateTime.Month);
                        Log.AddLog("日:" + data.UpdateTime.Day);
						hasSend = true;
						Activity.dataList.RemoveAt(i);
					}
					else
					{
						++i;
					}
				}

				if (hasSend) { Activity.SendUpdate(); }
			}
		}

#endregion

#region 消息处理

		/// <summary>
		/// 消息处理：检查
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="reader">读取器</param>
		private static void ProcessCheck(ushort type, BinaryReader reader)
		{
			BinaryWriter writer = new BinaryWriter(new MemoryStream());

			writer.Write((int)0);
			writer.Write((ushort)type);
			writer.Write(Activity.hasInitialize);

			Network.Send(writer);

			if (Activity.hasInitialize) { Activity.SendUpdate(); }
		}
		
		/// <summary>
		/// 消息处理：更新
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="reader">读取器</param>
		private static void ProcessUpdate(ushort type, BinaryReader reader)
		{
			lock (Activity.dataList)
			{
				Activity.dataList.Clear();

				ushort count = reader.ReadUInt16();

				for (int i = 0; i < count; ++i)
				{
					ActivityData data = new ActivityData();
					data.Address = reader.ReadString();
					data.ServerId = reader.ReadString();
                    //data.UpdateTime = new DateTime(reader.ReadUInt16(), reader.ReadByte(), reader.ReadByte());
                    //5点更新
                    data.UpdateTime = new DateTime(reader.ReadUInt16(), reader.ReadByte(), reader.ReadByte(),5,0,0);
					data.Buffer = reader.ReadBytes(reader.ReadUInt16());
					Activity.dataList.Add(data);
				}
			}

			Activity.hasInitialize = true;
		}

#endregion

#region 内部方法

		/// <summary>
		/// 发送更新
		/// </summary>
		private static void SendUpdate()
		{
			BinaryWriter writer = new BinaryWriter(new MemoryStream());

			writer.Write((int)0);
			writer.Write((ushort)MessageType.ActivityUpdate);

			lock (Activity.dataList)
			{
				writer.Write((ushort)Activity.dataList.Count);

				foreach (var data in Activity.dataList)
				{
					writer.Write(data.Address);
					writer.Write(data.ServerId);
				}
			}

			Network.Send(writer);
		}
		
#endregion

#region 变量

		/// <summary>
		/// 是否初始化
		/// </summary>
		private static bool hasInitialize;

		/// <summary>
		/// 定时器
		/// </summary>
		private static System.Timers.Timer timer;

		/// <summary>
		/// 活动数据列表
		/// </summary>
		private static List<ActivityData> dataList = new List<ActivityData>();

#endregion

	}

	/// <summary>
	/// 活动数据
	/// </summary>
	class ActivityData
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
		/// 更新时间
		/// </summary>
		public DateTime UpdateTime;

		/// <summary>
		/// 缓冲区
		/// </summary>
		public byte[] Buffer;
	}
}