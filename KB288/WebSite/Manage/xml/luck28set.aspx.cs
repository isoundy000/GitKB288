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

public partial class Manage_xml_luck28set : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "幸运28游戏设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/luck28.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Luck28口号限50字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-2]$", "系统状态选择出错");
                string OType = Utils.GetRequest("OType", "post", 2, @"^[0-1]$", "开奖模式选择出错");
                //string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-2]$", "开奖类型选择出错");Luck28EveryPay
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string EveryPay = Utils.GetRequest("EveryPay", "post", 2, @"^[0-9]\d+$", "每期每ID最大投注填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小投注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大投注填写错误");
                string MaxPool = Utils.GetRequest("MaxPool", "post", 4, @"^[0-9]\d*$", "奖池最大限额填写错误");
                // string IDPay = Utils.GetRequest("IDPay", "post", 4, @"^[0-9]\d*$", "每ID每期下注限制填写错误");
                // string SysPay = Utils.GetRequest("SysPay", "post", 4, @"^[0-9]\d*$", "系统每期投入多少币填写错误");
                //    string CycleMin = Utils.GetRequest("CycleMin", "post", 2, @"^[0-9]\d*$", "游戏周期填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "游戏防刷填写错误");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择错误");
                string TestID = Utils.GetRequest("TestID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "内测ID请用#分隔，可以留空");
                string RobotIDS = Utils.GetRequest("RobotIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "机器人ID请用#分隔，可以留空");
                string RobotSet = Utils.GetRequest("RobotSet", "post", 2, @"^[0-1]$", "机器人开关选择错误");
                string RobotCent = Utils.GetRequest("RobotCent", "post", 2, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "机器人下注币填写出错");
                string robotbuy1 = (Utils.GetRequest("robotbuy", "post", 2, @"^\d+$", "设置机械人下注次数填写出错"));
                string overDay = (Utils.GetRequest("overDay", "post", 2, @"^\d+$", "设置过期兑奖天数填写出错"));
                string cfnum = Utils.GetRequest("cfnum", "post", 2, @"^[0-9]\d*$", "X期连开才开起浮动");
                string[] aa = RobotCent.Split('#');
                int min = Utils.ParseInt(aa[0]);
                int max = Utils.ParseInt(aa[0]);
                for (int i = 1; i < aa.Length; i++)
                {
                    int mintemp = Utils.ParseInt(aa[i]);
                    int maxtemp = Utils.ParseInt(aa[i]);
                    if (min > mintemp)//找最小值
                    {
                        min = mintemp;
                    }
                    if (max < maxtemp)//找最大值
                    {
                        max = maxtemp;
                    }
                }
                // Utils.Error("min:" + min+ ",max:"+ max, "");
                int a = Utils.ParseInt(SmallPay);
                int b = Utils.ParseInt(BigPay);
                int a1 = min;
                int b1 = max;
                if (a1 > b || a1 < a)
                {
                    Utils.Error("机械人最小下注金额填写出错，最小为:" + a, "");
                }
                if (b1 > b || b1 < a)
                {
                    Utils.Error("机械人最大下注金额填写出错,最大为" + b, "");
                }
                if (a1 > b1)
                {
                    Utils.Error("机器人下注币填写出错,最小下注不能大于最大下注", "");
                }
                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["overDay"] = overDay;
                xml.dss["Luck28OpenType"] = OType;
                xml.dss["Luck28RobotBuy"] = robotbuy1;
                xml.dss["Luck28TestID"] = TestID;
                xml.dss["Luck28Name"] = Name;
                xml.dss["Luck28Notes"] = Notes;
                xml.dss["Luck28Logo"] = Logo;
                xml.dss["Luck28Status"] = Status;
                //xml.dss["Luck28OpenType"] = OpenType;
                xml.dss["Luck28Tax"] = Tax;
                xml.dss["Luck28SmallPay"] = SmallPay;
                xml.dss["Luck28BigPay"] = BigPay;
                xml.dss["Luck28EveryPay"] = EveryPay;
                xml.dss["Luck28MaxPool"] = MaxPool;
                // xml.dss["Luck28IDPay"] = IDPay;
                // xml.dss["Luck28SysPay"] = SysPay;
                //   xml.dss["Luck28CycleMin"] = CycleMin;
                xml.dss["Luck28Expir"] = Expir;
                xml.dss["Luck28OnTime"] = OnTime;
                xml.dss["Luck28Foot"] = Foot;
                xml.dss["Luck28GuestSet"] = GuestSet;
                xml.dss["Luck28RobotIDS"] = RobotIDS.Replace("\r\n", "").Replace(" ", "");
                xml.dss["Luck28RobotSet"] = RobotSet;
                xml.dss["Luck28RobotCent"] = RobotCent.Replace("\r\n", "").Replace(" ", "");
                xml.dss["cfnum"] = cfnum;
            }
            else
            {
                int Num = 0;
                for (int i = 0; i < 28; i++)
                {
                    int fNum = int.Parse(Utils.GetRequest("Num" + i + "", "post", 2, @"^[0-9]\d*$", "号码" + i + "千分比填写错误"));
                    xml.dss["Luck28Num" + i + ""] = Request["Num" + i + ""];
                    Num += fNum;
                }
                if (Num != 1000)
                {
                    Utils.Error("赔率百分比总和必须是1000", "");
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("幸运28游戏设置", "设置成功，正在返回..", Utils.getUrl("luck28set.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("../game/luck28.aspx") + "\">" + ub.GetSub("Luck28Name", xmlPath) + "</a>&gt;游戏配置");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (Utils.GetDomain().Contains("168yy") || Utils.GetDomain().Contains("tl88") || Utils.GetDomain().Contains("127.0.0.6"))
            {
                if (ptype == 0)
                {
                    builder.Append("游戏设置|");
                    builder.Append("<a href=\"" + Utils.getUrl("luck28set.aspx?ptype=1") + "\">概率</a>");
                }
                else
                {
                    builder.Append("<a href=\"" + Utils.getUrl("luck28set.aspx?ptype=0") + "\">游戏设置</a>");
                    builder.Append("|概率");
                }
                //builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=6&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            }
            else
            {
                builder.Append("游戏设置|");//<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=6&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>
            }
            builder.Append("<a href=\"" + Utils.getUrl("../game/luck28.aspx?act=baseset") + "\">基本赔率</a>");
            builder.Append(Out.Tab("</div>", ""));
            if ((!Utils.GetDomain().Contains("168yy") && !Utils.GetDomain().Contains("tl88") && !Utils.GetDomain().Contains("127.0.0.6")) || ptype == 0)
            {

                string strText = "游戏名称:/,开奖模式:/,游戏口号(可留空):/,游戏状态:/,X期连开才开起浮动：(系统不能设置超过6)/,系统收税(‰千分):/,内测ID:/,每期下注总额:/,玩家单次最小投注:/,玩家单次最大投注:/,每期每ID最大下注额:/,游戏下注防刷(秒):/,游戏开放时间(可留空):/,底部Ubb:/,超过多少天自动回收" + ub.Get("SiteBz") + ":/,兑奖内线:/,=土豪网功能=/机器人ID(用#分开):/,机器人开关:/,机器人下注币(如填写“100#200”):/,机械人下注次数:/,";
                string strName = "Name,OType,Notes,Status,cfnum,Tax,TestID,MaxPool,SmallPay,BigPay,EveryPay,Expir,OnTime,Foot,overDay,GuestSet,RobotIDS,RobotSet,RobotCent,robotbuy,backurl";
                string strType = "text,select,text,select,text,text,text,num,num,num,num,num,text,text,num,select,big,select,big,num,hidden";
                string strValu = "" + xml.dss["Luck28Name"] + "'" + xml.dss["Luck28OpenType"] + "'" + xml.dss["Luck28Notes"] + "'" + xml.dss["Luck28Status"] + "'" + xml.dss["cfnum"] + "'" + xml.dss["Luck28Tax"] + "'" + xml.dss["Luck28TestID"] + "'" + xml.dss["Luck28MaxPool"] + "'" + xml.dss["Luck28SmallPay"] + "'" + xml.dss["Luck28BigPay"] + "'" + xml.dss["Luck28EveryPay"] + "'" + xml.dss["Luck28Expir"] + "'" + xml.dss["Luck28OnTime"] + "'" + xml.dss["Luck28Foot"] + "'" + xml.dss["overDay"] + "'" + xml.dss["Luck28GuestSet"] + "'" + xml.dss["Luck28RobotIDS"] + "'" + xml.dss["Luck28RobotSet"] + "'" + xml.dss["Luck28RobotCent"] + "'" + ub.GetSub("Luck28RobotBuy", xmlPath) + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,0|抓取开奖|1|人工开奖,true,0|正常|1|维护|2|内测,false,false,false,false,false,false,false,false,true,true,false,0|开启|1|关闭,true,0|开启|1|关闭,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,luck28set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "";
                string strName = "";
                string strType = "";
                string strValu = "";
                string strEmpt = "";
                for (int i = 0; i < 28; i++)
                {
                    strText += "号码" + i + "千分比,";
                    strName += "Num" + i + ",";
                    strType += "snum,";
                    strValu += "" + xml.dss["Luck28Num" + i + ""] + "'";
                    strEmpt += "false,";
                }
                strText += "";
                strName += "ptype";
                strType += "hidden";
                strValu += "1";
                strEmpt += "false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,luck28set.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />开奖概率使用千分比配置，可以控制赔率开出的概率.即次数比例.<br />开奖类型只有设置为概率随机才会应用此概率配置功能.");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("../game/luck28.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
