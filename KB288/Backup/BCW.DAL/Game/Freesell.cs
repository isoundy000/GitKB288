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
    /// 数据访问类Freesell。
    /// </summary>
    public class Freesell
    {
        public Freesell()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Freesell");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Freesell");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Freesell model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Freesell(");
            strSql.Append("cclID,UserID,UserName,Title,Content,Odds,Price,Counts,Counts2,CloseTime,OpenTime,OpenTime2,State,OpenStats,IsGood,OpenText,Openbbs,OpenbbsTime,cclUserID,cclUserName,ccliType,ccliName)");
            strSql.Append(" values (");
            strSql.Append("@cclID,@UserID,@UserName,@Title,@Content,@Odds,@Price,@Counts,@Counts2,@CloseTime,@OpenTime,@OpenTime2,@State,@OpenStats,@IsGood,@OpenText,@Openbbs,@OpenbbsTime,@cclUserID,@cclUserName,@ccliType,@ccliName)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@cclID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@Odds", SqlDbType.Money,8),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Counts", SqlDbType.Int,4),
					new SqlParameter("@Counts2", SqlDbType.Int,4),
					new SqlParameter("@CloseTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime2", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@OpenStats", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.Int,4),
					new SqlParameter("@OpenText", SqlDbType.NVarChar,50),
					new SqlParameter("@Openbbs", SqlDbType.NVarChar,50),
					new SqlParameter("@OpenbbsTime", SqlDbType.DateTime),
					new SqlParameter("@cclUserID", SqlDbType.Int,4),
					new SqlParameter("@cclUserName", SqlDbType.NVarChar,50),
					new SqlParameter("@ccliType", SqlDbType.Int,4),
					new SqlParameter("@ccliName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.cclID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Odds;
            parameters[6].Value = model.Price;
            parameters[7].Value = model.Counts;
            parameters[8].Value = model.Counts2;
            parameters[9].Value = model.CloseTime;
            parameters[10].Value = model.OpenTime;
            parameters[11].Value = model.OpenTime2;
            parameters[12].Value = model.State;
            parameters[13].Value = model.OpenStats;
            parameters[14].Value = model.IsGood;
            parameters[15].Value = model.OpenText;
            parameters[16].Value = model.Openbbs;
            parameters[17].Value = model.OpenbbsTime;
            parameters[18].Value = model.cclUserID;
            parameters[19].Value = model.cclUserName;
            parameters[20].Value = model.ccliType;
            parameters[21].Value = model.ccliName;

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
        public void Update(BCW.Model.Game.Freesell model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Freesell set ");
            strSql.Append("cclID=@cclID,");
            strSql.Append("UserID=@UserID,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("Odds=@Odds,");
            strSql.Append("Price=@Price,");
            strSql.Append("Counts=@Counts,");
            strSql.Append("Counts2=@Counts2,");
            strSql.Append("CloseTime=@CloseTime,");
            strSql.Append("OpenTime=@OpenTime,");
            strSql.Append("OpenTime2=@OpenTime2,");
            strSql.Append("State=@State,");
            strSql.Append("OpenStats=@OpenStats,");
            strSql.Append("IsGood=@IsGood,");
            strSql.Append("OpenText=@OpenText,");
            strSql.Append("Openbbs=@Openbbs,");
            strSql.Append("OpenbbsTime=@OpenbbsTime,");
            strSql.Append("cclUserID=@cclUserID,");
            strSql.Append("cclUserName=@cclUserName,");
            strSql.Append("ccliType=@ccliType,");
            strSql.Append("ccliName=@ccliName");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@cclID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@Odds", SqlDbType.Money,8),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Counts", SqlDbType.Int,4),
					new SqlParameter("@Counts2", SqlDbType.Int,4),
					new SqlParameter("@CloseTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime2", SqlDbType.DateTime),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@OpenStats", SqlDbType.Int,4),
					new SqlParameter("@IsGood", SqlDbType.Int,4),
					new SqlParameter("@OpenText", SqlDbType.NVarChar,50),
					new SqlParameter("@Openbbs", SqlDbType.NVarChar,50),
					new SqlParameter("@OpenbbsTime", SqlDbType.DateTime),
					new SqlParameter("@cclUserID", SqlDbType.Int,4),
					new SqlParameter("@cclUserName", SqlDbType.NVarChar,50),
					new SqlParameter("@ccliType", SqlDbType.Int,4),
					new SqlParameter("@ccliName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.cclID;
            parameters[2].Value = model.UserID;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.Odds;
            parameters[7].Value = model.Price;
            parameters[8].Value = model.Counts;
            parameters[9].Value = model.Counts2;
            parameters[10].Value = model.CloseTime;
            parameters[11].Value = model.OpenTime;
            parameters[12].Value = model.OpenTime2;
            parameters[13].Value = model.State;
            parameters[14].Value = model.OpenStats;
            parameters[15].Value = model.IsGood;
            parameters[16].Value = model.OpenText;
            parameters[17].Value = model.Openbbs;
            parameters[18].Value = model.OpenbbsTime;
            parameters[19].Value = model.cclUserID;
            parameters[20].Value = model.cclUserName;
            parameters[21].Value = model.ccliType;
            parameters[22].Value = model.ccliName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Freesell ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Freesell ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Freesell GetFreesell(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,cclID,UserID,UserName,Title,Content,Odds,Price,Counts,Counts2,CloseTime,OpenTime,OpenTime2,State,OpenStats,IsGood,OpenText,Openbbs,OpenbbsTime,cclUserID,cclUserName,ccliType,ccliName from tb_Freesell ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Freesell model = new BCW.Model.Game.Freesell();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.cclID = reader.GetInt32(1);
                    model.UserID = reader.GetInt32(2);
                    model.UserName = reader.GetString(3);
                    model.Title = reader.GetString(4);
                    model.Content = reader.GetString(5);
                    model.Odds = reader.GetDecimal(6);
                    model.Price = reader.GetInt32(7);
                    model.Counts = reader.GetInt32(8);
                    model.Counts2 = reader.GetInt32(9);
                    model.CloseTime = reader.GetDateTime(10);
                    model.OpenTime = reader.GetDateTime(11);
                    if (!reader.IsDBNull(12))
                        model.OpenTime2 = reader.GetDateTime(12);
                    model.State = reader.GetInt32(13);
                    model.OpenStats = reader.GetInt32(14);
                    model.IsGood = reader.GetInt32(15);
                    if (!reader.IsDBNull(16))
                        model.OpenText = reader.GetString(16);
                    if (!reader.IsDBNull(17))
                        model.Openbbs = reader.GetString(17);
                    if (!reader.IsDBNull(18))
                        model.OpenbbsTime = reader.GetDateTime(18);
                    model.cclUserID = reader.GetInt32(19);
                    model.cclUserName = reader.GetString(20);
                    model.ccliType = reader.GetInt32(21);
                    model.ccliName = reader.GetString(22);
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
            strSql.Append(" FROM tb_Freesell ");
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
        /// <returns>IList Freesell</returns>
        public IList<BCW.Model.Game.Freesell> GetFreesells(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Freesell> listFreesells = new List<BCW.Model.Game.Freesell>();
            string sTable = "tb_Freesell";
            string sPkey = "id";
            string sField = "ID,UserID,UserName,Odds,Price,Counts2,State,OpenStats,IsGood,ccliName,Title";
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
                    return listFreesells;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Freesell objFreesell = new BCW.Model.Game.Freesell();
                    objFreesell.ID = reader.GetInt32(0);
                    objFreesell.UserID = reader.GetInt32(1);
                    objFreesell.UserName = reader.GetString(2);
                    objFreesell.Odds = reader.GetDecimal(3);
                    objFreesell.Price = reader.GetInt32(4);
                    objFreesell.Counts2 = reader.GetInt32(5);
                    objFreesell.State = reader.GetInt32(6);
                    objFreesell.OpenStats = reader.GetInt32(7);
                    objFreesell.IsGood = reader.GetInt32(8);
                    objFreesell.ccliName = reader.GetString(9);
                    objFreesell.Title = reader.GetString(10);
                    listFreesells.Add(objFreesell);
                }
            }
            return listFreesells;
        }

        #endregion  成员方法
    }
}

