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
	/// 数据访问类Shoptg。
	/// </summary>
	public class Shoptg
	{
		public Shoptg()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Shoptg"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Shoptg");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Shoptg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Shoptg(");
			strSql.Append("ZrID,UsID,UsName,Notes,DetailId,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@ZrID,@UsID,@UsName,@Notes,@DetailId,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ZrID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ZrID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.Notes;
			parameters[4].Value = model.DetailId;
			parameters[5].Value = model.AddTime;

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
		public void Update(BCW.Model.Shoptg model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Shoptg set ");
			strSql.Append("ZrID=@ZrID,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("Notes=@Notes,");
			strSql.Append("DetailId=@DetailId,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ZrID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@DetailId", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.ZrID;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.Notes;
			parameters[5].Value = model.DetailId;
			parameters[6].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Shoptg ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Shoptg GetShoptg(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,ZrID,UsID,UsName,Notes,DetailId,AddTime from tb_Shoptg ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Shoptg model=new BCW.Model.Shoptg();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.ZrID = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.UsName = reader.GetString(3);
					model.Notes = reader.GetString(4);
					model.DetailId = reader.GetInt32(5);
					model.AddTime = reader.GetDateTime(6);
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
			strSql.Append(" FROM tb_Shoptg ");
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
		/// <returns>IList Shoptg</returns>
		public IList<BCW.Model.Shoptg> GetShoptgs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Shoptg> listShoptgs = new List<BCW.Model.Shoptg>();
			string sTable = "tb_Shoptg";
			string sPkey = "id";
			string sField = "ID,ZrID,UsID,UsName,Notes,DetailId,AddTime";
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
					return listShoptgs;
				}
				while (reader.Read())
				{
						BCW.Model.Shoptg objShoptg = new BCW.Model.Shoptg();
						objShoptg.ID = reader.GetInt32(0);
						objShoptg.ZrID = reader.GetInt32(1);
						objShoptg.UsID = reader.GetInt32(2);
						objShoptg.UsName = reader.GetString(3);
						objShoptg.Notes = reader.GetString(4);
						objShoptg.DetailId = reader.GetInt32(5);
						objShoptg.AddTime = reader.GetDateTime(6);
						listShoptgs.Add(objShoptg);
				}
			}
			return listShoptgs;
		}

		#endregion  成员方法
	}
}

