using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;
using BCW.Common;
namespace BCW.DAL.Game
{
	/// <summary>
	/// 数据访问类Lottery。
	/// </summary>
	public class Lottery
	{
		public Lottery()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return SqlHelper.GetMaxID("UsID", "tb_Lottery"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UsID,int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from tb_Lottery");
			strSql.Append(" where UsID=@UsID and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = UsID;
			parameters[1].Value = ID;

			return SqlHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.Model.Game.Lottery model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_Lottery(");
			strSql.Append("Types,Title,PayCent,OutCent,OutGift,UsID,UsName,FreshSec,FreshMin,WinCount,AcTime,AddTime,EndTime)");
			strSql.Append(" values (");
			strSql.Append("@Types,@Title,@PayCent,@OutCent,@OutGift,@UsID,@UsName,@FreshSec,@FreshMin,@WinCount,@AcTime,@AddTime,@EndTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@OutCent", SqlDbType.BigInt,8),
					new SqlParameter("@OutGift", SqlDbType.NVarChar,200),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@FreshSec", SqlDbType.Int,4),
					new SqlParameter("@FreshMin", SqlDbType.Int,4),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@AcTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
			parameters[0].Value = model.Types;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.PayCent;
			parameters[3].Value = model.OutCent;
			parameters[4].Value = model.OutGift;
			parameters[5].Value = model.UsID;
			parameters[6].Value = model.UsName;
			parameters[7].Value = model.FreshSec;
			parameters[8].Value = model.FreshMin;
			parameters[9].Value = model.WinCount;
			parameters[10].Value = model.AcTime;
			parameters[11].Value = model.AddTime;
			parameters[12].Value = model.EndTime;

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
		public void Update(BCW.Model.Game.Lottery model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_Lottery set ");
			strSql.Append("Types=@Types,");
			strSql.Append("Title=@Title,");
			strSql.Append("PayCent=@PayCent,");
			strSql.Append("OutCent=@OutCent,");
			strSql.Append("OutGift=@OutGift,");
			strSql.Append("UsName=@UsName,");
			strSql.Append("FreshSec=@FreshSec,");
			strSql.Append("FreshMin=@FreshMin,");
			strSql.Append("WinCount=@WinCount,");
			strSql.Append("AcTime=@AcTime,");
			strSql.Append("AddTime=@AddTime,");
			strSql.Append("EndTime=@EndTime");
			strSql.Append(" where UsID=@UsID and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4),
					new SqlParameter("@Types", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,50),
					new SqlParameter("@PayCent", SqlDbType.BigInt,8),
					new SqlParameter("@OutCent", SqlDbType.BigInt,8),
					new SqlParameter("@OutGift", SqlDbType.NVarChar,200),
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@UsName", SqlDbType.NVarChar,50),
					new SqlParameter("@FreshSec", SqlDbType.Int,4),
					new SqlParameter("@FreshMin", SqlDbType.Int,4),
					new SqlParameter("@WinCount", SqlDbType.Int,4),
					new SqlParameter("@AcTime", SqlDbType.DateTime),
					new SqlParameter("@AddTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime)};
			parameters[0].Value = model.ID;
			parameters[1].Value = model.Types;
			parameters[2].Value = model.Title;
			parameters[3].Value = model.PayCent;
			parameters[4].Value = model.OutCent;
			parameters[5].Value = model.OutGift;
			parameters[6].Value = model.UsID;
			parameters[7].Value = model.UsName;
			parameters[8].Value = model.FreshSec;
			parameters[9].Value = model.FreshMin;
			parameters[10].Value = model.WinCount;
			parameters[11].Value = model.AcTime;
			parameters[12].Value = model.AddTime;
			parameters[13].Value = model.EndTime;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int UsID,int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_Lottery ");
			strSql.Append(" where UsID=@UsID and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = UsID;
			parameters[1].Value = ID;

			SqlHelper.ExecuteSql(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Lottery GetLottery(int UsID,int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Types,Title,PayCent,OutCent,OutGift,UsID,UsName,FreshSec,FreshMin,WinCount,AcTime,AddTime,EndTime from tb_Lottery ");
			strSql.Append(" where UsID=@UsID and ID=@ID ");
			SqlParameter[] parameters = {
					new SqlParameter("@UsID", SqlDbType.Int,4),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = UsID;
			parameters[1].Value = ID;

			BCW.Model.Game.Lottery model=new BCW.Model.Game.Lottery();
			using (SqlDataReader reader=SqlHelper.ExecuteReader(strSql.ToString(),parameters))
			{
				if (reader.HasRows)
				{
					reader.Read();
					model.ID = reader.GetInt32(0);
					model.Types = reader.GetInt32(1);
					model.Title = reader.GetString(2);
					model.PayCent = reader.GetInt64(3);
					model.OutCent = reader.GetInt64(4);
					model.OutGift = reader.GetString(5);
					model.UsID = reader.GetInt32(6);
					model.UsName = reader.GetString(7);
					model.FreshSec = reader.GetInt32(8);
					model.FreshMin = reader.GetInt32(9);
					model.WinCount = reader.GetInt32(10);
					model.AcTime = reader.GetDateTime(11);
					model.AddTime = reader.GetDateTime(12);
					model.EndTime = reader.GetDateTime(13);
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
			strSql.Append(" FROM tb_Lottery ");
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
		/// <returns>IList Lottery</returns>
		public IList<BCW.Model.Game.Lottery> GetLotterys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			IList<BCW.Model.Game.Lottery> listLotterys = new List<BCW.Model.Game.Lottery>();
			string sTable = "tb_Lottery";
			string sPkey = "id";
			string sField = "ID,Types,Title,PayCent,OutCent,OutGift,UsID,UsName,FreshSec,FreshMin,WinCount,AcTime,AddTime,EndTime";
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
					return listLotterys;
				}
				while (reader.Read())
				{
						BCW.Model.Game.Lottery objLottery = new BCW.Model.Game.Lottery();
						objLottery.ID = reader.GetInt32(0);
						objLottery.Types = reader.GetInt32(1);
						objLottery.Title = reader.GetString(2);
						objLottery.PayCent = reader.GetInt64(3);
						objLottery.OutCent = reader.GetInt64(4);
						objLottery.OutGift = reader.GetString(5);
						objLottery.UsID = reader.GetInt32(6);
						objLottery.UsName = reader.GetString(7);
						objLottery.FreshSec = reader.GetInt32(8);
						objLottery.FreshMin = reader.GetInt32(9);
						objLottery.WinCount = reader.GetInt32(10);
						objLottery.AcTime = reader.GetDateTime(11);
						objLottery.AddTime = reader.GetDateTime(12);
						objLottery.EndTime = reader.GetDateTime(13);
						listLotterys.Add(objLottery);
				}
			}
			return listLotterys;
		}

		#endregion  成员方法
	}
}

