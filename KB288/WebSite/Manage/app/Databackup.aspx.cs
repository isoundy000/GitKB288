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
using BCW.Data;
public partial class Manage_app_Databackup : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "ok":
                Backup();
                break;
            case "log":
                LOG();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        Master.Title = "数据库备份与恢复";
        builder.Append(Out.Tab("<div class=\"title\">数据库备份与恢复</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("请谨慎选择执行类型:");
        builder.Append(Out.Tab("</div>", ""));
        string DataBaseName = ConfigHelper.GetConfigString("DataBaseName");

        string strText = "数据库名称:/,备份或还原路径:/,操作类型,";
        string strName = "DataName,DataPath,iType,act";
        string strType = "text,text,select,hidden";
        string strValu = "" + DataBaseName + "'/App_Data/" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + new Rand().RandNume(4) + ".bak'1'ok";
        string strEmpt = "false,true,1|备份数据|2|还原数据|3|清空日志,false";
        string strIdea = "/";
        string strOthe = "确定执行|reset,databackup.aspx,post,1,red|blue";


        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />1.请慎用还原功能，选定清空日志时无需填写路径<br />2.备份数据时务必备份在App_Data文件夹，该系统文件夹为NET系统文件夹，可禁止下载<br />3.当网站运行一段时间后，数据日志文件必然慢慢增大，这时通过清日志加快网站运行速度");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("databackup.aspx?act=log") + "\">数据日志作业</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">系统服务中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void Backup()
    {
        int iType = int.Parse(Utils.GetRequest("iType", "post", 2, @"^[1-3]*$", "请选择操作类型"));
        string DataName = Utils.GetRequest("DataName", "post", 2, @"^[(A-Za-z0-9)|( !@#\^\*\(\)\-_=\+,\.<>\?/\:;"")]+$", "数据库名称应该是由字母、数字和下划线组成");

        string labletxt = string.Empty;
        string DataPath = string.Empty;

        if (iType != 3)
        {
            DataPath = Utils.GetRequest("DataPath", "post", 2, @"^[(A-Za-z0-9)|( !@#\^\*\(\)\-_=\+,\.<>\?/\:;"")]+$", "数据库路径不能为中文和其它特殊字符");
        }

        if (iType == 1)
        {
            new SqlUp().BackUp(DataName, DataPath);
            string BaseLength = BCW.Files.FileTool.GetFileContentLength(DataPath);
            labletxt = "数据库(" + BaseLength + ")备份成功,备份路径" + DataPath + "";
        }
        else if (iType == 2)
        {
            new SqlUp().UpBack(DataName, DataPath);
            labletxt = "数据库恢复成功";
        }
        else
        {
            new SqlUp().ClearDataLog(DataName);
            labletxt = "数据库日志清空成功";
        }
        Utils.Success("数据库管理", "" + labletxt + "，正在返回..", Utils.getUrl("databackup.aspx"), "1");    

    }

    private void LOG()
    {
        Master.Title = "数据日志作业";

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove("xml_wap");//清缓存
        xml.Reload(); //加载网站配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string iLogTime = Utils.GetRequest("iLogTime", "post", 2, @"^[0-9]\d*$", "填写小时错误");

            xml.ds["SiteiLogTime"] = iLogTime;
            xml.ds["SiteLogTime"] = DateTime.Now.ToString();
            System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
            Utils.Success("设置日志作业", "设置日志作业成功，正在返回..", Utils.getUrl("databackup.aspx?act=log"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "日志作业设置"));

            string strText = "截断日志间隔时间/,";
            string strName = "iLogTime,act";
            string strType = "snum,hidden";
            string strValu = "" + xml.ds["SiteiLogTime"] + "'log";
            string strEmpt = "true,false";
            string strIdea = "小时'|/";
            string strOthe = "确定修改|reset,databackup.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />设置间隔时间可以自动截断数据库日志<br />填写0则取消自动清日志作业");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("databackup.aspx") + "\">返回上一级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}