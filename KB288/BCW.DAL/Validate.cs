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
    /// 数据访问类tb_Validate。
    /// </summary>
    public class tb_Validate
    {
        public tb_Validate()
        { }
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Validate");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// type 1 注册，2 修改密码 
        /// </summary>
        public bool ExistsPhone(string Phone, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Validate");
            strSql.Append(" where Phone=@Phone and type=@type ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = Phone;
            parameters[1].Value = type;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsPhone(string Phone)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Validate");
            strSql.Append(" where Phone=@Phone");
            SqlParameter[] parameters = {
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50)};
            parameters[0].Value = Phone;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.tb_Validate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Validate(");
            strSql.Append("Phone,IP,Time,Flag,codeTime,mesCode,type)");
            strSql.Append(" values (");
            strSql.Append("@Phone,@IP,@Time,@Flag,@codeTime,@mesCode,@type)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@IP", SqlDbType.NVarChar,50),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@Flag", SqlDbType.Int,4),
                    new SqlParameter("@codeTime", SqlDbType.DateTime),
                    new SqlParameter("@mesCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.Phone;
            parameters[1].Value = model.IP;
            parameters[2].Value = model.Time;
            parameters[3].Value = model.Flag;
            parameters[4].Value = model.codeTime;
            parameters[5].Value = model.mesCode;
            parameters[6].Value = model.type;

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
        public void UpdateFlag(int Flag,int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Validate set ");
            strSql.Append("Flag=@Flag");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Flag", SqlDbType.Int,4),
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Flag;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_Validate model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Validate set ");
            strSql.Append("Phone=@Phone,");
            strSql.Append("IP=@IP,");
            strSql.Append("Time=@Time,");
            strSql.Append("Flag=@Flag,");
            strSql.Append("codeTime=@codeTime,");
            strSql.Append("mesCode=@mesCode,");
            strSql.Append("type=@type");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@IP", SqlDbType.NVarChar,50),
                    new SqlParameter("@Time", SqlDbType.DateTime),
                    new SqlParameter("@Flag", SqlDbType.Int,4),
                    new SqlParameter("@codeTime", SqlDbType.DateTime),
                    new SqlParameter("@mesCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Phone;
            parameters[2].Value = model.IP;
            parameters[3].Value = model.Time;
            parameters[4].Value = model.Flag;
            parameters[5].Value = model.codeTime;
            parameters[6].Value = model.mesCode;
            parameters[7].Value = model.type;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Validate ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_Validate Gettb_Validate(string Phone,int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Phone,IP,Time,Flag,codeTime,mesCode from tb_Validate ");
            strSql.Append(" where Phone=@Phone and type=@type order by Time desc ");
            SqlParameter[] parameters = {
                new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = Phone;
            parameters[1].Value = type;
            BCW.Model.tb_Validate model = new BCW.Model.tb_Validate();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Phone = reader.GetString(1);
                    model.IP = reader.GetString(2);
                    model.Time = reader.GetDateTime(3);
                    model.Flag = reader.GetInt32(4);
                    model.codeTime = reader.GetDateTime(5);
                    model.mesCode = reader.GetString(6);
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
        public BCW.Model.tb_Validate Gettb_Validate(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Phone,IP,Time,Flag,codeTime,mesCode,type from tb_Validate ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.tb_Validate model = new BCW.Model.tb_Validate();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Phone = reader.GetString(1);
                    model.IP = reader.GetString(2);
                    model.Time = reader.GetDateTime(3);
                    model.Flag = reader.GetInt32(4);
                    model.codeTime = reader.GetDateTime(5);
                    model.mesCode = reader.GetString(6);
                    model.type = reader.GetInt32(7);
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
            strSql.Append(" FROM tb_Validate ");
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
        /// <returns>IList tb_Validate</returns>
        public IList<BCW.Model.tb_Validate> Gettb_Validates(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.tb_Validate> listtb_Validates = new List<BCW.Model.tb_Validate>();
            string sTable = "tb_Validate";
            string sPkey = "id";
            string sField = "ID,Phone,IP,Time,Flag,codeTime,mesCode,type";
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
                    return listtb_Validates;
                }
                while (reader.Read())
                {
                    BCW.Model.tb_Validate objtb_Validate = new BCW.Model.tb_Validate();
                    objtb_Validate.ID = reader.GetInt32(0);
                    objtb_Validate.Phone = reader.GetString(1);
                    objtb_Validate.IP = reader.GetString(2);
                    objtb_Validate.Time = reader.GetDateTime(3);
                    objtb_Validate.Flag = reader.GetInt32(4);
                    objtb_Validate.codeTime = reader.GetDateTime(5);
                    objtb_Validate.mesCode = reader.GetString(6);
                    objtb_Validate.type = reader.GetInt32(7);
                    listtb_Validates.Add(objtb_Validate);
                }
            }
            return listtb_Validates;
        }

        #endregion  成员方法
    }
}

