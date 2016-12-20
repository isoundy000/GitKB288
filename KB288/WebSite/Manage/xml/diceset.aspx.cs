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

public partial class Manage_xml_diceset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "挖宝游戏设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/dice.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "Dice口号限50字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string Tax = Utils.GetRequest("Tax", "post", 2, @"^[0-9]\d*$", "税率填写错误");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                string SmallPay2 = Utils.GetRequest("SmallPay2", "post", 2, @"^[0-9]\d*$", "最小下注填写错误");
                string BigPay2 = Utils.GetRequest("BigPay2", "post", 2, @"^[0-9]\d*$", "最大下注填写错误");
                string IDMaxPay = Utils.GetRequest("IDMaxPay", "post", 2, @"^[0-9]\d*$", "每ID每局最大下注填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写错误");
                string DtPay = Utils.GetRequest("DtPay", "post", 2, @"^[0-9]\d*$", "默认押注大小填写错误");
                string CycleMin = Utils.GetRequest("CycleMin", "post", 2, @"^[0-9]\d*$", "游戏周期填写错误");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");
                string OpenType = Utils.GetRequest("OpenType", "post", 2, @"^[0-1]$", "开奖类型选择出错");

                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }

                xml.dss["DiceName"] = Name;
                xml.dss["DiceNotes"] = Notes;
                xml.dss["DiceLogo"] = Logo;
                xml.dss["DiceStatus"] = Status;
                xml.dss["DiceTax"] = Tax;
                xml.dss["DiceSmallPay"] = SmallPay;
                xml.dss["DiceBigPay"] = BigPay;
                xml.dss["DiceSmallPay2"] = SmallPay2;
                xml.dss["DiceBigPay2"] = BigPay2;
                xml.dss["DiceIDMaxPay"] = IDMaxPay;
                xml.dss["DiceExpir"] = Expir;
                xml.dss["DiceDtPay"] = DtPay;
                xml.dss["DiceCycleMin"] = CycleMin;
                xml.dss["DiceOnTime"] = OnTime;
                xml.dss["DiceFoot"] = Foot;
                xml.dss["DiceGuestSet"] = GuestSet;
                xml.dss["DiceIsBot"] = IsBot;
                xml.dss["DiceOpenType"] = OpenType;
            }
            else
            {
                string Dx = Utils.GetRequest("Dx", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "大小赔率填写错误");
                string Ds = Utils.GetRequest("Ds", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "单双赔率填写错误");
                string Dz = Utils.GetRequest("Dz", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "对子赔率填写错误");
                string Bz = Utils.GetRequest("Bz", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "任意豹子赔率填写错误");
                string Bz2 = Utils.GetRequest("Bz2", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "指定豹子赔率填写错误");
                string Sum417 = Utils.GetRequest("Sum417", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率4|17赔率填写错误");
                string Sum516 = Utils.GetRequest("Sum516", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率5|16赔率填写错误");
                string Sum615 = Utils.GetRequest("Sum615", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率6|15赔率填写错误");
                string Sum714 = Utils.GetRequest("Sum714", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率7|14赔率填写错误");
                string Sum813 = Utils.GetRequest("Sum813", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率8|13赔率填写错误");
                string Sum9012 = Utils.GetRequest("Sum9012", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "总和赔率9012赔率填写错误");
                xml.dss["DiceDx"] = Dx;
                xml.dss["DiceDs"] = Ds;
                xml.dss["DiceDz"] = Dz;
                xml.dss["DiceBz"] = Bz;
                xml.dss["DiceBz2"] = Bz2;
                xml.dss["DiceSum417"] = Sum417;
                xml.dss["DiceSum516"] = Sum516;
                xml.dss["DiceSum615"] = Sum615;
                xml.dss["DiceSum714"] = Sum714;
                xml.dss["DiceSum813"] = Sum813;
                xml.dss["DiceSum9012"] = Sum9012;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("挖宝游戏设置", "设置成功，正在返回..", Utils.getUrl("diceset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "挖宝游戏设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("挖宝设置|");
                builder.Append("<a href=\"" + Utils.getUrl("diceset.aspx?ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">赔率</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("diceset.aspx?ptype=0&amp;backurl=" + Utils.getPage(0) + "") + "\">挖宝设置</a>");
                builder.Append("|赔率");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=9&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,系统收税(%):/,最小下注:/,最大下注:/,下注防刷(秒):/,押注默认大小:/,游戏周期(分钟):/,游戏开放时间(可留空):/,底部Ubb:/,兑奖内线:/,机器人:/,最小下注(" + ub.Get("SiteBz2") + "):/,最大下注(" + ub.Get("SiteBz2") + "):/,每ID每局最大下注(" + ub.Get("SiteBz2") + "):/,";
                string strName = "Name,Notes,Logo,Status,Tax,SmallPay,BigPay,Expir,DtPay,CycleMin,OnTime,Foot,GuestSet,IsBot,SmallPay2,BigPay2,IDMaxPay,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,num,text,text,select,select,num,num,num,hidden";
                string strValu = "" + xml.dss["DiceName"] + "'" + xml.dss["DiceNotes"] + "'" + xml.dss["DiceLogo"] + "'" + xml.dss["DiceStatus"] + "'" + xml.dss["DiceTax"] + "'" + xml.dss["DiceSmallPay"] + "'" + xml.dss["DiceBigPay"] + "'" + xml.dss["DiceExpir"] + "'" + xml.dss["DiceDtPay"] + "'" + xml.dss["DiceCycleMin"] + "'" + xml.dss["DiceOnTime"] + "'" + xml.dss["DiceFoot"] + "'" + xml.dss["DiceGuestSet"] + "'" + xml.dss["DiceIsBot"] + "'" + xml.dss["DiceSmallPay2"] + "'" + xml.dss["DiceBigPay2"] + "'" + xml.dss["DiceIDMaxPay"] + "'" + Utils.getPage(0) + "'";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,false,true,true,0|开启|1|关闭,0|关闭|1|开启,false,false,false,false";

                  int ManageId = new BCW.User.Manage().IsManageLogin();
                  if (ManageId == 1 || ManageId == 11)
                  {
                      strText += ",开奖类型:/";
                      strName += ",OpenType";
                      strType += ",select";
                      strValu += "" + xml.dss["DiceOpenType"] + "";
                      strEmpt += ",0|分析类型|1|随机类型";
                  }
                  else
                  {
                      strText += ",开奖类型:/";
                      strName += ",OpenType";
                      strType += ",hidden";
                      strValu += "" + xml.dss["DiceOpenType"] + "";
                      strEmpt += ",true";
                  }
                
                string strIdea = "/";
                string strOthe = "确定修改|reset,diceset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                //builder.Append(Out.Tab("<div>", "<br />"));
                //builder.Append("温馨提示:<br />随机开奖指正常的随机开出三个骰子，不输开奖指自动开出最少购买的选项赢.<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                //builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "大小赔率:/,单双赔率:/,对子赔率:/,任意豹子:/,指定豹子:/,=总和赔率=/4|17:/,5|16:/,6|15:/,7|14:/,8|13:/,9|10|11|12:/,,";
                string strName = "Dx,Ds,Dz,Bz,Bz2,Sum417,Sum516,Sum615,Sum714,Sum813,Sum9012,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["DiceDx"] + "'" + xml.dss["DiceDs"] + "'" + xml.dss["DiceDz"] + "'" + xml.dss["DiceBz"] + "'" + xml.dss["DiceBz2"] + "'" + xml.dss["DiceSum417"] + "'" + xml.dss["DiceSum516"] + "'" + xml.dss["DiceSum615"] + "'" + xml.dss["DiceSum714"] + "'" + xml.dss["DiceSum813"] + "'" + xml.dss["DiceSum9012"] + "'" + ptype + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,0|随机开奖|1|不输开奖,false,false,false,false,true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,diceset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:赔率支持小数点，如填写1.9");
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