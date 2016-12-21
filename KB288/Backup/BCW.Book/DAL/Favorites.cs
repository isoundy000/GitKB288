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
    /// 数据访问类Favorites。
    /// </summary>
    public class Favorites
    {
        public Favorites()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelperBook.GetMaxID("id", "Favorites");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Favorites");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在书架记录
        /// </summary>
        public bool Exists2(int usid, int nid, int sid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Favorites");
            strSql.Append(" where usid=@usid");
            strSql.Append(" and nid=@nid");
            strSql.Append(" and sid=@sid and types=0");
            SqlParameter[] parameters = {
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@sid", SqlDbType.Int,4)};
            parameters[0].Value = usid;
            parameters[1].Value = nid;
            parameters[2].Value = sid;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 是否存在书签记录
        /// </summary>
        public bool Exists3(int usid, int nid, int sid, string purl)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Favorites");
            strSql.Append(" where usid=@usid");
            strSql.Append(" and nid=@nid");
            strSql.Append(" and sid=@sid");
            strSql.Append(" and purl=@purl and types=1");
            SqlParameter[] parameters = {
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@sid", SqlDbType.Int,4),
					new SqlParameter("@purl", SqlDbType.NVarChar,200)};
            parameters[0].Value = usid;
            parameters[1].Value = nid;
            parameters[2].Value = sid;
            parameters[3].Value = purl;

            return SqlHelperBook.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 计算某书架收藏数量
        /// </summary>
        public int GetCount(int favid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from Favorites");
            strSql.Append(" where types=0 ");
            strSql.Append(" and favid=" + favid + "");     
            object obj = SqlHelperBook.GetSingle(strSql.ToString());
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
        /// 计算某用户收藏数量
        /// </summary>
        public int GetCount(int usid, int types)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT Count(ID) from Favorites");
            strSql.Append(" where types=" + types + "");
            strSql.Append(" and usid=" + usid + "");
            object obj = SqlHelperBook.GetSingle(strSql.ToString());
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
        public int Add(Book.Model.Favorites model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Favorites(");
            strSql.Append("favid,types,title,nid,sid,purl,usid,addtime)");
            strSql.Append(" values (");
            strSql.Append("@favid,@types,@title,@nid,@sid,@purl,@usid,@addtime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@favid", SqlDbType.Int,4),
					new SqlParameter("@types", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@sid", SqlDbType.Int,4),
					new SqlParameter("@purl", SqlDbType.NVarChar,200),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
            parameters[0].Value = model.favid;
            parameters[1].Value = model.types;
            parameters[2].Value = model.title;
            parameters[3].Value = model.nid;
            parameters[4].Value = model.sid;
            parameters[5].Value = model.purl;
            parameters[6].Value = model.usid;
            parameters[7].Value = model.addtime;

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
        public void Update(Book.Model.Favorites model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Favorites set ");
            strSql.Append("types=@types,");
            strSql.Append("title=@title,");
            strSql.Append("nid=@nid,");
            strSql.Append("sid=@sid,");
            strSql.Append("purl=@purl,");
            strSql.Append("usid=@usid,");
            strSql.Append("addtime=@addtime");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@types", SqlDbType.Int,4),
					new SqlParameter("@title", SqlDbType.NVarChar,50),
					new SqlParameter("@nid", SqlDbType.Int,4),
					new SqlParameter("@sid", SqlDbType.Int,4),
					new SqlParameter("@purl", SqlDbType.NVarChar,200),
					new SqlParameter("@usid", SqlDbType.Int,4),
					new SqlParameter("@addtime", SqlDbType.DateTime)};
            parameters[0].Value = model.id;
            parameters[1].Value = model.types;
            parameters[2].Value = model.title;
            parameters[3].Value = model.nid;
            parameters[4].Value = model.sid;
            parameters[5].Value = model.purl;
            parameters[6].Value = model.usid;
            parameters[7].Value = model.addtime;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Favorites ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            SqlHelperBook.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Favorites ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelperBook.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Book.Model.Favorites GetFavorites(int id)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,favid,types,title,nid,sid,purl,usid,addtime from Favorites ");
            strSql.Append(" where id=@id ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            Book.Model.Favorites model = new Book.Model.Favorites();
            using (SqlDataReader reader = SqlHelperBook.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.id = reader.GetInt32(0);
                    model.favid = reader.GetInt32(1);
                    model.types = reader.GetInt32(2);
                    model.title = reader.GetString(3);
                    model.nid = reader.GetInt32(4);
                    model.sid = reader.GetInt32(5);
                    model.purl = reader.GetString(6);
                    model.usid = reader.GetInt32(7);
                    model.addtime = reader.GetDateTime(8);
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
            strSql.Append(" FROM Favorites ");
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
        /// <returns>IList Favorites</returns>
        public IList<Book.Model.Favorites> GetFavoritess(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<Book.Model.Favorites> listFavoritess = new List<Book.Model.Favorites>();
            string sTable = "Favorites";
            string sPkey = "id";
            string sField = "id,favid,types,title,nid,sid,purl,usid,addtime";
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
                    return listFavoritess;
                }
                while (reader.Read())
                {
                    Book.Model.Favorites objFavorites = new Book.Model.Favorites();
                    objFavorites.id = reader.GetInt32(0);
                    objFavorites.favid = reader.GetInt32(1);
                    objFavorites.types = reader.GetInt32(2);
                    objFavorites.title = reader.GetString(3);
                    objFavorites.nid = reader.GetInt32(4);
                    objFavorites.sid = reader.GetInt32(5);
                    objFavorites.purl = reader.GetString(6);
                    objFavorites.usid = reader.GetInt32(7);
                    objFavorites.addtime = reader.GetDateTime(8);
                    listFavoritess.Add(objFavorites);
                }
            }
            return listFavoritess;
        }

        #endregion  成员方法
    }
}
