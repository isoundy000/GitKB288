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
	/// 数据访问类dawnlifedaoju。
	/// </summary>
	public class dawnlifedaoju
	{
		public dawnlifedaoju()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifedaoju"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifedaoju");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Model.dawnlifedaoju model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifedaoju(");
			strSql.Append("ID,UsID,UsName,city,area,goods,price,news)");
			strSql.Append(" values (");
			strSql.Append("@ID,@UsID,@UsName,@city,@area,@goods,@price,@news)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@area", SqlDbType.NVarChar,50),
					new SqlParameter("@goods", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.Char,10),
                    new SqlParameter("@news",SqlDbType.NVarChar,255)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.city;
			parameters[4].Value = model.area;
			parameters[5].Value = model.goods;
			parameters[6].Value = model.price;
            parameters[7].Value = model.news;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.dawnlifedaoju model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifedaoju set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("city=@city,");
			strSql.Append("area=@area,");
			strSql.Append("goods=@goods,");
			strSql.Append("price=@price");
            strSql.Append("news=@news");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@area", SqlDbType.NVarChar,50),
					new SqlParameter("@goods", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.Char,10),
                                         new SqlParameter("@news",SqlDbType.NVarChar,255)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.city;
			parameters[4].Value = model.area;
			parameters[5].Value = model.goods;
			parameters[6].Value = model.price;
            parameters[7].Value = model.news;
			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifedaoju ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.dawnlifedaoju Getdawnlifedaoju(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,UsName,city,area,goods,price,news from tb_dawnlifedaoju ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifedaoju model=new BCW.Model.dawnlifedaoju();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.city = reader.GetString(3);
					model.area = reader.GetString(4);
					model.goods = reader.GetString(5);
					model.price = reader.GetString(6);
                    model.news = reader.GetString(7);
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
			strSql.Append(" FROM tb_dawnlifedaoju ");
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
		/// <returns>IList dawnlifedaoju</returns>
		public IList<BCW.Model.dawnlifedaoju> Getdawnlifedaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifedaoju> listdawnlifedaojus = new List<BCW.Model.dawnlifedaoju>();
			string sTable = "tb_dawnlifedaoju";
			string sPkey = "id";
			string sField = "ID,UsID,UsName,city,area,goods,price,news";
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
					return listdawnlifedaojus;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifedaoju objdawnlifedaoju = new BCW.Model.dawnlifedaoju();
						objdawnlifedaoju.ID = reader.GetInt32(0);
						objdawnlifedaoju.UsID = reader.GetInt32(1);
						objdawnlifedaoju.UsName = reader.GetString(2);
						objdawnlifedaoju.city = reader.GetString(3);
						objdawnlifedaoju.area = reader.GetString(4);
						objdawnlifedaoju.goods = reader.GetString(5);
						objdawnlifedaoju.price = reader.GetString(6);
                        objdawnlifedaoju.news = reader.GetString(7);
						listdawnlifedaojus.Add(objdawnlifedaoju);
				}
			}
			return listdawnlifedaojus;
		}

		#endregion  成员方法
	}
}

