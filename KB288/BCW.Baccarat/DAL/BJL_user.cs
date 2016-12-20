using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Baccarat.DAL
{
    /// <summary>
    /// ���ݷ�����BJL_user��
    /// </summary>
    public class BJL_user
    {
        public BJL_user()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_user");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_user");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_user(");
            strSql.Append("usid,setshow,kainum)");
            strSql.Append(" values (");
            strSql.Append("@usid,@setshow,@kainum)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@setshow", SqlDbType.Int,4),
                    new SqlParameter("@kainum", SqlDbType.Int,4)};
            parameters[0].Value = model.usid;
            parameters[1].Value = model.setshow;
            parameters[2].Value = model.kainum;

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
        public void Update(BCW.Baccarat.Model.BJL_user model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_user set ");
            strSql.Append("usid=@usid,");
            strSql.Append("setshow=@setshow,");
            strSql.Append("kainum=@kainum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@setshow", SqlDbType.Int,4),
                    new SqlParameter("@kainum", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.usid;
            parameters[2].Value = model.setshow;
            parameters[3].Value = model.kainum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_user ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_�õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_user GetBJL_user(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_user ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_user model = new BCW.Baccarat.Model.BJL_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.setshow = reader.GetInt32(2);
                    model.kainum = reader.GetInt32(3);
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
            strSql.Append(" FROM tb_BJL_user ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists_user(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_user");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_�����ֶ��޸������б�
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BJL_user SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Baccarat.Model.BJL_user GetBJL_setshow(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_user ");
            strSql.Append(" where usid=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_user model = new BCW.Baccarat.Model.BJL_user();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.usid = reader.GetInt32(1);
                    model.setshow = reader.GetInt32(2);
                    model.kainum = reader.GetInt32(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BJL_user</returns>
        public IList<BCW.Baccarat.Model.BJL_user> GetBJL_users(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_user> listBJL_users = new List<BCW.Baccarat.Model.BJL_user>();
            string sTable = "tb_BJL_user";
            string sPkey = "id";
            string sField = "ID,usid,setshow,kainum";
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
                    return listBJL_users;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_user objBJL_user = new BCW.Baccarat.Model.BJL_user();
                    objBJL_user.ID = reader.GetInt32(0);
                    objBJL_user.usid = reader.GetInt32(1);
                    objBJL_user.setshow = reader.GetInt32(2);
                    objBJL_user.kainum = reader.GetInt32(3);
                    listBJL_users.Add(objBJL_user);
                }
            }
            return listBJL_users;
        }

        #endregion  ��Ա����
    }
}

