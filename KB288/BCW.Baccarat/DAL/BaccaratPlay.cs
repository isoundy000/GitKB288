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
	/// 数据访问类BaccaratPlay。
	/// </summary>
	public class BaccaratPlay
	{
		public BaccaratPlay()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_BaccaratPlay"); 
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
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratPlay");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratPlay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratPlay(");
            strSql.Append("UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,SetTime,ActID,LastTime)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@RoomID,@RoomDoName,@RoomDoTable,@RoomDoTotal,@RoomDoTitle,@RoomDoAnnouces,@RoomDoHigh,@RoomDoLow,@SetTime,@ActID,@LastTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTitle", SqlDbType.Text),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.Text),
					new SqlParameter("@RoomDoHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomDoLow", SqlDbType.Int,4),
					new SqlParameter("@SetTime", SqlDbType.DateTime),
					new SqlParameter("@ActID", SqlDbType.Int,4),
					new SqlParameter("@LastTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.RoomDoName;
            parameters[4].Value = model.RoomDoTable;
            parameters[5].Value = model.RoomDoTotal;
            parameters[6].Value = model.RoomDoTitle;
            parameters[7].Value = model.RoomDoAnnouces;
            parameters[8].Value = model.RoomDoHigh;
            parameters[9].Value = model.RoomDoLow;
            parameters[10].Value = model.SetTime;
            parameters[11].Value = model.ActID;
            parameters[12].Value = model.LastTime;

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
        /// 返回当前用户没结束的房间的个数
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay Times(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(*) as RoomTime ");
            strSql.Append("FROM tb_BaccaratPlay where UsID=@UsID AND ActID<2");
            SqlParameter[] parameters = {
                new SqlParameter("@UsID", SqlDbType.Int,4)
            };
            parameters[0].Value = UsID;

            BCW.Baccarat.Model.BaccaratPlay model = new BCW.Baccarat.Model.BaccaratPlay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if(reader.HasRows)
                {
                    reader.Read();
                    model.RoomTime = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratPlay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomDoName=@RoomDoName,");
            strSql.Append("RoomDoTable=@RoomDoTable,");
            strSql.Append("RoomDoTotal=@RoomDoTotal,");
            strSql.Append("RoomDoTitle=@RoomDoTitle,");
            strSql.Append("RoomDoAnnouces=@RoomDoAnnouces,");
            strSql.Append("RoomDoHigh=@RoomDoHigh,");
            strSql.Append("RoomDoLow=@RoomDoLow,");
            strSql.Append("SetTime=@SetTime,");
            strSql.Append("ActID=@ActID,");
            strSql.Append("LastTime=@LastTime");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTitle", SqlDbType.Text),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.Text),
					new SqlParameter("@RoomDoHigh", SqlDbType.Int,4),
					new SqlParameter("@RoomDoLow", SqlDbType.Int,4),
					new SqlParameter("@SetTime", SqlDbType.DateTime),
					new SqlParameter("@ActID", SqlDbType.Int,4),
					new SqlParameter("@LastTime", SqlDbType.DateTime),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.RoomDoName;
            parameters[4].Value = model.RoomDoTable;
            parameters[5].Value = model.RoomDoTotal;
            parameters[6].Value = model.RoomDoTitle;
            parameters[7].Value = model.RoomDoAnnouces;
            parameters[8].Value = model.RoomDoHigh;
            parameters[9].Value = model.RoomDoLow;
            parameters[10].Value = model.SetTime;
            parameters[11].Value = model.ActID;
            parameters[12].Value = model.LastTime;
            parameters[13].Value = model.ID;

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
		/// 删除一条数据2
		/// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratPlay ");
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
        /// 返回固定用户ID的数据
        /// </summary>
        /// <param name="UsID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratPlay GetOwnMessage(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_BaccaratPlay ");
            strSql.Append("where UsID=@UsID order by RoomID,RoomDoTable desc ");
            SqlParameter[] paramaters ={
                   new SqlParameter("@UsID",SqlDbType.Int,4)
            };
            paramaters[0].Value = UsID;

            BCW.Baccarat.Model.BaccaratPlay model = new BCW.Baccarat.Model.BaccaratPlay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), paramaters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.RoomID = reader.GetInt32(3);
                    model.RoomDoName = reader.GetString(4);
                    model.RoomDoTable = reader.GetInt32(5);
                    model.RoomDoTotal = reader.GetInt32(6);
                    model.RoomDoTitle = reader.GetString(7);
                    model.RoomDoAnnouces = reader.GetString(8);
                    model.RoomDoHigh = reader.GetInt32(9);
                    model.RoomDoLow = reader.GetInt32(10);
                    model.SetTime = reader.GetDateTime(11);
                    model.ActID = reader.GetInt32(12);
                    model.LastTime = reader.GetDateTime(13);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 更新某房间的彩池的资金
        /// </summary>
        public void UpdateTotal(int RoomID, int RoomDoTotal)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("RoomDoTotal=RoomDoTotal+@RoomDoTotal ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTotal;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新最高下注
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoHigh"></param>
        public void UpadteHigh(int RoomID,int RoomDoHigh)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("RoomDoHigh=@RoomDoHigh ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoHigh", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoHigh;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新最低下注
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoLow"></param>
        public void UpdateLow(int RoomID, int RoomDoLow)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("RoomDoLow=@RoomDoLow ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoLow", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoLow;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 直接封庄
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <param name="ActID"></param>
        public void UpdateRoom(int RoomID,int RoomDoTable,int ActID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("RoomDoTable=@RoomDoTable,ActID=@ActID ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
                    new SqlParameter("@ActID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;
            parameters[2].Value = ActID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新房间结束后的信息
        /// </summary>
        /// <param name="roomid"></param>
        /// <param name="table"></param>
        /// <param name="actid"></param>
        public void updateActID(int roomid,int table,int actid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append("RoomDoTable=@table,ActID=@actid ");
            strSql.Append("where RoomID=@roomid");
            SqlParameter[] parameters = {
					new SqlParameter("@roomid", SqlDbType.Int,4),
					new SqlParameter("@table", SqlDbType.Int,4),
                    new SqlParameter("@actid", SqlDbType.Int,4)};
            parameters[0].Value = roomid;
            parameters[1].Value = table;
            parameters[2].Value = actid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="title"></param>
        /// <param name="announces"></param>
        public void updateannounce(int RoomID, string title, string announces)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratPlay set ");
            strSql.Append(" RoomDoTitle=@title, RoomDoAnnouces=@announces ");
            strSql.Append(" where RoomID=@RoomID");
            SqlParameter[] parameters ={
                                          new SqlParameter("@RoomID",SqlDbType.Int,4),
                                          new SqlParameter("@title",SqlDbType.Text),
                                          new SqlParameter("@announces",SqlDbType.Text)
                                      };
            parameters[0].Value = RoomID;
            parameters[1].Value = title;
            parameters[2].Value = announces;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratPlay ");
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
        public BCW.Baccarat.Model.BaccaratPlay GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,SetTime,ActID,LastTime from tb_BaccaratPlay ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BaccaratPlay model = new BCW.Baccarat.Model.BaccaratPlay();
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
        public BCW.Baccarat.Model.BaccaratPlay DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratPlay model = new BCW.Baccarat.Model.BaccaratPlay();
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
                if (row["SetTime"] != null && row["SetTime"].ToString() != "")
                {
                    model.SetTime = DateTime.Parse(row["SetTime"].ToString());
                }
                if (row["ActID"] != null && row["ActID"].ToString() != "")
                {
                    model.ActID = int.Parse(row["ActID"].ToString());
                }
                if (row["LastTime"] != null && row["LastTime"].ToString() != "")
                {
                    model.LastTime = DateTime.Parse(row["LastTime"].ToString());
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
            strSql.Append("select ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,SetTime,ActID,LastTime ");
            strSql.Append(" FROM tb_BaccaratPlay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetPlayList(string strWhere,string strOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,SetTime,ActID,LastTime ");
            strSql.Append(" FROM tb_BaccaratPlay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            if (strOrder.Trim() != "")
            {
                strSql.Append(" order by " + strOrder);
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
            strSql.Append(" ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoTitle,RoomDoAnnouces,RoomDoHigh,RoomDoLow,SetTime,ActID,LastTime ");
            strSql.Append(" FROM tb_BaccaratPlay ");
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
            strSql.Append("select count(1) FROM tb_BaccaratPlay ");
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
            strSql.Append(")AS Row, T.*  from tb_BaccaratPlay T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取特定房间的对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratPlay GetPlay(int RoomID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratPlay ");
            strSql.Append("where RoomID=@RoomID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;

            BCW.Baccarat.Model.BaccaratPlay model = new BCW.Baccarat.Model.BaccaratPlay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.RoomID = reader.GetInt32(3);
                    model.RoomDoName = reader.GetString(4);
                    model.RoomDoTable = reader.GetInt32(5);
                    model.RoomDoTotal = reader.GetInt32(6);
                    model.RoomDoTitle = reader.GetString(7);
                    model.RoomDoAnnouces = reader.GetString(8);
                    model.RoomDoHigh = reader.GetInt32(9);
                    model.RoomDoLow = reader.GetInt32(10);
                    model.SetTime = reader.GetDateTime(11);
                    model.ActID = reader.GetInt32(12);
                    model.LastTime = reader.GetDateTime(13);
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
			strSql.Append(" FROM tb_BaccaratPlay ");
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
		/// <returns>IList BaccaratPlay</returns>
		public IList<BCW.Baccarat.Model.BaccaratPlay> GetBaccaratPlays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratPlay> listBaccaratPlays = new List<BCW.Baccarat.Model.BaccaratPlay>();
			string sTable = "tb_BaccaratPlay";
			string sPkey = "id";
			string sField = "*";
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
					return listBaccaratPlays;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratPlay objBaccaratPlay = new BCW.Baccarat.Model.BaccaratPlay();
						objBaccaratPlay.ID = reader.GetInt32(0);
						objBaccaratPlay.UsID = reader.GetInt32(1);
						objBaccaratPlay.UsName = reader.GetString(2);
						objBaccaratPlay.RoomID = reader.GetInt32(3);
						objBaccaratPlay.RoomDoName = reader.GetString(4);
						objBaccaratPlay.RoomDoTable = reader.GetInt32(5);
						objBaccaratPlay.RoomDoTotal = reader.GetInt32(6);
                        objBaccaratPlay.RoomDoTitle = reader.GetString(7);
						objBaccaratPlay.RoomDoAnnouces = reader.GetString(8);
						objBaccaratPlay.RoomDoHigh = reader.GetInt32(9);
						objBaccaratPlay.RoomDoLow = reader.GetInt32(10);
						objBaccaratPlay.SetTime = reader.GetDateTime(11);
						objBaccaratPlay.ActID = reader.GetInt32(12);
						objBaccaratPlay.LastTime = reader.GetDateTime(13);
						listBaccaratPlays.Add(objBaccaratPlay);
				}
			}
			return listBaccaratPlays;
		}

		#endregion  成员方法
	}
}

