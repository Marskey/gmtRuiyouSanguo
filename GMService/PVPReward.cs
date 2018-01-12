using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GMService
{
	/// <summary>
	/// PVP奖励
	/// </summary>
	static class PVPReward
	{

#region 对外方法

		/// <summary>
		/// 启动
		/// </summary>
		public static void Start()
		{
			PVPReward.timer = new System.Timers.Timer(1000 * 60);
			PVPReward.timer.Elapsed += PVPReward.timer_Elapsed;
			PVPReward.timer.Start();

			Network.RegisterMessageProcess(MessageType.PVPRewardCheck, PVPReward.ProcessCheck);
			Network.RegisterMessageProcess(MessageType.PVPRewardUpdate, PVPReward.ProcessUpdate);
		}

#endregion

#region 事件处理
		
		/// <summary>
		/// 定时器计时
		/// </summary>
		private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			if (!PVPReward.hasInitialize) { return; }

			bool hasSend = false;

			lock (PVPReward.dataList)
			{
				for (int i = 0; i < PVPReward.dataList.Count; ++i)
				{
					PVPRewardData data = PVPReward.dataList[i];

					if (data.gift != 0 &&
						(DateTime.Now - data.UpdateTime).TotalDays >= 7)
					{
						data.gift = 0;
						hasSend = true;
						Log.AddLog(data.ServerId + " PVP奖励更新时间太早 " + data.UpdateTime.ToShortDateString());
						continue;
					}

					if (data.gift != 0 &&
						DateTime.Now >= data.UpdateTime &&
						GMCommand.Execute(data.Address, data.ServerId, "0", string.Format("RPVP({0})", data.gift), ""))
					{
						data.gift = 0;
						hasSend = true;
					}
				}

				if (hasSend) { PVPReward.SendUpdate(); }
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
			writer.Write(PVPReward.hasInitialize);

			Network.Send(writer);

			if (PVPReward.hasInitialize) { PVPReward.SendUpdate(); }
		}
		
		/// <summary>
		/// 消息处理：更新
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="reader">读取器</param>
		private static void ProcessUpdate(ushort type, BinaryReader reader)
		{
			lock (PVPReward.dataList)
			{
				PVPReward.dataList.Clear();

				ushort count = reader.ReadUInt16();

				for (int i = 0; i < count; ++i)
				{
					PVPRewardData data = new PVPRewardData();
					data.Address = reader.ReadString();
					data.ServerId = reader.ReadString();
					data.UpdateTime = new DateTime(reader.ReadUInt16(), reader.ReadByte(), reader.ReadByte());
					data.gift = reader.ReadInt32();
					PVPReward.dataList.Add(data);
				}
			}

			PVPReward.hasInitialize = true;
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
			writer.Write((ushort)MessageType.PVPRewardUpdate);

			lock (PVPReward.dataList)
			{
				writer.Write((ushort)PVPReward.dataList.Count);

				foreach (var data in PVPReward.dataList)
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
		/// PVP奖励数据列表
		/// </summary>
		private static List<PVPRewardData> dataList = new List<PVPRewardData>();

#endregion

	}

	/// <summary>
	/// PVP奖励数据
	/// </summary>
	class PVPRewardData
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
		/// 礼包编号
		/// </summary>
		public int gift;
	}
}