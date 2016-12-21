using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Guest 的摘要说明。
    /// </summary>
    public class Guest
    {
        private readonly BCW.DAL.Guest dal = new BCW.DAL.Guest();
        public Guest()
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
        /// 得到Types
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            dal.UpdateTypes(ID, Types);
        }
        /// <summary>
        /// 得到Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// 增加一条系统消息
        /// 内线管理 20161024
        /// </summary>
        public int AddNew(int Types, int ToId, string ToName, string Content)
        {
            return dal.Add(Types, ToId, ToName, Content);
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
        public bool ExistsFrom(int ID, int UsID)
        {
            return dal.ExistsFrom(ID, UsID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool ExistsTo(int ID, int UsID)
        {
            return dal.ExistsTo(ID, UsID);
        }

        /// <summary>
        /// 计算多少条未读非系统消息
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 计算多少条未读系统消息
        /// </summary>
        public int GetXCount(int UsID)
        {
            return dal.GetXCount(UsID);
        }

        /// <summary>
        /// 计算某ID消息发送数
        /// </summary>
        public int GetIDCount(int UsID)
        {
            return dal.GetIDCount(UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// 增加发内线活跃抽奖入口--姚志光
        /// 删去入口20160608
        /// </summary>
        public int Add(BCW.Model.Guest model)
        {
            return dal.Add(model);
            //int ID = dal.Add(model);
            //try
            //{
            //    string xmlPath = "/Controls/winners.xml";
            //    string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            //    string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            //    string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            //    string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            //    string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            //    int usid = model.FromId;
            //    string username = model.FromName;
            //    string Notes = "发内线";
            //    int id = new BCW.BLL.Action().GetMaxId();
            //    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, id);
            //    if (isHit == 1)
            //    {
            //        if (WinnersGuessOpen == "1")
            //        {
            //            new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
            //        }
            //    }
            //    return ID;
            //}
            //catch { return ID; }
        }

        /// <summary>
        /// 增加一条系统消息
        /// </summary>
        public int Add(int ToId, string ToName, string Content)
        {
            return dal.Add(0, ToId, ToName, Content);
        }

        /// <summary>
        /// 增加一条系统消息
        /// </summary>
        public int Add(int Types, int ToId, string ToName, string Content)
        {
            bool flag = true;
            string ForumSet = new BCW.BLL.User().GetForumSet(ToId);
            if (ForumSet != "")
            {
                if (Types == 0)//系统消息
                {
                    int xTotal = GetForumSet(ForumSet, 15);
                    if (xTotal == 1)
                        flag = false;
                }
                else if (Types == 1)//游戏PK结果消息
                {
                    int gTotal = GetForumSet(ForumSet, 16);
                    if (gTotal == 1)
                        flag = false;
                }
                else if (Types == 2)//推荐邀请提醒消息
                {
                    int tTotal = GetForumSet(ForumSet, 14);
                    if (tTotal == 1)
                        flag = false;
                }
                else if (Types == 3)//空间留言提醒消息
                {
                    int bTotal = GetForumSet(ForumSet, 20);
                    if (bTotal == 1)
                        flag = false;
                }
                else if (Types == 4)//闲聊私聊提醒消息/竞猜走地成功失败提醒
                {
                    int sTotal = GetForumSet(ForumSet, 33);
                    if (sTotal == 1)
                        flag = false;
                }
                if (flag)
                {
                    return dal.Add(Types, ToId, ToName, Content);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 得到个性设置
        /// </summary>
        public int GetForumSet(string ForumSet, int iType)
        {
            string[] forumset = ForumSet.Split(",".ToCharArray());

            string[] fs = forumset[iType].ToString().Split("|".ToCharArray());

            return Convert.ToInt32(fs[1]);
        }

        /// <summary>
        /// 更新为收藏
        /// </summary>
        public void UpdateIsKeep(int ID, int UsID)
        {

            dal.UpdateIsKeep(ID, UsID);
        }

        /// <summary>
        /// 更新为已读
        /// </summary>
        public void UpdateIsSeen(int ID)
        {

            dal.UpdateIsSeen(ID);
        }

        /// <summary>
        /// 更新为已读
        /// </summary>
        public void UpdateIsSeenAll(int UsID, int ptype)
        {

            dal.UpdateIsSeenAll(UsID, ptype);
        }

        /// <summary>
        /// 隐藏一条数据
        /// </summary>
        public void UpdateFDel(int ID)
        {

            dal.UpdateFDel(ID);
        }

        /// <summary>
        /// 隐藏一条数据
        /// </summary>
        public void UpdateTDel(int ID)
        {

            dal.UpdateTDel(ID);
        }

        /// <summary>
        /// 隐藏已读数据
        /// </summary>
        public void UpdateTDel2(int UsID)
        {

            dal.UpdateTDel2(UsID);
        }

        /// <summary>
        /// 隐藏已发数据
        /// </summary>
        public void UpdateFDel2(int UsID)
        {

            dal.UpdateFDel2(UsID);
        }

        /// <summary>
        /// 删除已读系统消息
        /// </summary>
        public void UpdateXDel2(int UsID)
        {
            dal.UpdateXDel2(UsID);
        }


        /// <summary>
        /// 隐藏收藏数据
        /// </summary>
        public void UpdateKDel2(int UsID)
        {

            dal.UpdateKDel2(UsID);
        }

        /// <summary>
        ///  删除对话(发送)
        /// </summary>
        public void UpdateChatFDel(int UsID, int Hid)
        {

            dal.UpdateChatFDel(UsID, Hid);
        }

        /// <summary>
        ///  删除对话(接收)
        /// </summary>
        public void UpdateChatTDel(int UsID, int Hid)
        {

            dal.UpdateChatTDel(UsID, Hid);
        }

        /// <summary>
        ///  删除收信箱消息
        /// </summary>
        public void UpdateSClear(int UsID)
        {

            dal.UpdateSClear(UsID);
        }

        /// <summary>
        ///  删除系统消息
        /// </summary>
        public int UpdateXClear(int UsID)
        {

            return dal.UpdateXClear(UsID);
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
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 得到接收ID
        /// </summary>
        public int GetToId(int ID)
        {
            return dal.GetToId(ID);
        }

        /// <summary>
        /// 得到是否已读
        /// </summary>
        public int GetIsSeen(int ID)
        {
            return dal.GetIsSeen(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Guest GetGuest(int ID)
        {

            return dal.GetGuest(ID);
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
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGuests(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// 取得每页记录Asc
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuestsAsc(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetGuestsAsc(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        /// <summary>
        /// 取得每页ID集合
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Guest</returns>
        public IList<BCW.Model.Guest> GetGuestsID(int p_pageIndex, int p_pageSize, string strWhere)
        {
            return dal.GetGuestsID(p_pageIndex, p_pageSize, strWhere);
        }

        /// <summary>
        /// 存储过程分页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Guest> GetGuests(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetGuests(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }
        #endregion  成员方法
    }
}