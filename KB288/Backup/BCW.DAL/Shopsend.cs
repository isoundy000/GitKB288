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
    /// 数据访问类Shopsend。
    /// </summary>
    public class Shopsend
    {
        public Shopsend()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Shopsend");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopsend");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某ID给某ID的送礼次数
        /// </summary>
        public int GetCount(int UsID, int ToID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Count(ID) from tb_Shopsend");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ToID=@ToID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ToID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ToID;
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Shopsend model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Shopsend(");
            strSql.Append("Title,GiftId,PrevPic,UsID,UsName,ToID,ToName,Total,Message,AddTime,PIC)");
            strSql.Append(" values (");
            strSql.Append("@Title,@GiftId,@PrevPic,@UsID,@UsName,@ToID,@ToName,@Total,@Message,@AddTime,@PIC)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftId", SqlDbType.Int,4),
                    new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@UsName", SqlDbType.NVarChar,50),
                    new SqlParameter("@ToID", SqlDbType.Int,4),
                    new SqlParameter("@ToName", SqlDbType.NVarChar,50),
                    new SqlParameter("@Total", SqlDbType.Int,4),
                    new SqlParameter("@Message", SqlDbType.NVarChar,100),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@PIC", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.GiftId;
            parameters[2].Value = model.PrevPic;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.UsName;
            parameters[5].Value = model.ToID;
            parameters[6].Value = model.ToName;
            parameters[7].Value = model.Total;
            parameters[8].Value = model.Message;
            parameters[9].Value = model.AddTime;
            parameters[10].Value = model.PIC;

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
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Shopsend ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Shopsend GetShopsend(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,GiftId,PrevPic,UsID,UsName,ToID,ToName,Total,Message,AddTime from tb_Shopsend ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Shopsend model = new BCW.Model.Shopsend();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.GiftId = reader.GetInt32(2);
                    model.PrevPic = reader.GetString(3);
                    model.UsID = reader.GetInt32(4);
                    model.UsName = reader.GetString(5);
                    model.ToID = reader.GetInt32(6);
                    model.ToName = reader.GetString(7);
                    model.Total = reader.GetInt32(8);
                    model.Message = reader.GetString(9);
                    model.AddTime = reader.GetDateTime(10);
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
            strSql.Append(" FROM tb_Shopsend ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Shopsend</returns>
        public IList<BCW.Model.Shopsend> GetShopsends(int SizeNum, string strWhere)
        {
            IList<BCW.Model.Shopsend> listShopsends = new List<BCW.Model.Shopsend>();
            string sTable = "tb_Shopsend";
            string sPkey = "id";
            string sField = "ID,Title,UsID,UsName,Total,Message,AddTime";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = SizeNum;
            int p_recordCount;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, 1, SizeNum, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount == 0)
                {
                    return listShopsends;
                    
                }
                while (reader.Read())
                {
                    BCW.Model.Shopsend objShopsend = new BCW.Model.Shopsend();
                    objShopsend.ID = reader.GetInt32(0);
                    objShopsend.Title= reader.GetString(1);
                    objShopsend.UsID = reader.GetInt32(2);
                    objShopsend.UsName = reader.GetString(3);
                    objShopsend.Total = reader.GetInt32(4);
                    objShopsend.Message = reader.GetString(5);
                    objShopsend.AddTime = reader.GetDateTime(6);
                    listShopsends.Add(objShopsend);
                }
            }
            return listShopsends;
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Shopsend</returns>
        public IList<BCW.Model.Shopsend> GetShopsends(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Shopsend> listShopsends = new List<BCW.Model.Shopsend>();
            string sTable = "tb_Shopsend";
            string sPkey = "id";
            string sField = "ID,Title,GiftId,PrevPic,UsID,UsName,ToID,ToName,Total,Message,AddTime";
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
                    return listShopsends;
                }
                while (reader.Read())
                {
                    BCW.Model.Shopsend objShopsend = new BCW.Model.Shopsend();
                    objShopsend.ID = reader.GetInt32(0);
                    objShopsend.Title = reader.GetString(1);
                    objShopsend.GiftId = reader.GetInt32(2);
                    objShopsend.PrevPic = reader.GetString(3);
                    objShopsend.UsID = reader.GetInt32(4);
                    objShopsend.UsName = reader.GetString(5);
                    objShopsend.ToID = reader.GetInt32(6);
                    objShopsend.ToName = reader.GetString(7);
                    objShopsend.Total = reader.GetInt32(8);
                    objShopsend.Message = reader.GetString(9);
                    objShopsend.AddTime = reader.GetDateTime(10);
                    listShopsends.Add(objShopsend);
                }
            }
            return listShopsends;
        }

        #endregion  成员方法
    }
}