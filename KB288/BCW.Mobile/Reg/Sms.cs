using System;
using System.Collections.Generic;
using System.Text;
using BCW.Mobile;
using BCW.Common;
using BCW.Mobile.Home;    
using System.Text.RegularExpressions;
using BCW.Mobile.WebReference;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using BCW.Mobile.Error;


namespace BCW.Mobile.SMS
{
    public class SmsData
    {
        public Header header;    //头信息

        [JsonIgnore]
        public string monbile;          //手机号码
        [JsonIgnore]
        public string smsVerifyCode;    //验证码           

        public SmsData()
        {
            header = new Header();
        }
    }

    public class SmsManager
    {
       private static SmsManager instance;
       private string xmlPath = "/Controls/mobileSms.xml";


       public static SmsManager GetInstance()
       {
           if( instance == null )
               instance = new SmsManager();

           return instance;
       }

       public SmsData SendSms(string _mobile)
       {
           SmsData _smsData = new SmsData();  

           //检查手机号码是否为空
           if( string.IsNullOrEmpty( _mobile ) )
           {
               _smsData.header.status = ERequestResult.faild;
               _smsData.header.statusCode = MOBILE_ERROR_CODE.MOBILE_PHONE_ISNULL;
               return _smsData;
           }

           //检查手机号码是否合法
           if( Regex.IsMatch( _mobile, @"^(?:11|12|13|14|15|16|17|18|19)\d{9}$" ) == false )
           {
               _smsData.header.status = ERequestResult.faild;
               _smsData.header.statusCode = MOBILE_ERROR_CODE.MOBILE_PHONE_VERIFY;
               return _smsData;
           }


           char[] character = { '0', '1', '2', '3', '4', '5', '6', '8', '9' };
           string mesCode = string.Empty; //手机验证码
           Random rnd = new Random();
           //生成验证码字符串
           for( int i = 0; i < 4; i++ )
           {
               mesCode += character[ rnd.Next( character.Length ) ];
           }


           int tm = int.Parse( ub.GetSub( "expireTime", xmlPath ) );                  //短信过期时间(分钟)
           int total = int.Parse( ub.GetSub( "dayCount", xmlPath ) );
           int ipCount = int.Parse( ub.GetSub( "IPCount", xmlPath ) );
           int phoneCount = int.Parse( ub.GetSub( "phoneCount", xmlPath ) );
           int msgremain = int.Parse( ub.GetSub( "msgremain", xmlPath ) );
           int callID = int.Parse( ub.GetSub( "callID", xmlPath ) );
           if( new BCW.BLL.tb_Validate().ExistsPhone( _mobile ) )//不是第一次获取短信
           {
               DataSet data = new BCW.BLL.tb_Validate().GetList( " Top 1 *", "Phone=" + _mobile + " order by time desc" );
               DateTime changeTime = Convert.ToDateTime( data.Tables[ 0 ].Rows[ 0 ][ "Time" ].ToString() );
               int changeday = changeTime.DayOfYear;
               if( ( DateTime.Now.DayOfYear - changeday ) >= 1 )//上一条短信不是在当天
               {
                   BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
                   validate.Phone = _mobile;
                   validate.IP = Utils.GetUsIP();
                   validate.Time = DateTime.Now.AddMinutes( 0 );
                   validate.Flag = 1;
                   validate.mesCode = mesCode;
                   validate.codeTime = DateTime.Now.AddMinutes( tm );
                   validate.type = 1;
                   validate.source = 1;
                   Soap57ProviderService MesExt = new Soap57ProviderService();
                   string result = "";
                   result = MesExt.Submit( ub.GetSub( "smsUsid", xmlPath ), ub.GetSub( "smsUsPwd", xmlPath ), ub.GetSub( "smsAccount", xmlPath ), "【" + ub.GetSub( "SiteName", "/Controls/wap.xml" ) + "】亲，您的验证码是:" + mesCode, _mobile );
                   string[] results = result.Split( '#' );
                   if( ( int.Parse( results[ 2 ] ) / 80 ) < msgremain )
                   {
                       new BCW.BLL.Guest().Add( 0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!" );
                   }
                   if( results[ 8 ] == "0" )
                   {
                       new BCW.BLL.tb_Validate().Add( validate );

                       _smsData.header.status = ERequestResult.success;
                       _smsData.monbile = _mobile;
                       _smsData.smsVerifyCode = mesCode;
                       return _smsData;
                   }
               }
               else//当天时间内
               {
                   //获取当天的短信数量
                   DataSet dt2 = new BCW.BLL.tb_Validate().GetList( "*", "Phone=" + _mobile + " and time>='" + DateTime.Now.ToShortDateString() + "' order by time desc" );
                   if( dt2.Tables[ 0 ].Rows.Count >= total )//当天时间内超过特定数
                   {
                       _smsData.header.status = ERequestResult.faild;
                       _smsData.header.statusCode = MOBILE_ERROR_CODE.SMS_FREQUENTLY_TODAY;
                       return _smsData;
                   }
                   DateTime check = DateTime.Now.AddMinutes( -30 );
                   if( check.DayOfYear < DateTime.Now.DayOfYear )
                   {
                       check = Convert.ToDateTime( DateTime.Now.ToShortDateString() );
                   }
                   else
                   {
                       check = DateTime.Now.AddMinutes( -30 );
                   }
                   //获取最近半小时的短信量
                   string str = "Phone=" + _mobile + " and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
                   DataSet dt1 = new BCW.BLL.tb_Validate().GetList( "*", str );
                   if( data.Tables[ 0 ].Rows[ 0 ][ "Flag" ].ToString() == "0" )//最新一条显示当天不能发送了
                   {
                       _smsData.header.status = ERequestResult.faild;
                       _smsData.header.statusCode = MOBILE_ERROR_CODE.SMS_FREQUENTLY_FLAG;
                       return _smsData;
    
                   }
                   string IP = Utils.GetUsIP();
                   //查看限制IP
                   string str1 = "IP= '" + IP + "' and time>='" + check + "' and time <='" + DateTime.Now + "' order by time desc";
                   DataSet dt3 = new BCW.BLL.tb_Validate().GetList( "*", str1 );
                   if( dt3.Tables[ 0 ].Rows.Count >= ipCount )//半小时内超过10条
                   {
                       ////跟新标示
                       //int ID = int.Parse(dt3.Tables[0].Rows[0]["ID"].ToString());
                       //new BCW.BLL.tb_Validate().UpdateFlag(0, ID);
                       _smsData.header.status = ERequestResult.faild;
                       _smsData.header.statusCode = MOBILE_ERROR_CODE.SMS_FREQUENTLY_IP;
                       return _smsData;
                   }
                   if( dt1.Tables[ 0 ].Rows.Count >= phoneCount )//半小时内超过10条
                   {
                       //跟新标示
                       int ID = int.Parse( dt1.Tables[ 0 ].Rows[ 0 ][ "ID" ].ToString() );
                       new BCW.BLL.tb_Validate().UpdateFlag( 0, ID );
                       _smsData.header.status = ERequestResult.faild;
                       _smsData.header.statusCode = MOBILE_ERROR_CODE.SMS_FREQUENTLY_PHONE;
                       return _smsData;
                   }
                   else
                   {
                       BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
                       validate.Phone = _mobile;
                       validate.IP = Utils.GetUsIP();
                       validate.Time = DateTime.Now.AddMinutes( 0 );
                       validate.Flag = 1;
                       validate.mesCode = mesCode;
                       validate.codeTime = DateTime.Now.AddMinutes( tm );
                       validate.type = 1;
                       validate.source = 1;
                       Soap57ProviderService MesExt = new Soap57ProviderService();
                       string result = "";
                       result = MesExt.Submit( ub.GetSub( "smsUsid", xmlPath ), ub.GetSub( "smsUsPwd", xmlPath ), ub.GetSub( "smsAccount", xmlPath ), "【" + ub.GetSub( "SiteName", "/Controls/wap.xml" ) + "】亲，您的验证码是:" + mesCode, _mobile );
                       string[] results = result.Split( '#' );
                       if( ( int.Parse( results[ 2 ] ) / 80 ) < msgremain )
                       {
                           new BCW.BLL.Guest().Add( 0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!" );
                       }
                       if( results[ 8 ] == "0" )
                       {
                           new BCW.BLL.tb_Validate().Add( validate );
                           _smsData.header.status = ERequestResult.success;
                           _smsData.monbile = _mobile;
                           _smsData.smsVerifyCode = mesCode;
                           return _smsData;
                       }
                   }
               }
           }
           else
           {
               BCW.Model.tb_Validate validate = new BCW.Model.tb_Validate();
               validate.Phone = _mobile;
               validate.IP = Utils.GetUsIP();
               validate.Time = DateTime.Now.AddMinutes( 0 );
               validate.Flag = 1;
               validate.mesCode = mesCode;
               validate.codeTime = DateTime.Now.AddMinutes( tm );
               validate.type = 1;
               validate.source = 1;
               Soap57ProviderService MesExt = new Soap57ProviderService();
               string result = "";
               result = MesExt.Submit( ub.GetSub( "smsUsid", xmlPath ), ub.GetSub( "smsUsPwd", xmlPath ), ub.GetSub( "smsAccount", xmlPath ), "【" + ub.GetSub( "SiteName", "/Controls/wap.xml" ) + "】亲，您的验证码是:" + mesCode, _mobile );
               string[] results = result.Split( '#' );
               if( ( int.Parse( results[ 2 ] ) / 80 ) < msgremain )
               {
                   new BCW.BLL.Guest().Add( 0, callID, "", "剩余短信数量低于" + msgremain + "条了，请注意!" );
               }
               if( results[ 8 ] == "0" )
               {
                   new BCW.BLL.tb_Validate().Add( validate );
                   _smsData.header.status = ERequestResult.success;
                   _smsData.monbile = _mobile;
                   _smsData.smsVerifyCode = mesCode;
                   return _smsData;
               }
           }
           return null;    
       }
    }
}
