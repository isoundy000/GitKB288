using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BCW.Common;
using BCW.PK10;
using System.Data;
using System.Collections;

public partial class bbs_game_RobotGold : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/RobotGold.xml";
    protected void Page_Load(object sender, EventArgs e)
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
                if (!string.IsNullOrEmpty(expids))
                {
                    string[] ids = expids.Split('#');
                    if (ids.Length > 0)
                    {
                        for (int i = 0; i < ids.Length; i++)
                        {
                            int id = 0;
                            int.TryParse(ids[i], out id);
                            if (id > 0)
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
                string cSQL = "Select id,UsName,iGold from tb_user where IsSpier=1 and iGold<" + minmoney + whereIds;
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
        catch (Exception ex)
        {
            context.Append("error1:" + ex.Message);
        }
        //
        Response.Write(context);
    }
}