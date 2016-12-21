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
    /// 数据访问类Gameowe。
    /// </summary>
    public class Gameowe
    {
        public Gameowe()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Gameowe");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Types, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Gameowe");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Gameowe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Gameowe(");
            strSql.Append("Types,UsID,UsName,Content,OweCent,EnId,BzType,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@Types,@UsID,@UsName,@Content,@OweCent,@EnId,@BzType,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200),
					new SqlParameter("@OweCent", SqlDbType.BigInt,8),
					new SqlParameter("@EnId", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.Types;
            parameters[1].Value = model.UsID;
            parameters[2].Value = model.UsName;
            parameters[3].Value = model.Content;
            parameters[4].Value = model.OweCent;
            parameters[5].Value = model.EnId;
            parameters[6].Value = model.BzType;
            parameters[7].Value = model.AddTime;

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
        public void Update(BCW.Model.Gameowe model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Gameowe set ");
            strSql.Append("UsID=@UsID,");
            strSql.Append("UsName=@UsName,");
            strSql.Append("Content=@Content,");
            strSql.Append("OweCent=@OweCent,");
            strSql.Append("EnId=@EnId,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Content", SqlDbType.NVarChar,200),
					new SqlParameter("@OweCent", SqlDbType.BigInt,8),
					new SqlParameter("@EnId", SqlDbType.Int,4),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Types;
            parameters[2].Value = model.UsID;
            parameters[3].Value = model.UsName;
            parameters[4].Value = model.Content;
            parameters[5].Value = model.OweCent;
            parameters[6].Value = model.EnId;
            parameters[7].Value = model.BzType;
            parameters[8].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Gameowe ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Gameowe GetGameowe(int Types, int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Types,UsID,UsName,Content,OweCent,EnId,BzType,AddTime from tb_Gameowe ");
            strSql.Append(" where Types=@Types and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = Types;
            parameters[1].Value = ID;

            BCW.Model.Gameowe model = new BCW.Model.Gameowe();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.Types = reader.GetInt32(1);
                    model.UsID = reader.GetInt32(2);
                    model.UsName = reader.GetString(3);
                    model.Content = reader.GetString(4);
                    model.OweCent = reader.GetInt64(5);
                    model.EnId = reader.GetInt32(6);
                    model.BzType = reader.GetByte(7);
                    model.AddTime = reader.GetDateTime(8);
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
            strSql.Append(" FROM tb_Gameowe ");
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
        /// <returns>IList Gameowe</returns>
        public IList<BCW.Model.Gameowe> GetGameowes(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Gameowe> listGameowes = new List<BCW.Model.Gameowe>();
            string sTable = "tb_Gameowe";
            string sPkey = "id";
            string sField = "ID,Types,UsID,UsName,Content,OweCent,EnId,BzType,AddTime";
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
                    return listGameowes;
                }
                while (reader.Read())
                {
                    BCW.Model.Gameowe objGameowe = new BCW.Model.Gameowe();
                    objGameowe.ID = reader.GetInt32(0);
                    objGameowe.Types = reader.GetInt32(1);
                    objGameowe.UsID = reader.GetInt32(2);
                    objGameowe.UsName = reader.GetString(3);
                    objGameowe.Content = reader.GetString(4);
                    objGameowe.OweCent = reader.GetInt64(5);
                    objGameowe.EnId = reader.GetInt32(6);
                    objGameowe.BzType = reader.GetByte(7);
                    objGameowe.AddTime = reader.GetDateTime(8);
                    listGameowes.Add(objGameowe);
                }
            }
            return listGameowes;
        }

        #endregion  成员方法
    }
}
