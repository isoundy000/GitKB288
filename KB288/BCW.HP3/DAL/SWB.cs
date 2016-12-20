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
	/// 数据访问类:SWB
	/// </summary>
    public partial class SWB
    {
        public SWB()
        {}
        #region  BasicMethod
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("UserID", "tb_SWB");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SWB");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(BCW.HP3.Model.SWB model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SWB(");
            strSql.Append("UserID,HP3Money,HP3IsGet)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@HP3Money,@HP3IsGet)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@HP3Money", SqlDbType.BigInt,8),
					new SqlParameter("@HP3IsGet", SqlDbType.DateTime)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.HP3Money;
            parameters[2].Value = model.HP3IsGet;

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
        public bool Update(BCW.HP3.Model.SWB model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SWB set ");
            strSql.Append("HP3Money=@HP3Money,");
            strSql.Append("HP3IsGet=@HP3IsGet");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HP3Money", SqlDbType.BigInt,8),
					new SqlParameter("@HP3IsGet", SqlDbType.DateTime),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.HP3Money;
            parameters[1].Value = model.HP3IsGet;
            parameters[2].Value = model.UserID;

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
        /// 由用户ID更新HP3钱币
        /// </summary>
        public void UpdateHP3Money(int UserID, long HP3Money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SWB set ");
            strSql.Append(" HP3Money=HP3Money+@HP3Money ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HP3Money", SqlDbType.BigInt,8),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value =HP3Money;
            parameters[1].Value =UserID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 由用户ID更新HP3领钱时间
        /// </summary>
        public void UpdateHP3IsGet(int UserID,DateTime HP3IsGet)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SWB set ");
            strSql.Append(" HP3IsGet=@HP3IsGet ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@HP3IsGet", SqlDbType.DateTime),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value =HP3IsGet;
            parameters[1].Value =UserID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SWB ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string UserIDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SWB ");
            strSql.Append(" where UserID in (" + UserIDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.SWB GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,HP3Money,HP3IsGet from tb_SWB ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)			};
            parameters[0].Value = UserID;

            BCW.HP3.Model.SWB model = new BCW.HP3.Model.SWB();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HP3.Model.SWB DataRowToModel(DataRow row)
        {
            BCW.HP3.Model.SWB model = new BCW.HP3.Model.SWB();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["HP3Money"] != null && row["HP3Money"].ToString() != "")
                {
                    model.HP3Money = long.Parse(row["HP3Money"].ToString());
                }
                if (row["HP3IsGet"] != null && row["HP3IsGet"].ToString() != "")
                {
                    model.HP3IsGet = Convert.ToDateTime(row["HP3IsGet"].ToString());
                }
            }
            return model;
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserID,HP3Money,HP3IsGet ");
            strSql.Append(" FROM tb_SWB ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获得发奖数据列表
        /// </summary>
        public DataSet ReWardList(string strWhere1, string strWhere2, string strWhere3)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Notes from tb_Action where Types=2501 ");
            if (strWhere1.Trim() != "")
            {
                strSql.Append("  AND YEAR(AddTime) = YEAR('" + strWhere1 + "') ");
            }
            if (strWhere2.Trim() != "")
            {
                strSql.Append(" AND  MONTH(AddTime) = MONTH('" + strWhere2 + "') ");
            }
            if (strWhere3.Trim() != "")
            {
                strSql.Append(" AND  DAY(AddTime) = DAY('" + strWhere3 + "') ");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获取发奖记录总数
        /// </summary>
        public int ReWardCount(string strWhere1, string strWhere2, string strWhere3)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(Notes) from tb_Action where Types=2501 ");
            
            if (strWhere1.Trim() != "")
            {
                strSql.Append("  AND YEAR(AddTime) = YEAR('" + strWhere1 + "') ");
            }
            if (strWhere2.Trim() != "")
            {
                strSql.Append(" AND MONTH(AddTime) = MONTH('" + strWhere2 + "') ");
            }
            if (strWhere3.Trim() != "")
            {
                strSql.Append(" AND DAY(AddTime) = DAY('" + strWhere3 + "') ");
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
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_SWB ");
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
                strSql.Append("order by T.UserID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_SWB T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }

        #endregion  成员方法
    }
}
