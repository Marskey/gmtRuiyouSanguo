using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GMService
{
	/// <summary>
	/// 网络
	/// </summary>
	static class Network
	{

#region 对外方法

		/// <summary>
		/// 启动
		/// </summary>
		public static void Start()
		{
			Network.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            string[] configs;

            string configBinaryFile = System.IO.Directory.GetCurrentDirectory() +"/"+ "NetPort.txt";
            //端口读配置
            if (File.Exists(configBinaryFile))
            {
                string contents = System.IO.File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "/" + "NetPort.txt");
                if (!string.IsNullOrEmpty(contents))
                {
                    configs = contents.Split(',');
                    Network.listenSocket.Bind(new IPEndPoint(IPAddress.Parse(configs[0]), int.Parse(configs[1])));
                    Console.WriteLine("监听gmt端口号: " + contents);
                }
            }
            else
            {
                Console.WriteLine("未找到Service端口号配置文件NetPort.txt, 使用默认端口号18885");
                Network.listenSocket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 18885));
            }

			Network.listenSocket.Listen(1);
			Network.listenSocket.BeginAccept(Network.OnAccept, null);
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="writer">写入器</param>
		public static void Send(BinaryWriter writer)
		{
			if (Network.clientSocket == null || !Network.clientSocket.Connected) { return; }

			writer.Seek(0, SeekOrigin.Begin);
			writer.Write((int)writer.BaseStream.Length);

			lock (Network.listenSocket)
			{
				if (Network.sendingWriter != null)
				{
					Network.sendQueue.Enqueue(writer);
				}
				else
				{
					Network.sendingWriter = writer;
					Network.totalSendSize = 0;

					Network.BeginSend();
				}
			}
		}

		/// <summary>
		/// 注册消息处理
		/// </summary>
		/// <param name="type">消息类型</param>
		/// <param name="process">消息处理</param>
		public static void RegisterMessageProcess(ushort type, Action<ushort, BinaryReader> process)
		{
			if (process == null) { return; }

			if (Network.messageProcessDictionary.ContainsKey(type)) { return; }

			Network.messageProcessDictionary.Add(type, process);
		}

#endregion
		
#region 内部方法

		/// <summary>
		/// 接受连接。
		/// </summary>
		/// <param name="result">异步连接结果</param>
		private static void OnAccept(IAsyncResult result)
		{
			lock (Network.listenSocket)
			{
				Network.totalReceiveSize = 0;
				Network.sendQueue.Clear();
				Network.sendingWriter = null;
				Network.totalSendSize = 0;
				Network.clientSocket = Network.listenSocket.EndAccept(result);
				Network.clientSocket.BeginReceive(Network.receiveBuffer, 0, Network.receiveBuffer.Length, SocketFlags.None, Network.OnReceive, null);
			}

			Network.listenSocket.BeginAccept(Network.OnAccept, null);

            Console.WriteLine("gmt  连接成功");

		}

		/// <summary>
		/// 接收数据
		/// </summary>
		/// <param name="result">异步连接结果</param>
		private static void OnReceive(IAsyncResult result)
		{
			lock (Network.listenSocket)
			{
				try
				{
					if (!Network.clientSocket.Connected)
					{
						Network.clientSocket = null;
						return;
					}

					SocketError	error;
					int			receiveSize;

					if ((receiveSize = Network.clientSocket.EndReceive(result, out error)) > 0)
					{
						if (error == SocketError.Success)
						{
							Network.totalReceiveSize += receiveSize;

							while (Network.totalReceiveSize >= sizeof(int))
							{
								int length = BitConverter.ToInt32(Network.receiveBuffer, 0);

								if (length <= Network.totalReceiveSize)
								{
									Network.DoReceive();
									Network.totalReceiveSize -= length;

									if (Network.totalReceiveSize != 0)
									{
										Array.Copy(Network.receiveBuffer, length, Network.receiveBuffer, 0, Network.totalReceiveSize);
									}
								}
								else
								{
									if (length > Network.receiveBuffer.Length)
									{
										byte[] buffer = new byte[length];
										Array.Copy(Network.receiveBuffer, buffer, Network.receiveBuffer.Length);
										Network.receiveBuffer = buffer;
									}
									break;
								}
							}

							Network.clientSocket.BeginReceive(Network.receiveBuffer, Network.totalReceiveSize, Network.receiveBuffer.Length - Network.totalReceiveSize, SocketFlags.None, Network.OnReceive, null);
						}
					}
					else
					{
						Network.clientSocket.Shutdown(SocketShutdown.Both);
						Network.clientSocket.Close();
						Network.clientSocket = null;
						return;
					}

					switch (error)
					{
						case SocketError.ConnectionReset:
						case SocketError.ConnectionAborted:
							Network.clientSocket = null;
							break;
					}
				}

				catch (SocketException exception)
				{
					switch (exception.ErrorCode)
					{
						// 您的主机中的软件中止了一个已建立的连接
						case 10053:
							Network.clientSocket = null;
							break;
					}
				}

				// 已关闭 Safe handle
				catch (ObjectDisposedException)
				{
					Network.clientSocket = null;
				}

				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// 发送数据响应
		/// </summary>
		/// <param name="result">异步发送结果</param>
		private static void OnSend(IAsyncResult result)
		{
			lock (Network.listenSocket)
			{
				try
				{
					SocketError	error;
					int			sendSize = Network.clientSocket.EndSend(result, out error);

					if (sendSize > 0) { Network.totalSendSize += sendSize; }

					switch (error)
					{
						case SocketError.Success:
							if (Network.totalSendSize == Network.sendingWriter.BaseStream.Length)
							{
								Network.sendingWriter.Close();
								Network.totalSendSize = 0;

								if (Network.sendQueue.Count > 0)
								{
									Network.sendingWriter = Network.sendQueue.Dequeue();
									Network.BeginSend();
								}
								else
								{
									Network.sendingWriter = null;
								}
							}
							break;

						case SocketError.ConnectionAborted:
						case SocketError.ConnectionReset:
						case SocketError.ConnectionRefused:
							Network.clientSocket = null;
							break;
					}
				}

				catch (SocketException)
				{
				}

				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// 发送
		/// </summary>
		private static void BeginSend()
		{
			SocketError	error		= SocketError.Success;
			int			sendSize	= (int)Network.sendingWriter.BaseStream.Length - Network.totalSendSize;

			Network.clientSocket.BeginSend((Network.sendingWriter.BaseStream as MemoryStream).GetBuffer(), Network.totalSendSize, sendSize, SocketFlags.None, out error, Network.OnSend, null);

			switch (error)
			{
				case SocketError.Success:
					break;

				case SocketError.ConnectionReset:
					Network.clientSocket = null;
					break;
			}
		}

		/// <summary>
		/// 接收数据
		/// </summary>
		private static void DoReceive()
		{
			lock (Network.listenSocket)
			{
				try
				{
					Action<ushort, BinaryReader> process;
					if (Network.messageProcessDictionary.TryGetValue(BitConverter.ToUInt16(Network.receiveBuffer, sizeof(int)), out process))
					{
						using (var reader = new BinaryReader(new MemoryStream(Network.receiveBuffer)))
						{
							reader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
							ushort message = reader.ReadUInt16();
                            Console.WriteLine(string.Format("接收消息:{0}", message));
							Log.AddLog(string.Format("接收消息:{0}", message));
							process(message, reader);
						}
					}
					else
					{
						Log.AddLog("接收到未知消息");
					}
				}

				catch (Exception exception)
				{
					Log.AddError(exception.ToString());
				}
			}
		}

#endregion

#region 变量

		/// <summary>
		/// 监听套接字
		/// </summary>
		private static Socket listenSocket;

		/// <summary>
		/// 客户端套接字
		/// </summary>
		private static Socket clientSocket;

		/// <summary>
		/// 接收缓冲区
		/// </summary>
		private static byte[] receiveBuffer = new byte[1024];

		/// <summary>
		/// 总共接收尺寸
		/// </summary>
		private static int totalReceiveSize;

		/// <summary>
		/// 消息处理字典
		/// </summary>
		private static Dictionary<ushort, Action<ushort, BinaryReader>> messageProcessDictionary = new Dictionary<ushort, Action<ushort, BinaryReader>>();

		/// <summary>
		/// 发送队列
		/// </summary>
		private static Queue<BinaryWriter> sendQueue = new Queue<BinaryWriter>();

		/// <summary>
		/// 正在发送流
		/// </summary>
		private static BinaryWriter sendingWriter;

		/// <summary>
		/// 总共发送尺寸
		/// </summary>
		private static int totalSendSize;

#endregion
		
	}
}