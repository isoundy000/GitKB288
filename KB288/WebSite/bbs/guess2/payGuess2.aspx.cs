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
using BCW.Common;
using TPR2.Common;

/// <summary>
/// 更新其他胆波胆下注
/// 黄国军 20160706
/// 去除1010接收的内线信息
/// 黄国军 20160309
/// 邵广林 20160617 动态添加usid
/// 姚志光 20160621 活跃抽奖写到页面
/// </summary>
public partial class bbs_guess2_payGuess2 : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        //会员身份页面取会员实体
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        long gold = new BCW.BLL.User().GetGold(meid);
        long money = new BCW.BLL.User().GetMoney(meid);
        int gid = Utils.ParseInt(Utils.GetRequest("gid", "all", 2, @"^[0-9]*$", "竞猜ID无效"));
        string p = Utils.GetRequest("p", "all", 2, @"^\d\d$|^5z$|^5k$|^ot$", "选择无效");

        TPR2.BLL.guess.BaList BaListbll = new TPR2.BLL.guess.BaList();

        TPR2.Model.guess.BaList modelBaList = BaListbll.GetModel(gid);
        if (modelBaList == null)
        {
            Utils.Error("不存在的记录", "");
        }
        if (modelBaList.p_del == 1)
        {
            Utils.Error("不存在的记录", "");
        }
        //联赛限制显示
        string Levens = "";
        if (modelBaList.p_type == 1)
            Levens = "#" + ub.GetSub("SiteLeven1", xmlPath) + "#" + ub.GetSub("SiteLeven2", xmlPath) + "#" + ub.GetSub("SiteLeven3", xmlPath) + "#" + ub.GetSub("SiteLeven4", xmlPath) + "#";
        else
            Levens = "#" + ub.GetSub("SiteLevenb1", xmlPath) + "#" + ub.GetSub("SiteLevenb2", xmlPath) + "#" + ub.GetSub("SiteLevenb3", xmlPath) + "#" + ub.GetSub("SiteLevenb4", xmlPath) + "#";

        if (!Levens.Contains("#" + modelBaList.p_title + "#"))
        {
            Utils.Error("联赛“" + modelBaList.p_title + "”尚未设置，请联系<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=10086") + "\">客服</a>申诉，谢谢", "");
        }
        //--------------------------------------------------------------
        if (string.IsNullOrEmpty(modelBaList.p_score))
        {
            Utils.Error("不存在的波胆盘或已关闭波胆投注", "");
        }
        //根据P值得到比分和赔率
        string bf = "";
        string pl = "";
        string[] score = modelBaList.p_score.Split(',');
        for (int i = 0; i < score.Length; i++)
        {
            string[] Temp = score[i].Split('|');
            if (Temp[0].Replace(":", "") == p)
            {
                bf = Temp[0];
                pl = Temp[1];
                break;
            }
        }
        if (pl == "-1")
        {
            Utils.Error("此波胆暂无数据", "");
        }


        //--------------------------------------------------------------
        Master.Title = modelBaList.p_one + "VS" + modelBaList.p_two + "";
        string ac = Utils.GetRequest("ac", "post", 1, "", "");

        if (ac != "")
        {
            #region 下注确认
            //判断金额是否够了
            int types = 0;
            int payCent = 0;
            string BzText = "";
            if (Utils.ToSChinese(ac) == ub.Get("SiteBz2") + "下注")
            {
                payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                if (money < Convert.ToInt64(payCent))
                {
                    Utils.Error("你的" + ub.Get("SiteBz2") + "不够此次下注", "");
                }
                types = 1;
                BzText = ub.Get("SiteBz2");
            }
            else
            {
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    if (Utils.ToSChinese(ac) == "50万")
                    {
                        payCent = 500000;
                    }
                    else if (Utils.ToSChinese(ac) == "100万")
                    {
                        payCent = 1000000;
                    }
                    else if (Utils.ToSChinese(ac) == "200万")
                    {
                        payCent = 2000000;
                    }
                    else if (Utils.ToSChinese(ac) == "500万")
                    {
                        payCent = 5000000;
                    }
                    else
                    {
                        payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                    }
                }
                else
                {
                    if (Utils.ToSChinese(ac) == "100万")
                    {
                        payCent = 1000000;
                    }
                    else if (Utils.ToSChinese(ac) == "200万")
                    {
                        payCent = 2000000;
                    }
                    else if (Utils.ToSChinese(ac) == "500万")
                    {
                        payCent = 5000000;
                    }
                    else if (Utils.ToSChinese(ac) == "1000万")
                    {
                        payCent = 10000000;
                    }
                    else
                    {
                        payCent = Utils.ParseInt(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "下注无效"));
                    }

                }
                if (gold < Convert.ToInt64(payCent))
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不够此次下注", "");
                }
                types = 0;
                BzText = ub.Get("SiteBz");
            }

            if (modelBaList.p_TPRtime <= DateTime.Now)
            {
                Utils.Error("开赛时间已到，暂停下注", "");
            }

            if (modelBaList.p_result_temp1 != null && modelBaList.p_result_temp2 != null && modelBaList.p_result_temp1 != 0 && modelBaList.p_result_temp2 != 0)
            {
                Utils.Error("开赛时间已到，暂停下注！", "");
            }
            if (modelBaList.p_isondel == 1)
            {
                Utils.Error("已封盘，暂停下注", "");
            }
            if (types == 1)
            {

                if (payCent < Convert.ToInt64(ub.GetSub("SiteSmallPay3", xmlPath)) || payCent > Convert.ToInt64(ub.GetSub("SiteBigPay3", xmlPath)))
                {
                    Utils.Error("" + ub.Get("SiteBz2") + "金额限" + ub.GetSub("SiteSmallPay3", xmlPath) + "-" + ub.GetSub("SiteBigPay3", xmlPath) + "" + ub.Get("SiteBz2") + "", "");
                }

                //每场每ID下注额
                long setPayCents = Utils.ParseInt64(ub.GetSub("SiteIDMaxPay", xmlPath));
                if (setPayCents != 0)
                {
                    long myPayCents = new TPR2.BLL.guess.BaPay().GetBaPaypayCent("bcid=" + gid + " and pType=" + Convert.ToInt32(modelBaList.p_type) + " and payusid=" + meid + " and Types=1");
                    if (myPayCents + Convert.ToInt64(payCent) > setPayCents)
                    {
                        if (myPayCents >= setPayCents)
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz2") + "，欢迎在下场下注", "");
                        }
                        else
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz2") + "，你现在最多可以下注" + (setPayCents - myPayCents) + "" + ub.Get("SiteBz2") + "", "");
                        }

                    }
                }

            }
            if (types == 0)
            {
                //此波胆还可以下注多少
                long MaxCent = Utils.ParseInt64(ub.GetSub("SiteScore" + p + "", xmlPath));
                if (MaxCent > 0)
                {
                    int getp = ScoreType(p);
                    long Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), getp);
                    if (Cent + Convert.ToInt64(payCent) > MaxCent)
                    {
                        if (Cent >= MaxCent)
                        {
                            Utils.Error("此波胆下注上限" + MaxCent + "" + ub.Get("SiteBz") + "，欢迎在下场下注", "");
                        }
                        else
                        {
                            Utils.Error("此波胆下注上限" + MaxCent + "" + ub.Get("SiteBz") + "，你现在最多可以下注" + (MaxCent - Cent) + "" + ub.Get("SiteBz") + "", "");
                        }
                    }
                }

                //每场每ID下注额
                long setPayCents = Utils.ParseInt64(ub.GetSub("SitePayCent", xmlPath));
                if (setPayCents != 0)
                {
                    long myPayCents = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, Convert.ToInt32(modelBaList.p_type), meid);
                    if (myPayCents + Convert.ToInt64(payCent) > setPayCents)
                    {
                        if (myPayCents >= setPayCents)
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz") + "，欢迎在下场下注", "");
                        }
                        else
                        {
                            Utils.Error("系统限制本场每ID下注上限" + setPayCents + "" + ub.Get("SiteBz") + "，你现在最多可以下注" + (setPayCents - myPayCents) + "" + ub.Get("SiteBz") + "", "");
                        }
                    }
                }
            }
            //组合显示串
            string payview = "";
            payview += "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]";
            payview += "压波胆" + ScoreType3(bf) + "(" + pl + "倍),投" + payCent + "" + BzText + "";


            //支付安全提示
            string[] p_pageArr = { "ac", "gid", "payCent", "p" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

            //是否刷屏
            long small = Convert.ToInt64(ub.GetSub("SiteSmallPay", xmlPath));
            long big = Convert.ToInt64(ub.GetSub("SiteBigPay", xmlPath));
            string appName = "LIGHT_GUESS";
            int Expir = Utils.ParseInt(ub.GetSub("SiteExpir", xmlPath));

            BCW.User.Users.IsFresh(appName, Expir, Convert.ToInt64(payCent), small, big);
            //写入bapay
            string mename = new BCW.BLL.User().GetUsName(meid);
            TPR2.Model.guess.BaPay model = new TPR2.Model.guess.BaPay();
            model.Types = types;
            model.payview = payview;
            model.payusid = meid;
            model.payusname = mename;
            model.bcid = gid;
            model.pType = modelBaList.p_type;
            model.PayType = ScoreType(p);
            model.payCent = payCent;
            model.payonLuone = Convert.ToDecimal(pl);
            model.payonLutwo = 0;
            model.payonLuthr = 0;
            model.p_pk = 0;
            modelBaList.p_dx_pk = 0;
            model.p_dx_pk = 0;
            model.p_pn = 0;
            model.paytimes = DateTime.Now;
            model.p_result_temp1 = 0;
            model.p_result_temp2 = 0;
            model.itypes = 0;
            model.state = 0;
            model.p_TPRtime = Convert.ToDateTime(modelBaList.p_TPRtime);
            model.p_oncetime2 = DateTime.Parse("1990-1-1");
            int pid = new TPR2.BLL.guess.BaPay().Add(model);
            //操作币
            if (types == 0)
                new BCW.BLL.User().UpdateiGold(meid, -Convert.ToInt64(payCent), "球彩下注记录" + gid + "-" + pid + "|" + payview);
            else
                new BCW.BLL.User().UpdateiMoney(meid, -Convert.ToInt64(payCent), "球彩下注记录" + gid + "-" + pid + "|" + payview);

            //   活跃抽奖入口_20160621姚志光
            try
            {
                //表中存在虚拟球类记录
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName("虚拟球类"))
                {
                    //投注是否大于设定的限额，是则有抽奖机会
                    if (payCent > new BCW.BLL.tb_WinnersGame().GetPrice("虚拟球类"))
                    {
                        string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                        string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                        int hit = new BCW.winners.winners().CheckActionForAll(1, pid, meid, mename, "球彩", 3);
                        if (hit == 1)
                        {
                            //内线开关 1开
                            if (WinnersGuessOpen == "1")
                            {
                                //发内线到该ID
                                new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                            }
                        }
                    }
                }
            }
            catch { }
            string TzMaxGuest = "#" + ub.GetSub("SiteTzMaxGuest", xmlPath) + "#";
            if (!TzMaxGuest.Contains("#" + gid + "#"))
            {
                long gidPrices = new TPR2.BLL.guess.BaPay().GetBaPayCent(gid, Convert.ToInt32(modelBaList.p_type));
                if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                {
                    if (gidPrices >= 5000000)
                    {
                        new BCW.BLL.Guest().Add(10086, "10086", "球赛ID：" + gid + "超5百W，请检查");

                        ub xml = new ub();
                        xml.ReloadSub(xmlPath); //加载配置
                        xml.dss["SiteTzMaxGuest"] = xml.dss["SiteTzMaxGuest"] + "#" + gid;

                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                else if (Utils.GetTopDomain().Contains("kb288.net"))
                {
                    if (gidPrices >= 50000)
                    {
                        new BCW.BLL.Guest().Add(10086, "10086", "球赛ID：" + gid + "超5W，请检查");
                        new BCW.BLL.Guest().Add(1, "1", "球赛ID：" + gid + "超5W，请检查");
                        new BCW.BLL.Guest().Add(2, "2", "球赛ID：" + gid + "超5W，请检查");

                        ub xml = new ub();
                        xml.ReloadSub(xmlPath); //加载配置
                        xml.dss["SiteTzMaxGuest"] = xml.dss["SiteTzMaxGuest"] + "#" + gid;

                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
                else
                {
                    if (gidPrices >= 2000000)
                    {
                        //new BCW.BLL.Guest().Add(10086, "客服", "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]达200w，请检查");
                        //new BCW.BLL.Guest().Add(1010, "客服", "[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]达200w，请检查");

                        new BCW.BLL.Guest().Add(10086, "10086", "球赛ID：" + gid + "超2百W，请检查");
                        //if (!Utils.GetTopDomain().Contains("tuhao") && !Utils.GetTopDomain().Contains("th"))
                        //{
                        //    new BCW.BLL.Guest().Add(1010, "1010", "球赛ID：" + gid + "超2百W，请检查");
                        //}
                        ub xml = new ub();
                        xml.ReloadSub(xmlPath); //加载配置
                        xml.dss["SiteTzMaxGuest"] = xml.dss["SiteTzMaxGuest"] + "#" + gid;

                        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                    }
                }
            }
            string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]在[url=/bbs/guess2/default.aspx]球彩[/url]:[url=/bbs/guess2/showguess.aspx?gid=" + gid + "]" + modelBaList.p_one + "VS" + modelBaList.p_two + "[/url]下注**" + BzText + "";
            new BCW.BLL.Action().Add(5, 0, meid, "", wText);
            Utils.Success("下注", "恭喜，下注成功..", Utils.getUrl("showGuess.aspx?act=score&amp;gid=" + gid + ""), "1");
            #endregion
        }
        else
        {
            #region 下注
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("波胆盘" + ScoreType3(bf) + "(" + pl + "倍)含本金");
            builder.Append(Out.Tab("</div>", ""));

            string strText = "输入,,";
            string strName = "payCent,gid,p";
            string strType = "num,hidden,hidden";
            string strValu = "'" + gid + "'" + p + "";
            string strEmpt = "true,true,true";
            string strIdea = "/限" + ub.GetSub("SiteSmallPay", xmlPath) + "-" + ub.GetSub("SiteBigPay", xmlPath) + "" + ub.Get("SiteBz") + "/";
            //此波胆还可以下注多少
            long MaxCent = Utils.ParseInt64(ub.GetSub("SiteScore" + p + "", xmlPath));
            if (MaxCent > 0)
            {
                int getp = ScoreType(p);
                long Cent = new TPR2.BLL.guess.BaPay().GetBaPayCent2(gid, Convert.ToInt32(modelBaList.p_type), getp);
                strIdea += "提示:此波胆还可以下注" + (MaxCent - Cent) + "" + ub.Get("SiteBz") + "/";
            }
            string strOthe = "" + ub.Get("SiteBz") + "下注|" + ub.Get("SiteBz2") + "下注,payGuess2.aspx,post,0,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("=" + ub.Get("SiteBz") + "快捷下注=");
            builder.Append(Out.Tab("</div>", "<br />"));
            //快捷下注
            strText = ",,";
            strName = "gid,p";
            strType = "hidden,hidden,hidden";
            strValu = "" + gid + "'" + p + "";
            strEmpt = "true,true";
            strIdea = "";
            if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
            {
                strOthe = "50万|100万|200万|500万,payGuess2.aspx,post,3,other|other|other|other";
            }
            else
            {
                strOthe = "100万|200万|500万|1000万,payGuess2.aspx,post,3,other|other|other|other";

            }

            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            builder.Append("您现在有" + Utils.ConvertGold(gold) + "" + ub.Get("SiteBz") + "/" + Utils.ConvertGold(money) + "" + ub.Get("SiteBz2") + "<br />");
            builder.Append(Out.waplink(Utils.getUrl("showGuess.aspx?gid=" + gid + ""), "取消下注"));
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=1"), "未开投注") + " ");
        builder.Append(Out.waplink(Utils.getUrl("myGuess.aspx?ptype=2"), "历史投注") + "<br />");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "返回球彩首页") + "");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append(Out.waplink(Utils.getUrl("/default.aspx"), "首页") + "-");
        builder.Append(Out.waplink(Utils.getPage("default.aspx"), "上级") + "-");
        builder.Append(Out.waplink(Utils.getUrl("default.aspx"), "球彩") + "");
        builder.Append(Out.Tab("</div>", ""));

    }

    private int ScoreType(string pStr)
    {
        int p = 0;
        if (pStr == "5z")
            p = 150;
        else if (pStr == "5k")
            p = 151;
        else if (pStr == "ot")
            p = 152;
        else
            p = Convert.ToInt32("1" + pStr);

        return p;
    }

    private string ScoreType2(int p)
    {
        string pStr = "";
        if (p == 150)
            pStr = "5z";
        else if (p == 151)
            pStr = "5k";
        else
            pStr = Utils.Mid(p.ToString(), 1, p.ToString().Length);

        return pStr;
    }

    private string ScoreType3(string pStr)
    {
        if (pStr == "5z")
            pStr = "主净胜5球或以上";
        else if (pStr == "5k")
            pStr = "客净胜5球或以上";
        else if (pStr == "ot")
            pStr = "其他盘";

        return pStr;
    }
}