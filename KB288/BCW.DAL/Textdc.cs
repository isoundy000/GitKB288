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
    /// 数据访问类Textdc。
    /// </summary>
    public class Textdc
    {
        public Textdc()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Textdc");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Textdc");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在未结束的闲家记录
        /// </summary>
        public bool Exists2(int BID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Textdc");
            strSql.Append(" where BID=@BID and IsZtid=1 and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4)};
            parameters[0].Value = BID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某帖子竞猜的庄/闲家保证金总额
        /// </summary>
        public long GetCents(int BID, int IsZtid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(OutCent) from tb_Textdc");
            strSql.Append(" where BID=@BID and IsZtid=@IsZtid and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@IsZtid", SqlDbType.BigInt,8)};
            parameters[0].Value = BID;
            parameters[1].Value = IsZtid;
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
        public int Add(BCW.Model.Textdc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Textdc(");
            strSql.Append("BID,UsID,OutCent,IsZtid,BzType,State,AddTime,LogText)");
            strSql.Append(" values (");
            strSql.Append("@BID,@UsID,@OutCent,@IsZtid,@BzType,@State,@AddTime,@LogText)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@OutCent", SqlDbType.BigInt,8),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@LogText", SqlDbType.NText)};
            parameters[0].Value = model.BID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.OutCent;
            parameters[3].Value = model.IsZtid;
            parameters[4].Value = model.BzType;
            parameters[5].Value = model.State;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.LogText;

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
        public void Update(BCW.Model.Textdc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textdc set ");
            strSql.Append("BID=@BID,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("OutCent=@OutCent,");
            strSql.Append("IsZtid=@IsZtid,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@OutCent", SqlDbType.BigInt,8),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.BID;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.OutCent;
            parameters[4].Value = model.IsZtid;
            parameters[5].Value = model.BzType;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOutCent(BCW.Model.Textdc model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textdc set ");
            strSql.Append("OutCent=OutCent+@OutCent,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where BID=@BID and IsZtid=@IsZtid and UsID=@UsID and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@OutCent", SqlDbType.BigInt,8),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.BID;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.OutCent;
            parameters[3].Value = model.IsZtid;
            parameters[4].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textdc set ");
            strSql.Append("State=@State");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1)};
            parameters[0].Value = ID;
            parameters[1].Value = State;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateState(int ID, int State, long AcCent)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textdc set ");
            strSql.Append("State=@State,");
            strSql.Append("AcCent=@AcCent");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@AcCent", SqlDbType.BigInt,8)};
            parameters[0].Value = ID;
            parameters[1].Value = State;
            parameters[2].Value = AcCent;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 记录日志
        /// </summary>
        public void UpdateLogText(int BID, int UsID, string LogText)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Textdc set ");
            strSql.Append("LogText=@LogText");
            strSql.Append(" where BID=@BID and UsID=@UsID and IsZtid=0");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@LogText", SqlDbType.NText)};
            parameters[0].Value = BID;
            parameters[1].Value = UsID;
            parameters[2].Value = LogText;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Textdc ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,BID,UsID,OutCent,IsZtid,BzType,State,AddTime,AcCent from tb_Textdc ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Textdc model = new BCW.Model.Textdc();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.BID = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.OutCent = reader.GetInt64(3);
                    model.IsZtid = reader.GetByte(4);
                    model.BzType = reader.GetByte(5);
                    model.State = reader.GetByte(6);
                    model.AddTime = reader.GetDateTime(7);
                    model.AcCent = reader.GetInt64(8);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc2(int ID, int BID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,OutCent,BzType,State,AddTime,AcCent from tb_Textdc ");
            strSql.Append(" where ID=@ID and BID=@BID  and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@BID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = BID;

            BCW.Model.Textdc model = new BCW.Model.Textdc();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.OutCent = reader.GetInt64(2);
                    model.BzType = reader.GetByte(3);
                    model.State = reader.GetByte(4);
                    model.AddTime = reader.GetDateTime(5);
                    model.AcCent = reader.GetInt64(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,OutCent,BzType,State,AddTime,LogText,AcCent from tb_Textdc ");
            strSql.Append(" where BID=@BID and IsZtid=@IsZtid  and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1)};
            parameters[0].Value = BID;
            parameters[1].Value = IsZtid;

            BCW.Model.Textdc model = new BCW.Model.Textdc();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.OutCent = reader.GetInt64(2);
                    model.BzType = reader.GetByte(3);
                    model.State = reader.GetByte(4);
                    model.AddTime = reader.GetDateTime(5);
                    if (!reader.IsDBNull(6))
                        model.LogText = reader.GetString(6);
                    else
                        model.LogText = "";
                    model.AcCent = reader.GetInt64(7);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Textdc GetTextdc(int BID, int IsZtid, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,OutCent,BzType,State,AddTime,LogText,UsID,AcCent from tb_Textdc ");
            strSql.Append(" where BID=@BID and IsZtid=@IsZtid and UsID=@UsID  and State<>3");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = BID;
            parameters[1].Value = IsZtid;
            parameters[2].Value = UsID;

            BCW.Model.Textdc model = new BCW.Model.Textdc();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.OutCent = reader.GetInt64(1);
                    model.BzType = reader.GetByte(2);
                    model.State = reader.GetByte(3);
                    model.AddTime = reader.GetDateTime(4);
                    if (!reader.IsDBNull(5))
                        model.LogText = reader.GetString(5);
                    else
                        model.LogText = "";

                    model.UsID = reader.GetInt32(6);
                    model.AcCent = reader.GetInt64(7);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public string GetLogText(int BID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 LogText from tb_Textdc ");
            strSql.Append(" where BID=@BID and IsZtid=@IsZtid");
            SqlParameter[] parameters = {
					new SqlParameter("@BID", SqlDbType.Int,4),
					new SqlParameter("@IsZtid", SqlDbType.TinyInt,1)};
            parameters[0].Value = BID;
            parameters[1].Value = 0;

            BCW.Model.Textdc model = new BCW.Model.Textdc();
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
            strSql.Append(" FROM tb_Textdc ");
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
        /// <returns>IList Textdc</returns>
        public IList<BCW.Model.Textdc> GetTextdcs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Textdc> listTextdcs = new List<BCW.Model.Textdc>();
            string sTable = "tb_Textdc";
            string sPkey = "id";
            string sField = "ID,BID,UsID,OutCent,AcCent,IsZtid,BzType,State,AddTime";
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
                    return listTextdcs;
                }
                while (reader.Read())
                {
                    BCW.Model.Textdc objTextdc = new BCW.Model.Textdc();
                    objTextdc.ID = reader.GetInt32(0);
                    objTextdc.BID = reader.GetInt32(1);
                    objTextdc.UsID = reader.GetInt32(2);
                    objTextdc.OutCent = reader.GetInt64(3);
                    objTextdc.AcCent = reader.GetInt64(4);
                    objTextdc.IsZtid = reader.GetByte(5);
                    objTextdc.BzType = reader.GetByte(6);
                    objTextdc.State = reader.GetByte(7);
                    objTextdc.AddTime = reader.GetDateTime(8);
                    listTextdcs.Add(objTextdc);
                }
            }
            return listTextdcs;
        }

        #endregion  成员方法
    }
}

