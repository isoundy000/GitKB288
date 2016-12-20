using System;
using System.Net;

namespace BCW.Update
{
    /// <summary>
    /// HttpWebRquest �첽����
    /// </summary>
    public class HttpRequestAsync
    {
        #region ˽�б���

        private string _PostData;
        private string _PostCharset;
        private byte[] _ByteArray;

        private string _RevCharset;

        private HttpWebRequest _HttpRequest;
        private HttpWebResponse _HttpResponse;

        private WebHeaderCollection _HeaderCollection;
        private string _Value;
        private string _TimeoutMsg = "���ӳ�ʱ, ������";
        private bool _Timeout = false;

        #endregion ˽�б���

        #region ����

        /// <summary>
        /// HttpWebRequest
        /// </summary>
        public HttpWebRequest HttpRequest
        {
            set { _HttpRequest = value; }
            get { return _HttpRequest; }
        }

        /// <summary>
        /// �ύ���ݴ�
        /// </summary>
        public string PostData
        {
            set { _PostData = value; }
        }

        /// <summary>
        /// �ύ���ݴ����뷽ʽ
        /// </summary>
        public string PostCharset
        {
            set { _PostCharset = value; }
        }

        /// <summary>
        /// ��ҳ���ص��ַ�����
        /// </summary>
        public string RevCharset
        {
            set { _RevCharset = value; }
        }

        /// <summary>
        /// ������ҳͷ��Ϣ
        /// </summary>
        public WebHeaderCollection HeaderCollection
        {
            get { return _HeaderCollection; }
        }

        /// <summary>
        /// ������ҳԴ�ļ�
        /// </summary>
        public string Value
        {
            get { return _Value; }
        }

        /// <summary>
        /// �Ƿ��ѳ�ʱ
        /// </summary>
        public bool Timeout
        {
            get { return _Timeout; }
        }

        /// <summary>
        /// ��ʱ��Ϣ
        /// </summary>
        public string TimeoutMsg
        {
            set { _TimeoutMsg = value; }
        }

        #endregion ����

        #region ��������

        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="p_url">Ŀ����վ��ַ</param>
        public HttpRequestAsync(string p_url)
        {
            _HttpRequest = WebRequest.Create(p_url) as HttpWebRequest;
            _HttpRequest.Method = "GET";
            _HttpRequest.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727)";
            _HttpRequest.Accept = "*/*";
            _HttpRequest.AutomaticDecompression = DecompressionMethods.GZip;
        }

        #endregion ��������

        #region �첽����

        /// <summary>
        /// ��ʼ������д�����ݵ� Stream ������첽����
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
        /// ����������д�����ݵ� Stream ������첽����
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
        /// ��ʼ�� Internet ��Դ���첽����
        /// </summary>
        public IAsyncResult BeginAsyncResponse(object sender, EventArgs e, AsyncCallback p_cb, object p_state)
        {
            return _HttpRequest.BeginGetResponse(p_cb, p_state);
        }

        /// <summary>
        /// ������ Internet ��Դ���첽����
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
        /// �첽��ʱ
        /// </summary>
        public void TimeoutAsyncOperation(IAsyncResult p_ar)
        {
            // this._Value = "Async Timeout";
            this._Value = this._TimeoutMsg;
            this._Timeout = true;
        }

        #endregion �첽����
    }
}
