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
	/// ���ݷ�����dawnlifeUser��
	/// </summary>
	public class dawnlifeUser
	{
		public dawnlifeUser()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_dawnlifeUser"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_dawnlifeUser");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.dawnlifeUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_dawnlifeUser(");
			strSql.Append("UsID,UsName,coin,money,debt,health,reputation,storehouse,stock,city)");
			strSql.Append(" values (");
			strSql.Append("@UsID,@UsName,@coin,@money,@debt,@health,@reputation,@storehouse,@stock,@city)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@debt", SqlDbType.BigInt,8),
					new SqlParameter("@health", SqlDbType.Int,4),
					new SqlParameter("@reputation", SqlDbType.Int,4),
					new SqlParameter("@storehouse", SqlDbType.NVarChar,50),
					new SqlParameter("@stock", SqlDbType.NVarChar,50),
                                        new SqlParameter("@city", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.coin;
			parameters[3].Value = model.money;
			parameters[4].Value = model.debt;
			parameters[5].Value = model.health;
			parameters[6].Value = model.reputation;
			parameters[7].Value = model.storehouse;
			parameters[8].Value = model.stock;
            parameters[9].Value = model.city;

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
		public void Update(BCW.Model.dawnlifeUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_dawnlifeUser set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("coin=@coin,");
			strSql.Append("money=@money,");
			strSql.Append("debt=@debt,");
			strSql.Append("health=@health,");
			strSql.Append("reputation=@reputation,");
			strSql.Append("storehouse=@storehouse,");
			strSql.Append("stock=@stock,");
            strSql.Append("city=@city");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@coin", SqlDbType.BigInt,8),
					new SqlParameter("@money", SqlDbType.BigInt,8),
					new SqlParameter("@debt", SqlDbType.BigInt,8),
					new SqlParameter("@health", SqlDbType.Int,4),
					new SqlParameter("@reputation", SqlDbType.Int,4),
					new SqlParameter("@storehouse", SqlDbType.NVarChar,50),
					new SqlParameter("@stock", SqlDbType.NVarChar,50),
                                        new SqlParameter("@city", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.coin;
			parameters[4].Value = model.money;
			parameters[5].Value = model.debt;
			parameters[6].Value = model.health;
			parameters[7].Value = model.reputation;
			parameters[8].Value = model.storehouse;
			parameters[9].Value = model.stock;
            parameters[10].Value = model.city;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        //����һ���ֶ�
        public void Updatedebt(int ID, long debt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("debt=@debt ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@debt", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = debt;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void Updatemoney(int ID, long money)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("money=@money ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@money", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = money;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void Updatecoin(int ID, long coin)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("coin=@coin ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@coin", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = coin;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void UpdateStock(int ID, string stock)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("stock=@stock ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@stock", SqlDbType.NChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = stock;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void UpdateStorehouse(int ID, string storehouse)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("storehouse=@storehouse ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@storehouse", SqlDbType.NChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = storehouse;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void Updatehealth(int ID, int health)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("health=@health ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@health", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = health;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void Updatereputation(int ID, int reputation)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_dawnlifeUser set ");
            strSql.Append("reputation=@reputation ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@reputation", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = reputation;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_dawnlifeUser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.dawnlifeUser GetdawnlifeUser(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,UsName,coin,money,debt,health,reputation,storehouse,stock,city from tb_dawnlifeUser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.dawnlifeUser model=new BCW.Model.dawnlifeUser();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.coin = reader.GetInt64(3);
					model.money = reader.GetInt64(4);
					model.debt = reader.GetInt64(5);
					model.health = reader.GetInt32(6);
					model.reputation = reader.GetInt32(7);
					model.storehouse = reader.GetString(8);
					model.stock = reader.GetString(9);
                    model.city = reader.GetString(10);
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
			strSql.Append(" FROM tb_dawnlifeUser ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}
        // me_��ʼ��ĳ���ݱ�
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// �����û�ID�Ϳ�Ҳ�ѯӰ���������ID
        /// </summary>
        /// <returns></returns>
        public int GetRowByUsID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Top(1)* from tb_dawnlifeUser where UsID=@UsID order by coin desc ");
            //strSql.Append("UsID=@UsID ");
            //strSql.Append("and coin=@coin");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int),
					};
            parameters[0].Value = UsID ;
           

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
		/// ȡ��ÿҳ��¼
		/// </summary>
		/// <param name="p_pageIndex">��ǰҳ</param>
		/// <param name="p_pageSize">��ҳ��С</param>
		/// <param name="p_recordCount">�����ܼ�¼��</param>
		/// <param name="strWhere">��ѯ����</param>
		/// <returns>IList dawnlifeUser</returns>
		public IList<BCW.Model.dawnlifeUser> GetdawnlifeUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.dawnlifeUser> listdawnlifeUsers = new List<BCW.Model.dawnlifeUser>();
			string sTable = "tb_dawnlifeUser";
			string sPkey = "id";
			string sField = "ID,UsID,UsName,coin,money,debt,health,reputation,storehouse,stock,city";
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
					return listdawnlifeUsers;
				}
				while (reader.Read())
				{
						BCW.Model.dawnlifeUser objdawnlifeUser = new BCW.Model.dawnlifeUser();
						objdawnlifeUser.ID = reader.GetInt32(0);
						objdawnlifeUser.UsID = reader.GetInt32(1);
						objdawnlifeUser.UsName = reader.GetString(2);
						objdawnlifeUser.coin = reader.GetInt64(3);
						objdawnlifeUser.money = reader.GetInt64(4);
						objdawnlifeUser.debt = reader.GetInt64(5);
						objdawnlifeUser.health = reader.GetInt32(6);
						objdawnlifeUser.reputation = reader.GetInt32(7);
						objdawnlifeUser.storehouse = reader.GetString(8);
						objdawnlifeUser.stock = reader.GetString(9);
                        objdawnlifeUser.city = reader.GetString(10);
						listdawnlifeUsers.Add(objdawnlifeUser);
				}
			}
			return listdawnlifeUsers;
		}

		#endregion  ��Ա����
	}
}

