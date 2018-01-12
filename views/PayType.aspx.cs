using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gmt
{
    public partial class PayType : AGmPage
    {
        public PayType()
            : base(PrivilegeType.PayType)
        {
        }

        /// <summary>
        /// 页面载入响应
        /// </summary>
        protected override void OnGmPageLoad()
        {
            string appId = Request.QueryString["AppId"];
            if (appId != null && appId != "")
            {
                string payType = PayType.GetPayTypeByAppId(appId);
                Response.Write(payType);
                Response.End();
                return;
            }
        }

        [WebMethod(EnableSession = true)]
        public static string GetPayType()
        {
            payTypeList.Clear();
            int idx = 0;
            while (true)
            {
                string pt = CUtils.ReadIniValue("Pay_Type_Info", "pt" + idx, "", Global.ConfigPath + "config.ini");
                if (pt != "")
                {
                    PayTypeInfo pti = new PayTypeInfo();
                    pti.payTypeValue = idx;
                    pti.payTypeDesc = pt;
                    payTypeList.Add(pti);
                    idx++;
                }
                else
                    break;
            }

            if (payTypeList.Count > 0)
                return JsonConvert.SerializeObject(payTypeList);
            else
                return "[{\"error\":1}]";
        }

        [WebMethod(EnableSession = true)]
        public static string QueryPayTypeData()
        {
            return JsonConvert.SerializeObject(payInfoDic.Values);
        }

        [WebMethod(EnableSession = true)]
        public static string SetPayType(int optType, string packageName, string appId, int payType, string exParam, string exParam2)
        {
            if (optType == 0)
            {
                //添加
                if (payInfoDic.ContainsKey(appId))
                {
                    return "[{\"error\":\"1\"}]";
                }
                string exStr = payType == 1 ? exParam : "";
                payInfoDic.Add(appId, new PayTypeInfo(packageName, appId, payType, payTypeList[payType].payTypeDesc, exStr, exParam2));
                SavePayType();
            }
            else if (optType == 1)
            {
                //修改
                if (payInfoDic.ContainsKey(appId))
                {
                    payInfoDic[appId].payTypeValue = payType;
                    payInfoDic[appId].payTypeDesc = payTypeList[payType].payTypeDesc;
                    payInfoDic[appId].exParam = exParam;
                    payInfoDic[appId].exParam2 = exParam2;
                    SavePayType();
                }
                else
                {
                    return "[{\"error\":\"1\"}]";
                }
            }
            else if (optType == 2)
            {
                //删除
                if (payInfoDic.ContainsKey(appId))
                {
                    payInfoDic.Remove(appId);
                    SavePayType();
                }
                else
                {
                    return "[{\"error\":\"1\"}]";
                }
            }

            return JsonConvert.SerializeObject(payInfoDic.Values);
        }

        public static string GetPayTypeByAppId(string appid)
        {
            if (payInfoDic.ContainsKey(appid)
                && payInfoDic[appid].payTypeValue != 0)
            {
                return payInfoDic[appid].payTypeValue + "," + payInfoDic[appid].exParam + "," + payInfoDic[appid].exParam2;
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 存储支付方式列表
        /// </summary>
        public static void SavePayType()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)payInfoDic.Count());

                foreach (var pi in payInfoDic.Values)
                {
                    writer.Write(pi.packageName);
                    writer.Write(pi.appId);
                    writer.Write(pi.payTypeValue);
                    writer.Write(pi.payTypeDesc);
                    writer.Write(pi.exParam);
                }

                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string filePath = Global.ConfigPath + "PayType.bytes";
                using (FileStream fileStream = File.Create(filePath))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }

            }
        }

        /// <summary>
        /// 读取支付方式列表
        /// </summary>
        public static void LoadPayType()
        {
            string path = Global.ConfigPath + "PayType.bytes";
            if (File.Exists(path))
            {
                payInfoDic.Clear();
                using (FileStream fileStream = File.OpenRead(path))
                {
                    byte[] buffer = new byte[fileStream.Length];
                    fileStream.Read(buffer, 0, buffer.Length);

                    for (int i = 0; i < fileStream.Length; ++i)
                    {
                        buffer[i] = (byte)(buffer[i] ^ 0x37);
                    }

                    using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                    {
                        ushort count = reader.ReadUInt16();
                        for (int i = 0; i < count; ++i)
                        {
                            PayTypeInfo pti = new PayTypeInfo();
                            pti.packageName = reader.ReadString();
                            pti.appId = reader.ReadString();
                            pti.payTypeValue = reader.ReadInt32();
                            pti.payTypeDesc = reader.ReadString();
                            pti.exParam = reader.ReadString();

                            payInfoDic.Add(pti.appId, pti);
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 支付方式列表
        /// </summary>
        private static List<PayTypeInfo> payTypeList = new List<PayTypeInfo>();

        /// <summary>
        /// 支付信息列表<APPID,支付信息>
        /// </summary>
        private static Dictionary<string, PayTypeInfo> payInfoDic = new Dictionary<string, PayTypeInfo>();
    }

    class PayTypeInfo
    {
        public string packageName;
        public string appId;
        public int payTypeValue;
        public string payTypeDesc;
        public string exParam;
        public string exParam2;

        public PayTypeInfo()
        {
            packageName = "";
            appId = "";
            payTypeValue = -1;
            payTypeDesc = "";
            exParam = "";
            exParam2 = "";
        }

        public PayTypeInfo(string pkgName, string pkgType, int payType, string payDesc, string exparam, string exparam2)
        {
            packageName = pkgName;
            appId = pkgType;
            payTypeValue = payType;
            payTypeDesc = payDesc;
            exParam = exparam;
            exParam2 = exparam2;
        }

    }

}