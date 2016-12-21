using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Manage 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Manage
    {
        public Manage()
        { }
        #region Model
        private int _id;
        private string _suser;
        private string _spwd;
        private string _skeys;
        private DateTime _stime;
        private string _suserip;
        /// <summary>
        /// 管理ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public string sUser
        {
            set { _suser = value; }
            get { return _suser; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string sPwd
        {
            set { _spwd = value; }
            get { return _spwd; }
        }
        /// <summary>
        /// 管理身份码
        /// </summary>
        public string sKeys
        {
            set { _skeys = value; }
            get { return _skeys; }
        }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime sTime
        {
            set { _stime = value; }
            get { return _stime; }
        }
        /// <summary>
        /// 登录IP
        /// </summary>
        public string sUserIP
        {
            set { _suserip = value; }
            get { return _suserip; }
        }
        #endregion Model

    }
}
