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
	/// ���ݷ�����Freemoney��
	/// </summary>
	public class Freemoney
	{
		public Freemoney()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Freemoney"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Freemoney");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.Game.Freemoney model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Freemoney(");
			strSql.Append("zid,kid,kname,winmoney)");
			strSql.Append(" values (");
			strSql.Append("@zid,@kid,@kname,@winmoney)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@kid", SqlDbType.Int,4),
					new SqlParameter("@kname", SqlDbType.NVarChar,50),
					new SqlParameter("@winmoney", SqlDbType.Money,8)};
			parameters[0].Value = model.zid;
			parameters[1].Value = model.kid;
			parameters[2].Value = model.kname;
			parameters[3].Value = model.winmoney;

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
		public void Update(BCW.Model.Game.Freemoney model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Freemoney set ");
			strSql.Append("zid=@zid,");
			strSql.Append("kid=@kid,");
			strSql.Append("kname=@kname,");
			strSql.Append("winmoney=@winmoney");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@zid", SqlDbType.Int,4),
					new SqlParameter("@kid", SqlDbType.Int,4),
					new SqlParameter("@kname", SqlDbType.NVarChar,50),
					new SqlParameter("@winmoney", SqlDbType.Money,8)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.zid;
			parameters[2].Value = model.kid;
			parameters[3].Value = model.kname;
			parameters[4].Value = model.winmoney;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Freemoney ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.Freemoney GetFreemoney(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,zid,kid,kname,winmoney from tb_Freemoney ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.Freemoney model=new BCW.Model.Game.Freemoney();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.zid = reader.GetInt32(1);
					model.kid = reader.GetInt32(2);
					model.kname = reader.GetString(3);
					model.winmoney = reader.GetDecimal(4);
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
			strSql.Append(" FROM tb_Freemoney ");
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
		/// <returns>IList Freemoney</returns>
		public IList<BCW.Model.Game.Freemoney> GetFreemoneys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.Freemoney> listFreemoneys = new List<BCW.Model.Game.Freemoney>();
			string sTable = "tb_Freemoney";
			string sPkey = "id";
			string sField = "ID,zid,kid,kname,winmoney";
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
					return listFreemoneys;
				}
				while (reader.Read())
				{
						BCW.Model.Game.Freemoney objFreemoney = new BCW.Model.Game.Freemoney();
						objFreemoney.ID = reader.GetInt32(0);
						objFreemoney.zid = reader.GetInt32(1);
						objFreemoney.kid = reader.GetInt32(2);
						objFreemoney.kname = reader.GetString(3);
						objFreemoney.winmoney = reader.GetDecimal(4);
						listFreemoneys.Add(objFreemoney);
				}
			}
			return listFreemoneys;
		}

		#endregion  ��Ա����
	}
}

