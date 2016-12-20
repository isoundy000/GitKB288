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

public partial class bbs_Gsqiset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "设置期数";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string GsAdminID2 = ub.GetSub("BbsGsAdminID2", xmlPath);
            if (!("#" + GsAdminID2 + "#").Contains("#" + meid + "#"))
            {
                Utils.Error("你的权限不足", "");
            }
            string GsqiNum = Utils.GetRequest("GsqiNum", "post", 2, @"^[0-9]\d*$", "期数填写错误");
            string GsStopTime = Utils.GetRequest("GsStopTime", "post", 2, DT.RegexTime, "截止发布时间填写错误");

            //全部参赛论坛还有其中的论坛版主未开奖，管理员就不能开出下期------》》》但可以修改截止时间
            int qiNum = Convert.ToInt32(ub.GetSub("BbsGsqiNum", xmlPath));
            if (GsqiNum != qiNum.ToString())
            {
                if (new BCW.BLL.Forumvote().ExistsKz())
                {
                    Utils.Error("上期还没有开奖完成不能开通下期（改变期数），可修改截止时间", "");
                }
            }

            xml.dss["BbsGsqiNum"] = GsqiNum;
            xml.dss["BbsGsStopTime"] = GsStopTime;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("设置期数", "设置成功，正在返回..", Utils.getPage("forum.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("设置期数");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "本期期数:/,本期截止发布时间:/,,";
            string strName = "GsqiNum,GsStopTime,act,backurl";
            string strType = "num,date,hidden,hidden";
            string strValu = "" + xml.dss["BbsGsqiNum"] + "'" + xml.dss["BbsGsStopTime"] + "'config'" + Utils.getPage(0) + "";
            string strEmpt = "true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,Gsqiset.aspx,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
            builder.Append("<a href=\"" + Utils.getPage("forum.aspx") + "\">&lt;&lt;返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
