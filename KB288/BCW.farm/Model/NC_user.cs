using System;
namespace BCW.farm.Model
{
    /// <summary>
    /// 实体类NC_user 。(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public class NC_user
    {
        public NC_user()
        { }
        #region Model
        private int _id;
        private int _usid;
        private int _grade;
        private long _goid;
        private long _experience;
        private int _ischan;
        private int _iszhong;
        private int _iscao;
        private int _iswater;
        private int _isinsect;
        private int _isshou;
        private int _isshifei;
        private DateTime _signtime;
        private int _signtotal;
        private int _signkeep;
        private int _tuditpye;
        private int _big_bozhong;
        private int _big_bangmang;
        private int _big_shihuai;
        private DateTime _updatetime;
        private int _isbaitan;
        private int _shoutype;
        private int _big_shifei;
        private int _big_beicaozuo;
        private int _big_zfcishu;
        private int _big_cccishu;
        private int _big_zjcaozuo;
        private int _zengsongnum;
        private string _jiyu;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
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
        /// 等级
        /// </summary>
        public int Grade
        {
            set { _grade = value; }
            get { return _grade; }
        }
        /// <summary>
        /// 金币
        /// </summary>
        public long Goid
        {
            set { _goid = value; }
            get { return _goid; }
        }
        /// <summary>
        /// 经验
        /// </summary>
        public long Experience
        {
            set { _experience = value; }
            get { return _experience; }
        }
        /// <summary>
        /// 一键铲地
        /// </summary>
        public int ischan
        {
            set { _ischan = value; }
            get { return _ischan; }
        }
        /// <summary>
        /// 一键种植
        /// </summary>
        public int iszhong
        {
            set { _iszhong = value; }
            get { return _iszhong; }
        }
        /// <summary>
        /// 一键除草
        /// </summary>
        public int iscao
        {
            set { _iscao = value; }
            get { return _iscao; }
        }
        /// <summary>
        /// 一键浇水
        /// </summary>
        public int iswater
        {
            set { _iswater = value; }
            get { return _iswater; }
        }
        /// <summary>
        /// 一键昆虫
        /// </summary>
        public int isinsect
        {
            set { _isinsect = value; }
            get { return _isinsect; }
        }
        /// <summary>
        /// 一键收获
        /// </summary>
        public int isshou
        {
            set { _isshou = value; }
            get { return _isshou; }
        }
        /// <summary>
        /// 一键施肥
        /// </summary>
        public int isshifei
        {
            set { _isshifei = value; }
            get { return _isshifei; }
        }
        /// <summary>
        /// 签到时间
        /// </summary>
        public DateTime SignTime
        {
            set { _signtime = value; }
            get { return _signtime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignTotal
        {
            set { _signtotal = value; }
            get { return _signtotal; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int SignKeep
        {
            set { _signkeep = value; }
            get { return _signkeep; }
        }
        /// <summary>
        /// 1普通2红3黑4金
        /// </summary>
        public int tuditpye
        {
            set { _tuditpye = value; }
            get { return _tuditpye; }
        }
        /// <summary>
        /// 播种最大经验
        /// </summary>
        public int big_bozhong
        {
            set { _big_bozhong = value; }
            get { return _big_bozhong; }
        }
        /// <summary>
        /// 除草除虫浇水经验
        /// </summary>
        public int big_bangmang
        {
            set { _big_bangmang = value; }
            get { return _big_bangmang; }
        }
        /// <summary>
        /// 种草放虫经验
        /// </summary>
        public int big_shihuai
        {
            set { _big_shihuai = value; }
            get { return _big_shihuai; }
        }
        /// <summary>
        /// 更新农场经验的时间
        /// </summary>
        public DateTime updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 1可以摆摊0不可以
        /// </summary>
        public int isbaitan
        {
            set { _isbaitan = value; }
            get { return _isbaitan; }
        }
        /// <summary>
        /// 收获判断
        /// </summary>
        public int shoutype
        {
            set { _shoutype = value; }
            get { return _shoutype; }
        }
        /// <summary>
        /// 施肥次数
        /// </summary>
        public int big_shifei
        {
            set { _big_shifei = value; }
            get { return _big_shifei; }
        }
        /// <summary>
        /// 自己农场操作最大经验
        /// </summary>
        public int big_beicaozuo
        {
            set { _big_beicaozuo = value; }
            get { return _big_beicaozuo; }
        }
        /// <summary>
        /// 最大种草放虫次数
        /// </summary>
        public int big_zfcishu
        {
            set { _big_zfcishu = value; }
            get { return _big_zfcishu; }
        }
        /// <summary>
        /// 自己最大除草除虫次数
        /// </summary>
        public int big_cccishu
        {
            set { _big_cccishu = value; }
            get { return _big_cccishu; }
        }
        /// <summary>
        /// 自己最大除草除虫次数
        /// </summary>
        public int big_zjcaozuo
        {
            set { _big_zjcaozuo = value; }
            get { return _big_zjcaozuo; }
        }
        /// <summary>
        /// 每天赠送次数
        /// </summary>
        public int zengsongnum
        {
            set { _zengsongnum = value; }
            get { return _zengsongnum; }
        }
        /// <summary>
        /// 农场寄语
        /// </summary>
        public string jiyu
        {
            set { _jiyu = value; }
            get { return _jiyu; }
        }
        #endregion Model

    }
}

