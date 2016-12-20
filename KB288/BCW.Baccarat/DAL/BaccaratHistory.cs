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
	/// 数据访问类BaccaratHistory。
	/// </summary>
	public class BaccaratHistory
	{
		public BaccaratHistory()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("HID", "tb_BaccaratHistory");
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
        public bool Exists(int HID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratHistory");
            strSql.Append(" where HID=@HID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HID", SqlDbType.Int,4)			};
            parameters[0].Value = HID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratHistory(");
            strSql.Append("RoomID,RoomDoName,RoomDoTable,UsID,UsName,BetMoney,BetType,BankerPoker,BankerPoint,HunterPoker,HunterPoint,BonusPlayer,BonusMoney,BonusTimes)");
            strSql.Append(" values (");
            strSql.Append("@RoomID,@RoomDoName,@RoomDoTable,@UsID,@UsName,@BetMoney,@BetType,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint,@BonusPlayer,@BonusMoney,@BonusTimes)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.VarChar,50),
                    new SqlParameter("@BetMoney", SqlDbType.Int,4),
                    new SqlParameter("@BetType", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4),
                    new SqlParameter("@BonusPlayer", SqlDbType.NVarChar,50),
                    new SqlParameter("@BonusMoney", SqlDbType.Int,4),
                    new SqlParameter("@BonusTimes", SqlDbType.DateTime)};
            parameters[0].Value = model.RoomID;
            parameters[1].Value = model.RoomDoName;
            parameters[2].Value = model.RoomDoTable;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.BetMoney;
            parameters[6].Value = model.BetType;
            parameters[7].Value = model.BankerPoker;
            parameters[8].Value = model.BankerPoint;
            parameters[9].Value = model.HunterPoker;
            parameters[10].Value = model.HunterPoint;
            parameters[11].Value = model.BonusPlayer;
            parameters[12].Value = model.BonusMoney;
            parameters[13].Value = model.BonusTimes;

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
        public bool Update(BCW.Baccarat.Model.BaccaratHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratHistory set ");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomDoName=@RoomDoName,");
            strSql.Append("RoomDoTable=@RoomDoTable,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BetMoney=@BetMoney,");
            strSql.Append("BetType=@BetType,");
            strSql.Append("BankerPoker=@BankerPoker,");
            strSql.Append("BankerPoint=@BankerPoint,");
            strSql.Append("HunterPoker=@HunterPoker,");
            strSql.Append("HunterPoint=@HunterPoint,");
            strSql.Append("BonusPlayer=@BonusPlayer,");
            strSql.Append("BonusMoney=@BonusMoney,");
            strSql.Append("BonusTimes=@BonusTimes");
            strSql.Append(" where HID=@HID ");
            SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.NVarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.VarChar,50),
					new SqlParameter("@BetMoney", SqlDbType.Int,4),
					new SqlParameter("@BetType", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoint", SqlDbType.Int,4),
					new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
					new SqlParameter("@HunterPoint", SqlDbType.Int,4),
					new SqlParameter("@BonusPlayer", SqlDbType.NVarChar,50),
					new SqlParameter("@BonusMoney", SqlDbType.Int,4),
					new SqlParameter("@BonusTimes", SqlDbType.DateTime),
					new SqlParameter("@HID", SqlDbType.Int,4)};
            parameters[0].Value = model.RoomID;
            parameters[1].Value = model.RoomDoName;
            parameters[2].Value = model.RoomDoTable;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.BetMoney;
            parameters[6].Value = model.BetType;
            parameters[7].Value = model.BankerPoker;
            parameters[8].Value = model.BankerPoint;
            parameters[9].Value = model.HunterPoker;
            parameters[10].Value = model.HunterPoint;
            parameters[11].Value = model.BonusPlayer;
            parameters[12].Value = model.BonusMoney;
            parameters[13].Value = model.BonusTimes;
            parameters[14].Value = model.HID;

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
        public bool Delete(int HID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratHistory ");
            strSql.Append(" where HID=@HID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HID", SqlDbType.Int,4)			};
            parameters[0].Value = HID;

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
        /// 返回一条获奖数据信息
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomTable"></param>
        /// <param name="UsID"></param>
        /// <param name="BetType"></param>
        /// <returns></returns>
        public BCW.Baccarat.Model.BaccaratHistory GetHistory( int RoomID, int RoomTable, int UsID, string BetType)
        {
            StringBuilder strSql = new StringBuilder();
            //SELECT * from tb_BaccaratHistory where RoomID=8 and RoomDoTable=10 and BonusMoney>0
            strSql.Append("select * from tb_BaccaratHistory ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomTable and UsID=@UsID and BetType=@BetType and BonusMoney>0");
            SqlParameter[] paramaters ={
                   new SqlParameter("@RoomID",SqlDbType.Int,4),
                   new SqlParameter("@RoomTable",SqlDbType.Int,4),
                   new SqlParameter("@UsID",SqlDbType.Int,4),
                   new SqlParameter("@BetType",SqlDbType.VarChar,50)
            };
            paramaters[0].Value = RoomID;
            paramaters[1].Value = RoomTable;
            paramaters[2].Value = UsID;
            paramaters[3].Value = BetType;

            BCW.Baccarat.Model.BaccaratHistory model = new BCW.Baccarat.Model.BaccaratHistory();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), paramaters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.HID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomDoName = reader.GetString(2);
                    model.RoomDoTable= reader.GetInt32(3);
                    model.UsID= reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.BetMoney = reader.GetInt32(6);
                    model.BetType = reader.GetString(7);
                    model.BankerPoker = reader.GetString(8);
                    model.BankerPoint = reader.GetInt32(9);
                    model.HunterPoker = reader.GetString(10);
                    model.HunterPoint = reader.GetInt32(11);
                    model.BonusPlayer = reader.GetString(12);
                    model.BonusMoney = reader.GetInt32(13);
                    model.BonusTimes = reader.GetDateTime(14);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public DateTime GetMinTime(int RoomID, int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            //SELECT * from tb_BaccaratHistory where RoomID=16 and RoomDoTable=1 ORDER by BonusTimes ASC
            strSql.Append("select top(1)BonusTimes from tb_BaccaratHistory ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomDoTable order by BonusTimes asc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratHistory GetModel(int HID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 HID,RoomID,RoomDoName,RoomDoTable,UsID,UsName,BetMoney,BetType,BankerPoker,BankerPoint,HunterPoker,HunterPoint,BonusPlayer,BonusMoney,BonusTimes from tb_BaccaratHistory ");
            strSql.Append(" where HID=@HID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HID", SqlDbType.Int,4)			};
            parameters[0].Value = HID;

            BCW.Baccarat.Model.BaccaratHistory model = new BCW.Baccarat.Model.BaccaratHistory();
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
        public BCW.Baccarat.Model.BaccaratHistory DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratHistory model = new BCW.Baccarat.Model.BaccaratHistory();
            if (row != null)
            {
                if (row["HID"] != null && row["HID"].ToString() != "")
                {
                    model.HID = int.Parse(row["HID"].ToString());
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
                if (row["UsID"] != null && row["UsID"].ToString() != "")
                {
                    model.UsID = int.Parse(row["UsID"].ToString());
                }
                if (row["UsName"] != null)
                {
                    model.UsName = row["UsName"].ToString();
                }
                if (row["BetMoney"] != null && row["BetMoney"].ToString() != "")
                {
                    model.BetMoney = int.Parse(row["BetMoney"].ToString());
                }
                if (row["BetType"] != null)
                {
                    model.BetType = row["BetType"].ToString();
                }
                if (row["BankerPoker"] != null)
                {
                    model.BankerPoker = row["BankerPoker"].ToString();
                }
                if (row["BankerPoint"] != null && row["BankerPoint"].ToString() != "")
                {
                    model.BankerPoint = int.Parse(row["BankerPoint"].ToString());
                }
                if (row["HunterPoker"] != null)
                {
                    model.HunterPoker = row["HunterPoker"].ToString();
                }
                if (row["HunterPoint"] != null && row["HunterPoint"].ToString() != "")
                {
                    model.HunterPoint = int.Parse(row["HunterPoint"].ToString());
                }
                if (row["BonusPlayer"] != null)
                {
                    model.BonusPlayer = row["BonusPlayer"].ToString();
                }
                if (row["BonusMoney"] != null && row["BonusMoney"].ToString() != "")
                {
                    model.BonusMoney = int.Parse(row["BonusMoney"].ToString());
                }
                if (row["BonusTimes"] != null && row["BonusTimes"].ToString() != "")
                {
                    model.BonusTimes = DateTime.Parse(row["BonusTimes"].ToString());
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
            strSql.Append("select * ");
            strSql.Append(" FROM tb_BaccaratHistory ");
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
            strSql.Append(" HID,RoomID,RoomDoName,RoomDoTable,UsID,UsName,BetMoney,BetType,BankerPoker,BankerPoint,HunterPoker,HunterPoint,BonusPlayer,BonusMoney,BonusTimes ");
            strSql.Append(" FROM tb_BaccaratHistory ");
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
            strSql.Append("select count(1) FROM tb_BaccaratHistory ");
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
                strSql.Append("order by T.HID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_BaccaratHistory T ");
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
			strSql.Append(" FROM tb_BaccaratHistory ");
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
		/// <returns>IList BaccaratHistory</returns>
        public IList<BCW.Baccarat.Model.BaccaratHistory> GetBaccaratHistorys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratHistory> listBaccaratHistorys = new List<BCW.Baccarat.Model.BaccaratHistory>();
			string sTable = "tb_BaccaratHistory";
            string sPkey = "HID";
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
					return listBaccaratHistorys;
				}
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BaccaratHistory objBaccaratHistory = new BCW.Baccarat.Model.BaccaratHistory();
                    objBaccaratHistory.HID = reader.GetInt32(0);
                    objBaccaratHistory.RoomID = reader.GetInt32(1);
                    objBaccaratHistory.RoomDoName = reader.GetString(2);
                    objBaccaratHistory.RoomDoTable = reader.GetInt32(3);
                    objBaccaratHistory.UsID = reader.GetInt32(4);
                    objBaccaratHistory.UsName = reader.GetString(5);
                    objBaccaratHistory.BetMoney = reader.GetInt32(6);
                    objBaccaratHistory.BetType = reader.GetString(7);
                    objBaccaratHistory.BankerPoker = reader.GetString(8);
                    objBaccaratHistory.BankerPoint = reader.GetInt32(9);
                    objBaccaratHistory.HunterPoker = reader.GetString(10);
                    objBaccaratHistory.HunterPoint = reader.GetInt32(11);
                    objBaccaratHistory.BonusPlayer = reader.GetString(12);
                    objBaccaratHistory.BonusMoney = reader.GetInt32(13);
                    objBaccaratHistory.BonusTimes = reader.GetDateTime(14);
                    listBaccaratHistorys.Add(objBaccaratHistory);
                }
			}
			return listBaccaratHistorys;
		}

		#endregion  成员方法
	}
}

