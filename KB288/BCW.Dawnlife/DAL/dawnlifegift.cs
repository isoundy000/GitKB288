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
	/// ���ݷ�����dawnlifegift��
	/// </summary>
	public class dawnlifegift
	{
		public dawnlifegift()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifegift"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifegift");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.dawnlifegift model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifegift(");
			strSql.Append("date,gift,UsID,UsName,coin,state,giftj)");
			strSql.Append(" values (");
			strSql.Append("@date,@gift,@UsID,@UsName,@coin,@state,@giftj)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@date", SqlDbType.DateTime),
					new SqlParameter("@gift", SqlDbType.BigInt,8),                   
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@state", SqlDbType.Int,4),
                    new SqlParameter("@giftj", SqlDbType.BigInt,8)};
			parameters[0].Value = model.date;
			parameters[1].Value = model.gift;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.UsName;
			parameters[4].Value = model.coin;
			parameters[5].Value = model.state;
            parameters[6].Value = model.giftj;
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
		public void Update(BCW.Model.dawnlifegift model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifegift set ");
			strSql.Append("date=@date,");
			strSql.Append("gift=@gift,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("coin=@coin,");
			strSql.Append("state=@state"); 
            strSql.Append("gift=@giftj,");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@date", SqlDbType.DateTime),
					new SqlParameter("@gift", SqlDbType.BigInt,8),                  	
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@state", SqlDbType.Int,4),
                                        new SqlParameter("@giftj", SqlDbType.BigInt,8),};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.date;
			parameters[2].Value = model.gift;
			parameters[3].Value = model.UsID;
			parameters[4].Value = model.UsName;
			parameters[5].Value = model.coin;
			parameters[6].Value = model.state;
            parameters[7].Value = model.giftj;
			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifegift ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.dawnlifegift Getdawnlifegift(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,date,gift,UsID,UsName,coin,state,giftj from tb_dawnlifegift ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifegift model=new BCW.Model.dawnlifegift();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.date = reader.GetDateTime(1);
					model.gift = reader.GetInt64(2);
					model.UsID = reader.GetInt32(3);
					model.UsName = reader.GetString(4);
					model.coin = reader.GetInt64(5);
					model.state = reader.GetInt32(6);
                    model.giftj = reader.GetInt64(7);
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
			strSql.Append(" FROM tb_dawnlifegift ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

        /// <summary>
        /// ����gift�ܱ�ֵ
        /// </summary>
        public long GetPrice(string coin, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(" + coin + ") from tb_dawnlifegift");
            strSql.Append(" where " + strWhere + " ");
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList dawnlifegift</returns>
		public IList<BCW.Model.dawnlifegift> Getdawnlifegifts(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifegift> listdawnlifegifts = new List<BCW.Model.dawnlifegift>();
            string sTable = "tb_dawnlifegift";
			string sPkey = "id";
			string sField = "ID,date,gift,UsID,UsName,coin,state,giftj";
			string sCondition = strWhere;
            string sOrder = strOrder;
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
					return listdawnlifegifts;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifegift objdawnlifegift = new BCW.Model.dawnlifegift();
						objdawnlifegift.ID = reader.GetInt32(0);
						objdawnlifegift.date = reader.GetDateTime(1);
						objdawnlifegift.gift = reader.GetInt64(2);
						objdawnlifegift.UsID = reader.GetInt32(3);
						objdawnlifegift.UsName = reader.GetString(4);
						objdawnlifegift.coin = reader.GetInt64(5);
						objdawnlifegift.state = reader.GetInt32(6);
                        objdawnlifegift.giftj = reader.GetInt64(7);
						listdawnlifegifts.Add(objdawnlifegift);
				}
			}
			return listdawnlifegifts;
		}

		#endregion  ��Ա����
	}
}

