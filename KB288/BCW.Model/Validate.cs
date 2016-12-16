using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类tb_Validate 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class tb_Validate
    {
        public tb_Validate()
        { }
        #region Model
        private int _id;
        private string _phone;
        private string _ip;
        private DateTime _time;
        private int _flag;
        private DateTime _codetime;
        private string _mescode;
        private int _type;
        private int _source;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Phone
        {
            set { _phone = value; }
            get { return _phone; }
        }
        /// <summary>
        /// IP地址
        /// </summary>
        public string IP
        {
            set { _ip = value; }
            get { return _ip; }
        }
        /// <summary>
        /// 获取手机验证码时间
        /// </summary>
        public DateTime Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 能否获取标示 1可 0不可
        /// </summary>
        public int Flag
        {
            set { _flag = value; }
            get { return _flag; }
        }
        /// <summary>
        /// 验证码过期时间
        /// </summary>
        public DateTime codeTime
        {
            set { _codetime = value; }
            get { return _codetime; }
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string mesCode
        {
            set { _mescode = value; }
            get { return _mescode; }
        }
        /// <summary>
        /// 短信类型   1 注册，2 修改密码 3 修改密保 4修改手机 5 忘记密码
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        /// 获取来源   0  PC端   1 手机端
        /// </summary>
        public int source
       {
            set { _source = value; }
            get { return _source; }
        }
        #endregion Model

    }
}

