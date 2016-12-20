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
	/// 数据访问类BaccaratUserDo。
	/// </summary>
	public class BaccaratUserDo
	{
		public BaccaratUserDo()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("DoID", "tb_BaccaratUserDo"); 
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
		public bool Exists(int DoID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BaccaratUserDo");
			strSql.Append(" where DoID=@DoID ");
			SqlParameter[] parameters = {
					new SqlParameter("@DoID", SqlDbType.Int,4)};
			parameters[0].Value = DoID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 某用户在某一个房间的桌面是否存在了该类型的数据
        /// </summary>
        public bool ExistsMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratUserDo ");
            strSql.Append("where UsID=@UsID and RoomID=@RoomID and RoomDoTable=@RoomDoTable and BetTypes=@BetTypes");
            SqlParameter[] parameters = {
                                        new SqlParameter("@UsID",SqlDbType.Int,4),
                                        new SqlParameter("@RoomID",SqlDbType.Int,4),
                                        new SqlParameter("@RoomDoTable",SqlDbType.Int,4),
                                        new SqlParameter("@BetTypes",SqlDbType.VarChar,50)};
            parameters[0].Value = UsID;
            parameters[1].Value = RoomID;
            parameters[2].Value = RoomDoTable;
            parameters[3].Value = BetTypes;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获取房间的最新台面
        /// </summary>
        /// <param name="RoomID"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo NewRoomMessage(int RoomID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_BaccaratUserDo ");
            strSql.Append("where RoomID=@RoomID order by RoomDoTable desc");
            SqlParameter[] parameters ={
                                          new SqlParameter("@RoomID",SqlDbType.Int,4)
                                      };
            parameters[0].Value = RoomID;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.DoID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.RoomID = reader.GetInt32(3);
                    model.RoomDoName = reader.GetString(4);
                    model.RoomDoTable = reader.GetInt32(5);
                    model.RoomDoTotal = reader.GetInt32(6);
                    model.BetMoney = reader.GetInt32(7);
                    model.BetTypes = reader.GetString(8);
                    model.BetDate = reader.GetDateTime(9);
                    model.BonusMoney = reader.GetInt32(10);
                    model.BonusTimes = reader.GetDateTime(11);
                    model.type = reader.GetInt32(12);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Baccarat.Model.BaccaratUserDo model)
		{
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratUserDo(");
            strSql.Append("UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,BetMoney,BetTypes,BetDate,BonusMoney,BonusTimes,type)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@RoomID,@RoomDoName,@RoomDoTable,@RoomDoTotal,@BetMoney,@BetTypes,@BetDate,@BonusMoney,@BonusTimes,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@BetMoney", SqlDbType.Int,4),
					new SqlParameter("@BetTypes", SqlDbType.VarChar,50),
					new SqlParameter("@BetDate", SqlDbType.DateTime),
					new SqlParameter("@BonusMoney", SqlDbType.Int,4),
					new SqlParameter("@BonusTimes", SqlDbType.DateTime),
					new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.RoomDoName;
            parameters[4].Value = model.RoomDoTable;
            parameters[5].Value = model.RoomDoTotal;
            parameters[6].Value = model.BetMoney;
            parameters[7].Value = model.BetTypes;
            parameters[8].Value = model.BetDate;
            parameters[9].Value = model.BonusMoney;
            parameters[10].Value = model.BonusTimes;
            parameters[11].Value = model.type;

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
		public void Update(BCW.Baccarat.Model.BaccaratUserDo model)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("update tb_BaccaratUserDo set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomDoName=@RoomDoName,");
            strSql.Append("RoomDoTable=@RoomDoTable,");
            strSql.Append("RoomDoTotal=@RoomDoTotal,");
            strSql.Append("BetMoney=@BetMoney,");
            strSql.Append("BetTypes=@BetTypes,");
            strSql.Append("BetDate=@BetDate,");
            strSql.Append("BonusMoney=@BonusMoney,");
            strSql.Append("BonusTimes=@BonusTimes,");
            strSql.Append("type=@type");
            strSql.Append(" where DoID=@DoID");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@BetMoney", SqlDbType.Int,4),
					new SqlParameter("@BetTypes", SqlDbType.VarChar,50),
					new SqlParameter("@BetDate", SqlDbType.DateTime),
					new SqlParameter("@BonusMoney", SqlDbType.Int,4),
					new SqlParameter("@BonusTimes", SqlDbType.DateTime),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@DoID", SqlDbType.Int,4)};
			parameters[0].Value = model.DoID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.RoomID;
			parameters[4].Value = model.RoomDoName;
			parameters[5].Value = model.RoomDoTable;
            parameters[6].Value = model.RoomDoTotal;
			parameters[7].Value = model.BetMoney;
			parameters[8].Value = model.BetTypes;
			parameters[9].Value = model.BetDate;
			parameters[10].Value = model.BonusMoney;
			parameters[11].Value = model.BonusTimes;
			parameters[12].Value = model.type;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新某一个表的某一特定字段
        /// </summary>
        public DataSet UpdateBonus(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratUserDo set ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());

        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int DoID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BaccaratUserDo ");
			strSql.Append(" where DoID=@DoID ");
			SqlParameter[] parameters = {
					new SqlParameter("@DoID", SqlDbType.Int,4)};
			parameters[0].Value = DoID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 返回具体信息
        /// </summary>
        /// <param name="UsID"></param>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserDo(int UsID, int RoomID, int RoomTable, string BetType)
        {
            StringBuilder strSql = new StringBuilder();
            //SELECT * from tb_BaccaratHistory where RoomID=8 and RoomDoTable=10 and BonusMoney>0
            strSql.Append("select * from tb_BaccaratUserDo ");
            strSql.Append("where UsID=@UsID and RoomID=@RoomID and RoomDoTable=@RoomTable and BetTypes=@BetType ");
            SqlParameter[] paramaters ={
                   new SqlParameter("@UsID",SqlDbType.Int,4),
                   new SqlParameter("@RoomID",SqlDbType.Int,4),
                   new SqlParameter("@RoomTable",SqlDbType.Int,4),
                   new SqlParameter("@BetType",SqlDbType.VarChar,50)
            };
            paramaters[0].Value = UsID;
            paramaters[1].Value = RoomID;
            paramaters[2].Value = RoomTable;
            paramaters[3].Value = BetType;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), paramaters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.DoID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.RoomID = reader.GetInt32(3);
                    model.RoomDoName = reader.GetString(4);
                    model.RoomDoTable = reader.GetInt32(5);
                    model.RoomDoTotal = reader.GetInt32(6);
                    model.BetMoney = reader.GetInt32(7);
                    model.BetTypes = reader.GetString(8);
                    model.BetDate = reader.GetDateTime(9);
                    model.BonusMoney = reader.GetInt32(10);
                    model.BonusTimes = reader.GetDateTime(11);
                    model.type = reader.GetInt32(12);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获取在确定用户、房间、桌面和下注类型的数据类型
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetBetTypesMessage(int UsID, int RoomID, int RoomDoTable, string BetTypes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_BaccaratUserDo ");
            strSql.Append("where UsID=@UsID and RoomID=@RoomID and RoomDoTable=@RoomDoTable and BetTypes=@BetTypes");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
                    new SqlParameter("@BetTypes", SqlDbType.VarChar,50)};
            parameters[0].Value = UsID;
            parameters[1].Value = RoomID;
            parameters[2].Value = RoomDoTable;
            parameters[3].Value = BetTypes;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
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
		public BCW.Baccarat.Model.BaccaratUserDo GetBaccaratUserDo(int DoID)
		{

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 DoID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,BetMoney,BetTypes,BetDate,BonusMoney,BonusTimes,type from tb_BaccaratUserDo ");
            strSql.Append(" where DoID=@DoID");
            SqlParameter[] parameters = {
					new SqlParameter("@DoID", SqlDbType.Int,4)
			};
            parameters[0].Value = DoID;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
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
        public BCW.Baccarat.Model.BaccaratUserDo DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
            if (row != null)
            {
                if (row["DoID"] != null && row["DoID"].ToString() != "")
                {
                    model.DoID = int.Parse(row["DoID"].ToString());
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
                if (row["BetMoney"] != null && row["BetMoney"].ToString() != "")
                {
                    model.BetMoney = int.Parse(row["BetMoney"].ToString());
                }
                if (row["BetTypes"] != null)
                {
                    model.BetTypes = row["BetTypes"].ToString();
                }
                if (row["BetDate"] != null && row["BetDate"].ToString() != "")
                {
                    model.BetDate = DateTime.Parse(row["BetDate"].ToString());
                }
                if (row["BonusMoney"] != null && row["BonusMoney"].ToString() != "")
                {
                    model.BonusMoney = int.Parse(row["BonusMoney"].ToString());
                }
                if (row["BonusTimes"] != null && row["BonusTimes"].ToString() != "")
                {
                    model.BonusTimes = DateTime.Parse(row["BonusTimes"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获取特定房间的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomMessage(int RoomID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratUserDo ");
            strSql.Append("where RoomID=@RoomID order by BetDate desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
            //using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            //{
            //    if (reader.HasRows)
            //    {
            //        reader.Read();
            //        model.DoID = reader.GetInt32(0);
            //        model.UsID = reader.GetInt32(1);
            //        model.UsName = reader.GetString(2);
            //        model.RoomID = reader.GetInt32(3);
            //        model.RoomDoName = reader.GetString(4);
            //        model.RoomDoTable = reader.GetInt32(5);
            //        model.BetMoney = reader.GetInt32(6);
            //        model.BetTypes = reader.GetString(7);
            //        model.BetDate = reader.GetDateTime(8);
            //        model.BonusMoney = reader.GetInt32(9);
            //        model.BonusTimes = reader.GetDateTime(10);
            //        model.type = reader.GetInt32(11);
            //        return model;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetRoomtableMessage(int RoomID,int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratUserDo ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomDoTable order by BetDate desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
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
       /// 获取特定用户ID在特定房间ID和台数Table的所有数据
       /// </summary>
        public BCW.Baccarat.Model.BaccaratUserDo GetUserMessage(int UsID, int RoomID, int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_BaccaratUserDo ");
            strSql.Append("where UsID=@UsID and RoomID=@RoomID and RoomDoTable=@RoomDoTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = RoomID;
            parameters[2].Value = RoomDoTable;

            BCW.Baccarat.Model.BaccaratUserDo model = new BCW.Baccarat.Model.BaccaratUserDo();
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
        ///得到特定房间ID和桌面table的最新的下注时间
        /// </summary>
        public DateTime GetMaxBetTime(int RoomID, int RoomDoTable, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)BetDate from tb_BaccaratUserDo ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomDoTable and type=@type order by BetDate desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;
            parameters[2].Value = type;
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime .Now;
                }
            }
        }

        /// <summary>
        ///得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int RoomDoTable,int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)BetDate from tb_BaccaratUserDo ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomDoTable and type=@type order by BetDate asc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;
            parameters[2].Value = type;
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime.Now;
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
			strSql.Append(" FROM tb_BaccaratUserDo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  * ");
            strSql.Append(" FROM tb_BaccaratUserDo ");
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
		/// <returns>IList BaccaratUserDo</returns>
		public IList<BCW.Baccarat.Model.BaccaratUserDo> GetBaccaratUserDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratUserDo> listBaccaratUserDos = new List<BCW.Baccarat.Model.BaccaratUserDo>();
			string sTable = "tb_BaccaratUserDo";
			string sPkey = "id";
			string sField = "*";
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
					return listBaccaratUserDos;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratUserDo objBaccaratUserDo = new BCW.Baccarat.Model.BaccaratUserDo();
						objBaccaratUserDo.DoID = reader.GetInt32(0);
						objBaccaratUserDo.UsID = reader.GetInt32(1);
						objBaccaratUserDo.UsName = reader.GetString(2);
						objBaccaratUserDo.RoomID = reader.GetInt32(3);
						objBaccaratUserDo.RoomDoName = reader.GetString(4);
						objBaccaratUserDo.RoomDoTable = reader.GetInt32(5);
                        objBaccaratUserDo.RoomDoTotal = reader.GetInt32(6);
						objBaccaratUserDo.BetMoney = reader.GetInt32(7);
						objBaccaratUserDo.BetTypes = reader.GetString(8);
						objBaccaratUserDo.BetDate = reader.GetDateTime(9);
						objBaccaratUserDo.BonusMoney = reader.GetInt32(10);
						objBaccaratUserDo.BonusTimes = reader.GetDateTime(11);
						objBaccaratUserDo.type = reader.GetInt32(12);
						listBaccaratUserDos.Add(objBaccaratUserDo);
				}
			}
			return listBaccaratUserDos;
		}
        //取排行榜数据列表
        public List<int> GetListBang(int RoomID, int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsID,SUM(convert(int,BetTypes)) from tb_BaccaratUserDo ");
            strSql.Append(" where RoomID="+ RoomID + " AND RoomDoTable="+ RoomDoTable + "  ");
            strSql.Append(" group by UsID");
            DataSet lis= SqlHelper.Query(strSql.ToString());
            List<int> list = new List<int>();
            for(int i = 0; i < lis.Tables[0].Rows.Count; i++)
            {
                list.Add(Convert.ToInt32(lis.Tables[0].Rows[i][0]));
            }
            return list;
        }
        #endregion  成员方法
    }
}

