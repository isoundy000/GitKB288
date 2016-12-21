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
    /// ���ݷ�����Favorites��
    /// </summary>
    public class Favorites
    {
        public Favorites()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Favorites");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Favorites");
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
            strSql.Append("select count(1) from tb_Favorites");
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
        public bool Exists(int ID, int NodeId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Favorites");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and NodeId=@NodeId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NodeId;
            parameters[2].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsTitle(int UsID, string Title, string PUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Favorites");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Title=@Title ");
            strSql.Append(" and PUrl=@PUrl ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,200)};
            parameters[0].Value = UsID;
            parameters[1].Value = Title;
            parameters[2].Value = PUrl;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ĳ�����ղ�����
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Favorites");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

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
        /// ����ĳ�����ղ�����
        /// </summary>
        public int GetTypesCount(int UsID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Favorites");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = Types;

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
        public int Add(BCW.Model.Favorites model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Favorites(");
            strSql.Append("Types,NodeId,UsID,Title,PUrl,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@NodeId,@UsID,@Title,@PUrl,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.PUrl;
            parameters[5].Value = model.AddTime;

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
        public void Update(BCW.Model.Favorites model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Favorites set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("Title=@Title,");
            strSql.Append("PUrl=@PUrl,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.PUrl;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Favorites ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Friend ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Favorites GetFavorites(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,NodeId,UsID,Title,PUrl,AddTime from tb_Favorites ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Favorites model = new BCW.Model.Favorites();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.NodeId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.Title = reader.GetString(4);
                    model.PUrl = reader.GetString(5);
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
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Favorites ");
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
        /// <returns>IList Favorites</returns>
        public IList<BCW.Model.Favorites> GetFavoritess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Favorites> listFavoritess = new List<BCW.Model.Favorites>();
            string sTable = "tb_Favorites";
            string sPkey = "id";
            string sField = "ID,NodeId,Title";
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
                    return listFavoritess;
                }
                while (reader.Read())
                {
                    BCW.Model.Favorites objFavorites = new BCW.Model.Favorites();
                    objFavorites.ID = reader.GetInt32(0);
                    objFavorites.NodeId = reader.GetInt32(1);
                    objFavorites.Title = reader.GetString(2);
                    listFavoritess.Add(objFavorites);
                }
            }
            return listFavoritess;
        }

        #endregion  ��Ա����
    }
}
