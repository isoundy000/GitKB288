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
	/// 数据访问类flowmyzz。
	/// </summary>
	public class flowmyzz
	{
		public flowmyzz()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_flowmyzz"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(int UsID, int Types, int zid)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_flowmyzz");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and zid=@zid ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = zid;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.flowmyzz model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_flowmyzz(");
			strSql.Append("Types,zid,ztitle,znum,UsID,UsName)");
			strSql.Append(" values (");
			strSql.Append("@Types,@zid,@ztitle,@znum,@UsID,@UsName)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@ztitle", SqlDbType.NVarChar,50),
					new SqlParameter("@znum", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.zid;
			parameters[2].Value = model.ztitle;
			parameters[3].Value = model.znum;
			parameters[4].Value = model.UsID;
			parameters[5].Value = model.UsName;

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
		public void Update(BCW.Model.Game.flowmyzz model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_flowmyzz set ");
            strSql.Append("znum=znum+@znum,");
			strSql.Append("UsName=@UsName");
            strSql.Append(" where UsID=@UsID");
            strSql.Append(" and Types=@Types");
            strSql.Append(" and zid=@zid");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@znum", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.zid;
			parameters[2].Value = model.znum;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.UsName;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新种子或花
        /// </summary>
        public void Updateznum(int UsID, int zid, int Types, int znum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowmyzz set ");
            strSql.Append("znum=znum+@znum");
            strSql.Append(" where UsID=@UsID");
            strSql.Append(" and Types=@Types");
            strSql.Append(" and zid=@zid");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@znum", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = zid;
            parameters[2].Value = znum;
            parameters[3].Value = UsID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_flowmyzz ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
        public BCW.Model.Game.flowmyzz Getflowmyzz(int UsID, int Types, int zid)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,zid,ztitle,znum,UsID,UsName from tb_flowmyzz ");
			strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and zid=@zid ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;
            parameters[2].Value = zid;

			BCW.Model.Game.flowmyzz model=new BCW.Model.Game.flowmyzz();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.zid = reader.GetInt32(2);
					model.ztitle = reader.GetString(3);
					model.znum = reader.GetInt32(4);
					model.UsID = reader.GetInt32(5);
					model.UsName = reader.GetString(6);
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
			strSql.Append(" FROM tb_flowmyzz ");
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
		/// <returns>IList flowmyzz</returns>
		public IList<BCW.Model.Game.flowmyzz> Getflowmyzzs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.flowmyzz> listflowmyzzs = new List<BCW.Model.Game.flowmyzz>();
			string sTable = "tb_flowmyzz";
			string sPkey = "id";
			string sField = "ID,Types,zid,ztitle,znum,UsID,UsName";
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
					return listflowmyzzs;
				}
				while (reader.Read())
				{
						BCW.Model.Game.flowmyzz objflowmyzz = new BCW.Model.Game.flowmyzz();
						objflowmyzz.ID = reader.GetInt32(0);
						objflowmyzz.Types = reader.GetInt32(1);
						objflowmyzz.zid = reader.GetInt32(2);
						objflowmyzz.ztitle = reader.GetString(3);
						objflowmyzz.znum = reader.GetInt32(4);
						objflowmyzz.UsID = reader.GetInt32(5);
						objflowmyzz.UsName = reader.GetString(6);
						listflowmyzzs.Add(objflowmyzz);
				}
			}
			return listflowmyzzs;
		}

		#endregion  成员方法
	}
}

