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
    /// 数据访问类tb_WinnersAward。
    /// </summary>
    public class tb_WinnersAward
    {
        public tb_WinnersAward()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_WinnersAward");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_WinnersAward");
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_WinnersAward model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_WinnersAward(");
            strSql.Append("periods,awardNumber,awardNowCount,winNumber,winNowCount,getUsId,getWinNumber,identification,Remarks,isGet,addTime,overTime,award,isDone,getRedy)");
            strSql.Append(" values (");
            strSql.Append("@periods,@awardNumber,@awardNowCount,@winNumber,@winNowCount,@getUsId,@getWinNumber,@identification,@Remarks,@isGet,@addTime,@overTime,@award,@isDone,@getRedy)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@periods", SqlDbType.Int,4),
					new SqlParameter("@awardNumber", SqlDbType.Int,4),
					new SqlParameter("@awardNowCount", SqlDbType.Int,4),
					new SqlParameter("@winNumber", SqlDbType.Int,4),
					new SqlParameter("@winNowCount", SqlDbType.NVarChar),
					new SqlParameter("@getUsId", SqlDbType.NVarChar),
					new SqlParameter("@getWinNumber", SqlDbType.NVarChar),
					new SqlParameter("@identification", SqlDbType.Int,4),
					new SqlParameter("@Remarks", SqlDbType.NVarChar),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@addTime", SqlDbType.DateTime),
					new SqlParameter("@overTime", SqlDbType.DateTime),
					new SqlParameter("@award", SqlDbType.NVarChar),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@getRedy", SqlDbType.NVarChar)};
            parameters[0].Value = model.periods;
            parameters[1].Value = model.awardNumber;
            parameters[2].Value = model.awardNowCount;
            parameters[3].Value = model.winNumber;
            parameters[4].Value = model.winNowCount;
            parameters[5].Value = model.getUsId;
            parameters[6].Value = model.getWinNumber;
            parameters[7].Value = model.identification;
            parameters[8].Value = model.Remarks;
            parameters[9].Value = model.isGet;
            parameters[10].Value = model.addTime;
            parameters[11].Value = model.overTime;
            parameters[12].Value = model.award;
            parameters[13].Value = model.isDone;
            parameters[14].Value = model.getRedy;

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
        ///更新isDone
        /// </summary>
        public void UpdateIsDone(int Id, int isDone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersAward set ");
            strSql.Append("isDone=@isDone ");
            strSql.Append("where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4),
                    new SqlParameter("@isDone", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            parameters[1].Value = isDone;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_WinnersAward model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersAward set ");
            strSql.Append("periods=@periods,");
            strSql.Append("awardNumber=@awardNumber,");
            strSql.Append("awardNowCount=@awardNowCount,");
            strSql.Append("winNumber=@winNumber,");
            strSql.Append("winNowCount=@winNowCount,");
            strSql.Append("getUsId=@getUsId,");
            strSql.Append("getWinNumber=@getWinNumber,");
            strSql.Append("identification=@identification,");
            strSql.Append("Remarks=@Remarks,");
            strSql.Append("isGet=@isGet,");
            strSql.Append("addTime=@addTime,");
            strSql.Append("overTime=@overTime,");
            strSql.Append("award=@award,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("getRedy=@getRedy");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@periods", SqlDbType.Int,4),
					new SqlParameter("@awardNumber", SqlDbType.Int,4),
					new SqlParameter("@awardNowCount", SqlDbType.Int,4),
					new SqlParameter("@winNumber", SqlDbType.Int,4),
					new SqlParameter("@winNowCount", SqlDbType.NVarChar),
					new SqlParameter("@getUsId", SqlDbType.NVarChar),
					new SqlParameter("@getWinNumber", SqlDbType.NVarChar),
					new SqlParameter("@identification", SqlDbType.Int,4),
					new SqlParameter("@Remarks", SqlDbType.NVarChar),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@addTime", SqlDbType.DateTime),
					new SqlParameter("@overTime", SqlDbType.DateTime),
					new SqlParameter("@award", SqlDbType.NVarChar),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@getRedy", SqlDbType.NVarChar)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.periods;
            parameters[2].Value = model.awardNumber;
            parameters[3].Value = model.awardNowCount;
            parameters[4].Value = model.winNumber;
            parameters[5].Value = model.winNowCount;
            parameters[6].Value = model.getUsId;
            parameters[7].Value = model.getWinNumber;
            parameters[8].Value = model.identification;
            parameters[9].Value = model.Remarks;
            parameters[10].Value = model.isGet;
            parameters[11].Value = model.addTime;
            parameters[12].Value = model.overTime;
            parameters[13].Value = model.award;
            parameters[14].Value = model.isDone;
            parameters[15].Value = model.getRedy;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_WinnersAward ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到内线语句
        /// </summary>
        public string GetRemarks(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Remarks from tb_WinnersAward ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

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
        /// 得到奖池类型都最新奖池
        /// 返回奖池ID
        /// </summary>
        public int GetMaxAwardForType(string getRedy)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 ID from tb_WinnersAward ");
            strSql.Append(" where getRedy=@getRedy order by ID desc ");
            SqlParameter[] parameters = {
                 new SqlParameter("@getRedy", SqlDbType.NVarChar)};
            parameters[0].Value = getRedy;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_WinnersAward Gettb_WinnersAward(int Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,periods,awardNumber,awardNowCount,winNumber,winNowCount,getUsId,getWinNumber,identification,Remarks,isGet,addTime,overTime,award,isDone,getRedy from tb_WinnersAward ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;

            BCW.Model.tb_WinnersAward model = new BCW.Model.tb_WinnersAward();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt32(0);
                    model.periods = reader.GetInt32(1);
                    model.awardNumber = reader.GetInt32(2);
                    model.awardNowCount = reader.GetInt32(3);
                    model.winNumber = reader.GetInt32(4);
                    model.winNowCount = reader.GetString(5);
                    model.getUsId = reader.GetString(6);
                    model.getWinNumber = reader.GetString(7);
                    model.identification = reader.GetInt32(8);
                    model.Remarks = reader.GetString(9);
                    model.isGet = reader.GetInt32(10);
                    model.addTime = reader.GetDateTime(11);
                    model.overTime = reader.GetDateTime(12);
                    model.award = reader.GetString(13);
                    model.isDone = reader.GetInt32(14);
                    model.getRedy = reader.GetString(15);
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
            strSql.Append(" FROM tb_WinnersAward ");
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
        /// <returns>IList tb_WinnersAward</returns>
        public IList<BCW.Model.tb_WinnersAward> Gettb_WinnersAwards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_WinnersAward> listtb_WinnersAwards = new List<BCW.Model.tb_WinnersAward>();
            string sTable = "tb_WinnersAward";
            string sPkey = "id";
            string sField = "Id,periods,awardNumber,awardNowCount,winNumber,winNowCount,getUsId,getWinNumber,identification,Remarks,isGet,addTime,overTime,award,isDone,getRedy";
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
                    return listtb_WinnersAwards;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_WinnersAward objtb_WinnersAward = new BCW.Model.tb_WinnersAward();
                    objtb_WinnersAward.Id = reader.GetInt32(0);
                    objtb_WinnersAward.periods = reader.GetInt32(1);
                    objtb_WinnersAward.awardNumber = reader.GetInt32(2);
                    objtb_WinnersAward.awardNowCount = reader.GetInt32(3);
                    objtb_WinnersAward.winNumber = reader.GetInt32(4);
                    objtb_WinnersAward.winNowCount = reader.GetString(5);
                    objtb_WinnersAward.getUsId = reader.GetString(6);
                    objtb_WinnersAward.getWinNumber = reader.GetString(7);
                    objtb_WinnersAward.identification = reader.GetInt32(8);
                    objtb_WinnersAward.Remarks = reader.GetString(9);
                    objtb_WinnersAward.isGet = reader.GetInt32(10);
                    objtb_WinnersAward.addTime = reader.GetDateTime(11);
                    objtb_WinnersAward.overTime = reader.GetDateTime(12);
                    objtb_WinnersAward.award = reader.GetString(13);
                    objtb_WinnersAward.isDone = reader.GetInt32(14);
                    objtb_WinnersAward.getRedy = reader.GetString(15);
                    listtb_WinnersAwards.Add(objtb_WinnersAward);
                }
            }
            return listtb_WinnersAwards;
        }

        #endregion  成员方法
    }
}

