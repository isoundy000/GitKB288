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
using BCW.Files;

/// <summary>
/// 蒙宗将 2016/5/9
/// 蒙宗将 2016/5/10 去掉道具价值的添加与显示 添加管理链接
/// 蒙宗将 2016/5/12 增加奖品库存设置
/// 蒙宗将 2016/5/13 奖品下架显示，排行榜ID显示
/// 蒙宗将 201605/14 兑换道具管理 
/// 蒙宗将 2016/05/16 修改奖品管理立即编辑功能
/// 蒙宗将 2016/5/20  增加道具自动获取图片
/// 蒙宗将 2016/8/31  增加权值与概率的计算查询
/// 蒙宗将 2016/9/10  增加聊吧发言，游戏名称管理修复统一
/// 蒙宗将 2016/09/13 增加消费点值top
/// 蒙宗将 2016/09/18 奖品显示（list修改）
/// mzj 20160919 
/// 蒙宗将 2016/9/21  抽奖与兑换分开
/// 蒙宗将 2016/9/29 修改几等奖显示
/// 蒙宗将 2016/10/07 后台派送信息显示等奖出错
/// 蒙宗将 2016/11/15 增加显示快递信息，修复修改已送达
/// </summary>

public partial class Manage_game_draw : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/draw.xml";
    protected string xmlPath2 = "/Controls/gamezdid.xml";
    protected string GameName = Convert.ToString(ub.GetSub("drawName", "/Controls/draw.xml"));
    protected string drawJifen = Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml"));//兑换的积分名字

    protected string strText = string.Empty;
    protected string strName = string.Empty;
    protected string strType = string.Empty;
    protected string strValu = string.Empty;
    protected string strEmpt = string.Empty;
    protected string strIdea = string.Empty;
    protected string strOthe = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "reset":
                ResetPage();
                break;
            case "stat":
                StatPage();
                break;
            case "peizhi":
                PeizhiPage();
                break;
            case "jifenpeizhi":
                JifenPeizhiPage();//兑换积分配置
                break;
            case "jfscpz":
                JfscpzPage();//积分生成设定值
                break;
            case "scpeizhi":
                ScpeiPage();//积分生成来源管理
                break;
            case "jifenjiangli":
                JifenjiangliPage();//对用户进行积分奖励
                break;
            case "jfjl":
                jfjlPage();//确定对用户进行积分奖励
                break;
            case "drawset":
                DrawSetPage();//中奖概率与权值配置
                break;
            case "drawsett":
                DrawSettPage();//权值设置与计算概率
                break;
            case "drawlist":
                DrawListPage();//积分历史表
                break;
            case "conlist":
                ConlistPage();//积分消费
                break;
            case "setceshi":
                SetStatueCeshi();//内测设置
                break;
            case "case":
                CasePage();//排行查看
                break;
            case "top"://排行榜单
                TopPage();
                break;
            case "paisong":
                PaisongPage();//奖品派送
                break;
            case "paisongxinxi":
                PaisongXXPage();//奖品派送信息
                break;
            case "xinxifankui":
                FankuiPage();//信息反馈
                break;
            case "jiangpinbianji":
                JPbjPage();//添加奖品
                break;
            case "addgoods1":
                AddGoods1Page();//添加酷币
                break;
            case "addgoods2":
                AddGoods2Page();//添加实物
                break;
            case "addgoods3":
                AddGoods3Page();//添加道具 
                break;
            case "addgoods4":
                AddGoods4Page();//添加属性
                break;
            case "addgoods5":
                AddGoods5Page();//添加任意值酷币
                break;
            case "addjiangji":
                AddjiangjiPage();//奖级设定
                break;
            case "addjiangjikg":
                AddjiangjikgPage();//奖级设定开关
                break;
            case "addimg":
                AddImgPage();//添加图片
                break;
            case "jiangpin":
                JiangpinPage();//奖品管理
                break;
            case "guanli":
                GuanliPage();//管理
                break;
            case "xinlun":
                XinlunPage();//清空奖箱，重新一轮奖品
                break;
            case "update"://修改商品信息
                UpdateGoodsAll();
                break;
            case "del":
                DelPage();//删除
                break;
            case "chi":
                ChiPage();//奖池管理
                break;
            default:
                ReloadPage();
                break;
        }
    }
    //首页
    private void ReloadPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;" + Title + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int id = int.Parse(Utils.GetRequest("id", "post", 1, @"^[0-9]\d*$", "0"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-5]$", "1"));
        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = string.Empty;
        string strOrder = "";
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【奖品管理】");
        builder.Append("<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 4)
            builder.Append("<b style=\"color:black\">总" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=0&amp;ptype=4&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">总</a>" + "|");
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">进行中" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=0&amp;ptype=1&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">进行中</a>" + "|");
        if (ptype == 5)
            builder.Append("<b style=\"color:black\">已结束" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=0&amp;ptype=5&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">已结束</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">已下架" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=0&amp;ptype=2&amp;id=" + ptype + "&amp;backurl=" + Utils.getPage(0) + "") + "\">已下架</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //查询条件
        if (ptype == 1)
        {
            strWhere = "statue='0'";
            strOrder = "GoodsCounts Desc";
        }
        if (ptype == 2)
        {
            strWhere = "statue='5'";
            strOrder = "GoodsCounts Desc";
        }
        if (ptype == 4)
        {
            strWhere = "";
            strOrder = "GoodsCounts Desc";
        }
        if (ptype == 5)
        {
            strWhere = "statue='1'";
            strOrder = "OverTime Desc";
        }

        if (uid > 0)
        {
            if (ac == "搜中奖")
                strWhere += "usid=" + uid + "";
            else if (ac == "搜参与")
                strWhere += "reid=" + uid + "";
        }
        string[] pageValUrl = { "ac", "act", "ptype", "uid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;


        // 开始读取列表
        IList<BCW.Draw.Model.DrawBox> listbox = new BCW.Draw.BLL.DrawBox().GetDrawBoxs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listbox.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawBox n in listbox)
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
                id = n.GoodsCounts;

                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=guanli&amp;backurl=" + Utils.PostPage(1) + "") + "\">[管理]&gt;</a>");
                builder.Append("" + id + ".<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>");
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

        string strText = "输入奖品编号:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜奖品,draw.aspx?act=update&amp;u=" + Utils.getstrU() + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));

        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("draw.aspx?act=setceshi") + "\">内测配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=top&amp;backurl=" + Utils.PostPage(1) + "") + "\">排行榜单</a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong") + "\">奖品派送</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=drawlist") + "\">" + drawJifen + "历史</a><br />");
        builder.Append("<a href =\"" + Utils.getUrl("draw.aspx?act=conlist") + "\">" + drawJifen + "消费</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=stat") + "\">赢利分析</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=reset") + "\">重置游戏</a><br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //游戏配置
    private void PeizhiPage()
    {

        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;游戏配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=drawset&amp;backurl=" + Utils.getPage(1) + "") + "\">中奖概率配置</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;backurl=" + Utils.getPage(1) + "") + "\">" + drawJifen + "生成设定</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=scpeizhi&amp;backurl=" + Utils.getPage(1) + "") + "\">" + drawJifen + "生成管理</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jifenpeizhi&amp;backurl=" + Utils.getPage(1) + "") + "\">换币配置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "游戏名称限1-20字内");
            string Jifen = Utils.GetRequest("Jifen", "post", 2, @"^[0-9]\d*$", "抽奖值输入错误");
            string JifenName = Utils.GetRequest("JifenName", "post", 2, @"^[^\^]{1,20}$", "抽奖值名称限1-20字内");
            string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,2000}$", "口号限2000字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string Head = Utils.GetRequest("Head", "post", 3, @"^[\s\S]{1,2000}$", "头部Ubb限2000字内");
            string Foot = Utils.GetRequest("Foot", "post", 3, @"^[\s\S]{1,2000}$", "底部Ubb限2000字内");
            string Rule = Utils.GetRequest("Rule", "post", 3, @"^[\s\S]{1,8000}$", "规则限8000字内");
            string Xiangqing = Utils.GetRequest("Xiangqing", "post", 3, @"^[\s\S]{1,8000}$", "活动详情限8000字内");
            string shuoming = Utils.GetRequest("shuoming", "post", 3, @"^[\s\S]{1,8000}$", "兑奖说明限8000字内");
            string chuzhi = Utils.GetRequest("chuzhi", "post", 2, @"^[0-9]\d*$", "首次游戏获得抽奖值输入错误");
            string drawOpenOrClose = Utils.GetRequest("drawOpenOrClose", "post", 2, @"^[0-1]$", "状态选择错误");
            string huitie = Utils.GetRequest("huitie", "post", 2, @"^[0-1]$", "抽奖活动选择出错");

            xml.dss["drawName"] = Name;
            xml.dss["jifen"] = Jifen;
            xml.dss["drawJifen"] = JifenName;
            xml.dss["drawNotes"] = Notes;
            xml.dss["drawLogo"] = Logo;
            xml.dss["drawStatus"] = Status;
            xml.dss["drawHead"] = Head;
            xml.dss["drawFoot"] = Foot;
            xml.dss["drawRule"] = Rule;
            xml.dss["drawXiangqing"] = Xiangqing;
            xml.dss["drawshuoming"] = shuoming;
            xml.dss["chuzhi"] = chuzhi;
            xml.dss["drawOpenOrClose"] = drawOpenOrClose;//开放选择（0开启，1关闭）
            xml.dss["huitie"] = huitie;//(0开启，1关闭)

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=peizhi&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            string strText = "游戏名称:/,每次抽奖值数:/,抽奖值名称:/,玩家初值进入游戏赠送值:/,抽奖值生成开关:/,游戏口号(可留空):/,游戏Logo(可留空):/,游戏状态:/,抽奖活动开放与关闭:/,头部Ubb:/,底部Ubb:/,游戏规则:/,活动详情:/,兑奖说明:/,";
            string strName = "Name,Jifen,JifenName,chuzhi,drawOpenOrClose,Notes,Logo,Status,huitie,Head,Foot,Rule,Xiangqing,shuoming,backurl";
            string strType = "text,num,text,num,select,text,text,select,select,text,text,textarea,textarea,textarea,hidden";
            string strValu = "" + xml.dss["drawName"] + "'" + xml.dss["jifen"] + "'" + xml.dss["drawJifen"] + "'" + xml.dss["chuzhi"] + "'" + xml.dss["drawOpenOrClose"] + "'" + xml.dss["drawNotes"] + "'" + xml.dss["drawLogo"] + "'" + xml.dss["drawStatus"] + "'" + xml.dss["huitie"] + "'" + xml.dss["drawHead"] + "'" + xml.dss["drawFoot"] + "'" + xml.dss["drawRule"] + "'" + xml.dss["drawXiangqing"] + "'" + xml.dss["drawshuoming"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,false,false,false,0|关闭|1|开启,true,true,0|正常|1|维护,0|开放|1|关闭,true,true,true,true,true,false";
            string strIdea = "/";
            string strOthe = "确定修改,draw.aspx?act=peizhi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:为了方便规则等填写，最好使用彩版面页进行编辑");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

    }
    //中奖概率配置
    private void DrawSetPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a>&gt;中奖概率配置");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=drawsett") + "\">查看权值与等奖的概率关系</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Chance = Utils.GetRequest("chance", "post", 4, @"^[0-9]\d*$", "中奖概率填写错误(整数)");
            string Rq1 = Utils.GetRequest("num1", "post", 4, @"^[0-9]\d*$", "一等奖权值填写错误");
            string Rq2 = Utils.GetRequest("num2", "post", 4, @"^[0-9]\d*$", "二等奖权值填写错误");
            string Rq3 = Utils.GetRequest("num3", "post", 4, @"^[0-9]\d*$", "三等奖权值填写错误");
            string Rq4 = Utils.GetRequest("num4", "post", 4, @"^[0-9]\d*$", "四等奖权值填写错误");
            string Rq5 = Utils.GetRequest("num5", "post", 4, @"^[0-9]\d*$", "五等奖权值填写错误");
            string Rq6 = Utils.GetRequest("num6", "post", 4, @"^[0-9]\d*$", "六等奖权值填写错误");
            string Rq7 = Utils.GetRequest("num7", "post", 4, @"^[0-9]\d*$", "七等奖权值填写错误");
            string Rq8 = Utils.GetRequest("num8", "post", 4, @"^[0-9]\d*$", "八等奖权值填写错误");
            string Rq9 = Utils.GetRequest("num9", "post", 4, @"^[0-9]\d*$", "九等奖权值填写错误");
            string Rq10 = Utils.GetRequest("num10", "post", 4, @"^[0-9]\d*$", "十等奖权值填写错误");
            string Rq11 = Utils.GetRequest("num11", "post", 4, @"^[0-9]\d*$", "十一等奖权值填写错误");
            string Rq12 = Utils.GetRequest("num12", "post", 4, @"^[0-9]\d*$", "十二等奖权值填写错误");
            string Rq13 = Utils.GetRequest("num13", "post", 4, @"^[0-9]\d*$", "十三等奖权值填写错误");
            string Rq14 = Utils.GetRequest("num14", "post", 4, @"^[0-9]\d*$", "十四等奖权值填写错误");
            string Rq15 = Utils.GetRequest("num15", "post", 4, @"^[0-9]\d*$", "十五等奖权值填写错误");
            string Rq16 = Utils.GetRequest("num16", "post", 4, @"^[0-9]\d*$", "十六等奖权值填写错误");
            string Rq17 = Utils.GetRequest("num17", "post", 4, @"^[0-9]\d*$", "十七等奖权值填写错误");
            string Rq18 = Utils.GetRequest("num18", "post", 4, @"^[0-9]\d*$", "十八等奖权值填写错误");
            string Rq19 = Utils.GetRequest("num19", "post", 4, @"^[0-9]\d*$", "十九等奖权值填写错误");
            string Rq20 = Utils.GetRequest("num20", "post", 4, @"^[0-9]\d*$", "二十等奖权值填写错误");

            string Rq = string.Empty;
            Rq = Rq1 + "#" + Rq2 + "#" + Rq3 + "#" + Rq4 + "#" + Rq5 + "#" + Rq6 + "#" + Rq7 + "#" + Rq8 + "#" + Rq9 + "#" + Rq10 + "#" + Rq11 + "#" + Rq12 + "#" + Rq13 + "#" + Rq14 + "#" + Rq15 + "#" + Rq16 + "#" + Rq17 + "#" + Rq18 + "#" + Rq19 + "#" + Rq20 + "#" + 8 + "#" + 8 + "#" + 8 + "#" + 8 + "#" + 8 + "";

            xml.dss["chance"] = Chance;
            xml.dss["Rq"] = Rq;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=drawset&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            string[] Rq = ub.GetSub("Rq", xmlPath).Split('#');
            string strText = "中奖概率百分比%:/,一等奖中奖权值(0-9):/,二等奖中奖权值(0-9):/,三等奖中奖权值(0-9):/,四等奖中奖权值(0-9):/,五等奖中奖权值(0-9):/,六等奖中奖权值(0-9):/,七等奖中奖权值(0-9):/,八等奖中奖权值(0-9):/,九等奖中奖权值(0-9):/,十等奖中奖权值(0-9):/,十一等奖中奖权值(0-9):/,十二等奖中奖权值(0-9):/,十三等奖中奖权值(0-9):/,十四等奖中奖权值(0-9):/,十五等奖中奖权值(0-9):/,十六等奖中奖权值(0-9):/,十七等奖中奖权值(0-9):/,十八等奖中奖权值(0-9):/,十九等奖中奖权值(0-9):/,二十等奖中奖权值(0-9):/,";
            string strName = "Chance,num1,num2,num3,num4,num5,num6,num7,num8,num9,num10,num11,num12,num13,num14,num15,num16,num17,num18,num19,num20,backurl";
            string strType = "text,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,hidden";
            string strValu = "" + xml.dss["chance"] + "'" + Rq[0] + "'" + Rq[1] + "'" + Rq[2] + "'" + Rq[3] + "'" + Rq[4] + "'" + Rq[5] + "'" + Rq[6] + "'" + Rq[7] + "'" + Rq[8] + "'" + Rq[9] + "'" + Rq[10] + "'" + Rq[11] + "'" + Rq[12] + "'" + Rq[13] + "'" + Rq[14] + "'" + Rq[15] + "'" + Rq[16] + "'" + Rq[17] + "'" + Rq[18] + "'" + Rq[19] + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,";
            string strIdea = "/";
            string strOthe = "确定修改,draw.aspx?act=drawset,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:中奖概率取百分比，即当前中奖概率为" + xml.dss["chance"] + "%<br />");
            builder.Append("<b style=\"color:red\">特别注意:这个权值就是随机几率，比如正常被抽中的几率为1，如果将权值设为0将永远也不会被抽中！权值1-9，权值数越大，被抽中概率越大，如果所有等奖都设为同一个数字，比如2，那么所有等奖抽中概率一样。此处只能默认设置权值的等级为20等奖内，如果奖箱没有那么多，可以忽略余下的权值，如果等奖超过了20，那么超出的等奖权值默认都为8</b>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //中奖概率配置与权值关系
    private void DrawSettPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[0-9]\d*$", "1000"));
        int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=drawset") + "\">中奖概率配置</a>&gt;权值概率查询");
        builder.Append(Out.Tab("</div>", "<br />"));

        #region 默认一百万次
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("当前模拟为<b style=\"color:red\">" + ptype + "</b>次随机抽取一个等奖(当前奖箱" + jiangji + "个等级奖)出现的各等奖与权值的关系");
        builder.Append(Out.Tab("</div>", "<br />"));
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        //从集合中随机抽取个数
        const ushort COUNT = 1;
        //循环次数
        int FOR_COUNT = ptype;//1000000

        //1000、10000次随机抽取，每次抽取6个

        RandomController rc = new RandomController(COUNT);
        //累积器
        Dictionary<char, int> result = new Dictionary<char, int>();
        //随机数生成器
        Random rand = new Random();
        //循环生成随机数
        for (int i = 0; i < FOR_COUNT; i++)
        {
            char[] rands = rc.ControllerRandomExtract(rand);
            for (int j = 0; j < COUNT; j++)
            {
                char item = rands[j];
                if (result.ContainsKey(item))
                    result[item] += 1;
                else
                    result.Add(item, 1);
            }
            //Thread.Sleep(5);
        }

        Dictionary<char, ushort> items = new Dictionary<char, ushort>();
        #region items取方法
        if (jiangji == 1)
        {
            for (int i = 0, j = rc.datas1.Count; i < j; i++)
            {
                items.Add(rc.datas1[i], rc.weights[i]);
            }
        }
        if (jiangji == 2)
        {
            for (int i = 0, j = rc.datas2.Count; i < j; i++)
            {
                items.Add(rc.datas2[i], rc.weights[i]);
            }
        }
        if (jiangji == 3)
        {
            for (int i = 0, j = rc.datas3.Count; i < j; i++)
            {
                items.Add(rc.datas3[i], rc.weights[i]);
            }
        }
        if (jiangji == 4)
        {
            for (int i = 0, j = rc.datas4.Count; i < j; i++)
            {
                items.Add(rc.datas4[i], rc.weights[i]);
            }
        }
        if (jiangji == 5)
        {
            for (int i = 0, j = rc.datas5.Count; i < j; i++)
            {
                items.Add(rc.datas5[i], rc.weights[i]);
            }
        }
        if (jiangji == 6)
        {
            for (int i = 0, j = rc.datas6.Count; i < j; i++)
            {
                items.Add(rc.datas6[i], rc.weights[i]);
            }
        }
        if (jiangji == 7)
        {
            for (int i = 0, j = rc.datas7.Count; i < j; i++)
            {
                items.Add(rc.datas7[i], rc.weights[i]);
            }
        }
        if (jiangji == 8)
        {
            for (int i = 0, j = rc.datas8.Count; i < j; i++)
            {
                items.Add(rc.datas8[i], rc.weights[i]);
            }
        }
        if (jiangji == 9)
        {
            for (int i = 0, j = rc.datas9.Count; i < j; i++)
            {
                items.Add(rc.datas9[i], rc.weights[i]);
            }
        }
        if (jiangji == 10)
        {
            for (int i = 0, j = rc.datas10.Count; i < j; i++)
            {
                items.Add(rc.datas10[i], rc.weights[i]);
            }
        }
        if (jiangji == 11)
        {
            for (int i = 0, j = rc.datas11.Count; i < j; i++)
            {
                items.Add(rc.datas11[i], rc.weights[i]);
            }
        }
        if (jiangji == 12)
        {
            for (int i = 0, j = rc.datas12.Count; i < j; i++)
            {
                items.Add(rc.datas12[i], rc.weights[i]);
            }
        }
        if (jiangji == 13)
        {
            for (int i = 0, j = rc.datas13.Count; i < j; i++)
            {
                items.Add(rc.datas13[i], rc.weights[i]);
            }
        }
        if (jiangji == 14)
        {
            for (int i = 0, j = rc.datas14.Count; i < j; i++)
            {
                items.Add(rc.datas14[i], rc.weights[i]);
            }
        }
        if (jiangji == 15)
        {
            for (int i = 0, j = rc.datas15.Count; i < j; i++)
            {
                items.Add(rc.datas15[i], rc.weights[i]);
            }
        }
        if (jiangji == 16)
        {
            for (int i = 0, j = rc.datas16.Count; i < j; i++)
            {
                items.Add(rc.datas16[i], rc.weights[i]);
            }
        }
        if (jiangji == 17)
        {
            for (int i = 0, j = rc.datas17.Count; i < j; i++)
            {
                items.Add(rc.datas17[i], rc.weights[i]);
            }
        }
        if (jiangji == 18)
        {
            for (int i = 0, j = rc.datas18.Count; i < j; i++)
            {
                items.Add(rc.datas18[i], rc.weights[i]);
            }
        }
        if (jiangji == 19)
        {
            for (int i = 0, j = rc.datas19.Count; i < j; i++)
            {
                items.Add(rc.datas19[i], rc.weights[i]);
            }
        }
        if (jiangji == 20)
        {
            for (int i = 0, j = rc.datas20.Count; i < j; i++)
            {
                items.Add(rc.datas20[i], rc.weights[i]);
            }
        }
        if (jiangji > 20)
        {
            for (int i = 0, j = rc.datas21.Count; i < j; i++)
            {
                items.Add(rc.datas21[i], rc.weights[i]);
            }
        }
        #endregion

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<table>");
        builder.Append("<tr><td>几等奖</td><td>出现次数</td><td>占总共出现次数百分比</td><td>权值</td></tr>");

        foreach (KeyValuePair<char, int> item in result)
        {
            builder.Append("<tr><td>" + GetRank2(item.Key) + "</td><td>" + item.Value.ToString() + "</td><td>" + ((double)item.Value / (double)(FOR_COUNT * COUNT)).ToString("0.00%") + "</td><td>" + items[item.Key] + "</td></tr>");
        }
        builder.Append("</table>");

        stopwatch.Stop();
        builder.Append("" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒");
        builder.Append(Out.Tab("</div>", ""));
        #endregion

        int ptype1 = int.Parse(Utils.GetRequest("ptype1", "post", 1, @"^[0-9]\d*$", "1000"));
        string strText = "输入预计的奖品总量:/,";
        string strName = "ptype1,backurl";
        string strType = "num,hidden";
        string strValu = "" + ptype1 + "'" + Utils.getPage(0) + "";
        string strEmpt = "false,true";
        string strIdea = "/";
        string strOthe = "确定模拟查看,draw.aspx?act=drawsett&amp;ptype=" + ptype1 + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=drawset") + "\">返回上级重新设置权值</a>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">特别注意:由于奖品总量的多少与权值共同决定了奖品出现的概率，所以有些权值低的很难出现，所以请根据自身奖品总数与奖级权值算出的概率选择每个奖级合理的权值，避免出现基本不能被抽中，根据具体需求设置</b>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //兑换积分配置
    private void JifenPeizhiPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a>&gt;换币配置");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string farm = Utils.GetRequest("farm", "post", 4, @"^[0-9]\d*$", "农场换币值填写错误(整数)");

            xml.dss["farm"] = farm;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=jifenpeizhi&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            string strText = "农场换币（金币）:/,";
            string strName = "farm,backurl";
            string strType = "text,hidden";
            string strValu = "" + xml.dss["farm"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "true";
            string strIdea = "/";
            string strOthe = "确定修改,draw.aspx?act=jifenpeizhi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("温馨提示:换币值为多少币值能换一个" + drawJifen + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //积分生成配置
    private void JfscpzPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a>&gt;" + drawJifen + "生成设定");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("===" + drawJifen + "生成大小配置===<br />");
        if (ptype == 0)
            builder.Append("社区活动|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=1") + "\">酷币消费</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=2") + "\">充值</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=3") + "\">签到</a>");
        else if (ptype == 1)
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=0") + "\">社区活动</a>|酷币消费|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=2") + "\">充值</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=3") + "\">签到</a>");
        else if (ptype == 2)
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=0") + "\">社区活动</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=1") + "\">酷币消费</a>|充值|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=3") + "\">签到</a>");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=0") + "\">社区活动</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=1") + "\">酷币消费</a>|<a href=\"" + Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=2") + "\">充值</a>|签到");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (ptype == 0)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {

                int xianliao = Convert.ToInt32(Utils.GetRequest("xianliao", "post", 4, @"^[0-9]\d*$", "闲聊发言生成" + drawJifen + "出错"));//闲聊发言
                int ht = Convert.ToInt32(Utils.GetRequest("ht", "post", 4, @"^[0-9]\d*$", "论坛回帖生成" + drawJifen + "出错"));//回帖
                int ft = Convert.ToInt32(Utils.GetRequest("ft", "post", 4, @"^[0-9]\d*$", "论坛发帖生成" + drawJifen + "出错"));//发帖
                int fahb = Convert.ToInt32(Utils.GetRequest("fahb", "post", 4, @"^[0-9]\d*$", "聊吧发红包生成" + drawJifen + "出错"));//发红包
                int td = Convert.ToInt32(Utils.GetRequest("td", "post", 4, @"^[0-9]\d*$", "帖子打赏生成" + drawJifen + "出错"));//帖子打赏
                int tj = Convert.ToInt32(Utils.GetRequest("tj", "post", 4, @"^[0-9]\d*$", "帖子加精生成" + drawJifen + "出错"));//帖子加精
                int tt = Convert.ToInt32(Utils.GetRequest("tt", "post", 4, @"^[0-9]\d*$", "推荐帖子生成" + drawJifen + "出错"));//推荐帖子
                int ts = Convert.ToInt32(Utils.GetRequest("ts", "post", 4, @"^[0-9]\d*$", "设滚帖子生成" + drawJifen + "出错"));//设滚帖子
                int tmax = Convert.ToInt32(Utils.GetRequest("tmax", "post", 4, @"^[0-9]\d*$", "帖子生成" + drawJifen + "最大值出错"));//帖子生成最大值
                int tzkg = Convert.ToInt32(Utils.GetRequest("tzkg", "post", 4, @"^[0-9]\d*$", "帖子生成" + drawJifen + "的开关出错"));//帖子生成点值的开关
                int tzkl = Convert.ToInt32(Utils.GetRequest("tzkl", "post", 4, @"^[0-9]\d*$", "生成" + drawJifen + "的概率出错"));//生成概率
                int shup = Convert.ToInt32(Utils.GetRequest("shup", "post", 4, @"^[0-9]\d*$", "发表书评生成" + drawJifen + "出错"));//发表书评
                int ksfy = Convert.ToInt32(Utils.GetRequest("ksfy", "post", 4, @"^[0-9]\d*$", "聊吧发言生成" + drawJifen + "出错"));//聊吧发言

                xml.dss["xianliao"] = xianliao;
                xml.dss["ht"] = ht;
                xml.dss["ft"] = ft;
                xml.dss["fahb"] = fahb;
                xml.dss["td"] = td;
                xml.dss["tj"] = tj;
                xml.dss["tt"] = tt;
                xml.dss["ts"] = ts;
                xml.dss["tmax"] = tmax;
                xml.dss["tzkg"] = tzkg;
                xml.dss["tzkl"] = tzkl;
                xml.dss["shup"] = shup;
                xml.dss["ksfy"] = ksfy;

                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strText = "社区产生" + drawJifen + "开关:/,闲聊发言:/,发红包:/,发帖:/,回帖:/,帖子打赏:/,帖子加精:/,推荐帖子:/,设滚帖子:/,发表书评:/,聊吧发言:/,产生" + drawJifen + "概率（1／" + xml.dss["tzkl"] + "）:/,社区每天每个账号产生" + drawJifen + "最大上限:/,,";
                string strName = "tzkg,xianliao,fahb,ft,ht,td,tj,tt,ts,shup,ksfy,tzkl,tmax,backurl";
                string strType = "select,num,num,num,num,num,num,num,num,num,num,num,num,hidden";
                string strValu = "" + xml.dss["tzkg"] + "'" + xml.dss["xianliao"] + "'" + xml.dss["fahb"] + "'" + xml.dss["ft"] + "'" + xml.dss["ht"] + "'" + xml.dss["td"] + "'" + xml.dss["tj"] + "'" + xml.dss["tt"] + "'" + xml.dss["ts"] + "'" + xml.dss["shup"] + "'" + xml.dss["ksfy"] + "'" + xml.dss["tzkl"] + "'" + xml.dss["tmax"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|开启|1|关闭,false,false,false,false,false,false,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,draw.aspx?act=jfscpz&amp;ptype=0,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:社区生成" + drawJifen + "值是有产生概率的，随机产生且设定每用户每天产生最大上限");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        if (ptype == 1)
        {

            if (Utils.ToSChinese(ac) == "ok")
            {
                int kbkg = Convert.ToInt32(Utils.GetRequest("kbkg", "all", 1, @"^[0-9]\d*$", "0"));
                int kb1 = Convert.ToInt32(Utils.GetRequest("kb1", "all", 2, @"^[0-9]\d*$", "填写错误a1"));
                int kb2 = Convert.ToInt32(Utils.GetRequest("kb2", "all", 2, @"^[0-9]\d*$", "填写错误a2"));
                int kb3 = Convert.ToInt32(Utils.GetRequest("kb3", "all", 2, @"^[0-9]\d*$", "填写错误a3"));
                int kb4 = Convert.ToInt32(Utils.GetRequest("kb4", "all", 2, @"^[0-9]\d*$", "填写错误a4"));
                int kb5 = Convert.ToInt32(Utils.GetRequest("kb5", "all", 2, @"^[0-9]\d*$", "填写错误a5"));
                int kb1jf = Convert.ToInt32(Utils.GetRequest("kb1jf", "all", 2, @"^[0-9]\d*$", "填写错误b1"));
                int kb12jf = Convert.ToInt32(Utils.GetRequest("kb12jf", "all", 2, @"^[0-9]\d*$", "填写错误b2"));
                int kb23jf = Convert.ToInt32(Utils.GetRequest("kb23jf", "all", 2, @"^[0-9]\d*$", "填写错误b3"));
                int kb34jf = Convert.ToInt32(Utils.GetRequest("kb34jf", "all", 2, @"^[0-9]\d*$", "填写错误b4"));
                int kb45jf = Convert.ToInt32(Utils.GetRequest("kb45jf", "all", 2, @"^[0-9]\d*$", "填写错误b5"));
                int kb5jf = Convert.ToInt32(Utils.GetRequest("kb5jf", "all", 2, @"^[0-9]\d*$", "填写错误b6"));
                int wabao = Convert.ToInt32(Utils.GetRequest("wabao", "all", 2, @"^[0-9]\d*$", "酷币消费当日最大上限填写错误"));

                xml.dss["kbkg"] = kbkg;
                xml.dss["kb1"] = kb1;
                xml.dss["kb2"] = kb2;
                xml.dss["kb3"] = kb3;
                xml.dss["kb4"] = kb4;
                xml.dss["kb5"] = kb5;
                xml.dss["kb1jf"] = kb1jf;
                xml.dss["kb12jf"] = kb12jf;
                xml.dss["kb23jf"] = kb23jf;
                xml.dss["kb34jf"] = kb34jf;
                xml.dss["kb45jf"] = kb45jf;
                xml.dss["kb5jf"] = kb5jf;
                xml.dss["wabao"] = wabao;

                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + ""), "2");

            }
            else
            {
                string strText = "" + ub.Get("SiteBz") + "生成" + drawJifen + "开关:,/" + ub.Get("SiteBz") + "(a1):," + ub.Get("SiteBz") + "(a2):," + ub.Get("SiteBz") + "(a3):," + ub.Get("SiteBz") + "(a4):," + ub.Get("SiteBz") + "(a5):," + drawJifen + "(b1):," + drawJifen + "(b2):," + drawJifen + "(b3):," + drawJifen + "(b4):," + drawJifen + "(b5):," + drawJifen + "(b6):,每天每个账号游戏消费" + ub.Get("SiteBz") + "产生" + drawJifen + "最大上限:,,,";
                string strName = "kbkg,kb1,kb2,kb3,kb4,kb5,kb1jf,kb12jf,kb23jf,kb34jf,kb45jf,kb5jf,wabao,ptype,ac,backurl";
                string strType = "select,num,num,num,num,num,num,num,num,num,num,num,num,hidden,hidden,hidden";
                string strValu = xml.dss["kbkg"] + "'" + xml.dss["kb1"] + "'" + xml.dss["kb2"] + "'" + xml.dss["kb3"] + "'" + xml.dss["kb4"] + "'" + xml.dss["kb5"] + "'" + xml.dss["kb1jf"] + "'" + xml.dss["kb12jf"] + "'" + xml.dss["kb23jf"] + "'" + xml.dss["kb34jf"] + "'" + xml.dss["kb45jf"] + "'" + xml.dss["kb5jf"] + "'" + xml.dss["wabao"] + "'" + "1" + "'" + "ok" + "'" + Utils.getPage(0);
                string strEmpt = "0|开启|1|关闭,true,true,true,true,true,true,true,true,true,true,true,true,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,draw.aspx?act=jfscpz&amp;ptype=1&amp;ac=ok&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                string kg = string.Empty;
                if (Convert.ToInt32(xml.dss["kbkg"]) == 0) { kg = "开启"; }
                else { kg = "关闭"; }
                builder.Append("说明：当前" + ub.Get("SiteBz") + "生成" + drawJifen + kg + "<br />");
                builder.Append("1.消费" + ub.Get("SiteBz") + "＜ <b style=\"color:red\">" + xml.dss["kb1"] + "</b>" + ub.Get("SiteBz") + "(a1),生成<b style=\"color:red\">" + xml.dss["kb1jf"] + "</b>" + drawJifen + "(b1)<br />");
                builder.Append("2.<b style=\"color:red\">" + xml.dss["kb1"] + "</b>" + ub.Get("SiteBz") + "(a1)≤消费" + ub.Get("SiteBz") + "＜ <b style=\"color:red\">" + xml.dss["kb2"] + "</b>" + ub.Get("SiteBz") + "(a2),生成<b style=\"color:red\">" + xml.dss["kb12jf"] + "</b>" + drawJifen + "(b2)<br />");
                builder.Append("3.<b style=\"color:red\">" + xml.dss["kb2"] + "</b>" + ub.Get("SiteBz") + "(a2)≤消费" + ub.Get("SiteBz") + "＜ <b style=\"color:red\">" + xml.dss["kb3"] + "</b>" + ub.Get("SiteBz") + "(a3),生成<b style=\"color:red\">" + xml.dss["kb23jf"] + "</b>" + drawJifen + "(b3)<br />");
                builder.Append("4.<b style=\"color:red\">" + xml.dss["kb3"] + "</b>" + ub.Get("SiteBz") + "(a3)≤消费" + ub.Get("SiteBz") + "＜ <b style=\"color:red\">" + xml.dss["kb4"] + "</b>" + ub.Get("SiteBz") + "(a4),生成<b style=\"color:red\">" + xml.dss["kb34jf"] + "</b>" + drawJifen + "(b4)<br />");
                builder.Append("5.<b style=\"color:red\">" + xml.dss["kb4"] + "</b>" + ub.Get("SiteBz") + "(a4)≤消费" + ub.Get("SiteBz") + "＜ <b style=\"color:red\">" + xml.dss["kb5"] + "</b>" + ub.Get("SiteBz") + "(a5),生成<b style=\"color:red\">" + xml.dss["kb45jf"] + "</b>" + drawJifen + "(b5)<br />");
                builder.Append("6.消费" + ub.Get("SiteBz") + "≥ <b style=\"color:red\">" + xml.dss["kb5"] + "</b>" + ub.Get("SiteBz") + "(a5),生成<b style=\"color:red\">" + xml.dss["kb5jf"] + "</b>" + drawJifen + "(b6)");

                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:生成" + drawJifen + "值");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));


            }
        }
        if (ptype == 2)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                int chongzhikg = Convert.ToInt32(Utils.GetRequest("chongzhikg", "post", 4, @"^[0-9]\d*$", "充值金额生成" + drawJifen + "选择出错"));
                int chongzhi = Convert.ToInt32(Utils.GetRequest("chongzhi", "post", 4, @"^[0-9]\d*$", "充值金额填写出错"));
                int chongmin = Convert.ToInt32(Utils.GetRequest("chongmin", "post", 4, @"^[0-9]\d*$", "充值金额最小值填写出错"));
                int chongzhijf = Convert.ToInt32(Utils.GetRequest("chongzhijf", "post", 4, @"^[0-9]\d*$", "生成" + drawJifen + "填写出错"));
                int chongmax = Convert.ToInt32(Utils.GetRequest("chongmax", "post", 4, @"^[0-9]\d*$", "充值金额最大值填写出错"));
                int chongmaxzhi = Convert.ToInt32(Utils.GetRequest("chongmaxzhi", "post", 4, @"^[0-9]\d*$", "生成最大" + drawJifen + "填写出错"));
                int kaizhuang = Convert.ToInt32(Utils.GetRequest("kaizhuang", "post", 4, @"^[0-9]\d*$", "每日生成" + drawJifen + "上限填写出错"));

                xml.dss["chongzhikg"] = chongzhikg;
                xml.dss["chongzhi"] = chongzhi;
                xml.dss["chongmin"] = chongmin;
                xml.dss["chongzhijf"] = chongzhijf;
                xml.dss["chongmax"] = chongmax;
                xml.dss["chongmaxzhi"] = chongmaxzhi;
                xml.dss["kaizhuang"] = kaizhuang;

                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strText = "网上充值生成" + drawJifen + "开关:/,至少充值金额（RMBmin）:/,最大充值金额（RMBmax）:/,每充值金额（RMB）:/,生成" + drawJifen + "的值（DZ）:/,生成最大" + drawJifen + "的值（DZmax）:/,每天每个账号充值产生" + drawJifen + "最大上限:/,,";
                string strName = "chongzhikg,chongmin,chongmax,chongzhi,chongzhijf,chongmaxzhi,kaizhuang,backurl";
                string strType = "select,num,num,num,num,num,num,hidden";
                string strValu = "" + xml.dss["chongzhikg"] + "'" + xml.dss["chongmin"] + "'" + xml.dss["chongmax"] + "'" + xml.dss["chongzhi"] + "'" + xml.dss["chongzhijf"] + "'" + xml.dss["chongmaxzhi"] + "'" + xml.dss["kaizhuang"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|开启|1|关闭,false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,draw.aspx?act=jfscpz&amp;ptype=2,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("网上充值算法：<br />");
                builder.Append("1.网上充值开关开启，充值金额X不小于至少充值金额（RMBmin）,不超过最大充值金额（RMBmax），即" + xml.dss["chongmin"] + " ≤X≤" + xml.dss["chongmax"] + "，每充值" + xml.dss["chongzhi"] + "（RMB）,生成" + xml.dss["chongzhijf"] + drawJifen + "（DZ）,当充值金额大于最大充值金额时，无论充值多少都只生成" + xml.dss["chongmaxzhi"] + drawJifen + "（DZmax）<br />");
                builder.Append(Out.Tab("</div>", "<br />"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:充值仅限于网上充值（1元=2500币的接口），充值按" + xml.dss["chongzhi"] + "（RMB）倍数计算，当金额除余不足时按1" + drawJifen + "计算");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }
        if (ptype == 3)
        {
            if (Utils.ToSChinese(ac) == "确定修改")
            {

                int Qd = Convert.ToInt32(Utils.GetRequest("Qd", "post", 4, @"^[0-9]\d*$", "普通签到值出错"));
                int Qdvip = Convert.ToInt32(Utils.GetRequest("Qdvip", "post", 4, @"^[0-9]\d*$", "VIP签到值出错"));
                int Qdweek = Convert.ToInt32(Utils.GetRequest("Qdweek", "post", 4, @"^[0-9]\d*$", "连续签到天数出错"));
                int Qdkg = Convert.ToInt32(Utils.GetRequest("Qdkg", "post", 4, @"^[0-9]\d*$", "普通签到开关出错"));
                int Qdvipkg = Convert.ToInt32(Utils.GetRequest("Qdvipkg", "post", 4, @"^[0-9]\d*$", "VIP签到开关出错"));

                xml.dss["Qd"] = Qd;
                xml.dss["Qdvip"] = Qdvip;
                xml.dss["Qdweek"] = Qdweek;
                xml.dss["Qdkg"] = Qdkg;
                xml.dss["Qdvipkg"] = Qdvipkg;

                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=jfscpz&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strText = "普通会员连续签到开关:/,普通会员点到产生值（A）:/,VIP会员连续签到开关:/,VIP会员点到产生值（B）:/,连续签到天数（C）:/,,";
                string strName = "Qdkg,Qd,Qdvipkg,Qdvip,Qdweek,backurl";
                string strType = "select,num,select,num,num,hidden";
                string strValu = "" + xml.dss["Qdkg"] + "'" + xml.dss["Qd"] + "'" + xml.dss["Qdvipkg"] + "'" + xml.dss["Qdvip"] + "'" + xml.dss["Qdweek"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "0|开启|1|关闭,false,0|开启|1|关闭,,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,draw.aspx?act=jfscpz&amp;ptype=3,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("签到算法：<br />");
                builder.Append("1.普通会员非连续签到算法：当普通会员连续签到开关关闭时，普通会员每一日签到产生的" + drawJifen + "等于A的值，即为<b style=\"color:red\">" + xml.dss["Qd"] + drawJifen + "</b><br />");
                builder.Append("2.普通会员连续签到算法：开启普通会员连续签到，普通会员连续签到产生的" + drawJifen + "等于连续签到天数X与A的乘积，即为<b style=\"color:red\">X*" + xml.dss["Qd"] + drawJifen + "</b><br />");
                builder.Append("3.VIP会员非连续签到算法：当VIP会员连续签到开关关闭时，VIP会员每一日签到产生的" + drawJifen + "等于B的值，即为<b style=\"color:red\">" + xml.dss["Qdvip"] + drawJifen + "</b><br />");
                builder.Append("4.VIP会员连续签到算法：开启VIP会员连续签到，VIP会员连续签到产生的" + drawJifen + "等于连续签到天数X与B的乘积，即为<b style=\"color:red\">X*" + xml.dss["Qdvip"] + drawJifen + "</b><br />");
                builder.Append(Out.Tab("</div>", "<br />"));


                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示:签到为普通会员签到与VIP会员签到，签到分为普通签到与连续签到，各签到生成" + drawJifen + "算法不一样，当连续签到达到" + xml.dss["Qdweek"] + "天（C），则第二天签到重新计算");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
        }


    }
    //积分生成来源管理
    private void ScpeiPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        //ub xml = new ub();
        //Application.Remove(xmlPath);//清缓存
        //xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=peizhi") + "\">游戏配置</a>&gt;" + drawJifen + "生成管理");
        builder.Append(Out.Tab("</div>", "<br />"));


        string info = Utils.GetRequest("info", "all", 1, "", "");

        string[] Num = new string[50];
        Num[1] = Utils.GetRequest("Num1", "post", 1, "", "");
        Num[2] = Utils.GetRequest("Num2", "post", 1, "", "");
        Num[3] = Utils.GetRequest("Num3", "post", 1, "", "");
        Num[4] = Utils.GetRequest("Num4", "post", 1, "", "");
        Num[5] = Utils.GetRequest("Num5", "post", 1, "", "");
        Num[6] = Utils.GetRequest("Num6", "post", 1, "", "");
        Num[7] = Utils.GetRequest("Num7", "post", 1, "", "");
        Num[8] = Utils.GetRequest("Num8", "post", 1, "", "");
        Num[9] = Utils.GetRequest("Num9", "post", 1, "", "");
        Num[10] = Utils.GetRequest("Num10", "post", 1, "", "");
        Num[11] = Utils.GetRequest("Num11", "post", 1, "", "");
        Num[12] = Utils.GetRequest("Num12", "post", 1, "", "");
        Num[13] = Utils.GetRequest("Num13", "post", 1, "", "");
        Num[14] = Utils.GetRequest("Num14", "post", 1, "", "");
        Num[15] = Utils.GetRequest("Num15", "post", 1, "", "");
        Num[16] = Utils.GetRequest("Num16", "post", 1, "", "");
        Num[17] = Utils.GetRequest("Num17", "post", 1, "", "");
        Num[18] = Utils.GetRequest("Num18", "post", 1, "", "");
        Num[19] = Utils.GetRequest("Num19", "post", 1, "", "");
        //
        Num[20] = Utils.GetRequest("Num20", "post", 1, "", "");
        Num[21] = Utils.GetRequest("Num21", "post", 1, "", "");
        Num[22] = Utils.GetRequest("Num22", "post", 1, "", "");
        Num[23] = Utils.GetRequest("Num23", "post", 1, "", "");
        Num[24] = Utils.GetRequest("Num24", "post", 1, "", "");
        Num[25] = Utils.GetRequest("Num25", "post", 1, "", "");
        Num[26] = Utils.GetRequest("Num26", "post", 1, "", "");
        Num[27] = Utils.GetRequest("Num27", "post", 1, "", "");
        Num[28] = Utils.GetRequest("Num28", "post", 1, "", "");
        Num[29] = Utils.GetRequest("Num29", "post", 1, "", "");
        Num[30] = Utils.GetRequest("Num30", "post", 1, "", "");
        Num[31] = Utils.GetRequest("Num31", "post", 1, "", "");
        Num[32] = Utils.GetRequest("Num32", "post", 1, "", "");
        Num[33] = Utils.GetRequest("Num33", "post", 1, "", "");
        Num[34] = Utils.GetRequest("Num34", "post", 1, "", "");
        Num[35] = Utils.GetRequest("Num35", "post", 1, "", "");
        Num[36] = Utils.GetRequest("Num36", "post", 1, "", "");
        Num[37] = Utils.GetRequest("Num37", "post", 1, "", "");
        Num[38] = Utils.GetRequest("Num38", "post", 1, "", "");
        Num[39] = Utils.GetRequest("Num39", "post", 1, "", "");
        Num[40] = Utils.GetRequest("Num40", "post", 1, "", "");
        Num[41] = Utils.GetRequest("Num41", "post", 1, "", "");
        Num[42] = Utils.GetRequest("Num42", "post", 1, "", "");
        Num[43] = Utils.GetRequest("Num43", "post", 1, "", "");
        Num[44] = Utils.GetRequest("Num44", "post", 1, "", "");
        Num[45] = Utils.GetRequest("Num45", "post", 1, "", "");
        if (info == "ok")
        {
            if (ac == "ok")
            {
                ub xml = new ub();
                Application.Remove(xmlPath);//清缓存
                xml.ReloadSub(xmlPath); //加载配置 .
                string game = Utils.GetRequest("game", "all", 1, "", "");
                string bbs = Utils.GetRequest("bbs", "all", 1, "", "");
                //1全社区2社区3仅游戏
                xml.dss["gameAction"] = game;//游戏
                xml.dss["bbsAction"] = bbs;//论坛

                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=scpeizhi&amp;backurl=" + Utils.getPage(1) + ""), "1");

            }
            else
            {

                string game = "";
                string bbs = "";
                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("确定配置选择吗?" + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div>", ""));
                //1全社区2社区3仅游戏
                builder.Append("游戏区：" + "<br/>");
                int k = 1;
                for (int i = 1; i < 26; i++)
                {
                    if (Num[i] != "")
                    {
                        game += Num[i];
                        game += "#";
                        builder.Append(Num[i] + ".");
                        if (k % 3 == 0)
                        { builder.Append("<br/>"); }
                        k++;
                    }
                }

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("-----------");
                builder.Append(Out.Tab("</div>", "<br/>"));

                builder.Append("" + "论坛区：" + "<br/>");
                int j = 1;
                for (int i = 21; i < 36; i++)
                {
                    if (Num[i] != "")
                    {
                        bbs += Num[i];
                        bbs += "#";
                        builder.Append(Num[i] + ".");
                        if (j % 3 == 0)
                        { builder.Append("<br/>"); }
                        j++;
                    }
                }

                builder.Append("<input type=\"hidden\" name=\"gameAction\" Value=\"" + game + "\"/>");
                builder.Append("<input type=\"hidden\" name=\"bbsAction\" Value=\"" + bbs + "\"/>");
                builder.Append(Out.Tab("</div>", "<br/>"));
                string strText = ":,:,,,";
                string strName = "game,bbs,info,ac,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden";
                string strValu = game + "'" + bbs + "'" + "ok" + "'" + "ok" + "'" + Utils.getPage(0);
                string strEmpt = "true,false,false,false,false";
                string strIdea = "";
                string strOthe = "确定选择,draw.aspx?act=scpeizhi&amp;info=ok&amp;ac=ok&amp;,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=scpeizhi&amp;") + "\">重新选择</a>");
                builder.Append(Out.Tab("</div>", ""));

            }
        }
        else
        {
            string gameAction = (ub.GetSub("gameAction", xmlPath));
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));
            builder.Append("<form id=\"form1\" method=\"post\" action=\"draw.aspx\">");
            //1全社区2社区3仅游戏
            builder.Append("<div class=\"text\">= = =" + ub.Get("SiteBz") + "消费产生" + drawJifen + "管理= = =<br/></div>");
            builder.Append("<div class=\"text\">= = =游戏区选择 可全勾选= = =<br/></div>");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<table><tr><td>");
            if (gameAction.Contains("虚拟竞猜"))
            { builder.Append("<input type=\"checkbox\" name=\"Num1\" value=\"虚拟竞猜\" checked=\"true\"/> 虚拟竞猜"); }
            else
            { builder.Append("<input type=\"checkbox\" name=\"Num1\" value=\"虚拟竞猜\" /> 虚拟竞猜"); }
            builder.Append("</td><td>");
            if (gameAction.Contains("虚拟彩票"))
            { builder.Append("<input type=\"checkbox\" name=\"Num2\" value=\"虚拟彩票\" checked=\"true\" /> 虚拟彩票"); }
            else
            { builder.Append("<input type=\"checkbox\" name=\"Num2\" value=\"虚拟彩票\" /> 虚拟彩票"); }
            builder.Append("</td><td>");
            if (gameAction.Contains("幸运二八"))
            { builder.Append("<input type=\"checkbox\" name=\"Num3\" value=\"幸运二八\" checked=\"true\" /> 幸运二八"); }
            else
            { builder.Append("<input type=\"checkbox\" name=\"Num3\" value=\"幸运二八\" /> 幸运二八"); }
            builder.Append("</td></tr><tr><td>");
            if (gameAction.Contains("点数挖宝"))
            { builder.Append("<input type=\"checkbox\" name=\"Num4\" value=\"点数挖宝\" checked=\"true\" /> 点数挖宝"); }
            else { builder.Append("<input type=\"checkbox\" name=\"Num4\" value=\"点数挖宝\" /> 点数挖宝"); }
            builder.Append("</td><td>");
            if (gameAction.Contains("大小庄家"))
                builder.Append("<input type=\"checkbox\" name=\"Num5\" value=\"大小庄家\" checked=\"true\"/> 大小庄家");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num5\" value=\"大小庄家\" /> 大小庄家");
            builder.Append("</td><td>");
            if (gameAction.Contains("快乐十分"))
                builder.Append("<input type=\"checkbox\" name=\"Num6\" value=\"快乐十分\" checked=\"true\" /> 快乐十分");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num6\" value=\"快乐十分\" /> 快乐十分");
            builder.Append("</td></tr><tr><td>");
            if (gameAction.Contains("上证指数"))
                builder.Append("<input type=\"checkbox\" name=\"Num7\" value=\"上证指数\" checked=\"true\" /> 上证指数");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num7\" value=\"上证指数\" /> 上证指数");
            builder.Append("</td><td>");
            if (gameAction.Contains("大小掷骰"))
                builder.Append("<input type=\"checkbox\" name=\"Num8\" value=\"大小掷骰\" checked=\"true\" /> 大小掷骰");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num8\" value=\"大小掷骰\" /> 大小掷骰");
            builder.Append("</td><td>");
            if (gameAction.Contains("欢乐竞拍"))
                builder.Append("<input type=\"checkbox\" name=\"Num9\" value=\"欢乐竞拍\" checked=\"true\" /> 欢乐竞拍");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num9\" value=\"欢乐竞拍\" /> 欢乐竞拍");
            builder.Append("</td></tr><tr><td>");
            if (gameAction.Contains("时时彩票"))
                builder.Append("<input type=\"checkbox\" name=\"Num10\" value=\"时时彩票\" checked=\"true\"  /> 时时彩票");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num10\" value=\"时时彩票\"  /> 时时彩票");
            builder.Append("</td><td>");
            if (gameAction.Contains("吹牛游戏"))
                builder.Append("<input type=\"checkbox\" name=\"Num11\" value=\"吹牛游戏\" checked=\"true\" /> 吹牛游戏");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num11\" value=\"吹牛游戏\"  /> 吹牛游戏");
            builder.Append("</td><td>");
            if (gameAction.Contains("捕鱼达人"))
                builder.Append("<input type=\"checkbox\" name=\"Num12\" value=\"捕鱼达人\" checked=\"true\" /> 捕鱼达人");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num12\" value=\"捕鱼达人\" /> 捕鱼达人");
            builder.Append("</td></tr><tr><td>");
            if (gameAction.Contains("闯荡全城"))
                builder.Append("<input type=\"checkbox\" name=\"Num13\" value=\"闯荡全城\" checked=\"true\" /> 闯荡全城");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num13\" value=\"闯荡全城\"  /> 闯荡全城");
            builder.Append("</td><td>");
            if (gameAction.Contains("快3挖宝"))
                builder.Append("<input type=\"checkbox\" name=\"Num14\" value=\"快3挖宝\" checked=\"true\"  /> 快3挖宝");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num14\" value=\"快3挖宝\"  /> 快3挖宝");
            builder.Append("</td><td>");
            if (gameAction.Contains("快乐扑克3"))
                builder.Append("<input type=\"checkbox\" name=\"Num15\" value=\"快乐扑克3\" checked=\"true\"  /> 快乐扑克3");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num15\" value=\"快乐扑克3\"  /> 快乐扑克3");
            builder.Append("</td></tr><tr><td>");
            if (gameAction.Contains("开心农场"))
                builder.Append("<input type=\"checkbox\" name=\"Num16\" value=\"开心农场\"  checked=\"true\" /> 开心农场");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num16\" value=\"开心农场\" /> 开心农场");
            builder.Append("</td><td>");
            if (gameAction.Contains("德州扑克"))
                builder.Append("<input type=\"checkbox\" name=\"Num17\" value=\"德州扑克\"  checked=\"true\" /> 德州扑克");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num17\" value=\"德州扑克\" /> 德州扑克");
            builder.Append("</td><td>");
            if (gameAction.Contains("百家欢乐"))
                builder.Append("<input type=\"checkbox\" name=\"Num18\" value=\"百家欢乐\"  checked=\"true\"/> 百家欢乐");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num18\" value=\"百家欢乐\" /> 百家欢乐");
            builder.Append("</td></tr>");
            builder.Append("<tr><td>");
            if (gameAction.Contains("酷币云购"))
                builder.Append("<input type=\"checkbox\" name=\"Num19\" value=\"酷币云购\" checked=\"true\" /> 酷币云购");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num19\" value=\"酷币云购\" /> 酷币云购");
            builder.Append("</td>");
            builder.Append("<td>");
            if (gameAction.Contains("6场半"))
                builder.Append("<input type=\"checkbox\" name=\"Num20\" value=\"6场半\" checked=\"true\" /> 6场半");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num20\" value=\"6场半\" /> 6场半");
            builder.Append("</td>");
            builder.Append("<td>");
            if (gameAction.Contains("好彩一"))
                builder.Append("<input type=\"checkbox\" name=\"Num21\" value=\"好彩一\" checked=\"true\" /> 好彩一");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num21\" value=\"好彩一\" /> 好彩一");
            builder.Append("</td></tr>");
            builder.Append("<tr><td>");
            if (gameAction.Contains("PK拾"))
                builder.Append("<input type=\"checkbox\" name=\"Num22\" value=\"PK拾\" checked=\"true\" /> PK拾");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num22\" value=\"PK拾\" /> PK拾");
            builder.Append("</td>");
            builder.Append("<td>");
            if (gameAction.Contains("胜负彩"))
                builder.Append("<input type=\"checkbox\" name=\"Num23\" value=\"胜负彩\" checked=\"true\" /> 胜负彩");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num23\" value=\"胜负彩\" /> 胜负彩");
            builder.Append("</td>");
            builder.Append("<td>");
            if (gameAction.Contains("进球彩"))
                builder.Append("<input type=\"checkbox\" name=\"Num24\" value=\"进球彩\" checked=\"true\" /> 进球彩");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num24\" value=\"进球彩\" /> 进球彩");
            builder.Append("</td></tr>");
            builder.Append("<tr><td>");
            if (gameAction.Contains("宠物"))
                builder.Append("<input type=\"checkbox\" name=\"Num25\" value=\"至爱宠物\" checked=\"true\" /> 至爱宠物");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num25\" value=\"至爱宠物\" /> 至爱宠物");
            builder.Append("</td></tr></table>");


            builder.Append("<div class=\"text\">= = =社区选择 可全勾选= = =<br/></div>");

            builder.Append("<table><tr><td>");
            if (bbsAction.Contains("发表帖子"))
                builder.Append("<input type=\"checkbox\" name=\"Num26\" value=\"发表帖子\" checked=\"true\"/> 发表帖子");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num26\" value=\"发表帖子\" /> 发表帖子");
            builder.Append("</td><td>");
            if (bbsAction.Contains("回复帖子"))
                builder.Append("<input type=\"checkbox\" name=\"Num27\" value=\"回复帖子\" checked=\"true\"/> 回复帖子");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num27\" value=\"回复帖子\" /> 回复帖子");
            builder.Append("</td><td>");
            if (bbsAction.Contains("帖子打赏"))
                builder.Append("<input type=\"checkbox\" name=\"Num28\" value=\"帖子打赏\" checked=\"true\"/> 帖子打赏");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num28\" value=\"帖子打赏\" /> 帖子打赏");
            builder.Append("</td></tr><tr><td>");
            if (bbsAction.Contains("加精帖子"))
                builder.Append("<input type=\"checkbox\" name=\"Num29\" value=\"加精帖子\" checked=\"true\"/> 加精帖子");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num29\" value=\"加精帖子\" /> 加精帖子");
            builder.Append("</td><td>");
            if (bbsAction.Contains("推荐帖子"))
                builder.Append("<input type=\"checkbox\" name=\"Num30\" value=\"推荐帖子\" checked=\"true\"/> 推荐帖子");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num30\" value=\"推荐帖子\" /> 推荐帖子");
            builder.Append("</td><td>");
            if (bbsAction.Contains("设滚帖子"))
                builder.Append("<input type=\"checkbox\" name=\"Num31\" value=\"设滚帖子\" checked=\"true\"/> 设滚帖子");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num31\" value=\"设滚帖子\" /> 设滚帖子");
            builder.Append("</td></tr><tr><td>");
            if (bbsAction.Contains("闲聊发言"))
                builder.Append("<input type=\"checkbox\" name=\"Num32\" value=\"闲聊发言\" checked=\"true\"/> 闲聊发言");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num32\" value=\"闲聊发言\" /> 闲聊发言");
            builder.Append("</td><td>");
            if (bbsAction.Contains("发红包者"))
                builder.Append("<input type=\"checkbox\" name=\"Num33\" value=\"发红包者\" checked=\"true\"/> 发红包者");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num33\" value=\"发红包者\" /> 发红包者");
            builder.Append("</td><td>");
            if (bbsAction.Contains("发表书评"))
                builder.Append("<input type=\"checkbox\" name=\"Num34\" value=\"发表书评\" checked=\"true\"/> 发表书评");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num34\" value=\"发表书评\" /> 发表书评");
            builder.Append("</td></tr>");
            builder.Append("<tr><td>");
            if (bbsAction.Contains("聊吧发言"))
                builder.Append("<input type=\"checkbox\" name=\"Num35\" value=\"聊吧发言\" checked=\"true\"/> 聊吧发言");
            else
                builder.Append("<input type=\"checkbox\" name=\"Num35\" value=\"聊吧发言\" /> 聊吧发言");
            builder.Append("</td></tr></table>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"scpeizhi\"/>");
            builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"not\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            builder.Append("</form>");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("温馨提示：已勾选的表示参与" + drawJifen + "生成.");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //游戏内测管理
    private void SetStatueCeshi()
    {
        Master.Title = "" + GameName + "设置测试状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;");
        builder.Append("设置测试状态");
        builder.Append(Out.Tab("</div>", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/draw.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ceshi = Utils.GetRequest("ceshi", "post", 2, @"^[0-9]\d*$", "测试权限管理隔输入出错");
            string CeshiQualification = Utils.GetRequest("CeshiQualification", "all", 2, @"^[^\^]{1,2000}$", "请输入测试号");
            xml.dss["ceshi"] = ceshi;
            xml.dss["CeshiQualification"] = CeshiQualification;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=setceshi"), "2");
        }
        else
        {

            string strText = "测试权限管理:/,添加测试号(多测试号用#分隔):/,";
            string strName = "ceshi,CeshiQualification,backurl";
            string strType = "select,text,hidden";
            string strValu = xml.dss["ceshi"] + "'" + xml.dss["CeshiQualification"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "0|开放|1|内测,true";
            string strIdea = "/";
            string strOthe = "确定修改,draw.aspx?act=setceshi,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", xmlPath));
            string[] name = CeshiQualification.Split('#');
            // foreach (string n in imgNum)
            builder.Append("当前测试号:<br />");
            for (int n = 0; n < name.Length; n++)
            {
                if ((n + 1) % 5 == 0)
                {
                    builder.Append(name[n] + "," + "<br />");
                }
                else
                {
                    builder.Append(name[n] + ",");
                }
            }
            builder.Append(Out.Tab("</div>", "<br/>"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            //builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
    //重置游戏
    private void ResetPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 9)
        {
            Utils.Error("权限不足", "");
        }

        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;游戏重置");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?info=1&amp;act=reset") + "\">[一键全部重置]</a>");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "1")
        {
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=reset&amp;info=" + 2 + "") + "\">&nbsp;-->确认重置</a>");

        }
        if (info == "2")
        {
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_DrawUser");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawBox");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawDS");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawnotes");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawJifen");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawlist");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_drawjifenlog");
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_DrawTen");
            BCW.Draw.Model.DrawTen mod = new BCW.Draw.Model.DrawTen();
            for (int i = 0; i < 10; i++)
            {
                mod.ID = i;
                mod.Rank = i;
                mod.GoodsCounts = 0;
                new BCW.Draw.BLL.DrawTen().Add(mod);
            }

            int j = 10;
            string str = string.Empty;
            string str1 = string.Empty;
            int K = 0;
            for (int i = 0; i < j; i++)
            {
                str += 0;
                if (i <= (j - 2))
                {
                    str += "#";
                }
                K++;
            }
            xml.dss["jiangji"] = 10;
            xml.dss["jiangjikg"] = str;
            xml.dss["lun"] = 0;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            Utils.Success("重置游戏", "重置游戏成功..", Utils.getUrl("draw.aspx?act=reset"), "3");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b style=\"color:red\">注意：重置后，数据无法恢复。</b><br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">[再看看吧..]</a>");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //奖品派送
    private void PaisongPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));

        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;奖品派送 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "2"));

        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));

        builder.Append(Out.Tab("", ""));
        string strText = "输入奖品ID:/,";
        string strName = "counts,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜奖品,draw.aspx?act=paisongxinxi,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", "<br />----------<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">未确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;ptype=1&amp;backurl=" + Utils.getPage(0) + "") + "\">未确认信息</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">已确认信息" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\">已确认信息</a>" + "|");

        if (ptype == 3)
            builder.Append("<b style=\"color:black\">已发货" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;ptype=3&amp;backurl=" + Utils.getPage(0) + "") + "\">已发货</a>" + "|");
        if (ptype == 4)
            builder.Append("<b style=\"color:black\">已送达" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;ptype=4&amp;backurl=" + Utils.getPage(0) + "") + "\">已送达</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        //查询条件
        if (ptype == 1)
        {
            strWhere = " (MyGoodsStatue='0' or MyGoodsStatue='4')";
            strOrder = "Num Desc";
        }
        if (ptype == 2)
        {
            strWhere = " MyGoodsStatue='3'";
            strOrder = "Num Desc";
        }
        if (ptype == 3)
        {
            strWhere = " MyGoodsStatue='1'";
            strOrder = "Num Desc";
        }
        if (ptype == 4)
        {
            strWhere = "MyGoodsStatue='2'";
            strOrder = "Num Desc";
        }

        string[] pageValUrl = { "act", "ac", "id", "usid", "ptype", "counts", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listdrawuser = new BCW.Draw.BLL.DrawUser().GetDrawUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);

        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listdrawuser)
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
                int d = ((pageIndex - 1) * 10 + k);
                string sText = string.Empty;
                BCW.Draw.Model.DrawUser some = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(Convert.ToInt32(n.Num));
                int rrank = 0;
                try
                {
                    rrank = new BCW.Draw.BLL.DrawBox().GetRank(some.GoodsCounts);
                }
                catch { }
                string rank = "";
                int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
                for (int i = 0; i < j; i++)
                {
                    if (i < 20)
                    {
                        if (rrank == 0) { rank = "一等奖"; }
                        if (rrank == 1) { rank = "二等奖"; }
                        if (rrank == 2) { rank = "三等奖"; }
                        if (rrank == 3) { rank = "四等奖"; }
                        if (rrank == 4) { rank = "五等奖"; }
                        if (rrank == 5) { rank = "六等奖"; }
                        if (rrank == 6) { rank = "七等奖"; }
                        if (rrank == 7) { rank = "八等奖"; }
                        if (rrank == 8) { rank = "九等奖"; }
                        if (rrank == 9) { rank = "十等奖"; }
                        if (rrank == 10) { rank = "十一等奖"; }
                        if (rrank == 11) { rank = "十二等奖"; }
                        if (rrank == 12) { rank = "十三等奖"; }
                        if (rrank == 13) { rank = "十四等奖"; }
                        if (rrank == 14) { rank = "十五等奖"; }
                        if (rrank == 15) { rank = "十六等奖"; }
                        if (rrank == 16) { rank = "十七等奖"; }
                        if (rrank == 17) { rank = "十八等奖"; }
                        if (rrank == 18) { rank = "十九等奖"; }
                        if (rrank == 19) { rank = "二十等奖"; }
                        if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                    }
                    else
                    {
                        if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                        else
                        {
                            if (rrank == i) { rank = (i + 1) + "等奖"; }
                        }
                    }
                }

                if (n.R != 888)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "抽中" + rank + "<a href=\"" + Utils.getUrl("draw.aspx?act=paisongxinxi&amp;counts=" + n.Num + "") + "\">" + n.MyGoods + "</a>" + "|奖品ID:" + n.Num + "";
                }
                else
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "兑换" + "<a href=\"" + Utils.getUrl("draw.aspx?act=paisongxinxi&amp;counts=" + n.Num + "") + "\">" + n.MyGoods + "</a>" + "|奖品ID:" + n.Num + "";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;ptype=" + ptype + "&amp;counts=" + n.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "");
        }

        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //奖品派送信息
    private void PaisongXXPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        //  int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=paisong") + "\">奖品派送</a>&gt;奖品派送信息 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (!new BCW.Draw.BLL.DrawUser().Existsnum(counts))
            Utils.Error("不存在该记录，请重新查询ID", "");
        BCW.Draw.Model.DrawUser model = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(counts);
        string type = "";
        if (model.MyGoodsType == 0) { type = "酷币"; }
        if (model.MyGoodsType == 1) { type = "实物"; }
        if (model.MyGoodsType == 2) { type = "道具"; }
        if (model.MyGoodsType == 3) { type = "属性"; }

        int rrank = 0;
        try
        {
            rrank = new BCW.Draw.BLL.DrawBox().GetRank(model.GoodsCounts);
        }
        catch { }
        string rank = "";
        int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
        for (int i = 0; i < j; i++)
        {
            if (i < 20)
            {
                if (rrank == 0) { rank = "一等奖"; }
                if (rrank == 1) { rank = "二等奖"; }
                if (rrank == 2) { rank = "三等奖"; }
                if (rrank == 3) { rank = "四等奖"; }
                if (rrank == 4) { rank = "五等奖"; }
                if (rrank == 5) { rank = "六等奖"; }
                if (rrank == 6) { rank = "七等奖"; }
                if (rrank == 7) { rank = "八等奖"; }
                if (rrank == 8) { rank = "九等奖"; }
                if (rrank == 9) { rank = "十等奖"; }
                if (rrank == 10) { rank = "十一等奖"; }
                if (rrank == 11) { rank = "十二等奖"; }
                if (rrank == 12) { rank = "十三等奖"; }
                if (rrank == 13) { rank = "十四等奖"; }
                if (rrank == 14) { rank = "十五等奖"; }
                if (rrank == 15) { rank = "十六等奖"; }
                if (rrank == 16) { rank = "十七等奖"; }
                if (rrank == 17) { rank = "十八等奖"; }
                if (rrank == 18) { rank = "十九等奖"; }
                if (rrank == 19) { rank = "二十等奖"; }
                if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
            }
            else
            {
                if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                else
                {
                    if (rrank == i) { rank = (i + 1) + "等奖"; }
                }
            }
        }


        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【奖品信息】<br />");
        builder.Append("奖品ID：" + model.Num + "<br />");
        builder.Append("奖品等级：" + rank + "<br />");
        if (model.MyGoodsType == 0)
        {
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            builder.Append("酷币价值：" + model.MyGoodsValue + "<br />");
        }
        if (model.MyGoodsType == 1)
        {
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "<br />");
            //builder.Append("奖品价值：" + model.MyGoodsValue + "<br />");
        }
        if (model.MyGoodsType == 2)
        {
            string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
            builder.Append("奖品名称：" + model.MyGoods + "<br />");
            builder.Append("奖品类型：" + type + "(" + GN + ")" + "<br />");
            //builder.Append("道具价值：" + model.MyGoodsValue + "<br />");
        }
        if (model.MyGoodsType == 3)
        {
            builder.Append("奖品名称：" + model.MyGoods + "" + "+" + model.MyGoodsValue + " <br />");
            builder.Append("奖品类型：" + type + "<br />");
        }
        builder.Append("奖品个数: <b style=\"color:red\">" + model.MyGoodsNum + "</b>");

        if (model.MyGoodsImg.ToString() != "0" && model.MyGoodsImg.ToString() != "100" && model.MyGoodsImg.ToString() != "5" && model.MyGoodsImg.ToString() != "10" && model.MyGoodsImg.ToString() != "1")
        {
            builder.Append("<br/>" + "图片描述:");
            builder.Append("<br/>");
            string[] imgNum = model.MyGoodsImg.Split(',');
            // foreach (string n in imgNum)
            for (int c = 0; c < imgNum.Length - 1; c++)
            {
                if (model.MyGoodsType == 2)
                {
                    builder.Append("" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "&nbsp;&nbsp;");
                }
                else
                {
                    if (img != 0)
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisongxinxi&amp;counts=" + counts + "&amp;usid=" + model.UsID + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=paisongxinxi&amp;counts=" + counts + "&amp;usid=" + model.UsID + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                }
            }
        }
        builder.Append("<br/>" + "奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");
        builder.Append(Out.Tab("</div>", ""));

        BCW.Draw.Model.DrawUser ontime = new BCW.Draw.BLL.DrawUser().GetOnTimebynum(counts);
        string statue = null;
        if (model.MyGoodsStatue == 0) { statue = "待确认信息"; }
        if (model.MyGoodsStatue == 3) { statue = "奖品准备中"; }
        if (model.MyGoodsStatue == 1) { statue = "奖品发货中"; }
        if (model.MyGoodsStatue == 2) { statue = "奖品已于" + model.InTime + "送达"; }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("中奖时间：" + ontime.OnTime + "<br />");
        builder.Append("奖品状态：" + statue + "<br />");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<h style=\"color:red\">【用户信息】</h><br />");
        builder.Append("<h style=\"color:blue\">用户ID：</h>" + model.UsID + "<br />");
        builder.Append("<h style=\"color:blue\">用户姓名：</h>" + model.RealName + "<br />");
        builder.Append("<h style=\"color:blue\">用户收货地址：</h>" + model.Address + "<br />");
        builder.Append("<h style=\"color:blue\">用户联系方式：</h>" + model.Phone + "<br />");
        builder.Append("<h style=\"color:blue\">用户邮箱：</h>" + model.Email + "<br />");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (model.Numbers != "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【快递信息】<br />");
            builder.Append("快递公司：<h style=\"color:red\">" + model.Express + "<br /></h>");
            builder.Append("快递单号：<h style=\"color:red\">" + model.Numbers + "<br /></h>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }

        builder.Append(Out.Tab("<div>", ""));
        if (model.MyGoodsStatue == 3)
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h><br />");
            builder.Append("若已查看用户信息，并已经将奖品送出（已快递），则进行信息录入，以便用户查询<br />");
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinxifankui&amp;counts=" + counts + "&amp;num=" + model.Num + "&amp;usid=" + model.UsID + "") + "\">&gt;&gt;&gt;信息反馈&lt;&lt;&lt;</a>");
        }
        if (model.MyGoodsStatue == 1)
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h><br />");
            builder.Append("若用户已经收到奖品并反馈信息，则进行信息录入，更改奖品状态<br />");
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinxifankui&amp;counts=" + counts + "&amp;num=" + model.Num + "&amp;usid=" + model.UsID + "") + "\">&gt;&gt;&gt;信息反馈&lt;&lt;&lt;</a>");
        }
        if (model.MyGoodsStatue == 2)
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h>");
            builder.Append("奖品已完成送达，不再需要信息反馈");
        }
        if (model.MyGoodsStatue == 0)
        {
            builder.Append("<h style=\"color:red\">【信息反馈】</h>");
            builder.Append("请等待用户确认信息");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //信息反馈
    private void FankuiPage()
    {

        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int counts = int.Parse(Utils.GetRequest("counts", "all", 1, @"^[0-9]\d*$", "0"));
        int num = int.Parse(Utils.GetRequest("num", "all", 1, @"^[0-9]\d*$", "0"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[0-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=paisong") + "\">奖品派送</a>&gt;信息反馈 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("奖品ID：" + num);
        builder.Append(Out.Tab("</div>", "<br />"));

        BCW.Draw.Model.DrawUser model = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(counts);
        if (model.MyGoodsStatue == 3)
        {
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            ub xml = new ub();
            //string xmlPath = "/Controls/Dawnlife.xml";
            Application.Remove(xmlPath);//清缓存
            xml.ReloadSub(xmlPath); //加载配置
            if (Utils.ToSChinese(ac) == "确定修改")
            {
                string Express = Utils.GetRequest("Express", "post", 2, @"^[^\^]{1,200}$", "快递公司填写出错");
                string Numbers = Utils.GetRequest("Numbers", "post", 2, @"^[^\^]{1,200}$", "快递单号填写错误");

                int Statue = 1;//确认派送
                new BCW.Draw.BLL.DrawUser().UpdateExpressbynum(num, Express, Numbers, Statue);//根据编号更新信息

                //发内线
                string strLog = "根据你在" + GameName + "获得奖品" + new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(num) + "，你的奖品已经发货，请耐心等待并注意查收" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
                new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

                Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=paisong&amp;counts=" + counts + "&amp;backurl=" + Utils.getPage(0) + ""), "2");
            }
            else
            {
                string strText = "快递公司:/,快递单号:/,";
                string strName = "Express,Numbers";
                string strType = "text,text";
                string strValu = "" + xml.dss["Express"] + "'" + xml.dss["Numbers"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确定修改,draw.aspx?act=xinxifankui&amp;num=" + num + "&amp;counts=" + counts + "&amp;usid=" + usid + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }
        }
        if (model.MyGoodsStatue == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("【信息录入】<br />");
            builder.Append("若收到用户确定收货的的信息，则修改奖品状态为已送达（此时表示该编号的奖品已经完成）");
            builder.Append(Out.Tab("</div>", ""));

            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定修改")
            {

                string MyGoodsStatue = Utils.GetRequest("MyGoodsStatue", "all", 2, @"^[0-9]\d*$", "奖品状态填写错误");
                //Utils.Error("" + counts, "");
                try
                {
                    //   model.MyGoodsStatue = Convert.ToInt32(MyGoodsStatue);
                    new BCW.Draw.BLL.DrawUser().UpdateMyGoodsStatue(model.GoodsCounts, Convert.ToInt32(MyGoodsStatue));

                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改成功!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=paisong&amp;backurl=" + Utils.PostPage(1) + "") + "\"><br />返回奖品派送</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));

                }
                catch
                {
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=xinxifankui&amp;counts=" + counts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                    builder.Append(Out.Tab("</div>", "<br/>"));
                }

            }
            else
            {
                //BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(id);
                builder.Append(Out.Tab("</div>", ""));
                string Text = "奖品状态:/,";
                string Name = "MyGoodsStatue";
                string Type = "select";
                string Valu = "" + model.MyGoodsStatue + "'" + Utils.getPage(0) + "";
                string Empt = "1|派送中|2|已送达";
                string Idea = "/";
                string Othe = "确定修改,draw.aspx?act=xinxifankui&amp;num=" + num + "&amp;counts=" + counts + "&amp;,post,1,red";
                builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));

            }
        }

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行榜单
    private void TopPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;排行榜单 ");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append("【排行榜】");
        builder.Append("<br />");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "get", 1, @"^[0-4]$", "1"));
        if (ptypec == 1)
            builder.Append("<b style=\"color:black\">抽奖" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=top&amp;ptypec=1&amp;id1=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">抽奖</a>" + "|");
        if (ptypec == 2)
            builder.Append("<b style=\"color:black\">兑换" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=top&amp;ptypec=2&amp;id1=" + ptypec + "&amp;backurl=" + Utils.getPage(0) + "") + "\">兑换</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string _where = "";
        string strWhere = "";
        string[] pageValUrl = { "act", "ptypec", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);


        if (ptypec == 1)
        {
            _where = "and R!=888";
            strWhere = "MyGoodsStatue!=88 and R!=888";
        }
        if (ptypec == 2)
        {
            _where = "and R='888'";
            strWhere = "MyGoodsStatue!=88 and R='888'";
        }

        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listUserTop = new BCW.Draw.BLL.DrawUser().GetUserTop(pageIndex, pageSize, _where, strWhere, out recordCount);
        if (listUserTop.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listUserTop)
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
                if (ptypec == 1)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>中奖" + n.aa + " 次");
                    builder.Append(".<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>");
                    builder.Append(" . <a href=\"" + Utils.getUrl("draw.aspx?act=jifenjiangli&amp;usid=" + n.UsID + "") + "\">奖励" + drawJifen + "</a>");
                }
                if (ptypec == 2)
                {
                    builder.Append("[第" + ((pageIndex - 1) * pageSize + k) + "名]<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + BCW.User.Users.SetUser(n.UsID) + "</a>" + drawJifen + "兑换奖品" + n.aa + " 次");
                    builder.Append(".<a href=\"" + Utils.getUrl("draw.aspx?act=case&amp;id=" + n.UsID + "&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">查看</a>");
                    builder.Append(" . <a href=\"" + Utils.getUrl("draw.aspx?act=jifenjiangli&amp;usid=" + n.UsID + "") + "\">奖励" + drawJifen + "</a>");
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


        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //排行记录
    private void CasePage()
    {
        //int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-3]$", "0"));
        int ptypec = Utils.ParseInt(Utils.GetRequest("ptypec", "all", 1, @"^[1-2]$", "1"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        Master.Title = ub.GetSub("drawName", xmlPath);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=top") + "\">排行榜单</a>&gt;记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex = 0;//当前页
        int recordCount;//记录总条数
        int pageSize = 10;//分页大小
        string strWhere = "";
        string strOrder = "";
        int iSCounts = 0;


        if (ptypec == 1)
        {
            strWhere = "UsID=" + id + " and MyGoodsStatue!=88 and R!=888";
        }
        if (ptypec == 2)
        {
            strWhere = "UsID=" + id + " and MyGoodsStatue!=88 and R='888'";
        }
        strOrder = "OnTime desc ";

        string[] pageValUrl = { "act", "ptypec", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        IList<BCW.Draw.Model.DrawUser> listdrawuser = new BCW.Draw.BLL.DrawUser().GetDrawUsers1(pageIndex, 10, strWhere, strOrder, iSCounts, out recordCount);

        if (listdrawuser.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawUser n in listdrawuser)
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
                int d = (pageIndex - 1) * 10 + k;
                string sText = string.Empty;
                if (ptypec == 1)
                {
                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "抽中ID为" + n.Num + "的奖品:" + n.MyGoods + "";
                }
                if (ptypec == 2)
                {

                    sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.UsName + "(" + n.UsID + ")" + "</a>" + "于" + Convert.ToDateTime(n.OnTime).ToString("yyyy-MM-dd HH:mm:ss") + "" + drawJifen + "兑换ID为" + n.Num + "的奖品:" + n.MyGoods + "";
                }
                builder.AppendFormat("<a href=\"" + Utils.getUrl("Draw.aspx?act=case&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"></a>" + d + sText);
                k++;

                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
        }

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=top&amp;ptypec=" + ptypec + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回排行</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理首页</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //管理
    private void GuanliPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;管理");
        builder.Append(Out.Tab("</div>", "<br />"));


        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-5]\d*$", "1"));
        builder.Append(Out.Tab("<div>", ""));
        if (ptype == 1)
            builder.Append("<b style=\"color:black\">总" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=guanli&amp;ptype=1") + "\">总</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">酷币" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=guanli&amp;ptype=2") + "\">酷币</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:black\">实物" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=guanli&amp;ptype=3") + "\">实物</a>" + "|");
        if (ptype == 4)
            builder.Append("<b style=\"color:black\">道具" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=guanli&amp;ptype=4") + "\">道具</a>" + "|");
        if (ptype == 5)
            builder.Append("<b style=\"color:black\">属性" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=guanli&amp;ptype=5") + "\">属性</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        try
        {
            //商品最大Id
            int newGoods = new BCW.Draw.BLL.DrawBox().GetMaxId();

            DateTime nowTime = DateTime.Now;
            BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(--newGoods);


            int pageIndex = 0;//当前页
            int recordCount;//记录总条数
            int pageSize = 10;//分页大小
            string strWhere = "";
            string strOrder = "";
            //查询条件

            if (ptype == 1)
            {
                strWhere = "GoodsType!='6'";
                strOrder = "GoodsCounts Desc";
            }
            if (ptype == 2)
            {
                strWhere = "GoodsType='0'";
                strOrder = "GoodsCounts Desc";
            }
            if (ptype == 3)
            {
                strWhere = "GoodsType='1'";
                strOrder = "GoodsCounts Desc";
            }
            if (ptype == 4)
            {
                strWhere = "GoodsType='2'";
                strOrder = "GoodsCounts Desc";
            }
            if (ptype == 5)
            {
                strWhere = "GoodsType='3'";
                strOrder = "GoodsCounts Desc";
            }

            string[] pageValUrl = { "ac", "act", "ptype", "uid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;


            // 开始读取列表
            IList<BCW.Draw.Model.DrawBox> listbox = new BCW.Draw.BLL.DrawBox().GetDrawBoxs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("总量：" + listbox.Count + "");
            //builder.Append(Out.Tab("</div>", "<br />")); 
            if (listbox.Count > 0)
            {
                int k = 1;
                foreach (BCW.Draw.Model.DrawBox n in listbox)
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
                    id = n.GoodsCounts;
                    string strg = string.Empty;
                    if (n.points != 0) { strg = "兑"; }
                    else { strg = "抽"; }
                    builder.Append("" + id + ".<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.GoodsName + "</a>(" + strg + ")");

                    if (n.Statue == 5)
                    {
                        builder.Append("(已下架)");
                    }
                    if (n.Statue == 1)
                    {
                        builder.Append("(已结束)");
                    }
                    builder.Append("  <a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + n.GoodsCounts + "&amp;rank=" + n.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a>");

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

        }
        catch
        {
            builder.Append("没有更多正在进行中的奖品记录...");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("温馨提示：奖品名字后面带的（抽）表示此奖品为抽奖活动奖品，（兑）表示此奖品为兑换活动的奖品<br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖品管理
    private void JiangpinPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;奖品管理");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addjiangji") + "\">奖级设置</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=chi") + "\">奖池管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods1") + "\">添加酷币</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods2") + "\">添加实物</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods3") + "\">添加道具</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods4") + "\">添加属性</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinlun") + "\">重置奖箱</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=guanli&amp;backurl=" + Utils.PostPage(1) + "") + "\">[ 管   理 ]</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("" + "-----------" + "");
        builder.Append(Out.Tab("</div>", ""));

        string strText = "输入奖品编号:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜奖品,draw.aspx?act=update&amp;u=" + Utils.getstrU() + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", Out.Hr()));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("正在进行中的奖品：");
        builder.Append(Out.Tab("</div>", "<br />"));

        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-5]\d*$", "1"));
        int ptypeb = Utils.ParseInt(Utils.GetRequest("ptypeb", "all", 1, @"^[1-2]\d*$", "1"));
        builder.Append(Out.Tab("<div>", ""));

        if (ptypeb == 1)
            builder.Append("<b style=\"color:red\">抽奖奖品</b>|<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptypeb=2&amp;ptype=" + ptype + "") + "\">兑换奖品</a>" + "<br />");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptypeb=1&amp;ptype=" + ptype + "") + "\">抽奖奖品</a>" + "|<b style=\"color:red\">兑换奖品</b><br />");

        if (ptype == 1)
            builder.Append("<b style=\"color:black\">总" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptype=1&amp;ptypeb=" + ptypeb + "") + "\">总</a>" + "|");
        if (ptype == 2)
            builder.Append("<b style=\"color:black\">酷币" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptype=2&amp;ptypeb=" + ptypeb + "") + "\">酷币</a>" + "|");
        if (ptype == 3)
            builder.Append("<b style=\"color:black\">实物" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptype=3&amp;ptypeb=" + ptypeb + "") + "\">实物</a>" + "|");
        if (ptype == 4)
            builder.Append("<b style=\"color:black\">道具" + "</b>" + "|");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptype=4&amp;ptypeb=" + ptypeb + "") + "\">道具</a>" + "|");
        if (ptype == 5)
            builder.Append("<b style=\"color:black\">属性" + "</b>" + "");
        else
            builder.Append("<a href=\"" + Utils.getUrl("Draw.aspx?act=jiangpin&amp;ptype=5&amp;ptypeb=" + ptypeb + "") + "\">属性</a>" + "");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div>", ""));
        try
        {
            //商品最大Id
            //int newGoods = new BCW.BLL.GoodsList().GetMaxId();
            int newGoods = new BCW.Draw.BLL.DrawBox().GetMaxId();
            //路径
            //string Logo = ub.GetSub("img", xmlPath);
            //显示商品列表记录数
            int ListCount = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            //显示最大图片量
            string imgCount = ub.GetSub("imgCount", xmlPath);
            DataSet das = null;
            if (ptypeb == 1)
                das = new BCW.Draw.BLL.DrawBox().GetList("ID", "Statue=0 and Points=0  Order by Rank Asc,ID Desc");
            else
                das = new BCW.Draw.BLL.DrawBox().GetList("ID", "Statue=0  and Points!=0   Order by Rank Asc,ID Desc");
            if (das != null && das.Tables[0].Rows.Count > 0)
            {
                for (int lists = 0; lists < das.Tables[0].Rows.Count; lists++)
                {
                    int uu = int.Parse(das.Tables[0].Rows[lists][0].ToString());
                    DateTime nowTime = DateTime.Now;
                    BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBox(uu);
                    string type = "";
                    if (model.GoodsType == 0) { type = "酷币"; }
                    if (model.GoodsType == 1) { type = "实物"; }
                    if (model.GoodsType == 2) { type = "道具"; }
                    if (model.GoodsType == 3) { type = "属性"; }

                    string rank = "";
                    int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
                    for (int i = 0; i < j; i++)
                    {
                        if (i < 20)
                        {
                            if (model.rank == 0) { rank = "一等奖"; }
                            if (model.rank == 1) { rank = "二等奖"; }
                            if (model.rank == 2) { rank = "三等奖"; }
                            if (model.rank == 3) { rank = "四等奖"; }
                            if (model.rank == 4) { rank = "五等奖"; }
                            if (model.rank == 5) { rank = "六等奖"; }
                            if (model.rank == 6) { rank = "七等奖"; }
                            if (model.rank == 7) { rank = "八等奖"; }
                            if (model.rank == 8) { rank = "九等奖"; }
                            if (model.rank == 9) { rank = "十等奖"; }
                            if (model.rank == 10) { rank = "十一等奖"; }
                            if (model.rank == 11) { rank = "十二等奖"; }
                            if (model.rank == 12) { rank = "十三等奖"; }
                            if (model.rank == 13) { rank = "十四等奖"; }
                            if (model.rank == 14) { rank = "十五等奖"; }
                            if (model.rank == 15) { rank = "十六等奖"; }
                            if (model.rank == 16) { rank = "十七等奖"; }
                            if (model.rank == 17) { rank = "十八等奖"; }
                            if (model.rank == 18) { rank = "十九等奖"; }
                            if (model.rank == 19) { rank = "二十等奖"; }
                            if (model.rank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                        }
                        else
                        {
                            if (model.rank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                            else
                            {
                                if (model.rank == i) { rank = (i + 1) + "等奖"; }
                            }
                        }

                    }


                    //是否为进行中商品   
                    if (model.Statue == 0)
                    {
                        if (ptype == 1)
                        {
                            builder.Append((lists + 1) + ".");
                            builder.Append("奖品等级：" + rank + "" + "<br />");
                            builder.Append("&nbsp;&nbsp;&nbsp;奖品编号：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsCounts + "</a>" + "<br />");
                            if (model.GoodsType == 0)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;酷币价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                            }
                            if (model.GoodsType == 1)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                //builder.Append("&nbsp;&nbsp;&nbsp;奖品价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                            }
                            if (model.GoodsType == 2)
                            {
                                string GN = new BCW.Draw.BLL.DrawDS().GetGN(model.GoodsCounts);
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "(" + GN + ")" + "<br />");
                                //builder.Append("&nbsp;&nbsp;&nbsp;道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                            }
                            if (model.GoodsType == 3)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "+" + model.GoodsValue + " <br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                            }
                            if (model.points == 0)
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                            }
                            else
                            {
                                builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                            }
                            //lists++;
                            if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                            {
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                builder.Append("<br/>");
                                string[] imgNum = model.GoodsImg.Split(',');
                                // foreach (string n in imgNum)
                                for (int c = 0; c < imgNum.Length - 1; c++)
                                {
                                    if (model.GoodsType == 2)
                                    {
                                        builder.Append("&nbsp;&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                    }
                                    else
                                    {
                                        if (id == model.GoodsCounts)
                                        {
                                            if (img != 0)
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                            else
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                        }
                                    }
                                }
                            }
                            builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

                            builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "") + "\">查看属性</a>" + "   ");
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + model.GoodsCounts + "&amp;rank=" + model.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br/>");
                        }
                        if (ptype == 2)
                        {
                            if (model.GoodsType == 0)
                            {
                                builder.Append((lists + 1) + ".");
                                builder.Append("奖品等级：" + rank + "" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品编号：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsCounts + "</a>" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;酷币价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                                if (model.points == 0)
                                {
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                }
                                else
                                {
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                }
                                //lists++;
                                if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                                {
                                    builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                    builder.Append("<br/>");
                                    string[] imgNum = model.GoodsImg.Split(',');
                                    // foreach (string n in imgNum)
                                    for (int c = 0; c < imgNum.Length - 1; c++)
                                    {
                                        if (model.GoodsType == 2)
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            if (id == model.GoodsCounts)
                                            {
                                                if (img != 0)
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                                else
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                            else
                                            {
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                        }
                                    }
                                }
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "") + "\">查看属性</a>" + "   ");
                                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + model.GoodsCounts + "&amp;rank=" + model.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br/>");
                            }
                        }
                        if (ptype == 3)
                        {
                            if (model.GoodsType == 1)
                            {
                                builder.Append((lists + 1) + ".");
                                builder.Append("奖品等级：" + rank + "" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品编号：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsCounts + "</a>" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                                if (model.points == 0)
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                else
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                //lists++;
                                if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                                {
                                    builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                    builder.Append("<br/>");
                                    string[] imgNum = model.GoodsImg.Split(',');
                                    // foreach (string n in imgNum)
                                    for (int c = 0; c < imgNum.Length - 1; c++)
                                    {
                                        if (model.GoodsType == 2)
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            if (id == model.GoodsCounts)
                                            {
                                                if (img != 0)
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                                else
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                            else
                                            {
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                        }
                                    }
                                }
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "") + "\">查看属性</a>" + "   ");
                                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + model.GoodsCounts + "&amp;rank=" + model.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br/>");
                            }
                        }
                        if (ptype == 4)
                        {
                            if (model.GoodsType == 2)
                            {
                                builder.Append((lists + 1) + ".");
                                builder.Append("奖品等级：" + rank + "" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品编号：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsCounts + "</a>" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                //builder.Append("&nbsp;&nbsp;&nbsp;道具价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                                if (model.points == 0)
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                else
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                //lists++;
                                if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                                {
                                    builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                    builder.Append("<br/>");
                                    string[] imgNum = model.GoodsImg.Split(',');
                                    // foreach (string n in imgNum)
                                    for (int c = 0; c < imgNum.Length - 1; c++)
                                    {
                                        if (model.GoodsType == 2)
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            if (id == model.GoodsCounts)
                                            {
                                                if (img != 0)
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                                else
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                            else
                                            {
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                        }
                                    }
                                }
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "") + "\">查看属性</a>" + "   ");
                                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + model.GoodsCounts + "&amp;rank=" + model.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br/>");
                            }
                        }
                        if (ptype == 5)
                        {
                            if (model.GoodsType == 3)
                            {
                                builder.Append((lists + 1) + ".");
                                builder.Append("奖品等级：" + rank + "" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品编号：" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + model.GoodsCounts + "</a>" + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品名称：" + model.GoodsName + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;奖品类型：" + type + "<br />");
                                builder.Append("&nbsp;&nbsp;&nbsp;属性价值：" + model.GoodsValue + " " + model.beizhu + "<br />");
                                if (model.points == 0)
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + "不可兑换" + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                else
                                    builder.Append("&nbsp;&nbsp;&nbsp;兑换" + drawJifen + ": " + model.points + "<br />&nbsp;&nbsp;&nbsp;奖品余量: " + model.GoodsNum + "");
                                //lists++;
                                if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                                {
                                    builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;图片描述:");
                                    builder.Append("<br/>");
                                    string[] imgNum = model.GoodsImg.Split(',');
                                    // foreach (string n in imgNum)
                                    for (int c = 0; c < imgNum.Length - 1; c++)
                                    {
                                        if (model.GoodsType == 2)
                                        {
                                            builder.Append("&nbsp;&nbsp;&nbsp;" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "</a>" + "&nbsp;&nbsp;");
                                        }
                                        else
                                        {
                                            if (id == model.GoodsCounts)
                                            {
                                                if (img != 0)
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                                else
                                                    builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                            else
                                            {
                                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;id=" + model.GoodsCounts + "&amp;ptype=" + ptype + "&amp;ptypeb=" + ptypeb + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                                            }
                                        }
                                    }
                                }
                                builder.Append("<br/>" + "&nbsp;&nbsp;&nbsp;奖品描述：" + Out.SysUBB(model.Explain) + "<br/>");

                                builder.Append("&nbsp;&nbsp;&nbsp;<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + model.GoodsCounts + "") + "\">查看属性</a>" + "   ");
                                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=del&amp;counts=" + model.GoodsCounts + "&amp;rank=" + model.rank + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">[删]</a><br/>");
                            }
                        }

                    }
                }
            }
        }
        catch
        {
            builder.Append("没有更多正在进行中的奖品记录...");
        }
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\">", Out.Hr()));
        builder.Append("温馨提示：抽奖奖品表示奖品只参与抽奖活动，兑换奖品表示奖品只参与兑换活动<br />");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //添加酷币 
    private void AddGoods1Page()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "添加奖箱奖品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;添加酷币");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "奖品名称限1-20字内");
            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
            string beizhu = Utils.GetRequest("beizhu", "post", 3, @"^[^\^]{1,2000}$", "奖品备注限2000字内(为奖品市值单位)");
            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "奖品类型选择出错");
            string Rank = Utils.GetRequest("Rank", "post", 2, @"^[0-9]\d*$", "奖品等级选择出错");
            string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[0-9]\d*$", "酷币值填写错误(整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "添加奖品量填写错误(整数)");
            string points = Utils.GetRequest("points", "post", 4, @"^[0-9]\d*$", "兑换奖品" + drawJifen + "填写错误（正整数）");
            int lun = Convert.ToInt32(ub.GetSub("lun", xmlPath));

            if (new BCW.Draw.BLL.DrawBox().Exists(1))
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = Convert.ToInt32(GoodsType);
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = counts.GoodsCounts + 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);
                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        if (mode.GoodsCounts == 0)
                        {
                            mode.ID = mode.ID;
                            mode.GoodsCounts = model.GoodsCounts;
                            mode.Rank = Convert.ToInt32(Rank);
                            new BCW.Draw.BLL.DrawTen().Update(mode);

                            new BCW.Draw.BLL.DrawBox().Add(model);
                            int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            Utils.Error("奖品等级选择出错，请重新选择..", "");
                        }
                        //--------------------------------------------------------------
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }
            }
            else
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = Convert.ToInt32(GoodsType);
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);

                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        mode.ID = mode.ID;
                        mode.GoodsCounts = model.GoodsCounts;
                        mode.Rank = Convert.ToInt32(Rank);
                        new BCW.Draw.BLL.DrawTen().Update(mode);
                        //--------------------------------------------------------------

                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }
            }
        }
        else
        {
            int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            string sr = "r";
            string ww = string.Empty;
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                if (i == 0)
                {
                    ww += i + "|" + str;
                }
                else
                {
                    ww += "|" + i + "|" + str;
                }
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("添加奖品请选择为【抽奖】或者【兑换】奖品<br />");
            if (ptype == 0)
                builder.Append("<b style=\"color:red\">添加抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods1&amp;ptype=1") + "\">添加兑换奖品</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods1&amp;ptype=0") + "\">添加抽奖奖品</a>|<b style=\"color:red\">添加兑换奖品</b>");
            builder.Append(Out.Tab("</div>", ""));

            if (ptype == 0)
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,酷币值:/,酷币单位:/,,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,GoodsValue,beizhu,points";
                string strType = "text,textarea,num,select,select,select,text,hidden,textarea,act";
                string strValu = "" + "酷币" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 0 + "'" + "" + "'" + "酷币" + "'" + 0 + "'";
                string strEmpt = "false,true,false,0|酷币," + ww + ",50|50|100|100|200|200|300|300|500|500|1000|1000,true,false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods1&amp;ptype=" + 0 + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,,酷币值:/,酷币单位:/,奖品兑换" + drawJifen + "值:/,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,GoodsValue,beizhu,points";
                string strType = "text,textarea,num,select,hidden,select,text,num,textarea,act";
                string strValu = "" + "酷币" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 1000 + "'" + "" + "'" + "酷币" + "'" + "" + "'";
                string strEmpt = "false,true,false,0|酷币,false,50|50|100|100|200|200|300|300|500|500|1000|1000,true,false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods1&amp;ptype=" + 1 + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:若要添加任意的酷币值，请手动添加酷币值。【抽奖】表示添加的奖品只参与抽奖活动，【兑换】表示添加的奖品只参加兑换活动<br />");
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx?act=addgoods5") + "\">添加任意值酷币</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //添加酷币手动值 
    private void AddGoods5Page()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "添加奖箱奖品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;添加酷币");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "奖品名称限1-20字内");
            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
            string beizhu = Utils.GetRequest("beizhu", "post", 3, @"^[^\^]{1,2000}$", "奖品备注限2000字内(为奖品市值单位)");
            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "奖品类型选择出错");
            string Rank = Utils.GetRequest("Rank", "post", 2, @"^[0-9]\d*$", "奖品等级选择出错");
            string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[1-9]\d*$", "酷币值填写错误(整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "添加奖品量填写错误(整数)");
            string points = Utils.GetRequest("points", "post", 4, @"^[0-9]\d*$", "兑换奖品" + drawJifen + "填写错误（正整数）");
            int lun = Convert.ToInt32(ub.GetSub("lun", xmlPath));

            if (new BCW.Draw.BLL.DrawBox().Exists(1))
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = Convert.ToInt32(GoodsType);
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = counts.GoodsCounts + 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);
                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        if (mode.GoodsCounts == 0)
                        {
                            mode.ID = mode.ID;
                            mode.GoodsCounts = model.GoodsCounts;
                            mode.Rank = Convert.ToInt32(Rank);
                            new BCW.Draw.BLL.DrawTen().Update(mode);

                            new BCW.Draw.BLL.DrawBox().Add(model);
                            int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            Utils.Error("奖品等级选择出错，请重新选择..", "");
                        }
                        //--------------------------------------------------------------
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {

                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
            else
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = Convert.ToInt32(GoodsType);
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);

                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        mode.ID = mode.ID;
                        mode.GoodsCounts = model.GoodsCounts;
                        mode.Rank = Convert.ToInt32(Rank);
                        new BCW.Draw.BLL.DrawTen().Update(mode);
                        //--------------------------------------------------------------

                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
        }
        else
        {
            int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            string sr = "r";
            string ww = string.Empty;
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                if (i == 0)
                {
                    ww += i + "|" + str;
                }
                else
                {
                    ww += "|" + i + "|" + str;
                }
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("添加奖品请选择为【抽奖】或者【兑换】奖品<br />");
            if (ptype == 0)
                builder.Append("<b style=\"color:red\">添加抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods5&amp;ptype=1") + "\">添加兑换奖品</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods5&amp;ptype=0") + "\">添加抽奖奖品</a>|<b style=\"color:red\">添加兑换奖品</b>");
            builder.Append(Out.Tab("</div>", ""));

            if (ptype == 0)
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,酷币值:/,酷币单位:/,,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,GoodsValue,beizhu,points";
                string strType = "text,textarea,num,select,select,num,text,hidden,textarea,act";
                string strValu = "" + "酷币" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 0 + "'" + "" + "'" + "酷币" + "'" + 0 + "'";
                string strEmpt = "false,true,false,0|酷币," + ww + ",false,true,false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods1&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,,酷币值:/,酷币单位:/,奖品兑换" + drawJifen + "值:/,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,GoodsValue,beizhu,points";
                string strType = "text,textarea,num,select,hidden,num,text,num,textarea,act";
                string strValu = "" + "酷币" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 1000 + "'" + "" + "'" + "酷币" + "'" + "" + "'";
                string strEmpt = "false,true,false,0|酷币,false,false,true,false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods1&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:【抽奖】表示添加的奖品只参与抽奖活动，【兑换】表示添加的奖品只参加兑换活动<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //添加实物 
    private void AddGoods2Page()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "添加奖箱奖品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;添加实物");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "奖品名称限1-20字内");
            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
            //string beizhu = Utils.GetRequest("beizhu", "post", 3, @"^[^\^]{1,2000}$", "奖品备注限2000字内(为奖品市值单位)");
            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "奖品类型选择出错");
            string Rank = Utils.GetRequest("Rank", "post", 2, @"^[0-9]\d*$", "奖品等级选择出错");
            //string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[0-9]\d*$", "奖品总价值填写错误(整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "添加奖品量填写错误(整数)");
            string points = Utils.GetRequest("points", "post", 4, @"^[0-9]\d*$", "兑换奖品" + drawJifen + "填写错误（正整数）");
            int lun = Convert.ToInt32(ub.GetSub("lun", xmlPath));

            if (new BCW.Draw.BLL.DrawBox().Exists(1))
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = 1;
                model.GoodsValue = 0;
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = counts.GoodsCounts + 1;
                model.Statue = 0;
                model.beizhu = "";
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);
                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        if (mode.GoodsCounts == 0)
                        {
                            mode.ID = mode.ID;
                            mode.GoodsCounts = model.GoodsCounts;
                            mode.Rank = Convert.ToInt32(Rank);
                            new BCW.Draw.BLL.DrawTen().Update(mode);

                            new BCW.Draw.BLL.DrawBox().Add(model);
                            int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            Utils.Error("奖品等级选择出错，请重新选择..", "");
                        }
                        //--------------------------------------------------------------
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }
            }
            else
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                model.GoodsName = GoodsName;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = 1;
                model.GoodsValue = 0;
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = 1;
                model.Statue = 0;
                model.beizhu = "";
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);

                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        mode.ID = mode.ID;
                        mode.GoodsCounts = model.GoodsCounts;
                        mode.Rank = Convert.ToInt32(Rank);
                        new BCW.Draw.BLL.DrawTen().Update(mode);
                        //--------------------------------------------------------------

                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
        }
        else
        {
            int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            string sr = "r";
            string ww = string.Empty;
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                if (i == 0)
                {
                    ww += i + "|" + str;
                }
                else
                {
                    ww += "|" + i + "|" + str;
                }
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("添加奖品请选择为【抽奖】或者【兑换】奖品<br />");
            if (ptype == 0)
                builder.Append("<b style=\"color:red\">添加抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods2&amp;ptype=1") + "\">添加兑换奖品</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods2&amp;ptype=0") + "\">添加抽奖奖品</a>|<b style=\"color:red\">添加兑换奖品</b>");
            builder.Append(Out.Tab("</div>", ""));

            if (ptype == 0)
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,points";
                string strType = "text,textarea,num,select,select,hidden,textarea,act";
                string strValu = "" + "标题" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 0 + "'" + 0 + "'" + " " + "'" + "" + "'";
                string strEmpt = "false,true,false,1|实物," + ww + ",false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods2&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "奖品名称:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,,奖品兑换" + drawJifen + "值:/,";
                string strName = "GoodsName,explain,GoodsNum,GoodsType,Rank,points";
                string strType = "text,textarea,num,select,hidden,num,textarea,act";
                string strValu = "" + "标题" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 1000 + "'" + "" + "'" + " " + "'" + "" + "'";
                string strEmpt = "false,true,false,1|实物,true,false";
                string strIdea = "/";
                string strOthe = "确定添加|reset,draw.aspx?act=addgoods2&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:【抽奖】表示添加的奖品只参与抽奖活动，【兑换】表示添加的奖品只参加兑换活动<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //添加道具 
    private void AddGoods3Page()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "添加奖箱奖品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;添加道具");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        ub xml = new ub();
        //string xmlPath = "/Controls/draw.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定选择")
        {
            string gamename = Utils.GetRequest("gamename", "post", 3, @"^[^\^]{1,200}$", "游戏选择出错");

            xml.dss["gamename"] = gamename;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
            {
                Utils.Success("" + GameName + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=addgoods3&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "1");
            }
            else
            {
                Utils.Success("" + GameName + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=addgoods3&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(0) + ""), "0");
            }
        }
        else
        {
            string strText = "选择游戏:/,";
            string strName = "gamename,backurl";
            string strType = "select,hidden";
            string strValu = "" + xml.dss["gamename"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "农场|农场";
            string strIdea = "/";
            string strOthe = "确定选择,draw.aspx?act=addgoods3&amp;ptype=" + ptype + ",post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string Dj = Utils.GetRequest("Dj", "post", 4, @"^[0-9]\d*$", "道具选择");
            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
            string beizhu = Utils.GetRequest("beizhu", "post", 3, @"^[^\^]{1,2000}$", "奖品备注限2000字内(为奖品市值单位)");
            string GoodsType = Utils.GetRequest("GoodsType", "post", 2, @"^[0-9]$", "奖品类型选择出错");
            string Rank = Utils.GetRequest("Rank", "post", 2, @"^[0-9]\d*$", "奖品等级选择出错");
            //string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[0-9]\d*$", "奖品总价值填写错误(整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "添加奖品量填写错误(整数)");
            string points = Utils.GetRequest("points", "post", 4, @"^[0-9]\d*$", "兑换奖品" + drawJifen + "填写错误（正整数）");
            int lun = Convert.ToInt32(ub.GetSub("lun", xmlPath));

            if (new BCW.Draw.BLL.DrawBox().Exists(1))
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                BCW.farm.Model.NC_daoju ui = new BCW.farm.BLL.NC_daoju().GetNC_daoju(Convert.ToInt32(Dj));
                model.GoodsName = ui.name;//根据道具ID取道具名
                model.Explain = explain;
                model.GoodsImg = ui.picture + ",";
                model.GoodsType = 2;
                model.GoodsValue = 0;
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = counts.GoodsCounts + 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                string gamename = xml.dss["gamename"].ToString();
                BCW.Draw.Model.DrawDS DS = new BCW.Draw.Model.DrawDS();
                DS.GoodsCounts = model.GoodsCounts;
                DS.gamename = gamename;
                DS.DS = model.GoodsName;
                DS.DSID = Convert.ToInt32(Dj);
                DS.one = "";
                DS.two = "";
                DS.three = "";
                new BCW.Draw.BLL.DrawDS().Add(DS);

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);
                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        if (mode.GoodsCounts == 0)
                        {
                            mode.ID = mode.ID;
                            mode.GoodsCounts = model.GoodsCounts;
                            mode.Rank = Convert.ToInt32(Rank);
                            new BCW.Draw.BLL.DrawTen().Update(mode);

                            new BCW.Draw.BLL.DrawBox().Add(model);
                            int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("温馨提示：农场游戏道具自带图片，不需要添加，请选择否<br />");
                            builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>");//<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a>
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            Utils.Error("奖品等级选择出错，请重新选择..", "");
                        }
                        //--------------------------------------------------------------
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("温馨提示：农场游戏道具自带图片，不需要添加，请选择否<br />");
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "");//<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
            else
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                BCW.farm.Model.NC_daoju ui = new BCW.farm.BLL.NC_daoju().GetNC_daoju(Convert.ToInt32(Dj));
                model.GoodsName = ui.name;//道具名
                model.Explain = explain;
                model.GoodsImg = ui.picture + ",";
                model.GoodsType = 2;
                model.GoodsValue = 0;
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = 1;
                model.Statue = 0;
                model.beizhu = beizhu;
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                string gamename = xml.dss["gamename"].ToString();
                BCW.Draw.Model.DrawDS DS = new BCW.Draw.Model.DrawDS();
                DS.GoodsCounts = model.GoodsCounts;
                DS.gamename = gamename;
                DS.DS = model.GoodsName;
                DS.DSID = Convert.ToInt32(Dj);
                DS.one = "";
                DS.two = "";
                DS.three = "";
                new BCW.Draw.BLL.DrawDS().Add(DS);

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);

                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        mode.ID = mode.ID;
                        mode.GoodsCounts = model.GoodsCounts;
                        mode.Rank = Convert.ToInt32(Rank);
                        new BCW.Draw.BLL.DrawTen().Update(mode);
                        //--------------------------------------------------------------

                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("温馨提示：农场游戏道具自带图片，不需要添加，请选择否<br />");
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "");//<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("温馨提示：农场游戏道具自带图片，不需要添加，请选择否<br />");
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "");//<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {
                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
        }
        else
        {
            int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            string sr = "r";
            string wwr = string.Empty;
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                if (i == 0)
                {
                    wwr += i + "|" + str;
                }
                else
                {
                    wwr += "|" + i + "|" + str;
                }
            }

            string gamename = xml.dss["gamename"].ToString();
            if (gamename == "农场")
            {
                //根据道具表拿出道具ID 与道具名
                string some = string.Empty;
                DataSet ds = new BCW.farm.BLL.NC_daoju().GetList("*", "id!=''");
                int aa = ds.Tables[0].Rows.Count;
                string[] yy = new string[aa];
                string[] ww = new string[aa];
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string id = (ds.Tables[0].Rows[i]["id"].ToString());
                        string zuowu = ds.Tables[0].Rows[i]["name"].ToString().Trim();
                        string names = string.Empty;
                        string ids = string.Empty;
                        names = names + zuowu;
                        ids = ids + id;
                        yy[i] = ids;
                        ww[i] = names;

                    }
                }
                for (int i = 0; i < aa; i++)
                {
                    if (i < aa - 1)
                    {
                        some = some + yy[i] + "|" + ww[i] + "|";
                    }
                    else
                        some = some + yy[i] + "|" + ww[i];
                }


                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("添加奖品请选择为【抽奖】或者【兑换】奖品<br />");
                if (ptype == 0)
                    builder.Append("<b style=\"color:red\">添加抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods3&amp;ptype=1") + "\">添加兑换奖品</a>");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods3&amp;ptype=0") + "\">添加抽奖奖品</a>|<b style=\"color:red\">添加兑换奖品</b>");
                builder.Append(Out.Tab("</div>", ""));

                if (ptype == 0)
                {
                    string strText = "选择道具:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,,";
                    string strName = "Dj,explain,GoodsNum,GoodsType,Rank,points";
                    string strType = "select,textarea,num,select,select,hidden,textarea,act";
                    string strValu = "" + "" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 0 + "'" + 0 + "'";
                    string strEmpt = "" + some + ",true,false,2|道具," + wwr + ",false";
                    string strIdea = "/";
                    string strOthe = "确定添加,draw.aspx?act=addgoods3&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
                else
                {
                    string strText = "选择道具:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,,奖品兑换" + drawJifen + "值:/,";
                    string strName = "Dj,explain,GoodsNum,GoodsType,Rank,points";
                    string strType = "select,textarea,num,select,hidden,num,textarea,act";
                    string strValu = "" + "" + "'" + "奖品详情..." + "'" + "" + "'" + 0 + "'" + 1000 + "'" + "" + "'";
                    string strEmpt = "" + some + ",true,false,2|道具,false,false";
                    string strIdea = "/";
                    string strOthe = "确定添加,draw.aspx?act=addgoods3&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            //if (gamename == "闯荡")
            //{
            //    string strText = "选择道具:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,奖品市价值:/,市值单位(可留空):/,奖品兑换" + drawJifen + "值:/,";
            //    string strName = "Dj,explain,GoodsNum,GoodsType,Rank,GoodsValue,beizhu,points";
            //    string strType = "select,textarea,num,select,select,text,text,num,textarea,act";
            //    string strValu = "" + 0 + "'" + "奖品详情..." + "'" + 1 + "'" + 0 + "'" + 0 + "'" + 0 + "'" + " " + "'" + "" + "'";
            //    string strEmpt = "假酒|假酒|走私汽车|走私汽车|如来神掌|如来神掌,true,true,2|道具,0|" + r1 + " |1|" + r2 + "|2|" + r3 + "|3|" + r4 + "|4|" + r5 + "|5|" + r6 + "|6|" + r7 + "|7|" + r8 + "|8|" + r9 + "|9|" + r10 + ",false,true,false";
            //    string strIdea = "/";
            //    string strOthe = "确定添加,draw.aspx?act=addgoods3&amp;backurl=" + Utils.getPage(1) + ",post,1,red";
            //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            //}

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:【抽奖】表示添加的奖品只参与抽奖活动，【兑换】表示添加的奖品只参加兑换活动<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //添加属性
    private void AddGoods4Page()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "添加奖箱奖品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;添加属性");

        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定添加")
        {
            string GoodsName = Utils.GetRequest("GoodsName", "post", 4, @"^[0-9]\d*$", "属性选择出错");
            string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
            string beizhu = Utils.GetRequest("beizhu", "post", 3, @"^[^\^]{1,2000}$", "奖品备注限2000字内(为奖品市值单位)");
            //string GoodsType = Utils.GetRequest("GoodsType", "post", 3, @"^[0-9]\d*$", "奖品类型选择出错");
            string Rank = Utils.GetRequest("Rank", "post", 2, @"^[0-9]\d*$", "奖品等级选择出错");
            string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[1-9]\d*$", "属性值填写错误(正整整数)");
            string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[1-9]\d*$", "添加奖品量填写错误(整数)");
            string points = Utils.GetRequest("points", "post", 4, @"^[0-9]\d*$", "兑换奖品" + drawJifen + "填写错误（正整数）");
            int lun = Convert.ToInt32(ub.GetSub("lun", xmlPath));

            if (new BCW.Draw.BLL.DrawBox().Exists(1))
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                string GoodsName1 = string.Empty;
                if (GoodsName == "0") { GoodsName1 = "体力"; }
                if (GoodsName == "1") { GoodsName1 = "魅力"; }
                if (GoodsName == "2") { GoodsName1 = "智慧"; }
                if (GoodsName == "3") { GoodsName1 = "威望"; }
                if (GoodsName == "4") { GoodsName1 = "邪恶"; }
                model.GoodsName = GoodsName1;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = 3;
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = counts.GoodsCounts + 1;
                model.Statue = 0;
                model.beizhu = "";
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);
                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        if (mode.GoodsCounts == 0)
                        {
                            mode.ID = mode.ID;
                            mode.GoodsCounts = model.GoodsCounts;
                            mode.Rank = Convert.ToInt32(Rank);
                            new BCW.Draw.BLL.DrawTen().Update(mode);

                            new BCW.Draw.BLL.DrawBox().Add(model);
                            int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                            builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            Utils.Error("奖品等级选择出错，请重新选择..", "");
                        }
                        //--------------------------------------------------------------
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + model.GoodsCounts + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {

                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
            else
            {
                int Id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                BCW.Draw.Model.DrawBox counts = new BCW.Draw.BLL.DrawBox().GetDrawBox(Id);

                string GoodsName1 = string.Empty;
                if (GoodsName == "0") { GoodsName1 = "体力"; }
                if (GoodsName == "1") { GoodsName1 = "魅力"; }
                if (GoodsName == "2") { GoodsName1 = "智慧"; }
                if (GoodsName == "3") { GoodsName1 = "威望"; }
                if (GoodsName == "4") { GoodsName1 = "邪恶"; }
                model.GoodsName = GoodsName1;
                model.Explain = explain;
                model.GoodsImg = "0";
                model.GoodsType = 3;
                model.GoodsValue = Convert.ToInt32(GoodsValue);
                model.GoodsNum = Convert.ToInt32(GoodsNum);
                model.AddTime = DateTime.Now;
                model.OverTime = DateTime.Now;
                model.ReceiveTime = DateTime.Now;
                model.GoodsCounts = 1;
                model.Statue = 0;
                model.beizhu = "";
                model.rank = Convert.ToInt32(Rank);
                model.points = Convert.ToInt32(points);
                model.AllNum = Convert.ToInt32(GoodsNum);
                if (ptype == 0)
                    model.lun = lun;

                try
                {
                    if (ptype == 0)
                    {
                        //是否刷屏
                        string appName = "LIGHT_BRAG";
                        int Expir = 5;
                        BCW.User.Users.IsFresh(appName, Expir);

                        ////-----------------------------------------------------------
                        int rank = Convert.ToInt32(Rank);
                        BCW.Draw.Model.DrawTen mode = new BCW.Draw.BLL.DrawTen().GetDrawTen(rank);
                        mode.ID = mode.ID;
                        mode.GoodsCounts = model.GoodsCounts;
                        mode.Rank = Convert.ToInt32(Rank);
                        new BCW.Draw.BLL.DrawTen().Update(mode);
                        //--------------------------------------------------------------

                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        new BCW.Draw.BLL.DrawBox().Add(model);
                        int id = new BCW.Draw.BLL.DrawBox().GetMaxId() - 1;
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("添加奖品成功，编号为" + model.GoodsCounts + ",是否继续添加图片?" + "<br/>" + "<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">是</a><br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;") + "\">否</a><br/>");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                catch (Exception e)
                {

                    builder.Append("添加出错,请重新添加");
                    builder.Append(e);
                }

            }
        }
        else
        {
            int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
            string sr = "r";
            string wwr = string.Empty;
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                if (i == 0)
                {
                    wwr += i + "|" + str;
                }
                else
                {
                    wwr += "|" + i + "|" + str;
                }
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("添加奖品请选择为【抽奖】或者【兑换】奖品<br />");
            if (ptype == 0)
                builder.Append("<b style=\"color:red\">添加抽奖奖品</b>|<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods4&amp;ptype=1") + "\">添加兑换奖品</a>");
            else
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addgoods4&amp;ptype=0") + "\">添加抽奖奖品</a>|<b style=\"color:red\">添加兑换奖品</b>");
            builder.Append(Out.Tab("</div>", ""));

            if (ptype == 0)
            {
                string strText = "属性选择:/,属性值:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,奖品等级:/,,";
                string strName = "GoodsName,GoodsValue,explain,GoodsNum,GoodsType,Rank,points";
                string strType = "select,num,textarea,num,select,select,hidden,textarea,act";
                string strValu = "" + 0 + "'" + "" + "'" + "奖品详情..." + "'" + "" + "'" + "" + "'" + "" + "'" + 0 + "'";
                string strEmpt = "0|体力|1|魅力|2|智慧|3|威望|4|邪恶,false,true,false,3|属性," + wwr + ",false";
                string strIdea = "/";
                string strOthe = "确定添加,draw.aspx?act=addgoods4&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string strText = "属性选择:/,属性值:/,奖品描述(可留空):/,添加奖品量:/,奖品类型:/,,奖品兑换" + drawJifen + "值:/,";
                string strName = "GoodsName,GoodsValue,explain,GoodsNum,GoodsType,Rank,points";
                string strType = "select,num,textarea,num,select,hidden,text,textarea,act";
                string strValu = "" + 0 + "'" + "" + "'" + "奖品详情..." + "'" + "" + "'" + "" + "'" + 1000 + "'" + "" + "'";
                string strEmpt = "0|体力|1|魅力|2|智慧|3|威望|4|邪恶,false,true,false,3|属性,false,false";
                string strIdea = "/";
                string strOthe = "确定添加,draw.aspx?act=addgoods4&amp;ptype=" + ptype + "&amp;backurl=" + Utils.getPage(1) + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:【抽奖】表示添加的奖品只参与抽奖活动，【兑换】表示添加的奖品只参加兑换活动<br />");
            builder.Append(Out.Tab("</div>", ""));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            //builder.Append(Out.Tab("<div>", ""));
            //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a>");
            //builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //重置奖箱
    private void XinlunPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "奖品等级设定";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;重置奖箱");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置 .

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确认修改")
        {
            #region 奖级设定产生奖级，且自动更新奖级开关默认为开启（0）
            int jiangji = int.Parse(Utils.GetRequest("jiangji", "all", 2, @"^[0-9]\d*$", "状奖级填写错误"));
            int j = jiangji;
            string str = string.Empty;
            int K = 0;
            for (int i = 0; i < j; i++)
            {
                str += 0;
                if (i <= (j - 2))
                {
                    str += "#";
                }
                K++;
            }

            xml.dss["jiangjikg"] = str;
            xml.dss["jiangji"] = jiangji;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            #endregion

            #region 更新奖箱表 tb_DrawTen ,且下架未结束的奖箱tb_DrawBox(进行中的下架5，结束的不管)

            new BCW.Draw.BLL.DrawUser().ClearTable("tb_DrawTen");
            new BCW.Draw.BLL.DrawBox().UpdateStatueAllgo();//字段'5'为奖箱奖品作废标识
            new BCW.Draw.BLL.DrawBox().UpdateGoodsNumgo(Convert.ToInt32(ub.GetSub("lun", xmlPath)));//作废的奖品清空库存
            BCW.Draw.Model.DrawTen mod = new BCW.Draw.Model.DrawTen();
            for (int i = 0; i < j; i++)
            {
                mod.ID = i;
                mod.Rank = i;
                mod.GoodsCounts = 0;
                new BCW.Draw.BLL.DrawTen().Add(mod);
            }

            #endregion

            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=xinlun&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            if (ptype == 2)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("==重置奖箱==<br />");
                builder.Append("数据可贵，请操作前慎重考虑");
                builder.Append(Out.Tab("</div>", "<br />"));
                string strText = ",";
                string strName = "backurl";
                string strType = "hidden";
                string strValu = "" + xml.dss["jiangji"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确认修改,draw.aspx?act=xinlun&amp;jiangji=" + xml.dss["jiangji"] + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=xinlun") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("==重置奖箱==<br />");
                builder.Append("数据可贵，请操作前慎重考虑");
                builder.Append(Out.Tab("</div>", "<br />"));
                string strText = ",";
                string strName = "backurl";
                string strType = "hidden";
                string strValu = "" + xml.dss["jiangji"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "重置奖箱,draw.aspx?act=xinlun&amp;ptype=2,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }

            builder.Append(Out.Tab("<div>", "<br /><br />"));
            builder.Append("温馨提示:<b style=\"color:red\">一旦重置奖箱，那么原来的奖箱的奖品全部下架，请在设置奖级前慎重选择</b><br />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //奖池管理
    private void ChiPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "奖池管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;奖池管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置 .

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确认添加")
        {
            #region 奖池新一轮
            int lun = int.Parse(Utils.GetRequest("lun", "all", 2, @"^[0-9]\d*$", "新一轮奖池填写错误"));
            xml.dss["lun"] = lun;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            #endregion

            #region 更新奖箱表 tb_DrawTen ,且下架未结束的奖箱tb_DrawBox(进行中的下架5，结束的不管)
            int j = Convert.ToInt32(xml.dss["jiangji"]);
            new BCW.Draw.BLL.DrawUser().ClearTable("tb_DrawTen");
            new BCW.Draw.BLL.DrawBox().UpdateStatueAllgo();//字段'5'为奖箱奖品作废标识
            BCW.Draw.Model.DrawTen mod = new BCW.Draw.Model.DrawTen();
            for (int i = 0; i < j; i++)
            {
                mod.ID = i;
                mod.Rank = i;
                mod.GoodsCounts = 0;
                new BCW.Draw.BLL.DrawTen().Add(mod);
            }

            #endregion

            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=chi&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            if (ptype == 2)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("==添加新一轮奖池==<br />");
                builder.Append("您即将添加第" + (Convert.ToInt32(xml.dss["lun"]) + 1) + "奖池，是否确定要添加");
                builder.Append(Out.Tab("</div>", "<br />"));
                string strText = ",";
                string strName = "backurl";
                string strType = "hidden";
                string strValu = "" + (Convert.ToInt32(xml.dss["lun"]) + 1) + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确认添加,draw.aspx?act=chi&amp;lun=" + (Convert.ToInt32(xml.dss["lun"]) + 1) + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=chi") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int xmllun = Convert.ToInt32(ub.GetSub("lun", xmlPath));
                int lun = 0;
                try { lun = xmllun; }
                catch { lun = 0; }
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("当前为第" + lun + "轮抽奖奖池");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("【第" + lun + "轮抽奖奖池最新动态】");
                builder.Append(Out.Tab("</div>", "<br />"));
                int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
                string str = string.Empty;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<table>");
                for (int i = 0; i < j; i++)
                {
                    string sssss = string.Empty;
                    if (i < 20)
                    {
                        if (i == 0) { sssss = "一等奖"; }
                        if (i == 1) { sssss = "二等奖"; }
                        if (i == 2) { sssss = "三等奖"; }
                        if (i == 3) { sssss = "四等奖"; }
                        if (i == 4) { sssss = "五等奖"; }
                        if (i == 5) { sssss = "六等奖"; }
                        if (i == 6) { sssss = "七等奖"; }
                        if (i == 7) { sssss = "八等奖"; }
                        if (i == 8) { sssss = "九等奖"; }
                        if (i == 9) { sssss = "十等奖"; }
                        if (i == 10) { sssss = "十一等奖"; }
                        if (i == 11) { sssss = "十二等奖"; }
                        if (i == 12) { sssss = "十三等奖"; }
                        if (i == 13) { sssss = "十四等奖"; }
                        if (i == 14) { sssss = "十五等奖"; }
                        if (i == 15) { sssss = "十六等奖"; }
                        if (i == 16) { sssss = "十七等奖"; }
                        if (i == 17) { sssss = "十八等奖"; }
                        if (i == 18) { sssss = "十九等奖"; }
                        if (i == 19) { sssss = "二十等奖"; }
                        str = sssss;
                    }
                    else
                    {
                        str = (i + 1) + "等奖";
                    }
                    string jiang = string.Empty;
                    int jiangnum = 0;
                    jiangnum = new BCW.Draw.BLL.DrawBox().GetGoodsNum(new BCW.Draw.BLL.DrawTen().GetCounts(i));
                    if (new BCW.Draw.BLL.DrawTen().GetCounts(i) == 0)
                    {
                        jiang = "奖品下架";
                    }
                    else
                    {
                        jiang = "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + new BCW.Draw.BLL.DrawTen().GetCounts(i) + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.Draw.BLL.DrawBox().GetGoodsName(new BCW.Draw.BLL.DrawTen().GetCounts(i)) + "</a>";
                    }

                    builder.Append("<tr><td>" + str + ":</td><td>" + jiang + "</td><td>|剩余:" + jiangnum + "</td></tr>");
                }
                //if(lun==new BCW.Draw.BLL.DrawBox().Getlun())
                // {
                try
                {
                    builder.Append("<tr><td>总奖池数:</td><td>" + new BCW.Draw.BLL.DrawBox().GetAllNum(lun) + "</td><td>|剩余:" + new BCW.Draw.BLL.DrawBox().GetAllNumS(lun) + "</td></tr>");
                }
                catch
                {

                }
                // }
                //else
                // {
                //     builder.Append("<tr><td>总奖池数:</td><td>" + 0 + "</td><td>|剩余:" + 0 + "</td></tr>");
                // }

                builder.Append("</table>");
                builder.Append(Out.Tab("</div>", ""));


                builder.Append(Out.Tab("<div>", ""));
                builder.Append("==添加新一轮奖池==<br />");
                builder.Append("数据可贵，请操作前慎重考虑");
                builder.Append(Out.Tab("</div>", "<br />"));
                string strText = ",";
                string strName = "backurl";
                string strType = "hidden";
                string strValu = "" + xml.dss["lun"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "添加新一轮奖池,draw.aspx?act=chi&amp;ptype=2,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            }

            builder.Append(Out.Tab("<div>", "<br /><br />"));
            builder.Append("温馨提示:<b style=\"color:red\">一旦重新生成新的一轮奖池，那么原来的奖箱的奖品全部下架，请在设置前慎重选择</b><br />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //奖级设定
    private void AddjiangjiPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "奖品等级设定";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;奖级设定");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("奖级设定|<a href=\"" + Utils.getUrl("draw.aspx?act=addjiangjikg") + "\">奖级开关设定</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置 .

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确认修改")
        {
            #region 奖级设定产生奖级，且自动更新奖级开关默认为开启（0）
            int jiangji = int.Parse(Utils.GetRequest("jiangji", "all", 2, @"^[0-9]\d*$", "状奖级填写错误"));
            int j = jiangji;
            string str = string.Empty;
            string str1 = string.Empty;
            int K = 0;
            for (int i = 0; i < j; i++)
            {
                str += 0;
                if (i <= (j - 2))
                {
                    str += "#";
                }
                K++;
            }
            xml.dss["jiangjikg"] = str;
            xml.dss["jiangji"] = jiangji;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            #endregion

            #region 更新奖箱表 tb_DrawTen ,且下架未结束的奖箱tb_DrawBox(进行中的下架5，结束的不管)

            new BCW.Draw.BLL.DrawUser().ClearTable("tb_DrawTen");
            new BCW.Draw.BLL.DrawBox().UpdateStatueAllgo();//字段'5'为奖箱奖品作废标识
            BCW.Draw.Model.DrawTen mod = new BCW.Draw.Model.DrawTen();
            for (int i = 0; i < j; i++)
            {
                mod.ID = i;
                mod.Rank = i;
                mod.GoodsCounts = 0;
                new BCW.Draw.BLL.DrawTen().Add(mod);
            }

            #endregion

            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=addjiangji&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            if (ptype == 2)
            {
                int jiangji = int.Parse(Utils.GetRequest("jiangji", "all", 2, @"^[0-9]\d*$", "状奖级填写错误"));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<b style=\"color:red\">即将修改奖级N为" + jiangji + "!!!</b>");
                builder.Append(Out.Tab("</div>", "<br />"));
                string strText = ",";
                string strName = "backurl";
                string strType = "hidden";
                string strValu = "" + jiangji + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确认修改,draw.aspx?act=addjiangji&amp;jiangji=" + jiangji + ",post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addjiangji") + "\">再看看吧</a>");
                builder.Append(Out.Tab("</div>", "<br />"));
            }
            else
            {
                string strText = "奖品奖级N（即奖箱共有N等奖）:/,";
                string strName = "jiangji,backurl";
                string strType = "num,hidden";
                string strValu = "" + xml.dss["jiangji"] + "'" + Utils.getPage(0) + "";
                string strEmpt = "false,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,draw.aspx?act=addjiangji&amp;ptype=2,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));


            }

            builder.Append(Out.Tab("<div>", "<br /><br />"));
            builder.Append("温馨提示:设置奖品等级输入N，那么奖品箱可增加N等奖，<b style=\"color:red\">一旦重新设置奖级N，那么原来的奖箱的奖品全部下架，所有奖品为空，请在设置奖级前慎重选择</b><br />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //奖级设定开关
    private void AddjiangjikgPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = "奖品等级设定";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;奖级设定");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("", ""));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addjiangji") + "\">奖级设定</a>|奖级设定开关");
        builder.Append(Out.Tab("</div>", "<br /><br />"));

        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置 .

        string[] Num = new string[Convert.ToInt32(xml.dss["jiangji"])];
        for (int i = 0; i < Convert.ToInt32(xml.dss["jiangji"]); i++)
        {
            Num[i] = Utils.GetRequest("Num" + i, "post", 1, "", "");
        }

        BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
        if (Utils.ToSChinese(ac) == "确定选择")
        {
            int jiangji = Convert.ToInt32(xml.dss["jiangji"]);
            int j = jiangji;
            string str = string.Empty;
            int K = 0;
            for (int i = 0; i < j; i++)
            {

                if ("" + Num[i] + "" != "")
                {
                    str += 0;
                }
                else
                {
                    str += 1;
                }
                if (i <= (j - 2))
                {
                    str += "#";
                }
                K++;
            }

            xml.dss["jiangjikg"] = str;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + Title + "游戏设置", "设置成功，正在返回..", Utils.getUrl("draw.aspx?act=addjiangjikg&amp;backurl=" + Utils.getPage(0) + ""), "2");
        }
        else
        {
            builder.Append("<form id=\"form1\" method=\"post\" action=\"draw.aspx\">");

            int j = Convert.ToInt32(xml.dss["jiangji"]);
            string sr = "r";
            string str = string.Empty;
            int Counts = 0;
            for (int i = 0; i < j; i++)
            {
                str = sr + (i + 1);
                Counts = new BCW.Draw.BLL.DrawTen().GetCounts(i);

                string sssss = string.Empty;
                string rrrrr = string.Empty;
                if (i < 20)
                {
                    if (i == 0) { sssss = "一等奖"; rrrrr = "一等奖（不可选）"; }
                    if (i == 1) { sssss = "二等奖"; rrrrr = "二等奖（不可选）"; }
                    if (i == 2) { sssss = "三等奖"; rrrrr = "三等奖（不可选）"; }
                    if (i == 3) { sssss = "四等奖"; rrrrr = "四等奖（不可选）"; }
                    if (i == 4) { sssss = "五等奖"; rrrrr = "五等奖（不可选）"; }
                    if (i == 5) { sssss = "六等奖"; rrrrr = "六等奖（不可选）"; }
                    if (i == 6) { sssss = "七等奖"; rrrrr = "七等奖（不可选）"; }
                    if (i == 7) { sssss = "八等奖"; rrrrr = "八等奖（不可选）"; }
                    if (i == 8) { sssss = "九等奖"; rrrrr = "九等奖（不可选）"; }
                    if (i == 9) { sssss = "十等奖"; rrrrr = "十等奖（不可选）"; }
                    if (i == 10) { sssss = "十一等奖"; rrrrr = "十一等奖（不可选）"; }
                    if (i == 11) { sssss = "十二等奖"; rrrrr = "十二等奖（不可选）"; }
                    if (i == 12) { sssss = "十三等奖"; rrrrr = "十三等奖（不可选）"; }
                    if (i == 13) { sssss = "十四等奖"; rrrrr = "十四等奖（不可选）"; }
                    if (i == 14) { sssss = "十五等奖"; rrrrr = "十五等奖（不可选）"; }
                    if (i == 15) { sssss = "十六等奖"; rrrrr = "十六等奖（不可选）"; }
                    if (i == 16) { sssss = "十七等奖"; rrrrr = "十七等奖（不可选）"; }
                    if (i == 17) { sssss = "十八等奖"; rrrrr = "十八等奖（不可选）"; }
                    if (i == 18) { sssss = "十九等奖"; rrrrr = "十九等奖（不可选）"; }
                    if (i == 19) { sssss = "二十等奖"; rrrrr = "二十等奖（不可选）"; }
                    if (Counts == 0) { str = sssss; } else { str = rrrrr; }
                }
                else
                {
                    if (Counts == 0) { str = (i + 1) + "等奖"; } else { str = (i + 1) + "等奖（不可选）"; }
                }
                string[] a = xml.dss["jiangjikg"].ToString().Split('#');

                builder.Append(Out.Tab("<div>", ""));
                if (Convert.ToInt32(a[i]) == 0)
                { builder.Append("<input type=\"checkbox\" name=\"Num" + i + "\" value=\"" + 0 + "\" checked=\"true\"/> " + sssss + ""); }
                else
                { builder.Append("<input type=\"checkbox\" name=\"Num" + i + "\" value=\"" + 1 + "\" /> " + sssss + ""); }
                builder.Append(Out.Tab("</div>", "<br />"));

            }
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"addjiangjikg\"/>");
            builder.Append("<input type=\"hidden\" name=\"ac\" Value=\"确定选择\"/>");
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"确定选择\"/>");
            builder.Append("</form>");
            builder.Append(Out.Tab("</div>", ""));


            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<b style=\"color:red\">奖级开关是控制奖品是否参与抽奖，如果未选择则表示这一个等级的奖品控制不中奖</b><br />");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">抽奖管理</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

        }
    }
    //增加商品图片
    private void AddImgPage()
    {
        Master.Title = "奖箱添加奖品图片";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖管理中心</a>&gt;");
        builder.Append("添加奖品图片" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "0"));//对应的商品编号  
        BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
        try
        {
            if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))//u=" + Utils.getstrU() + "&amp;
            {
                Utils.Success("温馨提示", "使用彩版，才能够上传图片，页面更直观，更快捷！正在切换进入彩版添加图片...", "draw.aspx?act=addimg&amp;ve=2a&amp;id=" + id + "&amp;u=" + Utils.getstrU(), "2");

            }

            int max = 10;// Convert.ToInt32(ub.GetSub("UpaMaxFileNum", xmlPath));
            string ac = Utils.GetRequest("ac", "all", 1, "", "");
            if (Utils.ToSChinese(ac) == "确定添加")
            {
                //遍历File表单元素
                System.Web.HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                builder.Append(files.Count);
                //try
                {
                    //string GetFiles = string.Empty;
                    string sqlimg = "";
                    for (int iFile = 0; iFile < files.Count; iFile++)
                    {
                        //检查文件扩展名字
                        System.Web.HttpPostedFile postedFile = files[iFile];
                        string fileName, fileExtension;
                        fileName = System.IO.Path.GetFileName(postedFile.FileName);
                        if (fileName != "")
                        {
                            fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                            string DirPath = string.Empty;
                            // string prevDirPath = string.Empty;
                            string Path = "/bbs/game/img/draw_img/";
                            if (FileTool.CreateDirectory(Path, out DirPath))
                            {
                                //上传数量限制
                                //生成随机文件名
                                fileName = DT.getDateTimeNum() + iFile + fileExtension;
                                sqlimg += DirPath + fileName + ",";
                                string SavePath = System.Web.HttpContext.Current.Request.MapPath(DirPath) + fileName;
                                postedFile.SaveAs(SavePath);
                            }
                        }
                    }
                    model.GoodsName = model.GoodsName;
                    model.GoodsValue = model.GoodsValue;
                    model.GoodsNum = model.GoodsNum;
                    model.Explain = model.Explain;
                    model.GoodsType = model.GoodsType;
                    model.Statue = model.Statue;
                    model.beizhu = model.beizhu;
                    model.rank = model.rank;
                    model.points = model.points;
                    model.AllNum = model.AllNum;
                    model.AddTime = model.AddTime;
                    model.OverTime = model.OverTime;
                    model.ReceiveTime = model.ReceiveTime;
                    model.GoodsValue = model.GoodsValue;
                    model.lun = model.lun;

                    model.GoodsImg = sqlimg;

                    new BCW.Draw.BLL.DrawBox().Update(model);

                    Utils.Success("抽奖吧添加商品", "添加图片成功，正在返回奖品管理页..", Utils.getUrl("draw.aspx?act=jiangpin&amp;backurl=" + Utils.getPage(0) + ""), "3");
                }
                //catch { builder.Append("出错啦，从新添加"); }
            }
            else
            {
                builder.Append(Out.Tab("<div>", ""));
                try
                {
                    builder.Append("你将为新增的商品:" + model.GoodsName + ",添加图片" + "<br/>");///////////////////////
                }
                catch
                {
                    builder.Append("你将为新增的编号为" + id + "的商品,添加图片" + "<br/>");///////////////////////
                }
                // builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("上传允许格式:" + "jpg,ogg,png" + "<br />");
                //builder.Append("每个文件限" + 2 + "K");
                builder.Append(Out.Tab("</div>", "<br />"));
                int num = int.Parse(Utils.GetRequest("num", "get", 1, @"^[1-9]\d*$", "1"));
                if (num > max)
                    num = max;
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("上传:");
                for (int i = 1; i <= max; i++)
                {
                    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;num=" + i + "&amp;backurl=" + Utils.getPage(0) + "") + "\"><b>" + i + "</b></a> ");
                }
                builder.Append("个");
                builder.Append(Out.Tab("</div>", ""));
                string ssUpType = string.Empty;
                string ssText = string.Empty;
                string ssName = string.Empty;
                string ssType = string.Empty;
                string ssValu = string.Empty;
                string ssEmpt = string.Empty;
                string ssIdea = string.Empty;
                string ssOthe = string.Empty;
                for (int i = 0; i < num; i++)
                {
                    string y = ",";
                    if (num == 1)
                    {
                        ssText = ssText + y + "选择" + ssUpType + "附件:/,";
                    }
                    else
                    {
                        ssText = ssText + y + "" + ssUpType + "第" + (i + 1) + "个附件:/,";
                    }
                    ssName = ssName + y + "file" + (i + 1) + y;
                    ssType = ssType + y + "file" + y;
                    ssValu = ssValu + "''";
                    ssEmpt = ssEmpt + y + y;
                }
                string strUpgroup = string.Empty;
                strUpgroup = "" + strUpgroup;
                ssText = ssText + Utils.Mid(strText, 1, strText.Length) + "," + ",";
                ssName = ssName + Utils.Mid(strName, 1, strName.Length) + ",act";
                ssType = ssType + Utils.Mid(strType, 1, strType.Length) + "hidden,hidden";
                ssValu = ssValu + Utils.Mid(strValu, 1, strValu.Length) + "ac" + "'img";
                ssEmpt = ssEmpt + Utils.Mid(strEmpt, 1, strEmpt.Length) + ",,";
                ssIdea = "/";
                ssOthe = "确定添加|reset,draw.aspx?act=addimg&amp;id=" + id + ",post,2,red|blue";
                builder.Append(Out.wapform(ssText, ssName, ssType, ssValu, ssEmpt, ssIdea, ssOthe));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">不添加照片，返回奖品管理首页</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        catch (Exception e) { builder.Append(e); }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖管理</a>&gt;");
        builder.Append("添加商品图片" + "<br/>");
        builder.Append(Out.Tab("</div>", ""));
        //  Footer();
    }
    //修改商品参数
    private void UpdateGoodsAll()
    {
        Master.Title = "修改商品参数";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖管理</a>&gt;奖品修改");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int id = int.Parse(Utils.GetRequest("id", "all", 1, @"^[0-9]\d*$", "0"));
        int img = int.Parse(Utils.GetRequest("img", "all", 1, @"^[0-9]\d*$", "0"));
        string strText = "输入奖品编号:/,";
        string strName = "id,backurl";
        string strType = "num,hidden";
        string strValu = "" + id + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜奖品,draw.aspx?act=update&amp;u=" + Utils.getstrU() + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("", Out.Hr()));

        BCW.Draw.Model.DrawBox mod = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
        int uu = 0;
        try
        {
            uu = mod.Statue;
        }
        catch
        {
            uu = 0;
        }

        if (uu != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<h style=\"color:red\">只能对正在进行且未被抽中的奖品进行修改！</h>");
            builder.Append(Out.Tab("</div>", ""));
        }

        if (id == 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("请输入奖品的编号" + "<br/>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (!new BCW.Draw.BLL.DrawBox().CountsExists(id))
            {
                Utils.Error("不存在该商品记录.", "");
            }
            else
            {
                string ac = Utils.GetRequest("ac", "all", 1, "", "");
                if (Utils.ToSChinese(ac) == "确定修改")
                {
                    string GoodsName = Utils.GetRequest("GoodsName", "post", 2, @"^[^\^]{1,20}$", "奖品名称限1-20字内");
                    string AllNum = Utils.GetRequest("AllNum", "post", 4, @"^[0-9]\d*$", "奖品数量填写错误(整数)");
                    string GoodsNum = Utils.GetRequest("GoodsNum", "post", 4, @"^[0-9]\d*$", "奖品余量填写错误(整数)");
                    string GoodsImg = Utils.GetRequest("GoodsImg", "post", 3, @"^[^\^]{0,2000}$", "图片输入错误");
                    string GoodsTpye = Utils.GetRequest("GoodsTpye", "post", 3, @"^[0-9]\d*$", "奖品类型出错");
                    string GoodsValue = Utils.GetRequest("GoodsValue", "post", 4, @"^[0-9]\d*$", "奖品总价值填写错误(整数)");
                    string statue = Utils.GetRequest("statue", "post", 2, @"^[0-9]\d*$", "奖品状态填写错误");
                    string explain = Utils.GetRequest("explain", "post", 3, @"^[^\^]{1,20000}$", "奖品描述限20000字内");
                    string beizhu = Utils.GetRequest("beizhu", "post", 2, @"^[^\^]{1,20}$", "备注填写错误(为奖品市值单位)");
                    string points = Utils.GetRequest("points", "post", 2, @"^[0-9]\d*$", "奖品所需兑换" + drawJifen + "值填写错误");

                    if (mod.Statue == 0)
                    {
                        try
                        {
                            mod.GoodsName = GoodsName;
                            mod.GoodsValue = Convert.ToInt32(GoodsValue);
                            mod.GoodsNum = Convert.ToInt32(GoodsNum);
                            mod.GoodsImg = GoodsImg;
                            mod.Explain = explain;
                            mod.GoodsType = Convert.ToInt32(GoodsTpye);
                            mod.Statue = Convert.ToInt32(statue);
                            mod.beizhu = beizhu;
                            mod.rank = mod.rank;
                            mod.points = Convert.ToInt32(points);
                            mod.AllNum = Convert.ToInt32(AllNum);
                            mod.AddTime = mod.AddTime;
                            mod.OverTime = mod.OverTime;
                            mod.ReceiveTime = mod.ReceiveTime;
                            mod.lun = mod.lun;
                            new BCW.Draw.BLL.DrawBox().Update(mod);

                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("修改成功!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><br />返回继续修改</a>");
                            builder.Append(Out.Tab("</div>", "<br/>"));

                        }
                        catch
                        {
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("修改失败!" + "<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">返回查看</a>");
                            builder.Append(Out.Tab("</div>", "<br/>"));
                        }
                    }
                    else
                    {
                        Utils.Error("只能对正在进行的奖品做修改，已抽中、派送与送达的奖品不能更改！.. ", "");
                    }


                }
                else
                {
                    string rank = "";
                    int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
                    for (int i = 0; i < j; i++)
                    {
                        if (i < 20)
                        {
                            if (mod.rank == 0) { rank = "一等奖"; }
                            if (mod.rank == 1) { rank = "二等奖"; }
                            if (mod.rank == 2) { rank = "三等奖"; }
                            if (mod.rank == 3) { rank = "四等奖"; }
                            if (mod.rank == 4) { rank = "五等奖"; }
                            if (mod.rank == 5) { rank = "六等奖"; }
                            if (mod.rank == 6) { rank = "七等奖"; }
                            if (mod.rank == 7) { rank = "八等奖"; }
                            if (mod.rank == 8) { rank = "九等奖"; }
                            if (mod.rank == 9) { rank = "十等奖"; }
                            if (mod.rank == 10) { rank = "十一等奖"; }
                            if (mod.rank == 11) { rank = "十二等奖"; }
                            if (mod.rank == 12) { rank = "十三等奖"; }
                            if (mod.rank == 13) { rank = "十四等奖"; }
                            if (mod.rank == 14) { rank = "十五等奖"; }
                            if (mod.rank == 15) { rank = "十六等奖"; }
                            if (mod.rank == 16) { rank = "十七等奖"; }
                            if (mod.rank == 17) { rank = "十八等奖"; }
                            if (mod.rank == 18) { rank = "十九等奖"; }
                            if (mod.rank == 19) { rank = "二十等奖"; }
                            if (mod.rank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                        }
                        else
                        {
                            if (mod.rank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                            else
                            {
                                if (mod.rank == i) { rank = (i + 1) + "等奖"; }
                            }
                        }

                    }

                    BCW.Draw.Model.DrawBox model = new BCW.Draw.BLL.DrawBox().GetDrawBoxbyC(id);
                    builder.Append(Out.Tab("<div>", "<br/>"));
                    builder.Append("奖品编号：" + id + "<br />");
                    builder.Append("奖品等级：" + rank + "");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    if (model.GoodsImg.ToString() != "0" && model.GoodsImg.ToString() != "100" && model.GoodsImg.ToString() != "5" && model.GoodsImg.ToString() != "10" && model.GoodsImg.ToString() != "1")
                    {
                        string[] imgNum = model.GoodsImg.Split(',');

                        for (int c = 0; c < imgNum.Length - 1; c++)
                        {
                            if (model.GoodsType == 2)
                            {
                                builder.Append("" + "<img src=\"" + imgNum[c].ToString() + "\"  alt=\"load\"/>" + "&nbsp;&nbsp;");
                            }
                            else
                            {
                                if (img != 0)
                                    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;img=" + 0 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\"><img src=\"" + imgNum[c].ToString() + "\"   alt=\"load\"/></a>&nbsp;&nbsp;");
                                else
                                    builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=update&amp;id=" + id + "&amp;img=" + 1 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + "<img src=\"" + imgNum[c].ToString() + "\"  width=\"10%\" height=\"auto\" alt=\"load\"/>" + "</a>&nbsp;&nbsp;");
                            }
                        }
                    }

                    builder.Append(Out.Tab("</div>", ""));
                    string Text = "奖品名称:/,奖品数量:/,奖品余量:/,奖品图片:/,奖品类型:/,奖品市值:/,兑换" + drawJifen + "值（填0奖品不参与兑换）:/,市值单位:/,奖品状态:/,奖品描述:/,";
                    string Name = "GoodsName,AllNum,GoodsNum,GoodsImg,GoodsTpye,GoodsValue,points,beizhu,statue,explain";
                    string Type = "text,num,text,text,select,text,text,text,select,textarea";
                    string Valu = "" + model.GoodsName + "'" + model.AllNum + "'" + model.GoodsNum + "'" + model.GoodsImg + "'" + model.GoodsType + "'" + model.GoodsValue + "'" + model.points + "'" + model.beizhu + "'" + model.Statue + "'" + model.Explain + "'" + Utils.getPage(0) + "";
                    string Empt = "false,false,false,true,0|酷币|1|实物|2|道具|3|属性,true,true,true,0|进行中|1|结束|5|下架,true";
                    string Idea = "/";
                    string Othe = "确定修改,draw.aspx?act=update&amp;id=" + id + "&amp;,post,1,red";
                    builder.Append(Out.wapform(Text, Name, Type, Valu, Empt, Idea, Othe));
                    if (mod.Statue == 0)
                    {
                        builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=addimg&amp;id=" + id + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">修改图片</a>");
                        builder.Append(Out.Tab("</div>", "<br />"));
                    }
                    builder.Append(Out.Tab("<div>", "<br />"));
                    builder.Append("温馨提示：奖品类型为酷币时，奖品市值为酷币大小，为必填。修改图片会删除原有图片，农场道具自带图片，请慎重操作。抽奖活动的奖品不能与兑换活动的奖品互相更改，抽奖活动的奖品的等级不能修改");
                    builder.Append(Out.Tab("</div>", "<br />"));
                }
            }

        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //删除奖品
    private void DelPage()
    {
        Master.Title = "抽奖奖品管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin") + "\">奖品管理</a>&gt;删除奖品");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int ManageId = new BCW.User.Manage().IsManageLogin();
        //if (ManageId != 1 && ManageId != 9)
        //{
        //    Utils.Error("权限不足", "");
        //}
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int counts = int.Parse(Utils.GetRequest("counts", "all", 2, @"^[0-9]\d*$", "奖品编号错误"));
        int dsd = new BCW.Draw.BLL.DrawBox().GetStatue(counts);
        int rank = int.Parse(Utils.GetRequest("rank", "all", 2, @"^[0-9]\d*$", "奖品等级错误"));
        if (info == "")
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("确定删除编号为" + counts + "的奖品吗？" + "");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("删除奖品会将奖品作废，不再参与一切抽奖活动");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?info=ok&amp;act=del&amp;counts=" + counts + "&amp;rank=" + rank + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=jiangpin&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.Draw.BLL.DrawBox().CountsExists(counts))
            {
                Utils.Error("不存在的记录", "");
            }
            if (dsd == 0)
            {
                new BCW.Draw.BLL.DrawTen().UpdateGoodsCounts(rank, 0);//把奖级里面的编号去掉，停止参与抽奖
                new BCW.Draw.BLL.DrawBox().UpdateStatue(counts, 5);//字段'5'为奖箱奖品作废标识
                new BCW.Draw.BLL.DrawBox().UpdateGoodsNum(counts, 0);
                Utils.Success("删除奖品", "删除成功..", Utils.getPage("draw.aspx?act=jiangpin"), "2");
            }
            else if (dsd == 5)
            {
                int ID = new BCW.Draw.BLL.DrawBox().GetID(counts);
                new BCW.Draw.BLL.DrawBox().Delete(ID);
                Utils.Success("删除奖品", "已经彻底清除下架奖品删除成功..", Utils.getPage("draw.aspx?act=jiangpin"), "2");
            }
            else
            {
                Utils.Error("不能对已经结束的奖品删除...", "");
            }
        }
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + "抽奖管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //添加
    private void JPbjPage()
    {
        string Title = ub.GetSub("drawName", xmlPath);
        Master.Title = (Title + "管理中心");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        //string xmlPath = "/Controls/Dawnlife.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理</a>&gt;奖品编辑");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + Title + "管理中心</a><br />");
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //积分记录
    private void DrawListPage()
    {
        Master.Title = "" + GameName + drawJifen + "记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;" + drawJifen + "历史记录");
        builder.Append(Out.Tab("</div>", "<br />"));


        if (new BCW.Draw.BLL.Drawlist().Exists(1))
        {
            DataSet ds0 = new BCW.Draw.BLL.Drawlist().GetList("Top 1 UsID,Time ", " Type!=9 Order by Time ");
            DataSet ds1 = new BCW.Draw.BLL.Drawlist().GetList("Top 1 UsID,Time ", " Type!=9  Order by Time Desc ");
            DateTime ontime = Convert.ToDateTime(ds0.Tables[0].Rows[0]["Time"].ToString());
            DateTime stoptime = Convert.ToDateTime(ds1.Tables[0].Rows[0]["Time"].ToString());
            int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
            string UName = new BCW.BLL.User().GetUsName(usid);
            string searchday1 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", ontime.ToString("yyyyMMdd"));
            string searchday2 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", stoptime.ToString("yyyyMMdd"));

            string strText = "输入用户ID：,开 始 日 期：,结 束 日 期：,,";
            string strName = "usid,oTime,sTime,backurl";
            string strType = "num,date,date,hidden";
            string strValu = "" + usid + "'" + searchday1 + "'" + searchday2 + "'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false";
            string strIdea = "/";
            string strOthe = "确认搜索,draw.aspx?act=drawlist,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            long Jifen = 0;
            if (usid != 0)
            {
                Jifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'and UsID=" + usid + " ");
            }
            else
            {
                Jifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'");
            }
            builder.Append(Out.Tab("<div>", "<br />"));
            if (usid != 0)
            {
                builder.Append(" 玩家" + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UName + "(" + usid + ")" + "</a>" + "投入" + Jifen + "" + drawJifen + "" + " ");
                builder.Append("  <a href=\"" + Utils.getUrl("draw.aspx?act=jifenjiangli&amp;usid=" + usid + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "") + "\">奖励" + drawJifen + "</a>");
            }
            else
            {
                builder.Append(">>>所有玩家投入" + Jifen + "" + drawJifen + "" + "&lt;&lt;&lt;");
            }
            builder.Append(Out.Tab("</div>", Out.Hr()));

            int pageIndex;
            int recordCount;
            int pageSize = 10;//分页大小;
            string strWhere = string.Empty;
            if (usid != 0)
            {
                strWhere = " Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "' and UsID=" + usid + " ";
            }
            else
            {
                strWhere = " Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "' ";
            }
            string strOrder;
            strOrder = " Time DESC ";
            string[] pageValUrl = { "act", "usid", "oTime", "sTime", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            // 开始读取列表
            IList<BCW.Draw.Model.Drawlist> listdrawlist = new BCW.Draw.BLL.Drawlist().GetDrawlists(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listdrawlist.Count > 0)
            {
                int k = 1;
                foreach (BCW.Draw.Model.Drawlist n in listdrawlist)
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

                    usid = Convert.ToInt32(n.UsID);
                    int d = (pageIndex - 1) * 10 + k;
                    string sText = string.Empty;
                    string type = string.Empty;
                    if (n.Type == 0) { type = "不中奖"; }
                    if (n.Type == 1) { type = "抽中"; }
                    if (n.Type == 2) { type = "兑换"; }
                    string UsName = new BCW.BLL.User().GetUsName(usid);
                    string GName = new BCW.Draw.BLL.DrawUser().GetMyGoodsbynum(Convert.ToInt32(n.GoodsCounts));// n.goodscounts 是奖品编号，不是ID
                    BCW.Draw.Model.DrawUser some = new BCW.Draw.BLL.DrawUser().GetDrawUserbynum(Convert.ToInt32(n.GoodsCounts));
                    int rrank = 0;
                    try
                    {
                        rrank = new BCW.Draw.BLL.DrawBox().GetRank(some.GoodsCounts);
                    }
                    catch { }
                    string rank = "";
                    int j = Convert.ToInt32(ub.GetSub("jiangji", xmlPath));
                    for (int i = 0; i < j; i++)
                    {
                        if (i < 20)
                        {
                            if (rrank == 0) { rank = "一等奖"; }
                            if (rrank == 1) { rank = "二等奖"; }
                            if (rrank == 2) { rank = "三等奖"; }
                            if (rrank == 3) { rank = "四等奖"; }
                            if (rrank == 4) { rank = "五等奖"; }
                            if (rrank == 5) { rank = "六等奖"; }
                            if (rrank == 6) { rank = "七等奖"; }
                            if (rrank == 7) { rank = "八等奖"; }
                            if (rrank == 8) { rank = "九等奖"; }
                            if (rrank == 9) { rank = "十等奖"; }
                            if (rrank == 10) { rank = "十一等奖"; }
                            if (rrank == 11) { rank = "十二等奖"; }
                            if (rrank == 12) { rank = "十三等奖"; }
                            if (rrank == 13) { rank = "十四等奖"; }
                            if (rrank == 14) { rank = "十五等奖"; }
                            if (rrank == 15) { rank = "十六等奖"; }
                            if (rrank == 16) { rank = "十七等奖"; }
                            if (rrank == 17) { rank = "十八等奖"; }
                            if (rrank == 18) { rank = "十九等奖"; }
                            if (rrank == 19) { rank = "二十等奖"; }
                            if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                        }
                        else
                        {
                            if (rrank == 1000) { rank = "<b style=\"color:red\">兑换奖品</b>"; }
                            else
                            {
                                if (rrank == i) { rank = (i + 1) + "等奖"; }
                            }
                        }

                    }
                    if (n.Type == 0)
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + n.UsID + ")" + "</a>" + drawJifen + "-" + n.Jifen + "|" + type + GName + "|" + Convert.ToDateTime(n.Time).ToString("yyyy-MM-dd HH:mm:ss") + "";
                    }
                    else
                    {
                        sText = "." + "<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + UsName + "(" + n.UsID + ")" + "</a>" + drawJifen + "-" + n.Jifen + "|" + type + rank + "：" + GName + "|奖品ID：" + n.GoodsCounts + "|" + Convert.ToDateTime(n.Time).ToString("yyyy-MM-dd HH:mm:ss") + "";
                    }
                    builder.AppendFormat("<a href=\"" + Utils.getUrl("draw.aspx?act=drawlist&amp;usid=" + usid + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">  </a>" + d + sText + "   ");

                    k++;

                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0) + "");
            }

            else
            {
                builder.Append(Out.Div("div", "没有相关记录..") + "<br />");
            }
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append("没有相关记录...");
        }
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    //积分消费记录
    private void ConlistPage()
    {
        Master.Title = "" + GameName + drawJifen + "消费记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;" + drawJifen + "消费记录");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("【" + drawJifen + "Top排行】");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex1;
        int recordCount1;
        int pageSize1 = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere1 = "";
        string strOrder1 = "";
        string[] pageValUrl1 = { "act", "backurl" };
        pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
        if (pageIndex1 == 0)
            pageIndex1 = 1;

        strWhere1 = " Jifen>0 ";
        strOrder1 = " Jifen Desc ";
        IList<BCW.Draw.Model.DrawJifen> listjifen = new BCW.Draw.BLL.DrawJifen().GetDrawJifens(pageIndex1, pageSize1, strWhere1, strOrder1, out recordCount1);
        if (listjifen.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawJifen n in listjifen)
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

                string mename = new BCW.BLL.User().GetUsName(n.UsID);
                int wd = ((pageIndex1 - 1) * 10 + k);
                builder.Append("[<b style=\"color:red\">TOP" + wd + "</b>].<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + n.UsID + ")</a>拥有" + "<b style=\"color:red\">" + n.Jifen + "</b>" + drawJifen + "");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, recordCount1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关记录.."));
        }

        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        if (usid != 0)
        {
            builder.Append("【<b style=\"color:red\"><a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "(" + usid + ")</a></b>消费记录】");
        }
        else
        {
            builder.Append("【消费记录】");
        }
        builder.Append(Out.Tab("</div>", ""));


        string strText = "输入用户ID:/,";
        string strName = "usid,backurl";
        string strType = "num,hidden";
        string strValu = "" + usid + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确认搜索/,draw.aspx?act=conlist,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "act", "ptype", "usid", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        //查询条件
        if (usid != 0)
        {
            strWhere = "UsID=" + usid + " ";
        }
        else
        {
            strWhere = " ";
        }

        // 开始读取列表
        IList<BCW.Draw.Model.DrawJifenlog> listJifenlog = new BCW.Draw.BLL.DrawJifenlog().GetJifenlogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listJifenlog.Count > 0)
        {
            int k = 1;
            foreach (BCW.Draw.Model.DrawJifenlog n in listJifenlog)
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
                builder.AppendFormat("{0}.<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid={1}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{2}</a>{3}|操作{4}|结{5}({6})", (pageIndex - 1) * pageSize + k, n.UsId, n.UsName + "(" + n.UsId + ")", Out.SysUBB(n.AcText), n.AcGold, n.AfterGold, DT.FormatDate(n.AddTime, 1));

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

        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //builder.Append(Out.Tab("<div>", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
        //builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖励用户积分
    private void JifenjiangliPage()
    {
        Master.Title = "" + GameName + drawJifen + "记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=drawlist") + "\">" + drawJifen + "历史记录</a>&gt;" + drawJifen + "奖励");
        builder.Append(Out.Tab("</div>", "<br />"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        string UName = new BCW.BLL.User().GetUsName(usid);
        string searchday1 = Utils.GetRequest("srarchday1", "all", 1, @"^{200}[0-9]$", DateTime.Now.ToString());
        string searchday2 = Utils.GetRequest("srarchday2", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString());

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("" + UName + "(" + usid + ")" + "拥有" + new BCW.Draw.BLL.DrawJifen().GetJifen(usid) + drawJifen);
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("奖励" + UName + "(" + usid + ")" + drawJifen);
        builder.Append(Out.Tab("</div>", ""));
        // 输入框
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));
        string strText = "输入奖励" + drawJifen + ":/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "确定/,draw.aspx?act=jfjl&amp;usid=" + usid + ",post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx?act=drawlist&amp;usid=" + usid + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "") + "\">返回" + drawJifen + "记录</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //确定奖励用户积分
    private void jfjlPage()
    {
        Master.Title = "" + GameName + drawJifen + "记录";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx?act=drawlist") + "\">" + drawJifen + "历史记录</a>&gt;" + drawJifen + "奖励");
        builder.Append(Out.Tab("</div>", "<br />"));
        int usid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        string searchday1 = Utils.GetRequest("srarchday1", "all", 1, @"^{200}[0-9]$", DateTime.Now.ToString());
        string searchday2 = Utils.GetRequest("srarchday2", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString());
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[0-9]\d*$", "0"));

        new BCW.Draw.BLL.DrawJifen().UpdateJifen(usid, uid, "" + GameName + "系统奖励");
        //发内线
        string strLog = "根据你近期的表现，系统奖励你" + uid + drawJifen + "(" + GameName + ")" + "[url=/bbs/game/draw.aspx]进入" + GameName + "[/url]";
        new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

        Utils.Success("游戏设置", "成功奖励，正在返回..", Utils.getUrl("draw.aspx?act=drawlist&amp;usid=" + usid + "&amp;searchday1=" + searchday1 + "&amp;searchday2=" + searchday2 + "&amp;backurl=" + Utils.getPage(1) + ""), "1");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">返回" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //盈利分析
    private void StatPage()
    {
        Master.Title = "" + GameName + "盈利分析";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">管理中心</a>&gt;<a href=\"" + Utils.getUrl("draw.aspx") + "\">" + GameName + "管理</a>&gt;盈利分析");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>赢利分析：</b>");
        builder.Append(Out.Tab("</div>", ""));

        //今天投入积分
        long todayjifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "DateDiff(day,Time,getdate())=0");
        //昨天投入
        long yesterdayjifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "DateDiff(day,Time,getdate()-1)=0");
        //本月投入
        long monthjifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "DateDiff(month,Time,getdate())=0");
        //上月投入
        long lastmonthjifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "DateDiff(month,Time,getdate())=1");
        //今年投入
        long yearjifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "DateDiff(YEAR,Time,getdate())=0");
        //总投入
        long alljifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", " Type!=9 ");
        builder.Append(Out.Tab("", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今天玩家投入：" + todayjifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("昨天玩家投入：" + yesterdayjifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("本月玩家投入：" + monthjifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("上月玩家投入：" + lastmonthjifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("今年玩家投入：" + yearjifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("总玩家投入：" + alljifen + " " + Convert.ToString(ub.GetSub("drawJifen", "/Controls/draw.xml")));
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append("<hr/>");

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (Utils.ToSChinese(ac) == "马上查询")
        {
            string searchday1 = string.Empty;
            string searchday2 = string.Empty;
            searchday1 = Utils.GetRequest("sTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));
            searchday2 = Utils.GetRequest("oTime", "all", 1, @"^([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})(((0[13578]|1[02])(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))$", DateTime.Now.ToString("yyyyMMdd"));

            if (searchday1 == "")
            {
                searchday1 = DateTime.Now.ToString("yyyyMMdd");
            }
            if (searchday2 == "")
            {
                searchday2 = DateTime.Now.AddDays(1).ToString("yyyyMMdd");
            }

            long Jifen = new BCW.Draw.BLL.Drawlist().GetJifensum("Jifen", "Time>='" + (DateTime.ParseExact(searchday1, "yyyyMMdd", null).ToString("yyyy-MM-dd")) + "' and Time<='" + (DateTime.ParseExact(searchday2, "yyyyMMdd", null).AddDays(1).ToString("yyyy-MM-dd")) + "'AND Type!='9'");

            builder.Append(Out.Tab("<div>", ""));
            builder.Append(">>>玩家投入" + Jifen + "" + drawJifen + "" + "&lt;&lt;&lt;");
            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + searchday1 + "'" + searchday2 + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,draw.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(">>>玩家投入 0 " + drawJifen + "&lt;&lt;&lt;");
            string strText = "开始日期：,结束日期：,";
            string strName = "sTime,oTime,act";
            string strType = "date,date,hidden";
            string strValu = "" + DateTime.Now.ToString("yyyyMMdd") + "'" + DateTime.Now.ToString("yyyyMMdd") + "'''backsave";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "马上查询,draw.aspx?act=stat,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        builder.Append(Out.Tab("</div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("draw.aspx") + "\">" + GameName + "管理</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }
    //奖级
    public string GetRank2(char i)
    {
        string str = string.Empty;
        string sssss = string.Empty;
        if (i == 'A') { sssss = "一等奖"; }
        if (i == 'B') { sssss = "二等奖"; }
        if (i == 'C') { sssss = "三等奖"; }
        if (i == 'D') { sssss = "四等奖"; }
        if (i == 'E') { sssss = "五等奖"; }
        if (i == 'F') { sssss = "六等奖"; }
        if (i == 'G') { sssss = "七等奖"; }
        if (i == 'H') { sssss = "八等奖"; }
        if (i == 'I') { sssss = "九等奖"; }
        if (i == 'J') { sssss = "十等奖"; }
        if (i == 'K') { sssss = "十一等奖"; }
        if (i == 'L') { sssss = "十二等奖"; }
        if (i == 'M') { sssss = "十三等奖"; }
        if (i == 'N') { sssss = "十四等奖"; }
        if (i == 'O') { sssss = "十五等奖"; }
        if (i == 'P') { sssss = "十六等奖"; }
        if (i == 'Q') { sssss = "十七等奖"; }
        if (i == 'R') { sssss = "十八等奖"; }
        if (i == 'S') { sssss = "十九等奖"; }
        if (i == 'T') { sssss = "二十等奖"; }
        str = sssss;
        return str;

    }

    #region 随机值计算方法
    public class RandomController
    {
        //待随机抽取数据集合
        public List<char> datas1 = new List<char>(new char[] { 'A' });
        public List<char> datas2 = new List<char>(new char[] { 'A', 'B' });
        public List<char> datas3 = new List<char>(new char[] { 'A', 'B', 'C' });
        public List<char> datas4 = new List<char>(new char[] { 'A', 'B', 'C', 'D' });
        public List<char> datas5 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E' });
        public List<char> datas6 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F' });
        public List<char> datas7 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G' });
        public List<char> datas8 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H' });
        public List<char> datas9 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I' });
        public List<char> datas10 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' });
        public List<char> datas11 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K' });
        public List<char> datas12 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L' });
        public List<char> datas13 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M' });
        public List<char> datas14 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N' });
        public List<char> datas15 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' });
        public List<char> datas16 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P' });
        public List<char> datas17 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q' });
        public List<char> datas18 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' });
        public List<char> datas19 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S' });
        public List<char> datas20 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T' });
        public List<char> datas21 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U' });
        public List<char> datas22 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V' });
        public List<char> datas23 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W' });
        public List<char> datas24 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' });
        public List<char> datas25 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y' });
        public List<char> datas26 = new List<char>(new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' });

        static string[] Rq = ub.GetSub("Rq", "/Controls/draw.xml").Split('#');
        //权值
        static ushort a = Convert.ToUInt16(Rq[0]);
        static ushort[] temp = new ushort[] {
           Convert.ToUInt16(Rq[0]), Convert.ToUInt16(Rq[1]), Convert.ToUInt16(Rq[2]), Convert.ToUInt16(Rq[3]), Convert.ToUInt16(Rq[4]),
           Convert.ToUInt16(Rq[5]), Convert.ToUInt16(Rq[6]), Convert.ToUInt16(Rq[7]), Convert.ToUInt16(Rq[8]), Convert.ToUInt16(Rq[9]),
           Convert.ToUInt16(Rq[10]), Convert.ToUInt16(Rq[11]), Convert.ToUInt16(Rq[12]), Convert.ToUInt16(Rq[13]), Convert.ToUInt16(Rq[14]),
           Convert.ToUInt16(Rq[15]), Convert.ToUInt16(Rq[16]), Convert.ToUInt16(Rq[17]), Convert.ToUInt16(Rq[18]), Convert.ToUInt16(Rq[19]),
           Convert.ToUInt16(Rq[20]), Convert.ToUInt16(Rq[21]), Convert.ToUInt16(Rq[22]), Convert.ToUInt16(Rq[23]), Convert.ToUInt16(Rq[24]) };
        public List<ushort> weights = new List<ushort>(temp);

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="count">随机抽取个数</param>

        public RandomController(ushort count)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            if (count > jiangji)
                throw new Exception("抽取个数不能超过数据集合大小！！");
            _Count = count;
        }

        /// <summary>
        /// 随机抽取
        /// </summary>
        /// <param name="rand">随机数生成器</param>
        /// <returns></returns>
        public char[] RandomExtract(Random rand)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            char datas = '0';
            if (jiangji == 1) { datas = datas1[rand.Next((jiangji))]; }
            if (jiangji == 2) { datas = datas2[rand.Next((jiangji))]; }
            if (jiangji == 3) { datas = datas3[rand.Next((jiangji))]; }
            if (jiangji == 4) { datas = datas4[rand.Next((jiangji))]; }
            if (jiangji == 5) { datas = datas5[rand.Next((jiangji))]; }
            if (jiangji == 6) { datas = datas6[rand.Next((jiangji))]; }
            if (jiangji == 7) { datas = datas7[rand.Next((jiangji))]; }
            if (jiangji == 8) { datas = datas8[rand.Next((jiangji))]; }
            if (jiangji == 9) { datas = datas9[rand.Next((jiangji))]; }
            if (jiangji == 10) { datas = datas10[rand.Next((jiangji))]; }
            if (jiangji == 11) { datas = datas11[rand.Next((jiangji))]; }
            if (jiangji == 12) { datas = datas12[rand.Next((jiangji))]; }
            if (jiangji == 13) { datas = datas13[rand.Next((jiangji))]; }
            if (jiangji == 14) { datas = datas14[rand.Next((jiangji))]; }
            if (jiangji == 15) { datas = datas15[rand.Next((jiangji))]; }
            if (jiangji == 16) { datas = datas16[rand.Next((jiangji))]; }
            if (jiangji == 17) { datas = datas17[rand.Next((jiangji))]; }
            if (jiangji == 18) { datas = datas18[rand.Next((jiangji))]; }
            if (jiangji == 19) { datas = datas19[rand.Next((jiangji))]; }
            if (jiangji == 20) { datas = datas20[rand.Next((jiangji))]; }
            if (jiangji == 21) { datas = datas21[rand.Next((jiangji))]; }

            List<char> result = new List<char>();
            if (rand != null)
            {
                for (int i = Count; i > 0; )
                {
                    char item = datas;
                    if (result.Contains(item))
                        continue;
                    else
                    {
                        result.Add(item);
                        i--;
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 随机抽取
        /// </summary>
        /// <param name="rand">随机数生成器</param>
        /// <returns></returns>
        public char[] ControllerRandomExtract(Random rand)
        {
            int jiangji = Convert.ToInt32(ub.GetSub("jiangji", "/Controls/draw.xml"));
            string[] Rq = ub.GetSub("Rq", "/Controls/draw.xml").Split('#');
            List<char> result = new List<char>();
            if (rand != null)
            {
                //临时变量
                Dictionary<char, int> dict = new Dictionary<char, int>(jiangji);
                if (jiangji == 1)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas1.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas1[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 2)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas2.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas2[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 3)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas3.Count - 1; i >= 0; i--)
                    {
                        dict.Add(datas3[i], rand.Next(100) * weights[i]);
                    }
                }
                if (jiangji == 4)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas4.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas4[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 5)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas5.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas5[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 6)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas6.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas6[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 7)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas7.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas7[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 8)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas8.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas8[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 9)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas9.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas9[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 10)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas10.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas10[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 11)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas11.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas11[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 12)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas12.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas12[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 13)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas13.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas13[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 14)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas14.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas14[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 15)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas15.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas15[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 16)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas16.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas16[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 17)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas17.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas17[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 18)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas18.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas18[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 19)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas19.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas19[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 20)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas20.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas20[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 21)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas21.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas21[i], rand.Next(100) * weights[i]);

                    }
                }
                if (jiangji == 22)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas22.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas22[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 23)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas23.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas23[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 24)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas24.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas24[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 25)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas25.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas25[i], rand.Next(100) * weights[i]);


                    }
                }
                if (jiangji == 26)
                {
                    //为每个项算一个随机数并乘以相应的权值
                    for (int i = datas26.Count - 1; i >= 0; i--)
                    {

                        dict.Add(datas26[i], rand.Next(100) * weights[i]);


                    }
                }
                //排序

                List<KeyValuePair<char, int>> listDict = SortByValue(dict);

                //拷贝抽取权值最大的前Count项

                foreach (KeyValuePair<char, int> kvp in listDict.GetRange(0, Count))
                {

                    result.Add(kvp.Key);

                }

            }

            return result.ToArray();

        }

        /// <summary>

        /// 排序集合

        /// </summary>

        /// <param name="dict"></param>

        /// <returns></returns>

        private List<KeyValuePair<char, int>> SortByValue(Dictionary<char, int> dict)
        {

            List<KeyValuePair<char, int>> list = new List<KeyValuePair<char, int>>();

            if (dict != null)
            {

                list.AddRange(dict);



                list.Sort(

                  delegate(KeyValuePair<char, int> kvp1, KeyValuePair<char, int> kvp2)
                  {

                      return kvp2.Value - kvp1.Value;

                  });

            }

            return list;

        }


        private int _Count;

        /// <summary>

        /// 随机抽取个数

        /// </summary>

        public int Count
        {

            get
            {

                return _Count;

            }

            set
            {

                _Count = value;

            }

        }

    }
    #endregion
}