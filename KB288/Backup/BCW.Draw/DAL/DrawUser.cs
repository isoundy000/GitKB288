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
	/// 数据访问类DrawUser。
	/// </summary>
	public class DrawUser
	{
		public DrawUser()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("ID", "tb_DrawUser"); 
		}
        //-------------------------------------------------------//
        /// <summary>
        /// 得到用
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTime(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OnTime from tb_DrawUser ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.OnTime = reader.GetDateTime(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public BCW.Draw.Model.DrawUser GetInTime(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select InTime from tb_DrawUser ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.InTime = reader.GetDateTime(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到编号取奖品类型
        /// </summary>
        public int GetMyGoodsType(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsType from tb_DrawUser ");
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

        public int GetMyStatue(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsStatue from tb_DrawUser ");
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
        /// 得到编号取奖品类型
        /// </summary>
        public DateTime  Getontime(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OnTime from tb_DrawUser ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }
        /// <summary>
        /// 得到编号取奖品价值
        /// </summary>
        public int GetMyGoodsValue(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsValue from tb_DrawUser ");
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
        /// 得到
        /// </summary>
        public string GetMyGoods(int GoodsCounts)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoods from tb_DrawUser ");
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
                    return null;
                }
            }
        }





        /// <summary>
        /// 得到用
        /// </summary>
        public BCW.Draw.Model.DrawUser GetOnTimebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OnTime from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.OnTime = reader.GetDateTime(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        public BCW.Draw.Model.DrawUser GetInTimebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select InTime from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.InTime = reader.GetDateTime(0);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 得到编号取奖品类型
        /// </summary>
        public int GetMyGoodsTypebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsType from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

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

        public int GetMyStatuebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsStatue from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

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
        /// 得到编号取奖品类型
        /// </summary>
        public DateTime Getontimebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select OnTime from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetDateTime(0);
                }
                else
                {
                    return DateTime.Now;
                }
            }
        }
        /// <summary>
        /// 得到编号取奖品价值
        /// </summary>
        public int GetMyGoodsValuebynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsValue from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

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
        /// 得到编号取奖品数量
        /// </summary>
        public int GetMyGoodsNumbynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoodsNum from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

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
        /// 得到
        /// </summary>
        public string GetMyGoodsbynum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MyGoods from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            using (SqlDataReader reader = SqlHelperUser.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return reader.GetString(0);
                }
                else
                {
                    return null;
                }
            }
        }





        //==根据编号更新地址========//

        public void UpdateMessage(int GoodsCounts, string Address, string Phone, string Email,string RealName ,int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("Address=@Address,Phone=@Phone,Email=@Email,RealName=@RealName  ,MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,50),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@Email", SqlDbType.NVarChar,50),
                     new SqlParameter("@RealName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = Address;
            parameters[2].Value = Phone;
            parameters[3].Value = Email;
            parameters[4].Value = RealName;
            parameters[5].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateExpress(int GoodsCounts, string Express,string  Numbers, int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("Express=@Express,Numbers=@Numbers ,MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@Express", SqlDbType.NVarChar,50),
                    new SqlParameter("@Numbers", SqlDbType.NVarChar,50),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = Express;
            parameters[2].Value = Numbers;
            parameters[3].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateIntime(int GoodsCounts,DateTime InTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("InTime=@InTime");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@InTime", SqlDbType.DateTime)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = InTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateMyGoodsStatue(int GoodsCounts, int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where GoodsCounts=@GoodsCounts");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;
            parameters[1].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }




        public void UpdateMessagebynum(int Num, string Address, string Phone, string Email, string RealName, int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("Address=@Address,Phone=@Phone,Email=@Email,RealName=@RealName  ,MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where Num=@Num");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4),
					new SqlParameter("@Address", SqlDbType.NVarChar,50),
                    new SqlParameter("@Phone", SqlDbType.NVarChar,50),
                    new SqlParameter("@Email", SqlDbType.NVarChar,50),
                     new SqlParameter("@RealName", SqlDbType.NVarChar,50),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = Num;
            parameters[1].Value = Address;
            parameters[2].Value = Phone;
            parameters[3].Value = Email;
            parameters[4].Value = RealName;
            parameters[5].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateExpressbynum(int Num, string Express, string Numbers, int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("Express=@Express,Numbers=@Numbers ,MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where Num=@Num");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4),
					new SqlParameter("@Express", SqlDbType.NVarChar,50),
                    new SqlParameter("@Numbers", SqlDbType.NVarChar,50),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = Num;
            parameters[1].Value = Express;
            parameters[2].Value = Numbers;
            parameters[3].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateIntimebynum(int Num, DateTime InTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("InTime=@InTime");
            strSql.Append(" where Num=@Num");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4),
                    new SqlParameter("@InTime", SqlDbType.DateTime)};
            parameters[0].Value = Num;
            parameters[1].Value = InTime;

            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }

        public void UpdateMyGoodsStatuebynum(int Num, int MyGoodsStatue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update tb_DrawUser set ");
            strSql.Append("MyGoodsStatue=@MyGoodsStatue");
            strSql.Append(" where Num=@Num");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4),
                    new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4)};
            parameters[0].Value = Num;
            parameters[1].Value = MyGoodsStatue;
            SqlHelper.ExecuteSql(strSql.ToString(), parameters);
        }


        //-------------------------------------------------------//
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DrawUser");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Existsnum(int Num)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from tb_DrawUser");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            return SqlHelper.Exists(strSql.ToString(), parameters);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(BCW.Draw.Model.DrawUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DrawUser(");
			strSql.Append("UsID,UsName,GoodsCounts,MyGoods,Explain,MyGoodsImg,MyGoodsType,MyGoodsValue,MyGoodsStatue,MyGoodsNum,OnTime,InTime,Address,Phone,Email,R,Num,RealName,Express,Numbers)");
			strSql.Append(" values (");
            strSql.Append("@UsID,@UsName,@GoodsCounts,@MyGoods,@Explain,@MyGoodsImg,@MyGoodsType,@MyGoodsValue,@MyGoodsStatue,@MyGoodsNum,@OnTime,@InTime,@Address,@Phone,@Email,@R,@Num,@RealName,@Express,@Numbers)");
			SqlParameter[] parameters = {
					
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@MyGoods", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@MyGoodsImg", SqlDbType.NVarChar),
					new SqlParameter("@MyGoodsType", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsValue", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsNum", SqlDbType.Int,4),
					new SqlParameter("@OnTime", SqlDbType.DateTime),
					new SqlParameter("@InTime", SqlDbType.DateTime),
					new SqlParameter("@Address", SqlDbType.NVarChar,50),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
                                        new SqlParameter("@R", SqlDbType.Int,4),
                                        new SqlParameter("@Num", SqlDbType.Int,4),
                                        new SqlParameter("@RealName", SqlDbType.NVarChar,50),
                                        new SqlParameter("@Express", SqlDbType.NVarChar,50),
                                        new SqlParameter("@Numbers", SqlDbType.NVarChar,50)};
			
			parameters[0].Value = model.UsID;
			parameters[1].Value = model.UsName;
			parameters[2].Value = model.GoodsCounts;
			parameters[3].Value = model.MyGoods;
			parameters[4].Value = model.Explain;
			parameters[5].Value = model.MyGoodsImg;
			parameters[6].Value = model.MyGoodsType;
			parameters[7].Value = model.MyGoodsValue;
			parameters[8].Value = model.MyGoodsStatue;
			parameters[9].Value = model.MyGoodsNum;
			parameters[10].Value = model.OnTime;
			parameters[11].Value = model.InTime;
			parameters[12].Value = model.Address;
			parameters[13].Value = model.Phone;
			parameters[14].Value = model.Email;
            parameters[15].Value = model.R;
            parameters[16].Value = model.Num;
            parameters[17].Value = model.RealName;
            parameters[18].Value = model.Express;
            parameters[19].Value = model.Numbers;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Draw.Model.DrawUser model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DrawUser set ");
			strSql.Append("UsID=@UsID,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("GoodsCounts=@GoodsCounts,");
			strSql.Append("MyGoods=@MyGoods,");
			strSql.Append("Explain=@Explain,");
			strSql.Append("MyGoodsImg=@MyGoodsImg,");
			strSql.Append("MyGoodsType=@MyGoodsType,");
			strSql.Append("MyGoodsValue=@MyGoodsValue,");
			strSql.Append("MyGoodsStatue=@MyGoodsStatue,");
			strSql.Append("MyGoodsNum=@MyGoodsNum,");
			strSql.Append("OnTime=@OnTime,");
			strSql.Append("InTime=@InTime,");
			strSql.Append("Address=@Address,");
			strSql.Append("Phone=@Phone,");
			strSql.Append("Email=@Email,");
            strSql.Append("R=@R,");
            strSql.Append("Num=@Num");
            strSql.Append("RealName=@RealName");
            strSql.Append("Express=@Express");
            strSql.Append("Numbers=@Numbers");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4),
					new SqlParameter("@MyGoods", SqlDbType.NVarChar,50),
					new SqlParameter("@Explain", SqlDbType.NVarChar),
					new SqlParameter("@MyGoodsImg", SqlDbType.NVarChar),
					new SqlParameter("@MyGoodsType", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsValue", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsStatue", SqlDbType.Int,4),
					new SqlParameter("@MyGoodsNum", SqlDbType.Int,4),
					new SqlParameter("@OnTime", SqlDbType.DateTime),
					new SqlParameter("@InTime", SqlDbType.DateTime),
					new SqlParameter("@Address", SqlDbType.NVarChar,50),
					new SqlParameter("@Phone", SqlDbType.NVarChar,50),
					new SqlParameter("@Email", SqlDbType.NVarChar,50),
                                        new SqlParameter("@R", SqlDbType.Int,4),
                                        new SqlParameter("@Num", SqlDbType.Int,4),
                                        new SqlParameter("@RealName", SqlDbType.NVarChar,50),
                                        new SqlParameter("@Express", SqlDbType.NVarChar,50),
                                        new SqlParameter("@Numbers", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.UsID;
			parameters[2].Value = model.UsName;
			parameters[3].Value = model.GoodsCounts;
			parameters[4].Value = model.MyGoods;
			parameters[5].Value = model.Explain;
			parameters[6].Value = model.MyGoodsImg;
			parameters[7].Value = model.MyGoodsType;
			parameters[8].Value = model.MyGoodsValue;
			parameters[9].Value = model.MyGoodsStatue;
			parameters[10].Value = model.MyGoodsNum;
			parameters[11].Value = model.OnTime;
			parameters[12].Value = model.InTime;
			parameters[13].Value = model.Address;
			parameters[14].Value = model.Phone;
			parameters[15].Value = model.Email;
            parameters[16].Value = model.R;
            parameters[17].Value = model.Num;
            parameters[18].Value = model.RealName;
            parameters[19].Value = model.Express;
            parameters[20].Value = model.Numbers;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DrawUser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}
        // me_初始化某数据表
        public void ClearTable(string TableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" TRUNCATE table ");
            string sd_where = TableName;
            strSql.Append(sd_where);
            SqlHelper.ExecuteSql(strSql.ToString());
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Draw.Model.DrawUser GetDrawUser(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,UsID,UsName,GoodsCounts,MyGoods,Explain,MyGoodsImg,MyGoodsType,MyGoodsValue,MyGoodsStatue,MyGoodsNum,OnTime,InTime,Address,Phone,Email,R,Num,RealName,Express,Numbers from tb_DrawUser ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.Draw.Model.DrawUser model=new BCW.Draw.Model.DrawUser();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.UsID = reader.GetInt32(1);
					model.UsName = reader.GetString(2);
					model.GoodsCounts = reader.GetInt32(3);
					model.MyGoods = reader.GetString(4);
					model.Explain = reader.GetString(5);
					model.MyGoodsImg = reader.GetString(6);
					model.MyGoodsType = reader.GetInt32(7);
					model.MyGoodsValue = reader.GetInt32(8);
					model.MyGoodsStatue = reader.GetInt32(9);
					model.MyGoodsNum = reader.GetInt32(10);
					model.OnTime = reader.GetDateTime(11);
					model.InTime = reader.GetDateTime(12);
					model.Address = reader.GetString(13);
					model.Phone = reader.GetString(14);
					model.Email = reader.GetString(15);
                    model.R = reader.GetInt32(16);
                    model.Num = reader.GetInt32(17);
                    model.RealName = reader.GetString(18);
                    model.Express = reader.GetString(19);
                    model.Numbers = reader.GetString(20);
					return model;
				}
				else
				{
				return null;
				}
			}
		}

        //////////////////////////////

    

        //------------------------------------------根据编号取数据
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Draw.Model.DrawUser GetDrawUserbyCounts(int GoodsCounts)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,GoodsCounts,MyGoods,Explain,MyGoodsImg,MyGoodsType,MyGoodsValue,MyGoodsStatue,MyGoodsNum,OnTime,InTime,Address,Phone,Email,R,Num,RealName,Express,Numbers from tb_DrawUser ");
            strSql.Append(" where GoodsCounts=@GoodsCounts ");
            SqlParameter[] parameters = {
					new SqlParameter("@GoodsCounts", SqlDbType.Int,4)};
            parameters[0].Value = GoodsCounts;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.GoodsCounts = reader.GetInt32(3);
                    model.MyGoods = reader.GetString(4);
                    model.Explain = reader.GetString(5);
                    model.MyGoodsImg = reader.GetString(6);
                    model.MyGoodsType = reader.GetInt32(7);
                    model.MyGoodsValue = reader.GetInt32(8);
                    model.MyGoodsStatue = reader.GetInt32(9);
                    model.MyGoodsNum = reader.GetInt32(10);
                    model.OnTime = reader.GetDateTime(11);
                    model.InTime = reader.GetDateTime(12);
                    model.Address = reader.GetString(13);
                    model.Phone = reader.GetString(14);
                    model.Email = reader.GetString(15);
                    model.R = reader.GetInt32(16);
                    model.Num = reader.GetInt32(17);
                    model.RealName = reader.GetString(18);
                    model.Express = reader.GetString(19);
                    model.Numbers = reader.GetString(20);
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
        public BCW.Draw.Model.DrawUser GetDrawUserbynum(int Num)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,UsID,UsName,GoodsCounts,MyGoods,Explain,MyGoodsImg,MyGoodsType,MyGoodsValue,MyGoodsStatue,MyGoodsNum,OnTime,InTime,Address,Phone,Email,R,Num,RealName,Express,Numbers from tb_DrawUser ");
            strSql.Append(" where Num=@Num ");
            SqlParameter[] parameters = {
					new SqlParameter("@Num", SqlDbType.Int,4)};
            parameters[0].Value = Num;

            BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString(), parameters))
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    model.ID = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    model.UsName = reader.GetString(2);
                    model.GoodsCounts = reader.GetInt32(3);
                    model.MyGoods = reader.GetString(4);
                    model.Explain = reader.GetString(5);
                    model.MyGoodsImg = reader.GetString(6);
                    model.MyGoodsType = reader.GetInt32(7);
                    model.MyGoodsValue = reader.GetInt32(8);
                    model.MyGoodsStatue = reader.GetInt32(9);
                    model.MyGoodsNum = reader.GetInt32(10);
                    model.OnTime = reader.GetDateTime(11);
                    model.InTime = reader.GetDateTime(12);
                    model.Address = reader.GetString(13);
                    model.Phone = reader.GetString(14);
                    model.Email = reader.GetString(15);
                    model.R = reader.GetInt32(16);
                    model.Num = reader.GetInt32(17);
                    model.RealName = reader.GetString(18);
                    model.Express = reader.GetString(19);
                    model.Numbers = reader.GetString(20);
                    return model;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// me_计算和值出现次数并排序
        /// </summary>
        public IList<BCW.Draw.Model.DrawUser> Get_UsID(string _where)
        {
            IList<BCW.Draw.Model.DrawUser> Get_UsID = new List<BCW.Draw.Model.DrawUser>();
            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(UsID) as aa,[UsID] FROM tb_DrawUser where [MyGoodsStatue] not in (88) ");
            strSql.Append(sd_where + "group by UsID");
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                while (reader.Read())
                {
                    BCW.Draw.Model.DrawUser model = new BCW.Draw.Model.DrawUser();
                    //reader.Read();
                    model.aa = reader.GetInt32(0);
                    model.UsID = reader.GetInt32(1);
                    Get_UsID.Add(model);
                }
            }
            return Get_UsID;
        }


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList HcPay</returns>
        public IList<BCW.Draw.Model.DrawUser> GetUserTop(int p_pageIndex, int p_pageSize,string _where, string strWhere, out int p_recordCount)
        {
            IList<BCW.Draw.Model.DrawUser> listUserTop = new List<BCW.Draw.Model.DrawUser>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT UsID) FROM tb_DrawUser where " + strWhere + "";

            p_recordCount = Convert.ToInt32(SqlHelper.GetSingle(countString));
            if (p_recordCount > 50)
                p_recordCount = 50;

            if (p_recordCount > 0)
            {
                int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
            }
            else
            {
                return listUserTop;
            }

            string sd_where = _where;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(UsID) as aa,[UsID] FROM tb_DrawUser where [MyGoodsStatue] not in (88) ");
            strSql.Append(sd_where + "group by UsID order by aa DESC");
            using (SqlDataReader reader = SqlHelper.ExecuteReader(strSql.ToString()))
            {
                int stratIndex = (p_pageIndex - 1) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while (reader.Read())
                {
                    if (k >= stratIndex && k < endIndex)
                    {
                        BCW.Draw.Model.DrawUser objHcPay = new BCW.Draw.Model.DrawUser();
                        objHcPay.aa = reader.GetInt32(0);
                        objHcPay.UsID = reader.GetInt32(1);
                        //objHcPay.aa = reader.GetInt32(2);
                        listUserTop.Add(objHcPay);
                    }
                    if (k == endIndex)
                        break;

                    k++;
                }
            }

            return listUserTop;
        }


		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  " + strField + " ");
			strSql.Append(" FROM tb_DrawUser ");
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
		/// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
		{
            IList<BCW.Draw.Model.DrawUser> listDrawUsers = new List<BCW.Draw.Model.DrawUser>();
            string sTable = "tb_DrawUser";
            string sPkey = "id";
            string sField = "*";
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
                    return listDrawUsers;
                }
                while (reader.Read())
                {
                    BCW.Draw.Model.DrawUser objDrawUser = new BCW.Draw.Model.DrawUser();
                    objDrawUser.ID = reader.GetInt32(0);
                    objDrawUser.UsID = reader.GetInt32(1);
                    objDrawUser.UsName = reader.GetString(2);
                    objDrawUser.GoodsCounts = reader.GetInt32(3);
                    objDrawUser.MyGoods = reader.GetString(4);
                    objDrawUser.Explain = reader.GetString(5);
                    objDrawUser.MyGoodsImg = reader.GetString(6);
                    objDrawUser.MyGoodsType = reader.GetInt32(7);
                    objDrawUser.MyGoodsValue = reader.GetInt32(8);
                    objDrawUser.MyGoodsStatue = reader.GetInt32(9);
                    objDrawUser.MyGoodsNum = reader.GetInt32(10);
                    objDrawUser.OnTime = reader.GetDateTime(11);
                    objDrawUser.InTime = reader.GetDateTime(12);
                    objDrawUser.Address = reader.GetString(13);
                    objDrawUser.Phone = reader.GetString(14);
                    objDrawUser.Email = reader.GetString(15);
                    objDrawUser.R = reader.GetInt32(16);
                    objDrawUser.Num = reader.GetInt32(17);
                    objDrawUser.RealName = reader.GetString(18);
                    objDrawUser.Express = reader.GetString(19);
                    objDrawUser.Numbers = reader.GetString(20);
                    listDrawUsers.Add(objDrawUser);
                }
            }
            return listDrawUsers;
		}


        /////////////////////////////////////
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList DrawUser</returns>
        public IList<BCW.Draw.Model.DrawUser> GetDrawUsers1(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int iSCounts, out int p_recordCount)
        {
            IList<BCW.Draw.Model.DrawUser> listDrawUsers = new List<BCW.Draw.Model.DrawUser>();
            string sTable = "tb_DrawUser";
            string sPkey = "id";
            string sField = "ID,UsID,UsName,GoodsCounts,MyGoods,Explain,MyGoodsImg,MyGoodsType,MyGoodsValue,MyGoodsStatue,MyGoodsNum,OnTime,InTime,Address,Phone,Email,R,Num,RealName,Express,Numbers";
            string sCondition = strWhere;
            string sOrder = strOrder;
            int iSCounts1 = iSCounts;
            using (SqlDataReader reader = SqlHelper.RunProcedureMe(sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts1, out p_recordCount))
            {
                //计算总页数
                if (p_recordCount > 0)
                {
                    int pageCount = BasePage.CalcPageCount(p_recordCount, p_pageSize, ref p_pageIndex);
                }
                else
                {
                    return listDrawUsers;
                }
                while (reader.Read())
                {
                    BCW.Draw.Model.DrawUser objDrawUser = new BCW.Draw.Model.DrawUser();
                    objDrawUser.ID = reader.GetInt32(0);
                    objDrawUser.UsID = reader.GetInt32(1);
                    objDrawUser.UsName = reader.GetString(2);
                    objDrawUser.GoodsCounts = reader.GetInt32(3);
                    objDrawUser.MyGoods = reader.GetString(4);
                    objDrawUser.Explain = reader.GetString(5);
                    objDrawUser.MyGoodsImg = reader.GetString(6);
                    objDrawUser.MyGoodsType = reader.GetInt32(7);
                    objDrawUser.MyGoodsValue = reader.GetInt32(8);
                    objDrawUser.MyGoodsStatue = reader.GetInt32(9);
                    objDrawUser.MyGoodsNum = reader.GetInt32(10);
                    objDrawUser.OnTime = reader.GetDateTime(11);
                    objDrawUser.InTime = reader.GetDateTime(12);
                    objDrawUser.Address = reader.GetString(13);
                    objDrawUser.Phone = reader.GetString(14);
                    objDrawUser.Email = reader.GetString(15);
                    objDrawUser.R = reader.GetInt32(16);
                    objDrawUser.Num = reader.GetInt32(17);
                    objDrawUser.RealName = reader.GetString(18);
                    objDrawUser.Express = reader.GetString(19);
                    objDrawUser.Numbers = reader.GetString(20);
                    listDrawUsers.Add(objDrawUser);
                }
            }
            return listDrawUsers;
        }

		#endregion  成员方法
	}
}

