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
    /// 数据访问类Transfer。
    /// </summary>
    public class Transfer
    {
        public Transfer()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Transfer");
        }
        /// <summary>
        /// 是否订单号是否重复
        /// </summary>
        public bool Exists(int FromId, string zfbNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Transfer");
            strSql.Append(" where FromId=@FromId ");
            strSql.Append(" and zfbNo=@zfbNo ");
            SqlParameter[] parameters = {
					new SqlParameter("@FromId", SqlDbType.Int,4),
					new SqlParameter("@zfbNo", SqlDbType.NVarChar,50)};
            parameters[0].Value = FromId;
            parameters[1].Value = zfbNo;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Transfer");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某用户今天过币额
        /// </summary>
        public long GetAcCents(int FromID, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Sum(AcCent) from tb_Transfer");
            strSql.Append(" where FromID=@FromID ");
            strSql.Append(" and Types=@Types ");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@FromID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = FromID;
            parameters[1].Value = Types;

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
        public int Add(BCW.Model.Transfer model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Transfer(");
            strSql.Append("Types,FromId,FromName,ToId,ToName,AcCent,AddTime,zfbNo)");
            strSql.Append(" values (");
            strSql.Append("@Types,@FromId,@FromName,@ToId,@ToName,@AcCent,@AddTime,@zfbNo)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@FromId", SqlDbType.Int,4),
					new SqlParameter("@FromName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToId", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@AcCent", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@zfbNo", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.FromId;
            parameters[2].Value = model.FromName;
            parameters[3].Value = model.ToId;
            parameters[4].Value = model.ToName;
            parameters[5].Value = model.AcCent;
            parameters[6].Value = model.AddTime;
            if (model.zfbNo == null)
                model.zfbNo = "no";

            parameters[7].Value = model.zfbNo;
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
        public void Update(BCW.Model.Transfer model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Transfer set ");
            strSql.Append("FromId=@FromId,");
            strSql.Append("FromName=@FromName,");
            strSql.Append("ToId=@ToId,");
            strSql.Append("ToName=@ToName,");
            strSql.Append("AcCent=@AcCent,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@FromId", SqlDbType.Int,4),
					new SqlParameter("@FromName", SqlDbType.NVarChar,50),
					new SqlParameter("@ToId", SqlDbType.Int,4),
					new SqlParameter("@ToName", SqlDbType.NVarChar,50),
					new SqlParameter("@AcCent", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.FromId;
            parameters[2].Value = model.FromName;
            parameters[3].Value = model.ToId;
            parameters[4].Value = model.ToName;
            parameters[5].Value = model.AcCent;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Transfer ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Transfer GetTransfer(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,FromId,FromName,ToId,ToName,AcCent,AddTime from tb_Transfer ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Transfer model = new BCW.Model.Transfer();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.FromId = reader.GetInt32(1);
                    model.FromName = reader.GetString(2);
                    model.ToId = reader.GetInt32(3);
                    model.ToName = reader.GetString(4);
                    model.AcCent = reader.GetInt64(5);
                    model.AddTime = reader.GetDateTime(6);
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
            strSql.Append(" FROM tb_Transfer ");
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
        /// <returns>IList Transfer</returns>
        public IList<BCW.Model.Transfer> GetTransfers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Transfer> listTransfers = new List<BCW.Model.Transfer>();
            string sTable = "tb_Transfer";
            string sPkey = "id";
            string sField = "ID,Types,FromId,FromName,ToId,ToName,AcCent,AddTime";
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
                    return listTransfers;
                }
                while (reader.Read())
                {
                    BCW.Model.Transfer objTransfer = new BCW.Model.Transfer();
                    objTransfer.ID = reader.GetInt32(0);
                    objTransfer.Types = reader.GetInt32(1);
                    objTransfer.FromId = reader.GetInt32(2);
                    objTransfer.FromName = reader.GetString(3);
                    objTransfer.ToId = reader.GetInt32(4);
                    objTransfer.ToName = reader.GetString(5);
                    objTransfer.AcCent = reader.GetInt64(6);
                    objTransfer.AddTime = reader.GetDateTime(7);
                    listTransfers.Add(objTransfer);
                }
            }
            return listTransfers;
        }

        /// <summary>
        /// 根据条件计算过币次数
        /// </summary>
        public int GetCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Transfer");
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
        #endregion  成员方法
    }
}
