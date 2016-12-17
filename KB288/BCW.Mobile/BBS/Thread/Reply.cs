using BCW.Mobile.Protocol;
using System.Data;
using System.Collections.Generic;


namespace BCW.Mobile.BBS.Thread
{



    /// <summary>
    /// 回贴管理
    /// </summary>
    public  class ReplyManager
    {
        private static ReplyManager mInstance;

        public static ReplyManager Instance()
        {
            if (mInstance == null)
                mInstance = new ReplyManager();
            return mInstance;
        }


        public RspReplyList GetReplyList(ReqReplyList _reqData)
        {
            RspReplyList _rspData = new RspReplyList();

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

            //检查帖子有效性
            BCW.Model.Text threadModel = new BCW.BLL.Text().GetText(_reqData.threadId);//GetTextMe
            if (threadModel == null)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspData;
            }

            //检查论坛访问限制
            BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(threadModel.ForumId);
            BCW.User.Users.ShowForumLimit(_reqData.userId, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);

            //得到帖子类型
            int TextUsId = 0;

            string strWhere = string.Empty;
            string strOrder = string.Empty;

            //查询条件
            strWhere = "Bid=" + _reqData.threadId + " and IsDel=0 ";

            switch (_reqData.showType)
            {
                case 1:
                    strWhere += " and IsGood=1";
                    break;
                case 2:
                    strWhere += " and IsTop=1";
                    break;
                case 3:
                    TextUsId = new BCW.BLL.Text().GetUsID(_reqData.threadId);       //得到该帖作者
                    strWhere += " and UsID=" + TextUsId + "";
                    break;
                case 4:                         //预留(热门评论)
                    break;
            }

            if (_reqData.authorId > 0)
            {
                TextUsId = _reqData.authorId;
                strWhere += " and UsID=" + TextUsId + "";
            }

            if (_reqData.replyId > 0)
                strWhere += " and ID < " + _reqData.replyId + "";

            strOrder = " Order by Istop Desc,AddTime Desc";

            // 开始读取列表
            DataSet _ds = new BCW.BLL.Reply().GetList(" TOP 10 Floor,UsName,Content,FileNum,ReplyId,AddTime,UsID,IsTop,IsGood ", strWhere + strOrder );
            if (_ds.Tables[0].Rows.Count > 0)
            {
                int k = 1;
                for (int i = 0;i< _ds.Tables[0].Rows.Count;i++)
                {
                    
                    RspReplyData _data = new RspReplyData();
                    _data.isTop = int.Parse(_ds.Tables[0].Rows[i]["IsTop"].ToString()) == 1;
                    _data.isGood = int.Parse(_ds.Tables[0].Rows[i]["IsGood"].ToString()) == 1;
                    _data.isFile = int.Parse(_ds.Tables[0].Rows[i]["FileNum"].ToString()) > 0;
                    _data.floor = int.Parse(_ds.Tables[0].Rows[i]["Floor"].ToString());
                    _data.content = BCW.Common.Out.SysUBB(_ds.Tables[0].Rows[i]["Content"].ToString());
                    _data.ReplyId = int.Parse(_ds.Tables[0].Rows[i]["ReplyId"].ToString());
                    _data.addTime = Common.Common.GetLongTime(System.DateTime.Parse(_ds.Tables[0].Rows[i]["AddTime"].ToString()));
                    _data.praise = 0;
                    _data.replyContent = _data.ReplyId > 0 ? BCW.Common.Out.SysUBB(new BCW.BLL.Reply().GetContent(_reqData.threadId, _data.ReplyId)) : "";
                    _rspData.lstReplyData.Add(_data);
                }
            }

            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }
           

        public RspAddReplyThread  AddReplyThread(ReqAddReplyThread _reqData)
        {
            RspAddReplyThread _rspAddReplyThread = new RspAddReplyThread();


            //    //验证用户ID格式
            //    if (_reqData.userId < 0)
            //    {
            //        _rspReplyThread.header.status = ERequestResult.faild;
            //        _rspReplyThread.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
            //        return _rspReplyThread;
            //    }

            //    //检查是否登录状态
            //    if (BCW.Mobile.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            //    {
            //        _rspReplyThread.header.status = ERequestResult.faild;
            //        _rspReplyThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
            //        return _rspReplyThread;
            //    }

            //    //帖子有效性
            //    BCW.Model.Text threadModel = new BCW.BLL.Text().GetText(bid);//GetTextMe
            //    if (threadModel == null)
            //    {
            //        _rspdelThread.header.status = ERequestResult.faild;
            //        _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
            //        return _rspdelThread;
            //    }

            //    //版块是否可用
            //    if (!new BCW.BLL.Forum().Exists2(threadModel.ForumId))
            //    {
            //        _rspdelThread.header.status = ERequestResult.faild;
            //        _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_NOT_FOUND;
            //        return _rspdelThread;
            //    }

            //    //自身权限不足
            //    if (new BCW.User.Limits().IsUserLimit(User.Limits.enumRole.Role_Reply, _reqData.userId) == true)
            //    {
            //        _rspdelThread.header.status = ERequestResult.faild;
            //        _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH;
            //        return _rspdelThread;
            //    }

            //    //板块权限不足
            //    if (Common.CheckUserFLimit(User.FLimits.enumRole.Role_Reply, _reqData.userId, threadModel.ForumId))
            //    {
            //        _rspdelThread.header.status = ERequestResult.faild;
            //        _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
            //        return _rspdelThread;
            //    }

            //    //论坛限制性
            //    BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(threadModel.ForumId);
            //    //圈子限制性
            //    if (CheckGroupLimit(Forummodel.)




            _rspAddReplyThread.header.status = ERequestResult.success;
            return _rspAddReplyThread;
        }

        //private Error.MOBILE_ERROR_CODE CheckGroupLimit(int _forumId, int _meid)
        //{
        //    //论坛限制性
        //    BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(_forumId);
        //    //圈子限制性
        //    BCW.Model.Group modelgr = null;
        //    if (Forummodel.GroupId > 0)
        //    {
        //        modelgr = new BCW.BLL.Group().GetGroupMe(Forummodel.GroupId);
        //        if (modelgr == null)
        //        {
        //            return Error.MOBILE_ERROR_CODE.BBS_GROUP_NOT_EXISTS;
        //        }
        //        else if (DT.FormatDate(modelgr.ExTime, 0) != "1990-01-01 00:00:00" && modelgr.ExTime < DateTime.Now)
        //        {
        //            return Error.MOBILE_ERROR_CODE.BBS_GROUP_EXPIRE;
        //        }
        //        if (modelgr.ForumStatus == 2)
        //        {
        //            return Error.MOBILE_ERROR_CODE.BBS_GROUP_CLOSED;
        //        }
        //        if (modelgr.ForumStatus == 1)
        //        {
        //            string GroupId = new BCW.BLL.User().GetGroupId(meid);
        //            if (GroupId.IndexOf("#" + Forummodel.GroupId + "#") == -1 && IsCTID(meid) == false)
        //            {
        //                return Error.MOBILE_ERROR_CODE.BBS_GROUP_VISIT_NO_LIMIT;
        //            }
        //        }
        //    }
        //    return Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE;         //无错误
        //}
    }
}
