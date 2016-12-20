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

public partial class Manage_xml_newsset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "采集新闻设置";
        builder.Append(Out.Tab("", ""));
        int Types = 1;

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/news.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            for (int i = 0; i <= 8; i++)
            {
                string ID = Utils.GetRequest("ID" + i + "", "post", 2, @"^[0-9]\d*$", "导入ID填写错误");
                xml.dss["NewsID"+i+""] = ID;
                if (Types == 1)
                {
                    string ID2 = Utils.GetRequest("IDb" + i + "", "post", 2, @"^[0-9]\d*$", "会员ID填写错误");
                    xml.dss["NewsIDb" + i + ""] = ID2;
                }
            }
            xml.dss["NewsTypes"] = Types;
 
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("邮箱系统设置", "设置成功，正在返回..", Utils.getUrl("newsset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "采集新闻设置"));

            //国内0
            //国际1
            //社会2
            //国际财经3
            //军事4
            //体育5
            //娱乐6
            //文化7
            //汽车8

            if (Types == 0)
            {
                string strText = "新闻导入:/,国内新闻导入栏目ID:/,国际新闻导入栏目ID:/,社会新闻导入栏目ID:/,财经新闻导入栏目ID:/,军事新闻导入栏目ID:/,体育新闻导入栏目ID:/,娱乐新闻导入栏目ID:/,文化新闻导入栏目ID:/,汽车新闻导入栏目ID:/,";
                string strName = "Types,ID0,ID1,ID2,ID3,ID4,ID5,ID6,ID7,ID8";
                string strType = "hidden,num,num,num,num,num,num,num,num,num";
                string strValu = "0'" + xml.dss["NewsID0"] + "'" + xml.dss["NewsID1"] + "'" + xml.dss["NewsID2"] + "'" + xml.dss["NewsID3"] + "'" + xml.dss["NewsID4"] + "'" + xml.dss["NewsID5"] + "'" + xml.dss["NewsID6"] + "'" + xml.dss["NewsID7"] + "'" + xml.dss["NewsID8"] + "";
                string strEmpt = "0|文章栏目|1|论坛列表,false,false,false,false,false,false,false,false,false";

                string strIdea = "/";
                string strOthe = "确定修改|reset,newsset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
            else
            {

                string strText = "新闻导入:/,国内新闻导入论坛ID:/,国内新闻发表会员ID:/,国际新闻导入论坛ID:/,国际新闻发表会员ID:/,社会新闻导入论坛ID:/,社会新闻发表会员ID:/,财经新闻导入论坛ID:/,财经新闻发表会员ID:/,军事新闻导入论坛ID:/,军事新闻发表会员ID:/,体育新闻导入论坛ID:/,体育新闻发表会员ID:/,娱乐新闻导入论坛ID:/,娱乐新闻发表会员ID:/,文化新闻导入论坛ID:/,文化新闻发表会员ID:/,汽车新闻导入论坛ID:/,汽车新闻发表会员ID:/";
                string strName = "Types,ID0,IDb0,ID1,IDb1,ID2,IDb2,ID3,IDb3,ID4,IDb4,ID5,IDb5,ID6,IDb6,ID7,IDb7,ID8,IDb8";
                string strType = "hidden,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num,num";
                string strValu = "1'" + xml.dss["NewsID0"] + "'" + xml.dss["NewsIDb0"] + "'" + xml.dss["NewsID1"] + "'" + xml.dss["NewsIDb1"] + "'" + xml.dss["NewsID2"] + "'" + xml.dss["NewsIDb2"] + "'" + xml.dss["NewsID3"] + "'" + xml.dss["NewsIDb3"] + "'" + xml.dss["NewsID4"] + "'" + xml.dss["NewsIDb4"] + "'" + xml.dss["NewsID5"] + "'" + xml.dss["NewsIDb5"] + "'" + xml.dss["NewsID6"] + "'" + xml.dss["NewsIDb6"] + "'" + xml.dss["NewsID7"] + "'" + xml.dss["NewsIDb7"] + "'" + xml.dss["NewsID8"] + "'" + xml.dss["NewsIDb8"] + "";
                string strEmpt = "0|文章栏目|1|论坛列表,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false,false";

                string strIdea = "/";
                string strOthe = "确定修改|reset,newsset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
