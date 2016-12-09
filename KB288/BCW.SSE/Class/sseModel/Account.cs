using System;
using System.Collections.Generic;
using System.Text;
using BCW.Common;
using BCW.farm;



namespace BCW.SSE.Class.sseModel
{
    public abstract class AccountBase
    {
        /// <summary>
        /// 帐户名称
        /// </summary>
        protected string mName; 
        protected int mMeid;

        /// <summary>
        /// 构造函数
        /// </summary>
        public AccountBase()
        {  
            this.mMeid = new BCW.User.Users().GetUsId();

            this.Init();
        }

        /// <summary>
        /// 初始化帐户
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 获取帐户余额
        /// </summary>
        /// <returns></returns>
        public abstract long GetGold();    

        /// <summary>
        /// 设置帐户余额
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public abstract bool UpdateiGold( long _val, string AcText );

        public abstract bool UpdateiGold( int userId, long _val, string AcText );

        /// <summary>
        /// 帐户名称
        /// </summary>
        public string name
        {
            get
            {
                return this.mName;
            }
            set
            {
                this.mName = value;
            }
        }

        
    }


    //酷币帐户
    public class AccountKB : AccountBase
    {

        public override void Init()
        {
            this.name = ub.Get( "SiteBz" ); 
        }

        public override long GetGold()
        {
            return new BCW.BLL.User().GetGold( this.mMeid );
        }

        public override bool UpdateiGold( long _val, string AcText )
        {
            string UsName = new BCW.BLL.User().GetUsName( mMeid );
            new BCW.BLL.User().UpdateiGold( mMeid, UsName, _val, AcText );
            return true;
        }

        public override bool UpdateiGold( int userId, long _val, string AcText )
        {
            string UsName = new BCW.BLL.User().GetUsName( userId );
            new BCW.BLL.User().UpdateiGold( userId, UsName, _val, AcText );
            return true;
        }
    }

    public class AccountJB : AccountBase
    {


        public override void Init()
        {
            this.name = "金币"; 
        }


        public override long GetGold()
        {
            return new BCW.farm.BLL.NC_user().GetGold(mMeid);
        }


        public override bool UpdateiGold( long _val, string AcText )
        {
            AcText += string.Format( "|{0}{1}|结{2}", _val > 0 ? "获得" : "消费", Math.Abs( _val ).ToString( "#0" ) + this.name, ( this.GetGold() + _val ).ToString( "#0" ) + this.name );
            new BCW.farm.BLL.NC_user().UpdateiGold( this.mMeid,new BCW.BLL.User().GetUsName(mMeid),_val,AcText,8 );
            return true;
        }

        public override bool UpdateiGold( int userId, long _val, string AcText )
        {
            AcText += string.Format( "|{0}{1}|结{2}", _val > 0 ? "获得" : "消费", Math.Abs( _val ).ToString( "#0" ) + this.name, ( new BCW.farm.BLL.NC_user().GetGold( userId ) + _val ).ToString( "#0" ) + this.name );
            new BCW.farm.BLL.NC_user().UpdateiGold( userId, new BCW.BLL.User().GetUsName( userId ), _val, AcText, 8 );
            return true;
        }  
        
    }
  
}
