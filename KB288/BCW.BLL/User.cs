using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类User 的摘要说明。
    
    /// 增加已支付字段
    /// 黄国军 20160611
    
    /// 添加活跃抽奖入口201605姚志光
    /// 
    /// 增加点值抽奖入口（酷币消费） 20160823 蒙宗将
   /// </summary>
    public class User
    {
        private readonly BCW.DAL.User dal = new BCW.DAL.User();
        public User()
        { }
        #region  成员方法
        /// <summary>
        /// 得到前台设计中心超时时间
        /// </summary>
        public DateTime GetManAcTime(int ID)
        {
            return dal.GetManAcTime(ID);
        }
        /// <summary>
        /// 更新前台设计中心超时时间
        /// </summary>
        public void UpdateManAcTime(int ID, DateTime ManAcTime)
        {
            dal.UpdateManAcTime(ID, ManAcTime);
        }
         /// <summary>
        /// 更新财富/基金当前支付类型0选择/1财富/2基金
        /// </summary>
        public void UpdatePayType(int ID, int PayType)
        {
            dal.UpdatePayType(ID, PayType);
        }
         /// <summary>
        /// 更新财富/基金当前支付时间
        /// </summary>
        public void UpdateTimeLimit(int ID, DateTime TimeLimit)
        {
            dal.UpdateTimeLimit(ID, TimeLimit);
        }
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
        public bool ExistsID(long ID)
        {
            return dal.ExistsID(ID);
        }
        /// <summary>
        /// 是否存在该手机号记录
        /// </summary>
        public bool Exists(string Mobile)
        {
            return dal.Exists(Mobile);
        }

        /// <summary>
        /// 是否存在该邮箱记录
        /// </summary>
        public bool ExistsEmail(string Email)
        {
            return dal.ExistsEmail(Email);
        }

        /// <summary>
        /// 是否存在该用户昵称记录
        /// </summary>
        public bool ExistsUsName(string UsName)
        {
            return dal.ExistsUsName(UsName);
        }

        /// <summary>
        /// 是否存在该用户昵称记录
        /// </summary>
        public bool ExistsUsName(string UsName, int ID)
        {
            return dal.ExistsUsName(UsName, ID);
        }

        /// <summary>
        /// 是否存在该手机号和邮箱（找回密码）
        /// </summary>
        public bool Exists(string Mobile, string Email)
        {
            return dal.Exists(Mobile, Email);
        }

        /// <summary>
        /// 根据ID和密码查询影响的行数
        /// </summary>
        public int GetRowByID(BCW.Model.User model)
        {
            return dal.GetRowByID(model);
        }
        /// <summary>
        /// 根据手机号和密码查询影响的行数
        /// </summary>
        public int GetRowByMobile(BCW.Model.User model)
        {
            return dal.GetRowByMobile(model);
        }

        /// <summary>
        /// 得到某论坛的在线人数
        /// </summary>
        public int GetForumNum(int ForumID)
        {
            return dal.GetForumNum(ForumID);
        }

        /// <summary>
        /// 得到某圈子的在线人数
        /// </summary>
        public int GetGroupNum(int GroupId)
        {
            return dal.GetGroupNum(GroupId);
        }

        /// <summary>
        /// 得到某聊天室的在线人数
        /// </summary>
        public int GetChatNum(int ChatID)
        {
            return dal.GetChatNum(ChatID);
        }

        /// <summary>
        /// 得到聊天室总在线人数
        /// </summary>
        public int GetChatNum()
        {
            return dal.GetChatNum();
        }

        /// <summary>
        /// 得到闲聊总在线人数
        /// </summary>
        public int GetSpeakNum()
        {
            return dal.GetSpeakNum();
        }

        /// <summary>
        /// 得到在线人数
        /// </summary>
        public int GetNum(int Types)
        {
            return dal.GetNum(Types);
        }

        /// <summary>
        /// 得到会员总数
        /// </summary>
        public int GetCount()
        {
            return dal.GetCount();
        }

        /// <summary>
        /// 得到人气排名
        /// </summary>
        public int GetClickTop(int ID)
        {
            return dal.GetClickTop(ID);
        }

        /// <summary>
        /// 得到是否机器人ID(0否/1是)
        /// </summary>
        public int GetIsSpier(int ID)
        {
            return dal.GetIsSpier(ID);
        }

        /// <summary>
        /// 得到随机一个机器人ID
        /// </summary>
        public int GetIsSpierRandID()
        {
            return dal.GetIsSpierRandID();
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(BCW.Model.User model)
        {
            dal.Add(model);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = model.ID;
                string username = model.UsName; ;
                string Notes = "新注册会员";
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
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.User model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 修改基本资料
        /// </summary>
        public void UpdateEditBasic(BCW.Model.User model)
        {
            dal.UpdateEditBasic(model);
        }

        ///// <summary>
        ///// 更新新消息条数
        ///// </summary>
        //public void UpdateGutNum(int ID)
        //{
        //    dal.UpdateGutNum(ID);
        //}

        ///// <summary>
        ///// 更新新消息条数
        ///// </summary>
        //public void UpdateGutNum(int ID, int GutNum)
        //{
        //    dal.UpdateGutNum(ID, GutNum);
        //}

        /// <summary>
        /// 更新最后时间/IP
        /// </summary>
        public void UpdateIpTime(int ID)
        {
            dal.UpdateIpTime(ID);
        }

        /// <summary>
        /// 更新在线时间
        /// </summary>
        public void UpdateTime(int ID)
        {
            dal.UpdateTime(ID);
        }

        /// <summary>
        /// 更新足迹
        /// </summary>
        public void UpdateVisitHy(int ID, string VisitHy)
        {
            dal.UpdateVisitHy(ID, VisitHy);
        }

        /// <summary>
        /// 更新在线时间
        /// </summary>
        public void UpdateTime(int ID, int OnTime)
        {
            dal.UpdateTime(ID, OnTime);
        }

        /// <summary>
        /// 更新最后在线论坛ID
        /// </summary>
        public void UpdateEndForumID(int ID, int ForumID)
        {
            dal.UpdateEndForumID(ID, ForumID);
        }

        /// <summary>
        /// 更新最后在线聊天室ID
        /// </summary>
        public void UpdateEndChatID(int ID, int ChatID)
        {
            dal.UpdateEndChatID(ID, ChatID);
        }

        /// <summary>
        /// 更新最后在线闲聊ID
        /// </summary>
        public void UpdateEndSpeakID(int ID, int SpeakID)
        {
            dal.UpdateEndSpeakID(ID, SpeakID);
        }

        /// <summary>
        /// 更新昵称
        /// </summary>
        public void UpdateUsName(int ID, string UsName)
        {
            dal.UpdateUsName(ID, UsName);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = UsName;
                string Notes = "空间设置昵称";
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
        /// 更新手机号
        /// </summary>
        public void UpdateMobile(int ID, string Mobile)
        {
            dal.UpdateMobile(ID, Mobile);
        }

        /// <summary>
        /// 更新139提醒邮箱
        /// </summary>
        public void UpdateSmsEmail(int ID, string SmsEmail)
        {
            dal.UpdateSmsEmail(ID, SmsEmail);
        }

        /// <summary>
        /// 更新个性签名
        /// </summary>
        public void UpdateSign(int ID, string Sign)
        {
            dal.UpdateSign(ID, Sign);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = dal.GetUsName(ID);
                string Notes = "空间设置个性签名";
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
        /// 更新登录密码
        /// </summary>
        public void UpdateUsPwd(int ID, string UsPwd)
        {
            dal.UpdateUsPwd(ID, UsPwd);
        }

        /// <summary>
        /// 更新登录密码
        /// </summary>
        public void UpdateUsPwd(string Mobile, string UsPwd)
        {
            dal.UpdateUsPwd(Mobile, UsPwd);
        }

        /// <summary>
        /// 更新用户密匙
        /// </summary>
        public void UpdateUsKey(int ID, string UsKey)
        {
            dal.UpdateUsKey(ID, UsKey);
        }

        /// <summary>
        /// 更新支付密码
        /// </summary>
        public void UpdateUsPled(int ID, string UsPled)
        {
            dal.UpdateUsPled(ID, UsPled);
        }

        /// <summary>
        /// 更新支付密码
        /// </summary>
        public void UpdateUsPled(string Mobile, string UsPled)
        {
            dal.UpdateUsPled(Mobile, UsPled);
        }

        /// <summary>
        /// 更新管理密码
        /// </summary>
        public void UpdateUsAdmin(int ID, string UsAdmin)
        {
            dal.UpdateUsAdmin(ID, UsAdmin);
        }

        /// <summary>
        /// 更新登录密码
        /// </summary>
        public void UpdatePhoto(int ID, string Photo)
        {
            dal.UpdatePhoto(ID, Photo);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public void UpdateState(int ID, int State)
        {
            dal.UpdateState(ID, State);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = dal.GetUsName(ID);
                string Notes = "空间设置更新状态";
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
        /// 更新人气
        /// </summary>
        public void UpdateClick(int ID, int Click)
        {
            dal.UpdateClick(ID, Click);
        }

        /// <summary>
        /// 更新升级等级
        /// </summary>
        public void UpdateLeven(int ID, int Leven, long iScore)
        {
            dal.UpdateLeven(ID, Leven, iScore);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "升级";
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
        /// 更新VIP信息
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate)
        {
            dal.UpdateVipData(ID, VipDayGrow, VipDate); 
             try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid =ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "VIP";
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
        /// 更新VIP信息
        /// 购买vip加入到活跃抽奖入口--姚志光
        /// </summary>
        public void UpdateVipData(int ID, int VipDayGrow, DateTime VipDate, int VipGrow)
        {
            dal.UpdateVipData(ID, VipDayGrow, VipDate, VipGrow);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "VIP";
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
        /// 更新VIP成长点
        /// </summary>
        public void UpdateVipGrow(int ID, int VipGrow)
        {
            dal.UpdateVipGrow(ID, VipGrow);
        }

        /// <summary>
        /// 更新属性
        /// </summary>
        public void UpdateParas(int ID, string Paras)
        {
            dal.UpdateParas(ID, Paras);
        }

        /// <summary>
        /// 更新个性设置
        /// </summary>
        public void UpdateForumSet(int ID, string ForumSet)
        {
            dal.UpdateForumSet(ID, ForumSet);
        }

        /// <summary>
        /// 更新复制历史
        /// </summary>
        public void UpdateCopytemp(int ID, string Copytemp)
        {
            dal.UpdateCopytemp(ID, Copytemp);
        }

        /// <summary>
        /// 更新圈子ID
        /// </summary>
        public void UpdateGroupId(int ID, string GroupId)
        {
            dal.UpdateGroupId(ID, GroupId);
        }

        /// <summary>
        /// 更新验证码
        /// </summary>
        public void UpdateVerifys(int ID, string Verifys)
        {
            dal.UpdateVerifys(ID, Verifys);
        }


        /// <summary>
        /// 更新推荐人ID
        /// </summary>
        public void UpdateInviteNum(int id, int InviteNum)
        {
            dal.UpdateInviteNum(id, InviteNum);
        }
        /// <summary>
        /// 更新为验证会员
        /// </summary>
        public void UpdateIsVerify(string Mobile, int IsVerify)
        {
            dal.UpdateIsVerify(Mobile, IsVerify);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = dal.GetID(Mobile);
                string username = dal.GetUsName(usid); ;
                string Notes = "验证";
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
        /// 冻结会员ID
        /// </summary>
        public void UpdateIsFreeze(int ID, int IsFreeze)
        {
            dal.UpdateIsFreeze(ID, IsFreeze);
        }

        /// <summary>
        /// 更新签到信息
        /// </summary>
        public void UpdateSingData(int ID, int SignTotal, int SignKeep)
        {
            dal.UpdateSingData(ID, SignTotal, SignKeep);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username = dal.GetUsName(ID); ;
                string Notes = "空间签到";
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
        /// 更新权限
        /// </summary>
        public void UpdateLimit(int ID, string Limit)
        {
            dal.UpdateLimit(ID, Limit);
        }

        /// <summary>
        /// 更新UsUbb
        /// </summary>
        public void UpdateUsUbb(int ID, string UsUbb)
        {
            dal.UpdateUsUbb(ID, UsUbb);
        }

        /// <summary>
        /// 更新用户积分
        /// </summary>
        public void UpdateiScore(int ID, long iScore)
        {
            dal.UpdateiScore(ID, iScore);
        }

        /// <summary>
        /// 更新推广拥金
        /// </summary>
        public void UpdateiFcGold(int ID, long iFcGold)
        {
            dal.UpdateiFcGold(ID, iFcGold);
            try
            {
                string xmlPath = "/Controls/winners.xml";
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
                string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
                string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
                string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
                int usid = ID;
                string username =dal.GetUsName(ID);
                string Notes = "推广";
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
        /// 删除一条数据
        /// </summary>
        public void Delete(int ID)
        {

            dal.Delete(ID);
        }

        /// <summary>
        /// 得到ID
        /// </summary>
        public int GetID(string Mobile)
        {
            return dal.GetID(Mobile);
        }

        /// <summary>
        /// 得到手机号
        /// </summary>
        public string GetMobile(int ID)
        {
            return dal.GetMobile(ID);
        }

        /// <summary>
        /// 得到登录密码
        /// </summary>
        public string GetUsPwd(int ID)
        {
            return dal.GetUsPwd(ID);
        }
        /// <summary>
        /// 得到登录密码
        /// </summary>
        public string GetUsPwd(int ID, string Mobile)
        {
            return dal.GetUsPwd(ID, Mobile);
        }


        /// <summary>
        /// 得到最后IP
        /// </summary>
        public BCW.Model.User GetEndIpTime(int ID)
        {
            return dal.GetEndIpTime(ID);
        }

        /// <summary>
        /// 得到支付密码
        /// </summary>
        public string GetUsPled(int ID)
        {
            return dal.GetUsPled(ID);
        }

        /// <summary>
        /// 得到支付密码
        /// </summary>
        public string GetUsPled(int ID, string Mobile)
        {
            return dal.GetUsPled(ID, Mobile);
        }

        /// <summary>
        /// 得到管理密码
        /// </summary>
        public string GetUsAdmin(int ID)
        {
            return dal.GetUsAdmin(ID);
        }

        /// <summary>
        /// 得到属性
        /// </summary>
        public string GetParas(int ID)
        {
            return dal.GetParas(ID);
        }

        /// <summary>
        /// 得到个性设置
        /// </summary>
        public string GetForumSet(int ID)
        {
            return dal.GetForumSet(ID);
        }

        /// <summary>
        /// 得到139手机邮箱
        /// </summary>
        public string GetSmsEmail(int ID)
        {
            return dal.GetSmsEmail(ID);
        }

        /// <summary>
        /// 得到社区UBB身份
        /// </summary>
        public string GetUsUbb(int ID)
        {
            return dal.GetUsUbb(ID);
        }

        /// <summary>
        /// 得到复制历史
        /// </summary>
        public string GetCopytemp(int ID)
        {
            return dal.GetCopytemp(ID);
        }

        /// <summary>
        /// 得到足迹
        /// </summary>
        public string GetVisitHy(int ID)
        {
            return dal.GetVisitHy(ID);
        }

        /// <summary>
        /// 得到加入的圈子ID
        /// </summary>
        public string GetGroupId(int ID)
        {
            return dal.GetGroupId(ID);
        }

        /// <summary>
        /// 得到验证码
        /// </summary>
        public string GetVerifys(int ID)
        {
            return dal.GetVerifys(ID);
        }

        /// <summary>
        /// 得到头像
        /// </summary>
        public string GetPhoto(int ID)
        {
            return dal.GetPhoto(ID);
        }

        ///// <summary>
        ///// 得到新消息条数
        ///// </summary>
        //public int GetGutNum(int ID)
        //{
        //    return dal.GetGutNum(ID);
        //}

        /// <summary>
        /// 得到推荐自己的ID
        /// </summary>
        public int GetInviteNum(int ID)
        {
            return dal.GetInviteNum(ID);
        }

        /// <summary>
        /// 得到推广拥金
        /// </summary>
        public long GetFcGold(int ID)
        {
            return dal.GetFcGold(ID);
        }

        /// <summary>
        /// 得到权限
        /// </summary>
        public string GetLimit(int ID)
        {
            return dal.GetLimit(ID);
        }

        /// <summary>
        /// 得到是否已验证(0未验证/1已验证)
        /// </summary>
        public int GetIsVerify(int ID)
        {
            return dal.GetIsVerify(ID);
        }

        /// <summary>
        /// 得到帐户是否已冻结
        /// </summary>
        public int GetIsFreeze(int ID)
        {
            return dal.GetIsFreeze(ID);
        }

        /// <summary>
        /// 得到权限实体，从缓存中。
        /// </summary>
        public string GetLimitByCache(int ID)
        {
            string CacheKey = CacheName.App_LimitModel(ID);
            object objModel = DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = dal.GetLimit(ID);
                    if (objModel != null)
                    {
                        int ModelCache = CacheTime.LimitExpir;
                        DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return objModel.ToString();
        }

        /// <summary>
        /// 得到用户币
        /// </summary>
        public long GetGold(int ID)
        {
            return dal.GetGold(ID);
        }
         /// <summary>
        /// 得到打赏支付的时间
        /// </summary>
        public BCW.Model.User GetTimeLimit(int ID)
        {
           return  dal.GetTimeLimit(ID);
        }
         /// <summary>
        /// 得到用户支付方法 1财富  2基金
        /// </summary>
        public int GetPayType(int ID)
        {
            return dal.GetPayType(ID);
        }
        /// <summary>
        /// 得到用户元
        /// </summary>
        public long GetMoney(int ID)
        {
            return dal.GetMoney(ID);
        }

        /// <summary>
        /// 得到用户银行币
        /// </summary>
        public long GetBank(int ID)
        {
            return dal.GetBank(ID);
        }

        /// <summary>
        /// 得到用户等级
        /// </summary>
        public int GetLeven(int ID)
        {
            return dal.GetLeven(ID);
        }

        /// <summary>
        /// 得到用户昵称
        /// </summary>
        public string GetUsName(int ID)
        {
            return dal.GetUsName(ID);
        }

        /// <summary>
        /// 得到账户已支付金额
        /// </summary>
        public string GetUsISGive(int ID)
        {
            return dal.GetUsISGive(ID);
        }

        /// <summary>
        /// 更新账户已支付金额
        /// </summary>
        public void SetUsISGive(int ID, double ISGive)
        {
            dal.SetUsISGive(ID, ISGive);
        }

        /// <summary>
        /// 得到用户签名
        /// </summary>
        public string GetSign(int ID)
        {
            return dal.GetSign(ID);
        }

        /// <summary>
        /// 得到用户状态
        /// </summary>
        public int GetState(int ID)
        {
            return dal.GetState(ID);
        }

        /// <summary>
        /// 得到用户性别
        /// </summary>
        public int GetSex(int ID)
        {
            return dal.GetSex(ID);
        }

        /// <summary>
        /// 得到最后论坛ID
        /// </summary>
        public int GetEndForumID(int ID)
        {
            return dal.GetEndForumID(ID);
        }

        /// <summary>
        /// 得到最后聊天室ID
        /// </summary>
        public int GetEndChatID(int ID)
        {
            return dal.GetEndChatID(ID);
        }

        /// <summary>
        /// 得到最后闲聊ID
        /// </summary>
        public int GetEndSpeakID(int ID)
        {
            return dal.GetEndSpeakID(ID);
        }

        /// <summary>
        /// 得到签到信息
        /// </summary>
        public BCW.Model.User GetSignData(int ID)
        {
            return dal.GetSignData(ID);
        }

        /// <summary>
        /// 得到VIP信息
        /// </summary>
        public BCW.Model.User GetVipData(int ID)
        {
            return dal.GetVipData(ID);
        }

        /// <summary>
        /// 得到构造用户昵称显示的标识
        /// </summary>
        public BCW.Model.User GetShowName(int ID)
        {
            return dal.GetShowName(ID);
        }

        /// <summary>
        /// 得到用户基本信息
        /// </summary>
        public BCW.Model.User GetBasic(int ID)
        {
            return dal.GetBasic(ID);
        }

        /// <summary>
        /// 得到修改的基本信息
        /// </summary>
        public BCW.Model.User GetEditBasic(int ID)
        {
            return dal.GetEditBasic(ID);
        }

        /// <summary>
        /// 得到找回密码的基本信息
        /// </summary>
        public BCW.Model.User GetPwdBasic(string Mobile)
        {
            return dal.GetPwdBasic(Mobile);
        }

        /// <summary>
        /// 得到Uskey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(int ID)
        {
            return dal.GetKey(ID);
        }
        /// <summary>
        /// 得到Uskey/UsPwd
        /// </summary>
        public BCW.Model.User GetKey(string Mobile)
        {
            return dal.GetKey(Mobile);
        }

        /// <summary>
        /// 得到在线的基本信息
        /// </summary>
        public BCW.Model.User GetOnlineBasic(int ID)
        {
            return dal.GetOnlineBasic(ID);
        }
        //------------------------------金融-------------------------------------
        /// <summary>
        /// 更新用户虚拟币
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="iGold">操作币</param>
        public void UpdateiGold(int ID, long iGold)
        {
            dal.UpdateiGold(ID, iGold);
        }
        /// <summary>
        /// 更新用户银行币
        /// </summary>
        public void UpdateiBank(int ID, long iBank)
        {
            dal.UpdateiBank(ID, iBank);
        }

        /// <summary>
        /// 更新用户虚拟元
        /// </summary>
        public void UpdateiMoney(int ID, long iMoney)
        {
            dal.UpdateiMoney(ID, iMoney);
        }
        //------------------------------金融-------------------------------------
        /// <summary>
        /// 更新用户虚拟币/更新消费记录
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="UsName">用户昵称</param>
        /// <param name="iGold">操作币</param>
        /// <param name="AcText">说明</param>
        public void UpdateiGold(int ID, long iGold, string AcText)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, AcText);
        }

        public void UpdateiGold(int ID, string UsName, long iGold, string AcText)
        {
            //更新用户虚拟币
            dal.UpdateiGold(ID, iGold);
            //更新消费记录
            BCW.Model.Goldlog model = new BCW.Model.Goldlog();
            model.BbTag = 0;
            model.Types = 0;
            model.PUrl = Utils.getPageUrl();//操作的文件名
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iGold;
            model.AfterGold = GetGold(ID);//更新后的币数
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(model);

            #region 点值抽奖接口 16/08/15
            try
            {
                new BCW.Draw.draw().AddjfbyiGold(ID, UsName, iGold, AcText);
            }
            catch { }
            #endregion
        }

        public void UpdateiGold(int ID, long iGold, string AcText, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, AcText, Types);
        }

        public void UpdateiGold(int ID, string UsName, long iGold, string AcText, int Types)
        {
            UpdateiGold(ID, UsName, iGold, AcText);
            //更新排行榜
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        /// <summary>
        ///  根据字段修改数据列表 邵广林 20161107 
        /// </summary>
        public DataSet update_ziduan(string strField, string strWhere)
        {
            return dal.update_ziduan(strField, strWhere);
        }


        /// <summary>
        /// 更新用户虚拟币/更新排行榜/更新消费记录
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="UsName">用户昵称</param>
        /// <param name="iGold">操作币</param>
        /// <param name="Types">排行榜类型</param>
        public void UpdateiGold(int ID, long iGold, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGold(ID, UsName, iGold, Types);
        }
        public void UpdateiGold(int ID, string UsName, long iGold, int Types)
        {
            //更新用户虚拟币
            string AcText = string.Empty;
            if (Types == 1)
                AcText = "多人石头游戏消费";
            else if (Types == 2)
                AcText = "789游戏消费";
            else if (Types == 3)
                AcText = "猜猜乐游戏消费";
            else if (Types == 4)
                AcText = "幸运28游戏消费";
            else if (Types == 5)
                AcText = "虚拟投注游戏消费";
            else if (Types == 6)
                AcText = "疯狂彩球消费";
            else if (Types == 7)
                AcText = "疯狂吹牛消费";
            else if (Types == 8)
                AcText = "猜拳消费";
            else if (Types == 9)
                AcText = "大小庄消费";
            else if (Types == 10)
                AcText = "掷骰消费";
            else if (Types == 11)
                AcText = "时时彩消费";
            else if (Types == 22)
                AcText = "新快3消费";
            else if (Types == 25)
                AcText = "快乐扑克3消费";
            else if (Types == 26)
                AcText = "捕鱼消费";

            UpdateiGold(ID, UsName, iGold, AcText);
            //更新排行榜
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        #region 大小庄用更新金币 UpdateiGold
        /// <summary>
        /// 大小庄用更新数据
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="UsName"></param>
        /// <param name="iGold"></param>
        /// <param name="Types"></param>
        /// <param name="AcText"></param>
        public void UpdateiGold(int ID, string UsName, long iGold, int Types, string AcText)
        {
            //更新用户虚拟币
            UpdateiGold(ID, UsName, iGold, AcText);
            //更新排行榜
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }
        #endregion

        /// <summary>
        /// 更新更新排行榜
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="UsName">用户昵称</param>
        /// <param name="iGold">操作币</param>
        /// <param name="Types">排行榜类型</param>
        public void UpdateiGoldTop(int ID, long iGold, int Types)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiGoldTop(ID, UsName, iGold, Types);
        }
        public void UpdateiGoldTop(int ID, string UsName, long iGold, int Types)
        {
            //更新排行榜
            BCW.Model.Toplist model = new BCW.Model.Toplist();
            model.Types = Types;
            model.UsId = ID;
            model.UsName = UsName;
            if (iGold > 0)
            {
                model.WinNum = 1;
                model.WinGold = iGold;
            }
            else
            {
                model.PutNum = 1;
                model.PutGold = iGold;
            }
            if (!new Toplist().Exists(ID, Types))
                new Toplist().Add(model);
            else
                new Toplist().Update(model);
        }

        /// <summary>
        /// 更新用户虚拟元/更新消费记录
        /// </summary>
        /// <param name="ID">用户ID</param>
        /// <param name="UsName">用户昵称</param>
        /// <param name="iGold">操作币</param>
        /// <param name="AcText">说明</param>
        public void UpdateiMoney(int ID, long iMoney, string AcText)
        {
            string UsName = dal.GetUsName(ID);
            UpdateiMoney(ID, UsName, iMoney, AcText);
        }

        public void UpdateiMoney(int ID, string UsName, long iMoney, string AcText)
        {
            //更新用户虚拟币
            dal.UpdateiMoney(ID, iMoney);
            //更新消费记录
            BCW.Model.Goldlog model = new BCW.Model.Goldlog();
            model.BbTag = 0;
            model.Types = 1;
            model.PUrl = Utils.getPageUrl();//操作的文件名
            model.UsId = ID;
            model.UsName = UsName;
            model.AcGold = iMoney;
            model.AfterGold = GetMoney(ID);//更新后的元数
            model.AcText = AcText;
            model.AddTime = DateTime.Now;
            new BCW.BLL.Goldlog().Add(model);
        }

        /// <summary>
        /// 根据字段取数据列表
        /// </summary>
        public DataSet GetList(string strField, string strWhere)
        {
            return dal.GetList(strField, strWhere);
        }

        /// <summary>
        /// 取得每页记录（后台列表使用）
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排列方式</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsersManage(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetUsersManage(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// 取得每页记录（搜索/在线页面使用）
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 会员排行榜使用
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <returns>IList User</returns>
        public IList<BCW.Model.User> GetUsers(int p_pageIndex, int p_pageSize, string strWhere, string strOrder, out int p_recordCount)
        {
            return dal.GetUsers(p_pageIndex, p_pageSize, strWhere, strOrder, out p_recordCount);
        }

        /// <summary>
        /// 推荐会员排行榜
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">每页显示记录数</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>List</returns>
        public IList<BCW.Model.User> GetInvites(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetInvites(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        #endregion  成员方法
    }
}
