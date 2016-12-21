using System;
namespace BCW.Model
{
    /// <summary>
    /// 实体类tb_BasketBallList 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class tb_BasketBallList
    {
        public tb_BasketBallList()
        { }
        #region Model
        private int _id;
        private int _name_en;
        private string _classtype;
        private DateTime _matchtime;
        private string _matchstate;
        private DateTime _remaintime;
        private int _hometeamid;
        private string _hometeam;
        private int _guestteamid;
        private string _guestteam;
        private int _homescore;
        private int _guestscore;
        private string _homeone;
        private string _guestone;
        private string _hometwo;
        private string _guesttwo;
        private string _homethree;
        private string _guestthree;
        private string _homefour;
        private string _guestfour;
        private DateTime _addtime;
        private string _addtechnic;
        private string _explain;
        private string _explain2;
        private string _contentlist;
        private int _ishidden;
        private int _connectid;
        private string _isdone;
        private string _result;
        private string _tv;
        private string _homeeurope;
        private string _guesteurope;
        private string _team1;
        private string _team2;
        /// <summary>
        /// ID
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 编号
        /// </summary>
        public int name_en
        {
            set { _name_en = value; }
            get { return _name_en; }
        }
        /// <summary>
        /// 类型
        /// </summary>
        public string classType
        {
            set { _classtype = value; }
            get { return _classtype; }
        }
        /// <summary>
        /// 比赛时间
        /// </summary>
        public DateTime matchtime
        {
            set { _matchtime = value; }
            get { return _matchtime; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public string matchstate
        {
            set { _matchstate = value; }
            get { return _matchstate; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime remaintime
        {
            set { _remaintime = value; }
            get { return _remaintime; }
        }
        /// <summary>
        /// 主队id
        /// </summary>
        public int hometeamID
        {
            set { _hometeamid = value; }
            get { return _hometeamid; }
        }
        /// <summary>
        /// 主队
        /// </summary>
        public string hometeam
        {
            set { _hometeam = value; }
            get { return _hometeam; }
        }
        /// <summary>
        /// 客队id
        /// </summary>
        public int guestteamID
        {
            set { _guestteamid = value; }
            get { return _guestteamid; }
        }
        /// <summary>
        /// 客队
        /// </summary>
        public string guestteam
        {
            set { _guestteam = value; }
            get { return _guestteam; }
        }
        /// <summary>
        /// 主队比分
        /// </summary>
        public int homescore
        {
            set { _homescore = value; }
            get { return _homescore; }
        }
        /// <summary>
        /// 客队
        /// </summary>
        public int guestscore
        {
            set { _guestscore = value; }
            get { return _guestscore; }
        }
        /// <summary>
        /// 主队第一节
        /// </summary>
        public string homeone
        {
            set { _homeone = value; }
            get { return _homeone; }
        }
        /// <summary>
        /// 1
        /// </summary>
        public string guestone
        {
            set { _guestone = value; }
            get { return _guestone; }
        }
        /// <summary>
        /// 2
        /// </summary>
        public string hometwo
        {
            set { _hometwo = value; }
            get { return _hometwo; }
        }
        /// <summary>
        /// 2
        /// </summary>
        public string guesttwo
        {
            set { _guesttwo = value; }
            get { return _guesttwo; }
        }
        /// <summary>
        /// 3
        /// </summary>
        public string homethree
        {
            set { _homethree = value; }
            get { return _homethree; }
        }
        /// <summary>
        /// 3
        /// </summary>
        public string guestthree
        {
            set { _guestthree = value; }
            get { return _guestthree; }
        }
        /// <summary>
        /// 4
        /// </summary>
        public string homefour
        {
            set { _homefour = value; }
            get { return _homefour; }
        }
        /// <summary>
        /// 4
        /// </summary>
        public string guestfour
        {
            set { _guestfour = value; }
            get { return _guestfour; }
        }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime addtime
        {
            set { _addtime = value; }
            get { return _addtime; }
        }
        /// <summary>
        /// 颜色
        /// </summary>
        public string addTechnic
        {
            set { _addtechnic = value; }
            get { return _addtechnic; }
        }
        /// <summary>
        /// 主队助攻
        /// </summary>
        public string explain
        {
            set { _explain = value; }
            get { return _explain; }
        }
        /// <summary>
        /// 客队助攻
        /// </summary>
        public string explain2
        {
            set { _explain2 = value; }
            get { return _explain2; }
        }
        /// <summary>
        /// 直播
        /// </summary>
        public string contentList
        {
            set { _contentlist = value; }
            get { return _contentlist; }
        }
        /// <summary>
        /// 隐藏开关
        /// </summary>
        public int isHidden
        {
            set { _ishidden = value; }
            get { return _ishidden; }
        }
        /// <summary>
        /// 关联id
        /// </summary>
        public int connectId
        {
            set { _connectid = value; }
            get { return _connectid; }
        }
        /// <summary>
        /// 剩余时间
        /// </summary>
        public string isDone
        {
            set { _isdone = value; }
            get { return _isdone; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string result
        {
            set { _result = value; }
            get { return _result; }
        }
        /// <summary>
        /// tv直播地址
        /// </summary>
        public string tv
        {
            set { _tv = value; }
            get { return _tv; }
        }
        /// <summary>
        /// 欧赔
        /// </summary>
        public string homeEurope
        {
            set { _homeeurope = value; }
            get { return _homeeurope; }
        }
        /// <summary>
        /// 欧赔
        /// </summary>
        public string guestEurope
        {
            set { _guesteurope = value; }
            get { return _guesteurope; }
        }
        /// <summary>
        /// 主队数据1
        /// </summary>
        public string team1
        {
            set { _team1 = value; }
            get { return _team1; }
        }
        /// <summary>
        /// 客队数据2
        /// </summary>
        public string team2
        {
            set { _team2 = value; }
            get { return _team2; }
        }
        #endregion Model

    }
}

