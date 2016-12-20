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
    /// 数据访问类Lucklist。
    /// </summary>
    public class Lucklist
    {
        public Lucklist()
        { }
        #region  成员方法

        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Lucklist");
        }
        /// <summary>
        /// 统计数据
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select COUNT(*)   from tb_Lucklist ");
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
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
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,4)};
            parameters[0].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在的期号
        /// </summary>
        public bool ExistsBJQH(int Bjkl8Qihao)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where Bjkl8Qihao=@Bjkl8Qihao");
            SqlParameter[] parameters = {
					new SqlParameter("@Bjkl8Qihao", SqlDbType.Int,4)};
            parameters[0].Value = Bjkl8Qihao;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该时间段的期号
        /// </summary>
        public bool ExistsEndTime(string EndTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where EndTime=@EndTime");
            SqlParameter[] parameters = {
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = EndTime;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existspanduan(string panduan)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where panduan=@panduan");
            SqlParameter[] parameters = {
					new SqlParameter("@panduan", SqlDbType.Int,4)};
            parameters[0].Value = panduan;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该抓取的期号
        /// </summary>
        public bool ExistsQihao(string Qihao)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Lucklist");
            strSql.Append(" where Qihao=@Qihao ");
            SqlParameter[] parameters = {
					new SqlParameter("@Qihao",SqlDbType.NVarChar,50)};
            parameters[0].Value = Qihao;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Lucklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Lucklist(");
            strSql.Append("SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao)");
            strSql.Append(" values (");
            strSql.Append("@SumNum,@PostNum,@BeginTime,@EndTime,@LuckCent,@Pool,@BeforePool,@State,@panduan,@Bjkl8Qihao)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@SumNum", SqlDbType.Int,4),
					new SqlParameter("@PostNum", SqlDbType.NVarChar,50),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@LuckCent", SqlDbType.BigInt,8),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@BeforePool", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),             
                    new SqlParameter("@panduan", SqlDbType.NVarChar,50),
                    new SqlParameter("@Bjkl8Qihao", SqlDbType.Int,4)};
            parameters[0].Value = model.SumNum;
            parameters[1].Value = model.PostNum;
            parameters[2].Value = model.BeginTime;
            parameters[3].Value = model.EndTime;
            parameters[4].Value = model.LuckCent;
            parameters[5].Value = model.Pool;
            parameters[6].Value = model.BeforePool;
            parameters[7].Value = model.State;
            parameters[8].Value = model.panduan;
            parameters[9].Value = model.Bjkl8Qihao;
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
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.Game.Lucklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Lucklist(");
            strSql.Append("SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao,Bjkl8Nums)");
            strSql.Append(" values (");
            strSql.Append("@SumNum,@PostNum,@BeginTime,@EndTime,@LuckCent,@Pool,@BeforePool,@State,@panduan,@Bjkl8Qihao,@Bjkl8Nums)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@SumNum", SqlDbType.Int,4),
                    new SqlParameter("@PostNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@BeginTime", SqlDbType.DateTime),
                    new SqlParameter("@EndTime", SqlDbType.DateTime),
                    new SqlParameter("@LuckCent", SqlDbType.BigInt,8),
                    new SqlParameter("@Pool", SqlDbType.BigInt,8),
                    new SqlParameter("@BeforePool", SqlDbType.BigInt,8),
                    new SqlParameter("@State", SqlDbType.TinyInt,1),
                    new SqlParameter("@panduan", SqlDbType.NVarChar,50),
                    new SqlParameter("@Bjkl8Qihao", SqlDbType.Int,4),
                    new SqlParameter("@Bjkl8Nums", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.SumNum;
            parameters[1].Value = model.PostNum;
            parameters[2].Value = model.BeginTime;
            parameters[3].Value = model.EndTime;
            parameters[4].Value = model.LuckCent;
            parameters[5].Value = model.Pool;
            parameters[6].Value = model.BeforePool;
            parameters[7].Value = model.State;
            parameters[8].Value = model.panduan;
            parameters[9].Value = model.Bjkl8Qihao;
            parameters[10].Value = model.Bjkl8Nums;
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
        public void Update(BCW.Model.Game.Lucklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Lucklist set ");
            strSql.Append("SumNum=@SumNum,");
            strSql.Append("BeginTime=@BeginTime,");
            strSql.Append("EndTime=@EndTime,");
            strSql.Append("State=@State,");
            strSql.Append("PostNum=@PostNum,");
            strSql.Append("Bjkl8Nums=@Bjkl8Nums,");
            strSql.Append("LuckCent=@LuckCent");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SumNum", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
                      new SqlParameter("@State", SqlDbType.Int,4), 
                    new SqlParameter("@PostNum",SqlDbType.NVarChar,50),
                    new SqlParameter("@Bjkl8Nums", SqlDbType.NVarChar,100),
                    new SqlParameter("@LuckCent", SqlDbType.Int,8)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.SumNum;
            parameters[2].Value = model.BeginTime;
            parameters[3].Value = model.EndTime;
            parameters[4].Value = model.State;
            parameters[5].Value = model.PostNum;
            parameters[6].Value = model.Bjkl8Nums;
            parameters[7].Value = model.LuckCent;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新本期记录
        /// </summary>
        public int Update2(BCW.Model.Game.Lucklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Lucklist set ");
            strSql.Append("SumNum=@SumNum,");
            strSql.Append("PostNum=@PostNum,");
            strSql.Append("LuckCent=@LuckCent,");
            strSql.Append("Bjkl8Nums=@Bjkl8Nums,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID and State=0");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SumNum", SqlDbType.Int,4),
					new SqlParameter("@PostNum", SqlDbType.NVarChar,50),
					new SqlParameter("@LuckCent", SqlDbType.BigInt,8),
                    new SqlParameter("@Bjkl8Nums",  SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.SumNum;
            parameters[2].Value = model.PostNum;
            parameters[3].Value = model.LuckCent;
            parameters[4].Value = model.Bjkl8Nums;
            parameters[5].Value = model.State;

            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 根据Bjkl8Qihao开奖
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int Update3(BCW.Model.Game.Lucklist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Lucklist set ");
            strSql.Append("SumNum=@SumNum,");
            strSql.Append("PostNum=@PostNum,");
            strSql.Append("LuckCent=@LuckCent,");
            strSql.Append("Bjkl8Nums=@Bjkl8Nums,");
            strSql.Append("State=@State");
            strSql.Append(" where Bjkl8Qihao=@Bjkl8Qihao and State=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bjkl8Qihao", SqlDbType.Int,4),
                    new SqlParameter("@SumNum", SqlDbType.Int,4),
                    new SqlParameter("@PostNum", SqlDbType.NVarChar,50),
                    new SqlParameter("@LuckCent", SqlDbType.BigInt,8),
                    new SqlParameter("@Bjkl8Nums",  SqlDbType.NVarChar,100),
                    new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Bjkl8Qihao;
            parameters[1].Value = model.SumNum;
            parameters[2].Value = model.PostNum;
            parameters[3].Value = model.LuckCent;
            parameters[4].Value = model.Bjkl8Nums;
            parameters[5].Value = model.State;

            return SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update(int ID, int SumNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Lucklist set ");
            strSql.Append("SumNum=@SumNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@SumNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = SumNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新奖池基金
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Lucklist set ");
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
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Lucklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top(1) * from tb_Lucklist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                    {
                        model.panduan = reader.GetString(9);
                    }
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    model.Bjkl8Nums = reader.GetString(11);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到最新一期的状态
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistState()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao from tb_Lucklist ");
            strSql.Append("order by Bjkl8Qihao desc");
            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                        model.panduan = reader.GetString(9);
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SumNum = 0;
                    model.PostNum = "";
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now.AddMinutes(30);
                    model.LuckCent = 0;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.panduan = "";
                    model.Bjkl8Qihao = 1;
                    return model;
                }
            }
        }

        /// <summary>
        /// 根据时间获取最新一条数据
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistByTime()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao from tb_Lucklist ");
            strSql.Append("order by BeginTime desc");
            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                        model.panduan = reader.GetString(9);
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SumNum = 0;
                    model.PostNum = "";
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now.AddMinutes(30);
                    model.LuckCent = 0;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.panduan = "";
                    model.Bjkl8Qihao = 1;
                    return model;
                }
            }
        }
        /// <summary>
        /// 得到下一期未开奖的期数
        /// </summary>
        public BCW.Model.Game.Lucklist GetNextLucklist()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao from tb_Lucklist ");
            strSql.Append("where PostNum='' and State=0");
            strSql.Append("order by Bjkl8Qihao desc");
            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                        model.panduan = reader.GetString(9);
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SumNum = 0;
                    model.PostNum = "";
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now.AddMinutes(30);
                    model.LuckCent = 0;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.panduan = "";
                    model.Bjkl8Qihao = 1;
                    return model;
                }
            }
        }
        /// <summary>
        /// 得到本期对象实体
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklistSecond()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_Lucklist ");
            strSql.Append("where Bjkl8Qihao not in (select top 1 Bjkl8Qihao from tb_Lucklist order by Bjkl8Qihao desc ) ");
            strSql.Append("order by Bjkl8Qihao desc");

            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                        model.panduan = reader.GetString(9);
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SumNum = 0;
                    model.PostNum = "";
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now.AddMinutes(30);
                    model.LuckCent = 0;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.panduan = "";
                    return model;
                }
            }
        }
        /// <summary>
        /// 得到本期对象实体
        /// </summary>
        public BCW.Model.Game.Lucklist GetLucklist()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,SumNum,PostNum,BeginTime,EndTime,LuckCent,Pool,BeforePool,State,panduan,Bjkl8Qihao from tb_Lucklist ");
            strSql.Append("where PostNum='' and State=0");
            strSql.Append("order by id asc");
            BCW.Model.Game.Lucklist model = new BCW.Model.Game.Lucklist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.SumNum = reader.GetInt32(1);
                    model.PostNum = reader.GetString(2);
                    model.BeginTime = reader.GetDateTime(3);
                    model.EndTime = reader.GetDateTime(4);
                    model.LuckCent = reader.GetInt64(5);
                    model.Pool = reader.GetInt64(6);
                    model.BeforePool = reader.GetInt64(7);
                    model.State = reader.GetByte(8);
                    if (!reader.IsDBNull(9))
                        model.panduan = reader.GetString(9);
                    model.Bjkl8Qihao = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.SumNum = 0;
                    model.PostNum = "";
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now.AddMinutes(30);
                    model.LuckCent = 0;
                    model.Pool = 0;
                    model.BeforePool = 0;
                    model.panduan = "";
                    return model;
                }
            }
        }
        /// <summary>
        /// 根据Bjkl8Qihao得到对应数据的ID
        /// </summary>
        public int GetID(int Bjkl8Qihao)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_Lucklist ");
            strSql.Append(" where Bjkl8Qihao=@Bjkl8Qihao ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Bjkl8Qihao", SqlDbType.Int,4)};
            parameters[0].Value = Bjkl8Qihao;
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
        /// 得到当天的开始期数
        /// </summary>
        public string GetPanduan()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 panduan from tb_Lucklist ");
            strSql.Append(" order by ID desc");
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 得到奖池基金
        /// </summary>
        public long GetPool(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Pool from tb_Lucklist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
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
            strSql.Append(" FROM tb_Lucklist ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Lucklist> listLucklists = new List<BCW.Model.Game.Lucklist>();
            string sTable = "tb_Lucklist";
            string sPkey = "id";
            string sField = "ID,SumNum,PostNum,Bjkl8Qihao";
            string sCondition = strWhere;
            string sOrder = "Bjkl8Qihao Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listLucklists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Lucklist objLucklist = new BCW.Model.Game.Lucklist();
                    objLucklist.ID = reader.GetInt32(0);
                    objLucklist.SumNum = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        objLucklist.PostNum = reader.GetString(2);
                    objLucklist.Bjkl8Qihao = reader.GetInt32(3);
                    listLucklists.Add(objLucklist);
                }
            }
            return listLucklists;
        }


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Lucklist</returns>
        public IList<BCW.Model.Game.Lucklist> GetLucklists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Lucklist> listLucklists = new List<BCW.Model.Game.Lucklist>();
            string sTable = "tb_Lucklist";
            string sPkey = "id";
            string sField = "ID,SumNum,PostNum,State,Bjkl8Qihao,Bjkl8Nums";
            string sCondition = strWhere;
            string sOrder = "Bjkl8Qihao Desc";
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
                    return listLucklists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Lucklist objLucklist = new BCW.Model.Game.Lucklist();
                    objLucklist.ID = reader.GetInt32(0);
                    objLucklist.SumNum = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        objLucklist.PostNum = reader.GetString(2);
                    objLucklist.State = reader.GetByte(3);
                    objLucklist.Bjkl8Qihao = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        objLucklist.Bjkl8Nums = reader.GetString(5);
                    listLucklists.Add(objLucklist);
                }
            }
            return listLucklists;
        }

        #endregion  成员方法
    }
}

