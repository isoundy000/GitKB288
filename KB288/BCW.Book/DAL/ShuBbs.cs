using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace Book.DAL
{
	/// <summary>
	/// ���ݷ�����ShuBbs��
	/// </summary>
	public class ShuBbs
	{
		public ShuBbs()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelperBook.GetMaxID("id", "ShuBbs"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from ShuBbs");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return SqlHelperBook.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(Book.Model.ShuBbs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into ShuBbs(");
			strSql.Append("nid,usid,usname,content,addtime,addusip,retext)");
			strSql.Append(" values (");
			strSql.Append("@nid,@usid,@usname,@content,@addtime,@addusip,@retext)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@content", SqlDbType.NVarChar,300),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@addusip", SqlDbType.NVarChar,50),
					new SqlParameter("@retext", SqlDbType.NVarChar,300)};
			parameters[0].Value = model.nid;
			parameters[1].Value = model.usid;
			parameters[2].Value = model.usname;
			parameters[3].Value = model.content;
			parameters[4].Value = model.addtime;
			parameters[5].Value = model.addusip;
			parameters[6].Value = model.retext;

			object obj = SqlHelperBook.GetSingle(strSql.ToString(),parameters);
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
		/// ����һ������
		/// </summary>
		public void Update(Book.Model.ShuBbs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update ShuBbs set ");
			strSql.Append("nid=@nid,");
			strSql.Append("usid=@usid,");
			strSql.Append("usname=@usname,");
			strSql.Append("content=@content,");
			strSql.Append("addtime=@addtime,");
			strSql.Append("addusip=@addusip,");
			strSql.Append("retext=@retext");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@usname", SqlDbType.NVarChar,50),
					new SqlParameter("@content", SqlDbType.NVarChar,300),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@addusip", SqlDbType.NVarChar,50),
					new SqlParameter("@retext", SqlDbType.NVarChar,300)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.nid;
			parameters[2].Value = model.usid;
			parameters[3].Value = model.usname;
			parameters[4].Value = model.content;
			parameters[5].Value = model.addtime;
			parameters[6].Value = model.addusip;
			parameters[7].Value = model.retext;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from ShuBbs ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.ShuBbs GetShuBbs(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,nid,usid,usname,content,addtime,addusip,retext from ShuBbs ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			Book.Model.ShuBbs model=new Book.Model.ShuBbs();
			using (SqlDataReader reader=SqlHelperBook.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.id = reader.GetInt32(0);
					model.nid = reader.GetInt32(1);
					model.usid = reader.GetInt32(2);
					model.usname = reader.GetString(3);
					model.content = reader.GetString(4);
					model.addtime = reader.GetDateTime(5);
					model.addusip = reader.GetString(6);
					model.retext = reader.GetString(7);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM ShuBbs ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelperBook.Query(strSql.ToString());
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList ShuBbs</returns>
		public IList<Book.Model.ShuBbs> GetShuBbss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<Book.Model.ShuBbs> listShuBbss = new List<Book.Model.ShuBbs>();
			string sTable = "ShuBbs";
			string sPkey = "id";
			string sField = "id,nid,usid,usname,content,addtime,addusip,retext";
			string sCondition = strWhere;
			string sOrder = "ID Desc";
			int iSCounts = 0;
			using (SqlDataReader reader = SqlHelperBook.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
			{
				//������ҳ��
				if (p_recordCount > 0)
				{
					int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
				}
				else
				{
					return listShuBbss;
				}
				while (reader.Read())
				{
						Book.Model.ShuBbs objShuBbs = new Book.Model.ShuBbs();
						objShuBbs.id = reader.GetInt32(0);
						objShuBbs.nid = reader.GetInt32(1);
						objShuBbs.usid = reader.GetInt32(2);
						objShuBbs.usname = reader.GetString(3);
						objShuBbs.content = reader.GetString(4);
						objShuBbs.addtime = reader.GetDateTime(5);
						objShuBbs.addusip = reader.GetString(6);
						objShuBbs.retext = reader.GetString(7);
						listShuBbss.Add(objShuBbs);
				}
			}
			return listShuBbss;
		}

		#endregion  ��Ա����
	}
}

