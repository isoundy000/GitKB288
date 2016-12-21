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
    /// 数据访问类Free。
    /// </summary>
    public class Free
    {
        public Free()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Free");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Free");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Free");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UserID=@UserID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UserID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Free model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Free(");
            strSql.Append("UserID,UserName,Title,Content,pText,pCount,pCount2,pOdds,pNum,Price,Counts,Counts2,CloseTime,OpenTime,OpenTime2,CclType,pState,State,OpenStats,OpenText,Good,General,Poor,cclstat,cclcent,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@UserName,@Title,@Content,@pText,@pCount,@pCount2,@pOdds,@pNum,@Price,@Counts,@Counts2,@CloseTime,@OpenTime,@OpenTime2,@CclType,@pState,@State,@OpenStats,@OpenText,@Good,@General,@Poor,@cclstat,@cclcent,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@pText", SqlDbType.NVarChar,800),
					new SqlParameter("@pCount", SqlDbType.NVarChar,800),
					new SqlParameter("@pCount2", SqlDbType.NVarChar,800),
					new SqlParameter("@pOdds", SqlDbType.NVarChar,800),
					new SqlParameter("@pNum", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@Counts", SqlDbType.Int,4),
					new SqlParameter("@Counts2", SqlDbType.Int,4),
					new SqlParameter("@CloseTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime2", SqlDbType.DateTime),
					new SqlParameter("@CclType", SqlDbType.Int,4),
					new SqlParameter("@pState", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.Int,4),
					new SqlParameter("@OpenStats", SqlDbType.Int,4),
					new SqlParameter("@OpenText", SqlDbType.NVarChar,50),
					new SqlParameter("@Good", SqlDbType.Int,4),
					new SqlParameter("@General", SqlDbType.Int,4),
					new SqlParameter("@Poor", SqlDbType.Int,4),
					new SqlParameter("@cclstat", SqlDbType.Int,4),
					new SqlParameter("@cclcent", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.pText;
            parameters[5].Value = model.pCount;
            parameters[6].Value = model.pCount2;
            parameters[7].Value = model.pOdds;
            parameters[8].Value = model.pNum;
            parameters[9].Value = model.Price;
            parameters[10].Value = model.Counts;
            parameters[11].Value = model.Counts2;
            parameters[12].Value = model.CloseTime;
            parameters[13].Value = model.OpenTime;
            parameters[14].Value = model.OpenTime2;
            parameters[15].Value = model.CclType;
            parameters[16].Value = "";
            parameters[17].Value = 0;
            parameters[18].Value = 0;
            parameters[19].Value = "";
            parameters[20].Value = 0;
            parameters[21].Value = 0;
            parameters[22].Value = 0;
            parameters[23].Value = 0;
            parameters[24].Value = model.cclcent;
            parameters[25].Value = model.AddTime;

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
        public void Update(BCW.Model.Game.Free model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Free set ");
            strSql.Append("UserID=@UserID,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("Title=@Title,");
            strSql.Append("Content=@Content,");
            strSql.Append("Price=@Price,");
            strSql.Append("CloseTime=@CloseTime,");
            strSql.Append("OpenTime=@OpenTime,");
            strSql.Append("CclType=@CclType,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,1000),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@CloseTime", SqlDbType.DateTime),
					new SqlParameter("@OpenTime", SqlDbType.DateTime),
					new SqlParameter("@CclType", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.UserName;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.CloseTime;
            parameters[7].Value = model.OpenTime;
            parameters[8].Value = model.CclType;
            parameters[9].Value = model.State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Free ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Free ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Free GetFree(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UserID,UserName,Title,Content,pText,pCount,pCount2,pOdds,pNum,Price,Counts,Counts2,State,CloseTime,OpenTime,OpenTime2,OpenText,Good,General,Poor,pState,CclType,cclcent from tb_Free ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Free model = new BCW.Model.Game.Free();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UserID = reader.GetInt32(1);
                    model.UserName = reader.GetString(2);
                    model.Title = reader.GetString(3);
                    model.Content = reader.GetString(4);
                    model.pText = reader.GetString(5);
                    model.pCount = reader.GetString(6);
                    model.pCount2 = reader.GetString(7);
                    model.pOdds = reader.GetString(8);
                    model.pNum = reader.GetInt32(9);
                    model.Price = reader.GetInt32(10);
                    model.Counts = reader.GetInt32(11);
                    model.Counts2 = reader.GetInt32(12);
                    model.State = reader.GetInt32(13);
                    model.CloseTime = reader.GetDateTime(14);
                    model.OpenTime = reader.GetDateTime(15);
                    model.OpenTime2 = reader.GetDateTime(16);
                    model.OpenText = reader.GetString(17);
                    model.Good = reader.GetInt32(18);
                    model.General = reader.GetInt32(19);
                    model.Poor = reader.GetInt32(20);
                    model.pState = reader.GetString(21);
                    model.CclType = reader.GetInt32(22);
                    model.cclcent = reader.GetInt64(23);
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
            strSql.Append(" FROM tb_Free ");
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
        /// <returns>IList Free</returns>
        public IList<BCW.Model.Game.Free> GetFrees(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.Free> listFrees = new List<BCW.Model.Game.Free>();
            string sTable = "tb_Free";
            string sPkey = "id";
            string sField = "ID,Title,cclType,Price";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listFrees;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Free objFree = new BCW.Model.Game.Free();
                    objFree.ID = reader.GetInt32(0);
                    objFree.Title = reader.GetString(1);
                    objFree.CclType = reader.GetInt32(2);
                    objFree.Price = reader.GetInt32(3);
                    listFrees.Add(objFree);
                }
            }
            return listFrees;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Free</returns>
        public IList<BCW.Model.Game.Free> GetFrees(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Free> listFrees = new List<BCW.Model.Game.Free>();
            string sTable = "tb_Free";
            string sPkey = "id";
            string sField = "ID,Title,cclType,Price";
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
                    return listFrees;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Free objFree = new BCW.Model.Game.Free();
                    objFree.ID = reader.GetInt32(0);
                    objFree.Title = reader.GetString(1);
                    objFree.CclType = reader.GetInt32(2);
                    objFree.Price = reader.GetInt32(3);
                    listFrees.Add(objFree);
                }
            }
            return listFrees;
        }

        #endregion  成员方法
    }
}

