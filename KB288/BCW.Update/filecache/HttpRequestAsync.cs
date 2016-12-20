using System;
using System.Net;

namespace BCW.Update
{
    /// <summary>
    /// HttpWebRquest 异步方法
    /// </summary>
    public class HttpRequestAsync
    {
        #region 私有变量

        private string _PostData;
        private string _PostCharset;
        private byte[] _ByteArray;

        private string _RevCharset;

        private HttpWebRequest _HttpRequest;
        private HttpWebResponse _HttpResponse;

        private WebHeaderCollection _HeaderCollection;
        private string _Value;
        private string _TimeoutMsg = "连接超时, 请重试";
        private bool _Timeout = false;

        #endregion 私有变量

        #region 属性

        /// <summary>
        /// HttpWebRequest
        /// </summary>
        public HttpWebRequest HttpRequest
        {
            set { _HttpRequest = value; }
            get { return _HttpRequest; }
        }

        /// <summary>
        /// 提交数据串
        /// </summary>
        public string PostData
        {
            set { _PostData = value; }
        }

        /// <summary>
        /// 提交数据串编码方式
        /// </summary>
        public string PostCharset
        {
            set { _PostCharset = value; }
        }

        /// <summary>
        /// 网页返回的字符编码
        /// </summary>
        public string RevCharset
        {
            set { _RevCharset = value; }
        }

        /// <summary>
        /// 返回网页头信息
        /// </summary>
        public WebHeaderCollection HeaderCollection
        {
            get { return _HeaderCollection; }
        }

        /// <summary>
        /// 返回网页源文件
        /// </summary>
        public string Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// 是否已超时
        /// </summary>
        public bool Timeout
        {
            get { return _Timeout; }
        }

        /// <summary>
        /// 超时信息
        /// </summary>
        public string TimeoutMsg
        {
            set { _TimeoutMsg = value; }
        }

        #endregion 属性

        #region 公开方法

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="p_url">目标网站地址</param>
        public HttpRequestAsync(string p_url)
        {
            _HttpRequest = WebRequest.Create(p_url) as HttpWebRequest;
            _HttpRequest.Method = "GET";
            _HttpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            _HttpRequest.Accept = "*/*";
            _HttpRequest.AutomaticDecompression = DecompressionMethods.GZip;
        }

        #endregion 公开方法

        #region 异步处理

        /// <summary>
        /// 开始对用来写入数据的 Stream 对象的异步请求
        /// </summary>
        public IAsyncResult BeginAsyncStream(object sender, EventArgs e, AsyncCallback p_cb, object p_state)
        {
            _HttpRequest.Method = "POST";
            if(string.IsNullOrEmpty(_HttpRequest.ContentType))
                _HttpRequest.ContentType = "application/x-www-form-urlencoded";

            this._ByteArray = System.Text.Encoding.GetEncoding(this._PostCharset).GetBytes(this._PostData);
            _HttpRequest.ContentLength = this._ByteArray.Length;

            return _HttpRequest.BeginGetRequestStream(p_cb, p_state);
        }

        /// <summary>
        /// 结束对用来写入数据的 Stream 对象的异步请求
        /// </summary>
        public void EndAsyncStream(IAsyncResult p_ar)
        {
            try
            {
                using (System.IO.Stream postStream = _HttpRequest.EndGetRequestStream(p_ar))
                {
                    postStream.Write(this._ByteArray, 0, this._ByteArray.Length);
                }
            }
            catch
            {
                this._Value = "EndGetRequestStream Error";
                this._Timeout = true;
            }
        }

        /// <summary>
        /// 开始对 Internet 资源的异步请求
        /// </summary>
        public IAsyncResult BeginAsyncResponse(object sender, EventArgs e, AsyncCallback p_cb, object p_state)
        {
            return _HttpRequest.BeginGetResponse(p_cb, p_state);
        }

        /// <summary>
        /// 结束对 Internet 资源的异步请求
        /// </summary>
        public void EndAsyncResponse(IAsyncResult p_ar)
        {
            try
            {
                using (_HttpResponse = _HttpRequest.EndGetResponse(p_ar) as HttpWebResponse)
                {
                    this._HeaderCollection = _HttpResponse.Headers;
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(_HttpResponse.GetResponseStream(), System.Text.Encoding.GetEncoding(this._RevCharset)))
                    {
                        this._Value = reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                // this._Value = ((HttpWebResponse)ex.Response).StatusDescription;
                this._Value = "EndGetResponse Error";
                this._Timeout = true;
            }
        }

        /// <summary>
        /// 异步超时
        /// </summary>
        public void TimeoutAsyncOperation(IAsyncResult p_ar)
        {
            // this._Value = "Async Timeout";
            this._Value = this._TimeoutMsg;
            this._Timeout = true;
        }

        #endregion 异步处理
    }
}
