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
	/// ���ݷ�����MarryAction��
	/// </summary>
	public class MarryAction
	{
		public MarryAction()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_MarryAction"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_MarryAction");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.MarryAction model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_MarryAction(");
			strSql.Append("MarryId,Content,AddTime)");
			strSql.Append(" values (");
			strSql.Append("@MarryId,@Content,@AddTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.MarryId;
			parameters[1].Value = model.Content;
			parameters[2].Value = model.AddTime;

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
		public void Update(BCW.Model.MarryAction model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_MarryAction set ");
			strSql.Append("MarryId=@MarryId,");
			strSql.Append("Content=@Content,");
			strSql.Append("AddTime=@AddTime");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@MarryId", SqlDbType.Int,4),
					new SqlParameter("@Content", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.MarryId;
			parameters[2].Value = model.Content;
			parameters[3].Value = model.AddTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_MarryAction ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.MarryAction GetMarryAction(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,MarryId,Content,AddTime from tb_MarryAction ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.MarryAction model=new BCW.Model.MarryAction();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.MarryId = reader.GetInt32(1);
					model.Content = reader.GetString(2);
					model.AddTime = reader.GetDateTime(3);
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
			strSql.Append(" FROM tb_MarryAction ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList MarryAction</returns>
        public IList<BCW.Model.MarryAction> GetMarryActions(int SizeNum, string strWhere)
        {
            IList<BCW.Model.MarryAction> listMarryActions = new List<BCW.Model.MarryAction>();
            string sTable = "tb_MarryAction";
            string sPkey = "id";
            string sField = "ID,MarryId,Content,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listMarryActions;
                }
                while (reader.Read())
                {
                    BCW.Model.MarryAction objMarryAction = new BCW.Model.MarryAction();
                    objMarryAction.ID = reader.GetInt32(0);
                    objMarryAction.MarryId = reader.GetInt32(1);
                    objMarryAction.Content = reader.GetString(2);
                    objMarryAction.AddTime = reader.GetDateTime(3);
                    listMarryActions.Add(objMarryAction);
                }
            }
            return listMarryActions;
        }


		/// <summary>
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList MarryAction</returns>
		public IList<BCW.Model.MarryAction> GetMarryActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.MarryAction> listMarryActions = new List<BCW.Model.MarryAction>();
			string sTable = "tb_MarryAction";
			string sPkey = "id";
			string sField = "ID,MarryId,Content,AddTime";
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
					return listMarryActions;
				}
				while (reader.Read())
				{
                    BCW.Model.MarryAction objMarryAction = new BCW.Model.MarryAction();
                    objMarryAction.ID = reader.GetInt32(0);
                    objMarryAction.MarryId = reader.GetInt32(1);
                    objMarryAction.Content = reader.GetString(2);
                    objMarryAction.AddTime = reader.GetDateTime(3);
                    listMarryActions.Add(objMarryAction);
				}
			}
			return listMarryActions;
		}

		#endregion  ��Ա����
	}
}

