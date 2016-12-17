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

public partial class Manage_xml_dzpkset : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    BCW.dzpk.dzpk dz = new BCW.dzpk.dzpk();

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Title = "德州扑克管理";

        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("..\\default.aspx") + "\">管理中心</a>&gt;");
        builder.Append("<a href=\"" + Utils.getUrl("..\\game\\dzpk.aspx") + "\">德州扑克管理</a>&gt;");
        builder.Append("基础配置");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));

        //设置XML地址
        ub xml = new ub();
        string xmlPath = "/Controls/dzpk.xml";
        Application.Remove(xmlPath);//清缓存
        xml.ReloadSub(xmlPath); //加载配置

        if (Utils.ToSChinese(ac) == "确定修改")
        {
            string DzpkName = Utils.GetRequest("DzpkName", "post", 2, @"^[^\^]{1,20}$", "系统名称限1-20字内");
            string GameStatus = Utils.GetRequest("GameStatus", "post", 2, @"^[0-2]$", "系统状态 选择出错");
            string RUnits = Utils.GetRequest("RUnits", "post", 2, @"^[^\^]{1}$", "房间单位 填写错误");
            string RmType = Utils.GetRequest("RmType", "post", 2, @"^[^\^]{1,200}$", "房型类型 填写超出范围");
            string DzCoin = Utils.GetRequest("DzCoin", "post", 2, @"^[^\^]{1,200}$", "测试币名称 填写错误");
            string GSmallb = Utils.GetRequest("GSmallb", "post", 2, @"^[^\^]{1,200}$", "小盲注初始值 填写错误");
            string GBigb = Utils.GetRequest("GBigb", "post", 2, @"^[^\^]{1,200}$", "大盲注初始值 填写错误");
            string GMinb = Utils.GetRequest("GMinb", "post", 2, @"^[^\^]{1,200}$", "最小资金值 填写错误");
            string GMaxb = Utils.GetRequest("GMaxb", "post", 2, @"^[^\^]{1,200}$", "最大资金值 填写错误");
            string GSerCharge = Utils.GetRequest("GSerCharge", "post", 2, @"^[^\^]{1,200}$", "手续费 填写错误");
            string GMaxUser = Utils.GetRequest("GMaxUser", "post", 2, @"^[^\^]{1,200}$", "房间最大人数 填写错误");
            string GsetTime = Utils.GetRequest("GsetTime", "post", 2, @"^[^\^]{1,200}$", "操作时间 填写错误");
            string GetGoldTime = Utils.GetRequest("GetGoldTime", "post", 2, @"^[^\^]{1,200}$", "测试币领取间隔 填写错误");
            string GetGold = Utils.GetRequest("GetGold", "post", 2, @"^[^\^]{1,200}$", "测试币每次领取金额 填写错误");
            string DemoIDS = Utils.GetRequest("DemoIDS", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,5000}$", "内测ID填写错误");
            int err = 0;
            //检查输入是否正确，直接更改到房间
            try
            {
                #region 更新到房间
                string[] tmRmType = RmType.Split('|');
                for (int i = 0; i < tmRmType.Length; i++)
                {
                    //检查数据源是否完整
                    long mGSmallb = long.Parse(GSmallb.Split('|')[i].ToString());
                    long mGBigb = long.Parse(GBigb.Split('|')[i].ToString());
                    long mGMinb = long.Parse(GMinb.Split('|')[i].ToString());
                    long mGMaxb = long.Parse(GMaxb.Split('|')[i].ToString());
                    long mGSerCharge = long.Parse(GSerCharge.Split('|')[i].ToString());
                    int mGMaxUser = int.Parse(GMaxUser.Split('|')[i].ToString());
                    int mGsetTime = int.Parse(GsetTime.Split('|')[i].ToString());

                    DataTable dt_Room = new BCW.dzpk.BLL.DzpkRooms().GetList("ID", " DRType=" + i).Tables[0];

                    for (int j = 0; j < dt_Room.Rows.Count; j++)
                    {
                        BCW.dzpk.Model.DzpkRooms Room_Model = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(int.Parse(dt_Room.Rows[j]["ID"].ToString()));
                        Room_Model.GSmallb = mGSmallb;
                        Room_Model.GBigb = mGBigb;
                        Room_Model.GMinb = mGMinb;
                        Room_Model.GMaxb = mGMaxb;
                        Room_Model.GSerCharge = mGSerCharge;
                        Room_Model.GMaxUser = mGMaxUser;
                        Room_Model.GSetTime = mGsetTime;
                        new BCW.dzpk.BLL.DzpkRooms().Update(Room_Model);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                err++;
                Utils.Error("设置失败，输入数值有误<br />" + ex.StackTrace, Utils.getUrl("dzpkset.aspx?backurl=" + Utils.getPage(0) + ""));
            }

            if (err == 0)
            {
                #region 更新XML数据
                xml.dss["DzpkName"] = DzpkName;
                xml.dss["GameStatus"] = GameStatus;
                xml.dss["DzCoin"] = DzCoin;
                xml.dss["RUnits"] = RUnits;
                xml.dss["RmType"] = RmType;
                xml.dss["GSmallb"] = GSmallb;
                xml.dss["GBigb"] = GBigb;
                xml.dss["GMinb"] = GMinb;
                xml.dss["GMaxb"] = GMaxb;
                xml.dss["GMaxUser"] = GMaxUser;
                xml.dss["GsetTime"] = GsetTime;
                xml.dss["GSerCharge"] = GSerCharge;
                xml.dss["GetGoldTime"] = GetGoldTime;
                xml.dss["GetGold"] = GetGold;
                xml.dss["DemoIDS"] = DemoIDS;
                System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                Utils.Success("德州扑克游戏设置", "设置成功，正在返回..", Utils.getUrl("dzpkset.aspx?backurl=" + Utils.getPage(0) + ""), "1");
                #endregion
            }
        }
        else
        {
            string strText = "游戏名称:/,游戏状态(正常状态扣" + ub.Get("SiteBz") + "，测试状态扣测试币):/,房间单位:/,房间类型:/,测试币名称:/,小盲注初始值（" + ub.Get("SiteBz") + "）:/,大盲注初始值（" + ub.Get("SiteBz") + "）:/,最小资金值（" + ub.Get("SiteBz") + "）:/,最大资金值（" + ub.Get("SiteBz") + "）:/,手续费(%):/,房间最大人数:/,操作时间:/,测试币领取间隔(秒):/,测试币每次领取金额:/,测试ID(留空不限制，多个ID用#分隔):/,";
            string strName = "DzpkName,GameStatus,RUnits,RmType,DzCoin,GSmallb,GBigb,GMinb,GMaxb,GSerCharge,GMaxUser,GsetTime,GetGoldTime,GetGold,DemoIDS,backurl";
            string strType = "text,select,select,text,text,text,text,text,text,text,text,text,text,text,text,hidden";
            string strValu = "" + xml.dss["DzpkName"] + "'" + xml.dss["GameStatus"] + "'" + xml.dss["RUnits"] + "'" + xml.dss["RmType"] + "'" + xml.dss["DzCoin"] + "'" + xml.dss["GSmallb"] + "'" + xml.dss["GBigb"] + "'" + xml.dss["GMinb"] + "'" + xml.dss["GMaxb"] + "'" + xml.dss["GSerCharge"] + "'" + xml.dss["GMaxUser"] + "'" + xml.dss["GsetTime"] + "'" + xml.dss["GetGoldTime"] + "'" + xml.dss["GetGold"] + "'" + xml.dss["DemoIDS"] + "'" + Utils.getPage(0) + "";
            string strEmpt = "false,0|正常|1|维护|2|测试,桌|桌|房|房,false,false,false,false,false,false,false,false,true,true,false,false";
            string strIdea = "/";
            string strOthe = "确定修改|reset,dzpkset.aspx,post,1,red|blue";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append("<font color=red>温馨提示:</font><br />本页设置 【将更改所有对应房间的定义，会影响在线的房间定义】 如需单独改变原有房间设置，直接进入房间管理即可。<br />标记的符号'|'为分隔对应每个房间的数值，不可缺少。");
            builder.Append(Out.Tab("</div>", ""));
            builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getPage("") + "\">返回上级</a><br />");
            builder.Append(Out.Tab("</div><div class=\"title\"><a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>", "<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>"));
            builder.Append(Out.Tab("</div>", "<br />"));
        }
    }
}
