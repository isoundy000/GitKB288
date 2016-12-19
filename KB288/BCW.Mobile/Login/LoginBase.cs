using System;
using System.Collections.Generic;
using System.Text;

namespace BCW.Mobile.Login
{
    public enum EMobileLoginType
    {
       account,             //账号密码登录 
       weChat,              //微信登录 
       qq,                  //qq
       sinaBlog,                //新浪微博
    }

    #region 基类
    public abstract class LoginBase
    {
        

        protected EMobileLoginType loginType;

        public LoginBase()
        {
            this.Init();
        }

        public abstract void Init();

        public virtual void Login()
        {
            ;
        }
    }
    #endregion           



    #region 第三方平台帐号登录基类
    public class Loginplatform : LoginBase
    {
        protected string assessToken;                       //第三方访问用户标识(唯一的)

        public Loginplatform( string _assessToken )
            : base()
        {
            this.assessToken = _assessToken;
        }

        public override void Login()
        {
            //查询是否绑定关联帐号
            
            
            base.Login();               //执行普通帐号密码登录
        }

        public override void Init()
        {
            ;
        }

    }
    #endregion


    #region 帐号密码登录
    public class LoginAccount : LoginBase
    {
        public override void Init()
        {
            this.loginType = EMobileLoginType.account;          //指定类型
        }
    }
    #endregion


    #region 微信登录
    public class LoginWeChat : Loginplatform
    {
        public LoginWeChat( string _assessToken )
            : base( _assessToken )
        {

        }

        public override void Init()
        {
            this.loginType = EMobileLoginType.weChat;          //指定类型为微信
        }
      
    }
    #endregion

    #region QQ登录
    public class LoginQQ : Loginplatform
    {
        public LoginQQ( string _assessToken )
            : base( _assessToken )
        {

        }

        public override void Init()
        {
            this.loginType = EMobileLoginType.qq;             //指定类型为QQ
        }
    }
    #endregion

    #region 新浪微博登录
    public class LoginSinaBlog : Loginplatform
    {
        public LoginSinaBlog( string _assessToken )
            : base( _assessToken )
        {

        }

        public override void Init()
        {
            this.loginType = EMobileLoginType.sinaBlog;      //指定类型为新浪微博
        }
    }
    #endregion

}
