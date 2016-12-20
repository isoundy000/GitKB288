using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
	/// <summary>
	/// 业务逻辑类Textcent 的摘要说明。
	/// </summary>
    /// 增加发帖抽奖入口0528姚志光
    /// 增加点值抽奖入口 20160823 蒙宗将
	public class Textcent
	{
		private readonly BCW.DAL.Textcent dal=new BCW.DAL.Textcent();
		public Textcent()
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
        /// 根据条件得到今天使用论坛基金打赏币额
        /// </summary>
        public long GetForrmCents(int BID, int BzType, int ToID)
        {
            return dal.GetForrmCents(BID,BzType,ToID);
        }  
        /// <summary>
        /// 根据条件得到今天打赏币额
        /// </summary>
        public long GetCents(int BID, int BzType, int UsID)
        {
            return dal.GetCents(BID, BzType, UsID);
        }

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(BCW.Model.Textcent model)
		{
			//return dal.Add(model);
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
                string username = model.UsName;
                string Notes = "发表帖子";
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

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子打赏";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }
                return ID;

		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public void Update(BCW.Model.Textcent model)
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
		public BCW.Model.Textcent GetTextcent(int ID)
		{
			
			return dal.GetTextcent(ID);
		}
                
        /// <summary>
        /// 得到最后一个对象实体，帖子里面的
        /// </summary>
        public BCW.Model.Textcent GetTextcentLast(int BID)
        {

            return dal.GetTextcentLast(BID);
        }
        /// <summary>
        /// 得到一个打赏对象实体，回复里面的
        /// </summary>
        public BCW.Model.Textcent GetTextcentReply(int ToID,int Floor, int BID)
        {

            return dal.GetTextcentReplyFloor(ToID,Floor,BID);
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
		/// <returns>IList Textcent</returns>
		public IList<BCW.Model.Textcent> GetTextcents(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
		{
			return dal.GetTextcents(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
		}
               
        /// <summary>
        /// 取到排行榜
        /// </summary>
        /// <param name="Types">排行类别</param>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Textcent> GetTextcentsTop(int Types, int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetTextcentsTop(Types, p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

		#endregion  成员方法
	}
}

