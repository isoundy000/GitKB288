using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
	/// <summary>
	/// ���ݷ�����NC_zhitiao��
	/// </summary>
	public class NC_zhitiao
	{
		public NC_zhitiao()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_NC_zhitiao"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_NC_zhitiao");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.farm.Model.NC_zhitiao model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_NC_zhitiao(");
			strSql.Append("UsID,contact,AddTime,type)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@contact,@AddTime,@type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@contact", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@type", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.contact;
			parameters[2].Value = model.AddTime;
			parameters[3].Value = model.type;

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
		public void Update(BCW.farm.Model.NC_zhitiao model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_NC_zhitiao set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("contact=@contact,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("type=@type");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@contact", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@type", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.contact;
			parameters[3].Value = model.AddTime;
			parameters[4].Value = model.type;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_NC_zhitiao ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.farm.Model.NC_zhitiao GetNC_zhitiao(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,contact,AddTime,type from tb_NC_zhitiao ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.farm.Model.NC_zhitiao model=new BCW.farm.Model.NC_zhitiao();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.contact = reader.GetString(2);
					model.AddTime = reader.GetDateTime(3);
					model.type = reader.GetInt32(4);
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
			strSql.Append(" FROM tb_NC_zhitiao ");
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
		/// <returns>IList NC_zhitiao</returns>
		public IList<BCW.farm.Model.NC_zhitiao> GetNC_zhitiaos(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.farm.Model.NC_zhitiao> listNC_zhitiaos = new List<BCW.farm.Model.NC_zhitiao>();
			string sTable = "tb_NC_zhitiao";
			string sPkey = "id";
			string sField = "ID,UsID,contact,AddTime,type";
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
					return listNC_zhitiaos;
				}
				while (reader.Read())
				{
						BCW.farm.Model.NC_zhitiao objNC_zhitiao = new BCW.farm.Model.NC_zhitiao();
						objNC_zhitiao.ID = reader.GetInt32(0);
						objNC_zhitiao.UsID = reader.GetInt32(1);
						objNC_zhitiao.contact = reader.GetString(2);
						objNC_zhitiao.AddTime = reader.GetDateTime(3);
						objNC_zhitiao.type = reader.GetInt32(4);
						listNC_zhitiaos.Add(objNC_zhitiao);
				}
			}
			return listNC_zhitiaos;
		}

		#endregion  ��Ա����
	}
}

