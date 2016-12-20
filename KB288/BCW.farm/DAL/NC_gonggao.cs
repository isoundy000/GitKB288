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
    /// 数据访问类NC_gonggao。
    /// </summary>
    public class NC_gonggao
    {
        public NC_gonggao()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_gonggao");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_gonggao");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_gonggao model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_gonggao(");
            strSql.Append("title,contact,updatetime,type)");
            strSql.Append(" values (");
            strSql.Append("@title,@contact,@updatetime,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@title", SqlDbType.VarChar,50),
                    new SqlParameter("@contact", SqlDbType.VarChar,300),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.title;
            parameters[1].Value = model.contact;
            parameters[2].Value = model.updatetime;
            parameters[3].Value = model.type;

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
        public void Update(BCW.farm.Model.NC_gonggao model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_gonggao set ");
            strSql.Append("title=@title,");
            strSql.Append("contact=@contact,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@title", SqlDbType.VarChar,50),
                    new SqlParameter("@contact", SqlDbType.VarChar,300),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.title;
            parameters[2].Value = model.contact;
            parameters[3].Value = model.updatetime;
            parameters[4].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_gonggao ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_gonggao GetNC_gonggao(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,title,contact,updatetime,type from tb_NC_gonggao ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_gonggao model = new BCW.farm.Model.NC_gonggao();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.title = reader.GetString(1);
                    model.contact = reader.GetString(2);
                    model.updatetime = reader.GetDateTime(3);
                    model.type = reader.GetInt32(4);
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
            strSql.Append(" FROM tb_NC_gonggao ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //============================
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_gonggao(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_gonggao SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //========================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_gonggao</returns>
        public IList<BCW.farm.Model.NC_gonggao> GetNC_gonggaos(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_gonggao> listNC_gonggaos = new List<BCW.farm.Model.NC_gonggao>();
            string sTable = "tb_NC_gonggao";
            string sPkey = "id";
            string sField = "ID,title,contact,updatetime,type";
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
                    return listNC_gonggaos;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_gonggao objNC_gonggao = new BCW.farm.Model.NC_gonggao();
                    objNC_gonggao.ID = reader.GetInt32(0);
                    objNC_gonggao.title = reader.GetString(1);
                    objNC_gonggao.contact = reader.GetString(2);
                    objNC_gonggao.updatetime = reader.GetDateTime(3);
                    objNC_gonggao.type = reader.GetInt32(4);
                    listNC_gonggaos.Add(objNC_gonggao);
                }
            }
            return listNC_gonggaos;
        }

        #endregion  成员方法
    }
}

