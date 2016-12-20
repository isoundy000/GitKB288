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
using BCW.Common;
public partial class bbs_guestsendflash: System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {          
        try
        {
            SendGuestTimer();
            Response.Write("刷新完成ok1");
        }
        catch(Exception  ee) 
        {
            Response.Write("刷新异常no1");
            Response.Write(ee.ToString());
        }
    }

    //定时发送的内线
    /// <summary>
    /// 检测guestsendlist表的guestsendID标识
    /// type=1时
    /// </summary>
    public void SendGuestTimer()
    {
        string guestsendflash = ub.GetSub("guestsendflash", "/Controls/guestsend.xml");
        if (guestsendflash != "0")//0 自动刷新 12手动 关闭
        {
            return;
        }
        string strWhere = " isDone=1 and  sentDay>=isSendCount ";
        //内线控制表
        DataSet dssend = new BCW.BLL.tb_GuestSend().GetList(" * ", strWhere);
        DataSet dssendList = null;
        BCW.Model.tb_GuestSend mm = new BCW.Model.tb_GuestSend();
        if (dssend != null && dssend.Tables[0].Rows.Count > 0)
        {
            string username = "";
            string guestContent = "";
            int guestid = 0;
            int guessCount = 0;
            BCW.Model.tb_GuestSendList list = new BCW.Model.tb_GuestSendList();
            for (int i = 0; i < dssend.Tables[0].Rows.Count; i++)
            {
                guestContent = dssend.Tables[0].Rows[i]["guestContent"].ToString();
                //发送列控制表
                dssendList = new BCW.BLL.tb_GuestSendList().GetList("  usid,guestsendID,type ", " type=1 and guestsendID=" + dssend.Tables[0].Rows[i]["ID"].ToString() + " group by usid,guestsendID,type ");
                int userid = 0;
                //string sendtime = DateTime.Now.ToString("yyyyMMdd") + dssend.Tables[0].Rows[i]["sendTime"].ToString();
                //builder.Append("====" + DateTime.Now.Hour + "====" + dssend.Tables[0].Rows[i]["sendTime"].ToString() + DateTime.Now.Hour.ToString().Equals(dssend.Tables[0].Rows[i]["sendTime"].ToString()));
                //同一小时内比较
                if (DateTime.Now.Hour.ToString() == (dssend.Tables[0].Rows[i]["sendTime"].ToString().Trim()))
                {
                    //builder.Append((DateTime.Now.Hour.ToString() + "==" + dssend.Tables[0].Rows[i]["sendTime"].ToString().Trim()));
                   // builder.Append("当前检测GuestSend :" + dssend.Tables[0].Rows[i]["ID"].ToString() + "<br/>----------<br/>");
                    mm = new BCW.BLL.tb_GuestSend().Gettb_GuestSend(Convert.ToInt32(dssend.Tables[0].Rows[i]["ID"]));
                    if (Convert.ToDateTime(mm.sendDateTime).Day == DateTime.Now.Day)
                    { continue; }
                    if (dssendList != null && dssendList.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dssendList.Tables[0].Rows.Count; j++)
                        {
                            if (!("#" + dssend.Tables[0].Rows[i]["seeUidList"].ToString()).Contains("#" + dssendList.Tables[0].Rows[j]["usid"].ToString() + "#"))
                            {
                                //为1时未阅读
                                if (dssendList.Tables[0].Rows[j]["type"].ToString().Trim() == "1")
                                {
                                    guessCount++;
                                    userid = Convert.ToInt32(dssendList.Tables[0].Rows[j]["usid"]);
                                    username = new BCW.BLL.User().GetUsName(userid);
                                    guestid = new BCW.BLL.Guest().AddNew(5, userid, username, guestContent);
                                    list.addtime = DateTime.Now;
                                    list.getGold = 0;
                                    list.guestID = guestid;
                                    list.guestsendID = Convert.ToInt32(dssendList.Tables[0].Rows[j]["guestsendID"]);
                                    list.idDone = 1;
                                    list.overtime = DateTime.Now.AddDays(30);
                                    list.remark = "";
                                    list.type = 1;//未阅读
                                    list.usid = userid;
                                    int aa = new BCW.BLL.tb_GuestSendList().Add(list);
                                   Response.Write("当前已发送tb_GuestSendList_ID:" + aa+"<br/>");
                                }
                            }
                        }
                    }
                    mm.remark = " " + DateTime.Now.ToString("yyyy-MM-dd ") + mm.sendTime.ToString().Trim() + " 时内线刷新机已发送";
                    mm.sendCount += guessCount;//总发送量
                    mm.isSendCount += 1;
                    mm.sendDateTime = DateTime.Now;
                    if (mm.isSendCount == mm.sentDay)
                    {
                        mm.overtime = DateTime.Now;
                        mm.isDone = 0;//正式停止标识s
                        mm.remark = " " + DateTime.Now.ToString("yyyy-MM-dd ") + mm.sendTime.ToString().Trim() + " 时内线已发送(已完结)";
                    }
                    //操作，放回guestsend
                    new BCW.BLL.tb_GuestSend().Update(mm);
                    Response.Write("当前已放回tb_GuestSend_ID:" + mm.ID + "<br/>");
                }

                #region 
                //guestContent = dssend.Tables[0].Rows[i]["guestContent"].ToString();
                //senduserID = dssend.Tables[0].Rows[i]["notSeenIDList"].ToString();
                //userID = senduserID.Split('#');
                //for (int j = 0; j < userID.Length; j++)
                //{
                //    usid = Convert.ToInt32(userID[i]);
                //    username = new BCW.BLL.User().GetUsName(Convert.ToInt32(userID[j]));
                //    guestid = new BCW.BLL.Guest().AddNew(5, usid, username, guestContent);
                //    //uidlist += usid.ToString() + "#"; 
                //    guessCount++;//已发送内线的id
                //    list.addtime = DateTime.Now;
                //    list.getGold = 0;
                //    list.guestID = guestid;
                //    list.guestsendID = Convert.ToInt32(dssend.Tables[i].Rows[i]["ID"]);
                //    list.idDone = 1;
                //    list.overtime = DateTime.Now.AddDays(30);
                //    list.remark = "";
                //    list.type = 1;//未阅读
                //    list.usid = usid;
                //    new BCW.BLL.tb_GuestSendList().Add(list);
                //}
                #endregion

            }
        }
       // Utils.Success("刷新成功,3s后返回", "刷新成功,3s后返回...", Utils.getUrl("guestsend.aspx?act=view&amp;backurl=" + Utils.getPage(0) + ""), "3")
    }

}
