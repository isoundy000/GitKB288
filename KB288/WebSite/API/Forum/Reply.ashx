<%@ WebHandler Language="C#" Class="Signin" %>

using System.Web;
using BCW.Common;
using BCW.Mobile.Protocol;
using BCW.Mobile.BBS.Thread;

public class Signin : IHttpHandler {

    private HttpContext httpContext;

    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        httpContext = context;

        string pAct = Utils.GetRequest( "pAct", "all", 1, "", "" );

        switch( pAct )
        {
            case "list":            //评论列表
                ShowReplyList();
                break;
            case "add":            //发表评论
                AddReply();
                break;
            case "del":            //删除评论
                DelReply();
                break;
        }
    }


    private void ShowReplyList()
    {
        ReqReplyList _reqData = new ReqReplyList();

        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));
        _reqData.showType = int.Parse(Utils.GetRequest("pType", "post", 1, @"^\d*$", "0"));
        _reqData.authorId = int.Parse(Utils.GetRequest("pAuthor", "post", 1, @"^\d*$", "-1"));
        _reqData.replyId = int.Parse(Utils.GetRequest("pReplyId", "post", 1, @"^\d*$", "-1"));

        RspReplyList _rspData = ReplyManager.Instance().GetReplyList(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());

    }


    private void AddReply()
    {
        ReqAddReplyThread _reqData = new ReqAddReplyThread();

        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));

        _reqData.replyContent = Utils.GetRequest("pContent", "post", 0, "", "");
        _reqData.replyId = int.Parse(Utils.GetRequest("pReplyId", "post", 1, @"^\d*$", "0"));
        _reqData.Remind = int.Parse(Utils.GetRequest("pRemind", "post", 1, @"^\d*$", "0"));

        RspAddReplyThread _rspData = ReplyManager.Instance().AddReplyThread(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());

    }

    private void DelReply()
    {
        ReqDelReply _reqData = new ReqDelReply();     
            
        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));
        _reqData.reid = int.Parse(Utils.GetRequest("pFloor", "post", 1, @"^\d*$", "0"));

        RspDelReply _rspData = ReplyManager.Instance().DelReply(_reqData);
        httpContext.Response.Write(_rspData.SerializeObject());
    }


    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

