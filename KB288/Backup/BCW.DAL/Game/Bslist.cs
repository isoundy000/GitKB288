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
    /// 数据访问类Bslist。
    /// </summary>
    public class Bslist
    {
        public Bslist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Bslist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Bslist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Bslist(");
            strSql.Append("Title,Money,SmallPay,BigPay,UsID,UsName,Click,BzType,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Title,@Money,@SmallPay,@BigPay,@UsID,@UsName,@Click,@BzType,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Money", SqlDbType.BigInt,8),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.Money;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Click;
            parameters[7].Value = model.BzType;
            parameters[8].Value = model.AddTime;

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
        public void Update(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Title=@Title,");
            strSql.Append("SmallPay=@SmallPay,");
            strSql.Append("BigPay=@BigPay,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Click=@Click,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Click;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBasic(BCW.Model.Game.Bslist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Title=@Title,");
            strSql.Append("SmallPay=@SmallPay,");
            strSql.Append("BigPay=@BigPay,");
            strSql.Append("UsName=@UsName");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SmallPay", SqlDbType.BigInt,8),
					new SqlParameter("@BigPay", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.SmallPay;
            parameters[3].Value = model.BigPay;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateMoney(int ID, long Money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Money=Money+@Money");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Money", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Money;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateClick(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bslist set ");
            strSql.Append("Click=Click+@Click");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Bslist GetBslist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,Money,SmallPay,BigPay,UsID,UsName,Click,BzType,AddTime from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Bslist model = new BCW.Model.Game.Bslist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.Money = reader.GetInt64(2);
                    model.SmallPay = reader.GetInt64(3);
                    model.BigPay = reader.GetInt64(4);
                    model.UsID = reader.GetInt32(5);
                    model.UsName = reader.GetString(6);
                    model.Click = reader.GetInt32(7);
                    model.BzType = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个GetID
        /// </summary>
        public int GetID(int UsID, int BzType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID from tb_Bslist ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.Int,4)};

            parameters[0].Value = UsID;
            parameters[1].Value = BzType;

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
        /// 得到一个Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title from tb_Bslist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};

            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
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
            strSql.Append(" FROM tb_Bslist ");
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Bslist</returns>
        public IList<BCW.Model.Game.Bslist> GetBslists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Game.Bslist> listBslists = new List<BCW.Model.Game.Bslist>();
            string sTable = "tb_Bslist";
            string sPkey = "id";
            string sField = "ID,Title,Money,Click,BzType,SmallPay,BigPay";
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
                    return listBslists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Bslist objBslist = new BCW.Model.Game.Bslist();
                    objBslist.ID = reader.GetInt32(0);
                    objBslist.Title = reader.GetString(1);
                    objBslist.Money = reader.GetInt64(2);
                    objBslist.Click = reader.GetInt32(3);
                    objBslist.BzType = reader.GetByte(4);
                    objBslist.SmallPay = reader.GetInt64(5);
                    objBslist.BigPay = reader.GetInt64(6);
                    listBslists.Add(objBslist);
                }
            }
            return listBslists;
        }

        #endregion  成员方法
    }
}
