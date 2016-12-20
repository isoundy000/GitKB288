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
	/// 数据访问类GoodsList。
	/// </summary>
	public class GoodsList
	{
		public GoodsList()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_GoodsList");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
			parameters[0].Value = Id;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("Id", "tb_GoodsList");
        }
        //Getperiods
             /// <summary>
        /// 得到商品期数
        /// </summary>
        public long Getperiods(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select periods from tb_GoodsList ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;
            object obj = SqlHelperUser.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt64(obj);
            }
        } 
        /// <summary>
        /// 得到商品Getstatue
        /// </summary>
        public string Getstatue(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select statue from tb_GoodsList ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;
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
        /// 得到商品名称
        /// </summary>
        public string GetGoodsName(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsName from tb_GoodsList ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;   
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
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.GoodsList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_GoodsList(");
            strSql.Append("GoodsName,explain,GoodsImg,ImgCounts,GoodsValue,periods,Number,Addtime,OverTime,GoodsType,RemainingTime,Winner,lotteryTime,statue,isDone,GoodsFrom,Identification,StockYungouma,GoodsSell)");
            strSql.Append(" values (");
            strSql.Append("@GoodsName,@explain,@GoodsImg,@ImgCounts,@GoodsValue,@periods,@Number,@Addtime,@OverTime,@GoodsType,@RemainingTime,@Winner,@lotteryTime,@statue,@isDone,@GoodsFrom,@Identification,@StockYungouma,@GoodsSell)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@explain", SqlDbType.VarChar),
					new SqlParameter("@GoodsImg", SqlDbType.VarChar),
					new SqlParameter("@ImgCounts", SqlDbType.Int,4),
					new SqlParameter("@GoodsValue", SqlDbType.BigInt,8),
					new SqlParameter("@periods", SqlDbType.BigInt,8),
					new SqlParameter("@Number", SqlDbType.BigInt,8),
					new SqlParameter("@Addtime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@GoodsType", SqlDbType.Int,4),
					new SqlParameter("@RemainingTime", SqlDbType.DateTime),
					new SqlParameter("@Winner", SqlDbType.VarChar,50),
					new SqlParameter("@lotteryTime", SqlDbType.DateTime),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
                    new SqlParameter("@GoodsFrom", SqlDbType.VarChar,50),
                    new SqlParameter("@Identification", SqlDbType.Int,4),
                    new SqlParameter("@StockYungouma", SqlDbType.VarChar,50),
                    new SqlParameter("@GoodsSell", SqlDbType.Int,4),    };
            parameters[0].Value = model.GoodsName;
            parameters[1].Value = model.explain;
            parameters[2].Value = model.GoodsImg;
            parameters[3].Value = model.ImgCounts;
            parameters[4].Value = model.GoodsValue;
            parameters[5].Value = model.periods;
            parameters[6].Value = model.Number;
            parameters[7].Value = model.Addtime;
            parameters[8].Value = model.OverTime;
            parameters[9].Value = model.GoodsType;
            parameters[10].Value = model.RemainingTime;
            parameters[11].Value = model.Winner;
            parameters[12].Value = model.lotteryTime;
            parameters[13].Value = model.statue;
            parameters[14].Value = model.isDone;
            parameters[15].Value = model.GoodsFrom;
            parameters[16].Value = model.Identification;
            parameters[17].Value = model.StockYungouma;
            parameters[18].Value = model.GoodsSell;

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
        /// 更新一个goodsType数据
        /// </summary>
        public void UpdateGoodsType(long Id, int GoodsType)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GoodsList set ");
            strSql.Append("GoodsType=@GoodsType ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt),
		        	new SqlParameter("@GoodsType", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            parameters[1].Value = GoodsType;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一个isdone数据
        /// </summary>
        public void UpdateisDone(long Id, int isDone)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GoodsList set");
            strSql.Append("isDone=@isDone ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt),
					new SqlParameter("@isDone", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            parameters[1].Value = isDone;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        
         /// <summary>
        /// 更新一个yungouma数据
        /// </summary>
        public void UpdateYunGouMa(long Id, string StockYungouma)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GoodsList set ");
            strSql.Append("StockYungouma=@StockYungouma ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt),
		        	  new SqlParameter("@StockYungouma", SqlDbType.VarChar,50)};
            parameters[0].Value = Id;
            parameters[1].Value = StockYungouma;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 更新一个Number数据
        /// </summary>
        public void UpdateNum(long Id, long StockYungouma)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GoodsList set ");
            strSql.Append("StockYungouma=@StockYungouma ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt),
					new SqlParameter("@StockYungouma", SqlDbType.BigInt)};
            parameters[0].Value = Id;
            parameters[1].Value = StockYungouma;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.GoodsList model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_GoodsList set ");
            strSql.Append("GoodsName=@GoodsName,");
            strSql.Append("explain=@explain,");
            strSql.Append("GoodsImg=@GoodsImg,");
            strSql.Append("ImgCounts=@ImgCounts,");
            strSql.Append("GoodsValue=@GoodsValue,");
            strSql.Append("periods=@periods,");
            strSql.Append("Number=@Number,");
            strSql.Append("Addtime=@Addtime,");
            strSql.Append("OverTime=@OverTime,");
            strSql.Append("GoodsType=@GoodsType,");
            strSql.Append("RemainingTime=@RemainingTime,");
            strSql.Append("Winner=@Winner,");
            strSql.Append("lotteryTime=@lotteryTime,");
            strSql.Append("statue=@statue,");
            strSql.Append("isDone=@isDone,");
            strSql.Append("GoodsFrom=@GoodsFrom,");
            strSql.Append("Identification=@Identification,");
            strSql.Append("StockYungouma=@StockYungouma,");
            strSql.Append("GoodsSell=@GoodsSell ");
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt,8),
					new SqlParameter("@GoodsName", SqlDbType.VarChar,50),
					new SqlParameter("@explain", SqlDbType.VarChar),
					new SqlParameter("@GoodsImg", SqlDbType.VarChar),
					new SqlParameter("@ImgCounts", SqlDbType.Int,4),
					new SqlParameter("@GoodsValue", SqlDbType.BigInt,8),
					new SqlParameter("@periods", SqlDbType.BigInt,8),
					new SqlParameter("@Number", SqlDbType.BigInt,8),
					new SqlParameter("@Addtime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@GoodsType", SqlDbType.Int,4),
					new SqlParameter("@RemainingTime", SqlDbType.DateTime),
					new SqlParameter("@Winner", SqlDbType.VarChar,50),
					new SqlParameter("@lotteryTime", SqlDbType.DateTime),
					new SqlParameter("@statue", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@GoodsFrom", SqlDbType.VarChar,50),
					new SqlParameter("@Identification", SqlDbType.Int,4),
					new SqlParameter("@StockYungouma", SqlDbType.VarChar),
                    new SqlParameter("@GoodsSell", SqlDbType.Int,4),};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.GoodsName;
            parameters[2].Value = model.explain;
            parameters[3].Value = model.GoodsImg;
            parameters[4].Value = model.ImgCounts;
            parameters[5].Value = model.GoodsValue;
            parameters[6].Value = model.periods;
            parameters[7].Value = model.Number;
            parameters[8].Value = model.Addtime;
            parameters[9].Value = model.OverTime;
            parameters[10].Value = model.GoodsType;
            parameters[11].Value = model.RemainingTime;
            parameters[12].Value = model.Winner;
            parameters[13].Value = model.lotteryTime;
            parameters[14].Value = model.statue;
            parameters[15].Value = model.isDone;
            parameters[16].Value = model.GoodsFrom;
            parameters[17].Value = model.Identification;
            parameters[18].Value = model.StockYungouma;
            parameters[19].Value = model.GoodsSell;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(long Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_GoodsList ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
			parameters[0].Value = Id;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.GoodsList GetGoodsList(long Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select * from tb_GoodsList ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
			parameters[0].Value = Id;

			BCW.Model.GoodsList model=new BCW.Model.GoodsList();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
                    model.Id = reader.GetInt64(0);
                    model.GoodsName = reader.GetString(1);
                    model.explain = reader.GetString(2);
                    model.GoodsImg = reader.GetString(3);
                    model.ImgCounts = reader.GetInt32(4);
                    model.GoodsValue = reader.GetInt64(5);
                    model.periods = reader.GetInt64(6);
                    model.Number = reader.GetInt64(7);
                    model.Addtime = reader.GetDateTime(8);
                    model.OverTime = reader.GetDateTime(9);
                    model.GoodsType = reader.GetInt32(10);
                    model.RemainingTime = reader.GetDateTime(11);
                    model.Winner = reader.GetString(12);
                    model.lotteryTime = reader.GetDateTime(13);
                    model.statue = reader.GetInt32(14);
                    model.isDone = reader.GetInt32(15);
                    model.GoodsFrom = reader.GetString(16);
                    model.Identification = reader.GetInt32(17);
                    model.StockYungouma = reader.GetString(18);
                    model.GoodsSell = reader.GetInt32(19);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_GoodsList ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}
      //  GetGoodsListsForGoodsOpen
            /// <summary>
		/// 开奖使用
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList GoodsList</returns>
        public IList<BCW.Model.GoodsList> GetGoodsListsForGoodsOpen(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.GoodsList> listGoodsLists = new List<BCW.Model.GoodsList>();
            string sTable = "tb_GoodsList";
            string sPkey = "id";
            string sField = "Id,GoodsName,explain,GoodsImg,ImgCounts,GoodsValue,periods,Number,Addtime,OverTime,GoodsType,RemainingTime,Winner,lotteryTime,statue,isDone,GoodsFrom,Identification,StockYungouma,GoodsSell";
            string sCondition = strWhere;
            string sOrder = "OverTime Desc";
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
                    return listGoodsLists;
                }
                while (reader.Read())
                {
                    BCW.Model.GoodsList objGoodsList = new BCW.Model.GoodsList();
                    objGoodsList.Id = reader.GetInt64(0);
                    objGoodsList.GoodsName = reader.GetString(1);
                    objGoodsList.explain = reader.GetString(2);
                    objGoodsList.GoodsImg = reader.GetString(3);
                    objGoodsList.ImgCounts = reader.GetInt32(4);
                    objGoodsList.GoodsValue = reader.GetInt64(5);
                    objGoodsList.periods = reader.GetInt64(6);
                    objGoodsList.Number = reader.GetInt64(7);
                    objGoodsList.Addtime = reader.GetDateTime(8);
                    objGoodsList.OverTime = reader.GetDateTime(9);
                    objGoodsList.GoodsType = reader.GetInt32(10);
                    objGoodsList.RemainingTime = reader.GetDateTime(11);
                    objGoodsList.Winner = reader.GetString(12);
                    objGoodsList.lotteryTime = reader.GetDateTime(13);
                    objGoodsList.statue = reader.GetInt32(14);
                    objGoodsList.isDone = reader.GetInt32(15);
                    objGoodsList.GoodsFrom = reader.GetString(16);
                    objGoodsList.Identification = reader.GetInt32(17);
                    objGoodsList.StockYungouma = reader.GetString(18);
                    objGoodsList.GoodsSell = reader.GetInt32(19);    
                    listGoodsLists.Add(objGoodsList);
                }
            }
            return listGoodsLists;
        }
		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList GoodsList</returns>
        public IList<BCW.Model.GoodsList> GetGoodsLists(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.GoodsList> listGoodsLists = new List<BCW.Model.GoodsList>();
            string sTable = "tb_GoodsList";
            string sPkey = "id";
            string sField = "Id,GoodsName,explain,GoodsImg,ImgCounts,GoodsValue,periods,Number,Addtime,OverTime,GoodsType,RemainingTime,Winner,lotteryTime,statue,isDone,GoodsFrom,Identification,StockYungouma,GoodsSell";
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
                    return listGoodsLists;
                }
                while (reader.Read())
                {
                    BCW.Model.GoodsList objGoodsList = new BCW.Model.GoodsList();
                    objGoodsList.Id = reader.GetInt64(0);
                    objGoodsList.GoodsName = reader.GetString(1);
                    objGoodsList.explain = reader.GetString(2);
                    objGoodsList.GoodsImg = reader.GetString(3);
                    objGoodsList.ImgCounts = reader.GetInt32(4);
                    objGoodsList.GoodsValue = reader.GetInt64(5);
                    objGoodsList.periods = reader.GetInt64(6);
                    objGoodsList.Number = reader.GetInt64(7);
                    objGoodsList.Addtime = reader.GetDateTime(8);
                    objGoodsList.OverTime = reader.GetDateTime(9);
                    objGoodsList.GoodsType = reader.GetInt32(10);
                    objGoodsList.RemainingTime = reader.GetDateTime(11);
                    objGoodsList.Winner = reader.GetString(12);
                    objGoodsList.lotteryTime = reader.GetDateTime(13);
                    objGoodsList.statue = reader.GetInt32(14);
                    objGoodsList.isDone = reader.GetInt32(15);
                    objGoodsList.GoodsFrom = reader.GetString(16);
                    objGoodsList.Identification = reader.GetInt32(17);
                    objGoodsList.StockYungouma = reader.GetString(18);
                    objGoodsList.GoodsSell = reader.GetInt32(19);  
                    listGoodsLists.Add(objGoodsList);
                }
            }
            return listGoodsLists;
        }

        #endregion  成员方法
    }
}

