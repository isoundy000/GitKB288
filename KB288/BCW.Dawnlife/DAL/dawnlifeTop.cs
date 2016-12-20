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
	/// 数据访问类dawnlifeTop。
	/// </summary>
	public class dawnlifeTop
	{
		public dawnlifeTop()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifeTop"); 
		}


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifeTop");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existscoin(long coin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_dawnlifeTop");
            strSql.Append(" where coin=@coin ");
            SqlParameter[] parameters = {
					new SqlParameter("@coin", SqlDbType.Int,4)};
            parameters[0].Value = coin;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.dawnlifeTop model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifeTop(");
			strSql.Append("UsID,date,city,UsName,coin,money,sum,cum)");
			strSql.Append(" values (");
            strSql.Append("@UsID,@date,@city,@UsName,@coin,@money,@sum,@cum)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@date", SqlDbType.DateTime),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@sum", SqlDbType.Int,4),
                                        new SqlParameter("@cum", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.date;
			parameters[2].Value = model.city;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.coin;
			parameters[5].Value = model.money;
			parameters[6].Value = model.sum;
            parameters[7].Value = model.cum;

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
		public void Update(BCW.Model.dawnlifeTop model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifeTop set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("date=@date,");
			strSql.Append("city=@city,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("coin=@coin,");
			strSql.Append("money=@money,");
			strSql.Append("sum=@sum");
            strSql.Append("cum=@cum");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@date", SqlDbType.DateTime),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@sum", SqlDbType.Int,4),
                                        	new SqlParameter("@cum", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.date;
			parameters[3].Value = model.city;
			parameters[4].Value = model.UsName;
			parameters[5].Value = model.coin;
			parameters[6].Value = model.money;
			parameters[7].Value = model.sum;
            parameters[8].Value = model.cum;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


        public void Updatesum(int ID, int sum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeTop set ");
            strSql.Append("sum=@sum ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@sum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = sum;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void Updatecum(int ID, int cum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeTop set ");
            strSql.Append("cum=@cum ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@cum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = cum;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifeTop ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.dawnlifeTop GetdawnlifeTop(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,date,city,UsName,coin,money,sum,cum from tb_dawnlifeTop ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifeTop model=new BCW.Model.dawnlifeTop();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.date = reader.GetDateTime(2);
					model.city = reader.GetString(3);
					model.UsName = reader.GetString(4);
					model.coin = reader.GetInt64(5);
					model.money = reader.GetInt64(6);
					model.sum = reader.GetInt32(7);
                    model.cum = reader.GetInt32(8);
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
            strSql.Append("select " +strField + " ");
			strSql.Append(" FROM tb_dawnlifeTop ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}


        /// 根据用户ID和coin查询影响的行数的ID
        /// </summary>
        /// <returns></returns>
        public int GetRowByUsID(int UsID, long coin,long money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_dawnlifeTop where ");
            strSql.Append("UsID=@UsID ");
            strSql.Append("and coin=@coin ");
            strSql.Append("and money=@money");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int),
					new SqlParameter("@coin", SqlDbType.BigInt),
                    new SqlParameter("@money", SqlDbType.BigInt)};
            parameters[0].Value = UsID;
            parameters[1].Value = coin;
            parameters[2].Value = money;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList dawnlifeTop</returns>
		public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifeTop> listdawnlifeTops = new List<BCW.Model.dawnlifeTop>();
			string sTable = "tb_dawnlifeTop";
			string sPkey = "id";
			string sField = "ID,UsID,date,city,UsName,coin,money,sum,cum";
			string sCondition = strWhere;
			string sOrder = strOrder;
			int iSCounts =0;
			using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
			{
				//计算总页数
				if (p_recordCount > 0)
				{
					int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
				}
				else
				{
					return listdawnlifeTops;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifeTop objdawnlifeTop = new BCW.Model.dawnlifeTop();
						objdawnlifeTop.ID = reader.GetInt32(0);
						objdawnlifeTop.UsID = reader.GetInt32(1);
						objdawnlifeTop.date = reader.GetDateTime(2);
						objdawnlifeTop.city = reader.GetString(3);
						objdawnlifeTop.UsName = reader.GetString(4);
						objdawnlifeTop.coin = reader.GetInt64(5);
						objdawnlifeTop.money = reader.GetInt64(6);
						objdawnlifeTop.sum = reader.GetInt32(7);
                        objdawnlifeTop.cum = reader.GetInt32(8);
						listdawnlifeTops.Add(objdawnlifeTop);
				}
			}
			return listdawnlifeTops;
		}
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList dawnlifeTop</returns>
        public IList<BCW.Model.dawnlifeTop> GetdawnlifeTops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder,int iSCounts, out int p_recordCount)
        {
            IList<BCW.Model.dawnlifeTop> listdawnlifeTops = new List<BCW.Model.dawnlifeTop>();
            string sTable = "tb_dawnlifeTop";
            string sPkey = "id";
            string sField = "ID,UsID,date,city,UsName,coin,money,sum,cum";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts1 = iSCounts;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts1, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listdawnlifeTops;
                }
                while (reader.Read())
                {
                    BCW.Model.dawnlifeTop objdawnlifeTop = new BCW.Model.dawnlifeTop();
                    objdawnlifeTop.ID = reader.GetInt32(0);
                    objdawnlifeTop.UsID = reader.GetInt32(1);
                    objdawnlifeTop.date = reader.GetDateTime(2);
                    objdawnlifeTop.city = reader.GetString(3);
                    objdawnlifeTop.UsName = reader.GetString(4);
                    objdawnlifeTop.coin = reader.GetInt64(5);
                    objdawnlifeTop.money = reader.GetInt64(6);
                    objdawnlifeTop.sum = reader.GetInt32(7);
                    objdawnlifeTop.cum = reader.GetInt32(8);
                    listdawnlifeTops.Add(objdawnlifeTop);
                }
            }
            return listdawnlifeTops;
        }
		#endregion  成员方法
	}
}

