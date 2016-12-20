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
    /// 数据访问类Luckpay。
    /// </summary>
    public class Luckpay
    {
        public Luckpay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Luckpay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Luckpay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Luckpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int LuckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到机器人投注次数
        /// </summary>
        public int GetRobotbuy(int usid,int luckid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1) from tb_Luckpay ");
            strSql.Append(" where datediff(day,AddTime,getdate())=0 ");
            strSql.Append("and UsID=@UsID and IsRobot=1 and LuckId=@luckid");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                      new SqlParameter("@LuckId", SqlDbType.Int,4)};

            parameters[0].Value = usid;
            parameters[1].Value = luckid;
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
        /// 计算某期购买幸运数字的总币数
        /// </summary>
        public long GetSumBuyCent(int LuckId, string BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            strSql.Append(" and ','+BuyNum+',' Like '%," + BuyNum + ",%' and BuyType='Luck28End' ");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;
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
        /// 计算某期购买自选幸运数字的总币数
        /// </summary>
        public long GetSumBuyCentbychoose(int LuckId, string BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            strSql.Append(" and ','+BuyNum+',' Like '%," + BuyNum + ",%' and BuyType='Luck28Choose'");
            SqlParameter[] parameters = {
                    new SqlParameter("@LuckId", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;
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
        /// 计算某期购买某类型的总币数
        /// </summary>
        public long GetSumBuyTypeCent(int LuckId, string BuyType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCents) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            strSql.Append(" and BuyType=@BuyType");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4),
                    new SqlParameter("@BuyType", SqlDbType.NVarChar,50)};
            parameters[0].Value = LuckId;
            parameters[1].Value = BuyType;
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
        /// 计算某期某ID购买的总币数
        /// </summary>
        public long GetSumBuyCent(int LuckId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCents) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;
            parameters[1].Value = UsID;

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
        /// 计算某期所有玩家下注金额
        /// </summary>
        public long GetAllBuyCent(int LuckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCents) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;

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
        /// 计算某期购买人数
        /// </summary>
        public int GetCount(int LuckId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(DISTINCT UsID) from tb_Luckpay");
            strSql.Append(" where LuckId=@LuckId ");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4)};
            parameters[0].Value = LuckId;

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
        public int Add(BCW.Model.Game.Luckpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Luckpay(");
            strSql.Append("LuckId,UsID,UsName,BuyNum,BuyCent,BuyCents,WinCent,State,AddTime,BuyType,IsRobot,odds)");
            strSql.Append(" values (");
            strSql.Append("@LuckId,@UsID,@UsName,@BuyNum,@BuyCent,@BuyCents,@WinCent,@State,@AddTime,@BuyType,@IsRobot,@odds)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@LuckId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyNum", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@BuyCents", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@BuyType", SqlDbType.NVarChar,50), 
                    new SqlParameter("@IsRobot", SqlDbType.Int,4),
                    new SqlParameter("@odds", SqlDbType.NVarChar,10)};
            parameters[0].Value = model.LuckId;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BuyNum;
            parameters[4].Value = model.BuyCent;
            parameters[5].Value = model.BuyCents;
            parameters[6].Value = model.WinCent;
            parameters[7].Value = model.State;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.BuyType;
            parameters[10].Value = model.IsRobot;
            parameters[11].Value = model.odds;
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
        public void Update(BCW.Model.Game.Luckpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Luckpay set ");
            strSql.Append("LuckId=@LuckId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BuyNum=@BuyNum,");
            strSql.Append("BuyCent=@BuyCent,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LuckId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyNum", SqlDbType.NVarChar,200),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.LuckId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BuyNum;
            parameters[5].Value = model.BuyCent;
            parameters[6].Value = model.WinCent;
            parameters[7].Value = model.State;
            parameters[8].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Luckpay set ");
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
        /// 更新未兑奖的
        /// </summary>
        public void UpdateOverDay(string AddTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Luckpay set ");
            strSql.Append("State=3");
            strSql.Append(" where WinCent>0 and State=1 and AddTime<@AddTime");
            SqlParameter[] parameters = {
                new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = AddTime;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Luckpay set ");
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
            strSql.Append("delete from tb_Luckpay ");
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
            strSql.Append("delete from tb_Luckpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(" + ziduan + ") from tb_Luckpay");
            strSql.Append(" where " + strWhere + "  ");
            //strSql.Append(" where " + strWhere + "and UsID in (select ID from tb_User  where IsSpier=0) ");
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
        /// 后台计算投注总币值
        /// </summary>
        public long ManGetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(" + ziduan + ") from tb_Luckpay");
            //strSql.Append(" where " + strWhere + "  ");
            strSql.Append(" where " + strWhere + "and UsID in (select ID from tb_User  where IsSpier=0) ");
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
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_Luckpay ");
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
        public BCW.Model.Game.Luckpay GetLuckpay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,LuckId,UsID,UsName,BuyNum,BuyCent,WinCent,State,AddTime,BuyType from tb_Luckpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Luckpay model = new BCW.Model.Game.Luckpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.LuckId = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.BuyNum = reader.GetString(4);
                    model.BuyCent = reader.GetInt64(5);
                    model.WinCent = reader.GetInt64(6);
                    model.State = reader.GetByte(7);
                    model.AddTime = reader.GetDateTime(8);
                    if (!reader.IsDBNull(9))
                        model.BuyType = reader.GetString(9);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Luckpay ");
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
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Luckpay> listLuckpays = new List<BCW.Model.Game.Luckpay>();
            string sTable = "tb_Luckpay";
            string sPkey = "id";
            string sField = "ID,LuckId,UsID,UsName,BuyNum,BuyCent,WinCent,State,AddTime,BuyType,odds";
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
                    return listLuckpays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Luckpay objLuckpay = new BCW.Model.Game.Luckpay();
                    objLuckpay.ID = reader.GetInt32(0);
                    objLuckpay.LuckId = reader.GetInt32(1);
                    objLuckpay.UsID = reader.GetInt32(2);
                    objLuckpay.UsName = reader.GetString(3);
                    objLuckpay.BuyNum = reader.GetString(4);
                    objLuckpay.BuyCent = reader.GetInt64(5);
                    objLuckpay.WinCent = reader.GetInt64(6);
                    objLuckpay.State = reader.GetByte(7);
                    objLuckpay.AddTime = reader.GetDateTime(8);
                    if (!reader.IsDBNull(9))
                        objLuckpay.BuyType = reader.GetString(9);
                    objLuckpay.odds = reader.GetString(10);
                    listLuckpays.Add(objLuckpay);

                }
            }
            return listLuckpays;
        }


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.Model.Game.Luckpay> GetLuckpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Luckpay> listLuckpayTop = new List<BCW.Model.Game.Luckpay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Luckpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listLuckpayTop;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-BuyCents) as WinCents FROM tb_Luckpay where " + strWhere + " group by UsID Order by sum(WinCent-BuyCents) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.Luckpay objLuckpay = new BCW.Model.Game.Luckpay();
                        objLuckpay.UsID = reader.GetInt32(0);
                        objLuckpay.WinCent = reader.GetInt64(1);
                        listLuckpayTop.Add(objLuckpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listLuckpayTop;
        }

        #endregion  成员方法
    }
}
