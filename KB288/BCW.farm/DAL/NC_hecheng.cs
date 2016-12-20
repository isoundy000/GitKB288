using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.farm.DAL
{
    /// <summary>
    /// 数据访问类NC_hecheng。
    /// </summary>
    public class NC_hecheng
    {
        public NC_hecheng()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_hecheng");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_hecheng");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_hecheng model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_hecheng(");
            strSql.Append("Title,GiftId,PrevPic,UsID,num,AddTime,all_num)");
            strSql.Append(" values (");
            strSql.Append("@Title,@GiftId,@PrevPic,@UsID,@num,@AddTime,@all_num)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftId", SqlDbType.Int,4),
                    new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@all_num", SqlDbType.Int,4)};
            parameters[0].Value = model.Title;
            parameters[1].Value = model.GiftId;
            parameters[2].Value = model.PrevPic;
            parameters[3].Value = model.UsID;
            parameters[4].Value = model.num;
            parameters[5].Value = model.AddTime;
            parameters[6].Value = model.all_num;

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
        public void Update(BCW.farm.Model.NC_hecheng model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_hecheng set ");
            strSql.Append("Title=@Title,");
            strSql.Append("GiftId=@GiftId,");
            strSql.Append("PrevPic=@PrevPic,");
            strSql.Append("UsID=@UsID,");
            strSql.Append("num=@num,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Title", SqlDbType.NVarChar,50),
                    new SqlParameter("@GiftId", SqlDbType.Int,4),
                    new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
                    new SqlParameter("@UsID", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.GiftId;
            parameters[3].Value = model.PrevPic;
            parameters[4].Value = model.UsID;
            parameters[5].Value = model.num;
            parameters[6].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_hecheng ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_hecheng GetNC_hecheng(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,Title,GiftId,PrevPic,UsID,num,AddTime from tb_NC_hecheng ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_hecheng model = new BCW.farm.Model.NC_hecheng();
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
                    model.num = reader.GetInt32(5);
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
            strSql.Append(" FROM tb_NC_hecheng ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        //==================================
        /// <summary>
        /// me_是否存在该记录
        /// </summary>
        public bool Exists_ID(int ID, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_hecheng");
            strSql.Append(" where GiftId=@ID and usid=@UsID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
             new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_ID(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_hecheng SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_更新果实数量
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_hecheng set ");
            strSql.Append("num=num+@num");
            strSql.Append(" where UsID=@usid and GiftId=@name_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@name_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = num;
            parameters[2].Value = name_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到种子数量
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select num from tb_NC_hecheng ");
            strSql.Append(" where UsID=@meid and GiftId=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt32(0);
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        ///me_是否存在该作物2
        /// </summary>
        public bool Exists_zuowu2(int name_id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_hecheng");
            strSql.Append(" where GiftId=@name_id and UsID=@usid and num>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_id", SqlDbType.VarChar,20),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = name_id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_hecheng GetNC_hecheng2(int ID, int UsID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_NC_hecheng ");
            strSql.Append(" where GiftId=@ID and UsID=@UsID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
            new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UsID;

            BCW.farm.Model.NC_hecheng model = new BCW.farm.Model.NC_hecheng();
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
                    model.num = reader.GetInt32(5);
                    model.AddTime = reader.GetDateTime(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }


        //=====================================================
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_hecheng</returns>
        public IList<BCW.farm.Model.NC_hecheng> GetNC_hechengs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_hecheng> listNC_hechengs = new List<BCW.farm.Model.NC_hecheng>();
            string sTable = "tb_NC_hecheng";
            string sPkey = "id";
            string sField = "*";
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
                    return listNC_hechengs;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_hecheng objNC_hecheng = new BCW.farm.Model.NC_hecheng();
                    objNC_hecheng.ID = reader.GetInt32(0);
                    objNC_hecheng.Title = reader.GetString(1);
                    objNC_hecheng.GiftId = reader.GetInt32(2);
                    objNC_hecheng.PrevPic = reader.GetString(3);
                    objNC_hecheng.UsID = reader.GetInt32(4);
                    objNC_hecheng.num = reader.GetInt32(5);
                    objNC_hecheng.AddTime = reader.GetDateTime(6);
                    objNC_hecheng.all_num = reader.GetInt32(7);
                    listNC_hechengs.Add(objNC_hecheng);
                }
            }
            return listNC_hechengs;
        }

        #endregion  成员方法
    }
}

