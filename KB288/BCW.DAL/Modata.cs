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
    /// ���ݷ�����Modata��
    /// </summary>
    public class Modata
    {
        public Modata()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Modata");
        }

        /// <summary>
        /// �õ����Types
        /// </summary>
        public int GetMaxTypes()
        {
            return SqlHelper.GetMaxID("Types", "tb_Modata");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Modata");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Modata");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ���¼��
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Modata ");

            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        public void Add(BCW.Model.Modata model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Modata(");
            strSql.Append("Types,PhoneBrand,PhoneModel,PhoneSystem,PhoneSize,PhoneClick)");
            strSql.Append(" values (");
            strSql.Append("@Types,@PhoneBrand,@PhoneModel,@PhoneSystem,@PhoneSize,@PhoneClick)");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PhoneBrand", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneModel", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneSystem", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneSize", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneClick", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.PhoneBrand;
            parameters[2].Value = model.PhoneModel;
            parameters[3].Value = model.PhoneSystem;
            parameters[4].Value = model.PhoneSize;
            parameters[5].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.Modata model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Modata set ");
            strSql.Append("Types=@Types,");
            strSql.Append("PhoneBrand=@PhoneBrand,");
            strSql.Append("PhoneModel=@PhoneModel,");
            strSql.Append("PhoneSystem=@PhoneSystem,");
            strSql.Append("PhoneSize=@PhoneSize,");
            strSql.Append("PhoneClick=@PhoneClick");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@PhoneBrand", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneModel", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneSystem", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneSize", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneClick", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.PhoneBrand;
            parameters[3].Value = model.PhoneModel;
            parameters[4].Value = model.PhoneSystem;
            parameters[5].Value = model.PhoneSize;
            parameters[6].Value = model.PhoneClick;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ѡ������
        /// </summary>
        public void UpdatePhoneClick(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Modata set ");
            strSql.Append("PhoneClick=PhoneClick+@PhoneClick");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PhoneClick", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Modata ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Modata GetModata(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,PhoneBrand,PhoneModel,PhoneSystem,PhoneSize,PhoneClick from tb_Modata ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Modata model = new BCW.Model.Modata();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.PhoneBrand = reader.GetString(2);
                    model.PhoneModel = reader.GetString(3);
                    model.PhoneSystem = reader.GetString(4);
                    model.PhoneSize = reader.GetString(5);
                    model.PhoneClick = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Modata GetModata2(string Model)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,PhoneBrand,PhoneModel,PhoneSystem,PhoneSize,PhoneClick from tb_Modata ");
            strSql.Append(" where PhoneModel=@PhoneModel ");
            SqlParameter[] parameters = {
					new SqlParameter("@PhoneModel", SqlDbType.NVarChar,50)};
            parameters[0].Value = Model;

            BCW.Model.Modata model = new BCW.Model.Modata();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.PhoneBrand = reader.GetString(2);
                    model.PhoneModel = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.PhoneSystem = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.PhoneSize = reader.GetString(5);
                    model.PhoneClick = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ��Types
        /// </summary>
        public int GetTypes(string Brand)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Types from tb_Modata ");
            strSql.Append(" where PhoneBrand=@PhoneBrand ");
            SqlParameter[] parameters = {
					new SqlParameter("@PhoneBrand", SqlDbType.NVarChar,50)};
            parameters[0].Value = Brand;

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
        /// ����Types�õ�һ��Brand
        /// </summary>
        public string GetPhoneBrand(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PhoneBrand from tb_Modata ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

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
        /// ����Types�õ�һ��Model
        /// </summary>
        public string GetPhoneModel(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 PhoneModel from tb_Modata ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

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
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Modata ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ��Ʒ�Ƽ�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Modata> GetBrand(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Modata> listModatas = new List<BCW.Model.Modata>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT PhoneBrand) FROM tb_Modata where " + strWhere + "";
            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listModatas;
            }
            // SQL
            string queryString = "SELECT PhoneBrand, COUNT(*)  FROM tb_Modata where " + strWhere + " GROUP BY PhoneBrand ORDER BY COUNT(*) DESC";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Modata objModata = new BCW.Model.Modata();
                        if (!reader.IsDBNull(0))
                            objModata.PhoneBrand = reader.GetString(0);
                        else
                            objModata.PhoneBrand = "δ֪";

                        listModatas.Add(objModata);
                    }

                    if (k == endIndex)
                        break;
                    k++;
                }
            }
            return listModatas;
        }


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Modata</returns>
        public IList<BCW.Model.Modata> GetModatas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Modata> listModatas = new List<BCW.Model.Modata>();
            string sTable = "tb_Modata";
            string sPkey = "id";
            string sField = "ID,PhoneBrand,PhoneModel";
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
                    return listModatas;
                }
                while (reader.Read())
                {
                    BCW.Model.Modata objModata = new BCW.Model.Modata();
                    objModata.ID = reader.GetInt32(0);
                    if (!reader.IsDBNull(1))
                        objModata.PhoneBrand = reader.GetString(1);
                    objModata.PhoneModel = reader.GetString(2);
                    listModatas.Add(objModata);
                }
            }
            return listModatas;
        }

        #endregion  ��Ա����
    }
}

