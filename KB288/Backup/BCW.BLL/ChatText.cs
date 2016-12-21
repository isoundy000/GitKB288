using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类ChatText 的摘要说明。
    /// </summary>
    public class ChatText
    {
        private readonly BCW.DAL.ChatText dal = new BCW.DAL.ChatText();
        public ChatText()
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
        /// 计算某会员ID发表的聊天数
        /// </summary>
        public int GetCount(int UsID)
        {
            return dal.GetCount(UsID);
        }

        /// <summary>
        /// 计算发表的聊天数
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// 计算某时间段的闲聊发表数
        /// </summary>
        public int GetCount(DateTime dt)
        {
            return dal.GetCount(dt);
        }


        ///// <summary>
        ///// 增加一条数据
        ///// </summary>
        //public int  Add(BCW.Model.ChatText model)
        //{
        //    return dal.Add(model);
        //}
        /// <summary>
        /// 增加一条数据
        /// 活跃抽奖入口_酷友热聊室
        /// </summary>
        public int Add(BCW.Model.ChatText model)
        {
            int id = dal.Add(model);
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
            int UsId = model.UsID;
            string UsName = model.UsName;
            string Notes = "聊吧酷友热聊室";
            //活跃抽奖开关
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, UsId, UsName, Notes, id);
                    if (isHit == 1)
                    {
                        if (WinnersGuessOpen == "1")
                        {
                            new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//发内线到该ID
                            //if (ActionOpen == "1")
                            //{
                            //    Add(UsId, ActionText);
                            //}
                        }
                    }
                    //return id;
                }
                catch
                {
                    // return id;
                }
            }
            else
            {

            }

            // 蒙宗将 20160910 增加聊吧发言点值抽奖接口
            try { new BCW.Draw.draw().AddjfbyTz(UsId, UsName, "聊吧发言"); }
            catch { }
            return id;

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.ChatText model)
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
        /// 删除某聊室某ID聊天数据
        /// </summary>
        public void Delete(int ChatId, int UsID)
        {
            dal.Delete(ChatId, UsID);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(int ChatId)
        {
            dal.DeleteStr(ChatId);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void DeleteStr(string strWhere)
        {

            dal.DeleteStr(strWhere);
        }

        /// <summary>
        /// 删除某聊天室N天前的数据
        /// </summary>
        public void DeleteStr(int ChatId, int Day)
        {
            dal.DeleteStr(ChatId, Day);
        }

        /// <summary>
        /// 根据条件删除聊天室数据
        /// </summary>
        public void DeleteStr2(string strWhere)
        {
            dal.DeleteStr2(strWhere);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.ChatText GetChatText(int ID)
        {
            return dal.GetChatText(ID);
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
        /// <returns>IList ChatText</returns>
        public IList<BCW.Model.ChatText> GetChatTexts(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetChatTexts(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 发言排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.ChatText> GetChatTextsTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetChatTextsTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}

