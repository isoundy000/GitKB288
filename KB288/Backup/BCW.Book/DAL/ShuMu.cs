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
    /// ���ݷ�����ShuMu��
    /// </summary>
    public class ShuMu
    {
        public ShuMu()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperBook.GetMaxID("id", "ShuMu");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ShuMu");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ����ĳ�����¼
        /// </summary>
        public bool ExistsNode(int nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ShuMu");
            strSql.Append(" where nid=@nid ");
            SqlParameter[] parameters = {
					new SqlParameter("@nid", SqlDbType.Int,4)};
            parameters[0].Value = nid;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����δ��˵��鱾����
        /// </summary>
        public int GetCount(int nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from ShuMu");
            strSql.Append(" where ");
            if (nid > 0)
            {
                strSql.Append(" nid=" + nid + " and ");
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
        /// ����һ������
        /// </summary>
        public int Add(Book.Model.ShuMu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ShuMu(");
            strSql.Append("nid,title,summary,img,aid,author,addtime,state,tags,types,length,click,good,gxtime,isover,isgood)");
            strSql.Append(" values (");
            strSql.Append("@nid,@title,@summary,@img,@aid,@author,@addtime,@state,@tags,@types,@length,@click,@good,@gxtime,@isover,@isgood)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.VarChar,200),
					new SqlParameter("@summary", SqlDbType.VarChar,500),
					new SqlParameter("@img", SqlDbType.VarChar,200),
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@author", SqlDbType.VarChar,50),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@tags", SqlDbType.VarChar,100),
					new SqlParameter("@types", SqlDbType.TinyInt,1),
					new SqlParameter("@length", SqlDbType.Int,4),
					new SqlParameter("@click", SqlDbType.Int,4),
					new SqlParameter("@good", SqlDbType.Int,4),
					new SqlParameter("@gxtime", SqlDbType.DateTime),
					new SqlParameter("@isover", SqlDbType.TinyInt,1),
					new SqlParameter("@isgood", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.nid;
            parameters[1].Value = model.title;
            parameters[2].Value = model.summary;
            parameters[3].Value = model.img;
            parameters[4].Value = model.aid;
            parameters[5].Value = model.author;
            parameters[6].Value = model.addtime;
            parameters[7].Value = model.state;
            parameters[8].Value = model.tags;
            parameters[9].Value = model.types;
            parameters[10].Value = model.length;
            parameters[11].Value = 0;
            parameters[12].Value = 0;
            parameters[13].Value = DateTime.Now;
            parameters[14].Value = 0;
            parameters[15].Value = model.isgood;

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
        public void Update(Book.Model.ShuMu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("nid=@nid,");
            strSql.Append("title=@title,");
            strSql.Append("summary=@summary,");
            strSql.Append("img=@img,");
            strSql.Append("aid=@aid,");
            strSql.Append("author=@author,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("state=@state,");
            strSql.Append("tags=@tags,");
            strSql.Append("types=@types,");
            strSql.Append("length=@length,");
            strSql.Append("isover=@isover");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.VarChar,200),
					new SqlParameter("@summary", SqlDbType.VarChar,500),
					new SqlParameter("@img", SqlDbType.VarChar,200),
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@author", SqlDbType.VarChar,50),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@tags", SqlDbType.VarChar,100),
					new SqlParameter("@types", SqlDbType.TinyInt,1),
					new SqlParameter("@length", SqlDbType.Int,4),
					new SqlParameter("@isover", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.nid;
            parameters[2].Value = model.title;
            parameters[3].Value = model.summary;
            parameters[4].Value = model.img;
            parameters[5].Value = model.aid;
            parameters[6].Value = model.author;
            parameters[7].Value = model.addtime;
            parameters[8].Value = model.state;
            parameters[9].Value = model.tags;
            parameters[10].Value = model.types;
            parameters[11].Value = model.length;
            parameters[12].Value = model.isover;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������(��̨ʹ��)
        /// </summary>
        public void Update2(Book.Model.ShuMu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("nid=@nid,");
            strSql.Append("title=@title,");
            strSql.Append("summary=@summary,");
            strSql.Append("img=@img,");
            strSql.Append("aid=@aid,");
            strSql.Append("author=@author,");
            strSql.Append("addtime=@addtime,");
            strSql.Append("state=@state,");
            strSql.Append("tags=@tags,");
            strSql.Append("types=@types,");
            strSql.Append("length=@length,");
            strSql.Append("click=@click,");
            strSql.Append("good=@good,");
            strSql.Append("gxtime=@gxtime,");
            strSql.Append("isover=@isover,");
            strSql.Append("isgood=@isgood");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.VarChar,200),
					new SqlParameter("@summary", SqlDbType.VarChar,500),
					new SqlParameter("@img", SqlDbType.VarChar,200),
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@author", SqlDbType.VarChar,50),
					new SqlParameter("@addtime", SqlDbType.DateTime),
					new SqlParameter("@state", SqlDbType.Int,4),
					new SqlParameter("@tags", SqlDbType.VarChar,100),
					new SqlParameter("@types", SqlDbType.TinyInt,1),
					new SqlParameter("@length", SqlDbType.Int,4),
					new SqlParameter("@click", SqlDbType.Int,4),
					new SqlParameter("@good", SqlDbType.Int,4),
					new SqlParameter("@gxtime", SqlDbType.DateTime),
					new SqlParameter("@isover", SqlDbType.TinyInt,1),
					new SqlParameter("@isgood", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.nid;
            parameters[2].Value = model.title;
            parameters[3].Value = model.summary;
            parameters[4].Value = model.img;
            parameters[5].Value = model.aid;
            parameters[6].Value = model.author;
            parameters[7].Value = model.addtime;
            parameters[8].Value = model.state;
            parameters[9].Value = model.tags;
            parameters[10].Value = model.types;
            parameters[11].Value = model.length;
            parameters[12].Value = model.click;
            parameters[13].Value = model.good;
            parameters[14].Value = model.gxtime;
            parameters[15].Value = model.isover;
            parameters[16].Value = model.isgood;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƽ��鱾
        /// </summary>
        public void UpdateGood(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("good=good+1");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ��������
        /// </summary>
        public void UpdateClick(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("click=click+1");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����鱾
        /// </summary>
        public void Updatestate(int id, int state)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
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
        /// �ָ���ɾ��
        /// </summary>
        public void Updateisdel(int id, int isdel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
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
        /// д�����ʱ��
        /// </summary>
        public void Updategxtime(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("gxtime=@gxtime");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@gxtime", SqlDbType.DateTime)};
            parameters[0].Value = id;
            parameters[1].Value = DateTime.Now;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// д������ID
        /// </summary>
        public void Updategxids(int id, string gxids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("gxids=@gxids");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@gxids", SqlDbType.NText)};
            parameters[0].Value = id;
            parameters[1].Value = gxids;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// д��������Ŀ
        /// </summary>
        public void Updatepl(int id, int pl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("pl=pl+@pl");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@pl", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = pl;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���´���ID��ת���ã�
        /// </summary>
        public void Updatenid(int id, int nid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuMu set ");
            strSql.Append("nid=@nid");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = nid;

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
                strSql.Append("update ShuMu set isdel=1");
            }
            else
            {
                strSql.Append("delete from ShuMu ");
            }
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�����ID
        /// </summary>
        public int Getnid(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 nid from ShuMu ");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
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
        /// �õ����ѵĻ�ԱID
        /// </summary>
        public string Getgxids(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gxids from ShuMu ");
            strSql.Append(" where id=@id");
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
        public Book.Model.ShuMu GetShuMu(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,nid,title,summary,img,aid,author,addtime,state,tags,types,length,click,good,gxtime,isover,isgood,pl,gxids,isdel from ShuMu ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Book.Model.ShuMu model = new Book.Model.ShuMu();
            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.nid = reader.GetInt32(1);
                    model.title = reader.GetString(2);
                    model.summary = reader.GetString(3);
                    model.img = reader.GetString(4);
                    if (!reader.IsDBNull(5))
                        model.aid = reader.GetInt32(5);
                    model.author = reader.GetString(6);
                    model.addtime = reader.GetDateTime(7);
                    if (!reader.IsDBNull(8))
                        model.state = reader.GetInt32(8);
                    model.tags = reader.GetString(9);
                    model.types = reader.GetByte(10);
                    model.length = reader.GetInt32(11);
                    model.click = reader.GetInt32(12);
                    model.good = reader.GetInt32(13);
                    model.gxtime = reader.GetDateTime(14);
                    model.isover = reader.GetByte(15);
                    model.isgood = reader.GetByte(16);
                    model.pl = reader.GetInt32(17);
                    if (!reader.IsDBNull(18))
                        model.gxids = reader.GetString(18);
                    model.isdel = reader.GetInt32(19);
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
            strSql.Append(" FROM ShuMu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelperBook.Query(strSql.ToString());
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<Book.Model.ShuMu> listShuMus = new List<Book.Model.ShuMu>();
            string sTable = "ShuMu";
            string sPkey = "id";
            string sField = "id,nid,title,state,isover";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
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
                    return listShuMus;
                }
                while (reader.Read())
                {
                    Book.Model.ShuMu objShuMu = new Book.Model.ShuMu();
                    objShuMu.id = reader.GetInt32(0);
                    objShuMu.nid = reader.GetInt32(1);
                    objShuMu.title = reader.GetString(2);
                    objShuMu.state = reader.GetInt32(3);
                    objShuMu.isover = reader.GetByte(4);
                    listShuMus.Add(objShuMu);
                }
            }
            return listShuMus;
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <param name="strOrder">��������</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMus(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<Book.Model.ShuMu> listShuMus = new List<Book.Model.ShuMu>();
            string sTable = "ShuMu";
            string sPkey = "id";
            string sField = "id,nid,title,state,isover";
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
                    return listShuMus;
                }
                while (reader.Read())
                {
                    Book.Model.ShuMu objShuMu = new Book.Model.ShuMu();
                    objShuMu.id = reader.GetInt32(0);
                    objShuMu.nid = reader.GetInt32(1);
                    objShuMu.title = reader.GetString(2);
                    objShuMu.state = reader.GetInt32(3);
                    objShuMu.isover = reader.GetByte(4);
                    listShuMus.Add(objShuMu);
                }
            }
            return listShuMus;
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList ShuMu</returns>
        public IList<Book.Model.ShuMu> GetShuMusTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<Book.Model.ShuMu> listShuMus = new List<Book.Model.ShuMu>();
            string sTable = "ShuMu";
            string sPkey = "id";
            string sField = "id,nid,title,click";
            string sCondition = strWhere;
            string sOrder = "click Desc";
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
                    return listShuMus;
                }
                while (reader.Read())
                {
                    Book.Model.ShuMu objShuMu = new Book.Model.ShuMu();
                    objShuMu.id = reader.GetInt32(0);
                    objShuMu.nid = reader.GetInt32(1);
                    objShuMu.title = reader.GetString(2);
                    objShuMu.click = reader.GetInt32(3);
                    listShuMus.Add(objShuMu);
                }
            }
            return listShuMus;
        }

        #endregion  ��Ա����
    }
}

