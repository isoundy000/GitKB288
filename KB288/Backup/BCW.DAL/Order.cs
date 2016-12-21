using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Common;
using BCW.Data;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Order。
    /// </summary>
    public class Order
    {
        public Order()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Order");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Order");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TopicId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Order");
            strSql.Append(" where TopicId=@TopicId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@TopicId", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = TopicId;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int TopicId, int UsID, DateTime ExTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Order");
            strSql.Append(" where TopicId=@TopicId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and ExTime>@ExTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@TopicId", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = TopicId;
            parameters[1].Value = UsID;
            parameters[2].Value = ExTime;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Order(");
            strSql.Append("TopicId,UsId,UsName,Title,SellTypes,AddTime,ExTime)");
            strSql.Append(" values (");
            strSql.Append("@TopicId,@UsId,@UsName,@Title,@SellTypes,@AddTime,@ExTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@TopicId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SellTypes", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = model.TopicId;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.SellTypes;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.ExTime;

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
        public void Update(BCW.Model.Order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Order set ");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Title=@Title,");
            strSql.Append("SellTypes=@SellTypes,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ExTime=@ExTime");
            strSql.Append(" where TopicId=@TopicId ");
            strSql.Append(" and UsId=@UsId");

            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TopicId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@SellTypes", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TopicId;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.SellTypes;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.ExTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Order set ");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("ExTime=@ExTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ExTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.AddTime;
            parameters[4].Value = model.ExTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Order ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到Title
        /// </summary>
        public string GetTitle(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Order ");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Order GetOrder(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,TopicId,UsId,UsName,Title,SellTypes,AddTime,ExTime from tb_Order ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Order model = new BCW.Model.Order();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.TopicId = reader.GetInt32(1);
                    model.UsId = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.Title = reader.GetString(4);
                    model.SellTypes = reader.GetInt32(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.ExTime = reader.GetDateTime(7);
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
            strSql.Append(" FROM tb_Order ");
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
        /// <returns>IList Order</returns>
        public IList<BCW.Model.Order> GetOrders(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Order> listOrders = new List<BCW.Model.Order>();
            string sTable = "tb_Order";
            string sPkey = "id";
            string sField = "ID,TopicId,UsId,UsName,Title,SellTypes,AddTime,ExTime";
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
                    return listOrders;
                }
                while (reader.Read())
                {
                    BCW.Model.Order objOrder = new BCW.Model.Order();
                    objOrder.ID = reader.GetInt32(0);
                    objOrder.TopicId = reader.GetInt32(1);
                    objOrder.UsId = reader.GetInt32(2);
                    objOrder.UsName = reader.GetString(3);
                    objOrder.Title = reader.GetString(4);
                    objOrder.SellTypes = reader.GetInt32(5);
                    objOrder.AddTime = reader.GetDateTime(6);
                    objOrder.ExTime = reader.GetDateTime(7);
                    listOrders.Add(objOrder);
                }
            }
            return listOrders;
        }

        #endregion  成员方法
    }
}


