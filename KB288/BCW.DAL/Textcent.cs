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
    /// ���ݷ�����Textcent��
    /// </summary>
    public class Textcent
    {
        public Textcent()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Textcent");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Textcent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���������õ�����ʹ����̳������ͱҶ�
        /// </summary>
        public long GetForrmCents(int BID, int BzType, int ToID)
        {
            StringBuilder strSql = new StringBuilder();
            if (BzType == 0)
                strSql.Append("SELECT Sum(Cents) from tb_Textcent");
            else
                strSql.Append("SELECT Sum(Cents) from tb_Textcent");

            strSql.Append(" where BzType=" + BzType + " and ");
            strSql.Append(" PayByFund=" + 1 + " and ");
            if (BID > 0)
            {
                strSql.Append(" BID=" + BID + " and ");

            }
            if (ToID > 0)
            {
                strSql.Append(" ToID=" + ToID + " and ");
            }
            strSql.Append(" Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        /// <summary>
        /// ���������õ�������˴��ͳ�ȥ�ıҶ�
        /// </summary>
        public long GetCents(int BID, int BzType, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            if (BzType == 0)
                strSql.Append("SELECT Sum(Cents) from tb_Textcent");
            else
                strSql.Append("SELECT Sum(Cents) from tb_Textcent");

            strSql.Append(" where BzType=" + BzType + " and ");
            if (BID > 0)
            {
                strSql.Append(" BID=" + BID + " and ");

            }
            if (UsID > 0)
            {
                strSql.Append(" UsID=" + UsID + " and ");
            }
            strSql.Append(" Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Textcent model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Textcent(");
            strSql.Append("BID,UsID,UsName,ToID,Cents,BzType,Notes,AddTime,ReplyFloor,PayByFund)");
            strSql.Append(" values (");
            strSql.Append("@BID,@UsID,@UsName,@ToID,@Cents,@BzType,@Notes,@AddTime,@ReplyFloor,@PayByFund)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@Cents", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@ReplyFloor", SqlDbType.Int,4),
                    new SqlParameter("@PayByFund", SqlDbType.Int,4)};
            parameters[0].Value = model.BID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.ToID;
            parameters[4].Value = model.Cents;
            parameters[5].Value = model.BzType;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.ReplyFloor;
            parameters[9].Value = model.PayByFund;
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
        public void Update(BCW.Model.Textcent model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textcent set ");
            strSql.Append("BID=@BID,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("ToID=@ToID,");
            strSql.Append("Cents=@Cents,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append("PayByFund=@PayByFund");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToID", SqlDbType.Int,4),
					new SqlParameter("@Cents", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@PayByFund", SqlDbType.Int,4),};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BID;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.ToID;
            parameters[5].Value = model.Cents;
            parameters[6].Value = model.BzType;
            parameters[7].Value = model.Notes;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.PayByFund;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Textcent ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Textcent GetTextcent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,BID,UsID,UsName,ToID,Cents,BzType,Notes,AddTime from tb_Textcent ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Textcent model = new BCW.Model.Textcent();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.BID = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.ToID = reader.GetInt32(4);
                    model.Cents = reader.GetInt64(5);
                    model.BzType = reader.GetByte(6);
                    model.Notes = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// �õ�һ�����Ͷ����ڻظ�����ģ�
        /// </summary>
        public BCW.Model.Textcent GetTextcentReplyFloor(int ToID,int ReplyFloor, int BID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UsName,Cents,BzType from tb_Textcent ");
            strSql.Append(" where ToID=@ToID And BID=@BID And ReplyFloor=@ReplyFloor  order by Addtime desc");
            SqlParameter[] parameters = {
					new SqlParameter("@ToID", SqlDbType.Int,4),
                    new SqlParameter("@BID", SqlDbType.Int,4),
                    new SqlParameter("@ReplyFloor", SqlDbType.Int,4)};
            parameters[0].Value = ToID;
            parameters[1].Value = BID;
            parameters[2].Value = ReplyFloor;
            BCW.Model.Textcent model = new BCW.Model.Textcent();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsName = reader.GetString(0);
                    model.Cents = reader.GetInt64(1);
                    model.BzType = reader.GetByte(2);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// �õ����һ������ʵ��
        /// </summary>
        public BCW.Model.Textcent GetTextcentLast(int BID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UsName,Cents,BzType from tb_Textcent ");
            strSql.Append(" where BID=@BID  Order by id desc");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4)};
            parameters[0].Value = BID;

            BCW.Model.Textcent model = new BCW.Model.Textcent();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.UsName = reader.GetString(0);
                    model.Cents = reader.GetInt64(1);
                    model.BzType = reader.GetByte(2);
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
            strSql.Append(" FROM tb_Textcent ");
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
        /// <returns>IList Textcent</returns>
        public IList<BCW.Model.Textcent> GetTextcents(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Textcent> listTextcents = new List<BCW.Model.Textcent>();
            string sTable = "tb_Textcent";
            string sPkey = "id";
            string sField = "ID,BID,UsID,UsName,ToID,Cents,BzType,Notes,AddTime,ReplyFloor,PayByFund";
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
                    return listTextcents;
                }
                while (reader.Read())
                {
                    BCW.Model.Textcent objTextcent = new BCW.Model.Textcent();
                    objTextcent.ID = reader.GetInt32(0);
                    objTextcent.BID = reader.GetInt32(1);
                    objTextcent.UsID = reader.GetInt32(2);
                    objTextcent.UsName = reader.GetString(3);
                    objTextcent.ToID = reader.GetInt32(4);
                    objTextcent.Cents = reader.GetInt64(5);
                    objTextcent.BzType = reader.GetByte(6);
                    objTextcent.Notes = reader.GetString(7);
                    objTextcent.AddTime = reader.GetDateTime(8);
                    objTextcent.ReplyFloor = reader.GetInt32(9);
                    objTextcent.PayByFund = reader.GetInt32(10);
                    listTextcents.Add(objTextcent);
                }
            }
            return listTextcents;
        }

        /// <summary>
        /// ȡ�����а�
        /// </summary>
        /// <param name="Types">�������</param>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">ÿҳ��ʾ��¼��</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Textcent> GetTextcentsTop(int Types, int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Textcent> listTextcent = new List<BCW.Model.Textcent>();

            string strWhe = "";
            if (strWhere != "")
                strWhe = " where " + strWhere + "";

            // �����¼��
            string countString = "";
            if (Types == 0)
                countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Textcent " + strWhe + "";
            else if (Types == 1)
                countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Textcent " + strWhe + "";
            else if (Types == 2)
                countString = "SELECT COUNT(DISTINCT ToID) FROM tb_Textcent " + strWhe + "";
            else if (Types == 3)
                countString = "SELECT COUNT(DISTINCT ToID) FROM tb_Textcent " + strWhe + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                if (pageCount > 10)
                    pageCount = 10;
            }
            else
            {
                return listTextcent;
            }

            // ȡ����ؼ�¼
            string queryString = "";
            if (Types == 0)
                queryString = "SELECT UsID, Sum(Cents) as Cents FROM tb_Textcent " + strWhe + " GROUP BY UsID ORDER BY Sum(Cents) DESC";
            else if (Types == 1)
                queryString = "SELECT UsID, Count(ID) as Cents FROM tb_Textcent " + strWhe + " GROUP BY UsID ORDER BY Count(ID) DESC";
            else if (Types == 2)
                queryString = "SELECT ToID, Sum(Cents) as Cents FROM tb_Textcent " + strWhe + " GROUP BY ToID ORDER BY Sum(Cents) DESC";
            else if (Types == 3)
                queryString = "SELECT ToID, Count(ID) as Cents FROM tb_Textcent " + strWhe + " GROUP BY ToID ORDER BY Count(ID) DESC";

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Textcent objTextcent = new BCW.Model.Textcent();
                        objTextcent.UsID = reader.GetInt32(0);
                        if (Types == 1 || Types == 3)
                            objTextcent.Cents = Convert.ToInt64(reader.GetInt32(1));
                        else
                            objTextcent.Cents = reader.GetInt64(1);

                        listTextcent.Add(objTextcent);
                    }

                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listTextcent;
        }

        #endregion  ��Ա����
    }
}

