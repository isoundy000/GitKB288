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
    /// 数据访问类NC_shop。
    /// </summary>
    public class NC_shop
    {
        public NC_shop()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_shop");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_shop");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_shop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_shop(");
            strSql.Append("name,num_id,grade,picture,jidu,jidu_time,price_in,price_out,experience,output,type,iszengsong,zhonglei,caotime,chongtime,shuitime,meili,tili)");
            strSql.Append(" values (");
            strSql.Append("@name,@num_id,@grade,@picture,@jidu,@jidu_time,@price_in,@price_out,@experience,@output,@type,@iszengsong,@zhonglei,@caotime,@chongtime,@shuitime,@meili,@tili)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@num_id", SqlDbType.Int,4),
                    new SqlParameter("@grade", SqlDbType.Int,4),
                    new SqlParameter("@picture", SqlDbType.VarChar,300),
                    new SqlParameter("@jidu", SqlDbType.Int,4),
                    new SqlParameter("@jidu_time", SqlDbType.Int,4),
                    new SqlParameter("@price_in", SqlDbType.BigInt,8),
                    new SqlParameter("@price_out", SqlDbType.BigInt,8),
                    new SqlParameter("@experience", SqlDbType.Int,4),
                    new SqlParameter("@output", SqlDbType.VarChar,20),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4),
                    new SqlParameter("@zhonglei", SqlDbType.Int,4),
                    new SqlParameter("@caotime", SqlDbType.Int,4),
                    new SqlParameter("@chongtime", SqlDbType.Int,4),
                    new SqlParameter("@shuitime", SqlDbType.Int,4),
                    new SqlParameter("@meili", SqlDbType.Int,4),
                    new SqlParameter("@tili", SqlDbType.Int,4)};
            parameters[0].Value = model.name;
            parameters[1].Value = model.num_id;
            parameters[2].Value = model.grade;
            parameters[3].Value = model.picture;
            parameters[4].Value = model.jidu;
            parameters[5].Value = model.jidu_time;
            parameters[6].Value = model.price_in;
            parameters[7].Value = model.price_out;
            parameters[8].Value = model.experience;
            parameters[9].Value = model.output;
            parameters[10].Value = model.type;
            parameters[11].Value = model.iszengsong;
            parameters[12].Value = model.zhonglei;
            parameters[13].Value = model.caotime;
            parameters[14].Value = model.chongtime;
            parameters[15].Value = model.shuitime;
            parameters[16].Value = model.meili;
            parameters[17].Value = model.tili;

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
        public void Update(BCW.farm.Model.NC_shop model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_shop set ");
            strSql.Append("name=@name,");
            strSql.Append("num_id=@num_id,");
            strSql.Append("grade=@grade,");
            strSql.Append("picture=@picture,");
            strSql.Append("jidu=@jidu,");
            strSql.Append("jidu_time=@jidu_time,");
            strSql.Append("price_in=@price_in,");
            strSql.Append("price_out=@price_out,");
            strSql.Append("experience=@experience,");
            strSql.Append("output=@output,");
            strSql.Append("type=@type,");
            strSql.Append("iszengsong=@iszengsong,");
            strSql.Append("zhonglei=@zhonglei,");
            strSql.Append("caotime=@caotime,");
            strSql.Append("chongtime=@chongtime,");
            strSql.Append("shuitime=@shuitime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@name", SqlDbType.VarChar,20),
                    new SqlParameter("@num_id", SqlDbType.Int,4),
                    new SqlParameter("@grade", SqlDbType.Int,4),
                    new SqlParameter("@picture", SqlDbType.VarChar,300),
                    new SqlParameter("@jidu", SqlDbType.Int,4),
                    new SqlParameter("@jidu_time", SqlDbType.Int,4),
                    new SqlParameter("@price_in", SqlDbType.BigInt,8),
                    new SqlParameter("@price_out", SqlDbType.BigInt,8),
                    new SqlParameter("@experience", SqlDbType.Int,4),
                    new SqlParameter("@output", SqlDbType.VarChar,20),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4),
                    new SqlParameter("@zhonglei", SqlDbType.Int,4),
                    new SqlParameter("@caotime", SqlDbType.Int,4),
                    new SqlParameter("@chongtime", SqlDbType.Int,4),
                    new SqlParameter("@shuitime", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.name;
            parameters[2].Value = model.num_id;
            parameters[3].Value = model.grade;
            parameters[4].Value = model.picture;
            parameters[5].Value = model.jidu;
            parameters[6].Value = model.jidu_time;
            parameters[7].Value = model.price_in;
            parameters[8].Value = model.price_out;
            parameters[9].Value = model.experience;
            parameters[10].Value = model.output;
            parameters[11].Value = model.type;
            parameters[12].Value = model.iszengsong;
            parameters[13].Value = model.zhonglei;
            parameters[14].Value = model.caotime;
            parameters[15].Value = model.chongtime;
            parameters[16].Value = model.shuitime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_shop ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name,num_id,grade,picture,jidu,jidu_time,price_in,price_out,experience,output,type,iszengsong,zhonglei,caotime,chongtime,shuitime from tb_NC_shop ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_shop model = new BCW.farm.Model.NC_shop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num_id = reader.GetInt32(2);
                    model.grade = reader.GetInt32(3);
                    model.picture = reader.GetString(4);
                    model.jidu = reader.GetInt32(5);
                    model.jidu_time = reader.GetInt32(6);
                    model.price_in = reader.GetInt64(7);
                    model.price_out = reader.GetInt64(8);
                    model.experience = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.type = reader.GetInt32(11);
                    model.iszengsong = reader.GetInt32(12);
                    model.zhonglei = reader.GetInt32(13);
                    model.caotime = reader.GetInt32(14);
                    model.chongtime = reader.GetInt32(15);
                    model.shuitime = reader.GetInt32(16);
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
            strSql.Append(" FROM tb_NC_shop ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //=================================
        /// <summary>
        /// me_随机得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop Getsd_suiji(int grade)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1  * from  tb_NC_shop WHERE grade<=@grade AND type!=10 order by newid()");
            SqlParameter[] parameters = {
                    new SqlParameter("@grade", SqlDbType.Int,4)};
            parameters[0].Value = grade;

            BCW.farm.Model.NC_shop model = new BCW.farm.Model.NC_shop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num_id = reader.GetInt32(2);
                    model.grade = reader.GetInt32(3);
                    model.picture = reader.GetString(4);
                    model.jidu = reader.GetInt32(5);
                    model.jidu_time = reader.GetInt32(6);
                    model.price_in = reader.GetInt64(7);
                    model.price_out = reader.GetInt64(8);
                    model.experience = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.type = reader.GetInt32(11);
                    model.iszengsong = reader.GetInt32(12);
                    model.zhonglei = reader.GetInt32(13);
                    model.caotime = reader.GetInt32(14);
                    model.chongtime = reader.GetInt32(15);
                    model.shuitime = reader.GetInt32(16);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_得到总价钱
        /// </summary>
        public long get_usergoid(int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(s.num * (m.price_in/2)) from tb_NC_mydaoju s inner join tb_NC_shop m on s.name_id=m.num_id AND s.usid=@usid AND s.suoding=0");
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
        /// me_根据类型查询数量
        /// </summary>
        public long get_typenum(int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(*) FROM tb_NC_shop WHERE zhonglei=@type");
            SqlParameter[] parameters = {
                    new SqlParameter("@type", SqlDbType.Int,4)};
            parameters[0].Value = type;

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
        /// me_根据name_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop1(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_shop ");
            strSql.Append(" where num_id=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_shop model = new BCW.farm.Model.NC_shop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num_id = reader.GetInt32(2);
                    model.grade = reader.GetInt32(3);
                    model.picture = reader.GetString(4);
                    model.jidu = reader.GetInt32(5);
                    model.jidu_time = reader.GetInt32(6);
                    model.price_in = reader.GetInt64(7);
                    model.price_out = reader.GetInt64(8);
                    model.experience = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.type = reader.GetInt32(11);
                    model.iszengsong = reader.GetInt32(12);
                    model.zhonglei = reader.GetInt32(13);
                    model.caotime = reader.GetInt32(14);
                    model.chongtime = reader.GetInt32(15);
                    model.shuitime = reader.GetInt32(16);
                    model.meili = reader.GetInt32(17);
                    model.tili = reader.GetInt32(18);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_根据name得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop2(string name)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_shop ");
            strSql.Append(" where name=@name");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20)};
            parameters[0].Value = name;

            BCW.farm.Model.NC_shop model = new BCW.farm.Model.NC_shop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num_id = reader.GetInt32(2);
                    model.grade = reader.GetInt32(3);
                    model.picture = reader.GetString(4);
                    model.jidu = reader.GetInt32(5);
                    model.jidu_time = reader.GetInt32(6);
                    model.price_in = reader.GetInt64(7);
                    model.price_out = reader.GetInt64(8);
                    model.experience = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.type = reader.GetInt32(11);
                    model.iszengsong = reader.GetInt32(12);
                    model.zhonglei = reader.GetInt32(13);
                    model.caotime = reader.GetInt32(14);
                    model.chongtime = reader.GetInt32(15);
                    model.shuitime = reader.GetInt32(16);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_是否存在该种子ID
        /// //购买时，除去type=10的特殊作物
        /// </summary>
        public bool Exists_zzid(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_shop");
            strSql.Append(" where num_id=@ID and type!=10");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该种子ID
        /// 
        /// </summary>
        public bool Exists_zzid2(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_shop");
            strSql.Append(" where num_id=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该种子名称
        /// </summary>
        public bool Exists_zzmc(string name)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_shop");
            strSql.Append(" where name=@name ");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.VarChar,20)};
            parameters[0].Value = name;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段修改数据列表
        /// </summary>
        public DataSet update_shop(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE tb_NC_shop SET ");
            strSql.Append(strField);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_得到最后一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_shop GetNC_shop_last(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP(1)* from tb_NC_shop ORDER BY ID DESC");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_shop model = new BCW.farm.Model.NC_shop();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num_id = reader.GetInt32(2);
                    model.grade = reader.GetInt32(3);
                    model.picture = reader.GetString(4);
                    model.jidu = reader.GetInt32(5);
                    model.jidu_time = reader.GetInt32(6);
                    model.price_in = reader.GetInt64(7);
                    model.price_out = reader.GetInt64(8);
                    model.experience = reader.GetInt32(9);
                    model.output = reader.GetString(10);
                    model.type = reader.GetInt32(11);
                    model.iszengsong = reader.GetInt32(12);
                    model.zhonglei = reader.GetInt32(13);
                    model.caotime = reader.GetInt32(14);
                    model.chongtime = reader.GetInt32(15);
                    model.shuitime = reader.GetInt32(16);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_shop</returns>
        public IList<BCW.farm.Model.NC_shop> GetNC_shops(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_shop> listNC_shops = new List<BCW.farm.Model.NC_shop>();
            string sTable = "tb_NC_shop";
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
                    return listNC_shops;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_shop objNC_shop = new BCW.farm.Model.NC_shop();
                    objNC_shop.ID = reader.GetInt32(0);
                    objNC_shop.name = reader.GetString(1);
                    objNC_shop.num_id = reader.GetInt32(2);
                    objNC_shop.grade = reader.GetInt32(3);
                    objNC_shop.picture = reader.GetString(4);
                    objNC_shop.jidu = reader.GetInt32(5);
                    objNC_shop.jidu_time = reader.GetInt32(6);
                    objNC_shop.price_in = reader.GetInt64(7);
                    objNC_shop.price_out = reader.GetInt64(8);
                    objNC_shop.experience = reader.GetInt32(9);
                    objNC_shop.output = reader.GetString(10);
                    objNC_shop.type = reader.GetInt32(11);
                    objNC_shop.iszengsong = reader.GetInt32(12);
                    objNC_shop.zhonglei = reader.GetInt32(13);
                    objNC_shop.caotime = reader.GetInt32(14);
                    objNC_shop.chongtime = reader.GetInt32(15);
                    objNC_shop.shuitime = reader.GetInt32(16);
                    listNC_shops.Add(objNC_shop);
                }
            }
            return listNC_shops;
        }

        #endregion  成员方法
    }
}

