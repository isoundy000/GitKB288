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
    /// ���ݷ�����Medalget��
    /// </summary>
    public class Medalget
    {
        public Medalget()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("UsID", "tb_Medalget");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Medalget");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ĳ�û�����
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Medalget");
            strSql.Append(" where UsID=@UsID and Types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Medalget model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Medalget(");
            strSql.Append("Types,UsID,MedalId,Notes,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@MedalId,@Notes,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@MedalId", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.MedalId;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.AddTime;

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
        public void Update(BCW.Model.Medalget model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Medalget set ");
            strSql.Append("Types=@Types,");
            strSql.Append("MedalId=@MedalId,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@MedalId", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.MedalId;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Medalget ");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Medalget GetMedalget(int UsID, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,MedalId,Notes,AddTime from tb_Medalget ");
            strSql.Append(" where UsID=@UsID and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ID;

            BCW.Model.Medalget model = new BCW.Model.Medalget();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.MedalId = reader.GetInt32(3);
                    model.Notes = reader.GetString(4);
                    model.AddTime = reader.GetDateTime(5);
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
            strSql.Append(" FROM tb_Medalget ");
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
        /// <returns>IList Medalget</returns>
        public IList<BCW.Model.Medalget> GetMedalgets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Medalget> listMedalgets = new List<BCW.Model.Medalget>();
            string sTable = "tb_Medalget";
            string sPkey = "id";
            string sField = "ID,Types,UsID,MedalId,Notes,AddTime";
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
                    return listMedalgets;
                }
                while (reader.Read())
                {
                    BCW.Model.Medalget objMedalget = new BCW.Model.Medalget();
                    objMedalget.ID = reader.GetInt32(0);
                    objMedalget.Types = reader.GetInt32(1);
                    objMedalget.UsID = reader.GetInt32(2);
                    objMedalget.MedalId = reader.GetInt32(3);
                    objMedalget.Notes = reader.GetString(4);
                    objMedalget.AddTime = reader.GetDateTime(5);
                    listMedalgets.Add(objMedalget);
                }
            }
            return listMedalgets;
        }

        /// <summary>
        /// ����ѫ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Medalget> GetMedalgetsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Medalget> listMedalget = new List<BCW.Model.Medalget>();

            string strWhe = string.Empty;
            if (strWhere != "")
                strWhe = " where " + strWhere
;
            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Medalget " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 100)
                    pageCount = 100;
            }
            else
            {
                return listMedalget;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT UsID, COUNT(ID) FROM tb_Medalget " + strWhe + " GROUP BY UsID ORDER BY COUNT(ID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Medalget objMedalget = new BCW.Model.Medalget();
                        objMedalget.UsID = reader.GetInt32(0);
                        objMedalget.Types = reader.GetInt32(1);
                        listMedalget.Add(objMedalget);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listMedalget;
        }


        #endregion  ��Ա����
    }
}

