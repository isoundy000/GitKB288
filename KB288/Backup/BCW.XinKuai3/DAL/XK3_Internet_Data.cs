using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.XinKuai3.DAL
{
    /// <summary>
    /// 数据访问类XK3_Internet_Data。
    /// </summary>
    public class XK3_Internet_Data
    {
        public XK3_Internet_Data()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_XK3_Internet_Data");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_XK3_Internet_Data");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_XK3_Internet_Data(");
            strSql.Append("Lottery_issue,Lottery_num,Lottery_time,UpdateTime,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,DaXiao,DanShuang)");
            strSql.Append(" values (");
            strSql.Append("@Lottery_issue,@Lottery_num,@Lottery_time,@UpdateTime,@Sum,@Three_Same_All,@Three_Same_Single,@Three_Same_Not,@Three_Continue_All,@Two_Same_All,@Two_Same_Single,@Two_dissame,@DaXiao,@DanShuang)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,10),
                    new SqlParameter("@Lottery_time", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Sum", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_dissame", SqlDbType.VarChar,10),
                    new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
                    new SqlParameter("@DanShuang", SqlDbType.VarChar,10)};
            parameters[0].Value = model.Lottery_issue;
            parameters[1].Value = model.Lottery_num;
            parameters[2].Value = model.Lottery_time;
            parameters[3].Value = model.UpdateTime;
            parameters[4].Value = model.Sum;
            parameters[5].Value = model.Three_Same_All;
            parameters[6].Value = model.Three_Same_Single;
            parameters[7].Value = model.Three_Same_Not;
            parameters[8].Value = model.Three_Continue_All;
            parameters[9].Value = model.Two_Same_All;
            parameters[10].Value = model.Two_Same_Single;
            parameters[11].Value = model.Two_dissame;
            parameters[12].Value = model.DaXiao;
            parameters[13].Value = model.DanShuang;

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
        public void Update(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Internet_Data set ");
            strSql.Append("Lottery_issue=@Lottery_issue,");
            strSql.Append("Lottery_num=@Lottery_num,");
            strSql.Append("Lottery_time=@Lottery_time,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Sum=@Sum,");
            strSql.Append("Three_Same_All=@Three_Same_All,");
            strSql.Append("Three_Same_Single=@Three_Same_Single,");
            strSql.Append("Three_Same_Not=@Three_Same_Not,");
            strSql.Append("Three_Continue_All=@Three_Continue_All,");
            strSql.Append("Two_Same_All=@Two_Same_All,");
            strSql.Append("Two_Same_Single=@Two_Same_Single,");
            strSql.Append("Two_dissame=@Two_dissame,");
            strSql.Append("DaXiao=@DaXiao,");
            strSql.Append("DanShuang=@DanShuang");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,10),
                    new SqlParameter("@Lottery_time", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Sum", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_dissame", SqlDbType.VarChar,10),
                    new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
                    new SqlParameter("@DanShuang", SqlDbType.VarChar,10)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Lottery_issue;
            parameters[2].Value = model.Lottery_num;
            parameters[3].Value = model.Lottery_time;
            parameters[4].Value = model.UpdateTime;
            parameters[5].Value = model.Sum;
            parameters[6].Value = model.Three_Same_All;
            parameters[7].Value = model.Three_Same_Single;
            parameters[8].Value = model.Three_Same_Not;
            parameters[9].Value = model.Three_Continue_All;
            parameters[10].Value = model.Two_Same_All;
            parameters[11].Value = model.Two_Same_Single;
            parameters[12].Value = model.Two_dissame;
            parameters[13].Value = model.DaXiao;
            parameters[14].Value = model.DanShuang;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_XK3_Internet_Data ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Lottery_issue,Lottery_num,Lottery_time,UpdateTime,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,DaXiao,DanShuang from tb_XK3_Internet_Data ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
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
        public BCW.XinKuai3.Model.XK3_Internet_Data GetXK3_Internet_Data2(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_XK3_Internet_Data ");
            strSql.Append(" where Lottery_issue=@ID  ORDER BY ID desc");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
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
            strSql.Append(" FROM tb_XK3_Internet_Data ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }



        //============================================
        /// <summary>
        /// me_根据字段取数据列表222
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM (SELECT TOP 2 * FROM tb_XK3_Internet_Data WHERE DaXiao!='' ORDER BY Lottery_time DESC) AS tb_XK3_Internet_Data ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到开奖号码的一个对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3kainum(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,Lottery_issue,Lottery_num,DaXiao,DanShuang from tb_XK3_Internet_Data where Lottery_issue =");
            strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.DaXiao = reader.GetString(3);
                    model.DanShuang = reader.GetString(4);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.Lottery_issue = "";
                    model.Lottery_num = "";
                    model.DaXiao = "";
                    model.DanShuang = "";
                    return model;
                }
            }
        }
        /// <summary>
        /// me_后台人工开奖，根据开奖号码对应的该条开奖数据22
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3all_num2(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_XK3_Internet_Data where Lottery_issue =");
            strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_得到最后一期对象实体
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Lottery_issue,Lottery_num,Lottery_time from tb_XK3_Internet_Data ");
            strSql.Append(sd_where);

            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.Lottery_issue = "0";
                    model.Lottery_num = "";
                    model.Lottery_time = DateTime.Now;
                    return model;
                }
            }
        }
        /// <summary>
        /// me_增加一条开奖数据
        /// </summary>
        public int Add_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_XK3_Internet_Data(");
            strSql.Append("Lottery_issue,Lottery_num,Lottery_time,UpdateTime)");
            strSql.Append(" values (");
            strSql.Append("@Lottery_issue,@Lottery_num,@Lottery_time,@UpdateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,15),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,15),
                    new SqlParameter("@Lottery_time", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Lottery_issue;
            parameters[1].Value = model.Lottery_num;
            parameters[2].Value = model.Lottery_time;
            parameters[3].Value = model.UpdateTime;

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
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_XK3_Internet_Data");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,15)};
            parameters[0].Value = Lottery_issue;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Internet_Data set ");
            //strSql.Append("update tb_XK3_Internet_Data_copy set ");//----------------测试用
            strSql.Append("Lottery_issue=@Lottery_issue,");
            //strSql.Append("Lottery_num=@Lottery_num,");
            //strSql.Append("Lottery_time=@Lottery_time,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Sum=@Sum,");
            strSql.Append("Three_Same_All=@Three_Same_All,");
            strSql.Append("Three_Same_Single=@Three_Same_Single,");
            strSql.Append("Three_Same_Not=@Three_Same_Not,");
            strSql.Append("Three_Continue_All=@Three_Continue_All,");
            strSql.Append("Two_Same_All=@Two_Same_All,");
            strSql.Append("Two_Same_Single=@Two_Same_Single,");
            strSql.Append("Two_dissame=@Two_dissame,");
            strSql.Append("DaXiao=@DaXiao,");
            strSql.Append("DanShuang=@DanShuang");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    //new SqlParameter("@Lottery_num", SqlDbType.VarChar,10),
                    //new SqlParameter("@Lottery_time", SqlDbType.DateTime),
					new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Sum", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_dissame", SqlDbType.VarChar,10),
                    new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
                    new SqlParameter("@DanShuang", SqlDbType.VarChar,10)
                                        };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Lottery_issue;
            //parameters[2].Value = model.Lottery_num;
            //parameters[3].Value = model.Lottery_time;
            parameters[2].Value = model.UpdateTime;
            parameters[3].Value = model.Sum;
            parameters[4].Value = model.Three_Same_All;
            parameters[5].Value = model.Three_Same_Single;
            parameters[6].Value = model.Three_Same_Not;
            parameters[7].Value = model.Three_Continue_All;
            parameters[8].Value = model.Two_Same_All;
            parameters[9].Value = model.Two_Same_Single;
            parameters[10].Value = model.Two_dissame;
            parameters[11].Value = model.DaXiao;
            parameters[12].Value = model.DanShuang;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_更新一条网上开奖数据
        /// </summary>
        public void update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Internet_Data set ");
            strSql.Append("Lottery_issue=@Lottery_issue,");
            strSql.Append("Lottery_num=@Lottery_num");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,10)
                                        };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Lottery_issue;
            parameters[2].Value = model.Lottery_num;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_后台--获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num2(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Internet_Data set ");
            //strSql.Append("update tb_XK3_Internet_Data_copy set ");//----------------测试用
            strSql.Append("Lottery_issue=@Lottery_issue,");
            strSql.Append("Lottery_num=@Lottery_num,");
            strSql.Append("Lottery_time=@Lottery_time,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Sum=@Sum,");
            strSql.Append("Three_Same_All=@Three_Same_All,");
            strSql.Append("Three_Same_Single=@Three_Same_Single,");
            strSql.Append("Three_Same_Not=@Three_Same_Not,");
            strSql.Append("Three_Continue_All=@Three_Continue_All,");
            strSql.Append("Two_Same_All=@Two_Same_All,");
            strSql.Append("Two_Same_Single=@Two_Same_Single,");
            strSql.Append("Two_dissame=@Two_dissame,");
            strSql.Append("DaXiao=@DaXiao,");
            strSql.Append("DanShuang=@DanShuang");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,10),
                    new SqlParameter("@Lottery_time", SqlDbType.DateTime),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Sum", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_dissame", SqlDbType.VarChar,10),
                    new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
                    new SqlParameter("@DanShuang", SqlDbType.VarChar,10)
                                        };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Lottery_issue;
            parameters[2].Value = model.Lottery_num;
            parameters[3].Value = model.Lottery_time;
            parameters[4].Value = model.UpdateTime;
            parameters[5].Value = model.Sum;
            parameters[6].Value = model.Three_Same_All;
            parameters[7].Value = model.Three_Same_Single;
            parameters[8].Value = model.Three_Same_Not;
            parameters[9].Value = model.Three_Continue_All;
            parameters[10].Value = model.Two_Same_All;
            parameters[11].Value = model.Two_Same_Single;
            parameters[12].Value = model.Two_dissame;
            parameters[13].Value = model.DaXiao;
            parameters[14].Value = model.DanShuang;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_后台--获取开奖结果，更新中奖数据
        /// </summary>
        public void Update_num3(BCW.XinKuai3.Model.XK3_Internet_Data model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_XK3_Internet_Data set ");
            strSql.Append("Lottery_issue=@Lottery_issue,");
            strSql.Append("Lottery_num=@Lottery_num,");
            //strSql.Append("Lottery_time=@Lottery_time,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("Sum=@Sum,");
            strSql.Append("Three_Same_All=@Three_Same_All,");
            strSql.Append("Three_Same_Single=@Three_Same_Single,");
            strSql.Append("Three_Same_Not=@Three_Same_Not,");
            strSql.Append("Three_Continue_All=@Three_Continue_All,");
            strSql.Append("Two_Same_All=@Two_Same_All,");
            strSql.Append("Two_Same_Single=@Two_Same_Single,");
            strSql.Append("Two_dissame=@Two_dissame,");
            strSql.Append("DaXiao=@DaXiao,");
            strSql.Append("DanShuang=@DanShuang");
            strSql.Append(" where Lottery_issue=@Lottery_issue ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Lottery_issue", SqlDbType.VarChar,20),
                    new SqlParameter("@Lottery_num", SqlDbType.VarChar,10),
                    //new SqlParameter("@Lottery_time", SqlDbType.DateTime),
					new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@Sum", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Same_Not", SqlDbType.VarChar,10),
                    new SqlParameter("@Three_Continue_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_All", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_Same_Single", SqlDbType.VarChar,10),
                    new SqlParameter("@Two_dissame", SqlDbType.VarChar,10),
                    new SqlParameter("@DaXiao", SqlDbType.VarChar,10),
                    new SqlParameter("@DanShuang", SqlDbType.VarChar,10)
                                        };
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Lottery_issue;
            parameters[2].Value = model.Lottery_num;
            //parameters[3].Value = model.Lottery_time;
            parameters[3].Value = model.UpdateTime;
            parameters[4].Value = model.Sum;
            parameters[5].Value = model.Three_Same_All;
            parameters[6].Value = model.Three_Same_Single;
            parameters[7].Value = model.Three_Same_Not;
            parameters[8].Value = model.Three_Continue_All;
            parameters[9].Value = model.Two_Same_All;
            parameters[10].Value = model.Two_Same_Single;
            parameters[11].Value = model.Two_dissame;
            parameters[12].Value = model.DaXiao;
            parameters[13].Value = model.DanShuang;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_开奖后，获取最后一期
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast2()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_XK3_Internet_Data ");
            strSql.Append(" WHERE Lottery_num!='' Order by Lottery_issue desc");

            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_开奖后，获取最后一期
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3listLast3()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_XK3_Internet_Data where Lottery_num!='' ");
            strSql.Append(" Order by Lottery_time desc ");

            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_计算和值出现次数并排序
        /// </summary>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3all(string _where)
        {
            IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3all = new List<BCW.XinKuai3.Model.XK3_Internet_Data>();
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(Sum) as aa,[Sum] FROM tb_XK3_Internet_Data where [Sum] in (4,5,6,7,8,9,10,11,12,13,14,15,16,17) ");
            strSql.Append(sd_where + "group by Sum ORDER BY aa DESC");
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
                    //reader.Read();
                    model.aa = reader.GetInt32(0);
                    model.Sum = reader.GetString(1);
                    Getxk3all.Add(model);
                }
            }
            return Getxk3all;
        }

        /// <summary>
        /// me_获取二同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_Same_All(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE Two_Same_All!='0' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取二不同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Two_dissame(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE Two_dissame!='0' " + sd_where + ") c ) d ");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取三同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3Three_Same_All(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE Three_Same_All!='0' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取三不同号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Same_Not(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE Three_Same_Not!='0' " + sd_where + ") c ) d ");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取三连号出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3where_Three_Continue_All(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE Three_Continue_All!='0' " + sd_where + ") c ) d ");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取大出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3da(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            //1大2小、1单2双
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE DaXiao='1' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取小出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3xiao(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            //1大2小、1单2双
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE DaXiao='2' " + sd_where + ") c ) d ");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取双出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3shuang(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            //1大2小、1单2双
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE DanShuang='2' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_获取单出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3dan(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            //1大2小、1单2双
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE DanShuang='1' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_计算通吃出现的次数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Internet_Data Getxk3tongchi(string _where)
        {
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) as aa from (SELECT distinct(Lottery_issue) from (SELECT * from tb_XK3_Internet_Data WHERE DanShuang='0' " + sd_where + ") c ) d");
            //strSql.Append(sd_where);
            BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.aa = reader.GetInt32(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_获取最近10期开奖情况
        /// </summary>
        /// 
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3listTop(string _where)
        {
            IList<BCW.XinKuai3.Model.XK3_Internet_Data> Getxk3all = new List<BCW.XinKuai3.Model.XK3_Internet_Data>();
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT TOP(10)* FROM tb_XK3_Internet_Data");
            strSql.Append(sd_where + "Order by Lottery_time desc");//20160328--id改为Lottery_time
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data model = new BCW.XinKuai3.Model.XK3_Internet_Data();
                    model.ID = reader.GetInt32(0);
                    model.Lottery_issue = reader.GetString(1);
                    model.Lottery_num = reader.GetString(2);
                    model.Lottery_time = reader.GetDateTime(3);
                    model.UpdateTime = reader.GetDateTime(4);
                    model.Sum = reader.GetString(5);
                    model.Three_Same_All = reader.GetString(6);
                    model.Three_Same_Single = reader.GetString(7);
                    model.Three_Same_Not = reader.GetString(8);
                    model.Three_Continue_All = reader.GetString(9);
                    model.Two_Same_All = reader.GetString(10);
                    model.Two_Same_Single = reader.GetString(11);
                    model.Two_dissame = reader.GetString(12);
                    model.DaXiao = reader.GetString(13);
                    model.DanShuang = reader.GetString(14);
                    Getxk3all.Add(model);
                }
            }
            return Getxk3all;
        }


        //============================================




        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList XK3_Internet_Data</returns>
        public IList<BCW.XinKuai3.Model.XK3_Internet_Data> GetXK3_Internet_Datas(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.XinKuai3.Model.XK3_Internet_Data> listXK3_Internet_Datas = new List<BCW.XinKuai3.Model.XK3_Internet_Data>();
            string sTable = "tb_XK3_Internet_Data";
            //string sTable = "tb_XK3_Internet_Data_copy";//测试用
            string sPkey = "id";
            string sField = "ID,Lottery_issue,Lottery_num,Lottery_time,UpdateTime,Sum,Three_Same_All,Three_Same_Single,Three_Same_Not,Three_Continue_All,Two_Same_All,Two_Same_Single,Two_dissame,DaXiao,DanShuang";
            string sCondition = strWhere;
            string sOrder = "Lottery_time Desc";
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
                    return listXK3_Internet_Datas;
                }
                while (reader.Read())
                {
                    BCW.XinKuai3.Model.XK3_Internet_Data objXK3_Internet_Data = new BCW.XinKuai3.Model.XK3_Internet_Data();
                    objXK3_Internet_Data.ID = reader.GetInt32(0);
                    objXK3_Internet_Data.Lottery_issue = reader.GetString(1);
                    objXK3_Internet_Data.Lottery_num = reader.GetString(2);
                    objXK3_Internet_Data.Lottery_time = reader.GetDateTime(3);
                    objXK3_Internet_Data.UpdateTime = reader.GetDateTime(4);
                    objXK3_Internet_Data.Sum = reader.GetString(5);
                    objXK3_Internet_Data.Three_Same_All = reader.GetString(6);
                    objXK3_Internet_Data.Three_Same_Single = reader.GetString(7);
                    objXK3_Internet_Data.Three_Same_Not = reader.GetString(8);
                    objXK3_Internet_Data.Three_Continue_All = reader.GetString(9);
                    objXK3_Internet_Data.Two_Same_All = reader.GetString(10);
                    objXK3_Internet_Data.Two_Same_Single = reader.GetString(11);
                    objXK3_Internet_Data.Two_dissame = reader.GetString(12);
                    objXK3_Internet_Data.DaXiao = reader.GetString(13);
                    objXK3_Internet_Data.DanShuang = reader.GetString(14);
                    listXK3_Internet_Datas.Add(objXK3_Internet_Data);
                }
            }
            return listXK3_Internet_Datas;
        }

        #endregion  成员方法
    }
}

