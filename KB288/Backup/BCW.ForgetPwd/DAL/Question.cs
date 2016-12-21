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
    /// ���ݷ�����tb_Question��
    /// </summary>
    public class tb_Question
    {
        public tb_Question()
        { }
        #region  ��Ա����

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(string account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Question");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@account", SqlDbType.Int)};
            parameters[0].Value = account;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Question");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �����޸�����Ĵ���
        /// </summary>
        public void UpdateChangeCount(int changecount, int account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("changecount=@changecount ");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@changecount", SqlDbType.Int,4),
					new SqlParameter("@account", SqlDbType.Int,4)};
            parameters[0].Value = changecount;
            parameters[1].Value = account;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �������뱣����Ϊ�ֻ�����λ
        /// </summary>
        public void UpdateAnswer(string Answer, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("Answer=@Answer ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Answer", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Answer;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ������֤��
        /// </summary>
        public void UpdateCode(string code, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("code=@code ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@code", SqlDbType.NVarChar,10),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = code;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ������һ���޸����������
        /// </summary>
        public void UpdateLastChange(int lastchange, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("lastchange=@lastchange ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@lastchange", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = lastchange;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �����ֻ���
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("Mobile=@Mobile ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Mobile;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public void Add(BCW.Model.tb_Question model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Question(");
            strSql.Append("Question,Answer,state,Mobile,ID,lastchange,changecount,code)");
            strSql.Append(" values (");
            strSql.Append("@Question,@Answer,@state,@Mobile,@ID,@lastchange,@changecount,@code)");
            SqlParameter[] parameters = {
					new SqlParameter("@Question", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer", SqlDbType.NVarChar,50),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@lastchange", SqlDbType.Int,4),
					new SqlParameter("@changecount", SqlDbType.Int,4),
					new SqlParameter("@code", SqlDbType.NChar,10)};
            parameters[0].Value = model.Question;
            parameters[1].Value = model.Answer;
            parameters[2].Value = model.state;
            parameters[3].Value = model.Mobile;
            parameters[4].Value = model.ID;
            parameters[5].Value = model.lastchange;
            parameters[6].Value = model.changecount;
            parameters[7].Value = model.code;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void Update(BCW.Model.tb_Question model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Question set ");
            strSql.Append("Question=@Question,");
            strSql.Append("Answer=@Answer,");
            strSql.Append("state=@state,");
           // strSql.Append("ID=@ID,");
            strSql.Append("lastchange=@lastchange,");
            strSql.Append("changecount=@changecount,");
            strSql.Append("code=@code");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Question", SqlDbType.NVarChar,50),
					new SqlParameter("@Answer", SqlDbType.NVarChar,50),
					new SqlParameter("@state", SqlDbType.Int,4),
					//new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@lastchange", SqlDbType.Int,4),
					new SqlParameter("@changecount", SqlDbType.Int,4),
					new SqlParameter("@code", SqlDbType.NChar,10)};
            parameters[0].Value = model.Question;
            parameters[1].Value = model.Answer;
            parameters[2].Value = model.state;
            //parameters[3].Value = model.Mobile;
            parameters[3].Value = model.ID;
            parameters[4].Value = model.lastchange;
            parameters[5].Value = model.changecount;
            parameters[6].Value = model.code;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �����˺ŵõ�һ���޸�����Ĵ���
        /// </summary>
        public int GetChangeCount(int account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select changecount from tb_Question ");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@account", SqlDbType.Int,4)};
            parameters[0].Value = account;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// ���ݺ���õ���֤��
        /// </summary>
        public string GetCode(string Mypmobile)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select code from tb_Question ");
            strSql.Append(" where Mobile=@Mypmobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mypmobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mypmobile;

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
        /// <summary>
        /// ���ݺ���õ�����һ���޸��������������һ���е�������
        /// </summary>
        public int GetLastChange(int account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select lastchange from tb_Question ");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@account", SqlDbType.Int,4)};
            parameters[0].Value = account;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// ���ݺ���õ��������
        /// </summary>
        public string GetQuestion(int account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Question from tb_Question ");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@account", SqlDbType.Int,4)};
            parameters[0].Value = account;

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
        /// <summary>
        /// ���ݺ���õ���Ĵ�
        /// </summary>
        public string GetAnswer(int account)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Answer from tb_Question ");
            strSql.Append(" where ID=@account ");
            SqlParameter[] parameters = {
					new SqlParameter("@account", SqlDbType.Int,4)};
            parameters[0].Value = account;

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

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string Mobile)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Question ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_Question Gettb_Question(string Mobile)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Question,Answer,state,Mobile,ID,lastchange,changecount,code from tb_Question ");
            strSql.Append(" where Mobile=@Mobile ");
            SqlParameter[] parameters = {
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50)};
            parameters[0].Value = Mobile;

            BCW.Model.tb_Question model = new BCW.Model.tb_Question();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Question = reader.GetString(0);
                    model.Answer = reader.GetString(1);
                    model.state = reader.GetInt32(2);
                    model.Mobile = reader.GetString(3);
                    model.ID = reader.GetInt32(4);
                    model.lastchange = reader.GetInt32(5);
                    model.changecount = reader.GetInt32(6);
                    model.code = reader.GetString(7);
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
            strSql.Append(" FROM tb_Question ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// ȡ�ù̶��б��¼
        /// </summary>
        /// <param name="SizeNum">�б��¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Brag</returns>
        public IList<BCW.Model.tb_Question> GetBrags(int SizeNum, string strWhere)
        {
            IList<BCW.Model.tb_Question> listQuestion = new List<BCW.Model.tb_Question>();
            string sTable = "tb_Question";
            string sPkey = "id";
            string sField = "Mobile,Question";
            string sCondition = strWhere;
            string sOrder = "Mobile Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listQuestion;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_Question objQuestion = new BCW.Model.tb_Question();
                    objQuestion.ID = reader.GetInt32(0);
                    objQuestion.Question = reader.GetString(1);
                    listQuestion.Add(objQuestion);
                }
            }
            return listQuestion;
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList tb_Question</returns>
        public IList<BCW.Model.tb_Question> Gettb_Questions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_Question> listtb_Questions = new List<BCW.Model.tb_Question>();
            string sTable = "tb_Question";
            string sPkey = "id";
            string sField = "Question,Answer,state,Mobile,ID,lastchange,changecount,code";
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
                    return listtb_Questions;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_Question objtb_Question = new BCW.Model.tb_Question();
                    objtb_Question.Question = reader.GetString(0);
                    objtb_Question.Answer = reader.GetString(1);
                    objtb_Question.state = reader.GetInt32(2);
                    objtb_Question.Mobile = reader.GetString(3);
                    objtb_Question.ID = reader.GetInt32(4);
                    objtb_Question.lastchange = reader.GetInt32(5);
                    objtb_Question.changecount = reader.GetInt32(6);
                    objtb_Question.code = reader.GetString(7);
                    listtb_Questions.Add(objtb_Question);
                }
            }
            return listtb_Questions;
        }

        #endregion  ��Ա����
    }
}

