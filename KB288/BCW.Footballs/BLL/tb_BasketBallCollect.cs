using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类tb_BasketBallCollect 的摘要说明。
	/// </summary>
	public class tb_BasketBallCollect
	{
		private readonly BCW.DAL.tb_BasketBallCollect dal=new BCW.DAL.tb_BasketBallCollect();
		public tb_BasketBallCollect()
		{}
		#region  成员方法
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsUsIdAndBaskId(int UsId, int BaskId)
        {
            return dal.ExistsUsIdAndBaskId(UsId, BaskId);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int  Add(BCW.Model.tb_BasketBallCollect model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.tb_BasketBallCollect model)
		{
			dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}
        /// <summary>
        /// 删除一条数据  邵广林 20161004 增加删除收藏
        /// </summary>
        public void Delete(int ID, int usid)
        {

            dal.Delete(ID, usid);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.tb_BasketBallCollect Gettb_BasketBallCollect(int ID)
		{
			
			return dal.Gettb_BasketBallCollect(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList tb_BasketBallCollect</returns>
		public IList<BCW.Model.tb_BasketBallCollect> Gettb_BasketBallCollects(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Gettb_BasketBallCollects(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

