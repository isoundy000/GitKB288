using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile.Home;
using System.Text.RegularExpressions;
using BCW.Common;


namespace BCW.Mobile
{
    public class MobileReg
    {
        public Header header;

        public void Register( string _mobile, string _verifyCode, string _pwd, string _pwdr )
        {
            //检查手机号码是否为空
            if( string.IsNullOrEmpty( _mobile ) )
            {
                header.status = ERequestResult.faild;
                header.statusMsg = MOBILE_ERROR_CODE.MOBILE_PHONE_ISNULL; 
            }

            //检查手机号码是否合法
            if( Regex.IsMatch( _mobile, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$" ) == false )
            {
                header.status = ERequestResult.faild;
                header.statusMsg = MOBILE_ERROR_CODE.MOBILE_PHONE_VERIFY; 
            }

            //检查手机号码是否已注册
            if( new BCW.BLL.User().Exists(_mobile))
            {
                header.status = ERequestResult.faild;
                header.statusMsg = MOBILE_ERROR_CODE.REGEDIT_MOBILE_EXISTS;
            }
              
            //检查手机验证码是否过期
            if( string.IsNullOrEmpty( _verifyCode ) )
            {
                header.status = ERequestResult.faild;
                header.statusMsg = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_EXPIRE;
            }
   
        }

         

        
    }




}
