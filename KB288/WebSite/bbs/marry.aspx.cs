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
using System.Reflection;
using BCW.Common;
using BCW.Files;

public partial class bbs_marry : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/marry.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //维护提示
        if (ub.GetSub("MarryStatus", xmlPath) == "1")
        {
            Utils.Safe("婚姻系统");
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "love":
                LovePage();
                break;
            case "yes":
                YesPage();
                break;
            case "no":
                NoPage();
                break;
            case "yes2":
                Yes2Page();
                break;
            case "no2":
                No2Page();
                break;
            case "yes3":
                Yes3Page();
                break;
            case "no3":
                No3Page();
                break;
            case "lovesave":
                LoveSavePage();
                break;
            case "lost":
            case "lost2":
                LostPage(act);
                break;
            case "lostinfo":
            case "lostinfo2":
                LostInfoPage(act);
                break;
            case "list":
                MarryListPage();
                break;
            case "marry":
                MarryPage();
                break;
            case "view":
                MarryViewPage();
                break;
            case "mylove":
                MyLovePage();
                break;
            case "loveinfo":
                LoveInfoPage();
                break;
            case "home":
                HomePage();
                break;
            case "marrybook":
                MarryBookPage();
                break;
            case "marryaction":
                MarryActionPage();
                break;
            case "marryphoto":
                MarryPhotoPage();
                break;
            case "marryflow":
                MarryFlowPage();
                break;
            case "marrytop":
                MarryTopPage();
                break;
            //case "marrypk":
            //    MarryPkPage();
            //    break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {

        Master.Title = "婚姻殿堂";
        string Logo = ub.GetSub("MarryLogo", xmlPath);
        string Notes = ub.GetSub("MarryNotes", xmlPath);

        if (Logo != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<img src=\"" + Logo + "\" alt=\"load\"/>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        if (Notes != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(Out.SysUBB(Notes));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=我的乐园=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        //取我的恋爱记录
        int meid = new BCW.User.Users().GetUsId();
        if (meid > 0)
        {
            DataSet ds = new BCW.BLL.Marry().GetList("ID,Types", "(UsID=" + meid + " OR ReID=" + meid + ") and (Types=0 OR Types=1)");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int Types = int.Parse(ds.Tables[0].Rows[0]["Types"].ToString());
                if (Types == 0)
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=mylove") + "\">我的恋爱乐园</a>");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home") + "\">我的浪漫花园</a>");
            }
            else
            {
                builder.Append("还没恋爱呢!");
                builder.Append("<br /><a href=\"" + Utils.getUrl("/bbs/forum.aspx?forumid=77") + "\">婚姻介绍论坛</a>");
            }
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=婚姻服务=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=love") + "\">我要谈恋爱</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=lost") + "\">我要离婚</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=lost2") + "\">我要强制离婚</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=服务记录=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=list&amp;ptype=0") + "\">正在求婚记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=list&amp;ptype=2") + "\">已离婚记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=list&amp;ptype=1") + "\">婚姻见证处</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=list&amp;ptype=3") + "\">正在热恋的情侣</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryaction&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看婚恋动态&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("花园:<a href=\"" + Utils.getUrl("marry.aspx?act=marrytop&amp;ptype=1") + "\">人气花园</a>-");
        builder.Append("<a href=\"" + Utils.getPage("marry.aspx?act=marrytop&amp;ptype=2") + "\">鲜花排行</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LovePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //自动清空48小前的过期请求
        new BCW.BLL.Marry().Delete("AddTime<'" + DateTime.Now.AddHours(-48) + "' and Types=-1");

        Master.Title = "我要恋爱";
        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 1, @"^[1-9]\d*$", "0"));

        if (ReID > 0)
        {
            if (!new BCW.BLL.User().Exists(ReID))
            {
                Utils.Error("不存在的会员", "");
            }
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("我要与<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ReID + "&amp;backurl==" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(ReID) + "(" + ReID + ")</a>恋爱");
            builder.Append(Out.Tab("</div>", ""));
            string strText = ",愿你真挚的言语能融化TA冰封的心:/,";
            string strName = "ReID,Content,act";
            string strType = "hidden,textarea,hidden";
            string strValu = "" + ReID + "''lovesave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "我要示爱|reset,marry.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("我要恋爱");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "输入对方ID:/,愿你真挚的言语能融化TA冰封的心:/,";
            string strName = "ReID,Content,act";
            string strType = "num,textarea,hidden";
            string strValu = "''lovesave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "我要示爱|reset,marry.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("如果对方48小时内不作回应，系统将自动撤销你的请求<br />查看");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marry") + "\">我的示爱&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LoveSavePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "post", 2, @"^[1-9]\d*$", "对方ID错误"));
        string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,300}$", "真挚的言语限1-300字");
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (ReID == meid)
        {
            Utils.Error("你想搞自恋啊，不能自己与自己示爱哟", "");
        }
        int UsSex = new BCW.BLL.User().GetSex(meid);
        int ReSex = new BCW.BLL.User().GetSex(ReID);
        if (UsSex == 0)
        {
            Utils.Error("请先设置您的性别！<a href=\"" + Utils.getUrl("myedit.aspx?act=basic&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上设置</a>", "");
        }
        if (ReSex == 0)
        {
            Utils.Error("对方还没有设置性别", "");
        }
        if (UsSex == ReSex)
        {
            Utils.Error("很抱歉，同性不能恋爱哦", "");
        }
        if (new BCW.BLL.Friend().Exists(ReID, meid, 1))
        {
            Utils.Error("很抱歉，对方已将你拉进黑名单", "");
        }
        if (new BCW.BLL.Marry().ExistsMarry(ReID))
        {
            Utils.Error("很遗憾，对方已名花有主咯", "");
        }
        if (new BCW.BLL.Marry().Exists(meid, ReID, -1))
        {
            Utils.Error("您已发送请求，请耐心等待对方回应", "");
        }
        if (new BCW.BLL.Marry().Exists(ReID, meid, -1))
        {
            Utils.Error("对方已给您示爱<a href=\"" + Utils.getUrl("marry.aspx?act=marry") + "\">查看</a>", "");
        }
        if (new BCW.BLL.Marry().ExistsLostMarry(meid))
        {
            Utils.Error("重复示爱", "");
        }
        int IsVerify = new BCW.BLL.User().GetIsVerify(meid);
        if (IsVerify == 0)
        {
            Utils.Error("您属于手工注册会员，还未通过短信验证<br /><a href=\"/reg.aspx\">免费验证会员</a>", "");
        }
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        BCW.Model.Marry model = new BCW.Model.Marry();
        model.Types = -1;
        model.UsID = meid;
        model.UsName = mename;
        model.ReID = ReID;
        model.ReName = rename;
        model.UsSex = UsSex;
        model.ReSex = ReSex;
        model.IsParty = 0;
        model.Oath = "";
        model.AddTime = DateTime.Now;
        model.AcUsID = 0;
        model.State = 0;
        int id = new BCW.BLL.Marry().Add(model);

        //内线通知
        new BCW.BLL.Guest().Add(ReID, rename, "恭喜，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向你示爱啦![br]肉麻情话：" + Content + "[br]如果您接受，你们将立即成为恋人哦![url=/bbs/marry.aspx?act=yes&amp;ReID=" + meid + "]接受请求[/url]|[url=/bbs/marry.aspx?act=no&amp;ReID=" + meid + "]拒绝[/url]");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]提出[url=/bbs/marry.aspx]示爱啦[/url]";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = id;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);

        Utils.Success("我要恋爱", "示爱成功，请等待对方回应..", Utils.getUrl("marry.aspx"), "2");
    }

    private void MarryPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "我的示爱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=我的示爱=");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        DataSet ds = new BCW.BLL.Marry().GetList("UsID,UsName,ReID,ReName", "(UsID=" + meid + " OR ReID=" + meid + ") and Types=-1");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
            int ReID = int.Parse(ds.Tables[0].Rows[0]["ReID"].ToString());
            string UsName = ds.Tables[0].Rows[0]["UsName"].ToString();
            string ReName = ds.Tables[0].Rows[0]["ReName"].ToString();

            builder.AppendFormat("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}({0})</a>", UsID, UsName);

            string aText = "";
            if (ReID == meid)
            {
                aText += "<a href=\"" + Utils.getUrl("marry.aspx?act=yes&amp;ReID=" + UsID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">[同意]</a>";
                aText += "|<a href=\"" + Utils.getUrl("marry.aspx?act=no&amp;ReID=" + UsID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">[拒绝]</a>";
            }
            builder.AppendFormat("向<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}({0})</a>示爱啦~~" + aText + "", ReID, ReName);
        }
        else
        {
            builder.Append("没有相关记录..");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));

    }
    private void YesPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        DataSet ds = new BCW.BLL.Marry().GetList("id", "UsID=" + ReID + " and ReID=" + meid + " and Types=-1");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("不存在的请求记录", "");
        }
        int id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());

        //成为恋人
        new BCW.BLL.Marry().UpdateLove(ReID, meid);
        //内线通知
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        new BCW.BLL.Guest().Add(ReID, rename, "恭喜，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]接受您的示爱啦!你们已成为恋人![url=/bbs/marry.aspx?act=mylove]进入恋爱乐园[/url]");

        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]接受[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]的[url=/bbs/marry.aspx]示爱啦[/url]，两人成为恋人";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = id;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);

        Utils.Success("成为恋人", "操作成功，恭喜你与" + rename + "已成为恋人!<br /><a href=\"" + Utils.getUrl("marry.aspx?act=mylove") + "\">马上进入恋爱乐园</a>", Utils.getUrl("marry.aspx?act=love"), "2");
    }

    private void NoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        if (!new BCW.BLL.Marry().Exists(ReID, meid, -1))
        {
            Utils.Error("不存在的请求记录", "");
        }
        //删除记录
        new BCW.BLL.Marry().Delete("UsID=" + ReID + " and ReID=" + meid + "");
        //内线通知
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        new BCW.BLL.Guest().Add(ReID, rename, "很遗憾，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经拒绝您的示爱!");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]拒绝[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]的[url=/bbs/marry.aspx]示爱[/url]，很遗憾";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = 0;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);

        Utils.Success("我要拒绝", "操作成功，已拒绝成为恋人!", Utils.getUrl("marry.aspx"), "2");
    }

    private void Yes2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        //if (!new BCW.BLL.Marry().Exists3(ReID, meid, 1))
        //{
        //    Utils.Error("不存在的请求记录", "");
        //}
        DataSet ds = new BCW.BLL.Marry().GetList("id", "Types=0 and State=1 and ((UsID=" + ReID + " and ReID=" + meid + ") or (UsID=" + meid + " and ReID=" + ReID + "))");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("不存在的请求记录", "");
        }
        int id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());

        if (new BCW.BLL.Marry().ExistsMarry(meid))
        {
            Utils.Error("很抱歉，请遵守国策，禁止重婚", "");
        }
        if (new BCW.BLL.Marry().ExistsMarry(ReID))
        {
            Utils.Error("很遗憾，对方已名花有主咯", "");
        }

        //收取登记费
        long Price = Convert.ToInt64(ub.GetSub("MarryPrice", xmlPath));
        long gold = new BCW.BLL.User().GetGold(ReID);
        if (gold < Price)
        {
            Utils.Error("对方自带" + ub.Get("SiteBz") + "不足<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx") + "\">购买礼物送" + ub.Get("SiteBz") + "&gt;&gt;</a>" + Price + "，无法登记结婚", "");
        }
        //扣币
        string rename = new BCW.BLL.User().GetUsName(ReID);
        new BCW.BLL.User().UpdateiGold(ReID, rename, -Price, "结婚登记费");

        //成为夫妻
        new BCW.BLL.Marry().UpdateMarry(ReID, meid);
        //内线通知
        string mename = new BCW.BLL.User().GetUsName(meid);
        new BCW.BLL.Guest().Add(ReID, rename, "恭喜，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]接受您的求婚啦!你们已成为夫妻!系统收取您的" + Price + "" + ub.Get("SiteBz") + "作为登记费![url=/bbs/marry.aspx?act=home]进入花园[/url]");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]接受[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]的[url=/bbs/marry.aspx]求婚啦[/url],有情人终成眷属!";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = id;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);

        Utils.Success("成为夫妻", "操作成功，恭喜你与" + rename + "已成为夫妻!<br /><a href=\"" + Utils.getUrl("marry.aspx?act=home") + "\">马上进入花园</a>", Utils.getUrl("marry.aspx?act=home"), "2");
    }

    private void No2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        //if (!new BCW.BLL.Marry().Exists3(ReID, meid, 1))
        //{
        //    Utils.Error("不存在的请求记录", "");
        //}

        DataSet ds = new BCW.BLL.Marry().GetList("id", "Types=0 and State=1 and ((UsID=" + ReID + " and ReID=" + meid + ") or (UsID=" + meid + " and ReID=" + ReID + "))");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("不存在的请求记录", "");
        }
        int id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());

        // 取消求婚请求
        new BCW.BLL.Marry().UpdateMarry2(ReID, meid);
        //内线通知
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        new BCW.BLL.Guest().Add(ReID, rename, "很遗憾，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经拒绝您的求婚!");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]拒绝[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]的[url=/bbs/marry.aspx]求婚[/url],很遗憾!";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = id;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);
        Utils.Success("我要拒绝", "操作成功，已拒绝成为夫妻!", Utils.getUrl("marry.aspx"), "2");
    }

    private void LostPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string sTitle = string.Empty;
        if (act == "lost2")
            sTitle = "强制离婚";
        else
            sTitle = "离婚";

        Master.Title = "我要" + sTitle + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("我要" + sTitle + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = "Types=1 and (UsID=" + meid + " or ReID=" + meid + ")";

        // 开始读取列表
        IList<BCW.Model.Marry> listMarry = new BCW.BLL.Marry().GetMarrys(pageIndex, pageSize, strWhere, out recordCount);
        if (listMarry.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Marry n in listMarry)
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

                string aText = string.Empty;
                if (act == "lost2")
                    aText = "<a href=\"" + Utils.getUrl("marry.aspx?act=lostinfo2&amp;ReID={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">[强制离婚]</a>";
                else
                    aText = "<a href=\"" + Utils.getUrl("marry.aspx?act=lostinfo&amp;ReID={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">[离婚]</a>";

                if (n.UsID == meid)
                    builder.AppendFormat("配偶:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}({0})</a>" + aText + "", n.ReID, n.ReName);
                else
                    builder.AppendFormat("配偶:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={0}&amp;backurl=" + Utils.getPage(1) + "") + "\">{1}({0})</a>" + aText + "", n.UsID, n.UsName);

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }

            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

        }
        else
        {
            builder.Append(Out.Div("div", "请先结婚再提出离婚.."));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));

    }

    private void LostInfoPage(string act)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        string sTitle = string.Empty;
        if (act == "lostinfo2")
            sTitle = "强制";

        int ReID = int.Parse(Utils.GetRequest("ReID", "all", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }
        if (!new BCW.BLL.Marry().Exists2(meid, ReID, 1))
        {
            Utils.Error("请先结婚再进行离婚..", "");
        }
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            string mename = new BCW.BLL.User().GetUsName(meid);
            string rename = new BCW.BLL.User().GetUsName(ReID);
            long gold = new BCW.BLL.User().GetGold(meid);
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "离婚原因限1-50字");
            if (act == "lostinfo2")
            {
                long LostPrice2 = Convert.ToInt64(ub.GetSub("MarryLostPrice2", xmlPath));

                if (gold < LostPrice2)
                {
                    Utils.Error("强制离婚需要花费" + LostPrice2 + "" + ub.Get("SiteBz") + "，你的" + ub.Get("SiteBz") + "不足<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx") + "\">购买礼物送" + ub.Get("SiteBz") + "&gt;&gt;</a>", "");
                }
                new BCW.BLL.User().UpdateiGold(meid, mename, -LostPrice2, "强制离婚费");
                //成为离婚
                new BCW.BLL.Marry().UpdateLost(meid, ReID, Content);
                new BCW.BLL.Marry().UpdateLost(meid, ReID);

                new BCW.BLL.Guest().Add(ReID, rename, "嘎嘎，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]使用强制离婚方式与你离婚!你们已不再是夫妻![br]离婚原因：" + Content + "");
                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]使用强制离婚方式与[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]成功[url=/bbs/marry.aspx]离婚[/url],再见亦是朋友!";
                BCW.Model.MarryAction A = new BCW.Model.MarryAction();
                A.MarryId = 0;
                A.Content = wText;
                A.AddTime = DateTime.Now;
                new BCW.BLL.MarryAction().Add(A);

                Utils.Success("强制离婚", "强制离婚成功，花费了" + LostPrice2 + "" + ub.Get("SiteBz") + "", Utils.getUrl("marry.aspx"), "2");
            }
            else
            {
                long LostPrice = Convert.ToInt64(ub.GetSub("MarryLostPrice", xmlPath));
                if (gold < LostPrice)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx") + "\">购买礼物送" + ub.Get("SiteBz") + "&gt;&gt;</a>，如果离婚成功，需要花费" + LostPrice + "" + ub.Get("SiteBz") + "", "");
                }
                new BCW.BLL.Marry().UpdateLost(meid, ReID, Content);
                //内线通知

                new BCW.BLL.Guest().Add(ReID, rename, "嘎嘎，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向你提出离婚啦![br]离婚原因：" + Content + "[br]如果您接受，你们将离婚成功![url=/bbs/marry.aspx?act=yes3&amp;ReID=" + meid + "]接受请求[/url]|[url=/bbs/marry.aspx?act=no3&amp;ReID=" + meid + "]拒绝[/url]");
                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]提出[url=/bbs/marry.aspx]离婚请求了[/url]";
                new BCW.BLL.Action().Add(15, 0, 0, "", wText);
                Utils.Success("我要离婚", "离婚请求成功，请等待对方回应..", Utils.getUrl("marry.aspx"), "2");
            }
        }
        else
        {
            Master.Title = "我要离婚";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("我向<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + ReID + "&amp;backurl==" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(ReID) + "(" + ReID + ")</a>" + sTitle + "离婚");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "," + sTitle + "离婚原因(50字内):/,,";
            string strName = "ReID,Content,act,info";
            string strType = "hidden,textarea,hidden,hidden";
            string strValu = "" + ReID + "''" + act + "'ok";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "我要" + sTitle + "离婚|reset,marry.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("温馨提示:<br />");
            if (act == "lostinfo2")
                builder.Append("强制离婚需要支付" + ub.GetSub("MarryLostPrice2", xmlPath) + "" + ub.Get("SiteBz") + "");
            else
                builder.Append("如果离婚成功，您需要支付" + ub.GetSub("MarryLostPrice", xmlPath) + "" + ub.Get("SiteBz") + "");

            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    private void Yes3Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        if (!new BCW.BLL.Marry().Exists4(ReID, meid, 1))
        {
            Utils.Error("不存在的请求记录", "");
        }
        long LostPrice = Convert.ToInt64(ub.GetSub("MarryLostPrice", xmlPath));
        long gold = new BCW.BLL.User().GetGold(ReID);
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        if (gold < LostPrice)
        {
            new BCW.BLL.Guest().Add(ReID, rename, "很抱歉，您向[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]提出的离婚请求因您帐户无法支付" + LostPrice + "" + ub.Get("SiteBz") + "而导致失败!");
            Utils.Error("对方" + ub.Get("SiteBz") + "不足<br /><a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx") + "\">购买礼物送" + ub.Get("SiteBz") + "&gt;&gt;</a>，离婚还不能成功，离婚需要对方花费" + LostPrice + "" + ub.Get("SiteBz") + "，系统已通过消息内线通知对方该信息", "");
        }
        new BCW.BLL.User().UpdateiGold(ReID, rename, -LostPrice, "离婚手续费");

        //成为离婚
        new BCW.BLL.Marry().UpdateLost(ReID, meid);
        //内线通知
        new BCW.BLL.Guest().Add(ReID, rename, "嘎嘎，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已接受您的离婚请求!再见亦是朋友!");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已接受[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]提出的[url=/bbs/marry.aspx]离婚请求[/url],再见亦是朋友!";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = 0;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);
        Utils.Success("离婚", "离婚成功，你与" + rename + "已不再是夫妻!", Utils.getUrl("marry.aspx"), "2");
    }

    private void No3Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ReID = int.Parse(Utils.GetRequest("ReID", "get", 2, @"^[1-9]\d*$", "对方ID错误"));
        if (!new BCW.BLL.User().Exists(ReID))
        {
            Utils.Error("不存在的会员ID", "");
        }

        //if (!new BCW.BLL.Marry().Exists4(ReID, meid, 1))
        //{
        //    Utils.Error("不存在的请求记录", "");
        //}

        DataSet ds = new BCW.BLL.Marry().GetList("id", "Types=1 and State=1 and ((UsID=" + ReID + " and ReID=" + meid + ") or (UsID=" + meid + " and ReID=" + ReID + "))");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("不存在的请求记录", "");
        }
        int id = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());

        // 取消求婚请求
        new BCW.BLL.Marry().UpdateLost2(ReID, meid);
        //内线通知
        string mename = new BCW.BLL.User().GetUsName(meid);
        string rename = new BCW.BLL.User().GetUsName(ReID);
        new BCW.BLL.Guest().Add(ReID, rename, "很遗憾，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已经拒绝您的离婚请求!");
        //动态
        string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已拒绝[url=/bbs/uinfo.aspx?uid=" + ReID + "]" + rename + "[/url]提出的[url=/bbs/marry.aspx]离婚请求[/url]...";
        BCW.Model.MarryAction A = new BCW.Model.MarryAction();
        A.MarryId = id;
        A.Content = wText;
        A.AddTime = DateTime.Now;
        new BCW.BLL.MarryAction().Add(A);

        Utils.Success("我要拒绝", "操作成功，已成功拒绝离婚!", Utils.getUrl("marry.aspx"), "2");
    }

    private void MarryListPage()
    {

        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-3]$", "0"));
        string sTitle = string.Empty;
        if (ptype == 0)
            sTitle = "正在求婚记录";
        else if (ptype == 1)
            sTitle = "婚姻见证处";
        else if (ptype == 2)
            sTitle = "已离婚记录";
        else
            sTitle = "正在热恋情侣";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=" + sTitle + "=");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (ptype == 0)
            strWhere = "Types=0 and State=1";
        else if (ptype == 1)
            strWhere = "Types=1";
        else if (ptype == 2)
            strWhere = "Types=2";
        else
            strWhere = "Types=0";

        // 开始读取列表
        IList<BCW.Model.Marry> listMarry = new BCW.BLL.Marry().GetMarrys(pageIndex, pageSize, strWhere, out recordCount);
        if (listMarry.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Marry n in listMarry)
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

                builder.Append(((pageIndex - 1) * pageSize + k) + ".");
                string sText = string.Empty;
                if (n.AcUsID == 0 || n.UsID == n.AcUsID)
                    sText = "" + n.UsName + "与" + n.ReName + "";
                else
                    sText = "" + n.ReName + "与" + n.UsName + "";

                if (ptype == 0)
                {
                    if (n.AcUsID == 0 || n.UsID == n.AcUsID)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.UsName + "</a>向");
                        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.ReName + "</a>提出求婚" + DT.FormatDate(n.AcTime, 2) + "");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.ReName + "</a>向");
                        builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.UsName + "</a>提出求婚" + DT.FormatDate(n.AcTime, 2) + "");
                    }

                }
                else if (ptype == 3)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.UsName + "</a>与");
                    builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.ReName + "</a>恋爱啦" + DT.FormatDate(n.AddTime, 2) + "");
                }
                else if (ptype == 1)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + sText + "结婚啦</a>" + DT.FormatDate(n.AcTime, 2) + "");
                }
                else if (ptype == 2)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + sText + "已离婚</a>" + DT.FormatDate(n.AcTime, 2) + "");
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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MarryViewPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (model.Types != 1 && model.Types != 2)
        {
            Utils.Error("不存在的记录", "");
        }
        string sTitle = string.Empty;
        if (model.Types == 1)
            sTitle = "查看结婚";
        else
            sTitle = "查看离婚";

        Master.Title = sTitle;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(sTitle);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        if (model.Types == 1)
        {
            if (model.AcUsID == model.UsID)
            {
                builder.Append("结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
                builder.Append("被结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
            }
            else
            {
                builder.Append("结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
                builder.Append("被结婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
            }
            builder.Append("爱情誓言:" + model.Oath + "<br />");
            builder.Append("接受时间:" + DT.FormatDate(model.AcTime, 3) + "");
        }
        else
        {
            if (model.AcUsID == model.UsID)
            {
                builder.Append("离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
                builder.Append("被离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
            }
            else
            {
                builder.Append("离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>(" + BCW.User.Users.UserSex(model.ReSex) + ")<br />");
                builder.Append("被离婚人:<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>(" + BCW.User.Users.UserSex(model.UsSex) + ")<br />");
            }
            builder.Append("离婚原因:" + model.Oath2 + "<br />");
            builder.Append("离婚时间:" + DT.FormatDate(model.AcTime, 3) + "");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MyLovePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        Master.Title = "恋爱乐园";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("恋爱乐园");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));

        //取我的恋爱记录
        DataSet ds = new BCW.BLL.Marry().GetList("LoveStat", "(UsID=" + meid + " OR ReID=" + meid + ") and Types=0");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("你还没有恋爱呢", "");
        }
        string LoveStat = ds.Tables[0].Rows[0]["LoveStat"].ToString();
        if (LoveStat.Contains("1#1#1#1#1#1#1#1#1#1"))
        {
            builder.Append("祝有情人终成眷属，爱神正在祝福你们哦，还等什么？<br /><a href=\"" + Utils.getUrl("marry.aspx?act=marryinfo") + "\">马上求婚</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=1") + "\">恋爱乐园</a><br />");
        }
        else
        {
            builder.Append("西施、王昭君、貂蝉和杨贵妃，既是我国古代的四大美人，又是代表爱情的爱神。千年以来，正是她们保佑者有情人终成眷属。如果你们想要结成夫妻的话，必须要先得到某一位爱神的祝福哦，还等什么？");
            builder.Append("<br /><b>您一次只能玩一个游戏，请选择:</b><br />");

            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=1") + "\">1.沉鱼</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=2") + "\">2.落雁</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=3") + "\">3.闭月</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=4") + "\">4.羞花</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void LoveInfoPage()
    {

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-9]$", "0"));

        //取我的恋爱记录
        DataSet ds = new BCW.BLL.Marry().GetList("ID,UsID,UsName,UsSex,ReID,ReName,ReSex,AcTime,LoveStat", "(UsID=" + meid + " OR ReID=" + meid + ") and Types=0");
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("你还没有恋爱呢", "");
        }
        int ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
        int ReID = int.Parse(ds.Tables[0].Rows[0]["ReID"].ToString());
        string UsName = ds.Tables[0].Rows[0]["UsName"].ToString();
        string ReName = ds.Tables[0].Rows[0]["ReName"].ToString();
        int UsSex = int.Parse(ds.Tables[0].Rows[0]["UsSex"].ToString());
        int ReSex = int.Parse(ds.Tables[0].Rows[0]["ReSex"].ToString());
        string LoveStat = ds.Tables[0].Rows[0]["LoveStat"].ToString();
        string[] LoveTemp = LoveStat.Split("#".ToCharArray());

        //浇心开始
        if (p == 1)
        {
            int pt = 0;
            int mySex = 0;
            DateTime dt = DateTime.Now;
            if (UsID == meid)
            {
                dt = Convert.ToDateTime(LoveTemp[10]);
                pt = 10;
                mySex = UsSex;
            }
            else
            {
                dt = Convert.ToDateTime(LoveTemp[11]);
                pt = 11;
                mySex = ReSex;
            }
            if (dt > DateTime.Now.AddHours(-1))
            {
                Utils.Error("一小时只可以浇心一次哦，请稍后再来吧", "");
            }
            int s = 0;
            int j = 5;
            if (mySex == 1)
            {
                s = 5;
                j = 10;
            }
            bool IsTrue = false;
            for (int i = s; i < j; i++)
            {
                if (LoveTemp[i] == "0")
                {
                    string vLove = "";
                    for (int k = 0; k < LoveTemp.Length; k++)
                    {
                        if (i == k)
                        {
                            vLove += "#1";
                        }
                        else
                        {
                            if (k == pt)
                                vLove += "#" + DateTime.Now;
                            else
                                vLove += "#" + LoveTemp[k];
                        }
                    }
                    //写入更新
                    vLove = Utils.Mid(vLove, 1, vLove.Length);
                    new BCW.BLL.Marry().UpdateLoveStat(ID, vLove);
                    IsTrue = true;
                    break;
                }
            }
            if (IsTrue == true)
            {
                Utils.Success("浇心", "操作成功!", Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                Utils.Success("浇心", "您的浇心结束啦", Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + ""), "2");
            }
        }
        else if (p == 2)//爱的召唤
        {
            int getUsID = 0;
            string getUsName = "";
            if (UsID == meid)
            {
                getUsID = ReID;
                getUsName = ReName;
            }
            else
            {
                getUsID = UsID;
                getUsName = UsName;
            }
            new BCW.BLL.Guest().Add(getUsID, getUsName, "亲爱的，是时候去看看我们的恋爱乐园了![url=/bbs/marry.aspx?act=loveinfo&amp;ptype=" + ptype + "]这就去[/url]");
            Utils.Success("爱的召唤", "操作成功!", Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + ""), "2");
        }
        else if (p == 3)//分手
        {
            int getUsID = 0;
            string getUsName = "";
            if (UsID == meid)
            {
                getUsID = ReID;
                getUsName = ReName;
            }
            else
            {
                getUsID = UsID;
                getUsName = UsName;
            }

            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "分手理由限1-50字");

                new BCW.BLL.Marry().Delete("ID=" + ID + "");
                //内线通知
                string mename = new BCW.BLL.User().GetUsName(meid);
                new BCW.BLL.Guest().Add(getUsID, getUsName, "很遗憾，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]已决定与你分手，请不要再伤害TA了![br]分手理由：" + Content + "");

                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向[url=/bbs/uinfo.aspx?uid=" + getUsID + "]" + getUsName + "[/url]提出[url=/bbs/marry.aspx]分手[/url]，两人不再是恋人";
                new BCW.BLL.Action().Add(15, 0, 0, "", wText);

                Utils.Success("我要分手", "分手成功，你与" + getUsName + "不再是恋人关系", Utils.getUrl("marry.aspx"), "2");
            }
            else
            {
                Master.Title = "我要分手";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("我向<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + getUsID + "&amp;backurl==" + Utils.PostPage(1) + "") + "\">" + getUsName + "(" + getUsID + ")</a>立即分手");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "分手理由(50字内):/,,,,";
                string strName = "Content,ptype,p,act,info";
                string strType = "textarea,hidden,hidden,hidden,hidden";
                string strValu = "'" + ptype + "'" + p + "'loveinfo'ok";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "我要分手|reset,marry.aspx,post,0,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
        }
        else if (p == 4)//求婚
        {
            if (!LoveStat.Contains("1#1#1#1#1#1#1#1#1#1"))
            {
                Utils.Error("浇心还没有完成，不能求婚..", "");
            }
            int getUsID = 0;
            string getUsName = "";
            if (UsID == meid)
            {
                getUsID = ReID;
                getUsName = ReName;
            }
            else
            {
                getUsID = UsID;
                getUsName = UsName;
            }

            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {

                if (!new BCW.BLL.User().Exists(getUsID))
                {
                    Utils.Error("不存在的会员ID", "");
                }
                if (!new BCW.BLL.Marry().Exists2(meid, getUsID))
                {
                    Utils.Error("请先恋爱再进行求婚..", "");
                }

                int IsVerify = new BCW.BLL.User().GetIsVerify(meid);
                if (IsVerify == 0)
                {
                    Utils.Error("您属于手工注册会员，还未通过短信验证<br /><a href=\"/reg.aspx\">免费验证会员</a>", "");
                }
                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "爱情誓言限1-50字");
                if (new BCW.BLL.Marry().ExistsMarry(meid))
                {
                    Utils.Error("很抱歉，请遵守国策，禁止重婚", "");
                }
                if (new BCW.BLL.Marry().ExistsMarry(getUsID))
                {
                    Utils.Error("很遗憾，对方已名花有主咯", "");
                }
                //送礼数达一定数量才可以求婚
                //int GiftNum = Utils.ParseInt(ub.GetSub("MarryGiftNum", xmlPath));
                //if (GiftNum > 0)
                //{
                //    int getGiftNum = new BCW.BLL.Shopsend().GetCount(meid, ReID);
                //    if (getGiftNum < GiftNum)
                //    {
                //        Utils.Success("我要求婚", "送礼不足" + GiftNum + "次，不能求婚！<a href=\"" + Utils.getUrl("bbsshop.aspx?backurl=" + Server.UrlEncode("/bbs/marry.aspx") + "") + "\">马上到商城选礼物&gt;&gt;</a>", Utils.getUrl("bbsshop.aspx?backurl=" + Server.UrlEncode("/bbs/marry.aspx") + ""), "2");
                //    }
                //}
                new BCW.BLL.Marry().UpdateMarry(meid, getUsID, Content);
                //内线通知
                string mename = new BCW.BLL.User().GetUsName(meid);
                new BCW.BLL.Guest().Add(getUsID, getUsName, "恭喜，[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向你求婚啦![br]爱情誓言：" + Content + "[br]如果您接受，你们将立即成为夫妻哦![url=/bbs/marry.aspx?act=yes2&amp;ReID=" + meid + "]接受请求[/url]|[url=/bbs/marry.aspx?act=no2&amp;ReID=" + meid + "]拒绝[/url]");

                //动态
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]向[url=/bbs/uinfo.aspx?uid=" + getUsID + "]" + getUsName + "[/url]提出[url=/bbs/marry.aspx]求婚啦[/url]";
                new BCW.BLL.Action().Add(15, 0, 0, "", wText);

                Utils.Success("我要求婚", "求婚请求成功，请等待对方回应..", Utils.getUrl("marry.aspx"), "2");
            }
            else
            {
                Master.Title = "我要求婚";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("我向<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + getUsID + "&amp;backurl==" + Utils.PostPage(1) + "") + "\">" + getUsName + "(" + getUsID + ")</a>求婚");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "爱情誓言(将永久保留，50字内):/,,,,";
                string strName = "Content,ptype,p,act,info";
                string strType = "textarea,hidden,hidden,hidden,hidden";
                string strValu = "'" + ptype + "'" + p + "'loveinfo'ok";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "我要求婚|reset,marry.aspx,post,0,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("温馨提示:<br />遵守国策，一夫一妻制，禁止重婚!<br />求婚成功需收取您的" + ub.GetSub("MarryPrice", xmlPath) + "" + ub.Get("SiteBz") + "作为登记费");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            Master.Title = "恋爱乐园";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("恋爱乐园");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            int cNum = Utils.GetStringNum("#" + LoveStat + "#", "#1");
            if (ptype == 1)
            {
                builder.Append("“沉鱼”是我国古代四大美人之西施的故事。只要金鱼宝宝长大，就能得到美女西施的保佑哦~~<br />");
                if (cNum > 0)
                {
                    builder.Append("<img src=\"/Files/sys/flows/A" + cNum + ".gif\" alt=\"load\"/><br />");
                    string AText = OutAText(cNum);
                    if (AText != "")
                    {
                        builder.Append(AText + "<br />");
                    }
                }
                else
                {
                    builder.Append("<img src=\"/Files/sys/flows/A.jpg\" alt=\"load\"/><br />");
                }
            }
            else if (ptype == 2)
            {
                builder.Append("“落雁”是我国古代四大美人之王昭君的故事。只要美丽的鸟儿长大，就能得到明妃的恩赐哦~~<br />");
                if (cNum > 0)
                {
                    builder.Append("<img src=\"/Files/sys/flows/B" + cNum + ".gif\" alt=\"load\"/><br />");
                    string BText = OutBText(cNum);
                    if (BText != "")
                    {
                        builder.Append(BText + "<br />");
                    }
                }
                else
                {
                    builder.Append("<img src=\"/Files/sys/flows/B.jpg\" alt=\"load\"/><br />");
                }
            }
            else if (ptype == 3)
            {
                builder.Append("“闭月”是我国古代四大美人之貂婵的故事。只要皓月被爱心围绕，就能得到貂婵的祝福哦~~<br />");
                if (cNum > 0)
                {
                    builder.Append("<img src=\"/Files/sys/flows/C" + cNum + ".gif\" alt=\"load\"/><br />");
                    string CText = OutCText(cNum);
                    if (CText != "")
                    {
                        builder.Append(CText + "<br />");
                    }
                }
                else
                {
                    builder.Append("<img src=\"/Files/sys/flows/C.jpg\" alt=\"load\"/><br />");
                }
            }
            else if (ptype == 4)
            {
                builder.Append("“羞花”是我国古代四大美人之杨贵妃的故事。只要爱情花盛开。就能得到贵妃娘娘的祝福哦~~<br />");
                if (cNum > 0)
                {
                    builder.Append("<img src=\"/Files/sys/flows/D" + cNum + ".gif\" alt=\"load\"/><br />");
                    string DText = OutDText(cNum);
                    if (DText != "")
                    {
                        builder.Append(DText + "<br />");
                    }
                }
                else
                {
                    builder.Append("<img src=\"/Files/sys/flows/D.jpg\" alt=\"load\"/><br />");
                }
            }

            //男
            if (UsSex == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ReName + "</a>");
            }
            else //女
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ReName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
            }
            builder.Append("<br />恋爱时间:" + DT.FormatDate(DateTime.Parse(ds.Tables[0].Rows[0]["AcTime"].ToString()), 11) + "");
            if (cNum < 4)
            {
                builder.Append("<br />恋爱状态:青涩初恋<br />");
            }
            else if (cNum > 4 && cNum < 10)
            {
                builder.Append("<br />恋爱状态:浓情热恋"+cNum+"<br />");
            }
            else
            {
                builder.Append("<br />恋爱状态:谈婚论嫁<br />");
            }

            for (int i = 0; i < 5; i++)
            {
                if (LoveTemp[i] == "0")
                    builder.Append("<img src=\"/Files/sys/flows/marry_s.gif\" alt=\"load\"/>");
                else
                    builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
            }
            for (int i = 9; i >= 5; i--)
            {
                if (LoveTemp[i] == "0")
                    builder.Append("<img src=\"/Files/sys/flows/marry_s.gif\" alt=\"load\"/>");
                else
                    builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (LoveStat.Contains("1#1#1#1#1#1#1#1#1#1"))
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + "&amp;p=4&amp;backurl=" + Utils.PostPage(1) + "") + "\">马上求婚</a><br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + "&amp;p=1") + "\">开始游戏</a><br />");

            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + "&amp;p=2") + "\">爱的召唤</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=loveinfo&amp;ptype=" + ptype + "&amp;p=3&amp;backurl=" + Utils.PostPage(1) + "") + "\">我要分手</a>");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("点击开始游戏就能浇心<br />一颗心是有两个人共同灌溉的，左半边的心由男方灌溉，右半边的心为女方灌溉。<br />每一小时灌溉一次，等心全部浇完时，就会出现求婚。");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void HomePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        DataSet ds = null;
        int uid = meid;
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-9]$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        int hid = int.Parse(Utils.GetRequest("hid", "all", 1, @"^[1-9]\d*$", "0"));

        if (id > 0)
        {
            ds = new BCW.BLL.Marry().GetList("ID,UsID,UsName,UsSex,ReID,ReName,ReSex,AcTime,HomeName,FlowNum,HomeClick,FlowStat", "ID=" + id + " and Types=1");
        }
        else
        {
            if (hid > 0)
            {
                uid = hid;
            }
            ds = new BCW.BLL.Marry().GetList("ID,UsID,UsName,UsSex,ReID,ReName,ReSex,AcTime,HomeName,FlowNum,HomeClick,FlowStat", "(UsID=" + uid + " OR ReID=" + uid + ") and Types=1");
        }
        //取我的恋爱记录
        if (ds == null || ds.Tables[0].Rows.Count == 0)
        {
            Utils.Error("没有相关记录", "");
        }
        int ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
        int UsID = int.Parse(ds.Tables[0].Rows[0]["UsID"].ToString());
        int ReID = int.Parse(ds.Tables[0].Rows[0]["ReID"].ToString());
        string UsName = ds.Tables[0].Rows[0]["UsName"].ToString();
        string ReName = ds.Tables[0].Rows[0]["ReName"].ToString();
        int UsSex = int.Parse(ds.Tables[0].Rows[0]["UsSex"].ToString());
        int ReSex = int.Parse(ds.Tables[0].Rows[0]["ReSex"].ToString());
        DateTime AcTime = DateTime.Parse(ds.Tables[0].Rows[0]["AcTime"].ToString());
        string HomeName = ds.Tables[0].Rows[0]["HomeName"].ToString();
        int FlowNum = int.Parse(ds.Tables[0].Rows[0]["FlowNum"].ToString());
        int HomeClick = int.Parse(ds.Tables[0].Rows[0]["HomeClick"].ToString());
        string FlowStat = ds.Tables[0].Rows[0]["FlowStat"].ToString();
        if (p == 1)//修改花园名称
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,10}$", "花园名称限1-10字");

                new BCW.BLL.Marry().UpdateHomeName(ID, Content);

                Utils.Success("花园名称", "修改花园名称成功，正在返回...", Utils.getUrl("marry.aspx?act=home"), "2");
            }
            else
            {
                Master.Title = "修改花园名称";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("修改花园名称");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "花园名称(10字内):/,,,,";
                string strName = "Content,p,act,info";
                string strType = "text,hidden,hidden,hidden";
                string strValu = "" + HomeName + "'" + p + "'home'ok";
                string strEmpt = "true,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,marry.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            Master.Title = HomeName;
            builder.Append(Out.Tab("<div class=\"title\">" + HomeName + "</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=花园主人=<br />");
            //男
            if (UsSex == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ReName + "</a>");
            }
            else //女
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + ReName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "</a>");
            }
            //builder.Append("<br />→<a href=\"" + Utils.getUrl("marry.aspx?act=marrypk&amp;id=" + ID + "") + "\">结婚证书</a>←");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=爱的见证=");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));

            string flows = FlowStat;
            if (string.IsNullOrEmpty(flows))
            {
                flows = "0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0";
            }
            string flowtitle = "百合#杜鹃花#富贵竹#海棠#荷花#菊花#康乃馨#满天星#玫瑰#梅花#茉莉花#牡丹#牵牛花#水仙#桃花#仙人掌#郁金香#紫罗兰";
            string flowpy = "baihe#dujuanhua#fuguizhu#haitang#hehua#juhua#kangnaixing#mantianxing#meigui#meihua#molihua#mudan#qianniuhua#shuixian#taohua#xianrenjiang#yujinxiang#ziluolan";

            string[] flowTemp = flows.Split("#".ToCharArray());
            string[] flowTemp2 = flowtitle.Split("#".ToCharArray());
            string[] flowTemp3 = flowpy.Split("#".ToCharArray());
            int k = 0;
            for (int i = 0; i < flowTemp.Length; i++)
            {
                if (Convert.ToInt32(flowTemp[i]) > 0)
                {
                    builder.Append("<img src=\"/Files/sys/Flows/" + flowTemp3[i] + ".jpg\" alt=\"" + flowTemp2[1] + "\"/>x" + flowTemp[i] + "");

                    k++;
                    if (k > 4)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + ID + "&amp;p=2") + "\">..</a>");
                        break;
                    }
                }
            }
            if (k > 0)
                builder.Append("<br />");

            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + ID + "") + "\">浪漫花园[" + FlowNum + "朵]</a><br />");
            builder.Append("来花园种花,经营永恒的爱情");
            builder.Append(Out.Tab("</div>", "<br />"));


            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=甜蜜瞬间=");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            IList<BCW.Model.MarryPhoto> listMarryPhoto = new BCW.BLL.MarryPhoto().GetMarryPhotos(2, "MarryId=" + ID + "");
            if (listMarryPhoto.Count > 0)
            {
                foreach (BCW.Model.MarryPhoto n in listMarryPhoto)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("" + n.ActFile + "") + "\"><img src=\"" + n.PrevFile + "\" alt=\"load\"/></a>");
                }
                builder.Append("<br />");
            }
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + ID + "") + "\">幸福相册[" + new BCW.BLL.MarryPhoto().GetCount(ID) + "]</a><br />");
            builder.Append("记录爱情中每一个甜蜜的瞬间");
            builder.Append(Out.Tab("</div>", "<br />"));
            
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("=<a href=\"" + Utils.getUrl("marry.aspx?act=marryaction&amp;id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">生活点滴</a>=");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            IList<BCW.Model.MarryAction> listMarryAction = new BCW.BLL.MarryAction().GetMarryActions(5, "MarryId=" + ID + "");
            if (listMarryAction.Count > 0)
            {
                foreach (BCW.Model.MarryAction n in listMarryAction)
                {
                    builder.Append("" + DT.DateDiff2(DateTime.Now, n.AddTime) + "前" + Out.SysUBB(n.Content) + "<br />");
                }
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("结婚时间:" + DT.FormatDate(AcTime, 11) + "<br />");
            builder.Append("访问次数:" + HomeClick + "次");
            builder.Append("<br /><a href=\"" + Utils.getUrl("marry.aspx?act=marrybook&amp;id=" + ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">留言板(" + new BCW.BLL.MarryBook().GetCount(id) + ")</a>");

            if (meid == uid)
            {
                builder.Append("<br /><a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;p=1") + "\">修改花园名称</a>");
            }
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
            builder.Append(Out.Tab("</div>", ""));
            //访问增加
            string appName = "LIGHT_MARRYCLICK_" + ID + "";
            int Expir = 300;
            string CacheKey = appName + "_" + Utils.Mid(Utils.getstrU(), 0, Utils.getstrU().Length - 4);
            object getObjCacheTime = DataCache.GetCache(CacheKey);
            if (getObjCacheTime == null)
            {
                new BCW.BLL.Marry().UpdateHomeClick(ID, 1);
            }
            object ObjCacheTime = DateTime.Now;
            DataCache.SetCache(CacheKey, ObjCacheTime, DateTime.Now.AddSeconds(Expir), TimeSpan.Zero);
        }
    }

    private void MarryBookPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,200}$", "请输入不超200字的内容");
            int Face = int.Parse(Utils.GetRequest("Face", "post", 1, @"^[0-9]\d*$", "0"));

            if (Face > 0 && Face < 25)
                Content = "[F]" + Face + "[/F]" + Content;

            //是否刷屏
            string appName = "LIGHT_MARRYBOOK";
            int Expir = 30;// Convert.ToInt32(ub.GetSub("Expir", xmlPath));
            BCW.User.Users.IsFresh(appName, Expir);

            string mename = new BCW.BLL.User().GetUsName(meid);
            BCW.Model.MarryBook m = new BCW.Model.MarryBook();
            m.MarryId = id;
            m.ReID = meid;
            m.ReName = mename;
            m.Content = Content;
            m.AddTime = DateTime.Now;
            new BCW.BLL.MarryBook().Add(m);

            //内线通知
            new BCW.BLL.Guest().Add(model.UsID, model.UsName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在你的[url=/bbs/marry.aspx?act=home]"+model.HomeName+"[/url]留言啦~~");
            new BCW.BLL.Guest().Add(model.ReID, model.ReName, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在你的[url=/bbs/marry.aspx?act=home]" + model.HomeName + "[/url]留言啦~~");

            Utils.Success("发表留言", "发表留言成功，正在返回...", Utils.getUrl("marry.aspx?act=marrybook&amp;id="+id+"&amp;backurl=" + Utils.getPage(0) + ""), "2");

        }

        Master.Title = model.HomeName + "-留言板";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(model.HomeName + "-留言板");
        builder.Append(Out.Tab("</div>", "<br />"));
        string strText = ",,,,,,";
        string strName = "Face,Content,id,info,act,backurl";
        string strType = "hidden,text,hidden,hidden,hidden,hidden";
        string strValu = "0''" + id + "'ok'marrybook'" + Utils.getPage(0) + "";
        string strEmpt = "0|表情|1|微笑|2|哭泣|3|调皮|4|脸红|5|害羞|6|生气|7|偷笑|8|流汗|9|我倒|10|得意|11|眦牙|12|疑问|13|睡觉|14|好色|15|口水|16|敲头|17|可爱|18|猪头|19|胜利|20|玫瑰|21|吃药|22|白酒|23|可乐|24|囧囧,true,false,false,false,false";
        string strIdea = "";
        string strOthe = "发表,marry.aspx,post,3,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "MarryId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.MarryBook> listMarryBook = new BCW.BLL.MarryBook().GetMarryBooks(pageIndex, pageSize, strWhere, out recordCount);
        if (listMarryBook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.MarryBook n in listMarryBook)
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

                builder.Append(((pageIndex - 1) * pageSize + k) + ".");

                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.getPage(1) + "") + "\">" + n.ReName + "</a>说:" + n.Content + "(" + DT.FormatDate(n.AddTime, 11) + ")");

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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + id + "") + "\">&lt;&lt;" + model.HomeName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("marry.aspx?act=home&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void MarryActionPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
        BCW.Model.Marry model = null;
        if (id > 0)
        {
            model = new BCW.BLL.Marry().GetMarry(id);
            if (model == null)
            {
                Utils.Error("不存在的记录", "");
            }
            Master.Title = model.HomeName;
        }
        else
        {
            Master.Title = "婚恋动态";
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("爱情的记忆永远都是那么美好!");
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
        if (id > 0)
            strWhere = "MarryId=" + id + "";

        // 开始读取列表
        IList<BCW.Model.MarryAction> listMarryAction = new BCW.BLL.MarryAction().GetMarryActions(pageIndex, pageSize, strWhere, out recordCount);
        if (listMarryAction.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.MarryAction n in listMarryAction)
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

                builder.Append(((pageIndex - 1) * pageSize + k) + ".");

                builder.Append("" + Out.SysUBB(n.Content) + "(" + DT.FormatDate(n.AddTime, 11) + ")");

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
        if (id > 0)
        {
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + id + "") + "\">&lt;&lt;" + model.HomeName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    private void MarryPhotoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-9]\d*$", "0"));

        if (p == 1)
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                BCW.User.Users.ShowVerifyRole("f", meid);//非验证会员提示
                new BCW.User.Limits().CheckUserLimit(BCW.User.Limits.enumRole.Role_Upfile, meid);//会员上传权限


                //是否刷屏
                string appName = "LIGHT_MARRYPHOTO";
                int Expir = 30;
                BCW.User.Users.IsFresh(appName, Expir);

                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,500}$", "相片描述限1-500字");
                string GetFiles = string.Empty;
                //遍历File表单元素
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                try
                {
                    for (int iFile = 0; iFile < files.Count; iFile++)
                    {
                        //检查文件扩展名字
                        System.Web.HttpPostedFile postedFile = files[iFile];
                        string fileName, fileExtension;
                        fileName = System.IO.Path.GetFileName(postedFile.FileName);
                        string UpExt = ".gif,.jpg,.jpeg,.png"; ;
                        int UpLength = 300;
                        if (fileName != "")
                        {
                            fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                            //检查是否允许上传格式
                            if (UpExt.IndexOf(fileExtension) == -1)
                            {
                                continue;
                            }
                            if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))
                            {
                                continue;
                            }
                            string DirPath = string.Empty;
                            string prevDirPath = string.Empty;
                            string Path = "/Files/marry/act/";
                            string prevPath = "/Files/marry/prev/";
                            if (FileTool.CreateDirectory(Path, out DirPath))
                            {
                                //生成随机文件名
                                fileName = DT.getDateTimeNum() + iFile + fileExtension;
                                string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                                postedFile.SaveAs(SavePath);

                                //=============================图片木马检测,包括TXT===========================
                                string vSavePath = SavePath;

                                bool IsPass = true;
                                System.IO.StreamReader sr = new System.IO.StreamReader(vSavePath, System.Text.Encoding.Default);
                                string strContent = sr.ReadToEnd().ToLower();
                                sr.Close();
                                string str = "system.|request|javascript|script |script>|.getfolder|.createfolder|.deletefolder|.createdirectory|.deletedirectory|.saveas|wscript.shell|script.encode|server.|.createobject|execute|activexobject|language=";
                                foreach (string s in str.Split('|'))
                                {
                                    if (strContent.IndexOf(s) != -1)
                                    {
                                        System.IO.File.Delete(vSavePath);
                                        IsPass = false;
                                        break;
                                    }
                                }
                                if (IsPass == false)
                                    Utils.Error("非法图片..", "");

                                //=============================图片木马检测,包括TXT===========================

                                int width = 70;
                                int height = 70;

                                bool pbool = false;
                                pbool = true;//保持比例
                                if (FileTool.CreateDirectory(prevPath, out prevDirPath))
                                {
                                    string prevSavePath = System.Web.HttpContext.Current.Request.MapPath(prevDirPath) + fileName;
                                    GetFiles += "#" + prevDirPath + fileName;
                                    if (fileExtension == ".gif")
                                    {
                                        new BCW.Graph.GifHelper().GetThumbnail(SavePath, prevSavePath, width, height, pbool);

                                    }
                                    else
                                    {
                                        new BCW.Graph.ImageHelper().ResizeImage(SavePath, prevSavePath, width, height, pbool);

                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                    Utils.Error("上传的相片无法生成缩略图", "");
                }


                BCW.Model.MarryPhoto m = new BCW.Model.MarryPhoto();
                m.MarryId = id;
                m.UsID = meid;
                m.UsName = new BCW.BLL.User().GetUsName(meid);
                m.PrevFile = Utils.Mid(GetFiles, 1, GetFiles.Length);
                m.ActFile = Utils.Mid(GetFiles, 1, GetFiles.Length).Replace("/prev/", "/act/");
                m.Notes = Content;
                m.AddTime = DateTime.Now;
                new BCW.BLL.MarryPhoto().Add(m);
                Utils.Success("上传相片", "上传相片成功，正在返回...", Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + ""), "2");

            }
            else
            {
                Master.Title = model.HomeName + "-幸福相册";
                if (!Utils.Isie())
                {
                    string VE = ConfigHelper.GetConfigString("VE");
                    string SID = ConfigHelper.GetConfigString("SID");
                    builder.Append("<a href=\"marry.aspx?act=marryphoto&amp;id=" + id + "&amp;p=1&amp;" + VE + "=2a&amp;" + SID + "=" + Utils.getstrU() + "\">[切换2.0上传]</a>");
                }
                else
                {
                    builder.Append(Out.Tab("<div class=\"title\">上传相片</div>", ""));

                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("上传允许格式:..gif,.jpg,.png,.jpeg<br />");
                    builder.Append("每张图片限300k内");
                    builder.Append(Out.Tab("</div>", "<br />"));

                    string strText = string.Empty;
                    string strName = string.Empty;
                    string strType = string.Empty;
                    string strValu = string.Empty;
                    string strEmpt = string.Empty;
                    string strIdea = string.Empty;
                    string strOthe = string.Empty;

                    string sText = string.Empty;
                    string sName = string.Empty;
                    string sType = string.Empty;
                    string sValu = string.Empty;
                    string sEmpt = string.Empty;

                    sText = "相片描述(50字内):/,";
                    sName = "Content,";
                    sType = "textarea,";
                    sValu = "'";
                    sEmpt = "false,";
                    int num = 1;
                    for (int i = 0; i < num; i++)
                    {
                        string y = ",";
                        if (num == 1)
                        {
                            strText = strText + y + "选择图片:/";
                        }
                        else
                        {
                            strText = strText + y + "第" + (i + 1) + "个图片:/";
                        }
                        strName = strName + y + "file" + (i + 1);
                        strType = strType + y + "file";
                        strValu = strValu + "'";
                        strEmpt = strEmpt + y;
                    }

                    strText = sText + Utils.Mid(strText, 1, strText.Length) + ",,,,,";
                    strName = sName + Utils.Mid(strName, 1, strName.Length) + ",id,p,backurl,act,info";
                    strType = sType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden,hidden";
                    strValu = sValu + Utils.Mid(strValu, 1, strValu.Length) + "'" + id + "'" + p + "'" + Utils.getPage(0) + "'marryphoto'ok";
                    strEmpt = sEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,,,"; ;
                    strIdea = "/";
                    strOthe = "我要上传|reset,marry.aspx,post,2,red|blue";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    builder.Append(Out.Tab("<div>", " "));
                    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">取消</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }
        else
        {
            int showtype = int.Parse(Utils.GetRequest("showtype", "all", 1, @"^[0-1]$", "0"));

            Master.Title = model.HomeName + "-幸福相册";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("每张照片,都是幸福爱情的瞬间!");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("查看:");
            if (showtype == 0)
                builder.Append("小图|");
            else
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + id + "&amp;showtype=0") + "\">小图</a>|");

            if (showtype == 1)
                builder.Append("原图");
            else
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + id + "&amp;showtype=1") + "\">原图</a>");

            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "act", "id", "showtype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            //查询条件
            strWhere = "MarryId=" + id + "";

            // 开始读取列表
            IList<BCW.Model.MarryPhoto> listMarryPhoto = new BCW.BLL.MarryPhoto().GetMarryPhotos(pageIndex, pageSize, strWhere, out recordCount);
            if (listMarryPhoto.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.MarryPhoto n in listMarryPhoto)
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

                    //builder.Append(((pageIndex - 1) * pageSize + k) + ".");
                    if (showtype == 0)
                        builder.Append("<a href=\"" + Utils.getUrl("" + n.ActFile + "") + "\"><img src=\"" + n.PrevFile + "\" alt=\"load\"/></a>");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("" + n.ActFile + "") + "\"><img src=\"" + n.ActFile + "\" alt=\"load\"/></a>");

                    builder.Append("<br />上传:<a href=\"" + Utils.getUrl("uinfo.aspx?uid="+n.UsID+"") + "\">"+n.UsName+"</a>");
                    builder.Append("<br />描述:" + n.Notes + "(" + DT.FormatDate(n.AddTime, 11) + ")");

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
            if (model.UsID == meid || model.ReID == meid)
            {
                builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryphoto&amp;id=" + id + "&amp;p=1") + "\">我要上传相片</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + id + "") + "\">&lt;&lt;" + model.HomeName + "</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("marry.aspx?act=home&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MarryFlowPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }

        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-5]$", "0"));
        if (p == 1)
        {
            if (model.UsID != meid && model.ReID != meid)
            {
                Utils.Error("你不是花园主人", "");
            }
            DateTime dt = DateTime.Now;
            string FlowTimes = model.FlowTimes;
            string flows = model.FlowStat;
            if (string.IsNullOrEmpty(FlowTimes))
            {
                FlowTimes = "1990-1-1#1990-1-1";
            }
            if (string.IsNullOrEmpty(flows))
            {
                flows = "0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0";
            }

            bool IsBoy = true;
            string[] dTemp = FlowTimes.Split("#".ToCharArray());
            if ((model.UsSex == 2 && model.UsID == meid))
            {
                dt = Convert.ToDateTime(dTemp[0]);
            }
            else if ((model.ReSex == 2 && model.ReID == meid))
            {
                dt = Convert.ToDateTime(dTemp[0]);
            }
            else
            {
                dt = Convert.ToDateTime(dTemp[1]);
                IsBoy = false;
            }
            //6小时可以浇一次花
            if (dt > DateTime.Now.AddHours(-6))
            {
                Utils.Error("6小时只可以浇花一次哦，请稍后再来吧", "");
            }

            string flowtitle = "百合#杜鹃花#富贵竹#海棠#荷花#菊花#康乃馨#满天星#玫瑰#梅花#茉莉花#牡丹#牵牛花#水仙#桃花#仙人掌#郁金香#紫罗兰";
            string flowpy = "baihe#dujuanhua#fuguizhu#haitang#hehua#juhua#kangnaixing#mantianxing#meigui#meihua#molihua#mudan#qianniuhua#shuixian#taohua#xianrenjiang#yujinxiang#ziluolan";
            string[] flowTemp = flows.Split("#".ToCharArray());
            string[] flowTemp2 = flowtitle.Split("#".ToCharArray());
            string[] flowTemp3 = flowpy.Split("#".ToCharArray());
            int rd = new Random().Next(0, 18);
            string Bz = ub.Get("SiteBz");
            int cent = 10;
            if (model.FlowNum > 0)
            {
                cent = cent + (model.FlowNum * 5);
            }
            if (cent > 520)
                cent = 520;

            long gold = new BCW.BLL.User().GetGold(meid);
            if (gold < cent)
            {
                Utils.Error("你自带不足" + cent + "" + ub.Get("SiteBz") + "，不能进行浇花", "");
            }
            string Result = "";
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int rd1 = ra.Next(1, 100);
            if (rd1 > 70)
            {
                Result += "走运啦，遇到酷爆老板，" + Bz + "-" + cent + "，+5朵" + flowTemp2[rd] + "@5@" + cent + "#";
                Result += "遇到花仙子了，事半功倍，" + Bz + "-" + cent + "，+2朵" + flowTemp2[rd] + "@2@" + cent + "#";
                Result += "555，被花痴骚扰了。" + Bz + "-" + cent + "，+0朵" + flowTemp2[rd] + "@0@" + cent + "#";
                Result += "嘿嘿，遇到财神爷了，" + Bz + "-0，+1朵" + flowTemp2[rd] + "@1@0";
                if (model.FlowNum > 0)
                {
                    Result += "#倒霉催的，遇到强盗了，" + Bz + "-" + cent + "，-1朵" + flowTemp2[rd] + "@-1@" + cent + "";
                }
                string[] sNum = Result.Split("#".ToCharArray());
                Random rd2 = new Random();
                Result = sNum[rd2.Next(sNum.Length)];
            }
            else
            {
                Result = "浇了一朵花，" + Bz + "-" + cent + "，+1朵" + flowTemp2[rd] + "@1@" + cent + "";
            }
            string[] rTemp = Result.Split("@".ToCharArray());
            string reText = rTemp[0];
            int iflow = Convert.ToInt32(rTemp[1]);//增加的鲜花数量
            long icent = Convert.ToInt64(rTemp[2]);//需要扣的币


            string vStat = "";
            for (int i = 0; i < flowTemp.Length; i++)
            {
                if (i == rd)
                {
                    int getiflow = Convert.ToInt32(flowTemp[i]) + iflow;
                    if (getiflow < 0)
                        getiflow = 0;

                    vStat += "#" + getiflow;
                }
                else
                {
                    vStat += "#" + flowTemp[i];
                }
            }
            if (vStat != "")
            {
                vStat = Utils.Mid(vStat, 1, vStat.Length);

            }
            string vTimes = "";
            if (IsBoy)
                vTimes = DateTime.Now + "#" + dTemp[1];
            else
                vTimes = dTemp[0] + "#" + DateTime.Now;

            new BCW.BLL.Marry().UpdateFlowStat(id, vStat, vTimes, iflow);

            string mename = new BCW.BLL.User().GetUsName(meid);
            if (icent > 0)
            {
                new BCW.BLL.User().UpdateiGold(meid, mename, -icent, "花园浇花扣币");
            }

            //动态
            if (iflow > 0)
            {
                string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]种下了[url=/bbs/marry.aspx?act=home&amp;id=" + id + "]" + iflow + "朵[/url]" + flowTemp2[rd] + "!";
                BCW.Model.MarryAction A = new BCW.Model.MarryAction();
                A.MarryId = id;
                A.Content = wText;
                A.AddTime = DateTime.Now;
                new BCW.BLL.MarryAction().Add(A);
            }

            Utils.Success("浇花", "" + reText + "，正在返回...", Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + id + ""), "3");
        }
        else if (p == 2)
        {
            Master.Title = model.HomeName;
            builder.Append(Out.Tab("<div class=\"title\">" + model.HomeName + "</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=鲜花列表=");
            builder.Append(Out.Tab("</div>", "<br />"));

            string flows = model.FlowStat;
            if (string.IsNullOrEmpty(flows))
            {
                flows = "0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0";
            }
            string flowtitle = "百合#杜鹃花#富贵竹#海棠#荷花#菊花#康乃馨#满天星#玫瑰#梅花#茉莉花#牡丹#牵牛花#水仙#桃花#仙人掌#郁金香#紫罗兰";
            string flowpy = "baihe#dujuanhua#fuguizhu#haitang#hehua#juhua#kangnaixing#mantianxing#meigui#meihua#molihua#mudan#qianniuhua#shuixian#taohua#xianrenjiang#yujinxiang#ziluolan";

            string[] flowTemp = flows.Split("#".ToCharArray());
            string[] flowTemp2 = flowtitle.Split("#".ToCharArray());
            string[] flowTemp3 = flowpy.Split("#".ToCharArray());
            for (int i = 0; i < flowTemp.Length; i++)
            {
                if (Convert.ToInt32(flowTemp[i]) > 0)
                {
                    builder.Append("" + flowTemp2[i] + "<img src=\"/Files/sys/Flows/" + flowTemp3[i] + ".jpg\" alt=\"" + flowTemp2[i] + "\"/>x" + flowTemp[i] + "");
                    builder.Append("<br />");
                }
            }
            builder.Append(Out.Tab("<div class=\"title2\">", Out.RHr()));
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + id + "") + "\">&lt;&lt;返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));

        }
        else if (p == 3 || p == 4)
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,50}$", "誓言限1-50字");
                if (p == 3)
                    new BCW.BLL.Marry().UpdateOath(id, Content);
                else
                    new BCW.BLL.Marry().UpdateOath2(id, Content);

                Utils.Success("修改誓言", "修改誓言成功，正在返回...", Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + id + ""), "2");
            }
            else
            {
                Master.Title = "修改誓言";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("修改誓言");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "誓言内容(50字内):/,,,,,";
                string strName = "Content,id,p,act,info";
                string strType = "text,hidden,hidden,hidden,hidden";
                string strValu = "";
                if (p == 3)
                    strValu = "" + model.Oath + "'" + id + "'" + p + "'marryflow'ok";
                else
                    strValu = "" + model.Oath2 + "'" + id + "'" + p + "'marryflow'ok";

                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,marry.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", " "));
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + id + "") + "\">取消</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else if (p == 5)
        {
            Master.Title = model.HomeName;
            builder.Append(Out.Tab("<div class=\"title\">" + model.HomeName + "</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=花园帮助=");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.浪漫花园共18个种类，浇花成功将会随机得到其中一种鲜花。<br />");
            builder.Append("2.鲜花是每6小时浇一次，男女双方在6小时内都可以浇花一次。<br />");
            builder.Append("3.浇花需要花费" + ub.Get("SiteBz") + "，花费" + ub.Get("SiteBz") + "的数量与男女双方浇花的总次数成正比，但最高上限为520" + ub.Get("SiteBz") + "。<br />如第一次浇花花费10" + ub.Get("SiteBz") + "，然后第二次15" + ub.Get("SiteBz") + "，以此类推。<br />");
            builder.Append("4.在浇花过程中，还会有鲜花翻倍、免费浇花等一些意外惊喜哦。<br />");
            builder.Append("=鲜花种类=<br />百合、杜鹃花、富贵竹、海棠、荷花、菊花、康乃馨、满天星、玫瑰<br />梅花、茉莉花、牡丹、牵牛花、水仙、桃花、仙人掌、郁金香、紫罗兰");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;id=" + id + "") + "\">&lt;&lt;返回上级</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            Master.Title = model.HomeName;
            builder.Append(Out.Tab("<div class=\"title\">" + model.HomeName + "</div>", ""));
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("=花园主人=<br />");
            //男
            if (model.UsSex == 2)
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>");
            }
            else //女
            {
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.ReName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + model.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.UsName + "</a>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            if (model.UsID == meid || model.ReID == meid)
            {
                DateTime dt = DateTime.Now;
                string FlowTimes = model.FlowTimes;
                if (string.IsNullOrEmpty(FlowTimes))
                {
                    FlowTimes = "1990-1-1#1990-1-1";
                }
                string[] dTemp = FlowTimes.Split("#".ToCharArray());
                if ((model.UsSex == 2 && model.UsID == meid))
                {
                    dt = Convert.ToDateTime(dTemp[0]);
                }
                else if ((model.ReSex == 2 && model.ReID == meid))
                {
                    dt = Convert.ToDateTime(dTemp[0]);
                }
                else
                {
                    dt = Convert.ToDateTime(dTemp[1]);
                }
                //6小时可以浇一次花
                string dtText = "";
                if (dt > DateTime.Now.AddHours(-6))
                {
                    dtText = "还剩" + DT.DateDiff(dt.AddHours(6), DateTime.Now);
                }
                else
                {
                    dtText = "可以浇花";
                }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<img src=\"/Files/sys/flow.jpg\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;p=1&amp;id=" + id + "") + "\">浇花</a>(" + dtText + ")");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("+<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;p=2&amp;id=" + id + "") + "\">鲜花列表</a><br />");

            string flows = model.FlowStat;
            if (string.IsNullOrEmpty(flows))
            {
                flows = "0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0#0";
            }
            string flowtitle = "百合#杜鹃花#富贵竹#海棠#荷花#菊花#康乃馨#满天星#玫瑰#梅花#茉莉花#牡丹#牵牛花#水仙#桃花#仙人掌#郁金香#紫罗兰";
            string flowpy = "baihe#dujuanhua#fuguizhu#haitang#hehua#juhua#kangnaixing#mantianxing#meigui#meihua#molihua#mudan#qianniuhua#shuixian#taohua#xianrenjiang#yujinxiang#ziluolan";

            string[] flowTemp = flows.Split("#".ToCharArray());
            string[] flowTemp2 = flowtitle.Split("#".ToCharArray());
            string[] flowTemp3 = flowpy.Split("#".ToCharArray());
            int k = 0;
            for (int i = 0; i < flowTemp.Length; i++)
            {
                if (Convert.ToInt32(flowTemp[i]) > 0)
                {
                    builder.Append("<img src=\"/Files/sys/Flows/" + flowTemp3[i] + ".jpg\" alt=\"" + flowTemp2[1] + "\"/>x" + flowTemp[i] + "");

                    k++;
                    if (k % 6 == 0)
                        builder.Append("<br />");
                }
            }
            if (k % 6 != 0)
                builder.Append("<br />");

            builder.Append("鲜花数量:" + model.FlowNum + "<br />");
            builder.Append("花园排名:" + new BCW.BLL.Marry().GetFlowNumTop(model.ID) + "");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"title2\">", ""));
            builder.Append("=爱情誓言=");
            builder.Append("<br />男:" + model.Oath + "");
            if ((model.UsSex == 2 && model.UsID == meid) || (model.ReSex == 2 && model.ReID == meid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;p=3&amp;id=" + id + "") + "\">修改</a>");
            }
            builder.Append("<br />女:" + model.Oath2 + "");
            if ((model.UsSex == 1 && model.UsID == meid) || (model.ReSex == 1 && model.ReID == meid))
            {
                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;p=4&amp;id=" + id + "") + "\">修改</a>");
            }
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marryflow&amp;p=5&amp;id=" + id + "") + "\">花园帮助&gt;&gt;</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + id + "") + "\">&lt;&lt;" + model.HomeName + "</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("marry.aspx?act=home&amp;id=" + id + "") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    private void MarryTopPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        Master.Title = "花园列表";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("=花园列表=");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
            builder.Append("按:人气|");
        else
            builder.Append("按:<a href=\"" + Utils.getUrl("marry.aspx?act=marrytop&amp;ptype=1") + "\">人气</a>|");

        if (ptype == 2)
            builder.Append("鲜花数");
        else
            builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=marrytop&amp;ptype=2") + "\">鲜花数</a>");

        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //查询条件
        strWhere = "Types=1";
        if (ptype == 1)
            strOrder = "HomeClick desc";
        else
            strOrder = "FlowNum desc";

        // 开始读取列表
        IList<BCW.Model.Marry> listMarry = new BCW.BLL.Marry().GetMarrysTop(pageIndex, pageSize, strWhere,strOrder, out recordCount);
        if (listMarry.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Marry n in listMarry)
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

                builder.Append(((pageIndex - 1) * pageSize + k) + ".");

                builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id="+n.ID+"") + "\">"+n.HomeName+"</a>");
                if (ptype == 1)
                    builder.Append("人气" + n.HomeClick + "");
                else
                    builder.Append("鲜花" + n.FlowNum + "");

                builder.Append("<br /><a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>");
                builder.Append("<img src=\"/Files/sys/flows/marry_x.gif\" alt=\"load\"/>");
                builder.Append("<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.ReID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.ReName + "</a>");
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
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">我要结婚建花园&gt;&gt;</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
        builder.Append(Out.Tab("</div>", ""));
    }


    //private void MarryPkPage()
    //{
    //    int meid = new BCW.User.Users().GetUsId();
    //    if (meid == 0)
    //        Utils.Login();

    //    int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID错误"));
    //    BCW.Model.Marry model = new BCW.BLL.Marry().GetMarry(id);
    //    if (model == null)
    //    {
    //        Utils.Error("不存在的记录", "");
    //    }
    //    string pic = model.MarryPk;
    //    if (string.IsNullOrEmpty(pic) || Request["info"] == "re")
    //    {

    //        //生成结婚证
    //        string UsID = "";
    //        string UsName = "";
    //        string ReID = "";
    //        string ReName = "";
    //        string Date = DT.FormatDate(model.AcTime, 11);
    //        string ID = "1000000-" + id + "";
    //        string UsPhoto = "";
    //        string RePhoto = "";
    //        if (model.UsSex == 2)
    //        {
    //            BCW.Model.User m = new BCW.BLL.User().GetBasic(model.UsID);
    //            BCW.Model.User n = new BCW.BLL.User().GetBasic(model.ReID);
    //            if (m == null)
    //            {
    //                Utils.Error("某位会员已被删除", "");
    //            }
    //            if (n == null)
    //            {
    //                Utils.Error("某位会员已被删除", "");
    //            }
    //            UsID = model.UsID.ToString();
    //            UsName = m.UsName;
    //            ReID = model.ReID.ToString();
    //            ReName = n.UsName;
    //            UsPhoto = m.Photo;
    //            RePhoto = n.Photo;
    //        }
    //        else
    //        {
    //            BCW.Model.User n = new BCW.BLL.User().GetBasic(model.UsID);
    //            BCW.Model.User m = new BCW.BLL.User().GetBasic(model.ReID);
    //            if (m == null)
    //            {
    //                Utils.Error("某位会员已被删除", "");
    //            }
    //            if (n == null)
    //            {
    //                Utils.Error("某位会员已被删除", "");
    //            }
    //            UsID = model.ReID.ToString();
    //            UsName = m.UsName;
    //            ReID = model.UsID.ToString();
    //            ReName = n.UsName;
    //            UsPhoto = m.Photo;
    //            RePhoto = n.Photo;
    //        }

    //        pic = "/files/marry/pk/" + UsID + "_" + ReID + ".jpg";
    //        ASPJPEGLib.IASPJpeg Jpeg = new ASPJPEGLib.ASPJpeg();
    //        // 源图片路径
    //        string strPath = Server.MapPath("/Files/sys/flows/marrydiy.jpg");
    //        // 打开源图片
    //        Jpeg.Open(strPath);

    //        //男姓名 
    //        Jpeg.Canvas.Font.Color = 0xFF0000; //red 颜色 
    //        Jpeg.Canvas.Font.Family = "宋体";//字体 
    //        Jpeg.Canvas.Font.Size = 13;//字体 
    //        Jpeg.Canvas.Font.Bold = 1;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(155, 107, UsName, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 
    //        //女姓名 
    //        Jpeg.Canvas.Font.Color = 0xFF0000;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "宋体";//字体 
    //        Jpeg.Canvas.Font.Size = 13;//字体 
    //        Jpeg.Canvas.Font.Bold = 1;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(155, 153, ReName, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 

    //        //男ID号
    //        Jpeg.Canvas.Font.Color = 0xFF0000;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "宋体";//字体 
    //        Jpeg.Canvas.Font.Size = 13;//字体 
    //        Jpeg.Canvas.Font.Bold = 1;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(155, 131, UsID, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 
    //        //女ID号
    //        Jpeg.Canvas.Font.Color = 0xFF0000;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "宋体";//字体 
    //        Jpeg.Canvas.Font.Size = 13;//字体 
    //        Jpeg.Canvas.Font.Bold = 1;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(155, 175, ReID, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符

    //        //日期 
    //        Jpeg.Canvas.Font.Color = 000000;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "黑体";//字体 
    //        Jpeg.Canvas.Font.Size = 16;//字体 
    //        Jpeg.Canvas.Font.Bold = 0;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(321, 254, Date, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 
    //        //编号 
    //        Jpeg.Canvas.Font.Color = 000000;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "黑体";//字体 
    //        Jpeg.Canvas.Font.Size = 12;//字体 
    //        Jpeg.Canvas.Font.Bold = 0;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(354, 276, ID, Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 

    //        //发证单位 
    //        Jpeg.Canvas.Font.Color = 605666;//red 颜色 
    //        Jpeg.Canvas.Font.Family = "宋体";//字体 
    //        Jpeg.Canvas.Font.Size = 12;//字体 
    //        Jpeg.Canvas.Font.Bold = 0;//是否粗体，粗体用：True(1),False(0) 
    //        Jpeg.Canvas.Print(50, 265, "发证单位:168xin.com酷爆网婚部", Missing.Value);//打印坐标x 打印坐标y 需要打印的字符 

    //        Jpeg.Quality = 100;
    //        Jpeg.Save(Server.MapPath(pic)); //保存
    //        Jpeg.Close();


    //        ASPJPEGLib.IASPJpeg Logo = new ASPJPEGLib.ASPJpeg();
    //        string LogoPath = "";
    //        LogoPath = Server.MapPath(UsPhoto);
    //        Logo.Open(LogoPath);
    //        Logo.Width = 65;
    //        Logo.Height = 75;
    //        Logo.Save(Server.MapPath("/files/temp/temp1.jpg")); //保存

    //        LogoPath = Server.MapPath(RePhoto);
    //        Logo.Open(LogoPath);
    //        Logo.Width = 65;
    //        Logo.Height = 75;
    //        Logo.Save(Server.MapPath("/files/temp/temp2.jpg")); //保存
    //        Logo.Close();

    //        new BCW.Graph.ImageHelper().WaterMark2(Server.MapPath(pic), "", Server.MapPath("/files/temp/temp1.jpg"), 276, 107);
    //        new BCW.Graph.ImageHelper().WaterMark2(Server.MapPath(pic), "", Server.MapPath("/files/temp/temp2.jpg"), 344, 107);
    //        new BCW.BLL.Marry().UpdateMarryPk(id, pic);
    //        if (Request["info"] == "re")
    //        {
    //            Utils.Success("生成结婚证", "生成结婚证成功，正在返回...", Utils.getUrl("marry.aspx?act=marrypk&amp;id=" + id + ""), "1");
    //        }
    //    }
    //    Master.Title = "结婚证";
    //    builder.Append(Out.Tab("<div class=\"title\">", ""));
    //    builder.Append("=结婚证=");
    //    builder.Append(Out.Tab("</div>", "<br />"));
    //    builder.Append(Out.Tab("<div>", ""));
    //    builder.Append("<img src=\"" + pic + "\" alt=\"load\"/>");
    //    builder.Append(Out.Tab("</div>", ""));
    //    builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
    //    builder.Append("贴图ubb:[img]" + pic + "[/img]<br />");
    //    builder.Append("<a href=\"" + Utils.getUrl("" + pic + "") + "\">下载结婚证</a><br />");
    //    builder.Append("=昵称或头像更改了=<br />可以<a href=\"" + Utils.getUrl("marry.aspx?act=marrypk&amp;info=re&amp;id=" + id + "") + "\">重新生成</a>");
    //    builder.Append(Out.Tab("</div>", ""));

    //    builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
    //    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx?act=home&amp;id=" + id + "") + "\">&lt;&lt;" + model.HomeName + "</a>");
    //    builder.Append(Out.Tab("</div>", ""));
    //    builder.Append(Out.Tab("<div class=\"title2\">", Out.Hr()));
    //    builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
    //    builder.Append("<a href=\"" + Utils.getPage("uinfo.aspx") + "\">上级</a>-");
    //    builder.Append("<a href=\"" + Utils.getUrl("marry.aspx") + "\">婚姻</a>");
    //    builder.Append(Out.Tab("</div>", ""));
    //}


    private string OutAText(int cNum)
    {
        string OutText = "";
        if (cNum == 1)
            OutText = "爱对方多一点，就喂喂这可爱的鱼宝宝吧！";
        else if (cNum == 2)
            OutText = "爱情的甜蜜，让小鱼儿越长越大，越来越漂亮……";
        else if (cNum == 3)
            OutText = "恋爱是两个人的事，照顾金鱼也是两个人的事哦";
        else if (cNum == 4)
            OutText = "可爱的金鱼已经长大，色彩也变得五彩斑斓……";
        else if (cNum == 5)
            OutText = "加油哦，水里的鱼儿正看着你们笑呢，好甜蜜哦~~";
        else if (cNum == 6)
            OutText = "这美丽的金鱼不只是水中的精灵，更是你们甜美爱情的象征";
        else if (cNum == 7)
            OutText = "鱼儿开始接吻，呵呵，你们是否也走入了人生的浪漫？";
        else if (cNum == 8)
            OutText = "一路走来不容易，站在成功的面前，你能看到爱人甜蜜的笑脸么？";

        return OutText;
    }

    private string OutBText(int cNum)
    {
        string OutText = "";
        if (cNum == 1)
            OutText = "爱对方多一点，就喂喂这美丽的鸟儿吧！";
        else if (cNum == 2)
            OutText = "爱情的甜蜜，让小鸟儿越长越大，越来越漂亮……";
        else if (cNum == 3)
            OutText = "恋爱是两个人的事，照顾树上的黄鹂也是两个人的事哦";
        else if (cNum == 4)
            OutText = "可爱的鸟儿已经长大，色彩也变得五彩斑斓……";
        else if (cNum == 5)
            OutText = "加油哦，树上的鸟儿正看着你们笑呢，好甜蜜哦~~";
        else if (cNum == 6)
            OutText = "这美丽的小鸟不只是天空的精灵，更是你们甜美爱情的象征";
        else if (cNum == 7)
            OutText = "鸟儿开始为爱人筑巢，呵呵，你们是否也走入了人生的浪漫？";
        else if (cNum == 8)
            OutText = "一路走来不容易，站在成功的面前，你能看到爱人甜蜜的笑脸么？";

        return OutText;
    }

    private string OutCText(int cNum)
    {
        string OutText = "";
        if (cNum == 1)
            OutText = "爱对方多一点，月亮的光芒就明亮一点！";
        else if (cNum == 2)
            OutText = "爱情的甜蜜包围了月亮，星空也变得浪漫起来……";
        else if (cNum == 3)
            OutText = "恋爱是两个人的事，别忘了多给对方一些问候哦";
        else if (cNum == 4)
            OutText = "天心之皓月见证了你们爱情的成长，但是还要继续加油哦……";
        else if (cNum == 5)
            OutText = "天心之皓月见证了你们爱情的成长，但是还要继续加油哦……";
        else if (cNum == 6)
            OutText = "爱情的气氛让星空变得不再黑暗，深夜不再寒冷……";
        else if (cNum == 7)
            OutText = "月亮偷偷的问你们：感受到爱情的快乐了么？";
        else if (cNum == 8)
            OutText = "一路走来不容易，站在成功的面前，你能看到爱人甜蜜的笑脸么？";

        return OutText;
    }

    private string OutDText(int cNum)
    {
        string OutText = "";
        if (cNum == 1)
            OutText = "青涩的初恋就像稚嫩的小树苗，需要恋人们用心浇灌！";
        else if (cNum == 2)
            OutText = "爱对方多一点就每天浇水多一点！";
        else if (cNum == 3)
            OutText = "爱情的滋润让小树慢慢长大，慢慢长大……";
        else if (cNum == 4)
            OutText = "恋爱是两个人的事，给爱情果树浇水也是两个人的事哦！";
        else if (cNum == 5)
            OutText = "爱情的种子已长成参天大树，树枝是甜蜜，树叶是思念……";
        else if (cNum == 6)
            OutText = "爱情像含苞待放的花骨朵，浓浓的、满满的。";
        else if (cNum == 7)
            OutText = "当爱情伊甸园挂满爱的果实，双宿双栖的快乐就会到来了！";
        else if (cNum == 8)
            OutText = "看到绽放的爱情之花就像看见恋人微笑的脸……";
        else if (cNum == 9)
            OutText = "幸福像花儿一样开满伊甸园的爱情之树。";
        else if (cNum == 10)
            OutText = "幸福像花儿一样开满伊甸园的爱情之树。";

        return OutText;
    }

}