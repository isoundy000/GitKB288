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

public partial class Manage_guest : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xml = "/Controls/guestlist.xml";
    protected string gestName = (ub.GetSub("gestName", "/Controls/guestlist.xml"));
    protected string guestsee = (ub.GetSub("guestsee", "/Controls/guestlist.xml"));//小号使用者
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        string ac = Utils.ToSChinese(Utils.GetRequest("ac", "all", 1, "", ""));
        if (ac == "高级")
            act = "search";

        switch (act)
        {
            case "view":
                ViewPage();
                break;
            case "del":
                DelPage();
                break;
            case "clear":
                ClearPage();
                break;
            case "clearok":
                ClearOkPage();
                break;
            case "clearsys":
                ClearSysPage();
                break;
            case "search":
                SearchPage();
                break;
            case "manage":
                Manage();
                break;
            case "add":
                AddPage();
                break;
            case "hidmoney":
                hidmoney();
                break;
            case "reset":
                ReSet();
                break;
            case "resetimple":
                ReSetImple();
                break;
            case "groupNums":
                groupNums();
                break;
            case "msg": //d短信配置
                Msg();
                break;
            case "Robot"://机械人设置
                Robot();
                break;
            case "xiaohao":
                MidUser();
                break;
            default:
                ReloadPage();
                break;
        }
    }


    //总机械人设置
    private void Robot()
    {
        if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
        {
            Utils.Success("温馨提示", "使用彩版，页面更直观，更快捷！正在切换进入...", "guest.aspx?act=Robot&amp;ve=2a&amp;u=" + Utils.getstrU() + "", "1");
        }

        Master.Title = "总机械人设置";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>><a href=\"" + Utils.getUrl("guest.aspx?act=manage") + "\">号码管理</a>>总机械人设置");
        builder.Append(Out.Tab("</div>", ""));

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-1]$", "0"));
        string VE = ConfigHelper.GetConfigString("VE");
        string SID = ConfigHelper.GetConfigString("SID");

        ub xml = new ub();
        string xmlPath = "/Controls/bossRobot.xml";//总机械人XML
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (info == "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
            if (ptype == 0)
            {
                builder.Append("系统号设置|");
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=Robot&amp;ptype=1") + "\">游戏设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=Robot&amp;ptype=0") + "\">系统号设置</a>");
                builder.Append("|游戏设置");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)//机器人设置界面
            {
                #region
                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("<form id=\"form3\" method=\"POST\" action=\"guest.aspx\">");
                builder.Append("状态:<br/><select name=\"robotopenstate\" >");
                if (xml.dss["robotopenstate"].ToString() == "0")
                {
                    builder.Append("<option value=\"0\">关闭</option> ");
                    builder.Append("<option value=\"1\">开启</option> ");
                }
                else
                {
                    builder.Append("<option value=\"1\">开启</option> ");
                    builder.Append("<option value=\"0\">关闭</option> ");
                }
                builder.Append("</select>");
                builder.Append("<input type=\"hidden\" name=\"act\" Value=\"Robot\"/>");
                builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok\"/>");
                builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                builder.Append("<br/>换班开始时间(如8:00):<br/><input type=\"text\" name=\"startTime\" value=\"" + xml.dss["startTime"] + "\" /><br/>");
                builder.Append("换班结束时间(如20:00):<br/><input type=\"text\" name=\"endTime\" value=\"" + xml.dss["endTime"] + "\" /><br/>");
                builder.Append("多少天更换分组:<br/><input type=\"text\" name=\"dayToChange\" value=\"" + xml.dss["dayToChange"] + "\" /><br/>");
                //builder.Append("从某个日期开始轮换:(如:2016-10-01)<br/><input type=\"text\" name=\"data\" value=\"" + xml.dss["data"] + "\" /><br/>");
                builder.Append("总机械人ID(号码之间用#符号分割):<br/>");
                //判断是否为手机
                string agent = (Request.UserAgent + "").ToLower().Trim();
                if (agent == "" ||
                    agent.IndexOf("mobile") != -1 ||
                    agent.IndexOf("mobi") != -1 ||
                    agent.IndexOf("nokia") != -1 ||
                    agent.IndexOf("samsung") != -1 ||
                    agent.IndexOf("sonyericsson") != -1 ||
                    agent.IndexOf("mot") != -1 ||
                    agent.IndexOf("blackberry") != -1 ||
                    agent.IndexOf("lg") != -1 ||
                    agent.IndexOf("htc") != -1 ||
                    agent.IndexOf("j2me") != -1 ||
                    agent.IndexOf("ucweb") != -1 ||
                    agent.IndexOf("opera mini") != -1 ||
                    agent.IndexOf("mobi") != -1 ||
                    agent.IndexOf("android") != -1 ||
                    agent.IndexOf("iphone") != -1)
                {
                    builder.Append("<textarea  name=\"allRobotID\" >" + xml.dss["allRobotID"].ToString() + "</textarea>");
                }
                else
                {
                    builder.Append("<textarea cols=\"60\" rows=\"20\" name=\"allRobotID\" >" + xml.dss["allRobotID"].ToString() + "</textarea>");
                }

                try
                {
                    if (xml.dss["allRobotID"].ToString() == "")
                        builder.Append("一共0个机械人.");
                    else
                        builder.Append("一共" + xml.dss["allRobotID"].ToString().Split('#').Length + "个机械人.");
                }
                catch
                {
                    builder.Append("一共0个机械人.");
                }
                builder.Append("<br/><input class=\"btn-red\" type=\"submit\" value=\"提交确认\"/>");
                builder.Append("</form>");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("<b>下面数据只作显示,无法修改：</b>");
                builder.Append(Out.Tab("</div>", "<br/>"));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("白班一组:<br/><textarea name=\"b_1\" >" + xml.dss["b_1"].ToString() + "</textarea>");
                builder.Append("白班1组共：" + xml.dss["b_1"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>白班二组:<br/><textarea  name=\"b_2\" >" + xml.dss["b_2"].ToString() + "</textarea>");
                builder.Append("白班2组共：" + xml.dss["b_2"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>白班三组:<br/><textarea name=\"b_3\" >" + xml.dss["b_3"].ToString() + "</textarea>");
                builder.Append("白班3组共：" + xml.dss["b_3"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>夜班一组:<br/><textarea  name=\"y_1\" >" + xml.dss["y_1"].ToString() + "</textarea>");
                builder.Append("夜班1组共：" + xml.dss["y_1"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>夜班二组:<br/><textarea name=\"y_2\" >" + xml.dss["y_2"].ToString() + "</textarea>");
                builder.Append("夜班2组共：" + xml.dss["y_2"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>夜班三组:<br/><textarea name=\"y_3\" >" + xml.dss["y_3"].ToString() + "</textarea>");
                builder.Append("夜班3组共：" + xml.dss["y_3"].ToString().Split('#').Length + "个机械人.");
                builder.Append("<br/>当前使用白夜班组数：《" + xml.dss["zushu"].ToString() + "组》");
                builder.Append("<br/>从某个日期开始轮换：" + xml.dss["data"].ToString() + "<br/>");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
            else//游戏设置界面
            {
                #region
                #region 旧
                //string gameChoose = xml.dss["gameChoose"].ToString();
                //if (gameChoose.Contains("幸运二八"))
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"幸运二八\" checked=\"true\" /> 幸运二八");
                //}
                //else
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"幸运二八\" /> 幸运二八");
                //}
                //if (gameChoose.Contains("PK拾"))
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"PK拾\" checked=\"true\" /> PK拾");
                //}
                //else
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"PK拾\" /> PK拾");
                //}
                //if (gameChoose.Contains("新快3"))
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"新快3\" checked=\"true\" /> 新快3");
                //}
                //else
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"新快3\" /> 新快3");
                //}
                //if (gameChoose.Contains("球赛"))
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"球赛\" checked=\"true\" /> 球赛");
                //}
                //else
                //{
                //    builder.Append("<input type=\"checkbox\" name=\"gameChoose\" value=\"球赛\" /> 球赛");
                //}
                #endregion

                builder.Append(Out.Tab("<div>", "<br/>"));
                try
                {
                    if (xml.dss["allRobotID"].ToString() == "")
                        builder.Append("一共0个机械人.");
                    else
                        builder.Append("<h style=\"color:red\">以下打勾的游戏机器人总数不能超过<b>" + (xml.dss["allRobotID"].ToString().Split('#').Length / 6) + "个</b>！现有" + new BCW.JS.BLL.bossrobot().Get_num() + "个.</h><br/>");
                }
                catch
                {
                    builder.Append("一共0个机械人.");
                }
                builder.Append("<b>请选择游戏：</b><br/>");

                DataSet ds = new BCW.JS.BLL.bossrobot().GetList("*", "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    builder.Append("<form id=\"form3\" method=\"POST\" action=\"guest.aspx\">");
                    builder.Append("<input type=\"hidden\" name=\"act\" Value=\"Robot\"/>");
                    builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok2\"/>");
                    builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
                    builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string name = ds.Tables[0].Rows[i]["GameName"].ToString();
                        int type = int.Parse(ds.Tables[0].Rows[i]["type"].ToString());
                        int num = int.Parse(ds.Tables[0].Rows[i]["robotnum"].ToString());
                        string XML = ds.Tables[0].Rows[i]["XML"].ToString();
                        string ziduan = ds.Tables[0].Rows[i]["ziduan"].ToString();
                        string tf = "";
                        if (type == 1)
                            tf = "checked=\"true\"";

                        builder.Append("<input type=\"checkbox\" name=\"name" + i + "\" value=\"" + name + "\" " + tf + " />" + name + "");
                        builder.Append("<input type=\"text\" name=\"num" + i + "\" value=\"" + num + "\" />个,");
                        builder.Append("字段:<input type=\"text\" name=\"ziduan" + i + "\" value=\"" + ziduan + "\" />,");
                        builder.Append("XML:<input type=\"text\" name=\"XML" + i + "\" value=\"" + XML + "\" /><br/>");
                    }
                    builder.Append("<input class=\"btn-red\" type=\"submit\" value=\"提交确认\"/>");
                    builder.Append("</form>");
                }
                else
                {
                    builder.Append("<b>数据为空...</b>");
                }
                builder.Append("<hr/>");
                builder.Append(Out.Tab("</div>", ""));
                string strText = "添加游戏：(名字)/";
                string strName = "gamename";
                string strType = "text";
                string strValu = "'";
                string strEmpt = "false";
                string strIdea = "/";
                string strOthe = "马上添加,guest.aspx?act=Robot&amp;info=add,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("<b>温馨提示：</b><br/>1、<h style=\"color:red\">先设置总的机器人，然后再设置每个游戏需要的机器人ID个数.</h><br/>2、<h style=\"color:red\">字段与XML由技术员设置.</h>");
                builder.Append(Out.Tab("</div>", ""));
                #endregion
            }
        }
        else if (info == "ok")//机器人设置
        {
            #region
            string robotopenstate = Utils.GetRequest("robotopenstate", "post", 2, @"^[0-1]\d*$", "开关选择错误");
            string startTime = (Utils.GetRequest("startTime", "post", 2, @"^[^\^]+$", "设置开始时间填写出错"));
            string endTime = (Utils.GetRequest("endTime", "post", 2, @"^[^\^]+$", "设置结束时间填写出错"));
            string dayToChange = (Utils.GetRequest("dayToChange", "post", 2, @"^\d+$", "设置多少天更换分组填写出错"));
            string allRobotID = (Utils.GetRequest("allRobotID", "all", 1, "", "")).Replace("\r\n", "").Replace(" ", "");
            //DateTime data = Utils.ParseTime(Utils.GetRequest("data", "all", 2, @"\d{4}-\d{2}-\d{2}", "日期轮换填写无效"));

            xml.dss["robotopenstate"] = robotopenstate;
            xml.dss["startTime"] = startTime;
            xml.dss["endTime"] = endTime;
            xml.dss["dayToChange"] = dayToChange;
            //xml.dss["data"] = data.ToString("yyyy-MM-dd");
            if (allRobotID != xml.dss["allRobotID"].ToString())//比较前后2个是否相等
            {
                //判断是否不是系统号+判断是否存在该ID
                string[] ss = allRobotID.Split('#');
                if (allRobotID.Length > 0)
                {
                    for (int pp = 0; pp < allRobotID.Split('#').Length; pp++)
                    {
                        if (!new BCW.BLL.User().ExistsID(Convert.ToInt64(ss[pp])))
                        {
                            Utils.Error("该ID不存在：" + ss[pp] + "，请更改.", "");
                        }
                        else
                        {
                            if (new BCW.BLL.User().GetIsSpier(int.Parse(ss[pp])) == 0)
                            {
                                Utils.Error("存在不是系统号：" + ss[pp] + "，请删除.", "");
                            }
                        }
                    }
                }


                xml.dss["allRobotID"] = allRobotID;

                int nuu = allRobotID.Split('#').Length / 6;//得到每组个数
                if (nuu < new BCW.JS.BLL.bossrobot().Get_num())//比较数量
                    Utils.Error("抱歉,机器人个数比游戏设定的个数要少.请添加机器人或者减少游戏的机器人个数.", "");

                string a1 = ""; string a2 = ""; string a3 = ""; string a4 = ""; string a5 = ""; string a6 = "";
                for (int aa1 = 0; aa1 < allRobotID.Length; aa1++)
                {
                    if (aa1 < nuu)
                        a1 += ss[aa1] + "#";
                    if ((nuu < aa1 + 1) && (aa1 < nuu * 2))
                        a2 += ss[aa1] + "#";
                    if ((nuu * 2 < aa1 + 1) && (aa1 < nuu * 3))
                        a3 += ss[aa1] + "#";
                    if ((nuu * 3 <= aa1) && (aa1 < nuu * 4))
                        a4 += ss[aa1] + "#";
                    if ((nuu * 4 <= aa1) && (aa1 < nuu * 5))
                        a5 += ss[aa1] + "#";
                    if ((nuu * 5 <= aa1) && (aa1 < nuu * 6))
                        a6 += ss[aa1] + "#";
                }
                xml.dss["b_1"] = a1.Substring(0, a1.Length - 1);
                xml.dss["b_2"] = a2.Substring(0, a2.Length - 1);
                xml.dss["b_3"] = a3.Substring(0, a3.Length - 1);
                xml.dss["y_1"] = a4.Substring(0, a4.Length - 1);
                xml.dss["y_2"] = a5.Substring(0, a5.Length - 1);
                xml.dss["y_3"] = a6.Substring(0, a6.Length - 1);
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);

            //刷新机分配ID到各xml
            Utils.Success("设置成功", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=Robot&amp;ptype=0"), "2");
            #endregion
        }
        else if (info == "add")//添加游戏
        {
            #region
            string gamename = (Utils.GetRequest("gamename", "all", 2, @"^[^\^]{1,20}$", "游戏名字填写出错."));
            BCW.JS.Model.bossrobot model = new BCW.JS.Model.bossrobot();
            model.GameName = gamename;
            model.type = 0;
            model.XML = "";
            model.ziduan = "";
            model.robotnum = 0;
            model.GameID = 0;
            new BCW.JS.BLL.bossrobot().Add(model);
            Utils.Success("添加成功", "添加成功，正在返回..", Utils.getUrl("guest.aspx?act=Robot&amp;ptype=1"), "2");
            #endregion
        }
        else//游戏设置
        {
            #region
            DataSet ds = new BCW.JS.BLL.bossrobot().GetList("*", "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int bb = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string type = (Utils.GetRequest("name" + i + "", "all", 1, "", ""));
                    int robotnum = int.Parse(Utils.GetRequest("num" + i + "", "all", 1, @"^[0-9]\d*$", "0"));
                    if (type != "")
                        bb += robotnum;
                }
                if (bb > (xml.dss["allRobotID"].ToString().Split('#').Length / 6))
                    Utils.Error("抱歉,设置的数量大于每组数量.", "");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                    string XML = (Utils.GetRequest("XML" + i + "", "all", 1, "", ""));
                    string ziduan = (Utils.GetRequest("ziduan" + i + "", "all", 1, "", ""));
                    int robotnum = int.Parse(Utils.GetRequest("num" + i + "", "all", 1, @"^[0-9]\d*$", "0"));
                    string type = (Utils.GetRequest("name" + i + "", "all", 1, "", ""));
                    int a = 0;
                    if (type != "")
                        a = 1;
                    new BCW.JS.BLL.bossrobot().update_zd("robotnum=" + robotnum + ",type=" + a + ",XML='" + XML + "',ziduan='" + ziduan + "'", "ID=" + ID + "");
                }
            }
            Utils.Success("设置成功", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=Robot&amp;ptype=1"), "2");
            #endregion
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }

    //隐藏金融选项
    private void hidmoney()
    {
        Master.Title = "系统号金融状态";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>><a href=\"" + Utils.getUrl("guest.aspx?act=manage") + "\">号码管理</a>>金融状态设置");
        builder.Append(Out.Tab("</div>", ""));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int ishidmoney = int.Parse(Utils.GetRequest("ishidmoney", "post", 1, @"^[0-1]$", "1"));
            DataSet data = new BCW.BLL.User().GetList(" ID ", "IsSpier=1");
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                int recordCount = data.Tables[0].Rows.Count; //得到总数
                for (int j = 0; j < recordCount; j++)
                {
                    int meid = int.Parse(data.Tables[0].Rows[j]["ID"].ToString());//得到每个号码的ID
                    #region  隐藏财产
                    string ForumSet = new BCW.BLL.User().GetForumSet(meid);//得到个性设置
                    string[] fs = ForumSet.Split(",".ToCharArray());//32
                    string sforumsets = string.Empty;
                    for (int i = 0; i < fs.Length; i++)
                    {
                        string[] sfs = fs[i].ToString().Split("|".ToCharArray());
                        if (i == 24)
                        {
                            if (string.IsNullOrEmpty(sforumsets))
                            {
                                sforumsets += sfs[0] + "|" + ishidmoney;
                            }
                            else
                            {
                                sforumsets += "," + sfs[0] + "|" + ishidmoney;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(sforumsets))
                            {
                                sforumsets += sfs[0] + "|" + sfs[1];
                            }
                            else
                            {
                                sforumsets += "," + sfs[0] + "|" + sfs[1];
                            }
                        }
                    }
                    new BCW.BLL.User().UpdateForumSet(meid, sforumsets);
                    #endregion
                }
                Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=hidmoney"), "1");
            }
        }
        else
        {
            string strText = "是否隐藏系统号的金融:/,,,";
            string strName = "ishidmoney,act,info,backurl";
            string strType = "select,hidden,hidden,hidden";
            string strValu = "'hidmoney'ok'" + Utils.getPage(0) + "";
            string strEmpt = "1|隐藏|0|不隐藏,false,false,false";
            string strIdea = "/";
            string strOthe = "确定,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }

    //短息管理
    private void Msg()
    {
        Master.Title = "短信设置";
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guestlist.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (info == "")
        {
            string strText = "短信有效时长:/,每天发送的总数量:/,半小时内每IP发送的最大数量:/,半小时内每手机号码发送的最大数量:/,剩余短信数低于多少条通知:/,通知号码:/,,";
            string strName = "msgTime,dayCount,IPCount,phoneCount,msgremain,callID,info,act";
            string strType = "num,num,num,num,num,num,hidden,hidden";
            string strValu = "" + xml.dss["msgTime"] + "'" + xml.dss["dayCount"] + "'" + xml.dss["IPCount"] + "'" + xml.dss["phoneCount"] + "'" + xml.dss["msgremain"] + "'" + xml.dss["callID"] + "'ok'msg";
            string strEmpt = "false,false,false,false,false,false,false,false,";
            string strIdea = "/";
            string strOthe = "确定修改,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            string msgTime = (Utils.GetRequest("msgTime", "post", 2, @"^\d+$", "设置短信有效时长填写出错"));
            string dayCount = (Utils.GetRequest("dayCount", "post", 2, @"^\d+$", "设置每天发送的总数量填写出错"));
            string IPCount = (Utils.GetRequest("IPCount", "post", 2, @"^\d+$", "设置每IP发送的最大数量填写出错"));
            string phoneCount = (Utils.GetRequest("phoneCount", "post", 2, @"^\d+$", "设置每手机号码发送的最大数量填写出错"));
            string msgremain = (Utils.GetRequest("msgremain", "post", 2, @"^\d+$", "设置剩余短信填写出错"));
            string callID = (Utils.GetRequest("callID", "post", 2, @"^\d+$", "设置通知号码填写出错"));
            // Utils.Error("msgTime:"+ msgTime+ ",dayCount:"+ dayCount+ ",IPCount:"+ IPCount+ ",phoneCount:"+ phoneCount, "");
            xml.dss["msgTime"] = msgTime;
            xml.dss["dayCount"] = dayCount;
            xml.dss["IPCount"] = IPCount;
            xml.dss["phoneCount"] = phoneCount;
            xml.dss["msgremain"] = msgremain;
            xml.dss["callID"] = callID;
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("短信设置", "设置成功，正在返回..", Utils.getUrl("spaceapp/default.aspx"), "2");
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }

    //小号分组（没用到）
    private void groupNums()
    {
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[0-2]$", "0"));
        int check = int.Parse(Utils.GetRequest("check", "all", 1, @"^[0-1]$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        Master.Title = "管理分组";
        builder.Append(Out.Tab("<div class=\"\">", Out.Hr()));
        int count = int.Parse(ub.GetSub("GroupCount", xml));//得到分组数
        ub xml1 = new ub();
        string xmlPath = "/Controls/guestlist.xml";
        Application.Remove(xmlPath);//清缓存
        xml1.ReloadSub(xmlPath); //加载配置
        if (check == 1)
        {
            int groupid = int.Parse(Utils.GetRequest("groupid", "all", 2, @"^[1-9]\d*$", "分组ID错误"));
            xml1.dss["GroupCount"] = count - 1;
            if (groupid == count)
            {
                xml1.dss["MidUidLists" + groupid] = "";
            }
            else
            {
                for (int i = groupid; i < count; i++)
                {
                    xml1.dss["MidUidLists" + i] = ub.GetSub("MidUidLists" + (i + 1), xml);
                }
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml1.Post(xml1.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=manage&amp;backurl=" + Utils.getPage(1) + ""), "1");
        }
        if (type == 0)  //管理
        {
            #region  管理分组
            builder.Append("管理分组|<a href=\"" + Utils.getUrl("guest.aspx?act=groupNums&amp;type=1&amp;backurl=" + Utils.getPage(0) + "") + "\">添加分组</a>");
            builder.Append("<br/>现有拥有" + count + "个小组：<br/>");
            if (info == "")
            {
                string group = "";
                for (int i = 1; i <= count; i++)
                {
                    if (i == 1)
                    {
                        group = ub.GetSub("MidUidLists", xml);
                    }
                    else
                    {
                        group = ub.GetSub("MidUidLists" + i, xml);
                    }
                    string strText = "分组" + i + ":/,,,";
                    string strName = "group,groupid,info,act";
                    string strType = "textarea,hidden,hidden,hidden";
                    string strValu = group + "'" + i + "'ok'groupNums";
                    string strEmpt = "false,false,false,false";
                    string strIdea = "/";
                    string strOthe = "确定修改|删除分组,guest.aspx,post,1,red|blue";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            else
            {
                if (Utils.ToSChinese(ac) == "确定修改")
                {
                    int groupid = int.Parse(Utils.GetRequest("groupid", "post", 2, @"^[1-9]\d*$", "分组ID错误"));
                    string MidUidLists = Utils.GetRequest("group", "post", 2, @"^[^\^]{1,2000}$", "小号ID填写错误");
                    if (groupid == 1)
                    {
                        xml1.dss["MidUidLists"] = MidUidLists;//小
                    }
                    else
                    {
                        xml1.dss["MidUidLists" + groupid] = MidUidLists;//小
                    }
                    System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml1.Post(xml1.dss), System.Text.Encoding.UTF8);
                    Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=manage&amp;backurl=" + Utils.getPage(1) + ""), "1");
                }
                else
                {
                    int groupid = int.Parse(Utils.GetRequest("groupid", "post", 2, @"^[1-9]\d*$", "分组ID错误"));
                    builder.Append("<b><a href=\"" + Utils.getUrl("guest.aspx?act=groupNums&amp;groupid=" + groupid + "&amp;check=1&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除?</a></b><br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=groupNums&amp;backurl=" + Utils.getPage(0) + "") + "\">先留着吧!</a>");
                }
            }
            #endregion
        }
        else if (type == 1)//添加
        {
            #region  添加分组
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=groupNums&amp;type=0&amp;backurl=" + Utils.getPage(0) + "") + "\">管理分组</a>|添加分组");
            builder.Append("<br/>你现在添加的是第" + (count + 1) + "个分组");
            if (info == "")
            {
                string strText = "分组" + (count + 1) + ":/,,,,";
                string strName = "add,groupid,type,info,act";
                string strType = "textarea,hidden,hidden,hidden,hidden";
                string strValu = "'" + (count + 1) + "'1'ok'groupNums";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定修改,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                int groupid = int.Parse(Utils.GetRequest("groupid", "post", 2, @"^[1-9]\d*$", "分组ID错误"));
                string MidUidLists = Utils.GetRequest("add", "post", 2, @"^[^\^]{1,2000}$", "小号ID填写错误");
                xml1.dss["MidUidLists" + groupid] = MidUidLists;
                xml1.dss["GroupCount"] = groupid;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml1.Post(xml1.dss), System.Text.Encoding.UTF8);
                Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=manage&amp;backurl=" + Utils.getPage(1) + ""), "1");
            }
            #endregion
        }
        builder.Append(Out.Tab("</div>", "<br /> "));

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));

    }
    private void Manage()
    {
        Master.Title = "号码管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>>号码管理");
        builder.Append(Out.Tab("</div>", "<br /> "));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("1、<a href=\"" + Utils.getUrl("guest.aspx?act=xiaohao") + "\">" + "小号管理" + " </a><br />");//" + gestName + "
        builder.Append("2、<a href=\"" + Utils.getUrl("guest.aspx?act=reset") + "\">" + "重置" + gestName + "使用者管理密码" + " </a><br />");
        builder.Append("3、<a href=\"" + Utils.getUrl("guest.aspx?act=resetimple") + "\">" + "设置" + gestName + "返回主账号ID信息" + " </a><br />");
        //builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=groupNums") + "\">" + "号码分组设置" + " </a><br />");
        builder.Append("4、<a href=\"" + Utils.getUrl("guest.aspx?act=hidmoney") + "\">" + "系统号金融状态管理" + " </a><br />");
        builder.Append("5、<a href=\"" + Utils.getUrl("guest.aspx?act=Robot") + "\">" + "总机器人管理" + " </a><br />");
        builder.Append(Out.Tab("</div>", " "));
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
    private void ReSetImple()
    {
        Master.Title = "返回主账号ID信息";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>><a href=\"" + Utils.getUrl("guest.aspx?act=manage") + "\">号码管理</a>>返回主账号ID信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        string guestnew = ub.GetSub("guestnew", "/Controls/guestlist.xml");
        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        if (new BCW.BLL.numsManage().ExistsByUsID(int.Parse(guestnew))) //存在数据
        {
            if (info == "")
            {
                BCW.Model.numsManage model = new BCW.BLL.numsManage().GetByUsID(int.Parse(guestnew));
                builder.Append(Out.Tab("<div class=\"\">", ""));
                builder.Append("请注意，你重置的是" + gestName + "管理接口ID:<b>" + guestnew + "</b><br/>");
                builder.Append("原问题:<b>" + model.Question + "</b><br/>");
                builder.Append("原答案:<b>" + model.answer + "</b><br/>");
                builder.Append(Out.Tab("</div>", " "));
                string strText = "请输入你的" + gestName + "管理问题:/,请输入你的" + gestName + "管理答案(用于找回" + gestName + "管理密码):/,请输入" + gestName + "管理密码:/,请再次确认管理密码:/,,,";
                string strName = "question,answer,pwd,pwd1,act,info,backurl";
                string strType = "text,text,text,text,hidden,hidden,hidden";
                string strValu = "''''resetimple'ok'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string pwd = Utils.GetRequest("pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                string pwd1 = Utils.GetRequest("pwd1", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                // string pwd1 = Utils.GetRequest("newpwd", "post", 2, @"^[^\^]{6,20}$", "请输入六到二十个字符的密码");
                string question = Utils.GetRequest("question", "post", 2, @"^[\s\S]{3,50}$", "请输入问题（3-50字以内）");
                string answer = Utils.GetRequest("answer", "post", 2, @"^[\s\S]{3,50}$", "请输入问题答案（3-50字以内）");
                if (pwd != pwd1)
                {
                    Utils.Error("输入两次的密码不相等,请再次输入.", "");
                }
                else
                {

                    if (info1 == "")
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        builder.Append("请再次确认你的信息<br/>");
                        builder.Append("账号:" + guestnew + "<br/>");
                        builder.Append("" + gestName + "管理问题:" + question + "<br/>");
                        builder.Append("" + gestName + "管理答案:" + answer + "<br/>");
                        builder.Append("" + gestName + "管理密码:" + pwd + "");
                        builder.Append(Out.Tab("</div>", ""));
                        string strText = "你的" + gestName + "管理问题:/,你的" + gestName + "管理答案:/," + gestName + "管理密码:/,,,,,";
                        string strName = "question,answer,pwd,pwd1,act,info,info1,backurl";
                        string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                        string strValu = question + "'" + answer + "'" + pwd + "'" + pwd1 + "'resetimple'ok'ok'" + Utils.getPage(0) + "";
                        string strEmpt = "false,false,false,false,false,false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定,guest.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        BCW.Model.numsManage numsManage = new BCW.Model.numsManage();
                        numsManage.UsID = int.Parse(guestnew);
                        numsManage.Pwd = Utils.MD5Str(pwd);
                        numsManage.loginTime = DateTime.Now;
                        numsManage.Question = question;
                        numsManage.answer = answer;
                        new BCW.BLL.numsManage().UpdateByUI(numsManage);
                        Utils.Success("设置成功", "设置成功!，请牢记!,<br/>你的管理问题是:" + question + "<br/>你的答案是:" + answer + "<br/>你的密码是" + pwd + "....3秒后自动跳转", Utils.getUrl("spaceapp/default.aspx"), "3");
                    }
                }
            }
        }
        else  //后台设置
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("请注意，现在首次设置的是" + gestName + "返回主账号ID:<b>" + guestnew + "</b>");
            builder.Append(Out.Tab("</div>", ""));
            if (info == "")
            {
                string strText = "请输入你的" + gestName + "管理问题:/,请输入你的" + gestName + "管理答案(用于找回" + gestName + "管理密码):/,请输入" + gestName + "管理密码:/,请再次确认管理密码:/,,,";
                string strName = "question,answer,pwd,pwd1,act,info,backurl";
                string strType = "text,text,text,text,hidden,hidden,hidden";
                string strValu = "''''resetimple'ok'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定,guest.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {
                string pwd = Utils.GetRequest("pwd", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                string pwd1 = Utils.GetRequest("pwd1", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                // string pwd1 = Utils.GetRequest("newpwd", "post", 2, @"^[^\^]{6,20}$", "请输入六到二十个字符的密码");
                string question = Utils.GetRequest("question", "post", 2, @"^[\s\S]{3,50}$", "请输入问题（3-50字以内）");
                string answer = Utils.GetRequest("answer", "post", 2, @"^[\s\S]{3,50}$", "请输入问题答案（3-50字以内）");
                if (pwd != pwd1)
                {
                    Utils.Error("输入两次的密码不相等,请再次输入.", "");
                }
                else
                {
                    if (info1 == "")
                    {

                        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                        builder.Append("请再次确认你的信息<br/>");
                        builder.Append("你的" + gestName + "管理问题:" + question + "<br/>");
                        builder.Append("你的" + gestName + "管理答案:" + answer + "<br/>");
                        builder.Append("" + gestName + "管理密码:" + pwd + "");
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = "你的" + gestName + "管理问题:/,你的" + gestName + "管理答案:/," + gestName + "管理密码:/,,,,,";
                        string strName = "question,answer,pwd,pwd1,act,info,info1,backurl";
                        string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                        string strValu = question + "'" + answer + "'" + pwd + "'" + pwd1 + "'resetimple'ok'ok'" + Utils.getPage(0) + "";
                        string strEmpt = "false,false,false,false,false,false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定,guest.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        BCW.Model.numsManage numsManage = new BCW.Model.numsManage();
                        numsManage.UsID = int.Parse(guestnew);
                        numsManage.Pwd = Utils.MD5Str(pwd);
                        numsManage.loginTime = DateTime.Now;
                        numsManage.Question = question;
                        numsManage.answer = answer;
                        new BCW.BLL.numsManage().Add(numsManage);
                        Utils.Success("设置密码成功", "设置密码成功!，请牢记!,<br/>你的管理问题是:" + question + "<br/>你的答案是:" + answer + "<br/>你的密码是" + pwd + "....3秒后自动跳转到小号管理", Utils.getUrl("guest.aspx"), "3");
                    }
                }

            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
    private void ReSet()
    {
        Master.Title = "重置使用者密码";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>><a href=\"" + Utils.getUrl("guest.aspx?act=manage") + "\">号码管理</a>>重置使用者密码");
        builder.Append(Out.Tab("</div>", ""));


        string info = Utils.GetRequest("info", "all", 1, "", "");
        string info1 = Utils.GetRequest("info1", "all", 1, "", "");
        if (info == "")
        {
            string strText = "请输入号码管理账号ID:/,,,";
            string strName = "ID,act,info,backurl";
            string strType = "text,hidden,hidden,hidden";
            string strValu = "'reset'ok'" + Utils.getPage(0) + "";
            string strEmpt = "false,,false,false,false";
            string strIdea = "/";
            string strOthe = "确定,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            int ID = int.Parse(Utils.GetRequest("ID", "all", 2, @"^[\d]+$", "请输入ID账号"));
            if (new BCW.BLL.User().Exists(ID))
            {
                if (guestsee.Contains(ID.ToString()))//大号  如果是管理号
                {
                    if (new BCW.BLL.numsManage().ExistsByUsID(ID)) //存在数据
                    {
                        if (info1 == "")
                        {
                            builder.Append(Out.Tab("<div class=\"\">", ""));
                            string phone = new BCW.BLL.User().GetMobile(ID);
                            phone = phone.Substring(5, 6);
                            builder.Append("你需要重置号码管理密码的账号是：" + ID + "，(重置为改账号的手机号码的后六位(" + phone + "))<br/>");
                            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok&amp;info1=ok&amp;act=reset&amp;ID=" + ID + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定重置</a><br />");
                            builder.Append("<a href=\"" + Utils.getPage("spaceapp/default.aspx") + "\">不了..</a>");
                            builder.Append(Out.Tab("</div>", "<br />"));
                        }
                        else
                        {
                            string phone = new BCW.BLL.User().GetMobile(ID);
                            phone = phone.Substring(5, 6);
                            //Utils.Error("phone."+ Utils.MD5Str(phone), "");
                            new BCW.BLL.numsManage().UpdatePwd(ID, Utils.MD5Str(phone));
                            Utils.Success("设置成功", "设置成功!....", Utils.getUrl("spaceapp/default.aspx"), "2");
                        }
                    }
                    else
                    {
                        Utils.Error("该账号暂未设置相关信息.", "");
                    }
                }
                else
                {
                    Utils.Error("该账号暂无权限管理号码.", "");
                }
            }
            else
            {
                Utils.Error("不存在账号.", "");
            }
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心-</a>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">应用列表</a>");
        builder.Append(Out.Tab("</div>", "<br /> "));
    }
    private void MidUser()
    {
        string GameName = Convert.ToString(ub.GetSub("gestName", "/Controls/guestlist.xml"));

        Master.Title = "小号内线管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">管理中心</a>>");
        builder.Append("<a href=\"" + Utils.getUrl("spaceapp/default.aspx") + "\">社区应用管理</a>><a href=\"" + Utils.getUrl("guest.aspx?act=manage") + "\">号码管理</a>>小号内线管理");
        builder.Append(Out.Tab("</div>", ""));

        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;ptype=0") + "\">配置管理</a><br/>");
        //builder.Append("小号内线");
        //builder.Append(Out.Tab("</div>", "<br />"));
        Master.Title = GameName + "设置";
        string info = Utils.GetRequest("info", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guestlist.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (info == "ok")
        {
            string gestName = Utils.GetRequest("gestName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string guestopen = Utils.GetRequest("guestopen", "post", 2, @"^[0-9]\d*$", "开关选择错误");
            string guesttime = Utils.GetRequest("guesttime", "post", 2, @"^[\d]+$", "输入时间出错");
            string guestgroup = Utils.GetRequest("guestgroup", "post", 2, @"^[\d]+$", "分组个数出错");
            string guestsee = Utils.GetRequest("guestsee", "post", 2, @"^[^\^]{1,20000}$", "使用者填写错误");
            string guestnew = Utils.GetRequest("guestnew", "post", 2, @"^[^\^]{1,20000}$", "接口ID填写错误");
            string MidUidLists = Utils.GetRequest("MidUidLists", "post", 2, @"^[^\^]+$", "小号ID填写错误");
            // Utils.Error(""+ guestopen, "");
            if (!guestsee.Contains(guestnew))//大号  如果是管理号
            {
                Utils.Error("接口ID必须包含在使用者当中.", "");
            }
            xml.dss["gestName"] = gestName;//名称
            xml.dss["guesttime"] = guesttime;//时间
            xml.dss["guestgroup"] = guestgroup;//分组个数
            xml.dss["guestopen"] = guestopen;//开关
            xml.dss["guestsee"] = guestsee.Replace("\r\n", "").Replace(" ", "");//可见
            xml.dss["guestnew"] = guestnew;//接口ID
            xml.dss["MidUidLists"] = MidUidLists.Replace("\r\n", "").Replace(" ", "");//小号
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("游戏设置", "设置成功，正在返回..", Utils.getUrl("guest.aspx?act=xiaohao&amp;backurl=" + Utils.getPage(1) + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            string VE = ConfigHelper.GetConfigString("VE");
            string SID = ConfigHelper.GetConfigString("SID");
            builder.Append("<form id=\"form2\" method=\"post\" action=\"guest.aspx\">");
            builder.AppendFormat("<input type=\"hidden\" name=\"" + VE + "\" value=\"{0}\"/>", Utils.getstrVe());
            builder.AppendFormat("<input type=\"hidden\" name=\"" + SID + "\" value=\"{0}\"/>", Utils.getstrU());
            builder.Append("<input type=\"hidden\" name=\"act\" Value=\"xiaohao\"/>");
            builder.Append("<input type=\"hidden\" name=\"info\" Value=\"ok\"/>");
            builder.Append("*显示名称:<br/><input type=\"text\" name=\"gestName\" value=\"" + xml.dss["gestName"] + "\" />&nbsp;");
            builder.Append("<br/>*状态:<br/><select name=\"guestopen\" >&nbsp;");
            if (xml.dss["guestopen"].ToString() == "0")
            {
                builder.Append("<option value=\"0\">关闭</option> ");
                builder.Append("<option value=\"1\">开启</option> ");
            }
            else
            {
                builder.Append("<option value=\"1\">开启</option> ");
                builder.Append("<option value=\"0\">关闭</option> ");
            }
            builder.Append("</select>");
            //  builder.Append("<hr/>");
            builder.Append("<br/>*使用者:<br/><textarea cols=\"60\" rows=\"4\" name=\"guestsee\" >" + xml.dss["guestsee"] + "</textarea>");
            builder.Append("<br/>*接口ID:<br/><input type=\"text\" name=\"guestnew\" value=\"" + xml.dss["guestnew"] + "\" />&nbsp;");
            builder.Append("<br/>*多久再次输入管理密码:<br/><input type=\"text\" name=\"guesttime\" value=\"" + xml.dss["guesttime"] + "\" />&nbsp;");
            builder.Append("<br/>*多少个分一组:<br/><input type=\"text\" name=\"guestgroup\" value=\"" + xml.dss["guestgroup"] + "\" />&nbsp;");
            builder.Append("<br/>*小号ID:<br/><textarea cols=\"60\" rows=\"20\" name=\"MidUidLists\" >" + xml.dss["MidUidLists"] + "</textarea>");
            builder.Append("<br/><input class=\"btn-red\" type=\"submit\" value=\"提交确认\"/>");
            builder.Append("</form>");
            //string strText = "显示名称:/,状态:/,使用者:/,接口ID:/,多久再次输入管理密码(分钟):/,多少个分一组:/,小号ID:/";
            //string strName = "gestName,guestopen,guestsee,guestnew,guesttime,guestgroup,MidUidLists";
            //string strType = "text,select,textarea,text,text,text,textarea";
            //string strValu = xml.dss["gestName"] + "'" + xml.dss["guestopen"] + "'" + xml.dss["guestsee"] + "'" + xml.dss["guestnew"] + "'" + xml.dss["guesttime"] + "'" + xml.dss["guestgroup"] + "'" + xml.dss["MidUidLists"] + "'" + Utils.getPage(0) + "";
            //string strEmpt = "true,0|关闭|1|开启,true,true,true,true,true";
            //string strIdea = "/";
            //string strOthe = "确定修改,guest.aspx?act=xiaohao,post,1,red";
            //builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            // builder.Append("温馨提示:" + "输入的ID#号间隔" + "<br />");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;ptype=0") + "\">返回上级</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    private void ReloadPage()
    {
        Master.Title = "内线管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("内线管理");
        builder.Append(Out.Tab("</div>", "<br />"));
        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
        int toid = int.Parse(Utils.GetRequest("toid", "all", 1, @"^[1-9]\d*$", "0"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-5]$", "0"));

        if (toid > 0 && uid == 0)
        {
            Utils.Error("填写接收ID时，请同时填写发送ID", "");
        }
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        bool IsSeen = true;
        if (Utils.GetDomain().Contains("ky288") || Utils.GetDomain().Contains("boyi929") || Utils.GetDomain().Contains("dyj6") || Utils.GetDomain().Contains("kubao") || Utils.GetDomain().Contains("tuhao") || Utils.GetDomain().Contains("th") || Utils.GetDomain().Contains("kb288") || Utils.GetDomain().Contains("192"))
        {
            int ManageId = new BCW.User.Manage().IsManageLogin();
            if (ManageId != 1 && ManageId != 9 && ManageId != 26)
            {
                IsSeen = false;
            }
        }
        if (IsSeen)
        {
            if (toid == 0)
            {
                if (ptype == 0)
                    builder.Append("全部|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;ptype=0") + "\">全部</a>|");


                if (uid > 0)
                {
                    if (ptype == 1)
                        builder.Append("收信|");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;uid=" + uid + "&amp;ptype=1") + "\">收信</a>|");

                    if (ptype == 2)
                        builder.Append("发信|");
                    else
                        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;uid=" + uid + "&amp;ptype=2") + "\">发信</a>|");
                }

                if (ptype == 3)
                    builder.Append("会员内线|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;uid=" + uid + "&amp;ptype=3") + "\">会员内线</a>|");

                if (ptype == 4)
                    builder.Append("系统内线|");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;uid=" + uid + "&amp;ptype=4") + "\">系统内线</a>|");

                if (ptype == 5)
                    builder.Append("收藏内线");
                else
                    builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;uid=" + uid + "&amp;ptype=5") + "\">收藏内线</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=list&amp;ptype=0") + "\">&lt;&lt;查看全部</a>");
            }
        }
        else
        {
            builder.Append("系统内线：");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
        string strWhere = "";
        string[] pageValUrl = { "uid", "toid", "ptype" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (IsSeen)
        {

            //查询条件
            if (toid == 0)
            {
                if (uid > 0)
                {
                    if (ptype == 1)
                        strWhere = "ToId=" + uid + " and FromId>0";
                    else if (ptype == 2)
                        strWhere = "FromId=" + uid + "";
                    else if (ptype == 3)
                        strWhere = "ToId=" + uid + " and FromId<>0";
                    else if (ptype == 4)
                        strWhere = "ToId=" + uid + " and FromId=0";
                    else if (ptype == 5)
                        strWhere = "ToId=" + uid + " and TDel=0 and IsKeep=1";
                }
                else
                {
                    if (ptype == 3)
                        strWhere = "FromId<>0";
                    if (ptype == 4)
                        strWhere = "FromId=0";
                    else if (ptype == 5)
                        strWhere = "TDel=0 and IsKeep=1";
                }
            }
            else
            {
                strWhere = "FromId=" + uid + " and ToId=" + toid + "";

            }
        }
        else
        {
            strWhere = "FromId=0";
            if (uid > 0)
                strWhere += " and ToId=" + uid + "";
        }

        // 开始读取列表
        IList<BCW.Model.Guest> listGuest = new BCW.BLL.Guest().GetGuests(pageIndex, pageSize, strWhere, out recordCount);
        if (listGuest.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Guest n in listGuest)
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
                string sText = n.Content;

                //if (n.Content.Length > 30)
                //{
                sText = "<a href=\"" + Utils.getUrl("guest.aspx?act=view&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + Out.UBB(Utils.Left(sText, 40)) + "</a>";
                //}
                builder.Append("[<a href=\"" + Utils.getUrl("guest.aspx?act=del&amp;id=" + n.ID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">删</a>]");
                builder.AppendFormat("{0}.", (pageIndex - 1) * pageSize + k);
                builder.AppendFormat("{0}", sText);
                builder.AppendFormat("{0}", DT.FormatDate(n.AddTime, 1));
                if (n.IsSeen == 1)
                    builder.Append("<br /><b>[已读]</b>");
                else
                    builder.Append("<br /><b>[未读]</b>");

                if (n.FromId == 0)
                    builder.Append("『系统』");
                else
                    builder.AppendFormat("『<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>』", n.FromId, n.FromName);

                builder.AppendFormat("发给『<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>』", n.ToId, n.ToName);

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
        string strText = "输入发送ID:/,";
        string strName = "uid,ptype";
        string strType = "num,hidden";
        string strValu = "'1";
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜内线|高级,guest.aspx,post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        string gestName = Convert.ToString(ub.GetSub("gestName", "/Controls/guestlist.xml"));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=xiaohao") + "\">" + gestName + "</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clear") + "\">清空记录</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=add") + "\">发送内线</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));

    }

    private void AddPage()
    {
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (info == "ok")
        {
            int fid = int.Parse(Utils.GetRequest("fid", "post", 1, @"^[1-9]\d*$", "0"));
            string uid = Utils.GetRequest("uid", "post", 1, @"^[^\#]{1,10}(?:\#[^\#]{1,10}){0,500}$", "0");
            int addType = int.Parse(Utils.GetRequest("addType", "post", 1, @"^[0-4]$", "0"));
            string Content = Utils.GetRequest("Content", "post", 2, @"^[\s\S]{1,1000}$", "内容限1-1000字");
            if (uid == "0" && addType == 0)
            {
                Utils.Error("用户ID和群发选项至少填写/选择一项", "");
            }
            if (uid != "0")
            {
                string[] uidTemp = uid.Split("#".ToCharArray());
                for (int i = 0; i < uidTemp.Length; i++)
                {
                    int hid = Convert.ToInt32(uidTemp[i]);
                    string hname = new BCW.BLL.User().GetUsName(hid);
                    //发送内线
                    //new BCW.BLL.Guest().Add(uid, new BCW.BLL.User().GetUsName(uid), Content);

                    string mename = new BCW.BLL.User().GetUsName(fid);
                    BCW.Model.Guest model = new BCW.Model.Guest();
                    model.FromId = fid;
                    model.FromName = mename;
                    model.ToId = hid;
                    model.ToName = hname;
                    model.Content = Content;
                    model.TransId = 0;
                    new BCW.BLL.Guest().Add(model);
                }
                Utils.Success("发送内线", "发送内线成功..", Utils.getPage("guest.aspx"), "1");
            }
            else
            {
                string strWhere = string.Empty;
                if (addType == 1)
                {
                    strWhere = "EndTime>'" + DateTime.Now.AddDays(-2) + "'";
                }
                else if (addType == 2)
                {
                    strWhere = "EndTime>'" + DateTime.Now.AddDays(-7) + "'";
                }
                if (addType == 3)
                {
                    strWhere = "EndTime>'" + DateTime.Now.AddDays(-30) + "'";
                }
                if (addType == 4)
                {
                    strWhere = "ID>0";
                }
                int k = 0;
                string mename = new BCW.BLL.User().GetUsName(fid);
                DataSet ds = new BCW.BLL.User().GetList("ID,UsName", strWhere);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int hid = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                        string UsName = ds.Tables[0].Rows[i]["UsName"].ToString();
                        //发送内线
                        //new BCW.BLL.Guest().Add(hid, UsName, Content);


                        BCW.Model.Guest model = new BCW.Model.Guest();
                        model.FromId = fid;
                        model.FromName = mename;
                        model.ToId = hid;
                        model.ToName = UsName;
                        model.Content = Content;
                        model.TransId = 0;
                        new BCW.BLL.Guest().Add(model);

                        k++;
                    }
                }
                Utils.Success("群发内线", "群发" + k + "条内线成功..", Utils.getPage("guest.aspx"), "1");
            }

        }
        else
        {
            Master.Title = "发送内线";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("发送系统内线");
            builder.Append(Out.Tab("</div>", ""));
            string strText = "发送ID(填写0则为系统消息):/,用户ID(多个用#分开):/,或群发以下:/,内线内容/,,";
            string strName = "fid,uid,addType,Content,act,info";
            string strType = "num,textarea,select,textarea,hidden,hidden";
            string strValu = "''0''add'ok";
            string strEmpt = "true,true,0|不选择|1|两天内上线|2|一周内上线|3|一个月内上线|4|全部注册会员,true,false,false";
            string strIdea = "/";
            string strOthe = "发送内线,guest.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("温馨提示:<br />如果同时填写用户ID和选择群发，收件人默认为用户ID");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线管理</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }


    private void SearchPage()
    {
        Master.Title = "高级搜索";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("高级搜索");
        builder.Append(Out.Tab("</div>", ""));
        string strText = "发送ID:/,接收ID(可空)/,";
        string strName = "uid,toid,ptype";
        string strType = "num,num,hidden";
        string strValu = "''1";
        string strEmpt = "true,true,false";
        string strIdea = "/";
        string strOthe = "搜内线,guest.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("温馨提示:接收ID可以留空");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void ViewPage()
    {
        Master.Title = "查看内线";
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "ID错误"));
        BCW.Model.Guest model = new BCW.BLL.Guest().GetGuest(id);
        if (model == null)
        {
            Utils.Error("不存在的记录", "");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("查看内线");
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div>", ""));
        if (model.FromId == 0)
            builder.Append("发信人:系统");
        else
            builder.AppendFormat("发信人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>", model.FromId, model.FromName);

        builder.AppendFormat("<br />收信人:<a href=\"" + Utils.getUrl("uinfo.aspx?uid={0}&amp;backurl=" + Utils.PostPage(1) + "") + "\">{1}</a>", model.ToId, model.ToName);

        builder.Append("<br />内容:" + Out.SysUBB(model.Content) + "");
        builder.Append("<br />时间:" + DT.FormatDate(model.AddTime, 0) + "");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">返回上一级</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }

    private void DelPage()
    {
        string info = Utils.GetRequest("info", "all", 1, "", "");
        int id = int.Parse(Utils.GetRequest("id", "get", 2, @"^[0-9]\d*$", "ID错误"));
        if (info == "")
        {
            Master.Title = "删除内线";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定删除此内线记录吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok1&amp;act=del&amp;id=" + id + "&amp;backurl=" + Utils.getPage(0) + "") + "\">确定删除</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("guest.aspx") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (!new BCW.BLL.Guest().Exists(id))
            {
                Utils.Error("不存在的记录", "");
            }
            new BCW.BLL.Guest().Delete(id);
            Utils.Success("删除内线", "删除内线成功..", Utils.getPage("guest.aspx"), "1");
        }
    }

    //清空统计记录
    private void ClearPage()
    {
        Master.Title = "清空内线";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("请选择清空选项");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearok&amp;ptype=1") + "\">清空本日前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearok&amp;ptype=2") + "\">清空本周前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearok&amp;ptype=3") + "\">清空本月前</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearok&amp;ptype=4") + "\">清空全部</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", "<br />"));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearok&amp;ptype=5") + "\">清空全部(含收藏)</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearsys&amp;ptype=1") + "\">清空已读系统内线</a><br />");
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?act=clearsys&amp;ptype=2") + "\">清空全部系统内线</a>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("guest.aspx") + "\">内线管理</a><br />");
        builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("default.aspx") + "\">返回管理中心</a>"));
        builder.Append(Out.Tab("</div>", "<br />"));
    }


    //清空内线记录
    private void ClearOkPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }

        Master.Title = "清空内线";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-5]$", "0"));
        int v = int.Parse(Utils.GetRequest("v", "get", 1, @"^[1-2]$", "1"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "&amp;v=1") + "\">确定清空(不含未删除)</a><br />");
            builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "&amp;v=2") + "\">确定清空(含未删除)</a><br />");
            builder.Append("<a href=\"" + Utils.getPage("guest.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getPage("guest.aspx?info=acok&amp;act=" + act + "&amp;ptype=" + ptype + "&amp;v=" + v + ""), "2");
            }
            else
            {

                //已删除的内线
                string M_Str_mindate = string.Empty;
                string sWhe = " and IsKeep=0";
                string sWhe2 = "IsKeep=0";
                if (ptype == 5)
                    sWhe2 = "";

                if (v == 1)
                {
                    sWhe += " and TDel=1 and FDel=1";
                    if (sWhe2 != "")
                        sWhe2 += " and TDel=1 and FDel=1";
                    else
                        sWhe2 += "TDel=1 and FDel=1";

                }
                if (ptype == 1)
                {
                    //保留本日计算
                    M_Str_mindate = DateTime.Now.ToShortDateString() + " 0:00:00";
                    new BCW.BLL.Guest().DeleteStr("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
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
                    new BCW.BLL.Guest().DeleteStr("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
                }
                else if (ptype == 3)
                {
                    //保留本月计算
                    string MonthText = DateTime.Now.Year + "-" + DateTime.Now.Month;
                    M_Str_mindate = MonthText + "-1 0:00:00";
                    new BCW.BLL.Guest().DeleteStr("AddTime<'" + M_Str_mindate + "' " + sWhe + "");
                }
                else if (ptype == 4)
                {
                    new BCW.BLL.Guest().DeleteStr(sWhe2);
                }
                else if (ptype == 5)
                {
                    new BCW.BLL.Guest().DeleteStr(sWhe2);
                }
                Utils.Success("清空成功", "清空操作成功..", Utils.getPage("guest.aspx"), "2");
            }
        }
    }

    // 清空系统内线记录
    private void ClearSysPage()
    {
        int ManageId = new BCW.User.Manage().IsManageLogin();
        if (ManageId != 1 && ManageId != 11)
        {
            Utils.Error("只有系统管理员1号才可以进行此操作", "");
        }


        Master.Title = "清空系统内线";
        string act = Utils.GetRequest("act", "get", 1, "", "");
        string info = Utils.GetRequest("info", "get", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "get", 1, @"^[1-4]$", "0"));
        if (info != "ok" && info != "acok")
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("确定要清空吗");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Tab("<div>", ""));
            if (ptype == 1)
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "") + "\">确定清空已读系统内线</a><br />");
            else
                builder.Append("<a href=\"" + Utils.getUrl("guest.aspx?info=ok&amp;act=" + act + "&amp;ptype=" + ptype + "") + "\">确定清空全部系统内线</a><br />");

            builder.Append("<a href=\"" + Utils.getPage("guest.aspx?act=clear") + "\">先留着吧..</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
        }
        else
        {
            if (info == "ok")
            {
                Utils.Success("正在清空", "正在清空,请稍后..", Utils.getPage("guest.aspx?info=acok&amp;act=" + act + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {

                if (ptype == 1)
                {
                    new BCW.BLL.Guest().DeleteStr("FromId=0 and IsSeen=1");//清已读系统内线
                }
                else if (ptype == 2)
                {
                    new BCW.BLL.Guest().DeleteStr("FromId=0");//清全部系统内线
                }
                Utils.Success("清空成功", "清空系统内线成功..", Utils.getPage("guest.aspx"), "2");
            }
        }
    }


}

