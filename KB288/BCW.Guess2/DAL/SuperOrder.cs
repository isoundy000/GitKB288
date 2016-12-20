using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace TPR2.DAL.guess
{
    /// <summary>
    /// ���ݷ�����SuperOrder��
    /// </summary>
    public class SuperOrder
    {
        public SuperOrder()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_TSuperOrder");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_TSuperOrder");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(TPR2.Model.guess.SuperOrder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_TSuperOrder(");
            strSql.Append("UsID,UsName,Cent)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@Cent)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Cent", SqlDbType.Money,8)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Cent;

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
        public void Update(TPR2.Model.guess.SuperOrder model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_TSuperOrder set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Cent=Cent+@Cent");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Cent", SqlDbType.Money,8)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.Cent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_TSuperOrder ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_TSuperOrder ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public TPR2.Model.guess.SuperOrder GetSuperOrder(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,Cent from tb_TSuperOrder ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            TPR2.Model.guess.SuperOrder model = new TPR2.Model.guess.SuperOrder();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.Cent = reader.GetDecimal(3);
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
            strSql.Append(" FROM tb_TSuperOrder ");
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
        /// <returns>IList SuperOrder</returns>
        public IList<TPR2.Model.guess.SuperOrder> GetSuperOrders(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<TPR2.Model.guess.SuperOrder> listSuperOrders = new List<TPR2.Model.guess.SuperOrder>();
            string sTable = "tb_TSuperOrder";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,Cent";
            string sCondition = strWhere;
            string sOrder = "Cent Desc";
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
                    return listSuperOrders;
                }
                while (reader.Read())
                {
                    TPR2.Model.guess.SuperOrder objSuperOrder = new TPR2.Model.guess.SuperOrder();
                    objSuperOrder.ID = reader.GetInt32(0);
                    objSuperOrder.UsID = reader.GetInt32(1);
                    objSuperOrder.UsName = reader.GetString(2);
                    objSuperOrder.Cent = reader.GetDecimal(3);
                    listSuperOrders.Add(objSuperOrder);
                }
            }
            return listSuperOrders;
        }

        #endregion  ��Ա����
    }
}

