using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace Book.DAL
{
    /// <summary>
    /// ���ݷ�����Contents��
    /// </summary>
    public class Contents
    {
        public Contents()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperBook.GetMaxID("id", "Contents");
        }

        /// <summary>
        /// �ָ���ɾ��
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contents set ");
            strSql.Append("isdel=@isdel");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@isdel", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = isdel;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Contents");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����δ��˵��½�����
        /// </summary>
        public int GetCount(int shi)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from Contents");
            strSql.Append(" where ");
            if (shi > 0)
            {
                strSql.Append(" shi=" + shi + " and ");
            }
            strSql.Append(" state=@state and isdel=0");
            SqlParameter[] parameters = {
					new SqlParameter("@state", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            object obj = SqlHelperBook.GetSingle(strSql.ToString(), parameters);
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
        /// ����ĳ�鱾�½ڻ�־�����types(0����/1�־�)
        /// </summary>
        public int GetCount(int shi, int jid, int types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from Contents");
            strSql.Append(" where ");

            strSql.Append(" shi=@shi and ");

            if (jid > 0)
                strSql.Append(" jid=" + jid + " ");
            else
            {
                if (types == 0)
                    strSql.Append(" jid<>-1 ");
                else
                    strSql.Append(" jid=-1 ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@shi", SqlDbType.Int,4)};
            parameters[0].Value = shi;

            object obj = SqlHelperBook.GetSingle(strSql.ToString(), parameters);
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
        public int Add(Book.Model.Contents model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Contents(");
            strSql.Append("shi,title,summary,contents,addtime,state,jid,tags,aid,pid)");
            strSql.Append(" values (");
            strSql.Append("@shi,@title,@summary,@contents,@addtime,@state,@jid,@tags,@aid,@pid)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@shi", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.VarChar,255),
					new SqlParameter("@summary", SqlDbType.VarChar,500),
					new SqlParameter("@contents", SqlDbType.Text),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@jid", SqlDbType.Int,4),
					new SqlParameter("@tags", SqlDbType.VarChar,100),
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@pid", SqlDbType.Int,4)};
            parameters[0].Value = model.shi;
            parameters[1].Value = model.title;
            parameters[2].Value = model.summary;
            parameters[3].Value = model.contents;
            parameters[4].Value = model.addtime;
            parameters[5].Value = model.state;
            parameters[6].Value = model.jid;
            parameters[7].Value = model.tags;
            parameters[8].Value = model.aid;
            parameters[9].Value = model.pid;

            object obj = SqlHelperBook.GetSingle(strSql.ToString(), parameters);
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
        public void Update(Book.Model.Contents model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contents set ");
            strSql.Append("shi=@shi,");
            strSql.Append("title=@title,");
            strSql.Append("summary=@summary,");
            strSql.Append("contents=@contents,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("state=@state,");
            strSql.Append("jid=@jid,");
            strSql.Append("tags=@tags,");
            strSql.Append("aid=@aid,");
            strSql.Append("pid=@pid");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@shi", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.VarChar,255),
					new SqlParameter("@summary", SqlDbType.VarChar,500),
					new SqlParameter("@contents", SqlDbType.Text),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@jid", SqlDbType.Int,4),
					new SqlParameter("@tags", SqlDbType.VarChar,100),
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@pid", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.shi;
            parameters[2].Value = model.title;
            parameters[3].Value = model.summary;
            parameters[4].Value = model.contents;
            parameters[5].Value = model.addtime;
            parameters[6].Value = model.state;
            parameters[7].Value = model.jid;
            parameters[8].Value = model.tags;
            parameters[9].Value = model.aid;
            parameters[10].Value = model.pid;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����½�
        /// </summary>
        public void Updatestate(int id,int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Contents set ");
            strSql.Append("state=@state");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@state", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = state;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            if (Utils.getPageAll().ToLower().Contains("book/"))
            {
                strSql.Append("update Contents set isdel=1");
            }
            else
            {
                strSql.Append("delete from Contents ");
            }
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            if (Utils.getPageAll().ToLower().Contains("book/"))
            {
                strSql.Append("update Contents set isdel=1");
            }
            else
            {
                strSql.Append("delete from Contents ");              
            }
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelperBook.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ��������
        /// </summary>
        public string GetTitle(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 title from Contents ");
            strSql.Append(" where id=@id and jid=0");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
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
        /// �õ�һ������ʵ��
        /// </summary>
        public Book.Model.Contents GetContents(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,shi,title,summary,contents,addtime,state,jid,tags,aid,pid,isdel from Contents ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Book.Model.Contents model = new Book.Model.Contents();
            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.shi = reader.GetInt32(1);
                    model.title = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.summary = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        model.contents = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.addtime = reader.GetDateTime(5);
                    if (!reader.IsDBNull(6))
                        model.state = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        model.jid = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        model.tags = reader.GetString(8);

                    model.aid = reader.GetInt32(9);
                    model.pid = reader.GetInt32(10);
                    model.isdel = reader.GetInt32(11);
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
            strSql.Append(" FROM Contents ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelperBook.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ����(��)һ����¼
        /// </summary>
        public Book.Model.Contents GetPreviousNextContents(int ID, int shi, int jid, bool p_next)
        {
            List<Book.Model.Contents> listContents = new List<Book.Model.Contents>();

            // ��������
            SqlParameter[] tmpSqlParam = {
                new SqlParameter("@shi", SqlDbType.Int, 4),
                new SqlParameter("@jid", SqlDbType.Int, 4),
                new SqlParameter("@ID", SqlDbType.Int, 4)
            };

            string where = "";
            if (shi != 0)
            {
                where += " shi=@shi AND";
                where += " jid=@jid AND";
                tmpSqlParam[0].Value = shi;
                tmpSqlParam[1].Value = jid;
            }

            where += !p_next ? " ID<@ID AND" : " ID>@ID AND";
            tmpSqlParam[2].Value = ID;

            if (where != "")
                where = " WHERE" + where.Substring(0, where.Length - 4);


            // �������� SqlParameter ˳��
            int i = 0;
            SqlParameter[] SqlParam = new SqlParameter[3];
            foreach (SqlParameter p in tmpSqlParam)
            {
                if (p.Value != null)
                {
                    SqlParam[i] = new SqlParameter();
                    SqlParam[i].ParameterName = p.ParameterName;
                    SqlParam[i].SqlDbType = p.SqlDbType;
                    SqlParam[i].Size = p.Size;
                    SqlParam[i].Value = p.Value;
                    i++;
                }
            }

            string order = string.Empty;
            if (!p_next)
            {
                order = " ORDER BY ID DESC";
            }
            else
            {
                order = " ORDER BY ID ASC";
            }


            // ȡ����ؼ�¼

            Book.Model.Contents objContents = new Book.Model.Contents();

            string queryString = "SELECT TOP 1 ID, Title" +
                                " FROM Contents" + where + " and state=1 and isdel=0 " + order;

            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(queryString.ToString(), SqlParam))
            {

                while (reader.Read())
                {

                    objContents.id = reader.GetInt32(0);
                    objContents.title = reader.GetString(1);
                }
            }

            return objContents;
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList Contents</returns>
        public IList<Book.Model.Contents> GetContentss(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<Book.Model.Contents> listContentss = new List<Book.Model.Contents>();
            string sTable = "Contents";
            string sPkey = "id";
            string sField = "id,shi,title,summary,contents,addtime,state,jid,tags,aid,pid";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelperBook.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listContentss;
                }
                while (reader.Read())
                {
                    Book.Model.Contents objContents = new Book.Model.Contents();
                    objContents.id = reader.GetInt32(0);
                    objContents.shi = reader.GetInt32(1);
                    objContents.title = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        objContents.summary = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        objContents.contents = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        objContents.addtime = reader.GetDateTime(5);
                    if (!reader.IsDBNull(6))
                        objContents.state = reader.GetInt32(6);
                    if (!reader.IsDBNull(7))
                        objContents.jid = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        objContents.tags = reader.GetString(8);

                    objContents.aid = reader.GetInt32(9);
                    objContents.pid = reader.GetInt32(10);
                    listContentss.Add(objContents);
                }
            }
            return listContentss;
        }

        #endregion  ��Ա����
    }
}

