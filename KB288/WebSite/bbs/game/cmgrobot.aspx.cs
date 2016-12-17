using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;

public partial class bbs_game_cmgRobot : System.Web.UI.Page
{
    protected string xmlPath = "/Controls/BYDR.xml";
    protected string GameName = ub.GetSub("bydrName", "/Controls/BYDR.xml");//游戏名字
    protected int time = Convert.ToInt32(ub.GetSub("bydrExpir1", "/Controls/BYDR.xml"));//防刷时间
    protected int ROBOTcishu = Convert.ToInt32(ub.GetSub("ROBOTcishu", "/Controls/BYDR.xml"));//捕鱼次数
    protected string zuid = ub.GetSub("zuid", "/Controls/BYDR.xml");//随机的机器人
    protected int zu = Convert.ToInt32(ub.GetSub("zu", "/Controls/BYDR.xml"));//每组机器人个数

    protected void Page_Load(object sender, EventArgs e)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

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
            Play_jqcRobot();
            Robot_case();//机器人兑奖
        }
        stopwatch.Stop();
        Response.Write("<font color=\"red\">" + "<br/><br/>刷新总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒</font><br/>");
    }

    //机器人进入场景
    private void Play_jqcRobot()
    {
        Response.Write("当前组的机器人：" + zuid + "<br/><br/>");
        if (zuid == "")
        {
            zuid = GetUsID();
            if (zuid == "")
            {
                Response.Write("随机机器人ID出错.error1<br/>");
                Response.Close();
            }
            ub xml = new ub();
            string xmlPath1 = "/Controls/BYDR.xml";
            Application.Remove(xmlPath1);//清缓存
            xml.ReloadSub(xmlPath1); //加载配置
            xml.dss["zuid"] = zuid.Replace("\r\n", "").Replace(" ", "");
            System.IO.File.WriteAllText(Server.MapPath(xmlPath1), xml.Post(xml.dss), System.Text.Encoding.UTF8);
        }
        //在zuid里面随机一个usid
        string[] sNum = Regex.Split(zuid.Replace("\r\n", "").Replace(" ", ""), "#");
        Random rd = new Random();
        int oo = rd.Next(sNum.Length);
        if (oo == 0 || oo == sNum.Length - 1)
        {
            oo = 1;
        }
        int meid = int.Parse(sNum[oo]);//得到随机的UsID

        if (ROBOTcishu < 1)//捕鱼次数
        {
            Response.End();
        }
        //判断该id是否有未捕完的场景
        bool sss = new BCW.bydr.BLL.Cmg_Top().ExistsusID1(meid);
        if (sss)
        {
            //查询是哪个场景
            DataSet cj = new BCW.bydr.BLL.Cmg_Top().GetList("TOP(1)*", "usID=" + meid + " AND DcolletGold!=10 ORDER BY ID DESC");
            string randgoldnum = "0";
            int _changjing = 0;
            if ((cj.Tables[0].Rows[0]["changj"]).ToString() == "山涧小溪")
            {
                _changjing = 1;
            }
            else if ((cj.Tables[0].Rows[0]["changj"]).ToString() == "梦幻沙河")
            {
                _changjing = 2;
            }
            else if ((cj.Tables[0].Rows[0]["changj"]).ToString() == "云阳大泽")
            {
                _changjing = 3;
            }
            else if ((cj.Tables[0].Rows[0]["changj"]).ToString() == "梦迹蓝礁")
            {
                _changjing = 4;
            }
            else if ((cj.Tables[0].Rows[0]["changj"]).ToString() == "沉没古迹")
            {
                _changjing = 5;
            }
            else
            {
                _changjing = 6;
            }
            BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(_changjing);

            #region 捕鱼开始
            string[] numgoldf = new string[] { };
            //读取加入的10条数据
            //读取用户id最后一条数据
            DataSet rows11 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID ", "usID=" + meid + " order by Time desc");
            int id11 = Convert.ToInt32(rows11.Tables[0].Rows[0][0]);
            BCW.bydr.Model.Cmg_Top modle11 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
            string[] randgoldnum_1 = modle11.randgoldnum.Split(',');
            //鱼id
            int[] daojunum1 = new int[] { 15, 19, 22, 23, 24, 24, 37, 38, 59, 67, 73, 79, 84, 85, 92, 13 };//最低价值鱼类
            int[] daojunum2 = new int[] { 16, 18, 29, 32, 34, 40, 48, 49, 51, 54, 56, 60, 63, 68, 70, 71, 75, 76, 77, 81, 72, 82, 91, 92, 93, 94, 95 };//中间价值鱼类
            int[] daojunum3 = new int[] { 100, 101, 102, 97, 98, 99, 96, 14, 17, 20, 25, 21, 27, 28, 30, 31, 33, 36, 39, 42, 43, 44, 45, 46, 47, 50, 52, 53, 57, 58, 64, 65, 66, 69, 74, 78, 80, 83, 86, 87, 88, 89, 90 };//最高价值鱼类
            int randgold = 0;
            try
            {
                randgold = Convert.ToInt32(randgoldnum_1[modle11.randnum]);
            }
            catch
            {
                randgold = 1;
            }

            #region 捕鱼操作
            for (int xx = 0; xx < ROBOTcishu; xx++)
            {
                BCW.bydr.Model.Cmg_Top modle22 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                string[] randaoju = modle22.randdaoju.Split(',');
                string[] randyu = modle22.randyuID.Split(',');
                string[] numgoldo = new string[] { };
                switch (_changjing)
                {
                    #region 场景1操作
                    case 1:
                        {
                            string[] numgold0 = new string[] { };
                            //判断随机给定的场景价格包
                            if (modle22.McolletGold == 0)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                                numgold0 = modeldaoju0.Changjing.Split(',');
                                numgoldo = numgold0;
                            }
                            else if (modle22.McolletGold == 1)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(301);
                                string[] numgold1 = modeldaoju1.Changjing.Split(',');
                                numgoldo = numgold1;
                            }
                            else if (modle22.McolletGold == 2)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju2 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(302);
                                string[] numgold2 = modeldaoju2.Changjing.Split(',');
                                numgoldo = numgold2;
                            }
                            else if (modle22.McolletGold == 3)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju3 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(303);
                                string[] numgold3 = modeldaoju3.Changjing.Split(',');
                                numgoldo = numgold3;
                            }
                            else if (modle22.McolletGold == 4)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju4 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(304);
                                string[] numgold4 = modeldaoju4.Changjing.Split(',');
                                numgoldo = numgold4;
                            }

                            //读取场景的价格包
                            if (randgold == 0)
                            {
                                #region 获得超级大奖的处理,跳出循环
                                //获得超级大奖的处理
                                int goldnumder = Convert.ToInt32(numgoldo[0]);
                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 200);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                }
                                #endregion
                            }
                            else
                            {
                                #region 普通奖品派发
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    #region 更新鱼的价格
                                    string randdaoju = string.Empty;
                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldo[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 200;
                                    }
                                    if (goldnum == 0)
                                    {
                                        goldnum = 201;
                                    }
                                    int a = 0;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);//更新每条鱼的价格
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    #endregion
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        #region 判断第9次加数据
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                      
                                        }
                                        #endregion

                                        #region 根据价格取不同的鱼类    
                                        string rand = "";
                                        string rand1 = "";
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 20)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 20 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 100)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        #endregion

                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");
                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                    }
                                }
                                #endregion
                            }
                        }; break;
                    #endregion

                    #region 场景2操作
                    case 2:
                        {
                            string[] numgold5 = new string[] { };
                            if (modle22.McolletGold == 5)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju5 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(305);
                                numgold5 = modeldaoju5.Changjing.Split(',');
                                numgoldf = numgold5;
                            }
                            else if (modle22.McolletGold == 6)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju6 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(306);
                                string[] numgold6 = modeldaoju6.Changjing.Split(',');
                                numgoldf = numgold6;
                            }
                            else if (modle22.McolletGold == 7)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju7 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(307);
                                string[] numgold7 = modeldaoju7.Changjing.Split(',');
                                numgoldf = numgold7;
                            }
                            else if (modle22.McolletGold == 8)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju8 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(308);
                                string[] numgold8 = modeldaoju8.Changjing.Split(',');
                                numgoldf = numgold8;
                            }
                            else if (modle22.McolletGold == 9)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju9 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(309);
                                string[] numgold9 = modeldaoju9.Changjing.Split(',');
                                numgoldf = numgold9;
                            }

                            if (randgold == 0)
                            {
                                #region 获得超级大奖的处理,跳出循环
                                int goldnumder = Convert.ToInt32(numgoldf[0]);
                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                 
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 1000);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                }
                                #endregion
                            }
                            else
                            {
                                #region 普通奖品派发
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    int a = 0;
                                    string randdaoju = string.Empty;
                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 1000;
                                    }
                                    if (goldnum == 0)
                                        goldnum = 1000;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid)); //更新排行榜总收集币
                                        }

                                        //根据价格取不同的鱼类
                                        string rand = "";
                                        string rand1 = "";
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 100)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 100 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 10000)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }

                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 

                                    }
                                }
                                #endregion
                            }

                        }; break;
                    #endregion

                    #region 场景3操作
                    case 3:
                        {
                            string[] numgold10 = new string[] { };
                            if (modle22.McolletGold == 10)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju10 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(310);
                                numgold10 = modeldaoju10.Changjing.Split(',');
                                numgoldf = numgold10;
                            }
                            else if (modle22.McolletGold == 11)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju11 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(311);
                                string[] numgold11 = modeldaoju11.Changjing.Split(',');
                                numgoldf = numgold11;
                            }
                            else if (modle22.McolletGold == 12)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju12 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(312);
                                string[] numgold12 = modeldaoju12.Changjing.Split(',');
                                numgoldf = numgold12;
                            }
                            else if (modle22.McolletGold == 13)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju13 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(313);
                                string[] numgold13 = modeldaoju13.Changjing.Split(',');
                                numgoldf = numgold13;
                            }
                            else if (modle22.McolletGold == 14)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju14 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(314);
                                string[] numgold14 = modeldaoju14.Changjing.Split(',');
                                numgoldf = numgold14;
                            }

                            if (randgold == 0)
                            {
                                int goldnumder = Convert.ToInt32(numgoldf[0]);

                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 5000);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                }
                            }
                            else
                            {
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    int a = 0;
                                    string randdaoju = string.Empty;
                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 5000;
                                    }
                                    if (goldnum == 0)
                                        goldnum = 5000;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        }
                                        string rand = "";
                                        string rand1 = "";
                                        //根据价格取不同的鱼类
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 200)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }


                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                    }
                                }
                            }
                        }; break;
                    #endregion

                    #region 场景4操作
                    case 4:
                        {
                            string[] numgold15 = new string[] { };
                            if (modle22.McolletGold == 15)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju15 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(315);
                                numgold15 = modeldaoju15.Changjing.Split(',');
                                numgoldf = numgold15;
                            }
                            else if (modle22.McolletGold == 16)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju16 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(316);
                                string[] numgold16 = modeldaoju16.Changjing.Split(',');
                                numgoldf = numgold16;
                            }
                            else if (modle22.McolletGold == 17)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju17 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(317);
                                string[] numgold17 = modeldaoju17.Changjing.Split(',');
                                numgoldf = numgold17;
                            }
                            else if (modle22.McolletGold == 18)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju18 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(318);
                                string[] numgold18 = modeldaoju18.Changjing.Split(',');
                                numgoldf = numgold18;
                            }
                            else if (modle22.McolletGold == 19)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju19 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(319);
                                string[] numgold19 = modeldaoju19.Changjing.Split(',');
                                numgoldf = numgold19;
                            }

                            if (randgold == 0)
                            {
                                int goldnumder = Convert.ToInt32(numgoldf[0]);

                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                            
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 10000);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                }
                            }
                            else
                            {
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    int a = 0;
                                    string randdaoju = string.Empty;
                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 10000;
                                    }
                                    if (goldnum == 0)
                                        goldnum = 10000;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        }
                                        string rand = "";
                                        string rand1 = "";
                                        //根据价格取不同的鱼类
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 200)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }

                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                    }
                                }
                            }
                        }; break;
                    #endregion

                    #region 场景5操作
                    case 5:
                        {
                            string[] numgold20 = new string[] { };
                            if (modle22.McolletGold == 20)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju20 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(320);
                                numgold20 = modeldaoju20.Changjing.Split(',');
                                numgoldf = numgold20;
                            }
                            else if (modle22.McolletGold == 21)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju21 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(321);
                                string[] numgold21 = modeldaoju21.Changjing.Split(',');
                                numgoldf = numgold21;
                            }
                            else if (modle22.McolletGold == 22)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju22 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(322);
                                string[] numgold22 = modeldaoju22.Changjing.Split(',');
                                numgoldf = numgold22;
                            }
                            else if (modle22.McolletGold == 23)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju23 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(323);
                                string[] numgold23 = modeldaoju23.Changjing.Split(',');
                                numgoldf = numgold23;
                            }

                            else if (modle22.McolletGold == 24)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju24 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(324);
                                string[] numgold24 = modeldaoju24.Changjing.Split(',');
                                numgoldf = numgold24;
                            }
                            if (randgold == 0)
                            {
                                int goldnumder = Convert.ToInt32(numgoldf[0]);

                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 20000);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                }
                            }
                            else
                            {
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    int a = 0;
                                    string randdaoju = string.Empty;
                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 20000;
                                    }
                                    if (goldnum == 0)
                                        goldnum = 20000;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                  
                                        }
                                        string rand = "";
                                        string rand1 = "";
                                        //根据价格取不同的鱼类
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 1000)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }

                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                    }
                                }
                            }
                        }; break;
                    #endregion

                    #region 场景6操作
                    case 6:
                        {
                            string[] numgold25 = new string[] { };
                            if (modle22.McolletGold == 25)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju25 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(325);
                                numgold25 = modeldaoju25.Changjing.Split(',');
                                numgoldf = numgold25;
                            }
                            else if (modle22.McolletGold == 26)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju26 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(326);
                                string[] numgold26 = modeldaoju26.Changjing.Split(',');
                                numgoldf = numgold26;
                            }
                            else if (modle22.McolletGold == 27)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju27 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(327);
                                string[] numgold27 = modeldaoju27.Changjing.Split(',');
                                numgoldf = numgold27;
                            }
                            else if (modle22.McolletGold == 28)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju28 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(328);
                                string[] numgold28 = modeldaoju28.Changjing.Split(',');
                                numgoldf = numgold28;
                            }
                            else if (modle22.McolletGold == 29)
                            {
                                BCW.bydr.Model.CmgDaoju modeldaoju29 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(329);
                                string[] numgold29 = modeldaoju29.Changjing.Split(',');
                                numgoldf = numgold29;
                            }

                            if (randgold == 0)
                            {
                                int goldnumder = Convert.ToInt32(numgoldf[0]);

                                if (modle22.DcolletGold == 0)
                                {
                                    BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                    m1.AllGold = 0;
                                    m1.AcolletGold = goldnumder;
                                    m1.changj = modle22.Changj;
                                    m1.coID = "";
                                    m1.cxid = 1;
                                    m1.random = 0;
                                    m1.Stime = DateTime.Now;
                                    m1.Signtime = DateTime.Now;
                                    m1.usID = meid;
                                    m1.Vit = 0;
                                    new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                    new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                    new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 50000);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                }
                            }
                            else
                            {
                                if (modle22.randdaoju == "")//更新随机鱼的价格
                                {
                                    int a = 0;
                                    string randdaoju = string.Empty;
                                    Random ran = new Random();

                                    int goldnum = 0;
                                    try
                                    {
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum[modle22.randnum/*根据次数读取鱼的价格*/])]);
                                    }
                                    catch
                                    {
                                        goldnum = 50000;
                                    }
                                    if (goldnum == 0)
                                        goldnum = 50000;
                                    while (a < 10)
                                    {
                                        int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                        randdaoju += Convert.ToString(r);
                                        randdaoju += ",";
                                        a++;
                                    }
                                    new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                    new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                    new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                }
                                else
                                {
                                    if (modle22.DcolletGold < 10)
                                    {
                                        if (modle22.DcolletGold == 9)
                                        {
                                            new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                        }

                                        string rand = "";
                                        string rand1 = "";
                                        //根据价格取不同的鱼类
                                        if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum1.Length - 1);
                                            rand = Convert.ToString(daojunum1[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 2000)
                                        {
                                            int r = 0;
                                            r = R(0, daojunum2.Length - 1);
                                            rand = Convert.ToString(daojunum2[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }
                                        else
                                        {
                                            int r = 0;
                                            r = R(0, daojunum3.Length - 1);
                                            rand = Convert.ToString(daojunum3[r]);
                                            rand1 += modle22.randyuID + rand + ",";
                                            new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                        }

                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                        BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                        string[] randyu33 = modle33.randyuID.Split(',');
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                        m1.AllGold = 0;
                                        m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 0;
                                        m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                        new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                    }
                                }
                            }
                        }; break;
                    default:
                        break;
                        #endregion
                }
                System.Threading.Thread.Sleep(R(1, 4) * 1000);//睡眠
            }
            Response.Write("<br/>" + meid + "在场景" + _changjing + "继续捕鱼完成ok————当前时间:" + DateTime.Now + "<br/><br/>");
            #endregion
            #endregion

        }
        else
        {
            new BCW.BLL.User().UpdateTime(meid, 5);//更新会员在线时长
            int changjing = Get_changjing();//随机得到进入的场景
            int buycou = Convert.ToInt32(ub.GetSub("ROBOTBUY", xmlPath));//xml限定每个机器人每天购买次数
            long prices = 0;//花费
            long gold = new BCW.BLL.User().GetGold(meid);//查询usid的金币

            bool meid6 = new BCW.bydr.BLL.Cmg_Top().ExistsusID(meid);
            if (meid6 == false)
                addbuyub(meid);//如不存在,则增加一条数据

            //查询今天是否够捕鱼次数
            if (new BCW.bydr.BLL.Cmg_Top().GetCmgcount1(meid) <= buycou || (buycou == 0))
            {
                BCW.bydr.Model.CmgDaoju model0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(changjing);//根据随机数查询场景进入的花费
                prices = model0.changj2;

                //如
                if (new BCW.bydr.BLL.CmgToplist().Getvit(meid) < 0)
                {
                    #region 果没有体力,自动购买体力再下注
                    new BCW.bydr.BLL.CmgToplist().Updatevit(meid, 100);
                    long gold2 = (Convert.ToInt32(ub.GetSub("bydrbuyuTi", xmlPath)) * 100);
                    //更新购买体力的消费记录
                    if (gold < gold2)
                    {
                        BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                        modelx.BbTag = 3;
                        modelx.Types = 0;
                        modelx.PUrl = Utils.getPageUrl();//操作的文件名
                        modelx.UsId = meid;
                        modelx.UsName = new BCW.BLL.User().GetUsName(meid);
                        modelx.AcGold = gold2;
                        modelx.AfterGold = gold + gold2;//更新后的币数
                        modelx.AcText = "系统捕鱼机器人购买体力自动操作";
                        modelx.AddTime = DateTime.Now;
                        new BCW.BLL.Goldlog().Add(modelx);
                        BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + gold2 + ") where id=" + meid + "");
                    }
                    new BCW.BLL.User().UpdateiGold(meid, -gold2, "捕鱼购买体力花费。");
                    #endregion
                }

                if (gold < prices)
                {
                    #region 更新入场费的消费记录
                    BCW.Model.Goldlog modelx = new BCW.Model.Goldlog();
                    modelx.BbTag = 3;
                    modelx.Types = 0;
                    modelx.PUrl = Utils.getPageUrl();//操作的文件名
                    modelx.UsId = meid;
                    modelx.UsName = new BCW.BLL.User().GetUsName(meid);
                    modelx.AcGold = prices;
                    modelx.AfterGold = gold + prices;//更新后的币数
                    modelx.AcText = "系统捕鱼机器人自动操作";
                    modelx.AddTime = DateTime.Now;
                    new BCW.BLL.Goldlog().Add(modelx);
                    BCW.Data.SqlHelper.ExecuteSql("Update tb_User set iGold=iGold+(" + prices + ") where id=" + meid + "");
                    #endregion
                }

                #region 捕鱼入场
                int k = 0;//控制价格变动！！
                BCW.bydr.Model.Cmg_Top m2 = new BCW.bydr.Model.Cmg_Top();
                //读取随机次数
                BCW.bydr.Model.CmgDaoju modeldaoju = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                int randnum = 0;//每个相同场景增加1到30次一轮回
                int i = 0;
                string randgoldnum = "0";
                int biaoid = 0;//兑奖标识
                switch (changjing)
                {
                    #region 场景1
                    case 1:
                        {
                            k = R(0, 5);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(1);
                            DataSet changj1 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang1.Changjing + "' order by Time desc ");
                            int changid1 = 1;
                            try
                            {
                                changid1 = Convert.ToInt32(changj1.Tables[0].Rows[0][0]);
                            }
                            catch (Exception)
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj1 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid1);

                            m2.Changj = model0.Changjing;
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            BCW.bydr.Model.CmgDaoju modeldaoju111 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                            string[] randgoldnum1 = modelchangj1.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang1.Changjing && randgoldnum1.Length - 1 == modeldaoju111.changj2))
                            {
                                if ((model0.Changjing == modelchang1.Changjing && modelchangj1.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj1.randnum + 1;
                                    m2.randgoldnum = modelchangj1.randgoldnum;
                                    k = Convert.ToInt32(modelchangj1.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j1 = 0;
                            string randte1 = string.Empty;
                            while (j1 < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j1];
                                randte1 += Convert.ToString(r);
                                randte1 += ",";
                                j1++;
                            }
                            m2.randten = randte1;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                    #endregion

                    #region 场景2
                    case 2:
                        {
                            k = R(5, 10);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang2 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(2);
                            DataSet changj2 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang2.Changjing + "' order by Time desc ");
                            int changid2 = 1;
                            try
                            {
                                changid2 = Convert.ToInt32(changj2.Tables[0].Rows[0][0]);
                            }
                            catch (Exception)
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj2 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid2);
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            string[] randgoldnum2 = modelchangj2.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang2.Changjing && randgoldnum2.Length - 1 == modeldaoju.changj2))
                            {
                                //每个场合循环判断
                                if ((model0.Changjing == modelchang2.Changjing && modelchangj2.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj2.randnum + 1;
                                    m2.randgoldnum = modelchangj2.randgoldnum;
                                    k = Convert.ToInt32(modelchangj2.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j = 0;
                            string randte = string.Empty;
                            while (j < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j];
                                randte += Convert.ToString(r);
                                randte += ",";
                                j++;
                            }
                            m2.randten = randte;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                    #endregion

                    #region 场景3
                    case 3:
                        {
                            k = R(10, 15);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang3 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(3);
                            DataSet changj3 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang3.Changjing + "' order by Time desc ");
                            int changid3 = 1;
                            try
                            {
                                changid3 = Convert.ToInt32(changj3.Tables[0].Rows[0][0]);
                            }
                            catch (Exception)
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj3 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid3);
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            string[] randgoldnum3 = modelchangj3.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang3.Changjing && randgoldnum3.Length - 1 == modeldaoju.changj2))
                            {
                                if ((model0.Changjing == modelchang3.Changjing && modelchangj3.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj3.randnum + 1;
                                    m2.randgoldnum = modelchangj3.randgoldnum;
                                    k = Convert.ToInt32(modelchangj3.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j = 0;
                            string randte = string.Empty;
                            while (j < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j];
                                randte += Convert.ToString(r);
                                randte += ",";
                                j++;
                            }
                            m2.randten = randte;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                    #endregion

                    #region 场景4
                    case 4:
                        {
                            k = R(15, 20);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang4 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(4);
                            DataSet changj4 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang4.Changjing + "' order by Time desc ");
                            int changid4 = 1;
                            try
                            {
                                changid4 = Convert.ToInt32(changj4.Tables[0].Rows[0][0]);
                            }
                            catch (Exception)
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj4 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid4);
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            string[] randgoldnum4 = modelchangj4.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang4.Changjing && randgoldnum4.Length - 1 == modeldaoju.changj2))
                            {
                                if ((model0.Changjing == modelchang4.Changjing && modelchangj4.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj4.randnum + 1;
                                    m2.randgoldnum = modelchangj4.randgoldnum;
                                    k = Convert.ToInt32(modelchangj4.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j = 0;
                            string randte = string.Empty;
                            while (j < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j];
                                randte += Convert.ToString(r);
                                randte += ",";
                                j++;
                            }
                            m2.randten = randte;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                    #endregion

                    #region 场景5
                    case 5:
                        {
                            k = R(20, 25);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang5 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(5);
                            DataSet changj5 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang5.Changjing + "' order by Time desc ");
                            int changid5 = 1;
                            try
                            {
                                changid5 = Convert.ToInt32(changj5.Tables[0].Rows[0][0]);
                            }
                            catch (Exception)
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj5 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid5);
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            string[] randgoldnum5 = modelchangj5.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang5.Changjing && randgoldnum5.Length - 1 == modeldaoju.changj2))
                            {
                                if ((model0.Changjing == modelchang5.Changjing && modelchangj5.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj5.randnum + 1;
                                    m2.randgoldnum = modelchangj5.randgoldnum;
                                    k = Convert.ToInt32(modelchangj5.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j = 0;
                            string randte = string.Empty;
                            while (j < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j];
                                randte += Convert.ToString(r);
                                randte += ",";
                                j++;
                            }
                            m2.randten = randte;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                    #endregion

                    #region 场景6
                    case 6:
                        {
                            k = R(25, 30);//每个场景价格均有一个唯一对应的值！！！
                            BCW.bydr.Model.CmgDaoju modelchang6 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(6);
                            DataSet changj6 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID", "Changj='" + modelchang6.Changjing + "' order by Time desc ");
                            int changid6 = 1;
                            try
                            {
                                changid6 = Convert.ToInt32(changj6.Tables[0].Rows[0][0]);
                            }
                            catch
                            {
                            }
                            BCW.bydr.Model.Cmg_Top modelchangj6 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(changid6);
                            m2.usID = meid;
                            m2.AllcolletGold = 0;
                            m2.Changj = model0.Changjing;
                            m2.ColletGold = 0;
                            m2.DcolletGold = 0;
                            m2.Time = DateTime.Now;
                            m2.YcolletGold = 0;//每个场景n次标识
                            m2.Bid = 1;
                            m2.jID = 1;
                            string[] randgoldnum6 = modelchangj6.randgoldnum.Split(',');
                            if ((model0.Changjing == modelchang6.Changjing && randgoldnum6.Length - 1 == modeldaoju.changj2))
                            {
                                if ((model0.Changjing == modelchang6.Changjing && modelchangj6.randnum > (Convert.ToInt32(modeldaoju.changj2) - 2)))
                                {
                                    randnum = 0;
                                    Random ran = new Random();
                                    while (i < modeldaoju.changj2)
                                    {
                                        randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                        randgoldnum += ",";
                                        i++;
                                    }
                                    m2.randgoldnum = randgoldnum;
                                }
                                else
                                {
                                    randnum = modelchangj6.randnum + 1;
                                    m2.randgoldnum = modelchangj6.randgoldnum;
                                    k = Convert.ToInt32(modelchangj6.McolletGold);
                                }
                            }
                            else
                            {
                                randnum = 0;
                                Random ran = new Random();
                                while (i < modeldaoju.changj2)
                                {
                                    randgoldnum += Convert.ToString(GetRandomNum(Convert.ToInt32(modeldaoju.changj2), 0, Convert.ToInt32(modeldaoju.changj2) - 1)[i]);
                                    randgoldnum += ",";
                                    i++;
                                }
                                m2.randgoldnum = randgoldnum;
                            }
                            m2.randnum = randnum;
                            m2.McolletGold = k;
                            m2.randdaoju = "";
                            m2.randyuID = "";

                            int j = 0;
                            string randte = string.Empty;
                            while (j < 10)
                            {
                                int r = GetRandomNum(10, 3, 25)[j];
                                randte += Convert.ToString(r);
                                randte += ",";
                                j++;
                            }
                            m2.randten = randte;
                            m2.updatetime = DateTime.Now;
                            m2.isrobot = 1;
                            //增加兑换表数据
                            biaoid = new BCW.bydr.BLL.Cmg_Top().Add(m2);
                        }
                        break;
                        #endregion
                }
                //体力值-1
                new BCW.bydr.BLL.CmgToplist().Updatevit1(meid, new BCW.bydr.BLL.CmgToplist().Getvit(meid) - 1);
                //等级+1    
                BCW.bydr.Model.CmgToplist model21 = new BCW.bydr.BLL.CmgToplist().GetCmgToplistusID(meid);
                new BCW.bydr.BLL.CmgToplist().Updatestype(meid, Convert.ToInt32(model21.stype) + 1);
                //更新防刷字段
                new BCW.bydr.BLL.CmgToplist().Updatesid(meid, 0);
                //更新消费信息
                new BCW.BLL.User().UpdateiGold(meid, new BCW.BLL.User().GetUsName(meid), -prices, "捕鱼入场费-标识ID:" + biaoid);
                Response.Write("机器人[" + meid + "]已入场！ok1————场景" + changjing + ",消费" + prices + "<br/><br/>");
                #endregion

                #region 捕鱼开始
                string[] numgoldf = new string[] { };
                //读取加入的10条数据
                //读取用户id最后一条数据
                DataSet rows11 = new BCW.bydr.BLL.Cmg_Top().GetList("Top 1 ID ", "usID=" + meid + " order by Time desc");
                int id11 = Convert.ToInt32(rows11.Tables[0].Rows[0][0]);
                BCW.bydr.Model.Cmg_Top modle11 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                string[] randgoldnum_1 = modle11.randgoldnum.Split(',');
                //鱼id
                int[] daojunum1 = new int[] { 15, 19, 22, 23, 24, 24, 37, 38, 59, 67, 73, 79, 84, 85, 92, 13 };//最低价值鱼类
                int[] daojunum2 = new int[] { 16, 18, 29, 32, 34, 40, 48, 49, 51, 54, 56, 60, 63, 68, 70, 71, 75, 76, 77, 81, 72, 82, 91, 92, 93, 94, 95 };//中间价值鱼类
                int[] daojunum3 = new int[] { 100, 101, 102, 97, 98, 99, 96, 14, 17, 20, 25, 21, 27, 28, 30, 31, 33, 36, 39, 42, 43, 44, 45, 46, 47, 50, 52, 53, 57, 58, 64, 65, 66, 69, 74, 78, 80, 83, 86, 87, 88, 89, 90 };//最高价值鱼类
                int randgold = 0;
                try
                {
                    randgold = Convert.ToInt32(randgoldnum_1[modle11.randnum]);
                }
                catch
                {
                    randgold = 1;
                }

                #region 捕鱼操作
                for (int xx = 0; xx < ROBOTcishu; xx++)
                {
                    BCW.bydr.Model.Cmg_Top modle22 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                    string[] randaoju = modle22.randdaoju.Split(',');
                    string[] randyu = modle22.randyuID.Split(',');
                    string[] numgoldo = new string[] { };
                    switch (changjing)
                    {
                        #region 场景1操作
                        case 1:
                            {
                                string[] numgold0 = new string[] { };
                                if (modle22.McolletGold == 0)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju0 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(300);
                                    numgold0 = modeldaoju0.Changjing.Split(',');
                                    numgoldo = numgold0;
                                }
                                else if (modle22.McolletGold == 1)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju1 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(301);
                                    string[] numgold1 = modeldaoju1.Changjing.Split(',');
                                    numgoldo = numgold1;
                                }
                                else if (modle22.McolletGold == 2)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju2 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(302);
                                    string[] numgold2 = modeldaoju2.Changjing.Split(',');
                                    numgoldo = numgold2;
                                }
                                else if (modle22.McolletGold == 3)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju3 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(303);
                                    string[] numgold3 = modeldaoju3.Changjing.Split(',');
                                    numgoldo = numgold3;
                                }
                                else if (modle22.McolletGold == 4)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju4 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(304);
                                    string[] numgold4 = modeldaoju4.Changjing.Split(',');
                                    numgoldo = numgold4;
                                }

                                //读取场景的价格包
                                if (randgold == 0)
                                {
                                    #region 获得超级大奖的处理,跳出循环
                                    //获得超级大奖的处理
                                    int goldnumder = Convert.ToInt32(numgoldo[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 200);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 普通奖品派发
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        #region 更新鱼的价格
                                        string randdaoju = string.Empty;

                                        int goldnum = 0;

                                        goldnum = Convert.ToInt32(numgoldo[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                        {
                                            goldnum = 201;
                                        }
                                        int a = 0;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格

                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);//更新每条鱼的价格
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                        #endregion
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            #region 判断第9次加数据
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                      
                                            }
                                            #endregion

                                            #region 根据价格取不同的鱼类    
                                            string rand = "";
                                            string rand1 = "";
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 20)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 20 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 100)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            #endregion

                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");
                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);
                                        }
                                    }
                                    #endregion
                                }
                            }; break;
                        #endregion

                        #region 场景2操作
                        case 2:
                            {
                                string[] numgold5 = new string[] { };
                                if (modle22.McolletGold == 5)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju5 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(305);
                                    numgold5 = modeldaoju5.Changjing.Split(',');
                                    numgoldf = numgold5;
                                }
                                else if (modle22.McolletGold == 6)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju6 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(306);
                                    string[] numgold6 = modeldaoju6.Changjing.Split(',');
                                    numgoldf = numgold6;
                                }
                                else if (modle22.McolletGold == 7)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju7 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(307);
                                    string[] numgold7 = modeldaoju7.Changjing.Split(',');
                                    numgoldf = numgold7;
                                }
                                else if (modle22.McolletGold == 8)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju8 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(308);
                                    string[] numgold8 = modeldaoju8.Changjing.Split(',');
                                    numgoldf = numgold8;
                                }
                                else if (modle22.McolletGold == 9)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju9 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(309);
                                    string[] numgold9 = modeldaoju9.Changjing.Split(',');
                                    numgoldf = numgold9;
                                }
                                if (randgold == 0)
                                {
                                    #region 获得超级大奖的处理,跳出循环

                                    int goldnumder = Convert.ToInt32(numgoldf[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                 
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 1000);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region 普通奖品派发
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        int a = 0;
                                        string randdaoju = string.Empty;
                                        int goldnum = 0;

                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                            goldnum = 1000;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid)); //更新排行榜总收集币
                                            }

                                            //根据价格取不同的鱼类
                                            string rand = "";
                                            string rand1 = "";
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 100)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 100 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 10000)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }

                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 

                                        }
                                    }
                                    #endregion
                                }

                            }; break;
                        #endregion

                        #region 场景3操作
                        case 3:
                            {
                                string[] numgold10 = new string[] { };
                                if (modle22.McolletGold == 10)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju10 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(310);
                                    numgold10 = modeldaoju10.Changjing.Split(',');
                                    numgoldf = numgold10;
                                }
                                else if (modle22.McolletGold == 11)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju11 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(311);
                                    string[] numgold11 = modeldaoju11.Changjing.Split(',');
                                    numgoldf = numgold11;
                                }
                                else if (modle22.McolletGold == 12)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju12 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(312);
                                    string[] numgold12 = modeldaoju12.Changjing.Split(',');
                                    numgoldf = numgold12;
                                }
                                else if (modle22.McolletGold == 13)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju13 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(313);
                                    string[] numgold13 = modeldaoju13.Changjing.Split(',');
                                    numgoldf = numgold13;
                                }
                                else if (modle22.McolletGold == 14)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju14 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(314);
                                    string[] numgold14 = modeldaoju14.Changjing.Split(',');
                                    numgoldf = numgold14;
                                }
                                if (randgold == 0)
                                {
                                    int goldnumder = Convert.ToInt32(numgoldf[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                   
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 5000);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                }
                                else
                                {
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        int a = 0;
                                        string randdaoju = string.Empty;

                                        int goldnum = 0;

                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                            goldnum = 5000;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                            }
                                            string rand = "";
                                            string rand1 = "";
                                            //根据价格取不同的鱼类
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 200)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);

                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }


                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                        }
                                    }
                                }
                            }; break;
                        #endregion

                        #region 场景4操作
                        case 4:
                            {
                                string[] numgold15 = new string[] { };
                                if (modle22.McolletGold == 15)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju15 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(315);
                                    numgold15 = modeldaoju15.Changjing.Split(',');
                                    numgoldf = numgold15;
                                }
                                else if (modle22.McolletGold == 16)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju16 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(316);
                                    string[] numgold16 = modeldaoju16.Changjing.Split(',');
                                    numgoldf = numgold16;
                                }
                                else if (modle22.McolletGold == 17)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju17 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(317);
                                    string[] numgold17 = modeldaoju17.Changjing.Split(',');
                                    numgoldf = numgold17;
                                }
                                else if (modle22.McolletGold == 18)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju18 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(318);
                                    string[] numgold18 = modeldaoju18.Changjing.Split(',');
                                    numgoldf = numgold18;
                                }
                                else if (modle22.McolletGold == 19)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju19 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(319);
                                    string[] numgold19 = modeldaoju19.Changjing.Split(',');
                                    numgoldf = numgold19;
                                }
                                if (randgold == 0)
                                {
                                    int goldnumder = Convert.ToInt32(numgoldf[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                            
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 10000);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                }
                                else
                                {
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        int a = 0;
                                        string randdaoju = string.Empty;

                                        int goldnum = 0;

                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                            goldnum = 10000;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                            }
                                            string rand = "";
                                            string rand1 = "";
                                            //根据价格取不同的鱼类
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 200)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 200 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }

                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                        }
                                    }
                                }
                            }; break;
                        #endregion

                        #region 场景5操作
                        case 5:
                            {
                                string[] numgold20 = new string[] { };
                                if (modle22.McolletGold == 20)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju20 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(320);
                                    numgold20 = modeldaoju20.Changjing.Split(',');
                                    numgoldf = numgold20;
                                }
                                else if (modle22.McolletGold == 21)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju21 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(321);
                                    string[] numgold21 = modeldaoju21.Changjing.Split(',');
                                    numgoldf = numgold21;
                                }
                                else if (modle22.McolletGold == 22)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju22 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(322);
                                    string[] numgold22 = modeldaoju22.Changjing.Split(',');
                                    numgoldf = numgold22;
                                }
                                else if (modle22.McolletGold == 23)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju23 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(323);
                                    string[] numgold23 = modeldaoju23.Changjing.Split(',');
                                    numgoldf = numgold23;
                                }

                                else if (modle22.McolletGold == 24)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju24 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(324);
                                    string[] numgold24 = modeldaoju24.Changjing.Split(',');
                                    numgoldf = numgold24;
                                }
                                if (randgold == 0)
                                {
                                    int goldnumder = Convert.ToInt32(numgoldf[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 20000);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                }
                                else
                                {
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        int a = 0;
                                        string randdaoju = string.Empty;

                                        int goldnum = 0;
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                            goldnum = 20000;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                  
                                            }
                                            string rand = "";
                                            string rand1 = "";
                                            //根据价格取不同的鱼类
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 1000)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }

                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);

                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间   
                                        }
                                    }
                                }
                            }; break;
                        #endregion

                        #region 场景6操作
                        case 6:
                            {
                                string[] numgold25 = new string[] { };
                                if (modle22.McolletGold == 25)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju25 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(325);
                                    numgold25 = modeldaoju25.Changjing.Split(',');
                                    numgoldf = numgold25;
                                }
                                else if (modle22.McolletGold == 26)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju26 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(326);
                                    string[] numgold26 = modeldaoju26.Changjing.Split(',');
                                    numgoldf = numgold26;
                                }
                                else if (modle22.McolletGold == 27)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju27 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(327);
                                    string[] numgold27 = modeldaoju27.Changjing.Split(',');
                                    numgoldf = numgold27;
                                }
                                else if (modle22.McolletGold == 28)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju28 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(328);
                                    string[] numgold28 = modeldaoju28.Changjing.Split(',');
                                    numgoldf = numgold28;
                                }
                                else if (modle22.McolletGold == 29)
                                {
                                    BCW.bydr.Model.CmgDaoju modeldaoju29 = new BCW.bydr.BLL.CmgDaoju().GetCmgDaoju(329);
                                    string[] numgold29 = modeldaoju29.Changjing.Split(',');
                                    numgoldf = numgold29;
                                }
                                if (randgold == 0)
                                {
                                    int goldnumder = Convert.ToInt32(numgoldf[0]);

                                    if (modle22.DcolletGold == 0)
                                    {
                                        BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();
                                        m1.AllGold = 0;
                                        m1.AcolletGold = goldnumder;
                                        m1.changj = modle22.Changj;
                                        m1.coID = "";
                                        m1.cxid = 1;
                                        m1.random = 0;
                                        m1.Stime = DateTime.Now;
                                        m1.Signtime = DateTime.Now;
                                        m1.usID = meid;
                                        m1.Vit = 0;
                                        new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, 10);
                                        new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币                     
                                        new BCW.bydr.BLL.CmgToplist().Updatetime(meid, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().updatetime1(id11, DateTime.Now);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnumder - 50000);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnumder);//更新本次游戏所得的游戏币
                                    }
                                }
                                else
                                {
                                    if (modle22.randdaoju == "")//更新随机鱼的价格
                                    {
                                        int a = 0;
                                        string randdaoju = string.Empty;
                                        Random ran = new Random();

                                        int goldnum = 0;
                                        goldnum = Convert.ToInt32(numgoldf[Convert.ToInt32(randgoldnum_1[modle22.randnum/*根据次数读取鱼的价格*/])]);

                                        if (goldnum == 0)
                                            goldnum = 50000;
                                        while (a < 10)
                                        {
                                            int r = num(modle22.randten, goldnum)[a];//根据随机数的权值计算出每条鱼的价格
                                            randdaoju += Convert.ToString(r);
                                            randdaoju += ",";
                                            a++;
                                        }
                                        new BCW.bydr.BLL.Cmg_Top().Updateranddaoju(randdaoju, id11);
                                        new BCW.bydr.BLL.Cmg_Top().UpdateColletGold(id11, goldnum);//更新本次游戏所得的游戏币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateAllcolletGold(id11, goldnum - model0.changj2);//更新实际所得币
                                        new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold);
                                    }
                                    else
                                    {
                                        if (modle22.DcolletGold < 10)
                                        {
                                            if (modle22.DcolletGold == 9)
                                            {
                                                new BCW.bydr.BLL.CmgToplist().UpdateAllcolletGold(meid, new BCW.bydr.BLL.Cmg_Top().GetCmg_AllcolletGold1(meid));//更新排行榜总收集币
                                            }

                                            string rand = "";
                                            string rand1 = "";
                                            //根据价格取不同的鱼类
                                            if (Convert.ToInt32(randaoju[modle22.DcolletGold]) < 500)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum1.Length - 1);
                                                rand = Convert.ToString(daojunum1[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else if (Convert.ToInt32(randaoju[modle22.DcolletGold]) >= 500 && Convert.ToInt32(randaoju[modle22.DcolletGold]) < 2000)
                                            {
                                                int r = 0;
                                                r = R(0, daojunum2.Length - 1);
                                                rand = Convert.ToString(daojunum2[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }
                                            else
                                            {
                                                int r = 0;
                                                r = R(0, daojunum3.Length - 1);
                                                rand = Convert.ToString(daojunum3[r]);
                                                rand1 += modle22.randyuID + rand + ",";
                                                new BCW.bydr.BLL.Cmg_Top().UpdaterandyuID(rand1, id11);
                                            }

                                            new BCW.bydr.BLL.Cmg_Top().UpdateDcolletGold(id11, modle22.DcolletGold + 1);
                                            new BCW.bydr.BLL.Cmg_Top().UpdateYcolletGold(id11, 0);
                                            BCW.bydr.Model.Cmg_Top modle33 = new BCW.bydr.BLL.Cmg_Top().GetCmg_Top(id11);
                                            string[] randyu33 = modle33.randyuID.Split(',');
                                            BCW.bydr.Model.Cmg_notes m1 = new BCW.bydr.Model.Cmg_notes();//添加动态记录
                                            m1.AllGold = 0;
                                            m1.AcolletGold = Convert.ToInt64(randaoju[modle22.DcolletGold]);
                                            m1.changj = modle22.Changj;
                                            m1.coID = "";
                                            m1.cxid = 0;
                                            m1.random = Convert.ToInt32(randyu33[modle33.DcolletGold - 1]);
                                            m1.Stime = DateTime.Now;
                                            m1.Signtime = DateTime.Now;
                                            m1.usID = meid;
                                            m1.Vit = 0;
                                            new BCW.bydr.BLL.Cmg_notes().Add(m1);
                                            Response.Write("捕鱼成功,价值" + randaoju[modle22.DcolletGold] + ub.Get("SiteBz") + " <br />");

                                            new BCW.bydr.BLL.Cmg_Top().updatetime(id11, DateTime.Now);//更新防刷时间 
                                        }
                                    }
                                }
                            }; break;
                        default:
                            break;
                            #endregion
                    }
                    System.Threading.Thread.Sleep(R(1, 4) * 1000);//睡眠
                }
                #endregion
                #endregion

                Response.Write("<br/>" + meid + "捕鱼完成ok1————当前时间:" + DateTime.Now + "<br/><br/>");
            }
            else
            {
                //在zuid去除该meid
                zuid = zuid.Replace("#" + meid + "#", "#").Trim();
                if (zuid == "#")
                {
                    zuid = "";
                }
                ub xml2 = new ub();
                string xmlPath2 = "/Controls/BYDR.xml";
                Application.Remove(xmlPath2);//清缓存
                xml2.ReloadSub(xmlPath2); //加载配置
                xml2.dss["zuid"] = zuid.Trim();
                System.IO.File.WriteAllText(Server.MapPath(xmlPath2), xml2.Post(xml2.dss), System.Text.Encoding.UTF8);
                Response.Write("<br/>" + meid + "机器人的捕鱼次数已全部捕完ok1————当前时间:" + DateTime.Now + "<br/><br/>");
            }
        }
    }

    //机器人兑奖
    public void Robot_case()
    {
        double buyuprofit = 0;
        try
        {
            buyuprofit = Convert.ToInt32(ub.GetSub("bydrbuyuprofit", xmlPath)) / 1000;//扣税
        }
        catch
        {
            buyuprofit = 0;
        }

        DataSet ds = new BCW.bydr.BLL.Cmg_Top().GetList("*", "Bid=1 and DcolletGold=10 and isrobot=1");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                //本地投注数据
                int ID = int.Parse(ds.Tables[0].Rows[i]["ID"].ToString());
                int usID = int.Parse(ds.Tables[0].Rows[i]["usID"].ToString());
                long ColletGold = Int64.Parse(ds.Tables[0].Rows[i]["ColletGold"].ToString());

                new BCW.BLL.User().UpdateiGold(usID, +Convert.ToInt64(ColletGold * (1 - buyuprofit)), "" + GameName + "兑奖—标识id:" + ID + "");
                new BCW.bydr.BLL.Cmg_Top().UpdateBid(0, ID, usID);
                Response.Write("机器人[" + usID + "]兑奖成功！ok1————获得" + ColletGold + "");
            }

        }
    }

    //随机得到出动的ID
    private string GetUsID()
    {
        string UsID = "";
        string PlayUsID = ub.GetSub("ROBOTID", "/Controls/BYDR.xml");
        try
        {
            string[] sNum = Regex.Split(PlayUsID, "#");
            List<string> aa = new List<string>();
            for (int i = 0; i < sNum.Length; i++)
            {
                aa.Add(sNum[i]);
            }
            Random rd = new Random();
            int temp = 0;
            for (int i = 0; i < zu; i++)
            {
                temp = rd.Next(0, aa.Count);
                UsID = UsID + (aa[temp]) + "#";
                aa.Remove(aa[temp]);
            }
            UsID = "#" + UsID;
        }
        catch { }

        return UsID;
    }

    //随机得到进入的场景
    private int Get_changjing()
    {
        Random rac = new Random();
        int cc = rac.Next(1, 7);
        return cc;
    }

    protected int R(int x, int y)
    {
        Random ran = new Random();
        int RandKey = ran.Next(x, y);
        return RandKey;
    }

    //生成随机不重复的数
    private int[] GetRandomNum(int n, int min, int max)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        Random r = new Random();
        int[] result = new int[n];
        int num = 0;
        for (int i = 0; i < n; i++)
        {
            num = r.Next(min, max + 1);
            while (dict.ContainsKey(num))
            {
                num = r.Next(min, max + 1);
            }
            dict.Add(num, 1);
        }
        dict.Keys.CopyTo(result, 0);
        return result;
    }


    //根据随机数的权值算出每条鱼的价格
    private int[] num(string m, int x)
    {
        string[] randten = m.Split(',');
        int i = 0;
        int n = 0;
        int[] num1 = new int[10];
        for (i = 0; i < randten.Length - 1; i++)
            n += Convert.ToInt32(randten[i]);//数组求和
        int k = 0;
        for (i = 0; i < 9; i++)
        {

            double m1 = 0;
            m1 = Convert.ToDouble(randten[i]) / n;//数组中的权值
            k += Convert.ToInt32(m1 * x);
            num1[i] = Convert.ToInt32(m1 * x);
        }
        num1[9] = x - k;//最后一个价格
        return num1;
    }

    private void addbuyub(int meid1)
    {
        int meid = meid1;
        BCW.bydr.Model.Cmg_Top m1 = new BCW.bydr.Model.Cmg_Top();
        m1.AllcolletGold = 0;
        m1.Bid = 0;
        m1.Changj = "";
        m1.ColletGold = 0;
        m1.DcolletGold = 10;
        m1.jID = 0;
        m1.McolletGold = 0;
        m1.randdaoju = "";
        m1.randgoldnum = "";
        m1.randnum = 0;
        m1.randyuID = "";
        m1.Time = DateTime.Now;
        m1.updatetime = DateTime.Now;
        m1.usID = meid;
        m1.YcolletGold = 0;
        m1.randten = "";
        m1.isrobot = 0;
        new BCW.bydr.BLL.Cmg_Top().Add(m1);
        BCW.bydr.Model.CmgToplist m2 = new BCW.bydr.Model.CmgToplist();
        m2.AllcolletGold = 0;
        m2.DcolletGold = 0;
        m2.McolletGold = 0;
        m2.stype = 0;
        m2.YcolletGold = 0;
        m2.Time = DateTime.Now;
        m2.usID = meid;
        m2.sid = 0;
        m2.updatetime = DateTime.Now;
        m2.vit = 10;
        m2.Signtime = DateTime.Now;
        new BCW.bydr.BLL.CmgToplist().Add(m2);
        BCW.bydr.Model.Cmg_buyuDonation m3 = new BCW.bydr.Model.Cmg_buyuDonation();
        m3.Ctime = DateTime.Now;
        m3.Donation = 0;
        m3.stype = 0;
        m3.time = DateTime.Now;
        m3.usID = meid;
        new BCW.bydr.BLL.Cmg_buyuDonation().Add(m3);
    }

}
