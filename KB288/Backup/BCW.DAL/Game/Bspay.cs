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
    /// 数据访问类Bspay。
    /// </summary>
    public class Bspay
    {
        public Bspay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Bspay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Bspay");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件得到赢利值
        /// </summary>
        public long GetWinCent(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(WinCent) from tb_BsPay where " + strWhere + "");

            object obj = SqlHelper.GetSingle(strSql.ToString());
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.Bspay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Bspay(");
            strSql.Append("BsId,BsTitle,UsID,UsName,BetType,PayCent,WinCent,BzType,AddTime,UsIP,UsUA)");
            strSql.Append(" values (");
            strSql.Append("@BsId,@BsTitle,@UsID,@UsName,@BetType,@PayCent,@WinCent,@BzType,@AddTime,@UsIP,@UsUA)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@BsId", SqlDbType.Int,4),
					new SqlParameter("@BsTitle", SqlDbType.NVarChar,50),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BetType", SqlDbType.TinyInt,1),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@UsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@UsUA", SqlDbType.NVarChar,200)};

            parameters[0].Value = model.BsId;
            parameters[1].Value = model.BsTitle;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BetType;
            parameters[5].Value = model.PayCent;
            parameters[6].Value = model.WinCent;
            parameters[7].Value = model.BzType;
            parameters[8].Value = model.AddTime;
            parameters[9].Value = Utils.GetUsIP();
            parameters[10].Value = Utils.GetUA();

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
        public void Update(BCW.Model.Game.Bspay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Bspay set ");
            strSql.Append("BsId=@BsId,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("BetType=@BetType,");
            strSql.Append("PayCent=@PayCent,");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BsId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@BetType", SqlDbType.TinyInt,1),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BsId;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.BetType;
            parameters[5].Value = model.PayCent;
            parameters[6].Value = model.WinCent;
            parameters[7].Value = model.BzType;
            parameters[8].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Bspay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Bspay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Bspay GetBspay(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,BsId,UsID,UsName,BetType,PayCent,WinCent,BzType,AddTime from tb_Bspay ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Game.Bspay model = new BCW.Model.Game.Bspay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.BsId = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.BetType = reader.GetByte(4);
                    model.PayCent = reader.GetInt64(5);
                    model.WinCent = reader.GetInt64(6);
                    model.BzType = reader.GetInt32(7);
                    model.AddTime = reader.GetDateTime(8);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到上一个下注记录实体
        /// </summary>
        public BCW.Model.Game.Bspay GetBspayBf(int UsID, int BsId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 BetType,PayCent from tb_Bspay ");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and BsId=@BsId ");
            strSql.Append(" Order By ID DESC ");
            SqlParameter[] parameters = {
					new SqlParameter("@BsID", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4)};
            parameters[0].Value = BsId;
            parameters[1].Value = UsID;

            BCW.Model.Game.Bspay model = new BCW.Model.Game.Bspay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.BetType = reader.GetByte(0);
                    model.PayCent = reader.GetInt64(1);
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
            strSql.Append(" FROM tb_Bspay ");
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
        /// <returns>IList Bspay</returns>
        public IList<BCW.Model.Game.Bspay> GetBspays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Bspay> listBspays = new List<BCW.Model.Game.Bspay>();
            string sTable = "tb_Bspay";
            string sPkey = "id";
            string sField = "BsId,BsTitle,UsID,UsName,PayCent,WinCent,BzType,AddTime,BetType,UsIP,UsUA";
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
                    return listBspays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Bspay objBspay = new BCW.Model.Game.Bspay();
                    objBspay.BsId = reader.GetInt32(0);
                    objBspay.BsTitle = reader.GetString(1);
                    objBspay.UsID = reader.GetInt32(2);
                    objBspay.UsName = reader.GetString(3);
                    objBspay.PayCent = reader.GetInt64(4);
                    objBspay.WinCent = reader.GetInt64(5);
                    objBspay.BzType = reader.GetByte(6);
                    objBspay.AddTime = reader.GetDateTime(7);
                    objBspay.BetType = reader.GetByte(8);
                    if(!reader.IsDBNull(9))
                        objBspay.UsIP = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        objBspay.UsUA = reader.GetString(10);

                    listBspays.Add(objBspay);
                }
            }
            return listBspays;
        }


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Bspay</returns>
        public IList<BCW.Model.Game.Bspay> GetBspaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Bspay> listBspayTop = new List<BCW.Model.Game.Bspay>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_Bspay where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 0)
            {
                if (p_recordCount > 50)
                    p_recordCount = 50;

                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listBspayTop;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT TOP 50 UsID,sum(WinCent) as WinCents FROM tb_Bspay where " + strWhere + " group by UsID Order by sum(WinCent) desc";
            using (SqlDataReader reader = SqlHelper.ExecuteReader(queryString))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Model.Game.Bspay objBspay = new BCW.Model.Game.Bspay();
                        objBspay.UsID = reader.GetInt32(0);
                        objBspay.WinCent = reader.GetInt64(1);
                        listBspayTop.Add(objBspay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listBspayTop;
        }

        #endregion  成员方法
    }
}

