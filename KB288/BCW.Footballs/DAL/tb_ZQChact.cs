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
	/// 数据访问类tb_ZQChact。
	/// </summary>
	public class tb_ZQChact
	{
		public tb_ZQChact()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_ZQChact"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_ZQChact");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
		/// 上一次发言秒数
		/// </summary>
		public int GetSecond(int UsId)
        {
           // SELECT DATEDIFF(Second, '2009-8-25 12:15:12', '2009-9-1 7:18:20')
            DateTime dt=DateTime.Now;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) DATEDIFF(SECOND, AddTime, GETDATE()) AS time1 from tb_ZQChact");
            strSql.Append(" where UsId=@UsId order by AddTime desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsId", SqlDbType.Int,4)};
            parameters[0].Value = UsId;
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
        /// 一场球赛的发言总量
        /// </summary>
        public int GetCountForId(int toFootID)
        {
            // SELECT DATEDIFF(Second, '2009-8-25 12:15:12', '2009-9-1 7:18:20')
            DateTime dt = DateTime.Now;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(ID) from tb_ZQChact");
            strSql.Append(" where toFootID=@toFootID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@toFootID", SqlDbType.Int,4)};
            parameters[0].Value = toFootID;
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
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.tb_ZQChact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_ZQChact(");
			strSql.Append("UsId,toFootID,TextContent,toUsId,isHit,AddTime,ident)");
			strSql.Append(" values (");
			strSql.Append("@UsId,@toFootID,@TextContent,@toUsId,@isHit,@AddTime,@ident)");
			SqlParameter[] parameters = {
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@toFootID", SqlDbType.Int,4),
					new SqlParameter("@TextContent", SqlDbType.NVarChar),
					new SqlParameter("@toUsId", SqlDbType.Int,4),
					new SqlParameter("@isHit", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.UsId;
			parameters[1].Value = model.toFootID;
			parameters[2].Value = model.TextContent;
			parameters[3].Value = model.toUsId;
			parameters[4].Value = model.isHit;
			parameters[5].Value = model.AddTime;
			parameters[6].Value = model.ident;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_ZQChact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_ZQChact set ");
			strSql.Append("UsId=@UsId,");
			strSql.Append("toFootID=@toFootID,");
			strSql.Append("TextContent=@TextContent,");
			strSql.Append("toUsId=@toUsId,");
			strSql.Append("isHit=@isHit,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("ident=@ident");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@toFootID", SqlDbType.Int,4),
					new SqlParameter("@TextContent", SqlDbType.NVarChar),
					new SqlParameter("@toUsId", SqlDbType.Int,4),
					new SqlParameter("@isHit", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsId;
			parameters[2].Value = model.toFootID;
			parameters[3].Value = model.TextContent;
			parameters[4].Value = model.toUsId;
			parameters[5].Value = model.isHit;
			parameters[6].Value = model.AddTime;
			parameters[7].Value = model.ident;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_ZQChact ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_ZQChact Gettb_ZQChact(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsId,toFootID,TextContent,toUsId,isHit,AddTime,ident from tb_ZQChact ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_ZQChact model=new BCW.Model.tb_ZQChact();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsId = reader.GetInt32(1);
					model.toFootID = reader.GetInt32(2);
					model.TextContent = reader.GetString(3);
					model.toUsId = reader.GetInt32(4);
					model.isHit = reader.GetInt32(5);
					model.AddTime = reader.GetDateTime(6);
					model.ident = reader.GetString(7);
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
			strSql.Append(" FROM tb_ZQChact ");
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
		/// <returns>IList tb_ZQChact</returns>
		public IList<BCW.Model.tb_ZQChact> Gettb_ZQChacts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_ZQChact> listtb_ZQChacts = new List<BCW.Model.tb_ZQChact>();
			string sTable = "tb_ZQChact";
			string sPkey = "id";
			string sField = "ID,UsId,toFootID,TextContent,toUsId,isHit,AddTime,ident";
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
					return listtb_ZQChacts;
				}
				while (reader.Read())
				{
						BCW.Model.tb_ZQChact objtb_ZQChact = new BCW.Model.tb_ZQChact();
						objtb_ZQChact.ID = reader.GetInt32(0);
						objtb_ZQChact.UsId = reader.GetInt32(1);
						objtb_ZQChact.toFootID = reader.GetInt32(2);
						objtb_ZQChact.TextContent = reader.GetString(3);
						objtb_ZQChact.toUsId = reader.GetInt32(4);
						objtb_ZQChact.isHit = reader.GetInt32(5);
						objtb_ZQChact.AddTime = reader.GetDateTime(6);
						objtb_ZQChact.ident = reader.GetString(7);
						listtb_ZQChacts.Add(objtb_ZQChact);
				}
			}
			return listtb_ZQChacts;
		}

		#endregion  成员方法
	}
}

