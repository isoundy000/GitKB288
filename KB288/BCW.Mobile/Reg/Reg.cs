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
        public int userId;

        public MobileReg()
        {
            header = new Header();
        }

        public void Register( string _mobile, string _verifyCode, string _pwd, string _pwdr )
        {
            
            //检查手机号码是否为空
            if( string.IsNullOrEmpty( _mobile ) )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.MOBILE_PHONE_ISNULL; 
                return;
            }


            //检查手机号码是否合法
            if( Regex.IsMatch( _mobile, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$" ) == false )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.MOBILE_PHONE_VERIFY; 
                return;
            }

            //检查手机号码是否已注册
            if( new BCW.BLL.User().Exists( _mobile ) )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_MOBILE_EXISTS;
                return;
            }

            //检查密码是否合法
            if( Regex.IsMatch( _pwd, @"^[A-Za-z0-9]{6,20}$" ) == false )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_PWD_VERIFY;
                return;
            }

            //检查确认密码是否合法
            if( Regex.IsMatch( _pwdr, @"^[A-Za-z0-9]{6,20}$" ) == false )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_PWDR_VERIFY;
                return;
            }

            //两次密码输入是否一致
            if( !_pwd .Equals( _pwdr ) )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_PWD_DIFF;
                return;
            }

              
            //检查手机验证码是否正确
            BCW.Model.tb_Validate _validate = new BCW.BLL.tb_Validate().Gettb_Validate( _mobile, 1 );
            if( _validate == null )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_EXPIRE;
                return;
            }

            //检查手机验证码是否有效
            if( _validate.mesCode != _verifyCode )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_DIFF;
                return;
            }

            //检查手机验证码是否过期
            if( DateTime.Now > _validate.codeTime ||_validate.Flag == 0 )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.REGEDIT_VERIFYCODE_EXPIRE;
                return;
            }

            //取得会员ID
            int maxId = BCW.User.Reg.GetRandId();
            if( maxId == 0 )
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.MOBILE_SYS_BUSY;
                return;
            }

            //加密用户密码
            string strPwd = Utils.MD5Str(_pwd);
            //取随机识别串
            string UsKey = new Rand().RandNum( 10 );

            string newName = ub.GetSub( "RegName", "/Controls/reg.xml" );
            if( newName == "" )
                newName = "新会员";    

            try
            {
                //写入注册表
                BCW.Model.User model = new BCW.Model.User();
                model.ID = maxId;
                model.Mobile = _mobile;
                model.UsName = "" + newName + "" + maxId + "";
                model.UsPwd = strPwd;
                model.UsKey = UsKey;
                model.Photo = "/Files/Avatar/image0.gif";
                model.Sex = 0;
                model.RegTime = DateTime.Now;
                model.RegIP = Utils.GetUsIP();
                model.EndTime = DateTime.Now;
                model.Birth = DateTime.Parse( "1980-1-1" );
                model.Sign = "未设置签名";
                model.InviteNum = 0;
                model.IsVerify = 0;
                model.Email = "";
                new BCW.BLL.User().Add( model );
                //发送内线
                new BCW.BLL.Guest().Add( model.ID, model.UsName, ub.GetSub( "RegGuest", "/Controls/reg.xml" ) );
                //积分操作
                new BCW.User.Cent().UpdateCent( BCW.User.Cent.enumRole.Cent_RegUser, maxId );

                //注册成令验证码失效
                _validate.type = 0;
                new BCW.BLL.tb_Validate().UpdateFlag(0,_validate.ID);

                header.status = ERequestResult.success;
                userId = maxId;
            }
            catch (Exception e)
            {
                header.status = ERequestResult.faild;
                header.statusCode = MOBILE_ERROR_CODE.MOBILE_SYS_ERROR;
            }
        }

         

        
    }




}
