using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
	/// <summary>
	/// 数据访问类flowzz。
	/// </summary>
	public class flowzz
	{
		public flowzz()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_flowzz"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_flowzz");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.flowzz model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_flowzz(");
			strSql.Append("Title,cNum,Price,Leven)");
			strSql.Append(" values (");
			strSql.Append("@Title,@cNum,@Price,@Leven)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@cNum", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Leven", SqlDbType.Int,4)};
			parameters[0].Value = model.Title;
			parameters[1].Value = model.cNum;
			parameters[2].Value = model.Price;
			parameters[3].Value = model.Leven;

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
		public void Update(BCW.Model.Game.flowzz model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_flowzz set ");
			strSql.Append("Title=@Title,");
			strSql.Append("cNum=@cNum,");
			strSql.Append("Price=@Price,");
			strSql.Append("Leven=@Leven");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@cNum", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Leven", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.cNum;
			parameters[3].Value = model.Price;
			parameters[4].Value = model.Leven;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_flowzz ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.flowzz Getflowzz(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Title,cNum,Price,Leven from tb_flowzz ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.flowzz model=new BCW.Model.Game.flowzz();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Title = reader.GetString(1);
					model.cNum = reader.GetInt32(2);
					model.Price = reader.GetInt32(3);
					model.Leven = reader.GetInt32(4);
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
			strSql.Append(" FROM tb_flowzz ");
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
		/// <returns>IList flowzz</returns>
		public IList<BCW.Model.Game.flowzz> Getflowzzs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.flowzz> listflowzzs = new List<BCW.Model.Game.flowzz>();
			string sTable = "tb_flowzz";
			string sPkey = "id";
			string sField = "ID,Title,cNum,Price,Leven";
			string sCondition = strWhere;
			string sOrder = "ID asc";
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
					return listflowzzs;
				}
				while (reader.Read())
				{
						BCW.Model.Game.flowzz objflowzz = new BCW.Model.Game.flowzz();
						objflowzz.ID = reader.GetInt32(0);
						objflowzz.Title = reader.GetString(1);
						objflowzz.cNum = reader.GetInt32(2);
						objflowzz.Price = reader.GetInt32(3);
						objflowzz.Leven = reader.GetInt32(4);
						listflowzzs.Add(objflowzz);
				}
			}
			return listflowzzs;
		}

		#endregion  成员方法
	}
}

