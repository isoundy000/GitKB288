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
    /// 数据访问类Applepay。
    /// </summary>
    public class Applepay
    {
        public Applepay()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("Types", "tb_Applepay");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Types, int UsID, int AppleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applepay");
            strSql.Append(" where Types=@Types and UsID=@UsID and AppleId=@AppleId  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@AppleId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;
            parameters[2].Value = AppleId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int AppleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applepay");
            strSql.Append(" where AppleId=@AppleId ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@AppleId", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = AppleId;
            parameters[1].Value = 0;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Applepay");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            strSql.Append(" and WinCent>@WinCent ");
            strSql.Append(" and State=@State ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
        			new SqlParameter("@UsID", SqlDbType.Int,4),
    				new SqlParameter("@WinCent", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;
            parameters[2].Value = 0;
            parameters[3].Value = 1;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某ID在本局下注数
        /// </summary>
        public int GetCount(int UsID, int AppleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(PayCount) from tb_Applepay");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and AppleId=@AppleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@AppleId", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = AppleId;

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
        /// 计算本局某类型下注数
        /// </summary>
        public long GetCount2(int Types, int AppleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(PayCount) from tb_Applepay");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and AppleId=@AppleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AppleId", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = AppleId;

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
        public int Add(BCW.Model.Game.Applepay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Applepay(");
            strSql.Append("Types,AppleId,PayCount,UsID,UsName,WinCent,State,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@AppleId,@PayCount,@UsID,@UsName,@WinCent,@State,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AppleId", SqlDbType.Int,4),
					new SqlParameter("@PayCount", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.AppleId;
            parameters[2].Value = model.PayCount;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = 0;
            parameters[6].Value = 0;
            parameters[7].Value = model.AddTime;

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
        public void Update(BCW.Model.Game.Applepay model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applepay set ");
            strSql.Append("PayCount=@PayCount+PayCount,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where Types=@Types and AppleId=@AppleId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AppleId", SqlDbType.Int,4),
					new SqlParameter("@PayCount", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.AppleId;
            parameters[2].Value = model.PayCount;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applepay set ");
            strSql.Append("WinCent=@WinCent,");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@WinCent", SqlDbType.BigInt,8),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = WinCent;
            parameters[2].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int AppleId, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applepay set ");
            strSql.Append("State=@State");
            strSql.Append(" where AppleId=@AppleId and Types<>@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@AppleId", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = AppleId;
            parameters[1].Value = Types;
            parameters[2].Value = 1;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Applepay set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
				new SqlParameter("@ID", SqlDbType.Int,4),
                new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = 2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Applepay ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Applepay ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.Applepay GetApplepay(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,AppleId,UsID,UsName,AddTime from tb_Applepay ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            BCW.Model.Game.Applepay model = new BCW.Model.Game.Applepay();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.AppleId = reader.GetInt32(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.AddTime = reader.GetDateTime(5);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 WinCent from tb_Applepay ");
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
                        return reader.GetInt64(0);
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_Applepay ");
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
        /// <returns>IList Applepay</returns>
        public IList<BCW.Model.Game.Applepay> GetApplepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.Applepay> listApplepays = new List<BCW.Model.Game.Applepay>();
            string sTable = "tb_Applepay";
            string sPkey = "id";
            string sField = "ID,Types,AppleId,PayCount,UsID,UsName,WinCent,AddTime,State";
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
                    return listApplepays;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.Applepay objApplepay = new BCW.Model.Game.Applepay();
                    objApplepay.ID = reader.GetInt32(0);
                    objApplepay.Types = reader.GetInt32(1);
                    objApplepay.AppleId = reader.GetInt32(2);
                    objApplepay.PayCount = reader.GetInt32(3);
                    objApplepay.UsID = reader.GetInt32(4);
                    objApplepay.UsName = reader.GetString(5);
                    objApplepay.WinCent = reader.GetInt64(6);
                    objApplepay.AddTime = reader.GetDateTime(7);
                    objApplepay.State = reader.GetByte(8);
                    listApplepays.Add(objApplepay);
                }
            }
            return listApplepays;
        }

        #endregion  成员方法
    }
}

