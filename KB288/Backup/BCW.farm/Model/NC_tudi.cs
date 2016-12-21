using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_tudi 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_tudi
    {
        public NC_tudi()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _tudi;
        private int _tudi_type;
        private string _zuowu;
        private int _zuowu_ji;
        private int _iscao;
        private int _iswater;
        private int _isinsect;
        private int _ischandi;
        private string _output;
        private int _zuowu_experience;
        private int _harvest;
        private DateTime _updatetime;
        private int _zuowu_time;
        private int _isshifei;
        private string _touid;
        private int _xianjing;
        private string _caoid;
        private string _chongid;
        private DateTime _z_caotime;
        private DateTime _z_chongtime;
        private DateTime _z_shuitime;
        private int _z_shuinum;
        private int _aa;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        public int aa
        {
            set { _aa = value; }
            get { return _aa; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int usid
        {
            set { _usid = value; }
            get { return _usid; }
        }
        /// <summary>
        /// 土地
        /// </summary>
        public int tudi
        {
            set { _tudi = value; }
            get { return _tudi; }
        }
        /// <summary>
        /// 土地类型
        /// </summary>
        public int tudi_type
        {
            set { _tudi_type = value; }
            get { return _tudi_type; }
        }
        /// <summary>
        /// 作物
        /// </summary>
        public string zuowu
        {
            set { _zuowu = value; }
            get { return _zuowu; }
        }
        /// <summary>
        /// 作物生长的季度
        /// </summary>
        public int zuowu_ji
        {
            set { _zuowu_ji = value; }
            get { return _zuowu_ji; }
        }
        /// <summary>
        /// 除草
        /// </summary>
        public int iscao
        {
            set { _iscao = value; }
            get { return _iscao; }
        }
        /// <summary>
        /// 是否浇水
        /// </summary>
        public int iswater
        {
            set { _iswater = value; }
            get { return _iswater; }
        }
        /// <summary>
        /// 昆虫
        /// </summary>
        public int isinsect
        {
            set { _isinsect = value; }
            get { return _isinsect; }
        }
        /// <summary>
        /// 铲地(0空1有2枯萎)
        /// </summary>
        public int ischandi
        {
            set { _ischandi = value; }
            get { return _ischandi; }
        }
        /// <summary>
        /// 产/剩
        /// </summary>
        public string output
        {
            set { _output = value; }
            get { return _output; }
        }
        /// <summary>
        /// 作物经验
        /// </summary>
        public int zuowu_experience
        {
            set { _zuowu_experience = value; }
            get { return _zuowu_experience; }
        }
        /// <summary>
        /// 收获季度
        /// </summary>
        public int harvest
        {
            set { _harvest = value; }
            get { return _harvest; }
        }
        /// <summary>
        /// 种植时间
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 作物生长时间周长
        /// </summary>
        public int zuowu_time
        {
            set { _zuowu_time = value; }
            get { return _zuowu_time; }
        }
        /// <summary>
        /// 0为该阶段未施肥1为已施
        /// </summary>
        public int isshifei
        {
            set { _isshifei = value; }
            get { return _isshifei; }
        }
        /// <summary>
        /// 偷菜人的ID
        /// </summary>
        public string touID
        {
            set { _touid = value; }
            get { return _touid; }
        }
        /// <summary>
        /// 陷阱(0没有1有)
        /// </summary>
        public int xianjing
        {
            set { _xianjing = value; }
            get { return _xianjing; }
        }
        /// <summary>
        /// 放草的ID
        /// </summary>
        public string caoID
        {
            set { _caoid = value; }
            get { return _caoid; }
        }
        /// <summary>
        /// 放虫的ID
        /// </summary>
        public string chongID
        {
            set { _chongid = value; }
            get { return _chongid; }
        }
        /// <summary>
        /// 刷新机种草时间
        /// </summary>
        public DateTime z_caotime
        {
            set { _z_caotime = value; }
            get { return _z_caotime; }
        }
        /// <summary>
        /// 刷新机放虫时间
        /// </summary>
        public DateTime z_chongtime
        {
            set { _z_chongtime = value; }
            get { return _z_chongtime; }
        }
        /// <summary>
        /// 刷新机缺水时间
        /// </summary>
        public DateTime z_shuitime
        {
            set { _z_shuitime = value; }
            get { return _z_shuitime; }
        }
        /// <summary>
        /// 缺水次数
        /// </summary>
        public int z_shuinum
        {
            set { _z_shuinum = value; }
            get { return _z_shuinum; }
        }
        #endregion Model

    }
}

