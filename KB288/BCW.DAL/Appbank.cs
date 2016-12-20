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
    /// 数据访问类Appbank。
    /// </summary>
    public class Appbank
    {
        public Appbank()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Appbank");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Appbank");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Appbank model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Appbank(");
            strSql.Append("Types,AddGold,UsID,UsName,Mobile,CardNum,CardName,CardAddress,Notes,State,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@AddGold,@UsID,@UsName,@Mobile,@CardNum,@CardName,@CardAddress,@Notes,@State,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@AddGold", SqlDbType.BigInt,8),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@CardNum", SqlDbType.NVarChar,50),
					new SqlParameter("@CardName", SqlDbType.NVarChar,50),
					new SqlParameter("@CardAddress", SqlDbType.NVarChar,100),
					new SqlParameter("@Notes", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.AddGold;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Mobile;
            parameters[5].Value = model.CardNum;
            parameters[6].Value = model.CardName;
            parameters[7].Value = model.CardAddress;
            parameters[8].Value = model.Notes;
            parameters[9].Value = model.State;
            parameters[10].Value = model.AddTime;

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
        public void Update(BCW.Model.Appbank model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Appbank set ");
            strSql.Append("AdminNotes=@AdminNotes,");
            strSql.Append("State=@State,");
            strSql.Append("ReTime=@ReTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@AdminNotes", SqlDbType.NVarChar,50),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@ReTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.AdminNotes;
            parameters[2].Value = model.State;
            parameters[3].Value = model.ReTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Appbank ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Appbank GetAppbank(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,AddGold,UsID,UsName,Mobile,CardNum,CardName,CardAddress,Notes,AdminNotes,State,AddTime,ReTime from tb_Appbank ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Appbank model = new BCW.Model.Appbank();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.AddGold = reader.GetInt64(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Mobile = reader.GetString(5);
                    model.CardNum = reader.GetString(6);
                    model.CardName = reader.GetString(7);
                    model.CardAddress = reader.GetString(8);
                    model.Notes = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        model.AdminNotes = reader.GetString(10);
                    model.State = reader.GetByte(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.ReTime = reader.GetDateTime(13);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        /// <summary>
        /// 得到某会员上一次交易对象实体
        /// </summary>
        public BCW.Model.Appbank GetAppbankLast(int Types, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,AddGold,UsID,UsName,Mobile,CardNum,CardName,CardAddress,Notes,AdminNotes,State,AddTime,ReTime from tb_Appbank ");
            strSql.Append(" where Types=@Types and UsID=@UsID Order BY id desc");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = UsID;

            BCW.Model.Appbank model = new BCW.Model.Appbank();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.AddGold = reader.GetInt64(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.Mobile = reader.GetString(5);
                    model.CardNum = reader.GetString(6);
                    model.CardName = reader.GetString(7);
                    model.CardAddress = reader.GetString(8);
                    model.Notes = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        model.AdminNotes = reader.GetString(10);
                    model.State = reader.GetByte(11);
                    model.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        model.ReTime = reader.GetDateTime(13);
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
            strSql.Append(" FROM tb_Appbank ");
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
        /// <returns>IList Appbank</returns>
        public IList<BCW.Model.Appbank> GetAppbanks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Appbank> listAppbanks = new List<BCW.Model.Appbank>();
            string sTable = "tb_Appbank";
            string sPkey = "id";
            string sField = "ID,Types,AddGold,UsID,UsName,Mobile,CardNum,CardName,CardAddress,Notes,AdminNotes,State,AddTime,ReTime";
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
                    return listAppbanks;
                }
                while (reader.Read())
                {
                    BCW.Model.Appbank objAppbank = new BCW.Model.Appbank();
                    objAppbank.ID = reader.GetInt32(0);
                    objAppbank.Types = reader.GetInt32(1);
                    objAppbank.AddGold = reader.GetInt64(2);
                    objAppbank.UsID = reader.GetInt32(3);
                    objAppbank.UsName = reader.GetString(4);
                    objAppbank.Mobile = reader.GetString(5);
                    objAppbank.CardNum = reader.GetString(6);
                    objAppbank.CardName = reader.GetString(7);
                    objAppbank.CardAddress = reader.GetString(8);
                    objAppbank.Notes = reader.GetString(9);
                    if (!reader.IsDBNull(10))
                        objAppbank.AdminNotes = reader.GetString(10);
                    objAppbank.State = reader.GetByte(11);
                    objAppbank.AddTime = reader.GetDateTime(12);
                    if (!reader.IsDBNull(13))
                        objAppbank.ReTime = reader.GetDateTime(13);
                    listAppbanks.Add(objAppbank);
                }
            }
            return listAppbanks;
        }

        #endregion  成员方法
    }
}

