using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.User;

namespace BCW.Mobile.Protocol
{


    /// <summary>
    /// 请求会员帐户信息
    /// </summary>
    public class ReqUserAccount:ReqProtocolBase
    {

    }

    public class RspUserAccount:RspProtocolBase
    {
        public List<UserAccount> lstUserAccount;

        public RspUserAccount()
        {
            lstUserAccount = new List<UserAccount>();
        }
    }
}
