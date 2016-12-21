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
	/// ���ݷ�����tb_ZQCollection��
	/// </summary>
	public class tb_ZQCollection
	{
		public tb_ZQCollection()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_ZQCollection"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_ZQCollection");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool ExistsUsIdAndFootId(int UsId,int FootBallId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZQCollection");
            strSql.Append(" where UsId=@UsId and FootBallId=@FootBallId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsId", SqlDbType.Int,4),
            new SqlParameter("@FootBallId", SqlDbType.Int,4)};
            parameters[0].Value = UsId;
            parameters[1].Value = FootBallId;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
		/// 
		/// </summary>
		public int CountUsIdAndFootId(int FootBallId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ZQCollection");
            strSql.Append(" where   FootBallId=@FootBallId ");
            SqlParameter[] parameters = {
            new SqlParameter("@FootBallId", SqlDbType.Int,4)};
            parameters[0].Value = FootBallId;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.tb_ZQCollection model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_ZQCollection(");
			strSql.Append("UsId,FootBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark)");
			strSql.Append(" values (");
			strSql.Append("@UsId,@FootBallId,@Bianhao,@team1,@team2,@result,@sendCount,@AddTime,@ident,@Remark)");
			SqlParameter[] parameters = {
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@FootBallId", SqlDbType.Int,4),
					new SqlParameter("@Bianhao", SqlDbType.Int,4),
					new SqlParameter("@team1", SqlDbType.NChar,200),
					new SqlParameter("@team2", SqlDbType.NChar,200),
					new SqlParameter("@result", SqlDbType.NChar,50),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,500)};
			parameters[0].Value = model.UsId;
			parameters[1].Value = model.FootBallId;
			parameters[2].Value = model.Bianhao;
			parameters[3].Value = model.team1;
			parameters[4].Value = model.team2;
			parameters[5].Value = model.result;
			parameters[6].Value = model.sendCount;
			parameters[7].Value = model.AddTime;
			parameters[8].Value = model.ident;
			parameters[9].Value = model.Remark;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// ����һ������
		/// </summary>
		public void Update(BCW.Model.tb_ZQCollection model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_ZQCollection set ");
			strSql.Append("UsId=@UsId,");
			strSql.Append("FootBallId=@FootBallId,");
			strSql.Append("Bianhao=@Bianhao,");
			strSql.Append("team1=@team1,");
			strSql.Append("team2=@team2,");
			strSql.Append("result=@result,");
			strSql.Append("sendCount=@sendCount,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("ident=@ident,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@FootBallId", SqlDbType.Int,4),
					new SqlParameter("@Bianhao", SqlDbType.Int,4),
					new SqlParameter("@team1", SqlDbType.NChar,200),
					new SqlParameter("@team2", SqlDbType.NChar,200),
					new SqlParameter("@result", SqlDbType.NChar,50),
					new SqlParameter("@sendCount", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@ident", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NChar,500)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsId;
			parameters[2].Value = model.FootBallId;
			parameters[3].Value = model.Bianhao;
			parameters[4].Value = model.team1;
			parameters[5].Value = model.team2;
			parameters[6].Value = model.result;
			parameters[7].Value = model.sendCount;
			parameters[8].Value = model.AddTime;
			parameters[9].Value = model.ident;
			parameters[10].Value = model.Remark;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_ZQCollection ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.tb_ZQCollection Gettb_ZQCollection(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsId,FootBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark from tb_ZQCollection ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.tb_ZQCollection model=new BCW.Model.tb_ZQCollection();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsId = reader.GetInt32(1);
					model.FootBallId = reader.GetInt32(2);
					model.Bianhao = reader.GetInt32(3);
					model.team1 = reader.GetString(4);
					model.team2 = reader.GetString(5);
					model.result = reader.GetString(6);
					model.sendCount = reader.GetInt32(7);
					model.AddTime = reader.GetDateTime(8);
					model.ident = reader.GetInt32(9);
					model.Remark = reader.GetString(10);
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
			strSql.Append(" FROM tb_ZQCollection ");
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
		/// <returns>IList tb_ZQCollection</returns>
		public IList<BCW.Model.tb_ZQCollection> Gettb_ZQCollections(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.tb_ZQCollection> listtb_ZQCollections = new List<BCW.Model.tb_ZQCollection>();
			string sTable = "tb_ZQCollection";
			string sPkey = "id";
			string sField = "ID,UsId,FootBallId,Bianhao,team1,team2,result,sendCount,AddTime,ident,Remark";
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
					return listtb_ZQCollections;
				}
				while (reader.Read())
				{
						BCW.Model.tb_ZQCollection objtb_ZQCollection = new BCW.Model.tb_ZQCollection();
						objtb_ZQCollection.ID = reader.GetInt32(0);
						objtb_ZQCollection.UsId = reader.GetInt32(1);
						objtb_ZQCollection.FootBallId = reader.GetInt32(2);
						objtb_ZQCollection.Bianhao = reader.GetInt32(3);
						objtb_ZQCollection.team1 = reader.GetString(4);
						objtb_ZQCollection.team2 = reader.GetString(5);
						objtb_ZQCollection.result = reader.GetString(6);
						objtb_ZQCollection.sendCount = reader.GetInt32(7);
						objtb_ZQCollection.AddTime = reader.GetDateTime(8);
						objtb_ZQCollection.ident = reader.GetInt32(9);
						objtb_ZQCollection.Remark = reader.GetString(10);
						listtb_ZQCollections.Add(objtb_ZQCollection);
				}
			}
			return listtb_ZQCollections;
		}

		#endregion  ��Ա����
	}
}

