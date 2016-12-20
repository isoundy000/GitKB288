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
    /// 数据访问类Buylist。
    /// </summary>
    public class Buylist
    {
        public Buylist()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Buylist");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Buylist");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UserId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Buylist");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UserId=@UserId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            	    new SqlParameter("@UserId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UserId;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int UserId, int AcStats)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Buylist");
            strSql.Append(" where ID=@ID ");
            strSql.Append(" and UserId=@UserId ");
            strSql.Append(" and AcStats=@AcStats ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            	    new SqlParameter("@UserId", SqlDbType.Int,4),
                    new SqlParameter("@AcStats", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = UserId;
            parameters[2].Value = AcStats;
            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Buylist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Buylist(");
            strSql.Append("TingNo,NodeId,GoodsId,Title,Price,Paycount,PostMoney,SellId,UserId,UserName,RealName,Mobile,Address,Notes,AcPrice,AcStats,AcText,AddUsIP,AddTime)");
            strSql.Append(" values (");
            strSql.Append("@TingNo,@NodeId,@GoodsId,@Title,@Price,@Paycount,@PostMoney,@SellId,@UserId,@UserName,@RealName,@Mobile,@Address,@Notes,@AcPrice,@AcStats,@AcText,@AddUsIP,@AddTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@TingNo", SqlDbType.NVarChar,50),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@GoodsId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@Paycount", SqlDbType.Int,4),
					new SqlParameter("@PostMoney", SqlDbType.Int,4),
					new SqlParameter("@SellId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AcPrice", SqlDbType.Money,8),
					new SqlParameter("@AcStats", SqlDbType.TinyInt,1),
					new SqlParameter("@AcText", SqlDbType.NVarChar,500),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.TingNo;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.GoodsId;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.Price;
            parameters[5].Value = model.Paycount;
            parameters[6].Value = model.PostMoney;
            parameters[7].Value = model.SellId;
            parameters[8].Value = model.UserId;
            parameters[9].Value = model.UserName;
            parameters[10].Value = model.RealName;
            parameters[11].Value = model.Mobile;
            parameters[12].Value = model.Address;
            parameters[13].Value = model.Notes;
            parameters[14].Value = model.AcPrice;
            parameters[15].Value = model.AcStats;
            parameters[16].Value = model.AcText;
            parameters[17].Value = model.AddUsIP;
            parameters[18].Value = model.AddTime;

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
        public void Update(BCW.Model.Buylist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Buylist set ");
            strSql.Append("TingNo=@TingNo,");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("GoodsId=@GoodsId,");
            strSql.Append("Title=@Title,");
            strSql.Append("Price=@Price,");
            strSql.Append("Paycount=@Paycount,");
            strSql.Append("PostMoney=@PostMoney,");
            strSql.Append("SellId=@SellId,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("RealName=@RealName,");
            strSql.Append("Mobile=@Mobile,");
            strSql.Append("Address=@Address,");
            strSql.Append("Notes=@Notes,");
            strSql.Append("AcPrice=@AcPrice,");
            strSql.Append("AcStats=@AcStats,");
            strSql.Append("AcText=@AcText,");
            strSql.Append("AddUsIP=@AddUsIP,");
            strSql.Append("AddTime=@AddTime");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@TingNo", SqlDbType.NVarChar,50),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@GoodsId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@Paycount", SqlDbType.Int,4),
					new SqlParameter("@PostMoney", SqlDbType.Int,4),
					new SqlParameter("@SellId", SqlDbType.Int,4),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,50),
					new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,200),
					new SqlParameter("@Notes", SqlDbType.NVarChar,500),
					new SqlParameter("@AcPrice", SqlDbType.Money,8),
					new SqlParameter("@AcStats", SqlDbType.TinyInt,1),
					new SqlParameter("@AcText", SqlDbType.NVarChar,500),
					new SqlParameter("@AddUsIP", SqlDbType.NVarChar,50),
					new SqlParameter("@AddTime", SqlDbType.DateTime)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.TingNo;
            parameters[2].Value = model.NodeId;
            parameters[3].Value = model.GoodsId;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.Price;
            parameters[6].Value = model.Paycount;
            parameters[7].Value = model.PostMoney;
            parameters[8].Value = model.SellId;
            parameters[9].Value = model.UserId;
            parameters[10].Value = model.UserName;
            parameters[11].Value = model.RealName;
            parameters[12].Value = model.Mobile;
            parameters[13].Value = model.Address;
            parameters[14].Value = model.Notes;
            parameters[15].Value = model.AcPrice;
            parameters[16].Value = model.AcStats;
            parameters[17].Value = model.AcText;
            parameters[18].Value = model.AddUsIP;
            parameters[19].Value = model.AddTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新前台订单状态
        /// </summary>
        public void UpdateStats(BCW.Model.Buylist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Buylist set ");
            strSql.Append("LaNotes=@LaNotes,");
            strSql.Append("AcStats=@AcStats ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@LaNotes", SqlDbType.NVarChar,500),
					new SqlParameter("@AcStats", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.LaNotes;
            parameters[2].Value = model.AcStats;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新后台订单状态
        /// </summary>
        public void UpdateMStats(BCW.Model.Buylist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Buylist set ");
            strSql.Append("AcText=@AcText, ");
            strSql.Append("AcEms=@AcEms, ");
            strSql.Append("AcStats=@AcStats ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@AcText", SqlDbType.NVarChar,500),
					new SqlParameter("@AcEms", SqlDbType.NVarChar,500),
					new SqlParameter("@AcStats", SqlDbType.TinyInt,1)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.AcText;
            parameters[2].Value = model.AcEms;
            parameters[3].Value = model.AcStats;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新用户订单记录
        /// </summary>
        public void UpdateBuy(BCW.Model.Buylist model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Buylist set ");
            strSql.Append("RealName=@RealName, ");
            strSql.Append("Mobile=@Mobile, ");
            strSql.Append("Address=@Address, ");
            strSql.Append("Notes=@Notes, ");
            strSql.Append("LaNotes=@LaNotes, ");
            strSql.Append("AcPrice=@AcPrice ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,200),
            		new SqlParameter("@Notes", SqlDbType.NVarChar,500),
                    new SqlParameter("@LaNotes", SqlDbType.NVarChar,500),
            		new SqlParameter("@AcPrice", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.RealName;
            parameters[2].Value = model.Mobile;
            parameters[3].Value = model.Address;
            parameters[4].Value = model.Notes;
            parameters[5].Value = model.LaNotes;
            parameters[6].Value = model.AcPrice;


            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Buylist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete2(int GoodsId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Buylist ");
            strSql.Append(" where GoodsId=@GoodsId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsId", SqlDbType.Int,4)};
            parameters[0].Value = GoodsId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete3(int NodeId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Buylist ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Buylist GetBuylist(int ID, int UserId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,TingNo,NodeId,GoodsId,Title,Price,Paycount,PostMoney,SellId,UserId,UserName,RealName,Mobile,Address,Notes,LaNotes,AcPrice,AcStats,AcEms,AcText,AddUsIP,AddTime from tb_Buylist ");
            strSql.Append(" where ID=@ID ");
            if (UserId != 0)
            {
                strSql.Append(" AND UserId=" + UserId + " ");
            }
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Buylist model = new BCW.Model.Buylist();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                model.TingNo = ds.Tables[0].Rows[0]["TingNo"].ToString();
                if (ds.Tables[0].Rows[0]["NodeId"].ToString() != "")
                {
                    model.NodeId = int.Parse(ds.Tables[0].Rows[0]["NodeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["GoodsId"].ToString() != "")
                {
                    model.GoodsId = int.Parse(ds.Tables[0].Rows[0]["GoodsId"].ToString());
                }
                model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                if (ds.Tables[0].Rows[0]["Price"].ToString() != "")
                {
                    model.Price = decimal.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Paycount"].ToString() != "")
                {
                    model.Paycount = int.Parse(ds.Tables[0].Rows[0]["Paycount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PostMoney"].ToString() != "")
                {
                    model.PostMoney = int.Parse(ds.Tables[0].Rows[0]["PostMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SellId"].ToString() != "")
                {
                    model.SellId = int.Parse(ds.Tables[0].Rows[0]["SellId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(ds.Tables[0].Rows[0]["UserId"].ToString());
                }
                model.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                model.RealName = ds.Tables[0].Rows[0]["RealName"].ToString();
                model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                model.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                model.Notes = ds.Tables[0].Rows[0]["Notes"].ToString();
                model.LaNotes = ds.Tables[0].Rows[0]["LaNotes"].ToString();
                if (ds.Tables[0].Rows[0]["AcPrice"].ToString() != "")
                {
                    model.AcPrice = decimal.Parse(ds.Tables[0].Rows[0]["AcPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AcStats"].ToString() != "")
                {
                    model.AcStats = int.Parse(ds.Tables[0].Rows[0]["AcStats"].ToString());
                }
                model.AcEms = ds.Tables[0].Rows[0]["AcEms"].ToString();
                model.AcText = ds.Tables[0].Rows[0]["AcText"].ToString();
                model.AddUsIP = ds.Tables[0].Rows[0]["AddUsIP"].ToString();
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个实体
        /// </summary>
        public BCW.Model.Buylist GetBuylistMe(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 Paycount,GoodsId,AcPrice,UserId,UserName from tb_Buylist ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Buylist model = new BCW.Model.Buylist();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Paycount = reader.GetInt32(0);
                    model.GoodsId = reader.GetInt32(1);
                    model.AcPrice = reader.GetDecimal(2);
                    model.UserId = reader.GetInt32(3);
                    model.UserName = reader.GetString(4);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int GoodsId, int TopNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TOP " + TopNum + " Mobile,AddTime ");
            strSql.Append(" FROM tb_Buylist ");
            strSql.Append(" where GoodsId=@GoodsId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsId", SqlDbType.Int,4)};
            parameters[0].Value = GoodsId;
            strSql.Append(" Order By ID DESC");

            return SqlHelper.Query(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <returns>IList Buylist</returns>
        public IList<BCW.Model.Buylist> GetBuylists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Buylist> listBuylists = new List<BCW.Model.Buylist>();

            string sTable = "tb_Buylist";
            string sPkey = "id";
            string sField = "ID,Title,AcPrice,AcStats,AddTime,Mobile,LaNotes,Paycount";
            string sCondition = strWhere;
            string sOrder = "id DESC";
            int iSCounts = 0;

            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount))
            {

                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {

                    return listBuylists;
                }

                while (reader.Read())
                {
                    BCW.Model.Buylist objBuylist = new BCW.Model.Buylist();
                    objBuylist.ID = reader.GetInt32(0);
                    objBuylist.Title = reader.GetString(1);
                    objBuylist.AcPrice = reader.GetDecimal(2);
                    objBuylist.AcStats = reader.GetByte(3);
                    objBuylist.AddTime = reader.GetDateTime(4);
                    objBuylist.Mobile = reader.GetString(5);
                    if (!reader.IsDBNull(6))
                        objBuylist.LaNotes = reader.GetString(6);

                    objBuylist.Paycount = reader.GetInt32(7);
                    listBuylists.Add(objBuylist);


                }
            }
            return listBuylists;
        }
        #endregion  成员方法
    }
}

