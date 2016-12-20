using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Baccarat.DAL
{
    /// <summary>
    /// 数据访问类BJL_Play。
    /// </summary>
    public class BJL_Play
    {
        public BJL_Play()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_Play");
        }



        /// <summary>
        /// me_增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Play model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_Play(");
            strSql.Append("UsID,RoomID,Play_Table,PutTypes,BankerPoker,BankerPoint,HunterPoker,HunterPoint,updatetime,isRobot,Total,buy_usid,zhu_money,PutMoney,GetMoney,type,shouxufei)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@RoomID,@Play_Table,@PutTypes,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint,@updatetime,@isRobot,@Total,@buy_usid,@zhu_money,@PutMoney,@GetMoney,@type,@shouxufei)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4),
                    new SqlParameter("@PutTypes", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@isRobot", SqlDbType.Int,4),
                    new SqlParameter("@Total", SqlDbType.BigInt,8),
                    new SqlParameter("@buy_usid", SqlDbType.Int,4),
                    new SqlParameter("@zhu_money", SqlDbType.BigInt,8),
                    new SqlParameter("@PutMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@shouxufei", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.RoomID;
            parameters[2].Value = model.Play_Table;
            parameters[3].Value = model.PutTypes;
            parameters[4].Value = model.BankerPoker;
            parameters[5].Value = model.BankerPoint;
            parameters[6].Value = model.HunterPoker;
            parameters[7].Value = model.HunterPoint;
            parameters[8].Value = model.updatetime;
            parameters[9].Value = model.isRobot;
            parameters[10].Value = model.Total;
            parameters[11].Value = model.buy_usid;
            parameters[12].Value = model.zhu_money;
            parameters[13].Value = model.PutMoney;
            parameters[14].Value = model.GetMoney;
            parameters[15].Value = model.type;
            parameters[16].Value = model.shouxufei;

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
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Baccarat.Model.BJL_Play model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_Play set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("Play_Table=@Play_Table,");
            strSql.Append("PutTypes=@PutTypes,");
            strSql.Append("BankerPoker=@BankerPoker,");
            strSql.Append("BankerPoint=@BankerPoint,");
            strSql.Append("HunterPoker=@HunterPoker,");
            strSql.Append("HunterPoint=@HunterPoint,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("isRobot=@isRobot,");
            strSql.Append("Total=@Total,");
            strSql.Append("buy_usid=@buy_usid,");
            strSql.Append("zhu_money=@zhu_money,");
            strSql.Append("PutMoney=@PutMoney,");
            strSql.Append("GetMoney=@GetMoney,");
            strSql.Append("type=@type,");
            strSql.Append("shouxufei=@shouxufei");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4),
                    new SqlParameter("@PutTypes", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@isRobot", SqlDbType.Int,4),
                    new SqlParameter("@Total", SqlDbType.BigInt,8),
                    new SqlParameter("@buy_usid", SqlDbType.Int,4),
                    new SqlParameter("@zhu_money", SqlDbType.BigInt,8),
                    new SqlParameter("@PutMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@shouxufei", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.Play_Table;
            parameters[4].Value = model.PutTypes;
            parameters[5].Value = model.BankerPoker;
            parameters[6].Value = model.BankerPoint;
            parameters[7].Value = model.HunterPoker;
            parameters[8].Value = model.HunterPoint;
            parameters[9].Value = model.updatetime;
            parameters[10].Value = model.isRobot;
            parameters[11].Value = model.Total;
            parameters[12].Value = model.buy_usid;
            parameters[13].Value = model.zhu_money;
            parameters[14].Value = model.PutMoney;
            parameters[15].Value = model.GetMoney;
            parameters[16].Value = model.type;
            parameters[17].Value = model.shouxufei;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_Play ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_Play ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }



        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_BJL_Play ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList2(int strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(*) AS a FROM (SELECT DISTINCT(Play_Table) FROM tb_BJL_Play WHERE  RoomID='" + strField + "' AND BankerPoker!='' AND " + strWhere + " GROUP BY Play_Table) c");
            return SqlHelper.Query(strSql.ToString());
        }
        //---------------------------------------------

        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and buy_usid=@UsID ");
            strSql.Append(" and type=2 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + s1 + " from tb_BJL_Play " + s2 + " ");
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.bb desc");
            strSql.Append(")AS Row, T.*  from #bang3 T ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            strSql.Append("drop table #bang3");
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Play ");
            strSql.Append(" where RoomID=@ID and BankerPoker!='' ORDER BY Play_Table desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.Model.BJL_Play();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.RoomID = reader.GetInt32(2);
                    model.Play_Table = reader.GetInt32(3);
                    model.PutTypes = reader.GetString(4);
                    model.BankerPoker = reader.GetString(5);
                    model.BankerPoint = reader.GetInt32(6);
                    model.HunterPoker = reader.GetString(7);
                    model.HunterPoint = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.isRobot = reader.GetInt32(10);
                    model.Total = reader.GetInt64(11);
                    model.buy_usid = reader.GetInt32(12);
                    model.zhu_money = reader.GetInt64(13);
                    model.PutMoney = reader.GetInt64(14);
                    model.GetMoney = reader.GetInt64(15);
                    model.type = reader.GetInt32(16);
                    model.shouxufei = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.Play_Table = 0;
                    return model;
                }
            }
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play3(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Play ");
            strSql.Append(" where RoomID=@ID  ORDER BY id desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.Model.BJL_Play();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.RoomID = reader.GetInt32(2);
                    model.Play_Table = reader.GetInt32(3);
                    model.PutTypes = reader.GetString(4);
                    model.BankerPoker = reader.GetString(5);
                    model.BankerPoint = reader.GetInt32(6);
                    model.HunterPoker = reader.GetString(7);
                    model.HunterPoint = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.isRobot = reader.GetInt32(10);
                    model.Total = reader.GetInt64(11);
                    model.buy_usid = reader.GetInt32(12);
                    model.zhu_money = reader.GetInt64(13);
                    model.PutMoney = reader.GetInt64(14);
                    model.GetMoney = reader.GetInt64(15);
                    model.type = reader.GetInt32(16);
                    model.shouxufei = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.Total = 0;
                    return model;
                }
            }
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play2(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Play ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.Model.BJL_Play();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.RoomID = reader.GetInt32(2);
                    model.Play_Table = reader.GetInt32(3);
                    model.PutTypes = reader.GetString(4);
                    model.BankerPoker = reader.GetString(5);
                    model.BankerPoint = reader.GetInt32(6);
                    model.HunterPoker = reader.GetString(7);
                    model.HunterPoint = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.isRobot = reader.GetInt32(10);
                    model.Total = reader.GetInt64(11);
                    model.buy_usid = reader.GetInt32(12);
                    model.zhu_money = reader.GetInt64(13);
                    model.PutMoney = reader.GetInt64(14);
                    model.GetMoney = reader.GetInt64(15);
                    model.type = reader.GetInt32(16);
                    model.shouxufei = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }

        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where RoomID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在有下注的房间
        /// </summary>
        public bool Exists_xz(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where RoomID=@RoomID and Play_Table=@Play_Table");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
            new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_id(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在玩家在玩
        /// </summary>
        public bool Exists_wj(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where BankerPoker='' AND RoomID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///  me_是否存在该房间该局未开奖
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where BankerPoker='' AND type=0 ");
            return SqlHelper.Exists(strSql.ToString());
        }

        /// <summary>
        ///  me_是否存在该房间该局未开奖
        /// </summary>
        public bool Exists(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Play");
            strSql.Append(" where RoomID=@RoomID and Play_Table=@Play_Table and BankerPoker=''");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
            new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Play GetBJL_Play(int RoomID, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_BJL_Play ");
            strSql.Append(" where RoomID=@RoomID and UsID=@UsID ORDER BY Play_Table desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
            new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = UsID;
            BCW.Baccarat.Model.BJL_Play model = new BCW.Baccarat.Model.BJL_Play();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.RoomID = reader.GetInt32(2);
                    model.Play_Table = reader.GetInt32(3);
                    model.PutTypes = reader.GetString(4);
                    model.BankerPoker = reader.GetString(5);
                    model.BankerPoint = reader.GetInt32(6);
                    model.HunterPoker = reader.GetString(7);
                    model.HunterPoint = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.isRobot = reader.GetInt32(10);
                    model.Total = reader.GetInt64(11);
                    model.buy_usid = reader.GetInt32(12);
                    model.zhu_money = reader.GetInt64(13);
                    model.PutMoney = reader.GetInt64(14);
                    model.GetMoney = reader.GetInt64(15);
                    model.type = reader.GetInt32(16);
                    model.shouxufei = reader.GetInt32(17);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }

        /// <summary>
        ///me_得到特定房间ID和桌面table的最旧的下注时间
        /// </summary>
        public DateTime GetMinBetTime(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)updatetime from tb_BJL_Play ");
            strSql.Append("where RoomID=@RoomID and Play_Table=@Play_Table order by updatetime ASC");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }

        /// <summary>
        /// me_计算投注总币值
        /// </summary>
        public long GetPrice(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(PutMoney) from tb_BJL_Play");
            strSql.Append(" where PutMoney>0 AND RoomID=@RoomID AND Play_Table=@Play_Table ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// me_计算中奖总币值
        /// </summary>
        public long Getmoney(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(GetMoney) from tb_BJL_Play");
            strSql.Append(" where GetMoney>0 AND RoomID=@RoomID AND Play_Table=@Play_Table ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// me_计算手续费总币值
        /// </summary>
        public long Getsxf(int RoomID, int Play_Table)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(shouxufei) from tb_BJL_Play");
            strSql.Append(" where  RoomID=@RoomID AND Play_Table=@Play_Table ");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@Play_Table", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = Play_Table;
            object obj = SqlHelper.GetSingle(strSql.ToString(), parameters);
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
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_zd(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_BJL_Play SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //---------------------------------------------


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_Play</returns>
        public IList<BCW.Baccarat.Model.BJL_Play> GetBJL_Plays(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_Play> listBJL_Plays = new List<BCW.Baccarat.Model.BJL_Play>();
            string sTable = "tb_BJL_Play";
            string sPkey = "id";
            string sField = "*";
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
                    return listBJL_Plays;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_Play objBJL_Play = new BCW.Baccarat.Model.BJL_Play();
                    objBJL_Play.ID = reader.GetInt32(0);
                    objBJL_Play.UsID = reader.GetInt32(1);
                    objBJL_Play.RoomID = reader.GetInt32(2);
                    objBJL_Play.Play_Table = reader.GetInt32(3);
                    objBJL_Play.PutTypes = reader.GetString(4);
                    objBJL_Play.BankerPoker = reader.GetString(5);
                    objBJL_Play.BankerPoint = reader.GetInt32(6);
                    objBJL_Play.HunterPoker = reader.GetString(7);
                    objBJL_Play.HunterPoint = reader.GetInt32(8);
                    objBJL_Play.updatetime = reader.GetDateTime(9);
                    objBJL_Play.isRobot = reader.GetInt32(10);
                    objBJL_Play.Total = reader.GetInt64(11);
                    objBJL_Play.buy_usid = reader.GetInt32(12);
                    objBJL_Play.zhu_money = reader.GetInt64(13);
                    objBJL_Play.PutMoney = reader.GetInt64(14);
                    objBJL_Play.GetMoney = reader.GetInt64(15);
                    objBJL_Play.type = reader.GetInt32(16);
                    objBJL_Play.shouxufei = reader.GetInt32(17);
                    listBJL_Plays.Add(objBJL_Play);
                }
            }
            return listBJL_Plays;
        }

        #endregion  成员方法
    }
}

