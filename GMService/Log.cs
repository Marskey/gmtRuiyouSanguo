using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GMService
{
	/// <summary>
	/// 日志
	/// </summary>
	static class Log
	{
		
#region 常量定义

		/// <summary>
		/// 日志文件夹
		/// </summary>
		private static readonly string Directory = System.AppDomain.CurrentDomain.BaseDirectory + "Log/";

#endregion

#region 对外方法

		/// <summary>
		/// 添加日志
		/// </summary>
		/// <param name="context">日志内容</param>
		public static void AddLog(string context)
		{
			lock (Log.Directory)
			{
				if (!System.IO.Directory.Exists(Log.Directory))
				{
					System.IO.Directory.CreateDirectory(Log.Directory);
				}

				using (var writer = File.AppendText(Log.Directory + DateTime.Today.ToString("yyyy-MM-dd") + ".txt"))
				{
					writer.WriteLine(DateTime.Now.ToShortTimeString());
					writer.WriteLine(context);
					writer.WriteLine();
				}
			}
		}

		/// <summary>
		/// 添加错误
		/// </summary>
		/// <param name="error">错误</param>
		public static void AddError(string error)
		{
			lock (Log.Directory)
			{
				if (!System.IO.Directory.Exists(Log.Directory))
				{
					System.IO.Directory.CreateDirectory(Log.Directory);
				}

				using (var writer = File.AppendText(Log.Directory + "error_" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt"))
				{
					writer.WriteLine(DateTime.Now.ToShortTimeString());
					writer.WriteLine(error);
					writer.WriteLine();
				}
			}
		}

#endregion

	}
}