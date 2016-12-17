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

public partial class Manage_BackMoreQuestion : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "del":
                DelQuestion();
                break;
            case "addquestion":
                AddQuestion();
                break;
            case "resetques":
                ResetQuestion();
                break;
            case "gocleanbyid":
                GoCleanByID();
                break;
            case "cleanbyid":
                CleanByID();
                break;
            case "change":
                Change();
                break;
            case "shuoming":
                Shuoming();
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
      //  builder.Append("<a href=\"" + Utils.getUrl("/Manage/BackMoreQuestion.aspx?act=gocleanbyid") + "\">这是按ID手机号重置问题密码</a><br/>");
        //builder.Append("<a href=\"" + Utils.getUrl("/Manage/BackMoreQuestion.aspx?act=resetques&amp;hid=" + hid + "") + "\">重置密码保护问题答案(手机后六位)</a><br />");
        //builder.Append("<a href=\"" + Utils.getUrl("/Manage/BackMoreQuestion.aspx?act=resetques&amp;info=ok&amp;hid=" + hid + "") + "\">清除该账号密保数据（需用户重设）</a><br />");
       // builder.Append("<a href=\"" + Utils.getUrl("/Manage/BackMoreQuestion.aspx?act=resetques&amp;hid=" + 1 + "") + "\">重置密码保护问题答案(手机后六位)</a><br />");
       // builder.Append("<a href=\"" + Utils.getUrl("/Manage/BackMoreQuestion.aspx?act=resetques&amp;info=ok&amp;hid=" + 1 + "") + "\">清除该账号密保数据（需用户重设）</a><br />");
        int id = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int pageIndex;    //第几页
        int recordCount;   //记录的总条数
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));//一页显示多少条数据
        string strWhere = string.Empty;
        if (id > 0)
            strWhere += "id=" + id + "";

        string[] pageValUrl = { "act", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);   //跳到第几页
        if (pageIndex == 0)
            pageIndex = 1;

        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().Gettb_Helps(pageIndex, pageSize, strWhere, out recordCount);
        if (listHelp.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.tb_Help n in listHelp)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=del&amp;id=" + (n.ID) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]&gt;</a>");
                builder.Append(((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=shuoming&amp;id=" + n.ID + "") + "\">" + n.Title + "</a>");
                builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=change&amp;id=" + n.ID) + "\">...&gt;&gt;[编辑]&gt;</a>");
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }
        string strText = "输入问题ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜问题ID,BackMoreQuestion.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append("<br/><a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=addquestion") + "\">[增加问题]<br/></a>");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void GoCleanByID()
    {

        string strText = "ID或手机号:/,";
        string strName = "uid,act";
        string strType = "num,hidden";
        string strValu = "'cleanbyid";
        string strEmpt = "false,false";
        string strIdea = "/";
        string strOthe = "删除密保,BackMoreQuestion.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

    }
    private void CleanByID()
    {
        string uid = Utils.GetRequest("uid", "all", 2, @"^[\d]{1,11}$", "请输入正确ID或手机号码");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (uid.Length != 11)
        {
            uid = new BCW.BLL.User().GetMobile(Convert.ToInt32(uid));//转换手机号码
        }
        int id = new BCW.BLL.User().GetID(uid);//得到ID号
        if (new BCW.BLL.User().Exists(id))
        {
            if (info == "")
            {
                if (new BCW.BLL.tb_Question().Exists(uid))
                {
                    builder.Append(Out.Tab("<div class=\"title\">", ""));
                    builder.Append("确定删除该密保吗？");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=cleanbyid&amp;info=ok&amp;uid=" + uid + "") + "\">确定删除</a><br />");
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">先留着吧..</a>");  //需要修改链接
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
                else
                {
                    Utils.Success("温馨提示", "该用户没有设置密码保护问题!", Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + ""), "3");
                }
            }
            else
            {
                //string str = uid.Substring(5, 6);
                //new BCW.BLL.tb_Question().UpdateAnswer(str, uid);
                //builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                //Utils.Success("温馨提示", "重置密码保护问题答案成功！ID是" + id + "..手机号是：" + uid + "..新问题密码是：" + str, Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + ""), "3");
                //builder.Append(Out.Tab("</div>", "<br />"));
                new BCW.BLL.tb_Question().Delete(uid);
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                Utils.Success("温馨提示", "成功清除该ID：" + id + "..手机号是：" + uid + "的密保数据，用户可以自行重新设置密码保护问题！", Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + ""), "3");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        else
        {
            Utils.Success("温馨提示", "不存在该账号", Utils.getUrl("uinfo.aspx"), "3");
        }
    }
    private void ResetQuestion()
    {
        string meid = Utils.GetRequest("hid", "all", 2, "", "");
        string info = Utils.GetRequest("info", "all", 0, "", "");
        meid = new BCW.BLL.User().GetMobile(int.Parse(meid));//转换手机号码
        int id = new BCW.BLL.User().GetID(meid);//得到ID号   
        if (info == "")
        {
            if (new BCW.BLL.tb_Question().Exists(meid))
            {
                string str = meid.Substring(5, 6);
                new BCW.BLL.tb_Question().UpdateAnswer(str, meid);
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                Utils.Success("温馨提示", "重置密码保护问题答案成功！ ID是" + id + "..手机号是：" + meid + "..新问题密码是：" + str, Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + ""), "3");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                Utils.Success("温馨提示", "不存在该账号密码保护问题数据", Utils.getUrl("uinfo.aspx"), "3");
            }
        }
        else
        {
            if (new BCW.BLL.tb_Question().Exists(meid))
            {

                new BCW.BLL.tb_Question().Delete(meid);
                builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
                Utils.Success("温馨提示", "成功清除该ID：" + id + "..手机号是：" + meid + "的密保数据，用户可以自行重新设置密码保护问题！", Utils.getUrl("uinfo.aspx?act=view&amp;uid=" + id + ""), "3");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                Utils.Success("温馨提示", "不存在该账号密码保护问题数据", Utils.getUrl("uinfo.aspx"), "3");
            }

        }
    }
    private void Change()
    {
        //builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        //builder.Append("这里是编辑修改原本的问题说明的：" + "<br/>");
        //builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx") + "\">返回上一级</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {

            int id = Convert.ToInt32(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
            BCW.Model.tb_Help help = new BCW.BLL.tb_Help().Gettb_Help(id);
            string Title = help.Title;
            string Explain = help.Explain;
            string LinkName = help.LinkName;
            int hasLink = help.HasLink;
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("你的ID 是：" + help.ID + "<br/>");
            builder.Append("你原本的问题题目是：" + help.Title + "<br/>");
            builder.Append("你原本的问题解释是：" + help.Explain + "<br/>");
            builder.Append("你原本的链接名字是：" + help.LinkName + "<br/>");
            builder.Append("你原来是否有链接的( 1：代表有， 0：代表没有 )：" + help.HasLink + "<br/>");
            builder.Append(Out.Tab("</div>", "<br />"));
            string strText = "输入新的问题标题:/,输入新的问题解释:/,输入新的链接:";
            string strName = "Title,Explain,LinkName";
            string strType = "textarea,big,select";
            string strValu = "''";
            string strEmpt = "false,false,null|null|GetPwd.aspx|GetPwd.aspx";
            string strIdea = "/";
            string strOthe = "提交,BackMoreQuestion.aspx?act=change&amp;info=ok&amp;id=" + help.ID + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append("<br/>");
        }
        else
        {
            string Title = Utils.GetRequest("Title", "post", 1, "", "");
            string Explain = Utils.GetRequest("Explain", "post", 1, "", "");
            string LinkName = Utils.GetRequest("LinkName", "post", 1, "", "");

            Explain = Explain.Replace("\n","<br/>");

            BCW.Model.tb_Help objHelp = new BCW.Model.tb_Help();
            objHelp.Title = Title;
            objHelp.Explain = Explain;
            if (!LinkName.Equals("null"))
            {
                objHelp.LinkName = LinkName;
                objHelp.HasLink = 1;
            }
            else
            {
                objHelp.HasLink = 0;
                objHelp.LinkName = "null";
            }
            objHelp.ID = Convert.ToInt32(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
            new BCW.BLL.tb_Help().Update(objHelp);
            Utils.Success("修改问题", "修改成功..", Utils.getPage("BackMoreQuestion.aspx"), "3");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx") + "\">返回上一级</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void AddQuestion()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("添加问题说明" + "<br />");
        builder.Append(Out.Tab("</div>", "" + ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "")
        {

            string strText = "输入问题名称:/,输入问题的解释:/,输入的链接:";
            string strName = "Title,Explain,LinkName";
            string strType = "textarea,big,select";
            string strValu = "''";
            string strEmpt = "false,false,null|null|brag.aspx|brag.aspx|GetPwd.aspx|GetPwd.aspx";
            string strIdea = "/";
            string strOthe = "添加问题,BackMoreQuestion.aspx?act=addquestion&amp;info=ok,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string Title = Utils.GetRequest("Title", "post", 1, "", "");
            string Explain = Utils.GetRequest("Explain", "post", 1, "", "");
            string LinkName = Utils.GetRequest("LinkName", "post", 1, "", "");

            Explain = Explain.Replace("\n", "<br/>");
            BCW.Model.tb_Help objHelp = new BCW.Model.tb_Help();
            objHelp.Title = Title;
            objHelp.Explain = Explain;
            if (!LinkName.Equals("null"))
            {
                objHelp.LinkName = LinkName;
                objHelp.HasLink = 1;
            }
            else
            {
                objHelp.HasLink = 0;
                objHelp.LinkName = "null";
            }
            int maxid = new BCW.BLL.tb_Help().GetMaxId();
            int i;
            for (i = 1; i <= maxid; i++)
            {
                if (!new BCW.BLL.tb_Help().Exists(i))
                {
                    objHelp.ID = i;
                    break;
                }
            }
            if (i == maxid + 1)
            {
                objHelp.ID = (maxid + 1);
            }
            new BCW.BLL.tb_Help().Add(objHelp);
            Utils.Success("添加问题", "添加成功..", Utils.getPage("BackMoreQuestion.aspx"), "3");
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx") + "\">返回上一级</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void Shuoming()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("./default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        int id = Convert.ToInt32(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        BCW.Model.tb_Help help = new BCW.BLL.tb_Help().Gettb_Help(id);

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>" + help.Title + "</b><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append(help.Explain + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        otherhelp();
        builder.Append(Out.Tab("<div class=\"title\">", "<br/>"));
        builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx") + "\">返回上一级</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    private void DelQuestion()
    {
        //builder.Append("当前地址" + Utils.PostPage(1)+"<br />");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        //    builder.Append("当前ID=" + id);
        if (info == "")
        {
            Master.Title = "删除问题";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?info=ok&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.tb_Help().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.tb_Help().Delete(id);
            Utils.Success("删除问题", "删除成功..", Utils.getPage("brag.aspx"), "1");
        }
    }
    public void otherhelp()
    {
        int SizeNum = 3;
        string strWhere = "";
        IList<BCW.Model.tb_Help> listHelp = new BCW.BLL.tb_Help().GetHelps(SizeNum, strWhere);
        if (listHelp.Count > 0)
        {
            builder.Append(Out.Div("div", "其他相关帮助.." + "<br/>"));

            int k = 1;
            foreach (BCW.Model.tb_Help n in listHelp)
            {
                if (k == 1)
                    builder.Append(Out.Tab("<div>", ""));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("BackMoreQuestion.aspx?act=shuoming&amp;id=" + n.ID + "") + "\">" + n.Title + "</a>");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }

        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

    }
}
