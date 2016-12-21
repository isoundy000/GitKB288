using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;

namespace BCW.HB.DAL
{
    public partial class HbGetNote
    {
        public HbGetNote()
        { }

        #region 成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HbGetNote");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int GetID,int HbID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_HbGetNote");
            strSql.Append(" where GetID=@GetID and HbID=@HbID");
            SqlParameter[] parameters = {
                    new SqlParameter("@GetID", SqlDbType.NChar,10),
				    new SqlParameter("@HbID", SqlDbType.NChar,10)
					
			};
            parameters[0].Value = GetID;
            parameters[1].Value = HbID;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.HB.Model.HbGetNote model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_HbGetNote(");
            strSql.Append("HbID,GetID,GetMoney,GetTime,IsMax)");
            strSql.Append(" values (");
            strSql.Append("@HbID,@GetID,@GetMoney,@GetTime,@IsMax)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@HbID", SqlDbType.NChar,10),
					new SqlParameter("@GetID", SqlDbType.NChar,10),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@GetTime", SqlDbType.DateTime),
					new SqlParameter("@IsMax", SqlDbType.Int,4)};
            parameters[0].Value = model.HbID;
            parameters[1].Value = model.GetID;
            parameters[2].Value = model.GetMoney;
            parameters[3].Value = model.GetTime;
            parameters[4].Value = model.IsMax;

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
        public bool Update(BCW.HB.Model.HbGetNote model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_HbGetNote set ");
            strSql.Append("HbID=@HbID,");
            strSql.Append("GetID=@GetID,");
            strSql.Append("GetMoney=@GetMoney,");
            strSql.Append("GetTime=@GetTime,");
            strSql.Append("IsMax=@IsMax");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@HbID", SqlDbType.NChar,10),
					new SqlParameter("@GetID", SqlDbType.NChar,10),
					new SqlParameter("@GetMoney", SqlDbType.BigInt,8),
					new SqlParameter("@GetTime", SqlDbType.DateTime),
					new SqlParameter("@IsMax", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = model.HbID;
            parameters[1].Value = model.GetID;
            parameters[2].Value = model.GetMoney;
            parameters[3].Value = model.GetTime;
            parameters[4].Value = model.IsMax;
            parameters[5].Value = model.ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HbGetNote ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            int rows = SqlHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_HbGetNote ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = SqlHelper.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbGetNote GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,HbID,GetID,GetMoney,GetTime,IsMax from tb_HbGetNote ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
            parameters[0].Value = ID;

            BCW.HB.Model.HbGetNote model = new BCW.HB.Model.HbGetNote();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        ///通过GetID 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbGetNote GetModelByGetID(int GetID,int HbID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,HbID,GetID,GetMoney,GetTime,IsMax from tb_HbGetNote ");
            strSql.Append(" where GetID=@GetID and HbID=@HbID");
            SqlParameter[] parameters = {
					new SqlParameter("@GetID", SqlDbType.Int,4),
                    new SqlParameter("@HbID", SqlDbType.Int,4)
			};
            parameters[0].Value = GetID;
            parameters[1].Value = HbID;
            BCW.HB.Model.HbGetNote model = new BCW.HB.Model.HbGetNote();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.HB.Model.HbGetNote DataRowToModel(DataRow row)
        {
            BCW.HB.Model.HbGetNote model = new BCW.HB.Model.HbGetNote();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["HbID"] != null)
                {
                    model.HbID = row["HbID"].ToString();
                }
                if (row["GetID"] != null)
                {
                    model.GetID = row["GetID"].ToString();
                }
                if (row["GetMoney"] != null && row["GetMoney"].ToString() != "")
                {
                    model.GetMoney = long.Parse(row["GetMoney"].ToString());
                }
                if (row["GetTime"] != null && row["GetTime"].ToString() != "")
                {
                    model.GetTime = DateTime.Parse(row["GetTime"].ToString());
                }
                if (row["IsMax"] != null && row["IsMax"].ToString() != "")
                {
                    model.IsMax = int.Parse(row["IsMax"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,HbID,GetID,GetMoney,GetTime,IsMax ");
            strSql.Append(" FROM tb_HbGetNote ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" ID,HbID,GetID,GetMoney,GetTime,IsMax ");
            strSql.Append(" FROM tb_HbGetNote ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM tb_HbGetNote ");
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
        /// <summary>
        /// 获取抢到的总钱数
        /// </summary>
        public long GetAllMoney(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(GetMoney) FROM tb_HbGetNote ");
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
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ID desc");
            }
            strSql.Append(")AS Row, T.*  from tb_HbGetNote T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ChatText</returns>
        public IList<BCW.HB.Model.HbGetNote> GetListByPage(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.HB.Model.HbGetNote> listHbpost = new List<BCW.HB.Model.HbGetNote>();
            string sTable = "tb_HbGetNote";
            string sPkey = "ID";
            string sField = "ID,HbID,GetID,GetMoney,GetTime,IsMax";
            string sCondition = strWhere;
            string sOrder = "GetTime Desc";
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
                    return listHbpost;
                }
                while (reader.Read())
                {
                    BCW.HB.Model.HbGetNote objHbpost =new  BCW.HB.Model.HbGetNote();
                    objHbpost.ID= reader.GetInt32(0);
                    objHbpost.HbID= reader.GetString(1);
                    objHbpost.GetID=reader.GetString(2);
                    objHbpost.GetMoney = reader.GetInt64(3);
                    objHbpost.GetTime = reader.GetDateTime(4);
                    objHbpost.IsMax = reader.GetInt32(5);
                    listHbpost.Add(objHbpost);
                }
            }
            return listHbpost;
        }
        #endregion 成员方法
    }
}
