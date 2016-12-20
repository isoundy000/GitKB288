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
using System.Text.RegularExpressions;
using System.Threading;

public partial class bbs_game_jqcRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/jqc.xml";
    protected string GameName = ub.GetSub("GameName", "/Controls/jqc.xml");//游戏名字

    protected void Page_Load(object sender, EventArgs e)
    {
        //防止缓存
        Response.Buffer = true;
        Response.ExpiresAbsolute = DateTime.Now.AddSeconds(-1);
        Response.Expires = 0;
        Response.CacheControl = "no-cache";

        if (ub.GetSub("IsBot", xmlPath) != "1")
        {
            Response.Write("close1");
        }
        else
        {
            OpenRobot();//打开机器人
        }
    }

    //开启机器人
    private void OpenRobot()
    {
        //进行自动下注
        int hour = DateTime.Now.Hour;
        if (hour > 23 || hour < 9)
        {
            Response.Write("close1");
        }
        else
        {
            string where1 = "where phase!='' ORDER BY phase DESC";
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where1);//最后一期
            if (model.phase != 0)
            {
                Play_jqcRobot();
            }
            else
            {
                Response.Write("error1");
            }
        }
    }

    //机器人购买
    private void Play_jqcRobot()
    {
        int meid = GetUsID();//得到随机的UsID
        if (!new BCW.BLL.User().ExistsID(meid))
        {
            Response.Write("随机机器人ID不存在.error1<br/>");
            Response.End();
        }
        if (meid > 0)
        {
            //随机延时
            System.Threading.Thread.Sleep(R(1, 11) * 1000);
            int beilv = GetPayCent();//得到随机投注的倍率
            int buycou = Convert.ToInt32(ub.GetSub("ROBOTBUY", xmlPath));//xml限定每个机器人购买次数
            int zhuPrice = Convert.ToInt32(ub.GetSub("zhuPrice", xmlPath));//每注酷币
            long Price = zhuPrice * beilv;
            //随机得到投注号码
            string votenum = string.Empty;
            int[] a = new int[] { 0, 1, 2, 3 };
            string s = string.Empty;
            Random ran = new Random();
            for (int j = 0; j < 8; j++)
            {
                s = a[ran.Next(0, 4)].ToString();
                if (j == 7)
                {
                    votenum += s;
                }
                else
                {
                    votenum += s + "#";
                }
            }

            string where1 = "where phase!='' and Sale_Start < getdate() and  Sale_End > getdate() order by newid()";//查询数据库可以下注的随机一期
            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.BLL.JQC_Internet().GetJQC_Internet_model(where1);
            if (model.phase > 0)
            {
                BCW.JQC.Model.JQC_Bet modelBuy = new BCW.JQC.Model.JQC_Bet();
                //计数出该机器人投注的次数是否大于xml限定次数
                int count = new BCW.JQC.BLL.JQC_Bet().GetRecordCount(" UsID=" + meid + " and Lottery_issue='" + model.phase + "'");
                if ((count < buycou) || (buycou == 0))
                {
                    modelBuy.Lottery_issue = model.phase;
                    modelBuy.UsID = meid;
                    modelBuy.Input_Time = DateTime.Now;
                    modelBuy.State = 0;
                    modelBuy.isRobot = 1;
                    modelBuy.Zhu = 1;
                    modelBuy.Zhu_money = zhuPrice;
                    modelBuy.GetMoney = 0;
                    modelBuy.VoteRate = beilv;
                    modelBuy.PutGold = Price;
                    modelBuy.VoteNum = votenum;

                    long gold = new BCW.BLL.User().GetGold(meid);
                    long prices = Price;
                    string mename = new BCW.BLL.User().GetUsName(meid);

                    if (gold < prices)
                    {
                        //更新消费记录
                        BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                        modelx.BbTag = 3;
                        modelx.Types = 0;
                        modelx.PUrl = Utils.getPageUrl();//操作的文件名
                        modelx.UsId = meid;
                        modelx.UsName = mename;
                        modelx.AcGold = prices;
                        modelx.AfterGold = gold + prices;//更新后的币数
                        modelx.AcText = "系统进球彩机器人自动操作";
                        modelx.AddTime = DateTime.Now;
                        new BCW.BLL.Goldlog().Add(modelx);
                        BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + prices + ") where id=" + meid + "");
                    }

                    long xPrices = Utils.ParseInt64(ub.GetSub("BigPrice", xmlPath));
                    if (xPrices > 0)
                    {
                        long oPrices = 0;
                        DataSet ds = new BCW.JQC.BLL.JQC_Bet().GetList("PutGold", "UsID=" + meid + " and Lottery_issue='" + model.phase + "'");
                        DataTable dt = ds.Tables[0];
                        foreach (DataRow dr in dt.Rows)
                        {
                            int drs = int.Parse(dr[0].ToString());
                            oPrices = oPrices + drs;
                        }
                        if (oPrices + prices > xPrices)
                        {
                            Response.End();
                        }
                    }
                    int id = new BCW.JQC.BLL.JQC_Bet().Add(modelBuy);
                    new BCW.BLL.User().UpdateiGold(meid, mename, -prices, "进球彩第" + model.phase + "期投注消费-标识ID" + id + "");

                    //更新奖池
                    BCW.JQC.Model.JQC_Jackpot jack = new BCW.JQC.Model.JQC_Jackpot();
                    jack.UsID = meid;
                    jack.InPrize = Price;
                    jack.OutPrize = 0;
                    //jack.Jackpot = Price + new BCW.JQC.BLL.JQC_Jackpot().GetGold();//实时奖池
                    jack.Jackpot = Price + new BCW.JQC.BLL.JQC_Jackpot().GetGold_phase(model.phase);
                    jack.AddTime = DateTime.Now;
                    jack.phase = model.phase;
                    jack.type = 0;//玩家下注
                    jack.BetID = id;
                    new BCW.JQC.BLL.JQC_Jackpot().Add(jack);
                    new BCW.BLL.User().UpdateTime(meid, 5);//更新会员在线时长
                                                           //动态
                    string wText = "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在[url=/bbs/game/jqc.aspx]" + GameName + "[/url]第" + model.phase + "期下注" + Price + "" + ub.Get("SiteBz") + "";
                    new BCW.BLL.Action().Add(1014, id, meid, "", wText);
                }
            }
            Response.Write("ok1");
        }
        else
            Response.Write("error1.机器人为空.");
    }

    //随机得到出动的ID
    private int GetUsID()
    {
        int UsID = 0;
        string PlayUsID = ub.GetSub("ROBOTID", "/Controls/jqc.xml");
        if (PlayUsID.Trim() != "")
        {
            string[] sNum = Regex.Split(PlayUsID.Trim(), "#");
            Random rd = new Random();
            try
            {
                UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
            }
            catch
            {
                UsID = Convert.ToInt32(sNum[rd.Next(sNum.Length)]);
            }
        }
        return UsID;
    }

    //随机得到下注的倍率
    private int GetPayCent()
    {
        Random rac = new Random();
        int price = 1;
        string aa = ub.GetSub("ROBOTbeilv", xmlPath);
        if (aa.Trim() != "")
        {
            string[] sNum = Regex.Split(aa.Trim(), "#");
            int bb = sNum.Length;
            int cc = rac.Next(1, bb + 1);
            try
            {
                price = Convert.ToInt32(sNum[(cc - 1)]);
            }
            catch 
            {
                price = Convert.ToInt32(sNum[(cc - 1)]);
            }
        }
        else
        {
            price = 1;
        }
        return price;
    }


    protected int R(int x, int y)
    {
        Random ran = new Random();
        int RandKey = ran.Next(x, y);
        return RandKey;
    }


}
