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

public partial class model : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "设置我的机型";
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "auto":
                AutoPage();
                break;
            case "ok":
                OkPage();
                break;
            case "more":
                MorePage();
                break;
            case "list":
                ListPage();
                break;
            case "all":
                AllPage();
                break;
            case "search":
                SearchPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=auto&amp;backurl=" + Utils.getPage(0) + "") + "\">自动适配我的机型</a>");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入机型:如N73/,,";
        string strName = "keyword,backurl,act";
        string strType = "text,hidden,hidden";
        string strValu = "'" + Utils.getPage(0) + "'search";
        string strEmpt = "false,false,false";
        string strIdea = "";
        string strOthe = "搜索,model.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">诺基亚</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">索爱</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">三星</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">摩托</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;backurl=" + Utils.getPage(0) + "") + "\">多普达</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=16&amp;backurl=" + Utils.getPage(0) + "") + "\">联想</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=8&amp;backurl=" + Utils.getPage(0) + "") + "\">魅族</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=6&amp;backurl=" + Utils.getPage(0) + "") + "\">苹果</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=9&amp;backurl=" + Utils.getPage(0) + "") + "\">HTC</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=7&amp;backurl=" + Utils.getPage(0) + "") + "\">黑莓</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=23&amp;backurl=" + Utils.getPage(0) + "") + "\">LG</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=19&amp;backurl=" + Utils.getPage(0) + "") + "\">OPPO</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=more&amp;backurl=" + Utils.getPage(0) + "") + "\">更多..</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = null;
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">诺基亚</a>〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        ds = new BCW.BLL.Modata().GetList("TOP 20 ID,PhoneModel", "Types=3 ORDER BY PhoneClick DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[i]["PhoneModel"] + "</a>");
                if (k % 4 == 0)
                {
                    builder.Append("<br />");
                }
                else
                {
                    builder.Append("|");
                }
                k++;
            }
        }
        builder.Append("开头:");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=X&amp;backurl=" + Utils.getPage(0) + "") + "\">X</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=N&amp;backurl=" + Utils.getPage(0) + "") + "\">N</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=E&amp;backurl=" + Utils.getPage(0) + "") + "\">E</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=C&amp;backurl=" + Utils.getPage(0) + "") + "\">C</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=9&amp;backurl=" + Utils.getPage(0) + "") + "\">9</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=8&amp;backurl=" + Utils.getPage(0) + "") + "\">8</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=7&amp;backurl=" + Utils.getPage(0) + "") + "\">7</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=6&amp;backurl=" + Utils.getPage(0) + "") + "\">6</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=5&amp;backurl=" + Utils.getPage(0) + "") + "\">5</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=3&amp;backurl=" + Utils.getPage(0) + "") + "\">3</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=2&amp;backurl=" + Utils.getPage(0) + "") + "\">2</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=3&amp;keyword=1&amp;backurl=" + Utils.getPage(0) + "") + "\">1</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">索尼爱立信</a>〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        ds = new BCW.BLL.Modata().GetList("TOP 20 ID,PhoneModel", "Types=4 ORDER BY PhoneClick DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[i]["PhoneModel"] + "</a>");
                if (k % 4 == 0)
                {
                    builder.Append("<br />");
                }
                else
                {
                    builder.Append("|");
                }
                k++;
            }
        }
        builder.Append("开头:");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=Z&amp;backurl=" + Utils.getPage(0) + "") + "\">Z</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=X&amp;backurl=" + Utils.getPage(0) + "") + "\">X</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=W&amp;backurl=" + Utils.getPage(0) + "") + "\">W</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=V&amp;backurl=" + Utils.getPage(0) + "") + "\">V</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=U&amp;backurl=" + Utils.getPage(0) + "") + "\">U</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=T&amp;backurl=" + Utils.getPage(0) + "") + "\">T</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=S&amp;backurl=" + Utils.getPage(0) + "") + "\">S</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=R&amp;backurl=" + Utils.getPage(0) + "") + "\">R</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=M&amp;backurl=" + Utils.getPage(0) + "") + "\">M</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=K&amp;backurl=" + Utils.getPage(0) + "") + "\">K</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=4&amp;keyword=J&amp;backurl=" + Utils.getPage(0) + "") + "\">J</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;backurl=" + Utils.getPage(0) + "") + "\">多普达</a>〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        ds = new BCW.BLL.Modata().GetList("TOP 20 ID,PhoneModel", "Types=18 ORDER BY PhoneClick DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[i]["PhoneModel"] + "</a>");
                if (k % 4 == 0)
                {
                    builder.Append("<br />");
                }
                else
                {
                    builder.Append("|");
                }
                k++;
            }
        }
        builder.Append("开头:");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=5&amp;backurl=" + Utils.getPage(0) + "") + "\">5</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=6&amp;backurl=" + Utils.getPage(0) + "") + "\">6</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=8&amp;backurl=" + Utils.getPage(0) + "") + "\">8</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=9&amp;backurl=" + Utils.getPage(0) + "") + "\">9</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=A&amp;backurl=" + Utils.getPage(0) + "") + "\">A</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=C&amp;backurl=" + Utils.getPage(0) + "") + "\">C</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=D&amp;backurl=" + Utils.getPage(0) + "") + "\">D</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=P&amp;backurl=" + Utils.getPage(0) + "") + "\">P</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=S&amp;backurl=" + Utils.getPage(0) + "") + "\">S</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=18&amp;keyword=T&amp;backurl=" + Utils.getPage(0) + "") + "\">T</a>.");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("〓<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;backurl=" + Utils.getPage(0) + "") + "\">摩托罗拉</a>〓");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        ds = new BCW.BLL.Modata().GetList("TOP 20 ID,PhoneModel", "Types=5 ORDER BY PhoneClick DESC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[i]["PhoneModel"] + "</a>");
                if (k % 4 == 0)
                {
                    builder.Append("<br />");
                }
                else
                {
                    builder.Append("|");
                }
                k++;
            }
        }
        builder.Append("开头:");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=A&amp;backurl=" + Utils.getPage(0) + "") + "\">A</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=C&amp;backurl=" + Utils.getPage(0) + "") + "\">C</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=E&amp;backurl=" + Utils.getPage(0) + "") + "\">E</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=L&amp;backurl=" + Utils.getPage(0) + "") + "\">L</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=M&amp;backurl=" + Utils.getPage(0) + "") + "\">M</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=Q&amp;backurl=" + Utils.getPage(0) + "") + "\">Q</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=T&amp;backurl=" + Utils.getPage(0) + "") + "\">T</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=V&amp;backurl=" + Utils.getPage(0) + "") + "\">V</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=W&amp;backurl=" + Utils.getPage(0) + "") + "\">W</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=X&amp;backurl=" + Utils.getPage(0) + "") + "\">X</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=5&amp;keyword=Z&amp;backurl=" + Utils.getPage(0) + "") + "\">Z</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("注：以上机型均热门机型，其他机型均可使用搜索功能找到.");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">自动适配</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MorePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("选择手机品牌");
        builder.Append(Out.Tab("</div>", "<br />"));
        DataSet ds = null;
        //列出手机品牌
        builder.Append(Out.Tab("<div>", ""));
        ds = new BCW.BLL.Modata().GetList("Types,PhoneBrand", "Types > 0 GROUP BY Types,PhoneBrand ORDER BY Types");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                int Types = int.Parse(ds.Tables[0].Rows[i]["Types"].ToString());
                string Brand = ds.Tables[0].Rows[i]["PhoneBrand"].ToString();
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype=" + Types + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + Brand + "</a>");
                if (k != ds.Tables[0].Rows.Count)
                    builder.Append("|");
                k++;
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "")+"\">设置机型</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AllPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        if (!new BCW.BLL.Modata().Exists2(ptype))
        {
            Utils.Error("不存在的品牌类型", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "") + "\">设置机型</a>&gt;" + new BCW.BLL.Modata().GetPhoneBrand(ptype) + "&gt;设置机型");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = null;
        //列出开头字符
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        ds = new BCW.BLL.Modata().GetList("DISTINCT left(PhoneModel,1) as PhoneModel", "Types=" + ptype + "");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string keyword = ds.Tables[0].Rows[i]["PhoneModel"].ToString();
                builder.Append("〓" +keyword + "开头〓<br />");
                //查找适配开头机型
                DataSet dss = new BCW.BLL.Modata().GetList("ID,PhoneModel", "Types=" + ptype + " and left(PhoneModel,1)='" + keyword + "'");
                if (dss != null && dss.Tables[0].Rows.Count > 0)
                {
                    int k = 1;
                    builder.Append(Out.Tab("<div>", ""));
                    for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + dss.Tables[0].Rows[j]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + dss.Tables[0].Rows[j]["PhoneModel"] + "</a>");
                        if (k != dss.Tables[0].Rows.Count)
                            builder.Append("|");
                        k++;
                    }
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "")+"\">设置机型</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ListPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "类型错误"));
        string keyword = Utils.GetRequest("keyword", "get", 1, @"^[A-Z0-9]$", "");
        if (!new BCW.BLL.Modata().Exists2(ptype))
        {
            Utils.Error("不存在的品牌类型", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=all&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + new BCW.BLL.Modata().GetPhoneBrand(ptype) + "</a>&gt;设置机型");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (keyword == "")
        {
            keyword = new BCW.BLL.Modata().GetPhoneModel(ptype);
            keyword = Utils.Left(keyword, 1);
        }
        DataSet ds = null;
        //列出开头字符
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("" + keyword + "开头 ");
        ds = new BCW.BLL.Modata().GetList("DISTINCT left(PhoneModel,1) as PhoneModel", "Types=" + ptype + " and left(PhoneModel,1)<>'" + keyword + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string KY = ds.Tables[0].Rows[i]["PhoneModel"].ToString();
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=list&amp;ptype="+ptype+"&amp;keyword=" + KY + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + KY + "</a>");
                builder.Append("|");
            }
        }
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=all&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">全部</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //查找适配开头机型
        ds = new BCW.BLL.Modata().GetList("ID,PhoneModel", "Types=" + ptype + " and left(PhoneModel,1)='" + keyword + "'");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int k = 1;
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id=" + ds.Tables[0].Rows[i]["ID"] + "&amp;backurl=" + Utils.getPage(0) + "") + "\">" + ds.Tables[0].Rows[i]["PhoneModel"] + "</a>");
                if (k != ds.Tables[0].Rows.Count)
                    builder.Append("|");
                k++;
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "")+"\">设置机型</a>");
        builder.Append(Out.Tab("</div>", ""));
    
    }

    private void SearchPage()
    {
        string keyword = Utils.GetRequest("keyword", "all", 3, @"^[\s\S]{1,15}$", "请正确输入手机型号");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("搜索“" + keyword + "”结果");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "keyword", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "PhoneModel like '" + keyword + "%'";

        // 开始读取列表
        IList<BCW.Model.Modata> listModata = new BCW.BLL.Modata().GetModatas(pageIndex, pageSize, strWhere, out recordCount);
        if (listModata.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Modata n in listModata)
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

                builder.AppendFormat("<a href=\"" + Utils.getUrl("model.aspx?act=ok&amp;id={0}&amp;backurl=" + Utils.PostPage(true) + "") + "\">{1}.{2}{3}[设置]</a>", n.ID, (pageIndex - 1) * pageSize + k, n.PhoneBrand, n.PhoneModel);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = ",,";
        string strName = "keyword,backurl,act";
        string strType = "text,hidden,hidden";
        string strValu = "'" + Utils.getPage(0) + "'search";
        string strEmpt = "false,false,false";
        string strIdea = "";
        string strOthe = "继续搜,model.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + "")+"\">设置机型</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void OkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (!new BCW.BLL.Modata().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        BCW.Model.Modata model = new BCW.BLL.Modata().GetModata(id);
        //写入Cookies
        HttpCookie cookie = new HttpCookie("BrandComment");
        cookie.Expires = DateTime.Now.AddDays(365);
        cookie.Values.Add("PhoneBrand", HttpUtility.UrlEncode(model.PhoneBrand));
        cookie.Values.Add("PhoneModel", HttpUtility.UrlEncode(model.PhoneModel));
        cookie.Values.Add("PhoneSystem", HttpUtility.UrlEncode(model.PhoneSystem));
        cookie.Values.Add("PhoneSize", HttpUtility.UrlEncode(model.PhoneSize));
        Response.Cookies.Add(cookie);
        new BCW.BLL.Modata().UpdatePhoneClick(id);

        Utils.Success("设置我的机型", "设置机型成功！<br />您的手机:" + model.PhoneBrand + "" + model.PhoneModel + "<br />操作系统:" + model.PhoneSystem + "<br />屏幕分辨率:" + model.PhoneSize + "", Utils.getPage("default.aspx"), "2");
    }

    private void AutoPage()
    {
        //获得UA
        string UA = Utils.GetBrowser();
        BCW.Model.Modata model = new BCW.BLL.Modata().GetModata2(UA);
        if (model != null)
        {
            //写入Cookies
            HttpCookie cookie = new HttpCookie("BrandComment");
            cookie.Expires = DateTime.Now.AddDays(365);
            cookie.Values.Add("PhoneBrand", HttpUtility.UrlEncode(model.PhoneBrand));
            cookie.Values.Add("PhoneModel", HttpUtility.UrlEncode(model.PhoneModel));
            cookie.Values.Add("PhoneSystem", HttpUtility.UrlEncode(model.PhoneSystem));
            cookie.Values.Add("PhoneSize", HttpUtility.UrlEncode(model.PhoneSize));
            Response.Cookies.Add(cookie);
            new BCW.BLL.Modata().UpdatePhoneClick(model.ID);

            Utils.Success("设置我的机型", "设置机型成功！<br />您的手机:" + model.PhoneBrand + "" + model.PhoneModel + "<br />操作系统:" + model.PhoneSystem + "<br />屏幕分辨率:" + model.PhoneSize + "", Utils.getPage("default.aspx"), "2");
        }
        else
        {
            Utils.Success("自动适配机型", "抱歉,无法自动识别到您的手机型号.请返回手动设置机型..", Utils.getUrl("model.aspx?backurl=" + Utils.getPage(0) + ""), "2");
        
        }
    }
}
