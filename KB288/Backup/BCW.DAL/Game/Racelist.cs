using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
    /// <summary>
    /// 数据访问类Racelist。
    /// </summary>
    public class Racelist
    {
        public Racelist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Racelist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Racelist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Racelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Racelist(");
            strSql.Append("payname,payusid,payCent,paytime,raceid,paytype)");
            strSql.Append(" values (");
            strSql.Append("@payname,@payusid,@payCent,@paytime,@raceid,@paytype)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@payname", SqlDbType.NVarChar,50),
					new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@paytime", SqlDbType.DateTime),
					new SqlParameter("@raceid", SqlDbType.Int,4),
					new SqlParameter("@paytype", SqlDbType.Int,4)};
            parameters[0].Value = model.payname;
            parameters[1].Value = model.payusid;
            parameters[2].Value = model.payCent;
            parameters[3].Value = model.paytime;
            parameters[4].Value = model.raceid;
            parameters[5].Value = model.paytype;

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
        public void Update(BCW.Model.Game.Racelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Racelist set ");
            strSql.Append("payname=@payname,");
            strSql.Append("payusid=@payusid,");
            strSql.Append("payCent=@payCent,");
            strSql.Append("paytime=@paytime,");
            strSql.Append("raceid=@raceid,");
            strSql.Append("paytype=@paytype");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@payname", SqlDbType.NVarChar,50),
					new SqlParameter("@payusid", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@paytime", SqlDbType.DateTime),
					new SqlParameter("@raceid", SqlDbType.Int,4),
					new SqlParameter("@paytype", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.payname;
            parameters[2].Value = model.payusid;
            parameters[3].Value = model.payCent;
            parameters[4].Value = model.paytime;
            parameters[5].Value = model.raceid;
            parameters[6].Value = model.paytype;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Racelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Racelist GetRacelist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,payname,payusid,payCent,paytime,raceid,paytype from tb_Racelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Racelist model = new BCW.Model.Game.Racelist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.payname = reader.GetString(1);
                    model.payusid = reader.GetInt32(2);
                    model.payCent = reader.GetInt64(3);
                    model.paytime = reader.GetDateTime(4);
                    model.raceid = reader.GetInt32(5);
                    model.paytype = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_Racelist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Racelist</returns>
        public IList<BCW.Model.Game.Racelist> GetRacelists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Racelist> listRacelists = new List<BCW.Model.Game.Racelist>();
            string sTable = "tb_Racelist";
            string sPkey = "id";
            string sField = "payname,payusid,paycent,paytime,raceid";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listRacelists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Racelist objRacelist = new BCW.Model.Game.Racelist();
                    objRacelist.payname = reader.GetString(0);
                    objRacelist.payusid = reader.GetInt32(1);
                    objRacelist.payCent = reader.GetInt64(2);
                    objRacelist.paytime = reader.GetDateTime(3);
                    objRacelist.raceid = reader.GetInt32(4);
                    listRacelists.Add(objRacelist);
                }
            }
            return listRacelists;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Racelist</returns>
        public IList<BCW.Model.Game.Racelist> GetRacelists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Racelist> listRacelists = new List<BCW.Model.Game.Racelist>();
            string sTable = "tb_Racelist";
            string sPkey = "id";
            string sField = "ID,payname,payusid,payCent,paytime,raceid,paytype";
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
                    return listRacelists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Racelist objRacelist = new BCW.Model.Game.Racelist();
                    objRacelist.ID = reader.GetInt32(0);
                    objRacelist.payname = reader.GetString(1);
                    objRacelist.payusid = reader.GetInt32(2);
                    objRacelist.payCent = reader.GetInt64(3);
                    objRacelist.paytime = reader.GetDateTime(4);
                    objRacelist.raceid = reader.GetInt32(5);
                    objRacelist.paytype = reader.GetInt32(6);
                    listRacelists.Add(objRacelist);
                }
            }
            return listRacelists;
        }

        #endregion  成员方法
    }
}

