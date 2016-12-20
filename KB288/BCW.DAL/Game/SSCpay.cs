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
    /// ���ݷ�����SSCpay��
    /// </summary>
    public class SSCpay
    {
        public SSCpay()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_SSCpay");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSCpay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SSCpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������������ұ���ֵ
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(Prices) from tb_SSCpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        /// �����������㷵��ֵ
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_SSCpay");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
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
        public int Add(BCW.Model.Game.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SSCpay(");
            strSql.Append("Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@SSCId,@UsID,@UsName,@Price,@iCount,@Notes,@Result,@Prices,@WinCent,@State,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.SSCId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.Result;
            parameters[8].Value = model.Prices;
            parameters[9].Value = model.WinCent;
            parameters[10].Value = model.State;
            parameters[11].Value = model.AddTime;

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
        public int Add2(BCW.Model.Game.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SSCpay(");
            strSql.Append("Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,IsSpier)");
            strSql.Append(" values (");
            strSql.Append("@Types,@SSCId,@UsID,@UsName,@Price,@iCount,@Notes,@Result,@Prices,@WinCent,@State,@AddTime,@IsSpier)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@IsSpier", SqlDbType.TinyInt,1),};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.SSCId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.iCount;
            parameters[6].Value = model.Notes;
            parameters[7].Value = model.Result;
            parameters[8].Value = model.Prices;
            parameters[9].Value = model.WinCent;
            parameters[10].Value = model.State;
            parameters[11].Value = model.AddTime;
            parameters[12].Value = model.IsSpier;

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
        public void Update(BCW.Model.Game.SSCpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("Types=@Types,");
            strSql.Append("SSCId=@SSCId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Price=@Price,");
            strSql.Append("iCount=@iCount,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Result=@Result,");
            strSql.Append("Prices=@Prices,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.BigInt,8),
					new SqlParameter("@iCount", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Prices", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.SSCId;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.iCount;
            parameters[7].Value = model.Notes;
            parameters[8].Value = model.Result;
            parameters[9].Value = model.Prices;
            parameters[10].Value = model.WinCent;
            parameters[11].Value = model.State;
            parameters[12].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID and State<>2");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ʱʱ�¿������
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("Result=@Result");
            strSql.Append(" where SSCId=@SSCId and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@SSCId", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50)};
            parameters[0].Value = SSCId;
            parameters[1].Value = Result;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������Ϸ�����ñ�
        /// </summary>
        public void UpdateWinCent(int ID, long WinCent, string WinNotes)
        {
            string oldNotes = GetWinNotes(ID) + "#";

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SSCpay set ");
            strSql.Append("WinCent=WinCent+@WinCent,");
            strSql.Append("WinNotes=@WinNotes");
            strSql.Append(" where ID=@ID and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinNotes", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;
            parameters[2].Value = oldNotes + WinNotes;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SSCpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(Prices) from tb_SSCpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and SSCId=@SSCId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@SSCId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = SSCId;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetInt64(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// �õ�һ��IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 IsSpier from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetByte(0);
                    else
                        return 0;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// �õ�һ��WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinNotes from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
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
        public BCW.Model.Game.SSCpay GetSSCpay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime,WinNotes from tb_SSCpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.SSCpay model = new BCW.Model.Game.SSCpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.SSCId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Price = reader.GetInt64(5);
                    model.iCount = reader.GetInt32(6);
                    model.Notes = reader.GetString(7);
                    model.Result = reader.GetString(8);
                    model.Prices = reader.GetInt64(9);
                    model.WinCent = reader.GetInt64(10);
                    model.State = reader.GetByte(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.WinNotes = reader.GetString(13);

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
            strSql.Append(" FROM tb_SSCpay ");
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
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.SSCpay> listSSCpays = new List<BCW.Model.Game.SSCpay>();
            string sTable = "tb_SSCpay";
            string sPkey = "id";
            string sField = "ID,Types,SSCId,UsID,UsName,Price,iCount,Notes,Result,Prices,WinCent,State,AddTime";
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
                    return listSSCpays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.SSCpay objSSCpay = new BCW.Model.Game.SSCpay();
                    objSSCpay.ID = reader.GetInt32(0);
                    objSSCpay.Types = reader.GetInt32(1);
                    objSSCpay.SSCId = reader.GetInt32(2);
                    objSSCpay.UsID = reader.GetInt32(3);
                    objSSCpay.UsName = reader.GetString(4);
                    objSSCpay.Price = reader.GetInt64(5);
                    objSSCpay.iCount = reader.GetInt32(6);
                    objSSCpay.Notes = reader.GetString(7);
                    objSSCpay.Result = reader.GetString(8);
                    objSSCpay.Prices = reader.GetInt64(9);
                    objSSCpay.WinCent = reader.GetInt64(10);
                    objSSCpay.State = reader.GetByte(11);
                    objSSCpay.AddTime = reader.GetDateTime(12);
                    listSSCpays.Add(objSSCpay);
                }
            }
            return listSSCpays;
        }

        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.SSCpay> listSSCpayTop = new List<BCW.Model.Game.SSCpay>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_SSCpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listSSCpayTop;
            }

            // ȡ����ؼ�¼
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-Prices) as WinCents FROM tb_SSCpay where " + strWhere + " group by UsID Order by sum(WinCent-Prices) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.SSCpay objSSCpay = new BCW.Model.Game.SSCpay();
                        objSSCpay.UsID = reader.GetInt32(0);
                        objSSCpay.WinCent = reader.GetInt64(1);
                        listSSCpayTop.Add(objSSCpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listSSCpayTop;
        }


        #endregion  ��Ա����
    }
}

