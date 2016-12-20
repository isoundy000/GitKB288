using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;//Please add references
namespace BCW.SSE.DAL
{
	/// <summary>
	/// 数据访问类:SsePrizePool
	/// </summary>
	public partial class SsePrizePool
	{
		public SsePrizePool()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.SSE.Model.SsePrizePool model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SsePrizePool(");
			strSql.Append("prizeType,sseNo,totalPrizeMoney,bz)");
			strSql.Append(" values (");
			strSql.Append("@prizeType,@sseNo,@totalPrizeMoney,@bz)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@prizeType", SqlDbType.TinyInt,1),
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@totalPrizeMoney", SqlDbType.Money,8),
					new SqlParameter("@bz", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.prizeType;
			parameters[1].Value = model.sseNo;
			parameters[2].Value = model.totalPrizeMoney;
			parameters[3].Value = model.bz;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(BCW.SSE.Model.SsePrizePool model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SsePrizePool set ");
			strSql.Append("prizeType=@prizeType,");
			strSql.Append("sseNo=@sseNo,");
			strSql.Append("totalPrizeMoney=@totalPrizeMoney,");
			strSql.Append("bz=@bz");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@prizeType", SqlDbType.TinyInt,1),
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@totalPrizeMoney", SqlDbType.Money,8),
					new SqlParameter("@bz", SqlDbType.NVarChar,500),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.prizeType;
			parameters[1].Value = model.sseNo;
			parameters[2].Value = model.totalPrizeMoney;
			parameters[3].Value = model.bz;
			parameters[4].Value = model.id;

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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SsePrizePool ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

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
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SsePrizePool ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=SqlHelper.ExecuteSql(strSql.ToString());
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
		public BCW.SSE.Model.SsePrizePool GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,prizeType,sseNo,totalPrizeMoney,bz from tb_SsePrizePool ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.SSE.Model.SsePrizePool model=new BCW.SSE.Model.SsePrizePool();
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
		public BCW.SSE.Model.SsePrizePool DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SsePrizePool model=new BCW.SSE.Model.SsePrizePool();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["prizeType"]!=null && row["prizeType"].ToString()!="")
				{
					model.prizeType=int.Parse(row["prizeType"].ToString());
				}
				if(row["sseNo"]!=null && row["sseNo"].ToString()!="")
				{
					model.sseNo=int.Parse(row["sseNo"].ToString());
				}
				if(row["totalPrizeMoney"]!=null && row["totalPrizeMoney"].ToString()!="")
				{
					model.totalPrizeMoney=decimal.Parse(row["totalPrizeMoney"].ToString());
				}
				if(row["bz"]!=null)
				{
					model.bz=row["bz"].ToString();
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,prizeType,sseNo,totalPrizeMoney,bz ");
			strSql.Append(" FROM tb_SsePrizePool ");
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
			strSql.Append(" id,prizeType,sseNo,totalPrizeMoney,bz ");
			strSql.Append(" FROM tb_SsePrizePool ");
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
			strSql.Append("select count(1) FROM tb_SsePrizePool ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            object obj = BCW.Data.SqlHelper.GetSingle( strSql.ToString() );
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
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from tb_SsePrizePool T ");
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
			parameters[0].Value = "tb_SsePrizePool";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return SqlHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

