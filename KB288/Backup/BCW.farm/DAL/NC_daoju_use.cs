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
    /// 数据访问类NC_daoju_use。
    /// </summary>
    public class NC_daoju_use
    {
        public NC_daoju_use()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_daoju_use");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju_use");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_daoju_use model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_daoju_use(");
            strSql.Append("usid,daoju_id,updatetime,type,tudi)");
            strSql.Append(" values (");
            strSql.Append("@usid,@daoju_id,@updatetime,@type,@tudi)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.daoju_id;
            parameters[2].Value = model.updatetime;
            parameters[3].Value = model.type;
            parameters[4].Value = model.tudi;

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
        public void Update(BCW.farm.Model.NC_daoju_use model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_daoju_use set ");
            strSql.Append("usid=@usid,");
            strSql.Append("daoju_id=@daoju_id,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("type=@type,");
            strSql.Append("tudi=@tudi");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@tudi", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.daoju_id;
            parameters[3].Value = model.updatetime;
            parameters[4].Value = model.type;
            parameters[5].Value = model.tudi;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_daoju_use ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_daoju_use GetNC_daoju_use(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,daoju_id,updatetime,type,tudi from tb_NC_daoju_use ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_daoju_use model = new BCW.farm.Model.NC_daoju_use();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.daoju_id = reader.GetInt32(2);
                    model.updatetime = reader.GetDateTime(3);
                    model.type = reader.GetInt32(4);
                    model.tudi = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_NC_daoju_use ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //=======================================

        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_type(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_daoju_use SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_是否存在已使用该道具
        /// </summary>
        public bool Exists_daoju(int usid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju_use");
            strSql.Append(" where daoju_id=@ID and usid=@usid and type=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //=======================================
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_daoju_use</returns>
        public IList<BCW.farm.Model.NC_daoju_use> GetNC_daoju_uses2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_daoju_use> listNC_daoju_uses = new List<BCW.farm.Model.NC_daoju_use>();
            string sTable = "tb_NC_daoju_use";
            string sPkey = "id";
            string sField = "*";
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
                    return listNC_daoju_uses;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_daoju_use objNC_daoju_use = new BCW.farm.Model.NC_daoju_use();
                    objNC_daoju_use.ID = reader.GetInt32(0);
                    objNC_daoju_use.usid = reader.GetInt32(1);
                    objNC_daoju_use.daoju_id = reader.GetInt32(2);
                    objNC_daoju_use.updatetime = reader.GetDateTime(3);
                    objNC_daoju_use.type = reader.GetInt32(4);
                    objNC_daoju_use.tudi = reader.GetInt32(5);
                    listNC_daoju_uses.Add(objNC_daoju_use);
                }
            }
            return listNC_daoju_uses;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_daoju_use</returns>
        public IList<BCW.farm.Model.NC_daoju_use> GetNC_daoju_uses(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_daoju_use> listNC_daoju_uses = new List<BCW.farm.Model.NC_daoju_use>();
            string sTable = "tb_NC_daoju_use";
            string sPkey = "id";
            string sField = "ID,usid,daoju_id,updatetime,type,tudi";
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
                    return listNC_daoju_uses;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_daoju_use objNC_daoju_use = new BCW.farm.Model.NC_daoju_use();
                    objNC_daoju_use.ID = reader.GetInt32(0);
                    objNC_daoju_use.usid = reader.GetInt32(1);
                    objNC_daoju_use.daoju_id = reader.GetInt32(2);
                    objNC_daoju_use.updatetime = reader.GetDateTime(3);
                    objNC_daoju_use.type = reader.GetInt32(4);
                    objNC_daoju_use.tudi = reader.GetInt32(5);
                    listNC_daoju_uses.Add(objNC_daoju_use);
                }
            }
            return listNC_daoju_uses;
        }

        #endregion  成员方法
    }
}

