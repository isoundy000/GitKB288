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
	/// 数据访问类BaccaratCard。
	/// </summary>
	public class BaccaratCard
	{
		public BaccaratCard()
		{}
		#region  成员方法

        public void DeleteByRoomDoTable(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratCard");
            strSql.Append(" where RoomDoTable=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

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
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BaccaratCard");
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_BaccaratCard");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)			};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 判断是否存在某房间的桌面的扑克牌
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BaccaratCard");
            strSql.Append(" where RoomID=@RoomID and RoomDoTable=@RoomDoTable");
            SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(BCW.Baccarat.Model.BaccaratCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_BaccaratCard(");
			strSql.Append("RoomID,RoomDoName,RoomDoTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint)");
			strSql.Append(" values (");
			strSql.Append("@RoomID,@RoomDoName,@RoomDoTable,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint)");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoint", SqlDbType.Int,4),
					new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
					new SqlParameter("@HunterPoint", SqlDbType.Int,4)};
			parameters[0].Value = model.RoomID;
			parameters[1].Value = model.RoomDoName;
			parameters[2].Value = model.RoomDoTable;
			parameters[3].Value = model.BankerPoker;
			parameters[4].Value = model.BankerPoint;
			parameters[5].Value = model.HunterPoker;
			parameters[6].Value = model.HunterPoint;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
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
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.Baccarat.Model.BaccaratCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_BaccaratCard set ");
			strSql.Append("RoomID=@RoomID,");
			strSql.Append("RoomDoName=@RoomDoName,");
			strSql.Append("RoomDoTable=@RoomDoTable,");
			strSql.Append("BankerPoker=@BankerPoker,");
			strSql.Append("BankerPoint=@BankerPoint,");
			strSql.Append("HunterPoker=@HunterPoker,");
			strSql.Append("HunterPoint=@HunterPoint");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoint", SqlDbType.Int,4),
					new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
					new SqlParameter("@HunterPoint", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.RoomID;
			parameters[1].Value = model.RoomDoName;
			parameters[2].Value = model.RoomDoTable;
			parameters[3].Value = model.BankerPoker;
			parameters[4].Value = model.BankerPoint;
			parameters[5].Value = model.HunterPoker;
			parameters[6].Value = model.HunterPoint;
			parameters[7].Value = model.ID;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
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
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_BaccaratCard ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)			};
			parameters[0].Value = ID;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public BCW.Baccarat.Model.BaccaratCard GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RoomID,RoomDoName,RoomDoTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint from tb_BaccaratCard ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)			};
			parameters[0].Value = ID;

			BCW.Baccarat.Model.BaccaratCard model=new BCW.Baccarat.Model.BaccaratCard();
			DataSet ds=SqlHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
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
		public BCW.Baccarat.Model.BaccaratCard DataRowToModel(DataRow row)
		{
			BCW.Baccarat.Model.BaccaratCard model=new BCW.Baccarat.Model.BaccaratCard();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["RoomID"]!=null && row["RoomID"].ToString()!="")
				{
					model.RoomID=int.Parse(row["RoomID"].ToString());
				}
				if(row["RoomDoName"]!=null)
				{
					model.RoomDoName=row["RoomDoName"].ToString();
				}
				if(row["RoomDoTable"]!=null && row["RoomDoTable"].ToString()!="")
				{
					model.RoomDoTable=int.Parse(row["RoomDoTable"].ToString());
				}
				if(row["BankerPoker"]!=null)
				{
					model.BankerPoker=row["BankerPoker"].ToString();
				}
				if(row["BankerPoint"]!=null && row["BankerPoint"].ToString()!="")
				{
					model.BankerPoint=int.Parse(row["BankerPoint"].ToString());
				}
				if(row["HunterPoker"]!=null)
				{
					model.HunterPoker=row["HunterPoker"].ToString();
				}
				if(row["HunterPoint"]!=null && row["HunterPoint"].ToString()!="")
				{
					model.HunterPoint=int.Parse(row["HunterPoint"].ToString());
				}
			}
			return model;
		}

        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BaccaratCard GetCardMessage(int RoomID, int RoomDoTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BaccaratCard ");
            strSql.Append("where RoomID=@RoomID and RoomDoTable=@RoomDoTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomDoTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomDoTable;
            BCW.Baccarat.Model.BaccaratCard model = new BCW.Baccarat.Model.BaccaratCard();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomDoName = reader.GetString(2);
                    model.RoomDoTable = reader.GetInt32(3);
                    model.BankerPoker = reader.GetString(4);
                    model.BankerPoint = reader.GetInt32(5);
                    model.HunterPoker = reader.GetString(6);
                    model.HunterPoint = reader.GetInt32(7);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * ");
			strSql.Append(" FROM tb_BaccaratCard ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" ID,RoomID,RoomDoName,RoomDoTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint ");
			strSql.Append(" FROM tb_BaccaratCard ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_BaccaratCard ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from tb_BaccaratCard T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return SqlHelper.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_BaccaratCard";
			parameters[1].Value = "ID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Card</returns>
        public IList<BCW.Baccarat.Model.BaccaratCard> GetCards(int SizeNum, string strWhere)
        {
            IList<BCW.Baccarat.Model.BaccaratCard> listCards = new List<BCW.Baccarat.Model.BaccaratCard>();
            string sTable = "tb_BaccaratCard";
            string sPkey = "id";
            string sField = "ID,RoomID,RoomDoTable,BankerPoint,HunterPoint";
            string sCondition = strWhere;
            string sOrder = "RoomDoTable desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listCards;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BaccaratCard card= new BCW.Baccarat.Model.BaccaratCard();
                    card.ID = reader.GetInt32(0);
                    card.RoomID = reader.GetInt32(1);
                    card.RoomDoTable = reader.GetInt32(2);
                    card.BankerPoint = reader.GetInt32(3);
                    card.HunterPoint = reader.GetInt32(4);

                    listCards.Add(card);
                }
            }
            return listCards;
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList BaccaratCard</returns>
		public IList<BCW.Baccarat.Model.BaccaratCard> GetBaccaratCards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Baccarat.Model.BaccaratCard> listBaccaratCards = new List<BCW.Baccarat.Model.BaccaratCard>();
			string sTable = "tb_BaccaratCard";
			string sPkey = "id";
			string sField = "ID,RoomID,RoomDoName,RoomDoTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint";
			string sCondition = strWhere;
            string sOrder = "RoomDoTable Desc";
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
					return listBaccaratCards;
				}
				while (reader.Read())
				{
						BCW.Baccarat.Model.BaccaratCard objBaccaratCard = new BCW.Baccarat.Model.BaccaratCard();
						objBaccaratCard.ID = reader.GetInt32(0);
						objBaccaratCard.RoomID = reader.GetInt32(1);
						objBaccaratCard.RoomDoName = reader.GetString(2);
						objBaccaratCard.RoomDoTable = reader.GetInt32(3);
                        objBaccaratCard.BankerPoker = reader.GetString(4);
                        objBaccaratCard.BankerPoint = reader.GetInt32(5);
                        objBaccaratCard.HunterPoker = reader.GetString(6);
                        objBaccaratCard.HunterPoint = reader.GetInt32(7);
						listBaccaratCards.Add(objBaccaratCard);
				}
			}
			return listBaccaratCards;
		}

		#endregion  成员方法
	}
}

