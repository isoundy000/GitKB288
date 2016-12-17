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
//2016/4/29 dsy
public partial class Manage_chat : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    private string M_Str_mindate = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "text":
                TextPage();
                break;
            case "del":
                DelPage();
                break;
            case "deltext":
                DelTextPage();
                break;
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            case "add":
                AddPage();
                break;
            case "addsave":
                AddSavePage();
                break;
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "hb":
                HbPage();
                break;
            case "delhb":
                DelHb();
                break;
            case "set":
                SetPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "红包群管理";
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("红包群管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 0)
            builder.Append("主题红包群|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=0") + "\">主题</a>|");

        //if (ptype == 1)
        //    builder.Append("圈子红包群|");
        //else
        //    builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=1") + "\">圈子</a>|");

        if (ptype == 2)
            builder.Append("同城红包群|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=2") + "\">同城</a>|");

        if (ptype == 3)
            builder.Append("自建红包群");
        else
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?ptype=3") + "\">自建</a>");

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

        //查询条件
        strWhere = "Types=" + ptype + "";
        //排序条件
        strOrder = "Paixu Asc";
        // 开始读取列表
        IList<BCW.Model.Chat> listChat = new BCW.BLL.Chat().GetChats(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listChat.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Chat n in listChat)
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
                builder.AppendFormat("<a href=\"" + Utils.getUrl("chat.aspx?act=edit&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>{1}.<a href=\"" + Utils.getUrl("chat.aspx?act=text&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}(ID:{0})</a>", n.ID, n.Paixu, n.ChatName);
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=add") + "\">添加红包群</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=set") + "\">红包群设置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clear") + "\">清空聊天记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hb") + "\">红包功能管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void SetPage()
    {
        Master.Title = "红包群设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("红包群设置");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string xmlPath = "/Controls/chat.xml";
        ub xml = new ub();
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ChatHID = Utils.GetRequest("ChatHID", "post", 1, "", "");
            string ChatMost = Utils.GetRequest("ChatMost", "post", 2, @"^[1-9]\d*$", "每日发言最大奖币填写错误");
            string APIKEY = Utils.GetRequest("APIKEY", "post", 1, "", "139deae1b7771302fc95a6ac9562dea7");
            xml.dss["ChatHID"] = ChatHID;
            xml.dss["ChatMost"] = ChatMost;
            xml.dss["APIKEY"] = APIKEY;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("红包群设置", "设置成功，正在返回..", Utils.getUrl("chat.aspx?act=set"), "1");
        }
        else
        {
            string strText = "系统管理ID/,每日发言最大奖币/,酷宝APIKEY/,";
            string strName = "ChatHID,ChatMost,APIKEY,backurl";
            string strType = "num,num,text,hidden";
            string strValu = xml.dss["ChatHID"] + "'" + xml.dss["ChatMost"] + "'" + xml.dss["APIKEY"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,chat.aspx?act=set,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>注：系统管理ID请以#结束。多个管理ID请用#隔开。</div>", "注：管理权限ID请以#结束。多个管理ID请用#隔开。<br/>"));
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">返回红包群管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void TextPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "红包群ID错误"));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        Master.Title = "" + model.ChatName + "聊天记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("" + model.ChatName + "聊天记录");
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
        strWhere = "ChatId=" + id + "";
        // 开始读取列表
        IList<BCW.Model.ChatText> listChatText = new BCW.BLL.ChatText().GetChatTexts(pageIndex, pageSize, strWhere, out recordCount);
        if (listChatText.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.ChatText n in listChatText)
            {

                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));

                builder.Append((pageIndex - 1) * pageSize + k + ".");
                int Isac = 0;
                string Content = n.Content.Trim();
                if (Content.Contains("$N"))
                {
                    Isac = 1;
                    Content = Content.Replace("$N", n.UsName).Replace("$n", n.ToName);
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}({0})</a>", n.UsID, n.UsName);

                if (n.ToID == 0)
                {
                    builder.AppendFormat(":{0}[{1}]", Out.SysUBB(Content), DT.FormatDate(n.AddTime, 10));
                }
                else
                {
                    if (Isac == 0)
                        builder.AppendFormat("对{0}说", n.ToName);
                    builder.AppendFormat(":{0}[{1}]", Out.SysUBB(Content), DT.FormatDate(n.AddTime, 10));
                }
                builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=deltext&amp;id=" + n.ChatId + "&amp;uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");
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
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("chat.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clear&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">清空聊天记录</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelTextPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "会员ID错误"));
        if (!new BCW.BLL.Chat().Exists(id))
        {
            Utils.Error("不存在的记录", "");
        }
        if (info == "")
        {
            Master.Title = "删除Ta的聊天记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除Ta的聊天记录吗.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?info=ok&amp;act=deltext&amp;id=" + id + "&amp;uid=" + uid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("chat.aspx?act=text&amp;id=" + id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            new BCW.BLL.ChatText().Delete(id, uid);
            Utils.Success("删除聊天记录", "删除Ta的聊天记录成功..", Utils.getUrl("chat.aspx?act=text&amp;id=" + id + ""), "1");
        }
    }

    /// <summary>
    /// 清空聊天记录
    /// </summary>
    private void ClearPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        string ChatName = string.Empty;
        if (id > 0)
        {
            BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
            if (model == null)
            {
                Utils.Error("不存在的红包群", "");
            }
            ChatName = model.ChatName;
        }
        Master.Title = "清空" + ChatName + "聊天记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("清空" + ChatName + "聊天记录");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearok&amp;id=" + id + "&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearok&amp;id=" + id + "&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearok&amp;id=" + id + "&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=clearok&amp;id=" + id + "&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    /// <summary>
    /// 清空聊天记录
    /// </summary>
    private void ClearOkPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));
        string ChatName = string.Empty;
        if (id > 0)
        {
            BCW.Model.Chat model = new BCW.BLL.Chat().GetChatBasic(id);
            if (model == null)
            {
                Utils.Error("不存在的红包群", "");
            }
            ChatName = model.ChatName;
        }
        Master.Title = "清空" + ChatName + "聊天记录";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空" + ChatName + "聊天记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?info=ok&amp;act=" + act + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定清空</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("chat.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群管理</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getUrl("chat.aspx?info=acok&amp;act=" + act + "&amp;id=" + id + "&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strWhere = string.Empty;
                if (id > 0)
                    strWhere = "ChatId=" + id + " and ";

                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.ChatText().DeleteStr2("" + strWhere + " AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 2)
                {
                    //保留本周计算
                    switch (DateTime.Now.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Tuesday:
                            M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Wednesday:
                            M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Thursday:
                            M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Friday:
                            M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Saturday:
                            M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + " 0:00:00";
                            break;
                        case DayOfWeek.Sunday:
                            M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + " 0:00:00";
                            break;
                    }
                    new BCW.BLL.ChatText().DeleteStr2("" + strWhere + " AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.ChatText().DeleteStr2("" + strWhere + " AddTime<'" + M_Str_mindate + "'");
                }
                else if (ptype == 4)
                {
                    if (id > 0)
                        new BCW.BLL.ChatText().DeleteStr2("ChatId=" + id + "");
                    else
                        new BCW.BLL.ChatText().DeleteStr2("");
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("chat.aspx"), "2");
            }
        }

    }
    private void AddPage()
    {
        Master.Title = "添加红包群";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加红包群");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "红包群名称:/,红包群主题(200字内):/,室主ID(多个用#分隔):/,见习室主ID(多个用#分隔):/,临管(多个用#分隔):/,红包群底部UBB:/,红包群类型:/,排序:/,,";
        string strName = "ChatName,ChatNotes,ChatSZ,ChatJS,ChatLG,ChatFoot,Types,Paixu,act,backurl";
        string strType = "text,text,text,text,text,textarea,select,snum,hidden,hidden";
        string strValu = "''''''0'0'addsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,true,true,true,true,true,0|主题红包群|2|同城红包群|3|自建红包群,false,false,false";
        string strIdea = "/";
        string strOthe = "添加红包群|reset,chat.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void AddSavePage()
    {
        string ChatName = Utils.GetRequest("ChatName", "post", 2, @"^[^\^]{1,20}$", "聊天名称限20字内");
        string ChatNotes = Utils.GetRequest("ChatNotes", "post", 3, @"^[^\^]{1,200}$", "红包群主题限500字内，可留空");
        string ChatSZ = Utils.GetRequest("ChatSZ", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "室主ID多个请用#分隔，可留空");
        string ChatJS = Utils.GetRequest("ChatJS", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "见习室主ID多个请用#分隔，可留空");
        string ChatLG = Utils.GetRequest("ChatLG", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "临管ID多个请用#分隔，可留空");
        string ChatFoot = Utils.GetRequest("ChatFoot", "post", 3, @"^[^\^]{1,2000}$", "红包群底部UBB限2000字内，可留空");
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-3]$", "类型选择错误"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须为数字"));

        BCW.Model.Chat model = new BCW.Model.Chat();
        model.ChatName = ChatName;
        model.ChatNotes = ChatNotes;
        model.ChatSZ = ChatSZ;
        model.ChatJS = ChatJS;
        model.ChatLG = ChatLG;
        model.ChatFoot = ChatFoot;
        model.Types = Types;
        model.UsID = 0;
        model.GroupId = 0;
        model.Paixu = Paixu;
        model.AddTime = DateTime.Now;
        model.ExTime = DateTime.Parse("1990-1-1 00:00:00");
        new BCW.BLL.Chat().Add(model);
        Utils.Success("添加红包群", "添加红包群成功..", Utils.getUrl("chat.aspx"), "1");
    }

    private void EditPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "红包群ID错误"));
        Master.Title = "编辑红包群";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑红包群");
        builder.Append(Out.Tab("</div>", ""));
        BCW.Model.Chat model = new BCW.BLL.Chat().GetChat(id);
        if (model == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        string ExTime = model.ExTime.ToString();
        if (DT.FormatDate(model.ExTime, 0) == "1990-01-01 00:00:00")
        {
            ExTime = "0";
        }
        string strText = "红包群名称:/,红包群主题(200字内):/,室主ID(多个用#分隔):/,见习室主ID(多个用#分隔):/,临管(多个用#分隔):/,红包群底部UBB:/,红包群基金:/,基金密码:/,最高在线:/,红包群积分:/,创建ID:/,关联城市ID:/,红包群类型:/,红包群密码(自建类型有效):/,抢币词语(15字内):/,抢币整点随机值(格式如10-20):/,抢币非整点随机值(格式如1-5):/,抢币循环时间(秒):/,排序:/,创建时间:/,过期时间(永不过期请填写0):/,,,";
        string strName = "ChatName,ChatNotes,ChatSZ,ChatJS,ChatLG,ChatFoot,ChatCent,CentPwd,ChatTopLine,ChatScore,UsID,GroupId,Types,ChatPwd,ChatCT,ChatCbig,ChatCsmall,ChatCon,Paixu,AddTime,ExTime,id,act,backurl";
        string strType = "text,text,text,text,text,textarea,num,text,num,text,num,num,select,text,text,text,text,num,snum,date,date,hidden,hidden,hidden";
        string strValu = "" + model.ChatName + "'" + model.ChatNotes + "'" + model.ChatSZ + "'" + model.ChatJS + "'" + model.ChatLG + "'" + model.ChatFoot + "'" + model.ChatCent + "'" + model.CentPwd + "'" + model.ChatTopLine + "'" + Convert.ToDouble(model.ChatScore) + "'" + model.UsID + "'" + model.GroupId + "'" + model.Types + "'" + model.ChatPwd + "'" + model.ChatCT + "'" + model.ChatCbig + "'" + model.ChatCsmall + "'" + model.ChatCon + "'" + model.Paixu + "'" + DT.FormatDate(model.AddTime, 0) + "'" + ExTime + "'" + id + "'editsave'" + Utils.getPage(0) + "";
        string strEmpt = "false,true,true,true,true,true,false,true,false,false,false,false,0|主题红包群|1|圈子红包群|2|同城红包群|3|自建红包群,true,true,true,true,false,false,false,false,false,false,false";
        string strIdea = "/";
        string strOthe = "编辑红包群|reset,chat.aspx,post,1,red|blue";

        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示：<br />抢币词语如:“我要抢币”，限15字内.<br />当会员整点抢币时，系统自动调用整点随机值随机奖币，反之随机非整点值<br />抢币词语不为空与抢币循环时间不为0时将启用该红包群的抢币功能");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("chat.aspx") + "\">返回上一级</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">删除此红包群</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1)
        {
            Utils.Error("此功能暂停开放", "");
        }
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "红包群ID错误"));
        string ChatName = Utils.GetRequest("ChatName", "post", 2, @"^[^\^]{1,20}$", "聊天名称限20字内");
        string ChatNotes = Utils.GetRequest("ChatNotes", "post", 3, @"^[^\^]{1,200}$", "红包群主题限200字内，可留空");
        string ChatSZ = Utils.GetRequest("ChatSZ", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "室主ID多个请用#分隔，可留空");
        string ChatJS = Utils.GetRequest("ChatJS", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "见习室主ID多个请用#分隔，可留空");
        string ChatLG = Utils.GetRequest("ChatLG", "post", 3, @"^[0-9]{1,10}(?:\#[0-9]{1,10}){0,500}$", "临管ID多个请用#分隔，可留空");
        string ChatFoot = Utils.GetRequest("ChatFoot", "post", 3, @"^[^\^]{1,2000}$", "红包群底部UBB限2000字内，可留空");
        int ChatCent = int.Parse(Utils.GetRequest("ChatCent", "post", 2, @"^[0-9]\d*$", "红包群基金填写错误"));
        string CentPwd = Utils.GetRequest("CentPwd", "post", 3, @"^[A-Za-z0-9]{4,20}$", "基金密码限4-20位,必须由字母或数字组成");
        int ChatTopLine = int.Parse(Utils.GetRequest("ChatTopLine", "post", 2, @"^[0-9]\d*$", "最高在线人数填写错误"));
        int ChatScore = int.Parse(Utils.GetRequest("ChatScore", "post", 2, @"^[0-9]\d*$", "红包群积分填写错误"));
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[0-9]\d*$", "创建ID填写错误"));
        int GroupId = int.Parse(Utils.GetRequest("GroupId", "post", 2, @"^[0-9]\d*$", "关联ID填写错误"));
        int Types = int.Parse(Utils.GetRequest("Types", "post", 2, @"^[0-3]$", "类型选择错误"));
        string ChatPwd = Utils.GetRequest("ChatPwd", "post", 3, @"^[A-Za-z0-9]{1,20}$", "红包群密码限1-20位,必须由字母或数字组成，可留空");
        string ChatCT = Utils.GetRequest("ChatCT", "post", 3, @"^[A-Za-z\u4e00-\u9fa5]{1,15}$", "抢币词语10字内，不能使用特殊符号，可留空");
        string ChatCbig = Utils.GetRequest("ChatCbig", "post", 3, @"^[1-9]\d*-[1-9]\d*$", "抢币整点随机值填写格式10-20，不开放抢币请留空");
        string ChatCsmall = Utils.GetRequest("ChatCsmall", "post", 3, @"^[1-9]\d*-[1-9]\d*$", "抢币非整点随机值填写格式1-5，不开放抢币请留空");
        int ChatCon = int.Parse(Utils.GetRequest("ChatCon", "post", 2, @"^[0-9]\d*$", "循环时间必须为数字，不开放抢币请填写0"));
        int Paixu = int.Parse(Utils.GetRequest("Paixu", "post", 2, @"^[0-9]\d*$", "排序必须为数字"));
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "创建时间格式填写出错,正确格式如" + DT.FormatDate(DateTime.Now, 0) + ""));
        string sExTime = Utils.GetRequest("ExTime", "post", 1, "", "");
        DateTime ExTime = DateTime.Parse("1990-01-01 00:00:00");
        if (sExTime != "0")
            ExTime = Utils.ParseTime(Utils.GetRequest("ExTime", "post", 2, DT.RegexTime, "过期时间填写错误，永不过期请填写0"));

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
        BCW.Model.Chat m = new BCW.BLL.Chat().GetChat(id);
        if (m == null)
        {
            Utils.Error("不存在的红包群", "");
        }
        if (UsID > 0)
        {
            if (!new BCW.BLL.User().Exists(UsID))
            {
                Utils.Error("不存在的创建ID", "");
            }
        }
        if (Types == 3 && sExTime == "0")
        {
            Utils.Error("选择自建类型时，过期时间不能填写0", "");
        }
        string getChatPwd = new BCW.BLL.Chat().GetChatPwd(id);
        if (getChatPwd != ChatPwd)
        {
            new BCW.BLL.Chat().UpdatePwdID(id, "");//清空进入的ID标识
        }
        //抢币标识
        string sCTemp = string.Empty;
        if (ChatCT != m.ChatCT)
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
            sCTemp = m.ChatCId;

        BCW.Model.Chat model = new BCW.Model.Chat();
        model.ID = id;
        model.ChatName = ChatName;
        model.ChatNotes = ChatNotes;
        model.ChatSZ = ChatSZ;
        model.ChatJS = ChatJS;
        model.ChatLG = ChatLG;
        model.ChatFoot = ChatFoot;
        model.ChatCent = ChatCent;
        model.CentPwd = CentPwd;
        model.ChatTopLine = ChatTopLine;
        model.ChatScore = ChatScore;
        model.UsID = UsID;
        model.GroupId = GroupId;
        model.Types = Types;
        model.ChatPwd = ChatPwd;
        model.ChatCT = ChatCT;
        model.ChatCbig = ChatCbig;
        model.ChatCsmall = ChatCsmall;
        model.ChatCId = sCTemp;//初始化抢币
        model.ChatCon = ChatCon;
        model.Paixu = Paixu;
        model.AddTime = AddTime;
        model.ExTime = ExTime;
        new BCW.BLL.Chat().Update(model);
        Utils.Success("编辑红包群", "编辑红包群成功..", Utils.getUrl("chat.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除红包群";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此红包群吗.删除同时将会删除此红包群的聊天记录和日志.");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Chat().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Chat().Delete(id);
            new BCW.BLL.ChatText().DeleteStr2("ChatId=" + id + "");
            new BCW.BLL.ChatLog().DeleteStr(id);
            new BCW.BLL.Chatblack().DeleteStr(id);
            Utils.Success("删除红包群", "删除红包群成功..", Utils.getPage("chat.aspx"), "1");
        }
    }
    //红包功能管理
    private void HbPage()
    {

        Master.Title = "红包功能管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("红包功能管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ptyge = int.Parse(Utils.GetRequest("ptyge", "all", 1, @"^[0-9]$", "1"));
        string userid = Utils.GetRequest("userid", "all", 1, @"^\d*$", "");
        string hbid = Utils.GetRequest("hbid", "all", 1, @"^\d*$", "");
        if (ptyge == 0)
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hb") + "\">红包设置</a>|");
            builder.Append("红包查看");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("红包设置|");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hb&amp;ptyge=0") + "\">红包查看</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "";
        string[] pageValUrl = { "ptyge", "act", "userid", "hbid", "backurl" };
        if (userid != "")
        {
            strWhere = "UserID=" + userid;
        }
        if (hbid != "" && userid == "")
        {
            strWhere = "ID=" + hbid;
        }
        if (ptyge == 0)
        {
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            IList<BCW.HB.Model.HbPost> listHbGet = new BCW.HB.BLL.HbPost().GetListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHbGet.Count > 0)
            {
                int k = 1;
                foreach (BCW.HB.Model.HbPost n in listHbGet)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string name = new BCW.BLL.User().GetUsName(n.UserID);
                    string sbb = "";
                    if (n.State == 0)
                    {
                        sbb = "待发";
                    }
                    else if (n.State == 2)
                    {
                        sbb = "过期";
                    }
                    else
                    {
                        sbb = "已发";
                    }
                    long sum = n.money;
                    string hyni = "";
                    string biubiu = "";
                    if (n.RadomNum.Trim() == "pt")
                    {
                        sum = n.money * n.num;
                        n.RadomNum = "普通";
                        hyni = "普通";
                        biubiu = n.GetIDList;
                    }
                    else
                    {
                        hyni = "拼手气";
                        if (n.num < 2)
                        {
                            if (n.GetIDList.Trim() != "")
                            {
                                string[] someid = n.GetIDList.Split('#');
                                biubiu = someid[0] + "得" + n.RadomNum + "币";
                            }
                            else
                            {
                                biubiu = n.RadomNum + "币";
                            }

                        }
                        else
                        {
                            string[] kubi = n.RadomNum.Split('#');
                            string[] someid = n.GetIDList.Split('#');
                            int j = 0;
                            for (int i = 0; i < kubi.Length - 1; i++)
                            {
                                if (j < someid.Length - 1)
                                {
                                    biubiu = biubiu + someid[j] + "得" + kubi[i] + "币" + "|";
                                    j++;
                                }
                                else
                                {
                                    biubiu = biubiu + kubi[i] + "币|";
                                }
                            }
                        }
                    }

                    builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=delhb&amp;hbid=" + n.ID) + "\">[删]</a>");
                    builder.Append(n.ID + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + name + "</a>(" + n.UserID + ")" + "的" + hyni + "红包(" + n.ID + ")|总金额" + sum + "|" + n.PostTime + "|[" + sbb + "]|共" + n.num + "个(" + biubiu + ") <br/> ");
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
            string strText = "发红包用户ID：,红包ID:";
            string strName = "userid,hbid";
            string strType = "num,num";
            string strValu = "'";
            string strEmpt = "true,true";
            string strIdea = "/";
            string strOthe = "查询,chat.aspx?act=hb&amp;ptyge=0,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("", "<br />"));
        }
        else
        {
            string xmlPath = "/Controls/chathb.xml";
            ub xml = new ub();
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                string HbOpen = Utils.GetRequest("hbgn", "post", 2, @"^[0-1]$", "红包功能选择出错");
                string HbNumMax = Utils.GetRequest("hbnum", "post", 2, @"^[1-9]\d*$", "单次最多红包个数填写错误");
                string HbMoneyMax = Utils.GetRequest("sqmax", "post", 2, @"^[1-9]\d*$", "手气红包最大金额填写错误");
                string HbMoneyMax2 = Utils.GetRequest("ptmax", "post", 2, @"^[1-9]\d*$", "普通红包最大金额填写错误");
                string HbMoneyMin = Utils.GetRequest("sqmin", "post", 2, @"^[1-9]\d*$", "手气红包最小金额填写错误");
                string HbMoneyMin2 = Utils.GetRequest("ptmin", "post", 2, @"^[1-9]\d*$", "普通红包最小金额填写错误");
                string HbTime = Utils.GetRequest("hbtime", "post", 2, @"^[1-9]\d*$", "红包过期时间填写错误");
                string HbSpeed = Utils.GetRequest("hbspeed", "post", 2, @"^[1-9]\d*$", "红包滚动速度填写错误");
                xml.dss["HbOpen"] = HbOpen;
                xml.dss["HbNumMax"] = HbNumMax;
                xml.dss["HbMoneyMax"] = HbMoneyMax;
                xml.dss["HbMoneyMax2"] = HbMoneyMax2;
                xml.dss["HbMoneyMin"] = HbMoneyMin;
                xml.dss["HbMoneyMin2"] = HbMoneyMin2;
                xml.dss["HbTime"] = HbTime;
                xml.dss["HbSpeed"] = HbSpeed;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("红包群红包设置", "设置成功，正在返回..", Utils.getUrl("chat.aspx?act=hb"), "1");
            }
            else
            {
                string strText = "红包功能/,单次最多红包个数/,手气红包最小金额/,手气红包最大金额/,普通红包最小金额/,普通红包最大金额/,红包过期时间(天)/,红包滚动速度(越小滚动越快，50为正常速度)/,";
                string strName = "hbgn,hbnum,sqmin,sqmax,ptmin,ptmax,hbtime,hbspeed,backurl";
                string strType = "select,num,num,num,num,num,num,num,hidden";
                string strValu = xml.dss["HbOpen"] + "'" + xml.dss["HbNumMax"] + "'" + xml.dss["HbMoneyMin"] + "'" + xml.dss["HbMoneyMax"] + "'" + xml.dss["HbMoneyMin2"] + "'" + xml.dss["HbMoneyMax2"] + "'" + xml.dss["HbTime"] + "'" + xml.dss["HbSpeed"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|开启|1|关闭,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,chat.aspx?act=hb,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">返回红包群管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //删除页面
    private void DelHb()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("hbid", "get", 2, @"^[1-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除红包";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该红包吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?info=ok&amp;act=delhb&amp;hbid=" + id) + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx?act=hb&amp;ptyge=0") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.HB.BLL.HbPost().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            BCW.HB.Model.HbPost modell = new BCW.HB.BLL.HbPost().GetModel(id);
            List<BCW.HB.Model.HbGetNote> hbnote = new BCW.HB.BLL.HbGetNote().GetModelList("HbID=" + id);
            long symoney = 0;
            for (int iso = 0; iso < hbnote.Count; iso++)
            {
                new BCW.HB.BLL.HbGetNote().Delete(hbnote[iso].ID);
                symoney = symoney + hbnote[iso].GetMoney;
            }
            new BCW.HB.BLL.HbPost().Delete(id);
            string postname = new BCW.BLL.User().GetUsName(modell.UserID);
            new BCW.BLL.User().UpdateiGold(modell.UserID, symoney, "错误红包返还");
            new BCW.BLL.Guest().Add(1, modell.UserID, postname, "您的错误红包剩余酷币已返还，获得了" + symoney + "酷币。");
            Utils.Success("删除红包", "删除成功..", Utils.getPage("chat.aspx?act=hb"), "1");
        }
    }
}
