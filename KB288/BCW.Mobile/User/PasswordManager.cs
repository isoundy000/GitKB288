using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Protocol;
using System.Text.RegularExpressions;
using BCW.Mobile.Error;
using BCW.Common;

namespace BCW.Mobile.User
{
    public class PasswordManager
    {
        private static PasswordManager mInstance;

        public static PasswordManager Instance()
        {
            if (mInstance == null)
                mInstance = new PasswordManager();
            return mInstance;
        }

        public RspUserModifyPwd UserModifyPwd(ReqUserModifyPwd _reqData)
        {
            RspUserModifyPwd _rspData = new RspUserModifyPwd();

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

            //检查密码是否合法
            if (Regex.IsMatch(_reqData.newPwd, @"^[A-Za-z0-9]{6,20}$") == false)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode =MOBILE_ERROR_CODE.REGEDIT_PWD_VERIFY;
                return _rspData;
            }

            //旧密码是否正确
            string ordPwd = new BCW.BLL.User().GetUsPwd(_reqData.userId);
            if (!Utils.MD5Str(_reqData.oldPwd).Equals(ordPwd))
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = MOBILE_ERROR_CODE.SYS_USER_OLDPWD_ERROR;
                return _rspData;
            }

            new BCW.BLL.User().UpdateUsPwd(_reqData.userId, Utils.MD5Str(_reqData.newPwd));

            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }

        public RspUserResetPwd UserResetPwd(ReqUserResetPwd _reqData)
        {
            RspUserResetPwd _rspData = new RspUserResetPwd();
            
            if (_reqData.accountId.Length == 11)
            {
                //检查手机号码是否合法
                if (Regex.IsMatch(_reqData.accountId.ToString(), @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$") == false)
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = MOBILE_ERROR_CODE.MOBILE_PHONE_VERIFY;
                    return _rspData;
                }

                //检查帐号(手机)是否存在
                if (!new BCW.BLL.User().Exists(_reqData.accountId.ToString()))
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND;
                    return _rspData;
                }
            }
            else
            {
                //检查用户ID是否合法
                if (Regex.IsMatch(_reqData.accountId.ToString(), @"^[0-9]\d*$") == false)
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = MOBILE_ERROR_CODE.SYS_USER_ACCOUNT_VERIFY;
                    return _rspData;
                }

                //检查帐号(ID)是否存在
                if (!new BCW.BLL.User().Exists(int.Parse(_reqData.accountId)))
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND;
                    return _rspData;
                }

                //将ID变更为手机
                _reqData.accountId = new BCW.BLL.User().GetMobile(int.Parse(_reqData.accountId));
            }

            
            //检查密码是否合法
            if (Regex.IsMatch(_reqData.newPwd, @"^[A-Za-z0-9]{6,20}$") == false)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = MOBILE_ERROR_CODE.REGEDIT_PWD_VERIFY;
                return _rspData;
            }
            

            //检查手机验证码是否正确
            BCW.Model.tb_Validate _validate = new BCW.BLL.tb_Validate().Gettb_Validate(_reqData.accountId, 5);
            if (_validate == null)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_DIFF;
                return _rspData;
            }

            //检查手机验证码是否有效
            if (_validate.mesCode != _reqData.ValidateCode.ToString())
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_DIFF;
                return _rspData;
            }

            //检查手机验证码是否过期
            if (DateTime.Now > _validate.codeTime || _validate.Flag == 0)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_EXPIRE;
                return _rspData;
            }

            new BCW.BLL.User().UpdateUsPwd(_reqData.accountId, Utils.MD5Str(_reqData.newPwd));

            _rspData.header.status = ERequestResult.success;
            return _rspData;
        } 

        
    }
}
