using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.bydr.DAL
{
    /// <summary>
    /// 数据访问类CmgToplist。
    /// </summary>
    public class CmgToplist
    {
        public CmgToplist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_CmgToplist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_CmgToplist");
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
            strSql.Append("select count(1) from tb_CmgToplist");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = meid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否vip时间
        /// </summary>
        public bool Existsusvip(int meid, string stime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Cmg_Top");
            strSql.Append(" where usID=@usID ");
            strSql.Append(" and Changj='山涧小溪' and Year(Time) = YEAR('" + stime + "') and MONTH(Time) = MONTH('" + stime + "') and DAY(Time) = DAY('" + stime + "')");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = meid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该特殊id
        /// </summary>
        public bool ExistsusID1(int meid, int sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_CmgToplist");
            strSql.Append(" where usID=@usID and sid=@sid");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@sid", SqlDbType.Int,4)};
            parameters[0].Value = meid;
            parameters[1].Value = sid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.bydr.Model.CmgToplist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_CmgToplist(");
            strSql.Append("AllcolletGold,McolletGold,DcolletGold,YcolletGold,usID,stype,Time,sid,updatetime,vit,Signtime)");
            strSql.Append(" values (");
            strSql.Append("@AllcolletGold,@McolletGold,@DcolletGold,@YcolletGold,@usID,@stype,@Time,@sid,@updatetime,@vit,@Signtime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@AllcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@McolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@DcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@YcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@stype", SqlDbType.Int,4),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@sid", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                     new SqlParameter("@vit", SqlDbType.Int,4),
                                         new SqlParameter("@Signtime", SqlDbType.DateTime)};
            parameters[0].Value = model.AllcolletGold;
            parameters[1].Value = model.McolletGold;
            parameters[2].Value = model.DcolletGold;
            parameters[3].Value = model.YcolletGold;
            parameters[4].Value = model.usID;
            parameters[5].Value = model.stype;
            parameters[6].Value = model.Time;
            parameters[7].Value = model.sid;
            parameters[8].Value = model.updatetime;
            parameters[9].Value = model.vit;
            parameters[10].Value = model.Signtime;

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
        public void Update(BCW.bydr.Model.CmgToplist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("AllcolletGold=@AllcolletGold,");
            strSql.Append("McolletGold=@McolletGold,");
            strSql.Append("DcolletGold=@DcolletGold,");
            strSql.Append("YcolletGold=@YcolletGold,");
            strSql.Append("usID=@usID,");
            strSql.Append("stype=@stype,");
            strSql.Append("Time=@Time,");
            strSql.Append("sid=@sid,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("vit=@vit,");
            strSql.Append("Signtime=@Signtime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@AllcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@McolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@DcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@YcolletGold", SqlDbType.BigInt,8),
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@stype", SqlDbType.Int,4),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@sid", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime),
                    new SqlParameter("@vit", SqlDbType.Int,4),
                    new SqlParameter("@Signtime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.AllcolletGold;
            parameters[2].Value = model.McolletGold;
            parameters[3].Value = model.DcolletGold;
            parameters[4].Value = model.YcolletGold;
            parameters[5].Value = model.usID;
            parameters[6].Value = model.stype;
            parameters[7].Value = model.Time;
            parameters[8].Value = model.sid;
            parameters[9].Value = model.updatetime;
            parameters[10].Value = model.vit;
            parameters[11].Value = model.Signtime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新总收集币
        /// </summary>
        public void UpdateAllcolletGold(int usID, long AllcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("AllcolletGold=@AllcolletGold ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@AllcolletGold", SqlDbType.Int,4)
                   };

            parameters[0].Value = usID;
            parameters[1].Value = AllcolletGold;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新等级
        /// </summary>
        public void Updatestype(int usID, int stype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("stype=@stype ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@stype", SqlDbType.Int,4)
                   };

            parameters[0].Value = usID;
            parameters[1].Value = stype;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新每日收集币
        /// </summary>
        public void UpdateDcolletGold(int usID, long DcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("DcolletGold=@DcolletGold ");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@DcolletGold", SqlDbType.Int,4)
                  };
            parameters[0].Value = usID;
            parameters[1].Value = DcolletGold;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新赞助币
        /// </summary>
        public void UpdateYcolletGold(int usID, long YcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("YcolletGold=YcolletGold+@YcolletGold ");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@YcolletGold", SqlDbType.Int,4)
                  };
            parameters[0].Value = usID;
            parameters[1].Value = YcolletGold;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 每日收集币清除
        /// </summary>
        public void UpdateDcolletGold1(long DcolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("DcolletGold=@DcolletGold ");
            SqlParameter[] parameters = {
                    new SqlParameter("@DcolletGold", SqlDbType.Int,4)
                  };
            parameters[1].Value = DcolletGold;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新每月收集币
        /// </summary>
        public void UpdateMcolletGold1(long McolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("McolletGold=@McolletGold ");

            SqlParameter[] parameters = {
                    new SqlParameter("@McolletGold", SqlDbType.Int,4)
                    };
            parameters[1].Value = McolletGold;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新每月收集币
        /// </summary>
        public void UpdateMcolletGold(int usID, long McolletGold)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("McolletGold=@McolletGold ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@McolletGold", SqlDbType.Int,4)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = McolletGold;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新字段
        /// </summary>
        public void Updatesid(int usID, int sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("sid=@sid ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@sid", SqlDbType.Int,4)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = sid;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public void Updatetime(int usID, DateTime updatetime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("updatetime=@updatetime ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@updatetime", SqlDbType.DateTime)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = updatetime;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新签到时间
        /// </summary>
        public void UpdateSigntime(int usID, DateTime Signtime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("Signtime=@Signtime ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@Signtime", SqlDbType.DateTime)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = Signtime;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新体力值
        /// </summary>
        public void Updatevit(int usID, int vit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("vit=vit+@vit ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@vit", SqlDbType.Int,4)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = vit;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新体力值
        /// </summary>
        public void Updatevit1(int usID, int vit)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("vit=@vit ");
            strSql.Append(" where usID=@usID ");

            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4),
                    new SqlParameter("@vit", SqlDbType.Int,4)
                    };
            parameters[0].Value = usID;
            parameters[1].Value = vit;


            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新字段
        /// </summary>
        public void Updatesid1(int sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_CmgToplist set ");
            strSql.Append("sid=@sid ");

            SqlParameter[] parameters = {
                    new SqlParameter("@sid", SqlDbType.Int,4)
                    };
            parameters[0].Value = sid;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_CmgToplist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgToplist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,AllcolletGold,McolletGold,DcolletGold,YcolletGold,usID,stype,Time,sid,updatetime,vit,Signtime from tb_CmgToplist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.bydr.Model.CmgToplist model = new BCW.bydr.Model.CmgToplist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.AllcolletGold = reader.GetInt64(1);
                    model.McolletGold = reader.GetInt64(2);
                    model.DcolletGold = reader.GetInt64(3);
                    model.YcolletGold = reader.GetInt64(4);
                    model.usID = reader.GetInt32(5);
                    model.stype = reader.GetInt32(6);
                    model.Time = reader.GetDateTime(7);
                    model.sid = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.vit = reader.GetInt32(10);
                    model.Signtime = reader.GetDateTime(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgToplistusID(int usID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,AllcolletGold,McolletGold,DcolletGold,YcolletGold,usID,stype,Time, sid ,updatetime,vit,Signtime from tb_CmgToplist ");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = usID;

            BCW.bydr.Model.CmgToplist model = new BCW.bydr.Model.CmgToplist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.AllcolletGold = reader.GetInt64(1);
                    model.McolletGold = reader.GetInt64(2);
                    model.DcolletGold = reader.GetInt64(3);
                    model.YcolletGold = reader.GetInt64(4);
                    model.usID = reader.GetInt32(5);
                    model.stype = reader.GetInt32(6);
                    model.Time = reader.GetDateTime(7);
                    model.sid = reader.GetInt32(8);
                    model.updatetime = reader.GetDateTime(9);
                    model.vit = reader.GetInt32(10);
                    model.Signtime = reader.GetDateTime(11);
                    return model;
                }
                else
                {
                    model.vit = 0;
                    return model;
                }
            }
        }

        /// <summary>
        /// 得到总收集币
        /// </summary>
        public BCW.bydr.Model.CmgToplist GetCmgTopAllcolletGold(int usID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AllcolletGold from tb_CmgToplist ");
            strSql.Append(" where usID=@usID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = usID;
            BCW.bydr.Model.CmgToplist model = new BCW.bydr.Model.CmgToplist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.AllcolletGold = reader.GetInt64(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到每日收集币
        /// </summary>
        public long GetCmgTopDcolletGold(int usID, string time)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select DcolletGold from tb_CmgToplist");
            strSql.Append(" where  Time BETWEEN '" + time + "" + " 00:00:00' and '" + time + "" + " 23:59:59'");
            strSql.Append(" and usID=@usID");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
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
        /// <summary>
        /// 得到每月收集币
        /// </summary>
        public long GetCmgTopMcolletGold(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select McolletGold from tb_CmgToplist");
            strSql.Append(" where datediff(month,updatetime,getdate())=0 ");
            strSql.Append(" and usID=@usID");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
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
        /// <summary>
        /// 得到体力值
        /// </summary>
        public int Getvit(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select vit from tb_CmgToplist");
            strSql.Append(" where usID=@usID ");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
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
        /// 得到签到时间
        /// </summary>
        public string GetSigntime(int usID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Signtime from tb_CmgToplist");
            strSql.Append(" where usID=@usID ");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
            SqlParameter[] parameters = {
                    new SqlParameter("@usID", SqlDbType.Int,4)};
            parameters[0].Value = usID;

            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return Convert.ToString(obj);
            }
        }
        /// <summary>
        /// 得到id
        /// </summary>

        public int Gettoplistid(int sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_CmgToplist");
            strSql.Append(" where sid=@sid ");
            SqlParameter[] parameters = {
                    new SqlParameter("@sid", SqlDbType.Int,4)};
            parameters[0].Value = sid;

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
        /// 得到全部收集币
        /// </summary>
        public long GettoplistAllcolletGoldsum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(AllcolletGold) from tb_CmgToplist");
            strSql.Append(" order by sum(AllcolletGold)");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
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
        /// 得到全部今日收集币
        /// </summary>
        public long GettoplistDcolletGoldsum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(DcolletGold) from tb_CmgToplist");
            strSql.Append(" where datediff(day,updatetime,getdate())=0 ");
            strSql.Append(" order by sum(DcolletGold)");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
            SqlParameter[] parameters = {
                    new SqlParameter("@DcolletGold", SqlDbType.Int,8)};
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
        /// 得到全部本月收集币
        /// </summary>
        public long GettoplistMcolletGoldsum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(McolletGold) from tb_CmgToplist");
            strSql.Append(" where datediff(month,updatetime,getdate())=0 ");
            strSql.Append(" order by sum(McolletGold)");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
            SqlParameter[] parameters = {
                    new SqlParameter("@McolletGold", SqlDbType.Int,8)};
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
        /// 得到全部赞助币
        /// </summary>
        public long GettoplistYcolletGoldsum()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(YcolletGold) from tb_CmgToplist");
            strSql.Append(" order by sum(YcolletGold)");
            //strSql.Append(" where AllcolletGold=@AllcolletGold ");
            SqlParameter[] parameters = {
                    new SqlParameter("@YcolletGold", SqlDbType.Int,8)};
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

        // me_初始化某数据表
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_CmgToplist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
        /// <returns>IList CmgToplist</returns>
        public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.bydr.Model.CmgToplist> listCmgToplists = new List<BCW.bydr.Model.CmgToplist>();
            string sTable = "tb_CmgToplist";
            string sPkey = "id";
            string sField = "ID,AllcolletGold,McolletGold,DcolletGold,YcolletGold,usID,stype,Time,sid,updatetime,vit,Signtime";
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
                    return listCmgToplists;
                }
                while (reader.Read())
                {
                    BCW.bydr.Model.CmgToplist objCmgToplist = new BCW.bydr.Model.CmgToplist();
                    objCmgToplist.ID = reader.GetInt32(0);
                    objCmgToplist.AllcolletGold = reader.GetInt64(1);
                    objCmgToplist.McolletGold = reader.GetInt64(2);
                    objCmgToplist.DcolletGold = reader.GetInt64(3);
                    objCmgToplist.YcolletGold = reader.GetInt64(4);
                    objCmgToplist.usID = reader.GetInt32(5);
                    objCmgToplist.stype = reader.GetInt32(6);
                    objCmgToplist.Time = reader.GetDateTime(7);
                    objCmgToplist.sid = reader.GetInt32(8);
                    objCmgToplist.updatetime = reader.GetDateTime(9);
                    objCmgToplist.vit = reader.GetInt32(10);
                    objCmgToplist.Signtime = reader.GetDateTime(11);
                    listCmgToplists.Add(objCmgToplist);
                }
            }
            return listCmgToplists;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList CmgToplist</returns>
        public IList<BCW.bydr.Model.CmgToplist> GetCmgToplists1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.bydr.Model.CmgToplist> listCmgToplists = new List<BCW.bydr.Model.CmgToplist>();
            string sTable = "tb_CmgToplist";
            string sPkey = "id";
            string sField = "ID,AllcolletGold,McolletGold,DcolletGold,YcolletGold,usID,stype,Time,sid,updatetime,vit,Signtime";
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
                    return listCmgToplists;
                }
                while (reader.Read())
                {
                    BCW.bydr.Model.CmgToplist objCmgToplist = new BCW.bydr.Model.CmgToplist();
                    objCmgToplist.ID = reader.GetInt32(0);
                    objCmgToplist.AllcolletGold = reader.GetInt64(1);
                    objCmgToplist.McolletGold = reader.GetInt64(2);
                    objCmgToplist.DcolletGold = reader.GetInt64(3);
                    objCmgToplist.YcolletGold = reader.GetInt64(4);
                    objCmgToplist.usID = reader.GetInt32(5);
                    objCmgToplist.stype = reader.GetInt32(6);
                    objCmgToplist.Time = reader.GetDateTime(7);
                    objCmgToplist.sid = reader.GetInt32(8);
                    objCmgToplist.updatetime = reader.GetDateTime(9);
                    objCmgToplist.vit = reader.GetInt32(10);
                    objCmgToplist.Signtime = reader.GetDateTime(11);
                    listCmgToplists.Add(objCmgToplist);
                }
            }
            return listCmgToplists;
        }
        #endregion  成员方法
    }
}

