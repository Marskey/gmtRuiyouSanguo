using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
	/// <summary>
	/// 下载CDN的表格zip包
	/// </summary>
	public partial class DownloadCDNTableZip : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			if (ServerListConfig.DataList.Count == 0) { return; }

			Response.ContentType = "application/x-msdownload";
			string filename = "attachment; filename=" + "GMTActivityTable.zip";
			Response.AddHeader("Content-Disposition", filename);
			Response.BinaryWrite(CdnZip.PackTable(Request.Params["version"]));
		}
	}
}