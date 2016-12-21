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
    /// ���ݷ�����NC_daoju��
    /// </summary>
    public class NC_daoju
    {
        public NC_daoju()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_daoju");
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.farm.Model.NC_daoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_daoju(");
            strSql.Append("name,price,note,picture,time,type)");
            strSql.Append(" values (");
            strSql.Append("@name,@price,@note,@picture,@time,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@note", SqlDbType.NVarChar,200),
                    new SqlParameter("@picture", SqlDbType.VarChar,20),
                    new SqlParameter("@time", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.name;
            parameters[1].Value = model.price;
            parameters[2].Value = model.note;
            parameters[3].Value = model.picture;
            parameters[4].Value = model.time;
            parameters[5].Value = model.type;

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
        public void Update(BCW.farm.Model.NC_daoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_daoju set ");
            strSql.Append("name=@name,");
            strSql.Append("price=@price,");
            strSql.Append("note=@note,");
            strSql.Append("picture=@picture,");
            strSql.Append("time=@time,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@note", SqlDbType.NVarChar,200),
                    new SqlParameter("@picture", SqlDbType.VarChar,20),
                    new SqlParameter("@time", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.name;
            parameters[2].Value = model.price;
            parameters[3].Value = model.note;
            parameters[4].Value = model.picture;
            parameters[5].Value = model.time;
            parameters[6].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_daoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.farm.Model.NC_daoju GetNC_daoju(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name,price,note,picture,time,type from tb_NC_daoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_daoju model = new BCW.farm.Model.NC_daoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.price = reader.GetInt64(2);
                    model.note = reader.GetString(3);
                    model.picture = reader.GetString(4);
                    model.time = reader.GetInt32(5);
                    model.type = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_NC_daoju ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //======================================
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where ID=@ID and type!=10 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// //��̨�����жϸ�id�Ƿ����
        /// </summary>
        public bool Exists2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�Ƿ���ڸõ�������
        /// </summary>
        public bool Exists_djmc(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_daoju");
            strSql.Append(" where name=@name ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20)};
            parameters[0].Value = name;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_daoju(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_daoju SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_�õ�����ͼƬ·��
        /// </summary>
        public string Get_picture(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select picture from tb_NC_daoju ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        //======================================


        /// <summary>
        /// me_ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList NC_daoju</returns>
        public IList<BCW.farm.Model.NC_daoju> GetNC_daojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_daoju> listNC_daojus = new List<BCW.farm.Model.NC_daoju>();
            string sTable = "tb_NC_daoju";
            string sPkey = "id";
            string sField = "ID,name,price,note,picture,time,type";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listNC_daojus;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_daoju objNC_daoju = new BCW.farm.Model.NC_daoju();
                    objNC_daoju.ID = reader.GetInt32(0);
                    objNC_daoju.name = reader.GetString(1);
                    objNC_daoju.price = reader.GetInt64(2);
                    objNC_daoju.note = reader.GetString(3);
                    objNC_daoju.picture = reader.GetString(4);
                    objNC_daoju.time = reader.GetInt32(5);
                    objNC_daoju.type = reader.GetInt32(6);
                    listNC_daojus.Add(objNC_daoju);
                }
            }
            return listNC_daojus;
        }

        #endregion  ��Ա����
    }
}

