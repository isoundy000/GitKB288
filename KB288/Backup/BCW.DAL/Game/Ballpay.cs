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
	/// 数据访问类Ballpay。
	/// </summary>
	public class Ballpay
	{
		public Ballpay()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Ballpay"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Ballpay");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Ballpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在应数字购买记录
        /// </summary>
        public bool ExistsBuyNum(int BallId, int BuyNum, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Ballpay");
            strSql.Append(" where BallId=@BallId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and BuyNum=@BuyNum ");
            SqlParameter[] parameters = {
					new SqlParameter("@BallId", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@BuyNum", SqlDbType.Int,4)};
            parameters[0].Value = BallId;
            parameters[1].Value = UsID;
            parameters[2].Value = BuyNum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int BallId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Ballpay");
            strSql.Append(" where BallId=@BallId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@BallId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = BallId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某期某数字的购买数量
        /// </summary>
        public int GetCount(int BallId, int BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCount) from tb_Ballpay");
            strSql.Append(" where BallId=@BallId ");
            strSql.Append(" and BuyNum=@BuyNum ");
            SqlParameter[] parameters = {
					new SqlParameter("@BallId", SqlDbType.Int,4),
					new SqlParameter("@BuyNum", SqlDbType.Int,4)};
            parameters[0].Value = BallId;
            parameters[1].Value = BuyNum;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
		public int Add(BCW.Model.Game.Ballpay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Ballpay(");
			strSql.Append("BallId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@BallId,@UsID,@UsName,@BuyNum,@BuyCount,@BuyCent,@WinCent,@State,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@BallId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyNum", SqlDbType.Int,4),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.BallId;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.BuyNum;
			parameters[4].Value = model.BuyCount;
			parameters[5].Value = model.BuyCent;
			parameters[6].Value = model.WinCent;
			parameters[7].Value = model.State;
			parameters[8].Value = model.AddTime;

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
		public void Update(BCW.Model.Game.Ballpay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Ballpay set ");
            strSql.Append("BuyCount=BuyCount+@BuyCount,");
            strSql.Append("BuyCent=BuyCent+@BuyCent,");
			strSql.Append("AddTime=@AddTime");
            strSql.Append(" where BallId=@BallId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and BuyNum=@BuyNum");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BallId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BuyNum", SqlDbType.Int,4),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.BallId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.BuyNum;
			parameters[4].Value = model.BuyCount;
			parameters[5].Value = model.BuyCent;
			parameters[6].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Ballpay set ");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;
            parameters[2].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Ballpay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Ballpay ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Ballpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个BuyCount
        /// </summary>
        public int GetBuyCount(int BallId, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Sum(BuyCount) from tb_Ballpay ");
            strSql.Append(" where BallId=@BallId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@BallId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = BallId;
            parameters[1].Value = UsID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt32(0);
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
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_Ballpay ");
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
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Ballpay GetBallpay(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,BallId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,AddTime from tb_Ballpay ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.Ballpay model=new BCW.Model.Game.Ballpay();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.BallId = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.UsName = reader.GetString(3);
					model.BuyNum = reader.GetInt32(4);
					model.BuyCount = reader.GetInt32(5);
					model.BuyCent = reader.GetInt64(6);
					model.WinCent = reader.GetInt64(7);
					model.State = reader.GetByte(8);
					model.AddTime = reader.GetDateTime(9);
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
			strSql.Append(" FROM tb_Ballpay ");
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
		/// <returns>IList Ballpay</returns>
		public IList<BCW.Model.Game.Ballpay> GetBallpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.Ballpay> listBallpays = new List<BCW.Model.Game.Ballpay>();
			string sTable = "tb_Ballpay";
			string sPkey = "id";
			string sField = "ID,BallId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,AddTime";
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
					return listBallpays;
				}
				while (reader.Read())
				{
						BCW.Model.Game.Ballpay objBallpay = new BCW.Model.Game.Ballpay();
						objBallpay.ID = reader.GetInt32(0);
						objBallpay.BallId = reader.GetInt32(1);
						objBallpay.UsID = reader.GetInt32(2);
						objBallpay.UsName = reader.GetString(3);
						objBallpay.BuyNum = reader.GetInt32(4);
						objBallpay.BuyCount = reader.GetInt32(5);
						objBallpay.BuyCent = reader.GetInt64(6);
						objBallpay.WinCent = reader.GetInt64(7);
						objBallpay.State = reader.GetByte(8);
						objBallpay.AddTime = reader.GetDateTime(9);
						listBallpays.Add(objBallpay);
				}
			}
			return listBallpays;
		}

		#endregion  成员方法
	}
}

