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
	/// 数据访问类dawnlifeDo。
	/// </summary>
	public class dawnlifeDo
	{
		public dawnlifeDo()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifeDo"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifeDo");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.dawnlifeDo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifeDo(");
			strSql.Append("UsID,UsName,sum,stock,stocky,goods,price,dsg,coin)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@UsName,@sum,@stock,@stocky,@goods,@price,@dsg,@coin)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@sum", SqlDbType.Int,4),
					new SqlParameter("@stock", SqlDbType.Int,4),
					new SqlParameter("@stocky", SqlDbType.Int,4),
					new SqlParameter("@goods", SqlDbType.NVarChar),
					new SqlParameter("@price", SqlDbType.NVarChar),
					new SqlParameter("@dsg", SqlDbType.NVarChar),
                    new SqlParameter("@coin", SqlDbType.BigInt)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.sum;
			parameters[3].Value = model.stock;
			parameters[4].Value = model.stocky;
			parameters[5].Value = model.goods;
			parameters[6].Value = model.price;
			parameters[7].Value = model.dsg;
            parameters[8].Value = model.coin;
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
		public void Update(BCW.Model.dawnlifeDo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifeDo set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("sum=@sum,");
			strSql.Append("stock=@stock,");
			strSql.Append("stocky=@stocky,");
			strSql.Append("goods=@goods,");
			strSql.Append("price=@price,");
			strSql.Append("dsg=@dsg,");
            strSql.Append("coin=@coin");
            //strSql.Append("n=@n");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@sum", SqlDbType.Int,4),
					new SqlParameter("@stock", SqlDbType.Int,4),
					new SqlParameter("@stocky", SqlDbType.Int,4),
					new SqlParameter("@goods", SqlDbType.NVarChar),
					new SqlParameter("@price", SqlDbType.NVarChar),
					new SqlParameter("@dsg", SqlDbType.NVarChar),
                    new SqlParameter("@coin", SqlDbType.BigInt )};
                    //new SqlParameter("@n", SqlDbType.Int,4)
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.sum;
			parameters[4].Value = model.stock;
			parameters[5].Value = model.stocky;
			parameters[6].Value = model.goods;
			parameters[7].Value = model.price;
			parameters[8].Value = model.dsg;
            parameters[9].Value = model.coin;
            //parameters[10].Value = model.n;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifeDo ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// 根据用户ID和酷币查询影响的行数的ID
        /// </summary>
        /// <returns></returns>
        public int GetRowByUsID(int UsID, long coin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_dawnlifeDo where ");
            strSql.Append("UsID=@UsID ");
            strSql.Append("and coin=@coin");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int),
					new SqlParameter("@coin", SqlDbType.BigInt)};
            parameters[0].Value = UsID;
            parameters[1].Value = coin;

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


        public int GetByUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Top(1)* from tb_dawnlifeDo where UsID=@UsID order by coin desc ");
            //strSql.Append("UsID=@UsID ");
            //strSql.Append("and coin=@coin");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int),
					};
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
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.dawnlifeDo GetdawnlifeDo(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,UsName,sum,stock,stocky,goods,price,dsg,coin from tb_dawnlifeDo ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifeDo model=new BCW.Model.dawnlifeDo();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.sum = reader.GetInt32(3);
					model.stock = reader.GetInt32(4);
					model.stocky = reader.GetInt32(5);
					model.goods = reader.GetString(6);
					model.price = reader.GetString(7);
					model.dsg = reader.GetString(8);
                    model.coin = reader.GetInt64(9);
                    //model.n = reader.GetInt32(10);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

        public void UpdateStock(int ID, int stock)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeDo set ");
            strSql.Append("stock=@stock ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@stock", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = stock;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_dawnlifeDo ");
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
		/// <returns>IList dawnlifeDo</returns>
		public IList<BCW.Model.dawnlifeDo> GetdawnlifeDos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifeDo> listdawnlifeDos = new List<BCW.Model.dawnlifeDo>();
			string sTable = "tb_dawnlifeDo";
			string sPkey = "id";
			string sField = "ID,UsID,UsName,sum,stock,stocky,goods,price,dsg,coin";
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
					return listdawnlifeDos;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifeDo objdawnlifeDo = new BCW.Model.dawnlifeDo();
						objdawnlifeDo.ID = reader.GetInt32(0);
						objdawnlifeDo.UsID = reader.GetInt32(1);
						objdawnlifeDo.UsName = reader.GetString(2);
						objdawnlifeDo.sum = reader.GetInt32(3);
						objdawnlifeDo.stock = reader.GetInt32(4);
						objdawnlifeDo.stocky = reader.GetInt32(5);
						objdawnlifeDo.goods = reader.GetString(6);
						objdawnlifeDo.price = reader.GetString(7);
						objdawnlifeDo.dsg = reader.GetString(8);
                        objdawnlifeDo.coin = reader.GetInt64(9);
                        //objdawnlifeDo.n = reader.GetInt32(10);
						listdawnlifeDos.Add(objdawnlifeDo);
				}
			}
			return listdawnlifeDos;
		}

		#endregion  成员方法
	}
}

