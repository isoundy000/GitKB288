using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.XinKuai3.Model;
namespace BCW.XinKuai3.BLL
{
	/// <summary>
	/// 业务逻辑类XK3_Bet_SWB 的摘要说明。
	/// </summary>
	public class XK3_Bet_SWB
	{
		private readonly BCW.XinKuai3.DAL.XK3_Bet_SWB dal=new BCW.XinKuai3.DAL.XK3_Bet_SWB();
		public XK3_Bet_SWB()
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
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.XinKuai3.Model.XK3_Bet_SWB model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.XinKuai3.Model.XK3_Bet_SWB model)
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
		/// 得到一个对象实体
		/// </summary>
		public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB(int ID)
		{
			
			return dal.GetXK3_Bet_SWB(ID);
		}



        //===============================
        /// <summary>
        /// me_机器人增加一条数据
        /// </summary>
        public int Add_Robot(BCW.XinKuai3.Model.XK3_Bet_SWB model)
        {
            return dal.Add_Robot(model);
        }
        /// <summary>
        /// me_更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
        }

        /// <summary>
        /// me_是否存在兑奖记录
        /// </summary>
        public bool ExistsState(int ID, int UsID)
        {
            return dal.ExistsState(ID, UsID);
        }

        /// <summary>
        /// me_是否存在开奖后没有返奖的
        /// </summary>
        public bool Exists_num(string Lottery_issue)
        {
            return dal.Exists_num(Lottery_issue);
        }
        /// <summary>
        /// me_更新中奖状态
        /// </summary>
        public void Update_win(int ID, long GetMoney)
        {
            dal.Update_win(ID, GetMoney);
        }
        /// <summary>
        /// me_根据所点的历史记录，通过开奖期号查询相应的投注情况
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_num(string Lottery_issue)
        {
            return dal.GetXK3_Bet_SWB_num(Lottery_issue);
        }
        /// <summary>
        /// me_查找中奖后，超7天未领奖的id
        /// </summary>
        public void UpdateExceed_num(string _where)
        {
            dal.UpdateExceed_num(_where);
        }
        /// <summary>
        /// 计算投注总币值
        /// </summary>
        public long GetPrice(string ziduan,string strWhere)
        {
            return dal.GetPrice(ziduan,strWhere);
        }
        /// <summary>
        /// me_后台，根据开奖的期号，对应该期购买的人数
        /// </summary>
        public BCW.XinKuai3.Model.XK3_Bet_SWB GetXK3_Bet_SWB_hounum(string Lottery_issue)
        {
            return dal.GetXK3_Bet_SWB_hounum(Lottery_issue);
        }
        /// <summary>
        /// me_初始化某数据表
        /// </summary>
        /// <param name="TableName">数据表名称</param>
        public void ClearTable(string TableName)
        {
            dal.ClearTable(TableName);
        }
        /// <summary>
        /// me_查询机器人购买次数
        /// </summary>
        public int GetXK3_Bet_SWB_GetRecordCount(string strWhere)
        {
            return dal.GetXK3_Bet_SWB_GetRecordCount(strWhere);
        }

        /// <summary>
        /// me_取得每个玩家投注的记录进行排序
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList XK3_Bet_SWB</returns>
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum1(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum1(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWB_playnum3(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetXK3_Bet_SWB_playnum3(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// me_后台分页条件获取排行榜数据列表
        /// </summary>
        public DataSet GetListByPage2(int startIndex, int endIndex, string s1, string s2)
        {
            return dal.GetListByPage2(startIndex, endIndex, s1, s2);
        }



        //================================




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
		/// <returns>IList XK3_Bet_SWB</returns>
		public IList<BCW.XinKuai3.Model.XK3_Bet_SWB> GetXK3_Bet_SWBs(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetXK3_Bet_SWBs(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

