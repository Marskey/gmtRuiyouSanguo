using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
	/// <summary>
	/// 下载成就
	/// </summary>
	public partial class DownloadAchieve : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.ContentType = "application/x-msdownload";
			string filename = "attachment; filename=" + "AchieveExConfig.protodata.bytes";
			Response.AddHeader("Content-Disposition", filename);
			string filepath = HttpRuntime.AppDomainAppPath + "AchieveExConfig.protodata.bytes";
			Response.TransmitFile(filepath);
		}
	}
}