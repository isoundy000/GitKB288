using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.bydr.DAL
{
	/// <summary>
	/// ���ݷ�����CmgTicket��
	/// </summary>
	public class CmgTicket
	{
		public CmgTicket()
		{}
		#region  ��Ա����


        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_CmgTicket");
        }
		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_CmgTicket");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsBID(int Bid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_CmgTicket");
            strSql.Append(" where Bid=@Bid ");
            SqlParameter[] parameters = {
					new SqlParameter("@Bid", SqlDbType.Int,4)};
            parameters[0].Value = Bid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.bydr.Model.CmgTicket model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_CmgTicket(");
			strSql.Append("ColletGold,Bid,Time,usID)");
			strSql.Append(" values (");
			strSql.Append("@ColletGold,@Bid,@Time,@usID)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ColletGold", SqlDbType.BigInt,8),
					new SqlParameter("@Bid", SqlDbType.Int,4),
					new SqlParameter("@Time", SqlDbType.DateTime),
					new SqlParameter("@usID", SqlDbType.Int,4)};
			parameters[0].Value = model.ColletGold;
			parameters[1].Value = model.Bid;
			parameters[2].Value = model.Time;
			parameters[3].Value = model.usID;

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
		public void Update(BCW.bydr.Model.CmgTicket model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_CmgTicket set ");
			strSql.Append("ColletGold=@ColletGold,");
			strSql.Append("Bid=@Bid,");
			strSql.Append("Time=@Time,");
			strSql.Append("usID=@usID");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ColletGold", SqlDbType.BigInt,8),
					new SqlParameter("@Bid", SqlDbType.Int,4),
					new SqlParameter("@Time", SqlDbType.DateTime),
					new SqlParameter("@usID", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.ColletGold;
			parameters[2].Value = model.Bid;
			parameters[3].Value = model.Time;
			parameters[4].Value = model.usID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_CmgTicket ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.bydr.Model.CmgTicket GetCmgTicket(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,ColletGold,Bid,Time,usID from tb_CmgTicket ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.bydr.Model.CmgTicket model=new BCW.bydr.Model.CmgTicket();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.ColletGold = reader.GetInt64(1);
					model.Bid = reader.GetInt32(2);
					model.Time = reader.GetDateTime(3);
					model.usID = reader.GetInt32(4);
					return model;
				}
				else
				{
				return null;
				}
			}
		}
        /// <summary>
        /// �õ�ȫ���ռ���
        /// </summary>
        public long GettoplistColletGoldsum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(ColletGold) from tb_CmgToplist");
            strSql.Append(" order by sum(ColletGold)");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
            SqlParameter[] parameters = {
					new SqlParameter("@ColletGold", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }

        }

		/// <summary>
		/// �����ֶ�ȡ�����б�
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_CmgTicket ");
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
		/// <returns>IList CmgTicket</returns>
		public IList<BCW.bydr.Model.CmgTicket> GetCmgTickets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.bydr.Model.CmgTicket> listCmgTickets = new List<BCW.bydr.Model.CmgTicket>();
			string sTable = "tb_CmgTicket";
			string sPkey = "id";
			string sField = "ID,ColletGold,Bid,Time,usID";
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
					return listCmgTickets;
				}
				while (reader.Read())
				{
						BCW.bydr.Model.CmgTicket objCmgTicket = new BCW.bydr.Model.CmgTicket();
						objCmgTicket.ID = reader.GetInt32(0);
						objCmgTicket.ColletGold = reader.GetInt64(1);
						objCmgTicket.Bid = reader.GetInt32(2);
						objCmgTicket.Time = reader.GetDateTime(3);
						objCmgTicket.usID = reader.GetInt32(4);
						listCmgTickets.Add(objCmgTicket);
				}
			}
			return listCmgTickets;
		}

		#endregion  ��Ա����
	}
}

