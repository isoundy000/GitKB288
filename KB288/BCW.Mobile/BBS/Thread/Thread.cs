using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile;
using System.Data;
using BCW.Mobile.Home;
using BCW.Common;
using System.Text.RegularExpressions;
using BCW.Mobile.Protocol;

namespace BCW.Mobile.BBS.Thread
{
    /// <summary>
    /// 贴子内容
    /// </summary>
    public class EssencePostItem
    {
        public int threadId;                 //贴子ID
        public int authorId;                //作者id
        public string author;                //作者
        public string authorImg;             //作者头像   
        public int forumId;                  //论坛栏目ID
        public string title;                 //帖子标题
        public string content;               //贴子内容
        public string ubb_content;           //ubb贴子内容
        public string preview;               //预览图
        public string forum;                 //论坛栏目名称
        public int views;                    //阅读数
        public int replys;                   //评论数
        public int likes;                    //喜欢数 
        public long addTime;            //发贴时间
        public int IsGood;                  //是否精华
        public int IsRecom;                 //是否推荐
        public int IsLock;                  //是否锁定
        public int IsTop;                   //是否置顶
    }

    /// <summary>
    /// 论坛精华贴
    /// </summary>
    public class EssencePost
    {
        public List<EssencePostItem> items;
        public bool finish;
        public const int RECORD_COUNT = 10;

        public EssencePost()
        {
            items = new List<EssencePostItem>();
        }

        public void InitData(int ForumId, int postId, int ptype,int _userId)
        {

            this.finish = true;
            items.Clear();

            string strWhere = string.Empty;
            string strOrder = string.Empty;
            string[] pageValUrl = { "act", "forumid", "tsid", "ptype", "backurl" };

            strWhere = "IsDel=0 and HideType<>9";

            if (ptype == 1)
                strWhere += " and IsGood=1";
            else if (ptype == 2)
                strWhere += " and IsRecom=1";
            if (ptype == 3)
                strWhere += " and AddTime>='" + DateTime.Now.AddDays(-2) + "'";
            else if (ptype == 4)
                strWhere += " and IsLock=1";
            else if (ptype == 5)
                strWhere += " and IsTop=-1";

            if (ForumId > 0)
                strWhere += " and ForumId=" + ForumId.ToString();

            if (_userId >0)
                strWhere += " and UsID=" + _userId.ToString();

            if (postId >= 0)
                strWhere += " and 1=1";

            //排序条件
            if (ForumId > 0 && postId < 0)
                strOrder += "IsTop desc,";
            strOrder += "ID Desc";

            DataSet _ds = new BCW.BLL.Text().GetList(string.Format("TOP {0} ID,ForumId,Types,LabelId,Title,Content,UsID,UsName,ReplyNum,ReadNum,IsGood,IsRecom,IsLock,IsTop,IsOver,AddTime,Gaddnum,Gwinnum,GoodSmallIcon", RECORD_COUNT), strWhere.Replace("1=1", "ID<" + postId) + " Order by " + strOrder);
            if (_ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < _ds.Tables[0].Rows.Count; i++)
                {
                    EssencePostItem _essencePostItem = new EssencePostItem();
                    _essencePostItem.threadId = int.Parse(_ds.Tables[0].Rows[i]["ID"].ToString());
                    _essencePostItem.authorId = int.Parse(_ds.Tables[0].Rows[i]["UsID"].ToString());
                    _essencePostItem.author = _ds.Tables[0].Rows[i]["UsName"].ToString();
                    _essencePostItem.authorImg = "http://" + Utils.GetDomain() + new BCW.BLL.User().GetPhoto(int.Parse(_ds.Tables[0].Rows[i]["UsID"].ToString()));
                    _essencePostItem.forumId = int.Parse(_ds.Tables[0].Rows[i]["ForumId"].ToString());
                    _essencePostItem.title = _ds.Tables[0].Rows[i]["Title"].ToString();//.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
                    _essencePostItem.content = Out.SysUBB(_ds.Tables[0].Rows[i]["Content"].ToString());//.Replace("\\", "\\\\").Replace("\"", "\\\"").Replace("\n", "\\n");
                    _essencePostItem.ubb_content =_ds.Tables[0].Rows[i]["Content"].ToString();
                    _essencePostItem.preview = string.IsNullOrEmpty(_ds.Tables[0].Rows[i]["GoodSmallIcon"].ToString()) ? "http://" + Utils.GetDomain() + "/Files/threadImg/def.png" : _ds.Tables[0].Rows[i]["GoodSmallIcon"].ToString();
                    BCW.Model.Forum _forummodel = new BCW.BLL.Forum().GetForum(_essencePostItem.forumId);
                    _essencePostItem.forum = _forummodel != null ? _forummodel.Title : "";
                    _essencePostItem.views = int.Parse(_ds.Tables[0].Rows[i]["ReadNum"].ToString());
                    _essencePostItem.replys = int.Parse(_ds.Tables[0].Rows[i]["ReplyNum"].ToString());
                    System.DateTime _startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
                    _essencePostItem.addTime = (long)(DateTime.Parse(_ds.Tables[0].Rows[i]["AddTime"].ToString()) - _startTime).TotalSeconds;

                    //打赏
                    DataSet _dsCent = new BCW.BLL.Textcent().GetList("isnull(SUM(Cents),0)cents", "BID='" + _essencePostItem.threadId + "'");
                    _essencePostItem.likes = int.Parse(_dsCent.Tables[0].Rows[0]["cents"].ToString());

                    _essencePostItem.IsGood = int.Parse(_ds.Tables[0].Rows[i]["IsGood"].ToString());
                    _essencePostItem.IsRecom = int.Parse(_ds.Tables[0].Rows[i]["IsRecom"].ToString());
                    _essencePostItem.IsLock = int.Parse(_ds.Tables[0].Rows[i]["IsLock"].ToString());
                    _essencePostItem.IsTop = int.Parse(_ds.Tables[0].Rows[i]["IsTop"].ToString());

                    items.Add(_essencePostItem);

                    //检查是否到底
                    if (i == _ds.Tables[0].Rows.Count - 1)
                    {
                        if (strWhere.Contains("1=1") == false)
                            strWhere += " and 1=1";
                        DataSet _dsCheck = new BCW.BLL.Text().GetList("TOP 1 * ", strWhere.Replace("1=1", "ID<" + _essencePostItem.threadId) + " Order by " + strOrder);
                        finish = _dsCheck.Tables[0].Rows.Count <= 0;
                    }
                }
            }

        }
    }


    public class BestInfo
    {
        public Header header;
        public EssencePost bests;    //论坛精华贴  
        
        private string xmlPath = "../../Controls/bbs.xml";

        public BestInfo()
        {
            bests = new EssencePost();
            header = new Header();
        }

        /// <summary>
        /// 获取贴子列表
        /// </summary>
        /// <param name="ForumId">板块ID</param>
        /// <param name="_pIndex">分页ID</param>
        /// <param name="pType">贴子类型(1:精华  2：推荐  3：两日前  4：锁定  5：置顶)</param>
        public void InitData(int ForumId, int _pIndex, int pType,int _userId)
        {
            bests.InitData(ForumId, _pIndex, pType,_userId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ForumId"></param>
        /// <returns>发表贴子结果类</returns>
        public RspAddThread AddThread(ReqAddThread _reqData)
        {
            RspAddThread _rspAddThread = new RspAddThread();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspAddThread;
            }

            //验证帖子类型
            if (Regex.IsMatch(_reqData.pType.ToString(), @"^[0-4]$|^6$|^7$|^8$") == false)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_TYPE_ERROR;
                return _rspAddThread;
            }

            //验证帖子类型
            if (Regex.IsMatch(_reqData.forumId.ToString(), @"^[0-9]\d*$") == false)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_TYPE_ERROR;
                return _rspAddThread;
            }

            //验证贴子标题长度
            if (Regex.IsMatch(_reqData.title, @"^[\s\S]{" + ub.GetSub("BbsThreadMin", xmlPath) + "," + ub.GetSub("BbsThreadMax", xmlPath) + "}$") == false)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_TITLE_LENGTH_ERROR;
                _rspAddThread.header.statusMsg = string.Format(_rspAddThread.header.statusMsg, ub.GetSub("BbsThreadMin", xmlPath), ub.GetSub("BbsThreadMax", xmlPath));
                return _rspAddThread;
            }

            //验证内容长度
            if (Regex.IsMatch(_reqData.content, @"^[\s\S]{" + ub.GetSub("BbsContentMin", xmlPath) + "," + ub.GetSub("BbsContentMax", xmlPath) + "}$") == false)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_CONTENT_LENGTH_ERROR;
                _rspAddThread.header.statusMsg = string.Format(_rspAddThread.header.statusMsg, ub.GetSub("BbsContentMin", xmlPath), ub.GetSub("BbsContentMax", xmlPath));
                return _rspAddThread;
            }


            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspAddThread;
            }

            //版块是否可用
            if (!new BCW.BLL.Forum().Exists2(_reqData.forumId))
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_NOT_FOUND;
                return _rspAddThread;
            }

            //自身权限不足
            if (new BCW.User.Limits().IsUserLimit(User.Limits.enumRole.Role_Text, _reqData.userId) == true)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_LIMIT_NOT_ENOUGH ;
                return _rspAddThread;
            }

            //板块权限不足
            if (Common.CheckUserFLimit(User.FLimits.enumRole.Role_Text,_reqData.userId,_reqData.forumId))
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                return _rspAddThread;
            }

            string mename = new BCW.BLL.User().GetUsName(_reqData.userId);

            int ThreadNum = Utils.ParseInt(ub.GetSub("BbsThreadNum", xmlPath));
            if (ThreadNum > 0)
            {
                int ToDayCount = new BCW.BLL.Forumstat().GetCount(_reqData.userId, 1);//今天发布帖子数
                if (ToDayCount >= ThreadNum)
                {
                    _rspAddThread.header.status = ERequestResult.faild;
                    _rspAddThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_THREAD_NUM;
                    return _rspAddThread;
                }
            }

            BCW.Model.Forum model = new BCW.BLL.Forum().GetForum(_reqData.forumId);

            //论坛限制性
            //BCW.User.Users.ShowForumLimit(_reqData.userId, model.Gradelt, model.Visitlt, model.VisitId, model.IsPc);      //浏览限制
            //发贴限制
            Error.MOBILE_ERROR_CODE _result = BCW.Mobile.Common.ShowAddThread(_reqData.userId, model.Postlt);
            if (_result != Error.MOBILE_ERROR_CODE.MOBILE_MSG_NONE)
            {
                _rspAddThread.header.status = ERequestResult.faild;
                _rspAddThread.header.statusCode = _result;
                return _rspAddThread;
            }
            
            string Hide = string.Empty;
            int Price = 0;
            int Price2 = 0;
            long Prices = 0;
            int BzType = 0;

            int HideType = 0;
            int IsSeen = 0;
            string PayCi = string.Empty;
            string Vote = string.Empty;

            DateTime VoteExTime = DateTime.Now;

            int LabelId = 0;

            BCW.Model.Text addmodel = new BCW.Model.Text();
            addmodel.ForumId = _reqData.forumId;
            addmodel.Types = _reqData.pType;
            addmodel.LabelId = LabelId;
            addmodel.Title = _reqData.title;
            addmodel.Content = _reqData.content;
            addmodel.HideContent = Hide;
            addmodel.UsID = _reqData.userId;
            addmodel.UsName = mename;
            addmodel.Price = Price;
            addmodel.Price2 = Price2;
            addmodel.Prices = Prices;
            addmodel.HideType = HideType;
            addmodel.BzType = BzType;
            addmodel.PayCi = PayCi;
            addmodel.IsSeen = IsSeen;
            addmodel.IsDel = 0;
            addmodel.AddTime = DateTime.Now;
            addmodel.ReTime = DateTime.Now;
            addmodel.PricesLimit = "";

            addmodel.Gaddnum = 0;
            addmodel.Gqinum = 0;

            int k = 0;
            k = new BCW.BLL.Text().Add(addmodel);

            //论坛统计
            BCW.User.Users.UpdateForumStat(1, _reqData.userId, mename, _reqData.forumId);
            //动态记录
            if (model.GroupId > 0)
            {
                new BCW.BLL.Action().Add(-2, 0, _reqData.userId, mename, "在圈坛-" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + _reqData.forumId + "&amp;bid=" + k + "]" + _reqData.title + "[/URL]的帖子");
            }
            else
            {
                new BCW.BLL.Action().Add(-1, 0, _reqData.userId, mename, "在" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + _reqData.forumId + "&amp;bid=" + k + "]" + _reqData.title + "[/URL]的帖子");
            }
            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int GroupId = new BCW.BLL.Forum().GetGroupId(_reqData.forumId);
            int IsAcc = -1;
            if (GroupId == 0)
            {
                IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, _reqData.userId, true);
            }
            else
            {
                if (!Utils.GetDomain().Contains("th"))
                    IsAcc = new BCW.User.Cent().UpdateCent2(BCW.User.Cent.enumRole.Cent_Text, _reqData.userId, false);
            }
            #region  这里开始修改提醒ID 发内线
            string remind = ub.GetSub("remindid" + _reqData.forumId, xmlPath);//获取XML的值
            if (remind != "")  //如果有提醒ID
            {
                string[] IDS = remind.Split('#');
                for (int i = 0; i < IDS.Length; i++)
                {
                    if (model.GroupId > 0)
                    {
                        new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + mename + "(" + _reqData.userId + ")[/url]在圈坛-" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + _reqData.forumId + "&amp;bid=" + k + "]" + _reqData.title + "[/URL]的帖子");
                    }
                    else
                    {
                        new BCW.BLL.Guest().Add(0, int.Parse(IDS[i]), new BCW.BLL.User().GetUsName(int.Parse(IDS[i])), "请注意!用户[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + mename + "(" + _reqData.userId + ")[/url]在" + model.Title + "发表了[URL=/bbs/topic.aspx?forumid=" + _reqData.forumId + "&amp;bid=" + k + "]" + _reqData.title + "[/URL]的帖子");
                    }
                }
            }
            #endregion

            _rspAddThread.header.status = ERequestResult.success;
            _rspAddThread.threadId = k;
            return _rspAddThread;

        }

        public RspEditThread EditThread(ReqEditThread _reqData)
        {
            RspEditThread _rspEditThread =new RspEditThread();

            //验证用户ID格式
            if (_reqData.userId < 0)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.MOBILE_PARAMS_ERROR;
                return _rspEditThread;
            }

            //验证贴子标题长度
            if (Regex.IsMatch(_reqData.title, @"^[\s\S]{" + ub.GetSub("BbsThreadMin", xmlPath) + "," + ub.GetSub("BbsThreadMax", xmlPath) + "}$") == false)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_TITLE_LENGTH_ERROR;
                _rspEditThread.header.statusMsg = string.Format(_rspEditThread.header.statusMsg, ub.GetSub("BbsThreadMin", xmlPath), ub.GetSub("BbsThreadMax", xmlPath));
                return _rspEditThread;
            }

            //验证内容长度
            if (Regex.IsMatch(_reqData.content, @"^[\s\S]{" + ub.GetSub("BbsContentMin", xmlPath) + "," + ub.GetSub("BbsContentMax", xmlPath) + "}$") == false)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_CONTENT_LENGTH_ERROR;
                _rspEditThread.header.statusMsg = string.Format(_rspEditThread.header.statusMsg, ub.GetSub("BbsContentMin", xmlPath), ub.GetSub("BbsContentMax", xmlPath));
                return _rspEditThread;
            }

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(_reqData.userId, _reqData.userKey) == 0)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspEditThread;
            }

            BCW.Model.Text model = new BCW.BLL.Text().GetText(_reqData.threadId);
            if (model == null)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspEditThread;
            }

            if (model.UsID != _reqData.userId && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_EditText, _reqData.userId, model.ForumId) == false)
            {                
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                return _rspEditThread;
            }

            if (model.IsLock == 1)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_LOCK;
                return _rspEditThread;
            }
            if (model.IsTop == -1)
            {
                _rspEditThread.header.status = ERequestResult.faild;
                _rspEditThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_TOP;
                return _rspEditThread;
            }

            BCW.Model.Text model2 = new BCW.Model.Text();
            model2.ID = _reqData.threadId;
            model2.Title = _reqData.title;
            model2.Content = _reqData.content;
            new BCW.BLL.Text().Update(model2);

            //记录日志
            string strLog = string.Empty;
            if (model.UsID != _reqData.userId)
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的主题[url=/bbs/topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + _reqData.threadId + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + new BCW.BLL.User().GetUsName(_reqData.userId) + "[/url]编辑!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的主题[url=/bbs/topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + _reqData.threadId + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + _reqData.userId + "]" + new BCW.BLL.User().GetUsName(_reqData.userId) + "[/url]编辑!");
            }
            else
            {
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]编辑自己的主题[url=/bbs/topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + _reqData.threadId + "]《" + model.Title + "》[/url]!";
            }
            new BCW.BLL.Forumlog().Add(7, model.ForumId, _reqData.threadId, strLog);

            _rspEditThread.header.status = ERequestResult.success;
            _rspEditThread.threadId = _reqData.threadId;
            return _rspEditThread;
        }

        public RspDelThread DelThread(ReqDelThread _reqData)
        {
            RspDelThread _rspdelThread = new RspDelThread();

            int uid = _reqData.userId;
            int bid = _reqData.threadId;

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(uid, _reqData.userKey) == 0)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspdelThread;
            }


            BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);//GetTextMe
            if (model == null)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspdelThread;
            }

            int forumid = model.ForumId;
                       
            if (ub.GetSub("BbsThreadDel", xmlPath) == "0")
            {
                if (model.UsID != _reqData.userId && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelText, uid, model.ForumId) == false)
                {
                    _rspdelThread.header.status = ERequestResult.faild;
                    _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspdelThread;
                }
            }
            else
            {
                if (new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelText, uid, forumid) == false)
                {
                    _rspdelThread.header.status = ERequestResult.faild;
                    _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspdelThread;
                }
            }

            if (model.IsGood == 1)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_GOOD;
                return _rspdelThread;
            }
            if (model.IsRecom == 1)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_RECOM;
                return _rspdelThread;
            }
            if (model.IsLock == 1)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_LOCK;
                return _rspdelThread;
            }
            if (model.IsTop == -1)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_TOP;
                return _rspdelThread;
            }
            if (model.ForumId == 1)
            {
                _rspdelThread.header.status = ERequestResult.faild;
                _rspdelThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_DEL_FORBID;
                return _rspdelThread;
            }

            new BCW.BLL.Text().UpdateIsDel(bid, 1);
            new BCW.BLL.Forumstat().Update2(1, model.UsID, forumid, model.AddTime);//更新统计表发帖
            DataSet ds = new BCW.BLL.Reply().GetList("ID,AddTime,UsID,IsDel", "forumid=" + forumid + " and bid=" + bid + "");  //更新统计表回帖
            {
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (int.Parse(ds.Tables[0].Rows[i]["IsDel"].ToString()) == 0)//如果回帖没有删除
                        {
                            new BCW.BLL.Forumstat().Update2(2, int.Parse(ds.Tables[0].Rows[i]["UsID"].ToString()), forumid, DateTime.Parse(ds.Tables[0].Rows[i]["AddTime"].ToString()));
                        }

                    }
                }
            }
            new BCW.BLL.Reply().UpdateIsDel(bid, 1);

            //记录日志
            string strLog = string.Empty;
            if (model.UsID != uid)
            {
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_DelText, model.UsID);
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的主题《" + model.Title + "》被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]删除!";
                new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的主题《" + model.Title + "》被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]删除!");
            }
            else
            {
                //积分操作
                new BCW.User.Cent().UpdateCent(BCW.User.Cent.enumRole.Cent_MeDelText, model.UsID);
                strLog = "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]删除自己的主题《" + model.Title + "》";
            }

            new BCW.BLL.Forumlog().Add(6, forumid, strLog);

            _rspdelThread.header.status = ERequestResult.success;
            return _rspdelThread;
        }

        public RspTopThread SetTopThread(ReqTopThread _reqData)
        {
            RspTopThread _rspTopThread = new RspTopThread();

            int uid = _reqData.userId;
            int bid = _reqData.threadId;

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(uid, _reqData.userKey) == 0)
            {
                _rspTopThread.header.status = ERequestResult.faild;
                _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspTopThread;
            }

            BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);//GetTextMe

            if (model == null)
            {
                _rspTopThread.header.status = ERequestResult.faild;
                _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspTopThread;
            }

            //是否总版权限
            bool IsSuper = new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopText, uid);

            string sText = string.Empty;
            if (_reqData.topType >0)
                sText = "设置";               
            else
                sText = "去掉";            

            //得到置顶类型
            int threadTopType = new BCW.BLL.Text().GetIsTop(bid);

            if (_reqData.topType  == 1 )    //版内置顶
            {
                //是否有置顶权限
                if (IsSuper == false && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_TopText, uid, model.ForumId) == false)
                {
                    _rspTopThread.header.status = ERequestResult.faild;
                    _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspTopThread;
                }

                if (threadTopType > 0)
                {
                    _rspTopThread.header.status = ERequestResult.faild;
                    _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_TOP;
                    return _rspTopThread;
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 1);
            }
            else if (_reqData.topType == 2) //全区置顶
            {
                if (threadTopType == 2)
                {
                    _rspTopThread.header.status = ERequestResult.faild;
                    _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_TOP;
                    return _rspTopThread;
                }
                if (!IsSuper)
                {
                    _rspTopThread.header.status = ERequestResult.faild;
                    _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspTopThread;
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 2);
                sText = "设置全版区";
            }
            else  //取消置顶
            {
                if (threadTopType == 2 && IsSuper == false)
                {
                    _rspTopThread.header.status = ERequestResult.faild;
                    _rspTopThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspTopThread;
                }
                if (_reqData.topType == 2)
                {
                    sText = "去掉全版区";
                }
                new BCW.BLL.Text().UpdateIsTop(bid, 0);

            }

            //加币与扣币(无)

            //记录日志
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "置顶!";
            new BCW.BLL.Forumlog().Add(3, model.ForumId, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);

            _rspTopThread.header.status = ERequestResult.success;
            return _rspTopThread;
        }

        public RspGoodThread SetGoodThread(ReqGoodThread _reqData)
        {
            RspGoodThread _rspGoodThread = new RspGoodThread();

            int uid = _reqData.userId;
            int bid = _reqData.threadId;

            //检查是否登录状态
            if (BCW.Mobile.Common.CheckLogin(uid, _reqData.userKey) == 0)
            {
                _rspGoodThread.header.status = ERequestResult.faild;
                _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.SYS_USER_NOLOGIN;
                return _rspGoodThread;
            }

            BCW.Model.Text model = new BCW.BLL.Text().GetText(bid);//GetTextMe

            if (model == null)
            {
                _rspGoodThread.header.status = ERequestResult.faild;
                _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_NOT_FOUND;
                return _rspGoodThread;
            }

            string sText = string.Empty;
            if (_reqData.goodType == 1)
                sText = "加为";
            else
                sText = "解除";

            int IsGood = new BCW.BLL.Text().GetIsGood(bid);
            if (_reqData.goodType == 1 && IsGood == 1)
            {
                _rspGoodThread.header.status = ERequestResult.faild;
                _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_IS_GOOD;
                return _rspGoodThread;
            }

            //不能操作自己的
            if (uid == model.UsID)
            {
                _rspGoodThread.header.status = ERequestResult.faild;
                _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_THREAD_OPER_MYSELF;
                return _rspGoodThread;
            }


            //积分操作/论坛统计/圈子论坛不进行任何奖励
            int GroupId = new BCW.BLL.Forum().GetGroupId(model.ForumId);
            if (GroupId == 0)
            {
                //检查权限
                if (_reqData.goodType == 1 && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_GoodText, uid, model.ForumId)== false)
                {
                    _rspGoodThread.header.status = ERequestResult.faild;
                    _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspGoodThread;
                }
                else if (_reqData.goodType == 0 && new BCW.User.Role().IsUserRole(BCW.User.Role.enumRole.Role_DelGoodText, uid, model.ForumId) == false)
                {
                    _rspGoodThread.header.status = ERequestResult.faild;
                    _rspGoodThread.header.statusCode = Error.MOBILE_ERROR_CODE.BBS_FORUM_LIMIT_NOT_ENOUGH;
                    return _rspGoodThread;
                }
            }
            string strLog = "主题[url=/bbs/topic.aspx?forumid=" + model.ForumId + "&amp;bid=" + bid + "]《" + model.Title + "》[/url]被[url=/bbs/uinfo.aspx?uid=" + uid + "]" + new BCW.BLL.User().GetUsName(uid) + "[/url]" + sText + "精华!";

            new BCW.BLL.Text().UpdateIsGood(bid, _reqData.goodType);

            new BCW.BLL.Forumlog().Add(1, model.ForumId, bid, "[url=/bbs/uinfo.aspx?uid=" + model.UsID + "]" + model.UsName + "[/url]的" + strLog);
            new BCW.BLL.Guest().Add(0, model.UsID, model.UsName, "您的" + strLog);

            _rspGoodThread.header.status = ERequestResult.success;
            return _rspGoodThread;
        }

    }
 }
