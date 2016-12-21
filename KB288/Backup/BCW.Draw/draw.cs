using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using BCW.Common;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace BCW.Draw
{
    public class draw
    {
        protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
        protected string GameName = Convert.ToString(ub.GetSub("deawName", "/Controls/draw.xml"));
        protected int statue = Convert.ToInt32(ub.GetSub("drawStatus", "/Controls/draw.xml"));
        protected string xmlPath = "/Controls/draw.xml";

        /// <summary>
        /// //返回判断记录属于的名称
        /// 游戏的属性
        /// 返回各动作的地址
        /// </summary>
        //返回游戏社区链接
        public string getTypesForGameName(string notes)
        {
            string str = "";
            if (notes.Contains("多人剪刀"))
            { str = "在[URL=/bbs/game/kbyg.aspx]多人剪刀[/URL]"; }
            if (notes.Contains("Ktv789"))
            { str = "在[URL=/bbs/game/ktv789.aspx]Ktv789[/URL]"; }
            if (notes.Contains("猜猜乐"))
            { str = "在[URL=/bbs/game/ccl.aspx]猜猜乐[/URL]"; }
            if (notes.Contains("大小庄"))
            { str = "在[URL=/bbs/game/bigsmall.aspx]大小庄家[/URL]"; }
            if (notes.Contains("欢乐竞拍"))
            { str = "在[URL=/bbs/game/race.aspx]欢乐竞拍[/URL]"; }
            if (notes.Contains("竞猜"))
            { str = "在[URL=/bbs/game/race.aspx]竞猜[/URL]"; }
            if (notes.Contains("幸运28"))
            { str = "在[URL=/bbs/game/luck28.aspx]幸运二八[/URL]"; }
            if (notes.Contains("虚拟彩票"))
            { str = "在[URL=/bbs/game/six49.aspx]虚拟彩票[/URL]"; }
            if (notes.Contains("挖宝"))
            { str = "在[URL=/bbs/game/dice.aspx]点数挖宝[/URL]"; }
            if (notes.Contains("跑马"))
            { str = "在[URL=/bbs/game/horse.aspx]跑马[/URL]"; }
            if (notes.Contains("上证"))
            { str = "在[URL=/bbs/game/stkguess.aspx]上证指数[/URL]"; }
            if (notes.Contains("吹牛"))
            { str = "在[URL=/bbs/game/brag.aspx]吹牛[/URL]"; }
            if (notes.Contains("猜拳"))
            { str = "在[URL=/bbs/game/caiqquan.aspx]猜拳[/URL]"; }
            if (notes.Contains("苹果机"))
            { str = "在[URL=/bbs/game/pgj.aspx]苹果机[/URL]"; }
            if (notes.Contains("掷骰"))
            { str = "在[URL=/bbs/game/dxdice.aspx]大小掷骰[/URL]"; }
            if (notes.Contains("拾物"))
            { str = "在[URL=/bbs/game/flows.aspx]拾物[/URL]"; }
            if (notes.Contains("直播"))
            { str = "在[URL=/bbs/guess2/live.aspx]直播[/URL]"; }
            if (notes.Contains("时时彩"))
            { str = "在[URL=/bbs/game/ssc.aspx]时时彩票[/URL]"; }
            if (notes.Contains("拾物"))
            { str = "在[URL=/bbs/game/flows.aspx]拾物[/URL]"; }
            if (notes.Contains("活动礼品"))
            { str = "在[URL=/bbs/game/flows.aspx]活动礼品[/URL]"; }
            if (notes.Contains("百花谷"))
            { str = "在[URL=/bbs/game/race.aspx]百花谷[/URL]"; }
            if (notes.Contains("新快3"))
            { str = "在[URL=/bbs/game/xk3.aspx]新快3[/URL]"; }
            if (notes.Contains("彩票"))
            { str = "在[URL=/bbs/game/six49.aspx]虚拟彩票[/URL]"; }
            if (notes.Contains("球彩"))
            { str = "在[URL=/bbs/guess2/default.aspx]虚拟球类[/URL]"; }
            if (notes.Contains("捕鱼"))
            { str = "在[URL=/bbs/game/cmg.aspx]捕鱼达人[/URL]"; }
            if (notes.Contains("快乐十分"))
            { str = "在[URL=/bbs/game/klsf.aspx]快乐十分[/URL]"; }
            if (notes.Contains("大小掷骰"))
            { str = "在[URL=/bbs/game/dxdice.aspx]大小掷骰[/URL]"; }
            if (notes.Contains("闯荡全城"))
            { str = "在[URL=/bbs/game/Dawnlife.aspx]闯荡全城[/URL]"; }
            if (notes.Contains("点值"))
            { str = "在[URL=/bbs/game/Draw.aspx]点值抽奖[/URL]"; }
            if (notes.Contains("德州扑克"))
            { str = "在[URL=/bbs/game/dzpk.aspx]德州扑克[/URL]"; }
            if (notes.Contains("快乐扑克3"))
            { str = "在[URL=/bbs/game/hp3.aspx]快乐扑克3[/URL]"; }
            if (notes.Contains("云购"))
            { str = "在[URL=/bbs/game/kbyg.aspx]酷币云购[/URL]"; }
            if (notes.Contains("红包群聊"))
            { str = "在[URL=/bbs/game/chatroom.aspx]红包群聊[/URL]"; }
            if (notes.Contains("论坛功能"))
            { str = "在[URL=/bbs/forum.aspx]论坛功能[/URL]"; }
            if (notes.Contains("球彩"))
            { str = "在[URL=/bbs/guess2/default.aspx]虚拟球类[/URL]"; }
            //if (notes.Contains("宠物"))
            //{ str = "在[URL=/bbs/game/kbyg.aspx]宠物[/URL]"; }
            if (notes.Contains("百家欢乐"))
            { str = "在[URL=/bbs/game/baccarat.aspx]百家欢乐[/URL]"; }
            if (notes.Contains("虚拟彩票"))
            { str = "在[URL=/bbs/game/six49.aspx]虚拟彩票[/URL]"; }
            if (notes.Contains("竞拍"))
            { str = "在[URL=/bbs/game/race.aspx]欢乐竞拍[/URL]"; }
            if (notes.Contains("农场"))
            { str = "在[URL=/bbs/game/farm.aspx]开心农场[/URL]"; }
            //// ------ 
            if (notes.Contains("加精"))
            { str = "在[URL=/bbs/forum.aspx]加精[/URL]"; }
            if (notes.Contains("帖子加精"))
            { str = "在[URL=/bbs/forum.aspx]帖子加精[/URL]"; }
            if (notes.Contains("帖子设滚"))
            { str = "在[URL=/bbs/forum.aspx]帖子设滚[/URL]"; }
            if (notes.Contains("帖子币"))
            { str = "在[URL=/bbs/forum.aspx]派币帖子[/URL]"; }
            if (notes.Contains("帖子打赏"))
            { str = "在[URL=/bbs/forum.aspx]帖子打赏[/URL]"; }
            if (notes.Contains("帖子推荐"))
            { str = "在[URL=/bbs/forum.aspx]帖子推荐[/URL]"; }
            if (notes.Contains("帖子推荐"))
            { str = "在[URL=/bbs/forum.aspx]帖子推荐[/URL]"; }
            if (notes.Contains("签到"))
            { str = "在[URL=/bbs/signin.aspx]签到[/URL]"; }
            if (notes.Contains("获得") && notes.Contains("flows.aspx"))
            { str = "在[URL=/bbs/game/flows.aspx]拾物[/URL]"; }
            if (notes.Contains("回复帖子"))
            { str = "在[URL=/bbs/forum.aspx]回复帖子[/URL]"; }
            if (notes.Contains("发表"))
            { str = "在[URL=/bbs/addThread.aspx]发表帖子[/URL]"; }
            if (notes.Contains("游戏竞技"))
            { str = "在[URL=/bbs/forum.aspx]游戏竞技[/URL]"; }
            if (notes.Contains("新注册会员"))
            { str = "在[URL=/default.aspx]新注册会员[/URL]"; }
            if (notes.Contains("赠送礼物"))
            { str = "在[URL=/bbs/bbsshop.aspx]赠送礼物[/URL]"; }
            if (notes.Contains("过户币"))
            { str = "在[URL=/bbs/finance.aspx]过户币[/URL]"; }
            if (notes.Contains("帖子打赏"))
            { str = "在[URL=/bbs/forum.aspx]帖子打赏[/URL]"; }
            if (notes.Contains("发喇叭"))
            { str = "在[URL=/bbs/network.aspx]发喇叭[/URL]"; }
            if (notes.Contains("结婚"))
            { str = "在[URL=/bbs/marry.aspx]结婚[/URL]"; }
            if (notes.Contains("种花"))
            { str = "在[URL=/bbs/marry.aspx]种花[/URL]"; }
            if (notes.Contains("购买靓号"))
            { str = "在[URL=/bbs/spaceapp/sellnum.aspx]购买靓号[/URL]"; }
            if (notes.Contains("上传照片"))
            { str = "在[URL=/bbs/albums.aspx]上传照片[/URL]"; }
            if (notes.Contains("购买商城礼物"))
            { str = "在[URL=/bbs/bbsshop.aspx]购买商城礼物[/URL]"; }
            if (notes.Contains("发内线"))
            { str = "在[URL=/bbs/guest.aspx]发内线[/URL]"; }
            if (notes.Contains("加好友"))
            { str = "在[URL=/bbs/friend.aspx]加好友[/URL]"; }
            if (notes.Contains("上传书"))
            { str = "在[URL=/kb288book/bookman.aspx]上传书[/URL]"; }
            if (notes.Contains("书评"))
            { str = "在[URL=/kb288book/default.aspx]书评[/URL]"; }
            if (notes.Contains("获授勋章"))
            { str = "在[URL=/bbs/bbsshop.aspx]获授勋章[/URL]"; }
            if (notes.Contains("赠送"))
            { str = "在[URL=/bbs/bbsshop.aspx]赠送礼物[/URL]"; }
            if (notes.Contains("空间设置"))
            { str = "在[URL=/bbs/myedit.aspx]空间设置[/URL]"; }
            if (notes.Contains("空间签到"))
            { str = "在[URL=/bbs/signin.aspx]空间签到[/URL]"; }
            if (notes.Contains("空间连续一周签到"))
            { str = "在[URL=/bbs/signin.aspx]空间连续一周签到[/URL]"; }
            if (notes.Contains("闲聊"))
            { str = "在[URL=/bbs/game/speak.aspx]闲聊[/URL]"; }
            if (notes.Contains("聊吧"))
            { str = "在[URL=/bbs/chat.aspx]聊吧[/URL]"; }
            if (notes.Contains("酷友热聊室"))
            { str = "在[URL=/bbs/chatroom.aspx]酷友热聊室[/URL]"; }
            if (notes.Contains("红包"))
            { str = "在[URL=/bbs/game/speak.aspx]红包[/URL]"; }
            if (notes.Contains("活跃抽奖"))
            { str = "在[URL=/bbs/game/winners.aspx]活跃抽奖[/URL]"; }
            return str;
        }
        //返回游戏社区名字
        public string getTypesForGameName2(string notes)
        {
            string str = "";
            if (notes.Contains("多人剪刀"))
            { str = "多人剪刀"; }
            if (notes.Contains("Ktv789"))
            { str = "Ktv789"; }
            if (notes.Contains("猜猜乐"))
            { str = "猜猜乐"; }
            if (notes.Contains("大小庄"))
            { str = "大小庄家"; }
            if (notes.Contains("掷骰"))
            { str = "大小掷骰"; }
            if (notes.Contains("欢乐竞拍"))
            { str = "欢乐竞拍"; }
            if (notes.Contains("竞猜"))
            { str = "竞猜"; }
            if (notes.Contains("幸运28") || notes.Contains("二八"))
            { str = "幸运二八"; }
            if (notes.Contains("虚拟投注"))
            { str = "虚拟投注"; }
            if (notes.Contains("挖宝"))
            { str = "点数挖宝"; }
            if (notes.Contains("跑马"))
            { str = "跑马风云"; }
            if (notes.Contains("上证"))
            { str = "上证指数"; }
            if (notes.Contains("吹牛"))
            { str = "吹牛游戏"; }
            if (notes.Contains("猜拳"))
            { str = "猜拳"; }
            if (notes.Contains("苹果机"))
            { str = "苹果机"; }
            if (notes.Contains("拾物"))
            { str = "拾物"; }
            if (notes.Contains("直播"))
            { str = "直播"; }
            if (notes.Contains("时时彩"))
            { str = "时时彩票"; }
            if (notes.Contains("活动礼品"))
            { str = "活动礼品"; }
            if (notes.Contains("百花谷"))
            { str = "百花谷"; }
            if (notes.Contains("快3"))
            { str = "快3挖宝"; }
            if (notes.Contains("彩票"))
            { str = "虚拟彩票"; }
            if (notes.Contains("球彩"))
            { str = "虚拟竞猜"; }
            if (notes.Contains("捕鱼"))
            { str = "捕鱼达人"; }
            if (notes.Contains("快乐十分"))
            { str = "快乐十分"; }
            if (notes.Contains("闯荡全城"))
            { str = "闯荡全城"; }
            if (notes.Contains("挖宝"))
            { str = "点数挖宝"; }
            if (notes.Contains("点值"))
            { str = "点值抽奖"; }
            if (notes.Contains("德州扑克"))
            { str = "德州扑克"; }
            if (notes.Contains("快乐扑克3"))
            { str = "快乐扑克3"; }
            if (notes.Contains("云购"))
            { str = "酷币云购"; }
            if (notes.Contains("红包群聊"))
            { str = "红包群聊"; }
            if (notes.Contains("论坛功能"))
            { str = "论坛功能"; }
            //if (notes.Contains("宠物"))
            //{ str = "[URL=/bbs/game/0.aspx]宠物[/URL]"; }
            if (notes.Contains("百家欢乐"))
            { str = "百家欢乐"; }
            if (notes.Contains("虚拟彩票"))
            { str = "虚拟彩票"; }
            if (notes.Contains("农场"))
            { str = "开心农场"; }
            if (notes.Contains("6场半"))
            { str = "6场半"; }
            if (notes.Contains("好彩一"))
            { str = "好彩一"; }
            if (notes.Contains("PK拾"))
            { str = "PK拾"; }
            if (notes.Contains("胜负彩"))
            { str = "胜负彩"; }
            if (notes.Contains("进球彩"))
            { str = "进球彩"; }
            if (notes.Contains("宠物"))
            { str = "至爱宠物"; }
            //   发表帖子#回复帖子#帖子打赏#加精帖子#推荐帖子#设滚帖子#空间留言#购买VIP
            //#购买靓号#上存照片#商城礼物#赠送礼物#空间签到#过户币值#闲聊发言#发红包者
            //#发喇叭者#婚恋结婚#花园种花#注册会员#等级晋升#推荐注册#加为好友#书节上存#" />
            if (notes.Contains("加精"))
            { str = "加精帖子"; }
            if (notes.Contains("帖子加精"))
            { str = "加精帖子"; }
            if (notes.Contains("帖子设滚"))
            { str = "设滚帖子"; }
            if (notes.Contains("帖子币"))
            { str = "派币帖子"; }
            if (notes.Contains("帖子打赏"))
            { str = "帖子打赏"; }
            if (notes.Contains("帖子推荐"))
            { str = "推荐帖子"; }
            if (notes.Contains("帖子推荐"))
            { str = "推荐帖子["; }
            if (notes.Contains("签到"))
            { str = "签到"; }
            if (notes.Contains("获得") && notes.Contains("flows.aspx"))
            { str = "拾物"; }
            if (notes.Contains("回复帖子"))
            { str = "回复帖子"; }
            if (notes.Contains("发表帖子"))
            { str = "发表帖子"; }
            if (notes.Contains("游戏竞技"))
            { str = "游戏竞技"; }
            if (notes.Contains("注册会员"))
            { str = "注册会员"; }
            if (notes.Contains("赠送礼物"))
            { str = "赠送礼物"; }
            if (notes.Contains("过户币"))
            { str = "过户币值"; }
            if (notes.Contains("发喇叭"))
            { str = "发喇叭者"; }
            if (notes.Contains("结婚"))
            { str = "婚恋结婚"; }
            if (notes.Contains("种花"))
            { str = "花园种花"; }
            if (notes.Contains("购买靓号"))
            { str = "购买靓号"; }
            if (notes.Contains("上传照片"))
            { str = "上传照片"; }
            if (notes.Contains("购买商城礼物"))
            { str = "商城礼物"; }
            if (notes.Contains("发内线"))
            { str = "发内线"; }
            if (notes.Contains("加好友"))
            { str = "加为好友"; }
            if (notes.Contains("上传书"))
            { str = "书节上存"; }
            if (notes.Contains("发表书评"))
            { str = "发表书评"; }
            if (notes.Contains("获授勋章"))
            { str = "获授勋章"; }
            if (notes.Contains("赠送"))
            { str = "赠送礼物"; }
            if (notes.Contains("空间设置"))
            { str = "空间签到"; }
            if (notes.Contains("空间签到"))
            { str = "空间签到"; }
            if (notes.Contains("空间连续一周签到"))
            { str = "空间签到"; }
            if (notes.Contains("闲聊发言"))
            { str = "闲聊发言"; }
            if (notes.Contains("聊吧发言"))
            { str = "聊吧发言"; }
            if (notes.Contains("酷友热聊室"))
            { str = "酷友热聊室"; }
            if (notes.Contains("发红包者"))
            { str = "发红包者"; }
            if (notes.Contains("活跃抽奖"))
            { str = "活跃抽奖"; }
            if (notes.Contains("球彩"))
            { str = "虚拟球类"; }
            if (str == "")
            {
                return "404";
            }
            else
            {
                return str;
            }
        }

        #region 酷币消费生成点值
        /// <summary>
        ///主判断区判断酷币消费范围
        ///UsId   会员ID
        ///UsName  昵称
        ///iGold   消费的酷币
        ///Notes 动作说明
        /// </summary>
        public void AddjfbyiGold(int UsID, string UsName, long iGold, string Notes)
        {
            int ceshi = Convert.ToInt32(ub.GetSub("ceshi", "/Controls/draw.xml"));
            //1全社区2社区3仅游戏
            int drawOpenChoose = Convert.ToInt32(ub.GetSub("drawOpenChoose", "/Controls/draw.xml"));
            //状态1维护2内测0正常
            string drawStatus = (ub.GetSub("drawStatus", xmlPath));
            //0|关闭抽奖值生成|1|开启抽奖值生成
            string drawOpenOrClose = (ub.GetSub("drawOpenOrClose", xmlPath));

            string strNote = Notes;
            string games = getTypesForGameName(Notes);//链接
            string games2 = getTypesForGameName2(Notes);//游戏名字
            string gameAction = (ub.GetSub("gameAction", xmlPath));//游戏区
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));//社区


            #region 判断酷币下注生成点值
            long kb1 = Convert.ToInt64(ub.GetSub("kb1", xmlPath));
            long kb2 = Convert.ToInt64(ub.GetSub("kb2", xmlPath));
            long kb3 = Convert.ToInt64(ub.GetSub("kb3", xmlPath));
            long kb4 = Convert.ToInt64(ub.GetSub("kb4", xmlPath));
            long kb5 = Convert.ToInt64(ub.GetSub("kb5", xmlPath));
            int kb1jf = Convert.ToInt32(ub.GetSub("kb1jf", xmlPath));
            int kb12jf = Convert.ToInt32(ub.GetSub("kb12jf", xmlPath));
            int kb23jf = Convert.ToInt32(ub.GetSub("kb23jf", xmlPath));
            int kb34jf = Convert.ToInt32(ub.GetSub("kb34jf", xmlPath));
            int kb45jf = Convert.ToInt32(ub.GetSub("kb45jf", xmlPath));
            int kb5jf = Convert.ToInt32(ub.GetSub("kb5jf", xmlPath));
            int kbkg = Convert.ToInt32(ub.GetSub("kbkg", xmlPath));
            int tmax = Convert.ToInt32(ub.GetSub("wabao", xmlPath));//帖子生成最大值
            long kb = iGold;
            if (drawStatus == "0" && drawOpenOrClose == "1" && kbkg == 0)//游戏开放且点值生成开放,并且酷币消费生成开启
            {
                if (ceshi == 0)// 0酷币版开放
                {
                    if (("#" + gameAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#") || ("#" + bbsAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#"))//判断游戏或者社区选中了抽奖值生成
                    {
                        if (kb < 0)//酷币是消费的
                        {
                            if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyGame(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                            {
                                if (Math.Abs(kb) < kb1)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb1jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                                else if (Math.Abs(kb) >= kb1 && Math.Abs(kb) < kb2)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb12jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                                else if (Math.Abs(kb) >= kb2 && Math.Abs(kb) < kb3)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb23jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                                else if (Math.Abs(kb) >= kb3 && Math.Abs(kb) < kb4)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb34jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                                else if (Math.Abs(kb) >= kb4 && Math.Abs(kb) < kb5)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb45jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                                else if (Math.Abs(kb) > kb5)
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb5jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                }
                            }
                        }
                    }
                }
                else//1酷币版测试状态
                {
                    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", "/Controls/draw.xml"));
                    if (CeshiQualification != "")
                    {
                        if (("#" + CeshiQualification + "#").Contains("#" + UsID.ToString() + "#"))//如果是测试号
                        {
                            if (("#" + gameAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#") || ("#" + bbsAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#"))//判断游戏或者社区选中了抽奖值生成
                            {
                                if (kb < 0)//酷币是消费的
                                {
                                    if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyGame(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                                    {
                                        if (Math.Abs(kb) < kb1)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb1jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                        else if (Math.Abs(kb) >= kb1 && Math.Abs(kb) < kb2)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb12jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                        else if (Math.Abs(kb) >= kb2 && Math.Abs(kb) < kb3)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb23jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                        else if (Math.Abs(kb) >= kb3 && Math.Abs(kb) < kb4)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb34jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                        else if (Math.Abs(kb) >= kb4 && Math.Abs(kb) < kb5)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb45jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                        else if (Math.Abs(kb) > kb5)
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, kb5jf, "" + getTypesForGameName2(Notes) + "消费" + Math.Abs(iGold) + ub.Get("SiteBz") + "奖励");//XXX得到抽奖值
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            #endregion
        }
        #endregion


        #region 社区生成点值
        /// <summary>
        ///主判断区判断论坛帖子范围
        ///UsId   会员ID
        ///UsName  昵称
        ///Notes 动作说明
        /// </summary>
        public int AddjfbyTz(int UsID, string UsName, string Notes)
        {
            int ceshi = Convert.ToInt32(ub.GetSub("ceshi", "/Controls/draw.xml"));
            //1全社区2社区3仅游戏
            int drawOpenChoose = Convert.ToInt32(ub.GetSub("drawOpenChoose", "/Controls/draw.xml"));
            //状态1维护2内测0正常
            string drawStatus = (ub.GetSub("drawStatus", xmlPath));
            //0|关闭抽奖值生成|1|开启抽奖值生成
            string drawOpenOrClose = (ub.GetSub("drawOpenOrClose", xmlPath));

            string strNote = Notes;
            string games = getTypesForGameName(Notes);//链接
            string games2 = getTypesForGameName2(Notes);//游戏名字
            string gameAction = (ub.GetSub("gameAction", xmlPath));//游戏区
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));//社区


            #region 判断社区生成点值
            int ft = Convert.ToInt32(ub.GetSub("ft", xmlPath));//发帖
            int ht = Convert.ToInt32(ub.GetSub("ht", xmlPath));//回帖
            int td = Convert.ToInt32(ub.GetSub("td", xmlPath));//帖子打赏
            int tj = Convert.ToInt32(ub.GetSub("tj", xmlPath));//帖子加精
            int tt = Convert.ToInt32(ub.GetSub("tt", xmlPath));//推荐帖子
            int ts = Convert.ToInt32(ub.GetSub("ts", xmlPath));//设滚帖子
            int tmax = Convert.ToInt32(ub.GetSub("tmax", xmlPath));//帖子生成最大值
            int tzkg = Convert.ToInt32(ub.GetSub("tzkg", xmlPath));//帖子生成点值的开关
            int tzkl = Convert.ToInt32(ub.GetSub("tzkl", xmlPath));//产生点值的概率
            int xianliao = Convert.ToInt32(ub.GetSub("xianliao", xmlPath));//闲聊
            int fahb = Convert.ToInt32(ub.GetSub("fahb", xmlPath));//发红包者
            int shup = Convert.ToInt32(ub.GetSub("shup", xmlPath));//发表书评
            int ksfy = Convert.ToInt32(ub.GetSub("ksfy", xmlPath));//聊吧发言

            if (drawStatus == "0" && drawOpenOrClose == "1" && tzkg == 0)//游戏开放且点值生成开放,并且帖子生成开启
            {
                if (ceshi == 0)// 0酷币版开放
                {
                    if (("#" + bbsAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#"))//判断社区选中了抽奖值生成
                    {
                        Random r = new Random();
                        int kl = 0;
                        kl = r.Next(0, (tzkl + 1));//产生随机数，随机数为1时可生成点值
                        if (kl == 1)
                        {
                            if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyTz(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                            {
                                if (Notes.Contains("帖子加精"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, tj, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("帖子设滚"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ts, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("帖子打赏"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, td, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("帖子推荐"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, tt, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("回复帖子"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ht, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("发表帖子"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ft, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("闲聊发言"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, xianliao, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("发红包者"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, fahb, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("发表书评"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, shup, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                                if (Notes.Contains("聊吧发言"))
                                {
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ksfy, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                }
                            }
                        }
                    }
                }
                else//1酷币版测试状态
                {
                    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", "/Controls/draw.xml"));
                    if (CeshiQualification != "")
                    {
                        if (("#" + CeshiQualification + "#").Contains("#" + UsID.ToString() + "#"))//如果是测试号
                        {
                            if (("#" + bbsAction + "#").Contains("#" + getTypesForGameName2(Notes) + "#"))//判断社区选中了抽奖值生成
                            {
                                Random r = new Random();
                                int kl = 0;
                                kl = r.Next(0, (tzkl + 1));//产生随机数，随机数为1时可生成点值
                                if (kl == 1)
                                {
                                    if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyTz(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                                    {
                                        if (Notes.Contains("帖子加精"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, tj, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("帖子设滚"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ts, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("帖子打赏"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, td, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("帖子推荐"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, tt, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("回复帖子"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ht, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("发表帖子"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ft, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("闲聊发言"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, xianliao, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("发红包者"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, fahb, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("发表书评"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, shup, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                        if (Notes.Contains("聊吧发言"))
                                        {
                                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, ksfy, "社区" + getTypesForGameName2(Notes) + "奖励");//XXX得到抽奖值
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return 1;
            }
            else
            {
                return 1;
            }
            #endregion
        }
        #endregion

        #region 网上充值方法
        /// <summary>
        ///UsId   会员ID
        ///UsName  昵称
        ///RMB 充值金额
        ///Notes 动作说明
        /// </summary>
        public void Addjfbychongzhi(int UsID, string UsName, decimal RMB, string Notes)
        {
            int ceshi = Convert.ToInt32(ub.GetSub("ceshi", "/Controls/draw.xml"));
            //状态1维护2内测0正常
            string drawStatus = (ub.GetSub("drawStatus", xmlPath));
            //0|关闭抽奖值生成|1|开启抽奖值生成
            string drawOpenOrClose = (ub.GetSub("drawOpenOrClose", xmlPath));

            string strNote = Notes;
            string games = getTypesForGameName(Notes);//链接
            string games2 = getTypesForGameName2(Notes);//游戏名字
            string gameAction = (ub.GetSub("gameAction", xmlPath));//游戏区
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));//社区


            #region 判断网上充值生成点值
            int chongzhikg = Convert.ToInt32(ub.GetSub("chongzhikg", xmlPath));
            int chongzhi = Convert.ToInt32(ub.GetSub("chongzhi", xmlPath));
            int chongmin = Convert.ToInt32(ub.GetSub("chongmin", xmlPath));
            int chongzhijf = Convert.ToInt32(ub.GetSub("chongzhijf", xmlPath));
            int chongmax = Convert.ToInt32(ub.GetSub("chongmax", xmlPath));
            int chongmaxzhi = Convert.ToInt32(ub.GetSub("chongmaxzhi", xmlPath));
            int tmax = Convert.ToInt32(ub.GetSub("kaizhuang", xmlPath));//帖子生成最大值
            int R = 0;

            if (drawStatus == "0" && drawOpenOrClose == "1" && chongzhikg == 0)//游戏开放且点值生成开放,并且网上充值生成开启
            {
                if (ceshi == 0)// 0酷币版开放
                {
                    if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyCz(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                    {
                        if (RMB >= chongmin && RMB <= chongmax)
                        {
                            if (RMB % chongzhi == 0)
                            {
                                R = Convert.ToInt32(RMB / chongzhi) * chongzhijf;
                                new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                            }
                            else
                            {
                                R = (Convert.ToInt32(RMB / chongzhi) * chongzhijf) + 1;
                                new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                            }
                        }
                        else if (RMB > chongmax)
                        {
                            R = chongmaxzhi;
                            new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                        }
                    }
                }
            }
            else//1酷币版测试状态
            {
                string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", "/Controls/draw.xml"));
                if (CeshiQualification != "")
                {
                    if (("#" + CeshiQualification + "#").Contains("#" + UsID.ToString() + "#"))//如果是测试号
                    {
                        if (new BCW.Draw.BLL.DrawJifenlog().GetJfbyCz(UsID) <= tmax)//当天当用户生成的抽奖值小于当天能生成的最大值
                        {
                            if (RMB >= chongmin && RMB <= chongmax)
                            {
                                if (RMB % chongzhi == 0)
                                {
                                    R = Convert.ToInt32(RMB / chongzhi) * chongzhijf;
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                                }
                                else
                                {
                                    R = (Convert.ToInt32(RMB / chongzhi) * chongzhijf) + 1;
                                    new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                                }
                            }
                            else if (RMB > chongmax)
                            {
                                R = chongmaxzhi;
                                new BCW.Draw.BLL.DrawJifen().UpdateJifen(UsID, UsName, R, "网上充值" + RMB + "（RMB）奖励");//XXX得到抽奖值
                            }
                        }
                    }
                }
            }
        }

            #endregion

        #endregion


    }
}
