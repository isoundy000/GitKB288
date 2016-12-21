using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model.Game;
namespace BCW.BLL.Game
{
	/// <summary>
	/// 业务逻辑类SSCpay 的摘要说明。
	/// </summary>
	public class SSCpay
	{
		private readonly BCW.DAL.Game.SSCpay dal=new BCW.DAL.Game.SSCpay();
		public SSCpay()
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
        /// 是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Game.SSCpay model)
		{
			return dal.Add(model);
		}

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add2(BCW.Model.Game.SSCpay model)
        {
            return dal.Add2(model);
        }

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Game.SSCpay model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 更新用户兑奖标识
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }
                
        /// <summary>
        /// 更新时时彩开奖结果
        /// </summary>
        public void UpdateResult(int SSCId, string Result)
        {
            dal.UpdateResult(SSCId, Result);
        }
                
        /// <summary>
        /// 更新游戏开奖得币
        /// </summary>
        public void UpdateWinCent(int ID, int SSCId, int UsID, string UsName, long WinCent, string WinNotes)
        {
            if (ub.GetSub("SSCGuestSet", "/Controls/ssc.xml") == "0")
            {
                new BCW.BLL.Guest().Add(1, UsID, UsName, "您的" + SSCId + "期时时彩:" + WinNotes.Replace("|", "中奖") + "" + ub.Get("SiteBz") + "！[url=/bbs/game/ssc.aspx?act=case]兑奖[/url]");
            }

            dal.UpdateWinCent(ID, WinCent, WinNotes);
            //机器人自动兑奖
            if (new BCW.BLL.Game.SSCpay().GetIsSpier(ID) == 1)
            {
                new BCW.BLL.Game.SSCpay().UpdateState(ID, 2);

                new BCW.BLL.User().UpdateiGold(UsID, UsName, WinCent, 11);
            }
        }

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public void Delete(int ID)
		{
			
			dal.Delete(ID);
		}

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }
                
        /// <summary>
        /// 某期某ID共投了多少币
        /// </summary>
        public long GetSumPrices(int UsID, int SSCId)
        {
            return dal.GetSumPrices(UsID, SSCId);
        }

        /// <summary>
        /// 得到一个WinCent
        /// </summary>
        public long GetWinCent(int ID)
        {
            return dal.GetWinCent(ID);
        }
        
        /// <summary>
        /// 得到一个IsSpier
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// 根据条件计算币本金值
        /// </summary>
        public long GetSumPrices(string strWhere)
        {
            return dal.GetSumPrices(strWhere);
        }

        /// <summary>
        /// 根据条件计算返彩值
        /// </summary>
        public long GetSumWinCent(string strWhere)
        {
            return dal.GetSumWinCent(strWhere);
        }    
        
        /// <summary>
        /// 得到一个WinCentNotes
        /// </summary>
        public string GetWinNotes(int ID)
        {

            return dal.GetWinNotes(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Game.SSCpay GetSSCpay(int ID)
		{
			
			return dal.GetSSCpay(ID);
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
		/// <returns>IList SSCpay</returns>
		public IList<BCW.Model.Game.SSCpay> GetSSCpays(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetSSCpays(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}


        /// <summary>
        /// 取得排行记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList SSCpay</returns>
        public IList<BCW.Model.Game.SSCpay> GetSSCpaysTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSSCpaysTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
		#endregion  成员方法
	}
}

