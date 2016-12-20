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
	/// ���ݷ�����Navigation��
	/// </summary>
	public class Navigation
	{
		public Navigation()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelperBook.GetMaxID("id", "Navigation"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Navigation");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return SqlHelperBook.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(Book.Model.Navigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Navigation(");
			strSql.Append("pid,Name)");
			strSql.Append(" values (");
			strSql.Append("@pid,@Name)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@pid", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,50)};
			parameters[0].Value = model.pid;
			parameters[1].Value = model.Name;

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
		public void Update(Book.Model.Navigation model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Navigation set ");
			strSql.Append("pid=@pid,");
			strSql.Append("Name=@Name");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@pid", SqlDbType.Int,4),
					new SqlParameter("@Name", SqlDbType.VarChar,50)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.pid;
			parameters[2].Value = model.Name;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Navigation ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			SqlHelperBook.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public Book.Model.Navigation GetNavigation(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,pid,Name from Navigation ");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			Book.Model.Navigation model=new Book.Model.Navigation();
			using (SqlDataReader reader=SqlHelperBook.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.id = reader.GetInt32(0);
					model.pid = reader.GetInt32(1);
					model.Name = reader.GetString(2);
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
			strSql.Append(" FROM Navigation ");
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
		/// <returns>IList Navigation</returns>
		public IList<Book.Model.Navigation> GetNavigations(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<Book.Model.Navigation> listNavigations = new List<Book.Model.Navigation>();
			string sTable = "Navigation";
			string sPkey = "id";
			string sField = "id,pid,Name";
			string sCondition = strWhere;
			string sOrder = "pid asc";
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
					return listNavigations;
				}
				while (reader.Read())
				{
						Book.Model.Navigation objNavigation = new Book.Model.Navigation();
						objNavigation.id = reader.GetInt32(0);
						objNavigation.pid = reader.GetInt32(1);
						objNavigation.Name = reader.GetString(2);
						listNavigations.Add(objNavigation);
				}
			}
			return listNavigations;
		}

		#endregion  ��Ա����
	}
}

