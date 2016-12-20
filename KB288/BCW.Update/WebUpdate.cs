using System;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;

namespace BCW.Update
{
    /// <summary>
    /// 更新系统
    /// </summary>
    public class WebUpdate
    {
        public WebUpdate()
        { 
        }

        #region 私有变量

        private string _frompath;
        private string _topath;
        private string _repath;
        private string _remotehost;
        private int _remoteport;
        private string _remoteuser;
        private string _remotepass;
        private string _remotepath;

        #endregion 私有变量

        #region Model

        /// <summary>
        /// FTP目录或文件路径
        /// </summary>
        public string FromPath
        {
            set { _frompath = value; }
            get { return _frompath; }
        }

        /// <summary>
        /// 本地根目录路径
        /// </summary>
        public string ToPath
        {
            set { _topath = value; }
            get { return _topath; }
        }

        /// <summary>
        /// 升级主路径(版本文件夹全路径)
        /// </summary>
        public string RePath
        {
            set { _repath = value; }
            get { return _repath; }
        }

        /// <summary>
        /// FTP服务器
        /// </summary>
        public string RemoteHost
        {
            set { _remotehost = value; }
            get { return _remotehost; }
        }

        /// <summary>
        /// FTP端口
        /// </summary>
        public int RemotePort
        {
            set { _remoteport = value; }
            get { return _remoteport; }
        }

        /// <summary>
        /// FTP用户名
        /// </summary>
        public string RemoteUser
        {
            set { _remoteuser = value; }
            get { return _remoteuser; }
        }

        /// <summary>
        /// FTP密码
        /// </summary>
        public string RemotePass
        {
            set { _remotepass = value; }
            get { return _remotepass; }
        }

        /// <summary>
        /// FTP根路径
        /// </summary>
        public string RemotePath
        {
            set { _remotepath = value; }
            get { return _remotepath; }
        }

        #endregion Model



        public void ftp()
        {
            //StreamWriter sw = new StreamWriter(FromPath, false, System.Text.Encoding.GetEncoding("utf-8"));
            //sw.WriteLine("");
            //sw.Close();
            //System.Threading.Thread th = new System.Threading.Thread(new System.Threading.ThreadStart(PutFile));
            //th.Start();
            GetFile();
        }

        //上传文件
        private void PutFile()
        {
            //initClient initclient = new initClient();
            FTPClient Client = new FTPClient();
            Client.RemoteHost = this._remotehost;
            Client.RemotePass = this._remotepass;
            Client.RemotePath = this._remotepath;
            Client.RemotePort = this._remoteport;
            Client.RemoteUser = this._remoteuser;
            //Client = initclient.GetDefaultClient();
            Client.Connect();
            Client.Put(FromPath, false);
            Client.DisConnect();
            File.Delete(FromPath);
        }
        //下载文件
        private void GetFile()
        {
            //initClient initclient = new initClient();
            FTPClient Client = new FTPClient();
            Client.RemoteHost = this._remotehost;
            Client.RemotePass = this._remotepass;
            Client.RemotePath = this._remotepath;
            Client.RemotePort = this._remoteport;
            Client.RemoteUser = this._remoteuser;
            //Client = initclient.GetDefaultClient();

           // System.Web.HttpContext.Current.Response.Write(this._frompath + "|" + HttpContext.Current.Server.MapPath(this._topath) + "|" + this.RePath);
           // System.Web.HttpContext.Current.Response.End();

            Client.Connect();
            Client.Get(this._frompath, HttpContext.Current.Server.MapPath(this._topath), this.RePath + "", false);
            Client.DisConnect();
        }

        //class initClient
        //{    
        //    FTPClient Client = new FTPClient();
        //    public initClient()
        //    {
        //        //Client.RemoteHost = "203.156.254.4";
        //        //Client.RemotePass = "lujinjie";
        //        //Client.RemotePath = "/Webnowtx/wwwroot/";
        //        //Client.RemotePort = 21;
        //        //Client.RemoteUser = "webnowtx";

        //        Client.RemoteHost = "";
        //        Client.RemotePass = "lujinjie";
        //        Client.RemotePath = "/Webnowtx/wwwroot/";
        //        Client.RemotePort = 21;
        //        Client.RemoteUser = "webnowtx";

        //    }
        //    public FTPClient GetDefaultClient(string f)
        //    {
        //        return this.Client;
        //    }
        //}

        /// <summary>    
        /// 判断是否是版本号格式 0.0.0
        /// </summary>     
        public static bool IsVersion(string str1)
        {
            if (str1 == null) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }

    }
}
