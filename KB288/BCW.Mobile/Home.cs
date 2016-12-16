using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Error;
using BCW.Mobile.BBS.Thread;

namespace BCW.Mobile.Home
{
    public class Header
    {
        [JsonConverter( typeof( StringEnumConverter ) )]
        public ERequestResult status;

        private MOBILE_ERROR_CODE mStatusCode; 



        public Header()
        {
            clientIP = "http://" + Utils.GetUsIP();
            domainIP = "http://" + Utils.GetDomain();
            mStatusCode = MOBILE_ERROR_CODE.MOBILE_MSG_NONE;
        }

        public MOBILE_ERROR_CODE statusCode
        {
            get
            {
                return mStatusCode;
            }

            set
            {
                mStatusCode = value;
                statusMsg = ErrorCodeManager.Instance().GetErrorMsg( mStatusCode );
            }
        }
              
        public string statusMsg;
        public string clientIP;
        public string domainIP;

    }

    //首页数据
    public class HomePageInfo
    {
        public Header header;
        public bool has_msg;
        public bool is_login;
        public sliderPic slider;          //轮播对象
        public Notices notices;           //公告或喇叭正文
        public EssencePost bests;        //论坛精华贴


        public HomePageInfo()
        {
            header = new Header();   
        }


        //初始化轮播.
        public void InitSlider()
        {
            slider = new sliderPic();
            slider.InitData( EPageType.eHome );
        }

        //初始化喇叭及公告
        public void InitNotice()
        {
            notices = new Notices( ENoticeType.e_all );
            ( ( NoticesAllItem ) notices.noticeData).InitData( 0 );
        }

        //初始化精华贴
        public void InitEssencePost(int _threadId)
        {
            bests = new EssencePost();
            bests.InitData( 0, _threadId, 1,-1 );
        }
  
        

    }
}
