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
	/// ���ݷ�����SysTemp��
	/// </summary>
	public class SysTemp
	{
		public SysTemp()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_SysTemp"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_SysTemp");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.SysTemp model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SysTemp(");
			strSql.Append("GuessOddsTime)");
			strSql.Append(" values (");
			strSql.Append("@GuessOddsTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GuessOddsTime", SqlDbType.DateTime)};
			parameters[0].Value = model.GuessOddsTime;

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
        /// ����GuessOddsTime
		/// </summary>
        public void UpdateGuessOddsTime(int ID, DateTime GuessOddsTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SysTemp set ");
            strSql.Append("GuessOddsTime=@GuessOddsTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GuessOddsTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = GuessOddsTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SysTemp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
        /// �õ�GuessOddsTime
		/// </summary>
        public DateTime GetGuessOddsTime(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 GuessOddsTime from tb_SysTemp ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
                    return reader.GetDateTime(0);
				}
				else
				{
                    return DateTime.Parse("1990-1-1");
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
			strSql.Append(" FROM tb_SysTemp ");
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
		/// <returns>IList SysTemp</returns>
		public IList<BCW.Model.SysTemp> GetSysTemps(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.SysTemp> listSysTemps = new List<BCW.Model.SysTemp>();
			string sTable = "tb_SysTemp";
			string sPkey = "id";
			string sField = "ID,GuessOddsTime";
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
					return listSysTemps;
				}
				while (reader.Read())
				{
						BCW.Model.SysTemp objSysTemp = new BCW.Model.SysTemp();
						objSysTemp.ID = reader.GetInt32(0);
						objSysTemp.GuessOddsTime = reader.GetDateTime(1);
						listSysTemps.Add(objSysTemp);
				}
			}
			return listSysTemps;
		}

		#endregion  ��Ա����
	}
}

