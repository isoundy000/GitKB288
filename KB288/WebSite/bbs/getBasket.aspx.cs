using System;
using System.Data;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using BCW.Common;
using System.Timers;
using System.Xml;
using System.Collections.Generic;
//20160609   lanqiu 
public partial class bbs_getBasket:System.Web.UI.Page 
{
    private string ex="";
    string getZQOpen = (ub.GetSub("getZQOpen", "/Controls/footballs.xml"));
    string getLQOpen = (ub.GetSub("getLQOpen", "/Controls/footballs.xml"));
    protected void Page_Load(object sender, EventArgs e)
    {
    
        #region 篮球数据地址分析
        //string sa = "250105^友谊赛,友誼賽^4^#00A8A8^07月25日<br>08:00^50^^721^美国,美國,USA^83^中国,中國,China^55^29^26^13^29^16^^^^^0^^^^^^^^CCTV5^^<a href=http://www.310tv.com/channel/cctv5.html target=_blank><font color=blue>CCTV5</font></a>&nbsp;&nbsp;<a href=http://kbs.sports.qq.com/kbsweb/game.htm?mid=100002:2368 target=_blank><font color=blue>QQ直播</font></a>^2^^0^,^57^2016^^0";
        //string[] ssa = sa.Split('^');
        //int il = 0;
        //foreach (string s in ssa)
        //{
        //    Response.Write(il+"："+s + "<br/>");
        //    il++;
        //}

        //http://nba.win007.com/jsData/tech/2/47/247422.js?flesh=0.3412450622434914 一场比赛的数据
        // http://nba.win007.com/jsData/txtLive/2/47/247422.js 文字直播数据
        #endregion

        #region  篮球捉取 开关与开始
        if (getLQOpen == "1")
        {
            try
            {
                #region 篮球捉取开始
                try
                {
                    #region 开始
                    string[] url = new string[9];
                    url[0] = "http://bf.win007.com/NBA/today.xml";//今日赛程
                    url[1] = "http://bf.win007.com/NBA/today2.xml";//今日赛程
                    url[2] = "http://bf.win007.com/nba_date.aspx?date=2016-8-2&h=0&m=3";//一周赛程
                    for (int k = 3; k < 9; k++)
                    {
                        url[k] = "http://bf.win007.com/nba_date.aspx?date=" + DateTime.Now.AddDays(k - 1).ToString("yyyy-MM-dd");//一周赛程
                    }
                    //XmlDocument xl= getBaskXML(); 
                    Response.Write("<meta http-equiv=\"refresh\" content=\"80\" />");
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    stopwatch.Start();
                    List<mac> list;
                    for (int i = 0; i < url.Length; i++)
                    {
                        list = TranList(url[i]);
                        if (list != null)
                            Response.Write("球赛总数:" + list.Count + "捉取完成!上次捉取:" + DateTime.Now + "耗时:" + stopwatch.Elapsed.TotalSeconds + "秒" + "<br/>");
                    }
                    stopwatch.Stop();
                    Response.Write("<font color=\"red\">" + "总耗时:" + stopwatch.Elapsed.TotalSeconds + "秒</font><br/>");
                    if (ex != "")
                    {
                        Response.Write("异常消息:" + ex);
                    }
                    #endregion
                }
                catch (Exception ee) { Response.Write("捉取异常：" + ee.ToString()); }

                #endregion
                Response.Write("=========篮球捉取成功！当前(getLQOen==1)=====ok1ok1ok1ok1ok1====" + "</br>");
            }
            catch { Response.Write("=========篮球捉取已异常动停止！当前(getLQOen==1)=====error1error1error1error1error1====" + "</br>"); }

        }
        else
        {
            Response.Write("=========篮球捉取已手动停止！当前(getLQOen==0)=====close1close1close1close1close====" + "</br>");
        }
        #endregion

        #region 足球捉取 开关于开始
        if (getZQOpen == "1")
        {
            try
            {
                #region 足球捉取开始

                string jinqiu = "";
                string gunqiu = "";
                int isNum = 1;//标识:默认mc1捉到
                #region  取进行中球赛更新
                //取进行中球赛更新//and convert(datetime,ft_time,120)>getDate() 
                string strWhere = "ft_state!='完' and ft_state!='未' and ft_state!='推迟'  and ft_state!='待定' and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)< DATEADD(hour,1,GETDATE()) and convert(datetime,ft_time+convert(datetime,ft_caipan,8),120)> DATEADD(hour,-3,GETDATE()) ORDER BY convert(datetime,ft_time,120) DESC,convert(datetime,ft_caipan,120) ASC";
                DataSet ds = new BCW.BLL.tb_ZQLists().GetList(" * ", strWhere);
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
                string[] pageValUrl = { "act", "ptype", "id", "backurl", "State" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;
                string timedate = "";
                string timehour = "";
                string alltime = "";
                string result1 = "";
                string state1 = "";
                string ZQurl1 = "http://3g.8bo.com/3g/football/score/history.aspx?date=2016/06/28&st=allEvents&by=detail&eid=857322";//历史地址
                string ZQurl2 = "http://3g.8bo.com/3g/football/score/today.aspx?st=hasStart&by=detail&eid=856226";//具体某一场的地址
                string htmlText = "";
                System.Diagnostics.Stopwatch ZQstopwatch = new System.Diagnostics.Stopwatch();
                System.Diagnostics.Stopwatch ZQstopwatch1 = new System.Diagnostics.Stopwatch();
                ZQstopwatch.Start();
                //是否执行更新每个页面
                if (false)
                {
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        // Response.Write("<br/>" + "当前进行球赛" + ds.Tables[0].Rows.Count);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ZQstopwatch1.Start();
                            timedate = ds.Tables[0].Rows[i]["ft_time"].ToString();
                            timehour = ds.Tables[0].Rows[i]["ft_caipan"].ToString();
                            alltime = timedate + " " + timehour;
                            ZQurl2 = "http://3g.8bo.com/3g/football/score/today.aspx?st=hasStart&by=detail&eid=" + ds.Tables[0].Rows[i]["ft_bianhao"].ToString();
                            System.Threading.Thread.Sleep(500);
                            Response.Write("当前捉取url:" + ZQurl2 + "<br/>");
                            //获取html
                            htmlText = GetHtmlSource(ZQurl2, Encoding.UTF8);
                            ZQstopwatch1.Reset();
                            if (ZQstopwatch1.Elapsed.TotalSeconds > 5)
                            { break; }
                            Response.Write("<font color=\"red\">" + "耗时:" + ZQstopwatch.Elapsed.TotalSeconds + "秒</font><br/>");
                            //从html获取滚球数据
                            gunqiu = getPeilvForLianSai(htmlText);
                            //从html获取状态
                            state1 = getStateForLianSai(htmlText);
                            //继续获取结果
                            result1 = getResult(htmlText);
                            #region 测试用
                            //  Response.Write("+"+ GetTitleContent(htmlText,"tbody") + "+");
                            // int p_id = 0;
                            // string[] boTemp1 = Regex.Split(htmlText, @"<td class=.W1\s[\w\d]+.>");
                            ////string[] boTemp1 = Regex.Split(htmlText, @"<hr  size=""1"">");
                            // string strpattern = @"by=detail&amp;eid=(\d+).>析";
                            // Match mtitle = Regex.Match(boTemp1[0], strpattern, RegexOptions.IgnoreCase);
                            //if (mtitle.Success)
                            //{
                            //    p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                            //    Response.Write(p_id + "<br />");
                            //}<hr size="1">
                            // Response.Write(boTemp1[1] + "<br />");
                            //string[] boTemp2 = Regex.Split(boTemp1[1], @"hr");
                            //Response.Write("boTemp2[1]"+boTemp2[1] + "<br />");
                            //Response.Write("boTemp2[2]" + boTemp2[2] + "<br />");
                            //Response.Write("boTemp2[3]"+ boTemp2[3] + "<br />-------------");
                            //Response.Write("boTemp2[4]" + boTemp2[4] + "<br />");
                            #endregion

                            #region 继续获取进球时刻的数据
                            Regex regex = new Regex(@"<table.*?>[\s\S]*?<\/table>");
                            MatchCollection mc = regex.Matches(htmlText);
                            //获取集合类中需要的table
                            String newHtmlStr = "";
                            try
                            {
                                newHtmlStr = mc[0].ToString();
                                Response.Write(mc[0].ToString());
                            }
                            catch { }
                            #endregion

                            //将数据返回(滚球.即时.初盘)
                            string[] hr = Regex.Split(newHtmlStr, (@"<hr size=""1"""));
                            string hitball = "";
                            try
                            {
                                if (mc[1].ToString().Length > 5)
                                {
                                    hitball = mc[1].ToString();
                                }
                                else
                                { isNum = 2; }
                            }
                            catch
                            {
                                isNum = 2;//默认mc1捉不到,null
                            }
                            // 获取Id
                            int idd = new BCW.BLL.tb_ZQLists().GetIdFromBianhao(Convert.ToInt32(ds.Tables[0].Rows[i]["ft_bianhao"]));
                            if (state1 != "" && result1 != "" && idd > 0)
                            {
                                BCW.Model.tb_ZQLists ftt = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(idd);
                                //  ftt.ft_addTime = DateTime.Now;
                                // ftt.ft_bianhao = Convert.ToInt32(ds.Tables[0].Rows[i]["ft_bianhao"]);
                                //  ftt.ft_beiyong = title;
                                //  ftt.ft_teamStyle = title;
                                //  ftt.ft_caipan = tttime;
                                //  ftt.ft_didian = "0";
                                //  ftt.ft_glod = 0;
                                //  ftt.ft_hit = 0;
                                //  ftt.ft_news = infomat;
                                //  ftt.ft_otherNews = "0";
                                //  ftt.ft_otherNews = jinqiu;
                                ftt.ft_overTime = DateTime.Now;
                                //  int idd = new BCW.BLL.tb_ZQLists().GetIdFromBianhao(p_id);
                                BCW.Model.tb_ZQLists fttball = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(idd);

                                if (fttball.ft_result != result1)
                                {
                                    string strwhere = " FootBallId =" + idd;
                                    DataSet dds = new BCW.BLL.tb_ZQCollection().GetList(" * ", strwhere);
                                    if (dds != null && dds.Tables[0].Rows.Count > 0)
                                    {
                                        string VE = ConfigHelper.GetConfigString("VE");
                                        string SID = ConfigHelper.GetConfigString("SID");
                                        for (int ii = 0; ii < dds.Tables[0].Rows.Count; ii++)
                                        {
                                            if (dds.Tables[0].Rows[ii]["UsId"].ToString().Length > 2)
                                            {
                                                int usid = Convert.ToInt32(dds.Tables[0].Rows[ii]["UsId"]);
                                                if (new BCW.BLL.User().Exists(usid))
                                                {
                                                    string name = new BCW.BLL.User().GetUsName(usid);
                                                    string text = "";
                                                    if (result1 == "")
                                                    {
                                                        text = "0-0";
                                                    }
                                                    string strText = "你收藏的" + "[URL=/bbs/guess2/live.aspx?act=21&amp;Id=" + idd + "]" + ftt.ft_team1 + "VS" + ftt.ft_team2 + "[/URL]变化啦," + "当前比分" + result1 + "快去看看吧";
                                                    new BCW.BLL.Guest().Add(0, usid, name, strText);
                                                }
                                            }

                                        }
                                    }
                                }
                                ftt.ft_result = result1;
                                ftt.ft_state = state1;
                                if (hr.Length > 3)
                                {
                                    ftt.ft_team1Explain = getStringNew(hr[1].ToString());
                                }
                                if (hr.Length > 3)
                                {
                                    ftt.ft_team2Explain = getStringNew(hr[2].ToString());
                                }
                                if (hr.Length > 3)
                                {
                                    ftt.ft_state1 = getStringNew(hr[3].ToString());
                                }
                                ftt.ft_state2 = (hitball);
                                //  ftt.ft_team1 = p_one;   
                                //  ftt.ft_team2 = p_two;                
                                //  ftt.ft_time = Convert.ToDateTime(Date);
                                // ftt.Identification = 1;
                                //  ftt.isDone = 1;
                                try
                                {
                                    new BCW.BLL.tb_ZQLists().Update(ftt);
                                }
                                catch (Exception ee) { Response.Write(ee.ToString()); }
                                Response.Write(idd + "[" + ftt.ft_time + "]" + ftt.ft_teamStyle + ftt.ft_team1 + "--" + ftt.ft_team2 + "赛事重更成功！" + "当前" + ftt.ft_result + "状态" + ftt.ft_state + "<br/>");
                            }
                            else
                            {
                                Response.Write("球赛ID:" + idd + "赛事重更失败！" + "<br/>");
                            }
                        }
                    }
                    else
                    {
                        Response.Write("无进行中赛事！" + "<br/>");
                    }
                }
                #endregion
                getMatch();
                ZQstopwatch1.Stop();
                ZQstopwatch.Stop();
                Response.Write("<font color=\"red\">" + "总耗时:" + ZQstopwatch.Elapsed.TotalSeconds + "秒</font><br/>");

                #endregion
                Response.Write("=========足球捉取成功！当前(getZQOpen==0)======ok1ok1ok1ok1ok1===" + "</br>");
            }
            catch { Response.Write("=========篮球捉取已异常动停止！当前(getZQOpen==1)=====error1error1error1error1error1====" + "</br>"); }
        }
        else
        {
            Response.Write("=========足球捉取已手动停止！当前(getZQOpen==0)======close1close1close1close1close1close===" + "</br>");
        }
        #endregion
    }

    #region 公用函数
    //获取网页HTML源码  
    public static string GetHtmlSource(string url, Encoding charset)
    {

        string _html = string.Empty;
        try
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            using (Stream _stream = _response.GetResponseStream())
            {
                using (StreamReader _reader = new StreamReader(_stream, charset))
                {
                    _html = _reader.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {
            using (StreamReader sr = new StreamReader(ex.Response.GetResponseStream()))
            {
                _html = sr.ReadToEnd();
            }
            // _html = ex.ToString();
        }
        catch (Exception ex)
        {
            _html = ex.Message;
        }
        return _html;
    }

    #endregion

    #region 篮球相关函数
    #region 类model
    public class Matchs1
    {
        public int name_en;
        public string classType;
        public DateTime machtime;
        public string matchstate;
        public DateTime remaintime;
        public int hometeamID;
        public string hometeam;
        public int guestteamID;
        public string guestteam;
        public int homescore;
        public int guestscore;
        public string homeone;
        public string guestone;
        public string hometwo;
        public string guesttwo;
        public string homethree;
        public string guestthree;
        public string homefour;
        public string guestfour;
        public DateTime addtime;
        public string addTechnic;
        public string explain;
        public string explain2;
        public string contentList;
    }
    public class Matchs
    {
       
        public List<Matchs1> listMatch;
        public Matchs()
        {
            listMatch = new List<Matchs1>();
        }

    }
    public class mac
    {
        public string h;
    }
    public class macs
    {
        public List<mac> listMatch;
        public macs()
        {
            listMatch = new List<mac>();
        }
    }
    #endregion

    //获得网页数据
    public XmlDocument getBaskXML(string url)
    {
        #region 地址
        //url = "http://bf.win007.com/NBA/today.xml";//今日
        //url = "http://bf.win007.com/NBA/today2.xml";
        //// url = "http://bf.win007.com/nba_date.aspx?date=2016-7-28";
        //url = "http://bf.win007.com/NBA/today.xml";
        #endregion

        XmlDocument xml = new XmlDocument();
        try
        {
            xml.Load(url);
        }
        catch (WebException e)
        {
            ex += "获取xml网络异常:" + e.ToString() + "#";
            xml = null;
        }
        catch(Exception ee)
        {
            ex += "获取xml异常:"+ee.ToString() + "#";
            xml = null;
        }      
        Response.Write("当前捉取网址："+url+"<br/>");
        return xml;
    }
    //取状态
    private string getCount(string count)
    {
        switch (count)
        {
            case "1":
                return "第一节";
            case "2":
                return "第二节";
            case "3":
                return "第三节";
            case "4":
                return "第四节";
            case "-1":
                return "完";
            case "0":
                return "未开赛";
            case "50":
                return "中场";
            case "-2":
                return "待定";
            case "-5":
                return "推迟";
            default:
                return "完";
        }
    }
    //数据处理
    public List<mac> TranList(string url)
    {
        List<mac> list = new List<mac>();
        XmlDocument dom = getBaskXML(url);
        XmlNodeList nodelist = dom.SelectSingleNode("/c").ChildNodes;
        XmlDocument text;
        string texturl = "";
        string textHtml ="";
        string[] textList;
        string[] s1;
        int ID = 0;
        BCW.Model.tb_BasketBallWord mod = new BCW.Model.tb_BasketBallWord();
        foreach (XmlNode node in nodelist)
        {
            foreach (XmlNode nodes in node)
            {
                if (nodes.Name == "h"&& nodes.InnerText.Length>10)
                {
                    #region <h>数据分析</h>
                    // 250106 ^  0球赛ID
                    //友谊赛,友誼賽 ^ 1 类型
                    //4 ^   2 总共多少节
                    //#00A8A8^  3 球队颜色类型
                    //07月20日14:00^  4开赛时间
                    //4 ^   5第几节
                    //04:31^  6剩余时间  
                    //2560 ^   7主队id
                    //山东女篮,山東女籃,Shandong Women's^  8主队名称
                    //2570 ^  9客队id 
                    //黑龙江女篮,黑龍江女籃,Heilongjiang Women's^ 10客队名称
                    //69 ^ 11主队当前得分
                    //67 ^ 12客队当前得分
                    //17 ^ 13主队第一节
                    //17 ^ 14客队第一节
                    //26 ^ 15主队第二节
                    //23 ^ 16客队第二节
                    //18 ^17主队第三节
                    //15 ^ 18客队第三节
                    //8 ^ 19主队第四节
                    //12 ^ 20客队第四节
                    //0 ^  21
                    //^  22
                    //^ 23
                    //^ 24
                    //^25
                    //^26
                    //^27
                    //^28
                    //^29
                    //^30
                    //^31
                    //2 ^32 tv直播地址
                    //^33
                    //0 ^34
                    //1.1,6.25^ 35 欧指  主队，客队
                    //57 ^  36 
                    //2016 ^37
                    //^0 38 247422^WNBA,WNBA^4^#446DAB^07月20日<br>23:30^-1^^69^华盛顿神秘人[5],華盛頓奇異[5],Washington Mystics[5]^68^纽约自由人[1],紐約自由人[1],New York Liberty[1]^
                    //81 ^88^23^17^14^30^22^22^22^19^0^0^0^^^^^True^^神秘人-得分:梅斯曼(20) 篮板:佛恩(6) 助攻:拉塔(6)<br>自由人-得分:丽贝卡艾伦(19) 篮板:查尔斯(9) 助攻:B.博伊德(7)^^1^2^0^2.29,1.59^2^2016^^0
                    #endregion

                    #region 取值
                    string[] str = nodes.InnerText.ToString().Split('^');
                    BCW.Model.tb_BasketBallList model = new BCW.Model.tb_BasketBallList();
                    model.name_en = int.Parse(str[0]);                 
                    string[] s = str[1].ToString().Split(',');
                    model.classType = s[0].Trim();
                    model.addTechnic = str[3].Trim();
                    DateTime start = Convert.ToDateTime(Convert.ToDateTime((DateTime.Now.Year + "年" + str[4]).Replace("<br>", " ").ToString()).ToString("yyy-MM-dd HH:mm:ss"));
                    // Utils.Error(""+ start + "","");
                    if (start.ToString().Trim() != "")
                    {
                        start = Convert.ToDateTime(start);
                    }
                    else
                    {
                        start = new DateTime(1900, 1, 1);
                    }

                    model.matchtime = start;//比赛时间    
                    model.addtime = DateTime.Now;
                    model.remaintime = DateTime.Now;
                    model.matchstate = str[5].ToString().Trim();

                    #region 直播文字捉取
                    //  System.Diagnostics.Stopwatch stopwatch2 = new System.Diagnostics.Stopwatch();
                    //  stopwatch2.Start();
                    //已开赛 
                    model.remaintime = DateTime.Now;
                    mod.addtime = DateTime.Now;
                    model.ID = new BCW.BLL.tb_BasketBallList().GetIDFromName_en(model.name_en);
                    if (model.matchstate != "-1" && model.matchstate != "0" && model.matchstate != "-5" && model.matchtime.AddHours(4) > DateTime.Now)
                    {
                        //线路1
                        texturl = "http://nba.win007.com/jsData/txtLive/" + model.name_en.ToString().Substring(0, 1) + "/" + model.name_en.ToString().Substring(1, 2) + "/" + model.name_en + ".js";               
                        textHtml = GetHtmlSource(texturl, Encoding.UTF8);
                        if (textHtml.Length < 10)
                        {
                            //线路2
                            texturl = "http://nba.nowscore.com/jsData/txtLive/" + model.name_en.ToString().Substring(0, 1) + "/" + model.name_en.ToString().Substring(1, 2) + "/" + model.name_en + ".js";
                            textHtml = GetHtmlSource(texturl, Encoding.UTF8);
                        }
                        Response.Write("当前直播文字地址:" + "<a href=\"" + texturl + "\">" + texturl + "</a><br/>");
                        textList = textHtml.Split('!');      
                        if (textHtml.Length > 50 && textHtml.Contains("!"))
                        {
                            foreach (string a in textList)
                            {
                                if (a != "" && a.Contains("^"))
                                {
                                    s1 = (a).Split('^');
                                    mod.guestteam = (s1[3].ToString().Trim());
                                    mod.hometeam = (s1[2].ToString().Trim());
                                    mod.isSame = s1[0].ToString();
                                    mod.last = Convert.ToInt32(s1[5].ToString().Trim());
                                    mod.whichTeam = s1[1].ToString().Trim();
                                    mod.name_enId = model.name_en;
                                    mod.listContent = s1[4].ToString() + "!";
                                    //   Response.Write(a + "<br/>");
                                    if (!new BCW.BLL.tb_BasketBallWord().ExistsName_enOne(mod.last))//不存在该句，添加
                                    {
                                        new BCW.BLL.tb_BasketBallWord().Add(mod);
                                    }
                                }
                            }
                        }
                    }
                    // stopwatch2.Stop();
                    // Response.Write(stopwatch2.Elapsed.TotalSeconds + "<br/>");
                    #endregion

                    model.isDone = str[6].ToString().Trim();//剩余时间
                    model.hometeamID = Convert.ToInt32(str[7]);
                    model.hometeam = (str[8].ToString().Split(','))[0].Trim();
                    model.guestteamID = Convert.ToInt32(str[9]);
                    model.guestteam = (str[10].ToString().Split(','))[0].Trim();
                    if (str[11] == "")
                    model.homescore = 0;
                    else
                    model.homescore = Convert.ToInt32(str[11]);
                    if (str[12] == "")
                        model.guestscore = 0;
                    else
                        model.guestscore = Convert.ToInt32(str[12]);
                    //1
                    model.homeone = str[13].Trim();
                    model.guestone = str[14].Trim();
                    //2
                    model.hometwo = str[15].Trim();
                    model.guesttwo = str[16].Trim();
                    //3
                    model.homethree = str[17].Trim();
                    model.guestthree = str[18].Trim();
                    //4
                    model.homefour = str[19].Trim();
                    model.guestfour = str[20].Trim();
                    model.connectId = 0;//与官网链接ID
                    model.contentList = "";
                    //if (str[30].ToString() != "")
                    model.contentList = str[30].ToString() + "!";

                    if (str[30].ToString().Contains("<br>"))
                    {
                        string inf = str[30].ToString().Replace("<br>", "#").ToString();
                        string[] imfo = inf.Split('#');
                        if (imfo.Length > 1)
                        {
                            model.explain = imfo[0];//主队得分,助攻等数据
                            model.explain2 = imfo[1];   //客队得分,助攻等数据      
                        }
                    }
                    else
                    {
                        model.explain = "";
                        model.explain2 = "";
                    }
                    if (str[31].ToString() == "")
                    {
                        model.tv ="0";
                    }
                    else
                    {
                        model.tv = str[31].ToString();
                    }
                    model.team1 = str[33].ToString();
                    model.team2 = str[34].ToString();
                    model.homeEurope = str[35].ToString();
                    model.guestEurope = str[36].ToString();
                    model.isHidden = 0;//默认隐藏     
                    model.connectId = 0;//，默认无关联
                    //  Response.Write(model.matchtime+"********");

                    #region 开始读取竞猜 列表  自动识别隐藏与开启           
                    string strWhere = "p_type=" + 2 + " and p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0))  and p_basketve=0";
                    // 开始读取竞猜
                    DataSet have = new TPR2.BLL.guess.BaList().GetBaListList("*", strWhere);
                    if (have != null && have.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < have.Tables[0].Rows.Count; i++)
                        {
                            if (DT.GetTime(model.addtime.ToString(), "MM月dd日").ToString() == DT.GetTime(have.Tables[0].Rows[i]["p_TPRtime"].ToString(), "MM月dd日").ToString())
                            {
                                if (model.hometeam.Contains(have.Tables[0].Rows[i]["p_once"].ToString()) && model.guestteam.Contains(have.Tables[0].Rows[i]["p_two"].ToString()))
                                {
                                    if (model.classType.Contains(have.Tables[0].Rows[i]["p_title"].ToString()))
                                    {
                                        model.isHidden = 1;//模糊识别，1设为显示
                                        model.connectId = Convert.ToInt32(have.Tables[0].Rows[i]["ID"].ToString());
                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    model.result = str[31].ToString();
                    model.ID = new BCW.BLL.tb_BasketBallList().GetIDFromName_en(model.name_en);

                    #endregion 取值

                    #region 操作数据库
                    if (!new BCW.BLL.tb_BasketBallList().ExistsName(model.name_en))
                    {
                        int ia = 0;
                        try
                        {
                            int id = 0;
                            if (model.ID > 0 || model.name_en > 0)
                            {
                                id=new BCW.BLL.tb_BasketBallList().Add(model);
                                ia = id;
                            }
                            Response.Write(id+"*("+model.name_en+ ")#" + model.matchtime+"#" + model.hometeam + "-" + model.guestteam + "-" + "增加成功!状态:"+model.matchstate + "<br/>");
                        }
                        catch (Exception e)
                        {
                            ex += "--"+ia+"--"+"增加Name_en" + model.name_en + "新球赛异常:" + e.ToString() + "#" + "<br/>";
                        }
                    }
                    else 
                //    if((model.matchstate.ToString().Trim())!="-1")
                    {
                        //更新球赛
                       // try
                        {
                            //// 获取已有的数据行
                            BCW.Model.tb_BasketBallList ball = new BCW.BLL.tb_BasketBallList().Gettb_BasketBallListForName_en(model.name_en);
                            //统计内线数
                            int send = 0;
                            string BasketBallCollect = (ub.GetSub("BasketBallCollect", "/Controls/footballs.xml"));

                            //比分变化发内线
                            #region  收藏发内线
                            //收藏开关 1开 0关
                            if (BasketBallCollect == "1")
                            {
                              
                                //若正在进行的球赛比分变化
                                if (model.matchstate != "-1" && model.homescore != ball.homescore || model.guestscore != ball.guestscore&&model.matchtime.AddHours(4)>DateTime.Now)
                                {
                                    string strwhere = " BasketBallId =" + ball.ID;
                                    DataSet dds = new BCW.BLL.tb_BasketBallCollect().GetList(" * ", strwhere);
                                    if (dds != null && dds.Tables[0].Rows.Count > 0)
                                    {
                                        string VE = ConfigHelper.GetConfigString("VE");
                                        string SID = ConfigHelper.GetConfigString("SID");
                                        for (int ii = 0; ii < dds.Tables[0].Rows.Count; ii++)
                                        {
                                            // if (dds.Tables[0].Rows[ii]["UsId"].ToString().Length > 2)
                                            {
                                                int usid = Convert.ToInt32(dds.Tables[0].Rows[ii]["UsId"]);
                                                if (new BCW.BLL.User().Exists(usid))
                                                {
                                                    send++;
                                                    string name = (dds.Tables[0].Rows[ii]["UsName"]).ToString();
                                                    string strText = "你收藏的" + "[URL=/bbs/guess2/live.aspx?act=29&amp;Id=" + ball.ID + "]" + model.hometeam + "VS" + model.guestteam + "[/URL]变化啦," + "比分(" + "<font color=\"red\">" + model.homescore + "-" + model.guestscore + "</font>" + ")快去看看吧";
                                                    new BCW.BLL.Guest().Add(0, usid, name, strText);
                                                }
                                            }
                                        }
                                    }
                                }
                          //      Response.Write(ball.ID + "*(" + model.name_en + ")#" + model.matchtime + "#" + model.hometeam + "-" + model.guestteam + "-" + "比分(" + model.homescore + "-" + model.guestscore + ")" + "更新成功!状态:" + "<font color=\"red\">" + getCount(model.matchstate.Trim()) + "</font>");
                                if (send > 0)
                                {
                                    Response.Write("#已发内线[" + "<font color=\"red\">" + send + "</font>" + "]条<br/>");
                                }
                                else
                                { Response.Write("<br/>"); }
                            }
                            #endregion

                            if (model.ID > 0 || model.name_en > 0 )
                            {
                                if (model.matchstate != "-10" || model.matchstate =="0")
                                {
                                    if (ball.matchstate.ToString().Trim()!= "-1")//是否已完结
                                    {
                                        new BCW.BLL.tb_BasketBallList().UpdateName_en2(model);
                                        Response.Write(model.ID + "*(" + model.name_en + ")#" + model.matchtime + "#" + model.hometeam + "-" + model.guestteam + "-" + "球赛更新成功!状态:" + model.matchstate + "<br/>");
                                    }
                                    else
                                    {
                                        Response.Write(model.ID + "*(" + model.name_en + ")#" + model.matchtime + "#" + model.hometeam + "-" + model.guestteam + "-" + "已完场,无更新!状态:" + model.matchstate + "<br/>");
                                    }
                                }
                                else
                                {
                                    Response.Write(model.ID + "*(" + model.name_en + ")#" + model.matchtime + "#" + model.hometeam + "-" + model.guestteam + "-" +"<font color=\"blue\">"+ "未开赛,无更新!"+"</font>"+"状态:" + model.matchstate + "<br/>");
                                }
                            }
                            #region 选择批量更新 已屏蔽
                            //new BCW.BLL.tb_BasketBallList().UpdateisDone(ID, model.isDone);
                            //new BCW.BLL.tb_BasketBallList().Updatematchstate(ID, model.matchstate);
                            //new BCW.BLL.tb_BasketBallList().UpdateExplain(ID, model.explain, model.explain2);
                            //new BCW.BLL.tb_BasketBallList().UpdateEurope(ID, model.homeEurope, model.guestEurope);
                            //new BCW.BLL.tb_BasketBallList().UpdateScore(ID, model.homescore, model.guestscore);
                            //new BCW.BLL.tb_BasketBallList().UpdateOneScore(ID, model.homefour, model.guestfour);
                            //new BCW.BLL.tb_BasketBallList().UpdateTwoScore(ID, model.homefour, model.guestfour);
                            //new BCW.BLL.tb_BasketBallList().UpdateThreeScore(ID, model.homefour, model.guestfour);
                            //new BCW.BLL.tb_BasketBallList().UpdateFourScore(ID, model.homefour, model.guestfour);
                            #endregion
                        }
                        //catch (Exception e)
                        //{
                        //    ex += "更新Name_en" + model.name_en + "球赛异常:" + e.ToString() + "#";
                        //}
                        //new BCW.BLL.tb_BasketBallList().UpdateScore(Id);

                      
                    }
                    #endregion

                    mac mac = new mac();
                    mac.h = nodes.InnerText;
                    list.Add(mac);
                }
            }
        }
        return list;
    }


    #endregion


    #region 足球相关函数

    //更新所有球赛
    public void getMatch()
    {
        string s1 = "";
        s1 = GetFootbolist(2, -1);//1已开2未开
        string jinqiu = "";
        string gunqiu = "";
        string bo = s1;
        string[] boTemp = Regex.Split(bo, @"<td class=.W1\s[\w\d]+.>");
        BCW.Model.tb_ZQLists ft = new BCW.Model.tb_ZQLists();
        for (int i = 1; i < boTemp.Length; i++)
        {
            Response.Write("<br/>" + "--------------------------------我是分割线-------------------------------" + "<br/>");
            //取联赛名称
            string title = "";
            string strpattern1 = @"([\s\S]+?)</td><td class=""W2"">";
            Match mtitle1 = Regex.Match(boTemp[i], strpattern1, RegexOptions.IgnoreCase);
            if (mtitle1.Success)
            {
                title = mtitle1.Groups[1].Value;
                title = Regex.Replace(title, @"<.+?>", "");
            }
            //比赛时间
            string Date = "";
            string strpattern2 = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
            Match mtitle2 = Regex.Match(boTemp[i], strpattern2, RegexOptions.IgnoreCase);
            if (mtitle2.Success)
            {
                Date = mtitle2.Groups[1].Value;
                //HttpContext.Current.Response.Write(Date + "<br />");
            }
            string state = getStateForLianSai(boTemp[i]);
            string result = getResult(boTemp[i]);
            //Response.Write("比赛日期:" + Date + "<br />");
            //Response.Write("比赛状态:" + state + "<br />");
            //Response.Write("比分:" + result + "<br />");
            jinqiu = getPeilvForLianSai(boTemp[i]);
            //   if (boTemp[i].Contains("<td>↑滾球</td>"))
            {
                //取p_id
                int p_id = 0;
                string strpattern = @"by=detail&amp;eid=(\d+).>析";
                Match mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_id = Utils.ParseInt(mtitle.Groups[1].Value);
                    //  Response.Write(p_id + "<br />");
                }
                bool bbb = new BCW.BLL.tb_ZQLists().Exists_ft_bianhao(p_id);
                //  Response.Write("联赛名字:" + title + "<br />");

                //取主队名称
                string p_one = "";
                strpattern = @"<td class=""teamname"">([\s\S]+)<a href=""today.aspx";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_one = mtitle.Groups[1].Value.Trim();
                    Response.Write("主队名称:" + p_one + "<br />");
                    //这里取主队红牌数量
                    string strpatternHp = @"<span class=""rc"">(\d)</span>";
                    Match mtitleHp = Regex.Match(p_one, strpatternHp, RegexOptions.IgnoreCase);
                    if (mtitleHp.Success)
                    {
                        int hp_one = Utils.ParseInt(mtitleHp.Groups[1].Value);
                        if (hp_one > 0)
                        {
                            //  Response.Write("主队红牌数量:" + p_one + "<br />");
                        }
                    }

                }
                else
                {
                    p_one = getTeam1ForLianSai(boTemp[i]);
                    // boTemp[i].ToString();                                                       
                    // Response.Write("主队名称:" + p_one + "<br />");
                }
                string team2 = getTeam2ForLianSai(boTemp[i]);
                //   Response.Write("函数客队名称:" + team2 + "<br />");
                //取客队名称
                string p_two = "";
                string tttime = "";
                strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)<small>\[[\w\d]+\]</small>(<span class=""rc"">(\d)</span>)*";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    p_two = mtitle.Groups[0].Value.Trim();
                    p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                    p_two = Regex.Replace(p_two, @"</td>", "");
                    p_two = Regex.Replace(p_two, @"<tr>", "");
                    p_two = Regex.Replace(p_two, @"</tr>", "");
                    p_two = Regex.Replace(p_two, @"<td>", "");
                    p_two = Regex.Replace(p_two, @"<b class=", "");
                    p_two = Regex.Replace(p_two, @"<td class=""teamname"">", "");
                    p_two = Regex.Replace(p_two, @"<tr class=""teamname"">", "");
                    p_two = Regex.Replace(p_two, @"<tr> ""score"" > (\d) - (\d) </ b >", "");
                    //  Response.Write("客队名称if:" + p_two + "<br />");
                    // team2 = p_two;

                    //这里取客队红牌数量
                    string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                    Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                    if (mtitleHp2.Success)
                    {
                        int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                        if (hp_two > 0)
                        {
                            Response.Write("客队红牌数量:" + team2 + "<br />");
                        }
                    }
                }
                else
                {
                    if (mtitle.Success)
                    {
                        p_two = mtitle.Groups[0].Value.Trim();
                        // Response.Write("客队名称else:" + p_two.ToString() + "<br />");
                        p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
                        p_two = Regex.Replace(p_two, @"(\d){2}-(\d){2}</b>", "");
                        p_two = Regex.Replace(p_two, @"<i>(.*)</i>", "");
                        p_two = Regex.Replace(p_two, @"</td>", "");
                        p_two = Regex.Replace(p_two, @"<td>", "");
                        p_two = Regex.Replace(p_two, @"</tr>", "");
                        p_two = Regex.Replace(p_two, @"<tr>", "");
                        p_two = Regex.Replace(p_two, @"<b>", "");
                        p_two = Regex.Replace(p_two, @"<b class=", "");
                        p_two = Regex.Replace(p_two, @"<td class=""teamname"">", "");
                        p_two = Regex.Replace(p_two, @"<tr class=""teamname"">", "");
                        p_two = Regex.Replace(p_two, "score", "");
                        p_two = Regex.Replace(p_two, "\"", "");
                        p_two = Regex.Replace(p_two, @"<td colspan = ""(\d)"" class=""info"">", "");
                        //strpattern1 = @"[\u4e00-\u9fa5]+";
                        //Match mtitle12 = Regex.Match(p_two, strpattern1, RegexOptions.IgnoreCase);
                        //p_two = mtitle12.Groups[0].Value.Trim();
                        //  Response.Write("客队名称elseif之后:" + p_two.ToString() + "<br />");

                        //这里取客队红牌数量
                        string strpatternHp2 = @"<span class=""rc"">(\d)</span>";
                        Match mtitleHp2 = Regex.Match(p_two, strpatternHp2, RegexOptions.IgnoreCase);
                        if (mtitleHp2.Success)
                        {
                            int hp_two = Utils.ParseInt(mtitleHp2.Groups[1].Value);
                            if (hp_two > 0)
                            {
                                // new TPR2.BLL.guess.BaList().Updatep_hp_two(p_id, hp_two);
                                //  Response.Write("客队红牌数量:" + p_two + "<br />");
                            }
                        }
                    }
                    else
                    {
                        p_two = team2;
                        // Response.Write("客队名称elseif之后用函数取得:" + p_two.ToString() + "<br />");
                    }
                }
                // getTeam2ForLianSai(string _html)
                string infomat = getInformationForLianSai(boTemp[i]);
                //  Response.Write("比赛信息：" + infomat + "<br/>");
                strpattern = @"<td>((\d){2}:(\d){2})</td><td class=""teamname"">";
                mtitle = Regex.Match(boTemp[i], strpattern, RegexOptions.IgnoreCase);
                if (mtitle.Success)
                {
                    tttime = mtitle.Groups[1].Value;
                    //HttpContext.Current.Response.Write(Time + "<br />");
                }
                //  Response.Write("比赛时间:" + "<b>" + tttime + "</b><br />");
                ///
                ///获取具体分析的页面
                ///
                //string htmlurl = "http://3g.8bo.com/3g/football/score/today.aspx?st=allEvents&by=detail&eid="+ p_id;
                //string htmltext = "";
                //htmltext = GetHtmlSource(htmlurl,Encoding.UTF8);//分析球赛页面@"<div class='left'>([\s\S]+?)<\/div>"
                //string strpattern0 = @"<div style=""margin: 1em 0;"">[\\s\\S]*</div>";
                //mtitle = Regex.Match(htmltext, strpattern0, RegexOptions.IgnoreCase);
                //string jinqiu = mtitle.Groups[1].Value;
                Response.Write("赔率:" + "<br/><b>" + jinqiu + "</b><br />");
                if (bbb)
                {

                    int idd = new BCW.BLL.tb_ZQLists().GetIdFromBianhao(p_id);
                    BCW.Model.tb_ZQLists ftt = new BCW.BLL.tb_ZQLists().Gettb_ZQLists(idd);
                    if (ftt.ft_result != result)
                    {
                        // string result1 = getResult(result);
                        string strwhere = " FootBallId =" + idd;
                        string text = "";
                        DataSet dds = new BCW.BLL.tb_ZQCollection().GetList(" * ", strwhere);
                        if (dds != null && dds.Tables[0].Rows.Count > 0)
                        {
                            for (int ii = 0; ii < dds.Tables[0].Rows.Count; ii++)
                            {
                                if (dds.Tables[0].Rows[ii]["UsId"].ToString().Length > 2)
                                {
                                    int usid = Convert.ToInt32(dds.Tables[0].Rows[ii]["UsId"]);
                                    if (new BCW.BLL.User().Exists(usid))
                                    {
                                        if (result == "")
                                        {
                                            text = "0-0";
                                        }
                                        string name = new BCW.BLL.User().GetUsName(usid);
                                        string strText = "你收藏的" + "[URL=/bbs/guess2/live.aspx?act=21&amp;Id=" + idd + "]" + ftt.ft_team1 + "VS" + ftt.ft_team2 + "[/URL]变化啦," + "当前比分" + result + "快去看看吧";
                                        new BCW.BLL.Guest().Add(0, usid, name, strText);
                                    }
                                }
                            }
                        }
                    }
                    //  ftt.ft_addTime = DateTime.Now;
                    //  ftt.ft_bianhao = Convert.ToInt32(p_id);
                    // ftt.ft_beiyong = jinqiu;
                    //  ftt.ft_teamStyle = title;
                    ftt.ft_caipan = tttime;
                    //   ftt.ft_otherNews =jinqiu;
                    ftt.ft_didian = "0";
                    ftt.ft_glod = 0;
                    // ftt.ft_hit = 0;
                    ftt.ft_news = infomat;
                    ftt.ft_overTime = DateTime.Now;
                    ftt.ft_result = result;
                    ftt.ft_state = state;
                    // ftt.ft_state1 = "0";
                    // ftt.ft_state2 = "0";
                    //  ftt.ft_team1 = getStringNew(p_one);
                    //  ftt.ft_team1Explain = "0";
                    //   ftt.ft_team2 = getStringNew(p_two);
                    // ftt.ft_team2Explain = "0";
                    //  ftt.ft_time = Convert.ToDateTime(Date);
                    // ftt.Identification = 1;
                    ftt.isDone = 1;
                    new BCW.BLL.tb_ZQLists().Update(ftt);
                    Response.Write("[" + ftt.ft_time + "]" + ftt.ft_teamStyle + ftt.ft_team1 + "--" + ftt.ft_team2 + "赛事更新成功！" + "当前" + ftt.ft_result + "状态" + ftt.ft_state);
                }
                else
                {
                    ft.ft_addTime = DateTime.Now;
                    ft.ft_bianhao = Convert.ToInt32(p_id);
                    ft.ft_beiyong = "0";
                    #region 开始读取竞猜 列表  自动识别隐藏与开启           
                    //string strWhere = "p_type=" + 1 + " and p_active=0 and p_del=0 and (p_TPRtime >= '" + System.DateTime.Now + "' OR (p_ison=1 and p_isondel=0))  and p_basketve=0";
                    //// 开始读取竞猜
                    //DataSet have = new TPR2.BLL.guess.BaList().GetBaListList("*", strWhere);
                    //if (have != null && have.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int il = 0; il < have.Tables[0].Rows.Count; il++)
                    //    {
                    //        if (DT.GetTime(ft.ft_time.ToString(), "MM月dd日").ToString() == DT.GetTime(have.Tables[0].Rows[il]["p_TPRtime"].ToString(), "MM月dd日").ToString())
                    //        {
                    //            if (ft.ft_team1.Contains(have.Tables[0].Rows[il]["p_once"].ToString()) && ft.ft_team2.Contains(have.Tables[0].Rows[il]["p_two"].ToString()))
                    //            {
                    //                if (ft.ft_teamStyle.Contains(have.Tables[0].Rows[il]["p_title"].ToString()))
                    //                {
                    //                    ft.Identification = 1;//模糊识别，1设为显示
                    //                    ft.ft_beiyong = (have.Tables[0].Rows[il]["ID"].ToString());
                    //                }
                    //            }
                    //        }
                    //    }
                    //}

                    #endregion
                    ft.ft_otherNews = jinqiu;
                    ft.ft_teamStyle = title;
                    ft.ft_caipan = tttime;
                    ft.ft_didian = "0";
                    ft.ft_glod = 0;
                    ft.ft_hit = 0;
                    ft.ft_news = infomat;
                    ft.ft_overTime = DateTime.Now;
                    ft.ft_result = result;
                    ft.ft_state = state;
                    ft.ft_state1 = state;
                    ft.ft_state2 = "0";
                    ft.ft_team1 = p_one;
                    ft.ft_team1Explain = "0";
                    ft.ft_team2 = p_two;
                    ft.ft_team2Explain = "0";
                    ft.ft_time = Convert.ToDateTime(Date);
                    ft.Identification = 0;//默认隐藏
                    ft.isDone = 1;
                    new BCW.BLL.tb_ZQLists().Add(ft);
                    Response.Write("[" + ft.ft_time + "]" + ft.ft_teamStyle + ft.ft_team1 + "--" + ft.ft_team2 + "赛事添加成功！");
                }
                //Thread.Sleep(10000);
            }
        }
    }


    public string getStringNew(string message)
    {
        //过滤\n 转换成空
        String result = message.Replace("\n", "");
        //过滤\r 转换成空
        result = result.Replace("\r", "");
        //过滤\t 转换成空
        result = result.Replace("\t", "");
        //过滤\ 转换成空
        result = result.Replace("\\", "");
        //获取html中的body标签
        //   String result = Regex.Match(newString, @"<body.*>.*</body>").ToString();
        //过滤注释
        result = Regex.Replace(result, @"<!--(?s).*?-->", "", RegexOptions.IgnoreCase);
        //过滤nbsp标签
        result = Regex.Replace(result, @"&nbsp;", "", RegexOptions.IgnoreCase);
        //过滤nbsp标签   /></td></tr><tr><td rowspan="3" align="center">
        //<b>滾球</b></td><td>↑让球</td><td>0.77 <i>平手</i> 1.12
        //</td></tr><tr><td>↑大小</td><td>0.95 <i>1.5/2</i> 0.91
        //</td></tr><tr><td>↑标准</td><td>1.21 4.75 15.00</td></tr><tr>
        //  />↑让球0.75 <i>平手</i> 1.14↑大小0.80 <i>2.5</i> 1.06↑标准1.02 11.50 28.00
        //<table cellspacing="3" cellpadding="0" border="0" class="detail">
        //<td align="right" class="type">进球<td align="right" class="name">Valdez<td align="center">
        //[18']<td align="right" class="type">进球<td align="right" class="name">Sambueza<td align="center">[67']
        //</table>
        result = Regex.Replace(result, @"</td>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"//", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</tr>;", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<tr>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</tr>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"/>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td rowspan=""3"" align=""center"">", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"<td colspan=""3"">", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</table>", "", RegexOptions.IgnoreCase);
        result = Regex.Replace(result, @"</spa>", "", RegexOptions.IgnoreCase);
        // result = Regex.Replace(result, @"<b>滾球</b>", "", RegexOptions.IgnoreCase);

        return result;
    }

    // uid=[0-9]\\d*
    /// 获取字符中指定标签的值  
    /// </summary>  
    /// <param name="str">字符串</param>  
    /// <param name="title">标签</param>  
    /// <returns>值</returns>  
    public static string GetTitleContent(string str, string title)
    {
        string tmpStr = string.Format("<{0}[^>]*?>(?<Text>[^<]*)</{1}>", title, title); //获取<title>之间内容  
        Match TitleMatch = Regex.Match(str, tmpStr, RegexOptions.IgnoreCase);
        string result = TitleMatch.Groups["Text"].Value;
        return result;
    }

    /// <summary>
    /// 得到列表的A标签中的地址
    /// </summary>
    /// <param name="p_html">HTML文档</param>
    private string FootbzlistHtml(string p_html)
    {
        //if (string.IsNullOrEmpty(p_html))
        //    return "";
        string s = "";
        // System.Text.StringBuilder builder = new System.Text.StringBuilder("");
        MatchCollection mc = Regex.Matches(p_html, @"[a-zA-z]+://[^\s]*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (mc.Count > 0)
        {
            for (int i = 0; i < mc.Count; i++)
            {
                s += "#" + mc[i].Groups[1].Value.ToString();
                // Response.Write("#" + mc[i].Groups[1].Value.ToString());
            }
        }
        return s;
    }

    /// <summary>
    /// 处理完场比分内容
    /// </summary>
    /// <param name="p_html">HTML文档</param>
    private string FootoverHtml(string p_html)
    {
        if (string.IsNullOrEmpty(p_html))
            return "";

        string pattern = @"场&gt;&gt;</b><br/>([\s\S]+?)</p><p align=.center.>";
        Match m = Regex.Match(p_html, pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (m.Success)
        {
            p_html = m.Groups[1].Value;
        }

        System.Text.StringBuilder builder = new System.Text.StringBuilder("");
        MatchCollection mc = Regex.Matches(p_html, @"<small>([\s\S]+?)</small>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        if (mc.Count > 0)
        {
            for (int i = 0; i < mc.Count; i++)
            {
                builder.Append("#" + mc[i].Groups[1].Value.ToString());
            }
        }

        return builder.ToString();
    }

    ///定义写入流操作 
    public string WriteStream(string _url)
    {
        HttpWebRequest httpReq;
        HttpWebResponse httpResp;

        string strBuff = "";
        char[] cbuffer = new char[256];
        int byteRead = 0;
        //  string _url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&refresh=stop&st=notStart&pagesize=1000";
        string filename = @"c:\log.txt";
        Uri httpURL = new Uri(_url);

        ///HttpWebRequest类继承于WebRequest，并没有自己的构造函数，需通过WebRequest的Creat方法 建立，并进行强制的类型转换 
        httpReq = (HttpWebRequest)WebRequest.Create(httpURL);
        ///通过HttpWebRequest的GetResponse()方法建立HttpWebResponse,强制类型转换

        httpResp = (HttpWebResponse)httpReq.GetResponse();
        ///GetResponseStream()方法获取HTTP响应的数据流,并尝试取得URL中所指定的网页内容

        ///若成功取得网页的内容，则以System.IO.Stream形式返回，若失败则产生ProtoclViolationException错 误。在此正确的做法应将以下的代码放到一个try块中处理。这里简单处理 
        Stream respStream = httpResp.GetResponseStream();

        ///返回的内容是Stream形式的，所以可以利用StreamReader类获取GetResponseStream的内容，并以

        //StreamReader类的Read方法依次读取网页源程序代码每一行的内容，直至行尾（读取的编码格式：UTF8） 
        StreamReader respStreamReader = new StreamReader(respStream, Encoding.UTF8);

        byteRead = respStreamReader.Read(cbuffer, 0, 256);

        while (byteRead != 0)
        {
            string strResp = new string(cbuffer, 0, byteRead);
            strBuff = strBuff + strResp;
            byteRead = respStreamReader.Read(cbuffer, 0, 256);
        }
        respStream.Close();
        //txtHTML.Text = strBuff;
        //  Response.Write(strBuff);


        return strBuff;




    }

    private string _ResponseValue = string.Empty;
    private string _CacheFolder = "~/Files/Cache/live/getzq/";
    private bool _CacheUsed = false; //是否记录缓存/存TXT
    private int _CacheTime = 5;//缓存时间(分钟)

    /// <summary>
    /// 取得足球XML
    /// </summary>
    public string GetFootbolist(int Types, int Page)
    {

        //足球的：赛事更新、水位变动、全场滚球（包括封盘）、即时比分、完场比分（加返彩）、比赛进行时间

        //篮球的：赛事、水位、全场滚球（包括封盘）、即时比分、完场比分返彩、比赛进行时间

        string obj = string.Empty;

        string url = string.Empty;
        if (Types == 1)
        {
            //  http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasStart
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasStart&refresh=stop&pagesize=1000";//已开赛
        }
        else if (Types == 2)
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&refresh=start&st=notStart&pagesize=1000";//未开赛
        }
        else if (Types == 3)
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=hasCompletedField&pagesize=1000";//已完场
        }
        //  url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=allEvents&page=" + Page + "";//所有比赛
        if (Page == -1)
        {
            url = "http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=allEvents&refresh=stop&pagesize=1000";
            //  http://3g.8bo.com/3g/football/score/today.aspx?by=event&st=allEvents
        }
        HttpRequestCache httpRequest = new HttpRequestCache(url);
        httpRequest.Fc.CacheUsed = this._CacheUsed;
        httpRequest.Fc.CacheTime = this._CacheTime;
        httpRequest.Fc.CacheFolder = this._CacheFolder;
        httpRequest.Fc.CacheFile = "FOOT8波XML" + Page + "";
        httpRequest.WebAsync.RevCharset = "UTF-8";
        if (httpRequest.MethodGetUrl(out this._ResponseValue))
        {
            if (this._ResponseValue.Contains(">尾页</a>]"))
                obj = FootbolistHtml(this._ResponseValue) + "#NEXT#";
            else
                obj = FootbolistHtml(this._ResponseValue);
        }

        return obj;
    }

    /// <summary>
    /// 处理足球XML
    /// </summary>
    /// <param name="p_html">HTML文档</param>
    private string FootbolistHtml(string p_html)
    {
        if (string.IsNullOrEmpty(p_html))
            return "";

        string str = string.Empty;
        string pattern = @"<table cellspacing=""0"" cellpadding=""0"" border=""0"" class=""events odds"">([\s\S]+?)</table>";
        Match m = Regex.Match(p_html, pattern, RegexOptions.IgnoreCase);
        if (m.Success)
        {
            str = m.Groups[1].Value;
        }

        return str;
    }

    //取联赛名称
    private string getNameForLianSai(string _html)
    {
        string title = "";
        string strpattern = @"([\s\S]+?)</td><td class=""W2"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            title = mtitle.Groups[1].Value;
            title = Regex.Replace(title, @"<.+?>", "");
        }
        return title;
    }
    //取比赛信息
    private string getInformationForLianSai(string _html)
    {
        string strState = "";
        string strpattern = @"<td colspan=""2"" class=""info"">([\s\S]+?)</td>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
        }
        return strState;
    }
    //取比赛状态
    private string getStateForLianSai(string _html)
    {
        string strState = "";
        string strpattern = @"<td align=""center"">([\s\S]{1,10})</td>";//未、点(这个是代表点球)、完、待定、腰斩、推迟（无这些选项则取的比赛进行分钟数）
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
        }
        return strState;
    }
    //取比赛日期
    private string getDateForLianSai(string _html)
    {
        string Date = "";
        string strpattern = @"<td class=""W2"">((\d){2}-(\d){2})</td><td class=""teamname"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            Date = mtitle.Groups[1].Value;
            //HttpContext.Current.Response.Write(Date + "<br />");
        }
        return Date;
    }
    //取主队名称
    private string getTeam1ForLianSai(string _html)
    {
        string p_one = "";
        string strpattern = @"<td class=""teamname"">([\s\S]+)<td class=""teamname"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            p_one = mtitle.Groups[1].Value.Trim();
            p_one = Regex.Replace(p_one, @"<small>\[[\s\S]+\]</small>", "");
            p_one = Regex.Replace(p_one, @"<span class=""rc"">[\w\d]+</span>", "");
            p_one = Regex.Replace(p_one, @"<.+?>", "");
            p_one = Regex.Replace(p_one, @"析", "");
            p_one = Regex.Replace(p_one, @"^[1-9]\d*$", "");
            p_one = Regex.Replace(p_one, @"\d", "");
            p_one = Regex.Replace(p_one, @"'", "");
            p_one = Regex.Replace(p_one, @":", "");
            p_one = Regex.Replace(p_one, @"完", "");
        }
        return p_one;
    }
    //取客队名称
    private string getTeam2ForLianSai(string _html)
    {
        //取客队名称
        string p_two = "";
        string strpattern = @"<td>(\d){2}:(\d){2}</td><td class=""teamname"">([\s\S]+)</td></tr>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            p_two = mtitle.Groups[0].Value.Trim();
            string[] p_twoTemp = Regex.Split(p_two, @"<tr class=""alternation"">");

            p_two = Regex.Replace(p_twoTemp[0], @"<small>\[[\s\S]+\]</small>", "");
            p_two = Regex.Replace(p_two, @"<span class=""rc"">[\w\d]+</span>", "");
            if (p_two.Contains("↑"))
            {
                p_two = Regex.Split(p_two, "↑")[0];
            }
            p_two = Regex.Replace(p_two, @"<td>(\d){2}:(\d){2}</td>", "");
            p_two = Regex.Replace(p_two, @"<td class=""teamname"">", "");
            p_two = Regex.Replace(p_two, @"<td>", "");
            p_two = Regex.Replace(p_two, @"<tr>", "");
            p_two = Regex.Replace(p_two, @"</td>", "");
            p_two = Regex.Replace(p_two, @"</tr>", "");
            p_two = Regex.Replace(p_two, @"<b class=""score"">\[[\s\S]+\]</b>", "");
            p_two = Regex.Replace(p_two, @"<td", "");
            p_two = Regex.Replace(p_two, @"colspan=""(\d){2}"" class=", "");
            p_two = Regex.Replace(p_two, @"<td colspan = ""(\d)"" class=""info"">", "");
            //  p_two = Regex.Replace(p_two, @"/^[a-z\d] +$/", "");
            string strpattern1 = @"[\u4e00-\u9fa5]+";
            Match mtitle12 = Regex.Match(p_two, strpattern1, RegexOptions.IgnoreCase);
            p_two = mtitle12.Groups[0].Value.Trim();
            //  string strText = System.Text.RegularExpressions.Regex.Replace(p_two, "<[^>]+>", "");
            //  p_two = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");
            // p_two = Regex.Replace(p_two, @"<b class=""score"">\[[\s\S]+\]</b>", "");
            // 平顺 < b class="score">0-0</b>
        }
        return p_two;
    }
    //取赔率
    private string getPeilvForLianSai(string _html)
    {
        //取赔率
        //滾球0.87 <i>平/半</i> 1.01<br/><tr class="alternation">↑大小1.02 <i>2.5/3</i> 0.84<br/><tr class="alternation">↑标准4.75 3.55 1.70<br/><tr class="alternation"></td>
        //滾球1.01 <i>受平手</i> 0.81<br/>↑大小0.90 <i>1</i> 0.90<br/>↑标准3.30 2.12 2.88<br/>
        //↑让球0.80 <i>受半球</i> 0.96<br/>↑大小0.87 <i>3.5</i> 0.89<br/>↑标准2.65 3.80 1.96<br/>
        string strState = "";
        string strpattern = @"<td>↑([\s\S]+?)<td colspan=""2"" class=""info"">";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            strState = mtitle.Groups[1].Value.Trim();
            strState = Regex.Replace(strState, @"</td><td>", "");
            strState = Regex.Replace(strState, @"</td></tr>", "<br/>");
            strState = Regex.Replace(strState, @"<td class=""alternation"">", "");
            strState = Regex.Replace(strState, @"<tr class=""alternation"">", "");
            strState = Regex.Replace(strState, @"<td>", "");
            strState = Regex.Replace(strState, @"<tr>", "");
            strState = Regex.Replace(strState, @"</tr>", "");
            strState = Regex.Replace(strState, @"</td>", "");
        }
        return ("↑" + strState);
    }

    //这里即时完场比分
    private string getResult(string _html)
    {
        string Result = "";
        string strpattern = @"<b class=""score"">((\d){1,2}-(\d){1,2})</b>";
        Match mtitle = Regex.Match(_html, strpattern, RegexOptions.IgnoreCase);
        if (mtitle.Success)
        {
            //取即时比分
            Result = mtitle.Groups[1].Value;
            int p_id = 811960183;
            string strState = getStateForLianSai(_html);
            if (Result.Contains("-"))
            {
                try
                {
                    string[] p_result = Result.Split('-');
                    if (strState == "完")
                    {
                        int p_result_one = Convert.ToInt32(p_result[0]);
                        int p_result_two = Convert.ToInt32(p_result[1]);
                        //  new TPR2.BLL.guess.BaList().UpdateBoResult(p_id, p_result_one, p_result_two);

                    }
                    else
                    {
                        int p_result_temp1 = Convert.ToInt32(p_result[0]);
                        int p_result_temp2 = Convert.ToInt32(p_result[1]);
                        TPR2.Model.guess.BaList bf = new TPR2.BLL.guess.BaList().GetTemp(p_id);
                        if (bf != null)
                        {
                            if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2) { }
                            //   new TPR2.BLL.guess.BaList().UpdateBoResult2(p_id, p_result_temp1, p_result_temp2);

                        }
                        //更新半场即时比分
                        bf = new TPR2.BLL.guess.BaList().GetTemp(p_id, 9);
                        if (bf != null)
                        {
                            if (bf.p_result_temp1 != p_result_temp1 || bf.p_result_temp2 != p_result_temp2)
                            {
                                //    new TPR2.BLL.guess.BaList().UpdateBoResultHalf(p_id, p_result_temp1, p_result_temp2);

                            }

                        }
                    }
                }
                catch { }
            }
        }
        return Result;
    }

    /// <summary>
    /// 获取网页HTML源码
    /// </summary>
    /// <param name="url">链接 eg:http://www.baidu.com/ </param>
    /// <param name="charset">编码 eg:Encoding.UTF8</param>
    /// <returns>HTML源码</returns>
    public static string GetHtmlSource1(string url, Encoding charset)
    {

        string _html = string.Empty;
        try
        {
            HttpWebRequest _request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse _response = (HttpWebResponse)_request.GetResponse();
            using (Stream _stream = _response.GetResponseStream())
            {
                using (StreamReader _reader = new StreamReader(_stream, charset))
                {
                    _html = _reader.ReadToEnd();
                }
            }
        }
        catch (WebException ex)
        {
            using (StreamReader sr = new StreamReader(ex.Response.GetResponseStream()))
            {
                _html = sr.ReadToEnd();
            }
            // _html = ex.ToString();
        }
        catch (Exception ex)
        {
            _html = ex.Message;
        }
        return _html;
    }

    #endregion
}

