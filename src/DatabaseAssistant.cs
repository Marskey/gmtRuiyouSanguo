using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace gmt
{
	/// <summary>
	/// 数据库助手
	/// </summary>
	public static class DatabaseAssistant
	{
		/// <summary>
		/// 连接文本
		/// </summary>
		private const string ConnectionText = "Data Source={0};PORT={1};charset={2};Database={3};User Id={4};Password={5};Allow User Variables=True;";

		/// <summary>
		/// 执行SQL文本
		/// </summary>
		/// <param name="resultProcess">执行结果处理</param>
		/// <param name="address">地址</param>
		/// <param name="port">端口</param>
		/// <param name="charSet">字符集</param>
		/// <param name="database">数据库</param>
		/// <param name="userId">用户</param>
		/// <param name="password">密码</param>
		/// <param name="sqlText">SQL文本</param>
		/// <param name="parameter">参数</param>
		/// <returns>是否成功</returns>
		public static bool Execute(Action<MySql.Data.MySqlClient.MySqlDataReader> resultProcess, string address, string port, string charSet, string database, string userId, string password, string sqlText, params object[] parameter)
		{
			return DatabaseAssistant.DoExecute
			(
				() =>
				{
					MySql.Data.MySqlClient.MySqlDataReader reader = command.ExecuteReader();
                    int n = reader.FieldCount;

					try
					{
						resultProcess(reader);
					}

					catch (Exception exception)
					{
						DatabaseAssistant.ReportException(exception);
					}

					reader.Close();
				},
				string.Format(DatabaseAssistant.ConnectionText, address, port, charSet, database, userId, password),
				sqlText,
				parameter
			);
		}

		/// <summary>
		/// 执行SQL文本
		/// </summary>
		/// <param name="address">地址</param>
		/// <param name="port">端口</param>
		/// <param name="charSet">字符集</param>
		/// <param name="database">数据库</param>
		/// <param name="userId">用户</param>
		/// <param name="password">密码</param>
		/// <param name="sqlText">SQL文本</param>
		/// <param name="parameter">参数</param>
		/// <returns>是否成功</returns>
		public static bool Execute(string address, string port, string charSet, string database, string userId, string password, string sqlText, params object[] parameter)
		{
			return DatabaseAssistant.DoExecute
			(
				() => { command.ExecuteNonQuery(); },
				string.Format(DatabaseAssistant.ConnectionText, address, port, charSet, database, userId, password),
				sqlText,
				parameter
			);
		}

		/// <summary>
		/// 报告异常
		/// </summary>
		/// <param name="exception">异常</param>
		public static void ReportException(Exception exception)
		{
			Log.AddLog(exception.ToString());
            //string description = DatabaseAssistant.DetectionSql(exception.Message + "\r\n" + exception.StackTrace);
            //DatabaseAssistant.Execute
            //(
            //    "INSERT INTO `log`(`Type`,`Description`) VALUES({0},'{1}');",
            //    0,
            //    description.Replace("\r\n", "\\r\\n")
            //);
		}

		/// <summary>
		/// 检测SQL文本
		/// </summary>
		/// <param name="sqlText">SQL文本</param>
		/// <returns>检测后的文本</returns>
		public static string DetectionSql(string sqlText)
		{
			return sqlText.Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\"", "\\\"");
		}

		/// <summary>
		/// 执行SQL文本
		/// </summary>
		/// <param name="executeProcess">执行处理</param>
		/// <param name="connectionText">连接文本</param>
		/// <param name="sqlText">SQL文本</param>
		/// <param name="parameter">参数</param>
		/// <returns>是否成功</returns>
		private static bool DoExecute(Action executeProcess, string connectionText, string sqlText, params object[] parameter)
		{
			lock (DatabaseAssistant.connection)
			{
				try
				{
					if (!DatabaseAssistant.IsConnected() || DatabaseAssistant.connection.ConnectionString != connectionText)
					{
						if (DatabaseAssistant.IsConnected()) { DatabaseAssistant.connection.Close(); }
						DatabaseAssistant.connection.ConnectionString = connectionText;
						DatabaseAssistant.connection.Open();
					}

					DatabaseAssistant.command.CommandText = parameter != null ? string.Format(sqlText, parameter) : sqlText;

					DatabaseAssistant.command.CommandTimeout = 3600;

					executeProcess();

					return true;
				}

				catch (Exception exception)
				{
					if (DatabaseAssistant.IsConnected()) { DatabaseAssistant.ReportException(exception); }

					return false;
				}

				finally
				{
					if (DatabaseAssistant.IsConnected()) { DatabaseAssistant.connection.Clone(); }
				}
			}
		}

		/// <summary>
		/// 是否连接
		/// </summary>
		/// <returns>是否连接</returns>
		private static bool IsConnected()
		{
			return (DatabaseAssistant.connection.State & System.Data.ConnectionState.Open) == System.Data.ConnectionState.Open;
		}

		/// <summary>
		/// 静态构造
		/// </summary>
		static DatabaseAssistant()
		{
			DatabaseAssistant.connection = new MySql.Data.MySqlClient.MySqlConnection();
			DatabaseAssistant.command = DatabaseAssistant.connection.CreateCommand();
		}

		/// <summary>
		/// 数据库连接器
		/// </summary>
		private static MySql.Data.MySqlClient.MySqlConnection connection;

		/// <summary>
		/// 数据库指令
		/// </summary>
		private static MySql.Data.MySqlClient.MySqlCommand command;
	}
}