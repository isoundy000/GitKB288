using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace Book.DAL
{
    /// <summary>
    /// 数据访问类ShuSelf。
    /// </summary>
    public class ShuSelf
    {
        public ShuSelf()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperBook.GetMaxID("id", "ShuSelf");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from ShuSelf");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Book.Model.ShuSelf model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into ShuSelf(");
            strSql.Append("aid,name,sex,city,addtime)");
            strSql.Append(" values (");
            strSql.Append("@aid,@name,@sex,@city,@addtime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@sex", SqlDbType.NVarChar,50),
					new SqlParameter("@city", SqlDbType.NVarChar,50),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
            parameters[0].Value = model.aid;
            parameters[1].Value = model.name;
            parameters[2].Value = model.sex;
            parameters[3].Value = model.city;
            parameters[4].Value = DateTime.Now;

            object obj = SqlHelperBook.GetSingle(strSql.ToString(), parameters);
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
        public void Update(Book.Model.ShuSelf model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuSelf set ");
            strSql.Append("name=@name,");
            strSql.Append("sex=@sex,");
            strSql.Append("city=@city");
            strSql.Append(" where aid=@aid ");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@name", SqlDbType.NVarChar,50),
					new SqlParameter("@sex", SqlDbType.NVarChar,50),
					new SqlParameter("@city", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.aid;
            parameters[1].Value = model.name;
            parameters[2].Value = model.sex;
            parameters[3].Value = model.city;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新默认字数
        /// </summary>
        public void UpdatePageNum(int aid, int pagenum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuSelf set ");
            strSql.Append("pagenum=@pagenum");
            strSql.Append(" where aid=@aid ");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@pagenum", SqlDbType.Int,4)};
            parameters[0].Value = aid;
            parameters[1].Value = pagenum;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 写入提醒ID
        /// </summary>
        public void Updategxids(int aid, string gxids)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update ShuSelf set ");
            strSql.Append("gxids=@gxids");
            strSql.Append(" where aid=@aid ");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4),
					new SqlParameter("@gxids", SqlDbType.NText)};
            parameters[0].Value = aid;
            parameters[1].Value = gxids;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from ShuSelf ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到默认字数
        /// </summary>
        public int GetPageNum(int aid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 pagenum from ShuSelf ");
            strSql.Append(" where aid=@aid ");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4)};
            parameters[0].Value = aid;

            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到提醒的书本ID
        /// </summary>
        public string Getgxids(int aid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 gxids from ShuSelf ");
            strSql.Append(" where aid=@aid");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4)};
            parameters[0].Value = aid;

            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    if (!reader.IsDBNull(0))
                        return reader.GetString(0);
                    else
                        return "";
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Book.Model.ShuSelf GetShuSelf(int aid)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,aid,name,sex,city,pagenum,gxids,addtime from ShuSelf ");
            strSql.Append(" where aid=@aid ");
            SqlParameter[] parameters = {
					new SqlParameter("@aid", SqlDbType.Int,4)};
            parameters[0].Value = aid;

            Book.Model.ShuSelf model = new Book.Model.ShuSelf();
            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.aid = reader.GetInt32(1);
                    model.name = reader.GetString(2);
                    model.sex = reader.GetString(3);
                    model.city = reader.GetString(4);
                    model.pagenum = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                        model.gxids = reader.GetString(6);
                    else
                        model.gxids = "";

                    model.addtime = reader.GetDateTime(7);
                    return model;
                }
                else
                {
                    model.id = 0;
                    model.aid = 0;
                    model.name = "未定义";
                    model.sex = "未定义";
                    model.city = "未定义";
                    model.pagenum = 0;
                    model.gxids = "";
                    model.addtime = DateTime.Parse("1990-1-1");
                    return model;
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
            strSql.Append(" FROM ShuSelf ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelperBook.Query(strSql.ToString());
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList ShuSelf</returns>
        public IList<Book.Model.ShuSelf> GetShuSelfs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<Book.Model.ShuSelf> listShuSelfs = new List<Book.Model.ShuSelf>();
            string sTable = "ShuSelf";
            string sPkey = "id";
            string sField = "id,aid,name,sex,city,pagenum,gxids";
            string sCondition = strWhere;
            string sOrder = "ID Desc";
            int iSCounts = 0;
            using (SqlDataReader reader = SqlHelperBook.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listShuSelfs;
                }
                while (reader.Read())
                {
                    Book.Model.ShuSelf objShuSelf = new Book.Model.ShuSelf();
                    objShuSelf.id = reader.GetInt32(0);
                    objShuSelf.aid = reader.GetInt32(1);
                    objShuSelf.name = reader.GetString(2);
                    objShuSelf.sex = reader.GetString(3);
                    objShuSelf.city = reader.GetString(4);
                    objShuSelf.pagenum = reader.GetInt32(5);
                    if (!reader.IsDBNull(6))
                        objShuSelf.gxids = reader.GetString(6);
                    listShuSelfs.Add(objShuSelf);
                }
            }
            return listShuSelfs;
        }

        #endregion  成员方法
    }
}

