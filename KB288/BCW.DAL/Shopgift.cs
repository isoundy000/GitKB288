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
    /// 数据访问类Shopgift。
    /// </summary>
    public class Shopgift
    {
        public Shopgift()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Shopgift");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopgift");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopgift");
            strSql.Append(" where ID=@ID and NodeId=@NodeId");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NodeId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Shopgift model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Shopgift(");
            strSql.Append("NodeId,Title,Notes,Pic,PrevPic,BzType,Price,PayCount,Total,Para,IsSex,IsVip,IsRecom,AddTime,Ids)");
            strSql.Append(" values (");
            strSql.Append("@NodeId,@Title,@Notes,@Pic,@PrevPic,@BzType,@Price,@PayCount,@Total,@Para,@IsSex,@IsVip,@IsRecom,@AddTime,@Ids)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@Pic", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@PayCount", SqlDbType.Int,4),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Para", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@IsVip", SqlDbType.TinyInt,1),
					new SqlParameter("@IsRecom", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@Ids", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.NodeId;
            parameters[1].Value = model.Title;
            parameters[2].Value = model.Notes;
            parameters[3].Value = model.Pic;
            parameters[4].Value = model.PrevPic;
            parameters[5].Value = model.BzType;
            parameters[6].Value = model.Price;
            parameters[7].Value = model.PayCount;
            parameters[8].Value = model.Total;
            parameters[9].Value = model.Para;
            parameters[10].Value = model.IsSex;
            parameters[11].Value = model.IsVip;
            parameters[12].Value = model.IsRecom;
            parameters[13].Value = model.AddTime;
            parameters[14].Value = model.IDS;
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
        public void Update(BCW.Model.Shopgift model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopgift set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("Title=@Title,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("Pic=@Pic,");
            strSql.Append("PrevPic=@PrevPic,");
            strSql.Append("BzType=@BzType,");
            strSql.Append("Price=@Price,");
            strSql.Append("PayCount=@PayCount,");
            strSql.Append("Total=@Total,");
            strSql.Append("Para=@Para,");
            strSql.Append("IsSex=@IsSex,");
            strSql.Append("IsVip=@IsVip,");
            strSql.Append("IsRecom=@IsRecom,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("Ids=@IDS");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Notes", SqlDbType.NVarChar,200),
					new SqlParameter("@Pic", SqlDbType.NVarChar,100),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@BzType", SqlDbType.TinyInt,1),
					new SqlParameter("@Price", SqlDbType.Int,4),
					new SqlParameter("@PayCount", SqlDbType.Int,4),
					new SqlParameter("@Total", SqlDbType.Int,4),
					new SqlParameter("@Para", SqlDbType.NVarChar,100),
					new SqlParameter("@IsSex", SqlDbType.TinyInt,1),
					new SqlParameter("@IsVip", SqlDbType.TinyInt,1),
					new SqlParameter("@IsRecom", SqlDbType.TinyInt,1),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
                    new SqlParameter("@Ids", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.Title;
            parameters[3].Value = model.Notes;
            parameters[4].Value = model.Pic;
            parameters[5].Value = model.PrevPic;
            parameters[6].Value = model.BzType;
            parameters[7].Value = model.Price;
            parameters[8].Value = model.PayCount;
            parameters[9].Value = model.Total;
            parameters[10].Value = model.Para;
            parameters[11].Value = model.IsSex;
            parameters[12].Value = model.IsVip;
            parameters[13].Value = model.IsRecom;
            parameters[14].Value = model.AddTime;
            parameters[15].Value = model.IDS;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新购买量
        /// </summary>
        public void Update(int ID, int PayCount, int Total)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopgift set ");
            strSql.Append("PayCount=PayCount+@PayCount,");
            strSql.Append("Total=Total+@Total");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@PayCount", SqlDbType.Int,4),
					new SqlParameter("@Total", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = PayCount;
            parameters[2].Value = Total;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Shopgift ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Shopgift ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Shopgift GetShopgift(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,NodeId,Title,Notes,Pic,PrevPic,BzType,Price,PayCount,Total,Para,IsSex,IsVip,IsRecom,AddTime,IDS from tb_Shopgift ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Shopgift model = new BCW.Model.Shopgift();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.NodeId = reader.GetInt32(1);
                    model.Title = reader.GetString(2);
                    if (!reader.IsDBNull(3))
                        model.Notes = reader.GetString(3);

                    model.Pic = reader.GetString(4);
                    model.PrevPic = reader.GetString(5);
                    model.BzType = reader.GetByte(6);
                    model.Price = reader.GetInt32(7);
                    model.PayCount = reader.GetInt32(8);
                    model.Total = reader.GetInt32(9);
                    model.Para = reader.GetString(10);
                    model.IsSex = reader.GetByte(11);
                    model.IsVip = reader.GetByte(12);
                    model.IsRecom = reader.GetByte(13);
                    model.AddTime = reader.GetDateTime(14);
                    if (!reader.IsDBNull(15))
                        model.IDS = reader.GetString(15);
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
            strSql.Append(" FROM tb_Shopgift ");
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Shopgift</returns>
        public IList<BCW.Model.Shopgift> GetShopgifts(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            IList<BCW.Model.Shopgift> listShopgifts = new List<BCW.Model.Shopgift>();
            string sTable = "tb_Shopgift";
            string sPkey = "id";
            string sField = "ID,Title,Pic,BzType,Price,PayCount,Para,Ids,Total,NodeId";
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
                    return listShopgifts;
                }
                while (reader.Read())
                {
                    BCW.Model.Shopgift objShopgift = new BCW.Model.Shopgift();
                    objShopgift.ID = reader.GetInt32(0);
                    objShopgift.Title = reader.GetString(1);
                    objShopgift.Pic = reader.GetString(2);
                    objShopgift.BzType = reader.GetByte(3);
                    objShopgift.Price = reader.GetInt32(4);
                    objShopgift.PayCount = reader.GetInt32(5);
                    objShopgift.Para = reader.GetString(6);
                    if (!reader.IsDBNull(7))
                        objShopgift.IDS = reader.GetString(7);
                    objShopgift.Total = reader.GetInt32(8);
                    objShopgift.NodeId = reader.GetInt32(9);
                    listShopgifts.Add(objShopgift);
                }
            }
            return listShopgifts;
        }

        #endregion  成员方法
    }
}

