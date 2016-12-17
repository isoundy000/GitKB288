using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;

namespace BCW.Mobile.Home
{
    public class Header:HomeBaseInfo
    {
        public ERequestResult status;
        public string statusMsg;
        public string clientIP;
        public string domainIP;

        public Header()
        {
            clientIP = "http://" + Utils.GetUsIP();
            domainIP = "http://" + Utils.GetDomain();
        }


        public override string OutPutJsonStr()
        {
           jsonBuilder.Append("\"header\":");
           jsonBuilder.Append( "{" );
           jsonBuilder.Append( string.Format( "\"status\":{0},", this.status == ERequestResult.eSuccess ? "\"success\"" : "\"faild\"" ));
           jsonBuilder.Append( string.Format( "\"statusMsg\":{0},", "\""+this.statusMsg+"\"" ) );
           jsonBuilder.Append( string.Format( "\"clientIP\":{0},", "\"" + this.clientIP + "\"" ) );
           jsonBuilder.Append( string.Format( "\"domainIP\":{0}", "\"" + this.domainIP + "\"" ) );
           jsonBuilder.Append( "}" );
           return jsonBuilder.ToString(); 
        }
    }

    /// <summary>
    /// 基础类
    /// </summary>
    public abstract class HomeBaseInfo
    {    
        public StringBuilder jsonBuilder;

        public HomeBaseInfo()
        {
            jsonBuilder = new StringBuilder();
        }

        public abstract string OutPutJsonStr();

        public virtual string OutMessage( string _str )
        {
            return "{\"message:\"" + _str + "}";
        }
    }

    //首页数据
    public class HomePageInfo : HomeBaseInfo
    {
        public Header header;
        public bool has_msg;
        public bool is_login;
        public sliderPic sliderPic;          //轮播对象
        public Notices notices;            //公告或喇叭正文
        public EssencePost essencePost;    //论坛精华贴


        public HomePageInfo()
        {
            header = new Header();   
        }


        //初始化轮播.
        public void InitSlider()
        {
            sliderPic = new sliderPic();
            sliderPic.InitData( EPageType.eHome );
        }

        //初始化喇叭及公告
        public void InitNotice()
        {
            notices = new Notices( ENoticeType.e_all );
            ( ( NoticesAllItem ) notices.noticesItem ).InitData( 0 );
        }

        //初始化精华贴
        public void InitEssencePost()
        {
            essencePost = new EssencePost();
            essencePost.InitData( 0, -1, 1 );
        }


        public override string OutPutJsonStr()
        {

            jsonBuilder.Append( "{" );
            jsonBuilder.Append( header.OutPutJsonStr()+",");            //输出头.
            if( header.status == ERequestResult.eSuccess )
            {
                jsonBuilder.Append( string.Format( "\"has_msg\":{0},", has_msg == true ? "true" : "false" ) );
                jsonBuilder.Append( string.Format( "\"is_login\":{0}", is_login == true ? "true" : "false" ) );

                if( sliderPic !=null)
                     jsonBuilder.Append( ","+ sliderPic.OutPutJsonStr()  );
                if (notices!= null)
                     jsonBuilder.Append( ","+ notices.OutPutJsonStr() );
                if (essencePost!=null)
                    jsonBuilder.Append( ","+ essencePost.OutPutJsonStr() );
            }
            jsonBuilder.Append( "}" );

            return jsonBuilder.ToString();
        }
    }
}
