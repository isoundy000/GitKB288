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
	/// 数据访问类xitonghao。
	/// </summary>
	public class xitonghao
	{
		public xitonghao()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_xitonghao"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_xitonghao");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.xitonghao model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_xitonghao(");
			strSql.Append("UsID,IP,type,AddTime,caoID)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@IP,@type,@AddTime,@caoID)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,50),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@caoID", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.IP;
			parameters[2].Value = model.type;
			parameters[3].Value = model.AddTime;
			parameters[4].Value = model.caoID;

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
		public void Update(BCW.Model.xitonghao model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_xitonghao set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("IP=@IP,");
			strSql.Append("type=@type,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("caoID=@caoID");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IP", SqlDbType.VarChar,50),
					new SqlParameter("@type", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@caoID", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.IP;
			parameters[3].Value = model.type;
			parameters[4].Value = model.AddTime;
			parameters[5].Value = model.caoID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_xitonghao ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.xitonghao Getxitonghao(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,IP,type,AddTime,caoID from tb_xitonghao ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.xitonghao model=new BCW.Model.xitonghao();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.IP = reader.GetString(2);
					model.type = reader.GetInt32(3);
					model.AddTime = reader.GetDateTime(4);
					model.caoID = reader.GetInt32(5);
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
			strSql.Append(" FROM tb_xitonghao ");
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
		/// <returns>IList xitonghao</returns>
		public IList<BCW.Model.xitonghao> Getxitonghaos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.xitonghao> listxitonghaos = new List<BCW.Model.xitonghao>();
			string sTable = "tb_xitonghao";
			string sPkey = "id";
			string sField = "ID,UsID,IP,type,AddTime,caoID";
			string sCondition = strWhere;
			string sOrder = "AddTime Desc";
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
					return listxitonghaos;
				}
				while (reader.Read())
				{
						BCW.Model.xitonghao objxitonghao = new BCW.Model.xitonghao();
						objxitonghao.ID = reader.GetInt32(0);
						objxitonghao.UsID = reader.GetInt32(1);
						objxitonghao.IP = reader.GetString(2);
						objxitonghao.type = reader.GetInt32(3);
						objxitonghao.AddTime = reader.GetDateTime(4);
						objxitonghao.caoID = reader.GetInt32(5);
						listxitonghaos.Add(objxitonghao);
				}
			}
			return listxitonghaos;
		}

		#endregion  成员方法
	}
}

