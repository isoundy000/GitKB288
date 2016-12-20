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

public partial class Manage_advert : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "广告管理";

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            case "gold":
                GoldPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("广告管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("全部广告|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?ptype=1") + "\">全部</a>|");

        if (ptype == 2)
            builder.Append("正在投放|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?ptype=2") + "\">正在</a>|");

        if (ptype == 3)
            builder.Append("即将投放|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?ptype=3") + "\">即将</a>|");

        if (ptype == 4)
            builder.Append("过期广告|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?ptype=4") + "\">过期</a>|");

        if (ptype == 5)
            builder.Append("已暂停");
        else
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?ptype=5") + "\">暂停</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 2)
            strWhere = "StartTime<='" + DateTime.Now + "' and OverTime>='" + DateTime.Now + "'";
        else if (ptype == 3)
            strWhere = "StartTime>'" + DateTime.Now + "'";
        else if (ptype == 4)
            strWhere = "OverTime<'" + DateTime.Now + "'";
        else if (ptype == 5)
            strWhere = "Status=1";

        // 开始读取列表
        IList<BCW.Model.Advert> listAdvert = new BCW.BLL.Advert().GetAdverts(pageIndex, pageSize, strWhere, out recordCount);
        if (listAdvert.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Advert n in listAdvert)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("advert.aspx?act=edit&amp;id={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>点击{3}", (pageIndex - 1) * pageSize + k, n.ID, n.Title, n.Click);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?act=add") + "\">添加广告</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?act=gold") + "\">点广告送币</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加广告");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "广告标题:/,广告内容:连接或UBB或WML:/,开始投放时间:/,过期时间:/,会员点击送币:/,送币性质:/,状态:/,,";
        string strName = "Title,AdUrl,StartTime,OverTime,iGold,adType,Status,act,backurl";
        string strType = "text,textarea,date,date,num,select,select,hidden,hidden";
        string strValu = "''" + DateTime.Now.ToString() + "'" + DateTime.Now.AddDays(30) + "'0'0'0'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,0|按天送|1|按周送|2|按次(不推荐),0|正常|1|暂停,false,false";
        string strIdea = "/";
        string strOthe = "添加广告|reset,advert.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示:<br />广告内容为连接时,统计点击/点广告送币才生效<br />");
        builder.Append("<a href=\"" + Utils.getUrl("advert.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{2,20}$", "请输入20字内的广告标题");
        string AdUrl = Utils.GetRequest("AdUrl", "post", 2, @"^[\s\S]{1,500}$", "广告限500字内，不能留空");
        DateTime StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "post", 2, DT.RegexTime, "投放时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        DateTime OverTime = Utils.ParseTime(Utils.GetRequest("OverTime", "post", 2, DT.RegexTime, "过期时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        int iGold = int.Parse(Utils.GetRequest("iGold", "post", 2, @"^[0-9]\d*$", "会员点击送币填写错误，不送币请填0"));
        int adType = int.Parse(Utils.GetRequest("adType", "post", 2, @"^[0-2]$", "送币性质选择错误"));
        int Status = int.Parse(Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "状态选择错误"));
        //广告类型
        int UrlType = 0;
        if (AdUrl.Contains("<") || AdUrl.Contains("&gt;"))
        {
            UrlType = 2;
        }
        else if (AdUrl.Contains("[") || AdUrl.Contains("("))
        {
            UrlType = 1;
        }
        else
        {
            UrlType = 0;
        }

        BCW.Model.Advert model = new BCW.Model.Advert();
        model.Title = Title;
        model.AdUrl = AdUrl;
        model.StartTime = StartTime;
        model.OverTime = OverTime;
        model.iGold = iGold;
        model.adType = adType;
        model.Status = Status;
        model.Click = 0;
        model.UrlType = UrlType;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Advert().Add(model);
        Utils.Success("添加广告", "添加广告成功..", Utils.getPage("advert.aspx"), "1");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Advert().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Advert model = new BCW.BLL.Advert().GetAdvert(id);

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("修改广告");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "广告标题:/,广告内容:连接或UBB或WML:/,开始投放时间:/,过期时间:/,点击数:/,会员点击送币:/,送币性质:/,状态:/,,,";
        string strName = "Title,AdUrl,StartTime,OverTime,Click,iGold,adType,Status,id,act,backurl";
        string strType = "text,textarea,date,date,num,num,select,select,hidden,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.AdUrl + "'" + DT.FormatDate(model.StartTime, 0) + "'" + DT.FormatDate(model.OverTime, 0) + "'" + model.Click + "'" + model.iGold + "'" + model.adType + "'" + model.Status + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,false,false,false,false,false,0|按天送|1|按周送|2|按次(不推荐),0|正常|1|暂停,false,false,false";
        string strIdea = "/";
        string strOthe = "修改广告|reset,advert.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("温馨提示:<br />广告内容为连接时,统计点击/点广告送币才生效<br />");
        builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?act=del&amp;id=" + id + "") + "\">删除广告</a><br />");
        builder.Append("<a href=\"" + Utils.getPage("advert.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[0-9]\d*$", "ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{2,20}$", "请输入20字内的广告标题");
        string AdUrl = Utils.GetRequest("AdUrl", "post", 2, @"^[\s\S]{1,500}$", "广告限500字内，不能留空");
        DateTime StartTime = Utils.ParseTime(Utils.GetRequest("StartTime", "post", 2, DT.RegexTime, "投放时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        DateTime OverTime = Utils.ParseTime(Utils.GetRequest("OverTime", "post", 2, DT.RegexTime, "过期时间格式填写出错,正确格式如" + DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") + ""));
        int Click = int.Parse(Utils.GetRequest("Click", "post", 2, @"^[0-9]\d*$", "点击数填写错误"));
        int iGold = int.Parse(Utils.GetRequest("iGold", "post", 2, @"^[0-9]\d*$", "会员点击送币填写错误，不送币请填0"));
        int adType = int.Parse(Utils.GetRequest("adType", "post", 2, @"^[0-2]$", "送币性质选择错误"));
        int Status = int.Parse(Utils.GetRequest("Status", "post", 2, @"^[0-9]\d*$", "状态选择错误"));
        if (!new BCW.BLL.Advert().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        //过期时间应大于投放时间
        if (StartTime > OverTime)
        {
            Utils.Error("过期时间应大于投放时间", "");
        }
        //广告类型
        int UrlType = 0;
        if (AdUrl.Contains("<") || AdUrl.Contains("&gt;"))
        {
            UrlType = 2;
        }
        else if (AdUrl.Contains("[") || AdUrl.Contains("("))
        {
            UrlType = 1;
        }
        else
        {
            UrlType = 0;
        }
        BCW.Model.Advert model = new BCW.Model.Advert();
        model.ID = id;
        model.Title = Title;
        model.AdUrl = AdUrl;
        model.StartTime = StartTime;
        model.OverTime = OverTime;
        model.iGold = iGold;
        model.adType = adType;
        model.Status = Status;
        model.Click = Click;
        model.UrlType = UrlType;
        model.AddTime = DateTime.Now;
        new BCW.BLL.Advert().Update(model);
        Utils.Success("修改广告", "修改广告成功..", Utils.getPage("advert.aspx"), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除广告";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此广告记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?info=ok&amp;act=del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx?act=edit&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Advert().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Advert().Delete(id);
            Utils.Success("删除广告", "删除广告成功..", Utils.getUrl("advert.aspx"), "1");
        }
    }

    private void GoldPage()
    {
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "确定设置")
        {
            string IsAdMsg = Utils.GetRequest("IsAdMsg", "post", 2, @"^[0-1]$", "送币内线通知选择错误");

            xml.dss["BbsIsAdMsg"] = IsAdMsg;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("点广告送币设置", "设置成功，正在返回..", Utils.getUrl("advert.aspx?act=gold"), "1");

        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("点广告送币设置");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("点广告送币也称打工送币<br />链接[URL=/adview.aspx]打工送币[/URL]");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "送币内线通知:/,,";
            string strName = "IsAdMsg,act,backurl";
            string strType = "select,hidden,hidden";
            string strValu = "" + xml.dss["BbsIsAdMsg"] + "'gold'" + Utils.getPage(0) + "";
            string strEmpt = "0|关闭|1|开启,false,false";
            string strIdea = "/";
            string strOthe = "确定设置|reset,advert.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("advert.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
