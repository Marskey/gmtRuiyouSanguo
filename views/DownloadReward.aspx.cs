using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
	/// <summary>
	/// 下载奖励
	/// </summary>
	public partial class DownloadReward : System.Web.UI.Page
	{
		/// <summary>
		/// 页面载入响应
		/// </summary>
		protected void Page_Load(object sender, EventArgs e)
		{
			Response.ContentType = "application/x-msdownload";
			string filename = "attachment; filename=" + "RewardExConfig.protodata.bytes";
			Response.AddHeader("Content-Disposition", filename);

			Response.BinaryWrite(TableManager.Serialize(RandTable.RandList));
		}
	}
}