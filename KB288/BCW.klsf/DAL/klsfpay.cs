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
	/// 数据访问类klsfpay。
	/// </summary>
	public class klsfpay
	{
		public klsfpay()
		{}
		#region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_klsfpay");
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_klsfpay");
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
            strSql.Append("select count(1) from tb_klsfpay");
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
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Prices) from tb_klsfpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 根据条件计算赢取币值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_klsfpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 获得状态
        /// </summary>
        /// <returns></returns>
        public int getState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_klsfpay where ID=" + ID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};

            parameters[0].Value = 0;
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
		public int Add(BCW.Model.klsfpay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_klsfpay(");
            strSql.Append("Types,klsfId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,iWin,isRoBot,Odds)");
			strSql.Append(" values (");
            strSql.Append("@Types,@klsfId,@UsID,@UsName,@Price,@iCount,@Notes,@Result,@Prices,@WinCent,@State,@AddTime,@iWin,@isRoBot,@Odds)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@iWin", SqlDbType.TinyInt,1),
                    new SqlParameter("@isRoBot", SqlDbType.Int,4),
                             new SqlParameter("@Odds", SqlDbType.Decimal,9)
                                        };
			parameters[0].Value = model.Types;
			parameters[1].Value = model.klsfId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.Price;
			parameters[5].Value = model.iCount;
			parameters[6].Value = model.Notes;
			parameters[7].Value = model.Result;
			parameters[8].Value = model.Prices;
			parameters[9].Value = model.WinCent;
			parameters[10].Value = model.State;
			parameters[11].Value = model.AddTime;
            parameters[12].Value = model.iWin;
            parameters[13].Value = model.isRoBot;
            parameters[14].Value = model.Odds;

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
		public void Update(BCW.Model.klsfpay model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_klsfpay set ");
			strSql.Append("Types=@Types,");
			strSql.Append("klsfId=@klsfId,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("Price=@Price,");
			strSql.Append("iCount=@iCount,");
			strSql.Append("Notes=@Notes,");
			strSql.Append("Result=@Result,");
			strSql.Append("Prices=@Prices,");
			strSql.Append("WinCent=@WinCent,");
			strSql.Append("State=@State,");
			strSql.Append("AddTime=@AddTime,");
            strSql.Append("isRoBot=@isRoBot,");
            strSql.Append("Odds=@Odds");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                                        new SqlParameter("@isRoBot", SqlDbType.Int,4),
                                           new SqlParameter("@Odds", SqlDbType.Decimal,9)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.klsfId;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.UsName;
			parameters[5].Value = model.Price;
			parameters[6].Value = model.iCount;
			parameters[7].Value = model.Notes;
			parameters[8].Value = model.Result;
			parameters[9].Value = model.Prices;
			parameters[10].Value = model.WinCent;
			parameters[11].Value = model.State;
			parameters[12].Value = model.AddTime;
            parameters[13].Value = model.isRoBot;
            parameters[14].Value = model.Odds;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_klsfpay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID and State<>2");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖数据
        /// </summary>
        public void UpdateResult(string klsfId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_klsfpay set ");
            strSql.Append("Result=@Result");
            strSql.Append(" where klsfId=@klsfId and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@klsfId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = klsfId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新游戏开奖得币
        /// </summary>
        public void UpdateWinCent(int ID, long WinCent, string WinNotes)
        {
            string oldNotes = GetWinNotes(ID) + "#";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_klsfpay set ");
            strSql.Append("WinCent=WinCent+@WinCent,");
            strSql.Append("WinNotes=@WinNotes");
            strSql.Append(" where ID=@ID and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinNotes", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;
            parameters[2].Value = oldNotes + WinNotes;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_klsfpay ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到一个WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinNotes from tb_klsfpay ");
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
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int klsfId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_klsfpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and klsfId=@klsfId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@klsfId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = klsfId;
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
        ///某期某种投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes(int Types, int klsfId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_klsfpay ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and klsfId=@klsfId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@klsfId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = klsfId;
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
        /// 根据ID得到GetklsfId
        /// </summary>
        public int GetklsfId(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 klsfId from tb_klsfpay ");
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
            strSql.Append("select  top 1 WinCent from tb_klsfpay ");
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
        /// 是否存在机器人
        /// </summary>
        public bool ExistsReBot(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_klsfpay");
            strSql.Append(" where ID=@ID and UsID=@UsID and isRoBot=1");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.klsfpay Getklsfpay(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,Types,klsfId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,iWin,isRoBot,Odds from tb_klsfpay ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.klsfpay model=new BCW.Model.klsfpay();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.klsfId = reader.GetInt32(2);
					model.UsID = reader.GetInt32(3);
					model.UsName = reader.GetString(4);
					model.Price = reader.GetInt64(5);
					model.iCount = reader.GetInt32(6);
					model.Notes = reader.GetString(7);
					model.Result = reader.GetString(8);
					model.Prices = reader.GetInt64(9);
					model.WinCent = reader.GetInt64(10);
					model.State = reader.GetInt32(11);
					model.AddTime = reader.GetDateTime(12);
                    model.iWin = reader.GetInt32(13);
                    model.isRoBot = reader.GetInt32(14);
                    model.Odds = reader.GetDecimal(15);
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
			strSql.Append(" FROM tb_klsfpay ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// 根据字段统计有多少条数据符合条件
        /// </summary>
        /// <param name="strWhere">统计条件</param>
        /// <returns>统计结果</returns>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_klsfpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
		/// <returns>IList klsfpay</returns>
		public IList<BCW.Model.klsfpay> Getklsfpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.klsfpay> listklsfpays = new List<BCW.Model.klsfpay>();
			string sTable = "tb_klsfpay";
			string sPkey = "id";
            string sField = "ID,Types,klsfId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,iWin,isRoBot,Odds";
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
					return listklsfpays;
				}
                while (reader.Read())
                {
                    BCW.Model.klsfpay objklsfpay = new BCW.Model.klsfpay();
                    objklsfpay.ID = reader.GetInt32(0);
                    objklsfpay.Types = reader.GetInt32(1);
                    objklsfpay.klsfId = reader.GetInt32(2);
                    objklsfpay.UsID = reader.GetInt32(3);
                    objklsfpay.UsName = reader.GetString(4);
                    objklsfpay.Price = reader.GetInt64(5);
                    objklsfpay.iCount = reader.GetInt32(6);
                    objklsfpay.Notes = reader.GetString(7);
                    objklsfpay.Result = reader.GetString(8);
                    objklsfpay.Prices = reader.GetInt64(9);
                    objklsfpay.WinCent = reader.GetInt64(10);
                    objklsfpay.State = reader.GetInt32(11);
                    objklsfpay.AddTime = reader.GetDateTime(12);
                    objklsfpay.iWin = reader.GetInt32(13);
                    objklsfpay.isRoBot = reader.GetInt32(14);
                    objklsfpay.Odds = reader.GetDecimal(15);
                    listklsfpays.Add(objklsfpay);
                }
            }
			return listklsfpays;
		}

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>listklsfpayTop</returns>
        public IList<BCW.Model.klsfpay> GetklsfpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.klsfpay> listklsfpayTop = new List<BCW.Model.klsfpay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_klsfpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listklsfpayTop;
            }
            // 取出相关记录
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-Prices) as WinCents FROM tb_klsfpay where " + strWhere + " group by UsID Order by sum(WinCent-Prices) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.klsfpay objklsfpay = new BCW.Model.klsfpay();
                        objklsfpay.UsID = reader.GetInt32(0);
                        objklsfpay.WinCent = reader.GetInt64(1);
                        listklsfpayTop.Add(objklsfpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listklsfpayTop;
        }

		#endregion  成员方法
	}
}

