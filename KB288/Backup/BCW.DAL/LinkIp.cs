using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
	/// <summary>
	/// 数据访问类LinkIp。
	/// </summary>
	public class LinkIp
	{
		public LinkIp()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_LinkIp"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_LinkIp");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LinkId, string AddUsIP, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_LinkIp");
            strSql.Append(" where LinkId=@LinkId ");
            strSql.Append(" and AddUsIP=@AddUsIP ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@LinkId", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = LinkId;
            parameters[1].Value = AddUsIP;
            parameters[2].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.LinkIp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_LinkIp(");
			strSql.Append("Types,AddUsIP,AddUsUA,AddUsPage,LinkId,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@Types,@AddUsIP,@AddUsUA,@AddUsPage,@LinkId,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsUA", SqlDbType.NVarChar,200),
					new SqlParameter("@AddUsPage", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkId", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.AddUsIP;
			parameters[2].Value = model.AddUsUA;
			parameters[3].Value = model.AddUsPage;
			parameters[4].Value = model.LinkId;
			parameters[5].Value = model.AddTime;

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
		public void Update(BCW.Model.LinkIp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_LinkIp set ");
			strSql.Append("Types=@Types,");
			strSql.Append("AddUsIP=@AddUsIP,");
			strSql.Append("AddUsUA=@AddUsUA,");
			strSql.Append("AddUsPage=@AddUsPage,");
			strSql.Append("LinkId=@LinkId,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddUsUA", SqlDbType.NVarChar,200),
					new SqlParameter("@AddUsPage", SqlDbType.NVarChar,200),
					new SqlParameter("@LinkId", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.AddUsIP;
			parameters[3].Value = model.AddUsUA;
			parameters[4].Value = model.AddUsPage;
			parameters[5].Value = model.LinkId;
			parameters[6].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_LinkIp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.LinkIp GetLinkIp(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,AddUsIP,AddUsUA,AddUsPage,LinkId,AddTime from tb_LinkIp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.LinkIp model=new BCW.Model.LinkIp();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.AddUsIP = reader.GetString(2);
					model.AddUsUA = reader.GetString(3);
					model.AddUsPage = reader.GetString(4);
					model.LinkId = reader.GetInt32(5);
					model.AddTime = reader.GetDateTime(6);
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
			strSql.Append(" FROM tb_LinkIp ");
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
		/// <returns>IList LinkIp</returns>
		public IList<BCW.Model.LinkIp> GetLinkIps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.LinkIp> listLinkIps = new List<BCW.Model.LinkIp>();
			string sTable = "tb_LinkIp";
			string sPkey = "id";
			string sField = "ID,Types,AddUsIP,AddUsUA,AddUsPage,LinkId,AddTime";
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
					return listLinkIps;
				}
				while (reader.Read())
				{
						BCW.Model.LinkIp objLinkIp = new BCW.Model.LinkIp();
						objLinkIp.ID = reader.GetInt32(0);
						objLinkIp.Types = reader.GetInt32(1);
						objLinkIp.AddUsIP = reader.GetString(2);
						objLinkIp.AddUsUA = reader.GetString(3);
						objLinkIp.AddUsPage = reader.GetString(4);
						objLinkIp.LinkId = reader.GetInt32(5);
						objLinkIp.AddTime = reader.GetDateTime(6);
						listLinkIps.Add(objLinkIp);
				}
			}
			return listLinkIps;
		}

		#endregion  成员方法
	}
}

