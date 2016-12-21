using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Baccarat.DAL
{
    /// <summary>
    /// 数据访问类BJL_user。
    /// </summary>
    public class BJL_user
    {
        public BJL_user()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_user");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_user");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_user(");
            strSql.Append("usid,setshow,kainum)");
            strSql.Append(" values (");
            strSql.Append("@usid,@setshow,@kainum)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@setshow", SqlDbType.Int,4),
                    new SqlParameter("@kainum", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.setshow;
            parameters[2].Value = model.kainum;

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
        public void Update(BCW.Baccarat.Model.BJL_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_user set ");
            strSql.Append("usid=@usid,");
            strSql.Append("setshow=@setshow,");
            strSql.Append("kainum=@kainum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@setshow", SqlDbType.Int,4),
                    new SqlParameter("@kainum", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.setshow;
            parameters[3].Value = model.kainum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_user ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_user GetBJL_user(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_user ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_user model = new BCW.Baccarat.Model.BJL_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.setshow = reader.GetInt32(2);
                    model.kainum = reader.GetInt32(3);
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
            strSql.Append(" FROM tb_BJL_user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists_user(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_user");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BJL_user SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_user GetBJL_setshow(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_user ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_user model = new BCW.Baccarat.Model.BJL_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.setshow = reader.GetInt32(2);
                    model.kainum = reader.GetInt32(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_user</returns>
        public IList<BCW.Baccarat.Model.BJL_user> GetBJL_users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_user> listBJL_users = new List<BCW.Baccarat.Model.BJL_user>();
            string sTable = "tb_BJL_user";
            string sPkey = "id";
            string sField = "ID,usid,setshow,kainum";
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
                    return listBJL_users;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_user objBJL_user = new BCW.Baccarat.Model.BJL_user();
                    objBJL_user.ID = reader.GetInt32(0);
                    objBJL_user.usid = reader.GetInt32(1);
                    objBJL_user.setshow = reader.GetInt32(2);
                    objBJL_user.kainum = reader.GetInt32(3);
                    listBJL_users.Add(objBJL_user);
                }
            }
            return listBJL_users;
        }

        #endregion  成员方法
    }
}

