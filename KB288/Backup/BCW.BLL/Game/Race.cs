using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Race 的摘要说明。
	/// </summary>
	public class Race
	{
		private readonly BCW.DAL.Game.Race dal=new BCW.DAL.Game.Race();
		public Race()
		{}
		#region  成员方法

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

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
        public bool Exists(int ID, int userid)
        {
            return dal.Exists(ID, userid);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID, int Types, int paytype)
        {
            return dal.Exists(ID, Types, paytype);
        }
                
        /// <summary>
        /// 计算某用户今天发布竞拍数量
        /// </summary>
        public int GetTodayCount(int userid)
        {
            return dal.GetTodayCount(userid);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Race model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Race model)
		{
			dal.Update(model);
		}
    
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatetotime(int ID, DateTime totime)
        {
            dal.Updatetotime(ID, totime);
        }
     
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Updatepaytype(int ID, int paytype)
        {
            dal.Updatepaytype(ID, paytype);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdatetopPrice(int ID, long topPrice, int winID, string winName, int paytype)
        {
            dal.UpdatetopPrice(ID, topPrice, winID, winName, paytype);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Race GetRace(int ID)
		{
			
			return dal.GetRace(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Race</returns>
        public IList<BCW.Model.Game.Race> GetRaces(int SizeNum, string strWhere)
        {
            return dal.GetRaces(SizeNum, strWhere);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Race</returns>
		public IList<BCW.Model.Game.Race> GetRaces(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetRaces(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

