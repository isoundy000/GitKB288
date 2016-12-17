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
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.HB;
using BCW.JS;
//2016/7/1戴少宇

/// <summary>
/// 蒙宗将 20160822 撤掉抽奖值生成
/// 邵广林 20161103 修改口令红包发送后，删除ptype的传值
/// 
/// </summary>
/// 
public partial class bbs_chathb : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/chathb.xml";
    protected int HbTime = Utils.ParseInt(ub.GetSub("HbTime", "/Controls/chathb.xml"));
    protected int hbzd = Utils.ParseInt(ub.GetSub("HbZD", "/Controls/chathb.xml"));

    protected void Page_Load(object sender, EventArgs e)
    {
        int HbOpen = Utils.ParseInt(ub.GetSub("HbOpen", xmlPath));
        if (HbOpen == 1)
        {
            Utils.Error("红包功能维护中", "");
        }
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "红包玩花样")
        {
            act = "hbhy";
        }
        switch (act.Trim())
        {
            case "gethb":
                GetHb();
                break;
            case "hblist":
                HbList();
                break;
            case "myhb":
                myhb();
                break;
            case "hbhy":
                hbwhy();
                break;
            case "mykeys":
                mykeys();
                break;
            case "hblb":
                hblb();
                break;
            default:
                PostHB();
                break;
        }
    }
    //红包玩花样
    private void hbwhy()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int hy = int.Parse(Utils.GetRequest("hy", "all", 1, @"^[1-2]$", "1"));
        int chatid = int.Parse(Utils.GetRequest("id", "all", 1, "", "2147483647"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, "", "2147483647"));
        if (chatid == 2147483647 && speakid == 2147483647)
        {
            Utils.Error("请选择有效的红包群！", Utils.getPage("chat.aspx"));
        }
        List<BCW.HB.Model.HbPost> hbwillpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + chatid + " and State=0 and UserID=" + meid);
        for (int ips = 0; ips < hbwillpost.Count; ips++)
        {
            if (new BCW.HB.BLL.HbPost().Exists(hbwillpost[ips].ID))
            {
                Utils.Error("有红包未完成发送！", Utils.getPage("chat.aspx"));
            }
        }
        Master.Title = "红包玩花样";
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群大厅</a>>");
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=1") + "\">红包群</a>>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=1") + "\">闲聊</a>>");
        }

        builder.Append("红包玩花样");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div >", "<br />"));
        //if (hy == 0)
        //{
        //    builder.Append("<b>他们专属</b>|");
        //    builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=1&amp;id=" + chatid + "") + "\">男女专属</a>|");
        //    builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=2&amp;id=" + chatid + "") + "\">口令密码</a>");
        //}
        //else 
        if (hy == 1)
        {
            //builder.Append("<a  href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=0&amp;id=" + chatid + "") + "\">他们专属</a>|");
            builder.Append("<b>男女专属</b>|");
            builder.Append("<a  href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=2&amp;id=" + chatid + "") + "\">口令密码</a>");
        }
        else
        {
            // builder.Append("<a  href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=0&amp;id=" + chatid + "") + "\">他们专属</a>|");
            builder.Append("<a  href=\"" + Utils.getUrl("chathb.aspx?act=hbhy&amp;hy=1&amp;id=" + chatid + "") + "\">男女专属</a>|");
            builder.Append("<b>口令密码</b>");
        }
        builder.Append(Out.Tab("</div>", ""));
        //if (hy == 0)
        //{
        //    string strText = ",,限定ID:/,,";
        //    string strName = "id,hy,FBI,isno,backurl";
        //    string strType = "hidden,hidden,text,select,hidden";
        //    string strValu = chatid + "'" + hy + "''1" + "'";
        //    string strEmpt = "false,false,false,1|可以领|2|不给领,false";
        //    string strIdea = "/";
        //    string strOthe = "跑步去发送,chathb.aspx?act=posthb,post,1,blue";
        //    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //    builder.Append(Out.Tab("<div >", "<br />"));
        //    builder.Append("有种红包叫想让谁领谁就能领，指定哪些ID可以领取或指定哪些ID不可以领取！注意用英文,号分开哦，最多限如123,321");
        //    builder.Append(Out.Tab("</div>", "<br/>"));

        //}
        //else 
        if (hy == 1)
        {
            string strText = ",,谁可以领:/,,";
            string strName = "id,speakid,hy,FBI,backurl";
            string strType = "hidden,hidden,hidden,select,hidden";
            string strValu = chatid + "'" + speakid + "'" + hy + "'" + "'";
            string strEmpt = "false,false,false,1|美女专享|2|帅哥专享,false";
            string strIdea = "/";
            string strOthe = "跑步去发送,chathb.aspx?act=posthb,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div >", "<br/>"));
            builder.Append("有种红包叫只能看不能领，可设定男女专属！");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            string strText = ",,口令设置:/,";
            string strName = "id,speakid,hy,FBI";
            string strType = "hidden,hidden,hidden,text";
            string strValu = chatid + "'" + speakid + "'" + hy + "'" + "";
            string strEmpt = "false,false,false,false";
            string strIdea = "/";
            string strOthe = "跑步去发送,chathb.aspx?act=posthb,post,1,blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div >", "<br/>"));
            builder.Append("有种红包叫不知道口令不能领，想知道口令那让他去你的个人主页翻一翻吧！");
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=1") + "\">上级</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=1") + "\">上级</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }
    //发红包
    private void PostHB()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "我要发红包";

        int HbNumMax = Utils.ParseInt(ub.GetSub("HbNumMax", xmlPath));
        int HbTime = Utils.ParseInt(ub.GetSub("HbTime", xmlPath));
        long SQmax = Convert.ToInt64(ub.GetSub("HbMoneyMax", xmlPath));
        long PTmax = Convert.ToInt64(ub.GetSub("HbMoneyMax2", xmlPath));
        long SQmin = Convert.ToInt64(ub.GetSub("HbMoneyMin", xmlPath));
        long PTmin = Convert.ToInt64(ub.GetSub("HbMoneyMin2", xmlPath));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        int chatid = int.Parse(Utils.GetRequest("id", "all", 1, "", "2147483647"));
        int hy = int.Parse(Utils.GetRequest("hy", "all", 1, @"^[0-2]$", "7"));
        int isno = int.Parse(Utils.GetRequest("isno", "all", 1, @"^[1-2]$", "0"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, "", "0"));

        string FBI = "";
        string strtest;
        switch (hy)
        {
            case 0:
                strtest = "[他们专属]";
                break;
            case 1:
                strtest = "[男女专属]";
                break;
            case 2:
                strtest = "[口令]";
                break;
            default:
                strtest = "";
                break;
        }
        if (hy == 0)
        {
            FBI = Utils.GetRequest("FBI", "all", 2, @"^[\d*|\d*,]*$", "ID输入错误");

        }
        else if (hy == 1)
        {
            FBI = Utils.GetRequest("FBI", "all", 2, @"^[1-2]$", "性别选择错误");
        }
        else if (hy == 2)
        {
            FBI = Utils.GetRequest("FBI", "all", 2, @"^[^\^]{1,20}$", "红包口令密码限1到20字");
            if (FBI.Contains("#"))
            {
                Utils.Error("无效的输入！", Utils.getUrl("chatroom.aspx?id=" + chatid));
            }
        }
        if (chatid == 2147483647)
        {
            Utils.Error("请选择有效的红包群！", Utils.getUrl("chat.aspx"));
        }
        List<BCW.HB.Model.HbPost> hbwillpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + chatid + " and State=0 and UserID=" + meid);
        for (int ips = 0; ips < hbwillpost.Count; ips++)
        {
            if (new BCW.HB.BLL.HbPost().Exists(hbwillpost[ips].ID))
            {
                Utils.Error("有红包未完成发送！", Utils.getUrl("chatroom.aspx?id=" + chatid));
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群大厅</a>>");
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=1") + "\">红包群</a>>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("game/speak.aspx?id=" + chatid + "&amp;hb=1") + "\">闲聊</a>>");
        }
        builder.Append("我要发红包");
        builder.Append(Out.Tab("</div>", ""));

        if (Utils.ToSChinese(ac) == "确定发送")
        {
            #region
            string[] p_pageArr = { "ac", "act", "Total", "Note", "Number", "Pty", "hy", "FBI", "id", "speakid", "isno" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            long Total = Convert.ToInt64(Utils.GetRequest("Total", "post", 2, @"^(?!0\d)[1-9]\d*$", "红包金额错误！"));
            int Number = int.Parse(Utils.GetRequest("Number", "post", 2, @"^^(?!0\d)[1-9]\d*$", "红包数量错误！"));
            string Note = Utils.GetRequest("Note", "post", 3, @"^[\s\S]{1,12}$", "留言限1-12字内");
            ptype = int.Parse(Utils.GetRequest("Pty", "post", 1, @"^[0-1]$", "0"));
            long mGold = new BCW.BLL.User().GetBasic(meid).iGold;


            if (Number > HbNumMax)
            {
                Utils.Error("亲，请不要一下发这么多个红包！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
            }
            if (ptype == 1)
            {
                #region 普通红包
                //增加判断自身金额是否足够 黄国军 20160131
                if (mGold <= 0 || mGold < (Total * Number))
                {
                    Utils.Error("帐户余额不足，发送失败", "");
                }

                if (Total > PTmax)
                {
                    Utils.Error("这个红包金额太大了！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));

                }
                if (Total < PTmin)
                {
                    Utils.Error("这个红包太小了！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
                }
                builder.Append("普通红包<br />");
                builder.Append(Number + "<br />");
                builder.Append(Total + "<br />");
                builder.Append(Note + "<br />");
                BCW.HB.Model.HbPost model = new BCW.HB.Model.HbPost();
                model.UserID = meid;
                model.num = Number;
                model.money = Total;
                model.RadomNum = "pt";
                model.PostTime = DateTime.Now;
                model.Note = Note;
                model.MaxRadom = Total;
                model.GetIDList = "";
                model.ChatID = chatid;
                int id = new BCW.HB.BLL.HbPost().Add(model);
                new BCW.BLL.User().UpdateiGold(meid, mename, -Total * Number, "在" + chatid + "红包群发普通红包" + strtest + "(" + id + ")消费");



                if (hy != 7)
                {
                    if (isno != 0 && hy == 0)
                    {
                        hy = 10 * isno;
                    }
                    new BCW.HB.BLL.HbPost().UpdateStyle(id, hy, FBI);
                }
                if (chatid != 0)
                {
                    Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "1");
                }
                else
                {
                    Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=1&amp;ptype=" + speakid), "1");
                }
                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                //{
                //    if (chatid != 0)
                //    {
                //        Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "1");
                //    }
                //    else
                //    {
                //        Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=1&amp;ptype=" + speakid), "1");
                //    }
                //}
                //else
                //{
                //    if (chatid != 0)
                //    {
                //        Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "0");
                //    }
                //    else
                //    {
                //        Utils.Success("发普通红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=1&amp;ptype=" + speakid), "0");
                //    }
                //}
                #endregion
            }
            else
            {
                #region 拼手气红包
                //增加判断自身金额是否足够 黄国军 20160131
                if (mGold <= 0 || mGold < Total)
                {
                    Utils.Error("帐户余额不足，发送失败", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
                }

                if (Number * 10 > Total)
                {
                    Utils.Error("亲，这样平均每个红包连10个币都不够！这样不行的哦！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
                }
                if (Total > SQmax)
                {
                    Utils.Error("这个红包金额太大了！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
                }
                if (Total < SQmin)
                {
                    Utils.Error("这个红包太小了！", Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + ""));
                }
                builder.Append("拼手气红包 <br />");

                builder.Append(Number + "<br />");//个数
                builder.Append(Total + "<br />");//金额
                builder.Append(Note + "<br />");//留言
                BCW.HB.Model.HbPost model = new BCW.HB.Model.HbPost();
                string hbsf = HaobaoSF(Number, Total);
                string[] hbsf2 = hbsf.Split('$');
                if (Number == 1)
                {
                    hbsf2[0] = Total.ToString();
                    hbsf2[1] = "1";
                }
                builder.Append(hbsf2[0] + "<br />");
                builder.Append(hbsf2[1] + "<br />");
                model.UserID = meid;
                model.num = Number;
                model.money = Total;
                model.RadomNum = hbsf2[0];
                model.PostTime = DateTime.Now;
                model.Note = Note;
                model.MaxRadom = Convert.ToInt32(hbsf2[1]);
                model.GetIDList = "";
                model.ChatID = chatid;
                int id = new BCW.HB.BLL.HbPost().Add(model);
                new BCW.BLL.User().UpdateiGold(meid, mename, -Total, "在" + chatid + "红包群发拼手气红包" + strtest + "(" + id + ")消费 ");

                if (hy != 7)
                {
                    if (isno != 0 && hy == 0)
                    {
                        hy = 10 * isno;
                    }
                    new BCW.HB.BLL.HbPost().UpdateStyle(id, hy, FBI);
                }
                if (chatid != 0)
                {
                    Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "1");
                }
                else
                {
                    //邵广林 删除红包跳转的ptype
                    Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=" + 1 + ""), "1");//&amp;&amp;ptype=" + speakid
                }
                //if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                //{
                //    if (chatid != 0)
                //    {
                //        Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "1");
                //    }
                //    else
                //    {
                //        Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=" + 1 + "&amp;ptype=" + speakid), "1");
                //    }
                //}
                //else
                //{
                //    if (chatid != 0)
                //    {
                //        Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + 1 + ""), "0");
                //    }
                //    else
                //    {
                //        Utils.Success("发拼手气红包" + strtest + "", "成功发出红包..", Utils.getUrl("game/speak.aspx?hb=" + 1 + "&amp;&amp;ptype=" + speakid), "0");
                //    }
                //}
                #endregion
            }
            #endregion
        }
        else
        {
            #region
            if (ptype == 1)
            {
                #region
                if (hy != 7)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    switch (hy)
                    {
                        case 0:
                            if (isno == 2)
                            {
                                builder.Append("拒绝他们:");
                            }
                            else
                            {
                                builder.Append("他们专属:");
                            }
                            builder.Append(FBI + "<br />");
                            break;
                        case 1:
                            builder.Append("男女专属:");
                            if (FBI.Trim() == "2")
                            {
                                builder.Append("帅哥专享<br />");
                            }
                            else
                            {
                                builder.Append("美女专享<br />");
                            }
                            break;
                        case 2:
                            builder.Append("口令红包:");
                            builder.Append(FBI + "<br />");
                            break;
                        default:
                            strtest = "";
                            break;
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
                string strText = ",,红包个数:/,单个金额:/,留言:/,,,,,,";
                string strName = "id,speakid,Number,Total,Note,Pty,hy,isno,FBI,act,backurl";
                string strType = "hidden,hidden,num,num,text,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = chatid + "'" + speakid + "'" + 0 + "'" + 0 + "'" + "恭喜发财，大吉大利！" + "'" + ptype + "'" + hy + "'" + isno + "'" + FBI + "'posthb'";
                string strEmpt = "false,false,false,false,false,false,false,true,false,false";
                string strIdea = "/";
                string strOthe = "确定发送|reset,chathb.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("每人收到固定金额，<a href=\"" + Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=0&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + "") + "\">改为拼手气红包</a>");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            else
            {
                #region
                if (hy != 7)
                {
                    builder.Append(Out.Tab("<div>", ""));
                    switch (hy)
                    {
                        case 0:
                            if (isno == 2)
                            {
                                builder.Append("拒绝他们:");
                            }
                            else
                            {
                                builder.Append("他们专属:");
                            }
                            builder.Append(FBI + "<br />");
                            break;
                        case 1:
                            builder.Append("男女专属:");
                            if (FBI.Trim() == "2")
                            {
                                builder.Append("帅哥专享<br />");
                            }
                            else
                            {
                                builder.Append("美女专享<br />");
                            }
                            break;
                        case 2:
                            builder.Append("口令红包:");
                            builder.Append(FBI + "<br />");
                            break;
                        default:
                            strtest = "";
                            break;
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
                string strText = ",,红包个数:/,总金额:/,留言:/,,,,,";
                string strName = "id,speakid,Number,Total,Note,Pty,hy,isno,FBI,act";
                string strType = "hidden,hidden,num,num,text,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = chatid + "'" + speakid + "'" + 0 + "'" + 0 + "'" + "恭喜发财，大吉大利！" + "'" + ptype + "'" + hy + "'" + isno + "'" + FBI + "'posthb";
                string strEmpt = "false,false,false,false,false,false,false,false,true,false";
                string strIdea = "/";
                string strOthe = "确定发送|reset,chathb.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("每人抽到金额随机，<a href=\"" + Utils.getUrl("chathb.aspx?act=posthb&amp;ptype=1&amp;id=" + chatid + "&amp;hy=" + hy + "&amp;FBI=" + FBI + "&amp;isno=" + isno + "") + "\">改为普通红包</a>");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            #endregion
        }
        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示：<br/>红包超过" + HbTime + "天未被抢完，剩余金额自动返回您的账户！<br/>单次最多发" + HbNumMax + "个红包，普通红包单个最多" + PTmax + ub.Get("SiteBz") + "、最少" + PTmin + ub.Get("SiteBz") + "，拼手气红包总额最多" + SQmax + ub.Get("SiteBz") + "、最少" + SQmin + ub.Get("SiteBz") + "");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=1") + "\">上级</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("game/speak.aspx?id=" + chatid + "&amp;hb=1") + "\">上级</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }
    //抢红包
    private void GetHb()
    {
        Master.Title = "我要抢红包";
        int ID = int.Parse(Utils.GetRequest("id", "all", 1, "", ""));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        BCW.HB.Model.HbPost mo = new BCW.HB.BLL.HbPost().GetModel(ID);
        #region 判断聊天室是否正确
        int lastchatid = new BCW.BLL.User().GetEndChatID(meid);
        if (lastchatid != mo.ChatID && mo.ChatID != 0)
        {
            Utils.Error("不是正确的聊天室！", "");
        }

        #endregion
        #region 个人性别判断
        if (mo.Style == 1)
        {
            #region 性别专属判断
            int hbsex = int.Parse(mo.Keys);
            int sex = new BCW.BLL.User().GetSex(meid);
            if (hbsex != sex)
            {
                if (mo.ChatID != 0)
                {
                    if (sex == 1)
                    {
                        Utils.Error("这红包是给帅哥的哦，美女请别抢！", Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1"));
                    }
                    else if (sex == 2)
                    {
                        Utils.Error("这红包是给美女的哦，帅哥请别抢！", Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1"));
                    }
                }
                else
                {
                    if (sex == 1)
                    {
                        Utils.Error("这红包是给帅哥的哦，美女请别抢！", Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1"));
                    }
                    else if (sex == 2)
                    {
                        Utils.Error("这红包是给美女的哦，帅哥请别抢！", Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1"));
                    }
                }
            }
            #endregion
        }
        else if (mo.Style == 10)
        {
            string[] idislist = mo.Keys.Split(',');
            bool exists = ((IList)idislist).Contains(meid.ToString());
            if (!exists)
            {
                if (mo.ChatID != 0)
                {
                    Utils.Error("红包主人说这个红包没你份！", Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1"));
                }
                else
                {
                    Utils.Error("红包主人说这个红包没你份！", Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1"));
                }
            }
        }
        else if (mo.Style == 20)
        {
            string[] idislist = mo.Keys.Split(',');
            bool exists = ((IList)idislist).Contains(meid.ToString());
            if (mo.ChatID != 0)
            {
                Utils.Error("红包主人说这个红包没你份！", Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1"));
            }
            else
            {
                Utils.Error("红包主人说这个红包没你份！", Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1"));
            }
        }
        #endregion

        if (new BCW.HB.BLL.HbPost().Exists2(ID, 1))
        {
            MatchCollection leyr = Regex.Matches(mo.GetIDList, "#");
            int ele = leyr.Count;
            if (ele < mo.num)
            {
                string[] sNum = Regex.Split(mo.GetIDList, "#");
                int yn = 0;
                for (int a = 0; a < sNum.Length - 1; a++)
                {
                    if (meid == int.Parse(sNum[a].Trim()))
                    {
                        yn++;
                    }
                }
                if (yn > 0)
                {
                    // builder.Append("你已经抢到了红包！");
                    // Utils.Success("抢红包", "你已经抢到了红包!正在返回..", Utils.getPage("chatroom.aspx?id=" + mo.ChatID), "");
                    builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                    builder.Append("你已经抢过了红包！");
                    builder.Append(Out.Tab("</div>", "<br />"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + mo.ID) + "\">" + "看看大家的手气" + "</a>");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                    if (mo.ChatID != 0)
                    {
                        Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                        Master.Gourl = Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                        builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回红包群</a>");
                    }
                    else
                    {
                        Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                        Master.Gourl = Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                        builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1") + "\">返回闲聊</a>");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    if (mo.Style == 2)
                    {
                        #region 口令红包判断
                        string postkey = Utils.GetRequest("postkey", "post", 1, "", "");
                        if (postkey == "")
                        {
                            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                            builder.Append("请输入口令密码");
                            builder.Append(Out.Tab("</div>", "<br />"));
                            string strText = ",,,";
                            string strName = "act,id,postkey,backurl";
                            string strType = "hidden,hidden,text,hidden";
                            string strValu = "gethb'" + ID + "''";
                            string strEmpt = "false,false,false,false";
                            string strIdea = "";
                            string strOthe = "确定,chathb.aspx,post,1,red";
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回红包群</a>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                        {
                            if (postkey != mo.Keys)
                            {
                                if (mo.ChatID != 0)
                                {
                                    Utils.Error("亲，口令输错了！换个口令试试吧", Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1"));
                                }
                                else
                                {
                                    Utils.Error("亲，口令输错了！换个口令试试吧", Utils.getPage("game/speak.aspx?ptype=" + mo.ChatID + "&amp;hb=1"));
                                }

                            }
                            new BCW.HB.BLL.HbPost().UpdateGetIDList(ID, mo.GetIDList + meid.ToString() + "#");
                            long getgold = 0;
                            BCW.HB.Model.HbGetNote mods = new BCW.HB.Model.HbGetNote();
                            if (mo.RadomNum.Trim() != "pt")
                            {
                                string[] getmoney = mo.RadomNum.Split('#');
                                mods.GetID = meid.ToString();
                                mods.GetMoney = Convert.ToInt32(getmoney[ele]);
                                mods.GetTime = DateTime.Now;
                                mods.HbID = ID.ToString();
                                mods.IsMax = 0;
                                getgold = Convert.ToInt32(getmoney[ele]);
                                if (ele + 1 == mo.MaxRadom)
                                {
                                    mods.IsMax = 1;
                                }
                                int id = new BCW.HB.BLL.HbGetNote().Add(mods);
                            }
                            else
                            {
                                mods.GetID = meid.ToString();
                                mods.GetMoney = mo.money;
                                getgold = mo.money;
                                mods.GetTime = DateTime.Now;
                                mods.HbID = ID.ToString();
                                mods.IsMax = 0;
                                int id = new BCW.HB.BLL.HbGetNote().Add(mods);
                            }
                            string ptname = new BCW.BLL.User().GetUsName(mo.UserID);
                            string mename = new BCW.BLL.User().GetUsName(meid);
                            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                            builder.Append("恭喜你抢到了" + ptname + "的红包！获得了" + getgold + ub.Get("SiteBz") + "。");
                            if (mo.ChatID == 0)
                            {
                                new BCW.BLL.Guest().Add(1, meid, mename, "恭喜您在游戏闲聊抢到了[url=/bbs/uinfo.aspx?uid=" + mo.UserID + "]" + ptname + "[/url]的红包，获得了" + getgold + ub.Get("SiteBz") + "。[url=/bbs/game/speak.aspx?id=" + mo.ChatID + "]马上进入闲聊[/url]");
                            }
                            else
                            {
                                BCW.Model.Chat mochat = new BCW.BLL.Chat().GetChat(mo.ChatID);
                                new BCW.BLL.Guest().Add(1, meid, mename, "恭喜您在" + mochat.ChatName + "红包群抢到了[url=/bbs/uinfo.aspx?uid=" + mo.UserID + "]" + ptname + "[/url]的红包，获得了" + getgold + ub.Get("SiteBz") + "。[url=/bbs/chatroom.aspx?id=" + mo.ChatID + "]马上进入红包群[/url]");
                            }
                            new BCW.BLL.User().UpdateiGold(meid, mename, getgold, "抢红包" + mo.ID + "收入");
                            builder.Append(Out.Tab("</div>", "<br />"));
                            builder.Append(Out.Tab("<div>", ""));
                            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + mo.ID) + "\">" + "看看大家的手气" + "</a>");
                            builder.Append(Out.Tab("</div>", ""));
                            builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                            if (mo.ChatID != 0)
                            {
                                Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                                Master.Gourl = Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                                builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回红包群</a>");
                            }
                            else
                            {
                                Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                                Master.Gourl = Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                                builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回闲聊</a>");
                            }
                            builder.Append(Out.Tab("</div>", ""));

                        }
                        #endregion
                    }
                    else
                    {
                        new BCW.HB.BLL.HbPost().UpdateGetIDList(ID, mo.GetIDList + meid.ToString() + "#");
                        BCW.HB.Model.HbGetNote mods = new BCW.HB.Model.HbGetNote();
                        long getgold = 0;
                        if (mo.RadomNum.Trim() != "pt")
                        {
                            string[] getmoney = mo.RadomNum.Split('#');
                            mods.GetID = meid.ToString();
                            mods.GetMoney = Convert.ToInt32(getmoney[ele]);
                            mods.GetTime = DateTime.Now;
                            mods.HbID = ID.ToString();
                            mods.IsMax = 0;
                            getgold = Convert.ToInt32(getmoney[ele]);
                            if (ele + 1 == mo.MaxRadom)
                            {
                                mods.IsMax = 1;
                            }
                            int id = new BCW.HB.BLL.HbGetNote().Add(mods);
                        }
                        else
                        {
                            mods.GetID = meid.ToString();
                            mods.GetMoney = mo.money;
                            getgold = mo.money;
                            mods.GetTime = DateTime.Now;
                            mods.HbID = ID.ToString();
                            mods.IsMax = 0;
                            int id = new BCW.HB.BLL.HbGetNote().Add(mods);
                        }
                        string ptname = new BCW.BLL.User().GetUsName(mo.UserID);
                        string mename = new BCW.BLL.User().GetUsName(meid);
                        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                        builder.Append("恭喜你抢到了" + ptname + "的红包！获得了" + getgold + ub.Get("SiteBz") + "。");
                        if (mo.ChatID == 0)
                        {
                            new BCW.BLL.Guest().Add(1, meid, mename, "恭喜您在游戏闲聊抢到了[url=/bbs/uinfo.aspx?uid=" + mo.UserID + "]" + ptname + "[/url]的红包，获得了" + getgold + ub.Get("SiteBz") + "。[url=/bbs/game/speak.aspx?id=" + mo.ChatID + "]马上进入闲聊[/url]");
                        }
                        else
                        {
                            BCW.Model.Chat mochat = new BCW.BLL.Chat().GetChat(mo.ChatID);
                            new BCW.BLL.Guest().Add(1, meid, mename, "恭喜您在" + mochat.ChatName + "红包群抢到了[url=/bbs/uinfo.aspx?uid=" + mo.UserID + "]" + ptname + "[/url]的红包，获得了" + getgold + ub.Get("SiteBz") + "。[url=/bbs/chatroom.aspx?id=" + mo.ChatID + "]马上进入红包群[/url]");
                        }
                        new BCW.BLL.User().UpdateiGold(meid, mename, getgold, "抢红包(" + mo.ID + ")收入");
                        builder.Append(Out.Tab("</div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + mo.ID) + "\">" + "看看大家的手气" + "</a>");
                        builder.Append(Out.Tab("</div>", ""));
                        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));

                        if (mo.ChatID != 0)
                        {
                            Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                            Master.Gourl = Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回红包群</a>");
                        }
                        else
                        {
                            Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                            Master.Gourl = Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回闲聊</a>");
                        }
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    BCW.HB.Model.HbPost max = new BCW.HB.BLL.HbPost().GetModel(ID);
                    MatchCollection leyr2 = Regex.Matches(max.GetIDList, "#");
                    int ele2 = leyr2.Count;
                    if (ele2 == max.num)
                    {
                        string[] maxID = max.GetIDList.Split('#');
                        int maxIDs = int.Parse(maxID[mo.MaxRadom - 1]);
                        string getname = new BCW.BLL.User().GetUsName(maxIDs);
                        string whoname = new BCW.BLL.User().GetUsName(mo.UserID);
                        BCW.Model.ChatText addmodel = new BCW.Model.ChatText();
                        addmodel.ChatId = mo.ChatID;
                        addmodel.Content = whoname + "的红包" + DT.DateDiff(DateTime.Now, mo.PostTime) + "被抢完。" + getname + "是运气王！";
                        addmodel.UsID = 0;
                        addmodel.UsName = "<b>系统</b>";
                        addmodel.ToID = 0;
                        addmodel.ToName = string.Empty;
                        addmodel.IsKiss = 1;
                        addmodel.AddTime = DateTime.Now;
                        new BCW.BLL.ChatText().Add(addmodel);
                    }
                }
            }
            else
            {

                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                builder.Append("红包已抢光了！");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + mo.ID) + "\">" + "看看大家的手气" + "</a>");
                builder.Append(Out.Tab("</div>", ""));
                builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
                if (mo.ChatID != 0)
                {
                    Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                    Master.Gourl = Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                    builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回红包群</a>");
                }
                else
                {
                    Master.Refresh = hbzd;//2秒后跳转到以下地址(可缺省)
                    Master.Gourl = Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1");//跳到的地址(可缺省)
                    builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + mo.ChatID + "&amp;hb=1") + "\">返回闲聊</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else if (new BCW.HB.BLL.HbPost().Exists2(ID, 2))
        {
            Utils.Error("该红包已过期!", "");
        }
        else
        {
            Utils.Error("该红包已不存在!", "");
        }
    }
    //当前红包列表
    private void HbList()
    {
        Master.Title = "红包列表";
        string hbid = Utils.GetRequest("id", "all", 1, "", "");
        int meid = new BCW.User.Users().GetUsId();
        BCW.HB.Model.HbPost hbpost = new BCW.HB.BLL.HbPost().GetModel(Convert.ToInt32(hbid));
        BCW.HB.Model.HbGetNote meget = new BCW.HB.BLL.HbGetNote().GetModelByGetID(meid, Convert.ToInt32(hbid));
        List<BCW.HB.Model.HbGetNote> hbnote = new BCW.HB.BLL.HbGetNote().GetModelList("HbID=" + hbid);
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        builder.Append("红包详情");
        builder.Append(Out.Tab("</div>", ""));
        string postname = new BCW.BLL.User().GetUsName(Convert.ToInt32(hbpost.UserID));
        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
        builder.Append("<b style=\"color:#8A7B66\">" + postname + "</b>的<b style=\"color:red\">红包</b>(" + hbid + ")");
        if (hbpost.RadomNum.Trim() == "pt")
        {
            builder.Append("<br />");
        }
        else
        {
            builder.Append("<b style=\"background:#FF8C00;color:#FFF\">拼</b><br />");
        }
        builder.Append("<b style=\"color:#8A7B88\">" + hbpost.Note + "</b><br/>");
        if (new BCW.HB.BLL.HbGetNote().Exists2(meid, Convert.ToInt32(hbid)))
        {
            builder.Append("<b style=\"color:red\">" + meget.GetMoney + ub.Get("SiteBz") + "</b>");
        }
        builder.Append(Out.Tab("</div>", ""));
        MatchCollection leyr = Regex.Matches(hbpost.GetIDList, "#");
        int ele = leyr.Count;
        builder.Append(Out.Tab("<div>", "<br />"));
        if (ele < hbpost.num)
        {
            builder.Append("已领取" + ele + "/" + hbpost.num + "个");
        }
        else
        {
            builder.Append(hbpost.num + "个红包被已抢光");
        }
        if (meid == Convert.ToInt32(hbpost.UserID))
        {
            if (hbpost.RadomNum.Trim() == "pt")
            {
                builder.Append(",共" + hbpost.money * hbpost.num + ub.Get("SiteBz") + "<br />");
            }
            else
            {
                builder.Append(",共" + hbpost.money + ub.Get("SiteBz") + "<br />");
            }
        }
        else
        {
            builder.Append("<br />");
        }
        builder.Append(Out.Tab("</div>", ""));
        for (int iso = 0; iso < hbnote.Count; iso++)
        {
            if (iso % 2 == 0)
                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            else
            {
                if (iso == 1)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                    builder.Append(Out.Tab("<div>", "<br />"));
            }
            string getname = new BCW.BLL.User().GetUsName(Convert.ToInt32(hbnote[iso].GetID));
            builder.Append("<b>" + getname + "</b>抢到了<b>" + hbnote[iso].GetMoney + ub.Get("SiteBz") + "</b><br />" + hbnote[iso].GetTime.ToString("hh:mm:ss"));
            if (hbnote[iso].IsMax == 1 && ele == hbpost.num)
            {
                builder.Append("<img src=\"game/img/wg.png\"  alt=\"load\" />");
                builder.Append("<b style=\"color:#FF8C00\">手气最佳</b>");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        builder.Append(Out.Tab("<div>", ""));
        if (hbpost.PostTime.AddDays(HbTime) > DateTime.Now)
        {
            if (Utils.Isie() || Utils.GetUA().ToLower().Contains("opera/8"))
            {
                string daojishi = new BCW.JS.somejs().daojishi("divhb", hbpost.PostTime.AddDays(HbTime));
                builder.Append("温馨提示：红包还有" + daojishi + "过期！");
            }
            else
            {
                builder.Append("温馨提示：红包还有" + DT.DateDiff(DateTime.Now, hbpost.PostTime.AddDays(HbTime)) + "过期！");
            }
        }
        else
        {
            builder.Append("温馨提示：红包已过期！");
        }
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (hbpost.ChatID != 0)
        {
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + hbpost.ChatID + "&amp;hb=1") + "\">红包群</a>-");
            builder.Append("<a href=\"" + Utils.getPage("chathb.aspx?act=myhb" + "&amp;chatid=" + hbpost.ChatID) + "\">我的红包</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + hbpost.ChatID + "&amp;hb=1") + "\">闲聊</a>-");
            builder.Append("<a href=\"" + Utils.getPage("chathb.aspx?act=myhb" + "&amp;speakid=" + hbpost.ChatID) + "\">我的红包</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }
    //拼手气红包算法
    public string HaobaoSF(int number, long total)////number红包数——total红包总额
    {
        long money = 0;
        ////最小红包
        double min = 10;
        double max;
        int i = 1;
        ArrayList math = new ArrayList();
        string lists = "";
        long totals = total;
        while (i < number)
        {
            //保证即使一个红包是最大的了,后面剩下的红包,每个红包也不会小于最小值
            max = total - min * (number - i);
            int k = (number - i) / 2;
            //保证最后两个人拿的红包不超出剩余红包
            if (number - i <= 2)
            {
                k = number - i;
            }
            //最大的红包限定的平均线上下
            max = max / k;

            //保证每个红包大于最小值,又不会大于最大值;
            Random rdm = new Random();
            double s = rdm.NextDouble();
            money = Convert.ToInt64(min * 100 + s * (max * 100 - min * 100 + 1));
            money = money / 100;
            //保留两位小数
            while (money == 0)
            {
                s = rdm.NextDouble();
                money = Convert.ToInt32(min * 100 + s * (max * 100 - min * 100 + 1));
                money = money / 100;
            }
            if (money > totals / 2)
            {
                money = totals / 2;
            }
            if (money < 10)
            {
                money = 10;
            }
            total = total * 100 - money * 100;
            total = total / 100;
            math.Add(money);
            lists = lists + money.ToString() + "#";
            //Response.Write("第" + i + "个人拿到" + money + "剩下" + total + "<br />");
            i++;
            //最后一个人拿走剩下的红包
            if (i == number)
            {
                math.Add(total);
                lists = lists + total + "#";
                //Response.Write("第" + i + "个人拿到" + total + "剩下0" + "<br />");
            }
        }
        if (number == 1)
        {
            math.Add(total);
            lists = money.ToString();
            // Response.Write("第" + i + "个人拿到" + total + "剩下0" + "<br />");
        }
        long maxmoney = 0;
        for (int v = 0; v < math.Count; v++)
        {
            if (Convert.ToInt64(math[v].ToString()) > maxmoney)
            {
                maxmoney = Convert.ToInt64(math[v].ToString());
            }
        }
        //取数组中最大的一个值的索引
        // Response.Write("本轮发红包中第" + (math.IndexOf(maxmoney) + 1) + "个人手气最佳" + "<br />");
        lists = lists + "$" + (math.IndexOf(maxmoney) + 1).ToString();
        return lists;
    }
    //我的红包
    private void myhb()
    {

        int meid = new BCW.User.Users().GetUsId();
        int posget = int.Parse(Utils.GetRequest("posget", "all", 1, @"^[0-1]$", "0"));
        int chatid = int.Parse(Utils.GetRequest("chatid", "all", 1, @"\d*", "0"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"\d*", "0"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, @"\d*", "2147483647"));
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "我的红包";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (chatid != 0)
        {
            if (posget == 0)
            {
                builder.Append("收到的红包|");
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;posget=1&amp;chatid=" + chatid + "&amp;hb=" + hbgn) + "\">发出的红包</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;chatid=" + chatid + "&amp;hb=" + hbgn) + "\">收到的红包|</a>");
                builder.Append("发出的红包");
            }
        }
        else if (speakid != 0)
        {
            if (posget == 0)
            {
                builder.Append("收到的红包|");
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;posget=1&amp;speakid=" + speakid + "&amp;hb=" + hbgn) + "\">发出的红包</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;speakid=" + speakid + "&amp;hb=" + hbgn) + "\">收到的红包|</a>");
                builder.Append("发出的红包");
            }
        }
        else
        {
            if (posget == 0)
            {
                builder.Append("收到的红包|");
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb&amp;posget=1") + "\">发出的红包</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=myhb") + "\">收到的红包|</a>");
                builder.Append("发出的红包");
            }
        }

        builder.Append(Out.Tab("</div>", "<br />"));
        int pageIndex;
        int recordCount;
        int pageSize = 5;
        string strWhere = "";
        string[] pageValUrl = { "ptype", "act", "posget", "id", "hb", "chatid", "speak", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        if (posget == 0)
        {
            int getcount = new BCW.HB.BLL.HbGetNote().GetRecordCount("GetID=" + meid);
            int maxcount = new BCW.HB.BLL.HbGetNote().GetRecordCount("GetID=" + meid + " and IsMAx=1");
            long getall = new BCW.HB.BLL.HbGetNote().GetAllMoney("GetID=" + meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(mename + "共收到<br /><b style=\"color:red\">" + getall + ub.Get("SiteBz") + "</b><br />");
            builder.Append("收到红包" + getcount + "个|<b style=\"color:#8A7B66\">" + maxcount + "</b>个手气最佳");
            builder.Append(Out.Tab("</div>", ""));
            // 开始读取列表
            strWhere = "GetID=" + meid;
            IList<BCW.HB.Model.HbGetNote> listHbGet = new BCW.HB.BLL.HbGetNote().GetListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHbGet.Count > 0)
            {
                int k = 1;
                foreach (BCW.HB.Model.HbGetNote n in listHbGet)
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
                    BCW.HB.Model.HbPost getsome = new BCW.HB.BLL.HbPost().GetModel(Convert.ToInt32(n.HbID));
                    string postname = new BCW.BLL.User().GetUsName(getsome.UserID);
                    string leix = "";
                    if (getsome.RadomNum.Trim() == "pt")
                    {
                        leix = "";
                    }
                    else
                    {
                        leix = "<b style=\" background:#FF8C00;color:#FFF\">拼</b>";
                    }
                    builder.Append("<table onclick=\"window.open('" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + n.HbID + "") + "', '_self')\">");
                    builder.Append("<tr><td><b>" + postname + "</b>" + leix + "</td><td><b>" + n.GetMoney + ub.Get("SiteBz") + "</b></td></tr>");
                    builder.Append("<tr><td>" + n.GetTime + "</td><td></td></tr>");
                    k++;
                    builder.Append("</table>");
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
        else
        {
            int postcount = new BCW.HB.BLL.HbPost().GetRecordCount("UserID=" + meid);
            long postsqall = new BCW.HB.BLL.HbPost().PostMoney("UserID=" + meid);
            long postptall = new BCW.HB.BLL.HbPost().PostMoney2("UserID=" + meid);
            long postall = postsqall + postptall;
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(mename + "共发出<br /><b style=\"color:red\">" + postall + ub.Get("SiteBz") + "</b><br />");
            builder.Append("发出红包总数<b style=\"color:#8A7B66\">" + postcount + "</b>个");
            builder.Append(Out.Tab("</div>", ""));
            // 开始读取列表
            strWhere = "UserID=" + meid;
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
                    string leixi = "";
                    string isnull = "";
                    MatchCollection leyr = Regex.Matches(n.GetIDList, "#");
                    int ele = leyr.Count;
                    builder.Append("<table onclick=\"window.open('" + Utils.getUrl("chathb.aspx?act=hblist&amp;id=" + n.ID + "") + "', '_self')\">");
                    if (n.RadomNum.Trim() == "pt")
                    {
                        leixi = "普通红包";
                    }
                    else
                    {
                        leixi = "拼手气红包";
                    }
                    if (ele < n.num && n.PostTime.AddDays(HbTime) < DateTime.Now)
                    {
                        isnull = "已过期";
                    }
                    builder.Append("<tr><td><b>" + leixi + "</b></td><td><b>" + n.money + ub.Get("SiteBz") + "</b></td></tr>");
                    builder.Append("<tr><td>" + n.PostTime + "</td><td>" + ele + "/" + n.num + isnull + "</td></tr>");
                    builder.Append("</table>");
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

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">红包群</a>");
        }
        else if (speakid != 2147483647)
        {
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?hb=" + hbgn + "&amp;ptype=" + speakid) + "\">闲聊</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("/default.aspx") + "\">首页</a>-");
            builder.Append("<a href=\"" + Utils.getPage("/bbs/finance.aspx") + "\">金融</a>-");
            builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        }
        builder.Append(Out.Tab("</div>", ""));
    }
    //我的红包口令
    private void mykeys()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, "", ""));
        string mename = new BCW.BLL.User().GetUsName(uid);
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        Master.Title = "红包口令密码";
        builder.Append(mename + "的红包口令密码");
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string strWhere = "";
        string[] pageValUrl = { "usid", "act", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        // 开始读取列表
        strWhere = "UserID=" + uid + " and Style=2 and State=1";

        IList<BCW.HB.Model.HbPost> listHbGet = new BCW.HB.BLL.HbPost().GetListByPage(pageIndex, pageSize, strWhere, out recordCount);
        if (listHbGet.Count > 0)
        {
            int k = 1;
            foreach (BCW.HB.Model.HbPost n in listHbGet)
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
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.Keys);
                //邵广林 增加ChatID>0  20161104
                if (n.ChatID > 0)
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?id=" + n.ChatID + "") + "\">.[进入红包群]</a>");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?id=" + n.ChatID + "") + "\">.[进入闲聊]</a>");
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
        builder.Append("<a href=\"" + Utils.getPage("/bbs/uinfo.aspx?act=view&amp;uid=" + uid) + "\">上级</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx") + "\">空间</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //红包列表
    private void hblb()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        Master.Title = "全部红包列表";
        int chatid = int.Parse(Utils.GetRequest("id", "all", 1, "", "0"));
        int lzt = int.Parse(Utils.GetRequest("lzt", "all", 1, "", "1"));
        int hbgn = int.Parse(Utils.GetRequest("hb", "all", 1, @"\d*", "0"));
        int speakid = int.Parse(Utils.GetRequest("speakid", "all", 1, @"\d*", "0"));
        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chat.aspx") + "\">红包群大厅</a>>");
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "") + "\">红包群</a>>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=" + hbgn + "&amp;ptype=" + speakid) + "\">闲聊</a>>");
        }

        builder.Append("全部红包列表");
        builder.Append(Out.Tab("</div>", "<br/>"));
        builder.Append(Out.Tab("<div class=\"class\">", ""));
        if (lzt == 1)
        {
            builder.Append("红包进行中|");
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=0&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">已抢完</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=2&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">已过期</a>");
        }
        else if (lzt == 0)
        {
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=1&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">进行中</a>|");
            builder.Append("红包已抢完|");
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=2&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">已过期</a>");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=1&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">进行中</a>|");
            builder.Append("<a href=\"" + Utils.getUrl("chathb.aspx?act=hblb&amp;lzt=0&amp;id=" + chatid + "&amp;hb=" + hbgn + "&amp;speakid=" + speakid) + "\">已抢完</a>|");
            builder.Append("红包已过期");

        }
        builder.Append(Out.Tab("</div>", ""));
        string strtest;
        int sbb = 1;
        List<BCW.HB.Model.HbPost> hbpost = new BCW.HB.BLL.HbPost().GetModelList("ChatID=" + chatid + " and State=1");
        if (lzt == 1)
        {
            for (int iso = hbpost.Count - 1; iso >= 0; iso--)
            {

                switch (hbpost[iso].Style)
                {
                    case 10:
                        strtest = "[他们专属]";
                        break;
                    case 20:
                        strtest = "[拒绝他们]";
                        break;
                    case 1:
                        strtest = "[男女专属]";
                        break;
                    case 2:
                        strtest = "[口令]";
                        break;
                    default:
                        strtest = "";
                        break;
                }
                string postname = new BCW.BLL.User().GetUsName(hbpost[iso].UserID);
                MatchCollection leyrm = Regex.Matches(hbpost[iso].GetIDList, "#");
                int elem = leyrm.Count;
                if (elem < hbpost[iso].num)
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
                    builder.Append(sbb + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hbpost[iso].UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + postname + " </a>(" + hbpost[iso].UserID + ")<a href=\"" + Utils.getUrl("chathb.aspx?act=gethb&amp;id=" + hbpost[iso].ID) + "\">" + strtest + "红包：" + hbpost[iso].Note + "</a><br/>");
                    sbb++;

                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            if (sbb == 1)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                if (chatid != 0)
                {
                    builder.Append("没有红包可以抢！<br/><a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">马上去发个红包吧>></a>");
                }
                else
                {
                    builder.Append("没有红包可以抢！<br/><a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=" + hbgn + "&amp;ptype=" + speakid) + "\">马上去发个红包吧>></a>>");

                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else if (lzt == 0)
        {
            for (int iso = hbpost.Count - 1; iso >= 0; iso--)
            {
                switch (hbpost[iso].Style)
                {
                    case 10:
                        strtest = "[他们专属]";
                        break;
                    case 20:
                        strtest = "[拒绝他们]";
                        break;
                    case 1:
                        strtest = "[男女专属]";
                        break;
                    case 2:
                        strtest = "[口令]";
                        break;
                    default:
                        strtest = "";
                        break;
                }
                string postname = new BCW.BLL.User().GetUsName(hbpost[iso].UserID);
                MatchCollection leyrm = Regex.Matches(hbpost[iso].GetIDList, "#");
                int elem = leyrm.Count;
                if (elem < hbpost[iso].num)
                {
                }
                else
                {
                    if (sbb % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                    {
                        if (sbb == 1)
                            builder.Append(Out.Tab("<div>", "<br/>"));
                        else
                            builder.Append(Out.Tab("<div>", ""));
                    }

                    builder.Append(sbb + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + hbpost[iso].UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + postname + " </a>(" + hbpost[iso].UserID + ")<a href=\"" + Utils.getUrl("chathb.aspx?act=gethb&amp;id=" + hbpost[iso].ID) + "\">" + strtest + "红包：" + hbpost[iso].Note + "</a><br/>");
                    sbb++;
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            if (sbb == 1)
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                if (chatid != 0)
                {
                    builder.Append("红包都抢完了！<br/><a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">马上去发个红包吧>></a>");
                }
                else
                {
                    builder.Append("红包都抢完了！<br/><a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=" + hbgn + "&amp;ptype=" + speakid) + "\">马上去发个红包吧>></a>>");

                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            int pageIndex;
            int recordCount;
            int pageSize = 5;
            int bh = 0;
            string strWhere = "";
            string[] pageValUrl = { "id", "act", "lzt", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            strWhere = "ChatID=" + chatid + " and State=2";
            IList<BCW.HB.Model.HbPost> listHbGet = new BCW.HB.BLL.HbPost().GetListByPage(pageIndex, pageSize, strWhere, out recordCount);
            if (listHbGet.Count > 0)
            {
                int k = 1;
                foreach (BCW.HB.Model.HbPost n in listHbGet)
                {
                    switch (n.Style)
                    {
                        case 10:
                            strtest = "[他们专属]";
                            break;
                        case 20:
                            strtest = "[拒绝他们]";
                            break;
                        case 1:
                            strtest = "[男女专属]";
                            break;
                        case 2:
                            strtest = "[口令]";
                            break;
                        default:
                            strtest = "";
                            break;
                    }
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string postname = new BCW.BLL.User().GetUsName(n.UserID);
                    bh = pageSize * (pageIndex - 1) + k;
                    builder.Append(bh + ".<a href=\"" + Utils.getUrl("uinfo.aspx?uid=" + n.UserID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + postname + " </a>(" + n.UserID + ")<a href=\"" + Utils.getUrl("chathb.aspx?act=gethb&amp;id=" + n.ID) + "\">" + strtest + "红包：" + n.Note + "</a><br/>");
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Tab("<div>", "<br />"));
                if (chatid != 0)
                {
                    builder.Append("红包都过期了<br/><a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">马上去发个红包吧>></a>");
                }
                else
                {
                    builder.Append("红包都过期了<br/><a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=" + hbgn + "&amp;ptype=" + speakid) + "\">马上去发个红包吧>></a>>");

                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }

        builder.Append(Out.Tab("<div class=\"title\">", "<br />"));
        if (chatid != 0)
        {
            builder.Append("<a href=\"" + Utils.getPage("chatroom.aspx?id=" + chatid + "&amp;hb=" + hbgn) + "\">上级</a>-");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getPage("game/speak.aspx?id=" + chatid + "&amp;hb=" + hbgn + "&amp;ptype=" + speakid) + "\">上级</a>-");
        }
        builder.Append("-<a href=\"" + Utils.getUrl("/bbs/chathb.aspx?act=myhb&amp;chatid=" + chatid + "&amp;hb=" + hbgn) + "\">我的红包</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
}
