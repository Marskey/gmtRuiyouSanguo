using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace gmt
{
    /// <summary>
    /// FTP功能对象
    /// </summary>
    public static class FTPManager
    {
        static string errorText = "";
        static Dictionary<int, FTP> m_dictFTP = new Dictionary<int, FTP>();

        public static void AddFTP(int idx, FTP ftp)
        {
            if (m_dictFTP.ContainsKey(idx))
            {
                m_dictFTP[idx] = ftp;
            }
            else
            {
                m_dictFTP.Add(idx, ftp);
            }
        }

        public static bool RemoveFTP(int idx)
        {
            if (m_dictFTP.ContainsKey(idx))
            {
                m_dictFTP.Remove(idx);
                return true;
            }
            return false;
        }

        public static void Clear()
        {
            m_dictFTP.Clear();
        }

        public static FTP GetFTP(int idx)
        {
            if (m_dictFTP.ContainsKey(idx))
            {
                return m_dictFTP[idx];
            }
            else
            {
                return null;
            }
        }

        public static int Count
        {
            get
            {
                return m_dictFTP.Count;
            }
        }

        public static void MakeDirectory(string directory)
        {
            foreach (var ftp in m_dictFTP)
            {
                ftp.Value.MakeDirectory(directory);
            }
        }

        public static bool Upload(string file, byte[] buffer)
        {
            errorText = "";
            bool ret = true;
            foreach (var ftp in m_dictFTP)
            {
                if (!ftp.Value.Upload(file, buffer))
                {
                    ret = false;
                    errorText += string.Format("ftpSite: {0} upload file[{1}] failed!<br>", ftp.Value.ftpSite, file);
                }
            }
            return ret;
        }

        public static void Save()
        {
            using (BinaryWriter writer = new BinaryWriter(new MemoryStream()))
            {
                writer.Write((ushort)m_dictFTP.Count());
                foreach (var pair in m_dictFTP)
                {
                    FTP ftp = pair.Value;
                    writer.Write(ftp.ftpSite);
                    writer.Write(ftp.ftpUser);
                    writer.Write(ftp.ftpPassword);
                }
                byte[] buffer = (writer.BaseStream as MemoryStream).GetBuffer();

                for (int i = 0; i < writer.BaseStream.Length; ++i)
                {
                    buffer[i] = (byte)(buffer[i] ^ 0x37);
                }

                string configBinaryFile = Global.ConfigPath + "FTP.bytes";

                using (FileStream fileStream = File.Create(configBinaryFile))
                {
                    fileStream.Write(buffer, 0, (int)writer.BaseStream.Length);
                }
            }

        }

        public static void Load()
        {
            string path = Global.ConfigPath + "FTP.bytes";
            if (File.Exists(path))
            {
                m_dictFTP.Clear();
                using (FileStream stream = File.OpenRead(path))
                {
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);

                    for (int i = 0; i < stream.Length; ++i)
                    {
                        buffer[i] = (byte)(buffer[i] ^ 0x37);
                    }

                    using (BinaryReader reader = new BinaryReader(new MemoryStream(buffer)))
                    {
                        ushort count = reader.ReadUInt16();
                        for (int i = 0; i < count; ++i)
                        {
                            FTP ftp = new FTP(reader.ReadString()
                                , reader.ReadString()
                                , reader.ReadString()
                                );

                            m_dictFTP.Add(i, ftp);
                        }
                    }
                }
            }
        }

        public static string GetLastError()
        {
            return errorText;
        }
    }

    public class FTP
    {

        #region 常量定义

        /// <summary>
        /// 写入长度
        /// </summary>
        public const int WriteLength = 1024;

        #endregion
        #region 变量

        /// <summary>
        /// 站点
        /// </summary>
        public string ftpSite = "";

        /// <summary>
        /// 用户名
        /// </summary>
        public string ftpUser = "";

        /// <summary>
        /// 密码
        /// </summary>
        public string ftpPassword = "";

        #endregion


        #region 对外方法

        /// <summary>
        /// 构造方法
        /// </summary>
        public FTP(string site, string user, string pwd)
        {
            ftpSite = site;
            ftpUser = user;
            ftpPassword = pwd;
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directory">文件夹</param>
        public void MakeDirectory(string directory)
        {
            FtpWebRequest request = null;
            FtpWebResponse response = null;

            try
            {
                request = FtpWebRequest.Create(ftpSite + directory) as FtpWebRequest;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.KeepAlive = false;
                request.Method = WebRequestMethods.Ftp.MakeDirectory;

                response = request.GetResponse() as FtpWebResponse;
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
            }

            finally
            {
                if (request != null) { request.Abort(); }
                if (response != null) { response.Close(); }
            }
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="buffer">缓冲区</param>
        /// <returns>是否成功</returns>
        public bool Upload(string file, byte[] buffer)
        {
            FtpWebRequest request = null;
            Stream stream = null;

            try
            {
                request = FtpWebRequest.Create(ftpSite + file) as FtpWebRequest;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.KeepAlive = false;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.ContentLength = FTP.WriteLength;

                stream = request.GetRequestStream();

                for (int i = 0; i < buffer.Length; i += FTP.WriteLength)
                {
                    stream.Write
                    (
                        buffer,
                        i,
                        Math.Min(FTP.WriteLength, buffer.Length - i)
                    );
                }

                stream.Close();
                stream = null;

                return true;
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                return false;
            }

            finally
            {
                if (stream != null) { stream.Close(); }
                if (request != null) { request.Abort(); }
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>缓冲区</returns>
        /// 返回数据长度
        //public int Download(string file, out byte[] buffer)
        //{
        //    buffer = null;
        //    int length = this.FileSize(file);
        //    if (length < 0) { return 0; }

        //    FtpWebRequest	request		= null;
        //    FtpWebResponse	response	= null;
        //    Stream			stream		= null;

        //    try
        //    {
        //        request = FtpWebRequest.Create(ftpSite + file) as FtpWebRequest;
        //        request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
        //        request.KeepAlive = false;
        //        request.Method = WebRequestMethods.Ftp.DownloadFile;
        //        request.UseBinary = true;

        //        response = request.GetResponse() as FtpWebResponse;
        //        stream = response.GetResponseStream();

        //        buffer = new byte[length];
        //        stream.Read(buffer, 0, buffer.Length);

        //        return buffer.Length;
        //    }

        //    catch (Exception exception)
        //    {
        //        DatabaseAssistant.ReportException(exception);
        //        return 0;
        //    }

        //    finally
        //    {
        //        if (stream != null) { stream.Close(); }
        //        if (response != null) { response.Close(); }
        //        if (request != null) { request.Abort(); }
        //    }
        //}

        /// <summary>
        /// 文件尺寸
        /// </summary>
        /// <param name="file">文件名</param>
        /// <returns>尺寸</returns>
        public int FileSize(string file)
        {
            FtpWebRequest request = null;
            FtpWebResponse response = null;

            try
            {
                request = FtpWebRequest.Create(ftpSite + file) as FtpWebRequest;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.KeepAlive = false;
                request.Method = WebRequestMethods.Ftp.GetFileSize;
                request.UseBinary = true;

                response = request.GetResponse() as FtpWebResponse;

                return (int)response.ContentLength;
            }

            catch (Exception exception)
            {
                DatabaseAssistant.ReportException(exception);
                return -1;
            }

            finally
            {
                if (response != null) { response.Close(); }
                if (request != null) { request.Abort(); }
            }
        }

        #endregion
    }
}