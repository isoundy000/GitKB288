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
    /// ���ݷ�����NC_slavelist��
    /// </summary>
    public class NC_slavelist
    {
        public NC_slavelist()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_slavelist");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_slavelist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_slavelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_slavelist(");
            strSql.Append("contact,inGold,outGold,type)");
            strSql.Append(" values (");
            strSql.Append("@contact,@inGold,@outGold,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@contact", SqlDbType.NVarChar,100),
                    new SqlParameter("@inGold", SqlDbType.Int,4),
                    new SqlParameter("@outGold", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.contact;
            parameters[1].Value = model.inGold;
            parameters[2].Value = model.outGold;
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
        public void Update(BCW.farm.Model.NC_slavelist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_slavelist set ");
            strSql.Append("contact=@contact,");
            strSql.Append("inGold=@inGold,");
            strSql.Append("outGold=@outGold,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@contact", SqlDbType.NVarChar,100),
                    new SqlParameter("@inGold", SqlDbType.Int,4),
                    new SqlParameter("@outGold", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.contact;
            parameters[2].Value = model.inGold;
            parameters[3].Value = model.outGold;
            parameters[4].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_slavelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_slavelist GetNC_slavelist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,contact,inGold,outGold,type from tb_NC_slavelist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_slavelist model = new BCW.farm.Model.NC_slavelist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.contact = reader.GetString(1);
                    model.inGold = reader.GetInt32(2);
                    model.outGold = reader.GetInt32(3);
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
            strSql.Append(" FROM tb_NC_slavelist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_yuju(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_slavelist SET ");
            strSql.Append(strField);
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
        /// <returns>IList NC_slavelist</returns>
        public IList<BCW.farm.Model.NC_slavelist> GetNC_slavelists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_slavelist> listNC_slavelists = new List<BCW.farm.Model.NC_slavelist>();
            string sTable = "tb_NC_slavelist";
            string sPkey = "id";
            string sField = "ID,contact,inGold,outGold,type";
            string sCondition = strWhere;
            string sOrder = "ID asc";
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
                    return listNC_slavelists;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_slavelist objNC_slavelist = new BCW.farm.Model.NC_slavelist();
                    objNC_slavelist.ID = reader.GetInt32(0);
                    objNC_slavelist.contact = reader.GetString(1);
                    objNC_slavelist.inGold = reader.GetInt32(2);
                    objNC_slavelist.outGold = reader.GetInt32(3);
                    objNC_slavelist.type = reader.GetInt32(4);
                    listNC_slavelists.Add(objNC_slavelist);
                }
            }
            return listNC_slavelists;
        }

        #endregion  ��Ա����
    }
}

