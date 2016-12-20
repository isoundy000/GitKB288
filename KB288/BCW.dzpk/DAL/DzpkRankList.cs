using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.dzpk.DAL
{
	/// <summary>
	/// 数据访问类DzpkRankList。
	/// </summary>
	public class DzpkRankList
	{
		public DzpkRankList()
		{}
		#region  成员方法

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkRankList");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.dzpk.Model.DzpkRankList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkRankList(");
			strSql.Append("UsID,GetPot,Gtime,RmMake)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@GetPot,@Gtime,@RmMake)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GetPot", SqlDbType.BigInt,8),
					new SqlParameter("@Gtime", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.GetPot;
			parameters[2].Value = model.Gtime;
			parameters[3].Value = model.RmMake;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public void Update(BCW.dzpk.Model.DzpkRankList model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkRankList set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("GetPot=@GetPot,");
			strSql.Append("Gtime=@Gtime,");
			strSql.Append("RmMake=@RmMake");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GetPot", SqlDbType.BigInt,8),
					new SqlParameter("@Gtime", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.GetPot;
			parameters[3].Value = model.Gtime;
			parameters[4].Value = model.RmMake;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkRankList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.dzpk.Model.DzpkRankList GetDzpkRankList(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,GetPot,Gtime,RmMake from tb_DzpkRankList ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.dzpk.Model.DzpkRankList model=new BCW.dzpk.Model.DzpkRankList();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.GetPot = reader.GetInt64(2);
					model.Gtime = reader.GetDateTime(3);
					model.RmMake = reader.GetString(4);
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
			strSql.Append(" FROM tb_DzpkRankList ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
		/// <returns>IList DzpkRankList</returns>
		public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkRankList> listDzpkRankLists = new List<BCW.dzpk.Model.DzpkRankList>();
			string sTable = "tb_DzpkRankList";
			string sPkey = "id";
			string sField = "ID,UsID,GetPot,Gtime,RmMake";
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
					return listDzpkRankLists;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkRankList objDzpkRankList = new BCW.dzpk.Model.DzpkRankList();
						objDzpkRankList.ID = reader.GetInt32(0);
						objDzpkRankList.UsID = reader.GetInt32(1);
						objDzpkRankList.GetPot = reader.GetInt64(2);
						objDzpkRankList.Gtime = reader.GetDateTime(3);
						objDzpkRankList.RmMake = reader.GetString(4);
						listDzpkRankLists.Add(objDzpkRankList);
				}
			}
			return listDzpkRankLists;
		}

        /// <summary>
        /// 取得排行榜合计的每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList DzpkRankList</returns>
        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total(int p_pageIndex, int p_pageSize, string strWhere, string Sort, string OrderBy, out int p_recordCount, out string strsql)
        {
            IList<BCW.dzpk.Model.DzpkRankList> listDzpkRankLists = new List<BCW.dzpk.Model.DzpkRankList>();
            #region SQL赋值
            string sTable = "tb_DzpkRankList";
            string sField = "UsID, SUM(GetPot) AS GetPot";
            string st = "";
            if (Sort == "w") { st = " having SUM(GetPot)>0 "; }
            else {
                st = " having SUM(GetPot)<0";
            }
            string sCondition = strWhere + " GROUP BY UsID" + st;
            string sOrder = "GetPot ";
            if (OrderBy=="A")
                sOrder += "ASC";
            else sOrder += "DESC";
            strsql = "SELECT " + sField + " FROM " + sTable + " WHERE " + sCondition + " ORDER BY " + sOrder;
            DataSet ds = SqlHelper.Query(strsql);
            #endregion
            p_recordCount = 0;

            ///计算页数
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    p_recordCount = Convert.ToInt32((ds.Tables[0].Rows.Count));
                }
            }

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listDzpkRankLists;
            }

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strsql))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.dzpk.Model.DzpkRankList objDzpkRankList = new BCW.dzpk.Model.DzpkRankList();
                        objDzpkRankList.UsID = reader.GetInt32(0);
                        objDzpkRankList.GetPot = reader.GetInt64(1);
                        listDzpkRankLists.Add(objDzpkRankList);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listDzpkRankLists;
        }

        /// <summary>
        /// 取得排行榜合计的每页记录
        /// </summary>
        public IList<BCW.dzpk.Model.DzpkRankList> GetDzpkRankLists_Total_All(string strWhere, string Sort, string OrderBy)
        {
            IList<BCW.dzpk.Model.DzpkRankList> listDzpkRankLists = new List<BCW.dzpk.Model.DzpkRankList>();
            string sTable = "tb_DzpkRankList";
            string sField = "UsID, SUM(GetPot) AS GetPot";
            string st = "";
            if (Sort == "w") { st = " having SUM(GetPot)>0 "; }
            else {
                st = " having SUM(GetPot)<0";
            }
            string sCondition = strWhere + " GROUP BY UsID" + st;
            string sOrder = "GetPot ";
            if (OrderBy == "A")
                sOrder += "ASC";
            else sOrder += "DESC";
            string sql = "SELECT " + sField + " FROM " + sTable + " WHERE " + sCondition + " ORDER BY " + sOrder;
            DataSet ds = SqlHelper.Query(sql);
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            BCW.dzpk.Model.DzpkRankList objDzpkRankList = new BCW.dzpk.Model.DzpkRankList();
                            objDzpkRankList.UsID = int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString());
                            objDzpkRankList.GetPot = long.Parse(ds.Tables[0].Rows[i]["GetPot"].ToString());
                            listDzpkRankLists.Add(objDzpkRankList);
                        }
                    }
                }
            }
            return listDzpkRankLists;
        }
        #endregion  成员方法
    }
}

