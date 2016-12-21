using System;
namespace BCW.Update.Model
{
    /// <summary>
    /// 升级实体类 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class UpdateInfo
    {
        public UpdateInfo()
        { }
        #region Model
        private string _version;
        private string _spdomain;
        private string _ftpdata;
        private string _topath;
        private string _repath;
        private string _paths;
        private string _notes;
        private string _withtime;
        private string _addtime;
        private string _remotehost;
        private int _remoteport;
        private string _remoteuser;
        private string _remotepass;
        private string _remotepath;
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }

        /// <summary>
        /// 特殊更新的网站
        /// </summary>
        public string SpDomain
        {
            set { _spdomain = value; }
            get { return _spdomain; }
        }

        /// <summary>
        /// FTP信息
        /// </summary>
        public string FtpData
        {
            set { _ftpdata = value; }
            get { return _ftpdata; }
        }

        /// <summary>
        /// 升级到某目录
        /// </summary>
        public string ToPath
        {
            set { _topath = value; }
            get { return _topath; }
        }

        /// <summary>
        /// 去掉文件夹名
        /// </summary>
        public string RePath
        {
            set { _repath = value; }
            get { return _repath; }
        }

        /// <summary>
        /// 升级全路径(多个路径用|分开)
        /// </summary>
        public string Paths
        {
            set { _paths = value; }
            get { return _paths; }
        }

        /// <summary>
        /// SQL语句内容
        /// </summary>
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }

        /// <summary>
        /// 升级大约用时
        /// </summary>
        public string WithTime
        {
            set { _withtime = value; }
            get { return _withtime; }
        }

        /// <summary>
        /// 发布版本时间
        /// </summary>
        public string AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
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

    }
}