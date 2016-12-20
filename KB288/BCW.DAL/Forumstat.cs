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
    /// 数据访问类Forumstat。
    /// </summary>
    public class Forumstat
    {
        public Forumstat()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forumstat");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumstat");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在今天记录
        /// </summary>
        public bool ExistsToDay(int UsID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumstat");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and AddTime=@AddTime ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now.ToShortDateString();

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Forumstat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumstat(");
            strSql.Append("ForumID,UsID,UsName,tTotal,rTotal,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@ForumID,@UsID,@UsName,@tTotal,@rTotal,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@tTotal", SqlDbType.Int,4),
                    new SqlParameter("@rTotal", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = model.ForumID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.tTotal;
            parameters[4].Value = model.rTotal;
            parameters[5].Value = model.AddTime;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int Types, int UsID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumstat set ");
            if (Types == 1)
                strSql.Append("tTotal=tTotal+1");
            else if (Types == 2)
                strSql.Append("rTotal=rTotal+1");
            else if (Types == 3)
                strSql.Append("gTotal=gTotal+1");
            else if (Types == 4)
                strSql.Append("jTotal=jTotal+1");

            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and AddTime=@AddTime");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now.ToShortDateString();

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumstat set ");
            if (Types == 1)
                strSql.Append("tTotal=tTotal+1");
            else if (Types == 2)
                strSql.Append("rTotal=rTotal+1");
            else if (Types == 3)
                strSql.Append("gTotal=gTotal+1");
            else if (Types == 4)
                strSql.Append("jTotal=jTotal+1");

            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and AddTime=@AddTime");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = AddTime.ToShortDateString();

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int Types, int UsID, int ForumID, DateTime AddTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumstat set ");
            if (Types == 1)
                strSql.Append("tTotal=tTotal-1");
            else if (Types == 2)
                strSql.Append("rTotal=rTotal-1");
            else if (Types == 3)
                strSql.Append("gTotal=gTotal-1");
            else if (Types == 4)
                strSql.Append("jTotal=jTotal-1");

            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and AddTime=@AddTime");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = AddTime.ToShortDateString();

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Forumstat model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumstat set ");
            strSql.Append("ForumID=@ForumID,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("tTotal=@tTotal,");
            strSql.Append("rTotal=@rTotal,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@tTotal", SqlDbType.Int,4),
                    new SqlParameter("@rTotal", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ForumID;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.tTotal;
            parameters[5].Value = model.rTotal;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量转移统计
        /// </summary>
        public void UpdateForumID(int ForumID, int NewForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumstat set ");
            strSql.Append("ForumID=@NewForumID ");
            strSql.Append(" where ForumID=@ForumID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ForumID", SqlDbType.Int,4),
                    new SqlParameter("@NewForumID", SqlDbType.Int,4)};
            parameters[0].Value = ForumID;
            parameters[1].Value = NewForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumstat ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumstat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 删除排行残余数据（数据为0的记录）
        /// </summary>
        public void Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumstat ");
            strSql.Append(" where tTotal=0 and rTotal=0 and gTotal=0 and jTotal=0 ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到某ID某选项数量
        /// </summary>
        public int GetIDCount(int UsID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ");
            if (Types == 1)
                strSql.Append("sum(tTotal)");
            else if (Types == 2)
                strSql.Append("sum(rTotal)");
            else if (Types == 3)
                strSql.Append("sum(gTotal)");
            else if (Types == 4)
                strSql.Append("sum(jTotal)");

            strSql.Append(" from tb_Forumstat ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到某选项今天数量
        /// </summary>
        public int GetCount(int UsID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ");
            if (Types == 1)
                strSql.Append("sum(tTotal)");
            else if (Types == 2)
                strSql.Append("sum(rTotal)");
            else if (Types == 3)
                strSql.Append("sum(gTotal)");
            else if (Types == 4)
                strSql.Append("sum(jTotal)");

            strSql.Append(" from tb_Forumstat ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and AddTime=@AddTime ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.SmallDateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now.ToShortDateString();

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 得到某ID本月发帖回帖数量(1发贴/2回帖)
        /// </summary>
        public int GetMonthCount(int UsID, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ");
            if (Types == 1)
                strSql.Append("sum(tTotal)");
            else if (Types == 2)
                strSql.Append("sum(rTotal)");

            strSql.Append(" from tb_Forumstat ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region 得到某论坛某选项数量 GetCount
        /// <summary>
        /// 得到某论坛某选项数量(fg 0值统计全部，1值统计主题论坛，2值统计圈子论坛)
        /// 黄国军20160124
        /// </summary>
        public int GetCount(int fg, int UsID, int ForumID, int Types, int dType)
        {

            StringBuilder strSql = new StringBuilder();
            //Types 固定类型1帖子数2回帖数
            strSql.Append("select  top 1 ");
            if (Types == 1)
                strSql.Append("sum(tTotal)");   //帖子总数
            else if (Types == 2)
                strSql.Append("sum(rTotal)");   //回帖总数
            //搜索表
            strSql.Append(" from tb_Forumstat where ");

            //指定用户
            if (UsID > 0)
                strSql.Append(" UsID=" + UsID + " ");
            else
                strSql.Append(" UsID>0 ");

            //指定论坛
            if (ForumID > 0)
                strSql.Append(" and ForumID=" + ForumID + " ");

            //时间分类
            if (dType > 0)
                strSql.Append(" and ");

            if (dType == 1)//今天
                strSql.Append(" AddTime='" + DateTime.Now.ToShortDateString() + "' ");
            if (dType == 2)//昨天
                strSql.Append(" AddTime='" + DateTime.Now.AddDays(-1).ToShortDateString() + "' ");

            if (dType == 3)//本周
            {
                //本周计算
                string M_Str_mindate = string.Empty;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                strSql.Append(" AddTime>='" + M_Str_mindate + "' ");
            }

            if (dType == 6)//上周 黄国军 20160120 
            {
                //上周计算
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
                string M_Str_mindate = string.Empty;
                string M_Str_Maxdate = string.Empty;

                switch (ForDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = ForDate.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = ForDate.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = ForDate.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = ForDate.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = ForDate.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = ForDate.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = ForDate.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                M_Str_Maxdate = DateTime.Parse(M_Str_mindate).AddDays(5).ToShortDateString();
                strSql.Append(" AddTime between '" + M_Str_mindate + " 00:00:00' AND '" + M_Str_Maxdate + " 23:59:59' ");
            }

            if (dType == 4)//本月
                strSql.Append(" Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "");

            if (dType == 5)//上月
            {
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
                int ForYear = ForDate.Year;
                int ForMonth = ForDate.Month;
                strSql.Append(" Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "");
            }

            //fg 0值统计全部，1值统计主题论坛，2值统计圈子论坛
            if (fg == 1)
                strSql.Append(" and (Select GroupId FROM tb_Forum where ID=ForumID)=0");
            else if (fg == 2)
                strSql.Append(" and (Select GroupId FROM tb_Forum where ID=ForumID)<>0");

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        #endregion

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumstat GetForumstat(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,ForumID,UsID,UsName,tTotal,rTotal,gTotal,jTotal,AddTime from tb_Forumstat ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forumstat model = new BCW.Model.Forumstat();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.ForumID = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.tTotal = reader.GetInt32(4);
                    model.rTotal = reader.GetInt32(5);
                    model.gTotal = reader.GetInt32(6);
                    model.jTotal = reader.GetInt32(7);
                    model.AddTime = reader.GetDateTime(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Forumstat ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        #region 论坛排行分页记录 增加上周排行 GetForumstats
        /// <summary>
        /// 论坛排行分页记录 增加上周排行 黄国军20160124
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            IList<BCW.Model.Forumstat> listForumstat = new List<BCW.Model.Forumstat>();
            string strWhe = string.Empty;
            if (strWhere != "" || showtype > 1)
                strWhe += " where ";

            if (strWhere != "")
                strWhe += strWhere;

            if (strWhere != "" && showtype > 1)
                strWhe += " and ";

            if (showtype == 2)  //本周
            {
                #region 本周
                string M_Str_mindate = string.Empty;
                switch (DateTime.Now.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = DateTime.Now.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = DateTime.Now.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = DateTime.Now.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = DateTime.Now.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = DateTime.Now.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = DateTime.Now.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = DateTime.Now.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                strWhe += " AddTime>='" + M_Str_mindate + "'";
                #endregion
            }
            else if (showtype == 3) //本月
            {
                #region 本月
                strWhe += " Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + "";
                #endregion
            }
            else if (showtype == 4) //上月
            {
                #region 上月
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddMonths(-1).ToShortDateString());
                int ForYear = ForDate.Year;
                int ForMonth = ForDate.Month;
                strWhe += " Year(AddTime) = " + (ForYear) + " AND Month(AddTime) = " + (ForMonth) + "";
                #endregion
            }
            else if (showtype == 5) //上周
            {
                #region 上周
                DateTime ForDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToShortDateString());
                string M_Str_mindate = string.Empty;
                string M_Str_Maxdate = string.Empty;

                switch (ForDate.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        M_Str_mindate = ForDate.AddDays(0).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Tuesday:
                        M_Str_mindate = ForDate.AddDays(-1).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Wednesday:
                        M_Str_mindate = ForDate.AddDays(-2).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Thursday:
                        M_Str_mindate = ForDate.AddDays(-3).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Friday:
                        M_Str_mindate = ForDate.AddDays(-4).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Saturday:
                        M_Str_mindate = ForDate.AddDays(-5).ToShortDateString() + "";
                        break;
                    case DayOfWeek.Sunday:
                        M_Str_mindate = ForDate.AddDays(-6).ToShortDateString() + "";
                        break;
                }
                M_Str_Maxdate = DateTime.Parse(M_Str_mindate).AddDays(5).ToShortDateString();
                strWhe += " AddTime between '" + M_Str_mindate + " 00:00:00' AND '" + M_Str_Maxdate + " 23:59:59'";
                #endregion
            }

            #region 计算记录数
            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Forumstat " + strWhe + "";
            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 100)
                p_recordCount = 100;
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listForumstat;
            }
            #endregion

            #region 取出相关记录数
            // 取出相关记录
            string queryString = "SELECT TOP 100 UsID," + strOrder + " FROM tb_Forumstat " + strWhe + " GROUP BY UsID ORDER BY " + strOrder + " DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Forumstat objForumstat = new BCW.Model.Forumstat();
                        objForumstat.UsID = reader.GetInt32(0);
                        //objForumstat.UsName = reader.GetString(1);
                        objForumstat.tTotal = reader.GetInt32(1);

                        listForumstat.Add(objForumstat);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }
            #endregion

            return listForumstat;
        }
        #endregion

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Forumstat</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumstat> listForumstats = new List<BCW.Model.Forumstat>();
            string sTable = "tb_Forumstat";
            string sPkey = "id";
            string sField = "ID,ForumID,UsID,UsName,tTotal,rTotal,gTotal,jTotal,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
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
                    return listForumstats;
                }
                while (reader.Read())
                {
                    BCW.Model.Forumstat objForumstat = new BCW.Model.Forumstat();
                    objForumstat.ID = reader.GetInt32(0);
                    objForumstat.ForumID = reader.GetInt32(1);
                    objForumstat.UsID = reader.GetInt32(2);
                    objForumstat.UsName = reader.GetString(3);
                    objForumstat.tTotal = reader.GetInt32(4);
                    objForumstat.rTotal = reader.GetInt32(5);
                    objForumstat.gTotal = reader.GetInt32(6);
                    objForumstat.jTotal = reader.GetInt32(7);
                    objForumstat.AddTime = reader.GetDateTime(11);
                    listForumstats.Add(objForumstat);
                }
            }
            return listForumstats;
        }

        /// <summary>
        /// 论坛排行分页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Forumstat> GetForumstats(int p_pageIndex, int p_pageSize, int ptype, string sWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumstat> listForumstat = new List<BCW.Model.Forumstat>();

            // 计算记录数
            string strWhere = "";
            if (ptype == 0)
                strWhere = "where (select GroupId from tb_Forum where ID=b.ForumID)=0";
            else
                strWhere = "where (select GroupId from tb_Forum where ID=b.ForumID)<>0";


            if (sWhere != "")
            {
                strWhere += " and " + sWhere + "";
            }

            string countString = "SELECT COUNT(DISTINCT b.ForumID) FROM tb_Forumstat b " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listForumstat;
            }
            // 取出相关记录


            string queryString = "select b.ForumID,sum(b.tTotal+b.rTotal) as Total from tb_forumstat b " + strWhere + "  Group by b.ForumID ORDER BY Total Desc";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Forumstat objForumstat = new BCW.Model.Forumstat();
                        objForumstat.ForumID = reader.GetInt32(0);
                        objForumstat.tTotal = reader.GetInt32(1);

                        listForumstat.Add(objForumstat);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listForumstat;
        }

        #endregion  成员方法
    }
}
