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
    /// 数据访问类NC_market。
    /// </summary>
    public class NC_market
    {
        public NC_market()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_market");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_market");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_market model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_market(");
            strSql.Append("usid,daoju_id,daoju_num,daoju_price,add_time,type,daoju_name,sui)");
            strSql.Append(" values (");
            strSql.Append("@usid,@daoju_id,@daoju_num,@daoju_price,@add_time,@type,@daoju_name,@sui)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@daoju_num", SqlDbType.Int,4),
                    new SqlParameter("@daoju_price", SqlDbType.BigInt,8),
                    new SqlParameter("@add_time", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@daoju_name", SqlDbType.VarChar,30),
                    new SqlParameter("@sui", SqlDbType.Float,8)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.daoju_id;
            parameters[2].Value = model.daoju_num;
            parameters[3].Value = model.daoju_price;
            parameters[4].Value = model.add_time;
            parameters[5].Value = model.type;
            parameters[6].Value = model.daoju_name;
            parameters[7].Value = model.sui;

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
        public void Update(BCW.farm.Model.NC_market model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_market set ");
            strSql.Append("usid=@usid,");
            strSql.Append("daoju_id=@daoju_id,");
            strSql.Append("daoju_num=@daoju_num,");
            strSql.Append("daoju_price=@daoju_price,");
            strSql.Append("add_time=@add_time,");
            strSql.Append("type=@type,");
            strSql.Append("daoju_name=@daoju_name");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@daoju_num", SqlDbType.Int,4),
                    new SqlParameter("@daoju_price", SqlDbType.BigInt,8),
                    new SqlParameter("@add_time", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@daoju_name", SqlDbType.VarChar,30)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.daoju_id;
            parameters[3].Value = model.daoju_num;
            parameters[4].Value = model.daoju_price;
            parameters[5].Value = model.add_time;
            parameters[6].Value = model.type;
            parameters[7].Value = model.daoju_name;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_market ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_market GetNC_market(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,daoju_id,daoju_num,daoju_price,add_time,type,daoju_name from tb_NC_market ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_market model = new BCW.farm.Model.NC_market();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.daoju_id = reader.GetInt32(2);
                    model.daoju_num = reader.GetInt32(3);
                    model.daoju_price = reader.GetInt64(4);
                    model.add_time = reader.GetDateTime(5);
                    model.type = reader.GetInt32(6);
                    model.daoju_name = reader.GetString(7);
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
            strSql.Append(" FROM tb_NC_market ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //===============================
        /// <summary>
        /// me_得到用户币
        /// </summary>
        public long Get_btcs(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(*) from tb_NC_market WHERE usid=@usid AND type=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// me_是否存在摆摊记录
        /// </summary>
        public bool Exists_baitan(int id, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_market");
            strSql.Append(" where id=@id ");
            strSql.Append(" and UsID=@UsID AND type=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在摆摊记录
        /// </summary>
        public bool Exists_baitan(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_market");
            strSql.Append(" where type=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_market(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_market SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_更新摊位道具的数量
        /// </summary>
        public void Update_twdj(int usid, int num, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_market set ");
            strSql.Append("daoju_num=daoju_num+@num");
            strSql.Append(" where usid=@usid and id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = num;
            parameters[2].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_判断道具是否为0
        /// </summary>
        public BCW.farm.Model.NC_market Get_djsl(int meid, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select daoju_num from tb_NC_market ");
            strSql.Append(" where usid=@meid and id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = meid;
            parameters[1].Value = id;

            BCW.farm.Model.NC_market model = new BCW.farm.Model.NC_market();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.daoju_num = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    model.daoju_num = 0;
                    return model;
                }
            }
        }
        //===============================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_market</returns>
        public IList<BCW.farm.Model.NC_market> GetNC_markets(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_market> listNC_markets = new List<BCW.farm.Model.NC_market>();
            string sTable = "tb_NC_market";
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
                    return listNC_markets;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_market objNC_market = new BCW.farm.Model.NC_market();
                    objNC_market.ID = reader.GetInt32(0);
                    objNC_market.usid = reader.GetInt32(1);
                    objNC_market.daoju_id = reader.GetInt32(2);
                    objNC_market.daoju_num = reader.GetInt32(3);
                    objNC_market.daoju_price = reader.GetInt64(4);
                    objNC_market.add_time = reader.GetDateTime(5);
                    objNC_market.type = reader.GetInt32(6);
                    objNC_market.daoju_name = reader.GetString(7);
                    listNC_markets.Add(objNC_market);
                }
            }
            return listNC_markets;
        }

        #endregion  成员方法
    }
}

