using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using System.Data;
using BCW.Mobile.Home;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
 

namespace BCW.Mobile
{
    public static class Common
    {
        public static int CheckLogin(int _userId, string _userKey)
        {
            BCW.Model.User _user = new BCW.BLL.User().GetKey(_userId);
            if (_user != null && _user.UsKey == _userKey)
                return _user.ID;
            return 0;
        }

        /// <summary>
        /// 论坛发贴限制
        /// </summary>
        /// <param name="meid"></param>
        /// <param name="Postlt"></param>
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


    }


    public enum ERequestResult                
    {
        success=0,
        faild,
    }

    public enum EPageType
    {
        eHome,
        eBbs
    }



    /// <summary>
    /// 轮播内容. 
    /// </summary>
    public class sliderPicItem
    {
        public string url;
        public string content_type;
        public string[] arryParams; 
    }


    /// <summary>
    /// 轮播对象
    /// </summary>
    public class sliderPic
    {
        private int count;

        [JsonProperty]
        private List<sliderPicItem> items;

        public sliderPic()
        {
            items = new List<sliderPicItem>();
        }

        public void InitData(EPageType type)
        {
            DataSet _ds = new BCW.MobileSlider.BLL.MobileSlider().GetList( "ptype=" + ( int ) type );

            for( int i = 0; i < _ds.Tables[0].Rows.Count; i++ )
            {
                sliderPicItem _item = new sliderPicItem();
                _item.url ="http://"+Utils.GetDomain()+_ds.Tables[ 0 ].Rows[ i ][ "url" ].ToString();
                _item.content_type = _ds.Tables[ 0 ].Rows[ i ][ "contentType" ].ToString();
                _item.arryParams = _ds.Tables[ 0 ].Rows[ i ][ "param" ].ToString().Split( '|' );
                items.Add( _item );
            }
        }
    }

     


 


}
