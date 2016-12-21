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
	/// 数据访问类DzpkCard。
	/// </summary>
	public class DzpkCard
	{
		public DzpkCard()
		{}
		#region  成员方法

        public void DeleteByRmID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_DzpkCard");
            strSql.Append(" where RmID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkCard");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.dzpk.Model.DzpkCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkCard(");
			strSql.Append("ID,RmID,PokerSuit,PokerRank)");
			strSql.Append(" values (");
			strSql.Append("@ID,@RmID,@PokerSuit,@PokerRank)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@PokerSuit", SqlDbType.Int,4),
					new SqlParameter("@PokerRank", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
			parameters[1].Value = model.RmID;
			parameters[2].Value = model.PokerSuit;
			parameters[3].Value = model.PokerRank;

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
		public void Update(BCW.dzpk.Model.DzpkCard model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkCard set ");
			strSql.Append("RmID=@RmID,");
			strSql.Append("PokerSuit=@PokerSuit,");
			strSql.Append("PokerRank=@PokerRank");
			strSql.Append(" where ID=@ID AND RmID=@RmID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@PokerSuit", SqlDbType.Int,4),
					new SqlParameter("@PokerRank", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.RmID;
			parameters[2].Value = model.PokerSuit;
			parameters[3].Value = model.PokerRank;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID,int RmID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkCard ");
            strSql.Append(" where ID=@ID AND RmID=@RmID  ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@RmID", SqlDbType.Int,4)};
			parameters[0].Value = ID;
            parameters[1].Value = RmID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.dzpk.Model.DzpkCard GetDzpkCard(int ID,int RmID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RmID,PokerSuit,PokerRank from tb_DzpkCard ");
			strSql.Append(" where ID=@ID AND RmID=@RmID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@RmID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = RmID;

			BCW.dzpk.Model.DzpkCard model=new BCW.dzpk.Model.DzpkCard();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.RmID = reader.GetInt32(1);
					model.PokerSuit = reader.GetInt32(2);
					model.PokerRank = reader.GetInt32(3);
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
			strSql.Append(" FROM tb_DzpkCard ");
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
		/// <returns>IList DzpkCard</returns>
		public IList<BCW.dzpk.Model.DzpkCard> GetDzpkCards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkCard> listDzpkCards = new List<BCW.dzpk.Model.DzpkCard>();
			string sTable = "tb_DzpkCard";
			string sPkey = "id";
			string sField = "ID,RmID,PokerSuit,PokerRank";
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
					return listDzpkCards;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkCard objDzpkCard = new BCW.dzpk.Model.DzpkCard();
						objDzpkCard.ID = reader.GetInt32(0);
						objDzpkCard.RmID = reader.GetInt32(1);
						objDzpkCard.PokerSuit = reader.GetInt32(2);
						objDzpkCard.PokerRank = reader.GetInt32(3);
						listDzpkCards.Add(objDzpkCard);
				}
			}
			return listDzpkCards;
		}

		#endregion  成员方法
	}
}

