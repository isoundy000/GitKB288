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
    /// 数据访问类NC_Goldlog。
    /// </summary>
    public class NC_Goldlog
    {
        public NC_Goldlog()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_Goldlog");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_Goldlog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_Goldlog(");
            strSql.Append("Types,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag)");
            strSql.Append(" values (");
            strSql.Append("@Types,@PUrl,@UsId,@UsName,@AcGold,@AfterGold,@AcText,@AddTime,@BbTag)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
                    new SqlParameter("@UsId", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@AcGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AfterGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AcText", SqlDbType.NVarChar),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@BbTag", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.AcGold;
            parameters[5].Value = model.AfterGold;
            parameters[6].Value = model.AcText;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.BbTag;

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
        public void Update(BCW.farm.Model.NC_Goldlog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_Goldlog set ");
            strSql.Append("Types=@Types,");
            strSql.Append("PUrl=@PUrl,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AcGold=@AcGold,");
            strSql.Append("AfterGold=@AfterGold,");
            strSql.Append("AcText=@AcText,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("BbTag=@BbTag");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
                    new SqlParameter("@UsId", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@AcGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AfterGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AcText", SqlDbType.NVarChar),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@BbTag", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.PUrl;
            parameters[3].Value = model.UsId;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.AcGold;
            parameters[6].Value = model.AfterGold;
            parameters[7].Value = model.AcText;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.BbTag;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_Goldlog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_Goldlog GetNC_Goldlog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag from tb_NC_Goldlog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_Goldlog model = new BCW.farm.Model.NC_Goldlog();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.PUrl = reader.GetString(2);
                    model.UsId = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.AcGold = reader.GetInt64(5);
                    model.AfterGold = reader.GetInt64(6);
                    model.AcText = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
                    model.BbTag = reader.GetInt32(9);
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
            strSql.Append(" FROM tb_NC_Goldlog ");
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
        /// <returns>IList NC_Goldlog</returns>
        public IList<BCW.farm.Model.NC_Goldlog> GetNC_Goldlogs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_Goldlog> listNC_Goldlogs = new List<BCW.farm.Model.NC_Goldlog>();
            string sTable = "tb_NC_Goldlog";
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
                    return listNC_Goldlogs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_Goldlog objNC_Goldlog = new BCW.farm.Model.NC_Goldlog();
                    objNC_Goldlog.ID = reader.GetInt32(0);
                    objNC_Goldlog.Types = reader.GetInt32(1);
                    objNC_Goldlog.PUrl = reader.GetString(2);
                    objNC_Goldlog.UsId = reader.GetInt32(3);
                    objNC_Goldlog.UsName = reader.GetString(4);
                    objNC_Goldlog.AcGold = reader.GetInt64(5);
                    objNC_Goldlog.AfterGold = reader.GetInt64(6);
                    objNC_Goldlog.AcText = reader.GetString(7);
                    objNC_Goldlog.AddTime = reader.GetDateTime(8);
                    objNC_Goldlog.BbTag = reader.GetInt32(9);
                    listNC_Goldlogs.Add(objNC_Goldlog);
                }
            }
            return listNC_Goldlogs;
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_Goldlog</returns>
        public IList<BCW.farm.Model.NC_Goldlog> GetNC_Goldlogs2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_Goldlog> listNC_Goldlogs = new List<BCW.farm.Model.NC_Goldlog>();
            string sTable = "tb_NC_Goldlog";
            string sPkey = "id";
            string sField = "ID,Types,PUrl,UsId,UsName,AcGold,AfterGold,AcText,AddTime,BbTag";
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
                    return listNC_Goldlogs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_Goldlog objNC_Goldlog = new BCW.farm.Model.NC_Goldlog();
                    objNC_Goldlog.ID = reader.GetInt32(0);
                    objNC_Goldlog.Types = reader.GetInt32(1);
                    objNC_Goldlog.PUrl = reader.GetString(2);
                    objNC_Goldlog.UsId = reader.GetInt32(3);
                    objNC_Goldlog.UsName = reader.GetString(4);
                    objNC_Goldlog.AcGold = reader.GetInt64(5);
                    objNC_Goldlog.AfterGold = reader.GetInt64(6);
                    objNC_Goldlog.AcText = reader.GetString(7);
                    objNC_Goldlog.AddTime = reader.GetDateTime(8);
                    objNC_Goldlog.BbTag = reader.GetInt32(9);
                    listNC_Goldlogs.Add(objNC_Goldlog);
                }
            }
            return listNC_Goldlogs;
        }

        #endregion  成员方法
    }
}

