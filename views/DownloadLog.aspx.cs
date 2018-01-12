using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ICSharpCode.SharpZipLib.Zip;

namespace gmt
{
	/// <summary>
	/// 下载日志页面
	/// </summary>
	public partial class DownloadLog : AGmPage
	{
		/// <summary>
		/// 构造方法
		/// </summary>
		public DownloadLog()
			: base(PrivilegeType.Download)
		{
		}

		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected override void OnGmPageLoad()
		{
			string path = HttpRuntime.AppDomainAppPath + "log";

			if (!Directory.Exists(path))
			{
				this.Response.Write(TableManager.GetGMTText(700));
				return;
			}

			string[] files = Directory.GetFileSystemEntries(path);

			using (var memory = new MemoryStream())
			{
				using (ZipOutputStream stream = new ZipOutputStream(memory))
				{
					foreach (var file in files)
					{
						if (File.Exists(file))
						{
							using (var fileStream = File.OpenRead(file))
							{
								byte[] buffer = new byte[fileStream.Length];
								fileStream.Read(buffer, 0, buffer.Length);

								string fileText = file.Replace('\\', '/');
								fileText = fileText.Substring(fileText.LastIndexOf('/') + 1);
								stream.PutNextEntry(new ZipEntry(fileText));
								stream.Write(buffer, 0, buffer.Length);
							}
						}
					}

					stream.CloseEntry();
					stream.Finish();

					byte[] zipBuffer = new byte[memory.Length];
					Array.Copy(memory.GetBuffer(), zipBuffer, zipBuffer.Length);

					Response.ContentType = "application/x-msdownload";
					string filename = "attachment; filename=" + "log.zip";
					Response.AddHeader("Content-Disposition", filename);
					Response.BinaryWrite(zipBuffer);
				}
			}
		}
	}
}