using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using BCW.PK10.Model;
using System.Text.RegularExpressions;
public partial class bbs_game_PK10 : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/PK10.xml";
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected bool isTest = false;//当前是否测试
    protected string GameName = "";
    protected string myFileName = "PK10.aspx";
    protected int myCurrentSaleID = 0;
    protected int myUserID = 0;
    protected PK10 _logic;
    protected bool isInit = true;//今天是否初始化数据
    protected bool isSale = false;//销售状态
    protected int myPageSize = 10; //显示数据时的默认行数
    protected int defaultTestMoney = 100000; //默认测试用的PK币
    protected int showSaleDataInitRecordCount = 6; //下注页面显示的期号行数
    protected int showSaleDataStep = 3;//下注页面显示的期号每次展开或收藏的行数
    //
    private string GetPage(string newParaName, string newParaValue,bool delFlag)
    {
        string cPage = Request.Url.AbsolutePath;
        bool hasKey = false;
        int count = Request.QueryString.Keys.Count;
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                string cpara = "";
                string key = Request.QueryString.GetKey(i);
                if (!string.IsNullOrEmpty(key))
                {
                    cpara = key + "=";
                    if (key.ToUpper().Trim() == newParaName.ToUpper().Trim())
                    {
                        if (!delFlag)
                        {
                            cpara += newParaValue;
                        }
                        else
                        {
                            cpara = "";
                        }
                        hasKey = true;
                    }
                    else
                    {
                        cpara += Request.QueryString[i];
                    }
                    if (i ==0 )
                        cPage += "?";
                    else
                        cPage += "&amp;";
                    cPage += cpara;
                }
            }
        }
        if (!hasKey && newParaName!="")
        {
            if (count == 0 )
                cPage += "?" + newParaName + "=" + newParaValue;
            else
                cPage +="&amp;" + newParaName + "=" + newParaValue;
        }
        return cPage;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Application.Remove(xmlPath);//清缓存
        GameName = ub.GetSub("GameName", xmlPath);
        Master.Title = GameName;
        _logic = new PK10();
        //
        #region 游戏状态判断
        string GameStatus = ub.GetSub("GameStatus", xmlPath);
        switch (GameStatus)
        {
            case "1": //维护
                Utils.Safe(GameName);
                break;
            case "2": //内测(PK币)
                GameName += "(内测)";
                #region 判断是否有效的内测帐号
                int meid = new BCW.User.Users().GetUsId();
                if (meid == 0)
                    Utils.Login();
                //
                int sbsy = 0;
                string cTestID = ub.GetSub("TestUserID", xmlPath).Trim();
                if (cTestID != "")
                {
                    string[] aIDs = cTestID.Split('#');

                    for (int a = 0; a < aIDs.Length; a++)
                    {
                        int tid = 0;
                        int.TryParse(aIDs[a].Trim(), out tid);
                        if (meid == tid)
                        {
                            sbsy++;
                        }
                    }
                }
                if (sbsy == 0)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
                #endregion
                isTest = true;
                break;
            case "3": //公测(PK币)
                GameName += "(公测)";
                isTest = true;
                break;
            case "4": //内测(酷币)
                GameName += "(内测)";
                #region 判断是否有效的内测帐号
                meid = new BCW.User.Users().GetUsId();
                if (meid == 0)
                    Utils.Login();
                //
                sbsy = 0;
                cTestID = ub.GetSub("TestUserID", xmlPath).Trim();
                if (cTestID != "")
                {
                    string[] aIDs = cTestID.Split('#');

                    for (int a = 0; a < aIDs.Length; a++)
                    {
                        int tid = 0;
                        int.TryParse(aIDs[a].Trim(), out tid);
                        if (meid == tid)
                        {
                            sbsy++;
                        }
                    }
                }
                if (sbsy == 0)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
                #endregion
                isTest = false;
                break;
            default:  //正常 
                isTest = false;
                break;
        }
        #endregion
        //
        #region 加载页面
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "buy":
                BuyPage();
                break;
            case "buydetail":
                ShowBuyDetail();
                break;
            case "buydetail2":
                ShowBuyDetail2();
                break;
            case "case":
                CasePage();
                break;
            case "caseok":
                CaseOKPage();
                break;
            case "casepost":
                CasePostPage();
                break;
            case "list":
                ListPage();
                break;
            case "view":
            case "listview":
                ListViewPage();
                break;
            case "pay":
                PayPage();
                break;
            case "payok":
                PayOKPage();
                break;
            case "report":
                ReportPage();
                break;
            case "rule":
                RulePage();
                break;
            case "top":
                TopPage();
                break;
            default:
                ReloadPage();
                break;
        }
        #endregion
        //
    }
    private void ReloadPage()//首页
    {
        string navText = "";
        ShowGameTop(navText);
        ShowGameNavi(0);
        //
        #region 初始化判断
        PK10_Base _base = _logic.GetSaleBase(); 
        if(_base==null)
        {
            isInit = false;
            Utils.Error("系统尚未初始化...", "");
        }
        else
        {
            if (_base.CurrentSaleDate != DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                isInit = false;
            }
        }
        #endregion
        #region 显示最近开奖情况
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("【最新开奖】");
        builder.Append(Out.Tab("</div>", "<br/>"));
        ShowSaleDatas(3,3, null);
        #endregion
        #region 显示下注导航
        List<PK10_BuyType> plist = _logic.GetBuyTypes(0);
        if (plist != null)
        {
            foreach (PK10_BuyType pitem in plist)
            {
                string ctext = "【" + pitem.Name.Trim() + "】";
                if (pitem.ID == 2)  //为了减少显示的长度，合并大小单双这两种类型（ID=2、3）的下注页面。
                    ctext = "【" + "大小单双玩法" + "】";
                if (pitem.ID != 3)
                {
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    builder.Append(ctext);
                    builder.Append(Out.Tab("</div>", ""));
                    List<PK10_BuyType> slist = _logic.GetBuyTypes(pitem.ID);
                    if (slist != null && slist.Count > 0)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        int line = 0;
                        foreach (PK10_BuyType sitem in slist)
                        {
                            if (line > 0 && line % 5 == 0)
                                builder.Append("<br/>");
                            builder.Append(" <a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+pitem.ID.ToString().Trim()+"&amp;buytype=" + sitem.ID.ToString().Trim()) + "\">" + sitem.Descript.Trim() + "</a> ");
                            line++;
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }
        }
        #endregion
        //
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void RulePage()//规则页
    {
        Master.Title = GameName + ".规则";
        string navText = "&gt;规则";
        ShowGameTop(navText);
        ShowGameNavi(1);
        //
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示规则
        List<string> list;
        try
        {
            list = Common.GetFromTxt(Server.MapPath("PK10Rule.txt"));
            builder.Append(Out.Tab("<div>", ""));
            foreach (string line in list)
            {
                builder.Append(line + "<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        catch (Exception ex)
        {
            builder.Append("<div>" + ex.Message + "</div>");
        }
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void BuyPage() //下注页
    {
        Master.Title = GameName + ".下注";
        string navText = "&gt;下注";
        ShowGameTop(navText);
        #region 初始化判断
        PK10_Base _base = _logic.GetSaleBase();
        if (_base == null)
        {
            isInit = false;
            Utils.Error("系统尚未初始化...", "");
        }
        else
        {
            if (_base.CurrentSaleDate != DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                isInit = false;
            }
        }
        #endregion
        builder.Append(Out.Tab("", "<br/>"));
        ShowUser();
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        if(!isInit)
            builder.Append("<font color=\"red\">该游戏数据服务未启动...</font>"+"<br/>");
        //
        #region 读取下注页面显示期号的设置
        try
        {
            int showSaleDataRecordCount = Convert.ToInt32(ub.GetSub("showSaleDataRecordCount", xmlPath));
            if (showSaleDataRecordCount >0)
                showSaleDataInitRecordCount = showSaleDataRecordCount;
        }
        catch { };
        try
        {
            int showSaleDataRecordStep = Convert.ToInt32(ub.GetSub("showSaleDataStep", xmlPath));
            if (showSaleDataRecordStep > 0)
                showSaleDataStep = showSaleDataRecordStep;
        }
        catch { };
        #endregion
        //
        #region 显示下注页面
        int buyTypeParentID = 0;
            string cBuyType = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "1");
            int.TryParse(cBuyType, out buyTypeParentID);
            switch(buyTypeParentID)
            {
                case 1:
                    BuyPage1();
                    break;
                case 2:
                case 3:
                    BuyPage23();
                    break;
                case 4:
                    BuyPage4();
                    break;
                case 5:
                    BuyPage5();
                    break;
                case 6:
                    BuyPage6();
                    break;
                default:
                    break;
            }
            #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    #region 各种下注类型
    private void BuyPage1() //号码买法
    {
        #region 读取参数
        string cBuyTypeParentID = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("buytype", "get", 1, @"^[1-9]\d*$", "0");
        string cSelects = Utils.GetRequest("Select", "get", 1, "", "");//已经选择的号码,例如（第一位选择02、03，第二位选择03、05，则是Select=1,02|1,03|2,03|2,05）
        string cAutoSelect = Utils.GetRequest("AutoSelect", "get", 1, "", "");
        string cBuyPrice = Utils.GetRequest("price", "get", 1, @"^[1-9]\d*$", "0");
        bool isAutoSelect = false;
        if (cAutoSelect.Trim() == "1")
            isAutoSelect = true;
        //////
        int buyTypeID = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int buyTypeParentID = 1, buyPrice = 0;
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cBuyPrice, out buyPrice);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        if (oBuyType == null || oBuyType.NumsCount <= 0)
            Utils.Error("找不到类型...", "");
        #endregion
        ShowSaleDatas(showSaleDataInitRecordCount, showSaleDataStep, oBuyType);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div>",""));
        builder.Append("【号码玩法】");
        builder.Append(Out.Tab("</div>", "<br />"));
        ////
        #region ----显示下注的类型导航
        builder.Append(Out.Tab("<div>", ""));
        List<PK10_BuyType> oBuyTypes = _logic.GetBuyTypes(oBuyType.ParentID);
        string shownavi = "";
        for (int i = 0; i < oBuyTypes.Count; i++)
        {
            string showtext = oBuyTypes[i].Descript.Trim();
            if (oBuyTypes[i].ID == buyTypeID)
                showtext = "<font color=\"red\">" + showtext + "</font>";
            string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ oBuyTypes[i].ParentID.ToString().Trim() + "&amp;buytype=" + oBuyTypes[i].ID.ToString().Trim()) + "\">" + showtext + "</a>";
            //string str = "<a href=\"" +GetPage("buytype",oBuyTypes[i].ID.ToString().Trim(),false) + "\">" + showtext + "</a>";
            shownavi += str;
            if (i < oBuyTypes.Count-1)
                shownavi += " |";
            if (i == 4 || i == 9)
                shownavi += "<br />";
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        //////
        List<string> aSelects;
        if (!isAutoSelect)
            aSelects = CheckAndGetNewSelect(cSelects, oBuyType);//检测选择的号码是否符合规则，并返回新的选择字符串数组(每行只记录一个第N位某一号码，如1,03）
        else
        {
            aSelects = CreateRandomNums(null, oBuyType.NumsCount, ""); //机选一注
        }
        #region ----显示下注选择区
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("当前选择：["+oBuyType.Name.Trim()+"] ");
        builder.Append("    " + "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID + "&amp;buyType=" + cBuyType + "&amp;AutoSelect=1") + "\"><font color=\"brown\">" + "机选一注" + "</font></a> ");
        //builder.Append("    " + "<a href=\"" + GetPage("AutoSelect","1",true) + "\"><font color=\"brown\">" + "机选一注" + "</font></a> ");
        builder.Append("<br />");
        string buyStr = "", searchStr = "", showNumStr = "", newSelect = "";
        string[] aNums = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
        List<string> anewSelects = new List<string>();
        for (int i = 1; i <= oBuyType.NumsCount; i++)
        {
            buyStr = i.ToString().Trim().PadLeft(2, '_').Replace('_', ' ') + ")";
            for (int j = 0; j < aNums.Length; j++)
            {
                #region 判断当前位数的当前号码，是否已经选择，并设置显示状态
                searchStr = i.ToString().Trim() + "," + aNums[j].Trim();
                showNumStr = "";
                newSelect = "";
                anewSelects = new List<string>(aSelects);
                if (aSelects.Contains(searchStr))
                {
                    anewSelects.Remove(searchStr);
                    showNumStr = "<font color=\"red\">" + aNums[j].Trim() + "</font>";
                }
                else
                {
                    anewSelects.Add(searchStr);
                    showNumStr = aNums[j].Trim();
                }
                #endregion
                #region 重新生成Select字符串
                if (anewSelects.Count > 0)
                {
                    for (int k = 0; k < anewSelects.Count; k++)
                    {
                        if (newSelect == "")
                            newSelect = anewSelects[k].Trim();
                        else
                            newSelect += "|" + anewSelects[k].Trim();
                    }
                }
                #endregion
                buyStr += " <a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID + "&amp;buyType=" + cBuyType + "&amp;Select=" + newSelect) + "\">" + showNumStr + "</a> ";
                //buyStr += " <a href=\"" +GetPage("Select",newSelect,false) + "\">" + showNumStr + "</a> ";
            }
            builder.Append(buyStr);
            builder.Append("<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        if (!isSale)
        {
            builder.Append(Out.Div("div", "<font color=\"red\">目前没有开售的期号，不能下注...</font>"));
        }
        else
        {
            #region 显示下注选择结果
            string buyNums = GenNumsFromSelectStr(aSelects, oBuyType.NumsCount);
            List<string> numsDetail = _logic.GenBuyNums(buyNums, oBuyType.NumsCount);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("您选择了[" + numsDetail.Count.ToString().Trim() + "]注:");
            builder.Append("<br/>");
            if (string.IsNullOrEmpty(buyNums))
                builder.Append("<font color=\"red\">" + "(您还没选择下注的号码)" + "</font>");
            else
            {
                builder.Append("<font color=\"red\">" + buyNums + "</font>");
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\"> 清空</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
            #endregion
            #region ----显示快捷下注金额       
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("选投注额："+"[<a href=\""+ Utils.getUrl("pk10_paysettings.aspx"+"?backurl="+ Utils.PostPage(1)) + "\">设置</a>]"+"<br/>");
            builder.Append(GetPaySettings(myUserID,buyTypeParentID,buyTypeID, cSelects));
            builder.Append(Out.Tab("</div>", ""));
            #endregion
            #region 显示手工下注金额       
            //
            string strText = "/,/,/,或手动输入:/,/,/,/,/,";
            string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
            string strType = "hidden,hidden,hidden,num,hidden,hidden,hidden,hidden";
            string strValu = buyTypeParentID + "'" + buyTypeID + "'" + buyNums + "'" + buyPrice + "'" + myCurrentSaleID + "'pay'"+cSelects+"'"+oBuyType.d0;
            string strEmpt = "false,false,false,false,false,false,false,false";

            string strIdea = "''<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\">清号<／a>'''''|/";
            string strOthe = "确定下注," + myFileName + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        #region 显示赔率/备注
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("(赔率：");
        decimal[] aprices = { oBuyType.d0,oBuyType.d1, oBuyType.d2, oBuyType.d3, oBuyType.d4, oBuyType.d5, oBuyType.d6, oBuyType.d7, oBuyType.d8, oBuyType.d9, oBuyType.d10 };
        if(oBuyType.NumsCount>0 && oBuyType.NumsCount<=10)
        {
            string str = "";
            if (oBuyType.NumsCount == 1)
                str = aprices[1].ToString().Trim() + "倍";
            else
            {
                for (int i = 0; i <= oBuyType.NumsCount; i++)
                {
                    if (aprices[i] != 0)
                    {
                        if (str == "")
                            str += "中" + i.ToString().Trim() + "个号码" + aprices[i].ToString().Trim() + "倍";
                        else
                            str += "," + "中" + i.ToString().Trim() + "个号码" + aprices[i].ToString().Trim() + "倍";
                    }
                }
            }
            builder.Append(str);
        }
        builder.Append(")");
        if (!string.IsNullOrEmpty(oBuyType.Remark))
            builder.Append("<br/>"+oBuyType.Remark.Trim());
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    private void BuyPage23() //大小单双买法
    {
        #region 读取参数
        string cBuyTypeParentID = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("buytype", "get", 1, @"^[1-9]\d*$", "0");
        string cSelects = Utils.GetRequest("Select", "get", 1, "", "0");//已经选择的（0，表示小或者单，1表示大或者双）
        string cBuyPrice = Utils.GetRequest("price", "get", 1, @"^[1-9]\d*$", "0");
        //////
        int buyTypeParentID = 2, buyTypeID = 0, select = 0, buyPrice = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cSelects, out select);
        int.TryParse(cBuyPrice, out buyPrice);
        //因为合并了大小与单双两张大类型的同时显示，需要切换BuyTypeID
        if (buyTypeID.ToString().Substring(0, 1) != buyTypeParentID.ToString().Trim())
            buyTypeID = int.Parse(buyTypeParentID.ToString().Trim() + buyTypeID.ToString().Substring(1, 1));
        //
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        if (oBuyType == null || oBuyType.NumsCount <= 0)
            Utils.Error("找不到类型...", "");
        #endregion
        ShowSaleDatas(showSaleDataInitRecordCount, showSaleDataStep, oBuyType);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【大小单双玩法】");
        builder.Append(Out.Tab("</div>", ""));
        ////
        #region ----显示下注的类型导航
        builder.Append(Out.Tab("<div>", "<br/>"));
        List<PK10_BuyType> oBuyTypes = _logic.GetBuyTypes(buyTypeParentID);
        string shownavi = "";
        for (int i = 0; i < oBuyTypes.Count; i++)
        {
            string showtext = oBuyTypes[i].Descript.Trim();
            if (oBuyTypes[i].ID == buyTypeID)
                showtext = "<font color=\"red\">" + showtext + "</font>";
            string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID.ToString().Trim() + "&amp;buytype=" + oBuyTypes[i].ID.ToString().Trim()+ "&amp;select=" + select.ToString().Trim()+ "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + showtext + "</a>";
            shownavi += str;
            if (i < oBuyTypes.Count - 1)
                shownavi += " |";
            if (i == 4 )
                shownavi += "<br />";
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        //////
        #region 显示选择结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("当前选择：[" + oBuyType.Descript.Trim()+"."+ GetBuyType23Name(buyTypeParentID,select) + "] ");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示大小单双导航
        string red1 = "<font color=\"red\">";
        string red2 = "</font>";
        string navi1 = (buyTypeParentID == 2 && select == 1) ? (red1 + "买大" + red2) : "买大";
        string navi2 = (buyTypeParentID == 2 && select == 0) ? (red1 + "买小" + red2) : "买小";
        string navi3 = (buyTypeParentID == 3 && select == 0) ? (red1 + "买单" + red2) : "买单";
        string navi4 = (buyTypeParentID == 3 && select == 1) ? (red1 + "买双" + red2) : "买双";
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=2&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=1" + "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + navi1 + "</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=2&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=0" + "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + navi2 + "</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=3&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=0" + "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + navi3 + "</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=3&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=1" + "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + navi4 + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region ----显示快捷下注金额       
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
        builder.Append(GetPaySettings(myUserID,buyTypeParentID,buyTypeID,select.ToString()));
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=200000" + "\">" + "20万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=400000" + "\">" + "40万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=600000" + "\">" + "60万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=800000" + "\">" + "80万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=1000000" + "\">" + "100万" + "</a> ");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        decimal rate = (select == 0) ? oBuyType.d8 : oBuyType.d9;
        #region 显示手工下注金额
        string strText = "/,/,/,或手动输入" + _logic.GetGoldName(isTest) + ":/,/,/,/,";
        string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
        string strType = "hidden,hidden,hidden,num,hidden,hidden,hidden,hidden";
        string strValu = buyTypeParentID + "'" + buyTypeID + "'" + select + "'"+ buyPrice + "'"+myCurrentSaleID+"'pay'"+cSelects+"'"+rate;
        string strEmpt = "false,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定下注," + myFileName + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示赔率/备注
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("(赔率：" + rate + ")");
        if (!string.IsNullOrEmpty(oBuyType.Remark))
            builder.Append("<br/>" + oBuyType.Remark.Trim());
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    private void BuyPage4() //龙虎买法
    {
        #region 读取参数
        string cBuyTypeParentID = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("buytype", "get", 1, @"^[1-9]\d*$", "0");
        string cSelects = Utils.GetRequest("Select", "get", 1, "", "0");//已经选择的（0，表示小或者单，1表示大或者双）
        string cBuyPrice = Utils.GetRequest("price", "get", 1, @"^[1-9]\d*$", "0");
        //////
        int buyTypeParentID = 2, buyTypeID = 0, select = 0, buyPrice = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cSelects, out select);
        int.TryParse(cBuyPrice, out buyPrice);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        if (oBuyType == null || oBuyType.NumsCount <= 0)
            Utils.Error("找不到类型...", "");
        #endregion
        ShowSaleDatas(showSaleDataInitRecordCount, showSaleDataStep, oBuyType);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【龙虎玩法】");
        builder.Append(Out.Tab("</div>", ""));
        ////
        #region ----显示下注的类型导航
        builder.Append(Out.Tab("<div>", "<br/>"));
        List<PK10_BuyType> oBuyTypes = _logic.GetBuyTypes(oBuyType.ParentID);
        string shownavi = "";
        for (int i = 0; i < oBuyTypes.Count; i++)
        {
            string showtext = oBuyTypes[i].Descript.Trim();
            if (oBuyTypes[i].ID == buyTypeID)
                showtext = "<font color=\"red\">" + showtext + "</font>";
            string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + oBuyTypes[i].ID.ToString().Trim() + "&amp;select=" + select.ToString().Trim() + "&amp;price=" + buyPrice.ToString().Trim()) + "\">" + showtext + "</a>";
            shownavi += str;
            if (i < oBuyTypes.Count - 1)
                shownavi += " |";
            if (i == 2 || i == 9)
                shownavi += "<br />";
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        //////
        #region 显示选择结果
        builder.Append(Out.Tab("<div>", Out.Hr()));
        string dname = (select == 1) ? "买龙" : "买虎";
        builder.Append("当前选择：[" + oBuyType.Descript.Trim() + "." + dname + "] ");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示龙虎导航
        string red1 = "<font color=\"red\">";
        string red2 = "</font>";
        string navi1 = (select == 1) ? (red1 + "买龙" + red2) : "买龙";
        string navi2 = (select == 0) ? (red1 + "买虎" + red2) : "买虎";
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=1" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi1 + "</a> | ");
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=0" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi2 + "</a>  ");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region ----显示快捷下注金额       
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=200000" + "\">" + "20万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=400000" + "\">" + "40万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=600000" + "\">" + "60万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=800000" + "\">" + "80万" + "</a> ");
        //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=1000000" + "\">" + "100万" + "</a> ");
        builder.Append(GetPaySettings(myUserID, buyTypeParentID, buyTypeID, select.ToString()));
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        decimal rate = (select == 0) ? oBuyType.d8 : oBuyType.d9;
        #region 显示收工下注金额
        string strText = "/,/,/,或手动输入" + _logic.GetGoldName(isTest) + ":/,/,/,";
        string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle";
        string strType = "hidden,hidden,hidden,num,hidden,hidden,hidden";
        string strValu = buyTypeParentID + "'" + buyTypeID + "'" + select + "'" + buyPrice + "'" + myCurrentSaleID + "'pay'"+cSelects ;
        string strEmpt = "false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定下注," + myFileName + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示赔率/备注
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("(赔率：" + rate + ")");
        if (!string.IsNullOrEmpty(oBuyType.Remark))
            builder.Append("<br/>" + oBuyType.Remark.Trim());
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    private void BuyPage5() //任选买法
    {
        #region 读取参数
        string cBuyTypeParentID = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("buytype", "get", 1, @"^[1-9]\d*$", "0");
        string cSelects = Utils.GetRequest("Select", "get", 1, "", "");//已经选择的号码,例如（Select=1,02|2,03|3,05|4,09）
        string cAutoSelect = Utils.GetRequest("AutoSelect", "get", 1, "", "");
        string cBuyPrice = Utils.GetRequest("price", "get", 1, @"^[1-9]\d*$", "0");
        bool isAutoSelect = false;
        if (cAutoSelect.Trim() == "1")
            isAutoSelect = true;
        //////
        int buyTypeID = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int buyTypeParentID = 1, buyPrice = 0;
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cBuyPrice, out buyPrice);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        if (oBuyType == null || oBuyType.NumsCount <= 0)
            Utils.Error("找不到类型...", "");
        #endregion
        ShowSaleDatas(showSaleDataInitRecordCount, showSaleDataStep, oBuyType);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【任选玩法】");
        builder.Append(Out.Tab("</div>", "<br />"));
        ////
        #region ----显示下注的类型导航
        builder.Append(Out.Tab("<div>", ""));
        List<PK10_BuyType> oBuyTypes = _logic.GetBuyTypes(oBuyType.ParentID);
        string shownavi = "";
        for (int i = 0; i < oBuyTypes.Count; i++)
        {
            string showtext = oBuyTypes[i].Descript.Trim();
            if (oBuyTypes[i].ID == buyTypeID)
                showtext = "<font color=\"red\">" + showtext + "</font>";
            string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + oBuyTypes[i].ParentID.ToString().Trim() + "&amp;buytype=" + oBuyTypes[i].ID.ToString().Trim()) + "\">" + showtext + "</a>";
            shownavi += str;
            if (i < oBuyTypes.Count - 1)
                shownavi += " |";
            if (i == 2 || i == 9)
                shownavi += "<br />";
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        //////
        string[] aNums = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
        List<string> aSelects;
        if (!isAutoSelect)
            aSelects = CheckAndGetNewSelect2(aNums,cSelects, oBuyType);//检测选择的号码是否符合规则，并返回新的选择字符串数组(每行只记录一个第N位某一号码，如1,03）
        else
        {
            aSelects = CreateRandomNums2(aNums,null, oBuyType.NumsCount, ""); //机选一注
        }
        #region ----显示下注选择区
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("当前选择：[" + oBuyType.Name.Trim() + "] ");
        builder.Append("    " + "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+buyTypeParentID+"&amp;buyType=" + cBuyType + "&amp;AutoSelect=1") + "\"><font color=\"brown\">" + "机选一注" + "</font></a> ");
        builder.Append("<br />");
        string buyStr = "", searchStr = "", showNumStr = "", newSelect = "";

        List<string> anewSelects = new List<string>();
        for (int i = 1; i <= 1; i++)
        {
            buyStr = "";
            for (int j = 0; j < aNums.Length; j++)
            {
                #region 判断当前位数的当前号码，是否已经选择，并设置显示状态
                searchStr = aNums[j].Trim();
                showNumStr = "";
                newSelect = "";
                anewSelects = new List<string>(aSelects);
                if (aSelects.Contains(searchStr))
                {
                    anewSelects.Remove(searchStr);
                    showNumStr = "<font color=\"red\">" + aNums[j].Trim() + "</font>";
                }
                else
                {
                    anewSelects.Add(searchStr);
                    showNumStr = aNums[j].Trim();
                }
                #endregion
                #region 重新生成Select字符串
                if (anewSelects.Count > 0)
                {
                    for (int k = 0; k < anewSelects.Count; k++)
                    {
                        if (newSelect == "")
                            newSelect = anewSelects[k].Trim();
                        else
                            newSelect += "|" + anewSelects[k].Trim();
                    }
                }
                #endregion
                buyStr += " <a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + cBuyType + "&amp;Select=" + newSelect) + "\">" + showNumStr + "</a> ";
            }
            builder.Append(buyStr);
            builder.Append("<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示下注选择结果
        string buyNums = GenNumsFromSelectStr2(aSelects, oBuyType.NumsCount);
        List<string> numsDetail = _logic.GenBuyNums2(buyNums, oBuyType.NumsCount);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您选择了[" + numsDetail.Count.ToString().Trim() + "]注:");
        builder.Append("<br/>");
        if (string.IsNullOrEmpty(buyNums))
            builder.Append("<font color=\"red\">" + "(您还没选择下注的号码)" + "</font>");
        else
        {
            builder.Append("<font color=\"red\">" + buyNums + "</font>");
            builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\"> 清空</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region ----显示快捷下注金额       
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
        builder.Append(GetPaySettings(myUserID, buyTypeParentID, buyTypeID, cSelects));
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        decimal rate = oBuyType.d1;
        #region ----显示下注号码及下注金额       
        string strText = "/,/,/,或手动输入:/,/,/,/,/,";
        string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
        string strType = "hidden,hidden,hidden,num,hidden,hidden,hidden,hidden";
        string strValu = buyTypeParentID + "'" + buyTypeID + "'" + buyNums + "'" + buyPrice + "'" + myCurrentSaleID + "'pay'"+cSelects+"'"+rate ;
        string strEmpt = "false,false,false,false,false,false,false,false";

        string strIdea = "''<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\">清号<／a>'''''|/";
        string strOthe = "确定下注," + myFileName + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示赔率/备注
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("(赔率：" + rate + ")");
        if (!string.IsNullOrEmpty(oBuyType.Remark))
            builder.Append("<br/>" + oBuyType.Remark.Trim());
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    private void BuyPage6() //冠亚军买法
    {
        #region 读取参数
        string cBuyTypeParentID = Utils.GetRequest("buytypePID", "get", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("buytype", "get", 1, @"^[1-9]\d*$", "0");
        string cSelects = Utils.GetRequest("Select", "get", 1, "", "");//已经选择的号码,例如（Select=1,02|2,03|3,05|4,09）
        string cAutoSelect = Utils.GetRequest("AutoSelect", "get", 1, "", "");
        string cBuyPrice = Utils.GetRequest("price", "get", 1, @"^[1-9]\d*$", "0");
        bool isAutoSelect = false;
        if (cAutoSelect.Trim() == "1")
            isAutoSelect = true;
        //////
        int buyTypeID = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int buyTypeParentID = 1, buyPrice = 0;
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cBuyPrice, out buyPrice);
        int select = 0;
        int.TryParse(cSelects, out select);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        if (oBuyType == null || oBuyType.NumsCount <= 0)
            Utils.Error("找不到类型...", "");
        #endregion
        ShowSaleDatas(showSaleDataInitRecordCount, showSaleDataStep, oBuyType);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【冠亚军玩法】");
        builder.Append(Out.Tab("</div>", "<br />"));
        ////
        #region ----显示下注的类型导航
        builder.Append(Out.Tab("<div>", ""));
        List<PK10_BuyType> oBuyTypes = _logic.GetBuyTypes(oBuyType.ParentID);
        string shownavi = "";
        for (int i = 0; i < oBuyTypes.Count; i++)
        {
            string showtext = oBuyTypes[i].Descript.Trim();
            if (oBuyTypes[i].ID == buyTypeID)
                showtext = "<font color=\"red\">" + showtext + "</font>";
            string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + oBuyTypes[i].ParentID.ToString().Trim() + "&amp;buytype=" + oBuyTypes[i].ID.ToString().Trim()) + "\">" + showtext + "</a>";
            shownavi += str;
            if (i < oBuyTypes.Count - 1)
                shownavi += " |";
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        decimal rate = 0;
        switch (oBuyType.ID)
        {
            case 60:
                #region 和值号码
                string[] aNums = new string[] { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" };
                List<string> aSelects;
                if (!isAutoSelect)
                    aSelects = CheckAndGetNewSelect2(aNums,cSelects, oBuyType);//检测选择的号码是否符合规则，并返回新的选择字符串数组(每行只记录一个第N位某一号码，如1,03）
                else
                {
                    aSelects = CreateRandomNums2(aNums,null, oBuyType.NumsCount, ""); //机选一注
                }
                #region ----显示下注选择区
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("当前选择：[" + oBuyType.Name.Trim() + "] ");
                builder.Append("    " + "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + cBuyType + "&amp;AutoSelect=1") + "\"><font color=\"brown\">" + "机选一注" + "</font></a> ");
                builder.Append("<br />");
                string buyStr = "", searchStr = "", showNumStr = "", newSelect = "";
                List<string> anewSelects = new List<string>();
                buyStr = "";
                for (int j = 0; j < aNums.Length; j++)
                {
                    #region 判断当前位数的当前号码，是否已经选择，并设置显示状态
                    searchStr = aNums[j].Trim();
                    showNumStr = "";
                    newSelect = "";
                    anewSelects = new List<string>(aSelects);
                    if (aSelects.Contains(searchStr))
                    {
                        anewSelects.Remove(searchStr);
                        showNumStr = "<font color=\"red\">" + aNums[j].Trim() + "</font>";
                    }
                    else
                    {
                        anewSelects.Add(searchStr);
                        showNumStr = aNums[j].Trim();
                    }
                    #endregion
                    #region 重新生成Select字符串
                    if (anewSelects.Count > 0)
                    {
                        for (int k = 0; k < anewSelects.Count; k++)
                        {
                            if (newSelect == "")
                                newSelect = anewSelects[k].Trim();
                            else
                                newSelect += "|" + anewSelects[k].Trim();
                        }
                    }
                    #endregion
                    buyStr += " <a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + cBuyType + "&amp;Select=" + newSelect) + "\">" + showNumStr + "</a> ";
                    if (j>=10 && j % 10 == 0)
                        buyStr += "<br/>";
                }
                builder.Append(buyStr);
                builder.Append("<br />");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region 显示下注选择结果
                string buyNums = GenNumsFromSelectStr2(aSelects, oBuyType.NumsCount);
                List<string> numsDetail = _logic.GenBuyNums2(buyNums, oBuyType.NumsCount);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("您选择了[" + numsDetail.Count.ToString().Trim() + "]注:");
                builder.Append("<br/>");
                if (string.IsNullOrEmpty(buyNums))
                    builder.Append("<font color=\"red\">" + "(您还没选择下注的号码)" + "</font>");
                else
                {
                    builder.Append("<font color=\"red\">" + buyNums + "</font>");
                    builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\"> 清空</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region ----显示快捷下注金额       
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
                builder.Append(GetPaySettings(myUserID, buyTypeParentID, buyTypeID, cSelects));
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region ----显示下注号码及下注金额       
                string strText = "/,/,/,或手动输入:/,/,/,";
                string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle";
                string strType = "hidden,hidden,hidden,num,hidden,hidden,hidden";
                string strValu = buyTypeParentID + "'" + buyTypeID + "'" + buyNums + "'" + buyPrice + "'" + myCurrentSaleID + "'pay'"+cSelects ;
                string strEmpt = "false,false,false,false,false,false,false";

                string strIdea = "''<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "") + "\">清号</a>''''|/";
                string strOthe = "确定下注," + myFileName + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                //
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #endregion
                #region 显示赔率/备注
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("(赔率：");
                builder.Append("3|4|18|19="+oBuyType.d1.ToString().Trim()+ "倍,");
                builder.Append("5|6|16|17=" + oBuyType.d2.ToString().Trim() + "倍,");
                builder.Append("7|8|14|15=" + oBuyType.d3.ToString().Trim() + "倍,");
                builder.Append("9|10|12|13=" + oBuyType.d4.ToString().Trim() + "倍,");
                builder.Append("11=" + oBuyType.d5.ToString().Trim() + "倍");
                builder.Append(")");
                if (!string.IsNullOrEmpty(oBuyType.Remark))
                    builder.Append("<br/>" + oBuyType.Remark.Trim());
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                break;
            case 61:
                #region 大小
                #region 显示选择结果
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("当前选择：[" + oBuyType.Descript.Trim() + "." + GetBuyType23Name(buyTypeParentID, select) + "] ");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region 显示大小单双导航
                string red1 = "<font color=\"red\">";
                string red2 = "</font>";
                string navi1 = (buyTypeID  == 61 && select == 1) ? (red1 + "买大" + red2) : "买大";
                string navi2 = (buyTypeID == 61 && select == 0) ? (red1 + "买小" + red2) : "买小";
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=1" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi1 + "</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=0" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi2 + "</a>  ");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region ----显示快捷下注金额       
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=200000" + "\">" + "20万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=400000" + "\">" + "40万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=600000" + "\">" + "60万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=800000" + "\">" + "80万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=1000000" + "\">" + "100万" + "</a> ");
                builder.Append(GetPaySettings(myUserID, buyTypeParentID, buyTypeID, select.ToString()));
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                rate = (select == 0) ? oBuyType.d8 : oBuyType.d9;
                #region 显示收工下注金额
                strText = "/,/,/,或手动输入" + _logic.GetGoldName(isTest) + ":/,/,/,/,";
                strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
                strType = "hidden,hidden,hidden,num,hidden,hidden,hidden,hidden";
                strValu = buyTypeParentID + "'" + buyTypeID + "'" + select + "'" + buyPrice + "'" + myCurrentSaleID + "'pay'"+cSelects+"'"+rate ;
                strEmpt = "false,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定下注," + myFileName + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #endregion
                #region 显示赔率/备注
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("(赔率：" + rate + ")");
                if (!string.IsNullOrEmpty(oBuyType.Remark))
                    builder.Append("<br/>" + oBuyType.Remark.Trim());
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                break;
            case 62:
                #region 单双
                #region 显示选择结果
                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("当前选择：[" + oBuyType.Descript.Trim() + "." + GetBuyType23Name(buyTypeParentID, select) + "] ");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region 显示大小单双导航
                red1 = "<font color=\"red\">";
                red2 = "</font>";
                string navi3 = (buyTypeID == 62 && select == 0) ? (red1 + "买单" + red2) : "买单";
                string navi4 = (buyTypeID == 62 && select == 1) ? (red1 + "买双" + red2) : "买双";
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=0" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi3 + "</a> | ");
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+ buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=1" + "&amp;price = " + buyPrice.ToString().Trim()) + "\">" + navi4 + "</a>  ");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #region ----显示快捷下注金额       
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("选投注额：" + "[<a href=\"" + Utils.getUrl("pk10_paysettings.aspx" + "?backurl=" + Utils.PostPage(1)) + "\">设置</a>]" + "<br/>");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=200000" + "\">" + "20万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=400000" + "\">" + "40万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=600000" + "\">" + "60万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=800000" + "\">" + "80万" + "</a> ");
                //builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price=1000000" + "\">" + "100万" + "</a> ");
                builder.Append(GetPaySettings(myUserID, buyTypeParentID, buyTypeID, select.ToString()));
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                rate = (select == 0) ? oBuyType.d8 : oBuyType.d9;
                #region 显示收工下注金额
                strText = "/,/,/,或手动输入" + _logic.GetGoldName(isTest) + ":/,/,/,/,";
                strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
                strType = "hidden,hidden,hidden,num,hidden,hidden,hidden,hidden";
                strValu = buyTypeParentID.ToString().Trim() + "'" + buyTypeID.ToString().Trim() + "'" + select.ToString().Trim() + "'" + buyPrice.ToString().Trim() + "'" + myCurrentSaleID.ToString().Trim() + "'pay'"+cSelects+"'"+rate ;
                strEmpt = "false,false,false,false,false,false,false,false";
                strIdea = "/";
                strOthe = "确定下注," + myFileName + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                #endregion
                #region 显示赔率/备注
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("(赔率：" + rate + ")");
                if (!string.IsNullOrEmpty(oBuyType.Remark))
                    builder.Append("<br/>" + oBuyType.Remark.Trim());
                builder.Append(Out.Tab("</div>", ""));
                #endregion
                break;
        }
    }
    #endregion
    private void PayPage()//付款确认页
    {
        Master.Title = GameName + ".付款";
        string navText = "&gt;付款";
        ShowGameTop(navText);
        ShowUser();
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        //
        #region 取回购买的信息
        string cBuyTypeParentID = Utils.GetRequest("BuyTypeParentID", "post", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("BuyTypeID", "post", 1, @"^[1-9]\d*$", "0");
        string cBuyNums = Utils.GetRequest("BuyNums", "post", 1, "", "");//已经选择的号码,例如（第一位选择02、03，第二位选择03、05，则是Select=1,02|1,03|2,03|2,05）
        string cBuyPrice = Utils.GetRequest("BuyPrice", "post", 1, "", "");
        string cSelects = Utils.GetRequest("selecttitle", "post", 1, "", "");
        string cListID= Utils.GetRequest("ListID", "post", 1, "", "");
        string cRate= Utils.GetRequest("rate", "post", 1, "", "");
        int buyTypeParentID = 0, buyTypeID = 0,  buyPrice = 0, listId=0;
        int.TryParse(cBuyType, out buyTypeID);
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cBuyPrice, out buyPrice);
        int.TryParse(cListID, out listId);
        decimal rate = 0;
        decimal.TryParse(cRate, out rate);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        //过滤掉输入中的错误，返回合法的下注字符串。
        cBuyNums = _logic.CheckAndGetNewNums(cBuyNums, oBuyType);
        //
        List<string> aSelects;
        aSelects = _logic.GenBuyNums(cBuyNums, oBuyType);
        if (oBuyType.NumsCount <= 0 || aSelects.Count <= 0)
            Utils.Error("下注的号码有误！", "");
        int minPrice = int.Parse(ub.GetSub("MinPayPrice", xmlPath));
        int maxPrice = int.Parse(ub.GetSub("MaxPayPrice", xmlPath));
        if (buyPrice < minPrice || buyPrice > maxPrice)
            Utils.Error("下注单价有误，范围是（"+minPrice.ToString().Trim()+"-"+maxPrice.ToString().Trim()+"）"+_logic.GetGoldName(isTest), "");
        int money = buyPrice * aSelects.Count;
        #endregion
        PK10_List list = _logic.GetListByID(listId);
        if (list == null)
            Utils.Error("当前没有开售的期号！", "");
        PK10_Stutas status=GetAndShowSaleDataSatus(list);
        #region 显示下注内容
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您下注的期号是：第" + list.No.Trim() + "期" + "<br /> ");
        builder.Append("您下注的内容是：" + _logic.GetBuyDescript(cBuyNums,oBuyType) + "<br /> ");
        builder.Append("您每注单价是：" + buyPrice.ToString().Trim() + "(" + _logic.GetGoldName(isTest) + ")" + "<br /> ");
        builder.Append("您共需要支付：" + money.ToString().Trim() + "(" + _logic.GetGoldName(isTest) + ")" + "<br /> ");
        if (oBuyType.PayLimitType == 1 && oBuyType.RateFlag==1)
        {
            decimal currRate = cBuyNums == "0" ? oBuyType.d8 : oBuyType.d9;
            builder.Append("【赔率】：" + currRate.ToString().Trim());
            if (rate != currRate)
            {
                builder.Append("<font color=\"red\">赔率发生了变化，原赔率："+rate.ToString().Trim()+"</font>");
                rate = currRate;
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #endregion
        if (status!=PK10_Stutas.在售)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<font color=\"red\">"+"您选择的期号已经停售了，不能下注！"+"</font>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            #region 显示确认购买
            string strText = "/,/,/,/,/,/,/,";
            string strName = "BuyTypeParentID,BuyTypeID,BuyNums,BuyPrice,ListID,act,selecttitle,rate";
            string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = buyTypeParentID + "'" + buyTypeID + "'" + cBuyNums + "'" + buyPrice + "'" + myCurrentSaleID + "'payok'"+cSelects+"'"+rate ;
            string strEmpt = "false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定购买," + Utils.getUrl(myFileName) + ",post,1,red";
            //string strOthe = "确定购买|返回下注," + Utils.getUrl(myFileName) + ",post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            #endregion
        }
        //
        builder.Append(Out.Tab("<div>", "<br /><br />"));
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID="+cBuyTypeParentID+"&amp;buyType=" + buyTypeID.ToString().Trim()+"&amp;select="+ cSelects + "&amp;price="+buyPrice) + "\">返回下注</a>");
        builder.Append(Out.Tab("</div>", ""));
        //
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void PayOKPage()//付款页
    {
        string navText = "&gt;付款";
        ShowGameTop(navText);
        int uID = GetUserID();
        #region 取回购买的信息
        string cBuyTypeParentID = Utils.GetRequest("BuyTypeParentID", "post", 1, @"^[1-9]\d*$", "0");
        string cBuyType = Utils.GetRequest("BuyTypeID", "post", 1, @"^[1-9]\d*$", "0");
        string cBuyNums = Utils.GetRequest("BuyNums", "post", 1, "", "");//已经选择的号码,例如（第一位选择02、03，第二位选择03、05，则是Select=1,02|1,03|2,03|2,05）
        string cSelects = Utils.GetRequest("selecttitle", "post", 1, "", "");
        string cBuyPrice = Utils.GetRequest("BuyPrice", "post", 1, "", "");
        string cListID = Utils.GetRequest("ListID", "post", 1, "", "");
        string cRate = Utils.GetRequest("rate", "post", 1, "", "");
        int buyTypeParentID = 0, buyTypeID = 0, buyPrice = 0, listId = 0;
        int.TryParse(cBuyType, out buyTypeID);
        int.TryParse(cBuyTypeParentID, out buyTypeParentID);
        int.TryParse(cBuyPrice, out buyPrice);
        int.TryParse(cListID, out listId);
        decimal rate = 0;
        decimal.TryParse(cRate, out rate);
        PK10_BuyType oBuyType = _logic.GetBuyTypeByID(buyTypeID);
        //
        //过滤掉输入中的错误，返回合法的下注字符串。
        cBuyNums = _logic.CheckAndGetNewNums(cBuyNums, oBuyType);
        //
        List<string> aSelects = _logic.GenBuyNums(cBuyNums, oBuyType);
        if (oBuyType.NumsCount<= 0 || aSelects.Count <= 0)
            Utils.Error("下注的号码有误！", "");
        int minPrice = int.Parse(ub.GetSub("MinPayPrice", xmlPath));
        int maxPrice = int.Parse(ub.GetSub("MaxPayPrice", xmlPath));
        if (buyPrice < minPrice || buyPrice > maxPrice)
            Utils.Error("下注单价有误，范围是（" + minPrice.ToString().Trim() + "-" + maxPrice.ToString().Trim() + "）" + _logic.GetGoldName(isTest), "");
        #endregion
        #region 生成购买记录
        PK10_Buy buy = new PK10_Buy();
        buy.BuyCount = 0;
        buy.BuyDescript = "";
        buy.BuyMulti = 1;
        buy.BuyPrice = buyPrice;
        buy.BuyTime = DateTime.Now;
        buy.BuyType = buyTypeID;
        buy.isRobot = 0;
        buy.isTest = (isTest) ? 1 : 0;
        buy.ListID = listId;
        buy.Nums = cBuyNums;
        buy.NumsDetail = "";
        buy.NumType = 0;
        buy.PayMoney = 0;
        buy.uID = uID;
        buy.uName = new BCW.BLL.User().GetUsName(uID);
        buy.Rate = rate.ToString().Trim();
        #endregion
        #region 付款
        //支付安全提示
        string[] p_pageArr = { "BuyTypeParentID", "BuyTypeID", "BuyNums", "selecttitle", "act", "BuyPrice" , "ListID","rate" };
        BCW.User.PaySafe.PaySafePage(uID, Utils.getPageUrl(), p_pageArr);
        //
        string cPayResult = _logic.Pay(true,buy, Utils.getPageUrl());
        if (cPayResult == "")
        {
            #region 活跃抽奖入口_20160621姚志光
            try
            {
                string _GameName = ub.GetSub("GameName", xmlPath);
                //表中存在pk10额度记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName(_GameName))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (buy.PayMoney > new BCW.BLL.tb_WinnersGame().GetPrice(_GameName))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, 1, uID, buy.uName, _GameName, 3);
                        if (hit == 1)
                        {
                            //内线开关 1开
                            if (WinnersGuessOpen == "1")
                            {
                                //发内线到该ID
                                new BCW.BLL.Guest().Add(0, uID, buy.uName, TextForUbb);
                            }
                        }
                    }
                }
            }
            catch { }
            #endregion
            Utils.Success("下注", "下注成功，花费了" + buy.PayMoney.ToString().Trim() + "" + _logic.GetGoldName(isTest) + "<br />", Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim()+"&amp;select="+ cSelects + "&amp;price="+buyPrice), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<font color=\"red\">" + "出错啦..." + "</font> <br/>");
            builder.Append("<font color=\"red\">" + cPayResult + "</font>" + " <br /> ");
            builder.Append(Out.Tab("</div>", ""));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("您下注的期号是：第" + buy.ListNo.Trim() + "期" + "<br /> ");
            //builder.Append("您下注的内容是：" + buy.BuyDescript.Trim() + "<br /> ");
            //builder.Append("您每注单价是：" + buy.BuyPrice.ToString().Trim() + "<br /> ");
            //builder.Append("您共需要支付：" + buy.PayMoney.ToString().Trim() + "<br /> ");
            //builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID + "&amp;buyType=" + buyTypeID.ToString().Trim() + "&amp;select=" + cSelects + "&amp;price=" + buyPrice) + "\">&gt;返回下注</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        }
        #endregion
        //
        //builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        //ShowGameBottom(navText);
    }
    private void CasePage()//兑奖页
    {
        Master.Title = GameName+".兑奖中心";
        string navText = "&gt;兑奖";
        ShowGameTop(navText);
        ShowGameNavi(2);
        builder.Append(Out.Tab("", "<br/>"));
        ShowUser();
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        string cShowType = Utils.GetRequest("showtype", "get", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //

        if (showtype == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<font color=\"red\">待兑奖 | </font>");
            builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=case&amp;showtype=1") + "\">历史投注</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
            ShowMyCasePage();
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=case") + "\">待兑奖</a>");
            builder.Append("<font color=\"red\"> | 历史投注</font>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
            ShowMyListPage();
        }
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void ShowMyCasePage() //待兑奖列表
    {
        #region 显示待兑奖页面
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        string arrId = "";
        List<PK10_Buy> lists = _logic.GetTobeCaseDatas(myUserID, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act","showtype","backurl" };
        if (lists != null && lists.Count > 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            int k = 1;
            foreach (PK10_Buy item in lists)
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
                builder.Append("(" + item.ListNo.Trim() + "期).买" + item.BuyDescript.Trim());
                if (item.BuyCount > 1)
                {
                    builder.Append(",每注下" + item.BuyPrice.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                    builder.Append(",总下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                else
                {
                    builder.Append(",下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                builder.Append(",赢" + item.WinMoney.ToString().Trim() + "" + _logic.GetGoldName(item.isTest == 1 ? true : false) + "<a href=\"" + Utils.getUrl(myFileName + "?act=caseok&amp;buyid=" + item.ID.ToString().Trim() + "") + "\">兑奖</a>");
                arrId = arrId + " " + item.ID;
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(Out.Tab("</div>", ""));
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (!string.IsNullOrEmpty(arrId))
        {
            builder.Append(Out.Tab("", "<br />"));
            arrId = arrId.Trim();
            arrId = arrId.Replace(" ", ",");
            string strName = "arrId,act";
            string strValu = "" + arrId + "'casepost";
            string strOthe = "本页兑奖," + myFileName + ",post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
        }
        #endregion
    }
    private void ShowMyListPage() //我的下注记录
    {
        #region 显示用户下注记录页面
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        List<PK10_Buy> lists = _logic.GetBuyDatas(0,myUserID,0, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "showtype", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            string showlistno = "";
            int showid = 0;
            foreach (PK10_Buy item in lists)
            {
                if (showlistno == item.ListNo.Trim())
                {
                    showid++;
                }
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                    builder.Append("第" + item.ListNo.Trim() + "期开出：" + item.ListNums.Trim());
                    builder.Append(Out.Tab("</div>", ""));
                    showid = 1;
                    showlistno = item.ListNo.Trim();
                }
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append(showid.ToString().Trim()+".买" + item.BuyDescript.Trim());
                if (item.BuyCount > 1)
                {
                    builder.Append(",每注下" + item.BuyPrice.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                    builder.Append(",总下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                else
                {
                    builder.Append(",下注" + item.PayMoney.ToString().Trim() + _logic.GetGoldName(item.isTest == 1 ? true : false));
                }
                if (item.ListNums.Trim() == "")
                    builder.Append("<font color=\"green\">(未开奖)</font>");
                else
                {
                    if (item.WinMoney > 0)
                    {
                        builder.Append("<font color=\"red\">(赢" + item.WinMoney.ToString().Trim() + "" + _logic.GetGoldName(item.isTest == 1 ? true : false)+ ")</font>");
                        if (item.CaseFlag == 1)
                            builder.Append("已兑奖");
                        else
                            if (item.ValidFlag == 1)
                            builder.Append("未兑奖");
                        else
                            builder.Append("<font color =\"green\">已过期</font>");
                    }
                 }
                builder.Append("<a href=\"" + Utils.getPage(myFileName + "?act=buydetail&amp;id=" + item.ID+"&amp;backurl=" + Utils.PostPage(1)) + "\">" + "详细" + "</a>");
                k++;               
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        #endregion
    }
    private void CaseOKPage()
    {
        GetUserID();
        int buyid = Utils.ParseInt(Utils.GetRequest("buyid", "get", 2, @"^[0-9]\d*$", "选择兑奖无效"));
        PK10_Buy obuy = _logic.GetBuyByID(buyid);
        if (obuy == null || obuy.uID != myUserID)
            Utils.Error("非法兑奖！", "");
        //
        int rate = 0, charge = 0;
        int.TryParse(ub.GetSub("WinChargeRate", xmlPath), out rate); //扣除的费用比率
        int.TryParse(ub.GetSub("WinCharge", xmlPath), out charge);//每笔扣除的固定费用
        int casemoney = 0;
        //
        string cFlag = _logic.Case(buyid, rate, charge, out casemoney, Utils.getPageUrl());
        if (string.IsNullOrEmpty(cFlag))
        {
            Utils.Success("兑奖", "恭喜，成功兑奖" + casemoney.ToString().Trim() + "" + _logic.GetGoldName(isTest) + "", Utils.getUrl(myFileName + "?act=case"), "2");
        }
        else
        {
            Utils.Success("兑奖失败", cFlag, Utils.getUrl(myFileName + "?act=case"), "2");
        }
        //
    }
    private void CasePostPage() //整页兑奖
    {
        GetUserID();
        string arrId = "";
        arrId = Utils.GetRequest("arrId", "post", 1, "", "");
        if (!Utils.IsRegex(arrId.Replace(",", ""), @"^[0-9]\d*$"))
            Utils.Error("选择本页兑奖出错", "");
        //
        int rate = 0, charge = 0;
        int.TryParse(ub.GetSub("WinChargeRate", xmlPath), out rate); //扣除的费用比率
        int.TryParse(ub.GetSub("WinCharge", xmlPath), out charge);//每笔扣除的固定费用
        int casemoney = 0, sumCaseMoney = 0, errors = 0;
        string[] strArrId = arrId.Split(",".ToCharArray());
        if(strArrId.Length>0)
        {
            for(int i=0;i<strArrId.Length;i++)
            {
                int buyid = Convert.ToInt32(strArrId[i]);
                string cFlag = _logic.Case(buyid, rate, charge, out casemoney, Utils.getPageUrl());
                if (string.IsNullOrEmpty(cFlag))
                {
                    sumCaseMoney += casemoney;
                }
                else
                {
                    errors += casemoney;
                }
            }
            if (errors == 0)
                Utils.Success("兑奖", "恭喜，成功兑奖" + sumCaseMoney.ToString().Trim() + "" + _logic.GetGoldName(isTest) + "", Utils.getUrl(myFileName + "?act=case"), "2");
            else
                Utils.Success("兑奖", "成功兑奖" + sumCaseMoney.ToString().Trim() + "" + _logic.GetGoldName(isTest) + ",但有" + errors.ToString().Trim() + _logic.GetGoldName(isTest) + "不能成功兑奖", Utils.getUrl(myFileName + "?act=case"), "3");
        }
        else
            Utils.Success("兑奖", "没有需要兑奖的记录", Utils.getUrl(myFileName+"?act=case"), "2");
        //
    }
    private void ListPage() //历史开奖记录
    {
        Master.Title = GameName + ".历史开奖";
        string navText = "&gt;历史";
        ShowGameTop(navText);
        ShowGameNavi(3);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        //
        #region 读取参数
        string cShowType = Utils.GetRequest("showtype", "all", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //
        string cdate1 = Utils.GetRequest("begindate", "all", 1, "", "");
        string cdate2 = Utils.GetRequest("enddate", "all", 1, "", "");
        DateTime dDate1 = DateTime.Parse(DateTime.Now.AddDays(-1).ToShortDateString());
        DateTime dDate2 = dDate1.AddDays(1);
        try
        {
            if (cdate1 != "")
                DateTime.TryParse(cdate1, out dDate1);
            if (cdate2 != "")
                DateTime.TryParse(cdate2, out dDate2);
        }
        catch { };
        //
        DateTime d1 = dDate1, d2 = dDate2;
        if (showtype == 0)
        {
            d1 = DateTime.MinValue;
            d2 = DateTime.MaxValue;
        }
        #endregion
        #region 显示历史记录
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        #region 读取指定日期的数据页并显示
        int recordCount = 0;
        List<PK10_List> lists = _logic.GetOpenDatas(d1,d2, pagesize, pageindex,out recordCount);
        string[] pageValUrl = { "act", "showtype", "begindate", "enddate", "backurl" };
        if (lists!=null && lists.Count>0)
        {
            int k = 1;
            DateTime dd = DateTime.MinValue;
            foreach (PK10_List item in lists)
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
                if (dd != item.Date)
                {
                    dd = item.Date;
                    builder.Append("<font color=\"red\">日期：" + item.Date.ToShortDateString() + "</font><br />");
                }
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=listview&amp;id=" + item.ID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1)) + "\">" + item.No.Trim() + "期:" + item.Nums.ToString().Trim().Replace(',', ' ') + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        #endregion
        //
        #region 显示日期查询控件
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=list&amp;showtype=0") + "\">查询全部数据</a>");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "开始日期：/,截止日期：/,/,/,";
        string strName = "begindate,enddate,act,showtype";
        string strType = "text,text,hidden,hidden";
        string strValu = dDate1.ToShortDateString() + "'" + dDate2.ToShortDateString() + "'" + "list" + "'1";
        string strEmpt = "false,false,false，false";
        string strIdea = "/";
        string strOthe = "按日期查询," + Utils.getUrl(myFileName) + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void ListPage2() //历史开奖记录
    {
        Master.Title = GameName + ".历史开奖";
        string navText = "&gt;历史";
        ShowGameTop(navText);
        ShowGameNavi(3);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        //
        #region 显示历史记录
        DateTime today = DateTime.Parse(DateTime.Now.ToShortDateString());
        DateTime showdate = today; //要显示的数据日期
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cshowday = Utils.GetRequest("showdate", "get", 1, "", today.ToShortDateString().Trim());
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        DateTime.TryParse(cshowday, out showdate);
        int.TryParse(cpageindex, out pageindex);
        #region 显示日期导航
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=list&amp;showdate=" + showdate.AddDays(1).ToShortDateString().Trim()) + "\">" + showdate.AddDays(1).ToShortDateString().Trim() + " |" + "</a>");
        builder.Append(showdate.ToShortDateString().Trim() + " |");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=list&amp;showdate=" + showdate.AddDays(-1).ToShortDateString().Trim()) + "\">" + showdate.AddDays(-1).ToShortDateString().Trim() + "</a>");
        builder.Append(Out.Tab("</div>", Out.Hr()));
        #endregion
        #region 读取指定日期的数据页并显示
        int recordCount = 0;
        List<PK10_List> lists = _logic.GetOpenDatas(showdate, showdate, pagesize, pageindex, out recordCount);
        string[] pageValUrl = { "act", "showdate", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            foreach (PK10_List item in lists)
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
                builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=listview&amp;id=" + item.ID.ToString().Trim() + "&amp;showdate=" + showdate + "&amp;parentpageindex=" + pageindex) + "\">" + item.No.Trim() + "期:" + item.Nums.ToString().Trim().Replace(',', ' ') + "</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void ListViewPage()//某一期的开奖情况
    {
        string navText = "&gt;历史";
        ShowGameTop(navText);
        //
        #region 显示某一期的开奖情况
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        PK10_List model = _logic.GetListByID(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        Master.Title = "第" + model.No.Trim() + "期"+GameName+"开奖情况";
        string backurl = Utils.GetRequest("backurl", "all", 1, "", "");
        builder.Append(Out.Tab("<div >", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(backurl) + "\">返回上一级</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("第" + model.No.Trim() + "期");
        builder.Append("(共有" + model.PayCount.ToString().Trim() +"人次参与)" + " <br /> ");
        //builder.Append("共下注：" + model.PayMoney.ToString().Trim() + _logic.GetGoldName(isTest) + " <br /> ");
        builder.Append("共赢取：" + model.WinMoney.ToString().Trim() + _logic.GetGoldName(isTest));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        #region 显示所有中奖情况
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        List<PK10_Buy> lists = _logic.GetWinDatas(id,true, pagesize, pageindex,out recordCount);
        string[] pageValUrl = { "act","id","backurl" };
        if (lists != null && lists.Count > 0)
        {
            builder.Append("<div>共" + recordCount + "注中奖<br /></div>");
            int k = 1;
            foreach (PK10_Buy item in lists)
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
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + item.uID + "&amp;backurl=" + Utils.getPage(0)) + "\">" + item.uName.Trim() + "</a>获得" + item.WinMoney.ToString().Trim() + "" + _logic.GetGoldName(item.isTest==1?true:false) + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有中奖记录.."));
        }
        #endregion
        //
        #endregion
        //
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ",""));
        ShowGameBottom(navText);
    }
    private void ShowBuyDetail() //显示下注号码的中奖明细
    {
        string navText = "&gt;下注明细";
        ShowGameTop(navText);
        //
        #region 显示导航
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        PK10_Buy obuy = _logic.GetBuyByID(id);
        if (obuy == null)
        {
            Utils.Error("不存在的下注记录", "");
        }
        PK10_BuyType obuyType = _logic.GetBuyTypeByID(obuy.BuyType);
        if(obuyType==null)
            Utils.Error("不存在的下注类型", "");
        Master.Title = GameName + "下注明细";
        builder.Append(Out.Tab("<div >", "<br/>"));
        string backurl = Utils.GetRequest("backurl", "get", 1, "","");
        builder.Append("<a href=\"" + Utils.getUrl(backurl) + "\">返回上一页</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示明细
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("期号：" + obuy.ListNo + "<br />");
        //builder.Append("买家：" + obuy.uName + "<br />");
        builder.Append("时间：" + obuy.BuyTime + "<br />");
        builder.Append("下注：" + obuy.BuyDescript + "<br />");
        builder.Append("总注数：" + obuy.BuyCount + "<br />");
        builder.Append("每一注单价：" + obuy.BuyPrice + "<br />");
        //builder.Append("总支付金额：" + obuy.PayMoney + "<br />");
        builder.Append("开奖号码：" + obuy.ListNums + "<br />");
        string winnums = obuy.WinNums.Trim(); //号码、任选、和值，保存的是中奖号，大小单双龙虎保存的是0、1
        int wincount = 0;
        if (obuy.WinNums.Trim() != "")
        {
            wincount = winnums.Split('#').Length;
        }
        #region 取到中奖的明细
        switch (obuyType.ParentID)
        {
            case 2://大小
                int select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "大";
                if (select == 0)
                    winnums = "小";
                break;
            case 3://单双
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "双";
                if (select == 0)
                    winnums = "单";
                break;
            case 4://龙虎
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "龙";
                if (select == 0)
                    winnums = "虎";
                break;
            case 6://
                switch(obuyType.Type)
                {
                    case 2://大小
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "大";
                        if (select == 0)
                            winnums = "小";
                        break;
                    case 3://单双
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "双";
                        if (select == 0)
                            winnums = "单";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        #endregion
        #region 显示中奖的明细
        builder.Append("中奖号码：");
        if (winnums != "")
        {
            string[] awinnums = winnums.Split('#');
            for (int i = 0; i < wincount; i++)
            {
                string[] anums = awinnums[i].Split(',');
                if (anums.Length > 0)
                {
                    if (i == 0 )
                    {
                        builder.Append(anums[0]);
                        if (wincount <= 1)
                            builder.Append("(赔率" + obuy.WinPrice + ")<br />");
                        else 
                            builder.Append("...<a href=\"" + Utils.getUrl(myFileName + "?act=buydetail2&amp;id=" + id + "&amp;backurl="+Utils.getPage(0))+ "\">" + "查看完整" + "</a>" + "<br />");
                        break;
                    }
                    //
                    builder.Append(anums[0]); //前二前三前四，复式时候，WinNums会记录所有中奖的组合，格式为“中奖号码，匹配奖号个数，赔率”
                    if (anums.Length > 1)
                        builder.Append("中出个数："+anums[1]);
                    if (anums.Length > 2)
                        builder.Append("赔率：" + anums[2]);
                }
                builder.Append("<br />");
            }
            if(wincount==0)
                builder.Append("<br />");
        }
        else
        {
            builder.Append("<br />");
        }
        #endregion
        builder.Append("中奖注数：" + wincount + "<br />");
        //if (wincount <= 1)
        //    builder.Append("中奖赔率：" + obuy.WinPrice + "<br />");
        //else
        //    builder.Append("平均赔率：" + obuy.WinPrice + "<br />");
        builder.Append("中奖金额：" + obuy.WinMoney + "<br />");
        builder.Append("手续费：" + obuy.Charges + "<br />");
        //builder.Append("兑奖金额：" + obuy.CaseMoney + "<br />");
        builder.Append("有效期至：" + obuy.ValidDate + "<br />");
        string status = "";
        #region 状态
        if (obuy.ListNums.Trim() == "")
            status = "未开奖";
        else
        {
            if (obuy.WinMoney > 0)
            {
                if (obuy.CaseFlag == 0)
                {
                    if (obuy.ValidFlag == 0)
                        status = "已过期";
                    else
                        status = "未兑奖";                  
                }
                else
                {
                    status = "已兑奖";
                }
            }
        }
        #endregion
        builder.Append("状态：" + status + "<br />");
        builder.Append(Out.Tab("</div", ""));
        #endregion
        //
        builder.Append(Out.Tab("<div >", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(backurl) + "\">返回上一页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void ShowBuyDetail2() //显示下注号码的中奖明细
    {
        string navText = "&gt;下注明细";
        ShowGameTop(navText);
        //
        #region 显示导航
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        PK10_Buy obuy = _logic.GetBuyByID(id);
        if (obuy == null)
        {
            Utils.Error("不存在的下注记录", "");
        }
        PK10_BuyType obuyType = _logic.GetBuyTypeByID(obuy.BuyType);
        if (obuyType == null)
            Utils.Error("不存在的下注类型", "");
        Master.Title = GameName + "下注明细";
        builder.Append(Out.Tab("<div >", "<br/>"));
        string backurl = Utils.GetRequest("backurl", "get", 1, "", "");
    
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buydetail&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0)) + "\">返回上一页</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示明细
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("期号：" + obuy.ListNo + "<br />");
        //builder.Append("下注：" + obuy.BuyDescript + "<br />");
        builder.Append("开奖号码：" + obuy.ListNums + "<br />");
        string winnums = obuy.WinNums.Trim(); //号码、任选、和值，保存的是中奖号，大小单双龙虎保存的是0、1
        int wincount = 0;
        if (obuy.WinNums.Trim() != "")
        {
            wincount = winnums.Split('#').Length;
        }
        #region 取到中奖的明细
        switch (obuyType.ParentID)
        {
            case 2://大小
                int select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "大";
                if (select == 0)
                    winnums = "小";
                break;
            case 3://单双
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "双";
                if (select == 0)
                    winnums = "单";
                break;
            case 4://龙虎
                select = 0;
                int.TryParse(winnums, out select);
                if (select == 1)
                    winnums = "龙";
                if (select == 0)
                    winnums = "虎";
                break;
            case 6://
                switch (obuyType.Type)
                {
                    case 2://大小
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "大";
                        if (select == 0)
                            winnums = "小";
                        break;
                    case 3://单双
                        select = 0;
                        int.TryParse(winnums, out select);
                        if (select == 1)
                            winnums = "双";
                        if (select == 0)
                            winnums = "单";
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }
        #endregion
        #region 显示中奖的明细
        builder.Append("中奖号码：<br/>");
        if (winnums != "")
        {
            string[] awinnums = winnums.Split('#');
            for (int i = 0; i < wincount; i++)
            {
                string[] anums = awinnums[i].Split(',');
                if (anums.Length > 0)
                {
                    builder.Append(anums[0]); //前二前三前四，复式时候，WinNums会记录所有中奖的组合，格式为“中奖号码，匹配奖号个数，赔率”
                    if (anums.Length > 1)
                        builder.Append("中出个数：" + anums[1]);
                    if (anums.Length > 2)
                        builder.Append("赔率：" + anums[2]);
                }
                builder.Append("<br />");
            }

        }
        else
        {
            builder.Append("<br />");
        }
        #endregion
        builder.Append("中奖注数：" + wincount + "<br />");
        builder.Append("中奖金额：" + obuy.WinMoney + "<br />");
        builder.Append(Out.Tab("</div", "<br />"));
        #endregion
        builder.Append(Out.Tab("<div >", ""));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName + "?act=buydetail&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0)) + "\">返回上一页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
    }
    private void TopPage() //排行榜
    {
        Master.Title = GameName + ".排行榜";
        string navText = "&gt;排行";
        ShowGameTop(navText);
        ShowGameNavi(4);
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        string cShowType = Utils.GetRequest("showtype", "get", 1, "", "0");
        int showtype = 0;
        int.TryParse(cShowType, out showtype);
        //
        #region 显示排行的类型
        DateTime beginDate = DateTime.MinValue;
        DateTime endDate = DateTime.MaxValue;
        //builder.Append(Out.Tab("<div>", ""));
        //switch (showtype)
        //{
        //    case 1:
        //        builder.Append("<font color=\"red\">最近7天 | </font>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=2") + "\">最近30天 | </a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=3") + "\">最近1年 |</a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=0") + "\">全部</a>");
        //        beginDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
        //        break;
        //    case 2:
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=1") + "\">最近7天 | </a>");
        //        builder.Append("<font color=\"red\">最近30天 | </font>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=3") + "\">最近1年 |</a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=0") + "\">全部 | </a>");
        //        beginDate = DateTime.Parse(DateTime.Now.AddDays(-30).ToShortDateString());
        //        break;
        //    case 3:
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=1") + "\">最近7天 | </a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=2") + "\">最近30天 | </a>");
        //        builder.Append("<font color=\"red\">最近1年 |</font>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=0") + "\">全部</a>");
        //        beginDate = DateTime.Parse(DateTime.Now.AddDays(-365).ToShortDateString());
        //        break;
        //    default:
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=1") + "\">最近7天 | </a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=2") + "\">最近30天 | </a>");
        //        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=top&amp;showtype=3") + "\">最近1年 |</a>");
        //        builder.Append("<font color=\"red\">全部</font>");
        //        break;
        //}
        //builder.Append(Out.Tab("</div>", ""));
        //builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #endregion
        #region 显示排行数据
        int pageindex = 1;//默认显示第一页
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        List<PK10_Top> lists = _logic.GetWinTopDatas(beginDate ,endDate,0,100,pagesize, pageindex, out recordCount); //只显示前100名
        string[] pageValUrl = { "act","showtype","backurl" };
        if (lists != null && lists.Count > 0)
        {
            int k = 1;
            foreach (PK10_Top item in lists)
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
                builder.Append("[第" +item.No.ToString().Trim() + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + item.UsID.ToString().Trim() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(item.UsID)+"("+item.UsID+")" + "</a>赢" + item.iGold.ToString().Trim() + "" + _logic.GetGoldName(isTest) + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    private void ReportPage() //分析页
    {
        Master.Title = GameName + ".分析";
        string navText = "&gt;分析";
        ShowGameTop(navText);
        ShowGameNavi(5);
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0,buyTypeParentID=0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        #endregion
        //
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示查询的期数
        builder.Append(Out.Tab("<div>", ""));
        string strcount1 =(count==30)? "<font color=\"red\">近30期</font>" : "近30期";
        string strcount2 = (count == 50) ? "<font color=\"red\">近50期</font>" : "近50期";
        string strcount3 = (count == 100) ? "<font color=\"red\">近100期</font>" : "近100期";
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;count=30") + "\">" + strcount1 + "</a> |");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;count=50") + "\">" + strcount2 + "</a> |");
        builder.Append("<a href =\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;count=100") + "\">" + strcount3 + "</a>");
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示分析的总类别
        List<PK10_BuyType> plist = _logic.GetBuyTypes(0);
        if (plist != null)
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            foreach (PK10_BuyType pitem in plist)
            {
                string showtext = pitem.Descript.Trim();
                if (pitem.ID == 2)  //为了减少显示的长度，合并大小单双这两种类型（ID=2、3）的下注页面。
                    showtext = "大小单双" ;
                if (pitem.ID != 3)
                {
                    if (pitem.ID == buyTypeParentID)
                        showtext = "<font color=\"red\">" + showtext + "</font>";
                    builder.Append(" <a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + pitem.ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ");
                    builder.Append(" |");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        #region 显示分析的类型与数据
        switch(buyTypeParentID)
        {
            case 1:
                ReportPage1();
                break;
            case 2:
            case 3:
                ReportPage23();
                break;
            case 4:
                ReportPage4();
                break;
            case 5:
                ReportPage5();
                break;
            case 6:
                ReportPage6();
                break;
        }
        #endregion
        //
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
        ShowGameBottom(navText);
    }
    #region 各种分析页
    private void ReportPage1() //号码分析页
    {
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0, buyTypeParentID = 0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        PK10_BuyType obuytype = _logic.GetBuyTypeByID(buyTypeID);
        if (obuytype==null || obuytype.ParentID != buyTypeParentID)
            buyTypeID = 0;
        #endregion
        #region 显示分析的类型
        builder.Append(Out.Tab("<div>", ""));
        string shownavi = "";
        List<PK10_BuyType> types = _logic.GetBuyTypes(buyTypeParentID);
        if (types != null && types.Count > 0)
        {
            if (buyTypeID == 0)
            {
                buyTypeID = types[0].ID;
                obuytype = _logic.GetBuyTypeByID(buyTypeID);
            }
            for (int i = 0; i < types.Count; i++)
            {
                string showtext = types[i].Descript.Trim();
                if (types[i].ID == buyTypeID)
                    showtext = "<font color=\"red\">" + showtext + "</font>";
                string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID="+buyTypeParentID.ToString().Trim()+"&amp;buyType=" + types[i].ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ";
                shownavi += str + "|";
                if (i == 4 || i==9)
                    shownavi += "<br />";
            }
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        string sumname = (obuytype.NumsCount > 1) ? "分布图型：" : "走势图型：";
        ShowReportData(obuytype, count, sumname, " 01 02 03 04 05 06 07 08 09 10", 10);
    }
    private void ReportPage23() //大小单双分析页
    {
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0, buyTypeParentID = 0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        PK10_BuyType obuytype = _logic.GetBuyTypeByID(buyTypeID);
        if (obuytype == null || obuytype.ParentID != buyTypeParentID)
            buyTypeID = 0;
        #endregion
        #region 显示分析的类型
        builder.Append(Out.Tab("<div>", ""));
        string shownavi = "";
        List<PK10_BuyType> types = _logic.GetBuyTypes(buyTypeParentID);
        if (types != null && types.Count > 0)
        {
            if (buyTypeID == 0)
            {
                buyTypeID = types[0].ID;
                obuytype = _logic.GetBuyTypeByID(buyTypeID);
            }
            for (int i = 0; i < types.Count; i++)
            {
                string showtext = types[i].Descript.Trim();
                if (types[i].ID == buyTypeID)
                    showtext = "<font color=\"red\">" + showtext + "</font>";
                string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buyType=" + types[i].ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ";
                shownavi += str + "|";
                if (i == 4 || i == 10)
                    shownavi += "<br />";
            }
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        ShowReportData(obuytype, count, "大小单双：", "  大   小   单   双 ", 4);
    }
    private void ReportPage4() //龙虎分析页
    {
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0, buyTypeParentID = 0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        PK10_BuyType obuytype = _logic.GetBuyTypeByID(buyTypeID);
        if (obuytype == null || obuytype.ParentID != buyTypeParentID)
            buyTypeID = 0;
        #endregion
        #region 显示分析的类型
        builder.Append(Out.Tab("<div>", ""));
        string shownavi = "";
        List<PK10_BuyType> types = _logic.GetBuyTypes(buyTypeParentID);
        if (types != null && types.Count > 0)
        {
            if (buyTypeID == 0)
            {
                buyTypeID = types[0].ID;
                obuytype = _logic.GetBuyTypeByID(buyTypeID);
            }
            for (int i = 0; i < types.Count; i++)
            {
                string showtext = types[i].Descript.Trim();
                if (types[i].ID == buyTypeID)
                    showtext = "<font color=\"red\">" + showtext + "</font>";
                string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buyType=" + types[i].ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ";
                shownavi += str + "|";
                if (i == 2 || i == 9)
                    shownavi += "<br />";
            }
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        ShowReportData(obuytype, count, "龙虎分析：", "  龙   虎 ", 2);
    }
    private void ReportPage5() //任选分析页
    {
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0, buyTypeParentID = 0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        PK10_BuyType obuytype = _logic.GetBuyTypeByID(buyTypeID);
        if (obuytype == null || obuytype.ParentID != buyTypeParentID)
            buyTypeID = 0;
        #endregion
        #region 显示分析的类型
        builder.Append(Out.Tab("<div>", ""));
        string shownavi = "";
        List<PK10_BuyType> types = _logic.GetBuyTypes(buyTypeParentID);
        if (types != null && types.Count > 0)
        {
            if (buyTypeID == 0)
            {
                buyTypeID = types[0].ID;
                obuytype = _logic.GetBuyTypeByID(buyTypeID);
            }
            for (int i = 0; i < types.Count; i++)
            {
                string showtext = types[i].Descript.Trim();
                if (types[i].ID == buyTypeID)
                    showtext = "<font color=\"red\">" + showtext + "</font>";
                string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buyType=" + types[i].ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ";
                shownavi += str + "|";
                if (i == 2 )
                    shownavi += "<br />";
            }
        }
        builder.Append(shownavi);
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        ShowReportData(obuytype, count, "分布图型：", " 01 02 03 04 05 06 07 08 09 10", 10);
    }
    private void ReportPage6() //冠亚军分析页
    {
        #region 读取参数
        string cbuyType = Utils.GetRequest("buyType", "get", 1, "", "1");
        string cbuyTypeParentID = Utils.GetRequest("buyTypePID", "get", 1, "", "1");
        string cCount = Utils.GetRequest("count", "get", 1, "", "30");
        int buyTypeID = 0, buyTypeParentID = 0, count = 30;
        int.TryParse(cbuyType, out buyTypeID);
        int.TryParse(cCount, out count);
        int.TryParse(cbuyTypeParentID, out buyTypeParentID);
        switch (count)
        {
            case 30:
            case 50:
            case 100:
                break;
            default:
                count = 30;
                break;
        }
        PK10_BuyType obuytype = _logic.GetBuyTypeByID(buyTypeID);
        if (obuytype == null || obuytype.ParentID != buyTypeParentID)
            buyTypeID = 0;
        #endregion
        #region 显示分析的类型
        List<PK10_BuyType> types = _logic.GetBuyTypes(buyTypeParentID);
        if (types != null && types.Count > 0)
        {
            if (buyTypeID == 0)
            {
                buyTypeID = types[0].ID;
                obuytype = _logic.GetBuyTypeByID(buyTypeID);
            }
            //3中类型显示的分析是同一个，所以不显示子分类
            //builder.Append(Out.Tab("<div>", ""));
            //string shownavi = "";
            //for (int i = 0; i < types.Count; i++)
            //{
            //    string showtext = types[i].Descript.Trim();
            //    if (types[i].ID == buyTypeID)
            //        showtext = "<font color=\"red\">" + showtext + "</font>";
            //    string str = "<a href=\"" + Utils.getUrl(myFileName + "?act=report&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buyType=" + types[i].ID.ToString().Trim() + "&amp;count=" + count.ToString().Trim()) + "\">" + showtext + "</a> ";
            //    shownavi += str + "|";
            //    if (i == 3 )
            //        shownavi += "<br />";
            //}
            //builder.Append(shownavi);
            //builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        //builder.Append(Out.Tab("<div class=\"hr\" ></div> ", Out.Hr()));
        ShowReportData(obuytype, count, "冠亚分析：", "  大   小   单    双    和", 5);
    }
    private void ShowReportData(PK10_BuyType obuytype,int count,string sumname,string sumtext,int sumcount)
    {
        #region 显示分析的数据
        int pageindex = 0;//默认显示最后页（GetReportDatas函数中PageIndex=0，表示最后一页）
        int pagesize = GetPageSize();//每页显示的行数
        string cpageindex = Utils.GetRequest("pageindex", "get", 1, "", "1");
        int.TryParse(cpageindex, out pageindex);
        int recordCount = 0;
        int[][] sumList = new int[4][];
        List<PK10_Report> lists = _logic.GetReportDatas(obuytype, count, pagesize, pageindex, out recordCount, out sumList);
        string[] pageValUrl = { "act", "buytypePID", "buyType", "count", "backurl" };
        if (lists != null && lists.Count > 0)
        {
            int titleLength = 8;
            #region 显示汇总数
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(sumname);
            builder.Append("<font color=\"green\">" + sumtext + "</font>");
            builder.Append("<br />");
            for (int i = 0; i < 4; i++)
            {
                string tmpTitle = "";
                switch (i)
                {
                    case 0:
                        tmpTitle = "当前遗漏：";
                        break;
                    case 1:
                        tmpTitle = "最大遗漏：";
                        break;
                    case 2:
                        tmpTitle = "平均遗漏：";
                        break;
                    case 3:
                        tmpTitle = "出现次数：";
                        break;
                }
                builder.Append(tmpTitle.PadLeft(titleLength).Substring(0, titleLength));
                for (int j = 0; j < sumcount; j++)
                {
                    string cc = sumList[i][j].ToString().Trim();
                    if (cc.Length < 2)
                        cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                    builder.Append(" " + cc);
                }
                if (i < 3)
                    builder.Append("<br />");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            #endregion
            builder.Append(Out.Tab("<div class=\"hr\" ></div> ", ""));
            #region 显示数字
            int k = 1;
            foreach (PK10_Report item in lists)
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
                #region 生成显示的格式
                builder.Append(Out.Tab("第" + item.No.Trim() + "期", item.No.Trim() + "期"));
                builder.Append(item.ShowNums);
                #endregion
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            #endregion
            // 分页
            builder.Append(BasePage.MultiPage(pageindex, pagesize, recordCount, Utils.getPageUrl(), pageValUrl, "pageindex", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        #endregion
    }
    #endregion

    #region 其他函数
    private int GetPageSize()
    {
        int mypagesize = 0;
        int.TryParse(ub.GetSub("ShowDataPageSize", xmlPath), out mypagesize);
        if (mypagesize <= 0)
            mypagesize = myPageSize;
        return mypagesize;
    }
    private int GetDefaultTestMoney()
    {
        int money = 0;
        int.TryParse(ub.GetSub("TestUserDefaultMoney", xmlPath), out money);
        if (money <= 0)
            money = defaultTestMoney;
        return money;
    }
    private void ShowUser() //显示用户信息，用户未登录则转登录页面
    {
        int meid = GetUserID();
        long gold = _logic.GetUserGold(meid, isTest);
        builder.Append(Out.Tab("<div>", ""));
        if(isTest)
            builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + _logic.GetGoldName(isTest) + "<a href=\"" + Utils.getUrl("/bbs/finance.aspx?act=vippay&amp;backurl=" + Utils.PostPage(1) + "") + "\"> 充值</a>");
        else
            builder.Append("您目前共有" + Utils.ConvertGold(gold) + "" + _logic.GetGoldName(isTest) + "");
        builder.Append(Out.Tab("</div>", ""));
    }
    private int GetUserID()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        myUserID = meid;
        if (meid > 0 && isTest)
            if (!_logic.CreateTestUser(meid, GetDefaultTestMoney()))
                Utils.Error("创建测试用户失败！","");
        return meid;
    }
    private void ShowGameTop(string navText) //显示游戏头部
    {
        #region 显示顶部导航
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append("&gt; " + " <a href =\"" + Utils.getUrl(myFileName) + "\">" + GameName + "</a>");
        if (!string.IsNullOrEmpty(navText))
        {
            builder.Append(navText);
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion
        #region 显示游戏信息
        string SaleMsg = ub.GetSub("SaleMsg", xmlPath);
        if (SaleMsg != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.SysUBB(SaleMsg) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        #region 显示Logo图片
        string Logo = ub.GetSub("LogoImage", xmlPath);
        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
        #region 显示注意事项
        string Notes = ub.GetSub("Notes", xmlPath);
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(Out.SysUBB(Notes) + "");
            builder.Append(Out.Tab("</div>", ""));
        }
        #endregion
    }
    private void ShowGameBottom(string navText)//显示游戏底部
    {
        #region 显示闲聊、游戏底部导航
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(32, myFileName, 5, 0)));
        //
        #region 显示动态信息
        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("【最新动态】");
        builder.Append(Out.Tab("</div>", ""));
        int SizeNum = 3;
        string strWhere = "";
        strWhere = "Types="+_logic.myPublishGameType.ToString();
        IList<BCW.Model.Action> listAction = new BCW.BLL.Action().GetActions(SizeNum, strWhere);
        if (listAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Action n in listAction)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                string ForNotes = Regex.Replace(n.Notes, @"\[url=\/bbs\/game\/([\s\S]+?)\]([\s\S]+?)\[\/url\]", "$2", RegexOptions.IgnoreCase);
                ForNotes = ForNotes.Replace("PK拾", "");
                builder.AppendFormat("{0}前{1}", DT.DateDiff2(DateTime.Now, n.AddTime), Out.SysUBB(ForNotes));
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            if (k > SizeNum)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/action.aspx?ptype="+ _logic.myPublishGameType.ToString().Trim()+ "&amp;backurl=" + Utils.PostPage(1) + "") + "\">更多动态&gt;&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion
        //
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append("&gt; " + " <a href =\"" + Utils.getUrl(myFileName) + "\">" + GameName + "</a>");
        if(!string.IsNullOrEmpty(navText))
        {
            builder.Append(navText);
        }
        builder.Append(Out.Tab("</div>", ""));
        #endregion
    }
    private void ShowSaleDatas(int initRecordCount,int Step,PK10_BuyType obuytype)//显示开售信息
    {
        int recentCount = initRecordCount;
        string cRecentCount = Utils.GetRequest("RecentCount", "all", 1, "", "");
        if (cRecentCount != "")
            int.TryParse(cRecentCount, out recentCount);
        if (recentCount < initRecordCount)
            recentCount = initRecordCount;
        List<PK10_List> recentData = _logic.GetRecentData(recentCount);//最近三期数据
        if (recentData.Count == 0)
        {
            isInit = false;
        }
        else
        {
             builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i < recentData.Count; i++)
            {
                string showStr = "";
                string showTime = "";
                #region 显示数据
                PK10_Stutas status = _logic.GetListStatus(recentData[i]);
                switch (status)
                {
                    case PK10_Stutas.在售:
                        //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                        //    showTime = new BCW.JS.somejs().daojishi2("OnSaleTimer1", recentData[i].EndTime);
                        //else
                        //    showTime = new BCW.JS.somejs().daojishi("OnSaleTimer1", recentData[i].EndTime);
                        showTime= new BCW.JS.somejs().newDaojishi("OnSaleTimer1", recentData[i].EndTime);
                        showStr = "第" + recentData[i].No.Trim() + "期[在售].还有 " + showTime;
                        //showStr += " |<a href=\"" + Utils.GetRequest("", "all", 1, "", "") + "\">刷新</a>";
                        showStr += " |<a href=\"" + GetPage("","",false) + "\">刷新</a>";
                        myCurrentSaleID = recentData[i].ID;
                        isSale = true;
                        break;
                    case PK10_Stutas.待售:
                        //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                        //    showTime = new BCW.JS.somejs().daojishi2("OnSaleTimer", recentData[i].BeginTime);
                        //else
                        //    showTime = new BCW.JS.somejs().daojishi("OnSaleTimer", recentData[i].BeginTime);
                        showTime = new BCW.JS.somejs().newDaojishi("OnSaleTimer1", recentData[i].BeginTime);
                        showStr = "第" + recentData[i].No.Trim() + "期[待售].还有" + showTime;
                        break;
                    case PK10_Stutas.待开奖:
                        showStr = "第" + recentData[i].No.Trim() + "期[待开奖]...";
                        break;
                    case PK10_Stutas.已开奖:
                        string str = recentData[i].No.Trim();
                        showStr = str.Substring(str.Length - 3, 3) + "期";
                        #region 特别显示
                        string[] anums = recentData[i].Nums.Split(',');
                        int value = 0;
                        if (obuytype!=null)
                        {
                            #region
                            switch(obuytype.ParentID)
                            {
                                case 1: //号码玩法
                                    #region 
                                    for (int j = 0; j < 10; j++)
                                    {
                                        string cnum = anums[j].Trim();
                                        if (j == obuytype.NumID)
                                            showStr = showStr.Trim() + "[<font color=\"red\">";
                                        showStr += cnum + " ";
                                        if (j == obuytype.NumID + obuytype.NumsCount - 1)
                                            showStr = showStr.Trim() + "</font>]";
                                    }
                                    #endregion
                                    break;
                                case 3: //单双玩法
                                case 2: //大小玩法
                                    #region 
                                    for (int j = 0; j < 10; j++)
                                    {

                                        if (j == obuytype.NumID)
                                        {
                                            showStr += "[<font color=\"red\">" + anums[j].Trim() + "</font>]";
                                        }
                                        else
                                            showStr += anums[j].Trim();
                                    }
                                    value = int.Parse(anums[obuytype.NumID]);
                                    if (value > 5)
                                        showStr +="<font color =\"red\">" +"大" + "</font>";
                                    else
                                        showStr += "<font color =\"red\">"+ "小" + "</font>";
                                    if (value %2==0)
                                        showStr += "<font color =\"red\">" + "双" + "</font>";
                                    else
                                        showStr += "<font color =\"red\">" + "单" + "</font>";
                                    #endregion
                                    break;
                                case 4: //龙虎玩法
                                    #region 
                                    for (int j = 0; j < 10; j++)
                                    {

                                        if (j == obuytype.NumID || j==(9-obuytype.NumID))
                                        {
                                            showStr += "[<font color=\"red\">" + anums[j].Trim() + "</font>]";
                                        }
                                        else
                                            showStr += anums[j].Trim();
                                    }
                                    int value1 = int.Parse(anums[obuytype.NumID]);
                                    int value2= int.Parse(anums[9-obuytype.NumID]);
                                    if (value1 > value2)
                                        showStr += "<font color =\"red\">" + "龙" + "</font>";
                                    else
                                        showStr += "<font color =\"red\">" + "虎" + "</font>";
                                    #endregion
                                    break;
                                case 5: //任选玩法
                                    #region
                                    for (int j = 0; j < 10; j++)
                                    {
                                        if (j < 5)
                                        {
                                            if (j == 0)
                                                showStr += "[<font color=\"red\">";
                                            showStr += anums[j].Trim()+" "; 
                                            if (j == 4)
                                                showStr =showStr.Trim()+"</font>]";
                                        }
                                        else
                                            showStr += anums[j].Trim();
                                    }
                                    #endregion
                                    break;
                                case 6: //冠亚军玩法
                                    #region 
                                    for (int j = 0; j < 10; j++)
                                    {
                                        if (j == 0 || j == 1)
                                        {
                                            if (j == 0)
                                                showStr += "[<font color=\"red\">" + anums[j].Trim()+" " + "</font>";
                                            else
                                                showStr += "<font color=\"red\">" + anums[j].Trim() + "</font>]";
                                        }
                                        else
                                            showStr += anums[j].Trim();

                                    }
                                    value = int.Parse(anums[0]) + int.Parse(anums[1]);

                                    if (value == 11)
                                        showStr += " 和 ";
                                    else
                                    {
                                        if (value > 11)
                                            showStr += "<font color =\"red\">" + "大" + "</font>";
                                        else
                                            showStr += "<font color =\"red\">" + "小" + "</font>";
                                        if (value % 2 == 0)
                                            showStr += "<font color =\"red\">" + "双" + "</font>";
                                        else
                                            showStr += "<font color =\"red\">" + "单" + "</font>";
                                    }
                                    showStr += value.ToString().Trim();
                                    #endregion
                                    break;
                                default:
                                    showStr += "[";
                                    for(int j=0;j<10;j++)
                                    {
                                        string cnum = anums[j].Trim();
                                        showStr += cnum + " ";
                                    }
                                    showStr=showStr.Trim()+ "]";
                                    break;
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            showStr += "[";
                            for (int j = 0; j < 10; j++)
                            {
                                string cnum = anums[j].Trim();
                                showStr += cnum + " ";
                            }
                            showStr = showStr.Trim() + "]";
                            #endregion
                        }
                        #endregion
                        break;
                    default:
                        break;
                }
                #endregion
                builder.Append(showStr);
                if(i<recentData.Count-1)
                    builder.Append("<br/>");
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + GetPage("RecentCount", (recentCount + Step).ToString().Trim(),false) + "\">" + "展开︾" + "</a>  ");
            if (recentCount > initRecordCount)
                builder.Append("<a href=\"" + GetPage("RecentCount", (recentCount - Step).ToString().Trim(),false) + "\">" + "叠起︽" + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    private PK10_Stutas GetAndShowSaleDataSatus(PK10_List list)
    {
        PK10_Stutas status=new PK10_Stutas();
        builder.Append(Out.Tab("<div>", ""));
        if (list == null)
        {
            builder.Append("没有这期号码！");
        }
        else
        {
            string showStr = "";
            string showTime = "";
            #region 显示数据
            status = _logic.GetListStatus(list);
            switch (status)
            {
                case PK10_Stutas.在售:
                    //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    //    showTime = new BCW.JS.somejs().daojishi2("OnSaleTimer1", list.EndTime);
                    //else
                    //    showTime = new BCW.JS.somejs().daojishi("OnSaleTimer1", list.EndTime);
                    showTime = new BCW.JS.somejs().newDaojishi("OnSaleTimer1", list.EndTime);
                    showStr = "第" + list.No.Trim() + "期[在售].还有 "+showTime;
                    myCurrentSaleID = list.ID;
                    isSale = true;
                    break;
                case PK10_Stutas.待售:
                    //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                    //    showTime = new BCW.JS.somejs().daojishi2("OnSaleTimer", list.BeginTime);
                    //else
                    //    showTime = new BCW.JS.somejs().daojishi("OnSaleTimer", list.BeginTime);
                    showTime = new BCW.JS.somejs().newDaojishi("OnSaleTimer1", list.BeginTime);
                    showStr = "第" + list.No.Trim() + "期[待售].还有" + showTime;
                    break;
                case PK10_Stutas.待开奖:
                    showStr = "第" + list.No.Trim() + "期[待开奖]";
                    break;
                case PK10_Stutas.已开奖:
                    showStr = "第" + list.No.Trim() + "期[已开奖]";
                    break;
                default:
                    break;
            }
            #endregion
            builder.Append(showStr);
            builder.Append("<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        return status;
    }
    private void ShowGameNavi(int menuNo)//显示游戏导航
    {
        string[] aNavi = new string[6];
        aNavi[0] = "<a href=\"" + Utils.getUrl(myFileName) + "\">" + "首页" + "</a>";
        aNavi[1] = "<a href=\"" + Utils.getUrl(myFileName + "?act=rule") + "\">" + "规则" + "</a>";
        aNavi[2] = "<a href=\"" + Utils.getUrl(myFileName + "?act=case") + "\">" + "兑奖" + "</a>";
        aNavi[3] = "<a href=\"" + Utils.getUrl(myFileName + "?act=list") + "\">" + "历史" + "</a>";
        aNavi[4] = "<a href=\"" + Utils.getUrl(myFileName + "?act=top") + "\">" + "排行" + "</a>";
        aNavi[5] = "<a href=\"" + Utils.getUrl(myFileName + "?act=report") + "\">" + "分析" + "</a>";
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        string str = "";
        for (int i=0;i<aNavi.Length;i++)
        {
            string tmpstr = "";
            if(i==menuNo)
            {
                tmpstr = "<font color=\"red\">"+Common.GetValueFromStr(aNavi[i],"\">","</a>")+ "</font>";
            }
            else
            {
                tmpstr = aNavi[i];
            }
            if (str == "")
                str = tmpstr;
            else
                str += "|" + tmpstr;
        }
        builder.Append(str);
        builder.Append(Out.Tab("</div>", ""));
    }
    private string GetPaySettings(int usID,int buyTypeParentID,int buyTypeID,string select)
    {
        string showStr = "";
        string settings = _logic.GetSettings(usID);
        if (string.IsNullOrEmpty(settings))
        {
            settings = ub.GetSub("defaultPaySettings", xmlPath);
        }
        if(!string.IsNullOrEmpty(settings))
        {
            string[] csettings = settings.Split('#');
            if (csettings.Length > 0)
            {
                for (int i = 0; i < csettings.Length; i++)
                {
                    string[] vs = csettings[i].Split('|');
                    if (vs.Length == 2)
                    {
                        int v = 0;
                        string vn = vs[0].ToString().Trim();
                        int.TryParse(vs[1].ToString(), out v);
                        showStr += "<a href=\"" + Utils.getUrl(myFileName + "?act=buy&amp;buytypePID=" + buyTypeParentID.ToString().Trim() + "&amp;buytype=" + buyTypeID.ToString().Trim() + "&amp;select=" + select.ToString().Trim()) + "&amp;price="+ v+ "\">" + vn + "</a> ";
                    }
                }
            }
        }
        //
        return showStr;
    } 
    private List<string> CheckAndGetNewSelect(string cSelects,PK10_BuyType oBuyType)//过滤下注的字符串，并返回
    {     
        List<string> resultSelects = new List<string>();
        #region 生成是否合法的字符串
        int maxNumCount = oBuyType.NumsCount; //当前购买类型号码的最大位数
        bool canMultiSelect = (oBuyType.MultiSelect == 1) ? true : false;//当前购买类型是否可以复式
        string newSelects = "";
        string[] aNums = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
        int nline = 0,num = 0;
        string[] list=(cSelects.Split('|'));//已经选择的号码,例如（第一位选择02、03，第二位选择03、05，则是Select=1,02|1,03|2,03|2,05）
        //
        if (list.Length>0)
        {
            string[] aSelects = new string[10];
            for (int i = 0; i < 10; i++)
                aSelects[i] = "";
            #region 生成合符规则的选号到aSelects
            for (int i=0;i< list.Length; i++)
            {
                nline = 0;
                num = 0;
                string[] aa = list[i].Split(',');//已经选择了的号码的列表,例如(1,02表示第1位选择了02这个号码）
                if (aa.Length>=2)
                {
                    int.TryParse(aa[0], out nline);
                    int.TryParse(aa[1], out num);
                    //
                    if(nline > 0 && nline <= maxNumCount && num>0 && num<=10) //（过滤掉非法的）最大10个位置，每个位置是01-10的号码
                    {
                        newSelects =aNums[num - 1].Trim();
                        if(canMultiSelect)
                        {
                            if (!aSelects[nline-1].Contains(newSelects))
                                aSelects[nline-1] += newSelects+",";
                        }
                        else
                        {
                            bool hasSelect = false;
                            for (int j = 0; j < 10; j++)
                                if (aSelects[j].Contains(newSelects))
                                    hasSelect = true;
                            if (!hasSelect) //所有位都没有选过这个号码
                                aSelects[nline - 1] = newSelects+","; //单选，直接用新的代替
                            else
                                aSelects[nline - 1] = "";
                        }
                    }
                    //
                }
            }
            #endregion
            #region 重新生成原来的格式，以供返回(每行只记录一个第N位某一号码，如1,03）
            for (int i=0;i<10;i++)
            {
                if (aSelects[i] != null)
                {
                    num = aSelects[i].Length / 3; //aSelects中某一行的内容格式，例如02,03,04，表示选择了02、03、04
                    if (num > 0)
                    {
                        for (int j = 0; j < num; j++)
                        {
                            newSelects = (i + 1).ToString().Trim() + "," + aSelects[i].Substring(j * 3, 2);
                            resultSelects.Add(newSelects);
                        }
                    }
                }
            }
            #endregion
        }
        #endregion
        return resultSelects;
    }
    private List<string> CreateRandomNums(Random rm, int resultLength, string strExclude) //机选一注(直选)
    {
        List<string> aSelects = new List<string>();
        List<string> list = new List<string>();
        //
        string strSource = "01,02,03,04,05,06,07,08,09,10";
        #region 建立查询列表
        if (strSource.Length > 0)
        {
            string[] astr = strSource.Split(',');
            if (astr.Length > 0)
            {
                for (int i = 0; i < astr.Length; i++)
                    list.Add(astr[i]);
            }
        }
        #endregion
        #region 排除字符
        if (strExclude.Length > 0)
        {
            string[] astr1 = strExclude.Split(',');
            if (astr1.Length > 0)
            {
                for (int i = 0; i < astr1.Length; i++)
                {
                    if (list.Contains(astr1[i]))
                        list.Remove(astr1[i]);
                }
            }
        }
        #endregion
        #region 生成随机不重复组合
        int length = list.Count;
        if (length > 0 && length >= resultLength)
        {
            List<string> list1 = new List<string>();
            #region 循环
            if (rm == null)
                rm = new Random();
            for (int i = 0; list1.Count < resultLength; i++)
            {
                int nIndex = rm.Next(list.Count);
                string cValue = list[nIndex];
                if (!list1.Contains(cValue))
                {
                    list1.Add(cValue);
                    list.Remove(cValue);
                }
            }
            #endregion
            #region 将结果List1转为“，”分割的字符串
            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    string str = (i + 1).ToString().Trim() + "," + list1[i].Trim();
                    aSelects.Add(str);
                }
            }
            #endregion
        }
        #endregion
        //
        return aSelects;
    }
    public string GenNumsFromSelectStr(List<string> aSelects,int numsCount)//
    {
        string resultNums = "";
        //aSelects(每行只记录一个第N位某一号码，如1,03）
        string[] lists = new string[numsCount];
        #region 初始化lists
        for (int i=0;i<numsCount;i++)
        {
            lists[i] = "";
        }
        #endregion
        #region 把aSelects中选择的号码填入lists
        for(int i=0;i<aSelects.Count;i++)
        {
            string[] cnums = aSelects[i].Split(',');
            int line = int.Parse(cnums[0]);
            string cnum = cnums[1].Trim();
            if (line > 0 && line <= numsCount)
            {
                if (!lists[line - 1].Contains(cnum))
                    lists[line - 1] += cnum+",";
            }
        }
        #endregion
        #region 从lists生成与数据库保存格式相同的下注组合
        for(int i=0;i<numsCount;i++)
        {
            string cnums = "";
            int ncount = lists[i].Length / 3; //aSelects中某一行的内容格式，例如020304，表示选择了02、03、04
            if (ncount > 0)
            {
                for (int j = 0; j < ncount; j++)
                {
                    string num = lists[i].Substring(j * 3, 2);
                    if (j == 0)
                        cnums = num;
                    else
                        cnums += "," + num;
                }
            }
            if (i == 0)
                resultNums = cnums;
            else
                resultNums += "|" + cnums;
        }
        #endregion
        //
        return resultNums;
    }
    private string GenNumsFromSelectStr2(List<string> aSelects, int numsCount)//(任选)
    {
        string resultNums = "";
        //aSelects(如03）
        if (aSelects.Count > 0)
        {
            for (int i = 0; i < aSelects.Count; i++)
            {
                if (i == 0)
                    resultNums = aSelects[i].ToString().Trim();
                else
                    resultNums += "|" + aSelects[i].ToString().Trim();
            }
        }
        //
        return resultNums;
    }
    private List<string> CheckAndGetNewSelect2(string[] aNums,string cSelects, PK10_BuyType oBuyType)//过滤下注的字符串(任选/和值)
    {
        List<string> resultSelects = new List<string>();
        #region 生成是否合法的字符串
        int maxNumCount = oBuyType.NumsCount; //当前购买类型号码的最大位数
        bool canMultiSelect = (oBuyType.MultiSelect == 1) ? true : false;//当前购买类型是否可以复式
        if(aNums==null)
            aNums = new string[] {"01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
        string[] list = (cSelects.Split('|'));//已经选择的号码,例如任选四，选择02、03、04、05，则是Select=02|03|04|05）
        //
        if (list.Length > 0)
        {
            //
            List<string> newlists = new List<string>();
            for (int i = 0; i < list.Length; i++)
            {
                string num = list[i].ToString().Trim();
                if (!newlists.Contains(num))
                    newlists.Add(num);
            }
            //
            for(int i=0;i<aNums.Length;i++)
            {
                string num = aNums[i].ToString().Trim();
                if(newlists.Contains(num))
                {
                    if (!resultSelects.Contains(num))
                        resultSelects.Add(num);
                }
            }
        }
        #endregion
        return resultSelects;
    }
    private List<string> CreateRandomNums2(string[] aNums, Random rm, int resultLength, string strExclude) //机选一注(任选）
    {
        List<string> aSelects = new List<string>();
        List<string> list = new List<string>();
        //
        if (aNums == null)
            aNums = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
        #region 建立查询列表
        if (aNums.Length > 0)
        {
                for (int i = 0; i < aNums.Length; i++)
                    list.Add(aNums[i]);
        }
        #endregion
        #region 排除字符
        if (strExclude.Length > 0)
        {
            string[] astr1 = strExclude.Split(',');
            if (astr1.Length > 0)
            {
                for (int i = 0; i < astr1.Length; i++)
                {
                    if (list.Contains(astr1[i]))
                        list.Remove(astr1[i]);
                }
            }
        }
        #endregion
        #region 生成随机不重复组合
        int length = list.Count;
        if (length > 0 && length >= resultLength)
        {
            List<string> list1 = new List<string>();
            #region 循环
            if (rm == null)
                rm = new Random();
            for (int i = 0; list1.Count < resultLength; i++)
            {
                int nIndex = rm.Next(list.Count);
                string cValue = list[nIndex];
                if (!list1.Contains(cValue))
                {
                    list1.Add(cValue);
                    list.Remove(cValue);
                }
            }
            #endregion
            #region 将结果List1转为字符串
            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i++)
                {
                    string str =list1[i].Trim();
                    aSelects.Add(str);
                }
            }
            #endregion
        }
        #endregion
        //
        return aSelects;
    }
    private string GetBuyType23Name(int buyTypeParentID,int select) //
    {
        string name = "";
        if(select==0)
        {
            switch(buyTypeParentID)
            {
                case 2:
                    name = "买小";
                    break;
                case 3:
                    name = "买单";
                    break;
            }
        }
        else
        {
            switch (buyTypeParentID)
            {
                case 2:
                    name = "买大";
                    break;
                case 3:
                    name = "买双";
                    break;
            }
        }
        return name;
    }
        
    #endregion
}