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
    /// ���ݷ�����NC_baoxiang��
    /// </summary>
    public class NC_baoxiang
    {
        public NC_baoxiang()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_baoxiang");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_baoxiang");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_baoxiang model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_baoxiang(");
            strSql.Append("prize,picture,daoju_id,type)");
            strSql.Append(" values (");
            strSql.Append("@prize,@picture,@daoju_id,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@prize", SqlDbType.VarChar,50),
                    new SqlParameter("@picture", SqlDbType.VarChar,60),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.prize;
            parameters[1].Value = model.picture;
            parameters[2].Value = model.daoju_id;
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
        public void Update(BCW.farm.Model.NC_baoxiang model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_baoxiang set ");
            strSql.Append("prize=@prize,");
            strSql.Append("picture=@picture,");
            strSql.Append("daoju_id=@daoju_id,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@prize", SqlDbType.VarChar,50),
                    new SqlParameter("@picture", SqlDbType.VarChar,60),
                    new SqlParameter("@daoju_id", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.prize;
            parameters[2].Value = model.picture;
            parameters[3].Value = model.daoju_id;
            parameters[4].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_baoxiang ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_baoxiang GetNC_baoxiang(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,prize,picture,daoju_id,type from tb_NC_baoxiang ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.prize = reader.GetString(1);
                    model.picture = reader.GetString(2);
                    model.daoju_id = reader.GetInt32(3);
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
            strSql.Append(" FROM tb_NC_baoxiang ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //========================================
        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_baoxiang DataRowToModel(DataRow row)
        {
            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["prize"] != null)
                {
                    model.prize = row["prize"].ToString();
                }
                if (row["picture"] != null)
                {
                    model.picture = row["picture"].ToString();
                }
                if (row["daoju_id"] != null && row["daoju_id"].ToString() != "")
                {
                    model.daoju_id = int.Parse(row["daoju_id"].ToString());
                }
                if (row["type"] != null && row["type"].ToString() != "")
                {
                    model.type = int.Parse(row["type"].ToString());
                }
            }
            return model;
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
        /// <summary>
        /// me_��ѯ�м�������
        /// </summary>
        public BCW.farm.Model.NC_baoxiang Get_num(int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  COUNT(*) as aa from tb_NC_baoxiang");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = type;

            BCW.farm.Model.NC_baoxiang model = new BCW.farm.Model.NC_baoxiang();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        ///me_�ж��Ƿ��������id
        /// </summary>
        public bool Exists_bxzzdj(int ID, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_baoxiang");
            strSql.Append(" where daoju_id=@ID and type=@type");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = type;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //========================================

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_baoxiang</returns>
        public IList<BCW.farm.Model.NC_baoxiang> GetNC_baoxiangs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_baoxiang> listNC_baoxiangs = new List<BCW.farm.Model.NC_baoxiang>();
            string sTable = "tb_NC_baoxiang";
            string sPkey = "id";
            string sField = "ID,prize,picture,daoju_id,type";
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
                    return listNC_baoxiangs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_baoxiang objNC_baoxiang = new BCW.farm.Model.NC_baoxiang();
                    objNC_baoxiang.ID = reader.GetInt32(0);
                    objNC_baoxiang.prize = reader.GetString(1);
                    objNC_baoxiang.picture = reader.GetString(2);
                    objNC_baoxiang.daoju_id = reader.GetInt32(3);
                    objNC_baoxiang.type = reader.GetInt32(4);
                    listNC_baoxiangs.Add(objNC_baoxiang);
                }
            }
            return listNC_baoxiangs;
        }

        #endregion  ��Ա����
    }
}

