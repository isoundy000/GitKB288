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
    /// 数据访问类Forumvotelog。
    /// </summary>
    public class Forumvotelog
    {
        public Forumvotelog()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Forumvotelog");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Forumvotelog");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Forumvotelog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Forumvotelog(");
            strSql.Append("UsID,UsName,AdminId,Title,BID,ForumId,Notes,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@AdminId,@Title,@BID,@ForumId,@Notes,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AdminId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.AdminId;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.BID;
            parameters[5].Value = model.ForumId;
            parameters[6].Value = model.Notes;
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
        public void Update(BCW.Model.Forumvotelog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Forumvotelog set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AdminId=@AdminId,");
            strSql.Append("Title=@Title,");
            strSql.Append("BID=@BID,");
            strSql.Append("ForumId=@ForumId,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AdminId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@ForumId", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.AdminId;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.BID;
            parameters[6].Value = model.ForumId;
            parameters[7].Value = model.Notes;
            parameters[8].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Forumvotelog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Forumvotelog GetForumvotelog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,AdminId,Title,BID,ForumId,Notes,AddTime from tb_Forumvotelog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Forumvotelog model = new BCW.Model.Forumvotelog();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.AdminId = reader.GetInt32(3);
                    model.Title = reader.GetString(4);
                    model.BID = reader.GetInt32(5);
                    model.ForumId = reader.GetInt32(6);
                    model.Notes = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
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
            strSql.Append(" FROM tb_Forumvotelog ");
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
        /// <returns>IList Forumvotelog</returns>
        public IList<BCW.Model.Forumvotelog> GetForumvotelogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Forumvotelog> listForumvotelogs = new List<BCW.Model.Forumvotelog>();
            string sTable = "tb_Forumvotelog";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,AdminId,Title,BID,ForumId,Notes,AddTime";
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
                    return listForumvotelogs;
                }
                while (reader.Read())
                {
                    BCW.Model.Forumvotelog objForumvotelog = new BCW.Model.Forumvotelog();
                    objForumvotelog.ID = reader.GetInt32(0);
                    objForumvotelog.UsID = reader.GetInt32(1);
                    objForumvotelog.UsName = reader.GetString(2);
                    objForumvotelog.AdminId = reader.GetInt32(3);
                    objForumvotelog.Title = reader.GetString(4);
                    objForumvotelog.BID = reader.GetInt32(5);
                    objForumvotelog.ForumId = reader.GetInt32(6);
                    objForumvotelog.Notes = reader.GetString(7);
                    objForumvotelog.AddTime = reader.GetDateTime(8);
                    listForumvotelogs.Add(objForumvotelog);
                }
            }
            return listForumvotelogs;
        }

        #endregion  成员方法
    }
}
