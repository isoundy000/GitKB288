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
    /// 数据访问类SellNum。
    /// </summary>
    public class SellNum
    {
        public SellNum()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_SellNum");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SellNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Types, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SellNum");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该提交记录
        /// </summary>
        public bool Exists(int Types, int BuyUID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SellNum");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and BuyUID=@BuyUID ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@BuyUID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = Types;
            parameters[1].Value = BuyUID;
            parameters[2].Value = State;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该提交记录
        /// </summary>
        public bool Exists(int Types, int UsID, int BuyUID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SellNum");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and BuyUID=@BuyUID ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BuyUID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = BuyUID;
            parameters[3].Value = State;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某会员今天兑换了多少充值或Q币
        /// </summary>
        public int GetSumBuyUID(int Types, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sum(BuyUID) from tb_SellNum");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
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
        /// 计算某会员今天查询多少次报价
        /// </summary>
        public int GetCount(int Types, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(ID) from tb_SellNum");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
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
        /// 计算某QQ号在6个月内每个服务开通的月份数量
        /// </summary>
        public int GetSumBuyUIDQQ(int Tags, string Mobile, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Sum(BuyUID) from tb_SellNum");
            strSql.Append(" where Tags=@Tags ");
            strSql.Append(" and Mobile=@Mobile ");
            strSql.Append(" and UsID=@UsID and Types=3 and AddTime>'" + DateTime.Now.AddMonths(-6) + "'");
            SqlParameter[] parameters = {
					new SqlParameter("@Tags", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Tags;
            parameters[1].Value = Mobile;
            parameters[2].Value = UsID;
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
        /// 每个ID每月只能为2个QQ号进行开通服务
        /// </summary>
        public int GetSumQQCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(DISTINCT Mobile) from tb_SellNum");
            strSql.Append(" where UsID=@UsID and Types=3 and AddTime>'" + DateTime.Now.AddMonths(-1) + "'");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
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
        public int Add(BCW.Model.SellNum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SellNum(");
            strSql.Append("Types,UsID,UsName,BuyUID,Price,State,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@BuyUID,@Price,@State,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyUID", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BuyUID;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.State;
            parameters[6].Value = model.AddTime;

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
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.SellNum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SellNum(");
            strSql.Append("Types,UsID,UsName,BuyUID,Price,State,Mobile,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@BuyUID,@Price,@State,@Mobile,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyUID", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.BuyUID;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.State;
            parameters[6].Value = model.Mobile;
            parameters[7].Value = model.AddTime;

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
        public void Update(BCW.Model.SellNum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("Types=@Types,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BuyUID=@BuyUID,");
            strSql.Append("Price=@Price,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("PayTime=@PayTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BuyUID", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PayTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BuyUID;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.State;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.PayTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新已报价
        /// </summary>
        public void UpdateState2(int ID, long Price)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("State=@State,");
            strSql.Append("Price=@Price,");
            strSql.Append("PayTime=@PayTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@PayTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 2;
            parameters[2].Value = Price;
            parameters[3].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新成为已申请兑换
        /// </summary>
        public void UpdateState3(int ID, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("State=@State,");
            strSql.Append("Mobile=@Mobile,");
            strSql.Append("PayTime=@PayTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@PayTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 3;
            parameters[2].Value = Mobile;
            parameters[3].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新成为已成功
        /// </summary>
        public void UpdateState4(int ID, string Notes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("State=@State,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("PayTime=@PayTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@PayTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 4;
            parameters[2].Value = Notes;
            parameters[3].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新成为已撤销
        /// </summary>
        public void UpdateState9(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = 9;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新Notes
        /// </summary>
        public void UpdateNotes(int ID, string Notes)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("Notes=@Notes,");
            strSql.Append("PayTime=@PayTime,State=3");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@PayTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = Notes;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新QQ服务类型
        /// </summary>
        public void UpdateTags(int ID, int Tags)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SellNum set ");
            strSql.Append("Tags=@Tags");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Tags;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SellNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.SellNum GetSellNum(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,BuyUID,Price,State,Mobile,Notes,AddTime,Tags,PayTime from tb_SellNum ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.SellNum model = new BCW.Model.SellNum();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.BuyUID = reader.GetInt32(4);
                    model.Price = reader.GetInt64(5);
                    model.State = reader.GetByte(6);
                    if (!reader.IsDBNull(7))
                        model.Mobile = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        model.Notes = reader.GetString(8);

                    model.AddTime = reader.GetDateTime(9);
                    model.Tags = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.PayTime = reader.GetDateTime(11);
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
            strSql.Append(" FROM tb_SellNum ");
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
        /// <returns>IList SellNum</returns>
        public IList<BCW.Model.SellNum> GetSellNums(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.SellNum> listSellNums = new List<BCW.Model.SellNum>();
            string sTable = "tb_SellNum";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,BuyUID,Price,State,Mobile,Notes,AddTime,Tags,PayTime";
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
                    return listSellNums;
                }
                while (reader.Read())
                {
                    BCW.Model.SellNum objSellNum = new BCW.Model.SellNum();
                    objSellNum.ID = reader.GetInt32(0);
                    objSellNum.Types = reader.GetInt32(1);
                    objSellNum.UsID = reader.GetInt32(2);
                    objSellNum.UsName = reader.GetString(3);
                    objSellNum.BuyUID = reader.GetInt32(4);
                    objSellNum.Price = reader.GetInt64(5);
                    objSellNum.State = reader.GetByte(6);

                    if (!reader.IsDBNull(7))
                        objSellNum.Mobile = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        objSellNum.Notes = reader.GetString(8);

                    objSellNum.AddTime = reader.GetDateTime(9);
                    objSellNum.Tags = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        objSellNum.PayTime = reader.GetDateTime(11);

                    listSellNums.Add(objSellNum);
                }
            }
            return listSellNums;
        }

        #endregion  成员方法
    }
}

