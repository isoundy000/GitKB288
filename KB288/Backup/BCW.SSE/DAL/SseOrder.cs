using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using BCW.Common;
using BCW.Data;//Please add references

namespace BCW.SSE.DAL
{
	/// <summary>
	/// 数据访问类:SseOrder
	/// </summary>
	public partial class SseOrder
	{
		public SseOrder()
		{}
		#region  BasicMethod



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(BCW.SSE.Model.SseOrder model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into tb_SseOrder(");
			strSql.Append("orderType,sseNo,orderDateTime,userId,buyType,buyMoney,state,bz,isAutoOrder)");
			strSql.Append(" values (");
			strSql.Append("@orderType,@sseNo,@orderDateTime,@userId,@buyType,@buyMoney,@state,@bz,@isAutoOrder)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@orderType", SqlDbType.TinyInt,1),
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@orderDateTime", SqlDbType.DateTime),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@buyType", SqlDbType.Bit,1),
					new SqlParameter("@buyMoney", SqlDbType.Money,8),
					new SqlParameter("@state", SqlDbType.TinyInt,1),
					new SqlParameter("@bz", SqlDbType.NVarChar,500),
					new SqlParameter("@isAutoOrder", SqlDbType.Bit,1)};
			parameters[0].Value = model.orderType;
			parameters[1].Value = model.sseNo;
			parameters[2].Value = model.orderDateTime;
			parameters[3].Value = model.userId;
			parameters[4].Value = model.buyType;
			parameters[5].Value = model.buyMoney;
			parameters[6].Value = model.state;
			parameters[7].Value = model.bz;
			parameters[8].Value = model.isAutoOrder;

			object obj = BCW.Data.SqlHelper.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(BCW.SSE.Model.SseOrder model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update tb_SseOrder set ");
			strSql.Append("orderType=@orderType,");
			strSql.Append("sseNo=@sseNo,");
			strSql.Append("orderDateTime=@orderDateTime,");
			strSql.Append("userId=@userId,");
			strSql.Append("buyType=@buyType,");
			strSql.Append("buyMoney=@buyMoney,");
			strSql.Append("state=@state,");
			strSql.Append("bz=@bz,");
			strSql.Append("isAutoOrder=@isAutoOrder");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@orderType", SqlDbType.TinyInt,1),
					new SqlParameter("@sseNo", SqlDbType.Int,4),
					new SqlParameter("@orderDateTime", SqlDbType.DateTime),
					new SqlParameter("@userId", SqlDbType.Int,4),
					new SqlParameter("@buyType", SqlDbType.Bit,1),
					new SqlParameter("@buyMoney", SqlDbType.Money,8),
					new SqlParameter("@state", SqlDbType.TinyInt,1),
					new SqlParameter("@bz", SqlDbType.NVarChar,500),
					new SqlParameter("@isAutoOrder", SqlDbType.Bit,1),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.orderType;
			parameters[1].Value = model.sseNo;
			parameters[2].Value = model.orderDateTime;
			parameters[3].Value = model.userId;
			parameters[4].Value = model.buyType;
			parameters[5].Value = model.buyMoney;
			parameters[6].Value = model.state;
			parameters[7].Value = model.bz;
			parameters[8].Value = model.isAutoOrder;
			parameters[9].Value = model.id;

            int rows = BCW.Data.SqlHelper.ExecuteSql( strSql.ToString(), parameters );
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
			strSql.Append("delete from tb_SseOrder ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

            int rows = BCW.Data.SqlHelper.ExecuteSql( strSql.ToString(), parameters );
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
			strSql.Append("delete from tb_SseOrder ");
			strSql.Append(" where id in ("+idlist + ")  ");
            int rows = BCW.Data.SqlHelper.ExecuteSql( strSql.ToString() );
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
		public BCW.SSE.Model.SseOrder GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,orderType,sseNo,orderDateTime,userId,buyType,buyMoney,state,bz,isAutoOrder from tb_SseOrder ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
			};
			parameters[0].Value = id;

			BCW.SSE.Model.SseOrder model=new BCW.SSE.Model.SseOrder();
            DataSet ds = BCW.Data.SqlHelper.Query( strSql.ToString(), parameters );
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
		public BCW.SSE.Model.SseOrder DataRowToModel(DataRow row)
		{
			BCW.SSE.Model.SseOrder model=new BCW.SSE.Model.SseOrder();
			if (row != null)
			{
				if(row["id"]!=null && row["id"].ToString()!="")
				{
					model.id=int.Parse(row["id"].ToString());
				}
				if(row["orderType"]!=null && row["orderType"].ToString()!="")
				{
					model.orderType=int.Parse(row["orderType"].ToString());
				}
				if(row["sseNo"]!=null && row["sseNo"].ToString()!="")
				{
					model.sseNo=int.Parse(row["sseNo"].ToString());
				}
				if(row["orderDateTime"]!=null && row["orderDateTime"].ToString()!="")
				{
					model.orderDateTime=DateTime.Parse(row["orderDateTime"].ToString());
				}
				if(row["userId"]!=null && row["userId"].ToString()!="")
				{
					model.userId=int.Parse(row["userId"].ToString());
				}
				if(row["buyType"]!=null && row["buyType"].ToString()!="")
				{
					if((row["buyType"].ToString()=="1")||(row["buyType"].ToString().ToLower()=="true"))
					{
						model.buyType=true;
					}
					else
					{
						model.buyType=false;
					}
				}
				if(row["buyMoney"]!=null && row["buyMoney"].ToString()!="")
				{
					model.buyMoney=decimal.Parse(row["buyMoney"].ToString());
				}
				if(row["state"]!=null && row["state"].ToString()!="")
				{
					model.state=int.Parse(row["state"].ToString());
				}
				if(row["bz"]!=null)
				{
					model.bz=row["bz"].ToString();
				}
				if(row["isAutoOrder"]!=null && row["isAutoOrder"].ToString()!="")
				{
					if((row["isAutoOrder"].ToString()=="1")||(row["isAutoOrder"].ToString().ToLower()=="true"))
					{
						model.isAutoOrder=true;
					}
					else
					{
						model.isAutoOrder=false;
					}
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
			strSql.Append("select id,orderType,sseNo,orderDateTime,userId,buyType,buyMoney,state,bz,isAutoOrder ");
			strSql.Append(" FROM tb_SseOrder ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            return BCW.Data.SqlHelper.Query( strSql.ToString() );
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
			strSql.Append(" id,orderType,sseNo,orderDateTime,userId,buyType,buyMoney,state,bz,isAutoOrder ");
			strSql.Append(" FROM tb_SseOrder ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
            return BCW.Data.SqlHelper.Query( strSql.ToString() );
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM tb_SseOrder ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
            object obj = BCW.Data.SqlHelper.GetSingle( strSql.ToString() );
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
			strSql.Append(")AS Row, T.*  from tb_SseOrder T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return BCW.Data.SqlHelper.Query( strSql.ToString() );
		}


        public IList<BCW.SSE.Model.sseOrderHistory> GetSseOrderHistoryPages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.sseOrderHistory> listSseOrderHistory = new List<BCW.SSE.Model.sseOrderHistory>();

            // 计算记录数
            string countString = "SELECT COUNT(*) FROM tb_SseOrder o where " + strWhere + "";

            p_recordCount = Convert.ToInt32( SqlHelper.GetSingle( countString ) );

            if( p_recordCount > 0 )
            {
                int pageCount = BasePage.CalcPageCount( p_recordCount, p_pageSize, ref p_pageIndex );
            }
            else
            {
                return listSseOrderHistory;
            }

            // 取出相关记录
            string queryString = "";

            queryString = "select o.id,o.sseNo,o.orderDateTime,o.buyType,o.buyMoney,state,isnull(case o.bz when 2 then o.buyMoney else p.prizeVal  end,0) backMoney  from tb_SseOrder o left join tb_SseGetPrize p on o.id = p.orderId where " + strWhere + "order by o.id desc";
            using( SqlDataReader reader = SqlHelper.ExecuteReader( queryString ) )
            {
                int stratIndex = ( p_pageIndex - 1 ) * p_pageSize;
                int endIndex = p_pageIndex * p_pageSize;
                int k = 0;

                while( reader.Read() )
                {
                    if( k >= stratIndex && k < endIndex )
                    {
                        BCW.SSE.Model.sseOrderHistory objSseOrderHistory = new BCW.SSE.Model.sseOrderHistory();
                        objSseOrderHistory.sseOrder.id = reader.GetInt32( 0 );
                        objSseOrderHistory.sseOrder.sseNo = reader.GetInt32( 1 );
                        objSseOrderHistory.sseOrder.orderDateTime = reader.GetDateTime( 2 );
                        objSseOrderHistory.sseOrder.buyType = reader.GetBoolean( 3 );
                        objSseOrderHistory.sseOrder.buyMoney = reader.GetDecimal( 4 );
                        objSseOrderHistory.sseOrder.state = reader.GetByte( 5 );
                        objSseOrderHistory.backMoney = reader.GetDecimal( 6 );

                        listSseOrderHistory.Add( objSseOrderHistory );
                    }
                    if( k == endIndex )
                        break;

                    k++;
                }
            }

            return listSseOrderHistory;
        }



        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Luckpay</returns>
        public IList<BCW.SSE.Model.SseOrder> GetSseOrderPages( int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount )
        {
            IList<BCW.SSE.Model.SseOrder> listSseOrder = new List<BCW.SSE.Model.SseOrder>();
            string sTable = "tb_SseOrder";
            string sPkey = "id";
            string sField = "id,sseNo,orderDateTime,buyType,buyMoney,state";
            string sCondition = strWhere;
            string sOrder = " sseNo desc, orderDateTime  desc";
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
                    BCW.SSE.Model.SseOrder objSseOrder = new BCW.SSE.Model.SseOrder();
                    objSseOrder.id = reader.GetInt32( 0 );
                    objSseOrder.sseNo = reader.GetInt32( 1 );
                    objSseOrder.orderDateTime = reader.GetDateTime( 2 );
                    objSseOrder.buyType = reader.GetBoolean( 3 );
                    objSseOrder.buyMoney = reader.GetDecimal( 4 );
                    objSseOrder.state = reader.GetByte( 5 );

                    listSseOrder.Add( objSseOrder );

                }
            }
            return listSseOrder;
        }


        public decimal GetGuessMoney( int orderType, int _sseNo, int _buyType )
        {
            string countString = string.Format( "select SUM(buyMoney) buyMoney from tb_SseOrder where sseNo = {0} and orderType = {1} and buyType = {2} and (state=0 or bz='2')  group by buyType", _sseNo,orderType, _buyType );

            return Convert.ToDecimal( SqlHelper.GetSingle( countString ) );   
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
			parameters[0].Value = "tb_SseOrder";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return sqlHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

