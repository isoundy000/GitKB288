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
	/// 数据访问类dawnlifenotes。
	/// </summary>
	public class dawnlifenotes
	{
		public dawnlifenotes()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifenotes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifenotes");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.dawnlifenotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifenotes(");
			strSql.Append("coin,UsID,city,area,money,debt,buy,sell,price,num,date,day)");
			strSql.Append(" values (");
			strSql.Append("@coin,@UsID,@city,@area,@money,@debt,@buy,@sell,@price,@num,@date,@day)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@coin", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@city", SqlDbType.Int,4),
					new SqlParameter("@area", SqlDbType.Int,4),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@debt", SqlDbType.BigInt,8),
					new SqlParameter("@buy", SqlDbType.NVarChar,50),
					new SqlParameter("@sell", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.BigInt,8),
					new SqlParameter("@num", SqlDbType.BigInt,8),
					new SqlParameter("@date", SqlDbType.DateTime),
                                        new SqlParameter("@day", SqlDbType.Int,4)};
			parameters[0].Value = model.coin;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.city;
			parameters[3].Value = model.area;
			parameters[4].Value = model.money;
			parameters[5].Value = model.debt;
			parameters[6].Value = model.buy;
			parameters[7].Value = model.sell;
			parameters[8].Value = model.price;
			parameters[9].Value = model.num;
			parameters[10].Value = model.date;
            parameters[11].Value = model.day;

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
		public void Update(BCW.Model.dawnlifenotes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifenotes set ");
			strSql.Append("coin=@coin,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("city=@city,");
			strSql.Append("area=@area,");
			strSql.Append("money=@money,");
			strSql.Append("debt=@debt,");
			strSql.Append("buy=@buy,");
			strSql.Append("sell=@sell,");
			strSql.Append("price=@price,");
			strSql.Append("num=@num,");
			strSql.Append("date=@date,");
            strSql.Append("day=@day");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@coin", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@city", SqlDbType.Int,4),
					new SqlParameter("@area", SqlDbType.Int,4),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@debt", SqlDbType.BigInt,8),
					new SqlParameter("@buy", SqlDbType.NVarChar,50),
					new SqlParameter("@sell", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.BigInt,8),
					new SqlParameter("@num", SqlDbType.BigInt,8),
					new SqlParameter("@date", SqlDbType.DateTime),
                                        new SqlParameter("@day", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.coin;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.city;
			parameters[4].Value = model.area;
			parameters[5].Value = model.money;
			parameters[6].Value = model.debt;
			parameters[7].Value = model.buy;
			parameters[8].Value = model.sell;
			parameters[9].Value = model.price;
			parameters[10].Value = model.num;
			parameters[11].Value = model.date;
            parameters[12].Value = model.day;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifenotes ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.dawnlifenotes Getdawnlifenotes(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,coin,UsID,city,area,money,debt,buy,sell,price,num,date,day from tb_dawnlifenotes ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifenotes model=new BCW.Model.dawnlifenotes();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.coin = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.city = reader.GetInt32(3);
					model.area = reader.GetInt32(4);
					model.money = reader.GetInt64(5);
					model.debt = reader.GetInt64(6);
					model.buy = reader.GetString(7);
					model.sell = reader.GetString(8);
					model.price = reader.GetInt64(9);
					model.num = reader.GetInt64(10);
					model.date = reader.GetDateTime(11);
                    model.day = reader.GetInt32(12);
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
			strSql.Append(" FROM tb_dawnlifenotes ");
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
		/// <returns>IList dawnlifenotes</returns>
		public IList<BCW.Model.dawnlifenotes> Getdawnlifenotess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifenotes> listdawnlifenotess = new List<BCW.Model.dawnlifenotes>();
			string sTable = "tb_dawnlifenotes";
			string sPkey = "id";
			string sField = "ID,coin,UsID,city,area,money,debt,buy,sell,price,num,date,day";
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
					return listdawnlifenotess;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifenotes objdawnlifenotes = new BCW.Model.dawnlifenotes();
						objdawnlifenotes.ID = reader.GetInt32(0);
						objdawnlifenotes.coin = reader.GetInt32(1);
						objdawnlifenotes.UsID = reader.GetInt32(2);
						objdawnlifenotes.city = reader.GetInt32(3);
						objdawnlifenotes.area = reader.GetInt32(4);
						objdawnlifenotes.money = reader.GetInt64(5);
						objdawnlifenotes.debt = reader.GetInt64(6);
						objdawnlifenotes.buy = reader.GetString(7);
						objdawnlifenotes.sell = reader.GetString(8);
						objdawnlifenotes.price = reader.GetInt64(9);
						objdawnlifenotes.num = reader.GetInt64(10);
						objdawnlifenotes.date = reader.GetDateTime(11);
                        objdawnlifenotes.day = reader.GetInt32(12);
						listdawnlifenotess.Add(objdawnlifenotes);
				}
			}
			return listdawnlifenotess;
		}


        public IList<BCW.Model.dawnlifenotes> Getdawnlifenotess2(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.dawnlifenotes> listdawnlifenotess = new List<BCW.Model.dawnlifenotes>();
            string sTable = "tb_dawnlifenotes";
            string sPkey = "id";
            string sField = "ID,coin,UsID,city,area,money,debt,buy,sell,price,num,date,day";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listdawnlifenotess;
                }
                while (reader.Read())
                {
                    BCW.Model.dawnlifenotes objdawnlifenotes = new BCW.Model.dawnlifenotes();
                    objdawnlifenotes.ID = reader.GetInt32(0);
                    objdawnlifenotes.coin = reader.GetInt32(1);
                    objdawnlifenotes.UsID = reader.GetInt32(2);
                    objdawnlifenotes.city = reader.GetInt32(3);
                    objdawnlifenotes.area = reader.GetInt32(4);
                    objdawnlifenotes.money = reader.GetInt64(5);
                    objdawnlifenotes.debt = reader.GetInt64(6);
                    objdawnlifenotes.buy = reader.GetString(7);
                    objdawnlifenotes.sell = reader.GetString(8);
                    objdawnlifenotes.price = reader.GetInt64(9);
                    objdawnlifenotes.num = reader.GetInt64(10);
                    objdawnlifenotes.date = reader.GetDateTime(11);
                    objdawnlifenotes.day = reader.GetInt32(12);
                    listdawnlifenotess.Add(objdawnlifenotes);
                }
            }
            return listdawnlifenotess;
        }


		#endregion  成员方法
	}
}

