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
    /// ���ݷ�����Speak��
    /// </summary>
    public class Speak
    {
        public Speak()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Speak");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Speak");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������������
        /// </summary>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Speak");
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
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetCount(int UsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Speak");
            strSql.Append(" where UsId=@UsId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsId", SqlDbType.Int,4)};
            parameters[0].Value = UsId;
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
        /// ����ĳʱ��ε����ķ�����
        /// </summary>
        public int GetCount(DateTime dt)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Speak");
            strSql.Append(" where AddTime>@AddTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = dt;
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
        /// ���������������ķ�����
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Speak");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        public int Add(BCW.Model.Speak model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Speak(");
            strSql.Append("Types,NodeId,UsId,UsName,ToId,ToName,Notes,AddTime,IsKiss)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsId,@UsName,@ToId,@ToName,@Notes,@AddTime,@IsKiss)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToId", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsKiss", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.ToId;
            parameters[5].Value = model.ToName;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.IsKiss;

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
        public void Update(BCW.Model.Speak model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Speak set ");
            strSql.Append("Types=@Types,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.UsId;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Notes;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Speak set ");
            strSql.Append("IsTop=@IsTop");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = IsTop;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Speak ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Speak ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���һ������
        /// </summary>
        public void Clear(int Types, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Speak ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Speak ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Speak GetSpeak(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,UsId,UsName,Notes,AddTime from tb_Speak ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Speak model = new BCW.Model.Speak();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.UsId = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Notes = reader.GetString(5);
                    model.AddTime = reader.GetDateTime(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ��IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsTop from tb_Speak ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetByte(0);
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// �õ���Ա��һ���������ݺ�ʱ��
        /// </summary>
        public BCW.Model.Speak GetNotesAddTime(int UsId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Notes,AddTime from tb_Speak ");
            strSql.Append(" where UsId=@UsId Order BY ID DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@UsId", SqlDbType.Int,4)};
            parameters[0].Value = UsId;

            BCW.Model.Speak model = new BCW.Model.Speak();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        model.Notes = reader.GetString(0);
                    else
                        model.Notes = "";
                    if (!reader.IsDBNull(1))
                        model.AddTime = reader.GetDateTime(1);
                    else
                        model.AddTime = DateTime.Now;

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
            strSql.Append(" FROM tb_Speak ");
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
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Speak> listSpeaks = new List<BCW.Model.Speak>();
            string sTable = "tb_Speak";
            string sPkey = "id";
            string sField = "ID,Types,UsId,UsName,Notes,AddTime,ToId,ToName,IsKiss,IsTop";
            string sCondition = strWhere;
            string sOrder = "IsTop Desc,ID Desc";
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
                    return listSpeaks;
                }
                while (reader.Read())
                {
                    BCW.Model.Speak objSpeak = new BCW.Model.Speak();
                    objSpeak.ID = reader.GetInt32(0);
                    objSpeak.Types = reader.GetInt32(1);
                    objSpeak.UsId = reader.GetInt32(2);
                    objSpeak.UsName = reader.GetString(3);
                    objSpeak.Notes = reader.GetString(4);
                    objSpeak.AddTime = reader.GetDateTime(5);
                    objSpeak.ToId = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        objSpeak.ToName = reader.GetString(7);
                    objSpeak.IsKiss = reader.GetByte(8);
                    objSpeak.IsTop = reader.GetByte(9);
                    listSpeaks.Add(objSpeak);
                }
            }
            return listSpeaks;
        }

        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Speak> listSpeaks = new List<BCW.Model.Speak>();
            string sTable = "tb_Speak";
            string sPkey = "id";
            string sField = "ID,Types,UsId,UsName,Notes,AddTime,ToId,ToName,IsKiss,IsTop";
            string sCondition = strWhere;
            string sOrder = "IsTop Desc,ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listSpeaks;
                }
                while (reader.Read())
                {
                    BCW.Model.Speak objSpeak = new BCW.Model.Speak();
                    objSpeak.ID = reader.GetInt32(0);
                    objSpeak.Types = reader.GetInt32(1);
                    objSpeak.UsId = reader.GetInt32(2);
                    objSpeak.UsName = reader.GetString(3);
                    objSpeak.Notes = reader.GetString(4);
                    objSpeak.AddTime = reader.GetDateTime(5);
                    objSpeak.ToId = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        objSpeak.ToName = reader.GetString(7);
                    objSpeak.IsKiss = reader.GetByte(8);
                    objSpeak.IsTop = reader.GetByte(9);
                    listSpeaks.Add(objSpeak);
                }
            }
            return listSpeaks;
        }

        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Speak> listSpeak = new List<BCW.Model.Speak>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT Types) FROM tb_Speak Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listSpeak;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT Types, COUNT(Types) FROM tb_Speak Where " + strWhere + " GROUP BY Types ORDER BY COUNT(Types) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Speak objSpeak = new BCW.Model.Speak();
                        objSpeak.ID = reader.GetInt32(0);
                        objSpeak.Types = reader.GetInt32(1);
                        listSpeak.Add(objSpeak);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listSpeak;
        }

        /// <summary>
        /// �������а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Speak> listSpeak = new List<BCW.Model.Speak>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Speak Where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 100)
                    pageCount = 100;
            }
            else
            {
                return listSpeak;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT UsID, COUNT(ID) FROM tb_Speak Where " + strWhere + " GROUP BY UsID ORDER BY COUNT(ID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Speak objSpeak = new BCW.Model.Speak();
                        objSpeak.UsId = reader.GetInt32(0);
                        objSpeak.Types = reader.GetInt32(1);
                        listSpeak.Add(objSpeak);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listSpeak;
        }

        #endregion  ��Ա����
    }
}

