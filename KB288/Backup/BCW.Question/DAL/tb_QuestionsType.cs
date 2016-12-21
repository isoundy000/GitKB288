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
	/// 数据访问类tb_QuestionsType。
	/// </summary>
	public class tb_QuestionsType
	{
		public tb_QuestionsType()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_QuestionsType");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.tb_QuestionsType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_QuestionsType(");
			strSql.Append("Name,rank,statue,ident,AddTime,Remark,type)");
			strSql.Append(" values (");
			strSql.Append("@Name,@rank,@statue,@ident,@AddTime,@Remark,@type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@rank", SqlDbType.Int,4),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@type", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.rank;
			parameters[2].Value = model.statue;
			parameters[3].Value = model.ident;
			parameters[4].Value = model.AddTime;
			parameters[5].Value = model.Remark;
			parameters[6].Value = model.type;

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
		public void Update(BCW.Model.tb_QuestionsType model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_QuestionsType set ");
			strSql.Append("Name=@Name,");
			strSql.Append("rank=@rank,");
			strSql.Append("statue=@statue,");
			strSql.Append("ident=@ident,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("type=@type");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@rank", SqlDbType.Int,4),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@type", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Name;
			parameters[2].Value = model.rank;
			parameters[3].Value = model.statue;
			parameters[4].Value = model.ident;
			parameters[5].Value = model.AddTime;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.type;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_QuestionsType ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到Name
        /// </summary>
        public string GetName(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Name from tb_QuestionsType ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_QuestionsType Gettb_QuestionsType(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Name,rank,statue,ident,AddTime,Remark,type from tb_QuestionsType ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_QuestionsType model=new BCW.Model.tb_QuestionsType();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Name = reader.GetString(1);
					model.rank = reader.GetInt32(2);
					model.statue = reader.GetInt32(3);
					model.ident = reader.GetInt32(4);
					model.AddTime = reader.GetDateTime(5);
					model.Remark = reader.GetString(6);
					model.type = reader.GetString(7);
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
			strSql.Append(" FROM tb_QuestionsType ");
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
		/// <returns>IList tb_QuestionsType</returns>
		public IList<BCW.Model.tb_QuestionsType> Gettb_QuestionsTypes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_QuestionsType> listtb_QuestionsTypes = new List<BCW.Model.tb_QuestionsType>();
			string sTable = "tb_QuestionsType";
			string sPkey = "id";
			string sField = "ID,Name,rank,statue,ident,AddTime,Remark,type";
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
					return listtb_QuestionsTypes;
				}
				while (reader.Read())
				{
						BCW.Model.tb_QuestionsType objtb_QuestionsType = new BCW.Model.tb_QuestionsType();
						objtb_QuestionsType.ID = reader.GetInt32(0);
						objtb_QuestionsType.Name = reader.GetString(1);
						objtb_QuestionsType.rank = reader.GetInt32(2);
						objtb_QuestionsType.statue = reader.GetInt32(3);
						objtb_QuestionsType.ident = reader.GetInt32(4);
						objtb_QuestionsType.AddTime = reader.GetDateTime(5);
						objtb_QuestionsType.Remark = reader.GetString(6);
						objtb_QuestionsType.type = reader.GetString(7);
						listtb_QuestionsTypes.Add(objtb_QuestionsType);
				}
			}
			return listtb_QuestionsTypes;
		}

		#endregion  成员方法
	}
}

