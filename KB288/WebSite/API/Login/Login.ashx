<%@ WebHandler Language="C#" Class="Login" %>

using System;
using System.Web;
using BCW.Common;
using System.Text.RegularExpressions;
using System.Text;

public class Login : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        string Name = context.Request.Form["userId"];   //帐号
        string Pwd = context.Request.Form["pwd"];     //密码

        if (Name != null && Pwd != null)
        {
            bool isname;
            Regex reg = new Regex(@"^\d{1,10}$|^(?:13|14|15|18)\d{9}$");
            if (reg.IsMatch(Name))
                isname = true;
            else
                isname = false;

            bool ispwd;
            Regex regp = new Regex(@"^[A-Za-z0-9]{6,20}$");
            if (regp.IsMatch(Pwd))
                ispwd = true;
            else
                ispwd = false;

            if (isname == true && ispwd == true)//验证账号密码是否符合
            {
                bool IsMy = bool.Parse(Utils.GetRequest("IsMy", "all", 1, @"^True|False$", "False"));
                int State = int.Parse(Utils.GetRequest("State", "all", 1, @"^[0-1]$", "0"));

                int rowsAffected = 0;
                BCW.Model.User modellogin = new BCW.Model.User();
                modellogin.UsPwd = Utils.MD5Str(Pwd);
                if (Name.Length == 11)
                {
                    modellogin.Mobile = Name;
                    rowsAffected = new BCW.BLL.User().GetRowByMobile(modellogin);
                }
                else
                {
                    modellogin.ID = Convert.ToInt32(Name);
                    rowsAffected = new BCW.BLL.User().GetRowByID(modellogin);
                }

                if (rowsAffected > 0)//帐号密码正确
                {
                    BCW.Model.User model = new BCW.BLL.User().GetKey(rowsAffected);


                    int UsId = model.ID;
                    string UsKey = model.UsKey;
                    string UsPwd = model.UsPwd;

                    BCW.Model.User modelgetbasic = new BCW.BLL.User().GetBasic(model.ID);

                    //设置keys
                    string keys = "";
                    keys = BCW.User.Users.SetUserKeys(UsId, UsPwd, UsKey);
                    string bUrl = string.Empty;
                    if (Utils.getPage(1) != "")
                    {
                        bUrl = Utils.getUrl(Utils.removeUVe(Utils.getPage(1)));
                    }
                    else
                    {
                        bUrl = Utils.getUrl("/default.aspx");
                    }
                    //更新识别串
                    string SID = ConfigHelper.GetConfigString("SID");
                    bUrl = UrlOper.UpdateParam(bUrl, SID, keys);

                    //写入Cookie
                    HttpCookie cookie = new HttpCookie("LoginComment");
                    cookie.Expires = DateTime.Now.AddDays(30);//30天
                    cookie.Values.Add("userkeys", DESEncrypt.Encrypt(Utils.Mid(keys, 0, keys.Length - 4)));
                    HttpContext.Current.Response.Cookies.Add(cookie);

                    //----------------------写入登录日志文件作永久保存
                    try
                    {
                        string MyIP = Utils.GetUsIP();
                        string ipCity = string.Empty;
                        if (!string.IsNullOrEmpty(MyIP))
                        {

                            ipCity = new IPSearch().GetAddressWithIP(MyIP);
                            if (!string.IsNullOrEmpty(ipCity))
                            {
                                ipCity = ipCity.Replace("IANA保留地址  CZ88.NET", "本机地址").Replace("CZ88.NET", "") + ":";
                            }

                            string FilePath = System.Web.HttpContext.Current.Server.MapPath("/log/loginip/" + UsId + "_" + DESEncrypt.Encrypt(UsId.ToString(), "kubaoLogenpt") + ".log");
                            LogHelper.Write(FilePath, "" + ipCity + "" + MyIP + "(登录)");
                        }
                    }
                    catch { }
                    //----------------------写入日志文件作永久保存

                    new BCW.BLL.User().UpdateTime(UsId);
                    //APP全部在线登录
                    new BCW.BLL.User().UpdateState(UsId, 0);

                    TimeSpan timediff = DateTime.Now - Convert.ToDateTime("1970-01-01 00:00:00");
                    long stt = (Int64)timediff.TotalMilliseconds;

                    string JsonStatue = LoginStatue(1, modelgetbasic.UsKey, stt.ToString(), "100000", model.ID.ToString(), modelgetbasic.Photo, modelgetbasic.UsName, modelgetbasic.Leven.ToString());//登录成功
                    context.Response.Write(JsonStatue);
                }
                else
                {
                    string JsonStatue = LoginStatue(2, "", "", "", "", "", "", "");//登录失败,帐号或密码错误
                    context.Response.Write(JsonStatue);
                }

            }
            else
            {
                string JsonStatue = LoginStatue(2, "", "", "", "", "", "", "");//登录失败,帐号或密码格式错误
                context.Response.Write(JsonStatue);
            }

        }
        else
        {
            string JsonStatue = LoginStatue(2, "", "", "", "", "", "", "");//登录失败,帐号或密码为空
            context.Response.Write(JsonStatue);
        }

    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    #region 登录判断
    public static string LoginStatue(int i, string a, string b, string c, string d, string e, string f, string g)
    {
        //{"status":"1", msg:"success"}
        if (i == 1)//登录成功
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{" + "");
            jsonBuilder.Append("\"" + "status" + "\"" + ": " + "\"" + "SUCCESS" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "status_msg" + "\"" + ":" + "\"" + "" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "accesstoken" + "\"" + ":" + "\"" + a + "\"");//身份识别字符串u
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "servertime" + "\"" + ":" + "\"" + b + "\"");//当前服务器时间
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "accesstoken_expire" + "\"" + ":" + "\"" + c + "\"");//accesstoken到期时间
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "userInfo" + "\"" + ":");
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "uid" + "\"" + ":" + "\"" + d + "\"");//用户id
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "face" + "\"" + ":" + "\"" + e + "\"");//头像url
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "nickname" + "\"" + ":" + "\"" + f + "\"");//昵称
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "level" + "\"" + ":" + "\"" + g + "\"");//等级数字
            jsonBuilder.Append("}");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        else if (i == 2)//登录失败
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "status" + "\"" + ":" + "\"" + "FAIL" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "status_msg" + "\"" + ":" + "\"" + "账号或密码错误" + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "err_code" + "\"" + ":" + "\"" + "701" + "\"");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
        else
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{");
            jsonBuilder.Append("\"" + "status" + "\"" + ":" + "\"" + 3 + "\"");
            jsonBuilder.Append(",");
            jsonBuilder.Append("\"" + "msg" + "\"" + ":" + "\"" + "error" + "\"");
            jsonBuilder.Append("}");
            return jsonBuilder.ToString();
        }
    }
    #endregion
}