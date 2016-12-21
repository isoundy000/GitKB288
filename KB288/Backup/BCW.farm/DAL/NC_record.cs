using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
	/// <summary>
	/// 数据访问类NC_record。
	/// </summary>
	public class NC_record
	{
		public NC_record()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_NC_record"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_NC_record");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.farm.Model.NC_record model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_NC_record(");
			strSql.Append("UsID,text,IP,AddTime,Browser)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@text,@IP,@AddTime,@Browser)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@text", SqlDbType.NVarChar),
					new SqlParameter("@IP", SqlDbType.NVarChar,100),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Browser", SqlDbType.NVarChar)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.text;
			parameters[2].Value = model.IP;
			parameters[3].Value = model.AddTime;
			parameters[4].Value = model.Browser;

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
		public void Update(BCW.farm.Model.NC_record model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_NC_record set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("text=@text,");
			strSql.Append("IP=@IP,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("Browser=@Browser");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@text", SqlDbType.NVarChar),
					new SqlParameter("@IP", SqlDbType.NVarChar,100),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Browser", SqlDbType.NVarChar)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.text;
			parameters[3].Value = model.IP;
			parameters[4].Value = model.AddTime;
			parameters[5].Value = model.Browser;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_NC_record ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.farm.Model.NC_record GetNC_record(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,text,IP,AddTime,Browser from tb_NC_record ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.farm.Model.NC_record model=new BCW.farm.Model.NC_record();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.text = reader.GetString(2);
					model.IP = reader.GetString(3);
					model.AddTime = reader.GetDateTime(4);
					model.Browser = reader.GetString(5);
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
			strSql.Append(" FROM tb_NC_record ");
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
		/// <returns>IList NC_record</returns>
		public IList<BCW.farm.Model.NC_record> GetNC_records(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.farm.Model.NC_record> listNC_records = new List<BCW.farm.Model.NC_record>();
			string sTable = "tb_NC_record";
			string sPkey = "id";
			string sField = "ID,UsID,text,IP,AddTime,Browser";
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
					return listNC_records;
				}
				while (reader.Read())
				{
						BCW.farm.Model.NC_record objNC_record = new BCW.farm.Model.NC_record();
						objNC_record.ID = reader.GetInt32(0);
						objNC_record.UsID = reader.GetInt32(1);
						objNC_record.text = reader.GetString(2);
						objNC_record.IP = reader.GetString(3);
						objNC_record.AddTime = reader.GetDateTime(4);
						objNC_record.Browser = reader.GetString(5);
						listNC_records.Add(objNC_record);
				}
			}
			return listNC_records;
		}

		#endregion  成员方法
	}
}

