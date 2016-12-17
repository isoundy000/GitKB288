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

        }
    }


    private void ShowReplyList()
    {
        ReqReplyList _reqData = new ReqReplyList();

        _reqData.userId = int.Parse(Utils.GetRequest("pUserId", "post", 1, @"^\d*$", "-1"));
        _reqData.userKey = Utils.GetRequest("pUsKey", "post", 0, "", "");
        _reqData.threadId = int.Parse(Utils.GetRequest("pThreadId", "post", 1, @"^\d*$", "-1"));
        _reqData.showType = int.Parse(Utils.GetRequest("pType", "post", 1, @"^\d*$", "-1"));
        _reqData.authorId = int.Parse(Utils.GetRequest("pAuthor", "post", 1, @"^\d*$", "-1"));
        _reqData.replyId = int.Parse(Utils.GetRequest("pReplyId", "post", 1, @"^\d*$", "-1"));

        RspReplyList _rspData = ReplyManager.Instance().GetReplyList(_reqData);
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

