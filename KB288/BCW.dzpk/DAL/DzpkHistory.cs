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
	/// 数据访问类DzpkHistory。
	/// </summary>
	public class DzpkHistory
	{
		public DzpkHistory()
		{ }
        #region  成员方法
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_DzpkHistory");
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkHistory");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.dzpk.Model.DzpkHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkHistory(");
			strSql.Append("RmID,UsID,RankChk,PokerCards,PokerChips,TimeOut,Winner,RmMake,GetMoney,IsPayOut)");
			strSql.Append(" values (");
			strSql.Append("@RmID,@UsID,@RankChk,@PokerCards,@PokerChips,@TimeOut,@Winner,@RmMake,@GetMoney,@IsPayOut)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@RankChk", SqlDbType.Int,4),
					new SqlParameter("@PokerCards", SqlDbType.NVarChar,50),
					new SqlParameter("@PokerChips", SqlDbType.NVarChar,50),
					new SqlParameter("@TimeOut", SqlDbType.DateTime),
					new SqlParameter("@Winner", SqlDbType.NVarChar,1),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@IsPayOut", SqlDbType.Int,4)};
			parameters[0].Value = model.RmID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.RankChk;
			parameters[3].Value = model.PokerCards;
			parameters[4].Value = model.PokerChips;
			parameters[5].Value = model.TimeOut;
			parameters[6].Value = model.Winner;
			parameters[7].Value = model.RmMake;
			parameters[8].Value = model.GetMoney;
			parameters[9].Value = model.IsPayOut;

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
		public void Update(BCW.dzpk.Model.DzpkHistory model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkHistory set ");
			strSql.Append("RmID=@RmID,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("RankChk=@RankChk,");
			strSql.Append("PokerCards=@PokerCards,");
			strSql.Append("PokerChips=@PokerChips,");
			strSql.Append("TimeOut=@TimeOut,");
			strSql.Append("Winner=@Winner,");
			strSql.Append("RmMake=@RmMake,");
			strSql.Append("GetMoney=@GetMoney,");
			strSql.Append("IsPayOut=@IsPayOut");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@RankChk", SqlDbType.Int,4),
					new SqlParameter("@PokerCards", SqlDbType.NVarChar,50),
					new SqlParameter("@PokerChips", SqlDbType.NVarChar,50),
					new SqlParameter("@TimeOut", SqlDbType.DateTime),
					new SqlParameter("@Winner", SqlDbType.NVarChar,1),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@IsPayOut", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.RmID;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.RankChk;
			parameters[4].Value = model.PokerCards;
			parameters[5].Value = model.PokerChips;
			parameters[6].Value = model.TimeOut;
			parameters[7].Value = model.Winner;
			parameters[8].Value = model.RmMake;
			parameters[9].Value = model.GetMoney;
			parameters[10].Value = model.IsPayOut;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkHistory ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.dzpk.Model.DzpkHistory GetDzpkHistory(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RmID,UsID,RankChk,PokerCards,PokerChips,TimeOut,Winner,RmMake,GetMoney,IsPayOut from tb_DzpkHistory ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.dzpk.Model.DzpkHistory model=new BCW.dzpk.Model.DzpkHistory();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.RmID = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.RankChk = reader.GetInt32(3);
					model.PokerCards = reader.GetString(4);
					model.PokerChips = reader.GetString(5);
					model.TimeOut = reader.GetDateTime(6);
					model.Winner = reader.GetString(7);
					model.RmMake = reader.GetString(8);
					model.GetMoney = reader.GetInt64(9);
					model.IsPayOut = reader.GetInt32(10);
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
			strSql.Append(" FROM tb_DzpkHistory ");
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
		/// <returns>IList DzpkHistory</returns>
		public IList<BCW.dzpk.Model.DzpkHistory> GetDzpkHistorys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkHistory> listDzpkHistorys = new List<BCW.dzpk.Model.DzpkHistory>();
			string sTable = "tb_DzpkHistory";
			string sPkey = "id";
			string sField = "ID,RmID,UsID,RankChk,PokerCards,PokerChips,TimeOut,Winner,RmMake,GetMoney,IsPayOut";
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
					return listDzpkHistorys;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkHistory objDzpkHistory = new BCW.dzpk.Model.DzpkHistory();
						objDzpkHistory.ID = reader.GetInt32(0);
						objDzpkHistory.RmID = reader.GetInt32(1);
						objDzpkHistory.UsID = reader.GetInt32(2);
						objDzpkHistory.RankChk = reader.GetInt32(3);
						objDzpkHistory.PokerCards = reader.GetString(4);
						objDzpkHistory.PokerChips = reader.GetString(5);
						objDzpkHistory.TimeOut = reader.GetDateTime(6);
						objDzpkHistory.Winner = reader.GetString(7);
						objDzpkHistory.RmMake = reader.GetString(8);
						objDzpkHistory.GetMoney = reader.GetInt64(9);
						objDzpkHistory.IsPayOut = reader.GetInt32(10);
						listDzpkHistorys.Add(objDzpkHistory);
				}
			}
			return listDzpkHistorys;
		}

		#endregion  成员方法
	}
}

