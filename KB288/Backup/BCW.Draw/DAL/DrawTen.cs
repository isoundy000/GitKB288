using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Draw.DAL
{
	/// <summary>
	/// 数据访问类DrawTen。
	/// </summary>
	public class DrawTen
	{
		public DrawTen()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_DrawTen"); 
		}



        /// <summary>
        /// 得到用
        /// </summary>
        public int GetCounts(int Rank)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsCounts from tb_DrawTen ");
            strSql.Append(" where Rank=@Rank ");
            SqlParameter[] parameters = {
					new SqlParameter("@Rank", SqlDbType.Int,4)};
            parameters[0].Value = Rank;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 得到用
        /// </summary>
        public int GetRank(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rank from tb_DrawTen ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DrawTen");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Draw.Model.DrawTen model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DrawTen(");
			strSql.Append("ID,GoodsCounts,Rank)");
			strSql.Append(" values (");
			strSql.Append("@ID,@GoodsCounts,@Rank)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Rank", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GoodsCounts;
			parameters[2].Value = model.Rank;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawTen model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DrawTen set ");
			strSql.Append("GoodsCounts=@GoodsCounts,");
			strSql.Append("Rank=@Rank");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Rank", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GoodsCounts;
			parameters[2].Value = model.Rank;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


        /// <summary>
        /// 根据rank更新编号
        /// </summary>
        public void UpdateGoodsCounts(int rank,int GoodsCounts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update  tb_DrawTen set ");
            strSql.Append(" GoodsCounts=@GoodsCounts ");
            strSql.Append(" where rank=@rank ");
            SqlParameter[] parameters = {
					new SqlParameter("@rank", SqlDbType.Int,4),
                    new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = rank;
            parameters[1].Value = GoodsCounts;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DrawTen ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.DrawTen GetDrawTen(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GoodsCounts,Rank from tb_DrawTen ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.DrawTen model=new BCW.Draw.Model.DrawTen();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.GoodsCounts = reader.GetInt32(1);
					model.Rank = reader.GetInt32(2);
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
			strSql.Append(" FROM tb_DrawTen ");
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
		/// <returns>IList DrawTen</returns>
		public IList<BCW.Draw.Model.DrawTen> GetDrawTens(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Draw.Model.DrawTen> listDrawTens = new List<BCW.Draw.Model.DrawTen>();
			string sTable = "tb_DrawTen";
			string sPkey = "id";
			string sField = "ID,GoodsCounts,Rank";
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
					return listDrawTens;
				}
				while (reader.Read())
				{
						BCW.Draw.Model.DrawTen objDrawTen = new BCW.Draw.Model.DrawTen();
						objDrawTen.ID = reader.GetInt32(0);
						objDrawTen.GoodsCounts = reader.GetInt32(1);
						objDrawTen.Rank = reader.GetInt32(2);
						listDrawTens.Add(objDrawTen);
				}
			}
			return listDrawTens;
		}

		#endregion  成员方法
	}
}

