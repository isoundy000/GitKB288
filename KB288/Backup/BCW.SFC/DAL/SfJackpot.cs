using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.SFC.DAL
{
    /// <summary>
    /// 数据访问类SfJackpot。
    /// </summary>
    public class SfJackpot
    {
        public SfJackpot()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_SfJackpot");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where CID=@CID and other like '%系统收取手续%' ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists4(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where CID=@CID and other like '%系统回收%' ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists4()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where other like '%系统首期%' ");
            SqlParameter[] parameters = {
                    };

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsgun(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where CID=@CID and usID=5 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsgun1(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where CID=@CID and usID=6 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfJackpot");
            strSql.Append(" where usid=@usid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.SFC.Model.SfJackpot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SfJackpot(");
            strSql.Append("usID,Prize,WinPrize,other,allmoney,AddTime,CID)");
            strSql.Append(" values (");
            strSql.Append("@usID,@Prize,@WinPrize,@other,@allmoney,@AddTime,@CID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@Prize", SqlDbType.BigInt,8),
					new SqlParameter("@WinPrize", SqlDbType.BigInt,8),
					new SqlParameter("@other", SqlDbType.NVarChar,50),
                    new SqlParameter("@allmoney", SqlDbType.BigInt,8),
                    new SqlParameter("AddTime",SqlDbType.DateTime),
                new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = model.usID;
            parameters[1].Value = model.Prize;
            parameters[2].Value = model.WinPrize;
            parameters[3].Value = model.other;
            parameters[4].Value = model.allmoney;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.CID;

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
        public void Update(BCW.SFC.Model.SfJackpot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfJackpot set ");
            strSql.Append("usID=@usID,");
            strSql.Append("Prize=@Prize,");
            strSql.Append("WinPrize=@WinPrize,");
            strSql.Append("other=@other, ");
            strSql.Append("allmoney=@allmoney, ");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("CID=@CID");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@usID", SqlDbType.Int,4),
					new SqlParameter("@Prize", SqlDbType.BigInt,8),
					new SqlParameter("@WinPrize", SqlDbType.BigInt,8),
					new SqlParameter("@other", SqlDbType.NVarChar,50),
                    	new SqlParameter("@allmoney", SqlDbType.BigInt,8),
                    new SqlParameter("AddTime",SqlDbType.DateTime),
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.usID;
            parameters[2].Value = model.Prize;
            parameters[3].Value = model.WinPrize;
            parameters[4].Value = model.other;
            parameters[5].Value = model.allmoney;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.CID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新usid
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usid"></param>
        public void UpdateusID(int id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfJackpot set ");
            strSql.Append("usID=@usID ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4) };
            parameters[0].Value = id;
            parameters[1].Value = usid;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新预售期的奖池
        /// </summary>
        /// <param name="allmoney"></param>
        /// <param name="CID"></param>
        public void updateallmoney(long allmoney, int CID, int usID, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfJackpot set ");
            strSql.Append("allmoney=@allmoney ");
            strSql.Append(" where CID=@CID ");
            strSql.Append(" and usID=@usID ");
            strSql.Append(" and id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@allmoney", SqlDbType.BigInt,8),
                    new SqlParameter("@CID", SqlDbType.Int,4),
                                        new SqlParameter("@usID",SqlDbType.Int,4),
                                            new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = allmoney;
            parameters[1].Value = CID;
            parameters[2].Value = usID;
            parameters[3].Value = id;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SfJackpot ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SfJackpot ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.SFC.Model.SfJackpot GetSfJackpot(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,usID,Prize,WinPrize,other,allmoney,AddTime,CID from tb_SfJackpot ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.SFC.Model.SfJackpot model = new BCW.SFC.Model.SfJackpot();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.usID = reader.GetInt32(1);
                    model.Prize = reader.GetInt64(2);
                    model.WinPrize = reader.GetInt64(3);
                    model.other = reader.GetString(4);
                    model.allmoney = reader.GetInt64(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.CID = reader.GetInt32(7);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinPrize) from tb_SfJackpot ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where (usID=1 or usID=0  or usID=2 or usID=3) and AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@WinPrize", SqlDbType.Int,8)};
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
        /// 得到一个WinCent不根据时间
        /// </summary>
        public long GetWinCentall()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinPrize) from tb_SfJackpot ");
            strSql.Append(" where usID=7 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@WinPrize", SqlDbType.Int,8)};
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
        /// 得到一个allmoney
        /// </summary>
        public long Getallmoney(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)allmoney from tb_SfJackpot where CID=" + CID + "");
            strSql.Append("  order by id DESC");

            SqlParameter[] parameters = {
                    new SqlParameter("@allmoney", SqlDbType.Int,8)};
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
            strSql.Append("select  sum(Prize) from tb_SfJackpot ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where (usID=1 or usID=0 or usID=2 or usID=3)  and AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@Prize", SqlDbType.Int,8)};
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
        /// 得到一个GetPayCent不根据时间
        /// </summary>
        public long GetPayCentall()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(Prize) from tb_SfJackpot ");
            strSql.Append(" where (usID=1 or usID=0 or usID=2 or usID=3) ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Prize", SqlDbType.Int,8)};
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
        /// 系统总投注额
        /// </summary>
        /// <returns></returns>
        public long SysPrice(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(Prize) from tb_SfJackpot where (usID=8 or usID=1) and CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@Prize", SqlDbType.BigInt,8)};

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
        /// 得到一个GetWinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysprize) from tb_SfList ");
            strSql.Append(" where CID=(select Top(1) CID from tb_SfList where State=1 Order by CID Desc) and sysprizestatue=2 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sysprize", SqlDbType.Int,8)};
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
        /// 得到一个GetWinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(sysprize) from tb_SfList ");
            strSql.Append(" where CID in (select Top(5) CID from tb_SfList where State=1 Order by CID Desc) and sysprizestatue=2 ");


            SqlParameter[] parameters = {
                    new SqlParameter("@sysprize", SqlDbType.Int,8)};
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
        /// 查询该期奖池
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long SysNowprize(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select nowprize from tb_SfJackpot where CID=" + CID + " order by id desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@nowprize", SqlDbType.BigInt,8)};

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
        /// 系统总回收额
        /// </summary>
        /// <returns></returns>
        public long SysWin(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinPrize) from tb_SfJackpot where usID=7  and CID=" + CID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinPrize", SqlDbType.BigInt,8)};

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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_SfJackpot ");
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
        /// <returns>IList SfJackpot</returns>
        public IList<BCW.SFC.Model.SfJackpot> GetSfJackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.SFC.Model.SfJackpot> listSfJackpots = new List<BCW.SFC.Model.SfJackpot>();
            string sTable = "tb_SfJackpot";
            string sPkey = "id";
            string sField = "id,usID,Prize,WinPrize,other,allmoney,AddTime,CID";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listSfJackpots;
                }
                while (reader.Read())
                {
                    BCW.SFC.Model.SfJackpot objSfJackpot = new BCW.SFC.Model.SfJackpot();
                    objSfJackpot.id = reader.GetInt32(0);
                    objSfJackpot.usID = reader.GetInt32(1);
                    objSfJackpot.Prize = reader.GetInt64(2);
                    objSfJackpot.WinPrize = reader.GetInt64(3);
                    objSfJackpot.other = reader.GetString(4);
                    objSfJackpot.allmoney = reader.GetInt64(5);
                    objSfJackpot.AddTime = reader.GetDateTime(6);
                    objSfJackpot.CID = reader.GetInt32(7);
                    listSfJackpots.Add(objSfJackpot);
                }
            }
            return listSfJackpots;
        }

        #endregion  成员方法
    }
}

