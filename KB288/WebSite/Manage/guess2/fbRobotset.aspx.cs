using System;
using System.Collections;
using System.Collections.Generic;
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
public partial class Manage_game_fbrobotset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");

    protected string xmlPath = "/Controls/guess2.xml";
    protected string GameName = ub.GetSub("SiteName", "/Controls/guess2.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            default:
                ReloadPage();
                break;
        }
    }

    private void ReloadPage()
    {
        Master.Title = "球彩竞猜_机器人";
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/guess2.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string ROBOTID = Utils.GetRequest("ROBOTID", "post", 1, @"^[^\^]{1,2000}$", "");//机器人ID
            string IsBot = Utils.GetRequest("IsBot", "post", 1, @"^[0-1]$", "0");//机器人状态
            string ROBOTBUY = Utils.GetRequest("ROBOTBUY", "post", 1, @"^[0-9]$", "1");//机器人每场比赛购买次数
            string ROBOTMIAO = Utils.GetRequest("ROBOTMIAO", "post", 1, @"^[^\^]{1,2000}$", "100");//机器人下注金额
            string fzzhu = Utils.GetRequest("fzzhu", "post", 1, @"^[^\^]{1,2000}$", xml.dss["ROBOTMIAO"].ToString());//多少分钟下多少注
            string zuqiupay = Utils.GetRequest("zuqiupay", "post", 1, @"^[0-1]$", "0");//足球下注
            string lanqiupay = Utils.GetRequest("lanqiupay", "post", 1, @"^[0-1]$", "0");//篮球下注

            //判断是否为系统号+判断是否存在该ID
            string[] ss = ROBOTID.Split('#');
            if (ROBOTID.Length > 0)
            {
                for (int pp = 0; pp < ROBOTID.Split('#').Length; pp++)
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

            xml.dss["ROBOTID"] = ROBOTID.Replace("\r\n", "").Replace(" ", "");
            xml.dss["IsBot"] = IsBot;
            xml.dss["ROBOTBUY"] = ROBOTBUY;
            xml.dss["ROBOTMIAO"] = ROBOTMIAO.Replace("\r\n", "").Replace(" ", "");
            xml.dss["fzzhu"] = fzzhu;
            xml.dss["zuqiupay"] = zuqiupay;
            xml.dss["lanqiupay"] = lanqiupay;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + GameName + "_机器人", "机器人设置成功，正在返回..", Utils.getUrl("fbrobotset.aspx?act=robot"), "1");
        }
        else
        {
            int nugg = 0;
            if (xml.dss["ROBOTID"].ToString() == "")
                nugg = 0;
            else
                nugg = xml.dss["ROBOTID"].ToString().Split('#').Length;
            builder.Append(Out.Div("title", "<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏</a>&gt;<a href=\"" + Utils.getUrl("default.aspx") + "\">" + GameName + "</a>&gt;机器人"));
            string strText = "机器人状态:,足球下注:,篮球下注:,多少分钟下多少注:(用英文逗号分隔)/,机器人ID:现在共有：" + nugg + "个机械人/,机器人每场比赛购买次数:（0为不限制）/,机器人下注金额:/";
            string strName = "IsBot,zuqiupay,lanqiupay,fzzhu,ROBOTID,ROBOTBUY,ROBOTMIAO";
            string strType = "select,select,select,text,big,text,big";
            string strValu = "" + xml.dss["IsBot"].ToString() + "'" + xml.dss["zuqiupay"].ToString() + "'" + xml.dss["lanqiupay"].ToString() + "'" + xml.dss["fzzhu"].ToString() + "'" + xml.dss["ROBOTID"].ToString() + "'" + xml.dss["ROBOTBUY"].ToString() + "'" + xml.dss["ROBOTMIAO"].ToString() + "";
            string strEmpt = "0|关闭|1|开启,0|关闭|1|开启,0|关闭|1|开启,true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定修改,fbrobotset.aspx?act=robot,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
    }

}
