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

public partial class Manage_xml_linkset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "友链系统设置";
        builder.Append(Out.Tab("", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[0-9]\d*$", "0"));
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        string xmlPath = "/Controls/link.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            if (ptype == 0)
            {
                string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
                string Notes = Utils.GetRequest("Notes", "post", 3, @"^[^\^]{1,50}$", "友链口号限50字内");
                string webName = Utils.GetRequest("webName", "post", 2, @"^[^\^]{1,20}$", "友链显示站名限1-20字内");
                string Domain = Utils.GetRequest("Domain", "post", 2, @"http://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", "请正确输入友链显示地址，如http://" + Utils.GetDomain() + "");
                string Summary = Utils.GetRequest("Summary", "post", 3, @"^[^\^]{1,200}$", "友链简介限200字内");
                string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
                string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
                string IsAc = Utils.GetRequest("IsAc", "post", 2, @"^[0-1]$", "审核性质选择错误");
                string IsUser = Utils.GetRequest("IsUser", "post", 2, @"^[0-1]$", "发布限制选择错误");
                string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");
                string IsPc = Utils.GetRequest("IsPc", "post", 2, @"^[0-1]$", "计量性质选择错误");
                string GoUrl = Utils.GetRequest("GoUrl", "post", 3, @"^[\s\S]{1,100}$", "跳到地址限100字符");
                string TopUbb = Utils.GetRequest("TopUbb", "post", 3, @"^[\s\S]{1,2000}$", "顶部UBB限2000字符");
                string FootUbb = Utils.GetRequest("FootUbb", "post", 3, @"^[\s\S]{1,2000}$", "底部UBB限2000字符");
                string Leibie = Utils.GetRequest("Leibie", "post", 3, @"^[^\|]{1,5}(?:\|[^\|]{1,5}){1,500}$", "分类格式填写错误,例子如:1|门户|2|音乐|3|软件");

                //----------计算分类合法性开始
                if (Leibie != "")
                {
                    int GetNum = Utils.GetStringNum(Leibie, "|");
                    if (GetNum % 2 == 0)
                    {
                        Utils.Error("分类格式填写错误,例子如:1|门户|2|音乐|3|软件", "");
                    }
                    string[] sTemp = Leibie.Split("|".ToCharArray());
                    for (int j = 0; j < sTemp.Length; j++)
                    {
                        if (j % 2 == 0)
                        {
                            if (sTemp[j] == "0")
                            {
                                Utils.Error("分类格式填写错误,例子如:1|门户|2|音乐|3|软件", "");
                                break;
                            }
                            try
                            {
                                int a = int.Parse(sTemp[j]);
                            }
                            catch
                            {
                                Utils.Error("分类格式填写错误,例子如:1|门户|2|音乐|3|软件", "");
                                break;
                            }
                            int b = int.Parse(sTemp[j]);
                            if (j != 0)
                            {
                                if ((b - 1) != int.Parse(sTemp[j - 2]))
                                {
                                    Utils.Error("分类格式填写错误,例子如:1|门户|2|音乐|3|软件", "");
                                    break;
                                }
                            }
                        }
                    }
                }
                //----------计算分类合法性结束

                xml.dss["LinkName"] = Name;
                xml.dss["LinkNotes"] = Notes;
                xml.dss["LinkwebName"] = webName;
                xml.dss["LinkDomain"] = Domain;
                xml.dss["LinkSummary"] = Summary;
                xml.dss["LinkLogo"] = Logo;
                xml.dss["LinkStatus"] = Status;
                xml.dss["LinkIsAc"] = IsAc;
                xml.dss["LinkIsUser"] = IsUser;
                xml.dss["LinkExpir"] = Expir;
                xml.dss["LinkIsPc"] = IsPc;
                xml.dss["LinkGoUrl"] = GoUrl;
                xml.dss["LinkTopUbb"] = TopUbb;
                xml.dss["LinkFootUbb"] = FootUbb;
                xml.dss["LinkLeibie"] = Leibie;
            }
            else {
                string bmType = Utils.GetRequest("bmType", "post", 2, @"^[0-1]$", "友链版面选择错误");
                string NameType = Utils.GetRequest("NameType", "post", 2, @"^[0-1]$", "站名显示选择错误");
                string ListType = Utils.GetRequest("ListType", "post", 2, @"^[0-3]$", "列表模式显示选择错误");
                string LeibieType = Utils.GetRequest("LeibieType", "post", 2, @"^[0-4]$", "分类模式显示选择错误");
                string ListNo = Utils.GetRequest("ListNo", "post", 2, @"^[0-9]\d*$", "分类模式每行友链数填写错误");
                string IsView = Utils.GetRequest("IsView", "post", 2, @"^[0-1]$", "友链前台数据显示选择错误");
                xml.dss["LinkbmType"] = bmType;
                xml.dss["LinkNameType"] = NameType;
                xml.dss["LinkListType"] = ListType;
                xml.dss["LinkLeibieType"] = LeibieType;
                xml.dss["LinkListNo"] = ListNo;
                xml.dss["LinkIsView"] = IsView;
            }
            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("友链系统设置", "设置成功，正在返回..", Utils.getUrl("linkset.aspx?ptype=" + ptype + ""), "1");
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            if (ptype == 0)
            {
                builder.Append("友链设置|");
                builder.Append("<a href=\"" + Utils.getUrl("linkset.aspx?ptype=1") + "\">显示设置</a>");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("linkset.aspx?ptype=0") + "\">友链设置</a>");
                builder.Append("|显示设置");
            }
            builder.Append(Out.Tab("</div>", ""));
            if (ptype == 0)
            {
                string strText = "友链名称:/,友链口号(可留空):/,友链显示站名:/,友链显示域名:/,友链显示简介:/,友链Logo(可留空):/,系统状态:/,审核性质:/,发布限制:/,防刷时间(秒):/,计量性质:/,链入跳到地址(留空跳首页):/,顶部Ubb:/,底部Ubb:/,友链分类(留空则不分类):/";
                string strName = "Name,Notes,webName,Domain,Summary,Logo,Status,IsAc,IsUser,Expir,IsPc,GoUrl,TopUbb,FootUbb,Leibie";
                string strType = "text,text,text,text,text,text,select,select,select,num,select,text,textarea,textarea,textarea";
                string strValu = "" + xml.dss["LinkName"] + "'" + xml.dss["LinkNotes"] + "'" + xml.dss["LinkwebName"] + "'" + xml.dss["LinkDomain"] + "'" + xml.dss["LinkSummary"] + "'" + xml.dss["LinkLogo"] + "'" + xml.dss["LinkStatus"] + "'" + xml.dss["LinkIsAc"] + "'" + xml.dss["LinkIsUser"] + "'" + xml.dss["LinkExpir"] + "'" + xml.dss["LinkIsPc"] + "'" + xml.dss["LinkGoUrl"] + "'" + xml.dss["LinkTopUbb"] + "'" + xml.dss["LinkFootUbb"] + "'" + xml.dss["LinkLeibie"] + "";
                string strEmpt = "false,true,false,false,true,true,0|正常|1|维护,0|需审核|1|不用审核,0|不作限制|1|仅限会员,false,0|统计手机|1|统计全部,true,true,true,true";
                string strIdea = "/";
                string strOthe = "确定修改|reset,linkset.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("温馨提示:分类格式规范例子:1|门户|2|音乐|3|软件,其中1、2、3是ID编号");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                string strText = "友链版面:/,站名显示:/,列表模式显示:/,分类模式显示:/,分类模式每行友链数:/,友链前台数据:/,";
                string strName = "bmType,NameType,ListType,LeibieType,ListNo,IsView,ptype";
                string strType = "select,select,select,select,num,select,hidden";
                string strValu = "" + xml.dss["LinkbmType"] + "'" + xml.dss["LinkNameType"] + "'" + xml.dss["LinkListType"] + "'" + xml.dss["LinkLeibieType"] + "'" + xml.dss["LinkListNo"] + "'" + xml.dss["LinkIsView"] + "'1";
                string strEmpt = "0|列表模式|1|分类模式,0|全称显示|1|简称显示,0|按进出比|1|按链入|2|按链出|3|按链入时间,0|按进出比|1|按链入|2|按链出|3|按链入时间|4|按推荐,false,0|显示|1|不显示,false";
                string strIdea = "/";
                string strOthe = "确定修改|reset,linkset.aspx,post,1,red|blue";
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