using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR3.Model.Http;
namespace TPR3.Http
{
    /// <summary>
    /// �ȷ�ֱ��������
    /// </summary>
    public class GetLive3
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/Live/jslq/";

        #region ����

        private bool _CacheUsed = true; //�Ƿ��¼����/��TXT
        private int _CacheTime = 5;//����ʱ��(����)

        /// <summary>
        /// �Ƿ�ʹ���ļ��ͻ���
        /// </summary>
        public bool CacheUsed
        {
            set { _CacheUsed = value; }
        }

        /// <summary>
        /// �ļ��ͻ������ʱ��
        /// </summary>
        public int CacheTime
        {
            set { _CacheTime = value; }
        }

        #endregion ����

        /// <summary>
        /// ���췽��
        /// </summary>
        public GetLive3()
        {
        }

        /// <summary>
        /// ȡ������ʱ�ȷ��б�
        /// </summary>
        public TPR3.Model.Http.Live3 Getllist(string p_title)
        {
            TPR3.Model.Http.Live3 objllist = null;

            string url = "http://wap.titan007.com/nbabflist.aspx?nametype=1";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objllist = Live3llistHtml(this._ResponseValue);

            return objllist;
        }

        /// <summary>
        /// ��������ʱ�ȷ��б�
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR3.Model.Http.Live3 Live3llistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objllist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "&gt;&gt;����ʱ�ȷ�<br/>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("/nbabfshow.aspx?", Utils.getUrl("live.aspx?act=11") + "&amp;").Replace("/nbaScheduleall.aspx?", Utils.getUrl("live.aspx?act=12") + "&amp;");
                    str = Regex.Replace(str, @"<a.href=.nbaScheduleall.aspx[\s\S]+?>����ȫ������</a><br/>", "");
                    objllist.txtLivellist = str;
                }
            }

            return objllist;
        }


        /// <summary>
        /// ȡ�ý�������ȫ������
        /// </summary>
        public TPR3.Model.Http.Live3 Gettlist(string p_title)
        {
            TPR3.Model.Http.Live3 objtlist = null;

            string url = "http://wap.titan007.com/nbaScheduleall.aspx?nametype=1";

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objtlist = Live3tlistHtml(this._ResponseValue);

            return objtlist;
        }

        /// <summary>
        /// �����������ȫ������
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR3.Model.Http.Live3 Live3tlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objtlist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/basketball.aspx[\s\S]+?>��</a>", "");
                    objtlist.txtLivetlist = str;
                }
            }

            return objtlist;
        }

        /// <summary>
        /// ȡ����ϸ��¼
        /// </summary>
        public TPR3.Model.Http.Live3 Getvlist(int SClassID, string SClass)
        {
            TPR3.Model.Http.Live3 objvlist = null;

            string url = string.Format("http://wap.titan007.com/nbabfshow.aspx?nametype=1&SClassID={0}&SClass={1}", SClassID, SClass);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = SClass + "" + SClassID;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objvlist = Live3vlistHtml(this._ResponseValue);

            return objvlist;
        }

        /// <summary>
        /// ������ϸ��¼
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR3.Model.Http.Live3 Live3vlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR3.Model.Http.Live3 objvlist = new TPR3.Model.Http.Live3();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/basketball.aspx[\s\S]+?>��</a>", "");
                    objvlist.txtLiveView = str;
                }
            }

            return objvlist;
        }
    }
}
