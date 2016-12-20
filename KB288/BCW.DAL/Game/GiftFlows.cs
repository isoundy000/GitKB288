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
    /// 数据访问类GiftFlows。
    /// </summary>
    public class GiftFlows
    {
        public GiftFlows()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("Types", "tb_GiftFlows");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Types, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录(规定数量)
        /// </summary>
        public bool Exists(int Types, int UsID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and UsID=@UsID and Totall>=@Totall ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = Totall;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录(N秒内)
        /// </summary>
        public bool ExistsSec(int Types, int UsID, int Sec)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and (UsID=@UsID OR UsIP='" + Utils.GetUsIP() + "') and AddTime>='" + DateTime.Now.AddSeconds(-Sec) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 计算不同物品有多少个类型
        /// </summary>
        public int GetTypesTotal(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(DISTINCT Types) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID and Totall>@Totall");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;

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
        /// 计算某用户花的总量
        /// </summary>
        public int GetTotal(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Total) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID ");
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
        /// 计算某用户花的剩余量
        /// </summary>
        public int GetTotall(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Totall) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID ");
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
        public int Add(BCW.Model.Game.GiftFlows model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_GiftFlows(");
            strSql.Append("Types,UsID,UsName,Total,Totall,AddTime,UsIP)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@Total,@Totall,@AddTime,@UsIP)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@UsIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Total;
            parameters[4].Value = model.Totall;
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = Utils.GetUsIP();

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
        public void Update(BCW.Model.Game.GiftFlows model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Total=Total+@Total,");
            strSql.Append("Totall=Totall+@Totall,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where Types=@Types and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Total;
            parameters[5].Value = model.Totall;
            parameters[6].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateTotall(int Types, int UsID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("Totall=Totall+@Totall");
            strSql.Append(" where Types=@Types and UsID=@UsID and Totall>0");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = Totall;

           return SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int UpdateTotall(int ID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("Totall=Totall+@Totall");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Totall;

            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_GiftFlows ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.GiftFlows GetGiftFlows(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,Total,Totall from tb_GiftFlows ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            BCW.Model.Game.GiftFlows model = new BCW.Model.Game.GiftFlows();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.Total = reader.GetInt32(4);
                    model.Totall = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_GiftFlows ");
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
        /// <returns>IList GiftFlows</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlowss = new List<BCW.Model.Game.GiftFlows>();
            string sTable = "tb_GiftFlows";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,Total,Totall";
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
                    return listGiftFlowss;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                    objGiftFlows.ID = reader.GetInt32(0);
                    objGiftFlows.Types = reader.GetInt32(1);
                    objGiftFlows.UsID = reader.GetInt32(2);
                    objGiftFlows.UsName = reader.GetString(3);
                    objGiftFlows.Total = reader.GetInt32(4);
                    objGiftFlows.Totall = reader.GetInt32(5);
                    listGiftFlowss.Add(objGiftFlows);
                }
            }
            return listGiftFlowss;
        }

        /// <summary>
        /// 取到排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlows = new List<BCW.Model.Game.GiftFlows>();

            string strWhe = "";
            if (strWhere != "")
                strWhe = " where " + strWhere + "";

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_GiftFlows " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 50)
                    pageCount = 50;
            }
            else
            {
                return listGiftFlows;
            }

            // 取出相关记录

            string queryString = "SELECT UsID, Sum(Total) as Total FROM tb_GiftFlows " + strWhe + " GROUP BY UsID ORDER BY Sum(Total) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                        objGiftFlows.UsID = reader.GetInt32(0);
                        objGiftFlows.Total = reader.GetInt32(1);
                        listGiftFlows.Add(objGiftFlows);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGiftFlows;
        }


        /// <summary>
        /// 取到排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlows = new List<BCW.Model.Game.GiftFlows>();

            string strWhe = "";
            if (strWhere != "")
                strWhe = " where " + strWhere + "";

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT Types) FROM tb_GiftFlows " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 50)
                    pageCount = 50;
            }
            else
            {
                return listGiftFlows;
            }

            // 取出相关记录

            string queryString = "SELECT Types, Sum(Total) as Total, Sum(Totall) as Totall FROM tb_GiftFlows " + strWhe + " GROUP BY Types ORDER BY Sum(Total) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                        objGiftFlows.Types = reader.GetInt32(0);
                        objGiftFlows.Total = reader.GetInt32(1);
                        objGiftFlows.Totall = reader.GetInt32(2);
                        listGiftFlows.Add(objGiftFlows);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGiftFlows;
        }

        #endregion  成员方法
    }
}

