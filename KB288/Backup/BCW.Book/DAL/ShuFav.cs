using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace Book.DAL
{
	/// <summary>
	/// 数据访问类ShuFav。
	/// </summary>
	public class ShuFav
	{
		public ShuFav()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelperBook.GetMaxID("id", "ShuFav"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ShuFav");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return SqlHelperBook.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 计算某用户书架数量
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from ShuFav");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            object obj = SqlHelperBook.GetSingle(strSql.ToString(), parameters);
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
		/// 增加一条数据
		/// </summary>
		public int Add(Book.Model.ShuFav model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ShuFav(");
			strSql.Append("name,usid)");
			strSql.Append(" values (");
			strSql.Append("@name,@usid)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@usid", SqlDbType.Int,4)};
			parameters[0].Value = model.name;
			parameters[1].Value = model.usid;

			object obj = SqlHelperBook.GetSingle(strSql.ToString(),parameters);
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
		public void Update(Book.Model.ShuFav model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ShuFav set ");
			strSql.Append("name=@name,");
			strSql.Append("usid=@usid");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@usid", SqlDbType.Int,4)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.name;
			parameters[2].Value = model.usid;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ShuFav ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Book.Model.ShuFav GetShuFav(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,name,usid from ShuFav ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			Book.Model.ShuFav model=new Book.Model.ShuFav();
			using (SqlDataReader reader=SqlHelperBook.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.id = reader.GetInt32(0);
					model.name = reader.GetString(1);
					model.usid = reader.GetInt32(2);
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
			strSql.Append(" FROM ShuFav ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelperBook.Query(strSql.ToString());
		}

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList ShuFav</returns>
		public IList<Book.Model.ShuFav> GetShuFavs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<Book.Model.ShuFav> listShuFavs = new List<Book.Model.ShuFav>();
			string sTable = "ShuFav";
			string sPkey = "id";
			string sField = "id,name,usid";
			string sCondition = strWhere;
			string sOrder = "ID Desc";
			int iSCounts = 0;
			using (SqlDataReader reader = SqlHelperBook.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
			{
				//计算总页数
				if (p_recordCount > 0)
				{
					int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
				}
				else
				{
					return listShuFavs;
				}
				while (reader.Read())
				{
						Book.Model.ShuFav objShuFav = new Book.Model.ShuFav();
						objShuFav.id = reader.GetInt32(0);
						objShuFav.name = reader.GetString(1);
						objShuFav.usid = reader.GetInt32(2);
						listShuFavs.Add(objShuFav);
				}
			}
			return listShuFavs;
		}

		#endregion  成员方法
	}
}

