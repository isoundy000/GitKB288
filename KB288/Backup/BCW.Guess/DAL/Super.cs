using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
/// <summary>
/// 增加重开奖取消兑奖入口
/// 黄国军 20160715
/// </summary>
namespace TPR.DAL.guess
{
    /// <summary>
    /// 数据访问类Super。
    /// </summary>
    public class Super
    {
        public Super()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Super");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Super");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsIsCase(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Super");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Status=@Status ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and getMoney>@getMoney ");
            strSql.Append(" and p_case=@p_case ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Status", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@getMoney", SqlDbType.Int,4),
					new SqlParameter("@p_case", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;
            parameters[2].Value = UsID;
            parameters[3].Value = 0;
            parameters[4].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateIsCase(int ID)
        {

            //更新此记录得到的币
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("p_case=@p_case");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_case", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 还原用户兑奖标识 一般用于重开奖
        /// </summary>
        /// <param name="ID"></param>
        public void UpdateNotCase(int ID)
        {
            //更新此记录得到的币
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("p_case=@p_case");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_case", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件得到赛事下注总注数
        /// </summary>
        public int GetSuperCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Super where " + strWhere + "");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// 根据条件得到赛事下注总金额
        /// </summary>
        public long GetSuperpayCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_Super where " + strWhere + "");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }


        /// <summary>
        /// 根据条件得到赛事下注盈利额
        /// </summary>
        public long GetSupergetMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(getMoney) from tb_Super where " + strWhere + "");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TPR.Model.guess.Super model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Super(");
            strSql.Append("UsID,UsName,BID,SP,Title,Times,StTitle,Odds,PayCent,Status,IsOpen,AddTime,PID)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@BID,@SP,@Title,@Times,@StTitle,@Odds,@PayCent,@Status,@IsOpen,@AddTime,@PID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,500),
					new SqlParameter("@BID", SqlDbType.NVarChar,500),
					new SqlParameter("@SP", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@Times", SqlDbType.NVarChar,500),
					new SqlParameter("@StTitle", SqlDbType.NVarChar,2000),
					new SqlParameter("@Odds", SqlDbType.NVarChar,500),
					new SqlParameter("@PayCent", SqlDbType.Money,8),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsOpen", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PID", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.BID;
            parameters[3].Value = model.SP;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Times;
            parameters[6].Value = model.StTitle;
            parameters[7].Value = model.Odds;
            parameters[8].Value = model.PayCent;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.IsOpen;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.PID;

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
        public void Update(TPR.Model.guess.Super model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BID=BID+@BID,");
            strSql.Append("SP=SP+@SP,");
            strSql.Append("Title=Title+@Title,");
            strSql.Append("Times=Times+@Times,");
            strSql.Append("StTitle=StTitle+@StTitle,");
            strSql.Append("Odds=Odds+@Odds,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("Status=@Status,");
            strSql.Append("IsOpen=@IsOpen,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("PID=PID+@PID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BID", SqlDbType.NVarChar,500),
					new SqlParameter("@SP", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@Times", SqlDbType.NVarChar,500),
					new SqlParameter("@StTitle", SqlDbType.NVarChar,2000),
					new SqlParameter("@Odds", SqlDbType.NVarChar,500),
					new SqlParameter("@PayCent", SqlDbType.Money,8),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsOpen", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PID", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BID;
            parameters[4].Value = model.SP;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.Times;
            parameters[7].Value = model.StTitle;
            parameters[8].Value = model.Odds;
            parameters[9].Value = model.PayCent;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.IsOpen;
            parameters[12].Value = model.AddTime;
            parameters[13].Value = model.PID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(TPR.Model.guess.Super model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BID=@BID,");
            strSql.Append("SP=@SP,");
            strSql.Append("Title=@Title,");
            strSql.Append("Times=@Times,");
            strSql.Append("StTitle=@StTitle,");
            strSql.Append("Odds=@Odds,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("Status=@Status,");
            strSql.Append("IsOpen=@IsOpen,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("PID=@PID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BID", SqlDbType.NVarChar,500),
					new SqlParameter("@SP", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@Times", SqlDbType.NVarChar,500),
					new SqlParameter("@StTitle", SqlDbType.NVarChar,2000),
					new SqlParameter("@Odds", SqlDbType.NVarChar,500),
					new SqlParameter("@PayCent", SqlDbType.Money,8),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@IsOpen", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PID", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BID;
            parameters[4].Value = model.SP;
            parameters[5].Value = model.Title;
            parameters[6].Value = model.Times;
            parameters[7].Value = model.StTitle;
            parameters[8].Value = model.Odds;
            parameters[9].Value = model.PayCent;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.IsOpen;
            parameters[12].Value = model.AddTime;
            parameters[13].Value = model.PID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(TPR.Model.guess.Super model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("IsOpen=@IsOpen,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayCent", SqlDbType.Money,8),
					new SqlParameter("@IsOpen", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.PayCent;
            parameters[2].Value = model.IsOpen;
            parameters[3].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update4(TPR.Model.guess.Super model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Super set ");
            strSql.Append("Status=@Status,");
            strSql.Append("getMoney=@getMoney,");
            strSql.Append("getOdds=@getOdds");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@getMoney", SqlDbType.Money,8),
					new SqlParameter("@getOdds", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Status;
            parameters[2].Value = model.getMoney;
            parameters[3].Value = model.getOdds;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Super ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Super ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个ID
        /// </summary>
        public int GetSuperID(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID from tb_Super ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and IsOpen=@IsOpen ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IsOpen", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到一个BID集合
        /// </summary>
        public string GetSuperBID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BID from tb_Super ");
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
        /// 得到一个Files
        /// </summary>
        public decimal GetgetMoney(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 getMoney from tb_Super ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetDecimal(0);
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
        /// 得到一个对象实体
        /// </summary>
        public TPR.Model.guess.Super GetSuper(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,BID,SP,Title,Times,StTitle,Odds,PayCent,Status,IsOpen,getMoney,getOdds,AddTime,PID from tb_Super ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR.Model.guess.Super model = new TPR.Model.guess.Super();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.BID = reader.GetString(2);
                    model.SP = reader.GetString(3);
                    model.Title = reader.GetString(4);
                    model.Times = reader.GetString(5);
                    model.StTitle = reader.GetString(6);
                    model.Odds = reader.GetString(7);
                    model.PayCent = reader.GetDecimal(8);
                    model.Status = reader.GetInt32(9);
                    model.IsOpen = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.getMoney = reader.GetDecimal(11);
                    if (!reader.IsDBNull(12))
                        model.getOdds = reader.GetString(12);

                    model.AddTime = reader.GetDateTime(13);
                    model.PID = reader.GetString(14);
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
            strSql.Append(" FROM tb_Super ");
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
        /// <returns>IList Super</returns>
        public IList<TPR.Model.guess.Super> GetSupers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<TPR.Model.guess.Super> listSupers = new List<TPR.Model.guess.Super>();
            string sTable = "tb_Super";
            string sPkey = "id";
            string sField = "ID,Title,PayCent,Status,p_case,getMoney,UsID";
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
                    return listSupers;
                }
                while (reader.Read())
                {
                    TPR.Model.guess.Super objSuper = new TPR.Model.guess.Super();
                    objSuper.ID = reader.GetInt32(0);
                    objSuper.Title = reader.GetString(1);
                    objSuper.PayCent = reader.GetDecimal(2);
                    objSuper.Status = reader.GetInt32(3);
                    objSuper.p_case = reader.GetInt32(4);
                    objSuper.getMoney = reader.GetDecimal(5);
                    objSuper.UsID = reader.GetInt32(6);
                    listSupers.Add(objSuper);
                }
            }
            return listSupers;
        }

        #endregion  成员方法
    }
}

