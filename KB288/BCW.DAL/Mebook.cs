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
    /// ���ݷ�����Mebook��
    /// </summary>
    public class Mebook
    {
        public Mebook()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Mebook");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mebook");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mebook");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mebook");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and MID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ĳ��ԱID���Ա�������
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mebook");
            strSql.Append(" where UsID=@UsID and type=0");//�۹��� 20160524 ����type=0
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
        /// ����ĳ��ԱID�����������
        /// </summary>
        public int GetIDCount(int MID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Mebook");
            strSql.Append(" where MID=@MID ");
            SqlParameter[] parameters = {
					new SqlParameter("@MID", SqlDbType.Int,4)};
            parameters[0].Value = MID;
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
        public int Add(BCW.Model.Mebook model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Mebook(");
            strSql.Append("UsID,MID,MName,MContent,IsTop,AddTime,Type)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@MID,@MName,@MContent,@IsTop,@AddTime,@Type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@MID", SqlDbType.Int,4),
					new SqlParameter("@MName", SqlDbType.NVarChar,50),
					new SqlParameter("@MContent", SqlDbType.NVarChar,500),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@Type", SqlDbType.Int,4)};//�۹��� 20160520 ����type
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.MID;
            parameters[2].Value = model.MName;
            parameters[3].Value = model.MContent;
            parameters[4].Value = model.IsTop;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.Type;

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
        public void Update(BCW.Model.Mebook model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mebook set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("MID=@MID,");
            strSql.Append("MName=@MName,");
            strSql.Append("MContent=@MContent,");
            strSql.Append("IsTop=@IsTop,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@MID", SqlDbType.Int,4),
					new SqlParameter("@MName", SqlDbType.NVarChar,50),
					new SqlParameter("@MContent", SqlDbType.NVarChar,500),
					new SqlParameter("@IsTop", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.MID;
            parameters[3].Value = model.MName;
            parameters[4].Value = model.MContent;
            parameters[5].Value = model.IsTop;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���»ظ�����
        /// </summary>
        public void UpdateReText(int ID, string ReText)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mebook set ");
            strSql.Append("ReText=@ReText");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReText", SqlDbType.NVarChar,500)};
            parameters[0].Value = ID;
            parameters[1].Value = ReText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �����Ƿ��ö�
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Mebook set ");
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
            strSql.Append("delete from tb_Mebook ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  ɾ��ũ����������
        /// </summary>
        public void Delete_farm(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Mebook ");
            strSql.Append(" where Type=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int MID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Mebook ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and MID=@MID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@MID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = MID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Mebook ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ��MID
        /// </summary>
        public int GetMID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 MID from tb_Mebook ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// �õ�һ��IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsTop from tb_Mebook ");
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Mebook GetMebook(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_Mebook ");//�۹��� 20160520 ����type
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Mebook model = new BCW.Model.Mebook();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.MID = reader.GetInt32(2);
                    model.MName = reader.GetString(3);
                    model.MContent = reader.GetString(4);
                    model.IsTop = reader.GetByte(5);
                    model.AddTime = reader.GetDateTime(6);

                    if (!reader.IsDBNull(7))
                        model.ReText = reader.GetString(7);

                    model.Type = reader.GetInt32(8);

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
            strSql.Append(" FROM tb_Mebook ");
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
        /// <returns>IList Mebook</returns>
        public IList<BCW.Model.Mebook> GetMebooks(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Mebook> listMebooks = new List<BCW.Model.Mebook>();
            string sTable = "tb_Mebook";
            string sPkey = "id";
            string sField = "MID,MName,MContent,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listMebooks;
                }
                while (reader.Read())
                {
                    BCW.Model.Mebook objMebook = new BCW.Model.Mebook();
                    objMebook.MID = reader.GetInt32(0);
                    objMebook.MName = reader.GetString(1);
                    objMebook.MContent = reader.GetString(2);
                    objMebook.AddTime = reader.GetDateTime(3);
                    listMebooks.Add(objMebook);
                }
            }
            return listMebooks;
        }


        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Mebook</returns>
        public IList<BCW.Model.Mebook> GetMebooks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Mebook> listMebooks = new List<BCW.Model.Mebook>();
            string sTable = "tb_Mebook";
            string sPkey = "id";
            string sField = "*";
            string sCondition = strWhere;
            string sOrder = "IsTop Desc, ID Desc";
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
                    return listMebooks;
                }
                while (reader.Read())
                {
                    BCW.Model.Mebook objMebook = new BCW.Model.Mebook();
                    objMebook.ID = reader.GetInt32(0);
                    objMebook.UsID = reader.GetInt32(1);
                    objMebook.MID = reader.GetInt32(2);
                    objMebook.MName = reader.GetString(3);
                    objMebook.MContent = reader.GetString(4);
                    objMebook.IsTop = reader.GetByte(5);
                    objMebook.AddTime = reader.GetDateTime(6);

                    if (!reader.IsDBNull(7))
                        objMebook.ReText = reader.GetString(7);

                    objMebook.Type = reader.GetInt32(8);//�۹��� 20160520 ����type

                    listMebooks.Add(objMebook);
                }
            }
            return listMebooks;
        }

        #endregion  ��Ա����
    }
}

