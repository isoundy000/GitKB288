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

public partial class Manage_xml_bbsset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "社区系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/bbs.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ThreadMin = Utils.GetRequest("ThreadMin", "post", 2, @"^[1-9]\d*$", "帖子标题最小字数填写错误");
            string ThreadMax = Utils.GetRequest("ThreadMax", "post", 2, @"^[1-9]\d*$", "帖子标题最大字数填写错误");
            string ContentMin = Utils.GetRequest("ContentMin", "post", 2, @"^[1-9]\d*$", "帖子内容最小字数填写错误");
            string ContentMax = Utils.GetRequest("ContentMax", "post", 2, @"^[1-9]\d*$", "帖子内容最大字数填写错误");
            string ReplyMax = Utils.GetRequest("ReplyMax", "post", 2, @"^[1-9]\d*$", "回帖内容最大字数填写错误");
            string ThreadNum = Utils.GetRequest("ThreadNum", "post", 2, @"^[0-9]\d*$", "每天每ID限发帖填写错误");
            string ReplyNum = Utils.GetRequest("ReplyNum", "post", 2, @"^[0-9]\d*$", "每天每ID限回帖填写错误");
            string ThreadExpir = Utils.GetRequest("ThreadExpir", "post", 2, @"^[0-9]\d*$", "发帖防刷秒数填写错误");
            string ReplyExpir = Utils.GetRequest("ReplyExpir", "post", 2, @"^[0-9]\d*$", "回帖防刷秒数填写错误");
            string MebookExpir = Utils.GetRequest("MebookExpir", "post", 2, @"^[0-9]\d*$", "空间留言防刷秒数填写错误");
            string IsSpeak = Utils.GetRequest("IsSpeak", "post", 2, @"^[0-1]\d*$", "游戏闲聊开关选择错误");
            string SpeakNum = Utils.GetRequest("SpeakNum", "post", 2, @"^[1-9]\d*$", "游戏闲聊显示条数填写错误");
            string SpeakExpir = Utils.GetRequest("SpeakExpir", "post", 2, @"^[0-9]\d*$", "游戏闲聊防刷秒数填写错误");
            string PsCent = Utils.GetRequest("PsCent", "post", 3, @"^[^\|]{1,50}(?:\|[^\|]{1,50}){1,500}$", "游戏快捷键填写错误，可以留空");
            string ThreadDel = Utils.GetRequest("ThreadDel", "post", 2, @"^[0-1]$", "帖子删除性质选择错误");
            string ReplyDel = Utils.GetRequest("ReplyDel", "post", 2, @"^[0-1]$", "回帖删除性质选择错误");
            string ReplyEdit = Utils.GetRequest("ReplyEdit", "post", 2, @"^[0-1]$", "回帖编辑性质选择错误");
            string ReplyTips = Utils.GetRequest("ReplyTips", "post", 2, @"^[0-9]\d*$", "回帖提醒收费填写错误");
            string CentThreadTar = Utils.GetRequest("CentThreadTar", "post", 2, @"^[0-9]\d*$", "派币帖手续费填写错误");

            string Greet = Utils.GetRequest("Greet", "post", 2, @"^[\s\S]{1,200}$", "刷屏问候语限1-200字");
            string Click = Utils.GetRequest("Click", "post", 2, @"^[1-9]\d*$", "阅读一次帖子增加多少次阅读数填写错误");
            string Mood = Utils.GetRequest("Mood", "post", 2, @"^[1-9]\d*$", "心情评价多少次显示图标");
            string ReplyMsg = Utils.GetRequest("ReplyMsg", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "多个快捷回复短语请用#分隔，可以留空");
            string ReplyGo = Utils.GetRequest("ReplyGo", "post", 2, @"^[0-2]$", "回帖默认返回选择错误");
            string IsOnline = Utils.GetRequest("IsOnline", "post", 2, @"^[0-2]$", "论坛显示在线选择错误");
            if (Convert.ToInt32(ThreadMin) > 50 || Convert.ToInt32(ThreadMax) > 50)
            {
                Utils.Error("帖子标题最大50字", "");
            }
            //----------计算快捷键合法性开始
            if (PsCent != "")
            {
                int GetNum = Utils.GetStringNum(PsCent, "|");
                if (GetNum % 2 == 0)
                {
                    Utils.Error("游戏快捷键填写错误，可以留空", "");
                }
            }
            xml.dss["BbsThreadMin"] = ThreadMin;
            xml.dss["BbsThreadMax"] = ThreadMax;
            xml.dss["BbsContentMin"] = ContentMin;
            xml.dss["BbsContentMax"] = ContentMax;
            xml.dss["BbsReplyMax"] = ReplyMax;
            xml.dss["BbsThreadNum"] = ThreadNum;
            xml.dss["BbsReplyNum"] = ReplyNum;
            xml.dss["BbsThreadExpir"] = ThreadExpir;
            xml.dss["BbsReplyExpir"] = ReplyExpir;
            xml.dss["BbsMebookExpir"] = MebookExpir;
            xml.dss["BbsIsSpeak"] = IsSpeak;
            xml.dss["BbsSpeakNum"] = SpeakNum;
            xml.dss["BbsSpeakExpir"] = SpeakExpir;
            xml.dss["BbsPsCent"] = PsCent;
            xml.dss["BbsThreadDel"] = ThreadDel;
            xml.dss["BbsReplyDel"] = ReplyDel;
            xml.dss["BbsReplyEdit"] = ReplyEdit;
            xml.dss["BbsReplyTips"] = ReplyTips;
            xml.dss["BbsCentThreadTar"] = CentThreadTar;

            xml.dss["BbsGreet"] = Greet;
            xml.dss["BbsClick"] = Click;
            xml.dss["BbsMood"] = Mood;
            xml.dss["BbsReplyMsg"] = ReplyMsg;
            xml.dss["BbsReplyGo"] = ReplyGo;
            xml.dss["BbsIsOnline"] = IsOnline;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("社区系统设置", "设置成功，正在返回..", Utils.getUrl("bbsset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "社区系统设置"));

            string strText = "帖子标题最小字数:/,帖子标题最大字数(50字内):/,帖子内容最小字数:/,帖子内容最大字数:/,回帖最大字数(500字内):/,每天每ID限发帖数(填0不限制):/,每天每ID限回帖数(填0不限制):/,发帖防刷秒数:/,回帖防刷秒数:/,空间留言防刷秒数:/,游戏闲聊:/,闲聊显示条数:/,游戏闲聊防刷秒数:/,游戏押注快捷键:/,帖子删除:,回帖编辑:,回帖删除,回帖提醒收费(按每人):/,派币帖手续费(%):/,刷屏问候语:/,阅读一次帖子增加多少次阅读数:/,心情评价多少次显示图标:/,帖子快捷回复短语(用#分开):/,回帖后默认返回:,论坛显示在线:";
            string strName = "ThreadMin,ThreadMax,ContentMin,ContentMax,ReplyMax,ThreadNum,ReplyNum,ThreadExpir,ReplyExpir,MebookExpir,IsSpeak,SpeakNum,SpeakExpir,PsCent,ThreadDel,ReplyEdit,ReplyDel,ReplyTips,CentThreadTar,Greet,Click,Mood,ReplyMsg,ReplyGo,IsOnline";
            string strType = "num,num,num,num,num,num,num,num,num,num,select,num,num,textarea,select,select,select,num,num,text,num,num,textarea,select,select";
            string strValu = "" + xml.dss["BbsThreadMin"] + "'" + xml.dss["BbsThreadMax"] + "'" + xml.dss["BbsContentMin"] + "'" + xml.dss["BbsContentMax"] + "'" + xml.dss["BbsReplyMax"] + "'" + xml.dss["BbsThreadNum"] + "'" + xml.dss["BbsReplyNum"] + "'" + xml.dss["BbsThreadExpir"] + "'" + xml.dss["BbsReplyExpir"] + "'" + xml.dss["BbsMebookExpir"] + "'" + xml.dss["BbsIsSpeak"] + "'" + xml.dss["BbsSpeakNum"] + "'" + xml.dss["BbsSpeakExpir"] + "'" + xml.dss["BbsPsCent"] + "'" + xml.dss["BbsThreadDel"] + "'" + xml.dss["BbsReplyEdit"] + "'" + xml.dss["BbsReplyDel"] + "'" + xml.dss["BbsReplyTips"] + "'" + xml.dss["BbsCentThreadTar"] + "'" + xml.dss["BbsGreet"] + "'" + xml.dss["BbsClick"] + "'" + xml.dss["BbsMood"] + "'" + xml.dss["BbsReplyMsg"] + "'" + xml.dss["BbsReplyGo"] + "'" + xml.dss["BbsIsOnline"] + "";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,0|开启|1|关闭,false,false,true,0|允许|1|禁止,0|允许|1|禁止,0|允许|1|禁止,false,false,false,false,false,true,0|回帖列表|1|帖子页面|2|论坛页面,0|显示在线|1|隐藏在线";
            string strIdea = "/";
            string strOthe = "确定修改|reset,bbsset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />押注快捷键填写格式:10000|一万币|5000|5千币|1000|1千币|500|500币<br />可以填写多个，系统自动排序，其中10000是值|一万币是显示名称");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}