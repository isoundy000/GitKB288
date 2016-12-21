using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.XinKuai3.DAL
{
	/// <summary>
	/// ���ݷ�����Public_User��
	/// </summary>
	public class Public_User
	{
		public Public_User()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Public_User"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Public_User");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.XinKuai3.Model.Public_User model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Public_User(");
			strSql.Append("UsID,UsName,Settings,Type)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@UsName,@Settings,@Type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Settings", SqlDbType.NVarChar),
					new SqlParameter("@Type", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.Settings;
			parameters[3].Value = model.Type;

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
		public void Update(BCW.XinKuai3.Model.Public_User model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Public_User set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("Settings=@Settings,");
			strSql.Append("Type=@Type");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Settings", SqlDbType.NVarChar),
					new SqlParameter("@Type", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.Settings;
			parameters[4].Value = model.Type;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Public_User ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.XinKuai3.Model.Public_User GetPublic_User(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,UsName,Settings,Type from tb_Public_User ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.XinKuai3.Model.Public_User model=new BCW.XinKuai3.Model.Public_User();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.Settings = reader.GetString(3);
					model.Type = reader.GetInt32(4);
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
			strSql.Append(" FROM tb_Public_User ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        ///  me_���¿����ע
        /// </summary>
        public void Update_1(int UsID, string Settings, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Public_User set ");
            strSql.Append("Settings=@Settings ");
            strSql.Append(" where UsID=@UsID and type=@type");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@Settings", SqlDbType.NVarChar),
            new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Settings;
            parameters[2].Value = type;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Public_User");
            strSql.Append(" where UsID=@UsID and type=@type");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
            new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = type;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Public_User</returns>
        public IList<BCW.XinKuai3.Model.Public_User> GetPublic_Users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.XinKuai3.Model.Public_User> listPublic_Users = new List<BCW.XinKuai3.Model.Public_User>();
			string sTable = "tb_Public_User";
			string sPkey = "id";
			string sField = "ID,UsID,UsName,Settings,Type";
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
					return listPublic_Users;
				}
				while (reader.Read())
				{
						BCW.XinKuai3.Model.Public_User objPublic_User = new BCW.XinKuai3.Model.Public_User();
						objPublic_User.ID = reader.GetInt32(0);
						objPublic_User.UsID = reader.GetInt32(1);
						objPublic_User.UsName = reader.GetString(2);
						objPublic_User.Settings = reader.GetString(3);
						objPublic_User.Type = reader.GetInt32(4);
						listPublic_Users.Add(objPublic_User);
				}
			}
			return listPublic_Users;
		}

		#endregion  ��Ա����
	}
}

