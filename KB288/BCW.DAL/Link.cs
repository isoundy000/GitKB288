using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Link。
    /// </summary>
    public class Link
    {
        public Link()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Link");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Link");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string LinkUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Link");
            strSql.Append(" where LinkUrl=@LinkUrl ");
            SqlParameter[] parameters = {
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,200)};
            parameters[0].Value = LinkUrl;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.Link model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Link(");
            strSql.Append("ID,LinkName,LinkNamt,LinkUrl,LinkNotes,KeyWord,LinkIn,LinkOut,ReStats,ReLastIP,LinkTime,LinkTime2,AddTime,Leibie,LinkRd,Hidden,InOnly,todayIn,yesterdayIn,beforeIn,todayOut,yesterdayOut,beforeOut,LinkIPIn,LinkIPOut,IPtodayIn,IPyesterdayIn,IPbeforeIn,IPtodayOut,IPyesterdayOut,IPbeforeOut)");
            strSql.Append(" values (");
            strSql.Append("@ID,@LinkName,@LinkNamt,@LinkUrl,@LinkNotes,@KeyWord,@LinkIn,@LinkOut,@ReStats,@ReLastIP,@LinkTime,@LinkTime2,@AddTime,@Leibie,@LinkRd,@Hidden,@InOnly,@todayIn,@yesterdayIn,@beforeIn,@todayOut,@yesterdayOut,@beforeOut,@LinkIPIn,@LinkIPOut,@IPtodayIn,@IPyesterdayIn,@IPbeforeIn,@IPtodayOut,@IPyesterdayOut,@IPbeforeOut)");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LinkName", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkNamt", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkNotes", SqlDbType.NVarChar,500),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkIn", SqlDbType.Int,4),
					new SqlParameter("@LinkOut", SqlDbType.Int,4),
					new SqlParameter("@ReStats", SqlDbType.NVarChar,50),
					new SqlParameter("@ReLastIP", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkTime", SqlDbType.DateTime),
					new SqlParameter("@LinkTime2", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Leibie", SqlDbType.Int,4),
					new SqlParameter("@LinkRd", SqlDbType.Int,4),
					new SqlParameter("@Hidden", SqlDbType.Int,4),
					new SqlParameter("@InOnly", SqlDbType.Int,4),
					new SqlParameter("@todayIn", SqlDbType.Int,4),
					new SqlParameter("@yesterdayIn", SqlDbType.Int,4),
					new SqlParameter("@beforeIn", SqlDbType.Int,4),
					new SqlParameter("@todayOut", SqlDbType.Int,4),
					new SqlParameter("@yesterdayOut", SqlDbType.Int,4),
					new SqlParameter("@beforeOut", SqlDbType.Int,4),
					new SqlParameter("@LinkIPIn", SqlDbType.Int,4),
					new SqlParameter("@LinkIPOut", SqlDbType.Int,4),
					new SqlParameter("@IPtodayIn", SqlDbType.Int,4),
					new SqlParameter("@IPyesterdayIn", SqlDbType.Int,4),
					new SqlParameter("@IPbeforeIn", SqlDbType.Int,4),
					new SqlParameter("@IPtodayOut", SqlDbType.Int,4),
					new SqlParameter("@IPyesterdayOut", SqlDbType.Int,4),
					new SqlParameter("@IPbeforeOut", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.LinkName;
            parameters[2].Value = model.LinkNamt;
            parameters[3].Value = model.LinkUrl;
            parameters[4].Value = model.LinkNotes;
            parameters[5].Value = model.KeyWord;
            parameters[6].Value = model.LinkIn;
            parameters[7].Value = model.LinkOut;
            parameters[8].Value = model.ReStats;
            parameters[9].Value = model.ReLastIP;
            parameters[10].Value = model.LinkTime;
            parameters[11].Value = model.LinkTime2;
            parameters[12].Value = model.AddTime;
            parameters[13].Value = model.Leibie;
            parameters[14].Value = model.LinkRd;
            parameters[15].Value = model.Hidden;
            parameters[16].Value = 0;
            parameters[17].Value = 0;
            parameters[18].Value = 0;
            parameters[19].Value = 0;
            parameters[20].Value = 0;
            parameters[21].Value = 0;
            parameters[22].Value = 0;
            parameters[23].Value = 0;
            parameters[24].Value = 0;
            parameters[25].Value = 0;
            parameters[26].Value = 0;
            parameters[27].Value = 0;
            parameters[28].Value = 0;
            parameters[29].Value = 0;
            parameters[30].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Link model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Link set ");
            strSql.Append("LinkName=@LinkName,");
            strSql.Append("LinkNamt=@LinkNamt,");
            strSql.Append("LinkUrl=@LinkUrl,");
            strSql.Append("LinkNotes=@LinkNotes,");
            strSql.Append("KeyWord=@KeyWord,");
            strSql.Append("LinkIn=@LinkIn,");
            strSql.Append("LinkOut=@LinkOut,");
            strSql.Append("LinkIPIn=@LinkIPIn,");
            strSql.Append("LinkIPOut=@LinkIPOut,");
            strSql.Append("Leibie=@Leibie,");
            strSql.Append("LinkRd=@LinkRd,");
            strSql.Append("Hidden=@Hidden");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LinkName", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkNamt", SqlDbType.NVarChar,50),
					new SqlParameter("@LinkUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkNotes", SqlDbType.NVarChar,500),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkIn", SqlDbType.Int,4),
					new SqlParameter("@LinkOut", SqlDbType.Int,4),
					new SqlParameter("@LinkIPIn", SqlDbType.Int,4),
					new SqlParameter("@LinkIPOut", SqlDbType.Int,4),
                    new SqlParameter("@Leibie", SqlDbType.Int,4),
                    new SqlParameter("@LinkRd", SqlDbType.Int,4),
                    new SqlParameter("@Hidden", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.LinkName;
            parameters[2].Value = model.LinkNamt;
            parameters[3].Value = model.LinkUrl;
            parameters[4].Value = model.LinkNotes;
            parameters[5].Value = model.KeyWord;
            parameters[6].Value = model.LinkIn;
            parameters[7].Value = model.LinkOut;
            parameters[8].Value = model.LinkIPIn;
            parameters[9].Value = model.LinkIPOut;
            parameters[10].Value = model.Leibie;
            parameters[11].Value = model.LinkRd;
            parameters[12].Value = model.Hidden;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Link set ");
            strSql.Append("ReStats=@ReStats, ");
            strSql.Append("ReLastIP=@ReLastIP ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReStats", SqlDbType.NVarChar,50),
            		new SqlParameter("@ReLastIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = ReStats;
            parameters[2].Value = ReLastIP;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新链入
        /// </summary>
        public void UpdateLinkIn(int ID)
        {
            //每天更新
            if (DT.TwoDateDiff(DateTime.Now, GetLinkTime()) >= 1)
            {
                System.Web.HttpContext.Current.Application.Lock();
                SqlHelper.ExecuteSql("update tb_link set beforeIn = yesterdayIn, yesterdayIn = todayIn,todayIn = 0,IPbeforeIn = IPyesterdayIn,IPyesterdayIn = IPtodayIn,IPtodayIn = 0");
                SqlHelper.ExecuteSql("delete from tb_LinkIp where Types=0");
                System.Web.HttpContext.Current.Application.UnLock();
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Link set ");
            strSql.Append("LinkTime=@LinkTime ");
            strSql.Append(",LinkIn=LinkIn+1 ");
            strSql.Append(",todayIn=todayIn+1 ");
            if (!new BCW.DAL.LinkIp().Exists(ID, Utils.GetUsIP(), 0))
            {
                strSql.Append(",LinkIPIn=LinkIPIn+1 ");
                strSql.Append(",IPtodayIn=IPtodayIn+1 ");
                //写入IP
                BCW.Model.LinkIp model = new BCW.Model.LinkIp();
                model.Types = 0;
                model.AddUsIP = Utils.GetUsIP();
                model.AddUsUA = Utils.GetUA();
                model.AddUsPage = Utils.GetBfUrl();
                model.LinkId = ID;
                model.AddTime = DateTime.Now;
                new BCW.DAL.LinkIp().Add(model);
            }
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LinkTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新链出
        /// </summary>
        public void UpdateLinkOut(int ID)
        {
            //每天更新
            if (DT.TwoDateDiff(DateTime.Now, GetLinkTime2()) >= 1)
            {
                System.Web.HttpContext.Current.Application.Lock();
                SqlHelper.ExecuteSql("update tb_link set beforeOut = yesterdayOut,yesterdayOut = todayOut,todayOut = 0,IPbeforeOut = IPyesterdayOut,IPyesterdayOut = IPtodayOut,IPtodayOut = 0");
                SqlHelper.ExecuteSql("delete from tb_LinkIp where Types=1");
                System.Web.HttpContext.Current.Application.UnLock();
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Link set ");
            strSql.Append("LinkTime2=@LinkTime2 ");
            strSql.Append(",LinkOut=LinkOut+1 ");
            strSql.Append(",todayOut=todayOut+1 ");
            if (!new BCW.DAL.LinkIp().Exists(ID, Utils.GetUsIP(), 1))
            {
                strSql.Append(",LinkIPOut=LinkIPOut+1 ");
                strSql.Append(",IPtodayOut=IPtodayOut+1 ");
                //写入IP
                BCW.Model.LinkIp model = new BCW.Model.LinkIp();
                model.Types = 1;
                model.AddUsIP = Utils.GetUsIP();
                model.AddUsUA = Utils.GetUA();
                model.AddUsPage = Utils.GetBfUrl();
                model.LinkId = ID;
                model.AddTime = DateTime.Now;
                new BCW.DAL.LinkIp().Add(model);
            }
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LinkTime2", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新审核
        /// </summary>
        public void UpdateHidden(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Link set ");
            strSql.Append("Hidden=1 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Link ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Link GetLink(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,LinkName,LinkNamt,LinkUrl,LinkNotes,KeyWord,LinkIn,LinkOut,ReStats,ReLastIP,LinkTime,LinkTime2,AddTime,Leibie,LinkRd,Hidden,InOnly,todayIn,yesterdayIn,beforeIn,todayOut,yesterdayOut,beforeOut,LinkIPIn,LinkIPOut,IPtodayIn,IPyesterdayIn,IPbeforeIn,IPtodayOut,IPyesterdayOut,IPbeforeOut from tb_Link ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Link model = new BCW.Model.Link();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.LinkName = reader.GetString(1);
                    model.LinkNamt = reader.GetString(2);
                    model.LinkUrl = reader.GetString(3);
                    model.LinkNotes = reader.GetString(4);
                    model.KeyWord = reader.GetString(5);
                    model.LinkIn = reader.GetInt32(6);
                    model.LinkOut = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        model.ReStats = reader.GetString(8);
                    if (!reader.IsDBNull(9))
                        model.ReLastIP = reader.GetString(9);

                    model.LinkTime = reader.GetDateTime(10);
                    if (!reader.IsDBNull(11))
                        model.LinkTime2 = reader.GetDateTime(11);
                    model.AddTime = reader.GetDateTime(12);
                    model.Leibie = reader.GetInt32(13);
                    model.LinkRd = reader.GetInt32(14);
                    model.Hidden = reader.GetInt32(15);
                    model.InOnly = reader.GetInt32(16);
                    model.todayIn = reader.GetInt32(17);
                    model.yesterdayIn = reader.GetInt32(18);
                    model.beforeIn = reader.GetInt32(19);
                    model.todayOut = reader.GetInt32(20);
                    model.yesterdayOut = reader.GetInt32(21);
                    model.beforeOut = reader.GetInt32(22);
                    model.LinkIPIn = reader.GetInt32(23);
                    model.LinkIPOut = reader.GetInt32(24);
                    model.IPtodayIn = reader.GetInt32(25);
                    model.IPyesterdayIn = reader.GetInt32(26);
                    model.IPbeforeIn = reader.GetInt32(27);
                    model.IPtodayOut = reader.GetInt32(28);
                    model.IPyesterdayOut = reader.GetInt32(29);
                    model.IPbeforeOut = reader.GetInt32(30);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到友链全称
        /// </summary>
        public string GetLinkName(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LinkName from tb_Link ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到友链地址
        /// </summary>
        public string GetLinkUrl(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LinkUrl from tb_Link ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到最后的链入时间
        /// </summary>
        public DateTime GetLinkTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 max([LinkTime]) from tb_Link ");

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDateTime(0);
                    else
                        return DateTime.Now;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 得到最后的链出时间
        /// </summary>
        public DateTime GetLinkTime2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 max([LinkTime2]) from tb_Link ");

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDateTime(0);
                    else
                        return DateTime.Now;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int TopNum, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + TopNum + " ID,LinkName,LinkNamt");
            strSql.Append(" FROM tb_Link ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Link ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Link</returns>
        public IList<BCW.Model.Link> GetLinks(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Link> listLinks = new List<BCW.Model.Link>();
            string sTable = "tb_Link";
            string sPkey = "id";
            string sField = "ID,LinkName,LinkNotes";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listLinks;
                }
                while (reader.Read())
                {
                    BCW.Model.Link objLink = new BCW.Model.Link();
                    objLink.ID = reader.GetInt32(0);
                    objLink.LinkName = reader.GetString(1);
                    objLink.LinkNotes = reader.GetString(2);
                    listLinks.Add(objLink);
                }
            }
            return listLinks;
        }

        #endregion  成员方法
    }
}

