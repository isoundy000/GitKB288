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
	/// 数据访问类ChatLog。
	/// </summary>
	public class ChatLog
	{
		public ChatLog()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_ChatLog"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_ChatLog");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.ChatLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_ChatLog(");
			strSql.Append("Types,ChatId,UsID,UsName,PayCent,Content,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@Types,@ChatId,@UsID,@UsName,@PayCent,@Content,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.ChatId;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.PayCent;
			parameters[5].Value = model.Content;
			parameters[6].Value = model.AddTime;

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
		public void Update(BCW.Model.ChatLog model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_ChatLog set ");
			strSql.Append("Types=@Types,");
			strSql.Append("ChatId=@ChatId,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("PayCent=@PayCent,");
			strSql.Append("Content=@Content,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.ChatId;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.UsName;
			parameters[5].Value = model.PayCent;
			parameters[6].Value = model.Content;
			parameters[7].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_ChatLog ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void DeleteStr(int ChatId, int Types)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_ChatLog ");
			strSql.Append(" where ChatId=@chatId ");
			strSql.Append(" and Types=@Types ");
			SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
			parameters[0].Value = ChatId;
			parameters[1].Value = Types;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void DeleteStr(int ChatId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_ChatLog ");
			strSql.Append(" where ChatId=@chatId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ChatId", SqlDbType.Int,4)};
			parameters[0].Value = ChatId;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.ChatLog GetChatLog(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,ChatId,UsID,UsName,PayCent,Content,AddTime from tb_ChatLog ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.ChatLog model=new BCW.Model.ChatLog();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.ChatId = reader.GetInt32(2);
					model.UsID = reader.GetInt32(3);
					model.UsName = reader.GetString(4);
					model.PayCent = reader.GetInt64(5);
					model.Content = reader.GetString(6);
					model.AddTime = reader.GetDateTime(7);
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
			strSql.Append(" FROM tb_ChatLog ");
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
		/// <returns>IList ChatLog</returns>
		public IList<BCW.Model.ChatLog> GetChatLogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.ChatLog> listChatLogs = new List<BCW.Model.ChatLog>();
			string sTable = "tb_ChatLog";
			string sPkey = "id";
			string sField = "ID,Types,ChatId,UsID,UsName,PayCent,Content,AddTime";
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
					return listChatLogs;
				}
				while (reader.Read())
				{
						BCW.Model.ChatLog objChatLog = new BCW.Model.ChatLog();
						objChatLog.ID = reader.GetInt32(0);
						objChatLog.Types = reader.GetInt32(1);
						objChatLog.ChatId = reader.GetInt32(2);
						objChatLog.UsID = reader.GetInt32(3);
						objChatLog.UsName = reader.GetString(4);
						objChatLog.PayCent = reader.GetInt64(5);
						objChatLog.Content = reader.GetString(6);
						objChatLog.AddTime = reader.GetDateTime(7);
						listChatLogs.Add(objChatLog);
				}
			}
			return listChatLogs;
		}

		#endregion  成员方法
	}
}

