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
    /// 数据访问类ktv789。
    /// </summary>
    public class ktv789
    {
        public ktv789()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_ktv789");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ktv789");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public int Getktv789Id(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_ktv789");
            strSql.Append(" where (OneUsId=@OneUsId ");
            strSql.Append(" OR TwoUsId=@TwoUsId ");
            strSql.Append(" OR ThrUsId=@ThrUsId) ");

            SqlParameter[] parameters = {
                    new SqlParameter("@OneUsId", SqlDbType.Int,4),
					new SqlParameter("@TwoUsId", SqlDbType.Int,4),
					new SqlParameter("@ThrUsId", SqlDbType.Int,4)};
            parameters[0].Value = userId;
            parameters[1].Value = userId;
            parameters[2].Value = userId;
            return Convert.ToInt32(SqlHelper.GetSingle(strSql.ToString(), parameters));
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int ptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ktv789");
            strSql.Append(" where ID=@ID ");

            if (ptype == 1)
                strSql.Append(" and OneUsId<>0 ");
            else if (ptype == 2)
                strSql.Append(" and TwoUsId<>0 ");
            else
                strSql.Append(" and ThrUsId<>0 ");

            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int userId, int ptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_ktv789");
            strSql.Append(" where ID=@ID ");

            if (ptype == 1)
            {
                strSql.Append(" and OneUsId=@OneUsId ");
                strSql.Append(" and OneShot<>NULL ");
            }
            else if (ptype == 2)
            {
                strSql.Append(" and TwoUsId=@TwoUsId ");
                strSql.Append(" and TwoShot<>NULL ");
            }
            else
            {
                strSql.Append(" and ThrUsId=@ThrUsId ");
                strSql.Append(" and ThrShot<>NULL ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@OneUsId", SqlDbType.Int,4),
					new SqlParameter("@TwoUsId", SqlDbType.Int,4),
					new SqlParameter("@ThrUsId", SqlDbType.Int,4)};

            parameters[0].Value = ID;
            parameters[1].Value = userId;
            parameters[2].Value = userId;
            parameters[3].Value = userId;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到某项总在线数
        /// </summary>
        public int GetCount(int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT sum(Online) from tb_ktv789 where ");
            strSql.Append("Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_ktv789(");
            strSql.Append("Types,StName,PayCent,OneUsId,TwoUsId,ThrUsId,Expir,PkCount,Online,NextShot,GoldType)");
            strSql.Append(" values (");
            strSql.Append("@Types,@StName,@PayCent,@OneUsId,@TwoUsId,@ThrUsId,@Expir,@PkCount,@Online,@NextShot,@GoldType)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@StName", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.Int,4),
					new SqlParameter("@OneUsId", SqlDbType.Int,4),
					new SqlParameter("@TwoUsId", SqlDbType.Int,4),
					new SqlParameter("@ThrUsId", SqlDbType.Int,4),
					new SqlParameter("@Expir", SqlDbType.Int,4),
					new SqlParameter("@PkCount", SqlDbType.Int,4),
					new SqlParameter("@Online", SqlDbType.Int,4),
					new SqlParameter("@NextShot", SqlDbType.Int,4),
					new SqlParameter("@GoldType", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.StName;
            parameters[2].Value = model.PayCent;
            parameters[3].Value = model.OneUsId;
            parameters[4].Value = model.TwoUsId;
            parameters[5].Value = model.ThrUsId;
            parameters[6].Value = model.Expir;
            parameters[7].Value = model.PkCount;
            parameters[8].Value = model.Online;
            parameters[9].Value = model.NextShot;
            parameters[10].Value = model.GoldType;

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
        public void Update(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("Types=@Types,");
            strSql.Append("StName=@StName,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("Expir=@Expir,");
            strSql.Append("PkCount=@PkCount,");
            strSql.Append("Online=@Online,");
            strSql.Append("NextShot=@NextShot,");
            strSql.Append("GoldType=@GoldType");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@StName", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.Int,4),
					new SqlParameter("@Expir", SqlDbType.Int,4),
					new SqlParameter("@PkCount", SqlDbType.Int,4),
					new SqlParameter("@Online", SqlDbType.Int,4),
					new SqlParameter("@NextShot", SqlDbType.Int,4),
					new SqlParameter("@GoldType", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.StName;
            parameters[3].Value = model.PayCent;
            parameters[4].Value = model.Expir;
            parameters[5].Value = model.PkCount;
            parameters[6].Value = model.Online;
            parameters[7].Value = model.NextShot;
            parameters[8].Value = model.GoldType;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 坐下位置1
        /// </summary>
        public void UpdateOne(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneUsName=@OneUsName,");
            strSql.Append("OneUsId=@OneUsId,");
            strSql.Append("OneTime=@OneTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OneUsName", SqlDbType.NVarChar,50),
					new SqlParameter("@OneUsId", SqlDbType.Int,4),
            	    new SqlParameter("@OneTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OneUsName;
            parameters[2].Value = model.OneUsId;
            parameters[3].Value = model.OneTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 坐下位置2
        /// </summary>
        public void UpdateTwo(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("TwoUsName=@TwoUsName,");
            strSql.Append("TwoUsId=@TwoUsId,");
            strSql.Append("TwoTime=@TwoTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TwoUsName", SqlDbType.NVarChar,50),
					new SqlParameter("@TwoUsId", SqlDbType.Int,4),
                    new SqlParameter("@TwoTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TwoUsName;
            parameters[2].Value = model.TwoUsId;
            parameters[3].Value = model.TwoTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 坐下位置3
        /// </summary>
        public void UpdateThr(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("ThrUsName=@ThrUsName,");
            strSql.Append("ThrUsId=@ThrUsId,");
            strSql.Append("ThrTime=@ThrTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ThrUsName", SqlDbType.NVarChar,50),
					new SqlParameter("@ThrUsId", SqlDbType.Int,4),
                    new SqlParameter("@ThrTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ThrUsName;
            parameters[2].Value = model.ThrUsId;
            parameters[3].Value = model.ThrTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 出手1
        /// </summary>
        public void UpdateOneShot(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneShot=@OneShot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OneShot", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.OneShot;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 出手2
        /// </summary>
        public void UpdateTwoShot(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("TwoShot=@TwoShot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TwoShot", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TwoShot;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 出手3
        /// </summary>
        public void UpdateThrShot(BCW.Model.Game.ktv789 model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("ThrShot=@ThrShot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ThrShot", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.ThrShot;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 重置桌子
        /// </summary>
        public void UpdateShot(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneShot=NULL,");
            strSql.Append("TwoShot=NULL,");
            strSql.Append("ThrShot=NULL,");
            strSql.Append("PkCount=PkCount+1,");//增加局数
            strSql.Append("OneTime='" + DateTime.Now + "',");//更新下一局开始时间，作为计算超时时间
            strSql.Append("TwoTime='" + DateTime.Now + "',");
            strSql.Append("ThrTime='" + DateTime.Now + "'");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空桌面
        /// </summary>
        public void UpdateClear(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneShot=NULL,");
            strSql.Append("TwoShot=NULL,");
            strSql.Append("ThrShot=NULL,");
            strSql.Append("OneUsId=0,");
            strSql.Append("TwoUsId=0,");
            strSql.Append("ThrUsId=0,");
            strSql.Append("OneTime=NULL,");
            strSql.Append("TwoTime=NULL,");
            strSql.Append("ThrTime=NULL,");
            strSql.Append("OneUsName=NULL,");
            strSql.Append("TwoUsName=NULL,");
            strSql.Append("ThrUsName=NULL,");
            strSql.Append("PayCent=0,");
            strSql.Append("Online=0,");
            strSql.Append("Lines=NULL");

            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新下注额
        /// </summary>
        public void UpdatePayCent(int ID, int PayCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("PayCent=@PayCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@PayCent", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = PayCent;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新下次出手桌子
        /// </summary>
        public void UpdateNextShot(int ID, int NextShot)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("NextShot=@NextShot");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@NextShot", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NextShot;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新超时时间
        /// </summary>
        public void UpdateTime(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneTime='" + DateTime.Now + "',");//更新下一局开始时间，作为计算超时时间
            strSql.Append("TwoTime='" + DateTime.Now + "',");
            strSql.Append("ThrTime='" + DateTime.Now + "'");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 退出游戏1
        /// </summary>
        public void UpdateOneExit(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("OneUsId=0,");
            strSql.Append("OneUsName=NULL,");
            strSql.Append("OneTime=NULL,");
            strSql.Append("OneShot=NULL,");
            strSql.Append("PkCount=1");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 退出游戏2
        /// </summary>
        public void UpdateTwoExit(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("TwoUsId=0,");
            strSql.Append("TwoUsName=NULL,");
            strSql.Append("TwoTime=NULL,");
            strSql.Append("TwoShot=NULL,");
            strSql.Append("PkCount=1");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 退出游戏3
        /// </summary>
        public void UpdateThrExit(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("ThrUsId=0,");
            strSql.Append("ThrUsName=NULL,");
            strSql.Append("ThrTime=NULL,");
            strSql.Append("ThrShot=NULL,");
            strSql.Append("PkCount=1");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新轮流PK1
        /// </summary>
        public void UpdateLLone(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("NextShot=1");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and TwoUsId=0 ");
            strSql.Append(" and ThrUsId=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新轮流PK2
        /// </summary>
        public void UpdateLLtwo(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("NextShot=2");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and OneUsId=0 ");
            strSql.Append(" and ThrUsId=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新轮流PK3
        /// </summary>
        public void UpdateLLthr(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("NextShot=3");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and OneUsId=0 ");
            strSql.Append(" and TwoUsId=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新桌子在线
        /// </summary>
        public void UpdateLines(int ID, string Lines)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("Lines=@Lines");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lines", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Lines;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新桌子在线人数
        /// </summary>
        public void UpdateOnline(int ID, int Online)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_ktv789 set ");
            strSql.Append("Online=@Online");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Online", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Online;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_ktv789 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.ktv789 Getktv789(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,StName,PayCent,OneUsName,OneUsId,TwoUsName,TwoUsId,ThrUsName,ThrUsId,Expir,OneShot,TwoShot,ThrShot,OneTime,TwoTime,ThrTime,PkCount,Online,NextShot,Lines,GoldType from tb_ktv789 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.ktv789 model = new BCW.Model.Game.ktv789();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.StName = reader.GetString(2);
                    model.PayCent = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                        model.OneUsName = reader.GetString(4);
                    model.OneUsId = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                        model.TwoUsName = reader.GetString(6);
                    model.TwoUsId = reader.GetInt32(7);
                    if (!reader.IsDBNull(8))
                        model.ThrUsName = reader.GetString(8);
                    model.ThrUsId = reader.GetInt32(9);
                    model.Expir = reader.GetInt32(10);
                    if (!reader.IsDBNull(11))
                        model.OneShot = reader.GetString(11);
                    if (!reader.IsDBNull(12))
                        model.TwoShot = reader.GetString(12);
                    if (!reader.IsDBNull(13))
                        model.ThrShot = reader.GetString(13);
                    if (!reader.IsDBNull(14))
                        model.OneTime = reader.GetDateTime(14);
                    if (!reader.IsDBNull(15))
                        model.TwoTime = reader.GetDateTime(15);
                    if (!reader.IsDBNull(16))
                        model.ThrTime = reader.GetDateTime(16);
                    if (!reader.IsDBNull(17))
                        model.PkCount = reader.GetInt32(17);
                    model.Online = reader.GetInt32(18);
                    model.NextShot = reader.GetInt32(19);
                    if (!reader.IsDBNull(20))
                        model.Lines = reader.GetString(20);
                    model.GoldType = reader.GetInt32(21);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到房间StName
        /// </summary>
        public string GetStName(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select StName from tb_ktv789 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到在线Lines
        /// </summary>
        public string GetLines(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Lines from tb_ktv789 ");
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
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到类型Types
        /// </summary>
        public int GetTypes(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Types from tb_ktv789 ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
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
            strSql.Append(" FROM tb_ktv789 ");
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
        /// <returns>IList ktv789</returns>
        public IList<BCW.Model.Game.ktv789> Getktv789s(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.ktv789> listktv789s = new List<BCW.Model.Game.ktv789>();
            string sTable = "tb_ktv789";
            string sPkey = "id";
            string sField = "ID,StName,OneUsId,TwoUsId,ThrUsId,OneUsName,TwoUsName,ThrUsName,Online,Lines,GoldType";
            string sCondition = strWhere;
            string sOrder = "ID Asc";
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
                    return listktv789s;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.ktv789 objktv789 = new BCW.Model.Game.ktv789();
                    objktv789.ID = reader.GetInt32(0);
                    objktv789.StName = reader.GetString(1);
                    objktv789.OneUsId = reader.GetInt32(2);
                    objktv789.TwoUsId = reader.GetInt32(3);
                    objktv789.ThrUsId = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        objktv789.OneUsName = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        objktv789.TwoUsName = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        objktv789.ThrUsName = reader.GetString(7);
                    objktv789.Online = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        objktv789.Lines = reader.GetString(9);
                    objktv789.GoldType = reader.GetInt32(10);
                    listktv789s.Add(objktv789);
                }
            }
            return listktv789s;
        }

        #endregion  成员方法
    }
}

