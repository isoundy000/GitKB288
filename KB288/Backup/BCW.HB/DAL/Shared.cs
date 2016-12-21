using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HB.DAL
{
    /// <summary>
    /// 数据访问类:Shared
    /// </summary>
    public partial class Shared
    {
        public Shared()
        { }
        #region 成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("UserID", "tb_Shared");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shared");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)            };
            parameters[0].Value = UserID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(BCW.HB.Model.Shared model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Shared(");
            strSql.Append("UserID,SharedIDList,ShareUrl,ShareContent)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@SharedIDList,@ShareUrl,@ShareContent)");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@SharedIDList", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ShareUrl", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ShareContent", SqlDbType.NVarChar,-1)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.SharedIDList;
            parameters[2].Value = model.ShareUrl;
            parameters[3].Value = model.ShareContent;

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
        public bool Update(BCW.HB.Model.Shared model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shared set ");
            strSql.Append("SharedIDList=@SharedIDList,");
            strSql.Append("ShareUrl=@ShareUrl,");
            strSql.Append("ShareContent=@ShareContent");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@SharedIDList", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ShareUrl", SqlDbType.NVarChar,-1),
                    new SqlParameter("@ShareContent", SqlDbType.NVarChar,-1),
                    new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.SharedIDList;
            parameters[1].Value = model.ShareUrl;
            parameters[2].Value = model.ShareContent;
            parameters[3].Value = model.UserID;

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
		/// 得到一个对象实体
		/// </summary>
		public BCW.HB.Model.Shared GetModel(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,SharedIDList,ShareUrl,ShareContent from tb_Shared ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)            };
            parameters[0].Value = UserID;

            BCW.HB.Model.Shared model = new BCW.HB.Model.Shared();
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
        public BCW.HB.Model.Shared DataRowToModel(DataRow row)
        {
            BCW.HB.Model.Shared model = new BCW.HB.Model.Shared();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["SharedIDList"] != null)
                {
                    model.SharedIDList = row["SharedIDList"].ToString();
                }
                if (row["ShareUrl"] != null)
                {
                    model.ShareUrl = row["ShareUrl"].ToString();
                }
                if (row["ShareContent"] != null)
                {
                    model.ShareContent = row["ShareContent"].ToString();
                }
            }
            return model;
        }
        #endregion
    }
}
