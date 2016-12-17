using System;
using BCW.Common;
using System.Collections.Generic;

public partial class Manage_xml_klsfset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected int gid = 32;
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "快乐十分设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/klsf.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,5000}$", "口号限5000字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string Sec = Utils.GetRequest("Sec", "post", 2, @"^[0-9]\d*$", "秒数填写出错");
                string SmallPay = Utils.GetRequest("SmallPay", "post", 2, @"^[0-9]\d*$", "最小下注" + ub.Get("SiteBz") + "填写错误");
                string BigPay = Utils.GetRequest("BigPay", "post", 2, @"^[0-9]\d*$", "最大下注" + ub.Get("SiteBz") + "填写错误");
                string Price = Utils.GetRequest("Price", "post", 2, @"^[0-9]\d*$", "每期每ID限购多少" + ub.Get("SiteBz") + "填写错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string OnTime = Utils.GetRequest("OnTime", "post", 3, @"^[0-9]{2}:[0-9]{2}-[0-9]{2}:[0-9]{2}$", "游戏开放时间填写错误，可留空");
                string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string GuestSet = Utils.GetRequest("GuestSet", "post", 2, @"^[0-1]$", "兑奖内线选择出错");
                string IsBot = Utils.GetRequest("IsBot", "post", 2, @"^[0-1]$", "机器人选择出错");
                string IsSWB = Utils.GetRequest("IsSWB", "post", 2, @"^[0-1]$", "快乐币选择出错");        

                //继续验证时间
                if (OnTime != "")
                {
                    string[] temp = OnTime.Split("-".ToCharArray());
                    DateTime dt1 = Utils.ParseTime(temp[0]);
                    DateTime dt2 = Utils.ParseTime(temp[1]);
                }
                xml.dss["klsfName"] = Name;
                xml.dss["klsfNotes"] = Notes;
                xml.dss["klsfLogo"] = Logo;
                xml.dss["klsfStatus"] = Status;
                xml.dss["klsfSec"] = Sec;
                xml.dss["klsfSmallPay"] = SmallPay;
                xml.dss["klsfBigPay"] = BigPay;
                xml.dss["klsfPrice"] = Price;
                xml.dss["klsfExpir"] = Expir;
                xml.dss["klsfOnTime"] = OnTime;
                xml.dss["klsfFoot"] = Foot;
                xml.dss["klsfGuestSet"] = GuestSet;
                xml.dss["klsfIsBot"] = IsBot;
                xml.dss["klsfSWB"] = IsSWB;
                
            }
            else if (ptype == 1)
            {
                for (int i = 1; i <= 17; i++)
                {
                    string Odds = "";
                    Odds = Utils.GetRequest("Odds" + i + "", "post", 2, @"^[0-9]\d*\.?\d{0,2}$", "赔率错误");

                    xml.dss["klsfOdds" + i + ""] = Odds;
                }
            }
            else if (ptype == 2)
            {
                string RobotId = Utils.GetRequest("RobotId", "post", 3, @"^(((\d){1,11}#)*(\d){1,11})?$", "机器人ID输入错误");
                string BuyCent = Utils.GetRequest("BuyCent", "post", 2, @"^(\d){1,5}$", "输入错误");
                string RobotBuy = Utils.GetRequest("RobotBuy", "post", 2, @"^(\d){1,3}$", "输入错误");

                xml.dss["klsfRobotId"] = RobotId;
                xml.dss["klsfRobotBuyCent"] = BuyCent;
                xml.dss["klsfRobotBuy"] = RobotBuy;
            }
            else
            {
                string GetTime = Utils.GetRequest("GetTime", "post", 2, @"^[0-9]\d*$", "时间选择出错");
                string GetMoneyMax = Utils.GetRequest("GetMoneyMax", "post", 2, @"^[0-9]\d*$", "金额选择出错");
                string GetMoney = Utils.GetRequest("GetMoney", "post", 2, @"^[0-9]\d*$", "金额选择出错");
                string newTestID = Utils.GetRequest("newTestID", "post", 3, @"^(((\d){1,11}#)*(\d){1,11})?$", "ID填写错误");
                string delTestID = Utils.GetRequest("delTestID", "post", 3, @"^(((\d){1,11}#)*(\d){1,11})?$", "ID填写错误");

                xml.dss["GetTime"] = GetTime;
                xml.dss["GetMoney"] = GetMoney;
                xml.dss["GetMoneyMax"] = GetMoneyMax;
                if (newTestID != "")
                {
                    string[] TestID = newTestID.Split('#');
                    int ID = 0;
                    for (int i = 0; i < TestID.Length; i++)
                    {
                        ID = Convert.ToInt32(TestID[i]);
                        if (new BCW.BLL.User().Exists(ID))
                        {
                            if (new BCW.SWB.BLL().ExistsUserID(ID, gid))
                                new BCW.SWB.BLL().UpdatePermission(ID, 1, gid);
                            else
                            {
                                BCW.SWB.Model model = new BCW.SWB.Model();
                                model.UserID = ID;
                                model.GameID = gid;
                                model.Money = 0;
                                model.UpdateTime = DateTime.Now;
                                model.Permission = 1;
                                new BCW.SWB.BLL().Add(model);
                            }
                        }
                    }
                }
                if (delTestID != "")
                {
                    string[] TestId = delTestID.Split('#');
                    string strWhere = string.Empty;
                    int tID = 0;
                    for (int i = 0; i < TestId.Length; i++)
                    {
                        tID = Convert.ToInt32(TestId[i]);
                        if (new BCW.SWB.BLL().ExistsUserID(tID, gid))
                            new BCW.SWB.BLL().UpdatePermission(tID, 0, gid);
                    }
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("快乐十分设置", "设置成功，正在返回..", Utils.getUrl("klsfset.aspx?ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "快乐十分设置"));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("快乐十分设置");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=1") + "\">赔率</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=2") + "\">机器人设置</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=3") + "\">试玩版设置</a>");
            }
            else if(ptype == 1)
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=0") + "\">快乐十分设置</a>");
                builder.Append("|赔率");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=2") + "\">机器人设置</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=3") + "\">试玩版设置</a>");
            }
            else if (ptype == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=0") + "\">快乐十分设置</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=1") + "\">赔率</a>");
                builder.Append("|机器人设置");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=3") + "\">试玩版设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=0") + "\">快乐十分设置</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=1") + "\">赔率</a>");
                builder.Append("|<a href=\"" + Utils.getUrl("klsfset.aspx?ptype=2") + "\">机器人设置</a>");
                builder.Append("|试玩版设置");
            }
            builder.Append("|<a href=\"" + Utils.getUrl("spkadminset.aspx?ptype=22&amp;backurl=" + Utils.PostPage(true) + "") + "\">管理员</a>");
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,离截止时间N秒前不能下注/,最小下注" + ub.Get("SiteBz") + ":/,最大下注" + ub.Get("SiteBz") + ":/,每期每ID限购多少" + ub.Get("SiteBz") + "(填0则不限制):/,下注防刷(秒):/,游戏开放时间(可留空):/,底部Ubb:/,兑奖内线:/,机器人:/,快乐币：(切换前需要重置游戏)/,";
                string strName = "Name,Notes,Logo,Status,Sec,SmallPay,BigPay,Price,Expir,OnTime,Foot,GuestSet,IsBot,IsSWB,backurl";
                string strType = "text,text,text,select,num,num,num,num,num,text,textarea,select,select,select,hidden";
                string strValu = "" + xml.dss["klsfName"] + "'" + xml.dss["klsfNotes"] + "'" + xml.dss["klsfLogo"] + "'" + xml.dss["klsfStatus"] + "'" + xml.dss["klsfSec"] + "'" + xml.dss["klsfSmallPay"] + "'" + xml.dss["klsfBigPay"] + "'" + xml.dss["klsfPrice"] + "'" + xml.dss["klsfExpir"] + "'" + xml.dss["klsfOnTime"] + "'" + xml.dss["klsfFoot"] + "'" + xml.dss["klsfGuestSet"] + "'" + xml.dss["klsfIsBot"] + "'" + xml.dss["klsfSWB"] + "'" +  Utils.getPage(0) + "";
                string strEmpt = "false,true,true,0|正常|1|维护,false,false,false,false,false,true,true,0|开启|1|关闭,0|关闭|1|开启,0|开启|1|关闭,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsfset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />游戏开放时间填写格式为:09:00-18:00，留空则全天开放.");
                builder.Append(Out.Hr());
                builder.Append("转换快乐币模式的时候需要先重置游戏，避免数据冲突.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else if(ptype == 1)
            {
                string strText = "任选五胆拖:,任选五普通:,任选四胆拖:,任选四普通:,任选三胆拖:,任选三普通:,任选二胆拖:,任选二普通:,连二直选:,连二组选:,前一红投:,前一数投:,大:,小:,单:,双:,浮动赔率:,,";
                string strName = "Odds1,Odds2,Odds3,Odds4,Odds5,Odds6,Odds7,Odds8,Odds9,Odds10,Odds11,Odds12,Odds13,Odds14,Odds15,Odds16,Odds17,ptype,backurl";
                string strType = "text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,text,hidden,hidden";
                string strValu = "" + xml.dss["klsfOdds1"] + "'" + xml.dss["klsfOdds2"] + "'" + xml.dss["klsfOdds3"] + "'" + xml.dss["klsfOdds4"] + "'" + xml.dss["klsfOdds5"] + "'" + xml.dss["klsfOdds6"] + "'" + xml.dss["klsfOdds7"] + "'" + xml.dss["klsfOdds8"] + "'" + xml.dss["klsfOdds9"] + "'" + xml.dss["klsfOdds10"] + "'" + xml.dss["klsfOdds11"] + "'" + xml.dss["klsfOdds12"] + "'" + xml.dss["klsfOdds13"] + "'" + xml.dss["klsfOdds14"] + "'" + xml.dss["klsfOdds15"] + "'" + xml.dss["klsfOdds16"] + "'" + xml.dss["klsfOdds17"] + "'1'" + Utils.getPage(1) + "";
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsfset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else if (ptype == 2)
            {
                string strText = "机器人ID(用#隔开):/,机器人每注购买金额:/,每个机器人每期最高购买次数:/,,";
                string strName = "RobotID,BuyCent,RobotBuy,ptype,backurl";
                string strType = "text,num,num,hidden,hidden";
                string strValu = "" + xml.dss["klsfRobotId"] + "'" + xml.dss["klsfRobotBuyCent"] + "'" + xml.dss["klsfRobotBuy"] + "'2'" + Utils.getPage(2) + "";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsfset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "快乐币获取时间间隔（分钟）/,快乐币最大获取金额/,快乐币每次获取金额/,新增测试会员ID（用#隔开）/,要取消测试资格的测试会员ID（用#隔开）/,,";
                string strName = "GetTime,GetMoneyMax,GetMoney,newTestID,delTestID,ptype,backurl";
                string strType = "num,num,num,text,text,hidden,hidden";
                string strValu = "" + xml.dss["GetTime"] + "'" + xml.dss["GetMoneyMax"] + "'" + xml.dss["GetMoney"] + "'''3'" + Utils.getPage(3) + "";
                string strEmpt = "false,false,false,true,true,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,klsfset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Hr());
                
                #region 显示测试用户
                int pageIndex;  //页数
                int recordCount;  //记录
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo")); //每页显示的记录的多少
                string strWhere = string.Empty;  //声明匹配条件
                strWhere = "Permission = 1 and GameID = " + gid; 
                string[] pageValUrl = { "ptype","backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                IList<BCW.SWB.Model> listSWB = new BCW.SWB.BLL().GetListByPage(pageIndex, pageSize, strWhere, out recordCount);
                //builder.Append(pageIndex + "," + pageSize + "," + strWhere + "," + recordCount + "," + listSWB.Count + "<br/>");
                if (listSWB.Count > 0)
                {
                    int k = 1;
                    string UsName = string.Empty;
                    builder.Append("目前的测试用户有<br/>");
                    foreach (BCW.SWB.Model n in listSWB)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        UsName = new BCW.BLL.User().GetUsName(n.UserID);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");

                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有测试用户.."));
                }
                #endregion
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
