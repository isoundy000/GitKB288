using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Baccarat.DAL
{
	/// <summary>
	/// 数据访问类BaccaratUser。
	/// </summary>
	public class BaccaratUser
	{
		public BaccaratUser()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_BaccaratUser"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratUser");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 判断是否存在该用户记录
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public bool ExistsUser(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratUser");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratUser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratUser(");
            strSql.Append("UsID,UsName,SetID,RoomTimes)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@SetID,@RoomTimes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,255),
                    new SqlParameter("@SetID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTimes", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.SetID;
            parameters[3].Value = model.RoomTimes;

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
        public bool Update(BCW.Baccarat.Model.BaccaratUser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratUser set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("SetID=@SetID,");
            strSql.Append("RoomTimes=@RoomTimes");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,255),
                    new SqlParameter("@SetID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTimes", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.SetID;
            parameters[3].Value = model.RoomTimes;
            parameters[4].Value = model.ID;

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
        /// 更新用户开庄权限
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomTimes"></param>
        /// <returns></returns>
        public bool UpdateTimes(int UsID, int RoomTimes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BaccaratUser SET ");
            strSql.Append("RoomTimes=@RoomTimes WHERE UsID=@UsID");
            SqlParameter[] parameters = {
                new SqlParameter("@UsID", SqlDbType.Int,4),
                new SqlParameter("@RoomTimes", SqlDbType.Int,4)

            };
            parameters[0].Value = UsID;
            parameters[1].Value = RoomTimes;
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
        /// 更新用户发牌显示设置
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="SetID"></param>
        /// <returns></returns>
        public bool UpdateSet(int UsID, int SetID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BaccaratUser SET ");
            strSql.Append("SetID=@SetID WHERE UsID=@UsID");
            SqlParameter[] parameters = {
                new SqlParameter("@UsID", SqlDbType.Int,4),
                new SqlParameter("@SetID", SqlDbType.Int,4)

            };
            parameters[0].Value = UsID;
            parameters[1].Value = SetID;
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratUser ");
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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratUser ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
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
        public BCW.Baccarat.Model.BaccaratUser GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,SetID,RoomTimes from tb_BaccaratUser ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BaccaratUser model = new BCW.Baccarat.Model.BaccaratUser();
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
		public BCW.Baccarat.Model.BaccaratUser DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratUser model = new BCW.Baccarat.Model.BaccaratUser();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["UsID"] != null && row["UsID"].ToString() != "")
                {
                    model.UsID = int.Parse(row["UsID"].ToString());
                }
                if (row["UsName"] != null)
                {
                    model.UsName = row["UsName"].ToString();
                }
                if (row["SetID"] != null && row["SetID"].ToString() != "")
                {
                    model.SetID = int.Parse(row["SetID"].ToString());
                }
                if (row["RoomTimes"] != null && row["RoomTimes"].ToString() != "")
                {
                    model.RoomTimes = int.Parse(row["RoomTimes"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获取用户权限信息
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUser GetUser(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratUser ");
            strSql.Append("where UsID=@UsID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            BCW.Baccarat.Model.BaccaratUser model = new BCW.Baccarat.Model.BaccaratUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.SetID = reader.GetInt32(3);
                    model.RoomTimes = reader.GetInt32(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsID,UsName,SetID,RoomTimes ");
            strSql.Append(" FROM tb_BaccaratUser ");
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
            strSql.Append(" ID,UsID,UsName,SetID,RoomTimes ");
            strSql.Append(" FROM tb_BaccaratUser ");
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
            strSql.Append("select count(1) FROM tb_BaccaratUser ");
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
            strSql.Append(")AS Row, T.*  from tb_BaccaratUser T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_BaccaratUser ");
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
        /// <returns>IList BaccaratUser</returns>
        public IList<BCW.Baccarat.Model.BaccaratUser> GetBaccaratUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BaccaratUser> listBaccaratUsers = new List<BCW.Baccarat.Model.BaccaratUser>();
            string sTable = "tb_BaccaratUser";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,SetID,RoomTimes";
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
                    return listBaccaratUsers;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BaccaratUser objBaccaratUser = new BCW.Baccarat.Model.BaccaratUser();
                    objBaccaratUser.ID = reader.GetInt32(0);
                    objBaccaratUser.UsID = reader.GetInt32(1);
                    objBaccaratUser.UsName = reader.GetString(2);
                    objBaccaratUser.SetID = reader.GetInt32(3);
                    objBaccaratUser.RoomTimes = reader.GetInt32(4);
                    listBaccaratUsers.Add(objBaccaratUser);
                }
            }
            return listBaccaratUsers;
        }


        #endregion  成员方法
    }
}

