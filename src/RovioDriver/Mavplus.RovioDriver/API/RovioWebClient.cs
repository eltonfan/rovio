using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Drawing;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace Mavplus.RovioDriver.API
{
    internal class WebDownload : WebClient
    {
        /// <summary>
        /// Time in milliseconds
        /// </summary>
        public int Timeout { get; set; }

        public WebDownload(int timeout = 60000)
        {
            this.Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            HttpWebRequest request = base.GetWebRequest(address) as HttpWebRequest;
            if (request != null)
            {
                request.ServicePoint.Expect100Continue = false;
                request.ServicePoint.UseNagleAlgorithm = false;
                request.ServicePoint.ConnectionLimit = 65500;
                request.AllowWriteStreamBuffering = false;
                request.Proxy = null;

                request.Timeout = this.Timeout;
                request.ReadWriteTimeout = this.Timeout;
            }
            return request;
        }
    }
    /// <summary>
    /// Class for performing http requests
    /// </summary>
    internal class RovioWebClient
    {
        readonly Encoding encoding = Encoding.GetEncoding("GB2312");
        /// <summary>
        /// Address to acces Rovio
        /// </summary>
        readonly string rovioAddress;

        /// <summary>
        ///  Network Credential to access Rovio
        /// </summary>
        readonly NetworkCredential rovioCredentials;
   
        /// <summary>
        /// Constructor for RovioWebClient
        /// </summary>
        /// <param name="settings"></param>
        public RovioWebClient(string host, int port, NetworkCredential credentials)
        {
            this.rovioAddress = "http://" + host;
            if (port != 80)
                this.rovioAddress += ":" + port;
            this.rovioCredentials = credentials;
        }

        /// <summary>
        /// Web request for string data to the Rovio API 
        /// </summary>
        /// <param name="cmd">Command of the Rovio API to execute</param>
        /// <returns></returns>
        internal string DownloadString(string relativeUri, int timeout = 5000)
        {
            string responseString = null;
            WebDownload wc = null;
            try
            {
                wc = new WebDownload(timeout);
                wc.Credentials = this.rovioCredentials;
                wc.BaseAddress = this.rovioAddress;
                wc.Encoding = this.encoding;
                Uri targetUri = new Uri(new Uri(wc.BaseAddress), relativeUri);

                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();
                responseString = wc.DownloadString(targetUri);
                stopwatch.Stop();
                if (stopwatch.ElapsedMilliseconds > 5000)
                {
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            finally
            {
                if (wc != null)
                {
                    wc.Dispose();
                    wc = null;
                }
            }

            return responseString;
        }
       
        internal RovioResponse Request(string url, int timeout = 5000, params RequestItem[] parameters)
        {
            StringBuilder sb = new StringBuilder(url);
            bool firstItem = true;
            foreach (RequestItem item in parameters)
            {
                if (firstItem)
                {//第一个参数
                    sb.Append("?");
                    firstItem = false;
                }
                else
                {
                    sb.Append("&");
                }

                sb.Append(item.Key);
                sb.Append("=");
                sb.Append(RovioWebClient.UrlEncode(item.Value, this.encoding));
            }

            //发起请求
            string responseString = DownloadString(sb.ToString(), timeout);
            return new RovioResponse(responseString);
        }

        /// <summary>
        /// Web request for byte data to the Rovio API 
        /// </summary>
        /// <param name="cmd">Command of the Rovio API to execute</param>
        /// <returns></returns>
        public byte[] DownloadData(string cmd)
        {
            WebClient wc = new WebClient();
            wc.Credentials = this.rovioCredentials;
            wc.BaseAddress = this.rovioAddress;
            Uri targetUri = new Uri(new Uri(wc.BaseAddress), cmd);
            try
            {
                return wc.DownloadData(targetUri);
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Post数据到Rovio上。
        /// Rovio 为HTTP 1.0版本，不支持分块上传。
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="audio"></param>
        public void UploadData(string cmd, byte[] audio, int offset, int length, int timeout = 30000, BackgroundWorker bw = null)
        {
            Uri targetUri = new Uri(new Uri(this.rovioAddress), cmd);

            HttpWebRequest request = null;
            try
            {
                request = WebRequest.Create(targetUri) as HttpWebRequest;
                request.Credentials = this.rovioCredentials;
                request.AllowWriteStreamBuffering = true;
                request.Timeout = timeout;
                request.Method = WebRequestMethods.Http.Post;
                request.ContentType = "application/x-www-form-urlencoded";
                //务必设置false，否则连接会一直占用
                request.KeepAlive = false;
                request.ContentLength = length;

                //Stopwatch stopwatch = new Stopwatch();
                //stopwatch.Start();
                Stream requestStream = request.GetRequestStream();
                //Debug.WriteLine("请求耗时：" + stopwatch.ElapsedMilliseconds);
                //启动异步写入
                UploadDataState state = new UploadDataState(requestStream);
                IAsyncResult result = requestStream.BeginWrite(audio, offset, length, UploadDataCallback, state);
                while (!state.Finished)
                {
                    if (bw != null && bw.CancellationPending)
                    {
                        try
                        {
                            requestStream.EndWrite(result);
                            requestStream.Close();
                        }
                        catch (Exception ex)
                        {
                            //
                        }
                        break;
                    }
                }
                //Debug.WriteLine("写入耗时：" + stopwatch.ElapsedMilliseconds);

                // 获取响应，但Rovio必然会断开连接
                HttpWebResponse response = null;
                try
                {
                    response = request.GetResponse() as HttpWebResponse;
                }
                catch (WebException ex)
                {
                    switch (ex.Status)
                    {
                        case WebExceptionStatus.KeepAliveFailure:
                        case WebExceptionStatus.ConnectionClosed:
                            //Rovio对该请求无响应，忽略此错误
                            break;
                        default:
                            throw ex;
                    }
                }
                finally
                {
                    if (response != null)
                        response.Close();
                }
                //Debug.WriteLine("关闭耗时：" + stopwatch.ElapsedMilliseconds);
            }
            catch (WebException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if(request != null)
                    request.Abort();
            }
        }
        class UploadDataState
        {
            public Stream RequestStream { get; private set; }
            public volatile bool Finished = false;
            public UploadDataState(Stream requestStream)
            {
                this.RequestStream = requestStream;
                this.Finished = false;
            }
        }
        static void UploadDataCallback(IAsyncResult result)
        {
            //结束异步写入
            UploadDataState state = result.AsyncState as UploadDataState;
            Stream stream = state.RequestStream;
            try
            {
                stream.EndWrite(result);
                stream.Close();
            }
            catch (Exception ex)
            {
                //
            }

            state.Finished = true;
        }

        /// <summary>
        ///  效果等同于 System.Web.HttpUtility.UrlEncode(string, Encoding)。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str, Encoding encoding)
        {
            StringBuilder sb = new StringBuilder();
            byte[] data = encoding.GetBytes(str);
            for (int i = 0; i < data.Length; i++)
                sb.Append(@"%" + Convert.ToString(data[i], 16));

            return sb.ToString();
        }
    }
}
