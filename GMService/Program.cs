using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace GMService
{
	/// <summary>
	/// 应用程序
	/// </summary>
	static class Program
	{
		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		static void Main()
		{
			GMService service = new GMService();

			if (Environment.UserInteractive)
			{
				service.Start();

				Console.ReadKey();

				service.Stop();
			}
			else
			{
				ServiceBase.Run(new ServiceBase[] { service });
			}
		}
	}
}