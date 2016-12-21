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
    /// 数据访问类Network。
    /// </summary>
    public class Network
    {
        public Network()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Network");
        }

        /// <summary>
        /// 是否存在新广播记录
        /// </summary>
        public bool Exists()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Network");
            strSql.Append(" where OverTime>=@OverTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@OverTime", SqlDbType.DateTime)};
            parameters[0].Value = DateTime.Now;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Network");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UsId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Network");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsGroupChat(int Types, int GroupId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Network");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
                    new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = GroupId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 计算某用户今天广播数量
        /// </summary>
        public int GetTodayCount(int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from tb_Network");
            strSql.Append(" where UsID=@UsID and Types<=1");
            strSql.Append(" and Year(AddTime)=" + DateTime.Now.Year + " AND Month(AddTime) = " + DateTime.Now.Month + " and Day(AddTime) = " + DateTime.Now.Day + " ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;

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
        public int Add(BCW.Model.Network model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Network(");
            strSql.Append("Types,Title,UsID,UsName,OverTime,AddTime,OnIDs,IsUbb)");
            strSql.Append(" values (");
            strSql.Append("@Types,@Title,@UsID,@UsName,@OverTime,@AddTime,@OnIDs,@IsUbb)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@OnIDs", SqlDbType.NText),
					new SqlParameter("@IsUbb", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.OverTime;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.OnIDs;
            parameters[7].Value = model.IsUbb;

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
        public void Update(int ID, DateTime OverTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = OverTime;
            parameters[2].Value = DateTime.Now;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOnIDs(int ID, string OnIDs)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("OnIDs=@OnIDs");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@OnIDs", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = OnIDs;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateOther(BCW.Model.Network model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("IsUbb=@IsUbb");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@IsUbb", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.OverTime;
            parameters[4].Value = model.IsUbb;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateGroupChat(BCW.Model.Network model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Title=@Title,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("OnIDs=@OnIDs");
            strSql.Append(" where Types=@Types ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@OnIDs", SqlDbType.NText)};
            parameters[0].Value = model.UsID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.OverTime;
            parameters[5].Value = model.OnIDs;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Network model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.OverTime;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateBasic(BCW.Model.Network model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Network set ");
            strSql.Append("Types=@Types,");
            strSql.Append("Title=@Title,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,500),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void Delete()
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Network ");

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Network ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Network GetNetwork(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,Title,UsID,UsName,OverTime,AddTime,IsUbb from tb_Network ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Network model = new BCW.Model.Network();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    model.UsID = reader.GetInt32(3);
                    model.UsName = reader.GetString(4);
                    model.OverTime = reader.GetDateTime(5);
                    model.AddTime = reader.GetDateTime(6);
                    model.IsUbb = reader.GetByte(7);
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
            strSql.Append(" FROM tb_Network ");
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
        /// <returns>IList Network</returns>
        public IList<BCW.Model.Network> GetNetworks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Network> listNetworks = new List<BCW.Model.Network>();
            string sTable = "tb_Network";
            string sPkey = "id";
            string sField = "ID,Types,Title,UsID,UsName,OverTime,AddTime,IsUbb";
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
                    return listNetworks;
                }
                while (reader.Read())
                {
                    BCW.Model.Network objNetwork = new BCW.Model.Network();
                    objNetwork.ID = reader.GetInt32(0);
                    objNetwork.Types = reader.GetInt32(1);
                    objNetwork.Title = reader.GetString(2);
                    objNetwork.UsID = reader.GetInt32(3);
                    objNetwork.UsName = reader.GetString(4);
                    objNetwork.OverTime = reader.GetDateTime(5);
                    objNetwork.AddTime = reader.GetDateTime(6);
                    objNetwork.IsUbb = reader.GetByte(7);
                    listNetworks.Add(objNetwork);
                }
            }
            return listNetworks;
        }

        #endregion  成员方法
    }
}

