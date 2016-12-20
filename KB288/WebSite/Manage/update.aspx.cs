using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;
using BCW.Update;
using BCW.Update.Model;

public partial class Manage_update : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string HttpHost = "http://bichengwei.nowtx.net/update/";//目标域名+目录
    protected string CacheVersion = "lightcmsVersion";
    protected string CacheSpDomain = "lightcmsSpDomain";
    protected string CacheFtpData = "lightcmsFtpData";
    protected string CacheFtpMore = "lightcmsFtpMore";
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "系统升级";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "init":
                InitializePage();
                break;
            case "check":
                CheckPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Application.Remove("xml_wap");//清缓存
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string vs = Utils.GetRequest("vs", "get", 1, "", "");
        //获取当前版本
        string OrdVersion = BCW.Common.ub.Get("SiteVersion");

        //获取新版本
        object NewVersion = DataCache.GetCache(CacheVersion);
        //获取特殊更新
        object SpDomain = DataCache.GetCache(CacheSpDomain);
        if (info != "ok" || NewVersion == null)
        {
            Utils.Success("正在检查更新", "正在检查更新,请稍后..", Utils.getUrl("update.aspx?act=check"), "1");
        }
        else
        {
            if (vs == "")
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("系统自助升级");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("当前版本:" + BCW.Common.ub.Get("SiteVersion") + "<br />");

                if (NewVersion.ToString() == OrdVersion.ToString())
                {
                    builder.Append("目前已是最新版本");
                    builder.Append("<a href=\"" + Utils.getUrl("update.aspx?act=init&amp;vs=" + NewVersion + "") + "\">重新升级</a>");
                }
                else
                {
                    builder.Append("最新版本:" + NewVersion + "<br />");
                    builder.Append("<b><a href=\"" + Utils.getUrl("update.aspx?act=init&amp;vs=" + NewVersion + "") + "\">立即升级</a></b>");
                }
                builder.Append(Out.Tab("</div>", ""));

                //最近升级列表
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                builder.Append("=最近升级版本=");
                builder.Append(Out.Tab("</div>", ""));
                string strName = SpDomain.ToString().Split("#".ToCharArray())[1];
                string[] sName = strName.Split("|".ToCharArray());
                int pageIndex;
                int recordCount;
                int pageSize = 10;
                string[] pageValUrl = { "info" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                //总记录数
                recordCount = sName.Length;

                int stratIndex = (pageIndex - 1) * pageSize;
                int endIndex = pageIndex * pageSize;
                int k = 0;
                for (int i = 0; i < sName.Length; i++)
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        if ((k + 1) % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));

                        builder.Append("[" + (i + 1) + "]<a href=\"" + Utils.getUrl("update.aspx?info=ok&amp;vs=" + sName[i] + "") + "\">版本" + sName[i].ToString() + "</a>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    if (k == endIndex)
                        break;
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("重要提示:请遵循从低版本到高版本顺序升级");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("您选择的版本:" + vs + "<br />");
                builder.Append("<b><a href=\"" + Utils.getUrl("update.aspx?act=init&amp;vs=" + vs + "") + "\">立即升级此版本</a></b>");
                builder.Append("<br /><a href=\"" + Utils.getUrl("update.aspx?info=ok") + "\">暂不升级</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("update.aspx?act=check") + "\">¤检查更新</a><br />");
            if (!string.IsNullOrEmpty(SpDomain.ToString()) && Utils.GetDomain().Contains(SpDomain.ToString()))
                builder.Append("<a href=\"" + Utils.getUrl("update.aspx?act=init&amp;vs=v9.9.9") + "\">*有您站特殊更新</a><br />");

            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitializePage()
    {
        string vs = Utils.GetRequest("vs", "get", 1, "", "");
        if (!WebUpdate.IsVersion(vs.Replace("v", "")))
        {
            Utils.Error("版本号错误", "");
        }

        //更新系统状态
        BCW.Common.ub xml = new BCW.Common.ub();
        xml.Reload();
        xml.ds["SiteStatus"] = 1; //维护升级状态
        System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
        //设置一个缓存，用作下一页比较
        int ModelCache = 60;
        DataCache.SetCache("LIGHT-CMSUPDATE", Utils.Mid(Utils.getstrU(), 0, Utils.getstrU().Length - 4), DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);

        //if (vs == "v1.3.52")
        //{
        //    SqlHelper.ExecuteSql("update tb_user set forumSet=replace(forumSet,'@','|')");
        //}
        Utils.Success("正在初始化", "系统正在进入维护状态,请稍后..", Utils.getUrl("updatest.aspx?vs=" + vs + ""), "2");
    }

    /// <summary>
    /// 检查版本号等
    /// </summary>
    private void CheckPage()
    {
        //获取更新并写入XML文件
        string GetUrl = "" + HttpHost + "lightBcwUpdate.xml";
        BCW.Update.Model.UpdateInfo model = new UpdateXML().GetVersionXML(GetUrl);
        if (model == null)
        {
            Utils.Error("网络超时", "");
        }
        if (model.Version != null)
        {
            int ModelCache = 120;
            DataCache.SetCache(CacheVersion, model.Version, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
            DataCache.SetCache(CacheSpDomain, model.SpDomain, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
            DataCache.SetCache(CacheFtpData, model.FtpData, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
        }
        DataCache.RemoveCache("LIGHTCMSUPDATE");//去掉升级提醒
        Utils.Success("检查更新", "检查更新完成,请稍后..", Utils.getUrl("update.aspx?info=ok"), "2");
    }

}
