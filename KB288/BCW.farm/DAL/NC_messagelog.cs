using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
    /// <summary>
    /// ���ݷ�����NC_messagelog��
    /// </summary>
    public class NC_messagelog
    {
        public NC_messagelog()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_messagelog");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_messagelog");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        /// �۹��� 20160721 �޸�ʱ��Ϊ���ݿ��Զ���ȡ
        public int Add(BCW.farm.Model.NC_messagelog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_messagelog(");
            strSql.Append("UsId,UsName,AcText,type)");
            strSql.Append(" values (");
            strSql.Append("@UsId,@UsName,@AcText,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsId", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.VarChar,50),
                    new SqlParameter("@AcText", SqlDbType.NVarChar),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.UsId;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.AcText;
            parameters[3].Value = model.type;

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
        /// ����һ������
        /// </summary>
        public void Update(BCW.farm.Model.NC_messagelog model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_messagelog set ");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AcText=@AcText,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsId", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.VarChar,50),
                    new SqlParameter("@AcText", SqlDbType.NVarChar),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.AcText;
            parameters[4].Value = model.AddTime;
            parameters[5].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_messagelog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_messagelog GetNC_messagelog(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsId,UsName,AcText,AddTime,type from tb_NC_messagelog ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_messagelog model = new BCW.farm.Model.NC_messagelog();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsId = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.AcText = reader.GetString(3);
                    model.AddTime = reader.GetDateTime(4);
                    model.type = reader.GetInt32(5);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_messagelog ");
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
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_messagelog</returns>
        public IList<BCW.farm.Model.NC_messagelog> GetNC_messagelogs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_messagelog> listNC_messagelogs = new List<BCW.farm.Model.NC_messagelog>();
            string sTable = "tb_NC_messagelog";
            string sPkey = "id";
            string sField = "*";
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
                    return listNC_messagelogs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_messagelog objNC_messagelog = new BCW.farm.Model.NC_messagelog();
                    objNC_messagelog.ID = reader.GetInt32(0);
                    objNC_messagelog.UsId = reader.GetInt32(1);
                    objNC_messagelog.UsName = reader.GetString(2);
                    objNC_messagelog.AcText = reader.GetString(3);
                    objNC_messagelog.AddTime = reader.GetDateTime(4);
                    objNC_messagelog.type = reader.GetInt32(5);
                    listNC_messagelogs.Add(objNC_messagelog);
                }
            }
            return listNC_messagelogs;
        }

        #endregion  ��Ա����
    }
}

