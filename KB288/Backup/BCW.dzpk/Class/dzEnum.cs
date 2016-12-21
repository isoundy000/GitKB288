using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.dzpk.Enum
{
    /// <summary>
    /// 枚举：游戏状态定义
    /// </summary>
    public enum dzFsmState
    {
        eWait=0,       //等待状态
        eBlinds,       //盲注
        eFlop,         //翻牌
        eTurn,         //转牌
        etRiver,       //河牌
        etShowdown     //摊牌(结算)
    }

    /// <summary>
    /// 枚举：身份定义
    /// </summary>
    public enum dzPlayerIdentity
    {
        eNone,           //无
        eButton,         //庄家
        eSb,             //小盲
        eGb,             //大盲
        eNormal,         //普通玩家
    }

    /// <summary>
    /// 枚举：操作方式定义
    /// </summary>
    public enum dzAcType
    {
        eCheck,         //过牌
        eBet,           //下注
        eCall,          //跟注
        eRaise,         //加注
        eFold,          //弃牌
        eAllIn          //全下
    }

    public enum dzUserStaus
    {
        eOnLooker,      //围观
        eWait,          //等待下一局开始
        sInGame         //游戏中
    }
}
