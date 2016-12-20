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
    /// ���ݷ�����Shopuser��
    /// </summary>
    public class Shopuser
    {
        public Shopuser()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Shopuser");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopuser");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int UsID, int GiftId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopuser");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GiftId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = GiftId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼_ũ�� //�۹���20160607 ����ũ�������ѯ�жϱ�ʶ
        /// </summary>
        public bool Exists_nc(int UsID, int GiftId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopuser");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId and pic=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@GiftId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = GiftId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ĳID��������
        /// </summary>
        public int GetCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Total) from tb_Shopuser");
            strSql.Append(" where UsID=@UsID and PIC=0");
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
        /// �۹��� 20160512
        /// ����ĳID��ũ������
        /// </summary>
        public int GetCount_nc(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Total) from tb_Shopuser");
            strSql.Append(" where UsID=@UsID and PIC=1");
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
        public int Add(BCW.Model.Shopuser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Shopuser(");
            strSql.Append("UsID,UsName,GiftId,GiftTitle,PrevPic,Total,AddTime,PIC)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@GiftId,@GiftTitle,@PrevPic,@Total,@AddTime,@PIC)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftId", SqlDbType.Int,4),
                    new SqlParameter("@GiftTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
                    new SqlParameter("@Total", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@PIC", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.GiftId;
            parameters[3].Value = model.GiftTitle;
            parameters[4].Value = model.PrevPic;
            parameters[5].Value = model.Total;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.PIC;

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
        public void Update(BCW.Model.Shopuser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopuser set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("GiftTitle=@GiftTitle,");
            strSql.Append("PrevPic=@PrevPic,");
            strSql.Append("Total=Total+@Total,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@GiftTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.GiftTitle;
            parameters[4].Value = model.GiftId;
            parameters[5].Value = model.PrevPic;
            parameters[6].Value = model.Total;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������_����pic=1 //�۹���20160607 ����ũ����������жϱ�ʶ
        /// </summary>
        public void Update_nc(BCW.Model.Shopuser model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopuser set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("GiftTitle=@GiftTitle,");
            strSql.Append("PrevPic=@PrevPic,");
            strSql.Append("Total=Total+@Total,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId and pic=1");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftTitle", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftId", SqlDbType.Int,4),
                    new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
                    new SqlParameter("@Total", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.GiftTitle;
            parameters[4].Value = model.GiftId;
            parameters[5].Value = model.PrevPic;
            parameters[6].Value = model.Total;
            parameters[7].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Shopuser ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Shopuser GetShopuser(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,GiftId,GiftTitle,PrevPic,Total from tb_Shopuser ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Shopuser model = new BCW.Model.Shopuser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.GiftId = reader.GetInt32(3);
                    model.GiftTitle = reader.GetString(4);
                    model.PrevPic = reader.GetString(5);
                    model.Total = reader.GetInt32(6);
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
        public BCW.Model.Shopuser GetShopuser(int UsID, int GiftId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,GiftId,GiftTitle,PrevPic,Total from tb_Shopuser ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and GiftId=@GiftId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@GiftId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = GiftId;

            BCW.Model.Shopuser model = new BCW.Model.Shopuser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.GiftId = reader.GetInt32(3);
                    model.GiftTitle = reader.GetString(4);
                    model.PrevPic = reader.GetString(5);
                    model.Total = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_Shopuser ");
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
        /// <returns>IList Shopuser</returns>
        public IList<BCW.Model.Shopuser> GetShopusers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Shopuser> listShopusers = new List<BCW.Model.Shopuser>();
            string sTable = "tb_Shopuser";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,GiftId,GiftTitle,PrevPic,Total";
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
                    return listShopusers;
                }
                while (reader.Read())
                {
                    BCW.Model.Shopuser objShopuser = new BCW.Model.Shopuser();
                    objShopuser.ID = reader.GetInt32(0);
                    objShopuser.UsID = reader.GetInt32(1);
                    objShopuser.UsName = reader.GetString(2);
                    objShopuser.GiftId = reader.GetInt32(3);
                    objShopuser.GiftTitle = reader.GetString(4);
                    objShopuser.PrevPic = reader.GetString(5);
                    objShopuser.Total = reader.GetInt32(6);
                    listShopusers.Add(objShopuser);
                }
            }
            return listShopusers;
        }

        /// <summary>
        /// ������˰�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Shopuser> GetShopusersTop(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Shopuser> listShopuser = new List<BCW.Model.Shopuser>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Shopuser";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listShopuser;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT UsID, Sum(Total) as Total FROM tb_Shopuser GROUP BY UsID ORDER BY Sum(Total) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Shopuser objShopuser = new BCW.Model.Shopuser();
                        objShopuser.UsID = reader.GetInt32(0);
                        objShopuser.Total = reader.GetInt32(1);
                        listShopuser.Add(objShopuser);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listShopuser;
        }


        #endregion  ��Ա����
    }
}
