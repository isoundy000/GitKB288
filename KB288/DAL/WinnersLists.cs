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
    /// ���ݷ�����tb_WinnersLists��
    /// </summary>
    public class tb_WinnersLists
    {
        public tb_WinnersLists()
        { }
        #region  ��Ա����
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_WinnersLists");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool ExistsUserID(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_User");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            return SqlHelperUser.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_WinnersLists");
        }
        /// <summary>
        /// �õ�ÿ��ÿ��񽱴���
        /// </summary>
        public int GetMaxCounts(int UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(*) from tb_WinnersLists ");
            strSql.Append(" where UserId=UserID and DateDiff(dd,AddTime,getdate())=0");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.BigInt)};
            parameters[0].Value = UserID;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            int count;
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    count= reader.GetInt32(0);
                    return count;
                }
                else
                {
                    return 0;
                }
            }
        }
        
        /// <summary>
        /// �õ�һ�����ݵĴ�����isGet���ֶ�
        /// </summary>
        public BCW.Model.tb_WinnersLists GetLastIsGet(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP 1 * isGet from tb_WinnersLists ");
            strSql.Append(" where Id=@Id  order by AddTime desc");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;
            //object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            //if (obj == null)
            //{
            //    return 0;
            //}
            //else
            //{
            //    return Convert.ToInt32(obj);
            //}
            BCW.Model.tb_WinnersLists model = new BCW.Model.tb_WinnersLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt64(0);
                    model.awardId = reader.GetInt32(1);
                    model.UserId = reader.GetInt32(2);
                    model.winGold = reader.GetInt32(3);
                    model.GetId = reader.GetInt32(4);
                    model.FromId = reader.GetInt32(5);
                    model.TabelId = reader.GetInt32(6);
                    model.GameName = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
                    model.isGet = reader.GetInt32(9);
                    model.overTime = reader.GetInt32(10);
                    model.Remarks = reader.GetString(11);
                    model.Ident = reader.GetInt32(12);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// ����һ������
        /// </summary>
        public int Add(BCW.Model.tb_WinnersLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_WinnersLists(");
            strSql.Append("awardId,UserId,winGold,GetId,FromId,TabelId,GameName,AddTime,isGet,overTime,Remarks,Ident)");
            strSql.Append(" values (");
            strSql.Append("@awardId,@UserId,@winGold,@GetId,@FromId,@TabelId,@GameName,@AddTime,@isGet,@overTime,@Remarks,@Ident)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@awardId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@winGold", SqlDbType.Int,4),
					new SqlParameter("@GetId", SqlDbType.Int,4),
					new SqlParameter("@FromId", SqlDbType.Int,4),
					new SqlParameter("@TabelId", SqlDbType.Int,4),
					new SqlParameter("@GameName", SqlDbType.NChar,10),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@overTime", SqlDbType.Int,4),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,50),
					new SqlParameter("@Ident", SqlDbType.Int,4)};
            parameters[0].Value = model.awardId;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.winGold;
            parameters[3].Value = model.GetId;
            parameters[4].Value = model.FromId;
            parameters[5].Value = model.TabelId;
            parameters[6].Value = model.GameName;
            parameters[7].Value = model.AddTime;
            parameters[8].Value = model.isGet;
            parameters[9].Value = model.overTime;
            parameters[10].Value = model.Remarks;
            parameters[11].Value = model.Ident;

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
        public void Update(BCW.Model.tb_WinnersLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersLists set ");
            strSql.Append("awardId=@awardId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("winGold=@winGold,");
            strSql.Append("GetId=@GetId,");
            strSql.Append("FromId=@FromId,");
            strSql.Append("TabelId=@TabelId,");
            strSql.Append("GameName=@GameName,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("isGet=@isGet,");
            strSql.Append("overTime=@overTime,");
            strSql.Append("Remarks=@Remarks,");
            strSql.Append("Ident=@Ident");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt,8),
					new SqlParameter("@awardId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@winGold", SqlDbType.Int,4),
					new SqlParameter("@GetId", SqlDbType.Int,4),
					new SqlParameter("@FromId", SqlDbType.Int,4),
					new SqlParameter("@TabelId", SqlDbType.Int,4),
					new SqlParameter("@GameName", SqlDbType.NChar,10),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@isGet", SqlDbType.Int,4),
					new SqlParameter("@overTime", SqlDbType.Int,4),
					new SqlParameter("@Remarks", SqlDbType.NVarChar,50),
					new SqlParameter("@Ident", SqlDbType.Int,4)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.awardId;
            parameters[2].Value = model.UserId;
            parameters[3].Value = model.winGold;
            parameters[4].Value = model.GetId;
            parameters[5].Value = model.FromId;
            parameters[6].Value = model.TabelId;
            parameters[7].Value = model.GameName;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = model.isGet;
            parameters[10].Value = model.overTime;
            parameters[11].Value = model.Remarks;
            parameters[12].Value = model.Ident;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����콱��ʶ0����1δ��2����
        /// </summary>
        public void UpdateIdent(int Id,int Ident)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersLists set ");
            strSql.Append("Ident=@Ident");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt,8),
					new SqlParameter("@Ident", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            parameters[1].Value = Ident;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(long Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_WinnersLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.tb_WinnersLists Gettb_WinnersLists(long Id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Id,awardId,UserId,winGold,GetId,FromId,TabelId,GameName,AddTime,isGet,overTime,Remarks,Ident from tb_WinnersLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;

            BCW.Model.tb_WinnersLists model = new BCW.Model.tb_WinnersLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt64(0);
                    model.awardId = reader.GetInt32(1);
                    model.UserId = reader.GetInt32(2);
                    model.winGold = reader.GetInt32(3);
                    model.GetId = reader.GetInt32(4);
                    model.FromId = reader.GetInt32(5);
                    model.TabelId = reader.GetInt32(6);
                    model.GameName = reader.GetString(7);
                    model.AddTime = reader.GetDateTime(8);
                    model.isGet = reader.GetInt32(9);
                    model.overTime = reader.GetInt32(10);
                    model.Remarks = reader.GetString(11);
                    model.Ident = reader.GetInt32(12);
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
            strSql.Append(" FROM tb_WinnersLists ");
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
        /// <returns>IList tb_WinnersLists</returns>
        public IList<BCW.Model.tb_WinnersLists> Gettb_WinnersListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_WinnersLists> listtb_WinnersListss = new List<BCW.Model.tb_WinnersLists>();
            string sTable = "tb_WinnersLists";
            string sPkey = "id";
            string sField = "Id,awardId,UserId,winGold,GetId,FromId,TabelId,GameName,AddTime,isGet,overTime,Remarks,Ident";
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
                    return listtb_WinnersListss;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_WinnersLists objtb_WinnersLists = new BCW.Model.tb_WinnersLists();
                    objtb_WinnersLists.Id = reader.GetInt64(0);
                    objtb_WinnersLists.awardId = reader.GetInt32(1);
                    objtb_WinnersLists.UserId = reader.GetInt32(2);
                    objtb_WinnersLists.winGold = reader.GetInt32(3);
                    objtb_WinnersLists.GetId = reader.GetInt32(4);
                    objtb_WinnersLists.FromId = reader.GetInt32(5);
                    objtb_WinnersLists.TabelId = reader.GetInt32(6);
                    objtb_WinnersLists.GameName = reader.GetString(7);
                    objtb_WinnersLists.AddTime = reader.GetDateTime(8);
                    objtb_WinnersLists.isGet = reader.GetInt32(9);
                    objtb_WinnersLists.overTime = reader.GetInt32(10);
                    objtb_WinnersLists.Remarks = reader.GetString(11);
                    objtb_WinnersLists.Ident = reader.GetInt32(12);
                    listtb_WinnersListss.Add(objtb_WinnersLists);
                }
            }
            return listtb_WinnersListss;
        }

        #endregion  ��Ա����
    }
}

