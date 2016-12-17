using System;
using System.Collections.Generic;
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
using System.Text.RegularExpressions;

/// <summary>
/// 更新红包群 戴少宇 20160127
/// </summary>
/// <summary>
/// 蒙宗将 20160822 撤掉抽奖值生成
/// </summary>
/// 
public partial class bbs_chat : System.Web.UI.Page
{

    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/chat.xml";
    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {        
        //维护提示
        if (ub.GetSub("ChatStatus", xmlPath) == "1")
        {
            Utils.Safe("红包群系统");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "top":
                TopPage();
                break;
            case "hbtop":
                HbTopPage();
                break;
            case "chatadmin":
                ChatAdminPage();
                break;
            case "help":
                ChatHelpPage();
                break;
            case "online":
                OnLinePage();
                break;
            case "exit":
                ExitPage();
                break;
            case "deltxl":
                DelTxl();
                break;
            case "member":
                Member();
                break;
            //case "checkdel":
            //    CheckDelPage();
            //    break;
            //case "checkout":
            //    CheckOutPage();
            //    break;
            //case "checkstop":
            //    CheckStopPage();
            //    break;
            case "checkadmin":
                CheckAdminPage();
                break;
            case "fund":
                FundPage();
                break;
            case "fundlist":
                FundListPage();
                break;
            case "fundpay":
                FundPayPage();
                break;
            case "fundget":
                FundGetPage();
                break;
            case "fundpwd":
                FundPwdPage();
                break;
            case "admin":
                AdminPage();
                break;
            case "admlist":
                AdminListPage();
                break;
            case "admlog":
                AdminLogPage();
                break;
            case "admblack":
                AdminBlackPage();
                break;
            case "admblackdel":
                AdminBlackDelPage();
                break;
            case "clear":
                ClearPage();
                break;
            case "edit":
                ChatEditPage();
                break;
            case "editpwd":
                ChatEditPwdPage();
                break;
            case "editpay":
                ChatEditPayPage();
                break;
            case "clearlog":
                ClearLogPage();
                break;
            case "admset":
                ChatAdminSetPage();
                break;
            case "admview":
                ChatAdminViewPage();
                break;
            case "cbset":
                ChatCbSetPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = ub.GetSub("ChatName", xmlPath);
        //取用户ID
        int meid = new BCW.User.Users().GetUsId();
        #region 是否超管
        string ChatHID = ub.GetSub("ChatHID", xmlPath);
        string[] sNum = Regex.Split(ChatHID, "#");
        int ISH = 0;
        for (int a = 0; a < sNum.Length - 1; a++)
        {
            if (new BCW.User.Users().GetUsId() == int.Parse(sNum[a].Trim()))
            {
                ISH++;
            }
        }
        #endregion
        if (ub.GetSub("ChatLogo", xmlPath) != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + ub.GetSub("ChatLogo", xmlPath) + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "-1"));
        string keyword = Utils.GetRequest("keyword", "all", 1, "", "");
      
        string ListTitle = string.Empty;
        if (ISH != 0)
        {
            if (ptype == -1)
            {
                ListTitle = "全部红包群";
                builder.Append("全部 ");
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">全部</a> ");
        }
        else
        {
            if (ptype == -1)
            {
                ListTitle = "热门红包群";
                builder.Append("热门 ");
            }
            else
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">热门</a> ");
        }

        if (ptype == 0)
        {
            ListTitle = "主题红包群";
            builder.Append("主题 ");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=0") + "\">主题</a> ");
        if (ptype == 2)
        {
            ListTitle = "同城红包群";
            builder.Append("同城 ");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=2") + "\">同城</a> ");

        if (ptype == 3)
        {
            ListTitle = "自建红包群";
            builder.Append("自建 ");
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=3") + "\">自建</a> ");
        if (ptype == 5)
        {
            ListTitle = "我的私人红包群";
            builder.Append("私人 ");
        }
        else
           builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=5") + "\">私人</a> ");


        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=add") + "\">我要建群</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top") + "\">聊天排行</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hbtop") + "\">红包排行</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (!string.IsNullOrEmpty(keyword))
            builder.Append("搜索红包群");
        else
            builder.Append(ListTitle);

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (ISH != 0)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                if (ptype == 5)
                {
                    strWhere = "ChatName LIKE '%" + keyword + "%'  and IShbchat=1 and UsID=" + meid;
                }
                else
                {
                    strWhere = "ChatName LIKE '%" + keyword + "%'  ";
                }
            }
            else
            {
                if (ptype != -1 && ptype != 5)
                    strWhere = "Types=" + ptype + " and IShbchat!=1";
                else if (ptype == -1)
                {
                    strWhere = "";
                }
                else if (ptype == 5)
                {
                    strWhere = "IShbchat=1 and UsID=" + meid;
                }

                //排序条件
                strOrder = "ChatOnLine Desc";
            }
        }
        else
        {
            //查询条件
            if (!string.IsNullOrEmpty(keyword))
            {
                if (ptype == 5)
                {
                    strWhere = "ChatName LIKE '%" + keyword + "%'  and IShbchat=1 and UsID=" + meid;
                }
                else
                {
                    strWhere = "ChatName LIKE '%" + keyword + "%'  and IShbchat!=1";
                }
            }
            else
            {
                if (ptype != -1 && ptype != 5)
                    strWhere = "Types=" + ptype + " and IShbchat!=1";
                else if (ptype == -1)
                {
                    strWhere = "IShbchat!=1";
                }
                else if (ptype == 5)
                {
                    strWhere = "IShbchat=1 and UsID=" + meid;
                }

                //排序条件
                strOrder = "ChatOnLine Desc";
            }
        }
       
        // 开始读取列表
        IList<BCW.Model.Chat> listChat = new BCW.BLL.Chat().GetChats(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listChat.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Chat n in listChat)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?id={0}" + "&amp;hb=1") + "\">{1}({2})</a>", n.ID, n.ChatName, n.ChatOnLine);
                //删除过期的红包群
                if (n.Types == 3 && n.ExTime < DateTime.Now)
                {
                    new BCW.BLL.Chat().Delete(n.ID);
                    new BCW.BLL.ChatText().DeleteStr(n.ID);
                    new BCW.HB.BLL.ChatMe().Delete2(n.ID);
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        if (ptype == 5&&meid!=0)
        {
            List<BCW.HB.Model.ChatMe> mylist = new BCW.HB.BLL.ChatMe().GetModelList("UserID="+meid);
            int sbb = 1;
            for (int iso = 0; iso < mylist.Count; iso++)
            {
                if (sbb % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                else
                {
                    if (sbb == 1)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                BCW.Model.Chat nm = new BCW.BLL.Chat().GetChat(mylist[iso].ChatID);
                builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?id={0}" + "&amp;hb=1") + "\">{1}({2})</a>", nm.ID, nm.ChatName, nm.ChatOnLine);
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=deltxl&amp;id=" + nm.ID + "") + "\">[退出]</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        builder.Append(Out.Tab("", "<br />"));
        strText = ",,,";
        strName = "ptype,keyword,act";
        strType = "hidden,stext,hidden";
        strValu = ptype+"''search";
        strEmpt = "true,true,false";
        strIdea = "";
        strOthe = "搜红包群,chat.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
     

        if (meid == 0)
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("请您先<a href=\"" + Utils.getUrl("/login.aspx?backurl=" + Utils.PostPage(1) + "") + "\">登录/注册</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=help") + "\">红包群帮助</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=chatadmin") + "\">聊管</a>");
        
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("当前聊天在线" + new BCW.BLL.User().GetChatNum() + "人.");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddPage()
    {
        if (ub.GetSub("ChatIsAdd", xmlPath) == "1")
        {
            Utils.Error("系统暂时不开放自建红包群功能..", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int leven = new BCW.BLL.User().GetLeven(meid);
        int AddLeven = Convert.ToInt32(ub.GetSub("ChatAddLeven", xmlPath));
        if (leven < AddLeven)
        {
            Utils.Error("等级不够" + AddLeven + "级，不能创建红包群", "");
        }
        int Count = new BCW.BLL.Chat().Exists4(meid);
        if(Count>=5)
        {
            Utils.Error("每位会员只能创建5个红包群", "");
        }
        //if (new BCW.BLL.Chat().Exists3(meid))
        //{
        //    Utils.Error("每位会员只能创建一个红包群", "");
        //}
  
        Master.Title = "自建红包群";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("创建自己的红包群");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "红包群名称:/,红包群主题:/,红包群时长/,类型/,";
        string strName = "ChatName,ChatNotes,Hour,Type,act";
        string strType = "text,text,hidden,select,hidden";
        string strValu = "''8'1'addsave";
        string strEmpt = "false,true,false,1|公共|2|私人,false";
        string strIdea = "/";
        string strOthe = "确定创建,chat.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("注：红包群30天无人发言自动解散！");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=help") + "\">创建帮助</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AddSavePage()
    {
        if (ub.GetSub("ChatIsAdd", xmlPath) == "1")
        {
            Utils.Error("系统暂时不开放自建红包群功能..", "");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.User.Users.ShowVerifyRole("j", meid);//非验证会员提示
        int leven = new BCW.BLL.User().GetLeven(meid);
        int AddLeven = Convert.ToInt32(ub.GetSub("ChatAddLeven", xmlPath));
        if (leven < AddLeven)
        {
            Utils.Error("等级不够" + AddLeven + "级，不能创建红包群", "");
        }
        int Count = new BCW.BLL.Chat().Exists4(meid);
        if (Count >= 5)
        {
            Utils.Error("每位会员只能创建5个红包群", "");
        }
        //if (new BCW.BLL.Chat().Exists3(meid))
        //{
        //    Utils.Error("每位会员只能创建一个红包群", "");
        //}
        string ChatName = Utils.GetRequest("ChatName", "post", 2,@"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "红包群名称限1-10字，不能使用特殊字符");
        string ChatNotes = Utils.GetRequest("ChatNotes", "post", 3,@"^[^\^]{1,200}$", "红包群主题限500字内，可留空");
        int Hour = int.Parse(Utils.GetRequest("Hour", "post", 2,@"^[0-8]$", "选择时长错误"));
        int Type = int.Parse(Utils.GetRequest("Type", "post", 2,@"^[1-2]$", "选择类型错误"));
        DateTime ExTime = DateTime.Now;
        int bCent = Convert.ToInt32(ub.GetSub("ChatAddPrice", xmlPath));
        int Cent = 0;
        int iCent = Convert.ToInt32(ub.GetSub("ChatiPrice", xmlPath));//每小时多少币
        Cent =0;
        ExTime = ExTime.AddDays(30);
        //减回2小时币数
        long payCent = Convert.ToInt64((bCent + Cent) - (iCent * 2));
        payCent = 0;
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info != "ok")
        {
            Master.Title = "自建红包群";
            builder.Append(Out.Tab("<div class=\"title\">自建红包群</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("红包群30天内无人发言将自动解散！");
            builder.Append(Out.Tab("</div>", "<br />"));

            strName = "ChatName,ChatNotes,Hour,Type,act,info";
            strValu = "" + ChatName + "'" + ChatNotes + "'" + Hour + "'" + Type + "'addsave'ok";
            strOthe = "确认创建,chat.aspx,post,0,red";
            builder.Append(Out.wapform(strName, strValu, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            BCW.Model.Chat model = new BCW.Model.Chat();
            model.ChatNotes = ChatNotes;
            model.ChatName =ChatName;
            model.ChatSZ = "";
            model.ChatJS = "";
            model.ChatLG = "";
            model.ChatFoot = "";
            
            model.Types = 3;
            model.UsID = meid;
            model.GroupId = 0;
            model.Paixu = 0;
            model.AddTime = DateTime.Now;
            model.ExTime = ExTime;
            int id = new BCW.BLL.Chat().Add(model);



            if (Type == 2)
            {
                new BCW.HB.BLL.HbPost().UpdateChatIsHb(id, 1);
            }
            else
            {
                new BCW.HB.BLL.HbPost().UpdateChatIsHb(id, 0);
            }
            Utils.Success("创建红包红包群", "创建派红包红包群成功..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + ""), "1");
        }
    }

    private void TopPage()
    {
        Master.Title = "红包群排行榜";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        int showtype = int.Parse(Utils.GetRequest("showtype", "get", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("排行: ");
        if (ptype == 0)
            builder.Append("主题 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=0") + "\">主题</a> ");

        //if (ptype == 1)
        //    builder.Append("圈聊 ");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=1") + "\">圈聊</a> ");

        if (ptype == 2)
            builder.Append("同城 ");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=2") + "\">同城</a> ");

        if (ptype == 3)
            builder.Append("自建");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=3") + "\">自建</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));

        if (showtype == 1)
            builder.Append("当前排行|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=1") + "\">当前排行</a>|");

        if (showtype == 2)
            builder.Append("最高记录");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=top&amp;ptype=" + ptype + "&amp;showtype=2") + "\">最高记录</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "showtype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "Types=" + ptype + " and IShbchat!=1";

        //排序条件
        if (showtype == 1)
            strOrder = "ChatOnLine Desc";
        else if (showtype == 2)
            strOrder = "ChatTopLine Desc";
        // 开始读取列表
        IList<BCW.Model.Chat> listChat = new BCW.BLL.Chat().GetChats(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listChat.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Chat n in listChat)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                int LineNum = 0;
                if (showtype == 1)
                    LineNum = n.ChatOnLine;
                else if (showtype == 2)
                    LineNum = n.ChatTopLine;

                builder.AppendFormat("<a href=\"" + Utils.getUrl("chatroom.aspx?id={0}" + "&amp;hb=1") + "\">{1}({2})</a>", n.ID, n.ChatName, LineNum);
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    private void HbTopPage()
    {
        Master.Title = "红包排行榜";
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, "", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("红包排行榜");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 1)
            builder.Append("总金额排行榜|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hbtop&amp;ptype=1") + "\">总金额排行榜</a>|");

        if (ptype == 2)
            builder.Append("总个数排行榜");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hbtop&amp;ptype=2") + "\">总个数排行榜</a>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex;
        int recordCount=0;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        recordCount = new BCW.HB.BLL.HbPost().GetCount();
        DataSet listhbc = new BCW.HB.BLL.HbPost().GetListByPage1(0, recordCount);
        DataSet listhb = new BCW.HB.BLL.HbPost().GetListByPage2(0, recordCount);
        if (recordCount >= 0)
        {
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }

            for (int soms = 0; soms < skt; soms++)
            {
                int usid;
                long usmoney;
                if (ptype == 1)
                {
                    usid = Convert.ToInt32(listhb.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(listhb.Tables[0].Rows[koo + soms][2]);
                }
                else
                {
                    usid = Convert.ToInt32(listhbc.Tables[0].Rows[koo + soms][1]);
                    usmoney = Convert.ToInt64(listhbc.Tables[0].Rows[koo + soms][2]);
                }
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                string mename = new BCW.BLL.Chat().GetChatName(usid);
                if (mename == "")
                {
                    mename = "**";
                }
                
                int wd = (pageIndex - 1) * 10 + k;
                if (ptype == 1)
                {
                    builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>]." + mename + "红包群共发了" + "<b style=\"color:red\">" + usmoney + "</b>" + ub.Get("SiteBz") + "的红包");

                }
                else
                {
                    builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>]." + mename +"红包群共发了" + "<b style=\"color:red\">" + usmoney + "</b> 个红包");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void CheckAdminPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));

        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        string UsName = new BCW.BLL.User().GetUsName(uid);
        if (UsName == "")
        {
            Utils.Error("不存在的会员", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int Types= int.Parse(Utils.GetRequest("Types", "all", 2, @"^[1-5]$", "选择类型错误"));
            if (Types == 1)
            {
                new BCW.BLL.ChatText().Delete(id, uid);
                //记录日志
                string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
                if (Why != "")
                    Why = ",原因:" + Why + "";

     
                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
                addmodel.ChatId = id;
                addmodel.Types = 0;
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]的聊天记录删除" + Why + "";
                addmodel.PayCent = 0;
                addmodel.AddTime = DateTime.Now;
                new BCW.BLL.ChatLog().Add(addmodel);
                //系统内线
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您在红包群[url=/bbs/chatroom.aspx?id=" + id + "]《" + model.ChatName + "》[/url]的聊天记录删除" + Why + "";
                new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
                Utils.Success("删除聊天记录", "恭喜，删除Ta的聊天记录成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
            }
            else if (Types == 2)
            {
                if (uid == meid)
                {
                    Utils.Error("不能将自己踢出", "");
                }
                if (new BCW.User.CLimits().GetChatLimit(uid, model) > 0)
                {
                    Utils.Error("你不能踢出有本红包群权限的ID", "");
                }
                if (new BCW.BLL.Chatblack().Exists(id, uid))
                {
                    Utils.Error("ID" + uid + "已是本红包群的黑名单", "");
                }
                string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
                //int Minute = int.Parse(Utils.GetRequest("Minute", "post", 2, @"^[1-9]\d*$", "踢出分钟数应是1-60分!"));
                //if (Minute > 60)
                //{
                //    Utils.Error("踢出分钟数应是1-60分", "");
                //}
                //记录黑名单
             
                BCW.Model.Chatblack bkmodel = new BCW.Model.Chatblack();
                bkmodel.Types = 2;
                bkmodel.ChatId = id;
                bkmodel.ChatName = model.ChatName;
                bkmodel.UsID = uid;
                bkmodel.UsName = UsName;
                bkmodel.BlackWhy = Why;
                bkmodel.BlackDay = 147483648;
                bkmodel.AdminUsID = meid;
                bkmodel.ExitTime = DateTime.Now.AddMinutes(147483648);
                bkmodel.AddTime = DateTime.Now;
                new BCW.BLL.Chatblack().Add(bkmodel);
                //记录日志
                if (Why != "")
                    Why = ",原因:" + Why + "";

                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
                addmodel.ChatId = id;
                addmodel.Types = 0;
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]踢出红包群" + Why + "";
                addmodel.PayCent = 0;
                addmodel.AddTime = DateTime.Now;
                new BCW.BLL.ChatLog().Add(addmodel);
                //系统内线
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您踢出红包群《" + model.ChatName + "》" + Why + "";
                new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
                Utils.Success("踢出红包群", "恭喜，将Ta踢出红包群成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
            }
            else if (Types == 3 || Types == 4||Types == 5)
            {
                if (uid == meid)
                {
                    Utils.Error("不能将自己禁言", "");
                }
                if (new BCW.User.CLimits().GetChatLimit(uid, model) > 0)
                {
                    Utils.Error("你不能禁言有本红包群权限的ID", "");
                }
                //int Day = int.Parse(Utils.GetRequest("Day", "post", 2, @"^[1-9]\d*$", "禁言天数应是1-30天"));
                //if (Day > 30)
                //{
                //    Utils.Error("禁言天数应是1-30天", "");
                //}

                string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");

                int Day = 1;
                if (Types == 4)
                    Day = 3;
                else if (Types == 5)
                    Day = 10;

                //记录黑名单
             
                BCW.Model.Chatblack bkmodel = new BCW.Model.Chatblack();
                bkmodel.Types = 1;
                bkmodel.ChatId = id;
                bkmodel.ChatName = model.ChatName;
                bkmodel.UsID = uid;
                bkmodel.UsName = UsName;
                bkmodel.BlackWhy = Why;
                bkmodel.BlackDay = Day;
                bkmodel.AdminUsID = meid;
                bkmodel.ExitTime = DateTime.Now.AddDays(Day);
                bkmodel.AddTime = DateTime.Now;
                new BCW.BLL.Chatblack().Add(bkmodel);
                //记录日志
                if (Why != "")
                    Why = ",原因:" + Why + "";

                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
                addmodel.ChatId = id;
                addmodel.Types = 0;
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]禁言红包群" + Why + "";
                addmodel.PayCent = 0;
                addmodel.AddTime = DateTime.Now;
                new BCW.BLL.ChatLog().Add(addmodel);
                //系统内线
                string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您禁言红包群《" + model.ChatName + "》" + Day + "天" + Why + "";
                new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
                Utils.Success("禁言红包群", "恭喜，将Ta禁言红包群成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
            }
        }
        else
        {
            Master.Title = "管理员操作";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("操作对象:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + uid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + uid + ")</a>");
            builder.Append(Out.Tab("</div>", ""));

            strText = "操作:,原因(20字内):/,,,,,";
            strName = "Types,Why,uid,id,info,act,backurl";
            strType = "select,text,hidden,hidden,hidden,hidden,hidden";
            strValu = "1''" + uid + "'" + id + "'ok'checkadmin'" + Utils.getPage(0) + "";
            strEmpt = "1|删除记录|2|踢出本室|3|禁言1天|4|禁言3天|5|禁言10天,false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定执行,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void CheckDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (!new BCW.BLL.User().Exists(uid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.ChatText().Delete(id, uid);
            //记录日志
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            if (Why != "")
                Why = ",原因:" + Why + "";

            string UsName = new BCW.BLL.User().GetUsName(uid);
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]的聊天记录删除" + Why + "";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            //系统内线
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您在红包群[url=/bbs/chatroom.aspx?id=" + id + "]《" + model.ChatName + "》[/url]的聊天记录删除" + Why + "";
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
            Utils.Success("删除聊天记录", "恭喜，删除Ta的聊天记录成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
        }
        else
        {
            Master.Title = "删除聊天记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除Ta的聊天记录吗.");
            builder.Append(Out.Tab("</div>", ""));
            strText = "原因(20字内):/,,,,,";
            strName = "Why,uid,id,info,act,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + uid + "'" + id + "'ok'checkdel'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定删除,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void CheckOutPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (!new BCW.BLL.User().Exists(uid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        if (uid == meid)
        {
            Utils.Error("不能将自己踢出", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(uid, model) > 0)
        {
            Utils.Error("你不能踢出有本红包群权限的ID", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            if (new BCW.BLL.Chatblack().Exists(id, uid))
            {
                Utils.Error("ID" + uid + "已是本红包群的黑名单", "");
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            int Minute = int.Parse(Utils.GetRequest("Minute", "post", 2, @"^[1-9]\d*$", "踢出分钟数应是1-60分"));
            if (Minute > 60)
            {
                Utils.Error("踢出分钟数应是1-60分", "");
            }
            //记录黑名单
            string UsName = new BCW.BLL.User().GetUsName(uid);
            BCW.Model.Chatblack bkmodel = new BCW.Model.Chatblack();
            bkmodel.Types = 2;
            bkmodel.ChatId = id;
            bkmodel.ChatName = model.ChatName;
            bkmodel.UsID = uid;
            bkmodel.UsName = UsName;
            bkmodel.BlackWhy = Why;
            bkmodel.BlackDay = Minute;
            bkmodel.AdminUsID = meid;
            bkmodel.ExitTime = DateTime.Now.AddMinutes(Minute);
            bkmodel.AddTime = DateTime.Now;
            new BCW.BLL.Chatblack().Add(bkmodel);
            //记录日志
            if (Why != "")
                Why = ",原因:" + Why + "";

            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]踢出红包群" + Minute + "分钟" + Why + "";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            //系统内线
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您踢出红包群《" + model.ChatName + "》" + Minute + "分钟" + Why + "";
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
            Utils.Success("踢出红包群", "恭喜，将Ta踢出红包群成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
        }
        else
        {
            Master.Title = "踢出会员";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将Ta踢出本红包群吗.");
            builder.Append(Out.Tab("</div>", ""));
            strText = "原因(20字内):/,踢出多少分钟(最多60分钟):/,,,,,";
            strName = "Why,Minute,uid,id,info,act,backurl";
            strType = "text,num,hidden,hidden,hidden,hidden,hidden";
            strValu = "'5'" + uid + "'" + id + "'ok'checkout'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定踢出,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void CheckStopPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (!new BCW.BLL.User().Exists(uid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        if (uid == meid)
        {
            Utils.Error("不能将自己禁言", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(uid, model) > 0)
        {
            Utils.Error("你不能禁言有本红包群权限的ID", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            if (new BCW.BLL.Chatblack().Exists(id, uid))
            {
                Utils.Error("ID" + uid + "已是本红包群的黑名单", "");
            }
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            int Day = int.Parse(Utils.GetRequest("Day", "post", 2, @"^[1-9]\d*$", "禁言天数应是1-30天"));
            if (Day > 30)
            {
                Utils.Error("禁言天数应是1-30天", "");
            }
            //记录黑名单
            string UsName = new BCW.BLL.User().GetUsName(uid);
            BCW.Model.Chatblack bkmodel = new BCW.Model.Chatblack();
            bkmodel.Types = 1;
            bkmodel.ChatId = id;
            bkmodel.ChatName = model.ChatName;
            bkmodel.UsID = uid;
            bkmodel.UsName = UsName;
            bkmodel.BlackWhy = Why;
            bkmodel.BlackDay = Day;
            bkmodel.AdminUsID = meid;
            bkmodel.ExitTime = DateTime.Now.AddDays(Day);
            bkmodel.AddTime = DateTime.Now;
            new BCW.BLL.Chatblack().Add(bkmodel);
            //记录日志
            if (Why != "")
                Why = ",原因:" + Why + "";

            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]禁言红包群" + Why + "";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            //系统内线
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您禁言红包群《" + model.ChatName + "》" + Day + "天" + Why + "";
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
            Utils.Success("禁言红包群", "恭喜，将Ta禁言红包群成功，正在返回..", Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1"), "1");
        }
        else
        {
            Master.Title = "禁言会员";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将Ta禁言本红包群吗.");
            builder.Append(Out.Tab("</div>", ""));
            strText = "原因(20字内):/,禁言多少天(最多30天):/,,,,,";
            strName = "Why,Day,uid,id,info,act,backurl";
            strType = "text,num,hidden,hidden,hidden,hidden,hidden";
            strValu = "'1'" + uid + "'" + id + "'ok'checkstop'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定禁言,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AdminBlackDelPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (!new BCW.BLL.User().Exists(uid))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) == 0)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            if (!new BCW.BLL.Chatblack().Exists(id, uid))
            {
                Utils.Error("不存在的黑名单记录", "");
            }
            new BCW.BLL.Chatblack().Delete(id, uid);
            string Why = Utils.GetRequest("Why", "post", 3, @"^[^\^]{1,20}$", "理由限20字内，可留空");
            //记录日志
            if (Why != "")
                Why = ",原因:" + Why + "";

            string UsName = new BCW.BLL.User().GetUsName(uid);
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "将[url=/bbs/uinfo.aspx?uid=" + uid + "]" + UsName + "[/url]解黑" + Why + "";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            //系统内线
            string strLog = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]将您在红包群[url=/bbs/chatroom.aspx?id=" + id + "]《" + model.ChatName + "》[/url]解黑" + Why + "";
            new BCW.BLL.Guest().Add(0, uid, UsName, strLog);
            Utils.Success("解除红包群黑名单", "恭喜，将Ta解除红包群黑名单成功，正在返回..", Utils.getUrl("chat.aspx?act=admblack&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            Master.Title = "解除红包群黑名单";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定将Ta解除本红包群黑名单吗.");
            builder.Append(Out.Tab("</div>", ""));
            strText = "原因(20字内):/,,,,,";
            strName = "Why,uid,id,info,act,backurl";
            strType = "text,hidden,hidden,hidden,hidden,hidden";
            strValu = "'" + uid + "'" + id + "'ok'admblackdel'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定解除,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admblack&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }


    private void AdminLogPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "【" + model.ChatName + "】操作日志";
        builder.Append(Out.Tab("<div class=\"title\">" + model.ChatName + "操作日志</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=0 and ChatId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.ChatLog> listChatLog = new BCW.BLL.ChatLog().GetChatLogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listChatLog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.ChatLog n in listChatLog)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append((pageIndex - 1) * pageSize + k + ".");

                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}</a>{2}[{3}]", n.UsID, n.UsName, Out.SysUBB(n.Content), DT.FormatDate(n.AddTime, 5));
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admin&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void AdminBlackPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-2]\d*$", "1"));
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }

        Master.Title = "【" + model.ChatName + "】监狱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
            builder.Append("被禁言|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admblack&amp;ptype=1&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">被禁言</a>|");

        if (ptype == 2)
            builder.Append("被踢出");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admblack&amp;ptype=2&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">被踢出</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=" + ptype + " and ChatId=" + id + " and ExitTime>'" + DateTime.Now + "'";

        // 开始读取列表
        IList<BCW.Model.Chatblack> listChatblack = new BCW.BLL.Chatblack().GetChatblacks(pageIndex, pageSize, strWhere, out recordCount);
        if (listChatblack.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Chatblack n in listChatblack)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append((pageIndex - 1) * pageSize + k + ".");

                string Why = n.BlackWhy;
                if (Why == "")
                    Why = "无";

                if (ptype == 1)
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>被禁言{2}天/原因:" + Why + "[{3}]", n.UsID, n.UsName, n.BlackDay, DT.FormatDate(n.AddTime, 5));
                else
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>被踢出/原因:" + Why + "[{3}]", n.UsID, n.UsName, n.BlackDay, DT.FormatDate(n.AddTime, 5));

                builder.Append("<br />操作ID:<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.AdminUsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.AdminUsID + "</a>");
                if (new BCW.User.CLimits().GetChatLimit(meid, model) > 0)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admblackdel&amp;uid=" + n.UsID + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[解除]</a>");
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admin&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }


    private void FundPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        Master.Title = "【" + model.ChatName + "】基金";
        builder.Append(Out.Tab("<div class=\"title\">" + model.ChatName + "基金</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("金库:" + model.ChatCent + "" + ub.Get("SiteBz") + "");
        //室主以上权限
        if (new BCW.User.CLimits().GetChatLimit(meid, model) > 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fundget&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[取款]</a>");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fundpwd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">[改密]</a>");
        }
        builder.Append("<br /><a href=\"" + Utils.getUrl("chat.aspx?act=fundpay&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">我要捐款</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fundlist&amp;ptype=2&amp;&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">支出明细</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【最新捐款】<a href=\"" + Utils.getUrl("chat.aspx?act=fundlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">光荣榜</a>");
        builder.Append(Out.Tab("</div>", ""));

        DataSet ds = new BCW.BLL.ChatLog().GetList("TOP 10 UsID,UsName,PayCent,AddTime", "Types=1 and ChatId=" + id + " ORDER BY PayCent DESC");
        if (ds != null && ds.Tables[0].Rows.Count != 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ds.Tables[0].Rows[i]["UsID"].ToString() + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ds.Tables[0].Rows[i]["UsName"].ToString() + "</a>捐了" + ds.Tables[0].Rows[i]["PayCent"].ToString() + "" + ub.Get("SiteBz") + "(" + DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()), 4) + ")");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("没有相关记录..");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));


    }

    private void FundListPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        string Title = string.Empty;
        if (ptype == 1)
            Title = "光荣榜";
        else
            Title = "支出明细";

        Master.Title = "【" + model.ChatName + "】" + Title + "";
        builder.Append(Out.Tab("<div class=\"title\">" + model.ChatName + "" + Title + "</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=" + ptype + " and ChatId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.ChatLog> listChatLog = new BCW.BLL.ChatLog().GetChatLogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listChatLog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.ChatLog n in listChatLog)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                if (ptype == 1)
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>捐了{2}" + ub.Get("SiteBz") + "[{3}]", n.UsID, n.UsName, n.PayCent, DT.FormatDate(n.AddTime, 5));
                else
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(0) + "") + "\">{1}({0})</a>取出{2}" + ub.Get("SiteBz") + "[{3}]", n.UsID, n.UsName, n.PayCent, DT.FormatDate(n.AddTime, 5));
                k++;
                builder.Append(Out.Tab("</div>", ""));

            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void FundPayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string paypwd = new BCW.BLL.User().GetUsPled(meid);
        if (info == "ok")
        {
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "请正确填写捐款数目"));
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[\s\S]{1,20}$", "请正确填写支付密码");
            if (!Utils.MD5Str(Pwd).Equals(paypwd))
            {
                Utils.Error("支付密码不正确", "");
            }
            if (new BCW.BLL.User().GetGold(meid) < payCent)
            {
                Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
            }
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 1;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = "";
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            new BCW.BLL.Chat().UpdateChatCent(id, payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, -payCent, "红包群捐款");
    
            new BCW.HB.BLL.ChatMe().Update(id, meid, Convert.ToInt32(payCent/100));
            Utils.Success("捐款", "捐款成功..", Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (paypwd == "")
            {
                Utils.Error("你还没有设置支付密码呢<br /><a href=\"" + Utils.getUrl("myedit.aspx?act=paypwd&amp;backurl=" + Utils.PostPage(1) + "") + "\">&gt;马上设置</a>", "");
            }
            Master.Title = "【" + model.ChatName + "】捐款";
            builder.Append(Out.Tab("<div class=\"title\">" + model.ChatName + "捐款</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("金库:" + model.ChatCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("您自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            strText = "" + ub.Get("SiteBz") + "数目:/,支付密码:/,,,,";
            strName = "payCent,Pwd,id,info,act,backurl";
            strType = "num,password,hidden,hidden,hidden,hidden";
            strValu = "''" + id + "'ok'fundpay'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定捐款,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FundGetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "请正确填写取款数目"));
            string Pwd = Utils.GetRequest("Pwd", "post", 2, @"^[\s\S]{1,20}$", "请正确填写基金密码");
            if (!Pwd.Equals(model.CentPwd))
            {
                Utils.Error("基金密码不正确", "");
            }
            if (model.ChatCent < payCent)
            {
                Utils.Error("基金不足" + payCent + "" + ub.Get("SiteBz") + "", "");
            }
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 2;
            addmodel.UsID = meid;
            addmodel.UsName = new BCW.BLL.User().GetUsName(meid);
            addmodel.Content = "";
            addmodel.PayCent = payCent;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            new BCW.BLL.Chat().UpdateChatCent(id, -payCent);
            //操作币
            new BCW.BLL.User().UpdateiGold(meid, payCent, "红包群取款");
            Utils.Success("取款", "取款成功..", Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            if (string.IsNullOrEmpty(model.CentPwd))
            {
                Utils.Error("你还没有设置基金密码呢<br /><a href=\"" + Utils.getUrl("chat.aspx?act=fundpwd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;马上设置</a>", "");
            }
            Master.Title = "【" + model.ChatName + "】取款";
            builder.Append(Out.Tab("<div class=\"title\">" + model.ChatName + "取款</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("金库:" + model.ChatCent + "" + ub.Get("SiteBz") + "<br />");
            builder.Append("您自带:" + new BCW.BLL.User().GetGold(meid) + "" + ub.Get("SiteBz") + "");
            builder.Append(Out.Tab("</div>", ""));
            strText = "" + ub.Get("SiteBz") + "数目:/,基金密码:/,,,,";
            strName = "payCent,Pwd,id,info,act,backurl";
            strType = "num,password,hidden,hidden,hidden,hidden";
            strValu = "''" + id + "'ok'fundget'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false,flase";
            strIdea = "/";
            strOthe = "确定取出,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void FundPwdPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string oPwd = Utils.GetRequest("oPwd", "post", 3, @"^[A-Za-z0-9]{6,20}$", "原密码限6-20位,必须由字母或数字组成");
            string nPwd = Utils.GetRequest("nPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "新密码限6-20位,必须由字母或数字组成");
            string rPwd = Utils.GetRequest("rPwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "确认密码限6-20位,必须由字母或数字组成");
            if (!nPwd.Equals(rPwd))
            {
                Utils.Error("新密码与确认密码不相符", "");
            }

            string ordPwd = model.CentPwd;
            if (!string.IsNullOrEmpty(ordPwd))
            {
                if (!oPwd.Equals(ordPwd))
                {
                    Utils.Error("原基金密码不正确", "");
                }
            }
            new BCW.BLL.Chat().UpdateCentPwd(id, nPwd);
            Utils.Success("修改基金密码", "恭喜，修改基金密码成功，正在返回..", Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + ""), "2");
        }
        else
        {
            Master.Title = "修改基金密码";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("修改基金密码");
            builder.Append(Out.Tab("</div>", ""));
            if (string.IsNullOrEmpty(model.CentPwd))
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("您是首次设置基金密码:");
                builder.Append(Out.Tab("</div>", ""));
                strText = "基金密码:/,确认密码:/,,,,";
                strName = "nPwd,rPwd,id,act,info,backurl";
                strType = "password,password,hidden,hidden,hidden,hidden";
                strValu = "''" + id + "'fundpwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false";
            }
            else
            {
                strText = "原密码:/,新密码:/,确认密码:/,,,,";
                strName = "oPwd,nPwd,rPwd,id,act,info,backurl";
                strType = "password,password,password,hidden,hidden,hidden,hidden";
                strValu = "'''" + id + "'fundpwd'ok'" + Utils.getPage(0) + "";
                strEmpt = "false,false,false,false,false,false,false";
            }
            strIdea = "/";
            strOthe = "确定设置,chat.aspx,post,1,red";

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void AdminPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        Master.Title = model.ChatName + "聊务";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.ChatName + "聊务");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("在线" + model.ChatOnLine + "人,最高" + model.ChatTopLine + "人<br />");
        builder.Append("基金:" + model.ChatCent + "" + ub.Get("SiteBz") + "");
        //if (model.Types > 0)
        //    builder.Append("<br />等级:5级");

        if (model.Types == 3)
        {
            builder.Append("<br />到期时间:" + DT.FormatDate(model.ExTime, 5) + "");
        }

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admview&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">红包群管理</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=fund&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">红包群基金</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admlog&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">操作日志</a>|");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admblack&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">红包群监狱</a>");
        if (new BCW.User.CLimits().GetChatLimit(meid, model) > 2)
        {
            builder.Append("<br /><a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;管理本红包群</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void AdminListPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = model.ChatName + "管理";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">马上进入房间</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("执行管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*红包群设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clear&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*清空聊天</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearlog&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*清空聊务</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admset&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*设管理员</a><br />");
        if (model.Types == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=editpwd&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*设置密码</a><br />");
            //builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=editpay&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*续费红包群</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=member&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*成员管理及权限</a><br />");
        }
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=cbset&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">*抢币设置</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admin&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void ClearPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 3)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int Day = int.Parse(Utils.GetRequest("Day", "all", 2, @"^[0-9]\d*$", "天数填写错误"));
            if (Day == 0)
                new BCW.BLL.ChatText().DeleteStr(id);
            else
                new BCW.BLL.ChatText().DeleteStr(id, Day);

            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "清空了聊天记录";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            Utils.Success("清空聊天记录", "恭喜，清空聊天记录成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            Master.Title = "清空《" + model.ChatName + "》聊天记录";
            builder.Append(Out.Tab("<div class=\"title\">清空聊天记录</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("清空N天前的聊天记录(填0则清空全部):");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,,";
            strName = "Day,id,act,info,backurl";
            strType = "snum,hidden,hidden,hidden,hidden";
            strValu = "10'" + id + "'clear'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";

            strIdea = "/";
            strOthe = "确定清空,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", " "));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ClearLogPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 3)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            new BCW.BLL.ChatLog().DeleteStr(id, 0);
            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "清空了聊务日志";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            Utils.Success("清空聊务", "恭喜，清空聊务成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            Master.Title = "清空《" + model.ChatName + "》聊务";
            builder.Append(Out.Tab("<div class=\"title\">清空聊务</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定清空本红包群的聊务日志吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearlog&amp;info=ok&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ChatEditPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string ChatName = Utils.GetRequest("ChatName", "post", 2, @"^[A-Za-zＡ-Ｚ\d０-９\u4E00-\u9FA5]{1,10}$", "红包群名称限1-10字，不能使用特殊字符");
            string ChatNotes = Utils.GetRequest("ChatNotes", "post", 3, @"^[^\^]{1,200}$", "红包群主题限500字内，可留空");
            string ChatFoot = Utils.GetRequest("ChatFoot", "post", 3, @"^[^\^]{1,2000}$", "红包群底部UBB限2000字内，可留空");
            int type= int.Parse(Utils.GetRequest("type", "all", 2, @"^[0-1]\d*$", "类型错误"));
            BCW.Model.Chat upmodel = new BCW.Model.Chat();
            upmodel.ID = id;
            upmodel.ChatName = ChatName;
            upmodel.ChatNotes = ChatNotes;
            upmodel.ChatFoot = ChatFoot;
            new BCW.BLL.Chat().UpdateBasic(upmodel);
            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "修改了红包群设置";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            new BCW.HB.BLL.HbPost().UpdateChatIsHb(id,type);
            Utils.Success("红包群设置", "恭喜，红包群设置成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            int ishb = new BCW.HB.BLL.HbPost().GetChatGS(id);
            Master.Title = "《" + model.ChatName + "》设置";
            builder.Append(Out.Tab("<div class=\"title\">红包群设置</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("红包群名称:");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",红包群主题:/,底部UBB:/,,,,,";
            strName = "ChatName,ChatNotes,ChatFoot,type,id,act,info,backurl";
            strType = "text,text,textarea,select,hidden,hidden,hidden,hidden";
            strValu = "" + model.ChatName + "'" + model.ChatNotes + "'" + model.ChatFoot + "'"+ishb+"'" + id + "'edit'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,true,true,0|公共|1|私人,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ChatEditPwdPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        if (model.Types != 3)
        {
            Utils.Error("只有自建红包群才可以设置密码", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string ChatPwd = Utils.GetRequest("ChatPwd", "post", 2, @"^[A-Za-z0-9]{1,20}$", "密码限1-20位,必须由字母或数字组成");
            new BCW.BLL.Chat().UpdateChatPwd(id, ChatPwd);
            if (model.ChatPwd != ChatPwd)
            {
                new BCW.BLL.Chat().UpdatePwdID(id, "");//清空进入的ID标识
            }

            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "设置了红包群密码";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            Utils.Success("设置密码", "恭喜，设置红包群密码成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            Master.Title = "《" + model.ChatName + "》设置密码";
            builder.Append(Out.Tab("<div class=\"title\">设置密码</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("设置密码:");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,,";
            strName = "ChatPwd,id,act,info,backurl";
            strType = "text,hidden,hidden,hidden,hidden";
            strValu = "" + model.ChatPwd + "'" + id + "'editpwd'ok'" + Utils.getPage(0) + "";
            strEmpt = "false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    //成员管理及邀请权限设置dsy
    private void Member()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        Master.Title = "《" + model.ChatName + "》成员管理及邀请设置";
        builder.Append(Out.Tab("<div class=\"title\">成员管理及邀请设置</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int invi = int.Parse(Utils.GetRequest("invit", "all", 1, @"^[0-4]\d*$", "0"));
        if (info == "ok")
        {
            new BCW.HB.BLL.ChatMe().UpdateInvite(invi,id);
            Utils.Success("邀请设置", "邀请设置成功，正在返回..", Utils.getUrl("chat.aspx?act=member&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("成员列表:");
            builder.Append(Out.Tab("</div>", ""));
            List<BCW.HB.Model.ChatMe> member = new BCW.HB.BLL.ChatMe().GetModelList("ChatID="+id);
            for (int i=0;i<member.Count;i++)
            {
                if(i%2==0)
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                    builder.Append(Out.Tab("<div>", ""));
                string name = new BCW.BLL.User().GetUsName(member[i].UserID);
                builder.Append((i + 1) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + member[i].UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name+"(" + member[i].UserID+")</a>");
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=deltxl&amp;id=" + member[i].ChatID + "&amp;member="+ member[i].UserID) + "\">[移除]</a>");
                builder.Append(Out.Tab("</div>", "<br/>")); 
            }
            if (meid == model.UsID)
            {
                int invit = new BCW.HB.BLL.ChatMe().GetInvite(id);
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("邀请权限：");
                builder.Append(Out.Tab("</div>", ""));
                strText = ",,,,,";
                strName = "invit,id,act,info,backurl";
                strType = "select,hidden,hidden,hidden,hidden";
                strValu = invit + "'" + id + "'member'ok'" + Utils.getPage(0) + "";
                strEmpt = "0|仅群主可以邀请|1|仅群主和室主可以邀请|2|仅群主、室主和见习室主可以邀请|3|仅群主、室主、见习室主和临管可以邀请|4|所有成员可以邀请,false,false,false,false";
                strIdea = "/";
                strOthe = "确定修改,chat.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
     
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }
    private void ChatEditPayPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        if (model.Types != 3)
        {
            Utils.Error("只有自建红包群才可以续费", "");
        }
        Master.Title = "《" + model.ChatName + "》续费";
        builder.Append(Out.Tab("<div class=\"title\">红包群续费</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok" || info == "acok")
        {
            int Hour = int.Parse(Utils.GetRequest("Hour", "post", 2, @"^[0-7]$", "选择时长错误"));
            DateTime ExTime = model.ExTime;
            int Cent = 0;
            int iCent = Convert.ToInt32(ub.GetSub("ChatiPrice", xmlPath));//每小时多少币
            if (Hour == 0)
            {
                Cent = iCent * 2;
                ExTime = ExTime.AddHours(2);
            }
            else if (Hour == 1)
            {
                Cent = iCent * 5;
                ExTime = ExTime.AddHours(5);
            }
            else if (Hour == 2)
            {
                Cent = iCent * 10;
                ExTime = ExTime.AddHours(10);
            }
            else if (Hour == 3)
            {
                Cent = iCent * 24;
                ExTime = ExTime.AddDays(1);
            }
            else if (Hour == 4)
            {
                Cent = iCent * 48;
                ExTime = ExTime.AddDays(2);
            }
            else if (Hour == 5)
            {
                Cent = iCent * 120;
                ExTime = ExTime.AddDays(5);
            }
            else if (Hour == 6)
            {
                Cent = iCent * 240;
                ExTime = ExTime.AddDays(10);
            }
            else if (Hour == 7)
            {
                Cent = iCent * 720;
                ExTime = ExTime.AddDays(30);
            }
            //消费币
            long payCent = Convert.ToInt64(Cent);
            if (info != "acok")
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("续费需" + payCent + "" + ub.Get("SiteBz") + ",确定要续费吗");
                builder.Append(Out.Tab("</div>", "<br />"));

                strName = "Hour,id,act,info,backurl";
                strValu = "" + Hour + "'" + id + "'editpay'acok'" + Utils.getPage(0) + "";
                strOthe = "确认续费,chat.aspx,post,0,red";
                builder.Append(Out.wapform(strName, strValu, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">&gt;返回上一级</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                long Gold = new BCW.BLL.User().GetGold(meid);
                if (Gold < payCent)
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足" + payCent + "" + ub.Get("SiteBz") + "", "");
                }
                new BCW.BLL.Chat().UpdateExitTime(id, ExTime);
                //操作币
                new BCW.BLL.User().UpdateiGold(meid, -payCent, "续费红包群");
                //记录日志
                string mename = new BCW.BLL.User().GetUsName(meid);
                BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
                addmodel.ChatId = id;
                addmodel.Types = 0;
                addmodel.UsID = meid;
                addmodel.UsName = mename;
                addmodel.Content = "续费红包群期限" + DT.FormatDate(model.ExTime, 0) + "至" + DT.FormatDate(ExTime, 0) + "";
                addmodel.PayCent = 0;
                addmodel.AddTime = DateTime.Now;
                new BCW.BLL.ChatLog().Add(addmodel);
                Utils.Success("续费红包群", "恭喜，续费红包群成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
             
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("选择续费时长:");
            builder.Append(Out.Tab("</div>", ""));
            strText = ",,,,,";
            strName = "Hour,id,act,info,backurl";
            strType = "select,hidden,hidden,hidden,hidden";
            strValu = "0'" + id + "'editpay'ok'" + Utils.getPage(0) + "";
            strEmpt = "0|2小时|1|5小时|2|10小时|3|1天|4|2天|5|5天|6|10天|7|30天,false,false,false,false";
            strIdea = "/";
            strOthe = "确定续费,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ChatCbSetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string ChatCT = Utils.GetRequest("ChatCT", "post", 3, @"^[A-Za-z\u4e00-\u9fa5]{1,15}$", "抢币词语10字内，不能使用特殊符号，可留空");
            string ChatCbig = Utils.GetRequest("ChatCbig", "post", 3, @"^[1-9]\d*-[1-9]\d*$", "抢币整点随机值填写格式10-20，不开放抢币请留空");
            string ChatCsmall = Utils.GetRequest("ChatCsmall", "post", 3, @"^[1-9]\d*-[1-9]\d*$", "抢币非整点随机值填写格式1-5，不开放抢币请留空");
            int ChatCon = int.Parse(Utils.GetRequest("ChatCon", "post", 2, @"^[0-9]\d*$", "循环时间必须为数字，不开放抢币请填写0"));


            //抢币随机值判断
            if (ChatCbig != "" && ChatCsmall != "")
            {
                string[] Cbig = ChatCbig.Split("-".ToCharArray());
                if (Convert.ToInt32(Cbig[0]) >= Convert.ToInt32(Cbig[1]))
                {
                    Utils.Error("抢币整点随机值填写格式10-20，不开放抢币请留空", "");
                }
                string[] Csmall = ChatCsmall.Split("-".ToCharArray());
                if (Convert.ToInt32(Csmall[0]) >= Convert.ToInt32(Csmall[1]))
                {
                    Utils.Error("抢币非整点随机值填写格式1-5，不开放抢币请留空", "");
                }
            }

            //抢币标识
            string sCTemp = string.Empty;
            if (ChatCT != model.ChatCT)
            {
                string CText = Utils.ConvertSeparated(ChatCT, 1, "#");
                string[] CTemp = CText.Split("#".ToCharArray());
                for (int i = 0; i < CTemp.Length; i++)
                {
                    sCTemp += "#0";
                }
                sCTemp = Utils.Mid(sCTemp, 1, sCTemp.Length);
            }
            else
                sCTemp = model.ChatCId;

            BCW.Model.Chat upmodel = new BCW.Model.Chat();
            upmodel.ID = id;
            upmodel.ChatCT = ChatCT;
            upmodel.ChatCbig = ChatCbig;
            upmodel.ChatCsmall = ChatCsmall;
            upmodel.ChatCId = sCTemp;//初始化抢币
            upmodel.ChatCon = ChatCon;
            new BCW.BLL.Chat().UpdateCb(upmodel);
            new BCW.HB.BLL.HbPost().UpdateChatCmoney(id, sCTemp);
            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "修改了红包群抢币设置";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            Utils.Success("红包群设置", "恭喜，抢币设置成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            Master.Title = "《" + model.ChatName + "》抢币设置";
            builder.Append(Out.Tab("<div class=\"title\">红包群抢币设置</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("红包群名称:");
            builder.Append(Out.Tab("</div>", ""));
            strText = "抢币词语(15字内):/,抢币整点随机值(格式如10-20):/,抢币非整点随机值(格式如1-5):/,抢币循环时间(秒):/,,,,";
            strName = "ChatCT,ChatCbig,ChatCsmall,ChatCon,id,act,info,backurl";
            strType = "text,text,text,num,hidden,hidden,hidden,hidden";
            strValu = "" + model.ChatCT + "'" + model.ChatCbig + "'" + model.ChatCsmall + "'" + model.ChatCon + "'" + id + "'cbset'ok'" + Utils.getPage(0) + "";
            strEmpt = "true,true,true,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示：<br />抢币词语如:“我要抢币”，限15字内.<br />当会员整点抢币时，系统自动调用整点随机值随机奖币，反之随机非整点值<br />抢币词语不为空与抢币循环时间不为0时将启用该红包群的抢币功能");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ChatAdminSetPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string ChatSZ = string.Empty;
            if (new BCW.User.CLimits().GetChatLimit(meid, model) > 3)
            {
                ChatSZ = Utils.GetRequest("ChatSZ", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "室主ID多个请用#分隔，可留空");
            }
            string ChatJS = Utils.GetRequest("ChatJS", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "见习室主ID多个请用#分隔，可留空");
            string ChatLG = Utils.GetRequest("ChatLG", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "临管ID多个请用#分隔，可留空");
            BCW.Model.Chat upmodel = new BCW.Model.Chat();
            upmodel.ID = id;
            upmodel.ChatSZ = ChatSZ;
            upmodel.ChatJS = ChatJS;
            upmodel.ChatLG = ChatLG;
            if (new BCW.User.CLimits().GetChatLimit(meid, model) > 3)
            {
                new BCW.BLL.Chat().UpdateAdmin(upmodel);
            }
            else
            {
                new BCW.BLL.Chat().UpdateAdmin2(upmodel);
            }

            //记录日志
            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.ChatLog addmodel = new BCW.Model.ChatLog();
            addmodel.ChatId = id;
            addmodel.Types = 0;
            addmodel.UsID = meid;
            addmodel.UsName = mename;
            addmodel.Content = "修改了设定管理员设置";
            addmodel.PayCent = 0;
            addmodel.AddTime = DateTime.Now;
            new BCW.BLL.ChatLog().Add(addmodel);
            Utils.Success("设定管理员", "恭喜，设定管理员成功，正在返回..", Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            Master.Title = "《" + model.ChatName + "》设定管理员";
            builder.Append(Out.Tab("<div class=\"title\">设定管理员</div>", ""));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("根据您的权限你可以设置以下管理员");
            builder.Append(Out.Tab("</div>", ""));

            string sText = string.Empty;
            string sName = string.Empty;
            string sType = string.Empty;
            string sValu = string.Empty;
            string sEmpt = string.Empty;
            if (new BCW.User.CLimits().GetChatLimit(meid, model) > 3)
            {
                sText = "室主ID(多个用#分隔):/,";
                sName = "ChatSZ,";
                sType = "text,";
                sValu = "" + model.ChatSZ + "'";
                sEmpt = "false,";
            }
            strText = sText + "见习室主(多个用#分隔):/,临管ID(多个用#分隔):/,,,,";
            strName = sName + "ChatJS,ChatLG,id,act,info,backurl";
            strType = sType + "text,text,hidden,hidden,hidden,hidden";
            strValu = sValu + "" + model.ChatJS + "'" + model.ChatLG + "'" + id + "'admset'ok'" + Utils.getPage(0) + "";
            strEmpt = sEmpt + "false,false,false,false,false,false";
            strIdea = "/";
            strOthe = "确定设置,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=admlist&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void ChatAdminViewPage()
    {
        Master.Title = "红包群管理员";
        builder.Append(Out.Tab("<div class=\"title\">红包群管理员</div>", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        if (new BCW.User.CLimits().GetChatLimit(meid, model) <= 2)
        {
            Utils.Error("你的权限不足", "");
        }
        //室主名单
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>室主名单</b>");
        if (!string.IsNullOrEmpty(model.ChatSZ))
        {
            string[] ChatSZ = model.ChatSZ.Split("#".ToCharArray());

            for (int i = 0; i < ChatSZ.Length; i++)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + Convert.ToInt32(ChatSZ[i]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ChatSZ[i])) + "</a>");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        //见习室主
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>见习室主名单</b>");
        if (!string.IsNullOrEmpty(model.ChatJS))
        {
            string[] ChatJS = model.ChatJS.Split("#".ToCharArray());

            for (int i = 0; i < ChatJS.Length; i++)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + Convert.ToInt32(ChatJS[i]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ChatJS[i])) + "</a>");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        //临管
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>临管名单</b>");
        if (!string.IsNullOrEmpty(model.ChatLG))
        {
            string[] ChatLG = model.ChatLG.Split("#".ToCharArray());

            for (int i = 0; i < ChatLG.Length; i++)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + Convert.ToInt32(ChatLG[i]) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(Convert.ToInt32(ChatLG[i])) + "</a>");
            }
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=admin&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ChatAdminPage()
    {
        Master.Title = "红包群管理员";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("chat.aspx") + "\">聊天大厅</a>&gt;聊管");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "Status=0 and Rolece collate Chinese_PRC_CS_AS_WS like '%E%'";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        // 开始读取列表
        IList<BCW.Model.Role> listRole = new BCW.BLL.Role().GetRolesAdmin(pageIndex, pageSize, strWhere, out recordCount);
        if (listRole.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Role n in listRole)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "(" + n.UsID + ")</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void OnLinePage()
    {
        Master.Title = "红包群在线";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "红包群ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">" + model.ChatName + "</a>&gt;在线聊友");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere += "EndChatID=" + id + " and EndTime>='" + DateTime.Now.AddMinutes(-5) + "'";

        // 开始读取列表
        IList<BCW.Model.User> listUser = new BCW.BLL.User().GetUsers(pageIndex, pageSize, strWhere, out recordCount);
        if (listUser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.User n in listUser)
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
                if (n.State == 1)
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".隐身会员");
                else
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.ID) + "(" + n.ID + ")</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + id + "&amp;hb=1") + "\">房间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void ExitPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "红包群ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        else if (model.Types == 3 && model.ExTime < DateTime.Now)
        {
            Utils.Error("红包群已过期", "");
        }
        new BCW.BLL.User().UpdateEndChatID(meid, 0);
        int ChatLine = new BCW.BLL.User().GetChatNum(id);
        new BCW.BLL.Chat().UpdateLine(id, ChatLine);
        Utils.Success("离开红包群", "离开红包群成功..", Utils.getUrl("chat.aspx"), "1");
    }

    private void ChatHelpPage()
    {
        Master.Title = "红包群帮助";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("红包群帮助");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.创建的会员必须是认证会员，等级" + ub.GetSub("ChatAddLeven", xmlPath) + "级以上.<br />");
        builder.Append("2.每位会员可以创建五个红包群<br/>");
        builder.Append("3.红包群30天无人发言自动解散<br/>");
        builder.Append("4.红包群设为私人，则除创建者其他人不可见<br/>");
        builder.Append("5.酷友创建的红包群，只能通过邀请方能进入。<br/>");
        builder.Append("6.红包群群主，可以设置邀请权限。<br/>");
        builder.Append("7.酷友热聊室发言有奖励。<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>");
        builder.Append("-<a href=\"" + Utils.getUrl("ARobot.aspx") + "\">酷宝</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    public void DelTxl()
    {
        int member = int.Parse(Utils.GetRequest("member", "all", 1, @"^[1-9]\d*$", "0"));
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "post", 1, "", ""));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "红包群不存在.."));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (member != 0)
        {
            Master.Title = "从红包群移除" + member;
            BCW.Model.Chat moo = new BCW.BLL.Chat().GetChat(id);
            if (meid != moo.UsID)
            {
                Utils.Error(moo.UsID + "无效的操作！", "");
            }
        }
        else
        {
            Master.Title = "退出红包群";
        }
        
        

        if (ac == "确定")
        {
            
            if (member != 0)
            {
                new BCW.HB.BLL.ChatMe().Delete(id, member);
                Utils.Success("从红包群移除" + member, "移除成功，正在返回..", Utils.getPage("chat.aspx?act=member&id=" + id), "0");
            }
            else
            {
                new BCW.HB.BLL.ChatMe().Delete(id, meid);
                Utils.Success("从红包群退出", "退出成功，正在返回..", Utils.getPage("chat.aspx?ptype=5"), "0");
            }
        }
        else
        {
            
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if(member != 0)
            {
                builder.Append("确定从红包群移除" + member+"吗");
            }
            else
            {
                builder.Append("确定退出该红包群吗");
            }
            
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            string strText = ",,";
            string strName = "id,act,member";
            string strType = "hidden,hidden,hidden";
            string strValu = id+ "'deltxl'"+member;
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "确定,chat.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            if (member == 0)
            {
                builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?ptype=5") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">上级</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(" <a href=\"" + Utils.getUrl("chat.aspx?act=member&id=" + id) + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id="+id + "&amp;hb=1") + "\">红包群</a>-");
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=member&id=" + id + "&amp;hb=1") + "\">上级</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            

        }
    }
}