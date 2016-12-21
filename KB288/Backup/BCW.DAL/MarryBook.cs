using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
	/// <summary>
	/// 数据访问类MarryBook。
	/// </summary>
	public class MarryBook
	{
		public MarryBook()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_MarryBook"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_MarryBook");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 计算某花园留言数量
        /// </summary>
        public int GetCount(int MarryId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_MarryBook");
            strSql.Append(" where MarryId=@MarryId ");
            SqlParameter[] parameters = {
					new SqlParameter("@MarryId", SqlDbType.Int,4)};
            parameters[0].Value = MarryId;

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
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.MarryBook model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_MarryBook(");
			strSql.Append("MarryId,ReID,ReName,Content,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@MarryId,@ReID,@ReName,@Content,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.MarryId;
			parameters[1].Value = model.ReID;
			parameters[2].Value = model.ReName;
			parameters[3].Value = model.Content;
			parameters[4].Value = model.AddTime;

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
		public void Update(BCW.Model.MarryBook model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_MarryBook set ");
			strSql.Append("MarryId=@MarryId,");
			strSql.Append("ReID=@ReID,");
			strSql.Append("ReName=@ReName,");
			strSql.Append("Content=@Content,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@ReName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.MarryId;
			parameters[2].Value = model.ReID;
			parameters[3].Value = model.ReName;
			parameters[4].Value = model.Content;
			parameters[5].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_MarryBook ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.MarryBook GetMarryBook(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,MarryId,ReID,ReName,Content,AddTime from tb_MarryBook ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.MarryBook model=new BCW.Model.MarryBook();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.MarryId = reader.GetInt32(1);
					model.ReID = reader.GetInt32(2);
					model.ReName = reader.GetString(3);
					model.Content = reader.GetString(4);
					model.AddTime = reader.GetDateTime(5);
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
			strSql.Append(" FROM tb_MarryBook ");
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
		/// <returns>IList MarryBook</returns>
		public IList<BCW.Model.MarryBook> GetMarryBooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.MarryBook> listMarryBooks = new List<BCW.Model.MarryBook>();
			string sTable = "tb_MarryBook";
			string sPkey = "id";
			string sField = "ID,MarryId,ReID,ReName,Content,AddTime";
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
					return listMarryBooks;
				}
				while (reader.Read())
				{
						BCW.Model.MarryBook objMarryBook = new BCW.Model.MarryBook();
						objMarryBook.ID = reader.GetInt32(0);
						objMarryBook.MarryId = reader.GetInt32(1);
						objMarryBook.ReID = reader.GetInt32(2);
						objMarryBook.ReName = reader.GetString(3);
						objMarryBook.Content = reader.GetString(4);
						objMarryBook.AddTime = reader.GetDateTime(5);
						listMarryBooks.Add(objMarryBook);
				}
			}
			return listMarryBooks;
		}

		#endregion  成员方法
	}
}

