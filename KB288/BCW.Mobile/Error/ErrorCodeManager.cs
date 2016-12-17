using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;

namespace BCW.Mobile.Error
{
    public class ErrorCodeManager
    {
        private static ErrorCodeManager mInstance;
        private string xmlPath = "../../Controls/bbs.xml";

        private Dictionary<MOBILE_ERROR_CODE, string> dctErrorCode;

        public ErrorCodeManager()
        {
            dctErrorCode = new Dictionary<MOBILE_ERROR_CODE, string>();

            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_MSG_NONE, "");
            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_SYS_ERROR, "系统出现未知错误，请稍候再试...");
            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_SYS_BUSY, "服务器繁忙，请稍候再试...");
            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR, "参数错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH, "自身权限不足");


            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_PHONE_ISNULL, "注册手机号码为空值");
            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_PHONE_VERIFY, "手机号码格式不正确");

            #region 短信验证码
            dctErrorCode.Add(MOBILE_ERROR_CODE.SMS_FREQUENTLY_TODAY, "今日获取短信太频繁，已达本日上限，请明天再试");
            dctErrorCode.Add(MOBILE_ERROR_CODE.SMS_FREQUENTLY_FLAG, "之前存在频繁获取短信，请明天再试");
            dctErrorCode.Add(MOBILE_ERROR_CODE.SMS_FREQUENTLY_IP, "同一IP由于频繁获取短信，请明天再试");
            dctErrorCode.Add(MOBILE_ERROR_CODE.SMS_FREQUENTLY_PHONE, "该号码由于频繁获取短信，请明天再试");
            #endregion

            #region  会员注册
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_MOBILE_EXISTS, "手机号码已经注册");
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_PWD_VERIFY, "密码限6-20位,必须由字母或数字组成");
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_PWDR_VERIFY, "确认密码限6-20位,必须由字母或数字组成");
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_PWD_DIFF, "两次密码输入不一致");
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_EXPIRE, "验证码已过期");
            dctErrorCode.Add(MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_DIFF, "请输入正确的验证码");
            #endregion   

            #region  登录
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_PARAMS_ERROR, "登录参数错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_PLATFORM_USER_NOTFOUND, "未绑定到第三方登录帐号");
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_ACCOUNT_NOTFOUND, "找不到用户帐户信息");
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_BINDPLATFORM_FAILD, "绑定第三方帐号失败");
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_USER_PWD_ERROR, "用户名或密码不正确");
            dctErrorCode.Add(MOBILE_ERROR_CODE.MOBILE_USERID_VERIFY, "帐号格式错误(必须是数字)");
            dctErrorCode.Add(MOBILE_ERROR_CODE.LOGIN_BINDPLATFORM_USE, "该帐号已被其它帐号绑定");
            dctErrorCode.Add(MOBILE_ERROR_CODE.SYS_USER_NOLOGIN, "该会员还未登录");
            #endregion

            #region 论坛
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_NOT_FOUND, "找不到该论坛或该论坛已暂停使用");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH, "论坛权限不足");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_TYPE_ERROR, "帖子类型错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_ID_ERROR, "论坛ID错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_TITLE_LENGTH_ERROR, "标题限" + ub.GetSub("BbsThreadMin", xmlPath) + "-" + ub.GetSub("BbsThreadMax", xmlPath) + "字");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_CONTENT_LENGTH_ERROR, "请输入" + ub.GetSub("BbsContentMin", xmlPath) + "-" + ub.GetSub("BbsContentMax", xmlPath) + "字的内容");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_THREAD_NUM, "每天发贴的数量超过系统限制");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_VIP, "本论坛限VIP会员才可以发帖");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ALLMODE, "本论坛限版主和管理员才可以发帖");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ADMIN, "本论坛限管理员才可以发帖");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_STOP, "本论坛禁止发帖");

            #endregion

        }

        public static ErrorCodeManager Instance()
        {
            if (mInstance == null)
                mInstance = new ErrorCodeManager();

            return mInstance;
        }

        public string GetErrorMsg(MOBILE_ERROR_CODE _code)
        {
            if (dctErrorCode.ContainsKey(_code))
                return dctErrorCode[_code];

            return dctErrorCode[MOBILE_ERROR_CODE.MOBILE_MSG_NONE];
        }

    }
}
