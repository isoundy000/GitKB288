using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.JQC.DAL
{
    /// <summary>
    /// 数据访问类JQC_Jackpot。
    /// </summary>
    public class JQC_Jackpot
    {
        public JQC_Jackpot()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_JQC_Jackpot");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Jackpot");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.JQC.Model.JQC_Jackpot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_JQC_Jackpot(");
            strSql.Append("UsID,InPrize,OutPrize,Jackpot,AddTime,phase,type,BetID)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@InPrize,@OutPrize,@Jackpot,@AddTime,@phase,@type,@BetID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@InPrize", SqlDbType.BigInt,8),
                    new SqlParameter("@OutPrize", SqlDbType.BigInt,8),
                    new SqlParameter("@Jackpot", SqlDbType.BigInt,8),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@phase", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@BetID", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.InPrize;
            parameters[2].Value = model.OutPrize;
            parameters[3].Value = model.Jackpot;
            parameters[4].Value = model.AddTime;
            parameters[5].Value = model.phase;
            parameters[6].Value = model.type;
            parameters[7].Value = model.BetID;

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
        public void Update(BCW.JQC.Model.JQC_Jackpot model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_JQC_Jackpot set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("InPrize=@InPrize,");
            strSql.Append("OutPrize=@OutPrize,");
            strSql.Append("Jackpot=@Jackpot,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("phase=@phase,");
            strSql.Append("type=@type,");
            strSql.Append("BetID=@BetID");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@InPrize", SqlDbType.BigInt,8),
                    new SqlParameter("@OutPrize", SqlDbType.BigInt,8),
                    new SqlParameter("@Jackpot", SqlDbType.BigInt,8),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@phase", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@BetID", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.InPrize;
            parameters[3].Value = model.OutPrize;
            parameters[4].Value = model.Jackpot;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.phase;
            parameters[7].Value = model.type;
            parameters[8].Value = model.BetID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_JQC_Jackpot ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.JQC.Model.JQC_Jackpot GetJQC_Jackpot(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,UsID,InPrize,OutPrize,Jackpot,AddTime,phase,type,BetID from tb_JQC_Jackpot ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.JQC.Model.JQC_Jackpot model = new BCW.JQC.Model.JQC_Jackpot();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.InPrize = reader.GetInt64(2);
                    model.OutPrize = reader.GetInt64(3);
                    model.Jackpot = reader.GetInt64(4);
                    model.AddTime = reader.GetDateTime(5);
                    model.phase = reader.GetInt32(6);
                    model.type = reader.GetInt32(7);
                    model.BetID = reader.GetInt32(8);
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
            strSql.Append(" FROM tb_JQC_Jackpot ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //===========================================
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_JQC_Jackpot");
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
        ///  me_根据id得到奖池
        /// </summary>
        public long GetGold()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) Jackpot from tb_JQC_Jackpot ");
            strSql.Append(" ORDER BY ID desc ");

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_根据期号得到奖池
        /// </summary>
        public long GetGold_phase(int phase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) Jackpot from tb_JQC_Jackpot where phase=@phase  ORDER BY id desc");

            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4)};
            parameters[0].Value = phase;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到系统投进的次数
        /// </summary>
        public int Getxitong_toujin()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_JQC_Jackpot WHERE type=4 AND InPrize>0");

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到系统回收的次数
        /// </summary>
        public int Getxitong_huishou()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_JQC_Jackpot WHERE type=4 AND OutPrize>0");

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_根据投注ID得到奖池——20160713
        /// </summary>
        public long Get_BetID(int BetID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1) Jackpot from tb_JQC_Jackpot where BetID=@BetID");

            SqlParameter[] parameters = {
                    new SqlParameter("@BetID", SqlDbType.Int,4)};
            parameters[0].Value = BetID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_根据期号得到未开奖的奖池——20160713
        /// </summary>
        public long Getweikai_phase(int phase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(InPrize) FROM tb_JQC_Jackpot where phase=@phase");

            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4)};
            parameters[0].Value = phase;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                try
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        return reader.GetInt64(0);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到该期系统收取
        /// </summary>
        public long Get_xtshouqu(int phase)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(OutPrize) FROM tb_JQC_Jackpot WHERE type=3 AND phase=@phase");
            SqlParameter[] parameters = {
                    new SqlParameter("@phase", SqlDbType.Int,4)};
            parameters[0].Value = phase;

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
        /// me_是否存在系统投进记录
        /// </summary>
        public bool Exists_jc(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Jackpot");
            strSql.Append(" where phase=@id AND type=4 AND InPrize>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_是否存在系统扣除记录
        /// </summary>
        public bool Exists_kc(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_JQC_Jackpot");
            strSql.Append(" where phase=@id AND type=4 AND OutPrize>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //==


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList JQC_Jackpot</returns>
        public IList<BCW.JQC.Model.JQC_Jackpot> GetJQC_Jackpots(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.JQC.Model.JQC_Jackpot> listJQC_Jackpots = new List<BCW.JQC.Model.JQC_Jackpot>();
            string sTable = "tb_JQC_Jackpot";
            string sPkey = "id";
            string sField = "*";
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
                    return listJQC_Jackpots;
                }
                while (reader.Read())
                {
                    BCW.JQC.Model.JQC_Jackpot objJQC_Jackpot = new BCW.JQC.Model.JQC_Jackpot();
                    objJQC_Jackpot.id = reader.GetInt32(0);
                    objJQC_Jackpot.UsID = reader.GetInt32(1);
                    objJQC_Jackpot.InPrize = reader.GetInt64(2);
                    objJQC_Jackpot.OutPrize = reader.GetInt64(3);
                    objJQC_Jackpot.Jackpot = reader.GetInt64(4);
                    objJQC_Jackpot.AddTime = reader.GetDateTime(5);
                    objJQC_Jackpot.phase = reader.GetInt32(6);
                    objJQC_Jackpot.type = reader.GetInt32(7);
                    objJQC_Jackpot.BetID = reader.GetInt32(8);
                    listJQC_Jackpots.Add(objJQC_Jackpot);
                }
            }
            return listJQC_Jackpots;
        }

        #endregion  成员方法
    }
}

