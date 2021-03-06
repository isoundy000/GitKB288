using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.dzpk.DAL
{
	/// <summary>
	/// 数据访问类DzpkRooms。
	/// </summary>
	public class DzpkRooms
	{
		public DzpkRooms()
		{}
		#region  成员方法
        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return SqlHelper.GetMaxID("ID", "tb_DzpkRooms");
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_DzpkRooms");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}

        #region 根据游戏标识获得房间名 GetRoom
        /// <summary>
        /// 根据游戏标识获得房间名
        /// </summary>
        /// <param name="r">游戏标识</param>
        /// <returns></returns>
        public BCW.dzpk.Model.DzpkRooms GetRoom(string r)
        {
            BCW.dzpk.Model.DzpkRooms Room = null;
            DataSet ds = new BCW.dzpk.BLL.DzpkHistory().GetList("top 1 *", "RmMake='" + r + "'");
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Room = new BCW.dzpk.BLL.DzpkRooms().GetDzpkRooms(int.Parse(ds.Tables[0].Rows[0]["RmID"].ToString()));                    
                }
            return Room;
        }
        #endregion

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.dzpk.Model.DzpkRooms model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_DzpkRooms(");
			strSql.Append("DRName,DRType,Owers,PassWD,GSmallb,GBigb,GMinb,GMaxb,GSerCharge,GSerChargeALL,GMaxUser,GSidePot,GSetTime,GActID,GActBetID,LastTime,LastRank)");
			strSql.Append(" values (");
			strSql.Append("@DRName,@DRType,@Owers,@PassWD,@GSmallb,@GBigb,@GMinb,@GMaxb,@GSerCharge,@GSerChargeALL,@GMaxUser,@GSidePot,@GSetTime,@GActID,@GActBetID,@LastTime,@LastRank)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@DRName", SqlDbType.NVarChar,50),
					new SqlParameter("@DRType", SqlDbType.Int,4),
					new SqlParameter("@Owers", SqlDbType.Int,4),
					new SqlParameter("@PassWD", SqlDbType.NVarChar,4),
					new SqlParameter("@GSmallb", SqlDbType.BigInt,8),
					new SqlParameter("@GBigb", SqlDbType.BigInt,8),
					new SqlParameter("@GMinb", SqlDbType.BigInt,8),
					new SqlParameter("@GMaxb", SqlDbType.BigInt,8),
					new SqlParameter("@GSerCharge", SqlDbType.BigInt,8),
					new SqlParameter("@GSerChargeALL", SqlDbType.BigInt,8),
					new SqlParameter("@GMaxUser", SqlDbType.Int,4),
					new SqlParameter("@GSidePot", SqlDbType.BigInt,8),
					new SqlParameter("@GSetTime", SqlDbType.Int,4),
					new SqlParameter("@GActID", SqlDbType.Int,4),
					new SqlParameter("@GActBetID", SqlDbType.NChar,1),
					new SqlParameter("@LastTime", SqlDbType.DateTime),
					new SqlParameter("@LastRank", SqlDbType.BigInt,8)};
			parameters[0].Value = model.DRName;
			parameters[1].Value = model.DRType;
			parameters[2].Value = model.Owers;
			parameters[3].Value = model.PassWD;
			parameters[4].Value = model.GSmallb;
			parameters[5].Value = model.GBigb;
			parameters[6].Value = model.GMinb;
			parameters[7].Value = model.GMaxb;
			parameters[8].Value = model.GSerCharge;
			parameters[9].Value = model.GSerChargeALL;
			parameters[10].Value = model.GMaxUser;
			parameters[11].Value = model.GSidePot;
			parameters[12].Value = model.GSetTime;
			parameters[13].Value = model.GActID;
			parameters[14].Value = model.GActBetID;
			parameters[15].Value = model.LastTime;
			parameters[16].Value = model.LastRank;

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
		public void Update(BCW.dzpk.Model.DzpkRooms model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_DzpkRooms set ");
			strSql.Append("DRName=@DRName,");
			strSql.Append("DRType=@DRType,");
			strSql.Append("Owers=@Owers,");
			strSql.Append("PassWD=@PassWD,");
			strSql.Append("GSmallb=@GSmallb,");
			strSql.Append("GBigb=@GBigb,");
			strSql.Append("GMinb=@GMinb,");
			strSql.Append("GMaxb=@GMaxb,");
			strSql.Append("GSerCharge=@GSerCharge,");
			strSql.Append("GSerChargeALL=@GSerChargeALL,");
			strSql.Append("GMaxUser=@GMaxUser,");
			strSql.Append("GSidePot=@GSidePot,");
			strSql.Append("GSetTime=@GSetTime,");
			strSql.Append("GActID=@GActID,");
			strSql.Append("GActBetID=@GActBetID,");
			strSql.Append("LastTime=@LastTime,");
			strSql.Append("LastRank=@LastRank");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@DRName", SqlDbType.NVarChar,50),
					new SqlParameter("@DRType", SqlDbType.Int,4),
					new SqlParameter("@Owers", SqlDbType.Int,4),
					new SqlParameter("@PassWD", SqlDbType.NVarChar,4),
					new SqlParameter("@GSmallb", SqlDbType.BigInt,8),
					new SqlParameter("@GBigb", SqlDbType.BigInt,8),
					new SqlParameter("@GMinb", SqlDbType.BigInt,8),
					new SqlParameter("@GMaxb", SqlDbType.BigInt,8),
					new SqlParameter("@GSerCharge", SqlDbType.BigInt,8),
					new SqlParameter("@GSerChargeALL", SqlDbType.BigInt,8),
					new SqlParameter("@GMaxUser", SqlDbType.Int,4),
					new SqlParameter("@GSidePot", SqlDbType.BigInt,8),
					new SqlParameter("@GSetTime", SqlDbType.Int,4),
					new SqlParameter("@GActID", SqlDbType.Int,4),
					new SqlParameter("@GActBetID", SqlDbType.NChar,1),
					new SqlParameter("@LastTime", SqlDbType.DateTime),
					new SqlParameter("@LastRank", SqlDbType.BigInt,8)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.DRName;
			parameters[2].Value = model.DRType;
			parameters[3].Value = model.Owers;
			parameters[4].Value = model.PassWD;
			parameters[5].Value = model.GSmallb;
			parameters[6].Value = model.GBigb;
			parameters[7].Value = model.GMinb;
			parameters[8].Value = model.GMaxb;
			parameters[9].Value = model.GSerCharge;
			parameters[10].Value = model.GSerChargeALL;
			parameters[11].Value = model.GMaxUser;
			parameters[12].Value = model.GSidePot;
			parameters[13].Value = model.GSetTime;
			parameters[14].Value = model.GActID;
			parameters[15].Value = model.GActBetID;
			parameters[16].Value = model.LastTime;
			parameters[17].Value = model.LastRank;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_DzpkRooms ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.dzpk.Model.DzpkRooms GetDzpkRooms(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,DRName,DRType,Owers,PassWD,GSmallb,GBigb,GMinb,GMaxb,GSerCharge,GSerChargeALL,GMaxUser,GSidePot,GSetTime,GActID,GActBetID,LastTime,LastRank from tb_DzpkRooms ");
			strSql.Append(" where ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = ID;

			BCW.dzpk.Model.DzpkRooms model=new BCW.dzpk.Model.DzpkRooms();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.DRName = reader.GetString(1);
					model.DRType = reader.GetInt32(2);
					model.Owers = reader.GetInt32(3);
					model.PassWD = reader.GetString(4);
					model.GSmallb = reader.GetInt64(5);
					model.GBigb = reader.GetInt64(6);
					model.GMinb = reader.GetInt64(7);
					model.GMaxb = reader.GetInt64(8);
					model.GSerCharge = reader.GetInt64(9);
					model.GSerChargeALL = reader.GetInt64(10);
					model.GMaxUser = reader.GetInt32(11);
					model.GSidePot = reader.GetInt64(12);
					model.GSetTime = reader.GetInt32(13);
					model.GActID = reader.GetInt32(14);
					model.GActBetID = reader.GetString(15);
					model.LastTime = reader.GetDateTime(16);
					model.LastRank = reader.GetInt64(17);
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
			strSql.Append(" FROM tb_DzpkRooms ");
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
		/// <returns>IList DzpkRooms</returns>
		public IList<BCW.dzpk.Model.DzpkRooms> GetDzpkRoomss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.dzpk.Model.DzpkRooms> listDzpkRoomss = new List<BCW.dzpk.Model.DzpkRooms>();
			string sTable = "tb_DzpkRooms";
			string sPkey = "id";
			string sField = "ID,DRName,DRType,Owers,PassWD,GSmallb,GBigb,GMinb,GMaxb,GSerCharge,GSerChargeALL,GMaxUser,GSidePot,GSetTime,GActID,GActBetID,LastTime,LastRank";
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
					return listDzpkRoomss;
				}
				while (reader.Read())
				{
						BCW.dzpk.Model.DzpkRooms objDzpkRooms = new BCW.dzpk.Model.DzpkRooms();
						objDzpkRooms.ID = reader.GetInt32(0);
						objDzpkRooms.DRName = reader.GetString(1);
						objDzpkRooms.DRType = reader.GetInt32(2);
						objDzpkRooms.Owers = reader.GetInt32(3);
						objDzpkRooms.PassWD = reader.GetString(4);
						objDzpkRooms.GSmallb = reader.GetInt64(5);
						objDzpkRooms.GBigb = reader.GetInt64(6);
						objDzpkRooms.GMinb = reader.GetInt64(7);
						objDzpkRooms.GMaxb = reader.GetInt64(8);
						objDzpkRooms.GSerCharge = reader.GetInt64(9);
						objDzpkRooms.GSerChargeALL = reader.GetInt64(10);
						objDzpkRooms.GMaxUser = reader.GetInt32(11);
						objDzpkRooms.GSidePot = reader.GetInt64(12);
						objDzpkRooms.GSetTime = reader.GetInt32(13);
						objDzpkRooms.GActID = reader.GetInt32(14);
						objDzpkRooms.GActBetID = reader.GetString(15);
						objDzpkRooms.LastTime = reader.GetDateTime(16);
						objDzpkRooms.LastRank = reader.GetInt64(17);
						listDzpkRoomss.Add(objDzpkRooms);
				}
			}
			return listDzpkRoomss;
		}

		#endregion  成员方法
	}
}

