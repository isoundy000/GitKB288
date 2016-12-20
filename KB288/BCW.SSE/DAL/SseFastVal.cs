using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;//Please add references
namespace BCW.SSE.DAL
{
	/// <summary>
	/// 数据访问类:SseFastVal
	/// </summary>
	public partial class SseFastVal
	{
		public SseFastVal()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("userId", "tb_SseFastVal"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int userId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_SseFastVal");
			strSql.Append(" where userId=@userId ");
			SqlParameter[] parameters = {
					new SqlParameter("@userId", SqlDbType.Int,4)			};
			parameters[0].Value = userId;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(BCW.SSE.Model.SseFastVal model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SseFastVal(");
			strSql.Append("userId,fastVal)");
			strSql.Append(" values (");
			strSql.Append("@userId,@fastVal)");
			SqlParameter[] parameters = {
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@fastVal", SqlDbType.NVarChar,500)};
			parameters[0].Value = model.userId;
			parameters[1].Value = model.fastVal;

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
		public bool Update(BCW.SSE.Model.SseFastVal model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SseFastVal set ");
			strSql.Append("fastVal=@fastVal");
			strSql.Append(" where userId=@userId ");
			SqlParameter[] parameters = {
					new SqlParameter("@fastVal", SqlDbType.NVarChar,500),
					new SqlParameter("@userId", SqlDbType.Int,4)};
			parameters[0].Value = model.fastVal;
			parameters[1].Value = model.userId;

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
		public bool Delete(int userId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SseFastVal ");
			strSql.Append(" where userId=@userId ");
			SqlParameter[] parameters = {
					new SqlParameter("@userId", SqlDbType.Int,4)			};
			parameters[0].Value = userId;

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
		public bool DeleteList(string userIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SseFastVal ");
			strSql.Append(" where userId in ("+userIdlist + ")  ");
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
		public BCW.SSE.Model.SseFastVal GetModel(int userId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 userId,fastVal from tb_SseFastVal ");
			strSql.Append(" where userId=@userId ");
			SqlParameter[] parameters = {
					new SqlParameter("@userId", SqlDbType.Int,4)			};
			parameters[0].Value = userId;

			BCW.SSE.Model.SseFastVal model=new BCW.SSE.Model.SseFastVal();
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
		public BCW.SSE.Model.SseFastVal DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SseFastVal model=new BCW.SSE.Model.SseFastVal();
			if (row != null)
			{
				if(row["userId"]!=null && row["userId"].ToString()!="")
				{
					model.userId=int.Parse(row["userId"].ToString());
				}
				if(row["fastVal"]!=null)
				{
					model.fastVal=row["fastVal"].ToString();
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
			strSql.Append("select userId,fastVal ");
			strSql.Append(" FROM tb_SseFastVal ");
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
			strSql.Append(" userId,fastVal ");
			strSql.Append(" FROM tb_SseFastVal ");
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
			strSql.Append("select count(1) FROM tb_SseFastVal ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = BCW.Data.SqlHelper.GetSingle(strSql.ToString());
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
				strSql.Append("order by T.userId desc");
			}
			strSql.Append(")AS Row, T.*  from tb_SseFastVal T ");
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
			parameters[0].Value = "tb_SseFastVal";
			parameters[1].Value = "userId";
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

