using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;

namespace BCW.Mobile.Common
{
    public static class Common
    {
        /// <summary>
        /// 检查会员是否登录 
        /// </summary>
        /// <param name="_userId">会员ID</param>
        /// <param name="_userKey">会员Key</param>
        /// <returns></returns>
        public static int CheckLogin(int _userId, string _userKey)
        {
            BCW.Model.User _user = new BCW.BLL.User().GetKey(_userId);
            if (_user != null && _user.UsKey == _userKey)
                return _user.ID;
            return 0;
        }

        /// <summary>
        /// 论坛发贴权限检查
        /// </summary>
        /// <param name="meid">会员Id</param>
        /// <param name="Postlt">1:是否VIP有权限发帖  2:是否版主或管理员才可发帖  3:是否管理员才可发帖 4:论坛禁止发帖</param>
        public static BCW.Mobile.Error.MOBILE_ERROR_CODE ShowAddThread(int meid, int Postlt)
        {
            bool flag = false;
            Error.MOBILE_ERROR_CODE _result = Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;
            switch (Postlt)
            {
                case 1:
                    int num2 = BCW.User.Users.VipLeven(meid);
                    flag = (num2 != 0);
                    if (!flag)
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_VIP;
                    break;
                case 2:
                    flag = new BCW.BLL.Role().IsAllMode(meid);
                    if (!flag)
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ALLMODE;
                    break;
                case 3:
                    flag = new BCW.BLL.Role().IsAdmin(meid);
                    if (!flag)
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ADMIN;
                    break;
                case 4:
                    _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_STOP;
                    break;
                default:
                    _result = Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;
                    break;
            }
            return _result;
        }

        /// <summary>
        /// 检查论坛访问限制
        /// </summary>
        /// <param name="meid">会员ID</param>
        /// <param name="Gradelt">可以访问的级别</param>
        /// <param name="Visitlt">特定访问对象（如管理员、版主）</param>
        /// <param name="VisitId">特定访问IP</param>
        /// <param name="IsPc">是否PC</param>
        /// <returns></returns>
        public static BCW.Mobile.Error.MOBILE_ERROR_CODE ShowForumLimit(int meid, int Gradelt, int Visitlt, string VisitId, int IsPc)
        {
            bool flag = false;
            #region Gradelt
            flag = (Gradelt <= 0);
            if (!flag)
            {
                if (meid == 0)
                    return Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;  
                //
                int leven = new BCW.BLL.User().GetLeven(meid);
                flag = (leven >= Gradelt);
                if (!flag)
                    return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_LEVEL;    
            }
            #endregion
            //
            #region Visitlt
            flag = (Visitlt <= 0);
            if (!flag)
            {
                if (meid == 0)
                    return Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                //
                switch (Visitlt)
                {
                    case 2:
                        int num2 = BCW.User.Users.VipLeven(meid);
                        if (num2 == 0)
                            return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_VIP;
                        break;
                    case 3:
                        flag = new BCW.BLL.Role().IsAllModeNoGroup(meid);
                        if (!flag)
                            return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_BBS_MODERATOR;
                        break;
                    case 4:
                        flag = new BCW.BLL.Role().IsAdmin(meid);
                        if (!flag)
                            return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_ADMIN;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            //
            #region VisitID
            flag = string.IsNullOrEmpty(VisitId);
            if (!flag)
            {
                if (meid == 0)
                    return Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                //
                VisitId = "#" + VisitId + "#";
                flag = (VisitId.IndexOf("#" + meid + "#") != -1);
                if (!flag)
                    return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_IP;
            }
            #endregion
            //
            #region IsPc
            flag = (IsPc != 1);
            if (!flag)
            {
                flag = Utils.IsMobileUa();
                if (!flag)
                    return Error.MOBILE_ERROR_CODE.BBS_FORUM_VISIT_LIMIT_MOBILE;
            }

            return Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;
            #endregion
        }

        /// <summary>
        /// 检查论坛回帖权限 
        /// </summary>
        /// <param name="meid"></param>
        /// <param name="Replylt"></param>
        public static BCW.Mobile.Error.MOBILE_ERROR_CODE ShowAddReply(int meid, int Replylt)
        {
            Error.MOBILE_ERROR_CODE _result = Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;
            if (Replylt <= 0)
                return _result;
            //
            bool flag = false;           
            switch (Replylt)
            {
                case 1:
                    int num2 = BCW.User.Users.VipLeven(meid);
                    if (num2 == 0)
                    {
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_VIP;
                    }
                    break;
                case 2:
                    flag = new BCW.BLL.Role().IsAllMode(meid);
                    if (!flag)
                    {
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ALLMODE;
                    }
                    break;
                case 3:
                    flag = new BCW.BLL.Role().IsAdmin(meid);
                    if (!flag)
                    {
                        _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_IS_ADMIN;
                    }
                    break;
                case 4:
                    _result = Error.MOBILE_ERROR_CODE.BBS_THREAD_ADD_STOP;
                    break;
                default:
                    break;
            }
            return _result;
            //
        }


        /// <summary>
        /// 检查会员自身权限
        /// </summary>
        /// <param name="objEnumRole">权限类型</param>
        /// <param name="uid">会员ID</param>
        /// <returns>False:有权限 ，True:没有权限</returns>
        public static bool IsUserLimit(BCW.User.Limits.enumRole objEnumRole, int uid)
        {
            return new BCW.User.Limits().IsUserLimit(objEnumRole, uid);
        }

        /// <summary>
        /// 检查会员版块内权限
        /// </summary>
        /// <param name="objEnumRole">权限类型</param>
        /// <param name="uid">会员ID</param>
        /// <param name="forumid">版块ID</param>
        /// <returns>False:有权限 ，True:没有权限 </returns>
        public static bool CheckUserFLimit(BCW.User.FLimits.enumRole objEnumRole, int uid, int forumid)
        {
            string text = string.Empty;
            bool flag = false;
            if (uid <= 0)
                return flag;
            //
            text = new BCW.BLL.Blacklist().GetRole(uid, forumid);
            if (string.IsNullOrEmpty(text))
                return flag;
            //

            switch (objEnumRole)
            {
                case BCW.User.FLimits.enumRole.Role_Text:
                    flag = text.Contains("a");
                    break;
                case BCW.User.FLimits.enumRole.Role_Reply:
                    flag = text.Contains("b");
                    break;
                default:
                    break;
            }
            return (flag);
        }


        /// <summary>
        /// 取得时间截
        /// </summary>
        /// <param name="_datetime"></param>
        /// <returns></returns>
        public static long GetLongTime(DateTime _datetime)
        {
            System.DateTime _startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (long)(_datetime - _startTime).TotalSeconds;
        }

        public static Error.MOBILE_ERROR_CODE CheckGroupLimit(int _forumId, int _meid)
        {
            //论坛限制性
            BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(_forumId);
            //圈子限制性
            BCW.Model.Group modelgr = null;
            if (Forummodel.GroupId > 0)
            {
                modelgr = new BCW.BLL.Group().GetGroupMe(Forummodel.GroupId);
                if (modelgr == null)
                {
                    return Error.MOBILE_ERROR_CODE.BBS_GROUP_NOT_EXISTS;
                }
                else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
                {
                    return Error.MOBILE_ERROR_CODE.BBS_GROUP_EXPIRE;
                }
                if (modelgr.ForumStatus == 2)
                {
                    return Error.MOBILE_ERROR_CODE.BBS_GROUP_CLOSED;
                }
                if (modelgr.ForumStatus == 1)
                {
                    string GroupId = new BCW.BLL.User().GetGroupId(_meid);
                    if (GroupId.IndexOf("#" + Forummodel.GroupId + "#") == -1 && IsCTID(_meid) == false)
                    {
                        return Error.MOBILE_ERROR_CODE.BBS_GROUP_VISIT_NO_LIMIT;
                    }
                }
            }
            return Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;         //无错误
        }

        /// <summary>
        /// 穿透圈子限制ID
        /// </summary>
        private static bool IsCTID(int meid)
        {
            bool Isvi = false;
            //能够穿透的ID
            string CTID = "#" + ub.GetSub("GroupCTID", "/Controls/group.xml") + "#";
            if (CTID.IndexOf("#" + meid + "#") != -1)
            {
                Isvi = true;
            }

            return Isvi;
        }

    }
}
