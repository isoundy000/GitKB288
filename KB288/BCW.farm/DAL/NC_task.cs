using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
    /// <summary>
    /// 数据访问类NC_task。
    /// </summary>
    public class NC_task
    {
        public NC_task()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_task");
        }
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId2()
        {
            return SqlHelper.GetMaxID("task_id", "tb_NC_task");
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_task model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_task(");
            strSql.Append("task_name,task_contact,task_id,task_num,task_grade,task_jiangli,task_time,task_type)");
            strSql.Append(" values (");
            strSql.Append("@task_name,@task_contact,@task_id,@task_num,@task_grade,@task_jiangli,@task_time,@task_type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@task_name", SqlDbType.VarChar,50),
                    new SqlParameter("@task_contact", SqlDbType.VarChar,200),
                    new SqlParameter("@task_id", SqlDbType.Int,4),
                    new SqlParameter("@task_num", SqlDbType.Int,4),
                    new SqlParameter("@task_grade", SqlDbType.Int,4),
                    new SqlParameter("@task_jiangli", SqlDbType.VarChar,200),
                    new SqlParameter("@task_time", SqlDbType.DateTime),
                    new SqlParameter("@task_type", SqlDbType.Int,4)};
            parameters[0].Value = model.task_name;
            parameters[1].Value = model.task_contact;
            parameters[2].Value = model.task_id;
            parameters[3].Value = model.task_num;
            parameters[4].Value = model.task_grade;
            parameters[5].Value = model.task_jiangli;
            parameters[6].Value = model.task_time;
            parameters[7].Value = model.task_type;

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
        public void Update(BCW.farm.Model.NC_task model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_task set ");
            strSql.Append("task_name=@task_name,");
            strSql.Append("task_contact=@task_contact,");
            strSql.Append("task_id=@task_id,");
            strSql.Append("task_num=@task_num,");
            strSql.Append("task_grade=@task_grade,");
            strSql.Append("task_jiangli=@task_jiangli,");
            strSql.Append("task_time=@task_time,");
            strSql.Append("task_type=@task_type,");
            strSql.Append("xiajia=@xiajia");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@task_name", SqlDbType.VarChar,50),
                    new SqlParameter("@task_contact", SqlDbType.VarChar,200),
                    new SqlParameter("@task_id", SqlDbType.Int,4),
                    new SqlParameter("@task_num", SqlDbType.Int,4),
                    new SqlParameter("@task_grade", SqlDbType.Int,4),
                    new SqlParameter("@task_jiangli", SqlDbType.VarChar,200),
                    new SqlParameter("@task_time", SqlDbType.DateTime),
                    new SqlParameter("@task_type", SqlDbType.Int,4),
            new SqlParameter("@xiajia", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.task_name;
            parameters[2].Value = model.task_contact;
            parameters[3].Value = model.task_id;
            parameters[4].Value = model.task_num;
            parameters[5].Value = model.task_grade;
            parameters[6].Value = model.task_jiangli;
            parameters[7].Value = model.task_time;
            parameters[8].Value = model.task_type;
            parameters[9].Value = model.xiajia;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_task ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        //==================================
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_task");
            strSql.Append(" where task_id=@ID and xiajia=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_task GetNC_task(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_NC_task ");
            strSql.Append(" where task_id=@ID and xiajia=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_task model = new BCW.farm.Model.NC_task();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.task_name = reader.GetString(1);
                    model.task_contact = reader.GetString(2);
                    model.task_id = reader.GetInt32(3);
                    model.task_num = reader.GetInt32(4);
                    model.task_grade = reader.GetInt32(5);
                    model.task_jiangli = reader.GetString(6);
                    model.task_time = reader.GetDateTime(7);
                    model.task_type = reader.GetInt32(8);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_task GetNC_task2(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_NC_task ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_task model = new BCW.farm.Model.NC_task();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.task_name = reader.GetString(1);
                    model.task_contact = reader.GetString(2);
                    model.task_id = reader.GetInt32(3);
                    model.task_num = reader.GetInt32(4);
                    model.task_grade = reader.GetInt32(5);
                    model.task_jiangli = reader.GetString(6);
                    model.task_time = reader.GetDateTime(7);
                    model.task_type = reader.GetInt32(8);
                    model.xiajia = reader.GetInt32(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        //===========================================
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_task ");
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
        /// <returns>IList NC_task</returns>
        public IList<BCW.farm.Model.NC_task> GetNC_tasks(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_task> listNC_tasks = new List<BCW.farm.Model.NC_task>();
            string sTable = "tb_NC_task";
            string sPkey = "id";
            string sField = "ID,task_name,task_contact,task_id,task_num,task_grade,task_jiangli,task_time,task_type";
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
                    return listNC_tasks;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_task objNC_task = new BCW.farm.Model.NC_task();
                    objNC_task.ID = reader.GetInt32(0);
                    objNC_task.task_name = reader.GetString(1);
                    objNC_task.task_contact = reader.GetString(2);
                    objNC_task.task_id = reader.GetInt32(3);
                    objNC_task.task_num = reader.GetInt32(4);
                    objNC_task.task_grade = reader.GetInt32(5);
                    objNC_task.task_jiangli = reader.GetString(6);
                    objNC_task.task_time = reader.GetDateTime(7);
                    objNC_task.task_type = reader.GetInt32(8);
                    listNC_tasks.Add(objNC_task);
                }
            }
            return listNC_tasks;
        }

        #endregion  成员方法
    }
}

