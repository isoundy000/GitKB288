using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
    /// <summary>
    /// ���ݷ�����GiftFlows��
    /// </summary>
    public class GiftFlows
    {
        public GiftFlows()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("Types", "tb_GiftFlows");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int Types, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼(�涨����)
        /// </summary>
        public bool Exists(int Types, int UsID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and UsID=@UsID and Totall>=@Totall ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = Totall;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼(N����)
        /// </summary>
        public bool ExistsSec(int Types, int UsID, int Sec)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_GiftFlows");
            strSql.Append(" where Types=@Types and (UsID=@UsID OR UsIP='" + Utils.GetUsIP() + "') and AddTime>='" + DateTime.Now.AddSeconds(-Sec) + "' ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ���㲻ͬ��Ʒ�ж��ٸ�����
        /// </summary>
        public int GetTypesTotal(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(DISTINCT Types) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID and Totall>@Totall");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = 0;

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
        /// ����ĳ�û���������
        /// </summary>
        public int GetTotal(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Total) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID ");
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
        /// ����ĳ�û�����ʣ����
        /// </summary>
        public int GetTotall(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Totall) from tb_GiftFlows");
            strSql.Append(" where UsID=@UsID ");
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
        public int Add(BCW.Model.Game.GiftFlows model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_GiftFlows(");
            strSql.Append("Types,UsID,UsName,Total,Totall,AddTime,UsIP)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@Total,@Totall,@AddTime,@UsIP)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@UsIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Total;
            parameters[4].Value = model.Totall;
            parameters[5].Value = DateTime.Now;
            parameters[6].Value = Utils.GetUsIP();

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
        public void Update(BCW.Model.Game.GiftFlows model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Total=Total+@Total,");
            strSql.Append("Totall=Totall+@Totall,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where Types=@Types and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Total;
            parameters[5].Value = model.Totall;
            parameters[6].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int UpdateTotall(int Types, int UsID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("Totall=Totall+@Totall");
            strSql.Append(" where Types=@Types and UsID=@UsID and Totall>0");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = Totall;

           return SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int UpdateTotall(int ID, int Totall)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GiftFlows set ");
            strSql.Append("Totall=Totall+@Totall");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Totall", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Totall;

            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);

        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_GiftFlows ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.GiftFlows GetGiftFlows(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,Total,Totall from tb_GiftFlows ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            BCW.Model.Game.GiftFlows model = new BCW.Model.Game.GiftFlows();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.Total = reader.GetInt32(4);
                    model.Totall = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_GiftFlows ");
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
        /// <returns>IList GiftFlows</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlowss = new List<BCW.Model.Game.GiftFlows>();
            string sTable = "tb_GiftFlows";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,Total,Totall";
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
                    return listGiftFlowss;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                    objGiftFlows.ID = reader.GetInt32(0);
                    objGiftFlows.Types = reader.GetInt32(1);
                    objGiftFlows.UsID = reader.GetInt32(2);
                    objGiftFlows.UsName = reader.GetString(3);
                    objGiftFlows.Total = reader.GetInt32(4);
                    objGiftFlows.Totall = reader.GetInt32(5);
                    listGiftFlowss.Add(objGiftFlows);
                }
            }
            return listGiftFlowss;
        }

        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlows = new List<BCW.Model.Game.GiftFlows>();

            string strWhe = "";
            if (strWhere != "")
                strWhe = " where " + strWhere + "";

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_GiftFlows " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 50)
                    pageCount = 50;
            }
            else
            {
                return listGiftFlows;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT UsID, Sum(Total) as Total FROM tb_GiftFlows " + strWhe + " GROUP BY UsID ORDER BY Sum(Total) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                        objGiftFlows.UsID = reader.GetInt32(0);
                        objGiftFlows.Total = reader.GetInt32(1);
                        listGiftFlows.Add(objGiftFlows);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGiftFlows;
        }


        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Game.GiftFlows> GetGiftFlowssTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.GiftFlows> listGiftFlows = new List<BCW.Model.Game.GiftFlows>();

            string strWhe = "";
            if (strWhere != "")
                strWhe = " where " + strWhere + "";

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT Types) FROM tb_GiftFlows " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 50)
                    pageCount = 50;
            }
            else
            {
                return listGiftFlows;
            }

            // ȡ����ؼ�¼

            string queryString = "SELECT Types, Sum(Total) as Total, Sum(Totall) as Totall FROM tb_GiftFlows " + strWhe + " GROUP BY Types ORDER BY Sum(Total) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.GiftFlows objGiftFlows = new BCW.Model.Game.GiftFlows();
                        objGiftFlows.Types = reader.GetInt32(0);
                        objGiftFlows.Total = reader.GetInt32(1);
                        objGiftFlows.Totall = reader.GetInt32(2);
                        listGiftFlows.Add(objGiftFlows);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listGiftFlows;
        }

        #endregion  ��Ա����
    }
}

