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
    /// 数据访问类Horselist。
    /// </summary>
    public class Horselist
    {
        public Horselist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Horselist");
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Horselist");
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
            strSql.Append("select count(1) from tb_Horselist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Horselist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Horselist(");
            strSql.Append("WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,Odds,State)");
            strSql.Append(" values (");
            strSql.Append("@WinNum,@BeginTime,@EndTime,@Pool,@WinCount,@WinPool,@Odds,@State)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@Pool", SqlDbType.BigInt,8),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@WinPool", SqlDbType.BigInt,8),
					new SqlParameter("@Odds", SqlDbType.NVarChar,500),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.WinNum;
            parameters[1].Value = model.BeginTime;
            parameters[2].Value = model.EndTime;
            parameters[3].Value = model.Pool;
            parameters[4].Value = model.WinCount;
            parameters[5].Value = model.WinPool;
            parameters[6].Value = model.Odds;
            parameters[7].Value = model.State;

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
        public void Update(BCW.Model.Game.Horselist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("BeginTime=@BeginTime,");
            strSql.Append("EndTime=@EndTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@BeginTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinNum;
            parameters[2].Value = model.BeginTime;
            parameters[3].Value = model.EndTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新本期记录
        /// </summary>
        public void Update2(BCW.Model.Game.Horselist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
            strSql.Append("WinNum=@WinNum,");
            strSql.Append("WinPool=@WinPool,");
            strSql.Append("WinCount=@WinCount,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@WinPool", SqlDbType.BigInt,8),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.WinNum;
            parameters[2].Value = model.WinPool;
            parameters[3].Value = model.WinCount;
            parameters[4].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新总押注金额
        /// </summary>
        public void UpdatePool(int ID, long Pool)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
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
        /// 更新总押注金额2
        /// </summary>
        public void UpdateWinPool(int ID, long WinPool)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
            strSql.Append("WinPool=WinPool+@WinPool");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinPool", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = WinPool;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖数字
        /// </summary>
        public void UpdateWinNum(int ID, int WinNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
            strSql.Append("WinNum=@WinNum");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinNum", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = WinNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖数据
        /// </summary>
        public void UpdateWinData(int ID, string WinData)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Horselist set ");
            strSql.Append("WinData=@WinData");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinData", SqlDbType.NVarChar,100)};
            parameters[0].Value = ID;
            parameters[1].Value = WinData;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Horselist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到上一期对象实体
        /// </summary>
        public BCW.Model.Game.Horselist GetHorselistBf()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,WinCount,Odds from tb_Horselist ");
            strSql.Append(" where State=@State ");
            strSql.Append(" Order By ID DESC ");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = 1;

            BCW.Model.Game.Horselist model = new BCW.Model.Game.Horselist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.WinCount = reader.GetInt32(2);
                    model.Odds = reader.GetString(3);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个本期实体
        /// </summary>
        public BCW.Model.Game.Horselist GetHorselist()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,BeginTime,EndTime,Pool,WinPool,Odds,State from tb_Horselist ");
            strSql.Append(" Order By ID DESC ");

            BCW.Model.Game.Horselist model = new BCW.Model.Game.Horselist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.BeginTime = reader.GetDateTime(2);
                    model.EndTime = reader.GetDateTime(3);
                    model.Pool = reader.GetInt64(4);
                    model.WinPool = reader.GetInt64(5);
                    model.Odds = reader.GetString(6);
                    model.State = reader.GetByte(7);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    model.WinNum = 0;
                    model.BeginTime = DateTime.Now;
                    model.EndTime = DateTime.Now;
                    model.Pool = 0;
                    model.State = 0;
                    model.Odds = "";
                    return model;
                }
            }
        }

        /// <summary>
        /// 得到一个WinNum
        /// </summary>
        public int GetWinNum(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinNum from tb_Horselist ");
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
        /// 得到WinData
        /// </summary>
        public string GetWinData(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select WinData from tb_Horselist ");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Horselist GetHorselist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,Odds,WinData,State from tb_Horselist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Horselist model = new BCW.Model.Game.Horselist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.WinNum = reader.GetInt32(1);
                    model.BeginTime = reader.GetDateTime(2);
                    model.EndTime = reader.GetDateTime(3);
                    model.Pool = reader.GetInt64(4);
                    model.WinCount = reader.GetInt32(5);
                    model.WinPool = reader.GetInt64(6);
                    model.Odds = reader.GetString(7);
                    if (!reader.IsDBNull(8))
                        model.WinData = reader.GetString(8);
                    model.State = reader.GetByte(9);
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
            strSql.Append(" FROM tb_Horselist ");
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
        /// <returns>IList Horselist</returns>
        public IList<BCW.Model.Game.Horselist> GetHorselists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Horselist> listHorselists = new List<BCW.Model.Game.Horselist>();
            string sTable = "tb_Horselist";
            string sPkey = "id";
            string sField = "ID,WinNum,Odds";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listHorselists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Horselist objHorselist = new BCW.Model.Game.Horselist();
                    objHorselist.ID = reader.GetInt32(0);
                    objHorselist.WinNum = reader.GetInt32(1);
                    objHorselist.Odds = reader.GetString(2);
                    listHorselists.Add(objHorselist);
                }
            }
            return listHorselists;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Horselist</returns>
        public IList<BCW.Model.Game.Horselist> GetHorselists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Horselist> listHorselists = new List<BCW.Model.Game.Horselist>();
            string sTable = "tb_Horselist";
            string sPkey = "id";
            string sField = "ID,WinNum,BeginTime,EndTime,Pool,WinCount,WinPool,Odds,State";
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
                    return listHorselists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Horselist objHorselist = new BCW.Model.Game.Horselist();
                    objHorselist.ID = reader.GetInt32(0);
                    objHorselist.WinNum = reader.GetInt32(1);
                    objHorselist.BeginTime = reader.GetDateTime(2);
                    objHorselist.EndTime = reader.GetDateTime(3);
                    objHorselist.Pool = reader.GetInt64(4);
                    objHorselist.WinCount = reader.GetInt32(5);
                    objHorselist.WinPool = reader.GetInt64(6);
                    objHorselist.Odds = reader.GetString(7);
                    objHorselist.State = reader.GetByte(8);
                    listHorselists.Add(objHorselist);
                }
            }
            return listHorselists;
        }

        #endregion  成员方法
    }
}

