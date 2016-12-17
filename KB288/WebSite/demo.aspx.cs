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
using System.Data.SqlClient;

public partial class demo : System.Web.UI.Page
{


    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected void Page_Load(object sender, EventArgs e)
    {

       //DataSet ds = new BCW.BLL.Text().GetList("id", "select  from tb_tbapay where group by id having count(*)>1");

       //DataSet ds = new TPR2.BLL.guess.BaPay().GetBaPayList("id", "dt>=0 group by id having count(*)>1");

        DataSet ds = BCW.Data.SqlHelper.Query("select Mobile,EndIP from tb_User  where IsVerify=1");

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string EndIP = ds.Tables[0].Rows[i]["EndIP"].ToString();
                string Mobile = ds.Tables[0].Rows[i]["Mobile"].ToString();

                string ipCity = string.Empty;
                if (!string.IsNullOrEmpty(EndIP))
                {
                    ipCity = new IPSearch().GetAddressWithIP(EndIP);
                    if (!string.IsNullOrEmpty(ipCity))
                    {
                        ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
                    }
                    if (!ipCity.Contains("肇庆"))
                    {
                        builder.Append("" + Mobile + "<br />");
                    }
                }

               
            }
        }



        //DataSet ds = new BCW.BLL.Goldlog().GetList("usid,Sum(AcGold) as cent", "Types=0 and addtime>'2014-3-1 12:00:00' and addtime<'2014-3-20 15:00:00' and PUrl LIKE '/bbs/guess%' group by usid");

        //int iTar = 50;
        //if (ds != null && ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {

        //        int usid = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
        //        long Cents = Convert.ToInt64(ds.Tables[0].Rows[i]["cent"].ToString());
        //        if (Cents < 0)
        //        {

        //            long cent = Convert.ToInt64((-Cents) * (iTar * 0.001));
        //            new BCW.BLL.User().UpdateiGold(usid, cent, "竞猜返负（含半场单节）");
        //            //发内线
        //            string strLog = "根据你上期竞猜（含半场单节）排行榜上的亏损情况，系统自动返还了" + cent + "" + ub.Get("SiteBz") + "[url=/bbs/guess2/default.aspx]进入球彩竞猜[/url]";
        //            new BCW.BLL.Guest().Add(0, usid, new BCW.BLL.User().GetUsName(usid), strLog);

        //            builder.Append(usid + "|" + Cents + "|" +cent + "<br />");
        //        }
        //    }
        //}


        ////注册一万个ID
        //int k = 0;
        //for (int i = 0; i < 2000; i++)
        //{

        //    Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
        //    string mobile = "15107582999";

        //    //取得会员ID
        //    int maxId = BCW.User.Reg.GetRandId();
        //    if (maxId == 0 || maxId.ToString().Length != 5)// 
        //    {
        //        continue;
        //    }

        //    //加密用户密码
        //    string strPwd = Utils.MD5Str(new Rand().RandNum(10) + ra.Next(10000, 1000000));
        //    //取随机识别串
        //    string UsKey = new Rand().RandNum(10);


        //    //写入注册表
        //    BCW.Model.User model = new BCW.Model.User();
        //    model.ID = maxId;
        //    model.Mobile = mobile;
        //    model.UsName = "" + ub.Get("SiteText") + "网" + maxId + "";
        //    model.UsPwd = strPwd;
        //    model.UsKey = UsKey;
        //    model.Photo = "/Files/Avatar/image0.gif";
        //    model.Sex = ra.Next(1, 3);
        //    model.RegTime = DateTime.Parse("2013-" + ra.Next(1, 9) + "-" + ra.Next(1, 29) + "");
        //    model.RegIP = Utils.GetUsIP();
        //    model.EndTime = DateTime.Now;
        //    model.Birth = DateTime.Parse("" + ra.Next(1980, 1999) + "-" + ra.Next(1, 9) + "-" + ra.Next(1, 29) + "");
        //    model.Sign = "未设置签名";
        //    model.InviteNum = 0;
        //    model.IsVerify = 1;
        //    model.Email = "";
        //    new BCW.BLL.User().Add(model);
        //    //积分操作
        //    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_RegUser, maxId);

        //    //城市就随机分配
        //    string sCity = Utils.DelLastChar(BCW.User.City.GetCity(), "#");
        //    string[] sNum = sCity.Split("#".ToCharArray());
        //    Random rd = new Random();
        //    string City = sNum[rd.Next(sNum.Length)];

        //    long iScore = ra.Next(800, 2000);
        //    int Click = ra.Next(10, 300);
        //    int Leven = ra.Next(1, 9);
        //    int OnTime = ra.Next(500, 20000);
        //    BCW.Data.SqlHelper.ExecuteSql("update tb_user set City='" + City + "',iScore=" + iScore + ",Click=" + Click + ",Leven=" + Leven + ",OnTime=" + OnTime + " where id=" + maxId + "");
        //    k++;
        //}

        //builder.Append("成功注册" + k + "个");

    }
}