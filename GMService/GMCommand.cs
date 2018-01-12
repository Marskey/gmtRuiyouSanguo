using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace GMService
{
	/// <summary>
	/// GM命令
	/// </summary>
	static class GMCommand
	{
		/// <summary>
		/// 执行GM命令
		/// </summary>
		/// <param name="address">发送地址</param>
		/// <param name="serverId">服务器编号</param>
		/// <param name="playerId">玩家编号</param>
		/// <param name="command">命令</param>
		/// <param name="operate">操作</param>
		/// <returns>是否成功</returns>
		public static bool Execute(string address, string serverId, string playerId, string command, string operate)
		{
			return GMCommand.Execute(address, serverId, playerId, Encoding.UTF8.GetBytes(command), Encoding.UTF8.GetBytes(operate));
		}
		
		/// <summary>
		/// 执行GM命令
		/// </summary>
		/// <param name="address">发送地址</param>
		/// <param name="serverId">服务器编号</param>
		/// <param name="playerId">玩家编号</param>
		/// <param name="command">命令</param>
		/// <param name="operate">操作</param>
		/// <returns>是否成功</returns>
		public static bool Execute(string address, string serverId, string playerId, byte[] command, byte[] operate)
		{
			HttpWebRequest	request = null;
			HttpWebResponse	respone = null;

			try
			{
				request = WebRequest.Create(address) as HttpWebRequest;
				request.KeepAlive = false;
				request.Headers["svr"] = serverId;
				request.Headers["uid"] = playerId;
				request.Headers["cmd"] = Convert.ToBase64String(command);
				request.Headers["opt"] = Convert.ToBase64String(operate);
				request.Timeout = 10000;

				respone = request.GetResponse() as HttpWebResponse;

				Log.AddLog(serverId + "\r\n" + Encoding.UTF8.GetString(command) + "\r\n成功");
				return true;
			}

			catch (Exception exception)
			{
				Log.AddLog(address + "\r\n" + serverId + "\r\n" + exception.ToString());
				return false;
			}

			finally
			{
				if (request != null) { request.Abort(); }
				if (respone != null) { respone.Close(); }
			}
		}
	}
}