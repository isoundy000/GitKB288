using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.dzpk.DAL
{
	/// <summary>
	/// 数据访问类DzpkAct。
	/// </summary>
	public class DzpkAct
	{
		public DzpkAct()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkAct");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.dzpk.Model.DzpkAct model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkAct(");
			strSql.Append("RmID,UsID,ActMake,Money,MaxCard,ActTime,RmMake)");
			strSql.Append(" values (");
			strSql.Append("@RmID,@UsID,@ActMake,@Money,@MaxCard,@ActTime,@RmMake)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ActMake", SqlDbType.NVarChar,50),
					new SqlParameter("@Money", SqlDbType.BigInt,8),
					new SqlParameter("@MaxCard", SqlDbType.NVarChar,50),
					new SqlParameter("@ActTime", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.RmID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.ActMake;
			parameters[3].Value = model.Money;
			parameters[4].Value = model.MaxCard;
			parameters[5].Value = model.ActTime;
			parameters[6].Value = model.RmMake;

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
		public void Update(BCW.dzpk.Model.DzpkAct model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkAct set ");
			strSql.Append("RmID=@RmID,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("ActMake=@ActMake,");
			strSql.Append("Money=@Money,");
			strSql.Append("MaxCard=@MaxCard,");
			strSql.Append("ActTime=@ActTime,");
			strSql.Append("RmMake=@RmMake");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ActMake", SqlDbType.NVarChar,50),
					new SqlParameter("@Money", SqlDbType.BigInt,8),
					new SqlParameter("@MaxCard", SqlDbType.NVarChar,50),
					new SqlParameter("@ActTime", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.RmID;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.ActMake;
			parameters[4].Value = model.Money;
			parameters[5].Value = model.MaxCard;
			parameters[6].Value = model.ActTime;
			parameters[7].Value = model.RmMake;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkAct ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.dzpk.Model.DzpkAct GetDzpkAct(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RmID,UsID,ActMake,Money,MaxCard,ActTime,RmMake from tb_DzpkAct ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.dzpk.Model.DzpkAct model=new BCW.dzpk.Model.DzpkAct();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.RmID = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.ActMake = reader.GetString(3);
					model.Money = reader.GetInt64(4);
					model.MaxCard = reader.GetString(5);
					model.ActTime = reader.GetDateTime(6);
					model.RmMake = reader.GetString(7);
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
			strSql.Append(" FROM tb_DzpkAct ");
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
		/// <returns>IList DzpkAct</returns>
		public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkAct> listDzpkActs = new List<BCW.dzpk.Model.DzpkAct>();
			string sTable = "tb_DzpkAct";
			string sPkey = "id";
			string sField = "ID,RmID,UsID,ActMake,Money,MaxCard,ActTime,RmMake";
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
					return listDzpkActs;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkAct objDzpkAct = new BCW.dzpk.Model.DzpkAct();
						objDzpkAct.ID = reader.GetInt32(0);
						objDzpkAct.RmID = reader.GetInt32(1);
						objDzpkAct.UsID = reader.GetInt32(2);
						objDzpkAct.ActMake = reader.GetString(3);
						objDzpkAct.Money = reader.GetInt64(4);
						objDzpkAct.MaxCard = reader.GetString(5);
						objDzpkAct.ActTime = reader.GetDateTime(6);
						objDzpkAct.RmMake = reader.GetString(7);
						listDzpkActs.Add(objDzpkAct);
				}
			}
			return listDzpkActs;
		}

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList DzpkAct</returns>
        public IList<BCW.dzpk.Model.DzpkAct> GetDzpkActs(int p_pageIndex, int p_pageSize, string strWhere,string sOrder, out int p_recordCount)
        {
            IList<BCW.dzpk.Model.DzpkAct> listDzpkActs = new List<BCW.dzpk.Model.DzpkAct>();
            string sTable = "tb_DzpkAct";
            string sPkey = "id";
            string sField = "ID,RmID,UsID,ActMake,Money,MaxCard,ActTime,RmMake";
            string sCondition = strWhere;
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
                    return listDzpkActs;
                }
                while (reader.Read())
                {
                    BCW.dzpk.Model.DzpkAct objDzpkAct = new BCW.dzpk.Model.DzpkAct();
                    objDzpkAct.ID = reader.GetInt32(0);
                    objDzpkAct.RmID = reader.GetInt32(1);
                    objDzpkAct.UsID = reader.GetInt32(2);
                    objDzpkAct.ActMake = reader.GetString(3);
                    objDzpkAct.Money = reader.GetInt64(4);
                    objDzpkAct.MaxCard = reader.GetString(5);
                    objDzpkAct.ActTime = reader.GetDateTime(6);
                    objDzpkAct.RmMake = reader.GetString(7);
                    listDzpkActs.Add(objDzpkAct);
                }
            }
            return listDzpkActs;
        }

        #endregion  成员方法
    }
}

