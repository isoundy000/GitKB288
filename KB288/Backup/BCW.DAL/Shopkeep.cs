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
    /// 数据访问类Shopkeep。
    /// </summary>
    public class Shopkeep
    {
        public Shopkeep()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Shopkeep");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopkeep");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int GiftId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopkeep");
            strSql.Append(" where GiftId=@GiftId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = GiftId;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopkeep");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件得到ID
        /// </summary>
        public int GetID(int GiftId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_Shopkeep");
            strSql.Append(" where GiftId=@GiftId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = GiftId;
            parameters[1].Value = UsID;

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
        /// 计算某礼物-某用户今天购买数量
        /// </summary>
        public int GetTodayCount(int UsID, int GiftId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(TopTotal) from tb_Shopkeep");
            strSql.Append(" where GiftId=@GiftId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = GiftId;
            parameters[1].Value = UsID;

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
        public int Add(BCW.Model.Shopkeep model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Shopkeep(");
            strSql.Append("GiftId,Title,Notes,Pic,PrevPic,UsID,UsName,Total,Para,IsSex,AddTime,TopTotal,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType,State,NodeId)");
            strSql.Append(" values (");
            strSql.Append("@GiftId,@Title,@Notes,@Pic,@PrevPic,@UsID,@UsName,@Total,@Para,@IsSex,@AddTime,@TopTotal,@MerBillNo,@Amount,@GatewayType,@Attach,@BillEXP,@GoodsName,@IsCredit,@BankCode,@ProductType,@State,@NodeId)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@Pic", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Para", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@TopTotal", SqlDbType.Int,4),
                    new SqlParameter("@MerBillNo", SqlDbType.NVarChar,30),
                    new SqlParameter("@Amount", SqlDbType.Money),
                    new SqlParameter("@GatewayType", SqlDbType.NVarChar,50),
                    new SqlParameter("@Attach", SqlDbType.NVarChar,50),
                    new SqlParameter("@BillEXP", SqlDbType.Int),
                    new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@IsCredit", SqlDbType.NVarChar,50),
                    new SqlParameter("@BankCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@ProductType", SqlDbType.NVarChar,50),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = model.GiftId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.Pic;
            parameters[4].Value = model.PrevPic;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.Total;
            parameters[8].Value = model.Para;
            parameters[9].Value = model.IsSex;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.TopTotal;
            parameters[12].Value = model.MerBillNo;
            parameters[13].Value = model.Amount;
            parameters[14].Value = model.GatewayType;
            parameters[15].Value = model.Attach;
            parameters[16].Value = model.BillEXP;
            parameters[17].Value = model.GoodsName;
            parameters[18].Value = model.IsCredit;
            parameters[19].Value = model.BankCode;
            parameters[20].Value = model.ProductType;
            parameters[21].Value = model.State;
            parameters[22].Value = model.NodeId;
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
        public void Update(BCW.Model.Shopkeep model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopkeep set ");
            strSql.Append("Title=@Title,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Pic=@Pic,");
            strSql.Append("PrevPic=@PrevPic,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Total=Total+@Total,");
            strSql.Append("Para=@Para,");
            strSql.Append("IsSex=@IsSex,");
            strSql.Append("AddTime=@AddTime, ");
            strSql.Append("TopTotal=TopTotal+@TopTotal ");
            strSql.Append("where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId");
            SqlParameter[] parameters = {
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@Pic", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Para", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@TopTotal", SqlDbType.Int,4)};
            parameters[0].Value = model.GiftId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.Pic;
            parameters[4].Value = model.PrevPic;
            parameters[5].Value = model.UsID;
            parameters[6].Value = model.UsName;
            parameters[7].Value = model.Total;
            parameters[8].Value = model.Para;
            parameters[9].Value = model.IsSex;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.TopTotal;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 充值返回更新标识
        /// </summary>
        public void Update_ips(BCW.Model.Shopkeep model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopkeep set ");
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
        public void UpdateTotal(int ID, int Total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopkeep set ");
            strSql.Append("Total=Total+@Total ");
            strSql.Append("where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Total", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Total;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Shopkeep ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Shopkeep GetShopkeep(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,GiftId,Title,Notes,Pic,PrevPic,UsID,UsName,Total,Para,IsSex,AddTime,NodeId,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType,State from tb_Shopkeep ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Shopkeep model = new BCW.Model.Shopkeep();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.GiftId = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.Notes = reader.GetString(3);

                    model.Pic = reader.GetString(4);
                    model.PrevPic = reader.GetString(5);
                    model.UsID = reader.GetInt32(6);
                    model.UsName = reader.GetString(7);
                    model.Total = reader.GetInt32(8);
                    model.Para = reader.GetString(9);
                    model.IsSex = reader.GetByte(10);
                    model.AddTime = reader.GetDateTime(11);
                    model.NodeId = reader.GetInt32(12);
                    if (reader.GetInt32(12) == 28 && !reader.IsDBNull(12))
                    {
                        model.MerBillNo = reader.GetString(13);
                        model.Amount = reader.GetDecimal(14);
                        model.GatewayType = reader.GetString(15);
                        model.Attach = reader.GetString(16);
                        model.BillEXP = reader.GetInt32(17);
                        model.GoodsName = reader.GetString(18);
                        model.IsCredit = reader.GetString(19);
                        model.BankCode = reader.GetString(20);
                        model.ProductType = reader.GetString(21);
                        model.State = reader.GetInt32(22);
                    }
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
            strSql.Append(" FROM tb_Shopkeep ");
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
        /// <returns>IList Shopkeep</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeeps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Shopkeep> listShopkeeps = new List<BCW.Model.Shopkeep>();
            string sTable = "tb_Shopkeep";
            string sPkey = "id";
            string sField = "ID,GiftId,Title,Notes,Pic,PrevPic,UsID,Total,Para,AddTime,NodeId,MerBillNo,Amount,GatewayType,Attach,BillEXP,GoodsName,IsCredit,BankCode,ProductType,State,UsName";
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
                    return listShopkeeps;
                }
                while (reader.Read())
                {
                    BCW.Model.Shopkeep objShopkeep = new BCW.Model.Shopkeep();
                    objShopkeep.ID = reader.GetInt32(0);
                    objShopkeep.GiftId = reader.GetInt32(1);
                    objShopkeep.Title = reader.GetString(2);
                    objShopkeep.Notes = reader.GetString(3);
                    objShopkeep.Pic = reader.GetString(4);
                    objShopkeep.PrevPic = reader.GetString(5);
                    objShopkeep.UsID = reader.GetInt32(6);
                    objShopkeep.Total = reader.GetInt32(7);
                    objShopkeep.Para = reader.GetString(8);
                    objShopkeep.AddTime = reader.GetDateTime(9);
                    objShopkeep.NodeId = reader.GetInt32(10);
                    if (reader.GetInt32(10) == 28 && !reader.IsDBNull(10))
                    {
                        objShopkeep.MerBillNo = reader.GetString(11);
                        objShopkeep.Amount = reader.GetDecimal(12);
                        objShopkeep.GatewayType = reader.GetString(13);
                        objShopkeep.Attach = reader.GetString(14);
                        objShopkeep.BillEXP = reader.GetInt32(15);
                        objShopkeep.GoodsName = reader.GetString(16);
                        objShopkeep.IsCredit = reader.GetString(17);
                        objShopkeep.BankCode = reader.GetString(18);
                        objShopkeep.ProductType = reader.GetString(19);
                        objShopkeep.State = reader.GetInt32(20);
                        objShopkeep.UsName = reader.GetString(21);
                    }
                    listShopkeeps.Add(objShopkeep);
                }
            }
            return listShopkeeps;
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Shopkeep> listShopkeep = new List<BCW.Model.Shopkeep>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT GiftId) FROM tb_Shopkeep Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listShopkeep;
            }

            // 取出相关记录

            string queryString = "SELECT GiftId,Title,PrevPic,Sum(TopTotal) as TopTotal FROM tb_Shopkeep Where " + strWhere + " GROUP BY GiftId,Title,PrevPic ORDER BY Sum(TopTotal) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Shopkeep objShopkeep = new BCW.Model.Shopkeep();
                        objShopkeep.GiftId = reader.GetInt32(0);
                        objShopkeep.Title = reader.GetString(1);
                        objShopkeep.PrevPic = reader.GetString(2);
                        objShopkeep.TopTotal = reader.GetInt32(3);

                        listShopkeep.Add(objShopkeep);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listShopkeep;
        }

        /// <summary>
        /// 排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopkeep> GetShopkeepsTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Shopkeep> listShopkeep = new List<BCW.Model.Shopkeep>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Shopkeep Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listShopkeep;
            }

            // 取出相关记录

            string queryString = "SELECT UsID,UsName,Sum(TopTotal) as TopTotal FROM tb_Shopkeep Where " + strWhere + " GROUP BY UsID,UsName ORDER BY Sum(TopTotal) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Shopkeep objShopkeep = new BCW.Model.Shopkeep();
                        objShopkeep.UsID = reader.GetInt32(0);
                        objShopkeep.UsName = reader.GetString(1);
                        objShopkeep.TopTotal = reader.GetInt32(2);

                        listShopkeep.Add(objShopkeep);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listShopkeep;
        }

        #endregion  成员方法
    }
}
