using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;

namespace BCW.Mobile.User
{
    public enum EUserAccount
    {
        cobi,       //酷币
        money,      //爆谷
        bank,       //银行存款
    }

    public class UserAccount
    {
        public EUserAccount accountName;        //帐户名称
        public long value;                      //帐户余额
    }

    public class UserManager
    {
        private static UserManager mInstance;

        public static UserManager Instance()
        {
            if (mInstance == null)
                mInstance = new UserManager();
            return mInstance;
        }

        //查询某个帐户余额
        public UserAccount GetUserAccount(EUserAccount _accountName,int _userId)
        {
            long _val = 0;
            switch (_accountName)
            {
                case EUserAccount.cobi:
                    _val = new BCW.BLL.User().GetGold(_userId);
                    break;
                case EUserAccount.money:
                    _val = new BCW.BLL.User().GetMoney(_userId);
                    break;
                case EUserAccount.bank:
                    _val = new BCW.BLL.User().GetBank(_userId);
                    break;
                default:
                    return null;
            }
            
            UserAccount _userAccount = new UserAccount();
            _userAccount.accountName = _accountName;
            _userAccount.value = _val;
            return _userAccount; 
        }

        public RspUserAccount GetUserAllAccount(ReqUserAccount _reqData)
        {
            RspUserAccount _rspData = new RspUserAccount();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspData;
            }

            //检查是否登录状态
            if (Common.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspData;
            }

            //获取酷币余额
            UserAccount _cobiAccount = GetUserAccount(EUserAccount.cobi, _reqData.userId);
            if (_cobiAccount != null)
                _rspData.lstUserAccount.Add(_cobiAccount);


            //获取爆谷余额
            UserAccount _moneyAccount = GetUserAccount(EUserAccount.money, _reqData.userId);
            if (_moneyAccount != null)
                _rspData.lstUserAccount.Add(_moneyAccount);

            _rspData.header.status = ERequestResult.success;            
            return _rspData;
        }     
    }
}
