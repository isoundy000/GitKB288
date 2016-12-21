using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
    /// <summary>
    /// 数据访问类Dicepay。
    /// </summary>
    public class Dicepay
    {
        public Dicepay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Dicepay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dicepay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int DiceId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dicepay");
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
        public bool Exists(int DiceId, int UsID, int bzType, int Types, int BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and bzType=@bzType ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and BuyNum=@BuyNum ");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@BuyNum", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
            parameters[1].Value = UsID;
            parameters[2].Value = bzType;
            parameters[3].Value = Types;
            parameters[4].Value = BuyNum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 计算某期购买人数
        /// </summary>
        public int GetCount(int DiceId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(DISTINCT UsID) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;

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
        /// 计算某期某选项购买人数
        /// </summary>
        public int GetCount(int DiceId, int Types, int BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and Types=@Types ");
            if (BuyNum > 0)
                strSql.Append(" and BuyNum=" + BuyNum + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
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
        /// 计算某期某选项购买币数
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, int BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and bzType=@bzType ");
            strSql.Append(" and Types=@Types ");
            if (BuyNum > 0)
                strSql.Append(" and BuyNum=" + BuyNum + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
            parameters[1].Value = bzType;
            parameters[2].Value = Types;

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
        /// 计算某期某选项购买人数
        /// </summary>
        public int GetCount(int DiceId, int Types, string BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and Types=@Types ");

            string[] sNum = BuyNum.Split("|".ToCharArray());
            if (sNum.Length > 0)
                strSql.Append(" and (");

            for (int i = 0; i < sNum.Length; i++)
            {
                if ((i + 1) == sNum.Length)
                    strSql.Append(" BuyNum=" + sNum[i] + " ");
                else
                    strSql.Append(" BuyNum=" + sNum[i] + " OR ");
            }
            if (sNum.Length > 0)
                strSql.Append(" )");

            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
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
        /// 计算某期某选项购买币数
        /// </summary>
        public long GetSumBuyCent(int DiceId, int bzType, int Types, string BuyNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Dicepay");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and bzType=@bzType ");
            strSql.Append(" and Types=@Types ");

            string[] sNum = BuyNum.Split("|".ToCharArray());
            if (sNum.Length > 0)
                strSql.Append(" and (");

            for (int i = 0; i < sNum.Length; i++)
            {
                if ((i + 1) == sNum.Length)
                    strSql.Append(" BuyNum=" + sNum[i] + " ");
                else
                    strSql.Append(" BuyNum=" + sNum[i] + " OR ");
            }
            if (sNum.Length > 0)
                strSql.Append(" )");

            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = DiceId;
            parameters[1].Value = bzType;
            parameters[2].Value = Types;

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
        /// 计算今天下注总币额
        /// </summary>
        public long GetSumBuyCent(int BzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Dicepay");
            strSql.Append(" where IsSpier=0 and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            if (BzType != -1)
                strSql.Append(" and BzType=" + BzType + "");

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
        /// 计算今天下注返彩总币额
        /// </summary>
        public long GetSumWinCent(int BzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_Dicepay");
            strSql.Append(" where IsSpier=0 and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            if (BzType != -1)
                strSql.Append(" and BzType=" + BzType + "");

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
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumBuyCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Dicepay");
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
        /// 根据条件计算返彩值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_Dicepay");
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Dicepay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Dicepay(");
            strSql.Append("Types,DiceId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,bzType,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@DiceId,@UsID,@UsName,@BuyNum,@BuyCount,@BuyCent,@WinCent,@State,@bzType,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyNum", SqlDbType.Int,4),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.DiceId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BuyNum;
            parameters[5].Value = model.BuyCount;
            parameters[6].Value = model.BuyCent;
            parameters[7].Value = model.WinCent;
            parameters[8].Value = model.State;
            parameters[9].Value = model.bzType;
            parameters[10].Value = model.AddTime;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
            //Hashtable ht = new Hashtable(); //创建一个Hashtable实例
            //ht.Add(strSql.ToString(), parameters);//key与value键值对

            //SqlHelper.ExecuteSqlTran(ht);
            //return 1;

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Dicepay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dicepay set ");
            strSql.Append("BuyCount=BuyCount+@BuyCount,");
            strSql.Append("BuyCent=BuyCent+@BuyCent,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where DiceId=@DiceId ");
            strSql.Append(" and UsID=@UsID");
            strSql.Append(" and Types=@Types");
            strSql.Append(" and bzType=@bzType");
            strSql.Append(" and BuyNum=@BuyNum");
            SqlParameter[] parameters = {
					new SqlParameter("@DiceId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BuyNum", SqlDbType.Int,4),
					new SqlParameter("@BuyCount", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.DiceId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.BuyNum;
            parameters[4].Value = model.BuyCount;
            parameters[5].Value = model.BuyCent;
            parameters[6].Value = model.bzType;
            parameters[7].Value = model.AddTime;

            //Hashtable ht = new Hashtable(); //创建一个Hashtable实例
            //ht.Add(strSql.ToString(), parameters);//key与value键值对

            //SqlHelper.ExecuteSqlTran(ht);

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Dicepay set ");
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
            strSql.Append("update tb_Dicepay set ");
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
            strSql.Append("delete from tb_Dicepay ");
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
            strSql.Append("delete from tb_Dicepay ");
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
            strSql.Append("select  top 1 WinCent from tb_Dicepay ");
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
            strSql.Append("select  top 1 bzType from tb_Dicepay ");
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
        public BCW.Model.Game.Dicepay GetDicepay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,DiceId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,bzType,AddTime from tb_Dicepay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Dicepay model = new BCW.Model.Game.Dicepay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.DiceId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.BuyNum = reader.GetInt32(5);
                    model.BuyCount = reader.GetInt32(6);
                    model.BuyCent = reader.GetInt64(7);
                    model.WinCent = reader.GetInt64(8);
                    model.State = reader.GetByte(9);
                    model.bzType = reader.GetInt32(10);
                    model.AddTime = reader.GetDateTime(11);
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
            strSql.Append(" FROM tb_Dicepay ");
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
        /// <returns>IList Dicepay</returns>
        public IList<BCW.Model.Game.Dicepay> GetDicepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Dicepay> listDicepays = new List<BCW.Model.Game.Dicepay>();
            string sTable = "tb_Dicepay";
            string sPkey = "id";
            string sField = "ID,Types,DiceId,UsID,UsName,BuyNum,BuyCount,BuyCent,WinCent,State,bzType,AddTime";
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
                    return listDicepays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Dicepay objDicepay = new BCW.Model.Game.Dicepay();
                    objDicepay.ID = reader.GetInt32(0);
                    objDicepay.Types = reader.GetInt32(1);
                    objDicepay.DiceId = reader.GetInt32(2);
                    objDicepay.UsID = reader.GetInt32(3);
                    objDicepay.UsName = reader.GetString(4);
                    objDicepay.BuyNum = reader.GetInt32(5);
                    objDicepay.BuyCount = reader.GetInt32(6);
                    objDicepay.BuyCent = reader.GetInt64(7);
                    objDicepay.WinCent = reader.GetInt64(8);
                    objDicepay.State = reader.GetByte(9);
                    objDicepay.bzType = reader.GetInt32(10);
                    objDicepay.AddTime = reader.GetDateTime(11);
                    listDicepays.Add(objDicepay);
                }
            }
            return listDicepays;
        }

        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Dicepay</returns>
        public IList<BCW.Model.Game.Dicepay> GetDicepaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Dicepay> listDicepayTop = new List<BCW.Model.Game.Dicepay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Dicepay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 50)
                p_recordCount = 50;

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listDicepayTop;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT Top 50 UsID,sum(WinCent-BuyCent) as WinCents FROM tb_Dicepay where " + strWhere + " group by UsID Order by sum(WinCent-BuyCent) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.Dicepay objDicepay = new BCW.Model.Game.Dicepay();
                        objDicepay.UsID = reader.GetInt32(0);
                        objDicepay.WinCent = reader.GetInt64(1);
                        listDicepayTop.Add(objDicepay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listDicepayTop;
        }

        #endregion  成员方法
    }
}
