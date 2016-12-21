using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Marry 的摘要说明。
	/// </summary>
	public class Marry
	{
		private readonly BCW.DAL.Marry dal=new BCW.DAL.Marry();
		public Marry()
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
        public bool Exists(int UsID, int ReID, int Types)
        {
            return dal.Exists(UsID, ReID, Types);
        }
        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int UsID, int ReID)
        {
            return dal.Exists(UsID, ReID);
        }
                        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int UsID, int ReID)
        {
            return dal.Exists2(UsID, ReID);
        }

        
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int UsID, int ReID, int Types)
        {
            return dal.Exists2(UsID, ReID, Types);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists3(int UsID, int ReID, int State)
        {
            return dal.Exists3(UsID, ReID, State);
        }
                
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists4(int UsID, int ReID, int State)
        {
            return dal.Exists4(UsID, ReID, State);
        }

        /// <summary>
        /// 是否存在恋爱记录
        /// </summary>
        public bool ExistsLove(int UsID)
        {
            return dal.ExistsLove(UsID);
        }
                
        /// <summary>
        /// 是否存在结婚记录
        /// </summary>
        public bool ExistsMarry(int UsID)
        {
            return dal.ExistsMarry(UsID);
        }
            
        /// <summary>
        /// 某会员是否存在非离婚的记录
        /// </summary>
        public bool ExistsLostMarry(int UsID)
        {
            return dal.ExistsLostMarry(UsID);
        }

        /// <summary>
        /// 更新HomeClick
        /// </summary>
        public void UpdateHomeClick(int ID, int HomeClick)
        {
            dal.UpdateHomeClick(ID, HomeClick);
        }

        /// <summary>
        /// 更新LoveStat
        /// </summary>
        public void UpdateLoveStat(int ID, string LoveStat)
        {
            dal.UpdateLoveStat(ID, LoveStat);
        }

                
        /// <summary>
        /// 更新花园名称
        /// </summary>
        public void UpdateHomeName(int ID, string HomeName)
        {
            dal.UpdateHomeName(ID, HomeName);
        }
                
        /// <summary>
        /// 更新男誓言
        /// </summary>
        public void UpdateOath(int ID, string Oath)
        {
            dal.UpdateOath(ID, Oath);
        }

        /// <summary>
        /// 更新女誓言
        /// </summary>
        public void UpdateOath2(int ID, string Oath2)
        {
            dal.UpdateOath2(ID, Oath2);
        }
               
        /// <summary>
        /// 更新FlowStat、FlowTimes和鲜花数量
        /// </summary>
        public void UpdateFlowStat(int ID, string FlowStat, string FlowTimes, int FlowNum)
        {
            dal.UpdateFlowStat(ID, FlowStat, FlowTimes, FlowNum);
        }

        /// <summary>
        /// 成为夫妻
        /// </summary>
        public void UpdateMarry(int UsID, int ReID, string Oath)
        {
            dal.UpdateMarry(UsID, ReID, Oath);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "成为夫妻";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
                    }
                }
                
            }
            catch {}

        }
                
        /// <summary>
        /// 成为夫妻
        /// </summary>
        public void UpdateMarry(int UsID, int ReID)
        {
            dal.UpdateMarry(UsID, ReID);
        }

        /// <summary>
        /// 取消求婚请求
        /// </summary>
        public void UpdateMarry2(int UsID, int ReID)
        {
            dal.UpdateMarry2(UsID, ReID);
        }

             
        /// <summary>
        /// 成为离婚
        /// </summary>
        public void UpdateLost(int UsID, int ReID, string Oath2)
        {
            dal.UpdateLost(UsID, ReID, Oath2);
        }
                
        /// <summary>
        /// 成为离婚
        /// </summary>
        public void UpdateLost(int UsID, int ReID)
        {
            dal.UpdateLost(UsID, ReID);
        }
                
        /// <summary>
        /// 取消离婚请求
        /// </summary>
        public void UpdateLost2(int UsID, int ReID)
        {
            dal.UpdateLost2(UsID, ReID);
        }

		/// <summary>
		/// 增加一条数据
        /// 加入活跃抽奖入口---20160528
		/// </summary>
		public int  Add(BCW.Model.Marry model)
		{
		//	return dal.Add(model);
            int ID = dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "结婚";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
                    }
                }
                return ID;
            }
            catch { return ID; }
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Marry model)
		{
			dal.Update(model);
		}
               
        /// <summary>
        /// 成为恋人
        /// </summary>
        public void UpdateLove(int UsID, int ReID)
        {
            dal.UpdateLove(UsID, ReID);
          
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "成为恋人";
                int id = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
                    }
                }
            }
            catch { }
        }

        
        /// <summary>
        /// 更新结婚证地址
        /// </summary>
        public void UpdateMarryPk(int ID, string MarryPk)
        {
            dal.UpdateMarryPk(ID, MarryPk);
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
        /// 得到鲜花排名
        /// </summary>
        public int GetFlowNumTop(int ID)
        {
            return dal.GetFlowNumTop(ID);
        }

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public BCW.Model.Marry GetMarry(int ID)
		{
			
			return dal.GetMarry(ID);
		}

		/// <summary>
		/// 根据字段取数据列表
		/// </summary>
		public DataSet GetList(string strField, string strWhere)
		{
			return dal.GetList(strField, strWhere);
		}
                
        /// <summary>
        /// 取得排行榜记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Marry</returns>
        public IList<BCW.Model.Marry> GetMarrysTop(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetMarrysTop(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

		/// <summary>
		/// 取得每页记录
		/// </summary>
		/// <param name="p_pageIndex">当前页</param>
		/// <param name="p_pageSize">分页大小</param>
		/// <param name="p_recordCount">返回总记录数</param>
		/// <param name="strWhere">查询条件</param>
		/// <returns>IList Marry</returns>
		public IList<BCW.Model.Marry> GetMarrys(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetMarrys(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

		#endregion  成员方法
	}
}

