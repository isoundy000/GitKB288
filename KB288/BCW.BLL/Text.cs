using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /**
     * 
     * 修改人 陈志基 2016/4/9
     * 修改人 姚志光 2016/5/28
     * 增加帖子活跃抽奖入口事件
     * 修改人  陈志基 2016/08/11
     * 增加帖子统计方法
     * 增加帖子产生点值抽奖入口 蒙宗将 2016/08/23
      **/

    /// <summary>
    /// 业务逻辑类Text 的摘要说明。
    /// </summary>
    public class Text
    {
        private readonly BCW.DAL.Text dal = new BCW.DAL.Text();
        public Text()
        { }
        #region  成员方法

        /// <summary>
        /// 更新新闻已采集的标识
        /// </summary>
        public void UpdateIstxt(int ID, int Istxt)
        {

            dal.UpdateIstxt(ID, Istxt);
        }
        //------------------------高手论坛使用-------------------------



        /// <summary>
        /// 更新总中奖数和连中、月中
        /// </summary>
        public void UpdategGsNum(int ID, int gwinnum, int glznum, int gmnum)
        {
            dal.UpdategGsNum(ID, gwinnum, glznum, gmnum);
        }

        /// <summary>
        /// 得到连中的次数
        /// </summary>
        public int Getglznum(int ID)
        {
            return dal.Getglznum(ID);
        }
        /// <summary>
        /// 得到点赞数
        /// </summary>
        public int GetPraise(int ID)
        {
            return dal.GetPraise(ID);
        }
        ///// <summary>
        ///// 得到点赞人ID
        ///// </summary>
        //public int GetPraiseID(int ID)
        //{
        //    return dal.GetPraiseID(ID);
        //}
        /// <summary>
        /// 更新新一期的期数
        /// </summary>
        public void UpdateGqinum(int ID, int Gqinum)
        {
            dal.UpdateGqinum(ID, Gqinum);
        }
        //------------------------高手论坛使用-------------------------

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
        /// 是否存在该记录(回收站)
        /// </summary>
        public bool Exists(int ID, int ForumID)
        {
            return dal.Exists(ID, ForumID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int ForumID)
        {
            return dal.Exists2(ID, ForumID);
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists2(int ID, int ForumID, int UsID)
        {
            return dal.Exists2(ID, ForumID, UsID);
        }

        /// <summary>
        /// 会员在某高手论坛是否存在发帖
        /// </summary>
        public int GetRaceBID(int ForumID, int UsID)
        {
            return dal.GetRaceBID(ForumID, UsID);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(BCW.Model.Text model)
        {
            int id= dal.Add(model);

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "发表帖子";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }

            return id;
        }
        /// <summary>
        /// 增加一条派币帖
        /// </summary>
        public int AddPricesLimit(BCW.Model.Text model)
        {
            int id = dal.AddPricesLimit(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = (model.UsID);
                string username = model.UsName;
                string Notes = "帖子币";
                int ID = new BCW.BLL.Action().GetMaxId();
                int isHit = new BCW.winners.winners().CheckActionForAll(0, 0, usid, username, Notes, ID);
                if (isHit == 1)
                {
                    if (WinnersGuessOpen == "1")
                    {
                        new BCW.BLL.Guest().Add(0, usid, username, TextForUbb);//发内线到该ID
                    }
                }

            }
            catch {  }

            try
            {
                int usid = model.UsID;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "发表帖子";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }  
            
            return id;
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Text model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update2(BCW.Model.Text model)
        {
            dal.Update2(model);
        }

        /// <summary>
        /// 更新贴子内容
        /// </summary>
        public void UpdateContent(int ID, string Content)
        {
            dal.UpdateContent(ID, Content);
        }

        /// <summary>
        /// 更新帖子类型
        /// </summary>
        public void UpdateTypes(int ID, int Types)
        {
            dal.UpdateTypes(ID, Types);
        }
        /// <summary>
        /// 更新点赞数
        /// </summary>
        public void UpdatePraise(int ID, int Praise)
        {
            dal.UpdatePraise(ID, Praise);
        }

        // /// <summary>
        ///// 更新点赞人ID
        ///// </summary>
        //public void UpdatePraiseID(int ID, int PraiseID)
        //{
        //    dal.UpdatePraiseID(ID,PraiseID);
        //}
        /// <summary>
        /// 更新详细统计
        /// </summary>
        public void UpdateReStats(int ID, string ReStats, string ReList)
        {
            dal.UpdateReStats(ID, ReStats, ReList);
        }

        /// <summary>
        /// 更新购买ID
        /// </summary>
        public void UpdatePayID(int ID, string PayID)
        {
            dal.UpdatePayID(ID, PayID);
        }

        /// <summary>
        /// 更新回复ID
        /// </summary>
        public void UpdateReplyID(int ID, string ReplyID)
        {
            dal.UpdateReplyID(ID, ReplyID);
        }
        /// <summary>
        /// 更新已派币ID
        /// </summary>
        public void UpdateIsPriceID(int ID, string IsPriceID)
        {
            dal.UpdateIsPriceID(ID, IsPriceID);
        }
        /// <summary>
        /// 更新点赞ID
        /// </summary>
        public void UpdatePraiseID(int ID, string PraiseID)
        {
            dal.UpdatePraiseID(ID, PraiseID);
        }

        /// <summary>
        /// 更新已派多少币
        /// </summary>
        public void UpdatePricel(int ID, long Price)
        {
            dal.UpdatePricel(ID, Price);
        }

        /// <summary>
        /// 更新回复数
        /// </summary>
        public void UpdateReplyNum(int ID, int ReplyNum)
        {
            dal.UpdateReplyNum(ID, ReplyNum);
        }

        /// <summary>
        /// 更新阅读数
        /// </summary>
        public void UpdateReadNum(int ID, int ReadNum)
        {
            dal.UpdateReadNum(ID, ReadNum);
        }

        /// <summary>
        /// 更新专题标识
        /// </summary>
        public void UpdateTsID(int ID, int TsID)
        {
            dal.UpdateTsID(ID, TsID);
        }

        /// <summary>
        /// 批量更新专题标识
        /// </summary>
        public void UpdateTsID2(int TsID, int NewTsID)
        {
            dal.UpdateTsID2(TsID, NewTsID);
        }

        /// <summary>
        /// 批量转移帖子
        /// </summary>
        public void UpdateForumID2(int ForumID, int NewForumID)
        {
            dal.UpdateForumID2(ForumID, NewForumID);
        }

        /// <summary>
        /// 更新帖子文件数
        /// </summary>
        public void UpdateFileNum(int ID, int FileNum)
        {
            dal.UpdateFileNum(ID, FileNum);
        }

        //-------------------------------------版主管理-------------------------------------------

        /// <summary>
        /// 放入/取出回收站
        /// </summary>
        public void UpdateIsDel(int ID, int IsDel)
        {
            dal.UpdateIsDel(ID, IsDel);
        }

        ///// <summary>
        ///// 加精/解精
        ///// </summary>
        //public void UpdateIsGood(int ID, int IsGood)
        //{
        //    dal.UpdateIsGood(ID, IsGood);
        //}
        ///// <summary>
        ///// 推荐/解推荐
        ///// </summary>
        //public void UpdateIsRecom(int ID, int IsRecom)
        //{
        //    dal.UpdateIsRecom(ID, IsRecom);
        //} 

        /// <summary>
        /// 置顶/去顶/固底/去底
        /// </summary>
        //public void UpdateIsTop(int ID, int IsTop)
        //{
        //    dal.UpdateIsTop(ID, IsTop);
        //}

        /// <summary>
        /// 加精/解精进入活跃抽奖--20160523姚志光
        /// </summary>
        /// <summary>
        /// 加精/解精
        /// </summary>
        public void UpdateIsGood(int ID, int IsGood)
        {
            dal.UpdateIsGood(ID, IsGood);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子加精";
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
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子加精";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }
        }
        /// <summary>
        /// 推荐/解推荐--进入活跃抽奖20160523姚志光
        /// </summary>
        public void UpdateIsRecom(int ID, int IsRecom)
        {
            dal.UpdateIsRecom(ID, IsRecom);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子推荐";
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
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子推荐";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { } 
        }

        /// <summary>
        /// 置顶/去顶/固底/去底--进入活跃抽奖20160523姚志光
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子置顶/去顶/固底/去底";
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
        /// 锁定/解锁
        /// </summary>
        public void UpdateIsLock(int ID, int IsLock)
        {
            dal.UpdateIsLock(ID, IsLock);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子锁定/解锁";
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
        /// 结束帖子
        /// </summary>
        public void UpdateIsOver(int ID, int IsOver)
        {
            dal.UpdateIsOver(ID, IsOver);
        }

        ///// <summary>
        ///// 设置滚动
        ///// </summary>
        //public void UpdateIsFlow(int ID, int IsFlow)
        //{
        //    dal.UpdateIsFlow(ID, IsFlow);
        //}
        /// <summary>
        /// 设置滚动---进入活跃抽奖20160523姚志光
        /// </summary>
        public void UpdateIsFlow(int ID, int IsFlow)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            dal.UpdateIsFlow(ID, IsFlow);
            try
            {
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子设滚";
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
                int usid = dal.GetUsID(ID);
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "帖子设滚";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }

        }
        /// <summary>
        /// 转移
        /// </summary>
        public void UpdateForumID(int ID, int ForumID)
        {
            dal.UpdateForumID(ID, ForumID);
        }

        /// <summary>
        /// 得到置项类型
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

        /// <summary>
        /// 得到锁定类型
        /// </summary>
        public int GetIsLock(int ID)
        {
            return dal.GetIsLock(ID);
        }

        /// <summary>
        /// 得到精华类型
        /// </summary>
        public int GetIsGood(int ID)
        {
            return dal.GetIsGood(ID);
        }
        /// <summary>
        /// 更行点赞时间
        /// 陈志基  2016 3/28
        /// </summary>
        public void UpdatePraiseTime(int ID, DateTime PraiseTime)
        {
            dal.UpdatePraiseTime(ID, PraiseTime);
        }
        /// <summary>
        /// 得到结帖类型
        /// </summary>
        public int GetIsOver(int ID)
        {
            return dal.GetIsOver(ID);
        }

        /// <summary>
        /// 得到帖子附件数
        /// </summary>
        public int GetFileNum(int ID)
        {
            return dal.GetFileNum(ID);
        }
        //-------------------------------------版主管理-------------------------------------------


        /// <summary>
        /// 更新滚动时间
        /// </summary>
        public void UpdateFlowTime(int ID, DateTime FlowTime, int IsFlow)
        {
            dal.UpdateFlowTime(ID, FlowTime, IsFlow);
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
        /// 清空回收站数据
        /// </summary>
        public void Delete()
        {

            dal.Delete();
        }

        /// <summary>
        /// 得到帖子标题
        /// </summary>
        public string GetTitle(int ID)
        {
            return dal.GetTitle(ID);
        }

        /// <summary>
        /// 得到帖主用户ID
        /// </summary>
        public int GetUsID(int ID)
        {
            return dal.GetUsID(ID);
        }

        /// <summary>
        /// 得到帖子Types
        /// </summary>
        public int GetTypes(int ID)
        {
            return dal.GetTypes(ID);
        }

        /// <summary>
        /// 得到IsFlow
        /// </summary>
        public int GetIsFlow(int ID)
        {
            return dal.GetIsFlow(ID);
        }

        /// <summary>
        /// 得到回复ID
        /// </summary>
        public string GetReplyID(int ID)
        {
            return dal.GetReplyID(ID);
        }
        /// <summary>
        /// 得到点赞ID
        /// </summary>
        public string GetPraiseID(int ID)
        {
            return dal.GetPraiseID(ID);
        }

        /// <summary>
        /// 得到派币帖实体
        /// </summary>
        public BCW.Model.Text GetPriceModel(int ID)
        {

            return dal.GetPriceModel(ID);
        }

        /// <summary>
        /// 随机得到一张社区滚动帖
        /// </summary>
        public BCW.Model.Text GetTextFlow()
        {
            return dal.GetTextFlow();
        }

        /// <summary>
        /// 随机得到某论坛一张滚动帖
        /// </summary>
        public BCW.Model.Text GetTextFlow(int ForumId)
        {

            return dal.GetTextFlow(ForumId);
        }

        /// <summary>
        /// 随机得到N天内的一张推荐或精华帖
        /// </summary>
        public BCW.Model.Text GetTextGoodReCom(int Day)
        {

            return dal.GetTextGoodReCom(Day);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Text GetTextMe(int ID)
        {

            return dal.GetTextMe(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Text GetText(int ID)
        {

            return dal.GetText(ID);
        }
        /// <summary>
        /// 帖子排行分页记录 陈志基 2016/08/10
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats1(int p_pageIndex, int p_pageSize, string strWhere, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats1(p_pageIndex, p_pageSize, strWhere, showtype, out p_recordCount);
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
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTexts(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTexts(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// 取得每页记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsMe(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTextsMe(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }


        #region 论坛点赞排行分页记录 这里开始修改
        /// <summary>
        /// 论坛点赞排行分页记录 陈志基 20160324
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Text> GetForumstats(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, int showtype, out int p_recordCount)
        {
            return dal.GetForumstats(p_pageIndex, p_pageSize, strWhere, strOrder, showtype, out p_recordCount);
        }
        #endregion

        /// <summary>
        /// 取得高手论坛排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList Text</returns>
        public IList<BCW.Model.Text> GetTextsGs(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetTextsGs(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);

        }

        #endregion  成员方法
    }
}
