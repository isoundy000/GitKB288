using System;
using System.Collections.Generic;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using BCW.Common;
using BCW.farm;
using BCW.Files;
using System.Text;
using System.Security.Cryptography;
using System.IO;

//姚志光 20160621 增加活跃抽奖入口控制

/// <summary>
/// 邵广林 20160721 
/// 修改施肥跳转时间为1s,刷新机改为15s一次------防止跳转时，刷新机去操作
/// 修改message的更新时间为数据库自动更新-------防止先收割再施肥的时间出错(若不行,把刷新机放服务器)
/// 修改从第一块土地到最后一块土地时间递增问题
/// 邵广林 20160722
/// 修改施肥跳转时间为3s，刷新机2s刷一次
/// 增加施肥跳转后，首页查询是否为施肥返回，如果是，则判断字段是否为1，为1时则延时0.5秒
/// 邵广林 20160730 增加所有操作防刷
/// </summary>

public partial class bbs_game_farm : System.Web.UI.Page
{
    protected System.Text.StringBuilder builder = new System.Text.StringBuilder("");
    protected string xmlPath = "/Controls/farm.xml";

    #region 游戏xml
    protected string GameName = ub.GetSub("FarmName", "/Controls/farm.xml");//游戏名字
    protected string XtestID = (ub.GetSub("XtestID", "/Controls/farm.xml"));//试玩ID
    protected float vipprice = float.Parse(ub.GetSub("vipprice", "/Controls/farm.xml"));//vip优惠
    protected int bigbuynum = Convert.ToInt32(ub.GetSub("bigbuynum", "/Controls/farm.xml"));//最大购买量
    protected int xExpir = Convert.ToInt32(ub.GetSub("xExpir", "/Controls/farm.xml"));//刷新
    protected int tou_renshu = Convert.ToInt32(ub.GetSub("tou_renshu", "/Controls/farm.xml"));//偷取的人数
    protected int bxys_num = Convert.ToInt32(ub.GetSub("bxys_num", "/Controls/farm.xml"));//宝箱所需的钥匙数量
    protected int yaoqing_jinbi = Convert.ToInt32(ub.GetSub("yaoqing_jinbi", "/Controls/farm.xml"));//邀请好友获得金币数
    protected int hecheng_num = Convert.ToInt32(ub.GetSub("hecheng_num", "/Controls/farm.xml"));//合成的数量
    protected int xExpir_huafei = Convert.ToInt32(ub.GetSub("xExpir_huafei", "/Controls/farm.xml"));//化肥防刷次数
    protected int zs_num = Convert.ToInt32(ub.GetSub("zs_num", "/Controls/farm.xml"));//可赠送个数
    protected string stop_IP = (ub.GetSub("stop_IP", "/Controls/farm.xml"));//禁止访问IP

    protected int slave_day = Convert.ToInt32(ub.GetSub("slave_day", "/Controls/farm.xml"));//奴隶过期天数
    protected int slave_num = Convert.ToInt32(ub.GetSub("slave_num", "/Controls/farm.xml"));//奴隶惩罚和安抚次数
    protected int slave_jingyan_me = Convert.ToInt32(ub.GetSub("slave_jingyan_me", "/Controls/farm.xml"));//惩罚经验
    protected int slave_jinbi_me = Convert.ToInt32(ub.GetSub("slave_jinbi_me", "/Controls/farm.xml"));//惩罚金币
    protected int slave_jingyan_he = Convert.ToInt32(ub.GetSub("slave_jingyan_he", "/Controls/farm.xml"));//安抚经验
    protected int slave_jinbi_he = Convert.ToInt32(ub.GetSub("slave_jinbi_he", "/Controls/farm.xml"));//安抚金币
    protected int xianjing_num = Convert.ToInt32(ub.GetSub("xianjing_num", "/Controls/farm.xml"));//陷阱个数
    protected int xianjing_dengji = Convert.ToInt32(ub.GetSub("xianjing_dengji", "/Controls/farm.xml"));//陷阱所需等级
    protected int xianjing_jinbi = Convert.ToInt32(ub.GetSub("xianjing_jinbi", "/Controls/farm.xml"));//陷阱所需金币
    protected int xianjing_jb = Convert.ToInt32(ub.GetSub("xianjing_jb", "/Controls/farm.xml"));//踩中陷阱所扣和所得的金币
    protected int xianjing_jilv = Convert.ToInt32(ub.GetSub("xianjing_jilv", "/Controls/farm.xml"));//踩中陷阱的机率
    protected int xianjing_day = Convert.ToInt32(ub.GetSub("xianjing_day", "/Controls/farm.xml"));//陷阱所用的钥匙最大使用天数

    protected int qd_jishu = Convert.ToInt32(ub.GetSub("qd_jishu", "/Controls/farm.xml"));//签到酷币奇数
    protected int qd_jishu_jinbi = Convert.ToInt32(ub.GetSub("qd_jishu_jinbi", "/Controls/farm.xml"));//签到金币奇数
    protected int bx_jishu = Convert.ToInt32(ub.GetSub("bx_jishu", "/Controls/farm.xml"));//宝箱酷币奇数
    protected int bx_jishu_jinbi = Convert.ToInt32(ub.GetSub("bx_jishu_jinbi", "/Controls/farm.xml"));//宝箱金币奇数

    protected int baitan_bignum = Convert.ToInt32(ub.GetSub("baitan_bignum", "/Controls/farm.xml"));//最大摆摊数量
    protected float baitan_koushui = float.Parse(ub.GetSub("baitan_koushui", "/Controls/farm.xml"));//摆摊扣税
    protected int baitan_day = Convert.ToInt32(ub.GetSub("baitan_day", "/Controls/farm.xml"));//摆摊过期天数

    protected int chucao1_grade = Convert.ToInt32(ub.GetSub("chucao1_grade", "/Controls/farm.xml"));//一键除草所需等级
    protected long chucao1_jinbi = Convert.ToInt64(ub.GetSub("chucao1_jinbi", "/Controls/farm.xml"));//一键除草所需金币

    protected int jiaoshui1_grade = Convert.ToInt32(ub.GetSub("jiaoshui1_grade", "/Controls/farm.xml"));//一键浇水所需等级
    protected int jiaoshui1_jinbi = Convert.ToInt32(ub.GetSub("jiaoshui1_jinbi", "/Controls/farm.xml"));//一键浇水所需金币

    protected int chuchong1_grade = Convert.ToInt32(ub.GetSub("chuchong1_grade", "/Controls/farm.xml"));//一键除虫所需等级
    protected int chuchong1_jinbi = Convert.ToInt32(ub.GetSub("chuchong1_jinbi", "/Controls/farm.xml"));//一键除虫所需金币

    protected int shouhuo1_grade = Convert.ToInt32(ub.GetSub("shouhuo1_grade", "/Controls/farm.xml"));//一键收获所需等级
    protected int shouhuo1_jinbi = Convert.ToInt32(ub.GetSub("shouhuo1_jinbi", "/Controls/farm.xml"));//一键收获所需金币

    protected int chandi1_grade = Convert.ToInt32(ub.GetSub("chandi1_grade", "/Controls/farm.xml"));//一键耕地所需等级
    protected int chandi1_jinbi = Convert.ToInt32(ub.GetSub("chandi1_jinbi", "/Controls/farm.xml"));//一键耕地所需金币

    protected int shifei1_grade = Convert.ToInt32(ub.GetSub("shifei1_grade", "/Controls/farm.xml"));//一键施肥所需等级
    protected int shifei1_jinbi = Convert.ToInt32(ub.GetSub("shifei1_jinbi", "/Controls/farm.xml"));//一键施肥所需金币

    protected int all_jingyan = Convert.ToInt32(ub.GetSub("all_jingyan", "/Controls/farm.xml"));//all得到经验
    protected int all_jinbi = Convert.ToInt32(ub.GetSub("all_jinbi", "/Controls/farm.xml"));//all得到金币
    protected int chandi_jingyan = Convert.ToInt32(ub.GetSub("chandi_jingyan", "/Controls/farm.xml"));//铲地得到经验
    protected int shifei_jingyan = Convert.ToInt32(ub.GetSub("shifei_jingyan", "/Controls/farm.xml"));//施肥得到经验
    protected int zhong_jingyan = Convert.ToInt32(ub.GetSub("zhong_jingyan", "/Controls/farm.xml"));//种植经验

    protected int zcaofchong_jingyan = Convert.ToInt32(ub.GetSub("zcaofchong_jingyan", "/Controls/farm.xml"));//种草放虫得到经验
    protected int zcaofchong_jinbi = Convert.ToInt32(ub.GetSub("zcaofchong_jinbi", "/Controls/farm.xml"));//种草放虫得到金币

    protected int bz_big_jingyan = Convert.ToInt32(ub.GetSub("bz_big_jingyan", "/Controls/farm.xml"));//种植最大经验
    protected int bm_big_jingyan = Convert.ToInt32(ub.GetSub("bm_big_jingyan", "/Controls/farm.xml"));//帮忙好友最大经验
    protected int sh_big_jingyan = Convert.ToInt32(ub.GetSub("sh_big_jingyan", "/Controls/farm.xml"));//种草放虫最大经验
    protected int zj_big_jingyan = Convert.ToInt32(ub.GetSub("zj_big_jingyan", "/Controls/farm.xml"));//自己农场操作最大经验
    protected int big_zcishu = Convert.ToInt32(ub.GetSub("big_zcishu", "/Controls/farm.xml"));//最大种草放虫次数
    protected int big_ccishu = Convert.ToInt32(ub.GetSub("big_ccishu", "/Controls/farm.xml"));//最大除草除虫次数
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        //维护提示
        if (ub.GetSub("farmStatus", xmlPath) == "1")
        {
            Utils.Safe("此游戏");
        }

        if (ub.GetSub("farmStatus", xmlPath) == "2")//内测
        {
            if (XtestID != "")
            {
                string[] sNum = XtestID.Split('#');
                int sbsy = 0;
                for (int a = 0; a < sNum.Length; a++)
                {
                    int tid = 0;
                    int.TryParse(sNum[a].Trim(), out tid);
                    if (meid == tid)
                    {
                        sbsy++;
                    }
                }
                if (sbsy == 1)
                {
                    Utils.Error("你没获取测试此游戏的资格，请与客服联系，谢谢。", "");
                }
            }
        }

        //stop_IP
        if (stop_IP != "")
        {
            string[] sNum = stop_IP.Split('#');
            int sbsy = 0;
            string iplogin = Utils.GetUsIP();//IP
            string browser = Utils.GetUA().ToLower();//browser
            for (int a = 0; a < sNum.Length; a++)
            {
                if (iplogin == sNum[a])
                {
                    sbsy++;
                }
                if (sbsy == 1)
                {
                    Utils.Error("抱歉,你访问农场过于频繁.请稍候在试.", "");
                }
            }
        }

        string act = Utils.GetRequest("act", "all", 1, "", "");
        int ptype = 1;
        int nb = 0;

        //测试用----后台测试ID如果有52784，则测试谁刷农场
        try
        {
            //if (act != "")
            {
                if (XtestID != "")
                {
                    string[] sNum = XtestID.Split('#');
                    int sbsy = 0;
                    for (int a = 0; a < sNum.Length; a++)
                    {
                        int tid = 0;
                        int.TryParse(sNum[a].Trim(), out tid);
                        if (52784 == tid)
                        {
                            sbsy++;
                        }
                        if (sbsy == 1)
                        {
                            BCW.farm.Model.NC_record gg = new BCW.farm.Model.NC_record();
                            gg.UsID = meid;
                            gg.AddTime = DateTime.Now;
                            gg.IP = Utils.GetUsIP();
                            gg.text = "" + meid + "进入农场act为：" + act + "";
                            gg.Browser = Utils.GetUA().ToLower(); //Utils.GetBrowser();
                            new BCW.farm.BLL.NC_record().Add(gg);
                        }
                    }
                }
            }
        }
        catch { }

        //仓库摆摊取消后返回
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "取消摆摊")
        {
            ptype = 3;//回到仓库的道具
            act = "cangku";
        }
        //购买道具成功后返回
        int vb = int.Parse(Utils.GetRequest("vb", "all", 1, "", "0"));//购买道具判断：1为宝箱
        if (vb == 1)
        {
            nb = 1;
        }
        //检查是否已有农场
        if (!new BCW.farm.BLL.NC_user().Exists(meid))
        {
            act = "";
        }

        switch (act)
        {
            case "market":
                marketPage();//交易市场
                break;
            case "market_tw":
                market_twPage();//我的摊位
                break;
            case "market_Ta":
                market_TaPage();//Ta的摊位
                break;
            case "NG_jiaoyi":
                NG_jiaoyiPage();//取消摆摊
                break;
            case "market_buy":
                market_buyPage();//交易市场购买
                break;
            case "punish":
                PunishPage();//我的奴隶
                break;
            case "nlcf":
                nlcfPage();//惩罚奴隶界面
                break;
            case "just":
                justPage();//奴隶的安抚和惩罚
                break;
            case "xianjing":
                XianjingPage();//陷阱界面
                break;
            case "setxj":
                setxjPage();//设置陷阱
                break;
            case "baoxiang":
                baoxiangPage();//宝箱界面
                break;
            case "openbx":
                openbxPage();//开启宝箱
                break;
            case "dengji":
                dengjiPage();//等级说明
                break;
            case "toucai":
                ToucaiPage();//偷菜
                break;
            case "faneixian":
                faneixianPage();//加好友发内线
                break;
            case "do":
                doPage();//去偷菜人的页面
                break;
            case "allsee":
                allseePage();//到处看看
                break;
            case "shangdian":
                ShangdianPage();//商店
                break;
            case "fast":
                fastPage();//快速购买_种子
                break;
            case "fast2":
                fast2Page();//快速购买_道具
                break;
            case "qiandao":
                QiandiaoPage();//签到
                break;
            case "qiandaolook":
                qiandaolookPage();//查看签到奖励
                break;
            case "message":
                MessagePage();//个人信息
                break;
            case "cangku":
                CangkuPage(ptype);//仓库
                break;
            case "hecheng":
                hechengPage();//合成
                break;
            case "use_daoju":
                use_daojuPage();//使用道具
                break;
            case "jiasuo":
                jiasuoPage();//加锁
                break;
            case "jiesuo":
                jiesuoPage();//解锁
                break;
            case "sell":
                sellPage();//单个卖出
                break;
            case "sellall":
                sellallPage();//全部卖出
                break;
            case "gongnegn":
                gongnegnPage();//功能
                break;
            case "paihangban":
                PaihangbanPage();//排行榜
                break;
            case "chucao":
                ChucaoPage();//除草
                break;
            case "chucao2":
                Chucao2Page();//(偷)除草
                break;
            case "chucao_1":
                chucao_1Page();//一键除草
                break;
            case "chucao_2":
                chucao_2Page();//一键(偷)除草
                break;
            case "jiaoshui":
                JiaoshuiPage();//浇水
                break;
            case "jiaoshui2":
                Jiaoshui2Page();//(偷)浇水
                break;
            case "jiaoshui_1":
                jiaoshui_1Page();//一键浇水
                break;
            case "jiaoshui_2":
                jiaoshui_2Page();//一键(偷)浇水
                break;
            case "zcao1":
                zcao1Page();//种草
                break;
            case "fchong1":
                fchong1Page();//放虫
                break;
            case "fangcao_2":
                fangcao_2Page();//一键(偷)种草、放虫
                break;
            case "chuchong":
                chuchongPage();//除虫
                break;
            case "chuchong2":
                chuchong2Page();//(偷)除虫
                break;
            case "chuchong_1":
                chuchong_1Page();//一键除虫
                break;
            case "chuchong_2":
                chuchong_2Page();//一键(偷)除虫
                break;
            case "shouhuo":
                shouhuoPage();//收获
                break;
            case "shouhuo2":
                shouhuo2Page();//(偷)菜
                break;
            case "shouhuo_1":
                shouhuo_1Page();//一键收获
                break;
            case "shouhuo_2":
                shouhuo_2Page();//一键(偷)菜
                break;
            case "chandi":
                chandiPage();//铲地
                break;
            case "chandi_1":
                chandi_1Page();//一键耕地
                break;
            case "shifei":
                shifeiPage();//施肥
                break;
            case "shifei_case":
                shifei_casePage();//执行施肥
                break;
            case "shifei_1":
                shifei_1Page();//一键施肥
                break;
            case "shifei_case2":
                shifei_case2Page();//执行一键施肥
                break;
            case "bozhong":
                bozhongPage();//播种
                break;
            case "zhongzhi":
                zhongzhiPage();//种植
                break;
            case "bozhong_1":
                bozhong_1Page();//一键播种
                break;
            case "zhongzhi_1":
                zhongzhi_1Page();//一键种植
                break;
            case "kaitong":
                kaitongPage();//开通 
                break;
            case "zengsong":
                zengsongPage();//赠送
                break;
            case "buycase":
                PayPage(nb);//购买道具
                break;
            case "gonggao":
                gonggaoPage();//公告
                break;
            case "task":
                taskPage();//任务活动
                break;
            case "taskok":
                taskokPage();//任务完成
                break;
            case "Currency":
                CurrencyPage();//换币
                break;
            case "liuyan":
                liuyanPage();//留言
                break;
            case "add_bbs":
                add_bbsPage();//增加留言
                break;
            case "yaoqing":
                yaoqingPage();//发邀请
                break;
            case "liuyao_list":
                liuyao_listPage();//留言查看
                break;
            case "jiyu":
                jiyuPage();//农场寄语
                break;
            case "zhitiao":
                zhitiaoPage();//纸条
                break;
            default:
                ReloadPage();//首页
                break;
        }
    }

    //收割机
    private void is_shougeji(int meid)
    {
        DataSet ds1 = new BCW.farm.BLL.NC_daoju_use().GetList("*", "type=1 AND (daoju_id=23 or daoju_id=24) and usid=" + meid + "");
        if (ds1 != null && ds1.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
            {
                int UsID2 = int.Parse(ds1.Tables[0].Rows[j]["usid"].ToString());//用户id

                //new BCW.farm.BLL.NC_user().update_zd("shoutype=1", "usid=" + UsID2 + " and shoutype=0");
                //if (new BCW.farm.BLL.NC_user().Getshoutype(UsID2) != 1)
                //{
                //    break;
                //}

                string mename = new BCW.BLL.User().GetUsName(UsID2);//用户姓名
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + UsID2 + " and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    int zuowu_experience = 0;
                    string[] ab = new string[2];
                    string zuowu = string.Empty;
                    string xx = string.Empty;
                    int yy = 0;
                    string jysm = string.Empty;//文字说明
                    string td_num = string.Empty;//统一修改作物成熟收割时间  邵广林 20160721
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string output = string.Empty;
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                        zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                        int zuowu_ji = int.Parse(ds.Tables[0].Rows[i]["zuowu_ji"].ToString());//作物生长的季度
                        output = ds.Tables[0].Rows[i]["output"].ToString();//产/剩
                        zuowu_experience = int.Parse(ds.Tables[0].Rows[i]["zuowu_experience"].ToString());//作物经验
                        int harvest = int.Parse(ds.Tables[0].Rows[i]["harvest"].ToString());//收获季度
                        int zuowu_time = int.Parse(ds.Tables[0].Rows[i]["zuowu_time"].ToString());//作物生长需要时间(分钟)

                        if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo(id, UsID))//是否存在可以收获的土地
                        {
                            if (zuowu_ji >= harvest)
                            {
                                ab = output.Split(',');
                                //双倍经验判断
                                int jy = 0;//经验
                                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(UsID, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(UsID, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(UsID, 27)))
                                {
                                    jy = zuowu_experience * 2;
                                    jysm = "由于你使用了自动收割机,作物自动收获,且有双倍经验卡,经验翻倍.";
                                }
                                else
                                {
                                    jy = zuowu_experience;
                                    jysm = "由于你使用了自动收割机,作物自动收获.";
                                }

                                if ((zuowu_ji - harvest) == 0)//种植季度=收获季度,可以铲地
                                {
                                    new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,ischandi=2", " usid = '" + UsID + "' AND tudi='" + id + "'");//可以铲地
                                    new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                    BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                    qq.name = zuowu;
                                    qq.num = int.Parse(ab[1]);
                                    BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                    qq.price_out = w.price_out;
                                    qq.name_id = w.num_id;
                                    qq.suoding = 0;
                                    qq.usid = UsID;
                                    qq.get_nums = int.Parse(ab[1]);
                                    qq.tou_nums = 0;
                                    if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                    }
                                    else
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                    }
                                }
                                else//收获季度+1,更新种植时间
                                {
                                    td_num = td_num + id + ",";
                                    int b = harvest + 1;
                                    if (b == 2)//如果等于第2个季度，就把时间减少
                                    {
                                        new BCW.farm.BLL.NC_tudi().update_tudi("zuowu_time=" + (zuowu_time * 2 / 5) + "", "usid = '" + UsID + "' AND tudi='" + id + "'");
                                    }
                                    string nn = ab[0] + "," + ab[0];
                                    new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,z_shuinum='',caoID='',chongID='',touID='',output='" + nn + "',isshifei=0,harvest=" + b + "", "usid = '" + UsID + "' AND tudi='" + id + "'");// , updatetime='" + DateTime.Now + "'
                                    new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                    BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                    qq.name = zuowu;
                                    qq.num = int.Parse(ab[1]);
                                    BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                    qq.price_out = w.price_out;
                                    qq.name_id = w.num_id;
                                    qq.usid = UsID;
                                    qq.get_nums = int.Parse(ab[1]);
                                    qq.tou_nums = 0;
                                    if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                                    {
                                        qq.suoding = 0;
                                        new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                    }
                                    else
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                    }
                                }
                                xx = xx + id + "#" + zuowu + ab[1] + "个,";
                                yy = jy + yy;
                            }
                        }
                    }
                    if (td_num.Length > 0)
                    {
                        string ahj = td_num.Substring(0, td_num.Length - 1);
                        if (ahj.Length > 0)
                        {
                            new BCW.farm.BLL.NC_tudi().update_tudi("updatetime=GETDATE()", "usid = '" + UsID2 + "' AND tudi in (" + ahj + ")");
                        }
                    }
                    new BCW.farm.BLL.NC_messagelog().addmessage(UsID2, mename, "" + jysm + "在农场收获" + xx + "总计" + yy + "经验.", 8);//消息
                }
                //复位
                //new BCW.farm.BLL.NC_user().update_zd("shoutype=0", "usid=" + UsID2 + "");
            }
        }
    }

    private void ReloadPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        if (!new BCW.farm.BLL.NC_user().Exists(meid))
        {
            #region 邀请奖励
            int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            if (uid != 0)
            {
                //加金币
                new BCW.farm.BLL.NC_user().UpdateiGold(uid, new BCW.BLL.User().GetUsName(uid), yaoqing_jinbi, "成功邀请好友[" + new BCW.BLL.User().GetUsName(meid) + "]进入" + GameName + ",获得" + yaoqing_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(uid) + yaoqing_jinbi) + "金币.", 10);
                //随机一个种子
                BCW.farm.Model.NC_shop bb = new BCW.farm.BLL.NC_shop().Getsd_suiji(Convert.ToInt32(new BCW.farm.BLL.NC_user().GetGrade(uid)));
                int Num1 = 1;
                string hg = string.Empty;
                int id = bb.num_id;
                hg = bb.name;
                if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, uid))
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(uid, Num1, id);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists(id, uid))//如果道具表有了该种子
                    {
                        new BCW.farm.BLL.NC_mydaoju().Update_zz(uid, Num1, id);
                    }
                    else
                    {
                        BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                        u.name = bb.name;
                        u.name_id = id;
                        u.num = Num1;
                        u.type = 1;
                        u.usid = uid;
                        u.zhonglei = bb.type;
                        u.huafei_id = 0;
                        try
                        {
                            string[] bv = bb.picture.Split(',');
                            u.picture = bv[4];//我的道具表加图片
                        }
                        catch
                        {
                            u.picture = "";
                        }
                        new BCW.farm.BLL.NC_mydaoju().Add(u);
                    }
                }
                //发内线
                new BCW.BLL.Guest().Add(1, uid, new BCW.BLL.User().GetUsName(uid), "成功邀请好友[" + new BCW.BLL.User().GetUsName(meid) + "]进入" + GameName + ",获得" + yaoqing_jinbi + "金币,并随机获得" + Num1 + "个" + hg + ".[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
            }
            #endregion

            #region 新用户自动添加土地
            //添加新id;
            BCW.farm.Model.NC_user a = new BCW.farm.Model.NC_user();
            a.Grade = 0;
            a.Experience = 0;
            a.Goid = 1000;//新用户有1000金币
            a.iscao = 0;
            a.ischan = 0;
            a.isinsect = 0;
            a.isshifei = 0;
            a.isshou = 0;
            a.iswater = 0;
            a.iszhong = 0;
            a.usid = meid;
            new BCW.farm.BLL.NC_user().Add_1(a);
            //添加土地---6块
            for (int ii = 1; ii < 7; ii++)
            {
                BCW.farm.Model.NC_tudi b = new BCW.farm.Model.NC_tudi();
                BCW.farm.Model.NC_shop c = new BCW.farm.BLL.NC_shop().GetNC_shop1(1);//查询商店对于的种子信息
                b.iscao = 0;
                b.isinsect = 0;
                b.iswater = 0;
                b.harvest = 1;
                b.ischandi = 1;
                b.isshifei = 0;
                b.tudi_type = 1;
                //b.updatetime = DateTime.Now.AddSeconds(-63);//仿qq农场作物成熟
                b.updatetime = DateTime.Now;
                b.tudi = ii;
                b.usid = meid;
                b.zuowu = c.name;
                b.zuowu_experience = c.experience;
                b.zuowu_ji = c.jidu;
                //b.zuowu_time = 1;//仿qq农场作物成熟
                b.zuowu_time = c.jidu_time;
                b.output = c.output;
                new BCW.farm.BLL.NC_tudi().Add(b);
            }
            Utils.Success("欢迎新用户", "欢迎来到" + GameName + ".<b>我要立志成为一代优秀的农夫!</b>", Utils.getUrl("farm.aspx"), "3");
            #endregion
        }
        if (ub.GetSub("isshouhuo", xmlPath) == "0")//0为开启1为关闭
        {
            is_shougeji(meid);//自动收割机——只收割自己的
        }
        try
        {
            zhixing();//执行程序
        }
        catch (Exception ee)
        {
            new BCW.BLL.Guest().Add(1, 52784, "森林仔555", "出错了：" + ee.ToString() + "");//内线提示我出错(测试用)
        }

        Master.Title = GameName;
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>");
        builder.Append(Out.Tab("</div>", "<br />"));

        //顶部ubb
        string farmtop = ub.GetSub("farmtop", xmlPath);
        if (farmtop != "")
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(farmtop)));
            builder.Append(Out.Tab("</div>", "<br/>"));
        }
        builder.Append(Out.Tab("<div>", ""));
        string gif2 = ub.GetSub("farm_logo", xmlPath);
        builder.Append("<img height=\"70px\" width=\"200px\" src=\"" + gif2 + "\" alt=\"load\"/><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">宝箱</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market") + "\">市场</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">任务</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gonggao") + "\">公告</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("draw.aspx") + "\">抽奖</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("../forum.aspx?forumid=13") + "\">论坛</a>");
        builder.Append("<img src=\"" + ub.GetSub("newslogo", xmlPath) + "\" alt=\"load\"/>");//图片


        builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">商店</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">仓库</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=qiandao") + "\">签到</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gongnegn") + "\">功能</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">留言</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiyu") + "\">寄语</a>");
        builder.Append("<img src=\"" + ub.GetSub("newslogo", xmlPath) + "\" alt=\"load\"/><br/>");//图片

        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban") + "\">排行</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=punish") + "\">奴隶</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">陷阱</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message") + "\">个人</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a> ");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zhitiao") + "\">纸条</a>");
        builder.Append("<img src=\"" + ub.GetSub("newslogo", xmlPath) + "\" alt=\"load\"/><br/>");//图片
        builder.Append(Out.Tab("</div>", "<br/>"));


        //随机读取一个usid
        BCW.farm.Model.NC_win aa = new BCW.farm.BLL.NC_win().GetNC_suiji(0);
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            builder.Append("宝箱信息:" + new BCW.BLL.User().GetUsName(aa.usid) + "(" + aa.usid + ")开启了宝箱获得" + aa.prize_name + ".<br/>");
        }
        catch
        {
            //builder.Append("宝箱信息:暂无.<br/>");
        }

        try
        {
            string jy = new BCW.farm.BLL.NC_user().Get_jiyu(meid);
            if (jy != "")
                builder.Append("农场寄语:" + Out.SysUBB(jy) + "");
        }
        catch { }
        builder.Append(Out.Tab("</div>", "<br/>"));


        builder.Append(Out.Tab("<div>", ""));
        dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验

        builder.Append("等级:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "</a>&nbsp;");
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) <= 20)
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 200) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 40) && (new BCW.farm.BLL.NC_user().GetGrade(meid) > 20))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 250) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 50) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 41))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 500) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 60) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 51))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 700) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 70) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 61))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 900) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 80) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 71))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1300) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 90) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 81))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1700) + "</a>&nbsp;<br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 100) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 91))
        {
            builder.Append("&nbsp;&nbsp;经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 2500) + "</a>&nbsp;<br/>");
        }
        builder.Append("" + ub.Get("SiteBz") + ":<a href=\"" + Utils.getUrl("../finance.aspx") + "\">" + (new BCW.BLL.User().GetGold(meid)) + "</a>&nbsp;");
        builder.Append("&nbsp;&nbsp;金币:<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=2") + "\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</a>&nbsp;");
        builder.Append(Out.Tab("</div>", "<br />"));

        //如果今天没有签到,就显示
        BCW.farm.Model.NC_user model_q = new BCW.farm.BLL.NC_user().GetSignData(meid);
        if (string.IsNullOrEmpty(model_q.SignTime.ToString()))
        {
            model_q.SignTime = DateTime.Now.AddDays(-1);
        }
        if (model_q.SignTime < DateTime.Parse(DateTime.Now.ToLongDateString()))
        {
            string gif1 = ub.GetSub("farm_qd", xmlPath);//签到图片
            string show_logo = "<img src=\"" + gif1 + "\" alt=\"load\"/>";
            builder.Append(Out.Tab("<div>", ""));
            builder.Append(show_logo);
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=qiandao") + "\">领取每日登录大礼包</a><br/>");
            builder.Append(Out.Tab("</div>", ""));
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>一键 </b>");
        string time = DateTime.Now.ToString("HHmmss");
        if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo_1&amp;GKeyStr=" + SetRoomID(time.ToString()) + "") + "\">收获</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_chandi_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chandi_1") + "\">耕地</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_bozhong_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1") + "\">播种</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_chucao_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao_1&amp;GG=" + SetRoomID(time.ToString()) + "") + "\">草</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_jiaoshui_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui_1&amp;GG=" + SetRoomID(time.ToString()) + "") + "\">水</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_chuchong_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong_1&amp;GG=" + SetRoomID(time.ToString()) + "") + "\">虫</a>.");
        }
        if (new BCW.farm.BLL.NC_tudi().Exists_shifei_1(meid))
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shifei_1") + "\">施肥</a>");
        }
        builder.Append(Out.Tab("</div>", "<br/>"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            builder.Append("我的土地");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?&amp;ptype=1") + "\">我的土地</a>");
        }
        if (ptype == 2)
        {
            builder.Append("|我的摊位");
        }
        else
        {
            builder.Append("|<a href=\"" + Utils.getUrl("farm.aspx?act=market_tw") + "\">我的摊位</a>");
        }
        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx") + "\">刷新</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        int pageIndex;
        int recordCount;
        int pageSize = 6;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[koo + i]["tudi"].ToString());//土地块数
                int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["usid"].ToString());//用户id
                string zuowu = ds.Tables[0].Rows[koo + i]["zuowu"].ToString().Trim();//作物名称
                int zuowu_time = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["updatetime"]);//作物种植时间
                int tudi_type = int.Parse(ds.Tables[0].Rows[koo + i]["tudi_type"].ToString());//土地类型
                int zuowu_ji = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_ji"].ToString());//作物生长的季度
                int iscao = int.Parse(ds.Tables[0].Rows[koo + i]["iscao"].ToString());//除草
                int iswater = int.Parse(ds.Tables[0].Rows[koo + i]["iswater"].ToString());//是否浇水
                int isinsect = int.Parse(ds.Tables[0].Rows[koo + i]["isinsect"].ToString());//昆虫
                int ischandi = int.Parse(ds.Tables[0].Rows[koo + i]["ischandi"].ToString());//铲地(0空1有2枯萎)
                string output = ds.Tables[0].Rows[koo + i]["output"].ToString();//产/剩
                int zuowu_experience = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_experience"].ToString());//作物经验
                int harvest = int.Parse(ds.Tables[0].Rows[koo + i]["harvest"].ToString());//收获多少季度

                //根据作物查询商店表对应的作物图片
                BCW.farm.Model.NC_shop gg = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                if (zuowu == "")
                {
                    #region 作物为空
                    builder.Append("" + OutType(tudi_type) + id + ":[空地].<br/>");
                    try
                    {
                        builder.Append("<img src=\"/bbs/game/img/farm/kongdi.gif\" alt=\"load\"/>");// height=\"45px\"
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    if (ischandi == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong&amp;id=" + id + "&amp;fenye=" + pageIndex + "") + "\">播种</a>");
                    }
                    #endregion
                }
                else
                {
                    #region 作物不为空
                    if (ischandi == 1)
                    {
                        builder.Append("" + OutType(tudi_type) + id + ":" + zuowu + " (" + harvest + "/" + zuowu_ji + "季)<br/>");
                    }
                    else if (ischandi == 2)
                    {
                        builder.Append("" + OutType(tudi_type) + id + ":[枯萎的作物].<br/>");//<br/>
                        try
                        {
                            builder.Append("<img src=\"/bbs/game/img/farm/kuwei.gif\" alt=\"load\"/>");
                        }
                        catch { builder.Append("[图片出错!]<br/>"); }
                    }
                    #endregion
                }
                if (zuowu_ji == 1)
                {
                    #region 作物为一个季度
                    if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                    {
                        #region 作物未成熟
                        if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5))))//1/5==发芽
                        {

                            try
                            {
                                builder.Append("<img src=\"/bbs/game/img/farm/faya.png\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else if ((updatetime.AddMinutes((zuowu_time / 5)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 3))))//2/5==小叶子
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[0] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else if ((updatetime.AddMinutes((zuowu_time / 5 * 3)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 4))))//1/5==大叶子
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else //if ((updatetime.AddMinutes((zuowu_time / 5 * 4)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time))))//1/5初熟
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h><br/>");
                        if (iscao == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除草</a>.");
                        }
                        if (iswater == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">浇水</a>.");
                        }
                        if (isinsect == 1)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除虫</a>.");
                        }
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shifei&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">施肥</a>.");
                        #endregion
                    }
                    else
                    {
                        #region 作物成熟
                        if (ischandi == 1)
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                            string[] ab = output.Split(',');
                            builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");
                            //如果成熟了,把虫,草,水改为0
                            //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                            //20160507 修改成熟后还缺水---若缺水到某个时间，就减产
                            //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                            //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                            builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">收获</a>.");

                            BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                            if (UU.iscao == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除草</a>.");
                            }
                            if (UU.iswater == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">浇水</a>.");
                            }
                            if (UU.isinsect == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除虫</a>.");
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region 作物为多季度
                    if (harvest == 1)
                    {
                        if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                        {
                            #region 作物为第一季度未成熟时
                            if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5))))//1/6==发芽
                            {
                                try
                                {
                                    builder.Append("<img src=\"/bbs/game/img/farm/faya.png\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else if ((updatetime.AddMinutes((zuowu_time / 5)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 3))))//1/6==小叶子
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[0] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else if ((updatetime.AddMinutes((zuowu_time / 5 * 3)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 4))))//2/6==大叶子
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else //if ((updatetime.AddMinutes((zuowu_time / 5 * 4)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time))))//2/6初熟
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h><br/>");
                            if (iscao == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除草</a>.");
                            }
                            if (iswater == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">浇水</a>.");
                            }
                            if (isinsect == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除虫</a>.");
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shifei&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">施肥</a>.");
                            #endregion
                        }
                        else
                        {
                            #region 作物为第一季度成熟
                            if (ischandi == 1)
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                                string[] ab = output.Split(',');
                                builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");
                                //如果成熟了,把虫,草,水改为0
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                                //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                                builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">收获</a>.");

                                BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                                if (UU.iscao == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除草</a>.");
                                }
                                if (UU.iswater == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">浇水</a>.");
                                }
                                if (UU.isinsect == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除虫</a>.");
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        //再成熟，时间为后面的2/5
                        if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                        {
                            #region 作物为多季度未成熟
                            if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes(zuowu_time / 2)))
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h><br/>");
                            if (iscao == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除草</a>.");
                            }
                            if (iswater == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">浇水</a>.");
                            }
                            if (isinsect == 1)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">除虫</a>.");
                            }
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shifei&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">施肥</a>.");
                            #endregion
                        }
                        else
                        {
                            #region 作物为多季度成熟
                            if (ischandi == 1)
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                                string[] ab = output.Split(',');
                                builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");//<br/>
                                //如果成熟了,把虫,草,水改为0
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                                //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                                builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">收获</a>.");

                                BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                                if (UU.iscao == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除草</a>.");
                                }
                                if (UU.iswater == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">浇水</a>.");
                                }
                                if (UU.isinsect == 1)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong&amp;usid=" + UsID + "&amp;tudi=" + id + "") + "\">除虫</a>.");
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                if (DateTime.Now < updatetime.AddMinutes(zuowu_time) || ischandi == 2)//未成熟
                {
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chandi&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "") + "\">铲地</a>");
                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "土地出错."));
        }

        #region 我的留言列表
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("我的留言列表：<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex1;
        int recordCount1;
        int pageSize1 = 5;
        string[] pageValUrl1 = { "act", "ptype", "pageIndex", "uid", "page", "backurl" };
        pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
        if (pageIndex1 == 0)
            pageIndex1 = 1;
        string strWhere1 = string.Empty;
        strWhere1 = "usid=" + meid + " and type=1001";
        // 开始读取列表
        IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex1, pageSize1, strWhere1, out recordCount1);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Mebook n in listMebook)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + n.MID + "") + "\">" + n.MName + "(" + n.MID + ")</a>");
                builder.Append(":" + Out.SysUBB(n.MContent) + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + n.MID + "") + "\">[回复]</a>");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, 1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">更多留言>></a>");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            builder.Append(Out.Div("div", "没有留言记录.."));
        }
        #endregion

        //闲聊显示
        builder.Append(Out.SysUBB(BCW.User.Users.ShowSpeak(30, "farm.aspx", 5, 0)));

        //游戏底部Ubb
        string Foot = ub.GetSub("farmFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //播种
    private void bozhongPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int tudi = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        if (new BCW.farm.BLL.NC_tudi().Exists_zhongzhi(tudi, meid))//是否存在空土地
        {
            Master.Title = "播种";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;播种");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("当前等级：<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
            if (ptype == 1)
            {
                builder.Append("<h style=\"color:red\">按金币" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong&amp;ptype=1&amp;id=" + tudi + "") + "\">按金币</a>" + "|");
            }
            if (ptype == 2)
            {
                builder.Append("<h style=\"color:red\">按经验" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong&amp;ptype=2&amp;id=" + tudi + "") + "\">按经验</a>" + "|");
            }
            if (ptype == 3)
            {
                builder.Append("<h style=\"color:red\">按时间" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong&amp;ptype=3&amp;id=" + tudi + "") + "\">按时间</a>" + "|");
            }
            if (ptype == 4)
            {
                builder.Append("<h style=\"color:red\">可赠送" + "</h>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong&amp;ptype=4&amp;id=" + tudi + "") + "\">可赠送</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));


            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string sOrder = "";
            string[] pageValUrl = { "act", "ptype", "id", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            if (ptype == 1)
            {
                sOrder = "ORDER BY c.price_in";//金币
            }
            else if (ptype == 2)
            {
                sOrder = "ORDER BY c.experience";//经验
            }
            else if (ptype == 3)
            {
                sOrder = "ORDER BY c.jidu_time";//时间
            }
            else
            {
                sOrder = "and c.iszengsong=1 ORDER BY c.grade asc";
            }

            DataSet ds = new BCW.farm.BLL.NC_mydaoju().GetList2("*", " a INNER JOIN tb_NC_shop c ON a.usid=" + meid + " and a.num>0 and a.name_id!=0 AND a.name_id=c.num_id " + sOrder + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    string name = (ds.Tables[0].Rows[koo + i]["name"]).ToString();//用户id
                    int num = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["num"]);//数量
                    int name_id = Convert.ToInt32(ds.Tables[0].Rows[koo + i]["name_id"]);//ID
                    //根据id查询种植等级 20160829 邵广林
                    BCW.farm.Model.NC_shop grade = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + name + "(" + grade.grade + "级) x " + num + "");
                    builder.Append(" <a href=\"" + Utils.getUrl("farm.aspx?act=zhongzhi&amp;name_id=" + name_id + "&amp;id=" + tudi + "&amp;fenye=" + fenye + "") + "\">种植</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "<h style=\"color:#A9A9A9\">你没有符合种植条件的种子..</h>"));
            }
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }

        foot_link();//底部链接

        foot_link2();
    }

    //种植
    private void zhongzhiPage()
    {
        int tudi = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int name_id = Utils.ParseInt(Utils.GetRequest("name_id", "get", 2, @"^[1-9]\d*$", "选择农作物出错"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (new BCW.farm.BLL.NC_tudi().Exists_zhongzhi(tudi, meid))//是否存在空土地
        {
            BCW.farm.Model.NC_mydaoju a = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, name_id);//查询种子个数
            if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, name_id) == 0)
            {
                Utils.Error("种子数量不足,请重新选择.", "");
            }
            //根据种子name_id,查询对应的等级,来判断是否够等级种植
            BCW.farm.Model.NC_shop uu = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);
            if (uu.grade > new BCW.farm.BLL.NC_user().GetGrade(meid))
            {
                Utils.Error("你的等级不够种植改作物,请先升级.", "");
            }
            //判断种子属于种植哪种类型的土地,只能高种低,不能低种高----普通土地不可以种植红种子，红土地可以种植普通种子
            if (uu.type != 10)
            {
                //根据土地id查土地类型
                BCW.farm.Model.NC_tudi ui = new BCW.farm.BLL.NC_tudi().Get_td(meid, tudi);
                if (uu.type > ui.tudi_type)
                {
                    Utils.Error("你的土地类型不能种植该种子,请先升级土地或选择其他种子种植.", "");
                }
            }
            if (a.num > 0)
            {
                BCW.farm.Model.NC_shop b = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);//查询商店对于的种子信息
                BCW.farm.Model.NC_tudi bb = new BCW.farm.Model.NC_tudi();//实例化土地
                BCW.farm.Model.NC_tudi hj = new BCW.farm.BLL.NC_tudi().Get_td(meid, tudi);//查询土地类型
                BCW.User.Users.IsFresh("Farmzz", 2);//防刷
                if (hj.tudi_type > 1)
                {
                    if (hj.tudi_type == 2)//红土地
                    {
                        bb.zuowu_time = b.jidu_time;
                        bb.zuowu_experience = b.experience;
                        //增产10%
                        string[] ui = b.output.Split(',');
                        int a0 = int.Parse(ui[0]) / 10 + int.Parse(ui[0]);
                        string aa0 = a0.ToString() + "," + a0.ToString();
                        bb.output = aa0;
                    }
                    else if (hj.tudi_type == 3)//黑土地
                    {
                        //减少10%的成熟时间
                        bb.zuowu_time = b.jidu_time - (b.jidu_time / 10);

                        //增加10%的经验
                        bb.zuowu_experience = b.experience + (b.experience / 10);

                        //增产20%
                        string[] ui = b.output.Split(',');
                        int a0 = int.Parse(ui[0]) / 5 + int.Parse(ui[0]);
                        string aa0 = a0.ToString() + "," + a0.ToString();
                        bb.output = aa0;
                    }
                    else if (hj.tudi_type == 4)//金土地
                    {
                        //减少20%的成熟时间
                        bb.zuowu_time = b.jidu_time - (b.jidu_time / 5);

                        //增加20%的经验
                        bb.zuowu_experience = b.experience + (b.experience / 5);

                        //增产30%
                        string[] ui = b.output.Split(',');
                        int a0 = int.Parse(ui[0]) * 3 / 10 + int.Parse(ui[0]);
                        string aa0 = a0.ToString() + "," + a0.ToString();
                        bb.output = aa0;
                    }
                }
                else//普通土地
                {
                    bb.zuowu_time = b.jidu_time;
                    bb.zuowu_experience = b.experience;
                    bb.output = b.output;
                }
                bb.tudi = tudi;
                bb.iscao = 0;
                bb.ischandi = 1;
                bb.isinsect = 0;
                bb.iswater = 0;
                bb.updatetime = DateTime.Now;
                bb.usid = meid;
                bb.zuowu = a.name;
                bb.zuowu_ji = b.jidu;
                bb.harvest = 1;
                bb.isshifei = 0;

                new BCW.farm.BLL.NC_tudi().Update_1(bb);//种植的时候更新草虫水的时间
                BCW.farm.Model.NC_mydaoju a1 = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, name_id);//查询种子个数
                if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, name_id) == 0)
                {
                    Utils.Error("种子数量不足,请重新选择.", "");
                }
                //int y = a1.num - 1;
                int y = -1;
                new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, y, name_id);//减少种子

                //双倍经验判断
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明

                if (bz_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bzjingyan(meid)))
                {
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = zhong_jingyan * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = zhong_jingyan;
                        jysm = "";
                    }
                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    //每日上限
                    new BCW.farm.BLL.NC_user().update_zd("big_bozhong=big_bozhong+2", "usid=" + meid + "");//邵广林 播种+2
                }
                else
                    jysm = "经验已达上限.";
                //等级操作
                dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场第" + tudi + "块土地种植了" + a.name + ".", 8);//消息
                Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块空土地种植了<b>" + a.name + "</b>." + jysm + "获得" + jy + "点经验", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "2");
            }
            else
            {
                Utils.Success("操作土地", "种子不足,请选择其他种子.", Utils.getUrl("farm.aspx?act=bozhong&amp;id=" + tudi + ""), "1");
            }
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
    }

    //一键播种
    private void bozhong_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        Master.Title = "一键播种";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;一键播种");
        builder.Append(Out.Tab("</div>", "<br />"));

        DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and zuowu=''");//查询空土地
        if (ds.Tables[0].Rows.Count != 0)
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你有" + ds.Tables[0].Rows.Count + "块空土地,");
            builder.Append("当前等级：<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "级</a>");
            builder.Append(Out.Tab("</div>", "<br />"));
            builder.Append(Out.Div("div", "<h style=\"color:#A9A9A9\">你选择的种子将播种在所有空地上</h><br/>"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string sOrder = "";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
            if (ptype == 1)
            {
                builder.Append("<h style=\"color:red\">按金币" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1&amp;ptype=1") + "\">按金币</a>" + "|");
            }
            if (ptype == 2)
            {
                builder.Append("<h style=\"color:red\">按经验" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1&amp;ptype=2") + "\">按经验</a>" + "|");
            }
            if (ptype == 3)
            {
                builder.Append("<h style=\"color:red\">按时间" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1&amp;ptype=3") + "\">按时间</a>" + "|");
            }
            if (ptype == 4)
            {
                builder.Append("<h style=\"color:red\">可赠送" + "</h>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1&amp;ptype=4") + "\">可赠送</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));


            if (ptype == 1)
            {
                sOrder = "ORDER BY c.price_in";//金币
            }
            else if (ptype == 2)
            {
                sOrder = "ORDER BY c.experience";//经验
            }
            else if (ptype == 3)
            {
                sOrder = "ORDER BY c.jidu_time";//时间
            }
            else
            {
                sOrder = "and c.iszengsong=1 ORDER BY c.grade asc";
            }

            DataSet dss = new BCW.farm.BLL.NC_mydaoju().GetList2("*", " a INNER JOIN tb_NC_shop c ON a.usid=" + meid + " and a.num>0 and a.name_id!=0 AND a.name_id=c.num_id " + sOrder + "");
            if (dss != null && dss.Tables[0].Rows.Count > 0)
            {
                recordCount = dss.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    string name = (dss.Tables[0].Rows[koo + i]["name"]).ToString();//用户id
                    int num = Convert.ToInt32(dss.Tables[0].Rows[koo + i]["num"]);//数量
                    int name_id = Convert.ToInt32(dss.Tables[0].Rows[koo + i]["name_id"]);//ID
                    //根据id查询种植等级 20160829 邵广林
                    BCW.farm.Model.NC_shop grade = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + name + "(" + grade.grade + "级) x " + num + "");
                    string time = DateTime.Now.ToString("HHmmss");
                    builder.Append(" <a href=\"" + Utils.getUrl("farm.aspx?act=zhongzhi_1&amp;name_id=" + name_id + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + "") + "\">种植</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "<h style=\"color:#A9A9A9\">你没有符合种植条件的种子..</h>"));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("暂无土地可以播种.");
            builder.Append(Out.Tab("</div>", ""));
        }

        foot_link();//底部链接

        foot_link2();
    }

    //一键种植
    private void zhongzhi_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //邵广林 20161025 加密传时间，判断是否操作超时
        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        if (GKeyStr == "")
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键种植操作中存在非法链接%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键种植操作中存在非法链接.");
            Utils.Error("非法操作链接.", Utils.getUrl("farm.aspx"));
        }
        int GKeyID = GetRoomID(GKeyStr);
        string yy = DateTime.Now.ToString("yyyy-MM-dd ") + GKeyID.ToString("00:00:00");
        DateTime qq = Convert.ToDateTime(yy);
        if (qq.AddSeconds(-30) > DateTime.Now || DateTime.Now > qq.AddSeconds(30))
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键种植操作中存在超时情况%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键种植操作中存在超时情况.");
            Utils.Error("亲,你操作超时咯.请重新种植!", Utils.getUrl("farm.aspx"));
        }

        int name_id = Utils.ParseInt(Utils.GetRequest("name_id", "get", 2, @"^[1-9]\d*$", "选择农作物出错"));

        BCW.farm.Model.NC_mydaoju a = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, name_id);//查询种子个数
        if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, name_id) == 0)
        {
            Utils.Error("种子数量不足,请重新选择.", "");
        }
        //根据种子name_id,查询对应的等级,来判断是否够等级种植
        BCW.farm.Model.NC_shop uu = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);
        if (uu.grade > new BCW.farm.BLL.NC_user().GetGrade(meid))
        {
            Utils.Error("你的等级不够种植改作物,请先升级.", "");
        }
        BCW.farm.Model.NC_shop b = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);//查询商店对于的种子信息
        BCW.farm.Model.NC_tudi bb = new BCW.farm.Model.NC_tudi();//实例化土地
        int h = 0;

        DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and zuowu=''");//查询空土地
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            BCW.User.Users.IsFresh("Farmzz", 2);//防刷
            if (a.num < 1)
            {
                Utils.Error("种子数量不足,请重新选择.", "");
            }
            if (ds.Tables[0].Rows.Count >= a.num)//如果土地块数>=种子数量
            {
                for (int i = 0; i < a.num; i++)
                {
                    BCW.farm.Model.NC_tudi zz = new BCW.farm.BLL.NC_tudi().Get_tudinum_bz(meid);
                    //查询土地类型
                    BCW.farm.Model.NC_tudi hj = new BCW.farm.BLL.NC_tudi().Get_td(meid, zz.tudi);
                    if (hj.tudi_type > 1)
                    {
                        if (hj.tudi_type == 2)//红土地
                        {
                            bb.zuowu_time = b.jidu_time;
                            bb.zuowu_experience = b.experience;
                            //增产10%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) / 10 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                        else if (hj.tudi_type == 3)//黑土地
                        {
                            //减少10%的成熟时间
                            bb.zuowu_time = b.jidu_time - (b.jidu_time / 10);

                            //增加10%的经验
                            bb.zuowu_experience = b.experience + (b.experience / 10);

                            //增产20%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) / 5 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                        else if (hj.tudi_type == 4)//金土地
                        {
                            //减少20%的成熟时间
                            bb.zuowu_time = b.jidu_time - (b.jidu_time / 5);

                            //增加20%的经验
                            bb.zuowu_experience = b.experience + (b.experience / 5);

                            //增产30%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) * 3 / 10 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                    }
                    else//普通土地
                    {
                        bb.zuowu_time = b.jidu_time;
                        bb.zuowu_experience = b.experience;
                        bb.output = b.output;
                    }

                    bb.tudi = zz.tudi;
                    bb.iscao = 0;
                    bb.ischandi = 1;
                    bb.isinsect = 0;
                    bb.iswater = 0;
                    bb.updatetime = DateTime.Now;
                    bb.usid = meid;
                    bb.zuowu = a.name;
                    bb.zuowu_ji = b.jidu;
                    bb.harvest = 1;
                    bb.isshifei = 0;
                    //判断种子属于种植哪种类型的土地,只能高种低,不能低种高----普通土地不可以种植红种子，红土地可以种植普通种子
                    if (b.type != 10)
                    {
                        if (b.type > hj.tudi_type)
                        {
                            Utils.Error("你的其中一块土地类型不能种植该种子,请先升级土地或选择其他种子种植.", "");
                        }
                    }
                    new BCW.farm.BLL.NC_tudi().Update_1(bb);//种植的时候更新草虫水的时间

                    BCW.farm.Model.NC_mydaoju a1 = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, name_id);//查询种子个数
                    if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, name_id) == 0)
                    {
                        Utils.Error("种子数量不足,请重新选择.", "");
                    }
                    int y = -1;
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, y, name_id);
                    if (bz_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bzjingyan(meid)))
                    {
                        //每日上限
                        new BCW.farm.BLL.NC_user().update_zd("big_bozhong=big_bozhong+2", "usid=" + meid + "");//邵广林 播种+2
                    }
                    h++;
                }
            }
            else
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BCW.farm.Model.NC_tudi z = new BCW.farm.BLL.NC_tudi().Get_tudinum_bz(meid);
                    //查询土地类型
                    BCW.farm.Model.NC_tudi hj = new BCW.farm.BLL.NC_tudi().Get_td(meid, z.tudi);
                    if (hj.tudi_type > 1)
                    {
                        if (hj.tudi_type == 2)//红土地
                        {
                            bb.zuowu_time = b.jidu_time;
                            bb.zuowu_experience = b.experience;
                            //增产10%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) / 10 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                        else if (hj.tudi_type == 3)//黑土地
                        {
                            //减少10%的成熟时间
                            bb.zuowu_time = b.jidu_time - (b.jidu_time / 10);

                            //增加10%的经验
                            bb.zuowu_experience = b.experience + (b.experience / 10);

                            //增产20%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) / 5 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                        else if (hj.tudi_type == 4)//金土地
                        {
                            //减少20%的成熟时间
                            bb.zuowu_time = b.jidu_time - (b.jidu_time / 5);

                            //增加20%的经验
                            bb.zuowu_experience = b.experience + (b.experience / 5);

                            //增产30%
                            string[] ui = b.output.Split(',');
                            int a0 = int.Parse(ui[0]) * 3 / 10 + int.Parse(ui[0]);
                            string aa0 = a0.ToString() + "," + a0.ToString();
                            bb.output = aa0;
                        }
                    }
                    else//普通土地
                    {
                        bb.zuowu_time = b.jidu_time;
                        bb.zuowu_experience = b.experience;
                        bb.output = b.output;
                    }
                    bb.tudi = z.tudi;
                    bb.iscao = 0;
                    bb.ischandi = 1;
                    bb.isinsect = 0;
                    bb.iswater = 0;
                    bb.updatetime = DateTime.Now;
                    bb.usid = meid;
                    bb.zuowu = a.name;
                    bb.zuowu_ji = b.jidu;
                    bb.harvest = 1;
                    bb.isshifei = 0;
                    //判断种子属于种植哪种类型的土地,只能高种低,不能低种高----普通土地不可以种植红种子，红土地可以种植普通种子
                    if (b.type != 10)
                    {
                        if (b.type > hj.tudi_type)
                        {
                            Utils.Error("你的其中一块土地类型不能种植该种子,请先升级土地或选择其他种子种植.", "");
                        }
                    }
                    new BCW.farm.BLL.NC_tudi().Update_1(bb);//种植的时候更新草虫水的时间
                    BCW.farm.Model.NC_mydaoju a1 = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, name_id);//查询种子个数
                    if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, name_id) == 0)
                    {
                        Utils.Error("种子数量不足,请重新选择.", "");
                    }
                    int y = -1;
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, y, name_id);
                    if (bz_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bzjingyan(meid)))
                    {
                        //每日上限
                        new BCW.farm.BLL.NC_user().update_zd("big_bozhong=big_bozhong+2", "usid=" + meid + "");//邵广林 播种+2
                    }
                    h++;
                }
            }
            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (bz_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bzjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = h * zhong_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                }
                else
                {
                    jy = h * zhong_jingyan;
                    jysm = "";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
            }
            else
                jysm = "经验已达上限.";

            //等级操作
            dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场的" + h + "块空土地种植了" + a.name + ".", 8);//消息
            Utils.Success("操作土地", "您成功在" + h + "块空土地种植了<b>" + a.name + "</b>." + jysm + "获得" + jy + "点经验", Utils.getUrl("farm.aspx?act=bozhong_1"), "2");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=bozhong_1"), "1");
        }
    }

    //除草
    private void ChucaoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名

        if (new BCW.farm.BLL.NC_tudi().Exists_chucao(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmcc", 3);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,caoID='',z_caotime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
            //任务
            BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(2, meid);
            if (wr.usid > 0)
            {
                new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+2", "usid=" + meid + " and task_id=2 and type=0");//邵广林 除草+2
            }

            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = all_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                }
                else
                {
                    jy = all_jingyan;
                    jysm = "获得" + jy + "经验和";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(usid, jy);
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "获得";

            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场第" + tudi + "块土地除去1堆杂草.", 1);//消息

            //查询除草除虫次数是否超xml 邵广林20160826
            if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
            {
                new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+1", "usid=" + meid + "");
                new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename, all_jinbi, "在自己农场第" + tudi + "块土地除去1堆杂草,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) + all_jinbi) + "金币.", 1);
                Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块土地除去1堆杂草." + jysm + "" + all_jinbi + "金币.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
            }
            else
                Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块土地除去1堆杂草.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
    }

    //(偷)除草
    private void Chucao2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename2 = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        //if ((!new BCW.BLL.Friend().Exists(meid, usid, 0)) && (!new BCW.BLL.Friend().Exists(usid, meid, 0)))//判断是否为好友
        //{
        //    Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
        //}
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        if (new BCW.farm.BLL.NC_tudi().Exists_chucao(tudi, usid))
        {
            if (!new BCW.farm.BLL.NC_tudi().Exists_zcao(tudi, usid, meid))//判断是否自己种的草
            {
                BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,caoID='',z_caotime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                //任务
                BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(2, meid);
                if (wr.usid > 0)
                {
                    new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+2", "usid=" + meid + " and task_id=2 and type=0");//邵广林 除草+2
                }
                //双倍经验判断
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明

                if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                {
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = all_jingyan / 2 * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = all_jingyan / 2;
                        jysm = "";
                    }
                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    //每日上限
                    new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");
                }
                else
                    jysm = "经验已达上限.";

                //等级操作
                dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename2, "在" + mename + "(" + usid + ")的农场第" + tudi + "块土地除去1堆杂草.", 1);//自己的消息
                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "" + mename2 + "(" + meid + ")来农场帮忙,在第" + tudi + "块土地除去1堆杂草.", 1);//被偷的消息

                //查询除草除虫次数是否超xml 邵广林20160826
                if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                {
                    new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+1", "usid=" + meid + "");
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename2, all_jinbi, "在" + mename + "的农场第" + tudi + "块土地除去1堆杂草,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + all_jinbi) + "金币.", 1);
                    Utils.Success("操作土地", "您成功在" + mename + "的农场的第" + tudi + "块土地除去1堆杂草." + jysm + "获得" + jy + "经验," + all_jinbi + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
                }
                else
                    Utils.Success("操作土地", "您成功在" + mename + "的农场的第" + tudi + "块土地除去1堆杂草." + jysm + "获得" + jy + "经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");

            }
            else
                Utils.Error("你太坏了,不能除掉自己种的草.", "");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //一键、除草
    private void chucao_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //邵广林 20161114 加密传时间，判断是否操作超时
        string GKeyStr = Utils.GetRequest("GG", "all", 1, @"^[^\^]{1,20}$", "");
        if (GKeyStr == "")
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键除草操作中存在非法链接%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键除草操作中存在非法链接.");
            Utils.Error("非法操作链接.", Utils.getUrl("farm.aspx"));
        }
        int GKeyID = GetRoomID(GKeyStr);
        string yy = DateTime.Now.ToString("yyyy-MM-dd ") + GKeyID.ToString("00:00:00");
        DateTime qq = Convert.ToDateTime(yy);
        if (qq.AddSeconds(-30) > DateTime.Now || DateTime.Now > qq.AddSeconds(30))
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键除草操作中存在超时情况%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键除草操作中存在超时情况.");
            Utils.Error("亲,你操作超时咯.请重新除草!", Utils.getUrl("farm.aspx"));
        }

        Master.Title = "一键除草";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;温馨提示");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < chucao1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键除草需要" + chucao1_grade + "级才能开通哦!");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getchucao(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键除草功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=1&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + chucao1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int aa = 0;
                int bb = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and iscao=1 and ischandi=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,caoID='',z_caotime=getdate()", "usid=" + meid + " and tudi=" + id + "");
                        if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
                        {
                            //每日上限
                            new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");
                            bb++;
                        }
                        aa++;
                    }
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if (bb > 0)
                    {
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = all_jingyan * bb * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                        }
                        else
                        {
                            jy = all_jingyan * bb;
                            jysm = "获得" + jy + "经验和";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    }
                    else
                        jysm = "获得";


                    //任务
                    BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(2, meid);
                    if (wr.usid > 0)
                    {
                        new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=2 and type=0");
                    }

                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己的农场除去" + aa + "堆杂草.", 1);//消息


                    //查询除草除虫次数是否超xml 邵广林20160826
                    if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                    {
                        new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+" + aa + "", "usid=" + meid + "");
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在农场除去" + aa + "堆杂草,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                        Utils.Success("操作土地", "您成功除去" + aa + "堆杂草." + jysm + "" + all_jinbi * aa + "金币", Utils.getUrl("farm.aspx"), "1");
                    }
                    else
                        Utils.Success("操作土地", "您成功除去" + aa + "堆杂草.", Utils.getUrl("farm.aspx"), "1");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
        }
        foot_link();//底部链接

        foot_link2();
    }

    //一键、(偷)除草
    private void chucao_2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//自己姓名
        string mename2 = new BCW.BLL.User().GetUsName(usid);//被偷姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;

        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
        }
        if (DateTime.Now < viptime)
        {
            int aa = 0;
            int bb = 0;
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and iscao=1 and ischandi=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                    if (!new BCW.farm.BLL.NC_tudi().Exists_zcao(id, usid, meid))//判断是否自己种的草
                    {
                        new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,caoID='',z_caotime=getdate()", "usid=" + usid + " and tudi=" + id + "");
                        if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                        {
                            new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");
                            bb++;
                        }
                        aa++;
                    }
                }
                if (aa > 0)
                {
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    //if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                    if (bb > 0)
                    {
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = all_jingyan / 2 * bb * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.";
                        }
                        else
                        {
                            jy = all_jingyan / 2 * bb;
                            jysm = "";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    }
                    else
                        jysm = "经验已达上限.";

                    //任务
                    BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(2, meid);
                    if (wr.usid > 0)
                    {
                        new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=2 and type=0");
                    }

                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验

                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场除去" + aa + "堆杂草.", 1);//消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场帮忙,除去" + aa + "堆杂草.", 1);//消息

                    //查询除草除虫次数是否超xml 邵广林20160826
                    if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                    {
                        if (aa > 0)
                        {
                            new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+" + aa + "", "usid=" + meid + "");
                            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在" + mename2 + "的农场除去" + aa + "堆杂草,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                        }
                        Utils.Success("操作土地", "您成功在" + mename2 + "的农场除去" + aa + "堆杂草." + jysm + "获得" + jy + "经验," + all_jinbi * aa + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                    else
                        Utils.Success("操作土地", "您成功在" + mename2 + "的农场除去" + aa + "堆杂草." + jysm + "获得" + jy + "经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                }
                else
                    Utils.Error("你太坏了,不能除掉自己种的草.", "");

            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
        }
        else
        {
            Utils.Error("抱歉,只有vip会员才可以一键除草哦,请开通vip再来,<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">马上开通</a>", "");
        }
    }

    //浇水
    private void JiaoshuiPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        if (new BCW.farm.BLL.NC_tudi().Exists_jiaoshui(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmjs", 1);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
            //任务
            BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(3, meid);
            if (wr.usid > 0)
            {
                new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+1", "usid=" + meid + " and task_id=3 and type=0");
            }
            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = all_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                }
                else
                {
                    jy = all_jingyan;
                    jysm = "获得" + jy + "经验和";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(usid, jy);
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "获得";

            //new BCW.farm.BLL.NC_user().Update_jinbi(usid, all_jinbi);
            new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename, all_jinbi, "在农场第" + tudi + "块土地浇水,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) + all_jinbi) + "金币.", 1);
            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场第" + tudi + "块土地浇水.", 1);//消息
            Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块土地浇水." + jysm + "" + all_jinbi + "金币", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
    }

    //(偷)浇水
    private void Jiaoshui2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename2 = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        if (new BCW.farm.BLL.NC_tudi().Exists_jiaoshui(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmjs", 1);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
            //任务
            BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(3, meid);
            if (wr.usid > 0)
            {
                new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+1", "usid=" + meid + " and task_id=3 and type=0");
            }
            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = all_jingyan / 2 * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                }
                else
                {
                    jy = all_jingyan / 2;
                    jysm = "";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "经验已达上限.";

            //等级操作
            dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验

            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename2, all_jinbi, "在" + mename + "的农场第" + tudi + "块土地浇水,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + all_jinbi) + "金币.", 1);
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename2, "在" + mename + "(" + usid + ")的农场第" + tudi + "块土地浇水.", 1);//自己的消息
            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "" + mename2 + "(" + meid + ")来农场帮忙,在第" + tudi + "块土地浇水.", 1);//被偷的消息

            Utils.Success("操作土地", "您成功在" + mename + "的农场的第" + tudi + "块土地浇水." + jysm + "获得" + jy + "经验," + all_jinbi + "金币", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //一键、浇水
    private void jiaoshui_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //邵广林 20161114 加密传时间，判断是否操作超时
        string GKeyStr = Utils.GetRequest("GG", "all", 1, @"^[^\^]{1,20}$", "");
        if (GKeyStr == "")
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键浇水操作中存在非法链接%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键浇水操作中存在非法链接.");
            Utils.Error("非法操作链接.", Utils.getUrl("farm.aspx"));
        }
        int GKeyID = GetRoomID(GKeyStr);
        string yy = DateTime.Now.ToString("yyyy-MM-dd ") + GKeyID.ToString("00:00:00");
        DateTime qq = Convert.ToDateTime(yy);
        if (qq.AddSeconds(-30) > DateTime.Now || DateTime.Now > qq.AddSeconds(30))
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键浇水操作中存在超时情况%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键浇水操作中存在超时情况.");
            Utils.Error("亲,你操作超时咯.请重新浇水!", Utils.getUrl("farm.aspx"));
        }


        Master.Title = "一键浇水";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;温馨提示");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < jiaoshui1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键浇水需要" + jiaoshui1_grade + "级才能开通哦!");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getjiaoshui(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键浇水功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=2&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + jiaoshui1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {

                int aa = 0;
                int bb = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and iswater=1 and ischandi=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmjs", 1);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid=" + meid + " and tudi=" + id + "");
                        if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
                        {
                            //每日上限
                            new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");
                            bb++;
                        }
                        aa++;
                    }
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if (bb > 0)
                    {
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = all_jingyan * bb * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                        }
                        else
                        {
                            jy = all_jingyan * bb;
                            jysm = "获得" + jy + "经验和";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    }
                    else
                        jysm = "获得";



                    //任务
                    BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(3, meid);
                    if (wr.usid > 0)
                    {
                        new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=3 and type=0");
                    }


                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                    //new BCW.farm.BLL.NC_user().Update_jinbi(meid, all_jinbi * aa);
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在农场给" + aa + "块土地浇水,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场给" + aa + "块土地浇水.", 1);//消息
                    Utils.Success("操作土地", "您成功给" + aa + "块土地浇水." + jysm + "" + all_jinbi * aa + "金币", Utils.getUrl("farm.aspx"), "1");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
        }
        foot_link();//底部链接

        foot_link2();
    }

    //一键、(偷)浇水
    private void jiaoshui_2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//自己姓名
        string mename2 = new BCW.BLL.User().GetUsName(usid);//被偷姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;

        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
        }
        if (DateTime.Now < viptime)
        {
            int aa = 0;
            int bb = 0;
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and iswater=1 and ischandi=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                BCW.User.Users.IsFresh("Farmjs", 1);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                    new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid=" + usid + " and tudi=" + id + "");
                    if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                    {
                        new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");//每日上限
                        bb++;
                    }
                    aa++;
                }
                //双倍经验判断
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明

                if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                {
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = all_jingyan / 2 * bb * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = all_jingyan / 2 * bb;
                        jysm = "";
                    }
                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                }
                else
                    jysm = "经验已达上限.";

                //任务
                BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(3, meid);
                if (wr.usid > 0)
                {
                    new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=3 and type=0");
                }

                //等级操作
                dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                if (aa > 0)
                {
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在" + mename2 + "的农场给" + aa + "块土地浇水,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                }
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场给" + aa + "块土地浇水.", 1);//消息
                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场帮忙,给" + aa + "块土地浇水.", 1);//消息
                Utils.Success("操作土地", "您成功在" + mename2 + "的农场给" + aa + "块土地浇水." + jysm + "获得" + jy + "点经验和" + all_jinbi * aa + "金币", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
        }
        else
        {
            Utils.Error("抱歉,只有vip会员才可以一键浇水哦,请开通vip再来,<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">马上开通</a>", "");
        }
    }

    //(偷)种草
    private void zcao1Page()
    {
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename2 = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        if ((!new BCW.farm.BLL.NC_tudi().Exists_chucao(tudi, usid)) && new BCW.farm.BLL.NC_tudi().Exists_zhongcao(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmcc", 3);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("iscao=1,caoID=" + meid + ",z_caotime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");

            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = zcaofchong_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                }
                else
                {
                    jy = zcaofchong_jingyan;
                    jysm = "";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_shihuai=big_shihuai+1", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "经验已达上限.";

            //等级操作
            dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename2, "在" + mename + "(" + usid + ")的农场第" + tudi + "块土地种下1个杂草.", 9);//消息
            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "" + mename2 + "(" + meid + ")来农场第" + tudi + "块土地种下1个杂草.", 9);//消息

            //查询种草放虫次数是否超xml 邵广林20160826
            if (new BCW.farm.BLL.NC_user().Get_zcfcnum(meid) < big_zcishu)
            {
                new BCW.farm.BLL.NC_user().update_zd("big_zfcishu=big_zfcishu+1", "usid=" + meid + "");
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename2, zcaofchong_jinbi, "在" + mename + "(" + usid + ")的农场种下1个杂草,获得" + zcaofchong_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (zcaofchong_jinbi)) + "金币.", 9);
                Utils.Success("操作土地", "成功在" + mename + "的农场第" + tudi + "块土地种下1个杂草." + jysm + "获得" + jy + "点经验和" + zcaofchong_jinbi + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
            }
            else
                Utils.Success("操作土地", "成功在" + mename + "的农场第" + tudi + "块土地种下1个杂草." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //(偷)放虫
    private void fchong1Page()
    {
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string mename2 = new BCW.BLL.User().GetUsName(usid);//用户姓名
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }
        //判断是否铲地为1，判断是否有草
        if ((!new BCW.farm.BLL.NC_tudi().Exists_chuchong(tudi, usid)) && new BCW.farm.BLL.NC_tudi().Exists_zhongcao(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmcc", 3);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=1,chongID=" + meid + ",z_chongtime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");

            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明
            if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = zcaofchong_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                }
                else
                {
                    jy = zcaofchong_jingyan;
                    jysm = "";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_shihuai=big_shihuai+1", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "经验已达上限.";

            //等级操作
            dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场第" + tudi + "块土地放下1条害虫.", 9);//消息
            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场第" + tudi + "块土地放下1条害虫.", 9);//消息

            //查询种草放虫次数是否超xml 邵广林20160826
            if (new BCW.farm.BLL.NC_user().Get_zcfcnum(meid) < big_zcishu)
            {
                new BCW.farm.BLL.NC_user().update_zd("big_zfcishu=big_zfcishu+1", "usid=" + meid + "");
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, zcaofchong_jinbi, "在" + mename2 + "(" + usid + ")的农场放下1条害虫,获得" + zcaofchong_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (zcaofchong_jinbi)) + "金币.", 9);
                Utils.Success("操作土地", "成功在" + mename2 + "的农场第" + tudi + "块土地放下1条虫子." + jysm + "获得" + jy + "点经验和" + zcaofchong_jinbi + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
            }
            else
                Utils.Success("操作土地", "成功在" + mename2 + "的农场第" + tudi + "块土地放下1条虫子." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //一键(偷)种草、放虫
    private void fangcao_2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype2 = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-2]$", "1"));//1种草2放虫
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//自己姓名
        string mename2 = new BCW.BLL.User().GetUsName(usid);//被偷姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }
        if (ptype2 == 1)
        {
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf3(28, meid))//查询是否有种草卡
            {
                int aa = 0;
                int bb = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and zuowu!='' and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and iscao!=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        //查询自己是否已种草
                        if (!new BCW.farm.BLL.NC_tudi().Exists_zcao(id, usid, meid))
                        {
                            //改时间.加字段
                            new BCW.farm.BLL.NC_tudi().update_tudi("iscao=1,caoID=" + meid + ",z_caotime=getdate()", "usid=" + usid + " and tudi=" + id + "");
                            if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
                            {
                                bb++;
                                new BCW.farm.BLL.NC_user().update_zd("big_shihuai=big_shihuai+1", "usid=" + meid + "");//每日上限
                            }
                            aa++;
                        }
                    }
                    if (aa > 0)
                    {
                        //减道具
                        //判断不可赠送是否有数量，如果无，就用可赠送的
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(28, meid, 1))
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 28, 1);
                        else
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 28, 0);
                        //双倍经验判断
                        long jy = 0;//经验
                        string jysm = string.Empty;//文字说明
                        if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
                        {
                            if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                            {
                                jy = zcaofchong_jingyan * bb * 2;
                                jysm = "由于你使用了双倍经验卡,经验翻倍.";
                            }
                            else
                            {
                                jy = zcaofchong_jingyan * bb;
                                jysm = "";
                            }
                            new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                        }
                        else
                            jysm = "经验已达上限.";

                        //等级操作
                        dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场一键种下" + aa + "个杂草.", 9);//消息
                        new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场一键种下" + aa + "个杂草.", 9);//消息

                        //查询种草放虫次数是否超xml 邵广林20160826
                        if (new BCW.farm.BLL.NC_user().Get_zcfcnum(meid) < big_zcishu)
                        {
                            if (aa > 0)
                            {
                                new BCW.farm.BLL.NC_user().update_zd("big_zfcishu=big_zfcishu+" + aa + "", "usid=" + meid + "");
                                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, zcaofchong_jinbi * aa, "在" + mename2 + "(" + usid + ")的农场一键种下" + aa + "个杂草,获得" + zcaofchong_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (zcaofchong_jinbi * aa)) + "金币.", 9);
                            }
                            Utils.Success("操作土地", "您使用一张种草卡,成功在" + mename2 + "的农场一键种下" + aa + "个杂草." + jysm + "获得" + jy + "点经验和" + zcaofchong_jinbi * aa + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                        }
                        else
                            Utils.Success("操作土地", "您使用一张种草卡,成功在" + mename2 + "的农场一键种下" + aa + "个杂草." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                    else
                        Utils.Error("抱歉,你已经在Ta土地种过草了,不能重复种草.", "");
                }
                else
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
            else
                Utils.Error("抱歉,你的种草卡不足.<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=28") + "\">马上购买种草卡</a>", "");
        }
        else
        {
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf3(29, meid))//查询是否有放虫卡
            {
                int aa = 0;
                int bb = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and zuowu!='' and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and isinsect!=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        //查询自己是否已种草
                        if (!new BCW.farm.BLL.NC_tudi().Exists_zchong(id, usid, meid))
                        {
                            //改时间.加字段
                            new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=1,chongID=" + meid + ",z_chongtime=getdate()", "usid=" + usid + " and tudi=" + id + "");
                            if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
                            {
                                bb++;
                                new BCW.farm.BLL.NC_user().update_zd("big_shihuai=big_shihuai+1", "usid=" + meid + "");//每日上限
                            }
                            aa++;
                        }
                    }
                    if (aa > 0)
                    {
                        //双倍经验判断
                        long jy = 0;//经验
                        string jysm = string.Empty;//文字说明
                        if (sh_big_jingyan > (new BCW.farm.BLL.NC_user().Get_shjingyan(meid)))
                        {
                            if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                            {
                                jy = zcaofchong_jingyan * bb * 2;
                                jysm = "由于你使用了双倍经验卡,经验翻倍.";
                            }
                            else
                            {
                                jy = zcaofchong_jingyan * bb;
                                jysm = "";
                            }
                            new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                        }
                        else
                            jysm = "经验已达上限.";


                        //等级操作
                        dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场一键放下" + aa + "条虫.", 9);//消息
                        new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场一键放下" + aa + "条虫.", 9);//消息
                        //减道具
                        //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 29);

                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(29, meid, 1))
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 29, 1);
                        else
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 29, 0);

                        //查询种草放虫次数是否超xml 邵广林20160826
                        if (new BCW.farm.BLL.NC_user().Get_zcfcnum(meid) < big_zcishu)
                        {
                            if (aa > 0)
                            {
                                new BCW.farm.BLL.NC_user().update_zd("big_zfcishu=big_zfcishu+" + aa + "", "usid=" + meid + "");
                                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, zcaofchong_jinbi * aa, "在" + mename2 + "(" + usid + ")的农场一键放下" + aa + "条虫,获得" + zcaofchong_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (zcaofchong_jinbi * aa)) + "金币.", 9);
                            }
                            Utils.Success("操作土地", "您使用一张放虫卡,成功在" + mename2 + "的农场一键放下" + aa + "条虫." + jysm + "获得" + jy + "点经验和" + zcaofchong_jinbi * aa + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                        }
                        else
                            Utils.Success("操作土地", "您使用一张放虫卡,成功在" + mename2 + "的农场一键放下" + aa + "条虫." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                    else
                        Utils.Error("抱歉,你已经在Ta土地放过虫了,不能重复放虫.", "");
                }
                else
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
            else
                Utils.Error("抱歉,你的放虫卡不足.<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=29") + "\">马上购买放虫卡</a>", "");
        }
    }

    //除虫
    private void chuchongPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        if (new BCW.farm.BLL.NC_tudi().Exists_chuchong(tudi, usid))
        {
            BCW.User.Users.IsFresh("Farmcc", 3);//防刷
            new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,chongID='',z_chongtime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
            //任务
            BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(1, meid);
            if (wr.usid > 0)
            {
                new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+1", "usid=" + meid + " and task_id=1 and type=0");
            }
            //双倍经验判断
            long jy = 0;//经验
            string jysm = string.Empty;//文字说明

            if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
            {
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = all_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                }
                else
                {
                    jy = all_jingyan;
                    jysm = "获得" + jy + "经验和";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(usid, jy);
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");//每日上限
            }
            else
                jysm = "获得";

            new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场第" + tudi + "块土地消灭1条害虫.", 1);//消息

            //查询除草除虫次数是否超xml 邵广林20160826
            if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
            {
                new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+1", "usid=" + meid + "");
                new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename, all_jinbi, "在农场第" + tudi + "块土地消灭1条害虫,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) + all_jinbi) + "金币.", 1);
                Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块土地消灭1条害虫." + jysm + "" + all_jinbi + "金币", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
            }
            else
                Utils.Success("操作土地", "您成功在自己农场第" + tudi + "块土地消灭1条害虫.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
    }

    //(偷)除虫
    private void chuchong2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename2 = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        if (new BCW.farm.BLL.NC_tudi().Exists_chuchong(tudi, usid))
        {
            if (!new BCW.farm.BLL.NC_tudi().Exists_zchong(tudi, usid, meid))//判断是否自己种的草
            {
                BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,chongID='',z_chongtime=getdate()", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                //任务
                BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(1, meid);
                if (wr.usid > 0)
                {
                    new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+1", "usid=" + meid + " and task_id=1 and type=0");
                }
                //双倍经验判断
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明

                if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                {
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = all_jingyan / 2 * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = all_jingyan / 2;
                        jysm = "";
                    }
                    //每日上限
                    new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");
                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                }
                else
                    jysm = "经验已达上限.";

                //等级操作
                dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename2, "在" + mename + "(" + usid + ")的农场第" + tudi + "块土地消灭1条害虫.", 1);//自己的消息
                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "" + mename2 + "(" + meid + ")来农场帮忙,在第" + tudi + "块土地消灭1条害虫.", 1);//被偷的消息

                //查询除草除虫次数是否超xml 邵广林20160826
                if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                {
                    new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+1", "usid=" + meid + "");
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename2, all_jinbi, "在" + mename + "的农场第" + tudi + "块土地消灭1条害虫,获得" + all_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + all_jinbi) + "金币.", 1);
                    Utils.Success("操作土地", "您成功在" + mename + "的农场第" + tudi + "块土地消灭1条害虫." + jysm + "获得" + jy + "点经验和" + all_jinbi + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
                }
                else
                    Utils.Success("操作土地", "您成功在" + mename + "的农场第" + tudi + "块土地消灭1条害虫." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");

            }
            else
                Utils.Error("你太坏了,不能除掉自己放的虫.", "");
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + fenye + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //一键、除虫
    private void chuchong_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //邵广林 20161114 加密传时间，判断是否操作超时
        string GKeyStr = Utils.GetRequest("GG", "all", 1, @"^[^\^]{1,20}$", "");
        if (GKeyStr == "")
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键除虫操作中存在非法链接%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键除虫操作中存在非法链接.");
            Utils.Error("非法操作链接.", Utils.getUrl("farm.aspx"));
        }
        int GKeyID = GetRoomID(GKeyStr);
        string yy = DateTime.Now.ToString("yyyy-MM-dd ") + GKeyID.ToString("00:00:00");
        DateTime qq = Convert.ToDateTime(yy);
        if (qq.AddSeconds(-30) > DateTime.Now || DateTime.Now > qq.AddSeconds(30))
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键除虫操作中存在超时情况%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键除虫操作中存在超时情况.");
            Utils.Error("亲,你操作超时咯.请重新除虫!", Utils.getUrl("farm.aspx"));
        }

        Master.Title = "一键除虫";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;温馨提示");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < chuchong1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键除虫需要" + chuchong1_grade + "级才能开通哦!");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getchuchong(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键除虫功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=3&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + chuchong1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int aa = 0;
                int bb = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and isinsect=1 and ischandi=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,chongID='',z_chongtime=getdate()", "usid=" + meid + " and tudi=" + id + "");
                        if (zj_big_jingyan > (new BCW.farm.BLL.NC_user().Get_zjjingyan(meid)))
                        {
                            //每日上限
                            new BCW.farm.BLL.NC_user().update_zd("big_zjcaozuo=big_zjcaozuo+2", "usid=" + meid + "");
                            bb++;
                        }
                        aa++;
                    }
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if (bb > 0)
                    {
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = all_jingyan * bb * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.获得" + jy + "经验和";
                        }
                        else
                        {
                            jy = all_jingyan * bb;
                            jysm = "获得" + jy + "经验和";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    }
                    else
                        jysm = "获得";

                    //任务
                    BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(1, meid);
                    if (wr.usid > 0)
                    {
                        new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=1 and type=0");
                    }

                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场消灭" + aa + "条害虫.", 1);//消息

                    //查询除草除虫次数是否超xml 邵广林20160826
                    if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                    {
                        new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+" + aa + "", "usid=" + meid + "");
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在农场消灭" + aa + "条害虫,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                        Utils.Success("操作土地", "您成功消灭" + aa + "条害虫." + jysm + "" + all_jinbi * aa + "金币", Utils.getUrl("farm.aspx"), "1");
                    }
                    else
                        Utils.Success("操作土地", "您成功消灭" + aa + "条害虫.", Utils.getUrl("farm.aspx"), "1");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
        }
        foot_link();//底部链接

        foot_link2();
    }

    //一键、(偷)除虫
    private void chuchong_2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//自己姓名
        string mename2 = new BCW.BLL.User().GetUsName(usid);//被偷姓名

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;

        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
        }
        if (DateTime.Now < viptime)
        {
            int aa = 0;
            int bb = 0;
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and isinsect=1 and ischandi=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                BCW.User.Users.IsFresh("Farmcc", 3);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                    if (!new BCW.farm.BLL.NC_tudi().Exists_zchong(id, usid, meid))//判断是否自己种的草
                    {
                        new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,chongID='',z_chongtime=getdate()", "usid=" + usid + " and tudi=" + id + "");
                        if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                        {
                            //每日上限
                            new BCW.farm.BLL.NC_user().update_zd("big_bangmang=big_bangmang+2", "usid=" + meid + "");
                            bb++;
                        }
                        aa++;
                    }
                }
                if (aa > 0)
                {
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    //if (bm_big_jingyan > (new BCW.farm.BLL.NC_user().Get_bmjingyan(meid)))
                    if (bb > 0)
                    {
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = all_jingyan / 2 * bb * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.";
                        }
                        else
                        {
                            jy = all_jingyan / 2 * bb;
                            jysm = "";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);//加经验
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验

                    }
                    else
                        jysm = "经验已达上限.";
                    //任务
                    BCW.farm.Model.NC_tasklist wr = new BCW.farm.BLL.NC_tasklist().Get_renwu(1, meid);
                    if (wr.usid > 0)
                    {
                        new BCW.farm.BLL.NC_tasklist().update_renwu("task_oknum=task_oknum+" + aa + "", "usid=" + meid + " and task_id=1 and type=0");
                    }

                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160922 增加各等级升级所需的经验
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场消灭" + aa + "条害虫.", 1);//消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场帮忙,除去" + aa + "条害虫.", 1);//消息


                    //查询除草除虫次数是否超xml 邵广林20160826
                    if (new BCW.farm.BLL.NC_user().Get_ccccnum(meid) < big_ccishu)
                    {
                        if (aa > 0)
                        {
                            new BCW.farm.BLL.NC_user().update_zd("big_cccishu=big_cccishu+" + aa + "", "usid=" + meid + "");
                            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, all_jinbi * aa, "在" + mename2 + "的农场消灭" + aa + "条害虫,获得" + all_jinbi * aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (all_jinbi * aa)) + "金币.", 1);
                        }
                        Utils.Success("操作土地", "您成功在" + mename2 + "的农场消灭" + aa + "条害虫." + jysm + "获得" + jy + "点经验和" + all_jinbi * aa + "金币.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                    else
                        Utils.Success("操作土地", "您成功在" + mename2 + "的农场消灭" + aa + "条害虫." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                }
                else
                    Utils.Error("你太坏了,不能除掉自己放的虫.", "");
            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
        }
        else
        {
            Utils.Error("抱歉,只有vip会员才可以一键除虫哦,请开通vip再来,<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">马上开通</a>", "");
        }
    }

    //收获
    private void shouhuoPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");

        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名

        if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo(tudi, usid))//是否存在可以收获的土地
        {
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and tudi=" + tudi + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                int zuowu_experience = 0;
                string[] ab = new string[2];
                string zuowu = string.Empty;
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明
                BCW.User.Users.IsFresh("Farmsh", 2);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string output = string.Empty;
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());//土地块数
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                    zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                    int zuowu_time = int.Parse(ds.Tables[0].Rows[i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                    //DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);//作物种植时间
                    int tudi_type = int.Parse(ds.Tables[0].Rows[i]["tudi_type"].ToString());//土地类型
                    int zuowu_ji = int.Parse(ds.Tables[0].Rows[i]["zuowu_ji"].ToString());//作物生长的季度
                    int iscao = int.Parse(ds.Tables[0].Rows[i]["iscao"].ToString());//除草
                    int iswater = int.Parse(ds.Tables[0].Rows[i]["iswater"].ToString());//是否浇水
                    int isinsect = int.Parse(ds.Tables[0].Rows[i]["isinsect"].ToString());//昆虫
                    int ischandi = int.Parse(ds.Tables[0].Rows[i]["ischandi"].ToString());//铲地(0空1有2枯萎)
                    output = ds.Tables[0].Rows[i]["output"].ToString();//产/剩
                    zuowu_experience = int.Parse(ds.Tables[0].Rows[i]["zuowu_experience"].ToString());//作物经验
                    int harvest = int.Parse(ds.Tables[0].Rows[i]["harvest"].ToString());//收获季度

                    if (zuowu_ji >= harvest)
                    {
                        ab = output.Split(',');
                        //双倍经验判断
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = zuowu_experience * 2;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.";
                        }
                        else
                        {
                            jy = zuowu_experience;
                            jysm = "";
                        }

                        if ((zuowu_ji - harvest) == 0)//种植季度=收获季度,可以铲地
                        {
                            new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,ischandi=2", " usid = '" + UsID + "' AND tudi='" + id + "'");//可以铲地
                            new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                            BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                            qq.name = zuowu;
                            qq.num = int.Parse(ab[1]);
                            BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                            qq.price_out = w.price_out;//邵广林 20160603 修改卖出价格
                            qq.name_id = w.num_id;
                            qq.suoding = 0;
                            qq.usid = UsID;
                            qq.get_nums = int.Parse(ab[1]);
                            qq.tou_nums = 0;
                            if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                            {
                                new BCW.farm.BLL.NC_GetCrop().Add(qq);
                            }
                            else
                            {
                                new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                            }
                        }
                        else//收获季度+1,更新种植时间
                        {
                            int b = harvest + 1;
                            if (b == 2)//如果等于第2个季度，就把时间减少
                            {
                                new BCW.farm.BLL.NC_tudi().update_tudi("zuowu_time=" + (zuowu_time * 2 / 5) + "", "usid = '" + UsID + "' AND tudi='" + id + "'");
                            }
                            string nn = ab[0] + "," + ab[0];
                            new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,z_shuinum='',caoID='',chongID='',touID='',output='" + nn + "',isshifei=0,harvest=" + b + " , updatetime='" + DateTime.Now + "'", "usid = '" + UsID + "' AND tudi='" + id + "'");//更新种植时间
                            new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                            BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                            qq.name = zuowu;
                            qq.num = int.Parse(ab[1]);
                            BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                            qq.price_out = w.price_out;
                            qq.name_id = w.num_id;
                            qq.usid = UsID;
                            qq.get_nums = int.Parse(ab[1]);
                            qq.tou_nums = 0;
                            if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                            {
                                qq.suoding = 0;
                                new BCW.farm.BLL.NC_GetCrop().Add(qq);
                            }
                            else
                            {
                                new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                            }
                        }
                    }
                }
                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场收获" + id + "#" + zuowu + "" + ab[1] + "个." + jysm + "获得" + jy + "经验.", 8);//消息
                chandi_get(3);
                Utils.Success("操作土地", "在第" + id + "块土地收获成功,获得" + zuowu + "" + ab[1] + "个." + jysm + "获得" + jy + "经验", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
            }
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
    }

    //(偷)偷菜--偷菜没经验
    private void shouhuo2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int pageIndex = Utils.ParseInt(Utils.GetRequest("pageIndex", "all", 1, @"^[1-9]\d*$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);
        string mename2 = new BCW.BLL.User().GetUsName(usid);

        if (new BCW.farm.BLL.NC_slave().Exists_nl(meid, usid))//是否已经是该usid的奴隶
        {
            Utils.Success("操作土地", "抱歉,该ID(" + usid + ")是你的主人,奴隶不可以偷主人的菜哦.", Utils.getUrl("farm.aspx?act=toucai"), "2");
        }

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.请返回首页查看.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        is_shougeji(usid);//自动收割

        if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo(tudi, usid))//是否存在可以偷取的土地
        {
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and tudi=" + tudi + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                int zuowu_experience = 0;
                string[] ab = new string[2];
                string[] tt = new string[2];
                string zuowu = string.Empty;
                int a = 0;
                int uu = 0;
                string ii = string.Empty;
                string jj = string.Empty;
                int tounum = 0;
                string hhh = "";//陷阱说明
                BCW.User.Users.IsFresh("Farmtq", 2);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string output = string.Empty;
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());//土地块数
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                    zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                    int zuowu_time = int.Parse(ds.Tables[0].Rows[i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                    DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);//作物种植时间
                    int tudi_type = int.Parse(ds.Tables[0].Rows[i]["tudi_type"].ToString());//土地类型
                    int zuowu_ji = int.Parse(ds.Tables[0].Rows[i]["zuowu_ji"].ToString());//作物生长的季度
                    int iscao = int.Parse(ds.Tables[0].Rows[i]["iscao"].ToString());//除草
                    int iswater = int.Parse(ds.Tables[0].Rows[i]["iswater"].ToString());//是否浇水
                    int isinsect = int.Parse(ds.Tables[0].Rows[i]["isinsect"].ToString());//昆虫
                    int ischandi = int.Parse(ds.Tables[0].Rows[i]["ischandi"].ToString());//铲地(0空1有2枯萎)
                    output = ds.Tables[0].Rows[i]["output"].ToString();//产/剩
                    zuowu_experience = int.Parse(ds.Tables[0].Rows[i]["zuowu_experience"].ToString());//作物经验
                    int harvest = int.Parse(ds.Tables[0].Rows[i]["harvest"].ToString());//收获季度
                    string touID = ds.Tables[0].Rows[i]["touID"].ToString();//偷菜人的ID
                    int xianjing = int.Parse(ds.Tables[0].Rows[i]["xianjing"].ToString());//陷阱

                    if (zuowu_ji >= harvest)
                    {
                        ab = output.Split(',');//拆分产剩
                        tt = touID.Split(',');//拆分用户id
                        //得到偷的人数
                        string[] sNum = Regex.Split(touID, ",");
                        int bb = sNum.Length;

                        for (int bd = 0; bd < tt.Length; bd++)
                        {
                            if (tt[bd] == meid.ToString())
                            {
                                a++;
                            }
                        }
                        //随机偷取1-2个果实
                        Random rac = new Random();
                        tounum = rac.Next(1, 3);
                        tounum = 1;//20160824 邵广林 设置偷取为1个
                        if (a == 0)
                        {
                            if (bb < tou_renshu + 2)//如果偷取的人数少于或等于设定的人数
                            {
                                //自己仓库加作物
                                BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                qq.name = zuowu;
                                qq.num = tounum;
                                BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                qq.price_out = w.price_out;
                                qq.name_id = w.num_id;
                                qq.usid = meid;
                                qq.get_nums = 0;
                                qq.tou_nums = tounum;
                                if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, meid))
                                {
                                    qq.suoding = 0;
                                    new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                }
                                else
                                {
                                    new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                }
                                //被偷的用户减作物
                                uu = int.Parse(ab[1]) - tounum;
                                ii = ab[0] + "," + uu;
                                new BCW.farm.BLL.NC_tudi().update_tudi("output='" + ii + "'", "usid=" + usid + " and tudi=" + tudi + "");
                                //加上已偷id
                                if (touID.Length < 1)
                                {
                                    //jj = meid.ToString();
                                    jj = "," + meid.ToString() + ",";
                                }
                                else
                                {
                                    jj = touID + meid + ",";
                                }
                                new BCW.farm.BLL.NC_tudi().update_tudi("touID='" + jj + "'", "usid=" + usid + " and tudi=" + tudi + "");

                                if (xianjing == 1)//有陷阱
                                {
                                    int kk = Get_xianjing();
                                    if (kk == 2)//中陷阱
                                    {
                                        if (!new BCW.farm.BLL.NC_slave().Exists_nl(meid, usid))//是否已经是该usid的奴隶
                                        {
                                            //陷阱字段改为0
                                            new BCW.farm.BLL.NC_tudi().update_tudi("xianjing=0", "usid=" + UsID + " and tudi=" + id + "");
                                            new BCW.farm.BLL.NC_messagelog().addmessage(UsID, mename2, "" + mename + "(" + meid + ")踩中了农场第" + tudi + "块土地的陷阱成为我的奴隶,[url=/bbs/game/farm.aspx?act=punish]马上惩罚奴隶[/url].", 2);//消息
                                            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "不幸踩中了" + mename2 + "(" + UsID + ")农场第" + tudi + "块土地的陷阱,成为Ta的奴隶.", 2);//消息
                                            //减100金币
                                            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -xianjing_jb, "去" + mename2 + "农场偷菜不小心踩到Ta农场第" + tudi + "块土地的陷阱，损失我" + xianjing_jb + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - (xianjing_jb)) + "金币.", 2);
                                            //加100金币
                                            new BCW.farm.BLL.NC_user().UpdateiGold(UsID, mename2, xianjing_jb, "" + mename + "踩中了农场第" + tudi + "块土地的陷阱,奖励我" + xianjing_jb + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (xianjing_jb)) + "金币.", 2);
                                            //道具使用表type改为0
                                            new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "usid=" + UsID + " and tudi=" + id + " and daoju_id=21");

                                            if (new BCW.farm.BLL.NC_slave().Exists_nl2(meid, usid))//如果存在奴隶记录，则刷新
                                            {
                                                BCW.farm.Model.NC_slave gg = new BCW.farm.Model.NC_slave();
                                                gg.punish = 0;
                                                gg.pacify = 0;
                                                gg.updatetime = DateTime.Now;
                                                gg.tpye = 1;
                                                gg.usid = usid;
                                                gg.slave_id = meid;
                                                new BCW.farm.BLL.NC_slave().Update_nl(gg);
                                            }
                                            else//添加
                                            {
                                                BCW.farm.Model.NC_slave rr = new BCW.farm.Model.NC_slave();
                                                rr.pacify = 0;
                                                rr.punish = 0;
                                                rr.slave_id = meid;
                                                rr.usid = usid;
                                                rr.updatetime = DateTime.Now;
                                                rr.tpye = 1;
                                                rr.num = 1;
                                                new BCW.farm.BLL.NC_slave().Add(rr);
                                            }
                                            hhh = "一不小心，踩中了" + mename2 + "农场第" + tudi + "块土地设置的陷阱，成为了Ta的奴隶，金币减少" + xianjing_jb + ".";
                                            //动态记录
                                            new BCW.BLL.Action().Add(1011, 0, usid, new BCW.BLL.User().GetUsName(usid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]的陷阱抓到偷菜者.");
                                            //内线给被偷者
                                            new BCW.BLL.Guest().Add(1, usid, mename2, "" + mename + "一不小心,踩中农场第" + tudi + "块土地的陷阱,成为我的奴隶.[url=/bbs/game/farm.aspx?act=punish]马上去惩罚奴隶[/url]");
                                            //内线给去偷者
                                            new BCW.BLL.Guest().Add(1, meid, mename, "偷菜时一不小心,踩中" + mename2 + "农场第" + tudi + "块土地的陷阱,成为他的奴隶,下次偷菜要小心点了.我也去[url=/bbs/game/farm.aspx?act=xianjing]设置陷阱[/url].继续[url=/bbs/game/farm.aspx?act=toucai]去偷菜[/url]");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //加上已偷id
                                if (touID.Length < 1)
                                {
                                    //jj = meid.ToString();
                                    jj = "," + meid.ToString() + ",";
                                }
                                else
                                {
                                    jj = touID + meid + ",";
                                }
                                new BCW.farm.BLL.NC_tudi().update_tudi("touID='" + jj + "'", "usid=" + usid + " and tudi=" + tudi + "");
                                Utils.Success("操作土地", "这块地里果实已经所剩无几,不能再偷了,请手下留情", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + pageIndex + "&amp;ptype=" + ptype + ""), "1");
                            }
                        }
                        else
                        {
                            Utils.Error("每块土地只能偷一次哦,手下留情吧.", "");
                        }
                    }
                }
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场摘取" + id + "#" + zuowu + "" + tounum + "个.", 9);//消息
                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场摘取" + id + "#" + zuowu + "" + tounum + "个.[url=/bbs/game/farm.aspx?act=do&amp;uid=" + meid + "]马上去Ta的农场[/url]", 9);//消息
                if (hhh == "")
                {
                    Utils.Success("操作土地", "成功在" + mename2 + "的农场第" + id + "块土地摘取了" + zuowu + "" + tounum + "个.", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + pageIndex + "&amp;ptype=" + ptype + ""), "1");
                }
                else
                    Utils.Success("操作土地", "成功在" + mename2 + "的农场第" + id + "块土地摘取了" + zuowu + "" + tounum + "个.<br/>" + hhh + "", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;page=" + pageIndex + "&amp;ptype=" + ptype + ""), "2");
            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
        }
    }

    //一键、收获
    private void shouhuo_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //邵广林 20161025 加密传时间，判断是否操作超时

        string GKeyStr = Utils.GetRequest("GKeyStr", "all", 1, @"^[^\^]{1,20}$", "");
        if (GKeyStr == "")
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键收获操作中存在非法链接%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键收获操作中存在非法链接.");
            Utils.Error("非法操作链接.", Utils.getUrl("farm.aspx"));
        }
        int GKeyID = GetRoomID(GKeyStr);
        string yy9 = DateTime.Now.ToString("yyyy-MM-dd ") + GKeyID.ToString("00:00:00");
        DateTime qq9 = Convert.ToDateTime(yy9);
        if (qq9.AddSeconds(-30) > DateTime.Now && DateTime.Now > qq9.AddSeconds(30))
        {
            DataSet we = new BCW.BLL.Guest().GetList("COUNT(*) as a", "ToId=52784 AND Content LIKE '%违法操作：ID" + meid + "在" + GameName + "一键收获操作中存在超时情况%'");
            if (we.Tables[0].Rows[0]["a"].ToString() == "0")
                new BCW.BLL.Guest().Add(1, 52784, "森林仔777", "违法操作：ID" + meid + "在" + GameName + "一键收获操作中存在超时情况.");
            Utils.Error("亲,你操作超时咯.请重新收获!", Utils.getUrl("farm.aspx"));
        }


        Master.Title = "一键收获";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;温馨提示");
        builder.Append(Out.Tab("</div>", "<br />"));

        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < shouhuo1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键收获需要" + shouhuo1_grade + "级才能开通哦!");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getshou(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键收获功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=4&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + shouhuo1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    int zuowu_experience = 0;
                    string[] ab = new string[2];
                    string zuowu = string.Empty;
                    string xx = string.Empty;
                    int yy = 0;
                    string jysm = string.Empty;//文字说明
                    BCW.User.Users.IsFresh("Farmyjsh", 2);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string output = string.Empty;
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                        zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                        int zuowu_ji = int.Parse(ds.Tables[0].Rows[i]["zuowu_ji"].ToString());//作物生长的季度
                        output = ds.Tables[0].Rows[i]["output"].ToString();//产/剩
                        zuowu_experience = int.Parse(ds.Tables[0].Rows[i]["zuowu_experience"].ToString());//作物经验
                        int harvest = int.Parse(ds.Tables[0].Rows[i]["harvest"].ToString());//收获季度
                        int zuowu_time = int.Parse(ds.Tables[0].Rows[i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                        //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0", "usid=" + meid + " and tudi=" + id + "");

                        if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo(id, UsID))//是否存在可以收获的土地
                        {
                            if (zuowu_ji >= harvest)
                            {
                                ab = output.Split(',');
                                //双倍经验判断
                                long jy = 0;//经验
                                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                                {
                                    jy = zuowu_experience * 2;
                                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                                }
                                else
                                {
                                    jy = zuowu_experience;
                                    jysm = "";
                                }

                                if ((zuowu_ji - harvest) == 0)//种植季度=收获季度,可以铲地
                                {
                                    new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,ischandi=2", " usid = '" + UsID + "' AND tudi='" + id + "'");//可以铲地
                                    new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                    BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                    qq.name = zuowu;
                                    qq.num = int.Parse(ab[1]);
                                    BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                    qq.price_out = w.price_out;
                                    qq.name_id = w.num_id;
                                    qq.suoding = 0;
                                    qq.usid = UsID;
                                    qq.get_nums = int.Parse(ab[1]);
                                    qq.tou_nums = 0;
                                    if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                    }
                                    else
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                    }
                                }
                                else//收获季度+1,更新种植时间
                                {
                                    int b = harvest + 1;
                                    if (b == 2)//如果等于第2个季度，就把时间减少
                                    {
                                        new BCW.farm.BLL.NC_tudi().update_tudi("zuowu_time=" + (zuowu_time * 2 / 5) + "", "usid = '" + UsID + "' AND tudi='" + id + "'");
                                    }
                                    string nn = ab[0] + "," + ab[0];
                                    new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,iswater=0,isinsect=0,z_shuinum='',caoID='',chongID='',touID='',output='" + nn + "',isshifei=0,harvest=" + b + " , updatetime='" + DateTime.Now + "'", "usid = '" + UsID + "' AND tudi='" + id + "'");//更新种植时间
                                    new BCW.farm.BLL.NC_user().Update_Experience(UsID, jy);//+经验
                                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                    BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                    qq.name = zuowu;
                                    qq.num = int.Parse(ab[1]);
                                    BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                    qq.price_out = w.price_out;
                                    qq.name_id = w.num_id;
                                    qq.usid = UsID;
                                    qq.get_nums = int.Parse(ab[1]);
                                    qq.tou_nums = 0;
                                    if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, UsID))
                                    {
                                        qq.suoding = 0;
                                        new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                    }
                                    else
                                    {
                                        new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                    }
                                }
                                xx = xx + id + "#" + zuowu + ab[1] + "个,";
                                yy = (int)jy + yy;
                            }
                        }
                        else
                        {
                            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                        }
                    }
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场收获" + xx + "" + jysm + "总计" + yy + "经验", 8);//消息
                    chandi_get(4);
                    Utils.Success("操作土地", "收获成功,获得" + xx + "" + jysm + "总计" + yy + "经验.", Utils.getUrl("farm.aspx"), "2");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
        }
        foot_link();//底部链接

        foot_link2();
    }

    //一键、(偷)偷菜
    private void shouhuo_2Page()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        string mename2 = new BCW.BLL.User().GetUsName(usid);//用户姓名

        if (new BCW.farm.BLL.NC_slave().Exists_nl(meid, usid))//是否已经是该usid的奴隶
        {
            Utils.Success("操作土地", "抱歉,该ID(" + usid + ")是你的主人,奴隶不可以偷主人的菜哦.", Utils.getUrl("farm.aspx?act=toucai"), "2");
        }

        if (meid == usid)
        {
            Utils.Error("不能去自己的农场.请返回首页查看.", "");
        }
        if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == false) || ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//判断是否为好友
        {
            if (((new BCW.BLL.Friend().Exists(meid, usid, 0)) == true) && ((new BCW.BLL.Friend().Exists(usid, meid, 0)) == false))//提示对方加你
            {
                Utils.Error("你不在他的好友列表,<a href=\"" + Utils.getUrl("farm.aspx?act=faneixian&amp;usid=" + usid + "") + "\">向TA申请加您为Ta的好友</a>.", "");
            }
            else///你没有对方
            {
                Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;id=1&amp;hid=" + usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");
            }
        }

        //识别会员vip
        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
        DateTime viptime = DateTime.Now;

        try
        {
            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
        }
        catch
        {
            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
        }

        if (DateTime.Now < viptime)
        {
            is_shougeji(usid);//自动收割

            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + usid + " and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int id = 1;
                int istou = 0;//判断是否有一键收获
                int zuowu_experience = 0;
                string[] ab = new string[2];
                string zuowu = string.Empty;
                string[] tt = new string[2];
                int uu = 0;
                string ii = string.Empty;
                string jj = string.Empty;
                int tounum = 0;
                string xx = string.Empty;
                int yy = 0;
                string hhh = "";//陷阱说明
                BCW.User.Users.IsFresh("Farmyjtq", 2);//防刷
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string output = string.Empty;
                    string touID = string.Empty;
                    id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                    int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                    zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                    int zuowu_ji = int.Parse(ds.Tables[0].Rows[i]["zuowu_ji"].ToString());//作物生长的季度
                    output = ds.Tables[0].Rows[i]["output"].ToString();//产/剩
                    zuowu_experience = int.Parse(ds.Tables[0].Rows[i]["zuowu_experience"].ToString());//作物经验
                    int harvest = int.Parse(ds.Tables[0].Rows[i]["harvest"].ToString());//收获季度
                    touID = ds.Tables[0].Rows[i]["touID"].ToString();//偷菜人的ID
                    int xianjing = int.Parse(ds.Tables[0].Rows[i]["xianjing"].ToString());//陷阱

                    if (new BCW.farm.BLL.NC_tudi().Exists_shouhuo(id, UsID))//是否存在可以收获的土地
                    {
                        ab = output.Split(',');//拆分产剩
                        tt = touID.Split(',');//拆分用户id
                        //得到偷的人数
                        string[] sNum = Regex.Split(touID, ",");
                        int bb = sNum.Length;
                        int a = 0;
                        for (int bd = 0; bd < tt.Length; bd++)
                        {
                            if (tt[bd] == meid.ToString())
                            {
                                a++;
                            }
                        }
                        //随机偷取1-2个果实
                        Random rac = new Random();
                        tounum = rac.Next(1, 3);
                        tounum = 1;//20160824 邵广林 设置偷取为1个
                        if (a == 0)
                        {
                            if (bb < tou_renshu + 2)//如果偷取的人数少于或等于设定的人数
                            {
                                //自己仓库加作物
                                BCW.farm.Model.NC_GetCrop qq = new BCW.farm.Model.NC_GetCrop();//实例化
                                qq.name = zuowu;
                                qq.num = tounum;
                                BCW.farm.Model.NC_shop w = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);
                                qq.price_out = w.price_out;
                                qq.name_id = w.num_id;
                                qq.usid = meid;
                                qq.get_nums = 0;
                                qq.tou_nums = tounum;
                                if (!new BCW.farm.BLL.NC_GetCrop().Exists_zuowu(zuowu, meid))
                                {
                                    qq.suoding = 0;
                                    new BCW.farm.BLL.NC_GetCrop().Add(qq);
                                }
                                else
                                {
                                    new BCW.farm.BLL.NC_GetCrop().Update1(qq);
                                }
                                //被偷的用户减作物
                                uu = int.Parse(ab[1]) - tounum;
                                ii = ab[0] + "," + uu;
                                new BCW.farm.BLL.NC_tudi().update_tudi("output='" + ii + "'", "usid=" + usid + " and tudi=" + id + "");
                                //加上已偷id
                                if (touID.Length < 1)
                                {
                                    //jj = meid.ToString();//邵广林 20160525 修改查询touid字段，多加,,
                                    jj = "," + meid.ToString() + ",";
                                }
                                else
                                {
                                    jj = touID + meid + ",";
                                }
                                new BCW.farm.BLL.NC_tudi().update_tudi("touID='" + jj + "'", "usid=" + usid + " and tudi=" + id + "");

                                if (xianjing == 1)//有陷阱
                                {
                                    int kk = Get_xianjing();
                                    if (kk == 2)//中陷阱
                                    {
                                        if (!new BCW.farm.BLL.NC_slave().Exists_nl(meid, usid))//是否已经是该usid的奴隶
                                        {
                                            //陷阱字段改为0
                                            new BCW.farm.BLL.NC_tudi().update_tudi("xianjing=0", "usid=" + UsID + " and tudi=" + id + "");
                                            new BCW.farm.BLL.NC_messagelog().addmessage(UsID, mename2, "" + mename + "(" + meid + ")踩中了农场第" + id + "块土地的陷阱成为我的奴隶,[url=/bbs/game/farm.aspx?act=punish]马上惩罚奴隶[/url].", 2);//消息
                                            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "不幸踩中了" + mename2 + "(" + UsID + ")农场第" + id + "块土地的陷阱,成为Ta的奴隶.", 2);//消息
                                            //减100金币
                                            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -xianjing_jb, "去" + mename2 + "农场偷菜不小心踩到Ta农场第" + id + "块土地的陷阱，损失我" + xianjing_jb + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - (xianjing_jb)) + "金币.", 2);
                                            //加100金币
                                            new BCW.farm.BLL.NC_user().UpdateiGold(UsID, mename2, xianjing_jb, "" + mename + "踩中了农场第" + id + "块土地的陷阱,奖励我" + xianjing_jb + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (xianjing_jb)) + "金币.", 2);
                                            //道具使用表type改为0
                                            new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "usid=" + UsID + " and tudi=" + id + " and daoju_id=21");

                                            if (new BCW.farm.BLL.NC_slave().Exists_nl2(meid, usid))//如果存在奴隶记录，则刷新
                                            {
                                                BCW.farm.Model.NC_slave gg = new BCW.farm.Model.NC_slave();
                                                gg.punish = 0;
                                                gg.pacify = 0;
                                                gg.updatetime = DateTime.Now;
                                                gg.tpye = 1;
                                                gg.usid = usid;
                                                gg.slave_id = meid;
                                                new BCW.farm.BLL.NC_slave().Update_nl(gg);
                                            }
                                            else//添加
                                            {
                                                BCW.farm.Model.NC_slave rr = new BCW.farm.Model.NC_slave();
                                                rr.pacify = 0;
                                                rr.punish = 0;
                                                rr.slave_id = meid;
                                                rr.usid = usid;
                                                rr.updatetime = DateTime.Now;
                                                rr.tpye = 1;
                                                rr.num = 1;
                                                new BCW.farm.BLL.NC_slave().Add(rr);
                                            }
                                            hhh = "一不小心，踩中了" + mename2 + "农场第" + id + "块土地设置的陷阱，成为了Ta的奴隶，金币减少" + xianjing_jb + ".";
                                            //动态记录
                                            new BCW.BLL.Action().Add(1011, 0, usid, new BCW.BLL.User().GetUsName(usid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]的陷阱抓到偷菜者.");
                                            //内线给被偷者
                                            new BCW.BLL.Guest().Add(1, usid, mename2, "" + mename + "一不小心,踩中农场第" + id + "块土地的陷阱,成为我的奴隶.[url=/bbs/game/farm.aspx?act=punish]马上去惩罚奴隶[/url]");
                                            //内线给去偷者
                                            new BCW.BLL.Guest().Add(1, meid, mename, "偷菜时一不小心,踩中" + mename2 + "农场第" + id + "块土地的陷阱,成为他的奴隶,下次偷菜要小心点了.我也去[url=/bbs/game/farm.aspx?act=xianjing]设置陷阱[/url].继续[url=/bbs/game/farm.aspx?act=toucai]去偷菜[/url]");
                                        }
                                    }
                                }
                                xx = xx + id + "#" + zuowu + tounum + "个,";//邵广林 20160525 修改一键偷还可以偷的情况
                                yy = zuowu_experience + yy;
                                istou = 1;
                            }
                            else
                            {
                                //加上已偷id
                                if (touID.Length < 1)
                                {
                                    //jj = meid.ToString();
                                    jj = "," + meid.ToString() + ",";
                                }
                                else
                                {
                                    jj = touID + meid + ",";
                                }
                                new BCW.farm.BLL.NC_tudi().update_tudi("touID='" + jj + "'", "usid=" + usid + " and tudi=" + id + "");
                            }
                        }
                    }
                    else
                    {
                        Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                }
                if (istou == 0)
                {
                    Utils.Success("操作土地", "这块地里果实已经所剩无几,不能再偷了,请手下留情", Utils.getUrl("farm.aspx?act=toucai"), "1");
                }
                else
                {
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在" + mename2 + "(" + usid + ")的农场摘取" + xx + "", 9);//消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")来农场摘取" + xx + "[url=/bbs/game/farm.aspx?act=do&amp;uid=" + meid + "]马上去Ta的农场[/url]", 9);//消息
                    //Utils.Success("操作土地", "成功在" + mename2 + "的农场摘取" + xx + "", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "3");
                    if (hhh == "")
                    {
                        Utils.Success("操作土地", "成功在" + mename2 + "的农场摘取" + xx + "", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
                    }
                    else
                        Utils.Success("操作土地", "成功在" + mename2 + "的农场摘取" + xx + "<br/>" + hhh + "", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "2");
                }
            }
            else
            {
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "&amp;ptype=" + ptype + ""), "1");
            }
        }
        else
        {
            Utils.Error("抱歉,只有vip会员才可以一键摘取哦,请开通vip再来,<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip&amp;farm=1") + "\">马上开通</a>", "");
        }
    }

    //铲地
    private void chandiPage()
    {
        Master.Title = "铲地";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;铲地");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));

        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "all", 2, @"^[1-9]\d*$", "选择土地出错"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        string info = Utils.GetRequest("info", "post", 1, "", "");
        if (new BCW.farm.BLL.NC_tudi().Exists_tudi(tudi, usid))
        {
            //查询土地属于哪种状态
            BCW.farm.Model.NC_tudi ui = new BCW.farm.BLL.NC_tudi().Get_td(usid, tudi);
            if (ui.zuowu == "" || ui.ischandi == 0)
            {
                Utils.Success("操作土地", "空土地不用铲地.", Utils.getUrl("farm.aspx"), "1");
            }
            else if (DateTime.Now > ui.updatetime.AddMinutes(ui.zuowu_time) && ui.ischandi == 1)//成熟
            {
                Utils.Success("操作土地", "作物成熟后不能铲地.", Utils.getUrl("farm.aspx"), "1");
            }
            else if (ui.ischandi == 2)
            {
                BCW.User.Users.IsFresh("Farmcd", 1);//防刷
                if (new BCW.farm.BLL.NC_tudi().Exists_chandi(tudi, usid))
                {
                    new BCW.farm.BLL.NC_tudi().update_tudi("z_shuinum='',isshifei=0,ischandi=0,zuowu='',zuowu_ji=0,iscao=0,iswater=0,isinsect=0,output='',zuowu_experience=0,harvest=0,zuowu_time=0,caoID='',chongID='',touID=''", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = chandi_jingyan * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = chandi_jingyan;
                        jysm = "";
                    }
                    new BCW.farm.BLL.NC_user().Update_Experience(usid, jy);
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场第" + tudi + "土地铲除枯萎作物.", 8);//消息
                    chandi_get(1);
                    Utils.Success("操作土地", "你成功铲除农场第" + tudi + "土地的枯萎作物." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + ""), "1");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
            else if (DateTime.Now < ui.updatetime.AddMinutes(ui.zuowu_time))//未成熟
            {
                if (info == "ok")
                {
                    int usid2 = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
                    int tudi2 = Utils.ParseInt(Utils.GetRequest("tudi", "all", 2, @"^[1-9]\d*$", "选择土地出错"));
                    int fenye2 = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
                    BCW.User.Users.IsFresh("Farmcd", 1);//防刷
                    new BCW.farm.BLL.NC_tudi().update_tudi("z_shuinum='',isshifei=0,ischandi=0,zuowu='',zuowu_ji=0,iscao=0,iswater=0,isinsect=0,output='',zuowu_experience=0,harvest=0,zuowu_time=0,caoID='',chongID='',touID=''", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在自己农场第" + tudi + "土地将铲除作物.", 8);//消息
                    Utils.Success("操作土地", "你成功铲除农场第" + tudi + "土地的作物.", Utils.getUrl("farm.aspx?&amp;page=" + fenye2 + ""), "1");
                }
                else
                {
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("作物还没有收获;真的要铲除吗？铲除后将变为空土地.");
                    builder.Append(Out.Tab("</div>", ""));

                    string strText = ",,,,";
                    string strName = "usid,tudi,fenye,act,info";
                    string strType = "hidden,hidden,hidden,hidden,hidden";
                    string strValu = "" + usid + "'" + tudi + "'" + fenye + "'chandi'ok";
                    string strEmpt = "false,false,false,false,false";
                    string strIdea = "/";
                    string strOthe = "确定,farm.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
            }
            else
                Utils.Error("请选择正确的土地操作.", "");
        }
        else
            Utils.Error("请选择正确的土地操作.", "");


        foot_link();//底部链接

        foot_link2();
    }

    //一键、铲地
    private void chandi_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        Master.Title = "一键耕地";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;温馨提示");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < chandi1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键耕地需要" + chandi1_grade + "级才能开通哦!<br/>开通后可以对枯萎的作物进行一键铲除(未成熟的作物除外).");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getchandi(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键耕地功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=5&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + chandi1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int aa = 0;
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and ischandi=2");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int id = 1;
                    BCW.User.Users.IsFresh("Farmcd", 1);//防刷
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());

                        new BCW.farm.BLL.NC_tudi().update_tudi("z_shuinum='',isshifei=0,ischandi=0,zuowu='',zuowu_ji=0,iscao=0,iswater=0,isinsect=0,output='',zuowu_experience=0,harvest=0,zuowu_time=0,caoID='',chongID='',touID=''", "usid = '" + meid + "' AND tudi='" + id + "'");
                        aa++;
                    }
                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = chandi_jingyan * aa * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = chandi_jingyan * aa;
                        jysm = "";
                    }

                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己铲除农场" + aa + "土地枯萎的作物.", 8);//消息
                    chandi_get(2);
                    Utils.Success("操作土地", "你成功将" + aa + "土地的枯萎作物铲除." + jysm + "获得" + jy + "点经验.", Utils.getUrl("farm.aspx"), "1");
                }
                else
                {
                    Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
                }
            }
        }
        foot_link();//底部链接

        foot_link2();
    }

    //施肥
    private void shifeiPage()
    {
        Master.Title = "施肥";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;我的化肥");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int tudi = Utils.ParseInt(Utils.GetRequest("tudi", "get", 2, @"^[1-9]\d*$", "选择土地出错"));

        if (new BCW.farm.BLL.NC_tudi().Exists_shifei(tudi, usid))
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("<b>我的化肥</b>");
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "usid=" + usid + " and type=2 and num>0 and huafei_id>0 and huafei_id<21";

            // 开始读取列表
            IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.Append("" + n.name + "x" + n.num + "");
                    if (n.iszengsong == 0)
                        builder.Append(" <a href=\"" + Utils.getUrl("farm.aspx?act=shifei_case&amp;id=" + n.huafei_id + "&amp;td=" + tudi + "&amp;fenye=" + fenye + "") + "\">施肥</a>");
                    else
                        builder.Append("(不可赠送) <a href=\"" + Utils.getUrl("farm.aspx?act=shifei_case&amp;id=" + n.huafei_id + "&amp;td=" + tudi + "&amp;fenye=" + fenye + "&amp;tt=1") + "\">施肥</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有化肥记录..<br/>"));
            }
        }
        else
        {
            Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">购买化肥>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }

    //执行施肥
    private void shifei_casePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择化肥出错"));
        int tudi = Utils.ParseInt(Utils.GetRequest("td", "get", 2, @"^[1-9]\d*$", "选择土地出错"));
        int fenye = Utils.ParseInt(Utils.GetRequest("fenye", "all", 1, @"^[1-9]\d*$", "1"));
        int tt = Utils.ParseInt(Utils.GetRequest("tt", "all", 1, @"^[0-9]\d*$", "0"));

        //查询是否超出使用化肥次数
        if (xExpir_huafei > 0)
        {
            if (new BCW.farm.BLL.NC_user().Get_hfnum(meid) >= xExpir_huafei)
            {
                Utils.Error("抱歉,你今天使用化肥的次数已达上限.上限为" + xExpir_huafei + "次.", "");
            }
        }

        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, tt))//判断是否存在化肥
        {
            BCW.User.Users.IsFresh("Farmsf", 3);//防刷
            //判断是否枯萎或土地为空
            BCW.farm.Model.NC_tudi a = new BCW.farm.BLL.NC_tudi().Get_td(meid, tudi);//根据usid和土地查询
            if (a.ischandi == 0)
                Utils.Error("抱歉,土地为空,请种植作物.", "");
            if (a.ischandi == 2)
                Utils.Error("抱歉,此作物已枯萎,请种植作物.", "");
            //判断是否已经成熟
            if (DateTime.Now > a.updatetime.AddMinutes(a.zuowu_time))
                Utils.Error("抱歉,此作物已成熟,不用施肥.", "");

            BCW.farm.Model.NC_daoju q = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);//根据id查询道具表
            if (id == 14 || id == 15 || id == 17 || id == 19)//可以多次施肥
            {
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥

                new BCW.farm.BLL.NC_tudi().update_tudi("isshifei=1", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改1
                new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + a.updatetime.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场第" + tudi + "块土地使用" + OutType2(id) + ".", 8);//消息
                BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                hu.type = 0;
                hu.daoju_id = id;
                hu.tudi = tudi;
                hu.updatetime = DateTime.Now;
                hu.usid = meid;
                new BCW.farm.BLL.NC_daoju_use().Add(hu);

                //双倍经验判断
                long jy = 0;//经验
                string jysm = string.Empty;//文字说明
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    jy = shifei_jingyan * 2;
                    jysm = "由于你使用了双倍经验卡,经验翻倍.";
                }
                else
                {
                    jy = shifei_jingyan;
                    jysm = "";
                }
                new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                //等级操作
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                  //加施肥次数
                new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                Utils.Success("操作化肥", "使用" + OutType2(id) + "成功," + a.zuowu + "加快生长" + q.time + "分钟." + jysm + "获得" + jy + "点经验.已使用化肥" + new BCW.farm.BLL.NC_user().Get_hfnum(meid) + "次.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + "&amp;crv=1"), "3");
            }
            else if (id == 1 || id == 2 || id == 4 || id == 6 || id == 8 || id == 10 || id == 12)//只可以一次
            {
                if (a.isshifei == 0)//0为未施肥
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥

                    new BCW.farm.BLL.NC_tudi().update_tudi("isshifei=1", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改1
                    new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + a.updatetime.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场第" + tudi + "块土地使用" + OutType2(id) + ".", 8);//消息
                    BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                    hu.type = 0;
                    hu.daoju_id = id;
                    hu.tudi = tudi;
                    hu.updatetime = DateTime.Now;
                    hu.usid = meid;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);

                    //双倍经验判断
                    long jy = 0;//经验
                    string jysm = string.Empty;//文字说明
                    if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                    {
                        jy = shifei_jingyan * 2;
                        jysm = "由于你使用了双倍经验卡,经验翻倍.";
                    }
                    else
                    {
                        jy = shifei_jingyan;
                        jysm = "";
                    }
                    new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                    //等级操作
                    dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                                      //加施肥次数
                    new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                    Utils.Success("操作化肥", "使用" + OutType2(id) + "成功," + a.zuowu + "加快生长" + q.time + "分钟." + jysm + "获得" + jy + "点经验.已使用化肥" + new BCW.farm.BLL.NC_user().Get_hfnum(meid) + "次.", Utils.getUrl("farm.aspx?&amp;page=" + fenye + "&amp;crv=1"), "3");
                }
                else if (a.isshifei == 1)//1为已施肥
                    Utils.Success("操作化肥", "该阶段已施肥,请换一种肥料或等待下一个阶段再施肥.", Utils.getUrl("farm.aspx?act=shifei&amp;usid=" + meid + "&amp;tudi=" + tudi + "&amp;page=" + fenye + ""), "2");
            }
            else
                Utils.Error("选择道具错误,请重新选择.", "");
        }
        else
            Utils.Error("你选择道具数量不足,请重新选择.", "");
    }

    //一键、施肥
    private void shifei_1Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        Master.Title = "一键施肥";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;一键施肥");
        builder.Append(Out.Tab("</div>", "<br />"));
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) < shifei1_grade)
        {
            long a = new BCW.farm.BLL.NC_user().GetGrade(meid);
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("你目前的等级为" + a + "级,一键施肥需要" + shifei1_grade + "级才能开通哦!");
            builder.Append(Out.Tab("</div>", ""));
        }
        else
        {
            if (new BCW.farm.BLL.NC_user().Getshifei(meid) == 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你还未开通一键施肥功能,现在就<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=6&amp;usid=" + meid + "") + "\">开通</a>!<br/>提示:开通需要支付" + shifei1_jinbi + "金币!<br/>");
                builder.Append("你自带<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
                string strWhere = " ";
                string[] pageValUrl = { "act", "ptype", "backurl" };
                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                strWhere = "usid=" + meid + " and type=2 and num>0 and huafei_id>0 and huafei_id<21";

                // 开始读取列表
                IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus(pageIndex, pageSize, strWhere, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        builder.Append("" + n.name + "x" + n.num + "");
                        if (n.iszengsong == 0)
                            builder.Append(" <a href=\"" + Utils.getUrl("farm.aspx?act=shifei_case2&amp;id=" + n.huafei_id + "") + "\">施肥</a>");
                        else
                            builder.Append("(不可赠送) <a href=\"" + Utils.getUrl("farm.aspx?act=shifei_case2&amp;id=" + n.huafei_id + "&amp;tt=1") + "\">施肥</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有化肥记录.."));
                }
            }
        }
        foot_link();//底部链接
        foot_link2();
    }

    //执行一键施肥
    private void shifei_case2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择化肥出错"));
        int tt = Utils.ParseInt(Utils.GetRequest("tt", "all", 1, @"^[0-9]\d*$", "0"));

        //查询是否超出使用化肥次数
        if (xExpir_huafei > 0)
        {
            if (new BCW.farm.BLL.NC_user().Get_hfnum(meid) >= xExpir_huafei)
            {
                Utils.Error("抱歉,你今天使用化肥的次数已达上限.上限为" + xExpir_huafei + "次.", "");
            }
        }

        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, tt))//判断是否存在化肥
        {
            BCW.farm.Model.NC_daoju q = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);//根据id查询道具表
            int wnum = new BCW.farm.BLL.NC_mydaoju().Get_daojunum2(meid, id, tt);//根据id查询我的道具,查询道具数量
            int aa = 0;
            int jj = 0;
            int sf = 0;
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + " and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");//可以施肥
            BCW.User.Users.IsFresh("Farmsf", 3);//防刷
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count >= wnum)//如果土地数量>道具数量
                {
                    jj = wnum;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int tudi = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);
                        int shifei = int.Parse(ds.Tables[0].Rows[i]["isshifei"].ToString());


                        //查询是否超出使用化肥次数
                        if (xExpir_huafei > 0)
                        {
                            if (new BCW.farm.BLL.NC_user().Get_hfnum(meid) >= xExpir_huafei)
                            {
                                Utils.Error("抱歉,你今天使用化肥的次数已达上限.上限为" + xExpir_huafei + "次.已为" + sf + "块土地施肥.", "");
                            }
                        }

                        if (shifei == 0)
                        {
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥
                            new BCW.farm.BLL.NC_tudi().update_tudi("isshifei=1", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改1
                            new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + time.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                            BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                            hu.type = 0;
                            hu.daoju_id = id;
                            hu.tudi = tudi;
                            hu.updatetime = DateTime.Now;
                            hu.usid = meid;
                            new BCW.farm.BLL.NC_daoju_use().Add(hu);
                            aa = aa + 1;
                            jj = jj - 1;

                            //加施肥次数
                            new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                            sf++;
                        }
                        else
                        {
                            if (id == 14 || id == 15 || id == 17 || id == 19)//可以多次施肥
                            {
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥
                                new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + time.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                                BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                                hu.type = 0;
                                hu.daoju_id = id;
                                hu.tudi = tudi;
                                hu.updatetime = DateTime.Now;
                                hu.usid = meid;
                                new BCW.farm.BLL.NC_daoju_use().Add(hu);
                                aa = aa + 1;
                                jj = jj - 1;

                                //加施肥次数
                                new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                                sf++;
                            }
                        }

                        if (jj == 0)
                        {
                            break;
                        }
                    }
                    if (aa == 0)
                        Utils.Success("操作化肥", "暂无给作物施肥,请换一种肥料或等待下一个阶段再施肥.", Utils.getUrl("farm.aspx"), "2");
                    else
                    {
                        //双倍经验判断
                        long jy = 0;//经验
                        string jysm = string.Empty;//文字说明
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = shifei_jingyan * 2 * aa;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.";
                        }
                        else
                        {
                            jy = shifei_jingyan * aa;
                            jysm = "";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                        //等级操作
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验

                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场共给" + aa + "块土地施用" + OutType2(id) + "." + jysm + "获得" + jy + "点经验.", 8);//消息
                        Utils.Success("操作化肥", "使用" + OutType2(id) + "成功,共给" + aa + "块土地施肥." + jysm + "获得" + jy + "点经验.已使用化肥" + new BCW.farm.BLL.NC_user().Get_hfnum(meid) + "次.", Utils.getUrl("farm.aspx?crv=1"), "3");
                    }
                }
                else
                {
                    jj = ds.Tables[0].Rows.Count;
                    for (int i = 0; i < wnum; i++)
                    {
                        int tudi = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());
                        DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);
                        int shifei = int.Parse(ds.Tables[0].Rows[i]["isshifei"].ToString());

                        //查询是否超出使用化肥次数
                        if (xExpir_huafei > 0)
                        {
                            if (new BCW.farm.BLL.NC_user().Get_hfnum(meid) >= xExpir_huafei)
                            {
                                Utils.Error("抱歉,你今天使用化肥的次数已达上限.上限为" + xExpir_huafei + "次.已为" + sf + "块土地施肥.", "");
                            }
                        }

                        if (shifei == 0)
                        {
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥
                            new BCW.farm.BLL.NC_tudi().update_tudi("isshifei=1", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改1
                            new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + time.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                            BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                            hu.type = 0;
                            hu.daoju_id = id;
                            hu.tudi = tudi;
                            hu.updatetime = DateTime.Now;
                            hu.usid = meid;
                            new BCW.farm.BLL.NC_daoju_use().Add(hu);
                            aa = aa + 1;

                            //加施肥次数
                            new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                            sf++;
                        }
                        else
                        {
                            if (id == 14 || id == 15 || id == 17 || id == 19)//可以多次施肥
                            {
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, id, tt);//减少化肥
                                new BCW.farm.BLL.NC_tudi().update_tudi("updatetime='" + time.AddMinutes(-q.time) + "'", "usid = '" + meid + "' AND tudi='" + tudi + "'");//改时间
                                BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
                                hu.type = 0;
                                hu.daoju_id = id;
                                hu.tudi = tudi;
                                hu.updatetime = DateTime.Now;
                                hu.usid = meid;
                                new BCW.farm.BLL.NC_daoju_use().Add(hu);
                                aa = aa + 1;

                                //加施肥次数
                                new BCW.farm.BLL.NC_user().Update_shifeinum(meid, 1);
                                sf++;
                            }
                        }
                        jj = jj - 1;
                        if (jj == 0)
                        {
                            break;
                        }
                    }
                    if (aa == 0)
                        Utils.Success("操作化肥", "暂无给作物施肥,请换一种肥料或等待下一个阶段再施肥.", Utils.getUrl("farm.aspx"), "2");
                    else
                    {
                        //双倍经验判断
                        long jy = 0;//经验
                        string jysm = string.Empty;//文字说明
                        if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                        {
                            jy = shifei_jingyan * 2 * aa;
                            jysm = "由于你使用了双倍经验卡,经验翻倍.";
                        }
                        else
                        {
                            jy = shifei_jingyan * aa;
                            jysm = "";
                        }
                        new BCW.farm.BLL.NC_user().Update_Experience(meid, jy);
                        //等级操作
                        dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验

                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场共给" + aa + "块土地施用" + OutType2(id) + "." + jysm + "获得" + jy + "点经验.", 8);//消息
                        Utils.Success("操作化肥", "使用" + OutType2(id) + "成功,共给" + aa + "块土地施肥." + jysm + "获得" + jy + "点经验.已使用化肥" + new BCW.farm.BLL.NC_user().Get_hfnum(meid) + "次.", Utils.getUrl("farm.aspx?crv=1"), "3");
                    }
                }
            }
            else
                Utils.Success("操作土地", "重复操作或没有可以操作的土地", Utils.getUrl("farm.aspx"), "1");
        }
        else
            Utils.Error("你选择道具数量不足,请重新选择.", "");
    }

    //开通
    private void kaitongPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        //判断登录id是否和传值id一致
        if (meid != usid)
        {
            Utils.Error("非法操作.", "");
        }
        int ptype = Utils.ParseInt(Utils.GetRequest("ptype", "get", 2, @"^[1-9]\d*$", "选择类型出错"));
        int p = int.Parse(Utils.GetRequest("p", "all", 1, @"^[1-2]$", "2"));//统一开通
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        long gold = new BCW.farm.BLL.NC_user().GetGold(meid);
        string time = DateTime.Now.ToString("HHmmss");//获取时间
        if (ptype == 1)//开通除草
        {
            if (gold < chucao1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (chucao1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_chucao_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -chucao1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -chucao1_jinbi, "在农场开通一键除草,花费" + chucao1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - chucao1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键除草.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键除草.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键除草.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键除草.", Utils.getUrl("farm.aspx?act=chucao_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
        if (ptype == 2)//开通浇水
        {
            if (gold < jiaoshui1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (jiaoshui1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_jiaoshui_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -jiaoshui1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -jiaoshui1_jinbi, "在农场开通一键浇水消费,花费" + jiaoshui1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - jiaoshui1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键浇水.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键浇水.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键浇水.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键浇水.", Utils.getUrl("farm.aspx?act=jiaoshui_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
        if (ptype == 3)//开通除虫
        {
            if (gold < chuchong1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (chuchong1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_chuchong_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -chuchong1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -chuchong1_jinbi, "在农场开通一键除虫消费,花费" + chuchong1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - chuchong1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键除虫.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键除虫.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键除虫.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键除虫.", Utils.getUrl("farm.aspx?act=chuchong_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
        if (ptype == 4)//开通收获
        {
            if (gold < shouhuo1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (shouhuo1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_shouhuo_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -shouhuo1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -shouhuo1_jinbi, "在农场开通一键收获消费,花费" + shouhuo1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - shouhuo1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键收获.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键收获.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键收获.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键收获.", Utils.getUrl("farm.aspx?act=shouhuo_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
        if (ptype == 5)//开通铲地
        {
            if (gold < chandi1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (chandi1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_chandi_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -chandi1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -chandi1_jinbi, "在农场开通一键耕地消费,花费" + chandi1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - chandi1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键耕地.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键铲地.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键耕地.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键耕地.", Utils.getUrl("farm.aspx?act=chandi_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
        if (ptype == 6)//开通施肥
        {
            if (gold < shifei1_jinbi)
            {
                Utils.Error("您的金币不足", "");
            }
            if (shifei1_grade > (new BCW.farm.BLL.NC_user().GetGrade(meid)))
            {
                Utils.Error("您的等级不够,请先升级.", "");
            }
            else
            {
                BCW.User.Users.IsFresh("Farmkt", 1);//防刷
                new BCW.farm.BLL.NC_user().Update_shifei_1(meid);
                //new BCW.farm.BLL.NC_user().Update_jinbi(meid, -shifei1_jinbi);
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -shifei1_jinbi, "在农场开通一键施肥消费,花费" + shifei1_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - shifei1_jinbi) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自己农场开通一键施肥.", 10);//消息
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]开通一键施肥.");
                if (p == 1)
                {
                    Utils.Success("操作土地", "恭喜你,成功开通了一键施肥.", Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2"), "1");
                }
                else
                    Utils.Success("操作土地", "恭喜你,成功开通了一键施肥.", Utils.getUrl("farm.aspx?act=shifei_1&amp;ptype=" + ptype + "&amp;GKeyStr=" + SetRoomID(time.ToString()) + ""), "1");
            }
        }
    }

    //商店
    private void ShangdianPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;商店购买");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "商店.种子";
            builder.Append("<h style=\"color:black\">种子" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1") + "\">种子</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "商店.道具";
            builder.Append("<h style=\"color:black\">道具" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">道具</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string strOrder = "";
        string[] pageValUrl = { "act", "ptype", "type", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;

        if (ptype == 1)
        {
            builder.Append(Out.Tab("<div>", ""));
            int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[0-4]$", "0"));
            if (type == 0)
            {
                builder.Append("<h style=\"color:red\">全部" + "</h>" + "|");
                strWhere = "type!=10  AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";//AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=0") + "\">全部</a>" + "|");
            }
            if (type == 1)
            {
                builder.Append("<h style=\"color:red\">蔬菜" + "</h>" + "|");
                strWhere = "type!=10 and zhonglei=1 AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=1") + "\">蔬菜</a>" + "|");
            }
            if (type == 2)
            {
                builder.Append("<h style=\"color:red\">水果" + "</h>" + "|");
                strWhere = "type!=10 and zhonglei=2 AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=2") + "\">水果</a>" + "|");
            }
            if (type == 3)
            {
                builder.Append("<h style=\"color:red\">鲜花" + "</h>" + "|");
                strWhere = "type!=10 and zhonglei=3 AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=3") + "\">鲜花</a>" + "|");
            }
            //邵广林 暂时屏蔽有机作物 20160529
            //if (type == 4)
            //{
            //    builder.Append("<h style=\"color:red\">有机作物" + "</h>" + "|");
            //    strWhere = "type!=10 and zhonglei=4";
            //}
            //else
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=4") + "\">有机作物</a>" + "|");
            //}
            if (type == 4)
            {
                builder.Append("<h style=\"color:red\">可赠送" + "</h>" + "");
                strWhere = "type!=10 and zhonglei=5 AND grade<=" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1&amp;type=4") + "\">可赠送</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));
            strOrder = "grade desc";//,id asc

            //种子
            IList<BCW.farm.Model.NC_shop> listNCpay = new BCW.farm.BLL.NC_shop().GetNC_shops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listNCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_shop n in listNCpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append((((pageIndex - 1) * pageSize + k)) + "." + n.name + "(播种需" + n.grade + "级)." + OutType(n.type) + "<br/>");
                    //拆分图片
                    try
                    {
                        string[] aa = n.picture.Split(',');
                        builder.Append("<img  src=\"" + aa[4] + "\" alt=\"load\"/><br/>");
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append("单价:" + n.price_in + "金币");
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=" + ptype + "&amp;id=" + n.num_id + "") + "\">[购买]</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "<h style=\"color:#A9A9A9\">暂无该种子或请先升级.</h>"));
            }
        }
        else
        {
            strWhere = "type!=10";
            //化肥
            IList<BCW.farm.Model.NC_daoju> listNCpay = new BCW.farm.BLL.NC_daoju().GetNC_daojus(pageIndex, pageSize, strWhere, out recordCount);
            if (listNCpay.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_daoju m in listNCpay)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append((((pageIndex - 1) * pageSize + k)) + "." + m.name + "<br/>");
                    try
                    {
                        builder.Append("<img  src=\"" + m.picture + "\" alt=\"load\"/><br/>");// height=\"45px\"
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append("单价:" + m.price + "" + ub.Get("SiteBz") + "");
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=" + ptype + "&amp;id=" + m.ID + "") + "\">[购买]</a>");

                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "<h style=\"color:#A9A9A9\">暂无道具..</h>"));
            }
        }
        //builder.Append(Out.Tab("<div>", Out.Hr()));
        //if (ptype == 1)
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2") + "\">&lt;&lt;返回我的仓库</a>");
        //if (ptype == 2)
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">&lt;&lt;返回我的仓库</a>");
        //builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接
        foot_link2();
    }

    //购买--道具可赠送
    private void PayPage(int nb)
    {
        Master.Title = "商店购买";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">商店购买</a>&gt;购买介绍");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        string info = Utils.GetRequest("info", "post", 1, "", "");
        long price_a = 0;
        int Num1;
        int id;
        int baoxiang = 0;//如果是1，可以返回宝箱界面
        if (info == "ok")
        {
            Num1 = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "请选择正确的种子个数"));//购买数量
            id = Utils.ParseInt(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "选择种子出错"));//种子or化肥ID
            long Price = Utils.ParseInt64(Utils.GetRequest("price", "post", 2, @"^[1-9]\d*$", "金币错误"));//种子单价
            int ptype_a = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));//种子or化肥


            //支付安全提示
            string[] p_pageArr = { "price", "num", "id", "ptype", "act", "info" };
            BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);
            long prices = Convert.ToInt64(Price * Num1);//购买所需的金币/酷币
            //是否刷屏
            BCW.User.Users.IsFresh("Farmgm", 3);
            long gold = 0;
            if (ptype_a == 1)
            {
                gold = new BCW.farm.BLL.NC_user().GetGold(meid);//个人的金币
                if (gold < prices)
                    Utils.Error("您的金币不足.", "");
            }
            else
            {
                gold = new BCW.BLL.User().GetGold(meid);//个人的酷币
                if (gold < prices)
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足.", "");
            }
            string hg = string.Empty;
            if (ptype_a == 1)//种子
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, meid))//存在该种子
                {
                    BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, id);//得到一个实体
                    hg = bbb.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                }
                else//不存在
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists(id, meid))//如果道具表有了该种子
                    {
                        BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, id);//查询种子个数
                        hg = bbb.name;
                        new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                    }
                    else
                    {
                        BCW.farm.Model.NC_shop y = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
                        BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                        hg = y.name;//名称
                        u.name = y.name;
                        u.name_id = id;
                        u.num = Num1;
                        u.type = 1;
                        u.usid = meid;
                        u.zhonglei = y.type;
                        u.huafei_id = 0;
                        try
                        {
                            string[] bv = y.picture.Split(',');
                            u.picture = bv[4];//我的道具表加图片
                        }
                        catch
                        {
                            u.picture = "";
                        }
                        new BCW.farm.BLL.NC_mydaoju().Add(u);
                    }
                }
                long hh = 0;
                //识别会员vip
                DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                DateTime viptime = DateTime.Now;
                try
                {
                    viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                }
                catch
                {
                    viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                }
                if (DateTime.Now < viptime)//会员
                {
                    hh = (long)(float.Parse(prices.ToString()) * vipprice);
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -hh, "在商店购买" + Num1 + "个" + hg + ",花费(VIP价格)" + hh + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - hh) + "金币.", 4);
                }
                else
                {
                    hh = prices;
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -hh, "在商店购买" + Num1 + "个" + hg + ",花费了" + hh + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - hh) + "金币.", 4);
                }
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在商店购买" + Num1 + "个" + hg + ".", 4);//消息
                Utils.Success("购买种子", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "金币.现在<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1") + "\">去播种吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "5");
            }
            else//道具
            {
                //int yy;
                int xx;
                int old_num = 0;//原有数量
                int new_num = 0;//现有数量
                //根据id查询usid对应的道具数量
                BCW.farm.Model.NC_mydaoju kk = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);
                try
                {
                    old_num = kk.num;
                }
                catch
                {
                    old_num = 0;
                }

                BCW.farm.Model.NC_daoju q = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);//根据id查询道具表
                if (q.type == 0)//如果是礼包
                {
                    //礼包
                    //if (id == 3 || id == 5 || id == 7 || id == 16 || id == 18 || id == 20)//|| id == 9 || id == 11 || id == 13
                    //{
                    //    yy = id - 1;
                    //    xx = Num1 * 7;

                    //    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(yy, meid))
                    //    {
                    //        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, yy);//查询化肥数量
                    //        hg = aaa.name;//名称
                    //        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, yy);
                    //    }
                    //    else
                    //    {
                    //        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(yy, meid))
                    //        {
                    //            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, yy);//查询化肥数量
                    //            hg = aaa.name;//名称
                    //            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, yy);
                    //        }
                    //        else
                    //        {
                    //            BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(yy);
                    //            BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    //            hg = t.name;//名称
                    //            w.name = t.name;
                    //            w.name_id = 0;
                    //            w.num = xx;
                    //            w.type = 2;
                    //            w.usid = meid;
                    //            w.zhonglei = 0;
                    //            w.huafei_id = yy;
                    //            w.picture = t.picture;
                    //            new BCW.farm.BLL.NC_mydaoju().Add(w);
                    //        }
                    //    }
                    //}
                }
                else//如果不是礼包
                {
                    if (Num1 >= 5)//如果超过5个,送2个
                    {
                        if (id == 2 || id == 4 || id == 6 || id == 15 || id == 17 || id == 19)//可以加2    //|| id == 8|| id == 10 || id == 12 
                        {
                            xx = Num1 / 5 * 2 + Num1;
                            new_num = xx + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = xx;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                        else if (id == 1 || id == 14)//不可以加2
                        {
                            new_num = Num1 + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = Num1;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                        else
                        {
                            new_num = Num1 + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = Num1;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                    }
                    else//如果不够5个
                    {
                        new_num = Num1 + old_num;
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                        {
                            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                            hg = aaa.name;//名称
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                        }
                        else
                        {
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                hg = t.name;//名称
                                w.name = t.name;
                                w.name_id = 0;
                                w.num = Num1;
                                w.type = 2;
                                w.usid = meid;
                                w.zhonglei = 0;
                                w.huafei_id = id;
                                w.picture = t.picture;
                                w.iszengsong = 0;
                                new BCW.farm.BLL.NC_mydaoju().Add(w);
                            }
                        }
                    }
                }
                long hh = 0;
                //识别会员vip
                DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                DateTime viptime = DateTime.Now;
                try
                {
                    viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                }
                catch
                {
                    viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                }
                if (DateTime.Now < viptime)//会员
                {
                    hh = (long)(float.Parse(prices.ToString()) * vipprice);
                    new BCW.BLL.User().UpdateiGold(meid, mename, -hh, "您在农场商店购买道具" + Num1 + "个" + hg + "--原" + old_num + "现" + new_num + "");
                }
                else
                {
                    hh = prices;
                    new BCW.BLL.User().UpdateiGold(meid, mename, -hh, "您在农场商店购买道具" + Num1 + "个" + hg + "--原" + old_num + "现" + new_num + "");
                }
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在酷乐农场记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName(GameName))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (hh > new BCW.BLL.tb_WinnersGame().GetPrice(GameName))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, GameName + "农场", 3);
                            if (hit == 1)
                            {
                                //内线开关 1开
                                if (WinnersGuessOpen == "1")
                                {
                                    //发内线到该ID
                                    new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                                }
                            }
                        }
                    }
                }
                catch { }
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx?act=shangdian]" + GameName + "[/url]的商店购买" + hg + ".");
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在商店购买" + Num1 + "个" + hg + ",花费了" + hh + "" + ub.Get("SiteBz") + ".", 4);//消息
                if (nb == 1)//如果是1，返回宝箱界面
                {
                    Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".", Utils.getUrl("farm.aspx?act=baoxiang"), "2");
                }
                else
                {
                    if (id == 1 || id == 2 || id == 4 || id == 6 || id == 14 || id == 15 || id == 17 || id == 19)
                        Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".现在<a href=\"" + Utils.getUrl("farm.aspx?act=shifei_1") + "\">去施肥吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "5");
                    else if (id == 22)
                    {
                        Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".现在<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">去开宝箱吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=22") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "5");
                    }
                    else
                        Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "2");
                }
            }
        }
        else
        {
            if (ptype == 1)
            {
                id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "选择种子出错"));
                if (new BCW.farm.BLL.NC_shop().Exists_zzid(id))
                {
                    BCW.farm.Model.NC_shop a = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
                    price_a = a.price_in;
                    string[] aa = a.output.Split(',');
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<b>种子详细介绍:</b><br />");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    //拆分图片
                    try
                    {
                        string[] tt = a.picture.Split(',');
                        builder.Append("<img  src=\"" + tt[4] + "\" alt=\"load\"/><br/>");// height=\"45px\"
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append("种子名称:" + a.name + "<br />");
                    builder.Append("种子原价:" + a.price_in + "金币<br />");
                    builder.Append("种子VIP价:" + a.price_in * vipprice + "金币<br />");
                    builder.Append("果实售价:" + a.price_out + "金币<br />");
                    builder.Append("预期产量:" + aa[1] + "个/x" + a.jidu + "季<br />");
                    TimeSpan span = new TimeSpan(0, a.jidu_time, 0);
                    if (a.jidu_time < 1440)
                    {
                        builder.Append("周期:" + span.Hours + "小时" + span.Minutes + "分钟/季<br />");
                    }
                    else
                    {
                        builder.Append("周期:" + span.Days + "天" + span.Hours + "小时" + span.Minutes + "分钟/季<br />");
                    }
                    builder.Append("农场经验:" + a.experience + "点/季<br />");
                    if (a.tili > 0)
                    {
                        builder.Append("魅力+" + a.meili + ",体力+" + a.tili + "<br/>");
                    }
                    builder.Append("种植所需等级:" + a.grade + "级.当前:" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "级<br />");
                    builder.Append("您自带:<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币<br />");
                    builder.Append("快速购买:<a href=\"" + Utils.getUrl("farm.aspx?act=fast&amp;ptype=1&amp;z=" + id + "&amp;n=1") + "\">1</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast&amp;ptype=1&amp;z=" + id + "&amp;n=10") + "\">10</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast&amp;ptype=1&amp;z=" + id + "&amp;n=20") + "\">20</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast&amp;ptype=1&amp;z=" + id + "&amp;n=30") + "\">30</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    Utils.Success("购买种子", "请选择正确的种子", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "1");
                }
            }
            else
            {
                id = Utils.ParseInt(Utils.GetRequest("id", "get", 1, @"^[1-9]\d*$", "选择化肥出错"));
                baoxiang = Utils.ParseInt(Utils.GetRequest("bx", "all", 1, @"^[0-1]\d*$", "0"));
                if (new BCW.farm.BLL.NC_daoju().Exists(id))
                {
                    BCW.farm.Model.NC_daoju b = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                    price_a = b.price;
                    builder.Append(Out.Tab("<div>", ""));
                    try
                    {
                        builder.Append("<img  src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(b.ID) + "\" alt=\"load\"/><br/>");// height=\"45px\"
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append("名称:" + b.name + "<br />");
                    builder.Append("原价:" + b.price + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("VIP价:" + b.price * vipprice + "" + ub.Get("SiteBz") + "<br />");
                    builder.Append("功能:" + b.note + "<br />");
                    builder.Append("您自带:<h style=\"color:red\">" + new BCW.BLL.User().GetGold(meid) + "</h>" + ub.Get("SiteBz") + "<br />");
                    builder.Append("快速购买:<a href=\"" + Utils.getUrl("farm.aspx?act=fast2&amp;ptype=2&amp;z=" + id + "&amp;n=1") + "\">1</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast2&amp;ptype=2&amp;z=" + id + "&amp;n=10") + "\">10</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast2&amp;ptype=2&amp;z=" + id + "&amp;n=20") + "\">20</a> | <a href=\"" + Utils.getUrl("farm.aspx?act=fast2&amp;ptype=2&amp;z=" + id + "&amp;n=30") + "\">30</a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    Utils.Success("购买道具", "请选择正确的道具", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "1");
                }
            }
            string strText = "购买数:/,,,,,,";
            string strName = "num,ID,ptype,price,vb,act,info";
            string strType = "num,hidden,hidden,hidden,hidden,hidden,hidden";
            string strValu = "'" + id + "'" + ptype + "'" + price_a + "'" + nb + "'buycase'ok";
            string strEmpt = "true,false,false,false,false,false,false";
            string strIdea = "/";
            string strOthe = "确定购买,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("温馨提示:<br/>");
            //根据id+usid查询数量
            if (ptype == 1)
            {
                builder.Append("我的该种子数量为：" + new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, id) + "<br/>");
            }
            else
            {
                builder.Append("我的该道具数量为：" + (new BCW.farm.BLL.NC_mydaoju().Get_daojunum2(meid, id, 0) + new BCW.farm.BLL.NC_mydaoju().Get_daojunum2(meid, id, 1)) + "<br/>");//可赠送和不可赠送相加
            }
            builder.Append("种子购买需花费金币,道具购买需花费" + ub.Get("SiteBz") + ".");
            builder.Append(Out.Tab("</div>", ""));
        }

        if (ptype == 2)
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">我的道具</a>.");
            if (baoxiang == 1)//1为宝箱
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">我的宝箱</a>");
            if (ptype == 2 && id == 22 && baoxiang == 0)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">我的宝箱</a>");
            if (ptype == 2 && id == 21)
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">我的陷阱</a>");
            builder.Append(Out.Tab("</div>", ""));
        }

        foot_link();//底部链接
        foot_link2();
    }

    //快速购买_种子---购买道具需要酷币、购买种子需要金币
    private void fastPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "1"));//类型：种子
        int id = Utils.ParseInt(Utils.GetRequest("z", "all", 1, @"^[1-9]\d*$", "选择种子出错"));//种子id
        int Num1 = Utils.ParseInt(Utils.GetRequest("n", "all", 1, @"^[1-9]\d*$", "请选择正确的种子个数"));//购买数量
        if (ptype == 1)
        {
            if (new BCW.farm.BLL.NC_shop().Exists(id))
            {
                //查询id对应的金币
                BCW.farm.Model.NC_shop p = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);

                long gold = new BCW.farm.BLL.NC_user().GetGold(meid);//个人的金币
                long prices = Convert.ToInt64(p.price_in * Num1);//购买所需的金币

                //支付安全提示
                string[] p_pageArr = { "ptype", "n", "z", "act" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                //是否刷屏
                BCW.User.Users.IsFresh("Farmgm", 3);
                if (gold < prices)
                {
                    Utils.Error("您的金币不足", "");
                }

                string hg = string.Empty;
                if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, meid))
                {
                    BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, id);
                    hg = bbb.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists(id, meid))
                    {
                        BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, id);
                        hg = bbb.name;
                        new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                    }
                    else
                    {
                        BCW.farm.Model.NC_shop y = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
                        BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                        hg = y.name;//名称
                        u.name = y.name;
                        u.name_id = id;
                        u.num = Num1;
                        u.type = 1;
                        u.usid = meid;
                        u.zhonglei = y.type;
                        u.huafei_id = 0;
                        try
                        {
                            string[] bv = y.picture.Split(',');
                            u.picture = bv[4];
                        }
                        catch
                        {
                            u.picture = "";
                        }
                        new BCW.farm.BLL.NC_mydaoju().Add(u);
                    }
                }
                //20150524 修改种子vip价格 邵广林
                long hh = 0;
                //识别会员vip
                DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                DateTime viptime = DateTime.Now;
                try
                {
                    viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                }
                catch
                {
                    viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                }
                if (DateTime.Now < viptime)//会员
                {
                    hh = (long)(float.Parse(prices.ToString()) * vipprice);
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -hh, "在商店购买" + Num1 + "个" + hg + ",花费(VIP价格)" + hh + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - hh) + "金币.", 4);
                }
                else
                {
                    hh = prices;
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -hh, "在商店购买" + Num1 + "个" + hg + ",花费了" + hh + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - hh) + "金币.", 4);
                }
                //new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -prices, "在商店购买" + Num1 + "个" + hg + ",花费了" + prices + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - prices) + "金币.", 4);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在商店购买" + Num1 + "个" + hg + ".", 4);//消息
                //Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "金币.", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "2");//act=buycase&amp;ptype=1&amp;id=" + id + "   邵广林20160519修改跳转回商店首页
                Utils.Success("购买种子", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "金币.现在<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1") + "\">去播种吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "5");
            }
            else
            {
                Utils.Success("购买种子", "购买种子出错,请选择正确的种子.", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "1");
            }
        }
        else
        {
            Utils.Success("购买种子", "购买种子出错,请选择正确的种子.", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=1"), "1");
        }
    }

    //快速购买_道具
    private void fast2Page()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]\d*$", "2"));//类型：道具
        int id = Utils.ParseInt(Utils.GetRequest("z", "all", 1, @"^[1-9]\d*$", "选择道具出错"));//道具id
        int Num1 = Utils.ParseInt(Utils.GetRequest("n", "all", 1, @"^[1-9]\d*$", "请选择正确的道具个数"));//购买数量
        if (ptype == 2)
        {
            if (new BCW.farm.BLL.NC_daoju().Exists(id))
            {
                //查询id对应的酷币
                BCW.farm.Model.NC_daoju p = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);

                long gold = new BCW.BLL.User().GetGold(meid);
                long prices = Convert.ToInt64(p.price * Num1);//购买所需的酷币

                //支付安全提示
                string[] p_pageArr = { "ptype", "n", "z", "act" };
                BCW.User.PaySafe.PaySafePage(meid, Utils.getPageUrl(), p_pageArr);

                //是否刷屏
                BCW.User.Users.IsFresh("Farmgm", 3);
                if (gold < prices)
                {
                    Utils.Error("您的" + ub.Get("SiteBz") + "不足", "");
                }

                string hg = string.Empty;
                int xx;
                //20160507 邵广林 增加消费记录
                int old_num = 0;//原有数量
                int new_num = 0;//现有数量
                //根据id查询usid对应的道具数量
                BCW.farm.Model.NC_mydaoju kk = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);
                try
                {
                    old_num = kk.num;
                }
                catch
                {
                    old_num = 0;
                }

                BCW.farm.Model.NC_daoju q = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);//根据id查询道具表
                if (q.type == 0)//如果是礼包
                {
                    //礼包
                    //if (id == 3 || id == 5 || id == 7 || id == 16 || id == 18 || id == 20)//|| id == 9 || id == 11 || id == 13
                    //{
                    //    yy = id - 1;
                    //    xx = Num1 * 7;

                    //    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(yy, meid))
                    //    {
                    //        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, yy);//查询化肥数量
                    //        hg = aaa.name;//名称
                    //        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, yy);
                    //    }
                    //    else
                    //    {
                    //        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(yy, meid))
                    //        {
                    //            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, yy);//查询化肥数量
                    //            hg = aaa.name;//名称
                    //            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, yy);
                    //        }
                    //        else
                    //        {
                    //            BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(yy);
                    //            BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    //            hg = t.name;//名称
                    //            w.name = t.name;
                    //            w.name_id = 0;
                    //            w.num = xx;
                    //            w.type = 2;
                    //            w.usid = meid;
                    //            w.zhonglei = 0;
                    //            w.huafei_id = yy;
                    //            w.picture = t.picture;
                    //            new BCW.farm.BLL.NC_mydaoju().Add(w);
                    //        }
                    //    }
                    //}
                }
                else//如果不是礼包
                {
                    if (Num1 >= 5)//如果超过5个,送2个
                    {
                        if (id == 2 || id == 4 || id == 6 || id == 15 || id == 17 || id == 19)//可以加2    //|| id == 8|| id == 10 || id == 12 
                        {
                            xx = Num1 / 5 * 2 + Num1;
                            new_num = xx + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, xx, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = xx;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                        else if (id == 1 || id == 14)//不可以加2
                        {
                            new_num = Num1 + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = Num1;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                        else
                        {
                            new_num = Num1 + old_num;
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                                {
                                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                    hg = aaa.name;//名称
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                                }
                                else
                                {
                                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                    hg = t.name;//名称
                                    w.name = t.name;
                                    w.name_id = 0;
                                    w.num = Num1;
                                    w.type = 2;
                                    w.usid = meid;
                                    w.zhonglei = 0;
                                    w.huafei_id = id;
                                    w.picture = t.picture;
                                    w.iszengsong = 0;
                                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                                }
                            }
                        }
                    }
                    else//如果不够5个
                    {
                        new_num = Num1 + old_num;
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid, 0))
                        {
                            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                            hg = aaa.name;//名称
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                        }
                        else
                        {
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid, 0))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id, 0);//查询化肥数量
                                hg = aaa.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id, 0);
                            }
                            else
                            {
                                BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                                BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                hg = t.name;//名称
                                w.name = t.name;
                                w.name_id = 0;
                                w.num = Num1;
                                w.type = 2;
                                w.usid = meid;
                                w.zhonglei = 0;
                                w.huafei_id = id;
                                w.picture = t.picture;
                                w.iszengsong = 0;
                                new BCW.farm.BLL.NC_mydaoju().Add(w);
                            }
                        }
                    }
                }
                //if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(id, meid))
                //{
                //    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id);//查询化肥数量
                //    hg = aaa.name;//名称
                //    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id);
                //}
                //else
                //{
                //    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(id, meid))
                //    {
                //        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, id);//查询化肥数量
                //        hg = aaa.name;//名称
                //        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, Num1, id);
                //    }
                //    else
                //    {
                //        BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(id);
                //        BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                //        hg = t.name;//名称
                //        w.name = t.name;
                //        w.name_id = 0;
                //        w.num = Num1;
                //        w.type = 2;
                //        w.usid = meid;
                //        w.zhonglei = 0;
                //        w.huafei_id = id;
                //        w.picture = t.picture;
                //        new BCW.farm.BLL.NC_mydaoju().Add(w);
                //    }
                //}
                long hh = 0;
                //识别会员vip
                DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                DateTime viptime = DateTime.Now;
                try
                {
                    viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                }
                catch
                {
                    viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                }
                if (DateTime.Now < viptime)//会员
                {
                    hh = (long)(float.Parse(prices.ToString()) * vipprice);
                    new BCW.BLL.User().UpdateiGold(meid, mename, -hh, "您在农场商店购买道具" + Num1 + "个" + hg + "--原" + old_num + "现" + new_num + "");
                }
                else
                {
                    hh = prices;
                    new BCW.BLL.User().UpdateiGold(meid, mename, -hh, "您在农场商店购买道具" + Num1 + "个" + hg + "--原" + old_num + "现" + new_num + "");
                }

                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx?act=shangdian]" + GameName + "[/url]的商店购买" + hg + ".");
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在商店购买" + Num1 + "个" + hg + ",花费了" + hh + "" + ub.Get("SiteBz") + ".", 4);//消息
                //活跃抽奖入口_20160621姚志光
                try
                {
                    //表中存在酷乐农场记录
                    if (new BCW.BLL.tb_WinnersGame().ExistsGameName(GameName))
                    {
                        //投注是否大于设定的限额，是则有抽奖机会
                        if (hh > new BCW.BLL.tb_WinnersGame().GetPrice(GameName))
                        {
                            string TextForUbb = (ub.GetSub("TextForUbb", "/Controls/winners.xml"));//设置内线提示的文字
                            string WinnersGuessOpen = (ub.GetSub("WinnersGuessOpen", "/Controls/winners.xml"));//1发内线2不发内线 
                            int hit = new BCW.winners.winners().CheckActionForAll(1, 1, meid, mename, "农场", 3);
                            if (hit == 1)
                            {
                                //内线开关 1开
                                if (WinnersGuessOpen == "1")
                                {
                                    //发内线到该ID
                                    new BCW.BLL.Guest().Add(0, meid, mename, TextForUbb);
                                }
                            }
                        }
                    }
                }
                catch { }
                if (id == 1 || id == 2 || id == 4 || id == 6 || id == 14 || id == 15 || id == 17 || id == 19)
                    Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".现在<a href=\"" + Utils.getUrl("farm.aspx?act=shifei_1") + "\">去施肥吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "5");
                else if (id == 22)
                {
                    Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".现在<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">去开宝箱吧.</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=22") + "\">继续购买>></a><br/><a href=\"" + Utils.getUrl("farm.aspx") + "\">返回首页>></a>", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "5");
                }
                else
                    Utils.Success("购买道具", "成功购买" + Num1 + "个<b>" + hg + "</b>,花费了" + hh + "" + ub.Get("SiteBz") + ".", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "2");//act=buycase&amp;ptype=2&amp;id=" + id + "  邵广林20160519修改跳转回商店首页
            }
            else
            {
                Utils.Success("购买道具", "购买道具出错,请选择正确的道具.", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "1");
            }
        }
        else
        {
            Utils.Success("购买道具", "购买道具出错,请选择正确的道具.", Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2"), "1");
        }
    }

    //排行榜
    private void PaihangbanPage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;排行榜");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-6]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "排行榜.按等级";
            builder.Append("<h style=\"color:red\">按等级" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=1") + "\">按等级</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "排行榜.按金币";
            builder.Append("<h style=\"color:red\">按金币" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=2") + "\">按金币</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "排行榜.按签到";
            builder.Append("<h style=\"color:red\">按签到" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=3") + "\">按签到</a>" + "|");
        }
        if (ptype == 4)
        {
            Master.Title = "排行榜.按偷取";
            builder.Append("<h style=\"color:red\">按偷取" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=4") + "\">按偷取</a>" + "|");
        }
        if (ptype == 5)
        {
            Master.Title = "排行榜.按合成";
            builder.Append("<h style=\"color:red\">按合成" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=5") + "\">按合成</a>" + "|");
        }
        if (ptype == 6)
        {
            Master.Title = "排行榜.按人气";
            builder.Append("<h style=\"color:red\">按人气" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=6") + "\">按人气</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;

        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strOrder = "";

        if (ptype == 1 || ptype == 2)
        {
            if (pageIndex > 20)
            {
                pageIndex = 20;
            }
            #region
            if (ptype == 1)
            {
                strOrder = "Grade Desc,Experience DESC";
            }
            if (ptype == 2)
            {
                strWhere = "Goid>0";
                strOrder = "(Goid) Desc";
            }
            IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //获取id对应的用户名
                    string mename = new BCW.BLL.User().GetUsName(n.usid);
                    if (ptype == 1)
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + n.usid + "") + "\">" + mename + "</a>:<h style=\"color:red\">" + (n.Grade) + "</h>级");
                    }
                    if (ptype == 2)
                    {
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + n.usid + "") + "\">" + mename + "</a>:<h style=\"color:red\">" + (n.Goid) + "</h>金币");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                if (recordCount >= 200)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 200, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        if (ptype == 3)
        {
            #region
            pageSize = 20;
            strWhere = "SignTotal>0";
            strOrder = "SignTotal Desc,SignTime ASC";
            // 开始读取列表
            IList<BCW.farm.Model.NC_user> listUser = new BCW.farm.BLL.NC_user().GetUsers(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listUser.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_user n in listUser)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string OutText = string.Empty;
                    OutText = ":<h style=\"color:red\">" + n.SignTotal + "</h>次";
                    string mename = new BCW.BLL.User().GetUsName(n.usid);//用户姓名
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.usid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "</a>" + OutText + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        if (ptype == 4)
        {
            #region
            DataSet dd = new BCW.farm.BLL.NC_GetCrop().GetList("UsID,sum(tou_nums) AS aa", "tou_nums>0 GROUP BY UsID ORDER BY aa DESC");
            int name_num = 0;
            if (dd != null && dd.Tables[0].Rows.Count > 0)
            {
                recordCount = dd.Tables[0].Rows.Count;
                int k = 1;
                int k1 = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                //查询个人排名
                for (int i1 = 0; i1 < dd.Tables[0].Rows.Count; i1++)
                {
                    int UsID = Convert.ToInt32(dd.Tables[0].Rows[koo + i1]["usid"]);
                    int id = Convert.ToInt32(dd.Tables[0].Rows[koo + i1]["aa"]);
                    if (UsID == meid)
                    {
                        name_num = k1;
                    }
                    k1++;
                }
                //正常排序--显示10个
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(dd.Tables[0].Rows[koo + i]["usid"]);
                    int id = Convert.ToInt32(dd.Tables[0].Rows[koo + i]["aa"]);

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>:<h style=\"color:red\">" + id + "</h>个");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, 10, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关排行记录.."));
            }
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("我的排名：第" + name_num + "名.");
            builder.Append("去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>升排名吧.");
            builder.Append(Out.Tab("</div>", ""));
            #endregion
        }
        if (ptype == 5)
        {
            #region
            DataSet hecheng = new BCW.farm.BLL.NC_hecheng().GetList("UsID,SUM(all_num) AS bb", "all_num>0 GROUP BY UsID ORDER BY bb DESC,UsID ASC");
            if (hecheng != null && hecheng.Tables[0].Rows.Count > 0)
            {
                recordCount = hecheng.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int usid = int.Parse(hecheng.Tables[0].Rows[koo + i]["usid"].ToString());
                    int bb = int.Parse(hecheng.Tables[0].Rows[koo + i]["bb"].ToString());

                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>:<h style=\"color:red\">" + bb + "</h>次");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        if (ptype == 6)
        {
            #region
            DataSet shoudao = new BCW.BLL.Shopuser().GetList("UsID,SUM(Total) AS bb", "PIC=1  GROUP BY UsID ORDER BY bb DESC,UsID ASC");
            if (shoudao != null && shoudao.Tables[0].Rows.Count > 0)
            {
                recordCount = shoudao.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int usid = int.Parse(shoudao.Tables[0].Rows[koo + i]["usid"].ToString());
                    int bb = int.Parse(shoudao.Tables[0].Rows[koo + i]["bb"].ToString());


                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + usid + "") + "\">" + new BCW.BLL.User().GetUsName(usid) + "</a>:收到<h style=\"color:red\">" + bb + "</h>朵花");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
            #endregion
        }
        foot_link();//底部链接

        foot_link2();
    }

    //仓库
    private void CangkuPage(int ptypes)
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;我的仓库");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptypes == 3)//摆摊回传的3
        {
            ptype = 3;
        }
        if (ptype == 1)
        {
            Master.Title = "我的仓库.果实";
            builder.Append("<h style=\"color:black\">果实" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1") + "\">果实</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "我的仓库.种子";
            builder.Append("<h style=\"color:black\">种子" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2") + "\">种子</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "我的仓库.道具";
            builder.Append("<h style=\"color:black\">道具" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">道具</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "ptype2", "ptype3", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strOrder = "";
        if (ptype == 1)
        {
            #region 果实
            builder.Append(Out.Tab("<div>", ""));
            int ptype3 = int.Parse(Utils.GetRequest("ptype3", "all", 1, @"^[1-3]$", "1"));
            if (ptype3 == 1)
            {
                builder.Append("<h style=\"color:red\">不可赠送" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=1") + "\">不可赠送</a>" + "|");
            }
            if (ptype3 == 2)
            {
                builder.Append("<h style=\"color:red\">待合成" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=2") + "\">待合成</a>" + "|");
            }
            if (ptype3 == 3)
            {
                builder.Append("<h style=\"color:red\">可赠送" + "</h>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=3") + "\">可赠送</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));



            if (ptype3 == 1 || ptype3 == 2)
            {
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("全部果实总价值:<h style=\"color:red\">" + new BCW.farm.BLL.NC_GetCrop().Get_allprice(meid) + "</h>金币(不含锁定果实)<br/><a href=\"" + Utils.getUrl("farm.aspx?act=sellall&amp;ptype=1") + "\">全部卖出</a>");
                builder.Append(Out.Tab("</div>", "<br />"));

                if (ptype3 == 1)
                    strWhere = "0";
                else if (ptype3 == 2)
                    strWhere = "1";
                strOrder = "";
                DataSet ds = new BCW.farm.BLL.NC_GetCrop().GetList2("*", " a INNER JOIN tb_NC_shop b ON a.name_id=b.ID WHERE a.usid=" + meid + " AND a.num>0 AND b.iszengsong='" + strWhere + "' ORDER BY a.name_id ASC");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = 0; i < skt; i++)
                    {
                        int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);
                        string name = ds.Tables[0].Rows[koo + i]["name"].ToString();
                        int name_id = int.Parse(ds.Tables[0].Rows[koo + i]["name_id"].ToString());
                        int num = int.Parse(ds.Tables[0].Rows[koo + i]["num"].ToString());
                        int price_out = int.Parse(ds.Tables[0].Rows[koo + i]["price_out"].ToString());
                        string picture = ds.Tables[0].Rows[koo + i]["picture"].ToString();
                        int suoding = int.Parse(ds.Tables[0].Rows[koo + i]["suoding"].ToString());
                        int iszengsong = int.Parse(ds.Tables[0].Rows[koo + i]["iszengsong"].ToString());

                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }

                        if (!new BCW.farm.BLL.NC_shop().Exists_zzid2(name_id))
                        {
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + name + "<h style=\"color:#A9A9A9\">(" + (price_out) + "金币/个)</h>：x" + num + ".<br/>");
                            builder.Append("(抱歉,该商品[" + name + "]已下架,建议<a href=\"" + Utils.getUrl("farm.aspx?act=sell&amp;name_id=" + name_id + "&amp;ptype=3") + "\">马上卖出</a>)");
                        }
                        else
                        {
                            try
                            {
                                if (picture != "")
                                {
                                    string[] ii = picture.Split(',');
                                    builder.Append("<img src=\"" + ii[4] + "\" alt=\"load\"/><br/>");
                                }
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + name + "<h style=\"color:#A9A9A9\">(" + (price_out) + "金币/个)</h>：x" + num + "<br/>");
                            if (suoding == 0)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiasuo&amp;id=" + name_id + "&amp;ptype=1") + "\">加锁</a> . ");
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=sell&amp;id=" + name_id + "&amp;ptype=1") + "\">卖出</a>");
                                if (iszengsong == 1)
                                {
                                    if (ptype == 1)
                                    {
                                        builder.Append(" . <a href=\"" + Utils.getUrl("farm.aspx?act=hecheng&amp;id=" + name_id + "") + "\">合成</a>");
                                    }
                                }
                            }
                            else
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiesuo&amp;id=" + name_id + "&amp;ptype=1") + "\">解锁</a>");
                            }
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    if (ptype3 == 0)
                        builder.Append(Out.Div("div", "仓库里没有可卖的果实,去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">好友农场</a>摘点果实来卖吧!"));
                    else
                        builder.Append(Out.Div("div", "仓库里没有可赠送的果实,去<a href=\"" + Utils.getUrl("farm.aspx") + "\">农场</a>种植来赠送吧!"));
                }
            }
            else
            {
                strWhere = "usid=" + meid + " and num>0";
                strOrder = "GiftId asc";
                IList<BCW.farm.Model.NC_hecheng> listFarm = new BCW.farm.BLL.NC_hecheng().GetNC_hechengs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_hecheng n in listFarm)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        try
                        {
                            builder.Append("<img  src=\"" + n.PrevPic + "\" alt=\"load\"/><br/>");
                        }
                        catch { builder.Append("[图片出错!]<br/>"); }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.Title + "<h style=\"color:#A9A9A9\"></h>：x" + n.num + "");
                        builder.Append(" . <a href=\"" + Utils.getUrl("farm.aspx?act=zengsong&amp;d=" + n.GiftId + "&amp;type=1") + "\">赠送</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "仓库暂没可赠送的花!"));
                }
            }
            #endregion
        }
        if (ptype == 2)
        {
            #region 种子
            builder.Append(Out.Tab("<div>", ""));
            builder.Append("当前种子总价值:<h style=\"color:red\">" + new BCW.farm.BLL.NC_shop().get_usergoid(meid) + "</h>金币(不含锁定种子)<br/><a href=\"" + Utils.getUrl("farm.aspx?act=sellall&amp;ptype=2") + "\">全部卖出</a>");
            builder.Append(Out.Tab("</div>", "<br />"));

            strWhere = "usid=" + meid + " and type=1 and num>0 and name_id>0";
            IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    //获取id对应的用户名
                    string mename = new BCW.BLL.User().GetUsName(n.usid);
                    BCW.farm.Model.NC_shop b = new BCW.farm.BLL.NC_shop().GetNC_shop1(n.name_id);
                    try
                    {
                        builder.Append("<img  src=\"" + n.picture + "\" alt=\"load\"/><br/>");
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + "<h style=\"color:#A9A9A9\">(" + (b.price_in / 2) + "金币/个)</h>：x" + n.num + "<br/>");
                    if (n.suoding == 0)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiasuo&amp;id=" + n.name_id + "&amp;ptype=2") + "\">加锁</a> . ");
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=sell&amp;id=" + n.name_id + "&amp;ptype=2") + "\">卖出</a>");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiesuo&amp;id=" + n.name_id + "&amp;ptype=2") + "\">解锁</a>");
                    }
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "仓库里没有可种植的种子,去<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">商店</a>购买种子吧!"));
            }
            #endregion
        }
        if (ptype == 3)
        {
            #region 道具
            builder.Append(Out.Tab("<div>", ""));
            int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-3]$", "1"));
            if (ptype2 == 1)
            {
                builder.Append("<h style=\"color:red\">待使用" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3&amp;ptype2=1") + "\">待使用</a>" + "|");
            }
            if (ptype2 == 2)
            {
                builder.Append("<h style=\"color:red\">使用中" + "</h>" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3&amp;ptype2=2") + "\">使用中</a>" + "|");
            }
            if (ptype2 == 3)
            {
                builder.Append("<h style=\"color:red\">已使用" + "</h>" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3&amp;ptype2=3") + "\">已使用</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            if (ptype2 == 1)
            {
                #region 待使用
                strWhere = "usid=" + meid + " and type=2 and num>0 and huafei_id>0";
                strOrder = "huafei_id asc";
                // 开始读取列表
                IList<BCW.farm.Model.NC_mydaoju> listFarm = new BCW.farm.BLL.NC_mydaoju().GetNC_mydaojus2(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_mydaoju n in listFarm)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        try
                        {
                            builder.Append("<img src=\"" + n.picture + "\" alt=\"load\"/>");//new BCW.farm.BLL.NC_daoju().Get_picture(n.huafei_id)
                        }
                        catch { builder.Append("[图片出错!]<br/>"); }
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=" + n.huafei_id + "") + "\">" + n.name + "</a> x" + n.num + "<br/>");
                        if (n.iszengsong == 0)
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zengsong&amp;d=" + n.huafei_id + "&amp;type=2") + "\">[赠送]</a>.");
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_tw&amp;d=" + n.huafei_id + "") + "\">[摆摊]</a>.");
                        }
                        if ((0 < n.huafei_id) && (n.huafei_id < 21))//化肥
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shifei_1") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 21)//银钥匙
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 22)//宝箱钥匙
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 23 || n.huafei_id == 24)//自动收割机
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=use_daoju&amp;w=" + n.huafei_id + "") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 25 || n.huafei_id == 26 || n.huafei_id == 27)//双倍经验卡
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=use_daoju&amp;w=" + n.huafei_id + "") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 28)//种草卡
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=5") + "\">[使用]</a>");
                        }
                        else if (n.huafei_id == 29)//放虫卡
                        {
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=5") + "\">[使用]</a>");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

                }
                else
                {
                    builder.Append(Out.Div("div", "仓库里没有道具,去<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">商店</a>购买道具吧!"));
                }
                #endregion
            }
            else
            {
                #region 已使用
                if (ptype2 == 2)
                {
                    strWhere = "usid=" + meid + " and type=1";
                    strOrder = "updatetime desc";
                }
                if (ptype2 == 3)
                {
                    strOrder = "updatetime desc";
                    strWhere = "usid=" + meid + " and type=0";
                }

                // 开始读取列表
                IList<BCW.farm.Model.NC_daoju_use> listFarm = new BCW.farm.BLL.NC_daoju_use().GetNC_daoju_uses2(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_daoju_use n in listFarm)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        BCW.farm.Model.NC_daoju hu = new BCW.farm.BLL.NC_daoju().GetNC_daoju(n.daoju_id);
                        try
                        {
                            builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");
                        }
                        catch { builder.Append("[图片出错!]<br/>"); }
                        builder.Append("道具名称：" + hu.name + "<br/>使用时间：" + n.updatetime + "");
                        if (n.daoju_id == 23)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(1) + "");
                        }
                        else if (n.daoju_id == 21)
                        {
                            builder.Append("<br/>使用土地：第" + n.tudi + "块土地.");
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(xianjing_day) + "");
                        }
                        else if (n.daoju_id == 24)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(7) + "");
                        }
                        else if (n.daoju_id == 25)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(1) + "");
                        }
                        else if (n.daoju_id == 26)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(7) + "");
                        }
                        else if (n.daoju_id == 27)
                        {
                            builder.Append("<br/>预计到期时间：" + n.updatetime.AddDays(30) + "");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }

                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂无使用中的道具,前往<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian&amp;ptype=2") + "\">道具商店</a>购买吧!"));
                }
                #endregion
            }
            #endregion
        }

        foot_link();//底部链接

        foot_link2();
    }
    //合成
    private void hechengPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "我的仓库.果实合成";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">我的仓库</a>&gt;合成");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "选择合成ID出错"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok")
        {
            int id1 = Utils.ParseInt(Utils.GetRequest("id", "all", 1, @"^[1-9]\d*$", "选择合成ID出错"));
            BCW.farm.Model.NC_shop aa = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);
            if (aa.iszengsong == 1)
            {
                BCW.farm.Model.NC_GetCrop bb = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(id, meid);
                BCW.farm.Model.NC_shop yu = new BCW.farm.BLL.NC_shop().GetNC_shop1(id1);
                if (bb.num >= hecheng_num)
                {
                    BCW.User.Users.IsFresh("Farmhc", 2);//防刷
                    //减果实
                    new BCW.farm.BLL.NC_GetCrop().Update_gs(meid, -hecheng_num, id1);
                    if (new BCW.farm.BLL.NC_hecheng().Exists_ID(id1, meid))
                    {
                        new BCW.farm.BLL.NC_hecheng().update_ID("num=num+1,all_num=all_num+1,AddTime='" + DateTime.Now + "'", "usid=" + meid + " and GiftId=" + id1 + "");
                    }
                    else
                    {
                        //赠送表增加
                        BCW.farm.Model.NC_hecheng opp = new BCW.farm.Model.NC_hecheng();
                        opp.Title = bb.name;
                        opp.GiftId = bb.name_id;
                        opp.UsID = meid;
                        try
                        {
                            string[] bv = yu.picture.Split(',');
                            opp.PrevPic = bv[4];
                        }
                        catch
                        {
                            opp.PrevPic = "";
                        }
                        opp.num = 1;
                        opp.AddTime = DateTime.Now;
                        opp.all_num = 1;
                        new BCW.farm.BLL.NC_hecheng().Add(opp);
                    }
                    Utils.Success("合成", "合成成功,得到一个" + bb.name + ".", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=2"), "2");
                }
                else
                    Utils.Error("抱歉,你的果实不足" + hecheng_num + "个.赶快去<a href=\"" + Utils.getUrl("farm.aspx") + "\">种植</a>吧", "");
            }
            else
                Utils.Success("合成", "抱歉,该果实暂不支持合成,请选择其它果实.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=2"), "2");
        }
        else
        {
            if (new BCW.farm.BLL.NC_GetCrop().Exists_zuowu2(id, meid))//判断是否存在该果实
            {
                BCW.farm.Model.NC_shop aa = new BCW.farm.BLL.NC_shop().GetNC_shop1(id);//查询是否可以赠送
                if (aa.iszengsong == 1)
                {
                    BCW.farm.Model.NC_GetCrop bb = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(id, meid);
                    if (bb.num >= hecheng_num)
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("你要合成的果实是：" + bb.name + "<br/>");
                        builder.Append("温馨提示：<br/>你需要花费" + hecheng_num + "朵" + bb.name + "合成一束可以赠送的" + bb.name + ".自带" + bb.num + "个.");
                        builder.Append(Out.Tab("</div>", ""));
                        string strText = ",,,";
                        string strName = "id,act,info";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + id + "'hecheng'ok";
                        string strEmpt = "false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定合成,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    }
                    else
                        Utils.Error("抱歉,你的果实不足" + hecheng_num + "个.赶快去<a href=\"" + Utils.getUrl("farm.aspx") + "\">种植</a>吧", "");
                }
                else
                    Utils.Success("合成", "抱歉,该果实暂不支持合成,请选择其它果实.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=2"), "2");
            }
            else
                Utils.Success("合成", "抱歉,你选择合成数量不足或没有该果实.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=2"), "2");
        }
        foot_link();//底部链接

        foot_link2();
    }
    //仓库使用道具
    private void use_daojuPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int daoju_id = Utils.ParseInt(Utils.GetRequest("w", "all", 2, @"^[1-9]\d*$", "选择道具无效"));
        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf3(daoju_id, meid))//判断是否存在道具
        {
            BCW.User.Users.IsFresh("Farmsy", 1);//防刷
            BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();//添加道具使用记录
            hu.updatetime = DateTime.Now;
            hu.usid = meid;
            hu.tudi = 0;
            hu.type = 1;
            if (daoju_id == 23)//收割机1天
            {
                //判断是否重复使用
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 23)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 24)))
                {
                    Utils.Error("你已正在使用类似的道具,不能重复使用作用一样的道具.", "");
                }
                else
                {
                    hu.daoju_id = 23;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);//加使用记录
                    //减道具
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 23);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(23, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 23, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 23, 0);
                }
            }
            else if (daoju_id == 24)//收割机7天
            {
                //判断是否重复使用
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 23)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 24)))
                {
                    Utils.Error("你已正在使用类似的道具,不能重复使用作用一样的道具.", "");
                }
                else
                {
                    hu.daoju_id = 24;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);//加使用记录
                    //减道具
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 24);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(24, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 24, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 24, 0);
                }
            }
            else if (daoju_id == 25)//双倍经验卡(1天)
            {
                //判断是否重复使用
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    Utils.Error("你已正在使用类似的道具,不能重复使用作用一样的道具.", "");
                }
                else
                {
                    hu.daoju_id = 25;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);//加使用记录
                    //减道具
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 25);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(25, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 25, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 25, 0);
                }
            }
            else if (daoju_id == 26)//双倍经验卡(7天)
            {
                //判断是否重复使用
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    Utils.Error("你已正在使用类似的道具,不能重复使用作用一样的道具.", "");
                }
                else
                {
                    hu.daoju_id = 26;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);//加使用记录
                    //减道具
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 26);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(26, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 26, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 26, 0);
                }
            }
            else if (daoju_id == 27)//双倍经验卡(30天)
            {
                //判断是否重复使用
                if ((new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 25)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 26)) || (new BCW.farm.BLL.NC_daoju_use().Exists_daoju(meid, 27)))
                {
                    Utils.Error("你已正在使用类似的道具,不能重复使用作用一样的道具.", "");
                }
                else
                {
                    hu.daoju_id = 27;
                    new BCW.farm.BLL.NC_daoju_use().Add(hu);//加使用记录
                    //减道具
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 27);
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(27, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 27, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 27, 0);
                }
            }
            else
            {
                Utils.Error("选择道具无效,请重新选择.", "");
            }
            Utils.Success("使用道具", "使用" + OutType2(daoju_id) + "成功.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3"), "1");
        }
        else
            Utils.Error("道具不足,请到商店购买.", "");

    }
    //赠送
    private void zengsongPage()
    {
        //仓库维护提示
        if (ub.GetSub("zsStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1") + "\">我的仓库</a>&gt;赠送果实/道具");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int aaa = Utils.ParseInt(Utils.GetRequest("d", "all", 2, @"^[1-9]\d*$", "选择赠送道具出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-2]$", "1"));//1是果实2是道具

        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        if (ptype == 1)
        {
            if (type == 1)
                Master.Title = "我的果实.赠送";
            else
                Master.Title = "我的道具.赠送";
            builder.Append("<h style=\"color:red\">赠送" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zengsong&amp;ptype=1&amp;d=" + aaa + "&amp;type=" + type + "") + "\">赠送</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "我的道具.我的赠送记录";
            builder.Append("<h style=\"color:red\">我的赠送记录" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zengsong&amp;ptype=2&amp;d=" + aaa + "&amp;type=" + type + "") + "\">我的赠送记录</a>" + "");
        }
        builder.Append(Out.Tab("</div>", ""));
        string[] pageValUrl = { "act", "ptype", "type", "d", "backurl" };//#region #endregion
        if (ptype == 1)
        {
            string info = Utils.GetRequest("info", "all", 1, "", "");
            if (info == "ok2")//赠送出去
            {
                #region
                int num = Utils.ParseInt(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "选择数量出错"));
                int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "赠送用户ID出错"));
                int type_ok2 = int.Parse(Utils.GetRequest("type_ok", "all", 1, @"^[1-2]$", "1"));//1是果实2是道具
                string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,100}$", "赠言限100字内");
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=zengsong");
                string oo = string.Empty;
                if (!new BCW.farm.BLL.NC_user().Exists(usid))//判断是否有这个usid
                    Utils.Error("抱歉，该用户还没开通农场,<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">马上邀请他开通农场</a>,一起来玩耍吧.", "");

                if (meid == usid)
                    Utils.Error("抱歉，不能赠送道具给自己.请输入其他ID.", "");

                int d = Utils.ParseInt(Utils.GetRequest("d", "all", 2, @"^[1-9]\d*$", "选择赠送道具出错"));//ID


                if (type_ok2 == 1)
                {
                    if (new BCW.farm.BLL.NC_hecheng().Get_daoju_num(meid, d) >= num)
                    {
                        BCW.User.Users.IsFresh("Farmzs", 1);//防刷
                        new BCW.farm.BLL.NC_hecheng().Update_gs(meid, -num, d);//减果实

                        //邵广林 20160606 修改赠送花功能，花不计入农场仓库，计入收取礼物的表
                        BCW.farm.Model.NC_hecheng yo = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(d, meid);
                        //送礼记录+添加pic为1农场
                        BCW.Model.Shopsend send = new BCW.Model.Shopsend();
                        oo = yo.Title;//名称
                        send.Title = yo.Title;
                        send.GiftId = yo.GiftId;
                        send.PrevPic = yo.PrevPic;
                        send.Message = Content;
                        send.UsID = meid;
                        send.UsName = new BCW.BLL.User().GetUsName(meid);
                        send.ToID = usid;
                        send.ToName = new BCW.BLL.User().GetUsName(usid);
                        send.Total = num;
                        send.AddTime = DateTime.Now;
                        send.PIC = "1";
                        new BCW.BLL.Shopsend().Add(send);

                        //主人礼物记录+添加pic为1农场
                        BCW.Model.Shopuser user = new BCW.Model.Shopuser();
                        user.GiftId = yo.GiftId;
                        user.UsID = usid;
                        user.UsName = new BCW.BLL.User().GetUsName(usid);
                        user.PrevPic = yo.PrevPic;
                        user.GiftTitle = yo.Title;
                        user.Total = num;
                        user.AddTime = DateTime.Now;
                        user.PIC = "1";
                        if (!new BCW.BLL.Shopuser().Exists_nc(usid, yo.GiftId))//农场判断
                            new BCW.BLL.Shopuser().Add(user);
                        else
                            new BCW.BLL.Shopuser().Update_nc(user);//根据pic=1更新

                        //增加属性与值
                        BCW.farm.Model.NC_shop hj = new BCW.farm.BLL.NC_shop().GetNC_shop1(d);
                        string sParas = new BCW.BLL.User().GetParas(usid);
                        if (hj.tili != 0)
                            sParas = BCW.User.Users.GetParaData(sParas, hj.tili, 0);
                        if (hj.meili != 0)
                            sParas = BCW.User.Users.GetParaData(sParas, hj.meili, 1);
                        //更新属性值
                        new BCW.BLL.User().UpdateParas(usid, sParas);
                    }
                    else
                        Utils.Success("赠送", "抱歉，你的果实不足.", Utils.getUrl("farm.aspx?act=zengsong&amp;type=" + type_ok2 + "&amp;d=" + d + ""), "1");


                    //if (new BCW.farm.BLL.NC_GetCrop().Exists_zuowu3(d, usid))//查询赠送者是否存在该道具，若有就增加一个，没有就新增一个
                    //{
                    //    BCW.farm.Model.NC_GetCrop v = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(d, meid);//查询化肥数量
                    //    oo = v.name;//名称
                    //    new BCW.farm.BLL.NC_GetCrop().Update_gs(usid, num, d);//加果实
                    //}
                    //else
                    //{
                    //    if (new BCW.farm.BLL.NC_GetCrop().Exists_zuowu2(d, usid))
                    //    {
                    //        BCW.farm.Model.NC_GetCrop v = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(d, meid);//查询化肥数量
                    //        oo = v.name;//名称
                    //        new BCW.farm.BLL.NC_GetCrop().Update_gs(usid, num, d);//加果实
                    //    }
                    //    else
                    //    {
                    //        BCW.farm.Model.NC_shop t = new BCW.farm.BLL.NC_shop().GetNC_shop(d);
                    //        BCW.farm.Model.NC_GetCrop w = new BCW.farm.Model.NC_GetCrop();
                    //        oo = t.name;
                    //        w.name = t.name;
                    //        w.name_id = d;
                    //        w.num = num;
                    //        w.price_out = t.price_out;
                    //        w.suoding = 0;
                    //        w.usid = usid;
                    //        new BCW.farm.BLL.NC_GetCrop().Add(w);
                    //    }
                    //}
                }
                else
                {
                    DataSet zsnum = new BCW.farm.BLL.NC_user().GetList("zengsongnum", "usid=" + meid + "");
                    int y_num = 0;
                    try
                    {
                        y_num = int.Parse(zsnum.Tables[0].Rows[0]["zengsongnum"].ToString());//已赠送个数
                    }
                    catch { }

                    if (num > (zs_num - y_num))
                    {
                        Utils.Error("抱歉,你选择道具数量过多,每人每天可赠送道具" + zs_num + "个,你已赠送" + y_num + "个,还可赠送" + (zs_num - y_num) + "个.", "");
                    }
                    if (y_num > zs_num)
                    {
                        Utils.Error("抱歉,每人每天可赠送道具" + zs_num + "个,你今天赠送个数已满.", "");
                    }

                    //判断赠送的道具是否足够
                    if (new BCW.farm.BLL.NC_mydaoju().Get_daojunum2(meid, d, 0) >= num)
                    {
                        BCW.User.Users.IsFresh("Farmzs", 1);//防刷
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -num, d, 0);//减道具

                        //添加到user表的赠送数量
                        new BCW.farm.BLL.NC_user().update_zd("zengsongnum=zengsongnum+" + num + "", "usid=" + meid + "");

                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(d, usid, 1))//查询赠送者是否存在该道具，若有就增加一个，没有就新增一个
                        {
                            BCW.farm.Model.NC_mydaoju v = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 1);//查询化肥数量
                            oo = v.name;//名称
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(usid, num, d, 1);//加道具
                        }
                        else
                        {
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(d, usid, 1))
                            {
                                BCW.farm.Model.NC_mydaoju v = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 1);//查询化肥数量
                                oo = v.name;//名称
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(usid, num, d, 1);//加道具
                            }
                            else
                            {
                                BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(d);
                                BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                oo = t.name;
                                w.name = t.name;
                                w.name_id = 0;
                                w.num = num;
                                w.type = 2;
                                w.usid = usid;
                                w.zhonglei = 0;
                                w.huafei_id = d;
                                w.picture = t.picture;
                                w.iszengsong = 1;
                                new BCW.farm.BLL.NC_mydaoju().Add(w);
                            }
                        }
                    }
                    else
                        Utils.Success("赠送", "抱歉，你的道具不足.", Utils.getUrl("farm.aspx?act=zengsong&amp;type=" + type_ok2 + "&amp;d=" + d + ""), "1");
                }
                string mename2 = new BCW.BLL.User().GetUsName(usid);//用户姓名
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]赠送" + oo + "给" + mename2 + ".");
                //内线
                if (Content.Trim() == "")//判断留言是否为空
                {
                    new BCW.BLL.Guest().Add(1, usid, mename2, "" + mename + "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]赠送你" + num + "个" + oo + "");
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")慷慨解囊，收到Ta赠送" + num + "个" + oo + ".", 3);//消息
                }
                else
                {
                    new BCW.BLL.Guest().Add(1, usid, mename2, "" + mename + "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]赠送你" + num + "个" + oo + ".留言：" + Content + "");
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "" + mename + "(" + meid + ")慷慨解囊，收到Ta赠送" + num + "个" + oo + ".留言：" + Content + ".", 3);//消息
                }
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "你慷慨解囊,给农场好友" + mename2 + "(" + usid + ")赠送" + num + "个" + oo + ".", 3);//消息
                if (type_ok2 == 1)
                    Utils.Success("操作道具", "您成功赠送" + num + "个" + oo + "给" + mename2 + ".", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1&amp;ptype3=3"), "1");
                else
                    Utils.Success("操作道具", "您成功赠送" + num + "个" + oo + "给" + mename2 + ".", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3"), "1");

                #endregion
            }
            else if (info == "ok")//当前界面好友赠送
            {
                #region
                int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "赠送数量出错"));
                int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "赠送用户ID出错"));
                int type_ok = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-2]$", "1"));//1是果实2是道具
                string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,100}$", "赠言限100字内");
                string qq = string.Empty;

                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=zengsong");
                if (!new BCW.farm.BLL.NC_user().Exists(usid))//判断是否有这个usid
                    Utils.Error("抱歉，该用户还没开通农场,<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">马上邀请他开通农场</a>,一起来玩耍吧.", "");

                if (meid == usid)
                    Utils.Error("抱歉，不能赠送道具给自己.请输入其他ID.", "");

                int d = Utils.ParseInt(Utils.GetRequest("d", "all", 2, @"^[1-9]\d*$", "选择赠送道具出错"));//ID
                if (type_ok == 1)
                {
                    if (new BCW.farm.BLL.NC_hecheng().Exists_zuowu2(d, meid))
                    {
                        BCW.farm.Model.NC_hecheng bb = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(d, meid);
                        qq = bb.Title;
                    }
                    else
                        Utils.Error("抱歉，你的果实不足.", "");
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(d, meid, 0))
                    {
                        BCW.farm.Model.NC_mydaoju bb = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 0);
                        qq = bb.name;
                    }
                    else
                        Utils.Error("抱歉，你的道具不足.", "");
                }

                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("赠送人：" + new BCW.BLL.User().GetUsName(usid) + "<br/>");
                builder.Append("赠送物：" + qq + "<br/>");
                builder.Append("赠送数量：" + num + "<br/>");
                builder.Append("留言：" + Content + "");
                builder.Append(Out.Tab("</div>", ""));

                string strText = ",,,,,,,";
                string strName = "num,usid,Content,d,type_ok,act,info";
                string strType = "hidden,hidden,hidden,hidden,hidden,hidden,hidden";
                string strValu = "" + num + "'" + usid + "'" + Content + "'" + d + "'" + type_ok + "'zengsong'ok2";
                string strEmpt = "true,false,true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定赠送,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                #endregion
            }
            else if (info == "ok3")//列表好友赠送
            {
                #region
                //int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "赠送数量出错"));
                int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "赠送用户ID出错"));
                int type_ok3 = int.Parse(Utils.GetRequest("type", "all", 1, @"^[1-2]$", "1"));//1是果实2是道具
                string Content = Utils.GetRequest("Content", "all", 3, @"^[\s\S]{1,100}$", "赠言限100字内");
                string ww = string.Empty;

                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=zengsong");
                if (!new BCW.farm.BLL.NC_user().Exists(usid))//判断是否有这个usid
                    Utils.Error("抱歉，该用户还没开通农场,<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">马上邀请他开通农场</a>,一起来玩耍吧.", "");

                if (meid == usid)
                    Utils.Error("抱歉，不能赠送道具给自己.请输入其他ID.", "");

                int d = Utils.ParseInt(Utils.GetRequest("d", "all", 2, @"^[1-9]\d*$", "选择赠送道具出错"));//ID
                if (type_ok3 == 1)
                {
                    if (new BCW.farm.BLL.NC_hecheng().Exists_zuowu2(d, meid))
                    {
                        BCW.farm.Model.NC_hecheng bb = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(d, meid);
                        ww = bb.Title;
                    }
                    else
                        Utils.Error("抱歉，你的果实不足.", "");
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(d, meid, 0))
                    {
                        BCW.farm.Model.NC_mydaoju bb = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 0);
                        ww = bb.name;
                    }
                    else
                        Utils.Error("抱歉，你的道具不足.", "");
                }

                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("赠送人：" + new BCW.BLL.User().GetUsName(usid) + "<br/>");
                builder.Append("赠送物：" + ww + "");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "赠送数量：,留言：,,,,,";
                string strName = "num,Content,usid,d,type_ok,act,info";
                string strType = "num,text,hidden,hidden,hidden,hidden,hidden";
                string strValu = "''" + usid + "'" + d + "'" + type_ok3 + "'zengsong'ok2";
                string strEmpt = "true,true,false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定赠送,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                #endregion
            }
            else
            {
                #region
                int huafei_id = Utils.ParseInt(Utils.GetRequest("d", "all", 2, @"^[1-9]\d*$", "选择赠送道具出错"));
                string gg = string.Empty;
                int g = 0;
                if (type == 1)
                {
                    if (new BCW.farm.BLL.NC_hecheng().Exists_zuowu2(huafei_id, meid))
                    {
                        BCW.farm.Model.NC_hecheng aa = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(huafei_id, meid);
                        gg = aa.Title;
                        g = aa.num;
                    }
                    else
                        Utils.Error("抱歉，你的果实不足.", "");
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(huafei_id, meid, 0))
                    {
                        BCW.farm.Model.NC_mydaoju aa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, huafei_id, 0);
                        gg = aa.name;
                        g = aa.num;
                    }
                    else
                        Utils.Error("抱歉，你的道具不足.", "");
                }
                string strText = "赠送：" + gg + ",赠送好友的ID,留言：,,,,";
                string strName = "num,usid,Content,d,type,act,info";
                string strType = "num,num,text,hidden,hidden,hidden,hidden";
                string strValu = "" + g + "'''" + huafei_id + "'" + type + "'zengsong'ok";
                string strEmpt = "true,false,true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "赠送,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
                builder.Append("或选择以下农场好友：");
                builder.Append(Out.Tab("</div>", "<br/>"));

                int pageIndex;
                int recordCount;
                int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
                string strWhere = "usid!=" + meid + "";

                pageIndex = Utils.ParseInt(Request.QueryString["page"]);
                if (pageIndex == 0)
                    pageIndex = 1;

                string strOrder = "grade desc";

                IList<BCW.farm.Model.NC_user> listFarm = new BCW.farm.BLL.NC_user().GetNC_users(pageIndex, pageSize, strWhere, strOrder, out recordCount);
                if (listFarm.Count > 0)
                {
                    int k = 1;
                    foreach (BCW.farm.Model.NC_user n in listFarm)
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        //获取id对应的用户名
                        string mename3 = new BCW.BLL.User().GetUsName(n.usid);
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + n.usid + "") + "\">" + mename3 + "(" + n.usid + ")</a>.");
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zengsong&amp;info=ok3&amp;usid=" + n.usid + "&amp;d=" + huafei_id + "&amp;type=" + type + "") + "\">[赠送]</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    // 分页
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有好友记录.."));
                }
                #endregion
            }
        }
        else
        {
            #region 赠送记录
            builder.Append(Out.Tab("<div></div>", "<br />"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strWhere = " ";
            //string[] pageValUrl = { "act", "ptype", "type", "d", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);

            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "UsId=" + meid + " and type=3";//NC_messagelog赠送记录type=3
            IList<BCW.farm.Model.NC_messagelog> listFarm = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_messagelog n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
                builder.Append(Out.Div("div", "没有赠送记录.."));
            #endregion
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">我的道具>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }
    //加锁
    private void jiasuoPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择种子ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            if (new BCW.farm.BLL.NC_GetCrop().Exists_zuowu2(id, meid))
            {
                new BCW.farm.BLL.NC_GetCrop().Update_suoding(meid, id);
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Utils.Success("操作种子", "锁定成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "1");
                }
                else
                {
                    Utils.Success("操作种子", "锁定成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "0");
                }
            }
            else
            {
                Utils.Success("操作种子", "没有该记录", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "1");
            }
        }
        else
        {
            if (new BCW.farm.BLL.NC_mydaoju().Exists_zz(id))
            {
                new BCW.farm.BLL.NC_mydaoju().Update_sd(meid, id);
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Utils.Success("操作种子", "锁定成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
                }
                else
                {
                    Utils.Success("操作种子", "锁定成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "0");
                }
            }
            else
            {
                Utils.Success("操作种子", "没有该记录", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
            }

        }

    }
    //解锁
    private void jiesuoPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择种子ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            if (new BCW.farm.BLL.NC_GetCrop().Exists_zuowu2(id, meid))
            {
                new BCW.farm.BLL.NC_GetCrop().Update_jiesuo(meid, id);
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Utils.Success("操作种子", "解锁成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "1");
                }
                else
                {
                    Utils.Success("操作种子", "解锁成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "0");
                }
            }
            else
            {
                Utils.Success("操作种子", "没有该记录", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "1");
            }
        }
        else
        {
            if (new BCW.farm.BLL.NC_mydaoju().Exists_zz(id))
            {
                new BCW.farm.BLL.NC_mydaoju().Update_jiesuo(meid, id);
                if (!Utils.Isie() && !Utils.GetUA().ToLower().Contains("opera/8"))
                {
                    Utils.Success("操作种子", "解锁成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
                }
                else
                {
                    Utils.Success("操作种子", "解锁成功", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "0");
                }
            }
            else
            {
                Utils.Success("操作种子", "没有该记录", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
            }
        }

    }
    //单个卖出
    private void sellPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "单个卖出";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">我的仓库</a>&gt;卖出果实");
        builder.Append(Out.Tab("</div>", "<br />"));
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string info = Utils.GetRequest("info", "post", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptype == 1)
        {
            if (info == "ok")
            {
                BCW.User.Users.IsFresh("Farmmc", 3);//防刷
                int Num1 = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "请输入需要卖出的正确个数"));//卖出数量
                int Num2 = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "请选择正确的种子"));//种植id
                BCW.farm.Model.NC_GetCrop b = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(Num2, meid);
                if (b.suoding == 1)
                {
                    Utils.Error("抱歉,该果实已经锁定,请先解锁在卖出.", "");
                }
                if (Num1 == 0 || Num1 > b.num)
                {
                    Utils.Error("没有该果实或请输入需要卖出的正确个数", "");
                }
                else
                {
                    new BCW.farm.BLL.NC_GetCrop().Update_maichu(meid, -Num1, Num2);//减数量
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, Num1 * b.price_out, "在仓库卖出" + Num1 + "个" + b.name + ",获得" + Num1 * b.price_out + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (Num1 * b.price_out)) + "金币.", 4);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在仓库卖出" + Num1 + "个" + b.name + ".", 4);//消息
                }
                Utils.Success("操作果实", "卖出成功,获得" + Num1 * b.price_out + "金币", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "3");
            }
            else
            {
                int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择种子ID无效"));
                BCW.farm.Model.NC_GetCrop a = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrop2(id, meid);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你共有" + a.num + "个" + a.name + ",单价" + (a.price_out) + "金币,总价" + a.num * (a.price_out) + "金币.");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "请输入卖出数量：/,,,,";
                string strName = "num,id,ptype,act,info";
                string strType = "num,hidden,hidden,hidden,hidden";
                string strValu = "" + a.num + "'" + id + "'" + ptype + "'sell'ok";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else if (ptype == 2)
        {
            if (info == "ok2")
            {
                BCW.User.Users.IsFresh("Farmmc", 3);//防刷
                int Num1 = int.Parse(Utils.GetRequest("num", "post", 2, @"^[1-9]\d*$", "请输入需要卖出的正确个数"));//卖出数量
                int Num2 = int.Parse(Utils.GetRequest("id", "post", 2, @"^[1-9]\d*$", "请选择正确的种子"));//种植id
                BCW.farm.Model.NC_mydaoju b = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, Num2);
                if (b.suoding == 1)
                {
                    Utils.Error("抱歉,该种子已经锁定,请先解锁在卖出.", "");
                }
                if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, Num2) == 0)
                {
                    Utils.Error("种子数量不足,请重新选择.", "");
                }
                BCW.farm.Model.NC_shop c = new BCW.farm.BLL.NC_shop().GetNC_shop1(b.name_id);
                if (Num1 == 0 || Num1 > b.num)
                {
                    Utils.Error("请输入需要卖出的正确个数", "");
                }
                else
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, -Num1, Num2);//减数量
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, Num1 * (c.price_in / 2), "在仓库卖出" + Num1 + "个" + b.name + ",获得" + Num1 * (c.price_in / 2) + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (Num1 * (c.price_in / 2))) + "金币.", 4);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在仓库卖出" + Num1 + "个" + b.name + ".", 4);//消息
                }
                Utils.Success("操作果实", "卖出成功,获得" + Num1 * (c.price_in / 2) + "金币", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "3");
            }
            else
            {
                int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择种子ID无效"));
                BCW.farm.Model.NC_mydaoju a = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, id);
                if (new BCW.farm.BLL.NC_mydaoju().Get_daoju_num(meid, id) == 0)
                {
                    Utils.Error("种子数量不足,请重新选择.", "");
                }
                BCW.farm.Model.NC_shop b = new BCW.farm.BLL.NC_shop().GetNC_shop1(a.name_id);
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你共有" + a.num + "个" + a.name + ",单价" + b.price_in / 2 + "金币,总价" + a.num * (b.price_in / 2) + "金币.");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "请输入卖出数量：/,,,,";
                string strName = "num,id,ptype,act,info";
                string strType = "num,hidden,hidden,hidden,hidden";
                string strValu = "" + a.num + "'" + id + "'" + ptype + "'sell'ok2";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "确定,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else
        {
            BCW.User.Users.IsFresh("Farmmc", 3);//防刷
            int nameid = int.Parse(Utils.GetRequest("name_id", "all", 2, @"^[1-9]\d*$", "请选择正确的果实ID"));
            if (!new BCW.farm.BLL.NC_shop().Exists_zzid2(nameid))
            {
                DataSet ds = new BCW.farm.BLL.NC_GetCrop().GetList("*", "usid=" + meid + " and num>0 and name_id=" + nameid + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    long abc = 0;
                    string yyy = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string name = (ds.Tables[0].Rows[i]["name"].ToString());//作物名称
                        int name_id = int.Parse(ds.Tables[0].Rows[i]["name_id"].ToString());//作物id
                        int num = int.Parse(ds.Tables[0].Rows[i]["num"].ToString());//数量
                        int price_out = int.Parse(ds.Tables[0].Rows[i]["price_out"].ToString());//卖出价钱
                        int suoding = int.Parse(ds.Tables[0].Rows[i]["suoding"].ToString());//锁定
                        new BCW.farm.BLL.NC_GetCrop().Update_maichu(meid, -num, name_id);//减数量
                        abc = abc + num * price_out;
                        yyy = yyy + num + "个" + name + "";
                    }
                    if (abc > 0)
                    {
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, abc, "在仓库卖出过期商品" + yyy + ",获得" + abc + "金币,目前拥有" + ((new BCW.farm.BLL.NC_user().GetGold(meid)) + abc) + "金币.", 4);
                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在仓库卖出过期商品" + yyy + ".", 4);//消息
                        Utils.Success("操作果实", "卖出成功,获得" + abc + "金币", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "3");
                    }
                    else
                        Utils.Success("操作果实", "仓库没有可卖出的果实.", Utils.getUrl("farm.aspx?act=cangku"), "1");
                }
                else
                    Utils.Success("操作果实", "仓库没有可卖出的果实.", Utils.getUrl("farm.aspx?act=cangku"), "1");
            }
            else
                Utils.Error("你输入的果实ID还没下架.请重新选择.", "");
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">再看看吧>></a>");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接

        foot_link2();
    }
    //全部卖出
    private void sellallPage()
    {
        //仓库维护提示
        if (ub.GetSub("ckStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "全部卖出";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        long abc = 0;
        string yyy = string.Empty;
        string info = Utils.GetRequest("info", "post", 1, "", "");
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)//果实
        {
            if (info == "ok")
            {
                BCW.User.Users.IsFresh("Farmmc", 3);//防刷
                DataSet ds = new BCW.farm.BLL.NC_GetCrop().GetList("*", "usid=" + meid + " and suoding=0 and num>0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string name = (ds.Tables[0].Rows[i]["name"].ToString());//作物名称
                        int name_id = int.Parse(ds.Tables[0].Rows[i]["name_id"].ToString());//作物id
                        int num = int.Parse(ds.Tables[0].Rows[i]["num"].ToString());//数量
                        int price_out = int.Parse(ds.Tables[0].Rows[i]["price_out"].ToString());//卖出价钱
                        int suoding = int.Parse(ds.Tables[0].Rows[i]["suoding"].ToString());//锁定

                        if (suoding == 0)
                        {
                            new BCW.farm.BLL.NC_GetCrop().Update_maichu(meid, -num, name_id);//减数量
                            abc = abc + num * price_out;
                            yyy = yyy + num + "个" + name + ",";
                        }
                    }
                    if (abc > 0)
                    {
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, abc, "在仓库卖出" + yyy + "获得" + abc + "金币,目前拥有" + ((new BCW.farm.BLL.NC_user().GetGold(meid)) + abc) + "金币.", 4);
                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在仓库卖出" + yyy + "", 4);//消息
                        Utils.Success("操作果实", "全部卖出成功（不包括锁定的果实）,获得" + abc + "金币", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "3");
                    }
                    else
                        Utils.Success("操作果实", "仓库没有可卖出的果实.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=1"), "1");
                }
                else
                {
                    Utils.Success("操作果实", "仓库没有可卖出的果实.", Utils.getUrl("farm.aspx?act=cangku"), "1");
                }
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">我的仓库</a>&gt;卖出全部果实");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你确定要卖出仓库里的所有果实？");
                builder.Append(Out.Tab("</div>", ""));

                string strText = ",,,";
                string strName = "ptype,act,info";
                string strType = "hidden,hidden,hidden";
                string strValu = "" + ptype + "'sellall'ok";
                string strEmpt = "false,false,false";
                string strIdea = "/";
                string strOthe = "确定卖出,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else//种子
        {
            if (info == "ok2")
            {
                BCW.User.Users.IsFresh("Farmmc", 3);//防刷
                DataSet ds = new BCW.farm.BLL.NC_mydaoju().GetList("*", "usid=" + meid + " and suoding=0 and num>0 and name_id>0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string name = (ds.Tables[0].Rows[i]["name"].ToString());//作物名称
                        int name_id = int.Parse(ds.Tables[0].Rows[i]["name_id"].ToString());//作物id
                        int num = int.Parse(ds.Tables[0].Rows[i]["num"].ToString());//数量
                        int suoding = int.Parse(ds.Tables[0].Rows[i]["suoding"].ToString());//锁定
                        if (suoding == 0)
                        {
                            BCW.farm.Model.NC_shop o = new BCW.farm.BLL.NC_shop().GetNC_shop1(name_id);//卖出价钱
                            long price_out = o.price_in / 2;
                            new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, -num, name_id);//减数量

                            abc = abc + num * price_out;
                            yyy = yyy + num + "个" + name + ",";
                        }
                    }
                    if (abc > 0)
                    {
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, abc, "在仓库卖出" + yyy + "获得" + abc + "金币,目前拥有" + ((new BCW.farm.BLL.NC_user().GetGold(meid)) + abc) + "金币.", 4);
                        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在仓库卖出" + yyy + "", 4);//消息
                        Utils.Success("操作种子", "全部卖出成功（不包括锁定的种子）,获得" + abc + "金币", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "3");
                    }
                    else
                        Utils.Success("操作种子", "仓库没有可卖出的种子.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
                }
                else
                {
                    Utils.Success("操作种子", "仓库没有可卖出的种子.", Utils.getUrl("farm.aspx?act=cangku&amp;ptype=2"), "1");
                }
            }
            else
            {
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">我的仓库</a>&gt;卖出全部种子");
                builder.Append(Out.Tab("</div>", "<br />"));
                builder.Append(Out.Tab("<div class=\"text\">", ""));
                builder.Append("你确定要卖出仓库里的所有种子？");
                builder.Append(Out.Tab("</div>", ""));

                string strText = ",,,";
                string strName = "ptype,act,info";
                string strType = "hidden,hidden,hidden";
                string strValu = "" + ptype + "'sellall'ok2";
                string strEmpt = "false,false,false";
                string strIdea = "/";
                string strOthe = "确定卖出,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">再看看吧>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }

    //个人信息
    private void MessagePage()
    {
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;个人信息");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + meid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + mename + "(" + meid + ")</a><br/>");
        builder.Append("等级:<a href=\"" + Utils.getUrl("farm.aspx?&amp;act=dengji") + "\">" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "</a> ");
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) <= 20)
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 200) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 40) && (new BCW.farm.BLL.NC_user().GetGrade(meid) > 20))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 250) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 50) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 41))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 500) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 60) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 51))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 700) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 70) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 61))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 900) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 80) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 71))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1300) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 90) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 81))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1700) + "</a><br/>");
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 100) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 91))
        {
            builder.Append("经验:<a href=\"" + Utils.getUrl("farm.aspx?act=dengji") + "\">" + new BCW.farm.BLL.NC_user().Getjingyan(meid) + "/" + (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 2500) + "</a><br/>");
        }



        builder.Append("" + ub.Get("SiteBz") + ":<a href=\"" + Utils.getUrl("../finance.aspx") + "\">" + (new BCW.BLL.User().GetGold(meid)) + "</a> ");
        builder.Append("金币:" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + " ");//Currency
        if (ub.GetSub("Currency", xmlPath) == "0")
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=Currency") + "\">[换币]</a>");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-4]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "个人信息.信息";
            builder.Append("<h style=\"color:red\">信息</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=1") + "\">信息</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "个人信息.消费记录";
            builder.Append("<h style=\"color:red\">消费</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=2") + "\">消费</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "个人信息.战绩";
            builder.Append("<h style=\"color:red\">战绩</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=3") + "\">战绩</a>" + "|");
        }
        if (ptype == 4)
        {
            builder.Append("<h style=\"color:red\">留言</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">留言</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "ptype2", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);

        if (pageIndex == 0)
            pageIndex = 1;

        string strOrder = "";

        //查询条件
        if (ptype == 1)
        {
            strWhere = "UsId=" + meid + "";//查询前100条
            IList<BCW.farm.Model.NC_messagelog> listFarm = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex, pageSize, strWhere, out recordCount);
            if ((listFarm.Count > 0))
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_messagelog n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                if (recordCount < 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有消息记录.."));
            }
        }
        if (ptype == 2)
        {
            //20160427删除收入支出显示
            //builder.Append(Out.Tab("<div>", ""));
            //int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-2]$", "1"));
            //if (ptype2 == 1)
            //{
            //    builder.Append("<h style=\"color:red\">收入明细" + "</h>" + " | ");
            //    strWhere = "UsId=" + meid + " and AcGold>0";//查询前100条
            //}
            //else
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=2&amp;ptype2=1") + "\">收入明细</a>" + " | ");
            //}
            //if (ptype2 == 2)
            //{
            //    builder.Append("<h style=\"color:red\">支出明细" + "</h>" + "");
            //    strWhere = "UsId=" + meid + " and AcGold<0";//查询前100条
            //}
            //else
            //{
            //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message&amp;ptype=2&amp;ptype2=2") + "\">支出明细</a>" + "");
            //}
            //builder.Append(Out.Tab("</div>", "<br />"));
            //strOrder = "id desc";

            strWhere = "UsId=" + meid + "";
            strOrder = "AddTime desc";
            IList<BCW.farm.Model.NC_Goldlog> listFarm = new BCW.farm.BLL.NC_Goldlog().GetNC_Goldlogs(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_Goldlog n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                if (recordCount < 100)
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 100, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有消费记录.."));
            }
        }
        if (ptype == 3)
        {
            strWhere = "UsId=" + meid + " AND (tou_nums>0 OR get_nums>0)";
            strOrder = "name_id asc";
            IList<BCW.farm.Model.NC_GetCrop> listFarm = new BCW.farm.BLL.NC_GetCrop().GetNC_GetCrops(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_GetCrop n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.name + ":收获" + (n.get_nums) + ".偷取" + n.tou_nums + "");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有相关记录.."));
            }
        }

        foot_link();//底部链接

        foot_link2();
    }

    //签到---道具不可赠送
    private void QiandiaoPage()
    {
        Master.Title = "每日签到";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;签到");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        BCW.farm.Model.NC_user model = new BCW.farm.BLL.NC_user().GetSignData(meid);
        if (string.IsNullOrEmpty(model.SignTime.ToString()))
        {
            model.SignTime = DateTime.Now.AddDays(-1);
        }
        if (model.SignTime > DateTime.Parse(DateTime.Now.ToLongDateString()))
        {
            Utils.Error("今天你已签到过了！<br /><a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=3") + "\">签到排行榜</a> . <a href=\"" + Utils.getUrl("farm.aspx?act=qiandaolook") + "\">我的签到奖励</a>", "");
        }
        int SignKeep = 1;
        int SignTotal = model.SignTotal + 1;
        if (model.SignTime >= DateTime.Parse(DateTime.Now.ToLongDateString()).AddDays(-1))
        {
            SignKeep = model.SignKeep + 1;
        }
        BCW.User.Users.IsFresh("Farmqd", 2);//防刷
        //更新签到信息
        new BCW.farm.BLL.NC_user().UpdateSingData(meid, SignTotal, SignKeep);

        //签到奖励
        string cc = string.Empty;
        string dd = string.Empty;
        int aa = GetPtype();
        int bb = GetPtype2();//签到专用
        if (aa == 20 || aa == 19)//奖励道具
        {
            if (bb == 5)//飞速化肥==6
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(6, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 6, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 6, 1);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(6, meid, 1))
                    {
                        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 6, 1);//查询化肥数量
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 6, 1);
                    }
                    else
                    {
                        BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(6);
                        BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                        w.name = t.name;
                        w.name_id = 0;
                        w.num = 1;
                        w.type = 2;
                        w.usid = meid;
                        w.zhonglei = 0;
                        w.huafei_id = 6;
                        w.picture = t.picture;
                        w.iszengsong = 1;
                        new BCW.farm.BLL.NC_mydaoju().Add(w);
                    }
                }
                cc = "飞速化肥一包";
            }
            else if (bb == 4)//极速化肥
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(4, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 4, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 4, 1);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(4, meid, 1))
                    {
                        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 4, 1);//查询化肥数量
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 4, 1);
                    }
                    else
                    {
                        BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(4);
                        BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                        w.name = t.name;
                        w.name_id = 0;
                        w.num = 1;
                        w.type = 2;
                        w.usid = meid;
                        w.zhonglei = 0;
                        w.huafei_id = 4;
                        w.picture = t.picture;
                        w.iszengsong = 1;
                        new BCW.farm.BLL.NC_mydaoju().Add(w);
                    }
                }
                cc = "极速化肥一包";
            }
            else if (bb == 3)//高速化肥
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(2, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 2, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 2, 1);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(2, meid, 1))
                    {
                        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 2, 1);//查询化肥数量
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 2, 1);
                    }
                    else
                    {
                        BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(2);
                        BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                        w.name = t.name;
                        w.name_id = 0;
                        w.num = 1;
                        w.type = 2;
                        w.usid = meid;
                        w.zhonglei = 0;
                        w.huafei_id = 2;
                        w.picture = t.picture;
                        w.iszengsong = 1;
                        new BCW.farm.BLL.NC_mydaoju().Add(w);
                    }
                }
                cc = "高速化肥一包";
            }
            else//普通化肥
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(1, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 1, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 1, 1);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(1, meid, 1))
                    {
                        BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 1, 1);//查询化肥数量
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 1, 1);
                    }
                    else
                    {
                        BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(1);
                        BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                        w.name = t.name;
                        w.name_id = 0;
                        w.num = 1;
                        w.type = 2;
                        w.usid = meid;
                        w.zhonglei = 0;
                        w.huafei_id = 1;
                        w.picture = t.picture;
                        w.iszengsong = 1;
                        new BCW.farm.BLL.NC_mydaoju().Add(w);
                    }

                }
                cc = "普通化肥一包";
            }
        }
        else if (aa == 18 || aa == 17 || aa == 16 || aa == 15)//奖励酷币
        {
            new BCW.BLL.User().UpdateiGold(meid, mename, bb * qd_jishu, "您在农场签到获得" + bb * qd_jishu + "" + ub.Get("SiteBz") + ".");//加酷币
            cc = "" + bb * qd_jishu + "" + ub.Get("SiteBz") + "";
        }
        else//奖励金币
        {
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, bb * qd_jishu_jinbi, "在农场签到,获得" + bb * qd_jishu_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (bb * qd_jishu_jinbi)) + "金币.", 5);
            cc = "" + bb * qd_jishu_jinbi + "金币";
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("签到成功,奖励" + cc + ".");
        if (SignKeep % 30 == 0)
        {
            //极速有机化肥==17
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(17, meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 17, 1);//查询化肥数量
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 17, 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(17, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 17, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 17, 1);
                }
                else
                {
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(17);
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    w.name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = 17;
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }

            }
            //邵广林 20161031 减少签到奖励酷币
            new BCW.BLL.User().UpdateiGold(meid, mename, 300, "您在农场连续签到一个月获得300" + ub.Get("SiteBz") + ".");//加酷币500
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 500, "在农场连续签到一个月,获得" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 500 + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + ((new BCW.farm.BLL.NC_user().GetGrade(meid)) * 500)) + "金币.", 5);
            dd = "300" + ub.Get("SiteBz") + "、" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 500 + "金币、极速有机化肥一袋";

            builder.Append("<br />连续签到一个月,额外追加奖励" + dd + "");
            //动态记录
            new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]连续签到一个月,奖励" + cc + "和额外追加奖励" + dd + ".");
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场签到成功,已连续签到一个月.奖励" + cc + "和额外追加奖励" + dd + ".", 5);//消息
        }
        else if (SignKeep % 7 == 0)
        {
            //高速有机化肥==15
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(15, meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 15, 1);//查询化肥数量
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 15, 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(15, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, 15, 1);//查询化肥数量
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, 15, 1);
                }
                else
                {
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(15);
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    w.name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = 15;
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }
            }

            new BCW.BLL.User().UpdateiGold(meid, mename, 70, "您在农场连续签到一周获得70" + ub.Get("SiteBz") + ".");//加酷币 原200
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 200, "在农场连续签到一周,获得" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 200 + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + ((new BCW.farm.BLL.NC_user().GetGrade(meid)) * 200)) + "金币.", 5);
            dd = "70" + ub.Get("SiteBz") + "、" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * 200 + "金币、高速有机化肥一袋";

            builder.Append("<br />连续签到一周,额外追加奖励" + dd + "");
            //动态记录
            new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]连续签到一周,奖励" + cc + "和额外追加奖励" + dd + ".");
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场签到成功,已连续签到一周.奖励" + cc + "和额外追加奖励" + dd + ".", 5);//消息
        }
        else
        {
            //动态记录
            new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]签到获得奖励" + cc + ".");
            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场签到成功,奖励" + cc + ".", 5);//消息
        }
        if (SignKeep > 1)
        {
            builder.Append("<br />您已经连续签到" + SignKeep + "天,累计签到" + SignTotal + "次.");
        }
        else
        {
            builder.Append("<br />每天坚持签到将获得意外的惊喜哦！");
        }
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=qiandaolook") + "\">我的签到奖励</a><br/><a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=3") + "\">签到排行榜>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }

    //查看签到奖励
    private void qiandaolookPage()
    {
        Master.Title = "签到奖励说明";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=qiandao") + "\">签到</a>&gt;签到奖励说明");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        BCW.farm.Model.NC_user model = new BCW.farm.BLL.NC_user().GetSignData(meid);
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("您已经连续签到" + model.SignKeep + "天,累计签到" + model.SignTotal + "次.再接再厉哦.<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("我的签到奖励：<br/>");
        builder.Append(Out.Tab("</div>", ""));


        int pageIndex;
        int recordCount;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);

        if (pageIndex == 0)
            pageIndex = 1;

        strWhere = "UsId=" + meid + " and type=5";//NC_messagelog农场签到type=5
        IList<BCW.farm.Model.NC_messagelog> listFarm = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex, pageSize, strWhere, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_messagelog n in listFarm)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有签到记录.."));
        }

        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        builder.Append("签到说明：<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        builder.Append("1.每天签到，有机会随机获得" + ub.Get("SiteBz") + "、金币、道具等奖励.<br/>");
        builder.Append("2.连续一周签到，根据农场等级，获得N*200金币、70" + ub.Get("SiteBz") + "奖励,并随机获得化肥.(N为等级)<br/>");
        builder.Append("3.连续一个月签到，根据农场等级，获得N*500金币、300" + ub.Get("SiteBz") + "奖励,并随机获得有机化肥.(N为等级)");
        builder.Append(Out.Tab("</div>", ""));


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban&amp;ptype=3") + "\">签到排行榜>></a>");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接

        foot_link2();
    }

    //功能--升级、一键
    private void gongnegnPage()
    {
        //功能维护提示
        if (ub.GetSub("gnStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;功能");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            Master.Title = "功能.土地扩建";
            builder.Append("<h style=\"color:red\">土地扩建" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=1") + "\">土地扩建</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "功能.一键功能";
            builder.Append("<h style=\"color:red\">一键功能" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=gongnegn&amp;ptype=2") + "\">一键功能</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        if (ptype == 1)
        {
            BCW.farm.Model.NC_tudi a = new BCW.farm.BLL.NC_tudi().Get_tudinum(meid);//查询id有几块土地
            BCW.farm.Model.NC_user b = new BCW.farm.BLL.NC_user().Get_user(meid);

            string info = Utils.GetRequest("info", "post", 1, "", "");
            long Price = 0;//钱
            int tudinum = 0;//土地ID 

            if (info == "ok1")//扩建24块土地
            {
                #region 扩建24块土地
                int id = Utils.ParseInt(Utils.GetRequest("tudinum", "post", 2, @"^[1-9]\d*$", "土地出错"));
                switch (id)
                {
                    case 7:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 5)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 10000)
                                Price = 10000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 8:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 7)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 20000)
                                Price = 20000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 9:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 9)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 30000)
                                Price = 30000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 10:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 11)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 50000)
                                Price = 50000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 11:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 13)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 70000)
                                Price = 70000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                        {
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        }
                        break;
                    case 12:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 15)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 90000)
                                Price = 90000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 13:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 17)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 120000)
                                Price = 120000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 14:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 19)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 150000)
                                Price = 150000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 15:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 21)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 180000)
                                Price = 180000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 16:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 23)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 230000)
                                Price = 230000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 17:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 25)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 300000)
                                Price = 300000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 18:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 27)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 500000)
                                Price = 500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 19:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 29)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 850000)
                                Price = 850000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 20:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 31)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1100000)
                                Price = 1100000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 21:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 33)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1300000)
                                Price = 1300000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 22:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 35)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1500000)
                                Price = 1500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 23:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 37)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1700000)
                                Price = 1700000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 24:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 39)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2000000)
                                Price = 2000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                }

                if (id == (a.aa + 1))
                {
                    BCW.User.Users.IsFresh("Farmkj", 2);//防刷
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -Price, "在农场扩建了第" + id + "块普通土地,花费了" + Price + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - Price) + "金币.", 10);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场扩建了第" + id + "块普通土地.", 10);//消息
                    BCW.farm.Model.NC_tudi c = new BCW.farm.Model.NC_tudi();
                    c.iscao = 0;
                    c.iswater = 0;
                    c.isinsect = 0;
                    c.harvest = 0;
                    c.ischandi = 0;
                    c.isshifei = 0;
                    c.tudi_type = 1;
                    c.updatetime = DateTime.Now;
                    c.tudi = id;
                    c.usid = meid;
                    c.zuowu = "";
                    c.zuowu_experience = 0;
                    c.zuowu_ji = 0;
                    c.zuowu_time = 0;
                    c.output = "";
                    new BCW.farm.BLL.NC_tudi().Add(c);
                    //动态记录
                    new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]扩建土地成功.");
                    Utils.Success("土地升级", "恭喜您,土地扩建成功.", Utils.getUrl("farm.aspx?act=gongnegn"), "2");
                }
                else
                {
                    Utils.Success("土地升级错误", "升级出错,请重新升级.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                #endregion
            }
            else if (info == "ok2")//升级为红土地.共花费3109万
            {
                #region 升级为红土地
                int id2 = Utils.ParseInt(Utils.GetRequest("tudinum", "post", 2, @"^[1-9]\d*$", "土地出错"));
                switch (id2)
                {
                    case 1:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 28)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 200000)
                                Price = 200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 2:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 29)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 220000)
                                Price = 220000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 3:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 30)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 240000)
                                Price = 240000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 4:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 31)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 260000)
                                Price = 260000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 5:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 32)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 290000)
                                Price = 290000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 6:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 33)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 320000)
                                Price = 320000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 7:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 34)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 350000)
                                Price = 350000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 8:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 35)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 380000)
                                Price = 380000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 9:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 36)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 410000)
                                Price = 410000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 10:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 37)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 440000)
                                Price = 440000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 11:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 38)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 480000)
                                Price = 480000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                        {
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        }
                        break;
                    case 12:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 39)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 520000)
                                Price = 520000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 13:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 40)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 560000)
                                Price = 560000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 14:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 41)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 600000)
                                Price = 600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 15:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 42)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 650000)
                                Price = 650000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 16:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 43)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 700000)
                                Price = 700000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 17:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 44)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 770000)
                                Price = 770000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 18:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 45)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 900000)
                                Price = 900000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 19:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 47)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1500000)
                                Price = 1500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 20:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 49)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2000000)
                                Price = 2000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 21:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 51)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 3000000)
                                Price = 3000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 22:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 53)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4000000)
                                Price = 4000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 23:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 55)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5500000)
                                Price = 5500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 24:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 57)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6800000)
                                Price = 6800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                }
                if (!new BCW.farm.BLL.NC_tudi().Exists_hong(meid, id2))
                {
                    BCW.User.Users.IsFresh("Farmkj", 2);//防刷
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -Price, "在农场将第" + id2 + "块普通土地升级为红土地,花费了" + Price + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - Price) + "金币.", 10);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场将第" + id2 + "块普通土地升级为红土地.", 10);//消息
                    new BCW.farm.BLL.NC_tudi().update_tudi("tudi_type=2", "usid=" + meid + " and tudi=" + id2 + "");//改字段为红土地
                    //把土地的作物弄成熟--（种植时间-成熟时间）
                    new BCW.farm.BLL.NC_tudi().update_tudi("updatetime=DATEADD(MINUTE, -(zuowu_time+2), updatetime)", "usid=" + meid + " and tudi=" + id2 + "");
                    //动态记录
                    new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]升级红土地成功.");
                    Utils.Success("土地升级", "恭喜您,土地成功升级为红土地.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                else
                {
                    Utils.Success("土地升级错误", "升级出错,请重新升级.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                #endregion
            }
            else if (info == "ok3")//升级为黑土地
            {
                #region 升级为黑土地
                int id3 = Utils.ParseInt(Utils.GetRequest("tudinum", "post", 2, @"^[1-9]\d*$", "土地出错"));
                switch (id3)
                {
                    case 1:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 40)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 500000)
                                Price = 500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 2:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 41)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 600000)
                                Price = 600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 3:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 42)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 700000)
                                Price = 700000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 4:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 43)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 800000)
                                Price = 800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 5:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 44)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 900000)
                                Price = 900000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 6:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 45)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 1000000)
                                Price = 1000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 7:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 46)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2000000)
                                Price = 2000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 8:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 47)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2200000)
                                Price = 2200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 9:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 48)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2400000)
                                Price = 2400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 10:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 49)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2600000)
                                Price = 2600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 11:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 50)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 2800000)
                                Price = 2800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                        {
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        }
                        break;
                    case 12:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 51)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 3000000)
                                Price = 3000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 13:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 52)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4000000)
                                Price = 4000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 14:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 53)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4200000)
                                Price = 4200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 15:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 54)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4400000)
                                Price = 4400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 16:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 55)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4600000)
                                Price = 4600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 17:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 56)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4800000)
                                Price = 4800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 18:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 57)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5000000)
                                Price = 5000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 19:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 58)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6000000)
                                Price = 6000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 20:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 59)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6200000)
                                Price = 6200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 21:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 60)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6400000)
                                Price = 6400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 22:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 61)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6600000)
                                Price = 6600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 23:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 62)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6800000)
                                Price = 6800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 24:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 63)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 7000000)
                                Price = 7000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                }
                if (!new BCW.farm.BLL.NC_tudi().Exists_hei(meid, id3))
                {
                    BCW.User.Users.IsFresh("Farmkj", 2);//防刷
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -Price, "在农场将第" + id3 + "块红土地升级为黑土地,花费了" + Price + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - Price) + "金币.", 10);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场将第" + id3 + "块红土地升级为黑土地.", 10);//消息
                    new BCW.farm.BLL.NC_tudi().update_tudi("tudi_type=3", "usid=" + meid + " and tudi=" + id3 + "");//改字段为黑土地
                    //把土地的作物弄成熟--（种植时间-成熟时间）
                    new BCW.farm.BLL.NC_tudi().update_tudi("updatetime=DATEADD(MINUTE, -(zuowu_time+2), updatetime)", "usid=" + meid + " and tudi=" + id3 + "");
                    //动态记录
                    new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]升级黑土地成功.");
                    Utils.Success("土地升级", "恭喜您,土地成功升级为黑土地.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                else
                {
                    Utils.Success("土地升级错误", "升级出错,请重新升级.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                #endregion
            }
            else if (info == "ok4")//升级为金土地
            {
                #region 升级为金土地
                int id4 = Utils.ParseInt(Utils.GetRequest("tudinum", "post", 2, @"^[1-9]\d*$", "土地出错"));
                switch (id4)
                {
                    case 1:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 58)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4000000)
                                Price = 4000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 2:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 59)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4200000)
                                Price = 4200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 3:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 60)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4400000)
                                Price = 4400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 4:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 61)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4600000)
                                Price = 4600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 5:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 62)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 4800000)
                                Price = 4800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 6:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 63)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5000000)
                                Price = 5000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 7:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 64)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5200000)
                                Price = 5200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 8:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 65)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5400000)
                                Price = 5400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 9:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 66)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5600000)
                                Price = 5600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 10:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 67)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 5800000)
                                Price = 5800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 11:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 68)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6000000)
                                Price = 6000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                        {
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        }
                        break;
                    case 12:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 69)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6200000)
                                Price = 6200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 13:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 70)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 6600000)
                                Price = 6600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 14:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 71)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 7000000)
                                Price = 7000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 15:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 72)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 7400000)
                                Price = 7400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 16:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 73)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 7800000)
                                Price = 7800000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 17:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 74)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 8200000)
                                Price = 8200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 18:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 75)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 8600000)
                                Price = 8600000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 19:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 77)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 9500000)
                                Price = 9500000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 20:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 79)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 10400000)
                                Price = 10400000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 21:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 81)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 11300000)
                                Price = 11300000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 22:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 83)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 12200000)
                                Price = 12200000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 23:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 85)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 13100000)
                                Price = 13100000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                    case 24:
                        if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 87)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(meid) >= 14000000)
                                Price = 14000000;
                            else
                                Utils.Error("你当前金币不够,快去偷菜吧.", "");
                        }
                        else
                            Utils.Error("你当前等级不够,快去种菜升级吧.", "");
                        break;
                }
                if (!new BCW.farm.BLL.NC_tudi().Exists_jin(meid, id4))
                {
                    BCW.User.Users.IsFresh("Farmkj", 2);//防刷
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -Price, "在农场将第" + id4 + "块黑土地升级为金土地,花费了" + Price + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - Price) + "金币.", 10);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场将第" + id4 + "块黑土地升级为金土地.", 10);//消息
                    new BCW.farm.BLL.NC_tudi().update_tudi("tudi_type=4", "usid=" + meid + " and tudi=" + id4 + "");//改字段为金土地
                    //把土地的作物弄成熟--（种植时间-成熟时间）
                    new BCW.farm.BLL.NC_tudi().update_tudi("updatetime=DATEADD(MINUTE, -(zuowu_time+2), updatetime)", "usid=" + meid + " and tudi=" + id4 + "");
                    //动态记录
                    new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]升级金土地成功.");
                    Utils.Success("土地升级", "恭喜您,土地成功升级为金土地.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                else
                {
                    Utils.Success("土地升级错误", "升级出错,请重新升级.", Utils.getUrl("farm.aspx?act=gongnegn"), "1");
                }
                #endregion
            }
            else if (info == "ok22")//开启红土地模式
            {
                BCW.User.Users.IsFresh("Farmkq", 2);//防刷
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -10000, "在农场开启了红土地模式,花费了10000金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - 10000) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场开启了红土地模式.", 10);//消息
                new BCW.farm.BLL.NC_user().Update_tdlx(meid, 2);//改为红土地
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]把土地全部扩建完成,进入了红土地模式.");
                Utils.Success("开启模式", "恭喜您,土地成为红土地模式.", Utils.getUrl("farm.aspx?act=gongnegn"), "3");
            }
            else if (info == "ok33")//开启黑土地模式
            {
                BCW.User.Users.IsFresh("Farmkq", 2);//防刷
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -20000, "在农场开启了黑土地模式,花费了20000金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - 20000) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场开启了黑土地模式.", 10);//消息
                new BCW.farm.BLL.NC_user().Update_tdlx(meid, 3);//改为黑土地
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]把土地全部升级为红土地,进入了黑土地模式.");
                Utils.Success("开启模式", "恭喜您,土地成为黑土地模式.", Utils.getUrl("farm.aspx?act=gongnegn"), "3");
            }
            else if (info == "ok44")//开启金土地模式
            {
                BCW.User.Users.IsFresh("Farmkq", 2);//防刷
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -30000, "在农场开启了金土地模式,花费了30000金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - 30000) + "金币.", 10);
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在农场开启了金土地模式.", 10);//消息
                new BCW.farm.BLL.NC_user().Update_tdlx(meid, 4);//改为金土地
                //动态记录
                new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx]" + GameName + "[/url]把土地全部升级为黑土地,进入了金土地模式.");
                Utils.Success("开启模式", "恭喜您,土地成为金土地模式.", Utils.getUrl("farm.aspx?act=gongnegn"), "3");
            }
            else
            {
                if (b.tuditpye == 1)//如果土地类型是1==普通土地--扩建提示
                {
                    #region 普通土地扩建提示
                    if (a.aa < 24)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("你目前有：" + a.aa + "块普通土地.<br/>");
                        builder.Append(Out.Tab("</div>", ""));
                        builder.Append(Out.Tab("<div>", ""));
                        if (a.aa == 6)
                        {
                            tudinum = 7;
                            builder.Append("扩建第7块土地需要5级和10000金币.确定扩建吗？");
                        }
                        else if (a.aa == 7)
                        {
                            tudinum = 8;
                            builder.Append("扩建第8块土地需要7级和20000金币.确定扩建吗？");
                        }
                        else if (a.aa == 8)
                        {
                            tudinum = 9;
                            builder.Append("扩建第9块土地需要9级和30000金币.确定扩建吗？");
                        }
                        else if (a.aa == 9)
                        {
                            tudinum = 10;
                            builder.Append("扩建第10块土地需要11级和50000金币.确定扩建吗？");
                        }
                        else if (a.aa == 10)
                        {
                            tudinum = 11;
                            builder.Append("扩建第11块土地需要13级和70000金币.确定扩建吗？");
                        }
                        else if (a.aa == 11)
                        {
                            tudinum = 12;
                            builder.Append("扩建第12块土地需要15级和90000金币.确定扩建吗？");
                        }
                        else if (a.aa == 12)
                        {
                            tudinum = 13;
                            builder.Append("扩建第13块土地需要17级和120000金币.确定扩建吗？");
                        }
                        else if (a.aa == 13)
                        {
                            tudinum = 14;
                            builder.Append("扩建第14块土地需要19级和150000金币.确定扩建吗？");
                        }
                        else if (a.aa == 14)
                        {
                            tudinum = 15;
                            builder.Append("扩建第15块土地需要21级和180000金币.确定扩建吗？");
                        }
                        else if (a.aa == 15)
                        {
                            tudinum = 16;
                            builder.Append("扩建第16块土地需要23级和230000金币.确定扩建吗？");
                        }
                        else if (a.aa == 16)
                        {
                            tudinum = 17;
                            builder.Append("扩建第17块土地需要25级和300000金币.确定扩建吗？");
                        }
                        else if (a.aa == 17)
                        {
                            tudinum = 18;
                            builder.Append("扩建第18块土地需要27级和500000金币.确定扩建吗？");
                        }
                        else if (a.aa == 18)
                        {
                            tudinum = 19;
                            builder.Append("扩建第19块土地需要29级和850000金币.确定扩建吗？");
                        }
                        else if (a.aa == 19)
                        {
                            tudinum = 20;
                            builder.Append("扩建第20块土地需要31级和1100000金币.确定扩建吗？");
                        }
                        else if (a.aa == 20)
                        {
                            tudinum = 21;
                            builder.Append("扩建第21块土地需要33级和1300000金币.确定扩建吗？");
                        }
                        else if (a.aa == 21)
                        {
                            tudinum = 22;
                            builder.Append("扩建第22块土地需要35级和1500000金币.确定扩建吗？");
                        }
                        else if (a.aa == 22)
                        {
                            tudinum = 23;
                            builder.Append("扩建第23块土地需要37级和1700000金币.确定扩建吗？");
                        }
                        else if (a.aa == 23)
                        {
                            tudinum = 24;
                            builder.Append("扩建第24块土地需要39级和2000000金币.确定扩建吗？");
                        }
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,,";
                        string strName = "tudinum,act,info";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + tudinum + "'gongnegn'ok1";
                        string strEmpt = "false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定扩建,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    if (a.aa == 24)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("您已成功把所有土地扩建完毕(共24块土地).<br/>");
                        builder.Append("是否开启红土地模式？确定需花费10000金币.<br/>(种子种在红土地上,增产10%,经验值不变.)");
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,";
                        string strName = "act,info";
                        string strType = "hidden,hidden";
                        string strValu = "gongnegn'ok22";
                        string strEmpt = "false,false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定开启,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    #endregion
                }
                else if (b.tuditpye == 2)//如果土地类型是2==红土地--扩建提示
                {
                    #region 红土地扩建提示
                    BCW.farm.Model.NC_tudi c = new BCW.farm.BLL.NC_tudi().Get_htd(meid);//查询id有几块红土地
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("你目前有：" + c.aa + "块红土地.<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    if (c.aa < 24)
                    {
                        if (c.aa == 0)
                        {
                            tudinum = 1;
                            builder.Append("升级第1块红土地需要28级和200000金币.确定升级吗？");
                        }
                        else if (c.aa == 1)
                        {
                            tudinum = 2;
                            builder.Append("升级第2块红土地需要29级和220000金币.确定升级吗？");
                        }
                        else if (c.aa == 2)
                        {
                            tudinum = 3;
                            builder.Append("升级第3块红土地需要30级和240000金币.确定升级吗？");
                        }
                        else if (c.aa == 3)
                        {
                            tudinum = 4;
                            builder.Append("升级第4块红土地需要31级和260000金币.确定升级吗？");
                        }
                        else if (c.aa == 4)
                        {
                            tudinum = 5;
                            builder.Append("升级第5块红土地需要32级和290000金币.确定升级吗？");
                        }
                        else if (c.aa == 5)
                        {
                            tudinum = 6;
                            builder.Append("升级第6块红土地需要33级和320000金币.确定升级吗？");
                        }
                        else if (c.aa == 6)
                        {
                            tudinum = 7;
                            builder.Append("升级第7块红土地需要34级和350000金币.确定升级吗？");
                        }
                        else if (c.aa == 7)
                        {
                            tudinum = 8;
                            builder.Append("升级第8块红土地需要35级和380000金币.确定升级吗？");
                        }
                        else if (c.aa == 8)
                        {
                            tudinum = 9;
                            builder.Append("升级第9块红土地需要36级和410000金币.确定升级吗？");
                        }
                        else if (c.aa == 9)
                        {
                            tudinum = 10;
                            builder.Append("升级第10块红土地需要37级和440000金币.确定升级吗？");
                        }
                        else if (c.aa == 10)
                        {
                            tudinum = 11;
                            builder.Append("升级第11块红土地需要38级和480000金币.确定升级吗？");
                        }
                        else if (c.aa == 11)
                        {
                            tudinum = 12;
                            builder.Append("升级第12块红土地需要39级和520000金币.确定升级吗？");
                        }
                        else if (c.aa == 12)
                        {
                            tudinum = 13;
                            builder.Append("升级第13块红土地需要40级和560000金币.确定升级吗？");
                        }
                        else if (c.aa == 13)
                        {
                            tudinum = 14;
                            builder.Append("升级第14块红土地需要41级和600000金币.确定升级吗？");
                        }
                        else if (c.aa == 14)
                        {
                            tudinum = 15;
                            builder.Append("升级第15块红土地需要42级和650000金币.确定升级吗？");
                        }
                        else if (c.aa == 15)
                        {
                            tudinum = 16;
                            builder.Append("升级第16块红土地需要43级和700000金币.确定升级吗？");
                        }
                        else if (c.aa == 16)
                        {
                            tudinum = 17;
                            builder.Append("升级第17块红土地需要44级和770000金币.确定升级吗？");
                        }
                        else if (c.aa == 17)
                        {
                            tudinum = 18;
                            builder.Append("升级第18块红土地需要45级和900000金币.确定升级吗？");
                        }
                        else if (c.aa == 18)
                        {
                            tudinum = 19;
                            builder.Append("升级第19块红土地需要47级和1500000金币.确定升级吗？");
                        }
                        else if (c.aa == 19)
                        {
                            tudinum = 20;
                            builder.Append("升级第20块红土地需要49级和2000000金币.确定升级吗？");
                        }
                        else if (c.aa == 20)
                        {
                            tudinum = 21;
                            builder.Append("升级第21块红土地需要51级和3000000金币.确定升级吗？");
                        }
                        else if (c.aa == 21)
                        {
                            tudinum = 22;
                            builder.Append("升级第22块红土地需要53级和4000000金币.确定升级吗？");
                        }
                        else if (c.aa == 22)
                        {
                            tudinum = 23;
                            builder.Append("升级第23块红土地需要55级和5500000金币.确定升级吗？");
                        }
                        else if (c.aa == 23)
                        {
                            tudinum = 24;
                            builder.Append("升级第24块红土地需要57级和6800000金币.确定升级吗？");
                        }
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,,";
                        string strName = "tudinum,act,info";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + tudinum + "'gongnegn'ok2";
                        string strEmpt = "false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定升级,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    if (c.aa == 24)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("您已成功把所有土地升级为红土地(共24块红土地).<br/>");
                        builder.Append("是否开启黑土地模式？确定需花费20000金币.<br/>(种子种在黑土地上,增产20%,并且减少10%的成熟时间和增加10%的作物经验.)");
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,";
                        string strName = "act,info";
                        string strType = "hidden,hidden";
                        string strValu = "gongnegn'ok33";
                        string strEmpt = "false,false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定开启,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    #endregion
                }
                else if (b.tuditpye == 3)//如果土地类型是3==黑土地--扩建提示
                {
                    #region 黑土地扩建提示
                    BCW.farm.Model.NC_tudi d = new BCW.farm.BLL.NC_tudi().Get_heitd(meid);//查询id有几块黑土地
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("你目前有：" + d.aa + "块黑土地.<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    if (d.aa < 24)
                    {
                        if (d.aa == 0)
                        {
                            tudinum = 1;
                            builder.Append("升级第1块黑土地需要40级和500000金币.确定升级吗？");
                        }
                        else if (d.aa == 1)
                        {
                            tudinum = 2;
                            builder.Append("升级第2块黑土地需要41级和600000金币.确定升级吗？");
                        }
                        else if (d.aa == 2)
                        {
                            tudinum = 3;
                            builder.Append("升级第3块黑土地需要42级和700000金币.确定升级吗？");
                        }
                        else if (d.aa == 3)
                        {
                            tudinum = 4;
                            builder.Append("升级第4块黑土地需要43级和800000金币.确定升级吗？");
                        }
                        else if (d.aa == 4)
                        {
                            tudinum = 5;
                            builder.Append("升级第5块黑土地需要44级和900000金币.确定升级吗？");
                        }
                        else if (d.aa == 5)
                        {
                            tudinum = 6;
                            builder.Append("升级第6块黑土地需要45级和1000000金币.确定升级吗？");
                        }
                        else if (d.aa == 6)
                        {
                            tudinum = 7;
                            builder.Append("升级第7块黑土地需要46级和2000000金币.确定升级吗？");
                        }
                        else if (d.aa == 7)
                        {
                            tudinum = 8;
                            builder.Append("升级第8块黑土地需要47级和2200000金币.确定升级吗？");
                        }
                        else if (d.aa == 8)
                        {
                            tudinum = 9;
                            builder.Append("升级第9块黑土地需要48级和2400000金币.确定升级吗？");
                        }
                        else if (d.aa == 9)
                        {
                            tudinum = 10;
                            builder.Append("升级第10块黑土地需要49级和2600000金币.确定升级吗？");
                        }
                        else if (d.aa == 10)
                        {
                            tudinum = 11;
                            builder.Append("升级第11块黑土地需要50级和2800000金币.确定升级吗？");
                        }
                        else if (d.aa == 11)
                        {
                            tudinum = 12;
                            builder.Append("升级第12块黑土地需要51级和3000000金币.确定升级吗？");
                        }
                        else if (d.aa == 12)
                        {
                            tudinum = 13;
                            builder.Append("升级第13块黑土地需要52级和4000000金币.确定升级吗？");
                        }
                        else if (d.aa == 13)
                        {
                            tudinum = 14;
                            builder.Append("升级第14块黑土地需要53级和4200000金币.确定升级吗？");
                        }
                        else if (d.aa == 14)
                        {
                            tudinum = 15;
                            builder.Append("升级第15块黑土地需要54级和4400000金币.确定升级吗？");
                        }
                        else if (d.aa == 15)
                        {
                            tudinum = 16;
                            builder.Append("升级第16块黑土地需要55级和4600000金币.确定升级吗？");
                        }
                        else if (d.aa == 16)
                        {
                            tudinum = 17;
                            builder.Append("升级第17块黑土地需要56级和4800000金币.确定升级吗？");
                        }
                        else if (d.aa == 17)
                        {
                            tudinum = 18;
                            builder.Append("升级第18块黑土地需要57级和5000000金币.确定升级吗？");
                        }
                        else if (d.aa == 18)
                        {
                            tudinum = 19;
                            builder.Append("升级第19块黑土地需要58级和6000000金币.确定升级吗？");
                        }
                        else if (d.aa == 19)
                        {
                            tudinum = 20;
                            builder.Append("升级第20块黑土地需要59级和6200000金币.确定升级吗？");
                        }
                        else if (d.aa == 20)
                        {
                            tudinum = 21;
                            builder.Append("升级第21块黑土地需要60级和6400000金币.确定升级吗？");
                        }
                        else if (d.aa == 21)
                        {
                            tudinum = 22;
                            builder.Append("升级第22块黑土地需要61级和6600000金币.确定升级吗？");
                        }
                        else if (d.aa == 22)
                        {
                            tudinum = 23;
                            builder.Append("升级第23块黑土地需要62级和6800000金币.确定升级吗？");
                        }
                        else if (d.aa == 23)
                        {
                            tudinum = 24;
                            builder.Append("升级第24块黑土地需要63级和7000000金币.确定升级吗？");
                        }
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,,";
                        string strName = "tudinum,act,info";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + tudinum + "'gongnegn'ok3";
                        string strEmpt = "false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定升级,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    if (d.aa == 24)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("您已成功把所有土地升级为黑土地(共24块黑土地).<br/>");
                        builder.Append("是否开启金土地模式？确定需花费30000金币.<br/>(种子种在金土地上,增产30%,并且减少20%的成熟时间和增加15%的作物经验.)");
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,";
                        string strName = "act,info";
                        string strType = "hidden,hidden";
                        string strValu = "gongnegn'ok44";
                        string strEmpt = "false,false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定开启,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    #endregion
                }
                else if (b.tuditpye == 4)//如果土地类型是4==金土地--扩建提示
                {
                    #region 金土地扩建提示
                    BCW.farm.Model.NC_tudi e = new BCW.farm.BLL.NC_tudi().Get_jtd(meid);//查询id有几块黑土地
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                    builder.Append("你目前有：" + e.aa + "块金土地.<br/>");
                    builder.Append(Out.Tab("</div>", ""));
                    builder.Append(Out.Tab("<div>", ""));
                    if (e.aa < 24)
                    {
                        if (e.aa == 0)
                        {
                            tudinum = 1;
                            builder.Append("升级第1块金土地需要58级和4000000金币.确定升级吗？");
                        }
                        else if (e.aa == 1)
                        {
                            tudinum = 2;
                            builder.Append("升级第2块金土地需要59级和4200000金币.确定升级吗？");
                        }
                        else if (e.aa == 2)
                        {
                            tudinum = 3;
                            builder.Append("升级第3块金土地需要60级和4400000金币.确定升级吗？");
                        }
                        else if (e.aa == 3)
                        {
                            tudinum = 4;
                            builder.Append("升级第4块金土地需要61级和4600000金币.确定升级吗？");
                        }
                        else if (e.aa == 4)
                        {
                            tudinum = 5;
                            builder.Append("升级第5块金土地需要62级和4800000金币.确定升级吗？");
                        }
                        else if (e.aa == 5)
                        {
                            tudinum = 6;
                            builder.Append("升级第6块金土地需要63级和5000000金币.确定升级吗？");
                        }
                        else if (e.aa == 6)
                        {
                            tudinum = 7;
                            builder.Append("升级第7块金土地需要64级和5200000金币.确定升级吗？");
                        }
                        else if (e.aa == 7)
                        {
                            tudinum = 8;
                            builder.Append("升级第8块金土地需要65级和5400000金币.确定升级吗？");
                        }
                        else if (e.aa == 8)
                        {
                            tudinum = 9;
                            builder.Append("升级第9块金土地需要66级和5600000金币.确定升级吗？");
                        }
                        else if (e.aa == 9)
                        {
                            tudinum = 10;
                            builder.Append("升级第10块金土地需要67级和5800000金币.确定升级吗？");
                        }
                        else if (e.aa == 10)
                        {
                            tudinum = 11;
                            builder.Append("升级第11块金土地需要68级和6000000金币.确定升级吗？");
                        }
                        else if (e.aa == 11)
                        {
                            tudinum = 12;
                            builder.Append("升级第12块金土地需要69级和6200000金币.确定升级吗？");
                        }
                        else if (e.aa == 12)
                        {
                            tudinum = 13;
                            builder.Append("升级第13块金土地需要70级和6600000金币.确定升级吗？");
                        }
                        else if (e.aa == 13)
                        {
                            tudinum = 14;
                            builder.Append("升级第14块金土地需要71级和7000000金币.确定升级吗？");
                        }
                        else if (e.aa == 14)
                        {
                            tudinum = 15;
                            builder.Append("升级第15块金土地需要72级和7400000金币.确定升级吗？");
                        }
                        else if (e.aa == 15)
                        {
                            tudinum = 16;
                            builder.Append("升级第16块金土地需要73级和7800000金币.确定升级吗？");
                        }
                        else if (e.aa == 16)
                        {
                            tudinum = 17;
                            builder.Append("升级第17块金土地需要74级和8200000金币.确定升级吗？");
                        }
                        else if (e.aa == 17)
                        {
                            tudinum = 18;
                            builder.Append("升级第18块金土地需要75级和8600000金币.确定升级吗？");
                        }
                        else if (e.aa == 18)
                        {
                            tudinum = 19;
                            builder.Append("升级第19块金土地需要77级和9500000金币.确定升级吗？");
                        }
                        else if (e.aa == 19)
                        {
                            tudinum = 20;
                            builder.Append("升级第20块金土地需要79级和10400000金币.确定升级吗？");
                        }
                        else if (e.aa == 20)
                        {
                            tudinum = 21;
                            builder.Append("升级第21块金土地需要81级和11300000金币.确定升级吗？");
                        }
                        else if (e.aa == 21)
                        {
                            tudinum = 22;
                            builder.Append("升级第22块金土地需要83级和12200000金币.确定升级吗？");
                        }
                        else if (e.aa == 22)
                        {
                            tudinum = 23;
                            builder.Append("升级第23块金土地需要85级和13100000金币.确定升级吗？");
                        }
                        else if (e.aa == 23)
                        {
                            tudinum = 24;
                            builder.Append("升级第24块金土地需要87级和14000000金币.确定升级吗？");
                        }
                        builder.Append(Out.Tab("</div>", ""));

                        string strText = ",,,";
                        string strName = "tudinum,act,info";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + tudinum + "'gongnegn'ok4";
                        string strEmpt = "false,false,false";
                        string strIdea = "/";
                        string strOthe = "确定升级,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    if (e.aa == 24)
                    {
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                        builder.Append("您已成功把所有土地升级为金土地(共24块金土地).<br/>敬请期待更好玩的土地.");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    #endregion
                }
                else
                {
                    Utils.Error("抱歉,你的土地类型出错,请联系客服.", "");
                }
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            builder.Append("你目前的等级:" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "级 ");
            builder.Append("金币:" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "");
            builder.Append(Out.Tab("</div>", "<br/>"));

            builder.Append(Out.Tab("<div>", ""));
            if (new BCW.farm.BLL.NC_user().Getshou(meid) == 0)
                builder.Append("一键收获：需要等级" + shouhuo1_grade + "级," + Utils.ConvertGold(shouhuo1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=4&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a><br/>");
            else
                builder.Append("一键收获：已开通<br/>");
            if (new BCW.farm.BLL.NC_user().Getchandi(meid) == 0)
                builder.Append("一键耕地：需要等级" + chandi1_grade + "级," + Utils.ConvertGold(chandi1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=5&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a><br/>");
            else
                builder.Append("一键耕地：已开通<br/>");
            if (new BCW.farm.BLL.NC_user().Getshifei(meid) == 0)
                builder.Append("一键施肥：需要等级" + shifei1_grade + "级," + Utils.ConvertGold(shifei1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=6&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a><br/>");
            else
                builder.Append("一键施肥：已开通<br/>");
            if (new BCW.farm.BLL.NC_user().Getchucao(meid) == 0)
                builder.Append("一键除草：需要等级" + chucao1_grade + "级," + Utils.ConvertGold(chucao1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=1&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a><br/>");
            else
                builder.Append("一键除草：已开通<br/>");
            if (new BCW.farm.BLL.NC_user().Getchuchong(meid) == 0)
                builder.Append("一键杀虫：需要等级" + chuchong1_grade + "级," + Utils.ConvertGold(chuchong1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=3&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a><br/>");
            else
                builder.Append("一键杀虫：已开通<br/>");
            if (new BCW.farm.BLL.NC_user().Getjiaoshui(meid) == 0)
                builder.Append("一键浇水：需要等级" + jiaoshui1_grade + "级," + Utils.ConvertGold(jiaoshui1_jinbi) + "金币.<a href=\"" + Utils.getUrl("farm.aspx?act=kaitong&amp;ptype=2&amp;usid=" + meid + "&amp;p=1") + "\">[开通]</a>");
            else
                builder.Append("一键浇水：已开通");
            builder.Append(Out.Tab("</div>", ""));
        }

        if (ptype == 1)
        {
            builder.Append(Out.Tab("<div>", "<br/>"));
            builder.Append("农场土地说明:<br/>1.每个会员开始会有6块土地,最多只能扩建到24块.<br/>2.扩建到24块土地后,才可以逐一把土地升级为红土地,作物种在红土地会增产10%.<br/>");
            builder.Append("3.只有把24块土地升级为红土地,才可以开始升级黑土地,作物种在黑土地会增产20%并且减少10%的成熟时间和增加10%的作物经验.<br/>");
            builder.Append("4.只有把24块土地升级为黑土地,才可以开始升级金土地,作物种在金土地会增产30%并且减少20%的成熟时间和增加15%的作物经验.");
            builder.Append(Out.Tab("</div>", ""));
        }

        foot_link();//底部链接

        foot_link2();
    }

    //偷菜界面
    private void ToucaiPage()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //如果金币不足，则不可以偷菜
        if (new BCW.farm.BLL.NC_user().GetGold(meid) < 0)
        {
            Utils.Error("抱歉,你的金币为负值,暂不能偷菜.去<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">仓库</a>卖出东西赚钱吧.", "");
        }

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        int uid = int.Parse(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        //int j = int.Parse(Utils.GetRequest("j", "all", 1, @"^[0-9]$", "1"));

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;偷菜");
        }
        else if (ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;可操作");
        }
        //else if (ptype == 2)
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;除草");
        //}
        //else if (ptype == 3)
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;害虫");
        //}
        //else if (ptype == 4)
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;浇水");
        //}
        //else if (ptype == 5)
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;种草/放虫");
        //}
        else if (ptype == 3)
        {
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;农场好友");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            Master.Title = "偷菜";
            builder.Append("<h style=\"color:red\">偷菜" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=1") + "\">偷菜</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "可操作";
            builder.Append("<h style=\"color:red\">可操作" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=2") + "\">可操作</a>" + "|");
        }
        //if (ptype == 2)
        //{
        //    Master.Title = "除草";
        //    builder.Append("<h style=\"color:red\">除草" + "</h>" + "|");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=2") + "\">除草</a>" + "|");
        //}
        //if (ptype == 3)
        //{
        //    Master.Title = "害虫";
        //    builder.Append("<h style=\"color:red\">害虫" + "</h>" + "|");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=3") + "\">害虫</a>" + "|");
        //}
        //if (ptype == 4)
        //{
        //    Master.Title = "浇水";
        //    builder.Append("<h style=\"color:red\">浇水" + "</h>" + "|");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=4") + "\">浇水</a>" + "|");
        //}
        //if (ptype == 5)
        //{
        //    Master.Title = "种草/放虫";
        //    builder.Append("<h style=\"color:red\">种草/放虫" + "</h>" + "|");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=5") + "\">种草/放虫</a>" + "|");
        //}
        if (ptype == 3)
        {
            Master.Title = "农场好友";
            builder.Append("<h style=\"color:red\">农场好友" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=3") + "\">农场好友</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));


        int pageIndex;
        int recordCount;//数量
        //string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "page1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));

        #region 偷菜 1
        if (ptype == 1)
        {
            if (uid == 0)
            {
                //邵广林 20160825 修改超好友不可以偷
                //AND ((((SELECT SUM (len(touID) - len(replace(touID, ',', '')))FROM tb_NC_tudi WHERE touID LIKE '%,%'AND ID = a.id)-1) < 5) or touID='')
                //旧DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 AND c.UsID=" + meid + " AND c.Types=0");
                DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' AND ((((SELECT SUM (len(touID) - len(replace(touID, ',', '')))FROM tb_NC_tudi WHERE touID LIKE '%,%'AND ID = a.id)-1) < " + tou_renshu + ") or touID='') and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 AND c.UsID=" + meid + " AND c.Types=0");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    string name = string.Empty;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int _UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                        if (((new BCW.BLL.Friend().Exists(meid, _UsID, 0)) == true) && ((new BCW.BLL.Friend().Exists(_UsID, meid, 0)) == true))
                        {
                            count++;
                            name = name + _UsID + ",";
                        }
                    }
                    if (count > 0)
                    {
                        recordCount = count;
                        int k = 1;
                        int koo = (pageIndex - 1) * pageSize;
                        int skt = pageSize;
                        if (recordCount > koo + pageSize)
                        {
                            skt = pageSize;
                        }
                        else
                        {
                            skt = recordCount - koo;
                        }
                        for (int i = koo; i < skt + koo; i++)
                        {
                            int UsID = int.Parse(name.Split(',')[i]);
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", ""));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }

                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID));
                            builder.Append(":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "&amp;ptype=" + ptype + "") + "\">(偷)</a>");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "暂时没有好友的菜可偷哦!<br/>"));
                        builder.Append(Out.Tab("<div>", ""));
                        string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                        builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请好友一起来玩" + GameName + ">></a> ");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                #region  20160825 邵广林 修改偷菜方式旧
                //if (ds != null && ds.Tables[0].Rows.Count > 0)
                //{
                //    recordCount = ds.Tables[0].Rows.Count;
                //    int k = 1;
                //    int koo = (pageIndex - 1) * pageSize;
                //    int skt = pageSize;
                //    if (recordCount > koo + pageSize)
                //    {
                //        skt = pageSize;
                //    }
                //    else
                //    {
                //        skt = recordCount - koo;
                //    }
                //    for (int i = 0; i < skt; i++)
                //    {
                //        int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                //        BCW.farm.Model.NC_tudi ty = new BCW.farm.BLL.NC_tudi().tou_tudinum2(UsID, meid);//查询已偷过的土地块数
                //        BCW.farm.Model.NC_tudi tp = new BCW.farm.BLL.NC_tudi().tou_tudinum1(UsID);//查询有几块土地可以偷取
                //        if (UsID != meid)//如果不等于自己的id
                //        {
                //            //if (new BCW.BLL.Friend().Exists(meid, UsID, 0))//判断是否为好友
                //            {
                //                //邵广林 20150519 排版去掉底色
                //                if (j % 2 == 0)
                //                    builder.Append(Out.Tab("<div>", "<br />"));
                //                else
                //                {
                //                    if (j == 1)
                //                        builder.Append(Out.Tab("<div>", ""));
                //                    else
                //                        builder.Append(Out.Tab("<div>", "<br />"));
                //                }
                //                //if (ty.aa < tp.aa)
                //                int yy = 0;
                //                DataSet tt = new BCW.farm.BLL.NC_tudi().GetList("touID", "usid=" + UsID + " AND updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
                //                if (tt != null && tt.Tables[0].Rows.Count > 0)
                //                {
                //                    for (int ii = 0; ii < tt.Tables[0].Rows.Count; ii++)
                //                    {
                //                        int db = 0;
                //                        string touID = (tt.Tables[0].Rows[ii]["touID"].ToString());
                //                        string[] ttt = touID.Split(',');//拆分id
                //                        //得到偷的人数
                //                        string[] sNum = Regex.Split(touID, ",");
                //                        int bb = sNum.Length;
                //                        if (bb <= tou_renshu + 2)//设定的人数
                //                        {
                //                            for (int bd = 0; bd < ttt.Length; bd++)
                //                            {
                //                                if (ttt[bd] == UsID.ToString())
                //                                {
                //                                    db++;
                //                                }
                //                            }
                //                        }
                //                        else
                //                        {
                //                            db++;
                //                        }
                //                        if (db == 0)//如果等于0，即是存在可以偷的土地
                //                        {
                //                            yy = 1;
                //                            break;
                //                        }
                //                    }
                //                }
                //                if (yy == 1)//有土地存在可以偷
                //                {
                //                    builder.Append("" + j + "." + new BCW.BLL.User().GetUsName(UsID));
                //                    builder.Append(":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "&amp;ptype=" + ptype + "") + "\">(偷)</a>");
                //                    j++;
                //                }
                //                k++;
                //                builder.Append(Out.Tab("</div>", ""));
                //            }
                //        }
                //    }
                //    builder.Append(BasePage.MultiPage(pageIndex, pageSize, 1, Utils.getPageUrl(), pageValUrl, "page", 0));
                //}
                #endregion
                else
                {
                    builder.Append(Out.Div("div", "暂时没有好友的菜可偷哦!<br/>"));
                    builder.Append(Out.Tab("<div>", ""));
                    string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                    builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请好友一起来玩" + GameName + ">></a> ");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
            {
                DataSet ds = new BCW.farm.BLL.NC_user().GetList("*", "usid=" + uid + "");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    recordCount = ds.Tables[0].Rows.Count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = 0; i < skt; i++)
                    {
                        int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["usid"].ToString());
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }

                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID));
                        builder.Append(":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">(去Ta农场看看)</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    if (!new BCW.BLL.User().Exists(uid))
                    {
                        builder.Append(Out.Tab("<div>", ""));
                        builder.Append("请输入正确ID!");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    else
                    {
                        builder.Append(Out.Div("div", "该好友(ID" + uid + ")暂未开通农场.<a href=\"" + Utils.getUrl("farm.aspx?act=yaoqing&amp;uid=" + uid + "") + "\">快速邀请Ta.</a><br/>"));
                        builder.Append(Out.Tab("<div>", ""));
                        string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                        builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
            }

            string strText = "输入用户ID：/,,,";
            string strName = "usid,act,ptype,backurl";
            string strType = "num,hidden,hidden,hidden";
            string strValu = string.Empty;
            if (uid == 0)
                strValu = "'toucai'1'" + Utils.getPage(0) + "";
            else
                strValu = "" + uid + "'toucai'1'" + Utils.getPage(0) + "";
            string strEmpt = "true,false,false,false";
            string strIdea = "/";
            string strOthe = "搜农场好友,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        #endregion

        #region 可操作 2
        if (ptype == 2)
        {
            //除草
            //DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' and iscao=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) AND c.UsID=" + meid + "");
            //20160820 邵广林
            //DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' AND ((caoID!=" + meid + " and caoID='') or (chongID!=" + meid + " AND chongID='')) and (iscao=1 or isinsect = 1 or iswater = 1 or zuowu!= '') and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1  AND c.UsID=" + meid + " AND c.Types=0");
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid != " + meid + " AND c.UsID = " + meid + " AND c.Types = 0 AND ((((caoID = '') OR(chongID = '')) and (updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1 AND zuowu!='')) OR ((iscao = 1 AND caoID != " + meid + ") OR (isinsect = 1 AND chongID != " + meid + ") OR iswater = 1))");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int count = 0;
                string name = string.Empty;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int _UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                    if (((new BCW.BLL.Friend().Exists(meid, _UsID, 0)) == true) && ((new BCW.BLL.Friend().Exists(_UsID, meid, 0)) == true))
                    {
                        count++;
                        name = name + _UsID + ",";
                    }
                }
                if (count > 0)
                {
                    recordCount = count;
                    int k = 1;
                    int koo = (pageIndex - 1) * pageSize;
                    int skt = pageSize;
                    if (recordCount > koo + pageSize)
                    {
                        skt = pageSize;
                    }
                    else
                    {
                        skt = recordCount - koo;
                    }
                    for (int i = koo; i < skt + koo; i++)
                    {
                        int UsID = int.Parse(name.Split(',')[i]);
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "&amp;ptype=" + ptype + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                    builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                }
                else
                {
                    builder.Append(Out.Div("div", "暂时没有可操作的好友哦!<br/>"));
                    builder.Append(Out.Tab("<div>", ""));
                    string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                    builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                    builder.Append(Out.Tab("</div>", ""));
                }
                #region 20160825 邵广林 旧的查询
                //recordCount = ds.Tables[0].Rows.Count;
                //int k = 1;
                //int koo = (pageIndex - 1) * pageSize;
                //int skt = pageSize;
                //if (recordCount > koo + pageSize)
                //{
                //    skt = pageSize;
                //}
                //else
                //{
                //    skt = recordCount - koo;
                //}
                //for (int i = 0; i < skt; i++)
                //{
                //    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                //    if (UsID != meid)//如果不等于自己的id
                //    {
                //        if (k % 2 == 0)
                //            builder.Append(Out.Tab("<div>", "<br />"));
                //        else
                //        {
                //            if (k == 1)
                //                builder.Append(Out.Tab("<div>", ""));
                //            else
                //                builder.Append(Out.Tab("<div>", "<br />"));
                //        }
                //        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "&amp;ptype=" + ptype + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>");
                //        k++;
                //        builder.Append(Out.Tab("</div>", ""));
                //    }
                //}
                //builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                #endregion
            }
            else
            {
                builder.Append(Out.Div("div", "暂时没有可操作的好友哦!<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 害虫 6
        if (ptype == 6)
        {
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' and isinsect=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) AND c.UsID=" + meid + "");
            recordCount = ds.Tables[0].Rows.Count;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                    if (UsID != meid)//如果不等于自己的id
                    {
                        //if (new BCW.BLL.Friend().Exists(meid, UsID, 0))//判断是否为好友
                        {
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", ""));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">(杀虫)</a>");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂时没有好友的菜可杀虫哦!<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 浇水 4
        if (ptype == 4)
        {
            //DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(usid)", "iswater=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE())");
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' and iswater=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) AND c.UsID=" + meid + "");
            recordCount = ds.Tables[0].Rows.Count;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                    if (UsID != meid)//如果不等于自己的id
                    {
                        //if (new BCW.BLL.Friend().Exists(meid, UsID, 0))//判断是否为好友
                        {
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div>", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", ""));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">(浇水)</a>");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂时没有好友的菜可浇水哦!<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 种草放虫 5
        if (ptype == 5)
        {
            //DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(usid)", "zuowu!='' and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) ");
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND touID not LIKE '%," + meid + ",%' and zuowu!='' and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) AND c.UsID=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                    if (UsID != meid)//如果不等于自己的id
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + new BCW.BLL.User().GetUsName(UsID) + ":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">(种草/放虫)</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂时没有好友的菜可种草/放虫哦!<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        #region 农场好友 3
        if (ptype == 3)
        {
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "a.usid!=" + meid + " AND c.UsID=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "&amp;ptype=" + ptype + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂时没有农场好友哦!<br/>"));
                builder.Append(Out.Tab("<div>", ""));
                string BackUrl = Server.UrlEncode("/bbs/game/farm.aspx?act=toucai");
                builder.Append("<a href=\"" + Utils.getUrl("/inter.aspx?act=recom&amp;backurl=" + BackUrl + "") + "\">邀请我的其他好友>></a> ");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        #endregion

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">每日任务>></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=allsee") + "\">到处看看>></a>");
        builder.Append(Out.Tab("</div>", ""));

        if (ptype == 1)
        {
            int pageIndex1;
            int recordCount1;//数量
            string strWhere1 = " ";
            string[] pageValUrl1 = { "act", "ptype", "page", "backurl" };
            pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
            if (pageIndex1 == 0)
                pageIndex1 = 1;
            int pageSize1 = 5;// Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath))//分页条数

            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("被偷记录：<br/>");
            builder.Append(Out.Tab("</div>", ""));
            strWhere1 = "UsId=" + meid + " AND type=9 AND AcText LIKE '%来农场摘取%'";//查询前50条
            IList<BCW.farm.Model.NC_messagelog> listFarm1 = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex1, pageSize1, strWhere1, out recordCount1);
            if ((listFarm1.Count > 0))
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_messagelog n in listFarm1)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex1 - 1) * pageSize1 + k) + "、" + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                if (recordCount1 < 25)
                    builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, recordCount1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
                else
                    builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, 25, Utils.getPageUrl(), pageValUrl1, "page1", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有消息记录.."));
            }

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("防偷菜技巧：<br/>1.设置<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">陷阱</a>，让来偷菜的人成为我的奴隶.<br/>");
            builder.Append("2.设置加<a href=\"" + Utils.getUrl("../myedit.aspx?act=friend&amp;name=name") + "\">好友权限</a>,防止陌生人任意偷菜.");
            builder.Append(Out.Tab("</div>", ""));

            foot_link();//底部链接
        }
        else
        {
            foot_link();//底部链接
        }

        foot_link2();
    }

    //去偷菜的页面
    private void doPage()
    {
        //偷菜维护提示
        if (ub.GetSub("tcStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;");
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>&gt;农场好友");
        }
        else if (ptype == 2)
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=2") + "\">可操作</a>&gt;农场好友");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai&amp;ptype=3") + "\">农场好友</a>&gt;农场好友");
        }
        builder.Append(Out.Tab("</div>", "<br />"));
        Master.Title = "偷菜";

        int meid_usid = new BCW.User.Users().GetUsId();
        if (meid_usid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid_usid);//用户姓名

        int meid = Utils.ParseInt(Utils.GetRequest("uid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));

        if (meid == meid_usid)
            Utils.Error("不能去自己的农场.", "");

        //判断id是否存在农场
        if (!new BCW.farm.BLL.NC_user().Exists(meid))
            Utils.Error("抱歉,请选择正确的农场好友.", "");

        //if (!new BCW.BLL.Friend().Exists(meid_usid, meid, 0))//判断是否为好友
        //    Utils.Error("此农场好友不在你的好友列表,请先<a href=\"" + Utils.getUrl("/bbs/friend.aspx?act=add&amp;hid=" + meid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">添加对方好友</a>.", "");

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + meid + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(meid) + "</a>的农场<br/>");
        builder.Append("Ta的金币:" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + " 等级:" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + "级<br/>");
        builder.Append(Out.Tab("</div>", ""));
        builder.Append(Out.Tab("<div>", ""));
        try
        {
            string jy = new BCW.farm.BLL.NC_user().Get_jiyu(meid);
            if (jy != "")
                builder.Append("农场寄语:" + Out.SysUBB(jy) + "<br/>");
        }
        catch { }
        builder.Append("Ta的土地|<a href=\"" + Utils.getUrl("farm.aspx?act=market_Ta&amp;usid=" + meid + "") + "\">Ta的摊位</a>");
        builder.Append("|<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + meid + "&amp;a=1") + "\">留言</a>");
        builder.Append("|<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + meid + "") + "\">刷新</a><br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("<b>一键 </b>");

        //自动收割
        is_shougeji(meid);

        //更新收获状态时--草虫的状态
        new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate(),isinsect=0,z_chongtime=getdate()", "updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");

        //查询偷菜
        BCW.farm.Model.NC_tudi ty = new BCW.farm.BLL.NC_tudi().tou_tudinum2(meid, meid_usid);//查询已偷过的土地块数
        BCW.farm.Model.NC_tudi tp = new BCW.farm.BLL.NC_tudi().tou_tudinum1(meid);//查询有几块土地可以偷取
        BCW.farm.Model.NC_tudi qq = new BCW.farm.BLL.NC_tudi().cao_tudinum1(meid);//查询有几块土地可以除草
        BCW.farm.Model.NC_tudi ww = new BCW.farm.BLL.NC_tudi().shui_tudinum1(meid);//查询有几块土地可以浇水
        BCW.farm.Model.NC_tudi ee = new BCW.farm.BLL.NC_tudi().chong_tudinum1(meid);//查询有几块土地可以除虫

        BCW.farm.Model.NC_tudi cao = new BCW.farm.BLL.NC_tudi().fangcao_num1(meid);//查询有几块土地可以种草
        BCW.farm.Model.NC_tudi cao2 = new BCW.farm.BLL.NC_tudi().fangcao_num2(meid);//查询有几块土地可以放虫
        //if (ty.aa < tp.aa)
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo_2&amp;usid=" + meid + "") + "\">偷菜</a>.");

        int yy = 0; int yj_chucao = 0; int yj_chuchong = 0;
        //查询一键偷取
        DataSet yi = new BCW.farm.BLL.NC_tudi().GetList("touID", "usid=" + meid + " AND updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
        if (yi != null && yi.Tables[0].Rows.Count > 0)
        {
            for (int ii = 0; ii < yi.Tables[0].Rows.Count; ii++)
            {
                int db = 0;
                string touID = (yi.Tables[0].Rows[ii]["touID"].ToString());
                string[] ttt = touID.Split(',');//拆分id
                //得到偷的人数
                string[] sNum = Regex.Split(touID, ",");
                int bb = sNum.Length;
                if (bb < tou_renshu + 2)//设定的人数
                {
                    for (int bd = 0; bd < ttt.Length; bd++)
                    {
                        if (ttt[bd] == meid_usid.ToString())
                        {
                            db++;
                        }
                    }
                }
                else
                {
                    db++;
                }

                if (db == 0)//如果等于0，即是存在可以偷的土地
                {
                    yy = 1;
                    break;
                }
            }
        }
        //查询一键种草
        DataSet oj = new BCW.farm.BLL.NC_tudi().GetList("caoID", "usid=" + meid + " AND iscao=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE())");
        if (oj != null && oj.Tables[0].Rows.Count > 0)
        {
            int db1 = 0;
            for (int ii = 0; ii < oj.Tables[0].Rows.Count; ii++)
            {
                string caoID = (oj.Tables[0].Rows[ii]["caoID"].ToString());
                string[] _cao1 = caoID.Split(',');
                for (int bd = 0; bd < _cao1.Length; bd++)
                {
                    if (_cao1[bd] == meid_usid.ToString())
                    {
                        db1++;
                    }
                }
            }
            if (db1 < oj.Tables[0].Rows.Count)
            {
                yj_chucao++;
            }
        }
        //查询一键放虫
        DataSet oj2 = new BCW.farm.BLL.NC_tudi().GetList("chongID", "usid=" + meid + " AND isinsect=1 and ischandi=1 and updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE())");
        if (oj2 != null && oj2.Tables[0].Rows.Count > 0)
        {
            int db2 = 0;
            for (int ii = 0; ii < oj2.Tables[0].Rows.Count; ii++)
            {
                string chongID = (oj2.Tables[0].Rows[ii]["chongID"].ToString());
                string[] _chong1 = chongID.Split(',');
                for (int bd = 0; bd < _chong1.Length; bd++)
                {
                    if (_chong1[bd] == meid_usid.ToString())
                    {
                        db2++;
                    }
                }
            }
            if (db2 < oj2.Tables[0].Rows.Count)
            {
                yj_chuchong++;
            }
        }
        if (yy == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo_2&amp;usid=" + meid + "&amp;ptype=" + ptype + "") + "\">偷菜</a>.");
        }
        if (qq.aa > 0 && yj_chucao > 0)
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao_2&amp;usid=" + meid + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
        if (ww.aa > 0)
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui_2&amp;usid=" + meid + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
        if (ee.aa > 0 && yj_chuchong > 0)
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong_2&amp;usid=" + meid + "&amp;ptype=" + ptype + "") + "\">除虫</a>.");
        if (cao.aa > 0)
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=fangcao_2&amp;usid=" + meid + "&amp;type=1&amp;ptype=" + ptype + "") + "\">种草</a>.");
        if (cao2.aa > 0)
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=fangcao_2&amp;usid=" + meid + "&amp;type=2&amp;ptype=" + ptype + "") + "\">放虫</a>");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        int pageSize = 6;
        string[] pageValUrl = { "act", "ptype", "pageIndex", "uid", "page1", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        string strWhere = string.Empty;


        BCW.farm.Model.NC_tudi model = new BCW.farm.BLL.NC_tudi().Getusid(meid);
        BCW.farm.Model.NC_tudi count = new BCW.farm.BLL.NC_tudi().Get_tudinum(meid);

        DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid);
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                int id = int.Parse(ds.Tables[0].Rows[koo + i]["tudi"].ToString());//土地块数
                int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["usid"].ToString());//用户id
                string zuowu = ds.Tables[0].Rows[koo + i]["zuowu"].ToString();//作物名称
                int zuowu_time = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["updatetime"]);//作物种植时间
                int tudi_type = int.Parse(ds.Tables[0].Rows[koo + i]["tudi_type"].ToString());//土地类型
                int zuowu_ji = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_ji"].ToString());//作物生长的季度
                int iscao = int.Parse(ds.Tables[0].Rows[koo + i]["iscao"].ToString());//除草
                int iswater = int.Parse(ds.Tables[0].Rows[koo + i]["iswater"].ToString());//是否浇水
                int isinsect = int.Parse(ds.Tables[0].Rows[koo + i]["isinsect"].ToString());//昆虫
                int ischandi = int.Parse(ds.Tables[0].Rows[koo + i]["ischandi"].ToString());//铲地(0空1有2枯萎)
                string output = ds.Tables[0].Rows[koo + i]["output"].ToString();//产/剩
                int zuowu_experience = int.Parse(ds.Tables[0].Rows[koo + i]["zuowu_experience"].ToString());//作物经验
                int harvest = int.Parse(ds.Tables[0].Rows[koo + i]["harvest"].ToString());//收获多少季度
                string touID = ds.Tables[0].Rows[koo + i]["touID"].ToString();//偷菜人的ID
                string caoID = ds.Tables[0].Rows[koo + i]["caoID"].ToString();//草的ID
                string chongID = ds.Tables[0].Rows[koo + i]["chongID"].ToString();//虫的ID

                //查询改块土地，是否存在自己种的草
                string[] cao_id = caoID.Split(',');
                string[] chong_id = chongID.Split(',');
                int _cid = 0; int _zid = 0;
                for (int bd = 0; bd < cao_id.Length; bd++)
                {
                    if (cao_id[bd] == meid_usid.ToString())
                    {
                        _cid++;
                    }
                }
                for (int bd = 0; bd < chong_id.Length; bd++)
                {
                    if (chong_id[bd] == meid_usid.ToString())
                    {
                        _zid++;
                    }
                }

                //根据作物查询商店表对应的作物图片
                BCW.farm.Model.NC_shop gg = new BCW.farm.BLL.NC_shop().GetNC_shop2(zuowu);

                string[] tt = touID.Split(',');//拆分id
                //得到偷的人数
                string[] sNum = Regex.Split(touID, ",");
                int bb = sNum.Length;
                int a = 0;
                if (bb < tou_renshu + 2)//设定的人数
                {
                    for (int bd = 0; bd < tt.Length; bd++)
                    {
                        if (tt[bd] == meid_usid.ToString())
                        {
                            a++;
                        }
                    }
                }
                else
                {
                    a++;
                }

                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }

                if (zuowu == "")
                {
                    #region 作物为空
                    builder.Append("" + OutType(tudi_type) + id + ":[空地]<br/>");
                    try
                    {
                        builder.Append("<img src=\"/bbs/game/img/farm/kongdi.gif\" alt=\"load\"/>");
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    #endregion
                }
                else
                {
                    #region 作物不为空
                    if (ischandi == 1)
                    {
                        builder.Append("" + OutType(tudi_type) + id + ":" + zuowu + " (" + harvest + "/" + zuowu_ji + "季)<br/>");
                    }
                    else if (ischandi == 2)
                    {
                        builder.Append("" + OutType(tudi_type) + id + ":[枯萎的作物].<br/>");//<br/>
                        try
                        {
                            builder.Append("<img src=\"/bbs/game/img/farm/kuwei.gif\" alt=\"load\"/>");
                        }
                        catch { builder.Append("[图片出错!]<br/>"); }
                    }
                    #endregion
                }

                if (zuowu_ji == 1)
                {
                    #region 作物为一个季度
                    if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                    {
                        if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5))))//1/6==发芽
                        {
                            try
                            {
                                builder.Append("<img src=\"/bbs/game/img/farm/faya.png\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else if ((updatetime.AddMinutes((zuowu_time / 5)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 3))))//1/6==小叶子
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[0] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else if ((updatetime.AddMinutes((zuowu_time / 5 * 3)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 4))))//2/6==大叶子
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        else
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                        }
                        builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h>");
                        if ((iscao == 1 && _cid == 0) || (iswater == 1) || (isinsect == 1 && _zid == 0) || (iscao == 0) || (isinsect == 0))
                        {
                            builder.Append("<br/>");
                        }
                        if (iscao == 1 && _cid == 0)
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                        if (iswater == 1)
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                        if (isinsect == 1 && _zid == 0)
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>.");
                        if (iscao == 0)
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zcao1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">种草</a>.");
                        if (isinsect == 0)
                            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=fchong1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">放虫</a>");
                    }
                    else
                    {
                        if (ischandi == 1)
                        {
                            try
                            {
                                string[] we = gg.picture.Split(',');
                                builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                            }
                            catch { builder.Append("[图片出错!]<br/>"); }
                            string[] ab = output.Split(',');
                            builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");
                            //如果成熟了,把虫,草,水改为0
                            //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                            //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                            //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                            if (a == 0)
                            {
                                builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;pageIndex=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">偷菜</a>.");
                            }

                            BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                            if (UU.iscao == 1 && _cid == 0)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                            }
                            if (UU.iswater == 1)
                            {
                                if (a == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                }
                                else
                                {
                                    builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                }
                            }
                            if (UU.isinsect == 1 && _zid == 0)
                            {
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>");
                            }
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 作物为多个季度
                    if (harvest == 1)
                    {
                        if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                        {
                            #region 作物为第一季度未成熟时
                            if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5))))//1/6==发芽
                            {
                                try
                                {
                                    builder.Append("<img src=\"/bbs/game/img/farm/faya.png\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else if ((updatetime.AddMinutes((zuowu_time / 5)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 3))))//1/6==小叶子
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[0] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else if ((updatetime.AddMinutes((zuowu_time / 5 * 3)) < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes((zuowu_time / 5 * 4))))//2/6==大叶子
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }

                            builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h>");
                            if ((iscao == 1 && _cid == 0) || (iswater == 1) || (isinsect == 1 && _zid == 0) || (iscao == 0) || (isinsect == 0))
                            {
                                builder.Append("<br/>");
                            }
                            if (iscao == 1 && _cid == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                            if (iswater == 1)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                            if (isinsect == 1 && _zid == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>.");
                            if (iscao == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zcao1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">种草</a>.");
                            if (isinsect == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=fchong1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">放虫</a>");
                            #endregion
                        }
                        else
                        {
                            #region 作物为第一季度成熟
                            if (ischandi == 1)
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                                string[] ab = output.Split(',');
                                builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");
                                //如果成熟了,把虫,草,水改为0
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                                //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                                if (a == 0)
                                {
                                    builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;pageIndex=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">偷菜</a>.");
                                }

                                BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                                if (UU.iscao == 1 && _cid == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                                }
                                if (UU.iswater == 1)
                                {
                                    if (a == 0)
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                    }
                                    else
                                    {
                                        builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                    }
                                }
                                if (UU.isinsect == 1 && _zid == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>");
                                }
                            }
                            #endregion
                        }
                    }
                    else
                    {
                        //再成熟，时间为后面的2/5
                        if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                        {
                            #region 作物为多季度未成熟
                            if ((updatetime < DateTime.Now) && (DateTime.Now < updatetime.AddMinutes(zuowu_time / 2)))
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[1] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            else
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[2] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                            }
                            builder.Append("<h style=\"color:#A9A9A9\">" + DT.DateDiff(DateTime.Now, updatetime.AddMinutes(zuowu_time)) + "后成熟</h>");
                            if ((iscao == 1 && _cid == 0) || (iswater == 1) || (isinsect == 1 && _zid == 0) || (iscao == 0) || (isinsect == 0))
                            {
                                builder.Append("<br/>");
                            }
                            if (iscao == 1 && _cid == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                            if (iswater == 1)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                            if (isinsect == 1 && _zid == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>.");
                            if (iscao == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=zcao1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">种草</a>.");
                            if (isinsect == 0)
                                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=fchong1&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">放虫</a>");
                            #endregion
                        }
                        else
                        {
                            #region 作物为多季度成熟
                            if (ischandi == 1)
                            {
                                try
                                {
                                    string[] we = gg.picture.Split(',');
                                    builder.Append("<img src=\"" + we[3] + "\" alt=\"load\"/><br/>");
                                }
                                catch { builder.Append("[图片出错!]<br/>"); }
                                string[] ab = output.Split(',');
                                builder.Append("<h style=\"color:#A9A9A9\">成熟,产" + ab[0] + "剩" + ab[1] + "</h>");
                                //如果成熟了,把虫,草,水改为0
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//草
                                //new BCW.farm.BLL.NC_tudi().update_tudi("iswater=0,z_shuitime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//水
                                //new BCW.farm.BLL.NC_tudi().update_tudi("isinsect=0,z_chongtime=getdate()", "usid = '" + UsID + "' AND tudi='" + id + "'");//虫
                                if (a == 0)
                                {
                                    builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=shouhuo2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;pageIndex=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">偷菜</a>.");
                                }

                                BCW.farm.Model.NC_tudi UU = new BCW.farm.BLL.NC_tudi().Get_td(UsID, id);
                                if (UU.iscao == 1 && _cid == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chucao2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除草</a>.");
                                }
                                if (UU.iswater == 1)
                                {
                                    if (a == 0)
                                    {
                                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                    }
                                    else
                                    {
                                        builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=jiaoshui2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">浇水</a>.");
                                    }
                                }
                                if (UU.isinsect == 1 && _zid == 0)
                                {
                                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=chuchong2&amp;usid=" + UsID + "&amp;tudi=" + id + "&amp;fenye=" + pageIndex + "&amp;ptype=" + ptype + "") + "\">除虫</a>");
                                }
                            }
                            #endregion
                        }
                    }
                    #endregion
                }
                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "土地出错."));
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("留言列表：<br/>");
        builder.Append(Out.Tab("</div>", ""));
        int pageIndex1;
        int recordCount1;
        int pageSize1 = 5;
        string[] pageValUrl1 = { "act", "ptype", "pageIndex", "uid", "page", "backurl" };
        pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
        if (pageIndex1 == 0)
            pageIndex1 = 1;
        string strWhere1 = string.Empty;
        strWhere1 = "usid=" + meid + " and type=1001";
        // 开始读取列表
        IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex1, pageSize1, strWhere1, out recordCount1);
        if (listMebook.Count > 0)
        {
            int k = 1;
            foreach (BCW.Model.Mebook n in listMebook)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + n.MID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + n.MName + "(" + n.MID + ")</a>");
                builder.Append(":" + Out.SysUBB(n.MContent) + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, 1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有留言记录.."));
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + meid + "&amp;a=1") + "\">给Ta留言>></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">更多好友农场>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }

    //到处看看
    private void allseePage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        Master.Title = "到处看看";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>&gt;到处看看");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));

        int ui = int.Parse(Utils.GetRequest("ui", "all", 1, @"^[0-1]$", "0"));

        if (ui == 1)
        {
            BCW.User.Users.IsFresh("farmkk", 1);//防刷
            int uid = Utils.ParseInt(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));
            if (uid != 0)
            {
                new BCW.BLL.Guest().Add(1, uid, new BCW.BLL.User().GetUsName(uid), "你的好友" + mename + "正在玩[url=/bbs/game/farm.aspx?&amp;uid=" + meid + "]" + GameName + "[/url],Ta邀请你一起去玩!");
                Utils.Success("邀请好友", "恭喜您邀请成功.", Utils.getUrl("farm.aspx?act=allsee&amp;ptype=2"), "1");
            }
        }

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype == 1)
        {
            builder.Append("<h style=\"color:red\">农场其他好友" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=allsee&amp;ptype=1") + "\">农场其他好友</a>" + "|");
        }
        if (ptype == 2)
        {
            builder.Append("<h style=\"color:red\">邀请好友" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=allsee&amp;ptype=2") + "\">邀请好友</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;//数量
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));//分页条数

        if (ptype == 1)
        {
            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(usid)", "usid!=" + meid + " and usid not in (select friendid from tb_Friend WHERE usid=" + meid + " ) and updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id

                    if (UsID != meid)//如果不等于自己的id
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>");
                        builder.Append("(" + UsID + ")");
                        builder.Append(":<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + UsID + "") + "\">[偷]</a>");
                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + UsID + "") + "\">[留言]</a>");
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有更多数据.."));
            }
        }
        else
        {
            DataSet ds = new BCW.BLL.Friend().GetList("FriendID", "UsID=" + meid + " and tb_Friend.FriendID NOT IN (SELECT UsID FROM tb_NC_tudi)");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id

                    if (UsID != meid)//如果不等于自己的id
                    {
                        //if (k % 2 == 0)
                        //    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        //else
                        //{
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                        //}
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + UsID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "</a>");
                        builder.Append("(" + UsID + ")");
                        string strText = ",,,";
                        string strName = "uid,act,ui";
                        string strType = "hidden,hidden,hidden";
                        string strValu = "" + UsID + "'allsee'1";
                        string strEmpt = "false,false,false";
                        string strIdea = "";
                        string strOthe = "发邀请,farm.aspx,post,1,red";
                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有更多数据.快去添加更多好友吧."));
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>1.成功邀请好友进入" + GameName + ",还可获得种子和金币奖励哦!邀请越多,赠送越多!");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }


    //我的奴隶
    private void PunishPage()
    {
        //奴隶和陷阱维护提示
        if (ub.GetSub("nlxjStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;我的奴隶");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("等级:" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) + " ");
        builder.Append("金币:" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "");
        builder.Append(Out.Tab("</div>", "<br />"));

        //查询奴隶是否过期，若过期，则tpye更新为0
        if (slave_day > 0)
            new BCW.farm.BLL.NC_slave().update_ziduan("tpye=0", "updatetime<DATEADD(day, -" + slave_day + ", GETDATE()) AND tpye=1");//usid=" + meid + " and 

        builder.Append(Out.Tab("<div>", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "我的奴隶";
            builder.Append("<h style=\"color:red\">我的奴隶" + "</h>" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=punish&amp;ptype=1") + "\">我的奴隶</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "我的惩罚/安抚记录";
            builder.Append("<h style=\"color:red\">我的惩罚/安抚记录" + "</h>" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=punish&amp;ptype=2") + "\">我的惩罚/安抚记录</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;

        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        if (pageIndex == 0)
            pageIndex = 1;

        string strOrder = "";
        if (ptype == 1)
        {
            strWhere = "usid=" + meid + " and tpye=1";

            IList<BCW.farm.Model.NC_slave> listFarm = new BCW.farm.BLL.NC_slave().GetNC_slaves(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_slave n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string mename2 = new BCW.BLL.User().GetUsName(n.slave_id);
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + n.slave_id + "") + "\">" + mename2 + "</a>：");
                    //builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=nlcf&amp;id=" + n.slave_id + "") + "\">[惩罚/安抚]</a>");
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=just&amp;ptype=1&amp;id=" + n.slave_id + "") + "\">[惩罚" + (n.punish) + "/" + slave_num + "]</a>.");
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=just&amp;ptype=2&amp;id=" + n.slave_id + "") + "\">[安抚" + (n.pacify) + "/" + slave_num + "]</a>");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "您暂时没有抓到任何奴隶..<br/><a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">立即设置陷阱>></a>"));
            }
        }
        if (ptype == 2)
        {
            strWhere = "usid=" + meid + " and type=2";//GetNC_messagelogs奴隶type为2

            IList<BCW.farm.Model.NC_messagelog> listFarm = new BCW.farm.BLL.NC_messagelog().GetNC_messagelogs(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_messagelog n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    string mename2 = new BCW.BLL.User().GetUsName(n.UsId);
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + Out.SysUBB(n.AcText) + "(" + (n.AddTime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "您暂时没有惩罚/安抚记录..<br/><a href=\"" + Utils.getUrl("farm.aspx?act=xianjing") + "\">立即设置陷阱>></a>"));
            }
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示：<br/>1.惩罚奴隶可以获得金币或经验.<br/>2.安抚奴隶可以让对方获得金币或经验.<br/>3.每个奴隶只能惩罚和安抚" + slave_num + "次.<br/>4.每个奴隶只能存放" + slave_day + "天,过期后则自动解除关系.");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();
        foot_link2();
    }
    //惩罚奴隶界面
    private void nlcfPage()
    {
        //奴隶和陷阱维护提示
        if (ub.GetSub("nlxjStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择奴隶ID无效"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        string mename2 = new BCW.BLL.User().GetUsName(usid);

        if (!new BCW.farm.BLL.NC_slavelist().Exists(id))
        {
            Utils.Error("请正确选择惩罚/安抚的方式.", "");
        }

        if (new BCW.farm.BLL.NC_slave().Exists_nl(usid, meid))
        {
            BCW.farm.Model.NC_slave aa = new BCW.farm.BLL.NC_slave().GetNCslave(meid, usid);
            BCW.farm.Model.NC_slavelist uu = new BCW.farm.BLL.NC_slavelist().GetNC_slavelist(id);
            if (ptype == 1)
            {
                if (aa.punish < slave_num)
                {
                    BCW.User.Users.IsFresh("Farmnl", 1);//防刷
                    new BCW.farm.BLL.NC_slave().update_ziduan("punish=" + (aa.punish + 1) + "", "usid=" + meid + " and slave_id=" + usid + " and tpye=1");//次数+1

                    //惩罚：主人有加钱，奴隶减钱
                    //加钱
                    if (uu.inGold > 0)
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, uu.inGold, "你惩罚奴隶(" + mename2 + "):" + uu.contact + ".得到" + uu.inGold + "金币.目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + uu.inGold) + "金币.", 2);
                    //减钱
                    if (uu.outGold > 0)
                        new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename2, -uu.outGold, "被主人惩罚了:" + uu.contact + ".损失" + uu.outGold + "金币.目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) - uu.inGold) + "金币.", 2);

                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "你惩罚了奴隶:[url=/bbs/uinfo.aspx?uid=" + usid + "]" + mename2 + "(" + usid + ")[/url]:" + uu.contact + ".得到" + uu.inGold + "金币.", 2);//消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]惩罚了你:" + uu.contact + ".损失" + uu.outGold + "金币", 2);//消息
                    new BCW.BLL.Guest().Add(1, usid, mename2, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]惩罚了你:" + uu.contact + ".损失" + uu.outGold + "金币.");//内线给被惩罚者
                    //内线给主人
                    new BCW.BLL.Guest().Add(1, meid, mename, "你惩罚了奴隶:[url=/bbs/uinfo.aspx?uid=" + usid + "]" + mename2 + "(" + usid + ")[/url]:" + uu.contact + ".得到" + uu.inGold + "金币.");
                    Utils.Success("操作奴隶", "您成功惩罚了奴隶:" + mename2 + ":" + uu.contact + ".得到" + uu.inGold + "金币", Utils.getUrl("farm.aspx?act=just&amp;ptype=1&amp;id=" + usid + ""), "1");
                }
                else
                    Utils.Success("惩罚奴隶", "该奴隶惩罚次数已达上限.", Utils.getUrl("farm.aspx?act=punish"), "1");
            }
            else
            {
                if (aa.pacify < slave_num)
                {
                    BCW.User.Users.IsFresh("Farmnl", 1);//防刷
                    new BCW.farm.BLL.NC_slave().update_ziduan("pacify=" + (aa.pacify + 1) + "", "usid=" + meid + " and slave_id=" + usid + " and tpye=1");

                    //安抚：主人得到的是奴隶的2倍
                    //加钱
                    if (uu.inGold > 0)
                    {
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, uu.inGold, "我安抚了奴隶:" + uu.contact + ".得到" + uu.inGold + "金币.目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + uu.inGold) + "金币.", 2);
                        new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename2, uu.outGold, "主人安抚了你:" + uu.contact + ".得到" + uu.outGold + "金币.目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) + uu.outGold) + "金币.", 2);
                    }

                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename2, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]安抚了你:" + uu.contact + ".得到" + uu.outGold + "金币.", 2);//消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "你安抚了奴隶:[url=/bbs/uinfo.aspx?uid=" + usid + "]" + mename2 + "(" + usid + ")[/url]:" + uu.contact + ".得到" + uu.inGold + "金币.", 2);//消息

                    new BCW.BLL.Guest().Add(1, usid, mename2, "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "(" + meid + ")[/url]安抚了你:" + uu.contact + ".得到" + uu.outGold + "金币.");//内线给被惩罚者
                    //内线给主人
                    new BCW.BLL.Guest().Add(1, meid, mename, "你安抚了奴隶:[url=/bbs/uinfo.aspx?uid=" + usid + "]" + mename2 + "(" + usid + ")[/url]:" + uu.contact + ".得到" + uu.inGold + "金币.");
                    Utils.Success("操作奴隶", "您成功安抚了奴隶:" + mename2 + ":" + uu.contact + ".得到" + uu.inGold + "金币", Utils.getUrl("farm.aspx?act=just&amp;ptype=2&amp;id=" + usid + ""), "1");
                }
                else
                    Utils.Success("安抚奴隶", "该奴隶安抚次数已达上限.", Utils.getUrl("farm.aspx?act=punish"), "1");
            }
        }
        else
            Utils.Error("抱歉,该奴隶不属于你.", "");

    }
    //奴隶的安抚和惩罚
    private void justPage()
    {
        //奴隶和陷阱维护提示
        if (ub.GetSub("nlxjStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=punish") + "\">我的奴隶</a>&gt;惩罚安抚奴隶");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int usid = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        string mename2 = new BCW.BLL.User().GetUsName(usid);

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "id", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        if (pageIndex == 0)
            pageIndex = 1;

        if (new BCW.farm.BLL.NC_slave().Exists_nl(usid, meid))//判断是否自己的奴隶
        {
            BCW.farm.Model.NC_slave aa = new BCW.farm.BLL.NC_slave().GetNCslave(meid, usid);
            if (ptype == 1)//惩罚
            {
                if (aa.punish < slave_num)
                {
                    Master.Title = "我的奴隶.惩罚奴隶";
                    strWhere = "type=0";
                    IList<BCW.farm.Model.NC_slavelist> listFarm = new BCW.farm.BLL.NC_slavelist().GetNC_slavelists(pageIndex, pageSize, strWhere, out recordCount);
                    if (listFarm.Count > 0)
                    {
                        int k = 1;
                        foreach (BCW.farm.Model.NC_slavelist n in listFarm)
                        {
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", ""));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.contact + ".<a href=\"" + Utils.getUrl("farm.aspx?act=nlcf&amp;id=" + n.ID + "&amp;usid=" + usid + "&amp;ptype=1") + "\">[惩罚]</a>");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        // 分页
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    }
                    else
                        builder.Append(Out.Div("div", "暂时没有惩罚方式."));
                }
                else
                    Utils.Success("惩罚奴隶", "该奴隶惩罚次数已达上限.", Utils.getUrl("farm.aspx?act=punish"), "1");
            }
            else
            {
                if (aa.pacify < slave_num)
                {
                    Master.Title = "我的奴隶.安抚奴隶";
                    strWhere = "type=1";
                    IList<BCW.farm.Model.NC_slavelist> listFarm = new BCW.farm.BLL.NC_slavelist().GetNC_slavelists(pageIndex, pageSize, strWhere, out recordCount);
                    if (listFarm.Count > 0)
                    {
                        int k = 1;
                        foreach (BCW.farm.Model.NC_slavelist n in listFarm)
                        {
                            if (k % 2 == 0)
                                builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                            else
                            {
                                if (k == 1)
                                    builder.Append(Out.Tab("<div>", ""));
                                else
                                    builder.Append(Out.Tab("<div>", "<br />"));
                            }
                            builder.Append("" + ((pageIndex - 1) * pageSize + k) + "." + n.contact + ".<a href=\"" + Utils.getUrl("farm.aspx?act=nlcf&amp;id=" + n.ID + "&amp;usid=" + usid + "&amp;ptype=2") + "\">[安抚]</a>");
                            k++;
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        // 分页
                        builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                    }
                    else
                        builder.Append(Out.Div("div", "暂时没有安抚方式."));
                }
                else
                    Utils.Success("安抚奴隶", "该奴隶安抚次数已达上限.", Utils.getUrl("farm.aspx?act=punish"), "1");
            }
        }
        else
            Utils.Error("抱歉,该奴隶不属于你.", "");

        foot_link();
        foot_link2();
    }
    //陷阱界面
    private void XianjingPage()
    {
        //奴隶和陷阱维护提示
        if (ub.GetSub("nlxjStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "设置陷阱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;设置陷阱");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string ac2 = Utils.GetRequest("ac2", "all", 1, "", "");

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("你目前设置有" + new BCW.farm.BLL.NC_tudi().Get_xianjing(meid) + "个陷阱,还可以设置" + (xianjing_num - new BCW.farm.BLL.NC_tudi().Get_xianjing(meid)) + "个.");
        builder.Append(Out.Tab("</div>", ""));

        if (Utils.ToSChinese(ac) == "设置陷阱" || ac2 == "szxj")
        {
            BCW.farm.Model.NC_mydaoju yy = new BCW.farm.BLL.NC_mydaoju().Get_yys(meid, 21);
            if (yy.num > 0)//判断是否有钥匙
            {
                if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= xianjing_dengji)
                {
                    if (new BCW.farm.BLL.NC_user().GetGold(meid) >= xianjing_jinbi)
                    {
                        DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("*", "usid=" + meid + "");//and zuowu!='' and ischandi=1
                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                        {
                            builder.Append(Out.Tab("<div></div>", ""));
                            int id = 1;
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                id = int.Parse(ds.Tables[0].Rows[i]["tudi"].ToString());//土地块数
                                int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());//用户id
                                string zuowu = ds.Tables[0].Rows[i]["zuowu"].ToString();//作物名称
                                int tudi_type = int.Parse(ds.Tables[0].Rows[i]["tudi_type"].ToString());//土地类型
                                int zuowu_time = int.Parse(ds.Tables[0].Rows[i]["zuowu_time"].ToString());//作物生长需要时间(分钟)
                                DateTime updatetime = Convert.ToDateTime(ds.Tables[0].Rows[i]["updatetime"]);//作物种植时间
                                int ischandi = int.Parse(ds.Tables[0].Rows[i]["ischandi"].ToString());//铲地(0空1有2枯萎)
                                int xianjing = int.Parse(ds.Tables[0].Rows[i]["xianjing"].ToString());//陷阱
                                if (ischandi == 1)
                                {
                                    builder.Append(Out.Tab("<div>", "<br/>"));
                                    builder.Append("" + OutType(tudi_type) + id + ":" + zuowu + ":");
                                    if (DateTime.Now < updatetime.AddMinutes(zuowu_time))
                                    {
                                        builder.Append("未成熟");
                                    }
                                    else
                                    {
                                        builder.Append("已成熟");
                                    }
                                    if (xianjing == 0)
                                    {
                                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=setxj&amp;usid=" + UsID + "&amp;id=" + id + "&amp;ptype=1") + "\">[设置陷阱]</a>");
                                    }
                                    else
                                    {
                                        builder.Append(".<h style=\"color:red\">[已设置]</h>.<a href=\"" + Utils.getUrl("farm.aspx?act=setxj&amp;usid=" + UsID + "&amp;id=" + id + "&amp;ptype=2") + "\">[取消设置]</a>");
                                    }
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else if (ischandi == 0)
                                {
                                    builder.Append(Out.Tab("<div>", "<br/>"));
                                    builder.Append("" + OutType(tudi_type) + id + ":[空土地]");
                                    if (xianjing == 1)
                                    {
                                        builder.Append(".<h style=\"color:red\">[已设置]</h>.<a href=\"" + Utils.getUrl("farm.aspx?act=setxj&amp;usid=" + UsID + "&amp;id=" + id + "&amp;ptype=2") + "\">[取消设置]</a>");
                                    }
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                                else
                                {
                                    builder.Append(Out.Tab("<div>", "<br/>"));
                                    builder.Append("" + OutType(tudi_type) + id + ":[枯萎的土地]");
                                    if (xianjing == 1)
                                    {
                                        builder.Append(".<h style=\"color:red\">[已设置]</h>.<a href=\"" + Utils.getUrl("farm.aspx?act=setxj&amp;usid=" + UsID + "&amp;id=" + id + "&amp;ptype=2") + "\">[取消设置]</a>");
                                    }
                                    builder.Append(Out.Tab("</div>", ""));
                                }
                            }
                        }
                        else
                        {
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            builder.Append("暂时没有土地可以设置.快去<a href=\"" + Utils.getUrl("farm.aspx") + "\">种植作物</a>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                    }
                    else
                        Utils.Error("抱歉,你的金币不够,不能设置陷阱.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>赚钱吧", "");
                }
                else
                    Utils.Error("抱歉,你的等级不够,不能设置陷阱.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>升级吧", "");
            }
            else
                Utils.Error("抱歉,你没有银钥匙.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=21") + "\">商店购买</a>吧", "");
        }
        else
        {
            string strText = ",,";
            string strName = "act,backurl";
            string strType = "hidden,hidden";
            string strValu = "xianjing'" + Utils.getPage(0) + "";
            string strEmpt = "false,false";
            string strIdea = "/";
            string strOthe = "设置陷阱,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }

        builder.Append(Out.Tab("<div>", "<br/>"));
        builder.Append("温馨提示：<br/>1.设置陷阱需要达到" + xianjing_dengji + "级.<br/>2.需要一把<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=21") + "\">银钥匙</a>，和" + xianjing_jinbi + "金币.<br/>");
        builder.Append("3.陷阱可以具体设置某一块土地,设置有效期为" + xianjing_day + "天.<br/>4.有1/" + xianjing_jilv + "的机会捕抓到偷菜的人,让他成为你的奴隶.<br/>");
        builder.Append("5.只能对有作物的土地进行设置.<br/>6.若对方在偷菜中踩中陷阱,则损失" + xianjing_jb + "金币,你将得到" + xianjing_jb + "金币.");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接

        foot_link2();
    }
    //设置陷阱
    private void setxjPage()
    {
        //奴隶和陷阱维护提示
        if (ub.GetSub("nlxjStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();

        int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int tudi = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择土地出错"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        string mename = new BCW.BLL.User().GetUsName(usid);//用户姓名
        string ac = Utils.GetRequest("ac", "all", 1, "", "");

        if (ptype == 1)//设置陷阱
        {
            BCW.farm.Model.NC_mydaoju yy = new BCW.farm.BLL.NC_mydaoju().Get_yys(usid, 21);
            if (yy.num > 0)//判断是否有钥匙
            {
                if (new BCW.farm.BLL.NC_tudi().Get_xianjing(usid) < xianjing_num)
                {
                    if (new BCW.farm.BLL.NC_tudi().Exists_xianjing(tudi, usid))
                    {
                        if (new BCW.farm.BLL.NC_user().GetGrade(usid) >= xianjing_dengji)
                        {
                            if (new BCW.farm.BLL.NC_user().GetGold(usid) >= xianjing_jinbi)
                            {
                                BCW.User.Users.IsFresh("Farm_xj", 2);//防刷
                                new BCW.farm.BLL.NC_tudi().update_tudi("xianjing=1", "usid = '" + usid + "' AND tudi='" + tudi + "'");//改1
                                //new BCW.farm.BLL.NC_mydaoju().Update_hf(usid, -1, 21);//扣道具
                                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(21, meid, 1))
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 21, 1);
                                else
                                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -1, 21, 0);
                                new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename, -xianjing_jinbi, "在农场第" + tudi + "块设置了陷阱,消费" + xianjing_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) - xianjing_jinbi) + "金币.", 2);
                                new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在农场第" + tudi + "块土地设置了陷阱.", 11);//消息
                                BCW.farm.Model.NC_daoju_use gg = new BCW.farm.Model.NC_daoju_use();//加到道具使用表
                                gg.updatetime = DateTime.Now;
                                gg.usid = meid;
                                gg.daoju_id = 21;
                                gg.tudi = tudi;
                                gg.type = 1;
                                new BCW.farm.BLL.NC_daoju_use().Add(gg);
                                Utils.Success("设置陷阱", "您成功在第" + tudi + "块土地设置了陷阱", Utils.getUrl("farm.aspx?act=xianjing&amp;ac2=szxj"), "1");
                            }
                            else
                                Utils.Error("抱歉,你的金币不够,不能设置陷阱.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>赚钱吧", "");
                        }
                        else
                            Utils.Error("抱歉,你的等级不够,不能设置陷阱.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>升级吧", "");
                    }
                    else
                        Utils.Success("设置陷阱", "重复操作或没有设置陷阱的土地", Utils.getUrl("farm.aspx?act=xianjing&amp;ac2=szxj"), "1");
                }
                else
                    Utils.Error("抱歉,最多只能设置了3个陷阱.", "");
            }
            else
                Utils.Error("抱歉,你没有银钥匙.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=21") + "\">商店购买</a>吧", "");

        }
        else if (ptype == 2)//取消陷阱
        {
            if (Utils.ToSChinese(ac) == "取消陷阱")
            {
                if (new BCW.farm.BLL.NC_tudi().Exists_xianjing2(tudi, usid))
                {
                    BCW.User.Users.IsFresh("Farm_xj", 2);//防刷
                    new BCW.farm.BLL.NC_tudi().update_tudi("xianjing=0", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(usid, 1, 21, 0);//加道具
                    new BCW.farm.BLL.NC_user().UpdateiGold(usid, mename, xianjing_jinbi, "在农场第" + tudi + "块取消了陷阱,返回" + xianjing_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(usid) + xianjing_jinbi) + "金币.", 2);
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, mename, "在农场第" + tudi + "块土地取消了陷阱.", 11);//消息
                    new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "usid=" + meid + " and tudi=" + tudi + " and daoju_id=21");
                    Utils.Success("设置陷阱", "您成功在第" + tudi + "块土地取消了陷阱", Utils.getUrl("farm.aspx?act=xianjing&amp;ac2=szxj"), "1");
                }
                else
                    Utils.Success("设置陷阱", "重复操作或没有取消陷阱的土地", Utils.getUrl("farm.aspx?act=xianjing&amp;ac2=szxj"), "1");
            }
            else
            {
                Master.Title = "取消陷阱";
                builder.Append(Out.Tab("<div class=\"title\">", ""));
                builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing&amp;ac=设置陷阱") + "\">设置陷阱</a>&gt;取消陷阱<br/>");
                builder.Append(Out.Tab("</div>", ""));

                builder.Append(Out.Tab("<div>", ""));
                builder.Append("温馨提示：<br/>取消设置的陷阱，所用的银钥匙不能退还，确定取消吗？");
                builder.Append(Out.Tab("</div>", ""));

                string strText = ",,,,,";
                string strName = "usid,id,ptype,act,backurl";
                string strType = "hidden,hidden,hidden,hidden,hidden";
                string strValu = "" + usid + "'" + tudi + "'" + ptype + "'setxj'" + Utils.getPage(0) + "";
                string strEmpt = "false,false,false,false,false";
                string strIdea = "/";
                string strOthe = "取消陷阱,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", Out.Hr()));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=xianjing&amp;ac2=szxj") + "\">再看看吧>></a>");
                builder.Append(Out.Tab("</div>", ""));
                foot_link();//底部链接
                foot_link2();
            }
        }
        else
            Utils.Error("抱歉,错误操作.", "");
    }

    //宝箱界面
    private void baoxiangPage()
    {
        //宝箱维护提示
        if (ub.GetSub("bxStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "宝箱";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;宝箱");
        builder.Append(Out.Tab("</div>", "<br />"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //随机读取一个usid
        BCW.farm.Model.NC_win aa = new BCW.farm.BLL.NC_win().GetNC_suiji(0);

        builder.Append(Out.Tab("<div>", ""));
        try
        {
            builder.Append("宝箱信息:" + new BCW.BLL.User().GetUsName(aa.usid) + "(" + aa.usid + ")开启了宝箱获得" + aa.prize_name + ".<br/>");
        }
        catch
        {
            builder.Append("宝箱信息:暂无.<br/>");
        }
        builder.Append(Out.Tab("</div>", ""));

        //图片gif
        builder.Append(Out.Tab("<div>", ""));
        string gif1 = ub.GetSub("farm_bx", xmlPath);
        string show_logo = "<img src=\"" + gif1 + "\" alt=\"load\"/>";
        builder.Append(show_logo);
        builder.Append("<br/><a href=\"" + Utils.getUrl("farm.aspx?act=openbx") + "\">开启宝箱</a>");
        builder.Append(Out.Tab("</div>", ""));

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        string[] pageValUrl = { "act", "ptype", "ac", "backurl" };
        if (ac == "bxjl")
        {
            builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
            builder.Append("我的宝箱记录：<br/>");
            builder.Append(Out.Tab("</div>", ""));

            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strWhere = " ";

            pageIndex = Utils.ParseInt(Request.QueryString["page"]);

            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "UsId=" + meid + "";
            IList<BCW.farm.Model.NC_win> listFarm = new BCW.farm.BLL.NC_win().GetNC_wins(pageIndex, pageSize, strWhere, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_win n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".我开启宝箱,获得" + Out.SysUBB(n.prize_name) + "(" + (n.addtime) + ")");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }

                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));

            }
            else
            {
                builder.Append(Out.Div("div", "没有开宝箱记录.."));
            }

            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("说明:开启宝箱将有机会获得农场经验奖励、金币奖励、道具奖励等大奖.<br/>条件:需要" + bxys_num + "把宝箱钥匙开启.<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=22&amp;bx=1") + "\">马上购买>></a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang&amp;ac=bxjl") + "\">查看我的宝箱记录>></a>");
            builder.Append(Out.Tab("</div>", ""));
            foot_link();//底部链接
        }
        else
        {
            builder.Append(Out.Tab("<div>", Out.Hr()));
            builder.Append("说明:开启宝箱将有机会获得农场经验奖励、金币奖励、道具奖励等大奖.<br/>条件:需要" + bxys_num + "把宝箱钥匙开启.<br/>");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=22&amp;bx=1") + "\">马上购买>></a><br/>");
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=baoxiang&amp;ac=bxjl") + "\">查看我的宝箱记录>></a>");
            builder.Append(Out.Tab("</div>", ""));
            foot_link();//底部链接
        }

        foot_link2();
    }
    //开启宝箱--不可赠送
    private void openbxPage()
    {
        //宝箱维护提示
        if (ub.GetSub("bxStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "开启宝箱";
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string cc = string.Empty;
        string dd = string.Empty;
        BCW.farm.Model.NC_mydaoju yy = new BCW.farm.BLL.NC_mydaoju().Get_bxys(meid, 22);
        if (yy.num >= bxys_num)//判断是否有钥匙
        {
            BCW.farm.Model.NC_baoxiang yu = new BCW.farm.BLL.NC_baoxiang().Get_num(1);//查询宝箱有多少个道具
            BCW.farm.Model.NC_win rp = new BCW.farm.Model.NC_win();

            int aa = 1; //GetPtype3()
            //int updateigold_id = 0;
            //int tt = 0;
            BCW.User.Users.IsFresh("Farmbx", 1);//防刷
            if (aa == 1)//随机道具
            {
                if (yu.aa > 0)//如果道具>0
                {
                    //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22);//减宝箱钥匙
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(22, meid, 1))
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 1);
                    else
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 0);
                    int ww = R(0, yu.aa);//随机宝箱表里面的道具ID
                    List<BCW.farm.Model.NC_baoxiang> bx = new BCW.farm.BLL.NC_baoxiang().GetModelList("");
                    if (bx[ww].type == 1)//1种子2道具
                    {
                        if (new BCW.farm.BLL.NC_mydaoju().Exists2(bx[ww].daoju_id, meid))
                        {
                            BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, bx[ww].daoju_id);//查询种子个数
                            dd = bbb.name;
                            new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, 1, bx[ww].daoju_id);
                        }
                        else
                        {
                            if (new BCW.farm.BLL.NC_mydaoju().Exists(bx[ww].daoju_id, meid))//如果道具表有了该种子
                            {
                                BCW.farm.Model.NC_mydaoju bbb = new BCW.farm.BLL.NC_mydaoju().Getname_id(meid, bx[ww].daoju_id);//查询种子个数
                                dd = bbb.name;
                                new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, 1, bx[ww].daoju_id);
                            }
                            else
                            {
                                BCW.farm.Model.NC_shop y = new BCW.farm.BLL.NC_shop().GetNC_shop1(bx[ww].daoju_id);
                                BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                                dd = y.name;//名称
                                u.name = y.name;
                                u.name_id = bx[ww].daoju_id;
                                u.num = 1;
                                u.type = 1;
                                u.usid = meid;
                                u.zhonglei = y.type;
                                u.huafei_id = 0;
                                try
                                {
                                    string[] bv = y.picture.Split(',');
                                    u.picture = bv[4];
                                }
                                catch
                                {
                                    u.picture = "";
                                }
                                new BCW.farm.BLL.NC_mydaoju().Add(u);
                            }
                        }
                        rp.prize_type = 1;
                    }
                    else
                    {
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(bx[ww].daoju_id, meid, 1))
                        {
                            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, bx[ww].daoju_id, 1);//查询化肥数量
                            dd = aaa.name;
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, bx[ww].daoju_id, 1);
                        }
                        else
                        {
                            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(bx[ww].daoju_id, meid, 1))
                            {
                                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, bx[ww].daoju_id, 1);//查询化肥数量
                                dd = aaa.name;
                                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, bx[ww].daoju_id, 1);
                            }
                            else
                            {
                                //根据id判断
                                BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(bx[ww].daoju_id);
                                BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                                dd = t.name;
                                w.name = t.name;
                                w.name_id = 0;
                                w.num = 1;
                                w.type = 2;
                                w.usid = meid;
                                w.zhonglei = 0;
                                w.huafei_id = bx[ww].daoju_id;
                                w.picture = t.picture;
                                w.iszengsong = 1;
                                new BCW.farm.BLL.NC_mydaoju().Add(w);
                            }
                        }
                        rp.prize_type = 2;
                    }
                    rp.prize_id = bx[ww].daoju_id;
                    rp.prize_name = "" + dd + "一个";
                    cc = "获得一个" + dd + ".";
                }
                else
                    Utils.Error("开宝箱失败,请稍候再试.", "");
            }
            else if (aa == 2)//奖励金币
            {
                //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22);//减宝箱钥匙
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(22, meid, 1))
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 1);
                else
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 0);
                int hh = GetPtype4();//宝箱专用
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, hh * bx_jishu_jinbi, "在农场开启宝箱,获得" + hh * bx_jishu_jinbi + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (hh * bx_jishu_jinbi)) + "金币.", 6);//加金币
                cc = "获得" + hh * bx_jishu_jinbi + "金币.";
                rp.prize_id = 0;
                rp.prize_name = "" + hh * bx_jishu_jinbi + "金币";
                rp.prize_type = 0;
            }
            else //奖励经验
            {
                //new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22);//减宝箱钥匙
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(22, meid, 1))
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 1);
                else
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22, 0);
                cc = "获得" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * bx_jishu + "经验.";//等级*奇数
                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid)) * bx_jishu);//+经验
                //等级操作
                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                rp.prize_id = 22222222;//8个2
                rp.prize_name = "" + (new BCW.farm.BLL.NC_user().GetGrade(meid)) * bx_jishu + "经验";
                rp.prize_type = 0;
            }
            //else//奖励酷币
            //{
            //    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -bxys_num, 22);//减宝箱钥匙
            //    tt = GetPtype4();//宝箱专用
            //    updateigold_id = 1;
            //    //new BCW.BLL.User().UpdateiGold(meid, mename, tt * bx_jishu, "您在农场开启了宝箱获得奖励" + tt * bx_jishu + "酷币.");//加酷币
            //    cc = "获得" + tt * bx_jishu + "酷币.";
            //    rp.prize_id = 11111111;//8个1
            //    rp.prize_name = "" + tt * bx_jishu + "酷币";
            //    rp.prize_type = 0;
            //}
            rp.addtime = DateTime.Now;
            rp.usid = meid;
            int kk = new BCW.farm.BLL.NC_win().Add(rp);

            //if (updateigold_id == 1)
            //{
            //    new BCW.BLL.User().UpdateiGold(meid, mename, tt * bx_jishu, "您在农场开启了宝箱获得奖励" + tt * bx_jishu + "酷币-标识ID" + kk + "");//加酷币
            //}

            new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, cc, 6);//宝箱
            BCW.farm.Model.NC_daoju_use hu = new BCW.farm.Model.NC_daoju_use();
            hu.type = 0;
            hu.daoju_id = 22;
            hu.tudi = 0;
            hu.updatetime = DateTime.Now;
            hu.usid = meid;
            int ii = new BCW.farm.BLL.NC_daoju_use().Add(hu);

            //动态
            string wText = "在[url=/bbs/game/farm.aspx?act=baoxiang]" + GameName + "[/url]开启宝箱," + cc + "";//[url=/bbs/uinfo.aspx?uid=" + meid + "]" + mename + "[/url]
            new BCW.BLL.Action().Add(1011, ii, meid, new BCW.BLL.User().GetUsName(meid), wText);

            Utils.Success("开启宝箱", "恭喜你,开启宝箱," + cc + "", Utils.getUrl("farm.aspx?act=baoxiang"), "3");
        }
        else
            Utils.Error("抱歉,宝箱钥匙不足.马上去<a href=\"" + Utils.getUrl("farm.aspx?act=buycase&amp;ptype=2&amp;id=22&amp;vb=1") + "\">商店购买</a>吧.", "");
    }


    //交易市场
    private void marketPage()
    {
        //交易市场维护提示
        if (ub.GetSub("scStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;交易市场");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        int ptype2 = int.Parse(Utils.GetRequest("ptype2", "all", 1, @"^[1-2]$", "1"));

        builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
        if (ptype == 1)
        {
            Master.Title = "交易市场.所有摊位";
            builder.Append("所有摊位" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market&amp;ptype=1") + "\">所有摊位</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "交易市场.我的摊位";
            builder.Append("我的摊位" + "" + "");//<h style=\"color:red\">
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_tw") + "\">我的摊位</a>");
        }
        builder.Append(Out.Tab("</div>", ""));

        string name = (Utils.GetRequest("name", "all", 1, @"^[^\^]{1,10}$", ""));//10字之内

        int pageIndex;
        int recordCount;

        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "name", "ptype2", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strOrder = "";

        string strText = "输入物品名称进行搜索:/,,,";
        string strName = "name,act,ptype,backurl";
        string strType = "text,hidden,hidden,hidden";
        string strValu = "" + name + "'market'" + ptype + "'" + Utils.getPage(0) + "";
        string strEmpt = "true,false,false,false";
        string strIdea = "";
        string strOthe = "搜一搜/,farm.aspx,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        if (ptype2 == 1)
        {
            builder.Append("最新" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market&amp;ptype=1&amp;ptype2=1&amp;name=" + name + "") + "\">最新</a>" + "|");
        }
        if (ptype2 == 2)
        {
            builder.Append("金币" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market&amp;ptype=1&amp;ptype2=2&amp;name=" + name + "") + "\">金币</a>" + "");
        }
        builder.Append("<br/>你有金币:" + new BCW.farm.BLL.NC_user().GetGold(meid) + "");
        builder.Append(Out.Tab("</div>", Out.Hr()));

        //查询条件//ID链接到摊位
        if (ptype == 1)
        {
            if (ptype2 == 1)
            {
                strWhere = "daoju_name LIKE '%" + name + "%' and type=1";
                strOrder = " add_time Desc";
            }
            else
            {
                strWhere = "daoju_name LIKE '%" + name + "%' and type=1";
                strOrder = " daoju_price ASC";
            }

            IList<BCW.farm.Model.NC_market> listFarm = new BCW.farm.BLL.NC_market().GetNC_markets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_market n in listFarm)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    //builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", ""));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.Append("<b>" + n.daoju_name + "x" + n.daoju_num + "</b><br/>");
                    try
                    {
                        builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");//根据道具id，查询对应的图片路径
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }//market_Ta
                    builder.Append("价格：" + n.daoju_price + "金币/个<br/>所属：<a href=\"" + Utils.getUrl("farm.aspx?act=market_Ta&amp;usid=" + n.usid + "") + "\">" + new BCW.BLL.User().GetUsName(n.usid) + "(" + n.usid + ")</a><br/>");
                    builder.Append("时间：[" + n.add_time + "]<br/>");
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_buy&amp;a=" + n.ID + "&amp;b=" + n.usid + "") + "\">[购买]</a>");
                    builder.Append(Out.Tab("", Out.Hr()));
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有摊位记录..<br/>"));
            }
        }

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">我的道具>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }
    //交易市场购买
    private void market_buyPage()
    {
        //交易市场维护提示
        if (ub.GetSub("scStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "交易市场.购买物品";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=market") + "\">交易市场</a>&gt;购买物品");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int id = Utils.ParseInt(Utils.GetRequest("a", "all", 2, @"^[1-9]\d*$", "选择的道具出错"));//数据库的ID
        int usid = Utils.ParseInt(Utils.GetRequest("b", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        string info = Utils.GetRequest("info", "all", 1, "", "");

        //是否存在摆摊记录
        if (new BCW.farm.BLL.NC_market().Exists_baitan(id, usid))
        {
            BCW.farm.Model.NC_market ii = new BCW.farm.BLL.NC_market().GetNC_market(id);//获得该ID对应的摊位信息
            if (meid != ii.usid)
            {
                if (info == "ok")
                {
                    //判断密码是否一致
                    string oPwd = Utils.GetRequest("pw", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                    string ordPwd = new BCW.BLL.User().GetUsPwd(meid);
                    if (!Utils.MD5Str(oPwd).Equals(ordPwd))
                    {
                        Utils.Error("密码不正确.", "");
                    }

                    //判断购买道具的ID和传过来的ID是否一致
                    int daoju_id = int.Parse(Utils.GetRequest("daoju_id", "all", 2, @"^[1-9]\d*$", "购买道具出错."));
                    if (daoju_id != ii.daoju_id)
                    {
                        Utils.Error("购买道具不一致.请重新选择道具", "");
                    }

                    //买进数量
                    int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "请输入需要卖出的正确个数."));
                    if (num > ii.daoju_num)
                    {
                        Utils.Error("购买数量不能大于现有数量.", "");
                    }

                    //判断金币是否充足
                    if (new BCW.farm.BLL.NC_user().GetGold(meid) < (num * ii.daoju_price))
                    {
                        Utils.Error("抱歉,你的金币不足.", "");
                    }
                    BCW.User.Users.IsFresh("Farmbt", 2);//防刷

                    //买家扣金币
                    new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -(num * ii.daoju_price), "在交易市场" + new BCW.BLL.User().GetUsName(ii.usid) + "的摊位购买" + num + "个" + ii.daoju_name + ",花费" + (num * ii.daoju_price) + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - (num * ii.daoju_price)) + "金币.", 7);//购买道具
                    //卖家加金币
                    new BCW.farm.BLL.NC_user().UpdateiGold(ii.usid, new BCW.BLL.User().GetUsName(ii.usid), (num * ii.daoju_price), "" + mename + "在我的摊位买走" + num + "个" + ii.daoju_name + ",获得" + (num * ii.daoju_price) + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (num * ii.daoju_price)) + "金币.", 7);//购买道具

                    //道具表增加
                    if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(ii.daoju_id, meid, 0))
                    {
                        new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, num, ii.daoju_id, 0);
                    }
                    else
                    {
                        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(ii.daoju_id, meid, 0))
                        {
                            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, num, ii.daoju_id, 0);
                        }
                        else
                        {
                            BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(ii.daoju_id);
                            BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                            w.name = t.name;
                            w.name_id = 0;
                            w.num = num;
                            w.type = 2;
                            w.usid = meid;
                            w.zhonglei = 0;
                            w.huafei_id = ii.daoju_id;
                            w.picture = t.picture;
                            w.iszengsong = 0;
                            new BCW.farm.BLL.NC_mydaoju().Add(w);
                        }
                    }

                    //摆摊表减少,如果num=0,摊位撤销 
                    new BCW.farm.BLL.NC_market().Update_twdj(ii.usid, -num, id);
                    BCW.farm.Model.NC_market hj = new BCW.farm.BLL.NC_market().Get_djsl(ii.usid, id);
                    string tishi = string.Empty;
                    if (hj.daoju_num == 0)
                    {
                        new BCW.farm.BLL.NC_market().update_market("type=0", "usid=" + ii.usid + " and id=" + id + " ");
                        tishi = "出售数量为0，已自动取消摊位.";
                    }
                    //动态记录
                    new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx?act=market]" + GameName + "[/url]的摊位买走" + ii.daoju_name + ".");
                    //内线
                    new BCW.BLL.Guest().Add(1, ii.usid, new BCW.BLL.User().GetUsName(ii.usid), "" + mename + "在[url=/bbs/game/farm.aspx?act=market_tw]我的摊位[/url]买走" + num + "个" + ii.daoju_name + ".得到" + (num * ii.daoju_price) + "金币.");
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自由市场" + new BCW.BLL.User().GetUsName(ii.usid) + "(" + usid + ")的摊位购买" + num + "个" + ii.daoju_name + ".", 7);//内线消息
                    new BCW.farm.BLL.NC_messagelog().addmessage(ii.usid, new BCW.BLL.User().GetUsName(ii.usid), "" + mename + "(" + meid + ")在我的摊位买走" + num + "个" + ii.daoju_name + "." + tishi + "", 7);//内线消息

                    Utils.Success("购买道具", "购买" + num + "个" + ii.daoju_name + "道具成功,花费" + num * ii.daoju_price + "金币.", Utils.getUrl("farm.aspx?act=market"), "2");
                }
                else
                {
                    try
                    {
                        builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(ii.daoju_id) + "\" alt=\"load\"/><br/>");
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("[名称]：" + ii.daoju_name + "<br/>");
                    builder.Append("[数量]：" + ii.daoju_num + "个<br/>");
                    builder.Append("[售价]：" + ii.daoju_price + "金币/个");
                    builder.Append(Out.Tab("</div>", ""));

                    string strText = "购买数量:,预防被盗，须验证登陆密码：/,,,,,";
                    string strName = "num,pw,daoju_id,a,b,act,info";
                    string strType = "num,num,hidden,hidden,hidden,hidden,hidden";
                    string strValu = "1''" + ii.daoju_id + "'" + id + "'" + usid + "'market_buy'ok";
                    string strEmpt = "true,true,false,false,false,false,false";
                    string strIdea = "/";
                    string strOthe = "确定购买,farm.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                    builder.Append(Out.Tab("<div></div>", Out.Hr()));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("你有金币:" + new BCW.farm.BLL.NC_user().GetGold(meid) + "");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
            else
                Utils.Error("抱歉,自己不能购买自己的东西.", "");
        }
        else
            Utils.Error("没有可以购买的摊位", "");

        foot_link();//底部链接

        foot_link2();
    }
    //我的摊位a
    private void market_twPage()
    {
        //交易市场维护提示
        if (ub.GetSub("scStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=market") + "\">交易市场</a>&gt;我的摊位");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //查询是否不允许摆摊——为0不能摆摊
        if (new BCW.farm.BLL.NC_user().Get_baitang(meid) == 0)
        {
            Utils.Error("抱歉,你没有摆摊权限,请联系客服.", "");
        }

        int d = Utils.ParseInt(Utils.GetRequest("d", "all", 1, @"^[1-9]\d*$", "0"));
        string info = Utils.GetRequest("info", "all", 1, "", "");

        //查询摊位是否过期，若过期，则tpye更新为0
        if (new BCW.farm.BLL.NC_market().Exists_baitan(1))
        {
            if (baitan_day > 0)
            {
                DataSet ds = new BCW.farm.BLL.NC_market().GetList("*", "add_time<DATEADD(day, -" + baitan_day + ", GETDATE()) AND type=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int ID = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                        int daoju_id = int.Parse(ds.Tables[0].Rows[i]["daoju_id"].ToString());
                        int daoju_num = int.Parse(ds.Tables[0].Rows[i]["daoju_num"].ToString());

                        new BCW.farm.BLL.NC_mydaoju().Update_hf(UsID, daoju_num, daoju_id, 0);//把道具加到自己的道具表
                        BCW.farm.Model.NC_daoju kk = new BCW.farm.BLL.NC_daoju().GetNC_daoju(daoju_id);//根据id查询道具信息
                        new BCW.farm.BLL.NC_market().update_market("type=0", "id=" + ID + "");
                        new BCW.farm.BLL.NC_messagelog().addmessage(UsID, new BCW.BLL.User().GetUsName(UsID), "出售时间已到期,摊位自动取消.退回" + daoju_num + "个" + kk.name + ".", 7);//内线消息
                    }
                }
            }
        }

        if (d != 0)
        {
            BCW.farm.Model.NC_mydaoju uu = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 0);
            if (info == "ok")
            {
                //判断密码是否一致
                //string oPwd = Utils.GetRequest("pw", "post", 2, @"^[A-Za-z0-9]{6,20}$", "密码限6-20位,必须由字母或数字组成");
                string oPwd = Utils.GetRequest("pw", "post", 2, @"^[A-Za-z0-9]{6,20}$", "请正确输入您的密码");
                string ordPwd = new BCW.BLL.User().GetUsPwd(meid);
                if (!Utils.MD5Str(oPwd).Equals(ordPwd))
                {
                    Utils.Error("密码不正确.", "");
                }
                //卖出数量
                int num = int.Parse(Utils.GetRequest("num", "all", 2, @"^[1-9]\d*$", "请输入需要卖出的正确个数"));
                if (num > 100)
                {
                    Utils.Error("最大一次卖出数量为100个.", "");
                }
                if (num > uu.num)
                    Utils.Error("抱歉,你道具不足.", "");
                //卖出单价
                long Price = Utils.ParseInt64(Utils.GetRequest("price", "all", 2, @"^[1-9]\d*$", "请输入正确单价"));

                //判断是否摆摊超过n个
                if (new BCW.farm.BLL.NC_market().Get_btcs(meid) < baitan_bignum)
                {
                    //防刷
                    BCW.User.Users.IsFresh("Farmbt", 2);//防刷
                    //摆摊扣税
                    if (new BCW.farm.BLL.NC_user().GetGold(meid) < Convert.ToInt32(baitan_koushui * Price))
                    {
                        Utils.Error("抱歉,你的金币不足.", "");
                    }
                    //扣税
                    if (baitan_koushui > 0)
                        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -Convert.ToInt32(baitan_koushui * Price), "在交易市场摆摊成功,缴纳税为" + Convert.ToInt32(baitan_koushui * Price) + ",目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - (Convert.ToInt32(baitan_koushui * Price))) + "金币.", 7);//扣税

                    //道具表减少
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, -num, d, 0);
                    //摆摊表增加 
                    BCW.farm.Model.NC_market ee = new BCW.farm.Model.NC_market();
                    BCW.farm.Model.NC_mydaoju rr = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, d, 0);//根据id去查询道具名称
                    ee.type = 1;
                    ee.daoju_id = d;
                    ee.daoju_num = num;
                    ee.daoju_price = Price;
                    ee.usid = meid;
                    ee.add_time = DateTime.Now;
                    ee.daoju_name = rr.name;
                    ee.sui = (decimal)baitan_koushui;
                    new BCW.farm.BLL.NC_market().Add(ee);
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自由市场摆摊成功,摊位出售的是" + rr.name + ".", 7);//内线消息
                    Utils.Success("操作摆摊", "摆摊成功,可以继续摆摊.", Utils.getUrl("farm.aspx?act=market_tw"), "2");
                }
                else
                {
                    Utils.Error("摊位数量不可以超过" + baitan_bignum + "个.", "");
                }
            }
            else
            {
                Master.Title = "交易市场.我的摊位";
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("你要交易的物品：" + uu.name + "<br/>");
                try
                {
                    builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(uu.huafei_id) + "\" alt=\"load\"/><br/>");//根据道具id，查询对应的图片路径
                }
                catch { builder.Append("[图片出错!]<br/>"); }
                builder.Append("您目前有总数量：" + uu.num + "");
                builder.Append(Out.Tab("</div>", ""));

                string strText = "卖出数量:,卖出单价:,预防被盗，须验证登陆密码：/,,,";
                string strName = "num,price,pw,d,act,info";
                string strType = "num,num,password,hidden,hidden,hidden";//密码type应该为password 邵广林 20160517
                string strValu = "1'''" + d + "'market_tw'ok";
                string strEmpt = "true,true,true,false,false,false";
                string strIdea = "/";
                string strOthe = "确定|取消摆摊,farm.aspx,post,1,red|blue";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
            }
        }
        else
        {
            builder.Append(Out.Tab("<div class=\"text\">", ""));
            int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
            if (ptype == 1)
            {
                Master.Title = "交易市场.我的摊位";
                builder.Append("我的摊位" + "" + "|");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_tw&amp;ptype=1") + "\">我的摊位</a>" + "|");
            }
            if (ptype == 2)
            {
                Master.Title = "交易市场.我的历史摊位";
                builder.Append("历史摊位" + "" + "");
            }
            else
            {
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_tw&amp;ptype=2") + "\">历史摊位</a>" + "");
            }
            builder.Append(Out.Tab("</div>", "<br />"));

            int pageIndex;
            int recordCount;
            string strWhere = " ";
            string[] pageValUrl = { "act", "ptype", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strOrder = "";

            if (ptype == 1)
            {
                strWhere = "usid=" + meid + " and type=1";
                strOrder = " add_time Desc";
            }
            else
            {
                strWhere = "usid=" + meid + " and type=0";
                strOrder = " add_time Desc";
            }

            IList<BCW.farm.Model.NC_market> listFarm = new BCW.farm.BLL.NC_market().GetNC_markets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
            if (listFarm.Count > 0)
            {
                int k = 1;
                foreach (BCW.farm.Model.NC_market n in listFarm)
                {
                    if (k % 2 == 0)
                        //builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                        builder.Append(Out.Tab("<div class=\"text\">", ""));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            //builder.Append(Out.Tab("<div>", "<br />"));
                            builder.Append(Out.Tab("<div>", ""));
                    }
                    builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                    builder.Append("名称：" + n.daoju_name + "<br/>");
                    try
                    {
                        builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");//根据道具id，查询对应的图片路径
                    }
                    catch { builder.Append("[图片出错!]<br/>"); }
                    if (ptype == 1)
                    {
                        builder.Append("数量：" + n.daoju_num + "<br/>");
                        builder.Append("价格：" + n.daoju_price + "金币/个<br/>");
                        builder.Append("摆摊时间：[" + n.add_time + "]<br/>");
                        builder.Append("到期时间：[" + n.add_time.AddDays(baitan_day) + "]<br/>");
                        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=NG_jiaoyi&amp;id=" + n.ID + "") + "\">[取消交易]</a>");
                    }
                    else
                    {
                        builder.Append("剩余数量：" + n.daoju_num + "");
                        if (n.daoju_num > 0)
                            builder.Append("(已退回)<br/>");
                        else
                            builder.Append("<br/>");
                        builder.Append("价格：" + n.daoju_price + "金币/个<br/>");
                        builder.Append("摆摊时间：[" + n.add_time + "]<br/>");
                        builder.Append("到期时间：[" + n.add_time.AddDays(baitan_day) + "]");
                    }
                    builder.Append(Out.Tab("", Out.Hr()));
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">继续添加摆摊物品>></a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                if (ptype == 1)
                {
                    builder.Append(Out.Div("div", "摊位上的物品：(空)<br/>空空如也的摊位,浪费啊!出售一点东西来赚金币吧!<br/>"));
                    builder.Append(Out.Tab("<div>", ""));
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">添加需要摆摊的物品>></a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
                else
                {
                    builder.Append(Out.Div("div", "没有摊位记录.."));
                    builder.Append(Out.Tab("<div>", Out.Hr()));
                    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku&amp;ptype=3") + "\">继续添加摆摊物品>></a>");
                    builder.Append(Out.Tab("</div>", ""));
                }
            }
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("温馨提示：<br/>");
        if (baitan_day > 0)
            builder.Append("1.每个ID最多摆" + baitan_bignum + "个摊位." + baitan_day + "天后自动取消.<br/>");
        else
            builder.Append("1.每个ID最多摆" + baitan_bignum + "个摊位.<br/>");
        builder.Append("2.每个摊位数量1-100个.<br/>3.每次摆摊按照卖出价格的" + baitan_koushui + "收取摊位费用.");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接

        foot_link2();
    }
    //他的摊位
    private void market_TaPage()
    {
        //交易市场维护提示
        if (ub.GetSub("scStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "all", 1, @"^[1-9]\d*$", "0"));
        Master.Title = "交易市场.Ta的摊位";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=market") + "\">交易市场</a>&gt;Ta的摊位");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //查询是否不允许摆摊——为0不能摆摊
        if (new BCW.farm.BLL.NC_user().Get_baitang(meid) == 0)
        {
            Utils.Error("抱歉,你没有摆摊权限,请联系客服.", "");
        }

        int pageIndex;
        int recordCount;
        string strWhere = " ";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strOrder = "";

        strWhere = "usid=" + usid + " and type=1";
        strOrder = " add_time Desc";

        IList<BCW.farm.Model.NC_market> listFarm = new BCW.farm.BLL.NC_market().GetNC_markets(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_market n in listFarm)
            {
                if (k % 2 == 0)
                    //builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    builder.Append(Out.Tab("<div class=\"text\">", ""));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        //builder.Append(Out.Tab("<div>", "<br />"));
                        builder.Append(Out.Tab("<div>", ""));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".");
                builder.Append("名称：" + n.daoju_name + "<br/>");
                try
                {
                    builder.Append("<img src=\"" + new BCW.farm.BLL.NC_daoju().Get_picture(n.daoju_id) + "\" alt=\"load\"/><br/>");//根据道具id，查询对应的图片路径
                }
                catch { builder.Append("[图片出错!]<br/>"); }
                builder.Append("数量：" + n.daoju_num + "<br/>");
                builder.Append("价格：" + n.daoju_price + "金币/个<br/>");
                builder.Append("时间：[" + n.add_time + "]<br/>");
                builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=market_buy&amp;a=" + n.ID + "&amp;b=" + n.usid + "") + "\">[购买]</a>");
                builder.Append(Out.Tab("", Out.Hr()));
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "Ta的摊位空空如也哦.."));
        }

        foot_link();//底部链接

        foot_link2();
    }
    //取消摆摊
    private void NG_jiaoyiPage()
    {
        //交易市场维护提示
        if (ub.GetSub("scStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int id = Utils.ParseInt(Utils.GetRequest("id", "get", 2, @"^[1-9]\d*$", "选择的道具出错"));//数据库的ID
        //是否存在摆摊记录
        if (new BCW.farm.BLL.NC_market().Exists_baitan(id, meid))
        {
            BCW.farm.Model.NC_market ii = new BCW.farm.BLL.NC_market().GetNC_market(id);//获得该ID对应的摊位信息
            //meid对应的道具表是否存在
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(ii.daoju_id, meid, 0))
            {
                //把道具加到自己的道具表
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, ii.daoju_num, ii.daoju_id, 0);
                //取消摆摊type=0
                new BCW.farm.BLL.NC_market().update_market("type=0", "usid=" + meid + " AND type=1 AND id=" + id + "");
                //内线消息
                new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在自由市场取消摆摊,摊位出售的是" + ii.daoju_name + ",退回" + ii.daoju_num + "个", 7);
                Utils.Success("操作摊位", "取消摊位成功.", Utils.getUrl("farm.aspx?act=market_tw"), "1");
            }
            else
            {
                Utils.Error("道具返回出错.", "");
            }
        }
        else
        {
            Utils.Success("操作摊位", "重复操作或没有可以取消的摊位", Utils.getUrl("farm.aspx?act=market_tw"), "1");
        }
    }

    //执行程序
    private void zhixing()
    {
        //查询摊位是否过期，若过期，则tpye更新为0
        //判断是否有摊位才执行
        if (new BCW.farm.BLL.NC_market().Exists_baitan(1))
        {
            if (baitan_day > 0)
            {
                DataSet ds = new BCW.farm.BLL.NC_market().GetList("*", "add_time<DATEADD(day, -" + baitan_day + ", GETDATE()) AND type=1");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int ID = int.Parse(ds.Tables[0].Rows[i]["id"].ToString());
                        int UsID = int.Parse(ds.Tables[0].Rows[i]["usid"].ToString());
                        int daoju_id = int.Parse(ds.Tables[0].Rows[i]["daoju_id"].ToString());
                        int daoju_num = int.Parse(ds.Tables[0].Rows[i]["daoju_num"].ToString());

                        new BCW.farm.BLL.NC_mydaoju().Update_hf(UsID, daoju_num, daoju_id, 0);//把道具加到自己的道具表
                        BCW.farm.Model.NC_daoju kk = new BCW.farm.BLL.NC_daoju().GetNC_daoju(daoju_id);//根据id查询道具信息
                        new BCW.farm.BLL.NC_market().update_market("type=0", "id=" + ID + "");
                        new BCW.farm.BLL.NC_messagelog().addmessage(UsID, new BCW.BLL.User().GetUsName(UsID), "出售时间已到期,摊位自动取消.退回" + daoju_num + "个" + kk.name + ".", 7);//内线消息
                    }
                }
            }
        }

        //查询奴隶是否过期，若过期，则tpye更新为0
        if (slave_day > 0)
            new BCW.farm.BLL.NC_slave().update_ziduan("tpye=0", "updatetime<DATEADD(day, -" + slave_day + ", GETDATE()) AND tpye=1");//usid=" + meid + " and 

        //银钥匙--暂定3天
        if (xianjing_day > 0)
        {
            DataSet daoju = new BCW.farm.BLL.NC_daoju_use().GetList("*", "updatetime<DATEADD(day, -" + xianjing_day + ", GETDATE()) AND type=1 and daoju_id=21");
            if (daoju != null && daoju.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < daoju.Tables[0].Rows.Count; j++)
                {
                    int usid = int.Parse(daoju.Tables[0].Rows[j]["usid"].ToString());
                    int tudi = int.Parse(daoju.Tables[0].Rows[j]["tudi"].ToString());
                    new BCW.farm.BLL.NC_tudi().update_tudi("xianjing=0", "usid = '" + usid + "' AND tudi='" + tudi + "'");
                    new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "usid=" + usid + " and tudi=" + tudi + " and daoju_id=21");

                    new BCW.BLL.Guest().Add(1, usid, new BCW.BLL.User().GetUsName(usid), "你农场第" + tudi + "块土地设置的陷阱超过有效期(" + xianjing_day + "天),已自动取消陷阱.[url=/bbs/game/farm.aspx?act=xianjing]马上设置陷阱[/url]");
                    new BCW.farm.BLL.NC_messagelog().addmessage(usid, new BCW.BLL.User().GetUsName(usid), "你农场第" + tudi + "块土地设置的陷阱超过有效期(" + xianjing_day + "天),已自动取消陷阱.", 11);
                }
            }
        }

        //查询双倍经验卡是否过期，若过期，则type为0----1天
        new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "updatetime<DATEADD(day, -1, GETDATE()) AND type=1 and daoju_id=25");

        //查询双倍经验卡是否过期，若过期，则type为0----7天
        new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "updatetime<DATEADD(day, -7, GETDATE()) AND type=1 and daoju_id=26");

        //查询双倍经验卡是否过期，若过期，则type为0----30天
        new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "updatetime<DATEADD(day, -30, GETDATE()) AND type=1 and daoju_id=27");

        //自动收割机--1天
        new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "updatetime<DATEADD(day, -1, GETDATE()) AND type=1 and daoju_id=23");

        //自动收割机--7天
        new BCW.farm.BLL.NC_daoju_use().update_type("type=0", "updatetime<DATEADD(day, -7, GETDATE()) AND type=1 and daoju_id=24");

        try
        {
            //更新各经验
            DataSet da = new BCW.farm.BLL.NC_user().GetList("*", "ID=1 AND datediff(DAY,updatetime,GETDATE())>0");
            if (da != null && da.Tables[0].Rows.Count > 0)
            {
                new BCW.farm.BLL.NC_user().update_zd("updatetime=getdate(),big_bozhong=0,big_bangmang=0,big_shihuai=0,big_shifei=0,big_zjcaozuo=0,big_zfcishu=0,big_cccishu=0,zengsongnum=0", "");
            }

            //更新任务状态---把过期的任务删除
            DataSet ds = new BCW.farm.BLL.NC_tasklist().GetList("*", "type=0 AND task_type=0 AND DateDiff(dd,task_time,getdate()-1)=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                {
                    int id = int.Parse(ds.Tables[0].Rows[j]["id"].ToString());
                    new BCW.farm.BLL.NC_tasklist().Delete(id);
                }
            }
        }
        catch
        { }

        //更新收获状态和耕地状态时间-草虫水的状态
        new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate(),isinsect=0,z_chongtime=getdate()", "updatetime<DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=1");

        //耕地时
        new BCW.farm.BLL.NC_tudi().update_tudi("iscao=0,z_caotime=getdate(),isinsect=0,z_chongtime=getdate(),iswater=0,z_shuitime=getdate()", "ischandi=2");

        //若同时存在ischandi=2和未成熟 邵广林 20160926
        DataSet cd = new BCW.farm.BLL.NC_tudi().GetList("*", "updatetime>DATEADD(MINUTE, -zuowu_time, GETDATE()) and ischandi=2");
        if (cd != null && cd.Tables[0].Rows.Count > 0)
        {
            for (int j = 0; j < cd.Tables[0].Rows.Count; j++)
            {
                int id = int.Parse(cd.Tables[0].Rows[j]["id"].ToString());
                int usid = int.Parse(cd.Tables[0].Rows[j]["usid"].ToString());
                int tudi = int.Parse(cd.Tables[0].Rows[j]["tudi"].ToString());
                new BCW.farm.BLL.NC_tudi().update_tudi("ischandi=1", "id=" + id + "");
                //通知52784有用户出错
                new BCW.BLL.Guest().Add(1, 52784, "森林仔555", "ID：" + usid + "的土地(" + tudi + ")出现未成熟有铲地的情况.已修复.");//内线提示我出错(测试用)
            }
        }

    }

    //等级与经验说明
    private void dengjiPage()
    {
        Master.Title = "等级与经验说明";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;等级与经验说明");
        builder.Append(Out.Tab("</div>", "<br />"));


        builder.Append(Out.Tab("<div class=\"text\">", ""));
        builder.Append("等级与经验说明:<br/>");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", ""));
        builder.Append("<b>经验:</b><br/>1、播种+" + zhong_jingyan + "经验(上限" + bz_big_jingyan + "经验).收获的经验根据作物而定(生长周期越长的作物经验越高).<br/>");
        builder.Append("2、帮自己农场的作物浇水、除草、除虫各+" + all_jingyan + "经验和+" + all_jinbi + "币(帮自己农场操作上限" + zj_big_jingyan + "经验).<br/>");
        builder.Append("3、帮好友农场的作物浇水、除草、除虫各+" + all_jingyan / 2 + "经验和+" + all_jinbi + "币(帮好友农场操作上限" + bm_big_jingyan + "经验).<br/>");
        builder.Append("4、铲地+" + chandi_jingyan + "经验(铲除未枯萎作物不加经验);施肥+" + shifei_jingyan + "经验(两者均无上限).<br/>");
        builder.Append("5、去好友农场种草/放虫每次+" + zcaofchong_jingyan + "经验和+" + zcaofchong_jinbi + "金币(上限" + sh_big_jingyan + "经验).<br/>");
        builder.Append("6、" + GameName + "等级升级时所需经验值计算公式为：<br/>&nbsp;&nbsp;&nbsp;&nbsp;0-20级为N*200点,20-40级为N*250点,41-50级为N*500点,51-60级为N*700点,61-70级为N*900点.N为下一等级.<br/>");
        builder.Append("7、各上限每天0点后更新.<br/>");
        builder.Append("<b>等级:</b><br/>1、等级的提高可以种植更多更高级的作物.<br/>2、可以扩建土地(从5级开始,每2级可以扩建1块土地).<br/>");
        builder.Append("<b>提示:</b><br/>1、给自己/好友浇水、除草、除虫,均可获得经验和金币奖励.<br/>2、邀请好友进入" + GameName + ",更有幸运获得种子和金币等的奖励.<br/>");
        builder.Append("3、作物成熟后,记得及时来收获哦,不然在缺水状态下,过了" + Convert.ToInt32(ub.GetSub("z_shui_numtime", xmlPath)) + "分钟后会减产哦!");
        if (xExpir_huafei > 0)
            builder.Append("<br/>4、每个ID每天可使用化肥上限" + xExpir_huafei + "次(0点后更新).");
        builder.Append(Out.Tab("</div>", ""));

        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=paihangban") + "\">等级排行榜>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接
        foot_link2();
    }

    //生成随机不重复的数
    private int[] GetRandomNum(int n, int min, int max)
    {
        Dictionary<int, int> dict = new Dictionary<int, int>();
        Random r = new Random();
        int[] result = new int[n];
        int num = 0;
        for (int i = 0; i < n; i++)
        {
            num = r.Next(min, max + 1);
            while (dict.ContainsKey(num))
            {
                num = r.Next(min, max + 1);
            }
            dict.Add(num, 1);
        }
        dict.Keys.CopyTo(result, 0);
        return result;
    }

    //随机得到(签到)的类型
    private int GetPtype()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, 21);
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //随机得到(签到)酷币、金币的基数
    private int GetPtype2()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, (Convert.ToInt32(ub.GetSub("qd_suiji", xmlPath)) + 1));//
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //随机得到宝箱的基数
    private int GetPtype4()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, (Convert.ToInt32(ub.GetSub("bx_suiji", xmlPath)) + 1));//
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //随机陷阱
    private int Get_xianjing()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, (xianjing_jilv + 1));//2为中
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //随机得到(宝箱)的基数
    private int GetPtype3()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, 4);//20160509 修改宝箱不出酷币 改换经验 ----1道具2金币3经验
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //铲地、收获随机奖励的基数
    private int GetPtype5()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, (Convert.ToInt32(ub.GetSub("cdsh_suiji", xmlPath)) + 1));//
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }
    //一键铲地、一键收获随机奖励的基数
    private int GetPtype6()
    {
        Random rac = new Random();
        int ptype = rac.Next(1, (Convert.ToInt32(ub.GetSub("cdsh1_suiji", xmlPath)) + 1));//
        System.Threading.Thread.Sleep(50);//延时
        return ptype;
    }

    //随机宝箱的道具
    protected int R(int x, int y)
    {
        //Random ran = new Random();
        Random ran = new Random(unchecked((int)DateTime.Now.Ticks));
        int RandKey = ran.Next(x, y);
        return RandKey;
    }

    //土地类型
    private string OutType(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "(普通)土地";
        else if (Types == 2)
            pText = "(红)土地";
        else if (Types == 3)
            pText = "(黑)土地";
        else if (Types == 4)
            pText = "(金)土地";
        return pText;
    }

    //道具种类
    private string OutType2(int Types)
    {
        string pText = string.Empty;
        if (Types == 1)
            pText = "普通化肥";
        else if (Types == 2)
            pText = "高速化肥";
        else if (Types == 3)
            pText = "高速化肥礼包";
        else if (Types == 4)
            pText = "极速化肥";
        else if (Types == 5)
            pText = "极速化肥礼包";
        else if (Types == 6)
            pText = "飞速化肥";
        else if (Types == 7)
            pText = "飞速化肥礼包";
        //else if (Types == 8)
        //    pText = "高速点券化肥";
        //else if (Types == 9)
        //    pText = "高速点卷化肥礼包";
        //else if (Types == 10)
        //    pText = "极速点券化肥";
        //else if (Types == 11)
        //    pText = "极速点卷化肥礼包";
        //else if (Types == 12)
        //    pText = "飞速点券化肥";
        //else if (Types == 13)
        //    pText = "飞速点卷化肥礼包";
        else if (Types == 14)
            pText = "普通有机化肥";
        else if (Types == 15)
            pText = "高速有机化肥";
        else if (Types == 16)
            pText = "高速有机化肥礼包";
        else if (Types == 17)
            pText = "极速有机化肥";
        else if (Types == 18)
            pText = "极速有机化肥礼包";
        else if (Types == 19)
            pText = "飞速有机化肥";
        else if (Types == 20)
            pText = "飞速有机化肥礼包";
        else if (Types == 21)
            pText = "银钥匙";
        else if (Types == 22)
            pText = "宝箱钥匙";
        else if (Types == 23)
            pText = "自动收割机(1天)";
        else if (Types == 24)
            pText = "自动收割机(7天)";
        else if (Types == 25)
            pText = "双倍经验卡(1天)";
        else if (Types == 26)
            pText = "双倍经验卡(7天)";
        else if (Types == 27)
            pText = "双倍经验卡(30天)";
        else if (Types == 28)
            pText = "种草卡";
        else if (Types == 29)
            pText = "放虫卡";

        return pText;
    }

    //铲地/收获奖励
    private void chandi_get(int Types)
    {
        if (Convert.ToInt32(ub.GetSub("cdsh_gl", xmlPath)) == 0)//0开
        {
            int aa = 0;//2为可以奖励
            if (Types == 1 || Types == 3)
            {
                aa = GetPtype5();
            }
            else if (Types == 2 || Types == 4)
            {
                aa = GetPtype6();
            }
            int meid = new BCW.User.Users().GetUsId();
            if (meid == 0)
                Utils.Login();
            string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
            if (aa == 2)
            {
                BCW.farm.Model.NC_shop bb = new BCW.farm.BLL.NC_shop().Getsd_suiji(Convert.ToInt32(new BCW.farm.BLL.NC_user().GetGrade(meid)));
                int Num1 = 1;
                string hg = string.Empty;
                int id = bb.num_id;
                hg = bb.name;
                if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, meid))
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                }
                else
                {
                    if (new BCW.farm.BLL.NC_mydaoju().Exists(id, meid))//如果道具表有了该种子
                    {
                        new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                    }
                    else
                    {
                        BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                        u.name = bb.name;
                        u.name_id = id;
                        u.num = Num1;
                        u.type = 1;
                        u.usid = meid;
                        u.zhonglei = bb.type;
                        u.huafei_id = 0;
                        try
                        {
                            string[] bv = bb.picture.Split(',');
                            u.picture = bv[4];//我的道具表加图片
                        }
                        catch
                        {
                            u.picture = "";
                        }
                        new BCW.farm.BLL.NC_mydaoju().Add(u);
                    }
                }
                if (Types == 1)
                {
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在铲地时，你人品大爆发，获得1个" + hg + ".", 4);
                    new BCW.BLL.Guest().Add(1, meid, mename, "在" + GameName + "铲地时，你人品大爆发，获得1个" + hg + ".[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
                }
                if (Types == 2)
                {
                    new BCW.BLL.Guest().Add(1, meid, mename, "在" + GameName + "一键铲地时，你人品大爆发，获得1个" + hg + ".[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在一键铲地时，你人品大爆发，获得1个" + hg + ".", 4);
                }
                if (Types == 3)
                {
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在收获时，你人品大爆发，获得1个" + hg + ".", 4);
                    new BCW.BLL.Guest().Add(1, meid, mename, "在" + GameName + "收获时，你人品大爆发，获得1个" + hg + ".[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
                }
                if (Types == 4)
                {
                    new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在一键收获时，你人品大爆发，获得1个" + hg + ".", 4);
                    new BCW.BLL.Guest().Add(1, meid, mename, "在" + GameName + "一键收获时，你人品大爆发，获得1个" + hg + ".[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
                }
            }
        }
    }

    //公告
    private void gonggaoPage()
    {
        Master.Title = "" + GameName + "_系统公告";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;系统公告");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int pageIndex;
        int recordCount;
        string strWhere = "type=0";
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
        string strOrder = "updatetime DESC";

        IList<BCW.farm.Model.NC_gonggao> listFarm = new BCW.farm.BLL.NC_gonggao().GetNC_gonggaos(pageIndex, pageSize, strWhere, strOrder, out recordCount);
        if (listFarm.Count > 0)
        {
            int k = 1;
            foreach (BCW.farm.Model.NC_gonggao n in listFarm)
            {
                if (k % 2 == 0)
                    builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<b>" + n.title + "</b><br/>" + Out.SysUBB(n.contact) + "<br/>[" + n.updatetime + "]");
                k++;
                builder.Append(Out.Tab("</div>", ""));
            }
            // 分页
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "没有相关公告.."));
        }

        foot_link();//底部链接

        foot_link2();
    }

    //任务活动
    private void taskPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //任务维护提示
        if (ub.GetSub("rwStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        Master.Title = "" + GameName + "_任务与活动";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;任务与活动");
        builder.Append(Out.Tab("</div>", "<br/>"));

        builder.Append(Out.Tab("<div class=\"text\">", ""));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        if (ptype == 1)
        {
            Master.Title = "日常任务";
            builder.Append("日常任务" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=1") + "\">日常任务</a>" + "|");
        }
        if (ptype == 2)
        {
            Master.Title = "主线任务";
            builder.Append("主线任务" + "" + "|");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=2") + "\">主线任务</a>" + "|");
        }
        if (ptype == 3)
        {
            Master.Title = "活动任务";
            builder.Append("活动任务" + "" + "");
        }
        else
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=3") + "\">活动任务</a>" + "");
        }
        builder.Append(Out.Tab("</div>", "<br />"));

        int pageIndex;
        int recordCount;
        int pageSize = 10;
        string[] pageValUrl = { "act", "ptype", "backurl" };
        pageIndex = Utils.ParseInt(Request.QueryString["page"]);
        if (pageIndex == 0)
            pageIndex = 1;
        int aa = 0;
        string bb = string.Empty;

        if (ptype == 1)
        {
            aa = 0;
            bb = "暂时没有日常任务!敬请期待!";
        }
        else if (ptype == 2)
        {
            aa = 1;
            bb = "暂时没有主线任务!敬请期待!";
        }
        else
        {
            aa = 2;
            bb = "暂时没有活动任务!敬请期待!";
        }

        DataSet ds = new BCW.farm.BLL.NC_task().GetList("*", "task_type=" + aa + " and xiajia=0");
        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            recordCount = ds.Tables[0].Rows.Count;
            int k = 1;
            int koo = (pageIndex - 1) * pageSize;
            int skt = pageSize;
            if (recordCount > koo + pageSize)
            {
                skt = pageSize;
            }
            else
            {
                skt = recordCount - koo;
            }
            for (int i = 0; i < skt; i++)
            {
                if (k % 2 == 0)
                    //builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    builder.Append(Out.Tab("<div>", "<br />"));
                else
                {
                    if (k == 1)
                        builder.Append(Out.Tab("<div>", ""));
                    else
                        builder.Append(Out.Tab("<div>", "<br />"));
                }
                int id = int.Parse(ds.Tables[0].Rows[koo + i]["task_id"].ToString());
                string name = ds.Tables[0].Rows[koo + i]["task_name"].ToString().Trim();//名称
                builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=taskok&amp;id=" + id + "&amp;ptype=" + ptype + "") + "\">" + name + "</a>");
                if (ptype == 1)
                {
                    if (new BCW.farm.BLL.NC_tasklist().Exists_usid(meid, id))
                        builder.Append("(今天已完成)");
                }
                if (ptype == 2)
                {
                    if (new BCW.farm.BLL.NC_tasklist().Exists_usid2(meid, id))
                    {
                        builder.Append("(已完成)");
                    }
                }
                if (ptype == 3)
                {
                    if (new BCW.farm.BLL.NC_tasklist().Exists_usid3(meid, id))
                    {
                        builder.Append("(今天已完成)");
                    }
                }
                BCW.farm.Model.NC_tasklist cc = new BCW.farm.BLL.NC_tasklist().Get_renwu(id, meid);
                if (cc.usid > 0)
                    builder.Append("(正在进行的任务)");

                builder.Append(Out.Tab("</div>", ""));
                k++;
            }
            builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
        }
        else
        {
            builder.Append(Out.Div("div", "" + bb + ""));
        }

        foot_link();//底部链接

        foot_link2();
    }

    //领取或完成任务
    private void taskokPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //任务维护提示
        if (ub.GetSub("rwStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }

        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=task") + "\">任务与活动</a>&gt;任务详情");
        builder.Append(Out.Tab("</div>", "<br/>"));
        int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID无效"));
        int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-3]$", "1"));
        string info = Utils.GetRequest("info", "all", 1, "", "");
        if (info == "ok1")
        {
            #region 判断接受任务
            int idd = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID无效"));

            //判断是否有这个任务
            if (new BCW.farm.BLL.NC_task().Exists(idd))
            {
                //根据任务id查询相应信息
                BCW.farm.Model.NC_task aa = new BCW.farm.BLL.NC_task().GetNC_task(id);
                if (aa.task_type == 0)//日常
                {
                    #region
                    //判断是否每天一次---查询今天DateDiff(dd,Input_Time,getdate())=0
                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid(meid, idd))
                    {
                        //判断是否已领取了这个任务
                        BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(idd, meid);
                        if (bb.usid == 0)
                        {
                            BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                            ab.task_id = idd;
                            ab.task_oknum = 0;
                            ab.task_oktime = aa.task_time;
                            ab.task_type = 0;
                            ab.usid = meid;
                            ab.task_time = DateTime.Now;
                            new BCW.farm.BLL.NC_tasklist().Add(ab);
                            Utils.Success("领取任务", "领取任务成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;id=" + idd + ""), "1");
                        }
                        else
                            Utils.Error("不能重复领取任务.", "");
                    }
                    else
                        Utils.Error("每天只能领取一次.请明天再来吧.", "");
                    #endregion
                }
                else if (aa.task_type == 1)//主线
                {
                    #region
                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid1(meid, idd))
                    {
                        //判断是否已领取了这个任务
                        BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(idd, meid);
                        if (bb.usid == 0)
                        {
                            BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                            ab.task_id = idd;
                            ab.task_oknum = aa.task_grade;
                            ab.task_oktime = DateTime.Now;
                            ab.task_type = 1;
                            ab.usid = meid;
                            ab.task_time = DateTime.Now;
                            new BCW.farm.BLL.NC_tasklist().Add(ab);
                            Utils.Success("领取任务", "领取任务成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;id=" + idd + ""), "1");
                        }
                        else
                            Utils.Error("不能重复领取任务.", "");
                    }
                    else
                        Utils.Error("该任务只能领取一次.", "");
                    #endregion
                }
                else//活动
                {
                    #region
                    if (idd == 11)//VIP
                    {
                        #region
                        //记得判断时间是否符合任务范围内
                        //识别会员vip
                        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                        DateTime viptime = DateTime.Now;
                        try
                        {
                            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                        }
                        catch
                        {
                            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                        }
                        if (DateTime.Now < viptime)//会员
                        {
                            if (!new BCW.farm.BLL.NC_tasklist().Exists_usid3(meid, idd))
                            {
                                //判断是否已领取了这个任务
                                BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(idd, meid);
                                if (bb.usid == 0)
                                {
                                    BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                    ab.task_id = idd;
                                    ab.task_oknum = 0;
                                    ab.task_oktime = aa.task_time;
                                    ab.task_type = 2;
                                    ab.usid = meid;
                                    ab.task_time = DateTime.Now;
                                    new BCW.farm.BLL.NC_tasklist().Add(ab);
                                    Utils.Success("领取任务", "领取任务成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;id=" + idd + ""), "1");
                                }
                                else
                                    Utils.Error("不能重复领取任务.", "");
                            }
                            else
                                Utils.Error("每天只能领取一次.请明天再来吧.", "");
                        }
                        else
                        {
                            Utils.Error("抱歉,你还没开通VIP.<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">马上开通>></a>", "");
                        }
                        #endregion
                    }
                    else if (idd == 12)//大礼包
                    {
                        #region
                        //记得判断时间是否符合任务范围内
                        if (!new BCW.farm.BLL.NC_tasklist().Exists_usid3(meid, idd))
                        {
                            //判断是否已领取了这个任务
                            BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(idd, meid);
                            if (bb.usid == 0)
                            {
                                BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                ab.task_id = idd;
                                ab.task_oknum = 0;
                                ab.task_oktime = aa.task_time;
                                ab.task_type = 2;
                                ab.usid = meid;
                                ab.task_time = DateTime.Now;
                                new BCW.farm.BLL.NC_tasklist().Add(ab);
                                Utils.Success("领取任务", "领取任务成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;id=" + idd + ""), "1");
                            }
                            else
                                Utils.Error("不能重复领取任务.", "");
                        }
                        else
                            Utils.Error("每天只能领取一次.请明天再来吧.", "");
                        #endregion
                    }
                    else if (idd == 13)
                    {
                        #region
                        //记得判断时间是否符合任务范围内
                        if (aa.task_time.AddDays(-90).AddSeconds(1) < DateTime.Now && aa.task_time > DateTime.Now)
                        {
                            string ac = Utils.GetRequest("ac", "all", 1, "", "");
                            DataSet dd = new BCW.BLL.Goldlog().GetList("-sum(AcGold)as a", "UsId=" + meid + " AND AcText LIKE '%您在农场%'  AND DateDiff(dd,AddTime,getdate())=0 AND AcGold<0");
                            long qian = 0;
                            try
                            {
                                qian = Convert.ToInt64(dd.Tables[0].Rows[0]["a"].ToString());
                            }
                            catch { }
                            BCW.User.Users.IsFresh("Farmrw", 3);//防刷
                            if (Utils.ToSChinese(ac) == "达到12500")
                            {
                                if (qian >= 12500)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 1))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 1;//第一个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(1);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else if (Utils.ToSChinese(ac) == "达到25000")
                            {
                                if (qian >= 25000)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 2))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 2;//第二个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(21);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else if (Utils.ToSChinese(ac) == "达到75000")
                            {
                                if (qian >= 75000)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 3))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 3;//第三个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(2);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else if (Utils.ToSChinese(ac) == "达到125000")
                            {
                                if (qian >= 125000)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 4))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 4;//第四个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(28);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else if (Utils.ToSChinese(ac) == "达到250000")
                            {
                                if (qian >= 250000)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 5))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 5;//第五个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(29);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else if (Utils.ToSChinese(ac) == "达到375000")
                            {
                                if (qian >= 375000)
                                {
                                    if (!new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, idd, 6))
                                    {
                                        //接任务
                                        BCW.farm.Model.NC_tasklist ab = new BCW.farm.Model.NC_tasklist();
                                        ab.task_id = idd;
                                        ab.task_oknum = 6;//第六个按钮
                                        ab.task_oktime = aa.task_time;
                                        ab.task_type = 2;
                                        ab.usid = meid;
                                        ab.type = 1;//标记已完成
                                        ab.task_time = DateTime.Now;
                                        new BCW.farm.BLL.NC_tasklist().Add(ab);
                                        //即刻拿奖励
                                        xiaofeihuikui(22);
                                        Utils.Success("领取回馈成功", "领取回馈成功,正在返回.", Utils.getUrl("farm.aspx?act=taskok&amp;ptype=3&amp;id=13"), "1");
                                    }
                                    else
                                        Utils.Error("每月只能领取一次.请下个月再来吧.", "");
                                }
                                else
                                    Utils.Error("抱歉,你消费还没达到要求.当前已消费" + qian + "", "");
                            }
                            else
                                Utils.Error("选择按钮出错.", "");
                        }
                        else
                            Utils.Error("抱歉,不在活动时间内.", "");
                        #endregion
                    }
                    else
                    {
                        Utils.Error("请选择正确的活动任务.", "");
                    }
                    #endregion
                }
            }
            else
                Utils.Error("请选择正确的任务.", "");
            #endregion
        }
        else if (info == "ok2")
        {
            #region 完成任务
            int idd = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID无效"));
            BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(idd, meid);
            BCW.farm.Model.NC_task cc = new BCW.farm.BLL.NC_task().GetNC_task(idd);
            if (cc.ID != 0)
            {
                if (cc.task_type == 0)//日常
                {
                    #region
                    if (bb.usid > 0)
                    {
                        if (bb.task_oknum >= cc.task_num)
                        {
                            BCW.User.Users.IsFresh("Farmrw", 1);//防刷
                                                                //更新字段
                            new BCW.farm.BLL.NC_tasklist().update_renwu("type=1,task_oktime=getdate()", "usid=" + meid + " and task_id=" + idd + "");
                            //奖励----
                            if (cc.task_type == 0)
                            {
                                //随机一个等级以下的种子
                                suiji_zhongzi(idd, 1, 5, "");
                                //得到N*5的经验
                                if (new BCW.farm.BLL.NC_user().GetGrade(meid) > 0)
                                {
                                    new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 5));//+经验
                                }
                                else
                                {
                                    new BCW.farm.BLL.NC_user().Update_Experience(meid, (1 * 5));//+经验
                                }
                                //等级操作
                                dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                            }
                            Utils.Success("完成任务", "提交任务成功,正在返回.", Utils.getUrl("farm.aspx?act=task"), "1");
                        }
                        else
                            Utils.Error("该任务还没完成,继续努力哦.", "");
                    }
                    else
                        Utils.Error("该任务不存在.请重新领取.", "");
                    #endregion
                }
                else if (cc.task_type == 1)//主线
                {
                    #region
                    if (new BCW.farm.BLL.NC_user().GetGrade(meid) >= bb.task_oknum)//等级达到
                    {
                        #region
                        BCW.User.Users.IsFresh("Farmrw", 1);//防刷
                        new BCW.farm.BLL.NC_tasklist().update_renwu("type=1,task_oktime=getdate()", "usid=" + meid + " and task_id=" + idd + "");//更新字段
                        //奖励
                        if (cc.task_type == 1)
                        {
                            if (bb.task_id == 4)//2-5-6
                            {
                                suiji_zhongzi(idd, 2, 6, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 6));
                            }
                            else if (bb.task_id == 5)//4-15-7
                            {
                                suiji_zhongzi(idd, 4, 7, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 7));
                            }
                            else if (bb.task_id == 6)//6-25-8
                            {
                                suiji_zhongzi(idd, 6, 8, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 8));
                            }
                            else if (bb.task_id == 7)//8-35-9
                            {
                                suiji_zhongzi(idd, 8, 9, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 9));
                            }
                            else if (bb.task_id == 8)//10-45-10
                            {
                                suiji_zhongzi(idd, 10, 10, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 10));
                            }
                            else if (bb.task_id == 9)//12-55-12
                            {
                                suiji_zhongzi(idd, 12, 12, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 12));
                            }
                            else if (bb.task_id == 10)//14-65-14
                            {
                                suiji_zhongzi(idd, 14, 14, "");
                                new BCW.farm.BLL.NC_user().Update_Experience(meid, (new BCW.farm.BLL.NC_user().GetGrade(meid) * 14));
                            }


                            //等级操作
                            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
                        }
                        Utils.Success("完成任务", "提交任务成功,正在返回.", Utils.getUrl("farm.aspx?act=task&amp;ptype=2"), "1");
                        #endregion
                    }
                    else
                    {
                        Utils.Error("抱歉,你等级还不够,无法完成该任务.", "");
                    }
                    #endregion
                }
                else//活动
                {
                    if (bb.task_id == 11)
                    {
                        #region
                        //识别会员vip
                        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                        DateTime viptime = DateTime.Now;
                        try
                        {
                            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                        }
                        catch
                        {
                            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                        }
                        if (DateTime.Now < viptime)//会员
                        {
                            BCW.User.Users.IsFresh("Farmrw", 1);//防刷
                            //更新字段
                            new BCW.farm.BLL.NC_tasklist().update_renwu("type=1,task_oktime=getdate()", "usid=" + meid + " and task_id=" + idd + "");
                            VIPPrize(idd, BCW.User.Users.VipLeven(meid));
                            Utils.Success("完成任务", "提交任务成功,正在返回.", Utils.getUrl("farm.aspx?act=task&amp;ptype=3"), "1");
                        }
                        else
                        {
                            Utils.Error("抱歉,你还没开通VIP.<a href=\"" + Utils.getUrl("../finance.aspx?act=addvip") + "\">马上开通>></a>", "");
                        }
                        #endregion
                    }
                    else if (bb.task_id == 12)
                    {
                        #region
                        BCW.User.Users.IsFresh("Farmrw", 1);//防刷
                        //更新字段
                        new BCW.farm.BLL.NC_tasklist().update_renwu("type=1,task_oktime=getdate()", "usid=" + meid + " and task_id=" + idd + "");
                        DaLiBao();
                        Utils.Success("完成任务", "提交任务成功,正在返回.", Utils.getUrl("farm.aspx?act=task&amp;ptype=3"), "1");
                        #endregion
                    }
                    else
                    {
                        Utils.Error("请选择正确的活动任务.", "");
                    }
                }
            }
            else
                Utils.Error("请选择正确任务.", "");
            #endregion
        }
        else
        {
            #region 任务详情
            if (new BCW.farm.BLL.NC_task().Exists(id))
            {
                BCW.farm.Model.NC_task aa = new BCW.farm.BLL.NC_task().GetNC_task(id);
                BCW.farm.Model.NC_tasklist bb = new BCW.farm.BLL.NC_tasklist().Get_renwu(id, meid);//根据usid查询任务状态
                Master.Title = "任务：" + aa.task_name + "";
                builder.Append(Out.Tab("<div>", ""));
                builder.Append("任务名称：" + aa.task_name + "<br/>");
                builder.Append("任务内容：" + aa.task_contact + "<br/>");
                builder.Append("任务奖励：" + aa.task_jiangli + "<br/>");
                if (aa.task_type == 0)//日常任务
                {
                    builder.Append("等级要求：0-" + aa.task_grade + "级<br/>");
                    builder.Append("完成情况：" + bb.task_oknum + "/" + aa.task_num + "<br/>");
                }
                else if (aa.task_type == 1)//主线任务
                {
                    builder.Append("等级要求：" + aa.task_grade + "级<br/>");
                    builder.Append("完成情况：" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "(级)/" + aa.task_grade + "(级)<br/>");
                }
                else//活动任务
                {
                    if (aa.task_id == 11)//vip每天领取
                    {
                        #region
                        builder.Append("是否会员：");
                        //识别会员vip
                        DataSet rows = new BCW.BLL.User().GetList("VipDate", "ID=" + meid);
                        DateTime viptime = DateTime.Now;
                        try
                        {
                            viptime = Convert.ToDateTime(rows.Tables[0].Rows[0][0]);
                        }
                        catch
                        {
                            viptime = Convert.ToDateTime("2010-01-01 12:00:00");
                        }
                        if (DateTime.Now < viptime)//会员
                        {
                            builder.Append("是。当前VIP等级为：" + BCW.User.Users.VipLeven(meid) + "级<br/>");
                            builder.Append("活动时间：" + aa.task_time.AddDays(-31).AddSeconds(1) + "--" + aa.task_time + "");
                        }
                        else
                        {
                            builder.Append("<b style=\"color:red\">不是</b><br/>");
                            builder.Append("活动时间：" + aa.task_time.AddDays(-31).AddSeconds(1) + "--" + aa.task_time + "");
                        }
                        #endregion
                    }
                    if (aa.task_id == 12)//大礼包
                    {
                        builder.Append("活动时间：" + aa.task_time.AddDays(-7).AddSeconds(1) + "--" + aa.task_time + "");
                    }
                    if (aa.task_id == 13)//消费回馈
                    {
                        builder.Append("活动时间：" + aa.task_time.AddDays(-90).AddSeconds(1) + "--" + aa.task_time + "<br/>");
                        //查询用户当月消费
                        DataSet dd = new BCW.BLL.Goldlog().GetList("-sum(AcGold)as a", "UsId=" + meid + " AND AcText LIKE '%您在农场%'  AND DateDiff(dd,AddTime,getdate())=0 AND AcGold<0");
                        long qian = 0;
                        try
                        {
                            qian = Convert.ToInt64(dd.Tables[0].Rows[0]["a"].ToString());
                        }
                        catch { }
                        builder.Append("当月消费：<h style=\"color:red\">" + qian + ub.Get("SiteBz") + "</h>");
                    }
                }
                if (id == 1 || id == 2 || id == 3)
                {
                    builder.Append("任务时间：只限当天有效.<br/>");
                }

                if (bb.usid == 0)
                {
                    string strText = ",,,";
                    string strName = "act,id,info";
                    string strType = "hidden,hidden,hidden";
                    string strValu = "taskok'" + id + "'ok1";
                    string strEmpt = "false,false,false";
                    string strIdea = "/";
                    string strOthe = "领取任务,farm.aspx,post,1,red";

                    if (aa.task_type == 0)
                    {
                        if (new BCW.farm.BLL.NC_tasklist().Exists_usid(meid, id))
                        {
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            builder.Append("<b style=\"color:red\">(温馨提示：今天已完成)</b>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else if (aa.task_type == 1)
                    {
                        if (new BCW.farm.BLL.NC_tasklist().Exists_usid1(meid, id))
                        {
                            builder.Append(Out.Tab("<div>", "<br/>"));
                            builder.Append("<b style=\"color:red\">(温馨提示：该任务已完成)</b>");
                            builder.Append(Out.Tab("</div>", ""));
                        }
                        else
                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                    }
                    else
                    {
                        if (aa.task_id == 11 || aa.task_id == 12)
                        {
                            if (new BCW.farm.BLL.NC_tasklist().Exists_usid3(meid, id))
                            {
                                builder.Append(Out.Tab("<div>", "<br/>"));
                                builder.Append("<b style=\"color:red\">(温馨提示：今日已完成)</b>");
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                if (aa.task_id == 11)
                                {
                                    #region
                                    if ((aa.task_time.AddDays(-31).AddSeconds(1) < DateTime.Now))
                                    {
                                        if (aa.task_time < DateTime.Now)
                                        {
                                            builder.Append("<br/><b style=\"color:red\">活动已结束！</b>");
                                        }
                                        else
                                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                                    }
                                    else
                                    {
                                        builder.Append("<br/><b style=\"color:red\">敬请期待！</b>");
                                    }
                                    #endregion
                                }
                                if (aa.task_id == 12)
                                {
                                    #region
                                    if (aa.task_time.AddDays(-7).AddSeconds(1) < DateTime.Now)
                                    {
                                        if (aa.task_time < DateTime.Now)
                                        {
                                            builder.Append("<br/><b style=\"color:red\">活动已结束！</b>");
                                        }
                                        else
                                            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                                    }
                                    else
                                    {
                                        builder.Append("<br/><b style=\"color:red\">敬请期待！</b>");
                                    }
                                    #endregion
                                }
                            }
                        }
                        if (aa.task_id == 13)
                        {
                            #region
                            if (new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 1) && new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 2) && new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 3) && new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 4) && new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 5) && new BCW.farm.BLL.NC_tasklist().Exists_usid13(meid, id, 6))
                            {
                                builder.Append(Out.Tab("<div>", "<br/>"));
                                builder.Append("<b style=\"color:red\">(温馨提示：本月已完成)</b>");
                                builder.Append(Out.Tab("</div>", ""));
                            }
                            else
                            {
                                if (aa.task_time.AddDays(-90).AddSeconds(1) < DateTime.Now)//90
                                {
                                    if (aa.task_time < DateTime.Now)
                                    {
                                        builder.Append("<br/><b style=\"color:red\">活动已结束！</b>");
                                    }
                                    else
                                    {
                                        strOthe = "达到12500|达到25000|达到75000|达到125000|达到250000|达到375000,farm.aspx,post,1,red|blue|red|blue|red|blue";
                                        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                                    }
                                }
                                else
                                {
                                    builder.Append("<br/><b style=\"color:red\">敬请期待！</b>");
                                }
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    string strText = ",,,";
                    string strName = "act,id,info";
                    string strType = "hidden,hidden,hidden";
                    string strValu = "taskok'" + id + "'ok2";
                    string strEmpt = "false,false,false";
                    string strIdea = "/";
                    string strOthe = "完成任务,farm.aspx,post,1,red";
                    builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
                }
                builder.Append(Out.Tab("</div>", ""));
            }
            else
                Utils.Error("请选择正确任务.", "");
            #endregion
        }

        builder.Append(Out.Tab("<div>", Out.Hr()));
        if (ptype == 1)
        {
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">好友农场>></a><br/>");
        }
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=task&amp;ptype=" + ptype + "") + "\">返回上级>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();
        foot_link2();
    }

    //完成任务随机奖励种子-等级以下
    private void suiji_zhongzi(int task_id, int num_get, int jingyang, string contact)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        BCW.farm.Model.NC_shop bb = new BCW.farm.BLL.NC_shop().Getsd_suiji(Convert.ToInt32(new BCW.farm.BLL.NC_user().GetGrade(meid)));
        int Num1 = 0;
        string hg = string.Empty;
        if (num_get > 0)
        {
            Num1 = num_get;
            int id = bb.num_id;
            hg = bb.name;
            if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, meid))
            {
                new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists(id, meid))//如果道具表有了该种子
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                }
                else
                {
                    BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                    u.name = bb.name;
                    u.name_id = id;
                    u.num = Num1;
                    u.type = 1;
                    u.usid = meid;
                    u.zhonglei = bb.type;
                    u.huafei_id = 0;
                    try
                    {
                        string[] bv = bb.picture.Split(',');
                        u.picture = bv[4];//我的道具表加图片
                    }
                    catch
                    {
                        u.picture = "";
                    }
                    new BCW.farm.BLL.NC_mydaoju().Add(u);
                }
            }
        }


        //根据task_id拿到名字
        BCW.farm.Model.NC_task aaa = new BCW.farm.BLL.NC_task().GetNC_task(task_id);
        if (num_get > 0)
        {
            hg = "获得" + Num1 + "个" + hg;
        }
        if (jingyang > 0)
        {
            hg = hg + ".并奖励" + (new BCW.farm.BLL.NC_user().GetGrade(meid) * jingyang) + "经验.";
        }
        if (contact != "")
        {
            hg = hg + contact;
        }

        //动态记录
        new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), "在[url=/bbs/game/farm.aspx?act=task]" + GameName + "[/url]完成任务." + hg + "");
        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, "在完成[" + aaa.task_name + "]任务时," + hg + "", 1);
        //内线
        new BCW.BLL.Guest().Add(1, meid, mename, "在" + GameName + "完成[" + aaa.task_name + "]任务," + hg + "[url=/bbs/game/farm.aspx]马上去农场播种[/url]");
    }

    //VIP领取奖励--道具不可赠送
    private void VIPPrize(int task_id, int num)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        string wText = "";
        string dd = "";
        string suiji = "";
        Random rac = new Random();
        if (num == 1 || num == 2)//金币+种子
        {
            #region
            //随机(1-5)*100的金币+随机2个种子
            int money = R(1, 6) * 100;
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, money, "完成活动任务:VIP每天领取,获得" + money + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (money)) + "金币.", 6);//加金币
            wText = ",获得" + money + "金币.";
            suiji_zhongzi(11, 2, 0, wText);
            #endregion
        }
        else if (num == 3 || num == 4)//种子+道具
        {
            #region
            //随机3个种子+随机1个道具：1、2、28、29
            string[] sNum = { "1", "2", "28", "29" };
            suiji = (sNum[rac.Next(sNum.Length)]);
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(int.Parse(suiji), meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                dd = aaa.name;
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(int.Parse(suiji), meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                    dd = aaa.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
                }
                else
                {
                    //根据id判断
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(int.Parse(suiji));
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    dd = t.name;
                    w.name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = int.Parse(suiji);
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }
            }
            wText = ",获得1个" + dd + ".";
            suiji_zhongzi(11, 3, 0, wText);
            #endregion
        }
        else if (num == 5 || num == 6)//金币+道具
        {
            #region
            //随机(5-10)*100金币+随机1个道具：1、2、21、28、29
            int money = R(5, 11) * 100;
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, money, "完成活动任务:VIP每天领取,获得" + money + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (money)) + "金币.", 6);//加金币
            wText = "获得" + money + "金币.";
            string[] sNum = { "1", "2", "21", "28", "29" };
            suiji = (sNum[rac.Next(sNum.Length)]);
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(int.Parse(suiji), meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                dd = aaa.name;
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(int.Parse(suiji), meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                    dd = aaa.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
                }
                else
                {
                    //根据id判断
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(int.Parse(suiji));
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    dd = t.name;
                    w.name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = int.Parse(suiji);
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }
            }
            wText = wText + ",获得1个" + dd + ".";
            suiji_zhongzi(11, 0, 0, wText);
            #endregion
        }
        else if (num == 7 || num == 8)//金币+种子+道具
        {
            #region
            //1000金币+4个随机种子+随机1个道具：1、21、22、28、29
            int money = 1000;
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, money, "完成活动任务:VIP每天领取,获得" + money + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (money)) + "金币.", 6);//加金币
            wText = ",获得" + money + "金币.";
            string[] sNum = { "1", "1", "1", "21", "21", "22", "28", "28", "28", "29", "29", "29" };
            suiji = (sNum[rac.Next(sNum.Length)]);
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(int.Parse(suiji), meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                dd = aaa.name;
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(int.Parse(suiji), meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, int.Parse(suiji), 1);//查询化肥数量
                    dd = aaa.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, int.Parse(suiji), 1);
                }
                else
                {
                    //根据id判断
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(int.Parse(suiji));
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    dd = t.name;
                    w.name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = int.Parse(suiji);
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }
            }
            wText = wText + ",获得1个" + dd + ".";
            suiji_zhongzi(11, 4, 0, wText);
            #endregion
        }
    }

    //大礼包--道具不可赠送
    private void DaLiBao()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        //随机选出礼包种类
        //0-3经验4-9种子10-15金币16-18道具19-20酷币21-22鲜花23点值24等级
        int num = R(0, 25);
        string wText = "";
        if (num == 0 || num == 1 || num == 2 || num == 3)//经验
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "50", "50", "50", "100", "100", "100", "150", "150", "150", "200", "200", "200", "250", "250", "300", "300", "350,", "400", "450", "500" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));
            new BCW.farm.BLL.NC_user().Update_Experience(meid, aa);
            dengjisuoxu(meid);//邵广林 20160929 增加各等级升级所需的经验
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得丰厚经验奖励共" + aa + "经验.";
            #endregion
        }
        else if (num == 4 || num == 5 || num == 6 || num == 7 || num == 8 || num == 9)//种子
        {
            #region
            BCW.farm.Model.NC_shop bb = new BCW.farm.BLL.NC_shop().Getsd_suiji(Convert.ToInt32(new BCW.farm.BLL.NC_user().GetGrade(meid)));
            int Num1 = 0;
            string hg = string.Empty;

            Num1 = R(5, 11);
            int id = bb.num_id;
            hg = bb.name;
            if (new BCW.farm.BLL.NC_mydaoju().Exists2(id, meid))
            {
                new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists(id, meid))//如果道具表有了该种子
                {
                    new BCW.farm.BLL.NC_mydaoju().Update_zz(meid, Num1, id);
                }
                else
                {
                    BCW.farm.Model.NC_mydaoju u = new BCW.farm.Model.NC_mydaoju();
                    u.name = bb.name;
                    u.name_id = id;
                    u.num = Num1;
                    u.type = 1;
                    u.usid = meid;
                    u.zhonglei = bb.type;
                    u.huafei_id = 0;
                    try
                    {
                        string[] bv = bb.picture.Split(',');
                        u.picture = bv[4];//我的道具表加图片
                    }
                    catch
                    {
                        u.picture = "";
                    }
                    new BCW.farm.BLL.NC_mydaoju().Add(u);
                }
            }
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得" + Num1 + "个" + hg + ".";
            #endregion
        }
        else if (num == 10 || num == 11 || num == 12 || num == 13 || num == 14 || num == 15)//金币
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "100", "100", "200", "200", "300", "300", "400", "400", "500", "500", "600", "600", "700", "700", "800", "800", "900", "900", "1000", "1000", "1500", "2000", "2500", "3000" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));
            new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, aa, "恭喜你在" + GameName + "领取国庆大礼包中获得" + aa + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + (aa)) + "金币.", 6);//加金币
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得" + aa + "金币.";
            #endregion
        }
        else if (num == 16 || num == 17 || num == 18)//道具
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "1", "1", "1", "2", "2", "2", "14", "14", "15", "15", "21", "22", "23", "25", "28", "28", "28", "29", "29", "29" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));
            string name = "";
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(aa, meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, aa, 1);//查询化肥数量
                name = aaa.name;
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, aa, 1);
            }
            else
            {
                if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(aa, meid, 1))
                {
                    BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, aa, 1);//查询化肥数量
                    name = aaa.name;
                    new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, aa, 1);
                }
                else
                {
                    //根据id判断
                    BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(aa);
                    BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                    w.name = t.name;
                    name = t.name;
                    w.name_id = 0;
                    w.num = 1;
                    w.type = 2;
                    w.usid = meid;
                    w.zhonglei = 0;
                    w.huafei_id = aa;
                    w.picture = t.picture;
                    w.iszengsong = 1;
                    new BCW.farm.BLL.NC_mydaoju().Add(w);
                }
            }
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得1个" + name + ".";
            #endregion
        }
        else if (num == 19 || num == 20)//酷币
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "100", "100", "200", "200", "300", "300", "400", "400", "500", "500", "600", "600", "700", "700", "800", "900", "1000", "1100" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));

            new BCW.BLL.User().UpdateiGold(meid, mename, aa, "您在农场领取国庆大礼包中获得" + aa + "" + ub.Get("SiteBz") + ".");//加酷币
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得" + aa + "" + ub.Get("SiteBz") + ".";
            #endregion
        }
        else if (num == 21 || num == 22)//鲜花
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "1", "1", "1", "1", "1", "1", "1", "1", "1", "2" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));

            BCW.farm.Model.NC_hecheng yo = new BCW.farm.BLL.NC_hecheng().GetNC_hecheng2(75, 131420);//75红玫瑰
            //送礼记录+添加pic为1农场
            BCW.Model.Shopsend send = new BCW.Model.Shopsend();
            send.Title = yo.Title;
            send.GiftId = yo.GiftId;
            send.PrevPic = yo.PrevPic;
            send.Message = "领取国庆大礼包获得" + aa + "个" + yo.Title + ".";
            send.UsID = 10086;
            send.UsName = new BCW.BLL.User().GetUsName(10086);
            send.ToID = meid;
            send.ToName = new BCW.BLL.User().GetUsName(meid);
            send.Total = aa;
            send.AddTime = DateTime.Now;
            send.PIC = "1";
            new BCW.BLL.Shopsend().Add(send);

            //主人礼物记录+添加pic为1农场
            BCW.Model.Shopuser user = new BCW.Model.Shopuser();
            user.GiftId = yo.GiftId;
            user.UsID = meid;
            user.UsName = new BCW.BLL.User().GetUsName(meid);
            user.PrevPic = yo.PrevPic;
            user.GiftTitle = yo.Title;
            user.Total = aa;
            user.AddTime = DateTime.Now;
            user.PIC = "1";
            if (!new BCW.BLL.Shopuser().Exists_nc(meid, yo.GiftId))//农场判断
                new BCW.BLL.Shopuser().Add(user);
            else
                new BCW.BLL.Shopuser().Update_nc(user);//根据pic=1更新

            //增加属性与值
            BCW.farm.Model.NC_shop hj = new BCW.farm.BLL.NC_shop().GetNC_shop1(75);
            string sParas = new BCW.BLL.User().GetParas(meid);
            if (hj.tili != 0)
                sParas = BCW.User.Users.GetParaData(sParas, hj.tili, 0);
            if (hj.meili != 0)
                sParas = BCW.User.Users.GetParaData(sParas, hj.meili, 1);
            //更新属性值
            new BCW.BLL.User().UpdateParas(meid, sParas);

            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得" + aa + "个" + yo.Title + ".";
            #endregion
        }
        else if (num == 23)//点值
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "1", "1", "1", "1", "2", "2", "2", "3", "3", "3", "4", "4", "5" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));

            new BCW.Draw.BLL.DrawJifen().UpdateJifen(meid, new BCW.BLL.User().GetUsName(meid), aa, "恭喜你在" + GameName + "领取国庆大礼包中获得" + aa + "点值.");
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得" + aa + "点值.";
            #endregion
        }
        else if (num == 24)//等级
        {
            #region
            Random ran1 = new Random(unchecked((int)DateTime.Now.Ticks));
            string[] sNum = { "1", "1", "1", "1", "1", "1", "1", "1", "1", "2" };
            int aa = int.Parse((sNum[ran1.Next(sNum.Length)]));

            new BCW.farm.BLL.NC_user().update_zd("Grade=Grade+" + aa + ",Experience=0", "usid=" + meid + "");
            wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]国庆大礼包[/url]中获得升级奖励,自动升级" + aa + "级.目前等级是" + new BCW.farm.BLL.NC_user().GetGrade(meid) + "";
            #endregion
        }
        new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), wText);
        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, wText, 1);
        new BCW.BLL.Guest().Add(1, meid, mename, wText);
    }

    //消费回馈--道具不可赠送
    private void xiaofeihuikui(int id)
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string wText = "";
        int aa = id;//id为奖励道具的id
        string name = "";
        if (new BCW.farm.BLL.NC_mydaoju().Exists_hf(aa, meid, 1))
        {
            BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, aa, 1);//查询化肥数量
            name = aaa.name;
            new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, aa, 1);
        }
        else
        {
            if (new BCW.farm.BLL.NC_mydaoju().Exists_hf2(aa, meid, 1))
            {
                BCW.farm.Model.NC_mydaoju aaa = new BCW.farm.BLL.NC_mydaoju().Gethf_id(meid, aa, 1);//查询化肥数量
                name = aaa.name;
                new BCW.farm.BLL.NC_mydaoju().Update_hf(meid, 1, aa, 1);
            }
            else
            {
                //根据id判断
                BCW.farm.Model.NC_daoju t = new BCW.farm.BLL.NC_daoju().GetNC_daoju(aa);
                BCW.farm.Model.NC_mydaoju w = new BCW.farm.Model.NC_mydaoju();
                w.name = t.name;
                name = t.name;
                w.name_id = 0;
                w.num = 1;
                w.type = 2;
                w.usid = meid;
                w.zhonglei = 0;
                w.huafei_id = aa;
                w.picture = t.picture;
                w.iszengsong = 1;
                new BCW.farm.BLL.NC_mydaoju().Add(w);
            }
        }
        wText = "恭喜你在" + GameName + "领取[url=/bbs/game/farm.aspx?act=task&amp;ptype=3]消费回馈[/url]中获得1个" + name + ".";
        new BCW.BLL.Action().Add(1011, 0, meid, new BCW.BLL.User().GetUsName(meid), wText);
        new BCW.farm.BLL.NC_messagelog().addmessage(meid, mename, wText, 1);
        new BCW.BLL.Guest().Add(1, meid, mename, wText);
    }

    //换币
    private void CurrencyPage()
    {
        Master.Title = "" + GameName + "_换币";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=message") + "\">个人信息</a>&gt;换币");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        if (ub.GetSub("Currency", xmlPath) == "1")
            Utils.Error("兑换功能已关闭.", "");

        //builder.Append(Out.Tab("<div class=\"text\">", ""));
        //int ptype = int.Parse(Utils.GetRequest("ptype", "all", 1, @"^[1-2]$", "1"));
        //if (ptype == 1)
        //{
        //    builder.Append("<h style=\"color:red\">酷币兑换金币" + "</h>" + "|");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=Currency&amp;ptype=1") + "\">酷币兑换金币</a>" + "|");
        //}
        //if (ptype == 2)
        //{
        //    builder.Append("<h style=\"color:red\">金币兑换酷币" + "</h>" + "");
        //}
        //else
        //{
        //    builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=Currency&amp;ptype=2") + "\">金币兑换酷币</a>" + "");
        //}
        //builder.Append(Out.Tab("</div>", ""));

        string Tar = ub.GetSub("Tar", xmlPath);
        string Tar2 = ub.GetSub("Tar2", xmlPath);

        //if (ptype == 1)
        {
            string info = Utils.GetRequest("info", "post", 1, "", "");
            if (info == "ok")
            {
                //判断密码是否一致
                string oPwd = Utils.GetRequest("pw", "post", 2, @"^[A-Za-z0-9]{6,20}$", "请正确输入您的密码");
                string ordPwd = new BCW.BLL.User().GetUsPwd(meid);
                if (!Utils.MD5Str(oPwd).Equals(ordPwd))
                {
                    Utils.Error("密码不正确.", "");
                }
                string meverify = new BCW.BLL.User().GetVerifys(meid);
                string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
                string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
                if (!string.IsNullOrEmpty(meverify))
                {
                    if (verify.Equals(meverify))
                    {
                        Utils.Error("验证码填写错误", "");
                    }
                }
                //更新验证码
                new BCW.BLL.User().UpdateVerifys(meid, verify);
                if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
                {
                    Utils.Error("验证码填写错误", "");
                }
                long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "数额填写错误"));
                long iMoney = new BCW.BLL.User().GetGold(meid);
                if (payCent > iMoney)
                {
                    Utils.Error("你的" + ub.Get("SiteBz") + "不足", "");
                }
                if (payCent < Convert.ToInt64(Tar))
                {
                    Utils.Error("兑换" + ub.Get("SiteBz") + "额必须是" + Tar + "的倍数", "");
                }
                if (payCent % Convert.ToInt64(Tar) != 0)
                {
                    Utils.Error("兑换" + ub.Get("SiteBz") + "额必须是" + Tar + "的倍数", "");
                }

                long iTar = Convert.ToInt64(Utils.ParseInt(Tar));
                long iTar2 = Convert.ToInt64(Utils.ParseInt(Tar2));
                long LostGold = 0;
                if (iTar == 1)
                    LostGold = Convert.ToInt64(payCent * iTar2);
                else
                    LostGold = Convert.ToInt64(payCent / iTar);

                //检测上一个是否一样
                DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + meid + " and Types=1 ORDER BY ID DESC");
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
                    long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
                    DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

                    if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == LostGold)
                    {
                        if (AddTime > DateTime.Now.AddSeconds(-5))
                        {
                            new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + meid + "在农场" + ub.Get("SiteBz") + "兑换金币存在嫌疑，请进后台查询消费日志并处理。");
                        }
                    }
                }
                //是否刷屏
                BCW.User.Users.IsFresh("Farm_ExChange", 5);
                //减酷币
                new BCW.BLL.User().UpdateiGold(meid, -payCent, "兑换了" + LostGold + "金币");
                //加金币
                new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, LostGold, "花费" + payCent + "" + ub.Get("SiteBz") + "兑换了" + LostGold + "金币,目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) + LostGold) + "金币.", 11);
                Utils.Success("兑换金币", "兑换" + LostGold + "金币成功，花费了" + payCent + "" + ub.Get("SiteBz") + "，正在返回..", Utils.getUrl("farm.aspx?act=Currency"), "1");
            }
            else
            {
                string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
                string strText = "支出多少" + ub.Get("SiteBz") + "：/,输入验证码：/,须验证登陆密码：/,,,,";
                string strName = "payCent,verify,pw,verifyKey,info,act";
                string strType = "num,text,password,hidden,hidden,hidden";
                string strValu = "'''" + verifyKey + "'ok'Currency";
                string strEmpt = "false,true,false,false,false,false";
                string strIdea = "'\n<a href=\"" + Utils.getUrl("farm.aspx?act=Currency&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''''|/";
                string strOthe = "&gt;兑换金币,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br />"));
                builder.Append("当前汇率:<br />" + ub.Get("SiteBz") + "兑换金币" + Tar + ":" + Tar2 + ".");
                builder.Append("<br />自带<h style=\"color:red\">" + (new BCW.BLL.User().GetGold(meid)) + "</h>" + ub.Get("SiteBz") + ";");
                builder.Append("<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        //else
        //{
        //    string info = Utils.GetRequest("info", "post", 1, "", "");
        //    if (info == "ok2")
        //    {
        //        string meverify = new BCW.BLL.User().GetVerifys(meid);
        //        string verify = Utils.GetRequest("verify", "post", 2, @"^[0-9][0-9][0-9][0-9]$", "验证码填写错误");
        //        string verifyKey = Utils.GetRequest("verifyKey", "post", 2, @"^[^\^]{1,}$", "验证码错误");
        //        if (!string.IsNullOrEmpty(meverify))
        //        {
        //            if (verify.Equals(meverify))
        //            {
        //                Utils.Error("验证码填写错误", "");
        //            }
        //        }
        //        //更新验证码
        //        new BCW.BLL.User().UpdateVerifys(meid, verify);
        //        if (!DESEncrypt.Encrypt(verify).Equals(verifyKey))
        //        {
        //            Utils.Error("验证码填写错误", "");
        //        }
        //        long payCent = Convert.ToInt64(Utils.GetRequest("payCent", "post", 2, @"^[1-9]\d*$", "数额填写错误"));//金币的
        //        long iMoney = new BCW.farm.BLL.NC_user().GetGold(meid);//我的金币
        //        if (payCent > iMoney)
        //        {
        //            Utils.Error("你的金币不足", "");
        //        }
        //        if (payCent < Convert.ToInt64(Tar2))
        //        {
        //            Utils.Error("兑换金币额必须是" + Tar2 + "的倍数", "");
        //        }
        //        if (payCent % Convert.ToInt64(Tar2) != 0)
        //        {
        //            Utils.Error("兑换金币额必须是" + Tar2 + "的倍数", "");
        //        }

        //        long iTar = Convert.ToInt64(Utils.ParseInt(Tar2));
        //        long iTar2 = Convert.ToInt64(Utils.ParseInt(Tar));
        //        long LostGold = 0;
        //        if (iTar == 1)
        //            LostGold = Convert.ToInt64(payCent * iTar2);
        //        else
        //            LostGold = Convert.ToInt64(payCent / iTar);

        //        //检测上一个是否一样
        //        DataSet ds = new BCW.BLL.Goldlog().GetList("TOP 1 PUrl,AcGold,AddTime", "UsId=" + meid + " and Types=1 ORDER BY ID DESC");
        //        if (ds != null && ds.Tables[0].Rows.Count > 0)
        //        {
        //            string PUrl = ds.Tables[0].Rows[0]["PUrl"].ToString();
        //            long AcGold = Int64.Parse(ds.Tables[0].Rows[0]["AcGold"].ToString());
        //            DateTime AddTime = DateTime.Parse(ds.Tables[0].Rows[0]["AddTime"].ToString());

        //            if (PUrl == Utils.getPageUrl() && Math.Abs(AcGold) == LostGold)
        //            {
        //                if (AddTime > DateTime.Now.AddSeconds(-5))
        //                {
        //                    new BCW.BLL.Guest().Add(10086, "管理员", "ID:" + meid + "在兑换" + ub.Get("SiteBz") + "嫌疑，请进后台查询消费日志并处理。");
        //                }
        //            }
        //        }
        //        //是否刷屏
        //        BCW.User.Users.IsFresh("Farm_ExChange", 60);
        //        //加酷币
        //        new BCW.BLL.User().UpdateiGold(meid, LostGold, "花费了" + payCent + "金币兑换");
        //        //减金币
        //        new BCW.farm.BLL.NC_user().UpdateiGold(meid, mename, -payCent, "花费" + payCent + "金币兑换了" + LostGold + "" + ub.Get("SiteBz") + ",目前拥有" + (new BCW.farm.BLL.NC_user().GetGold(meid) - payCent) + "金币.", 11);
        //        Utils.Success("兑换" + ub.Get("SiteBz") + "", "兑换" + LostGold + "" + ub.Get("SiteBz") + "成功，花费了" + payCent + "金币，正在返回..", Utils.getUrl("farm.aspx?act=Currency&amp;ptype=2"), "1");
        //    }
        //    else
        //    {
        //        string verifyKey = DESEncrypt.Encrypt(new Rand().RandNumer(4));
        //        string strText = "支出多少金币：/,输入验证码:/,,,,,";
        //        string strName = "payCent,verify,verifyKey,info,act,ptype";
        //        string strType = "num,text,hidden,hidden,hidden,hidden";
        //        string strValu = "''" + verifyKey + "'ok2'Currency'2";
        //        string strEmpt = "false,true,false,false,false,false";
        //        string strIdea = "'\n<a href=\"" + Utils.getUrl("farm.aspx?act=Currency&amp;ptype=2&amp;backurl=" + Utils.getPage(0) + "") + "\"><img src=\"/snap.aspx?act=paycode&amp;imgid=" + verifyKey + "\" alt=\"load\"/></a>'''''|/";
        //        string strOthe = "&gt;兑换酷币,farm.aspx,post,1,red";
        //        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        //        builder.Append(Out.Tab("<div>", "<br />"));
        //        builder.Append("当前汇率:<br />金币兑换" + ub.Get("SiteBz") + "" + Tar2 + ":" + Tar + ".");
        //        builder.Append("<br />自带<h style=\"color:red\">" + (new BCW.BLL.User().GetGold(meid)) + "</h>" + ub.Get("SiteBz") + ";");
        //        builder.Append("<h style=\"color:red\">" + (new BCW.farm.BLL.NC_user().GetGold(meid)) + "</h>金币.");
        //        builder.Append(Out.Tab("</div>", ""));
        //    }
        //}


        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=message") + "\">返回上级>></a>");
        builder.Append(Out.Tab("</div>", ""));
        foot_link();//底部链接

        foot_link2();
    }

    //留言
    private void liuyanPage()
    {
        //留言维护提示
        if (ub.GetSub("liuyanStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "" + GameName + "_留言";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;农场好友留言");
        builder.Append(Out.Tab("</div>", "<br/>"));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int uid = int.Parse(Utils.GetRequest("uid", "all", 1, @"^[1-9]\d*$", "0"));



        if (uid == 0)
        {
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.Get("SiteListNo"));
            //string strWhere = "";
            string[] pageValUrl = { "act", "ptype", "page1", "uid", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            DataSet ds = new BCW.farm.BLL.NC_tudi().GetList("DISTINCT(a.usid)", "tb_NC_tudi a INNER JOIN tb_Friend c ON a.usid=c.FriendID", "c.UsID=" + meid + "");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }
                for (int i = 0; i < skt; i++)
                {
                    int UsID = Convert.ToInt32(ds.Tables[0].Rows[koo + i][0]);//用户id

                    if (UsID != meid)//如果不等于自己的id
                    {
                        if (k % 2 == 0)
                            builder.Append(Out.Tab("<div>", "<br />"));
                        else
                        {
                            if (k == 1)
                                builder.Append(Out.Tab("<div>", ""));
                            else
                                builder.Append(Out.Tab("<div>", "<br />"));
                        }
                        builder.Append("" + ((pageIndex - 1) * pageSize + k) + ".<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + UsID + "") + "\">" + new BCW.BLL.User().GetUsName(UsID) + "(" + UsID + ")</a>");
                        int num = 0;
                        DataSet aa = new BCW.BLL.Mebook().GetList("COUNT(*) AS bb", "(UsID=" + UsID + " and mid=" + meid + ") or (UsID=" + meid + " and mid=" + UsID + ")  AND Type=1001");//
                        for (int j = 0; j < aa.Tables[0].Rows.Count; j++)
                        {
                            num = int.Parse(aa.Tables[0].Rows[j]["bb"].ToString());
                            if (num > 0)
                            {
                                builder.Append(".[<a href=\"" + Utils.getUrl("farm.aspx?act=liuyao_list&amp;id=" + UsID + "") + "\">" + num + "条</a>]");
                            }
                            else
                                builder.Append(".[" + num + "条]");
                        }
                        k++;
                        builder.Append(Out.Tab("</div>", ""));
                    }
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有留言记录.."));
            }
        }
        else
        {
            int pageIndex1;
            int recordCount1;
            int pageSize1 = Convert.ToInt32(ub.Get("SiteListNo"));
            string strWhere1 = "";
            string[] pageValUrl1 = { "act", "ptype", "page", "uid", "backurl" };
            pageIndex1 = Utils.ParseInt(Request.QueryString["page1"]);
            if (pageIndex1 == 0)
                pageIndex1 = 1;

            strWhere1 = "MID=" + uid + " and type=1001";
            IList<BCW.Model.Mebook> listMebook = new BCW.BLL.Mebook().GetMebooks(pageIndex1, pageSize1, strWhere1, out recordCount1);
            if (listMebook.Count > 0)
            {
                int k = 1;
                foreach (BCW.Model.Mebook n in listMebook)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div class=\"text\">", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    builder.Append("" + ((pageIndex1 - 1) * pageSize1 + k) + ".");
                    if (meid == n.MID)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.MID + "") + "\">我</a>对");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.MID + "") + "\">" + n.MName + "</a>对");
                    }
                    if (meid == n.UsID)
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">我</a>");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("/bbs/uinfo.aspx?uid=" + n.UsID + "") + "\">" + new BCW.BLL.User().GetUsName(n.UsID) + "</a>");
                    }
                    builder.Append("说：" + n.MContent + "[" + DT.FormatDate(n.AddTime, 1) + "]");
                    k++;
                    builder.Append(Out.Tab("</div>", ""));
                }
                // 分页
                builder.Append(BasePage.MultiPage(pageIndex1, pageSize1, recordCount1, Utils.getPageUrl(), pageValUrl1, "page1", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "没有留言记录.."));
            }
        }

        //搜索留言
        string strText = "输入用户ID:/,";
        string strName = "uid,backurl";
        string strType = "num,hidden";
        string strValu = string.Empty;
        if (uid == 0)
        {
            strValu = "'" + Utils.getPage(0) + "";
        }
        else
        {
            strValu = "" + uid + "'" + Utils.getPage(0) + "";
        }
        string strEmpt = "true,false";
        string strIdea = "/";
        string strOthe = "搜我的留言,farm.aspx?act=liuyan,post,1,red";
        builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

        foot_link();//底部链接

        foot_link2();
    }

    //增加留言
    private void add_bbsPage()
    {
        //留言维护提示
        if (ub.GetSub("liuyanStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名
        int usid = Utils.ParseInt(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int a = Utils.ParseInt(Utils.GetRequest("a", "all", 1, @"^[0-1]\d*$", "0"));
        if (meid == usid)
            Utils.Success("抱歉", "抱歉，不能给自己留言.", Utils.getUrl("farm.aspx?act=liuyan"), "1");
        int dd = int.Parse(Utils.GetRequest("dd", "get", 1, @"^[0-9]\d*$", "0"));
        string Copytemp = string.Empty;
        if (dd > 0)
            Copytemp += new BCW.BLL.Submit().GetContent(dd, meid);

        if (new BCW.BLL.User().Exists(usid))
        {
            if (!new BCW.farm.BLL.NC_user().Exists(usid))
                Utils.Success("抱歉", "抱歉，该好友没有开通农场.", Utils.getUrl("farm.aspx?act=liuyan"), "1");
            Master.Title = "" + GameName + "_给" + new BCW.BLL.User().GetUsName(usid) + "留言";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">留言</a>&gt;给" + new BCW.BLL.User().GetUsName(usid) + "留言");
            builder.Append(Out.Tab("</div>", ""));

            string info = Utils.GetRequest("info", "post", 1, "", "");
            if (info == "")
            {
                string strText = "我要对" + new BCW.BLL.User().GetUsName(usid) + "说：/,,,,,";
                string strName = "Content,uid,act,a,info";
                string strType = "textarea,hidden,hidden,hidden,hidden";
                string strValu = "" + Copytemp + "'" + usid + "'add_bbs'" + a + "'add";
                string strEmpt = "true,false,false,false,false";
                string strIdea = "/";
                string strOthe = "提交留言,farm.aspx,post,1,red";
                builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

                builder.Append(Out.Tab("<div>", "<br/>"));
                builder.Append("插入：<a href=\"" + Utils.getUrl("/bbs/function.aspx?act=admsub&amp;backurl=" + Utils.PostPage(1) + "") + "\">常用留言</a>");
                builder.Append(Out.Tab("</div>", ""));
            }
            else
            {
                BCW.User.Users.IsFresh("Farm_bbs", 3);//是否刷屏
                int a1 = Utils.ParseInt(Utils.GetRequest("a", "all", 1, @"^[0-1]\d*$", "0"));
                int usid1 = Utils.ParseInt(Utils.GetRequest("uid", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));
                string Content = Utils.GetRequest("Content", "all", 2, @"^[\s\S]{1,80}$", "留言内容限1-80字.");
                if (Content.Trim() == "")
                    Utils.Error("请输入留言内容.", "");
                //验证id是否为农场好友
                if (new BCW.farm.BLL.NC_user().Exists(usid1))
                {
                    BCW.Model.Mebook model = new BCW.Model.Mebook();
                    model.UsID = usid;
                    model.MID = meid;
                    model.MName = new BCW.BLL.User().GetUsName(meid);
                    model.MContent = Content.Trim();
                    model.IsTop = 0;
                    model.AddTime = DateTime.Now;
                    model.Type = 1001;
                    new BCW.BLL.Mebook().Add(model);

                    new BCW.BLL.Guest().Add(3, usid, new BCW.BLL.User().GetUsName(usid), "[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]在您的[url=/bbs/game/farm.aspx?act=liuyao_list&amp;id=" + meid + "]农场留言[/url]啦!");
                    if (a1 == 1)
                        Utils.Success("发表留言", "发表留言成功，正在返回..", Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + ""), "1");
                    else
                        Utils.Success("发表留言", "发表留言成功，正在返回..", Utils.getUrl("farm.aspx?act=liuyao_list&amp;id=" + usid + ""), "1");
                }
                else
                    Utils.Error("抱歉，该ID还没开通农场.", "");
            }
        }
        else
            Utils.Success("抱歉", "抱歉，请选择正确ID留言.", Utils.getUrl("farm.aspx?act=liuyan"), "1");

        builder.Append(Out.Tab("<div>", Out.Hr()));
        //builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">返回留言>></a><br/>");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=do&amp;uid=" + usid + "") + "\">Ta的农场>></a>");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();
        foot_link2();
    }

    //查看留言
    private void liuyao_listPage()
    {
        //留言维护提示
        if (ub.GetSub("liuyanStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string info = Utils.GetRequest("info", "all", 1, "", "");
        int usid = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择用户ID无效"));

        if (info == "del")
        {
            int usid1 = Utils.ParseInt(Utils.GetRequest("uid", "all", 2, @"^[0-9]\d*$", "选择用户usid无效"));//自己的usid
            int id = Utils.ParseInt(Utils.GetRequest("id", "all", 2, @"^[1-9]\d*$", "选择ID出错"));
            int mid1 = Utils.ParseInt(Utils.GetRequest("mid", "all", 2, @"^[0-9]\d*$", "选择用户MID无效"));
            BCW.Model.Mebook a = new BCW.BLL.Mebook().GetMebook(id);
            int bb = 0;
            if (usid1 == 0)
            {
                if (mid1 != 0)
                {
                    if (!new BCW.farm.BLL.NC_user().Exists(mid1))
                        Utils.Success("抱歉", "抱歉，该好友没有开通农场.", Utils.getUrl("farm.aspx?act=liuyao_list&amp;id=" + a.UsID + ""), "1");

                    if (meid != a.MID || meid != mid1)
                    {
                        Utils.Error("你的权限不足", "");
                    }
                    if (!new BCW.BLL.Mebook().Exists2(id, meid))
                    {
                        Utils.Error("不存在的记录", "");
                    }
                    bb = a.UsID;
                }
                else
                {
                    Utils.Error("请选择正确留言记录.", "");
                }
            }
            else
            {
                if (mid1 == 0)
                {
                    if (!new BCW.farm.BLL.NC_user().Exists(usid1))
                        Utils.Success("抱歉", "抱歉，该好友没有开通农场.", Utils.getUrl("farm.aspx?act=liuyao_list&amp;id=" + a.MID + ""), "1");

                    if (usid1 != a.UsID)
                    {
                        Utils.Error("你的权限不足", "");
                    }
                    if (!new BCW.BLL.Mebook().Exists(id, usid1))
                    {
                        Utils.Error("不存在的记录", "");
                    }
                    bb = a.MID;
                }
                else
                {
                    Utils.Error("请选择正确留言记录.", "");
                }
            }

            new BCW.BLL.Mebook().Delete(id);
            Utils.Success("删除留言", "删除成功，正在返回..", Utils.getPage("farm.aspx?act=liuyao_list&amp;id=" + bb + ""), "1");
        }
        else
        {
            Master.Title = "" + GameName + "_与" + new BCW.BLL.User().GetUsName(usid) + "的留言对话";
            builder.Append(Out.Tab("<div class=\"title\">", ""));
            builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">留言</a>&gt;与" + new BCW.BLL.User().GetUsName(usid) + "的留言对话");
            builder.Append(Out.Tab("</div>", "<br/>"));
            int pageIndex;
            int recordCount;
            int pageSize = Convert.ToInt32(ub.GetSub("SiteListNo", xmlPath));
            string strWhere = "";
            string[] pageValUrl = { "act", "info", "id", "backurl" };
            pageIndex = Utils.ParseInt(Request.QueryString["page"]);
            if (pageIndex == 0)
                pageIndex = 1;

            strWhere = "(usid=" + meid + " or MID=" + meid + ")  and type=1001";

            builder.Append(Out.Tab("<div>", ""));
            builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=add_bbs&amp;uid=" + usid + "") + "\">[回复:" + new BCW.BLL.User().GetUsName(usid) + "]</a>");
            builder.Append(Out.Tab("</div>", "<br/>"));

            DataSet ds = new BCW.BLL.Mebook().GetList("*", "(UsID=" + meid + " AND MID=" + usid + ") OR (UsID=" + usid + " AND MID=" + meid + ")  and type=1001 ORDER BY AddTime DESC");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                recordCount = ds.Tables[0].Rows.Count;
                int k = 1;
                int koo = (pageIndex - 1) * pageSize;
                int skt = pageSize;
                if (recordCount > koo + pageSize)
                {
                    skt = pageSize;
                }
                else
                {
                    skt = recordCount - koo;
                }

                for (int i = 0; i < skt; i++)
                {
                    if (k % 2 == 0)
                        builder.Append(Out.Tab("<div>", "<br />"));
                    else
                    {
                        if (k == 1)
                            builder.Append(Out.Tab("<div>", ""));
                        else
                            builder.Append(Out.Tab("<div>", "<br />"));
                    }
                    int ID = int.Parse(ds.Tables[0].Rows[koo + i]["ID"].ToString());
                    int UsID = int.Parse(ds.Tables[0].Rows[koo + i]["usid"].ToString());
                    int MID = int.Parse(ds.Tables[0].Rows[koo + i]["mid"].ToString());
                    string MName = ds.Tables[0].Rows[koo + i]["MName"].ToString().Trim();
                    string MContent = ds.Tables[0].Rows[koo + i]["MContent"].ToString().Trim();
                    DateTime AddTime = Convert.ToDateTime(ds.Tables[0].Rows[koo + i]["AddTime"]);
                    if (MID == meid)
                    {
                        builder.Append("我");
                    }
                    else
                    {
                        builder.Append("<a href=\"" + Utils.getUrl("../uinfo.aspx?uid=" + MID + "&amp;backurl=" + Utils.PostPage(1) + "") + "\">" + MName + "(" + MID + ")</a>");
                    }
                    builder.Append(":" + Out.SysUBB(MContent) + "[" + DT.FormatDate(AddTime, 1) + "]");
                    if (MID != meid)//对方的留言
                    {
                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=liuyao_list&amp;info=del&amp;id=" + ID + "&amp;uid=" + UsID + "&amp;MID=0") + "\">[删]</a>");
                    }
                    else//自己的留言
                    {
                        builder.Append(".<a href=\"" + Utils.getUrl("farm.aspx?act=liuyao_list&amp;info=del&amp;id=" + ID + "&amp;uid=0&amp;MID=" + MID + "") + "\">[删]</a>");
                    }
                    builder.Append(Out.Tab("</div>", ""));
                    k++;
                }
                builder.Append(BasePage.MultiPage(pageIndex, pageSize, recordCount, Utils.getPageUrl(), pageValUrl, "page", 0));
            }
            else
            {
                builder.Append(Out.Div("div", "暂无留言."));
            }
        }
        builder.Append(Out.Tab("<div>", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=liuyan") + "\">返回留言>></a>");
        builder.Append(Out.Tab("</div>", ""));

        foot_link();//底部链接

        foot_link2();
    }

    //底部链接
    private void foot_link()
    {
        builder.Append(Out.Tab("<div class=\"text\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=bozhong_1") + "\">播种</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=shangdian") + "\">商店</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=cangku") + "\">仓库</a>.");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx?act=toucai") + "\">偷菜</a>");
        builder.Append(Out.Tab("</div>", ""));
    }
    //底部链接2
    private void foot_link2()
    {
        //游戏底部Ubb
        string Foot = ub.GetSub("farmFoot", xmlPath);
        if (Foot != "")
        {
            builder.Append(Out.Tab("<div>", "<br />"));
            builder.Append(BCW.User.AdminCall.AdminUBB(Out.SysUBB(Foot)));
            builder.Append(Out.Tab("</div>", ""));
        }
        builder.Append(Out.Tab("<div class=\"title\">", Out.Hr()));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">首页</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>-");
        builder.Append("<a href=\"" + Utils.getUrl("farm.aspx") + "\">我的农场</a>");
        builder.Append(Out.Tab("</div>", ""));
    }

    //内线邀请
    private void yaoqingPage()
    {
        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        int usid = Utils.ParseInt(Utils.GetRequest("uid", "get", 1, @"^[1-9]\d*$", "0"));

        if (new BCW.BLL.User().Exists(usid))
        {
            //发内线
            BCW.Model.Guest model = new BCW.Model.Guest();
            model.FromId = meid;
            model.FromName = mename;
            model.ToId = usid;
            model.ToName = new BCW.BLL.User().GetUsName(usid);
            model.Content = "" + mename + "邀请您到" + GameName + "玩耍.[url=/bbs/game/farm.aspx]马上去" + GameName + "[/url]";
            model.TransId = 0;
            new BCW.BLL.Guest().Add(model);
            //new BCW.BLL.Guest().Add(1, usid, new BCW.BLL.User().GetUsName(usid), "" + mename + "邀请您到" + GameName + "玩耍.[url=/bbs/game/farm.aspx]马上去" + GameName + "[/url]");
            Utils.Success("邀请好友", "邀请成功.", Utils.getUrl("farm.aspx?act=toucai&amp;usid=" + usid + ""), "1");
        }
        else
            Utils.Error("抱歉，请邀请正确ID", "");
    }

    //偷菜加好友内线
    private void faneixianPage()
    {
        int usid = Utils.ParseInt(Utils.GetRequest("usid", "get", 2, @"^[1-9]\d*$", "选择用户ID无效"));
        int uid = new BCW.User.Users().GetUsId();
        if (uid == 0)
            Utils.Login();
        BCW.User.Users.IsFresh("Farmnx", 3);//防刷
        new BCW.BLL.Guest().Add(usid, new BCW.BLL.User().GetUsName(usid), "[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]申请加您为Ta的好友.[br]您可以接受并[url=/bbs/friend.aspx?act=save&amp;frid=" + uid + "]加Ta为好友[/url],[url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=1]只接受[/url]|[url=/bbs/friend.aspx?act=addok&amp;hid=" + uid + "&amp;ptype=2]拒绝Ta[/url]");
        Utils.Success("操作好友", "邀请发送成功.正在返回.", Utils.getUrl("farm.aspx?act=toucai"), "1");
    }

    //等级各阶段升级所需经验
    private void dengjisuoxu(int meid)
    {
        if (new BCW.farm.BLL.NC_user().GetGrade(meid) <= 20)
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 200))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 200));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 40) && (new BCW.farm.BLL.NC_user().GetGrade(meid) > 20))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 250))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 250));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        //邵广林 20160928 变动升级经验
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 50) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 41))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 500))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 500));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 60) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 51))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 700))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 700));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 70) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 61))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 900))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 900));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 80) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 71))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1300))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1300));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 90) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 81))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1700))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 1700));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        else if ((new BCW.farm.BLL.NC_user().GetGrade(meid) <= 100) && (new BCW.farm.BLL.NC_user().GetGrade(meid) >= 91))
        {
            if (new BCW.farm.BLL.NC_user().Getjingyan(meid) >= (((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 2500))
            {
                new BCW.farm.BLL.NC_user().Update_Experience(meid, -(((new BCW.farm.BLL.NC_user().GetGrade(meid)) + 1) * 2500));//减经验
                new BCW.farm.BLL.NC_user().Update_dengji(meid);//等级+1
            }
        }
        //大于100级
        else
        {

        }
    }

    //加密
    public string SetRoomID(string GKeyStr)
    {
        return BCW.Common.DESEncrypt.Encrypt(GKeyStr, "farm");
    }
    //解密
    public int GetRoomID(string GKeyStr)
    {
        int Rid = -1;
        try
        {
            Rid = int.Parse(BCW.Common.DESEncrypt.Decrypt(GKeyStr, "farm"));
        }
        catch { };
        return Rid;
    }

    //农场寄语
    private void jiyuPage()
    {
        Master.Title = "" + GameName + "_寄语";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;农场寄语");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "")
        {
            string jiyu = new BCW.farm.BLL.NC_user().Get_jiyu(meid);
            string strText = "我的农场寄语:(限30字包含UBB)/,,,";
            string strName = "jiyu,act,ac";
            string strType = "textarea,hidden,hidden";
            string strValu = "" + jiyu + "'jiyu'ok";
            string strEmpt = "false,false,false";
            string strIdea = "/";
            string strOthe = "确定,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));
        }
        else
        {
            BCW.User.Users.IsFresh("Farmjy", 5);//防刷
            string jiyu = Utils.GetRequest("jiyu", "all", 2, @"^[^\^]{1,30}$", "寄语限1-30字内.");
            new BCW.farm.BLL.NC_user().update_zd("jiyu='" + jiyu + "'", "usid=" + meid + "");
            Utils.Success("寄语设置", "寄语设置成功.", Utils.getUrl("farm.aspx"), "1");
        }

        foot_link();//底部链接
        foot_link2();
    }

    //纸条
    private void zhitiaoPage()
    {
        //纸条维护提示
        if (ub.GetSub("ztStatus", xmlPath) == "1")
        {
            Utils.Safe("此功能");
        }
        Master.Title = "" + GameName + "_纸条";
        builder.Append(Out.Tab("<div class=\"title\">", ""));
        builder.Append("<a href=\"" + Utils.getUrl("default.aspx") + "\">游戏大厅</a>&gt;<a href=\"" + Utils.getUrl("farm.aspx") + "\">" + GameName + "</a>&gt;农场纸条");
        builder.Append(Out.Tab("</div>", ""));

        int meid = new BCW.User.Users().GetUsId();
        if (meid == 0)
            Utils.Login();
        string mename = new BCW.BLL.User().GetUsName(meid);//用户姓名

        string ac = Utils.GetRequest("ac", "all", 1, "", "");
        if (ac == "")
        {
            string strText = "纸条类型:/,编写纸条:/,,";
            string strName = "type,contact,act,ac";
            string strType = "select,textarea,hidden,hidden";
            string strValu = "''zhitiao'ok";
            string strEmpt = "1|建议|0|投诉,,false,false,false";
            string strIdea = "/";
            string strOthe = "提交,farm.aspx,post,1,red";
            builder.Append(Out.wapform(strText, strName, strType, strValu, strEmpt, strIdea, strOthe));

            builder.Append(Out.Tab("<div class=\"text\">", "<br/>"));
            builder.Append("听你所言,助我成长!聆听农友们心声!!");
            builder.Append(Out.Tab("</div>", "<br/>"));

            //随机显示2条，不够10条不显示
            DataSet ds = new BCW.farm.BLL.NC_zhitiao().GetList("TOP(2) contact", "type=1 order by newid()");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                builder.Append(Out.Tab("<div>", ""));
                for (int i = 0; i < 2; i++)
                {
                    string contact = (ds.Tables[0].Rows[i]["contact"]).ToString();
                    builder.Append("**说：" + contact + "<br/>");
                }
                builder.Append(Out.Tab("</div>", ""));
            }
        }
        else
        {
            BCW.User.Users.IsFresh("Farmzt", 5);//防刷
            string contact = Utils.GetRequest("contact", "all", 2, @"^[^\^]{10,600}$", "纸条内容限10-600字内.");
            int type = int.Parse(Utils.GetRequest("type", "all", 1, @"^[0-1]\d*$", "0"));
            if (contact.Trim() == "")
                Utils.Error("请输入内容.", "");

            string stop_ZT = ub.GetSub("stop_ZT", xmlPath);
            if (stop_ZT != "")
            {
                string[] sNum = stop_ZT.Split('#');
                int sbsy = 0;
                for (int a = 0; a < sNum.Length; a++)
                {
                    int tid = 0;
                    int.TryParse(sNum[a].Trim(), out tid);
                    if (meid == tid)
                    {
                        sbsy++;
                    }
                }
                if (sbsy > 0)
                {
                    Utils.Error("抱歉,系统已拒绝你的建议.请与客服联系,谢谢.", "");
                }
            }

            BCW.farm.Model.NC_zhitiao model = new BCW.farm.Model.NC_zhitiao();
            model.AddTime = DateTime.Now;
            model.UsID = meid;
            model.type = type;
            model.contact = contact.Trim();
            new BCW.farm.BLL.NC_zhitiao().Add(model);
            Utils.Success("提交成功", "提交成功.感谢您提供宝贵意见.我们将不断改善,营造良好农场氛围！", Utils.getUrl("farm.aspx"), "2");
        }

        foot_link();//底部链接
        foot_link2();
    }


}

