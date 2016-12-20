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
    /// 数据访问类:BaccaratDiary
    /// </summary>
    public partial class BaccaratDiary
    {
        public BaccaratDiary()
        { }
        #region  BasicMethod

        /// <summary>
        /// 清楚表记录
        /// </summary>
        /// <param name="TableName"></param>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Baccarat.Model.BaccaratDiary model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_BaccaratDiary(");
            strSql.Append("UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoTitle,RoomDoAnnouces,BetMoney,BetTypes,BankerPoker,BankerPoint,HunterPoker,HunterPoint,actid,updatetime,BonusMoney)");
            strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@RoomID,@RoomDoName,@RoomDoTable,@RoomDoTotal,@RoomDoAdd,@RoomDoTitle,@RoomDoAnnouces,@BetMoney,@BetTypes,@BankerPoker,@BankerPoint,@HunterPoker,@HunterPoint,@actid,@updatetime,@BonusMoney)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAdd", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTitle", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.VarChar,50),
					new SqlParameter("@BetMoney", SqlDbType.Int,4),
					new SqlParameter("@BetTypes", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoint", SqlDbType.Int,4),
					new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
					new SqlParameter("@HunterPoint", SqlDbType.Int,4),
					new SqlParameter("@actid", SqlDbType.Int,4),
					new SqlParameter("@updatetime", SqlDbType.DateTime),
					new SqlParameter("@BonusMoney", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.RoomDoName;
            parameters[4].Value = model.RoomDoTable;
            parameters[5].Value = model.RoomDoTotal;
            parameters[6].Value = model.RoomDoAdd;
            parameters[7].Value = model.RoomDoTitle;
            parameters[8].Value = model.RoomDoAnnouces;
            parameters[9].Value = model.BetMoney;
            parameters[10].Value = model.BetTypes;
            parameters[11].Value = model.BankerPoker;
            parameters[12].Value = model.BankerPoint;
            parameters[13].Value = model.HunterPoker;
            parameters[14].Value = model.HunterPoint;
            parameters[15].Value = model.actid;
            parameters[16].Value = model.updatetime;
            parameters[17].Value = model.BonusMoney;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(BCW.Baccarat.Model.BaccaratDiary model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_BaccaratDiary set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("RoomID=@RoomID,");
            strSql.Append("RoomDoName=@RoomDoName,");
            strSql.Append("RoomDoTable=@RoomDoTable,");
            strSql.Append("RoomDoTotal=@RoomDoTotal,");
            strSql.Append("RoomDoAdd=@RoomDoAdd,");
            strSql.Append("RoomDoTitle=@RoomDoTitle,");
            strSql.Append("RoomDoAnnouces=@RoomDoAnnouces,");
            strSql.Append("BetMoney=@BetMoney,");
            strSql.Append("BetTypes=@BetTypes,");
            strSql.Append("BankerPoker=@BankerPoker,");
            strSql.Append("BankerPoint=@BankerPoint,");
            strSql.Append("HunterPoker=@HunterPoker,");
            strSql.Append("HunterPoint=@HunterPoint,");
            strSql.Append("actid=@actid,");
            strSql.Append("updatetime=@updatetime,");
            strSql.Append("BonusMoney=@BonusMoney");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomID", SqlDbType.Int,4),
					new SqlParameter("@RoomDoName", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoTable", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTotal", SqlDbType.Int,4),
					new SqlParameter("@RoomDoAdd", SqlDbType.Int,4),
					new SqlParameter("@RoomDoTitle", SqlDbType.VarChar,50),
					new SqlParameter("@RoomDoAnnouces", SqlDbType.VarChar,50),
					new SqlParameter("@BetMoney", SqlDbType.Int,4),
					new SqlParameter("@BetTypes", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoker", SqlDbType.VarChar,50),
					new SqlParameter("@BankerPoint", SqlDbType.Int,4),
					new SqlParameter("@HunterPoker", SqlDbType.VarChar,50),
					new SqlParameter("@HunterPoint", SqlDbType.Int,4),
					new SqlParameter("@actid", SqlDbType.Int,4),
					new SqlParameter("@updatetime", SqlDbType.DateTime),
					new SqlParameter("@BonusMoney", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.UsName;
            parameters[2].Value = model.RoomID;
            parameters[3].Value = model.RoomDoName;
            parameters[4].Value = model.RoomDoTable;
            parameters[5].Value = model.RoomDoTotal;
            parameters[6].Value = model.RoomDoAdd;
            parameters[7].Value = model.RoomDoTitle;
            parameters[8].Value = model.RoomDoAnnouces;
            parameters[9].Value = model.BetMoney;
            parameters[10].Value = model.BetTypes;
            parameters[11].Value = model.BankerPoker;
            parameters[12].Value = model.BankerPoint;
            parameters[13].Value = model.HunterPoker;
            parameters[14].Value = model.HunterPoint;
            parameters[15].Value = model.actid;
            parameters[16].Value = model.updatetime;
            parameters[17].Value = model.BonusMoney;
            parameters[18].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratDiary ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_BaccaratDiary ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratDiary GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoTitle,RoomDoAnnouces,BetMoney,BetTypes,BankerPoker,BankerPoint,HunterPoker,HunterPoint,actid,updatetime,BonusMoney from tb_BaccaratDiary ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            BCW.Baccarat.Model.BaccaratDiary model = new BCW.Baccarat.Model.BaccaratDiary();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Baccarat.Model.BaccaratDiary DataRowToModel(DataRow row)
        {
            BCW.Baccarat.Model.BaccaratDiary model = new BCW.Baccarat.Model.BaccaratDiary();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["UsID"] != null && row["UsID"].ToString() != "")
                {
                    model.UsID = int.Parse(row["UsID"].ToString());
                }
                if (row["UsName"] != null)
                {
                    model.UsName = row["UsName"].ToString();
                }
                if (row["RoomID"] != null && row["RoomID"].ToString() != "")
                {
                    model.RoomID = int.Parse(row["RoomID"].ToString());
                }
                if (row["RoomDoName"] != null)
                {
                    model.RoomDoName = row["RoomDoName"].ToString();
                }
                if (row["RoomDoTable"] != null && row["RoomDoTable"].ToString() != "")
                {
                    model.RoomDoTable = int.Parse(row["RoomDoTable"].ToString());
                }
                if (row["RoomDoTotal"] != null && row["RoomDoTotal"].ToString() != "")
                {
                    model.RoomDoTotal = int.Parse(row["RoomDoTotal"].ToString());
                }
                if (row["RoomDoAdd"] != null && row["RoomDoAdd"].ToString() != "")
                {
                    model.RoomDoAdd = int.Parse(row["RoomDoAdd"].ToString());
                }
                if (row["RoomDoTitle"] != null)
                {
                    model.RoomDoTitle = row["RoomDoTitle"].ToString();
                }
                if (row["RoomDoAnnouces"] != null)
                {
                    model.RoomDoAnnouces = row["RoomDoAnnouces"].ToString();
                }
                if (row["BetMoney"] != null && row["BetMoney"].ToString() != "")
                {
                    model.BetMoney = int.Parse(row["BetMoney"].ToString());
                }
                if (row["BetTypes"] != null)
                {
                    model.BetTypes = row["BetTypes"].ToString();
                }
                if (row["BankerPoker"] != null)
                {
                    model.BankerPoker = row["BankerPoker"].ToString();
                }
                if (row["BankerPoint"] != null && row["BankerPoint"].ToString() != "")
                {
                    model.BankerPoint = int.Parse(row["BankerPoint"].ToString());
                }
                if (row["HunterPoker"] != null)
                {
                    model.HunterPoker = row["HunterPoker"].ToString();
                }
                if (row["HunterPoint"] != null && row["HunterPoint"].ToString() != "")
                {
                    model.HunterPoint = int.Parse(row["HunterPoint"].ToString());
                }
                if (row["actid"] != null && row["actid"].ToString() != "")
                {
                    model.actid = int.Parse(row["actid"].ToString());
                }
                if (row["updatetime"] != null && row["updatetime"].ToString() != "")
                {
                    model.updatetime = DateTime.Parse(row["updatetime"].ToString());
                }
                if (row["BonusMoney"] != null && row["BonusMoney"].ToString() != "")
                {
                    model.BonusMoney = int.Parse(row["BonusMoney"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoTitle,RoomDoAnnouces,BetMoney,BetTypes,BankerPoker,BankerPoint,HunterPoker,HunterPoint,actid,updatetime,BonusMoney ");
            strSql.Append(" FROM tb_BaccaratDiary ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,UsID,UsName,RoomID,RoomDoName,RoomDoTable,RoomDoTotal,RoomDoAdd,RoomDoTitle,RoomDoAnnouces,BetMoney,BetTypes,BankerPoker,BankerPoint,HunterPoker,HunterPoint,actid,updatetime,BonusMoney ");
            strSql.Append(" FROM tb_BaccaratDiary ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_BaccaratDiary ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = SqlHelper.GetSingle(strSql.ToString());
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
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_BaccaratDiary T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList BaccaratDiary</returns>
        public IList<BCW.Baccarat.Model.BaccaratDiary> GetBaccaratDiarys(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Baccarat.Model.BaccaratDiary> listBaccaratDiary = new List<BCW.Baccarat.Model.BaccaratDiary>();
            string sTable = "tb_BaccaratDiary";
            string sPkey = "ID";
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
                    return listBaccaratDiary;
                }
                while (reader.Read())
                {
                    BCW.Baccarat.Model.BaccaratDiary objBaccaratDiary = new BCW.Baccarat.Model.BaccaratDiary();
                    objBaccaratDiary.ID = reader.GetInt32(0);
                    objBaccaratDiary.UsID = reader.GetInt32(1);
                    objBaccaratDiary.UsName = reader.GetString(2);
                    objBaccaratDiary.RoomID = reader.GetInt32(3);
                    objBaccaratDiary.RoomDoName = reader.GetString(4);
                    objBaccaratDiary.RoomDoTable = reader.GetInt32(5);
                    objBaccaratDiary.RoomDoTotal = reader.GetInt32(6);
                    objBaccaratDiary.RoomDoAdd = reader.GetInt32(7);
                    objBaccaratDiary.RoomDoTitle = reader.GetString(8);
                    objBaccaratDiary.RoomDoAnnouces = reader.GetString(9);
                    objBaccaratDiary.BetMoney = reader.GetInt32(10);
                    objBaccaratDiary.BetTypes = reader.GetString(11);
                    objBaccaratDiary.BankerPoker = reader.GetString(12);
                    objBaccaratDiary.BankerPoint = reader.GetInt32(13);
                    objBaccaratDiary.HunterPoker = reader.GetString(14);
                    objBaccaratDiary.HunterPoint = reader.GetInt32(15);
                    objBaccaratDiary.actid = reader.GetInt32(16);
                    objBaccaratDiary.updatetime = reader.GetDateTime(17);
                    objBaccaratDiary.BonusMoney = reader.GetInt32(18);
                    listBaccaratDiary.Add(objBaccaratDiary);
                }
            }
            return listBaccaratDiary;
        }

        /*
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetList(int PageSize,int PageIndex,string strWhere)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@tblName", SqlDbType.VarChar, 255),
                    new SqlParameter("@fldName", SqlDbType.VarChar, 255),
                    new SqlParameter("@PageSize", SqlDbType.Int),
                    new SqlParameter("@PageIndex", SqlDbType.Int),
                    new SqlParameter("@IsReCount", SqlDbType.Bit),
                    new SqlParameter("@OrderType", SqlDbType.Bit),
                    new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
                    };
            parameters[0].Value = "tb_BaccaratDiary";
            parameters[1].Value = "ID";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}

