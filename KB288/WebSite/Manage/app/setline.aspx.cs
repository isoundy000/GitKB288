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

public partial class Manage_app_setline : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");

        if (act == "ok")
        {
            string IDs = Utils.GetRequest("IDs", "post", 1, "", "");
            int OnTime = int.Parse(Utils.GetRequest("OnTime", "post", 1, @"^[0-9]\d*$", "0"));
            if (IDs != "")
            {
                string[] temp = IDs.Split("#".ToCharArray());
                for (int i = 0; i < temp.Length; i++)
                {
                    try
                    {
                        new BCW.BLL.User().UpdateTime(Convert.ToInt32(temp[i]), OnTime);
                    }
                    catch { }
                }
            }
            Utils.Success("更新会员在线", "更新会员在线成功，正在返回..", Utils.getUrl("setline.aspx"), "1");
        }
        else if (act == "iploginok")
        {
            ub xml = new ub();
            string xmlPath = "/Controls/bbs.xml";
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            string IPsID = Utils.GetRequest("IPsID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string MoneyID = Utils.GetRequest("MoneyID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string GetiCentID = Utils.GetRequest("GetiCentID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");
            string BankID = Utils.GetRequest("BankID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个ID请用#分隔，可以留空");

            xml.dss["BbsIPsID"] = IPsID;
            xml.dss["BbsMoneyID"] = MoneyID;
            xml.dss["BbsGetiCentID"] = GetiCentID;
            xml.dss["BbsBankID"] = BankID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("登录异常设置", "登录异常设置成功，正在返回..", Utils.getUrl("setline.aspx?act=iplogin"), "1");
        }
        else if (act == "iplogin")
        {
            Master.Title = "IP异常与查币设置";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("IP异常与查币");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "以下ID关闭登录异常(用#分开):/,以下ID有权查看隐藏币(用#分开):/,论坛基金取款ID(用#分开):/,以下ID有权查看银行资料(用#分开):/,";
            string strName = "IPsID,MoneyID,GetiCentID,BankID,act";
            string strType = "big,big,big,big,hidden";
            string strValu = "" + ub.GetSub("BbsIPsID", "/Controls/bbs.xml") + "'" + ub.GetSub("BbsMoneyID", "/Controls/bbs.xml") + "'" + ub.GetSub("BbsGetiCentID", "/Controls/bbs.xml") + "'" + ub.GetSub("BbsBankID", "/Controls/bbs.xml") + "'iploginok";
            string strEmpt = "true,true,true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,setline.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx") + "\">会员管理</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            Master.Title = "更新会员在线";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("更新会员在线");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入会员ID(用#分开):/,加在线时间,";
            string strName = "IDs,OnTime,act";
            string strType = "big,snum,hidden";
            string strValu = "'60'ok";
            string strEmpt = "true,false,false";
            string strIdea = "'分'|/";
            string strOthe = "确定执行|reset,setline.aspx,post,1,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
