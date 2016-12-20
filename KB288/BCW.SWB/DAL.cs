using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.SWB
{/// <summary>
 /// 数据访问类:Demo
 /// </summary>
    public partial class DAL
    {
        public DAL()
        { }
        #region  BasicMethod
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Demo");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

       /*增
     /// <summary>
    /// 是否存在该会员ID
    /// </summary>
    */
        public bool ExistsUserID(int UserID, int GameId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Demo");
            strSql.Append(" where UserID=@UserID AND GameId=@GameId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                      new SqlParameter("@GameId", SqlDbType.Int,4)
            };
            parameters[0].Value = UserID;
            parameters[1].Value = GameId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //add
        /// <summary>
        /// 得到用户币
        /// </summary>
        public long GeUserGold(int UserId, int GameID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Money from tb_Demo ");
            strSql.Append(" where UserID=@UserID AND GameID=@GameID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                     new SqlParameter("@GameID", SqlDbType.Int,4)
            };
            parameters[0].Value = UserId;
            parameters[1].Value = GameID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }

        }
        //add
        /// <summary>
        /// 从用户Id得到一个对象实体
        /// </summary>
        public BCW.SWB.Model GetModelForUserId(int UserID, int GameId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UserID,Money,GameID,UpdateTime,Permission from tb_Demo ");
            strSql.Append(" where UserId=@UserId  AND GameId=@GameId order by ID desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserId", SqlDbType.Int,4),
                      new SqlParameter("@GameId", SqlDbType.Int,4)
            };
            parameters[0].Value = UserID;
            parameters[1].Value = GameId;

            BCW.SWB.Model model = new BCW.SWB.Model();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.SWB.Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Demo(");
            strSql.Append("UserID,Money,GameID,UpdateTime,Permission)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@Money,@GameID,@UpdateTime,@Permission)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Money", SqlDbType.BigInt,8),
                    new SqlParameter("@GameID", SqlDbType.Int,4),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Permission", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.Money;
            parameters[2].Value = model.GameID;
            parameters[3].Value = model.UpdateTime;
            parameters[4].Value = model.Permission;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.SWB.Model model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Demo set ");
            strSql.Append("UserID=@UserID,");
            strSql.Append("Money=@Money,");
            strSql.Append("GameID=@GameID,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Permission=@Permission");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Money", SqlDbType.BigInt,8),
                    new SqlParameter("@GameID", SqlDbType.Int,4),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Permission", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.Money;
            parameters[2].Value = model.GameID;
            parameters[3].Value = model.UpdateTime;
            parameters[4].Value = model.Permission;
            parameters[5].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 由用户ID更新钱币
        /// </summary>
        public void UpdateMoney(int UserID, long Money, int GameID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Demo set ");
            strSql.Append(" Money=Money+@Money ");
            strSql.Append(" where UserID=@UserID and GameID=@GameID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Money", SqlDbType.BigInt,8),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@GameID", SqlDbType.Int,4)};
            parameters[0].Value = Money;
            parameters[1].Value = UserID;
            parameters[2].Value = GameID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 由用户ID更新领钱时间
        /// </summary>
        public void UpdateTime(int UserID, DateTime UpdateTime, int GameID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Demo set ");
            strSql.Append(" UpdateTime=@UpdateTime ");
            strSql.Append(" where UserID=@UserID and GameID=@GameID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@GameID", SqlDbType.Int,4)};
            parameters[0].Value = UpdateTime;
            parameters[1].Value = UserID;
            parameters[2].Value = GameID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 由用户ID更新权限
        /// </summary>
        public void UpdatePermission(int UserID, int Permission, int GameID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Demo set ");
            strSql.Append(" Permission=@Permission ");
            strSql.Append(" where UserID=@UserID and GameID=@GameID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Permission",SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@GameID", SqlDbType.Int,4)};
            parameters[0].Value = Permission;
            parameters[1].Value = UserID;
            parameters[2].Value = GameID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Demo ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        public bool DeleteWhere(string wheres)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Demo ");
            strSql.Append(" where " + wheres);
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SWB.Model GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UserID,Money,GameID,UpdateTime,Permission from tb_Demo ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            BCW.SWB.Model model = new BCW.SWB.Model();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SWB.Model DataRowToModel(DataRow row)
        {
            BCW.SWB.Model model = new BCW.SWB.Model();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["Money"] != null && row["Money"].ToString() != "")
                {
                    model.Money = long.Parse(row["Money"].ToString());
                }
                if (row["GameID"] != null && row["GameID"].ToString() != "")
                {
                    model.GameID = int.Parse(row["GameID"].ToString());
                }
                if (row["UpdateTime"] != null && row["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
                }
                if (row["Permission"] != null && row["Permission"].ToString() != "")
                {
                    model.Permission = int.Parse(row["Permission"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UserID,Money,GameID,UpdateTime,Permission ");
            strSql.Append(" FROM tb_Demo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,UserID,Money,GameID,UpdateTime,Permission ");
            strSql.Append(" FROM tb_Demo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_Demo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_Demo T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ChatText</returns>
        public IList<BCW.SWB.Model> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.SWB.Model> listSWB = new List<BCW.SWB.Model>();
            string sTable = "tb_Demo";
            string sPkey = "ID";
            string sField = "ID,UserID,Money,GameID,UpdateTime,Permission";
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
                    return listSWB;
                }
                while (reader.Read())
                {
                    BCW.SWB.Model objSWB = new BCW.SWB.Model();
                    objSWB.ID = reader.GetInt32(0);
                    objSWB.UserID = reader.GetInt32(1);
                    objSWB.Money = reader.GetInt64(2);
                    objSWB.GameID = reader.GetInt32(3);
                    objSWB.UpdateTime = reader.GetDateTime(4);
                    objSWB.Permission = reader.GetInt32(5);
                    listSWB.Add(objSWB);
                }
            }
            return listSWB;
        }
        #endregion  BasicMethod
    }
}
