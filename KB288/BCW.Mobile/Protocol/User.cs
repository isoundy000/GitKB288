using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.User;

namespace BCW.Mobile.Protocol
{


    /// <summary>
    /// 请求会员帐户信息
    /// </summary>
    public class ReqUserAccount : ReqProtocolBase
    {

    }

    public class RspUserAccount : RspProtocolBase
    {
        public List<UserAccount> lstUserAccount;

        public RspUserAccount()
        {
            lstUserAccount = new List<UserAccount>();
        }
    }

    /// <summary>
    /// 会员请求修改密码
    /// </summary>
    public class ReqUserModifyPwd:ReqProtocolBase
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public int userId;  

        /// <summary>
        /// 原密码
        /// </summary>
        public string oldPwd;

        /// <summary>
        /// 新密码
        /// </summary>
        public string newPwd;
    }

    /// <summary>
    /// 会员修改密码返回
    /// </summary>
    public class RspUserModifyPwd : RspProtocolBase
    {
        
    }

    public class ReqUserResetPwd : ReqProtocolBase
    {
        /// <summary>
        /// 帐号：用户ID或电话号码
        /// </summary>
        public string accountId;

        /// <summary>
        /// 手机验证码
        /// </summary>
        public int ValidateCode;

        /// <summary>
        /// 新密码
        /// </summary>
        public string newPwd;
    }

    public class RspUserResetPwd : RspProtocolBase
    {

    }
}
