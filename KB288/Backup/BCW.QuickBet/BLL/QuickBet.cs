using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.QuickBet.Model;
namespace BCW.QuickBet.BLL
{
	/// <summary>
	/// 业务逻辑类QuickBet 的摘要说明。
	/// </summary>
	public class QuickBet
	{
		private readonly BCW.QuickBet.DAL.QuickBet dal=new BCW.QuickBet.DAL.QuickBet();
		public QuickBet()
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
        /// 得到默认游戏
        /// </summary>
        public string GetGame()
        {
            string str="1#2#3#4#5#6#7#8#9#10";//支持十个游戏快捷
            return str;
        }

        /// <summary>
        /// 得到默认快捷下注
        /// </summary>
        public string GetBety()
        {
            string str = "100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0#100|500|1000|10000|1000000|0|0|0|0|0";
            return str;
        }

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			return dal.Exists(ID);
		}

        /// <summary>
        /// 是否存在该用户
        /// </summary>
        public bool ExistsUsID(int UsID)
        {
            return dal.ExistsUsID(UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.QuickBet.Model.QuickBet model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.QuickBet.Model.QuickBet model)
		{
			dal.Update(model);
		}

        /// <summary>
        /// 更新Game
        /// </summary>
        public void UpdateGame(int UsID,string Game)
        {
            dal.UpdateGame(UsID,Game);
        }

        /// <summary>
        /// 更新Bet
        /// </summary>
        public void UpdateBet(int UsID, string Bet)
        {
            dal.UpdateBet(UsID, Bet);
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// 根据用户ID得到Game
        /// </summary>
        public string GetGame(int UsID)
        {
          return  dal.GetGame(UsID);
        }

        /// <summary>
        /// 根据用户ID得到Bet
        /// </summary>
        public string  GetBet(int UsID)
        {
           return  dal.GetBet(UsID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.QuickBet.Model.QuickBet GetQuickBet(int ID)
		{
			
			return dal.GetQuickBet(ID);
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
		/// <returns>IList QuickBet</returns>
		public IList<BCW.QuickBet.Model.QuickBet> GetQuickBets(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetQuickBets(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

