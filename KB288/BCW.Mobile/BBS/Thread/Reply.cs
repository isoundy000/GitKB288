using BCW.Mobile.Protocol;
using System.Data;
using System.Collections.Generic;
using BCW.Common;
using System;
using System.Text.RegularExpressions;

namespace BCW.Mobile.BBS.Thread
{



    /// <summary>
    /// 回贴管理
    /// </summary>
    public  class ReplyManager
    {
        private static ReplyManager mInstance;
        protected string xmlPath = "../../Controls/bbs.xml";

        public static ReplyManager Instance()
        {
            if (mInstance == null)
                mInstance = new ReplyManager();
            return mInstance;
        }


        public RspReplyList GetReplyList(ReqReplyList _reqData)
        {
            RspReplyList _rspData = new RspReplyList();
            _rspData.isFinish = true;

            //验证用户ID格式
            if (_reqData.userId > 0)
            {
                if (Common.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                    return _rspData;
                }
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
            //BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(threadModel.ForumId);
            //Error.MOBILE_ERROR_CODE _error =  Common.Common.ShowForumLimit(_reqData.userId, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
            //if (_error != Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE)
            //{
            //    _rspData.header.status = ERequestResult.faild;
            //    _rspData.header.statusCode = _error;
            //    return _rspData;
            //}


            //得到帖子类型
            int TextUsId = 0;

            string strWhere = string.Empty;
            string strOrder = string.Empty;

            strWhere = " IsDel=0 ";

            //查询条件
            if (_reqData.threadId > 0)
                strWhere += " and Bid=" + _reqData.threadId;           

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
            DataSet _ds = new BCW.BLL.Reply().GetList(" TOP 10 ID,Floor,UsName,Content,FileNum,ReplyId,AddTime,UsID,IsTop,IsGood ", strWhere + strOrder );
            if (_ds.Tables[0].Rows.Count > 0)
            {
                int k = 1;
                for (int i = 0;i< _ds.Tables[0].Rows.Count;i++)
                {
                    
                    RspReplyData _data = new RspReplyData();
                    _data.id= int.Parse(_ds.Tables[0].Rows[i]["ID"].ToString());
                    _data.isTop = int.Parse(_ds.Tables[0].Rows[i]["IsTop"].ToString()) == 1;
                    _data.isGood = int.Parse(_ds.Tables[0].Rows[i]["IsGood"].ToString()) == 1;
                    _data.isFile = int.Parse(_ds.Tables[0].Rows[i]["FileNum"].ToString()) > 0;
                    _data.floor = int.Parse(_ds.Tables[0].Rows[i]["Floor"].ToString());
                    _data.content = BCW.Common.Out.SysUBB(_ds.Tables[0].Rows[i]["Content"].ToString());
                    _data.ReplyId = int.Parse(_ds.Tables[0].Rows[i]["ReplyId"].ToString());

                    BCW.Model.Reply _replyData = new BCW.BLL.Reply().GetReplyMe(_reqData.threadId, _data.ReplyId);
                    if (_replyData != null)
                    {
                        _data.replyContent = _data.ReplyId > 0 ? BCW.Common.Out.SysUBB(_replyData.Content) : "";
                        _data.replyAuthorId = _replyData.UsID ;
                        _data.replyAuthorName = _replyData.UsName;
                        _data.replyAddTime = Common.Common.GetLongTime(_replyData.AddTime);
                    }
                    
                    _data.addTime = Common.Common.GetLongTime(System.DateTime.Parse(_ds.Tables[0].Rows[i]["AddTime"].ToString()));
                    _data.praise = 0;
                    _data.authorId = int.Parse(_ds.Tables[0].Rows[i]["UsID"].ToString());
                    _data.authorName = _ds.Tables[0].Rows[i]["UsName"].ToString();
                    _data.authorImg =  "http://" + Utils.GetDomain() + new BCW.BLL.User().GetPhoto(_data.authorId);                    
                    _rspData.lstReplyData.Add(_data);


                    //检查是否到底
                    if (i == _ds.Tables[0].Rows.Count - 1)
                    {
                        if (strWhere.Contains("1=1") == false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Reply().GetList(" TOP 10 ID,Floor,UsName,Content,FileNum,ReplyId,AddTime,UsID,IsTop,IsGood ", strWhere.Replace("1=1", "ID<" + _data.id) + strOrder);
                            _rspData.isFinish = _dsCheck.Tables[0].Rows.Count <= 0;

                        _rspData.serverTime = Common.Common.GetLongTime(DateTime.Now);
                    }
                }
            }

            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }
           

        public RspAddReplyThread  AddReplyThread(ReqAddReplyThread _reqData)
        {
            RspAddReplyThread _rspData = new RspAddReplyThread();


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

            //检查自身权限不足
            if (Common.Common.IsUserLimit(BCW.User.Limits.enumRole.Role_Reply, _reqData.userId) == true)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH;
                return _rspData;
            }

            //板块权限不足
            if (Common.Common.CheckUserFLimit(BCW.User.FLimits.enumRole.Role_Reply, _reqData.userId, threadModel.ForumId))
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                return _rspData;
            }

            
            BCW.Model.Forum Forummodel = new BCW.BLL.Forum().GetForum(threadModel.ForumId);

            //检查圈子访问限制
            Error.MOBILE_ERROR_CODE _groupError = Common.Common.CheckGroupLimit(threadModel.ForumId, _reqData.userId);
            if (_groupError != Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = _groupError;
                return _rspData;
            }

            //检查论坛访问限制
            Error.MOBILE_ERROR_CODE _visitError = Common.Common.ShowForumLimit(_reqData.userId, Forummodel.Gradelt, Forummodel.Visitlt, Forummodel.VisitId, Forummodel.IsPc);
            if (_visitError != Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = _visitError;
                return _rspData;
            }

            //检查论坛回帖限制
            Error.MOBILE_ERROR_CODE _replyError = Common.Common.ShowAddReply(_reqData.userId, Forummodel.Replylt);
            if (_replyError != Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = _replyError;
                return _rspData;
            }

            BCW.Model.Text p = new BCW.BLL.Text().GetText(_reqData.threadId);
            if (p.IsOver == 1)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_OVER;
                return _rspData;
            }
            if (p.IsLock == 1)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode =  Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_LOCK;
                return _rspData;
            }
            if (p.IsTop == -1)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_BOTTOM;
                return _rspData;
            }

            string Content = _reqData.replyContent;
            if (Regex.IsMatch(Content, @"^[\s\S]{1," + ub.GetSub("BbsReplyMax", xmlPath) + "}$") == false)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_CONTENT_LENGTH_ERROR;
                _rspData.header.statusMsg = string.Format(_rspData.header.statusMsg, 1, ub.GetSub("BbsReplyMax", xmlPath));
                return _rspData;
            }


            int Remind = _reqData.Remind;  //提醒的ID.
            int reid = _reqData.replyId;


            int ReplyNum = Utils.ParseInt(ub.GetSub("BbsReplyNum", xmlPath));
            if (ReplyNum > 0)
            {
                int ToDayCount = new BCW.BLL.Forumstat().GetCount(_reqData.userId, 2);//今天发布回帖数
                if (ToDayCount >= ReplyNum)
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_REPLY_NUM;
                    return _rspData;
                }
            }

            string mename = new BCW.BLL.User().GetUsName(_reqData.userId);
            int Floor = new BCW.BLL.Reply().GetFloor(_reqData.threadId);

            //派币帖
            string CentText = string.Empty;
            string PbCent = string.Empty;
            int iTypes = p.Types;
            if (iTypes == 3)
            {
                BCW.Model.Text model1 = new BCW.BLL.Text().GetText(_reqData.threadId);
                if (p.Prices - p.Pricel > 0)
                {
                    string bzText = string.Empty;
                    if (p.BzType == 0)
                        bzText = ub.Get("SiteBz");
                    else
                        bzText = ub.Get("SiteBz2");

                    long zPrice = 0;
                    if (p.Price2 > 0)
                        zPrice = Convert.ToInt64(new Random().Next(p.Price, (p.Price2 + 1)));//随机得到奖币值
                    else
                        zPrice = Convert.ToInt64(p.Price);

                    long GetPrice = 0;
                    if (p.Prices - p.Pricel < zPrice)
                        GetPrice = p.Prices - p.Pricel;
                    else
                        GetPrice = zPrice;

                    bool a = ("#" + p.IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1;

                    if (p.PayCi == "0")  //判断派币楼层
                    {
                        if (!string.IsNullOrEmpty(model1.PricesLimit))//如果要求回复特殊内容
                        {

                            // builder.Append("判断的TF"+a);
                            //  if (model1.PricesLimit.Equals(Content))  //如果回帖正确
                            if (model1.PricesLimit.Replace(" ", "").Equals(Content.Replace(" ", "")))  //如果回复附言正确
                            {
                                if (("#" + p.IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1) //判断是否存在已派币ID
                                {
                                    if (p.BzType == 0)
                                        new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, GetPrice, "派币帖回帖获得");
                                    else
                                        new BCW.BLL.User().UpdateiMoney(_reqData.userId, mename, GetPrice, "派币帖回帖获得");

                                    //更新已派
                                    new BCW.BLL.Text().UpdatePricel(_reqData.threadId, GetPrice);
                                    CentText = "" + GetPrice + "" + bzText + "";
                                    PbCent = "楼主派" + GetPrice + "" + bzText + "";
                                    //更新派币ID
                                    string IsPriceID = p.IsPriceID;
                                    if (("#" + IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1)
                                    {
                                        string sPriceID = string.Empty;
                                        if (string.IsNullOrEmpty(IsPriceID))
                                            sPriceID = _reqData.userId.ToString();
                                        else
                                            sPriceID = IsPriceID + "#" + _reqData.userId;
                                        new BCW.BLL.Text().UpdateIsPriceID(_reqData.threadId, sPriceID);
                                    }
                                }
                            }

                        }
                        else //不需要回复内容
                        {
                            //builder.Append("判断的TF" + a);
                            if (("#" + p.IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1)  //判断是否存在已派币ID
                            {
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(_reqData.userId, mename, GetPrice, "派币帖回帖获得");

                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(_reqData.threadId, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "楼主派" + GetPrice + "" + bzText + "";
                                //更新派币ID
                                string IsPriceID = p.IsPriceID;
                                if (("#" + IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1)
                                {
                                    string sPriceID = string.Empty;
                                    if (string.IsNullOrEmpty(IsPriceID))
                                        sPriceID = _reqData.userId.ToString();
                                    else
                                        sPriceID = IsPriceID + "#" + _reqData.userId;
                                    new BCW.BLL.Text().UpdateIsPriceID(_reqData.threadId, sPriceID);
                                }
                            }
                        }

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(model1.PricesLimit))//如果要求回复特殊内容
                        {
                            if (("#" + p.PayCi + "#").IndexOf("#" + Utils.Right(Floor.ToString(), 1) + "#") != -1) //判断要求派币的楼层
                            {
                                if (model1.PricesLimit.Replace(" ", "").Equals(Content.Replace(" ", "")))  //如果回复附言正确
                                                                                                           // if (model1.PricesLimit.Equals(Content))  //如果回帖正确
                                {
                                    // builder.Append("判断的TF" + a);
                                    //if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1) //判断是否存在已派币ID
                                    //{
                                    if (p.BzType == 0)
                                        new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, GetPrice, "派币帖回帖获得");
                                    else
                                        new BCW.BLL.User().UpdateiMoney(_reqData.userId, mename, GetPrice, "派币帖回帖获得");

                                    //更新已派
                                    new BCW.BLL.Text().UpdatePricel(_reqData.threadId, GetPrice);
                                    CentText = "" + GetPrice + "" + bzText + "";
                                    PbCent = "踩中楼层" + Utils.Right(Floor.ToString(), 1) + "尾，楼主派" + GetPrice + "" + bzText + "";
                                    //更新派币ID
                                    string IsPriceID = p.IsPriceID;
                                    if (("#" + IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1)
                                    {
                                        string sPriceID = string.Empty;
                                        if (string.IsNullOrEmpty(IsPriceID))
                                            sPriceID = _reqData.userId.ToString();
                                        else
                                            sPriceID = IsPriceID + "#" + _reqData.userId;
                                        new BCW.BLL.Text().UpdateIsPriceID(_reqData.threadId, sPriceID);
                                    }
                                    //}
                                }
                            }
                        }
                        else //不需要回复内容
                        {
                            if (("#" + p.PayCi + "#").IndexOf("#" + Utils.Right(Floor.ToString(), 1) + "#") != -1)
                            {
                                // builder.Append("判断的TF" + a);
                                //if (("#" + p.IsPriceID + "#").IndexOf("#" + meid + "#") == -1) //判断是否存在已派币ID
                                //{
                                if (p.BzType == 0)
                                    new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, GetPrice, "派币帖回帖获得");
                                else
                                    new BCW.BLL.User().UpdateiMoney(_reqData.userId, mename, GetPrice, "派币帖回帖获得");
                                //更新已派
                                new BCW.BLL.Text().UpdatePricel(_reqData.threadId, GetPrice);
                                CentText = "" + GetPrice + "" + bzText + "";
                                PbCent = "踩中楼层" + Utils.Right(Floor.ToString(), 1) + "尾，楼主派" + GetPrice + "" + bzText + "";
                                //更新派币ID
                                string IsPriceID = p.IsPriceID;
                                if (("#" + IsPriceID + "#").IndexOf("#" + _reqData.userId + "#") == -1)
                                {
                                    string sPriceID = string.Empty;
                                    if (string.IsNullOrEmpty(IsPriceID))
                                        sPriceID = _reqData.userId.ToString();
                                    else
                                        sPriceID = IsPriceID + "#" + _reqData.userId;
                                    new BCW.BLL.Text().UpdateIsPriceID(_reqData.threadId, sPriceID);
                                }
                                //}
                            }
                        }

                    }
                    //检测15天前的派币帖，如果没有派完则自动清0并自动结帖
                    if (Utils.GetTopDomain().Contains("tuhao") || Utils.GetTopDomain().Contains("th"))
                    {
                        BCW.Data.SqlHelper.ExecuteSql("update tb_Text set Pricel=Prices,IsOver=1 where Types=3 and AddTime<'" + DateTime.Now.AddDays(-15) + "'");
                    }
                    else
                    {
                        BCW.Data.SqlHelper.ExecuteSql("update tb_Text set Pricel=Prices,IsOver=1 where Types=3 and AddTime<'" + DateTime.Now.AddDays(-7) + "'");

                    }
                }
                else
                {
                    //派完币即结帖
                    new BCW.BLL.Text().UpdateIsOver(_reqData.threadId, 1);
                }
            }


            BCW.Model.Reply model = new BCW.Model.Reply();
            model.Floor = Floor;
            model.ForumId = threadModel.ForumId;
            model.Bid = _reqData.threadId;
            model.UsID = _reqData.userId;
            model.UsName = mename;
            model.Content = Content;
            model.FileNum = 0;
            model.ReplyId = reid;
            model.AddTime = DateTime.Now;
            model.CentText = CentText;
            new BCW.BLL.Reply().Add(model);

            //builder.Append("p.IsPriceID=" + p.IsPriceID);

            //更新回复ID
            string sReplyID = p.ReplyID;
            if (("#" + sReplyID + "#").IndexOf("#" + _reqData.userId + "#") == -1)
            {
                string ReplyID = string.Empty;
                if (string.IsNullOrEmpty(sReplyID))
                    ReplyID = _reqData.userId.ToString();
                else
                    ReplyID = sReplyID + "#" + _reqData.userId;
                new BCW.BLL.Text().UpdateReplyID(_reqData.threadId, ReplyID);
            }

            //更新回复数
            new BCW.BLL.Text().UpdateReplyNum(_reqData.threadId, 1);

            //回复提醒:0|不提醒|1|帖子作者|2|回帖作者|3|全部提醒
            string strRemind = string.Empty;
            //提醒费用
            long Tips = Convert.ToInt64(ub.GetSub("BbsReplyTips", xmlPath));
            if (Remind == 1 || Remind == 3)
            {
                if (!p.UsID.Equals(_reqData.userId))
                {
                    string pForumSet = new BCW.BLL.User().GetForumSet(p.UsID);
                    if (BCW.User.Users.GetForumSet(pForumSet, 14) == 0)
                    {
                        if (new BCW.BLL.User().GetGold(_reqData.userId) >= Tips)
                        {

                            new BCW.BLL.Guest().Add(p.UsID, p.UsName, "[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + mename + "[/url]回复了您的帖子[url=/bbs/topic.aspx?forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "]" + p.Title + "[/url]");
                            if (Tips > 0)
                            {
                                new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, -Tips, "回帖提醒帖子作者");
                            }
                        }
                    }
                    else
                    {
                        strRemind = "帖子作者拒绝接收提醒消息.<br />";
                    }
                }
            }


            if (Remind == 2 || Remind == 3)
            {
                //回帖用户实体
                BCW.Model.Reply m = new BCW.BLL.Reply().GetReplyMe(_reqData.threadId, reid);
                if (!m.UsID.Equals(_reqData.userId))
                {
                    string mForumSet = new BCW.BLL.User().GetForumSet(m.UsID);
                    if (BCW.User.Users.GetForumSet(mForumSet, 14) == 0)
                    {
                        if (new BCW.BLL.User().GetGold(_reqData.userId) >= Tips)
                        {
                            string neirong = new BCW.BLL.Reply().GetContent(_reqData.threadId, reid);
                            if (neirong.Length > 30)
                            {
                                neirong = neirong.Substring(0, 30);
                                neirong += "...";
                                //builder.Append(":" + neirong);
                            }
                            else
                            {
                                // builder.Append(":" + neirong);
                            }
                            if (Content.Length > 30)
                            {
                                Content = Content.Substring(0, 30);
                                Content += "...";
                                //builder.Append(":" + neirong);
                            }
                            //  修改这里
                            // builder.Append("<a href=\"" + Utils.getUrl("reply.aspx?act=reply&amp;forumid=" + forumid + "&amp;bid=" + bid + "&amp;reid=" + reid + "&amp;backurl=" + Utils.getPage(0) + "") + "\">点评回复</a>|");
                            new BCW.BLL.Guest().Add(m.UsID, m.UsName, "[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + mename + "[/url]点评了您的回帖[url=/bbs/reply.aspx?act=view&amp;forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "&amp;reid=" + reid + "]" + reid + "楼[/url]:" + neirong + "<br/>回复内容为:" + Content + "[url=/bbs/reply.aspx?act=view&amp;forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "&amp;reid=" + Floor + "]更多[/url]<br/>[url=/bbs/reply.aspx?act=reply&amp;forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "&amp;reid=" + Floor + "]点评回复[/url]");
                            if (Tips > 0)
                            {
                                new BCW.BLL.User().UpdateiGold(_reqData.userId, mename, -Tips, "回帖提醒回帖作者");
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strRemind))
                            strRemind = "帖子作者与回帖作者拒绝接收提醒消息.<br />";
                        else
                            strRemind = "回帖作者拒绝接收提醒消息.<br />";
                    }
                }
            }
            //论坛统计
            BCW.User.Users.UpdateForumStat(2, _reqData.userId, mename, threadModel.ForumId);
            //动态记录
            if (Forummodel.GroupId == 0)
            {
                new BCW.BLL.Action().Add(-1, 0, _reqData.userId, mename, "在" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "]" + new BCW.BLL.Text().GetTitle(_reqData.threadId) + "[/URL]");
            }
            else
            {
                new BCW.BLL.Action().Add(-2, 0, _reqData.userId, mename, "在圈坛-" + Forummodel.Title + "回复帖子[URL=/bbs/topic.aspx?forumid=" + threadModel.ForumId + "&amp;bid=" + _reqData.threadId + "]" + new BCW.BLL.Text().GetTitle(_reqData.threadId) + "[/URL]");
            }
            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int IsAcc = -1;
            if (Forummodel.GroupId == 0)
            {
                IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, _reqData.userId, true);
            }
            else
            {
                if (!Utils.GetDomain().Contains("th"))
                    IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Reply, _reqData.userId, false);
            }

            _rspData.header.status = ERequestResult.success;
            if (IsAcc >= 0)
                _rspData.rewardItem = BCW.User.Users.GetWinCent(1, _reqData.userId);

            _rspData.header.status = ERequestResult.success;
            return _rspData;
        }

        public RspDelReply DelReply(ReqDelReply _reqData)
        {
            RspDelReply _rspData = new RspDelReply();

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


            //检查该论坛是否允许删除
            if (new BCW.User.ForumInc().IsForum68(threadModel.ForumId) == true)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_FORBID_DEL_REPLY;
                return _rspData;
            }


            int reid = _reqData.reid;
            int bid = _reqData.threadId;
            int meid = _reqData.userId;
            int forumid = threadModel.ForumId;

            BCW.Model.Reply model = new BCW.BLL.Reply().GetReplyMe(bid, reid);
            if (model == null)
            {
                _rspData.header.status = ERequestResult.faild;
                _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_REPLY_NOT_FOUND;
                return _rspData;
            }
            if (ub.GetSub("BbsReplyDel", xmlPath) == "0")
            {
                if (model.UsID != meid && !new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelReply, meid, forumid))
                {
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspData;
                }
            }
            else
            {
                //检查自身权限
                if (!new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelReply, meid, forumid))
                {                    
                    _rspData.header.status = ERequestResult.faild;
                    _rspData.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH;
                    return _rspData;
                }
            }


            new BCW.BLL.Reply().UpdateIsDel2(bid, reid, 1);
            new BCW.BLL.Forumstat().Update2(2, model.UsID, forumid, model.AddTime);//更新统计表发帖
                                                                                    //new BCW.BLL.Reply().Delete(bid, reid);
                                                                                    //更新回复数
                                                                                    //new BCW.BLL.Text().UpdateReplyNum(bid, -1);
                
            //记录日志
            string strLog = string.Empty;
            if (model.UsID != meid)
            {
                //积分操作
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelReply, model.UsID);
                }
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + reid + "楼回帖被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]删除!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + reid + "楼回帖被[url=/bbs/uinfo.aspx?uid=" + meid + "]" + new BCW.BLL.User().GetUsName(meid) + "[/url]删除!");
            }
            else
            {
                //积分操作
                int GroupId = new BCW.BLL.Forum().GetGroupId(forumid);
                if (GroupId == 0)
                {
                    new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_MeDelReply, model.UsID);
                }
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]删除自己的跟帖";
            }

            new BCW.BLL.Forumlog().Add(6, forumid, bid, reid, strLog);

            _rspData.header.status = ERequestResult.success;
            return _rspData;         

        }

    }
}
