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
    /// ���ݷ�����Stkpay��
    /// </summary>
    public class Stkpay
    {
        public Stkpay()
        { }
        #region  ��Ա����

        /// <summary>
        /// �õ����ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Stkpay");
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ����δ����¼
        /// </summary>
        public bool ExistsState(int StkId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = StkId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڶҽ���¼
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �Ƿ���ڸü�¼
        /// </summary>
        public bool Exists(int StkId, int UsID, int bzType, int Types, decimal Odds)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and bzType=@bzType ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and Odds=@Odds ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Odds", SqlDbType.Money,8)};
            parameters[0].Value = StkId;
            parameters[1].Value = UsID;
            parameters[2].Value = bzType;
            parameters[3].Value = Types;
            parameters[4].Value = Odds;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ����ĳ���͵�Ͷע��
        /// </summary>
        public int GetCent(int StkId, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(BuyCent) from tb_Stkpay");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and bzType=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = StkId;
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
        public int Add(BCW.Model.Game.Stkpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Stkpay(");
            strSql.Append("bzType,Types,WinNum,StkId,UsID,UsName,Odds,BuyCent,WinCent,State,AddTime,isSpier)");
            strSql.Append(" values (");
            strSql.Append("@bzType,@Types,@WinNum,@StkId,@UsID,@UsName,@Odds,@BuyCent,@WinCent,@State,@AddTime,@isSpier)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Odds", SqlDbType.Money,8),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    	new SqlParameter("@isSpier", SqlDbType.Int,4)
                                        };
            parameters[0].Value = model.bzType;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.WinNum;
            parameters[3].Value = model.StkId;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.UsName;
            parameters[6].Value = model.Odds;
            parameters[7].Value = model.BuyCent;
            parameters[8].Value = model.WinCent;
            parameters[9].Value = model.State;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.isSpier;

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
        public void Update(BCW.Model.Game.Stkpay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
            strSql.Append("BuyCent=BuyCent+@BuyCent,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where StkId=@StkId ");
            strSql.Append(" and UsID=@UsID");
            strSql.Append(" and Types=@Types");
            strSql.Append(" and bzType=@bzType");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@BuyCent", SqlDbType.BigInt,8),
					new SqlParameter("@bzType", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.StkId;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.BuyCent;
            parameters[4].Value = model.bzType;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���¿���
        /// </summary>
        public void Update(int ID, long WinCent, int State, int WinNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = WinNum;
            parameters[2].Value = WinCent;
            parameters[3].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// �����û��ҽ���ʶ
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Stkpay set ");
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
            strSql.Append("delete from tb_Stkpay ");
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
            strSql.Append("delete from tb_Stkpay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_Stkpay ");
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
        /// �õ�һ��bzType
        /// </summary>
        public int GetbzType(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 bzType from tb_Stkpay ");
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
        /// �õ�һ������ʵ��
        /// </summary>
        public BCW.Model.Game.Stkpay GetStkpay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,bzType,Types,StkId,UsID,UsName,BuyCent,WinCent,State,AddTime,Odds,isSpier from tb_Stkpay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Stkpay model = new BCW.Model.Game.Stkpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.bzType = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.StkId = reader.GetInt32(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.BuyCent = reader.GetInt64(6);
                    model.WinCent = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.Odds = reader.GetDecimal(10);
                    model.isSpier = reader.GetInt32(11);
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
        public BCW.Model.Game.Stkpay GetStkpaybystkid(int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,bzType,Types,StkId,UsID,UsName,BuyCent,WinCent,State,AddTime,Odds,isSpier from tb_Stkpay ");
            strSql.Append(" where StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = StkId;

            BCW.Model.Game.Stkpay model = new BCW.Model.Game.Stkpay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.bzType = reader.GetInt32(1);
                    model.Types = reader.GetInt32(2);
                    model.StkId = reader.GetInt32(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.BuyCent = reader.GetInt64(6);
                    model.WinCent = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    model.AddTime = reader.GetDateTime(9);
                    model.Odds = reader.GetDecimal(10);
                    model.isSpier = reader.GetInt32(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// ĳ��ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = StkId;
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
        /// ĳ��ĳ�淨ĳID��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPrices(int UsID, int StkId, int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and StkId=@StkId  and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4),
                                       new SqlParameter("@Types", SqlDbType.Int,4) };
            parameters[0].Value = UsID;
            parameters[1].Value = StkId;
            parameters[2].Value = Types;
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
        /// �õ�һ��GetPayCentlast
        /// </summary>
        public long GetPayCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(BuyCent) from tb_Stkpay  ");

            strSql.Append(" where StkId=(select Top(1) ID from tb_StkList where State=1  Order by ID Desc) and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");

            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");

            strSql.Append("where StkId=(select Top(1) ID from tb_StkList where State=1 Order by ID Desc) and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  ");

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
        /// �õ�һ��GetPayCentlast5
        /// </summary>
        public long GetPayCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(BuyCent) from tb_Stkpay  ");

            strSql.Append(" where StkId in (select Top(5) ID from tb_StkList where State=1  Order by ID Desc) and isSpier!=1 and UsID in (select ID from tb_User where IsSpier!=1) ");

            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");

            strSql.Append("where StkId in (select Top(5) ID from tb_StkList where State=1 Order by ID Desc) and isSpier!=1 and UsID in (select ID from tb_User where IsSpier!=1)  ");

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
        /// �õ�һ��GetPayCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(BuyCent) from tb_Stkpay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where  State!=0 and isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@BuyCent", SqlDbType.Int,8)};
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
        /// �õ�һ��WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_Stkpay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where State!=0 and  isSpier!=1  and UsID in (select ID from tb_User where IsSpier!=1)  and  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
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
        /// ����Ͷע�ܱ�ֵ
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_Stkpay ");
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
        /// ĳ��ĳͶע��ʽ��Ͷ�˶��ٱ�
        /// </summary>
        public long GetSumPricesbytype(int Types, int StkId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  Sum(BuyCent) from tb_Stkpay ");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and StkId=@StkId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@StkId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = StkId;
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
        /// �����ֶ�ͳ���ж��������ݷ�������
        /// </summary>
        /// <param name="strWhere">ͳ������</param>
        /// <returns>ͳ�ƽ��</returns>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_Stkpay ");
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
        public bool ExistsReBot(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Stkpay");
            strSql.Append(" where ID=@ID and UsID=@UsID and isSpier=1");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// ���״̬
        /// </summary>
        /// <returns></returns>
        public int getState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_Stkpay where ID=" + ID + " ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int)};

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
            strSql.Append(" FROM tb_Stkpay ");
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
        /// <returns>IList Stkpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Stkpay> listStkpays = new List<BCW.Model.Game.Stkpay>();
            string sTable = "tb_Stkpay";
            string sPkey = "id";
            string sField = "ID,bzType,Types,WinNum,StkId,UsID,UsName,Odds,BuyCent,WinCent,State,AddTime,isSpier";
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
                    return listStkpays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Stkpay objStkpay = new BCW.Model.Game.Stkpay();
                    objStkpay.ID = reader.GetInt32(0);
                    objStkpay.bzType = reader.GetInt32(1);
                    objStkpay.Types = reader.GetInt32(2);
                    objStkpay.WinNum = reader.GetInt32(3);
                    objStkpay.StkId = reader.GetInt32(4);
                    objStkpay.UsID = reader.GetInt32(5);
                    objStkpay.UsName = reader.GetString(6);
                    objStkpay.Odds = reader.GetDecimal(7);
                    objStkpay.BuyCent = reader.GetInt64(8);
                    objStkpay.WinCent = reader.GetInt64(9);
                    objStkpay.State = reader.GetByte(10);
                    objStkpay.AddTime = reader.GetDateTime(11);
                    objStkpay.isSpier = reader.GetInt32(12);
                    listStkpays.Add(objStkpay);
                }
            }
            return listStkpays;
        }

        /// <summary>
        /// ȡ�����м�¼
        /// </summary>
        /// <param name="p_pageIndex">��ǰҳ</param>
        /// <param name="p_pageSize">��ҳ��С</param>
        /// <param name="p_recordCount">�����ܼ�¼��</param>
        /// <param name="strWhere">��ѯ����</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.Stkpay> GetStkpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Stkpay> listStkpayTop = new List<BCW.Model.Game.Stkpay>();

            // �����¼��
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Stkpay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listStkpayTop;
            }

            // ȡ����ؼ�¼
            string queryString = "";

            queryString = "SELECT UsID,sum(WinCent-BuyCent) as WinCents FROM tb_Stkpay where " + strWhere + " group by UsID Order by sum(WinCent-BuyCent) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.Stkpay objStkpay = new BCW.Model.Game.Stkpay();
                        objStkpay.UsID = reader.GetInt32(0);
                        objStkpay.WinCent = reader.GetInt64(1);
                        listStkpayTop.Add(objStkpay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listStkpayTop;
        }

        #endregion  ��Ա����
    }
}

