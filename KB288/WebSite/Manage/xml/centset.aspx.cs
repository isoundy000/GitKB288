using System;
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

public partial class Manage_xml_centset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "积分系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/cent.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        Master.Title = "积分配置中心";

        string[] sNames = BCW.User.Cent.sNames;


        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "-1"));
        if (ptype != -1 && ptype <= sNames.Length)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                int Gold = int.Parse(Utils.GetRequest("Gold" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "" + ub.Get("SiteBz") + "填写出错"));
                int Money = int.Parse(Utils.GetRequest("Money" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "" + ub.Get("SiteBz2") + "填写出错"));
                int Score = int.Parse(Utils.GetRequest("Score" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "积分填写出错"));
                int Tl = int.Parse(Utils.GetRequest("Tl" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "体力填写出错"));
                int Ml = int.Parse(Utils.GetRequest("Ml" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "魅力填写出错"));
                int Zh = int.Parse(Utils.GetRequest("Zh" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "智慧填写出错"));
                int Ww = int.Parse(Utils.GetRequest("Ww" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "威望填写出错"));
                int Xe = int.Parse(Utils.GetRequest("Xe" + ptype + "", "post", 2, @"^-?[0-9]\d*$", "邪恶填写出错"));
                if (sNames[ptype].ToString().Contains("+"))
                {
                    int Num = int.Parse(Utils.GetRequest("Num" + ptype + "", "post", 2, @"^[0-9]\d*$", "每天上限次数填写出错"));
                    xml.dss["CentNum" + ptype + ""] = Num;
                }
                xml.dss["CentGold" + ptype + ""] = Gold;
                xml.dss["CentMoney" + ptype + ""] = Money;
                xml.dss["CentScore" + ptype + ""] = Score;
                xml.dss["CentTl" + ptype + ""] = Tl;
                xml.dss["CentMl" + ptype + ""] = Ml;
                xml.dss["CentZh" + ptype + ""] = Zh;
                xml.dss["CentWw" + ptype + ""] = Ww;
                xml.dss["CentXe" + ptype + ""] = Xe;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("积分配置", "配置成功，正在返回..", Utils.getUrl("centset.aspx?ptype=" + ptype + ""), "1");
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("" + sNames[ptype] + "配置");
                builder.Append(Out.Tab("</div>", ""));

                string sText = string.Empty;
                string sName = string.Empty;
                string sType = string.Empty;
                string sValu = string.Empty;
                string sEmpt = string.Empty;
                string mText = "罚";
                if (sNames[ptype].ToString().Contains("+"))
                {
                    sText = "每天上限(次):,";
                    sName = "Num" + ptype + ",";
                    sType = "snum,";
                    sValu = "" + xml.dss["CentNum" + ptype + ""] + "'";
                    sEmpt = "false,";
                    mText = "奖";
                }
                if (sNames[ptype].ToString().Contains("#"))
                {
                    mText = "奖";
                }
                string strText = "" + mText + "" + ub.Get("SiteBz") + ":," + mText + "" + ub.Get("SiteBz2") + ":," + mText + "积分:," + mText + "体力:," + mText + "魅力:," + mText + "智慧:," + mText + "威望:," + mText + "邪恶:," + sText + "";
                string strName = "Gold" + ptype + ",Money" + ptype + ",Score" + ptype + ",Tl" + ptype + ",Ml" + ptype + ",Zh" + ptype + ",Ww" + ptype + ",Xe" + ptype + "," + sName + "ptype";
                string strType = "stext,stext,stext,stext,stext,stext,stext,stext," + sType + "hidden";
                string strValu = "" + xml.dss["CentGold" + ptype + ""] + "'" + xml.dss["CentMoney" + ptype + ""] + "'" + xml.dss["CentScore" + ptype + ""] + "'" + xml.dss["CentTl" + ptype + ""] + "'" + xml.dss["CentMl" + ptype + ""] + "'" + xml.dss["CentZh" + ptype + ""] + "'" + xml.dss["CentWw" + ptype + ""] + "'" + xml.dss["CentXe" + ptype + ""] + "'" + sValu + "" + ptype + "";
                string strEmpt = "false,false,false,false,false,false,false," + sEmpt + "false";
                string strIdea = "/";
                string strOthe = "确定修改,centset.aspx,post,1,red";

                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示：<br />1.支持负数,负值将扣取相应的属性值.<br />2,注意邪恶值为贬义,奖为负数,罚为正数.<br />3.不奖罚积分请填写0.");
                if(mText=="奖")
                    builder.Append("<br />4.每天上限次数填写0则为每天不限次数奖励.");

                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("积分配置中心");
            builder.Append(Out.Tab("</div>", ""));
            int pageIndex;
            int recordCount; 
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string[] pageValUrl = { };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //总记录数
            recordCount = sNames.Length;

            int stratIndex = (pageIndex - 1) * pageSize;
            int endIndex = pageIndex * pageSize;
            int k = 0;
            for (int i = 0; i < sNames.Length; i++)
            {
                if (k >= stratIndex && k < endIndex)
                {
                    if ((k + 1) % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));

                    builder.Append("" + (i + 1) + ".<a href=\"" + Utils.getUrl("centset.aspx?ptype=" + i + "") + "\">" + sNames[i].ToString() + "</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                if (k == endIndex)
                    break;
                k++;
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype != -1)
            builder.Append("<a href=\"" + Utils.getUrl("centset.aspx") + "\">积分配置</a><br />");

        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
}