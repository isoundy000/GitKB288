using System;
using System.Collections.Generic;
using System.Text;
using BCW.PK10.Model;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.SqlClient;
using BCW.Data;
using System.Collections;
using BCW.Common;
using System.Xml;

namespace BCW.PK10
{
    public enum PK10_Stutas { 未知=0,待售=1,在售=2,待开奖=3,已开奖=4};
    public class PK10
    {
        public int myPublishGameType = 1030;//公共的游戏类型ID
        public string myPublishGameName = "PK拾";
        public string xmlPath = "/Controls/PK10.xml";
        //C正则表达式在线测试工具 http://tool.oschina.net/regex
        //PK10开奖最快的网站 http://www.pk10we.net
        #region 抓取网页数据
        #region 抓取
        public string GetHtmlByURL()//抓取页面源码
        {
            return GetHtmlByURL1();
        }
        public PK10_List GetCurrentOpenDataByURL()//抓取最新开奖数据
        {
            return GetCurrentOpenDataByURL1();
        }
        public PK10_List GetCurrentOpenDataByHtml(string html)//抓取最新开奖数据
        {
            return GetCurrentOpenDataByHtml1(html);
        }
        public IList<PK10_List> GetLatestOpenDataByURL()//抓取今天已经开奖的历史数据
        {
            return GetLatestOpenDataByURL1();
        }
        public IList<PK10_List> GetLatestOpenDataByHtml(string html)//抓取今天已经开奖的历史数据
        {
            return GetLatestOpenDataByHtml1(html);
        }
        public PK10_List GetCurrentSaleDataByURL()//抓取当期数据
        {
            return GetCurrentSaleDataByURL1();
        }
        public PK10_List GetCurrentSaleDataByHtml(string html)//抓取当期数据
        {
            return GetCurrentSaleDataByHtml1(html);
        }
        #endregion
        #region 从付费端口抓取
        public string GetHtmlByURL2()//抓取apiplus.net页面源码
        {
            return Common.GetSourceTextByUrl("http://c.apiplus.net/newly.do?token=5a8ebe52461354bc&code=bjpk10", "UTF-8");
        }
        public PK10_List GetCurrentOpenDataByURL2()//抓取最新一期开奖数据
        {
            string html = GetHtmlByURL2();
            return GetCurrentOpenDataByHtml2(html);
        }
        public PK10_List GetCurrentOpenDataByHtml2(string html)//抓取最新一期开奖数据
        {
            PK10_List list = new PK10_List();
            //
            try
            {
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(html);
                XmlElement root = null;
                root = xml.DocumentElement;

                XmlNodeList listNodes = null;
                listNodes = root.SelectNodes("row");
                int newid = 0;
                string newNums = "", newOpenTime = "";
                foreach (XmlNode node in listNodes)
                {
                    int id = 0;
                    int.TryParse(node.Attributes["expect"].Value.ToString(),out id);
                    string nums = node.Attributes["opencode"].Value;
                    string opentime= node.Attributes["opentime"].Value;
                    if(id>newid)
                    {
                        newid = id;
                        newNums = nums;
                        newOpenTime = opentime;
                    }
                }
                newNums = CheckAndGetNums(newNums, ',');
                if (newid>0 && newNums!="")
                {
                    list.No = newid.ToString().ToString().Trim();
                    list.Nums = newNums;
                }
                else
                {
                    list= null;
                }
                //
            }
            catch (Exception ex)
            {
                list = null;
                throw new Exception(ex.Message);
            }
            //          
            return list;
        }
        #endregion
        //
        #region 从baidu.lecai.com抓取
        public string CheckAndGetNums(string fromNums,char splitChar)
        {
            string[] anums = fromNums.Split(splitChar);
            if (fromNums.Length==0 || anums.Length != 10)
                return "";
            //  "01", "02", "03", "04", "05", "06", "07", "08", "09", "10"  //标准号码
            //
            List<string> lnums = new List<string>();
            for (int i = 0; i < anums.Length; i++)
            {
                int num = 0;
                int.TryParse(anums[i].ToString(), out num);
                if (num > 0 && num <= 10)
                {
                    string cnum = num.ToString().Trim();
                    if (cnum.Length == 1)
                        cnum = "0" + cnum;
                    else
                        cnum = cnum.Substring(0, 2);
                    //
                    if (!lnums.Contains(cnum))
                        lnums.Add(cnum);
                }
            }
            if (lnums.Count != 10)
                return "";
            //
            string result = "";
            for(int i=0;i<10;i++)
            {
                if (i == 0)
                    result = lnums[i];
                else
                    result += "," + lnums[i];
            }
            //
            return result;
        }
        public string GetHtmlByURL1()//抓取baidu.lecai.com页面源码
        {
           return Common.GetSourceTextByUrl("http://baidu.lecai.com/lottery/draw/view/557", "UTF-8");
        }
        public PK10_List GetCurrentOpenDataByURL1( )//抓取最新开奖数据
        {
            string html = GetHtmlByURL1();
            return GetCurrentOpenDataByHtml1(html);
        }
        public PK10_List GetCurrentOpenDataByHtml1(string html)//抓取最新开奖数据
        {
            PK10_List list = new PK10_List();
            //
            try
            {
                string latest_draw_result = Common.GetPageUtf8Html(html, @" var latest_draw_result", ";");
                #region 解析Json数据[net2.0 没有Json，需要第三方控件；暂时不用]
                string cnums = Common.GetValueFromStr(latest_draw_result, @"""red"":\[", @"]");
                cnums = cnums.Replace('"', ' ');
                cnums = CheckAndGetNums(cnums, ',');
                if (cnums == "")
                    return null;
                list.Nums = cnums;
                #endregion
                string latest_draw_phase = Common.GetPageUtf8Html(html, @" var latest_draw_phase", ";");
                latest_draw_phase = Common.GetValueFromStr(latest_draw_phase, "= '", "'");
                latest_draw_phase=latest_draw_phase.Replace('\'', ' ');
                list.No = latest_draw_phase;
                string latest_draw_time = Common.GetPageUtf8Html(html, @" var latest_draw_time", ";");
                latest_draw_time = Common.GetValueFromStr(latest_draw_time, "= '", "'");
                latest_draw_time=latest_draw_time.Replace('\'', ' ');
                list.Date = DateTime.Parse(DateTime.Parse(latest_draw_time).ToShortDateString());
                list.BeginTime = DateTime.Parse(latest_draw_time);
                list.EndTime = DateTime.Parse(latest_draw_time);
                //
            }
            catch(Exception ex)
            {
                list = null;
                throw new Exception(ex.Message);
            }
            //          
            return list;
        }
        public IList<PK10_List> GetLatestOpenDataByURL1()//抓取今天已经开奖的历史数据
        {
            string html = GetHtmlByURL1();
            return GetLatestOpenDataByHtml1(html);
        }
        public IList<PK10_List> GetLatestOpenDataByHtml1(string html)//抓取今天已经开奖的历史数据
        {
            IList<PK10_List> lists = new List<PK10_List>();
            //
            try
            {
                string phaseData = Common.GetPageUtf8Html(html, @" var phaseData", ";");
                //
                string cDateFlag = "([0-9]{3}[1-9]|[0-9]{2}[1-9][0-9]{1}|[0-9]{1}[1-9][0-9]{2}|[1-9][0-9]{3})-(((0[13578]|1[02])-(0[1-9]|[12][0-9]|3[01]))|((0[469]|11)-(0[1-9]|[12][0-9]|30))|(02-(0[1-9]|[1][0-9]|2[0-8])))";//日期格式（-符合）
                string cDate = Common.GetValueFromStr(phaseData, @"\{""" + cDateFlag + @"""\:"); //截取出{"XXXX-XX-XX":
                cDate = Common.GetValueFromStr(cDate, @"\{""", @"""\:");//截取出日期部分
                DateTime date = DateTime.Parse(cDate);
                //
                string cFlag1 = @"[0-9]{6}"; //期号的正则表达式（6位纯数字）
                string cFlag2 = @"[\s\S]+?";//所有字符（可多个）的表达式
                #region 解析Json数据[net2.0 没有Json，需要第三方控件；暂时不用]
                //每一期的开始，例如{ "560973":{ "result":{ "red":["01","07","09","03","02","10","06","04","08","05"],
                string[] cnums = Common.GetValuesFromStr(phaseData, @"""" + cFlag1 + @"""\:" + cFlag2 + @"\]");
                if (cnums != null && cnums.Length > 0)
                {
                    for (int i = 0; i < cnums.Length; i++)
                    {
                        PK10_List list = new PK10_List();
                        string str = cnums[i];
                        //
                        list.Date = date;
                        list.BeginTime = date;
                        list.EndTime = date;
                        list.No = Common.GetValueFromStr(str, cFlag1);
                        //
                        string cnum = Common.GetValueFromStr(str, @"""red""\:\[", @"\]");//"red":["01","07","09","03","02","10","06","04","08","05"]
                        cnum = cnum.Replace('"', ' ');
                        cnum = CheckAndGetNums(cnum, ',');
                        if (cnum != "")
                        {
                            list.Nums = cnum;
                            lists.Add(list);
                        }
                    }
                }
                #endregion
                //
            }
            catch (Exception ex)
            {
                //lists = null;
                throw new Exception(ex.Message);
            }
            //
            return lists;
        }
        public PK10_List GetCurrentSaleDataByURL1()//抓取当期数据
        {
            string html = GetHtmlByURL1();
            return GetCurrentSaleDataByHtml1(html);
        }
        public PK10_List GetCurrentSaleDataByHtml1(string html)//抓取当期数据
        {
            PK10_List list = new PK10_List();
            //
            try
            {
                string latest_draw_result = Common.GetPageUtf8Html(html, @" var currentPhaseCache", ";");
                #region 解析Json数据[net2.0 没有Json，需要第三方控件；暂时不用]
                list.No= Common.GetValueFromStr(latest_draw_result, @"""phase"":""", @""",");
                list.BeginTime = DateTime.Parse(Common.GetValueFromStr(latest_draw_result, @"""time_startsale"":""", @""","));
                list.EndTime = DateTime.Parse(Common.GetValueFromStr(latest_draw_result, @"""time_endsale"":""", @""","));
                list.Date = DateTime.Parse(list.BeginTime.ToShortDateString());
                list.OpenFlag = 0;
                #endregion
            }
            catch (Exception ex)
            {
                list = null;
                throw new Exception(ex.Message);
            }
            //          
            return list;
        }
        #endregion
        #endregion
        //
        #region 读取数据函数
        public int GetTodayFistCreateNo() //读取当前第一期要生成的号码
        {
            int no = 0;
            //
            try
            {
                DateTime lastDate = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(-1);
                string cSQL = "Select Top 1 no From tb_PK10_List Where Date='" + lastDate.ToShortDateString().Trim() + "' order by begintime desc";
                PK10_List list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
                if(list!=null)
                {
                    no = int.Parse(list.No) + 1;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取当前第一期要生成的号码 Error:" + ex.Message);
            }
            //
            return no;
        }
        public PK10_Base GetSaleBase()//读取销售基础数据
        {
            PK10_Base list = new PK10_Base();
            try
            {
                string cSQL = "Select Top 1 * From tb_PK10_Base Where ID=0";
                list = ModelHelper.GetModelBySql<PK10_Base>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetSaleBase Error:" + ex.Message);
            }
            return list;
        }
        public PK10_List GetCurrentSaleData() //读取当前开售记录
        {
            PK10_List list = new PK10_List();
            try
            {
                string now = DateTime.Now.ToString();
                string cSQL = "Select Top 1 * From tb_PK10_List Where BeginTime<='" + now + "' and EndTime>'" + now + "'";
                list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
            }
            catch(Exception ex)
            {
                throw new Exception("GetCurrentSaleData Error:" + ex.Message);
            }
            return list;
        }
        public PK10_List GetLastOpenData() //读取最后一开奖记录
        {
            PK10_List list = new PK10_List();
            try
            {
                string cSQL = "Select Top 1 * From tb_PK10_List Where OpenFlag=1 Order by BeginTime Desc";
                list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
            }
            catch(Exception ex)
            {
                throw new Exception("GetLastOpenData Error:" + ex.Message);
            }
            return list;
        }
        public PK10_List GetTobeOpenData() //读取准备开奖的记录
        {
            PK10_List list = new PK10_List();
            try
            {
                string now = DateTime.Now.ToString();
                string cSQL = "Select Top 1 * From tb_PK10_List Where OpenFlag=0 and beginTime<'" + now + "' and endTime>'"+DateTime.Now.AddMinutes(-30).ToString()+"' Order by BeginTime";
                list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
            }
            catch(Exception ex)
            {
                throw new Exception("GetTobeOpenData Error:" + ex.Message);
            }
            return list;
        }
        public List<PK10_List> GetRecentData(int recordCount) //返回最近几期数据（不论状态）
        {
            List<PK10_List> list = new List<PK10_List>();
            #region
            try
            {
                if (recordCount > 0)
                {
                    string now = DateTime.Now.ToString();
                    string cSQL = "Select Top " + recordCount.ToString() + " * from tb_PK10_List Where BeginTime<='" + now + "' Order by BeginTime Desc";
                    DataTable dt = MySqlHelper.GetTable(cSQL);
                    list = ModelHelper.DataTableToModel<PK10_List>(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetRecentData Error:" + ex.Message);
            }
            #endregion
            return list;
        }
        public List<PK10_List> GetOpenDatas(DateTime beginDate, DateTime endDate,int pageSize,int pageIndex,out int recordCount)//读取某天开奖记录(分页）
        {
            List<PK10_List> list = new List<PK10_List>();
            #region
            try
            {
                string sTable = "tb_PK10_List";
                string sPkey = "id";
                string sField = "ID,Date,No,Nums,PayCount,openflag,calcflag";
                string sCondition = "OpenFlag=1";
                if (beginDate > DateTime.MinValue)
                    sCondition += " And Date>='" + beginDate.ToShortDateString() + "'";
                if (endDate < DateTime.MaxValue)
                    sCondition += " And Date<'" + endDate.AddDays(1).ToShortDateString() + "'";
                string sOrder = "No DESC";
                recordCount = 0; //输出总记录数
                string ids = "";
                using (SqlDataReader reader = MySqlHelper.GetPageData(sTable, sPkey, sField,  sCondition, sOrder,pageIndex, pageSize,out recordCount))
                {
                    while (reader.Read())
                    {
                        PK10_List item = new PK10_List();
                        item.ID = reader.GetInt32(0);
                        item.Date = reader.GetDateTime(1);
                        item.No = reader.GetString(2);
                        item.Nums = reader.GetString(3);
                        item.PayCount = reader.GetInt32(4);
                        item.OpenFlag = reader.GetDecimal(5);
                        item.CalcFlag = reader.GetDecimal(6);
                        list.Add(item);
                        //
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids += "'" + item.ID.ToString().Trim() + "'";
                    }
                }
                #region 生成会员的下注总额
                if (!string.IsNullOrEmpty(ids))
                {
                    string cSQL = "Select A.listID,sum(A.PayMoney) as PayMoney From tb_PK10_Buy as A Left join tb_User as B on A.uid=B.ID Where A.listid in(" + ids + ") and B.IsSpier=0  group by A.listid";
                    DataTable dt = MySqlHelper.GetTable(cSQL);
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            int listid = int.Parse(dr["ListID"].ToString());
                            int paymoney = 0;
                            int.TryParse(dr["PayMoney"].ToString(), out paymoney);
                            PK10_List item = list.Find(delegate (PK10_List o) { return o.ID == listid; });
                            if (item != null)
                                item.PayMoney = paymoney;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("GetOpenDatas Error:"+ex.Message);
            }
            #endregion
            //
            return list;
        }
        public List<PK10_List> GetListDatasByDate(DateTime beginDate, DateTime endDate, int pageSize, int pageIndex, out int recordCount)//读取某天期号记录(分页）
        {
            List<PK10_List> list = new List<PK10_List>();
            #region
            try
            {
                string sTable = "tb_PK10_List";
                string sPkey = "id";
                string sField = "ID,Date,No,Nums,PayCount,openflag,calcflag";
                string sCondition = " BeginTime<'" + DateTime.Now.ToString() + "'";
                if (beginDate > DateTime.MinValue)
                    sCondition += " And Date>='" + beginDate.ToShortDateString() + "'";
                if (endDate < DateTime.MaxValue)
                    sCondition += " And Date<'" + endDate.AddDays(1).ToShortDateString() + "'";
                string sOrder = "No DESC";
                recordCount = 0; //输出总记录数
                string ids = "";
                using (SqlDataReader reader = MySqlHelper.GetPageData(sTable, sPkey, sField, sCondition, sOrder, pageIndex, pageSize, out recordCount))
                {
                    while (reader.Read())
                    {
                        PK10_List item = new PK10_List();
                        item.ID = reader.GetInt32(0);
                        item.Date = reader.GetDateTime(1);
                        item.No = reader.GetString(2);
                        item.Nums = reader.GetString(3);
                        item.PayCount = reader.GetInt32(4);
                        item.OpenFlag = reader.GetDecimal(5);
                        item.CalcFlag = reader.GetDecimal(6);
                        list.Add(item);
                        //
                        if (!string.IsNullOrEmpty(ids))
                            ids += ",";
                        ids+="'"+item.ID.ToString().Trim()+"'";
                    }
                }
                #region 生成会员的下注总额
                if (!string.IsNullOrEmpty(ids))
                {
                    string cSQL = "Select A.listID,sum(A.PayMoney) as PayMoney From tb_PK10_Buy as A Left join tb_User as B on A.uid=B.ID Where A.listid in(" + ids + ") and B.IsSpier=0  group by A.listid";
                    DataTable dt = MySqlHelper.GetTable(cSQL);
                    if(dt!=null)
                    {
                        foreach(DataRow dr in dt.Rows)
                        {
                            int listid = int.Parse(dr["ListID"].ToString());
                            int paymoney = 0;
                            int.TryParse(dr["PayMoney"].ToString(), out paymoney);
                            PK10_List item = list.Find(delegate (PK10_List o) { return o.ID == listid; });
                            if (item != null)
                                item.PayMoney = paymoney;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("GetListDatasByDate Error:" + ex.Message);
            }
            #endregion
            //
            return list;
        }
        public List<PK10_Buy> GetWinDatas(int listid,bool onlyShowWin, int pageSize, int pageIndex, out int recordCount)//读取某一期的中奖明细(分页）
        {
            List<PK10_Buy> list = new List<PK10_Buy>();
            #region
            try
            {
                string sTable = "tb_PK10_Buy";
                string sPkey = "id";
                string sField = "id,uid,uname,listid,winmoney,buytime,istest,paymoney,listnums,buyDescript,buyprice,buycount";
                string sCondition = "ListID=" + listid.ToString().Trim();
                if(onlyShowWin)
                    sCondition += " And WinMoney>0";
                string sOrder = "BuyTime";
                recordCount = 0; //输出总记录数
                using (SqlDataReader reader = MySqlHelper.GetPageData(sTable, sPkey, sField, sCondition, sOrder, pageIndex, pageSize, out recordCount))
                {
                    while (reader.Read())
                    {
                        PK10_Buy item = new PK10_Buy();
                        item.ID = reader.GetInt32(0);
                        item.uID = reader.GetInt32(1);
                        item.uName = reader.GetString(2);
                        item.ListID = reader.GetInt32(3);
                        item.WinMoney = reader.GetInt32(4);
                        item.BuyTime= reader.GetDateTime(5);
                        item.isTest= reader.GetDecimal(6);
                        item.PayMoney = reader.GetInt32(7);
                        item.ListNums = reader.GetString(8);
                        item.BuyDescript = reader.GetString(9);
                        item.BuyPrice = reader.GetInt32(10);
                        item.BuyCount = reader.GetInt32(11);
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetWinDatas Error:" + ex.Message);
            }
            #endregion
            //
            return list;
        }
        public List<PK10_Buy> GetTobeCaseDatas(int uid, int pageSize, int pageIndex, out int recordCount)//读取用户待兑奖的数据(过有效期的也不能兑奖)（分页）
        {
            List<PK10_Buy> list = new List<PK10_Buy>();
            #region
            try
            {
                string sTable = "tb_PK10_Buy";
                string sPkey = "id";
                string sField = "id,uid,uname,listid,listno,listnums,buyDescript,buyprice,paymoney,winmoney,buytime,istest,buycount";
                string sCondition = "uID=" + uid.ToString().Trim() + " And WinMoney>0 and CaseFlag=0 and ValidFlag=1";
                string sOrder = "BuyTime DESC";
                recordCount = 0; //输出总记录数
                using (SqlDataReader reader = MySqlHelper.GetPageData(sTable, sPkey, sField, sCondition, sOrder, pageIndex, pageSize, out recordCount))
                {
                    while (reader.Read())
                    {
                        PK10_Buy item = new PK10_Buy();
                        item.ID = reader.GetInt32(0);
                        item.uID = reader.GetInt32(1);
                        item.uName = reader.GetString(2);
                        item.ListID = reader.GetInt32(3);
                        item.ListNo= reader.GetString(4);
                        item.ListNums= reader.GetString(5);
                        item.BuyDescript= reader.GetString(6);
                        item.BuyPrice= reader.GetInt32(7);
                        item.PayMoney = reader.GetInt32(8);
                        item.WinMoney = reader.GetInt32(9);
                        item.BuyTime = reader.GetDateTime(10);
                        item.isTest = reader.GetDecimal(11);
                        item.BuyCount = reader.GetInt32(12);
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetTobeCaseDatas Error:" + ex.Message);
            }
            #endregion
            //
            return list;
        }
        public List<PK10_Buy> GetBuyDatas(int listid,int uid,int showtype, int pageSize, int pageIndex, out int recordCount)//读取用户已下注的数据（分页）
        {
            List<PK10_Buy> list = new List<PK10_Buy>();
            #region
            try
            {
                string sTable = "tb_PK10_Buy";
                string sPkey = "id";
                string sField = "id,uid,uname,listid,listno,listnums,buyDescript,buyprice,winmoney,buytime,istest,caseflag,paymoney,validflag,buycount";
                string sCondition = "";
                if (listid != 0)
                {
                    sCondition="listID=" + listid.ToString().Trim();
                }
                if (uid != 0)
                {
                    if (sCondition != "")
                        sCondition += " and ";
                    sCondition += "uID=" + uid.ToString().Trim();
                }
                //
                string cshowtype = "";
                switch(showtype)
                {
                    case 0://全部
                        break;
                    case 1: //未开奖
                        cshowtype = " listNums=''";
                        break; 
                    case 2: //待兑奖
                        cshowtype = " WinMoney > 0 and CaseFlag = 0 and ValidFlag = 1";
                        break;
                    case 3: //已兑奖
                        cshowtype = " WinMoney > 0 and CaseFlag = 1";
                        break;
                    case 4: //过期未兑奖的
                        cshowtype = " WinMoney > 0 and CaseFlag = 0 and ValidFlag=0";
                        break;
                }
                if (cshowtype != "")
                {
                    if (sCondition == "")
                        sCondition = cshowtype;
                    else
                        sCondition += " and " + cshowtype;
                  }  
                //
                string sOrder = "BuyTime DESC";
                recordCount = 0; //输出总记录数
                using (SqlDataReader reader = MySqlHelper.GetPageData(sTable, sPkey, sField, sCondition, sOrder, pageIndex, pageSize, out recordCount))
                {
                    while (reader.Read())
                    {
                        PK10_Buy item = new PK10_Buy();
                        item.ID = reader.GetInt32(0);
                        item.uID = reader.GetInt32(1);
                        item.uName = reader.GetString(2);
                        item.ListID = reader.GetInt32(3);
                        item.ListNo = reader.GetString(4);
                        item.ListNums = reader.GetString(5);
                        item.BuyDescript = reader.GetString(6);
                        item.BuyPrice = reader.GetInt32(7);
                        item.WinMoney = reader.GetInt32(8);
                        item.BuyTime = reader.GetDateTime(9);
                        item.isTest = reader.GetDecimal(10);
                        item.CaseFlag = reader.GetDecimal(11);
                        item.PayMoney = reader.GetInt32(12);
                        item.ValidFlag = reader.GetInt32(13);
                        item.BuyCount= reader.GetInt32(14);
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyDatas Error:" + ex.Message);
            }
            #endregion
            //
            return list;
        }
        public List<PK10_Top> GetWinTopDatas(DateTime beginDate,DateTime endDate,int listid,int maxcount, int pageSize, int pageIndex, out int recordCount)//统计出赢奖用户的排行(不计算扣除的手续费)（分页）
        {
            List<PK10_Top> list = new List<PK10_Top>();
            recordCount = 0;
            #region
            try
            {
                PK10_Base _base = GetSaleBase();
                if (_base != null && _base.CurrentSaleDate > DateTime.MinValue)
                {
                    string cWhere = " Where listnums!='' "; //没开奖的不计算
                    if (beginDate > DateTime.MinValue)
                        cWhere += " And BuyTime>='" + beginDate.ToString() + "'";
                    if (endDate < DateTime.MaxValue)
                        cWhere += " And BuyTime<'" + endDate.AddDays(1).ToString() + "'";
                    if (listid != 0)
                            cWhere += " And listid =" + listid.ToString().Trim();
                    #region 取得统计结果的总记录数
                    string topstr = maxcount == 0 ? "" : " Top " + maxcount.ToString().Trim();
                    string countString = "SELECT  COUNT(DISTINCT UID) FROM tb_PK10_Buy " + cWhere + "";
                    recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
                    if (maxcount > 0 && recordCount > maxcount)
                        recordCount = maxcount;
                    if (recordCount > 0)
                    {
                        int pageCount = BCW.Common.BasePage.CalcPageCount(recordCount, pageSize, ref pageIndex);
                    }
                    else
                    {
                        return list;
                    }
                    #endregion
                    #region 读取出相关记录
                    string queryString = "";
                    //queryString = "Select A.* From (SELECT " + topstr + " uID,sum(cast(WinMoney-PayMoney as bigint)) as iGold FROM tb_PK10_Buy " + cWhere + " group by UID ) as A Order By A.iGold DESC";
                    queryString = "Select "+topstr+" A.* From (SELECT uID,sum(cast(WinMoney-PayMoney as bigint)) as iGold FROM tb_PK10_Buy " + cWhere + " group by UID ) as A Order By A.iGold DESC";
                    using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
                    {
                        int stratIndex = (pageIndex - 1) * pageSize;
                        int endIndex = pageIndex * pageSize;
                        int k = 0;
                        while (reader.Read())
                        {
                            if (k >= stratIndex && k < endIndex)
                            {
                                PK10_Top item = new PK10_Top();
                                item.No = k +1 ;
                                item.UsID = reader.GetInt32(0);
                                item.UsName = "";
                                item.iGold  = reader.GetInt64(1);
                                list.Add(item);
                            }
                            if (k == endIndex)
                                break;
                            k++;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetWinTopDatas Error:" + ex.Message);
            }
            #endregion
            //
            return list;
        }
        public PK10_BuyType GetBuyTypeByID(int id)
        {
            PK10_BuyType buyType = new PK10_BuyType();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType Where ID=" + id.ToString();
                buyType = ModelHelper.GetModelBySql<PK10_BuyType>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyTypeByID Error:" + ex.Message);
            }
            return buyType;
        }
        public PK10_BuyType GetBuyTypeByID(SqlConnection conn,SqlTransaction trans, int id)
        {
            PK10_BuyType buyType = new PK10_BuyType();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType Where ID=" + id.ToString();
                buyType = ModelHelper.GetModelBySql<PK10_BuyType>(conn,trans,cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyTypeByID Error:" + ex.Message);
            }
            return buyType;
        }
        public PK10_List GetListByID(int id)
        {
            PK10_List list = new PK10_List();
            try
            {
                string cSQL = "Select * From tb_PK10_List Where ID=" + id.ToString();
                list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetListByID Error:" + ex.Message);
            }
            return list;
        }
        public PK10_List GetListByID(SqlConnection conn, SqlTransaction trans, int id)
        {
            PK10_List list = new PK10_List();
            try
            {
                string cSQL = "Select * From tb_PK10_List Where ID=" + id.ToString();
                list = ModelHelper.GetModelBySql<PK10_List>(conn, trans, cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetListByID Error:" + ex.Message);
            }
            return list;
        }
        public PK10_List GetListByNo(string no)
        {
            if (string.IsNullOrEmpty(no))
                return null;
            PK10_List list = new PK10_List();
            try
            {
                string cSQL = "Select * From tb_PK10_List Where No='" + no.ToString().Trim() + "'";
                list = ModelHelper.GetModelBySql<PK10_List>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetListByNo Error:" + ex.Message);
            }
            return list;
        }
        public PK10_Buy GetBuyByID(int id)
        {
            PK10_Buy list = new PK10_Buy();
            try
            {
                string cSQL = "Select * From tb_PK10_Buy Where ID=" + id.ToString();
                list = ModelHelper.GetModelBySql<PK10_Buy>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyByID Error:" + ex.Message);
            }
            return list;
        }
        public PK10_Buy GetBuyByID(SqlConnection conn, SqlTransaction trans, int id)
        {
            PK10_Buy list = new PK10_Buy();
            try
            {
                string cSQL = "Select * From tb_PK10_Buy Where ID=" + id.ToString();
                list = ModelHelper.GetModelBySql<PK10_Buy>(conn, trans, cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyByID Error:" + ex.Message);
            }
            return list;
        }
        public List<PK10_BuyType> GetBuyTypes() //读取所有购买类型
        {
            List<PK10_BuyType> list = new List<PK10_BuyType>();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType order by ParentID,No";
                DataTable dt = MySqlHelper.GetTable(cSQL);
                list = ModelHelper.DataTableToModel<PK10_BuyType>(dt);
            }
            catch(Exception ex)
            {
                throw new Exception("GetBuyTypes Error:" + ex.Message);
            }
            return list;
        }
        public List<PK10_BuyType> GetBuyTypes2() //读取所有真实的购买类型
        {
            List<PK10_BuyType> list = new List<PK10_BuyType>();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType Where ParentID!=0 order by ParentID,No";
                DataTable dt = MySqlHelper.GetTable(cSQL);
                list = ModelHelper.DataTableToModel<PK10_BuyType>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyTypes Error:" + ex.Message);
            }
            return list;
        }
        public List<PK10_BuyType> GetBuyTypes3() //读取机器人可以投注的所有真实的购买类型
        {
            List<PK10_BuyType> list = new List<PK10_BuyType>();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType Where ParentID!=0 and (norobot is null or NoRobot=0) order by ParentID,No";
                DataTable dt = MySqlHelper.GetTable(cSQL);
                list = ModelHelper.DataTableToModel<PK10_BuyType>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyTypes Error:" + ex.Message);
            }
            return list;
        }
        public List<PK10_BuyType> GetBuyTypes(int parentid) //读取购买类型
        {
            List<PK10_BuyType> list = new List<PK10_BuyType>();
            try
            {
                string cSQL = "Select * From tb_PK10_BuyType Where ParentID=" + parentid.ToString().Trim() + " order by No";
                DataTable dt = MySqlHelper.GetTable(cSQL);
                list = ModelHelper.DataTableToModel<PK10_BuyType>(dt);
            }
            catch (Exception ex)
            {
                throw new Exception("GetBuyTypes Error:" + ex.Message);
            }
            return list;
        }
        public PK10_Stutas GetListStatus(PK10_List list) //读取期号的状态
        {
            return GetListStatus(list,DateTime.Now);
        }
        public PK10_Stutas GetListStatus(PK10_List list, DateTime time)
        {
            PK10_Stutas status = PK10_Stutas.未知;
            //
            DateTime now = time;
            if (now < list.BeginTime)
                status = PK10_Stutas.待售;
            else
            {
                if (now < list.EndTime)
                    status = PK10_Stutas.在售;
                else
                {
                    if (list.OpenFlag == 0)
                        status = PK10_Stutas.待开奖;
                    else
                        status = PK10_Stutas.已开奖;
                }
            }
            //
            return status;
        }
        #endregion
        //
        #region 保存赔率
        public string SaveBuyType(PK10_BuyType otype)
        {
            string cSQL = "";
            cSQL = "Select * From tb_PK10_BuyType Where ID=" + otype.ID.ToString().Trim();
            DataTable dt = MySqlHelper.GetTable(cSQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                cSQL = "Update tb_PK10_BuyType Set ";
                cSQL += "MultiSelect=" + otype.MultiSelect.ToString().Trim();
                cSQL += "," + "d0=" + otype.d0.ToString().Trim();
                cSQL += "," + "d1=" + otype.d1.ToString().Trim();
                cSQL += "," + "d2=" + otype.d2.ToString().Trim();
                cSQL += "," + "d3=" + otype.d3.ToString().Trim();
                cSQL += "," + "d4=" + otype.d4.ToString().Trim();
                cSQL += "," + "d5=" + otype.d5.ToString().Trim();
                cSQL += "," + "d6=" + otype.d6.ToString().Trim();
                cSQL += "," + "d7=" + otype.d7.ToString().Trim();
                cSQL += "," + "d8=" + otype.d8.ToString().Trim();
                cSQL += "," + "d9=" + otype.d9.ToString().Trim();
                cSQL += "," + "d10=" + otype.d10.ToString().Trim();
                cSQL += "," + "paylimit=" + otype.PayLimit.ToString().Trim();
                cSQL += "," + "Remark='" + otype.Remark.Trim() + "'";
                cSQL += "," + "RateFlag=" + otype.RateFlag.ToString().Trim();
                cSQL += "," + "RateBeginStep=" + otype.RateBeginStep.ToString().Trim();
                cSQL += "," + "RateStepChange=" + otype.RateStepChange.ToString().Trim();
                cSQL += "," + "RateMin=" + otype.RateMin.ToString().Trim();
                cSQL += "," + "RateMax=" + otype.RateMax.ToString().Trim();
                cSQL += "," + "NoRobot=" + otype.NoRobot.ToString().Trim();
                cSQL += "," + "paylimitUser=" + otype.PayLimitUser.ToString().Trim();
                cSQL += " Where ID=" + otype.ID.ToString().Trim();
                int rows = MySqlHelper.ExecuteSql(cSQL);
                if (rows > 0)
                    return "";
                else
                    return "保存失败";
            }
            else
                return "找不到对应ID的记录！";
        }
        public string SaveBuyType2(PK10_BuyType otype) //保存浮动赔率类型的参数设置
        {
            if (otype == null)
                return "参数错误";
            string cFlag = "";
            try
            {
                string cSQL = "";
                cSQL = "Select * From tb_PK10_BuyType Where ID=" + otype.ID.ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cSQL = "Update tb_PK10_BuyType Set ";
                    cSQL += "MultiSelect=" + otype.MultiSelect.ToString().Trim();
                    cSQL += "," + "d0=" + otype.d0.ToString().Trim();
                    cSQL += "," + "d1=" + otype.d1.ToString().Trim();
                    cSQL += "," + "d2=" + otype.d2.ToString().Trim();
                    cSQL += "," + "d3=" + otype.d3.ToString().Trim();
                    //cSQL += "," + "d4=" + otype.d4.ToString().Trim();
                    //cSQL += "," + "d5=" + otype.d5.ToString().Trim();
                    //cSQL += "," + "d6=" + otype.d6.ToString().Trim();
                    //cSQL += "," + "d7=" + otype.d7.ToString().Trim();
                    cSQL += "," + "d8=" + otype.d8.ToString().Trim();
                    cSQL += "," + "d9=" + otype.d9.ToString().Trim();
                    //cSQL += "," + "d10=" + otype.d10.ToString().Trim();
                    cSQL += "," + "paylimit=" + otype.PayLimit.ToString().Trim();
                    cSQL += "," + "Remark='" + otype.Remark.Trim() + "'";
                    cSQL += "," + "RateFlag=" + otype.RateFlag.ToString().Trim();
                    cSQL += "," + "RateBeginStep=" + otype.RateBeginStep.ToString().Trim();
                    cSQL += "," + "RateStepChange=" + otype.RateStepChange.ToString().Trim();
                    cSQL += "," + "RateMin=" + otype.RateMin.ToString().Trim();
                    cSQL += "," + "RateMax=" + otype.RateMax.ToString().Trim();
                    cSQL += "," + "NoRobot=" + otype.NoRobot.ToString().Trim();
                    cSQL += "," + "paylimitUser=" + otype.PayLimitUser.ToString().Trim();
                    cSQL += " Where ID=" + otype.ID.ToString().Trim();
                    int rows = MySqlHelper.ExecuteSql(cSQL);
                    if (rows == 0)
                        return "保存失败";
                    cFlag= UpdateRateDatas(otype.ParentID,otype.ID);//更新赔率
                }
                else
                    return "找不到对应ID的记录！";
            }
            catch (Exception ex)
            {
                cFlag = ex.Message;
            }
            return cFlag;
        }
        public string SaveBuyTypes2(int id) //批量保存某一大类型的变动赔率设置
        {
            PK10_BuyType otype = GetBuyTypeByID(id);
            if (otype == null)
                return "参数错误";
            string cFlag = "";
            try
            {
                string cSQL = "";
                cSQL = "Update tb_PK10_BuyType Set ";
                cSQL += "MultiSelect=" + otype.MultiSelect.ToString().Trim();
                cSQL += "," + "d0=" + otype.d0.ToString().Trim();
                cSQL += "," + "d1=" + otype.d1.ToString().Trim();
                cSQL += "," + "d2=" + otype.d2.ToString().Trim();
                cSQL += "," + "d3=" + otype.d3.ToString().Trim();
                //cSQL += "," + "d4=" + otype.d4.ToString().Trim();
                //cSQL += "," + "d5=" + otype.d5.ToString().Trim();
                //cSQL += "," + "d6=" + otype.d6.ToString().Trim();
                //cSQL += "," + "d7=" + otype.d7.ToString().Trim();
                //cSQL += "," + "d8=" + otype.d8.ToString().Trim();
                //cSQL += "," + "d9=" + otype.d9.ToString().Trim();
                //cSQL += "," + "d10=" + otype.d10.ToString().Trim();
                cSQL += "," + "paylimit=" + otype.PayLimit.ToString().Trim();
                //cSQL += "," + "Remark='" + otype.Remark.Trim() + "'";
                cSQL += "," + "RateFlag=" + otype.RateFlag.ToString().Trim();
                cSQL += "," + "RateBeginStep=" + otype.RateBeginStep.ToString().Trim();
                cSQL += "," + "RateStepChange=" + otype.RateStepChange.ToString().Trim();
                cSQL += "," + "RateMin=" + otype.RateMin.ToString().Trim();
                cSQL += "," + "RateMax=" + otype.RateMax.ToString().Trim();
                //cSQL += "," + "NoRobot=" + otype.NoRobot.ToString().Trim();
                cSQL += "," + "paylimitUser=" + otype.PayLimitUser.ToString().Trim();
                cSQL += " Where ParentID=" + otype.ParentID.ToString().Trim();
                int rows = MySqlHelper.ExecuteSql(cSQL);
                if (rows == 0)
                    return "保存失败";
                cFlag = UpdateRateDatas(otype.ParentID);//更新赔率
            }
            catch(Exception ex)
            {
                cFlag = ex.Message;
            }
            return cFlag;
        }
        #endregion
        //
        #region 初始化当天销售数据（生成将要开售的所有期号）
        public string InitSaleData(DateTime gameBeginOpenTime, int BeginNo, int saleTimes, int validDays, int GameOpenTimes, int stopSec)
        {
            string cFlag = "";
            //
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = InitSaleData(conn, trans,gameBeginOpenTime, BeginNo, saleTimes,validDays, GameOpenTimes, stopSec);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        cFlag = E.Message;
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            //
            return cFlag;
        }
        public string InitSaleData(SqlConnection conn,SqlTransaction trans, DateTime gameBeginOpenTime, int BeginNo, int saleTimes,int validDays, int GameOpenTimes, int stopSec)
        {
            string cFlag = "";
            #region 添加期号数据
            string cSQL = "";
            int no = BeginNo;
            DateTime beginsaletime = DateTime.Parse(DateTime.Now.ToShortDateString());//每天从凌晨开始开售
            DateTime endsaletime = gameBeginOpenTime.AddSeconds(-stopSec); //开奖前X秒停售
            DateTime date = DateTime.Parse(DateTime.Now.ToShortDateString()); 
            while(no<(BeginNo+ GameOpenTimes) && endsaletime<date.AddDays(1))
            {
                cSQL = "Insert into tb_PK10_List(no,date,validdate,begintime,endtime) Values(";
                cSQL += "'" + no.ToString().Trim() + "'";
                cSQL += "," + "'" + date.ToShortDateString() + "'";
                cSQL += "," + "'" + date.AddDays(validDays).ToShortDateString() + "'";
                cSQL += "," + "'" + beginsaletime.ToString() + "'";
                cSQL += "," + "'" + endsaletime.ToString() + "'";
                cSQL += ")";
                int id = MySqlHelper.InsertAndGetID(cSQL, conn, trans);
                if (id == 0)
                {
                    cFlag = "创建开售失败！";
                    return cFlag;
                }
                //
                no++;
                beginsaletime = endsaletime;
                endsaletime = beginsaletime.AddMinutes(saleTimes);
                //
            }
            #endregion
            #region 更新基础数据
            cSQL = "Update tb_PK10_Base Set ";
            cSQL += " CurrentSaleDate='" + date.ToShortDateString() + "'";
            cSQL += " Where ID=0";
            int rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows == 0)
            {
                cFlag = "更新基础数据失败";
                return cFlag;
            }
            #endregion
            return cFlag;
        }
        #endregion
        #region 设置下次读取开奖信息
        public string SetNextGetOpenData(int openTimes,int saleTimes) 
        {
            string cFlag = "";
            //
            PK10_List list = GetTobeOpenData();
            if (list != null)
            {
                string cSQL = "";
                cSQL = "Update tb_PK10_Base Set ";
                cSQL += "GetOpenDataBeginTime='" + list.EndTime.AddSeconds(openTimes).ToString() + "'";
                cSQL += "," + "GetOpenDataEndTime='" + list.EndTime.AddMinutes(saleTimes).ToString() + "'";
                cSQL += "," + "GetOpenDataNo='" + list.No.ToString() + "'";
                cSQL += " Where ID=0";
                int rows = MySqlHelper.ExecuteSql(cSQL);
                if (rows == 0)
                    cFlag = "更新基础数据失败";
            }
            else
                cFlag = "找不到最后一期开售数据";
            //
            return cFlag;
        }
        #endregion

        #region 更新开奖记录
        public string SaveOpenData(PK10_List list) //保存最后开奖数据
        {
            string cFlag = "";
            //
            #region 启动数据事务，保存开奖记录
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = SaveOpenData(conn, trans, list); //保存开奖数据
                        //
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                        //
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        cFlag = E.Message;
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            //
            if (cFlag == "")
                cFlag = UpdateRateDatas();//更新浮动赔率
            //
            return cFlag;
        }
        public string SaveOpenDatas(IList<PK10_List> lists, string beginNo) //更新多个开奖记录
        {
            string cFlag = "";
            //
            int beginno = int.Parse(beginNo);
            if (lists.Count > 0)
            {
                #region 启动数据事务，循环保存
                using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            for (int i = 0; i < lists.Count; i++)
                            {
                                int no = int.Parse(lists[i].No.ToString());
                                if (no >= beginno) //指定从某期开始保存
                                {
                                    cFlag = SaveOpenData(conn, trans, lists[i]);
                                    if (cFlag != "")
                                        break;
                                }
                            }
                            if (cFlag == "")
                                trans.Commit();
                            else
                                trans.Rollback();
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            cFlag = E.Message;
                            trans.Rollback();
                            throw new Exception(E.Message);
                        }
                    }
                    conn.Close();
                }
                #endregion
            }
            //
            if (cFlag == "")
                cFlag = UpdateRateDatas();//更新浮动赔率
            //
            return cFlag;
        }
        private string SaveOpenData(SqlConnection conn, SqlTransaction trans, PK10_List list) //保存开奖数据
        {
            string cFlag = "";
            #region 保存数据
            string no = list.No.Trim();
            PK10_List _list = ModelHelper.GetModelBySql<PK10_List>(conn, trans, "Select ID From tb_PK10_List Where No='" + no + "'");
            string cSQL = "";
            if (_list != null)
            {
                if (_list.CalcFlag == 0 && _list.OpenFlag == 0)//忽略已经开奖或者派奖的记录
                {
                    string cid = _list.ID.ToString().Trim();
                    cSQL = "Update tb_PK10_List Set openFlag=1,nums='" + list.Nums.Trim() + "' Where ID=" + cid;
                }
            }
            if (cSQL != "")
            {
                int rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                if (rows == 0)
                    cFlag = "保存开奖记录失败！";
            }
            #endregion
            return cFlag;
        }
        #endregion
        #region 赔率变动
        //
        private string GetPrevNum(SqlConnection conn, SqlTransaction trans, string openNo) //取到上一期开奖号码
        {
            string nums = "";
            try
            {
                string prevNo = (int.Parse(openNo) - 1).ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(conn, trans, "Select * From tb_PK10_List Where no='" + prevNo + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    nums = dt.Rows[0]["Nums"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取上一期号码失败!" + ex.Message);
            }
            return nums;
        }
        private string UpdateRateDatas(SqlConnection conn, SqlTransaction trans,string openNo,string nums)//更新浮动赔率
        {
            string cFlag = "";
            //
            try
            {
                string prevNums = GetPrevNum(conn, trans, openNo);
                string[] currOpenLists = nums.Split(','); //nums格式为01,02,03,04,05,06,07,08,09,10
                string[] prevOpenLists = { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
                if(prevNums!="")
                {
                    string[] temp = prevNums.Split(',');
                    for (int i=0;i<temp.Length;i++)
                    {
                        prevOpenLists[i] = temp[i];
                    }
                }
                DataTable dtDRate = MySqlHelper.GetTable(conn, trans, "Select * From tb_PK10_BuyType Where PayLimitType=1"); //浮动赔率列表【可对冲类型】
                foreach (DataRow dr in dtDRate.Rows)
                {
                    UpdateRateData(conn, trans, dr, currOpenLists, prevOpenLists);
                }
            }
            catch(Exception ex)
            {
                cFlag = "更新浮动赔率失败！" + ex.Message;
            }
            //
            return cFlag;
        }
        //
        private string UpdateRateData(SqlConnection conn, SqlTransaction trans, DataTable dtBuyType)
        {
            string cFlag = "";
            //
            string cSQL = "Select A.* From (Select top 20 * From tb_PK10_List Where nums!='' order by begintime desc) as A order by A.begintime";
            DataTable dtList = MySqlHelper.GetTable(conn, trans, cSQL);
            #region 计算
            int currNo = 0, prevNo = 0;
            string[] prevOpenLists = { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
            string[] currOpenLists = { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
            foreach (DataRow dr in dtList.Rows)
            {
                currNo = int.Parse(dr["no"].ToString());
                if (currNo != prevNo + 1) //断开期号，重置上期号码为空
                {
                    string[] tmp = { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
                    prevOpenLists = tmp;
                }
                if (!string.IsNullOrEmpty(dr["nums"].ToString().Trim())) //空开奖号码的，跳过
                {
                    currOpenLists = dr["nums"].ToString().Trim().Split(','); //nums格式为01,02,03,04,05,06,07,08,09,10
                                                                             //
                    foreach (DataRow drRate in dtBuyType.Rows)
                    {
                        UpdateRateData(conn, trans, drRate, currOpenLists, prevOpenLists);
                    }
                }
                else
                {
                    string[] tmp = { "00", "00", "00", "00", "00", "00", "00", "00", "00", "00" };
                    currOpenLists = tmp;
                }
                //
                prevOpenLists = currOpenLists;
                prevNo = currNo;
            }
            #endregion
            //
            #region 保存数据
            decimal d5 = 0, d6 = 0, d8 = 0, d9 = 0;
            foreach (DataRow drRate in dtBuyType.Rows)
            {
                string cid = drRate["ID"].ToString().Trim();
                d5 = 0; d6 = 0; d8 = 0; d9 = 0;
                decimal.TryParse(drRate["d5"].ToString(), out d5);
                decimal.TryParse(drRate["d6"].ToString(), out d6);
                decimal.TryParse(drRate["d8"].ToString(), out d8);
                decimal.TryParse(drRate["d9"].ToString(), out d9);
                cSQL = "Update tb_PK10_BuyType Set ";
                cSQL += "d5=" + d5.ToString() + ",";
                cSQL += "d6=" + d6.ToString() + ",";
                cSQL += "d8=" + d8.ToString() + ",";
                cSQL += "d9=" + d9.ToString();
                cSQL += " Where ID=" + cid;
                int row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            }
            #endregion
            //
            return cFlag;
        }
        private string UpdateRateData(SqlConnection conn, SqlTransaction trans, DataRow drBuyType, string[] currOpenLists, string[] prevOpenLists) //更新单个类型的变动赔率
        {
            string cFlag = "";
            //
            decimal RateFlag = 0;
            decimal.TryParse(drBuyType["RateFlag"].ToString(), out RateFlag);
            decimal d5 = 0, d6 = 0, d8 = 0, d9 = 0, oRate1 = 0, oRate2 = 0; ;
            decimal.TryParse(drBuyType["d5"].ToString(), out d5);
            decimal.TryParse(drBuyType["d6"].ToString(), out d6);
            decimal.TryParse(drBuyType["d8"].ToString(), out d8);
            decimal.TryParse(drBuyType["d9"].ToString(), out d9);
            decimal.TryParse(drBuyType["d1"].ToString(), out oRate1);//原始赔率1
            decimal.TryParse(drBuyType["d2"].ToString(), out oRate2);//原始赔率2
            string cid = drBuyType["id"].ToString().Trim();
            if (RateFlag == 1)
            {
                #region 计算浮动赔率、连开次数
                decimal RateBeginStep = 0, RateStepChange = 0, RateMin = 0, RateMax = 0;
                decimal.TryParse(drBuyType["RateBeginStep"].ToString(), out RateBeginStep);//连开X期起步，进行赔率变化
                decimal.TryParse(drBuyType["RateStepChange"].ToString(), out RateStepChange);//赔率变化步长
                decimal.TryParse(drBuyType["RateMin"].ToString(), out RateMin);//赔率最小值
                decimal.TryParse(drBuyType["RateMax"].ToString(), out RateMax);//赔率最大值
                int numid = int.Parse(drBuyType["NumID"].ToString());
                string cparentid = drBuyType["parentid"].ToString().Trim();
                int currValue = 0, prevValue = 0, value1 = 0, value2 = 0;
                switch (cparentid)
                {
                    case "2": //大小
                        #region
                        currValue = int.Parse(currOpenLists[numid]);
                        prevValue = int.Parse(prevOpenLists[numid]);
                        value1 = currValue > 5 ? 1 : 0;
                        value2 = prevValue > 5 ? 1 : 0;
                        if (value1 == value2 && prevValue != 0) //连开
                        {
                            if (value1 == 0) //连开小
                            {
                                decimal steps = d5 + 1;
                                drBuyType["d5"] = steps; //小的连开次数
                                drBuyType["d6"] = 0; //大的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开小的赔率
                                drBuyType["d9"] = 0;//开大的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d9"] = rate1;
                                    else
                                        drBuyType["d9"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d8"] = rate2;
                                    else
                                        drBuyType["d8"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                            else //连开大
                            {
                                decimal steps = d6 + 1;
                                drBuyType["d5"] = 0;//小的连开次数
                                drBuyType["d6"] = steps;//大的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开小的赔率
                                drBuyType["d9"] = 0;//开大的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d8"] = rate1;
                                    else
                                        drBuyType["d8"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d9"] = rate2;
                                    else
                                        drBuyType["d9"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                        }
                        else //断开
                        {
                            drBuyType["d5"] = value1 == 0 ? 1 : 0; //小的连开次数
                            drBuyType["d6"] = value1 == 1 ? 1 : 0; //大的连开次数
                            //drBuyType["d7"] = 0;//预留
                            drBuyType["d8"] = oRate1;//开小的赔率
                            drBuyType["d9"] = oRate2;//开大的赔率
                            //drBuyType["d10"] = 0;//预留
                        }
                        #endregion
                        break;
                    case "3": //单双
                        #region
                        currValue = int.Parse(currOpenLists[numid]);
                        prevValue = int.Parse(prevOpenLists[numid]);
                        value1 = currValue % 2 == 0 ? 1 : 0;
                        value2 = prevValue % 2 == 0 ? 1 : 0;
                        if (value1 == value2 && prevValue != 0) //连开
                        {
                            if (value1 == 0) //连开单
                            {
                                decimal steps = d5 + 1;
                                drBuyType["d5"] = steps; //单的连开次数
                                drBuyType["d6"] = 0; //双的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开单的赔率
                                drBuyType["d9"] = 0;//开双的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d9"] = rate1;
                                    else
                                        drBuyType["d9"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d8"] = rate2;
                                    else
                                        drBuyType["d8"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                            else //连开双
                            {
                                decimal steps = d6 + 1;
                                drBuyType["d5"] = 0;//单的连开次数
                                drBuyType["d6"] = steps;//双的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开单的赔率
                                drBuyType["d9"] = 0;//开双的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d8"] = rate1;
                                    else
                                        drBuyType["d8"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d9"] = rate2;
                                    else
                                        drBuyType["d9"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                        }
                        else //断开
                        {
                            drBuyType["d5"] = value1 == 0 ? 1 : 0;  //单的连开次数
                            drBuyType["d6"] = value1 == 1 ? 1 : 0;  //双的连开次数
                            //drBuyType["d7"] = 0;//预留
                            drBuyType["d8"] = oRate1;//开单的赔率
                            drBuyType["d9"] = oRate2;//开双的赔率
                            //drBuyType["d10"] = 0;//预留
                        }
                        #endregion
                        break;
                    case "4": //龙虎
                        #region
                        int prevValue1 = int.Parse(prevOpenLists[numid]);
                        int prevValue2 = int.Parse(prevOpenLists[9 - numid]);
                        int currValue1 = int.Parse(currOpenLists[numid]);
                        int currValue2 = int.Parse(currOpenLists[9 - numid]);
                        value1 = currValue1 > currValue2 ? 1 : 0; //1:龙
                        value2 = prevValue1 > prevValue2 ? 1 : 0;
                        if (value1 == value2 && prevValue1 != 0) //连开
                        {
                            if (value1 == 0) //连开虎
                            {
                                decimal steps = d5 + 1;
                                drBuyType["d5"] = steps; //虎的连开次数
                                drBuyType["d6"] = 0; //龙的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开虎的赔率
                                drBuyType["d9"] = 0;//开龙的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d9"] = rate1;
                                    else
                                        drBuyType["d9"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d8"] = rate2;
                                    else
                                        drBuyType["d8"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                            else //连开龙
                            {
                                decimal steps = d6 + 1;
                                drBuyType["d5"] = 0;//虎的连开次数
                                drBuyType["d6"] = steps;//龙的连开次数
                                //drBuyType["d7"] = 0;//预留
                                drBuyType["d8"] = 0;//开虎的赔率
                                drBuyType["d9"] = 0;//开龙的赔率
                                //drBuyType["d10"] = 0;//预留
                                if (steps > RateBeginStep)
                                {
                                    decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                    decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                    if (rate1 >= RateMin)
                                        drBuyType["d8"] = rate1;
                                    else
                                        drBuyType["d8"] = RateMin;
                                    if (RateMax == 0 || rate2 <= RateMax)
                                        drBuyType["d9"] = rate2;
                                    else
                                        drBuyType["d9"] = RateMax;
                                }
                                else
                                {
                                    drBuyType["d8"] = oRate1;
                                    drBuyType["d9"] = oRate2;
                                }
                            }
                        }
                        else //断开
                        {
                            drBuyType["d5"] = value1 == 0 ? 1 : 0;  //虎的连开次数
                            drBuyType["d6"] = value1 == 1 ? 1 : 0; ; //龙的连开次数
                            //drBuyType["d7"] = 0;//预留
                            drBuyType["d8"] = oRate1;//开虎的赔率
                            drBuyType["d9"] = oRate2;//开龙的赔率
                            //drBuyType["d10"] = 0;//预留
                        }
                        #endregion
                        break;
                    case "6": //冠亚和值
                        #region
                        string type = drBuyType["Type"].ToString().Trim();
                        currValue = int.Parse(currOpenLists[0]) + int.Parse(currOpenLists[1]);
                        prevValue = int.Parse(prevOpenLists[0]) + int.Parse(prevOpenLists[1]);
                        switch (type)
                        {
                            case "2": //大小
                                #region
                                if (currValue == 11) //11为和，大小单双的赔率均等，重置
                                {
                                    drBuyType["d5"] = 0; //小/单的连开次数
                                    drBuyType["d6"] = 0; //大/双的连开次数
                                    //drBuyType["d7"] = 0;//预留
                                    drBuyType["d8"] = oRate1;//开小/单的赔率
                                    drBuyType["d9"] = oRate2;//开大的赔率
                                    //drBuyType["d10"] = 0;//预留
                                }
                                else
                                {
                                    value1 = currValue > 11 ? 1 : 0;
                                    value2 = prevValue > 11 ? 1 : 0;
                                    if (value1 == value2 && prevValue != 0) //连开
                                    {
                                        if (value1 == 0) //连开小
                                        {
                                            decimal steps = d5 + 1;
                                            drBuyType["d5"] = steps; //小的连开次数
                                            drBuyType["d6"] = 0; //大的连开次数
                                            //drBuyType["d7"] = 0;//预留
                                            drBuyType["d8"] = 0;//开小的赔率
                                            drBuyType["d9"] = 0;//开大的赔率
                                            //drBuyType["d10"] = 0;//预留
                                            if (steps > RateBeginStep)
                                            {
                                                decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                                decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                                if (rate1 >= RateMin)
                                                    drBuyType["d9"] = rate1;
                                                else
                                                    drBuyType["d9"] = RateMin;
                                                if (RateMax == 0 || rate2 <= RateMax)
                                                    drBuyType["d8"] = rate2;
                                                else
                                                    drBuyType["d8"] = RateMax;
                                            }
                                            else
                                            {
                                                drBuyType["d8"] = oRate1;
                                                drBuyType["d9"] = oRate2;
                                            }
                                        }
                                        else //连开大
                                        {
                                            decimal steps = d6 + 1;
                                            drBuyType["d5"] = 0;//小的连开次数
                                            drBuyType["d6"] = steps;//大的连开次数
                                            //drBuyType["d7"] = 0;//预留
                                            drBuyType["d8"] = 0;//开小的赔率
                                            drBuyType["d9"] = 0;//开大的赔率
                                            //drBuyType["d10"] = 0;//预留
                                            if (steps > RateBeginStep)
                                            {
                                                decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                                decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                                if (rate1 >= RateMin)
                                                    drBuyType["d8"] = rate1;
                                                else
                                                    drBuyType["d8"] = RateMin;
                                                if (RateMax == 0 || rate2 <= RateMax)
                                                    drBuyType["d9"] = rate2;
                                                else
                                                    drBuyType["d9"] = RateMax;
                                            }
                                            else
                                            {
                                                drBuyType["d8"] = oRate1;
                                                drBuyType["d9"] = oRate2;
                                            }
                                        }
                                    }
                                    else //断开
                                    {
                                        drBuyType["d5"] = value1 == 0 ? 1 : 0; ; //小的连开次数
                                        drBuyType["d6"] = value1 == 1 ? 1 : 0; ; //大的连开次数
                                        //drBuyType["d7"] = 0;//预留
                                        drBuyType["d8"] = oRate1;//开小的赔率
                                        drBuyType["d9"] = oRate2;//开大的赔率
                                        //drBuyType["d10"] = 0;//预留
                                    }
                                }
                                #endregion
                                break;
                            case "3": //单双
                                #region
                                if (currValue == 11) //11为和，大小单双的赔率均等，重置
                                {
                                    drBuyType["d5"] = 0; //小/单的连开次数
                                    drBuyType["d6"] = 0; //大/双的连开次数
                                    //drBuyType["d7"] = 0;//预留
                                    drBuyType["d8"] = oRate1;//开小/单的赔率
                                    drBuyType["d9"] = oRate2;//开大的赔率
                                    //drBuyType["d10"] = 0;//预留
                                }
                                else
                                {
                                    value1 = currValue % 2 == 0 ? 1 : 0;
                                    value2 = prevValue % 2 == 0 ? 1 : 0;
                                    if (value1 == value2 && prevValue != 0) //连开
                                    {
                                        if (value1 == 0) //连开单
                                        {
                                            decimal steps = d5 + 1;
                                            drBuyType["d5"] = steps; //单的连开次数
                                            drBuyType["d6"] = 0; //双的连开次数
                                            //drBuyType["d7"] = 0;//预留
                                            drBuyType["d8"] = 0;//开单的赔率
                                            drBuyType["d9"] = 0;//开双的赔率
                                            //drBuyType["d10"] = 0;//预留
                                            if (steps > RateBeginStep)
                                            {
                                                decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                                decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                                if (rate1 >= RateMin)
                                                    drBuyType["d9"] = rate1;
                                                else
                                                    drBuyType["d9"] = RateMin;
                                                if (RateMax == 0 || rate2 <= RateMax)
                                                    drBuyType["d8"] = rate2;
                                                else
                                                    drBuyType["d8"] = RateMax;
                                            }
                                            else
                                            {
                                                drBuyType["d8"] = oRate1;
                                                drBuyType["d9"] = oRate2;
                                            }
                                        }
                                        else //连开双
                                        {
                                            decimal steps = d6 + 1;
                                            drBuyType["d5"] = 0;//单的连开次数
                                            drBuyType["d6"] = steps;//双的连开次数
                                            //drBuyType["d7"] = 0;//预留
                                            drBuyType["d8"] = 0;//开单的赔率
                                            drBuyType["d9"] = 0;//开双的赔率
                                            //drBuyType["d10"] = 0;//预留
                                            if (steps > RateBeginStep)
                                            {
                                                decimal rate1 = oRate1 - (steps - RateBeginStep) * RateStepChange;
                                                decimal rate2 = oRate2 + (steps - RateBeginStep) * RateStepChange;
                                                if (rate1 >= RateMin)
                                                    drBuyType["d8"] = rate1;
                                                else
                                                    drBuyType["d8"] = RateMin;
                                                if (RateMax == 0 || rate2 <= RateMax)
                                                    drBuyType["d9"] = rate2;
                                                else
                                                    drBuyType["d9"] = RateMax;
                                            }
                                            else
                                            {
                                                drBuyType["d8"] = oRate1;
                                                drBuyType["d9"] = oRate2;
                                            }
                                        }
                                    }
                                    else //断开
                                    {
                                        drBuyType["d5"] = value1 == 0 ? 1 : 0;  //单的连开次数
                                        drBuyType["d6"] = value1 == 1 ? 1 : 0;  //双的连开次数
                                        //drBuyType["d7"] = 0;//预留
                                        drBuyType["d8"] = oRate1;//开单的赔率
                                        drBuyType["d9"] = oRate2;//开双的赔率
                                        //drBuyType["d10"] = 0;//预留
                                    }
                                }
                                #endregion
                                break;
                        }
                        #endregion
                        break;
                }
                #endregion
            }
            else //固定赔率
            {
                drBuyType["d5"] = 0;
                drBuyType["d6"] = 0;
                //drBuyType["d7"] = 0;//预留
                drBuyType["d8"] = oRate1;
                drBuyType["d9"] = oRate2;
                //drBuyType["d10"] = 0;//预留
            }
            //
            return cFlag;
        }
        //
        public string UpdateRateDatas()//更新所有浮动的赔率
        {
            string cFlag = "";
            #region 启动数据事务，循环保存
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = UpdateRateDatas(conn, trans);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        cFlag = E.Message;
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            return cFlag;
        }
        private string UpdateRateDatas(SqlConnection conn, SqlTransaction trans) //更新一次当前的赔率(从前30期开始重算连开次数)
        {
            string cFlag = "";
            //
            DataTable dtDRate = MySqlHelper.GetTable(conn,trans,"Select * From tb_PK10_BuyType Where PayLimitType=1"); //浮动赔率列表【可对冲类型】
            cFlag = UpdateRateData(conn, trans, dtDRate);
            //
            return cFlag;
        }
        //
        public string UpdateRateDatas(int parentid) //更新某一大类型的浮动赔率
        {
            string cFlag = "";
            #region 启动数据事务，循环保存
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = UpdateRateDatas(conn, trans,parentid);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        cFlag = E.Message;
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            return cFlag;
        }
        private string UpdateRateDatas(SqlConnection conn, SqlTransaction trans,int parentid) //更新一次当前的赔率(从前30期开始重算连开次数)
        {
            string cFlag = "";
            //
            DataTable dtDRate = MySqlHelper.GetTable(conn, trans, "Select * From tb_PK10_BuyType Where PayLimitType=1 and ParentID="+parentid.ToString().Trim()); //浮动赔率列表【可对冲类型】
            cFlag = UpdateRateData(conn, trans, dtDRate);
            //
            return cFlag;
        }
        //
        public string UpdateRateDatas(int parentid,int id) //更新某一类型的浮动赔率
        {
            string cFlag = "";
            #region 启动数据事务，循环保存
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = UpdateRateDatas(conn, trans, parentid,id);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        cFlag = E.Message;
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            return cFlag;
        }
        private string UpdateRateDatas(SqlConnection conn, SqlTransaction trans, int parentid,int id) //更新一次当前的赔率(从前30期开始重算连开次数)
        {
            string cFlag = "";
            //
            DataTable dtDRate = MySqlHelper.GetTable(conn, trans, "Select * From tb_PK10_BuyType Where PayLimitType=1 and ID=" + id.ToString().Trim()); //浮动赔率列表【可对冲类型】
            cFlag = UpdateRateData(conn, trans, dtDRate);
            //
            return cFlag;
        }
        #endregion

        #region 派奖计算
        public string CalcOpenData() //全部派奖
        {
            string cFlag = "";
            #region 列出没开奖计算的记录
            string cSQL = "";
            cSQL = "Select * From tb_PK10_List Where OpenFlag=1 and CalcFlag=0 Order By BeginTime";
            DataTable dt = MySqlHelper.GetTable(cSQL);
            if(dt!=null)
            {
                DataTable dtBuyType = MySqlHelper.GetTable("Select * From tb_PK10_BuyType Order by ID");
                dtBuyType.PrimaryKey = new DataColumn[] { dtBuyType.Columns["ID"] };
                foreach (DataRow dr in dt.Rows)
                {
                    int id = int.Parse(dr["ID"].ToString());
                    string nums = dr["Nums"].ToString().Trim();
                    if (nums != "")
                    {
                        #region 每一期的计算开一个事务进行计算
                        using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    cFlag = CalcOpenData(id, nums, dtBuyType, conn, trans);
                                    if (cFlag == "")
                                        trans.Commit();
                                    else
                                        trans.Rollback();
                                }
                                catch (System.Data.SqlClient.SqlException E)
                                {
                                    trans.Rollback();
                                    throw new Exception("全部派奖错误："+E.Message);
                                }
                            }
                            conn.Close();
                        }
                        #endregion
                    }
                }
            }
            #endregion
            return cFlag;
        }
        public string CalcOpenData(List<PK10_List> list) //指定的期数派奖
        {
            string cFlag = "";
            #region 开奖计算的记录
            if (list != null && list.Count > 0)
            {
                DataTable dtBuyType = MySqlHelper.GetTable("Select * From tb_PK10_BuyType Order by ID");
                dtBuyType.PrimaryKey = new DataColumn[] { dtBuyType.Columns["ID"] };
                for (int i = 0; i < list.Count; i++)
                {
                    int id = list[i].ID;
                    string listnums = list[i].Nums.Trim();
                    if (listnums != "")
                    {
                        #region 每一期的计算开一个事务进行计算
                        using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                        {
                            conn.Open();
                            using (SqlTransaction trans = conn.BeginTransaction())
                            {
                                try
                                {
                                    cFlag = CalcOpenData(id, listnums, dtBuyType, conn, trans);
                                    if (cFlag == "")
                                        trans.Commit();
                                    else
                                        trans.Rollback();
                                }
                                catch (System.Data.SqlClient.SqlException E)
                                {
                                    trans.Rollback();
                                    throw new Exception("计算指定的期数派奖错误:"+E.Message);
                                }
                            }
                            conn.Close();
                        }
                        #endregion
                    }
                }
            }
            #endregion
            return cFlag;
        }
        public string CalcOpenData(int id,string listnums, DataTable dtBuyType,SqlConnection conn,SqlTransaction trans)
        {
            string cFlag = "";
            try
            {
                string cSQL = "";
                #region 计算当期所有购买记录的派奖
                cSQL = "Select * From tb_PK10_Buy Where ListID=" + id.ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(conn, trans, cSQL);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        decimal winPrice = 0;
                        int winMoney = 0;
                        int WinNumsCount = 0;
                        string WinNums = "";
                        try
                        {
                            #region 计算并保存
                            string numsdetail = dr["NumsDetail"].ToString().Trim();
                            int BuyMulti = int.Parse(dr["BuyMulti"].ToString());
                            int buytype = int.Parse(dr["BuyType"].ToString());
                            int buyprice = int.Parse(dr["BuyPrice"].ToString());
                            string rate = dr["Rate"].ToString().Trim();

                            //
                            DataRow drbuytype = dtBuyType.Rows.Find(buytype);
                            if (drbuytype != null)
                            {
                                string[] Rates;
                                #region 取到当前购买类型的各种倍率
                                if (!string.IsNullOrEmpty(rate)) //购买时已经记录了赔率
                                {
                                    Rates = rate.Split('|');
                                }
                                else //购买时还没有设定赔率（旧程序没有记录赔率到购买记录【旧程序没有变动赔率】）
                                {
                                    if (drbuytype["PayLimitType"].ToString().Trim() == "1") //重置变动赔率列=固定赔率
                                    {
                                        drbuytype["d8"] = drbuytype["d1"];
                                        drbuytype["d9"] = drbuytype["d2"];
                                    }
                                    for (int ratei = 0; ratei <= 10; ratei++)
                                    {
                                        if (!string.IsNullOrEmpty(rate))
                                            rate += "|";
                                        rate += drbuytype["d" + ratei.ToString().Trim()].ToString().Trim();

                                    }
                                    Rates = rate.Split('|');
                                }
                                #endregion
                                int parentid = int.Parse(drbuytype["ParentID"].ToString());
                                switch (parentid)
                                {
                                    case 1: //
                                        CalcOpenData1(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                    case 2: //
                                        CalcOpenData2(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                    case 3: //
                                        CalcOpenData3(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                    case 4: //
                                        CalcOpenData4(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                    case 5: //
                                        CalcOpenData5(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                    case 6: //
                                        CalcOpenData6(Rates, listnums, numsdetail, buyprice, dtBuyType, buytype, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                                        break;
                                }
                            }
                            //
                            cSQL = "Update tb_PK10_Buy ";
                            cSQL += " Set ListNums='" + listnums + "'";
                            cSQL += ", WinNums='" + WinNums + "'";
                            cSQL += ", WinNumsCount=" + WinNumsCount.ToString();
                            cSQL += ", WinPrice=" + winPrice.ToString();
                            cSQL += ", WinMoney=" + winMoney.ToString();
                            cSQL += " Where ID=" + dr["ID"].ToString();
                            MySqlHelper.ExecuteSql(cSQL, conn, trans);
                            #endregion
                        }
                        catch(Exception ex) { return "计算并保存错误:" + ex.Message; };
                        try
                        {
                            #region 内线通知(机器人除外)
                            if (winMoney > 0)
                            {
                                int uid = int.Parse(dr["uID"].ToString());
                                int isRobot = int.Parse(dr["isRobot"].ToString());
                                int isTest = int.Parse(dr["isTest"].ToString());
                                if (isRobot == 0)
                                {
                                    string notes = "恭喜您在" + myPublishGameName + "第" + dr["listno"].ToString().Trim() + "期中奖赢得了" + winMoney + "" + GetGoldName(isTest == 1 ? true : false) + "[url=/bbs/game/pk10.aspx?act=case]马上兑奖[/url]";
                                    AddSysMsg(conn, trans, 1, uid, notes);//开奖提示信息,1表示开奖信息
                                }
                            }
                            #endregion
                        }
                        catch(Exception ex) { return "内线通知错误:" + ex.Message; };
                    }
                }
                #endregion
                try
                {
                    #region 统计汇总信息，并更新派奖标记
                    int sumPayMoney = 0, sumWinMoney = 0, sumCaseMoney = 0;
                    cSQL = "Select ListID,sum(paymoney) as paymoney,sum(winmoney) as winmoney,sum(casemoney) as casemoney From tb_PK10_Buy Where ListID=" + id.ToString() + " Group By ListID";
                    DataTable dtSum = MySqlHelper.GetTable(conn, trans, cSQL);
                    if (dtSum.Rows.Count > 0)
                    {
                        int.TryParse(dtSum.Rows[0]["paymoney"].ToString(), out sumPayMoney);
                        int.TryParse(dtSum.Rows[0]["winmoney"].ToString(), out sumWinMoney);
                        int.TryParse(dtSum.Rows[0]["casemoney"].ToString(), out sumCaseMoney);
                    }
                    //
                    int sumPayCount = 0, sumWinCount = 0, sumCaseCount = 0;
                    cSQL = "Select Count(*) as count From tb_PK10_Buy Where ListID=" + id.ToString();
                    DataTable dtSumPay = MySqlHelper.GetTable(conn, trans, cSQL);
                    if (dtSumPay.Rows.Count > 0)
                        int.TryParse(dtSumPay.Rows[0]["count"].ToString(), out sumPayCount);
                    //
                    cSQL = "Select Count(*) as count From tb_PK10_Buy Where ListID=" + id.ToString() + " And WinMoney>0";
                    DataTable dtSumWin = MySqlHelper.GetTable(conn, trans, cSQL);
                    if (dtSumWin.Rows.Count > 0)
                        int.TryParse(dtSumWin.Rows[0]["count"].ToString(), out sumWinCount);
                    //
                    cSQL = "Select Count(*) as count From tb_PK10_Buy Where ListID=" + id.ToString() + " And CaseFlag=1";
                    DataTable dtSumCase = MySqlHelper.GetTable(conn, trans, cSQL);
                    if (dtSumCase.Rows.Count > 0)
                        int.TryParse(dtSumCase.Rows[0]["count"].ToString(), out sumCaseCount);
                    //
                    cSQL = "Update tb_PK10_List Set CalcFlag=1, PayMoney=" + sumPayMoney.ToString() + ", WinMoney=" + sumWinMoney.ToString() + ", CaseMoney=" + sumCaseMoney.ToString();
                    cSQL += ",PayCount=" + sumPayCount.ToString() + ", WinCount=" + sumWinCount.ToString() + ", CaseCount=" + sumCaseCount.ToString();
                    cSQL += " Where ID=" + id.ToString().Trim();
                    MySqlHelper.ExecuteSql(cSQL, conn, trans);
                    #endregion
                }
                catch (Exception ex) { return "统计汇总错误:" + ex.Message; };
            }
            catch(Exception ex)
            {
                cFlag = "CalcOpenData:" + ex.Message;
            }
            return cFlag;
        }
        #region 号码买法派奖计算
        private void CalcOpenData1(string[] Rates,string listnums, string numsdetail,int buyprice,DataTable dtBuyType,int buytypeID,out int WinNumsCount,out string WinNums,out decimal  winPrice,out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                if (drBuyType == null)
                    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail格式为例子（前二）：01|02#01|03 
                int numid = 0;
                int.TryParse(drBuyType["NumID"].ToString(), out numid); //买的号码是从第几位开始
                int numscount = 1;
                int.TryParse(drBuyType["NumsCount"].ToString(), out numscount); //单位或者多位投注
                if (numscount == 1)
                {
                    string result = GetWinCount(listnums, numsdetail, numid);
                    string[] results = result.Split('#');
                    WinNumsCount = int.Parse(results[0].ToString());
                    WinNums = results[1].ToString();
                    //
                    winPrice = decimal.Parse(Rates[WinNumsCount].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                    winMoney = Convert.ToInt32(buyprice * winPrice);
                }
                else
                {
                    GetWinCount2(Rates, buyprice, listnums, numsdetail, numid, out WinNumsCount, out WinNums, out winPrice, out winMoney);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CalcOpenData1 err:" + ex.Message);
            }
        }
        public string GetWinCount(string listnums, string numsdetail,int startnumid) //单号码投注（PK10只计算一注最高奖励）
        {
            string result = "";
            int maxCount = 0;
            string cMaxCountNums = "";
            //nums格式为01,02,03,04,05,06,07,08,09,10
            string[] aOpen = listnums.Split(',');
            //numsdetail格式为例子（前二）：01|02#01|03 
            string[] anums = numsdetail.Split('#');
            if(anums.Length>0)
            {
                for(int i=0;i<anums.Length;i++) //循环每一个有效下注
                {
                    int count = 0; //匹配个数
                    string[] abuy = anums[i].Split('|');
                    for(int j=0;j<abuy.Length;j++)
                    {
                        if (aOpen[j+ startnumid].ToString().Trim() == abuy[j].ToString().Trim())
                            count += 1;
                    }
                    //
                    if (count > maxCount)
                    {
                        maxCount = count;
                        cMaxCountNums = anums[i].ToString();
                    }
                }
            }
            result = maxCount.ToString().Trim() + "#" + cMaxCountNums;
            return result;
        }
        public void GetWinCount2(string[] Rates,int buyprice, string listnums, string numsdetail, int startnumid,out int WinNumsCount,out string WinNums,out decimal winPrice,out int winMoney) //多位投注
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            //nums格式为01,02,03,04,05,06,07,08,09,10
            string[] aOpen = listnums.Split(',');
            //numsdetail格式为例子（前二）：01|02#01|03 
            string[] anums = numsdetail.Split('#');
            if (anums.Length > 0)
            {
                for (int i = 0; i < anums.Length; i++) //循环每一个有效下注
                {
                    int count = 0; //匹配个数
                    string[] abuy = anums[i].Split('|');
                    for (int j = 0; j < abuy.Length; j++)
                    {
                        if (aOpen[j + startnumid].ToString().Trim() == abuy[j].ToString().Trim())
                            count += 1;
                    }
                    //
                    if (count > 0)
                    {
                        winPrice = decimal.Parse(Rates[count].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                        //
                        WinNumsCount++;
                        if (WinNums == "")
                            WinNums = anums[i] +","+count.ToString().Trim()+ "," + winPrice.ToString().Trim(); //中奖号码，匹配奖号个数，赔率
                        else
                            WinNums += "#" + anums[i] + "," + count.ToString().Trim() + "," + winPrice.ToString().Trim();
                        //
                        winMoney += Convert.ToInt32(buyprice * winPrice); //累计中奖金额
                    }
                    //
                }
            }
            if(WinNumsCount>0)
                winPrice = winMoney / WinNumsCount;
        }
        #endregion
        #region 大小买法计算
        private void CalcOpenData2(string[] Rates,string listnums, string numsdetail, int buyprice, DataTable dtBuyType, int buytypeID, out int WinNumsCount, out string WinNums, out decimal winPrice, out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                if (drBuyType == null)
                    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail为0或者1，0表示小，1表示大
                int numid = 0;
                int.TryParse(drBuyType["NumID"].ToString(), out numid);
                string[] openlists = listnums.Split(',');
                int value = 0, select = 0;
                int.TryParse(openlists[numid], out value);
                int.TryParse(numsdetail, out select);
                if (value > 5) //出大
                {
                    if (select == 1)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 9;
                    }
                }
                else //出小
                {
                    if (select == 0)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 8;
                    }
                }
                //
                winPrice = decimal.Parse(Rates[WinNumsCount].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                winMoney = Convert.ToInt32(buyprice * winPrice);
            }
            catch(Exception ex)
            {
                throw new Exception("CalcOpenData2 err:" + ex.Message);
            }
        }
        #endregion
        #region 单双买法计算
        private void CalcOpenData3(string[] Rates,string listnums, string numsdetail, int buyprice, DataTable dtBuyType, int buytypeID, out int WinNumsCount, out string WinNums, out decimal winPrice, out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                if (drBuyType == null)
                    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail为0或者1，0表示小，1表示大
                int numid = 0;
                int.TryParse(drBuyType["NumID"].ToString(), out numid);
                string[] openlists = listnums.Split(',');
                int value = 0, select = 0;
                int.TryParse(openlists[numid], out value);
                int.TryParse(numsdetail, out select);
                if (value % 2 == 0) //出双
                {
                    if (select == 1)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 9;
                    }
                }
                else //出单
                {
                    if (select == 0)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 8;
                    }
                }
                //
                winPrice = decimal.Parse(Rates[WinNumsCount].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                winMoney = Convert.ToInt32(buyprice * winPrice);
            }
            catch (Exception ex)
            {
                throw new Exception("CalcOpenData3 err:" + ex.Message);
            }
        }
        #endregion
        #region 龙虎买法计算
        private void CalcOpenData4(string[] Rates,string listnums, string numsdetail, int buyprice, DataTable dtBuyType, int buytypeID, out int WinNumsCount, out string WinNums, out decimal winPrice, out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                if (drBuyType == null)
                    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail为0或者1，0表示小，1表示大
                int numid = 0;
                int.TryParse(drBuyType["NumID"].ToString(), out numid);
                string[] openlists = listnums.Split(',');
                int value1 = 0, value2 = 0, select = 0;
                int.TryParse(openlists[numid], out value1);
                int.TryParse(openlists[9 - numid], out value2);
                int.TryParse(numsdetail, out select);
                if (value1 > value2) //出龙
                {
                    if (select == 1)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 9;
                    }
                }
                else //出虎
                {
                    if (select == 0)
                    {
                        WinNums = numsdetail;
                        WinNumsCount = 8;
                    }
                }
                //
                winPrice = decimal.Parse(Rates[WinNumsCount].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                winMoney = Convert.ToInt32(buyprice * winPrice);
            }
            catch (Exception ex)
            {
                throw new Exception("CalcOpenData4 err:" + ex.Message);
            }
        }
        #endregion
        #region 前五任选买法计算
        private void CalcOpenData5(string[] Rates,string listnums, string numsdetail, int buyprice, DataTable dtBuyType, int buytypeID, out int WinNumsCount, out string WinNums, out decimal winPrice, out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                //DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                //if (drBuyType == null)
                //    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail格式为例子（前五任选二）：01|02#03|04#01|05 
                string[] openlists = listnums.Split(',');
                List<string> lists = new List<string>();
                for (int i = 0; i < 5; i++) //取出开奖的前五个号码
                {
                    lists.Add(openlists[i].Trim());
                }
                string[] anums = numsdetail.Split('#'); //购买的任选组合号码
                                                        //
                for (int i = 0; i < anums.Length; i++) //循环每一个购买的有效组合
                {
                    string[] a = anums[i].Split('|');
                    int okflag = 0;
                    for (int j = 0; j < a.Length; j++)
                    {
                        if (lists.Contains(a[j]))
                        {
                            okflag++;
                        }
                    }
                    if (okflag > 0 && okflag == a.Length) //全中
                    {
                        //if (WinNums == "")
                        //    WinNums += anums[i]; //任选，允许多注中出
                        //else
                        //    WinNums += "#" + anums[i];
                        //WinNumsCount += 1;
                        winPrice = decimal.Parse(Rates[1].ToString()); //d1保存奖金倍数.
                                                                       //
                        WinNumsCount++;
                        if (WinNums == "")
                            WinNums = anums[i] + "," + okflag.ToString().Trim() + "," + winPrice.ToString().Trim(); //中奖号码，匹配奖号个数，赔率
                        else
                            WinNums += "#" + anums[i] + "," + okflag.ToString().Trim() + "," + winPrice.ToString().Trim();
                        //
                    }
                }
                //
                if (WinNumsCount > 0)
                {
                    winPrice = decimal.Parse(Rates[1].ToString()); //d1保存奖金倍数。
                    winMoney = Convert.ToInt32(buyprice * winPrice * WinNumsCount); //可以多注中出
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CalcOpenData5 err:" + ex.Message);
            }
        }
        #endregion
        #region 冠亚军和值买法计算
        private void CalcOpenData6(string[] Rates,string listnums, string numsdetail, int buyprice, DataTable dtBuyType, int buytypeID, out int WinNumsCount, out string WinNums, out decimal winPrice, out int winMoney)
        {
            winPrice = 0;
            winMoney = 0;
            WinNumsCount = 0;
            WinNums = "";
            try
            {
                DataRow drBuyType = dtBuyType.Rows.Find(buytypeID);
                if (drBuyType == null)
                    return;
                //nums格式为01,02,03,04,05,06,07,08,09,10
                //numsdetail为0或者1，0表示小，1表示大或者和值03#17
                string[] openlists = listnums.Split(',');
                int value1 = 0, value2 = 0, value = 0, select = 0;
                int.TryParse(openlists[0], out value1);
                int.TryParse(openlists[1], out value2);
                value = value1 + value2; //和值
                int.TryParse(numsdetail, out select);
                int type = int.Parse(drBuyType["Type"].ToString());
                switch (type)
                {
                    case 6: //和值号码
                        #region
                        string[] anums = numsdetail.Split('#');
                        value2 = 0;
                        for (int i = 0; i < anums.Length; i++)
                        {
                            value1 = 0;
                            int.TryParse(anums[i], out value1);
                            if (value1 == value)
                            {
                                value2 = value;
                                break;
                            }
                        }
                        if (value2 == value) //中出和值
                        {
                            WinNums = value.ToString().Trim();
                            WinNumsCount = GetNumCount6(value); //取和值对应的赔率的序号
                        }
                        #endregion
                        break;
                    case 2: //和值大小
                        #region
                        if (value != 11) //11不计算
                        {
                            if (value > 11) //出大
                            {
                                if (select == 1)
                                {
                                    WinNums = numsdetail;
                                    WinNumsCount = 9;
                                }
                            }
                            else//出小
                            {
                                if (select == 0)
                                {
                                    WinNums = numsdetail;
                                    WinNumsCount = 8;
                                }
                            }
                        }
                        else //和，一般返本金
                        {
                            WinNums = "和";
                            WinNumsCount = 3;
                        }
                        #endregion
                        break;
                    case 3: //和值单双
                        #region
                        if (value != 11)
                        {
                            if (value % 2 == 0) //出双
                            {
                                if (select == 1)
                                {
                                    WinNums = numsdetail;
                                    WinNumsCount = 9;
                                }
                            }
                            else//出单
                            {
                                if (select == 0)
                                {
                                    WinNums = numsdetail;
                                    WinNumsCount = 8;
                                }
                            }
                        }
                        else //和，一般返本金
                        {
                            WinNums = "和";
                            WinNumsCount = 3;
                        }
                        #endregion
                        break;
                }
                //
                winPrice = decimal.Parse(Rates[WinNumsCount].ToString()); //d0-d10分别保存匹配的号码个数从0到10个所对于的奖金倍数。
                winMoney = Convert.ToInt32(buyprice * winPrice);
            }
            catch (Exception ex)
            {
                throw new Exception("CalcOpenData6 err:" + ex.Message);
            }
        }
        private int GetNumCount6(int value) //根据和值的不同，返回有不同的赔率编号
        {
            int result = 0; //不中
            switch (value)
            {
                case 3:
                case 4:
                case 18:
                case 19:
                    result = 1;
                    break;
                case 5:
                case 6:
                case 16:
                case 17:
                    result = 2;
                    break;
                case 7:
                case 8:
                case 14:
                case 15:
                    result = 3;
                    break;
                case 9:
                case 10:
                case 12:
                case 13:
                    result = 4;
                    break;
                case 11:
                    result =5;
                    break;
            }
            return result;
        }
        #endregion
        #endregion
        //
        #region 下注付款
        public string GetBuyDescript(string cnums,PK10_BuyType obuytype)
        {
            List<string> validBuyNums = GenBuyNums(cnums, obuytype); //有效的单注列表,例如，从Nums（格式：01,02|03,02,05|06,10）到01|03|06和01|03|10等各注的列表
            string[] temp = cnums.Split(','); //判断是复式或单式的选号，有“，“表示复式(Nums的格式：01,02 | 03,02,05 | 06,10）
            string numtype = (temp.Length > 1) ? "-复式" : ""; //复式或者单式
            decimal[] aprices = { obuytype.d0, obuytype.d1, obuytype.d2, obuytype.d3, obuytype.d4, obuytype.d5, obuytype.d6, obuytype.d7, obuytype.d8, obuytype.d9, obuytype.d10 };
            //
            string str = "未知";
            switch (obuytype.Type)
            {
                case 1: //号码
                    str = cnums;
                    if (obuytype.NumsCount == 1)
                        str +="["+ aprices[1].ToString().Trim() + "倍]";
                    else
                    {
                        str += "[";
                        string str1 = "";
                        for (int i = 0; i <= obuytype.NumsCount; i++)
                        {
                            if (aprices[i] != 0)
                            {
                                if (str1=="")
                                    str1 += "中" + i.ToString().Trim() + "号" + aprices[i].ToString().Trim() + "倍";
                                else
                                    str1 += "," + "中" + i.ToString().Trim() + "号" + aprices[i].ToString().Trim() + "倍";
                            }
                        }
                        str += str1;
                        str += "]";
                    }
                    break;
                case 2: //大小
                    int select = 0;
                    int.TryParse(cnums, out select);
                    if (select == 1)
                        str = "买大["+ aprices[9].ToString().Trim()+"倍]";
                    if (select == 0)
                        str = "买小[" + aprices[8].ToString().Trim() + "倍]";
                    break;
                case 3: //单双
                    select = 0;
                    int.TryParse(cnums, out select);
                    if (select == 1)
                        str = "买双[" + aprices[9].ToString().Trim() + "倍]";
                    if (select == 0)
                        str = "买单[" + aprices[8].ToString().Trim() + "倍]";
                    break;
                case 4: //龙虎
                    select = 0;
                    int.TryParse(cnums, out select);
                    if (select == 1)
                        str = "买龙[" + aprices[9].ToString().Trim() + "倍]";
                    if (select == 0)
                        str = "买虎[" + aprices[8].ToString().Trim() + "倍]";
                    break;
                case 5: //任选
                    str = cnums+"["+ aprices[1].ToString().Trim() + "倍]";
                    break;
                case 6://和值
                    str = cnums;
                    break;
            }
            //
            string descript = "";
            if (validBuyNums.Count>1)
                 descript = "[" + obuytype.Name.Trim()+ numtype + "]" + str + "[" + validBuyNums.Count.ToString().Trim() + "注]"; //例：[猜冠军-复式]03,04|07,09[4注]
            else
                 descript = "[" + obuytype.Name.Trim() + numtype + "]" + str; //例：[猜冠军-复式]03,04|07,09[4注]
            return descript;
        }
        public string CheckAndGetNewNums(string cnums, PK10_BuyType obuytype) //过滤掉不合法字符，返回合法的下注字符串
        {
            string cResult = "";
            if (string.IsNullOrEmpty(cnums))
                return "";
            //
            string[] aNums0 = new string[] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" }; //号码
            string[] aNums1= new string[] { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19" }; //和值
            string[] aNums2= new string[] { "0", "1" }; //大小/单双/龙虎
            List<string> aNums;
            //
            switch(obuytype.ParentID)
            {
                case 1: //号码(正确格式是01,02|03,02,05|06,10)
                    #region
                    aNums = new List<string>(aNums0);
                    string[] alines = cnums.Split('|');
                    if(alines.Length>0)
                    {
                        for (int i = 0; i < alines.Length; i++)
                        {
                            string strline = alines[i].ToString().Trim();
                            if (!string.IsNullOrEmpty(strline))
                            {
                                string[] anums = strline.Split(',');
                                if (anums.Length > 0)
                                {
                                    string cline = "";
                                    //
                                    for (int j = 0; j < anums.Length; j++)
                                    {
                                        string strnum = anums[j].ToString().Trim();
                                        if (aNums.Contains(strnum) && !cline.Contains(strnum)) //判断是否合法的数字，以及是否有重复
                                        {
                                            if (obuytype.MultiSelect == 1) //允许复式
                                            {
                                                if (cline == "")
                                                    cline = strnum;
                                                else
                                                    cline += "," + strnum;
                                            }
                                            else //只允许单式
                                            {
                                                cline = strnum;
                                                break;
                                            }
                                        }
                                    }
                                    //
                                    if (cline != "")
                                    {
                                        if (cResult == "")
                                            cResult = cline;
                                        else
                                            cResult += "|" + cline;
                                    }
                                }
                            }
                        }
                        //过滤位数长度错误
                        if (cResult != "" && cResult.Split('|').Length != obuytype.NumsCount)
                            cResult = "";
                    }
                    #endregion
                    break;
                case 2: //大小
                case 3: //单双
                case 4: //龙虎
                    aNums = new List<string>(aNums2);
                    if (aNums.Contains(cnums))
                        cResult = cnums;
                    break;
                case 5: //任选(正确格式是02|0305|06)
                    #region
                    aNums = new List<string>(aNums0);
                    alines = cnums.Split('|');
                    if (alines.Length > 0)
                    {
                        for (int i = 0; i < alines.Length; i++)
                        {
                            string strline = alines[i].ToString().Trim();
                            if (aNums.Contains(strline) && !cResult.Contains(strline))//判断是否合法的数字，以及是否有重复
                            {
                                if (cResult == "")
                                    cResult = strline;
                                else
                                    cResult += "|" + strline;
                            }
                        }
                        //过滤位数长度错误
                        if (cResult != "" && cResult.Split('|').Length < obuytype.NumsCount)
                            cResult = "";
                    }
                    #endregion
                    break;
                case 6: //冠亚军和值
                    #region
                    switch(obuytype.Type)
                    {
                        case 6:
                            #region 和值
                            aNums = new List<string>(aNums1);
                            alines = cnums.Split('|');
                            if (alines.Length > 0)
                            {
                                for (int i = 0; i < alines.Length; i++)
                                {
                                    string strline = alines[i].ToString().Trim();
                                    if (aNums.Contains(strline) && !cResult.Contains(strline))//判断是否合法的数字，以及是否有重复
                                    {
                                        if (cResult == "")
                                            cResult = strline;
                                        else
                                            cResult += "|" + strline;
                                    }
                                }
                            }
                            #endregion
                            break;
                        default://大小单双
                            aNums = new List<string>(aNums2);
                            if (aNums.Contains(cnums))
                                cResult = cnums;
                            break;
                    }
                    #endregion
                    break;
                default:
                    break;
            }
            return cResult;
        }
        public List<string> GenBuyNums(string nums,PK10_BuyType buytype)//根据下注内容/号码组合，生成有效的下注内容/号码组合
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(nums))
                return resultList;
            //
            List<string> tmpList = new List<string>();
            switch (buytype.ParentID)
            {
                case 1: //直选
                    tmpList = GenBuyNums(nums, buytype.NumsCount);
                    break;
                case 5: //任选
                    tmpList = GenBuyNums2(nums, buytype.NumsCount);
                    break;
                case 6: //冠亚军和值
                    tmpList = GenBuyNums2(nums, buytype.NumsCount);
                    break;
                default:
                    tmpList.Add(nums);
                    break;
            }
            if (buytype.MultiSelect == 1) //允许复式投注
            {
                resultList = tmpList;
            }
            else
            {
                if(tmpList.Count>0) //单式投注
                    resultList.Add(tmpList[0]);
            }
            //
            return resultList;
        }
        //
        public List<string> GenBuyNums(string nums, int numsCount) //根据复式号码组合，生成有效的直选注数列表(nums格式如：01,02|03,02,05|06,10）
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(nums))
                return resultList;
            //
            string[] strs = nums.Split('|');
            //int numsCount = strs.Length;//购买的类型是前几？
            if (numsCount > 0)
            {
                string[][] aa = new string[numsCount][];
                #region 将号码组合先转换成一个2维数组
                //如01,02|03,02,05|06,10转为
                //aa[0][01,02]
                //aa[1][03,02,05,03]
                //aa[2][06,10]
                for (int i = 0; i < numsCount; i++)
                {
                    string cstrs = strs[i].ToString().Trim();
                    string[] a = cstrs.Split(',');
                    string ca = "";
                    if (cstrs != "" && a.Length > 0)
                    {
                        for (int j = 0; j < a.Length; j++)
                        {
                            string cca = a[j].ToString().Trim();
                            if (cca != "" && !ca.Contains(cca)) //剔除重复数字
                            {
                                if (ca == "")
                                    ca = cca;
                                else
                                    ca += "," + cca;
                            }
                        }
                    }
                    a = ca.Split(','); //重新组成没重复的数组，例如aa[1][03,02,05,03]变为aa[1][03,02,05]
                    aa[i] = a;
                }
                #endregion
                #region 拆分出组合列表
                List<string> tempList = new List<string>();
                tempList.AddRange(aa[0]); //先将第一列（第一位号码放入结果列表中）
                for (int i = 1; i < numsCount; i++)
                {
                    GenBuyNums(aa[i], tempList); //循环把第二、三位...放入结果列表中
                }
                #endregion
                #region 排除一些不完整的记录，输出结果表
                if (tempList.Count > 0)
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        if (tempList[i].Split('|').Length == numsCount)
                            resultList.Add(tempList[i]);
                    }
                #endregion
            }
            //
            return resultList;
        }
        private void GenBuyNums(string[] list, List<string> tempList)
        {
            string str = string.Empty;
            #region tempList合并下一位的号码
            for (int i = 0; i < tempList.Count; i++)
            {
                for (int j = 0; j < list.Length; j++)
                {
                    string c1 = tempList[i].ToString().Trim();
                    string c2 = list[j].ToString().Trim();
                    if (!c1.Contains(c2)) //剔除重复，剩下有效的（例如前三号码：01|01|01）
                    {
                        if (i == 0 && j == 0)
                            str = tempList[i] + "|" + list[j];
                        else
                            str += "#" + tempList[i] + "|" + list[j];
                    }
                }
            }
            #endregion
            #region 重新把合并下一位的字符串拆分成数组列表
            //例如前3复式，第一位时是"01#02#",合并第二位时候，str的值会例如："01|03#01|02#01|05..."，合并到第三位时，str的值会是："01|03|06#01|02|06#01|05|10..."
            tempList.Clear();
            tempList.AddRange(str.Split('#'));
            #endregion
        }
        //
        public List<string> GenBuyNums2(string nums, int numsCount)//根据复式号码组合，生成有效的任选注数列表（任选，nums格式如：01|02|05|06|10）
        {
            List<string> resultList = new List<string>();
            if (string.IsNullOrEmpty(nums))
                return resultList;
            //
            string[] ss = nums.Split('|');
            int begin = 0;
            int end = ss.Length - numsCount + 1;
            string s = "";
            GenBuyNums2(ss, begin, end, resultList, numsCount, s);
            //
            return resultList;

        }
        private void GenBuyNums2(string[] ss, int begin, int end, List<string> list, int count, string s)
        {
            for (int i = begin; i < end; i++)
            {
                string tmps;
                if (string.IsNullOrEmpty(s))
                    tmps = ss[i];
                else
                    tmps = s + "|" + ss[i];
                if (tmps.Split('|').Length == count)
                {
                    list.Add(tmps);
                }
                else
                {
                    int newbegin = i + 1;
                    int newend = end + 1;
                    if (newend > ss.Length)
                        newend = ss.Length;
                    GenBuyNums2(ss, newbegin, newend, list, count, tmps);
                }
            }
        }
        //
        public string Pay(bool CheckRate,PK10_Buy buy, string sourceUrl) //下注付款
        {
            string cFlag = "";
            //
            #region 下注付款
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = Pay(true,conn, trans, buy, sourceUrl);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            //
            return cFlag;
        }
        public string Pay(PK10_Buy buy,string sourceUrl) //下注付款
        {
            string cFlag = "";
            //
            #region 下注付款
            using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        cFlag = Pay(false,conn, trans, buy,sourceUrl);
                        if (cFlag == "")
                            trans.Commit();
                        else
                            trans.Rollback();
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        trans.Rollback();
                        throw new Exception(E.Message);
                    }
                }
                conn.Close();
            }
            #endregion
            //
            return cFlag;
        }
        private string Pay(bool CheckRate, SqlConnection conn, SqlTransaction trans, PK10_Buy buy, string sourceUrl)
        {
            #region 生成完整的购买记录
            //
            DataTable dtUser = new DataTable();
            if (buy.isTest == 0) //测试
                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold,IsSpier From tb_User Where ID=" + buy.uID.ToString().Trim());
            else
                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_PK10_TestUser Where ID=" + buy.uID.ToString().Trim());
            if (dtUser == null || dtUser.Rows.Count != 1)
                return "游客不能下注！";
            buy.uName = dtUser.Rows[0]["UsName"].ToString().Trim();
            //
            PK10_List list = ModelHelper.GetModelBySql<PK10_List>(conn, trans, "Select * From tb_PK10_List Where ID=" + buy.ListID.ToString().Trim());
            if (list == null)
                return "找不到下注的期号!";
            if (GetListStatus(list) != PK10_Stutas.在售)
                return "当前期号并不是“在售”状态";
            buy.ListNo = list.No.Trim();
            buy.ValidDate = list.ValidDate;
            //
            int currenNo = 0, lastOpenNo = 0, maxLostGetData = 0;
            int.TryParse(buy.ListNo, out currenNo);
            int.TryParse(ub.GetSub("StopSaleWhenLostData", xmlPath), out maxLostGetData);
            if (maxLostGetData <= 0)
                maxLostGetData = 1;
            DataTable dtLastOpenData = MySqlHelper.GetTable(conn, trans, "Select Top 1 * From tb_PK10_List Where OpenFlag=1 Order by BeginTime Desc");
            if (dtLastOpenData != null && dtLastOpenData.Rows.Count > 0)
                int.TryParse(dtLastOpenData.Rows[0]["No"].ToString(), out lastOpenNo);
            if (currenNo > (lastOpenNo + maxLostGetData))
                return "系统已经超过" + maxLostGetData + "期没读取开奖信息了，暂停下注！请联系客服...";
            //
            PK10_BuyType obuytype = GetBuyTypeByID(conn, trans, buy.BuyType);
            if (obuytype == null || obuytype.NumsCount <= 0 || obuytype.NumsCount > 10)
                return "选号类型错误！";
            //检测赔率变化
            if (CheckRate)
            {
                if (obuytype.PayLimitType == 1 && obuytype.RateFlag==1) //浮动赔率
                {
                    decimal oldRate = 0;
                    decimal.TryParse(buy.Rate, out oldRate);
                    decimal currRate = buy.Nums.Trim() == "0" ? obuytype.d8 : obuytype.d9;
                    if (oldRate != currRate)
                        return "【赔率发生了变化】原赔率：" + oldRate.ToString().Trim() + ",新赔率：" + currRate.ToString().Trim();
                }
            }
            buy.Rate = obuytype.d0.ToString().Trim() + "|" + obuytype.d1.ToString().Trim() + "|" + obuytype.d2.ToString().Trim() + "|" + obuytype.d3.ToString().Trim() + "|" + obuytype.d4.ToString().Trim() + "|" + obuytype.d5.ToString().Trim() + "|" + obuytype.d6.ToString().Trim() + "|" + obuytype.d7.ToString().Trim() + "|" + obuytype.d8.ToString().Trim() + "|" + obuytype.d9.ToString().Trim() + "|" + obuytype.d10.ToString().Trim() + "|";
            //
            string[] temp = buy.Nums.Split(','); //判断是复式或单式的选号，有“，“表示复式(Nums的格式：01,02 | 03,02,05 | 06,10）
            buy.NumType = (temp.Length > 1) ? 1 : 0; //复式或者单式
            List<string> validBuyNums = GenBuyNums(buy.Nums, obuytype); //有效的单注列表,例如，从Nums（格式：01,02|03,02,05|06,10）到01|03|06和01|03|10等各注的列表
            buy.BuyCount = validBuyNums.Count;
            if (validBuyNums.Count > 0)
            {
                string details = "";
                for (int i = 0; i < validBuyNums.Count; i++) //拆分复式成每一注有效的单式号码，并变换成字符串格式，例猜前二位：01|02#01|03#01#04
                {
                    if (i == 0)
                        details = validBuyNums[i];
                    else
                        details += "#" + validBuyNums[i];
                }
                buy.NumsDetail = details;
            }
            else
                return "有效注数为0";
            //
            string descript = GetBuyDescript(buy.Nums, obuytype);
            buy.BuyDescript = descript;
            //
            if (buy.BuyPrice <= 0)
                return "下注金额必须大于0";
            buy.PayMoney = buy.BuyPrice * buy.BuyCount * buy.BuyMulti;//buymulti为陪数，默认为1，暂时没用到此字段
            int oldgold = 0;
            int.TryParse(dtUser.Rows[0]["iGold"].ToString(), out oldgold);
            if (buy.PayMoney > oldgold)
                return "您的余额(共" + oldgold.ToString().Trim() + ")不足以支付！";
            #endregion
            string cSQL = "";
            #region 保存购买记录
            cSQL = "Insert into tb_PK10_Buy(ListID,ListNo,UID,UName,BuyTime,BuyType,NumType,Nums,NumsDetail,BuyDescript,BuyPrice,BuyCount,BuyMulti,PayMoney,isTest,isRobot,ValidDate,Rate) Values(";
            cSQL += buy.ListID.ToString();
            cSQL += "," + "'" + buy.ListNo.Trim() + "'";
            cSQL += "," + buy.uID.ToString();
            cSQL += "," + "'" + buy.uName.ToString().Trim() + "'";
            cSQL += "," + "'" + buy.BuyTime.ToString() + "'";
            cSQL += "," + buy.BuyType.ToString();
            cSQL += "," + buy.NumType.ToString();
            cSQL += "," + "'" + buy.Nums.Trim() + "'";
            cSQL += "," + "'" + buy.NumsDetail.Trim() + "'";
            cSQL += "," + "'" + buy.BuyDescript.Trim() + "'";
            cSQL += "," + buy.BuyPrice.ToString();
            cSQL += "," + buy.BuyCount.ToString();
            cSQL += "," + buy.BuyMulti.ToString();
            cSQL += "," + buy.PayMoney.ToString();
            cSQL += "," + buy.isTest.ToString();
            cSQL += "," + buy.isRobot.ToString();
            cSQL += "," + "'" + buy.ValidDate.ToString() + "'";
            cSQL += "," + "'" + buy.Rate.ToString() + "'";
            cSQL += ")";
            int id = MySqlHelper.InsertAndGetID(cSQL, conn, trans);
            if (id <= 0)
            {
                return "保存购买记录失败！";
            }
            buy.ID = id; //重置buy对象的ID
            #endregion
            //
            #region 其他逻辑判断
            DataTable dt;
            #region 每期每个用户ID最多总共下注限制
            cSQL = "Select sum(paymoney) as paymoney from tb_PK10_Buy Where ListID='" + buy.ListID.ToString().Trim() + "' and UID=" + buy.uID.ToString().Trim();
            dt = MySqlHelper.GetTable(conn, trans, cSQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                int value = 0;
                int.TryParse(dt.Rows[0][0].ToString(), out value);
                int maxvalue = 0;
                int.TryParse(ub.GetSub("UserMaxPay", xmlPath), out maxvalue);
                if (value > maxvalue && maxvalue > 0)
                    return "您本期下注的累计总额已超过允许的范围(每人每期限" + maxvalue.ToString().Trim() + ")";
            }
            else
                return "统计用户ID的总下注金额出错！";
            #endregion
            #region 每期每一玩法每一用户ID的下注限制
            cSQL = "Select sum(paymoney) as paymoney from tb_PK10_Buy Where ListID='" + buy.ListID.ToString().Trim() + "' and UID=" + buy.uID.ToString().Trim()+" and BuyType="+buy.BuyType.ToString().Trim();
            dt = MySqlHelper.GetTable(conn, trans, cSQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                int value = 0;
                int.TryParse(dt.Rows[0][0].ToString(), out value);
                int maxvalue = 0;
                int.TryParse(obuytype.PayLimitUser.ToString(), out maxvalue);
                if (value > maxvalue && maxvalue > 0)
                    return "您本期在这种玩法下注的累计总额已超过允许的范围(每人每期限" + maxvalue.ToString().Trim() + ")";
            }
            else
                return "统计当期当前玩法用户ID的总下注金额出错！";
            #endregion
            #region 每种普通玩法的限额投注上限;大小、单双、龙虎这些半数/可对冲投注的浮动额度限制
            if (obuytype.PayLimitType == 0) //每种普通玩法的限额投注上限        
            {
                cSQL = "Select sum(paymoney) as paymoney from tb_PK10_Buy Where ListID='" + buy.ListID.ToString().Trim() + "' and buytype=" + buy.BuyType.ToString().Trim();
                dt = MySqlHelper.GetTable(conn, trans, cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int value = 0;
                    int.TryParse(dt.Rows[0][0].ToString(), out value);
                    int maxvalue = obuytype.PayLimit;
                    if (value > maxvalue & maxvalue > 0)
                        return "本期所有人下注的总额已超过该类型所允许的范围(" + maxvalue.ToString().Trim() + ")";
                }
                else
                    return "统计下注类型的总下注金额出错！";
            }
            else //大小、单双、龙虎这些半数/可对冲投注的浮动额度限制
            {
                int value1 = 0, value2 = 0;
                int maxvalue = obuytype.PayLimit;
                //
                cSQL = "Select sum(paymoney) as paymoney from tb_PK10_Buy Where ListID='" + buy.ListID.ToString().Trim() + "' and buytype=" + buy.BuyType.ToString().Trim() + " and NumsDetail='0'";
                dt = MySqlHelper.GetTable(conn, trans, cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0][0].ToString(), out value1);
                }
                else
                    return "统计下注类型的总下注金额出错！";
                cSQL = "Select sum(paymoney) as paymoney from tb_PK10_Buy Where ListID='" + buy.ListID.ToString().Trim() + "' and buytype=" + buy.BuyType.ToString().Trim() + " and NumsDetail='1'";
                dt = MySqlHelper.GetTable(conn, trans, cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int.TryParse(dt.Rows[0][0].ToString(), out value2);
                }
                else
                    return "统计下注类型的总下注金额出错！";
                //
                if (Math.Abs(value1 - value2) > maxvalue && maxvalue > 0)
                    return "投注的额度超过改类型所允许的浮动额度(" + maxvalue.ToString().Trim() + ")";
            }
            #endregion
            #region 机器人下注次数限制
            if (buy.isRobot == 1)
            {
                cSQL = "Select count(*) as count from tb_PK10_Buy Where ListID=" + buy.ListID.ToString().Trim() + " and isRobot=1";
                dt = MySqlHelper.GetTable(conn, trans, cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    int value = 0;
                    int.TryParse(dt.Rows[0][0].ToString(), out value);
                    int maxvalue = 0;
                    int.TryParse(ub.GetSub("RobotMaxTimes", xmlPath), out maxvalue);
                    if (value > maxvalue)
                        return "您本期下注的累计次数已超过允许的范围(每期限" + maxvalue.ToString().Trim() + ")";
                }
                else
                    return "统计机器人的的总下注次数出错！";
            }
            #endregion
            #endregion
            //
            #region 更新本期累计付款金额
            cSQL = "Update tb_PK10_List Set PayMoney=PayMoney+" + buy.PayMoney.ToString() + ",PayCount=PayCount+1 Where ID=" + buy.ListID.ToString().Trim();
            int rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "更新本期累计付款金额失败！";
            #endregion
            #region 更新会员余额
            if (buy.isTest == 0)
                cSQL = "Update tb_User Set iGold=iGold-" + buy.PayMoney.ToString().Trim() + " Where ID=" + buy.uID.ToString().Trim();
            else
                cSQL = "Update tb_PK10_TestUser Set iGold=iGold-" + buy.PayMoney.ToString().Trim() + " Where ID=" + buy.uID.ToString().Trim();
            rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "更新会员余额失败！";
            #endregion
            #region 更新消费记录tb_GoldLogs
            cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
            cSQL += "0";
            cSQL += ",'" + sourceUrl + "'";//操作的文件名
            cSQL += "," + buy.uID.ToString().Trim();
            cSQL += ",'" + buy.uName.Trim() + "'";
            cSQL += "," + (buy.PayMoney * -1).ToString().Trim();
            cSQL += "," + (oldgold - buy.PayMoney).ToString().Trim();
            // cSQL += ",'" + myPublishGameName.Trim() + "投注(第" + buy.ListNo.Trim() + "期ID" + buy.ID.ToString().Trim() + ")" + buy.BuyDescript.Trim() + "'";
            cSQL += ",'" + myPublishGameName.Trim() + "投注(第[url=./game/PK10.aspx?act=view&amp;id=" + buy.ListID + "]" + buy.ListNo.Trim() + "[/url]期ID" + buy.ID.ToString().Trim() + ")" + buy.BuyDescript.Trim() + "'";
            cSQL += ",'" + buy.BuyTime.ToString().Trim() + "'";
            cSQL += ",0,0)";
            rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "更新消费记录失败！";
            #endregion
            #region 添加消费记录到会员ID：104（投注，增加ID104的余额，兑奖减少ID104的余额，则ID104的余额=PK10的总赢利）
            int usID2 = 104;
            if (buy.isRobot == 0 && buy.isTest==0)
            {
                if (dtUser.Rows[0]["IsSpier"].ToString().Trim() == "0") //非系统号
                {
                    DataTable dtUser2 = new DataTable();
                    dtUser2 = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_User Where ID=" + usID2.ToString().Trim());
                    decimal oldgold2 = 0;
                    string usName2 = "";
                    if (dtUser2 != null && dtUser2.Rows.Count > 0)
                    {
                        decimal.TryParse(dtUser2.Rows[0]["iGold"].ToString(), out oldgold2);
                        usName2 = dtUser2.Rows[0]["UsName"].ToString().Trim();
                    }
                    cSQL = "Update tb_User Set iGold=iGold+" + buy.PayMoney.ToString().Trim() + " Where ID=" + usID2.ToString().Trim();
                    MySqlHelper.ExecuteSql(cSQL, conn, trans);
                    //
                    cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
                    cSQL += "0";
                    cSQL += ",'" + sourceUrl + "'";//操作的文件名
                    cSQL += "," + usID2.ToString().Trim(); //uid
                    cSQL += ",'" + usName2 + "'"; //uname
                    cSQL += "," + buy.PayMoney.ToString().Trim(); //增加
                    cSQL += "," + (oldgold2 + buy.PayMoney).ToString().Trim();
                    cSQL += ",'" + "ID:" + "[url=forumlog.aspx?act=xview&amp;uid=" + buy.uID + "]" + buy.uID.ToString().Trim() + "[/url]" + "在" + myPublishGameName.Trim() + "第[url=./game/PK10.aspx?act=view&amp;id=" + buy.ListID + "]" + buy.ListNo.Trim() + "[/url]期投注" + buy.BuyDescript.Trim() + "标识ID(" + buy.ID.ToString().Trim() + ")" + "'";
                    cSQL += ",'" + buy.BuyTime.ToString().Trim() + "'";
                    cSQL += ",0,0)";
                    MySqlHelper.ExecuteSql(cSQL, conn, trans);
                }
            }
            #endregion
            #region 更新排行榜tb_TopLists
            cSQL = "select count(1) as count from tb_Toplist where UsId=" + buy.uID.ToString().Trim() + " and Types=" + myPublishGameType.ToString().Trim();
            DataTable dtTopList = MySqlHelper.GetTable(conn, trans, cSQL);
            if (dtTopList != null && dtTopList.Rows.Count > 0)
            {
                if (dtTopList.Rows[0]["count"].ToString().Trim() == "0")
                {
                    cSQL = "Insert into tb_TopList(Types,UsID,UsName,PutNum,PutGold,WinNum,WinGold) Values(";
                    cSQL += myPublishGameType.ToString().Trim();
                    cSQL += "," + buy.uID.ToString().Trim();
                    cSQL += "," + "'" + buy.uName.Trim() + "'";
                    cSQL += ",1," + buy.PayMoney.ToString().Trim();
                    cSQL += ",0,0)";
                }
                else
                    cSQL = "Update tb_TopList Set PutNum=1,PutGold=" + buy.PayMoney.ToString().Trim();
                //
                rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                if (rows <= 0)
                    return "更新排行榜失败！";
            }
            else
                return "更新排行榜失败！";
            #endregion
            #region 添加动态消息记录tb_Action
            //string wText = "[url=/bbs/uinfo.aspx?uid=" + buy.uID + "]" + buy.uName + "[/url]在[url=/bbs/game/PK10.aspx]PK拾.第" + buy.ListNo + "期[/url]下注" + Convert.ToInt64(buy.PayMoney) + GetGoldName(buy.isTest == 1 ? true : false) + "";
            string wText = "[url=/bbs/uinfo.aspx?uid=" + buy.uID + "]" + buy.uName + "[/url]在[url=/bbs/game/PK10.aspx]PK拾.第" + buy.ListNo + "期[/url]下注**"  + GetGoldName(buy.isTest == 1 ? true : false) + "";
            cSQL = "Insert into tb_Action(Types,NodeId,UsID,UsName,Notes,AddTime) Values(";
            cSQL += myPublishGameType.ToString().Trim();
            cSQL += "," + buy.ID.ToString().Trim();
            cSQL += "," + buy.uID.ToString().Trim();
            cSQL += ",'" + buy.uName.ToString().Trim() + "'";
            cSQL += ",'" + wText + "'";
            cSQL += ",'" + buy.BuyTime.ToString().Trim() + "'";
            cSQL += ")";
            rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "更新消息记录失败！";
            #endregion
            //
            return "";
        }
        #endregion
        #region 兑奖
        public string CaseRobot(string sourceUrl)
        {
            try
            {
                int rate = 0, charge = 0;
                int.TryParse(ub.GetSub("WinChargeRate", xmlPath), out rate); //扣除的费用比率
                int.TryParse(ub.GetSub("WinCharge", xmlPath), out charge);//每笔扣除的固定费用
                int casemoney = 0, sumCaseMoney = 0, errors = 0;
                DataTable dt = MySqlHelper.GetTable("Select ID From tb_PK10_Buy Where isRobot=1 And WinMoney>0 and CaseFlag=0 and ValidFlag=1");
                foreach (DataRow dr in dt.Rows)
                {
                    int buyid = Convert.ToInt32(dr["ID"].ToString());
                    string cFlag = Case(buyid, rate, charge, out casemoney, sourceUrl);
                    if (string.IsNullOrEmpty(cFlag))
                    {
                        sumCaseMoney += casemoney;
                    }
                    else
                    {
                        errors += casemoney;
                    }
                }
                if (errors != 0)
                    return "有" + errors.ToString() + "个兑奖失败！";
            }
            catch (Exception ex)
            {
                throw new Exception("Case Error" + ex.Message);
            }
            return "";
        }
        public string Case(int uid,string sourceUrl) //用户的全部兑奖
        {
            try
            {
                int rate = 0, charge = 0;
                int.TryParse(ub.GetSub("WinChargeRate", xmlPath), out rate); //扣除的费用比率
                int.TryParse(ub.GetSub("WinCharge", xmlPath), out charge);//每笔扣除的固定费用
                int casemoney = 0, sumCaseMoney = 0, errors = 0;
                DataTable dt = MySqlHelper.GetTable("Select ID From tb_PK10_Buy Where uid=" + uid.ToString().Trim() + " And WinMoney>0 and CaseFlag=0 and ValidFlag=1");
                foreach(DataRow dr in dt.Rows)
                {
                    int buyid = Convert.ToInt32(dr["ID"].ToString());
                    string cFlag = Case(buyid, rate, charge, out casemoney, sourceUrl);
                    if (string.IsNullOrEmpty(cFlag))
                    {
                        sumCaseMoney += casemoney;
                    }
                    else
                    {
                        errors += casemoney;
                    }
                }
                if (errors!= 0)
                    return "有"+errors.ToString()+"个兑奖失败！";
            }
            catch (Exception ex)
            {
                throw new Exception("Case Error" + ex.Message);
            }
            return "";
        }
        public string Case(int buyid,int rate,int charges,out int casemoney,string sourceUrl) //兑奖
        {
            casemoney = 0;
            #region 兑奖
            try
            {
                PK10_Buy buy = GetBuyByID(buyid);
                #region 逻辑判断
                if (buy == null)
                    return "找不到购买记录！";
                if (buy.CaseFlag!= 0)
                    return "已兑奖，不能重复兑奖！";
                PK10_List list = GetListByID(buy.ListID);
                if (list == null || list.ValidFlag == 0)
                    return "已过有效期，不能兑奖！";
                if (list.WinMoney == 0)
                    return "没有中奖！";
                buy.CaseFlag = 1;
                buy.Charges = Convert.ToInt32((buy.WinMoney * rate)/1000) + charges;//总扣除的费用=扣除的费用比率* 总奖金-每笔扣除的固定费用 = 兑奖金额(可以设定用费用比率进行扣费，也可设定固定的费用扣除，也可以两者）
                buy.CaseMoney = buy.WinMoney - buy.Charges;//总奖金-总扣除的费用
                buy.CaseTime = DateTime.Now;
                casemoney = buy.CaseMoney;
                if (casemoney < 0)
                    return "兑奖金额不能为负数！(总奖金=" + buy.WinMoney.ToString().Trim() + "扣除费用=" + (Convert.ToInt32((buy.WinMoney * rate)/1000) + charges).ToString().Trim() + ")";
                #endregion
                #region 保存数据
                using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string cFlag = Case(conn, trans, buy, sourceUrl);
                            if (cFlag == "")
                                trans.Commit();
                            else
                            {
                                trans.Rollback();
                                return cFlag;
                            }
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            trans.Rollback();
                            throw new Exception(E.Message);
                        }
                    }
                    conn.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Case Error" + ex.Message);
            }
            #endregion
            return "";
        }
        private string Case(SqlConnection conn, SqlTransaction trans,PK10_Buy buy,string sourceUrl)
        {
            string cSQL = "";
            #region 保存兑奖记录信息
            cSQL = "Update tb_PK10_Buy Set ";
            cSQL += "CaseFlag=1";
            cSQL += "," + "CaseMoney=" + buy.CaseMoney.ToString().Trim();
            cSQL += "," + "CaseTime='" + buy.CaseTime.ToString().Trim() + "'";
            cSQL += "," + "Charges=" + buy.Charges.ToString().Trim();
            cSQL += " Where CaseFlag=0 and ID=" + buy.ID.ToString().Trim();
            int rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "保存兑奖记录失败！";
            #endregion
            #region 更新本期累计付款金额
            cSQL = "Update tb_PK10_List Set CaseMoney=CaseMoney+" + buy.CaseMoney.ToString() + ",CaseCount=CaseCount+1 Where ID=" + buy.ListID.ToString().Trim();
            rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (rows <= 0)
                return "更新本期累计兑奖金额失败！";
            #endregion
            #region 更新相关数据
            DataTable dtUser;
            if (buy.isTest == 0) //测试
                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold,IsSpier From tb_User Where ID=" + buy.uID.ToString().Trim());
            else
                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_PK10_TestUser Where ID=" + buy.uID.ToString().Trim());
            if (dtUser != null && dtUser.Rows.Count > 0)
            {
                int oldgold = int.Parse(dtUser.Rows[0]["iGold"].ToString());
                #region 更新会员余额
                if (buy.isTest == 0)
                    cSQL = "Update tb_User Set iGold=iGold+" + buy.CaseMoney.ToString().Trim() + " Where ID=" + buy.uID.ToString().Trim();
                else
                    cSQL = "Update tb_PK10_TestUser Set iGold=iGold+" + buy.CaseMoney.ToString().Trim() + " Where ID=" + buy.uID.ToString().Trim();
                rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                if (rows <= 0)
                    return "更新会员余额失败！";
                #endregion
                #region 更新消费记录tb_GoldLogs
                cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
                cSQL += "0";
                cSQL += ",'" + sourceUrl + "'";//操作的文件名
                cSQL += "," + buy.uID.ToString().Trim();
                cSQL += ",'" + buy.uName.Trim() + "'";
                cSQL += "," + buy.CaseMoney.ToString().Trim();
                cSQL += "," + (oldgold + buy.CaseMoney).ToString().Trim();
                //cSQL += ",'" + myPublishGameName.Trim() + "兑奖(第"+buy.ListNo.Trim()+"期)ID"+buy.ID.ToString().Trim()+"'";
                cSQL += ",'" + myPublishGameName.Trim() + "兑奖(第[url=./game/PK10.aspx?act=view&amp;id=" + buy.ListID + "]" + buy.ListNo.Trim() + "[/url]期)ID" + buy.ID.ToString().Trim() + "'";
                cSQL += ",'" + buy.CaseTime.ToString().Trim() + "'";
                cSQL += ",0,0)";
                rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                if (rows <= 0)
                    return "更新消费记录失败！";
                #endregion
                #region 添加消费记录到会员ID：104（投注，增加ID104的余额，兑奖减少ID104的余额，则ID104的余额=PK10的总赢利）
                int usID2 = 104;
                if (buy.isRobot == 0 && buy.isTest==0)
                {
                    if (dtUser.Rows[0]["IsSpier"].ToString().Trim() == "0") //非系统号
                    {
                        DataTable dtUser2 = new DataTable();
                        dtUser2 = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_User Where ID=" + usID2.ToString().Trim());
                        decimal oldgold2 = 0;
                        string usName2 = "";
                        if (dtUser2 != null && dtUser2.Rows.Count > 0)
                        {
                            decimal.TryParse(dtUser2.Rows[0]["iGold"].ToString(), out oldgold2);
                            usName2 = dtUser2.Rows[0]["UsName"].ToString().Trim();
                        }
                        cSQL = "Update tb_User Set iGold=iGold-" + buy.CaseMoney.ToString().Trim() + " Where ID=" + usID2.ToString().Trim();
                        rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                        //
                        cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
                        cSQL += "0";
                        cSQL += ",'" + sourceUrl + "'";//操作的文件名
                        cSQL += "," + usID2.ToString().Trim(); //uid
                        cSQL += ",'" + usName2 + "'"; //uname
                        cSQL += "," + (buy.CaseMoney * -1).ToString().Trim(); //减少
                        cSQL += "," + (oldgold2 - buy.CaseMoney).ToString().Trim();
                        cSQL += ",'" + "ID:" + "[url=forumlog.aspx?act=xview&amp;uid=" + buy.uID + "]" + buy.uID.ToString().Trim() + "[/url]" + "在" + myPublishGameName.Trim() + "第[url=./game/PK10.aspx?act=view&amp;id=" + buy.ListID + "]" + buy.ListNo.Trim() + "[/url]期兑奖" + "-标识ID(" + buy.ID.ToString().Trim() + ")" + "'";
                        cSQL += ",'" + buy.CaseTime.ToString().Trim() + "'";
                        cSQL += ",0,0)";
                        MySqlHelper.ExecuteSql(cSQL, conn, trans);
                    }
                }
                #endregion
            }
            #endregion
            return "";
        }
        #endregion
        #region 返赢返负
        public string AddUserMoney(bool SaveFlag,bool isTest,bool winFlag,DateTime beginTime,DateTime endTime,int Rate,int iMoney, string sourceUrl,out int reCount,out int reMoney)
        {
            reCount = 0;
            reMoney = 0;
            try
            {
                #region 保存数据
                using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string cFlag = AddUserMoney(conn, trans,SaveFlag, isTest, winFlag, beginTime, endTime, Rate, iMoney, sourceUrl,out reCount ,out reMoney);
                            if (cFlag == "")
                                trans.Commit();
                            else
                            {
                                trans.Rollback();
                                return cFlag;
                            }
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            trans.Rollback();
                            throw new Exception(E.Message);
                        }
                    }
                    conn.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("AddUserMoney Error:" + ex.Message);
            }
            return "";
        }
        private string AddUserMoney(SqlConnection conn,SqlTransaction trans,bool SaveFlag, bool isTest, bool winFlag, DateTime beginTime, DateTime endTime, int Rate, int iMoney,string sourceUrl,out int reCount,out int reMoney)
        {
            reCount = 0;
            reMoney = 0;
            string cSQL = "";
            cSQL = "Select UID,sum(WinMoney-PayMoney) as iMoney From tb_PK10_Buy Where BuyTime>='" + beginTime.ToShortDateString() + "' and BuyTime<'" + endTime.AddDays(1).ToShortDateString() + "'" + " Group By UID";
            DataTable dt = MySqlHelper.GetTable(conn, trans, cSQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    int usid = int.Parse(dr["UID"].ToString());
                    int money = int.Parse(dr["iMoney"].ToString());
                    string title = "", descript = "";
                    bool addflag = false;
                    #region 计算返还金额
                    if (winFlag) //返赢
                    {
                        if (money > 0 && money >= iMoney)
                        {
                            money= Convert.ToInt32(money * (Rate * 0.001));
                            title = "PK拾返赢";
                            descript = "根据你PK拾排行榜上的赢利情况，系统自动返还了" + money.ToString().Trim() + "" + GetGoldName(isTest) + "[url=/bbs/game/PK10.aspx]进入PK拾[/url]";
                            addflag = true;
                        }
                    }
                    else //返负
                    {
                        if (money < 0 && money < (-iMoney))
                        {
                            money = Convert.ToInt32(-money * (Rate * 0.001));
                            title = "PK拾返负";
                            descript = "根据你PK拾排行榜上的亏损情况，系统自动返还了" + money.ToString().Trim() + "" + GetGoldName(isTest) + "[url=/bbs/game/PK10.aspx]进入PK拾[/url]";
                            addflag = true;
                        }
                    }
                    #endregion
                    #region 更新相关数据
                    if (addflag && money>0)
                    {
                        reCount += 1;
                        reMoney += money;
                        if (SaveFlag)
                        {
                            #region 更新会员余额
                            DataTable dtUser;
                            if (!isTest) //测试
                                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_User Where ID=" + usid.ToString().Trim());
                            else
                                dtUser = MySqlHelper.GetTable(conn, trans, "Select ID,UsName,iGold From tb_PK10_TestUser Where ID=" + usid.ToString().Trim());
                            if (dtUser != null && dtUser.Rows.Count > 0)
                            {
                                int oldgold = int.Parse(dtUser.Rows[0]["iGold"].ToString());
                                string usname = dtUser.Rows[0]["UsName"].ToString().Trim();
                                #region 更新会员余额
                                if (!isTest)
                                    cSQL = "Update tb_User Set iGold=iGold+" + money.ToString().Trim() + " Where ID=" + usid.ToString().Trim();
                                else
                                    cSQL = "Update tb_PK10_TestUser Set iGold=iGold+" + money.ToString().Trim() + " Where ID=" + usid.ToString().Trim();
                                int rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                                if (rows <= 0)
                                    return "更新会员余额失败！";
                                #endregion
                                #region 更新消费记录tb_GoldLogs
                                cSQL = "Insert into tb_Goldlog(Types,Purl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag,IsCheck) Values(";
                                cSQL += "0";
                                cSQL += ",'" + sourceUrl + "'";//操作的文件名
                                cSQL += "," + usid.ToString().Trim();
                                cSQL += ",'" + usname.Trim() + "'";
                                cSQL += "," + money.ToString().Trim();
                                cSQL += "," + (oldgold + money).ToString().Trim();
                                cSQL += ",'" + title + "'";
                                cSQL += ",'" + DateTime.Now.ToString() + "'";
                                cSQL += ",0,0)";
                                rows = MySqlHelper.ExecuteSql(cSQL, conn, trans);
                                if (rows <= 0)
                                    return "更新消费记录失败！";
                                #endregion
                            }
                            #endregion
                            //发内线
                            AddSysMsg(conn, trans,0, usid, descript);
                        }
                    }
                    #endregion
                }
            }
            //
            return "";
        }
        #endregion
        #region 删除、清空
        public string DeleteList(PK10_List list)
        {
            if (list == null)
                return "找不到记录";
            #region 启动事务，删除
            try
            {
                using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string cFlag = DeleteList(conn, trans, list);
                            if (cFlag == "")
                                trans.Commit();
                            else
                            {
                                trans.Rollback();
                                return cFlag;
                            }
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            trans.Rollback();
                            throw new Exception(E.Message);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteList Error:" + ex.Message);
            }
            #endregion
            return "";
        }
        private string DeleteList(SqlConnection conn,SqlTransaction trans, PK10_List list)
        {
            string cSQL = "";
            //
            cSQL = "Delete From tb_PK10_List Where ID=" + list.ID.ToString().Trim() +" and CalcFlag=0";//忽略已经派奖的
            int row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            if (row == 0)
                return "找不到要删除的记录;或者是已经派奖，不能删除！";
            cSQL = "Delete From tb_PK10_Buy Where listID=" + list.ID.ToString().Trim();
            row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            //
            return "";
        }
        public string DeleteAll()
        {
            #region 启动事务，删除
            try
            {
                using (SqlConnection conn = new SqlConnection(MySqlHelper.connectionString))
                {
                    conn.Open();
                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            string cFlag = DeleteAll(conn, trans);
                            if (cFlag == "")
                                trans.Commit();
                            else
                            {
                                trans.Rollback();
                                return cFlag;
                            }
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            trans.Rollback();
                            throw new Exception(E.Message);
                        }
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("DeleteAll Error:" + ex.Message);
            }
            #endregion
            return "";
        }
        private string DeleteAll(SqlConnection conn,SqlTransaction trans)
        {
            string cSQL = "";
            //
            cSQL = "Delete From tb_PK10_List";
            int row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            cSQL = "Delete From tb_PK10_Buy";
            row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            cSQL = "Delete From tb_PK10_TestUser";
            row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            cSQL = "Update tb_PK10_Base Set CurrentSaleDate='1900-01-01'";
            row = MySqlHelper.ExecuteSql(cSQL, conn, trans);
            //
            return "";
        }
        #endregion
        //
        #region 数据分析
        public List<PK10_Report> GetReportDatas(PK10_BuyType obuytype, int calccount, int pageSize, int pageIndex, out int recordCount, out int[][] sumList)//统计出分析数据（分页）
        {
            List<PK10_Report> resultlist = new List<PK10_Report>();
            List<PK10_Report> list = new List<PK10_Report>();
            sumList = new int[4][];
            sumList[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存当前遗漏结果
            sumList[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存最大遗漏结果
            sumList[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存平均遗漏结果
            sumList[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存总次数结果
            recordCount = 0;
            #region
            try
            {
                if (calccount > 0)
                {
                    string cWhere = "";
                    #region 统计的期数范围
                    cWhere = " id in (Select Top " + calccount + " ID From tb_PK10_List Where OpenFlag=1 Order by no desc)";
                    #endregion
                    #region 取得统计结果的实际总记录数
                    string countString = "SELECT COUNT(*) FROM tb_PK10_List where " + cWhere + "";
                    recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
                    int pageCount = 0;
                    if (recordCount > 0)
                    {
                        pageCount = BCW.Common.BasePage.CalcPageCount(recordCount, pageSize, ref pageIndex);
                    }
                    else
                    {
                        return list;
                    }
                    #endregion
                    #region 读取出相关记录
                    string queryString = "";
                    queryString = "SELECT ID,No,Nums,Date From tb_PK10_List where " + cWhere + " Order by No DESC";
                    using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
                    {
                        while (reader.Read())
                        {
                            PK10_Report item = new PK10_Report();
                            item.ID = reader.GetInt32(0);
                            item.No = reader.GetString(1);
                            item.Nums = reader.GetString(2);
                            item.Date = reader.GetDateTime(3);
                            list.Add(item);
                        }
                    }
                    #endregion
                    #region 计算
                    switch (obuytype.ParentID)
                    {
                        case 1:
                            GenReportData1(list, obuytype, sumList);
                            break;
                        case 2:
                        case 3:
                            GenReportData23(list, obuytype, sumList);
                            break;
                        case 4:
                            GenReportData4(list, obuytype, sumList);
                            break;
                        case 5:
                            GenReportData5(list, obuytype, sumList);
                            break;
                        case 6:
                            GenReportData6(list, obuytype, sumList);
                            break;
                    }
                    #endregion
                    #region 输出指定页的数据
                    int stratIndex = (pageIndex - 1) * pageSize;
                    int endIndex = pageIndex * pageSize;
                    int k = 0;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (k >= stratIndex && k < endIndex)
                        {
                            resultlist.Add(list[i]);
                        }
                        if (k == endIndex)
                            break;
                        k++;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetReportDatas Error:" + ex.Message);
            }
            #endregion
            //
            return resultlist;
        }
        #region 生成各种类型分析数据
        private void GenReportData1(List<PK10_Report> list, PK10_BuyType obuytype, int[][] sumList) //号码类型分析
        {
            if (list.Count > 0)
            {
                string[] abase = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    #region 生成需要查找的号码
                    List<string> lnums = new List<string>();
                    string[] anums = list[i].Nums.Split(',');
                    if (obuytype.NumsCount > 1) //多位号码/前几位，分布图走势形式
                    {
                        for (int j = 0; j < obuytype.NumsCount; j++)
                        {
                            lnums.Add(anums[j].Trim());
                        }
                    }
                    else //某一位的定位走势形式
                    {
                        lnums.Add(anums[obuytype.NumID].Trim());
                    }
                    #endregion
                    #region 生成遗漏值
                    for (int j = 1; j <= 10; j++) //循环10个号码
                    {
                        int currentlost = sumList[0][j - 1];
                        if (lnums.Contains(abase[j - 1])) //中出了号码
                        {
                            sumList[3][j - 1] = sumList[3][j - 1] + 1;//总出现次数+1
                                                                      //更新最大遗漏值
                            if (sumList[1][j - 1] < currentlost)
                                sumList[1][j - 1] = currentlost;
                            //

                            //
                            sumList[0][j - 1] = 0; //重置该位置的当前遗漏为0
                        }
                        else
                        {
                            sumList[0][j - 1] = sumList[0][j - 1] + 1;
                        }
                    }
                    #endregion
                    #region 生成显示的格式
                    string str = "";
                    for (int m = 0; m < 10; m++)
                    {
                        if (sumList[0][m] == 0)
                        {
                            str += "<font color=\"red\">" + " " + abase[m].Trim() + "</font>";
                        }
                        else
                        {
                            string cc = sumList[0][m].ToString().Trim();
                            if (cc.Length == 1)
                                cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                            str += "<font color=\"Grey\">" + " " + cc + "</font>";
                        }
                    }
                    list[i].ShowNums = str;
                    #endregion
                }
            }
        }
        private void GenReportData23(List<PK10_Report> list, PK10_BuyType obuytype, int[][] sumList)//大小单双类型分析
        {
            if (list.Count > 0)
            {
                string[] abase = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    #region 取到指定的位数号码的值
                    string[] anums = list[i].Nums.Split(',');
                    int value=int.Parse(anums[obuytype.NumID]);
                    #endregion
                    #region 生成遗漏值
                    //sumList[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存当前遗漏结果{大，小，单，双,0,0,0,0,0,0}
                    //sumList[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存最大遗漏结果{大，小，单，双,0,0,0,0,0,0}
                    //sumList[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存平均遗漏结果{大，小，单，双,0,0,0,0,0,0}
                    //sumList[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存总次数结果  {大，小，单，双,0,0,0,0,0,0}
                    if (value>5) //大
                    {
                        int currentlost = sumList[0][0]; //大的当前遗漏
                        sumList[3][0] = sumList[3][0] + 1;//大的总出现次数+1
                        if (sumList[1][0] < currentlost)  //更新大的最大遗漏值
                            sumList[1][0] = currentlost;
                        sumList[0][0] = 0;//重置大的当前遗漏为0
                        //
                        sumList[0][1] = sumList[0][1]+1;//重置小的当前遗漏+1
                    }
                    else //小
                    {
                        int currentlost = sumList[0][1]; //小的当前遗漏
                        sumList[3][1] = sumList[3][1] + 1;//小的总出现次数+1
                        if (sumList[1][1] < currentlost)  //更新小的最大遗漏值
                            sumList[1][1] = currentlost;
                        sumList[0][1] = 0;//重置小的当前遗漏为0
                        //
                        sumList[0][0] = sumList[0][0] + 1;//重置大的当前遗漏+1
                    }
                    if (value % 2==0) //双
                    {
                        int currentlost = sumList[0][3]; //双的当前遗漏
                        sumList[3][3] = sumList[3][3] + 1;//双的总出现次数+1
                        if (sumList[1][3] < currentlost)  //更新双的最大遗漏值
                            sumList[1][3] = currentlost;
                        sumList[0][3] = 0;//重置双的当前遗漏为0
                        //
                        sumList[0][2] = sumList[0][2] + 1;//重置单的当前遗漏+1
                    }
                    else //单
                    {
                        int currentlost = sumList[0][2]; //单的当前遗漏
                        sumList[3][2] = sumList[3][2] + 1;//单的总出现次数+1
                        if (sumList[1][2] < currentlost)  //更新单的最大遗漏值
                            sumList[1][2] = currentlost;
                        sumList[0][2] = 0;//重置单的当前遗漏为0
                        //
                        sumList[0][3] = sumList[0][3] + 1;//重置双的当前遗漏+1
                    }

                    #endregion
                    #region 生成显示的格式
                    //显示号码
                    string showStr = "";
                    for (int j = 0; j < 10; j++)
                    {

                        if (j == obuytype.NumID)
                        {
                            showStr += "[<font color=\"red\">" + anums[j].Trim() + "</font>]";
                        }
                        else
                            showStr += anums[j].Trim();
                    }
                    string str = showStr;
                    if (value > 5)
                        str += "<font color =\"red\">" + "大" + "</font>";
                    else
                        str += "<font color =\"red\">" + "小" + "</font>";
                    if (value % 2 == 0)
                        str += "<font color =\"red\">" + "双" + "</font>";
                    else
                        str += "<font color =\"red\">" + "单" + "</font>";
                    //显示大小单双的遗漏
                    //string[] anames = { "大", "小", "单", "双" };
                    //for (int m = 0; m < anames.Length; m++)
                    //{
                    //    if (sumList[0][m] == 0)
                    //    {
                    //        str += "<font color=\"red\">" + " " + anames[m] + "</font>";
                    //    }
                    //    else
                    //    {
                    //        string cc = sumList[0][m].ToString().Trim();
                    //        if (cc.Length == 1)
                    //            cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                    //        str += "<font color=\"Grey\">" + " " + cc + "</font>";
                    //    }
                    //}
                    //
                    list[i].ShowNums = str;
                    #endregion
                }
            }
        }
        private void GenReportData4(List<PK10_Report> list, PK10_BuyType obuytype, int[][] sumList) //龙虎类型分析
        {
            if (list.Count > 0)
            {
                string[] abase = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    #region 取到指定的位数号码的值
                    string[] anums = list[i].Nums.Split(',');
                    int value1 = int.Parse(anums[obuytype.NumID]);
                    int value2 = int.Parse(anums[9 - obuytype.NumID]);
                    #endregion
                    #region 生成遗漏值
                    //sumList[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存当前遗漏结果{龙，虎，0，0,0,0,0,0,0,0}
                    //sumList[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存最大遗漏结果{龙，虎，0，0,0,0,0,0,0,0}
                    //sumList[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存平均遗漏结果{龙，虎，0，0,0,0,0,0,0,0}
                    //sumList[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存总次数结果  {龙，虎，0，0,0,0,0,0,0,0}
                    if (value1 > value2) //龙
                    {
                        int currentlost = sumList[0][0]; //龙的当前遗漏
                        sumList[3][0] = sumList[3][0] + 1;//龙的总出现次数+1
                        if (sumList[1][0] < currentlost)  //更新龙的最大遗漏值
                            sumList[1][0] = currentlost;
                        sumList[0][0] = 0;//重置龙的当前遗漏为0
                        //
                        sumList[0][1] = sumList[0][1] + 1;//重置虎的当前遗漏+1
                    }
                    else //小
                    {
                        int currentlost = sumList[0][1]; //虎的当前遗漏
                        sumList[3][1] = sumList[3][1] + 1;//虎的总出现次数+1
                        if (sumList[1][1] < currentlost)  //更新虎的最大遗漏值
                            sumList[1][1] = currentlost;
                        sumList[0][1] = 0;//重置虎的当前遗漏为0
                        //
                        sumList[0][0] = sumList[0][0] + 1;//重置龙的当前遗漏+1
                    }
                    #endregion
                    #region 生成显示的格式
                    //显示号码
                    string showStr = "";
                    for (int j = 0; j < 10; j++)
                    {
                        if (j == obuytype.NumID || j == (9 - obuytype.NumID))
                        {
                            showStr += "[<font color=\"red\">" + anums[j].Trim() + "</font>]";
                        }
                        else
                            showStr += anums[j].Trim();
                    }
                    string str = showStr;
                    if (value1 > value2)
                        str += "<font color =\"red\">" + "龙" + "</font>";
                    else
                        str += "<font color =\"red\">" + "虎" + "</font>";
                    #region 显示大小单双的遗漏
                    //string[] anames = { "龙", "虎" };
                    //for (int m = 0; m < anames.Length; m++)
                    //{
                    //    if (sumList[0][m] == 0)
                    //    {
                    //        str += "<font color=\"red\">" + " " + anames[m] + "</font>";
                    //    }
                    //    else
                    //    {
                    //        string cc = sumList[0][m].ToString().Trim();
                    //        if (cc.Length == 1)
                    //            cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                    //        str += "<font color=\"Grey\">" + " " + cc + "</font>";
                    //    }
                    //}
                    #endregion
                    list[i].ShowNums = str;
                    #endregion
                }
            }
        }
        private void GenReportData5(List<PK10_Report> list, PK10_BuyType obuytype, int[][] sumList)//任选号码类型分析
        {
            if (list.Count > 0)
            {
                string[] abase = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    #region 生成需要查找的号码
                    List<string> lnums = new List<string>();
                    string[] anums = list[i].Nums.Split(',');
                    for (int j = 0; j < obuytype.NumsCount; j++)
                    {
                        lnums.Add(anums[j].Trim());
                    }
                    #endregion
                    #region 生成遗漏值
                    for (int j = 1; j <= 10; j++) //循环10个号码
                    {
                        int currentlost = sumList[0][j - 1];
                        if (lnums.Contains(abase[j - 1])) //中出了号码
                        {
                            sumList[3][j - 1] = sumList[3][j - 1] + 1;//总出现次数+1
                                                                      //更新最大遗漏值
                            if (sumList[1][j - 1] < currentlost)
                                sumList[1][j - 1] = currentlost;
                            //

                            //
                            sumList[0][j - 1] = 0; //重置该位置的当前遗漏为0
                        }
                        else
                        {
                            sumList[0][j - 1] = sumList[0][j - 1] + 1;
                        }
                    }
                    #endregion
                    #region 生成显示的格式
                    string str = "";
                    for (int m = 0; m < 10; m++)
                    {
                        if (sumList[0][m] == 0)
                        {
                            str += "<font color=\"red\">" + " " + abase[m].Trim() + "</font>";
                        }
                        else
                        {
                            string cc = sumList[0][m].ToString().Trim();
                            if (cc.Length == 1)
                                cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                            str += "<font color=\"Grey\">" + " " + cc + "</font>";
                        }
                    }
                    list[i].ShowNums = str;
                    #endregion
                }
            }
        }
        private void GenReportData6(List<PK10_Report> list, PK10_BuyType obuytype, int[][] sumList)//冠亚军类型分析
        {
            if (list.Count > 0)
            {
                string[] abase = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10" };
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    #region 取到指定的位数号码的值
                    string[] anums = list[i].Nums.Split(',');
                    int value = int.Parse(anums[0])+int.Parse(anums[1]);
                    #endregion
                    #region 生成遗漏值
                    //sumList[0] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存当前遗漏结果{大，小，单，双,和,0,0,0,0,0}
                    //sumList[1] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存最大遗漏结果{大，小，单，双,和,0,0,0,0,0}
                    //sumList[2] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存平均遗漏结果{大，小，单，双,和,0,0,0,0,0}
                    //sumList[3] = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }; //保存总次数结果  {大，小，单，双,和,0,0,0,0,0}
                    if (value == 11) //11是和，
                    {
                        int currentlost = sumList[0][4]; //和的当前遗漏
                        sumList[3][4] = sumList[3][4] + 1;//和的总出现次数+1
                        if (sumList[1][4] < currentlost)  //更新和的最大遗漏值
                            sumList[1][4] = currentlost;
                        sumList[0][4] = 0;//重置和的当前遗漏为0
                        //
                        sumList[0][0] = sumList[0][0] + 1;//重置大的当前遗漏+1
                        sumList[0][1] = sumList[0][1] + 1;//重置小的当前遗漏+1
                        sumList[0][2] = sumList[0][2] + 1;//重置单的当前遗漏+1
                        sumList[0][3] = sumList[0][3] + 1;//重置双的当前遗漏+1
                    }
                    else 
                    {
                        #region 和值非11时候，区分大小单双
                        sumList[0][4] = sumList[0][4] + 1;//重置和的当前遗漏+1
                        //
                        if (value > 11) //大
                        {
                            int currentlost = sumList[0][0]; //大的当前遗漏
                            sumList[3][0] = sumList[3][0] + 1;//大的总出现次数+1
                            if (sumList[1][0] < currentlost)  //更新大的最大遗漏值
                                sumList[1][0] = currentlost;
                            sumList[0][0] = 0;//重置大的当前遗漏为0
                                              //
                            sumList[0][1] = sumList[0][1] + 1;//重置小的当前遗漏+1
                        }
                        else //小
                        {
                            int currentlost = sumList[0][1]; //小的当前遗漏
                            sumList[3][1] = sumList[3][1] + 1;//小的总出现次数+1
                            if (sumList[1][1] < currentlost)  //更新小的最大遗漏值
                                sumList[1][1] = currentlost;
                            sumList[0][1] = 0;//重置小的当前遗漏为0
                                              //
                            sumList[0][0] = sumList[0][0] + 1;//重置大的当前遗漏+1
                        }
                        if (value % 2 == 0) //双
                        {
                            int currentlost = sumList[0][3]; //双的当前遗漏
                            sumList[3][3] = sumList[3][3] + 1;//双的总出现次数+1
                            if (sumList[1][3] < currentlost)  //更新双的最大遗漏值
                                sumList[1][3] = currentlost;
                            sumList[0][3] = 0;//重置双的当前遗漏为0
                                              //
                            sumList[0][2] = sumList[0][2] + 1;//重置单的当前遗漏+1
                        }
                        else //单
                        {
                            int currentlost = sumList[0][2]; //单的当前遗漏
                            sumList[3][2] = sumList[3][2] + 1;//单的总出现次数+1
                            if (sumList[1][2] < currentlost)  //更新单的最大遗漏值
                                sumList[1][2] = currentlost;
                            sumList[0][2] = 0;//重置单的当前遗漏为0
                                              //
                            sumList[0][3] = sumList[0][3] + 1;//重置双的当前遗漏+1
                        }
                        #endregion
                    }
                    #endregion
                    #region 生成显示的格式
                    //显示号码
                    string showStr = "";
                    for (int j = 0; j < 10; j++)
                    {
                        if (j == 0 || j == 1)
                        {
                            if (j == 0)
                                showStr += "[<font color=\"red\">" + anums[j].Trim()+" " + "</font>";
                            else
                                showStr += "<font color=\"red\">" + anums[j].Trim() + "</font>]";
                        }
                        else
                            showStr += anums[j].Trim();
                    }
                    string str = showStr;
                    if (value == 11)
                        str += "和";
                    else
                    {
                        if (value > 11)
                            str += "<font color =\"red\">" + "大" + "</font>";
                        else
                            str += "<font color =\"red\">" + "小" + "</font>";
                        if (value % 2 == 0)
                            str += "<font color =\"red\">" + "双" + "</font>";
                        else
                            str += "<font color =\"red\">" + "单" + "</font>";
                    }
                    str +=  value.ToString().Trim();
                    //显示大小单双的遗漏
                    //string[] anames = { "大", "小", "单", "双","和" };
                    //for (int m = 0; m < anames.Length; m++)
                    //{
                    //    if (sumList[0][m] == 0)
                    //    {
                    //        str += "<font color=\"red\">" + " " + anames[m] + "</font>";
                    //    }
                    //    else
                    //    {
                    //        string cc = sumList[0][m].ToString().Trim();
                    //        if (cc.Length == 1)
                    //            cc = "<font color=\"white\">" + "0" + "</font>" + cc;
                    //        str += "<font color=\"Grey\">" + " " + cc + "</font>";
                    //    }
                    //}
                    //
                    list[i].ShowNums = str;
                    #endregion
                }
            }
        }
        #endregion
        public PK10_WinReport GetWinReport(DateTime begindate,DateTime enddate,bool hasRobot,bool hasSpier)//读取某一时间范围的收支总情况
        {
            PK10_WinReport oData = new PK10_WinReport();
            //
            try
            {
                string cSQL = "Select ";
                cSQL += "COUNT(*) as PayCount, sum(cast(A.PayMoney as bigint)) as PayMoney, ";
                cSQL += "sum(case when A.winmoney > 1 then 1 else 0 end) as WinCount, sum(cast(A.WinMoney as bigint)) as WinMoney,";
                cSQL += "sum(A.CaseFlag) as CaseCount, sum(cast(A.CaseMoney as bigint)) as CaseMoney,";
                cSQL += "sum(cast(A.Charges as bigint)) as Charges";
                cSQL += " from tb_PK10_Buy A Left join tb_User B on A.uID=B.ID where A.BuyTime>='" + begindate.ToShortDateString() + "' and A.BuyTime<'" + enddate.AddDays(1).ToShortDateString() + "'";
                if (!hasRobot)
                    cSQL += " And A.isRobot=0";
                if (!hasSpier)
                    cSQL += " And B.IsSpier=0";
                oData = ModelHelper.GetModelBySql<PK10_WinReport>(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("GetWinReport Error:" + ex.Message);
            }
            //
            return oData;
        }
        #endregion
        //
        #region 其他
        public string GetGoldName(bool isTest)
        {
            if (isTest)
                return "PK币";
            else
                return BCW.Common.ub.Get("SiteBz");
        }
        public long GetUserGold(int uID, bool isTest)
        {
            long gold = 0;
            //
            try
            {
                string cSQL = "";
                if (!isTest)
                    cSQL = "Select  iGold from tb_User Where ID=" + uID.ToString().Trim();
                else
                    cSQL = "Select  iGold from tb_PK10_TestUser Where ID=" + uID.ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null && dt.Rows.Count > 0)
                    long.TryParse(dt.Rows[0]["iGold"].ToString(), out gold);
            }
            catch (Exception ex)
            {
                throw new Exception("GetUserGold Error:" + ex.Message);
            }
            //
            return gold;
        }
        public bool CreateTestUser(int uID, int gold)
        {
            try
            {
                PK10_TestUser oUser = ModelHelper.GetModelBySql<PK10_TestUser>("Select * From tb_PK10_TestUser Where ID=" + uID.ToString().Trim());
                if (oUser == null)
                {
                    DataTable dtUser = MySqlHelper.GetTable("Select id,usname From tb_User Where ID=" + uID.ToString().Trim());
                    if (dtUser == null || dtUser.Rows.Count <= 0)
                        return false;
                    string usName = dtUser.Rows[0]["UsName"].ToString().Trim();
                    int rows = MySqlHelper.ExecuteSql("Insert into tb_PK10_TestUser(ID,UsName,iGold) Values(" + uID.ToString().Trim() + ",'" + usName + "'" + "," + gold.ToString().Trim() + ")");
                    if (rows <= 0)
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("CreateTestUser Error:" + ex.Message);
            }
            return true;
        }
        public string GetSettings(int uID)
        {
            string settings= "";
            //
            try
            {
                string cSQL = "Select  Settings from tb_PK10_TestUser Where ID=" + uID.ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null && dt.Rows.Count > 0)
                    settings = dt.Rows[0]["Settings"].ToString().Trim();
            }
            catch (Exception ex)
            {
                throw new Exception("GetSettings Error:" + ex.Message);
            }
            //
            return settings;
        }
        public void SaveSettings(int uID,string settings)
        {
            //
            try
            {
                string cSQL = "Select  ID from tb_PK10_TestUser Where ID=" + uID.ToString().Trim();
                DataTable dt = MySqlHelper.GetTable(cSQL);
                if (dt != null && dt.Rows.Count > 0)
                {
                    cSQL = "Update tb_PK10_TestUser Set settings ='" + settings + "' where ID=" + uID.ToString().Trim(); 
                }
                else
                {
                    DataTable dtUser = MySqlHelper.GetTable("Select id,usname From tb_User Where ID=" + uID.ToString().Trim());
                    if (dtUser == null || dtUser.Rows.Count <= 0)
                        return ;
                    string usName = dtUser.Rows[0]["UsName"].ToString().Trim();
                    cSQL = "Insert into tb_PK10_TestUser(ID,UsName,iGold,Settings) Values(";
                    cSQL += uID.ToString().Trim();
                    cSQL += ",'" + usName + "'";
                    cSQL += ",0";
                    cSQL += ",'" + settings + "'";
                    cSQL+=")";
                }
                MySqlHelper.ExecuteSql(cSQL);
            }
            catch (Exception ex)
            {
                throw new Exception("SaveSettings Error:" + ex.Message);
            }
            //
        }
        #endregion
        //
        #region 内线消息
        private int GetForumSet(string ForumSet, int iType)//得到个性设置
        {
            int v = 0;
            try
            {
                string[] forumset = ForumSet.Split(",".ToCharArray());

                string[] fs = forumset[iType].ToString().Split("|".ToCharArray());
                v = Convert.ToInt32(fs[1]);
            }
            catch(Exception ex)
            {
                throw new Exception("GetForumSet Err:"+ex.Message+",iType="+iType+"ForumSet=" + ForumSet);
            }
            return v;
        }
        public string AddSysMsg(SqlConnection conn, SqlTransaction trans, int Types, int uid, string Content) //添加内线消息
        {
            string cFlag = "";
            try
            {
                DataTable dt = MySqlHelper.GetTable(conn, trans, "select * from tb_User where id=" + uid.ToString().Trim());
                if (dt != null && dt.Rows.Count > 0)
                {
                    string ForumSet = dt.Rows[0]["ForumSet"].ToString();//个性设置
                    string uname = dt.Rows[0]["UsName"].ToString();
                    string ip = dt.Rows[0]["EndIP"].ToString();
                    string SmsEmail = dt.Rows[0]["SmsEmail"].ToString().Trim();
                    bool flag = true;
                    if (ForumSet != "")
                    {
                        if (Types == 0)//系统消息
                        {
                            int xTotal = GetForumSet(ForumSet, 15);
                            if (xTotal == 1)
                                flag = false;
                        }
                        else if (Types == 1)//游戏PK结果消息
                        {
                            int gTotal = GetForumSet(ForumSet, 16);
                            if (gTotal == 1)
                                flag = false;
                        }
                        else if (Types == 2)//推荐邀请提醒消息
                        {
                            int tTotal = GetForumSet(ForumSet, 14);
                            if (tTotal == 1)
                                flag = false;
                        }
                        else if (Types == 3)//空间留言提醒消息
                        {
                            int bTotal = GetForumSet(ForumSet, 20);
                            if (bTotal == 1)
                                flag = false;
                        }
                        else if (Types == 4)//闲聊私聊提醒消息/竞猜走地成功失败提醒
                        {
                            int sTotal = GetForumSet(ForumSet, 33);
                            if (sTotal == 1)
                                flag = false;
                        }
                        if (flag)
                            cFlag = CreateGuestRecord(conn, trans, Types, uid, uname, ip, ForumSet, Content, SmsEmail);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("AddSysMsg Err:"+ex.Message);
            }
            return cFlag;
        }
        private string CreateGuestRecord(SqlConnection conn, SqlTransaction trans, int Types, int uid, string uname, string ip, string ForumSet, string content,string SmsEmail)
        {
            #region 生成数据
            string cSQL = "Insert into tb_Guest(Types,FromId,FromName,ToId,ToName,Content,IsSeen,IsKeep,FDel,TDel,TransId,AddUsIP,AddTime) Values(";
            cSQL += Types.ToString().Trim();
            cSQL += "," + "0";
            cSQL += "," + "'系统消息'";
            cSQL += "," + uid.ToString().Trim();
            cSQL += "," + "'" + uname.Trim() + "'";
            cSQL += "," + "'" + content.Trim() + "'";
            cSQL += "," + "0";
            cSQL += "," + "0";
            cSQL += "," + "0";
            cSQL += "," + "0";
            cSQL += "," + "0";
            cSQL += "," + "'" + ip + "'";
            cSQL += "," + "'" + DateTime.Now.ToString().Trim() + "'";
            cSQL += ")";
            int row = MySqlHelper.InsertAndGetID(cSQL, conn, trans);
            if (row <= 0)
                return "建立系统消息失败！";
            #endregion
            if (Types != 4)
            {
                #region 发送邮件通知
                try
                {
                    int IsSms2 = GetForumSet(ForumSet, 28);
                    if (IsSms2 == 1 && SmsEmail != "" && !Utils.getPageUrl().Contains("getover.aspx"))
                    {
                        //设定参数
                        string xmlPath = "/Controls/email.xml";
                        string EmailFrom = ub.GetSub("EmailFrom", xmlPath);
                        string EmailFromUser = ub.GetSub("EmailFromUser", xmlPath);
                        string EmailFromPwd = ub.GetSub("EmailFromPwd", xmlPath);
                        string EmailFromHost = ub.GetSub("EmailFromHost", xmlPath);
                        string EmailFromPort = ub.GetSub("EmailFromPort", xmlPath);

                        // 发件人地址
                        string From = EmailFrom;
                        // 收件人地址
                        string To = SmsEmail;
                        // 邮件主题
                        string Subject = "来自-系统内线";
                        // 邮件正文
                        string Body = "c";
                        if (!SmsEmail.Contains("@139.com"))
                        {
                            Body = "系统内线<BR>" + content + "";
                        }
                        // 邮件主机地址
                        string Host = EmailFromHost;
                        // 邮件主机端口
                        int Port = Utils.ParseInt(EmailFromPort);
                        // 登录帐号
                        string UserName = EmailFromUser;
                        // 登录密码
                        string Password = EmailFromPwd;
                        //附件地址
                        string FilePath = "";
                        if (EmailFrom != "")
                            new SendMail().Send(From, To, Subject, Body, Host, Port, UserName, Password, FilePath);
                    }
                }
                catch { };
                #endregion
            }
            return "";
        }
        #endregion
        //
    }

}
