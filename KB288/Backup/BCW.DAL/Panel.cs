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
    /// 数据访问类Panel。
    /// </summary>
    public class Panel
    {
        public Panel()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Panel");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Panel");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Panel");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Panel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Panel(");
            strSql.Append("Title,PUrl,UsId,IsBr,Paixu)");
            strSql.Append(" values (");
            strSql.Append("@Title,@PUrl,@UsId,@IsBr,@Paixu)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.PUrl;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.IsBr;
            parameters[4].Value = model.Paixu;

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
        /// 更新横向竖向
        /// </summary>
        public void UpdateIsBr(int UsID, int IsBr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("IsBr=@IsBr ");
            strSql.Append(" where UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1)};
            parameters[0].Value = UsID;
            parameters[1].Value = IsBr;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新排序
        /// </summary>
        public void UpdatePaixu(int ID, int Paixu)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("Paixu=@Paixu ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Panel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Panel set ");
            strSql.Append("Title=@Title,");
            strSql.Append("PUrl=@PUrl,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("IsBr=@IsBr,");
            strSql.Append("Paixu=@Paixu");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsBr", SqlDbType.TinyInt,1),
					new SqlParameter("@Paixu", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.PUrl;
            parameters[3].Value = model.UsId;
            parameters[4].Value = model.IsBr;
            parameters[5].Value = model.Paixu;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Panel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        public void Delete(int UsID, string Title, string PUrl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Panel");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and Title=@Title ");
            strSql.Append(" and PUrl=@PUrl ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PUrl", SqlDbType.NVarChar,50)};
            parameters[0].Value = UsID;
            parameters[1].Value = Title;
            parameters[2].Value = PUrl;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Panel GetPanel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,PUrl,UsId,IsBr,Paixu from tb_Panel ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Panel model = new BCW.Model.Panel();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.PUrl = reader.GetString(2);
                    model.UsId = reader.GetInt32(3);
                    model.IsBr = reader.GetInt32(4);
                    model.Paixu = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_Panel ");
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
        /// <returns>IList Panel</returns>
        public IList<BCW.Model.Panel> GetPanels(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Panel> listPanels = new List<BCW.Model.Panel>();
            string sTable = "tb_Panel";
            string sPkey = "id";
            string sField = "ID,Title,PUrl,UsId,IsBr,Paixu";
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
                    return listPanels;
                }
                while (reader.Read())
                {
                    BCW.Model.Panel objPanel = new BCW.Model.Panel();
                    objPanel.ID = reader.GetInt32(0);
                    objPanel.Title = reader.GetString(1);
                    objPanel.PUrl = reader.GetString(2);
                    objPanel.UsId = reader.GetInt32(3);
                    objPanel.IsBr = reader.GetInt32(4);
                    objPanel.Paixu = reader.GetInt32(5);
                    listPanels.Add(objPanel);
                }
            }
            return listPanels;
        }

        #endregion  成员方法
    }
}

