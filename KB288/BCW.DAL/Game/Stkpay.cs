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
    /// 数据访问类Stkpay。
    /// </summary>
    public class Stkpay
    {
        public Stkpay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Stkpay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int StkId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = StkId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int StkId, int UsID, int bzType, int Types, decimal Odds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and bzType=@bzType ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and Odds=@Odds ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Odds", SqlDbType.Money,8)};
            parameters[0].Value = StkId;
            parameters[1].Value = UsID;
            parameters[2].Value = bzType;
            parameters[3].Value = Types;
            parameters[4].Value = Odds;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某类型的投注额
        /// </summary>
        public int GetCent(int StkId, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and bzType=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = StkId;
            parameters[1].Value = Types;

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
        public int Add(BCW.Model.Game.Stkpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Stkpay(");
            strSql.Append("bzType,Types,WinNum,StkId,UsID,UsName,Odds,BuyCent,WinCent,State,AddTime,isSpier)");
            strSql.Append(" values (");
            strSql.Append("@bzType,@Types,@WinNum,@StkId,@UsID,@UsName,@Odds,@BuyCent,@WinCent,@State,@AddTime,@isSpier)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Odds", SqlDbType.Money,8),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    	new SqlParameter("@isSpier", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.bzType;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.WinNum;
            parameters[3].Value = model.StkId;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Odds;
            parameters[7].Value = model.BuyCent;
            parameters[8].Value = model.WinCent;
            parameters[9].Value = model.State;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.isSpier;

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
        public void Update(BCW.Model.Game.Stkpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
            strSql.Append("BuyCent=BuyCent+@BuyCent,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and UsID=@UsID");
            strSql.Append(" and Types=@Types");
            strSql.Append(" and bzType=@bzType");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.StkId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.BuyCent;
            parameters[4].Value = model.bzType;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State, int WinNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = WinNum;
            parameters[2].Value = WinCent;
            parameters[3].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
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

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Stkpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Stkpay ");
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
            strSql.Append("select  top 1 WinCent from tb_Stkpay ");
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
        /// 得到一个bzType
        /// </summary>
        public int GetbzType(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 bzType from tb_Stkpay ");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Stkpay GetStkpay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,bzType,Types,StkId,UsID,UsName,BuyCent,WinCent,State,AddTime,Odds,isSpier from tb_Stkpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Stkpay model = new BCW.Model.Game.Stkpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.bzType = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.StkId = reader.GetInt32(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.BuyCent = reader.GetInt64(6);
                    model.WinCent = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.Odds = reader.GetDecimal(10);
                    model.isSpier = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Stkpay GetStkpaybystkid(int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,bzType,Types,StkId,UsID,UsName,BuyCent,WinCent,State,AddTime,Odds,isSpier from tb_Stkpay ");
            strSql.Append(" where StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = StkId;

            BCW.Model.Game.Stkpay model = new BCW.Model.Game.Stkpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.bzType = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.StkId = reader.GetInt32(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.BuyCent = reader.GetInt64(6);
                    model.WinCent = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.Odds = reader.GetDecimal(10);
                    model.isSpier = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = StkId;
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
        /// 某期某玩法某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int StkId, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and StkId=@StkId  and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4),
                                       new SqlParameter("@Types", SqlDbType.Int,4) };
            parameters[0].Value = UsID;
            parameters[1].Value = StkId;
            parameters[2].Value = Types;
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
        /// 得到一个GetPayCentlast
        /// </summary>
        public long GetPayCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(BuyCent) from tb_Stkpay  ");

            strSql.Append(" where StkId=(select Top(1) ID from tb_StkList where State=1  Order by ID Desc) and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");

            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
        /// 得到一个WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");

            strSql.Append("where StkId=(select Top(1) ID from tb_StkList where State=1 Order by ID Desc) and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");

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
        /// 得到一个GetPayCentlast5
        /// </summary>
        public long GetPayCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(BuyCent) from tb_Stkpay  ");

            strSql.Append(" where StkId in (select Top(5) ID from tb_StkList where State=1  Order by ID Desc) and isSpier!=1 and UsID in (select ID from tb_User where IsSpier!=1) ");

            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
        /// 得到一个WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");

            strSql.Append("where StkId in (select Top(5) ID from tb_StkList where State=1 Order by ID Desc) and isSpier!=1 and UsID in (select ID from tb_User where IsSpier!=1)  ");

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
        /// 得到一个GetPayCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(BuyCent) from tb_Stkpay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where  State!=0 and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where State!=0 and  isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
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
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_Stkpay ");
            strSql.Append(" where " + strWhere + " ");
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
        /// 某期某投注方式共投了多少币
        /// </summary>
        public long GetSumPricesbytype(int Types, int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = StkId;
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
        /// 根据字段统计有多少条数据符合条件
        /// </summary>
        /// <param name="strWhere">统计条件</param>
        /// <returns>统计结果</returns>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_Stkpay ");
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
        /// 是否存在机器人
        /// </summary>
        public bool ExistsReBot(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID and UsID=@UsID and isSpier=1");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 获得状态
        /// </summary>
        /// <returns></returns>
        public int getState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_Stkpay where ID=" + ID + " ");
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Stkpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
        /// <returns>IList Stkpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Stkpay> listStkpays = new List<BCW.Model.Game.Stkpay>();
            string sTable = "tb_Stkpay";
            string sPkey = "id";
            string sField = "ID,bzType,Types,WinNum,StkId,UsID,UsName,Odds,BuyCent,WinCent,State,AddTime,isSpier";
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
                    return listStkpays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Stkpay objStkpay = new BCW.Model.Game.Stkpay();
                    objStkpay.ID = reader.GetInt32(0);
                    objStkpay.bzType = reader.GetInt32(1);
                    objStkpay.Types = reader.GetInt32(2);
                    objStkpay.WinNum = reader.GetInt32(3);
                    objStkpay.StkId = reader.GetInt32(4);
                    objStkpay.UsID = reader.GetInt32(5);
                    objStkpay.UsName = reader.GetString(6);
                    objStkpay.Odds = reader.GetDecimal(7);
                    objStkpay.BuyCent = reader.GetInt64(8);
                    objStkpay.WinCent = reader.GetInt64(9);
                    objStkpay.State = reader.GetByte(10);
                    objStkpay.AddTime = reader.GetDateTime(11);
                    objStkpay.isSpier = reader.GetInt32(12);
                    listStkpays.Add(objStkpay);
                }
            }
            return listStkpays;
        }

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Stkpay> listStkpayTop = new List<BCW.Model.Game.Stkpay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Stkpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listStkpayTop;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-BuyCent) as WinCents FROM tb_Stkpay where " + strWhere + " group by UsID Order by sum(WinCent-BuyCent) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.Stkpay objStkpay = new BCW.Model.Game.Stkpay();
                        objStkpay.UsID = reader.GetInt32(0);
                        objStkpay.WinCent = reader.GetInt64(1);
                        listStkpayTop.Add(objStkpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listStkpayTop;
        }

        #endregion  成员方法
    }
}

