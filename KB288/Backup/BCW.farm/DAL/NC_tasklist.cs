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
    /// 数据访问类NC_tasklist。
    /// </summary>
    public class NC_tasklist
    {
        public NC_tasklist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_tasklist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_tasklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_tasklist(");
            strSql.Append("usid,task_id,task_oknum,task_time,task_oktime,task_type,type)");
            strSql.Append(" values (");
            strSql.Append("@usid,@task_id,@task_oknum,@task_time,@task_oktime,@task_type,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4),
                    new SqlParameter("@task_oknum", SqlDbType.Int,4),
                    new SqlParameter("@task_time", SqlDbType.DateTime),
                    new SqlParameter("@task_oktime", SqlDbType.DateTime),
                    new SqlParameter("@task_type", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.task_id;
            parameters[2].Value = model.task_oknum;
            parameters[3].Value = model.task_time;
            parameters[4].Value = model.task_oktime;
            parameters[5].Value = model.task_type;
            parameters[6].Value = model.type;

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
        public void Update(BCW.farm.Model.NC_tasklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_tasklist set ");
            strSql.Append("usid=@usid,");
            strSql.Append("task_id=@task_id,");
            strSql.Append("task_oknum=@task_oknum,");
            strSql.Append("task_time=@task_time,");
            strSql.Append("task_oktime=@task_oktime,");
            strSql.Append("task_type=@task_type,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4),
                    new SqlParameter("@task_oknum", SqlDbType.Int,4),
                    new SqlParameter("@task_time", SqlDbType.DateTime),
                    new SqlParameter("@task_oktime", SqlDbType.DateTime),
                    new SqlParameter("@task_type", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.task_id;
            parameters[3].Value = model.task_oknum;
            parameters[4].Value = model.task_time;
            parameters[5].Value = model.task_oktime;
            parameters[6].Value = model.task_type;
            parameters[7].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_tasklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_tasklist GetNC_tasklist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,task_id,task_oknum,task_time,task_oktime,task_type,type from tb_NC_tasklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_tasklist model = new BCW.farm.Model.NC_tasklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.task_id = reader.GetInt32(2);
                    model.task_oknum = reader.GetInt32(3);
                    model.task_time = reader.GetDateTime(4);
                    model.task_oktime = reader.GetDateTime(5);
                    model.task_type = reader.GetInt32(6);
                    model.type = reader.GetInt32(7);
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
            strSql.Append(" FROM tb_NC_tasklist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList1(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_tasklist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append("" + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //==
        /// <summary>
        ///  me_根据id和usid查询任务
        /// </summary>
        public BCW.farm.Model.NC_tasklist Get_renwu(int ID, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_tasklist ");
            strSql.Append(" where task_id=@ID and usid=@usid and type=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
            new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            BCW.farm.Model.NC_tasklist model = new BCW.farm.Model.NC_tasklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.task_id = reader.GetInt32(2);
                    model.task_oknum = reader.GetInt32(3);
                    model.task_time = reader.GetDateTime(4);
                    model.task_oktime = reader.GetDateTime(5);
                    model.task_type = reader.GetInt32(6);
                    model.type = reader.GetInt32(7);
                    return model;
                }
                else
                {
                    reader.Read();
                    model.usid = 0;
                    model.task_oknum = 0;
                    return model;
                }
            }
        }
        /// <summary>
        /// me_是否存在该记录--日常
        /// </summary>
        public bool Exists_usid(int usid, int task_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where usid=@usid and DateDiff(dd,task_time,getdate())=0 AND type=1 and task_id=@task_id and task_type=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = task_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录--主线
        /// </summary>
        public bool Exists_usid1(int usid, int task_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where usid=@usid and task_id=@task_id and task_type=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = task_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录--活动
        /// </summary>
        public bool Exists_usid3(int usid, int task_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where usid=@usid and DateDiff(dd,task_time,getdate())=0 AND type=1 and task_id=@task_id and task_type=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = task_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录--活动13-消费馈赠
        /// </summary>
        public bool Exists_usid13(int usid, int task_id ,int task_oknum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where usid=@usid and DateDiff(mm,task_time,getdate())=0 AND type=1 and task_id=@task_id and task_type=2 and task_oknum=@task_oknum");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4),
                    new SqlParameter("@task_oknum", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = task_id;
            parameters[2].Value = task_oknum;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录--主线(已完成)邵广林 20160922
        /// </summary>
        public bool Exists_usid2(int usid, int task_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_tasklist");
            strSql.Append(" where usid=@usid and task_id=@task_id and task_type=1 and type=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@task_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = task_id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_renwu(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_tasklist SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //==

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_tasklist</returns>
        public IList<BCW.farm.Model.NC_tasklist> GetNC_tasklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_tasklist> listNC_tasklists = new List<BCW.farm.Model.NC_tasklist>();
            string sTable = "tb_NC_tasklist";
            string sPkey = "id";
            string sField = "ID,usid,task_id,task_oknum,task_time,task_oktime,task_type,type";
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
                    return listNC_tasklists;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_tasklist objNC_tasklist = new BCW.farm.Model.NC_tasklist();
                    objNC_tasklist.ID = reader.GetInt32(0);
                    objNC_tasklist.usid = reader.GetInt32(1);
                    objNC_tasklist.task_id = reader.GetInt32(2);
                    objNC_tasklist.task_oknum = reader.GetInt32(3);
                    objNC_tasklist.task_time = reader.GetDateTime(4);
                    objNC_tasklist.task_oktime = reader.GetDateTime(5);
                    objNC_tasklist.task_type = reader.GetInt32(6);
                    objNC_tasklist.type = reader.GetInt32(7);
                    listNC_tasklists.Add(objNC_tasklist);
                }
            }
            return listNC_tasklists;
        }

        #endregion  成员方法
    }
}

