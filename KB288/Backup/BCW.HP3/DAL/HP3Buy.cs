using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HP3.DAL
{
    /// <summary>
    /// 数据访问类HP3Buy。
    /// </summary>
    public class HP3Buy
    {
        public HP3Buy()
        {
        }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3Buy");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        //增加一条HP3购彩记录
        public int Add(BCW.HP3.Model.HP3Buy model){
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HP3Buy(");
            strSql.Append("BuyID,BuyDate,BuyType,BuyNum,BuyMoney,BuyZhu,BuyTime,Odds)");
            strSql.Append("values(");
            strSql.Append("@BuyID,@BuyDate,@BuyType,@BuyNum,@BuyMoney,@BuyZhu,@BuyTime,@Odds)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = { 
                                        new SqlParameter("@BuyID", SqlDbType.Int,4),
                                        new SqlParameter("@BuyDate", SqlDbType.NChar,10),
                                        new SqlParameter("@BuyType", SqlDbType.Int,4),
                                        new SqlParameter("@BuyNum", SqlDbType.NVarChar,-1),
                                        new SqlParameter("@BuyMoney", SqlDbType.BigInt,8),
                                        new SqlParameter("@BuyZhu", SqlDbType.Int,4),
					                    new SqlParameter("@BuyTime", SqlDbType.DateTime),
                                        new SqlParameter("@Odds", SqlDbType.Decimal,9)};
            parameters[0].Value = model.BuyID;
            parameters[1].Value = model.BuyDate;
            parameters[2].Value = model.BuyType;
            parameters[3].Value = model.BuyNum;
            parameters[4].Value = model.BuyMoney;
            parameters[5].Value = model.BuyZhu;
            parameters[6].Value = model.BuyTime;
            parameters[7].Value = model.Odds;
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
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HP3.Model.HP3Buy model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3Buy set ");
            strSql.Append("BuyID=@BuyID,");
            strSql.Append("BuyDate=@BuyDate,");
            strSql.Append("BuyType=@BuyType,");
            strSql.Append("BuyNum=@BuyNum,");
            strSql.Append("BuyMoney=@BuyMoney");
            strSql.Append("BuyZhu=@BuyZhu");
            strSql.Append("BuyTime=@BuyTime");
            strSql.Append("Odds=@Odds");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyID", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.NChar,10),
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyNum", SqlDbType.NVarChar,-1),
					new SqlParameter("@BuyMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@BuyZhu", SqlDbType.Int,4),
                    new SqlParameter("@BuyTime", SqlDbType.DateTime),
                    new SqlParameter("@Odds", SqlDbType.Decimal,9),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.BuyID;
            parameters[1].Value = model.BuyDate;
            parameters[2].Value = model.BuyType;
            parameters[3].Value = model.BuyNum;
            parameters[4].Value = model.BuyMoney;
            parameters[5].Value = model.BuyZhu;
            parameters[6].Value = model.BuyTime;
            parameters[7].Value = model.Odds;
            parameters[8].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
             
        /// <summary>
        /// 更新WillGet
        /// </summary>
        public void UpdateWillGet(int ID, long WillGet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3Buy set ");
            strSql.Append(" WillGet=@WillGet");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WillGet", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = WillGet;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新IsRot
        /// </summary>
        public void UpdateIsRot(int ID, int IsRot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3Buy set ");
            strSql.Append(" IsRot=@IsRot");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsRot", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = IsRot;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HP3Buy ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 根据期数删除数据
        /// </summary>
        public bool DeleteBuyDate(string BuyDate) 
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HP3Buy ");
            strSql.Append(" where BuyDate=@BuyDate");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyDate", SqlDbType.Int,4)
			};
            parameters[0].Value = BuyDate;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //根据字段取数据列表
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_HP3Buy ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //取排行榜数据列表
        public DataSet GetListBang()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BuyID,sum(BuyMoney*BuyZhu) ");
            strSql.Append("from tb_HP3Buy ");
            strSql.Append(" group by BuyID order by sum(BuyMoney*BuyZhu)");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.HP3.Model.HP3Buy> GetHP3ListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.HP3.Model.HP3Buy> model = new List<BCW.HP3.Model.HP3Buy>();
            string sTable = "tb_HP3Buy";
            string sPkey = "id";
            string sField = "ID,BuyID,BuyDate,BuyType,BuyNum,BuyMoney,BuyZhu,BuyTime,Odds";
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
                    return model;
                }
                while (reader.Read())
                {
                    BCW.HP3.Model.HP3Buy objHP3 = new BCW.HP3.Model.HP3Buy();
                    objHP3.ID = reader.GetInt32(0);
                    objHP3.BuyID = reader.GetInt32(1);
                    objHP3.BuyDate= reader.GetString(2);
                    objHP3.BuyType = reader.GetInt32(3);
                    objHP3.BuyNum= reader.GetString(4);
                    objHP3.BuyMoney= reader.GetInt64(5);
                    objHP3.BuyZhu = reader.GetInt32(6);
                    objHP3.BuyTime = reader.GetDateTime(7);
                    objHP3.Odds = reader.GetDecimal(8);
                    model.Add(objHP3);
                }
            }
            return model;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3Buy GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,BuyID,BuyDate,BuyType,BuyNum,BuyMoney,BuyZhu,BuyTime,Odds from tb_HP3Buy ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            BCW.HP3.Model.HP3Buy model = new BCW.HP3.Model.HP3Buy();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_HP3Buy ");
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
        //根据字段取数据列表
        public DataSet GetDaXiao(string qihao, string goumai)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(BuyMoney*Buyzhu) ");
            strSql.Append(" FROM tb_HP3Buy ");
            strSql.Append(" where BuyType=17 and BuyDate='" + qihao + "' and BuyNum=" + goumai);
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3Buy DataRowToModel(DataRow row)
        {
            BCW.HP3.Model.HP3Buy model = new BCW.HP3.Model.HP3Buy();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["BuyID"] != null && row["BuyID"].ToString() != "")
                {
                    model.BuyID = int.Parse(row["BuyID"].ToString());
                }
                if (row["BuyDate"] != null)
                {
                    model.BuyDate = row["BuyDate"].ToString();
                }
                if (row["BuyType"] != null && row["BuyType"].ToString() != "")
                {
                    model.BuyType = int.Parse(row["BuyType"].ToString());
                }
                if (row["BuyNum"] != null)
                {
                    model.BuyNum = row["BuyNum"].ToString();
                }
                if (row["BuyMoney"] != null && row["BuyMoney"].ToString() != "")
                {
                    model.BuyMoney = long.Parse(row["BuyMoney"].ToString());
                }
                if (row["BuyZhu"] != null && row["BuyZhu"].ToString() != "")
                {
                    model.BuyZhu = int.Parse(row["BuyZhu"].ToString());
                }
                if (row["BuyTime"] != null && row["BuyTime"].ToString() != "")
                {
                    model.BuyTime = DateTime.Parse(row["BuyTime"].ToString());
                }
                if (row["Odds"] != null && row["Odds"].ToString() != "")
                {
                    model.Odds = Decimal.Parse(row["Odds"].ToString());
                }
            }
            return model;
        }
        //通过用户获得该用户总支出
        public DataSet GetMyAllPost(int BuyID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(BuyMoney*BuyZhu) from  tb_HP3Buy");
            strSql.Append(" where BuyID=" + BuyID);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        ///某期某种投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney*BuyZhu) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期同花投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes1(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and BuyNum>0 and BuyNum<=5 ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期顺子投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes2(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and BuyNum>=6 and BuyNum<=18 ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期同花顺投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes3(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and BuyNum>=19 and BuyNum<=23 ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期豹子投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes4(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and BuyNum>=24 and BuyNum<=37 ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期对子投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypes5(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and BuyNum>=38 and BuyNum<=51 ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期大小投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypesda(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and ( BuyNum=1 or BuyNum=2 ) ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        ///某期单双投注方式投了多少币
        /// </summary>
        public long GetSumPricebyTypesdan(int BuyType, int BuyDate)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyMoney) from tb_HP3Buy ");
            strSql.Append(" where BuyType=@BuyType ");
            strSql.Append(" and BuyDate=@BuyDate ");
            strSql.Append(" and ( BuyNum=3 or BuyNum=4 ) ");
            SqlParameter[] parameters = {
					new SqlParameter("@BuyType", SqlDbType.Int,4),
					new SqlParameter("@BuyDate", SqlDbType.Int,4)};
            parameters[0].Value = BuyType;
            parameters[1].Value = BuyDate;
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
        /// 得到酷币收入
        /// </summary>
        public DataSet GetMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(BuyMoney*BuyZhu) ");
            strSql.Append(" FROM tb_HP3Buy ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where IsRot is null and BuyDate  like '%" + strWhere + "%'");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到酷币收入
        /// </summary>
        public DataSet GetMoney2(string strWhere, string strWhere2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(BuyMoney*BuyZhu) ");
            strSql.Append(" FROM tb_HP3Buy ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where  IsRot is null and BuyDate>='" + strWhere + "' and BuyDate<='" + strWhere2 + "'");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //取排行榜数据列表
        public DataSet GetListBang2(string s1,string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BuyID,sum(BuyMoney*BuyZhu) ");
            strSql.Append("from tb_HP3Buy ");
            strSql.Append("  where BuyDate>='" + s1 + "' and BuyDate<='" + s2 + "' ");
            strSql.Append(" group by BuyID order by sum(BuyMoney*BuyZhu)");
            return SqlHelper.Query(strSql.ToString());
        }
        ///根据期号范围查询排行榜
        /// <summary>
        /// 分页获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage(int startIndex, int endIndex,string s1,string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BuyID,sum(BuyMoney*BuyZhu) AS'sm' into #bang2 from  tb_HP3Buy  where BuyDate>='" +s1+ "' and BuyDate<='"+s2+"' group by BuyID  order by sum(BuyMoney*BuyZhu) ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm desc");
            strSql.Append(")AS Row, T.*  from #bang2 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang2");
            return SqlHelper.Query(strSql.ToString());
        }
        //取净赚排行榜数据列表
        public DataSet GetBang(string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BuyID,sum(WillGet-BuyMoney*BuyZhu) ");
            strSql.Append("from tb_HP3Buy ");
            strSql.Append("  where BuyDate>='" + s1 + "' and BuyDate<='" + s2 + "' ");
            strSql.Append(" group by BuyID order by sum(BuyMoney*BuyZhu)");
            return SqlHelper.Query(strSql.ToString());
        }
        ///根据期号范围查询净赚排行榜
        /// <summary>
        /// 分页获取排行榜数据列表
        /// </summary>
        public DataSet GetBangByPage(int startIndex, int endIndex, string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select BuyID,sum(WillGet-BuyMoney*BuyZhu) AS'sm' into #bang2 from  tb_HP3Buy  where BuyDate>='" + s1 + "' and BuyDate<='" + s2 + "' group by BuyID  order by sum(BuyMoney*BuyZhu) ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm desc");
            strSql.Append(")AS Row, T.*  from #bang2 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang2");
            return SqlHelper.Query(strSql.ToString());
        }
        #endregion  成员方法
    }
}
