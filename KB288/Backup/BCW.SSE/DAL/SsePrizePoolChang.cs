using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;//Please add references
using System.Collections.Generic;
using BCW.Common;

namespace BCW.SSE.DAL
{
	/// <summary>
	/// 数据访问类:SsePrizePoolChang
	/// </summary>
	public partial class SsePrizePoolChang
	{
		public SsePrizePoolChang()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.SSE.Model.SsePrizePoolChang model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SsePrizePoolChang(");
			strSql.Append("poolType,orderId,operType,OperId,changeTime,changeMoney,totalMoney,bz,sseNo)");
			strSql.Append(" values (");
			strSql.Append("@poolType,@orderId,@operType,@OperId,@changeTime,@changeMoney,@totalMoney,@bz,@sseNo)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@poolType", SqlDbType.TinyInt,1),
					new SqlParameter("@orderId", SqlDbType.Int,4),
					new SqlParameter("@operType", SqlDbType.TinyInt,1),
					new SqlParameter("@OperId", SqlDbType.Int,4),
					new SqlParameter("@changeTime", SqlDbType.DateTime),
					new SqlParameter("@changeMoney", SqlDbType.Money,8),
					new SqlParameter("@totalMoney", SqlDbType.Money,8),
					new SqlParameter("@bz", SqlDbType.NVarChar,500),
					new SqlParameter("@sseNo", SqlDbType.Int,4)};
			parameters[0].Value = model.poolType;
			parameters[1].Value = model.orderId;
			parameters[2].Value = model.operType;
			parameters[3].Value = model.OperId;
			parameters[4].Value = model.changeTime;
			parameters[5].Value = model.changeMoney;
			parameters[6].Value = model.totalMoney;
			parameters[7].Value = model.bz;
			parameters[8].Value = model.sseNo;

			object obj = SqlHelper.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(BCW.SSE.Model.SsePrizePoolChang model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SsePrizePoolChang set ");
			strSql.Append("poolType=@poolType,");
			strSql.Append("orderId=@orderId,");
			strSql.Append("operType=@operType,");
			strSql.Append("OperId=@OperId,");
			strSql.Append("changeTime=@changeTime,");
			strSql.Append("changeMoney=@changeMoney,");
			strSql.Append("totalMoney=@totalMoney,");
			strSql.Append("bz=@bz,");
			strSql.Append("sseNo=@sseNo");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@poolType", SqlDbType.TinyInt,1),
					new SqlParameter("@orderId", SqlDbType.Int,4),
					new SqlParameter("@operType", SqlDbType.TinyInt,1),
					new SqlParameter("@OperId", SqlDbType.Int,4),
					new SqlParameter("@changeTime", SqlDbType.DateTime),
					new SqlParameter("@changeMoney", SqlDbType.Money,8),
					new SqlParameter("@totalMoney", SqlDbType.Money,8),
					new SqlParameter("@bz", SqlDbType.NVarChar,500),
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.poolType;
			parameters[1].Value = model.orderId;
			parameters[2].Value = model.operType;
			parameters[3].Value = model.OperId;
			parameters[4].Value = model.changeTime;
			parameters[5].Value = model.changeMoney;
			parameters[6].Value = model.totalMoney;
			parameters[7].Value = model.bz;
			parameters[8].Value = model.sseNo;
			parameters[9].Value = model.id;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SsePrizePoolChang ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			int rows=SqlHelper.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from tb_SsePrizePoolChang ");
			strSql.Append(" where id in ("+idlist + ")  ");
			int rows=SqlHelper.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.SSE.Model.SsePrizePoolChang GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,poolType,orderId,operType,OperId,changeTime,changeMoney,totalMoney,bz,sseNo from tb_SsePrizePoolChang ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.SSE.Model.SsePrizePoolChang model=new BCW.SSE.Model.SsePrizePoolChang();
			DataSet ds=SqlHelper.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.SSE.Model.SsePrizePoolChang DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SsePrizePoolChang model=new BCW.SSE.Model.SsePrizePoolChang();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["poolType"]!=null && row["poolType"].ToString()!="")
				{
					model.poolType=int.Parse(row["poolType"].ToString());
				}
				if(row["orderId"]!=null && row["orderId"].ToString()!="")
				{
					model.orderId=int.Parse(row["orderId"].ToString());
				}
				if(row["operType"]!=null && row["operType"].ToString()!="")
				{
					model.operType=int.Parse(row["operType"].ToString());
				}
				if(row["OperId"]!=null && row["OperId"].ToString()!="")
				{
					model.OperId=int.Parse(row["OperId"].ToString());
				}
				if(row["changeTime"]!=null && row["changeTime"].ToString()!="")
				{
					model.changeTime=DateTime.Parse(row["changeTime"].ToString());
				}
				if(row["changeMoney"]!=null && row["changeMoney"].ToString()!="")
				{
					model.changeMoney=decimal.Parse(row["changeMoney"].ToString());
				}
				if(row["totalMoney"]!=null && row["totalMoney"].ToString()!="")
				{
					model.totalMoney=decimal.Parse(row["totalMoney"].ToString());
				}
				if(row["bz"]!=null)
				{
					model.bz=row["bz"].ToString();
				}
				if(row["sseNo"]!=null && row["sseNo"].ToString()!="")
				{
					model.sseNo=int.Parse(row["sseNo"].ToString());
				}
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,poolType,orderId,operType,OperId,changeTime,changeMoney,totalMoney,bz,sseNo ");
			strSql.Append(" FROM tb_SsePrizePoolChang ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" id,poolType,orderId,operType,OperId,changeTime,changeMoney,totalMoney,bz,sseNo ");
			strSql.Append(" FROM tb_SsePrizePoolChang ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return SqlHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_SsePrizePoolChang ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = BCW.Data.SqlHelper.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.id desc");
			}
			strSql.Append(")AS Row, T.*  from tb_SsePrizePoolChang T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return SqlHelper.Query(strSql.ToString());
		}

        public IList<BCW.SSE.Model.SsePrizePoolChang> GetSsePrizePoolChangePages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.SsePrizePoolChang> listSseOrder = new List<BCW.SSE.Model.SsePrizePoolChang>();
            string sTable = "tb_SsePrizePoolChang";
            string sPkey = "id";
            string sField = "id,poolType,orderId,operType,OperId,changeTime,changeMoney,totalMoney,bz,sseNo";
            string sCondition = strWhere;
            string sOrder = " id , sseNo ";
            int iSCounts = 0;
            using( SqlDataReader reader = SqlHelper.RunProcedureMe( sTable, sPkey, sField, p_pageIndex, p_pageSize, sCondition, sOrder, iSCounts, out p_recordCount ) )
            {
                //计算总页数
                if( p_recordCount > 0 )
                {
                    int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
                }
                else
                {
                    return listSseOrder;
                }
                while( reader.Read() )
                {
                    BCW.SSE.Model.SsePrizePoolChang objSsePrizePoolChang = new BCW.SSE.Model.SsePrizePoolChang();
                    objSsePrizePoolChang.id = reader.GetInt32( 0 );
                    objSsePrizePoolChang.poolType = reader.GetByte( 1 );
                    objSsePrizePoolChang.orderId = reader.GetInt32( 2 );
                    objSsePrizePoolChang.operType = reader.GetByte( 3 );
                    objSsePrizePoolChang.OperId = reader.GetInt32( 4 );
                    objSsePrizePoolChang.changeTime = reader.GetDateTime( 5 );
                    objSsePrizePoolChang.changeMoney = reader.GetDecimal( 6 );
                    objSsePrizePoolChang.totalMoney = reader.GetDecimal( 7 );
                    objSsePrizePoolChang.bz = reader.IsDBNull( 8 ) ? " " : reader.GetString( 8 );
                    objSsePrizePoolChang.sseNo = reader.GetInt32( 9 );  

                    listSseOrder.Add( objSsePrizePoolChang );

                }
            }
            return listSseOrder;
        }

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "tb_SsePrizePoolChang";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return SqlHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

