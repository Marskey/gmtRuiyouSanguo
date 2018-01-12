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
    /// 下载活动
    /// </summary>
    public partial class DownloadActivity : System.Web.UI.Page
    {
        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/x-msdownload";
            string filename = "attachment; filename=" + "my_active_save.bytes";
            Response.AddHeader("Content-Disposition", filename);

            string[] filepathSet =
			{
				HttpRuntime.AppDomainAppPath + "protodatas/ActivityExConfig.protodata.bytes",
				HttpRuntime.AppDomainAppPath + "protodatas/AchieveExConfig.protodata.bytes",
				HttpRuntime.AppDomainAppPath + "protodatas/RewardExConfig.protodata.bytes",
			};

            foreach (var filepath in filepathSet)
            {
                using (var stream = File.OpenRead(filepath))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    Response.BinaryWrite(BitConverter.GetBytes(buffer.Length));
                    Response.BinaryWrite(buffer);
                }
            }
        }
    }
}