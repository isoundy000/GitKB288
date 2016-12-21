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
    /// 数据访问类NC_baoxiang。
    /// </summary>
    public class NC_baoxiang
    {
        public NC_baoxiang()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_baoxiang");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_baoxiang");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_baoxiang model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_baoxiang(");
            strSql.Append("prize,picture,daoju_id,type)");
            strSql.Append(" values (");
            strSql.Append("@prize,@picture,@daoju_id,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@prize", SqlDbType.VarChar,50),
                    new SqlParameter("@picture", SqlDbType.VarChar,60),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.prize;
            parameters[1].Value = model.picture;
            parameters[2].Value = model.daoju_id;
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
        public void Update(BCW.farm.Model.NC_baoxiang model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_baoxiang set ");
            strSql.Append("prize=@prize,");
            strSql.Append("picture=@picture,");
            strSql.Append("daoju_id=@daoju_id,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@prize", SqlDbType.VarChar,50),
                    new SqlParameter("@picture", SqlDbType.VarChar,60),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.prize;
            parameters[2].Value = model.picture;
            parameters[3].Value = model.daoju_id;
            parameters[4].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_baoxiang ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_baoxiang GetNC_baoxiang(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,prize,picture,daoju_id,type from tb_NC_baoxiang ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.prize = reader.GetString(1);
                    model.picture = reader.GetString(2);
                    model.daoju_id = reader.GetInt32(3);
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
            strSql.Append(" FROM tb_NC_baoxiang ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //========================================
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_baoxiang DataRowToModel(DataRow row)
        {
            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["prize"] != null)
                {
                    model.prize = row["prize"].ToString();
                }
                if (row["picture"] != null)
                {
                    model.picture = row["picture"].ToString();
                }
                if (row["daoju_id"] != null && row["daoju_id"].ToString() != "")
                {
                    model.daoju_id = int.Parse(row["daoju_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
            }
            return model;
        }
        // me_初始化某数据表
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// me_查询有几个道具
        /// </summary>
        public BCW.farm.Model.NC_baoxiang Get_num(int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_baoxiang");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = type;

            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        ///me_判断是否存在种子id
        /// </summary>
        public bool Exists_bxzzdj(int ID, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_baoxiang");
            strSql.Append(" where daoju_id=@ID and type=@type");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = type;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //========================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_baoxiang</returns>
        public IList<BCW.farm.Model.NC_baoxiang> GetNC_baoxiangs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_baoxiang> listNC_baoxiangs = new List<BCW.farm.Model.NC_baoxiang>();
            string sTable = "tb_NC_baoxiang";
            string sPkey = "id";
            string sField = "ID,prize,picture,daoju_id,type";
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
                    return listNC_baoxiangs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_baoxiang objNC_baoxiang = new BCW.farm.Model.NC_baoxiang();
                    objNC_baoxiang.ID = reader.GetInt32(0);
                    objNC_baoxiang.prize = reader.GetString(1);
                    objNC_baoxiang.picture = reader.GetString(2);
                    objNC_baoxiang.daoju_id = reader.GetInt32(3);
                    objNC_baoxiang.type = reader.GetInt32(4);
                    listNC_baoxiangs.Add(objNC_baoxiang);
                }
            }
            return listNC_baoxiangs;
        }

        #endregion  成员方法
    }
}

