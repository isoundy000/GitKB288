using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.BQC.DAL
{
    /// <summary>
    /// ���ݷ�����BQCPay��
    /// </summary>
    public class BQCPay
    {
        public BQCPay()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_BQCPay");
        }

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId(int usid)
        {
            return SqlHelper.GetMaxID("id", "tb_BQCPay where usID=" + usid);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay where IsPrize!=" + IsPrize);
            strSql.Append("  and CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists1(int id, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay where IsPrize=" + IsPrize);
            strSql.Append("  and id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists2(int id, int IsPrize2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay where IsPrize2>0 ");
            strSql.Append("  and id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        //me_��ѯ�����˹������
        public int GetBQCRobotCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_BQCPay ");
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
        /// �Ƿ���ڻ�����
        /// </summary>
        public bool ExistsReBot(int id, int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay");
            strSql.Append(" where id=@id and usID=@usID and IsSpier=1");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = id;
            parameters[1].Value = usID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ���ÿ����ע�ܶ�
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long PayCents(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_BQCPay");
            strSql.Append(" where CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};

            parameters[0].Value = CID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ����ID�õ�CID
        /// </summary>
        public int GetCID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CID from tb_BQCPay ");
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
                        return reader.GetInt32(0);
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
        /// ���״̬
        /// </summary>
        /// <returns></returns>
        public int getState(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_BQCPay where id=" + id + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int)};

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
        /// ���ÿ����עע��
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public int VoteNum(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_BQCPay");
            strSql.Append(" where CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};

            parameters[0].Value = CID;
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
        /// ������ܽ���
        /// </summary>
        /// <returns></returns>
        public long AllWinCentbyCID(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_BQCPay where CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8)};

            parameters[0].Value = CID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ���ÿ���н���ע��
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public int PrizeNum(int cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(case IsPrize WHEN 1 THEN Prize2Num+IsPrize WHEN 2 THEN Prize2Num  END) from tb_BQCPay");
            strSql.Append(" where CID=@CID");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = cid;
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
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (VoteNum*OverRide) from tb_BQCPay where ID=" + ID);
            SqlParameter[] parameters = {
                    new SqlParameter("@VoteNum", SqlDbType.Int,4)};

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
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum1(int ID, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (IsPrize*OverRide) from tb_BQCPay where IsPrize=1 and ID=" + ID + " and CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@IsPrize", SqlDbType.Int,4)};

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
        /// ��ȡͶע��
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum2(int ID, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (Prize2Num*OverRide) from tb_BQCPay where IsPrize2>0 and  ID=" + ID + " and CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@Prize2Num", SqlDbType.Int,4)};

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
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BQCPay");
            strSql.Append(" where id=@id ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@WinCent", SqlDbType.Int,4),
                    new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCPay set ");
            strSql.Append("State=@State");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = id;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_BQCPay ");
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
        /// �õ�һ��GetPayCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(PayCents) from tb_BQCPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where State!=0 and isSpier!=1 and  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��GetPayCentlast
        /// </summary>
        public long GetPayCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(PayCents) from tb_BQCPay  ");

            strSql.Append(" where CID=(select Top(1) CID from tb_BQCPay where State=1  Order by CID Desc) and isSpier!=1 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_BQCPay ");
            strSql.Append(" where " + strWhere + " ");
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
        /// �õ�һ��GetPayCentlast5
        /// </summary>
        public long GetPayCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(PayCents) from tb_BQCPay  ");

            strSql.Append(" where CID in (select Top(5) CID from tb_BQCPay where State=1  Order by CID Desc) and isSpier!=1 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }
        public int GetMaxCID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) * from tb_BQCList ORDER by CID DESC ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.BigInt,8)};

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
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_BQCPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where State!=0 and isSpier!=1 and   AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_BQCPay ");

            strSql.Append("where CID=(select Top(1) CID from tb_BQCList where State=1 Order by CID Desc) and isSpier!=1 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ��WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_BQCPay ");

            strSql.Append("where CID in (select Top(5) CID from tb_BQCList where State=1 Order by CID Desc) and isSpier!=1 ");

            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.Int,8)};
            parameters[0].Value = 0;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ������ܽ���
        /// </summary>
        /// <returns></returns>
        public long AllWinCent(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_BQCPay where CID=" + CID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8)};

            parameters[0].Value = CID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �������ע��
        /// </summary>
        /// <returns></returns>
        public long getAllPrice(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_BQCPay where CID=" + CID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }

            else
            {
                return Convert.ToInt64(obj);
            }
        }
        public long AllWinCentbyusID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_BQCPay where usID=" + usID + "");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8)};

            parameters[0].Value = usID;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }

            else
            {
                return Convert.ToInt64(obj);
            }
        }

        public int AllVoteNumbyusID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_BQCPay where usID=" + usID);
            SqlParameter[] parameters = {
                    new SqlParameter("@VoteNum", SqlDbType.Int,4)};

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


        public int AllVoteNum(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_BQCPay where CID=" + CID);
            SqlParameter[] parameters = {
                    new SqlParameter("@VoteNum", SqlDbType.Int,4)};

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
        /// ����һ������
        /// </summary>
        public int Add(BCW.BQC.Model.BQCPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BQCPay(");
            strSql.Append("CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,change)");
            strSql.Append(" values (");
            strSql.Append("@CID,@usID,@Vote,@VoteNum,@OverRide,@PayCent,@PayCents,@State,@WinCent,@AddTime,@IsPrize,@IsSpier,@change)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@Vote", SqlDbType.NVarChar),
                    new SqlParameter("@VoteNum", SqlDbType.Int,4),
                    new SqlParameter("@OverRide", SqlDbType.Int,4),
                    new SqlParameter("@PayCent", SqlDbType.Int,4),
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@IsPrize", SqlDbType.Int,4),
                    new SqlParameter("@IsSpier", SqlDbType.Int,4),
                    new SqlParameter("@change", SqlDbType.Int,4)};
            parameters[0].Value = model.CID;
            parameters[1].Value = model.usID;
            parameters[2].Value = model.Vote;
            parameters[3].Value = model.VoteNum;
            parameters[4].Value = model.OverRide;
            parameters[5].Value = model.PayCent;
            parameters[6].Value = model.PayCents;
            parameters[7].Value = model.State;
            parameters[8].Value = model.WinCent;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.IsPrize;
            parameters[11].Value = model.IsSpier;
            parameters[12].Value = model.change;

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
        public void Update(BCW.BQC.Model.BQCPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCPay set ");
            strSql.Append("CID=@CID,");
            strSql.Append("usID=@usID,");
            strSql.Append("Vote=@Vote,");
            strSql.Append("VoteNum=@VoteNum,");
            strSql.Append("OverRide=@OverRide,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("PayCents=@PayCents,");
            strSql.Append("State=@State,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("IsPrize=@IsPrize,");
            strSql.Append("IsSpier=@IsSpier,");
            strSql.Append("change=@change");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@Vote", SqlDbType.NVarChar),
                    new SqlParameter("@VoteNum", SqlDbType.Int,4),
                    new SqlParameter("@OverRide", SqlDbType.Int,4),
                    new SqlParameter("@PayCent", SqlDbType.Int,4),
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.Int,4),
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@IsPrize", SqlDbType.Int,4),
                    new SqlParameter("@IsSpier", SqlDbType.Int,4),
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@change", SqlDbType.Int,4)};
            parameters[0].Value = model.CID;
            parameters[1].Value = model.usID;
            parameters[2].Value = model.Vote;
            parameters[3].Value = model.VoteNum;
            parameters[4].Value = model.OverRide;
            parameters[5].Value = model.PayCent;
            parameters[6].Value = model.PayCents;
            parameters[7].Value = model.State;
            parameters[8].Value = model.WinCent;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.IsPrize;
            parameters[11].Value = model.IsSpier;
            parameters[12].Value = model.id;
            parameters[13].Value = model.change;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// ����һ������
        /// </summary>
        public void UpdateChange(int id, int i)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BQCPay set ");
            strSql.Append("change=@change+" + i + " where ");
            strSql.Append("id=" + id);
            SqlParameter[] parameters = {
                    new SqlParameter("@change", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BQCPay ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BQCPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ���1�Ƚ�ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum( IsPrize*OverRide) from tb_BQCPay where CID=@CID and IsPrize=@IsPrize");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@IsPrize",SqlDbType.Int,4)
            };
            parameters[0].Value = CID;
            parameters[1].Value = IsPrize;
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
        /// �õ���2�Ƚ�ע��
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize2(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum( Prize2Num*OverRide) from tb_BQCPay where CID=@CID and IsPrize2>0 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
            };
            parameters[0].Value = CID;
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
        /// �õ��н��ı�
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Isprize"></param>
        /// <returns></returns>
        public long Gold(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_BQCPay where CID=" + CID + " and IsPrize=" + IsPrize + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.Int,4)};
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
        /// �������ע��
        /// </summary>
        /// <returns></returns>
        public long AllPrice()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_BQCPay");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �������ע��
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_BQCPay where CID=" + CID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// ������ܽ���
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_BQCPay");
            SqlParameter[] parameters = {
                    new SqlParameter("@WinCent", SqlDbType.BigInt,8)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �������ע�����UsID
        /// </summary>
        /// <returns></returns>
        public long getAllPricebyusID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_BQCPay where usID=" + UsID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@PayCents", SqlDbType.BigInt,8)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.BQC.Model.BQCPay GetBQCPay(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,id,change,IsPrize2,Prize2Num from tb_BQCPay ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.BQC.Model.BQCPay model = new BCW.BQC.Model.BQCPay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.CID = reader.GetInt32(0);
                    model.usID = reader.GetInt32(1);
                    model.Vote = reader.GetString(2);
                    model.VoteNum = reader.GetInt32(3);
                    model.OverRide = reader.GetInt32(4);
                    model.PayCent = reader.GetInt32(5);
                    model.PayCents = reader.GetInt64(6);
                    model.State = reader.GetInt32(7);
                    model.WinCent = reader.GetInt64(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.IsPrize = reader.GetInt32(10);
                    model.IsSpier = reader.GetInt32(11);
                    model.id = reader.GetInt32(12);
                    model.change = reader.GetInt32(13);
                    model.IsPrize2 = reader.GetInt32(14);
                    model.Prize2Num = reader.GetInt32(15);
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
            strSql.Append(" FROM tb_BQCPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        public DataSet GetList(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AddTime,CID,Prize,WinPrize,usID,other from tb_BQCJackpot ");
            if (CID != 0)
            {
                strSql.Append(" where CID<=" + CID + " order by AddTime DESC");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary

        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.BQC.Model.BQCPay> listBQCPays = new List<BCW.BQC.Model.BQCPay>();
            string sTable = "tb_BQCPay";
            string sPkey = "id";
            string sField = "CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,id,change,IsPrize2,Prize2Num";
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
                    return listBQCPays;
                }
                while (reader.Read())
                {
                    BCW.BQC.Model.BQCPay objBQCPay = new BCW.BQC.Model.BQCPay();
                    objBQCPay.CID = reader.GetInt32(0);
                    objBQCPay.usID = reader.GetInt32(1);
                    objBQCPay.Vote = reader.GetString(2);
                    objBQCPay.VoteNum = reader.GetInt32(3);
                    objBQCPay.OverRide = reader.GetInt32(4);
                    objBQCPay.PayCent = reader.GetInt32(5);
                    objBQCPay.PayCents = reader.GetInt64(6);
                    objBQCPay.State = reader.GetInt32(7);
                    objBQCPay.WinCent = reader.GetInt64(8);
                    objBQCPay.AddTime = reader.GetDateTime(9);
                    objBQCPay.IsPrize = reader.GetInt32(10);
                    objBQCPay.IsSpier = reader.GetInt32(11);
                    objBQCPay.id = reader.GetInt32(12);
                    objBQCPay.change = reader.GetInt32(13);
                    objBQCPay.IsPrize2 = reader.GetInt32(14);
                    objBQCPay.Prize2Num = reader.GetInt32(15);
                    listBQCPays.Add(objBQCPay);
                }
            }
            return listBQCPays;
        }
        /// <summary>
        /// ȡ��ÿҳ��¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList BQCPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPays1(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
        {
            IList<BCW.BQC.Model.BQCPay> listBQCPays = new List<BCW.BQC.Model.BQCPay>();
            string sTable = "tb_BQCPay";
            string sPkey = "id";
            string sField = "CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,id,change,IsPrize2,Prize2Num";
            string sCondition = strWhere;
            string sOrder = strOrder;
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
                    return listBQCPays;
                }
                while (reader.Read())
                {
                    BCW.BQC.Model.BQCPay objBQCPay = new BCW.BQC.Model.BQCPay();
                    objBQCPay.CID = reader.GetInt32(0);
                    objBQCPay.usID = reader.GetInt32(1);
                    objBQCPay.Vote = reader.GetString(2);
                    objBQCPay.VoteNum = reader.GetInt32(3);
                    objBQCPay.OverRide = reader.GetInt32(4);
                    objBQCPay.PayCent = reader.GetInt32(5);
                    objBQCPay.PayCents = reader.GetInt64(6);
                    objBQCPay.State = reader.GetInt32(7);
                    objBQCPay.WinCent = reader.GetInt64(8);
                    objBQCPay.AddTime = reader.GetDateTime(9);
                    objBQCPay.IsPrize = reader.GetInt32(10);
                    objBQCPay.IsSpier = reader.GetInt32(11);
                    objBQCPay.id = reader.GetInt32(12);
                    objBQCPay.change = reader.GetInt32(13);
                    objBQCPay.IsPrize2 = reader.GetInt32(14);
                    objBQCPay.Prize2Num = reader.GetInt32(15);
                    listBQCPays.Add(objBQCPay);
                }
            }
            return listBQCPays;
        }
        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.BQC.Model.BQCPay> GetBQCPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.BQC.Model.BQCPay> listSFPayTop = new List<BCW.BQC.Model.BQCPay>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT usID) FROM tb_BQCPay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 50)
                p_recordCount = 50;

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listSFPayTop;
            }
            // ȡ����ؼ�¼
            string queryString = "";
            queryString = "SELECT Top 50 usID,sum(WinCent-PayCents) as WinCents FROM tb_BQCPay where " + strWhere + " group by usID Order by sum(WinCent-PayCents) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.BQC.Model.BQCPay objHcPay = new BCW.BQC.Model.BQCPay();
                        objHcPay.usID = reader.GetInt32(0);
                        objHcPay.WinCent = reader.GetInt64(1);
                        listSFPayTop.Add(objHcPay);
                    }
                    if (k == endIndex)
                        break;
                    k++;
                }
            }
            return listSFPayTop;
        }

        #endregion  ��Ա����
    }
}

