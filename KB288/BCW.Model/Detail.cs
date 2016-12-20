using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Detail 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Detail
    {
        public Detail()
        { }
        #region Model
        private int _id;
        private int _nodeid;
        private int _types;
        private bool _isad;
        private string _title;
        private string _keyword;
        private string _model;
        private string _pics;
        private string _cover;
        private string _content;
        private string _tartext;
        private string _lantext;
        private string _safetext;
        private string _lytext;
        private string _uptext;
        private int _isvisa;
        private string _restats;
        private string _relastip;
        private int _readcount;
        private int _recount;
        private int _bztype;
        private int _cent;
        private string _payid;
        private DateTime _addtime;
        private int _usid;
        private int _hidden;
        private string _clickid;
        /// <summary>
        /// 细节ID
        /// </summary>  
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        ///点击的id 
        /// </summary>
        public string ClickID
        {
            set { _clickid = value; }
            get { return _clickid; }
        }
        /// <summary>
        /// 专题ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// 专题类型
        /// </summary>
        public int Types
        {
            set { _types = value; }
            get { return _types; }
        }
        /// <summary>
        /// 是否广告
        /// </summary>
        public bool IsAd
        {
            set { _isad = value; }
            get { return _isad; }
        }
        /// <summary>
        /// 细节名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 细节关键字
        /// </summary>
        public string KeyWord
        {
            set { _keyword = value; }
            get { return _keyword; }
        }
        /// <summary>
        /// 适用机型
        /// </summary>
        public string Model
        {
            set { _model = value; }
            get { return _model; }
        }
        /// <summary>
        /// 细节截图
        /// </summary>
        public string Pics
        {
            set { _pics = value; }
            get { return _pics; }
        }
        /// <summary>
        /// 封面图片
        /// </summary>
        public string Cover
        {
            set { _cover = value; }
            get { return _cover; }
        }
        /// <summary>
        /// 细节内容
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 资费说明
        /// </summary>
        public string TarText
        {
            set { _tartext = value; }
            get { return _tartext; }
        }
        /// <summary>
        /// 语言说明
        /// </summary>
        public string LanText
        {
            set { _lantext = value; }
            get { return _lantext; }
        }
        /// <summary>
        /// 检查说明
        /// </summary>
        public string SafeText
        {
            set { _safetext = value; }
            get { return _safetext; }
        }
        /// <summary>
        /// 来源说明
        /// </summary>
        public string LyText
        {
            set { _lytext = value; }
            get { return _lytext; }
        }
        /// <summary>
        /// 更新说明
        /// </summary>
        public string UpText
        {
            set { _uptext = value; }
            get { return _uptext; }
        }
        /// <summary>
        /// 是否需签证(1|未知|2|需要|3|不需要)
        /// </summary>
        public int IsVisa
        {
            set { _isvisa = value; }
            get { return _isvisa; }
        }
        /// <summary>
        /// 细节统计
        /// </summary>
        public string ReStats
        {
            set { _restats = value; }
            get { return _restats; }
        }
        /// <summary>
        /// 细节统计最后ip
        /// </summary>
        public string ReLastIP
        {
            set { _relastip = value; }
            get { return _relastip; }
        }
        /// <summary>
        /// 阅读记录数
        /// </summary>
        public int Readcount
        {
            set { _readcount = value; }
            get { return _readcount; }
        }
        /// <summary>
        /// 评论记录数
        /// </summary>
        public int Recount
        {
            set { _recount = value; }
            get { return _recount; }
        }
        /// <summary>
        /// 收费币种
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// 收费币数
        /// </summary>
        public int Cent
        {
            set { _cent = value; }
            get { return _cent; }
        }
        /// <summary>
        /// 已购买的UID
        /// </summary>
        public string PayId
        {
            set { _payid = value; }
            get { return _payid; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 添加用户ID
        /// </summary>
        public int UsID
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public int Hidden
        {
            set { _hidden = value; }
            get { return _hidden; }
        }
        #endregion Model

    }
}

