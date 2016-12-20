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
	/// 数据访问类BaccaratRoom。
	/// </summary>
	public class BaccaratRoom
	{
		public BaccaratRoom()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("RoomID", "tb_BaccaratRoom"); 
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
		public bool Exists(int RoomID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BaccaratRoom");
			strSql.Append(" where RoomID=@RoomID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4)};
			parameters[0].Value = RoomID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Baccarat.Model.BaccaratRoom model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BaccaratRoom(");
			strSql.Append("UsID,UsName,RoomTotal,RoomHigh,RoomLow,RoomStart)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@UsName,@RoomTotal,@RoomHigh,@RoomLow,@RoomStart)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomLow", SqlDbType.Int,4),
					new SqlParameter("@RoomStart", SqlDbType.DateTime)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.RoomTotal;
			parameters[3].Value = model.RoomHigh;
			parameters[4].Value = model.RoomLow;
			parameters[5].Value = model.RoomStart;

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
		public void Update(BCW.Baccarat.Model.BaccaratRoom model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BaccaratRoom set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("RoomTotal=@RoomTotal,");
			strSql.Append("RoomHigh=@RoomHigh,");
			strSql.Append("RoomLow=@RoomLow,");
			strSql.Append("RoomStart=@RoomStart");
			strSql.Append(" where RoomID=@RoomID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomLow", SqlDbType.Int,4),
					new SqlParameter("@RoomStart", SqlDbType.DateTime)};
			parameters[0].Value = model.RoomID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.RoomTotal;
			parameters[4].Value = model.RoomHigh;
			parameters[5].Value = model.RoomLow;
			parameters[6].Value = model.RoomStart;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int RoomID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BaccaratRoom ");
			strSql.Append(" where RoomID=@RoomID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4)};
			parameters[0].Value = RoomID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Baccarat.Model.BaccaratRoom GetBaccaratRoom(int RoomID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RoomID,UsID,UsName,RoomTotal,RoomHigh,RoomLow,RoomStart from tb_BaccaratRoom ");
			strSql.Append(" where RoomID=@RoomID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4)};
			parameters[0].Value = RoomID;

			BCW.Baccarat.Model.BaccaratRoom model=new BCW.Baccarat.Model.BaccaratRoom();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.RoomID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.RoomTotal = reader.GetInt32(3);
					model.RoomHigh = reader.GetInt32(4);
					model.RoomLow = reader.GetInt32(5);
					model.RoomStart = reader.GetDateTime(6);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_BaccaratRoom ");
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
		/// <returns>IList BaccaratRoom</returns>
        public IList<BCW.Baccarat.Model.BaccaratRoom> GetBaccaratRooms(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratRoom> listBaccaratRooms = new List<BCW.Baccarat.Model.BaccaratRoom>();
			string sTable = "tb_BaccaratRoom";
			string sPkey = "id";
			string sField = "RoomID,UsID,UsName,RoomTotal,RoomHigh,RoomLow,RoomStart";
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
					return listBaccaratRooms;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratRoom objBaccaratRoom = new BCW.Baccarat.Model.BaccaratRoom();
						objBaccaratRoom.RoomID = reader.GetInt32(0);
						objBaccaratRoom.UsID = reader.GetInt32(1);
						objBaccaratRoom.UsName = reader.GetString(2);
						objBaccaratRoom.RoomTotal = reader.GetInt32(3);
						objBaccaratRoom.RoomHigh = reader.GetInt32(4);
						objBaccaratRoom.RoomLow = reader.GetInt32(5);
						objBaccaratRoom.RoomStart = reader.GetDateTime(6);
						listBaccaratRooms.Add(objBaccaratRoom);
				}
			}
			return listBaccaratRooms;
		}

		#endregion  成员方法
	}
}

