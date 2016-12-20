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

public partial class Manage_xml_horseset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "跑马游戏设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/horse.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Horse口号限50字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-2]$", "开奖类型选择出错");
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
                string DtPay = Utils.GetRequest("DtPay", "post", 2, @"^[0-9]\d*$", "默认押注大小填写错误");
                string CycleMin = Utils.GetRequest("CycleMin", "post", 2, @"^[0-9]\d*$", "游戏周期填写错误");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                //string Odds = Utils.GetRequest("Odds", "post", 2, @"^[^\#]{1,10}(?:\#[^\#]{1,10}){0,14}$", "赔率请用#分开，并且赔率必须是15个选项");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");
                //string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                //string BigPay2 = Utils.GetRequest("BigPay2", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                //string IDMaxPay = Utils.GetRequest("IDMaxPay", "post", 2, @"^[0-9]\d*$", "每ID每局最大下注填写错误");
                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }

                xml.dss["HorseName"] = Name;
                xml.dss["HorseNotes"] = Notes;
                xml.dss["HorseLogo"] = Logo;
                xml.dss["HorseStatus"] = Status;
                xml.dss["HorseOpenType"] = OpenType;
                xml.dss["HorseTax"] = Tax;
                xml.dss["HorseSmallPay"] = SmallPay;
                xml.dss["HorseBigPay"] = BigPay;
                xml.dss["HorseExpir"] = Expir;
                xml.dss["HorseDtPay"] = DtPay;
                xml.dss["HorseCycleMin"] = CycleMin;
                xml.dss["HorseOnTime"] = OnTime;
                xml.dss["HorseOdds"] = "3#4#5#8#10#20#30#50#75#100#125#175#200#250#500";
                xml.dss["HorseFoot"] = Foot;
                xml.dss["HorseGuestSet"] = GuestSet;
                xml.dss["HorseIsBot"] = IsBot;
                //xml.dss["HorseSmallPay2"] = SmallPay2;
                //xml.dss["HorseBigPay2"] = BigPay2;
                //xml.dss["HorseIDMaxPay"] = IDMaxPay;
            }
            else
            { 

                int Odds3 = int.Parse(Utils.GetRequest("Odds3", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds4 = int.Parse(Utils.GetRequest("Odds4", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds5 = int.Parse(Utils.GetRequest("Odds5", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds8 = int.Parse(Utils.GetRequest("Odds8", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds10 = int.Parse(Utils.GetRequest("Odds10", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds20 = int.Parse(Utils.GetRequest("Odds20", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds30 = int.Parse(Utils.GetRequest("Odds30", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds50 = int.Parse(Utils.GetRequest("Odds50", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds75 = int.Parse(Utils.GetRequest("Odds75", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds100 = int.Parse(Utils.GetRequest("Odds100", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds125 = int.Parse(Utils.GetRequest("Odds125", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds175 = int.Parse(Utils.GetRequest("Odds175", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds200 = int.Parse(Utils.GetRequest("Odds200", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds250 = int.Parse(Utils.GetRequest("Odds250", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                int Odds500 = int.Parse(Utils.GetRequest("Odds500", "post", 2, @"^[0-9]\d*$", "赔率填写错误"));
                long MaxOutCent = int.Parse(Utils.GetRequest("MaxOutCent", "post", 4, @"^[0-9]\d*$", "赔付上限填写错误"));
                xml.dss["HorseOdds3"] = Odds3;
                xml.dss["HorseOdds4"] = Odds4;
                xml.dss["HorseOdds5"] = Odds5;
                xml.dss["HorseOdds8"] = Odds8;
                xml.dss["HorseOdds10"] = Odds10;
                xml.dss["HorseOdds20"] = Odds20;
                xml.dss["HorseOdds30"] = Odds30;
                xml.dss["HorseOdds50"] = Odds50;
                xml.dss["HorseOdds75"] = Odds75;
                xml.dss["HorseOdds100"] = Odds100;
                xml.dss["HorseOdds125"] = Odds125;
                xml.dss["HorseOdds175"] = Odds175;
                xml.dss["HorseOdds200"] = Odds200;
                xml.dss["HorseOdds250"] = Odds250;
                xml.dss["HorseOdds500"] = Odds500;
                xml.dss["HorseMaxOutCent"] = MaxOutCent;
                long Odds = Odds3 + Odds4 + Odds5 + Odds8 + Odds10 + Odds20 + Odds30 + Odds50 + Odds75 + Odds100 + Odds125 + Odds175 + Odds200 + Odds250 + Odds500;
                if (Odds != 1000)
                {
                    Utils.Error("赔率百分比总和必须是1000", "");
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("跑马游戏设置", "设置成功，正在返回..", Utils.getUrl("horseset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "跑马系统设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("跑马设置|");
                builder.Append("<a href=\"" + Utils.getUrl("horseset.aspx?ptype=1") + "\">概率</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("horseset.aspx?ptype=0") + "\">跑马设置</a>");
                builder.Append("|概率");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=10&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,开奖类型:/,系统收税(%):/,最小下注:/,最大下注:/,下注防刷(秒):/,押注默认大小:/,游戏周期(分钟):/,游戏开放时间(可留空):/,底部Ubb:/,兑奖内线:/,机器人:/,";
                string strName = "Name,Notes,Logo,Status,OpenType,Tax,SmallPay,BigPay,Expir,DtPay,CycleMin,OnTime,Foot,GuestSet,IsBot,backurl";
                string strType = "text,text,text,select,select,num,num,num,num,num,num,text,textarea,select,select,hidden";
                string strValu = "" + xml.dss["HorseName"] + "'" + xml.dss["HorseNotes"] + "'" + xml.dss["HorseLogo"] + "'" + xml.dss["HorseStatus"] + "'" + xml.dss["HorseOpenType"] + "'" + xml.dss["HorseTax"] + "'" + xml.dss["HorseSmallPay"] + "'" + xml.dss["HorseBigPay"] + "'" + xml.dss["HorseExpir"] + "'" + xml.dss["HorseDtPay"] + "'" + xml.dss["HorseCycleMin"] + "'" + xml.dss["HorseOnTime"] + "'" + xml.dss["HorseFoot"] + "'" + xml.dss["HorseGuestSet"] + "'" + xml.dss["HorseIsBot"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,0|概率随机|1|随机开奖|2|智能分析,false,false,false,false,false,false,true,true,0|开启|1|关闭,0|关闭|1|开启,false";

                //strText += ",最小下注(" + ub.Get("SiteBz2") + "):/,最大下注(" + ub.Get("SiteBz2") + "):/,每ID每局最大下注(" + ub.Get("SiteBz2") + "):/";
                //strName += ",SmallPay2,BigPay2,IDMaxPay";
                //strType += ",num,num,num";
                //strValu += "'" + xml.dss["HorseSmallPay2"] + "'" + xml.dss["HorseBigPay2"] + "'" + xml.dss["HorseIDMaxPay"] + "";
                //strEmpt += ",false,false,false";
                
                string strIdea = "/";
                string strOthe = "确定修改|reset,horseset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />概率随机指按概率配置来随机出开任意赔率的两匹马，需配置概率<br />随机开奖指正常随机开出两匹马，有可能经常开出大赔率的两匹马<br />智能分析指自动开出最少赔付或者没有购买记录的两匹马,选择此项系统必赢.<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                //string[] sTemp = ("3#4#5#8#10#20#30#50#75#100#125#175#200#250#500").Split("#".ToCharArray());

                string strText = "赔率3千分比:,赔率4千分比:,赔率5千分比:,赔率8千分比:,赔率10千分比:,赔率20千分比:,赔率30千分比:,赔率50千分比:,赔率75千分比:,赔率100千分比:,赔率125千分比:,赔率175千分比:,赔率200千分比:,赔率250千分比:,赔率500千分比:,总赔付上限(" + ub.Get("SiteBz") + "):,,";
                string strName = "Odds3,Odds4,Odds5,Odds8,Odds10,Odds20,Odds30,Odds50,Odds75,Odds100,Odds125,Odds175,Odds200,Odds250,Odds500,MaxOutCent,ptype,backurl";
                string strType = "snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,snum,hidden,hidden";
                string strValu = "" + xml.dss["HorseOdds3"] + "'" + xml.dss["HorseOdds4"] + "'" + xml.dss["HorseOdds5"] + "'" + xml.dss["HorseOdds8"] + "'" + xml.dss["HorseOdds10"] + "'" + xml.dss["HorseOdds20"] + "'" + xml.dss["HorseOdds30"] + "'" + xml.dss["HorseOdds50"] + "'" + xml.dss["HorseOdds75"] + "'" + xml.dss["HorseOdds100"] + "'" + xml.dss["HorseOdds125"] + "'" + xml.dss["HorseOdds175"] + "'" + xml.dss["HorseOdds200"] + "'" + xml.dss["HorseOdds250"] + "'" + xml.dss["HorseOdds500"] + "'" + xml.dss["HorseMaxOutCent"] + "'1'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,horseset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />开奖概率使用千分比配置，可以控制赔率开出的概率.即次数比例.<br />当某期开奖的赔付总额超每期赔付上限时将重新随机.将赔付最大限度地减小.<br />开奖类型只有设置为概率随机才会应用此概率配置功能.");
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("game.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}