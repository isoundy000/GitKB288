using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using BCW.Common;
using System.Data.SqlClient;
using BCW.Data;

namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Health。
    /// </summary>
    public class Health
    {
        public Health()
        { }
        #region  成员方法

        /// <summary>
        /// 删除数据
        /// </summary>
        public void Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Health ");
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Health</returns>
        public IList<BCW.Model.Health> GetHealths(int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            IList<BCW.Model.Health> listHealths = new List<BCW.Model.Health>();

            string sTable = "tb_Health";
            string sPkey = "id";
            string sField = "ID,EventCode, Message, EventTime, RequestUrl, ExceptionType, ExceptionMessage";
            string sCondition = "";
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

                    return listHealths;
                }

                while (reader.Read())
                {
                    BCW.Model.Health objHealth = new BCW.Model.Health();
                    objHealth.ID = reader.GetInt32(0);
                    objHealth.EventCode = reader.GetInt32(1);
                    objHealth.Message = reader.GetString(2);
                    objHealth.EventTime = reader.GetDateTime(3);
                    objHealth.RequestUrl = reader.GetString(4);
                    objHealth.ExceptionType = reader.GetString(5);
                    objHealth.ExceptionMessage = reader.GetString(6);
                    listHealths.Add(objHealth);


                }
            }

            return listHealths;
        }
        #endregion  成员方法
    }
}

