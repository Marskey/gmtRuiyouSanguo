using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.IO;

namespace gmt
{
    public struct STTimedMail
    {
        public int id;
        public int mailId;
        public uint uid;
        public uint sendTime;
        public string cmd;
        public string serverName;
    };

    public static class TimedMailSender
    {
        //private static string _DataPath = HttpRuntime.AppDomainAppPath + 
        private static int _nextId = 0;
        private static System.Timers.Timer _timer;
        private static Dictionary<int, STTimedMail> m_dicTimedMails = new Dictionary<int, STTimedMail>();
        private static bool _dirty = false;

        public static void Init()
        {
            LoadTimedMail();

            if (_timer != null) { return; }
            _timer = new System.Timers.Timer(1000 * 5);
            _timer.Elapsed += OnTimer;
            _timer.Start();
        }

        private static void OnTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<int> sentList = new List<int>();
            lock (m_dicTimedMails)
            {
                uint time = CUtils.GetTimestamp(DateTime.Now);
                int pcnt = 0;

                foreach (var pair in m_dicTimedMails)
                {
                    if (pcnt > 50) { break; }

                    if (time >= pair.Value.sendTime)
                    {
                        if (_SendMail(pair.Value))
                        {
                            sentList.Add(pair.Key);
                        }
                    }

                    pcnt++;
                }

            }

            foreach (var id in sentList)
            {
                RemoveTimedMail(id);
            }

            SaveTimedMail();
        }

        public static bool AddTimedMail(STTimedMail stMail)
        {
            bool bRet = false;
            lock (m_dicTimedMails)
            {
                //if (m_dicTimedMails.Count() <= 10)
                //{
                    int id = _GenMailId();
                    stMail.id = id;
                    m_dicTimedMails.Add(id, stMail);
                    _dirty = true;
                    bRet = true;
                //}
            }
            return bRet;
        }

        public static bool RemoveTimedMail(int id)
        {
            bool bRet = false;
            lock (m_dicTimedMails)
            {
                if (m_dicTimedMails.ContainsKey(id))
                {
                    m_dicTimedMails.Remove(id);
                    _dirty = true;
                    bRet = true;
                }
            }
            return bRet;
        }

        public static void LoadTimedMail()
        {
            string filePath = HttpRuntime.AppDomainAppPath + "configs/TimedMail.bytes";

            if (!File.Exists(filePath)) { return; }

            using (FileStream stream = File.OpenRead(filePath))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                //for (int i = 0; i < stream.Length; ++i)
                //{
                //    buffer[i] = (byte)(buffer[i] ^ 0x37);
                //}

                using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                {
                    string json = reader.ReadString();
                    m_dicTimedMails = JsonConvert.DeserializeObject<Dictionary<int, STTimedMail>>(json);

                    m_dicTimedMails = m_dicTimedMails.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
                    _nextId = m_dicTimedMails.LastOrDefault().Key + 1;
                }
            }
        }

        public static void SaveTimedMail()
        {
            if (_dirty)
            {
                lock (m_dicTimedMails)
                {
                    using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
                    {
                        writer.Write(JsonConvert.SerializeObject(m_dicTimedMails));

                        byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                        //for (int i = 0; i < writer.BaseStream.Length; ++i)
                        //{
                        //    buffer[i] = (byte)(buffer[i] ^ 0x37);
                        //}

                        using (FileStream fileStream = File.Create(HttpRuntime.AppDomainAppPath + "configs/TimedMail.bytes"))
                        {
                            fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                        }

                        _dirty = false;
                    }
                }
            }
        }

        public static string GetMailList()
        {
            string json;
            lock (m_dicTimedMails)
            {
                json = JsonConvert.SerializeObject(m_dicTimedMails.Values);
            }
            return json;
        }

        private static bool _SendMail(STTimedMail stMail)
        {
            Server server = Server.GetServer(stMail.serverName);
            return AGmPage.ExecuteGmCommand("SYSTEM", server, "0", stMail.cmd, "", true, null);
        }

        private static int _GenMailId()
        {
            return _nextId++;
        }
    }
}