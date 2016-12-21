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
	/// 数据访问类GroupText。
	/// </summary>
	public class GroupText
	{
		public GroupText()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_GroupText"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_GroupText");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int GroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GroupText");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and GroupId=@GroupId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = GroupId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.GroupText model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_GroupText(");
			strSql.Append("GroupId,UsID,UsName,ToID,ToName,Content,IsKiss,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@GroupId,@UsID,@UsName,@ToID,@ToName,@Content,@IsKiss,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200),
					new SqlParameter("@IsKiss", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.GroupId;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.ToID;
			parameters[4].Value = model.ToName;
			parameters[5].Value = model.Content;
			parameters[6].Value = model.IsKiss;
			parameters[7].Value = model.AddTime;

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
		public void Update(BCW.Model.GroupText model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_GroupText set ");
			strSql.Append("GroupId=@GroupId,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("ToID=@ToID,");
			strSql.Append("ToName=@ToName,");
			strSql.Append("Content=@Content,");
			strSql.Append("IsKiss=@IsKiss,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GroupId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200),
					new SqlParameter("@IsKiss", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GroupId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.ToID;
			parameters[5].Value = model.ToName;
			parameters[6].Value = model.Content;
			parameters[7].Value = model.IsKiss;
			parameters[8].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_GroupText ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_GroupText ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.GroupText GetGroupText(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GroupId,UsID,UsName,ToID,ToName,Content,IsKiss,AddTime from tb_GroupText ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.GroupText model=new BCW.Model.GroupText();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.GroupId = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.UsName = reader.GetString(3);
					model.ToID = reader.GetInt32(4);
					model.ToName = reader.GetString(5);
					model.Content = reader.GetString(6);
					model.IsKiss = reader.GetByte(7);
					model.AddTime = reader.GetDateTime(8);
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
			strSql.Append(" FROM tb_GroupText ");
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
		/// <returns>IList GroupText</returns>
		public IList<BCW.Model.GroupText> GetGroupTexts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.GroupText> listGroupTexts = new List<BCW.Model.GroupText>();
			string sTable = "tb_GroupText";
			string sPkey = "id";
			string sField = "ID,GroupId,UsID,UsName,ToID,ToName,Content,IsKiss,AddTime";
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
					return listGroupTexts;
				}
				while (reader.Read())
				{
						BCW.Model.GroupText objGroupText = new BCW.Model.GroupText();
						objGroupText.ID = reader.GetInt32(0);
						objGroupText.GroupId = reader.GetInt32(1);
						objGroupText.UsID = reader.GetInt32(2);
						objGroupText.UsName = reader.GetString(3);
						objGroupText.ToID = reader.GetInt32(4);
						objGroupText.ToName = reader.GetString(5);
						objGroupText.Content = reader.GetString(6);
						objGroupText.IsKiss = reader.GetByte(7);
						objGroupText.AddTime = reader.GetDateTime(8);
						listGroupTexts.Add(objGroupText);
				}
			}
			return listGroupTexts;
		}

		#endregion  成员方法
	}
}

