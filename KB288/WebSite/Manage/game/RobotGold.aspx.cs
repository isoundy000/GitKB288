using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
public partial class Manage_RobotCheck : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string myFileName = "RobotGold.aspx";
    protected string xmlPath = "/Controls/RobotGold.xml";
    protected string title = "系统号自动补币";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "all", 1, "", "");
        switch (act)
        {
            case "run":
                RunPage();
                break;
            case "save":
                SavePage();
                break;
            default:
                ReloadPage();
                break;
        }
    }
    private void ReloadPage()
    {
        Master.Title = title+".管理";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏管理</a>&gt;");
        builder.Append(title);
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"hr\"></div>", Out.Hr()));
        //
        Application.Remove(xmlPath);//清缓存
        ub xml = new ub();
        xml.ReloadSub(xmlPath); //加载配置
        //
        string strText = "状态:/,少于X币才补:/,补币数:/,排除的系统号(#号分隔):/,,";
        string strName = "status,minmoney,addmoney,expids,act,backurl";
        string strType = "select,num,num,text,hidden,hidden";
        string strValu = "" + xml.dss["Status"] + "'" + xml.dss["MinMoney"] + "'" + xml.dss["AddMoney"] +"'"+ xml.dss["ExpIds"] + "'save'" + Utils.getPage(0) + "";
        string strEmpt = "0|禁用|1|启用,false,false,true,false,false";
        string strIdea = "/";
        string strOthe = "确定|reset,"+ myFileName+",post,1,red|blue";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        //
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName+"?act=run") + "\">手动执行一次</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

    }
    private void SavePage()
    {
        string status = Utils.GetRequest("Status", "post", 2, @"^[0-1]$", "状态设置有错误");
        string minmoney = Utils.GetRequest("MinMoney", "post", 4, @"^[0-9]\d*$", "游戏口号限50字内");
        string addmoney = Utils.GetRequest("AddMoney", "post", 4, @"^[0-9]\d*$", "游戏提示限50字内");
        string expids= Utils.GetRequest("ExpIds", "post", 3, @"^[^\#]{1,50}(?:\#[^\#]{1,50}){0,500}$", "排除的系统号ID请用#分隔，可以留空");
        //
        ub xml = new ub();
        xml.ReloadSub(xmlPath); //加载配置
        xml.dss["Status"] = status;
        xml.dss["MinMoney"] = minmoney;
        xml.dss["AddMoney"] = addmoney;
        xml.dss["ExpIds"] = expids;
        //
        System.IO.File.WriteAllText(Server.MapPath(xmlPath), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        Utils.Success(title+"设置", "设置成功，正在返回..", Utils.getUrl(myFileName), "1");
    }
    private void RunPage()
    {
        System.Text.StringBuilder context = new System.Text.StringBuilder("");
        try
        {
            int status = 0, minmoney = 0, addmoney = 0;
            int.TryParse(ub.GetSub("Status", xmlPath), out status);
            int.TryParse(ub.GetSub("MinMoney", xmlPath), out minmoney);
            int.TryParse(ub.GetSub("AddMoney", xmlPath), out addmoney);
            string expids = ub.GetSub("ExpIds", xmlPath);
            if (status == 0 || addmoney <= 0)
            {
                context.Append("close1:" + "没开启自动补币功能！");
            }
            else
            {
                int rows = 0;
                int errros = 0;
                string errmsg = "";
                #region 补币
                #region 排除的ID列表
                string whereIds = "";
                if(!string.IsNullOrEmpty(expids))
                {
                    string[] ids = expids.Split('#');
                    if(ids.Length>0)
                    {
                        for(int i=0;i<ids.Length;i++)
                        {
                            int id = 0;
                            int.TryParse(ids[i], out id);
                            if(id>0)
                            {
                                if (!string.IsNullOrEmpty(whereIds))
                                    whereIds += ",";
                                whereIds += "'" + id.ToString().Trim() + "'";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(whereIds))
                    whereIds = " and id not in(" + whereIds + ")";
                #endregion
                string cSQL = "Select id,UsName,iGold from tb_user where IsSpier=1 and iGold<" + minmoney+whereIds;
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        try
                        {
                            int uid = int.Parse(dr["ID"].ToString());
                            string uName = dr["usName"].ToString().Trim();
                            int oldgold = int.Parse(dr["iGold"].ToString());
                            ArrayList sqlList = new ArrayList();
                            #region 会员余额累加
                            cSQL = "update tb_user set iGold=iGold+" + addmoney + " where ID=" + uid.ToString();
                            sqlList.Add(cSQL);
                            #endregion
                            #region 更新消费记录tb_GoldLogs
                            cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
                            cSQL += "0";
                            cSQL += ",''";//操作的文件名
                            cSQL += "," + uid.ToString().Trim();
                            cSQL += ",'" + uName.Trim() + "'";
                            cSQL += "," + addmoney.ToString().Trim();
                            cSQL += "," + (oldgold + addmoney).ToString().Trim();
                            // cSQL += ",'" + myPublishGameName.Trim() + "投注(第" + buy.ListNo.Trim() + "期ID" + buy.ID.ToString().Trim() + ")" + buy.BuyDescript.Trim() + "'";
                            cSQL += ",'补币'";
                            cSQL += ",'" + DateTime.Now.ToString().Trim() + "'";
                            cSQL += ",0,0)";
                            sqlList.Add(cSQL);
                            #endregion
                            BCW.PK10.MySqlHelper.ExecuteSqlTran(sqlList);
                            rows++;
                        }
                        catch (Exception ex)
                        {
                            errros++;
                            errmsg += ex.Message;
                        }
                    }
                }
                #endregion
                context.Append("ok1:" + "补偿系统号个数:" + rows + " 失败个数：" + errros + errmsg);
            }
        }
        catch(Exception ex)
        {
            context.Append("error1:"+ex.Message);
        }
        //
        builder = context;
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl(myFileName) + "\">返回设置</a><br />");
        builder.Append(Out.Tab("</div>", "<br />"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../default.aspx") + "\">返回管理中心</a>");
        builder.Append(Out.Tab("</div>", "<br />"));
        //

    }
}