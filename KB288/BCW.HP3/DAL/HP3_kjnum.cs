using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HP3.DAL
{
    /// <summary>
    /// 数据访问类HP2_kjnum。
    /// </summary>
    public class HP3_kjnum
    {
        public HP3_kjnum()
        {
        }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string datenum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3_kjnum");
            strSql.Append(" where datenum=@datenum ");
            SqlParameter[] parameters = {
					new SqlParameter("@datenum", SqlDbType.NChar,10)			};
            parameters[0].Value = datenum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(string datenum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3_kjnum");
            strSql.Append(" where datenum=@datenum and  Fnum!='null'");
            SqlParameter[] parameters = {
                    new SqlParameter("@datenum", SqlDbType.NChar,10)            };
            parameters[0].Value = datenum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(string datenum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3_kjnum");
            strSql.Append(" where datenum=@datenum and  Fnum='null'");
            SqlParameter[] parameters = {
                    new SqlParameter("@datenum", SqlDbType.NChar,10)            };
            parameters[0].Value = datenum;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
 
        /// <summary>
        /// 增加一条新开奖信息
        /// </summary>
        public bool Add(BCW.HP3.Model.HP3_kjnum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HP3_kjnum(");
            strSql.Append("datenum,datetime,Fnum,Snum,Tnum,Winum)");
            strSql.Append(" values (");
            strSql.Append("@datenum,@datetime,@Fnum,@Snum,@Tnum,@Winum)");
            SqlParameter[] parameters = {
					new SqlParameter("@datenum", SqlDbType.NChar,10),
                    new SqlParameter("@datetime", SqlDbType.DateTime),
					new SqlParameter("@Fnum", SqlDbType.NChar,10),
					new SqlParameter("@Snum", SqlDbType.NChar,10),
					new SqlParameter("@Tnum", SqlDbType.NChar,10),
                    new SqlParameter("@Winum", SqlDbType.NChar,20)};
            parameters[0].Value = model.datenum;
            parameters[1].Value = model.datetime;
            parameters[2].Value = model.Fnum;
            parameters[3].Value = model.Snum;
            parameters[4].Value = model.Tnum;
            parameters[5].Value = model.Winum;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
         /// 更新一条数据
        /// </summary>
        public bool Update(BCW.HP3.Model.HP3_kjnum model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HP3_kjnum set ");
            strSql.Append("datetime=@datetime,");
            strSql.Append("Fnum=@Fnum,");
            strSql.Append("Snum=@Snum,");
            strSql.Append("Tnum=@Tnum,");
            strSql.Append("Winum=@Winum");
            strSql.Append(" where datenum=@datenum ");
            SqlParameter[] parameters = {
					new SqlParameter("@datetime", SqlDbType.DateTime),
					new SqlParameter("@Fnum", SqlDbType.NChar,10),
					new SqlParameter("@Snum", SqlDbType.NChar,10),
					new SqlParameter("@Tnum", SqlDbType.NChar,10),
					new SqlParameter("@Winum", SqlDbType.NChar,20),
					new SqlParameter("@datenum", SqlDbType.NChar,10)};
            parameters[0].Value = model.datetime;
            parameters[1].Value = model.Fnum;
            parameters[2].Value = model.Snum;
            parameters[3].Value = model.Tnum;
            parameters[4].Value = model.Winum;
            parameters[5].Value = model.datenum;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 是否存在要更新结果的记录
        /// </summary>
        public bool ExistsUpdateResult()
        {
            int Sec = Utils.ParseInt(ub.GetSub("HP3Sec", "/Controls/HappyPoker3.xml"));

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HP3_kjnum");
            strSql.Append(" where Winum='null' and datetime<'" + DateTime.Now.AddSeconds(Sec) + "'");

            return SqlHelper.Exists(strSql.ToString());
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string datenum)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HP3_kjnum ");
            strSql.Append(" where datenum=@datenum ");
            SqlParameter[] parameters = {
					new SqlParameter("@datenum", SqlDbType.NChar,10)			};
            parameters[0].Value = datenum;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select datenum,Fnum,Snum,Tnum,Winum ");
            strSql.Append(" FROM tb_HP3_kjnum ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_HP3_kjnum ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得最大非空期数数据列表
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetListLast()
        {
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top (1)*");
            strSql.Append(" FROM tb_HP3_kjnum  ");
            strSql.Append(" order by datenum desc ");
            DataSet ds = SqlHelper.Query(strSql.ToString());
            return DataRowToModel(ds.Tables[0].Rows[0]);
                
        }
        /// <summary>
        /// 获得最大期数数据列表
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetListLastNull()
        {
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 1 *");
            strSql.Append(" FROM tb_HP3_kjnum ");
            strSql.Append(" where Fnum!='null' ");
            strSql.Append(" order by datenum desc ");
            DataSet ds = SqlHelper.Query(strSql.ToString());
            try
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            catch
            {
                return null;
            }

        }
   
        /// <summary>
        /// 根据期号取数据
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum GetDataByState(string datenum)
        {
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM tb_HP3_kjnum ");
            strSql.Append(" where datenum='"+datenum+"'");
            DataSet ds = SqlHelper.Query(strSql.ToString());
            return DataRowToModel(ds.Tables[0].Rows[0]);

        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.HP3_kjnum DataRowToModel(DataRow row)
        {
            BCW.HP3.Model.HP3_kjnum model = new BCW.HP3.Model.HP3_kjnum();
            if (row != null)
            {
                if (row["datenum"] != null)
                {
                    model.datenum = row["datenum"].ToString();
                }
                if (row["datetime"] != null && row["datetime"].ToString() != "")
                {
                    model.datetime = DateTime.Parse(row["datetime"].ToString());
                }
                if (row["Fnum"] != null)
                {
                    model.Fnum = row["Fnum"].ToString();
                }
                if (row["Snum"] != null)
                {
                    model.Snum = row["Snum"].ToString();
                }
                if (row["Tnum"] != null)
                {
                    model.Tnum = row["Tnum"].ToString();
                }
                if (row["Winum"] != null)
                {
                    model.Winum = row["Winum"].ToString();
                }
            }
            return model;
        }
        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_HP3Buy ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 获取第几个期数
        /// </summary>
        public int GetXXCID(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) datenum FROM tb_HP3_kjnum ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@datenum", SqlDbType.Int,8)};
            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.datenum desc");
            }
            strSql.Append(")AS Row, T.*  from tb_HP3_kjnum T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }
        public IList<BCW.HP3.Model.HP3_kjnum> GetHP3ListByPage(int startIndex, int endIndex)
        {
            IList<BCW.HP3.Model.HP3_kjnum> model = new List<BCW.HP3.Model.HP3_kjnum>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.datenum desc");
            strSql.Append(")AS Row, T.*  from tb_HP3_kjnum T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
             DataSet ds=SqlHelper.Query(strSql.ToString());
             return model;
        }
        /// <summary>
        /// 取顶部数据列表
        /// </summary>
        public DataSet GetList(int TopNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + TopNum + " * ");
            strSql.Append(" FROM tb_HP3_kjnum ");
            strSql.Append(" where Winum!='null' ");
            strSql.Append(" Order By datenum desc");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// 
        ///<param name="storedProcName">存储过程名</param>
        /// <param name="sTable">表名</param>
        /// <param name="sPkey">主键</param>
        /// <param name="sField">字段</param>
        /// <param name="iPageCurr">当前页面</param>
        /// <param name="iPageSize">每页记录数</param>
        /// <param name="sCondition">WHERE条件</param>
        /// <param name="sOrder">排序</param>
        /// <param name="iSCounts">总记录数/0为存储过程计算</param>
        /// <param name="counts">OUT返回值</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.HP3.Model.HP3_kjnum> GetHP3ListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.HP3.Model.HP3_kjnum> model = new List<BCW.HP3.Model.HP3_kjnum>();
            string sTable = "tb_HP3_kjnum";
            string sPkey = "datenum";
            string sField = "datenum,datetime,Fnum,Snum,Tnum,Winum";
            string sCondition = strWhere;
            string sOrder = "datenum Desc";
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
                    return model;
                }
                while (reader.Read())
                {
                    BCW.HP3.Model.HP3_kjnum objHP3 = new BCW.HP3.Model.HP3_kjnum();
                    objHP3.datenum = reader.GetString(0);
                    objHP3.datetime = reader.GetDateTime(1);
                    objHP3.Fnum = reader.GetString(2);
                    objHP3.Snum = reader.GetString(3);
                    objHP3.Tnum = reader.GetString(4);
                    objHP3.Winum = reader.GetString(5);
                    model.Add(objHP3);
                }
            }
            return model;
        }
        //根据日期查询期号
        public DataSet GetDatenumByDate(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Winum ");
            strSql.Append(" from tb_HP3_kjnum");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where datenum like '%" + strWhere + "%' and Winum!='null'");
            }
            strSql.Append(" order by datenum");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("truncate table ");
            strSql.Append(TableName);
            SqlHelper.Query(strSql.ToString());
        }
        #endregion  成员方法
    }
}
