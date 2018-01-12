using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Web;

namespace gmt
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
			lock (Network.messageProcessDictionary)
			{
				if (Network.socket == null)
				{
					Network.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    string[] config;

                    string configBinaryFile = HttpRuntime.AppDomainAppPath +"NetPort.txt";
                    //端口读配置
                    if (File.Exists(configBinaryFile))
                    {
                        Log.AddLog("找到gm端口号配置文件NetPort.txt");
                        string contents = System.IO.File.ReadAllText(HttpRuntime.AppDomainAppPath + "NetPort.txt");
                        if (!string.IsNullOrEmpty(contents))
                        {
                            config = contents.Split(',');
                            Network.socket.BeginConnect(new IPEndPoint(IPAddress.Parse(config[0]), int.Parse(config[1])), Network.OnConnect, null);
                        }
                    }
                    else
                    {
                        Network.socket.BeginConnect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 18885), Network.OnConnect, null);
                    }

					Thread.Sleep(1000);
				}
			}
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <param name="writer">写入器</param>
		/// <returns>是否成功</returns>
		public static bool Send(BinaryWriter writer)
		{
			lock (Network.messageProcessDictionary)
			{
				if (Network.socket == null) { Network.Start(); }

				if (Network.socket == null || !Network.socket.Connected) { return false; }

				writer.Seek(0, SeekOrigin.Begin);
				writer.Write((int)writer.BaseStream.Length);

				if (Network.sendingWriter == null)
				{
					Network.sendingWriter = writer;
					Network.totalSendSize = 0;

					Network.BeginSend();
				}
				else
				{
					Network.sendQueue.Enqueue(writer);
				}

				return true;
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
		/// 建立连接响应
		/// </summary>
		/// <param name="result">异步连接结果</param>
		private static void OnConnect(IAsyncResult result)
		{
			lock (Network.messageProcessDictionary)
			{
				try
				{
					Network.socket.EndConnect(result);
					Network.socket.BeginReceive(Network.receiveBuffer, 0, Network.receiveBuffer.Length, SocketFlags.None, Network.OnReceive, null);
				}

				catch (SocketException exception)
				{
					switch (exception.ErrorCode)
					{
						// 由于连接方在一段时间后没有正确答复或连接的主机没有反应，连接尝试失败。
						case 10060:
						// 由于目标计算机积极拒绝，无法连接。
						case 10061:
						default:
							break;
					}
				}

				catch (Exception)
				{
				}
			}
		}

		/// <summary>
		/// 接收数据响应
		/// </summary>
		/// <param name="result">异步接收结果</param>
		private static void OnReceive(IAsyncResult result)
		{
			if (Network.socket == null) { return; }

			lock (Network.messageProcessDictionary)
			{
				try
				{
					if (!Network.socket.Connected)
					{
						Network.socket = null;
						return;
					}

					SocketError	error;
					int			receiveSize;

					if ((receiveSize = Network.socket.EndReceive(result, out error)) > 0)
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
								else if (length > Network.receiveBuffer.Length)
								{
									byte[] buffer = new byte[length];
									Array.Copy(Network.receiveBuffer, buffer, Network.receiveBuffer.Length);
									Network.receiveBuffer = buffer;
									break;
								}
							}

							Network.socket.BeginReceive(Network.receiveBuffer, Network.totalReceiveSize, Network.receiveBuffer.Length - Network.totalReceiveSize, SocketFlags.None, Network.OnReceive, null);
						}
					}
					else
					{
						Network.socket.Shutdown(SocketShutdown.Both);
						Network.socket.Close();
						Network.socket = null;
						return;
					}

					switch (error)
					{
						case SocketError.ConnectionReset:
						case SocketError.ConnectionAborted:
							Network.socket = null;
							break;
					}
				}

				catch (SocketException exception)
				{
					switch (exception.ErrorCode)
					{
						// 您的主机中的软件中止了一个已建立的连接
						case 10053:
							Network.socket = null;
							break;
					}
				}

				// 已关闭 Safe handle
				catch (ObjectDisposedException)
				{
					Network.socket = null;
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
			if (Network.socket == null) { return; }

			lock (Network.messageProcessDictionary)
			{
				try
				{
					SocketError	error;
					int			sendSize = Network.socket.EndSend(result, out error);

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
							Network.socket = null;
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

			Network.socket.BeginSend((Network.sendingWriter.BaseStream as MemoryStream).GetBuffer(), Network.totalSendSize, sendSize, SocketFlags.None, out error, Network.OnSend, null);

			switch (error)
			{
				case SocketError.Success:
					break;

				case SocketError.ConnectionReset:
					Network.socket = null;
					break;
			}
		}

		/// <summary>
		/// 接收数据
		/// </summary>
		private static void DoReceive()
		{
			lock (Network.messageProcessDictionary)
			{
				try
				{
					Action<ushort, BinaryReader> process;
					if (Network.messageProcessDictionary.TryGetValue(BitConverter.ToUInt16(Network.receiveBuffer, sizeof(int)), out process))
					{
						using (var reader = new BinaryReader(new MemoryStream(Network.receiveBuffer)))
						{
							reader.BaseStream.Seek(sizeof(int), SeekOrigin.Begin);
							process(reader.ReadUInt16(), reader);
						}
					}
				}

				catch (Exception)
				{
				}
			}
		}

#endregion

#region 变量

		/// <summary>
		/// 套接字
		/// </summary>
		private static Socket socket;

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