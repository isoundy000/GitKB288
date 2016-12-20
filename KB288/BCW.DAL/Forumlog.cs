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
    /// 数据访问类Forumlog。
    /// </summary>
    public class Forumlog
    {
        public Forumlog()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forumlog");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumlog");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到记录数
        /// </summary>
        public int GetCount(int BID, int ReID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumlog");
            strSql.Append(" where BID=@BID ");
            strSql.Append(" and ReID=@ReID ");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4)};
            parameters[0].Value = BID;
            parameters[1].Value = ReID;

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
        public int Add(int Types, int ForumID, string Content)
        {
            return Add(Types, ForumID, 0, 0, Content);
        }
  
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types, int ForumID, int BID, string Content)
        {
            return Add(Types, ForumID, BID, 0, Content);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(int Types,int ForumID, int BID, int ReID, string Content)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumlog(");
            strSql.Append("Types,ForumID,BID,ReID,Content,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@ForumID,@BID,@ReID,@Content,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,300),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = Types;
            parameters[1].Value = ForumID;
            parameters[2].Value = BID;
            parameters[3].Value = ReID;
            parameters[4].Value = Content;
            parameters[5].Value = DateTime.Now;

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
        public int Add(BCW.Model.Forumlog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumlog(");
            strSql.Append("Types,ForumID,BID,ReID,Content,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@ForumID,@BID,@ReID,@Content,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,300),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.ForumID;
            parameters[2].Value = model.BID;
            parameters[3].Value = model.ReID;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.AddTime;

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
        public void Update(BCW.Model.Forumlog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumlog set ");
            strSql.Append("Types=@Types,");
            strSql.Append("ForumID=@ForumID,");
            strSql.Append("BID=@BID,");
            strSql.Append("ReID=@ReID,");
            strSql.Append("Content=@Content,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ReID", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,300),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.ForumID;
            parameters[3].Value = model.BID;
            parameters[4].Value = model.ReID;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumlog ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumlog GetForumlog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,ForumID,BID,ReID,Content,AddTime from tb_Forumlog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forumlog model = new BCW.Model.Forumlog();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.ForumID = reader.GetInt32(2);
                    model.BID = reader.GetInt32(3);
                    model.ReID = reader.GetInt32(4);
                    model.Content = reader.GetString(5);
                    model.AddTime = reader.GetDateTime(6);
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
            strSql.Append(" FROM tb_Forumlog ");
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
        /// <returns>IList Forumlog</returns>
        public IList<BCW.Model.Forumlog> GetForumlogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumlog> listForumlogs = new List<BCW.Model.Forumlog>();
            string sTable = "tb_Forumlog";
            string sPkey = "id";
            string sField = "ID,Types,ForumID,BID,ReID,Content,AddTime";
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
                    return listForumlogs;
                }
                while (reader.Read())
                {
                    BCW.Model.Forumlog objForumlog = new BCW.Model.Forumlog();
                    objForumlog.ID = reader.GetInt32(0);
                    objForumlog.Types = reader.GetInt32(1);
                    objForumlog.ForumID = reader.GetInt32(2);
                    objForumlog.BID = reader.GetInt32(3);
                    objForumlog.ReID = reader.GetInt32(4);
                    objForumlog.Content = reader.GetString(5);
                    objForumlog.AddTime = reader.GetDateTime(6);
                    listForumlogs.Add(objForumlog);
                }
            }
            return listForumlogs;
        }

        #endregion  成员方法
    }
}

