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

public partial class Manage_xml_groupset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {
        string xmlPath = "/Controls/group.xml";
        Master.Title = "" + ub.GetSub("GroupName", xmlPath) + "系统设置";
        builder.Append(Out.Tab("", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        ub xml = new ub();
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置
        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string Name = Utils.GetRequest("Name", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string Logo = Utils.GetRequest("Logo", "post", 3, @"^[^\^]{1,200}$", "Logo地址限200字内");
            string zName = Utils.GetRequest("zName", "post", 2, @"^[^\^]{1,5}$", "主人名称限1-5字内");
            string Status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "系统状态选择出错");
            string IsAdd = Utils.GetRequest("IsAdd", "post", 2, @"^[0-1]$", "是否允许自建聊室选择错误");
            string AddLeven = Utils.GetRequest("AddLeven", "post", 2, @"^[0-9]\d*$", "自建需要等级填写错误");
            string AddPrice = Utils.GetRequest("AddPrice", "post", 2, @"^[0-9]\d*$", "自建聊室手续费填写错误");
            string iPrice = Utils.GetRequest("iPrice", "post", 2, @"^[0-9]\d*$", "每小时需付多少币填写错误");
            string GuestPrice = Utils.GetRequest("GuestPrice", "post", 2, @"^[0-9]\d*$", "群发成员每人收币填写错误");
            string PelNum = Utils.GetRequest("PelNum", "post", 2, @"^[0-9]\d*$", "申请论坛需成员人数填写错误");
            string Pel2Num = Utils.GetRequest("Pel2Num", "post", 2, @"^[0-9]\d*$", "申请聊室需成员人数填写错误");
            string Expir = Utils.GetRequest("Expir", "post", 2, @"^[0-9]\d*$", "防刷时间填写错误");
            string CTID = Utils.GetRequest("CTID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "穿透ID请用#分隔，可以留空");
            string GuestID = Utils.GetRequest("GuestID", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "接收开圈审核内线ID请用#分隔，可以留空");

            xml.dss["GroupName"] = Name;
            xml.dss["GroupLogo"] = Logo;
            xml.dss["GroupzName"] = zName;
            xml.dss["GroupStatus"] = Status;
            xml.dss["GroupIsAdd"] = IsAdd;
            xml.dss["GroupAddLeven"] = AddLeven;
            xml.dss["GroupAddPrice"] = AddPrice;
            xml.dss["GroupiPrice"] = iPrice;
            xml.dss["GroupGuestPrice"] = GuestPrice;
            xml.dss["GroupPelNum"] = PelNum;
            xml.dss["GroupPel2Num"] = Pel2Num;
            xml.dss["GroupExpir"] = Expir;
            xml.dss["GroupCTID"] = CTID;
            xml.dss["GroupGuestID"] = GuestID;

            System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
            Utils.Success("" + ub.GetSub("GroupName", xmlPath) + "系统设置", "设置成功，正在返回..", Utils.getUrl("groupset.aspx"), "1");
        }
        else
        {
            builder.Append(Out.Div("title", "" + ub.GetSub("GroupName", xmlPath) + "系统设置"));

            string strText = "" + ub.GetSub("GroupName", xmlPath) + "名称:/," + ub.GetSub("GroupName", xmlPath) + "Logo(可留空):/," + ub.GetSub("GroupName", xmlPath) + "主人名称:/,系统状态:/,是否允许申请创" + ub.GetSub("GroupName", xmlPath) + ":/,创" + ub.GetSub("GroupName", xmlPath) + "需要等级(级):/,创" + ub.GetSub("GroupName", xmlPath) + "手续费(币):/,每天需付多少币:/,群发成员每人收币:/,申请论坛需成员人数:/,申请聊室需成员人数:/,发言防刷(秒):/,穿透限制ID(多个用#分隔):/,接收开圈审核内线ID(多个用#分隔):/";
            string strName = "Name,Logo,zName,Status,IsAdd,AddLeven,AddPrice,iPrice,GuestPrice,PelNum,Pel2Num,Expir,CTID,GuestID";
            string strType = "text,text,text,select,select,num,num,num,num,num,num,num,textarea,textarea";
            string strValu = "" + xml.dss["GroupName"] + "'" + xml.dss["GroupLogo"] + "'" + xml.dss["GroupzName"] + "'" + xml.dss["GroupStatus"] + "'" + xml.dss["GroupIsAdd"] + "'" + xml.dss["GroupAddLeven"] + "'" + xml.dss["GroupAddPrice"] + "'" + xml.dss["GroupiPrice"] + "'" + xml.dss["GroupGuestPrice"] + "'" + xml.dss["GroupPelNum"] + "'" + xml.dss["GroupPel2Num"] + "'" + xml.dss["GroupExpir"] + "'" + xml.dss["GroupCTID"] + "'" + xml.dss["GroupGuestID"] + "";
            string strEmpt = "false,true,false,0|正常|1|维护,0|允许|1|禁止,false,false,false,false,false,false,false,true,true";
            string strIdea = "/";
            string strOthe = "确定修改|reset,groupset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">配置中心</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}

