using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
	/// <summary>
	/// 下载礼包
	/// </summary>
	public partial class DownloadGift : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.ContentType = "application/x-msdownload";
			string filename = "attachment; filename=" + "GiftExConfig.protodata.bytes";
			Response.AddHeader("Content-Disposition", filename);

			Response.BinaryWrite(TableManager.Serialize(GiftTable.GiftList));
		}
	}
}