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

public partial class Manage_diary : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/diary.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "edit":
                EditPage();
                break;
            case "editsave":
                EditSavePage();
                break;
            case "del":
                DelPage();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "日记管理";
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (uid > 0)
            builder.Append("ID:" + uid + "");

        builder.Append("日记管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "UsID=" + uid + "";

        strOrder = "ID Desc";
        // 开始读取列表
        IList<BCW.Model.Diary> listDiary = new BCW.BLL.Diary().GetDiarys(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listDiary.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Diary n in listDiary)
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
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=edit&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.Title + "</a>(" + DT.FormatDate(n.AddTime, 2) + ")");
                builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>");
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
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜日记,diary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void EditPage()
    {
        Master.Title = "编辑日记";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "日记ID错误"));
        BCW.Model.Diary model = new BCW.BLL.Diary().GetDiary(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("编辑日记");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "日记标题:/,日记内容:/,日记天气:/,用户ID:/,用户昵称:/,是否置顶:/,人气:/,添加IP:/,添加时间:/,,";
        string strName = "Title,Content,Weather,UsID,UsName,IsTop,ReadNum,AddUsIP,AddTime,id,act";
        string strType = "text,textarea,text,num,text,select,num,text,text,hidden,hidden";
        string strValu = "" + model.Title + "'" + model.Content + "'" + model.Weather + "'" + model.UsID + "'" + model.UsName + "'" + model.IsTop + "'" + model.ReadNum + "'" + model.AddUsIP + "'" + DT.FormatDate(model.AddTime, 0) + "'" + id + "'editsave";
        string strEmpt = "false,false,false,false,false,0|非置顶|1|置顶,false,false,false,false";
        string strIdea = "/";
        string strOthe = "确定编辑,diary.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append(" <a href=\"" + Utils.getPage("diary.aspx") + "\">取消</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("diary.aspx") + "\">日记管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void EditSavePage()
    {
        int TitleNum = Utils.ParseInt(ub.GetSub("DiaryTitleNum", xmlPath));
        int ContentNum = Utils.ParseInt(ub.GetSub("DiaryContentNum", xmlPath));
        int id = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "日记ID错误"));
        string Title = Utils.GetRequest("Title", "post", 2, @"^[^\^]{1," + TitleNum + "}$", "标题限1-" + TitleNum + "字");
        string Content = Utils.GetRequest("Content", "post", 2, @"^[^\^]{1," + ContentNum + "}$", "请输入不超" + ContentNum + "的内容");
        string Weather = Utils.GetRequest("Weather", "post", 2, @"^[^\^]{1,5}$", "天气限1-5字");
        int UsID = int.Parse(Utils.GetRequest("UsID", "post", 2, @"^[1-9]\d*$", "用户ID错误"));
        string UsName = Utils.GetRequest("UsName", "post", 2, @"^[^\^]{1,50}$", "昵称不超50字");
        int IsTop = int.Parse(Utils.GetRequest("IsTop", "post", 2, @"^[0-1]$", "置顶选择错误"));
        //int IsGood = int.Parse(Utils.GetRequest("IsGood", "post", 2, @"^[0-1]$", "精华选择错误"));
        int ReadNum = int.Parse(Utils.GetRequest("ReadNum", "post", 2, @"^[0-9]\d*$", "人气填写错误"));
        string AddUsIP = Utils.GetRequest("AddUsIP", "post", 1, "", "");
        if (!Ipaddr.IsIPAddress(AddUsIP))
        {
            Utils.Error("IP填写错误", "");
        }
        DateTime AddTime = Utils.ParseTime(Utils.GetRequest("AddTime", "post", 2, DT.RegexTime, "添加时间填写出错"));
        BCW.Model.Diary model = new BCW.Model.Diary();
        model.ID = id;
        model.Title = Title;
        model.Content = Content;
        model.Weather = Weather;
        model.UsID = UsID;
        model.UsName = UsName;
        model.IsTop = IsTop;
        model.IsGood = 0;
        model.ReadNum = ReadNum;
        model.AddUsIP = AddUsIP;
        model.AddTime = AddTime;
        new BCW.BLL.Diary().Update2(model);

        Utils.Success("编辑日记", "编辑日记成功..", Utils.getUrl("diary.aspx?act=edit&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        if (info != "ok")
        {
            Master.Title = "删除日记";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("不能恢复！确定删除此日记吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("diary.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("diary.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Diary().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Diary().Delete(id);
            new BCW.BLL.FComment().Delete(0, id);
            Utils.Success("删除日记", "删除日记成功..", Utils.getPage("diary.aspx"), "1");
        }
    }
}
