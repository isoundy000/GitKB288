using System;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Text;
using BCW.Common;
using BCW.Model;
using System.Data;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Diagnostics;
using BCW.Common;
using System.IO;

using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using BCW.Common;
///
/// 20160606_去除拾物入口版本
/// 20160607_减少一等奖的出现
/// 20160611_游戏仅开放球彩
/// 20160613_继续修复一等奖流失过快
/// 20160616_完善后台版本，勾选功能
/// 20160621_加控制额度的表
/// 20160627  球彩停止派奖问 题
/// 20160815 变成5个奖池
/// 20161008 使用每个游戏都名字作为标识
/// 
namespace BCW.winners
{
    public class winners
    {
        protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
        protected int PassTime = Convert.ToInt32(ub.GetSub("PassTime", "/Controls/winners.xml"));
        protected int maxGet = Convert.ToInt32(ub.GetSub("maxGet", "/Controls/winners.xml"));
        protected int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", "/Controls/winners.xml"));
        protected string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));
        protected string GameName = Convert.ToString(ub.GetSub("WinnersName", "/Controls/winners.xml"));
        protected int statue = Convert.ToInt32(ub.GetSub("WinnersStatus", "/Controls/winners.xml"));
        protected string xmlPath = "/Controls/winners.xml";
        protected string bydrname = Convert.ToString(ub.GetSub("bydrName", "/Controls/BYDR.xml"));//捕鱼达人
        protected string drawlife = ub.GetSub("DawnlifeName", "/Controls/Dawnlife.xml");//闯荡人生
        protected string dzpkName = ub.GetSub("DzpkName", "/Controls/dzpk.xml");//德州扑克
        protected string farmName = ub.GetSub("FarmName", "/Controls/farm.xml");//农场
        protected string hc1Name = ub.GetSub("Hc1Name", "/Controls/hc1.xml");//好彩一
        protected string hp3 = ub.GetSub("HP3Name", "/Controls/HappyPoker3.xml");//快乐扑克3
        protected string jqc = ub.GetSub("GameName", "/Controls/jqc.xml");//进球彩
        protected string kbyg = Convert.ToString(ub.GetSub("KbygName", "/Controls/myyg.xml"));//云购
        protected string klsf = ub.GetSub("klsfName", "/Controls/klsf.xml");//快乐十分
        protected string luck28 = ub.GetSub("Luck28Name", "/Controls/luck28.xml");//幸运28
        protected string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");//pk10
        protected string sfc = ub.GetSub("SFName", "/Controls/SFC.xml");//胜负彩
        protected string xk3 = ub.GetSub("XinKuai3Name", "/Controls/xinkuai3.xml");//新快3
        protected string sixchangban = ub.GetSub("BQCName", "/Controls/BQC.xml");//六场半
        protected string bjl = ub.GetSub("baccaratName", "/Controls/bjl.xml");//百家乐

        /// <summary>
        /// //返回判断记录属于的名称
        /// 游戏的属性
        /// 返回各动作的地址
        /// </summary>
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
            if (notes.Contains(luck28))
            { str = "在[URL=/bbs/game/luck28.aspx]" + luck28 + "[/URL]"; }
            if (notes.Contains("幸运二八"))
            { str = "在[URL=/bbs/game/luck28.aspx]" + luck28 + "[/URL]"; }
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
            if (notes.Contains(bydrname) || notes.Contains("捕鱼"))
            { str = "在[URL=/bbs/game/cmg.aspx]" + bydrname + "[/URL]"; }
            if (notes.Contains(klsf))
            { str = "在[URL=/bbs/game/klsf.aspx]" + klsf + "[/URL]"; }
            if (notes.Contains("大小掷骰"))
            { str = "在[URL=/bbs/game/dxdice.aspx]大小掷骰[/URL]"; }
            if (notes.Contains(drawlife))
            { str = "在[URL=/bbs/game/Dawnlife.aspx]" + drawlife + "[/URL]"; }
            if (notes.Contains("点值"))
            { str = "在[URL=/bbs/game/Draw.aspx]点值抽奖[/URL]"; }
            if (notes.Contains(dzpkName))
            { str = "在[URL=/bbs/game/dzpk.aspx]" + dzpkName + "[/URL]"; }
            if (notes.Contains(hp3))
            { str = "在[URL=/bbs/game/hp3.aspx]" + hp3 + "[/URL]"; }
            string kbyg = ub.GetSub("KbygName", "/Controls/myyg.xml");
            if (notes.Contains(kbyg) || notes.Contains("云购"))
            { str = "在[URL=/bbs/game/kbyg.aspx]" + kbyg + "[/URL]"; }
            if (notes.Contains("红包群聊"))
            { str = "在[URL=/bbs/game/chatroom.aspx]红包群聊[/URL]"; }
            if (notes.Contains("论坛功能"))
            { str = "在[URL=/bbs/forum.aspx]论坛功能[/URL]"; }
            if (notes.Contains("球彩"))
            { str = "在[URL=/bbs/guess2/default.aspx]虚拟球类[/URL]"; }
            //if (notes.Contains("宠物"))
            //{ str = "在[URL=/bbs/game/kbyg.aspx]宠物[/URL]"; }
            if (notes.Contains("百家欢乐") || notes.Contains(bjl))
            { str = "在[URL=/bbs/game/baccarat.aspx]" + bjl + "[/URL]"; }
            if (notes.Contains("虚拟彩票"))
            { str = "在[URL=/bbs/game/six49.aspx]虚拟彩票[/URL]"; }
            if (notes.Contains("竞拍"))
            { str = "在[URL=/bbs/game/race.aspx]欢乐竞拍[/URL]"; }
            if (notes.Contains("农场") || notes.Contains(farmName))
            { str = "在[URL=/bbs/game/farm.aspx]" + farmName + "[/URL]"; }
            string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");
            if (notes.Contains(pk10))
            { str = "在[URL=/bbs/game/PK10.aspx]" + pk10 + "[/URL]"; }
            string hc1 = ub.GetSub("Hc1Name", "/Controls/hc1.xml");
            if (notes.Contains("好彩一"))
            { str = "在[URL=/bbs/game/hc1.aspx]" + hc1 + "[/URL]"; }
            if (notes.Contains("点值"))
            { str = "在[URL=/bbs/game/draw.aspx]点值抽奖[/URL]"; }
            //// ------ 
            if (notes.Contains("加精"))
            { str = "在[URL=/bbs/forum.aspx]加精[/URL]"; }
            if (notes.Contains("帖子加精"))
            { str = "在[URL=/bbs/forum.aspx]帖子加精[/URL]"; }
            if (notes.Contains("设滚"))
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
            if (notes.Contains("发表") && notes.Contains("帖子") && notes.Contains("topic"))
            { str = "在[URL=/bbs/addThread.aspx]发表帖子[/URL]"; }
            if (notes.Contains("游戏竞技"))
            { str = "在[URL=/bbs/forum.aspx]游戏竞技[/URL]"; }
            if (notes.Contains("新注册会员"))
            { str = "在[URL=/default.aspx]新注册会员[/URL]"; }
            if (notes.Contains("赠送礼物"))
            { str = "在[URL=/bbs/bbsshop.aspx]赠送礼物[/URL]"; }
            if (notes.Contains("过户币"))
            { str = "在[URL=/bbs/finance.aspx]过户币[/URL]"; }
            if (notes.Contains("打赏") && notes.Contains("帖子"))
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
            if (notes.Contains("酷友热聊室"))
            { str = "在[URL=/bbs/chatroom.aspx]酷友热聊室[/URL]"; }
            if (notes.Contains("聊吧"))
            { str = "在[URL=/bbs/chatroom.aspx?id=29]聊吧[/URL]"; }
            if (notes.Contains("红包"))
            { str = "在[URL=/bbs/game/speak.aspx]红包[/URL]"; }
            if (notes.Contains("活跃抽奖"))
            { str = "在[URL=/bbs/game/winners.aspx]活跃抽奖[/URL]"; }
            return str;
        }

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
            if (notes.Contains(luck28))
            { str = luck28; }
            if (notes.Contains("幸运二八"))
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
            if (notes.Contains("新快3") || notes.Contains(xk3))
            { str = xk3; }
            if (notes.Contains("彩票"))
            { str = "虚拟彩票"; }
            if (notes.Contains("球彩"))
            { str = "虚拟球类"; }
            if (notes.Contains("捕鱼") || notes.Contains(bydrname))
            { str = bydrname; }
            if (notes.Contains("快乐十分") || notes.Contains(klsf))
            { str = klsf; }
            if (notes.Contains("闯荡全城") || notes.Contains(drawlife))
            { str = drawlife; }
            if (notes.Contains("挖宝"))
            { str = "点数挖宝"; }
            if (notes.Contains("点值"))
            { str = "点值抽奖"; }
            if (notes.Contains("德州扑克") || notes.Contains(dzpkName))
            { str = dzpkName; }
            if (notes.Contains("快乐扑克3") || notes.Contains(hp3))
            { str = hp3; }
            if (notes.Contains("云购") || notes.Contains(kbyg))
            { str = kbyg; }
            if (notes.Contains("红包群聊"))
            { str = "红包群聊"; }
            if (notes.Contains("论坛功能"))
            { str = "论坛功能"; }
            //if (notes.Contains("宠物"))
            //{ str = "[URL=/bbs/game/kbyg.aspx]宠物[/URL]"; }
            if (notes.Contains("百家欢乐") || notes.Contains(bjl))
            { str = bjl; }
            if (notes.Contains("虚拟彩票"))
            { str = "虚拟彩票"; }
            if (notes.Contains("虚拟球类"))
            { str = "虚拟球类"; }
            if (notes.Contains(farmName) || notes.Contains("农场"))
            { str = farmName; }
            string pk10 = ub.GetSub("GameName", "/Controls/PK10.xml");
            if (notes.Contains(pk10))
            { str = pk10; }
            if (notes.Contains("好彩一") || notes.Contains(hc1Name))
            { str = hc1Name; }
            if (notes.Contains("点值"))
            { str = "点值抽奖"; }
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
            if (notes.Contains("获得"))
            { str = "拾物"; }
            if (notes.Contains("回复帖子"))
            { str = "回复帖子"; }
            if (notes.Contains("发表"))
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
            if (notes.Contains("书评"))
            { str = "发表书评"; }
            if (notes.Contains("获授勋章"))
            { str = "获授勋章"; }
            if (notes.Contains("赠送"))
            { str = "赠送礼物"; }
            if (notes.Contains("空间设置"))
            { str = "空间设置"; }
            if (notes.Contains("空间签到"))
            { str = "空间签到"; }
            if (notes.Contains("空间连续一周签到"))
            { str = "空间签到"; }
            if (notes.Contains("闲聊"))
            { str = "闲聊发言"; }
            if (notes.Contains("聊吧"))
            { str = "聊吧"; }
            if (notes.Contains("酷友热聊室"))
            { str = "酷友热聊室"; }
            if (notes.Contains("红包"))
            { str = "红包"; }
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

        //返回 0游戏 1论坛 2 总
        public int getTypesForGameName3(string notes)
        {
            // 0 | 游戏区奖池1 | 1 | 游戏区奖池2 | 2 | 游戏区奖池3 | 3 | 论坛奖池4 | 4 | 总奖池
            int str = 0;
            if (notes.Contains(bjl))
            { str = 0; }
            if (notes.Contains(sixchangban))
            { str = 0; }
            if (notes.Contains(xk3))
            { str = 0; }
            if (notes.Contains(sfc))
            { str = 0; }
            if (notes.Contains(pk10))
            { str = 0; }
            if (notes.Contains(luck28))
            { str = 0; }
            if (notes.Contains(klsf))
            { str = 0; }
            if (notes.Contains(kbyg))
            { str = 0; }
            if (notes.Contains(jqc))
            { str = 0; }
            if (notes.Contains(hp3))
            { str = 0; }
            if (notes.Contains(hc1Name))
            { str = 0; }
            if (notes.Contains(farmName))
            { str = 0; }
            if (notes.Contains(dzpkName))
            { str = 0; }
            if (notes.Contains(drawlife))
            { str = 0; }
            if (notes.Contains(bydrname))
            { str = 0; }
            if (notes.Contains("多人剪刀"))
            { str = 0; }
            if (notes.Contains("天天好彩"))
            { str = 0; }
            if (notes.Contains("Ktv789"))
            { str = 0; }
            if (notes.Contains("猜猜乐"))
            { str = 0; }
            if (notes.Contains("大小庄"))
            { str = 0; }
            if (notes.Contains("欢乐竞拍"))
            { str = 0; }
            if (notes.Contains("竞猜"))
            { str = 0; }
            if (notes.Contains("幸运28"))
            { str = 0; }
            if (notes.Contains("幸运二八"))
            { str = 0; }
            if (notes.Contains("虚拟投注"))
            { str = 0; }
            if (notes.Contains("挖宝"))
            { str = 0; }
            if (notes.Contains("跑马"))
            { str = 0; }
            if (notes.Contains("上证"))
            { str = 0; }
            if (notes.Contains("吹牛"))
            { str = 0; }
            if (notes.Contains("猜拳"))
            { str = 0; }
            if (notes.Contains("苹果机"))
            { str = 0; }
            if (notes.Contains("掷骰"))
            { str = 0; }
            if (notes.Contains("拾物"))
            { str = 0; }
            if (notes.Contains("直播"))
            { str = 0; }
            if (notes.Contains("时时彩"))
            { str = 0; }
            if (notes.Contains("活动礼品"))
            { str = 0; }
            if (notes.Contains("百花谷"))
            { str = 0; }
            if (notes.Contains("新快3"))
            { str = 0; }
            if (notes.Contains("彩票"))
            { str = 0; }
            if (notes.Contains("球彩"))
            { str = 0; }
            if (notes.Contains("捕鱼"))
            { str = 0; }
            if (notes.Contains("快乐十分"))
            { str = 0; }
            if (notes.Contains("大小掷骰"))
            { str = 0; }
            if (notes.Contains("闯荡全城"))
            { str = 0; }
            if (notes.Contains("点值"))
            { str = 0; }
            if (notes.Contains("德州扑克"))
            { str = 0; }
            if (notes.Contains("快乐扑克3"))
            { str = 0; }
            if (notes.Contains("云购"))
            { str = 0; }
            if (notes.Contains("红包群聊"))
            { str = 0; }
            if (notes.Contains("论坛功能"))
            { str = 0; }
            if (notes.Contains("宠物"))
            { str = 0; }
            if (notes.Contains("百家欢乐"))
            { str = 0; }
            if (notes.Contains("虚拟彩票"))
            { str = 0; }
            if (notes.Contains("虚拟球类"))
            { str = 0; }
            if (notes.Contains("农场"))
            { str = 0; }
            if (notes.Contains("快3"))
            { str = 0; }
            if (notes.Contains("竞拍"))
            { str = 0; }
            if (notes.Contains("活跃抽奖"))
            { str = 3; }
            if (notes.Contains(ub.GetSub("GameName", "/Controls/PK10.xml")))
            { str = 0; }
            if (notes.Contains("好彩一"))
            { str = 0; }
            if (notes.Contains("点值"))
            { str = 0; }
            //论坛开始   tempAward.getRedy = type;//0 1 2 类型 0游戏奖池 1论坛奖池 2总奖池
            if (notes.Contains("空间设置"))
            { str = 1; }
            if (notes.Contains("加精"))
            { str = 1; }
            if (notes.Contains("帖子加精"))
            { str = 1; }
            if (notes.Contains("帖子设滚"))
            { str = 1; }
            if (notes.Contains("帖子币"))
            { str = 1; }
            if (notes.Contains("帖子打赏"))
            { str = 1; }
            if (notes.Contains("帖子推荐"))
            { str = 1; }
            if (notes.Contains("帖子推荐"))
            { str = 1; }
            if (notes.Contains("签到"))
            { str = 1; }
            if (notes.Contains("获得") && notes.Contains("flows.aspx"))
            { str = 1; }
            if (notes.Contains("回复帖子"))
            { str = 1; }
            if (notes.Contains("发表"))
            { str = 1; }
            if (notes.Contains("游戏竞技"))
            { str = 1; }
            if (notes.Contains("新注册会员"))
            { str = 1; }
            if (notes.Contains("赠送礼物"))
            { str = 1; }
            if (notes.Contains("过户币"))
            { str = 1; }
            if (notes.Contains("帖子打赏"))
            { str = 1; }
            if (notes.Contains("发喇叭"))
            { str = 1; }
            if (notes.Contains("结婚"))
            { str = 1; }
            if (notes.Contains("种花"))
            { str = 1; }
            if (notes.Contains("购买靓号"))
            { str = 1; }
            if (notes.Contains("上传照片"))
            { str = 1; }
            if (notes.Contains("购买商城礼物"))
            { str = 1; }
            if (notes.Contains("发内线"))
            { str = 1; }
            if (notes.Contains("加好友"))
            { str = 1; }
            if (notes.Contains("上传书"))
            { str = 1; }
            if (notes.Contains("书评"))
            { str = 1; }
            if (notes.Contains("获授勋章"))
            { str = 1; }
            if (notes.Contains("赠送"))
            { str = 1; }
            if (notes.Contains("空间设置"))
            { str = 1; }
            if (notes.Contains("空间签到"))
            { str = 1; }
            if (notes.Contains("空间连续一周签到"))
            { str = 1; }
            if (notes.Contains("闲聊"))
            { str = 1; }
            if (notes.Contains("聊吧"))
            { str = 1; }
            if (notes.Contains("酷友热聊室"))
            { str = 1; }
            if (notes.Contains("红包"))
            { str = 1; }
            if (notes.Contains("论坛"))
            { str = 1; }
            if (notes.Contains("礼品"))
            { str = 1; }
            if (str >= 0)
            { return str; }
            else
            { return 2; }
        }

        #region 判断数据是否丢失，丢失则返回真
        private bool isLost(int id)
        {

            int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
            int ActionId = id;//new BCW.BLL.Action().GetMaxId();
            BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);
            string[] losenum = model.getUsId.Split(',');
            string[] getwinnumber = model.getWinNumber.Split('#');
            bool boo = false;
            int num = 0;
            Stopwatch watch1 = new Stopwatch();//实例化一个计时器 
            watch1.Start();//开始计时/*
            for (int j = 0; j < getwinnumber.Length - 1; j++)//统计等奖number 的数据剩余
            {
                num += Convert.ToInt32(getwinnumber[j].ToString().Trim());
            }
            if (num != Convert.ToInt32(model.awardNowCount))
            {
                boo = true;
            }
            //for (int i = 0; i < losenum.Length - 1; i++)
            //{
            //    if (ActionId > Convert.ToInt32(losenum[i].ToString().Trim()))
            //    {
            //        boo = true;
            //    }
            //    watch1.Stop();//结束计时
            //    int time = Convert.ToInt32(watch1.ElapsedMilliseconds);
            //    if (time > 500)
            //    { return false; }
            //    watch1.Start();
            //}
            watch1.Reset();
            if ((losenum.Length - 1) != model.awardNowCount)
            { boo = true; }
            if (num != losenum.Length - 1)
            {
                boo = true;
            }
            return boo;
        }
        #endregion

        /// <summary>
        ///主判断区，是否中奖，是否达到最大限量
        ///达到则不派奖
        ///返回值：1中奖 2达到最大限量 3未有权限获奖
        ///Types 游戏类型
        ///NodId 记录表ID
        ///UsId   会员ID
        ///UsName  昵称
        ///Notes 动作说明
        ///ID 
        /// </summary>
        public int CheckActionForAll(int Types, int NodeId, int UsId, string UsName, string Notes, int ID)
        {
            int rettr = 4;
            int ceshi = Convert.ToInt32(ub.GetSub("ceshi", "/Controls/winners.xml"));
            //1全社区2社区3仅游戏
            int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", "/Controls/winners.xml"));
            //状态1维护2测试0正常
            string WinnersStatus = (ub.GetSub("WinnersStatus", xmlPath));
            //0|停止放送机会|1|开启放送机会
            string WinnersOpenOrClose = (ub.GetSub("WinnersOpenOrClose", xmlPath));
            string gameAction = (ub.GetSub("gameAction", xmlPath));
            string bbsAction = (ub.GetSub("bbsAction", xmlPath));
            string blackID = (ub.GetSub("blackID", xmlPath));

            string strNote = Notes;
            string games = getTypesForGameName(Notes);//游戏链接,存数据库
            string games2 = getTypesForGameName2(games);//游戏名字
            int games3 = getTypesForGameName3(games2);//类型  返回 0游戏 1论坛 2 总


            //1 维护 0 关闭
            if (WinnersStatus == "1" || WinnersOpenOrClose == "0")
            { return 2; }

            // 关闭拾物，抽奖
            if (Notes.Contains("拾物") || games.Contains("拾物") || games.Contains("活跃抽奖") || Notes.Contains("活跃抽奖"))
            {
                return 3;
            }

            #region 开关
            // 全网开放
            if (WinnersOpenChoose == 1)
            {
                if (gameAction.Contains(games2) || bbsAction.Contains(games2))
                {
                    rettr = 2;
                }
                else
                {
                    rettr = 403;
                    return 403;
                }
            }

            //仅社区
            else if (WinnersOpenChoose == 2)
            {
                if (bbsAction != "")
                {
                    if (!(bbsAction.Contains(games2)))//社区仅允许的动作
                    {
                        rettr = 404;
                        return 404;
                    }
                }
            }

            // 仅游戏
            else if (WinnersOpenChoose == 3)//仅游戏_20160611_仅开放球赛
            {
                if (gameAction != "")
                {
                    if (!gameAction.Contains(games2))//游戏区仅允许的动作
                    {
                        rettr = 405;
                        return 405;
                    }
                }
            }
            else
            {
                rettr = 406;
                return 406;
            }
            #endregion

            #region 黑名单处理
            if (blackID != "")
            {
                if (("#" + blackID + "#").Contains("#" + UsId.ToString() + "#"))
                {
                    rettr = 404;
                    return 404;
                }
            }
            #endregion

            #region 判断仅测试号能测试 4
            if (statue == 0)// 正常0 维护1 测试为2
            {
                if (ceshi == 0)////0酷币版测试状态 1酷币版开放
                {
                    string CeshiQualification = Convert.ToString(ub.GetSub("CeshiQualification", "/Controls/winners.xml"));
                    if (CeshiQualification != "")
                    {
                        if (!("#" + CeshiQualification + "#").Contains("#" + UsId.ToString() + "#"))
                        {
                            rettr = 4;
                            return 4;
                        }
                    }
                }
            }
            #endregion

            #region 获取会员ID 5
            if (UsId == 0)//会员ID为空返回3
            {
                Match m;
                Match m1;
                string reg = "uid=[0-9]\\d*";
                string reg1 = "[0-9]\\d*";
                m = Regex.Match(strNote, reg);
                m1 = Regex.Match(m.Groups[0].ToString(), reg1);
                UsId = Convert.ToInt32(m1.Groups[0].ToString());
                rettr = 5;
                if (!new BCW.BLL.tb_WinnersLists().ExistsUserID(UsId))
                {
                    return 5;
                }
            }
            #endregion

            int awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;
            string typed = "";
            //识别奖池类并获得ID
            //0|游戏区奖池1|1|游戏区奖池2|2|游戏区奖池3|3|论坛奖池4|4|总奖池
            if (games3 == 0)//游戏
            {
                typed = "0";
                if (new BCW.BLL.tb_WinnersGame().ExistsGameName(games2))//游戏表存在游戏名字
                {
                    typed = (new BCW.BLL.tb_WinnersGame().GetPtype(games2)).ToString();
                    awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(typed);
                }
            }
            else if (games3 == 1)//论坛
            {
                typed = "3";
                awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("3");
            }
            else
            {
                typed = "4";
                awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType("4");
            }
            if (awardId < 0)
            { return 15; }

            #region 记录总数
            //记录进入量
            int awardId0 = awardId;
            BCW.Model.tb_WinnersAward award0 = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId0);
            award0.isDone = Convert.ToInt32(award0.isDone) + 1;
            new BCW.BLL.tb_WinnersAward().UpdateIsDone(awardId0, Convert.ToInt32(award0.isDone));
            #endregion
            //typed = (new BCW.BLL.tb_WinnersGame().GetPtype(games2)).ToString();
            //awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(typed);

            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);
            string[] getWinN = award.winNowCount.Split('#');//数据等奖数处理提出处理

            #region 是否数据错误 6
            if (games == "" || UsId == 0)
            {
                rettr = 6;
                return 6;
            }
            #endregion

            #region 生存新奖池 7
            if (award.awardNowCount == 0)
            {
                AddAward(award.getRedy);
                return 7;
            }
            #endregion

            #region 每天的最大次数 8
            if (getTimesForCount(UsId) >= maxGet)
            {
                rettr = 8;
                return 8;
            }
            #endregion

            #region start 中1 否 9 异常404
            Random ranop = new Random(unchecked((int)DateTime.Now.Ticks));
            int op = ranop.Next(1, Convert.ToInt32(award.periods));
            if (op < Convert.ToInt32(award.awardNumber) && op > 0)
            // if (true)
            {
                //该ID中奖
                rettr = 1;
                DateTime dt = DateTime.Now;
                int getWinNum = 0;

                string[] StrmaxgetWin = award.getWinNumber.Split('#');
                try
                {
                    getWinNum = getGetWinNumber(getWinNum, awardId);//获得随机等奖数
                    int maxgetWin = Convert.ToInt32(award.winNumber);
                    //if (getWinNum == 1)
                    //{
                    //    getWinNum = getGetWinNumber(8, awardId);//获得随机等奖数 
                    //}
                }
                catch { return 4; }

                BCW.Model.tb_WinnersLists lis = new BCW.Model.tb_WinnersLists();//中奖表
                lis.UserId = UsId;// 记录玩家ID 
                lis.awardId = award.Id;//记录奖池表的ID                        
                lis.AddTime = dt;//当前时间，记录中奖时间，判断是否过期
                lis.FromId = ID;//记录Action表的Id
                lis.GameName = Types.ToString();//游戏标识的ID
                lis.GetId = getWinNum;//获得的等奖类型,1等2等3等...
                string nowGetWInNumber = losGetWinNumber(getWinNum, awardId);//#等奖数减1
                int getGold = getGetWinNowCount(getWinNum, awardId);//获得等奖对应的币值
                lis.Ident = 1;//标识1，未领奖
                lis.isGet = 1;//每人每天最大允许次数
                lis.overTime = PassTime;//逾期时间,后台设置(小时)
                lis.Remarks = games;//来源指示说明
                lis.TabelId = games3;//记录Action表的NodeId，对应该游戏的记录Id
                lis.winGold = getGold;//等奖的钱数             
                //中奖者增加一行
                int ID1 = new BCW.BLL.tb_WinnersLists().Add(lis);
                //增动态，增加lists表记录中奖ID
                int meid = UsId;
                string mename = UsName;
                string gessText = TextForUbb;
                //new BCW.BLL.Guest.Add(0, meid, mename, gessText);//发内线到该ID
                //对中奖ID进行处理，删除数据库中已取出的Id,放回数据库中    
                //award.getUsId = delStr(id.ToString(), award.getUsId.ToString());//去除ID
                // model.isDone -= 1;
                award.awardNowCount -= 1;//当前-1
                award.getWinNumber = nowGetWInNumber;
                //放回
                new BCW.BLL.tb_WinnersAward().Update(award);
                int AllAwardCountNow = Convert.ToInt32(ub.GetSub("AllAwardCountNow", xmlPath));
                try
                {
                    string xmlPath1 = "/Controls/winners.xml";
                    int ccoun = (AllAwardCountNow - getGold);
                    ub xml = new ub();
                    xml.ReloadSub(xmlPath1); //加载配置
                    xml.dss["AllAwardCountNow"] = ccoun;
                    if (ccoun <= 0)//派完自动停止
                    {
                        WinnersOpenOrClose = "0";
                        ccoun = 0;
                    }
                    //if (getWinNum == 1 && getGold > 100000)
                    //{
                    //    xml.dss["WinnersGonggao"] += UsName + games+"[br]";
                    //}
                    xml.dss["AllAwardCountNow"] = ccoun;
                    if (getWinNum == 1 && getGold > 200000)
                    {
                        xml.dss["WinnersNotes"] = "[br]上轮得主:[url=/bbs/uinfo.aspx?uid=" + UsId + "]" + UsName + "[/url]" + games + "获得" + getAwardID(getWinNum) + "等奖,价值[绿]" + getGold + "[/绿]" + ub.Get("SiteBz") + "[br]";
                    }
                    xml.dss["WinnersOpenOrClose"] = WinnersOpenOrClose;//0|停止放送机会|1|开启放送机会
                    System.IO.File.WriteAllText(HttpContext.Current.Server.MapPath(xmlPath1), xml.Post(xml.dss), System.Text.Encoding.UTF8);
                }
                catch { }
                //  new BCW.BLL.Guest().Add(0, UsId, UsName, TextForUbb);
                return rettr;
            }
            else
            {
                rettr = 10;
                return 10;
            }
            #endregion
        }

        #region 获得等奖中文
        //获得等奖 一二三...
        private string getAwardID(int i)
        {
            switch (i)
            {
                case 1:
                    return "一";
                case 2:
                    return "二";
                case 3:
                    return "三";
                case 4:
                    return "四";
                case 5:
                    return "五";
                case 6:
                    return "六";
                case 7:
                    return "七";
                case 8:
                    return "八";
                case 9:
                    return "九";
                case 10:
                    return "十";
                case 11:
                    return "十一";
                case 12:
                    return "十二";
                case 13:
                    return "十三";
                case 14:
                    return "十四";
                case 15:
                    return "十五";
                case 16:
                    return "十六";
                case 17:
                    return "十七";
                case 18:
                    return "十八";
                default:
                    return "一";
            }
        }
        #endregion


        #region 生成新一轮奖池数据
        private void AddAward(string type)
        {
            /// <summary>
            /// 生成最新一期奖池数据
            /// </summary>
            /// 
            try
            {

                int MaxCount = Convert.ToInt32(ub.GetSub("MaxCount", xmlPath));//设置下一轮奖池最大份数
                int paisong = Convert.ToInt32(ub.GetSub("paisong", xmlPath));//设置下一轮奖池派送份数
                int CountList = Convert.ToInt32(ub.GetSub("CountList", xmlPath));//设置等奖数
                int baifenbi = Convert.ToInt32(ub.GetSub("baifenbi", xmlPath));//玩家中奖百分比
                string CountListNumber = (ub.GetSub("CountListNumber", xmlPath));//设置每等奖份数
                string ListIGold = (ub.GetSub("ListIGold", xmlPath));//设置每等奖金额
                int WinnersOpenChoose = Convert.ToInt32(ub.GetSub("WinnersOpenChoose", xmlPath));//开放选择（int）
                string TextForUbb = (ub.GetSub("TextForUbb", xmlPath));//设置内线提示的文字
                int PassTime = Convert.ToInt32(ub.GetSub("PassTime", xmlPath));//设置领取过期时间(天)
                //  int nowId = id;// new BCW.BLL.Action().GetMaxId() - 1;//当前ACTion最大ID
                Random ran = new Random();
                string getUsId = "";//生成的中奖ID
                int awardNumber1 = paisong;
                ub xml = new ub();
                xml.ReloadSub(xmlPath); //加载配置
                if (Convert.ToInt32(type) == 0)//加载奖池1配置
                {
                    PassTime = Convert.ToInt32(xml.dss["PassTime1"]);
                    MaxCount = Convert.ToInt32(xml.dss["MaxCount1"]);
                    paisong = Convert.ToInt32(xml.dss["paisong1"]);
                    CountList = Convert.ToInt32(xml.dss["CountList1"]);
                    CountListNumber = xml.dss["CountListNumber1"].ToString();
                    ListIGold = xml.dss["ListIGold1"].ToString();
                }
                if (Convert.ToInt32(type) == 1)//加载奖池2配置
                {
                    PassTime = Convert.ToInt32(xml.dss["PassTime2"]);
                    MaxCount = Convert.ToInt32(xml.dss["MaxCount2"]);
                    paisong = Convert.ToInt32(xml.dss["paisong2"]);
                    CountList = Convert.ToInt32(xml.dss["CountList2"]);
                    CountListNumber = xml.dss["CountListNumber2"].ToString();
                    ListIGold = xml.dss["ListIGold2"].ToString();
                }
                if (Convert.ToInt32(type) == 2)//加载奖池3配置
                {
                    PassTime = Convert.ToInt32(xml.dss["PassTime3"]);
                    MaxCount = Convert.ToInt32(xml.dss["MaxCount3"]);
                    paisong = Convert.ToInt32(xml.dss["paisong3"]);
                    CountList = Convert.ToInt32(xml.dss["CountList3"]);
                    CountListNumber = xml.dss["CountListNumber3"].ToString();
                    ListIGold = xml.dss["ListIGold3"].ToString();
                }
                if (Convert.ToInt32(type) == 3)//加载奖池4配置
                {
                    PassTime = Convert.ToInt32(xml.dss["PassTime4"]);
                    MaxCount = Convert.ToInt32(xml.dss["MaxCount4"]);
                    paisong = Convert.ToInt32(xml.dss["paisong4"]);
                    CountList = Convert.ToInt32(xml.dss["CountList4"]);
                    CountListNumber = xml.dss["CountListNumber4"].ToString();
                    ListIGold = xml.dss["ListIGold4"].ToString();
                }
                if (Convert.ToInt32(type) == 4)//加载奖池5配置
                {
                    PassTime = Convert.ToInt32(xml.dss["PassTime5"]);
                    MaxCount = Convert.ToInt32(xml.dss["MaxCount5"]);
                    paisong = Convert.ToInt32(xml.dss["paisong5"]);
                    CountList = Convert.ToInt32(xml.dss["CountList5"]);
                    CountListNumber = xml.dss["CountListNumber5"].ToString();
                    ListIGold = xml.dss["ListIGold5"].ToString();
                }

                if (!new BCW.BLL.tb_WinnersAward().Exists(1))//不存在数据直接生成
                {
                    BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                    string[] getWinN = CountListNumber.Split('#');//数据等奖数处理提出处理
                    tempAward.periods = MaxCount;//奖池总数
                    tempAward.addTime = DateTime.Now;//开始时间
                    tempAward.awardNumber = paisong;//最大派出量
                    tempAward.awardNowCount = paisong;//当前派出剩余量
                    tempAward.award = ListIGold + "#";//设置每等奖份数  100#50#20...数量
                    tempAward.getRedy = type;//012类型 游戏12345奖池 论坛奖池 总奖池 0 1 2 3 4
                    tempAward.getUsId = "0";//获奖者，派送后递减ID
                    tempAward.getWinNumber = CountListNumber + "#";//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                    tempAward.identification = PassTime;//过期标识（单位小时）
                    tempAward.isDone = 0;//
                    tempAward.isGet = 0;//
                    tempAward.overTime = DateTime.Now;
                    tempAward.Remarks = TextForUbb;//发内线的语句
                    tempAward.winNowCount = CountListNumber;//每等奖份数 不变
                    tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                    int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存
                }
                if (new BCW.BLL.tb_WinnersAward().Exists(1))//存在奖池数据
                {
                    int awardId = new BCW.BLL.tb_WinnersAward().GetMaxAwardForType(type);
                    //awardId = new BCW.BLL.tb_WinnersAward().GetMaxId() - 1;//奖池表当前最大ID   
                    BCW.Model.tb_WinnersAward model = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(awardId);//奖池列   
                    BCW.Model.tb_WinnersAward tempAward = new BCW.Model.tb_WinnersAward();//奖池列
                    string[] getWinN = CountListNumber.Split('#');//数据等奖数处理提出处理
                    tempAward.periods = MaxCount;//奖池总数
                    tempAward.addTime = DateTime.Now;//开始时间
                    tempAward.awardNumber = paisong;//最大派出量
                    tempAward.awardNowCount = paisong;//当前派出剩余量
                    tempAward.award = ListIGold + "#";//设置每等奖份数  100#50#20...数量
                    tempAward.getRedy = type;//012类型 游戏奖池 论坛奖池 总奖池 0  1  2  3  4
                    tempAward.getUsId = getUsId;//获奖者，派送后递减ID
                    tempAward.getWinNumber = CountListNumber;//设置每等奖份数  数量,便于统计剩余等奖量 统计剩余用
                    tempAward.identification = PassTime;//过期标识（单位天）
                    tempAward.isDone = 0;//
                    tempAward.isGet = 0;//当前ACTion最大ID
                    tempAward.overTime = DateTime.Now;
                    tempAward.Remarks = TextForUbb;//发内线的语句
                    tempAward.winNowCount = CountListNumber;//每等奖份数 不变
                    tempAward.winNumber = CountList;//等奖份数，5即5等奖  
                    try
                    {
                        int a = new BCW.BLL.tb_WinnersAward().Add(tempAward);//生成的新的中奖码保存

                        string gessText = "下一轮生成奖池数据成功，当前奖池ID" + a;
                        //  new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                    }
                    catch
                    {
                        string gessText = "下一轮生成奖池数据失败！002";
                        //  new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);
                    }
                }
            }
            catch (Exception ee)
            {
                string gessText = "新生成奖池数据失败！001";
                //  new BCW.BLL.Guest().Add(0, 727, "测试727", gessText);//异常报错
            }
        }
        #endregion

        /// <summary>
        ///后台控制每人每天最大获奖次数
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="NodeId"></param>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected int getTimesForCount(int UsId)
        {
            // BCW.Model.tb_WinnersLists last = new BCW.BLL.tb_WinnersLists().GetLastIsGet(UsId);
            //if (last.isGet > maxGet && Convert.ToDateTime(last.AddTime).Date == DateTime.Now.Date)//判断次数
            //{
            //    return 1;//超次
            //}
            //else
            //{
            //    return 2;
            //}
            string strwhere = "UserId=" + UsId + "and  DateDiff(dd,AddTime,getdate())=0";
            DataSet ds = new BCW.BLL.tb_WinnersLists().GetList("COUNT(*) as counts", strwhere);
            int count = 0;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                // for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    count = Convert.ToInt32(ds.Tables[0].Rows[0]["counts"]);
                }
            }
            return count;
        }

        /// <summary>
        ///获取对应的等奖金额
        /// </summary>
        /// <param name="Types"></param>
        /// <param name="NodeId"></param>
        /// <param name="UsId"></param>
        /// <param name="UsName"></param>
        /// <param name="Notes"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        protected int getGetWinNowCount(int i, int ID)
        {
            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
            string[] getWinN = award.award.Split('#');//数据等奖数处理提出处理
            int get = 0;
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                if (k == (i - 1))//识别位置-1操作
                {
                    get = (Convert.ToInt32(getWinN[k]));
                }
            }
            return get;
            // new BCW.BLL.tb_WinnersAward().Update(award);

        }

        /// <summary>
        ///获取当前对应的1等奖数
        /// </summary>
        protected int getGetWinNumberOne(int i, int ID)
        {
            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
            string[] getWinN = award.getWinNumber.Split('#');//数据等奖数处理提出处理
            ArrayList list = new ArrayList();
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                list.Add(Convert.ToInt32(getWinN[k]));
            }
            int kk = Convert.ToInt32(list[i]);
            return kk;
        }

        /// <summary>
        ///获取当前对应的系统设置的固定1等奖数
        /// </summary>
        protected int getGetWinNumberOneforWinNowCount(int i, int ID)
        {
            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
            string[] getWinN = award.winNowCount.Split('#');//数据等奖数处理提出处理
            ArrayList list = new ArrayList();
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                list.Add(Convert.ToInt32(getWinN[k]));
            }
            int kk = Convert.ToInt32(list[i]);
            return kk;
        }

        /// <summary>
        ///获取对应的等奖数
        /// </summary>
        protected int getGetWinNumber(int i, int ID)
        {
            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
            string[] getWinN = award.getWinNumber.Split('#');//数据等奖数处理提出处理
            int jl = 1;
            int kk = 0;
            ArrayList list = new ArrayList();
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                if (Convert.ToInt32(getWinN[k]) > 0)
                {
                    for (int j = 1; j <= Convert.ToInt32(getWinN[k]); j++)
                    {
                        list.Add(jl);
                    }
                }
                jl++;
            }
            ArrayList list2 = GetRandomLisint(list);
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            int index = ran1.Next(0, list2.Count);
            kk = Convert.ToInt32(list2[index]);
            list.Clear();
            return kk;
        }

        /// <summary>
        /// 打乱等奖数组操作
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        public static ArrayList GetRandomLisint(ArrayList inputList)
        {
            //Copy to a array
            int[] copyArray = new int[inputList.Count];
            inputList.CopyTo(copyArray);

            //Add range
            ArrayList copyList = new ArrayList();
            copyList.AddRange(copyArray);

            //Set outputList and random
            ArrayList outputList = new ArrayList();
            Random rd = new Random(DateTime.Now.Millisecond);

            while (copyList.Count > 0)
            {
                //Select an index and item
                int rdIndex = rd.Next(0, copyList.Count - 1);
                int remove = Convert.ToInt32(copyList[rdIndex]);

                //remove it from copyList and add it to output
                copyList.Remove(remove);
                outputList.Add(remove);
            }
            return outputList;
        }

        /// <summary>
        ///等奖数减一操作
        /// </summary>
        protected string losGetWinNumber(int i, int ID)
        {
            BCW.Model.tb_WinnersAward award = new BCW.BLL.tb_WinnersAward().Gettb_WinnersAward(ID);//最新一轮的
            string[] getWinN = award.getWinNumber.Split('#');//数据等奖数处理提出处理
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                if (k == (i - 1))//识别位置-1操作
                {
                    if ((Convert.ToInt32(getWinN[k]) > 0))
                    {
                        getWinN[k] = (Convert.ToInt32(getWinN[k]) - 1).ToString();
                    }
                    else
                    {
                        getWinN[k] = "0";
                    }
                }
            }
            award.getWinNumber = "";
            for (int k = 0; k < getWinN.Length - 1; k++)
            {
                award.getWinNumber += getWinN[k] + "#";
            }
            return award.getWinNumber;
            // new BCW.BLL.tb_WinnersAward().Update(award);

        }

        /// <summary>
        /// 去除已派出去的一个
        /// </summary>
        /// <param name="one"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        protected string delStr(string one, string all)
        {
            string b = all.Replace(one + ",", "");
            return b;
        }

        /// <summary>
        ///生成是否重复
        /// </summary>
        protected bool isGet(string r, string yungouma)
        {
            bool b = true;
            if (yungouma == "")
            { return b; }
            else
            {
                string[] yun = yungouma.Split(',');
                foreach (string j in yun)
                {
                    //int temp = int.Parse(i);
                    if (j.ToString() == r.ToString())
                    {
                        b = false;
                    }
                }
                return b;
            }

        }

    }
}
