using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Transfer 的摘要说明。
	/// </summary>
	public class Transfer
	{
		private readonly BCW.DAL.Transfer dal=new BCW.DAL.Transfer();
		public Transfer()
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
        /// 是否订单号是否重复
        /// </summary>
        public bool Exists(int FromId, string zfbNo)
        {
            return dal.Exists(FromId, zfbNo);
        }
                
        /// <summary>
        /// 计算某用户今天过币额
        /// </summary>
        public long GetAcCents(int FromID, int Types)
        {   
            return dal.GetAcCents(FromID, Types);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Transfer model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
        /// 过户币增加抽奖入口---姚志光
		/// </summary>
		public void Update(BCW.Model.Transfer model)
		{
			dal.Update(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = model.FromId;
                string username = model.FromName;
                string Notes = "过户";
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
            catch {  }
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
		public BCW.Model.Transfer GetTransfer(int ID)
		{
			
			return dal.GetTransfer(ID);
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
		/// <returns>IList Transfer</returns>
		public IList<BCW.Model.Transfer> GetTransfers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTransfers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}

        /// <summary>
        /// 根据条件计算过币次数
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }
		#endregion  成员方法
	}
}

