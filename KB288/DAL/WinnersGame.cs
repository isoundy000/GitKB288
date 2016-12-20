using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类tb_WinnersGame。
    /// </summary>
    public class tb_WinnersGame
    {
        public tb_WinnersGame()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_WinnersGame");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsGameName(string GameName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_WinnersGame");
            strSql.Append(" where GameName=@GameName ");
            SqlParameter[] parameters = {
                  new SqlParameter("@GameName", SqlDbType.NVarChar,50)};
            parameters[0].Value = GameName;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_WinnersGame");
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_WinnersGame model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_WinnersGame(");
            strSql.Append("GameName,price,ptype,Ident)");
            strSql.Append(" values (");
            strSql.Append("@GameName,@price,@ptype,@Ident)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@GameName", SqlDbType.NVarChar,50),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@ptype", SqlDbType.Int,4),
                    new SqlParameter("@Ident", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.GameName;
            parameters[1].Value = model.price;
            parameters[2].Value = model.ptype;
            parameters[3].Value = model.Ident;

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
        /// 通过Ident更新
        /// price的值
        /// /// </summary>
        public void UpdatePriceForIdent(string Ident, long price)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersGame set ");
            strSql.Append("price=@price ");
            strSql.Append(" where Ident=@Ident ");
            SqlParameter[] parameters = {
                  new SqlParameter("@Ident", SqlDbType.NVarChar,50),
                    new SqlParameter("@price", SqlDbType.BigInt,8)};
            parameters[0].Value = Ident;
            parameters[1].Value = price;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 通过Ident更新
        /// price,ptype的值
        /// /// </summary>
        public void UpdatePricePtypeForIdent(string Ident, long price, int ptype)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersGame set ");
            strSql.Append("price=@price ,");
            strSql.Append("ptype=@ptype ");
            strSql.Append(" where Ident=@Ident ");
            SqlParameter[] parameters = {
                  new SqlParameter("@Ident", SqlDbType.NVarChar,50),
                  new SqlParameter("@price", SqlDbType.BigInt,8),
                  new SqlParameter("@ptype", SqlDbType.Int, 4)};
            parameters[0].Value = Ident;
            parameters[1].Value = price;
            parameters[2].Value = ptype;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 通过Id更新
        /// price的值
        /// /// </summary>
        public void UpdatePrice(int ID, long price)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersGame set ");
            strSql.Append("price=@price ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@price", SqlDbType.BigInt,8)};

            parameters[0].Value = ID;
            parameters[1].Value = price;

            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.tb_WinnersGame model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_WinnersGame set ");
            strSql.Append("GameName=@GameName,");
            strSql.Append("price=@price,");
            strSql.Append("ptype=@ptype,");
            strSql.Append("Ident=@Ident");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@GameName", SqlDbType.NVarChar,50),
                    new SqlParameter("@price", SqlDbType.BigInt,8),
                    new SqlParameter("@ptype", SqlDbType.Int,4),
                    new SqlParameter("@Ident", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.GameName;
            parameters[2].Value = model.price;
            parameters[3].Value = model.ptype;
            parameters[4].Value = model.Ident;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_WinnersGame ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_WinnersGame Gettb_WinnersGame(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,GameName,price,ptype,Ident from tb_WinnersGame ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.tb_WinnersGame model = new BCW.Model.tb_WinnersGame();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.GameName = reader.GetString(1);
                    model.price = reader.GetInt64(2);
                    model.ptype = reader.GetInt32(3);
                    model.Ident = reader.GetString(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到最低下注
        /// </summary>
        public long GetPrice(string GameName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select price from tb_WinnersGame ");
            strSql.Append(" where GameName=@GameName ");
            SqlParameter[] parameters = {
                  new SqlParameter("@GameName", SqlDbType.NVarChar,50)};
            parameters[0].Value = GameName;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到最低下注
        /// </summary>
        public int GetPtype(string GameName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ptype from tb_WinnersGame ");
            strSql.Append(" where GameName=@GameName ");
            SqlParameter[] parameters = {
                  new SqlParameter("@GameName", SqlDbType.NVarChar,50)};
            parameters[0].Value = GameName;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
            strSql.Append(" FROM tb_WinnersGame ");
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
        /// <returns>IList tb_WinnersGame</returns>
        public IList<BCW.Model.tb_WinnersGame> Gettb_WinnersGames(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_WinnersGame> listtb_WinnersGames = new List<BCW.Model.tb_WinnersGame>();
            string sTable = "tb_WinnersGame";
            string sPkey = "id";
            string sField = "ID,GameName,price,ptype,Ident";
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
                    return listtb_WinnersGames;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_WinnersGame objtb_WinnersGame = new BCW.Model.tb_WinnersGame();
                    objtb_WinnersGame.ID = reader.GetInt32(0);
                    objtb_WinnersGame.GameName = reader.GetString(1);
                    objtb_WinnersGame.price = reader.GetInt64(2);
                    objtb_WinnersGame.ptype = reader.GetInt32(3);
                    objtb_WinnersGame.Ident = reader.GetString(4);
                    listtb_WinnersGames.Add(objtb_WinnersGame);
                }
            }
            return listtb_WinnersGames;
        }

        #endregion  成员方法
    }
}

