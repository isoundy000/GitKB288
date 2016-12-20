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
	/// 数据访问类GiftChange。
	/// </summary>
	public class GiftChange
	{
		public GiftChange()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("Types", "tb_GiftChange"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Types,int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_GiftChange");
			strSql.Append(" where Types=@Types and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = Types;
			parameters[1].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.GiftChange model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_GiftChange(");
			strSql.Append("Types,UsID,UsName,Notes,State,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@Types,@UsID,@UsName,@Notes,@State,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.Notes;
			parameters[4].Value = model.State;
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
		public void Update(BCW.Model.Game.GiftChange model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_GiftChange set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("Notes=@Notes,");
			strSql.Append("State=@State,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where Types=@Types and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.Notes;
			parameters[5].Value = model.State;
			parameters[6].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftChange set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Types,int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_GiftChange ");
			strSql.Append(" where Types=@Types and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = Types;
			parameters[1].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.GiftChange GetGiftChange(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,UsID,UsName,Notes,State,AddTime from tb_GiftChange ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.GiftChange model=new BCW.Model.Game.GiftChange();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.UsName = reader.GetString(3);
					model.Notes = reader.GetString(4);
					model.State = reader.GetByte(5);
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
			strSql.Append(" FROM tb_GiftChange ");
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
		/// <returns>IList GiftChange</returns>
		public IList<BCW.Model.Game.GiftChange> GetGiftChanges(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.GiftChange> listGiftChanges = new List<BCW.Model.Game.GiftChange>();
			string sTable = "tb_GiftChange";
			string sPkey = "id";
			string sField = "ID,Types,UsID,UsName,Notes,State,AddTime";
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
					return listGiftChanges;
				}
				while (reader.Read())
				{
						BCW.Model.Game.GiftChange objGiftChange = new BCW.Model.Game.GiftChange();
						objGiftChange.ID = reader.GetInt32(0);
						objGiftChange.Types = reader.GetInt32(1);
						objGiftChange.UsID = reader.GetInt32(2);
						objGiftChange.UsName = reader.GetString(3);
						objGiftChange.Notes = reader.GetString(4);
						objGiftChange.State = reader.GetByte(5);
						objGiftChange.AddTime = reader.GetDateTime(6);
						listGiftChanges.Add(objGiftChange);
				}
			}
			return listGiftChanges;
		}

		#endregion  成员方法
	}
}

