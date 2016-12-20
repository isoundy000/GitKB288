using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
	/// <summary>
	/// 数据访问类HcPay。
	/// </summary>
	public class HcPay
	{
		public HcPay()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("id", "tb_HcPay"); 
		}
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_HcPay");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HcPay");
            strSql.Append(" where id=@id ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 每ID每期下注币数
        /// </summary>
        public long GetPayCent(int UsID, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_HcPay");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and CID=@CID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = CID;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.HcPay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_HcPay(");
            strSql.Append("Types,CID,UsID,UsName,Vote,PayCent,PayCents,Result,State,WinCent,BzType,AddTime,IsSpier)");
			strSql.Append(" values (");
            strSql.Append("@Types,@CID,@UsID,@UsName,@Vote,@PayCent,@PayCents,@Result,@State,@WinCent,@BzType,@AddTime,@IsSpier)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Vote", SqlDbType.NVarChar,200),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@PayCents", SqlDbType.BigInt,8),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsSpier", SqlDbType.TinyInt,1)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.CID;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.Vote;
			parameters[5].Value = model.PayCent;
            parameters[6].Value = model.PayCents;
			parameters[7].Value = model.Result;
			parameters[8].Value = model.State;
			parameters[9].Value = model.WinCent;
			parameters[10].Value = model.BzType;
			parameters[11].Value = model.AddTime;
			parameters[12].Value = model.IsSpier;

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
		public void Update(BCW.Model.Game.HcPay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_HcPay set ");
			strSql.Append("Types=@Types,");
			strSql.Append("CID=@CID,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("Vote=@Vote,");
			strSql.Append("PayCent=@PayCent,");
			strSql.Append("Result=@Result,");
			strSql.Append("State=@State,");
			strSql.Append("WinCent=@WinCent,");
			strSql.Append("BzType=@BzType,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("IsSpier=@IsSpier");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Vote", SqlDbType.NVarChar,200),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsSpier", SqlDbType.TinyInt,1)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.CID;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.UsName;
			parameters[5].Value = model.Vote;
			parameters[6].Value = model.PayCent;
			parameters[7].Value = model.Result;
			parameters[8].Value = model.State;
			parameters[9].Value = model.WinCent;
			parameters[10].Value = model.BzType;
			parameters[11].Value = model.AddTime;
			parameters[12].Value = model.IsSpier;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HcPay set ");
            strSql.Append("State=@State");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
				new SqlParameter("@id", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_HcPay ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HcPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_HcPay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent1(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_HcPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }          
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// 得到一个GetPayCent1
        /// </summary>
        public long GetPayCent1(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(PayCents) from tb_HcPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.HcPay GetHcPay(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,Types,CID,UsID,UsName,Vote,PayCent,Result,State,WinCent,BzType,AddTime,IsSpier from tb_HcPay ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			BCW.Model.Game.HcPay model=new BCW.Model.Game.HcPay();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.id = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.CID = reader.GetInt32(2);
					model.UsID = reader.GetInt32(3);
					model.UsName = reader.GetString(4);
					model.Vote = reader.GetString(5);
					model.PayCent = reader.GetInt64(6);
					model.Result = reader.GetString(7);
					model.State = reader.GetByte(8);
					model.WinCent = reader.GetInt64(9);
					model.BzType = reader.GetByte(10);
					model.AddTime = reader.GetDateTime(11);
					model.IsSpier = reader.GetByte(12);
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
			strSql.Append(" FROM tb_HcPay ");
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
		/// <returns>IList HcPay</returns>
		public IList<BCW.Model.Game.HcPay> GetHcPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.HcPay> listHcPays = new List<BCW.Model.Game.HcPay>();
			string sTable = "tb_HcPay";
			string sPkey = "id";
			string sField = "id,Types,CID,UsID,UsName,Vote,PayCent,Result,State,WinCent,BzType,AddTime,IsSpier";
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
					return listHcPays;
				}
				while (reader.Read())
				{
						BCW.Model.Game.HcPay objHcPay = new BCW.Model.Game.HcPay();
						objHcPay.id = reader.GetInt32(0);
						objHcPay.Types = reader.GetInt32(1);
						objHcPay.CID = reader.GetInt32(2);
						objHcPay.UsID = reader.GetInt32(3);
						objHcPay.UsName = reader.GetString(4);
						objHcPay.Vote = reader.GetString(5);
						objHcPay.PayCent = reader.GetInt64(6);
						objHcPay.Result = reader.GetString(7);
						objHcPay.State = reader.GetByte(8);
						objHcPay.WinCent = reader.GetInt64(9);
						objHcPay.BzType = reader.GetByte(10);
						objHcPay.AddTime = reader.GetDateTime(11);
						objHcPay.IsSpier = reader.GetByte(12);
						listHcPays.Add(objHcPay);
				}
			}
			return listHcPays;
		}


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Model.Game.HcPay> GetHcPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.HcPay> listHcPayTop = new List<BCW.Model.Game.HcPay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_HcPay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 50)
                p_recordCount = 50;

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listHcPayTop;
            }

            // 取出相关记录
            string queryString = "";
            queryString = "SELECT Top 50 UsID,sum(WinCent-PayCents) as WinCents FROM tb_HcPay where " + strWhere + " group by UsID Order by sum(WinCent-PayCents) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.HcPay objHcPay = new BCW.Model.Game.HcPay();
                        objHcPay.UsID = reader.GetInt32(0);
                        objHcPay.WinCent = reader.GetInt64(1);
                        listHcPayTop.Add(objHcPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listHcPayTop;
        }


		#endregion  成员方法
	}
}

