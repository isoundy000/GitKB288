using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.IO;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using BCW.Common;
using TPR2.Common;

public partial class bbs_guess2_footCollec : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/guess2.xml";
    protected void Page_Load(object sender, EventArgs e)
    {
        string act = Utils.GetRequest("act", "get", 1, "", "1");
        switch (act)
        {
            case "1":
                GetFoot();
                break;
            case "2":
                GetFootdx();
                break;
            case "3":
                GetFootbz();
                break;
            default:
                GetFoot();
                break;
        }
    }

    public void GetFoot()
    {
        int recordCount = 0;
        IList<TPR2.Model.guess.BaList> listFoot = new TPR2.Collec.Foot().GetFoot(out recordCount);
        if (listFoot != null)
        {
            if (listFoot.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listFoot)
                {

                    //builder.AppendFormat("赛事ID:{0}<br />", n.p_id);
                    //builder.AppendFormat("赛事名称:{0}<br />", n.p_title);
                    //builder.AppendFormat("赛事时间:{0}<br />", n.p_TPRtime);
                    //builder.AppendFormat("主队名称:{0}<br />", n.p_one);
                    //builder.AppendFormat("客队名称:{0}<br />", n.p_two);
                    //builder.AppendFormat("盘口类型:{0}<br />", n.p_pn);
                    //builder.AppendFormat("让球盘口:{0}<br />", n.p_pk);
                    //builder.AppendFormat("主队赔率:{0}<br />", n.p_one_lu);
                    //builder.AppendFormat("客队赔率:{0}<br />", n.p_two_lu);
                    //builder.Append("<br />----------<br />");

                    //写入数据
                    if (Convert.ToDateTime(n.p_TPRtime) <= DateTime.Now.AddHours(Convert.ToDouble(ub.GetSub("SiteJcTime", xmlPath))))
                    {
                        if (ub.GetSub("Sitezqstat", xmlPath).IndexOf(n.p_title) != -1)
                        {
                            if (n.p_one_lu != 0)
                            {
                                TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                                model.p_id = n.p_id;
                                model.p_title = n.p_title;
                                model.p_type = 1;
                                model.p_one = n.p_one;
                                model.p_two = n.p_two;
                                if (n.p_pn == 1)
                                {
                                    model.p_one_lu = n.p_one_lu + 1;
                                    model.p_two_lu = n.p_two_lu + 1;
                                }
                                else
                                {
                                    model.p_one_lu = n.p_two_lu + 1;
                                    model.p_two_lu = n.p_one_lu + 1;
                                }
                                //算得最终的受让结果
                                if (Convert.ToString(n.p_pk).IndexOf("-") != -1)
                                {
                                    model.p_pk = GCK.getPkNum(Convert.ToString(n.p_pk).Replace("-", ""));
                                    model.p_pn = 2;
                                }
                                else
                                {
                                    model.p_pk = GCK.getPkNum(Convert.ToString(n.p_pk));
                                    model.p_pn = n.p_pn;
                                }

                                model.p_addtime = DateTime.Now;
                                model.p_TPRtime = n.p_TPRtime;

                                TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                                if (bll.ExistsByp_id(Convert.ToInt32(n.p_id)))
                                {
                                    bll.FootUpdate(model);
                                }
                                else
                                {
                                    //是否先隐藏
                                    if (ub.GetSub("SiteIsyc", xmlPath) == "1")
                                    {
                                        model.p_del = 1;
                                    }
                                    else
                                    {
                                        model.p_del = 0;
                                    }
                                    bll.FootAdd(model);
                                }
                            }
                        }
                    }
                }
                binddata();
            }
        }
    }

    public void GetFootdx()
    {
        int recordCount = 0;
        IList<TPR2.Model.guess.BaList> listFootdx = new TPR2.Collec.Foot().GetFootdx(out recordCount);
        if (listFootdx != null)
        {
            if (listFootdx.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listFootdx)
                {

                    //builder.AppendFormat("赛事ID:{0}<br />", n.p_id);
                    //builder.AppendFormat("赛事时间:{0}<br />", n.p_TPRtime);
                    //builder.AppendFormat("大小球盘口:{0}<br />", n.p_dx_pk);
                    //builder.AppendFormat("大球赔率:{0}<br />", n.p_big_lu);
                    //builder.AppendFormat("小球赔率:{0}<br />", n.p_small_lu);
                    //builder.Append("<br />----------<br />");
                    //写入数据
                    if (Convert.ToDateTime(n.p_TPRtime) <= DateTime.Now.AddHours(Convert.ToDouble(ub.GetSub("SiteJcTime", xmlPath))))
                    {
                        if (n.p_big_lu != 0)
                        {
                            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                            model.p_id = n.p_id;
                            model.p_dx_pk = GCK.getDxPkNum(Convert.ToString(n.p_dx_pk));
                            model.p_big_lu = n.p_big_lu + 1;
                            model.p_small_lu = n.p_small_lu + 1;

                            TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                            if (bll.ExistsByp_id(Convert.ToInt32(n.p_id)) && !bll.ExistsDX(Convert.ToInt32(n.p_id)))
                            {
                                bll.FootdxUpdate(model);
                            }
                        }
                    }
                }
                binddata();
            }
        }
    }

    public void GetFootbz()
    {
        int recordCount = 0;
        IList<TPR2.Model.guess.BaList> listFootbz = new TPR2.Collec.Foot().GetFootbz(out recordCount);
        if (listFootbz != null)
        {
            if (listFootbz.Count > 0)
            {
                foreach (TPR2.Model.guess.BaList n in listFootbz)
                {

                    //builder.AppendFormat("赛事ID:{0}<br />", n.p_id);
                    //builder.AppendFormat("赛事时间:{0}<br />", n.p_TPRtime);
                    //builder.AppendFormat("标准主胜:{0}<br />", n.p_bzs_lu);
                    //builder.AppendFormat("标准平手:{0}<br />", n.p_bzp_lu);
                    //builder.AppendFormat("标准客胜:{0}<br />", n.p_bzx_lu);
                    //builder.Append("<br />----------<br />");
                    //写入数据
                    if (Convert.ToDateTime(n.p_TPRtime) <= DateTime.Now.AddHours(Convert.ToDouble(ub.GetSub("SiteJcTime", xmlPath))))
                    {
                        if (n.p_bzs_lu != 0)
                        {
                            TPR2.Model.guess.BaList model = new TPR2.Model.guess.BaList();
                            model.p_id = n.p_id;
                            model.p_bzs_lu = n.p_bzs_lu;
                            model.p_bzp_lu = n.p_bzp_lu;
                            model.p_bzx_lu = n.p_bzx_lu;

                            TPR2.BLL.guess.BaList bll = new TPR2.BLL.guess.BaList();
                            if (bll.ExistsByp_id(Convert.ToInt32(n.p_id)) && !bll.ExistsBZ(Convert.ToInt32(n.p_id)))
                            {
                                bll.FootbzUpdate(model);
                            }
                        }
                    }
                }
                binddata();
            }
        }
    }

    private void binddata()
    {
        FileStream fs = new FileStream(Server.MapPath("/Files/sys/loading.gif"), FileMode.Open, FileAccess.Read);
        byte[] mydata = new byte[fs.Length];
        int Length = Convert.ToInt32(fs.Length);
        fs.Read(mydata, 0, Length);
        fs.Close();
        Response.Clear();
        Response.ContentType = "image/gif";
        Response.OutputStream.Write(mydata, 0, Length);
        Response.End();
    }
}
