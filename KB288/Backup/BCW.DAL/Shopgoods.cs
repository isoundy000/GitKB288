using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.Shop.DAL
{
	/// <summary>
	/// 数据访问类Shopgoods。
	/// </summary>
	public class Shopgoods
	{
		public Shopgoods()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_Shopgoods"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Shopgoods");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopgoods");
            strSql.Append(" where UsID=@UsID ");
            strSql.Append(" and ID=@ID ");
            SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
            parameters[0].Value = UsID;
            parameters[1].Value = ID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Shop.Model.Shopgoods model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Shopgoods(");
            strSql.Append("Title,GiftId,PrevPic,Num,BuyTime,SendTime,ReceiveTime,UsID,Address,Phone,Email,RealName,Message,ShopGiftId,Express,Expressnum)");
			strSql.Append(" values (");
            strSql.Append("@Title,@GiftId,@PrevPic,@Num,@BuyTime,@SendTime,@ReceiveTime,@UsID,@Address,@Phone,@Email,@RealName,@Message,@ShopGiftId,@Express,@Expressnum)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Int,4),
					new SqlParameter("@BuyTime", SqlDbType.DateTime),
					new SqlParameter("@SendTime", SqlDbType.DateTime),
					new SqlParameter("@ReceiveTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,100),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Message", SqlDbType.NVarChar),
                                        new SqlParameter("@ShopGiftId", SqlDbType.Int,4),
                                        	new SqlParameter("@Express", SqlDbType.NVarChar,50),
					new SqlParameter("@Expressnum", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.Title;
			parameters[1].Value = model.GiftId;
			parameters[2].Value = model.PrevPic;
			parameters[3].Value = model.Num;
			parameters[4].Value = model.BuyTime;
			parameters[5].Value = model.SendTime;
			parameters[6].Value = model.ReceiveTime;
			parameters[7].Value = model.UsID;
			parameters[8].Value = model.Address;
			parameters[9].Value = model.Phone;
			parameters[10].Value = model.Email;
			parameters[11].Value = model.RealName;
			parameters[12].Value = model.Message;
            parameters[13].Value = model.ShopGiftId;
            parameters[14].Value = model.Express;
            parameters[15].Value = model.Expressnum;

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
		public void Update(BCW.Shop.Model.Shopgoods model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Shopgoods set ");
			strSql.Append("Title=@Title,");
			strSql.Append("GiftId=@GiftId,");
			strSql.Append("PrevPic=@PrevPic,");
			strSql.Append("Num=@Num,");
			strSql.Append("BuyTime=@BuyTime,");
			strSql.Append("SendTime=@SendTime,");
			strSql.Append("ReceiveTime=@ReceiveTime,");
			strSql.Append("UsID=@UsID,");
			strSql.Append("Address=@Address,");
			strSql.Append("Phone=@Phone,");
			strSql.Append("Email=@Email,");
			strSql.Append("RealName=@RealName,");
			strSql.Append("Message=@Message,");
            strSql.Append("ShopGiftId=@ShopGiftId,");
            strSql.Append("Express=@Express,");
            strSql.Append("Expressnum=@Expressnum");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@GiftId", SqlDbType.Int,4),
					new SqlParameter("@PrevPic", SqlDbType.NVarChar,100),
					new SqlParameter("@Num", SqlDbType.Int,4),
					new SqlParameter("@BuyTime", SqlDbType.DateTime),
					new SqlParameter("@SendTime", SqlDbType.DateTime),
					new SqlParameter("@ReceiveTime", SqlDbType.DateTime),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,100),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
					new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Message", SqlDbType.NVarChar),
                                     new SqlParameter("@ShopGiftId", SqlDbType.Int,4),
                                               	new SqlParameter("@Express", SqlDbType.NVarChar,50),
					new SqlParameter("@Expressnum", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.GiftId;
			parameters[3].Value = model.PrevPic;
			parameters[4].Value = model.Num;
			parameters[5].Value = model.BuyTime;
			parameters[6].Value = model.SendTime;
			parameters[7].Value = model.ReceiveTime;
			parameters[8].Value = model.UsID;
			parameters[9].Value = model.Address;
			parameters[10].Value = model.Phone;
			parameters[11].Value = model.Email;
			parameters[12].Value = model.RealName;
			parameters[13].Value = model.Message;
            parameters[14].Value = model.ShopGiftId;
            parameters[15].Value = model.Express;
            parameters[16].Value = model.Expressnum;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsgd(int ShopGiftId, int UsID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_Shopgoods");
            strSql.Append(" where ShopGiftId=@ShopGiftId ");
            strSql.Append(" and UsID=@UsID ");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopGiftId", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4)};
            parameters[0].Value = ShopGiftId;
            parameters[1].Value = UsID;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }
        public void UpdateMessagebyid(int ID, string RealName,string Address, string Phone, string Email, string Message)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopgoods set ");
            strSql.Append("RealName=@RealName ,Address=@Address,Phone=@Phone,Email=@Email,Message=@Message");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@RealName", SqlDbType.NVarChar,50),
					new SqlParameter("@Address", SqlDbType.NVarChar,100),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@Email", SqlDbType.NVarChar,50),
                    new SqlParameter("@Message", SqlDbType.NVarChar)};
            parameters[0].Value = ID;
            parameters[1].Value = RealName;
            parameters[2].Value = Address;
            parameters[3].Value = Phone;
            parameters[4].Value = Email;
            parameters[5].Value = Message;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateMessagebyID(int ID, string Express, string Expressnum,DateTime SendTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopgoods set ");
            strSql.Append("Express=@Express ,Expressnum=@Expressnum,SendTime=@SendTime");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
                    new SqlParameter("@Express", SqlDbType.NVarChar,50),
					new SqlParameter("@Expressnum", SqlDbType.NVarChar,50),
                                        new SqlParameter("@SendTime",SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = Express;
            parameters[2].Value = Expressnum;
            parameters[3].Value = SendTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateReceivebyID(int ID, DateTime ReceiveTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_Shopgoods set ");
            strSql.Append("ReceiveTime=@ReceiveTime ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                                            new SqlParameter("@ID", SqlDbType.Int,4),
                                        new SqlParameter("@ReceiveTime",SqlDbType.DateTime)};
            parameters[0].Value = ID;
            parameters[1].Value = ReceiveTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Shopgoods ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Shop.Model.Shopgoods GetShopgoods(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select  top 1 ID,Title,GiftId,PrevPic,Num,BuyTime,SendTime,ReceiveTime,UsID,Address,Phone,Email,RealName,Message,ShopGiftId,Express,Expressnum from tb_Shopgoods ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Shop.Model.Shopgoods model=new BCW.Shop.Model.Shopgoods();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Title = reader.GetString(1);
					model.GiftId = reader.GetInt32(2);
					model.PrevPic = reader.GetString(3);
					model.Num = reader.GetInt32(4);
					model.BuyTime = reader.GetDateTime(5);
					model.SendTime = reader.GetDateTime(6);
					model.ReceiveTime = reader.GetDateTime(7);
					model.UsID = reader.GetInt32(8);
					model.Address = reader.GetString(9);
					model.Phone = reader.GetString(10);
					model.Email = reader.GetString(11);
					model.RealName = reader.GetString(12);
					model.Message = reader.GetString(13);
                    model.ShopGiftId = reader.GetInt32(14);
                    model.Express = reader.GetString(15);
                    model.Expressnum = reader.GetString(16);
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
        public BCW.Shop.Model.Shopgoods GetShopgoods1(int ShopGiftId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 * from tb_Shopgoods ");
            strSql.Append(" where ShopGiftId=@ShopGiftId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ShopGiftId", SqlDbType.Int,4)};
            parameters[0].Value = ShopGiftId;

            BCW.Shop.Model.Shopgoods model = new BCW.Shop.Model.Shopgoods();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                   
                    model.ID = reader.GetInt32(0);
                    model.Title = reader.GetString(1);
                    model.GiftId = reader.GetInt32(2);
                    model.PrevPic = reader.GetString(3);
                    model.Num = reader.GetInt32(4);
                    model.BuyTime = reader.GetDateTime(5);
                    model.SendTime = reader.GetDateTime(6);
                    model.ReceiveTime = reader.GetDateTime(7);
                    model.UsID = reader.GetInt32(8);
                    model.Address = reader.GetString(9);
                    model.Phone = reader.GetString(10);
                    model.Email = reader.GetString(11);
                    model.RealName = reader.GetString(12);
                    model.Message = reader.GetString(13);
                    model.ShopGiftId = reader.GetInt32(14); 
                 //   model.ShopGiftId = reader.GetInt32(15);
                    model.Express = reader.GetString(15);
                    model.Expressnum = reader.GetString(16);
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
			strSql.Append(" FROM tb_Shopgoods ");
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
		/// <returns>IList Shopgoods</returns>
		public IList<BCW.Shop.Model.Shopgoods> GetShopgoodss(int p_pageIndex, int p_pageSize, string strWhere,string strOrder, out int p_recordCount)
		{
			IList<BCW.Shop.Model.Shopgoods> listShopgoodss = new List<BCW.Shop.Model.Shopgoods>();
			string sTable = "tb_Shopgoods";
			string sPkey = "id";
            string sField = "ID,Title,GiftId,PrevPic,Num,BuyTime,SendTime,ReceiveTime,UsID,Address,Phone,Email,RealName,Message,ShopGiftId,Express,Expressnum";
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
					return listShopgoodss;
				}
				while (reader.Read())
				{
						BCW.Shop.Model.Shopgoods objShopgoods = new BCW.Shop.Model.Shopgoods();
						objShopgoods.ID = reader.GetInt32(0);
						objShopgoods.Title = reader.GetString(1);
						objShopgoods.GiftId = reader.GetInt32(2);
						objShopgoods.PrevPic = reader.GetString(3);
						objShopgoods.Num = reader.GetInt32(4);
						objShopgoods.BuyTime = reader.GetDateTime(5);
						objShopgoods.SendTime = reader.GetDateTime(6);
						objShopgoods.ReceiveTime = reader.GetDateTime(7);
						objShopgoods.UsID = reader.GetInt32(8);
						objShopgoods.Address = reader.GetString(9);
						objShopgoods.Phone = reader.GetString(10);
						objShopgoods.Email = reader.GetString(11);
						objShopgoods.RealName = reader.GetString(12);
						objShopgoods.Message = reader.GetString(13);
                        objShopgoods.ShopGiftId = reader.GetInt32(14);
                        objShopgoods.Express = reader.GetString(15);
                        objShopgoods.Expressnum = reader.GetString(16);
						listShopgoodss.Add(objShopgoods);
				}
			}
			return listShopgoodss;
		}

		#endregion  成员方法
	}
}

