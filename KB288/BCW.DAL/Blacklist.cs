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
    /// 数据访问类Blacklist。
    /// </summary>
    public class Blacklist
    {
        public Blacklist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Blacklist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Blacklist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ForumID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Blacklist");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and ExitTime>=@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该权限记录
        /// </summary>
        public bool ExistsRole(int UsID, string BlackRole)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Blacklist");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and (BlackRole=@BlackRole OR BlackRole='A')");//A指锁定，父权限
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and ExitTime>=@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BlackRole", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = BlackRole;
            parameters[2].Value = 0;
            parameters[3].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Blacklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Blacklist(");
            strSql.Append("UsID,UsName,ForumID,ForumName,BlackRole,BlackWhy,BlackDay,Include,AdminUsID,ExitTime,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@ForumID,@ForumName,@BlackRole,@BlackWhy,@BlackDay,@Include,@AdminUsID,@ExitTime,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ForumName", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackRole", SqlDbType.NVarChar,200),
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@Include", SqlDbType.TinyInt,1),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.ForumID;
            parameters[3].Value = model.ForumName;
            parameters[4].Value = model.BlackRole;
            parameters[5].Value = model.BlackWhy;
            parameters[6].Value = model.BlackDay;
            parameters[7].Value = model.Include;
            parameters[8].Value = model.AdminUsID;
            parameters[9].Value = model.ExitTime;
            parameters[10].Value = model.AddTime;

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
        public void Update(BCW.Model.Blacklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Blacklist set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ForumID=@ForumID,");
            strSql.Append("ForumName=@ForumName,");
            strSql.Append("BlackRole=@BlackRole,");
            strSql.Append("BlackWhy=@BlackWhy,");
            strSql.Append("BlackDay=@BlackDay,");
            strSql.Append("Include=@Include,");
            strSql.Append("AdminUsID=@AdminUsID,");
            strSql.Append("ExitTime=@ExitTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ForumName", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackRole", SqlDbType.NVarChar,200),
					new SqlParameter("@BlackWhy", SqlDbType.NVarChar,50),
					new SqlParameter("@BlackDay", SqlDbType.Int,4),
					new SqlParameter("@Include", SqlDbType.TinyInt,1),
					new SqlParameter("@AdminUsID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.ForumID;
            parameters[4].Value = model.ForumName;
            parameters[5].Value = model.BlackRole;
            parameters[6].Value = model.BlackWhy;
            parameters[7].Value = model.BlackDay;
            parameters[8].Value = model.Include;
            parameters[9].Value = model.AdminUsID;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.ExitTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Blacklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UsID, int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Blacklist ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void DeleteRole(int UsID, string BlackRole)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Blacklist ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BlackRole=@BlackRole");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BlackRole", SqlDbType.NVarChar,50)};
            parameters[0].Value = UsID;
            parameters[1].Value = BlackRole;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个Role
        /// </summary>
        public string GetRole(int UsID, int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BlackRole from tb_Blacklist ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and ExitTime>=@ExitTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ExitTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
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
        public BCW.Model.Blacklist GetBlacklist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,ForumID,ForumName,BlackRole,BlackWhy,Include,AdminUsID,ExitTime,AddTime from tb_Blacklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Blacklist model = new BCW.Model.Blacklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.ForumID = reader.GetInt32(3);
                    model.ForumName = reader.GetString(4);
                    model.BlackRole = reader.GetString(5);
                    model.BlackWhy = reader.GetString(6);
                    model.Include = reader.GetByte(7);
                    model.AdminUsID = reader.GetInt32(8);
                    model.ExitTime = reader.GetDateTime(9);
                    model.AddTime = reader.GetDateTime(10);
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
            strSql.Append(" FROM tb_Blacklist ");
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
        /// <returns>IList Blacklist</returns>
        public IList<BCW.Model.Blacklist> GetBlacklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Blacklist> listBlacklists = new List<BCW.Model.Blacklist>();
            string sTable = "tb_Blacklist";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,ForumID,ForumName,BlackRole,BlackWhy,BlackDay,Include,AdminUsID,ExitTime,AddTime";
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
                    return listBlacklists;
                }
                while (reader.Read())
                {
                    BCW.Model.Blacklist objBlacklist = new BCW.Model.Blacklist();
                    objBlacklist.ID = reader.GetInt32(0);
                    objBlacklist.UsID = reader.GetInt32(1);
                    objBlacklist.UsName = reader.GetString(2);
                    objBlacklist.ForumID = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                        objBlacklist.ForumName = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        objBlacklist.BlackRole = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        objBlacklist.BlackWhy = reader.GetString(6);
                    objBlacklist.BlackDay = reader.GetInt32(7);
                    objBlacklist.Include = reader.GetByte(8);
                    objBlacklist.AdminUsID = reader.GetInt32(9);
                    objBlacklist.ExitTime = reader.GetDateTime(10);
                    objBlacklist.AddTime = reader.GetDateTime(11);
                    listBlacklists.Add(objBlacklist);
                }
            }
            return listBlacklists;
        }

        #endregion  成员方法
    }
}
