using System;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using BCW.Model;
using System.Text.RegularExpressions;
namespace BCW.BLL
{
    /// <summary>
    /// 业务逻辑类Action 的摘要说明。
    /// </summary>
    public class Action
    {
        private readonly BCW.DAL.Action dal = new BCW.DAL.Action();
        public Action()
        { }
        #region  成员方法

        #region 游戏Action分类返回
        // BCW.User.AppCase
        public string CaseAction(int Types)
        {
            string text = string.Empty;
            switch (Types)
            {
                #region 未知ID
                //未知 
                case 4:
                case 40:
                    {
                        text = "未知ID:" + Types.ToString();
                    }; break;
                #endregion

                #region 2015旧连接
                case 5:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/guessbc/default.aspx?backurl=" + Utils.PostPage(1)) + "\">竞猜</a>";
                    }; break;
                case 6:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/luck28.aspx?backurl=" + Utils.PostPage(1)) + "\">二八</a>";
                    }; break;
                case 9:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dice.aspx?backurl=" + Utils.PostPage(1)) + "\">挖宝</a>";
                    }; break;
                case 10:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/horse.aspx?backurl=" + Utils.PostPage(1)) + "\">跑马</a>";
                    }; break;
                case 11:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/stkguess.aspx?backurl=" + Utils.PostPage(1)) + "\">上证</a>";
                    }; break;
                case 12:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/bbsshop.aspx?backurl=" + Utils.PostPage(1)) + "\">商城</a>";
                    }; break;
                case 13:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/bigsmall.aspx?backurl=" + Utils.PostPage(1)) + "\">大小</a>";
                    }; break;
                case 14:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/brag.aspx?backurl=" + Utils.PostPage(1)) + "\">吹牛</a>";
                    }; break;
                case 15:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/marry.aspx?backurl=" + Utils.PostPage(1)) + "\">结婚</a>";
                    }; break;
                case 18:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dxdice.aspx?backurl=" + Utils.PostPage(1)) + "\">掷骰</a>";
                    }; break;
                case 19:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/flows.aspx?backurl=" + Utils.PostPage(1)) + "\">拾物</a>";
                    }; break;
                case 20:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/fruit28.aspx?backurl=" + Utils.PostPage(1)) + "\">水果</a>";
                    }; break;
                case 22:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/ssc.aspx?backurl=" + Utils.PostPage(1)) + "\">时时彩</a>";
                    }; break;
                case 23:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/guessbc/default.aspx?backurl=" + Utils.PostPage(1)) + "\">竞猜</a>";
                    }; break;
                #endregion

                #region 2016新游戏连接
                //2016 新游戏
                case 1001:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/xk3.aspx?backurl=" + Utils.PostPage(1)) + "\">新快三</a>";
                    }; break;
                case 1002:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/xk3sw.aspx?backurl=" + Utils.PostPage(1)) + "\">新快三试玩版</a>";
                    }; break;
                case 1003:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hp3.aspx?backurl=" + Utils.PostPage(1)) + "\">快乐扑克三</a>";
                    }; break;
                case 1004:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hp3Sw.aspx?backurl=" + Utils.PostPage(1)) + "\">快乐扑克三试玩版</a>";
                    }; break;
                case 1005:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/cmg.aspx?backurl=" + Utils.PostPage(1)) + "\">捕鱼达人</a>";
                    }; break;
                case 1006:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/klsf.aspx?backurl=" + Utils.PostPage(1)) + "\">快乐十分</a>";
                    }; break;
                case 1007:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/kbyg.aspx?backurl=" + Utils.PostPage(1)) + "\">一元云购</a>";
                    }; break;
                case 1008:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/Dawnlife.aspx?backurl=" + Utils.PostPage(1)) + "\">闯荡人生</a>";
                    }; break;
                case 1009:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/baccarat.aspx?backurl=" + Utils.PostPage(1)) + "\">百家乐</a>";
                    }; break;
                case 1010:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/chatroom.aspx?backurl=" + Utils.PostPage(1)) + "\">红包</a>";
                    }; break;
                case 1011:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/farm.aspx?backurl=" + Utils.PostPage(1)) + "\">酷爆农场</a>";
                    }; break;
                case 1012:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/dzpk.aspx?backurl=" + Utils.PostPage(1)) + "\">德州扑克</a>";
                    }; break;
                case 1013:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/draw.aspx?backurl=" + Utils.PostPage(1)) + "\">抽奖</a>";
                    }; break;
                case 1014:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/jqc.aspx?backurl=" + Utils.PostPage(1)) + "\">进球彩</a>";
                    }; break;
                case 1015:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/hc1.aspx?backurl=" + Utils.PostPage(1)) + "\">好彩一</a>";
                    }; break;
                case 1016:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/sfc.aspx?backurl=" + Utils.PostPage(1)) + "\">胜负彩</a>";
                    }; break;
                case 1017:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/bqc.aspx?backurl=" + Utils.PostPage(1)) + "\">6场半</a>";
                    }; break;
                case 1018:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/winners.aspx?backurl=" + Utils.PostPage(1)) + "\">活跃抽奖</a>";
                    }; break;
                case 1019:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/ssc.aspx?backurl=" + Utils.PostPage(1)) + "\">时时彩</a>";
                    }; break;
                case 1020:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/luck28.aspx?backurl=" + Utils.PostPage(1)) + "\">幸运二八</a>";
                    }; break;
                case 1030:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/PK10.aspx?backurl=" + Utils.PostPage(1)) + "\">北京赛车</a>";
                    }; break;
                #endregion

                #region 2015闲聊ID
                case -2:
                case -1:
                case 25:
                case 26:
                case 30:
                case 31:
                case 33:
                case 998:
                case 999:
                case 2501:
                    {
                        text = "<a href=\"" + Utils.getUrl("/bbs/game/speak.aspx?backurl=" + Utils.PostPage(1)) + "\">闲聊</a>";
                    }; break;
                    #endregion
            }
            return text;
        }
        #endregion

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
        public int Add(BCW.Model.Action model)
        {
            return dal.Add(model);
        }

        /// <summary>
        /// 增加一条数据
        /// 活跃抽奖入口1
        /// </summary>
        public int Add(int UsId, string Notes)
        {
            string UsName = new BCW.BLL.User().GetUsName(UsId);
            int id = dal.Add(0, 0, UsId, UsName, Notes);
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
            //活跃抽奖开关
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {

                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("多人剪刀") || games.Contains("虚拟球类") || games.Contains("球彩") || games.Contains("虚拟") || games.Contains("彩票") || games.Contains("幸运28") || games.Contains("幸运二八") || games.Contains("挖宝") || games.Contains("大小庄") || games.Contains("跑马") || games.Contains("上证指数") || games.Contains("竞拍") || games.Contains("拾物") || games.Contains("时时彩") || games.Contains("吹牛") || games.Contains("捕鱼达人") || games.Contains("闯荡全城") || games.Contains("新快3") || games.Contains("快乐扑克3") || games.Contains("云购") || games.Contains("德州扑克") || games.Contains("农场") || games.Contains("活跃抽奖") || games.Contains("点值抽奖") || games.Contains("百家欢乐") || games.Contains("宠物") || games.Contains("红包群聊") || games.Contains("大小掷骰"))
                    { return id; }
                    else
                    {
                        if (UsId == 0)//会员ID为空返回3
                        {
                            //url=/bbs/uinfo.aspx?uid=" + meid +             
                            Match m;
                            Match m1;
                            string reg = "uid=[0-9]\\d*";
                            string reg1 = "[0-9]\\d*";
                            m = Regex.Match(Notes, reg);
                            m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                            UsId = Convert.ToInt32(m1.Groups[0].ToString());
                            try
                            {
                                if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                                {
                                    return id;
                                }
                            }
                            catch { }
                        }
                        //是否中奖：返回1中将
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
                        return id;
                    }
                }
                catch
                {
                    return id;
                }
            }
            else
            {
                return id;
            }
        }

        /// <summary>
        /// 活跃论坛抽奖入口2_20160518-姚志光
        /// </summary>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <returns></returns>
        public int Add(int UsId, string UsName, string Notes)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏 
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线 
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
            int id = dal.Add(0, 0, UsId, UsName, Notes);
            //活跃抽奖开关
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("多人剪刀") || games.Contains("虚拟球类") || games.Contains("球彩") || games.Contains("虚拟") || games.Contains("彩票") || games.Contains("幸运二八") || games.Contains("幸运28") || games.Contains("挖宝") || games.Contains("大小庄") || games.Contains("跑马") || games.Contains("上证指数") || games.Contains("竞拍") || games.Contains("拾物") || games.Contains("时时彩") || games.Contains("吹牛") || games.Contains("捕鱼达人") || games.Contains("闯荡全城") || games.Contains("新快3") || games.Contains("快乐扑克3") || games.Contains("云购") || games.Contains("德州扑克") || games.Contains("农场") || games.Contains("活跃抽奖") || games.Contains("点值抽奖") || games.Contains("百家欢乐") || games.Contains("宠物") || games.Contains("红包群聊") || games.Contains("大小掷骰"))
                    { return id; }
                    else
                    {
                        if (UsId == 0)//会员ID为空返回3
                        {
                            //url=/bbs/uinfo.aspx?uid=" + meid +             
                            Match m;
                            Match m1;
                            string reg = "uid=[0-9]\\d*";
                            string reg1 = "[0-9]\\d*";
                            m = Regex.Match(Notes, reg);
                            m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                            UsId = Convert.ToInt32(m1.Groups[0].ToString());
                            try
                            {
                                if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                                {
                                    return id;
                                }
                            }
                            catch { }
                        }
                        //是否中奖：返回1中将
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
                        return id;
                    }
                }
                catch
                {
                    return id;
                }
            }
            else
            {
                return id;
            }
        }
        //游戏等插件专用
        /// <summary>
        /// 活跃抽奖游戏类入口3 20160518--姚志光
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="NodeId"></param>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <returns></returns>
        public int Add(int Types, int NodeId, int UsId, string UsName, string Notes)
        {
            string xmlPath = "/Controls/winners.xml";
            string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));//状态1维护2测试0正常
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));//0|停止放送机会|1|开启放送机会
            string WinnersOpenChoose = (ub.GetSub("WinnersOpenChoose", xmlPath));//1全社区2社区3仅游戏
            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", xmlPath));//1发内线2不发内线  
            string ActionText = (ub.GetSub("ActionText", xmlPath));//Action语句
            string ActionOpen = (ub.GetSub("ActionOpen", xmlPath));//Action语句开关
            int id = dal.Add(Types, NodeId, UsId, UsName, Notes);
            //活跃抽奖开关 1维护 
            if (WinnersStatus != "1" && WinnersOpenOrClose == "1")
            {
                try
                {
                    string games = new BCW.winners.winners().getTypesForGameName(Notes);
                    if (games.Contains("多人剪刀") || games.Contains("虚拟球类") || games.Contains("球彩") || games.Contains("虚拟") || games.Contains("彩票") || games.Contains("幸运28") || games.Contains("幸运二八") || games.Contains("挖宝") || games.Contains("大小庄") || games.Contains("跑马") || games.Contains("上证指数") || games.Contains("竞拍") || games.Contains("拾物") || games.Contains("时时彩") || games.Contains("吹牛") || games.Contains("捕鱼达人") || games.Contains("闯荡全城") || games.Contains("新快3") || games.Contains("快乐扑克3") || games.Contains("云购") || games.Contains("德州扑克") || games.Contains("农场") || games.Contains("活跃抽奖") || games.Contains("点值抽奖") || games.Contains("百家欢乐") || games.Contains("宠物") || games.Contains("红包群聊") || games.Contains("大小掷骰"))
                    { return id; }
                    else
                    {
                        if (UsId == 0)//会员ID为空返回3
                        {
                            //url=/bbs/uinfo.aspx?uid=" + meid +             
                            Match m;
                            Match m1;
                            string reg = "uid=[0-9]\\d*";
                            string reg1 = "[0-9]\\d*";
                            m = Regex.Match(Notes, reg);
                            m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                            UsId = Convert.ToInt32(m1.Groups[0].ToString());
                            try
                            {
                                if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                                {
                                    return id;
                                }
                            }
                            catch { }
                        }
                        //是否中奖：返回1中奖
                        int isHit = new BCW.winners.winners().CheckActionForAll(Types, NodeId, UsId, UsName, Notes, id);
                        if (isHit == 1)
                        {
                            if (WinnersGuessOpen == "1")
                            {
                                new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);//发内线到该ID
                                //if (ActionOpen == "1")
                                //{
                                //     Add(UsId,ActionText);
                                //}
                            }
                        }
                        return id;
                    }
                }
                catch
                {
                    return id;
                }
            }
            else
            {
                return id;
            }

        }

        ///
        ///原函数
        ///
        //public int Add(int UsId, string UsName, string Notes)
        //{
        //    return dal.Add(0, 0, UsId, UsName, Notes);
        //}
        ////游戏等插件专用
        //public int Add(int Types, int NodeId, int UsId, string UsName, string Notes)
        //{
        //    return dal.Add(Types, NodeId, UsId, UsName, Notes);
        //}
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public void Update(BCW.Model.Action model)
        {
            dal.Update(model);
        }

        /// <summary>
        /// 删除一组数据
        /// </summary>
        public void Delete(string strWhere)
        {

            dal.Delete(strWhere);
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
        public void Clear()
        {

            dal.Clear();
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public BCW.Model.Action GetAction(int ID)
        {

            return dal.GetAction(ID);
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
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int p_pageIndex, int p_pageSize, string strWhere, out int p_recordCount)
        {
            return dal.GetActions(p_pageIndex, p_pageSize, strWhere, out p_recordCount);
        }

        /// <summary>
        /// 取得固定列表记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActions(int SizeNum, string strWhere)
        {
            return dal.GetActions(SizeNum, strWhere);
        }

        /// <summary>
        /// 取得好友动态记录
        /// </summary>
        /// <param name="SizeNum">列表记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int SizeNum)
        {
            return dal.GetActionsFriend(Types, uid, SizeNum);
        }

        /// <summary>
        /// 取得好友动态记录
        /// </summary>
        /// <param name="p_pageIndex">当前页</param>
        /// <param name="p_pageSize">分页大小</param>
        /// <param name="p_recordCount">返回总记录数</param>
        /// <param name="strWhere">查询条件</param>
        /// <returns>IList Action</returns>
        public IList<BCW.Model.Action> GetActionsFriend(int Types, int uid, int p_pageIndex, int p_pageSize, out int p_recordCount)
        {
            return dal.GetActionsFriend(Types, uid, p_pageIndex, p_pageSize, out p_recordCount);
        }

        #endregion  成员方法
    }
}
