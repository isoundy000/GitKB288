using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using BCW.Update;
using BCW.Update.Model;

public partial class Manage_updatest : System.Web.UI.Page
{
    protected string HttpHost = "http://bichengwei.nowtx.net/update/";//目标域名+目录
    protected static string VE = GetConfigString("VE");
    protected static string SID = GetConfigString("SID");

    /// <summary>
    /// 得到AppSettings中的配置字符串信息
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetConfigString(string key)
    {
        object objModel = ConfigurationManager.AppSettings[key];

        return objModel.ToString();
    }

    /// <summary>
    /// 获取当前应用程序指定CacheKey的Cache值
    /// </summary>
    /// <param name="CacheKey"></param>
    /// <returns></returns>
    public static object GetCache(string CacheKey)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        return objCache[CacheKey];
    }

    /// <summary>
    /// 设置当前应用程序指定CacheKey的Cache值
    /// </summary>
    /// <param name="CacheKey"></param>
    /// <param name="objObject"></param>
    public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
    {
        System.Web.Caching.Cache objCache = HttpRuntime.Cache;
        objCache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        string back = "";
        back = HttpUtils.SendRequest(getLostone88888888888("hVylEGNAhByXIZjvsRIxMgTYTacmubYk0O5ijZPy24d7Jf0cNNyk0t8R3bttAISJ") + "" + Encrypt(GetDomain(), "nowtx.net") + "", "");

        if (back == "error")
        {
            head("网络超时或没有升级权限！");
        }
        if (!GetDomain().Contains(back))
        {
            head("网络超时或没有升级权限！");
        }
        string info = GetRequest("info", "get", 1, "", "");
        int num = int.Parse(GetRequest("num", "get", 1, @"^[0-9]\d*$", "0"));
        string vs = GetRequest("vs", "get", 1, "", "");
        if (!WebUpdate.IsVersion(vs.Replace("v", "")))
        {
            head("版本号错误");
        }

        //后台管理员权限判断(缓存)
        string u = "";
        u = HttpContext.Current.Request["" + SID + ""];
        if (!string.IsNullOrEmpty(u))
        {
            u = Mid(u, 0, u.Length - 4);
        }
        object strU = GetCache("LIGHT-CMSUPDATE");
        if (num == 0)
        {
            if (strU == null || u != strU.ToString())
            {
                Response.Redirect("login.aspx");
                Response.End();
            }
        }
        //----------------------开始获取基本信息---------------------
        UpdateInfo model = null;
        UpdateInfo ftpmodel = null;

        //获取FTP信息
        string GetUrl = "" + HttpHost + "lightBcwUpdate.xml";
        try
        {
            model = new UpdateXML().GetVersionXML(GetUrl);
            string GetUrl2 = "" + HttpHost + model.FtpData;
            ftpmodel = new UpdataFTP().GetFtpXML(GetUrl2);
            if (ftpmodel == null)
            {
                head("不存在的版本记录或网络超时e");
            }
        }
        catch
        {
        }
        if (vs != "v9.9.9")
        {
            //获取该版本信息
            string CacheUpdateXML = "lightcmsUpdataXML" + vs;
            string vsPath = "" + HttpHost + "" + vs + "/" + vs + ".xml";
            try
            {
                model = new UpdateXML().GetUpdateXML(vsPath);
                if (model == null)
                {
                    head("不存在的版本记录或网络超时fe");
                }
            }
            catch
            {

            }
        }
        else
        { 
        //特殊更新
            string CacheUpdateXML = "lightcmsUpdataXML2" + vs;
            string vsPath = "" + HttpHost + "" + back + ".xml";
            try
            {
                model = new UpdateXML().GetUpdateXML(vsPath);
                if (model == null)
                {
                    head("网络超时");
                }
            }
            catch
            {

            }
        
        }
        //----------------------结束获取基本信息---------------------

        string[] sPath = model.Paths.Split("|".ToCharArray());

        if (info != "ok")
        {
            head("正在升级", "本次升级共分" + sPath.Length + "个步骤,大约用时" + model.WithTime + "，请不要刷新本页！<br />正在执行第1个步骤...", getUrl("updatest.aspx?info=ok&amp;act=start&amp;vs=" + vs + ""));
        }
        else
        {
            WebUpdate objftp = new WebUpdate();
            objftp.FromPath = sPath[num];    //文件路径
            if (sPath[num].Contains("{RE}"))
            {
                objftp.FromPath = objftp.FromPath.Replace("{RE}", back);
            }
            if (sPath[num].Contains("{ADMIN}"))
            {
                string AdminPath = GetConfigString("AdminPath");
                objftp.FromPath = objftp.FromPath.Replace("{ADMIN}", AdminPath);
            }
            objftp.ToPath = model.ToPath;   //网站根目录
            objftp.RePath = model.RePath;            //去掉目录
            if (sPath[num].Contains("{RE}"))
            {
                objftp.RePath = back;

            }
            objftp.RemoteHost = ftpmodel.RemoteHost;
            objftp.RemotePort = ftpmodel.RemotePort;
            objftp.RemoteUser = ftpmodel.RemoteUser;
            objftp.RemotePass = ftpmodel.RemotePass;
            objftp.RemotePath = ftpmodel.RemotePath;
            objftp.ftp();
            if (sPath.Length == 1 || sPath.Length == (num + 1))
            {
                //执行SQL语句
                if (!string.IsNullOrEmpty(model.Notes))
                {
                    string[] sqlTemp = model.Notes.Split("|".ToCharArray());
                    for (int i = 0; i < sqlTemp.Length; i++)
                    {
                        try
                        {
                            SqlHelper.ExecuteSql(sqlTemp[i].ToString());
                        }
                        catch { }

                    }
                }
                if (vs != "v9.9.9")
                {
                    //更新版本
                    ub xml = new ub();
                    xml.Reload();
                    xml.ds["SiteVersion"] = model.Version;
                    xml.ds["SiteStatus"] = 0;
                    System.IO.File.WriteAllText(Server.MapPath("~/Controls/wap.xml"), xml.Post(xml.ds), System.Text.Encoding.UTF8);
                    head("升级完成", "恭喜，升级(" + model.Version + ")成功！", getUrl("update.aspx?info=ok"));
                }
                else
                {
                    head("特殊更新", "恭喜，特殊更新成功！", getUrl("update.aspx?info=ok"));
                }
            }
            else
            {
                head("正在升级", "正在执行第" + (num + 2) + "个步骤...", getUrl("updatest.aspx?info=ok&amp;act=start&amp;vs=" + vs + "&amp;num=" + (num + 1) + ""));
            }
        }
    }

    public void head(string p_strNote)
    {
        if (!Isie())
        {
            HttpContext.Current.Response.ContentType = "text/vnd.wap.wml";
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            HttpContext.Current.Response.Write("<!DOCTYPE wml PUBLIC \"-//WAPFORUM//DTD WML 1.1//EN\" \"http://www.wapforum.org/DTD/wml_1.1.xml\">\r\n");
            HttpContext.Current.Response.Write("<wml>\r\n");
            HttpContext.Current.Response.Write("<card id=\"main\" title=\"出错了\">");
            HttpContext.Current.Response.Write("<p align=\"left\">");
            HttpContext.Current.Response.Write("" + p_strNote + "");
            Response.Write(foot());
            Response.End();
        }
        else
        {
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n");
            HttpContext.Current.Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
            HttpContext.Current.Response.Write("<head>\r\n");
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>\r\n");
            HttpContext.Current.Response.Write("<title>出错了</title>\r\n");
            HttpContext.Current.Response.Write("</head>\r\n");
            HttpContext.Current.Response.Write("<body>");
            HttpContext.Current.Response.Write("" + p_strNote + "");
            Response.Write(foot());
            Response.End();
        }
    }

    public void head(string p_strTilte, string p_strNote, string p_strUrl)
    {
        if (!Isie())
        {
            HttpContext.Current.Response.ContentType = "text/vnd.wap.wml";
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            HttpContext.Current.Response.Write("<!DOCTYPE wml PUBLIC \"-//WAPFORUM//DTD WML 1.1//EN\" \"http://www.wapforum.org/DTD/wml_1.1.xml\">\r\n");
            HttpContext.Current.Response.Write("<wml>\r\n");
            HttpContext.Current.Response.Write("<card id=\"main\" title=\"" + p_strTilte + "\" ontimer=\"" + p_strUrl + "\">\r\n<timer value=\"20\"/>");
            HttpContext.Current.Response.Write("<p align=\"left\">");
            HttpContext.Current.Response.Write("" + p_strNote + "");
            Response.Write(foot());
            Response.End();
        }
        else
        {
            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n");
            HttpContext.Current.Response.Write("<!DOCTYPE html PUBLIC \"-//WAPFORUM//DTD XHTML Mobile 1.0//EN\" \"http://www.wapforum.org/DTD/xhtml-mobile10.dtd\">\r\n");
            HttpContext.Current.Response.Write("<html xmlns=\"http://www.w3.org/1999/xhtml\" >\r\n");
            HttpContext.Current.Response.Write("<head>\r\n");
            HttpContext.Current.Response.Write("<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml;charset=UTF-8\"/>\r\n");
            HttpContext.Current.Response.Write("<meta http-equiv=\"refresh\" content=\"2;url=" + p_strUrl + "\"/>\r\n");
            HttpContext.Current.Response.Write("<title>" + p_strTilte + "</title>\r\n");
            HttpContext.Current.Response.Write("</head>\r\n");
            HttpContext.Current.Response.Write("<body>");
            HttpContext.Current.Response.Write("" + p_strNote + "");
            Response.Write(foot());
            Response.End();

        }
    }

    /// <summary>
    /// 输出底部
    /// </summary>
    /// <returns></returns>
    public string foot()
    {
        if (Isie())  //如果是IE
        {
            return "</body>\r\n</html>";
        }
        else
        {
            return "</p></card>\r\n</wml>";
        }
    }

    /// <summary>
    /// 判断是否为IE
    /// </summary>
    /// <param name="ua"></param>
    /// <param name="cr"></param>
    /// <returns></returns>
    public static bool Isie()
    {
        //string ua = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];
        string ua = HttpContext.Current.Request.Browser.Browser;
        if (int.Parse(Left(getstrVe(), 1)) > 1)
        {
            return true;
        }
        else if (!string.IsNullOrEmpty(ua))
        {
            if (ua == "IE" || ua == "Mozilla" || ua == "Firefox" || ua == "NetScape" || ua == "HotJava")
                return true;
        }
        return false;
    }

    /// <summary>
    /// 实现Left()
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Left(string sSource, int iLength)
    {
        if (string.IsNullOrEmpty(sSource))
            return null;

        return sSource.Substring(0, iLength > sSource.Length ? sSource.Length : iLength);
    }

    /// <summary>
    /// 实现Mid()
    /// </summary>
    /// <param name="sSource"></param>
    /// <param name="iStart"></param>
    /// <param name="iLength"></param>
    /// <returns></returns>
    public static string Mid(string sSource, int iStart, int iLength)
    {
        if (string.IsNullOrEmpty(sSource))
            return null;

        int iStartPoint = iStart > sSource.Length ? sSource.Length : iStart;
        return sSource.Substring(iStartPoint, iStartPoint + iLength > sSource.Length ? sSource.Length - iStartPoint : iLength);
    }

    /// <summary>
    /// 超强判断表单
    /// </summary>
    public static string GetRequest(string p_strVal, string p_strType, int p_intPn, string p_strRegex, string p_strzhi)
    {
        //Request取值
        string strVal = "";
        if (p_strType == "get")
        {
            strVal = HttpContext.Current.Request.QueryString[p_strVal];
        }
        else if (p_strType == "post")
        {
            strVal = HttpContext.Current.Request.Form[p_strVal];
        }
        else
        {
            strVal = HttpContext.Current.Request[p_strVal];
        }
        //字符串进行过滤

        //如果允许空值则不通过正则判断
        if (p_intPn == 3 && string.IsNullOrEmpty(strVal))
        {
            return "";
        }
        //正则判断是否合法并作出默认值或者提示
        if ((!string.IsNullOrEmpty(p_strRegex) && !string.IsNullOrEmpty(strVal) && !Regex.IsMatch(strVal, @p_strRegex)) || string.IsNullOrEmpty(strVal))
        {
            return p_strzhi;
        }

        return strVal;

    }

    /// <summary>
    /// 获取ve值
    /// </summary>
    /// <returns></returns>
    public static string getstrVe()
    {
        string ve = HttpContext.Current.Request["" + VE + ""];

        string ua = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"];

        bool Sfie = false;
        if (!string.IsNullOrEmpty(ua))
        {
            if (ua.IndexOf("MSIE") > 0 && ua.IndexOf("Opera") < 0)
                Sfie = true;
        }

        if (string.IsNullOrEmpty(ve))
        {
            if (Sfie)
                ve = "2a";
            else
                ve = "1a";
        }
        else
        {
            //防止重复
            if (ve.IndexOf(",") != -1)
            {
                ve = ve.Split(",".ToCharArray())[0];
            }
        }
        return ve;
    }

    /// <summary>
    /// Url增加u值
    /// </summary>
    /// <param name="p_strVal"></param>
    public static string getUrl(string p_strVal)
    {
        string strVal = "";
        if (string.IsNullOrEmpty(p_strVal))
            return null;

        if ((p_strVal.IndexOf("?") + 1) < p_strVal.Length)
        {
            if (p_strVal.IndexOf("?") > 0)
            {
                if ((p_strVal.IndexOf("&amp;") + 1) < p_strVal.Length)
                {
                    strVal = p_strVal + "&amp;" + VE + "=" + getstrVe() + "&amp;" + SID + "=" + getstrU() + "";

                    strVal = strVal.Replace("&amp;&amp;", "&amp;");
                }
                else
                {
                    strVal = p_strVal + "" + VE + "=" + getstrVe() + "&amp;" + SID + "=" + getstrU() + "";
                }
            }
            else
            {
                strVal = p_strVal + "?" + VE + "=" + getstrVe() + "&amp;" + SID + "=" + getstrU() + "";
            }
        }
        else
        {
            strVal = p_strVal + "" + VE + "=" + getstrVe() + "&amp;" + SID + "=" + getstrU() + "";
        }
        strVal = Regex.Replace(strVal, "%26amp%3b&amp;", "&amp;");
        strVal = Regex.Replace(strVal, "backurl=&amp;", "");

        return strVal;
    }

    /// <summary>
    /// 取得域名
    /// </summary>
    public static string GetDomain()
    {
        string authority = HttpContext.Current.Request.Url.Authority;

        return (authority == null) ? "127.0.0.1" : authority.ToString();
    }

    /// <summary>
    /// 获取u值
    /// </summary>
    /// <returns></returns>
    public static string getstrU()
    {
        string u = "";
        string PageUrl = HttpContext.Current.Request["backurl"];
        if (string.IsNullOrEmpty(PageUrl))
            PageUrl = "";

        PageUrl = HttpContext.Current.Server.UrlDecode(PageUrl.ToLower());


        u = HttpContext.Current.Request["" + SID + ""];
        if (!string.IsNullOrEmpty(u))
        {
            u = Mid(u, 0, u.Length - 4) + new Random().Next(1000, 9999);
        }


        //防止重复
        if (!string.IsNullOrEmpty(u))
        {
            if (u.IndexOf(",") != -1)
            {
                u = u.Split(",".ToCharArray())[0];
            }
        }
        return u;
    }

    private static string getLostone88888888888(string uid)
    {
        string fsfsd = "edgejcsr";
        string jhkjfj = "chengwei";

        try
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(fsfsd);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(jhkjfj);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(uid);
            }
            catch
            {
                return uid;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        catch { return uid; }
    }

    /// <summary> 
    /// 加密数据 
    /// </summary> 
    /// <param name="Text"></param> 
    /// <param name="sKey"></param> 
    /// <returns></returns> 
    public static string Encrypt(string Text, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray;
        inputByteArray = Encoding.Default.GetBytes(Text);
        des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        return ret.ToString();
    }

}
