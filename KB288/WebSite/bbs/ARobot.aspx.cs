using System;
using System.Collections.Generic;
using System.Data;
using BCW.Common;
using System.Text;
using System.Net;
using System.IO;
using BCW.JS;
using Newtonsoft.Json;

public partial class bbs_ARobot : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected string xmlPath = "/Controls/chat.xml";
    private string APIKEY = string.Empty;
    private string INFO = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes("你好"));
    private string getURl = string.Empty;
    private string code = "";
    private string text = "";
    private string url = "";
    private string list = "";

    public string article = "";
    public string source = "";
    public string detailurl = "";
    public string trainnum = "";
    public string start = "";
    public string terminal = "";
    public string flight = "";
    public string route = "";
    public string starttime = "";
    public string endtime = "";
    public string icon = "";
    public string name = "";
    public string info = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        APIKEY = ub.GetSub("APIKEY", xmlPath).Trim();
        getURl = "http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + INFO;
        Master.Title = "酷友小宝宝";
        builder.Append(Out.Tab("<div class=\"title\">酷友小宝宝</div>", ""));
        string Msg = Utils.GetRequest("Content", "post", 1, "", "");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "清屏")
        {
            Session["lastsee"] = "";
            Session["Msg"] = "";
        }
        try
        {
            if (Session["Msg"].ToString() == Msg&& Session["Msg"].ToString() != "")
            {
                Utils.Error("请不要老是问酷宝相同的问题！", "");
            }
        }
        catch
        {

        }
        if (Msg != "")
        {
            builder.Append(Out.Tab("<div class=\"class\">", ""));
            builder.Append(Session["lastsee"]);
            builder.Append(Recevie(Msg));
            builder.Append(Out.Tab("</div>", "<br/>"));
            Session["lastsee"] = Session["lastsee"]+Recevie(Msg);
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"class\">", ""));
            Session["lastsee"] = "<b style=\"color:red\">酷宝：</b>您好，我是酷宝，有什么可以帮您的吗？[" + DT.FormatDate(DateTime.Now, 13) + "]<br/>";
            if (ac == "清屏")
            {
                Session["lastsee"] = "";
            }
            builder.Append(Session["lastsee"]);
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        Session["Msg"] = Msg;
        strText = ",";
        strName = "Content";
        strType = "text";
        strValu = "";
        strEmpt = "true";
        strIdea = "/";
        strOthe = "发送|清屏,ARobot.aspx,post,3,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("chat.aspx?act=help") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    public ToJoson DoAPI(string Msg)
    {
        INFO = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(Msg));
        getURl = "http://www.tuling123.com/openapi/api?key=" + APIKEY + "&info=" + INFO;
        Uri uri = new Uri(getURl);
        HttpWebRequest getUrl = WebRequest.Create(uri) as HttpWebRequest;
        getUrl.Method = "GET";
        HttpWebResponse response = getUrl.GetResponse() as HttpWebResponse;
        Stream respStream = response.GetResponseStream();
        StreamReader stream = new StreamReader(respStream, Encoding.UTF8);
        string respStr = stream.ReadToEnd();
        stream.Close();
        JsonSerializer serializer = new JsonSerializer();
        StringReader sr = new StringReader(respStr);
        ToJoson joson = serializer.Deserialize<ToJoson>(new JsonTextReader(sr));
        code = joson.code;
        text = joson.text;
        url = joson.url;
        List<list> list = joson.list;
        return joson;
    }
    public string Recevie(string msg)
    {
        int meid = new BCW.User.Users().GetUsId();
        string mename = "游客";
        if (meid != 0)
        {
            mename = new BCW.BLL.User().GetUsName(meid);
        }
        StringBuilder receiveMsg = new StringBuilder();
        receiveMsg.Append("<b style=\"color:blue\">" + mename+"：</b>");
        receiveMsg.Append(msg + "[" + DT.FormatDate(DateTime.Now, 13) + "]<br/>");
        receiveMsg.Append("<b style=\"color:red\">酷宝：</b>");
        ToJoson joson = DoAPI(msg);
        List<list> list = joson.list;
        switch (joson.code)
        {
            //文本类
            case "100000":
                receiveMsg.Append(Out.SysUBB(joson.text));
                break;
            //列车
            case "305000":
                receiveMsg.Append(joson.text+"<br/>")
                          .Append("起始站【" + list[0].start + "】，到达站【" + list[0].terminal + "】<br/>");
                receiveMsg.Append("车次\t\t\t\t\t开车时间\t\t到达时间<br/>");
                foreach (list listDetail in list)
                {
                    receiveMsg.Append(listDetail.trainnum.PadRight(30, ' ') + "\t" + listDetail.starttime + "\t\t\t" + listDetail.endtime+"<br/>" );
                }
                receiveMsg.Append("详情地址:" + Out.SysUBB("[url]"+list[0].detailurl+"[/url]"));
                break;
            //航班
            case "306000":
                receiveMsg.Append(joson.text+"<br/>");
                receiveMsg.Append("航班\t\t\t\t\t起飞时间\t到达时间<br/>" );
                foreach (list listDetail in list)
                {
                    receiveMsg.Append(listDetail.flight.PadRight(50, ' ') + listDetail.starttime + "\t\t" + listDetail.endtime+"<br/>" );
                }
                break;
            //网址类数据 
            case "200000":
                receiveMsg.Append(joson.text+"<br/>")
                           .Append(Out.SysUBB("[url]" + joson.url+ "[/url]"));
                break;
            //新闻 
            case "302000":
                receiveMsg.Append(joson.text+"<br/>")
                          .Append(Out.SysUBB("[url]" + joson.url + "[/url]"));
                foreach (list listDetail in list)
                {
                    receiveMsg.Append(listDetail.source + "   " + listDetail.article)
                        .Append(Out.SysUBB("[url]" + listDetail.detailurl + "[/url]"));
                }
                break;
            //菜谱、视频、小说 
            case "308000":
                receiveMsg.Append(joson.text)
                         .Append(Out.SysUBB("[url]" + joson.url + "[/url]"));
                foreach (list listDetail in list)
                {
                    receiveMsg.Append(listDetail.name + "   " + listDetail.info)
                        .Append(Out.SysUBB("[url]" + listDetail.detailurl + "[/url]"));
                }
                break;
            //key的长度错误（32位）
            case "40001":
                receiveMsg.Append("酷宝生病了，可以帮我告诉管理员吗？");
                break;
            //请求内容为空
            case "40002":
                receiveMsg.Append("喂喂，酷宝听不清你在说什么？");
                break;
            //Key错误或帐号未激活
            case "40003":
                receiveMsg.Append("酷宝感冒了，可以帮我告诉管理员吗？");
                break;
            //当天请求次数已用完
            case "40004":
                receiveMsg.Append("酷宝今天累了，明天再问吧！");
                break;
            //暂不支持该功能
            case "40005":
                receiveMsg.Append("酷宝还不懂这个！");
                break;
            //服务器升级中
            case "40006":
                receiveMsg.Append("酷宝在学习，请先不要打扰我！");
                break;
            //服务器数据格式异常
            case "40007":
                receiveMsg.Append("酷宝发烧了，可以帮我告诉管理员吗？");
                break;
            default:
                break;
        }
        receiveMsg.Append("[" + DT.FormatDate(DateTime.Now, 13) + "]<br/>");
        return receiveMsg.ToString();
    }
}


