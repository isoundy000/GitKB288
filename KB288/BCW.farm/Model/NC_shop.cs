using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_shop 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_shop
    {
        public NC_shop()
        { }
        #region Model
        private int _id;
        private string _name;
        private int _num_id;
        private int _grade;
        private string _picture;
        private int _jidu;
        private int _jidu_time;
        private long _price_in;
        private long _price_out;
        private int _experience;
        private string _output;
        private int _type;
        private int _iszengsong;
        private int _zhonglei;
        private int _caotime;
        private int _chongtime;
        private int _shuitime;
        private int _meili;
        private int _tili;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 作物名称
        /// </summary>
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 作物ID
        /// </summary>
        public int num_id
        {
            set { _num_id = value; }
            get { return _num_id; }
        }
        /// <summary>
        /// 种植等级
        /// </summary>
        public int grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 照片路径
        /// </summary>
        public string picture
        {
            set { _picture = value; }
            get { return _picture; }
        }
        /// <summary>
        /// 作物季度
        /// </summary>
        public int jidu
        {
            set { _jidu = value; }
            get { return _jidu; }
        }
        /// <summary>
        /// 季度时间
        /// </summary>
        public int jidu_time
        {
            set { _jidu_time = value; }
            get { return _jidu_time; }
        }
        /// <summary>
        /// 买入单价
        /// </summary>
        public long price_in
        {
            set { _price_in = value; }
            get { return _price_in; }
        }
        /// <summary>
        /// 卖出单价
        /// </summary>
        public long price_out
        {
            set { _price_out = value; }
            get { return _price_out; }
        }
        /// <summary>
        /// 作物经验
        /// </summary>
        public int experience
        {
            set { _experience = value; }
            get { return _experience; }
        }
        /// <summary>
        /// 作物产量
        /// </summary>
        public string output
        {
            set { _output = value; }
            get { return _output; }
        }
        /// <summary>
        /// 种子类型(1普2红3黑4金10珍稀)
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 0不可赠送1可以
		/// </summary>
        public int iszengsong
        {
            set { _iszengsong = value; }
            get { return _iszengsong; }
        }
        /// <summary>
        /// 1农作物2水果3花4有机5赠送
        /// </summary>
        public int zhonglei
        {
            set { _zhonglei = value; }
            get { return _zhonglei; }
        }
        /// <summary>
        /// 随机多少分钟出草
        /// </summary>
        public int caotime
        {
            set { _caotime = value; }
            get { return _caotime; }
        }
        /// <summary>
        /// 随机多少分钟出虫
        /// </summary>
        public int chongtime
        {
            set { _chongtime = value; }
            get { return _chongtime; }
        }
        /// <summary>
        /// 随机多少分钟缺水
        /// </summary>
        public int shuitime
        {
            set { _shuitime = value; }
            get { return _shuitime; }
        }
        /// <summary>
        /// 魅力值
        /// </summary>
        public int meili
        {
            set { _meili = value; }
            get { return _meili; }
        }
        /// <summary>
        /// 体力值
        /// </summary>
        public int tili
        {
            set { _tili = value; }
            get { return _tili; }
        }
        #endregion Model

    }
}

