using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace TPR3.DAL.guess
{
    /// <summary>
    /// 数据访问类BaPay。
    /// </summary>
    public class BaPay
    {
        public BaPay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到一个bcid
        /// </summary>
        public int Getbcid(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 bcid from tb_ZBaPay ");
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
        /// 得到某一赛事某项下注总金额
        /// </summary>
        public long GetBaPayCent2(int ibcid, int iptype, int ipaytype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("PayType=@PayType and ");
            strSql.Append("itypes=@itypes and Types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = ipaytype;
            parameters[3].Value = 0;
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
        /// 得到某一赛事某项走地下注总金额
        /// </summary>
        public long GetBaPayCent2(int ibcid, int iptype, int ipaytype, DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("PayType=@PayType and ");
            strSql.Append("itypes=@itypes and Types=0");
            strSql.Append(" and paytimes>'" + dt + "'");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = ipaytype;
            parameters[3].Value = 0;
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
        //-------------------------超级竞猜使用--------------------------------

        /// <summary>
        /// 是否存在超级竞猜结束记录
        /// </summary>
        public bool Exists(int bcid, int payusid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaPay");
            strSql.Append(" where bcid=@bcid ");
            strSql.Append(" and payusid=@payusid ");
            strSql.Append(" and itypes=@itypes ");
            strSql.Append(" and p_active>@p_active ");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = bcid;
            parameters[1].Value = payusid;
            parameters[2].Value = 1;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到某一赛事下注结果得币
        /// </summary>
        public decimal GetBaPayMoney(int id, int payusid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT p_getMoney from tb_ZBaPay where ");
            strSql.Append(" id=@id ");
            strSql.Append(" and payusid=@payusid ");
            strSql.Append(" and itypes=@itypes ");
            strSql.Append(" and p_active>@p_active ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4),
					new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = payusid;
            parameters[2].Value = 1;
            parameters[3].Value = 0;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDecimal(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        //--------------------------------超级竞猜使用-------------------------


        /// <summary>
        /// 更新走地下注为失败并结束
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateLost(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaPay set ");
            strSql.Append("state=9,p_active=1,p_getMoney=payCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到某赛事各项目的记录数
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("PayType=@PayType and ");
            strSql.Append("itypes>=@itypes ");//itypes=@itypes
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = ipaytype;
            parameters[3].Value = 0;

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
        /// 得到某ID在某赛事各项目的记录数
        /// </summary>
        public int GetCount(int ibcid, int iptype, int ipaytype, int iusid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("payusid=@payusid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("PayType=@PayType and ");
            strSql.Append("itypes=@itypes ");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
                    new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iusid;
            parameters[2].Value = iptype;
            parameters[3].Value = ipaytype;
            parameters[4].Value = 0;

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
        /// 得到某一赛事下注总注数
        /// </summary>
        public int GetBaPayNum(int ibcid, int iptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("itypes=@itypes ");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = 0;

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
        /// 得到某一赛事下注总金额
        /// </summary>
        public long GetBaPayCent(int ibcid, int iptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("itypes=@itypes ");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = 0;
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
        /// 得到某一赛事某ID下注总金额
        /// </summary>
        public long GetBaPayCent(int ibcid, int iptype, int payusid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_ZBaPay where ");
            strSql.Append("bcid=@bcid and ");
            strSql.Append("pType=@pType and ");
            strSql.Append("itypes=@itypes and ");
            strSql.Append("payusid=@payusid ");
            SqlParameter[] parameters = {
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4),
					new SqlParameter("@payusid", SqlDbType.Int,4)};
            parameters[0].Value = ibcid;
            parameters[1].Value = iptype;
            parameters[2].Value = 0;
            parameters[3].Value = payusid;
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
        /// 根据条件得到赛事下注总注数
        /// </summary>
        public int GetBaPayCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_ZBaPay where " + strWhere + " and itypes=0");

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
        /// 根据条件得到赛事下注总金额
        /// </summary>
        public long GetBaPaypayCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(payCent) from tb_ZBaPay where " + strWhere + " and itypes=0");

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
        /// 根据条件得到赛事下注盈利额
        /// </summary>
        public long GetBaPaygetMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(p_getMoney) from tb_ZBaPay where " + strWhere + " and itypes=0");

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
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsIsCase(int ID, int payusid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZBaPay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and p_active>@p_active ");
            strSql.Append(" and payusid=@payusid ");
            strSql.Append(" and p_getMoney>@p_getMoney ");
            strSql.Append(" and p_case=@p_case ");
            strSql.Append(" and itypes=@itypes ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@p_active", SqlDbType.Int,4),
        			new SqlParameter("@payusid", SqlDbType.Int,4),
    				new SqlParameter("@p_getMoney", SqlDbType.Int,4),
					new SqlParameter("@p_case", SqlDbType.Int,4),
					new SqlParameter("@itypes", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;
            parameters[2].Value = payusid;
            parameters[3].Value = 0;
            parameters[4].Value = 0;
            parameters[5].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdateIsCase(int ID)
        {

            //更新此记录得到的币
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaPay set ");
            strSql.Append("p_case=@p_case");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_case", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新走地下注为正常
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void Updatestate(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaPay set ");
            strSql.Append("state=@state");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@state", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新平盘业务
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void UpdatePPCase(TPR3.Model.guess.BaPay model)
        {

            //更新此记录得到的币
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaPay set ");
            strSql.Append("p_getMoney=@p_getMoney,");
            strSql.Append("p_case=@p_case,");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_getMoney", SqlDbType.Money,8),
                new SqlParameter("@p_case", SqlDbType.Int,4),
                new SqlParameter("@p_result_one", SqlDbType.Int,4),
                new SqlParameter("@p_result_two", SqlDbType.Int,4),
                new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.p_getMoney;
            parameters[2].Value = 0;
            parameters[3].Value = model.p_result_one;
            parameters[4].Value = model.p_result_two;
            parameters[5].Value = model.p_active;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新开奖业务
        /// </summary>
        /// <param name="model"></param>
        /// <param name="p_strVal"></param>
        /// <returns></returns>
        public void UpdateCase(TPR3.Model.guess.BaPay model, string p_strVal, out decimal p_intDuVal, out int p_intWin)
        {
            string[] strVal = { };
            decimal duVal = 0;
            if (!string.IsNullOrEmpty(p_strVal))
            {
                strVal = p_strVal.Split("|".ToCharArray());
                duVal = Convert.ToDecimal(strVal[0]);
                p_intWin = 1;
                p_intDuVal = duVal;
            }
            else
            {
                p_intWin = 0;
                p_intDuVal = 0;
            }

            //更新此记录得到的币
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ZBaPay set ");
            strSql.Append("p_getMoney=@p_getMoney,");
            strSql.Append("p_case=@p_case,");
            strSql.Append("p_result_one=@p_result_one,");
            strSql.Append("p_result_two=@p_result_two,");
            strSql.Append("p_active=@p_active");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@p_getMoney", SqlDbType.Money,8),
                new SqlParameter("@p_case", SqlDbType.Int,4),
                new SqlParameter("@p_result_one", SqlDbType.Int,4),
                new SqlParameter("@p_result_two", SqlDbType.Int,4),
                new SqlParameter("@p_active", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = duVal;
            parameters[2].Value = 0;
            parameters[3].Value = model.p_result_one;
            parameters[4].Value = model.p_result_two;
            parameters[5].Value = model.p_active;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(TPR3.Model.guess.BaPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ZBaPay(");
            strSql.Append("payview,payusid,payusname,bcid,pType,PayType,payCent,payonLuone,payonLutwo,payonLuthr,p_pk,p_dx_pk,p_pn,paytimes,p_result_temp1,p_result_temp2,itypes,types,state,p_oncetime2,p_TPRtime)");
            strSql.Append(" values (");
            strSql.Append("@payview,@payusid,@payusname,@bcid,@pType,@PayType,@payCent,@payonLuone,@payonLutwo,@payonLuthr,@p_pk,@p_dx_pk,@p_pn,@paytimes,@p_result_temp1,@p_result_temp2,@itypes,@types,@state,@p_oncetime2,@p_TPRtime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@payview", SqlDbType.NVarChar,500),
					new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@payusname", SqlDbType.NVarChar,500),
					new SqlParameter("@bcid", SqlDbType.Int,4),
					new SqlParameter("@pType", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.Money,8),
					new SqlParameter("@payonLuone", SqlDbType.Money,8),
					new SqlParameter("@payonLutwo", SqlDbType.Money,8),
					new SqlParameter("@payonLuthr", SqlDbType.Money,8),
					new SqlParameter("@p_pk", SqlDbType.Money,8),
					new SqlParameter("@p_dx_pk", SqlDbType.Money,8),
					new SqlParameter("@p_pn", SqlDbType.Int,4),
					new SqlParameter("@paytimes", SqlDbType.DateTime),
                    new SqlParameter("@p_result_temp1", SqlDbType.Int,4),			
                    new SqlParameter("@p_result_temp2", SqlDbType.Int,4),
                    new SqlParameter("@itypes", SqlDbType.Int,4),
                    new SqlParameter("@types", SqlDbType.Int,4),
                     new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@p_oncetime2", SqlDbType.DateTime),
					new SqlParameter("@p_TPRtime", SqlDbType.DateTime)};
            parameters[0].Value = model.payview;
            parameters[1].Value = model.payusid;
            parameters[2].Value = model.payusname;
            parameters[3].Value = model.bcid;
            parameters[4].Value = model.pType;
            parameters[5].Value = model.PayType;
            parameters[6].Value = model.payCent;
            parameters[7].Value = model.payonLuone;
            parameters[8].Value = model.payonLutwo;
            parameters[9].Value = model.payonLuthr;
            parameters[10].Value = model.p_pk;
            parameters[11].Value = model.p_dx_pk;
            parameters[12].Value = model.p_pn;
            parameters[13].Value = model.paytimes;
            parameters[14].Value = model.p_result_temp1;
            parameters[15].Value = model.p_result_temp2;
            parameters[16].Value = model.itypes;
            parameters[17].Value = model.Types;
            parameters[18].Value = model.state;
            parameters[19].Value = model.p_oncetime2;
            parameters[20].Value = model.p_TPRtime;
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
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaPay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据,根据赛事ID
        /// </summary>
        public void Deletebcid(int gid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ZBaPay ");
            strSql.Append(" where bcid=@gid ");
            SqlParameter[] parameters = {
					new SqlParameter("@gid", SqlDbType.Int,4)};
            parameters[0].Value = gid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个p_getMoney
        /// </summary>
        public decimal Getp_getMoney(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 p_getMoney from tb_ZBaPay ");
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
                        return reader.GetDecimal(0);
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
        /// 得到一个Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_ZBaPay ");
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
        public TPR3.Model.guess.BaPay GetModelIsCase(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,payview,p_getMoney,pType,bcid,types,paycent,payusid,payusname from tb_ZBaPay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR3.Model.guess.BaPay model = new TPR3.Model.guess.BaPay();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                model.payview = ds.Tables[0].Rows[0]["payview"].ToString();

                if (ds.Tables[0].Rows[0]["p_getMoney"].ToString() != "")
                {
                    model.p_getMoney = decimal.Parse(ds.Tables[0].Rows[0]["p_getMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["pType"].ToString() != "")
                {
                    model.pType = int.Parse(ds.Tables[0].Rows[0]["pType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["bcid"].ToString() != "")
                {
                    model.bcid = int.Parse(ds.Tables[0].Rows[0]["bcid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["types"].ToString() != "")
                {
                    model.Types = int.Parse(ds.Tables[0].Rows[0]["types"].ToString());
                }
                if (ds.Tables[0].Rows[0]["paycent"].ToString() != "")
                {
                    model.payCent = decimal.Parse(ds.Tables[0].Rows[0]["paycent"].ToString());
                }
                if (ds.Tables[0].Rows[0]["payusid"].ToString() != "")
                {
                    model.payusid = int.Parse(ds.Tables[0].Rows[0]["payusid"].ToString());
                }
                model.payusname = ds.Tables[0].Rows[0]["payusname"].ToString();
                return model;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaPayList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_ZBaPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetBaPayList(string strField, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_ZBaPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPay</returns>
        public IList<TPR3.Model.guess.BaPay> GetBaPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaPay> listBaPays = new List<TPR3.Model.guess.BaPay>();


            string sTable = "tb_ZBaPay";
            string sPkey = "id";
            string sField = "id,payview,payusid,payusname,pType,PayType,payCent,payonLuone,payonLutwo,payonLuthr,p_pk,p_dx_pk,p_pn,p_result_one,p_result_two,p_getMoney,paytimes,p_active,p_result_temp1,p_result_temp2,itypes,types,state";
            string sCondition = strWhere;
            string sOrder = "id desc";
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

                    return listBaPays;
                }

                while (reader.Read())
                {
                    TPR3.Model.guess.BaPay objBaPay = new TPR3.Model.guess.BaPay();
                    objBaPay.ID = reader.GetInt32(0);
                    objBaPay.payview = reader.GetString(1);
                    objBaPay.payusid = reader.GetInt32(2);
                    objBaPay.payusname = reader.GetString(3);
                    objBaPay.pType = reader.GetInt32(4);
                    objBaPay.PayType = reader.GetInt32(5);
                    objBaPay.payCent = reader.GetDecimal(6);
                    objBaPay.payonLuone = reader.GetDecimal(7);
                    objBaPay.payonLutwo = reader.GetDecimal(8);
                    objBaPay.payonLuthr = reader.GetDecimal(9);
                    objBaPay.p_pk = reader.GetDecimal(10);
                    objBaPay.p_dx_pk = reader.GetDecimal(11);
                    objBaPay.p_pn = reader.GetInt32(12);

                    if (reader.IsDBNull(13))
                        objBaPay.p_result_one = null;
                    else
                        objBaPay.p_result_one = reader.GetInt32(13);

                    if (reader.IsDBNull(14))
                        objBaPay.p_result_two = null;
                    else
                        objBaPay.p_result_two = reader.GetInt32(14);

                    objBaPay.p_getMoney = reader.GetDecimal(15);
                    objBaPay.paytimes = reader.GetDateTime(16);
                    objBaPay.p_active = reader.GetInt32(17);
                    if (reader.IsDBNull(18))
                        objBaPay.p_result_temp1 = null;
                    else
                        objBaPay.p_result_temp1 = reader.GetInt32(18);

                    if (reader.IsDBNull(19))
                        objBaPay.p_result_temp2 = null;
                    else
                        objBaPay.p_result_temp2 = reader.GetInt32(19);

                    objBaPay.itypes = reader.GetInt32(20);
                    objBaPay.Types = reader.GetInt32(21);

                    if (reader.IsDBNull(22))
                        objBaPay.state = 0;
                    else
                        objBaPay.state = reader.GetInt32(22);

                    listBaPays.Add(objBaPay);


                }
            }

            return listBaPays;
        }


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPayView</returns>
        public IList<TPR3.Model.guess.BaPay> GetBaPayViews(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaPay> listBaPayViews = new List<TPR3.Model.guess.BaPay>();

            if (strWhere != "")
                strWhere = strWhere + " and itypes=0";
            else
                strWhere = "itypes=0";

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT payusid) FROM tb_ZBaPay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBaPayViews;
            }

            // 取出相关记录
            string queryString = "";
            if (itype == 0)
            {
                queryString = "SELECT payusid,payusname,count(payusid) as payCount,sum(payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid,payusname Order by sum(payCent) desc";
            }
            else
            {
                queryString = "SELECT payusid,payusname,sum(p_getMoney) as payCount,sum(payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid,payusname Order by (sum(p_getMoney)-sum(payCent)) desc";
            }
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR3.Model.guess.BaPay objBaPay = new TPR3.Model.guess.BaPay();
                        objBaPay.payusid = reader.GetInt32(0);
                        objBaPay.payusname = reader.GetString(1);
                        if (itype == 0)
                            objBaPay.payCount = Convert.ToDecimal(reader.GetInt32(2));
                        else
                            objBaPay.payCount = reader.GetDecimal(2);

                        objBaPay.payCents = reader.GetDecimal(3);
                        listBaPayViews.Add(objBaPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBaPayViews;
        }


        /// <summary>
        /// 取得详细排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPayTop</returns>
        public IList<TPR3.Model.guess.BaPay> GetBaPayTop(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaPay> listBaPayTop = new List<TPR3.Model.guess.BaPay>();

            if (strWhere != "")
                strWhere = strWhere + " and itypes=0";
            else
                strWhere = "itypes=0";

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT payusid) FROM tb_ZBaPay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBaPayTop;
            }

            // 取出相关记录
            string queryString = "";
            if (itype != 1)
            {
                queryString = "SELECT payusid,count(payusid) as payCount,sum(payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid Order by Count(ID) desc";
            }
            else
            {
                queryString = "SELECT payusid,sum(p_getMoney-payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid Order by sum(p_getMoney-payCent) desc";
            }
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR3.Model.guess.BaPay objBaPay = new TPR3.Model.guess.BaPay();
                        objBaPay.payusid = reader.GetInt32(0);
                        if (itype != 1)
                            objBaPay.payCount = Convert.ToDecimal(reader.GetInt32(1));
                        else
                            objBaPay.payCount = reader.GetDecimal(1);

                        listBaPayTop.Add(objBaPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBaPayTop;
        }

        /// <summary>
        /// 取得详细排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList BaPayTop</returns>
        public IList<TPR3.Model.guess.BaPay> GetBaPayTop2(int p_pageIndex, int p_pageSize, string strWhere, int itype, out int p_recordCount)
        {
            IList<TPR3.Model.guess.BaPay> listBaPayTop = new List<TPR3.Model.guess.BaPay>();

            if (strWhere != "")
                strWhere = strWhere + " and itypes=0 and Types=0";
            else
                strWhere = "itypes=0 and Types=0";

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT payusid) FROM tb_ZBaPay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                //if (p_recordCount > 100)
                //    p_recordCount = 100;

                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBaPayTop;
            }

            // 取出相关记录
            string queryString = "";
            if (itype != 1)
            {
                queryString = "SELECT payusid,count(payusid) as payCount,sum(payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid Order by Count(ID) desc";
            }
            else
            {
                queryString = "SELECT payusid,sum(p_getMoney-payCent) as payCents FROM tb_ZBaPay where " + strWhere + " group by payusid Order by sum(p_getMoney-payCent) desc";
            }
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        TPR3.Model.guess.BaPay objBaPay = new TPR3.Model.guess.BaPay();
                        objBaPay.payusid = reader.GetInt32(0);
                        if (itype != 1)
                            objBaPay.payCount = Convert.ToDecimal(reader.GetInt32(1));
                        else
                            objBaPay.payCount = reader.GetDecimal(1);

                        listBaPayTop.Add(objBaPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBaPayTop;
        }


        #endregion  成员方法
    }
}

