using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Config
{
	/// <summary>
	/// 应用程序
	/// </summary>
	class Program
	{
		/// <summary>
		/// 主入口
		/// </summary>
		/// <param name="args">启动参数</param>
		static void Main(string[] args)
		{
			string[] textFileSet =
			{
				AppDomain.CurrentDomain.BaseDirectory + "Config.txt",
				AppDomain.CurrentDomain.BaseDirectory + "BillConfig.txt",
			};
			string[] binaryFileSet =
			{
				AppDomain.CurrentDomain.BaseDirectory + "Config.bytes",
				AppDomain.CurrentDomain.BaseDirectory + "BillConfig.bytes",
			};

			for (int i = 0; i < textFileSet.Length; ++i)
			{
				if (!File.Exists(textFileSet[i]))
				{
					Console.WriteLine("找不到配置文件（{0}）", textFileSet[i]);
					continue;
				}

				try
				{
					using (StreamReader reader = new StreamReader(File.OpenRead(textFileSet[i])))
					{
						string content = reader.ReadToEnd();
						byte[] buffer = Encoding.UTF8.GetBytes(content);

						for (int j = 0; j < buffer.Length; ++j)
						{
							buffer[j] = (byte)(buffer[j] ^ 0x37);
						}

						using (FileStream stream = File.Create(binaryFileSet[i]))
						{
							stream.Write(buffer, 0, buffer.Length);
						}
					}
				}

				catch (Exception exception)
				{
					Console.WriteLine(exception.ToString());
				}
			}

			Program.Exit();
		}

		/// <summary>
		/// 退出
		/// </summary>
		static void Exit()
		{
			Console.Write("按任意键退出");
			Console.ReadKey(true);
		}
	}
}