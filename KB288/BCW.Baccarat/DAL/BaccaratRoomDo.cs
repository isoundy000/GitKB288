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
	/// 数据访问类BaccaratRoomDo。
	/// </summary>
	public class BaccaratRoomDo
	{
		public BaccaratRoomDo()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("RoomDoID", "tb_BaccaratRoomDo"); 
		}

        /// <summary>
        /// 清楚表记录
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RoomDoID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BaccaratRoomDo");
			strSql.Append(" where RoomDoID=@RoomDoID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomDoID", SqlDbType.Int,4)};
			parameters[0].Value = RoomDoID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Baccarat.Model.BaccaratRoomDo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BaccaratRoomDo(");
			strSql.Append("UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoAddTime,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,RoomDoEnd,state)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@UsName,@RoomID,@RoomDoName,@RoomDoTable,@RoomDoTotal,@RoomDoAdd,@RoomDoAddTime,@RoomDoTitle,@RoomDoAnnouces,@RoomDoHigh,@RoomDoLow,@RoomDoEnd,@state)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAdd", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAddTime", SqlDbType.DateTime),
					new SqlParameter("@RoomDoTitle", SqlDbType.Text),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.Text),
					new SqlParameter("@RoomDoHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomDoLow", SqlDbType.Int,4),
					new SqlParameter("@RoomDoEnd", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.RoomID;
			parameters[3].Value = model.RoomDoName;
			parameters[4].Value = model.RoomDoTable;
			parameters[5].Value = model.RoomDoTotal;
			parameters[6].Value = model.RoomDoAdd;
			parameters[7].Value = model.RoomDoAddTime;
			parameters[8].Value = model.RoomDoTitle;
			parameters[9].Value = model.RoomDoAnnouces;
			parameters[10].Value = model.RoomDoHigh;
			parameters[11].Value = model.RoomDoLow;
			parameters[12].Value = model.RoomDoEnd;
			parameters[13].Value = model.state;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public void Update(BCW.Baccarat.Model.BaccaratRoomDo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BaccaratRoomDo set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("RoomID=@RoomID,");
			strSql.Append("RoomDoName=@RoomDoName,");
			strSql.Append("RoomDoTable=@RoomDoTable,");
			strSql.Append("RoomDoTotal=@RoomDoTotal,");
			strSql.Append("RoomDoAdd=@RoomDoAdd,");
			strSql.Append("RoomDoAddTime=@RoomDoAddTime,");
			strSql.Append("RoomDoTitle=@RoomDoTitle,");
			strSql.Append("RoomDoAnnouces=@RoomDoAnnouces,");
			strSql.Append("RoomDoHigh=@RoomDoHigh,");
			strSql.Append("RoomDoLow=@RoomDoLow,");
			strSql.Append("RoomDoEnd=@RoomDoEnd,");
			strSql.Append("state=@state");
			strSql.Append(" where RoomDoID=@RoomDoID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomDoID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAdd", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAddTime", SqlDbType.DateTime),
					new SqlParameter("@RoomDoTitle", SqlDbType.Text),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.Text),
					new SqlParameter("@RoomDoHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomDoLow", SqlDbType.Int,4),
					new SqlParameter("@RoomDoEnd", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4)};
			parameters[0].Value = model.RoomDoID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.RoomID;
			parameters[4].Value = model.RoomDoName;
			parameters[5].Value = model.RoomDoTable;
			parameters[6].Value = model.RoomDoTotal;
			parameters[7].Value = model.RoomDoAdd;
			parameters[8].Value = model.RoomDoAddTime;
			parameters[9].Value = model.RoomDoTitle;
			parameters[10].Value = model.RoomDoAnnouces;
			parameters[11].Value = model.RoomDoHigh;
			parameters[12].Value = model.RoomDoLow;
			parameters[13].Value = model.RoomDoEnd;
			parameters[14].Value = model.state;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
           
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
        public bool Delete(int RoomDoID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BaccaratRoomDo ");
			strSql.Append(" where RoomDoID=@RoomDoID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomDoID", SqlDbType.Int,4)};
			parameters[0].Value = RoomDoID;

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
        public bool DeleteList(string RoomDoIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratRoomDo ");
            strSql.Append(" where RoomDoID in (" + RoomDoIDlist + ")  ");
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
        public BCW.Baccarat.Model.BaccaratRoomDo GetModel(int RoomDoID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RoomDoID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoAddTime,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,RoomDoEnd,state from tb_BaccaratRoomDo ");
            strSql.Append(" where RoomDoID=@RoomDoID");
            SqlParameter[] parameters = {
					new SqlParameter("@RoomDoID", SqlDbType.Int,4)
			};
            parameters[0].Value = RoomDoID;

            BCW.Baccarat.Model.BaccaratRoomDo model = new BCW.Baccarat.Model.BaccaratRoomDo();
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
        public BCW.Baccarat.Model.BaccaratRoomDo DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratRoomDo model = new BCW.Baccarat.Model.BaccaratRoomDo();
            if (row != null)
            {
                if (row["RoomDoID"] != null && row["RoomDoID"].ToString() != "")
                {
                    model.RoomDoID = int.Parse(row["RoomDoID"].ToString());
                }
                if (row["UsID"] != null && row["UsID"].ToString() != "")
                {
                    model.UsID = int.Parse(row["UsID"].ToString());
                }
                if (row["UsName"] != null)
                {
                    model.UsName = row["UsName"].ToString();
                }
                if (row["RoomID"] != null && row["RoomID"].ToString() != "")
                {
                    model.RoomID = int.Parse(row["RoomID"].ToString());
                }
                if (row["RoomDoName"] != null)
                {
                    model.RoomDoName = row["RoomDoName"].ToString();
                }
                if (row["RoomDoTable"] != null && row["RoomDoTable"].ToString() != "")
                {
                    model.RoomDoTable = int.Parse(row["RoomDoTable"].ToString());
                }
                if (row["RoomDoTotal"] != null && row["RoomDoTotal"].ToString() != "")
                {
                    model.RoomDoTotal = int.Parse(row["RoomDoTotal"].ToString());
                }
                if (row["RoomDoAdd"] != null && row["RoomDoAdd"].ToString() != "")
                {
                    model.RoomDoAdd = int.Parse(row["RoomDoAdd"].ToString());
                }
                if (row["RoomDoAddTime"] != null && row["RoomDoAddTime"].ToString() != "")
                {
                    model.RoomDoAddTime = DateTime.Parse(row["RoomDoAddTime"].ToString());
                }
                if (row["RoomDoTitle"] != null)
                {
                    model.RoomDoTitle = row["RoomDoTitle"].ToString();
                }
                if (row["RoomDoAnnouces"] != null)
                {
                    model.RoomDoAnnouces = row["RoomDoAnnouces"].ToString();
                }
                if (row["RoomDoHigh"] != null && row["RoomDoHigh"].ToString() != "")
                {
                    model.RoomDoHigh = int.Parse(row["RoomDoHigh"].ToString());
                }
                if (row["RoomDoLow"] != null && row["RoomDoLow"].ToString() != "")
                {
                    model.RoomDoLow = int.Parse(row["RoomDoLow"].ToString());
                }
                if (row["RoomDoEnd"] != null && row["RoomDoEnd"].ToString() != "")
                {
                    model.RoomDoEnd = DateTime.Parse(row["RoomDoEnd"].ToString());
                }
                if (row["state"] != null && row["state"].ToString() != "")
                {
                    model.state = int.Parse(row["state"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获取特定房间的对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratRoomDo GetBaccaratRoom(int RoomID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratRoomDo ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;

            BCW.Baccarat.Model.BaccaratRoomDo model = new BCW.Baccarat.Model.BaccaratRoomDo();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.RoomDoID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.RoomID = reader.GetInt32(3);
                    model.RoomDoName = reader.GetString(4);
                    model.RoomDoTable = reader.GetInt32(5);
                    model.RoomDoTotal = reader.GetInt32(6);
                    model.RoomDoAdd = reader.GetInt32(7);
                    model.RoomDoAddTime = reader.GetDateTime(8);
                    model.RoomDoTitle = reader.GetString(9);
                    model.RoomDoAnnouces = reader.GetString(10);
                    model.RoomDoHigh = reader.GetInt32(11);
                    model.RoomDoLow = reader.GetInt32(12);
                    model.RoomDoEnd = reader.GetDateTime(13);
                    model.state = reader.GetInt32(14);
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
            strSql.Append("select RoomDoID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoAddTime,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,RoomDoEnd,state ");
            strSql.Append(" FROM tb_BaccaratRoomDo ");
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
            strSql.Append(" RoomDoID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoAddTime,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,RoomDoEnd,state ");
            strSql.Append(" FROM tb_BaccaratRoomDo ");
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
            strSql.Append("select count(1) FROM tb_BaccaratRoomDo ");
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
                strSql.Append("order by T.RoomDoID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_BaccaratRoomDo T ");
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_BaccaratRoomDo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
		/// <returns>IList BaccaratRoomDo</returns>
        public IList<BCW.Baccarat.Model.BaccaratRoomDo> GetBaccaratRoomDos(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratRoomDo> listBaccaratRoomDos = new List<BCW.Baccarat.Model.BaccaratRoomDo>();
			string sTable = "tb_BaccaratRoomDo";
            string sPkey = "RoomDoID";
			string sField = "RoomDoID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoAddTime,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,RoomDoEnd,state";
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
					return listBaccaratRoomDos;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratRoomDo objBaccaratRoomDo = new BCW.Baccarat.Model.BaccaratRoomDo();
						objBaccaratRoomDo.RoomDoID = reader.GetInt32(0);
						objBaccaratRoomDo.UsID = reader.GetInt32(1);
						objBaccaratRoomDo.UsName = reader.GetString(2);
						objBaccaratRoomDo.RoomID = reader.GetInt32(3);
						objBaccaratRoomDo.RoomDoName = reader.GetString(4);
						objBaccaratRoomDo.RoomDoTable = reader.GetInt32(5);
						objBaccaratRoomDo.RoomDoTotal = reader.GetInt32(6);
						objBaccaratRoomDo.RoomDoAdd = reader.GetInt32(7);
						objBaccaratRoomDo.RoomDoAddTime = reader.GetDateTime(8);
						objBaccaratRoomDo.RoomDoTitle = reader.GetString(9);
						objBaccaratRoomDo.RoomDoAnnouces = reader.GetString(10);
						objBaccaratRoomDo.RoomDoHigh = reader.GetInt32(11);
						objBaccaratRoomDo.RoomDoLow = reader.GetInt32(12);
						objBaccaratRoomDo.RoomDoEnd = reader.GetDateTime(13);
						objBaccaratRoomDo.state = reader.GetInt32(14);
						listBaccaratRoomDos.Add(objBaccaratRoomDo);
				}
			}
			return listBaccaratRoomDos;
		}

		#endregion  成员方法
	}
}

