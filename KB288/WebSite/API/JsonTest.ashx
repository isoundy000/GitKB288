<%@ WebHandler Language="C#" Class="JsonTest" %>

using System;
using System.Web;
using BCW.Mobile.Home;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;


public class JsonTest : IHttpHandler {

    private HomePageInfo homePageInfo;
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        homePageInfo = new HomePageInfo();       
      //  homePageInfo.InitEssencePost();
        string aa = JsonConvert.SerializeObject( homePageInfo );

        context.Response.Write( aa );
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}