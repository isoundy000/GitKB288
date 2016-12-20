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
	/// 数据访问类yg_BuyLists。
	/// </summary>
	public class yg_BuyLists
	{
		public yg_BuyLists()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_yg_BuyLists");
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
            return SqlHelper.GetMaxID("Id", "tb_yg_BuyLists");
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.yg_BuyLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into tb_yg_BuyLists(");
            strSql.Append("UserId,GoodsNum,yungouma,Counts,Ip,System,Address,BuyTime,IsGet)");
            strSql.Append(" values (");
            strSql.Append("@UserId,@GoodsNum,@yungouma,@Counts,@Ip,@System,@Address,@BuyTime,@IsGet)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.BigInt,8),
					new SqlParameter("@yungouma", SqlDbType.VarChar),
					new SqlParameter("@Counts", SqlDbType.Int,4),
					new SqlParameter("@Ip", SqlDbType.VarChar,50),
					new SqlParameter("@System", SqlDbType.VarChar,50),
					new SqlParameter("@Address", SqlDbType.VarChar,50),
					new SqlParameter("@BuyTime", SqlDbType.DateTime),
					new SqlParameter("@IsGet", SqlDbType.Int,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.GoodsNum;
            parameters[2].Value = model.yungouma;
            parameters[3].Value = model.Counts;
            parameters[4].Value = model.Ip;
            parameters[5].Value = model.System;
            parameters[6].Value = model.Address;
            parameters[7].Value = model.BuyTime;
            parameters[8].Value = model.IsGet;

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
        public void Update(BCW.Model.yg_BuyLists model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_yg_BuyLists set ");
            strSql.Append("UserId=@UserId,");
            strSql.Append("GoodsNum=@GoodsNum,");
            strSql.Append("yungouma=@yungouma,");
            strSql.Append("Counts=@Counts,");
            strSql.Append("Ip=@Ip,");
            strSql.Append("System=@System,");
            strSql.Append("Address=@Address,");
            strSql.Append("BuyTime=@BuyTime,");
            strSql.Append("IsGet=@IsGet");
            strSql.Append(" where Id=@Id and IsGet=@IsGet and UserId=@UserId and GoodsNum=@GoodsNum and yungouma=@yungouma and Counts=@Counts and Ip=@Ip and System=@System and Address=@Address and BuyTime=@BuyTime ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt,8),
					new SqlParameter("@UserId", SqlDbType.VarChar,50),
					new SqlParameter("@GoodsNum", SqlDbType.BigInt,8),
					new SqlParameter("@yungouma", SqlDbType.VarChar),
					new SqlParameter("@Counts", SqlDbType.Int,4),
					new SqlParameter("@Ip", SqlDbType.VarChar,50),
					new SqlParameter("@System", SqlDbType.VarChar,50),
					new SqlParameter("@Address", SqlDbType.VarChar,50),
					new SqlParameter("@BuyTime", SqlDbType.DateTime),
					new SqlParameter("@IsGet", SqlDbType.Int,4)};
            parameters[0].Value = model.Id;
            parameters[1].Value = model.UserId;
            parameters[2].Value = model.GoodsNum;
            parameters[3].Value = model.yungouma;
            parameters[4].Value = model.Counts;
            parameters[5].Value = model.Ip;
            parameters[6].Value = model.System;
            parameters[7].Value = model.Address;
            parameters[8].Value = model.BuyTime;
            parameters[9].Value = model.IsGet;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新address
        /// </summary>
        public void UpdateAddress(long ID,string address)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_yg_BuyLists set ");
            strSql.Append("Address=@address ");
            strSql.Append(" where ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@address", SqlDbType.Int,4)};
            parameters[0].Value = ID;
            parameters[1].Value = address;
            SqlHelperUser.ExecuteSql(strSql.ToString(), parameters);
        }
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(long Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_yg_BuyLists ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
			parameters[0].Value = Id;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 根据商品编号判断云购码是否存在，传入商品编号，新云购码数
        /// </summary>
        public bool Getyg_BuyListsForYungouma(int Id,int yungouma)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_yg_BuyLists ");
            strSql.Append(" where GoodsNum=@Id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;
            bool l = false;
            BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        model.Id = reader.GetInt64(0);
                        model.UserId = reader.GetString(1);
                        model.GoodsNum = reader.GetInt64(2);
                        model.yungouma = reader.GetString(3);
                        model.Counts = reader.GetInt32(4);
                        model.Ip = reader.GetString(5);
                        model.System = reader.GetString(6);
                        model.Address = reader.GetString(7);
                        model.BuyTime = reader.GetDateTime(8);  
                        model.IsGet = reader.GetInt32(9);
                        string[] yun = model.yungouma.Split(',');
                        //string[] sArray = Regex.Split(model.yungouma, ",", RegexOptions.IgnoreCase);
                        //for (int i = 0; i < model.Counts; i++)
                        foreach (string i in yun)
                        {
                            //int temp = int.Parse(i);
                            if (i == yungouma.ToString())
                            {
                                l = true;
                            }
                        }                      // return model.yungouma;
                    }
                   
                } 
            }
            return l;

        }
        public long  GetUserId_yg_BuyListsForYungouma(int Id, int r)//返回获奖用户id
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from tb_yg_BuyLists ");
            strSql.Append(" where GoodsNum=@Id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;
            BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
            long modelHave = 0;
           
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                while (reader.Read())
                {
                    if (reader.HasRows)
                    {
                        model.Id = reader.GetInt64(0);
                        model.UserId = reader.GetString(1);
                        model.GoodsNum = reader.GetInt64(2);
                        model.yungouma = reader.GetString(3);
                        model.Counts = reader.GetInt32(4);
                        model.Ip = reader.GetString(5);
                        model.System = reader.GetString(6);
                        model.Address = reader.GetString(7);
                        model.BuyTime = reader.GetDateTime(8);
                        model.IsGet = reader.GetInt32(9);
                        string[] yun = model.yungouma.Split(',');
                        foreach (string i in yun)
                        {                        
                            if (i == r.ToString())
                            {   //userId = model..ToString();
                                modelHave = model.Id;
                                return model.Id;
                            }
                        }                    
                    }

                }
            }
            return modelHave;
        }
       /// <summary>
		/// 得到某商品的最新购买行,传入商品id
		/// </summary>
        public BCW.Model.yg_BuyLists Getyg_BuyListsForGoods(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 * from tb_yg_BuyLists ");
            strSql.Append(" where GoodsNum=@Id order by Id desc ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
            parameters[0].Value = Id;

            BCW.Model.yg_BuyLists model = new BCW.Model.yg_BuyLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt64(0);
                    model.UserId = reader.GetString(1);
                    model.GoodsNum = reader.GetInt64(2);
                    model.yungouma = reader.GetString(3);
                    model.Counts = reader.GetInt32(4);
                    model.Ip = reader.GetString(5);
                    model.System = reader.GetString(6);
                    model.Address = reader.GetString(7);
                    model.BuyTime = reader.GetDateTime(8);
                    model.IsGet = reader.GetInt32(9);
                    return model;
                }
                else
                {
                    return null;
                }
            }

        }
        /// <summary>
        /// 得到用户Id
        /// </summary>
        public long GetUserId(long Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserId from tb_yg_BuyLists ");
            strSql.Append(" where Id=@Id ");
            SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
            parameters[0].Value = Id;
            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetInt64(0);
                }
                else
                {
                    return 0;
                }
            }
        }
		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.yg_BuyLists Getyg_BuyLists(long Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  *  from tb_yg_BuyLists ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.BigInt)};
			parameters[0].Value = Id;

			BCW.Model.yg_BuyLists model=new BCW.Model.yg_BuyLists();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
                    reader.Read();
                    model.Id = reader.GetInt64(0);
                    model.UserId = reader.GetString(1);
                    model.GoodsNum = reader.GetInt64(2);
                    model.yungouma = reader.GetString(3);
                    model.Counts = reader.GetInt32(4);
                    model.Ip = reader.GetString(5);
                    model.System = reader.GetString(6);
                    model.Address = reader.GetString(7);
                    model.BuyTime = reader.GetDateTime(8);
                    model.IsGet = reader.GetInt32(9);
                    return model;
				}
				else
				{
				return null;
				}
			}
		}

        /// <summary>
        /// 取某Id的前100条记录
        /// </summary>
        public DataSet GetListTopForId(int Id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select Top 100 * ");
            strSql.Append(" FROM tb_yg_BuyLists ");
            strSql.Append(" Order by BuyTime Desc");
            strSql.Append(" where  Id=@Id");    
            return SqlHelper.Query(strSql.ToString());
        }
      
        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetListTop(string strField, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  " +strField + "" );
            strSql.Append(" FROM tb_yg_BuyLists ");
            strSql.Append(" Order by BuyTime Desc");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return SqlHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 根据Id取此Id前100条记录
        /// </summary>
        public DataSet GetListTopId(long Id)
        {
            long start = Id - 100;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *");
            strSql.Append(" FROM tb_yg_BuyLists ");
            strSql.Append("where Id>="+start+" and Id<=@Id");          
            return SqlHelper.Query(strSql.ToString());
        }

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_yg_BuyLists ");
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
		/// <returns>IList yg_BuyLists</returns>
        public IList<BCW.Model.yg_BuyLists> Getyg_BuyListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            IList<BCW.Model.yg_BuyLists> listyg_BuyListss = new List<BCW.Model.yg_BuyLists>();
            string sTable = "tb_yg_BuyLists";
            string sPkey = "id";
            string sField = "Id,UserId,GoodsNum,yungouma,Counts,Ip,System,Address,BuyTime,IsGet";
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
                    return listyg_BuyListss;
                }
                while (reader.Read())
                {
                    BCW.Model.yg_BuyLists objyg_BuyLists = new BCW.Model.yg_BuyLists();
                    objyg_BuyLists.Id = reader.GetInt64(0);
                    objyg_BuyLists.UserId = reader.GetString(1);
                    objyg_BuyLists.GoodsNum = reader.GetInt64(2);
                    objyg_BuyLists.yungouma = reader.GetString(3);
                    objyg_BuyLists.Counts = reader.GetInt32(4);
                    objyg_BuyLists.Ip = reader.GetString(5);
                    objyg_BuyLists.System = reader.GetString(6);
                    objyg_BuyLists.Address = reader.GetString(7);
                    objyg_BuyLists.BuyTime = reader.GetDateTime(8);
                    objyg_BuyLists.IsGet = reader.GetInt32(9);
                    listyg_BuyListss.Add(objyg_BuyLists);
                }
            }
            return listyg_BuyListss;
        }

        #endregion  成员方法
    }
}

