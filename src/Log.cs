using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace gmt
{
	/// <summary>
	/// 日志
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// 启动
		/// </summary>
		public static void Start()
		{
			if (Log.timer != null) { return; }

			Log.timer = new System.Timers.Timer(1000 * 60);
			Log.timer.Elapsed += Log.timer_Elapsed;
			Log.timer.Start();
		}

		/// <summary>
		/// 停止
		/// </summary>
		public static void End()
		{
			Log.Save();
		}

		/// <summary>
		/// 添加记录
		/// </summary>
		/// <param name="user">用户</param>
		/// <param name="log">记录</param>
		public static void AddRecord(string user, string record)
		{
			lock (Log.record)
			{
				Log.record.Append(user).Append(" ");
				Log.record.AppendLine(DateTime.Now.ToShortTimeString());
				Log.record.AppendLine(record);
			}
		}

		/// <summary>
		/// 添加日志
		/// </summary>
		/// <param name="record">日志</param>
		public static void AddLog(string log)
		{
			lock (Log.log)
			{
				Log.record.AppendLine(DateTime.Now.ToShortTimeString());
				Log.log.AppendLine(log);
			}
		}
		
		/// <summary>
		/// 定时器计时
		/// </summary>
		private static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			Log.Save();
		}

		/// <summary>
		/// 保存
		/// </summary>
		private static void Save()
		{
			if (!Directory.Exists(HttpRuntime.AppDomainAppPath + "log"))
			{
				Directory.CreateDirectory(HttpRuntime.AppDomainAppPath + "log");
			}

			lock (Log.record)
			{
				using (StreamWriter writer = new StreamWriter(File.OpenWrite(HttpRuntime.AppDomainAppPath + "log/" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt")))
				{
					try
					{
						writer.BaseStream.Seek(0, SeekOrigin.End);
						writer.Write(Log.record.ToString());
						Log.record = new StringBuilder();
					}

					catch (Exception)
					{
					}
				}
			}

			lock (Log.log)
			{
				using (StreamWriter writer = new StreamWriter(File.OpenWrite(HttpRuntime.AppDomainAppPath + "log/log_" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt")))
				{
					try
					{
						writer.BaseStream.Seek(0, SeekOrigin.End);
						writer.Write(Log.log.ToString());
						Log.log = new StringBuilder();
					}

					catch (Exception)
					{
					}
				}
			}
		}

		/// <summary>
		/// 定时器
		/// </summary>
		private static System.Timers.Timer timer;

		/// <summary>
		/// 记录
		/// </summary>
		private static StringBuilder record = new StringBuilder();

		/// <summary>
		/// 日志
		/// </summary>
		private static StringBuilder log = new StringBuilder();
	}
}