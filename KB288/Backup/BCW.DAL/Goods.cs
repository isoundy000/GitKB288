using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL
{
    /// <summary>
    /// 数据访问类Goods。
    /// </summary>
    public class Goods
    {
        public Goods()
        { }
        #region  成员方法

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_Goods");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Goods");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string Title)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Goods");
            strSql.Append(" where Title=@Title ");
            SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50)};
            parameters[0].Value = Title;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Goods model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_Goods(");
            strSql.Append("NodeId,UsId,IsAd,Title,KeyWord,Content,Mobile,CityMoney,VipMoney,SellCount,StockCount,PayType,PostType,PostMoney,Readcount,Paycount,Evcount,AddTime,Config,Recount)");
            strSql.Append(" values (");
            strSql.Append("@NodeId,@UsId,@IsAd,@Title,@KeyWord,@Content,@Mobile,@CityMoney,@VipMoney,@SellCount,@StockCount,@PayType,@PostType,@PostMoney,@Readcount,@Paycount,@Evcount,@AddTime,@Config,@Recount)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsAd", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,500),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,200),
					new SqlParameter("@CityMoney", SqlDbType.Money,8),
					new SqlParameter("@VipMoney", SqlDbType.Money,8),
					new SqlParameter("@SellCount", SqlDbType.Int,4),
					new SqlParameter("@StockCount", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.TinyInt,1),
					new SqlParameter("@PostType", SqlDbType.TinyInt,1),
					new SqlParameter("@PostMoney", SqlDbType.NVarChar,100),
					new SqlParameter("@Readcount", SqlDbType.Int,4),
					new SqlParameter("@Paycount", SqlDbType.Int,4),
                    new SqlParameter("@Evcount", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Config", SqlDbType.NVarChar,500),
                    new SqlParameter("@Recount", SqlDbType.Int,4)};
            parameters[0].Value = model.NodeId;
            parameters[1].Value = model.UsId;
            parameters[2].Value = model.IsAd;
            parameters[3].Value = model.Title;
            parameters[4].Value = model.KeyWord;
            parameters[5].Value = model.Content;
            parameters[6].Value = model.Mobile;
            parameters[7].Value = model.CityMoney;
            parameters[8].Value = model.VipMoney;
            parameters[9].Value = 0;
            parameters[10].Value = model.StockCount;
            parameters[11].Value = model.PayType;
            parameters[12].Value = model.PostType;
            parameters[13].Value = model.PostMoney;
            parameters[14].Value = 0;
            parameters[15].Value = 0;
            parameters[16].Value = 0;
            parameters[17].Value = model.AddTime;
            parameters[18].Value = model.Config;
            parameters[19].Value = 0;

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
        public void Update(BCW.Model.Goods model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("NodeId=@NodeId,");
            strSql.Append("UsId=@UsId,");
            strSql.Append("IsAd=@IsAd,");
            strSql.Append("Title=@Title,");
            strSql.Append("KeyWord=@KeyWord,");
            strSql.Append("Content=@Content,");
            strSql.Append("Mobile=@Mobile,");
            strSql.Append("CityMoney=@CityMoney,");
            strSql.Append("VipMoney=@VipMoney,");
            strSql.Append("StockCount=@StockCount,");
            strSql.Append("SellCount=@SellCount,");
            strSql.Append("Paycount=@Paycount,");
            strSql.Append("PayType=@PayType,");
            strSql.Append("PostType=@PostType,");
            strSql.Append("PostMoney=@PostMoney,");
            strSql.Append("AddTime=@AddTime,");
            strSql.Append("Config=@Config");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4),
					new SqlParameter("@UsId", SqlDbType.Int,4),
					new SqlParameter("@IsAd", SqlDbType.Bit,1),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@KeyWord", SqlDbType.NVarChar,500),
					new SqlParameter("@Content", SqlDbType.NText),
					new SqlParameter("@Mobile", SqlDbType.NVarChar,200),
					new SqlParameter("@CityMoney", SqlDbType.Money,8),
					new SqlParameter("@VipMoney", SqlDbType.Money,8),
					new SqlParameter("@StockCount", SqlDbType.Int,4),
					new SqlParameter("@SellCount", SqlDbType.Int,4),
					new SqlParameter("@Paycount", SqlDbType.Int,4),
					new SqlParameter("@PayType", SqlDbType.TinyInt,1),
					new SqlParameter("@PostType", SqlDbType.TinyInt,1),
					new SqlParameter("@PostMoney", SqlDbType.NVarChar,100),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@Config", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.NodeId;
            parameters[2].Value = model.UsId;
            parameters[3].Value = model.IsAd;
            parameters[4].Value = model.Title;
            parameters[5].Value = model.KeyWord;
            parameters[6].Value = model.Content;
            parameters[7].Value = model.Mobile;
            parameters[8].Value = model.CityMoney;
            parameters[9].Value = model.VipMoney;
            parameters[10].Value = model.StockCount;
            parameters[11].Value = model.SellCount;
            parameters[12].Value = model.Paycount;
            parameters[13].Value = model.PayType;
            parameters[14].Value = model.PostType;
            parameters[15].Value = model.PostMoney;
            parameters[16].Value = model.AddTime;
            parameters[17].Value = model.Config;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新出售商品数量
        /// </summary>
        public void UpdateSellCount(int ID, int SellCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("SellCount=SellCount+@SellCount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@SellCount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = SellCount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReLastIP)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("ReStats=@ReStats, ");
            strSql.Append("ReLastIP=@ReLastIP ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@ReStats", SqlDbType.NVarChar,50),
            		new SqlParameter("@ReLastIP", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = ReStats;
            parameters[2].Value = ReLastIP;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新点击数
        /// </summary>
        public void UpdateReadcount(int ID, int Readcount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Readcount=Readcount+@Readcount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Readcount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Readcount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新评论条数
        /// </summary>
        public void UpdateRecount(int ID, int Recount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Recount=Recount+@Recount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Recount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Recount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新购买人数
        /// </summary>
        public void UpdatePaycount(int ID, int Paycount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Paycount=Paycount+@Paycount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Paycount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Paycount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新评价人数
        /// </summary>
        public void UpdateEvcount(int ID, int Evcount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Evcount=Evcount+@Evcount ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
            		new SqlParameter("@Evcount", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = Evcount;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新文件
        /// </summary>
        public void UpdateFiles(int ID, string Files)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Files=@Files ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NText)};
            parameters[0].Value = ID;
            parameters[1].Value = Files;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新封面
        /// </summary>
        public void UpdateCover(int ID, string Cover)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Cover=@Cover ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Cover", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = Cover;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新节点
        /// </summary>
        public void UpdateNodeId(int ID, int NodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("NodeId=@NodeId ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = NodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 批量更新节点
        /// </summary>
        public void UpdateNodeIds(int NewNodeId, int OrdNodeId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("NodeId=@NewNodeId ");
            strSql.Append(" where NodeId=@OrdNodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NewNodeId", SqlDbType.Int,4),
					new SqlParameter("@OrdNodeId", SqlDbType.Int,4)};
            parameters[0].Value = NewNodeId;
            parameters[1].Value = OrdNodeId;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除文件记录
        /// </summary>
        public void DeleteFiles(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Goods set ");
            strSql.Append("Files=@Files, ");
            strSql.Append("Cover=@Cover ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Files", SqlDbType.NText),
					new SqlParameter("@Cover", SqlDbType.NVarChar,50)};
            parameters[0].Value = ID;
            parameters[1].Value = "";
            parameters[2].Value = "";

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除该节点下的所有记录
        /// </summary>
        public void DeleteNodeId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from tb_Goods ");
            strSql.Append(" where NodeId=@NodeId ");
            SqlParameter[] parameters = {
					new SqlParameter("@NodeId", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Goods GetGoods(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,NodeId,UsId,IsAd,Title,KeyWord,Files,Content,Mobile,CityMoney,VipMoney,SellCount,StockCount,PayType,PostType,PostMoney,ReStats,ReLastIP,Readcount,Paycount,Evcount,AddTime,Config,Recount from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            BCW.Model.Goods model = new BCW.Model.Goods();
            DataSet ds = SqlHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["NodeId"].ToString() != "")
                {
                    model.NodeId = int.Parse(ds.Tables[0].Rows[0]["NodeId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["UsId"].ToString() != "")
                {
                    model.UsId = int.Parse(ds.Tables[0].Rows[0]["UsId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["IsAd"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["IsAd"].ToString() == "1") || (ds.Tables[0].Rows[0]["IsAd"].ToString().ToLower() == "true"))
                    {
                        model.IsAd = true;
                    }
                    else
                    {
                        model.IsAd = false;
                    }
                }
                model.Title = ds.Tables[0].Rows[0]["Title"].ToString();
                model.KeyWord = ds.Tables[0].Rows[0]["KeyWord"].ToString();
                model.Files = ds.Tables[0].Rows[0]["Files"].ToString();
                model.Content = ds.Tables[0].Rows[0]["Content"].ToString();
                model.Mobile = ds.Tables[0].Rows[0]["Mobile"].ToString();
                if (ds.Tables[0].Rows[0]["CityMoney"].ToString() != "")
                {
                    model.CityMoney = decimal.Parse(ds.Tables[0].Rows[0]["CityMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["VipMoney"].ToString() != "")
                {
                    model.VipMoney = decimal.Parse(ds.Tables[0].Rows[0]["VipMoney"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SellCount"].ToString() != "")
                {
                    model.SellCount = int.Parse(ds.Tables[0].Rows[0]["SellCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["StockCount"].ToString() != "")
                {
                    model.StockCount = int.Parse(ds.Tables[0].Rows[0]["StockCount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PayType"].ToString() != "")
                {
                    model.PayType = int.Parse(ds.Tables[0].Rows[0]["PayType"].ToString());
                }
                if (ds.Tables[0].Rows[0]["PostType"].ToString() != "")
                {
                    model.PostType = int.Parse(ds.Tables[0].Rows[0]["PostType"].ToString());
                }
                model.PostMoney = ds.Tables[0].Rows[0]["PostMoney"].ToString();
                model.ReStats = ds.Tables[0].Rows[0]["ReStats"].ToString();
                model.ReLastIP = ds.Tables[0].Rows[0]["ReLastIP"].ToString();
                if (ds.Tables[0].Rows[0]["Readcount"].ToString() != "")
                {
                    model.Readcount = int.Parse(ds.Tables[0].Rows[0]["Readcount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Paycount"].ToString() != "")
                {
                    model.Paycount = int.Parse(ds.Tables[0].Rows[0]["Paycount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Evcount"].ToString() != "")
                {
                    model.Evcount = int.Parse(ds.Tables[0].Rows[0]["Evcount"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AddTime"].ToString() != "")
                {
                    model.AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Config"].ToString() != "")
                {
                    model.Config = ds.Tables[0].Rows[0]["Config"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Recount"].ToString() != "")
                {
                    model.Recount = int.Parse(ds.Tables[0].Rows[0]["Recount"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到Title标题
        /// </summary>
        public string GetTitle(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Title from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// 得到节点NodeId
        /// </summary>
        public int GetNodeId(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NodeId from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到文件Files
        /// </summary>
        public string GetFiles(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Files from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到封面Cover
        /// </summary>
        public string GetCover(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Cover from tb_Goods ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = ID;

            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
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
        /// 获得文件列数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Files,Cover");
            strSql.Append(" FROM tb_Goods ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select " + strField + " ");
            strSql.Append(" FROM tb_Goods ");
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
        /// <returns>IList Goods</returns>
        public IList<BCW.Model.Goods> GetGoodss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.Goods> listGoodss = new List<BCW.Model.Goods>();

            string sTable = "tb_Goods";
            string sPkey = "id";
            string sField = "ID,IsAd,Title,UsId,NodeId,Cover";
            string sCondition = strWhere;
            string sOrder = "AddTime DESC";
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

                    return listGoodss;
                }

                while (reader.Read())
                {
                    BCW.Model.Goods objGoods = new BCW.Model.Goods();
                    objGoods.ID = reader.GetInt32(0);
                    objGoods.IsAd = reader.GetBoolean(1);
                    objGoods.Title = reader.GetString(2);
                    objGoods.UsId = reader.GetInt32(3);
                    objGoods.NodeId = reader.GetInt32(4);
                    if (!reader.IsDBNull(5))
                        objGoods.Cover = reader.GetString(5);
                    listGoodss.Add(objGoods);
                }
            }

            return listGoodss;
        }
        #endregion  成员方法
    }
}
