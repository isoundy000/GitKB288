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
	/// ���ݷ�����Favgroup��
	/// </summary>
	public class Favgroup
	{
		public Favgroup()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Favgroup"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Favgroup");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Favgroup");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsTitle(int UsID, string Title, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Favgroup");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Title=@Title ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Title;
            parameters[2].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.Favgroup model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Favgroup(");
			strSql.Append("Types,Title,UsID,Paixu,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@Types,@Title,@UsID,@Paixu,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.Paixu;
			parameters[4].Value = model.AddTime;

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
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.Favgroup model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Favgroup set ");
			strSql.Append("Types=@Types,");
			strSql.Append("Title=@Title,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Paixu=@Paixu,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.Title;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.Paixu;
			parameters[5].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Favgroup ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// �õ�Title
        /// </summary>
        public string GetTitle(int ID, int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Title from tb_Favgroup ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = Types;

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
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Favgroup GetFavgroup(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,Title,UsID,Paixu,AddTime from tb_Favgroup ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Favgroup model=new BCW.Model.Favgroup();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.Title = reader.GetString(2);
					model.UsID = reader.GetInt32(3);
					model.Paixu = reader.GetInt32(4);
					model.AddTime = reader.GetDateTime(5);
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
			strSql.Append(" FROM tb_Favgroup ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList Favgroup</returns>
		public IList<BCW.Model.Favgroup> GetFavgroups(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Favgroup> listFavgroups = new List<BCW.Model.Favgroup>();
			string sTable = "tb_Favgroup";
			string sPkey = "id";
			string sField = "ID,Types,Title,UsID,Paixu,AddTime";
			string sCondition = strWhere;
			string sOrder = "ID Desc";
			int iSCounts = 0;
			using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
			{
				//������ҳ��
				if (p_recordCount > 0)
				{
					int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
				}
				else
				{
					return listFavgroups;
				}
				while (reader.Read())
				{
						BCW.Model.Favgroup objFavgroup = new BCW.Model.Favgroup();
						objFavgroup.ID = reader.GetInt32(0);
						objFavgroup.Types = reader.GetInt32(1);
						objFavgroup.Title = reader.GetString(2);
						objFavgroup.UsID = reader.GetInt32(3);
						objFavgroup.Paixu = reader.GetInt32(4);
						objFavgroup.AddTime = reader.GetDateTime(5);
						listFavgroups.Add(objFavgroup);
				}
			}
			return listFavgroups;
		}

		#endregion  ��Ա����
	}
}

