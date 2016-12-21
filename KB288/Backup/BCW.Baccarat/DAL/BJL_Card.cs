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
    /// 数据访问类BJL_Card。
    /// </summary>
    public class BJL_Card
    {
        public BJL_Card()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_BJL_Card");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Card");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BJL_Card model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BJL_Card(");
            strSql.Append("RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint)");
            strSql.Append(" values (");
            strSql.Append("@RoomID,@RoomTable,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4)};
            parameters[0].Value = model.RoomID;
            parameters[1].Value = model.RoomTable;
            parameters[2].Value = model.BankerPoker;
            parameters[3].Value = model.BankerPoint;
            parameters[4].Value = model.HunterPoker;
            parameters[5].Value = model.HunterPoint;

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
        public void Update(BCW.Baccarat.Model.BJL_Card model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BJL_Card set ");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomTable=@RoomTable,");
            strSql.Append("BankerPoker=@BankerPoker,");
            strSql.Append("BankerPoint=@BankerPoint,");
            strSql.Append("HunterPoker=@HunterPoker,");
            strSql.Append("HunterPoint=@HunterPoint");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4),
                    new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@BankerPoint", SqlDbType.Int,4),
                    new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
                    new SqlParameter("@HunterPoint", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.RoomID;
            parameters[2].Value = model.RoomTable;
            parameters[3].Value = model.BankerPoker;
            parameters[4].Value = model.BankerPoint;
            parameters[5].Value = model.HunterPoker;
            parameters[6].Value = model.HunterPoint;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BJL_Card ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetBJL_Card(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint from tb_BJL_Card ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BJL_Card model = new BCW.Baccarat.Model.BJL_Card();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomTable = reader.GetInt32(2);
                    model.BankerPoker = reader.GetString(3);
                    model.BankerPoint = reader.GetInt32(4);
                    model.HunterPoker = reader.GetString(5);
                    model.HunterPoint = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_BJL_Card ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //===========================================

        /// <summary>
        /// 判断是否存在某房间的桌面的扑克牌
        /// </summary>
        /// <param name="RoomID"></param>
        /// <param name="RoomDoTable"></param>
        /// <returns></returns>
        public bool ExistsCard(int RoomID, int RoomTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_BJL_Card");
            strSql.Append(" where RoomID=@RoomID and RoomTable=@RoomTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomTable;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///得到特定房间ID和桌面table的最新的数据
        /// </summary>
        public BCW.Baccarat.Model.BJL_Card GetCardMessage(int RoomID, int RoomTable)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top(1)* from tb_BJL_Card ");
            strSql.Append("where RoomID=@RoomID and RoomTable=@RoomTable");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoomID", SqlDbType.Int,4),
                    new SqlParameter("@RoomTable", SqlDbType.Int,4)};
            parameters[0].Value = RoomID;
            parameters[1].Value = RoomTable;
            BCW.Baccarat.Model.BJL_Card model = new BCW.Baccarat.Model.BJL_Card();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.RoomID = reader.GetInt32(1);
                    model.RoomTable = reader.GetInt32(2);
                    model.BankerPoker = reader.GetString(3);
                    model.BankerPoint = reader.GetInt32(4);
                    model.HunterPoker = reader.GetString(5);
                    model.HunterPoint = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    model.ID = 0;
                    return model;
                }
            }
        }
        //==========================================


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BJL_Card</returns>
        public IList<BCW.Baccarat.Model.BJL_Card> GetBJL_Cards(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BJL_Card> listBJL_Cards = new List<BCW.Baccarat.Model.BJL_Card>();
            string sTable = "tb_BJL_Card";
            string sPkey = "id";
            string sField = "ID,RoomID,RoomTable,BankerPoker,BankerPoint,HunterPoker,HunterPoint";
            string sCondition = strWhere;
            string sOrder = "RoomTable Desc";
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
                    return listBJL_Cards;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BJL_Card objBJL_Card = new BCW.Baccarat.Model.BJL_Card();
                    objBJL_Card.ID = reader.GetInt32(0);
                    objBJL_Card.RoomID = reader.GetInt32(1);
                    objBJL_Card.RoomTable = reader.GetInt32(2);
                    objBJL_Card.BankerPoker = reader.GetString(3);
                    objBJL_Card.BankerPoint = reader.GetInt32(4);
                    objBJL_Card.HunterPoker = reader.GetString(5);
                    objBJL_Card.HunterPoint = reader.GetInt32(6);
                    listBJL_Cards.Add(objBJL_Card);
                }
            }
            return listBJL_Cards;
        }

        #endregion  成员方法
    }
}

