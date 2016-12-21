using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.bydr.DAL
{
	/// <summary>
	/// 数据访问类Cmg_buyuDonation。
	/// </summary>
	public class Cmg_buyuDonation
	{
		public Cmg_buyuDonation()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Cmg_buyuDonation");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperUser.GetMaxID("ID", "tb_Cmg_buyuDonation");
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) from tb_Cmg_buyuDonation");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.bydr.Model.Cmg_buyuDonation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Cmg_buyuDonation(");
			strSql.Append("usID,time,Ctime,Donation,stype)");
			strSql.Append(" values (");
			strSql.Append("@usID,@time,@Ctime,@Donation,@stype)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@time", SqlDbType.DateTime),
					new SqlParameter("@Ctime", SqlDbType.DateTime),
					new SqlParameter("@Donation", SqlDbType.BigInt,8),
					new SqlParameter("@stype", SqlDbType.Int,4)};
			parameters[0].Value = model.usID;
			parameters[1].Value = model.time;
			parameters[2].Value = model.Ctime;
			parameters[3].Value = model.Donation;
			parameters[4].Value = model.stype;

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
		public void Update(BCW.bydr.Model.Cmg_buyuDonation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Cmg_buyuDonation set ");
			strSql.Append("usID=@usID,");
			strSql.Append("time=@time,");
			strSql.Append("Ctime=@Ctime,");
			strSql.Append("Donation=@Donation,");
			strSql.Append("stype=@stype");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@time", SqlDbType.DateTime),
					new SqlParameter("@Ctime", SqlDbType.DateTime),
					new SqlParameter("@Donation", SqlDbType.BigInt,8),
					new SqlParameter("@stype", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.usID;
			parameters[2].Value = model.time;
			parameters[3].Value = model.Ctime;
			parameters[4].Value = model.Donation;
			parameters[5].Value = model.stype;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Cmg_buyuDonation ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.bydr.Model.Cmg_buyuDonation GetCmg_buyuDonation(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,usID,time,Ctime,Donation,stype from tb_Cmg_buyuDonation ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.bydr.Model.Cmg_buyuDonation model=new BCW.bydr.Model.Cmg_buyuDonation();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.usID = reader.GetInt32(1);
					model.time = reader.GetDateTime(2);
					model.Ctime = reader.GetDateTime(3);
					model.Donation = reader.GetInt64(4);
					model.stype = reader.GetInt32(5);
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
			strSql.Append(" FROM tb_Cmg_buyuDonation ");
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
		/// <returns>IList Cmg_buyuDonation</returns>
		public IList<BCW.bydr.Model.Cmg_buyuDonation> GetCmg_buyuDonations(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.bydr.Model.Cmg_buyuDonation> listCmg_buyuDonations = new List<BCW.bydr.Model.Cmg_buyuDonation>();
			string sTable = "tb_Cmg_buyuDonation";
			string sPkey = "id";
			string sField = "ID,usID,time,Ctime,Donation,stype";
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
					return listCmg_buyuDonations;
				}
				while (reader.Read())
				{
						BCW.bydr.Model.Cmg_buyuDonation objCmg_buyuDonation = new BCW.bydr.Model.Cmg_buyuDonation();
						objCmg_buyuDonation.ID = reader.GetInt32(0);
						objCmg_buyuDonation.usID = reader.GetInt32(1);
						objCmg_buyuDonation.time = reader.GetDateTime(2);
						objCmg_buyuDonation.Ctime = reader.GetDateTime(3);
						objCmg_buyuDonation.Donation = reader.GetInt64(4);
						objCmg_buyuDonation.stype = reader.GetInt32(5);
						listCmg_buyuDonations.Add(objCmg_buyuDonation);
				}
			}
			return listCmg_buyuDonations;
		}

		#endregion  成员方法
	}
}

