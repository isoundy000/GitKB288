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
    /// 数据访问类HP3WinnerSY。
    /// </summary>
    public class HP3WinnerSY
    {
        public HP3WinnerSY()
        {
        }
    #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3WinnerSY");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(string WinDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3WinnerSY");
            strSql.Append(" where WinDate=@WinDate");
            SqlParameter[] parameters = {
					new SqlParameter("@WinDate",  SqlDbType.NChar,10)
			};
            parameters[0].Value = WinDate;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int ID,int WinUserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3WinnerSY");
            strSql.Append(" where ID=@ID and WinUserID=@WinUserID and WinBool=1");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@WinUserID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;
            parameters[1].Value = WinUserID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HP3.Model.HP3WinnerSY model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HP3WinnerSY(");
            strSql.Append("ID,WinUserID,WinDate,WinMoney,WinBool)");
            strSql.Append(" values (");
            strSql.Append("@ID,@WinUserID,@WinDate,@WinMoney,@WinBool)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinUserID", SqlDbType.Int,4),
					new SqlParameter("@WinDate", SqlDbType.NChar,10),
					new SqlParameter("@WinMoney", SqlDbType.BigInt,8),
					new SqlParameter("@WinBool", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinUserID;
            parameters[2].Value = model.WinDate;
            parameters[3].Value = model.WinMoney;
            parameters[4].Value = model.WinBool;

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
        public bool Update(BCW.HP3.Model.HP3WinnerSY model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3WinnerSY set ");
            strSql.Append("WinUserID=@WinUserID,");
            strSql.Append("WinDate=@WinDate,");
            strSql.Append("WinMoney=@WinMoney,");
            strSql.Append("WinBool=@WinBool");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@WinUserID", SqlDbType.Int,4),
					new SqlParameter("@WinDate", SqlDbType.NChar,10),
					new SqlParameter("@WinMoney", SqlDbType.BigInt,8),
					new SqlParameter("@WinBool", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.WinUserID;
            parameters[1].Value = model.WinDate;
            parameters[2].Value = model.WinMoney;
            parameters[3].Value = model.WinBool;
            parameters[4].Value = model.ID;

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
        ///通过ID更新数据
        public bool UpdateByID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3WinnerSY ");
            strSql.Append("SET WinBool=0");
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
        ///通过ID更新数据
        public bool RoBotByID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3WinnerSY ");
            strSql.Append("SET WinBool=2");
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
        ///通过ID更新WinZhu数据
        public bool UpdateWinZhu(int ID,int winzhu)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3WinnerSY ");
            strSql.Append("SET WinZhu=@WinZhu");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinZhu", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = winzhu;
            parameters[1].Value = ID;
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
        ///通过期号更新数据
        public void UpdateByWinDate(string WinDate)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3WinnerSY ");
            strSql.Append("set WinBool=2");
            strSql.Append(" where WinBool=1 and WinDate<='"+WinDate+"'");
            SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HP3WinnerSY ");
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(string swhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HP3WinnerSY ");
            strSql.Append(" where "+ swhere);
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,WinUserID,WinDate,WinMoney,WinBool ");
            strSql.Append(" FROM tb_HP3WinnerSY ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得全部数据列表
        /// </summary>
        public DataSet GetLists(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM tb_HP3WinnerSY ");
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
            strSql.Append("select WinUserID,sum(WinMoney) ");
            strSql.Append("from tb_HP3WinnerSY ");
            strSql.Append(" group by WinUserID order by sum(WinMoney)");
            return SqlHelper.Query(strSql.ToString());
        }
        //取排行榜数据列表
        public DataSet GetListBang2(string s1,string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select WinUserID,sum(WinMoney) ");
            strSql.Append("from tb_HP3WinnerSY ");
            strSql.Append("  where WinDate>='" + s1 + "'and WinDate<='" + s2 + "' ");
            strSql.Append(" group by WinUserID order by sum(WinMoney)");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3WinnerSY GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinUserID,WinDate,WinMoney,WinBool,WinZhu from tb_HP3WinnerSY ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)			};
            parameters[0].Value = ID;

            BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.Model.HP3WinnerSY();
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3WinnerSY DataRowToModel(DataRow row)
        {
            BCW.HP3.Model.HP3WinnerSY model = new BCW.HP3.Model.HP3WinnerSY();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["WinUserID"] != null && row["WinUserID"].ToString() != "")
                {
                    model.WinUserID = int.Parse(row["WinUserID"].ToString());
                }
                if (row["WinDate"] != null)
                {
                    model.WinDate = row["WinDate"].ToString();
                }
                if (row["WinMoney"] != null && row["WinMoney"].ToString() != "")
                {
                    model.WinMoney = long.Parse(row["WinMoney"].ToString());
                }
                if (row["WinBool"] != null && row["WinBool"].ToString() != "")
                {
                    model.WinBool = int.Parse(row["WinBool"].ToString());
                }
                if (row["WinZhu"] != null && row["WinZhu"].ToString() != "")
                {
                    model.WinZhu = int.Parse(row["WinZhu"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 分页获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage(int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  WinUserID,sum(WinMoney) AS'sm' into #bang from tb_HP3WinnerSY  group by WinUserID order by sum(WinMoney)");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm desc");
            strSql.Append(")AS Row, T.*  from #bang T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex,string s1,string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  WinUserID,sum(WinMoney) AS'sm' into #bang3 from tb_HP3WinnerSY   where WinDate>='" + s1 + "' and WinDate<='" + s2 + "'  group by WinUserID order by sum(WinMoney)");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.sm desc");
            strSql.Append(")AS Row, T.*  from #bang3 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang3");
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
        public IList<BCW.HP3.Model.HP3WinnerSY> GetListNes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.HP3.Model.HP3WinnerSY> listHP3 = new List<BCW.HP3.Model.HP3WinnerSY>();
            string sTable = "tb_HP3WinnerSY";
            string sPkey = "ID";
            string sField = "ID,WinUserID,WinDate,WinMoney,WinBool,WinZhu";
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
                    return listHP3;
                }
                while (reader.Read())
                {
                    BCW.HP3.Model.HP3WinnerSY objHP3 = new BCW.HP3.Model.HP3WinnerSY();
                    objHP3.ID = reader.GetInt32(0);
                    objHP3.WinUserID = reader.GetInt32(1);
                    objHP3.WinDate = reader.GetString(2);
                    objHP3.WinMoney = reader.GetInt64(3);
                    objHP3.WinBool = reader.GetInt32(4);
                    objHP3.WinZhu= reader.GetInt32(5);
                    listHP3.Add(objHP3);
                }
            }
            return listHP3;
        }
       //查询我的中奖未兑换订单
        public DataSet GetMyWinList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" from tb_HP3WinnerSY inner join tb_HP3BuySY on tb_HP3WinnerSY.ID=tb_HP3BuySY.ID  ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where WinBool=1 and WinUserID=" + strWhere);
            }
            strSql.Append("order by BuyTime desc");
            return SqlHelper.Query(strSql.ToString());
        }
        //通过用户获得该用户总收入
        public DataSet GetMyAllGet(int WinUserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinMoney) from  tb_HP3WinnerSY");
            strSql.Append(" where WinUserID="+WinUserID);
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到酷币支出
        /// </summary>
        public DataSet GetMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinMoney) ");
            strSql.Append(" FROM tb_HP3WinnerSY ");
            strSql.Append(" where WinBool=0 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and WinDate  like '%" + strWhere + "%'");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到酷币支出2
        /// </summary>
        public DataSet GetMoney2(string strWhere, string strWhere2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinMoney) ");
            strSql.Append(" FROM tb_HP3WinnerSY ");
            strSql.Append(" where WinBool=0 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and WinDate>='" + strWhere + "' and WinDate<='" + strWhere2+ "'");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        
    #endregion  成员方法
    }
}
