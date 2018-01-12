using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
	/// <summary>
	/// 下载公告
	/// </summary>
	public partial class DownloadNotice : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
            //Response.ContentType = "application/x-msdownload";
            //string filename = "attachment; filename=" + "SysNtfExConfig.protodata.bytes";
            //Response.AddHeader("Content-Disposition", filename);

            //Response.BinaryWrite(TableManager.Serialize(NoticeEditData.NoticeList));
		}
	}
}