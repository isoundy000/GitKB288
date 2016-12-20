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
    /// 数据访问类Toplist。
    /// </summary>
    public class Toplist
    {
        public Toplist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Toplist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Toplist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsId, int Types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Toplist");
            strSql.Append(" where UsId=@UsId ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
            	    new SqlParameter("@UsId", SqlDbType.Int,4),
                    new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = UsId;
            parameters[1].Value = Types;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Toplist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Toplist(");
            strSql.Append("Types,UsId,UsName,WinGold,PutGold,WinNum,PutNum)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsId,@UsName,@WinGold,@PutGold,@WinNum,@PutNum)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@WinGold", SqlDbType.BigInt,8),
					new SqlParameter("@PutGold", SqlDbType.BigInt,8),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@PutNum", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.WinGold;
            parameters[4].Value = model.PutGold;
            parameters[5].Value = model.WinNum;
            parameters[6].Value = model.PutNum;

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
        public void Update(BCW.Model.Toplist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Toplist set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("WinGold=WinGold+@WinGold,");
            strSql.Append("PutGold=PutGold+@PutGold,");
            strSql.Append("WinNum=WinNum+@WinNum,");
            strSql.Append("PutNum=PutNum+@PutNum");
            strSql.Append(" where UsId=@UsId ");
            strSql.Append(" and Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@WinGold", SqlDbType.BigInt,8),
					new SqlParameter("@PutGold", SqlDbType.BigInt,8),
					new SqlParameter("@WinNum", SqlDbType.Int,4),
					new SqlParameter("@PutNum", SqlDbType.Int,4)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.WinGold;
            parameters[4].Value = model.PutGold;
            parameters[5].Value = model.WinNum;
            parameters[6].Value = model.PutNum;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }



        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Toplist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空排行榜
        /// </summary>
        public void Clear(int Types)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Toplist ");
            strSql.Append(" where Types=@Types ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4)};
            parameters[0].Value = Types;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空全部排行榜
        /// </summary>
        public void Clear()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Toplist ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Toplist GetToplist(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsId,UsName,WinGold,PutGold,WinNum,PutNum from tb_Toplist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Toplist model = new BCW.Model.Toplist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsId = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.WinGold = reader.GetInt64(4);
                    model.PutGold = reader.GetInt64(5);
                    model.WinNum = reader.GetInt32(6);
                    model.PutNum = reader.GetInt32(7);
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
            strSql.Append(" FROM tb_Toplist ");
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Toplist</returns>
        public IList<BCW.Model.Toplist> GetToplists(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Toplist> listToplists = new List<BCW.Model.Toplist>();
            string sTable = "tb_Toplist";
            string sPkey = "id";
            string sField = "UsId,UsName,WinGold,PutGold,WinNum,PutNum";
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
                    return listToplists;
                }
                while (reader.Read())
                {
                    BCW.Model.Toplist objToplist = new BCW.Model.Toplist();
                    objToplist.UsId = reader.GetInt32(0);
                    objToplist.UsName = reader.GetString(1);
                    objToplist.WinGold = reader.GetInt64(2);
                    objToplist.PutGold = reader.GetInt64(3);
                    objToplist.WinNum = reader.GetInt32(4);
                    objToplist.PutNum = reader.GetInt32(5);
                    listToplists.Add(objToplist);
                }
            }
            return listToplists;
        }

        #endregion  成员方法
    }
}
