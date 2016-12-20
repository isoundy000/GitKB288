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
using System.Collections.Generic;
using BCW.Common;
using System.Text.RegularExpressions;
using BCW.Files;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;

public partial class Manage_game_farm : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/farm.xml";
    protected string GameName = ub.GetSub("FarmName", "/Controls/farm.xml");//游戏名字
    ub xml = new ub();
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "weihu":
                WeihuPage();//游戏维护
                break;
            case "paihang":
                PaihangPage();//用户排行
                break;
            case "jingyan":
                jingyanPage();
                break;
            case "user_gl":
                user_glPage();//用户查看
                break;
            case "zuowu_dagang":
                zuowu_dagangPage();//作物首页
                break;
            case "zuowuguanli":
                zuowuguanliPage();//作物管理
                break;
            case "zuowu_add":
                zuowu_addPage();//作物增加
                break;
            case "del_zz_id":
                del_zz_idPage();//作物删除
                break;
            case "edit_zw_mess":
                edit_zw_messPage();//作物信息修改
                break;
            case "edit_zw_picture":
                edit_zw_picturePage();//作物图片修改
                break;
            case "daojuguanli":
                daojuguanliPage();//道具管理
                break;
            case "daoju_add":
                daoju_addPage();//道具增加
                break;
            case "daoju_use":
                daoju_usePage();//道具使用情况
                break;
            case "edit_dj_mess":
                edit_dj_messPage();//道具信息修改
                break;
            case "edit_dj_picture":
                edit_dj_picturePage();//道具图片修改
                break;
            case "reset":
                ResetPage();//重置
                break;
            case "peizhi":
                PeizhiPage();//配置管理
                break;
            case "tanwei_gl":
                tanwei_glPage();//摊位管理
                break;
            case "tudi_gl":
                tudi_glPage();//土地管理
                break;
            case "message":
                messagePage();//消息和消费查看
                break;
            case "baoxiang":
                baoxiangPage();//宝箱管理
                break;
            case "bx_add_zz":
                bx_add_zzPage();//从已有中添加到宝箱
                break;
            case "bx_jilu":
                bx_jiluPage();//用户在宝箱抽奖记录
                break;
            case "xiaofei":
                xiaofeiPage();//用户消费情况
                break;
            case "xiaofei_jt":
                xiaofei_jtPage();//消费具体情况
                break;
            case "nuli_gl":
                nuli_glPage();//奴隶管理
                break;
            case "cangku_gl":
                cangku_glPage();//仓库管理
                break;
            case "cangku_cg":
                cangku_cgPage();//仓库成果
                break;
            case "cangku_gl_show":
                cangku_gl_showPage();//仓库usid物品详细显示
                break;
            case "cangku_gl_gai":
                cangku_gl_gaiPage();//仓库物品修改
                break;
            case "cangku_gl_del":
                cangku_gl_delPage();//仓库物品删除
                break;
            case "yingli":
                yingliPage();//盈利分析
                break;
            case "gonggao":
                gonggaoPage();//系统公告
                break;
            case "task":
                taskPage();//任务
                break;
            case "taskdel":
                taskdelPage();//删除任务
                break;
            case "taskedit":
                taskeditPage();//任务修改
                break;
            case "taskadd":
                taskaddPage();//任务增加
                break;
            case "liuyan_gl":
                liuyan_glPage();//留言管理
                break;
            case "del_liuyan":
                del_liuyanPage();//留言删除
                break;
            case "tasklist":
                tasklistPage();//任务完成情况查询
                break;
            case "hecheng_gl":
                hecheng_glPage();//合成管理
                break;
            case "hechenglist":
                hechenglistPage();//合成详细
                break;
            case "hecheng_gai":
                hecheng_gaiPage();//合成修改
                break;
            case "hecheng_del":
                hecheng_delPage();//合成删除
                break;
            case "nuliyuyan_gl":
                nuliyuyan_glPage();//惩罚安抚奴隶语句管理
                break;
            case "nlyy_edit":
                nlyy_editPage();//惩罚安抚奴隶语句修改
                break;
            case "nlyy_del":
                nlyy_delPage();//惩罚安抚奴隶语句删除
                break;
            case "jiyu":
                jiyuPage();//农场寄语管理
                break;
            case "xiaofei_gl":
                xiaofei_glPage();//消费管理
                break;
            case "zhitiao_gl":
                zhitiao_glPage();//纸条管理
                break;
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "" + GameName + "";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("" + GameName + "");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【信息查看】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message") + "\">消息查看</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei") + "\">消费查看</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang") + "\">用户排行</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan") + "\">经验上限</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【管理操作】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_dagang") + "\">作物管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli") + "\">道具管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">宝箱管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl") + "\">奴隶管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tanwei_gl") + "\">摊位管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tudi_gl") + "\">土地管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl") + "\">仓库管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan_gl") + "\">留言管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=hecheng_gl") + "\">合成管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuliyuyan_gl") + "\">语言管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_gl") + "\">消费管理</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao_gl") + "\">纸条管理</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("【系统设置】<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=peizhi") + "\">游戏配置</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=weihu") + "\">游戏维护</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=yingli") + "\">盈利分析</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">重置游戏</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gonggao") + "\">系统公告</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">活动任务</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //游戏维护
    private void WeihuPage()
    {
        Master.Title = "" + GameName + "_游戏维护";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/farm.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string farmStatus = Utils.GetRequest("farmStatus", "post", 2, @"^[0-2]$", "0");//总
            string nlxjStatus = Utils.GetRequest("nlxjStatus", "post", 2, @"^[0-1]$", "0");//奴隶和陷阱
            string bxStatus = Utils.GetRequest("bxStatus", "post", 2, @"^[0-1]$", "0");//宝箱
            string scStatus = Utils.GetRequest("scStatus", "post", 2, @"^[0-1]$", "0");//市场
            string gnStatus = Utils.GetRequest("gnStatus", "post", 2, @"^[0-1]$", "0");//功能
            string tcStatus = Utils.GetRequest("tcStatus", "post", 2, @"^[0-1]$", "0");//偷菜
            string ckStatus = Utils.GetRequest("ckStatus", "post", 2, @"^[0-1]$", "0");//仓库
            string Currency = Utils.GetRequest("Currency", "post", 2, @"^[0-1]$", "0");//兑换
            string liuyanStatus = Utils.GetRequest("liuyanStatus", "post", 2, @"^[0-1]$", "0");//留言
            string XtestID = Utils.GetRequest("XtestID", "all", 1, @"^[^\^]{1,2000}$", "");
            string isshouhuo = Utils.GetRequest("isshouhuo", "post", 2, @"^[0-1]$", "0");//兑换
            string rwStatus = Utils.GetRequest("rwStatus", "post", 2, @"^[0-1]$", "0");//任务
            string zsStatus = Utils.GetRequest("zsStatus", "post", 2, @"^[0-1]$", "0");//赠送
            string ztStatus = Utils.GetRequest("ztStatus", "post", 2, @"^[0-1]$", "0");//纸条

            string stop_IP = Utils.GetRequest("stop_IP", "post", 1, "", "");//IP
            string stop_ZT = Utils.GetRequest("stop_ZT", "post", 1, "", "");//禁止纸条ID

            xml.dss["XtestID"] = XtestID;
            xml.dss["farmStatus"] = farmStatus;
            xml.dss["nlxjStatus"] = nlxjStatus;
            xml.dss["bxStatus"] = bxStatus;
            xml.dss["scStatus"] = scStatus;
            xml.dss["gnStatus"] = gnStatus;
            xml.dss["tcStatus"] = tcStatus;
            xml.dss["ckStatus"] = ckStatus;
            xml.dss["Currency"] = Currency;
            xml.dss["liuyanStatus"] = liuyanStatus;
            xml.dss["isshouhuo"] = isshouhuo;
            xml.dss["rwStatus"] = rwStatus;
            xml.dss["zsStatus"] = zsStatus;
            xml.dss["ztStatus"] = ztStatus;
            xml.dss["stop_IP"] = stop_IP;
            xml.dss["stop_ZT"] = stop_ZT;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "_游戏维护", "设置成功，正在返回..", Utils.getUrl("farm.aspx?act=weihu&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;游戏维护"));
            string strText = "总游戏状态：/,奴隶和陷阱：/,宝箱功能：/,交易市场：/,页面收割：/,功能：/,偷菜：/,仓库：/,赠送：/,兑换：/,留言：/,任务：/,纸条：/,测试ID：(#号分隔)/,禁止进入IP：(#号分隔)/,禁止纸条ID：(#号分隔)/,,";
            string strName = "farmStatus,nlxjStatus,bxStatus,scStatus,isshouhuo,gnStatus,tcStatus,ckStatus,zsStatus,Currency,liuyanStatus,rwStatus,ztStatus,XtestID,stop_IP,stop_ZT,act,backurl";
            string strType = "select,select,select,select,select,select,select,select,select,select,select,select,select,big,big,text,hidden,hidden";//textarea
            string strValu = "" + xml.dss["farmStatus"] + "'" + xml.dss["nlxjStatus"] + "'" + xml.dss["bxStatus"] + "'" + xml.dss["scStatus"] + "'" + xml.dss["isshouhuo"] + "'" + xml.dss["gnStatus"] + "'" + xml.dss["tcStatus"] + "'" + xml.dss["ckStatus"] + "'" + xml.dss["zsStatus"] + "'" + xml.dss["Currency"] + "'" + xml.dss["liuyanStatus"] + "'" + xml.dss["rwStatus"] + "'" + xml.dss["ztStatus"] + "'" + xml.dss["XtestID"] + "'" + xml.dss["stop_IP"] + "'" + xml.dss["stop_ZT"] + "'weihu'" + Utils.getPage(0) + "";//stop_IP
            string strEmpt = "0|正常|1|维护|2|内测,0|正常|1|维护,0|正常|1|维护,0|正常|1|维护,0|正常|1|维护,0|开启|1|关闭,0|正常|1|维护,0|正常|1|维护,0|正常|1|维护,0|开启|1|关闭,0|开启|1|关闭,0|开启|1|关闭,0|开启|1|关闭,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        string aa = xml.dss["XtestID"].ToString();
        if (aa != "")
        {
            string[] sNum = Regex.Split(aa, "#");
            builder.Append("测试ID：《" + sNum.Length + "》个<br/>");
            for (int n = 0; n < sNum.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(sNum[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(sNum[n] + ",");
                }
            }
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //各经验上限查看
    private void jingyanPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;各经验上限查看");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-8]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "_播种经验";
            builder.Append("<h style=\"color:red\">播种经验</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=1") + "\">播种经验</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "" + GameName + "_种草虫经验";
            builder.Append("<h style=\"color:red\">种草虫经验</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=2") + "\">种草虫经验</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "" + GameName + "_除草虫水经验(自己)";
            builder.Append("<h style=\"color:red\">除草虫水经验(自己)</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=3") + "\">除草虫水经验(自己)</a>" + "|");
        }
        if (ptype == 4)
        {
            Master.Title = "" + GameName + "_除草虫水经验(好友)";
            builder.Append("<h style=\"color:red\">除草虫水经验(好友)</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=4") + "\">除草虫水经验(好友)</a>" + "|");
        }
        if (ptype == 5)
        {
            Master.Title = "" + GameName + "_除草虫水次数";
            builder.Append("<h style=\"color:red\">除草虫水次数</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=5") + "\">除草虫水次数</a>" + "|");
        }
        if (ptype == 6)
        {
            Master.Title = "" + GameName + "_种草虫次数";
            builder.Append("<h style=\"color:red\">种草虫次数</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=6") + "\">种草虫次数</a>" + "|");
        }
        if (ptype == 7)
        {
            Master.Title = "" + GameName + "_施肥次数";
            builder.Append("<h style=\"color:red\">施肥次数</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=7") + "\">施肥次数</a>" + "|");
        }
        if (ptype == 8)
        {
            Master.Title = "" + GameName + "_赠送次数";
            builder.Append("<h style=\"color:red\">赠送次数</h>" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jingyan&amp;ptype=8") + "\">赠送次数</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
        {
            strWhere = "big_bozhong>0";
            strOrder = "big_bozhong desc";
        }
        if (ptype == 2)
        {
            strWhere = "big_shihuai>0";
            strOrder = "big_shihuai desc";
        }
        if (ptype == 3)
        {
            strWhere = "big_zjcaozuo>0";
            strOrder = "big_zjcaozuo desc";
        }
        if (ptype == 4)
        {
            strWhere = "big_bangmang>0";
            strOrder = "big_bangmang desc";
        }
        if (ptype == 5)
        {
            strWhere = "big_cccishu>0";
            strOrder = "big_cccishu desc";
        }
        if (ptype == 6)
        {
            strWhere = "big_zfcishu>0";
            strOrder = "big_zfcishu desc";
        }
        if (ptype == 7)
        {
            strWhere = "big_shifei>0";
            strOrder = "big_shifei desc";
        }
        if (ptype == 8)
        {
            strWhere = "zengsongnum>0";
            strOrder = "zengsongnum desc";
        }

        IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_user n in listFarm)
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
                //获取id对应的用户名
                string mename = new BCW.BLL.User().GetUsName(n.usid);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + n.usid + ")</a>:");
                if (ptype == 1)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_bozhong) + "</h>");
                }
                if (ptype == 2)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_shihuai) + "</h>");
                }
                if (ptype == 3)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_zjcaozuo) + "</h>");
                }
                if (ptype == 4)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_bangmang) + "</h>");
                }
                if (ptype == 5)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_cccishu) + "次</h>");
                }
                if (ptype == 6)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_zfcishu) + "次</h>");
                }
                if (ptype == 7)
                {
                    builder.Append("<h style =\"color:red\">" + (n.big_shifei) + "次</h>");
                }
                if (ptype == 8)
                {
                    builder.Append("<h style =\"color:red\">" + (n.zengsongnum) + "次</h>");
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
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //用户排行
    private void PaihangPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;用户排行");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-7]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "_排行榜.按等级";
            builder.Append("<h style=\"color:red\">按等级</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=1") + "\">按等级</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "" + GameName + "_排行榜.按金币";
            builder.Append("<h style=\"color:red\">按金币</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=2") + "\">按金币</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "" + GameName + "_排行榜.按签到";
            builder.Append("<h style=\"color:red\">按签到</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=3") + "\">按签到</a>" + "|");
        }
        if (ptype == 4)
        {
            Master.Title = "" + GameName + "_排行榜.按偷取";
            builder.Append("<h style=\"color:red\">按偷取</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=4") + "\">按偷取</a>" + "|");
        }
        if (ptype == 5)
        {
            Master.Title = "" + GameName + "_排行榜.按收获";
            builder.Append("<h style=\"color:red\">按收获</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=5") + "\">按收获</a>" + "|");
        }
        if (ptype == 6)
        {
            Master.Title = "" + GameName + "_排行榜.按合成";
            builder.Append("<h style=\"color:red\">按合成</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=6") + "\">按合成</a>" + "|");
        }
        if (ptype == 7)
        {
            Master.Title = "" + GameName + "_排行榜.按人气";
            builder.Append("<h style=\"color:red\">按人气" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihang&amp;ptype=7") + "\">按人气</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1 || ptype == 2)
        {
            #region
            if (ptype == 1)
            {
                strWhere = "";
                strOrder = " (Grade) Desc";
            }
            if (ptype == 2)
            {
                strWhere = "";
                strOrder = "(Goid) Desc";
            }
            IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listFarm)
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
                    //获取id对应的用户名
                    string mename = new BCW.BLL.User().GetUsName(n.usid);
                    if (ptype == 1)
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>Lv<h style=\"color:red\">" + (n.Grade) + "级</h>");
                    }
                    if (ptype == 2)
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei&amp;usid=" + n.usid + "") + "\">" + mename + "</a>:<h style=\"color:red\">" + (n.Goid) + "</h>金币");
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
            #endregion
        }
        if (ptype == 3)
        {
            #region
            strWhere = "SignTotal>0";
            strOrder = "SignTotal Desc";
            // 开始读取列表
            IList<BCW.farm.Model.NC_user> listUser = new BCW.farm.BLL.NC_user().GetUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listUser.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listUser)
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
                    string OutText = string.Empty;
                    OutText = "(" + n.SignTotal + "次)";
                    string mename = new BCW.BLL.User().GetUsName(n.usid);//用户姓名
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>" + OutText + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        if (ptype == 4 || ptype == 5)
        {
            #region
            string bv = string.Empty;
            if (ptype == 4)
            {
                bv = "tou_nums";
            }
            else
            {
                bv = "get_nums";
            }
            DataSet dd = new BCW.farm.BLL.NC_GetCrop().GetList("UsID,sum(" + bv + ") AS aa", "" + bv + ">0 GROUP BY UsID ORDER BY aa DESC");
            if (dd != null && dd.Tables[0].Rows.Count > 0)
            {
                recordCount = dd.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(dd.Tables[0].Rows[koo + i]["usid"]);
                    int id = Convert.ToInt32(dd.Tables[0].Rows[koo + i]["aa"]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>:");
                    if (ptype == 4)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=9&amp;usid=" + UsID + "") + "\">[" + id + "个]</a>");
                    }
                    else if (ptype == 5)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=8&amp;usid=" + UsID + "") + "\">[" + id + "个]</a>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关排行记录.."));
            }
            #endregion
        }
        if (ptype == 6)
        {
            #region
            DataSet hecheng = new BCW.farm.BLL.NC_hecheng().GetList("UsID,SUM(all_num) AS bb", "all_num>0 GROUP BY UsID ORDER BY bb DESC,UsID ASC");
            if (hecheng != null && hecheng.Tables[0].Rows.Count > 0)
            {
                recordCount = hecheng.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    int usid = int.Parse(hecheng.Tables[0].Rows[koo + i]["usid"].ToString());
                    int bb = int.Parse(hecheng.Tables[0].Rows[koo + i]["bb"].ToString());

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>:<h style=\"color:red\">" + bb + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        if (ptype == 7)
        {
            #region
            DataSet shoudao = new BCW.BLL.Shopuser().GetList("UsID,SUM(Total) AS bb", "PIC=1  GROUP BY UsID ORDER BY bb DESC,UsID ASC");
            if (shoudao != null && shoudao.Tables[0].Rows.Count > 0)
            {
                recordCount = shoudao.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    int usid = int.Parse(shoudao.Tables[0].Rows[koo + i]["usid"].ToString());
                    int bb = int.Parse(shoudao.Tables[0].Rows[koo + i]["bb"].ToString());


                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>:收到<h style=\"color:red\">" + bb + "</h>朵花");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=user_gl") + "\"><h style=\"color:red\">用户查看>></h></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiyu") + "\"><h style=\"color:red\">农场寄语>></h></a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //作物首页
    private void zuowu_dagangPage()
    {
        Master.Title = "" + GameName + "_作物管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;作物管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.蔬菜类:<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=1") + "\">" + new BCW.farm.BLL.NC_shop().get_typenum(1) + "个</a>.<br/>");
        builder.Append("2.水果类:<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=2") + "\">" + new BCW.farm.BLL.NC_shop().get_typenum(2) + "个</a>.<br/>");
        builder.Append("3.鲜花类:<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=3") + "\">" + new BCW.farm.BLL.NC_shop().get_typenum(3) + "个</a>.<br/>");
        builder.Append("4.有机作物:<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=4") + "\">" + new BCW.farm.BLL.NC_shop().get_typenum(4) + "个</a>.<br/>");
        builder.Append("5.可赠送类:<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=5") + "\">" + new BCW.farm.BLL.NC_shop().get_typenum(5) + "个</a>.");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //作物管理
    private void zuowuguanliPage()
    {
        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "xiao", "type", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));//
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int xiao = Utils.ParseInt(Utils.GetRequest("xiao", "all", 1, @"^[1-9]\d*$", "0"));
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, @"^[1-5]\d*$", "1"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_dagang") + "\">作物管理</a>");
        if (type == 1)
        {
            strWhere = "zhonglei=1";
            Master.Title = "" + GameName + "_作物管理_蔬菜管理";
            builder.Append("&gt;蔬菜管理");
        }
        if (type == 2)
        {
            strWhere = "zhonglei=2";
            Master.Title = "" + GameName + "_作物管理_水果管理";
            builder.Append("&gt;水果管理");
        }
        if (type == 3)
        {
            strWhere = "zhonglei=3";
            Master.Title = "" + GameName + "_作物管理_鲜花管理";
            builder.Append("&gt;鲜花管理");
        }
        if (type == 4)
        {
            strWhere = "zhonglei=4";
            Master.Title = "" + GameName + "_作物管理_有机作物管理";
            builder.Append("&gt;有机作物管理");
        }
        if (type == 5)
        {
            strWhere = "zhonglei=5";
            Master.Title = "" + GameName + "_作物管理_可赠送类管理";
            builder.Append("&gt;可赠送类管理");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));
        if (xiao == 1)
            strWhere = "";

        strOrder = "grade asc,id asc";
        IList<BCW.farm.Model.NC_shop> listFarm = new BCW.farm.BLL.NC_shop().GetNC_shops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_shop n in listFarm)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getPage("farm.aspx?act=edit_zw_mess&amp;id=" + n.num_id + "&amp;type=" + type + "") + "\">" + n.name + "</a>(等级" + n.grade + ")");//，ID" + n.num_id + "

                if (xiao == 1)
                {
                    if (!new BCW.farm.BLL.NC_baoxiang().Exists_bxzzdj(n.num_id, 1))
                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=bx_add_zz&amp;type=1&amp;id=" + n.num_id + "") + "\"><h style=\"color:red\">[添加]</h></a>");//红色
                    else
                        builder.Append(".[已添加]");
                }
                else
                {
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=edit_zw_picture&amp;id=" + n.num_id + "") + "\">[修改图片]</a>");
                    if (Convert.ToInt32(ub.GetSub("farmStatus", xmlPath)) == 1)
                        builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=del_zz_id&amp;id=" + n.ID + "&amp;type=" + type + "") + "\">[删]</a>");
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

        builder.Append(Out.Tab("<div>", Out.Hr()));


        if (xiao == 1)
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=baoxiang") + "\">返回上级</a>");
        else
        {
            if (type == 1)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;zl=1") + "\">蔬菜添加</a><br/>");
            else if (type == 2)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;zl=2") + "\">水果添加</a><br/>");
            else if (type == 3)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;zl=3") + "\">鲜花添加</a><br/>");
            else if (type == 4)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;zl=4") + "\">有机作物添加</a><br/>");
            else if (type == 5)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;zl=5") + "\">可赠送类添加</a><br/>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add") + "\">作物添加</a><br/>");

            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=zuowu_dagang") + "\">返回上级</a>");
        }

        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //作物增加
    private void zuowu_addPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("温馨提示", "使用彩版，才能够上传图片，页面更直观，更快捷！正在切换进入彩版添加图片...", "farm.aspx?act=zuowu_add&amp;info=" + info + "&amp;ve=2a&amp;u=" + Utils.getstrU(), "1");
        }

        Master.Title = "商店.添加作物";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_dagang") + "\">作物管理</a>&gt;添加作物");
        builder.Append(Out.Tab("</div>", ""));


        if (info == "ok")
        {
            string n = Utils.GetRequest("name", "post", 2, @"^[^\^]{1,10}$", "作物名称限1-10字内");//作物名称
            string name = n.Replace(" ", "");
            int num_id = Utils.ParseInt(Utils.GetRequest("num_id", "post", 2, @"^[1-9]\d*$", "作物ID填写出错"));//作物ID
            int grade = Utils.ParseInt(Utils.GetRequest("grade", "post", 2, @"^[0-9]\d*$", "作物等级填写出错"));//作物等级
            int jidu = Utils.ParseInt(Utils.GetRequest("jidu", "post", 2, @"^[1-9]\d*$", "作物季度填写出错"));//作物季度
            int jidu_time = Utils.ParseInt(Utils.GetRequest("jidu_time", "post", 2, @"^[1-9]\d*$", "作物成熟所需时间(分钟)填写出错"));//作物成熟所需时间(分钟)
            int price_in = Utils.ParseInt(Utils.GetRequest("price_in", "post", 2, @"^[1-9]\d*$", "种子买进价格填写出错"));//种子买进价格
            int price_out = Utils.ParseInt(Utils.GetRequest("price_out", "post", 2, @"^[1-9]\d*$", "果实卖出价格填写出错"));//果实卖出价格
            int experience = Utils.ParseInt(Utils.GetRequest("experience", "post", 2, @"^[1-9]\d*$", "每个季度收获的经验填写出错"));//每个季度收获的经验
            string output = Utils.GetRequest("output", "post", 2, @"^[^\^]{1,10}$", "作物产量限1-10字内");//作物产量
            int type = Utils.ParseInt(Utils.GetRequest("type", "post", 2, @"^[1-4]\d*$", "适合种植的土地类型填写出错"));//适合种植的土地类型
            int iszengsong = Utils.ParseInt(Utils.GetRequest("iszengsong", "post", 2, @"^[0-1]\d*$", "赠送类型填写出错"));//1赠送0不赠送
            int zhonglei = Utils.ParseInt(Utils.GetRequest("zhonglei", "post", 2, @"^[1-9]\d*$", "种类填写出错"));
            int caotime = Utils.ParseInt(Utils.GetRequest("caotime", "post", 2, @"^[1-9]\d*$", "出现草所需的时间填写出错"));
            int chongtime = Utils.ParseInt(Utils.GetRequest("chongtime", "post", 2, @"^[1-9]\d*$", "出现虫所需的时间填写出错"));
            int shuitime = Utils.ParseInt(Utils.GetRequest("shuitime", "post", 2, @"^[1-9]\d*$", "需浇水时间填写出错"));
            int meili = Utils.ParseInt(Utils.GetRequest("meili", "post", 2, @"^[0-9]\d*$", "魅力值填写出错"));
            int tili = Utils.ParseInt(Utils.GetRequest("tili", "post", 2, @"^[0-9]\d*$", "体力值填写出错"));
            //判断该作物是否存在
            if (new BCW.farm.BLL.NC_shop().Exists_zzid2(num_id))
            {
                Utils.Error("该作物ID已存在.", "");
            }
            else if (new BCW.farm.BLL.NC_shop().Exists_zzmc(name))
            {
                Utils.Error("该作物名称已存在.", "");
            }
            else
            {
                BCW.farm.Model.NC_shop oo = new BCW.farm.Model.NC_shop();
                oo.name = name;
                oo.num_id = num_id;
                oo.grade = grade;
                oo.jidu = jidu;
                oo.jidu_time = jidu_time;
                oo.price_in = price_in;
                oo.price_out = price_out;
                oo.experience = experience;
                oo.output = output;
                oo.type = type;
                oo.picture = "";
                oo.iszengsong = iszengsong;
                oo.zhonglei = zhonglei;
                oo.caotime = caotime;
                oo.chongtime = chongtime;
                oo.shuitime = shuitime;
                oo.meili = meili;
                oo.tili = tili;
                new BCW.farm.BLL.NC_shop().Add(oo);//11
                Utils.Success("添加作物", "添加作物信息成功,请上传图片.", Utils.getUrl("farm.aspx?act=zuowu_add&amp;info=ok2&amp;id=" + num_id + ""), "1");
            }
        }
        else if (info == "ok2")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "作物ID选择出错"));//作物ID
            if (new BCW.farm.BLL.NC_shop().Exists_zzid(id))
            {
                BCW.farm.Model.NC_shop up = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你正在为作物:【" + up.name + "】添加图片：");
                builder.Append(Out.Tab("</div>", ""));

                string ssUpType = string.Empty;
                string ssText = string.Empty;
                string ssName = string.Empty;
                string ssType = string.Empty;
                string ssValu = string.Empty;
                string ssEmpt = string.Empty;
                string ssIdea = string.Empty;
                string ssOthe = string.Empty;
                string strText = string.Empty;
                string strName = string.Empty;
                string strType = string.Empty;
                string strValu = string.Empty;
                string strEmpt = string.Empty;

                for (int i = 0; i < 5; i++)
                {
                    string y = ",";
                    ssText = ssText + y + "" + ssUpType + "第" + (i + 1) + "张图片:/,";
                    ssName = ssName + y + "file" + (i + 1) + y;
                    ssType = ssType + y + "file" + y;
                    ssValu = ssValu + "''";
                    ssEmpt = ssEmpt + y + y;
                }
                string strUpgroup = string.Empty;
                strUpgroup = "" + strUpgroup;

                ssText = ssText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
                ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act,info,id";
                ssType = ssType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden";
                ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "'zuowu_add'ok3'" + id + "";
                ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
                ssIdea = "/";
                ssOthe = "确定添加,farm.aspx,post,2,red";
                builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
            }
            else
                Utils.Error("该作物ID不存在.", "");
        }
        else if (info == "ok3")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "作物ID选择出错"));//作物ID

            if (new BCW.farm.BLL.NC_shop().Exists_zzid(id))
            {
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string DirPath = string.Empty;
                string fileName = string.Empty;
                string fileExtension = string.Empty;
                string hebing = string.Empty;
                string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
                int UpLength = 2000;
                //遍历File表单元素
                try
                {
                    //先判断是否符合要求,因为下面提交后就添加到文件夹里
                    for (int iFile = 0; iFile < 5; iFile++)
                    {
                        //检查文件扩展名字
                        System.Web.HttpPostedFile postedFile = files[iFile];
                        fileName = System.IO.Path.GetFileName(postedFile.FileName);
                        //检查是否允许上传格式
                        if (UpExt.IndexOf(fileExtension) == -1)
                        {
                            Utils.Error("第" + iFile + "张图片类型选择错误！", "");
                        }
                        //非法上传
                        if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                        {
                            Utils.Error("第" + iFile + "张非法上传,请重新选择.", "");
                        }
                        if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))//3M
                        {
                            Utils.Error("第" + iFile + "张图片大小已超过2Mb.", "");
                        }
                        if (fileName == "")
                        {
                            Utils.Error("第" + iFile + "张图片不能为空.", "");
                        }
                    }
                }
                catch
                {
                    //Utils.Error("上传图片出错,001.", "");
                }

                for (int iFile = 0; iFile < 5; iFile++)
                {
                    //检查文件扩展名字
                    System.Web.HttpPostedFile postedFile = files[iFile];
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                        string Path = "/bbs/game/img/farm/zuowu/";
                        if (FileTool.CreateDirectory(Path, out DirPath))
                        {
                            //生成随机文件名
                            fileName = DT.getDateTimeNum() + iFile + fileExtension;
                            string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                            postedFile.SaveAs(SavePath);
                            //=============================图片木马检测,包括TXT===========================
                            string vSavePath = SavePath;
                            if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                            {
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
                                    Utils.Error("选择图片文件出错.", "");
                            }
                        }
                        if (iFile == 4)
                            hebing = hebing + DirPath + fileName;
                        else
                            hebing = hebing + DirPath + fileName + ",";
                    }
                    else
                        Utils.Error("请选择图片文件.", "");
                }
                //把路径添加到字段
                builder.Append(hebing);
                new BCW.farm.BLL.NC_shop().update_shop("picture='" + hebing + "'", "num_id=" + id + "");
                Utils.Success("添加图片", "添加图片成功.", Utils.getUrl("farm.aspx?act=zuowu_add"), "1");
            }
            else
                Utils.Error("该作物ID不存在.", "");
        }
        else
        {
            string type = (Utils.GetRequest("type", "all", 1, @"^[1-9]\d*$", ""));
            int zl = Utils.ParseInt(Utils.GetRequest("zl", "all", 1, @"^[1-9]\d*$", "1"));
            int zengsong = 0;
            if (zl == 5) { zengsong = 1; } else { zengsong = 0; }
            BCW.farm.Model.NC_shop ui = new BCW.farm.BLL.NC_shop().GetNC_shop_last(1);//查询数据库最后一条数据
            int yy = 1;
            try
            {
                yy = ui.num_id + 1;
            }
            catch
            {
                yy = 1;
            }
            string strText = "作物名称:/,作物ID:(已自动填写)/,作物种植所需等级:/,作物生长季度:/,作物成熟所需时间(分钟每个季度):/,种子买进价格:/,果实卖出价格:/,每个季度收获的经验:/,作物产量:/,适合种植的土地类型:/(1普通2红3黑4金10珍稀不可购买)/,果实是否赠送(0不赠送1赠送):/,作物种类：/(1蔬菜2水果3鲜花4有机类5赠送类)/,出现草所需的分钟：(刷新机使用)/,出现虫所需的分钟：(刷新机使用)/,需浇水的分钟：(刷新机使用)/,魅力值：/,体力：/,,,";
            string strName = "name,num_id,grade,jidu,jidu_time,price_in,price_out,experience,output,type,iszengsong,zhonglei,caotime,chongtime,shuitime,meili,tili,act,info";
            string strType = "text,hidden,num,num,num,num,num,num,text,num,num,num,num,num,num,num,num,hidden,hidden";
            string strValu = "'" + yy + "''''''''" + type + "'" + zengsong + "'" + zl + "''''''zuowu_add'ok";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定添加,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br/>1.作物产量填写格式:10,10。说明：(产10/剩10)<br/>2.珍稀作物属于赠送类,土地类型是10,商店不可购买,只用作宝箱抽奖.<br/>3.3个刷新机的时间填写格式：<br/>(季度成熟时间-3)/3。例如牧草成熟时间是31分钟,则刷新机时间为(31-3)/3=9.(去余数)");
            builder.Append(Out.Tab("</div>", ""));
        }
        if (info == "ok2")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br/>第一张图片指小叶子,第二张指大叶子,第三张指初熟,第四张指成熟,第五张指商店图鉴.");
            builder.Append(Out.Tab("</div>", ""));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //作物删除
    private void del_zz_idPage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "1"));
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, @"^[1-9]\d*$", "1"));
        BCW.farm.Model.NC_shop a = new BCW.farm.BLL.NC_shop().GetNC_shop(id);
        string[] p = a.picture.Split(',');
        //删除原文件
        for (int i = 0; i < 5; i++)
        {
            try
            {
                string SavePath2 = System.Web.HttpContext.Current.Request.MapPath(p[i]);
                System.IO.File.Delete(SavePath2);
            }
            catch
            {
            }
        }
        new BCW.farm.BLL.NC_shop().Delete(id);
        Utils.Success("删除作物", "删除作物成功,正在返回.", Utils.getUrl("farm.aspx?act=zuowuguanli&amp;type=" + type + ""), "1");
    }

    //作物信息修改
    private void edit_zw_messPage()
    {
        int type_1 = Utils.ParseInt(Utils.GetRequest("type", "all", 2, @"^[1-9]\d*$", "type类型出错1"));
        Master.Title = "" + GameName + "_作物信息修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli") + "\">作物管理</a>");
        if (type_1 == 1)
        {
            builder.Append("&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=1") + "\">蔬菜管理</a>&gt;作物信息修改");
        }
        else if (type_1 == 2)
        {
            builder.Append("&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=2") + "\">水果管理</a>&gt;作物信息修改");
        }
        else if (type_1 == 3)
        {
            builder.Append("&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=3") + "\">鲜花管理</a>&gt;作物信息修改");
        }
        else if (type_1 == 4)
        {
            builder.Append("&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=4") + "\">有机管理</a>&gt;作物信息修改");
        }
        else if (type_1 == 5)
        {
            builder.Append("&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=5") + "\">可赠送管理</a>&gt;作物信息修改");
        }
        builder.Append(Out.Tab("</div>", ""));

        int name_id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择作物ID出错"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string n = Utils.GetRequest("name", "post", 2, @"^[^\^]{1,10}$", "作物名称限1-10字内");//作物名称
            string name = n.Replace(" ", "");
            int grade = Utils.ParseInt(Utils.GetRequest("grade", "post", 2, @"^[0-9]\d*$", "作物等级填写出错"));//作物等级
            int jidu = Utils.ParseInt(Utils.GetRequest("jidu", "post", 2, @"^[1-9]\d*$", "作物季度填写出错"));//作物季度
            int jidu_time = Utils.ParseInt(Utils.GetRequest("jidu_time", "post", 2, @"^[1-9]\d*$", "作物成熟所需时间(分钟)填写出错"));//作物成熟所需时间(分钟)
            int price_in = Utils.ParseInt(Utils.GetRequest("price_in", "post", 2, @"^[1-9]\d*$", "种子买进价格填写出错"));//种子买进价格
            int price_out = Utils.ParseInt(Utils.GetRequest("price_out", "post", 2, @"^[1-9]\d*$", "果实卖出价格填写出错"));//果实卖出价格
            int experience = Utils.ParseInt(Utils.GetRequest("experience", "post", 2, @"^[1-9]\d*$", "每个季度收获的经验填写出错"));//每个季度收获的经验
            string output = Utils.GetRequest("output", "post", 2, @"^[^\^]{1,10}$", "作物产量限1-10字内");//作物产量
            int type = Utils.ParseInt(Utils.GetRequest("type", "post", 2, @"^[1-4]\d*$", "适合种植的土地类型填写出错"));//适合种植的土地类型
            int iszengsong = Utils.ParseInt(Utils.GetRequest("iszengsong", "post", 2, @"^[0-1]\d*$", "赠送类型填写出错"));//1赠送0不赠送
            int zhonglei = Utils.ParseInt(Utils.GetRequest("zhonglei", "post", 2, @"^[1-9]\d*$", "种类填写出错"));//caotime,chongtime,shuitime
            int caotime = Utils.ParseInt(Utils.GetRequest("caotime", "post", 2, @"^[1-9]\d*$", "出现草所需的时间填写出错"));
            int chongtime = Utils.ParseInt(Utils.GetRequest("chongtime", "post", 2, @"^[1-9]\d*$", "出现虫所需的时间填写出错"));
            int shuitime = Utils.ParseInt(Utils.GetRequest("shuitime", "post", 2, @"^[1-9]\d*$", "需浇水时间填写出错"));
            int meili = Utils.ParseInt(Utils.GetRequest("meili", "post", 2, @"^[0-9]\d*$", "魅力值填写出错"));
            int tili = Utils.ParseInt(Utils.GetRequest("tili", "post", 2, @"^[0-9]\d*$", "体力值填写出错"));
            int type_11 = Utils.ParseInt(Utils.GetRequest("type", "all", 1, @"^[1-9]\d*$", "1"));
            new BCW.farm.BLL.NC_shop().update_shop("name='" + name + "',caotime=" + caotime + ",chongtime=" + chongtime + ",shuitime=" + shuitime + ",grade=" + grade + ",jidu=" + jidu + ",jidu_time=" + jidu_time + ",price_in=" + price_in + " ,price_out=" + price_out + ",experience='" + experience + "',output='" + output + "',type=" + type + ",iszengsong=" + iszengsong + ",zhonglei=" + zhonglei + ",meili=" + meili + ",tili=" + tili + "", "num_id='" + name_id + "'");
            Utils.Success("修改作物", "修改作物信息成功.", Utils.getUrl("farm.aspx?act=edit_zw_mess&amp;id=" + name_id + "&amp;type=" + type_11 + ""), "1");
        }
        else
        {
            BCW.farm.Model.NC_shop we = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);

            string strText = "作物名称:/,作物种植所需等级:/,作物生长季度:/,作物成熟所需时间(分钟):/,种子买进价格:/,果实卖出价格:/,每个季度收获的经验:/,作物产量:/,适合种植的土地类型:/(1普通2红3黑4金)/,果实是否赠送(0不赠送1赠送):/,作物种类：/(1蔬菜2水果3鲜花4有机类5赠送类)/,出现草所需的分钟：(刷新机使用)/,出现虫所需的分钟：(刷新机使用)/,需浇水的分钟：(刷新机使用)/,魅力值：/,体力：/,,,";
            string strName = "name,grade,jidu,jidu_time,price_in,price_out,experience,output,type,iszengsong,zhonglei,caotime,chongtime,shuitime,meili,tili,act,info";
            string strType = "text,num,num,num,num,num,num,text,num,num,num,num,num,num,num,num,hidden,hidden,hidden";
            string strValu = "" + we.name + "'" + we.grade + "'" + we.jidu + "'" + we.jidu_time + "'" + we.price_in + "'" + we.price_out + "'" + we.experience + "'" + we.output + "'" + we.type + "'" + we.iszengsong + "'" + we.zhonglei + "'" + we.caotime + "'" + we.chongtime + "'" + we.shuitime + "'" + we.meili + "'" + we.tili + "'zuowu_add'ok";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx?act=edit_zw_mess&amp;id=" + name_id + "&amp;type=" + type_1 + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("温馨提示:<br/>作物产量填写格式:10,10");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli&amp;type=" + type_1 + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //作物图片修改
    private void edit_zw_picturePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择作物ID出错"));
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("温馨提示", "上传或修改图片请使用彩版！正在切换进入...", "farm.aspx?act=edit_zw_picture&amp;id=" + id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        Master.Title = "" + GameName + "_作物图片修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli") + "\">作物管理</a>&gt;作物图片修改");
        builder.Append(Out.Tab("</div>", ""));


        BCW.farm.Model.NC_shop we = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
        string[] ab = we.picture.Split(',');
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string r = string.Empty;
        if (info == "ok")
        {
            int yy = Utils.ParseInt(Utils.GetRequest("ww", "all", 2, @"^[0-4]\d*$", "选择图片出错"));
            if (yy == 0)
                r = "一";
            else if (yy == 1)
                r = "二";
            else if (yy == 2)
                r = "三";
            else if (yy == 3)
                r = "四";
            else
                r = "五";


            if (new BCW.farm.BLL.NC_shop().Exists_zzid(id))
            {
                string ssUpType = string.Empty;
                string ssText = string.Empty;
                string ssName = string.Empty;
                string ssType = string.Empty;
                string ssValu = string.Empty;
                string ssEmpt = string.Empty;
                string ssIdea = string.Empty;
                string ssOthe = string.Empty;
                string strText = string.Empty;
                string strName = string.Empty;
                string strType = string.Empty;
                string strValu = string.Empty;
                string strEmpt = string.Empty;

                for (int i = 0; i < 1; i++)
                {
                    string y = ",";
                    ssText = ssText + y + "" + ssUpType + "正在修改第" + r + "张图片，请选择:/,";
                    ssName = ssName + y + "file" + (i + 1) + y;
                    ssType = ssType + y + "file" + y;
                    ssValu = ssValu + "''";
                    ssEmpt = ssEmpt + y + y;
                }
                string strUpgroup = string.Empty;
                strUpgroup = "" + strUpgroup;

                ssText = ssText + Utils.Mid(strText, 1, strText.Length) + ",,,,,";
                ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act,info,id,num";
                ssType = ssType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden,hidden";
                ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "'edit_zw_picture'ok3'" + id + "'" + yy + "";
                ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,,";
                ssIdea = "";
                ssOthe = "确定修改,farm.aspx,post,2,red";
                builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
            }
            else
                Utils.Error("该作物ID不存在.", "");
        }
        else if (info == "ok3")
        {
            int num = Utils.ParseInt(Utils.GetRequest("num", "all", 1, @"^[0-4]\d*$", "0"));
            string tt = string.Empty;
            if (num == 0)
                tt = ab[0];
            else if (num == 1)
                tt = ab[1];
            else if (num == 2)
                tt = ab[2];
            else if (num == 3)
                tt = ab[3];
            else
                tt = ab[4];

            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string DirPath = string.Empty;
            string fileName = string.Empty;
            string fileExtension = string.Empty;
            string hebing = string.Empty;
            string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
            int UpLength = 2000;
            try
            {
                //先判断是否符合要求,因为下面提交后就添加到文件夹里
                for (int iFile = 0; iFile < 1; iFile++)
                {
                    //检查文件扩展名字
                    System.Web.HttpPostedFile postedFile = files[iFile];
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        Utils.Error("第" + iFile + "张图片类型选择错误！", "");
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        Utils.Error("第" + iFile + "张非法上传,请重新选择.", "");
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))//3M
                    {
                        Utils.Error("第" + iFile + "张图片大小已超过2Mb.", "");
                    }
                    if (fileName == "")
                    {
                        Utils.Error("第" + iFile + "张图片不能为空.", "");
                    }
                }
            }
            catch
            {

            }
            for (int iFile = 0; iFile < 1; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                fileName = System.IO.Path.GetFileName(postedFile.FileName);//得到最原始的图片名字
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();//得到图片的格式
                    string Path = "/bbs/game/img/farm/zuowu/";
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        //删除原文件
                        try
                        {
                            string SavePath2 = System.Web.HttpContext.Current.Request.MapPath(tt);
                            System.IO.File.Delete(SavePath2);
                        }
                        catch
                        {
                        }
                        //保存新文件
                        postedFile.SaveAs(SavePath);
                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                        {
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
                                Utils.Error("选择图片文件出错.", "");
                        }
                    }
                }
                else
                    Utils.Error("请选择图片文件.", "");
            }
            //把路径添加到字段
            if (num == 0)
                hebing = DirPath + fileName + "," + ab[1] + "," + ab[2] + "," + ab[3] + "," + ab[4];
            else if (num == 1)
                hebing = ab[0] + "," + DirPath + fileName + "," + ab[2] + "," + ab[3] + "," + ab[4];
            else if (num == 2)
                hebing = ab[0] + "," + ab[1] + "," + DirPath + fileName + "," + ab[3] + "," + ab[4];
            else if (num == 3)
                hebing = ab[0] + "," + ab[1] + "," + ab[2] + "," + DirPath + fileName + "," + ab[4];
            else
                hebing = ab[0] + "," + ab[1] + "," + ab[2] + "," + ab[3] + "," + DirPath + fileName;
            new BCW.farm.BLL.NC_shop().update_shop("picture='" + hebing + "'", "num_id=" + id + "");
            Utils.Success("修改图片", "修改图片成功.", Utils.getUrl("farm.aspx?act=edit_zw_picture&amp;id=" + id + ""), "1");
        }
        else
        {
            if (we.picture == "")
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("该作物图片为空,作物为：[" + we.name + "]<br/><a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;info=ok2&amp;id=" + id + "") + "\">马上增加图片</a>.");
                builder.Append(Out.Tab("</div>", "  "));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("你正在修改[" + we.name + "]的图片:<br/>");
                for (int i = 0; i < ab.Length; i++)
                {
                    try
                    {
                        builder.Append("<img  src=\"" + ab[i] + "\" alt=\"load\"/>");//height=\"55px\"
                    }
                    catch
                    {
                        builder.Append("图片出错.<a href=\"" + Utils.getPage("farm.aspx?act=edit_zw_picture&amp;info=ok&amp;ww=" + i + "&amp;id=" + id + "") + "\">马上修改</a>");
                    }
                    builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=edit_zw_picture&amp;info=ok&amp;ww=" + i + "&amp;id=" + id + "") + "\">修改</a><br/>");
                }
                builder.Append(Out.Tab("</div>", "  "));
            }
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:<br/>1.第一张图片指小叶子,第二张指大叶子,第三张指初熟,第四张指成熟,第五张指商店图鉴.<br/>2.请采用彩版上传或修改图片.");
            builder.Append(Out.Tab("</div>", ""));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=zuowuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //道具管理
    private void daojuguanliPage()
    {
        Master.Title = "" + GameName + "_道具管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;道具管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "xiao", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));//
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int xiao = Utils.ParseInt(Utils.GetRequest("xiao", "all", 1, @"^[1-9]\d*$", "0"));

        IList<BCW.farm.Model.NC_daoju> listFarm = new BCW.farm.BLL.NC_daoju().GetNC_daojus(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_daoju n in listFarm)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getPage("farm.aspx?act=edit_dj_mess&amp;id=" + n.ID + "") + "\">" + n.name + "</a>.(价钱" + n.price + ")");
                builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=edit_dj_picture&amp;id=" + n.ID + "") + "\">[修改图片]</a>");
                if (xiao == 1)
                {
                    if (!new BCW.farm.BLL.NC_baoxiang().Exists_bxzzdj(n.ID, 2))
                    {
                        if ((n.ID != 3) && (n.ID != 5) && (n.ID != 7) && (n.ID != 16) && (n.ID != 18) && (n.ID != 20))
                        {
                            builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=bx_add_zz&amp;type=2&amp;id=" + n.ID + "") + "\"><h style=\"color:red\">[添加]</h></a>");//红色
                        }
                    }
                    else
                        builder.Append(".[已添加]");
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

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daoju_add") + "\">道具添加</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daoju_use") + "\">道具使用情况查看</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (xiao == 1)
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=baoxiang") + "\">返回上级</a>");
        else
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //道具使用情况
    private void daoju_usePage()
    {
        Master.Title = "" + GameName + "道具管理_道具使用情况";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli") + "\">道具管理</a>&gt;道具使用情况");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        if (uid > 0)
            strWhere = "UsID=" + uid + "";
        else
            strWhere = "";

        IList<BCW.farm.Model.NC_daoju_use> listFarm = new BCW.farm.BLL.NC_daoju_use().GetNC_daoju_uses(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_daoju_use n in listFarm)
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
                string mename = new BCW.BLL.User().GetUsName(n.usid);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + n.usid + ")</a>：");
                BCW.farm.Model.NC_daoju q = new BCW.farm.BLL.NC_daoju().GetNC_daoju(n.daoju_id);
                if (n.tudi > 0)
                    builder.Append("在农场第" + n.tudi + "块土地使用了" + q.name + "");
                else
                    builder.Append("使用了" + q.name + "");
                builder.Append(".[" + n.updatetime + "]");
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

        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=daoju_use,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=daojuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //道具增加
    private void daoju_addPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("温馨提示", "使用彩版，才能够上传图片，页面更直观，更快捷！正在切换进入彩版添加图片...", "farm.aspx?act=daoju_add&amp;info=" + info + "&amp;ve=2a&amp;u=" + Utils.getstrU(), "1");
        }

        Master.Title = "商店.添加道具";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli") + "\">作物管理</a>&gt;添加道具");
        builder.Append(Out.Tab("</div>", ""));

        if (info == "ok")
        {
            string n = Utils.GetRequest("name", "post", 2, @"^[^\^]{1,10}$", "道具名称限1-10字内");//道具名称
            string name = n.Replace(" ", "");
            int price = Utils.ParseInt(Utils.GetRequest("price", "post", 2, @"^[1-9]\d*$", "道具价格填写出错"));//道具价格:(酷币)
            string m = Utils.GetRequest("note", "post", 2, @"^[^\^]{1,100}$", "道具说明限1-100字内");//道具说明
            string note = m.Replace(" ", "");
            int time = Utils.ParseInt(Utils.GetRequest("time", "post", 2, @"^[0-9]\d*$", "减少施肥的时间填写出错"));//减少施肥的时间
            int type = Utils.ParseInt(Utils.GetRequest("type", "post", 2, @"^[0-9]\d*$", "道具类型填写出错"));//道具类型

            //判断该作物是否存在
            if (new BCW.farm.BLL.NC_daoju().Exists_djmc(name))
            {
                Utils.Error("该作物名称已存在.", "");
            }
            else
            {
                BCW.farm.Model.NC_daoju oo = new BCW.farm.Model.NC_daoju();
                oo.name = name;
                oo.note = note;
                oo.price = price;
                oo.time = time;
                oo.type = type;
                oo.picture = "";
                int id = new BCW.farm.BLL.NC_daoju().Add(oo);
                Utils.Success("添加道具", "添加道具信息成功,请上传图片.", Utils.getUrl("farm.aspx?act=daoju_add&amp;info=ok2&amp;id=" + id + ""), "1");
            }
        }
        else if (info == "ok2")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "道具ID选择出错"));//道具ID
            if (new BCW.farm.BLL.NC_daoju().Exists2(id))
            {
                BCW.farm.Model.NC_daoju up = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你正在为道具:【" + up.name + "】添加图片：");
                builder.Append(Out.Tab("</div>", ""));

                string ssUpType = string.Empty;
                string ssText = string.Empty;
                string ssName = string.Empty;
                string ssType = string.Empty;
                string ssValu = string.Empty;
                string ssEmpt = string.Empty;
                string ssIdea = string.Empty;
                string ssOthe = string.Empty;
                string strText = string.Empty;
                string strName = string.Empty;
                string strType = string.Empty;
                string strValu = string.Empty;
                string strEmpt = string.Empty;

                for (int i = 0; i < 1; i++)
                {
                    string y = ",";
                    ssText = ssText + y + "" + ssUpType + "请选择图片:/,";
                    ssName = ssName + y + "file" + (i + 1) + y;
                    ssType = ssType + y + "file" + y;
                    ssValu = ssValu + "''";
                    ssEmpt = ssEmpt + y + y;
                }
                string strUpgroup = string.Empty;
                strUpgroup = "" + strUpgroup;

                ssText = ssText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
                ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act,info,id";
                ssType = ssType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden";
                ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "'daoju_add'ok3'" + id + "";
                ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
                ssIdea = "/";
                ssOthe = "确定添加,farm.aspx,post,2,red";
                builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
            }
            else
                Utils.Error("该道具ID不存在.", "");
        }
        else if (info == "ok3")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "道具ID选择出错"));//道具ID
            if (new BCW.farm.BLL.NC_daoju().Exists2(id))
            {
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                string DirPath = string.Empty;
                string fileName = string.Empty;
                string fileExtension = string.Empty;
                string hebing = string.Empty;
                string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
                int UpLength = 2000;
                //遍历File表单元素
                try
                {
                    //先判断是否符合要求,因为下面提交后就添加到文件夹里
                    for (int iFile = 0; iFile < 1; iFile++)
                    {
                        //检查文件扩展名字
                        System.Web.HttpPostedFile postedFile = files[iFile];
                        fileName = System.IO.Path.GetFileName(postedFile.FileName);
                        //检查是否允许上传格式
                        if (UpExt.IndexOf(fileExtension) == -1)
                        {
                            Utils.Error("图片类型选择错误！", "");
                        }
                        //非法上传
                        if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                        {
                            Utils.Error("非法上传,请重新选择.", "");
                        }
                        if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))//3M
                        {
                            Utils.Error("图片大小已超过2Mb.", "");
                        }
                    }
                }
                catch
                {
                    //Utils.Error("上传图片出错,001.", "");
                }

                for (int iFile = 0; iFile < 1; iFile++)
                {
                    //检查文件扩展名字
                    System.Web.HttpPostedFile postedFile = files[iFile];
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    if (fileName != "")
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                        string Path = "/bbs/game/img/farm/daoju/";
                        if (FileTool.CreateDirectory(Path, out DirPath))
                        {
                            //生成随机文件名
                            fileName = DT.getDateTimeNum() + iFile + fileExtension;
                            string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                            postedFile.SaveAs(SavePath);
                            //=============================图片木马检测,包括TXT===========================
                            string vSavePath = SavePath;
                            if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                            {
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
                                    Utils.Error("选择图片文件出错.", "");
                            }
                        }
                        hebing = DirPath + fileName;
                    }
                    else
                        Utils.Error("请选择图片文件.", "");
                }
                //把路径添加到字段
                new BCW.farm.BLL.NC_daoju().update_daoju("picture='" + hebing + "'", "id=" + id + "");
                Utils.Success("添加图片", "添加图片成功.", Utils.getUrl("farm.aspx?act=daojuguanli"), "1");
            }
            Utils.Error("该道具ID不存在.", "");
        }
        else
        {
            string type = (Utils.GetRequest("type", "all", 1, @"^[1-9]\d*$", ""));
            string strText = "道具名称:/,道具价格:(" + ub.Get("SiteBz") + ")/,道具说明:/,减少施肥的时间:/,道具类型:/,,,";
            string strName = "name,price,note,time,type,act,info";
            string strType = "text,num,big,num,num,hidden,hidden";
            string strValu = "'''" + 0 + "'" + type + "'daoju_add'ok";
            string strEmpt = "false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定添加,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("道具类型说明:<br/>1.0为可施肥1次,1为可施肥多次,若不是化肥建议从2开始.<br/>2.若不是化肥,施肥时间为0.<br/>3.宝箱道具请在类型输入10.");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //道具信息修改
    private void edit_dj_messPage()
    {
        Master.Title = "" + GameName + "_道具信息修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=daojuguanli") + "\">道具管理</a>&gt;道具信息修改");
        builder.Append(Out.Tab("</div>", ""));

        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择道具ID出错"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string n = Utils.GetRequest("name", "all", 2, @"^[^\^]{1,10}$", "道具名称限1-10字内");//道具名称
            string name = n.Replace(" ", "");
            int price = Utils.ParseInt(Utils.GetRequest("price", "all", 2, @"^[1-9]\d*$", "道具价格填写出错"));//道具价格:(酷币)
            string m = Utils.GetRequest("note", "post", 2, @"^[^\^]{1,100}$", "道具说明限1-100字内");//道具说明
            string note = m.Replace(" ", "");
            int time = Utils.ParseInt(Utils.GetRequest("time", "all", 2, @"^[0-9]\d*$", "减少施肥的时间填写出错"));//减少施肥的时间
            int type = Utils.ParseInt(Utils.GetRequest("type", "all", 2, @"^[0-9]\d*$", "道具类型填写出错"));//道具类型

            new BCW.farm.BLL.NC_daoju().update_daoju("name='" + name + "',note='" + note + "',price='" + price + "',time=" + time + ",type=" + type + "", "id=" + id + "");
            Utils.Success("修改道具", "修改道具信息成功.", Utils.getUrl("farm.aspx?act=edit_dj_mess&amp;id=" + id + ""), "1");
        }
        else
        {
            BCW.farm.Model.NC_daoju ui = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
            string strText = "道具名称:/,道具价格:(" + ub.Get("SiteBz") + ")/,道具说明:/,减少施肥的时间:/,道具类型:/,,,,";
            string strName = "name,price,note,time,type,act,id,info";
            string strType = "text,num,big,num,num,hidden,hidden,hidden";
            string strValu = "" + ui.name + "'" + ui.price + "'" + ui.note + "'" + ui.time + "'" + ui.type + "'edit_dj_mess'" + id + "'ok";
            string strEmpt = "false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("道具类型说明:<br/>1.0为可施肥1次,1为可施肥多次,若不是化肥建议从2开始.<br/>2.若不是化肥,施肥时间为0.");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=daojuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //道具图片修改
    private void edit_dj_picturePage()
    {
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择道具ID出错"));
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("温馨提示", "上传或修改图片请使用彩版！正在切换进入...", "farm.aspx?act=edit_dj_picture&amp;id=" + id + "&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        Master.Title = "" + GameName + "_道具图片修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=daojuguanli") + "\">道具管理</a>&gt;道具图片修改");
        builder.Append(Out.Tab("</div>", ""));

        BCW.farm.Model.NC_daoju we = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string ssUpType = string.Empty;
            string ssText = string.Empty;
            string ssName = string.Empty;
            string ssType = string.Empty;
            string ssValu = string.Empty;
            string ssEmpt = string.Empty;
            string ssIdea = string.Empty;
            string ssOthe = string.Empty;
            string strText = string.Empty;
            string strName = string.Empty;
            string strType = string.Empty;
            string strValu = string.Empty;
            string strEmpt = string.Empty;

            for (int i = 0; i < 1; i++)
            {
                string y = ",";
                ssText = ssText + y + "" + ssUpType + "正在修改[" + we.name + "]的图片，请选择:/,";
                ssName = ssName + y + "file" + (i + 1) + y;
                ssType = ssType + y + "file" + y;
                ssValu = ssValu + "''";
                ssEmpt = ssEmpt + y + y;
            }
            string strUpgroup = string.Empty;
            strUpgroup = "" + strUpgroup;

            ssText = ssText + Utils.Mid(strText, 1, strText.Length) + ",,,,";
            ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act,info,id";
            ssType = ssType + Utils.Mid(strType, 1, strType.Length) + ",hidden,hidden,hidden";
            ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "'edit_dj_picture'ok3'" + id + "";
            ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,,,";
            ssIdea = "";
            ssOthe = "确定修改,farm.aspx,post,2,red";
            builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
        }
        else if (info == "ok3")
        {
            System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
            string DirPath = string.Empty;
            string fileName = string.Empty;
            string fileExtension = string.Empty;
            string hebing = string.Empty;
            string UpExt = ".gif,.jpg,.jpeg,.png,.bmp";
            int UpLength = 2000;
            try
            {
                //先判断是否符合要求,因为下面提交后就添加到文件夹里
                for (int iFile = 0; iFile < 1; iFile++)
                {
                    //检查文件扩展名字
                    System.Web.HttpPostedFile postedFile = files[iFile];
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    //检查是否允许上传格式
                    if (UpExt.IndexOf(fileExtension) == -1)
                    {
                        Utils.Error("图片类型选择错误！", "");
                    }
                    //非法上传
                    if (fileExtension == ".asp" || fileExtension == ".aspx" || fileExtension == ".jsp" || fileExtension == ".php" || fileExtension == ".asa" || fileExtension == ".cer" || fileExtension == ".cdx" || fileExtension == ".htr" || fileExtension == ".exe")
                    {
                        Utils.Error("非法上传,请重新选择.", "");
                    }
                    if (postedFile.ContentLength > Convert.ToInt32(UpLength * 1024))//3M
                    {
                        Utils.Error("图片大小已超过2Mb.", "");
                    }
                    if (fileName == "")
                    {
                        Utils.Error("图片不能为空.", "");
                    }
                }
            }
            catch
            {

            }
            for (int iFile = 0; iFile < 1; iFile++)
            {
                //检查文件扩展名字
                System.Web.HttpPostedFile postedFile = files[iFile];
                fileName = System.IO.Path.GetFileName(postedFile.FileName);//得到最原始的图片名字
                if (fileName != "")
                {
                    fileExtension = System.IO.Path.GetExtension(fileName).ToLower();//得到图片的格式
                    string Path = "/bbs/game/img/farm/daoju/";
                    if (FileTool.CreateDirectory(Path, out DirPath))
                    {
                        //生成随机文件名
                        fileName = DT.getDateTimeNum() + iFile + fileExtension;
                        string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                        //删除原文件
                        try
                        {
                            string SavePath2 = System.Web.HttpContext.Current.Request.MapPath(we.picture);
                            System.IO.File.Delete(SavePath2);
                        }
                        catch
                        {
                        }
                        //保存新文件
                        postedFile.SaveAs(SavePath);
                        //=============================图片木马检测,包括TXT===========================
                        string vSavePath = SavePath;
                        if (fileExtension == ".txt" || fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == "png" || fileExtension == ".bmp")
                        {
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
                                Utils.Error("选择图片文件出错.", "");
                        }
                    }
                }
                else
                    Utils.Error("请选择图片文件.", "");
            }
            new BCW.farm.BLL.NC_daoju().update_daoju("picture='" + DirPath + fileName + "'", "id=" + id + "");
            Utils.Success("修改图片", "修改图片成功.", Utils.getUrl("farm.aspx?act=edit_dj_picture&amp;id=" + id + ""), "1");
        }
        else
        {
            if (we.picture == "")
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("该道具图片为空,道具为：[" + we.name + "]<br/><a href=\"" + Utils.getUrl("farm.aspx?act=daoju_add&amp;info=ok2&amp;id=" + id + "") + "\">马上增加图片</a>.");
                builder.Append(Out.Tab("</div>", "  "));
            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("你正在修改[" + we.name + "]的图片:<br/>");
                try
                {
                    builder.Append("<img src=\"" + we.picture + "\" alt=\"load\"/>");// height=\"55px\"
                }
                catch
                {
                    builder.Append("图片出错.<a href=\"" + Utils.getPage("farm.aspx?act=edit_dj_picture&amp;info=ok&amp;id=" + id + "") + "\">马上修改</a>");
                }
                builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=edit_dj_picture&amp;info=ok&amp;id=" + id + "") + "\">修改</a>");
                builder.Append(Out.Tab("</div>", "  "));
            }
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=daojuguanli") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //重置游戏
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        string info = Utils.GetRequest("info", "all", 1, "", "");
        string table = Utils.GetRequest("table", "all", 1, "", "");
        if (info == "1")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_baoxiang");//宝箱
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_daoju_use");//道具使用记录
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_GetCrop");//偷菜/果实收获表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_Goldlog");//消费记录
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_market");//市场
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_messagelog");//信息记录
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_mydaoju");//我的道具
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_slave");//奴隶表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_tudi");//土地
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_user");//用户表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_win");//宝箱获奖表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_gonggao");//公告表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_tasklist");//任务列表
            new BCW.farm.BLL.NC_baoxiang().ClearTable("tb_NC_hecheng");//合成表
            new BCW.BLL.Mebook().Delete_farm(1001);//删除留言板
            SqlHelper.ExecuteSql("DELETE  FROM tb_Goldlog WHERE AcText LIKE '%您在农场%'");//删除盈利分析
            SqlHelper.ExecuteSql("DELETE  FROM tb_Shopuser WHERE PIC=1");//删除赠送
            SqlHelper.ExecuteSql("DELETE  FROM tb_Shopsend WHERE PIC=1");//删除赠送
            Utils.Success("重置游戏", "重置所以数据成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "2")
        {
            Master.Title = "重置宝箱表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置宝箱表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=22&amp;table=tb_NC_baoxiang") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "22")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//宝箱
            Utils.Success("重置游戏", "重置宝箱数据成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "3")
        {
            Master.Title = "重置道具使用记录表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置道具使用记录表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=33&amp;table=tb_NC_daoju_use") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "33")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//道具使用记录
            Utils.Success("重置游戏", "重置道具使用记录成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "4")
        {
            Master.Title = "重置偷菜/果实收获表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置偷菜/果实收获表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=44&amp;table=tb_NC_GetCrop") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "44")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//偷菜/果实收获表
            Utils.Success("重置游戏", "重置偷菜/果实收获表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "5")
        {
            Master.Title = "重置消费记录表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置消费记录表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=55&amp;table=tb_NC_Goldlog") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "55")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//消费记录
            Utils.Success("重置游戏", "重置消费记录成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "6")
        {
            Master.Title = "重置市场表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置市场表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=66&amp;table=tb_NC_market") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "66")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//市场
            Utils.Success("重置游戏", "重置市场表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "7")
        {
            Master.Title = "重置信息记录表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置信息记录表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=77&amp;table=tb_NC_messagelog") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "77")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//信息记录
            Utils.Success("重置游戏", "重置信息记录成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "8")
        {
            Master.Title = "重置我的道具表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置我的道具表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=88&amp;table=tb_NC_mydaoju") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "88")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//我的道具
            Utils.Success("重置游戏", "重置我的道具成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "9")
        {
            Master.Title = "重置奴隶表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置奴隶表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=99&amp;table=tb_NC_slave") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "99")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//奴隶表
            Utils.Success("重置游戏", "重置奴隶表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "10")
        {
            Master.Title = "重置土地表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置土地表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1010&amp;table=tb_NC_tudi") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1010")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//土地
            Utils.Success("重置游戏", "重置土地成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "11")
        {
            Master.Title = "重置用户表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置用户表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1111&amp;table=tb_NC_user") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1111")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//用户表
            Utils.Success("重置游戏", "重置用户表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "12")
        {
            Master.Title = "重置宝箱获奖表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置宝箱获奖表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1212&amp;table=tb_NC_win") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1212")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//宝箱获奖表
            Utils.Success("重置游戏", "重置宝箱获奖表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "13")
        {
            Master.Title = "重置公告表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置公告表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1313&amp;table=tb_NC_gonggao") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1313")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//公告表
            Utils.Success("重置游戏", "重置公告表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "14")
        {
            Master.Title = "重置任务列表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置任务列表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1414&amp;table=tb_NC_tasklist") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1414")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//任务列表
            Utils.Success("重置游戏", "重置任务列表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "15")
        {
            Master.Title = "删除农场所有留言吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除农场所有留言吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1515&amp;table=tb_Mebook") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1515")
        {
            new BCW.BLL.Mebook().Delete_farm(1001);
            Utils.Success("重置游戏", "删除农场所有留言成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else if (info == "16")
        {
            Master.Title = "重置合成表吗？";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定重置合成表吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset&amp;info=1616&amp;table=tb_NC_hecheng") + "\">确定重置</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=reset") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        else if (info == "1616")
        {
            new BCW.farm.BLL.NC_baoxiang().ClearTable(table);//合成表
            Utils.Success("重置游戏", "重置合成表成功..", Utils.getUrl("farm.aspx?act=reset"), "2");
        }
        else
        {
            Master.Title = "" + GameName + "_重置游戏";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;重置游戏");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("1.<a href=\"" + Utils.getUrl("farm.aspx?info=1&amp;act=reset") + "\">[<b>一键全部重置</b>]</a><br/>");
            builder.Append("2.<a href=\"" + Utils.getUrl("farm.aspx?info=2&amp;act=reset") + "\">[重置:宝箱表]</a><br/>");
            builder.Append("3.<a href=\"" + Utils.getUrl("farm.aspx?info=3&amp;act=reset") + "\">[重置:道具使用记录表]</a><br/>");
            builder.Append("4.<a href=\"" + Utils.getUrl("farm.aspx?info=4&amp;act=reset") + "\">[重置:偷菜/果实收获表]</a><br/>");
            builder.Append("5.<a href=\"" + Utils.getUrl("farm.aspx?info=5&amp;act=reset") + "\">[重置:消费记录表]</a><br/>");
            builder.Append("6.<a href=\"" + Utils.getUrl("farm.aspx?info=6&amp;act=reset") + "\">[重置:市场摆摊表]</a><br/>");
            builder.Append("7.<a href=\"" + Utils.getUrl("farm.aspx?info=7&amp;act=reset") + "\">[重置:信息记录表]</a><br/>");
            builder.Append("8.<a href=\"" + Utils.getUrl("farm.aspx?info=8&amp;act=reset") + "\">[重置:我的道具表]</a><br/>");
            builder.Append("9.<a href=\"" + Utils.getUrl("farm.aspx?info=9&amp;act=reset") + "\">[重置:奴隶表]</a><br/>");
            builder.Append("10.<a href=\"" + Utils.getUrl("farm.aspx?info=10&amp;act=reset") + "\">[重置:土地表]</a><br/>");
            builder.Append("11.<a href=\"" + Utils.getUrl("farm.aspx?info=11&amp;act=reset") + "\">[重置:用户表]</a><br/>");
            builder.Append("12.<a href=\"" + Utils.getUrl("farm.aspx?info=12&amp;act=reset") + "\">[重置:宝箱获奖表]</a><br/>");
            builder.Append("13.<a href=\"" + Utils.getUrl("farm.aspx?info=13&amp;act=reset") + "\">[重置:公告表]</a><br/>");
            builder.Append("14.<a href=\"" + Utils.getUrl("farm.aspx?info=14&amp;act=reset") + "\">[重置:任务列表]</a><br/>");
            builder.Append("15.<a href=\"" + Utils.getUrl("farm.aspx?info=15&amp;act=reset") + "\">[删除农场所有留言]</a><br/>");
            builder.Append("16.<a href=\"" + Utils.getUrl("farm.aspx?info=16&amp;act=reset") + "\">[重置:合成表]</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<h style=\"color:red\">注意：重置后，数据无法恢复。</h><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //系统配置
    private void PeizhiPage()
    {
        Master.Title = "" + GameName + "_游戏配置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string xmlPath = "/Controls/farm.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string FarmName = Utils.GetRequest("FarmName", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
                string farmtop = Utils.GetRequest("farmtop", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string farm_logo = Utils.GetRequest("farm_logo", "post", 3, @"^[^\^]{1,200}$", "农场Logo地址限200字内");
                string farm_qd = Utils.GetRequest("farm_qd", "post", 3, @"^[^\^]{1,200}$", "签到Logo地址限200字内");
                string farm_bx = Utils.GetRequest("farm_bx", "post", 3, @"^[^\^]{1,200}$", "宝箱Logo地址限200字内");
                string bxys_num = Utils.GetRequest("bxys_num", "post", 2, @"^[1-9]\d*$", "宝箱需要的钥匙数量填写出错");
                string SiteListNo = Utils.GetRequest("SiteListNo", "post", 2, @"^[0-9]\d*$", "分页数填写出错");
                string Expir = Utils.GetRequest("xExpir", "post", 2, @"^[0-9]\d*$", "防刷秒数填写出错");
                string Foot = Utils.GetRequest("farmFoot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
                string vipprice = Utils.GetRequest("vipprice", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "vip打折填写出错");
                string slave_day = Utils.GetRequest("slave_day", "post", 2, @"^[1-9]\d*$", "奴隶存放天数填写出错");
                string slave_num = Utils.GetRequest("slave_num", "post", 2, @"^[0-9]\d*$", "奴隶惩罚和安抚次数填写出错");
                string slave_jinbi_he = Utils.GetRequest("slave_jinbi_he", "post", 2, @"^[0-9]\d*$", "安抚奴隶金币填写出错");
                string slave_jinbi_me = Utils.GetRequest("slave_jinbi_me", "post", 2, @"^[0-9]\d*$", "惩罚奴隶金币填写出错");
                string slave_jingyan_me = Utils.GetRequest("slave_jingyan_me", "post", 2, @"^[0-9]\d*$", "惩罚奴隶经验填写出错");
                string slave_jingyan_he = Utils.GetRequest("slave_jingyan_he", "post", 2, @"^[0-9]\d*$", "安抚奴隶经验填写出错");
                string xianjing_num = Utils.GetRequest("xianjing_num", "post", 2, @"^[1-9]\d*$", "陷阱个数填写出错");
                string xianjing_dengji = Utils.GetRequest("xianjing_dengji", "post", 2, @"^[0-9]\d*$", "陷阱所需等级填写出错");
                string xianjing_jinbi = Utils.GetRequest("xianjing_jinbi", "post", 2, @"^[0-9]\d*$", "陷阱所需金币填写出错");
                string xianjing_jilv = Utils.GetRequest("xianjing_jilv", "post", 2, @"^[1-9]\d*$", "陷阱机率填写出错");
                string xianjing_jb = Utils.GetRequest("xianjing_jb", "post", 2, @"^[0-9]\d*$", "陷阱中获得与失去金币填写出错");
                string bigbuynum = Utils.GetRequest("bigbuynum", "post", 2, @"^[0-9]\d*$", "商店一次最多可购买数量填写出错");
                string baitan_bignum = Utils.GetRequest("baitan_bignum", "post", 2, @"^[1-9]\d*$", "最大摆摊数量填写出错");
                string baitan_day = Utils.GetRequest("baitan_day", "post", 2, @"^[1-9]\d*$", "摆摊天数填写出错");
                string baitan_koushui = Utils.GetRequest("baitan_koushui", "post", 2, @"^(\d)*(\.(\d){1,2})?$", "摆摊扣税填写出错");
                string tou_renshu = Utils.GetRequest("tou_renshu", "post", 2, @"^[0-9]\d*$", "每阶段允许偷取作物的人数填写出错");
                string Tar = Utils.GetRequest("Tar", "post", 2, @"^[1-9]\d*$", "" + ub.Get("SiteBz") + "兑换数填写出错");
                string Tar2 = Utils.GetRequest("Tar2", "post", 2, @"^[1-9]\d*$", "金币兑换数填写出错");
                string z_shui_numtime = Utils.GetRequest("z_shui_numtime", "post", 2, @"^[1-9]\d*$", "缺水减产时间填写出错");
                string z_shui_num = Utils.GetRequest("z_shui_num", "post", 2, @"^[1-9]\d*$", "缺水减产最大数量填写出错");
                string hecheng_num = Utils.GetRequest("hecheng_num", "post", 2, @"^[1-9]\d*$", "合成数量填写出错");
                string xExpir_huafei = Utils.GetRequest("xExpir_huafei", "post", 2, @"^[0-9]\d*$", "化肥防刷次数填写出错");
                string zs_num = Utils.GetRequest("zs_num", "post", 2, @"^[1-9]\d*$", "可赠送个数填写出错");
                string xianjing_day = Utils.GetRequest("xianjing_day", "post", 2, @"^[0-9]\d*$", "陷阱钥匙使用天数填写出错");
                string big_zcishu = Utils.GetRequest("big_zcishu", "post", 2, @"^[1-9]\d*$", "最大种草放虫数量填写出错");
                string big_ccishu = Utils.GetRequest("big_ccishu", "post", 2, @"^[1-9]\d*$", "最大自己除草除虫数量填写出错");

                xml.dss["FarmName"] = FarmName;
                xml.dss["farmtop"] = farmtop;
                xml.dss["farm_logo"] = farm_logo;
                xml.dss["bxys_num"] = bxys_num;
                xml.dss["farm_qd"] = farm_qd;
                xml.dss["farm_bx"] = farm_bx;
                xml.dss["SiteListNo"] = SiteListNo;
                xml.dss["xExpir"] = Expir;
                xml.dss["farmFoot"] = Foot;
                xml.dss["vipprice"] = vipprice;
                xml.dss["slave_day"] = slave_day;
                xml.dss["slave_num"] = slave_num;
                xml.dss["slave_jinbi_he"] = slave_jinbi_he;
                xml.dss["slave_jinbi_me"] = slave_jinbi_me;
                xml.dss["slave_jingyan_me"] = slave_jingyan_me;
                xml.dss["slave_jingyan_he"] = slave_jingyan_he;//zs_num
                xml.dss["xianjing_num"] = xianjing_num;
                xml.dss["xianjing_dengji"] = xianjing_dengji;
                xml.dss["xianjing_jinbi"] = xianjing_jinbi;
                xml.dss["xianjing_jb"] = xianjing_jb;
                xml.dss["xianjing_jilv"] = xianjing_jilv;
                xml.dss["bigbuynum"] = bigbuynum;
                xml.dss["baitan_day"] = baitan_day;
                xml.dss["baitan_bignum"] = baitan_bignum;
                xml.dss["baitan_koushui"] = baitan_koushui;
                xml.dss["tou_renshu"] = tou_renshu;
                xml.dss["Tar"] = Tar;
                xml.dss["Tar2"] = Tar2;
                xml.dss["z_shui_numtime"] = z_shui_numtime;
                xml.dss["z_shui_num"] = z_shui_num;
                xml.dss["hecheng_num"] = hecheng_num;
                xml.dss["xExpir_huafei"] = xExpir_huafei;
                xml.dss["xianjing_day"] = xianjing_day;
                xml.dss["big_zcishu"] = big_zcishu;
                xml.dss["big_ccishu"] = big_ccishu;
                xml.dss["zs_num"] = zs_num;
            }
            else
            {
                string shouhuo1_grade = Utils.GetRequest("shouhuo1_grade", "post", 2, @"^[0-9]\d*$", "一键收获所需等级填写错误");//
                string shouhuo1_jinbi = Utils.GetRequest("shouhuo1_jinbi", "post", 2, @"^[0-9]\d*$", "一键收获所需金币填写错误");
                string chandi1_grade = Utils.GetRequest("chandi1_grade", "post", 2, @"^[0-9]\d*$", "一键铲地所需等级填写错误");
                string chandi1_jinbi = Utils.GetRequest("chandi1_jinbi", "post", 2, @"^[0-9]\d*$", "一键铲地所需金币填写错误");
                string shifei1_grade = Utils.GetRequest("shifei1_grade", "post", 2, @"^[0-9]\d*$", "一键施肥所需等级填写错误");
                string shifei1_jinbi = Utils.GetRequest("shifei1_jinbi", "post", 2, @"^[0-9]\d*$", "一键施肥所需金币填写错误");
                string chucao1_grade = Utils.GetRequest("chucao1_grade", "post", 2, @"^[0-9]\d*$", "一键除草所需等级填写错误");
                string chucao1_jinbi = Utils.GetRequest("chucao1_jinbi", "post", 2, @"^[0-9]\d*$", "一键除草所需金币填写错误");
                string chuchong1_grade = Utils.GetRequest("chuchong1_grade", "post", 2, @"^[0-9]\d*$", "一键除虫所需等级填写错误");
                string chuchong1_jinbi = Utils.GetRequest("chuchong1_jinbi", "post", 2, @"^[0-9]\d*$", "一键除虫所需金币填写错误");
                string jiaoshui1_grade = Utils.GetRequest("jiaoshui1_grade", "post", 2, @"^[0-9]\d*$", "一键浇水所需等级填写错误");
                string jiaoshui1_jinbi = Utils.GetRequest("jiaoshui1_jinbi", "post", 2, @"^[0-9]\d*$", "一键浇水所需金币填写错误");
                string bz_big_jingyan = Utils.GetRequest("bz_big_jingyan", "post", 2, @"^[0-9]\d*$", "种植最大经验填写错误");
                string zj_big_jingyan = Utils.GetRequest("zj_big_jingyan", "post", 2, @"^[0-9]\d*$", "自己操作最大经验填写错误");
                string sh_big_jingyan = Utils.GetRequest("sh_big_jingyan", "post", 2, @"^[0-9]\d*$", "种草放虫最大经验填写错误");
                string bm_big_jingyan = Utils.GetRequest("bm_big_jingyan", "post", 2, @"^[0-9]\d*$", "帮忙好友最大经验填写错误");
                string all_jinbi = Utils.GetRequest("all_jinbi", "post", 2, @"^[0-9]\d*$", "所有动作获得的金币填写错误");
                string all_jingyan = Utils.GetRequest("all_jingyan", "post", 2, @"^[0-9]\d*$", "所有动作获得的经验填写错误");
                string zcaofchong_jingyan = Utils.GetRequest("zcaofchong_jingyan", "post", 2, @"^[0-9]\d*$", "种草放虫经验填写错误");
                string zcaofchong_jinbi = Utils.GetRequest("zcaofchong_jinbi", "post", 2, @"^[0-9]\d*$", "种草放虫金币填写错误");
                string zhong_jingyan = Utils.GetRequest("zhong_jingyan", "post", 2, @"^[0-9]\d*$", "种植获得经验填写错误");
                string shifei_jingyan = Utils.GetRequest("shifei_jingyan", "post", 2, @"^[0-9]\d*$", "施肥获得经验填写错误");
                string chandi_jingyan = Utils.GetRequest("chandi_jingyan", "post", 2, @"^[0-9]\d*$", "铲地获得经验填写错误");
                string qd_suiji = Utils.GetRequest("qd_suiji", "post", 2, @"^[1-9]\d*$", "签到随机数填写错误");
                string qd_jishu = Utils.GetRequest("qd_jishu", "post", 2, @"^[1-9]\d*$", "签到" + ub.Get("SiteBz") + "的基数填写错误");
                string qd_jishu_jinbi = Utils.GetRequest("qd_jishu_jinbi", "post", 2, @"^[1-9]\d*$", "签到金币的基数填写错误");
                string bx_suiji = Utils.GetRequest("bx_suiji", "post", 2, @"^[1-9]\d*$", "宝箱随机数填写错误");
                string bx_jishu = Utils.GetRequest("bx_jishu", "post", 2, @"^[1-9]\d*$", "宝箱基数填写错误");
                string bx_jishu_jinbi = Utils.GetRequest("bx_jishu_jinbi", "post", 2, @"^[1-9]\d*$", "宝箱金币的基数填写错误");
                string cdsh_suiji = Utils.GetRequest("cdsh_suiji", "post", 2, @"^[1-9]\d*$", "铲地/收获随机数填写错误");
                string cdsh1_suiji = Utils.GetRequest("cdsh1_suiji", "post", 2, @"^[1-9]\d*$", "一键铲地/一键收获随机数填写错误");
                string cdsh_gl = Utils.GetRequest("cdsh_gl", "post", 2, @"^[0-1]\d*$", "管理铲地收获开关填写错误");
                string set_ccs_miao = Utils.GetRequest("set_ccs_miao", "post", 2, @"^[1-9]\d*$", "随机种草放虫缺水的概率填写错误");//
                string yaoqing_jinbi = Utils.GetRequest("yaoqing_jinbi", "post", 2, @"^[1-9]\d*$", "邀请好友奖励金币数填写错误");

                xml.dss["shouhuo1_grade"] = shouhuo1_grade;
                xml.dss["shouhuo1_jinbi"] = shouhuo1_jinbi;
                xml.dss["chandi1_grade"] = chandi1_grade;
                xml.dss["chandi1_jinbi"] = chandi1_jinbi;
                xml.dss["shifei1_grade"] = shifei1_grade;
                xml.dss["shifei1_jinbi"] = shifei1_jinbi;
                xml.dss["chucao1_grade"] = chucao1_grade;
                xml.dss["chucao1_jinbi"] = chucao1_jinbi;
                xml.dss["chuchong1_grade"] = chuchong1_grade;
                xml.dss["chuchong1_jinbi"] = chuchong1_jinbi;
                xml.dss["jiaoshui1_grade"] = jiaoshui1_grade;
                xml.dss["jiaoshui1_jinbi"] = jiaoshui1_jinbi;
                xml.dss["bz_big_jingyan"] = bz_big_jingyan;
                xml.dss["sh_big_jingyan"] = sh_big_jingyan;
                xml.dss["bm_big_jingyan"] = bm_big_jingyan;
                xml.dss["all_jinbi"] = all_jinbi;
                xml.dss["all_jingyan"] = all_jingyan;
                xml.dss["zcaofchong_jingyan"] = zcaofchong_jingyan;
                xml.dss["zcaofchong_jinbi"] = zcaofchong_jinbi;
                xml.dss["zhong_jingyan"] = zhong_jingyan;
                xml.dss["shifei_jingyan"] = shifei_jingyan;
                xml.dss["chandi_jingyan"] = chandi_jingyan;
                xml.dss["qd_suiji"] = qd_suiji;
                xml.dss["qd_jishu"] = qd_jishu;
                xml.dss["bx_suiji"] = bx_suiji;
                xml.dss["bx_jishu"] = bx_jishu;
                xml.dss["qd_jishu_jinbi"] = qd_jishu_jinbi;
                xml.dss["bx_jishu_jinbi"] = bx_jishu_jinbi;
                xml.dss["cdsh_suiji"] = cdsh_suiji;
                xml.dss["cdsh1_suiji"] = cdsh1_suiji;
                xml.dss["cdsh_gl"] = cdsh_gl;
                xml.dss["set_ccs_miao"] = set_ccs_miao;//zj_big_jingyan
                xml.dss["yaoqing_jinbi"] = yaoqing_jinbi;
                xml.dss["zj_big_jingyan"] = zj_big_jingyan;
            }

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("farm.aspx?act=peizhi&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("农场基本设置|");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=peizhi&amp;ptype=1") + "\">经验和金币设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=peizhi&amp;ptype=0") + "\">农场基本设置</a>");
                builder.Append("|经验和金币设置");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "游戏名称：/," + ub.Get("SiteBz") + "基数：/,金币兑换数：(" + ub.Get("SiteBz") + "换金币：" + xml.dss["Tar"] + ":" + xml.dss["Tar2"] + ")/,商店VIP会员购买打折：/,化肥每天可使用次数：(0为不限制)/,缺水减产时间：(分钟)/,缺水减产最大数量：/,合成所需个数：/,宝箱所需的钥匙数量：/,奴隶使用天数：/,奴隶的惩罚和安抚次数：/,惩罚奴隶(自己)奖励金币：/,惩罚奴隶(自己)奖励经验：/,安抚奴隶奖励(对方)金币：/,安抚奴隶(对方)奖励经验：/,陷阱个数：/,陷阱钥匙的使用天数：/,设置陷阱所需等级：/,设置陷阱所需金币：/,踩中陷阱惩罚和得到的金币：/,陷阱被踩中的机率：(填1-10即可.自动1除)/,商店每次最多限购：/,在好友农场最大种草放虫数量：/,在自己农场最大除草除虫数量：/,最大摆摊数量：/,限摆摆摊天数：(为0则不限制)/,摆摊扣税：(为0不扣税)/,每阶段每块土地允许偷取作物的人数：(个)/,每个ID每天可赠送道具数量：/,游戏Logo路径：/,签到Logo路径：/,宝箱Logo路径：/,分页条数：/,下注防刷(秒)：/,头部Ubb：/,底部Ubb：/,";
                string strName = "FarmName,Tar,Tar2,vipprice,xExpir_huafei,z_shui_numtime,z_shui_num,hecheng_num,bxys_num,slave_day,slave_num,slave_jinbi_me,slave_jingyan_me,slave_jinbi_he,slave_jingyan_he,xianjing_num,xianjing_day,xianjing_dengji,xianjing_jinbi,xianjing_jb,xianjing_jilv,bigbuynum,big_ccishu,big_zcishu,baitan_bignum,baitan_day,baitan_koushui,tou_renshu,zs_num,farm_logo,farm_qd,farm_bx,SiteListNo,xExpir,farmtop,farmFoot,backurl";//baitan_day
                string strType = "text,num,num,text,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,text,num,num,text,text,text,num,num,textarea,textarea,hidden";
                string strValu = "" + xml.dss["FarmName"] + "'" + xml.dss["Tar"] + "'" + xml.dss["Tar2"] + "'" + xml.dss["vipprice"] + "'" + xml.dss["xExpir_huafei"] + "'" + xml.dss["z_shui_numtime"] + "'" + xml.dss["z_shui_num"] + "'" + xml.dss["hecheng_num"] + "'" + xml.dss["bxys_num"] + "'" + xml.dss["slave_day"] + "'" + xml.dss["slave_num"] + "'" + xml.dss["slave_jinbi_me"] + "'" + xml.dss["slave_jingyan_me"] + "'" + xml.dss["slave_jinbi_he"] + "'" + xml.dss["slave_jingyan_he"] + "'" + xml.dss["xianjing_num"] + "'" + xml.dss["xianjing_day"] + "'" + xml.dss["xianjing_dengji"] + "'" + xml.dss["xianjing_jinbi"] + "'" + xml.dss["xianjing_jb"] + "'" + xml.dss["xianjing_jilv"] + "'" + xml.dss["bigbuynum"] + "'" + xml.dss["big_ccishu"] + "'" + xml.dss["big_zcishu"] + "'" + xml.dss["baitan_bignum"] + "'" + xml.dss["baitan_day"] + "'" + xml.dss["baitan_koushui"] + "'" + xml.dss["tou_renshu"] + "'" + xml.dss["zs_num"] + "'" + xml.dss["farm_logo"] + "'" + xml.dss["farm_qd"] + "'" + xml.dss["farm_bx"] + "'" + xml.dss["SiteListNo"] + "'" + xml.dss["xExpir"] + "'" + xml.dss["farmtop"] + "'" + xml.dss["farmFoot"] + "'" + Utils.getPage(0) + "";//bigbuynum
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,farm.aspx?act=peizhi&amp;ptype=0,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />1.请采用2版本设置.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "邀请好友获得金币：/,一键收获所需等级：/,一键收获所需金币：/,一键铲地所需等级：/,一键铲地所需金币：/,一键施肥所需等级：/,一键施肥所需金币：/,一键除草所需等级：/,一键除草所需金币：/,一键除虫所需等级：/,一键除虫所需金币：/,一键浇水所需等级：/,一键浇水所需金币：/,种植每天获得最大经验：/,在自己农场操作每天获得最大经验：/,种草放虫每天获得最大经验：/,帮忙好友每天获得最大经验：/,除草除虫浇水获得的金币：(包括去好友农场帮忙)/,除草除虫浇水获得的经验：(包括去好友农场帮忙)/,种草放虫经验：/,种草放虫金币：/,种植作物获得的经验：/,施肥获得的经验：/,铲地获得的经验：/,签到随机数(1-x，x大于1)/,签到" + ub.Get("SiteBz") + "相乘奇数：/,签到金币相乘奇数：/,宝箱随机数(1-x，x大于1)/,宝箱经验相乘奇数：(等级*奇数=所得经验)/,宝箱金币相乘奇数：(随机数*奇数=所得金币)/,铲地和收获奖励开关：/,铲地和收获随机得到种子的概率(1-x，x大于2)/,一键铲地和一键收获随机得到种子的概率(1-x，x大于2)/,刷新机每块土地产生草虫水的概率(1-x，x大于2)/,";//
                string strName = "yaoqing_jinbi,shouhuo1_grade,shouhuo1_jinbi,chandi1_grade,chandi1_jinbi,shifei1_grade,shifei1_jinbi,chucao1_grade,chucao1_jinbi,chuchong1_grade,chuchong1_jinbi,jiaoshui1_grade,jiaoshui1_jinbi,bz_big_jingyan,zj_big_jingyan,sh_big_jingyan,bm_big_jingyan,all_jinbi,all_jingyan,zcaofchong_jingyan,zcaofchong_jinbi,zhong_jingyan,shifei_jingyan,chandi_jingyan,qd_suiji,qd_jishu,qd_jishu_jinbi,bx_suiji,bx_jishu,bx_jishu_jinbi,cdsh_gl,cdsh_suiji,cdsh1_suiji,set_ccs_miao,backurl";
                string strType = "num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,select,num,num,num,hidden";
                string strValu = "" + xml.dss["yaoqing_jinbi"] + "'" + xml.dss["shouhuo1_grade"] + "'" + xml.dss["shouhuo1_jinbi"] + "'" + xml.dss["chandi1_grade"] + "'" + xml.dss["chandi1_jinbi"] + "'" + xml.dss["shifei1_grade"] + "'" + xml.dss["shifei1_jinbi"] + "'" + xml.dss["chucao1_grade"] + "'" + xml.dss["chucao1_jinbi"] + "'" + xml.dss["chuchong1_grade"] + "'" + xml.dss["chuchong1_jinbi"] + "'" + xml.dss["jiaoshui1_grade"] + "'" + xml.dss["jiaoshui1_jinbi"] + "'" + xml.dss["bz_big_jingyan"] + "'" + xml.dss["zj_big_jingyan"] + "'" + xml.dss["sh_big_jingyan"] + "'" + xml.dss["bm_big_jingyan"] + "'" + xml.dss["all_jinbi"] + "'" + xml.dss["all_jingyan"] + "'" + xml.dss["zcaofchong_jingyan"] + "'" + xml.dss["zcaofchong_jinbi"] + "'" + xml.dss["zhong_jingyan"] + "'" + xml.dss["shifei_jingyan"] + "'" + xml.dss["chandi_jingyan"] + "'" + xml.dss["qd_suiji"] + "'" + xml.dss["qd_jishu"] + "'" + xml.dss["qd_jishu_jinbi"] + "'" + xml.dss["bx_suiji"] + "'" + xml.dss["bx_jishu"] + "'" + xml.dss["bx_jishu_jinbi"] + "'" + xml.dss["cdsh_gl"] + "'" + xml.dss["cdsh_suiji"] + "'" + xml.dss["cdsh1_suiji"] + "'" + xml.dss["set_ccs_miao"] + "'" + Utils.getPage(0) + "";//set_ccs_miao
                string strEmpt = "false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,0|开|1|关,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,farm.aspx?act=peizhi&amp;ptype=1,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:<br />1.所有等级和金币不能设置为0.<br/>2.<b>签到/铲地/收获</b>的随机数是从1开始,若填5则1-5随机取值.<br/>3.签到奇数是作为相乘的一个条件,随机数*奇数将得到奖励的" + ub.Get("SiteBz") + "或金币.<br/>4.请采用2版本设置.");
                builder.Append(Out.Tab("</div>", ""));
            }

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //土地管理
    private void tudi_glPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "1")
        {
            int tudi_id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择土地出错"));
            int uid_1 = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok&amp;act=tudi_gl&amp;id=" + tudi_id + "&amp;usid=" + uid_1 + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tudi_gl&amp;usid=" + uid_1 + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ac == "ok")
        {
            int tudi_id_ok = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择土地出错"));
            int uid_ok = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));

            new BCW.farm.BLL.NC_tudi().Delete(tudi_id_ok);
            Utils.Success("删除土地", "删除土地成功,正在返回.", Utils.getUrl("farm.aspx?act=tudi_gl&amp;usid=" + uid_ok + ""), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_土地管理";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;土地管理");
            builder.Append(Out.Tab("</div>", "<br/>"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string strOrder = "usid,tudi";
            string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            if (uid > 0)
            {
                strWhere = "UsID=" + uid + "";
            }
            else
                strWhere = "";


            IList<BCW.farm.Model.NC_tudi> listFarm = new BCW.farm.BLL.NC_tudi().GetNC_tudis(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_tudi n in listFarm)
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
                    //获取id对应的用户名
                    string mename = new BCW.BLL.User().GetUsName(n.usid);
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "") + "\">" + mename + "(" + n.usid + ")</a>" + OutType(n.tudi_type) + n.tudi + ":");
                    if (n.zuowu == "")
                        builder.Append("(空)");
                    else
                        builder.Append("[" + n.zuowu + "]经验:" + n.zuowu_experience + ",");

                    if (n.ischandi == 1)
                    {
                        builder.Append("(" + n.harvest + "/" + n.zuowu_ji + "季)");
                        if (DateTime.Now > n.updatetime.AddMinutes(n.zuowu_time))
                            builder.Append("已成熟,产量:" + n.output + ".");
                        else
                        {
                            builder.Append("距离成熟:" + DT.DateDiff(DateTime.Now, n.updatetime.AddMinutes(n.zuowu_time)) + ",");
                            if (n.iscao == 1)
                                builder.Append("有草,");
                            if (n.isinsect == 1)
                                builder.Append("有虫,");
                            if (n.iswater == 1)
                                builder.Append("缺水,");
                            if (n.isshifei == 1)
                                builder.Append("[已施肥]");
                            else
                                builder.Append("[未施肥]");
                        }
                        if (n.touID != "")
                            builder.Append("(被偷ID:" + n.touID + ")");
                        if (n.caoID != "")
                            builder.Append("(放草ID:" + n.caoID + ")");
                        if (n.chongID != "")
                            builder.Append("(放虫ID:" + n.chongID + ")");
                    }
                    else if (n.ischandi == 2)
                        builder.Append("[枯萎的作物]");

                    if (n.xianjing == 1)
                        builder.Append("[设有陷阱]");

                    builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=tudi_gl&amp;id=" + n.ID + "&amp;usid=" + uid + "&amp;ac=1") + "\">[删]</a>");
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

            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=tudi_gl,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //消息查看和消费查看
    private void messagePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;消息查看");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "_信息查看.草虫水";
            builder.Append("草虫水 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=1";
            else
                strWhere = "type=1";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=1&amp;usid=" + uid + "") + "\">草虫水</a> ");

        if (ptype == 2)
        {
            Master.Title = "" + GameName + "_信息查看.奴隶";
            builder.Append("奴隶 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and (type=2 or type=11) ";
            else
                strWhere = "(type=2 or type=11)";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=2&amp;usid=" + uid + "") + "\">奴隶</a> ");

        if (ptype == 3)
        {
            Master.Title = "" + GameName + "_信息查看.赠送";
            builder.Append("赠送 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=3";
            else
                strWhere = "type=3";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=3&amp;usid=" + uid + "") + "\">赠送</a> ");

        if (ptype == 4)
        {
            Master.Title = "" + GameName + "_信息查看.商店仓库";
            builder.Append("商店仓库 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=4";
            else
                strWhere = "type=4";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=4&amp;usid=" + uid + "") + "\">商店仓库</a> ");
        if (ptype == 5)
        {
            Master.Title = "" + GameName + "_信息查看.签到";
            builder.Append("签到 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=5";
            else
                strWhere = "type=5";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=5&amp;usid=" + uid + "") + "\">签到</a> ");
        if (ptype == 11)
        {
            Master.Title = "" + GameName + "_信息查看.送礼";
            builder.Append("送礼 <br/>");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and pic=1";
            else
                strWhere = "pic=1";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=11&amp;usid=" + uid + "") + "\">送礼</a> <br/>");

        if (ptype == 6)
        {
            Master.Title = "" + GameName + "_信息查看.宝箱";
            builder.Append("宝箱 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=6";
            else
                strWhere = "type=6";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=6&amp;usid=" + uid + "") + "\">宝箱</a> ");
        if (ptype == 7)
        {
            Master.Title = "" + GameName + "_信息查看.市场";
            builder.Append("市场 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=7";
            else
                strWhere = "type=7";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=7&amp;usid=" + uid + "") + "\">市场</a> ");
        if (ptype == 8)
        {
            Master.Title = "" + GameName + "_信息查看.收铲播施";
            builder.Append("收铲播施 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=8";
            else
                strWhere = "type=8";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=8&amp;usid=" + uid + "") + "\">收铲播施</a> ");
        if (ptype == 9)
        {
            Master.Title = "" + GameName + "_信息查看.偷放种";
            builder.Append("偷放种 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=9";
            else
                strWhere = "type=9";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=9&amp;usid=" + uid + "") + "\">偷放种</a> ");
        if (ptype == 10)
        {
            Master.Title = "" + GameName + "_信息查看.功能";
            builder.Append("功能 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and type=10";
            else
                strWhere = "type=10";
        }
        else
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=10&amp;usid=" + uid + "") + "\">功能</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        if (ptype == 11)
        {
            IList<BCW.Model.Shopsend> listFarm = new BCW.BLL.Shopsend().GetShopsends(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Shopsend n in listFarm)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + n.UsName + "</a>送给(<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.ToID + "") + "\">" + n.ToName + "</a>)" + n.Total + "个" + n.Title + ".[" + DT.FormatDate(n.AddTime, 1) + "]");
                    if (!string.IsNullOrEmpty(n.Message))
                        builder.Append(".赠言:" + n.Message + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }
        else
        {
            IList<BCW.farm.Model.NC_messagelog> listFarm = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_messagelog n in listFarm)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>：" + Out.SysUBB(n.AcText) + "[" + n.AddTime + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=message&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //宝箱管理
    private void baoxiangPage()
    {
        Master.Title = "" + GameName + "_宝箱管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;宝箱管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        //2为删除数据
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, @"^[1-2]\d*$", "1"));
        if (type == 2)
        {
            int aid = int.Parse(Utils.GetRequest("aid", "all", 1, @"^[1-9]\d*$", "0"));
            new BCW.farm.BLL.NC_baoxiang().Delete(aid);
            Utils.Success("删除宝箱记录", "删除一条数据成功.", Utils.getUrl("farm.aspx?act=baoxiang"), "1");
        }


        IList<BCW.farm.Model.NC_baoxiang> listFarm = new BCW.farm.BLL.NC_baoxiang().GetNC_baoxiangs(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_baoxiang n in listFarm)
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
                try
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<img src=\"" + n.picture + "\" alt=\"load\"/>");//height=\"45px\"
                }
                catch
                {
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".[图片出错!]");
                }
                builder.Append("" + n.prize + ".");
                if (n.type == 1)
                    builder.Append("(种子)");
                else
                    builder.Append("(道具)");
                builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang&amp;type=2&amp;aid=" + n.ID + "") + "\">[删]</a>");
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

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bx_jilu") + "\">用户抽奖情况查看</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;xiao=1") + "\">从已有的种子中添加</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli&amp;xiao=1") + "\">从已有的道具中添加</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zuowu_add&amp;type=10") + "\">新增珍稀种子</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=daoju_add&amp;type=10") + "\">新增高级道具</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //从已有中添加到宝箱
    private void bx_add_zzPage()
    {
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 2, @"^[1-2]\d*$", "选择类型出错"));//1作物2道具
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
        BCW.farm.Model.NC_baoxiang b = new BCW.farm.Model.NC_baoxiang();
        if (type == 1)
        {
            BCW.farm.Model.NC_shop a = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
            b.prize = a.name;
            b.daoju_id = a.num_id;
            try
            {
                string[] c = a.picture.Split(',');
                b.picture = c[3];
            }
            catch
            {
                b.picture = "";
            }
            b.type = 1;
            new BCW.farm.BLL.NC_baoxiang().Add(b);
            Utils.Success("增加宝箱种子", "增加该种子成功.<a href=\"" + Utils.getUrl("farm.aspx?act=zuowuguanli&amp;xiao=1") + "\">继续添加</a>", Utils.getUrl("farm.aspx?act=baoxiang"), "2");
        }
        else
        {
            BCW.farm.Model.NC_daoju aa = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
            b.prize = aa.name;
            b.daoju_id = aa.ID;
            b.picture = aa.picture;
            b.type = 2;
            new BCW.farm.BLL.NC_baoxiang().Add(b);
            Utils.Success("增加宝箱道具", "增加该道具成功.<a href=\"" + Utils.getUrl("farm.aspx?act=daojuguanli&amp;xiao=1") + "\">继续添加</a>", Utils.getUrl("farm.aspx?act=baoxiang"), "2");
        }
    }

    //用户在宝箱抽奖记录
    private void bx_jiluPage()
    {
        int type = Utils.ParseInt(Utils.GetRequest("type", "all", 1, @"^[0-3]\d*$", "0"));
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        if (type == 2)
        {
            int id2 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            int uid3 = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            BCW.farm.Model.NC_win bo = new BCW.farm.BLL.NC_win().GetNC_win(id2);
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?type=1&amp;act=bx_jilu&amp;id=" + id2 + "&amp;usid=" + uid3 + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid3 + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (type == 1)
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            int uid2 = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            //减少用户道具表中的中奖道具
            BCW.farm.Model.NC_win a = new BCW.farm.BLL.NC_win().GetNC_win(id);
            if (a.prize_id == 0)//金币
            {
                MatchCollection aa = Regex.Matches(a.prize_name, @"\d*");
                new BCW.farm.BLL.NC_user().UpdateiGold(a.usid, new BCW.BLL.User().GetUsName(a.usid), -Convert.ToInt64(aa[0].ToString()), "宝箱非法操作,扣除所得的" + aa[0] + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(a.usid) - Convert.ToInt64(aa[0].ToString())) + "金币.", 6);
                //删除宝箱中奖记录
                new BCW.farm.BLL.NC_win().Delete(id);
                Utils.Success("删除宝箱中奖记录", "删除成功，减少用户：" + new BCW.BLL.User().GetUsName(a.usid) + "(" + a.usid + ")" + aa[0] + "金币", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
            }
            else if (a.prize_id == 22222222)//经验
            {
                MatchCollection bb = Regex.Matches(a.prize_name, @"\d*");
                if (new BCW.farm.BLL.NC_user().Getjingyan(a.usid) < Convert.ToInt64(bb[0].ToString()))//如果减的大于原来经验
                {
                    long ui = (new BCW.farm.BLL.NC_user().Getjingyan(a.usid));//原经验
                    new BCW.farm.BLL.NC_user().Update_dengji2(a.usid);//等级减1
                    long jj = (((new BCW.farm.BLL.NC_user().GetGrade(a.usid)) + 1) * 200) - (Convert.ToInt64(bb[0].ToString()) - ui);
                    new BCW.farm.BLL.NC_user().update_zd("Experience=" + jj + "", "usid=" + a.usid + "");
                }
                else
                {
                    new BCW.farm.BLL.NC_user().Update_Experience(a.usid, -Convert.ToInt64(bb[0].ToString()));
                }
                //删除宝箱中奖记录
                new BCW.farm.BLL.NC_win().Delete(id);
                Utils.Success("删除宝箱中奖记录", "删除成功，减少用户：" + new BCW.BLL.User().GetUsName(a.usid) + "(" + a.usid + ")" + bb[0] + "经验", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
            }
            else if (a.prize_id == 11111111)//酷币
            {
                MatchCollection bb = Regex.Matches(a.prize_name, @"\d*");
                //如果币不够扣则记录日志并冻结IsFreeze
                if (new BCW.BLL.User().GetGold(a.usid) < Convert.ToInt64(bb[0].ToString()))
                {
                    BCW.Model.Gameowe owe = new BCW.Model.Gameowe();
                    owe.Types = 1;
                    owe.UsID = a.usid;
                    owe.UsName = new BCW.BLL.User().GetUsName(a.usid);
                    owe.Content = "在农场开宝箱中非法操作，扣除所得的" + bb[0] + "" + ub.Get("SiteBz") + ".已扣" + new BCW.BLL.User().GetGold(a.usid) + "，还差" + (Convert.ToInt64(bb[0].ToString()) - new BCW.BLL.User().GetGold(a.usid)) + "";
                    owe.OweCent = (Convert.ToInt32(bb[0]) - new BCW.BLL.User().GetGold(a.usid));
                    owe.BzType = 11;//农场抽奖记录type的id
                    owe.EnId = a.ID;
                    owe.AddTime = DateTime.Now;
                    new BCW.BLL.Gameowe().Add(owe);
                    new BCW.BLL.User().UpdateIsFreeze(a.usid, 1);
                }
                //操作币并内线通知
                new BCW.BLL.User().UpdateiGold(a.usid, new BCW.BLL.User().GetUsName(a.usid), -Convert.ToInt64(bb[0].ToString()), "宝箱非法操作,扣除所得的" + bb[0] + "" + ub.Get("SiteBz") + "");
                //发送内线
                string strGuess = "你在农场非法操作宝箱，你欠下系统的" + bb[0] + "" + ub.Get("SiteBz") + ".[br]根据您的帐户数额，实扣" + bb[0] + "" + ub.Get("SiteBz") + ".[br]如果您的" + ub.Get("SiteBz") + "不足，系统将您帐户冻结，直到成功扣除为止。[br]";
                new BCW.BLL.Guest().Add(1, a.usid, new BCW.BLL.User().GetUsName(a.usid), strGuess);
                //删除宝箱中奖记录
                new BCW.farm.BLL.NC_win().Delete(id);
                Utils.Success("删除宝箱中奖记录", "删除成功，减少用户：" + new BCW.BLL.User().GetUsName(a.usid) + "(" + a.usid + ")" + bb[0] + "" + ub.Get("SiteBz") + "", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
            }
            else
            {
                if (a.prize_type == 1)//种子
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(a.usid, a.prize_id) > 0)
                    {
                        new BCW.farm.BLL.NC_mydaoju().Update_zz(a.usid, -1, a.prize_id);
                        //删除宝箱中奖记录
                        new BCW.farm.BLL.NC_win().Delete(id);
                        Utils.Success("删除宝箱中奖记录", "删除成功，减少用户：" + new BCW.BLL.User().GetUsName(a.usid) + "(" + a.usid + ")的种子数量1个", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
                    }
                    else
                        Utils.Success("删除宝箱中奖记录", "删除失败，由于该用户的仓库不存在该种子.", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
                }
                else //if (a.prize_type == 2)//道具
                {
                    //删除宝箱中奖记录
                    new BCW.farm.BLL.NC_win().Delete(id);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(a.prize_id, a.usid, 1))
                    {
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(a.usid, -1, a.prize_id, 1);
                        //删除宝箱中奖记录
                        new BCW.farm.BLL.NC_win().Delete(id);
                        Utils.Success("删除宝箱中奖记录", "删除成功，减少用户：" + new BCW.BLL.User().GetUsName(a.usid) + "(" + a.usid + ")的道具数量1个", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
                    }
                    else
                        Utils.Success("删除宝箱中奖记录", "删除失败，由于该用户的仓库不存在该道具.", Utils.getUrl("farm.aspx?act=bx_jilu&amp;usid=" + uid2 + ""), "2");
                }
            }
        }
        else
        {
            Master.Title = "" + GameName + "宝箱管理";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">宝箱管理</a>&gt;抽奖情况查看");
            builder.Append(Out.Tab("</div>", "<br/>"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "usid", "type", "backurl" };
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (uid > 0)
                strWhere = "UsID=" + uid + "";
            else
                strWhere = "";

            IList<BCW.farm.Model.NC_win> listFarm = new BCW.farm.BLL.NC_win().GetNC_wins(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_win n in listFarm)
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
                    string mename = new BCW.BLL.User().GetUsName(n.usid);
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + n.usid + ")</a>获得：" + n.prize_name + "[" + n.addtime + "]");
                    builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=bx_jilu&amp;id=" + n.ID + "&amp;type=2&amp;usid=" + uid + "") + "\">[删]</a>");
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
            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=bx_jilu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=baoxiang") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }

    //用户消费查看
    private void xiaofeiPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;消费查看");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        if (uid > 0)
            strWhere = "UsID=" + uid + "";
        else
            strWhere = "";

        // 开始读取列表
        IList<BCW.farm.Model.NC_Goldlog> listGoldlog = new BCW.farm.BLL.NC_Goldlog().GetNC_Goldlogs2(pageIndex, pageSize, strWhere, out recordCount);
        if (listGoldlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_Goldlog n in listGoldlog)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}|操作{4}|结{5}({6})", (pageIndex - 1) * pageSize + k, n.UsId, n.UsName, Out.SysUBB(n.AcText.Replace("/bbs/guess2/", "guess2/")), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 0));

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

        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=xiaofei,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=xiaofei_jt") + "\">[查看具体情况]</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //消费具体情况
    private void xiaofei_jtPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei") + "\">消费查看</a>&gt;消费具体情况");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-9]\d*$", "1"));
        if (ptype == 1)
        {
            Master.Title = "用户消费—草虫水";
            builder.Append("草虫水 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=1";
            else
                strWhere = "Bbtag=1";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=1&amp;UsID=" + uid + "") + "\">草虫水</a> ");
        }
        if (ptype == 2)
        {
            Master.Title = "用户消费—奴隶";
            builder.Append("奴隶 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and (Bbtag=2 or Bbtag=11) ";
            else
                strWhere = "(type=2 or Bbtag=11)";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=2&amp;UsID=" + uid + "") + "\">奴隶</a> ");
        }
        if (ptype == 3)
        {
            Master.Title = "用户消费—商店";
            builder.Append("商店 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=4";
            else
                strWhere = "Bbtag=4";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=3&amp;UsID=" + uid + "") + "\">商店</a> ");
        }
        if (ptype == 4)
        {
            Master.Title = "用户消费—签到";
            builder.Append("签到 <br/>");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=5";
            else
                strWhere = "Bbtag=5";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=4&amp;UsID=" + uid + "") + "\">签到</a> <br/>");
        }
        if (ptype == 5)
        {
            Master.Title = "用户消费—宝箱";
            builder.Append("宝箱 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=6";
            else
                strWhere = "Bbtag=6";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=5&amp;UsID=" + uid + "") + "\">宝箱</a> ");
        }
        if (ptype == 6)
        {
            Master.Title = "用户消费—市场";
            builder.Append("市场 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=7";
            else
                strWhere = "Bbtag=7";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=6&amp;UsID=" + uid + "") + "\">市场</a> ");
        }
        if (ptype == 7)
        {
            Master.Title = "用户消费—放种";
            builder.Append("放种 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=9";
            else
                strWhere = "Bbtag=9";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=7&amp;UsID=" + uid + "") + "\">放种</a> ");
        }
        if (ptype == 8)
        {
            Master.Title = "用户消费—功能";
            builder.Append("功能 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=10";
            else
                strWhere = "Bbtag=10";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=8&amp;UsID=" + uid + "") + "\">功能</a> ");
        }
        if (ptype == 9)
        {
            Master.Title = "用户消费—兑换";
            builder.Append("兑换 ");
            if (uid > 0)
                strWhere = "UsID=" + uid + " and Bbtag=11";
            else
                strWhere = "Bbtag=11";
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_jt&amp;ptype=9&amp;UsID=" + uid + "") + "\">兑换</a> ");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        IList<BCW.farm.Model.NC_Goldlog> listFarm = new BCW.farm.BLL.NC_Goldlog().GetNC_Goldlogs2(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_Goldlog n in listFarm)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.UsId + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "</a>：" + Out.SysUBB(n.AcText) + "[" + n.AddTime + "]");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=xiaofei_jt&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //用户查看
    private void user_glPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "edit")
        {
            Master.Title = "" + GameName + "_用户修改";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=paihang") + "\">用户排行</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=user_gl") + "\">用户查看</a>&gt;用户修改");
            builder.Append(Out.Tab("</div>", "<br/>"));
            int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            BCW.farm.Model.NC_user aa = new BCW.farm.BLL.NC_user().Get_user(usid);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("正在修改【" + new BCW.BLL.User().GetUsName(usid) + "】的信息：");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "等级：/,金币：/,草虫水经验：/,播种经验：/,种草放虫经验：/,施肥次数：/,一键除草：/,一键铲地：/,一键除虫：/,一键施肥：/,一键收获：/,一键浇水：/,一键播种：/,一键摆摊：/";
            string strName = "Grade,Goid,big_bangmang,big_bozhong,big_shihuai,big_shifei,iscao,ischan,isinsect,isshifei,isshou,iswater,iszhong,isbaitan";
            string strType = "num,num,num,num,num,num,select,select,select,select,select,select,select,select";
            string strValu = "" + aa.Grade + "'" + aa.Goid + "'" + aa.big_bangmang + "'" + aa.big_bozhong + "'" + aa.big_shihuai + "'" + aa.big_shifei + "'" + aa.iscao + "'" + aa.ischan + "'" + aa.isinsect + "'" + aa.isshifei + "'" + aa.isshou + "'" + aa.iswater + "'" + aa.iszhong + "'" + aa.isbaitan + "";
            string strEmpt = "false,false,false,false,false,false,0|无|1|有,0|无|1|有,0|无|1|有,0|无|1|有,0|无|1|有,0|无|1|有,0|无|1|有,0|无|1|有";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx?act=user_gl&amp;usid=" + usid + "&amp;ac=edit1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示:<br/>1.经验分别为：除草除虫浇水经验、播种经验、种草放虫经验.<br/>");
            builder.Append("2.一键功能为:1除草、2铲地、3除虫、4施肥、5收获、6浇水、7播种、8摆摊.<br/>");
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=user_gl") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ac == "edit1")
        {
            int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            int Grade = int.Parse(Utils.GetRequest("Grade", "all", 2, @"^[0-9]\d*$", "等级填写出错"));
            long Goid = Convert.ToInt64(Utils.GetRequest("Goid", "all", 2, @"^\d*$", "金币填写出错"));
            int big_bangmang = int.Parse(Utils.GetRequest("big_bangmang", "all", 2, @"^[0-9]\d*$", "经验1填写出错"));
            int big_bozhong = int.Parse(Utils.GetRequest("big_bozhong", "all", 2, @"^[0-9]\d*$", "经验2填写出错"));
            int big_shihuai = int.Parse(Utils.GetRequest("big_shihuai", "all", 2, @"^[0-9]\d*$", "经验3填写出错"));
            int big_shifei = int.Parse(Utils.GetRequest("big_shifei", "all", 2, @"^[0-9]\d*$", "限制施肥的次数填写出错"));

            int iscao = int.Parse(Utils.GetRequest("iscao", "all", 2, @"^[0-1]\d*$", "一键除草选择出错"));
            int ischan = int.Parse(Utils.GetRequest("ischan", "all", 2, @"^[0-1]\d*$", "一键铲地选择出错"));
            int isinsect = int.Parse(Utils.GetRequest("isinsect", "all", 2, @"^[0-1]\d*$", "一键除虫选择出错"));
            int isshifei = int.Parse(Utils.GetRequest("isshifei", "all", 2, @"^[0-1]\d*$", "一键施肥选择出错"));
            int isshou = int.Parse(Utils.GetRequest("isshou", "all", 2, @"^[0-1]\d*$", "一键收获选择出错"));
            int iswater = int.Parse(Utils.GetRequest("iswater", "all", 2, @"^[0-1]\d*$", "一键浇水选择出错"));
            int iszhong = int.Parse(Utils.GetRequest("iszhong", "all", 2, @"^[0-1]\d*$", "一键播种选择出错"));
            int isbaitan = int.Parse(Utils.GetRequest("isbaitan", "all", 2, @"^[0-1]\d*$", "一键摆摊选择出错"));
            new BCW.farm.BLL.NC_user().update_zd("Grade=" + Grade + ",Goid=" + Goid + ",big_bozhong=" + big_bozhong + ",big_bangmang=" + big_bangmang + ",big_shihuai=" + big_shihuai + ",big_shifei=" + big_shifei + ",iscao=" + iscao + ",ischan=" + ischan + ",isinsect=" + isinsect + ",isshifei= " + isshifei + ",isshou= " + isshou + ",iswater= " + iswater + ",iszhong= " + iszhong + ",isbaitan= " + isbaitan + "", "usid=" + usid + "");
            Utils.Success("修改数据", "修改数据成功.正在返回.", Utils.getUrl("farm.aspx?act=user_gl&amp;ac=edit&amp;usid=" + usid + ""), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_用户查看";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=paihang") + "\">用户排行</a>&gt;用户查看");
            builder.Append(Out.Tab("</div>", "<br/>"));
            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            string strOrder = "grade desc";//等级排列
            string a, b, c, d, e, f, g, h = string.Empty;

            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            if (uid > 0)
                strWhere = "UsID=" + uid + "";
            else
                strWhere = "";

            IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listFarm)
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
                    string mename = new BCW.BLL.User().GetUsName(n.usid);//用户姓名
                    //<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei&amp;usid=" + n.usid + "") + "\">" + mename + "</a>.");
                    builder.Append("(" + n.Grade + "级/" + n.Experience + "经验/" + n.Goid + "金币)");
                    builder.Append("[经验:" + n.big_bangmang + "、" + n.big_bozhong + "、" + n.big_shihuai + "、" + n.big_shifei + "]");
                    if (n.iscao == 1) { a = "有/"; } else { a = "无/"; }
                    if (n.ischan == 1) { b = "有/"; } else { b = "无/"; }
                    if (n.isinsect == 1) { c = "有/"; } else { c = "无/"; }
                    if (n.isshifei == 1) { d = "有/"; } else { d = "无/"; }
                    if (n.isshou == 1) { e = "有/"; } else { e = "无/"; }
                    if (n.iswater == 1) { f = "有/"; } else { f = "无/"; }
                    if (n.iszhong == 1) { g = "有/"; } else { g = "无/"; }
                    if (n.isbaitan == 1) { h = "有"; } else { h = "无"; }

                    builder.Append("{一键功能:" + "1" + a + "2" + b + "3" + c + "4" + d + "5" + e + "6" + f + "7" + g + "8" + h + "}");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=user_gl&amp;usid=" + n.usid + "&amp;ac=edit") + "\">[修改]</a>");
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

            builder.Append(Out.Tab("", Out.Hr()));
            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=user_gl,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示:<br/>1.经验分别为：除草除虫浇水经验、播种经验、种草放虫经验、施肥次数.<br/>");
            builder.Append("2.一键功能为:1除草、2铲地、3除虫、4施肥、5收获、6浇水、7播种、8摆摊.<br/>");
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=paihang") + "\">返回上级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //盈利分析
    private void yingliPage()
    {
        Master.Title = "" + GameName + "_盈利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", "<br/>"));

        //大于0系统亏，少于0系统赚
        //今天
        DataSet qq = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "DateDiff(dd,AddTime,getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold<0 ");
        long TodayTou = 0;
        try
        {
            TodayTou = 0 - Convert.ToInt64(qq.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            TodayTou = 0;
        }
        DataSet qqq = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "DateDiff(dd,AddTime,getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold>0 ");
        long TodayDui = 0;
        try
        {
            TodayDui = Convert.ToInt64(qqq.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            TodayDui = 0;
        }

        //昨天
        DataSet ww = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "DateDiff(dd,AddTime,getdate()-1)=0 AND AcText LIKE '%您在农场%' AND AcGold<0 ");
        long yesTou = 0;
        try
        {
            yesTou = 0 - Convert.ToInt64(ww.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            yesTou = 0;
        }
        DataSet www = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "DateDiff(dd,AddTime,getdate()-1)=0 AND AcText LIKE '%您在农场%' AND AcGold>0 ");
        long yesDui = 0;
        try
        {
            yesDui = Convert.ToInt64(www.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            yesDui = 0;
        }

        //本月
        DataSet ee = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(month,[AddTime],getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold<0 ");
        long MonthTou = 0;
        try
        {
            MonthTou = 0 - Convert.ToInt64(ee.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            MonthTou = 0;
        }
        DataSet eee = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(month,[AddTime],getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold>0 ");
        long MonthDui = 0;
        try
        {
            MonthDui = Convert.ToInt64(eee.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            MonthDui = 0;
        }
        //上月
        DataSet rr = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(month,[AddTime],getdate())=1 AND AcText LIKE '%您在农场%' AND AcGold<0 ");
        long Month2Tou = 0;
        try
        {
            Month2Tou = 0 - Convert.ToInt64(rr.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            Month2Tou = 0;
        }
        DataSet rrr = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(month,[AddTime],getdate())=1 AND AcText LIKE '%您在农场%' AND AcGold>0 ");
        long Month2Dui = 0;
        try
        {
            Month2Dui = Convert.ToInt64(rrr.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            Month2Dui = 0;
        }
        //今年
        DataSet tt = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(YEAR,[AddTime],getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold<0 ");
        long yearTou = 0;
        try
        {
            yearTou = 0 - Convert.ToInt64(tt.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            yearTou = 0;
        }
        DataSet ttt = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "datediff(YEAR,[AddTime],getdate())=0 AND AcText LIKE '%您在农场%' AND AcGold>0 ");
        long yearDui = 0;
        try
        {
            yearDui = Convert.ToInt64(ttt.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            yearDui = 0;
        }
        //总
        DataSet yy = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "AcText LIKE '%您在农场%' AND AcGold<0 ");
        long allTou = 0;
        try
        {
            allTou = 0 - Convert.ToInt64(yy.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            allTou = 0;
        }
        DataSet yyy = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "AcText LIKE '%您在农场%' AND AcGold>0 ");
        long allDui = 0;
        try
        {
            allDui = Convert.ToInt64(yyy.Tables[0].Rows[0]["aa"].ToString());
        }
        catch
        {
            allDui = 0;
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天赢利：" + (TodayTou - TodayDui) + "" + ub.Get("SiteBz") + "<br/>今天收支：收" + TodayTou + "，支" + TodayDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天赢利：" + (yesTou - yesDui) + "" + ub.Get("SiteBz") + "<br/>昨天收支：收" + yesTou + "，支" + yesDui + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("本月赢利：" + (MonthTou - MonthDui) + "" + ub.Get("SiteBz") + "<br/>本月收支：收" + MonthTou + "，支" + MonthDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月赢利：" + (Month2Tou - Month2Dui) + "" + ub.Get("SiteBz") + "<br/>上月收支：收" + Month2Tou + "，支" + Month2Dui + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("今年赢利：" + (yearTou - yearDui) + "" + ub.Get("SiteBz") + "<br/>今年收支：收" + yearTou + "，支" + yearDui + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>总赢利：" + (allTou - allDui) + "" + ub.Get("SiteBz") + "<br/>总收支：收" + allTou + "，支" + allDui + "</b>");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            DateTime searchday1 = Utils.ParseTime(Utils.GetRequest("sTime", "all", 2, DT.RegexTime, "开始时间填写无效"));
            DateTime searchday2 = Utils.ParseTime(Utils.GetRequest("oTime", "all", 2, DT.RegexTime, "结束时间填写无效"));

            DataSet aa = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "AddTime>='" + searchday1 + "' and AddTime<='" + searchday2 + "' and AcText LIKE '%您在农场%' AND AcGold<0 ");
            long dateTou = 0;
            try
            {
                dateTou = 0 - Convert.ToInt64(aa.Tables[0].Rows[0]["aa"].ToString());
            }
            catch
            {
                dateTou = 0;
            }
            DataSet bb = new BCW.BLL.Goldlog().GetList("SUM(AcGold) AS aa", "AddTime>='" + searchday1 + "' and AddTime<='" + searchday2 + "' and AcText LIKE '%您在农场%' AND AcGold>0 ");
            long dateDui = 0;
            try
            {
                dateDui = 0 - Convert.ToInt64(bb.Tables[0].Rows[0]["aa"].ToString());
            }
            catch
            {
                dateDui = 0;
            }

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利" + (dateTou - dateDui) + "" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + searchday1.ToString("yyyy-MM-dd HH:mm:ss") + "'" + searchday2.ToString("yyyy-MM-dd HH:mm:ss") + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,farm.aspx?act=yingli,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<b>盈利0" + ub.Get("SiteBz") + ".</b>");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime";
            string strType = "date,date";
            string strValu = "" + DT.FormatDate(DateTime.Now.AddDays(-1), 0) + "'" + DT.FormatDate(DateTime.Now, 0) + "''";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "马上查询,farm.aspx?act=yingli,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=xiaofei_gl") + "\"><h style=\"color:red\">[消费馈赠]</h></a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //摊位管理
    private void tanwei_glPage()
    {
        Master.Title = "" + GameName + "_摊位管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;摊位管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "1")
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择摊位id出错"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok&amp;act=tanwei_gl&amp;id=" + id + "&amp;usid=" + uid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tanwei_gl&amp;usid=" + uid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ac == "ok")
        {
            int id_1 = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择摊位id出错"));
            BCW.farm.Model.NC_market aa = new BCW.farm.BLL.NC_market().GetNC_market(id_1);
            new BCW.farm.BLL.NC_mydaoju().Update_hf(aa.usid, aa.daoju_num, aa.daoju_num, 0);//把道具加到道具表
            BCW.farm.Model.NC_daoju kk = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id_1);//根据id查询道具信息
            new BCW.farm.BLL.NC_market().Delete(id_1);
            new BCW.farm.BLL.NC_messagelog().addmessage(aa.usid, new BCW.BLL.User().GetUsName(aa.usid), "非法摆摊,系统已自动清理.退回摊位上的" + aa.daoju_num + "个" + kk.name + ".", 7);//内线消息
            Utils.Success("删除摊位", "删除摊位成功,正在返回.", Utils.getUrl("farm.aspx?act=tanwei_gl&amp;usid=" + uid + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            if (ptype == 1)
            {
                builder.Append("正在交易" + "" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tanwei_gl&amp;ptype=1") + "\">正在交易</a>" + "|");
            }
            if (ptype == 2)
            {
                builder.Append("过期交易" + "" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tanwei_gl&amp;ptype=2") + "\">过期交易</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strOrder = "";


            if (ptype == 1)
            {
                if (uid > 0)
                    strWhere = "UsID=" + uid + " and type=1";
                else
                    strWhere = "type=1";

                strOrder = " add_time Desc";
            }
            else
            {
                if (uid > 0)
                    strWhere = "UsID=" + uid + " and type=0";
                else
                    strWhere = "type=0";

                strOrder = " add_time Desc";
            }

            IList<BCW.farm.Model.NC_market> listFarm = new BCW.farm.BLL.NC_market().GetNC_markets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_market n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", ""));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.Append("名称：" + n.daoju_name + "<br/>");
                    try
                    {
                        builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");
                    }
                    catch
                    {
                        builder.Append("[图片出错!]<br/>");
                    }
                    if (ptype == 1)
                    {
                        builder.Append("数量：" + n.daoju_num + "<br/>");
                        builder.Append("价格：" + n.daoju_price + "金币/个<br/>所属：" + new BCW.BLL.User().GetUsName(n.usid) + "(" + n.usid + ")<br/>");
                        builder.Append("时间：[" + n.add_time + "]<br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tanwei_gl&amp;id=" + n.ID + "&amp;ac=1") + "\">[删除交易]</a>");
                    }
                    else
                    {
                        builder.Append("剩余数量：" + n.daoju_num + "");
                        if (n.daoju_num > 0)
                            builder.Append("(已退回)<br/>");
                        else
                            builder.Append("<br/>");
                        builder.Append("价格：" + n.daoju_price + "金币/个<br/>所属：" + new BCW.BLL.User().GetUsName(n.usid) + "(" + n.usid + ")<br/>");
                        builder.Append("时间：[" + n.add_time + "]");
                    }
                    builder.Append(Out.Tab("", Out.Hr()));
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..<br/>"));
            }

            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=tanwei_gl&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //奴隶管理
    private void nuli_glPage()
    {
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));//
        int slave_id = int.Parse(Utils.GetRequest("slave_id", "all", 1, @"^[1-9]\d*$", "0"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "1")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok&amp;act=nuli_gl&amp;id=" + id + "&amp;usid=" + uid + "&amp;slave_id=" + slave_id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl&amp;usid=" + uid + "&amp;slave_id=" + slave_id + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ac == "ok")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
            new BCW.farm.BLL.NC_slave().Delete(id);
            Utils.Success("删除奴隶", "删除奴隶成功,正在返回.", Utils.getUrl("farm.aspx?act=nuli_gl&amp;usid=" + uid + "&amp;slave_id=" + slave_id + ""), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_奴隶管理";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;奴隶管理");
            builder.Append(Out.Tab("</div>", "<br/>"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            if (ptype == 1)
            {
                builder.Append("存在奴隶关系" + "" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl&amp;ptype=1") + "\">存在奴隶关系</a>" + "|");
            }
            if (ptype == 2)
            {
                builder.Append("过期奴隶" + "" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl&amp;ptype=2") + "\">过期奴隶</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "usid", "slave_id", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strOrder = "";

            if (ptype == 1)
            {
                if (uid > 0)
                {
                    if (slave_id > 0)
                        strWhere = "UsID=" + uid + " and slave_id=" + slave_id + " and tpye=1";
                    else
                        strWhere = "UsID=" + uid + " and tpye=1";
                }
                else
                {
                    if (slave_id > 0)
                        strWhere = "slave_id=" + slave_id + " and tpye=1";
                    else
                        strWhere = "tpye=1";
                }
            }
            else
            {
                if (uid > 0)
                {
                    if (slave_id > 0)
                        strWhere = "UsID=" + uid + " and slave_id=" + slave_id + " and tpye=0";
                    else
                        strWhere = "UsID=" + uid + " and tpye=0";
                }
                else
                {
                    if (slave_id > 0)
                        strWhere = "slave_id=" + slave_id + " and tpye=0";
                    else
                        strWhere = "tpye=0";
                }
            }
            strOrder = " updatetime Desc";

            IList<BCW.farm.Model.NC_slave> listFarm = new BCW.farm.BLL.NC_slave().GetNC_slaves(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_slave n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", ""));
                    }
                    if (ptype == 1)
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        builder.Append("<h style=\"color:red\">奴隶主</h>：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.usid) + "(" + n.usid + ")</a><br/>");
                        builder.Append("奴隶：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.slave_id) + "(" + n.slave_id + ")</a>");
                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl&amp;ac=1&amp;id=" + n.ID + "") + "\">[删]</a><br/>");
                        //builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=nuli_gl&amp;ac=2&amp;id=" + n.ID + "") + "\">[改]</a><br/>");
                        builder.Append("惩罚：" + n.punish + "次.安抚：" + n.pacify + "次.<br/>");
                        builder.Append("时间：[" + n.updatetime + "]<br/>");
                        builder.Append("到期：[" + n.updatetime.AddDays(int.Parse(ub.GetSub("slave_day", xmlPath))) + "]");
                    }
                    else
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        builder.Append("<h style=\"color:red\">奴隶主</h>：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.usid) + "(" + n.usid + ")</a><br/>");
                        builder.Append("奴隶：<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.slave_id) + "(" + n.slave_id + ")</a><br/>");
                        builder.Append("惩罚：" + n.punish + "次.安抚：" + n.pacify + "次.<br/>");
                        builder.Append("时间：[" + n.updatetime + "]<br/>");
                        builder.Append("到期：[" + n.updatetime.AddDays(int.Parse(ub.GetSub("slave_day", xmlPath))) + "]");
                    }
                    builder.Append(Out.Tab("", Out.Hr()));

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录..<br/>"));
            }

            string strText = "输入奴隶主ID(为空搜索全部):/,输入奴隶ID:/,";
            string strName = "usid,slave_id,backurl";
            string strType = "num,num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
            {
                if (slave_id == 0)
                    strValu = "''" + Utils.getPage(0) + "";
                else
                    strValu = "'" + slave_id + "'" + Utils.getPage(0) + "";
            }
            else
            {
                if (slave_id == 0)
                    strValu = "" + uid + "''" + Utils.getPage(0) + "";
                else
                    strValu = "" + uid + "'" + slave_id + "'" + Utils.getPage(0) + "";
            }
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=nuli_gl&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //仓库管理
    private void cangku_glPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;仓库管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-3]$", "1"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "_仓库管理.果实";
            builder.Append("果实" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=1&amp;usid=" + uid + "") + "\">果实</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "" + GameName + "_仓库管理.种子";
            builder.Append("种子" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=2&amp;usid=" + uid + "") + "\">种子</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "" + GameName + "_仓库管理.道具";
            builder.Append("道具" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=3&amp;usid=" + uid + "") + "\">道具</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        if (uid > 0)
            strWhere = "usid=" + uid + " AND";
        else
            strWhere = "";
        string[] pageValUrl = { "act", "ptype", "ptype2", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
        {
            #region 果实
            DataSet ds = new BCW.farm.BLL.NC_GetCrop().GetList(" usid,COUNT(usid) as aa ", "" + strWhere + " num>0 group by usid ORDER BY aa DESC ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);//用户id
                    int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);//用户id
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=1") + "\">[" + id + "种果实]</a>");
                    builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_cg&amp;usid=" + UsID + "") + "\">[查看战绩]</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
            #endregion
        }
        if (ptype == 2)
        {
            #region 种子
            DataSet ds = new BCW.farm.BLL.NC_mydaoju().GetList(" usid,COUNT(usid) as aa ", "" + strWhere + " type=1 and num>0 and name_id>0 group by usid ORDER BY aa DESC ");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
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
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);
                    int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=2") + "\">[" + id + "种种子]</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
            #endregion
        }
        if (ptype == 3)
        {
            builder.Append(Out.Tab("<div>", ""));
            if (ptype2 == 1)
            {
                builder.Append("<h style=\"color:red\">已拥有" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=3&amp;ptype2=1&amp;usid=" + uid + "") + "\">已拥有</a>" + "|");
            }
            if (ptype2 == 2)
            {
                builder.Append("<h style=\"color:red\">使用中" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=3&amp;ptype2=2&amp;usid=" + uid + "") + "\">使用中</a>" + "|");
            }
            if (ptype2 == 3)
            {
                builder.Append("<h style=\"color:red\">已使用" + "</h>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl&amp;ptype=3&amp;ptype2=3&amp;usid=" + uid + "") + "\">已使用</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ptype2 == 1)
            {
                #region 已拥有的道具
                DataSet ds = new BCW.farm.BLL.NC_mydaoju().GetList(" usid,COUNT(usid) as aa ", "" + strWhere + " type=2 and num>0 and huafei_id>0 group by usid ORDER BY aa DESC ");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
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
                    for (int i = 0; i < skt; i++)
                    {
                        int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);
                        int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=3&amp;type=1") + "\">[" + id + "种道具]</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
                #endregion
            }
            else
            {
                #region 正在使用/已使用的道具
                string strWhere2 = " ";
                if (ptype2 == 2)
                    strWhere2 = "type=1";
                if (ptype2 == 3)
                    strWhere2 = "type=0";
                DataSet ds = new BCW.farm.BLL.NC_daoju_use().GetList(" usid,COUNT(usid) as aa ", "" + strWhere + " " + strWhere2 + " group by usid ORDER BY aa DESC ");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
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
                    for (int i = 0; i < skt; i++)
                    {
                        int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["usid"]);
                        int id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["aa"]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;usid=" + UsID + "&amp;ptype=3&amp;type=" + ptype2 + "") + "\">[" + id + "种道具]</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
                #endregion
            }
        }

        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=cangku_gl&amp;ptype=" + ptype + "&amp;ptype2=" + ptype2 + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>成果是指收获和偷取的果实数量。<br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //仓库成果查看
    private void cangku_cgPage()
    {
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl") + "\">仓库管理</a>&gt;" + new BCW.BLL.User().GetUsName(uid) + "的成果查看");
        builder.Append(Out.Tab("</div>", "<br/>"));

        Master.Title = "仓库管理." + new BCW.BLL.User().GetUsName(uid) + "的成果查看";

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "usid", "ptype2", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);

        if (pageIndex == 0)
            pageIndex = 1;

        string strOrder = "";
        strWhere = "UsId=" + uid + " AND (tou_nums>0 OR get_nums>0)";
        strOrder = "name_id asc";
        IList<BCW.farm.Model.NC_GetCrop> listFarm = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_GetCrop n in listFarm)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + ":收获" + (n.get_nums) + ".偷取" + n.tou_nums + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有" + new BCW.BLL.User().GetUsName(uid) + "的相关记录.."));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //仓库usid详细显示
    private void cangku_gl_showPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl") + "\">仓库管理</a>&gt;物品详细显示");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]$", "1"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "type", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = "";

        if (ptype == 1)
        {
            #region 果实详细
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("你正在查看【" + mename + "】的果实：");
            builder.Append(Out.Tab("</div>", "<br/>"));

            strWhere = " usid=" + usid + " and num>0";
            strOrder = "";
            IList<BCW.farm.Model.NC_GetCrop> listFarm = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_GetCrop n in listFarm)
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
                    //根据name_id查询对应商店表的图片
                    BCW.farm.Model.NC_shop jj = new BCW.farm.BLL.NC_shop().GetNC_shop1(n.name_id);
                    builder.Append(n.name);
                    //if (!new BCW.farm.BLL.NC_shop().Exists_zzid2(n.name_id))
                    //{
                    //    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + "<h style=\"color:#A9A9A9\">(" + (n.price_out) + "金币/个)</h>：x" + n.num + ".<br/>");
                    //    builder.Append("(抱歉,该商品[" + n.name + "]已下架.)");
                    //}
                    //else
                    //{
                    try
                    {
                        if (jj.picture != "")
                        {
                            string[] ii = jj.picture.Split(',');
                            builder.Append("<img src=\"" + ii[4] + "\" alt=\"load\"/><br/>");
                        }
                    }
                    catch
                    {
                        builder.Append("[图片出错!]<br/>");
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + "<h style=\"color:#A9A9A9\">(" + (n.price_out) + "金币/个)</h>：");
                    builder.Append("x" + n.num + "");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_gai&amp;ptype=" + ptype + "&amp;id=" + n.name_id + "&amp;usid=" + usid + "") + "\">[改]</a>");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_del&amp;ptype=" + ptype + "&amp;id=" + n.name_id + "&amp;usid=" + usid + "") + "\">[删]</a><br/>");
                    if (n.suoding == 0)
                    {
                        builder.Append("未加锁.");
                        if (jj.iszengsong == 1)
                        {
                            builder.Append("可赠送.");
                        }
                    }
                    else
                    {
                        builder.Append("已加锁");
                    }
                    //}
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
            #endregion
        }
        else if (ptype == 2)
        {
            #region 种子详细
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("你正在查看【" + mename + "】的种子：");
            builder.Append(Out.Tab("</div>", "<br/>"));

            strWhere = " usid=" + usid + " and type=1 and num>0 and name_id>0";
            IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
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
                    BCW.farm.Model.NC_shop b = new BCW.farm.BLL.NC_shop().GetNC_shop1(n.name_id);
                    try
                    {
                        builder.Append("<img  src=\"" + n.picture + "\" alt=\"load\"/><br/>");
                    }
                    catch
                    {
                        builder.Append("[图片出错!]<br/>");
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + "<h style=\"color:#A9A9A9\">(" + (b.price_in / 2) + "金币/个)</h>：");
                    builder.Append("x" + n.num + "");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_gai&amp;ptype=" + ptype + "&amp;id=" + n.name_id + "&amp;usid=" + usid + "") + "\">[改]</a>");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_del&amp;ptype=" + ptype + "&amp;id=" + n.name_id + "&amp;usid=" + usid + "") + "\">[删]</a><br/>");
                    if (n.suoding == 0)
                    {
                        builder.Append("未加锁.");
                    }
                    else
                    {
                        builder.Append("已加锁");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无数据!"));
            }
            #endregion
        }
        else
        {
            if (type == 1)
            {
                #region 拥有道具
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你正在查看【" + mename + "】已拥有的道具：");
                builder.Append(Out.Tab("</div>", "<br/>"));
                strWhere = "usid=" + usid + " and type=2 and num>0 and huafei_id>0";
                // 开始读取列表
                IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus(pageIndex, pageSize, strWhere, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
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
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        try
                        {
                            builder.Append("<img src=\"" + n.picture + "\" alt=\"load\"/>");//new BCW.farm.BLL.NC_daoju().Get_picture(n.huafei_id)
                        }
                        catch
                        {
                            builder.Append("[图片出错!]<br/>");
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=" + n.huafei_id + "") + "\">" + n.name + "</a>");
                        builder.Append("x" + n.num + "");
                        builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_gai&amp;ptype=" + ptype + "&amp;id=" + n.huafei_id + "&amp;usid=" + usid + "") + "\">[改]</a>");
                        builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_del&amp;ptype=" + ptype + "&amp;id=" + n.huafei_id + "&amp;usid=" + usid + "") + "\">[删]</a>");

                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
                #endregion
            }
            else
            {
                #region 正在使用或已使用道具
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你正在查看【" + mename + "】已使用的道具：");
                builder.Append(Out.Tab("</div>", "<br/>"));
                int pageIndex2;
                int recordCount2;
                int pageSize2 = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
                string strWhere2 = "";
                string[] pageValUrl2 = { "act", "ptype", "type", "usid", "backurl" };
                pageIndex2 = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex2 == 0)
                    pageIndex2 = 1;

                if (type == 2)
                    strWhere2 = "usid=" + usid + " and type=1";
                else
                    strWhere2 = "usid=" + usid + " and type=0";

                // 开始读取列表
                IList<BCW.farm.Model.NC_daoju_use> listFarm = new BCW.farm.BLL.NC_daoju_use().GetNC_daoju_uses(pageIndex2, pageSize2, strWhere2, out recordCount2);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_daoju_use n in listFarm)
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
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        BCW.farm.Model.NC_daoju hu = new BCW.farm.BLL.NC_daoju().GetNC_daoju(n.daoju_id);
                        try
                        {
                            builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");
                        }
                        catch
                        {
                            builder.Append("[图片出错!]<br/>");
                        }
                        builder.Append("道具名称：" + hu.name + "<br/>使用时间：" + n.updatetime + "");
                        if (n.daoju_id == 23)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(1) + "");
                        }
                        else if (n.daoju_id == 24)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(7) + "");
                        }
                        else if (n.daoju_id == 25)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(1) + "");
                        }
                        else if (n.daoju_id == 26)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(7) + "");
                        }
                        else if (n.daoju_id == 27)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(30) + "");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex2, pageSize2, recordCount2, Utils.getPageUrl(), pageValUrl2, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无数据!"));
                }
                #endregion
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl&amp;ptype=" + ptype + "&amp;type" + type + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //仓库物品修改
    private void cangku_gl_gaiPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl") + "\">仓库管理</a>&gt;信息修改");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]$", "1"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);

        if (ptype == 1)//果实
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok1")
            {
                int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
                int num1 = int.Parse(Utils.GetRequest("num", "all", 2, @"^[0-9]\d*$", "数量填写出错"));

                new BCW.farm.BLL.NC_GetCrop().Update_gs2(usid1, num1, id1);
                Utils.Success("更新果实数量", "更新果实数量成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
            }
            else
            {
                BCW.farm.Model.NC_GetCrop aa = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(id, usid);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("果实名称：" + aa.name + "");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "果实数量：,";
                string strName = "num";
                string strType = "num,";
                string strValu = "" + aa.num + "'";
                string strEmpt = "false";
                string strIdea = "/";
                string strOthe = "确定修改,farm.aspx?act=cangku_gl_gai&amp;ptype=1&amp;id=" + id + "&amp;usid=" + usid + "&amp;ac=ok1,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else if (ptype == 2)//种子
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok2")
            {
                int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
                int num1 = int.Parse(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "数量填写出错"));

                new BCW.farm.BLL.NC_mydaoju().Update_zz2(usid1, num1, id1);
                Utils.Success("更新种子数量", "更新种子数量成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
            }
            else
            {
                BCW.farm.Model.NC_mydaoju aa = new BCW.farm.BLL.NC_mydaoju().Getname_id(usid, id);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("种子名称：" + aa.name + "");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "种子数量：,";
                string strName = "num";
                string strType = "num,";
                string strValu = "" + aa.num + "'";
                string strEmpt = "false";
                string strIdea = "/";
                string strOthe = "确定修改,farm.aspx?act=cangku_gl_gai&amp;ptype=2&amp;id=" + id + "&amp;usid=" + usid + "&amp;ac=ok2,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else//道具
        {
            if (type == 1)
            {
                string ac = Utils.GetRequest("ac", "all", 1, "", "0");
                if (ac == "ok3")
                {
                    int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                    int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
                    int num1 = int.Parse(Utils.GetRequest("num", "all", 2, @"^[0-9]\d*$", "数量填写出错"));

                    new BCW.farm.BLL.NC_mydaoju().Update_hf2(usid1, num1, id1);
                    Utils.Success("更新道具数量", "更新道具数量成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
                }
                else
                {
                    BCW.farm.Model.NC_mydaoju aa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(usid, id, 0);
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("道具名称：" + aa.name + "");
                    builder.Append(Out.Tab("</div>", ""));
                    string strText = "道具数量：,";
                    string strName = "num";
                    string strType = "num,";
                    string strValu = "" + aa.num + "'";
                    string strEmpt = "false";
                    string strIdea = "/";
                    string strOthe = "确定修改,farm.aspx?act=cangku_gl_gai&amp;ptype=3&amp;id=" + id + "&amp;usid=" + usid + "&amp;ac=ok3,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //仓库物品删除
    private void cangku_gl_delPage()
    {
        //builder.Append(Out.Tab("<div class=\"title\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl") + "\">仓库管理</a>&gt;物品删除");
        //builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-3]$", "1"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);

        if (ptype == 1)//果实
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok1")
            {
                int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

                new BCW.farm.BLL.NC_GetCrop().Delete(id1, usid1);
                Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
            }
            else
            {
                Master.Title = "删除一条投注记录";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok1&amp;act=cangku_gl_del&amp;id=" + id + "&amp;usid=" + usid + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else if (ptype == 2)//种子
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok2")
            {
                int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

                new BCW.farm.BLL.NC_mydaoju().Delete(id1, usid1);
                Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
            }
            else
            {
                Master.Title = "删除一条投注记录";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok2&amp;act=cangku_gl_del&amp;id=" + id + "&amp;usid=" + usid + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else//道具
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok3")
            {
                int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
                int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

                new BCW.farm.BLL.NC_mydaoju().Delete2(id1, usid1);
                Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid1 + ""), "1");
            }
            else
            {
                Master.Title = "删除一条投注记录";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok3&amp;act=cangku_gl_del&amp;id=" + id + "&amp;usid=" + usid + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid + "") + "\">先留着吧..</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=cangku_gl_show&amp;ptype=" + ptype + "&amp;usid=" + usid + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //种子类型
    private string OutType_zz(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "普通土地";
        else if (Types == 2)
            pText = "红土地";
        else if (Types == 3)
            pText = "黑土地";
        else if (Types == 4)
            pText = "金土地";
        return pText;
    }

    //土地类型
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "(普通)土地";
        else if (Types == 2)
            pText = "(红)土地";
        else if (Types == 3)
            pText = "(黑)土地";
        else if (Types == 4)
            pText = "(金)土地";
        return pText;
    }

    //系统公告
    private void gonggaoPage()
    {
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        Master.Title = "" + GameName + "_系统公告";
        if (ptype == 0)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;系统公告");
            builder.Append(Out.Tab("</div>", "<br/>"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strOrder = "updatetime DESC";

            IList<BCW.farm.Model.NC_gonggao> listFarm = new BCW.farm.BLL.NC_gonggao().GetNC_gonggaos(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_gonggao n in listFarm)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<b>" + n.title + "</b><a href=\"" + Utils.getPage("farm.aspx?act=gonggao&amp;ptype=1&amp;id=" + n.ID + "") + "\">[删]</a>.");
                    builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=gonggao&amp;ptype=2&amp;id=" + n.ID + "") + "\">[改]</a><br/>");
                    builder.Append("" + n.contact + "<br/>[" + n.updatetime + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关公告.."));
            }
        }
        else if (ptype == 1)
        {
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "1"));
            string ac = Utils.GetRequest("ac", "all", 1, "", "0");
            if (ac == "ok1")
            {
                int id6 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
                new BCW.farm.BLL.NC_gonggao().Delete(id6);
                Utils.Success("删除公告", "删除公告成功,正在返回.", Utils.getUrl("farm.aspx?act=gonggao"), "1");
            }
            else
            {
                Master.Title = "删除一条投注记录";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("确定删除该记录吗");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok1&amp;act=gonggao&amp;id=" + id + "&amp;ptype=1") + "\">确定删除</a><br />");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gonggao") + "\">先留着吧..</a><br/>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else if (ptype == 2)
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=gonggao") + "\">系统公告</a>&gt;公告修改");
            builder.Append(Out.Tab("</div>", ""));

            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok")
            {
                string title = Utils.GetRequest("title", "all", 2, @"^[^\^]{1,100}$", "标题限1-100字内");
                string contact = Utils.GetRequest("contact", "all", 2, @"^[^\^]{1,500}$", "内容限1-500字内");
                string updatetime = Utils.GetRequest("updatetime", "all", 2, @"^[^\^]{1,200}$", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int type = Utils.ParseInt(Utils.GetRequest("type", "all", 2, @"^[0-9]\d*$", "类型填写出错"));
                int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID选择出错"));
                int ptype2 = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "类型填写出错"));
                BCW.farm.Model.NC_gonggao hh = new BCW.farm.Model.NC_gonggao();
                hh.ID = id;
                hh.title = title;
                hh.contact = contact;
                hh.updatetime = Convert.ToDateTime(updatetime);
                hh.type = type;
                new BCW.farm.BLL.NC_gonggao().Update(hh);

                Utils.Success("修改公告", "修改公告成功.", Utils.getUrl("farm.aspx?act=gonggao&amp;ptype=" + ptype2 + "&amp;id=" + id + ""), "1");
            }
            else
            {
                int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID选择出错"));
                BCW.farm.Model.NC_gonggao aa = new BCW.farm.BLL.NC_gonggao().GetNC_gonggao(id);
                string strText = "标题：/,内容：/,时间：/,类型：/,,,,,";
                string strName = "title,contact,updatetime,type,act,ptype,id,info";
                string strType = "big,big,text,num,hidden,hidden,hidden,hidden";
                string strValu = "" + aa.title + "'" + aa.contact + "'" + aa.updatetime + "'" + aa.type + "'gonggao'" + ptype + "'" + id + "'ok";
                string strEmpt = "false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=gonggao") + "\">系统公告</a>&gt;添加公告");
            builder.Append(Out.Tab("</div>", ""));

            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok1")
            {
                string title = Utils.GetRequest("title", "all", 2, @"^[^\^]{1,100}$", "标题限1-100字内");
                string contact = Utils.GetRequest("contact", "all", 2, @"^[^\^]{1,500}$", "内容限1-500字内");
                //string updatetime = Utils.GetRequest("updatetime", "all", 2, @"^[^\^]{1,200}$", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                int type = Utils.ParseInt(Utils.GetRequest("type", "all", 2, @"^[0-9]\d*$", "类型填写出错"));
                int ptype2 = Utils.ParseInt(Utils.GetRequest("ptype", "all", 2, @"^[0-9]\d*$", "类型填写出错"));
                BCW.farm.Model.NC_gonggao hh = new BCW.farm.Model.NC_gonggao();
                hh.title = title;
                hh.contact = contact;
                hh.updatetime = DateTime.Now;
                hh.type = type;
                new BCW.farm.BLL.NC_gonggao().Add(hh);

                //邵广林 20160525 增加添加公告发农场好友内线
                DataSet ds = new BCW.farm.BLL.NC_user().GetList("*", "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int usid = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                        new BCW.BLL.Guest().Add(1, usid, new BCW.BLL.User().GetUsName(usid), "亲爱的农友们," + GameName + "有新公告了.快去[url=/bbs/game/farm.aspx?act=gonggao]" + GameName + "[/url]看看吧.");
                    }
                }
                Utils.Success("添加公告", "添加公告成功.", Utils.getUrl("farm.aspx?act=gonggao"), "1");
            }
            else
            {
                string strText = "标题：/,内容：/,类型：/,,,,";
                string strName = "title,contact,type,act,ptype,info";
                string strType = "big,big,num,hidden,hidden,hidden";
                string strValu = "''0'gonggao'" + ptype + "'ok1";
                string strEmpt = "false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定添加,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>1、0为显示公告，1为不显示.<br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=gonggao&amp;ptype=3") + "\">添加公告</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //任务
    private void taskPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;活动与任务管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "_日常任务管理";
            builder.Append("日常任务" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=1") + "\">日常任务</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "" + GameName + "_主线任务管理";
            builder.Append("主线任务" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=2") + "\">主线任务</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "" + GameName + "_活动任务管理";
            builder.Append("活动任务" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=3") + "\">活动任务</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strOrder = "";
        string strWhere = string.Empty;
        if (ptype == 1)
        {
            strWhere = "task_type=0";
        }
        else if (ptype == 2)
        {
            strWhere = "task_type=1";
        }
        else
        {
            strWhere = "task_type=2";
        }

        IList<BCW.farm.Model.NC_task> listFarm = new BCW.farm.BLL.NC_task().GetNC_tasks(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_task n in listFarm)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getPage("farm.aspx?act=taskedit&amp;id=" + n.ID + "&amp;ptype=" + ptype + "") + "\">" + n.task_name + "</a>.");
                builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=taskdel&amp;id=" + n.ID + "&amp;ptype=" + ptype + "") + "\">[删]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关任务.."));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=taskadd") + "\">任务添加</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=tasklist") + "\">任务完成情况查看</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //删除任务
    private void taskdelPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "ok1")
        {
            int ptype1 = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
            int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

            new BCW.farm.BLL.NC_task().Delete(id1);
            Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=task&amp;ptype=" + ptype1 + ""), "1");
        }
        else
        {
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok1&amp;act=taskdel&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=" + ptype + "") + "\">先留着吧..</a><br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
    }

    //任务修改
    private void taskeditPage()
    {
        Master.Title = "" + GameName + "活动任务修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">任务与活动管理</a>&gt;修改");
        builder.Append(Out.Tab("</div>", ""));

        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string task_name = Utils.GetRequest("task_name", "all", 2, @"^[^\^]{1,100}$", "任务名称限1-100字内");
            string task_contact = Utils.GetRequest("task_contact", "all", 2, @"^[^\^]{1,500}$", "任务内容限1-500字内");
            string task_jiangli = Utils.GetRequest("task_jiangli", "all", 2, @"^[^\^]{1,500}$", "任务奖励限1-500字内");
            int task_grade = Utils.ParseInt(Utils.GetRequest("task_grade", "all", 2, @"^[0-9]\d*$", "等级要求填写出错"));
            int task_num = Utils.ParseInt(Utils.GetRequest("task_num", "all", 2, @"^[0-9]\d*$", "完成的数量填写出错"));
            string task_time = Utils.GetRequest("task_time", "all", 2, @"^[^\^]{1,200}$", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            int task_type = Utils.ParseInt(Utils.GetRequest("task_type", "all", 2, @"^[0-9]\d*$", "任务类型填写出错"));
            int xiajia = Utils.ParseInt(Utils.GetRequest("xiajia", "all", 2, @"^[0-1]\d*$", "任务状态填写出错"));
            int id2 = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "ID选择出错"));

            BCW.farm.Model.NC_task hh = new BCW.farm.Model.NC_task();
            hh.ID = id2;
            hh.task_name = task_name;
            hh.task_contact = task_contact;
            hh.task_time = Convert.ToDateTime(task_time);
            hh.task_jiangli = task_jiangli;
            hh.task_type = task_type;
            hh.task_num = task_num;
            hh.task_grade = task_grade;
            hh.task_id = id2;
            hh.xiajia = xiajia;
            new BCW.farm.BLL.NC_task().Update(hh);

            Utils.Success("修改任务", "修改任务成功.", Utils.getUrl("farm.aspx?act=taskedit&amp;id=" + id2 + ""), "1");
        }
        else
        {

            BCW.farm.Model.NC_task aa = new BCW.farm.BLL.NC_task().GetNC_task2(id);
            string strText = "任务名称：/,任务内容：/,任务奖励：/,等级要求：/,完成的数量：/,任务完成时间：/,任务类型：/,任务状态：(0上线1下架)/,,,,";
            string strName = "task_name,task_contact,task_jiangli,task_grade,task_num,task_time,task_type,xiajia,act,id,info";
            string strType = "text,big,big,num,num,text,num,num,hidden,hidden,hidden";
            string strValu = "" + aa.task_name + "'" + aa.task_contact + "'" + aa.task_jiangli + "'" + aa.task_grade + "'" + aa.task_num + "'" + aa.task_time + "'" + aa.task_type + "'" + aa.xiajia + "'taskedit'" + id + "'ok";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=task&amp;ptype=" + ptype + "") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //任务增加
    private void taskaddPage()
    {
        Master.Title = "" + GameName + "活动任务增加";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">任务与活动管理</a>&gt;增加");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            string task_name = Utils.GetRequest("task_name", "all", 2, @"^[^\^]{1,100}$", "任务名称限1-100字内");
            string task_contact = Utils.GetRequest("task_contact", "all", 2, @"^[^\^]{1,500}$", "任务内容限1-500字内");
            string task_jiangli = Utils.GetRequest("task_jiangli", "all", 2, @"^[^\^]{1,500}$", "任务奖励限1-500字内");
            int task_grade = Utils.ParseInt(Utils.GetRequest("task_grade", "all", 2, @"^[0-9]\d*$", "等级要求填写出错"));
            int task_num = Utils.ParseInt(Utils.GetRequest("task_num", "all", 2, @"^[0-9]\d*$", "完成的数量填写出错"));
            string task_time = Utils.GetRequest("task_time", "all", 2, @"^[^\^]{1,200}$", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            int task_type = Utils.ParseInt(Utils.GetRequest("task_type", "all", 2, @"^[0-9]\d*$", "任务类型填写出错"));
            int task_id = Utils.ParseInt(Utils.GetRequest("task_id", "all", 2, @"^[1-9]\d*$", "ID选择出错"));

            BCW.farm.Model.NC_task hh = new BCW.farm.Model.NC_task();
            hh.task_name = task_name;
            hh.task_contact = task_contact;
            hh.task_time = Convert.ToDateTime(task_time);
            hh.task_jiangli = task_jiangli;
            hh.task_type = task_type;
            hh.task_num = task_num;
            hh.task_grade = task_grade;
            hh.task_id = task_id;
            new BCW.farm.BLL.NC_task().Add(hh);

            Utils.Success("增加任务", "增加任务成功.", Utils.getUrl("farm.aspx?act=task"), "1");
        }
        else
        {
            int aa = 0;
            try
            {
                DataSet hio = new BCW.farm.BLL.NC_task().GetList("top(1)task_id", "task_id>0 ORDER BY ID desc");
                aa = int.Parse(hio.Tables[0].Rows[0]["task_id"].ToString()) + 1;
            }
            catch
            {
                aa = 1;
            }
            string strText = "任务名称：/,任务内容：/,任务奖励：/,等级要求：/,完成的数量：/,任务完成时间：/,任务类型：/,,,,";
            string strName = "task_name,task_contact,task_jiangli,task_grade,task_num,task_time,task_type,act,task_id,info";
            string strType = "text,big,big,num,num,text,num,hidden,hidden,hidden";
            string strValu = "'''''" + DateTime.Now + "''taskadd'" + aa + "'ok";
            string strEmpt = "false,false,false,false,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定增加,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>1、0为日常任务，1为主线任务，2为活动任务.<br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=task") + "\">返回上级</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //任务完成情况查看
    private void tasklistPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=task") + "\">活动与任务管理</a>&gt;完成情况查看");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int a = int.Parse(Utils.GetRequest("a", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            Master.Title = "" + GameName + "任务查看_未完成";
            builder.Append("未完成" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tasklist&amp;ptype=1&amp;uid=" + uid + "") + "\">未完成</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "" + GameName + "任务查看_已完成";
            builder.Append("已完成" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tasklist&amp;ptype=2&amp;uid=" + uid + "") + "\">已完成</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", ""));
            for (int i = 0; i <= new BCW.farm.BLL.NC_task().GetMaxId() - 1; i++)
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=tasklist&amp;ptype=2&amp;uid=" + uid + "&amp;a=" + i + "") + "\">" + i + "</a>" + " | ");
            }
            builder.Append(Out.Tab("</div>", "<br/>"));
        }

        int pageIndex;
        int recordCount;
        string[] pageValUrl = { "act", "ptype", "a", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = string.Empty;

        #region 查询
        if (ptype == 1)
        {
            if (uid != 0)
            {
                strWhere = "usid=" + uid + " and type=0";
            }
            else
            {
                strWhere = "type=0";
            }
        }
        else
        {
            if (a == 0)
            {
                if (uid != 0)
                {
                    strWhere = "usid=" + uid + " and type=1";
                }
                else
                {
                    strWhere = "type=1";
                }
            }
            else
            {
                if (uid != 0)
                {
                    strWhere = "usid=" + uid + " and type=1 and task_id=" + a + "";
                }
                else
                {
                    strWhere = "type=1 and task_id=" + a + "";
                }
            }
        }
        #endregion

        IList<BCW.farm.Model.NC_tasklist> listFarm = new BCW.farm.BLL.NC_tasklist().GetNC_tasklists(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_tasklist n in listFarm)
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
                string mename = new BCW.BLL.User().GetUsName(n.usid);
                BCW.farm.Model.NC_task ai = new BCW.farm.BLL.NC_task().GetNC_task(n.task_id);
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>:" + ai.task_name + "");
                if (ptype == 1)
                {
                    builder.Append(".开始时间[" + n.task_oktime + "]");
                }
                else
                {
                    #region
                    if (a == 11)
                    {
                        DataSet dd = new BCW.farm.BLL.NC_tasklist().GetList1("*", "a INNER JOIN tb_NC_messagelog b ON a.usid=b.UsId WHERE a.task_id=11 AND a.usid=" + n.usid + " AND b.type=1 AND task_type=2 AND AcText LIKE '%VIP特权%' ");
                        try
                        {
                            builder.Append("：" + dd.Tables[0].Rows[0]["AcText"].ToString().Substring(14) + "[" + n.task_oktime + "]");
                        }
                        catch
                        {
                            builder.Append("暂无");
                        }
                    }
                    else if (a == 12)
                    {
                        DataSet dd = new BCW.farm.BLL.NC_tasklist().GetList1("*", "a INNER JOIN tb_NC_messagelog b ON a.usid=b.UsId WHERE a.task_id=12 AND a.usid=" + n.usid + " AND b.type=1 AND task_type=2 AND AcText LIKE '%国庆大礼包%' AND CONVERT(varchar(100), b.AddTime, 23) = '" + n.task_oktime.ToString("yyyy-MM-dd") + "'");
                        try
                        {
                            builder.Append("：" + dd.Tables[0].Rows[0]["AcText"].ToString().Substring(68) + "[" + n.task_oktime + "]");
                        }
                        catch
                        {
                            builder.Append("暂无");
                        }
                    }
                    else if (a == 13)
                    {
                        DataSet dd = new BCW.farm.BLL.NC_tasklist().GetList1("*", "a INNER JOIN tb_NC_messagelog b ON a.usid=b.UsId WHERE a.task_id=13 AND a.usid=" + n.usid + " AND b.type=1 AND task_type=2 AND AcText LIKE '%消费回馈%'");// 
                        try
                        {
                            builder.Append("：" + dd.Tables[0].Rows[0]["AcText"].ToString().Substring(67) + "[" + n.task_time + "]");
                        }
                        catch
                        {
                            builder.Append("暂无");
                        }
                    }
                    else
                    {
                        builder.Append(".开始时间[" + (n.task_time) + "].");
                        builder.Append("完成时间[" + (n.task_oktime) + "]");
                    }
                    #endregion
                }
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 1));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关任务.."));
        }

        string strText = "输入用户ID:/,,,";
        string strName = "uid,ptype,backurl,act";
        string strType = "num,hidden,hidden,hidden";
        string strValu = string.Empty;
        if (uid == 0)
        {
            strValu = "'" + ptype + "'" + Utils.getPage(0) + "'tasklist";
        }
        else
        {
            strValu = "" + uid + "'" + ptype + "'" + Utils.getPage(0) + "'tasklist";
        }
        string strEmpt = "true,true,false,false";
        string strIdea = "/";
        string strOthe = "搜ID,farm.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=taskadd") + "\">任务添加</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=task") + "\">返回上级</a><br/>");
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //留言管理
    private void liuyan_glPage()
    {
        Master.Title = "" + GameName + "_留言管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;农场留言管理");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (uid > 0)
            strWhere = "MID=" + uid + " and type=1001";
        else
            strWhere = "type=1001";

        // 开始读取列表
        IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex, pageSize, strWhere, out recordCount);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Mebook n in listMebook)
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
                //builder.AppendFormat("<a href=\"" + Utils.getUrl("farm.aspx?act=del_liuyan&amp;uid=" + uid + "&amp;id={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>{1}.{2}:{3}({4})", n.ID, (pageIndex - 1) * pageSize + k, n.MName, n.MContent, DT.FormatDate(n.AddTime, 1));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=del_liuyan&amp;uid=" + uid + "&amp;id=" + n.ID + "") + "\">[删]</a>");
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "、");
                builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.MID + "") + "\">" + n.MName + "</a>对<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");
                builder.Append("说：" + n.MContent + "[" + DT.FormatDate(n.AddTime, 1) + "]");
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
        string strValu = string.Empty;
        if (uid == 0)
        {
            strValu = "'" + Utils.getPage(0) + "";
        }
        else
        {
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        }
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜农场留言,farm.aspx?act=liuyan_gl,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //留言删除
    private void del_liuyanPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 2, @"^[0-9]\d*$", "ID错误"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        if (info != "ok")
        {
            Master.Title = "删除留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此留言记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?info=ok&amp;act=del_liuyan&amp;uid=" + uid + "&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan_gl&amp;uid=" + uid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            int uid2 = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            if (!new BCW.BLL.Mebook().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            //删除
            new BCW.BLL.Mebook().Delete(id);
            Utils.Success("删除成功", "删除留言成功..", Utils.getPage("farm.aspx?act=liuyan_gl&amp;uid=" + uid2 + ""), "1");
        }
    }


    //合成管理
    private void hecheng_glPage()
    {
        Master.Title = "" + GameName + "_合成管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;合成管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string[] pageValUrl = { "uid", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        DataSet ds;
        if (uid > 0)
            ds = new BCW.farm.BLL.NC_hecheng().GetList("UsID,count(GiftId) as aa,sum(all_num) as bb,sum(num)as cc", "usid=" + uid + " and all_num>0 GROUP BY UsID ORDER BY aa DESC,bb ASC");
        else
            ds = new BCW.farm.BLL.NC_hecheng().GetList("UsID,count(GiftId) as aa,sum(all_num) as bb,sum(num)AS cc", "all_num>0 GROUP BY UsID ORDER BY aa DESC,bb ASC");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
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
            for (int i = 0; i < skt; i++)
            {
                int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["usid"].ToString());
                int aa = int.Parse(ds.Tables[0].Rows[koo + i]["aa"].ToString());
                int bb = int.Parse(ds.Tables[0].Rows[koo + i]["bb"].ToString());
                int cc = int.Parse(ds.Tables[0].Rows[koo + i]["cc"].ToString());
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + UsID + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>合成有" + aa + "种,总合成" + bb + "个,剩下<a href=\"" + Utils.getPage("farm.aspx?act=hechenglist&amp;usid=" + UsID + "") + "\">" + cc + "个</a>.");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, 1, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
        {
            strValu = "'" + Utils.getPage(0) + "";
        }
        else
        {
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        }
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜用户ID,farm.aspx?act=hecheng_gl,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //合成详细
    private void hechenglistPage()
    {
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));

        Master.Title = "" + GameName + "-" + new BCW.BLL.User().GetUsName(uid) + "的合成详细种类";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=hecheng_gl") + "\">合成管理</a>&gt;" + new BCW.BLL.User().GetUsName(uid) + "的详细合成种类");
        builder.Append(Out.Tab("</div>", "<br/>"));



        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "usid", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        strWhere = " UsID=" + uid + " and num>0";
        string strOrder = "GiftId asc";
        IList<BCW.farm.Model.NC_hecheng> listFarm = new BCW.farm.BLL.NC_hecheng().GetNC_hechengs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_hecheng n in listFarm)
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
                string mename = new BCW.BLL.User().GetUsName(n.UsID);//用户姓名
                try
                {
                    if (n.PrevPic != "")
                    {
                        builder.Append("<img src=\"" + n.PrevPic + "\" alt=\"load\"/><br/>");
                    }
                }
                catch
                {
                    builder.Append("[图片出错!]<br/>");
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + ".[" + n.num + "个]");
                builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=hecheng_gai&amp;id=" + n.GiftId + "&amp;usid=" + n.UsID + "") + "\">[改]</a>");
                builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=hecheng_del&amp;id=" + n.ID + "&amp;usid=" + n.UsID + "") + "\">[删]</a>");

                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "暂无数据!"));
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //合成修改
    private void hecheng_gaiPage()
    {
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));

        Master.Title = "" + GameName + "-" + new BCW.BLL.User().GetUsName(uid) + "的数量修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=hecheng_gl") + "\">合成管理</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=hechenglist&amp;usid=" + uid + "") + "\">" + new BCW.BLL.User().GetUsName(uid) + "的详细合成种类</a>&gt;数量修改");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "ok1")
        {
            int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));
            int num1 = int.Parse(Utils.GetRequest("num", "all", 2, @"^[0-9]\d*$", "数量填写出错"));

            new BCW.farm.BLL.NC_hecheng().update_ID("num=" + num1 + "", "usid=" + usid1 + " and giftid=" + id1 + "");
            Utils.Success("更新果实数量", "更新果实数量成功.正在返回.", Utils.getUrl("farm.aspx?act=hechenglist&amp;usid=" + usid1 + ""), "1");
        }
        else
        {
            BCW.farm.Model.NC_hecheng aa = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(id, uid);
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("果实名称：" + aa.Title + "");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "果实数量：,";
            string strName = "num";
            string strType = "num,";
            string strValu = "" + aa.num + "'";
            string strEmpt = "false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx?act=hecheng_gai&amp;usid=" + uid + "&amp;id=" + id + "&amp;ac=ok1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //合成删除
    private void hecheng_delPage()
    {
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "ok1")
        {
            int usid1 = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

            new BCW.farm.BLL.NC_hecheng().Delete(id1);
            Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=hechenglist&amp;usid=" + usid1 + ""), "1");
        }
        else
        {
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该记录吗？删除并不会通知会员？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok1&amp;act=hecheng_del&amp;id=" + id + "&amp;usid=" + uid + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=hechenglist&amp;usid=" + uid + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //惩罚奴隶语句管理
    private void nuliyuyan_glPage()
    {
        Master.Title = "" + GameName + "_语言管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;语言管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-7]$", "1"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">惩罚奴隶</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuliyuyan_gl&amp;ptype=1") + "\">惩罚奴隶</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">安抚奴隶</h>" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuliyuyan_gl&amp;ptype=2") + "\">安抚奴隶</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
        {
            strWhere = "type=0";
        }
        else
        {
            strWhere = "type=1";
        }

        IList<BCW.farm.Model.NC_slavelist> listMebook = new BCW.farm.BLL.NC_slavelist().GetNC_slavelists(pageIndex, pageSize, strWhere, out recordCount);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_slavelist n in listMebook)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.contact + ".[奖" + n.inGold + "罚" + n.outGold + "].");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nlyy_edit&amp;id=" + n.ID + "") + "\">[改]</a>.");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nlyy_del&amp;id=" + n.ID + "") + "\">[删]</a>");
                builder.Append("");
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

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //惩罚奴隶语句修改
    private void nlyy_editPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));

        Master.Title = "" + GameName + "_语言管理.语言修改";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getPage("farm.aspx?act=nuliyuyan_gl") + "\">语言管理</a>&gt;语句修改");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "ok1")
        {
            int inGold = int.Parse(Utils.GetRequest("inGold", "all", 2, @"^[0-9]\d*$", "惩罚金币填写出错"));
            int outGold = int.Parse(Utils.GetRequest("outGold", "all", 2, @"^[0-9]\d*$", "安抚金币填写出错"));
            int type = int.Parse(Utils.GetRequest("type", "all", 2, @"^[0-1]\d*$", "类型填写出错"));
            string contact = Utils.GetRequest("contact", "all", 2, @"^[^\^]{1,30}$", "语句限1-30字内");
            int id1 = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));
            new BCW.farm.BLL.NC_slavelist().update_yuju("contact='" + contact + "',inGold='" + inGold + "',outGold='" + outGold + "',type='" + type + "'", "id=" + id1 + "");
            Utils.Success("更新语句", "更新语句成功.正在返回.", Utils.getUrl("farm.aspx?act=nuliyuyan_gl"), "1");
        }
        else
        {
            BCW.farm.Model.NC_slavelist aa = new BCW.farm.BLL.NC_slavelist().GetNC_slavelist(id);
            string strText = "奴隶语句：/,惩罚金币：/,安抚金币：/,类型：(0为惩罚1为安抚)/";
            string strName = "contact,inGold,outGold,type";
            string strType = "textarea,num,num,num,";
            string strValu = "" + aa.contact + "'" + aa.inGold + "'" + aa.outGold + "'" + aa.type + "'";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx?act=nlyy_edit&amp;id=" + id + "&amp;ac=ok1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //惩罚奴隶语句删除
    private void nlyy_delPage()
    {
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "0"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "ok1")
        {
            int id1 = int.Parse(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "id选择出错"));

            new BCW.farm.BLL.NC_slavelist().Delete(id1);
            Utils.Success("删除数据", "删除数据成功.正在返回.", Utils.getUrl("farm.aspx?act=nuliyuyan_gl"), "1");
        }
        else
        {
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除该语句吗？");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?ac=ok1&amp;act=nlyy_del&amp;id=" + id + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nuliyuyan_gl") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //农场寄语管理
    private void jiyuPage()
    {
        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "edit")
        {
            Master.Title = "" + GameName + "_用户寄语修改";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=paihang") + "\">用户排行</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=jiyu") + "\">用户寄语查看</a>&gt;用户寄语修改");
            builder.Append(Out.Tab("</div>", ""));

            int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            string jiyu = new BCW.farm.BLL.NC_user().Get_jiyu(usid);
            string strText = "" + new BCW.BLL.User().GetUsName(usid) + "(" + usid + ")的寄语修改:(限30字)/,,,";
            string strName = "jiyu,usid";
            string strType = "textarea,hidden";
            string strValu = "" + jiyu + "'" + usid + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "确定修改,farm.aspx?act=jiyu&amp;ac=edit1,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx?act=jiyu") + "\">返回上级</a><br/>");
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else if (ac == "edit1")
        {
            int usid = int.Parse(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "usid选择出错"));
            string jiyu = Utils.GetRequest("jiyu", "all", 2, @"^[^\^]{1,30}$", "寄语限1-30字内");
            new BCW.farm.BLL.NC_user().update_zd("jiyu='" + jiyu + "'", "usid=" + usid + "");
            Utils.Success("寄语设置", "寄语设置成功.", Utils.getUrl("farm.aspx?act=jiyu"), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_寄语管理";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=paihang") + "\">用户排行</a>&gt;寄语管理");
            builder.Append(Out.Tab("</div>", "<br/>"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "usid", "backurl" };
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            string strOrder = "";

            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            if (uid > 0)
                strWhere = "UsID=" + uid + " and jiyu!=''";
            else
                strWhere = "jiyu!=''";

            IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listFarm)
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
                    string mename = new BCW.BLL.User().GetUsName(n.usid);//用户姓名
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei&amp;usid=" + n.usid + "") + "\">" + mename + "</a>：" + n.jiyu + "");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=jiyu&amp;usid=" + n.usid + "&amp;ac=edit") + "\">[修改]</a>");
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

            builder.Append(Out.Tab("", Out.Hr()));
            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=jiyu,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }

    //消费管理
    private void xiaofei_glPage()
    {
        Master.Title = "" + GameName + "_消费管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;消费管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">会员消费</h>" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_gl&amp;ptype=1&amp;usid=" + uid + "") + "\">会员消费</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">会员获得</h>" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xiaofei_gl&amp;ptype=2&amp;usid=" + uid + "") + "\">会员获得</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (uid > 0)
            strWhere = "AcText LIKE '%您在农场%' AND UsId=" + uid + " ";
        else
            strWhere = "AcText LIKE '%您在农场%'";

        if (ptype == 1)
            strWhere += "and AcGold<0";
        else
            strWhere += "and AcGold>0";

        IList<BCW.Model.Goldlog> listMebook = new BCW.BLL.Goldlog().GetGoldlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Goldlog n in listMebook)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsId + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsId) + "</a>(" + n.UsId + ")：");
                if (ptype == 1)
                    builder.Append("消费" + n.AcGold + "." + n.AcText.Replace("您在农场", "在") + ".[" + DT.FormatDate(n.AddTime, 1) + "]");
                else
                    builder.Append("" + n.AcText.Replace("您在农场", "") + "[" + DT.FormatDate(n.AddTime, 1) + "]");
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

        builder.Append(Out.Tab("", Out.Hr()));
        string strText = "输入用户ID(为空搜索全部):/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
            strValu = "'" + Utils.getPage(0) + "";
        else
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜一搜,farm.aspx?act=xiaofei_gl&amp;ptype=" + ptype + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    //纸条管理
    private void zhitiao_glPage()
    {
        Master.Title = "" + GameName + "_纸条管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;纸条管理");
        builder.Append(Out.Tab("</div>", "<br/>"));

        string ac = Utils.GetRequest("ac", "all", 1, "", "0");
        if (ac == "del")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "1"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            Master.Title = "删除一条投注记录";
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("确定删除该记录吗?");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao_gl&amp;id=" + id + "&amp;ac=delok&amp;usid=" + uid + "&amp;ptype=" + ptype + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao_gl&amp;usid=" + uid + "&amp;ptype=" + ptype + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else if (ac == "delok")
        {
            int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "1"));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            new BCW.farm.BLL.NC_zhitiao().Delete(id);
            Utils.Success("删除纸条", "删除纸条成功.", Utils.getUrl("farm.aspx?act=zhitiao_gl&amp;usid=" + uid + "&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            if (ptype == 1)
            {
                builder.Append("<h style=\"color:red\">投诉</h>" + "" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao_gl&amp;ptype=1&amp;usid=" + uid + "") + "\">投诉</a>" + "|");
            }
            if (ptype == 2)
            {
                builder.Append("<h style=\"color:red\">建议</h>" + "" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao_gl&amp;ptype=2&amp;usid=" + uid + "") + "\">建议</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere = "";
            string[] pageValUrl = { "ptype", "act", "ac", "usid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (uid > 0)
                strWhere = "UsID=" + uid + " and ";
            else
                strWhere = "";

            if (ptype == 1)//0投诉1建议
                strWhere += "type=0";
            else
                strWhere += "type=1";

            IList<BCW.farm.Model.NC_zhitiao> listMebook = new BCW.farm.BLL.NC_zhitiao().GetNC_zhitiaos(pageIndex, pageSize, strWhere, out recordCount);
            if (listMebook.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_zhitiao n in listMebook)
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
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>(" + n.UsID + ")");
                    if (ptype == 1)
                        builder.Append("投诉：" + n.contact + ".[" + DT.FormatDate(n.AddTime, 1) + "]");
                    else
                        builder.Append("建议：" + n.contact + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    builder.Append(".<a href=\"" + Utils.getPage("farm.aspx?act=zhitiao_gl&amp;id=" + n.ID + "&amp;ac=del&amp;usid=" + uid + "&amp;ptype=" + ptype + "") + "\">[删]</a>");
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

            builder.Append(Out.Tab("", Out.Hr()));
            string strText = "输入用户ID(为空搜索全部):/,";
            string strName = "usid,backurl";
            string strType = "num,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false";
            string strIdea = "/";
            string strOthe = "搜一搜,farm.aspx?act=zhitiao_gl&amp;ptype=" + ptype + "&amp;ac=0,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getPage("farm.aspx") + "\">返回首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }



}
