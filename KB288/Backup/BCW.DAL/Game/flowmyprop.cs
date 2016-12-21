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
	/// ���ݷ�����flowmyprop��
	/// </summary>
	public class flowmyprop
	{
		public flowmyprop()
		{}
		#region  ��Ա����

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_flowmyprop"); 
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int UsID,int did)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_flowmyprop");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and did=@did ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@did", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = did;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(BCW.Model.Game.flowmyprop model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_flowmyprop(");
			strSql.Append("did,Title,dnum,ExTime,UsID)");
			strSql.Append(" values (");
            strSql.Append("@did,@Title,@dnum,@ExTime,@UsID)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@did", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@dnum", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
			parameters[0].Value = model.did;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.dnum;
			parameters[3].Value = model.ExTime;
            parameters[4].Value = model.UsID;

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
		public void Update(BCW.Model.Game.flowmyprop model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_flowmyprop set ");
            strSql.Append("dnum=dnum+@dnum");
			strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and did=@did");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@did", SqlDbType.Int,4),
					new SqlParameter("@dnum", SqlDbType.Int,4)};
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.did;
			parameters[2].Value = model.dnum;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// ʹ�õ���
        /// </summary>
        public void Update(int ID, int dnum, DateTime ExTime, int znum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowmyprop set ");
            strSql.Append("dnum=dnum+@dnum,");
            strSql.Append("znum=znum+@znum,");
            strSql.Append("ExTime=@ExTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ExTime", SqlDbType.DateTime),
					new SqlParameter("@dnum", SqlDbType.Int,4),
					new SqlParameter("@znum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = ExTime;
            parameters[2].Value = dnum;
            parameters[3].Value = znum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ʹ�õ���
        /// </summary>
        public void Update(int ID, int znum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_flowmyprop set ");
            strSql.Append("znum=znum+@znum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@znum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = znum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_flowmyprop ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// �õ�ĳ��������
        /// </summary>
        public int Getdnum(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 dnum from tb_flowmyprop ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.Model.Game.flowmyprop model = new BCW.Model.Game.flowmyprop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public BCW.Model.Game.flowmyprop Getflowmyprop(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,did,Title,dnum,ExTime,UsID,znum from tb_flowmyprop ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Model.Game.flowmyprop model=new BCW.Model.Game.flowmyprop();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.did = reader.GetInt32(1);
					model.Title = reader.GetString(2);
					model.dnum = reader.GetInt32(3);
					model.ExTime = reader.GetDateTime(4);
                    model.UsID = reader.GetInt32(5);
                    model.znum = reader.GetInt32(6);
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
			strSql.Append(" FROM tb_flowmyprop ");
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
		/// <returns>IList flowmyprop</returns>
		public IList<BCW.Model.Game.flowmyprop> Getflowmyprops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.flowmyprop> listflowmyprops = new List<BCW.Model.Game.flowmyprop>();
			string sTable = "tb_flowmyprop";
			string sPkey = "id";
			string sField = "ID,did,Title,dnum,ExTime,znum";
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
					return listflowmyprops;
				}
				while (reader.Read())
				{
						BCW.Model.Game.flowmyprop objflowmyprop = new BCW.Model.Game.flowmyprop();
						objflowmyprop.ID = reader.GetInt32(0);
						objflowmyprop.did = reader.GetInt32(1);
						objflowmyprop.Title = reader.GetString(2);
						objflowmyprop.dnum = reader.GetInt32(3);
						objflowmyprop.ExTime = reader.GetDateTime(4);
                        objflowmyprop.znum = reader.GetInt32(5);
						listflowmyprops.Add(objflowmyprop);
				}
			}
			return listflowmyprops;
		}

		#endregion  ��Ա����
	}
}

