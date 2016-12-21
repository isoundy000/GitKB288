using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类flows 的摘要说明。
	/// </summary>
	public class flows
	{
		private readonly BCW.DAL.Game.flows dal=new BCW.DAL.Game.flows();
		public flows()
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
        /// 是否存在花盆记录
        /// </summary>
        public bool Exists(int UsID)
        {
            return dal.Exists(UsID);
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
        public bool Exists(int ID, int UsID)
		{
            return dal.Exists(ID, UsID);
		}
        
        /// <summary>
        /// 计算某会员(空\非空\成熟)花盆数量
        /// </summary>
        public int GetCount(int UsID, int State)
        {
            return dal.GetCount(UsID, State);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.flows model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.flows model)
		{
			dal.Update(model);
		}
  
        /// <summary>
        /// 更新AddTime用作道具提前收获
        /// </summary>
        public void UpdateAddTime(int ID, DateTime AddTime)
        {
            dal.UpdateAddTime(ID, AddTime);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(int ID, int state)
        {
            dal.Update(ID, state);
        }
                     
        /// <summary>
        /// 更新水分
        /// </summary>
        public void Updatewater(int ID, int water)
        {
            dal.Updatewater(ID, water);
        }

        /// <summary>
        /// 更新杂草
        /// </summary>
        public void Updateweeds(int ID, int weeds)
        {
            dal.Updateweeds(ID, weeds);
        }
        /// <summary>
        /// 更新杂草
        /// </summary>
        public void Updateweeds2(int UsID, int weeds)
        {
            dal.Updateweeds2(UsID, weeds);
        }  
        /// <summary>
        /// 更新害虫
        /// </summary>
        public void Updateworm(int ID, int worm)
        {
            dal.Updateworm(ID, worm);
        }

        /// <summary>
        /// 更新害虫
        /// </summary>
        public void Updateworm2(int UsID, int worm)
        {
            dal.Updateworm2(UsID, worm);
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
		public BCW.Model.Game.flows Getflows(int ID)
		{
			
			return dal.Getflows(ID);
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
		/// <returns>IList flows</returns>
		public IList<BCW.Model.Game.flows> Getflowss(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.Getflowss(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
               
        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList flows</returns>
        public IList<BCW.Model.Game.flows> Getflowss2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount, int uid)
        {
            return dal.Getflowss2(p_pageIndex, p_pageSize, strWhere, out p_recordCount, uid);

        }

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public BCW.Model.Game.flows GetPreviousNextflows(int ID, int UsID, bool p_next,bool p_ismy)
        {
            return dal.GetPreviousNextflows(ID, UsID, p_next, p_ismy);
        }

        /// <summary>
        /// 取得上(下)一条记录
        /// </summary>
        public BCW.Model.Game.flows GetPreviousNextflows(int ID, int UsID, bool p_next)
        {
            return dal.GetPreviousNextflows(ID, UsID, p_next, true);
        }

		#endregion  成员方法
	}
}

