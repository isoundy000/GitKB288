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
    /// 数据访问类Forumts。
    /// </summary>
    public class Forumts
    {
        public Forumts()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forumts");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumts");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ForumID, string Title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumts");
            strSql.Append(" where ForumID=@ForumID ");
            strSql.Append(" and Title=@Title ");
            SqlParameter[] parameters = {
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50)};
            parameters[0].Value = ForumID;
            parameters[1].Value = Title;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumts");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumID=@ForumID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Forumts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumts(");
            strSql.Append("Title,ForumID,Paixu)");
            strSql.Append(" values (");
            strSql.Append("@Title,@ForumID,@Paixu)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.ForumID;
            parameters[2].Value = model.Paixu;

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
        public void Update(BCW.Model.Forumts model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumts set ");
            strSql.Append("Title=@Title,");
            strSql.Append("ForumID=@ForumID,");
            strSql.Append("Paixu=@Paixu");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.ForumID;
            parameters[3].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumts ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumts GetForumts(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,ForumID,Paixu from tb_Forumts ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forumts model = new BCW.Model.Forumts();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.ForumID = reader.GetInt32(2);
                    model.Paixu = reader.GetInt32(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到专题名称
        /// </summary>
        public string GetTitle(int ID,int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title from tb_Forumts ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and ForumID=@ForumID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ForumID;

            BCW.Model.Forumts model = new BCW.Model.Forumts();
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
            strSql.Append(" FROM tb_Forumts ");
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
        /// <returns>IList Forumts</returns>
        public IList<BCW.Model.Forumts> GetForumtss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumts> listForumtss = new List<BCW.Model.Forumts>();
            string sTable = "tb_Forumts";
            string sPkey = "id";
            string sField = "ID,Title,ForumID,Paixu";
            string sCondition = strWhere;
            string sOrder = "Paixu Desc,ID desc";
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
                    return listForumtss;
                }
                while (reader.Read())
                {
                    BCW.Model.Forumts objForumts = new BCW.Model.Forumts();
                    objForumts.ID = reader.GetInt32(0);
                    objForumts.Title = reader.GetString(1);
                    objForumts.ForumID = reader.GetInt32(2);
                    objForumts.Paixu = reader.GetInt32(3);
                    listForumtss.Add(objForumts);
                }
            }
            return listForumtss;
        }

        #endregion  成员方法
    }
}

