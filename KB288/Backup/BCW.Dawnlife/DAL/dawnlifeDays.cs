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
	/// 数据访问类dawnlifeDays。
	/// </summary>
	public class dawnlifeDays
	{
		public dawnlifeDays()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifeDays"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifeDays");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// 根据用户ID和day查询影响的行数的ID
        /// </summary>
        /// <returns></returns>
        public int GetRowByUsID(int UsID, int day,long coin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_dawnlifeDays where ");
            strSql.Append("UsID=@UsID ");
            strSql.Append("and day=@day ");
            strSql.Append("and coin=@coin");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int),
					new SqlParameter("@day", SqlDbType.Int),
                    new SqlParameter("@coin", SqlDbType.BigInt)};
            parameters[0].Value = UsID;
            parameters[1].Value = day;
            parameters[2].Value = coin;

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
		/// 增加一条数据
		/// </summary>
        public int Add(BCW.Model.dawnlifeDays model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_dawnlifeDays(");
            strSql.Append("UsID,UsName,day,goods,price,city,area,coin,n,news)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@day,@goods,@price,@city,@area,@coin,@n,@news)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@day", SqlDbType.Int,4),
					new SqlParameter("@goods", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.NVarChar),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@area", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
                    new SqlParameter("@n", SqlDbType.Int,4),
                                        new SqlParameter("@news", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.day;
            parameters[3].Value = model.goods;
            parameters[4].Value = model.price;
            parameters[5].Value = model.city;
            parameters[6].Value = model.area;
            parameters[7].Value = model.coin;
            parameters[8].Value = model.n;
            parameters[9].Value = model.news;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        public void Update(BCW.Model.dawnlifeDays model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeDays set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("day=@day,");
            strSql.Append("goods=@goods,");
            strSql.Append("price=@price,");
            strSql.Append("city=@city,");
            strSql.Append("area=@area,");
            strSql.Append("coin=@coin");
            strSql.Append("n=@n");
            strSql.Append("news=@news");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@day", SqlDbType.Int,4),
					new SqlParameter("@goods", SqlDbType.NVarChar,50),
					new SqlParameter("@price", SqlDbType.NVarChar),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@area", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
                    new SqlParameter("@n", SqlDbType.Int,4),
                                        new SqlParameter("@news", SqlDbType.NVarChar,255)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.day;
            parameters[4].Value = model.goods;
            parameters[5].Value = model.price;
            parameters[6].Value = model.city;
            parameters[7].Value = model.area;
            parameters[8].Value = model.coin;
            parameters[9].Value = model.n;
            parameters[10].Value = model.news;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifeDays ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        //更新一个字段
        public void Updategoods(int ID, string goods)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeDays set ");
            strSql.Append("goods=@goods ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@goods", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = goods;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        //更新一个字段
        public void Updateprice(int ID, string price)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeDays set ");
            strSql.Append("price=@price ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@price", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = price;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.dawnlifeDays GetdawnlifeDays(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,day,goods,price,city,area,coin,n,news from tb_dawnlifeDays ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.dawnlifeDays model = new BCW.Model.dawnlifeDays();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.day = reader.GetInt32(3);
                    model.goods = reader.GetString(4);
                    model.price = reader.GetString(5);
                    model.city = reader.GetString(6);
                    model.area = reader.GetString(7);
                    model.coin = reader.GetInt64(8);
                    model.n = reader.GetInt32(9);
                    model.news = reader.GetString(10);
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
			strSql.Append(" FROM tb_dawnlifeDays ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}



        /// 根据用户ID和酷币查询影响的行数的ID
        /// </summary>
        /// <returns></returns>
        public int GetDayByUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Top(1)* from tb_dawnlifeDays where UsID=@UsID order by coin desc,day desc ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int)};
            parameters[0].Value = UsID;


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
		/// <returns>IList dawnlifeDays</returns>
        public IList<BCW.Model.dawnlifeDays> GetdawnlifeDayss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.dawnlifeDays> listdawnlifeDayss = new List<BCW.Model.dawnlifeDays>();
            string sTable = "tb_dawnlifeDays";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,day,goods,price,city,area,coin,n,news";
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
                    return listdawnlifeDayss;
                }
                while (reader.Read())
                {
                    BCW.Model.dawnlifeDays objdawnlifeDays = new BCW.Model.dawnlifeDays();
                    objdawnlifeDays.ID = reader.GetInt32(0);
                    objdawnlifeDays.UsID = reader.GetInt32(1);
                    objdawnlifeDays.UsName = reader.GetString(2);
                    objdawnlifeDays.day = reader.GetInt32(3);
                    objdawnlifeDays.goods = reader.GetString(4);
                    objdawnlifeDays.price = reader.GetString(5);
                    objdawnlifeDays.city = reader.GetString(6);
                    objdawnlifeDays.area = reader.GetString(7);
                    objdawnlifeDays.coin = reader.GetInt64(8);
                    objdawnlifeDays.n = reader.GetInt32(9);
                    objdawnlifeDays.news = reader.GetString(10);
                    listdawnlifeDayss.Add(objdawnlifeDays);
                }
            }
            return listdawnlifeDayss;
        }

		#endregion  成员方法
	}
}

