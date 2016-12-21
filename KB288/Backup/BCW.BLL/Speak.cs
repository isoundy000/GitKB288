using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Speak 的摘要说明。
    /// 
    /// 增加点值抽奖闲聊入口 蒙宗将 20160823 
    /// </summary>
    public class Speak
    {
        private readonly BCW.DAL.Speak dal = new BCW.DAL.Speak();
        public Speak()
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
        /// 计算总闲聊数
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// 计算某会员ID发表的闲聊数
        /// </summary>
        public int GetCount(int UsId)
        {
            return dal.GetCount(UsId);
        }
               
        /// <summary>
        /// 计算某时间段的闲聊发表数
        /// </summary>
        public int GetCount(DateTime dt)
        {
            return dal.GetCount(dt);
        }
                
        /// <summary>
        /// 根据条件计算闲聊发表数
        /// </summary>
        public int GetCount(string strWhere)
        {
            return dal.GetCount(strWhere);
        }

        /// <summary>
        /// 增加一条数据
        /// 闲聊增加活跃抽奖入口
        /// 20160602姚志光
        /// </summary>
        public int Add(BCW.Model.Speak model)
        {
            int ID= dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = model.UsId;
                string username = model.UsName;
                string Notes = "闲聊";
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

            try
            {
                int usid = model.UsId;
                string username = new BCW.BLL.User().GetUsName(usid);
                string Notes = "闲聊发言";
                new BCW.Draw.draw().AddjfbyTz(usid, username, Notes);//点值抽奖
            }
            catch { }
            return ID;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Speak model)
        {
            dal.Update(model);
        }
                
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void UpdateIsTop(int ID, int IsTop)
        {
            dal.UpdateIsTop(ID, IsTop);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }
        /// <summary>
        /// 清空一组数据
        /// </summary>
        public void Clear(int Types)
        {

            dal.Clear(Types);
        }

        /// <summary>
        /// 清空一组数据
        /// </summary>
        public void Clear(int Types, int NodeId)
        {

            dal.Clear(Types, NodeId);
        }

        /// <summary>
        /// 清空全部数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Speak GetSpeak(int ID)
        {

            return dal.GetSpeak(ID);
        }

        /// <summary>
        /// 得到一个IsTop
        /// </summary>
        public int GetIsTop(int ID)
        {
            return dal.GetIsTop(ID);
        }

        /// <summary>
        /// 得到会员上一个发言内容和时间
        /// </summary>
        public BCW.Model.Speak GetNotesAddTime(int UsId)
        {

            return dal.GetNotesAddTime(UsId);
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
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaks(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Speak</returns>
        public IList<BCW.Model.Speak> GetSpeaks(int SizeNum, string strWhere)
        {
            return dal.GetSpeaks(SizeNum, strWhere);
        }
                
        /// <summary>
        /// 闲聊排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaksTop(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 闲聊排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.Speak> GetSpeaksTop2(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetSpeaksTop2(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }
        #endregion  成员方法
    }
}

