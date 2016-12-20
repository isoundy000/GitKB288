using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

/// <summary>
/// =================================
/// �޸����а�����
/// �ƹ��� 20160615
/// =================================
/// </summary>
namespace LHC.DAL
{
    /// <summary>
    /// ���ݷ�����VotePay49��
    /// </summary>
    public class VotePay49
    {
        public VotePay49()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_VotePay49");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_VotePay49");
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
            strSql.Append("select count(1) from tb_VotePay49");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and winCent>@winCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@winCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ÿIDÿ����ע����
        /// </summary>
        public long GetPayCent(int UsID, int qiNo, int bzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay49");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = qiNo;
            parameters[2].Value = bzType;
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
        /// ÿ���н�����
        /// </summary>
        public long GetwinCent(int qiNo, int bzType)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(winCent) from tb_VotePay49");
            strSql.Append(" where winCent>@winCent ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType ");
            SqlParameter[] parameters = {
					new SqlParameter("@winCent", SqlDbType.BigInt,8),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = 0;
            parameters[1].Value = qiNo;
            parameters[2].Value = bzType;
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
        /// ÿ��.��ÿ����ע����
        /// </summary>
        public long GetPayCent(int qiNo, int sNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay49");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and qiNo=@qiNo ");
            strSql.Append(" and BzType=@BzType  and ','+Vote+',' Like '%," + sNum + ",%'");

            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1)};
            parameters[0].Value = 1;
            parameters[1].Value = qiNo;
            parameters[2].Value = 0;
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
        /// ���������õ���ע����
        /// </summary>
        public long GetPayCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(payCent) from tb_VotePay49");
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
                return Convert.ToInt32(obj);
            }
        }

        /// <summary>
        /// ���������õ�Ӯ������
        /// </summary>
        public long GetwinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(winCent) from tb_VotePay49");
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
        public int Add(LHC.Model.VotePay49 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_VotePay49(");
            strSql.Append("Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType,payNum)");
            strSql.Append(" values (");
            strSql.Append("@Types,@qiNo,@UsID,@UsName,@Vote,@payCent,@winCent,@State,@AddTime,@BzType,@payNum)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Vote", SqlDbType.NVarChar,200),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@winCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
                    new SqlParameter("@payNum", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.qiNo;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Vote;
            parameters[5].Value = model.payCent;
            parameters[6].Value = model.winCent;
            parameters[7].Value = model.State;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.BzType;
            parameters[10].Value = model.PayNum;
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
        public void Update(LHC.Model.VotePay49 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay49 set ");
            strSql.Append("Types=@Types,");
            strSql.Append("qiNo=@qiNo,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Vote=@Vote,");
            strSql.Append("payCent=@payCent,");
            strSql.Append("winCent=@winCent,");
            strSql.Append("State=@State,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("payNum=@payNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@qiNo", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Vote", SqlDbType.NVarChar,200),
                    new SqlParameter("@payCent", SqlDbType.BigInt,8),
                    new SqlParameter("@winCent", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.TinyInt,1),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@BzType", SqlDbType.TinyInt,1),
                    new SqlParameter("@payNum", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.qiNo;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.Vote;
            parameters[6].Value = model.payCent;
            parameters[7].Value = model.winCent;
            parameters[8].Value = model.State;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.BzType;
            parameters[11].Value = model.PayNum;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ���¸���Ϊ����
        /// </summary>
        public void UpdateOver(int qiNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay49 set ");
            strSql.Append("State=@State");
            strSql.Append(" where qiNo=@qiNo and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@qiNo", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = qiNo;
            parameters[1].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_VotePay49 set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_VotePay49 ");
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
            strSql.Append("delete from tb_VotePay49 ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public LHC.Model.VotePay49 GetVotePay49(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType,payNum from tb_VotePay49 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            LHC.Model.VotePay49 model = new LHC.Model.VotePay49();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.qiNo = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Vote = reader.GetString(5);
                    model.payCent = reader.GetInt64(6);
                    model.winCent = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    //reader.GetInt32(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.BzType = reader.GetByte(10);
                    model.PayNum = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 winCent from tb_VotePay49 ");
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
        /// �õ�һ��BzType
        /// </summary>
        public int GetBzType(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BzType from tb_VotePay49 ");
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
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_VotePay49 ");
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
        /// <returns>IList VotePay49</returns>
        public IList<LHC.Model.VotePay49> GetVotePay49s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<LHC.Model.VotePay49> listVotePay49s = new List<LHC.Model.VotePay49>();
            string sTable = "tb_VotePay49";
            string sPkey = "id";
            string sField = "ID,Types,qiNo,UsID,UsName,Vote,payCent,winCent,State,AddTime,BzType";
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
                    return listVotePay49s;
                }
                while (reader.Read())
                {
                    LHC.Model.VotePay49 objVotePay49 = new LHC.Model.VotePay49();
                    objVotePay49.ID = reader.GetInt32(0);
                    objVotePay49.Types = reader.GetInt32(1);
                    objVotePay49.qiNo = reader.GetInt32(2);
                    objVotePay49.UsID = reader.GetInt32(3);
                    objVotePay49.UsName = reader.GetString(4);
                    objVotePay49.Vote = reader.GetString(5);
                    objVotePay49.payCent = reader.GetInt64(6);
                    objVotePay49.winCent = reader.GetInt64(7);
                    objVotePay49.State = reader.GetByte(8);
                    objVotePay49.AddTime = reader.GetDateTime(9);
                    objVotePay49.BzType = reader.GetByte(10);
                    listVotePay49s.Add(objVotePay49);
                }
            }
            return listVotePay49s;
        }

        /// <summary>
        /// ȡ��ÿҳ��¼ Ӯ�����а�
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList VotePay49</returns>
        public IList<LHC.Model.VotePay49> GetVotePay49s_px(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<LHC.Model.VotePay49> listVotePay49s = new List<LHC.Model.VotePay49>();
            // ȡ����ؼ�¼
            p_recordCount = 0;
            string queryString = "SELECT SUM(winCent - (payCent * payNum)) AS px, UsID FROM tb_VotePay49 " + strWhere + " GROUP BY UsID ORDER BY px DESC";
            DataSet ds = SqlHelper.Query(queryString);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    p_recordCount = ds.Tables[0].Rows.Count;
                }
            }
            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listVotePay49s;
            }

            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;
                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        LHC.Model.VotePay49 objPay49 = new LHC.Model.VotePay49();
                        objPay49.winCent = reader.GetInt64(0);
                        objPay49.UsID = reader.GetInt32(1);
                        listVotePay49s.Add(objPay49);
                    }

                    if (k == endIndex)
                        break;
                    k++;
                }
            }

            return listVotePay49s;
        }

        #endregion  ��Ա����
    }
}

