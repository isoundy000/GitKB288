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
    /// ���ݷ�����Advert��
    /// </summary>
    public class Advert
    {
        public Advert()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Advert");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Advert");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Advert");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and Status=@Status ");
            strSql.Append(" and OverTime>@OverTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = 0;
            parameters[2].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Advert model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Advert(");
            strSql.Append("Title,AdUrl,StartTime,OverTime,Status,iGold,Click,adType,UrlType,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Title,@AdUrl,@StartTime,@OverTime,@Status,@iGold,@Click,@adType,@UrlType,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@AdUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@iGold", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@adType", SqlDbType.Int,4),
					new SqlParameter("@UrlType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.AdUrl;
            parameters[2].Value = model.StartTime;
            parameters[3].Value = model.OverTime;
            parameters[4].Value = model.Status;
            parameters[5].Value = model.iGold;
            parameters[6].Value = model.Click;
            parameters[7].Value = model.adType;
            parameters[8].Value = model.UrlType;
            parameters[9].Value = model.AddTime;

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
        public void Update(BCW.Model.Advert model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Advert set ");
            strSql.Append("Title=@Title,");
            strSql.Append("AdUrl=@AdUrl,");
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("Status=@Status,");
            strSql.Append("iGold=@iGold,");
            strSql.Append("Click=@Click,");
            strSql.Append("adType=@adType,");
            strSql.Append("UrlType=@UrlType,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@AdUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@iGold", SqlDbType.Int,4),
					new SqlParameter("@Click", SqlDbType.Int,4),
					new SqlParameter("@adType", SqlDbType.Int,4),
					new SqlParameter("@UrlType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.AdUrl;
            parameters[3].Value = model.StartTime;
            parameters[4].Value = model.OverTime;
            parameters[5].Value = model.Status;
            parameters[6].Value = model.iGold;
            parameters[7].Value = model.Click;
            parameters[8].Value = model.adType;
            parameters[9].Value = model.UrlType;
            parameters[10].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���µ��
        /// </summary>
        public void UpdateClick(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Advert set ");
            strSql.Append("Click=Click+1");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���½�������ID
        /// </summary>
        public void UpdateClickID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Advert set ");
            strSql.Append("ClickID=@ClickID");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ClickID", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = "";

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���½�������ID
        /// </summary>
        public void UpdateClickID(int ID, string ClickID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Advert set ");
            strSql.Append("ClickID=@ClickID,");
            strSql.Append("ClickTime=@ClickTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ClickID", SqlDbType.NText),
					new SqlParameter("@ClickTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ClickID;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Advert ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����õ�һ�����
        /// </summary>
        public BCW.Model.Advert GetAdvert()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,AdUrl,UrlType from tb_Advert ");
            strSql.Append(" where Status=0 and OverTime>'" + DateTime.Now + "' Order By NewID() ");

            BCW.Model.Advert model = new BCW.Model.Advert();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.AdUrl = reader.GetString(2);
                    model.UrlType = reader.GetInt32(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Advert GetAdvert(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,AdUrl,StartTime,OverTime,Status,iGold,Click,ClickTime,ClickID,adType,AddTime from tb_Advert ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Advert model = new BCW.Model.Advert();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.AdUrl = reader.GetString(2);
                    model.StartTime = reader.GetDateTime(3);
                    model.OverTime = reader.GetDateTime(4);
                    model.Status = reader.GetInt32(5);
                    model.iGold = reader.GetInt32(6);
                    model.Click = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        model.ClickTime = reader.GetDateTime(8);
                    if (!reader.IsDBNull(9))
                        model.ClickID = reader.GetString(9);
                    model.adType = reader.GetInt32(10);
                    model.AddTime = reader.GetDateTime(11);
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
            strSql.Append(" FROM tb_Advert ");
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
        /// <returns>IList Advert</returns>
        public IList<BCW.Model.Advert> GetAdverts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Advert> listAdverts = new List<BCW.Model.Advert>();
            string sTable = "tb_Advert";
            string sPkey = "id";
            string sField = "ID,Title,Click,iGold,adType";
            string sCondition = strWhere;
            string sOrder = "OverTime Desc";
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
                    return listAdverts;
                }
                while (reader.Read())
                {
                    BCW.Model.Advert objAdvert = new BCW.Model.Advert();
                    objAdvert.ID = reader.GetInt32(0);
                    objAdvert.Title = reader.GetString(1);
                    objAdvert.Click = reader.GetInt32(2);
                    objAdvert.iGold = reader.GetInt32(3);
                    objAdvert.adType = reader.GetInt32(4);
                    listAdverts.Add(objAdvert);
                }
            }
            return listAdverts;
        }

        #endregion  ��Ա����
    }
}

