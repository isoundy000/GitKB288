using System;
using System.Web;
using System.Web.UI;

namespace BCW.Update
{
    public class HttpRequestCache
    {
        private HttpRequestAsync _WebAsync;
        /// <summary>
        /// HttpRequestAsync �첽������
        /// </summary>
        public HttpRequestAsync WebAsync
        {
            get { return _WebAsync; }
            set { _WebAsync = value; }
        }

        private FileCache _Fc;
        /// <summary>
        /// �ļ��ͻ�����
        /// </summary>
        public FileCache Fc
        {
            get { return _Fc; }
            set { _Fc = value; }
        }

        private TimeSpan _Ts;
        /// <summary>
        /// �첽��ʱʱ��
        /// </summary>
        public TimeSpan Ts
        {
            get { return _Ts; }
            set { _Ts = value; }
        }

        private bool _CacheRead = false;
        /// <summary>
        /// �Ƿ�ӻ����ȡ
        /// </summary>
        public bool CacheRead
        {
            get { return _CacheRead; }
        }

        /// <summary>
        /// ���췽��
        /// </summary>
        /// <param name="p_url">Ŀ����վ��ַ</param>
        public HttpRequestCache(string p_url)
        {
            _WebAsync = new HttpRequestAsync(p_url);
            _Fc = new FileCache();
        }

        /// <summary>
        /// Method GET
        /// </summary>
        public bool MethodGetUrl()
        {
            string outVal = string.Empty;
            return MethodGetUrl(out outVal);
        }

        /// <summary>
        /// Method GET
        /// </summary>
        /// <param name="responseText">���ص���ҳ����</param>
        public bool MethodGetUrl(out string responseText)
        {
            bool IsContinue = true;

            if (_Fc.ReadFileCache())
            {
                responseText = _Fc.FileText;
                _CacheRead = true;
            }
            else
            {
                PageAsyncTask task = new PageAsyncTask(
                    new BeginEventHandler(_WebAsync.BeginAsyncResponse),
                    new EndEventHandler(_WebAsync.EndAsyncResponse),
                    new EndEventHandler(_WebAsync.TimeoutAsyncOperation),
                    null
                );

                Page p = new Page();
                if (_Ts.TotalSeconds > 0)
                    p.AsyncTimeout = _Ts;
                p.RegisterAsyncTask(task);
                p.ExecuteRegisteredAsyncTasks();

                responseText = _WebAsync.Value;

                if (_WebAsync.Timeout)
                    IsContinue = false;

                if (_Fc.CacheUsed && IsContinue)
                    _Fc.WriteFileCache(responseText);
            }

            return IsContinue;
        }

        /// <summary>
        /// Method POST
        /// </summary>
        public bool MethodPostUrl()
        {
            string outVal = string.Empty;
            return MethodPostUrl(out outVal);
        }

        /// <summary>
        /// Method POST
        /// </summary>
        /// <param name="responseText">���ص���ҳ����</param>
        public bool MethodPostUrl(out string responseText)
        {
            bool IsContinue = true;

            if (_Fc.ReadFileCache())
            {
                responseText = _Fc.FileText;
                _CacheRead = true;
            }
            else
            {
                PageAsyncTask taskStream = new PageAsyncTask(
                    new BeginEventHandler(_WebAsync.BeginAsyncStream),
                    new EndEventHandler(_WebAsync.EndAsyncStream),
                    new EndEventHandler(_WebAsync.TimeoutAsyncOperation),
                    null
                );

                PageAsyncTask taskOperation = new PageAsyncTask(
                    new BeginEventHandler(_WebAsync.BeginAsyncResponse),
                    new EndEventHandler(_WebAsync.EndAsyncResponse),
                    new EndEventHandler(_WebAsync.TimeoutAsyncOperation),
                    null
                );

                Page p = new Page();
                if (_Ts.TotalSeconds > 0)
                    p.AsyncTimeout = _Ts;
                p.RegisterAsyncTask(taskStream);
                p.RegisterAsyncTask(taskOperation);
                p.ExecuteRegisteredAsyncTasks();

                responseText = _WebAsync.Value;

                if (_WebAsync.Timeout)
                    IsContinue = false;

                if (_Fc.CacheUsed && IsContinue)
                    _Fc.WriteFileCache(responseText);
            }

            return IsContinue;
        }
    }
}
