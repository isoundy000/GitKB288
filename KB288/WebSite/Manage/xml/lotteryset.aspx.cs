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

public partial class Manage_xml_lotteryset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "抽奖设置";
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/lottery.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "抽奖状态选择出错");
            string Benqi = Utils.GetRequest("Benqi", "post", 2, @"^[1-9]\d*$", "当前期数填写错误");
            string Title = Utils.GetRequest("Title", "post", 2, @"^[\s\S]{1,30}$", "抽奖标题限1-30字内");
            string Content = Utils.GetRequest("Content", "post", 3, @"^[\s\S]{1,2000}$", "祝福语限2000字内");
            string FreshSec = Utils.GetRequest("FreshSec", "post", 2, @"^[1-9]\d*$", "每局抽奖时间填写错误");
            string FreshMin = Utils.GetRequest("FreshMin", "post", 2, @"^[1-9]\d*$", "每局结束后N分钟后进行下一局填写错误");
            string WinTar = Utils.GetRequest("WinTar", "post", 2, @"^[1-9]\d*$", "中奖概率填写错误");
            if (int.Parse(WinTar) <= 0 || int.Parse(WinTar) > 10)
                Utils.Error("中奖概率填写错误", "");

            int Odds30 = int.Parse(Utils.GetRequest("Odds30", "post", 2, @"^[0-9]\d*$", "1币千分比填写错误"));
            int Odds50 = int.Parse(Utils.GetRequest("Odds50", "post", 2, @"^[0-9]\d*$", "2币千分比填写错误"));
            int Odds100 = int.Parse(Utils.GetRequest("Odds100", "post", 2, @"^[0-9]\d*$", "3币千分比填写错误"));
            int Odds200 = int.Parse(Utils.GetRequest("Odds200", "post", 2, @"^[0-9]\d*$", "4币千分比填写错误"));
            int Odds300 = int.Parse(Utils.GetRequest("Odds300", "post", 2, @"^[0-9]\d*$", "5币千分比填写错误"));
            int Odds500 = int.Parse(Utils.GetRequest("Odds500", "post", 2, @"^[0-9]\d*$", "10币千分比填写错误"));
            int Odds800 = int.Parse(Utils.GetRequest("Odds800", "post", 2, @"^[0-9]\d*$", "20币千分比填写错误"));
            int Odds999 = int.Parse(Utils.GetRequest("Odds999", "post", 2, @"^[0-9]\d*$", "50币千分比填写错误"));
            int Odds9999 = int.Parse(Utils.GetRequest("Odds9999", "post", 2, @"^[0-9]\d*$", "100币千分比填写错误"));
            int Odds99999 = int.Parse(Utils.GetRequest("Odds99999", "post", 2, @"^[0-9]\d*$", "200币千分比填写错误"));

            xml.dss["LotteryStatus"] = Status;
            xml.dss["LotteryBenqi"] = Benqi;
            xml.dss["LotteryTitle"] = Title;
            xml.dss["LotteryContent"] = Content;
            xml.dss["LotteryFreshSec"] = FreshSec;
            xml.dss["LotteryFreshMin"] = FreshMin;
            xml.dss["LotteryWinTar"] = WinTar;
            xml.dss["LotteryOdds30"] = Odds30;
            xml.dss["LotteryOdds50"] = Odds50;
            xml.dss["LotteryOdds100"] = Odds100;
            xml.dss["LotteryOdds200"] = Odds200;
            xml.dss["LotteryOdds300"] = Odds300;
            xml.dss["LotteryOdds500"] = Odds500;
            xml.dss["LotteryOdds800"] = Odds800;
            xml.dss["LotteryOdds999"] = Odds999;
            xml.dss["LotteryOdds9999"] = Odds9999;
            xml.dss["LotteryOdds99999"] = Odds99999;
            int iOdds = Odds30 + Odds50 + Odds100 + Odds200 + Odds300 + Odds500 + Odds800 + Odds999 + Odds9999 + Odds99999;
            if (iOdds != 1000)
                Utils.Error("币值概率相加必须等于1000", "");

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("抽奖设置", "设置成功，正在返回..", Utils.getUrl("lotteryset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "抽奖设置"));
            long PayCent = 0;
            long PayCent2 = 0;
            long PayCent3 = 0;
            object obj = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=999");
            if (obj != null)
            {
                PayCent = Convert.ToInt64(obj);
            }
            object obj2 = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=997");
            if (obj2 != null)
            {
                PayCent2 = Convert.ToInt64(obj2);
            }
            object obj3 = BCW.Data.SqlHelper.GetSingle("SELECT Sum(NodeId) from tb_Action where Types=998");
            if (obj3 != null)
            {
                PayCent3 = Convert.ToInt64(obj3);
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("当前已抽出" + PayCent + "" + ub.Get("SiteBz") + "/" + PayCent2 + "积分/" + PayCent3 + "" + ub.Get("SiteBz2") + "");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "抽奖状态:/,当前抽奖期数:/,抽奖标题:/,祝福语:/,每局抽奖时间(秒):/,每局结束后N分钟后进行下一局:/,中奖概率:,=以下为开出币值概率=/1币千分比:,2币千分比:,3币千分比:,4币千分比:,5币千分比:,10币千分比:,20币千分比:,50币千分比:,100币千分比:,200币千分比:,";
            string strName = "Status,Benqi,Title,Content,FreshSec,FreshMin,WinTar,Odds30,Odds50,Odds100,Odds200,Odds300,Odds500,Odds800,Odds999,Odds9999,Odds99999,backurl";
            string strType = "select,num,text,textarea,num,num,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,hidden";
            string strValu = "" + xml.dss["LotteryStatus"] + "'" + xml.dss["LotteryBenqi"] + "'" + xml.dss["LotteryTitle"] + "'" + xml.dss["LotteryContent"] + "'" + xml.dss["LotteryFreshSec"] + "'" + xml.dss["LotteryFreshMin"] + "'" + xml.dss["LotteryWinTar"] + "'" + xml.dss["LotteryOdds30"] + "'" + xml.dss["LotteryOdds50"] + "'" + xml.dss["LotteryOdds100"] + "'" + xml.dss["LotteryOdds200"] + "'" + xml.dss["LotteryOdds300"] + "'" + xml.dss["LotteryOdds500"] + "'" + xml.dss["LotteryOdds800"] + "'" + xml.dss["LotteryOdds999"] + "'" + xml.dss["LotteryOdds9999"] + "'" + xml.dss["LotteryOdds99999"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|正常|1|维护,false,false,true,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,lotteryset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />中奖概率使用10分比配置，如填写3则中奖率达十分之三.<br />币值概率使用千分比配置，可以控制币值开出的概率.即次数比例.");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
