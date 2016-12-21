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
    /// 数据访问类HcList。
    /// </summary>
    public class HcList
    {
        /// <summary>
        /// wdy 20160524
        /// </summary>
        public HcList()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("id", "tb_HcList");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HcList");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsm(int num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HcList");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4)};
            parameters[0].Value = num;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Game.HcList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HcList(");
            strSql.Append("CID,Result,Notes,State,payCent,payCount,payCent2,payCount2,EndTime)");
            strSql.Append(" values (");
            strSql.Append("@CID,@Result,@Notes,@State,@payCent,@payCount,@payCent2,@payCount2,@EndTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@Result", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,100),
					new SqlParameter("@State", SqlDbType.TinyInt,1),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@payCount", SqlDbType.Int,4),
					new SqlParameter("@payCent2", SqlDbType.BigInt,8),
					new SqlParameter("@payCount2", SqlDbType.Int,4),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.CID;
            parameters[1].Value = model.Result;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.State;
            parameters[4].Value = model.payCent;
            parameters[5].Value = model.payCount;
            parameters[6].Value = model.payCent2;
            parameters[7].Value = model.payCount2;
            parameters[8].Value = model.EndTime;

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
        public void Update(BCW.Model.Game.HcList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HcList set ");
            strSql.Append("CID=@CID,");
            strSql.Append("EndTime=@EndTime");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.CID;
            parameters[2].Value = model.EndTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update1(int CID, long payCent, int payCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HcList set ");
            strSql.Append("payCent=payCent+@payCent,");
            strSql.Append("payCount=payCount+@payCount");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@payCent", SqlDbType.BigInt,8),
					new SqlParameter("@payCount", SqlDbType.Int,4)};
            parameters[0].Value = CID;
            parameters[1].Value = payCent;
            parameters[2].Value = payCount;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(int CID, long payCent2, int payCount2)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HcList set ");
            strSql.Append("payCent2=payCent2+@payCent2,");
            strSql.Append("payCount2=payCount2+@payCount2");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
					new SqlParameter("@CID", SqlDbType.Int,4),
					new SqlParameter("@payCent2", SqlDbType.BigInt,8),
					new SqlParameter("@payCount2", SqlDbType.Int,4)};
            parameters[0].Value = CID;
            parameters[1].Value = payCent2;
            parameters[2].Value = payCount2;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateResult(int CID, int Result)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HcList set ");
            strSql.Append("Result=@Result ");
            strSql.Append(" where CID=@CID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CID", SqlDbType.Int,4),
                    new SqlParameter("@Result", SqlDbType.BigInt,8), };         
            parameters[0].Value = CID;
            parameters[1].Value = Result;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HcList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Game.HcList GetHcList(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,CID,Result,Notes,State,payCent,payCount,payCent2,payCount2,EndTime from tb_HcList ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            BCW.Model.Game.HcList model = new BCW.Model.Game.HcList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.CID = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.Result = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.Notes = reader.GetString(3);
                    model.State = reader.GetByte(4);
                    model.payCent = reader.GetInt64(5);
                    model.payCount = reader.GetInt32(6);
                    model.payCent2 = reader.GetInt64(7);
                    model.payCount2 = reader.GetInt32(8);
                    model.EndTime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 得到一个最新对象实体
        /// </summary>
        public BCW.Model.Game.HcList GetHcListNew(int State)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,CID,Result,Notes,State,payCent,payCount,payCent2,payCount2,EndTime from tb_HcList ");
            strSql.Append(" Where State=@State Order BY ID DESC");
            SqlParameter[] parameters = {
					new SqlParameter("@State", SqlDbType.Int,4)};
            parameters[0].Value = State;

            BCW.Model.Game.HcList model = new BCW.Model.Game.HcList();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.CID = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        model.Result = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.Notes = reader.GetString(3);
                    model.State = reader.GetByte(4);
                    model.payCent = reader.GetInt64(5);
                    model.payCount = reader.GetInt32(6);
                    model.payCent2 = reader.GetInt64(7);
                    model.payCount2 = reader.GetInt32(8);
                    model.EndTime = reader.GetDateTime(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到机器人投注次数
        /// </summary>
        public int GetcountRebot(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1) from tb_HcPay ");
            strSql.Append(" where datediff(day,AddTime,getdate())=0 ");
            strSql.Append("and UsID=@UsID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UsID", SqlDbType.Int,4)};

            parameters[0].Value = usid;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_HcList ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList1(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM (select top 2 * from tb_HcList order by id desc)as T ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 得到上一期CID
        /// </summary>
        public int CID()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ID FROM (SELECT ROW_NUMBER() OVER(ORDER BY ID desc) AS rowNum, * FROM tb_HcList where State=1) AS t WHERE rowNum=2");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};

            parameters[0].Value = 0;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
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
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcList</returns>
        public IList<BCW.Model.Game.HcList> GetHcLists(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Game.HcList> listHcLists = new List<BCW.Model.Game.HcList>();
            string sTable = "tb_HcList";
            string sPkey = "id";
            string sField = "ID,CID,Result";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listHcLists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.HcList objHcList = new BCW.Model.Game.HcList();
                    objHcList.id= reader.GetInt32(0);
                    objHcList.CID = reader.GetInt32(1);
                    objHcList.Result = reader.GetString(2);
                    listHcLists.Add(objHcList);
                }
            }
            return listHcLists;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcList</returns>
        public IList<BCW.Model.Game.HcList> GetHcLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Game.HcList> listHcLists = new List<BCW.Model.Game.HcList>();
            string sTable = "tb_HcList";
            string sPkey = "id";
            string sField = "id,CID,Result,Notes,State,payCent,payCount,payCent2,payCount2,EndTime";
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
                    return listHcLists;
                }
                while (reader.Read())
                {
                    BCW.Model.Game.HcList objHcList = new BCW.Model.Game.HcList();
                    objHcList.id = reader.GetInt32(0);
                    objHcList.CID = reader.GetInt32(1);
                    if (!reader.IsDBNull(2))
                        objHcList.Result = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        objHcList.Notes = reader.GetString(3);
                    objHcList.State = reader.GetByte(4);
                    objHcList.payCent = reader.GetInt64(5);
                    objHcList.payCount = reader.GetInt32(6);
                    objHcList.payCent2 = reader.GetInt64(7);
                    objHcList.payCount2 = reader.GetInt32(8);
                    objHcList.EndTime = reader.GetDateTime(9);
                    listHcLists.Add(objHcList);
                }
            }
            return listHcLists;
        }

        #endregion  成员方法
    }
}

