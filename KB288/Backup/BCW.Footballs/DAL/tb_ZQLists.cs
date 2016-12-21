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
    /// 数据访问类tb_ZQLists。
    /// </summary>
    public class tb_ZQLists
    {
        public tb_ZQLists()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZQLists");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该球队比赛记录
        /// </summary>
        public bool Exists_ft_bianhao(int ft_bianhao)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZQLists");
            strSql.Append(" where ft_bianhao=@ft_bianhao ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ft_bianhao", SqlDbType.Int,4)};
            parameters[0].Value = @ft_bianhao;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_ZQLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ZQLists(");
            strSql.Append("ft_bianhao,ft_didian,ft_teamStyle,ft_team1,ft_team2,ft_result,ft_state,ft_time,ft_caipan,ft_news,ft_otherNews,ft_hit,ft_glod,ft_team1Explain,ft_team2Explain,isDone,ft_addTime,ft_overTime,ft_state1,ft_state2,Identification,ft_beiyong)");
            strSql.Append(" values (");
            strSql.Append("@ft_bianhao,@ft_didian,@ft_teamStyle,@ft_team1,@ft_team2,@ft_result,@ft_state,@ft_time,@ft_caipan,@ft_news,@ft_otherNews,@ft_hit,@ft_glod,@ft_team1Explain,@ft_team2Explain,@isDone,@ft_addTime,@ft_overTime,@ft_state1,@ft_state2,@Identification,@ft_beiyong)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@ft_bianhao", SqlDbType.Int,4),
					new SqlParameter("@ft_didian", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_teamStyle", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_team1", SqlDbType.NVarChar,200),
					new SqlParameter("@ft_team2", SqlDbType.NVarChar,200),
					new SqlParameter("@ft_result", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_state", SqlDbType.NChar,10),
					new SqlParameter("@ft_time", SqlDbType.DateTime,3),
					new SqlParameter("@ft_caipan", SqlDbType.NChar,10),
					new SqlParameter("@ft_news", SqlDbType.NVarChar),
					new SqlParameter("@ft_otherNews", SqlDbType.NVarChar),
					new SqlParameter("@ft_hit", SqlDbType.Int,4),
					new SqlParameter("@ft_glod", SqlDbType.Int,4),
					new SqlParameter("@ft_team1Explain", SqlDbType.NVarChar),
					new SqlParameter("@ft_team2Explain", SqlDbType.NVarChar),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@ft_addTime", SqlDbType.DateTime),
					new SqlParameter("@ft_overTime", SqlDbType.DateTime),
					new SqlParameter("@ft_state1", SqlDbType.NVarChar),
					new SqlParameter("@ft_state2", SqlDbType.NVarChar),
					new SqlParameter("@Identification", SqlDbType.Int,4),
					new SqlParameter("@ft_beiyong", SqlDbType.NVarChar)};
            parameters[0].Value = model.ft_bianhao;
            parameters[1].Value = model.ft_didian;
            parameters[2].Value = model.ft_teamStyle;
            parameters[3].Value = model.ft_team1;
            parameters[4].Value = model.ft_team2;
            parameters[5].Value = model.ft_result;
            parameters[6].Value = model.ft_state;
            parameters[7].Value = model.ft_time;
            parameters[8].Value = model.ft_caipan;
            parameters[9].Value = model.ft_news;
            parameters[10].Value = model.ft_otherNews;
            parameters[11].Value = model.ft_hit;
            parameters[12].Value = model.ft_glod;
            parameters[13].Value = model.ft_team1Explain;
            parameters[14].Value = model.ft_team2Explain;
            parameters[15].Value = model.isDone;
            parameters[16].Value = model.ft_addTime;
            parameters[17].Value = model.ft_overTime;
            parameters[18].Value = model.ft_state1;
            parameters[19].Value = model.ft_state2;
            parameters[20].Value = model.Identification;
            parameters[21].Value = model.ft_beiyong;

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
        public void UpdateHit(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZQLists set ");
            strSql.Append(" ft_hit= ft_hit +1 ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4),
                    new SqlParameter("@ft_hit", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            parameters[1].Value = 0;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_ZQLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZQLists set ");
            strSql.Append("ft_bianhao=@ft_bianhao,");
            strSql.Append("ft_didian=@ft_didian,");
            strSql.Append("ft_teamStyle=@ft_teamStyle,");
            strSql.Append("ft_team1=@ft_team1,");
            strSql.Append("ft_team2=@ft_team2,");
            strSql.Append("ft_result=@ft_result,");
            strSql.Append("ft_state=@ft_state,");
            strSql.Append("ft_time=@ft_time,");
            strSql.Append("ft_caipan=@ft_caipan,");
            strSql.Append("ft_news=@ft_news,");
            strSql.Append("ft_otherNews=@ft_otherNews,");
            strSql.Append("ft_hit=@ft_hit,");
            strSql.Append("ft_glod=@ft_glod,");
            strSql.Append("ft_team1Explain=@ft_team1Explain,");
            strSql.Append("ft_team2Explain=@ft_team2Explain,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("ft_addTime=@ft_addTime,");
            strSql.Append("ft_overTime=@ft_overTime,");
            strSql.Append("ft_state1=@ft_state1,");
            strSql.Append("ft_state2=@ft_state2,");
            strSql.Append("Identification=@Identification,");
            strSql.Append("ft_beiyong=@ft_beiyong");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@ft_bianhao", SqlDbType.Int,4),
					new SqlParameter("@ft_didian", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_teamStyle", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_team1", SqlDbType.NVarChar,200),
					new SqlParameter("@ft_team2", SqlDbType.NVarChar,200),
					new SqlParameter("@ft_result", SqlDbType.NVarChar,50),
					new SqlParameter("@ft_state", SqlDbType.NChar,10),
					new SqlParameter("@ft_time", SqlDbType.DateTime,3),
					new SqlParameter("@ft_caipan", SqlDbType.NChar,10),
					new SqlParameter("@ft_news", SqlDbType.NVarChar),
					new SqlParameter("@ft_otherNews", SqlDbType.NVarChar),
					new SqlParameter("@ft_hit", SqlDbType.Int,4),
					new SqlParameter("@ft_glod", SqlDbType.Int,4),
					new SqlParameter("@ft_team1Explain", SqlDbType.NVarChar),
					new SqlParameter("@ft_team2Explain", SqlDbType.NVarChar),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@ft_addTime", SqlDbType.DateTime),
					new SqlParameter("@ft_overTime", SqlDbType.DateTime),
					new SqlParameter("@ft_state1", SqlDbType.NVarChar),
					new SqlParameter("@ft_state2", SqlDbType.NVarChar),
					new SqlParameter("@Identification", SqlDbType.Int,4),
					new SqlParameter("@ft_beiyong", SqlDbType.NVarChar)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.ft_bianhao;
            parameters[2].Value = model.ft_didian;
            parameters[3].Value = model.ft_teamStyle;
            parameters[4].Value = model.ft_team1;
            parameters[5].Value = model.ft_team2;
            parameters[6].Value = model.ft_result;
            parameters[7].Value = model.ft_state;
            parameters[8].Value = model.ft_time;
            parameters[9].Value = model.ft_caipan;
            parameters[10].Value = model.ft_news;
            parameters[11].Value = model.ft_otherNews;
            parameters[12].Value = model.ft_hit;
            parameters[13].Value = model.ft_glod;
            parameters[14].Value = model.ft_team1Explain;
            parameters[15].Value = model.ft_team2Explain;
            parameters[16].Value = model.isDone;
            parameters[17].Value = model.ft_addTime;
            parameters[18].Value = model.ft_overTime;
            parameters[19].Value = model.ft_state1;
            parameters[20].Value = model.ft_state2;
            parameters[21].Value = model.Identification;
            parameters[22].Value = model.ft_beiyong;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZQLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到Result
        /// </summary>
        public string GetResultFromId(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ft_result from tb_ZQLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }
        /// <summary>
        /// 得到编号
        /// </summary>
        public int GetBianhaoFromId(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ft_bianhao from tb_ZQLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int)};
            parameters[0].Value = Id;
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
        /// 得到Id
        /// </summary>
        public int GetIdFromBianhao(int bianhao)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Id from tb_ZQLists ");
            strSql.Append(" where ft_bianhao=@bianhao ");
            SqlParameter[] parameters = {
                    new SqlParameter("@bianhao", SqlDbType.Int)};
            parameters[0].Value = bianhao;
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
        /// 得到一个对象实体Gettb_ZQListsFromBianhao
        /// </summary>
        public BCW.Model.tb_ZQLists Gettb_ZQListsFromBianhao(int ft_bianhao)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,ft_bianhao,ft_didian,ft_teamStyle,ft_team1,ft_team2,ft_result,ft_state,ft_time,ft_caipan,ft_news,ft_otherNews,ft_hit,ft_glod,ft_team1Explain,ft_team2Explain,isDone,ft_addTime,ft_overTime,ft_state1,ft_state2,Identification,ft_beiyong from tb_ZQLists ");
            strSql.Append(" where ft_bianhao=@ft_bianhao ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = ft_bianhao;

            BCW.Model.tb_ZQLists model = new BCW.Model.tb_ZQLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt32(0);
                    model.ft_bianhao = reader.GetInt32(1);
                    model.ft_didian = reader.GetString(2);
                    model.ft_teamStyle = reader.GetString(3);
                    model.ft_team1 = reader.GetString(4);
                    model.ft_team2 = reader.GetString(5);
                    model.ft_result = reader.GetString(6);
                    model.ft_state = reader.GetString(7);
                    model.ft_time = reader.GetDateTime(8);
                    model.ft_caipan = reader.GetString(9);
                    model.ft_news = reader.GetString(10);
                    model.ft_otherNews = reader.GetString(11);
                    model.ft_hit = reader.GetInt32(12);
                    model.ft_glod = reader.GetInt32(13);
                    model.ft_team1Explain = reader.GetString(14);
                    model.ft_team2Explain = reader.GetString(15);
                    model.isDone = reader.GetInt32(16);
                    model.ft_addTime = reader.GetDateTime(17);
                    model.ft_overTime = reader.GetDateTime(18);
                    model.ft_state1 = reader.GetString(19);
                    model.ft_state2 = reader.GetString(20);
                    model.Identification = reader.GetInt32(21);
                    model.ft_beiyong = reader.GetString(22);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_ZQLists Gettb_ZQLists(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,ft_bianhao,ft_didian,ft_teamStyle,ft_team1,ft_team2,ft_result,ft_state,ft_time,ft_caipan,ft_news,ft_otherNews,ft_hit,ft_glod,ft_team1Explain,ft_team2Explain,isDone,ft_addTime,ft_overTime,ft_state1,ft_state2,Identification,ft_beiyong from tb_ZQLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            BCW.Model.tb_ZQLists model = new BCW.Model.tb_ZQLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt32(0);
                    model.ft_bianhao = reader.GetInt32(1);
                    model.ft_didian = reader.GetString(2);
                    model.ft_teamStyle = reader.GetString(3);
                    model.ft_team1 = reader.GetString(4);
                    model.ft_team2 = reader.GetString(5);
                    model.ft_result = reader.GetString(6);
                    model.ft_state = reader.GetString(7);
                    model.ft_time = reader.GetDateTime(8);
                    model.ft_caipan = reader.GetString(9);
                    model.ft_news = reader.GetString(10);
                    model.ft_otherNews = reader.GetString(11);
                    model.ft_hit = reader.GetInt32(12);
                    model.ft_glod = reader.GetInt32(13);
                    model.ft_team1Explain = reader.GetString(14);
                    model.ft_team2Explain = reader.GetString(15);
                    model.isDone = reader.GetInt32(16);
                    model.ft_addTime = reader.GetDateTime(17);
                    model.ft_overTime = reader.GetDateTime(18);
                    model.ft_state1 = reader.GetString(19);
                    model.ft_state2 = reader.GetString(20);
                    model.Identification = reader.GetInt32(21);
                    model.ft_beiyong = reader.GetString(22);
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
            strSql.Append(" FROM tb_ZQLists ");
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
        /// <returns>IList tb_ZQLists</returns>
        public IList<BCW.Model.tb_ZQLists> Gettb_ZQListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_ZQLists> listtb_ZQListss = new List<BCW.Model.tb_ZQLists>();
            string sTable = "tb_ZQLists";
            string sPkey = "id";
            string sField = "Id,ft_bianhao,ft_didian,ft_teamStyle,ft_team1,ft_team2,ft_result,ft_state,ft_time,ft_caipan,ft_news,ft_otherNews,ft_hit,ft_glod,ft_team1Explain,ft_team2Explain,isDone,ft_addTime,ft_overTime,ft_state1,ft_state2,Identification,ft_beiyong";
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
                    return listtb_ZQListss;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_ZQLists objtb_ZQLists = new BCW.Model.tb_ZQLists();
                    objtb_ZQLists.Id = reader.GetInt32(0);
                    objtb_ZQLists.ft_bianhao = reader.GetInt32(1);
                    objtb_ZQLists.ft_didian = reader.GetString(2);
                    objtb_ZQLists.ft_teamStyle = reader.GetString(3);
                    objtb_ZQLists.ft_team1 = reader.GetString(4);
                    objtb_ZQLists.ft_team2 = reader.GetString(5);
                    objtb_ZQLists.ft_result = reader.GetString(6);
                    objtb_ZQLists.ft_state = reader.GetString(7);
                    objtb_ZQLists.ft_time = reader.GetDateTime(8);
                    objtb_ZQLists.ft_caipan = reader.GetString(9);
                    objtb_ZQLists.ft_news = reader.GetString(10);
                    objtb_ZQLists.ft_otherNews = reader.GetString(11);
                    objtb_ZQLists.ft_hit = reader.GetInt32(12);
                    objtb_ZQLists.ft_glod = reader.GetInt32(13);
                    objtb_ZQLists.ft_team1Explain = reader.GetString(14);
                    objtb_ZQLists.ft_team2Explain = reader.GetString(15);
                    objtb_ZQLists.isDone = reader.GetInt32(16);
                    objtb_ZQLists.ft_addTime = reader.GetDateTime(17);
                    objtb_ZQLists.ft_overTime = reader.GetDateTime(18);
                    objtb_ZQLists.ft_state1 = reader.GetString(19);
                    objtb_ZQLists.ft_state2 = reader.GetString(20);
                    objtb_ZQLists.Identification = reader.GetInt32(21);
                    objtb_ZQLists.ft_beiyong = reader.GetString(22);
                    listtb_ZQListss.Add(objtb_ZQLists);
                }
            }
            return listtb_ZQListss;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList tb_ZQLists</returns>
        public IList<BCW.Model.tb_ZQLists> Gettb_ZQListss2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.tb_ZQLists> listtb_ZQListss = new List<BCW.Model.tb_ZQLists>();
            string sTable = "tb_ZQLists";
            string sPkey = "id";
            string sField = "* ";
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
                    return listtb_ZQListss;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_ZQLists objtb_ZQLists = new BCW.Model.tb_ZQLists();
                    objtb_ZQLists.Id = reader.GetInt32(0);
                    objtb_ZQLists.ft_bianhao = reader.GetInt32(1);
                    objtb_ZQLists.ft_didian = reader.GetString(2);
                    objtb_ZQLists.ft_teamStyle = reader.GetString(3);
                    objtb_ZQLists.ft_team1 = reader.GetString(4);
                    objtb_ZQLists.ft_team2 = reader.GetString(5);
                    objtb_ZQLists.ft_result = reader.GetString(6);
                    objtb_ZQLists.ft_state = reader.GetString(7);
                    objtb_ZQLists.ft_time = reader.GetDateTime(8);
                    objtb_ZQLists.ft_caipan = reader.GetString(9);
                    objtb_ZQLists.ft_news = reader.GetString(10);
                    objtb_ZQLists.ft_otherNews = reader.GetString(11);
                    objtb_ZQLists.ft_hit = reader.GetInt32(12);
                    objtb_ZQLists.ft_glod = reader.GetInt32(13);
                    objtb_ZQLists.ft_team1Explain = reader.GetString(14);
                    objtb_ZQLists.ft_team2Explain = reader.GetString(15);
                    objtb_ZQLists.isDone = reader.GetInt32(16);
                    objtb_ZQLists.ft_addTime = reader.GetDateTime(17);
                    objtb_ZQLists.ft_overTime = reader.GetDateTime(18);
                    objtb_ZQLists.ft_state1 = reader.GetString(19);
                    objtb_ZQLists.ft_state2 = reader.GetString(20);
                    objtb_ZQLists.Identification = reader.GetInt32(21);
                    objtb_ZQLists.ft_beiyong = reader.GetString(22);
                    listtb_ZQListss.Add(objtb_ZQLists);
                }
            }
            return listtb_ZQListss;
        }
        #endregion  成员方法
    }
}

