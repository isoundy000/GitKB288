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
    /// 数据访问类Role。
    /// </summary>
    public class Role
    {
        public Role()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Role");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Role");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否历任版主
        /// </summary>
        public bool ExistsOver(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Role");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and OverTime<'" + DateTime.Now + "' and OverTime<>'1991-01-01' and Status<>1 ");
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
            strSql.Append("select count(1) from tb_Role");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Role");
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
        /// 是否管理员
        /// </summary>
        public bool IsAdmin(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = -1;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否总版主
        /// </summary>
        public bool IsMode(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否版块版主
        /// </summary>
        public bool IsSubMode(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID>@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否有总版主以上权限
        /// </summary>
        public bool IsSumMode(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID<=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否有版块版主以上权限
        /// </summary>
        public bool IsAllMode(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否有版块版主以上权限(不包括圈子版主)
        /// </summary>
        public bool IsAllModeNoGroup(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Count(1) from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status and (ForumID=0 OR ForumID NOT in (Select a.ForumId from tb_Group a where a.ForumId=ForumID))");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Role(");
            strSql.Append("UsID,UsName,Rolece,RoleName,ForumID,ForumName,StartTime,OverTime,Include,Status)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@Rolece,@RoleName,@ForumID,@ForumName,@StartTime,@OverTime,@Include,@Status)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Rolece", SqlDbType.NVarChar,200),
					new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@ForumName", SqlDbType.NVarChar,50),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Include", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Rolece;
            parameters[3].Value = model.RoleName;
            parameters[4].Value = model.ForumID;
            parameters[5].Value = model.ForumName;
            parameters[6].Value = model.StartTime;
            parameters[7].Value = model.OverTime;
            parameters[8].Value = model.Include;
            parameters[9].Value = model.Status;

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
        public void Update(BCW.Model.Role model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Role set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Rolece=@Rolece,");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("ForumName=@ForumName,");
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("Include=@Include,");
            strSql.Append("Status=@Status");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Rolece", SqlDbType.NVarChar,200),
					new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
					new SqlParameter("@ForumName", SqlDbType.NVarChar,50),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Include", SqlDbType.TinyInt,1),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Rolece;
            parameters[3].Value = model.RoleName;
            parameters[4].Value = model.ForumName;
            parameters[5].Value = model.StartTime;
            parameters[6].Value = model.OverTime;
            parameters[7].Value = model.Include;
            parameters[8].Value = model.Status;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新为荣誉版主
        /// </summary>
        public void UpdateOver(int ID, int Status)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Role set ");
            strSql.Append("Status=@Status ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Status;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Role ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个ForumID
        /// </summary>
        public int GetForumID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ForumID from tb_Role ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

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
        /// 得到一个Rolece
        /// </summary>
        public string GetRolece(int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rolece from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID<=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

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
        /// 得到一个Rolece
        /// </summary>
        public string GetRolece(int UsID, int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rolece from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;

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
        /// 得到一个包含下级版块的Rolece
        /// </summary>
        public string GetRoleces(int UsID, int ForumID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Rolece from tb_Role ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ForumID=@ForumID ");
            strSql.Append(" and (OverTime>=@OverTime OR OverTime<'1991-01-01') ");
            strSql.Append(" and Status=@Status ");
            strSql.Append(" and Include=@Include ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ForumID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@Include", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = ForumID;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = 0;
            parameters[4].Value = 1;

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
        public BCW.Model.Role GetRole(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,Rolece,RoleName,ForumID,ForumName,StartTime,OverTime,Include,Status,AddName from tb_Role ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Role model = new BCW.Model.Role();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.Rolece = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.RoleName = reader.GetString(4);
                    model.ForumID = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                        model.ForumName = reader.GetString(6);
                    model.StartTime = reader.GetDateTime(7);
                    model.OverTime = reader.GetDateTime(8);
                    model.Include = reader.GetByte(9);
                    model.Status = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.AddName = reader.GetString(11);
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
            strSql.Append(" FROM tb_Role ");
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
        /// <returns>IList Role</returns>
        public IList<BCW.Model.Role> GetRoles(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Role> listRoles = new List<BCW.Model.Role>();
            string sTable = "tb_Role";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,Rolece,RoleName,ForumID,ForumName,StartTime,OverTime,Include,Status,AddName";
            string sCondition = strWhere;
            string sOrder = "ID ASC,ForumId ASC";
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
                    return listRoles;
                }
                while (reader.Read())
                {
                    BCW.Model.Role objRole = new BCW.Model.Role();
                    objRole.ID = reader.GetInt32(0);
                    objRole.UsID = reader.GetInt32(1);
                    objRole.UsName = reader.GetString(2);
                    objRole.Rolece = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        objRole.RoleName = reader.GetString(4);
                    objRole.ForumID = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                        objRole.ForumName = reader.GetString(6);
                    objRole.StartTime = reader.GetDateTime(7);
                    objRole.OverTime = reader.GetDateTime(8);
                    objRole.Include = reader.GetByte(9);
                    objRole.Status = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        objRole.AddName = reader.GetString(11);
                    listRoles.Add(objRole);
                }
            }
            return listRoles;
        }


        /// <summary>
        /// 取得管理员记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Role</returns>
        public IList<BCW.Model.Role> GetRolesAdmin(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            //Status=0
            IList<BCW.Model.Role> listRoles = new List<BCW.Model.Role>();
            string sTable = "tb_Role";
            string sPkey = "id";
            string sField = "ID,UsID";
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
                    return listRoles;
                }
                while (reader.Read())
                {
                    BCW.Model.Role objRole = new BCW.Model.Role();
                    objRole.ID = reader.GetInt32(0);
                    objRole.UsID = reader.GetInt32(1);
                    listRoles.Add(objRole);
                }
            }
            return listRoles;
        }


        #endregion  成员方法
    }
}

