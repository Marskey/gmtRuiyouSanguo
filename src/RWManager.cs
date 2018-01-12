using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace gm
{
    public class RWManager
    {
        //读文件
        public static List<T> Load<T>(string path) where T : class, global::ProtoBuf.IExtensible
        {
            List<T> config = new List<T>();

            string Path = HttpRuntime.AppDomainAppPath + path;
            if (File.Exists(Path))
            {
                using (FileStream stream = File.OpenRead(Path))
                {   
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    ProtoData<T> ConfigSet = new ProtoData<T>(buffer);
                    for (int i = 0; i < ConfigSet.Count; ++i)
                    {
                        config.Add(ConfigSet[i]);
                    }
                }
            }
            else
            {
                return null;
            }
            return config;
        }


        //写文件
        public static void Save<T>(string path, List<T> t) where T : class, global::ProtoBuf.IExtensible
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter br = new BinaryWriter(ms);
			string name = typeof(T).FullName;
            br.Write(name);
            uint count = (uint)t.Count;
            br.Write(count);

			mw_serializer0 serializer = new mw_serializer0();
            for (int i = 0; i < (uint)t.Count; i++)
            {
                MemoryStream stream = new MemoryStream();
			  //ProtoSerializer.Instance.Serialize(stream, t[i]);
				serializer.Serialize(stream, t[i]);
                byte[] Byte = stream.GetBuffer();
				br.Write((int)stream.Length);
				br.Write(Byte, 0, (int)stream.Length);
            }

			byte[] buffer = ms.GetBuffer();
            string configBinaryFile = HttpRuntime.AppDomainAppPath + path;
            using (FileStream fileStream = File.Create(configBinaryFile))
            {
                fileStream.Write(buffer, 0, (int)br.BaseStream.Length);
            }

            return;
        }


        //测试数据
        public static void test()
        {
			List<mw.ActivityConfig> list = new List<mw.ActivityConfig>();
			list.Add(new mw.ActivityConfig());
			RWManager.Save<mw.ActivityConfig>("Activity1.bytes", list);

            //RWManager.Load<mw.ActivityConfig>("ActivityConfig.protodata.bytes");
			RWManager.Load<mw.ActivityConfig>("Activity1.bytes");
        }



    }
}