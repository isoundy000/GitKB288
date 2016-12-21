using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
/// <summary>
/// 增加环迅支付接口
/// 
/// 黄国军20160512
/// </summary>
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Payrmb。
    /// </summary>
    public class Payrmb
    {
        public Payrmb()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Payrmb");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Payrmb");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
            		new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该订单记录
        /// </summary>
        public bool Exists(string CardOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Payrmb");
            strSql.Append(" where CardOrder=@CardOrder ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@CardOrder", SqlDbType.NVarChar,50),
            		new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = CardOrder;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Payrmb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Payrmb(");
            strSql.Append("UsID,UsName,Types,CardAmt,CardNum,CardPwd,CardOrder,State,AddUsIP,AddTime,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@Types,@CardAmt,@CardNum,@CardPwd,@CardOrder,@State,@AddUsIP,@AddTime,@MerBillNo,@Amount,@GatewayType,@Attach,@BillEXP,@GoodsName,@IsCredit,@BankCode,@ProductType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@CardAmt", SqlDbType.Int,4),
                    new SqlParameter("@CardNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
                    new SqlParameter("@CardOrder", SqlDbType.NVarChar,50),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@MerBillNo", SqlDbType.NVarChar,30),
                    new SqlParameter("@Amount", SqlDbType.Money),
                    new SqlParameter("@GatewayType", SqlDbType.NVarChar,50),
                    new SqlParameter("@Attach", SqlDbType.NVarChar,50),
                    new SqlParameter("@BillEXP", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IsCredit", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductType", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Types;
            parameters[3].Value = model.CardAmt;
            parameters[4].Value = model.CardNum;
            parameters[5].Value = model.CardPwd;
            parameters[6].Value = model.CardOrder;
            parameters[7].Value = model.State;
            parameters[8].Value = model.AddUsIP;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.MerBillNo;
            parameters[11].Value = model.Amount;
            parameters[12].Value = model.GatewayType;
            parameters[13].Value = model.Attach;
            parameters[14].Value = model.BillEXP;
            parameters[15].Value = model.GoodsName;
            parameters[16].Value = model.IsCredit;
            parameters[17].Value = model.BankCode;
            parameters[18].Value = model.ProductType;

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
        /// 充值返回更新标识
        /// </summary>
        public void Update_ips(BCW.Model.Payrmb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Payrmb set ");
            strSql.Append("GatewayType=@GatewayType,");
            strSql.Append("Attach=@Attach,");
            strSql.Append("BankCode=@BankCode,");
            strSql.Append("ProductType=@ProductType,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@GatewayType", SqlDbType.NVarChar,50),
                    new SqlParameter("@Attach", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductType",SqlDbType.NVarChar,50),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};

            parameters[0].Value = model.GatewayType;
            parameters[1].Value = model.Attach;
            parameters[2].Value = model.BankCode;
            parameters[3].Value = model.ProductType;
            parameters[4].Value = model.State;
            parameters[5].Value = model.ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Payrmb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Payrmb set ");
            strSql.Append("CardAmt=@CardAmt,");
            strSql.Append("State=@State");
            strSql.Append(" where CardOrder=@CardOrder ");
            SqlParameter[] parameters = {
					new SqlParameter("@CardOrder", SqlDbType.NVarChar,50),
					new SqlParameter("@CardAmt", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};

            parameters[0].Value = model.CardOrder;
            parameters[1].Value = model.CardAmt;
            parameters[2].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update3(BCW.Model.Payrmb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Payrmb set ");
            strSql.Append("CardAmt=@CardAmt,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@CardAmt", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};

            parameters[0].Value = model.ID;
            parameters[1].Value = model.CardAmt;
            parameters[2].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Payrmb model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Payrmb set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Types=@Types,");
            strSql.Append("CardAmt=@CardAmt,");
            strSql.Append("CardNum=@CardNum,");
            strSql.Append("CardPwd=@CardPwd,");
            strSql.Append("CardOrder=@CardOrder,");
            strSql.Append("State=@State,");
            strSql.Append("AddUsIP=@AddUsIP,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@CardAmt", SqlDbType.Int,4),
					new SqlParameter("@CardNum", SqlDbType.NVarChar,50),
					new SqlParameter("@CardPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@CardOrder", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Types;
            parameters[4].Value = model.CardAmt;
            parameters[5].Value = model.CardNum;
            parameters[6].Value = model.CardPwd;
            parameters[7].Value = model.CardOrder;
            parameters[8].Value = model.State;
            parameters[9].Value = model.AddUsIP;
            parameters[10].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Payrmb ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到用户ID
        /// </summary>
        public int GetUsID(string CardOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsID from tb_Payrmb ");
            strSql.Append(" where CardOrder=@CardOrder ");
            SqlParameter[] parameters = {
					new SqlParameter("@CardOrder", SqlDbType.NVarChar,50)};
            parameters[0].Value = CardOrder;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Payrmb GetPayrmb(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,Types,CardAmt,CardNum,CardPwd,CardOrder,State,AddUsIP,AddTime,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType from tb_Payrmb ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Payrmb model = new BCW.Model.Payrmb();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.Types = reader.GetInt32(3);
                    model.CardAmt = reader.GetInt32(4);
                    model.CardNum = reader.GetString(5);
                    model.CardPwd = reader.GetString(6);
                    model.CardOrder = reader.GetString(7);
                    model.State = reader.GetInt32(8);
                    model.AddUsIP = reader.GetString(9);
                    model.AddTime = reader.GetDateTime(10);
                    model.MerBillNo = reader.GetString(11);
                    model.Amount = reader.GetDecimal(12);
                    model.GatewayType = reader.GetString(13);
                    model.Attach = reader.GetString(14);
                    model.BillEXP = reader.GetInt32(15);
                    model.GoodsName = reader.GetString(16);
                    model.IsCredit = reader.GetString(17);
                    model.BankCode = reader.GetString(18);
                    model.ProductType = reader.GetString(19);
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
            strSql.Append(" FROM tb_Payrmb ");
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
        /// <returns>IList Payrmb</returns>
        public IList<BCW.Model.Payrmb> GetPayrmbs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Payrmb> listPayrmbs = new List<BCW.Model.Payrmb>();
            string sTable = "tb_Payrmb";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,Types,CardAmt,CardNum,CardPwd,CardOrder,State,AddUsIP,AddTime,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType";
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
                    return listPayrmbs;
                }
                while (reader.Read())
                {
                    BCW.Model.Payrmb objPayrmb = new BCW.Model.Payrmb();
                    objPayrmb.ID = reader.GetInt32(0);
                    objPayrmb.UsID = reader.GetInt32(1);
                    objPayrmb.UsName = reader.GetString(2);
                    objPayrmb.Types = reader.GetInt32(3);
                    objPayrmb.CardAmt = reader.GetInt32(4);
                    objPayrmb.CardNum = reader.GetString(5);
                    objPayrmb.CardPwd = reader.GetString(6);
                    objPayrmb.CardOrder = reader.GetString(7);
                    objPayrmb.State = reader.GetInt32(8);
                    objPayrmb.AddUsIP = reader.GetString(9);
                    objPayrmb.AddTime = reader.GetDateTime(10);
                    if (reader.GetInt32(3) == 100)
                    {
                        objPayrmb.MerBillNo = reader.GetString(11);
                        objPayrmb.Amount = reader.GetDecimal(12);
                        objPayrmb.GatewayType = reader.GetString(13);
                        objPayrmb.Attach = reader.GetString(14);
                        objPayrmb.BillEXP = reader.GetInt32(15);
                        objPayrmb.GoodsName = reader.GetString(16);
                        objPayrmb.IsCredit = reader.GetString(17);
                        objPayrmb.BankCode = reader.GetString(18);
                        objPayrmb.ProductType = reader.GetString(19);
                    }
                    listPayrmbs.Add(objPayrmb);
                }
            }
            return listPayrmbs;
        }

        #endregion  成员方法
    }
}
