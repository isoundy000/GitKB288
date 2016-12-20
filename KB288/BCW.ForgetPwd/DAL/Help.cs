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
	/// 数据访问类tb_Help。
	/// </summary>
	public class tb_Help
	{
		public tb_Help()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Help"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Help");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Model.tb_Help model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Help(");
			strSql.Append("ID,Title,Explain,LinkName,HasLink)");
			strSql.Append(" values (");
			strSql.Append("@ID,@Title,@Explain,@LinkName,@HasLink)");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@LinkName", SqlDbType.NVarChar,50),
					new SqlParameter("@HasLink", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.Explain;
			parameters[3].Value = model.LinkName;
			parameters[4].Value = model.HasLink;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_Help model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Help set ");
			strSql.Append("Title=@Title,");
			strSql.Append("Explain=@Explain,");
			strSql.Append("LinkName=@LinkName,");
			strSql.Append("HasLink=@HasLink");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@LinkName", SqlDbType.NVarChar,50),
					new SqlParameter("@HasLink", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.Explain;
			parameters[3].Value = model.LinkName;
			parameters[4].Value = model.HasLink;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Help ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.tb_Help Gettb_Help(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Title,Explain,LinkName,HasLink from tb_Help ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_Help model=new BCW.Model.tb_Help();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Title = reader.GetString(1);
					model.Explain = reader.GetString(2);
					model.LinkName = reader.GetString(3);
					model.HasLink = reader.GetInt32(4);
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
			strSql.Append(" FROM tb_Help ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}
        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.tb_Help> GetHelps(int SizeNum, string strWhere)
        {
            IList<BCW.Model.tb_Help> listtb_Helps = new List<BCW.Model.tb_Help>();
            string sTable = "tb_Help";
            string sPkey = "id";
            string sField = "ID,Title,Explain,LinkName,HasLink";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listtb_Helps;
                }
                while (reader.Read())
                {   
						BCW.Model.tb_Help objtb_Help = new BCW.Model.tb_Help();
						objtb_Help.ID = reader.GetInt32(0);
						objtb_Help.Title = reader.GetString(1);
						objtb_Help.Explain = reader.GetString(2);
						objtb_Help.LinkName = reader.GetString(3);
						objtb_Help.HasLink = reader.GetInt32(4);
						listtb_Helps.Add(objtb_Help);			
                }
            }
            return listtb_Helps;
        }
		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList tb_Help</returns>
		public IList<BCW.Model.tb_Help> Gettb_Helps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_Help> listtb_Helps = new List<BCW.Model.tb_Help>();
			string sTable = "tb_Help";
			string sPkey = "id";
			string sField = "ID,Title,Explain,LinkName,HasLink";
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
					return listtb_Helps;
				}
				while (reader.Read())
				{
						BCW.Model.tb_Help objtb_Help = new BCW.Model.tb_Help();
						objtb_Help.ID = reader.GetInt32(0);
						objtb_Help.Title = reader.GetString(1);
						objtb_Help.Explain = reader.GetString(2);
						objtb_Help.LinkName = reader.GetString(3);
						objtb_Help.HasLink = reader.GetInt32(4);
						listtb_Helps.Add(objtb_Help);
				}
			}
			return listtb_Helps;
		}

		#endregion  成员方法
	}
}

