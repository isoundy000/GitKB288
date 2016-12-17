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
public partial class addfile : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/upfile.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "post", 1, "", "");

        switch (act)
        {
            case "upload":
                UploadPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void UploadPage()
    { 
    
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string TypeName = "文件";
        int ptype = 13;
        int nid = int.Parse(Utils.GetRequest("nid", "all", 2, @"^[1-9]\d*$", "ID无效"));
        if (!new BCW.BLL.Topics().ExistsIdTypes(nid, ptype))
        {
            Utils.Error("不存在的上传栏目", "");
        }
        if (!Utils.Isie())
        {
            Master.Title = "温馨提示";
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<a href=\"addfile.aspx?nid=" + nid + "&amp;backurl=" + Utils.getPage(0) + "&amp;" + VE + "=2a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
        }
        else
        {
            Master.Title = "上传文件";
            builder.Append(Out.Tab("<div class=\"title\">WAP2.0上传文件</div>", ""));

            //上传个数
            int max = Convert.ToInt32(ub.GetSub("UpbMaxFileNum", xmlPath));
            int nn = int.Parse(Utils.GetRequest("nn", "get", 1, @"^[0-9]\d*$", "1"));
            if (nn > max)
                nn = max;
            int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[0-9]\d*$", "1"));
            if (num > max)
                num = max;

            builder.Append(Out.Tab("<div>", "<br />"));

            builder.Append("上传:");
            for (int i = 1; i <= max; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("addfile.aspx?num=" + i + "&amp;nn=" + nn + "") + "\"><b>" + i + "</b></a> ");
            }

            builder.Append(Out.Tab("</div>", ""));

            //编辑状态时的显示
            string sUpType = string.Empty;
            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;

            sUpType = "上传";
            sText = "" + TypeName + "标题:/," + TypeName + "描述:/,适用机型(多个逗号分开，可空):/,需要签证:/,";
            sName = "Title,Content,Model,IsVisa,";
            sType = "text,big,textarea,select,";
            sValu = "'''0'";
            sEmpt = "false,false,true,1|未知|2|需要|3|不需要,";


            strText += ",是否" + sUpType + "截图:/";
            strName += ",blpic";
            strType += ",select";
            strValu += "'False";
            strEmpt += ",True|上传|False|不上传";
            nn = 0; //截图标识
            for (int i = 0; i < num; i++)
            {
                string y = ",";
                if (num == 1)
                {
                    strText = strText + y + "选择" + sUpType + "附件:/," + sUpType + "附件描述:/";
                }
                else
                {
                    strText = strText + y + "" + sUpType + "第" + (i + 1) + "个附件:/," + sUpType + "附件描述" + (i + 1) + ":/";
                }
                strName = strName + y + "file" + (i + 1) + y + "stext" + (i + 1);
                if (!Utils.Isie())
                {
                    strType = strType + y + "text" + y + "text";
                }
                else
                {
                    strType = strType + y + "file" + y + "text";
                }
                strValu = strValu + "''";
                strEmpt = strEmpt + y + y;
            }

            strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,,";
            strName = sName + Utils.Mid(strName, 1, strName.Length) + ",ptype,nid,act,num,nn";
            strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden,hidden";

            strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + ptype + "'" + nid + "'soft'" + num + "'" + nn + "";
            strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,,";
            strIdea = "/";
            strOthe = "上传" + TypeName + "|reset,addfileok.aspx,post,2,red|blue";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("=上传帮助说明=<br />1.上传成功后将由管理员审核方可显示.<br />2.只有部分wap2.0手机支持上传,详情请咨询手机供应商.<br />3.文件上传需要时间较长,请耐心等待.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("default.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}