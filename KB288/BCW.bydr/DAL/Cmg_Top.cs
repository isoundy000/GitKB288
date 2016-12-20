using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
using System.Collections;

namespace BCW.bydr.DAL
{
    /// <summary>
    /// 数据访问类Cmg_Top
    /// </summary>
    public class Cmg_Top
    {

        public Cmg_Top()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Cmg_Top");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_Top");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsusID(int meid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_Top");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = meid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该Bid
        /// </summary>
        public bool ExistsBid(int id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_Top");
            strSql.Append(" where id=@id ");
            strSql.Append(" and usID=@usID ");
            strSql.Append(" and Bid=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int, 4)};
            parameters[0].Value = id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        public bool ExistsusID1(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_Top");
            strSql.Append(" where usID=@usID ");

            strSql.Append(" and  DcolletGold!=10 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int, 4)};
            parameters[0].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.bydr.Model.Cmg_Top model)
        {
            StringBuilder strSql = new StringBuilder();
            ArrayList List = new ArrayList();
            strSql.Append("insert into tb_Cmg_Top(");
            strSql.Append("McolletGold,YcolletGold,AllcolletGold,DcolletGold,usID,Changj,ColletGold,Time,Bid,jID,randnum,randgoldnum,randdaoju,randyuID,updatetime,randten,Expiry,isrobot)");
            strSql.Append(" values (");
            strSql.Append("@McolletGold,@YcolletGold,@AllcolletGold,@DcolletGold,@usID,@Changj,@ColletGold,@Time,@Bid,@jID,@randnum,@randgoldnum,@randdaoju,@randyuID,@updatetime,@randten,@Expiry,@isrobot)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@McolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@YcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AllcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@DcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@Changj", SqlDbType.NVarChar,50),
                    new SqlParameter("@ColletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@jID", SqlDbType.Int,4),
                    new SqlParameter("@randnum", SqlDbType.Int,4),
                    new SqlParameter("@randgoldnum", SqlDbType.NVarChar,100),
                    new SqlParameter("@randdaoju", SqlDbType.NVarChar,100),
                    new SqlParameter("@randyuID", SqlDbType.NVarChar,100),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@randten", SqlDbType.NVarChar,50),
                    new SqlParameter("@Expiry",  SqlDbType.Int,4),
            new SqlParameter("@isrobot",  SqlDbType.Int,4)};
            parameters[0].Value = model.McolletGold;
            parameters[1].Value = model.YcolletGold;
            parameters[2].Value = model.AllcolletGold;
            parameters[3].Value = model.DcolletGold;
            parameters[4].Value = model.usID;
            parameters[5].Value = model.Changj;
            parameters[6].Value = model.ColletGold;
            parameters[7].Value = model.Time;
            parameters[8].Value = model.Bid;
            parameters[9].Value = model.jID;
            parameters[10].Value = model.randnum;
            parameters[11].Value = model.randgoldnum;
            parameters[12].Value = model.randdaoju;
            parameters[13].Value = model.randyuID;
            parameters[14].Value = model.updatetime;
            parameters[15].Value = model.randten;
            parameters[16].Value = model.Expiry;
            parameters[17].Value = model.isrobot;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);

            if (obj == null)
            {
                return 1;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
            List.Add(strSql.ToString());
            SqlHelper.ExecuteSqlTran(List);

        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.bydr.Model.Cmg_Top model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("McolletGold=@McolletGold,");
            strSql.Append("YcolletGold=@YcolletGold,");
            strSql.Append("AllcolletGold=@AllcolletGold,");
            strSql.Append("DcolletGold=@DcolletGold,");
            strSql.Append("usID=@usID,");
            strSql.Append("Changj=@Changj,");
            strSql.Append("ColletGold=@ColletGold,");
            strSql.Append("Time=@Time,");
            strSql.Append("Bid=@Bid,");
            strSql.Append("jID=@jID,");
            strSql.Append("randnum=@randnum,");
            strSql.Append("randgoldnum=@randgoldnum,");
            strSql.Append("randdaoju=@randdaoju,");
            strSql.Append("randyuID=@randyuID,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("randten=@randten,");
            strSql.Append("Expiry=@Expiry");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@McolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@YcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@AllcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@DcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@Changj", SqlDbType.NVarChar,50),
                    new SqlParameter("@ColletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@jID", SqlDbType.Int,4),
                    new SqlParameter("@randnum", SqlDbType.Int,4),
                    new SqlParameter("@randgoldnum", SqlDbType.NVarChar,100),
                    new SqlParameter("@randdaoju", SqlDbType.NVarChar,100),
                    new SqlParameter("@randyuID", SqlDbType.NVarChar,100),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@randten", SqlDbType.NVarChar,50),
                    new SqlParameter("@Expiry", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.McolletGold;
            parameters[2].Value = model.YcolletGold;
            parameters[3].Value = model.AllcolletGold;
            parameters[4].Value = model.DcolletGold;
            parameters[5].Value = model.usID;
            parameters[6].Value = model.Changj;
            parameters[7].Value = model.ColletGold;
            parameters[8].Value = model.Time;
            parameters[9].Value = model.Bid;
            parameters[10].Value = model.jID;
            parameters[11].Value = model.randnum;
            parameters[12].Value = model.randgoldnum;
            parameters[13].Value = model.randdaoju;
            parameters[14].Value = model.randyuID;
            parameters[15].Value = model.updatetime;
            parameters[16].Value = model.randten;
            parameters[17].Value = model.Expiry;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新Jid
        /// </summary>
        public void UpdateJid(int Bid, int jID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("jID=@jID ");
            strSql.Append(" where Bid=@Bid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@jID", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = jID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新Bid
        /// </summary>
        public void UpdateBid(int Bid, int ID, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("Bid=@Bid ");
            strSql.Append(" where ID=@ID");
            strSql.Append(" and usID=@usID");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = Bid;
            parameters[1].Value = ID;
            parameters[2].Value = usid;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }


        public void UpdateExpiry(int Expiry, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("Expiry=@Expiry ");
            strSql.Append(" where ID=@ID");

            SqlParameter[] parameters = {
                    new SqlParameter("@Expiry", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Expiry;
            parameters[1].Value = ID;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新总收集币
        /// </summary>
        public void UpdateAllcolletGold(int id, long AllcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("AllcolletGold=@AllcolletGold ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@AllcolletGold", SqlDbType.BigInt,8)};
            parameters[0].Value = id;
            parameters[1].Value = AllcolletGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新收集币
        /// </summary>
        public void UpdateColletGold(int id, long ColletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("ColletGold=@ColletGold ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@ColletGold", SqlDbType.BigInt,8)};
            parameters[0].Value = id;
            parameters[1].Value = ColletGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新玩家次数
        /// </summary>
        public void UpdateDcolletGold(int id, long DcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("DcolletGold=@DcolletGold ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@DcolletGold", SqlDbType.BigInt,8)};
            parameters[0].Value = id;
            parameters[1].Value = DcolletGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新防刷字段
        /// </summary>
        public void UpdateYcolletGold(int id, long YcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("YcolletGold=@YcolletGold ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@YcolletGold", SqlDbType.BigInt,8)};
            parameters[0].Value = id;
            parameters[1].Value = YcolletGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新每月收集币
        /// </summary>
        public void UpdateMcolletGold(int Bid, long McolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("McolletGold=@McolletGold ");
            strSql.Append(" where Bid=@Bid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bid", SqlDbType.Int,4),
                    new SqlParameter("@McolletGold", SqlDbType.BigInt,8)};
            parameters[0].Value = Bid;
            parameters[1].Value = McolletGold;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新randdaoju
        /// </summary>
        public void Updateranddaoju(string randdaoju, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("randdaoju=@randdaoju ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@randdaoju", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;
            parameters[1].Value = randdaoju;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新randten
        /// </summary>
        public void Updaterandten(string randten, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("randten=@randten ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@randten", SqlDbType.NVarChar,50)};
            parameters[0].Value = id;
            parameters[1].Value = randten;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新randyuID
        /// </summary>
        public void UpdaterandyuID(string randyuID, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("randyuID=@randyuID ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@randyuID", SqlDbType.NVarChar,100)};
            parameters[0].Value = id;
            parameters[1].Value = randyuID;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新防刷时间
        /// </summary>
        public void updatetime(int id, DateTime updatetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("updatetime=@updatetime ");
            strSql.Append(" where id=@id ");

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime)};
            parameters[0].Value = id;
            parameters[1].Value = updatetime;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public void updatetime1(int id, DateTime updatetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Cmg_Top set ");
            strSql.Append("time=@time ");
            strSql.Append(" where id=@id ");

            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.Int,4),
                    new SqlParameter("@time", SqlDbType.DateTime)};
            parameters[0].Value = id;
            parameters[1].Value = updatetime;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Cmg_Top ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_Cmg_Top ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.bydr.Model.Cmg_Top model = new BCW.bydr.Model.Cmg_Top();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.McolletGold = reader.GetInt64(1);
                    model.YcolletGold = reader.GetInt64(2);
                    model.AllcolletGold = reader.GetInt64(3);
                    model.DcolletGold = reader.GetInt64(4);
                    model.usID = reader.GetInt32(5);
                    model.Changj = reader.GetString(6);
                    model.ColletGold = reader.GetInt64(7);
                    model.Time = reader.GetDateTime(8);
                    model.Bid = reader.GetInt32(9);
                    model.jID = reader.GetInt32(10);
                    model.randnum = reader.GetInt32(11);
                    model.randgoldnum = reader.GetString(12);
                    model.randdaoju = reader.GetString(13);
                    model.randyuID = reader.GetString(14);
                    model.updatetime = reader.GetDateTime(15);
                    model.randten = reader.GetString(16);
                    model.Expiry = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    model.McolletGold = 0;
                    model.randnum = 1;
                    model.randgoldnum = "0";
                    return model;
                }
            }
        }
        /// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.bydr.Model.Cmg_Top GetCmg_Top(int ID, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,McolletGold,YcolletGold,AllcolletGold,DcolletGold,usID,Changj,ColletGold,Time,Bid,jID,randnum,randgoldnum,randdaoju,randyuID,updatetime,randten,Expiry from tb_Cmg_Top ");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            BCW.bydr.Model.Cmg_Top model = new BCW.bydr.Model.Cmg_Top();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.McolletGold = reader.GetInt64(1);
                    model.YcolletGold = reader.GetInt64(2);
                    model.AllcolletGold = reader.GetInt64(3);
                    model.DcolletGold = reader.GetInt64(4);
                    model.usID = reader.GetInt32(5);
                    model.Changj = reader.GetString(6);
                    model.ColletGold = reader.GetInt64(7);
                    model.Time = reader.GetDateTime(8);
                    model.Bid = reader.GetInt32(9);
                    model.jID = reader.GetInt32(10);
                    model.randnum = reader.GetInt32(11);
                    model.randgoldnum = reader.GetString(12);
                    model.randdaoju = reader.GetString(13);
                    model.randyuID = reader.GetString(14);
                    model.updatetime = reader.GetDateTime(15);
                    model.randten = reader.GetString(16);
                    model.Expiry = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到一个今日收集币
        /// </summary>
        public long GetCmg_AllcolletGold(int usID, string time)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append(" where  Time BETWEEN '" + time + "" + " 00:00:00' and '" + time + "" + " 23:59:59'");
            strSql.Append("and usID=@usID ");
            strSql.Append("and DcolletGold=10 ");
            //strSql.Append(" order by sum(AllcolletGold)");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};

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

        //得到今日收集币
        public long GetCmg_AllcolletGoldday(string time1, string time2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append(" where  Time BETWEEN '" + time1 + "' and '" + time2 + "'");
            strSql.Append(" and DcolletGold=10 and isrobot=0");

            SqlParameter[] parameters = {
                    new SqlParameter("@AllcolletGold", SqlDbType.Int,8)};
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
        /// 得到id玩的次数
        /// </summary>
        public int GetCmgcount(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1) from tb_Cmg_Top ");
            strSql.Append(" where jID=1 and datediff(day,Time,getdate())=0 and Changj='山涧小溪'");
            strSql.Append("and usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};

            parameters[0].Value = usID;
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
        /// 得到id玩的次数
        /// </summary>
        public int GetCmgcount1(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT count(1) FROM tb_Cmg_Top WHERE usID=@usID AND DateDiff(dd,updatetime,getdate())=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};

            parameters[0].Value = usID;
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
        //得到本月收集币
        public long GetCmg_AllcolletGoldmonth(int usID, string time1, string time2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append(" where  Time BETWEEN '" + time1 + "' and '" + time2 + "'");
            strSql.Append("and usID=@usID ");
            strSql.Append("and DcolletGold=10 ");
            //strSql.Append(" order by sum(AllcolletGold)");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};

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
        //得到本月收集币
        public long GetCmg_AllcolletGoldmonth1(string time1, string time2)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append(" where  Time BETWEEN '" + time1 + "' and '" + time2 + "'");
            strSql.Append("and DcolletGold=10 and isrobot=0");
            //strSql.Append(" order by sum(AllcolletGold)");
            SqlParameter[] parameters = {
                    new SqlParameter("@AllcolletGold", SqlDbType.Int,8)};
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
        //得到总收集币
        public long GetCmg_AllcolletGold1(int usID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append(" where usID=@usID ");
            strSql.Append("and DcolletGold=10 ");
            //strSql.Append(" order by sum(AllcolletGold)");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};

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
        //得到总收集币
        public long GetCmg_AllcolletGold2()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(AllcolletGold) from tb_Cmg_Top ");
            strSql.Append("where DcolletGold=10 and isrobot=0");

            SqlParameter[] parameters = {
                    new SqlParameter("@AllcolletGold", SqlDbType.Int,8)};
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Cmg_Top ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage(string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  usID,sum(AllcolletGold) AS'sm' from tb_Cmg_Top ");
            if (s1.Trim() != "")
            {
                strSql.Append("where Time BETWEEN '" + s1 + " ' and '" + s2 + "' ");
                strSql.Append("and DcolletGold=10 and AllcolletGold!=0 ");
            }
            else
            {
                strSql.Append("where DcolletGold=10 and AllcolletGold!=0 ");
            }

            strSql.Append(" group by usID order by sum(AllcolletGold) desc ");
            strSql.Append("select  usID,sum(AllcolletGold) AS'sm' into #bang2 from tb_Cmg_Top ");
            if (s1.Trim() != "")
            {
                strSql.Append("where Time BETWEEN '" + s1 + " ' and '" + s2 + "' ");
                strSql.Append("and DcolletGold=10 and AllcolletGold!=0 ");
            }
            else
            {
                strSql.Append("where DcolletGold=10 and AllcolletGold!=0 ");
            }
            strSql.Append(" group by usID order by sum(AllcolletGold) ");
            strSql.Append("SELECT count(sm) from #bang2 ");
            strSql.Append("drop table #bang2");
            return SqlHelper.Query(strSql.ToString());
        }



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.bydr.Model.Cmg_Top> listCmg_Tops = new List<BCW.bydr.Model.Cmg_Top>();
            string sTable = "tb_Cmg_Top";
            string sPkey = "id";
            string sField = "ID,McolletGold,YcolletGold,AllcolletGold,DcolletGold,usID,Changj,ColletGold,Time,Bid,jID,randnum,randgoldnum,randdaoju,randyuID,updatetime,randten,Expiry";
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
                    return listCmg_Tops;
                }
                while (reader.Read())
                {
                    BCW.bydr.Model.Cmg_Top objCmg_Top = new BCW.bydr.Model.Cmg_Top();
                    objCmg_Top.ID = reader.GetInt32(0);
                    objCmg_Top.McolletGold = reader.GetInt64(1);
                    objCmg_Top.YcolletGold = reader.GetInt64(2);
                    objCmg_Top.AllcolletGold = reader.GetInt64(3);
                    objCmg_Top.DcolletGold = reader.GetInt64(4);
                    objCmg_Top.usID = reader.GetInt32(5);
                    objCmg_Top.Changj = reader.GetString(6);
                    objCmg_Top.ColletGold = reader.GetInt64(7);
                    objCmg_Top.Time = reader.GetDateTime(8);
                    objCmg_Top.Bid = reader.GetInt32(9);
                    objCmg_Top.jID = reader.GetInt32(10);
                    objCmg_Top.randnum = reader.GetInt32(11);
                    objCmg_Top.randgoldnum = reader.GetString(12);
                    objCmg_Top.randdaoju = reader.GetString(13);
                    objCmg_Top.randyuID = reader.GetString(14);
                    objCmg_Top.updatetime = reader.GetDateTime(15);
                    objCmg_Top.randten = reader.GetString(16);
                    objCmg_Top.Expiry = reader.GetInt32(17);

                    listCmg_Tops.Add(objCmg_Top);
                }
            }
            return listCmg_Tops;

        }


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Cmg_Top</returns>
        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.bydr.Model.Cmg_Top> listCmg_Tops = new List<BCW.bydr.Model.Cmg_Top>();
            string sTable = "tb_Cmg_Top";
            string sPkey = "id";
            string sField = "ID,McolletGold,YcolletGold,AllcolletGold,DcolletGold,usID,Changj,ColletGold,Time,Bid,jID,randnum,randgoldnum,randdaoju,randyuID,updatetime,randten,Expiry";
            string sCondition = strWhere;
            string sOrder = "updatetime Desc";
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
                    return listCmg_Tops;
                }
                while (reader.Read())
                {
                    BCW.bydr.Model.Cmg_Top objCmg_Top = new BCW.bydr.Model.Cmg_Top();
                    objCmg_Top.ID = reader.GetInt32(0);
                    objCmg_Top.McolletGold = reader.GetInt64(1);
                    objCmg_Top.YcolletGold = reader.GetInt64(2);
                    objCmg_Top.AllcolletGold = reader.GetInt64(3);
                    objCmg_Top.DcolletGold = reader.GetInt64(4);
                    objCmg_Top.usID = reader.GetInt32(5);
                    objCmg_Top.Changj = reader.GetString(6);
                    objCmg_Top.ColletGold = reader.GetInt64(7);
                    objCmg_Top.Time = reader.GetDateTime(8);
                    objCmg_Top.Bid = reader.GetInt32(9);
                    objCmg_Top.jID = reader.GetInt32(10);
                    objCmg_Top.randnum = reader.GetInt32(11);
                    objCmg_Top.randgoldnum = reader.GetString(12);
                    objCmg_Top.randdaoju = reader.GetString(13);
                    objCmg_Top.randyuID = reader.GetString(14);
                    objCmg_Top.updatetime = reader.GetDateTime(15);
                    objCmg_Top.randten = reader.GetString(16);
                    objCmg_Top.Expiry = reader.GetInt32(17);

                    listCmg_Tops.Add(objCmg_Top);
                }
            }
            return listCmg_Tops;

        }

        public IList<BCW.bydr.Model.Cmg_Top> GetCmg_Tops1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.bydr.Model.Cmg_Top> listCmg_Tops = new List<BCW.bydr.Model.Cmg_Top>();
            string sTable = "tb_Cmg_Top";
            string sPkey = "ID";
            string sField = "ID,McolletGold,YcolletGold,AllcolletGold,DcolletGold,usID,Changj,ColletGold,Time,Bid,jID,randnum,randgoldnum,randdaoju,randyuID,updatetime,randten,Expiry";
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
                    return listCmg_Tops;
                }
                while (reader.Read())
                {
                    BCW.bydr.Model.Cmg_Top objCmg_Top = new BCW.bydr.Model.Cmg_Top();
                    objCmg_Top.ID = reader.GetInt32(0);
                    objCmg_Top.McolletGold = reader.GetInt64(1);
                    objCmg_Top.YcolletGold = reader.GetInt64(2);
                    objCmg_Top.AllcolletGold = reader.GetInt64(0);
                    objCmg_Top.DcolletGold = reader.GetInt64(4);
                    objCmg_Top.usID = reader.GetInt32(1);
                    objCmg_Top.Changj = reader.GetString(6);
                    objCmg_Top.ColletGold = reader.GetInt64(7);
                    objCmg_Top.Time = reader.GetDateTime(8);
                    objCmg_Top.Bid = reader.GetInt32(2);
                    objCmg_Top.jID = reader.GetInt32(3);
                    objCmg_Top.randnum = reader.GetInt32(9);
                    objCmg_Top.randgoldnum = reader.GetString(10);
                    objCmg_Top.randdaoju = reader.GetString(11);
                    objCmg_Top.randyuID = reader.GetString(12);
                    objCmg_Top.updatetime = reader.GetDateTime(13);
                    objCmg_Top.randten = reader.GetString(14);
                    objCmg_Top.Expiry = reader.GetInt32(15);
                    listCmg_Tops.Add(objCmg_Top);
                }
            }
            return listCmg_Tops;
        }

        #endregion  成员方法

        public BCW.bydr.Model.Cmg_Top GetCmgAllcolletGold(int ID)
        {
            throw new NotImplementedException();
        }
    }
}

