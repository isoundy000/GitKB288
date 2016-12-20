using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Friend 的摘要说明。
    /// </summary>
    public class Friend
    {
        private readonly BCW.DAL.Friend dal = new BCW.DAL.Friend();
        public Friend()
        { }
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
        public bool Exists(int UsID, int FriendId, int Types)
        {
            return dal.Exists(UsID, FriendId, Types);
        }

        /// <summary>
        /// 计算某分组好友数量
        /// </summary>
        public int GetCount(int UsID, int NodeId)
        {
            return dal.GetCount(UsID, NodeId);
        }

        /// <summary>
        /// 计算某ID好友数量
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 计算某ID粉丝数量
        /// </summary>
        public int GetFansCount(int UsID)
        {
            return dal.GetFansCount(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// 加好友加入抽奖动态201605
        /// </summary>
        public int Add(BCW.Model.Friend model)
        {
           // return dal.Add(model);
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
                string Notes = "加好友";
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
        /// 更新某分组好友成为默认分类
        /// </summary>
        public void UpdateNodeId(int UsID, int NodeId)
        {
            dal.UpdateNodeId(UsID, NodeId);
        }

        /// <summary>
        /// 更新最后联系时间
        /// </summary>
        public void UpdateTime(int UsID, int FriendID)
        {
            dal.UpdateTime(UsID, FriendID);
        }

        /// <summary>
        /// 更新正式好友
        /// </summary>
        public void UpdateTypes(int UsID, int FriendID)
        {
            dal.UpdateTypes(UsID, FriendID);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Friend model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(int UsID, int NodeId)
        {

            dal.Delete(UsID, NodeId);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int UsID, int FriendID, int Types)
        {
            dal.Delete(UsID, FriendID, Types);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Friend GetFriend(int FriendId)
        {

            return dal.GetFriend(FriendId);
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
        /// <returns>IList Friend</returns>
        public IList<BCW.Model.Friend> GetFriends(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetFriends(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

