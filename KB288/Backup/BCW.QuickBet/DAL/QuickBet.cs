using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.QuickBet.DAL
{
	/// <summary>
	/// 数据访问类QuickBet。
	/// </summary>
	public class QuickBet
	{
		public QuickBet()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_QuickBet"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuickBet");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该用户ID
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_QuickBet");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.QuickBet.Model.QuickBet model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuickBet(");
			strSql.Append("UsID,Game,Bet)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@Game,@Bet)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Game", SqlDbType.NVarChar),
					new SqlParameter("@Bet", SqlDbType.NVarChar)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.Game;
			parameters[2].Value = model.Bet;

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
		public void Update(BCW.QuickBet.Model.QuickBet model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuickBet set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Game=@Game,");
			strSql.Append("Bet=@Bet");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Game", SqlDbType.NVarChar),
					new SqlParameter("@Bet", SqlDbType.NVarChar)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.Game;
			parameters[3].Value = model.Bet;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新Game
        /// </summary>
        public void UpdateGame(int UsID,string Game)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuickBet set ");
            strSql.Append("Game=@Game");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Game", SqlDbType.NVarChar)};
            parameters[0].Value = UsID;
            parameters[1].Value = Game;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新Bet
        /// </summary>
        public void UpdateBet(int UsID, string Bet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_QuickBet set ");
            strSql.Append("Bet=@Bet");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Bet", SqlDbType.NVarChar)};
            parameters[0].Value = UsID;
            parameters[1].Value = Bet;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuickBet ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 根据UsID得到Game
        /// </summary>
        public string GetGame(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Game  from tb_QuickBet ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString( obj);
            }
        }

        /// <summary>
        /// 根据UsID得到Game
        /// </summary>
        public string GetBet(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Bet  from tb_QuickBet ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.QuickBet.Model.QuickBet GetQuickBet(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,Game,Bet from tb_QuickBet ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.QuickBet.Model.QuickBet model=new BCW.QuickBet.Model.QuickBet();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.Game = reader.GetString(2);
					model.Bet = reader.GetString(3);
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
			strSql.Append(" FROM tb_QuickBet ");
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
		/// <returns>IList QuickBet</returns>
		public IList<BCW.QuickBet.Model.QuickBet> GetQuickBets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.QuickBet.Model.QuickBet> listQuickBets = new List<BCW.QuickBet.Model.QuickBet>();
			string sTable = "tb_QuickBet";
			string sPkey = "id";
			string sField = "ID,UsID,Game,Bet";
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
					return listQuickBets;
				}
				while (reader.Read())
				{
						BCW.QuickBet.Model.QuickBet objQuickBet = new BCW.QuickBet.Model.QuickBet();
						objQuickBet.ID = reader.GetInt32(0);
						objQuickBet.UsID = reader.GetInt32(1);
						objQuickBet.Game = reader.GetString(2);
						objQuickBet.Bet = reader.GetString(3);
						listQuickBets.Add(objQuickBet);
				}
			}
			return listQuickBets;
		}

		#endregion  成员方法
	}
}

