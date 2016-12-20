using System;
using System.Text.RegularExpressions;
using BCW.Common;
using TPR.Model.Http;
namespace TPR.Http
{
    /// <summary>
    /// �ȷ�ֱ��������
    /// </summary>
    public class GetLive
    {
        private string _ResponseValue = string.Empty;
        private string _CacheFolder = "~/Files/Cache/live/jszq/";

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
        public GetLive()
        {
        }

        /// <summary>
        /// ȡ������ʱ�ȷ��б�
        /// </summary>
        public TPR.Model.Http.Live Getzlist(string p_title, int kind)
        {
            TPR.Model.Http.Live objzlist = null;

            string url = string.Format("http://wap.titan007.com/cnlist.aspx?nametype=1&kind={0}", kind);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title + "" + kind;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            //httpRequest.WebAsync.PostData = "Input=" + p_mobileNumber;
            //httpRequest.WebAsync.PostCharset = "UTF-8";

            //if (httpRequest.MethodPostUrl(out this._ResponseValue))
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objzlist = LivezlistHtml(this._ResponseValue);

            return objzlist;
        }

        /// <summary>
        /// ��������ʱ�ȷ��б�
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR.Model.Http.Live LivezlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objzlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "title=\"δ����\">δ����</a><br/>([\\s\\S]+?)</small><br/>";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("/Schedule.aspx?", Utils.getUrl("live.aspx?act=5&amp;") + "&amp;");

                    objzlist.txtLivezlist = str;
                }
            }

            return objzlist;
        }


        /// <summary>
        /// ȡ��δ��/����/�곡�б�
        /// </summary>
        public TPR.Model.Http.Live Getmlist(string p_title, int State, int kind)
        {
            TPR.Model.Http.Live objmlist = null;

            string url = string.Format("http://wap.titan007.com/ScheduleAll.aspx?State={0}&kind={1}&nametype=1", State, kind);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = p_title + "" + State;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objmlist = LivemlistHtml(this._ResponseValue);

            return objmlist;
        }
        /// <summary>
        /// ����δ��/����/�곡�б�
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR.Model.Http.Live LivemlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objmlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "&gt;&gt;</b><br/>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/football.aspx[\s\S]+?>��</a>", "");
                    objmlist.txtLiveState = str;
                }
            }

            return objmlist;
        }


        /// <summary>
        /// ȡ����ϸ��¼
        /// </summary>
        public TPR.Model.Http.Live Getvlist(int kind, int SClassID, string SClass)
        {
            TPR.Model.Http.Live objvlist = null;

            string url = string.Format("http://wap.titan007.com/Schedule.aspx?nametype=1&kind={0}&SClassID={1}&SClass={2}", kind, SClassID, SClass);

            HttpRequestCache httpRequest = new HttpRequestCache(url);
            httpRequest.Fc.CacheUsed = this._CacheUsed;
            httpRequest.Fc.CacheTime = this._CacheTime;
            httpRequest.Fc.CacheFolder = this._CacheFolder;
            httpRequest.Fc.CacheFile = SClass + "" + SClassID + "" + kind;

            httpRequest.WebAsync.RevCharset = "UTF-8";
            if (httpRequest.MethodGetUrl(out this._ResponseValue))
                objvlist = LivevlistHtml(this._ResponseValue);

            return objvlist;
        }

        /// <summary>
        /// ������ϸ��¼
        /// </summary>
        /// <param name="p_html">HTML�ĵ�</param>
        private TPR.Model.Http.Live LivevlistHtml(string p_html)
        {
            string strVal = p_html;
            TPR.Model.Http.Live objvlist = new TPR.Model.Http.Live();

            if (!string.IsNullOrEmpty(strVal))
            {
                string pattern = "</p><p>([\\s\\S]+?)</p><p align=\"center\">";
                Match m1 = Regex.Match(strVal, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                if (m1.Success)
                {
                    string str = "";
                    str = m1.Groups[1].Value.Replace("<small>", "").Replace("</small>", "").Replace("</big>", "").Replace("<big>", "");
                    str = Regex.Replace(str, @"<a.href=./analysis/football.aspx[\s\S]+?>��</a>", "");
                    str = Regex.Replace(str, @"���[\s\S]+?[\d]{11}<br/>", "");
                    objvlist.txtLiveView = str;
                }
            }

            return objvlist;
        }
    }
}
