using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.SFC.DAL
{
    /// <summary>
    /// 数据访问类SfPay。
    /// </summary>
    public class SfPay
    {
        public SfPay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_SfPay");
        }
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId(int usid)
        {
            return SqlHelper.GetMaxID("id", "tb_SfPay where usID=" + usid);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int id, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay");
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
        /// 是否存在机器人
        /// </summary>
        public bool ExistsReBot(int id, int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay");
            strSql.Append(" where id=@id and usID=@usID and IsSpier=1");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)
			};
            parameters[0].Value = id;
            parameters[1].Value = usID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        //me_查询机器人购买次数
        public int GetSFCRobotCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_SfPay ");
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

        ///通过ID更新数据
        public bool RoBotByID(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfPay ");
            strSql.Append("SET IsSpier=2");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
            parameters[0].Value = id;
            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfPay set ");
            strSql.Append("State=2");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay where IsPrize!=0 ");
            strSql.Append(" and CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists1(int id, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay where IsPrize=" + IsPrize);
            strSql.Append("  and id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_SfPay where IsPrize!=" + IsPrize);
            strSql.Append("  and CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = CID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.SFC.Model.SfPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_SfPay(");
            strSql.Append("CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,change)");
            strSql.Append(" values (");
            strSql.Append("@CID,@usID,@Vote,@VoteNum,@OverRide,@PayCent,@PayCents,@State,@WinCent,@AddTime,@IsPrize,@IsSpier,@change)");
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
                    new SqlParameter("@change", SqlDbType.NVarChar,50)};

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

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.SFC.Model.SfPay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfPay set ");
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
            strSql.Append("change=@change,");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                new SqlParameter("@id", SqlDbType.Int,4),
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
                    new SqlParameter("@change", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.CID;
            parameters[2].Value = model.usID;
            parameters[3].Value = model.Vote;
            parameters[4].Value = model.VoteNum;
            parameters[5].Value = model.OverRide;
            parameters[6].Value = model.PayCent;
            parameters[7].Value = model.PayCents;
            parameters[8].Value = model.State;
            parameters[9].Value = model.WinCent;
            parameters[10].Value = model.AddTime;
            parameters[11].Value = model.IsPrize;
            parameters[12].Value = model.IsSpier;
            parameters[13].Value = model.change;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateChange(int id, string change)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfPay set ");
            strSql.Append("change=((select change from tb_SfPay where id =" + id + ")+'" + change + "') where ");
            strSql.Append("id=" + id);
            SqlParameter[] parameters = {
                    new SqlParameter("@change", SqlDbType.NVarChar,50)};
            parameters[0].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateResult(int id, string i)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_SfPay set ");
            strSql.Append("result=" + i + " where ");
            strSql.Append("id=" + id);
            SqlParameter[] parameters = {
                    new SqlParameter("@result", SqlDbType.Int,4)};
            parameters[0].Value = 0;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 得到中奖注数
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="IsPrize"></param>
        /// <returns></returns>
        public int countPrize(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_SfPay where CID=" + CID + " and IsPrize=" + IsPrize + " ");
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
        /// 得到中奖的币
        /// </summary>
        /// <param name="CID"></param>
        /// <param name="Isprize"></param>
        /// <returns></returns>
        public long Gold(int CID, int IsPrize)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_SfPay where CID=" + CID + " and IsPrize=" + IsPrize + " ");
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
        /// 获取投注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int VoteNum(int ID, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (VoteNum*OverRide) from tb_SfPay where ID=" + ID);
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
        /// 获取中单注奖注数
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public int DanVoteNum(int i, int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select (VoteNum*OverRide) from tb_SfPay where CID=" + CID + " and IsPrize=" + i + " ");
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
            strSql.Append("select sum(VoteNum*OverRide) from tb_SfPay where CID=" + CID);
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


        public int AllVoteNumbyusID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_SfPay where usID=" + usID);
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
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SfPay ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_SfPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 获得总下注额
        /// </summary>
        /// <returns></returns>
        public long AllPrice()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_SfPay");
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
        /// 获得总下注额
        /// </summary>
        /// <returns></returns>
        public long AllPrice(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_SfPay where CID=" + CID + " ");
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

        public int GetMaxCID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1) * from tb_SfList ORDER by CID DESC ");
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
        /// 获得总下注额
        /// </summary>
        /// <returns></returns>
        public long getAllPrice(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_SfPay where CID=" + CID + " ");
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
        /// 获得状态
        /// </summary>
        /// <returns></returns>
        public int getState(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select State from tb_SfPay where id=" + id + " ");
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
        /// 获得总下注额根据UsID
        /// </summary>
        /// <returns></returns>
        public long getAllPricebyusID(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_SfPay where usID=" + UsID + " ");
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
        /// 获得总总奖额
        /// </summary>
        /// <returns></returns>
        public long AllWinCent()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_SfPay");
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
        /// 获得总总奖额
        /// </summary>
        /// <returns></returns>
        public long AllWinCentbyCID(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_SfPay where CID=" + CID + "");
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


        public long AllWinCentbyusID(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(WinCent) from tb_SfPay where usID=" + usID + "");
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

        /// <summary>
        /// 获得每期下注总额
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public long PayCents(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(PayCents) from tb_SfPay");
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
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_SfPay ");
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
        /// 根据ID得到CID
        /// </summary>
        public int GetCID(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CID from tb_SfPay ");
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
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_SfPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where State!=0 and  isSpier!=1 and  AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
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
        /// 得到一个GetPayCent
        /// </summary>
        public long GetPayCent(string time1, string time2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(PayCents) from tb_SfPay ");
            if (time1.Trim() != "")
            {
                strSql.Append(" where  State!=0 and isSpier!=1 and AddTime BETWEEN '" + time1 + "' and '" + time2 + "'");
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
        /// 得到一个GetPayCentlast
        /// </summary>
        public long GetPayCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(PayCents) from tb_SfPay  ");

            strSql.Append(" where CID=(select Top(1) CID from tb_SfPay where State=1  Order by CID Desc) and isSpier!=1 ");

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
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT " + ziduan + " from tb_SfPay ");
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
        /// 得到一个WinCentlast
        /// </summary>
        public long GetWinCentlast()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_SfPay ");

            strSql.Append("where CID=(select Top(1) CID from tb_SfPay where State=1 Order by CID Desc) and isSpier!=1 ");

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
        /// 得到一个WinCentlast5
        /// </summary>
        public long GetWinCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(WinCent) from tb_SfPay ");

            strSql.Append("where CID in (select Top(5) CID from tb_SfPay where State=1 Order by CID Desc) and isSpier!=1 ");

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
        /// 得到一个GetPayCentlast5
        /// </summary>
        public long GetPayCentlast5()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select sum(PayCents) from tb_SfPay  ");

            strSql.Append(" where CID in (select Top(5) CID from tb_SfPay where State=1  Order by CID Desc) and isSpier!=1 ");

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
        /// 获得每期下注注数
        /// </summary>
        /// <param name="CID"></param>
        /// <returns></returns>
        public int VoteNum(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(VoteNum*OverRide) from tb_SfPay");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.SFC.Model.SfPay GetSfPay(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier from tb_SfPay ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.SFC.Model.SfPay model = new BCW.SFC.Model.SfPay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.CID = reader.GetInt32(1);
                    model.usID = reader.GetInt32(2);
                    model.Vote = reader.GetString(3);
                    model.VoteNum = reader.GetInt32(4);
                    model.OverRide = reader.GetInt32(5);
                    model.PayCent = reader.GetInt32(6);
                    model.PayCents = reader.GetInt64(7);
                    model.State = reader.GetInt32(8);
                    model.WinCent = reader.GetInt64(9);
                    model.AddTime = reader.GetDateTime(10);
                    model.IsPrize = reader.GetInt32(11);
                    model.IsSpier = reader.GetInt32(12);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_SfPay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList1(string strField)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY " + strField + " ASC) AS rowNum, * FROM tb_SfList where State=0 ) AS t WHERE rowNum=2 ");

            return SqlHelper.Query(strSql.ToString());
        }
        public DataSet GetList(int CID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AddTime,CID,Prize,WinPrize,usID,other from tb_SfJackpot ");
            if (CID != 0)
            {
                strSql.Append(" where CID<=" + CID + " order by CID desc ,AddTime desc,id desc");
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.SFC.Model.SfPay> listSfPays = new List<BCW.SFC.Model.SfPay>();
            string sTable = "tb_SfPay";
            string sPkey = "id";
            string sField = "id,CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,change";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listSfPays;
                }
                while (reader.Read())
                {
                    BCW.SFC.Model.SfPay objSfPay = new BCW.SFC.Model.SfPay();
                    objSfPay.id = reader.GetInt32(0);
                    objSfPay.CID = reader.GetInt32(1);
                    objSfPay.usID = reader.GetInt32(2);
                    objSfPay.Vote = reader.GetString(3);
                    objSfPay.VoteNum = reader.GetInt32(4);
                    objSfPay.OverRide = reader.GetInt32(5);
                    objSfPay.PayCent = reader.GetInt32(6);
                    objSfPay.PayCents = reader.GetInt64(7);
                    objSfPay.State = reader.GetInt32(8);
                    objSfPay.WinCent = reader.GetInt64(9);
                    objSfPay.AddTime = reader.GetDateTime(10);
                    objSfPay.IsPrize = reader.GetInt32(11);
                    objSfPay.IsSpier = reader.GetInt32(12);
                    objSfPay.change = reader.GetString(13);
                    listSfPays.Add(objSfPay);
                }
            }
            return listSfPays;
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SfPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSfPays1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.SFC.Model.SfPay> listSfPays = new List<BCW.SFC.Model.SfPay>();
            string sTable = "tb_SfPay";
            string sPkey = "id";
            string sField = "id,CID,usID,Vote,VoteNum,OverRide,PayCent,PayCents,State,WinCent,AddTime,IsPrize,IsSpier,change";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listSfPays;
                }
                while (reader.Read())
                {
                    BCW.SFC.Model.SfPay objSfPay = new BCW.SFC.Model.SfPay();
                    objSfPay.id = reader.GetInt32(0);
                    objSfPay.CID = reader.GetInt32(1);
                    objSfPay.usID = reader.GetInt32(2);
                    objSfPay.Vote = reader.GetString(3);
                    objSfPay.VoteNum = reader.GetInt32(4);
                    objSfPay.OverRide = reader.GetInt32(5);
                    objSfPay.PayCent = reader.GetInt32(6);
                    objSfPay.PayCents = reader.GetInt64(7);
                    objSfPay.State = reader.GetInt32(8);
                    objSfPay.WinCent = reader.GetInt64(9);
                    objSfPay.AddTime = reader.GetDateTime(10);
                    objSfPay.IsPrize = reader.GetInt32(11);
                    objSfPay.IsSpier = reader.GetInt32(12);
                    objSfPay.change = reader.GetString(13);
                    listSfPays.Add(objSfPay);
                }
            }
            return listSfPays;
        }
        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.SFC.Model.SfPay> GetSFPaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.SFC.Model.SfPay> listSFPayTop = new List<BCW.SFC.Model.SfPay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT usID) FROM tb_SFPay where " + strWhere + "";

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
            // 取出相关记录
            string queryString = "";
            queryString = "SELECT Top 50 usID,sum(WinCent-PayCents) as WinCents FROM tb_SFPay where " + strWhere + " group by usID Order by sum(WinCent-PayCents) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.SFC.Model.SfPay objHcPay = new BCW.SFC.Model.SfPay();
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
        #endregion  成员方法
    }
}

