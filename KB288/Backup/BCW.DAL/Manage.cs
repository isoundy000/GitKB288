using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using BCW.Common;
using System.Data.SqlClient;
using BCW.Data;

namespace BCW.DAL
{
	/// <summary>
	/// ���ݷ�����Manage��
	/// </summary>
	public class Manage
	{
		public Manage()
		{}
		#region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Manage");
        }

        /// <summary>
        /// ��ѯӰ�������
        /// </summary>
        /// <returns></returns>
        public int GetManageRow(BCW.Model.Manage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID from tb_Manage where ");
            strSql.Append("sUser=@sUser and ");
            strSql.Append("sPwd=@sPwd");
            SqlParameter[] parameters = {
					new SqlParameter("@sUser", SqlDbType.NVarChar,50),
					new SqlParameter("@sPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.sUser;
            parameters[1].Value = model.sPwd;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsKeys(string sKeys)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Manage");
            strSql.Append(" where sKeys=@sKeys ");
            SqlParameter[] parameters = {
					new SqlParameter("@sKeys", SqlDbType.NVarChar,50)};
            parameters[0].Value = sKeys;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsUser(string sUser)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Manage");
            strSql.Append(" where sUser=@sUser ");
            SqlParameter[] parameters = {
					new SqlParameter("@sUser", SqlDbType.NVarChar,50)};
            parameters[0].Value = sUser;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsUser(string sUser, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Manage");
            strSql.Append(" where sUser=@sUser ");
            strSql.Append(" and ID<>@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@sUser", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = sUser;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Manage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Manage(");
            strSql.Append("sUser,sPwd,sKeys,sTime,sUserIP)");
            strSql.Append(" values (");
            strSql.Append("@sUser,@sPwd,@sKeys,@sTime,@sUserIP)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@sUser", SqlDbType.NVarChar,50),
					new SqlParameter("@sPwd", SqlDbType.NVarChar,50),
					new SqlParameter("@sKeys", SqlDbType.NVarChar,50),
					new SqlParameter("@sTime", SqlDbType.DateTime),
					new SqlParameter("@sUserIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.sUser;
            parameters[1].Value = model.sPwd;
            parameters[2].Value = model.sKeys;
            parameters[3].Value = model.sTime;
            parameters[4].Value = Ipaddr.IPAddress;

            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// ���º�̨��¼ʱ��/ip
        /// </summary>
        public void UpdateTimeIP(BCW.Model.Manage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Manage set ");
            strSql.Append("sTime=@sTime,");
            strSql.Append("sUserIP=@sUserIP");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@sTime", SqlDbType.DateTime),
                    new SqlParameter("@sUserIP", SqlDbType.NVarChar,50),
            		new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.sTime;
            parameters[1].Value = Utils.GetUsIP();
            parameters[2].Value = model.ID;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���º�̨Keys
        /// </summary>
        public void UpdateKeys(BCW.Model.Manage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Manage set ");
            strSql.Append("sKeys=@sKeys");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
	            new SqlParameter("@sKeys", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.sKeys;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Manage model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Manage set ");
            strSql.Append("sUser=@sUser,");
            strSql.Append("sPwd=@sPwd");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@sUser", SqlDbType.NVarChar,50),
					new SqlParameter("@sPwd", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.sUser;
            parameters[2].Value = model.sPwd;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Manage ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����Keys�õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Manage GetModelByKeys(string sKeys)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,sUser,sPwd,sKeys,sTime from tb_Manage ");
            strSql.Append(" where sKeys=@sKeys ");
            SqlParameter[] parameters = {
					new SqlParameter("@sKeys", SqlDbType.NVarChar,50)};
            parameters[0].Value = sKeys;

            BCW.Model.Manage model = new BCW.Model.Manage();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                model.sUser = ds.Tables[0].Rows[0]["sUser"].ToString();
                model.sPwd = ds.Tables[0].Rows[0]["sPwd"].ToString();
                model.sKeys = ds.Tables[0].Rows[0]["sKeys"].ToString();
                if (ds.Tables[0].Rows[0]["sTime"].ToString() != "")
                {
                    model.sTime = DateTime.Parse(ds.Tables[0].Rows[0]["sTime"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// ����IDȡ�ù���ԱKeys
        /// </summary>
        public string GetKeys(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 sKeys from tb_Manage ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// ����IDȡ�ù���ԱKeys
        /// </summary>
        public string GetKeys()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 sKeys from tb_Manage ");
            strSql.Append(" Order by ID Desc");

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// ����IDȡ�ù���Ա��������
        /// </summary>
        public BCW.Model.Manage GetModel(int ID)
        {
            BCW.Model.Manage model = new BCW.Model.Manage();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,sUser,sKeys from tb_Manage ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.sUser = reader.GetString(1);
                    model.sKeys = reader.GetString(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �����û�������õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Manage GetModelByModel(string sUser, string sPwd)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,sKeys,sTime from tb_Manage ");
            strSql.Append(" where sUser='" + sUser + "'");
            strSql.Append(" and sPwd='" + sPwd + "'");

            BCW.Model.Manage model = new BCW.Model.Manage();
            DataSet ds = SqlHelper.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                model.sKeys = ds.Tables[0].Rows[0]["sKeys"].ToString();
                if (ds.Tables[0].Rows[0]["sTime"].ToString() != "")
                {
                    model.sTime = DateTime.Parse(ds.Tables[0].Rows[0]["sTime"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetManageList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_Manage ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>IList Manage</returns>
        public IList<BCW.Model.Manage> GetManages(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Manage> listManages = new List<BCW.Model.Manage>();

            string sTable = "tb_Manage";
            string sPkey = "id";
            string sField = "id,sUser,sTime,sUserIP";
            string sCondition = "";
            string sOrder = "id ASC";
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

                    return listManages;
                }

                while (reader.Read())
                {
                    BCW.Model.Manage objManage = new BCW.Model.Manage();
                    objManage.ID = reader.GetInt32(0);
                    objManage.sUser = reader.GetString(1);
                    objManage.sTime = reader.GetDateTime(2);
                    if (!reader.IsDBNull(3))
                        objManage.sUserIP = reader.GetString(3);

                    listManages.Add(objManage);


                }
            }

            return listManages;
        }

		#endregion  ��Ա����
	}
}

