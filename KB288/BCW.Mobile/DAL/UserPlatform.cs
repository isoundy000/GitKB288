using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;//Please add references
namespace BCW.Mobile.DAL
{
	/// <summary>
	/// 数据访问类:UserPlatform
	/// </summary>
	public partial class UserPlatform
	{
		public UserPlatform()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
            return SqlHelper.GetMaxID( "platformType", "tb_UserPlatform" ); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string platformId,int platformType)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_UserPlatform");
			strSql.Append(" where platformId=@platformId and platformType=@platformType ");
			SqlParameter[] parameters = {
					new SqlParameter("@platformId", SqlDbType.NVarChar,50),
					new SqlParameter("@platformType", SqlDbType.Int,4)			};
			parameters[0].Value = platformId;
			parameters[1].Value = platformType;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(BCW.Mobile.Model.UserPlatform model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_UserPlatform(");
			strSql.Append("platformId,platformType,userId)");
			strSql.Append(" values (");
			strSql.Append("@platformId,@platformType,@userId)");
			SqlParameter[] parameters = {
					new SqlParameter("@platformId", SqlDbType.NVarChar,50),
					new SqlParameter("@platformType", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4)};
			parameters[0].Value = model.platformId;
			parameters[1].Value = model.platformType;
			parameters[2].Value = model.userId;

            int rows = SqlHelper.ExecuteSql( strSql.ToString(), parameters );
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
		public bool Update(BCW.Mobile.Model.UserPlatform model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_UserPlatform set ");
			strSql.Append("userId=@userId");
			strSql.Append(" where platformId=@platformId and platformType=@platformType ");
			SqlParameter[] parameters = {
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@platformId", SqlDbType.NVarChar,50),
					new SqlParameter("@platformType", SqlDbType.Int,4)};
			parameters[0].Value = model.userId;
			parameters[1].Value = model.platformId;
			parameters[2].Value = model.platformType;

            int rows = SqlHelper.ExecuteSql( strSql.ToString(), parameters );
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
		public bool Delete(string platformId,int platformType)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_UserPlatform ");
			strSql.Append(" where platformId=@platformId and platformType=@platformType ");
			SqlParameter[] parameters = {
					new SqlParameter("@platformId", SqlDbType.NVarChar,50),
					new SqlParameter("@platformType", SqlDbType.Int,4)			};
			parameters[0].Value = platformId;
			parameters[1].Value = platformType;

            int rows = SqlHelper.ExecuteSql( strSql.ToString(), parameters );
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
		public BCW.Mobile.Model.UserPlatform GetModel(string platformId,int platformType)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 platformId,platformType,userId from tb_UserPlatform ");
			strSql.Append(" where platformId=@platformId and platformType=@platformType ");
			SqlParameter[] parameters = {
					new SqlParameter("@platformId", SqlDbType.NVarChar,50),
					new SqlParameter("@platformType", SqlDbType.Int,4)			};
			parameters[0].Value = platformId;
			parameters[1].Value = platformType;

			BCW.Mobile.Model.UserPlatform model=new BCW.Mobile.Model.UserPlatform();
            DataSet ds = SqlHelper.Query( strSql.ToString(), parameters );
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
		public BCW.Mobile.Model.UserPlatform DataRowToModel(DataRow row)
		{
			BCW.Mobile.Model.UserPlatform model=new BCW.Mobile.Model.UserPlatform();
			if (row != null)
			{
				if(row["platformId"]!=null)
				{
					model.platformId=row["platformId"].ToString();
				}
				if(row["platformType"]!=null && row["platformType"].ToString()!="")
				{
					model.platformType=int.Parse(row["platformType"].ToString());
				}
				if(row["userId"]!=null && row["userId"].ToString()!="")
				{
					model.userId=int.Parse(row["userId"].ToString());
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
			strSql.Append("select platformId,platformType,userId ");
			strSql.Append(" FROM tb_UserPlatform ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            return SqlHelper.Query( strSql.ToString() );
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
			strSql.Append(" platformId,platformType,userId ");
			strSql.Append(" FROM tb_UserPlatform ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query( strSql.ToString() );
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_UserPlatform ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            object obj = SqlHelper.GetSingle( strSql.ToString() );
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
				strSql.Append("order by T.platformType desc");
			}
			strSql.Append(")AS Row, T.*  from tb_UserPlatform T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query( strSql.ToString() );
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
			parameters[0].Value = "tb_UserPlatform";
			parameters[1].Value = "platformType";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return SQLHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

