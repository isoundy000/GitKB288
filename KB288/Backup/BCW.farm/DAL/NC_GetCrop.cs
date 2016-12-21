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
    /// 数据访问类NC_GetCrop。
    /// </summary>
    public class NC_GetCrop
    {
        public NC_GetCrop()
        { }
        #region  成员方法

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_GetCrop");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt)};
            parameters[0].Value = id;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_GetCrop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_GetCrop(");
            strSql.Append("name,name_id,num,price_out,usid,suoding,tou_nums,get_nums)");
            strSql.Append(" values (");
            strSql.Append("@name,@name_id,@num,@price_out,@usid,@suoding,@tou_nums,@get_nums)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@price_out", SqlDbType.BigInt,8),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@suoding", SqlDbType.Int,4),
                    new SqlParameter("@tou_nums", SqlDbType.Int,4),
                    new SqlParameter("@get_nums", SqlDbType.Int,4)};
            parameters[0].Value = model.name;
            parameters[1].Value = model.name_id;
            parameters[2].Value = model.num;
            parameters[3].Value = model.price_out;
            parameters[4].Value = model.usid;
            parameters[5].Value = model.suoding;
            parameters[6].Value = model.tou_nums;
            parameters[7].Value = model.get_nums;

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
        public void Update(BCW.farm.Model.NC_GetCrop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("name=@name,");
            strSql.Append("name_id=@name_id,");
            strSql.Append("num=@num,");
            strSql.Append("price_out=@price_out,");
            strSql.Append("usid=@usid,");
            strSql.Append("suoding=@suoding");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt,8),
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@price_out", SqlDbType.BigInt,8),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@suoding", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.name_id;
            parameters[3].Value = model.num;
            parameters[4].Value = model.price_out;
            parameters[5].Value = model.usid;
            parameters[6].Value = model.suoding;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(long id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_GetCrop ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt)};
            parameters[0].Value = id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop(long id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,name,name_id,num,price_out,usid,suoding from tb_NC_GetCrop ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt)};
            parameters[0].Value = id;

            BCW.farm.Model.NC_GetCrop model = new BCW.farm.Model.NC_GetCrop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt64(0);
                    model.name = reader.GetString(1);
                    model.name_id = reader.GetInt32(2);
                    model.num = reader.GetInt32(3);
                    model.price_out = reader.GetInt64(4);
                    model.usid = reader.GetInt32(5);
                    model.suoding = reader.GetInt32(6);
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
            strSql.Append(" FROM tb_NC_GetCrop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据字段取数据列表2
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_GetCrop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        //===============================

        /// <summary>
        /// me_根据name_id和usid删除一条数据
        /// </summary>
        public void Delete(int id, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_GetCrop ");
            strSql.Append(" where name_id=@id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt),
                    new SqlParameter("@usid", SqlDbType.BigInt)};
            parameters[0].Value = id;
            parameters[1].Value = usid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该作物
        /// </summary>
        public bool Exists_zuowu(string name, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_GetCrop");
            strSql.Append(" where name=@name and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = name;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_更新一条作物数据
        /// </summary>
        public void Update1(BCW.farm.Model.NC_GetCrop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("name=@name,");
            strSql.Append("name_id=@name_id,");
            strSql.Append("num=num+@num,");
            strSql.Append("price_out=@price_out,");
            strSql.Append("tou_nums=tou_nums+@tou_nums,");
            strSql.Append("get_nums=get_nums+@get_nums,");
            strSql.Append("usid=@usid");
            strSql.Append(" where usid=@usid and name_id=@name_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt,8),
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@price_out", SqlDbType.BigInt,8),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@tou_nums", SqlDbType.Int,4),
                    new SqlParameter("@get_nums", SqlDbType.Int,4)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.name;
            parameters[2].Value = model.name_id;
            parameters[3].Value = model.num;
            parameters[4].Value = model.price_out;
            parameters[5].Value = model.usid;
            parameters[6].Value = model.tou_nums;
            parameters[7].Value = model.get_nums;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        ///me_是否存在该作物2
        /// </summary>
        public bool Exists_zuowu2(int name_id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_GetCrop");
            strSql.Append(" where name_id=@name_id and usid=@usid and num>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_id", SqlDbType.VarChar,20),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = name_id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        ///me_是否存在该作物3
        /// </summary>
        public bool Exists_zuowu3(int name_id, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_GetCrop");
            strSql.Append(" where name_id=@name_id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_id", SqlDbType.VarChar,20),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = name_id;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_锁定
        /// </summary>
        public void Update_suoding(int usid, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("suoding=1");
            strSql.Append(" where name_id=@name_id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};

            parameters[0].Value = name_id;
            parameters[1].Value = usid;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_解锁
        /// </summary>
        public void Update_jiesuo(int usid, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("suoding=0");
            strSql.Append(" where name_id=@name_id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};

            parameters[0].Value = name_id;
            parameters[1].Value = usid;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_得到总价钱
        /// </summary>
        public long Get_allprice(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(num*(price_out)) as aa FROM tb_NC_GetCrop WHERE num>0 and usid=@usid and suoding=0");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = usid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    try
                    {
                        return reader.GetInt64(0);
                    }
                    catch
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_卖出
        /// </summary>
        public void Update_maichu(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("num=num+@num");
            strSql.Append(" where name_id=@name_id and usid=@usid");
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
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_GetCrop GetNC_GetCrop2(int id, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_GetCrop ");
            strSql.Append(" where name_id=@id and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@id", SqlDbType.BigInt),
                    new SqlParameter("@usid", SqlDbType.BigInt)};
            parameters[0].Value = id;
            parameters[1].Value = usid;

            BCW.farm.Model.NC_GetCrop model = new BCW.farm.Model.NC_GetCrop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt64(0);
                    model.name = reader.GetString(1);
                    model.name_id = reader.GetInt32(2);
                    model.num = reader.GetInt32(3);
                    model.price_out = reader.GetInt64(4);
                    model.usid = reader.GetInt32(5);
                    model.suoding = reader.GetInt32(6);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_更新果实数量
        /// </summary>
        public void Update_gs(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("num=num+@num");
            strSql.Append(" where usid=@usid and name_id=@name_id");
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
        /// me_更新果实数量
        /// </summary>
        public void Update_gs2(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_GetCrop set ");
            strSql.Append("num=@num");
            strSql.Append(" where usid=@usid and name_id=@name_id");
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
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_GetCrop</returns>
        public IList<BCW.farm.Model.NC_GetCrop> GetNC_GetCrops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_GetCrop> listNC_GetCrops = new List<BCW.farm.Model.NC_GetCrop>();
            string sTable = "tb_NC_GetCrop";
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
                    return listNC_GetCrops;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_GetCrop objNC_GetCrop = new BCW.farm.Model.NC_GetCrop();
                    objNC_GetCrop.id = reader.GetInt64(0);
                    objNC_GetCrop.name = reader.GetString(1);
                    objNC_GetCrop.name_id = reader.GetInt32(2);
                    objNC_GetCrop.num = reader.GetInt32(3);
                    objNC_GetCrop.price_out = reader.GetInt64(4);
                    objNC_GetCrop.usid = reader.GetInt32(5);
                    objNC_GetCrop.suoding = reader.GetInt32(6);
                    objNC_GetCrop.tou_nums = reader.GetInt32(7);
                    objNC_GetCrop.get_nums = reader.GetInt32(8);
                    listNC_GetCrops.Add(objNC_GetCrop);
                }
            }
            return listNC_GetCrops;
        }

        #endregion  成员方法

        //===============================
    }
}

