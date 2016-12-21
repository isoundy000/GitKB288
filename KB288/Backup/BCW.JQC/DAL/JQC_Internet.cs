using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.JQC.DAL
{
    /// <summary>
    /// 数据访问类JQC_Internet。
    /// </summary>
    public class JQC_Internet
    {
        public JQC_Internet()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_JQC_Internet");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Internet");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Internet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_JQC_Internet(");
            strSql.Append("phase,Match,Team_Home,Team_Away,Sale_Start,Sale_End,Start_Time,Score,Result)");
            strSql.Append(" values (");
            strSql.Append("@phase,@Match,@Team_Home,@Team_Away,@Sale_Start,@Sale_End,@Start_Time,@Score,@Result)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4),
                    new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Sale_Start", SqlDbType.DateTime),
                    new SqlParameter("@Sale_End", SqlDbType.DateTime),
                    new SqlParameter("@Start_Time", SqlDbType.NVarChar,300),
                    new SqlParameter("@Score", SqlDbType.NVarChar,50),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.phase;
            parameters[1].Value = model.Match;
            parameters[2].Value = model.Team_Home;
            parameters[3].Value = model.Team_Away;
            parameters[4].Value = model.Sale_Start;
            parameters[5].Value = model.Sale_End;
            parameters[6].Value = model.Start_Time;
            parameters[7].Value = model.Score;
            parameters[8].Value = model.Result;

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
        public void Update(BCW.JQC.Model.JQC_Internet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_JQC_Internet set ");
            strSql.Append("phase=@phase,");
            strSql.Append("Match=@Match,");
            strSql.Append("Team_Home=@Team_Home,");
            strSql.Append("Team_Away=@Team_Away,");
            strSql.Append("Sale_Start=@Sale_Start,");
            strSql.Append("Sale_End=@Sale_End,");
            strSql.Append("Start_Time=@Start_Time,");
            strSql.Append("Score=@Score,");
            strSql.Append("Result=@Result");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@phase", SqlDbType.Int,4),
                    new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Sale_Start", SqlDbType.DateTime),
                    new SqlParameter("@Sale_End", SqlDbType.DateTime),
                    new SqlParameter("@Start_Time", SqlDbType.NVarChar,300),
                    new SqlParameter("@Score", SqlDbType.NVarChar,50),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.phase;
            parameters[2].Value = model.Match;
            parameters[3].Value = model.Team_Home;
            parameters[4].Value = model.Team_Away;
            parameters[5].Value = model.Sale_Start;
            parameters[6].Value = model.Sale_End;
            parameters[7].Value = model.Start_Time;
            parameters[8].Value = model.Score;
            parameters[9].Value = model.Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_JQC_Internet ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_JQC_Internet ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.phase = reader.GetInt32(1);
                    model.Match = reader.GetString(2);
                    model.Team_Home = reader.GetString(3);
                    model.Team_Away = reader.GetString(4);
                    model.Sale_Start = reader.GetDateTime(5);
                    model.Sale_End = reader.GetDateTime(6);
                    model.Start_Time = reader.GetString(7);
                    model.Score = reader.GetString(8);
                    model.Result = reader.GetString(9);
                    model.nowprize = reader.GetInt32(10);
                    model.zhu = reader.GetString(11);
                    model.zhu_money = reader.GetString(12);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet2(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_JQC_Internet ");
            strSql.Append(" where phase=@ID ORDER BY ID desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.phase = reader.GetInt32(1);
                    model.Match = reader.GetString(2);
                    model.Team_Home = reader.GetString(3);
                    model.Team_Away = reader.GetString(4);
                    model.Sale_Start = reader.GetDateTime(5);
                    model.Sale_End = reader.GetDateTime(6);
                    model.Start_Time = reader.GetString(7);
                    model.Score = reader.GetString(8);
                    model.Result = reader.GetString(9);
                    model.nowprize = reader.GetInt32(10);
                    model.zhu = reader.GetString(11);
                    model.zhu_money = reader.GetString(12);
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
            strSql.Append(" FROM tb_JQC_Internet ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //========================
        /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update_ht(BCW.JQC.Model.JQC_Internet model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_JQC_Internet set ");
            strSql.Append("phase=@phase,");
            strSql.Append("Match=@Match,");
            strSql.Append("Team_Home=@Team_Home,");
            strSql.Append("Team_Away=@Team_Away,");
            strSql.Append("Sale_Start=@Sale_Start,");
            strSql.Append("Sale_End=@Sale_End,");
            strSql.Append("Start_Time=@Start_Time,");
            strSql.Append("Score=@Score,");
            strSql.Append("Result=@Result,");
            strSql.Append("nowprize=@nowprize,");
            strSql.Append("zhu=@zhu,");
            strSql.Append("zhu_money=@zhu_money");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@phase", SqlDbType.Int,4),
                    new SqlParameter("@Match", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Home", SqlDbType.NVarChar,300),
                    new SqlParameter("@Team_Away", SqlDbType.NVarChar,300),
                    new SqlParameter("@Sale_Start", SqlDbType.DateTime),
                    new SqlParameter("@Sale_End", SqlDbType.DateTime),
                    new SqlParameter("@Start_Time", SqlDbType.NVarChar,300),
                    new SqlParameter("@Score", SqlDbType.NVarChar,50),
                    new SqlParameter("@Result", SqlDbType.NVarChar,100),
                    new SqlParameter("@nowprize", SqlDbType.Int,4),
                    new SqlParameter("@zhu", SqlDbType.NVarChar,30),
                    new SqlParameter("@zhu_money", SqlDbType.NVarChar,30)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.phase;
            parameters[2].Value = model.Match;
            parameters[3].Value = model.Team_Home;
            parameters[4].Value = model.Team_Away;
            parameters[5].Value = model.Sale_Start;
            parameters[6].Value = model.Sale_End;
            parameters[7].Value = model.Start_Time;
            parameters[8].Value = model.Score;
            parameters[9].Value = model.Result;
            parameters[10].Value = model.nowprize;
            parameters[11].Value = model.zhu;
            parameters[12].Value = model.zhu_money;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// me_得到最后一期已开奖的数据——20160805
        /// </summary>
        public int Get_kaiID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) phase from tb_JQC_Internet where  Result!='' order by phase desc");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到最后一期未开奖的数据
        /// </summary>
        public int Get_newID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) phase from tb_JQC_Internet where  Result='' order by id ASC");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_后台奖池查询——20160808
        /// </summary>
        public int Get_oldID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) phase from tb_JQC_Internet where phase!='' and Sale_Start < getdate() and  Sale_End > getdate() and Result='' order by id desc");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///me_后台奖池查询——20160808
        /// </summary>
        public int Get_oldID2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) phase from tb_JQC_Internet where phase!='' and Result!='' order by id desc");
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_是否存在未开奖的期号 20161013邵广林
        /// </summary>
        public bool Exists_wei(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_JQC_Internet WHERE Result='' AND phase<@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_phase(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Internet");
            strSql.Append(" where phase=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_Result(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Internet");
            strSql.Append(" where phase=@ID and Result!='' ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet Update_Result(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_JQC_Internet SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet GetJQC_Internet_model(string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_JQC_Internet ");
            strSql.Append(where);

            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.phase = reader.GetInt32(1);
                    model.Match = reader.GetString(2);
                    model.Team_Home = reader.GetString(3);
                    model.Team_Away = reader.GetString(4);
                    model.Sale_Start = reader.GetDateTime(5);
                    model.Sale_End = reader.GetDateTime(6);
                    model.Start_Time = reader.GetString(7);
                    model.Score = reader.GetString(8);
                    model.Result = reader.GetString(9);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.phase = 0;
                    model.Result = "";
                    return model;
                }
            }
        }
        /// <summary>
        /// me_得到开奖号码的一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Internet Get_kainum(int Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_JQC_Internet where phase ='" + Lottery_issue + "'");

            BCW.JQC.Model.JQC_Internet model = new BCW.JQC.Model.JQC_Internet();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.phase = reader.GetInt32(1);
                    model.Match = reader.GetString(2);
                    model.Team_Home = reader.GetString(3);
                    model.Team_Away = reader.GetString(4);
                    model.Sale_Start = reader.GetDateTime(5);
                    model.Sale_End = reader.GetDateTime(6);
                    model.Start_Time = reader.GetString(7);
                    model.Score = reader.GetString(8);
                    model.Result = reader.GetString(9);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.phase = 0;
                    model.Result = "";
                    return model;
                }
            }
        }
        /// <summary>
        /// me_得到中奖注数
        /// </summary>
        public string get_zhumeney(int phase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT zhu_money FROM tb_JQC_Internet WHERE  phase='" + phase + "'");
            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4)};
            parameters[0].Value = phase;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// me_得到中奖注数
        /// </summary>
        public int get_jiangchi(int phase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT nowprize FROM tb_JQC_Internet WHERE  phase='" + phase + "'");
            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4)};
            parameters[0].Value = phase;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        //========================



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Internet</returns>
        public IList<BCW.JQC.Model.JQC_Internet> GetJQC_Internets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.JQC.Model.JQC_Internet> listJQC_Internets = new List<BCW.JQC.Model.JQC_Internet>();
            string sTable = "tb_JQC_Internet";
            string sPkey = "id";
            string sField = "*";
            string sCondition = strWhere;
            string sOrder = "phase Desc";
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
                    return listJQC_Internets;
                }
                while (reader.Read())
                {
                    BCW.JQC.Model.JQC_Internet objJQC_Internet = new BCW.JQC.Model.JQC_Internet();
                    objJQC_Internet.ID = reader.GetInt32(0);
                    objJQC_Internet.phase = reader.GetInt32(1);
                    objJQC_Internet.Match = reader.GetString(2);
                    objJQC_Internet.Team_Home = reader.GetString(3);
                    objJQC_Internet.Team_Away = reader.GetString(4);
                    objJQC_Internet.Sale_Start = reader.GetDateTime(5);
                    objJQC_Internet.Sale_End = reader.GetDateTime(6);
                    objJQC_Internet.Start_Time = reader.GetString(7);
                    objJQC_Internet.Score = reader.GetString(8);
                    objJQC_Internet.Result = reader.GetString(9);
                    objJQC_Internet.nowprize = reader.GetInt32(10);
                    objJQC_Internet.zhu = reader.GetString(11);
                    objJQC_Internet.zhu_money = reader.GetString(12);
                    listJQC_Internets.Add(objJQC_Internet);
                }
            }
            return listJQC_Internets;
        }

        #endregion  成员方法
    }
}

