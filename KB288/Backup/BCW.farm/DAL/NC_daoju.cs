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
    /// 数据访问类NC_daoju。
    /// </summary>
    public class NC_daoju
    {
        public NC_daoju()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_daoju");
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_daoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_daoju(");
            strSql.Append("name,price,note,picture,time,type)");
            strSql.Append(" values (");
            strSql.Append("@name,@price,@note,@picture,@time,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@note", SqlDbType.NVarChar,200),
                    new SqlParameter("@picture", SqlDbType.VarChar,20),
                    new SqlParameter("@time", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.name;
            parameters[1].Value = model.price;
            parameters[2].Value = model.note;
            parameters[3].Value = model.picture;
            parameters[4].Value = model.time;
            parameters[5].Value = model.type;

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
        public void Update(BCW.farm.Model.NC_daoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_daoju set ");
            strSql.Append("name=@name,");
            strSql.Append("price=@price,");
            strSql.Append("note=@note,");
            strSql.Append("picture=@picture,");
            strSql.Append("time=@time,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@note", SqlDbType.NVarChar,200),
                    new SqlParameter("@picture", SqlDbType.VarChar,20),
                    new SqlParameter("@time", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.name;
            parameters[2].Value = model.price;
            parameters[3].Value = model.note;
            parameters[4].Value = model.picture;
            parameters[5].Value = model.time;
            parameters[6].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_daoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_daoju GetNC_daoju(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name,price,note,picture,time,type from tb_NC_daoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_daoju model = new BCW.farm.Model.NC_daoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.price = reader.GetInt64(2);
                    model.note = reader.GetString(3);
                    model.picture = reader.GetString(4);
                    model.time = reader.GetInt32(5);
                    model.type = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_NC_daoju ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //======================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where ID=@ID and type!=10 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// //后台增加判断改id是否存在
        /// </summary>
        public bool Exists2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该道具名称
        /// </summary>
        public bool Exists_djmc(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where name=@name ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20)};
            parameters[0].Value = name;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_daoju(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_daoju SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到道具图片路径
        /// </summary>
        public string Get_picture(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select picture from tb_NC_daoju ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }
        //======================================


        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_daoju</returns>
        public IList<BCW.farm.Model.NC_daoju> GetNC_daojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_daoju> listNC_daojus = new List<BCW.farm.Model.NC_daoju>();
            string sTable = "tb_NC_daoju";
            string sPkey = "id";
            string sField = "ID,name,price,note,picture,time,type";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listNC_daojus;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_daoju objNC_daoju = new BCW.farm.Model.NC_daoju();
                    objNC_daoju.ID = reader.GetInt32(0);
                    objNC_daoju.name = reader.GetString(1);
                    objNC_daoju.price = reader.GetInt64(2);
                    objNC_daoju.note = reader.GetString(3);
                    objNC_daoju.picture = reader.GetString(4);
                    objNC_daoju.time = reader.GetInt32(5);
                    objNC_daoju.type = reader.GetInt32(6);
                    listNC_daojus.Add(objNC_daoju);
                }
            }
            return listNC_daojus;
        }

        #endregion  成员方法
    }
}

