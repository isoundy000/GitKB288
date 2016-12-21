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
    /// ���ݷ�����Panel��
    /// </summary>
    public class Panel
    {
        public Panel()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Panel");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Panel");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Panel");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Panel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Panel(");
            strSql.Append("Title,PUrl,UsId,IsBr,Paixu)");
            strSql.Append(" values (");
            strSql.Append("@Title,@PUrl,@UsId,@IsBr,@Paixu)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.IsBr;
            parameters[4].Value = model.Paixu;

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
        /// ���º�������
        /// </summary>
        public void UpdateIsBr(int UsID, int IsBr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("IsBr=@IsBr ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = IsBr;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("Paixu=@Paixu ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Panel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("Title=@Title,");
            strSql.Append("PUrl=@PUrl,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("IsBr=@IsBr,");
            strSql.Append("Paixu=@Paixu");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.PUrl;
            parameters[3].Value = model.UsId;
            parameters[4].Value = model.IsBr;
            parameters[5].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Panel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ��������ɾ������
        /// </summary>
        public void Delete(int UsID, string Title, string PUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Panel");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Title=@Title ");
            strSql.Append(" and PUrl=@PUrl ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50)};
            parameters[0].Value = UsID;
            parameters[1].Value = Title;
            parameters[2].Value = PUrl;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Panel GetPanel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,PUrl,UsId,IsBr,Paixu from tb_Panel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Panel model = new BCW.Model.Panel();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.PUrl = reader.GetString(2);
                    model.UsId = reader.GetInt32(3);
                    model.IsBr = reader.GetInt32(4);
                    model.Paixu = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_Panel ");
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
        /// <returns>IList Panel</returns>
        public IList<BCW.Model.Panel> GetPanels(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Panel> listPanels = new List<BCW.Model.Panel>();
            string sTable = "tb_Panel";
            string sPkey = "id";
            string sField = "ID,Title,PUrl,UsId,IsBr,Paixu";
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
                    return listPanels;
                }
                while (reader.Read())
                {
                    BCW.Model.Panel objPanel = new BCW.Model.Panel();
                    objPanel.ID = reader.GetInt32(0);
                    objPanel.Title = reader.GetString(1);
                    objPanel.PUrl = reader.GetString(2);
                    objPanel.UsId = reader.GetInt32(3);
                    objPanel.IsBr = reader.GetInt32(4);
                    objPanel.Paixu = reader.GetInt32(5);
                    listPanels.Add(objPanel);
                }
            }
            return listPanels;
        }

        #endregion  ��Ա����
    }
}

