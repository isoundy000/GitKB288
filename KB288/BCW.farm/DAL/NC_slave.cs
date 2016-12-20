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
    /// 数据访问类NC_slave。
    /// </summary>
    public class NC_slave
    {
        public NC_slave()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_slave");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_slave");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_slave model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_slave(");
            strSql.Append("usid,slave_id,punish,pacify,updatetime,tpye,num)");
            strSql.Append(" values (");
            strSql.Append("@usid,@slave_id,@punish,@pacify,@updatetime,@tpye,@num)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@slave_id", SqlDbType.Int,4),
                    new SqlParameter("@punish", SqlDbType.Int,4),
                    new SqlParameter("@pacify", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@tpye", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.slave_id;
            parameters[2].Value = model.punish;
            parameters[3].Value = model.pacify;
            parameters[4].Value = model.updatetime;
            parameters[5].Value = model.tpye;
            parameters[6].Value = model.num;

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
        public void Update(BCW.farm.Model.NC_slave model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_slave set ");
            strSql.Append("usid=@usid,");
            strSql.Append("slave_id=@slave_id,");
            strSql.Append("punish=@punish,");
            strSql.Append("pacify=@pacify,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("tpye=@tpye");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@slave_id", SqlDbType.Int,4),
                    new SqlParameter("@punish", SqlDbType.Int,4),
                    new SqlParameter("@pacify", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@tpye", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.slave_id;
            parameters[3].Value = model.punish;
            parameters[4].Value = model.pacify;
            parameters[5].Value = model.updatetime;
            parameters[6].Value = model.tpye;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_slave ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_slave GetNC_slave(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,slave_id,punish,pacify,updatetime,tpye from tb_NC_slave ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_slave model = new BCW.farm.Model.NC_slave();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.slave_id = reader.GetInt32(2);
                    model.punish = reader.GetInt32(3);
                    model.pacify = reader.GetInt32(4);
                    model.updatetime = reader.GetDateTime(5);
                    model.tpye = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_NC_slave ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //=================================

        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_slave SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_slave GetNCslave(int meid, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_slave ");
            strSql.Append(" where usid=@meid and slave_id=@usid and tpye=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = meid;
            parameters[1].Value = usid;

            BCW.farm.Model.NC_slave model = new BCW.farm.Model.NC_slave();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.slave_id = reader.GetInt32(2);
                    model.punish = reader.GetInt32(3);
                    model.pacify = reader.GetInt32(4);
                    model.updatetime = reader.GetDateTime(5);
                    model.tpye = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    //model.punish = 0;
                    //model.punish = 0;
                    return null;
                }
            }
        }
        /// <summary>
        /// me_是否存在该奴隶记录
        /// </summary>
        public bool Exists_nl(int slave_id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_slave");
            strSql.Append(" where slave_id=@slave_id and usid=@usid and tpye=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@slave_id", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = slave_id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该奴隶已经过期记录
        /// </summary>
        public bool Exists_nl2(int slave_id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_slave");
            strSql.Append(" where slave_id=@slave_id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@slave_id", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = slave_id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_更新奴隶信息
        /// </summary>
        public void Update_nl(BCW.farm.Model.NC_slave model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_slave set ");
            strSql.Append("punish=@punish,");
            strSql.Append("pacify=@pacify,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("tpye=@tpye,");
            strSql.Append("num=num+1");
            strSql.Append(" where usid=@usid and slave_id=@slave_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@slave_id", SqlDbType.Int,4),
                    new SqlParameter("@punish", SqlDbType.Int,4),
                    new SqlParameter("@pacify", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@tpye", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.slave_id;
            parameters[3].Value = model.punish;
            parameters[4].Value = model.pacify;
            parameters[5].Value = model.updatetime;
            parameters[6].Value = model.tpye;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        //=================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_slave</returns>
        public IList<BCW.farm.Model.NC_slave> GetNC_slaves(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_slave> listNC_slaves = new List<BCW.farm.Model.NC_slave>();
            string sTable = "tb_NC_slave";
            string sPkey = "id";
            string sField = "ID,usid,slave_id,punish,pacify,updatetime,tpye";
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
                    return listNC_slaves;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_slave objNC_slave = new BCW.farm.Model.NC_slave();
                    objNC_slave.ID = reader.GetInt32(0);
                    objNC_slave.usid = reader.GetInt32(1);
                    objNC_slave.slave_id = reader.GetInt32(2);
                    objNC_slave.punish = reader.GetInt32(3);
                    objNC_slave.pacify = reader.GetInt32(4);
                    objNC_slave.updatetime = reader.GetDateTime(5);
                    objNC_slave.tpye = reader.GetInt32(6);
                    listNC_slaves.Add(objNC_slave);
                }
            }
            return listNC_slaves;
        }

        #endregion  成员方法
    }
}

