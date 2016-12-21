using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类Applepay 的摘要说明。
	/// </summary>
	public class Applepay
	{
		private readonly BCW.DAL.Game.Applepay dal=new BCW.DAL.Game.Applepay();
		public Applepay()
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
        public bool Exists(int Types, int UsID, int AppleId)
        {
            return dal.Exists(Types, UsID, AppleId);
		}
                       
        /// <summary>
        /// 是否存在未开记录
        /// </summary>
        public bool ExistsState(int AppleId)
        {
            return dal.ExistsState(AppleId);
        }

        /// <summary>
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }
        
        /// <summary>
        /// 计算某ID在本局下注数
        /// </summary>
        public int GetCount(int UsID, int AppleId)
        {
            return dal.GetCount(UsID, AppleId);
        }
                
        /// <summary>
        /// 计算本局某类型下注数
        /// </summary>
        public long GetCount2(int Types, int AppleId)
        {
            return dal.GetCount2(Types, AppleId);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.Applepay model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.Applepay model)
		{
			dal.Update(model);
		}
              
        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int ID, long WinCent, int State)
        {
            dal.Update(ID, WinCent, State);
        }
                
        /// <summary>
        /// 更新开奖
        /// </summary>
        public void Update(int AppleId, int Types)
        {
            dal.Update(AppleId, Types);
        }

        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID)
        {
            dal.UpdateState(ID);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int Types,int ID)
		{
			
			dal.Delete(Types,ID);
		}
        
        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {
            dal.Delete(strWhere);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.Applepay GetApplepay(int Types,int ID)
		{
			
			return dal.GetApplepay(Types,ID);
		}

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
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
		/// <returns>IList Applepay</returns>
		public IList<BCW.Model.Game.Applepay> GetApplepays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetApplepays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

