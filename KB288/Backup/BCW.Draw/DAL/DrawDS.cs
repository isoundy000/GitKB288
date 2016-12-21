using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Draw.DAL
{
	/// <summary>
	/// 数据访问类DrawDS。
	/// </summary>
	public class DrawDS
	{
		public DrawDS()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_DrawDS"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DrawDS");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Draw.Model.DrawDS model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DrawDS(");
			strSql.Append("GoodsCounts,gamename,DS,DSID,one,two,three)");
			strSql.Append(" values (");
			strSql.Append("@GoodsCounts,@gamename,@DS,@DSID,@one,@two,@three)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@gamename", SqlDbType.NVarChar,50),
					new SqlParameter("@DS", SqlDbType.NVarChar,50),
					new SqlParameter("@DSID", SqlDbType.Int,4),
					new SqlParameter("@one", SqlDbType.NVarChar,50),
					new SqlParameter("@two", SqlDbType.NVarChar,50),
					new SqlParameter("@three", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.GoodsCounts;
			parameters[1].Value = model.gamename;
			parameters[2].Value = model.DS;
			parameters[3].Value = model.DSID;
			parameters[4].Value = model.one;
			parameters[5].Value = model.two;
			parameters[6].Value = model.three;

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
		public void Update(BCW.Draw.Model.DrawDS model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DrawDS set ");
			strSql.Append("GoodsCounts=@GoodsCounts,");
			strSql.Append("gamename=@gamename,");
			strSql.Append("DS=@DS,");
			strSql.Append("DSID=@DSID,");
			strSql.Append("one=@one,");
			strSql.Append("two=@two,");
			strSql.Append("three=@three");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@gamename", SqlDbType.NVarChar,50),
					new SqlParameter("@DS", SqlDbType.NVarChar,50),
					new SqlParameter("@DSID", SqlDbType.Int,4),
					new SqlParameter("@one", SqlDbType.NVarChar,50),
					new SqlParameter("@two", SqlDbType.NVarChar,50),
					new SqlParameter("@three", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GoodsCounts;
			parameters[2].Value = model.gamename;
			parameters[3].Value = model.DS;
			parameters[4].Value = model.DSID;
			parameters[5].Value = model.one;
			parameters[6].Value = model.two;
			parameters[7].Value = model.three;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DrawDS ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


        /// <summary>
        /// 根据GoodsCounts取
        /// </summary>
        public int GetDSID(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DSID from tb_DrawDS ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 根据GoodsCounts取
        /// </summary>
        public string GetDS(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DS from tb_DrawDS ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 根据GoodsCounts取
        /// </summary>
        public string GetGN(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GameName from tb_DrawDS ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return null;
                }
            }
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.DrawDS GetDrawDS(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GoodsCounts,gamename,DS,DSID,one,two,three from tb_DrawDS ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.DrawDS model=new BCW.Draw.Model.DrawDS();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.GoodsCounts = reader.GetInt32(1);
					model.gamename = reader.GetString(2);
					model.DS = reader.GetString(3);
					model.DSID = reader.GetInt32(4);
					model.one = reader.GetString(5);
					model.two = reader.GetString(6);
					model.three = reader.GetString(7);
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
			strSql.Append(" FROM tb_DrawDS ");
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
		/// <returns>IList DrawDS</returns>
		public IList<BCW.Draw.Model.DrawDS> GetDrawDSs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Draw.Model.DrawDS> listDrawDSs = new List<BCW.Draw.Model.DrawDS>();
			string sTable = "tb_DrawDS";
			string sPkey = "id";
			string sField = "ID,GoodsCounts,gamename,DS,DSID,one,two,three";
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
					return listDrawDSs;
				}
				while (reader.Read())
				{
						BCW.Draw.Model.DrawDS objDrawDS = new BCW.Draw.Model.DrawDS();
						objDrawDS.ID = reader.GetInt32(0);
						objDrawDS.GoodsCounts = reader.GetInt32(1);
						objDrawDS.gamename = reader.GetString(2);
						objDrawDS.DS = reader.GetString(3);
						objDrawDS.DSID = reader.GetInt32(4);
						objDrawDS.one = reader.GetString(5);
						objDrawDS.two = reader.GetString(6);
						objDrawDS.three = reader.GetString(7);
						listDrawDSs.Add(objDrawDS);
				}
			}
			return listDrawDSs;
		}

		#endregion  成员方法
	}
}

