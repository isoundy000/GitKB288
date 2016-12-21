using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.ssc.DAL
{
    /// <summary>
    /// ���ݷ�����SSClist��
    /// </summary>
    public class SSClist
    {
        public SSClist()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_SSClist");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSClist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸ���
        /// </summary>
        public bool ExistsSSCId(int SSCId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSClist");
            strSql.Append(" where SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = SSCId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸ�����δ����
        /// </summary>
        public bool ExistsSSCId(int SSCId,int k)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSClist");
            strSql.Append(" where SSCId=@SSCId and State=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = SSCId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistbySSCId(int SSCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SSCId,Result,Notes,State,EndTime,StateTime from tb_SSClist ");
            strSql.Append(" where SSCId=@SSCId");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = SSCId;

            BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {   try
                    {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SSCId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    model.Notes = reader.GetString(3);
                    model.State = reader.GetInt32(4);
                    model.EndTime = reader.GetDateTime(5);
                    model.StateTime = reader.GetString(6);
                  }
                    catch {  }
                        return model;
                   
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// �����ںŵõ�id
        /// </summary>
        public int id(int SSCId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID  from tb_SSClist where SSCId=" + SSCId);
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �����ںŵõ��������ͺͿ���ʱ��
        /// </summary>
        public string GetStateTime(int SSCId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StateTime from tb_SSClist where SSCId=" + SSCId);
            SqlParameter[] parameters = {
                    new SqlParameter("@StateTime", SqlDbType.NVarChar,4000)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return " ";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }

        /// <summary>
        /// �Ƿ����Ҫ���½���ļ�¼
        /// </summary>
        public bool ExistsUpdateResult()
        {
            int Sec = Utils.ParseInt(ub.GetSub("SSCSec", "/Controls/ssc.xml"));

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSClist");
            strSql.Append(" where State=0 and EndTime<'" + DateTime.Now.AddSeconds(Sec) + "'");

            return SqlHelper.Exists(strSql.ToString());
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.ssc.Model.SSClist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SSClist(");
            strSql.Append("SSCId,Result,Notes,State,EndTime,StateTime)");
            strSql.Append(" values (");
            strSql.Append("@SSCId,@Result,@Notes,@State,@EndTime,@StateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@StateTime",SqlDbType.NVarChar,4000)};
            parameters[0].Value = model.SSCId;
            parameters[1].Value = model.Result;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.State;
            parameters[4].Value = model.EndTime;
            parameters[5].Value = model.StateTime;

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
        public void Update(BCW.ssc.Model.SSClist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSClist set ");
            strSql.Append("SSCId=@SSCId,");
            strSql.Append("Result=@Result,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("StateTime=@StateTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                                       new SqlParameter("@StateTime", SqlDbType.NVarChar,4000) };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.SSCId;
            parameters[2].Value = model.Result;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.EndTime;
            parameters[5].Value = model.StateTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ���¿������
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSClist set ");
            strSql.Append("Result=@Result,");
            strSql.Append("State=1");
            strSql.Append(" where SSCId=@SSCId and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¿������ͺͿ���ʱ��
        /// </summary>
        public void UpdateStateTime(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSClist set ");
            strSql.Append("StateTime=@StateTime ");
            strSql.Append(" where SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@StateTime", SqlDbType.NVarChar,4000)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¿������
        /// </summary>
        public void UpdateResult1(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSClist set ");
            strSql.Append("Result=@Result,");
            strSql.Append("State=1");
            strSql.Append(" where SSCId=@SSCId and State=1");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SSClist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SSCId,Result,Notes,State,EndTime,StateTime from tb_SSClist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SSCId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    model.Notes = reader.GetString(3);
                    model.State = reader.GetByte(4);
                    model.EndTime = reader.GetDateTime(5);
                    model.StateTime = reader.GetString(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ����һ�ڶ���ʵ��
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SSCId,Result,Notes,State,EndTime,StateTime from tb_SSClist");
            strSql.Append(" Order by id desc ");

            BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SSCId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
                    model.Notes = reader.GetString(3);
                    model.State = reader.GetByte(4);
                    model.EndTime = reader.GetDateTime(5);
                    model.StateTime = reader.GetString(6);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SSCId = 0;
                    model.Result = "";
                    model.Notes = "";
                    model.State = 0;
                    model.EndTime = DateTime.Now;
                    model.StateTime = "";
                    return model;
                }
            }
        }

        /// <summary>
        /// �õ����ڿ���
        /// </summary>
        public BCW.ssc.Model.SSClist GetSSClistLast2()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SSCId,Result from tb_SSClist where State=1");
            strSql.Append(" Order by id desc ");

            BCW.ssc.Model.SSClist model = new BCW.ssc.Model.SSClist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SSCId = reader.GetInt32(1);
                    model.Result = reader.GetString(2);
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
            strSql.Append(" FROM tb_SSClist ");
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
        /// <returns>IList SSClist</returns>
        public IList<BCW.ssc.Model.SSClist> GetSSClists(int SizeNum, string strWhere)
        {
            IList<BCW.ssc.Model.SSClist> listSSClists = new List<BCW.ssc.Model.SSClist>();
            string sTable = "tb_SSClist";
            string sPkey = "id";
            string sField = "ID,SSCId,Result,Notes,State,EndTime,StateTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listSSClists;
                }
                while (reader.Read())
                {
                    BCW.ssc.Model.SSClist objSSClist = new BCW.ssc.Model.SSClist();
                    objSSClist.ID = reader.GetInt32(0);
                    objSSClist.SSCId = reader.GetInt32(1);
                    objSSClist.Result = reader.GetString(2);
                    objSSClist.Notes = reader.GetString(3);
                    objSSClist.State = reader.GetByte(4);
                    objSSClist.EndTime = reader.GetDateTime(5);
                    objSSClist.StateTime = reader.GetString(6);
                    listSSClists.Add(objSSClist);
                }
            }
            return listSSClists;
        }


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSClist</returns>
        public IList<BCW.ssc.Model.SSClist> GetSSClists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.ssc.Model.SSClist> listSSClists = new List<BCW.ssc.Model.SSClist>();
            string sTable = "tb_SSClist";
            string sPkey = "id";
            string sField = "ID,SSCId,Result,Notes,State,EndTime,StateTime";
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
                    return listSSClists;
                }
                while (reader.Read())
                {
                    BCW.ssc.Model.SSClist objSSClist = new BCW.ssc.Model.SSClist();
                    objSSClist.ID = reader.GetInt32(0);
                    objSSClist.SSCId = reader.GetInt32(1);
                    objSSClist.Result = reader.GetString(2);
                    objSSClist.Notes = reader.GetString(3);
                    objSSClist.State = reader.GetByte(4);
                    objSSClist.EndTime = reader.GetDateTime(5);
                    objSSClist.StateTime = reader.GetString(6);
                    listSSClists.Add(objSSClist);
                }
            }
            return listSSClists;
        }

        #endregion  ��Ա����
    }
}

