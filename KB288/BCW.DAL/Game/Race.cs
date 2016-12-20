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
    /// 数据访问类Race。
    /// </summary>
    public class Race
    {
        public Race()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Race");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Race");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int Types, int paytype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Race");
            strSql.Append(" where ID=@ID ");
            if (Types != -1)
                strSql.Append(" and Types=" + Types + " ");

            strSql.Append(" and paytype=@paytype ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@paytype", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = paytype;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Race");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and userid=@userid ");
            strSql.Append(" and payCount=@payCount ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@userid", SqlDbType.Int,4),
					new SqlParameter("@payCount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = userid;
            parameters[2].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某用户今天发布竞拍数量
        /// </summary>
        public int GetTodayCount(int userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Race");
            strSql.Append(" where userid=@userid ");
            strSql.Append(" and Year(writetime)=" + DateTime.Now.Year + " AND Month(writetime) = " + DateTime.Now.Month + " and Day(writetime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@userid", SqlDbType.Int,4)};
            parameters[0].Value = userid;

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
        public int Add(BCW.Model.Game.Race model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Race(");
            strSql.Append("userid,username,title,fileurl,content,pcontent,price,writetime,totime,payCount,paytype,topPrice,winID,winName,isCase,Notes,Types,writedate)");
            strSql.Append(" values (");
            strSql.Append("@userid,@username,@title,@fileurl,@content,@pcontent,@price,@writetime,@totime,@payCount,@paytype,@topPrice,@winID,@winName,@isCase,@Notes,@Types,@writedate)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@userid", SqlDbType.Int,4),
					new SqlParameter("@username", SqlDbType.NVarChar,50),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@fileurl", SqlDbType.NVarChar,100),
					new SqlParameter("@content", SqlDbType.NText),
					new SqlParameter("@pcontent", SqlDbType.NVarChar,500),
					new SqlParameter("@price", SqlDbType.BigInt,8),
					new SqlParameter("@writetime", SqlDbType.DateTime),
					new SqlParameter("@totime", SqlDbType.DateTime),
					new SqlParameter("@payCount", SqlDbType.Int,4),
					new SqlParameter("@paytype", SqlDbType.Int,4),
					new SqlParameter("@topPrice", SqlDbType.BigInt,8),
					new SqlParameter("@winID", SqlDbType.Int,4),
					new SqlParameter("@winName", SqlDbType.NVarChar,50),
					new SqlParameter("@isCase", SqlDbType.Int,4),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@writedate", SqlDbType.SmallDateTime)};
            parameters[0].Value = model.userid;
            parameters[1].Value = model.username;
            parameters[2].Value = model.title;
            parameters[3].Value = model.fileurl;
            parameters[4].Value = model.content;
            parameters[5].Value = model.pcontent;
            parameters[6].Value = model.price;
            parameters[7].Value = model.writetime;
            parameters[8].Value = model.totime;
            parameters[9].Value = 0;
            parameters[10].Value = model.paytype;
            parameters[11].Value = model.topPrice;
            parameters[12].Value = 0;
            parameters[13].Value = "";
            parameters[14].Value = 0;
            parameters[15].Value = "";
            parameters[16].Value = model.Types;
            parameters[17].Value = model.writedate;

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
        public void UpdatetopPrice(int ID, long topPrice, int winID, string winName, int paytype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Race set ");
            strSql.Append("payCount=payCount+1,");
            strSql.Append("topPrice=@topPrice,");
            strSql.Append("winID=@winID,");
            strSql.Append("winName=@winName,");
            strSql.Append("paytype=@paytype");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@topPrice", SqlDbType.BigInt,8),
					new SqlParameter("@winID", SqlDbType.Int,4),
					new SqlParameter("@winName", SqlDbType.NVarChar,50),
					new SqlParameter("@paytype", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = topPrice;
            parameters[2].Value = winID;
            parameters[3].Value = winName;
            parameters[4].Value = paytype;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatepaytype(int ID, int paytype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Race set ");
            strSql.Append("paytype=@paytype");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@paytype", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = paytype;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatetotime(int ID, DateTime totime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Race set ");
            strSql.Append("totime=@totime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@totime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = totime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Game.Race model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Race set ");
            strSql.Append("userid=@userid,");
            strSql.Append("username=@username,");
            strSql.Append("title=@title,");
            strSql.Append("fileurl=@fileurl,");
            strSql.Append("content=@content,");
            strSql.Append("pcontent=@pcontent,");
            strSql.Append("price=@price,");
            strSql.Append("writetime=@writetime,");
            strSql.Append("totime=@totime,");
            strSql.Append("paytype=@paytype,");
            strSql.Append("topPrice=@topPrice,");
            strSql.Append("winID=@winID,");
            strSql.Append("winName=@winName,");
            strSql.Append("Types=@Types");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@userid", SqlDbType.Int,4),
					new SqlParameter("@username", SqlDbType.NVarChar,50),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@fileurl", SqlDbType.NVarChar,100),
					new SqlParameter("@content", SqlDbType.NText),
					new SqlParameter("@pcontent", SqlDbType.NVarChar,500),
					new SqlParameter("@price", SqlDbType.BigInt,8),
					new SqlParameter("@writetime", SqlDbType.DateTime),
					new SqlParameter("@totime", SqlDbType.DateTime),
					new SqlParameter("@paytype", SqlDbType.Int,4),
					new SqlParameter("@topPrice", SqlDbType.BigInt,8),
					new SqlParameter("@winID", SqlDbType.Int,4),
					new SqlParameter("@winName", SqlDbType.NVarChar,50),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.userid;
            parameters[2].Value = model.username;
            parameters[3].Value = model.title;
            parameters[4].Value = model.fileurl;
            parameters[5].Value = model.content;
            parameters[6].Value = model.pcontent;
            parameters[7].Value = model.price;
            parameters[8].Value = model.writetime;
            parameters[9].Value = model.totime;
            parameters[10].Value = model.paytype;
            parameters[11].Value = model.topPrice;
            parameters[12].Value = model.winID;
            parameters[13].Value = model.winName;
            parameters[14].Value = model.Types;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Race ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Race GetRace(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,userid,username,title,fileurl,content,pcontent,price,writetime,totime,payCount,paytype,topPrice,winID,winName,isCase,Notes,Types,writedate from tb_Race ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Race model = new BCW.Model.Game.Race();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.userid = reader.GetInt32(1);
                    model.username = reader.GetString(2);
                    model.title = reader.GetString(3);
                    model.fileurl = reader.GetString(4);
                    model.content = reader.GetString(5);
                    model.pcontent = reader.GetString(6);
                    model.price = reader.GetInt64(7);
                    model.writetime = reader.GetDateTime(8);
                    model.totime = reader.GetDateTime(9);
                    model.payCount = reader.GetInt32(10);
                    model.paytype = reader.GetInt32(11);
                    model.topPrice = reader.GetInt64(12);
                    model.winID = reader.GetInt32(13);
                    model.winName = reader.GetString(14);
                    model.isCase = reader.GetInt32(15);
                    model.Notes = reader.GetString(16);
                    model.Types = reader.GetInt32(17);
                    model.writedate = reader.GetDateTime(18);
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
            strSql.Append(" FROM tb_Race ");
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
        /// <returns>IList Race</returns>
        public IList<BCW.Model.Game.Race> GetRaces(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Race> listRaces = new List<BCW.Model.Game.Race>();
            string sTable = "tb_Race";
            string sPkey = "id";
            string sField = "ID,Title,fileurl,payCount,Totime,Types";
            string sCondition = strWhere;
            string sOrder = "Totime Asc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listRaces;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Race objRace = new BCW.Model.Game.Race();
                    objRace.ID = reader.GetInt32(0);
                    objRace.title = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                        objRace.fileurl = reader.GetString(2);
                    objRace.payCount = reader.GetInt32(3);
                    if (!reader.IsDBNull(4))
                        objRace.totime = reader.GetDateTime(4);
                    objRace.Types = reader.GetInt32(5);
                    listRaces.Add(objRace);
                }
            }
            return listRaces;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Race</returns>
        public IList<BCW.Model.Game.Race> GetRaces(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Race> listRaces = new List<BCW.Model.Game.Race>();
            string sTable = "tb_Race";
            string sPkey = "id";
            string sField = "ID,userid,username,title,fileurl,price,writetime,totime,winID,winName,Types";
            string sCondition = strWhere;
            string sOrder = "Totime Asc";
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
                    return listRaces;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Race objRace = new BCW.Model.Game.Race();
                    objRace.ID = reader.GetInt32(0);
                    objRace.userid = reader.GetInt32(1);
                    objRace.username = reader.GetString(2);
                    objRace.title = reader.GetString(3);
                    if (!reader.IsDBNull(4))
                        objRace.fileurl = reader.GetString(4);
                    objRace.price = reader.GetInt64(5);
                    objRace.writetime = reader.GetDateTime(6);
                    objRace.totime = reader.GetDateTime(7);
                    objRace.winID = reader.GetInt32(8);
                    if (!reader.IsDBNull(9))
                        objRace.winName = reader.GetString(9);
                    objRace.Types = reader.GetInt32(10);
                    listRaces.Add(objRace);
                }
            }
            return listRaces;
        }

        #endregion  成员方法
    }
}
