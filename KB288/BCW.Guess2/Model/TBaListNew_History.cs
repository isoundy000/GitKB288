using System;
using System.Collections.Generic;
using System.Text;

namespace TPR2.Model.guess
{
    public class TBaListNew_History
    {
        public TBaListNew_History()
        {

        }

        #region Model
        private int _id;
        private int _source; //来源站点（默认0，预留大赢家1）
        private int _p_id; //赛事ID
        private int _type; //类型（1全场让球盘，2全场大小盘，3全场标准盘；4半场让球盘，5半场大小盘，6半场标准盘；7第一节让球盘，8第一节大小盘；9第二节让球盘，10第二节大小盘；11第三节让球盘，12第三节大小盘）
        private DateTime _downloadtime; //抓取来源数据的时间（抓取程序的电脑上的时间）
        private DateTime? _downloadtime_local; //变动(创建)记录的时间，数据库服务器的时间
        private string _result; //比分（0：0）
        private string _remark; //状态备注（例如：20'、中、完）
        private decimal? _v1;
        private decimal? _vs; //转换
        private decimal? _v2;
        private int? _zdflag; //滚球标志
        private int? _lockflag; //封盘标志
        #endregion
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 来源站点
        /// </summary>
        public int Source
        {
            set { _source = value; }
            get { return _source; }
        }
        /// <summary>
        /// 赛事ID
        /// </summary>
        public int p_id
        {
            set { _p_id = value; }
            get { return _p_id; }
        }
        /// <summary>
        /// 类型（1全场让球盘，2全场大小盘，3全场标准盘；4半场让球盘，5半场大小盘，6半场标准盘）
        /// </summary>
        public int type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 抓取来源数据的时间（抓取程序的电脑上的时间）
        /// </summary>
        public DateTime downloadtime
        {
            set { _downloadtime = value; }
            get { return _downloadtime; }
        }
        /// <summary>
        /// 赔率变动时间，创建记录的时间，数据库服务器的时间
        /// </summary>
        public DateTime? downloadtime_local
        {
            set { _downloadtime_local = value; }
            get { return _downloadtime_local; }
        }
        /// <summary>
        /// 比分
        /// </summary>
        public string result
        {
            set { _result = value; }
            get { return _result; }
        }
        /// <summary>
        /// 状态备注（例如：20'、中、完）
        /// </summary>
        public string remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 主队水位
        /// </summary>
        public decimal? v1
        {
            set { _v1 = value; }
            get { return _v1; }
        }
        /// <summary>
        /// 盘口
        /// </summary>
        public decimal? vs
        {
            set { _vs = value; }
            get { return _vs; }
        }
        /// <summary>
        /// 客队水位
        /// </summary>
        public decimal? v2
        {
            set { _v2 = value; }
            get { return _v2; }
        }
        /// <summary>
        /// 0表示即时赔率水位，1表示走地滚球赔率水位
        /// </summary>
        public int? zdflag
        {
            set { _zdflag = value; }
            get { return _zdflag; }
        }
        /// <summary>
        /// 1表示封盘
        /// </summary>
        public int? lockflag
        {
            set { _lockflag = value; }
            get { return _lockflag; }
        }
    }
}
