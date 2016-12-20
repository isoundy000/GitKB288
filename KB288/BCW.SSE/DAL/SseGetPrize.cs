using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using BCW.Data;//Please add references
using BCW.Common;
using System.Collections.Generic;

namespace BCW.SSE.DAL
{
    
	/// <summary>
	/// 数据访问类:SseGetPrize
	/// </summary>
    /// 
	public partial class SseGetPrize
	{
		public SseGetPrize()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.SSE.Model.SseGetPrize model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SseGetPrize(");
			strSql.Append("orderId,userId,isGet,getDateTime,openDateTime)");
			strSql.Append(" values (");
			strSql.Append("@orderId,@userId,@isGet,@getDateTime,@openDateTime)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@orderId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@isGet", SqlDbType.Bit,1),
					new SqlParameter("@getDateTime", SqlDbType.DateTime),
					new SqlParameter("@openDateTime", SqlDbType.NChar,10)};
			parameters[0].Value = model.orderId;
			parameters[1].Value = model.userId;
			parameters[2].Value = model.isGet;
			parameters[3].Value = model.getDateTime;
			parameters[4].Value = model.openDateTime;

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
		public bool Update(BCW.SSE.Model.SseGetPrize model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SseGetPrize set ");
			strSql.Append("orderId=@orderId,");
			strSql.Append("userId=@userId,");
			strSql.Append("isGet=@isGet,");
			strSql.Append("getDateTime=@getDateTime,");
			strSql.Append("openDateTime=@openDateTime");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@orderId", SqlDbType.Int,4),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@isGet", SqlDbType.Bit,1),
					new SqlParameter("@getDateTime", SqlDbType.DateTime),
					new SqlParameter("@openDateTime", SqlDbType.NChar,10),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.orderId;
			parameters[1].Value = model.userId;
			parameters[2].Value = model.isGet;
			parameters[3].Value = model.getDateTime;
			parameters[4].Value = model.openDateTime;
			parameters[5].Value = model.id;

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
			strSql.Append("delete from tb_SseGetPrize ");
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
			strSql.Append("delete from tb_SseGetPrize ");
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
		public BCW.SSE.Model.SseGetPrize GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,orderId,userId,isGet,getDateTime,openDateTime from tb_SseGetPrize ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.SSE.Model.SseGetPrize model=new BCW.SSE.Model.SseGetPrize();
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
		public BCW.SSE.Model.SseGetPrize DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SseGetPrize model=new BCW.SSE.Model.SseGetPrize();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["orderId"]!=null && row["orderId"].ToString()!="")
				{
					model.orderId=int.Parse(row["orderId"].ToString());
				}
				if(row["userId"]!=null && row["userId"].ToString()!="")
				{
					model.userId=int.Parse(row["userId"].ToString());
				}
				if(row["isGet"]!=null && row["isGet"].ToString()!="")
				{
					if((row["isGet"].ToString()=="1")||(row["isGet"].ToString().ToLower()=="true"))
					{
						model.isGet=true;
					}
					else
					{
						model.isGet=false;
					}
				}
				if(row["getDateTime"]!=null && row["getDateTime"].ToString()!="")
				{
					model.getDateTime=DateTime.Parse(row["getDateTime"].ToString());
				}
				if(row["openDateTime"]!=null)
				{
                    model.openDateTime = DateTime.Parse(row[ "openDateTime" ].ToString());
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
            strSql.Append( "select id,orderId,userId,isGet,getDateTime,openDateTime,prizeVal " );
			strSql.Append(" FROM tb_SseGetPrize ");
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
			strSql.Append(" id,orderId,userId,isGet,getDateTime,openDateTime ");
			strSql.Append(" FROM tb_SseGetPrize ");
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
			strSql.Append("select count(1) FROM tb_SseGetPrize ");
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
			strSql.Append(")AS Row, T.*  from tb_SseGetPrize T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return SqlHelper.Query(strSql.ToString());
		}


        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.SSE.Model.veSseGetPrize> GetSseGetPrizePages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.veSseGetPrize> listSseGetPrize = new List<BCW.SSE.Model.veSseGetPrize>();
            string sTable = "ve_SseGetPrize";
            string sPkey = "id";
            string sField = "id,orderId,isGet,orderType,sseNo,buyType,buyMoney,prizeVal,userId";
            string sCondition = strWhere;
            string sOrder = "id desc";
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
                    return listSseGetPrize;
                }
                while( reader.Read() )
                {
                    BCW.SSE.Model.veSseGetPrize objSseGetPrize = new BCW.SSE.Model.veSseGetPrize();
                    objSseGetPrize.id = reader.GetInt32( 0 );
                    objSseGetPrize.orderId = reader.GetInt32( 1 );
                    objSseGetPrize.isGet = reader.GetBoolean( 2 );
                    //objSseGetPrize.orderType = reader.GetInt32( 3 );
                    objSseGetPrize.sseNo = reader.GetInt32( 4 );
                    objSseGetPrize.buyType = reader.GetBoolean( 5 );
                    objSseGetPrize.buyMoney = reader.GetDecimal( 6 );
                    objSseGetPrize.prizeVal = reader.GetDecimal( 7 );

                    listSseGetPrize.Add( objSseGetPrize );

                }
            }
            return listSseGetPrize;
        }


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.SSE.Model.SseGetPrizeCharts> GetSseGetPrizeChartsPages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.SseGetPrizeCharts> listGetPrizeCharts = new List<BCW.SSE.Model.SseGetPrizeCharts>();

            // 计算记录数
            string countString = "SELECT COUNT(DISTINCT userId) FROM tb_SseGetPrize where " + strWhere + "";

            p_recordCount = Convert.ToInt32( SqlHelper.GetSingle( countString ) );

            if( p_recordCount > 0 )
            {
                int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
            }
            else
            {
                return listGetPrizeCharts;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "SELECT userId,SUM(prizeVal)totalprizeVal FROM tb_SseGetPrize where " + strWhere + " group by userId order by totalprizeVal desc";
            using( SqlDataReader reader = SqlHelper.ExecuteReader( queryString ) )
            {
                int stratIndex = ( p_pageIndex - 1 ) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while( reader.Read() )
                {
                    if( k >= stratIndex && k < endIndex )
                    {
                        BCW.SSE.Model.SseGetPrizeCharts objSseGetPrizeCharts = new BCW.SSE.Model.SseGetPrizeCharts();
                        objSseGetPrizeCharts.userId = reader.GetInt32( 0 );
                        objSseGetPrizeCharts.prizeVal = (Int64)reader.GetDecimal( 1 );
                        listGetPrizeCharts.Add( objSseGetPrizeCharts );
                    }
                    if( k == endIndex )
                        break;

                    k++;
                }
            }

            return listGetPrizeCharts;
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
			parameters[0].Value = "tb_SseGetPrize";
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

