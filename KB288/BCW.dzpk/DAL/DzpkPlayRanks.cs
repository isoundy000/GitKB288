using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.dzpk.DAL
{
	/// <summary>
	/// ���ݷ�����DzpkPlayRanks��
	/// </summary>
	public class DzpkPlayRanks
	{
		public DzpkPlayRanks()
		{}
		#region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_DzpkPlayRanks");
        }

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkPlayRanks");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.dzpk.Model.DzpkPlayRanks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkPlayRanks(");
			strSql.Append("RmID,UsID,RankChk,RankBanker,RankMake,RankChips,PokerCards,PokerChips,TimeOut,RmMake,UsPot)");
			strSql.Append(" values (");
			strSql.Append("@RmID,@UsID,@RankChk,@RankBanker,@RankMake,@RankChips,@PokerCards,@PokerChips,@TimeOut,@RmMake,@UsPot)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@RankChk", SqlDbType.Int,4),
					new SqlParameter("@RankBanker", SqlDbType.NVarChar,1),
					new SqlParameter("@RankMake", SqlDbType.NVarChar,1),
					new SqlParameter("@RankChips", SqlDbType.NVarChar,1),
					new SqlParameter("@PokerCards", SqlDbType.NVarChar,50),
					new SqlParameter("@PokerChips", SqlDbType.NVarChar,50),
					new SqlParameter("@TimeOut", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPot", SqlDbType.BigInt,8)};
			parameters[0].Value = model.RmID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.RankChk;
			parameters[3].Value = model.RankBanker;
			parameters[4].Value = model.RankMake;
			parameters[5].Value = model.RankChips;
			parameters[6].Value = model.PokerCards;
			parameters[7].Value = model.PokerChips;
			parameters[8].Value = model.TimeOut;
			parameters[9].Value = model.RmMake;
			parameters[10].Value = model.UsPot;

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
		public void Update(BCW.dzpk.Model.DzpkPlayRanks model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkPlayRanks set ");
			strSql.Append("RmID=@RmID,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("RankChk=@RankChk,");
			strSql.Append("RankBanker=@RankBanker,");
			strSql.Append("RankMake=@RankMake,");
			strSql.Append("RankChips=@RankChips,");
			strSql.Append("PokerCards=@PokerCards,");
			strSql.Append("PokerChips=@PokerChips,");
			strSql.Append("TimeOut=@TimeOut,");
			strSql.Append("RmMake=@RmMake,");
			strSql.Append("UsPot=@UsPot,");
            strSql.Append("TimeOutCount=@TimeOutCount");
            strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RmID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@RankChk", SqlDbType.Int,4),
					new SqlParameter("@RankBanker", SqlDbType.NVarChar,1),
					new SqlParameter("@RankMake", SqlDbType.NVarChar,1),
					new SqlParameter("@RankChips", SqlDbType.NVarChar,1),
					new SqlParameter("@PokerCards", SqlDbType.NVarChar,50),
					new SqlParameter("@PokerChips", SqlDbType.NVarChar,50),
					new SqlParameter("@TimeOut", SqlDbType.DateTime),
					new SqlParameter("@RmMake", SqlDbType.NVarChar,50),
					new SqlParameter("@UsPot", SqlDbType.BigInt,8),
                    new SqlParameter("@TimeOutCount", SqlDbType.Int,4)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.RmID;
			parameters[2].Value = model.UsID;
			parameters[3].Value = model.RankChk;
			parameters[4].Value = model.RankBanker;
			parameters[5].Value = model.RankMake;
			parameters[6].Value = model.RankChips;
			parameters[7].Value = model.PokerCards;
			parameters[8].Value = model.PokerChips;
			parameters[9].Value = model.TimeOut;
			parameters[10].Value = model.RmMake;
			parameters[11].Value = model.UsPot;
            parameters[12].Value = model.TimeOutCount;
            SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkPlayRanks ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        public void DeleteByRmID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_DzpkPlayRanks ");
            strSql.Append(" where RmID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.dzpk.Model.DzpkPlayRanks GetDzpkPlayRanks(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RmID,UsID,RankChk,RankBanker,RankMake,RankChips,PokerCards,PokerChips,TimeOut,RmMake,UsPot,TimeOutCount from tb_DzpkPlayRanks ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.dzpk.Model.DzpkPlayRanks model=new BCW.dzpk.Model.DzpkPlayRanks();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.RmID = reader.GetInt32(1);
					model.UsID = reader.GetInt32(2);
					model.RankChk = reader.GetInt32(3);
					model.RankBanker = reader.GetString(4);
					model.RankMake = reader.GetString(5);
					model.RankChips = reader.GetString(6);
					model.PokerCards = reader.GetString(7);
					model.PokerChips = reader.GetString(8);
					model.TimeOut = reader.GetDateTime(9);
					model.RmMake = reader.GetString(10);
					model.UsPot = reader.GetInt64(11);
                    model.TimeOutCount = reader.GetInt32(12);
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
			strSql.Append(" FROM tb_DzpkPlayRanks ");
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
		/// <returns>IList DzpkPlayRanks</returns>
		public IList<BCW.dzpk.Model.DzpkPlayRanks> GetDzpkPlayRankss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkPlayRanks> listDzpkPlayRankss = new List<BCW.dzpk.Model.DzpkPlayRanks>();
			string sTable = "tb_DzpkPlayRanks";
			string sPkey = "id";
			string sField = "ID,RmID,UsID,RankChk,RankBanker,RankMake,RankChips,PokerCards,PokerChips,TimeOut,RmMake,UsPot";
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
					return listDzpkPlayRankss;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkPlayRanks objDzpkPlayRanks = new BCW.dzpk.Model.DzpkPlayRanks();
						objDzpkPlayRanks.ID = reader.GetInt32(0);
						objDzpkPlayRanks.RmID = reader.GetInt32(1);
						objDzpkPlayRanks.UsID = reader.GetInt32(2);
						objDzpkPlayRanks.RankChk = reader.GetInt32(3);
						objDzpkPlayRanks.RankBanker = reader.GetString(4);
						objDzpkPlayRanks.RankMake = reader.GetString(5);
						objDzpkPlayRanks.RankChips = reader.GetString(6);
						objDzpkPlayRanks.PokerCards = reader.GetString(7);
						objDzpkPlayRanks.PokerChips = reader.GetString(8);
						objDzpkPlayRanks.TimeOut = reader.GetDateTime(9);
						objDzpkPlayRanks.RmMake = reader.GetString(10);
						objDzpkPlayRanks.UsPot = reader.GetInt64(11);
						listDzpkPlayRankss.Add(objDzpkPlayRanks);
				}
			}
			return listDzpkPlayRankss;
		}

		#endregion  ��Ա����
	}
}

