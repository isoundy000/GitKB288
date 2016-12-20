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
    /// 数据访问类NC_mydaoju。
    /// </summary>
    public class NC_mydaoju
    {
        public NC_mydaoju()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_NC_mydaoju");
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.farm.Model.NC_mydaoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_NC_mydaoju(");
            strSql.Append("name,num,usid,type,zhonglei,name_id,huafei_id,suoding,picture,iszengsong)");
            strSql.Append(" values (");
            strSql.Append("@name,@num,@usid,@type,@zhonglei,@name_id,@huafei_id,@suoding,@picture,@iszengsong)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@name", SqlDbType.NVarChar,30),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@zhonglei", SqlDbType.Int,4),
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4),
                    new SqlParameter("@suoding", SqlDbType.Int,4),
                    new SqlParameter("@picture", SqlDbType.VarChar,200),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = model.name;
            parameters[1].Value = model.num;
            parameters[2].Value = model.usid;
            parameters[3].Value = model.type;
            parameters[4].Value = model.zhonglei;
            parameters[5].Value = model.name_id;
            parameters[6].Value = model.huafei_id;
            parameters[7].Value = model.suoding;
            parameters[8].Value = model.picture;
            parameters[9].Value = model.iszengsong;
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
        public void Update(BCW.farm.Model.NC_mydaoju model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
            strSql.Append("name=@name,");
            strSql.Append("num=@num,");
            strSql.Append("usid=@usid,");
            strSql.Append("type=@type,");
            strSql.Append("zhonglei=@zhonglei,");
            strSql.Append("name_id=@name_id,");
            strSql.Append("huafei_id=@huafei_id,");
            strSql.Append("suoding=@suoding,");
            strSql.Append("picture=@picture");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@name", SqlDbType.NVarChar,30),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@type", SqlDbType.Int,4),
                    new SqlParameter("@zhonglei", SqlDbType.Int,4),
                    new SqlParameter("@name_id", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4),
                    new SqlParameter("@suoding", SqlDbType.Int,4),
                    new SqlParameter("@picture", SqlDbType.VarChar,200)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.name;
            parameters[2].Value = model.num;
            parameters[3].Value = model.usid;
            parameters[4].Value = model.type;
            parameters[5].Value = model.zhonglei;
            parameters[6].Value = model.name_id;
            parameters[7].Value = model.huafei_id;
            parameters[8].Value = model.suoding;
            parameters[9].Value = model.picture;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_mydaoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_mydaoju GetNC_mydaoju(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,name,num,usid,type,zhonglei,name_id,huafei_id,suoding,picture from tb_NC_mydaoju ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.farm.Model.NC_mydaoju model = new BCW.farm.Model.NC_mydaoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num = reader.GetInt32(2);
                    model.usid = reader.GetInt32(3);
                    model.type = reader.GetInt32(4);
                    model.zhonglei = reader.GetInt32(5);
                    model.name_id = reader.GetInt32(6);
                    model.huafei_id = reader.GetInt32(7);
                    model.suoding = reader.GetInt32(8);
                    model.picture = reader.GetString(9);
                    model.iszengsong = reader.GetInt32(10);
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
            strSql.Append(" FROM tb_NC_mydaoju ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }


        //===========================
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_mydaoju ");
            strSql.Append(" where name_id=@ID and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int ID, int usid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_NC_mydaoju ");
            strSql.Append(" where huafei_id=@ID and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_根据字段取数据列表
        /// </summary>
        public DataSet GetList2(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " + strField + " ");
            strSql.Append(" FROM tb_NC_mydaoju ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// me_根据name_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Getname_id(int meid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and name_id=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;

            BCW.farm.Model.NC_mydaoju model = new BCW.farm.Model.NC_mydaoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num = reader.GetInt32(2);
                    model.usid = reader.GetInt32(3);
                    model.type = reader.GetInt32(4);
                    model.zhonglei = reader.GetInt32(5);
                    model.name_id = reader.GetInt32(6);
                    model.huafei_id = reader.GetInt32(7);
                    model.suoding = reader.GetInt32(8);
                    model.picture = reader.GetString(9);
                    model.iszengsong = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_得到种子数量
        /// </summary>
        public int Get_daoju_num(int meid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select num from tb_NC_mydaoju ");//邵广林 20160601 修改为num
            strSql.Append(" where usid=@meid and name_id=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                try
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
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到道具数量
        /// </summary>
        public int Get_daojunum2(int meid, int ID, int iszengsong)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select num from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and huafei_id=@ID and iszengsong=@iszengsong ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;
            parameters[2].Value = iszengsong;
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                try
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
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_得到是否锁定
        /// </summary>
        public int Get_suoding(int meid, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and name_id=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                try
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
                catch
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// me_更新种植的种子
        /// </summary>
        public void Update_zz(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
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
        /// me_更新种植的种子
        /// </summary>
        public void Update_zz2(int usid, int num, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
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
        /// me_是否存在该记录
        /// </summary>
        public bool Exists(int ID, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where name_id=@ID and usid=@usid AND num>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在该记录2
        /// </summary>
        public bool Exists2(int ID, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where name_id=@ID and usid=@usid");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在化肥该记录
        /// </summary>
        public bool Exists_hf(int ID, int usid, int iszengsong)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where huafei_id=@ID and usid=@usid AND num>0 and iszengsong=@iszengsong");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;
            parameters[2].Value = iszengsong;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在化肥该记录
        /// </summary>
        public bool Exists_hf(int ID, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where huafei_id=@ID and usid=@usid AND num>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在化肥该记录2
        /// </summary>
        public bool Exists_hf2(int ID, int usid, int iszengsong)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where huafei_id=@ID and usid=@usid and iszengsong=@iszengsong");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;
            parameters[2].Value = iszengsong;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在化肥该记录
        /// </summary>
        public bool Exists_hf3(int ID, int usid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(num) from tb_NC_mydaoju");
            strSql.Append(" where huafei_id=@ID and usid=@usid AND num>0");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@usid", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = usid;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_是否存在银钥匙
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_yys(int meid, int huafei_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(num) from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and huafei_id=@huafei_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4)};
            parameters[0].Value = meid;
            parameters[1].Value = huafei_id;

            BCW.farm.Model.NC_mydaoju model = new BCW.farm.Model.NC_mydaoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                try
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        model.num = reader.GetInt32(0);
                        return model;
                    }
                    else
                    {
                        model.num = 0;
                        return model;
                    }
                }
                catch
                {
                    model.num = 0;
                    return model;
                }

            }
        }
        /// <summary>
        /// me_是否存在宝箱钥匙
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Get_bxys(int meid, int huafei_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select SUM(num) from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and huafei_id=@huafei_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4)};
            parameters[0].Value = meid;
            parameters[1].Value = huafei_id;

            BCW.farm.Model.NC_mydaoju model = new BCW.farm.Model.NC_mydaoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                try
                {
                    if (reader.HasRows)
                    {
                        reader.Read();
                        model.num = reader.GetInt32(0);
                        return model;
                    }
                    else
                    {
                        model.num = 0;
                        return model;
                    }
                }
                catch
                {
                    model.num = 0;
                    return model;
                }

            }
        }
        /// <summary>
        /// me_根据huafei_id得到一个对象实体
        /// </summary>
        public BCW.farm.Model.NC_mydaoju Gethf_id(int meid, int ID, int iszengsong)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_NC_mydaoju ");
            strSql.Append(" where usid=@meid and huafei_id=@ID and iszengsong=@iszengsong");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@meid", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = meid;
            parameters[2].Value = iszengsong;
            BCW.farm.Model.NC_mydaoju model = new BCW.farm.Model.NC_mydaoju();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.name = reader.GetString(1);
                    model.num = reader.GetInt32(2);
                    model.usid = reader.GetInt32(3);
                    model.type = reader.GetInt32(4);
                    model.zhonglei = reader.GetInt32(5);
                    model.name_id = reader.GetInt32(6);
                    model.huafei_id = reader.GetInt32(7);
                    model.suoding = reader.GetInt32(8);
                    model.picture = reader.GetString(9);
                    model.iszengsong = reader.GetInt32(10);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// me_更新化肥数量
        /// </summary>
        public void Update_hf(int usid, int num, int huafei_id, int iszengsong)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
            strSql.Append("num=num+@num");
            strSql.Append(" where usid=@usid and huafei_id=@huafei_id and iszengsong=@iszengsong");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4),
                    new SqlParameter("@iszengsong", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = num;
            parameters[2].Value = huafei_id;
            parameters[3].Value = iszengsong;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_更新化肥数量
        /// </summary>
        public void Update_hf2(int usid, int num, int huafei_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
            strSql.Append("num=@num");
            strSql.Append(" where usid=@usid and huafei_id=@huafei_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@num", SqlDbType.Int,4),
                    new SqlParameter("@huafei_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = num;
            parameters[2].Value = huafei_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// me_是否改作物种子
        /// </summary>
        public bool Exists_zz(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_NC_mydaoju");
            strSql.Append(" where name_id=@ID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// me_锁定
        /// </summary>
        public void Update_sd(int usid, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
            strSql.Append("suoding=1");
            strSql.Append(" where usid=@usid and name_id=@name_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@usid", SqlDbType.Int,4),
                    new SqlParameter("@name_id", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = name_id;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// me_解锁
        /// </summary>
        public void Update_jiesuo(int usid, int name_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_NC_mydaoju set ");
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
        /// me_取得每页记录2
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus2(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_mydaoju> listNC_mydaojus = new List<BCW.farm.Model.NC_mydaoju>();
            string sTable = "tb_NC_mydaoju";
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
                    return listNC_mydaojus;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_mydaoju objNC_mydaoju = new BCW.farm.Model.NC_mydaoju();
                    objNC_mydaoju.ID = reader.GetInt32(0);
                    objNC_mydaoju.name = reader.GetString(1);
                    objNC_mydaoju.num = reader.GetInt32(2);
                    objNC_mydaoju.usid = reader.GetInt32(3);
                    objNC_mydaoju.type = reader.GetInt32(4);
                    objNC_mydaoju.zhonglei = reader.GetInt32(5);
                    objNC_mydaoju.name_id = reader.GetInt32(6);
                    objNC_mydaoju.huafei_id = reader.GetInt32(7);
                    objNC_mydaoju.suoding = reader.GetInt32(8);
                    objNC_mydaoju.picture = reader.GetString(9);
                    objNC_mydaoju.iszengsong = reader.GetInt32(10);
                    listNC_mydaojus.Add(objNC_mydaoju);
                }
            }
            return listNC_mydaojus;
        }


        /// <summary>
        /// me_取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList NC_mydaoju</returns>
        public IList<BCW.farm.Model.NC_mydaoju> GetNC_mydaojus(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.farm.Model.NC_mydaoju> listNC_mydaojus = new List<BCW.farm.Model.NC_mydaoju>();
            string sTable = "tb_NC_mydaoju";
            string sPkey = "id";
            string sField = "*";
            string sCondition = strWhere;
            string sOrder = "";
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
                    return listNC_mydaojus;
                }
                while (reader.Read())
                {
                    BCW.farm.Model.NC_mydaoju objNC_mydaoju = new BCW.farm.Model.NC_mydaoju();
                    objNC_mydaoju.ID = reader.GetInt32(0);
                    objNC_mydaoju.name = reader.GetString(1);
                    objNC_mydaoju.num = reader.GetInt32(2);
                    objNC_mydaoju.usid = reader.GetInt32(3);
                    objNC_mydaoju.type = reader.GetInt32(4);
                    objNC_mydaoju.zhonglei = reader.GetInt32(5);
                    objNC_mydaoju.name_id = reader.GetInt32(6);
                    objNC_mydaoju.huafei_id = reader.GetInt32(7);
                    objNC_mydaoju.suoding = reader.GetInt32(8);
                    objNC_mydaoju.picture = reader.GetString(9);
                    objNC_mydaoju.iszengsong = reader.GetInt32(10);
                    listNC_mydaojus.Add(objNC_mydaoju);
                }
            }
            return listNC_mydaojus;
        }

        #endregion  成员方法


        //===========================
    }
}

