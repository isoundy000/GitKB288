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
    /// ���ݷ�����NC_gonggao��
    /// </summary>
    public class NC_gonggao
    {
        public NC_gonggao()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_gonggao");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_gonggao");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_gonggao model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_gonggao(");
            strSql.Append("title,contact,updatetime,type)");
            strSql.Append(" values (");
            strSql.Append("@title,@contact,@updatetime,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@title", SqlDbType.VarChar,50),
                    new SqlParameter("@contact", SqlDbType.VarChar,300),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.title;
            parameters[1].Value = model.contact;
            parameters[2].Value = model.updatetime;
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
        public void Update(BCW.farm.Model.NC_gonggao model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_gonggao set ");
            strSql.Append("title=@title,");
            strSql.Append("contact=@contact,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@title", SqlDbType.VarChar,50),
                    new SqlParameter("@contact", SqlDbType.VarChar,300),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.title;
            parameters[2].Value = model.contact;
            parameters[3].Value = model.updatetime;
            parameters[4].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_gonggao ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_gonggao GetNC_gonggao(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,title,contact,updatetime,type from tb_NC_gonggao ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_gonggao model = new BCW.farm.Model.NC_gonggao();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.title = reader.GetString(1);
                    model.contact = reader.GetString(2);
                    model.updatetime = reader.GetDateTime(3);
                    model.type = reader.GetInt32(4);
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
            strSql.Append(" FROM tb_NC_gonggao ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //============================
        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_gonggao(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_gonggao SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //========================


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_gonggao</returns>
        public IList<BCW.farm.Model.NC_gonggao> GetNC_gonggaos(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_gonggao> listNC_gonggaos = new List<BCW.farm.Model.NC_gonggao>();
            string sTable = "tb_NC_gonggao";
            string sPkey = "id";
            string sField = "ID,title,contact,updatetime,type";
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
                    return listNC_gonggaos;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_gonggao objNC_gonggao = new BCW.farm.Model.NC_gonggao();
                    objNC_gonggao.ID = reader.GetInt32(0);
                    objNC_gonggao.title = reader.GetString(1);
                    objNC_gonggao.contact = reader.GetString(2);
                    objNC_gonggao.updatetime = reader.GetDateTime(3);
                    objNC_gonggao.type = reader.GetInt32(4);
                    listNC_gonggaos.Add(objNC_gonggao);
                }
            }
            return listNC_gonggaos;
        }

        #endregion  ��Ա����
    }
}

