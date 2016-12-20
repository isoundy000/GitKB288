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
	/// 数据访问类yg_OrderLists。
	/// </summary>
	public class yg_OrderLists
	{
		public yg_OrderLists()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_yg_OrderLists");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsGoodsId(int GoodsListId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_yg_OrderLists");
            strSql.Append(" where GoodsListId=@GoodsListId ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsListId", SqlDbType.Int,4)};
            parameters[0].Value = GoodsListId;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.yg_OrderLists model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_yg_OrderLists(");
            strSql.Append("GoodsListId,BuyListId,AddTime,PostTime,OverTime,wuliu,yundanhao,Operator,OperatorNotes,OperatorStatue,wuliuStatue,Consignee,Address,ZipCode,PhoneMobile,PhoneHome,ConsigneeNotes,ConsigneeStatue,Statue,isDone,Spare,brack,UserId)");
			strSql.Append(" values (");
			strSql.Append("@GoodsListId,@BuyListId,@AddTime,@PostTime,@OverTime,@wuliu,@yundanhao,@Operator,@OperatorNotes,@OperatorStatue,@wuliuStatue,@Consignee,@Address,@ZipCode,@PhoneMobile,@PhoneHome,@ConsigneeNotes,@ConsigneeStatue,@Statue,@isDone,@Spare,@brack,@UserId)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@GoodsListId", SqlDbType.BigInt,8),
					new SqlParameter("@BuyListId", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PostTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@wuliu", SqlDbType.VarChar,50),
					new SqlParameter("@yundanhao", SqlDbType.NVarChar,50),
					new SqlParameter("@Operator", SqlDbType.NVarChar,50),
					new SqlParameter("@OperatorNotes", SqlDbType.NVarChar,50),
					new SqlParameter("@OperatorStatue", SqlDbType.Int,4),
					new SqlParameter("@wuliuStatue", SqlDbType.Int,4),
					new SqlParameter("@Consignee", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar),
					new SqlParameter("@ZipCode", SqlDbType.Int,4),
					new SqlParameter("@PhoneMobile", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneHome", SqlDbType.NVarChar,50),
					new SqlParameter("@ConsigneeNotes", SqlDbType.NVarChar),
					new SqlParameter("@ConsigneeStatue", SqlDbType.NVarChar),
					new SqlParameter("@Statue", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@Spare", SqlDbType.NVarChar,50),
					new SqlParameter("@brack", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
			parameters[0].Value = model.GoodsListId;
			parameters[1].Value = model.BuyListId;
			parameters[2].Value = model.AddTime;
			parameters[3].Value = model.PostTime;
			parameters[4].Value = model.OverTime;
			parameters[5].Value = model.wuliu;
			parameters[6].Value = model.yundanhao;
			parameters[7].Value = model.Operator;
			parameters[8].Value = model.OperatorNotes;
			parameters[9].Value = model.OperatorStatue;
			parameters[10].Value = model.wuliuStatue;
			parameters[11].Value = model.Consignee;
			parameters[12].Value = model.Address;
			parameters[13].Value = model.ZipCode;
			parameters[14].Value = model.PhoneMobile;
			parameters[15].Value = model.PhoneHome;
			parameters[16].Value = model.ConsigneeNotes;
			parameters[17].Value = model.ConsigneeStatue;
			parameters[18].Value = model.Statue;
			parameters[19].Value = model.isDone;
			parameters[20].Value = model.Spare;
            parameters[21].Value = model.brack;
			parameters[22].Value = model.UserId;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public void Update(BCW.Model.yg_OrderLists model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_yg_OrderLists set ");
			strSql.Append("GoodsListId=@GoodsListId,");
			strSql.Append("BuyListId=@BuyListId,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("PostTime=@PostTime,");
			strSql.Append("OverTime=@OverTime,");
			strSql.Append("wuliu=@wuliu,");
			strSql.Append("yundanhao=@yundanhao,");
			strSql.Append("Operator=@Operator,");
			strSql.Append("OperatorNotes=@OperatorNotes,");
			strSql.Append("OperatorStatue=@OperatorStatue,");
			strSql.Append("wuliuStatue=@wuliuStatue,");
			strSql.Append("Consignee=@Consignee,");
			strSql.Append("Address=@Address,");
			strSql.Append("ZipCode=@ZipCode,");
			strSql.Append("PhoneMobile=@PhoneMobile,");
			strSql.Append("PhoneHome=@PhoneHome,");
			strSql.Append("ConsigneeNotes=@ConsigneeNotes,");
			strSql.Append("ConsigneeStatue=@ConsigneeStatue,");
			strSql.Append("Statue=@Statue,");
			strSql.Append("isDone=@isDone,");
			strSql.Append("Spare=@Spare,");
            strSql.Append("brack=@brack,");
			strSql.Append("UserId=@UserId");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4),
					new SqlParameter("@GoodsListId", SqlDbType.BigInt,8),
					new SqlParameter("@BuyListId", SqlDbType.BigInt,8),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@PostTime", SqlDbType.DateTime),
					new SqlParameter("@OverTime", SqlDbType.DateTime),
					new SqlParameter("@wuliu", SqlDbType.VarChar,50),
					new SqlParameter("@yundanhao", SqlDbType.NVarChar,50),
					new SqlParameter("@Operator", SqlDbType.NVarChar,50),
					new SqlParameter("@OperatorNotes", SqlDbType.NVarChar,50),
					new SqlParameter("@OperatorStatue", SqlDbType.Int,4),
					new SqlParameter("@wuliuStatue", SqlDbType.Int,4),
					new SqlParameter("@Consignee", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar),
					new SqlParameter("@ZipCode", SqlDbType.Int,4),
					new SqlParameter("@PhoneMobile", SqlDbType.NVarChar,50),
					new SqlParameter("@PhoneHome", SqlDbType.NVarChar,50),
					new SqlParameter("@ConsigneeNotes", SqlDbType.NVarChar),
					new SqlParameter("@ConsigneeStatue", SqlDbType.NVarChar),
					new SqlParameter("@Statue", SqlDbType.Int,4),
					new SqlParameter("@isDone", SqlDbType.Int,4),
					new SqlParameter("@Spare", SqlDbType.NVarChar,50),
					new SqlParameter("@brack", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4)};
			parameters[0].Value = model.Id;
			parameters[1].Value = model.GoodsListId;
			parameters[2].Value = model.BuyListId;
			parameters[3].Value = model.AddTime;
			parameters[4].Value = model.PostTime;
			parameters[5].Value = model.OverTime;
			parameters[6].Value = model.wuliu;
			parameters[7].Value = model.yundanhao;
			parameters[8].Value = model.Operator;
			parameters[9].Value = model.OperatorNotes;
			parameters[10].Value = model.OperatorStatue;
			parameters[11].Value = model.wuliuStatue;
			parameters[12].Value = model.Consignee;
			parameters[13].Value = model.Address;
			parameters[14].Value = model.ZipCode;
			parameters[15].Value = model.PhoneMobile;
			parameters[16].Value = model.PhoneHome;
			parameters[17].Value = model.ConsigneeNotes;
			parameters[18].Value = model.ConsigneeStatue;
			parameters[19].Value = model.Statue;
			parameters[20].Value = model.isDone;
			parameters[21].Value = model.Spare;
            parameters[22].Value = model.brack;
			parameters[23].Value = model.UserId;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_yg_OrderLists ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.yg_OrderLists Getyg_OrderListsForGoodsListId(long GoodsListId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_yg_OrderLists ");
            strSql.Append(" where GoodsListId=@GoodsListId");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsListId", SqlDbType.Int,4)};
            parameters[0].Value = GoodsListId;

            BCW.Model.yg_OrderLists model = new BCW.Model.yg_OrderLists();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.Id = reader.GetInt32(0);
                    model.GoodsListId = reader.GetInt64(1);
                    model.BuyListId = reader.GetInt64(2);
                    model.AddTime = reader.GetDateTime(3);
                    model.PostTime = reader.GetDateTime(4);
                    model.OverTime = reader.GetDateTime(5);
                    model.wuliu = reader.GetString(6);
                    model.yundanhao = reader.GetString(7);
                    model.Operator = reader.GetString(8);
                    model.OperatorNotes = reader.GetString(9);
                    model.OperatorStatue = reader.GetInt32(10);
                    model.wuliuStatue = reader.GetInt32(11);
                    model.Consignee = reader.GetString(12);
                    model.Address = reader.GetString(13);
                    model.ZipCode = reader.GetInt32(14);
                    model.PhoneMobile = reader.GetString(15);
                    model.PhoneHome = reader.GetString(16);
                    model.ConsigneeNotes = reader.GetString(17);
                    model.ConsigneeStatue = reader.GetString(18);
                    model.Statue = reader.GetInt32(19);
                    model.isDone = reader.GetInt32(20);
                    model.Spare = reader.GetString(21);
                    model.brack = reader.GetString(22);
                    model.UserId = reader.GetInt32(23);
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
		public BCW.Model.yg_OrderLists Getyg_OrderLists(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 Id,GoodsListId,BuyListId,AddTime,PostTime,OverTime,wuliu,yundanhao,Operator,OperatorNotes,OperatorStatue,wuliuStatue,Consignee,Address,ZipCode,PhoneMobile,PhoneHome,ConsigneeNotes,ConsigneeStatue,Statue,isDone,Spare,brack,UserId from tb_yg_OrderLists ");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			BCW.Model.yg_OrderLists model=new BCW.Model.yg_OrderLists();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.Id = reader.GetInt32(0);
					model.GoodsListId = reader.GetInt64(1);
					model.BuyListId = reader.GetInt64(2);
					model.AddTime = reader.GetDateTime(3);
					model.PostTime = reader.GetDateTime(4);
					model.OverTime = reader.GetDateTime(5);
					model.wuliu = reader.GetString(6);
					model.yundanhao = reader.GetString(7);
					model.Operator = reader.GetString(8);
					model.OperatorNotes = reader.GetString(9);
					model.OperatorStatue = reader.GetInt32(10);
					model.wuliuStatue = reader.GetInt32(11);
					model.Consignee = reader.GetString(12);
					model.Address = reader.GetString(13);
					model.ZipCode = reader.GetInt32(14);
					model.PhoneMobile = reader.GetString(15);
					model.PhoneHome = reader.GetString(16);
					model.ConsigneeNotes = reader.GetString(17);
					model.ConsigneeStatue = reader.GetString(18);
					model.Statue = reader.GetInt32(19);
					model.isDone = reader.GetInt32(20);
					model.Spare = reader.GetString(21);
                    model.brack = reader.GetString(22);
					model.UserId = reader.GetInt32(23);
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
			strSql.Append(" FROM tb_yg_OrderLists ");
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
		/// <returns>IList yg_OrderLists</returns>
		public IList<BCW.Model.yg_OrderLists> Getyg_OrderListss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.yg_OrderLists> listyg_OrderListss = new List<BCW.Model.yg_OrderLists>();
			string sTable = "tb_yg_OrderLists";
			string sPkey = "id";
            string sField = "Id,GoodsListId,BuyListId,AddTime,PostTime,OverTime,wuliu,yundanhao,Operator,OperatorNotes,OperatorStatue,wuliuStatue,Consignee,Address,ZipCode,PhoneMobile,PhoneHome,ConsigneeNotes,ConsigneeStatue,Statue,isDone,Spare,brack,UserId";
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
					return listyg_OrderListss;
				}
				while (reader.Read())
				{
						BCW.Model.yg_OrderLists objyg_OrderLists = new BCW.Model.yg_OrderLists();
						objyg_OrderLists.Id = reader.GetInt32(0);
						objyg_OrderLists.GoodsListId = reader.GetInt64(1);
						objyg_OrderLists.BuyListId = reader.GetInt64(2);
						objyg_OrderLists.AddTime = reader.GetDateTime(3);
						objyg_OrderLists.PostTime = reader.GetDateTime(4);
						objyg_OrderLists.OverTime = reader.GetDateTime(5);
						objyg_OrderLists.wuliu = reader.GetString(6);
						objyg_OrderLists.yundanhao = reader.GetString(7);
						objyg_OrderLists.Operator = reader.GetString(8);
						objyg_OrderLists.OperatorNotes = reader.GetString(9);
						objyg_OrderLists.OperatorStatue = reader.GetInt32(10);
						objyg_OrderLists.wuliuStatue = reader.GetInt32(11);
						objyg_OrderLists.Consignee = reader.GetString(12);
						objyg_OrderLists.Address = reader.GetString(13);
						objyg_OrderLists.ZipCode = reader.GetInt32(14);
						objyg_OrderLists.PhoneMobile = reader.GetString(15);
						objyg_OrderLists.PhoneHome = reader.GetString(16);
						objyg_OrderLists.ConsigneeNotes = reader.GetString(17);
						objyg_OrderLists.ConsigneeStatue = reader.GetString(18);
						objyg_OrderLists.Statue = reader.GetInt32(19);
						objyg_OrderLists.isDone = reader.GetInt32(20);
						objyg_OrderLists.Spare = reader.GetString(21);
                        objyg_OrderLists.brack = reader.GetString(22);
						objyg_OrderLists.UserId = reader.GetInt32(23);
						listyg_OrderListss.Add(objyg_OrderLists);
				}
			}
			return listyg_OrderListss;
		}

		#endregion  成员方法
	}
}

