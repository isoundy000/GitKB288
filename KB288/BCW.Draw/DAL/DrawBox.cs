using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Draw.DAL
{
	/// <summary>
	/// 数据访问类DrawBox。
	/// </summary>
	public class DrawBox
	{
		public DrawBox()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_DrawBox"); 
		}


//----------------------------------------------------------------//
        /// <summary>
        /// 得到用
        /// </summary>
        public string GetGoodsName(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsName from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用
        /// </summary>
        public string GetExplain(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Explain from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用
        /// </summary>
        public string GetGoodsImg(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsImg from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
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
        /// 得到用
        /// </summary>
        public int GetGoodsType(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsType from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到用
        /// </summary>
        public int GetGoodsValue(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsValue from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到用
        /// </summary>
        public int GetGoodsNum(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GoodsNum from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到用
        /// </summary>
        public int GetRank(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Rank from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到用
        /// </summary>
        public int GetID(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到一个奖品的数量
        /// </summary>
        public int GetAllNumbyC(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AllNum from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
        /// 得到进行中的奖品的总数量
        /// </summary>
        public int GetAllNum(int lun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(AllNum) from tb_DrawBox ");
            strSql.Append(" where lun=@lun ");
            SqlParameter[] parameters = {
					new SqlParameter("@lun", SqlDbType.Int,4)};
            parameters[0].Value = lun;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    if(lun !=0){
                    reader.Read();
                    return reader.GetInt32(0);}
                    else{
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
        /// 得到进行中的奖品的总余量
        /// </summary>
        public int GetAllNumS(int lun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select sum(GoodsNum) from tb_DrawBox ");
            strSql.Append(" where lun=@lun and Statue=0 ");
            SqlParameter[] parameters = {
					new SqlParameter("@lun", SqlDbType.Int,4)};
            parameters[0].Value = lun;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    if (lun != 0)
                    {
                        reader.Read();
                        return reader.GetInt32(0);
                    }
                    else
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
        /// 得到第几轮奖池
        /// </summary>
        public int Getlun()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top(1) lun from tb_DrawBox ");
            strSql.Append(" where lun!=0 Order by lun ");
            SqlParameter[] parameters = {
					new SqlParameter("@lun", SqlDbType.Int,4)};
            parameters[0].Value = 0;

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
        /// 得到用
        /// </summary>
        public int GetStatue(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Statue from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

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
//----------------------------------------------------------------//
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DrawBox");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录根据编号
        /// </summary>
        public bool CountsExists(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_DrawBox");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

        //---------------------------根据更新
        public void UpdateStatue(int GoodsCounts,  int Statue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append("Statue=@Statue");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@Statue", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = Statue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 所有商品下架
        /// </summary>
        /// <param name="GoodsCounts"></param>
        /// <param name="Statue"></param>
        public void UpdateStatueAllgo()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append("Statue=5");
            strSql.Append(" where Statue!=1 ");
            SqlParameter[] parameters = {
                    new SqlParameter("@Statue", SqlDbType.Int,4)};
            parameters[0].Value = 5;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void UpdateGoodsNum(int GoodsCounts, int GoodsNum)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append("GoodsNum=@GoodsNum");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@GoodsNum", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = GoodsNum;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void UpdateGoodsNumgo(int lun)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append("GoodsNum=@GoodsNum");
            strSql.Append(" where lun=@lun");
            SqlParameter[] parameters = {
					new SqlParameter("@lun", SqlDbType.Int,4),
                    new SqlParameter("@GoodsNum", SqlDbType.Int,4)};
            parameters[0].Value = lun;
            parameters[1].Value = 0;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        public void UpdateOverTime(int GoodsCounts, DateTime OverTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append("OverTime=@OverTime");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@OverTime", SqlDbType.DateTime)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = OverTime;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

                public void UpdateReceiveTime(int GoodsCounts, DateTime  ReceiveTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawBox set ");
            strSql.Append(" ReceiveTime=@ReceiveTime");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@ReceiveTime", SqlDbType.DateTime)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = ReceiveTime;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Draw.Model.DrawBox model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_DrawBox(");
            strSql.Append("GoodsName,Explain,GoodsImg,GoodsType,GoodsValue,GoodsNum,AddTime,OverTime,ReceiveTime,GoodsCounts,Statue,beizhu,Rank,Points,AllNum,lun)");
            strSql.Append(" values (");
            strSql.Append("@GoodsName,@Explain,@GoodsImg,@GoodsType,@GoodsValue,@GoodsNum,@AddTime,@OverTime,@ReceiveTime,@GoodsCounts,@Statue,@beizhu,@Rank,@Points,@AllNum,@lun)");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@GoodsImg", SqlDbType.NVarChar),
					new SqlParameter("@GoodsType", SqlDbType.Int,4),
					new SqlParameter("@GoodsValue", SqlDbType.Int,4),
					new SqlParameter("@GoodsNum", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@ReceiveTime", SqlDbType.DateTime),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Statue", SqlDbType.Int,4),
                    new SqlParameter("@beizhu", SqlDbType.NVarChar,50),
                    new SqlParameter("@rank", SqlDbType.Int,4),
                    new SqlParameter("@Points", SqlDbType.Int,4),
                                        new SqlParameter("@AllNum", SqlDbType.Int,4),
                                        new SqlParameter("@lun", SqlDbType.Int,4) };
            parameters[0].Value = model.GoodsName;
            parameters[1].Value = model.Explain;
            parameters[2].Value = model.GoodsImg;
            parameters[3].Value = model.GoodsType;
            parameters[4].Value = model.GoodsValue;
            parameters[5].Value = model.GoodsNum;
            parameters[6].Value = model.AddTime;
            parameters[7].Value = model.OverTime;
            parameters[8].Value = model.ReceiveTime;
            parameters[9].Value = model.GoodsCounts;
            parameters[10].Value = model.Statue;
            parameters[11].Value = model.beizhu;
            parameters[12].Value = model.rank;
            parameters[13].Value = model.points;
            parameters[14].Value = model.AllNum;
            parameters[15].Value = model.lun;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawBox model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DrawBox set ");
			strSql.Append("GoodsName=@GoodsName,");
			strSql.Append("Explain=@Explain,");
			strSql.Append("GoodsImg=@GoodsImg,");
			strSql.Append("GoodsType=@GoodsType,");
			strSql.Append("GoodsValue=@GoodsValue,");
			strSql.Append("GoodsNum=@GoodsNum,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("OverTime=@OverTime,");
			strSql.Append("ReceiveTime=@ReceiveTime,");
			strSql.Append("GoodsCounts=@GoodsCounts,");
			strSql.Append("Statue=@Statue,");
            strSql.Append("beizhu=@beizhu,");
            strSql.Append("rank=@rank,");
            strSql.Append("points=@points, ");
            strSql.Append("AllNum=@AllNum, ");
            strSql.Append("lun=@lun ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@GoodsName", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@GoodsImg", SqlDbType.NVarChar),
					new SqlParameter("@GoodsType", SqlDbType.Int,4),
					new SqlParameter("@GoodsValue", SqlDbType.Int,4),
					new SqlParameter("@GoodsNum", SqlDbType.Int,4),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@ReceiveTime", SqlDbType.DateTime),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Statue", SqlDbType.Int,4),
                    new SqlParameter("@beizhu", SqlDbType.NVarChar,50),
                    new SqlParameter("@rank", SqlDbType.Int,4),
                    new SqlParameter("@points", SqlDbType.Int,4),
                                       new SqlParameter("@AllNum", SqlDbType.Int,4),
                                          new SqlParameter("@lun", SqlDbType.Int,4) };
			parameters[0].Value = model.ID;
			parameters[1].Value = model.GoodsName;
			parameters[2].Value = model.Explain;
			parameters[3].Value = model.GoodsImg;
			parameters[4].Value = model.GoodsType;
			parameters[5].Value = model.GoodsValue;
			parameters[6].Value = model.GoodsNum;
			parameters[7].Value = model.AddTime;
			parameters[8].Value = model.OverTime;
			parameters[9].Value = model.ReceiveTime;
			parameters[10].Value = model.GoodsCounts;
			parameters[11].Value = model.Statue;
            parameters[12].Value = model.beizhu;
            parameters[13].Value = model.rank;
            parameters[14].Value = model.points;
            parameters[15].Value = model.AllNum;
            parameters[16].Value = model.lun;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DrawBox ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.DrawBox GetDrawBox(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,GoodsName,Explain,GoodsImg,GoodsType,GoodsValue,GoodsNum,AddTime,OverTime,ReceiveTime,GoodsCounts,Statue,beizhu,rank,points,AllNum,lun from tb_DrawBox ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.DrawBox model=new BCW.Draw.Model.DrawBox();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.GoodsName = reader.GetString(1);
					model.Explain = reader.GetString(2);
					model.GoodsImg = reader.GetString(3);
					model.GoodsType = reader.GetInt32(4);
					model.GoodsValue = reader.GetInt32(5);
					model.GoodsNum = reader.GetInt32(6);
					model.AddTime = reader.GetDateTime(7);
					model.OverTime = reader.GetDateTime(8);
					model.ReceiveTime = reader.GetDateTime(9);
					model.GoodsCounts = reader.GetInt32(10);
					model.Statue = reader.GetInt32(11);
                    model.beizhu = reader.GetString(12);
                    model.rank = reader.GetInt32(13);
                    model.points = reader.GetInt32(14);
                    model.AllNum = reader.GetInt32(15);
                    model.lun = reader.GetInt32(16);
					return model;
				}
				else
				{
				return null;
				}
			}
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawBox GetDrawBoxbyC(int GoodsCounts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,GoodsName,Explain,GoodsImg,GoodsType,GoodsValue,GoodsNum,AddTime,OverTime,ReceiveTime,GoodsCounts,Statue,beizhu,rank,points,AllNum,lun from tb_DrawBox ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            BCW.Draw.Model.DrawBox model = new BCW.Draw.Model.DrawBox();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.GoodsName = reader.GetString(1);
                    model.Explain = reader.GetString(2);
                    model.GoodsImg = reader.GetString(3);
                    model.GoodsType = reader.GetInt32(4);
                    model.GoodsValue = reader.GetInt32(5);
                    model.GoodsNum = reader.GetInt32(6);
                    model.AddTime = reader.GetDateTime(7);
                    model.OverTime = reader.GetDateTime(8);
                    model.ReceiveTime = reader.GetDateTime(9);
                    model.GoodsCounts = reader.GetInt32(10);
                    model.Statue = reader.GetInt32(11);
                    model.beizhu = reader.GetString(12);
                    model.rank = reader.GetInt32(13);
                    model.points = reader.GetInt32(14);
                    model.AllNum = reader.GetInt32(15);
                    model.lun = reader.GetInt32(16);
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
			strSql.Append(" FROM tb_DrawBox ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
		/// <returns>IList DrawBox</returns>
		public IList<BCW.Draw.Model.DrawBox> GetDrawBoxs(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			IList<BCW.Draw.Model.DrawBox> listDrawBoxs = new List<BCW.Draw.Model.DrawBox>();
			string sTable = "tb_DrawBox";
			string sPkey = "id";
			string sField = "ID,GoodsName,Explain,GoodsImg,GoodsType,GoodsValue,GoodsNum,AddTime,OverTime,ReceiveTime,GoodsCounts,Statue,beizhu,rank,points,AllNum,lun";
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
					return listDrawBoxs;
				}
				while (reader.Read())
				{
						BCW.Draw.Model.DrawBox objDrawBox = new BCW.Draw.Model.DrawBox();
						objDrawBox.ID = reader.GetInt32(0);
						objDrawBox.GoodsName = reader.GetString(1);
						objDrawBox.Explain = reader.GetString(2);
						objDrawBox.GoodsImg = reader.GetString(3);
						objDrawBox.GoodsType = reader.GetInt32(4);
						objDrawBox.GoodsValue = reader.GetInt32(5);
						objDrawBox.GoodsNum = reader.GetInt32(6);
						objDrawBox.AddTime = reader.GetDateTime(7);
						objDrawBox.OverTime = reader.GetDateTime(8);
						objDrawBox.ReceiveTime = reader.GetDateTime(9);
						objDrawBox.GoodsCounts = reader.GetInt32(10);
						objDrawBox.Statue = reader.GetInt32(11);
                        objDrawBox.beizhu = reader.GetString(12);
                        objDrawBox.rank = reader.GetInt32(13);
                        objDrawBox.points = reader.GetInt32(14);
                        objDrawBox.AllNum = reader.GetInt32(15);
                        objDrawBox.AllNum = reader.GetInt32(16);
						listDrawBoxs.Add(objDrawBox);
				}
			}
			return listDrawBoxs;
		}

		#endregion  成员方法
	}
}

