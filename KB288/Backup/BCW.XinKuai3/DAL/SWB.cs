using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.XinKuai3.DAL
{
	/// <summary>
	/// 数据访问类SWB。
	/// </summary>
	public class SWB
	{
		public SWB()
		{}
		#region  成员方法



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.XinKuai3.Model.SWB model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SWB(");
			strSql.Append("UserID,XK3Money,XK3IsGet)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@XK3Money,@XK3IsGet)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@XK3Money", SqlDbType.BigInt,8),
					new SqlParameter("@XK3IsGet", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.XK3Money;
			parameters[2].Value = model.XK3IsGet;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.XinKuai3.Model.SWB model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SWB set ");
			strSql.Append("UserID=@UserID,");
			strSql.Append("XK3Money=@XK3Money,");
			strSql.Append("XK3IsGet=@XK3IsGet");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@XK3Money", SqlDbType.BigInt,8),
					new SqlParameter("@XK3IsGet", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.XK3Money;
			parameters[2].Value = model.XK3IsGet;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SWB ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.XinKuai3.Model.SWB GetSWB(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,XK3Money,XK3IsGet from tb_SWB ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            BCW.XinKuai3.Model.SWB model = new BCW.XinKuai3.Model.SWB();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UserID = reader.GetInt32(0);
                    model.XK3Money = reader.GetInt64(1);
                    model.XK3IsGet = reader.GetDateTime(2);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_SWB ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}



        //=============================================
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SWB");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到用户币
        /// </summary>
        public long GetGold(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select XK3Money from tb_SWB ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        ///  me_更新用户虚拟币/更新消费记录
        /// </summary>
        public void UpdateiGold(int UserID, long iGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SWB set ");
            strSql.Append("XK3Money=XK3Money+@iGold ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@iGold", SqlDbType.BigInt,8)};
            parameters[0].Value = UserID;
            parameters[1].Value = iGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_更新用户试玩领取的时间
        /// </summary>
        public void Updatecishu(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SWB set ");
            strSql.Append("XK3IsGet='" + DateTime.Now + "' ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = UserID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_增加一条数据
        /// </summary>
        public void Add_num(BCW.XinKuai3.Model.SWB model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SWB(");
            strSql.Append("UserID)");
            strSql.Append(" values (");
            strSql.Append("@UserID)");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        //=============================================



		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList SWB</returns>
		public IList<BCW.XinKuai3.Model.SWB> GetSWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.XinKuai3.Model.SWB> listSWBs = new List<BCW.XinKuai3.Model.SWB>();
			string sTable = "tb_SWB";
			string sPkey = "id";
			string sField = "UserID,XK3Money,XK3IsGet";
			string sCondition = strWhere;
            string sOrder = "UserID";
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
					return listSWBs;
				}
				while (reader.Read())
				{
						BCW.XinKuai3.Model.SWB objSWB = new BCW.XinKuai3.Model.SWB();
						objSWB.UserID = reader.GetInt32(0);
						objSWB.XK3Money = reader.GetInt64(1);
						objSWB.XK3IsGet = reader.GetDateTime(2);
						listSWBs.Add(objSWB);
				}
			}
			return listSWBs;
		}

		#endregion  成员方法
	}
}

