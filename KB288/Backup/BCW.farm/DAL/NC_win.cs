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
    /// 数据访问类NC_win。
    /// </summary>
    public class NC_win
    {
        public NC_win()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_win");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_win");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_win model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_win(");
            strSql.Append("usid,prize_id,prize_name,addtime,prize_type)");
            strSql.Append(" values (");
            strSql.Append("@usid,@prize_id,@prize_name,@addtime,@prize_type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@prize_id", SqlDbType.Int,4),
                    new SqlParameter("@prize_name", SqlDbType.VarChar,20),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@prize_type", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.prize_id;
            parameters[2].Value = model.prize_name;
            parameters[3].Value = model.addtime;
            parameters[4].Value = model.prize_type;

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
        public void Update(BCW.farm.Model.NC_win model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_win set ");
            strSql.Append("usid=@usid,");
            strSql.Append("prize_id=@prize_id,");
            strSql.Append("prize_name=@prize_name,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("prize_type=@prize_type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@prize_id", SqlDbType.Int,4),
                    new SqlParameter("@prize_name", SqlDbType.VarChar,20),
                    new SqlParameter("@addtime", SqlDbType.DateTime),
                    new SqlParameter("@prize_type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.prize_id;
            parameters[3].Value = model.prize_name;
            parameters[4].Value = model.addtime;
            parameters[5].Value = model.prize_type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_win ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_win GetNC_win(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,usid,prize_id,prize_name,addtime,prize_type from tb_NC_win ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_win model = new BCW.farm.Model.NC_win();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.prize_id = reader.GetInt32(2);
                    model.prize_name = reader.GetString(3);
                    model.addtime = reader.GetDateTime(4);
                    model.prize_type = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_NC_win ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //============================================
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_win GetNC_suiji(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP 1 * FROM (select top 15  * from  tb_NC_win ORDER BY addtime DESC) AS b order by newid()");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_win model = new BCW.farm.Model.NC_win();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.prize_id = reader.GetInt32(2);
                    model.prize_name = reader.GetString(3);
                    model.addtime = reader.GetDateTime(4);
                    model.prize_type = reader.GetInt32(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        //============================================

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_win</returns>
        public IList<BCW.farm.Model.NC_win> GetNC_wins(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_win> listNC_wins = new List<BCW.farm.Model.NC_win>();
            string sTable = "tb_NC_win";
            string sPkey = "id";
            string sField = "ID,usid,prize_id,prize_name,addtime,prize_type";
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
                    return listNC_wins;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_win objNC_win = new BCW.farm.Model.NC_win();
                    objNC_win.ID = reader.GetInt32(0);
                    objNC_win.usid = reader.GetInt32(1);
                    objNC_win.prize_id = reader.GetInt32(2);
                    objNC_win.prize_name = reader.GetString(3);
                    objNC_win.addtime = reader.GetDateTime(4);
                    objNC_win.prize_type = reader.GetInt32(5);
                    listNC_wins.Add(objNC_win);
                }
            }
            return listNC_wins;
        }

        #endregion  成员方法
    }
}

