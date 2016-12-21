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
    /// ���ݷ�����Visitor��
    /// </summary>
    public class Visitor
    {
        public Visitor()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Visitor");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Visitor");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsVisitId(int UsID, int VisitId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Visitor");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and VisitId=@VisitId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = VisitId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����������
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Visitor");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " and Month(AddTime)=" + DateTime.Now.Month + " and Day(AddTime)=" + DateTime.Now.Day + "");
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
        public int Add(BCW.Model.Visitor model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Visitor(");
            strSql.Append("UsID,UsName,VisitId,VisitName,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@VisitId,@VisitName,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@VisitId", SqlDbType.Int,4),
					new SqlParameter("@VisitName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.VisitId;
            parameters[3].Value = model.VisitName;
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
        public void Update(BCW.Model.Visitor model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Visitor set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("VisitId=@VisitId,");
            strSql.Append("VisitName=@VisitName,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID");
            strSql.Append(" and VisitId=@VisitId");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@VisitId", SqlDbType.Int,4),
					new SqlParameter("@VisitName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.VisitId;
            parameters[3].Value = model.VisitName;
            parameters[4].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(int UsID, int VisitId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Visitor set ");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID");
            strSql.Append(" and VisitId=@VisitId");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@VisitId", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = UsID;
            parameters[1].Value = VisitId;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Visitor ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Visitor GetVisitor(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,VisitId,VisitName,AddTime from tb_Visitor ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Visitor model = new BCW.Model.Visitor();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.VisitId = reader.GetInt32(3);
                    model.VisitName = reader.GetString(4);
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
            strSql.Append(" FROM tb_Visitor ");
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
        /// <returns>IList Visitor</returns>
        public IList<BCW.Model.Visitor> GetVisitors(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Visitor> listVisitors = new List<BCW.Model.Visitor>();
            string sTable = "tb_Visitor";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,VisitId,VisitName,AddTime";
            string sCondition = strWhere;
            string sOrder = "AddTime Desc";
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
                    return listVisitors;
                }
                while (reader.Read())
                {
                    BCW.Model.Visitor objVisitor = new BCW.Model.Visitor();
                    objVisitor.ID = reader.GetInt32(0);
                    objVisitor.UsID = reader.GetInt32(1);
                    objVisitor.UsName = reader.GetString(2);
                    objVisitor.VisitId = reader.GetInt32(3);
                    objVisitor.VisitName = reader.GetString(4);
                    objVisitor.AddTime = reader.GetDateTime(5);
                    listVisitors.Add(objVisitor);
                }
            }
            return listVisitors;
        }

        #endregion  ��Ա����
    }
}
