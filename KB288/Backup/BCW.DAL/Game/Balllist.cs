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
    /// ���ݷ�����Balllist��
    /// </summary>
    public class Balllist
    {
        public Balllist()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Balllist");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Balllist");
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
            strSql.Append("select count(1) from tb_Balllist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.Game.Balllist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Balllist(");
            strSql.Append("WinNum,OutNum,AddNum,iCent,Odds,BeginTime,EndTime,Pool,BeforePool,State)");
            strSql.Append(" values (");
            strSql.Append("@WinNum,@OutNum,@AddNum,@iCent,@Odds,@BeginTime,@EndTime,@Pool,@BeforePool,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@OutNum", SqlDbType.Int,4),
					new SqlParameter("@AddNum", SqlDbType.Int,4),
					new SqlParameter("@iCent", SqlDbType.Int,4),
					new SqlParameter("@Odds", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@BeforePool", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.WinNum;
            parameters[1].Value = model.OutNum;
            parameters[2].Value = model.AddNum;
            parameters[3].Value = model.iCent;
            parameters[4].Value = model.Odds;
            parameters[5].Value = model.BeginTime;
            parameters[6].Value = model.EndTime;
            parameters[7].Value = model.Pool;
            parameters[8].Value = model.BeforePool;
            parameters[9].Value = model.State;

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
        public void Update(BCW.Model.Game.Balllist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Balllist set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("OutNum=@OutNum,");
            strSql.Append("AddNum=@AddNum,");
            strSql.Append("iCent=@iCent,");
            strSql.Append("Odds=@Odds,");
            strSql.Append("BeginTime=@BeginTime,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("Pool=@Pool,");
            strSql.Append("BeforePool=@BeforePool");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@OutNum", SqlDbType.Int,4),
					new SqlParameter("@AddNum", SqlDbType.Int,4),
					new SqlParameter("@iCent", SqlDbType.Int,4),
					new SqlParameter("@Odds", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@BeforePool", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinNum;
            parameters[2].Value = model.OutNum;
            parameters[3].Value = model.AddNum;
            parameters[4].Value = model.iCent;
            parameters[5].Value = model.Odds;
            parameters[6].Value = model.BeginTime;
            parameters[7].Value = model.EndTime;
            parameters[8].Value = model.Pool;
            parameters[9].Value = model.BeforePool;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���±��ڼ�¼
        /// </summary>
        public void Update(int ID, int WinNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Balllist set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = WinNum;
            parameters[2].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���½��ػ���͹�����
        /// </summary>
        public void UpdatePool(int ID, long Pool, int AddNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Balllist set ");
            strSql.Append("Pool=Pool+@Pool,");
            strSql.Append("AddNum=AddNum+@AddNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@AddNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Pool;
            parameters[2].Value = AddNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Balllist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Balllist GetBalllist()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,OutNum,AddNum,iCent,Odds,BeginTime,EndTime,Pool,BeforePool,State from tb_Balllist ");
            strSql.Append(" Order by ID DESC");

            BCW.Model.Game.Balllist model = new BCW.Model.Game.Balllist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.OutNum = reader.GetInt32(2);
                    model.AddNum = reader.GetInt32(3);
                    model.iCent = reader.GetInt32(4);
                    model.Odds = reader.GetInt32(5);
                    model.BeginTime = reader.GetDateTime(6);
                    model.EndTime = reader.GetDateTime(7);
                    model.Pool = reader.GetInt64(8);
                    model.BeforePool = reader.GetInt64(9);
                    model.State = reader.GetByte(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.WinNum = 0;
                    model.OutNum = 0;
                    model.iCent = 0;
                    model.Odds = 0;
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.State = 0;
                    return model;
                }
            }
        }

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Balllist GetBalllist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,OutNum,AddNum,iCent,Odds,BeginTime,EndTime,Pool,BeforePool,State from tb_Balllist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Balllist model = new BCW.Model.Game.Balllist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.OutNum = reader.GetInt32(2);
                    model.AddNum = reader.GetInt32(3);
                    model.iCent = reader.GetInt32(4);
                    model.Odds = reader.GetInt32(5);
                    model.BeginTime = reader.GetDateTime(6);
                    model.EndTime = reader.GetDateTime(7);
                    model.Pool = reader.GetInt64(8);
                    model.BeforePool = reader.GetInt64(9);
                    model.State = reader.GetByte(10);
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
            strSql.Append(" FROM tb_Balllist ");
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
        /// <returns>IList Balllist</returns>
        public IList<BCW.Model.Game.Balllist> GetBalllists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Balllist> listBalllists = new List<BCW.Model.Game.Balllist>();
            string sTable = "tb_Balllist";
            string sPkey = "id";
            string sField = "ID,WinNum";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //������ҳ��
                if (p_recordCount == 0)
                {
                    return listBalllists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Balllist objBalllist = new BCW.Model.Game.Balllist();
                    objBalllist.ID = reader.GetInt32(0);
                    objBalllist.WinNum = reader.GetInt32(1);
                    listBalllists.Add(objBalllist);
                }
            }
            return listBalllists;
        }

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList Balllist</returns>
        public IList<BCW.Model.Game.Balllist> GetBalllists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Balllist> listBalllists = new List<BCW.Model.Game.Balllist>();
            string sTable = "tb_Balllist";
            string sPkey = "id";
            string sField = "ID,WinNum,OutNum,AddNum,iCent,Odds,BeginTime,EndTime,Pool,BeforePool,State";
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
                    return listBalllists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Balllist objBalllist = new BCW.Model.Game.Balllist();
                    objBalllist.ID = reader.GetInt32(0);
                    objBalllist.WinNum = reader.GetInt32(1);
                    objBalllist.OutNum = reader.GetInt32(2);
                    objBalllist.AddNum = reader.GetInt32(3);
                    objBalllist.iCent = reader.GetInt32(4);
                    objBalllist.Odds = reader.GetInt32(5);
                    objBalllist.BeginTime = reader.GetDateTime(6);
                    objBalllist.EndTime = reader.GetDateTime(7);
                    objBalllist.Pool = reader.GetInt64(8);
                    objBalllist.BeforePool = reader.GetInt64(9);
                    objBalllist.State = reader.GetByte(10);
                    listBalllists.Add(objBalllist);
                }
            }
            return listBalllists;
        }

        #endregion  ��Ա����
    }
}

