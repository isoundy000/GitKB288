using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using BCW.PK10.Model;

public partial class Manage_xml_PK10set : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/PK10.xml";
    protected string myFileName = "PK10set.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "save":
                SavePage();
                break;
            case "saverate":
                SaveWinPrice();
                break;
            case "saverate2":
                SaveWinPrice2();
                break;
            case "saverate3":
                SaveWinPrice3();
                break;
            default:
                ShowPage();
                break;
        }
    }
    private void ShowPage()
    {
        Master.Title = "PK拾配置";
        //
        #region 显示设置的类型
        string cShowType = Utils.GetRequest("showtype", "get", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        builder.Append(Out.Tab("<div class=\"text\" > ", Out.Hr()));
        switch (showtype)
        {
            case 1:
                builder.Append("<a href =\"" + Utils.getUrl(myFileName +"?showtype=0") + "\">基本设置 | </a>");
                builder.Append("<font color=\"red\">赔率设置</font>");
                break;
            default:
                builder.Append("<font color=\"red\">基本设置 | </font>");
                builder.Append("<a href =\"" + Utils.getUrl(myFileName +"?showtype=1") + "\">赔率设置</a>");
                break;
        }
        builder.Append(Out.Tab("</div> ", Out.Hr()));
        #endregion
        //
        #region 显示设置的内容
        switch (showtype)
        {
            case 1:
                ShowRatePage();
                break;
            default:
                ShowBasePage();
                break;
        }
        #endregion
        #region 底部
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("../game/PK10.aspx") + "\">返回上级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
        #endregion
    }
    private void ShowBasePage()
    {
        //
        Application.Remove(xmlPath);//清缓存
        ub xml = new ub();
        xml.ReloadSub(xmlPath); //加载配置
        //
        #region 设置标题提示字符串
        string strText = "/,";
        strText += "游戏名称:" + "/,";
        strText += "游戏口号(可留空):" + "/,";
        strText += "游戏提示(可留空):" + "/,";
        strText += "游戏Logo(可留空):" + "/,";
        strText += "游戏状态:" + "/,";
        strText += "内测人员ID(用#号分开):" + "/,";
        strText += "测试人员默认分配的PK币:" + "/,";
        strText += "每天第一期开奖时间(09:07:30):" + "/,";
        strText += "每天停售时间(23:57:30):" + "/,";
        strText += "每天期数(179期):" + "/,";
        strText += "每期销售时长(5分钟):" + "/,";
        strText += "开将前X秒停售(最少30秒):" + "/,";
        strText += "每期停售X秒后抓取开奖数据(60秒):" + "/,";
        strText += "每注最小投注:" + "/,";
        strText += "每注最大投注:" + "/,";
        strText += "每人每期最大投注:" + "/,";
        strText += "兑奖收税1(按中奖金额的‰):" + "/,";
        strText += "兑奖收税2(每注中奖固定收取费用):" + "/,";
        strText += "兑奖有效期长(天):" + "/,";
        strText += "游戏前端页面显示分页数据的页长(行):" + "/,";
        strText += "抓取开奖数据后自动计算派奖" + "/,";
        strText += "机器人开关:" + "/,";
        strText += "机器人ID(用#号分开):" + "/,";
        strText += "每期机器人累计下注次数限制:" + "/,";
        strText += "机器人下注单价(用#号分开):" + "/,";
        strText += "连续多少期没有开奖数据将暂时停售:" + "/,";
        strText += "下注页面显示X期号码:" + "/,";
        strText += "下注页面每次展开X期号码:" + "/,";
        strText += "默认快捷下注(如:2千|2000#2万|20000):" + "/,";
        //20万|200000#40万|400000#60万|600000#80万|800000#100万|1000000
        #endregion
        #region 设置控件名称
        string strName = "act,GameName,Notes,SaleMsg,LogoImage,GameStatus,TestUserID,TestUserDefaultMoney,GameBeginTime,GameEndTime,GameOpenTimes,SaleTimes,StopSaleSec,OpenTimes,";
        strName += "MinPayPrice,MaxPayPrice,UserMaxPay,WinChargeRate,WinCharge,ValidDays,ShowDataPageSize,AutoCalc,isRobot,Robots,RobotMaxTimes,RobotPrice,StopSaleWhenLostData,showSaleDataRecordCount,showSaleDataStep,defaultPaySettings";
        #endregion
        #region 设置控件类型
        string strType = "hidden,text,text,text,text,select,text,num,text,text,num,num,num,num,num,num,num,num,num,num,num,select,select,text,num,text,num,num,num,text";
        #endregion
        #region 设置控件值
        string strValu = "save'";
        strValu += xml.dss["GameName"] + "'";
        strValu += xml.dss["Notes"] + "'";
        strValu += xml.dss["SaleMsg"] + "'";
        strValu += xml.dss["LogoImage"] + "'";
        strValu += xml.dss["GameStatus"] + "'";
        strValu += xml.dss["TestUserID"] + "'";
        strValu += xml.dss["TestUserDefaultMoney"] + "'";
        strValu += xml.dss["GameBeginTime"] + "'";
        strValu += xml.dss["GameEndTime"] + "'";
        strValu += xml.dss["GameOpenTimes"] + "'";
        strValu += xml.dss["SaleTimes"] + "'";
        strValu += xml.dss["StopSaleSec"] + "'";
        strValu += xml.dss["OpenTimes"] + "'";
        strValu += xml.dss["MinPayPrice"] + "'";
        strValu += xml.dss["MaxPayPrice"] + "'";
        strValu += xml.dss["UserMaxPay"] + "'";
        strValu += xml.dss["WinChargeRate"] + "'";
        strValu += xml.dss["WinCharge"] + "'";
        strValu += xml.dss["ValidDays"] + "'";
        strValu += xml.dss["ShowDataPageSize"] + "'";
        strValu += xml.dss["AutoCalc"] + "'";
        strValu += xml.dss["isRobot"] + "'";
        strValu += xml.dss["Robots"]+"'";
        strValu += xml.dss["RobotMaxTimes"] + "'";
        strValu += xml.dss["RobotPrice"] + "'";
        strValu += xml.dss["StopSaleWhenLostData"] + "'";
        strValu += xml.dss["showSaleDataRecordCount"] + "'";
        strValu += xml.dss["showSaleDataStep"]+"'";
        strValu += xml.dss["defaultPaySettings"];
        
        #endregion
        #region 设置控件
        string strEmpt = "false,false,true,true,true," + "0|正常|1|维护|2|内测（PK币）|3|公测（PK币）|4|内测（"+ ub.Get("SiteBz") + "）" + ",true,false,false,false,false,false,false,false,false,false,true,true,false,false,false," + "0|否|1|是" + "," + "0|关闭|1|开启" + ",true,false,false,false,false,false,false";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void ShowRatePage()
    {
        #region 显示各种玩法
        builder.Append(Out.Tab("<div>", ""));
        //
        int buyTypeID = 0;
        string cBuyType = Utils.GetRequest("buytype", "get", 1, "", "0");
        int.TryParse(cBuyType, out buyTypeID);
        List<PK10_BuyType> oBuyTypes = new PK10().GetBuyTypes2();
        if (oBuyTypes != null && oBuyTypes.Count > 0)
        {
            if (buyTypeID == 0)
                buyTypeID = oBuyTypes[0].ID;
            string shownavi = "";
            int nparentid = oBuyTypes[0].ParentID;
            for (int i = 0; i < oBuyTypes.Count; i++)
            {
                if (oBuyTypes[i].ParentID != nparentid)
                {
                    shownavi += Out.Tab("<div class=\"hr\"></div>", Out.Hr());
                    nparentid = oBuyTypes[i].ParentID;
                }
                else
                {
                    if (shownavi != "")
                        shownavi += "|";
                }
                string showtext = oBuyTypes[i].Name.Trim();
                if (oBuyTypes[i].ID == buyTypeID)
                    showtext = "<font color=\"red\">" + showtext + "</font>";
                string str = "<a href=\"" + Utils.getUrl(myFileName + "?showtype=1&amp;buyType=" + oBuyTypes[i].ID.ToString().Trim()) + "\">" + showtext + "</a> ";
                shownavi += str;
            }
            builder.Append(shownavi);
        }
        else
            builder.Append("读取数据失败，可能系统没有初始化数据！");
        //
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示赔率内容
        PK10_BuyType otype = new PK10().GetBuyTypeByID(buyTypeID);
        if(otype!=null)
        {
            switch(otype.ParentID)
            {
                case 1:
                    ShowRatePage1(otype);
                    break;
                case 2:
                    ShowRatePage2(otype);
                    break;
                case 3:
                    ShowRatePage3(otype);
                    break;
                case 4:
                    ShowRatePage4(otype);
                    break;
                case 5:
                    ShowRatePage5(otype);
                    break;
                case 6:
                    ShowRatePage6(otype);
                    break;
            }
        }
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:<br />");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void SavePage()
    {
        string GameName = Utils.GetRequest("GameName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
        string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "游戏口号限50字内");
        string SaleMsg = Utils.GetRequest("SaleMsg", "post", 3, @"^[^\^]{1,50}$", "游戏提示限50字内");
        string LogoImage = Utils.GetRequest("LogoImage", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
        string GameStatus = Utils.GetRequest("GameStatus", "post", 2, @"^[0-4]$", "系统状态选择出错");
        string TestUserID = Utils.GetRequest("TestUserID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "测试人ID请用#分隔，可以留空");
        string TestUserDefaultMoney = Utils.GetRequest("TestUserDefaultMoney", "post", 4, @"^[0-9]\d*$", "测试人员默认分配的PK币填写错误");
        string GameBeginTime = Utils.GetRequest("GameBeginTime", "post", 3, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", "每天第一期开奖时间填写错误，可留空");
        string GameEndTime = Utils.GetRequest("GameEndTime", "post", 3, @"^[0-9]{2}:[0-9]{2}:[0-9]{2}$", "游戏停售时间填写错误，可留空");
        string GameOpenTimes = Utils.GetRequest("GameOpenTimes", "post", 4, @"^[0-9]\d*$", "每天期数填写错误");
        string SaleTimes = Utils.GetRequest("SaleTimes", "post", 4, @"^[0-9]\d*$", "每期销售时长填写错误");
        string StopSaleSec = Utils.GetRequest("StopSaleSec", "post", 4, @"^[0-9]\d*$", "开将前X秒停售,填写错误");
        string OpenTimes = Utils.GetRequest("OpenTimes", "post", 4, @"^[0-9]\d*$", "每期停售后X秒抓取开奖填写错误");
        string MinPayPrice = Utils.GetRequest("MinPayPrice", "post", 4, @"^[0-9]\d*$", "每注最小投注填写错误");
        string MaxPayPrice = Utils.GetRequest("MaxPayPrice", "post", 4, @"^[0-9]\d*$", "每注最大投注填写错误");
        string UserMaxPay = Utils.GetRequest("UserMaxPay", "post", 4, @"^[0-9]\d*$", "每人每期最大投注填写错误");
        string WinChargeRate = Utils.GetRequest("WinChargeRate", "post", 4, @"^[0-9]\d*$", "按中奖金额的‰填写错误");
        string WinCharge = Utils.GetRequest("WinCharge", "post", 4, @"^[0-9]\d*$", "每注中奖固定收取费用填写错误");
        string ValidDays = Utils.GetRequest("ValidDays", "post", 4, @"^[0-9]\d*$", "兑奖有效期长填写错误");
        string ShowDataPageSize = Utils.GetRequest("ShowDataPageSize", "post", 4, @"^[0-9]\d*$", "游戏前端页面显示分页数据的页长填写错误");
        string AutoCalc = Utils.GetRequest("AutoCalc", "post", 2, @"^[0-1]$", "抓取开奖数据后自动计算派奖选择出错");
        string isRobot = Utils.GetRequest("isRobot", "post", 2, @"^[0-1]$", "机器人开关选择出错");
        string Robots = Utils.GetRequest("Robots", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "机器人ID请用#分隔，可以留空");
        string RobotMaxTimes = Utils.GetRequest("RobotMaxTimes", "post", 4, @"^[0-9]\d*$", "每期机器人累计下注次数限制,设置错误");
        string RobotPrice = Utils.GetRequest("RobotPrice", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "机器人下注单价(用#分开),设置错误");
        string StopSaleWhenLostData = Utils.GetRequest("StopSaleWhenLostData", "post", 4, @"^[0-9]\d*$", "联系多少期没有开奖数据将暂停销售,设置错误");
        string showSaleDataRecordCount = Utils.GetRequest("showSaleDataRecordCount", "post", 4, @"^[0-9]\d*$", "下注页面显示X期号码,设置错误");
        string showSaleDataStep = Utils.GetRequest("showSaleDataStep", "post", 4, @"^[0-9]\d*$", "下注页面每次展开X期号码,设置错误");
        string defaultPaySettings= Utils.GetRequest("defaultPaySettings", "post", 3, @"^[^\^]{1,500}$", "默认快捷下注");
        //
        ub xml = new ub();
        xml.ReloadSub(xmlPath); //加载配置
        xml.dss["GameName"] = GameName;
        xml.dss["Notes"] = Notes;
        xml.dss["SaleMsg"] = SaleMsg;
        xml.dss["LogoImage"] = LogoImage;
        xml.dss["GameStatus"] = GameStatus;
        xml.dss["TestUserID"] = TestUserID;
        xml.dss["TestUserDefaultMoney"] = TestUserDefaultMoney;
        xml.dss["GameBeginTime"] = GameBeginTime;
        xml.dss["GameEndTime"] = GameEndTime;
        xml.dss["GameOpenTimes"] = GameOpenTimes;
        xml.dss["SaleTimes"] = SaleTimes;
        xml.dss["StopSaleSec"] = StopSaleSec;
        xml.dss["OpenTimes"] = OpenTimes;
        xml.dss["MinPayPrice"] = MinPayPrice;
        xml.dss["MaxPayPrice"] = MaxPayPrice;
        xml.dss["UserMaxPay"] = UserMaxPay;
        xml.dss["WinChargeRate"] = WinChargeRate;
        xml.dss["WinCharge"] = WinCharge;
        xml.dss["ValidDays"] = ValidDays;
        xml.dss["ShowDataPageSize"] = ShowDataPageSize;
        xml.dss["AutoCalc"] = AutoCalc;
        xml.dss["isRobot"] = isRobot;
        xml.dss["Robots"] = Robots;
        xml.dss["RobotMaxTimes"] = RobotMaxTimes;
        xml.dss["RobotPrice"] = RobotPrice;
        xml.dss["StopSaleWhenLostData"] = StopSaleWhenLostData;
        xml.dss["showSaleDataRecordCount"] = showSaleDataRecordCount;
        xml.dss["showSaleDataStep"] = showSaleDataStep;
        xml.dss["defaultPaySettings"] = defaultPaySettings;
        //
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        Utils.Success("PK拾游戏设置", "设置成功，正在返回..", Utils.getUrl(myFileName), "1");
    }
    private  void SaveWinPrice()
    {
        string backurl = Utils.GetRequest("backrul", "post", 1,"","");
        int id = Convert.ToInt32(Utils.GetRequest("buytype", "post", 2, @"^[0-9]\d*$", "ID设置读取错误"));
        int MultiSelect = Convert.ToInt32(Utils.GetRequest("MultiSelect", "post", 2, @"^[0-1]$", "复式或者单式选择错误"));
        decimal d0=0, d1=0, d2=0, d3=0, d4=0, d5=0, d6=0, d7=0, d8=0, d9=0, d10=0;
        decimal RateFlag = 0, RateBeginStep = 0, RateStepChange = 0, RateMin = 0, RateMax = 0,NoRobot=0;
        try
        {
            d0 = Convert.ToDecimal(Utils.GetRequest("D0", "post", 2, "", "D0设置错误"));
            d1 = Convert.ToDecimal(Utils.GetRequest("D1", "post", 2, "", "D1设置错误"));
            d2 = Convert.ToDecimal(Utils.GetRequest("D2", "post", 2, "", "D2设置错误"));
            d3 = Convert.ToDecimal(Utils.GetRequest("D3", "post", 2, "", "D3设置错误"));
            d4 = Convert.ToDecimal(Utils.GetRequest("D4", "post", 2, "", "D4设置错误"));
            d5 = Convert.ToDecimal(Utils.GetRequest("D5", "post", 2, "", "D5设置错误"));
            d6 = Convert.ToDecimal(Utils.GetRequest("D6", "post", 2, "", "D6设置错误"));
            d7 = Convert.ToDecimal(Utils.GetRequest("D7", "post", 2, "", "D7设置错误"));
            d8 = Convert.ToDecimal(Utils.GetRequest("D8", "post", 2, "", "D8设置错误"));
            d9 = Convert.ToDecimal(Utils.GetRequest("D9", "post", 2, "", "D9设置错误"));
            d10 = Convert.ToDecimal(Utils.GetRequest("D10", "post", 2, "", "D10设置错误"));
            RateFlag = Convert.ToDecimal(Utils.GetRequest("RateFlag", "post", 2, "", "浮动赔率开关设置错误"));
            RateBeginStep = Convert.ToDecimal(Utils.GetRequest("RateBeginStep", "post", 2, "", "浮动赔率起始期数设置错误"));
            RateStepChange = Convert.ToDecimal(Utils.GetRequest("RateStepChange", "post", 2, "", "浮动赔率变化幅度设置错误"));
            RateMin = Convert.ToDecimal(Utils.GetRequest("RateMin", "post", 2, "", "浮动赔率最小值设置错误"));
            RateMax = Convert.ToDecimal(Utils.GetRequest("RateMax", "post", 2, "", "浮动赔率最大值设置错误"));
            NoRobot = Convert.ToDecimal(Utils.GetRequest("NoRobot", "post", 2, @"^[0-1]$", "禁止机器人下注开关选择出错"));
        }
        catch
        {
            Utils.Success("PK拾游戏设置", "赔率格式不正确！", Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "1");
        }
        int paylimit = Convert.ToInt32(Utils.GetRequest("PayLimit", "post", 2, @"^[0-9]\d*$", "限额设置错误"));
        string remark = (Utils.GetRequest("Remark", "post", 1, "", ""));
        int paylimitUser = Convert.ToInt32(Utils.GetRequest("PayLimitUser", "post", 2, @"^[0-9]\d*$", "每用户限额设置错误"));

        //
        PK10_BuyType otype = new PK10_BuyType();
        otype.ID = id;
        otype.MultiSelect = MultiSelect;
        otype.d0 = d0;
        otype.d1 = d1;
        otype.d2 = d2;
        otype.d3 = d3;
        otype.d4 = d4;
        otype.d5 = d5;
        otype.d6 = d6;
        otype.d7 = d7;
        otype.d8 = d8;
        otype.d9 = d9;
        otype.d10 = d10;
        otype.PayLimit = paylimit;
        otype.Remark = remark;
        otype.RateFlag = RateFlag;
        otype.RateBeginStep = RateFlag;
        otype.RateStepChange = RateStepChange;
        otype.RateMin = RateMin;
        otype.RateMax = RateMax;
        otype.NoRobot = NoRobot;
        otype.PayLimitUser = paylimitUser;
        //

        string cFlag = new PK10().SaveBuyType(otype);
        if (string.IsNullOrEmpty(cFlag))
        {
            Utils.Success("PK拾游戏设置", "设置成功，正在返回..", Utils.getUrl(myFileName+ "?showtype=1&amp;buytype="+id.ToString().Trim()), "1");
        }
        else
        {
            Utils.Success("PK拾游戏设置", "设置失败："+cFlag, Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "3");
        }
        //
    }
    private void SaveWinPrice2()
    {
        string backurl = Utils.GetRequest("backrul", "post", 1, "", "");
        int id = Convert.ToInt32(Utils.GetRequest("buytype", "post", 2, @"^[0-9]\d*$", "ID设置读取错误"));
        int MultiSelect = Convert.ToInt32(Utils.GetRequest("MultiSelect", "post", 2, @"^[0-1]$", "复式或者单式选择错误"));
        decimal d0 = 0, d1 = 0, d2 = 0, d3 = 0, d4 = 0, d5 = 0, d6 = 0, d7 = 0, d8 = 0, d9 = 0, d10 = 0;
        decimal RateFlag = 0, RateBeginStep = 0, RateStepChange = 0, RateMin = 0, RateMax = 0, NoRobot = 0;
        try
        {
            d0 = Convert.ToDecimal(Utils.GetRequest("D0", "post", 2, "", "D0设置错误"));
            d1 = Convert.ToDecimal(Utils.GetRequest("D1", "post", 2, "", "D1设置错误"));
            d2 = Convert.ToDecimal(Utils.GetRequest("D2", "post", 2, "", "D2设置错误"));
            d3 = Convert.ToDecimal(Utils.GetRequest("D3", "post", 2, "", "D3设置错误"));
            d4 = Convert.ToDecimal(Utils.GetRequest("D4", "post", 2, "", "D4设置错误"));
            d5 = Convert.ToDecimal(Utils.GetRequest("D5", "post", 2, "", "D5设置错误"));
            d6 = Convert.ToDecimal(Utils.GetRequest("D6", "post", 2, "", "D6设置错误"));
            d7 = Convert.ToDecimal(Utils.GetRequest("D7", "post", 2, "", "D7设置错误"));
            d8 = Convert.ToDecimal(Utils.GetRequest("D8", "post", 2, "", "D8设置错误"));
            d9 = Convert.ToDecimal(Utils.GetRequest("D9", "post", 2, "", "D9设置错误"));
            d10 = Convert.ToDecimal(Utils.GetRequest("D10", "post", 2, "", "D10设置错误"));
            RateFlag = Convert.ToDecimal(Utils.GetRequest("RateFlag", "post", 2, "", "浮动赔率开关设置错误"));
            RateBeginStep = Convert.ToDecimal(Utils.GetRequest("RateBeginStep", "post", 2, "", "浮动赔率起始期数设置错误"));
            RateStepChange = Convert.ToDecimal(Utils.GetRequest("RateStepChange", "post", 2, "", "浮动赔率变化幅度设置错误"));
            RateMin = Convert.ToDecimal(Utils.GetRequest("RateMin", "post", 2, "", "浮动赔率最小值设置错误"));
            RateMax = Convert.ToDecimal(Utils.GetRequest("RateMax", "post", 2, "", "浮动赔率最大值设置错误"));
            NoRobot = Convert.ToDecimal(Utils.GetRequest("NoRobot", "post", 2, @"^[0-1]$", "禁止机器人下注开关选择出错"));
        }
        catch
        {
            Utils.Success("PK拾游戏设置", "赔率格式不正确！", Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "1");
        }
        int paylimit = Convert.ToInt32(Utils.GetRequest("PayLimit", "post", 2, @"^[0-9]\d*$", "限额设置错误"));
        string remark = (Utils.GetRequest("Remark", "post", 1, "", ""));
        int paylimitUser = Convert.ToInt32(Utils.GetRequest("PayLimitUser", "post", 2, @"^[0-9]\d*$", "每用户限额设置错误"));


        //
        PK10_BuyType otype = new PK10_BuyType();
        otype.ID = id;
        otype.MultiSelect = MultiSelect;
        otype.d0 = d0;
        otype.d1 = d1;
        otype.d2 = d2;
        otype.d3 = d3;
        otype.d4 = d4;
        otype.d5 = d5;
        otype.d6 = d6;
        otype.d7 = d7;
        otype.d8 = d8;
        otype.d9 = d9;
        otype.d10 = d10;
        otype.PayLimit = paylimit;
        otype.Remark = remark;
        otype.RateFlag = RateFlag;
        otype.RateBeginStep = RateBeginStep;
        otype.RateStepChange = RateStepChange;
        otype.RateMin = RateMin;
        otype.RateMax = RateMax;
        otype.NoRobot = NoRobot;
        otype.PayLimitUser = paylimitUser;

        //

        string cFlag = new PK10().SaveBuyType2(otype);
        if (string.IsNullOrEmpty(cFlag))
        {
            Utils.Success("PK拾游戏设置", "设置成功，正在返回..", Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "1");
        }
        else
        {
            Utils.Success("PK拾游戏设置", "设置失败：" + cFlag, Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "3");
        }
        //
    }
    private void SaveWinPrice3()
    {
        string backurl = Utils.GetRequest("backrul", "get", 1, "", "");
        int id = Convert.ToInt32(Utils.GetRequest("buytype", "get", 2, @"^[0-9]\d*$", "ID设置读取错误"));
        string cFlag = new PK10().SaveBuyTypes2(id);
        if (string.IsNullOrEmpty(cFlag))
        {
            Utils.Success("PK拾游戏设置", "设置成功，正在返回..", Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "1");
        }
        else
        {
            Utils.Success("PK拾游戏设置", "设置失败：" + cFlag, Utils.getUrl(myFileName + "?showtype=1&amp;buytype=" + id.ToString().Trim()), "3");
        }
        //
    }
    private void ShowRatePage1(PK10_BuyType otype)
    {
        #region 设置标题提示字符串
        string strText = "/,/,/,";
        strText += "玩法名称:" + "/,";
        strText += "可否复式下注:" + "/,";
        if (otype.NumsCount > 1)
            strText += "匹配0个数字的赔率:" + "/,";
        else
            strText += "/,";
        for(int i=1;i<=10;i++)
        {
            if(i<=otype.NumsCount)
                strText += "匹配"+i.ToString().Trim()+"个数字的赔率:" + "/,";
            else
                strText += "" + "/,";
        }
        strText += "每期下注限额(0表示不限):" + "/,";
        strText += "备注:" + "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "禁止机器人下注：/,";
        strText += "每用户下注限制(0表示不限)：/,";
        #endregion
        #region 设置控件名称
        string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
        #endregion
        #region 设置控件类型
        string strType = "hidden,hidden,hidden,text,select,";
        if (otype.NumsCount > 1)
            strType += "text,";
        else
            strType += "hidden,";
        for (int i = 1; i <= 10; i++)
        {
            if (i <= otype.NumsCount)
                strType += "text,";
            else
                strType += "hidden,";
        }
        strType +="num,text";
        strType += ",hidden,hidden,hidden,hidden,hidden,select,num";
        #endregion
        #region 设置控件值
        string strValu = "saverate'";
        strValu += Utils.getPage(0).Trim() + "'";
        strValu += otype.ID.ToString().Trim() + "'";
        strValu += otype.Name.Trim() + "'";
        strValu += otype.MultiSelect.ToString().Trim() + "'";
        strValu += otype.d0.ToString().Trim() + "'";
        strValu += otype.d1.ToString().Trim() + "'";
        strValu += otype.d2.ToString().Trim() + "'";
        strValu += otype.d3.ToString().Trim() + "'";
        strValu += otype.d4.ToString().Trim() + "'";
        strValu += otype.d5.ToString().Trim() + "'";
        strValu += otype.d6.ToString().Trim() + "'";
        strValu += otype.d7.ToString().Trim() + "'";
        strValu += otype.d8.ToString().Trim() + "'";
        strValu += otype.d9.ToString().Trim() + "'";
        strValu += otype.d10.ToString().Trim() + "'";
        strValu += otype.PayLimit.ToString().Trim() + "'";
        strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim()+"'";
        strValu += otype.RateFlag.ToString().Trim() + "'";
        strValu += otype.RateBeginStep.ToString().Trim() + "'";
        strValu += otype.RateStepChange.ToString().Trim() + "'";
        strValu += otype.RateMin.ToString().Trim() + "'";
        strValu += otype.RateMax.ToString().Trim() + "'";
        strValu += otype.NoRobot.ToString().Trim() + "'";
        strValu += otype.PayLimitUser.ToString().Trim() + "'";
        #endregion
        #region 设置控件
        string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
        strEmpt += ",true,true,true,true,true"+",0|否|1|是"+",true";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }
    private void ShowRatePage2(PK10_BuyType otype)
    {
        #region 设置标题提示字符串
        string strText = "/,/,/,";
        strText += "玩法名称:" + "/,";
        strText +=  "/,"; //MultiSelect
        strText +=  "/,"; //d0
        strText += "小的固定赔率:" + "/,"; //d1
        strText += "大的固定赔率:" + "/,"; //d2
        strText +=  "/,";//d3
        strText += "/,";//d4
        strText += "(小)已连开:/,";//d5
        strText += "(大)已连开:/,";//d6
        strText += "/,";//d7
        strText += "(小)浮动赔率:/,";//d8
        strText += "(大)浮动赔率:/,";//d9
        strText += "/,";//d10
        strText += "浮动限额(0表示不限):" + "/,";
        strText += "备注:" + "/,";
        strText += "浮动赔率标志:" + "/,";
        strText += "X期连开才开起浮动:" + "/,";
        strText += "浮动变化幅度:" + "/,";
        strText += "最小浮动赔率:" + "/,";
        strText += "最大浮动赔率:"+"/,";
        strText += "禁止机器人下注：/,";
        strText += "每用户下注限制(0表示不限)：/,";
        #endregion
        #region 设置控件名称
        string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
        #endregion
        #region 设置控件类型
        string strType = "hidden,hidden,hidden,text,hidden,hidden,text,text,";
        strType += "hidden,hidden,text,text,hidden,text,text,hidden,";
        strType += "num,text";
        strType += ",select,text,text,text,text,select,num";
        #endregion
        #region 设置控件值
        string strValu = "saverate2'";
        strValu += Utils.getPage(0).Trim() + "'";
        strValu += otype.ID.ToString().Trim() + "'";
        strValu += otype.Name.Trim() + "'";
        strValu += otype.MultiSelect.ToString().Trim() + "'";
        strValu += otype.d0.ToString().Trim() + "'";
        strValu += otype.d1.ToString().Trim() + "'";
        strValu += otype.d2.ToString().Trim() + "'";
        strValu += otype.d3.ToString().Trim() + "'";
        strValu += otype.d4.ToString().Trim() + "'";
        strValu += otype.d5.ToString().Trim() + "'";
        strValu += otype.d6.ToString().Trim() + "'";
        strValu += otype.d7.ToString().Trim() + "'";
        strValu += otype.d8.ToString().Trim() + "'";
        strValu += otype.d9.ToString().Trim() + "'";
        strValu += otype.d10.ToString().Trim() + "'";
        strValu += otype.PayLimit.ToString().Trim() + "'";
        strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim()+"'";
        strValu += otype.RateFlag.ToString().Trim() + "'";
        strValu += otype.RateBeginStep.ToString().Trim() + "'";
        strValu += otype.RateStepChange.ToString().Trim() + "'";
        strValu += otype.RateMin.ToString().Trim() + "'";
        strValu += otype.RateMax.ToString().Trim()+"'";
        strValu += otype.NoRobot.ToString().Trim() + "'";
        strValu += otype.PayLimitUser.ToString().Trim() + "'";
        #endregion
        #region 设置控件
        string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
        strEmpt += "," + "0|关闭|1|开启" + ",false,false,false,false"+ ",0|否|1|是" + ",true";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("将当前固定赔率、浮动限额、浮动赔率开关及相关的设置批量应用到其他号码位的配置中:");
        //builder.Append("<a href =\"" + Utils.getUrl(myFileName + "showtype=1&amp;buyType=" + otype.ID.ToString().Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=saverate3&amp;buyType="+otype.ID.ToString().Trim()+ "&amp;backrul=" + Utils.getPage(0).Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void ShowRatePage3(PK10_BuyType otype)
    {
        #region 设置标题提示字符串
        string strText = "/,/,/,";
        strText += "玩法名称:" + "/,";
        strText += "/,"; //MultiSelect
        strText += "/,"; //d0
        strText += "单的固定赔率:" + "/,"; //d1
        strText += "双的固定赔率:" + "/,"; //d2
        strText += "/,";//d3
        strText += "/,";//d4
        strText += "(单)已连开:/,";//d5
        strText += "(双)已连开:/,";//d6
        strText += "/,";//d7
        strText += "(单)浮动赔率:/,";//d8
        strText += "(双)浮动赔率:/,";//d9
        strText += "/,";//d10
        strText += "浮动限额(0表示不限):" + "/,";
        strText += "备注:" + "/,";
        strText += "浮动赔率标志:" + "/,";
        strText += "X期连开才开起浮动:" + "/,";
        strText += "浮动变化幅度:" + "/,";
        strText += "最小浮动赔率:" + "/,";
        strText += "最大浮动赔率:" + "/,";
        strText += "禁止机器人下注：/,";
        strText += "每用户下注限制(0表示不限)：/,";
        #endregion
        #region 设置控件名称
        string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
        #endregion
        #region 设置控件类型
        string strType = "hidden,hidden,hidden,text,hidden,hidden,text,text,";
        strType += "hidden,hidden,text,text,hidden,text,text,hidden,";
        strType += "num,text";
        strType += ",select,text,text,text,text,select,num";
        #endregion
        #region 设置控件值
        string strValu = "saverate2'";
        strValu += Utils.getPage(0).Trim() + "'";
        strValu += otype.ID.ToString().Trim() + "'";
        strValu += otype.Name.Trim() + "'";
        strValu += otype.MultiSelect.ToString().Trim() + "'";
        strValu += otype.d0.ToString().Trim() + "'";
        strValu += otype.d1.ToString().Trim() + "'";
        strValu += otype.d2.ToString().Trim() + "'";
        strValu += otype.d3.ToString().Trim() + "'";
        strValu += otype.d4.ToString().Trim() + "'";
        strValu += otype.d5.ToString().Trim() + "'";
        strValu += otype.d6.ToString().Trim() + "'";
        strValu += otype.d7.ToString().Trim() + "'";
        strValu += otype.d8.ToString().Trim() + "'";
        strValu += otype.d9.ToString().Trim() + "'";
        strValu += otype.d10.ToString().Trim() + "'";
        strValu += otype.PayLimit.ToString().Trim() + "'";
        strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim()+"'";
        strValu += otype.RateFlag.ToString().Trim() + "'";
        strValu += otype.RateBeginStep.ToString().Trim() + "'";
        strValu += otype.RateStepChange.ToString().Trim() + "'";
        strValu += otype.RateMin.ToString().Trim() + "'";
        strValu += otype.RateMax.ToString().Trim() + "'";
        strValu += otype.NoRobot.ToString().Trim() + "'";
        strValu += otype.PayLimitUser.ToString().Trim() + "'";
        #endregion
        #region 设置控件
        string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
        strEmpt += "," + "0|关闭|1|开启" + ",false,false,false,false"+ ",0|否|1|是" + ",true";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("将当前固定赔率、浮动限额、浮动赔率开关及相关的设置批量应用到其他号码位的配置中:");
        //builder.Append("<a href =\"" + Utils.getUrl(myFileName + "showtype=1&amp;buyType=" + otype.ID.ToString().Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=saverate3&amp;buyType=" + otype.ID.ToString().Trim() + "&amp;backrul=" + Utils.getPage(0).Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void ShowRatePage4(PK10_BuyType otype)
    {
        #region 设置标题提示字符串
        string strText = "/,/,/,";
        strText += "玩法名称:" + "/,";
        strText += "/,"; //MultiSelect
        strText += "/,"; //d0
        strText += "虎的固定赔率:" + "/,"; //d1
        strText += "龙的固定赔率:" + "/,"; //d2
        strText += "/,";//d3
        strText += "/,";//d4
        strText += "(虎)已连开:/,";//d5
        strText += "(龙)已连开:/,";//d6
        strText += "/,";//d7
        strText += "(虎)浮动赔率:/,";//d8
        strText += "(龙)浮动赔率:/,";//d9
        strText += "/,";//d10
        strText += "浮动限额(0表示不限):" + "/,";
        strText += "备注:" + "/,";
        strText += "浮动赔率标志:" + "/,";
        strText += "X期连开才开起浮动:" + "/,";
        strText += "浮动变化幅度:" + "/,";
        strText += "最小浮动赔率:" + "/,";
        strText += "最大浮动赔率:" + "/,";
        strText += "禁止机器人下注：/,";
        strText += "每用户下注限制(0表示不限)：/,";
        #endregion
        #region 设置控件名称
        string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
        #endregion
        #region 设置控件类型
        string strType = "hidden,hidden,hidden,text,hidden,hidden,text,text,";
        strType += "hidden,hidden,text,text,hidden,text,text,hidden,";
        strType += "num,text";
        strType += ",select,text,text,text,text,select,num";
        #endregion
        #region 设置控件值
        string strValu = "saverate2'";
        strValu += Utils.getPage(0).Trim() + "'";
        strValu += otype.ID.ToString().Trim() + "'";
        strValu += otype.Name.Trim() + "'";
        strValu += otype.MultiSelect.ToString().Trim() + "'";
        strValu += otype.d0.ToString().Trim() + "'";
        strValu += otype.d1.ToString().Trim() + "'";
        strValu += otype.d2.ToString().Trim() + "'";
        strValu += otype.d3.ToString().Trim() + "'";
        strValu += otype.d4.ToString().Trim() + "'";
        strValu += otype.d5.ToString().Trim() + "'";
        strValu += otype.d6.ToString().Trim() + "'";
        strValu += otype.d7.ToString().Trim() + "'";
        strValu += otype.d8.ToString().Trim() + "'";
        strValu += otype.d9.ToString().Trim() + "'";
        strValu += otype.d10.ToString().Trim() + "'";
        strValu += otype.PayLimit.ToString().Trim() + "'";
        strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim() + "'";
        strValu += otype.RateFlag.ToString().Trim() + "'";
        strValu += otype.RateBeginStep.ToString().Trim() + "'";
        strValu += otype.RateStepChange.ToString().Trim() + "'";
        strValu += otype.RateMin.ToString().Trim() + "'";
        strValu += otype.RateMax.ToString().Trim() + "'";
        strValu += otype.NoRobot.ToString().Trim() + "'";
        strValu += otype.PayLimitUser.ToString().Trim() + "'";
        #endregion
        #region 设置控件
        string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
        strEmpt += "," + "0|关闭|1|开启" + ",false,false,false,false"+ ",0|否|1|是" + ",true";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("将当前固定赔率、浮动限额、浮动赔率开关及相关的设置批量应用到其他号码位的配置中:");
        //builder.Append("<a href =\"" + Utils.getUrl(myFileName + "showtype=1&amp;buyType=" + otype.ID.ToString().Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=saverate3&amp;buyType=" + otype.ID.ToString().Trim() + "&amp;backrul=" + Utils.getPage(0).Trim()) + "\"><font color=\"red\">" + "应用" + "</font></a> ");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void ShowRatePage5(PK10_BuyType otype)
    {
        #region 设置标题提示字符串
        string strText = "/,/,/,";
        strText += "玩法名称:" + "/,";
        strText += "/,"; //MultiSelect
        strText += "/,"; //d0
        strText += "中奖赔率:" + "/,"; //d1
        strText +=  "/,"; //d2
        strText += "/,";//d3
        strText += "/,";//d4
        strText += "/,";//d5
        strText += "/,";//d6
        strText += "/,";//d7
        strText += "/,";//d8
        strText += "/,";//d9
        strText += "/,";//d10
        strText += "每期下注限额(0表示不限):" + "/,";
        strText += "备注:" + "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "/,";
        strText += "禁止机器人下注：/,";
        strText += "每用户下注限制(0表示不限)：/,";
        #endregion
        #region 设置控件名称
        string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
        #endregion
        #region 设置控件类型
        string strType = "hidden,hidden,hidden,text,hidden,hidden,text,hidden,";
        strType += "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden,";
        strType += "num,text";
        strType += ",hidden,hidden,hidden,hidden,hidden,select,num";
        #endregion
        #region 设置控件值
        string strValu = "saverate'";
        strValu += Utils.getPage(0).Trim() + "'";
        strValu += otype.ID.ToString().Trim() + "'";
        strValu += otype.Name.Trim() + "'";
        strValu += otype.MultiSelect.ToString().Trim() + "'";
        strValu += otype.d0.ToString().Trim() + "'";
        strValu += otype.d1.ToString().Trim() + "'";
        strValu += otype.d2.ToString().Trim() + "'";
        strValu += otype.d3.ToString().Trim() + "'";
        strValu += otype.d4.ToString().Trim() + "'";
        strValu += otype.d5.ToString().Trim() + "'";
        strValu += otype.d6.ToString().Trim() + "'";
        strValu += otype.d7.ToString().Trim() + "'";
        strValu += otype.d8.ToString().Trim() + "'";
        strValu += otype.d9.ToString().Trim() + "'";
        strValu += otype.d10.ToString().Trim() + "'";
        strValu += otype.PayLimit.ToString().Trim() + "'";
        strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim() + "'";
        strValu += otype.RateFlag.ToString().Trim() + "'";
        strValu += otype.RateBeginStep.ToString().Trim() + "'";
        strValu += otype.RateStepChange.ToString().Trim() + "'";
        strValu += otype.RateMin.ToString().Trim() + "'";
        strValu += otype.RateMax.ToString().Trim() + "'";
        strValu += otype.NoRobot.ToString().Trim() + "'";
        strValu += otype.PayLimitUser.ToString().Trim() + "'";
        #endregion
        #region 设置控件
        string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
        strEmpt += ",true,true,true,true,true"+ ",0|否|1|是" + ",true";
        #endregion
        string strIdea = "/";
        string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
    }
    private void ShowRatePage6(PK10_BuyType otype)
    {
        switch(otype.Type)
        {
            case 2: //大小
                #region 大小(11为和，和返回本金，相当与赔率=1)
                #region 设置标题提示字符串
                string strText = "/,/,/,";
                strText += "玩法名称:" + "/,";
                strText += "/,"; //MultiSelect
                strText += "/,"; //d0
                strText += "小的固定赔率:" + "/,"; //d1
                strText += "大的固定赔率:" + "/,"; //d2
                strText += "和的赔率:/,";//d3
                strText += "/,";//d4
                strText += "(小)已连开:/,";//d5
                strText += "(大)已连开:/,";//d6
                strText += "/,";//d7
                strText += "(小)浮动赔率:/,";//d8
                strText += "(大)浮动赔率:/,";//d9
                strText += "/,";//d10
                strText += "浮动限额(0表示不限):" + "/,";
                strText += "备注:" + "/,";
                strText += "浮动赔率标志:" + "/,";
                strText += "X期连开才开起浮动:" + "/,";
                strText += "浮动变化幅度:" + "/,";
                strText += "最小浮动赔率:" + "/,";
                strText += "最大浮动赔率:" + "/,";
                strText += "禁止机器人下注：/,";
                strText += "每用户下注限制(0表示不限)：/,";
                #endregion
                #region 设置控件名称
                string strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
                #endregion
                #region 设置控件类型
                string strType = "hidden,hidden,hidden,text,hidden,hidden,text,text,";
                strType += "text,hidden,text,text,hidden,text,text,hidden,";
                strType += "num,text";
                strType += ",select,text,text,text,text,select,num";
                #endregion
                #region 设置控件值
                string strValu = "saverate2'";
                strValu += Utils.getPage(0).Trim() + "'";
                strValu += otype.ID.ToString().Trim() + "'";
                strValu += otype.Name.Trim() + "'";
                strValu += otype.MultiSelect.ToString().Trim() + "'";
                strValu += otype.d0.ToString().Trim() + "'";
                strValu += otype.d1.ToString().Trim() + "'";
                strValu += otype.d2.ToString().Trim() + "'";
                strValu += otype.d3.ToString().Trim() + "'";
                strValu += otype.d4.ToString().Trim() + "'";
                strValu += otype.d5.ToString().Trim() + "'";
                strValu += otype.d6.ToString().Trim() + "'";
                strValu += otype.d7.ToString().Trim() + "'";
                strValu += otype.d8.ToString().Trim() + "'";
                strValu += otype.d9.ToString().Trim() + "'";
                strValu += otype.d10.ToString().Trim() + "'";
                strValu += otype.PayLimit.ToString().Trim() + "'";
                strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim() + "'";
                strValu += otype.RateFlag.ToString().Trim() + "'";
                strValu += otype.RateBeginStep.ToString().Trim() + "'";
                strValu += otype.RateStepChange.ToString().Trim() + "'";
                strValu += otype.RateMin.ToString().Trim() + "'";
                strValu += otype.RateMax.ToString().Trim() + "'";
                strValu += otype.NoRobot.ToString().Trim() + "'";
                strValu += otype.PayLimitUser.ToString().Trim() + "'";
                #endregion
                #region 设置控件
                string strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
                strEmpt += "," + "0|关闭|1|开启" + ",false,false,false,false"+ ",0|否|1|是" + ",true";
                #endregion
                string strIdea = "/";
                string strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                #endregion
                break;
            case 3: //单双
                #region 单双(11为和，和返回本金，相当与赔率=1)
                #region 设置标题提示字符串
                strText = "/,/,/,";
                strText += "玩法名称:" + "/,";
                strText += "/,"; //MultiSelect
                strText += "/,"; //d0
                strText += "单的固定赔率:" + "/,"; //d1
                strText += "双的固定赔率:" + "/,"; //d2
                strText += "和的赔率:/,";//d3
                strText += "/,";//d4
                strText += "(单)已连开:/,";//d5
                strText += "(双)已连开:/,";//d6
                strText += "/,";//d7
                strText += "(单)浮动赔率:/,";//d8
                strText += "(双)浮动赔率:/,";//d9
                strText += "/,";//d10
                strText += "浮动限额(0表示不限):" + "/,";
                strText += "备注:" + "/,";
                strText += "浮动赔率标志:" + "/,";
                strText += "X期连开才开起浮动:" + "/,";
                strText += "浮动变化幅度:" + "/,";
                strText += "最小浮动赔率:" + "/,";
                strText += "最大浮动赔率:" + "/,";
                strText += "禁止机器人下注：/,";
                strText += "每用户下注限制(0表示不限)：/,";
                #endregion
                #region 设置控件名称
                strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
                #endregion
                #region 设置控件类型
                strType = "hidden,hidden,hidden,text,hidden,hidden,text,text,";
                strType += "text,hidden,text,text,hidden,text,text,hidden,";
                strType += "num,text";
                strType += ",select,text,text,text,text,select,num";
                #endregion
                #region 设置控件值
                strValu = "saverate2'";
                strValu += Utils.getPage(0).Trim() + "'";
                strValu += otype.ID.ToString().Trim() + "'";
                strValu += otype.Name.Trim() + "'";
                strValu += otype.MultiSelect.ToString().Trim() + "'";
                strValu += otype.d0.ToString().Trim() + "'";
                strValu += otype.d1.ToString().Trim() + "'";
                strValu += otype.d2.ToString().Trim() + "'";
                strValu += otype.d3.ToString().Trim() + "'";
                strValu += otype.d4.ToString().Trim() + "'";
                strValu += otype.d5.ToString().Trim() + "'";
                strValu += otype.d6.ToString().Trim() + "'";
                strValu += otype.d7.ToString().Trim() + "'";
                strValu += otype.d8.ToString().Trim() + "'";
                strValu += otype.d9.ToString().Trim() + "'";
                strValu += otype.d10.ToString().Trim() + "'";
                strValu += otype.PayLimit.ToString().Trim() + "'";
                strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim() + "'";
                strValu += otype.RateFlag.ToString().Trim() + "'";
                strValu += otype.RateBeginStep.ToString().Trim() + "'";
                strValu += otype.RateStepChange.ToString().Trim() + "'";
                strValu += otype.RateMin.ToString().Trim() + "'";
                strValu += otype.RateMax.ToString().Trim() + "'";
                strValu += otype.NoRobot.ToString().Trim() + "'";
                strValu += otype.PayLimitUser.ToString().Trim() + "'";
                #endregion
                #region 设置控件
                strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
                strEmpt += "," + "0|关闭|1|开启" + ",false,false,false,false"+ ",0|否|1|是" + ",true";
                #endregion
                strIdea = "/";
                strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                #endregion
                break;
            default: //和值
                #region 分开不同种类的和值，不同的赔率
                #region 设置标题提示字符串
                strText = "/,/,/,";
                strText += "玩法名称:" + "/,";
                strText += "可否复式下注:" + "/,";
                strText += "/,"; //d0
                strText += "3、4、18、19赔率:/,"; //d1
                strText += "5、6、16、17赔率:/,"; //d2
                strText += "7、8、14、15赔率:/,"; //d3
                strText += "9、10、12、13赔率:/,"; //44
                strText += "11赔率:/,"; //d5
                strText += "/,"; //d6
                strText += "/,"; //d7
                strText += "/,"; //d8
                strText += "/,"; //d9
                strText += "/,"; //d10
                strText += "每期下注限额(0表示不限):" + "/,";
                strText += "备注:" + "/,";
                strText += "/,";
                strText += "/,";
                strText += "/,";
                strText += "/,";
                strText += "/,";
                strText += "禁止机器人下注：/,";
                strText += "每用户下注限制(0表示不限)：/,";
                #endregion
                #region 设置控件名称
                strName = "act,backrul,buytype,Name,MultiSelect,D0,D1,D2,D3,D4,D5,D6,D7,D8,D9,D10,PayLimit,Remark,RateFlag,RateBeginStep,RateStepChange,RateMin,RateMax,NoRobot,PayLimitUser";
                #endregion
                #region 设置控件类型
                strType = "hidden,hidden,hidden,text,select,hidden,";
                strType += "text,text,text,text,text,";
                strType += "hidden,hidden,hidden,hidden,hidden,";
                strType += "num,text";
                strType += ",hidden,hidden,hidden,hidden,hidden,select,num";
                #endregion
                #region 设置控件值
                strValu = "saverate'";
                strValu += Utils.getPage(0).Trim() + "'";
                strValu += otype.ID.ToString().Trim() + "'";
                strValu += otype.Name.Trim() + "'";
                strValu += otype.MultiSelect.ToString().Trim() + "'";
                strValu += otype.d0.ToString().Trim() + "'";
                strValu += otype.d1.ToString().Trim() + "'";
                strValu += otype.d2.ToString().Trim() + "'";
                strValu += otype.d3.ToString().Trim() + "'";
                strValu += otype.d4.ToString().Trim() + "'";
                strValu += otype.d5.ToString().Trim() + "'";
                strValu += otype.d6.ToString().Trim() + "'";
                strValu += otype.d7.ToString().Trim() + "'";
                strValu += otype.d8.ToString().Trim() + "'";
                strValu += otype.d9.ToString().Trim() + "'";
                strValu += otype.d10.ToString().Trim() + "'";
                strValu += otype.PayLimit.ToString().Trim() + "'";
                strValu += string.IsNullOrEmpty(otype.Remark) ? "'" : otype.Remark.ToString().Trim() + "'";
                strValu += otype.RateFlag.ToString().Trim() + "'";
                strValu += otype.RateBeginStep.ToString().Trim() + "'";
                strValu += otype.RateStepChange.ToString().Trim() + "'";
                strValu += otype.RateMin.ToString().Trim() + "'";
                strValu += otype.RateMax.ToString().Trim() + "'";
                strValu += otype.NoRobot.ToString().Trim() + "'";
                strValu += otype.PayLimitUser.ToString().Trim() + "'";
                #endregion
                #region 设置控件
                strEmpt = "false,false,false,false," + "0|否|1|可" + ",false,false,false,false,false,false,false,false,false,false,false,false,true";
                strEmpt += ",true,true,true,true,true"+ ",0|否|1|是" + ",true";
                #endregion
                strIdea = "/";
                strOthe = "确定修改|reset," + myFileName + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                #endregion
                break;
        }
    }
}