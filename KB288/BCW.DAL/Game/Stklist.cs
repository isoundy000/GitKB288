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
    /// ���ݷ�����Stklist��
    /// </summary>
    public class Stklist
    {
        public Stklist()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Stklist");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stklist");
            strSql.Append(" where State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,4)};
            parameters[0].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stklist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(DateTime EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stklist");
            strSql.Append(" where Year(EndTime)=" + EndTime.Year + " AND Month(EndTime) = " + EndTime.Month + " and Day(EndTime) = " + EndTime.Day + " ");
 
            return SqlHelper.Exists(strSql.ToString());
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Stklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Stklist(");
            strSql.Append("WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,State)");
            strSql.Append(" values (");
            strSql.Append("@WinNum,@BeginTime,@EndTime,@Pool,@WinCount,@WinPool,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@WinPool", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.WinNum;
            parameters[1].Value = model.BeginTime;
            parameters[2].Value = model.EndTime;
            parameters[3].Value = model.Pool;
            parameters[4].Value = model.WinCount;
            parameters[5].Value = model.WinPool;
            parameters[6].Value = model.State;

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
        public void Update(BCW.Model.Game.Stklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stklist set ");
            strSql.Append("BeginTime=@BeginTime,");
            strSql.Append("EndTime=@EndTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BeginTime;
            parameters[2].Value = model.EndTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update2(BCW.Model.Game.Stklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stklist set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("WinCount=@WinCount,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinNum;
            parameters[2].Value = model.WinCount;
            parameters[3].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������Ѻע���
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stklist set ");
            strSql.Append("Pool=Pool+@Pool");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Pool", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = Pool;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ������Ѻע���2
        /// </summary>
        public void UpdateWinPool(int ID, long WinPool)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stklist set ");
            strSql.Append("WinPool=WinPool+@WinPool");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinPool", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = WinPool;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Stklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public DateTime GetEndTime(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 EndTime from tb_Stklist ");
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
                        return reader.GetDateTime(0);
                    else
                        return DateTime.Now;
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Stklist GetStklist()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,State from tb_Stklist ");
            strSql.Append("Where State=0 Order By ID DESC ");

            BCW.Model.Game.Stklist model = new BCW.Model.Game.Stklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.BeginTime = reader.GetDateTime(2);
                    model.EndTime = reader.GetDateTime(3);
                    model.Pool = reader.GetInt64(4);
                    model.WinCount = reader.GetInt32(5);
                    model.WinPool = reader.GetInt64(6);
                    model.State = reader.GetByte(7);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.WinNum = 0;
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now;
                    model.Pool = 0;
                    model.WinCount = 0;
                    model.WinPool = 0;
                    model.State = 0;
                    return model;
                }
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Stklist GetStklist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,State from tb_Stklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Stklist model = new BCW.Model.Game.Stklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.BeginTime = reader.GetDateTime(2);
                    model.EndTime = reader.GetDateTime(3);
                    model.Pool = reader.GetInt64(4);
                    model.WinCount = reader.GetInt32(5);
                    model.WinPool = reader.GetInt64(6);
                    model.State = reader.GetByte(7);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        public int GetIDbyDate(string stk)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_Stklist");
            strSql.Append(" where EndTime = '"+stk+"' ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.TinyInt,4)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �����ֶ�ȡ�����б�
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Stklist ");
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
        /// <returns>IList Stklist</returns>
        public IList<BCW.Model.Game.Stklist> GetStklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Stklist> listStklists = new List<BCW.Model.Game.Stklist>();
            string sTable = "tb_Stklist";
            string sPkey = "id";
            string sField = "ID,WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,State";
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
                    return listStklists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Stklist objStklist = new BCW.Model.Game.Stklist();
                    objStklist.ID = reader.GetInt32(0);
                    objStklist.WinNum = reader.GetInt32(1);
                    objStklist.BeginTime = reader.GetDateTime(2);
                    objStklist.EndTime = reader.GetDateTime(3);
                    objStklist.Pool = reader.GetInt64(4);
                    objStklist.WinCount = reader.GetInt32(5);
                    objStklist.WinPool = reader.GetInt64(6);
                    objStklist.State = reader.GetByte(7);
                    listStklists.Add(objStklist);
                }
            }
            return listStklists;
        }

        #endregion  ��Ա����
    }
}

