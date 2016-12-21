using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类Shopgift 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class Shopgift
    {
        public Shopgift()
        { }
        #region Model
        private int _id;
        private int _nodeid;
        private string _title;
        private string _notes;
        private string _pic;
        private string _prevpic;
        private int _bztype;
        private int _price;
        private int _paycount;
        private int _total;
        private string _para;
        private int _issex;
        private int _isvip;
        private int _isrecom;
        private DateTime _addtime;
        private string _ids;

        /// <summary>
        /// 可视ID列表
        /// </summary>
        public string IDS
        {
            set { _ids = value; }
            get { return _ids; }
        }
        /// <summary>
        /// 自增ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 分类ID
        /// </summary>
        public int NodeId
        {
            set { _nodeid = value; }
            get { return _nodeid; }
        }
        /// <summary>
        /// 礼物名称
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 礼物描述
        /// </summary>
        public string Notes
        {
            set { _notes = value; }
            get { return _notes; }
        }
        /// <summary>
        /// 礼物大图
        /// </summary>
        public string Pic
        {
            set { _pic = value; }
            get { return _pic; }
        }
        /// <summary>
        /// 礼物小图
        /// </summary>
        public string PrevPic
        {
            set { _prevpic = value; }
            get { return _prevpic; }
        }
        /// <summary>
        /// 付款币种
        /// </summary>
        public int BzType
        {
            set { _bztype = value; }
            get { return _bztype; }
        }
        /// <summary>
        /// 金额数
        /// </summary>
        public int Price
        {
            set { _price = value; }
            get { return _price; }
        }
        /// <summary>
        /// 已出售数量
        /// </summary>
        public int PayCount
        {
            set { _paycount = value; }
            get { return _paycount; }
        }
        /// <summary>
        /// 总出售数量(-1代表不限.前台也不会显示剩余数量)
        /// </summary>
        public int Total
        {
            set { _total = value; }
            get { return _total; }
        }
        /// <summary>
        /// 属性参数（积分|体力|魅力|智慧|威望|邪恶)写入如:0|0|0|0|0|0）
        /// </summary>
        public string Para
        {
            set { _para = value; }
            get { return _para; }
        }
        /// <summary>
        /// 是否启用性别限制(0不启用/1女生/2男生)
        /// </summary>
        public int IsSex
        {
            set { _issex = value; }
            get { return _issex; }
        }
        /// <summary>
        /// 是否启用VIP折扣(0不启用/1启用)
        /// </summary>
        public int IsVip
        {
            set { _isvip = value; }
            get { return _isvip; }
        }
        /// <summary>
        /// 是否精品推荐(0不启用/1启用)
        /// </summary>
        public int IsRecom
        {
            set { _isrecom = value; }
            get { return _isrecom; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        #endregion Model

    }
}

