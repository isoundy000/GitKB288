using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;

namespace BCW.Mobile.Error
{
    public class ErrorCodeManager
    {
        private static ErrorCodeManager mInstance;

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

            #region
            dctErrorCode.Add(MOBILE_ERROR_CODE.SYS_USER_COBI_NOT_ENOUGH, "酷币不足");
            
            #endregion

            #region 论坛
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_NOT_FOUND, "找不到该论坛或该论坛已暂停使用");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH, "论坛权限不足");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_LEVEL, "访问等级不足");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_VIP, "本论坛限VIP会员才可以浏览");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_BBS_MODERATOR, "本论坛限版主和管理员才可以浏览");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_ADMIN, "本论坛限管理员才可以浏览");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_IP, "本论坛限指定ID进入，您的ID不在指定ID中");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_MOBILE, "本论坛限手机才能访问");

            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_TYPE_ERROR, "帖子类型错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_ID_ERROR, "论坛ID错误");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_TITLE_LENGTH_ERROR, "标题限{0}-{1}字");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_CONTENT_LENGTH_ERROR, "请输入{0}-{1}字的内容");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_THREAD_NUM, "每天发贴的数量超过系统限制");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_REPLY_NUM, "每天回贴的数量超过系统限制");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_VIP, "本论坛限VIP会员才可以操作");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ALLMODE, "本论坛限版主和管理员才可以操作");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ADMIN, "本论坛限管理员才可以操作");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_ADD_STOP, "本论坛已禁止该操作");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND, "找不到对应帖子");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_IS_LOCK, "帖子已被锁定");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_IS_TOP, "帖子已被置顶");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_IS_GOOD, "帖子已被加精");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_IS_RECOM, "帖子已被推荐");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_IS_OVER, "帖子已结帖");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_DEL_FORBID, "本版帖子不能删除");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_THREAD_OPER_MYSELF, "不能操作自己的帖子");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_SIGNIN_HAS_TODAY, "今天已经签过到");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_GROUP_NOT_EXISTS, "不存在该圈子");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_GROUP_EXPIRE, "圈子已经过期");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_GROUP_CLOSED, "圈子论坛已关闭");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_GROUP_VISIT_NO_LIMIT, "非成员不能访问该圈子");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FORUM_FORBID_DEL_REPLY, "该论坛不允许删除评论");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_REPLY_NOT_FOUND, "找不到该条评论");

            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FAVORITES_NOT_EXISTS, "不存在的收藏记录");
            dctErrorCode.Add(MOBILE_ERROR_CODE.BBS_FAVORITES_EXISTS, "重复收藏");
            

            #endregion

            #region 公告、喇叭
            dctErrorCode.Add(MOBILE_ERROR_CODE.NETWORK_SUONA_REGDAY_NOT_ENOUGH, "注册不到{0}天不能发布广播");
            dctErrorCode.Add(MOBILE_ERROR_CODE.NETWORK_SUONA_LEVEL_NOT_ENOUGH, "发布广播需要等级{0}级");
            dctErrorCode.Add(MOBILE_ERROR_CODE.NETWORK_SUONA_CONTENT_LENGTH_ERROR, "内容限{0}-{1}字");
            dctErrorCode.Add(MOBILE_ERROR_CODE.NETWORK_SUONA_TIME_LENGTH_ERROR, "显示时长限1-{0}分钟");
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
